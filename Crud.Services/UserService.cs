using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Dtos.User;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain;

namespace MisCanchas.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MisCanchasDbContext misCanchasDbContext;
        private readonly IMapper _mapper;

        public UserService(UserManager<IdentityUser> userManager, MisCanchasDbContext misCanchasDbContext, IMapper mapper)
        {
            this._userManager = userManager;
            this.misCanchasDbContext = misCanchasDbContext;
            this._mapper = mapper;
        }

        public async Task<IdentityResult> Create(UserCreateDto dto)
        {
            var user = this._mapper.Map<IdentityUser>(dto);
            var result = await this.Create(user, dto.Password);
            if (result.Succeeded)
            {
                await this.AddRol(user, Constants.RollUser);
            }
            return result;
        }

        public async Task<IdentityUser> Get(string email)
        {
            var user = await this.InternalGet(email);
            return user;
        }

        public async Task<bool> IsAdmin(string email)
        {
            var role = await this.InternalGetRoleByUser(email);
            if (role.Name == Constants.RollAdmin)
            {
                return true;
            }
            return false;
        }

        public async Task<IdentityResult> AddRol(IdentityUser user, string rol)
        {
            var role = rol.ToString();
            if (role == null)
            {
                throw new ArgumentNullException("Debe especificar un rol");
            }

            var result = await this.InternalAddRol(user, role);
            return result;
        }

        public async Task<IdentityResult> RemoveRol(IdentityUser user, string rol)
        {
            var role = rol.ToString();
            if (role == null)
            {
                throw new ArgumentNullException("Debe especificar un rol");
            }

            var result = await this.InternalRemoveRol(user, role);
            return result;
        }

        public async Task<IQueryable<IdentityUser>> List()
        {
            var users = await this.InternalGet();
            return users;
        }

        public async void Delete(string email)
        {
            var adminSelected = await this.InternalGet(email);
            if (adminSelected != null)
            {
                misCanchasDbContext.Users.Remove(adminSelected);
                misCanchasDbContext.SaveChanges();
            }
        }

        public void CreateDefaultUser()
        {
            this?.Create(new IdentityUser { Email = "admin@admin", UserName = "admin" }, "aA123456");
        }

        public async Task<IdentityRole<string>> GetRole(string email)
        {
            var role = await this.InternalGetRoleByUser(email);
            return role;
        }

        //INTERNAL METHODS

        internal async Task<IdentityUser> InternalGet(string email)
        {
            var user = await misCanchasDbContext.Users
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();
            if (user == null) { throw new ArgumentException("Usuario no encontrado"); };
            return user;
        }

        internal async Task<IQueryable<IdentityUser>> InternalGet()
        {
            var users = await misCanchasDbContext.Users.ToListAsync();
            var usersq = users.AsQueryable();
            return usersq;
        }

        internal async Task<IdentityUserRole<string>> InternalGetRolId(string id)
        {
            var userRole = await misCanchasDbContext.UserRoles.FirstOrDefaultAsync(u => u.UserId == id);
            if (userRole == null)
            {
                throw new ArgumentException("El usuario no tiene roles asignados");
            }
            return userRole;
        }

        internal async Task<IdentityRole<string>> InternalGetRoleByUser(string email)
        {
            var roleId = this.InternalGetRolId(this.InternalGet(email: email).Result.Id);
            var role = await misCanchasDbContext.Roles.FirstOrDefaultAsync(x => x.Id == roleId.Result.RoleId);
            if (role == null)
            {
                throw new ArgumentException("No se encontraron usuarios con este email");
            }
            return role;
        }

        internal async Task<IdentityResult> Create(IdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password: password);
            if (!result.Succeeded)
            {
                throw new ArgumentException($"{result?.Errors?.FirstOrDefault()?.Description.ToString()}");
            }
            return result;
        }

        internal async Task<IdentityResult> InternalAddRol(IdentityUser user, string rol)
        {
            if (rol != null)
            {
                var result = await this._userManager.AddToRoleAsync(user, rol);
                if (!result.Succeeded)
                {
                    throw new ArgumentException("Error al agregar el rol");
                }
            }
            return new IdentityResult();
        }

        internal async Task<IdentityResult> InternalRemoveRol(IdentityUser user, string rol)
        {
            if (rol != null)
            {
                var result = await this._userManager.RemoveFromRoleAsync(user, rol);
                if (!result.Succeeded)
                {
                    throw new ArgumentException("Error al remover el rol");
                }
            }
            return new IdentityResult();
        }
    }
}
