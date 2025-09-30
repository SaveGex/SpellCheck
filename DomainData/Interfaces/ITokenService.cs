
using DomainData.Models;

namespace DomainData.Interfaces
{
    public interface ITokenService
    {
        int GetAccessTokenExperationMinutes();

        Task<string> GenerateAccessTokenAsync(User user, IEnumerable<string> roles, out Guid jwtId, Client client);
        Task<RefreshToken> GenerateRefreshTokenAsync(int userId, Guid jwtId, Client client, string ipAddress);
    }
}
