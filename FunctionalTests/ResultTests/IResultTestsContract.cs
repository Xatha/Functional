namespace FunctionalTests.ResultTests;

public interface IResultTestsContract<T>
{
    #region IsOk & IsErr properties

    public void Result_IsOkProperty_OkWhenResultOk();

    public void Result_IsErrProperty_ErrWhenResultErr();

    #endregion

    #region Implicit conversion
    
    public void Result_ImplicitConversionFromOkToResult_IsOkResult();
    public void Result_ImplicitConversionFromNullToResult_IsErrResult();
    public void Result_ImplicitConversionFromErrToResult_IsErrResult();

    #endregion
    
    /* TODO: These methods are currently not implemented in Result.cs.
    #region Factory methods
    
    public void Result_OkFactoryMethod_IsOkResult();
    public void Result_ErrÅErrorÅFactoryMethod_IsErrResult();
    public void Result_ErrÅStringÅFactoryMethod_IsErrResult();
    public void Result_ErrÅExceptionÅFactoryMethod_IsErrResult();
    
    #endregion
    */
    
    #region Match method
    
    public void Result_Match_ExecutesOkFuncWhenResultOk();
    public void Result_Match_ExecutesErrFuncWhenResultErr();
    
    public void Result_Match_ReturnsOkFuncResultWhenResultOk();
    public void Result_Match_ReturnsErrFuncResultWhenResultErr();
    
    #endregion

    #region AndThen method

    public void Result_AndThen_ReturnOkFuncResultWhenResultOk();
    public void Result_AndThen_CanChainCalls(int nCalls);
    public void Result_AndThen_ShortCircuitsWhenResultErr();

    #endregion

    #region Unwrap & Except
    
    public void Result_Unwrap_ReturnsValueWhenResultOk();
    public void Result_Unwrap_ThrowsExceptionWhenResultErr();
    
    public void Result_Expect_ReturnsValueWhenResultOk();
    public void Result_Expect_ThrowsExceptionWhenResultErr();

    #endregion
}