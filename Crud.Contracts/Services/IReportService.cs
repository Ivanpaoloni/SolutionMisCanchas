using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.Services
{
    public interface IReportService
    {
        Task<Report> Get(DateTime dateTime);
        Task<IQueryable<Report>> Get(DateTime start, DateTime end);
        Task<IQueryable<Report>> GetAll();
        Task Update(Report report);
    }
}
