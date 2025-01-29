using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Commands.GeneratePlaces;

namespace WiseJourneyBackend.Api.Controllers;

[Authorize]
public class RecommendationController : BaseController
{
    private readonly IMediator _mediator;

    public RecommendationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("get-recommended-places")]
    public async Task<IActionResult> GenerateRecommendedPlacesAsync(GeneratePlacesCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}