
namespace CVParser.Pipeline;

/// <summary>Per-run configuration handed to the pipeline.</summary>
public interface IPipelineOptions
{
    bool IsEnabled(string stage);
}
