using BenchmarkDotNet.Attributes;
using VoidCore.Finance;

namespace VoidCore.Benchmark;

[MemoryDiagnoser]
public class FinanceBenchmarks
{
    [Params(1000, 2000, 4000)]
    public int Iterations { get; set; }

    [Benchmark]
    public void PresentValue()
    {
        for (var i = 0; i <= Iterations; i++)
        {
            Financial.PresentValue(0.035m, 362, 0, 482_000);
        }
    }

    [Benchmark]
    public void AmortizeLoan()
    {
        for (var i = 0; i <= Iterations; i++)
        {
            // Calculate a typical 30 year mortgage
            var request = new AmortizationRequest(340000, 360, 0.045m / 12);
            AmortizationCalculator.Calculate(request);
        }
    }

    [Benchmark]
    public void AmortizeLoanWithModification()
    {
        for (var i = 0; i <= Iterations; i++)
        {
            // Calculate a typical 30 year mortgage with modification
            var request = new AmortizationRequest(340000, 360, 0.045m / 12,
                new[] { new AmortizationPaymentModification(6, 2000m) });
            AmortizationCalculator.Calculate(request);
        }
    }

    // [Benchmark]
    // public void PerformanceTest()
    // {
    //     for (int i = 0; i <= Iterations; i++)
    //     {
    //         // Calculate a million year mortgage, may cause unchecked overflow
    //         var request = new AmortizationRequest(1999m, 1000000 * 12, 0.000001m / 12);
    //         AmortizationCalculator.Calculate(request);
    //     }
    // }

    // [Benchmark]
    // public void PerformanceTest2()
    // {
    //     for (int i = 0; i <= Iterations; i++)
    //     {
    //         // Calculate a 5 million year mortgage, may cause unchecked overflow and excessive cpu heat
    //         var request = new AmortizationRequest(1999m, 5000000 * 12, 0.000001m / 12);
    //         AmortizationCalculator.Calculate(request);
    //     }
    // }
}
