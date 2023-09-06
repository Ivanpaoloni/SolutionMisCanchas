using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Services
{
    public class ReportService : IReportService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;
        private readonly ITurnService _turnService;
        public ReportService(MisCanchasDbContext misCanchasDbContext, ITurnService turnService)
        {
            this.misCanchasDbContext = misCanchasDbContext;
            this._turnService = turnService;
        }


        public async Task<decimal> MonthReport(DateTime start, DateTime end)
        {
            
            var turns = await _turnService.GetByDateRange(start, end);
            decimal turnRevenue = 0;
            foreach (var turn in turns)
            {
                turnRevenue += turn.Price;
            }
            return turnRevenue;
        }
    }
}
