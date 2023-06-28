using BenchmarkDotNet.Running;

namespace FunctionalBenchmarks;

public class Program
{
    static void Main()
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();
    }
}