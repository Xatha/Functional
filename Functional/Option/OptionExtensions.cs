namespace Functional.Option;

public static class OptionExtensions
{
    #region ToOption

    public static Option<T> ToOption<T>(this T? value) where T : class
    {
        return value is null ? Option<T>.None() : Option<T>.Some(value);
    }

    public static Option<T> ToOption<T>(this T? value) where T : struct
    {
        return value is null ? Option<T>.None() : Option<T>.Some(value.Value);
    }

    #endregion
}