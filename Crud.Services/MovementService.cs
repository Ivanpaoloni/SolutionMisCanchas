using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using MisCanchas.EfSqlRepository.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MisCanchas.Services
{
    public class MovementService : IMovementService

    {

        private readonly MisCanchasDbContext _misCanchasDbContext;
        private readonly ICashService cashService;

        public MovementService(MisCanchasDbContext misCanchasDbContext, ICashService cashService)
        {
            this._misCanchasDbContext = misCanchasDbContext;
            this.cashService = cashService;
        }
        //Movements
        public async Task<IQueryable<Movement>> Get(int? id = null, MovementType? movementType = null, DateTime? date = null)
        {
            IQueryable<Movement> movements = new List<Movement>().AsQueryable();

            if (id.HasValue)
            {
                var movement = await _misCanchasDbContext.Movements.FindAsync(id);
                if (movement != null)
                {
                    movements.Append(movement);
                    return movements;
                }
            }
            if (movementType != null)
            {
                movements = _misCanchasDbContext.Movements.Where(m => m.MovementType == movementType);
                return movements;
            }
            if (date.HasValue)
            {
                movements = _misCanchasDbContext.Movements.Where(m => m.DateTime.Date == date.Value.Date);
                return movements;
            }

            //movements = (IQueryable<Movement>)await _misCanchasDbContext.Movements.ToListAsync();
            var allMovements = await _misCanchasDbContext.Movements.ToListAsync();
            var movementsq = allMovements.AsQueryable();
            return movementsq;
        }

        public async Task<IQueryable<Movement>> Get(DateTime start, DateTime end)
        {
            var movements = await _misCanchasDbContext.Movements.Where(m => m.DateTime.Date >= start && m.DateTime.Date <= end).ToListAsync();
            return movements.AsQueryable();
        }
        public async Task Add(Movement movement)
        {
            var cash = await cashService.Get();
            movement.MovementType = await GetTypeById(movement.MovementTypeId);

            //validacion si posee suficientes fondos
            if (!movement.MovementType.Incremental && movement.Amount > cash.Amount)
            {
                throw new CustomMovementException("Amount", "No hay suficiente saldo para retirar.");
            }

            movement.DateTime = DateTime.Now;
            if (movement.MovementType.Incremental == true)
            {
                movement.CurrentBalance = cash.Amount + movement.Amount;
                await cashService.Update(movement.Amount);
            }
            else
            {
                //movement type is not incremental
                movement.CurrentBalance = cash.Amount - movement.Amount;
                await cashService.Update(-movement.Amount);
                movement.Amount = -movement.Amount;
            }
            await _misCanchasDbContext.Movements.AddAsync(movement);
            await _misCanchasDbContext.SaveChangesAsync();
        }
        public Task Update(Movement movement)
        {
            throw new NotImplementedException();
        }

        //Movement types
        public async Task<IQueryable<MovementType>> GetTypes()
        {

            var movementTypes = await _misCanchasDbContext.MovementTypes.ToListAsync();
            var movementTypesq = movementTypes.AsQueryable();
            return movementTypesq;
        }
        public async Task<MovementType> GetTypeById(int id)
        {
            var movementType = await _misCanchasDbContext.MovementTypes.FindAsync(id);
            if (movementType != null)
            {
                return movementType;
            }
            throw new NotImplementedException();
        }
        public async Task AddType(MovementType movementType)
        {
            await _misCanchasDbContext.MovementTypes.AddAsync(movementType);
            await _misCanchasDbContext.SaveChangesAsync();
        }
        public async Task UpdateType(MovementType movementType)
        {
            var type = await _misCanchasDbContext.MovementTypes.FirstOrDefaultAsync(x => x.Id == movementType.Id);
            if (type != null)
            {
                type.Name = movementType.Name;
                type.Incremental = movementType.Incremental;
                await _misCanchasDbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteType(MovementType movementType)
        {
            _misCanchasDbContext.MovementTypes.Remove(movementType);
            await _misCanchasDbContext.SaveChangesAsync();
        }

        //excepciones
        public class CustomMovementException : Exception
        {
            public string PropertyName { get; }
            public CustomMovementException(string propertyName, string message) : base(message)
            {
                PropertyName = propertyName;
            }
        }

    }
}
