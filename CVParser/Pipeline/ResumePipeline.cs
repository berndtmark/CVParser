using CVParser.Enums;
using CVParser.Models;

namespace CVParser.Pipeline;

public sealed class ResumePipeline
{
    private readonly IReadOnlyList<(IPipelineStage Stage, StageDelegate Invoke)> _stages;

    internal ResumePipeline(IReadOnlyList<(IPipelineStage, StageDelegate)> stages) => _stages = stages;

    /// <summary>Names of all stages, in order (e.g. to populate a UI picker).</summary>
    public IEnumerable<Stage> Stages => _stages.Select(s => s.Stage.Name);

    public async Task<ResumeContext> RunAsync(PipelineOptions? options = null, CancellationToken ct = default)
    {
        var context = new ResumeContext { Options = options ?? new PipelineOptions() };
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
