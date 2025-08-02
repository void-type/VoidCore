namespace VoidCore.Finance;

/// <summary>
/// A period within the investment growth schedule.
/// </summary>
public sealed class InvestmentPeriod
{
    internal InvestmentPeriod(int periodNumber, decimal contribution, decimal interestEarned, decimal periodEndBalance)
    {
        PeriodNumber = periodNumber;
        Contribution = contribution;
        InterestEarned = interestEarned;
        PeriodEndBalance = periodEndBalance;
    }

    /// <summary>
    /// The chronological number of this period.
    /// </summary>
    public int PeriodNumber { get; }

    /// <summary>
    /// The contribution made this period.
    /// </summary>
    public decimal Contribution { get; }

    /// <summary>
    /// The interest earned this period.
    /// </summary>
    public decimal InterestEarned { get; }

    /// <summary>
    /// The total investment balance at the end of this period.
    /// </summary>
    public decimal PeriodEndBalance { get; }
}
