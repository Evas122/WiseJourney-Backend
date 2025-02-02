﻿using WiseJourneyBackend.Domain.Entities.Auth;

namespace WiseJourneyBackend.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenByUserIdAsync(Guid userId);
    Task UpdateAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenByTokenAsync(string refreshToken);
    Task<List<RefreshToken>> GetAllActiveTokensAsync(Guid userId);
    Task RemoveUserRefreshTokensAsync(List<RefreshToken> refreshTokens);
}