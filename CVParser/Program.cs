using CVParser;
using CVParser.Pipeline;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

// Transient: the builder is stateful, so each consumer gets a fresh one.
// Open generic: resolves IPipelineBuilder<T> for any context type T.
builder.Services.AddTransient(typeof(IPipelineBuilder<>), typeof(PipelineBuilder<>));
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
