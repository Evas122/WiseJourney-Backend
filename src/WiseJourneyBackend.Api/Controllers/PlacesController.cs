using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetPlaces(string place)
    {
        var result = await _placesService.GetNearbyPlaces(place);

        return Ok(result);
    }
}