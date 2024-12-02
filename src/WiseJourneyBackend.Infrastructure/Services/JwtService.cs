using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Entities;
using WiseJourneyBackend.Domain.Enums;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Infrastructure.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtService(IConfiguration configuration, IDateTimeProvider dateTimeProvider)
    {
        _configuration = configuration;
        _dateTimeProvider = dateTimeProvider;
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

    public List<Claim> GetClaims(User user, TokenType? tokenType = null)
    {
        if (user == null)
        {
            throw new ArgumentIsNullException(nameof(user), "The user object must not be null");
        }

        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
            };

        if (tokenType.HasValue)
        {
            claims.Add(new Claim("TokenType", tokenType.Value.ToString()));
        }

        return claims;
    }

    public string GenerateEmailVerificationToken(User user)
    {
        var claims = GetClaims(user, TokenType.EmailConfirmation);

        return GenerateToken(claims);
    }

    public string GeneratePasswordResetToken(User user)
    {
        var claims = GetClaims(user, TokenType.PasswordReset);

        return GenerateToken(claims);
    }

    private string GenerateToken(List<Claim> claims)
    {
        var secretKey = _configuration["Jwt:SecretKey"];
        var expire = _configuration.GetValue<double>("Jwt:AccessTokenExpirationMinutes");

        var key = secretKey != null
            ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            : throw new ConfigurationException("JWT secret key is not configured or is empty.");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = _dateTimeProvider.UtcNow.AddMinutes(expire),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var secretKey = _configuration["Jwt:SecretKey"];
        var key = secretKey != null
           ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
           : throw new ConfigurationException("JWT secret key is not configured or is empty.");

        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

        if (securityToken is JwtSecurityToken jwtToken &&
            jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return principal;
        }

        return null;
    }
}