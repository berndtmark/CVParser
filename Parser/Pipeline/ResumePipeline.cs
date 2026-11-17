using Parser.Models;

namespace Parser.Pipeline;

public sealed class ResumePipeline
{
    private readonly IReadOnlyList<(IPipelineStage Stage, StageDelegate Invoke)> _stages;

    internal ResumePipeline(IReadOnlyList<(IPipelineStage, StageDelegate)> stages) => _stages = stages;

    public async Task<ResumeContext> RunAsync(string sourcePath, CancellationToken ct = default)
    {
        var context = new ResumeContext { SourcePath = sourcePath };
        foreach (var (_, invoke) in _stages)
        {
            ct.ThrowIfCancellationRequested();
            await invoke(context, ct);
        }
        return context;
    }
}
