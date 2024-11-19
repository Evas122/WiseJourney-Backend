using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WiseJourneyBackend.Application.Cache;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Infrastructure.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Services;

public class RecommendationService : IRecommendationService
{
    private readonly IKernelService _kernelService;
    private readonly ICacheService _cacheService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;

    public RecommendationService(IKernelService kernelService,ICacheService cacheService, IHttpContextAccessor contextAccessor, IConfiguration configuration)
    {
        _kernelService = kernelService;
        _cacheService = cacheService;
        _contextAccessor = contextAccessor;
        _configuration = configuration;
    }

    public async Task SendUserPreferencesMessageAsync(SendPreferenceMessageCommand command)
    {
        var prompts = _kernelService.ImportAllPlugins();
        var chatHistoryKey = _configuration["Cache:ChatHistoryKey"] ?? throw new ConfigurationException("Invalid Cache Key");
        var cacheKey = _cacheService.GetCacheKey(chatHistoryKey);
        var chatHistoryCacheData = _cacheService.GetCache<ChatHistoryCacheData>(cacheKey) ?? new ChatHistoryCacheData();

        _cacheService.AddItemToLimitedList(chatHistoryCacheData.UserMessages, command.Message, 10);
        _cacheService.SetCache(cacheKey, chatHistoryCacheData);

        var chatHistory = _cacheService.GetCache<ChatHistoryCacheData>(cacheKey);

        var assistantResponse = await _kernelService.InvokeAsync(prompts["UserPreferences"], new() { { "chat_history", chatHistory } });

        _cacheService.AddItemToLimitedList(chatHistoryCacheData.AssistantMessages, assistantResponse, 10);
        _cacheService.SetCache(cacheKey, chatHistoryCacheData);

    }
}