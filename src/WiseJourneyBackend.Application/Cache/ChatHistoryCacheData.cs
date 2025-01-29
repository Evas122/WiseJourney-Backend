namespace WiseJourneyBackend.Application.Cache;

public class ChatHistoryCacheData
{
    public List<string> UserMessages { get; set; } = [];
    public List<string> AssistantMessages { get; set; } = [];
}