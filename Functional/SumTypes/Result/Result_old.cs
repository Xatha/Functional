namespace Functional.SumTypes;

/*
public readonly struct Result
{
    private readonly ResultError _error;
    private readonly bool _isSuccess;

    private Result(bool isSuccess)
    {
        _isSuccess = isSuccess;
        _error = default;
    }
    
    private Result(ResultError error)
    {
        _isSuccess = false;
        _error = error;
    }
    
    /// <summary>
    /// Gets the error message, if present.
    /// </summary>
    /// <returns>True if result failed, otherwise false.</returns>
    public bool IsFailed(out ResultError error)
    {
        error = _error;
        return !_isSuccess;
    }

    /// <summary>
    /// Creates a success result with no value.
    /// </summary>
    /// <returns>Result indicating success.</returns>
    public static Result Ok() => new(true);

    /// <summary>
    /// Creates a failure result with no value.
    /// </summary>
    /// <returns>Result indicating failure.</returns>
    public static Result Err() => new(false);
    
    /// <summary>
    /// Creates a failure result with no value. The error message is used to provide information about the error.
    /// </summary>
    /// <returns>Result indicating failure.</returns>
    public static Result Err(ResultError error) => new(error);
    
    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <param name="value">Value you wish to wrap.</param>
    /// <returns>Value wrapped in a Result type.</returns>
    public static Result<TResult> Ok<TResult>(TResult value) => new(value);
    
    /// <summary>
    /// Creates a failure result with information about the error.
    /// </summary>
    /// <param name="error">Information about the error.</param>
    /// <remarks><see cref="ResultError"/> supports implicit overloading for <see cref="string"/>, thus you can pass <see cref="string"/> as argument.</remarks>
    /// <returns>Error wrapped in a Result type.</returns>
    public static Result<TResult> Err<TResult>(ResultError error) => new(error);
    
    /// <summary>
    /// Creates a failure result with no information about the error.
    /// </summary>
    /// <returns>Empty error wrapped in a Result type.</returns>
    public static Result<TResult> Err<TResult>() => new(ResultError.Empty);
}


public readonly struct Result<TResult>
{
    private readonly TResult _value;
    private readonly ResultError _error;
    private readonly bool _isSuccess;

    internal Result(TResult value)
    {
        _isSuccess = true;
        _value = value;
    }

    internal Result(ResultError error)
    {
        _isSuccess = false;
        _value = default!; //Might be a bad idea (?)
        _error = error;
    }

    /// <summary>
    /// Attempts to unwrap the result. If the result is a success, the value is safe to use. If the result is a failure, the error is safe to use.
    /// It is recommended you use this in a pattern matching fashion, for example in an if-statement.
    /// </summary>
    /// <param name="error">Result error, if value is invalid.</param>
    /// <param name="value">The unwrapped value, if the value is valid.</param>
    /// <example> Example usage:
    /// <code>
    /// Result&lt;double&gt; Div(int a, int b)
    /// {
    ///    if (b == 0) return "Divide by zero!";
    ///    return (double)a / b;
    /// }
    ///
    /// Result&lt;double&gt; result = Div(4, 2);
    /// if (result.TryUnwrap(out ResultError error, out int value))
    /// {
    ///     Console.WriteLine($"Result: {value}");
    /// }
    /// else
    /// {
    ///     Console.WriteLine($"Error: {error}");
    /// }
    /// // Prints "Result: 2"
    /// </code>
    /// </example>
    /// <returns>True if value is present, otherwise false.</returns>
    public bool TryUnwrap(out ResultError error, out TResult value)
    {
        error = _error; 
        value = _value;
        return _isSuccess;
    }
    
    /// <summary>
    /// Attempts to unwrap the result. If the result is a success, the value is safe to use.
    /// It is recommended you use this in a pattern matching fashion, for example in an if-statement.
    /// </summary>
    /// <param name="value">The unwrapped value, if the value is valid. If invalid default is used.</param>
    /// <example> Example usage:
    /// <code>
    /// Result&lt;double&gt; Div(int a, int b)
    /// {
    ///    if (b == 0) return "Divide by zero!";
    ///    return (double)a / b;
    /// }
    ///
    /// Result&lt;double&gt; result = Div(4, 2);
    /// if (result.TryUnwrap(out int value))
    /// {
    ///     Console.WriteLine($"Result: {value}");
    /// }
    /// // Prints "Result: 2"
    /// </code>
    /// </example>
    /// <returns>True if value is present, otherwise false.</returns>
    public bool TryUnwrap(out TResult value)
    {
        value = _value;
        return _isSuccess;
    }

    /// <summary>
    /// Matches the result with a success and failure function. The success function is called if the result is a success, otherwise the failure function is called.
    /// </summary>
    /// <param name="success">Function to call if value exists.</param>
    /// <param name="failure">Function to call if value does not exist.</param>
    /// <returns>Result after being passed through one of the provided functions.</returns>
    public Result<TResult> Match(
        Func<TResult, Result<TResult>> success, 
        Func<ResultError, Result<TResult>> failure)
    {
        return _isSuccess ? success(_value) : failure(_error);
    }

    /// <summary>
    /// Matches the result with a success and failure function. The success function is called if the result is a success, otherwise the failure function is called.
    /// </summary>
    /// <param name="success">Function to call if value exists.</param>
    /// <param name="failure">Function to call if value does not exist.</param>
    public void Match(
        Action<TResult> success,
        Action<ResultError> failure)
    {
        if (_isSuccess)
        {
            success(_value);
        }
        else
        {
            failure(_error);
        }
    }
    
    public static implicit operator Result<TResult>(TResult value) => Ok(value);

    public static implicit operator Result<TResult>(ResultError error) => Err(error);

    //public static implicit operator Result<TResult>(string error) => Err(error);
    
    
    internal static Result<TResult> Ok(TResult value) => new(value);
    
    internal static Result<TResult> Err(ResultError error) => new(error);
    
    internal static Result<TResult> Err() => new(ResultError.Empty);
}

*/
