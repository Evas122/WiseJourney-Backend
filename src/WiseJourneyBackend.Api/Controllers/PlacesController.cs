using MediatR;
using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Commands.GeneratePlaces;
using WiseJourneyBackend.Application.Commands.SavePlaces;
using WiseJourneyBackend.Application.Queries.GetPhoto;

namespace WiseJourneyBackend.Api.Controllers;

public class PlacesController : BaseController
{
    private readonly IMediator _mediator;

    public PlacesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("save-places")]
    public async Task<IActionResult> SavePlaces(SavePlacesCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("photo/{photoId}")]
    public async Task<IActionResult> GetPhotoAsync(string photoId)
    {
        if (string.IsNullOrEmpty(photoId))
        {
            return BadRequest("Photo ID is required.");
        }

        var photoUrl = await _mediator.Send(new GetPhotoQuery(photoId));
        var decodedUrl = Uri.UnescapeDataString(photoUrl);

        return Ok(decodedUrl);
    }
}