namespace VoidCore.Finance;

/// <summary>
/// Calculates an investment growth schedule with information about periodic contributions and returns.
/// </summary>
public static class InvestmentCalculator
{
    /// <summary>
    /// Calculate the investment growth.
    /// </summary>
    /// <param name="request">The request to calculate</param>
    /// <returns>A completed InvestmentResponse</returns>
    /// <exception cref="ArgumentNullException">Throws when request is null.</exception>
    public static InvestmentResponse Calculate(InvestmentRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request), "Calculator request cannot be null.");
        }

        var ratePerPeriod = request.RatePerPeriod;
        var numberOfPeriods = request.NumberOfPeriods;
        var initialInvestment = request.InitialInvestment;
        var periodicContribution = request.PeriodicContribution;

        var schedule = new InvestmentPeriod[numberOfPeriods];
        var beginningBalance = initialInvestment;

        for (var periodNumber = 1; periodNumber <= numberOfPeriods; periodNumber++)
        {
            var interestEarned = beginningBalance * ratePerPeriod;

            var periodEndBalance = beginningBalance + interestEarned + periodicContribution;

            schedule[periodNumber - 1] = new InvestmentPeriod(periodNumber, periodicContribution, interestEarned, periodEndBalance);

            beginningBalance = periodEndBalance;
        }

        var finalValue = schedule.LastOrDefault()?.PeriodEndBalance ?? initialInvestment;
        var totalContributions = periodicContribution * numberOfPeriods;
        var totalInterestEarned = schedule.Sum(p => p.InterestEarned);

        return new InvestmentResponse(finalValue, totalContributions, totalInterestEarned, schedule, request);
    }
}
