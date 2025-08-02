namespace VoidCore.Finance;

/// <summary>
/// Represents a modification to a payment in an amortization schedule.
/// </summary>
public class AmortizationPaymentModification
{
    /// <summary>
    /// Creates a new payment modification.
    /// </summary>
    /// <param name="periodNumber">The period number where the modification applies.</param>
    /// <param name="modificationAmount">The amount of the payment modification. This is the difference from the normal payment amount for the period.</param>
    public AmortizationPaymentModification(int periodNumber, decimal modificationAmount)
    {
        PeriodNumber = periodNumber;
        ModificationAmount = modificationAmount;
    }

    /// <summary>
    /// Gets the period number where the modification applies.
    /// </summary>
    public int PeriodNumber { get; }

    /// <summary>
    /// Gets the amount of the payment modification.
    /// </summary>
    public decimal ModificationAmount { get; }
}
