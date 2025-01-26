using MisCanchas.Contracts.Dtos.Turn;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.Services
{
    public interface ITurnService
    {
        Task<int> Create(TurnCreateDto dto, bool saveChanges = false);
        Task Update(TurnUpdateDto dto, bool saveChanges = false);
        Task Delete(int id, bool saveChanges = false);
        Task<Turn> Get(int id);
        Task<IQueryable<Turn>> GetTurns();
        Task<IQueryable<Turn>> GetByDateRange(DateTime startDateTime, DateTime endDateTime);
        Task<IQueryable<Turn>> GetSingleTurnByDate(DateTime dateTime);
    }
}
