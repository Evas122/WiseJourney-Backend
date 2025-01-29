using WiseJourneyBackend.Domain.Entities.Auth;

namespace WiseJourneyBackend.Infrastructure.Interfaces;

public interface IEmailService
{
    Task SendResetPasswordEmail(User user, string token);
    Task SendVerificationEmail(User user, string token);
}