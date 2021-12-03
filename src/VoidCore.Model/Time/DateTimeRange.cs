using System;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Time;

/// <summary>
/// Represents a range of time.
/// </summary>
public record DateTimeRange
{
    /// <summary>
    /// Construct a new DateTimeRange
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    public DateTimeRange(DateTime startDate, DateTime endDate)
    {
        startDate.Ensure(s => s <= endDate, nameof(startDate), "startDate cannot be greater than endDate.");
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// The start date
    /// </summary>
    public DateTime StartDate { get; init; }

    /// <summary>
    /// The end date
    /// </summary>
    public DateTime EndDate { get; init; }
}
