using BenchmarkDotNet.Running;

namespace CircularList.Benchmark
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BenchmarkRunner.Run<RemoveBenchmarks>();
      BenchmarkRunner.Run<RemoveHeadBenchmarks>();
      BenchmarkRunner.Run<InsertBenchmarks>();
      BenchmarkRunner.Run<InsertHeadBenchmarks>();
      BenchmarkRunner.Run<RandomBenchmarks>();
    }
  }
}
