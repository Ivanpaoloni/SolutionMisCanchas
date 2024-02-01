using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Contracts.Services
{
    public interface IUserService
    {
        Task<IdentityResult> Create(string email, string password);
        void CreateDefaultUser();
        Task<IdentityUser> Get(string email);
        Task<bool> IsAdmin(string email);
        Task<IQueryable<IdentityUser>> List();
        Task Delete(string email);
    }
}
