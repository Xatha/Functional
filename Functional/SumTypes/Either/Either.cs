using System.Diagnostics;

namespace Functional.SumTypes;

public class Either
{
    public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value) => new(value);
    public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value) => new(value);
}

public readonly struct Either<TLeft, TRight>
{
    private readonly TLeft _left;
    private readonly TRight _right;
    
    public bool IsLeft { get; }
    public bool IsRight => !IsLeft;

    public Either()
    {
        Debug.Assert(false, "Either must be initialized with a value. This CTOR must not be used.");
        _left = default!;
        _right = default!;
        IsLeft = true;
    }

    public Either(TLeft value)
    {
        _left = value;
        IsLeft = true;
        _right = default!;
    }

    public Either(TRight value)
    {
        _left = default!;
        _right = value;
        IsLeft = false;
    }
    
    public bool Is<TType>()
    {
        return IsLeft 
            ? _left is TType 
            : _right is TType;
    }
    
    public Option<TType> As<TType>()
    {
        return IsLeft 
            ? Option.Some(_left).Cast<TType>() 
            : Option.Some(_right).Cast<TType>();
    }
    
    public Either<TNewLeft, TNewRight> Map<TNewLeft, TNewRight>(Func<TLeft, TNewLeft> left, Func<TRight, TNewRight> right)
    {
        return IsLeft 
            ? new Either<TNewLeft, TNewRight>(left(_left)) 
            : new Either<TNewLeft, TNewRight>(right(_right));
    }
    
    public Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> left)
    {
        return IsLeft 
            ? new Either<TNewLeft, TRight>(left(_left)) 
            : new Either<TNewLeft, TRight>(_right);
    }
    
    public Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> right)
    {
        return IsLeft 
            ? new Either<TLeft, TNewRight>(_left) 
            : new Either<TLeft, TNewRight>(right(_right));
    }
    
    public TResult Match<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
    {
        return IsLeft ? left(_left) : right(_right);
    }
    
    public void Match(Action<TLeft> left, Action<TRight> right)
    {
        if (IsLeft)
        {
            left(_left);
        }
        else
        {
            right(_right);
        }
    }

    public void Deconstruct(out bool isLeft, out TLeft left, out TRight right)
    {
        isLeft = IsLeft;
        left = _left;
        right = _right;
    }
}