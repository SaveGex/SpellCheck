using DomainData.Interfaces;
using DomainData.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

public class TokenService : ITokenService
{
    private IConfiguration Configuration { get; init; }

    public TokenService(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public int GetAccessTokenExperationMinutes()
    {
        int result = int.TryParse(Configuration["JWT:AccessTokenExpirationMinutes"], out var val) ? val : throw new InvalidOperationException("JWT:AccessTokenExpirationMinutes must be configured");
        return result;
    }

    public Task<string> GenerateAccessTokenAsync(User user, IEnumerable<string> roleNames, out Guid jwtId, Client client)
    {
        string? JwtTokenKeyValue;
        var tokenHandler = new JwtSecurityTokenHandler();
        {
            var JwtTokenKeyWord = Configuration["JWT:KeyWord"];
            if (string.IsNullOrWhiteSpace(JwtTokenKeyWord))
                throw new InvalidConfigurationException("JWT KeyWord is not configured");

            JwtTokenKeyValue = Configuration.GetValue<string>(JwtTokenKeyWord);
            if (string.IsNullOrWhiteSpace(JwtTokenKeyValue))
                throw new InvalidConfigurationException("JWT Key is not configured");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenKeyValue));

        jwtId = Guid.NewGuid();

        string issuer = Configuration.GetValue<string>("JWT:Issuer") ?? throw new InvalidOperationException("JWT:Issuer must be configured.");
        int accessTokenExpirationMinutes = int.TryParse(Configuration["JWT:AccessTokenExpirationMinutes"], out var val) ? val : 15;

        Claim loginCredentialsClaim = user switch
        {
            { Email: not null } => new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            { Number: not null } => new Claim(JwtRegisteredClaimNames.PhoneNumber, user.Number!),
            _ => throw new Exception("User has no email or phone number."),
        };

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

            new Claim(JwtRegisteredClaimNames.Jti, jwtId.ToString()),

            loginCredentialsClaim,

            new Claim(JwtRegisteredClaimNames.Iss, issuer),

            new Claim("client_id", client.ClientId)
        };

        claims.AddRange(roleNames.Select(roleName => new Claim(ClaimTypes.Role, roleName)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Expires = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes),

            SigningCredentials = creds,

            Issuer = issuer,

            Audience = client.URL
        };


        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Task.FromResult(tokenHandler.WriteToken(token));
    }

    public Task<RefreshToken> GenerateRefreshTokenAsync(int userId, Guid jwtId, Client client, string ipAddress)
    {
        var randomBytes = new byte[64];
        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        var refreshTokenExpirationDays = int.TryParse(Configuration["JWT:RefreshTokenExpirationDays"], out var val) ? val : throw new InvalidOperationException("JWT:RefreshTokenExpirationDays must be configured");

        return Task.FromResult(new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),

            JwtId = jwtId,

            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),

            CreatedAt = DateTime.UtcNow,

            AssociatedUserId = userId,

            AssociatedClientId = client.Id,

            IsRevoked = false,

            RevokedAt = null,

            CreatedByIp = ipAddress
        });

    }
}
