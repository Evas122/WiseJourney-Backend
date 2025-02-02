﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Infrastructure.ExceptionsHandlers;

internal sealed class ArgumentIsNullExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ArgumentIsNullExceptionHandler> _logger;

    public ArgumentIsNullExceptionHandler(ILogger<ArgumentIsNullExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ArgumentIsNullException argumentIsNullException)
        {
            return false;
        }

        _logger.LogError(
            argumentIsNullException,
            "ArgumentNullException occurred: {Message}, Parameter: {ParamName}",
            argumentIsNullException.Message,
            argumentIsNullException.ParamName);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Argument Null Error",
            Detail = $"Parameter '{argumentIsNullException.ParamName}' cannot be null or whitespace."
        };

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}