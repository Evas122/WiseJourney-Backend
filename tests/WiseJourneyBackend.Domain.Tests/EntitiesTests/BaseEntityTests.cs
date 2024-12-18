using Moq;
using WiseJourneyBackend.Domain.Common;

namespace WiseJourneyBackend.Domain.Tests.EntitiesTests;

public class BaseEntityTests
{
    private readonly BaseEntity _baseEntity;

    public BaseEntityTests()
    {
        _baseEntity = new Mock<BaseEntity>().Object;
    }

    [Fact]
    public void BaseEntity_ShouldHaveDefaultTimestamps()
    {
        // Act
        var createdAt = _baseEntity.CreatedAtUtc;
        var updatedAt = _baseEntity.UpdatedAtUtc;

        // Assert
        Assert.Equal(default(DateTime), createdAt);
        Assert.Equal(default(DateTime), updatedAt);
    }

    [Fact]
    public void BaseEntity_ShouldAllowSettingTimestamps()
    {
        // Arrange
        var now = DateTime.UtcNow;

        // Act
        _baseEntity.CreatedAtUtc = now;
        _baseEntity.UpdatedAtUtc = now;

        // Assert
        Assert.Equal(now, _baseEntity.CreatedAtUtc);
        Assert.Equal(now, _baseEntity.UpdatedAtUtc);
    }

    [Fact]
    public void BaseEntity_IdShouldBeUnique()
    {
        // Arrange
        var anotherBaseEntity = new Mock<BaseEntity>().Object;

        // Act
        _baseEntity.Id = Guid.NewGuid();
        anotherBaseEntity.Id = Guid.NewGuid();

        // Assert
        Assert.NotEqual(_baseEntity.Id, anotherBaseEntity.Id);
    }
}