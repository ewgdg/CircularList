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
    private readonly int _initialCapacity;
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
      if (index < 0 || index >= _count)
      {
        throw new ArgumentOutOfRangeException("index");
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected int CorrectInternalIndex(int internalIndex)
    {
      if (internalIndex >= _items.Length || internalIndex < 0)
      {
        internalIndex %= _items.Length;
      }
      if (internalIndex < 0)
      {
        internalIndex += _items.Length;
      }
      return internalIndex;
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
      return CorrectInternalIndex(internalIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected int IncreaseInternalIndex(int internalIndex)
    {
      internalIndex += 1;
      return CorrectInternalIndex(internalIndex);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected int DecreaseInternalIndex(int internalIndex)
    {
      internalIndex -= 1;
      return CorrectInternalIndex(internalIndex);
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
      _tailIndex = IncreaseInternalIndex(_tailIndex);

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
      if (_count == 0)
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
      _tailIndex = arrayIndex + _count - 1;
    }

    public IEnumerator<T> GetEnumerator()
    {
      for (int i = 0, internalIndex = GetInternalIndex(i); i < _count; i++, internalIndex = IncreaseInternalIndex(internalIndex))
      {
        yield return _items[internalIndex];
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

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

      if (index == 0 || index < (_count - 1) / 2)
      {
        InsertShiftHead(index, item);
      }
      else
      {
        InsertShiftTail(index, item);
      }
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

    public virtual void RemoveAt(int index)
    {
      EnsureValidIndex(index);

      //we can shift from either end
      if (index == 0 || index < (_count - 1) / 2)
      {
        RemoveAtShiftHead(index);
      }
      else
      {
        RemoveAtShiftTail(index);
      }
    }

    private void InsertShiftTail(int index, T item)
    {
      var internalIndex = GetInternalIndex(index);
      if (IsWrapped(internalIndex))
      {
        var copyCount = _tailIndex - internalIndex + 1;
        if (copyCount > 0)
        {
          Array.Copy(_items, internalIndex, _items, internalIndex + 1, copyCount);
        }
      }
      else
      {
        if (IsWrapped(_tailIndex))
        {
          Array.Copy(_items, 0, _items, 1, _tailIndex + 1);
        }

        var largestIndex = LargestIndex;
        _items[IncreaseInternalIndex(largestIndex)] = _items[largestIndex];
        var copyCount = largestIndex - internalIndex;
        if (copyCount > 0)
        {
          Array.Copy(_items, internalIndex, _items, internalIndex + 1, copyCount);
        }
      }

      _tailIndex = IncreaseInternalIndex(_tailIndex);
      _items[internalIndex] = item;
      ++_count;
    }

    private void InsertShiftHead(int index, T item)
    {
      var internalIndex = GetInternalIndex(index);
      var insertAtInternalIndex = DecreaseInternalIndex(internalIndex);
      var newHead = DecreaseInternalIndex(_headIndex);

      //if insert at index 0 then there is no need to shift
      if (index > 0)
      {
        var isInsertAtIndexWrapped = IsWrapped(insertAtInternalIndex);
        _items[newHead] = _items[_headIndex];
        var copyCount = (isInsertAtIndexWrapped ? _items.Length - 1 : insertAtInternalIndex) - _headIndex;
        if (copyCount > 0)
        {
          Array.Copy(_items, _headIndex + 1, _items, _headIndex, copyCount);
        }

        if (isInsertAtIndexWrapped)
        {
          _items[^1] = _items[0];
          var copyCount2 = insertAtInternalIndex;
          if (copyCount2 > 0)
          {
            Array.Copy(_items, 1, _items, 0, copyCount2);
          }
        }
      }

      _headIndex = newHead;
      _items[insertAtInternalIndex] = item;
      ++_count;
    }

    private void RemoveAtShiftTail(int index)
    {
      var internalIndex = GetInternalIndex(index);
      if (IsWrapped(internalIndex))
      {
        //Array.Copy can handle overlap
        var copyCount = _tailIndex - internalIndex;
        if (copyCount > 0)
        {
          Array.Copy(_items, internalIndex + 1, _items, internalIndex, copyCount);
        }
      }
      else
      {
        var copyCount = LargestIndex - internalIndex;
        Array.Copy(_items, internalIndex + 1, _items, internalIndex, copyCount);
        if (IsWrapped(_tailIndex))
        {
          _items[^1] = _items[0];
          var copyCount2 = _tailIndex;
          if (copyCount2 > 0)
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
      if (!IsWrapped(internalIndex))
      {
        var copyCount = internalIndex - _headIndex;
        if (copyCount > 0)
        {
          Array.Copy(_items, _headIndex, _items, _headIndex + 1, copyCount);
        }
      }
      else
      {
        var copyCount = internalIndex;
        if (copyCount > 0)
        {
          Array.Copy(_items, 0, _items, 1, copyCount);
        }
        _items[0] = _items[^1];
        var copyCount2 = _items.Length - 1 - _headIndex;
        if (copyCount2 > 0)
        {
          Array.Copy(_items, _headIndex, _items, _headIndex + 1, copyCount2);
        }
      }

      _count--;
      _headIndex = IncreaseInternalIndex(_headIndex);
    }
  }
}
