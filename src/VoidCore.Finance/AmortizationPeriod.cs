namespace VoidCore.Finance;

/// <summary>
/// A period within the amortization schedule.
/// </summary>
public sealed class AmortizationPeriod
{
    internal AmortizationPeriod(int periodNumber, decimal interestPayment, decimal principalPayment, decimal balanceLeft, decimal interestCapitalized = 0)
    {
        PeriodNumber = periodNumber;
        InterestPayment = interestPayment;
        PrincipalPayment = principalPayment;
        BalanceLeft = balanceLeft;
        InterestCapitalized = interestCapitalized;
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

    /// <summary>
    /// The interest due this period that the payment didn't cover. It was added to the balance, so the balance grows.
    /// </summary>
    public decimal InterestCapitalized { get; }
}
