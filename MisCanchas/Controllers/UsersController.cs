using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MisCanchas.Contracts.Dtos.User;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain;
using MisCanchas.Models;
using System.Reflection.Metadata;

namespace MisCanchas.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly MisCanchasDbContext _context;
        private readonly IUserService _userService;

        public UsersController
        (
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            MisCanchasDbContext context,
            IUserService userService
        )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._context = context;
            this._userService = userService;
        }

        [Authorize(Roles = Constants.RollAdmin)]
        public IActionResult Regist()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = Constants.RollAdmin)]
        public async Task<IActionResult> Regist(RegistViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _userService.Create(new UserCreateDto
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password
            });

            if (result.Succeeded)
            {
                //var user = await userService.Get(model.Email);
                //await signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("List", "Users");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }
                return View(model);
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            //if usersList is empty, create an admin user.
            var usersExist = _userService.List().Result.Count();
            if (usersExist == 0)
            {
                _userService.CreateDefaultUser();
                TurnAdmin("admin@admin").GetAwaiter().GetResult();
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync
                (
                    model.UserName,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false
                );

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto.");
                return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = Constants.RollAdmin)]
        public async Task<IActionResult> List(string? message = null)
        {
            var users = await _userService.List();
            var usersList = users.Select(u => new UserViewModel { Email = u.Email, UserName = u.UserName }).ToList();
            var model = new UsersListViewModel();
            if (usersList != null)
            {
                foreach (var user in usersList)
                {
                    if (user.Email != null)
                    {
                        user.IsAdmin = await this._userService.IsAdmin(user.Email);
                        var rol = await this._userService.GetRole(user.Email);
                        user.Role = rol.ToString();
                    }
                }
            }
            model.Users = usersList;
            model.Message = message;
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Constants.RollAdmin)]
        public async Task<IActionResult> EditRol(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            if (model.Email == null)
            {
                return NotFound();
            }

            //validacion para no editar el administrador en uso.
            if (User?.Identity?.Name == model.UserName)
            {
                return RedirectToAction("List", routeValues: new { message = "No puede editar una cuenta en uso." });

            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Constants.RollAdmin)]
        public async Task<IActionResult> Delete(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            if (model.Email == null)
            {
                return NotFound();
            }
            //validacion para no editar el administrador en uso.
            if (User?.Identity?.Name == model.Email)
            {
                return RedirectToAction("Delete", routeValues: new { message = "No puede eliminar una cuenta en uso." });

            }
            model.IsAdmin = await _userService.IsAdmin(model.Email);

            var user = await _userService.Get(model.Email);
            if (user == null)
            {
                return NotFound();
            }
            _userService.Delete(model.Email);
            return RedirectToAction("List", routeValues: new { message = "Se ha eliminado correctamente el usuario " + model.Email });
        }

        [HttpPost]
        [Authorize(Roles = Constants.RollAdmin)]
        public async Task<IActionResult> TurnAdmin(string email)
        {
            var user = await _userService.Get(email);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.RemoveRol(user, Constants.RollUser);
            await _userService.AddRol(user, Constants.RollAdmin);
            return RedirectToAction("List", routeValues: new { message = "Rol asignado correctamente a " + email });
        }

        [HttpPost]
        [Authorize(Roles = Constants.RollAdmin)]
        public async Task<IActionResult> RemoveAdmin(string email)
        {
            var user = await _userService.Get(email);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.RemoveRol(user, Constants.RollAdmin);
            await _userService.AddRol(user, Constants.RollUser);
            return RedirectToAction("List", routeValues: new { message = "Rol quitado correctamente a " + email });
        }
    }



}
