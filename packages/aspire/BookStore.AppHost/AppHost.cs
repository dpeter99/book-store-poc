var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgresql")
    .WithDataVolume(isReadOnly: false);
var bookdb = postgres.AddDatabase("bookdb");



var apiService = builder.AddProject<Projects.BookStore_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(bookdb);

var worker = builder.AddProject<Projects.BookStore_OptimizationRunner>("optimizer")
    .WithReplicas(2)
    .WithReference(bookdb);

builder.Build().Run();