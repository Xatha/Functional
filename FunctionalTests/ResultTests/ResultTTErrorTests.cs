using Functional;
using Functional.SumTypes;

namespace FunctionalTests.ResultTests;

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

[TestFixture(typeof(Unit), typeof(TestReferenceType))]
[TestFixture(typeof(TestReferenceType), typeof(TestValueType))]
[TestFixture(typeof(TestValueType), typeof(TestReferenceType))]
public class ResultTests<T, TError> where T : new() where TError : new()
{
 [SetUp]
    public void Setup()
    {
        _rawValue = new T();
        _rawError = new TError();
        _okResult = Result.Ok<T, TError>(_rawValue);
        _errResult = Result.Err<T, TError>(new TError());
    }

    private T _rawValue = default!;
    private TError _rawError = default!;
    private Result<T, TError> _okResult;
    private Result<T, TError> _errResult;

    #region IsOk & IsErr properties

    [Test]
    public void IsOk_ResultOk_ReturnsTrue()
    {
        // Arrange
        var successResult = _okResult;

        // Assert
        Assert.That(successResult.IsOk, Is.EqualTo(true));
        Assert.That(successResult.IsErr, Is.EqualTo(false));
    }

    [Test]
    public void IsErr_ResultErr_ReturnsTrue()
    {
        // Arrange
        var failureResult = _errResult;

        // Assert
        Assert.That(failureResult.IsOk, Is.EqualTo(false));
        Assert.That(failureResult.IsErr, Is.EqualTo(true));
    }

    #endregion

    #region Implicit conversion
    
    [Test]
    public void ImplicitConversion_FromOkToResult_IsOkResult()
    {
        Result<T, TError> successResult = _rawValue;

        Assert.That(successResult.IsOk, Is.EqualTo(true));
        Assert.That(successResult.IsErr, Is.EqualTo(false));
    }

    [Test]
    public void ImplicitConversion_FromNullToResult_IsErrResult()
    {
        if (typeof(T).IsValueType) Assert.Pass();

        var nullable = default(T);
        Result<T, TError> failureResult = nullable;

        Assert.That(nullable, Is.Null);
        Assert.That(failureResult.IsOk, Is.EqualTo(false));
        Assert.That(failureResult.IsErr, Is.EqualTo(true));
    }

    [Test]
    public void ImplicitConversion_FromErrToResult_IsErrResult()
    {
        var failureResult = _errResult;

        Assert.That(failureResult.IsOk, Is.EqualTo(false));
        Assert.That(failureResult.IsErr, Is.EqualTo(true));
    }

    #endregion

    #region Match Method
    
    [Test]
    public void Match_ResultOk_ExecutesOkFunc()
    {
        // Arrange
        var successResult = _okResult;
        var okFuncExecuted = false;
        var errFuncExecuted = false;

        void OkFunc()
        {
            okFuncExecuted = true;
        }

        void ErrFunc()
        {
            errFuncExecuted = true;
        }

        // Act
        successResult.Match(
            _ => OkFunc(),
            _ => ErrFunc());

        // Assert
        Assert.That(okFuncExecuted, Is.True);
        Assert.That(errFuncExecuted, Is.False);
    }

    [Test]
    public void Match_ResultErr_ExecutesErrFunc()
    {
        // Arrange
        var errorResult = _errResult;
        var okFuncExecuted = false;
        var errFuncExecuted = false;

        void OkFunc()
        {
            okFuncExecuted = true;
        }

        void ErrFunc()
        {
            errFuncExecuted = true;
        }

        // Act
        errorResult.Match(
            _ => OkFunc(),
            _ => ErrFunc());

        // Assert
        Assert.That(okFuncExecuted, Is.False);
        Assert.That(errFuncExecuted, Is.True);
    }

    [Test]
    public void Match_ResultOk_ReturnsOkFuncResult()
    {
        // Arrange
        var successResult = _okResult;

        bool OkFunc()
        {
            return true;
        }

        bool ErrFunc()
        {
            return false;
        }

        // Act
        var result = successResult.Match(
            _ => OkFunc(),
            _ => ErrFunc());

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void Match_ResultErr_ReturnsErrFuncResult()
    {
        // Arrange
        var errorResult = _errResult;

        bool OkFunc()
        {
            return true;
        }

        bool ErrFunc()
        {
            return false;
        }

        // Act
        var result = errorResult.Match(
            _ => OkFunc(),
            _ => ErrFunc());

        // Assert
        Assert.That(result, Is.False);
    }
    
    #endregion
    
    #region AndThen Method

    [Test]
    public void AndThen_ResultOk_ReturnOkFuncResult()
    {
        // Arrange
        var successResult = _okResult;

        Result<bool, TError> NextResult(T ok)
        {
            return true;
        }

        // Act
        var resultFromResultFunc = successResult.AndThen(NextResult).Match(
            ok => ok,
            _ => false);

        var resultFromUnwrappedType = successResult.AndThen(_ => true).Match(
            ok => ok,
            _ => false);

        // Assert
        Assert.That(resultFromResultFunc, Is.True);
        Assert.That(resultFromUnwrappedType, Is.True);
    }

    [Test]
    [TestCase(10)]
    [TestCase(100)]
    [TestCase(1000)]
    public void AndThen_CanChainCalls(int nCalls)
    {
        // Arrange
        Result<int, TError> successResult = 0;

        Result<int, TError> NextResult(int ok)
        {
            return ok + 1;
        }

        Result<int, TError> FailureResult(int ok)
        {
            return Result.Err<int, TError>(_rawError);
        }

        // Act
        var resultOk = successResult;
        var resultErr = successResult;
        for (var i = 0; i < nCalls; i++)
        {
            resultOk = resultOk.AndThen(NextResult);
            resultErr = resultErr.AndThen(NextResult);
            if (i == nCalls / 2) resultErr = resultOk.AndThen(FailureResult);
        }

        // Assert
        Assert.That(resultOk.IsOk);
        Assert.That(resultOk.Unwrap(), Is.EqualTo(nCalls));

        Assert.That(resultErr.IsErr);
    }

    [Test]
    public void AndThen_ResultErr_ShortCircuitsOnErr()
    {
        // Arrange
        var errorResult = _errResult;

        Result<bool, TError> NextResult(T ok)
        {
            return true;
        }

        Result<bool, TError> NextResult2(bool ok)
        {
            Assert.Fail();
            return false;
        }

        // Act
        var resultFromResultFunc = errorResult.AndThen(NextResult)
            .AndThen(NextResult2).Match(
                ok => ok,
                _ => false);

        var resultFromUnwrappedType = errorResult.AndThen(_ => true)
            .AndThen(NextResult2).Match(
                ok => ok,
                _ => false);

        // Assert
        Assert.That(resultFromResultFunc, Is.False);
        Assert.That(resultFromUnwrappedType, Is.False);
    }
    
    #endregion

    #region Unwrap method
    
    [Test]
    public void Unwrap_ResultOk_ReturnsValue()
    {
        // Arrange
        var value = _rawValue;
        Result<T, TError> successResult = value;

        // Act
        var result = successResult.Unwrap();

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Unwrap_ResultErr_ThrowsException()
    {
        // Arrange
        var errorResult = _errResult;

        // Assert
        Assert.That(() => errorResult.Unwrap(),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }
    
    #endregion

    #region Expect method
    
    [Test]
    public void Expect_ResultOk_ReturnsValue()
    {
        // Arrange
        var value = _rawValue;
        Result<T, TError> successResult = value;

        // Act
        var result = successResult.Expect("Expected a value");

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Expect_ResultErr_ThrowsException()
    {
        // Arrange
        var errorResult = _errResult;

        // Assert
        Assert.That(() => errorResult.Expect("Expected a value"),
            Throws.TypeOf<InvalidOperationException>()
                .With.Message.Contain("Expected a value"));
    }
    
    #endregion
}