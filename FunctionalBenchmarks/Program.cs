using Functional;
using Functional.SumTypes;

namespace FunctionalBenchmarks;

public class Program
{
    static void Main()
    {
        //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();
        
        Result<int> result = Divide(10, 0);
        

        var next = result.Match(
            ok: Result.Ok,
            error: err => err.Append("Error 1"));
        
        var next2 = next.Match(
            ok: Result.Ok,
            error: err => err.Append("Error 2"));
        
        var next3 = next2.Match(
            ok: Result.Ok,
            error: err => err.Append("Error 3"));
        
        var next4 = next3.Match(
            ok: Result.Ok,
            error: err => err.Append("Error 4"));
        
        var next5 = next4.Match(
            ok: Result.Ok,
            error: kekw);
        
        next5.Match(
            ok: Console.WriteLine,
            error: err => Console.WriteLine(err.Message));
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        
        next5.Unwrap();
    }

    static Result<int> kekw(Error err) => err.Append("xD");
    
    public static Result<int> Divide(int a, int b)
    {
        try
        {
            return a / b;
        }
        catch (DivideByZeroException e)
        {
            return Error.From(e);
        }
    }
    
    public static Result<string, Error> PrintLine(string line)
    {
        try
        {
            Console.WriteLine(line);
            return "Success";
        }
        catch (Exception e)
        {
            return Error.From(e);
        }
    }
    
    public static Result<Unit> PrintLine2(string line)
    {
        try
        {
            Console.WriteLine(line);
            return Result.Ok(Sets.Unit);
        }
        catch (Exception e)
        {
            return Error.From(e);
        }
    }
}