namespace Functional.SumTypes;

public interface IErrorMessage
{
    public Option<Exception> InnerException  { get; }

    public Option<string> Message { get; }
}