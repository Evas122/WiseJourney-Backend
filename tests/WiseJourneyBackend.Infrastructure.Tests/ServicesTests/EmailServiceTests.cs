using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using WiseJourneyBackend.Domain.Entities.Auth;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Infrastructure.Tests.ServicesTests;
public class EmailServiceTests
{
    private readonly Mock<IFluentEmail> _fluentEmailMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        _fluentEmailMock = new Mock<IFluentEmail>();
        _configurationMock = new Mock<IConfiguration>();
        _emailService = new EmailService(_fluentEmailMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task SendVerificationEmail_ShouldSendEmailWithCorrectLink()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var token = "verificationToken";
        var verificationLink = "http://example.com/confirm-email?token=verificationToken";
        bool sendAsyncCalled = true;

        _configurationMock
            .Setup(c => c["App:ConfirmEmail"])
            .Returns("http://example.com/confirm-email?token=");

        _fluentEmailMock
            .Setup(fe => fe.To(user.Email))
            .Returns(_fluentEmailMock.Object);
        _fluentEmailMock
            .Setup(fe => fe.Subject(It.IsAny<string>()))
            .Returns(_fluentEmailMock.Object);
        _fluentEmailMock
            .Setup(fe => fe.Body(It.IsAny<string>(), true))
            .Returns(_fluentEmailMock.Object);
        _fluentEmailMock
            .Setup(fe => fe.SendAsync(It.IsAny<CancellationToken>()))
            .Callback(() => sendAsyncCalled = true)
            .ReturnsAsync(new SendResponse());

        // Act
        await _emailService.SendVerificationEmail(user, token);

        // Assert
        Assert.True(sendAsyncCalled, "SendAsync was not called.");
        _fluentEmailMock.Verify(fe => fe.To(user.Email), Times.Once);
        _fluentEmailMock.Verify(fe => fe.Subject("Email verification for WiseJourney"), Times.Once);
        _fluentEmailMock.Verify(fe => fe.Body($"To verify your email address <a href='{verificationLink}'>click here</a>", true), Times.Once);
    }

    [Fact]
    public async Task SendResetPasswordEmail_ShouldSendEmailWithCorrectLink()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var token = "resetToken";
        var resetPasswordLink = "http://example.com/reset-password?token=resetToken";
        bool sendAsyncCalled = true;

        _configurationMock
            .Setup(c => c["App:ResetPassword"])
            .Returns("http://example.com/reset-password?token=");

        _fluentEmailMock
            .Setup(fe => fe.To(user.Email))
            .Returns(_fluentEmailMock.Object);
        _fluentEmailMock
            .Setup(fe => fe.Subject(It.IsAny<string>()))
            .Returns(_fluentEmailMock.Object);
        _fluentEmailMock
            .Setup(fe => fe.Body(It.IsAny<string>(), true))
            .Returns(_fluentEmailMock.Object);
        _fluentEmailMock
            .Setup(fe => fe.SendAsync(It.IsAny<CancellationToken>()))
            .Callback(() => sendAsyncCalled = true)
            .ReturnsAsync(new SendResponse());

        // Act
        await _emailService.SendResetPasswordEmail(user, token);

        // Assert
        Assert.True(sendAsyncCalled, "SendAsync was not called.");
        _fluentEmailMock.Verify(fe => fe.To(user.Email), Times.Once);
        _fluentEmailMock.Verify(fe => fe.Subject("Reset password for WiseJourney"), Times.Once);
        _fluentEmailMock.Verify(fe => fe.Body($"To reset your password <a href='{resetPasswordLink}'>click here</a>", true), Times.Once);
    }

    [Fact]
    public async Task SendVerificationEmail_ShouldThrowException_WhenEmailIsNull()
    {
        // Arrange
        #nullable disable
        var user = new User { Email = null };
        #nullable restore
        var token = "verificationToken";

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _emailService.SendVerificationEmail(user, token));
    }

    [Fact]
    public async Task SendResetPasswordEmail_ShouldThrowException_WhenEmailIsNull()
    {
        // Arrange
        #nullable disable
        var user = new User { Email = null };
        #nullable restore
        var token = "resetToken";

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _emailService.SendResetPasswordEmail(user, token));
    }

    [Fact]
    public async Task SendVerificationEmail_ShouldThrowException_WhenConfigurationKeyIsMissing()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var token = "verificationToken";

        _configurationMock
            .Setup(c => c["App:ConfirmEmail"])
        #nullable disable
            .Returns<string>(null);
        #nullable restore

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _emailService.SendVerificationEmail(user, token));
    }
}
