using MisCanchas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
