using Microsoft.Extensions.Configuration;
using WiseJourneyBackend.Domain.Exceptions;

namespace WiseJourneyBackend.Infrastructure.Delegating;

public class GroqDelegatingHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;
    public GroqDelegatingHandler(IConfiguration configuration) : base (new HttpClientHandler())
    {
        _configuration = configuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri == null)
        {
            throw new ArgumentIsNullException("RequestUri cannot be null.");
        }

        var urlToReplace = _configuration["Delegating:UrlToReplace"] ?? throw new ConfigurationException("Url to Replace cannot be empty.");
        var endpoint = _configuration["AiSettings:Endpoint"];

        request.RequestUri = new Uri(request.RequestUri.ToString().Replace(urlToReplace, endpoint));

        return await base.SendAsync(request, cancellationToken);
    }
}