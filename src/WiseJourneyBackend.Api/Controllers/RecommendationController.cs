using MediatR;
using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;
using WiseJourneyBackend.Application.Queries.ChatHistory;
using WiseJourneyBackend.Application.Queries.GetUserPreferences;

namespace WiseJourneyBackend.Api.Controllers;
public class RecommendationController : BaseController
{
    private readonly IMediator _mediator;

    public RecommendationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendUserMessageToChatAsync(SendPreferenceMessageCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("chat-history")]
    public async Task<IActionResult> GetUserChatHistoryAsync()
    {
        var result = await _mediator.Send(new GetChatHistoryQuery());

        return Ok(result);
    }

    [HttpGet("user-preferences")]
    public async Task<IActionResult> GetUserPreferencesAsync()
    {
        var result = await _mediator.Send(new GetUserPreferencesQuery());

        return Ok(result);
    }
}