using System.Linq;
using VoidCore.Finance;
using Xunit;

namespace VoidCore.Test.Finance
{
    public class AmortizationTests
    {
        private readonly AmortizationCalculator _calculator = new AmortizationCalculator(new Financial());

        [Fact]
        public void MediumMortgage()
        {
            var request = new AmortizationRequest(340000, 360, 0.045m / 12);

            var response = _calculator.Calculate(request);

            CheckLoan(1722.73m, 280182.82m, 620182.82m, 30 * 12, response);
            CheckPeriod(452.79m, 1269.94m, 338198.98m, response.Schedule[3]);
            CheckPeriod(1716.29m, 6.44m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void MediumNegativePrincipalMortgage()
        {
            var request = new AmortizationRequest(-340000, 360, 0.045m / 12);

            var response = _calculator.Calculate(request);

            CheckLoan(-1722.73m, -280182.82m, -620182.82m, 30 * 12, response);
            CheckPeriod(-452.79m, -1269.94m, -338198.98m, response.Schedule[3]);
            CheckPeriod(-1716.29m, -6.44m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void MediumNegativeInterestMortgage()
        {
            var request = new AmortizationRequest(340000, 360, -0.045m / 12);

            var response = _calculator.Calculate(request);

            CheckLoan(444.68m, -179914.99m, 160085.01m, 30 * 12, response);
            CheckPeriod(1700.41m, -1255.73m, 333159.87m, response.Schedule[3]);
            CheckPeriod(446.35m, -1.67m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void NoInterestLoan()
        {
            var request = new AmortizationRequest(1000, 100, 0);

            var response = _calculator.Calculate(request);

            CheckLoan(10m, 0m, 1000m, 100, response);
            CheckPeriod(10m, 0m, 960m, response.Schedule[3]);
            CheckPeriod(10m, 0m, 0m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void SmallLoanMonthly()
        {
            var request = new AmortizationRequest(2000, 60, 0.005m / 12);

            var response = _calculator.Calculate(request);

            CheckLoan(33.76m, 25.52m, 2025.52m, 5 * 12, response);
            CheckPeriod(32.97m, 0.79m, 1868.22m, response.Schedule.Where(p => p.PeriodNumber == 4).Single());
            CheckPeriod(33.74m, 0.01m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void HugeLoanMonthly()
        {
            var request = new AmortizationRequest(1000000, 480, 0.20m / 12);

            var response = _calculator.Calculate(request);

            CheckLoan(16672.64m, 7002867.64m, 8002867.64m, 40 * 12, response);
            CheckPeriod(6.28m, 16666.36m, 999975.50m, response.Schedule[3]);
            CheckPeriod(16399.32m, 273.32m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void HugeLoanQuarterly()
        {
            var request = new AmortizationRequest(1000000, 160, 0.20m / 4);

            var response = _calculator.Calculate(request);

            CheckLoan(50020.36m, 7003258.21m, 8003258.21m, 40 * 4, response);
            CheckPeriod(23.57m, 49996.79m, 999912.23m, response.Schedule[3]);
            CheckPeriod(47638.44m, 2381.92m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void HugeLoanYearly()
        {
            var request = new AmortizationRequest(1000000, 40, 0.20m);

            var response = _calculator.Calculate(request);

            CheckLoan(200136.17m, 7005446.73m, 8005446.73m, 40 * 1, response);
            CheckPeriod(235.30m, 199900.87m, 999269.05m, response.Schedule[3]);
            CheckPeriod(166780.14m, 33356.03m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void NoLoan()
        {
            var request = new AmortizationRequest(0, 480, 0.20m / 12);

            var response = _calculator.Calculate(request);

            CheckLoan(0.00m, 0.00m, 0.00m, 40 * 12, response);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[3]);
            CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        [Fact]
        public void LongLoanMonthly()
        {
            var request = new AmortizationRequest(1090, 2400, 0.05m / 12);

            var response = _calculator.Calculate(request);

            CheckLoan(4.54m, 9810.51m, 10900.51m, 200 * 12, response);
            CheckPeriod(0.00m, 4.54m, 1090.00m, response.Schedule[3]);
            CheckPeriod(4.52m, 0.02m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
        }

        private static void CheckLoan(decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, int numberOfPeriods, AmortizationResponse loan)
        {
            Assert.Equal(paymentPerPeriod, decimal.Round(loan.PaymentPerPeriod, 2));
            Assert.Equal(totalInterestPaid, decimal.Round(loan.TotalInterestPaid, 2));
            Assert.Equal(totalPaid, decimal.Round(loan.TotalPaid, 2));
            Assert.Equal(numberOfPeriods, loan.Request.NumberOfPeriods);
            Assert.Equal(loan.Request.NumberOfPeriods, loan.Schedule.Count);
        }

        private static void CheckPeriod(decimal principalPayment, decimal interestPayment, decimal balanceLeft, AmortizationPeriod actual)
        {
            Assert.Equal(principalPayment, decimal.Round(actual.PrincipalPayment, 2));
            Assert.Equal(interestPayment, decimal.Round(actual.InterestPayment, 2));
            Assert.Equal(balanceLeft, decimal.Round(actual.BalanceLeft, 2));
        }
    }
}
