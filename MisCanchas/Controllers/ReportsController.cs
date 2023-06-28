using Microsoft.AspNetCore.Mvc;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
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



    }
}
