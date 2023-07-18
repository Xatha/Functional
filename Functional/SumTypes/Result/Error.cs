using System.Runtime.CompilerServices;
using static Functional.SumTypes.OptionVariants;

namespace Functional.SumTypes;

public readonly struct Error
{
    private readonly int _errorCount;
    
    public static Error Empty { get; } = new Error() { Message = string.Empty };
    public required string Message { get; init; }

    public Error() { }

    private Error(int errorCount)
    {
        _errorCount = errorCount;
    }

    public static Error From(
        string message,
        [CallerMemberName] string caller = "",
        [CallerFilePath] string path = "",
        [CallerLineNumber] int line = 0)
    {
        string formattedMessage = FormatMessage(message, caller, path, line, 0);
        return new Error { Message = formattedMessage };
    }

    public static Error From(
        Exception exception, 
        [CallerMemberName] string caller = "", 
        [CallerFilePath] string path = "", 
        [CallerLineNumber] int line = 0)
    {
        string formattedMessage = FormatMessage(exception.Message, caller, path, line, 0);
        return new Error { Message = formattedMessage };
    }

    public Error Append(
        string message,         
        [CallerMemberName] string caller = "", 
        [CallerFilePath] string path = "", 
        [CallerLineNumber] int line = 0)
    {
        string previousMessage = Message;
        int errorCount = _errorCount + 1;
        string formattedMessage = FormatMessage(message, previousMessage, 
            caller, path, line, errorCount);
        return new(errorCount) { Message = formattedMessage};
    }

    private static string FormatMessage(string? message, string caller, 
        string path, int line, int errorCount)
    {
        string heading = errorCount == 0 ? "Error:" : string.Empty;
        string messageBody = message is null ? string.Empty : $"\"{message}\"";
        string callerBody = $"at {caller}() in {String.Join('/', 
            path.Split("/")[^3..])}:{line}";
        
        return $"{heading} {messageBody} {callerBody}";
    }
    
    private static string FormatMessage(string message, string previousMessage, 
        string caller, string path, int line, int errorCount)
    {
        return $"{previousMessage}\n   {errorCount}: {FormatMessage(message, 
            caller, path, line, errorCount)}";
    }


    public override string ToString() => Message;
}
