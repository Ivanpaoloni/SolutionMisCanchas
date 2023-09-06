using Microsoft.AspNetCore.Mvc;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using MisCanchas.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MisCanchas.Controllers
{
    public class ReportsController : Controller
    {
        private MisCanchasDbContext _context;
        private readonly IClientService _clientService;
        private readonly ITurnService _turnService;
        private readonly IFieldService _fieldService;
        private readonly IReportService _reportService;

        public ReportsController(MisCanchasDbContext context, IClientService clientService, ITurnService turnService, IFieldService fieldService, IReportService reportService)
        {
            this._context = context;
            this._clientService = clientService;
            this._turnService = turnService;
            this._fieldService = fieldService;
            this._reportService = reportService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DateTime currentDay = DateTime.Now;
            DateTime start = DateTime.Today.AddDays(-currentDay.Day + 1);
            DateTime end = start.AddMonths(1).AddDays(-1);

            var currentYear = await _reportService.MonthReport(start.AddMonths(-DateTime.Today.Month+1), start.AddMonths(-DateTime.Today.Month + 1).AddYears(1).AddDays(-1));
            var currentMonth = await _reportService.MonthReport(start, end);
            var lastMonth = await _reportService.MonthReport(start.AddMonths(-1), end.AddMonths(-1));
            var secondLastMonth = await _reportService.MonthReport(start.AddMonths(-2), end.AddMonths(-2));


            var viewModel = new ReportViewModel();
            viewModel.currentMonth  = currentMonth;
            viewModel.lastMonth  = lastMonth;
            viewModel.secondLastMonth  = secondLastMonth;
            viewModel.currentYear = currentYear;
            return View(viewModel);
        }

    }
}
