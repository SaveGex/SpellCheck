using DomainData.Models;

namespace Application.Interfaces
{
    public interface IClientCacheService
    {
        Task<Client?> GetClientByClientIdAsync(string clientId);
    }
}
