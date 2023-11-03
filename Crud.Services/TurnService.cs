using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Services
{
    public class TurnService : ITurnService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;
        private readonly IFieldService _fieldService;
		private readonly IMovementService _movementService;

		public TurnService(MisCanchasDbContext misCanchasDbContext, IFieldService fieldService, IMovementService movementService)
        {
            this.misCanchasDbContext = misCanchasDbContext;
            this._fieldService = fieldService;
			this._movementService = movementService;
		}
        public async Task Add(DateTime dateTime, int id, decimal price, bool paid)
        {
            var dateTimeNormalized = dateTime.AddMinutes(-dateTime.Minute);
            var turn = new Turn()
            {
                TurnDateTime = dateTimeNormalized,
                ClientId = id,
                Price = price,
                Paid = paid
            };

            // Validación si el turno es pasado
            if (turn.TurnDateTime < DateTime.Now)
            {
                throw new CustomTurnException("TurnDateTime" ,"La fecha y hora debe ser posterior a la actual.");
            }
            // Validación si el turno es duplicado
            var turns = await GetTurns();
            var turnDuplicate = turns.FirstOrDefault(t => t.TurnDateTime == turn.TurnDateTime);
            if (turnDuplicate != null)
            {
                throw new CustomTurnException("TurnDateTime", "El turno ya fue reservado.");
            }
            // Validación de turno seleccionado entre los horarios definidos
            int openHour = _fieldService.Get().Result.OpenHour;
            int closeHour = _fieldService.Get().Result.CloseHour;
            if (turn.TurnDateTime.Hour < openHour && turn.TurnDateTime.Hour > closeHour)
            {
                throw new CustomTurnException("TurnDateTime" ,$"El turno {turn.TurnDateTime} debe ser seleccionado en un horario disponible entre las {openHour} y las {closeHour}.");
            }
            await misCanchasDbContext.AddAsync(turn);

            // registra movimiento
            await GenerateMovement(turn);

            await misCanchasDbContext.SaveChangesAsync();
        }

        public async Task Update(Turn turn)
        {
            var turnq = await misCanchasDbContext.Turns.FindAsync(turn.TurnId);

            //valdiación de turno vencido (fecha pasada)
            if (turnq.TurnDateTime < DateTime.Now && turnq.Paid)
            {
                throw new CustomTurnException("TurnDateTime", "El turno caducó, y ya se registró su pago.");
            }
            if(turnq.TurnDateTime < DateTime.Now)
            {
                if (turnq.TurnDateTime != turn.TurnDateTime)
                {
				    throw new CustomTurnException("TurnDateTime", "El turno caducó y no es posible cambiar la fecha, solo es posible registrar su pago.");
                }
            }

            // Validación si el turno es pasado
            if (turn.TurnDateTime < DateTime.Now && turnq.TurnDateTime != turn.TurnDateTime)
            {
                throw new CustomTurnException("TurnDateTime", "La fecha y hora debe ser posterior a la actual.");
            }
            // Validación si el turno es duplicado
            var turns = await GetTurns();
            var turnDuplicate = turns.FirstOrDefault(t => t.TurnDateTime == turn.TurnDateTime);
            if (turnDuplicate != null && turnDuplicate.TurnId != turn.TurnId) // valida si ya existe PERO si es el mismo turno lo sobreescribe
            {
                throw new CustomTurnException("TurnDateTime", "El turno ya fue reservado.");
            }
            // Validación de turno seleccionado entre los horarios definidos
            int openHour = _fieldService.Get().Result.OpenHour;
            int closeHour = _fieldService.Get().Result.CloseHour;
            if (turn.TurnDateTime.Hour < openHour && turn.TurnDateTime.Hour > closeHour)
            {
                throw new CustomTurnException("TurnDateTime", $"El turno {turn.TurnDateTime} debe ser seleccionado en un horario disponible entre las {openHour} y las {closeHour}.");
            }

            //Validación de turno si ya fue pagado y se quita el pago.
            if (turnq.Paid == true && turn.Paid == false)
            {
                throw new CustomTurnException("Paid", "El turno ya fue pagado.");
            }

			if (turnq != null)
            {
                turnq.TurnId = turn.TurnId;
                turnq.TurnDateTime = turn.TurnDateTime;
                turnq.ClientId = turn.ClientId;
                turnq.Paid = turn.Paid;
                turnq.Price = turn.Price;

                //registrar movimiento
                await GenerateMovement(turnq);
                misCanchasDbContext.Update(turnq);
            }

            
            await misCanchasDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
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
                await misCanchasDbContext.SaveChangesAsync();
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

        ////Turn Selector dbcontext
        //private Task TurnSelector(string i)
        //{
        //    return i switch
        //    {
        //        "1" => misCanchasDbContext.Turns,
        //        "2" => misCanchasDbContext.Turns2,
        //        "3" => misCanchasDbContext.Turns3,
        //        _ => null
        //    };
        //}

        //private registro de movimiento
        private async Task GenerateMovement(Turn turn)
        {
            if (turn.Paid)
            {
                var movementType = _movementService.GetType().Result
                    .Where(x => x.Name.Contains("turno"))
                    .Where(x => x.Incremental == true)
                    .FirstOrDefault();
                if (movementType == null)
                {
                    await _movementService.AddType(new MovementType() { Name = "Pago de turno", Incremental = true });
                    movementType = _movementService.GetType().Result.Where(x => x.Name.Contains("turno")).FirstOrDefault();
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
                var movementType = _movementService.GetType().Result
                    .Where(x => x.Name.Contains("turno"))
                    .Where(x => x.Incremental == false)
                    .FirstOrDefault();
                if (movementType == null)
                {
                    await _movementService.AddType(new MovementType() { Name = "Cancelacion de turno", Incremental = false });
                    movementType = _movementService.GetType().Result
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
