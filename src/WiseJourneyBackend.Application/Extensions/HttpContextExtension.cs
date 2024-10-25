using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Application.Extensions;
public static class HttpContextExtension
{
    public static Guid GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? throw new BadRequestException("User not present");

        return Guid.TryParse(userIdString, out var userIdGuid)
               ? userIdGuid
               : throw new InvalidFormatException("Invalid GUID format.");
    }
}