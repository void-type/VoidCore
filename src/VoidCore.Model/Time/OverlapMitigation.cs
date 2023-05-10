namespace VoidCore.Model.Time;

/// <summary>
/// Time splitting overlap mitigation strategy.
/// </summary>
public enum OverlapMitigation
{
    /// <summary>
    /// None - Don't mitigate overlap. Useful if the end of one range should be the start of the next range.
    /// Use <![CDATA[ startDate <= x < endDate ]]>.
    /// For example: 7 day intervals from 14 days = 1:00:00:00-8:00:00:00, 8:00:00:00-15:00:00:00.
    /// </summary>
    None = 0,

    /// <summary>
    /// Tick - Subtract a tick from the range's end. Useful for non-overlapping ranges that use date and time with tick precision.
    /// Use <![CDATA[ startDate <= x <= endDate ]]>.
    /// For example: 7 day intervals from 14 days = 1:00:00:00-7:23:59:59.9999..., 8:00:00:00-15:00:00:00.
    /// </summary>
    SubtractTick = 1,

    /// <summary>
    /// Second - Subtract a second from the range's end. Useful for non-overlapping ranges that use date and time with second precision.
    /// Use <![CDATA[ startDate <= x <= endDate ]]>.
    /// For example: 7 day intervals from 14 days = 1:00:00:00-7:23:59:59, 8:00:00:00-15:00:00:00.
    /// </summary>
    SubtractSecond = 2,

    /// <summary>
    /// Day - Subtract a day from the range's end. Useful for non-overlapping ranges that use only use date. It is implied the interval goes to the end of the given day.
    /// Use <![CDATA[ startDate.Date <= x.Date <= endDate.Date ]]>.
    /// For example: 7 day intervals from 14 days = 1:00:00:00-7:00:00:00, 8:00:00:00-15:00:00:00.
    /// </summary>
    SubtractDay = 3
}
