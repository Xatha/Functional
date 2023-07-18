using System.Diagnostics.CodeAnalysis;
using Functional.SumTypes;
using static Functional.SumTypes.OptionVariants;

namespace FunctionalTests.OptionTests;

[TestFixture]
[SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
public class OptionTests
{
    
    // todo: add tests for OptionTaskWrapper
    /*
    [Test]
    public async Task MapAsyncTest()
    {
        // Arrange
        Option<int> number = Option.Some(5);
        Task<int> AddAsync(int x, int y) => Task.FromResult(x + y);
        Task<Option<float>> AddFloatAsync(float x, float y) => Task.FromResult(Option.Some<float>(x + y));
        double Multiply(float x, double y) => x * y;

        OptionTaskWrapper<int> DivideAsync(int x, int y)
        {
            return OptionTaskWrapper.Some(x);
        }

        // Act
        var optionTask1 = number
            .Map(async x => await AddAsync(x, 1))
            .Map(async x => await AddFloatAsync(await x, 4))
            .Map(async x => await AddAsync((int)await x, 1));
        var optionTask2 = optionTask1
            .Map(async x => Multiply(await x, 2.0));
        
        var resultOption1 = await optionTask1.Resolve();
        var resultOption2 = await optionTask2.Resolve();
        
        double result = resultOption1 switch { (Some, var v) => v, _ => -1 };
        double result2 = resultOption2 switch { (Some, var v) => v, _ => -1 };
        
        // Assert
        Assert.That(result, Is.EqualTo(9.6).Within(0.0001));
        Assert.That(result2, Is.EqualTo(19.2).Within(0.0001));
    }
    */
    
    [Test]
    public void Map_WhenValueIsNotNull_ReturnsCorrectValue()
    {
        // Arrange
        Option<string> option;
        int result;

        // Act
        option = Option.Some("Hello World!");
        result = option.Map(x => x.Length).Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo("Hello World!".Length));
    }
    
    [Test]
    public void Map_WhenValueIsNull_ReturnsDefaultValue()
    {
        // Arrange
        Option<string> option;
        int result;

        // Act
        option = Option.None<string>();
        result = option.Map(x => x.Length).Collapse(-1);
        
        
        // Assert
        Assert.That(result, Is.EqualTo(-1));
    }
    
    [Test]
    public void Map_WithOption_WhenValueIsNotNull_ReturnsCorrectValue()
    {
        // Arrange
        Option<string> option;
        Option<int> length;
        int result;
        
        Option<int> Add(int x, Option<int> y) => y.Map(z => x + z);
        
        // Act
        option = Option.Some("Hello World!");
        length = Option.Some(5);
        result = option.Map(x => Add(x.Length, length)).Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo("Hello World!".Length + 5));
    }
    
    [Test]
    public void Map_WithOption_WhenValueIsNull_ReturnsDefaultValue()
    {
        // Arrange
        Option<string> option;
        Option<int> length;
        int result;
        
        Option<int> Add(int x, Option<int> y) => y.Map(z => x + z);
        
        // Act
        option = Option.None<string>();
        length = Option.Some(5);
        result = option.Map(x => Add(x.Length, length)).Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo(-1));
    }

    [Test]
    public void BindMap_WithOption_WhenValueIsNull_ReturnsCorrectValue()
    {
        // Arrange
        Option<string> option;
        int length;
        int result;
        
        // Act
        option = Option.Some("Hello World!");
        length = 5;
        
        result = option.MapBind((x, y) => x.Length + y, length).Collapse(-1);
        
        // Assert
        Assert.That(result, Is.EqualTo("Hello World!".Length + 5));
    }
}