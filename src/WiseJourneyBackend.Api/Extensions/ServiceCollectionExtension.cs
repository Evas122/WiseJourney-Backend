namespace WiseJourneyBackend.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCorsExtension(configuration);

        return services;
    }
}