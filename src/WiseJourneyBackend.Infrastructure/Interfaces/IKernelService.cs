using Microsoft.SemanticKernel;

namespace WiseJourneyBackend.Infrastructure.Interfaces;

public interface IKernelService
{
    KernelPlugin ImportAllPlugins();
    Task<string> InvokeAsync(KernelFunction kernelFunction, KernelArguments kernelArguments);
}