using Functional;
using Functional.SumTypes;

namespace FunctionalTests.OptionTests;

/* Test names follow the convention of:
 * MethodName_StateUnderTest_ExpectedBehavior
 * -------------------------------------------
 * Tests are grouped into regions based on the method/properties they are testing.
 * Test classes that test generic types must be generic themselves where T is restricted to new().
 *
 * Generic test classes must be run with the following attribute:
 * [TestFixture(typeof(Unit))]
 * [TestFixture(typeof(TestReferenceType))]
 * [TestFixture(typeof(TestValueType))]
 * To ensure that the tests are run for all types.
 */

public class OptionExtensionsTests
{
    #region ToOption method

    [Test]
    public void ToOption_OnNullObject_ReturnsNoneOption()
    {
        // Arrange
        TestReferenceType? referenceInput = null;
        TestValueType? structInput = null;

        string resultReference;
        string resultStruct;

        // Act
        resultReference = referenceInput.ToOption().Map(_ => "Some").Collapse("None");
        resultStruct = structInput.ToOption().Map(_ => "Some").Collapse("None");

        // Assert
        Assert.That(resultReference, Is.EqualTo("None"));
        Assert.That(resultStruct, Is.EqualTo("None"));
    }

    [Test]
    public void ToOption_OnNotNull_ReturnsSomeOption()
    {
        // Arrange
        TestReferenceType? referenceInput = new();
        TestValueType? structInput = new();

        string resultReference;
        string resultStruct;

        // Act
        resultReference = referenceInput.ToOption().Map(_ => "Some").Collapse("None");
        resultStruct = structInput.ToOption().Map(_ => "Some").Collapse("None");

        // Assert
        Assert.That(resultReference, Is.EqualTo("Some"));
        Assert.That(resultStruct, Is.EqualTo("Some"));
    }
    
    #endregion
}