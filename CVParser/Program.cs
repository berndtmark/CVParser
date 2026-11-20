using CVParser;
using CVParser.Application;
using CVParser.Infrastructure.Database;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureDatabaseServices(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

if (builder.Environment.IsDevelopment())
    await host.Services.MigrateDatabaseAsync();

await host.RunAsync();
