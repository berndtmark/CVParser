var builder = DistributedApplication.CreateBuilder(args);

builder.AddDotnetProject("cvparser", "../CVParser/CVParser.csproj");

builder.Build().Run();
