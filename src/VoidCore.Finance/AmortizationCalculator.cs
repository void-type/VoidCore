namespace VoidCore.Finance;

/// <summary>
/// Calculates an amortization schedule with information about the loan.
/// </summary>
public static class AmortizationCalculator
{
    /// <summary>
    /// Calculate the loan request.
    /// </summary>
    /// <param name="request">The request to calculate</param>
    /// <returns>A completed AmortizationResponse</returns>
    /// <exception cref="ArgumentNullException">Throws when request is null.</exception>
    public static AmortizationResponse Calculate(AmortizationRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request), "Calculator request cannot be null.");
        }

        var ratePerPeriod = request.RatePerPeriod;
        var numberOfPeriods = request.NumberOfPeriods;
        var totalPrincipal = request.TotalPrincipal;

        var schedule = new AmortizationPeriod[numberOfPeriods];

        CalculateScheduleWithModifications(schedule, request);

        var paymentPerPeriod = Financial.Payment(ratePerPeriod, numberOfPeriods, -totalPrincipal);
        var totalInterestPaid = schedule.Sum(p => p.InterestPayment);
        var totalPaid = totalPrincipal + totalInterestPaid;

        return new AmortizationResponse(paymentPerPeriod, totalInterestPaid, totalPaid, schedule, request);
    }

    /// <summary>
    /// Calculates the amortization schedule with payment modifications by splitting into parallelizable chunks.
    /// </summary>
    private static void CalculateScheduleWithModifications(AmortizationPeriod[] schedule, AmortizationRequest request)
    {
        var ratePerPeriod = request.RatePerPeriod;
        var numberOfPeriods = request.NumberOfPeriods;
        var totalPrincipal = request.TotalPrincipal;
        var modificationPeriods = request.PaymentModifications;

        var standardPayment = Financial.Payment(ratePerPeriod, numberOfPeriods, -totalPrincipal);

        var currentBalance = totalPrincipal;
        var currentPeriod = 1;

        while (currentPeriod <= numberOfPeriods && currentBalance > 0)
        {
            var modificationAmount = modificationPeriods
                .Where(p => p.PeriodNumber == currentPeriod)
                .Sum(x => x.ModificationAmount);

            // Ensure payment is not negative
            var actualPayment = Math.Max(0, standardPayment + modificationAmount);

            var interestPayment = currentBalance * ratePerPeriod;

            // Ensure principal payment is not negative and doesn't exceed balance left
            var principalPayment = Math.Min(Math.Max(0, actualPayment - interestPayment), currentBalance);

            var newBalance = Math.Max(0, currentBalance - principalPayment);

            schedule[currentPeriod - 1] = new AmortizationPeriod(currentPeriod, interestPayment, principalPayment, newBalance);

            currentBalance = newBalance;
            currentPeriod++;
        }

        // Fill any remaining periods with zero values if balance is zero
        for (var zeroPeriod = currentPeriod; zeroPeriod <= numberOfPeriods; zeroPeriod++)
        {
            schedule[zeroPeriod - 1] = new AmortizationPeriod(zeroPeriod, 0, 0, 0);
        }
    }
}
