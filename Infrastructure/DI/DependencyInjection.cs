using Application.Interfaces;
using DbManagerApi.Services.ModuleServices;
using DbManagerApi.Services.UserServices;
using DbManagerApi.Services.WordServices;
using DomainData.Interfaces;
using DomainData.Models;
using Infrastructure.Configuration;
using Infrastructure.DB;
using Infrastructure.DB.Interceptors;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(
        this WebApplicationBuilder builder)
    {
        // DbContext
        builder.Services.AddSingleton<SoftDeleteInterceptor>();

        builder.Services.AddDbContext<SpellTestDbContext>((sp, options) =>
            options.AddInterceptors()
                .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>())
        );

        // Repositories
        builder.Services.AddTransient<IModuleRepository, ModuleRepository>();
        builder.Services.AddTransient<IWordRepository, WordRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IModelRepository, ModelRepository>();
        builder.Services.AddTransient<IRoleRepository, RoleRepository>();
        builder.Services.AddTransient<IDifficultyLevelRepository, DifficultyLevelRepository>();
        builder.Services.AddTransient<IFriendsRepository, FriendsRepository>();
        builder.Services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddTransient<IClientRepository, ClientRepository>();

        // Other
        builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        builder.Services.AddSingleton(sp =>
        {
            var options = new JwtConfigOptions();
            builder.Configuration.GetSection("JWT").Bind(options);
            return options;
        });

        // builder.Services
        builder.Services.AddSingleton<IClientCacheService, ClientCacheService>();
        builder.Services.AddTransient<ITokenService, TokenService>();
        return builder;
    }
}
