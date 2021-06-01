﻿using System;
using System.Collections.Generic;

namespace VoidCore.Model.Time
{
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
        /// <param name="overlap">Set to false if you don't want discrete dates to overlap</param>
        public static IEnumerable<DateTimeRange> SplitDateRangeIntoIntervals(DateTime startDate, DateTime endDate, int intervalSizeDays, bool overlap = true)
        {
            DateTime intervalEndDate;

            while ((intervalEndDate = startDate.AddDays(intervalSizeDays)) < endDate)
            {
                yield return new DateTimeRange(StartDate: startDate, EndDate: intervalEndDate);
                startDate = overlap ? intervalEndDate : intervalEndDate.AddDays(1);
            }

            yield return new DateTimeRange(StartDate: startDate, EndDate: endDate);
        }

        /// <summary>
        /// Split a larger date range into smaller intervals of a maximum size.
        /// </summary>
        /// <param name="range">The range</param>
        /// <param name="intervalSizeDays">How many days the interval should be</param>
        /// <param name="overlap">Set to false if you don't want discrete dates to overlap</param>
        public static IEnumerable<DateTimeRange> SplitDateRangeIntoIntervals(this DateTimeRange range, int intervalSizeDays, bool overlap = true)
        {
            return SplitDateRangeIntoIntervals(range.StartDate, range.EndDate, intervalSizeDays, overlap);
        }
    }
}
