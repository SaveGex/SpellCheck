using DomainData.Interfaces;
using DomainData.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    public SpellTestDbContext Context { get; init; }

    public ClientRepository(SpellTestDbContext context)
    {
        Context = context;
    }

    public async Task<Client> GetClientByClientIdAsync(string clientId)
    {
        Client? result = await Context.Clients
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.ClientId == clientId && c.IsActive);
        if (result == null)
        {
            throw new Exception($"Client with clientId '{clientId}' does not found");
        }
        return result;
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        return await Context.Clients.AsNoTracking().ToListAsync();
    }

    public async Task<Client> AddClientAsync(Client client)
    {
        Client result = Context.Clients.Add(client).Entity;
        await Context.SaveChangesAsync();
        return result;
    }

    public async Task<Client> UpdateClientAsync(int destId, Client client)
    {
        Client existingClient = await Context.Clients.FirstAsync(c => c.Id == destId);
        Context.Entry(existingClient).CurrentValues.SetValues(client);
        Context.Entry(existingClient).State = EntityState.Modified;
        await Context.SaveChangesAsync();

        return existingClient;
    }
}
