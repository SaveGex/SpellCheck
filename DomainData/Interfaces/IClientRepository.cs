using DomainData.Models;

namespace DomainData.Interfaces;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllClientsAsync();
    Task<Client> GetClientByClientIdAsync(string clientId);
    Task<Client> AddClientAsync(Client client);
    Task<Client> UpdateClientAsync(int destId, Client client);
    Task<bool> ExistsAsync(Client client);
    Task<bool> ExistsAsync(string clientId);
}
