using Microsoft.Extensions.DependencyInjection;

namespace WiseJourneyBackend.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationExtensions(this IServiceCollection services)
    {
        services.AddFluentValidationExtension();

        return services;
    }
}