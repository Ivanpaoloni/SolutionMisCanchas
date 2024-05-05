using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.Services
{
    public interface IClientService
    {
        Task Add(Client client);
        Task Delete(int id, Client client);
        Task Edit(int id, Client client);
        Task<IQueryable<Client>> GetClients();
        Task<Client> GetSingleClient(int id);
    }
}
