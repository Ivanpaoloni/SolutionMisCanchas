using Microsoft.AspNetCore.Identity;
using MisCanchas.Contracts.Dtos.User;
using MisCanchas.Domain;

namespace MisCanchas.Contracts.Services
{
    public interface IUserService
    {
        public Task<IdentityResult> Create(UserCreateDto dto);
        public void Delete(string email);
        public Task<IQueryable<IdentityUser>> List();
        public Task<IdentityUser> Get(string email);
        public Task<bool> IsAdmin(string email);
        public void CreateDefaultUser();
        public Task<IdentityRole<string>> GetRole(string email);
        public Task<IdentityResult> AddRol(IdentityUser user, string rol);
        public Task<IdentityResult> RemoveRol(IdentityUser user, string rol);
    }
}
