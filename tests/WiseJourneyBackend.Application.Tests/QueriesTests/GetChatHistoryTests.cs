using Moq;
using WiseJourneyBackend.Application.Cache;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Application.Queries.ChatHistory;

namespace WiseJourneyBackend.Application.Tests.QueriesTests;

public class GetChatHistoryHandlerTests
{
    private readonly Mock<IRecommendationService> _recommendationServiceMock;
    private readonly GetChatHistoryHandler _handler;

    public GetChatHistoryHandlerTests()
    {
        _recommendationServiceMock = new Mock<IRecommendationService>();
        _handler = new GetChatHistoryHandler(_recommendationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnChatHistoryCacheData_WhenDataIsAvailable()
    {
        //Arrrange
        var chatHistoryData = new ChatHistoryCacheData
        {
            AssistantMessages = new List<string> { "Message 1", "Message 2" }
        };

        _recommendationServiceMock
            .Setup(x => x.GetChatHistoryCacheData())
            .ReturnsAsync(chatHistoryData);

        var query = new GetChatHistoryQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(chatHistoryData, result);
        Assert.Equal(2, result.AssistantMessages.Count);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyChatHistory_WhenServiceReturnsEmptyHistory()
    {
        // Arrange
        var emptyChatHistoryData = new ChatHistoryCacheData();

        _recommendationServiceMock
            .Setup(x => x.GetChatHistoryCacheData())
            .ReturnsAsync(emptyChatHistoryData);

        var query = new GetChatHistoryQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result.AssistantMessages);
    }

    [Fact]
    public async Task Handle_ShouldCallGetChatHistoryCacheDataOnce()
    {
        // Arrange
        var chatHistoryData = new ChatHistoryCacheData
        {
            AssistantMessages = new List<string> { "Message 1" }
        };

        _recommendationServiceMock
            .Setup(x => x.GetChatHistoryCacheData())
            .ReturnsAsync(chatHistoryData);

        var query = new GetChatHistoryQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _recommendationServiceMock.Verify(x => x.GetChatHistoryCacheData(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPropagateException_WhenServiceThrowsException()
    {
        // Arrange
        var exceptionMessage = "Service failed to fetch chat history.";
        _recommendationServiceMock
            .Setup(x => x.GetChatHistoryCacheData())
            .ThrowsAsync(new Exception(exceptionMessage));

        var query = new GetChatHistoryQuery();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        Assert.Equal(exceptionMessage, exception.Message);
    }
}