using Google.Maps.AddressValidation.V1;
using Google.Maps.Places.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Infrastructure.Extensions;

public static class GoogleExtension
{
    public static void AddGoogleExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<PlacesClient>(provider =>
        new PlacesClientBuilder { ApiKey = configuration["GoogleApi:PlaceApiKey"] }.Build());

        services.AddScoped<AddressValidationClient>(provider =>
            new AddressValidationClientBuilder { ApiKey = configuration["GoogleApi:PlaceApiKey"] }.Build());

        services.AddScoped<IGooglePlacesService, GooglePlacesService>();
    }
}