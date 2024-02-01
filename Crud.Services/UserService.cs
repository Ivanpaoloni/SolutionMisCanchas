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
            var user = await misCanchasDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<bool> IsAdmin(string email)
        {
            var adminSelected = await misCanchasDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (adminSelected != null)
            {
                var id = adminSelected.Id;
                var admin = await misCanchasDbContext.UserRoles.FirstOrDefaultAsync(u => u.UserId == id);
                if (admin == null)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<IQueryable<IdentityUser>> List()
        {
            var users = await misCanchasDbContext.Users.ToListAsync();
            var usersq = users.AsQueryable();
            return usersq;
        }

        public void CreateDefaultUser()
        {
            Create("admin@admin","aA123456");
        }

        public async Task Delete(string email)
        {
            var adminSelected = await misCanchasDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (adminSelected != null)
            {
                misCanchasDbContext.Users.Remove(adminSelected);
                misCanchasDbContext.SaveChanges();
            }
        }
    }
}
