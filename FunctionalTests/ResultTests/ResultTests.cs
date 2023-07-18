using Functional;
using Functional.SumTypes;

namespace FunctionalTests.ResultTests;

[TestFixture(typeof(Unit))]
[TestFixture(typeof(TestReferenceType))]
[TestFixture(typeof(TestValueType))]
public class ResultTests<T> where T : new()
{
    [SetUp]
    public void Setup()
    {
        _rawValue = new T();
        _successResult = Result.Ok(_rawValue);
        _failureResult = Result.Err<T>(Error.Empty);
    }

    private T _rawValue = default!;
    private Result<T> _successResult;
    private Result<T> _failureResult;

    [Test]
    public void Result_OkIsOk()
    {
        // Arrange
        var successResult = _successResult;

        // Assert
        Assert.That(successResult.IsOk, Is.EqualTo(true));
        Assert.That(successResult.IsErr, Is.EqualTo(false));
    }

    [Test]
    public void Result_ErrIsErr()
    {
        // Arrange
        var failureResult = _failureResult;

        // Assert
        Assert.That(failureResult.IsOk, Is.EqualTo(false));
        Assert.That(failureResult.IsErr, Is.EqualTo(true));
    }

    [Test]
    public void Result_ImplicitConversionFromOkToResult_IsOkResult()
    {
        Result<T> successResult = _rawValue;

        Assert.That(successResult.IsOk, Is.EqualTo(true));
        Assert.That(successResult.IsErr, Is.EqualTo(false));
    }

    [Test]
    public void Result_ImplicitConversionFromNullToResult_IsErrResult()
    {
        if (typeof(T).IsValueType) Assert.Pass();

        var nullable = default(T);
        Result<T> failureResult = nullable;

        Assert.That(nullable, Is.Null);
        Assert.That(failureResult.IsOk, Is.EqualTo(false));
        Assert.That(failureResult.IsErr, Is.EqualTo(true));
    }

    [Test]
    public void Result_ImplicitConversionFromErrToResult_IsErrResult()
    {
        var failureResult = _failureResult;

        Assert.That(failureResult.IsOk, Is.EqualTo(false));
        Assert.That(failureResult.IsErr, Is.EqualTo(true));
    }

    [Test]
    public void Result_Match_ExecutesOkFuncWhenResultOk()
    {
        // Arrange
        var successResult = _successResult;
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
    public void Result_Match_ExecutesErrFuncWhenResultErr()
    {
        // Arrange
        var errorResult = _failureResult;
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
    public void Result_Match_ReturnsOkFuncResultWhenResultOk()
    {
        // Arrange
        var successResult = _successResult;

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
    public void Result_Match_ReturnsErrFuncResultWhenResultErr()
    {
        // Arrange
        var errorResult = _failureResult;

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

    [Test]
    public void Result_AndThen_ReturnOkFuncResultWhenResultOk()
    {
        // Arrange
        var successResult = _successResult;

        Result<bool> NextResult(T ok)
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
    public void Result_AndThen_CanChainCalls(int nCalls)
    {
        // Arrange
        Result<int> successResult = 0;

        Result<int> NextResult(int ok)
        {
            return ok + 1;
        }

        Result<int> FailureResult(int ok)
        {
            return Result.Err<int>();
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

    public void Result_AndThen_ShortCircuitsWhenResultErr()
    {
        // Arrange
        var errorResult = _failureResult;

        Result<bool> NextResult(T ok)
        {
            return true;
        }

        Result<bool> NextResult2(bool ok)
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

    [Test]
    public void Result_Unwrap_ReturnsValueWhenResultOk()
    {
        // Arrange
        var value = _rawValue;
        Result<T> successResult = value;

        // Act
        var result = successResult.Unwrap();

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Result_Unwrap_ThrowsExceptionWhenResultErr()
    {
        // Arrange
        var errorResult = _failureResult;

        // Assert
        Assert.That(() => errorResult.Unwrap(),
            Throws.Exception.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Result_Expect_ReturnsValueWhenResultOk()
    {
        // Arrange
        var value = _rawValue;
        Result<T> successResult = value;

        // Act
        var result = successResult.Expect("Expected a value");

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Result_Expect_ThrowsExceptionWhenResultErr()
    {
        // Arrange
        var errorResult = _failureResult;

        // Assert
        Assert.That(() => errorResult.Expect("Expected a value"),
            Throws.TypeOf<InvalidOperationException>()
                .With.Message.Contain("Expected a value"));
    }
}