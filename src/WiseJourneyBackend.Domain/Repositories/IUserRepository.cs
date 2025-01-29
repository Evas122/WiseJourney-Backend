using WiseJourneyBackend.Domain.Entities.Auth;

namespace WiseJourneyBackend.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> IsEmailExistsAsync(string email);
    Task<bool> IsEmailConfirmedAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
}