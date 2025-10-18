var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgresql")
    .WithDataVolume(isReadOnly: false);
var bookdb = postgres.AddDatabase("bookdb");



var apiService = builder.AddProject<Projects.BookStore_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(bookdb);


builder.Build().Run();