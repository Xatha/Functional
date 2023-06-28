using System.Collections;

namespace Functional.View;

/// <summary>
/// Provides an immutable view into an array. This view can be sliced to provide a view into a subset of the array.
/// While this view is immutable, the underlying array is not. This means that changes to the underlying array will be reflected,
/// but the view itself cannot be changed. Does also not guarantee immutability of the items in the array.
/// </summary> 
/// <typeparam name="T">Type of the view.</typeparam>
public readonly struct ArrayView<T> : IReadOnlyList<T>
{
    private readonly T[] _items;
    private readonly int _indexOffset;
    private readonly int _rangeLength;
    private readonly int _end;
    
    /// <summary>
    /// Gets the number of elements that can be accessed from this view.
    /// </summary>
    public int Count { get; }
    
    /// <summary>
    /// Gets a given element from the view. Notice that if you are using a slice of the view,
    /// the index will be relative to the slice, and you should not manually calculate offsets. 
    /// </summary>
    /// <param name="index">Index</param>
    /// <exception cref="IndexOutOfRangeException">Thrown if the index is out of bounds.</exception>
    /// <remarks> Elements are references, and any mutations to them will mutate them in the backing array too. </remarks>
    public T this[int index]
    {
        get
        {
            if ((uint) index >= (uint) _end) throw new IndexOutOfRangeException();
            return _items[index + _indexOffset];
        }
    }
    
    public ArrayView<T> this[Range range] => Slice(range);

    public ArrayView(T[] array)
    {
        _items = array;
        _indexOffset = 0;
        _rangeLength = array.Length;
        _end = array.Length;
        Count = array.Length;
    }

    public ArrayView(T[] array, Range range)
    {
        _items = array;
        (_indexOffset, _rangeLength) = range.GetOffsetAndLength(_items.Length);
        _end = _rangeLength;
        Count = _rangeLength + _indexOffset;
    }

    public ArrayView<T> Slice(Range range)
    {
        return new ArrayView<T>(_items, range);
    }

    public ReadOnlySpan<T> AsReadOnlySpan()
    {
        return _items.AsSpan(_indexOffset, _rangeLength);
    }

    #region Enumerator
    
    public ArrayView<T>.Enumerator GetEnumerator()
    {
        return new ArrayView<T>.Enumerator(this);
    }
    
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
    
    public struct Enumerator : IEnumerator<T>
    {
        private readonly T[] _items;
        private readonly int _start;
        private readonly int _end;
        private int _index;

        public T Current => _items[_index];
        object IEnumerator.Current => Current!;

        internal Enumerator(ArrayView<T> arrayView)
        {
            // Benchmarks have shown that copying the values here is faster than using the reference directly.
            // This results in a 5x speedup. I am unsure why.
            _items = arrayView._items;
            _index = arrayView._indexOffset - 1;
            _start = arrayView._indexOffset;
            _end   = arrayView.Count;
        }

        public bool MoveNext()
        {
            _index++;
            return _index < _end;
        }
        
        public void Reset()
        {
            _index = _start - 1;
        }

        
        public void Dispose() { }
    }
}

