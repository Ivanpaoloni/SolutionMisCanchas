using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using MisCanchas.Models;
using MisCanchas.Services;
using System.Web;

namespace MisCanchas.Controllers
{
    public class TurnsController : Controller
    {
        private MisCanchasDbContext _context;
        private readonly IClientService _clientService;
        private readonly ITurnService _turnService;
        private readonly IFieldService fieldService;

        public TurnsController(MisCanchasDbContext context, IClientService clientService, ITurnService turnService, IFieldService fieldService)
        {
            this._context = context;
            this._clientService = clientService;
            this._turnService = turnService;
            this.fieldService = fieldService;
        }

        public async Task<IActionResult> Index()
        {
            //var turns = await _turnService.GetTurns();
            //var clients = await _clientService.GetClients();
            //foreach (var turn in turns)
            //{
            //    var client = await _clientService.GetSingleClient(turn.ClientId);
            //    turn.Client.ClientName = client.ClientName;
            //    turn.Client.ClientTelephone = client.ClientTelephone;
            //    turn.Client.ClientEmail = client.ClientEmail;
            //    turn.Client.NationalIdentityDocument = client.NationalIdentityDocument;
            //}
            return View();
        }

        //funcion para completar el calendario inicial
        public async Task<JsonResult> GetTurnsCalendar(DateTime start, DateTime end)
        {
            var turns = await _turnService.GetByDateRange(start, end);
            var clients = await _clientService.GetClients();
            foreach (var turn in turns)
            {
                var client = await _clientService.GetSingleClient(turn.ClientId);
                turn.Client.ClientName = client.ClientName;
                turn.Client.ClientTelephone = client.ClientTelephone;
                turn.Client.ClientEmail = client.ClientEmail;
                turn.Client.NationalIdentityDocument = client.NationalIdentityDocument;
            }
            //convierto lista de turnos a listado Json
            var turnsJson = turns.Select(t => new
            {
                id = t.TurnId,
                title = t.Client.ClientName,
                start = t.TurnDateTime.ToString("yyyy-MM-dd HH:mm"),
                end = t.TurnDateTime.ToString("yyyy-MM-dd HH:mm")
            });
            return Json(turnsJson);
        }

        public async Task<JsonResult> GetSingleTurnByDate(DateTime dateTime)
        {
            //DateTime d2 = DateTime.Parse(dateTime.ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind);

            var turns = await _turnService.GetSingleTurnByDate(dateTime);
            var clients = await _clientService.GetClients();
            foreach (var turn in turns)
            {
                var client = await _clientService.GetSingleClient(turn.ClientId);
                turn.Client.ClientName = client.ClientName;
                turn.Client.ClientTelephone = client.ClientTelephone;
                turn.Client.ClientEmail = client.ClientEmail;
                turn.Client.NationalIdentityDocument = client.NationalIdentityDocument;
            }
            //convierto lista de turnos a listado Json
            var turnsJson = turns.Select(t => new
            {
                id = t.TurnId,
                title = t.Client.ClientName,
                start = t.TurnDateTime.ToString("yyyy-MM-dd HH:mm"),
                end = t.TurnDateTime.ToString("yyyy-MM-dd HH:mm")
            });

            return Json(turnsJson);
        }

        [HttpGet]
        public async Task<IActionResult> Add(DateTime dateTime)
        {
            if(dateTime.Year == 0001)
            {
                dateTime = DateTime.Today;
            }
            else
            {
                dateTime = dateTime.AddHours(-3); //UTC-3
            }
            var viewModel = new AddTurnViewModel();
            viewModel.TurnDateTime = dateTime;
            viewModel.Clients = await GetClients();
            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddTurnViewModel addTurnViewModel)
        {
            if (!ModelState.IsValid)
            {
                addTurnViewModel.Clients = await GetClients();
                return View(addTurnViewModel);
            }
            var turns = await _turnService.GetTurns();

            //validacion si el turno es pasado
            var turnSelected = addTurnViewModel.TurnDateTime;
            if (turnSelected < DateTime.Now) 
            {
                addTurnViewModel.Clients = await GetClients();
                ModelState.AddModelError(nameof(addTurnViewModel.TurnDateTime), $"El turno {addTurnViewModel.TurnDateTime} no puede ser anterior a la fecha y hora actual.");
                return View(addTurnViewModel);
            }

            //validacion si el turno es duplicado
            var turnDuplicate = turns.FirstOrDefault(t => t.TurnDateTime == addTurnViewModel.TurnDateTime);
            if (turnDuplicate != null) 
            {
                addTurnViewModel.Clients = await GetClients();
                ModelState.AddModelError(nameof(addTurnViewModel.TurnDateTime), $"El turno {addTurnViewModel.TurnDateTime} ya fue reservado.");
                return View(addTurnViewModel);
            }

            //validacion de turno seleccionado entre los horarios definidos.
            int openHour = fieldService.Get().Result.OpenHour;
            int closeHour = fieldService.Get().Result.CloseHour;
            if (addTurnViewModel.TurnDateTime.Hour < openHour && addTurnViewModel.TurnDateTime.Hour > closeHour)
            {
                addTurnViewModel.Clients = await GetClients();
                ModelState.AddModelError(nameof(addTurnViewModel.TurnDateTime), $"El turno {addTurnViewModel.TurnDateTime} debe ser seleccionado en un horario disponible entre las {openHour} y las {closeHour}.");
                return View(addTurnViewModel);
            }
            await _turnService.Add(addTurnViewModel.TurnDateTime, addTurnViewModel.ClientId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTurno(Turn turn)
        {
            var turns = await _turnService.GetTurns();
            var turnDuplicate = turns.FirstOrDefault(t => t.TurnDateTime == turn.TurnDateTime);

            if (turnDuplicate != null)
            {
                return Json($"El Turno {turn.TurnDateTime} ya existe.");
            }
            return Json(true);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            var turnSelected = _turnService.Get(id);
            if (turnSelected != null)
            {
                var viewModel = new DeleteTurnViewModel
                {
                    TurnId = turnSelected.Result.TurnId,
                    TurnDateTime = turnSelected.Result.TurnDateTime,
                    ClientId = turnSelected.Result.ClientId   
                };
                return await Task.Run(() => View("Delete", viewModel));
            }
            if (turnSelected is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteTurnViewModel model)
        {
            await _turnService.Delete(model.TurnId);
            return RedirectToAction("Index");
        }

        //privates
        private async Task<IEnumerable<SelectListItem>> GetClients()
        {
            var clients = await _clientService.GetClients();
            return clients.Select(x => new SelectListItem(x.ClientName + " / " + x.NationalIdentityDocument.ToString(), x.ClientId.ToString()));
        }

        //horario para fullcalendar
        public IActionResult GetTimeRange()
        {
            var range = fieldService.Get();
            if (range.Result.CloseHour < range.Result.OpenHour)
            {
                range.Result.CloseHour += 24;
            }
            TimeSpan openHour = TimeSpan.FromHours(range.Result.OpenHour);
            string openHourFormat = openHour.ToString(@"hh\:mm"); // Formatear el objeto TimeSpan
            var closeHour = range.Result.CloseHour;
            string closeHourFormat = closeHour.ToString("00")+":00"; // Formatear el objeto TimeSpan
            var rangeJson = new
            {
                slotMinTime = openHour,
                slotMaxTime = closeHourFormat
            };
            return Json(rangeJson);
        }

    }
}
