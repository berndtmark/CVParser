namespace CVParser.Domain.Entities;

/// <summary>One execution of the pipeline for a single file.</summary>
public class PipelineRun : EntityBase
{
    private PipelineRun() { } // EF Core

    public string FileName { get; private set; } = string.Empty;
    public DateTime StartedAt { get; private set; }

    #region Behaviours
    public static PipelineRun Start(string fileName)
    {
        return new PipelineRun
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            StartedAt = DateTime.UtcNow,
        };
    }
    #endregion Behaviours
}
