using MediatR;
using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Commands.CreateTrip;
using WiseJourneyBackend.Application.Queries.GetPagedTrips;
using WiseJourneyBackend.Application.Queries.GetTripDetails;

namespace WiseJourneyBackend.Api.Controllers;

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
    public async Task<IActionResult> GetTrips(GetPagedTripsQuery query)
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("trips-details")]
    public async Task<IActionResult> GetTripDetails(GetTripDetailsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}