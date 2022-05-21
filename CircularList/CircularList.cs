using System.Collections;
using System.Runtime.CompilerServices;

namespace NonSystem.Collections.Generic
{
  /// <summary>
  /// A double-ended queue implements IList interface.
  /// </summary>
  /// <typeparam name="T">Type</typeparam>
  /// <remarks>
  /// O(1) Insert/RemoveAt for index 0 and n-1
  /// </remarks>
  public class CircularList<T> : IList<T>
  {
    protected T[] _items;
    private int _initialCapacity;
    protected int _headIndex;
    protected int _tailIndex;
    private int LargestIndex
    {
      get
      {
        if (IsWrapped(_tailIndex))
        {
          return _items.Length - 1;
        }
        else
        {
          return _tailIndex;
        }
      }
    }
    public CircularList() : this(16)
    {

    }

    public CircularList(int initialCapacity)
    {
      if (_initialCapacity > int.MaxValue)
      {
        throw new ArgumentOutOfRangeException(nameof(initialCapacity));
      }
      _initialCapacity = initialCapacity;
      _items = new T[_initialCapacity];
      Clear();
    }

    public T this[int index]
    {
      get
      {
        EnsureValidIndex(index);
        return _items[GetInternalIndex(index)];
      }
      set
      {
        EnsureValidIndex(index);
        _items[GetInternalIndex(index)] = value;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected bool IsWrapped(int internalIndex)
      => internalIndex < _headIndex;


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void EnsureValidIndex(int index)
    {
      if (index < 0 || index >= Count)
      {
        throw new ArgumentOutOfRangeException("index");
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void CorrectInternalIndex(ref int internalIndex)
    {
      if (internalIndex >= _items.Length)
      {
        internalIndex %= _items.Length;
      }
    }

    /// <summary>
    /// Get the index for the internal array
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected int GetInternalIndex(int index)
    {
      var internalIndex = index + _headIndex;
      CorrectInternalIndex(ref internalIndex);
      return internalIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected int IncreaseInternalIndex(int internalIndex)
    {
      internalIndex += 1;
      CorrectInternalIndex(ref internalIndex);
      return internalIndex;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected int DecreaseInternalIndex(int internalIndex)
    {
      internalIndex -= 1;
      CorrectInternalIndex(ref internalIndex);
      return internalIndex;
    }


    protected int _count = 0;
    public int Count => _count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
      if (_count >= _items.Length)
      {
        ExpandCapacity();
      }
      _tailIndex += 1;
      if (_tailIndex >= _items.Length)
      {
        _tailIndex %= _items.Length;
      }

      _items[_tailIndex] = item;
      ++_count;
    }

    private void ExpandCapacity()
    {
      int capacity = _items.Length * 2;
      if ((uint)capacity > int.MaxValue)
      {
        capacity = int.MaxValue;
      }
      if (capacity == _items.Length)
      {
        throw new Exception("Reach Size Limit");
      }
      var res = new T[capacity];
      CopyTo(res, 0);
      _items = res;
    }

    public void Clear()
    {
      _count = 0;
      _headIndex = 0;
      _tailIndex = -1;
    }

    public bool Contains(T? item)
    {
      foreach (var internalItem in this)
      {
        if (EqualityComparer<T>.Default.Equals(internalItem, item))
        {
          return true;
        }
      }
      return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      if (Count == 0)
      {
        return;
      }
      int copyCount = LargestIndex - _headIndex + 1;
      Array.Copy(_items, _headIndex, array, arrayIndex, copyCount);

      if (IsWrapped(_tailIndex))
      {
        Array.Copy(_items, 0, array, arrayIndex + copyCount, _tailIndex + 1);
      }

      _headIndex = arrayIndex;
      _tailIndex = arrayIndex + Count - 1;
    }

    public IEnumerator<T> GetEnumerator()
    {
      for (int i = 0, internalIndex = GetInternalIndex(i); i < Count; i++, internalIndex = IncreaseInternalIndex(internalIndex))
      {
        yield return _items[internalIndex];
      }
    }

    public int IndexOf(T item)
    {
      var i = 0;
      foreach (var internalItem in this)
      {
        if (EqualityComparer<T>.Default.Equals(internalItem, item))
        {
          return i;
        }
        i += 1;
      }
      return -1;
    }

    public void Insert(int index, T item)
    {
      EnsureValidIndex(index);

      if (_count >= _items.Length)
      {
        ExpandCapacity();
      }

      var internalIndex = GetInternalIndex(index);

      //need to always shift to the right to maintain the order
      if (IsWrapped(internalIndex))
      {
        var count = _tailIndex - internalIndex + 1;
        if (count > 0)
        {
          Array.Copy(_items, internalIndex, _items, internalIndex + 1, _tailIndex - internalIndex + 1);
        }
      }
      else
      {
        if (IsWrapped(_tailIndex))
        {
          Array.Copy(_items, 0, _items, 1, _tailIndex + 1);
        }

        var largestIndex = LargestIndex;
        var count = largestIndex - internalIndex;
        if (largestIndex == _items.Length - 1)
        {
          _items[0] = _items[_items.Length - 1];
        }
        else
        {
          count += 1;
        }
        if (count > 0)
        {
          Array.Copy(_items, internalIndex, _items, internalIndex + 1, count);
        }
      }

      _items[internalIndex] = item;
      _tailIndex = IncreaseInternalIndex(_tailIndex);
      ++_count;
    }


    public bool Remove(T item)
    {
      var index = IndexOf(item);
      if (index < 0)
      {
        return false;
      }
      RemoveAt(index);
      return true;
    }

    private void RemoveAtShiftTail(int index)
    {

      var internalIndex = GetInternalIndex(index);

      if (IsWrapped(internalIndex))
      {
        //Array.Copy can handle overlap
        Array.Copy(_items, internalIndex + 1, _items, internalIndex, _tailIndex - internalIndex);
      }
      else
      {
        int count = LargestIndex - internalIndex;
        Array.Copy(_items, internalIndex + 1, _items, internalIndex, count);
        if (IsWrapped(_tailIndex))
        {
          _items[_items.Length - 1] = _items[0];
          if (_tailIndex > 0)
          {
            Array.Copy(_items, 1, _items, 0, _tailIndex);
          }
        }
      }

      _count--;
      _tailIndex = DecreaseInternalIndex(_tailIndex);
    }

    private void RemoveAtShiftHead(int index)
    {

      var internalIndex = GetInternalIndex(index);

      //if (index == 0 && internalIndex != _headIndex)
      //{
      //  Console.WriteLine($"internalIndex = {internalIndex}, _headIndex = {_headIndex}");
      //}

      if (!IsWrapped(internalIndex))
      {
        var count = internalIndex - _headIndex;
        if (count > 0)
        {
          Array.Copy(_items, _headIndex, _items, _headIndex + 1, count);
        }
      }
      else
      {
        Array.Copy(_items, 0, _items, 1, internalIndex);
        _items[0] = _items[_items.Length - 1];
        var count = _items.Length - 1 - _headIndex;
        if (count > 0)
        {
          Array.Copy(_items, _headIndex, _items, _headIndex + 1, count);
        }
        //if (index == 0)
        //{
        //  Console.WriteLine("wrong branch");
        //}
      }

      _count--;
      _headIndex = IncreaseInternalIndex(_headIndex);
    }

    public virtual void RemoveAt(int index)
    {
      EnsureValidIndex(index);
      //RemoveAtShiftHead(index);
      //RemoveAtShiftTail(index);

      //if (index == 0)
      //{
      //  _count--;
      //  IncreaseInternalIndex(ref _headIndex);
      //  return;
      //}

      //we can shift from either end
      if (index == 0 || index < (Count - 1) / 2)
      {
        RemoveAtShiftHead(index);
      }
      else
      {
        //if (index == 0 && Count > 2)
        //{
        //  Console.WriteLine("wrong branch");
        //}
        RemoveAtShiftTail(index);
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
  }
}
