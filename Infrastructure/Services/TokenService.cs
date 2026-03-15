using DomainData.Interfaces;
using DomainData.Models;
using Infrastructure.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

internal class TokenService : ITokenService
{
    public JwtConfigOptions JwtConfig { get; set; }

    public TokenService(JwtConfigOptions jwtConfig)
    {
        JwtConfig = jwtConfig;
    }

    public int GetAccessTokenExperationMinutes()
        => (int)JwtConfig.AccessTokenExpirationInSeconds / 60;

    public Task<string> GenerateAccessTokenAsync(User user, IEnumerable<string> roleNames, out Guid jwtId, Client client)
    {
        string? JwtTokenKeyValue;
        var tokenHandler = new JwtSecurityTokenHandler();
        {
            JwtTokenKeyValue = JwtConfig.SecretKey;
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenKeyValue));

        jwtId = Guid.NewGuid();

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

            new Claim(JwtRegisteredClaimNames.Iss, JwtConfig.Issuer),

            new Claim("client_id", client.ClientId)
        };

        claims.AddRange(roleNames.Select(roleName => new Claim(ClaimTypes.Role, roleName)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Expires = DateTime.UtcNow.AddMinutes(GetAccessTokenExperationMinutes()),

            SigningCredentials = creds,

            Issuer = JwtConfig.Issuer,

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

        return Task.FromResult(new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),

            JwtId = jwtId,

            ExpiresAt = DateTime.UtcNow.AddDays(JwtConfig.RefreshTokenExpirationInSeconds / 60 / 60 / 24),

            CreatedAt = DateTime.UtcNow,

            AssociatedUserId = userId,

            AssociatedClientId = client.Id,

            IsRevoked = false,

            RevokedAt = null,

            CreatedByIp = ipAddress
        });

    }
}
