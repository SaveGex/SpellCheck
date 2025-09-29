using Application.Interfaces;
using DbManagerApi.Services.ModuleServices;
using DbManagerApi.Services.UserServices;
using DbManagerApi.Services.WordServices;
using DomainData.Interfaces;
using DomainData.Models;
using Infrastructure.DB;
using Infrastructure.DB.Interceptors;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        // DbContext
        services.AddSingleton<SoftDeleteInterceptor>();

        services.AddDbContext<SpellTestDbContext>((sp, options) => 
            options.AddInterceptors()
                .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>())
        );

        // Repositories
        services.AddTransient<IModuleRepository, ModuleRepository>();
        services.AddTransient<IWordRepository, WordRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IModelRepository, ModelRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IDifficultyLevelRepository, DifficultyLevelRepository>();
        services.AddTransient<IFriendsRepository, FriendsRepository>();
        services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddTransient<IClientRepository, ClientRepository>();
        
        // Other
        services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

        // Services
        services.AddSingleton<IClientCacheService, ClientCacheService>();
        services.AddTransient<ITokenService, TokenService>();
        return services;
    }
}
