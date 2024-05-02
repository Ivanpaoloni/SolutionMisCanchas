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
            // Obtener la fecha actual
            DateTime now = DateTime.Now;
            // Calcular la fecha hace seis meses
            DateTime start = now.AddMonths(-6);
		    // Calcular la fecha dentro de seis meses
		    DateTime end = now.AddMonths(6);
            // Filtrar la lista para incluir solo las fechas dentro del rango
            //List<ReportViewModel> filteredList = listModel.Where(f => f.Date >= start && f.Date <= end) .ToList();
            //List<ReportViewModel> filteredList = _reportService.Get(start, end);

            var list = await _reportService.Get(start, end);
            var model = new ReportViewModel();
            List<ReportViewModel> listModel = new List<ReportViewModel>();
            foreach (var report in list)
            {
                
                var reportViewModel = new ReportViewModel();
                reportViewModel.Id = report.Id;
                reportViewModel.Amount = report.Amount;
                reportViewModel.In = report.In;
                reportViewModel.Out = report.Out;
                reportViewModel.Canceled = report.Canceled;
                reportViewModel.Booking = report.Booking;
                var date = report.Date;
                reportViewModel.Date = new DateOnly(report.Date.Year, report.Date.Month, report.Date.Day);
                listModel.Add(reportViewModel);
            }


			var filteredList = listModel.OrderBy(x => x.Date).ToList();

			List<DataPoint> dataPoints1 = new List<DataPoint>();
			List<DataPoint> dataPoints2 = new List<DataPoint>();
			List<DataPoint> dataPoints3 = new List<DataPoint>();
            foreach (var report in filteredList)
            {
                dataPoints1.Add(new DataPoint(report.Date.ToString("MMMM yyyy"), ((int)report.Amount)));
                dataPoints2.Add(new DataPoint(report.Date.ToString("MMMM yyyy"), ((int)report.In)));
                dataPoints3.Add(new DataPoint(report.Date.ToString("MMMM yyyy"), ((int)report.Out)));
            }

			ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
			ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
			ViewBag.DataPoints3 = JsonConvert.SerializeObject(dataPoints3);

			return View(filteredList);
        }

    }
}
