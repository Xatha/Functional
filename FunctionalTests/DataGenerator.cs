namespace FunctionalTests;

public static class DataGenerator
{ 
    private const int SEED = 42;
    
    public static int[] GenerateDeterministicIntDataSet(int size)
    {
        Random random = new(SEED);
        int[] dataSet = new int[size];
        
        for (int i = 0; i < size; i++)
        {
            dataSet[i] = random.Next();
        }

        return dataSet;
    }

    public static string[] GenerateStringDataSet(int size)
    {
        Random random = new(SEED);
        string[] dataSet = new string[size];

        for (int i = 0; i < size; i++)
        {
            dataSet[i] = random.Next().ToString();
        }
        
        return dataSet;
    }
}