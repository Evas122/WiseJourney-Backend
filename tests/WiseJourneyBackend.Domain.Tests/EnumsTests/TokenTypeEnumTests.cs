using WiseJourneyBackend.Domain.Enums;

namespace WiseJourneyBackend.Domain.Tests.EnumsTests;

public class TokenTypeEnumTests
{
    private readonly TokenType _emailConfirmation;
    private readonly TokenType _passwordReset;

    public TokenTypeEnumTests()
    {
        _emailConfirmation = TokenType.EmailConfirmation;
        _passwordReset = TokenType.PasswordReset;
    }

    [Fact]
    public void TokenType_ShouldHaveCorrectValues()
    {
        Assert.Equal(0, (int)_emailConfirmation);
        Assert.Equal(1, (int)_passwordReset);
    }

    [Fact]
    public void TokenType_ShouldBeEnumType()
    {
        Assert.IsType<TokenType>(_emailConfirmation);
        Assert.IsType<TokenType>(_passwordReset);
    }

    [Fact]
    public void TokenType_ShouldContainAllTokenTypes()
    {
        var allTokenTypes = Enum.GetValues(typeof(TokenType)).Cast<TokenType>().ToList();
        Assert.Equal(2, allTokenTypes.Count);
        Assert.Contains(TokenType.EmailConfirmation, allTokenTypes);
        Assert.Contains(TokenType.PasswordReset, allTokenTypes);
    }
}