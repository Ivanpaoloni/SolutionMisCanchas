using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.Services
{
    public interface ICashService
    {
        Task<Cash> Get();
        Task Update(decimal amount);
    }
}
