using Microsoft.EntityFrameworkCore;
using WiseJourneyBackend.Domain.Entities;
using WiseJourneyBackend.Domain.Repositories;
using WiseJourneyBackend.Infrastructure.Data;

namespace WiseJourneyBackend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Email == email);
    }

    public async Task<bool> IsEmailConfirmedAsync(User user)
    {
        return await _dbContext.Users.AnyAsync(user => user.EmailConfirmed == true);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
    }
}