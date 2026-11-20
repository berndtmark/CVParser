namespace CVParser.Application.Pipeline.Interfaces;

public interface IPipelineBuilder<TContext> where TContext : IPipelineContext, new()
{
    IPipelineBuilder<TContext> Add<TStage>() where TStage : class, IPipelineStage<TContext>;
    IPipelineBuilder<TContext> Add(IPipelineStage<TContext> stage);
    IPipelineBuilder<TContext> Use<TMiddleware>() where TMiddleware : class, IStageMiddleware;
    Pipeline<TContext> Build();
}
