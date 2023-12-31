using FunctionalTests.ResultTests;

namespace FunctionalTests;

public class TestReferenceType : IEquatable<TestReferenceType>
{
    public int Value { get; set; }

    public TestReferenceType()
    {
        Value = 42;
    }

    public bool Equals(TestReferenceType? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TestReferenceType)obj);
    }

    public override int GetHashCode()
    {
        return Value;
    }
}