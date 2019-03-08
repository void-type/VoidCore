using System;

namespace VoidCore.Model.Time
{
    /// <summary>
    /// Provides a single moment in time specified upon construction.
    /// </summary>
    public class DiscreteDateTimeService : IDateTimeService
    {
        private readonly DateTime _when;

        /// <summary>
        /// Make a new discrete date time service.
        /// </summary>
        /// <param name="when">The static moment in time to return</param>
        public DiscreteDateTimeService(DateTime when)
        {
            _when = when;
        }

        /// <inheritdoc/>
        public DateTime Moment => _when;
    }
}
