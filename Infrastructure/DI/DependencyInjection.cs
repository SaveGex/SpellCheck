using DbManagerApi.Services.ModuleServices;
using DbManagerApi.Services.UserServices;
using DbManagerApi.Services.WordServices;
using DomainData;
using DomainData.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddDbContext<SpellTestDbContext>();

        services.AddTransient<IModuleRepository, ModuleRepository>();
        services.AddTransient<IWordRepository, WordRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IModelRepository, ModelRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IDifficultyLevelRepository, DifficultyLevelRepository>();

        return services;
    }
}
