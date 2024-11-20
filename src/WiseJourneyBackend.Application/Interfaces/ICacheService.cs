using WiseJourneyBackend.Application.Cache;

namespace WiseJourneyBackend.Application.Interfaces;

public interface ICacheService
{
    T? GetCache<T>(string key) where T : class;
    void RemoveCache(string key);
    void SetCache<T>(string key, T value) where T : class;
    void AddItemToLimitedList<T>(List<T> list, T item, int maxItems) where T : class;
    public string GetCacheKey(string key);
    List<string> InterleaveChatHistoryMessages(ChatHistoryCacheData chatHistory);
}