using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using MisCanchas.Models;
using MisCanchas.Services;
using System;

namespace MisCanchas.Controllers
{
    public class MovementsController : Controller
    {
        private MisCanchasDbContext _context;
        private readonly IReportService _reportService;
        private readonly IMovementService _movementService;

        public MovementsController(MisCanchasDbContext context, IReportService reportService, IMovementService movementService)
        {
            this._context = context;
            this._reportService = reportService;
            this._movementService = movementService;
        }



        public async Task<IActionResult> Index()
        {
            var movements = await _movementService.Get();
            if(movements.Count() >= 10)
            {
                movements = movements.Skip(Math.Max(0, movements.Count() - 10)).Take(10);

            }


            //ordeno para mostar listado alfabetico por defecto
            if (movements != null)
            {
                movements = movements.OrderByDescending(c => c.DateTime).ToList().AsQueryable();
                foreach(var m in movements)
                {
                    var type = await _movementService.GetTypeById(m.MovementTypeId);
                    if (type != null)
                    {
                        m.MovementType = type;
                    }
                }

                return View(movements);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var viewModel = new AddMovementViewModel();
            viewModel.MovementTypes = await GetTypes();
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddMovementViewModel addMovementViewModel)
        {
            var model = new Movement();
            model.Name = addMovementViewModel.Name;
            model.Amount = addMovementViewModel.Amount;
            model.Description = addMovementViewModel.Description;
            model.MovementTypeId = addMovementViewModel.MovementTypeId;

            await _movementService.Add(model);

            return RedirectToAction("Index");
        }

        //privates
        //listado de items para seleccionar el tipo en el formulario
        private async Task<IEnumerable<SelectListItem>> GetTypes() 
        {
            var types = await _movementService.GetType();
            return types.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
    }
}
