namespace VoidCore.Finance;

/// <summary>
/// A period within the amortization schedule.
/// </summary>
public sealed class AmortizationPeriod
{
    internal AmortizationPeriod(int periodNumber, decimal interestPayment, decimal principalPayment, decimal balanceLeft)
    {
        PeriodNumber = periodNumber;
        InterestPayment = interestPayment;
        PrincipalPayment = principalPayment;
        BalanceLeft = balanceLeft;
    }

    /// <summary>
    /// The chronological number of this period.
    /// </summary>
    public int PeriodNumber { get; }

    /// <summary>
    /// The interest paid this period.
    /// </summary>
    public decimal InterestPayment { get; }

    /// <summary>
    /// The principal paid this period.
    /// </summary>
    public decimal PrincipalPayment { get; }

    /// <summary>
    /// The balance remaining after this period.
    /// </summary>
    public decimal BalanceLeft { get; }
}
