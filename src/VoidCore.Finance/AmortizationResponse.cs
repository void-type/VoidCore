using System.Collections.Generic;

namespace VoidCore.Finance;

/// <summary>
/// The response of the calculation. Includes the request.
/// </summary>
public sealed class AmortizationResponse
{
    internal AmortizationResponse(decimal paymentPerPeriod, decimal totalInterestPaid, decimal totalPaid, IReadOnlyList<AmortizationPeriod> schedule, AmortizationRequest request)
    {
        PaymentPerPeriod = paymentPerPeriod;
        TotalInterestPaid = totalInterestPaid;
        TotalPaid = totalPaid;
        Schedule = schedule;
        Request = request;
    }

    /// <summary>
    /// The total payment per period.
    /// </summary>
    public decimal PaymentPerPeriod { get; }

    /// <summary>
    /// The total interest paid over the lifetime of the loan.
    /// </summary>
    public decimal TotalInterestPaid { get; }

    /// <summary>
    /// The total paid over the lifetime of the loan.
    /// </summary>
    public decimal TotalPaid { get; }

    /// <summary>
    /// The amortization schedule.
    /// </summary>
    public IReadOnlyList<AmortizationPeriod> Schedule { get; }

    /// <summary>
    /// The calculation request.
    /// /// </summary>
    public AmortizationRequest Request { get; }
}
