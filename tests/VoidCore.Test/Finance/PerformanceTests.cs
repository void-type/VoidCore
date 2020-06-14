// using VoidCore.Finance;
// using Xunit;

// namespace VoidCore.Test.Finance
// {
//     public class PerformanceTests
//     {
//         private readonly AmortizationCalculator _calculator = new AmortizationCalculator(new Financial());

//         [Fact]
//         public void PerformanceTest()
//         {
//             // Calculate a million year mortgage, may cause unchecked overflow
//             var request = new AmortizationRequest(1999m, 1000000 * 12, 0.000001m / 12);
//             var response = _calculator.Calculate(request);

//             Assert.Equal(1000000 * 12, response.Request.NumberOfPeriods);
//         }

//         [Fact]
//         public void CpuBurner()
//         {
//             // Calculate a 5 million year mortgage, may cause unchecked overflow and excessive cpu heat
//             var request = new AmortizationRequest(1999m, 5000000 * 12, 0.000001m / 12);
//             var response = _calculator.Calculate(request);

//             Assert.Equal(5000000 * 12, response.Request.NumberOfPeriods);
//         }
//     }
// }
