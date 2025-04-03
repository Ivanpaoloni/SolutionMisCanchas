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
            var field1 = await fieldService.Get();
            field.OpenHour = field1.OpenHour;
            field.CloseHour = field1.CloseHour;
            field.Name = field1.Name;
            field.Price = field1.Price;
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

            await fieldService.Update(model.OpenHour, model.CloseHour, model.Name, model.Price);
            return RedirectToAction("Index");

        }
    }
}
