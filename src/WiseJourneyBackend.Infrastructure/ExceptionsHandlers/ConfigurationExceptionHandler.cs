using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Infrastructure.ExceptionsHandlers;
internal sealed class ConfigurationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ConfigurationExceptionHandler> _logger;

    public ConfigurationExceptionHandler(ILogger<ConfigurationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ConfigurationException configurationException)
        {
            return false;
        }

        _logger.LogError(
            configurationException,
            "ConfigurationException occurred: {Message}",
            configurationException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Configuration Error",
            Detail = configurationException.Message
        };

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}