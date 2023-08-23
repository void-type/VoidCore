# Benchmark results

## Financial

### (v10.0.4) Original

|       Method | Iterations |        Mean |       Error |      StdDev |      Gen0 |     Gen1 |   Allocated |
|------------- |----------- |------------:|------------:|------------:|----------:|---------:|------------:|
| PresentValue |       1000 |    177.5 μs |     0.68 μs |     0.64 μs |    2.1973 |        - |    23.46 KB |
| AmortizeLoan |       1000 | 99,052.5 μs | 1,293.98 μs | 1,147.08 μs | 3166.6667 | 166.6667 | 32456.32 KB |

### (v11.0.1) Convert to static classes

|       Method | Iterations |        Mean |     Error |    StdDev |      Gen0 |     Gen1 |  Allocated |
|------------- |----------- |------------:|----------:|----------:|----------:|---------:|-----------:|
| PresentValue |       1000 |    174.3 μs |   0.96 μs |   0.90 μs |         - |        - |          - |
| AmortizeLoan |       1000 | 96,139.5 μs | 239.38 μs | 199.89 μs | 3166.6667 | 333.3333 | 33192275 B |
