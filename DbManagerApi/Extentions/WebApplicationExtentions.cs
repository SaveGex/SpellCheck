using Application.Interfaces;
using Application.ModelsDTO;
using Microsoft.Build.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace DbManagerApi.Extentions
{
    public static class WebApplicationExtentions
    {
        extension (WebApplication app)
        {
            public IApplicationBuilder ExecuteMigrations()
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<Infrastructure.DB.SpellTestDbContext>();
                db.Database.Migrate();
                return app;
            }

            public async Task<IApplicationBuilder> RegisterAdminUserAsync()
            {
                using var scope = app.Services.CreateScope();
                var apiAdminSection = app.Configuration.GetRequiredSection("APIAdministrator");
                bool registerAdministrator = apiAdminSection.GetValue<bool?>("RegisterAdministrator")
                    ?? throw new InvalidConfigurationException("APIAdministrator:RegisterAdministrator is not configured");

                if (!registerAdministrator)
                    return app;

                var logger = scope.ServiceProvider.GetService<Infrastructure.Logging.Interfaces.IFileLogger>();

                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var registerAdminDTO = new UserRegisterDTO()
                {
                    Email = apiAdminSection.GetValue<string>("Email") ?? throw new InvalidConfigurationException("APIAdministrator:Email is not configured"),
                    Password = apiAdminSection.GetValue<string>("Password") ?? throw new InvalidConfigurationException("APIAdministrator:Password is not configured"),
                    Username = apiAdminSection.GetValue<string>("Username") ?? throw new InvalidConfigurationException("APIAdministrator:Username is not configured"),
                };

                try
                {
                    await authService.RegisterUserAsync(registerAdminDTO, DomainData.Roles.RoleNames.Admin);
                    logger?.LogDebug("Admin user has been registered");
                }
                catch (Exception ex)
                {
                    logger?.LogError("Admin user already exists: " + ex.Message);
                }

                return app;
            }

            public async Task<IApplicationBuilder> RegisterTestClientAsync()
            {
                using var scope = app.Services.CreateScope();
                var clientApiAdminSection = app.Configuration.GetRequiredSection("APIAdministrator:Client");
                bool registerTestClient = clientApiAdminSection.GetValue<bool?>("RegisterTestClient")
                    ?? throw new InvalidConfigurationException("APIAdministrator:Client:RegisterTestClient is not configured");

                if(!registerTestClient)
                    return app;

                var logger = scope.ServiceProvider.GetService<Infrastructure.Logging.Interfaces.IFileLogger>();
                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                var registerClientDTO = new ClientRequestDTO()
                {
                    ClientId = clientApiAdminSection.GetValue<string>("Id") ?? throw new InvalidConfigurationException("APIAdministrator:Client:Id is not configured"),
                    Name = clientApiAdminSection.GetValue<string>("Name") ?? throw new InvalidConfigurationException("APIAdministrator:Client:Name is not configured"),
                    URL = clientApiAdminSection.GetValue<string>("URL") ?? throw new InvalidConfigurationException("APIAdministrator:Client:URL is not configured"),
                };

                try
                {
                    await clientService.AddClientAsync(registerClientDTO);
                    logger?.LogDebug("Test client registered");
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex.Message);
                }

                return app;
            }

        }
    }
}
