using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Dtos.Recommendation;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Api.Controllers;

public class PlacesController : BaseController
{
    private readonly IGooglePlacesService _placesService;

    public PlacesController(GooglePlacesService placesService)
    {
        _placesService = placesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlaces(GooglePlacesPreferencesDto googlePlacesPreferencesDto)
    {
        var result = await _placesService.GetNearbyPlacesAsync(googlePlacesPreferencesDto);

        return Ok(result);
    }
}