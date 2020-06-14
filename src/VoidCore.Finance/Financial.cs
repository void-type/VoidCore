using System;

namespace VoidCore.Finance
{
    /// <inheritdoc/>
    public class Financial : IFinancial
    {
        /// <inheritdoc/>
        public decimal FutureValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal presentValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
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
                var pow = (decimal)Math.Pow(1 + (double)interestRatePerPeriod, numberOfPeriods);
                return -(payment * (pow - 1) / interestRatePerPeriod + presentValue * pow);
            }
        }

        /// <inheritdoc/>
        public decimal InterestPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var payment = Payment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
            var interestPayment = FutureValue(interestRatePerPeriod, periodNumber - 1, payment, presentValue, paymentDueAtBeginningOfPeriod) * interestRatePerPeriod;

            if (paymentDueAtBeginningOfPeriod)
            {
                interestPayment /= 1 + interestRatePerPeriod;
            }

            return interestPayment;
        }

        /// <inheritdoc/>
        public decimal NetPresentValue(decimal interestRatePerPeriod, params decimal[] cashFlows)
        {
            var netPresentValue = 0.0m;

            for (var i = 0; i < cashFlows.Length; i++)
            {
                netPresentValue += cashFlows[i] / (decimal)Math.Pow((1 + (double)interestRatePerPeriod), (i + 1));
            }

            return netPresentValue;
        }

        /// <inheritdoc/>
        public decimal Payment(decimal interestRatePerPeriod, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            decimal payment;

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
                var pow = (decimal)Math.Pow(1 + (double)interestRatePerPeriod, numberOfPeriods);
                payment = interestRatePerPeriod / (pow - 1) * -(presentValue * pow + futureValue);
            }

            if (paymentDueAtBeginningOfPeriod)
            {
                payment /= 1 + interestRatePerPeriod;
            }

            return payment;
        }

        /// <inheritdoc/>
        public decimal PresentValue(decimal interestRatePerPeriod, int numberOfPeriods, decimal payment, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var num = 1.0m;
            var pow = (decimal)Math.Pow(1 + (double)interestRatePerPeriod, numberOfPeriods);

            if (interestRatePerPeriod == 0)
            {
                return (-futureValue - (payment * numberOfPeriods));
            }

            if (paymentDueAtBeginningOfPeriod)
            {
                num = 1 + interestRatePerPeriod;
            }
            return -(futureValue + ((payment * num) * ((pow - 1) / interestRatePerPeriod))) / pow;
        }

        /// <inheritdoc/>
        public decimal PrincipalPayment(decimal interestRatePerPeriod, int periodNumber, int numberOfPeriods, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            var payment = Payment(interestRatePerPeriod, numberOfPeriods, presentValue, futureValue, paymentDueAtBeginningOfPeriod);
            var interestPayment = FutureValue(interestRatePerPeriod, periodNumber - 1, payment, presentValue, paymentDueAtBeginningOfPeriod) * interestRatePerPeriod;

            if (paymentDueAtBeginningOfPeriod)
            {
                interestPayment /= 1 + interestRatePerPeriod;
            }

            return payment - interestPayment;
        }

        /// <inheritdoc/>
        public decimal NumberOfPeriods(decimal interestRatePerPeriod, decimal payment, decimal presentValue, decimal futureValue = 0, bool paymentDueAtBeginningOfPeriod = false)
        {
            if (presentValue + futureValue == 0)
            {
                return 0;
            }

            if (interestRatePerPeriod == 0)
            {
                return -(presentValue + futureValue) / payment;
            }

            var adjustedPayment = paymentDueAtBeginningOfPeriod ? payment * (1 + interestRatePerPeriod) : payment;

            var a = (double)-((interestRatePerPeriod * futureValue) - adjustedPayment);
            var b = (double)((interestRatePerPeriod * presentValue) + adjustedPayment);
            var c = (double)(1 + interestRatePerPeriod);

            return (decimal)(Math.Log(a / b) / Math.Log(c));
        }
    }
}
