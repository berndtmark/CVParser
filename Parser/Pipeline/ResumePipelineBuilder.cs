using Microsoft.Extensions.DependencyInjection;

namespace Parser.Pipeline;

public sealed class ResumePipelineBuilder(IServiceProvider services)
{
    private readonly List<IPipelineStage> _stages = [];
    private readonly List<IStageMiddleware> _middleware = [];

    /// <summary>Add a stage resolved from DI (falls back to constructing it with injected deps).</summary>
    public ResumePipelineBuilder Add<TStage>() where TStage : class, IPipelineStage =>
        Add(ActivatorUtilities.GetServiceOrCreateInstance<TStage>(services));

    public ResumePipelineBuilder Add(IPipelineStage stage)
    {
        _stages.Add(stage);
        return this;
    }

    /// <summary>Add middleware. First registered = outermost.</summary>
    public ResumePipelineBuilder Use<TMiddleware>() where TMiddleware : class, IStageMiddleware
    {
        _middleware.Add(ActivatorUtilities.GetServiceOrCreateInstance<TMiddleware>(services));
        return this;
    }

    /// <summary>Lets a builder be assigned to a ResumePipeline without calling Build().</summary>
    public static implicit operator ResumePipeline(ResumePipelineBuilder builder) => builder.Build();

    public ResumePipeline Build()
    {
        if (_stages.Count == 0)
            throw new InvalidOperationException("Pipeline needs at least one stage.");

        var composed = _stages.Select(stage =>
        {
            StageDelegate next = (ctx, ct) => stage.ExecuteAsync(ctx, ct);
            foreach (var mw in Enumerable.Reverse(_middleware))
            {
                var inner = next;
                var current = mw;
                next = (ctx, ct) => current.InvokeAsync(stage, ctx, inner, ct);
            }
            return (stage, next);
        }).ToArray();

        return new ResumePipeline(composed);
    }
}
