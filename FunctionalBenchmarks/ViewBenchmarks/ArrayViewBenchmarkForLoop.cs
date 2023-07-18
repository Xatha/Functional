using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Functional.Datastructures;

namespace FunctionalBenchmarks.ViewBenchmarks;

[MemoryDiagnoser]
[SimpleJob(RunStrategy.Throughput, baseline: true)]
public class ArrayViewBenchmarkForLoop
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
    public int Baseline_ForLoop()
    {
        int result = 0;
        
        for (int i = 0; i < _dataSetSegment.Count; i++)
        {
            int value = _dataSetSegment[i];
            result = result + value;
        }

        return result;
    }
    
    [Benchmark]
    public int ArrayView_ForLoop()
    {
        int result = 0;

        for (int i = 0; i < _arrayView.Count; i++)
        {
            int value = _arrayView[i];
            result = result + value;
        }

        return result;
    }
}