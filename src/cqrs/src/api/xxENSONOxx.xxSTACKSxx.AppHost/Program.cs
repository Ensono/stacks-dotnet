using System.Diagnostics.CodeAnalysis;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<xxENSONOxx_xxSTACKSxx_API>("cqrs-api");

await builder.Build().RunAsync();

[ExcludeFromCodeCoverage]
public partial class Program { }
