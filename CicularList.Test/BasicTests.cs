using NonSystem.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CicularList.Test
{
  [TestClass]
  public class BasicTests
  {
    [TestMethod]
    public void SimpleTest()
    {
      var list = new CircularList<int>();
      list.Add(3);
      Assert.AreEqual(3, list[0]);
      list.Add(4);
      Assert.AreEqual(4, list[1]);
      Assert.AreEqual(0, list.IndexOf(3));
      list.Remove(3);
      Assert.AreEqual(1, list.Count);
      Assert.AreEqual(4, list[0]);
    }

    [TestMethod]
    public void RemoveAtTest()
    {
      var list = new CircularList<int>(2);
      for (int i = 0; i < 8; i++)
      {
        list.Add(i);
      }
      list.RemoveAt(5);
      Assert.IsTrue(list.SequenceEqual(new int[] { 0, 1, 2, 3, 4, 6, 7 }));
      list.RemoveAt(1);
      Assert.IsTrue(list.SequenceEqual(new int[] { 0, 2, 3, 4, 6, 7 }));
      list.RemoveAt(3);
      Assert.IsTrue(list.SequenceEqual(new int[] { 0, 2, 3, 6, 7 }));
      list.RemoveAt(2);
      Assert.IsTrue(list.SequenceEqual(new int[] { 0, 2, 6, 7 }));
      list.RemoveAt(2);
      Assert.IsTrue(list.SequenceEqual(new int[] { 0, 2, 7 }));
      list.RemoveAt(2);
      Assert.IsTrue(list.SequenceEqual(new int[] { 0, 2 }));
      list.RemoveAt(0);
      Assert.IsTrue(list.SequenceEqual(new int[] { 2 }));
      list.RemoveAt(0);
      var value = 0;
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => value = list[0]);
    }

    [TestMethod]
    public void RemoveAtTest2()
    {
      var list = new CircularList<int>(2);
      for (int i = 0; i < 8; i++)
      {
        list.Add(i);
      }
      Assert.IsTrue(list.SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }));
      for (int i = 0; i < 6; i++)
      {
        list.RemoveAt(0);
      }
      Assert.IsTrue(list.SequenceEqual(new int[] { 6, 7 }));
      list.Add(8);
      list.Add(9);
      Assert.IsTrue(list.SequenceEqual(new int[] { 6, 7, 8, 9 }));
      list.RemoveAt(2);
      Assert.IsTrue(list.SequenceEqual(new int[] { 6, 7, 9 }));
      list.RemoveAt(1);
      Assert.IsTrue(list.SequenceEqual(new int[] { 6, 9 }));
      list.RemoveAt(1);
      Assert.IsTrue(list.SequenceEqual(new int[] { 6 }));
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => list.RemoveAt(1));
      list.RemoveAt(0);
      var value = 0;
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => value = list[0]);
    }

    [TestMethod]
    public void RemoveAtTest3()
    {
      var list = new CircularList<int>(2);
      for (int i = 0; i < 8; i++)
      {
        list.Add(i);
      }
      for (int i = 0; i < 6; i++)
      {
        list.RemoveAt(0);
      }
      list.Add(8);
      list.Add(9);
      list.Add(10);
      list.Add(11);
      Assert.IsTrue(list.SequenceEqual(new int[] { 6, 7, 8, 9, 10, 11 }));
      list.RemoveAt(1);
      Assert.IsTrue(list.SequenceEqual(new int[] { 6, 8, 9, 10, 11 }));
      list.RemoveAt(1);
      Assert.IsTrue(list.SequenceEqual(new int[] { 6, 9, 10, 11 }));
      list.RemoveAt(0);
      Assert.IsTrue(list.SequenceEqual(new int[] { 9, 10, 11 }));
      list.RemoveAt(0);
      Assert.IsTrue(list.SequenceEqual(new int[] { 10, 11 }));
      list.RemoveAt(0);
      Assert.IsTrue(list.SequenceEqual(new int[] { 11 }));
    }

    [TestMethod]
    public void RemoveAtTest4()
    {
      var list = new CircularList<int>(2);
      for (int i = 0; i < 8; i++)
      {
        list.Add(i);
      }
      for (int i = 0; i < 7; i++)
      {
        list.RemoveAt(0);
      }
      list.Add(8);
      list.Add(9);
      list.Add(10);
      list.Add(11);
      list.Add(12);
      Assert.IsTrue(list.SequenceEqual(new int[] { 7, 8, 9, 10, 11, 12 }));
      list.RemoveAt(1);
      Assert.IsTrue(list.SequenceEqual(new int[] { 7, 9, 10, 11, 12 }));
      list.RemoveAt(1);
      Assert.IsTrue(list.SequenceEqual(new int[] { 7, 10, 11, 12 }));
      list.RemoveAt(1);
      Assert.IsTrue(list.SequenceEqual(new int[] { 7, 11, 12 }));
      list.RemoveAt(0);
      Assert.IsTrue(list.SequenceEqual(new int[] { 11, 12 }));
      list.RemoveAt(1);
      Assert.IsTrue(list.SequenceEqual(new int[] { 11 }));
    }
    [TestMethod]
    public void RandomInsertTest()
    {
      var count = 231;
      SetupRandomTest(out var refList, out var list, count);

      Random random = new Random(3214);
      for (int i = 0; i < count; ++i)
      {
        if (i == 33)
        {

        }
        var index = random.Next(refList.Count);
        var value = random.Next();

        list.Insert(index, value);
        refList.Insert(index, value);

        try
        {
          Assert.IsTrue(refList.SequenceEqual(list));
        }
        catch (Exception)
        {
          Console.WriteLine($"Case: i={i},index={index},value={value}");
          PrintResult(refList, list);
          throw;
        }
      }
    }

    [TestMethod]
    public void RandomRemoveTest()
    {
      var count = 10248;
      SetupRandomTest(out var refList, out var list, count);

      Random random = new Random(3456345);
      for (int i = 0; i < count; ++i)
      {
        var index = random.Next(refList.Count);
        var value = random.Next();

        list.RemoveAt(index);
        refList.RemoveAt(index);

        try
        {
          Assert.IsTrue(refList.SequenceEqual(list));
        }
        catch (Exception)
        {
          Console.WriteLine($"Case: i={i},index={index},value={value}");
          PrintResult(refList, list);
          throw;
        }
      }
    }

    [TestMethod]
    public void RandomInsertRemoveTest()
    {
      var count = 10248;
      SetupRandomTest(out var refList, out var list, count);
      Random random = new Random(123456);
      for (int i = 0; i < count; ++i)
      {
        var action = (ListAction)random.Next(3);
        var index = random.Next(refList.Count);
        var value = random.Next();
        if (action == ListAction.REMOVE_AT)
        {
          list.RemoveAt(index);
          refList.RemoveAt(index);
        }
        else if (action == ListAction.REPLACE)
        {
          list[index] = value;
          refList[index] = value;
        }
        else//insert
        {
          list.Insert(index, value);
          refList.Insert(index, value);
        }
        try
        {
          Assert.IsTrue(refList.SequenceEqual(list));
        }
        catch (Exception)
        {
          Console.WriteLine($"Case: i={i},action={action},index={index},value={value}");
          PrintResult(refList, list);
          throw;
        }
      }
    }

    private void SetupRandomTest(out List<int> refList, out CircularList<int> list, int count)
    {
      refList = new List<int>(33);
      list = new CircularList<int>(33);
      Random random = new Random(123456);

      for (int i = 0; i < count; ++i)
      {
        var value = random.Next();
        list.Add(value);
        refList.Add(value);
      }

      for (int i = 0; i < count / 2; ++i)
      {
        list.RemoveAt(0);
        refList.RemoveAt(0);
      }

      for (int i = 0; i < count / 2; ++i)
      {
        var value = random.Next();
        list.Add(value);
        refList.Add(value);
      }

      try
      {
        Assert.IsTrue(refList.SequenceEqual(list));
      }
      catch (Exception)
      {
        PrintResult(refList, list);
        throw;
      }
    }

    private void PrintResult(IList<int> refList, IList<int> list)
    {
      Console.WriteLine($"Expected Count:{refList.Count}, Actual Count:{list.Count} ");
      for (int i = 0; i < Math.Min(list.Count, refList.Count); i++)
      {
        if (list[i] != refList[i])
        {
          Console.WriteLine($"At index {i}, Expected Value:{refList[i]}, Actual Value:{list[i]}");
        }
      }
    }

    internal enum ListAction
    {
      REMOVE_AT,
      REPLACE,
      INSERT
    }
  }
}
