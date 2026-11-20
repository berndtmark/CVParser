using CVParser.Application.Middleware;
using CVParser.Application.Pipeline.Interfaces;
using CVParser.Application.Pipeline.Models;
using CVParser.Application.Stages;

namespace CVParser
{
    public class Worker(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Process(stoppingToken);
        }

        private async Task Process(CancellationToken stoppingToken)
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var pipelineBuilder = scope.ServiceProvider.GetRequiredService<IPipelineBuilder<ResumeContext>>();

            var pipeline = pipelineBuilder
                .Use<ErrorLoggingMiddleware>()
                .Use<LoggingMiddleware>()
                .Use<ProgressMiddleware>()
                .Add<TextExtractionStage>()
                .Add<SectionSplitStage>()
                .Add<ContactExtractionStage>()
                .Build();

            var result = await pipeline.RunAsync(new PipelineOptions { FileName = "sample.txt" }, stoppingToken);
        }
    }
}
