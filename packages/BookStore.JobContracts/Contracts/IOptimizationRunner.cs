using Hangfire;

namespace BookStore.JobContracts.Contracts;

public record OptimizationRunParams(string id);

public static class OptimizationRunnerQueue
{
    public const string QueueName = "optimization_queue";
}

public interface IOptimizationRunner
{
    [Queue(OptimizationRunnerQueue.QueueName)]
    void Run(OptimizationRunParams options);
}