using CVParser.Application.Pipeline;
using CVParser.Application.Pipeline.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CVParser.Application;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        // Transient: the builder is stateful, so each consumer gets a fresh one.
        // Open generic: resolves IPipelineBuilder<T> for any context type T.
        services.AddTransient(typeof(IPipelineBuilder<>), typeof(PipelineBuilder<>));

        return services;
    }
}
