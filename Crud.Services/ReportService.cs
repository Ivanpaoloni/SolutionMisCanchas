using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Services
{
    public class ReportService : IReportService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;
        private readonly ITurnService _turnService;
        private readonly IMovementService _movementService;
        public ReportService(MisCanchasDbContext misCanchasDbContext, ITurnService turnService, IMovementService movementService)
        {
            this.misCanchasDbContext = misCanchasDbContext;
            this._turnService = turnService;
            this._movementService = movementService;
        }

        public async Task<Report> Get(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            var report = await misCanchasDbContext.Reports
                .FirstOrDefaultAsync(t => t.Date.Month == month && t.Date.Year == year);

            return report ?? new Report();
        }
        public async Task<IQueryable<Report>> Get(DateTime start, DateTime end)
        {
            List<Report> reports = new List<Report>();
            var list = _turnService.GetByDateRange(start, end).Result.OrderBy(x => x.TurnDateTime);

            foreach (var turn in list)
            {
                var report = reports.FirstOrDefault(r => r.Date.Month == turn.TurnDateTime.Month && r.Date.Year == turn.TurnDateTime.Year);
                if (report == null)
                {
                    report = new Report
                    {
                        Booking = 1,
                        Date = turn.TurnDateTime.Date
                    };
                    reports.Add(report);
                }
                else
                {
                    report.Booking++;
                }
            }

            var listMovements = await _movementService.Get(start, end);
            var listMovementsType  = await _movementService.GetTypes();

            foreach (var movement in listMovements)
            {
                movement.MovementType = listMovementsType.FirstOrDefault(x => x.Id == movement.MovementTypeId)?? new MovementType();
                var report = reports.FirstOrDefault(r => r.Date.Month == movement.DateTime.Month && r.Date.Year == movement.DateTime.Year);
                if (report == null)
                {
                    report = new Report
                    {
                        In = 0,
                        Out = 0,
                        Amount = 0,
                        Date = movement.DateTime.Date
                    };
                    if(movement.MovementType.Incremental == false)
                    {
                        report.Out -= movement.Amount;
                    }
                    else
                    {
                        report.In += movement.Amount;
                    }
                    reports.Add(report);
                }
                else
                {
                    if (movement.MovementType.Incremental == false)
                    {
                        report.Out -= movement.Amount;
                    }
                    else
                    {
                        report.In += movement.Amount;
                    }
                }
                report.Amount += movement.Amount;
            }
            IQueryable<Report> reportsq = reports.AsQueryable();
            return reportsq;
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
