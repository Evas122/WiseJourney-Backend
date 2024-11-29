using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WiseJourneyBackend.Application.Cache;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;
using WiseJourneyBackend.Application.Dtos.Recommendation;
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
        var cacheKey = GetChatHistoryCacheKey();
        var chatHistoryCacheData = GetOrInitializeChatHistory();

        _cacheService.AddItemToLimitedList(chatHistoryCacheData.UserMessages, command.Message, 13);
        _cacheService.SetCache(cacheKey, chatHistoryCacheData);

        var interLeavedMessages = _cacheService.InterleaveChatHistoryMessages(chatHistoryCacheData);

        var chatHistoryJson = JsonConvert.SerializeObject(interLeavedMessages);

        var assistantResponse = await _kernelService.InvokeAsync(prompts["UserPreferencesChat"], new() { { "chat_history", chatHistoryJson } });

        _cacheService.AddItemToLimitedList(chatHistoryCacheData.AssistantMessages, assistantResponse, 13);
        _cacheService.SetCache(cacheKey, chatHistoryCacheData);
    }

    public Task<ChatHistoryCacheData> GetChatHistoryCacheData()
    {
        var firstAssistantMessage = _configuration["ChatHistoryMessage:FirstMessagePolish"] ?? throw new ConfigurationException("The message has not been set.");

        var chatHistoryCacheData = GetOrInitializeChatHistory(firstAssistantMessage);

        return Task.FromResult(chatHistoryCacheData);
    }

    public async Task<UserPreferencesDto> GenerateUserPreferencesAsync()
    {
        var prompts = _kernelService.ImportAllPlugins();
        var chatHistoryCacheData = GetOrInitializeChatHistory();

        var lastAssistantMessage = chatHistoryCacheData.AssistantMessages.LastOrDefault();

        var userPreferencesDtoJson = await _kernelService.InvokeAsync(prompts["GetUserPreferences"], new() { { "last_assistant_message", lastAssistantMessage } });

        var validJson = ValidJson(userPreferencesDtoJson);

        var userPreferencesDto = JsonConvert.DeserializeObject<UserPreferencesDto>(validJson) ?? throw new BadRequestException("A problem has occured");

        return userPreferencesDto;
    }

    private ChatHistoryCacheData GetOrInitializeChatHistory(string defaultAssistantMessage = "", int maxItems = 13)
    {
        var cacheKey = GetChatHistoryCacheKey();

        var chatHistoryCacheData = _cacheService.GetCache<ChatHistoryCacheData>(cacheKey) ?? new ChatHistoryCacheData();

        if (!chatHistoryCacheData.AssistantMessages.Any() && !string.IsNullOrWhiteSpace(defaultAssistantMessage))
        {
            _cacheService.AddItemToLimitedList(chatHistoryCacheData.AssistantMessages, defaultAssistantMessage, maxItems);
            _cacheService.SetCache(cacheKey, chatHistoryCacheData);
        }
        return chatHistoryCacheData;
    }

    private string GetChatHistoryCacheKey()
    {
        var chatHistoryKey = _configuration["Cache:ChatHistoryKey"] ?? throw new ConfigurationException("Invalid Cache Key");
        return _cacheService.GetCacheKey(chatHistoryKey);
    }

    private string GetChatHistoryInterleaveMessages(ChatHistoryCacheData chatHistoryCacheData)
    {
        var interLeavedMessages = _cacheService.InterleaveChatHistoryMessages(chatHistoryCacheData);

        return JsonConvert.SerializeObject(interLeavedMessages);
    }

    private string ValidJson(string json)
    {
        var jsonStartIndex = json.IndexOf('{');
        var jsonEndIndex = json.LastIndexOf('}') + 1;

        return json.Substring(jsonStartIndex, jsonEndIndex - jsonStartIndex);
    }
}