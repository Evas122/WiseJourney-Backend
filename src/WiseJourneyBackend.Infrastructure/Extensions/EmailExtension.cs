using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiseJourneyBackend.Infrastructure.Interfaces;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Infrastructure.Extensions;

public static class EmailExtension
{
    public static void AddEmailExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:SenderName"])
            .AddSmtpSender(configuration["Email:Host"],
            configuration.GetValue<int>("Email:Port"),
            configuration["Email:User"],
            configuration["Email:Password"]);

        services.AddScoped<IEmailService, EmailService>();
    }
}