using System;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Time
{
    /// <summary>
    /// Provides a single moment in time specified upon construction.
    /// </summary>
    public class DiscreteDateTimeService : IDateTimeService
    {
        private readonly DateTime? _moment;
        private readonly DateTimeOffset? _momentWithOffset;

        /// <summary>
        /// Make a new discrete date time service.
        /// </summary>
        /// <param name="when">The static moment in time to return</param>
        /// <param name="whenWithOffset">The static moment in time to return</param>
        public DiscreteDateTimeService(DateTime? when = null, DateTimeOffset? whenWithOffset = null)
        {
            _moment = when;
            _momentWithOffset = whenWithOffset;
        }

        /// <inheritdoc/>
        public DateTime Moment => _moment.EnsureNotNull(nameof(Moment), "Value was accessed without being set in constructor.").Value;

        /// <inheritdoc/>
        public DateTimeOffset MomentWithOffset => _momentWithOffset.EnsureNotNull(nameof(MomentWithOffset), "Value was accessed without being set in constructor.").Value;
    }
}
