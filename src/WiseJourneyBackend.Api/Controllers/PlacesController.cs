using MediatR;
using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Commands.SavePlaces;
using WiseJourneyBackend.Application.Queries.GetRecommendedPlaces;

namespace WiseJourneyBackend.Api.Controllers;

public class PlacesController : BaseController
{
    private readonly IMediator _mediator;

    public PlacesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-recommended-places")]
    public async Task<IActionResult> GetPlaces()
    {
        var result = await _mediator.Send(new GetRecommendedPlacesQuery());

        return Ok(result);
    }

    [HttpPost("save-places")]
    public async Task<IActionResult> SavePlaces(SavePlacesCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}