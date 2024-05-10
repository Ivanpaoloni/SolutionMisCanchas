using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly MisCanchasDbContext misCanchasDbContext;

        public UserService(UserManager<IdentityUser> userManager, MisCanchasDbContext misCanchasDbContext)
        {
            this.userManager = userManager;
            this.misCanchasDbContext = misCanchasDbContext;
        }

        public async Task<IdentityResult> Create(string email, string password)
        {
            var user = new IdentityUser() { Email = email, UserName = email };
            var result = await userManager.CreateAsync(user, password: password);

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
            Create("admin@admin", "aA123456");
        }

        public Task< IdentityRole<string>> GetRole(string email)
        {
            var role = this.InternalGetRoleByUser(email);
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
            return role;
        }
    }
}
