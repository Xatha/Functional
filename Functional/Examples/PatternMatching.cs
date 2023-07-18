using Functional.SumTypes;
using static Functional.SumTypes.OptionVariants;

namespace Functional.Examples;

public class PatternMatching
{
    # region Example1
    public Option<string> ConcatenateName(Person person)
    {
        Option<string> firstName = person.FirstName;
        Option<string> lastName = person.LastName;

        Option<(string, string)> fullName = firstName.Concat(lastName);

        return fullName switch
        {
            (Some, var (first, last)) => SumTypes.Option.Some($"{first} {last}"),
            (None, _) => SumTypes.Option.None<string>()
        };
    }
    # endregion
}

public record Person(Option<string> FirstName, Option<string> LastName);