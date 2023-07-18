namespace Functional.SumTypes;

public readonly struct Result
{
    public static Result<TOk> Ok<TOk>(TOk ok) => new(ok);
    public static Result<TOk> Err<TOk>(Error error) => new(error);
    public static Result<TOk> Err<TOk>() => new();

    public static Result<Unit> Ok() => new(Unit.Default);
    public static Result<Unit> Err() => new();
    public static Result<Unit> Err(Error error) => new(error);

    #region TOk, TError
    
    public static Result<TOk, TError> Ok<TOk, TError>(TOk ok)
    {
        return ok;
    }

    public static Result<TOk, TError> Err<TOk, TError>(TError error)
    {
        return error;
    }

    #endregion
    
}

public readonly struct Result<TOk>
{
    private const string ErrorMessageHeading = "Cannot `unwrap()` an `Error` value.\n------------ERROR WALKTHROUGH-------------\n";
    private const string ErrorMessageFooter = "\n------------------------------------------";

    private readonly Either<TOk, Error> _either;
    
    public bool IsOk => _either.IsLeft;
    public bool IsErr => _either.IsRight;
    
    internal Result(TOk ok)
    {
        _either = Either.Left<TOk, Error>(ok);
    }
    
    internal Result(Error error)
    {
        _either = Either.Right<TOk, Error>(error);
    }

    public static implicit operator Result<TOk>(TOk? ok) => ok is not null ? new(ok) : new(Error.From("Null value"));
    public static implicit operator Result<TOk>(Error error) => new(error);
    
    /* TODO: These methods might not be needed.
    public static Result<TOk> Ok(TOk ok) => new(ok);
    public static Result<TOk> Err(Error error) => new(error);

    public static Result<TOk> Err(string error) => new(Error.From(error));
    public static Result<TOk> Err(Exception error) => new(Error.From(error));
    */

    #region Match

    public void Match(Action<TOk> ok, Action<Error> error) => _either.Match(ok, error);

    public TResult Match<TResult>(Func<TOk, TResult> ok, Func<Error, TResult> error) => _either.Match(ok, error);

    #endregion

    #region AndThen

    public Result<TNextOk> AndThen<TNextOk>(Func<TOk, Result<TNextOk>> func)
    {
        return _either switch
        {
            (EitherVariants.Left, var ok, _) => func(ok),
            (EitherVariants.Right, _, var error) => error
        };
    }
    public Result<TNextOk> AndThen<TNextOk>(Func<TOk, TNextOk> func)
    {
        return _either switch
        {
            (EitherVariants.Left, var ok, _) => func(ok),
            (EitherVariants.Right, _, var error) => error
        };
    }

    #endregion


    #region Unwrap

    public TOk Expect(string message)
    {
        return _either.Match(
            ok => ok,
            error => throw new InvalidOperationException(
                $"{message}\n------------ERROR WALKTHROUGH-------------\n{error.Message}{ErrorMessageFooter}"));
    }
    
    public TOk Unwrap()
    {
        return _either.Match(
            ok => ok,
            error => throw new InvalidOperationException(
                $"{ErrorMessageHeading}{error.Message}{ErrorMessageFooter}"));
    }
    
    #endregion
    
    public void Deconstruct(out bool isOk, out TOk ok, out Error error)
    {
        _either.Deconstruct(out isOk, out ok, out error);
    }

    
}

public readonly struct Result<TOk, TError>
{
    private readonly Either<TOk, TError> _either;

    internal Result(TOk ok)
    {
        throw new NotImplementedException();
    }

    internal Result(TError error)
    {
        throw new NotImplementedException();
    }

    public static implicit operator Result<TOk, TError>(TOk ok) => new(ok);
    public static implicit operator Result<TOk, TError>(TError error) => new(error);
}