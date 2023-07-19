namespace Functional.SumTypes;

// Used for cleaner syntax when using creating a new Option with a value.
public static class Option
{
    public static Option<T> Some<T>(T value)
    {
        return new(value);
    }
    
    public static Option<T> None<T>()
    {
        return new();
    }
}

public readonly struct Option<T> : IEquatable<Option<T>>
{
    private readonly bool _isSome;
    private readonly T? _value;

    /// <summary>
    /// Creates a new Option with no value. Not recommended to use directly, use <see cref="Option{T}.None()"/> instead.
    /// </summary>
    public Option()
    {
        _value = default;
        _isSome = false;
    }

    /// <summary>
    /// Creates a new <see cref="Option{T}"/> with a value. Not recommended to use directly, use <see cref="Option.Some{T}(T)"/> instead.
    /// </summary>
    /// <param name="value">Value to wrap.</param>
    public Option(T value)
    {
        _value = value;
        _isSome = true;
    }
    
    internal static Option<T> Some(T value)
    {
        return new(value);
    }

    internal static Option<T> None()
    {
        return new();
    }

    #region Consume

    public void Consume(Action<T> action)
    {
        if (_isSome)
        {
            action(_value!);
        }
    }
    
    #endregion
    
    #region Map
    
    public OptionTaskWrapper<TResult> Map<TResult>(Func<T, Task<TResult>> func)
    {
        return _isSome ? OptionTaskWrapper.Some(func(_value!)) : OptionTaskWrapper.None<TResult>();
    }

    public Option<TResult> Map<TResult>(Func<T, TResult> func)
    {
        return _isSome ? new Option<TResult>(func(_value!)) : new Option<TResult>();
    }

    public Option<TResult> Map<TResult>(Func<T, Option<TResult>> func)
    {
        return _isSome ? func(_value!) : new Option<TResult>();
    }
    
    public Option<T> MapNone(Func<T> func)
    {
        return _isSome ? this : new Option<T>(func());
    }
    
    public Option<T> MapNone(Func<Option<T>> func)
    {
        return _isSome ? this : func();
    }

    #endregion

    #region Match

    public Option<T> Match(Action<T> some, Action none)
    {
        if (_isSome)
        {
            some(_value!);
        }
        else
        {
            none();
        }

        return this;
    }

    #endregion
    
    #region Collapse

    public T Collapse(T orElse)
    {
        return _isSome ? _value! : orElse;
    }

    //Lazily evaluated
    public T Collapse(Func<T> orElse)
    {
        return _isSome ? _value! : orElse();
    }

    #endregion

    #region Cast

    public Option<TResult> Cast<TResult>()
    {
        return _isSome && _value is TResult casted ? Option.Some(casted) : Option<TResult>.None();
    }

    #endregion
    
    #region Transform Option

    public Option<TResult> Transform<T1, TResult>(
        Option<T1> arg1,
        Func<T, T1, TResult> func)
    {
        if (_isSome && arg1._isSome)
        {
            return new Option<TResult>(func(_value!, arg1._value!));
        }

        return Option<TResult>.None();
    }

    public Option<TResult> Transform<T1, T2, TResult>(
        Option<T1> arg1,
        Option<T2> arg2,
        Func<T, T1, T2, TResult> func)
    {
        if (_isSome && arg1._isSome && arg2._isSome)
        {
            return new Option<TResult>(func(_value!, arg1._value!, arg2._value!));
        }

        return Option<TResult>.None();
    }

    public Option<TResult> Transform<T1, T2, T3, TResult>(
        Option<T1> arg1,
        Option<T2> arg2,
        Option<T3> arg3,
        Func<T, T1, T2, T3, TResult> func)
    {
        if (_isSome && arg1._isSome && arg2._isSome && arg3._isSome)
        {
            return new Option<TResult>(func(_value!, arg1._value!, arg2._value!, arg3._value!));
        }

        return Option<TResult>.None();
    }

    public Option<TResult> Transform<T1, T2, T3, T4, TResult>(
        Option<T1> arg1,
        Option<T2> arg2,
        Option<T3> arg3,
        Option<T4> arg4,
        Func<T, T1, T2, T3, T4, TResult> func)
    {
        if (_isSome && arg1._isSome && arg2._isSome && arg3._isSome && arg4._isSome)
        {
            return new Option<TResult>(func(_value!, arg1._value!, arg2._value!, arg3._value!,
                arg4._value!));
        }

        return Option<TResult>.None();
    }

    public Option<TResult> Transform<T1, T2, T3, T4, T5, TResult>(
        Option<T1> arg1,
        Option<T2> arg2,
        Option<T3> arg3,
        Option<T4> arg4,
        Option<T5> arg5,
        Func<T, T1, T2, T3, T4, T5, TResult> func)
    {
        if (_isSome && arg1._isSome && arg2._isSome && arg3._isSome && arg4._isSome &&
            arg5._isSome)
        {
            return new Option<TResult>(func(_value!, arg1._value!, arg2._value!, arg3._value!,
                arg4._value!, arg5._value!));
        }

        return Option<TResult>.None();
    }

    #endregion
    
    #region Operators

    public static bool operator ==(Option<T>? left, Option<T>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Option<T>? left, Option<T>? right)
    {
        return !Equals(left, right);
    }

    #endregion

    #region Equals & GetHashCode Overrides

    public override bool Equals(object? obj)
    {
        return obj is Option<T> other && Equals(other);
    }

    public bool Equals(Option<T> other)
    { 
        return _isSome == other._isSome && EqualityComparer<T?>.Default.Equals(_value, other._value);
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(_isSome, _value);
    }

    #endregion

    //TODO: probably temporary, might fully implement later or remove
    public Option<TReturn> MapBind<TReturn, TArg2>(Func<T, TArg2, TReturn> func, TArg2 arg2)
    {
        return _isSome ? new Option<TReturn>(func(_value!, arg2)) : new Option<TReturn>();
    }

    public TResult CollapseBind<TArg, TResult>(Func<TArg, TResult> func, TArg arg)
    {
        return func(arg);
    }
    
    /// <summary>
    /// Combines two <see cref="Option{T}"/>s into a single <see cref="Option{T}"/> with a tuple of the values
    /// that is only Some if both <see cref="Option{T}"/>s are Some.
    /// </summary>
    /// <param name="second">The <see cref="Option{T}"/> to combine with.</param>
    /// <typeparam name="TSecond">The type of the second.</typeparam>
    /// <returns>An <see cref="Option{T}"/> of type (T, TSecond) that is Some if and only if both this and second are Some.</returns>
    public Option<(T, TSecond)> Concat<TSecond>(Option<TSecond> second)
    {
        return Transform(second, (f, s) => (f, s));
    }
    
    public void Deconstruct(out bool isSome, out T value)
    {
        isSome = _isSome;
        value =  _value!;
    }
} 