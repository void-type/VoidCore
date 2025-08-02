using VoidCore.Finance;
using Xunit;

namespace VoidCore.Test.Finance;

public class InvestmentTests
{
    [Fact]
    public void MediumInvestmentMonthly()
    {
        var request = new InvestmentRequest(10000, 500, 120, 0.08m / 12);

        var response = InvestmentCalculator.Calculate(request);

        CheckInvestment(113669.42m, 60000m, 43669.42m, 10 * 12, response);
        CheckPeriod(500m, 749.47m, 113669.42m, response.Schedule[response.Request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void LargeInitialInvestmentNoContributions()
    {
        var request = new InvestmentRequest(50000, 0, 60, 0.10m / 12);

        var response = InvestmentCalculator.Calculate(request);

        CheckInvestment(82265.45m, 0m, 32265.45m, 5 * 12, response);
        CheckPeriod(0m, 679.88m, 82265.45m, response.Schedule[response.Request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void NoInitialInvestmentPeriodicContributions()
    {
        var request = new InvestmentRequest(0, 1000, 24, 0.06m / 12);

        var response = InvestmentCalculator.Calculate(request);

        CheckInvestment(25431.96m, 24000m, 1431.96m, 2 * 12, response);
        CheckPeriod(1000m, 121.55m, 25431.96m, response.Schedule[response.Request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void ZeroInterestRateInvestment()
    {
        var request = new InvestmentRequest(5000, 200, 36, 0m);

        var response = InvestmentCalculator.Calculate(request);

        CheckInvestment(12200m, 7200m, 0m, 3 * 12, response);
        CheckPeriod(200m, 0m, 12200m, response.Schedule[response.Request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void HighReturnInvestment()
    {
        var request = new InvestmentRequest(1000, 100, 48, 0.15m / 12);

        var response = InvestmentCalculator.Calculate(request);

        CheckInvestment(8338.19m, 4800m, 2538.19m, 4 * 12, response);
        CheckPeriod(100m, 12.50m, 1112.50m, response.Schedule[0]);
        CheckPeriod(100m, 101.71m, 8338.19m, response.Schedule[response.Request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void SmallRegularInvestment()
    {
        var request = new InvestmentRequest(100, 50, 12, 0.04m / 12);

        var response = InvestmentCalculator.Calculate(request);

        CheckInvestment(715.20m, 600m, 15.20m, 1 * 12, response);
        CheckPeriod(50m, 0.33m, 150.33m, response.Schedule[0]);
        CheckPeriod(50m, 2.21m, 715.20m, response.Schedule[response.Request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void LongTermInvestment()
    {
        var request = new InvestmentRequest(25000, 250, 360, 0.07m / 12);

        var response = InvestmentCalculator.Calculate(request);

        CheckInvestment(507905.19m, 90000m, 392905.19m, 30 * 12, response);
        CheckPeriod(250m, 145.83m, 25395.83m, response.Schedule[0]);
        CheckPeriod(250m, 2944.15m, 507905.19m, response.Schedule[response.Request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void NegativeInitialInvestment()
    {
        var request = new InvestmentRequest(-1000, 200, 12, 0.05m / 12);

        var response = InvestmentCalculator.Calculate(request);

        CheckInvestment(1404.61m, 2400m, 4.61m, 1 * 12, response);
        CheckPeriod(200m, -4.17m, -804.17m, response.Schedule[0]);
        CheckPeriod(200m, 5.00m, 1404.61m, response.Schedule[response.Request.NumberOfPeriods - 1]);
    }

    [Fact]
    public void SinglePeriodInvestment()
    {
        var request = new InvestmentRequest(1000, 500, 1, 0.10m);

        var response = InvestmentCalculator.Calculate(request);

        CheckInvestment(1600m, 500m, 100m, 1, response);
        CheckPeriod(500m, 100m, 1600m, response.Schedule[0]);
    }

    [Fact]
    public void ArgumentNullExceptionThrownWhenRequestIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => InvestmentCalculator.Calculate(null!));
    }

    private static void CheckInvestment(decimal finalValue, decimal totalContributions, decimal totalInterestEarned, int numberOfPeriods, InvestmentResponse investment)
    {
        Assert.Equal(finalValue, decimal.Round(investment.FinalValue, 2));
        Assert.Equal(totalContributions, decimal.Round(investment.TotalContributions, 2));
        Assert.Equal(totalInterestEarned, decimal.Round(investment.TotalInterestEarned, 2));
        Assert.Equal(numberOfPeriods, investment.Request.NumberOfPeriods);
        Assert.Equal(investment.Request.NumberOfPeriods, investment.Schedule.Count);
    }

    private static void CheckPeriod(decimal contribution, decimal interestEarned, decimal periodEndBalance, InvestmentPeriod actual)
    {
        Assert.Equal(contribution, decimal.Round(actual.Contribution, 2));
        Assert.Equal(interestEarned, decimal.Round(actual.InterestEarned, 2));
        Assert.Equal(periodEndBalance, decimal.Round(actual.PeriodEndBalance, 2));
    }
}
