using MisCanchas.Contracts.Dtos.Client;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.Services
{
    public interface IClientService
    {
        Task<int> Create(ClientCreateDto client, bool saveChanges = false);
        Task Update(ClientUpdateDto client, bool saveChanges = false);
        Task Delete(int id, bool saveChanges = false);
        Task<IQueryable<Client>> GetClients();
        Task<Client> GetSingleClient(int id);
    }
}
