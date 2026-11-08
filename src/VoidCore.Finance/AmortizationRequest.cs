namespace VoidCore.Finance;

/// <summary>
/// A request to calculate. Contains the information needed to initiate the loan.
/// </summary>
public sealed class AmortizationRequest
{
    /// <summary>
    /// Create a new request.
    /// </summary>
    /// <param name="totalPrincipal">The total principal amount of the loan.</param>
    /// <param name="numberOfPeriods">The number of payment periods in the loan.</param>
    /// <param name="ratePerPeriod">The interest rate per period of the loan.</param>
    /// <param name="paymentModifications">Optional payment modifications for specific periods.</param>
    /// <exception cref="ArgumentException">Throws when number of periods is less than 1.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throws when number of periods is more than <see cref="Financial.MaxSchedulePeriods"/> or the rate is -100% or lower.</exception>
    public AmortizationRequest(decimal totalPrincipal, int numberOfPeriods, decimal ratePerPeriod,
        IEnumerable<AmortizationPaymentModification>? paymentModifications = null)
    {
        if (totalPrincipal < 0)
        {
            throw new ArgumentException("Cannot be less than 0.", nameof(totalPrincipal));
        }

        if (numberOfPeriods < 1)
        {
            throw new ArgumentException("Cannot be less than 1.", nameof(numberOfPeriods));
        }

        ArgumentOutOfRangeException.ThrowIfGreaterThan(numberOfPeriods, Financial.MaxSchedulePeriods);
        Financial.EnsureValidRate(ratePerPeriod, nameof(ratePerPeriod));
        TotalPrincipal = totalPrincipal;
        NumberOfPeriods = numberOfPeriods;
        RatePerPeriod = ratePerPeriod;
        PaymentModifications = paymentModifications?.ToList().AsReadOnly()
            ?? new List<AmortizationPaymentModification>().AsReadOnly();
    }

    /// <summary>
    /// The total principal amount of the loan.
    /// </summary>
    public decimal TotalPrincipal { get; }

    /// <summary>
    /// The number of payment periods in the loan.
    /// </summary>
    public int NumberOfPeriods { get; }

    /// <summary>
    /// The interest rate per period of the loan.
    /// </summary>
    public decimal RatePerPeriod { get; }

    /// <summary>
    /// Optional payment modifications for specific periods.
    /// </summary>
    public IReadOnlyList<AmortizationPaymentModification> PaymentModifications { get; }
}
