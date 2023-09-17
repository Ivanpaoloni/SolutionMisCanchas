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

namespace MisCanchas.Services
{
    public class MovementService : IMovementService

    {

        private readonly MisCanchasDbContext _misCanchasDbContext;

        public MovementService(MisCanchasDbContext misCanchasDbContext)
        {
            this._misCanchasDbContext = misCanchasDbContext;
        }
        //Movements
        public async Task<IQueryable<Movement>> Get(int? id = null, MovementType movementType = null, DateTime? date = null)
        {
            IQueryable<Movement> movements = null;

            if (id.HasValue)
            {
                var movement = await _misCanchasDbContext.Movements.FindAsync(id);
                if(movement != null)
                {
                    movements.Append(movement);
                    return movements;
                }
            }
            if (movementType != null)
            {
                movements = _misCanchasDbContext.Movements.Where(m => m.MovementType == movementType);
            }

            if (date.HasValue)
            {
                movements = _misCanchasDbContext.Movements.Where(m => m.DateTime.Date == date.Value.Date);
            }

            //movements = (IQueryable<Movement>)await _misCanchasDbContext.Movements.ToListAsync();
            var allMovements = await _misCanchasDbContext.Movements.ToListAsync();
            var movementsq = allMovements.AsQueryable();
            return movementsq;


        }
        public async Task Add(Movement movement)
        {
            movement.DateTime = DateTime.Now;
            movement.MovementTypeId = movement.MovementTypeId;
            await _misCanchasDbContext.Movements.AddAsync(movement);
            await _misCanchasDbContext.SaveChangesAsync();
        }

        public Task Update(Movement movement)
        {
            throw new NotImplementedException();
        }


        //Movement types
        public async Task<IQueryable<MovementType>> GetType(int? id = null)
        {
            if (id.HasValue)
            {
                IQueryable <MovementType> movementTypeList = null;
                var movementType = await _misCanchasDbContext.MovementTypes.FindAsync(id);
                if (movementType != null)
                {
                    movementTypeList.Append(movementType);
                    return movementTypeList;
                }
            }
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
        public Task AddType(MovementType movementType)
        {
            throw new NotImplementedException();
        }

        public Task DeleteType(MovementType movementType)
        {
            throw new NotImplementedException();
        }

    }
}
