using Application.Interfaces;
using DomainData.Interfaces;
using DomainData.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Services;

public class ClientCacheService : IClientCacheService
{
    private const string ClientCacheKeyPrefix = "Client_";

    private IConfiguration Configuration { get; init; }

    private IMemoryCache MemoryCache { get; init; }

    private IServiceProvider ServiceProvider { get; init; }

    public ClientCacheService(IConfiguration configuration, IMemoryCache memoryCache, IServiceProvider serviceProvider)
    {
        Configuration = configuration;
        MemoryCache = memoryCache;
        ServiceProvider = serviceProvider;
    }

    public async Task<Client?> GetClientByClientIdAsync(string clientId)
    {
        string cacheKey = ClientCacheKeyPrefix + clientId;
        if (MemoryCache.TryGetValue(cacheKey, out Client? cachedClient))
        {
            return cachedClient;
        }

        var scope = ServiceProvider.CreateScope();
        var ClientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

        cachedClient = await ClientRepository.GetClientByClientIdAsync(clientId);

        int cacheExpirationTime = Configuration.GetRequiredSection("CacheSettings").GetValue<int>("ClientCacheExpirationTimeInMinutes");

        if (cachedClient != null)
        {
            MemoryCache.Set(cacheKey, cachedClient, TimeSpan.FromMinutes(60));
        }

        return cachedClient;
    }
}
