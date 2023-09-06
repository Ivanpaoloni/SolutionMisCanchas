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
    }
}
