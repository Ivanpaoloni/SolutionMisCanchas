using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.Services
{
    public interface ITurnService
    {
        Task Add(DateTime dateTime, int id, decimal price, bool paid);
        Task<Turn> Get(int id);
        Task<IQueryable<Turn>> GetTurns();
        Task Delete(int id);
        Task<IQueryable<Turn>> GetByDateRange(DateTime startDateTime, DateTime endDateTime);
        Task<IQueryable<Turn>> GetSingleTurnByDate(DateTime dateTime);
        Task Update(Turn turn);
    }
}
