using BenchmarkDotNet.Running;
using VoidCore.Benchmark;

BenchmarkRunner.Run<FinanceBenchmarks>();
BenchmarkRunner.Run<TextHelpersBenchmarks>();
// BenchmarkRunner.Run<LookupBenchmarks>();
