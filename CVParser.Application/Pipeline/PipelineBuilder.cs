using CVParser.Application.Pipeline.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CVParser.Application.Pipeline;

public sealed class PipelineBuilder<TContext>(IServiceProvider services) : IPipelineBuilder<TContext>
    where TContext : IPipelineContext, new()
{
    private readonly List<IPipelineStage<TContext>> _stages = [];
    private readonly List<IStageMiddleware> _middleware = [];

    public IPipelineBuilder<TContext> Add<TStage>() where TStage : class, IPipelineStage<TContext> =>
        Add(ActivatorUtilities.GetServiceOrCreateInstance<TStage>(services));

    public IPipelineBuilder<TContext> Add(IPipelineStage<TContext> stage)
    {
        _stages.Add(stage);
        return this;
    }

    public IPipelineBuilder<TContext> Use<TMiddleware>() where TMiddleware : class, IStageMiddleware
    {
        _middleware.Add(ActivatorUtilities.GetServiceOrCreateInstance<TMiddleware>(services));
        return this;
    }

    public Pipeline<TContext> Build()
    {
        if (_stages.Count == 0)
            throw new InvalidOperationException("Pipeline needs at least one stage.");

        var composed = _stages.Select(stage =>
        {
            StageDelegate next = (ctx, ct) => stage.ExecuteAsync((TContext)ctx, ct);
            foreach (var mw in Enumerable.Reverse(_middleware))
            {
                var inner = next;
                var current = mw;
                next = (ctx, ct) => current.InvokeAsync(stage, ctx, inner, ct);
            }
            return ((IPipelineStage)stage, next);
        }).ToArray();

        return new Pipeline<TContext>(composed);
    }
}
