using System;
using System.Collections.Generic;
using NonSystem.Collections.Generic;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace CircularList.Benchmark
{
  [MemoryDiagnoser]
  [IterationCount(5)]
  public class InsertHeadBenchmarks
  {
    private int count = 50000;
    private CircularList<int> circularList;
    private List<int> list;

    [IterationSetup]
    public void Setup()
    {
      circularList = new CircularList<int>(16);
      list = new List<int>(16);

      var lists = new IList<int>[] { circularList, list };

      foreach (var l in lists)
      {
        for (int i = 0; i < count; i++)
        {
          l.Add(i);
        }

        //shuffle the list
        for (int i = 0; i < count / 2; i++)
        {
          l.RemoveAt(0);
        }

        for (int i = 0; i < count / 2; i++)
        {
          l.Add(i);
        }
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
      for (int i = 0; i < count; i++)
      {
        listArg.Insert(0, i);
      }
    }

  }
}



