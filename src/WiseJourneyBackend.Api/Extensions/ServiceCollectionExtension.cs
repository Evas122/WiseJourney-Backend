namespace WiseJourneyBackend.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(configuration);

        return services;
    }
}