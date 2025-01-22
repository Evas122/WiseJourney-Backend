using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Domain.Tests.ExceptionsTests;

public class ArgumentIsNullExceptionTests
{
    private readonly string _paramName;
    private readonly string _message;

    public ArgumentIsNullExceptionTests()
    {
        _paramName = "testParam";
        _message = "Parameter 'testParam' cannot be null or whitespace.";
    }

    [Fact]
    public void ArgumentIsNullException_ShouldStoreParameterName()
    {
        var exception = new ArgumentIsNullException(_paramName);
        Assert.Equal(_paramName, exception.ParamName);
        Assert.Contains(_paramName, exception.Message);
    }

    [Fact]
    public void ArgumentIsNullException_ShouldStoreCustomMessage()
    {
        var customMessage = "Custom error message.";
        var exception = new ArgumentIsNullException(_paramName, customMessage);

        Assert.Equal(customMessage, exception.Message);
        Assert.Equal(_paramName, exception.ParamName);
    }

    [Fact]
    public void ArgumentIsNullException_ShouldValidateParameterName()
    {
        Assert.Throws<ArgumentException>(() => new ArgumentIsNullException(""));
    }
}