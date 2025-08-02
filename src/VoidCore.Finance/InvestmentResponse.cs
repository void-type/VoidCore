namespace VoidCore.Finance;

/// <summary>
/// The response of the investment calculation. Includes the request.
/// </summary>
public sealed class InvestmentResponse
{
    internal InvestmentResponse(decimal finalValue, decimal totalContributions, decimal totalInterestEarned, IReadOnlyList<InvestmentPeriod> schedule, InvestmentRequest request)
    {
        FinalValue = finalValue;
        TotalContributions = totalContributions;
        TotalInterestEarned = totalInterestEarned;
        Schedule = schedule;
        Request = request;
    }

    /// <summary>
    /// The final value of the investment at the end of all periods.
    /// </summary>
    public decimal FinalValue { get; }

    /// <summary>
    /// The total amount contributed over the lifetime of the investment.
    /// </summary>
    public decimal TotalContributions { get; }

    /// <summary>
    /// The total interest earned over the lifetime of the investment.
    /// </summary>
    public decimal TotalInterestEarned { get; }

    /// <summary>
    /// The investment growth schedule.
    /// </summary>
    public IReadOnlyList<InvestmentPeriod> Schedule { get; }

    /// <summary>
    /// The calculation request.
    /// </summary>
    public InvestmentRequest Request { get; }
}
