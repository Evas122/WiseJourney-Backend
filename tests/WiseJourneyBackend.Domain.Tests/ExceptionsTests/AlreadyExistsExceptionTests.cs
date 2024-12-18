using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Domain.Tests.ExceptionsTests;

public class AlreadyExistsExceptionTests
{
    private readonly string _message;
    private readonly string _existsValue;

    public AlreadyExistsExceptionTests()
    {
        _message = "Entity already exists.";
        _existsValue = "User123";
    }

    [Fact]
    public void AlreadyExistsException_ShouldStoreMessageAndValue()
    {
        var exception = new AlreadyExistsException(_message, _existsValue);
        Assert.Equal(_message, exception.Message);
        Assert.Equal(_existsValue, exception.ExistsValue);
    }

    [Fact]
    public void AlreadyExistsException_ShouldStoreInnerException()
    {
        var innerException = new Exception("Inner exception message.");
        var exception = new AlreadyExistsException(_existsValue, _message, innerException);

        Assert.Equal(innerException, exception.InnerException);
        Assert.Equal(_existsValue, exception.ExistsValue);
    }

    [Fact]
    public void AlreadyExistsException_ShouldInheritFromException()
    {
        var exception = new AlreadyExistsException(_message, _existsValue);
        Assert.IsAssignableFrom<Exception>(exception);
    }
}