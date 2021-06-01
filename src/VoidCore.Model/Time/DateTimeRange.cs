using System;

namespace VoidCore.Model.Time
{
    /// <summary>
    /// Represents a range of time.
    /// </summary>
    /// <param name="StartDate">Range start</param>
    /// <param name="EndDate">Range end</param>
    public record DateTimeRange(DateTime StartDate, DateTime EndDate);
}
