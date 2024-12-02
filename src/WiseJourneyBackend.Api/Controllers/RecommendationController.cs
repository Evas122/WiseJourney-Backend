using MediatR;
using Microsoft.AspNetCore.Mvc;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Queries.ChatHistory;
using WiseJourneyBackend.Application.Queries.GetRecommendedPlaces;

namespace WiseJourneyBackend.Api.Controllers;
public class RecommendationController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IRecommendationService _recommendationService;

    public RecommendationController(IMediator mediator, IRecommendationService recommendationService)
    {
        _mediator = mediator;
        _recommendationService = recommendationService;
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

    [HttpGet("get-recommended-places")]
    public async Task<IActionResult> GetRecommendedPlacesForUserAsync()
    {
        var result = await _mediator.Send(new GetRecommendedPlacesQuery());

        return Ok(result);
    }
}