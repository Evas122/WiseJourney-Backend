using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Entities;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Infrastructure.Data;
using WiseJourneyBackend.Infrastructure.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AppDbContext _dbContext;

    public JwtService(IConfiguration configuration, IDateTimeProvider dateTimeProvider, AppDbContext dbContext)
    {
        _configuration = configuration;
        _dateTimeProvider = dateTimeProvider;
        _dbContext = dbContext;
    }

    public string GenerateJwtToken(List<Claim> claims)
    {
        var secretKey = _configuration["Jwt:SecretKey"];
        var expire = _configuration.GetValue<double>("Jwt:AccessTokenExpirationMinutes");

        var key = secretKey != null
            ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            : throw new ConfigurationException("JWT secret key is not configured or is empty.");

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:issuer"],
            audience: _configuration["Jwt:audience"],
            claims: claims,
            expires: _dateTimeProvider.UtcNow.AddMinutes(expire),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public List<Claim> GetClaims(User user)
    {
        if (user == null)
        {
            throw new ArgumentIsNullException(nameof(user), "The user object must not be null");
        }

        return new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
            };
    }
}