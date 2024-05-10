using Microsoft.AspNetCore.Identity;

namespace MisCanchas.Contracts.Services
{
    public interface IUserService
    {
        public Task<IdentityResult> Create(string email, string password);
        public void Delete(string email);
        public Task<IQueryable<IdentityUser>> List();
        public Task<IdentityUser> Get(string email);
        public Task<bool> IsAdmin(string email);
        public void CreateDefaultUser();
        public Task<IdentityRole<string>> GetRole(string email);
    }
}
