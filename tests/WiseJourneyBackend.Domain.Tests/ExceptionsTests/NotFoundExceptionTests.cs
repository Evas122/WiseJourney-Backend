using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Domain.Tests.ExceptionsTests;

public class NotFoundExceptionTests
{
    private readonly string _resourceType;
    private readonly string _resourceId;

    public NotFoundExceptionTests()
    {
        _resourceType = "User";
        _resourceId = "123";
    }

    [Fact]
    public void NotFoundException_ShouldStoreResourceDetails()
    {
        var exception = new NotFoundException(_resourceType, _resourceId);
        Assert.Equal(_resourceType, exception.ResourceType);
        Assert.Equal(_resourceId, exception.ResourceId);
        Assert.Contains(_resourceType, exception.Message);
        Assert.Contains(_resourceId, exception.Message);
    }

    [Fact]
    public void NotFoundException_ShouldInheritFromException()
    {
        var exception = new NotFoundException(_resourceType, _resourceId);
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void NotFoundException_ShouldHaveCorrectMessageFormat()
    {
        var exception = new NotFoundException(_resourceType, _resourceId);
        Assert.Equal("User with id: 123 doesn't exist", exception.Message);
    }
}