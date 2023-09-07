using MisCanchas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Contracts.Services
{
    public interface IReportService
    {
        Task<decimal> MonthReport(DateTime start, DateTime end);
        Task<Report> Get(DateTime dateTime);
        Task<IQueryable<Report>> GetAll();
        Task Update(Report report);
    }
}
