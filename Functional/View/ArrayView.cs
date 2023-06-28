using System.Collections;

namespace Functional.View;

public readonly struct ArrayView<T> : IReadOnlyList<T>
{
    private readonly T[] _items;
    private readonly int _indexOffset;
    private readonly int _rangeLength;
    private readonly int _end;

    public int Count { get; }

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

    //public Span<T>.Enumerator GetEnumerator()

    //public Span<T>.Enumerator GetEnumerator()
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArrayView<T>.Enumerator GetEnumerator()
    {
        return new ArrayView<T>.Enumerator(this);
        //return new ArrayViewEnumerator<T>(this);
    }
    
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    #endregion
    
    public struct Enumerator : IEnumerator<T>
    {
        private readonly T[] _items;
        private readonly int _start;
        private readonly int _end;
        private int _index;

        public T Current => _items[_index];
        object IEnumerator.Current => Current;

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

