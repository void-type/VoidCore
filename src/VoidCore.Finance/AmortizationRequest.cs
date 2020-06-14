using System;

namespace VoidCore.Finance
{
    /// <summary>
    /// A request to calculate.true Contains the information needed to initiate the loan.
    /// </summary>
    public sealed class AmortizationRequest
    {
        /// <summary>
        /// Create a new request.
        /// </summary>
        /// <param name="totalPrincipal">The total principal amount of the loan.</param>
        /// <param name="numberOfPeriods">The number of payment periods in the loan.</param>
        /// <param name="ratePerPeriod">The interest rate per period of the loan.</param>
        public AmortizationRequest(decimal totalPrincipal, int numberOfPeriods, decimal ratePerPeriod)
        {
            if (numberOfPeriods < 1)
            {
                throw new ArgumentException("Cannot be less than 1.", nameof(numberOfPeriods));
            }

            TotalPrincipal = totalPrincipal;
            NumberOfPeriods = numberOfPeriods;
            RatePerPeriod = ratePerPeriod;
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
        /// /// </summary>
        public decimal RatePerPeriod { get; }
    }
}
