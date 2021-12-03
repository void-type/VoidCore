using System;
using System.Collections.Generic;
using VoidCore.Model.Guards;

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
        startDate.Ensure(s => s <= endDate, nameof(startDate), "startDate cannot be greater than endDate.");

        DateTime intervalEndDate;

        while ((intervalEndDate = startDate.AddDays(intervalSizeDays)) < endDate)
        {
            yield return new DateTimeRange(startDate, intervalEndDate);

            // Set the start date for the next range
            startDate = overlapMitigation switch
            {
                OverlapMitigation.Tick => intervalEndDate.AddTicks(1),
                OverlapMitigation.Second => intervalEndDate.AddSeconds(1),
                OverlapMitigation.Day => intervalEndDate.AddDays(1),
                _ => intervalEndDate,
            };
        }

        // The last range may not be a whole interval, so we end on the on the final endDate.
        yield return new DateTimeRange(startDate, endDate);
    }

    /// <summary>
    /// Split a larger date range into smaller intervals of a maximum size.
    /// </summary>
    /// <param name="range">The range</param>
    /// <param name="intervalSizeDays">How many days the interval should be</param>
    /// <param name="overlapMitigation">A strategy to mitigate overlap</param>
    public static IEnumerable<DateTimeRange> SplitDateRangeIntoIntervals(this DateTimeRange range, int intervalSizeDays, OverlapMitigation overlapMitigation = OverlapMitigation.None)
    {
        return SplitDateRangeIntoIntervals(range.StartDate, range.EndDate, intervalSizeDays, overlapMitigation);
    }
}
