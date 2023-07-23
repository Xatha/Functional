using Functional;

#pragma warning disable CS8073

namespace FunctionalTests.UnitTests;

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

public class UnitTests
{
    private Unit _unit;

    [SetUp]
    public void Setup()
    {
        _unit = new Unit();
    }

    #region Default property
    
    [Test]
    public void Default_ReturnsUnit()
    {
        // Arrange
        var expected = new Unit();
        
        // Act
        var actual = Unit.Default;
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    #endregion
    
    #region GetHashCode method
    
    [Test]
    public void GetHashCode_ReturnsZero()
    {
        // Arrange
        var expected = 0;
        
        // Act
        var actual = _unit.GetHashCode();
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    #endregion
    
    #region Equals method
    
    [Test]
    public void Equals_Unit_ReturnsTrue()
    {
        // Arrange
        var expected = true;
        
        // Act
        var actual = _unit.Equals(new Unit());
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void Equals_ObjectConvertableToUnit_ReturnsTrue()
    {
        // Arrange
        var expected = true;
        
        // Act
        var actual = _unit.Equals((object)new Unit());
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Equals_NotUnit_ReturnsFalse()
    {
        // Arrange
        var expected = false;
        
        // Act
        var actual = _unit.Equals(null!);
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    #endregion
    
    #region CompareTo method
    
    [Test]
    public void CompareTo_Unit_ReturnsZero()
    {
        // Arrange
        var expected = 0;
        
        // Act
        var actual = _unit.CompareTo(new Unit());
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    #endregion
    
    #region ToString method

    [Test]
    public void ToString_Returns()
    {
        // Arrange
        var expected = "()";
        
        // Act
        var actual = _unit.ToString();
        
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    #endregion

    #region Implicit conversion

    [Test]
    public void ImplicitConversion_FromValueTuple_ReturnsUnit()
    {
        ValueTuple valueTuple = new ValueTuple();
        Unit unit = valueTuple;
        
        Assert.That(unit, Is.EqualTo(new Unit()));
    } 

    #endregion
    
    #region Operators
    
    [Test]
    public void EqualsOperator_Unit_ReturnsTrue()
    {
        // Act
        var actual = _unit == new Unit();
        
        // Assert
        Assert.That(actual, Is.EqualTo(true));
    }
    
    [Test]
    public void NotEqualsOperator_Unit_ReturnsFalse()
    {
        // Act
        var actual = _unit != new Unit();
        
        // Assert
        Assert.That(actual, Is.EqualTo(false));
    }
    
    [Test]
    public void EqualsOperator_Null_ReturnsFalse()
    {
        // Act
        var actual = _unit == null!;
        
        // Assert
        Assert.That(actual, Is.EqualTo(false));
    }
    
    [Test]
    public void NotEqualsOperator_Null_ReturnsTrue()
    {
        // Act
        var actual = _unit != null!;
        
        // Assert
        Assert.That(actual, Is.EqualTo(true));
    }
    
    [Test]
    public void GreaterThanOperator_Unit_ReturnsFalse()
    {
        // Act
        var actual = _unit > new Unit();
        
        // Assert
        Assert.That(actual, Is.EqualTo(false));
    }
    
    [Test]
    public void GreaterThanOrEqualOperator_Unit_ReturnsTrue()
    {
        // Act
        var actual = _unit >= new Unit();
        
        // Assert
        Assert.That(actual, Is.EqualTo(true));
    }
    
    [Test]
    public void LessThanOperator_Unit_ReturnsFalse()
    {
        // Act
        var actual = _unit < new Unit();
        
        // Assert
        Assert.That(actual, Is.EqualTo(false));
    }
    
    [Test]
    public void LessThanOrEqualOperator_Unit_ReturnsTrue()
    {
        // Act
        var actual = _unit <= new Unit();
        
        // Assert
        Assert.That(actual, Is.EqualTo(true));
    }
    
    #endregion
}
