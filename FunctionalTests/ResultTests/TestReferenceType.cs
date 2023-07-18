namespace FunctionalTests.ResultTests;

public class TestReferenceType
{
    public TestReferenceType Reference { get; }
    public TestReferenceType? NullReference { get; }
    public TestValueType? NullValue { get; }
    
    public TestReferenceType()
    {
        Reference = this;
        NullReference = null;
        NullValue = null;
    }
}