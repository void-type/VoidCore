using System;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Time
{
    /// <summary>
    /// Represents a range of time.
    /// </summary>
    /// <param name="StartDate">Range start</param>
    /// <param name="EndDate">Range end</param>
    public record DateTimeRange
    {
        public DateTimeRange(DateTime startDate, DateTime endDate)
        {
            startDate.Ensure(s => s <= endDate, nameof(startDate), "startDate cannot be greater than endDate.");
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
