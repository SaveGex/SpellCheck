using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace Infrastructure.Services
{
    public static class JWTRequirementsProviderExtention
    {
        public static string GetJWTSigningSecretValue(this IConfiguration configuration)
        {
            var JwtTokenKeyWord = configuration["JWT:KeyWord"];
            if (string.IsNullOrWhiteSpace(JwtTokenKeyWord))
                throw new InvalidConfigurationException("JWT KeyWord is not configured");

            var JwtTokenKeyValue = configuration.GetValue<string>(JwtTokenKeyWord);
            if (string.IsNullOrWhiteSpace(JwtTokenKeyValue))
                throw new InvalidConfigurationException("JWT Key is not configured");

            return JwtTokenKeyValue;
        }


        public static async Task<IEnumerable<string>> GetClientURLsAsync(this IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var DB = serviceProvider.GetService(typeof(SpellTestDbContext)) as SpellTestDbContext;
            if (DB == null)
            {
                throw new InvalidOperationException("Database context is not available.");
            }

            List<string> clientUrls = await DB.Clients.Select(c => c.URL).ToListAsync();
            return clientUrls;
        }
    }
}
