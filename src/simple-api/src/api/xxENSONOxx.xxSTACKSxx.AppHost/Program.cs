var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.xxENSONOxx_xxSTACKSxx_API>("api");

builder.Build().Run();
