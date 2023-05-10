using System;
using System.Collections.Generic;

namespace VoidCore.Model.Time;

/// <summary>
/// Helper functions for DateTime.
/// </summary>
public static class DateTimeHelpers
{
    /// <summary>
    /// Split a larger date range into smaller intervals of a maximum size.
    /// </summary>
    /// <param name="startDate">Range start</param>
    /// <param name="endDate">Range end</param>
    /// <param name="intervalSizeDays">How many days the interval should be</param>
    /// <param name="overlapMitigation">A strategy to mitigate overlap</param>
    public static IEnumerable<DateTimeRange> SplitDateRangeIntoIntervals(DateTime startDate, DateTime endDate, int intervalSizeDays, OverlapMitigation overlapMitigation = OverlapMitigation.None)
    {
        return new DateTimeRange(startDate, endDate)
            .SplitDateRangeIntoIntervals(intervalSizeDays, overlapMitigation);
    }

    /// <summary>
    /// Split a larger date range into smaller intervals of a maximum size.
    /// </summary>
    /// <param name="range">The range</param>
    /// <param name="intervalSizeDays">How many days the interval should be</param>
    /// <param name="overlapMitigation">A strategy to mitigate overlap</param>
    public static IEnumerable<DateTimeRange> SplitDateRangeIntoIntervals(this DateTimeRange range, int intervalSizeDays, OverlapMitigation overlapMitigation = OverlapMitigation.None)
    {
        var rangeStart = range.StartDate;
        var rangeEnd = range.EndDate;

        var intervalStart = rangeStart;
        DateTime intervalEnd;

        while ((intervalEnd = intervalStart.AddDays(intervalSizeDays)) < rangeEnd)
        {
            var modifiedWorkingEndDate = overlapMitigation switch
            {
                OverlapMitigation.None => intervalEnd,
                OverlapMitigation.SubtractTick => intervalEnd.AddTicks(-1),
                OverlapMitigation.SubtractSecond => intervalEnd.AddSeconds(-1),
                OverlapMitigation.SubtractDay => intervalEnd.AddDays(-1),
                _ => throw new ArgumentException($"Invalid OverlapMitigation value of {overlapMitigation}.", nameof(overlapMitigation)),
            };

            yield return new DateTimeRange(intervalStart, modifiedWorkingEndDate);

            intervalStart = intervalEnd;
        }

        // The last range may not be a whole interval, so we end on the on the final endDate.
        yield return new DateTimeRange(intervalStart, rangeEnd);
    }
}
