using Parser.Models;
using Parser.Stages;

namespace Parser.Pipeline;

public sealed class ResumePipeline
{
    private readonly IReadOnlyList<(IPipelineStage Stage, StageDelegate Invoke)> _stages;

    internal ResumePipeline(IReadOnlyList<(IPipelineStage, StageDelegate)> stages) => _stages = stages;

    /// <summary>Names of all stages, in order (e.g. to populate a UI picker).</summary>
    public IEnumerable<Stage> Stages => _stages.Select(s => s.Stage.Name);

    /// <param name="options">Run configuration. Null = defaults (all stages).</param>
    public async Task<ResumeContext> RunAsync(string sourcePath, PipelineOptions? options = null, CancellationToken ct = default)
    {
        var context = new ResumeContext { SourcePath = sourcePath, Options = options ?? new PipelineOptions() };
        foreach (var (stage, invoke) in _stages)
        {
            ct.ThrowIfCancellationRequested();
            if (!context.Options.IsEnabled(stage.Name))
                continue;
            await invoke(context, ct);
        }
        return context;
    }
}
