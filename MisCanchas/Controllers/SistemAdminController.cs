using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain;
using MisCanchas.Models;

namespace MisCanchas.Controllers
{
    public class SistemAdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly MisCanchasDbContext misCanchasDbContext;
        private readonly IFieldService fieldService;

        public SistemAdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager , MisCanchasDbContext misCanchasDbContext, IFieldService fieldService ) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.misCanchasDbContext = misCanchasDbContext;
            this.fieldService = fieldService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = Constants.RollAdmin)]
        public async Task<IActionResult> FieldsAdjust()
        {
            var field = new AdminFieldViewModel();
            var dbField = await fieldService.Get();
            field.OpenHour = dbField.OpenHour;
            field.CloseHour = dbField.CloseHour;
            field.Name = dbField.Name;
            field.Price = dbField.Price;
            field.Deposit = dbField.Deposit;
            return View(field);
        }

        [HttpPost]
        [Authorize(Roles = Constants.RollAdmin)]
        public async Task<IActionResult> FieldsAdjust(AdminFieldViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await fieldService.Update(model.OpenHour, model.CloseHour, model.Name, model.Price, model.Deposit);
            return RedirectToAction("Index");

        }
    }
}
