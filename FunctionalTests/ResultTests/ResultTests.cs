using Functional.Result;

namespace FunctionalTests.ResultTests;

public class ResultTests
{
    [Test]
    public void OkAndErrTest()
    {
        // Arrange
        Result successResult, failureResult1, failureResult2;
        
        // Act
        successResult = Result.Ok();
        failureResult1 = Result.Err("Result error1");
        failureResult2 = Result.Err(ResultError.Create("Result error2"));
        
        // Assert
        Assert.That(successResult, Is.EqualTo(Result.Ok()));
        Assert.That(failureResult1, Is.EqualTo(Result.Err("Result error1")));
        Assert.That(failureResult2, Is.EqualTo(Result.Err("Result error2")));
    }
    
    [Test]
    public void OkIsSuccessAndErrIsFailure()
    {
        // Arrange
        Result successResult, failureResult;

        // Act
        successResult = Result.Ok();
        failureResult = Result.Err();

        // Assert
        Assert.That(successResult.IsFailed(out _), Is.EqualTo(false));
        Assert.That(failureResult.IsFailed(out _), Is.EqualTo(true));
    }
}