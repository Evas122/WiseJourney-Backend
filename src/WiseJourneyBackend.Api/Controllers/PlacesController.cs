using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Api.Controllers;

public class PlacesController : BaseController
{
    private readonly GooglePlacesService _placesService;

    public PlacesController(GooglePlacesService placesService)
    {
        _placesService = placesService;
    }
}