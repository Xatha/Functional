using System.Diagnostics.CodeAnalysis;
using Functional.Option;

namespace FunctionalTests.OptionTests;

[TestFixture]
[SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
public class OptionTests
{
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
        option = Option<string>.None();
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
        option = Option<string>.None();
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