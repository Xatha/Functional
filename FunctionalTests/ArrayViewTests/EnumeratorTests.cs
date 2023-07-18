using Functional.Datastructures;
using Microsoft.VisualBasic;

namespace FunctionalTests.ArrayViewTests;

[TestFixture]
public class EnumeratorTests
{
    private int[] _backingArray;
    private ImmutableArrayView<int> _fullArrayView;
    private ImmutableArrayView<int> _partialArrayView;

    [SetUp]
    public void Setup()
    {
        _backingArray = DataGenerator.GenerateDeterministicIntDataSet(1000);
        _fullArrayView = new ImmutableArrayView<int>(_backingArray);
        _partialArrayView = _fullArrayView.Slice(5..7);
    }
    
    [Test]
    public void ArrayView_Enumerator_LoopsFullView()
    {
        // Arrange
        int index = 0;
        
        // Act & Assert
        foreach (int element in _fullArrayView)
        {
            Assert.That(element, Is.EqualTo(_backingArray[index]));
            index++;
        }
        
        Assert.That(index, Is.EqualTo(_backingArray.Length));
    }
    
    [Test]
    public void ArrayView_Enumerator_LoopsPartialView(){
        // Arrange
        int index = 5;
        
        // Act & Assert
        foreach (int element in _partialArrayView)
        {
            Assert.That(element, Is.EqualTo(_backingArray[index]));
            Assert.That(index, Is.LessThanOrEqualTo(6));
            index++;
        }
        
        Assert.That(index, Is.EqualTo(7));
    }
    
    [Test]
    public void ArrayView_Enumerator_LoopsCorrectlyThroughEmptySlice()
    {
        // Arrange
        ImmutableArrayView<int> emptyView = _fullArrayView.Slice(0..0);
        
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
        ImmutableArrayView<int> singleElementView = _fullArrayView.Slice(0..1);
        bool hasLooped = false;
        
        // Act & Assert
        Assert.That(singleElementView.Count, Is.EqualTo(1));
        foreach (int element in singleElementView)
        {
            Assert.That(element, Is.EqualTo(_backingArray[0]));
            hasLooped = true;
        }
        Assert.That(hasLooped, Is.True);
    }

    [Test]
    public void ArrayView_Enumerator_Reset_ResetsEnumerator()
    {
        // Arrange
        ImmutableArrayView<int>.Enumerator enumerator = _fullArrayView.GetEnumerator();
        
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