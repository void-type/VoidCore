using VoidCore.Finance;
using Xunit;

namespace VoidCore.Test.Finance;

public class FinancialTests
{
    [Fact]
    public void PaymentNoFvAtEnd()
    {
        var answer = Financial.Payment(.05m, 60, -35);

        Assert.Equal(1.85m, decimal.Round(answer, 2));
    }

    [Fact]
    public void PaymentNoPeriods()
    {
        var answer = Financial.Payment(.05m, 0, -35);

        Assert.Equal(0m, decimal.Round(answer, 2));
    }

    [Fact]
    public void PaymentNoInterest()
    {
        var answer = Financial.Payment(0m, 60, -35);

        Assert.Equal(0.58m, decimal.Round(answer, 2));
    }

    [Fact]
    public void PaymentAtBeginning()
    {
        var answer = Financial.Payment(.05m, 60, -35, 4, true);

        Assert.Equal(1.75m, decimal.Round(answer, 2));
    }

    [Fact]
    public void FutureValueNoPvAtEnd()
    {
        var answer = Financial.FutureValue(.05m, 60, 35);

        Assert.Equal(-12375.43m, decimal.Round(answer, 2));
    }

    [Fact]
    public void FutureValueNoPeriods()
    {
        var answer = Financial.FutureValue(.05m, 0, 35, 110);

        Assert.Equal(-110m, decimal.Round(answer, 2));
    }

    [Fact]
    public void FutureValueNoInterest()
    {
        var answer = Financial.FutureValue(0m, 60, 35, 110);

        Assert.Equal(-2210m, decimal.Round(answer, 2));
    }

    [Fact]
    public void FutureValueAtBeginning()
    {
        var answer = Financial.FutureValue(.05m, 60, 35, -4, true);

        Assert.Equal(-12919.48m, decimal.Round(answer, 2));
    }

    [Fact]
    public void InterestPaymentNoFvAtEnd()
    {
        var answer = Financial.InterestPayment(.05m, 2, 60, -35);

        Assert.Equal(1.75m, decimal.Round(answer, 2));
    }

    [Fact]
    public void InterestPaymentAtBeginning()
    {
        var answer = Financial.InterestPayment(.05m, 2, 60, -35, 4, true);

        Assert.Equal(1.66m, decimal.Round(answer, 2));
    }

    [Fact]
    public void PrincipalPaymentNoFvAtEnd()
    {
        var answer = Financial.PrincipalPayment(.05m, 2, 60, -35);

        Assert.Equal(0.10m, decimal.Round(answer, 2));
    }

    [Fact]
    public void PrincipalPaymentAtBeginning()
    {
        var answer = Financial.PrincipalPayment(.05m, 2, 60, -35, 4, true);

        Assert.Equal(0.09m, decimal.Round(answer, 2));
    }

    [Fact]
    public void PresentValueNoFvAtEnd()
    {
        var answer = Financial.PresentValue(.05m, 60, -35);

        Assert.Equal(662.53m, decimal.Round(answer, 2));
    }

    [Fact]
    public void PresentValueNoPeriods()
    {
        var answer = Financial.PresentValue(.05m, 0, -35);

        Assert.Equal(0m, decimal.Round(answer, 2));
    }

    [Fact]
    public void PresentValueNoInterest()
    {
        var answer = Financial.PresentValue(0m, 60, -35);

        Assert.Equal(2100m, decimal.Round(answer, 2));
    }

    [Fact]
    public void PresentValueAtBeginning()
    {
        var answer = Financial.PresentValue(.05m, 60, -35, 4, true);

        Assert.Equal(695.44m, decimal.Round(answer, 2));
    }

    [Fact]
    public void NetPresentValue()
    {
        var answer = Financial.NetPresentValue(.04m / 12, 12, 12, 12, 12);

        Assert.Equal(47.60m, decimal.Round(answer, 2));
    }

    [Fact]
    public void NetPresentValueNoFlows()
    {
        var answer = Financial.NetPresentValue(.04m / 12);

        Assert.Equal(0m, decimal.Round(answer, 2));
    }

    [Fact]
    public void NumberOfPeriodsNoFvAtEnd()
    {
        var answer = Financial.NumberOfPeriods(.05m, 60, -35);

        Assert.Equal(0.61m, decimal.Round(answer, 2));
    }

    [Fact]
    public void NumberOfPeriodsNoValue()
    {
        var answer = Financial.NumberOfPeriods(.05m, 60, 0);

        Assert.Equal(0m, decimal.Round(answer, 2));
    }

    [Fact]
    public void NumberOfPeriodsNoInterest()
    {
        var answer = Financial.NumberOfPeriods(0m, 60, -35);

        Assert.Equal(0.58m, decimal.Round(answer, 2));
    }

    [Fact]
    public void NumberOfPeriodsAtBeginning()
    {
        var answer = Financial.NumberOfPeriods(.05m, 60, -35, 4, true);

        Assert.Equal(0.51m, decimal.Round(answer, 2));
    }
}
