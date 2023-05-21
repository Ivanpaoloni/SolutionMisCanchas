using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Services
{
    public class ClientService : IClientService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;

        public ClientService(MisCanchasDbContext dbContext)
        {
            this.misCanchasDbContext = dbContext;
        }
        public async Task Add(Client client)
        {
            await misCanchasDbContext.AddAsync(client);
            await misCanchasDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id, Client client)
        {
            var clientDeleted = await misCanchasDbContext.Clients.FindAsync(client.ClientId);
            if (clientDeleted != null)
            {
                misCanchasDbContext.Clients.Remove(clientDeleted);
                await misCanchasDbContext.SaveChangesAsync();
            }
        }

        public async Task Edit(int id, Client client)
        {
            var clientEdit = await misCanchasDbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == id);

            if (clientEdit != null)
            {
                clientEdit.ClientName = client.ClientName;
                clientEdit.NationalIdentityDocument = client.NationalIdentityDocument;
                clientEdit.ClientEmail = client.ClientEmail;
                clientEdit.ClientTelephone = client.ClientTelephone;

                await misCanchasDbContext.SaveChangesAsync();

            }
        }


            public async Task<IQueryable<Client>> GetClients()
        {
            var clients = await misCanchasDbContext.Clients.ToListAsync();
            var clientsq = clients.AsQueryable();
            return clientsq;
        }

        public async Task<Client> GetSingleClient(int id)
        {
            var singleClient = await misCanchasDbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == id);

            return singleClient;
        }
    }
}
