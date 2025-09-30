using DomainData.Interfaces;
using DomainData.Models;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    public SpellTestDbContext Context { get; init; }

    public RefreshTokenRepository(SpellTestDbContext context)
    {
        Context = context;
    }

    public async Task<RefreshToken> GetRefreshTokenByTokenAsync(string token)
    {
        return await Context.RefreshTokens.FirstAsync(rt => rt.Token == token);
    }

    public async Task<RefreshToken> GetRefreshTokenIncludeUserAndRolesByRefreshTokenAndClientIdAsync(string refreshToken, int clientId)
    {
        RefreshToken result = await Context.RefreshTokens
            .Include(rt => rt.AssociatedUser)
            .ThenInclude(u => u.Roles)
            .FirstAsync(rt => rt.Token == refreshToken && rt.AssociatedClientId == clientId);
        return result;
    }

    public async Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        RefreshToken result = Context.RefreshTokens.Add(refreshToken).Entity;
        await Context.SaveChangesAsync();
        return result;
    }

}
