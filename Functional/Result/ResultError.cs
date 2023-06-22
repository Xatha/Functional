namespace Functional.Result;

public readonly struct ResultError
{
    public string Message { get; }
    
    private ResultError(string message)
    {
        Message = message;
    }
    
    public static implicit operator ResultError(string message) => new(message);

    public static ResultError Create(string message) => new(message);

    public static ResultError Empty => new(string.Empty);
}