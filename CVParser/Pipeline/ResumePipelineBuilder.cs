namespace CVParser.Pipeline;

public sealed class ResumePipelineBuilder(IServiceProvider services) : IResumePipelineBuilder
{
    private readonly List<IPipelineStage> _stages = [];
    private readonly List<IStageMiddleware> _middleware = [];

    public IResumePipelineBuilder Add<TStage>() where TStage : class, IPipelineStage =>
        Add(ActivatorUtilities.GetServiceOrCreateInstance<TStage>(services));

    public IResumePipelineBuilder Add(IPipelineStage stage)
    {
        _stages.Add(stage);
        return this;
    }

    public IResumePipelineBuilder Use<TMiddleware>() where TMiddleware : class, IStageMiddleware
    {
        _middleware.Add(ActivatorUtilities.GetServiceOrCreateInstance<TMiddleware>(services));
        return this;
    }

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

    public static implicit operator ResumePipeline(ResumePipelineBuilder builder) => builder.Build();
}
