using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.Services
{
    public interface IMovementService
    {
        //movimientos
        Task<IQueryable<Movement>> Get(int? id = null, MovementType movementType = null, DateTime? fecha = null);
        Task<IQueryable<Movement>> Get(DateTime start, DateTime end);
        Task Add(Movement movement);
        Task Update(Movement movement);

        //tipos
        Task<IQueryable<MovementType>> GetTypes();
        Task AddType(MovementType movementType);
        Task DeleteType(MovementType movementType);
        Task<MovementType> GetTypeById(int id);
        Task UpdateType(MovementType movementType);
    }


}
