using BenchmarkDotNet.Running;
using FunctionalBenchmarks.OptionBenchmarks;

namespace FunctionalBenchmarks;

public class Program
{
    static void Main()
    {
        BenchmarkRunner.Run<OptionMapBenchmark>();
    }
}