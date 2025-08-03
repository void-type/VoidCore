using BenchmarkDotNet.Running;
using VoidCore.Benchmark;

BenchmarkRunner.Run<FinanceBenchmarks>();
BenchmarkRunner.Run<FinanceLongBenchmarks>();
BenchmarkRunner.Run<LookupBenchmarks>();
