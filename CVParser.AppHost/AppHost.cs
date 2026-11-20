var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("cvparser-data")
    .WithPgWeb();

var db = postgres.AddDatabase("cvparser-db");

builder.AddDotnetProject("cvparser", "../CVParser/CVParser.csproj")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
