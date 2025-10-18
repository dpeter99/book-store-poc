using BookStore.JobContracts.Contracts;
using BookStore.OptimizationRunner;
using Hangfire;
using Hangfire.PostgreSql;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(c =>
    {
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("bookdb"));
    });
});
builder.Services.AddHangfireServer(c =>
{
    c.Queues = [OptimizationRunnerQueue.QueueName];
});

builder.Services.AddTransient<IOptimizationRunner, OptimizationRunner>();

var host = builder.Build();
host.Run();