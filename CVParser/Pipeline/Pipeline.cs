
namespace CVParser.Pipeline;

public sealed class Pipeline<TContext> where TContext : IPipelineContext, new()
{
    private readonly IReadOnlyList<(IPipelineStage Stage, StageDelegate Invoke)> _stages;

    internal Pipeline(IReadOnlyList<(IPipelineStage, StageDelegate)> stages) => _stages = stages;

    /// <summary>Names of all stages, in order (e.g. to populate a UI picker).</summary>
    public IEnumerable<string> Stages => _stages.Select(s => s.Stage.Name);

    /// <summary>Creates a context from <paramref name="options"/>, runs every enabled stage, and returns it.</summary>
    public async Task<TContext> RunAsync(IPipelineOptions options, CancellationToken ct = default)
    {
        var context = new TContext { RunId = Guid.NewGuid(), Options = options };
        foreach (var (stage, invoke) in _stages)
        {
            ct.ThrowIfCancellationRequested();
            if (!options.IsEnabled(stage.Name))
                continue;
            await invoke(context, ct);
        }
        return context;
    }
}
