using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DbManagerApi.Extentions
{
    public class JwtBearerOptionsConfigurator : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly Infrastructure.Configuration.JwtConfigOptions _jwtConfig;

        public JwtBearerOptionsConfigurator(Infrastructure.Configuration.JwtConfigOptions jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            if (name != JwtBearerDefaults.AuthenticationScheme) return;

            options.TokenValidationParameters = new()
            {
                ValidateIssuer = _jwtConfig.ValidateIssuer,
                ValidateLifetime = _jwtConfig.ValidateLifetime,
                ValidateIssuerSigningKey = _jwtConfig.ValidateIssuerSigningKey,
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
                ValidateAudience = _jwtConfig.ValidateAudience
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var db = context.HttpContext.RequestServices
                        .GetRequiredService<Infrastructure.DB.SpellTestDbContext>();

                    var audiences = await db.Clients
                        .Select(c => c.URL)
                        .ToListAsync();

                    var jwtAudiences = context.Principal?
                        .FindAll("aud")
                        .Select(c => c.Value)
                        .ToList();

                    if (jwtAudiences == null || !jwtAudiences.Any())
                    {
                        context.Fail("Token has no audience claim.");
                        return;
                    }

                    if (!jwtAudiences.Any(aud => audiences.Contains(aud)))
                        context.Fail("Token audience not allowed.");
                }
            };
        }

        public void Configure(JwtBearerOptions options) =>
            Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }
}
