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
}
