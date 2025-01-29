using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Domain.Tests.ExceptionsTests;

public class InvalidFormatExceptionTests
{
    private readonly string _message;
    private readonly string _invalidValue;

    public InvalidFormatExceptionTests()
    {
        _message = "Invalid format.";
        _invalidValue = "123abc";
    }

    [Fact]
    public void InvalidFormatException_ShouldStoreMessage()
    {
        var exception = new InvalidFormatException(_message);
        Assert.Equal(_message, exception.Message);
    }

    [Fact]
    public void InvalidFormatException_ShouldStoreInvalidValue()
    {
        var exception = new InvalidFormatException(_message, _invalidValue);
        Assert.Equal(_invalidValue, exception.InvalidValue);
    }

    [Fact]
    public void InvalidFormatException_ShouldStoreInnerException()
    {
        var innerException = new Exception("Inner exception.");
        var exception = new InvalidFormatException(_message, innerException);

        Assert.Equal(innerException, exception.InnerException);
    }
}