using Functional;
using Functional.SumTypes;

namespace FunctionalTests.ResultTests;

[TestFixture]
public class ResultUnitTests : IResultTestsContract<Unit>
{
    private readonly ResultTestFactory<Unit> _resultTestFactory = new();
    
    #region IsOk & IsErr properties
    
    [Test]
    public void Result_IsOkProperty_OkWhenResultOk()
    {
        _resultTestFactory.Result_OkIsOk(Result.Ok);
    }

    [Test]
    public void Result_IsErrProperty_ErrWhenResultErr()
    {
        _resultTestFactory.Result_ErrIsErr(Result.Err);
    }

    #endregion

    #region Implicit conversion
    
    [Test]
    public void Result_ImplicitConversionFromOkToResult_IsOkResult()
    {
        _resultTestFactory.Result_ImplicitConversionFromOkToResult_IsOkResult(() => Unit.Default);
    }

    [Test]
    public void Result_ImplicitConversionFromNullToResult_IsErrResult()
    {
        // Unit is a struct, so it can't be null.
        Assert.Pass();
    }
    
    [Test]
    public void Result_ImplicitConversionFromErrToResult_IsErrResult()
    {
        _resultTestFactory.Result_ImplicitConversionFromErrToResult_IsErrResult(() => Error.Empty);
    }
    
    #endregion
    
    #region Match method
    
    [Test]
    public void Result_Match_ExecutesOkFuncWhenResultOk()
    {
        _resultTestFactory.Result_Match_ExecutesOkFuncWhenResultOk(() => Unit.Default);
    }
    
    [Test]
    public void Result_Match_ExecutesErrFuncWhenResultErr()
    {
        _resultTestFactory.Result_Match_ExecutesErrFuncWhenResultErr(() => Error.Empty);
    }

    [Test]
    public void Result_Match_ReturnsOkFuncResultWhenResultOk()
    {
        _resultTestFactory.Result_Match_ReturnsOkFuncResultWhenResultOk(() => Unit.Default);
    }

    [Test]
    public void Result_Match_ReturnsErrFuncResultWhenResultErr()
    {
        _resultTestFactory.Result_Match_ReturnsErrFuncResultWhenResultErr(() => Error.Empty);
    }
    
    #endregion

    #region AndThen method

    [Test]
    public void Result_AndThen_ReturnOkFuncResultWhenResultOk()
    {
        _resultTestFactory.Result_AndThen_ReturnOkFuncResultWhenResultOk(() => Unit.Default);
    }

    [Test]
    [TestCase(5)]
    [TestCase(10)]
    [TestCase(10_000)]
    public void Result_AndThen_CanChainCalls(int nCalls)
    {
        _resultTestFactory.Result_AndThen_CanChainCalls(nCalls);
    }

    [Test]
    public void Result_AndThen_ShortCircuitsWhenResultErr()
    {
        _resultTestFactory.Result_AndThen_ShortCircuitsWhenResultErr(() => Error.Empty);
    }

    #endregion
    
    #region Unwrap & Except
    
    [Test]
    public void Result_Unwrap_ReturnsValueWhenResultOk()
    {
        _resultTestFactory.Result_Unwrap_ReturnsValueWhenResultOk(() => Unit.Default);
    }

    [Test]
    public void Result_Unwrap_ThrowsExceptionWhenResultErr()
    {
        _resultTestFactory.Result_Unwrap_ThrowsExceptionWhenResultErr(() => Error.Empty);
    }

    [Test]
    public void Result_Expect_ReturnsValueWhenResultOk()
    {
        _resultTestFactory.Result_Expect_ReturnsValueWhenResultOk(() => Unit.Default);
    }

    [Test]
    public void Result_Expect_ThrowsExceptionWhenResultErr()
    {
        _resultTestFactory.Result_Expect_ThrowsExceptionWhenResultErr(() => Error.Empty);
    }
    
    #endregion
}