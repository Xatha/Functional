using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Functional.Datastructures;

namespace FunctionalBenchmarks.ViewBenchmarks;

[MemoryDiagnoser]
[SimpleJob(RunStrategy.Throughput, baseline: true)]
public class ArrayViewBenchmarkForEachLoop
{
    private int[] _dataSet = null!;
    private ImmutableArrayView<int> _arrayView;
    private ArraySegment<int> _dataSetSegment;

    [Params(10_000)]
    public int Size { get; set; }
    
    [GlobalSetup]
    public void Setup()
    {
        _dataSet = DataGenerator.GenerateIntDataSet(Size);
        _arrayView = new ImmutableArrayView<int>(_dataSet);
        _dataSetSegment = new ArraySegment<int>(_dataSet);
    }
    
    [Benchmark(Baseline = true)]
    public int Baseline_ForEachLoop()
    {
        int result = 0;

        foreach (var element in _dataSetSegment)
        {
            result = result + element;
        }

        return result;
    }


    [Benchmark]
    public int ArrayView_ForEachLoop()
    { 
        int result = 0;

        foreach (int element in _arrayView)
        {
            result = result + element;
        }

        return result;
    }
}