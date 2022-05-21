# CircularList
A double-ended queue implements IList interface, featuring O(1) `RemoveAt(0)` and `Insert(0, value)` compared to `List<>`.
This means each of removing/inserting head and tail is an O(1) operation.

## Usage
There is a single file project, just copy the CircularList.cs to the target project.

## Benchmark
### Insert
|           Method |      Mean |    Error |   StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |----------:|---------:|---------:|------:|------:|------:|----------:|
|         WithList | 169.51 ms | 3.759 ms | 0.582 ms |     - |     - |     - | 512.89 KB |
| WithCircularList |  75.86 ms | 4.366 ms | 0.676 ms |     - |     - |     - | 512.79 KB |
### Insert Head
|           Method |         Mean |       Error |      StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |-------------:|------------:|------------:|------:|------:|------:|----------:|
|         WithList | 367,056.2 μs | 18,414.2 μs | 4,782.11 μs |     - |     - |     - | 512.62 KB |
| WithCircularList |     412.6 μs |    248.6 μs |    64.55 μs |     - |     - |     - | 512.49 KB |
### Remove
|           Method |     Mean |    Error |   StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |---------:|---------:|---------:|------:|------:|------:|----------:|
|         WithList | 41.91 ms | 2.851 ms | 0.740 ms |     - |     - |     - |     784 B |
| WithCircularList | 24.70 ms | 4.885 ms | 1.269 ms |     - |     - |     - |     784 B |
### Remove Head
|           Method |        Mean |       Error |      StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |------------:|------------:|------------:|------:|------:|------:|----------:|
|         WithList | 78,514.6 μs | 34,478.1 μs | 8,953.85 μs |     - |     - |     - |     480 B |
| WithCircularList |    232.5 μs |    249.5 μs |    64.80 μs |     - |     - |     - |     480 B |
### Random Operations
|           Method |     Mean |     Error |   StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |---------:|----------:|---------:|------:|------:|------:|----------:|
|         WithList | 63.86 ms |  6.458 ms | 1.677 ms |     - |     - |     - |     784 B |
| WithCircularList | 37.20 ms | 15.901 ms | 4.129 ms |     - |     - |     - |     784 B |

## Todo
Support `TrimExcess`

