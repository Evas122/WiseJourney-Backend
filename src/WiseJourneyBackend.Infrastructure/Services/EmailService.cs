using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using WiseJourneyBackend.Domain.Entities;
using WiseJourneyBackend.Infrastructure.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IConfiguration _configuration;

    public EmailService(IFluentEmail fluentEmail, IConfiguration configuration)
    {
        _fluentEmail = fluentEmail;
        _configuration = configuration;
    }

    public async Task SendVerificationEmail(User user, string token)
    {
        string verificationLink = $"{_configuration["App:ConfirmEmail"]}{token}";

        await _fluentEmail
            .To(user.Email)
            .Subject("Email verification for WiseJourney")
            .Body($"To verify your email address <a href='{verificationLink}'>click here</a>", isHtml: true)
            .SendAsync();
    }

    public async Task SendResetPasswordEmail(User user, string token)
    {
        string resetPasswordLink = $"{_configuration["App:ResetPassword"]}{token}";

        await _fluentEmail
            .To(user.Email)
            .Subject("Reset password for WiseJourney")
            .Body($"To reset your password <a href='{resetPasswordLink}'>click here</a>", isHtml: true)
            .SendAsync();
    }
}