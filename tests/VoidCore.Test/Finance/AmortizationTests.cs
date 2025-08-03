using VoidCore.Finance;
using Xunit;

namespace VoidCore.Test.Finance;

public class AmortizationTests
{
    [Fact]
    public void NegativePrincipalMortgageRequestThrows()
    {
        Assert.Throws<ArgumentException>(() => new AmortizationRequest(-340000, 360, 0.045m / 12));
    }

    [Fact]
    public void NegativePeriodCountMortgageRequestThrows()
    {
        Assert.Throws<ArgumentException>(() => new AmortizationRequest(340000, -360, 0.045m / 12));
    }

    [Fact]
    public void MediumMortgage()
    {
        var request = new AmortizationRequest(340000, 360, 0.045m / 12);

        var response = AmortizationCalculator.Calculate(request);

        CheckLoan(1722.73m, 280182.82m, 620182.82m, 30 * 12, response);
        CheckPeriod(452.79m, 1269.94m, 338198.98m, response.Schedule[3]);
        CheckPeriod(1716.29m, 6.44m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void MediumNegativeInterestMortgage()
    {
        var request = new AmortizationRequest(340000, 360, -0.045m / 12);

        var response = AmortizationCalculator.Calculate(request);

        CheckLoan(444.68m, -179914.99m, 160085.01m, 30 * 12, response);
        CheckPeriod(1700.41m, -1255.73m, 333159.87m, response.Schedule[3]);
        CheckPeriod(446.35m, -1.67m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void NoInterestLoan()
    {
        var request = new AmortizationRequest(1000, 100, 0);

        var response = AmortizationCalculator.Calculate(request);

        CheckLoan(10m, 0m, 1000m, 100, response);
        CheckPeriod(10m, 0m, 960m, response.Schedule[3]);
        CheckPeriod(10m, 0m, 0m, response.Schedule[request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void SmallLoanMonthly()
    {
        var request = new AmortizationRequest(2000, 60, 0.005m / 12);

        var response = AmortizationCalculator.Calculate(request);

        CheckLoan(33.76m, 25.52m, 2025.52m, 5 * 12, response);
        CheckPeriod(32.97m, 0.79m, 1868.22m, response.Schedule.Where(p => p.PeriodNumber == 4).Single());
        CheckPeriod(33.74m, 0.01m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void HugeLoanMonthly()
    {
        var request = new AmortizationRequest(1000000, 480, 0.20m / 12);

        var response = AmortizationCalculator.Calculate(request);

        CheckLoan(16672.64m, 7002867.64m, 8002867.64m, 40 * 12, response);
        CheckPeriod(6.28m, 16666.36m, 999975.50m, response.Schedule[3]);
        CheckPeriod(16399.32m, 273.32m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void HugeLoanQuarterly()
    {
        var request = new AmortizationRequest(1000000, 160, 0.20m / 4);

        var response = AmortizationCalculator.Calculate(request);

        CheckLoan(50020.36m, 7003258.21m, 8003258.21m, 40 * 4, response);
        CheckPeriod(23.57m, 49996.79m, 999912.23m, response.Schedule[3]);
        CheckPeriod(47638.44m, 2381.92m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void HugeLoanYearly()
    {
        var request = new AmortizationRequest(1000000, 40, 0.20m);

        var response = AmortizationCalculator.Calculate(request);

        CheckLoan(200136.17m, 7005446.73m, 8005446.73m, 40 * 1, response);
        CheckPeriod(235.30m, 199900.87m, 999269.05m, response.Schedule[3]);
        CheckPeriod(166780.14m, 33356.03m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void NoPrincipalLoan()
    {
        var request = new AmortizationRequest(0, 480, 0.20m / 12);

        var response = AmortizationCalculator.Calculate(request);

        CheckLoan(0.00m, 0.00m, 0.00m, 40 * 12, response);
        CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[3]);
        CheckPeriod(0.00m, 0.00m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void LongLoanMonthly()
    {
        var request = new AmortizationRequest(1090, 2400, 0.05m / 12);

        var response = AmortizationCalculator.Calculate(request);

        CheckLoan(4.54m, 9810.51m, 10900.51m, 200 * 12, response);
        CheckPeriod(0.00m, 4.54m, 1090.00m, response.Schedule[3]);
        CheckPeriod(4.52m, 0.02m, 0.00m, response.Schedule[request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void PaymentModificationDoublePayment()
    {
        // $1000 loan for 12 months at 5% annual interest with double payment in month 6
        var request = new AmortizationRequest(1000, 12, 0.05m / 12);
        var standardPayment = Financial.Payment(0.05m / 12, 12, -1000);
        var requestWithModification = new AmortizationRequest(1000, 12, 0.05m / 12,
            [new AmortizationPaymentModification(6, standardPayment)]);

        var response = AmortizationCalculator.Calculate(requestWithModification);

        // Standard payment should be around $85.61
        Assert.Equal(85.61m, decimal.Round(response.PaymentPerPeriod, 2));

        // Period 6 should have double payment
        var period6 = response.Schedule[5]; // 0-indexed
        Assert.Equal(decimal.Round(2.46m, 2), decimal.Round(period6.InterestPayment, 2));
        Assert.Equal(decimal.Round(168.76m, 2), decimal.Round(period6.PrincipalPayment, 2));

        // The loan should have less total interest due to the extra payment
        var standardResponse = AmortizationCalculator.Calculate(request);
        Assert.True(response.TotalInterestPaid < standardResponse.TotalInterestPaid,
            "Total interest should be less with extra payment");

        Assert.Equal(25.14m, decimal.Round(response.TotalInterestPaid, 2));
    }

    [Fact]
    public void PaymentModificationMissedPayment()
    {
        // $1000 loan for 12 months at 5% annual interest with missed payment in month 3
        var request = new AmortizationRequest(1000, 12, 0.05m / 12);
        var standardPayment = Financial.Payment(0.05m / 12, 12, -1000);
        var requestWithModification = new AmortizationRequest(1000, 12, 0.05m / 12,
            [new AmortizationPaymentModification(3, -standardPayment)]);

        var response = AmortizationCalculator.Calculate(requestWithModification);

        // Period 3 should have zero principal payment (missed payment)
        var period3 = response.Schedule[2];
        Assert.Equal(0m, decimal.Round(period3.PrincipalPayment, 2));
        Assert.Equal(3.49m, decimal.Round(period3.InterestPayment, 2));

        // Balance should not decrease in period 3
        Assert.Equal(836.78m, decimal.Round(response.Schedule[1].BalanceLeft, 2));
        Assert.Equal(836.78m, decimal.Round(response.Schedule[2].BalanceLeft, 2));

        // The loan should have more total interest due to the missed payment
        var standardResponse = AmortizationCalculator.Calculate(request);
        Assert.True(response.TotalInterestPaid > standardResponse.TotalInterestPaid,
            "Total interest should be more with missed payment");

        Assert.Equal(30.42m, decimal.Round(response.TotalInterestPaid, 2));
    }

    [Fact]
    public void PaymentModificationMultipleModifications()
    {
        // $10000 loan for 24 months at 6% annual interest with various modifications
        var standardPayment = Financial.Payment(0.06m / 12, 24, -10000);
        var modifications = new[]
        {
            // Extra $200 in month 6
            new AmortizationPaymentModification(6, 200m),
            // Missed payment in month 12
            new AmortizationPaymentModification(12, -standardPayment),
            // Extra $500 in month 18
            new AmortizationPaymentModification(18, 500m)
        };
        var request = new AmortizationRequest(10000, 24, 0.06m / 12, modifications);

        var response = AmortizationCalculator.Calculate(request);

        // Verify period 6 has extra payment
        var period6 = response.Schedule[5];
        var period6TotalPayment = period6.InterestPayment + period6.PrincipalPayment;
        Assert.Equal(decimal.Round(standardPayment + 200m, 2), decimal.Round(period6TotalPayment, 2));

        // Verify period 12 has missed payment
        var period12 = response.Schedule[11];
        Assert.Equal(0m, decimal.Round(period12.PrincipalPayment, 2));

        // Verify period 18 has extra payment
        var period18 = response.Schedule[17];
        var period18TotalPayment = period18.InterestPayment + period18.PrincipalPayment;
        Assert.Equal(decimal.Round(standardPayment + 500m, 2), decimal.Round(period18TotalPayment, 2));

        // All periods should have valid data
        foreach (var period in response.Schedule)
        {
            Assert.True(period.PeriodNumber > 0);
            Assert.True(period.BalanceLeft >= 0);
        }

        Assert.Equal(10000, response.Schedule.Sum(p => p.PrincipalPayment));
    }

    [Fact]
    public void PaymentModificationExtraPaymentPaysOffEarly()
    {
        // Small loan with large extra payment that pays off the loan early
        var request = new AmortizationRequest(1000, 12, 0.05m / 12,
            [new AmortizationPaymentModification(3, 800m)]);

        var response = AmortizationCalculator.Calculate(request);

        // Find the last period with a balance
        var lastPeriodWithBalance = response.Schedule.LastOrDefault(p => p.BalanceLeft > 0);
        Assert.Equal(2, lastPeriodWithBalance?.PeriodNumber);
        Assert.NotNull(lastPeriodWithBalance);
        Assert.Equal(836.78m, decimal.Round(lastPeriodWithBalance.BalanceLeft, 2));
        Assert.Equal(81.78m, decimal.Round(lastPeriodWithBalance.PrincipalPayment, 2));
        Assert.Equal(3.83m, decimal.Round(lastPeriodWithBalance.InterestPayment, 2));

        var payoffPeriod = response.Schedule.FirstOrDefault(p => p.BalanceLeft == 0);
        Assert.Equal(3, payoffPeriod?.PeriodNumber);
        Assert.NotNull(payoffPeriod);
        Assert.Equal(0, decimal.Round(payoffPeriod.BalanceLeft, 2));
        Assert.Equal(836.78m, decimal.Round(payoffPeriod.PrincipalPayment, 2));
        Assert.Equal(3.49m, decimal.Round(payoffPeriod.InterestPayment, 2));

        // Remaining periods should have zero values
        var periodsAfterPayoff = response.Schedule.Where(p => p.PeriodNumber > lastPeriodWithBalance?.PeriodNumber + 1);
        foreach (var period in periodsAfterPayoff)
        {
            Assert.Equal(0m, period.BalanceLeft);
            Assert.Equal(0m, period.PrincipalPayment);
            Assert.Equal(0m, period.InterestPayment);
        }

        Assert.Equal(1000, response.Schedule.Sum(p => p.PrincipalPayment));
    }

    [Fact]
    public void PaymentModificationNoInterestLoan()
    {
        // Test payment modifications on a no-interest loan
        var request = new AmortizationRequest(1200, 12, 0,
            [new AmortizationPaymentModification(6, 200m)]);

        var response = AmortizationCalculator.Calculate(request);

        // All interest payments should be zero
        foreach (var period in response.Schedule)
        {
            Assert.Equal(0m, period.InterestPayment);
        }

        // Period 6 should have extra principal payment
        var period6 = response.Schedule[5];
        Assert.Equal(300m, period6.PrincipalPayment); // 100 (normal) + 200 (extra)

        // Total principal paid should equal loan amount
        var totalPrincipalPaid = response.Schedule.Sum(p => p.PrincipalPayment);
        Assert.Equal(1200m, totalPrincipalPaid);
    }

    [Fact]
    public void PaymentModificationEarlyPeriods()
    {
        // Test modifications in the first few periods
        var request = new AmortizationRequest(5000, 60, 0.04m / 12,
            [new AmortizationPaymentModification(1, 1000m)]);

        var response = AmortizationCalculator.Calculate(request);

        // Period 1 should have the extra payment
        var period1 = response.Schedule[0];
        var standardPayment = Financial.Payment(0.04m / 12, 60, -5000);
        var expectedTotalPayment = standardPayment + 1000m;
        var actualTotalPayment = period1.InterestPayment + period1.PrincipalPayment;
        Assert.Equal(decimal.Round(expectedTotalPayment, 2), decimal.Round(actualTotalPayment, 2));

        // Subsequent periods should be calculated correctly based on the reduced balance
        Assert.Equal(3924.58m, decimal.Round(period1.BalanceLeft, 2));
        Assert.All(response.Schedule, period => Assert.True(period.BalanceLeft >= 0));
    }

    [Fact]
    public void PaymentModificationConsecutiveModifications()
    {
        // Test modifications in consecutive periods
        var modifications = new[]
        {
            new AmortizationPaymentModification(5, 100m),
            new AmortizationPaymentModification(6, 150m),
            new AmortizationPaymentModification(7, 75m)
        };
        var request = new AmortizationRequest(2000, 24, 0.03m / 12, modifications);

        var response = AmortizationCalculator.Calculate(request);

        var standardPayment = Financial.Payment(0.03m / 12, 24, -2000);

        // Verify each modification period
        var period5 = response.Schedule[4];
        var period6 = response.Schedule[5];
        var period7 = response.Schedule[6];

        Assert.Equal(decimal.Round(standardPayment + 100m, 2),
            decimal.Round(period5.InterestPayment + period5.PrincipalPayment, 2));
        Assert.Equal(decimal.Round(standardPayment + 150m, 2),
            decimal.Round(period6.InterestPayment + period6.PrincipalPayment, 2));
        Assert.Equal(decimal.Round(standardPayment + 75m, 2),
            decimal.Round(period7.InterestPayment + period7.PrincipalPayment, 2));

        // All periods should have valid, non-negative balances
        Assert.Equal(1103.12m, decimal.Round(period7.BalanceLeft, 2));
        Assert.All(response.Schedule, period => Assert.True(period.BalanceLeft >= 0));
    }

    [Fact]
    public void PaymentModificationAnnualExtraPayment()
    {
        // 240k 10yr loan, 500 extra payment annually, 4.9% interest rate
        // Should be paid off in 118 months, 62,505 interest paid
        var request = new AmortizationRequest(240000, 120, 0.049m / 12,
            [
                new AmortizationPaymentModification(1, 500m),
                new AmortizationPaymentModification(13, 500m),
                new AmortizationPaymentModification(25, 500m),
                new AmortizationPaymentModification(37, 500m),
                new AmortizationPaymentModification(49, 500m),
                new AmortizationPaymentModification(61, 500m),
                new AmortizationPaymentModification(73, 500m),
                new AmortizationPaymentModification(85, 500m),
                new AmortizationPaymentModification(97, 500m),
                new AmortizationPaymentModification(109, 500m),
                new AmortizationPaymentModification(121, 500m),
            ]);

        var response = AmortizationCalculator.Calculate(request);

        Assert.Equal(240000, request.TotalPrincipal);
        Assert.Equal(2534m, decimal.Round(response.PaymentPerPeriod, 0));

        var period1 = response.Schedule[0];
        Assert.Equal(980.00m, decimal.Round(period1.InterestPayment, 2));
        Assert.Equal(2053.86m, decimal.Round(period1.PrincipalPayment, 2));
        Assert.Equal(237946.14m, decimal.Round(period1.BalanceLeft, 2));

        Assert.Equal(62504.63m, decimal.Round(response.TotalInterestPaid, 2));
        Assert.Equal(118, response.Schedule.Count(x => x.PrincipalPayment > 0));
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
