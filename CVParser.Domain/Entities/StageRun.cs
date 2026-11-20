namespace CVParser.Domain.Entities;

/// <summary>Progress of one stage within a <see cref="PipelineRun"/>.</summary>
public class StageRun : EntityBase
{
    private StageRun() { } // EF Core

    public Guid PipelineRunId { get; private set; }
    public string Stage { get; private set; } = string.Empty;
    public RunStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string? Error { get; private set; }

    #region Behaviours
    public static StageRun Start(Guid pipelineRunId, string stage)
    {
        return new StageRun
        {
            Id = Guid.NewGuid(),
            PipelineRunId = pipelineRunId,
            Stage = stage,
            Status = RunStatus.Running,
            StartedAt = DateTime.UtcNow,
        };
    }

    public void Complete()
    {
        Status = RunStatus.Succeeded;
        CompletedAt = DateTime.UtcNow;
    }

    public void Fail(string error)
    {
        Status = RunStatus.Failed;
        CompletedAt = DateTime.UtcNow;
        Error = error;
    }
    #endregion Behaviours
}
