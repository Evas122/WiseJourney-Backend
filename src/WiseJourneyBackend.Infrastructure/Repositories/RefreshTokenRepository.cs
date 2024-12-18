using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Entities.Auth;
using WiseJourneyBackend.Domain.Repositories;
using WiseJourneyBackend.Infrastructure.Data;

namespace WiseJourneyBackend.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RefreshTokenRepository(AppDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }
    public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(Guid userId)
    {
        return await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId && rt.ExpiresAtUtc > _dateTimeProvider.UtcNow && !rt.IsRevoked);
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Update(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string refreshToken)
    {
        return await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiresAtUtc > _dateTimeProvider.UtcNow && !rt.IsRevoked); 
    }

    public async Task<List<RefreshToken>> GetAllActiveTokensAsync(Guid userId)
    {
        return await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();
    }

    public async Task RemoveUserRefreshTokensAsync(List<RefreshToken> refreshTokens)
    {
        _dbContext.RefreshTokens.RemoveRange(refreshTokens);
        await _dbContext.SaveChangesAsync();
    }
}