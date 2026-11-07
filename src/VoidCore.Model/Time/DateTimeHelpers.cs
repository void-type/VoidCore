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
    /// <param name="intervalSizeDays">How many days the interval should be. Must be at least 1.</param>
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
    /// <param name="intervalSizeDays">How many days the interval should be. Must be at least 1.</param>
    /// <param name="overlapMitigation">A strategy to mitigate overlap</param>
    /// <exception cref="ArgumentOutOfRangeException">The interval size is less than 1 day or the overlap mitigation isn't a defined value</exception>
    public static IEnumerable<DateTimeRange> SplitDateRangeIntoIntervals(this DateTimeRange range, int intervalSizeDays, OverlapMitigation overlapMitigation = OverlapMitigation.None)
    {
        range.EnsureNotNull();
        ArgumentOutOfRangeException.ThrowIfLessThan(intervalSizeDays, 1);

        if (!Enum.IsDefined(overlapMitigation))
        {
            throw new ArgumentOutOfRangeException(nameof(overlapMitigation), overlapMitigation, $"Invalid OverlapMitigation value of {overlapMitigation}.");
        }

        return SplitIntoIntervals(range, intervalSizeDays, overlapMitigation);
    }

    // TimeSpan.FromDays throws above this many days. Any larger interval is already longer than every possible range.
    private const int MaxIntervalDays = 10_675_199;

    private static IEnumerable<DateTimeRange> SplitIntoIntervals(DateTimeRange range, int intervalSizeDays, OverlapMitigation overlapMitigation)
    {
        var rangeEnd = range.EndDate;
        var interval = TimeSpan.FromDays(Math.Min(intervalSizeDays, MaxIntervalDays));

        var intervalStart = range.StartDate;

        // Comparing the time remaining, instead of adding to the start, can't overflow near DateTime.MaxValue.
        while (rangeEnd - intervalStart > interval)
        {
            var intervalEnd = intervalStart + interval;

            var modifiedWorkingEndDate = overlapMitigation switch
            {
                OverlapMitigation.None => intervalEnd,
                OverlapMitigation.SubtractTick => intervalEnd.AddTicks(-1),
                OverlapMitigation.SubtractSecond => intervalEnd.AddSeconds(-1),
                _ => intervalEnd.AddDays(-1),
            };

            yield return new DateTimeRange(intervalStart, modifiedWorkingEndDate);

            intervalStart = intervalEnd;
        }

        // The last range may not be a whole interval, so we end on the on the final endDate.
        yield return new DateTimeRange(intervalStart, rangeEnd);
    }
}
