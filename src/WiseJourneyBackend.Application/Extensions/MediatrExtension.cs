using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace WiseJourneyBackend.Application.Extensions;

public static class MediatrExtension
{
    public static void AddMediatrExtension(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}