using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using MisCanchas.Models;
using MisCanchas.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using static MisCanchas.Services.MovementService;

namespace MisCanchas.Controllers
{
    public class MovementsController : Controller
    {
        private MisCanchasDbContext _context;
        private readonly IReportService _reportService;
        private readonly IMovementService _movementService;
        private readonly ICashService _cashService;

        public MovementsController(MisCanchasDbContext context, IReportService reportService, IMovementService movementService,
            ICashService cashService)
        {
            this._context = context;
            this._reportService = reportService;
            this._movementService = movementService;
            this._cashService = cashService;
        }



        public async Task<IActionResult> Index()
        {
            try
            {
                var movements = await _movementService.Get();
                var cash = _cashService.GetDto();
                ViewBag.Cash = cash.Amount;
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
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ha ocurrido un error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                var viewModel = new AddMovementViewModel();
                viewModel.MovementTypes = await GetTypes();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ha ocurrido un error: " + ex.Message;
            }
            return RedirectToAction("Add");
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddMovementViewModel addMovementViewModel)
        {
            if (!ModelState.IsValid)
            {
                addMovementViewModel.MovementTypes = await GetTypes();
                return View(addMovementViewModel);
            }
            try
            {
               
                var model = new Movement
                    {
                    Name = addMovementViewModel.Name,
                    MovementTypeId = addMovementViewModel.MovementTypeId,
                    Amount = addMovementViewModel.Amount,
                    Description = addMovementViewModel.Description
                };

                await _movementService.Add(model);

                return RedirectToAction("Index");
            }
            catch (CustomMovementException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            addMovementViewModel.MovementTypes = await GetTypes();
            return View(addMovementViewModel);

        }



        [HttpGet]
        public async Task<IActionResult> Types()
        {
            try
            {
                var movementTypes = new List<MovementType>();
                var list = await _movementService.GetTypes();
                if(list != null)
                {
                    movementTypes = list.ToList();
                }

                return View(movementTypes);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ha ocurrido un error: " + ex.Message;
            }
            return RedirectToAction("Types");
        }

        [HttpGet]
        public IActionResult AddType()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddType(AddMovementTypeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var type = new MovementType();
                type.Name = model.Name;
                type.Incremental = model.Incremental; 

                await _movementService.AddType(type);
                return RedirectToAction("Types");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ha ocurrido un error: " + ex.Message;
            }
            return RedirectToAction("AddType");
        }

        [HttpGet]
        public async Task<IActionResult> EditType(int id)
        {
            try
            {
                var model = new MovementType();
                var type = await _movementService.GetTypeById(id);
                if (type != null)
                {
                    model.Id = type.Id;
                    model.Name = type.Name;
                    model.Incremental = type.Incremental;
                    model.Movements = type.Movements;
                    return await Task.Run(() => View("EditType", model));
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ha ocurrido un error: " + ex.Message;
            }
            return RedirectToAction("EditType");
        }

        [HttpPost]
        public async Task<IActionResult> EditType(MovementType type)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(type);
                }
                var model = new MovementType();
                model.Id = type.Id;
                model.Name = type.Name;
                model.Incremental = type.Incremental;
                model.Movements = type.Movements;

                await _movementService.UpdateType(model);

                return RedirectToAction("Types");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ha ocurrido un error: " + ex.Message;
                return RedirectToAction("EditType");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Modal()
        {
            return await Task.Run(() => View("Add"));
        }
        //privates
        //listado de items para seleccionar el tipo en el formulario
        private async Task<IEnumerable<SelectListItem>> GetTypes() 
        {
            try
            {
                var types = await _movementService.GetTypes();
                return types.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            }
            catch (Exception ex)
            { 
                ViewBag.ErrorMessage = "Ha ocurrido un error: " + ex.Message;
                var types = new SelectListItem();
                return (IEnumerable<SelectListItem>)types;
            }
        }
    
    }
    
}
