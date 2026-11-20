namespace CVParser.Application.Pipeline.Interfaces;

/// <summary>Per-run configuration handed to the pipeline.</summary>
public interface IPipelineOptions
{
    /// <summary>The file this run is processing.</summary>
    string FileName { get; }

    bool IsEnabled(string stage);
}
