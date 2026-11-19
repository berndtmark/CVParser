using CVParser.Enums;

namespace CVParser.Pipeline;

/// <summary>Per-run configuration handed to the pipeline.</summary>
public interface IPipelineOptions
{
    bool IsEnabled(Stage stage);
}
