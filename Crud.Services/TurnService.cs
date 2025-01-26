using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Dtos.Turn;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
namespace MisCanchas.Services
{
    public class TurnService : ITurnService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;
        private readonly IFieldService _fieldService;
        private readonly IMovementService _movementService;
        private readonly IMapper _mapper;

        public TurnService(MisCanchasDbContext misCanchasDbContext, IFieldService fieldService, IMovementService movementService, IMapper mapper)
        {
            this.misCanchasDbContext = misCanchasDbContext;
            this._fieldService = fieldService;
            this._movementService = movementService;
            _mapper = mapper;
        }
        public async Task<int> Create(TurnCreateDto dto, bool saveChanges = false)
        {
            dto.TurnDateTime = dto.TurnDateTime.AddMinutes(-dto.TurnDateTime.Minute);
            var turn = _mapper.Map<Turn>(dto);

            // Validaciones
            ValidateTurn(turn);

            var turnId = await Create(turn, false);

            // registra movimiento
            await GenerateMovement(turn);

            if (saveChanges) await misCanchasDbContext.SaveChangesAsync();
            return turnId;
        }

        public async Task Update(TurnUpdateDto dto, bool saveChanges = false)
        {
            var turn = _mapper.Map<Turn>(dto);

            await Update(turn, false);

            if (saveChanges) await misCanchasDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id, bool saveChanges = false)
        {
            var turn = await misCanchasDbContext.Turns.FindAsync(id);
            if (turn != null)
            {
                if (turn.TurnDateTime < DateTime.Now)
                {
                    throw new CustomTurnException("TurnDateTime", "El turno caducó, por lo tanto no puede ser eliminado.");
                }
                if (turn.Paid)
                {
                    await DiscountMovement(turn);
                }
                misCanchasDbContext.Turns.Remove(turn);
                if (saveChanges) await misCanchasDbContext.SaveChangesAsync();
            }
        }
        public async Task<Turn> Get(int id)
        {
            var turn = await misCanchasDbContext.Turns.FirstOrDefaultAsync(t => t.TurnId == id);
            return turn;
        }

        public async Task<IQueryable<Turn>> GetByDateRange(DateTime startDateTime, DateTime endDateTime)
        {
            var turns = await misCanchasDbContext.Turns.Where(t => t.TurnDateTime >= startDateTime && t.TurnDateTime <= endDateTime).ToListAsync();
            var turnsq = turns.AsQueryable();
            return turnsq;
        }

        public async Task<IQueryable<Turn>> GetSingleTurnByDate(DateTime dateTime)
        {
            dateTime = dateTime.AddHours(-3); //UTC-3
            var turn = await misCanchasDbContext.Turns.Where(t => t.TurnDateTime.Date == dateTime.Date && t.TurnDateTime.Hour == dateTime.Hour).ToListAsync();
            var turnsq = turn.AsQueryable();
            return turnsq;
        }

        public async Task<IQueryable<Turn>> GetTurns()
        {
            var allTurns = await misCanchasDbContext.Turns.ToListAsync();
            var turnsq = allTurns.AsQueryable();
            return turnsq;
        }

        internal async Task<int> Create(Turn turn, bool saveChanges = false)
        {
            await misCanchasDbContext.AddAsync(turn);
            if (saveChanges) await misCanchasDbContext.SaveChangesAsync();
            return turn.TurnId;
        }

        internal async Task Update(Turn turn, bool saveChanges = false)
        {
            Turn? originalTurn = await misCanchasDbContext.Turns.FindAsync(turn.TurnId);

            if (originalTurn == null) throw new ArgumentException("Turn not found");

            //valdiaciones
            TurnIsExpired(turn, originalTurn);
            TurnIsPast(turn, originalTurn);
            TurnDuplicateValidator(turn);
            DateTimeRangeValidator(turn);
            TurnIsPaid(turn, originalTurn);

            originalTurn.TurnId = turn.TurnId;
            originalTurn.TurnDateTime = turn.TurnDateTime;
            originalTurn.ClientId = turn.ClientId;
            originalTurn.Paid = turn.Paid;
            originalTurn.Price = turn.Price;

            //registrar movimiento
            await GenerateMovement(originalTurn);

            misCanchasDbContext.Turns.Update(originalTurn);
            if (saveChanges) await misCanchasDbContext.SaveChangesAsync();
        }

        internal void ValidateTurn(Turn turn)
        {
            TurnIsPast(turn);
            TurnDuplicateValidator(turn);
            DateTimeRangeValidator(turn);
        }

        //validaciones
        private void TurnIsPast(Turn turn, Turn? turnq = null)
        {
            if (turnq != null)
            {
                if (turn.TurnDateTime < DateTime.Now && turnq.TurnDateTime != turn.TurnDateTime) //valida si es el mismo turno en casos como registro de pago
                {
                    throw new CustomTurnException("TurnDateTime", "La fecha y hora debe ser posterior a la actual.");
                }
            }
            else
            {
                if (turn.TurnDateTime < DateTime.Now)
                {
                    throw new CustomTurnException("TurnDateTime", "La fecha y hora debe ser posterior a la actual.");
                }
            }
        }
        private void TurnDuplicateValidator(Turn turn)
        {
            var turns = GetTurns();
            var turnDuplicate = turns.Result.FirstOrDefault(t => t.TurnDateTime == turn.TurnDateTime);
            if (turnDuplicate != null && turnDuplicate.TurnId != turn.TurnId) // valida si ya existe PERO si es el mismo turno lo sobreescribe
            {
                throw new CustomTurnException("TurnDateTime", "El turno ya fue reservado.");
            }
        }
        private void DateTimeRangeValidator(Turn turn)
        {
            int openHour = _fieldService.Get().Result.OpenHour;
            int closeHour = _fieldService.Get().Result.CloseHour;

            if (turn.TurnDateTime.Hour < openHour && turn.TurnDateTime.Hour >= closeHour)
            {
                throw new CustomTurnException("TurnDateTime", $"El turno {turn.TurnDateTime.ToShortTimeString()} debe ser seleccionado en un horario disponible entre las {openHour}:00 y las {closeHour - 1}:00.");
            }
        }
        private void TurnIsExpired(Turn turn, Turn? turnq = null)
        {
            if (turnq != null)
            {
                if (turnq.TurnDateTime < DateTime.Now && turnq.Paid)
                {
                    throw new CustomTurnException("TurnDateTime", "El turno caducó, y ya se registró su pago.");
                }
                if (turnq.TurnDateTime < DateTime.Now)
                {
                    if (turnq.TurnDateTime != turn.TurnDateTime)
                    {
                        throw new CustomTurnException("TurnDateTime", "El turno caducó y no es posible cambiar la fecha, solo es posible registrar su pago.");
                    }
                }
            }
        }
        private void TurnIsPaid(Turn turn, Turn? turnq = null)
        {
            if (turnq.Paid == true && turn.Paid == false)
            {
                throw new CustomTurnException("Paid", "El turno ya fue pagado.");
            }
        }

        //registro de movimiento - Reserva y cancelacion
        private async Task GenerateMovement(Turn turn)
        {
            if (turn.Paid)
            {
                var movementType = _movementService.GetTypes().Result
                    .Where(x => x.Name.Contains("turno"))
                    .Where(x => x.Incremental == true)
                    .FirstOrDefault();
                if (movementType == null)
                {
                    await _movementService.AddType(new MovementType() { Name = "Pago de turno", Incremental = true });
                    movementType = _movementService.GetTypes().Result.Where(x => x.Name.Contains("turno")).FirstOrDefault();
                }


                var movement = new Movement
                {
                    Amount = turn.Price,
                    DateTime = DateTime.Now,
                    Name = "Pago de reserva",
                    Description = $"reserva {turn.TurnDateTime.ToShortDateString()}, {turn.TurnDateTime.ToShortTimeString()}",
                    MovementTypeId = movementType.Id

                };
                await _movementService.Add(movement);
            }
        }
        private async Task DiscountMovement(Turn turn)
        {
            if (turn.Paid)
            {
                var movementType = _movementService.GetTypes().Result
                    .Where(x => x.Name.Contains("turno"))
                    .Where(x => x.Incremental == false)
                    .FirstOrDefault();
                if (movementType == null)
                {
                    await _movementService.AddType(new MovementType() { Name = "Cancelacion de turno", Incremental = false });
                    movementType = _movementService.GetTypes().Result
                        .Where(x => x.Name.Contains("turno"))
                        .Where(x => x.Incremental == false)
                        .FirstOrDefault();
                }


                var movement = new Movement
                {
                    Amount = turn.Price,
                    DateTime = DateTime.Now,
                    Name = "Cancelación de reserva",
                    Description = $"reserva {turn.TurnDateTime.ToShortDateString()}, {turn.TurnDateTime.ToShortTimeString()}",
                    MovementTypeId = movementType.Id

                };
                await _movementService.Add(movement);
            }
        }

        //excepciones
        public class CustomTurnException : Exception
        {
            public string PropertyName { get; }
            public CustomTurnException(string propertyName, string message) : base(message)
            {
                PropertyName = propertyName;
            }
        }

    }
}
