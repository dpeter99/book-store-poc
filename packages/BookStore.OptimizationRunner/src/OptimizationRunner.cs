using BookStore.JobContracts.Contracts;

namespace BookStore.OptimizationRunner;

public class OptimizationRunner : IOptimizationRunner
{
    public void Run(OptimizationRunParams options)
    {
        Console.WriteLine($"Running {nameof(OptimizationRunner)}...");
        Console.WriteLine($"{options.id}");
    }
}