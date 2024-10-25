using Microsoft.Extensions.DependencyInjection;
using WiseJourneyBackend.Infrastructure.ExceptionsHandlers;

namespace WiseJourneyBackend.Infrastructure.Extensions;
public static class ExceptionsHandlersExtension
{
    public static IServiceCollection AddExceptionsHandlersExtension(this IServiceCollection services)
    {
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddExceptionHandler<ArgumentNullExceptionHandler>();
        services.AddExceptionHandler<ConfigurationExceptionHandler>();
        services.AddExceptionHandler<InvalidFormatExceptionHandler>();

        services.AddProblemDetails();

        return services;
    }
}