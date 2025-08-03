using BenchmarkDotNet.Attributes;
using VoidCore.Finance;

namespace VoidCore.Benchmark;

[MemoryDiagnoser]
[ShortRunJob]
public class FinanceLongBenchmarks
{
    [Benchmark]
    public void PerformanceTest30()
    {
        var request = new AmortizationRequest(1999m, 30 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest100()
    {
        var request = new AmortizationRequest(1999m, 100 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest200()
    {
        var request = new AmortizationRequest(1999m, 200 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest300()
    {
        var request = new AmortizationRequest(1999m, 300 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest400()
    {
        var request = new AmortizationRequest(1999m, 400 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest500()
    {
        var request = new AmortizationRequest(1999m, 500 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest700()
    {
        var request = new AmortizationRequest(1999m, 700 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest5000()
    {
        var request = new AmortizationRequest(1999m, 5000 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest10000()
    {
        var request = new AmortizationRequest(1999m, 10000 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest1000000()
    {
        var request = new AmortizationRequest(1999m, 1000000 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }

    [Benchmark]
    public void PerformanceTest5000000()
    {
        var request = new AmortizationRequest(1999m, 5000000 * 12, 0.000001m / 12);
        AmortizationCalculator.Calculate(request);
    }
}
