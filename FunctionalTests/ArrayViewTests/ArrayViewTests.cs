using System.Diagnostics.CodeAnalysis;
using Functional.View;

namespace FunctionalTests.ArrayViewTests;

[TestFixture]
[SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
public class ArrayViewTests
{
    [Test]
    public void ArrayView_GetsElementsFromArray()
    {
        // Arrange
        int[] backingArray = {4, 3, 1, 5, 64, 2, -4};
        ArrayView<int> arrayView = new(backingArray);
        
        // Act & Assert
        for (int i = 0; i < backingArray.Length; i++)
        {
            Assert.That(arrayView[i], Is.EqualTo(backingArray[i]));
        }
    }

    [Test]
    public void ArrayView_WillCorrectlySlice()
    {
        // Arrange
        int[] backingArray = {4, 3, 1, 5, 64, 2, -4};
        ArrayView<int> arrayView = new(backingArray);

        var slicedView = arrayView.Slice(2..4);

        // Act & Assert
        Assert.That(slicedView[0], Is.EqualTo(1));
        Assert.That(slicedView[1], Is.EqualTo(5));
        Assert.That(() => slicedView[2], Throws.TypeOf<IndexOutOfRangeException>());
        Assert.That(() => slicedView[3], Throws.TypeOf<IndexOutOfRangeException>());
        Assert.That(() => slicedView[-1], Throws.TypeOf<IndexOutOfRangeException>());
    }

    [Test]
    public void ArrayView_Count_IsTheSameAsRange()
    {
        // Arrange
        int[] backingArray = {4, 3, 1, 5, 64, 2, -4};
        ArrayView<int> arrayView = new(backingArray);
        ArrayView<int> slicedView = arrayView.Slice(2..4);
        
        int slicedViewCount = slicedView.Count;
        int viewCount       = arrayView.Count;
        
        // Assert 
        Assert.That(viewCount, Is.EqualTo(backingArray.Length));
        Assert.That(slicedViewCount, Is.EqualTo(4));
    }

    [Test]
    public void ArrayView_ForEach_LoopsCorrectlyThroughView()
    {
        // Arrange
        int[] backingArray = {4, 3, 1, 5, 64, 2, -4};
        ArrayView<int> arrayView = new(backingArray);
        int backingArrayIndex = 0;
        
        // Act & Assert
        foreach (var element in arrayView)
        {
            Assert.That(element, Is.EqualTo(backingArray[backingArrayIndex++]));
        }
    }
    
    [Test]
    public void ArrayView_ForEach_LoopsCorrectlyThroughSlicedView()
    {
        // Arrange
        int[] backingArray = {4, 3, 1, 5, 64, 2, -4};
        ArrayView<int> arrayView = new(backingArray);
        ArrayView<int> slicedView = arrayView.Slice(2..4);
        
        int backingArrayIndex = 2;
        
        // Act & Assert
        foreach (var element in slicedView)
        {
            Assert.That(element, Is.EqualTo(backingArray[backingArrayIndex++]));
            Assert.That(backingArrayIndex, Is.LessThanOrEqualTo(4));
        }
    }
}