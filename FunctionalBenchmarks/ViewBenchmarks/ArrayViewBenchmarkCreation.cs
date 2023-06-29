using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Functional.View;

namespace FunctionalBenchmarks.ViewBenchmarks;

[MemoryDiagnoser]
[SimpleJob(RunStrategy.Throughput, baseline: true)]
public class ArrayViewBenchmarkCreation
{
    private int[] _dataSet = null!;
    private ArrayView<int> _arrayView;
    private Range _range;
    private int _offset;
    private int _length;

    [Params(10_000)]
    public int Size { get; set; }
    
    [GlobalSetup]
    public void Setup()
    {
        _dataSet = DataGenerator.GenerateIntDataSet(Size);
        _arrayView = new ArrayView<int>(_dataSet);
        _range = (Size / 5)..(Size / 2);
        (_offset, _length) = _range.GetOffsetAndLength(Size);
    }
    
    [Benchmark]
    public ArraySegment<int> Baseline_ArraySegment_FullSegment()
    {
        return new ArraySegment<int>(_dataSet);
    }
    
    [Benchmark]
    public ArraySegment<int> Baseline_ArraySegment_PartialSegment()
    {
        return new ArraySegment<int>(_dataSet, _offset, _length);
    }
    
    [Benchmark]
    public ArrayView<int> ArrayView_FullSegment()
    {
        return new ArrayView<int>(_dataSet);
    }
    
    [Benchmark]
    public ArrayView<int> ArrayView_PartialSegment()
    {
        return new ArrayView<int>(_dataSet, _range);
    }
    
    [Benchmark]
    public ArrayView<int> ArrayView_Slice()
    {
        return _arrayView.Slice(_range);
    }
}