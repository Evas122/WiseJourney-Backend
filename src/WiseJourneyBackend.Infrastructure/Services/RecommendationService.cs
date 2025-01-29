using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using WiseJourneyBackend.Application.Commands.GeneratePlaces;
using WiseJourneyBackend.Application.Dtos.Places;
using WiseJourneyBackend.Application.Dtos.Recommendation;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Infrastructure.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Services;

public class RecommendationService : IRecommendationService
{
    private readonly IKernelService _kernelService;
    private readonly ICacheService _cacheService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IGooglePlacesService _googlePlacesService;

    public RecommendationService(IKernelService kernelService,ICacheService cacheService, IHttpContextAccessor contextAccessor, IConfiguration configuration, IGooglePlacesService googlePlacesService)
    {
        _kernelService = kernelService;
        _cacheService = cacheService;
        _contextAccessor = contextAccessor;
        _configuration = configuration;
        _googlePlacesService = googlePlacesService;
    }

    public async Task<List<PlaceDto>> GetRecommendedPlacesAsync(GeneratePlacesCommand command)
    {
        var prompts = _kernelService.ImportAllPlugins();

        var userPreferencesEnglishJson = await ConvertUserPreferencesToEnglish(command, prompts);

        var googlePlacesQuery = await ConvertUserPreferencesToGoogleQuery(userPreferencesEnglishJson, prompts);

        var places = await _googlePlacesService.GetNearbyPlacesAsync(googlePlacesQuery);

        return places;
    }

    private async Task<GooglePlacesQuery> ConvertUserPreferencesToGoogleQuery(string userPreferencesEnglishJson, KernelPlugin prompts)
    {
        var googlePlacesPreferencesDtoJson = await _kernelService.InvokeAsync(prompts["UserPreferencesToGooglePlacesQuery"], new() { { "user_preferences", userPreferencesEnglishJson } });

        var validJson = ValidJson(googlePlacesPreferencesDtoJson);

        var googlePlacesPreferencesDto = JsonConvert.DeserializeObject<GooglePlacesQuery>(validJson) ?? throw new BadRequestException("A problem has occured");

        return googlePlacesPreferencesDto;
    }

    private async Task<string> ConvertUserPreferencesToEnglish(GeneratePlacesCommand command, KernelPlugin prompts)
    {
        var userPreferencesJson = JsonConvert.SerializeObject(command);

        var englishUserPreferencesJson = await _kernelService.InvokeAsync(prompts["UserPreferencesToEnglish"], new() { { "user_preferences", userPreferencesJson } });

        return englishUserPreferencesJson;
    }

    private string ValidJson(string json)
    {
        var jsonStartIndex = json.IndexOf('{');
        var jsonEndIndex = json.LastIndexOf('}') + 1;

        return json[jsonStartIndex..jsonEndIndex];
    }
}