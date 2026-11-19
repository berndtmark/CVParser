using CVParser.Middleware;
using CVParser.Models;
using CVParser.Pipeline;
using CVParser.Stages;

namespace CVParser
{
    public class Worker(ILogger<Worker> logger, IPipelineBuilder<ResumeContext> pipelineBuilder) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Process();

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task Process()
        {
            var pipeline = pipelineBuilder
                .Use<LoggingMiddleware>()
                .Add<TextExtractionStage>()
                .Add<SectionSplitStage>()
                .Add<ContactExtractionStage>()
                .Build();

            var result = await pipeline.RunAsync(new PipelineOptions());
        }
    }
}
