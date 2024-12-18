using Moq;
using WiseJourneyBackend.Application.Commands.SendPreferenceMessage;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Application.Tests.CommandsTests;

public class SendPreferenceMessageHandlerTests
{
    private readonly SendPreferenceMessageHandler _handler;
    private readonly Mock<IRecommendationService> _recommendationServiceMock;

    public SendPreferenceMessageHandlerTests()
    {
        _recommendationServiceMock = new Mock<IRecommendationService>();
        _handler = new SendPreferenceMessageHandler(_recommendationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallSendUserPreferencesMessageAsync_WhenMessageIsValid()
    {
        // Arrange
        var command = new SendPreferenceMessageCommand("Valid message");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _recommendationServiceMock
            .Verify(x => x.SendUserPreferencesMessageAsync(It.IsAny<SendPreferenceMessageCommand>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenMessageIsNullOrEmpty()
    {
        // Arrange
        var command = new SendPreferenceMessageCommand("");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentIsNullException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Parameter 'Message' cannot be null or whitespace.", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenSendUserPreferencesMessageAsyncFails()
    {
        // Arrange
        var command = new SendPreferenceMessageCommand("Valid message");

        _recommendationServiceMock
            .Setup(x => x.SendUserPreferencesMessageAsync(It.IsAny<SendPreferenceMessageCommand>()))
            .ThrowsAsync(new Exception("Service failure"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Service failure", exception.Message);
    }
}