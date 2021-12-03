using System;
using System.Linq;
using System.Threading.Tasks;

namespace VoidCore.Finance;

/// <summary>
/// Calculates an amortization schedule with information about the loan.
/// </summary>
public sealed class AmortizationCalculator
{
    private readonly IFinancial _financial;

    /// <summary>
    /// Create a new Amortization Calculator
    /// </summary>
    /// <param name="financial"></param>
    public AmortizationCalculator(IFinancial financial)
    {
        _financial = financial;
    }

    /// <summary>
    /// Calculate the loan request.
    /// </summary>
    /// <param name="request">The request to calculate</param>
    /// <returns>A completed AmortizationResponse</returns>
    /// <exception cref="ArgumentNullException">Throws when request is null.</exception>
    public AmortizationResponse Calculate(AmortizationRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request), "Calculator request cannot be null.");
        }

        var ratePerPeriod = request.RatePerPeriod;
        var numberOfPeriods = request.NumberOfPeriods;
        var totalPrincipal = request.TotalPrincipal;

        var schedule = new AmortizationPeriod[numberOfPeriods];

        Parallel.For(1, numberOfPeriods + 1, periodNumber =>
        {
            var principalPayment = _financial.PrincipalPayment(ratePerPeriod, periodNumber, numberOfPeriods, -totalPrincipal);

            var interestPayment = _financial.InterestPayment(ratePerPeriod, periodNumber, numberOfPeriods, -totalPrincipal);

            var balanceLeft = ratePerPeriod == 0 ?
                totalPrincipal - (principalPayment * periodNumber) :
                _financial.InterestPayment(ratePerPeriod, periodNumber + 1, numberOfPeriods, -totalPrincipal) / ratePerPeriod;

            schedule[periodNumber - 1] = new AmortizationPeriod(periodNumber, interestPayment, principalPayment, balanceLeft);
        });

        var paymentPerPeriod = _financial.Payment(ratePerPeriod, numberOfPeriods, -totalPrincipal);
        var totalInterestPaid = schedule.Sum(p => p.InterestPayment);
        var totalPaid = totalPrincipal + totalInterestPaid;

        return new AmortizationResponse(paymentPerPeriod, totalInterestPaid, totalPaid, schedule, request);
    }
}
