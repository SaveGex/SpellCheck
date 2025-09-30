using DomainData.Models;

namespace DomainData.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken> GetRefreshTokenByTokenAsync(string token);
    Task<RefreshToken> GetRefreshTokenIncludeUserAndRolesByRefreshTokenAndClientIdAsync(string refreshToken, int clientId);
}
