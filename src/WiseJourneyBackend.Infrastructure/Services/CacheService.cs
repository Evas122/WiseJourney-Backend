﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using WiseJourneyBackend.Application.Cache;
using WiseJourneyBackend.Application.Extensions;
using WiseJourneyBackend.Application.Interfaces;

namespace WiseJourneyBackend.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpContextAccessor _contextAccessor;

    public CacheService(IMemoryCache memoryCache, IHttpContextAccessor contextAccessor)
    {
        _memoryCache = memoryCache;
        _contextAccessor = contextAccessor;
    }

    public T? GetCache<T>(string key) where T : class
    {
        var cacheKey = GetCacheKey(key);
        return _memoryCache.TryGetValue(cacheKey, out T? value) ? value : null;
    }

    public void SetCache<T>(string key, T value) where T : class
    {
        var cacheKey = GetCacheKey(key);
        _memoryCache.Set(cacheKey, value, TimeSpan.FromMinutes(20));
    }

    public void RemoveCache(string key)
    {
        var cacheKey = GetCacheKey(key);
        _memoryCache.Remove(cacheKey);
    }

    public void AddItemToLimitedList<T>(List<T> list, T item, int maxItems) where T : class
    {
        list.Add(item);
        if(list.Count > maxItems)
        {
            list.RemoveRange(0, list.Count -  maxItems);
        }
    }

    public string GetCacheKey(string key)
    {
        var userId = _contextAccessor.GetUserId();
        var cacheKey = key + userId;

        return cacheKey;
    }

    public List<string> InterleaveChatHistoryMessages(ChatHistoryCacheData chatHistory)
    {
        var interleavedMessages = new List<string>();
        var userMessages = chatHistory.UserMessages;
        var assistantMessages = chatHistory.AssistantMessages;

        int maxLength = Math.Max(userMessages.Count, assistantMessages.Count);

        for (int i = 0; i < maxLength; i++)
        {
            if (i < userMessages.Count)
            {
                interleavedMessages.Add($"User: {userMessages[i]}");
            }
            if (i < assistantMessages.Count)
            {
                interleavedMessages.Add($"Assistant: {assistantMessages[i]}");
            }
        }
        return interleavedMessages;
    }
}