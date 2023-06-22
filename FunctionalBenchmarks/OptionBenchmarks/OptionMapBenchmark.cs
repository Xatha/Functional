using BenchmarkDotNet.Attributes;
using Functional.Option;

// Setup is always called once before running any benchmarks, so our private field _dataSet is initialized here.
#pragma warning disable CS8618 

namespace FunctionalBenchmarks.OptionBenchmarks;

[MemoryDiagnoser]
public class OptionMapBenchmark
{
    private int[] _dataSet;

    [GlobalSetup]
    public void Setup()
    {
        _dataSet = DataGenerator.GenerateIntDataSet();
    }
    
    [Benchmark]
    public int Baseline()
    {
        int result = 1;
        
        for (int i = 0; i < _dataSet.Length; i++)
        {
            int value = _dataSet[i];
            result = result + value;
        }

        return result;
    }
    
    [Benchmark]
    public Option<int> OptionMap()
    {
        Option<int> result = Option<int>.Some(1);
        
        for (int i = 0; i < _dataSet.Length; i++)
        {
            int value = _dataSet[i];
            result = result.Map(x => x + value);
        }

        return result;
    }
    
    [Benchmark]
    public Option<int> OptionBindMap()
    {
        Option<int> result = Option<int>.Some(1);
        
        for (int i = 0; i < _dataSet.Length; i++)
        {
            int value = _dataSet[i];
            result = result.MapBind((x, y) => x + y, value);
        }

        return result;
    }
}