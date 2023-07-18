using Functional;
using Functional.SumTypes;

namespace FunctionalTests.ResultTests;

public class ResultTestFactory<T>
{
    #region IsOk & IsErr properties
    
    public void Result_OkIsOk(Func<Result<T>> factory)
    {
        // Arrange
        Result<T> successResult = factory();

        // Assert
        Assert.That(successResult.IsOk, Is.EqualTo(true));
        Assert.That(successResult.IsErr, Is.EqualTo(false));
    }

    public void Result_ErrIsErr(Func<Result<T>> factory)
    {
        // Arrange
        Result<T> failureResult;
        
        // Act
        failureResult = factory();
        
        // Assert
        Assert.That(failureResult.IsOk, Is.EqualTo(false));
        Assert.That(failureResult.IsErr, Is.EqualTo(true));
    }

    #endregion
    
    #region Implicit conversion
    
    public void Result_ImplicitConversionFromOkToResult_IsOkResult(
        Func<T> factory)
    {
        Result<T> successResult = factory();
        
        Assert.That(successResult.IsOk, Is.EqualTo(true));
        Assert.That(successResult.IsErr, Is.EqualTo(false));
    }
    
    public void Result_ImplicitConversionFromNullToResult_IsErrResult(
        Func<T?> factory)
    {
        T? nullable = factory();
        Result<T> failureResult = nullable;
        
        Assert.That(nullable, Is.Null);
        Assert.That(failureResult.IsOk, Is.EqualTo(false));
        Assert.That(failureResult.IsErr, Is.EqualTo(true));
    }

    public void Result_ImplicitConversionFromErrToResult_IsErrResult(
        Func<Error> factory)
    {
        Result<T> failureResult = factory();
        
        Assert.That(failureResult.IsOk, Is.EqualTo(false));
        Assert.That(failureResult.IsErr, Is.EqualTo(true));
    }

    #endregion

    #region Match method
    
    public void Result_Match_ExecutesOkFuncWhenResultOk(Func<T> factory)
    {
        // Arrange
        Result<T> successResult = factory();
        bool okFuncExecuted = false;
        bool errFuncExecuted = false;

        void OkFunc() => okFuncExecuted = true;
        void ErrFunc() => errFuncExecuted = true;
        
        // Act
        successResult.Match(
             ok:    _ => OkFunc(),
             error: _ => ErrFunc());
        
        // Assert
        Assert.That(okFuncExecuted, Is.True);
        Assert.That(errFuncExecuted, Is.False);
    }

    public void Result_Match_ExecutesErrFuncWhenResultErr(Func<Error> factory)
    {
        // Arrange
        Result<T> successResult = factory();
        bool okFuncExecuted = false;
        bool errFuncExecuted = false;

        void OkFunc() => okFuncExecuted = true;
        void ErrFunc() => errFuncExecuted = true;
        
        // Act
        successResult.Match(
            ok:    _ => OkFunc(),
            error: _ => ErrFunc());
        
        // Assert
        Assert.That(okFuncExecuted, Is.False);
        Assert.That(errFuncExecuted, Is.True);
    }

    public void Result_Match_ReturnsOkFuncResultWhenResultOk(Func<T> factory)
    {
        // Arrange
        Result<T> successResult = factory();

        bool OkFunc(T ok) => true;
        bool ErrFunc() => false;
        
        // Act
        bool result = successResult.Match(
            ok:    ok => OkFunc(ok),
            error: _ => ErrFunc());
        
        // Assert
        Assert.That(result, Is.True);
    }

    public void Result_Match_ReturnsErrFuncResultWhenResultErr(Func<Error> factory)
    {
        // Arrange
        Result<T> successResult = factory();

        bool OkFunc(T ok) => true;
        bool ErrFunc() => false;
        
        // Act
        bool result = successResult.Match(
            ok:    ok => OkFunc(ok),
            error: _ => ErrFunc());
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    #endregion

    #region AndThen method

    public void Result_AndThen_ReturnOkFuncResultWhenResultOk(Func<T> factory)
    {
        // Arrange
        Result<T> successResult = factory();

        Result<bool> NextResult(T ok) => true;

        // Act
        bool resultFromResultFunc = successResult.AndThen(NextResult).Match(
            ok => ok,
            err => false);
        
        bool resultFromUnwrappedType = successResult.AndThen(_ => true).Match(
            ok => ok,
            err => false);
        
        // Assert
        Assert.That(resultFromResultFunc, Is.True);   
        Assert.That(resultFromUnwrappedType, Is.True);
    }
    
    public void Result_AndThen_CanChainCalls(int nCalls)
    {
        // Arrange
        Result<int> successResult = 0;
        
        Result<int> NextResult(int ok) => ok + 1;
        Result<int> FailureResult(int ok) => Result.Err<int>();

        // Act
        Result<int> resultOk = successResult;
        Result<int> resultErr = successResult;
        for (int i = 0; i < nCalls; i++)
        {
            resultOk = resultOk.AndThen(NextResult);
            resultErr = resultErr.AndThen(NextResult);
            if (i == nCalls / 2)
            {
                resultErr = resultOk.AndThen(FailureResult);
            }
        }

        // Assert
        Assert.That(resultOk.IsOk);
        Assert.That(resultOk.Unwrap(), Is.EqualTo(nCalls));
        
        Assert.That(resultErr.IsErr);
    }
    
    public void Result_AndThen_ShortCircuitsWhenResultErr(Func<Error> factory)
    {
        // Arrange
        Result<T> successResult = factory();

        Result<bool> NextResult(T ok) => true;
        Result<bool> NextResult2(bool ok)
        {
            Assert.Fail();
            return false;
        }

        // Act
        bool resultFromResultFunc = successResult.AndThen(NextResult).AndThen(NextResult2).Match(
            ok => ok,
            err => false);
        
        bool resultFromUnwrappedType = successResult.AndThen(_ => true).AndThen(NextResult2).Match(
            ok => ok,
            err => false);
        
        // Assert
        Assert.That(resultFromResultFunc, Is.False);   
        Assert.That(resultFromUnwrappedType, Is.False); 
    }
    

    #endregion

    #region Unwrap & Except

    public void Result_Unwrap_ReturnsValueWhenResultOk(Func<T> factory)
    {
        // Arrange
        T value = factory();
        Result<T> successResult = value;
        
        // Act
        T result = successResult.Unwrap();
        
        // Assert
        Assert.That(result, Is.EqualTo(value));
    }
    
    public void Result_Unwrap_ThrowsExceptionWhenResultErr(Func<Error> factory)
    {
        // Arrange
        Result<T> successResult = factory();

        // Assert
        Assert.That(() => successResult.Unwrap(), 
            Throws.Exception.TypeOf<InvalidOperationException>());
    }

    public void Result_Expect_ReturnsValueWhenResultOk(Func<T> factory)
    {
        // Arrange
        T value = factory();
        Result<T> successResult = value;
        
        // Act
        T result = successResult.Expect("Expected a value");
        
        // Assert
        Assert.That(result, Is.EqualTo(value));
    }
    
    public void Result_Expect_ThrowsExceptionWhenResultErr(Func<Error> factory)
    {
        // Arrange
        Result<T> errorResult = factory();
        
        // Assert
        Assert.That(() => errorResult.Expect("Expected a value"),
            Throws.TypeOf<InvalidOperationException>()
                .With.Message.Contain("Expected a value"));
    }

    #endregion
}