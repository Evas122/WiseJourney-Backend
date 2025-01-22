using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Security.Claims;
using WiseJourneyBackend.Application.Cache;
using WiseJourneyBackend.Application.Interfaces;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Infrastructure.Tests.ServicesTests;

public class CacheServiceTests
{
    private readonly Mock<IMemoryCache> _memoryCacheMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly ICacheService _cacheService;

    public CacheServiceTests()
    {
        _memoryCacheMock = new Mock<IMemoryCache>();
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _cacheService = new CacheService(_memoryCacheMock.Object, _contextAccessorMock.Object);
    }

    private void MockUserContext(Guid userId)
    {
        var httpContextMock = new Mock<HttpContext>();
        var claimsPrincipalMock = new Mock<ClaimsPrincipal>();

        claimsPrincipalMock.Setup(x => x.FindFirst(ClaimTypes.NameIdentifier))
                           .Returns(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

        httpContextMock.Setup(x => x.User).Returns(claimsPrincipalMock.Object);
        _contextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
    }

    [Fact]
    public void GetCache_ShouldReturnCachedItem_WhenCacheExists()
    {
        // Arrange
        var cacheKey = "someKey";
        var expectedValue = "cachedValue";

        var userId = Guid.NewGuid();
        MockUserContext(userId);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var cacheService = new CacheService(cache, _contextAccessorMock.Object);

        var fullCacheKey = cacheService.GetCacheKey(cacheKey);
        cache.Set(fullCacheKey, expectedValue);

        // Act
        var result = cacheService.GetCache<string>(cacheKey);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void SetCache_ShouldSetCacheItem()
    {
        // Arrange
        var cacheKey = "someKey";
        var value = "valueToCache";
        var userId = Guid.NewGuid();

        MockUserContext(userId);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var cacheService = new CacheService(cache, _contextAccessorMock.Object);

        // Act
        cacheService.SetCache(cacheKey, value);

        // Assert
        var fullCacheKey = cacheService.GetCacheKey(cacheKey);
        var cachedValue = cache.Get<string>(fullCacheKey);

        Assert.Equal(value, cachedValue);
    }


    [Fact]
    public void RemoveCache_ShouldRemoveCacheItem()
    {
        // Arrange
        var cacheKey = "someKey";

        var userId = Guid.NewGuid();
        MockUserContext(userId);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var cacheService = new CacheService(cache, _contextAccessorMock.Object);

        // Act
        cacheService.RemoveCache(cacheKey);

        // Assert
        var cachedValue = cache.Get<string>(cacheKey);
        Assert.Null(cachedValue);
    }

    [Fact]
    public void AddItemToLimitedList_ShouldAddItemAndLimitListSize()
    {
        // Arrange
        var list = new List<string> { "item1", "item2" };
        var newItem = "item3";
        int maxItems = 3;

        var userId = Guid.NewGuid();
        MockUserContext(userId);

        // Act
        _cacheService.AddItemToLimitedList(list, newItem, maxItems);

        // Assert
        Assert.Contains(newItem, list);
        Assert.Equal(maxItems, list.Count);
        Assert.Equal("item1", list[0]);
    }

    [Fact]
    public void InterleaveChatHistoryMessages_ShouldReturnInterleavedMessages()
    {
        // Arrange
        var chatHistory = new ChatHistoryCacheData
        {
            UserMessages = new List<string> { "User message 1", "User message 2" },
            AssistantMessages = new List<string> { "Assistant message 1", "Assistant message 2" }
        };

        var userId = Guid.NewGuid();
        MockUserContext(userId);

        // Act
        var result = _cacheService.InterleaveChatHistoryMessages(chatHistory);

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Equal("User: User message 1", result[0]);
        Assert.Equal("Assistant: Assistant message 1", result[1]);
        Assert.Equal("User: User message 2", result[2]);
        Assert.Equal("Assistant: Assistant message 2", result[3]);
    }

    [Fact]
    public void GetCache_ShouldReturnNull_WhenCacheDoesNotExist()
    {
        // Arrange
        var cacheKey = "nonExistingKey";
        var userId = Guid.NewGuid();
        MockUserContext(userId);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var cacheService = new CacheService(cache, _contextAccessorMock.Object);

        // Act
        var result = cacheService.GetCache<string>(cacheKey);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void SetCache_ShouldOverrideExistingCacheItem()
    {
        // Arrange
        var cacheKey = "someKey";
        var initialValue = "initialValue";
        var newValue = "newValue";

        var userId = Guid.NewGuid();
        MockUserContext(userId);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var cacheService = new CacheService(cache, _contextAccessorMock.Object);

        var fullCacheKey = cacheService.GetCacheKey(cacheKey);
        cache.Set(fullCacheKey, initialValue);

        // Act
        cacheService.SetCache(cacheKey, newValue);

        // Assert
        var cachedValue = cache.Get<string>(fullCacheKey);
        Assert.Equal(newValue, cachedValue);
    }

    [Fact]
    public void RemoveCache_ShouldNotThrowException_WhenKeyDoesNotExist()
    {
        // Arrange
        var cacheKey = "nonExistingKey";
        var userId = Guid.NewGuid();
        MockUserContext(userId);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var cacheService = new CacheService(cache, _contextAccessorMock.Object);

        // Act & Assert
        var exception = Record.Exception(() => cacheService.RemoveCache(cacheKey));
        Assert.Null(exception);
    }

    [Fact]
    public void AddItemToLimitedList_ShouldNotExceedMaxItems()
    {
        // Arrange
        var list = new List<string> { "item1", "item2", "item3" };
        var newItem = "item4";
        int maxItems = 3;

        var userId = Guid.NewGuid();
        MockUserContext(userId);

        // Act
        _cacheService.AddItemToLimitedList(list, newItem, maxItems);

        // Assert
        Assert.Equal(maxItems, list.Count);
        Assert.Contains(newItem, list);
        Assert.DoesNotContain("item1", list);
    }

    [Fact]
    public void InterleaveChatHistoryMessages_ShouldHandleEmptyMessagesLists()
    {
        // Arrange
        var chatHistory = new ChatHistoryCacheData
        {
            UserMessages = new List<string> { "User message 1" },
            AssistantMessages = new List<string>()
        };

        var userId = Guid.NewGuid();
        MockUserContext(userId);

        // Act
        var result = _cacheService.InterleaveChatHistoryMessages(chatHistory);

        // Assert
        Assert.Single(result);
        Assert.Equal("User: User message 1", result[0]);
    }
}