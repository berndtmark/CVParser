using CVParser;
using CVParser.Pipeline;

var builder = Host.CreateApplicationBuilder(args);

// Transient: the builder is stateful, so each consumer gets a fresh one.
builder.Services.AddTransient<IResumePipelineBuilder, ResumePipelineBuilder>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
