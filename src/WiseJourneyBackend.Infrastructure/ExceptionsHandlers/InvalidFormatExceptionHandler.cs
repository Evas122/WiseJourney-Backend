using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Infrastructure.ExceptionsHandlers;
internal sealed class InvalidFormatExceptionHandler : IExceptionHandler
{
    private readonly ILogger<InvalidFormatExceptionHandler> _logger;

    public InvalidFormatExceptionHandler(ILogger<InvalidFormatExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not InvalidFormatException formatException)
        {
            return false;
        }

        _logger.LogError(formatException, "FormatException occurred: {Message}", formatException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid format",
            Detail = formatException.Message
        };

        if (!string.IsNullOrEmpty(formatException.InvalidValue))
        {
            problemDetails.Extensions["invalidValue"] = formatException.InvalidValue;
        }

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}