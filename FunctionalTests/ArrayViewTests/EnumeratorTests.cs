using Functional.View;
using Microsoft.VisualBasic;

namespace FunctionalTests.ArrayViewTests;

[TestFixture]
public class EnumeratorTests
{
    private int[] _backingArray;
    private ArrayView<int> _fullArrayView;
    private ArrayView<int> _partialArrayView;

    [SetUp]
    public void Setup()
    {
        _backingArray = DataGenerator.GenerateDeterministicIntDataSet(1000);
        _fullArrayView = new ArrayView<int>(_backingArray);
        _partialArrayView = _fullArrayView.Slice(2..4);
    }
    
    [Test]
    public void ArrayView_Enumerator_LoopsCorrectlyThroughView()
    {
        // Arrange
        int index = 0;
        
        // Act & Assert
        foreach (int element in _fullArrayView)
        {
            Assert.That(element, Is.EqualTo(_backingArray[index]));
            index++;
        }
    }
    
    [Test]
    public void ArrayView_Enumerator_LoopsCorrectlyThroughPartialView()
    {
        // Arrange
        int index = 2;
        
        // Act & Assert
        foreach (int element in _partialArrayView)
        {
            Assert.That(element, Is.EqualTo(_backingArray[index]));
            Assert.That(index, Is.LessThanOrEqualTo(3));
            index++;
        }
    }
    
    [Test]
    public void ArrayView_Enumerator_LoopsCorrectlyThroughEmptySlice()
    {
        // Arrange
        ArrayView<int> emptyView = _fullArrayView.Slice(0..0);
        
        // Act & Assert
        Assert.That(emptyView.Count, Is.EqualTo(0));
        foreach (int element in emptyView)
        {
            Assert.Fail();
        }
    }
    
    [Test]
    public void ArrayView_Enumerator_LoopsCorrectlyThroughSingleElementSlice()
    {
        // Arrange
        ArrayView<int> singleElementView = _fullArrayView.Slice(0..1);
        
        // Act & Assert
        Assert.That(singleElementView.Count, Is.EqualTo(1));
        foreach (int element in singleElementView)
        {
            Assert.That(element, Is.EqualTo(_backingArray[0]));
        }
    }

    [Test]
    public void ArrayView_Enumerator_Reset_ResetsEnumerator()
    {
        // Arrange
        ArrayView<int>.Enumerator enumerator = _fullArrayView.GetEnumerator();
        
        int[] firstIteration = new int[_backingArray.Length];
        int[] secondIteration = new int[_backingArray.Length];
        int index = 0;
        
        // Act
        while(enumerator.MoveNext())
        {
            firstIteration[index++] = enumerator.Current;
        }
        
        enumerator.Reset();
        index = 0;
        
        while(enumerator.MoveNext())
        {
            secondIteration[index++] = enumerator.Current;
        }
        
        // Assert
        CollectionAssert.AreEqual(firstIteration, secondIteration);
    }
}