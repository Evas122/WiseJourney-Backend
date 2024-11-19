using MediatR;
using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;

namespace WiseJourneyBackend.Api.Controllers;
public class RecommendationController : BaseController
{
    private readonly IMediator _mediator;

    public RecommendationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sendMessage")]
    public async Task<IActionResult> SendUserMessageToChat(SendPreferenceMessageCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}