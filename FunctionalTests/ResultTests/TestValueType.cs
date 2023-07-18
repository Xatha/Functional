namespace FunctionalTests.ResultTests;

public class TestValueType
{
    public TestReferenceType? NullReference { get; }
    
    public TestValueType Value { get; }
    public TestValueType? NullValue { get; }
    
    public TestValueType()
    {
        NullReference = null;
        Value = this;
        NullValue = null;
    }
}