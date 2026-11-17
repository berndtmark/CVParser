using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Parser.Middleware;
using Parser.Stages;

namespace Parser;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddResumeParser(this IServiceCollection services)
    {
        services.AddLogging(b => b.AddSimpleConsole(o => o.SingleLine = true));

        services.AddTransient<LoggingMiddleware>();

        services.AddTransient<TextExtractionStage>();
        services.AddTransient<SectionSplitStage>();
        services.AddTransient<ContactExtractionStage>();

        return services;
    }
}
