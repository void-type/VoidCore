namespace VoidCore.Finance;

/// <summary>
/// A request to calculate investment growth. Contains the information needed to project investment returns.
/// </summary>
public sealed class InvestmentRequest
{
    /// <summary>
    /// Create a new investment request.
    /// </summary>
    /// <param name="initialInvestment">The initial investment amount.</param>
    /// <param name="periodicContribution">The amount contributed each period.</param>
    /// <param name="numberOfPeriods">The number of investment periods.</param>
    /// <param name="ratePerPeriod">The expected return rate per period.</param>
    /// <exception cref="ArgumentException">Throws when number of periods is less than 1.</exception>
    public InvestmentRequest(decimal initialInvestment, decimal periodicContribution, int numberOfPeriods, decimal ratePerPeriod)
    {
        if (numberOfPeriods < 1)
        {
            throw new ArgumentException("Cannot be less than 1.", nameof(numberOfPeriods));
        }

        InitialInvestment = initialInvestment;
        PeriodicContribution = periodicContribution;
        NumberOfPeriods = numberOfPeriods;
        RatePerPeriod = ratePerPeriod;
    }

    /// <summary>
    /// The initial investment amount.
    /// </summary>
    public decimal InitialInvestment { get; }

    /// <summary>
    /// The amount contributed each period.
    /// </summary>
    public decimal PeriodicContribution { get; }

    /// <summary>
    /// The number of investment periods.
    /// </summary>
    public int NumberOfPeriods { get; }

    /// <summary>
    /// The expected return rate per period.
    /// </summary>
    public decimal RatePerPeriod { get; }
}
