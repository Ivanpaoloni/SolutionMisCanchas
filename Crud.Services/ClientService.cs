using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Dtos.Client;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Services
{
    public class ClientService : IClientService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;
        private readonly IMapper _mapper;
        public ClientService(MisCanchasDbContext dbContext, IMapper mapper)
        {
            this.misCanchasDbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<int> Create(ClientCreateDto client, bool saveChanges = false)
        {
            var id = await Create(_mapper.Map<Client>(client), false);
            if (saveChanges) await misCanchasDbContext.SaveChangesAsync();
            return id;
        }
        internal async Task<int> Create(Client client, bool saveChanges = false)
        {
            var result = await misCanchasDbContext.AddAsync(client);
            if (saveChanges) await misCanchasDbContext.SaveChangesAsync();

            return result.Entity.ClientId;
        }
        public async Task Delete(int id, bool saveChanges = false)
        {
            var client = await misCanchasDbContext.Clients.FindAsync(id);
            if (client == null) throw new ArgumentException("Client not found");

            misCanchasDbContext.Clients.Remove(client);
            if (saveChanges) await misCanchasDbContext.SaveChangesAsync();
        }

        public async Task Update(ClientUpdateDto client, bool saveChanges = false)
        {
            await Update(_mapper.Map<Client>(client), false);

            if (saveChanges) await misCanchasDbContext.SaveChangesAsync();
        }
        internal async Task Update(Client client, bool saveChanges = false)
        {
            var clientEdit = await misCanchasDbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == client.ClientId);

            if (clientEdit == null) throw new ArgumentException("Client not found");

            clientEdit.ClientName = client.ClientName;
            clientEdit.NationalIdentityDocument = client.NationalIdentityDocument;
            clientEdit.ClientEmail = client.ClientEmail;
            clientEdit.ClientTelephone = client.ClientTelephone;

            misCanchasDbContext.Clients.Update(clientEdit);
            if (saveChanges) await misCanchasDbContext.SaveChangesAsync();

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

            if (singleClient == null)
            {
                // Opción 1: Lanza una excepción
                throw new Exception("Cliente no encontrado");
                // Opción 2: Devuelve un valor predeterminado
                // return new Client();
            }
            return singleClient;
        }

    }
}
