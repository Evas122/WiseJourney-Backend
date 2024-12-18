using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Domain.Tests.ExceptionsTests;

public class BadRequestExceptionTests
{
    private readonly string _message;
    private readonly IDictionary<string, string[]> _errors;

    public BadRequestExceptionTests()
    {
        _message = "Invalid request.";
        _errors = new Dictionary<string, string[]>
        {
            { "field", new[] { "Error1", "Error2" } }
        };
    }

    [Fact]
    public void BadRequestException_ShouldStoreMessage()
    {
        var exception = new BadRequestException(_message);
        Assert.Equal(_message, exception.Message);
    }

    [Fact]
    public void BadRequestException_ShouldStoreErrors()
    {
        var exception = new BadRequestException(_message, _errors);
        Assert.Equal(_errors, exception.Errors);
    }

    [Fact]
    public void BadRequestException_ShouldHandleEmptyErrors()
    {
        var emptyErrors = new Dictionary<string, string[]>();
        var exception = new BadRequestException(_message, emptyErrors);

        Assert.NotNull(exception.Errors);
        Assert.Empty(exception.Errors);
    }
}