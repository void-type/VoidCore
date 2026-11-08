namespace VoidCore.Finance;

/// <summary>
/// Calculates an amortization schedule with information about the loan.
/// </summary>
public static class AmortizationCalculator
{
    // A remaining balance smaller than this at the end of the loan is rounding error from the payment calculation, not money owed.
    private const decimal RoundingTolerance = 0.01m;

    /// <summary>
    /// Calculate the loan request. A payment that is less than the interest due for a period (for example a missed payment) leaves
    /// the unpaid interest to be added to the balance, which is reported as the period's InterestCapitalized. The schedule always has
    /// the requested number of periods, so after underpayments the last period can still have a balance left.
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

        var paymentPerPeriod = Financial.Payment(ratePerPeriod, numberOfPeriods, -totalPrincipal);

        CalculateScheduleWithModifications(schedule, request, paymentPerPeriod);

        var totalInterestPaid = schedule.Sum(p => p.InterestPayment);
        var totalPaid = schedule.Sum(p => p.InterestPayment + p.PrincipalPayment);

        return new AmortizationResponse(paymentPerPeriod, totalInterestPaid, totalPaid, schedule, request);
    }

    private static void CalculateScheduleWithModifications(AmortizationPeriod[] schedule, AmortizationRequest request, decimal standardPayment)
    {
        var ratePerPeriod = request.RatePerPeriod;
        var numberOfPeriods = request.NumberOfPeriods;
        var totalPrincipal = request.TotalPrincipal;

        var modificationsByPeriod = request.PaymentModifications
            .GroupBy(modification => modification.PeriodNumber)
            .ToDictionary(group => group.Key, group => group.Sum(modification => modification.ModificationAmount));

        var currentBalance = totalPrincipal;
        var currentPeriod = 1;

        while (currentPeriod <= numberOfPeriods && currentBalance > 0)
        {
            modificationsByPeriod.TryGetValue(currentPeriod, out var modificationAmount);

            // Ensure payment is not negative
            var actualPayment = Math.Max(0, standardPayment + modificationAmount);

            var interestDue = currentBalance * ratePerPeriod;

            decimal interestPayment;
            decimal interestCapitalized;
            decimal principalPayment;

            if (actualPayment < interestDue)
            {
                // The payment doesn't cover the interest. Everything paid goes to interest and the rest is added to the balance.
                interestPayment = actualPayment;
                interestCapitalized = interestDue - actualPayment;
                principalPayment = 0;
            }
            else
            {
                interestPayment = interestDue;
                interestCapitalized = 0;

                // Principal payment doesn't exceed the balance left
                principalPayment = Math.Min(actualPayment - interestDue, currentBalance);

                // Close out what the last payment leaves behind when the payment couldn't be rounded exactly.
                if (currentPeriod == numberOfPeriods && currentBalance - principalPayment is > 0 and < RoundingTolerance)
                {
                    principalPayment = currentBalance;
                }
            }

            var newBalance = currentBalance - principalPayment + interestCapitalized;

            schedule[currentPeriod - 1] = new AmortizationPeriod(currentPeriod, interestPayment, principalPayment, newBalance, interestCapitalized);

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
