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

        public async Task<Report> Get(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            var report = await misCanchasDbContext.Reports
                .FirstOrDefaultAsync(t => t.Date.Month == month && t.Date.Year == year);

            return report;
        }
        public async Task<IQueryable<Report>> GetAll()
        {
            var list = await misCanchasDbContext.Reports.ToListAsync();
            var listq = list.AsQueryable();
            return listq;
        }
        public async Task Update(Report report)
        {
            //var oldReport = await Get(report.Date);
            //if (oldReport == null)
            //{ 
            //    report.Amount += report.In;
            //}
            //if (oldReport  != null)
            //{
            //    if (report.Amount > oldReport.Amount) 
            //    { 
            //    }
            //}

            misCanchasDbContext.Reports.Update(report);
            await misCanchasDbContext.SaveChangesAsync();
        }
    }
}
