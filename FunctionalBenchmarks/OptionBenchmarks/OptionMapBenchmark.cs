using BenchmarkDotNet.Attributes;
using Functional.OptionFeature;

// Setup is always called once before running any benchmarks, so our private field _dataSet is initialized here.
#pragma warning disable CS8618 

namespace FunctionalBenchmarks.OptionBenchmarks;

[MemoryDiagnoser]
public class OptionMapSomeBenchmark
{
    private int[] _dataSet;

    [Params(1000, 10000)]
    public int Size { get; set; }
    
    [GlobalSetup]
    public void Setup()
    {
        _dataSet = DataGenerator.GenerateIntDataSet(Size);
    }

    #region Some

    [Benchmark]
    public int? Baseline_Some()
    {
        int? result = 1;
        
        for (int i = 0; i < _dataSet.Length; i++)
        {
            int value = _dataSet[i];
            result = result + value;
        }

        return result;
    }
    
    [Benchmark]
    public Option<int> OptionMap_Some()
    {
        Option<int> result = Option.Some(1);

        for (int i = 0; i < _dataSet.Length; i++)
        {
            int value = _dataSet[i];
            result = result.Map(x => x + value);
        }

        return result;
    }
    
    [Benchmark]
    public Option<int> OptionBindMap_Some()
    {
        Option<int> result = Option.Some(1);
        
        for (int i = 0; i < _dataSet.Length; i++)
        {
            int value = _dataSet[i];
            result = result.MapBind((x, y) => x + y, value);
        }

        return result;
    }

    #endregion
    
    #region None

    [Benchmark]
    public int? Baseline_None()
    {
        int? result = null;
        
        for (int i = 0; i < _dataSet.Length; i++)
        {
            int value = _dataSet[i];
            result = result + value;
        }

        return result;
    }
    
    [Benchmark]
    public Option<int> OptionMap_None()
    {
        Option<int> result = Option.None<int>();

        for (int i = 0; i < _dataSet.Length; i++)
        {
            int value = _dataSet[i];
            result = result.Map(x => x + value);
        }

        return result;
    }
    
    [Benchmark]
    public Option<int> OptionBindMap_None()
    {
        Option<int> result = Option.None<int>();
        
        for (int i = 0; i < _dataSet.Length; i++)
        {
            int value = _dataSet[i];
            result = result.MapBind((x, y) => x + y, value);
        }

        return result;
    }

    #endregion
}