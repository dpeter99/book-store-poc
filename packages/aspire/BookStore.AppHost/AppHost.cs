var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.BookStore_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");


builder.Build().Run();