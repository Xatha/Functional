namespace Functional.EitherFeature;

public readonly struct Either<T, U>
{
    internal readonly T _t;
    internal readonly U _u;
    
    private readonly Type _type;
    
    public Either(T value)
    {
        _t = value;
        _type = typeof(T);
    }
    
    public Either(U value)
    {
        _u = value;
        _type = typeof(U);
    }
    
    public bool Is<TType>()
    {
        return _type == typeof(TType);
    }
}