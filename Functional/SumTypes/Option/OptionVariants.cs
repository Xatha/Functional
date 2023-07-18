namespace Functional.SumTypes;


/// <summary>
/// A class containing the possible variants of an <see cref="Option{T}"/> as constants.
/// This is used to provider a richer switch pattern matching experience.
/// </summary>
/// <example>
/// Example usages of pattern matching. Notice the static import of OptionVariants.
/// <code>
/// using static Functional.OptionFeature.OptionVariants;
/// ...
/// public Option&lt;string&gt; ConcatenateName(Person person)
/// {
///     Option&lt;string&gt; firstName = person.FirstName;
///     Option&lt;string&gt; lastName = person.LastName;
/// 
///     Option&lt;(string, string)&gt; fullName = firstName.Concat(lastName);
/// 
///     return fullName switch
///     {
///         (Some, var (first, last)) =&gt; Option.Some($"{first} {last}"),
///         (None, _) =&gt; Option.None&lt;string&gt;()
///     };                           
/// }
/// </code>
/// </example>
public static class OptionVariants
{
    public const bool Some = true;
    public const bool None = false;
}