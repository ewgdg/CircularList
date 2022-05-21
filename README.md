# CircularList
A double-ended queue implements IList interface, featuring O(1) `RemoveAt(0)` compared to `List<>`.
This means both removing head and tail are O(1) operations.

## Usage
There is a single file project, just copy the CircularList.cs to the target project.

## Benchmark
### Insert
|           Method |     Mean |    Error |  StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |---------:|---------:|--------:|------:|------:|------:|----------:|
|         WithList | 168.2 ms | 11.25 ms | 2.92 ms |     - |     - |     - | 512.91 KB |
| WithCircularList | 175.6 ms | 25.83 ms | 6.71 ms |     - |     - |     - | 512.89 KB |
### Remove
|           Method |     Mean |    Error |   StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |---------:|---------:|---------:|------:|------:|------:|----------:|
|         WithList | 41.85 ms | 4.202 ms | 1.091 ms |     - |     - |     - |     784 B |
| WithCircularList | 40.84 ms | 2.433 ms | 0.377 ms |     - |     - |     - |     784 B |
### Remove Head
|           Method |        Mean |       Error |      StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |------------:|------------:|------------:|------:|------:|------:|----------:|
|         WithList | 78,514.6 μs | 34,478.1 μs | 8,953.85 μs |     - |     - |     - |     480 B |
| WithCircularList |    232.5 μs |    249.5 μs |    64.80 μs |     - |     - |     - |     480 B |
### Random Operations
|           Method |     Mean |    Error |   StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |---------:|---------:|---------:|------:|------:|------:|----------:|
|         WithList | 61.12 ms | 3.826 ms | 0.592 ms |     - |     - |     - |     784 B |
| WithCircularList | 51.09 ms | 4.990 ms | 1.296 ms |     - |     - |     - |     784 B |

## Todo
Support `TrimExcess`

