namespace CVParser.Pipeline;

public interface IResumePipelineBuilder
{
    IResumePipelineBuilder Add<TStage>() where TStage : class, IPipelineStage;
    IResumePipelineBuilder Add(IPipelineStage stage);
    IResumePipelineBuilder Use<TMiddleware>() where TMiddleware : class, IStageMiddleware;
    ResumePipeline Build();
}
