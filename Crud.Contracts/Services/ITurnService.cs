using MisCanchas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Contracts.Services
{
    public interface ITurnService
    {
        Task Add(DateTime dateTime, int id);
        Task<Turn> Get(int id);
        Task<IQueryable<Turn>> GetTurns();
        Task Delete(int id);
        Task<IQueryable<Turn>> GetByDateRange(DateTime startDateTime, DateTime endDateTime);
        Task<IQueryable<Turn>> GetSingleTurnByDate(DateTime dateTime);
    }
}
