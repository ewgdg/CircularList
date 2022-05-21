using System;
using System.Collections.Generic;
using NonSystem.Collections.Generic;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace CircularList.Benchmark
{
  [MemoryDiagnoser]
  [IterationCount(5)]
  public class RemoveBenchmarks
  {
    private int count = 50000;
    private CircularList<int> circularList;
    private List<int> list;

    [IterationSetup]
    public void Setup()
    {
      circularList = new CircularList<int>(16);
      list = new List<int>(16);

      for (int i = 0; i < count; i++)
      {
        circularList.Add(i);
        list.Add(i);
      }

      //shuffle the list
      for (int i = 0; i < count / 2; i++)
      {
        circularList.RemoveAt(i);
        list.RemoveAt(i);
      }

      for (int i = 0; i < count / 2; i++)
      {
        circularList.Add(i);
        list.Add(i);
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
      Random rnd = new Random(12345);
      for (int i = 0; i < count; i++)
      {
        var index = rnd.Next(listArg.Count);
        listArg.RemoveAt(index);
      }
    }

  }
}



