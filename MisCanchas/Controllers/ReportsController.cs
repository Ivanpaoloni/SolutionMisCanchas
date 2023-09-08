using Microsoft.AspNetCore.Mvc;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using MisCanchas.Models;
using Newtonsoft.Json;
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
            var list = await _reportService.GetAll();
            var model = new ReportViewModel();
            List<ReportViewModel> listModel = new List<ReportViewModel>();
            foreach (var report in list)
            {
                
                var reportViewModel = new ReportViewModel();
                reportViewModel.Id = report.Id;
                reportViewModel.Amount = report.Amount;
                var date = report.Date;
                reportViewModel.Date = new DateOnly(report.Date.Year, report.Date.Month, report.Date.Day);
                listModel.Add(reportViewModel);
            }

            // Obtener la fecha actual
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
		    // Calcular la fecha hace seis meses
		    DateOnly start = now.AddMonths(-6);
		    // Calcular la fecha dentro de seis meses
		    DateOnly end = now.AddMonths(6);
            // Filtrar la lista para incluir solo las fechas dentro del rango
            List<ReportViewModel> filteredList = listModel.Where(f => f.Date >= start && f.Date <= end) .ToList();


			filteredList = filteredList.OrderBy(x => x.Date).ToList();

			List<DataPoint> dataPoints1 = new List<DataPoint>();
            foreach (var report in filteredList)
            {
                dataPoints1.Add(new DataPoint(report.Date.ToString("MMMM yyyy"), ((int)report.Amount)));
            }

			ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

			return View(filteredList);
        }

    }
}
