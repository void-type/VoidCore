using System;

namespace VoidCore.Model.Time
{
    /// <summary>
    /// Provides a single moment in time specified upon construction.
    /// </summary>
    public class DiscreteDateTimeService : IDateTimeService
    {
        /// <summary>
        /// Make a new discrete date time service.
        /// /// </summary>
        /// <param name="when"></param>
        public DiscreteDateTimeService(DateTime when)
        {
            _when = when;
        }

        /// <inheritdoc/>
        public DateTime Moment => _when;

        private readonly DateTime _when;
    }
}
