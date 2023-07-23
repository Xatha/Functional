using Functional;
using Functional.SumTypes;

public class Program
{
    static async Task Main()
    {
        //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();
        
        Option<int> num = Option.Some(3);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken ctx = cancellationTokenSource.Token;
        ctx.Register(() => Console.WriteLine("Cancelled"));

        
        
        var x = num.Map(PlusOne)
            .Map(PlusOne)
            .Map(Multiply)
            .Map(PlusOne);

        await Task.Delay(2100);
        cancellationTokenSource.Cancel();
        await Print(x);


    }

    static async Task Print(OptionTaskWrapper<int> taskWrapper)
    {
        Console.WriteLine("Printing...");
        var res = await taskWrapper.Resolve();
        res.Match(
            some: i => Console.WriteLine(i),
            none: () => Console.WriteLine("None")
        );
    }   

    static async Task<int> PlusOne(int a)
    {
        Console.WriteLine("PlusOne");
        await Task.Delay(1000);
        return a + 1;
    }

    static async Task<int> Multiply(int a)
    {
        Console.WriteLine("Multiply");
        await Task.Delay(1000);
        return a * 2;
    }
    
    
    
}
