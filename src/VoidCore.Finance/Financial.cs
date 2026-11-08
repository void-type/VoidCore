namespace VoidCore.Finance;

/// <summary>
/// Functions for calculating the time value of money.
/// </summary>
public static class Financial
{
    /// <summary>
    /// The most periods a schedule calculator will build. It bounds memory use for requests that come from untrusted input.
    /// </summary>
    public const int MaxSchedulePeriods = 10_000_000;

    // (1 + rate) ^ periods as a decimal. Throws a clear error instead of an overflow or divide by zero when it can't be represented.
    private static decimal GrowthFactor(decimal interestRatePerPeriod, int numberOfPeriods)
    {
        EnsureValidRate(interestRatePerPeriod);

        var growth = Math.Pow(1 + (double)interestRatePerPeriod, numberOfPeriods);

        if (!double.IsFinite(growth) || growth >= (double)decimal.MaxValue || (growth <= 0 && numberOfPeriods != 0))
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfPeriods), "The interest rate and number of periods are too large or small to calculate.");
        }

        return (decimal)growth;
    }

    internal static void EnsureValidRate(decimal interestRatePerPeriod, string parameterName = "interestRatePerPeriod")
    {
        if (interestRatePerPeriod <= -1)
        {
            throw new ArgumentOutOfRangeException(parameterName, interestRatePerPeriod, "The interest rate per period must be greater than -1 (-100%).");
        }
    }

    /// <summary>
    /// Finds the future value of an annuity of periodic fixed payments and fixed interest rate.
    /// </summary>
    /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
    /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
    /// <param name="payment">The amount paid against the annuity every period.</param>
    /// <param name="presentValue">The present value of the annuity.</param>
    /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
    public static decimal FutureValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal presentValue = 0, bool paymentDueAtBeginningOfPeriod = false)
    {
        EnsureValidRate(interestRatePerPeriod);

        if (paymentDueAtBeginningOfPeriod)
        {
            payment *= 1 + interestRatePerPeriod;
        }

        if (numberOfPeriods == 0)
        {
            return -presentValue;
        }
        else if (interestRatePerPeriod == 0)
        {
            return -(presentValue + (numberOfPeriods * payment));
        }
        else
        {
            var pow = GrowthFactor(interestRatePerPeriod, numberOfPeriods);
            return -((payment * (pow - 1) / interestRatePerPeriod) + (presentValue * pow));
        }
    }

    /// <summary>
    /// Finds the amount of interest paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
    /// </summary>
    /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
    /// <param name="periodNumber">The period number in which to find the interest paid.</param>
    /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
    /// <param name="presentValue">The present value of the annuity.</param>
    /// <param name="futureValue">The future value of the annuity.</param>
    /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
    public static decimal InterestPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
    {
        var payment = Payment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
        return InterestPortion(interestRatePerPeriod, periodNumber, payment, presentValue, paymentDueAtBeginningOfPeriod);
    }

    /// <summary>
    /// Finds the net present value of an investment of cash flows (payments and receipts) with a discount rate.
    /// </summary>
    /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
    /// <param name="cashFlows">An array of cashflows in the order they are transacted.</param>
    public static decimal NetPresentValue(decimal interestRatePerPeriod, params decimal[] cashFlows)
    {
        EnsureValidRate(interestRatePerPeriod);

        var netPresentValue = 0.0m;

        for (var i = 0; i < cashFlows.Length; i++)
        {
            netPresentValue += cashFlows[i] / GrowthFactor(interestRatePerPeriod, i + 1);
        }

        return netPresentValue;
    }

    /// <summary>
    /// Finds the payment amount per period for an annuity of periodic fixed payments and fixed interest rate.
    /// </summary>
    /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
    /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
    /// <param name="presentValue">The present value of the annuity.</param>
    /// <param name="futureValue">The future value of the annuity.</param>
    /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
    public static decimal Payment(decimal interestRatePerPeriod, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
    {
        decimal payment;

        EnsureValidRate(interestRatePerPeriod);

        if (numberOfPeriods == 0)
        {
            payment = 0;
        }
        else if (interestRatePerPeriod == 0)
        {
            payment = (futureValue - presentValue) / numberOfPeriods;
        }
        else
        {
            var pow = GrowthFactor(interestRatePerPeriod, numberOfPeriods);

            // A rate too small to move 1 at decimal precision behaves like no interest, and would divide by zero below.
            payment = pow == 1
                ? (futureValue - presentValue) / numberOfPeriods
                : interestRatePerPeriod / (pow - 1) * -((presentValue * pow) + futureValue);
        }

        if (paymentDueAtBeginningOfPeriod)
        {
            payment /= 1 + interestRatePerPeriod;
        }

        return payment;
    }

    /// <summary>
    /// Finds the present value of an annuity of periodic fixed payments and fixed interest rate.
    /// </summary>
    /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
    /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
    /// <param name="payment">The amount paid against the annuity every period.</param>
    /// <param name="futureValue">The future value of the annuity.</param>
    /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
    public static decimal PresentValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
    {
        EnsureValidRate(interestRatePerPeriod);

        if (interestRatePerPeriod == 0)
        {
            return -futureValue - (payment * numberOfPeriods);
        }

        var num = 1.0m;
        var pow = GrowthFactor(interestRatePerPeriod, numberOfPeriods);
        if (paymentDueAtBeginningOfPeriod)
        {
            num = 1 + interestRatePerPeriod;
        }
        return -(futureValue + (payment * num * ((pow - 1) / interestRatePerPeriod))) / pow;
    }

    /// <summary>
    /// Finds the amount of principal paid as part of the payment made each payment of an annuity of periodic fixed payments and fixed interest rate.
    /// </summary>
    /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
    /// <param name="periodNumber">The period number in which to find the interest paid.</param>
    /// <param name="numberOfPeriods">The total number of periods in the annuity.</param>
    /// <param name="presentValue">The present value of the annuity.</param>
    /// <param name="futureValue">The future value of the annuity.</param>
    /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
    public static decimal PrincipalPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
    {
        var payment = Payment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
        return payment - InterestPortion(interestRatePerPeriod, periodNumber, payment, presentValue, paymentDueAtBeginningOfPeriod);
    }

    /// <summary>
    /// Finds the number of periods left in an annuity.
    /// </summary>
    /// <param name="interestRatePerPeriod">The interest rate per period. Note: use APR divided by number of periods in a year. Use decimal form: 4% should be passed as .04.</param>
    /// <param name="payment">The amount paid against the annuity every period.</param>
    /// <param name="presentValue">The present value of the annuity.</param>
    /// <param name="futureValue">The future value of the annuity.</param>
    /// <param name="paymentDueAtBeginningOfPeriod">True implies that the payments are due at the beginning of each period. Default is false.</param>
    public static decimal NumberOfPeriods(decimal interestRatePerPeriod, decimal payment, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
    {
        if (presentValue + futureValue == 0)
        {
            return 0;
        }

        EnsureValidRate(interestRatePerPeriod);

        if (interestRatePerPeriod == 0)
        {
            if (payment == 0)
            {
                throw new ArgumentException("The payment can't be zero when there is no interest.", nameof(payment));
            }

            return -(presentValue + futureValue) / payment;
        }

        var adjustedPayment = paymentDueAtBeginningOfPeriod ? payment * (1 + interestRatePerPeriod) : payment;

        var a = (double)-((interestRatePerPeriod * futureValue) - adjustedPayment);
        var b = (double)((interestRatePerPeriod * presentValue) + adjustedPayment);
        var c = (double)(1 + interestRatePerPeriod);

        var periods = Math.Log(a / b) / Math.Log(c);

        if (!double.IsFinite(periods) || Math.Abs(periods) >= (double)decimal.MaxValue)
        {
            throw new ArgumentException("The payment can never pay off the balance at this interest rate.", nameof(payment));
        }

        return (decimal)periods;
    }

    private static decimal InterestPortion(decimal interestRatePerPeriod, int periodNumber, decimal payment, decimal presentValue, bool paymentDueAtBeginningOfPeriod)
    {
        // A payment at the beginning of the first period is made before any interest has accrued.
        if (paymentDueAtBeginningOfPeriod && periodNumber == 1)
        {
            return 0;
        }

        var interestPayment = FutureValue(interestRatePerPeriod, periodNumber - 1, payment, presentValue, paymentDueAtBeginningOfPeriod) * interestRatePerPeriod;

        if (paymentDueAtBeginningOfPeriod)
        {
            interestPayment /= 1 + interestRatePerPeriod;
        }

        return interestPayment;
    }
}
