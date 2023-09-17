using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using MisCanchas.Models;
using MisCanchas.Services;
using System.Linq.Expressions;

namespace MisCanchas.Controllers
{
    public class ClientsController : Controller
    {
        private readonly MisCanchasDbContext _context;
        private readonly IClientService _clientService;

        public ClientsController(MisCanchasDbContext context, IClientService clientService)
        {
            this._context = context;
            this._clientService = clientService;
        }

        
        public async Task <IActionResult> Index()
        {
            
            var clients = await _clientService.GetClients();
            //ordeno para mostar listado alfabetico por defecto
            clients = clients.OrderBy(c => c.ClientName).ToList().AsQueryable();

            return View(clients);
        }
        public async Task<IActionResult> Edit()
        {
            var clients = await _clientService.GetClients();
            return View(clients);
        }

        [HttpGet]
        public IActionResult Add( string urlRetorno = null)
        {
            //si la url esta vacia, por defecto redireccionar a /clients.
            if (urlRetorno is null)
            {
                urlRetorno = "/Clients";
            }
            //addClientViewModel.UrlRetorno = urlRetorno;
            ViewBag.UrlRetorno = urlRetorno;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddClientViewModel addClientViewModel)
        {

            if(!ModelState.IsValid)
            {
                return View(addClientViewModel);
            }

            var clients = await _clientService.GetClients();
            //validacion si el DNI es repetido.
            var DNI = clients.FirstOrDefault(c => c.NationalIdentityDocument == addClientViewModel.NationalIdentityDocument);
            if (DNI != null)
            {
                ModelState.AddModelError(nameof(addClientViewModel.NationalIdentityDocument), $"El DNI {addClientViewModel.NationalIdentityDocument} ya existe.");
                return View(addClientViewModel);
            }

            Client client = new Client
            {
                ClientEmail = addClientViewModel.ClientEmail,
                ClientName = addClientViewModel.ClientName,
                ClientTelephone = addClientViewModel.ClientTelephone,
                NationalIdentityDocument = addClientViewModel.NationalIdentityDocument,
            };

            await _clientService.Add(client);


            if (string.IsNullOrEmpty(addClientViewModel.UrlRetorno))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return LocalRedirect(addClientViewModel.UrlRetorno);
            }
        }

        [HttpGet]
        public async Task<IActionResult> View(int id) 
        {
            var singleClient = _clientService.GetSingleClient(id);
            if (singleClient != null)
            {
                var viewModel = new UpdateClientViewModel
                {
                    ClientName = singleClient.Result.ClientName,
                    ClientEmail = singleClient.Result.ClientEmail,
                    NationalIdentityDocument = singleClient.Result.NationalIdentityDocument,
                    ClientTelephone = singleClient.Result.ClientTelephone,
                    ClientId = singleClient.Result.ClientId,
                };
                return await Task.Run(() => View("View" ,viewModel));
            }
            return View("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateClientViewModel updateClientViewModel)
        {
            if (!ModelState.IsValid)
            {
                return await Task.Run(() => View("View", updateClientViewModel));
            }

            var clients = await _clientService.GetClients();
            //validacion si el DNI es repetido.
            var DNI = clients.FirstOrDefault(c => c.NationalIdentityDocument == updateClientViewModel.NationalIdentityDocument);

            //valida si el dni ingresado es nulo y si es distinto del anterior
            if (DNI != null && DNI.NationalIdentityDocument != updateClientViewModel.NationalIdentityDocument)
            {
                ModelState.AddModelError(nameof(updateClientViewModel.NationalIdentityDocument), $"El DNI {updateClientViewModel.NationalIdentityDocument} ya existe.");
                return await Task.Run(() => View("View", updateClientViewModel));
            }

            Client client = new Client { 
                ClientEmail = updateClientViewModel.ClientEmail,
                ClientId=updateClientViewModel.ClientId,
                ClientName = updateClientViewModel.ClientName,
                ClientTelephone=updateClientViewModel.ClientTelephone,
                NationalIdentityDocument = updateClientViewModel.NationalIdentityDocument,

            };
            await _clientService.Edit(updateClientViewModel.ClientId, client);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, UpdateClientViewModel updateClientViewModel)
        {
            var singleClient = _clientService.GetSingleClient(id);
            if (singleClient != null)
            {
                var viewModel = new UpdateClientViewModel
                {
                    ClientId = singleClient.Result.ClientId,
                    ClientName = singleClient.Result.ClientName,
                    ClientEmail= singleClient.Result.ClientEmail,
                    ClientTelephone= singleClient.Result.ClientTelephone,
                    NationalIdentityDocument= singleClient.Result.NationalIdentityDocument,
                };
                return await Task.Run(() => View("Delete", viewModel));
            }
            return RedirectToAction("Index");
            }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateClientViewModel model)
        {
            Client client = new Client
            {
                ClientEmail = model.ClientEmail,
                ClientId = model.ClientId,
                ClientName = model.ClientName,
                ClientTelephone = model.ClientTelephone,
                NationalIdentityDocument = model.NationalIdentityDocument,

            };
            await _clientService.Delete(model.ClientId, client);
            return RedirectToAction("Index");
        }
    }
}
