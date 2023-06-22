namespace FunctionalBenchmarks;

public static class DataGenerator
{
    private const int DATA_SET_SIZE = 1000;
    private const int SEED = 42;
    
    public static int[] GenerateIntDataSet()
    {
        Random random = new(SEED);
        int[] dataSet = new int[DATA_SET_SIZE];
        
        for (int i = 0; i < DATA_SET_SIZE; i++)
        {
            dataSet[i] = random.Next();
        }

        return dataSet;
    }

    public static string[] GenerateStringDataSet()
    {
        Random random = new(SEED);
        string[] dataSet = new string[DATA_SET_SIZE];

        for (int i = 0; i < DATA_SET_SIZE; i++)
        {
            dataSet[i] = random.Next().ToString();
        }
        
        return dataSet;
    }
}