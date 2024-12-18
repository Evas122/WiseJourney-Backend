using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Domain.Tests.ExceptionsTests;

public class ConfigurationExceptionTests
{
    private readonly string _message;

    public ConfigurationExceptionTests()
    {
        _message = "Configuration error occurred.";
    }

    [Fact]
    public void ConfigurationException_ShouldStoreMessage()
    {
        var exception = new ConfigurationException(_message);
        Assert.Equal(_message, exception.Message);
    }

    [Fact]
    public void ConfigurationException_ShouldStoreInnerException()
    {
        var innerException = new Exception("Inner exception.");
        var exception = new ConfigurationException(_message, innerException);

        Assert.Equal(innerException, exception.InnerException);
    }

    [Fact]
    public void ConfigurationException_ShouldInheritFromException()
    {
        var exception = new ConfigurationException(_message);
        Assert.IsAssignableFrom<Exception>(exception);
    }
}