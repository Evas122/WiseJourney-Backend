using MediatR;
using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;
using WiseJourneyBackend.Application.Queries.ChatHistory;

namespace WiseJourneyBackend.Api.Controllers;
public class RecommendationController : BaseController
{
    private readonly IMediator _mediator;

    public RecommendationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendUserMessageToChat(SendPreferenceMessageCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("chat-history")]
    public async Task<IActionResult> GetUserChatHistory()
    {
        var result = await _mediator.Send(new GetChatHistoryQuery());

        return Ok(result);
    }
}