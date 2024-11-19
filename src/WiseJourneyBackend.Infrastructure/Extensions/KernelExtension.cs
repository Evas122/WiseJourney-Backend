using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using WiseJourneyBackend.Domain.Exceptions;
using WiseJourneyBackend.Infrastructure.Delegating;
using WiseJourneyBackend.Infrastructure.Interfaces;
using WiseJourneyBackend.Infrastructure.Services;

namespace WiseJourneyBackend.Infrastructure.Extensions;

public static class KernelExtension
{
    public static void AddGroqAiKernelExtension(this IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration["AiSettings:ApiKey"] ?? throw new ConfigurationException("api Key cannot be null.");
        var model = configuration["AiSettings:Model"] ?? throw new ConfigurationException("model cannot be null.");

        services.AddScoped<Kernel>(sp =>
        {
            HttpClient httpClient = new(new GroqDelegatingHandler(configuration));
            var kernelBuilder = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(model, apiKey, httpClient: httpClient);
            return kernelBuilder.Build();
        });

        services.AddScoped<IKernelService, KernelService>();
    }
}