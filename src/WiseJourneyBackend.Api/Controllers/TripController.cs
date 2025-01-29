using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Commands.CreateTrip;
using WiseJourneyBackend.Application.Commands.DeleteTrip;
using WiseJourneyBackend.Application.Queries.GetPagedTrips;
using WiseJourneyBackend.Application.Queries.GetTripDetails;

namespace WiseJourneyBackend.Api.Controllers;

[Authorize]
public class TripController : BaseController
{
    private readonly IMediator _mediator;

    public TripController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-trip")]
    public async Task<IActionResult> CreateTrip(CreateTripCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("trips")]
    public async Task<IActionResult> GetTrips([FromQuery] GetPagedTripsQuery query)
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTripDetails([FromRoute] Guid id)
    {
        var query = new GetTripDetailsQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteTrip([FromRoute] Guid id)
    {
        var command = new DeleteTripCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}