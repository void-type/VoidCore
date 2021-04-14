using System;

namespace VoidCore.Model.Time
{
    /// <summary>
    /// A service for getting the current local time.
    /// </summary>
    public class NowDateTimeService : IDateTimeService
    {
        /// <summary>
        /// Returns the current DateTime.
        /// </summary>
        public DateTime Moment => DateTime.Now;

        /// <summary>
        /// Returns the current DateTimeOffset.
        /// </summary>
        public DateTimeOffset MomentWithOffset => DateTimeOffset.Now;
    }
}
