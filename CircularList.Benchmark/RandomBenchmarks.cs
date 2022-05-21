using System;
using System.Collections.Generic;
using NonSystem.Collections.Generic;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace CircularList.Benchmark
{
  [MemoryDiagnoser]
  [IterationCount(5)]
  public class RandomBenchmarks
  {
    private CircularList<int> circularList;
    private List<int> list;
    private int count = 50000;

    [IterationSetup]
    public void Setup()
    {
      circularList = new CircularList<int>(16);
      list = new List<int>(16);
      SetupList(list);
      SetupList(circularList);
    }

    private void SetupList(IList<int> listArg)
    {
      var random = new Random(78927);
      for (int i = 0; i < count; ++i)
      {
        var value = random.Next();
        listArg.Add(value);
      }

      for (int i = 0; i < count / 2; ++i)
      {
        listArg.RemoveAt(0);
      }

      for (int i = 0; i < count / 2; ++i)
      {
        var value = random.Next();
        listArg.Add(value);
      }
    }

    [Benchmark]
    public void WithList()
    {
      RunTest(list);
    }

    [Benchmark]
    public void WithCircularList()
    {
      RunTest(circularList);
    }

    private void RunTest(IList<int> listArg)
    {
      var random = new Random(8493);
      for (int i = 0; i < count; ++i)
      {
        var action = (ListAction)random.Next(3);
        var index = random.Next(listArg.Count);
        var value = random.Next();
        if (action == ListAction.REMOVE_AT)
        {
          listArg.RemoveAt(index);
        }
        else if (action == ListAction.REPLACE)
        {
          listArg[index] = value;
        }
        else//insert
        {
          listArg.Insert(index, value);
        }
      }

    }
  }
}
