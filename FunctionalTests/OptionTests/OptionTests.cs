using System.Diagnostics.CodeAnalysis;
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

[TestFixture(typeof(Unit))]
[TestFixture(typeof(TestReferenceType))]
[TestFixture(typeof(TestValueType))]
public class OptionTests<T> where T : new()
{
    [SetUp]
    public void Setup()
    {
        _rawValue = new T();
        _someOption = Option.Some(_rawValue);
        _noneOption = Option.None<T>();
    }

    private T _rawValue = default!;
    private Option<T> _someOption;
    private Option<T> _noneOption;

    #region Construction

    [Test]
    public void Some_TValue_ReturnsSomeOption()
    {
        // Arrange
        Option<T> option = _someOption; 
        
        // Act
        int result = option.Map(_ => 10).Collapse(-1);

        // Assert
        Assert.That(result, Is.EqualTo(10));
    }
    
    [Test]
    public void Some_Null_ReturnsNoneOption()
    {
        // Arrange
        Option<TestReferenceType> referenceOption = Option.Some<TestReferenceType>(null);
        Option<TestValueType> valueOption = Option.Some<TestValueType>(null);

        // Act
        int referenceResult = referenceOption.Map(_ => 10).Collapse(-1);
        int valueResult = valueOption.Map(_ => 10).Collapse(-1);

        // Assert
        Assert.That(referenceResult, Is.EqualTo(-1));
        Assert.That(valueResult, Is.EqualTo(-1));
    }
    

    #endregion
    
    #region Consume method

    [Test]
    public void Consume_Some_CallsAction()
    {
        // Arrange
        Option<T> option = _someOption;
        bool called = false;

        // Act
        option.Consume(_ => called = true);

        // Assert
        Assert.That(called, Is.True);
    }
    
    [Test]
    public void Consume_None_DoesNotCallAction()
    {
        // Arrange
        Option<T> option = _noneOption;
        bool called = false;

        // Act
        option.Consume(_ => called = true);

        // Assert
        Assert.That(called, Is.False);
    }
    

    #endregion
    
    #region Map method
    
    [Test]
    public void Map_Some_IsComputed()
    {
        // Arrange
        Option<T> option = _someOption;
        bool called1 = false, called2 = false;
        int result1, result2;
        
        // Act
        result1 = option.Map(_ =>
        {
            called1 = true;
            return 10;
        }).Collapse(-1);
        
        result2 = option.Map(_ =>
        {
            called2 = true;
            return Option.Some(10);
        }).Collapse(-1);
        
        // Assert
        Assert.That(result1, Is.EqualTo(10));
        Assert.That(result2, Is.EqualTo(10));
        
        Assert.That(called1, Is.True);
        Assert.That(called2, Is.True);
    }
    
    [Test]
    public void Map_None_ReturnsOrElseValue()
    {
        // Arrange
        Option<T> option = _noneOption;
        int result1, result2;

        // Act
        result1 = option.Map(_ => 10).Collapse(-1);
        result2 = option.Map(_ => Option.Some(10)).Collapse(-1);
        
        // Assert
        Assert.That(result1, Is.EqualTo(-1));
        Assert.That(result2, Is.EqualTo(-1));
    }

    #endregion

    #region NoneMap

    [Test]
    public void MapNone_Some_ReturnsOriginal()
    {
        // Arrange
        Option<T> option = _someOption;
        bool called1 = false;
        bool called2 = false;
        T result1;
        T result2;
        
        // Act
        result1 = option.MapNone(() =>
        {
            called1 = true;
            return new T();
        }).Collapse(null!);
        
        result2 = option.MapNone(() =>
        {
            called2 = true;
            return Option.Some(new T());
        }).Collapse(null!);
        
        // Assert
        Assert.That(result1, Is.EqualTo(_rawValue));
        Assert.That(result2, Is.EqualTo(_rawValue));
        Assert.That(called1, Is.False);
        Assert.That(called2, Is.False);
    }
    
    [Test]
    public void MapNone_None_ReturnsNewValue()
    {
        // Arrange
        Option<T> option = _noneOption;
        bool called1 = false;
        bool called2 = false;

        // Act
        option.MapNone(() =>
        {
            called1 = true;
            return new T();
        }).Collapse(null!);
        
        option.MapNone(() =>
        {
            called2 = true;
            return Option.Some(new T());
        }).Collapse(null!);
        
        // Assert
        Assert.That(called1, Is.True);
        Assert.That(called2, Is.True);
    }
    
    #endregion

    #region MapBind

    [Test]
    public void MapBind_Some_IsComputed()
    {
        // Arrange
        Option<T> option = _someOption;
        int closure = 42;
        bool called = false;
        int result;
        
        // Action
        result = option.MapBind((_, y) =>
        {
            called = true;
            return 10 + y;
        }, closure).Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo(52));
        Assert.That(called, Is.True);
    }
    
    [Test]
    public void MapBind_None_IsNotComputed()
    {
        // Arrange
        Option<T> option = _noneOption;
        int closure = 42;
        bool called = false;
        int result;
        
        // Action
        result = option.MapBind((_, y) =>
        {
            called = true;
            return 10 + y;
        }, closure).Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo(-1));
        Assert.That(called, Is.False);
    }

    #endregion

    #region NoneMap method
    
    // TODO: Look at if API has changed for this method.
    // Otherwise, look into how to test this with generics.
    
    #endregion

    #region Match method
    
    [Test]
    public void Match_Some_CallsSomeAction()
    {
        // Arrange
        Option<T> option = _someOption;
        bool someCalled = false;
        bool noneCalled = false;

        // Act
        option.Match(_ => someCalled = true, () => noneCalled = false);

        // Assert
        Assert.That(someCalled, Is.True);
        Assert.That(noneCalled, Is.False);
    }
    
    [Test]
    public void Match_None_CallsNoneAction()
    {
        // Arrange
        Option<T> option = _noneOption;
        bool someCalled = false;
        bool noneCalled = false;

        // Act
        option.Match(_ => someCalled = true, () => noneCalled = true);

        // Assert
        Assert.That(someCalled, Is.False);
        Assert.That(noneCalled, Is.True);
    }

    #endregion
    
    #region Collapse method

    [Test]
    public void Collapse_Some_FuncIsNotCalled()
    {
        // Arrange
        Option<T> option = _someOption;
        int result;
        bool called = false;
        
        // Act
        result = option.Map(_ => 10).Collapse(() => 
        {
            called = true;
            return -1;
        });
        
        // Assert
        Assert.That(result, Is.EqualTo(10));
        Assert.That(called, Is.False);
    }

    [Test]
    public void Collapse_None_ReturnsFuncValue()
    {
        // Arrange
        Option<T> option = _noneOption;
        int result;
        bool called = false;
        
        // Act
        result = option.Map(_ => 10).Collapse(() => 
        {
            called = true;
            return -1;
        });
        
        // Assert
        Assert.That(result, Is.EqualTo(-1));
        Assert.That(called, Is.True);
    }
    
    #endregion

    #region Cast method

    [Test]
    [SuppressMessage("ReSharper", "ConvertTypeCheckToNullCheck")]
    public void Cast_SomeAndCastIsValid_ReturnsSomeNewType()
    {
        // Arrange
        Option<object> option = Option.Some<object>(new T());
        Option<T> result;
        
        // Act
        result = option.Cast<T>();
        
        // Assert
        Assert.That(result.Map(_ => 10).Collapse(-1), Is.EqualTo(10));
        Assert.That(_rawValue is T, Is.True);
    }
    
    [Test]
    public void Cast_SomeAndCastIsNotValid_ReturnsNone()
    {
        // Arrange
        Option<T> option = _someOption;
        Option<int> result;
        
        // Act
        result = option.Cast<int>();
        
        // Assert
        Assert.That(result.Map(_ => 10).Collapse(-1), Is.EqualTo(-1));
        Assert.That(_rawValue is int, Is.False);
    }
    
        
    [Test]
    public void Cast_NoneAndCastIsValid_ReturnsNone()
    {
        // Arrange
        Option<T> option = _noneOption;
        Option<object> result;
        
        // Act
        result = option.Cast<object>();

        // Assert
        Assert.That(result.Map(_ => 10).Collapse(-1), Is.EqualTo(-1));
        Assert.That(_rawValue is object, Is.True);
    }

    [Test]
    public void Cast_NoneAndCastIsNotValid_ReturnsNone()
    {
        // Arrange
        Option<T> option = _noneOption;
        Option<int> result;
        
        // Act
        result = option.Cast<int>();
        
        // Assert
        Assert.That(result.Map(_ => 10).Collapse(-1), Is.EqualTo(-1));
        Assert.That(_rawValue is int, Is.False);
    }
    
    #endregion
    
    #region Transform method
    
    [Test]
    public void Transform_SomeAndSome_ReturnsSomeNewType()
    {
        // Arrange
        Option<T> option = _someOption;
        Option<int> other = Option.Some(10);
        bool called = false;
        int result;
        
        // Act
        result = option.Transform(other, (_, o) =>
            {
                called = true;
                return o + 32;
            })
            .Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo(42));
        Assert.That(called, Is.True);
    }
    
    [Test]
    public void Transform_SomeAndNone_ReturnsNone()
    {
        // Arrange
        Option<T> option = _someOption;
        Option<int> other = Option.None<int>();
        bool called = false;
        int result;
        
        // Act
        result = option.Transform(other, (_, o) =>
            {
                called = true;
                return o + 32;
            })
            .Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo(-1));
        Assert.That(called, Is.False);
    }
    
    [Test]
    public void Transform_NoneAndSome_ReturnsNone()
    {
        // Arrange
        Option<T> option = _noneOption;
        Option<int> other = Option.Some(10);
        bool called = false;
        int result;
        
        // Act
        result = option.Transform(other, (_, o) =>
            {
                called = true;
                return o + 32;
            })
            .Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo(-1));
        Assert.That(called, Is.False);
    }
    
    [Test]
    public void Transform_NoneAndNone_ReturnsNone()
    {
        // Arrange
        Option<T> option = _noneOption;
        Option<int> other = Option.None<int>();
        bool called = false;
        int result;
        
        // Act
        result = option.Transform(other, (_, o) =>
            {
                called = true;
                return o + 32;
            })
            .Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo(-1));
        Assert.That(called, Is.False);
    }
    
    #endregion
    
    #region Concat method

    [Test]
    public void Concat_SomeAndSome_ReturnsCombinedSome()
    {
        // Arrange
        Option<T> option = _someOption;
        Option<int> other = Option.Some(5);
        
        Option<(T, int)> result;

        // Act
        result = option.Concat(other);
        
        // Assert
        Assert.That(result.Map(t => t.Item2).Collapse(-1), Is.EqualTo(5));
    }
    
    [Test]
    public void Concat_SomeAndNone_ReturnsNone()
    {
        // Arrange
        Option<T> option = _someOption;
        Option<int> other = Option.None<int>();
        
        Option<(T, int)> result;

        // Act
        result = option.Concat(other);
        
        // Assert
        Assert.That(result.Map(t => t.Item2).Collapse(-1), Is.EqualTo(-1));
    }
    
    [Test]
    public void Concat_NoneAndSome_ReturnsNone()
    {
        // Arrange
        Option<T> option = _noneOption;
        Option<int> other = Option.Some(5);
        
        Option<(T, int)> result;

        // Act
        result = option.Concat(other);
        
        // Assert
        Assert.That(result.Map(t => t.Item2).Collapse(-1), Is.EqualTo(-1));
    }
    
    #endregion

    #region Operators

    [Test]
    public void OperatorEquals_SameReference_ReturnsTrue()
    {
        // Arrange
        Option<T> option = _someOption;
        Option<T> other = _someOption;
        
        // Act
        bool result = option == other;
        bool result2 = option.Equals(other);
        bool result3 = option != other;
        
        // Assert
        Assert.That(result, Is.True);
        Assert.That(result2, Is.True);
        Assert.That(result3, Is.False);
    }
    
    [Test]
    public void OperatorEquals_SameValue_ReturnsTrue()
    {
        // Arrange
        Option<T> option = _someOption;
        Option<T> other = Option.Some(new T());
        
        // Act
        bool result = option == other;
        bool result2 = option.Equals(other);
        bool result3 = option != other;
        
        // Assert
        Assert.That(result, Is.True);
        Assert.That(result2, Is.True);
        Assert.That(result3, Is.False);
    }
    
    [Test]
    public void OperatorEquals_DifferentReference_ReturnsFalse()
    {
        // Arrange
        Option<TestReferenceType> option = Option.Some(new TestReferenceType { Value = 10 });
        Option<TestReferenceType> other = Option.Some(new TestReferenceType { Value = 20 });
        
        // Act
        bool result = option == other;
        bool result2 = option.Equals(other);
        bool result3 = option != other;
        
        // Assert
        Assert.That(result, Is.False);
        Assert.That(result2, Is.False);
        Assert.That(result3, Is.True);
    }
    
    [Test]
    public void OperatorEquals_DifferentValue_ReturnsFalse()
    {
        // Arrange
        Option<TestValueType> option = Option.Some(new TestValueType { Value = 10 });
        Option<TestValueType> other = Option.Some(new TestValueType { Value = 20 });
        
        // Act
        bool result = option == other;
        bool result2 = option.Equals(other);
        bool result3 = option != other;
        
        // Assert
        Assert.That(result, Is.False);
        Assert.That(result2, Is.False);
        Assert.That(result3, Is.True);
    }
    

    #endregion
}