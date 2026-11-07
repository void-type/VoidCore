using VoidCore.Model.Time;
using Xunit;

namespace VoidCore.Test.Model.Time;

public class DateTimeRangeTests
{
    [Fact]
    public void DateTimeRange_split_with_overlap()
    {
        var beforeCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 15))
            .SplitDateRangeIntoIntervals(7)
            .ToArray();

        Assert.Equal(2, beforeCutoff.Length);
        Assert.Equal(new DateTime(2000, 1, 1), beforeCutoff[0].StartDate);
        Assert.Equal(new DateTime(2000, 1, 8), beforeCutoff[0].EndDate);

        Assert.Equal(new DateTime(2000, 1, 8), beforeCutoff[1].StartDate);
        Assert.Equal(new DateTime(2000, 1, 15), beforeCutoff[1].EndDate);

        var afterCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 16))
            .SplitDateRangeIntoIntervals(7)
            .ToArray();

        Assert.Equal(3, afterCutoff.Length);
        Assert.Equal(new DateTime(2000, 1, 1), afterCutoff[0].StartDate);
        Assert.Equal(new DateTime(2000, 1, 8), afterCutoff[0].EndDate);

        Assert.Equal(new DateTime(2000, 1, 8), afterCutoff[1].StartDate);
        Assert.Equal(new DateTime(2000, 1, 15), afterCutoff[1].EndDate);

        Assert.Equal(new DateTime(2000, 1, 15), afterCutoff[2].StartDate);
        Assert.Equal(new DateTime(2000, 1, 16), afterCutoff[2].EndDate);
    }

    [Fact]
    public void DateTimeRange_split_without_overlap_tick_strategy()
    {
        var beforeCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 15))
            .SplitDateRangeIntoIntervals(7, OverlapMitigation.SubtractTick)
            .ToArray();

        Assert.Equal(2, beforeCutoff.Length);
        Assert.Equal(new DateTime(2000, 1, 1), beforeCutoff[0].StartDate);
        Assert.Equal(new DateTime(2000, 1, 8).AddTicks(-1), beforeCutoff[0].EndDate);

        Assert.Equal(new DateTime(2000, 1, 8), beforeCutoff[1].StartDate);
        Assert.Equal(new DateTime(2000, 1, 15), beforeCutoff[1].EndDate);

        var afterCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 16))
            .SplitDateRangeIntoIntervals(7, OverlapMitigation.SubtractTick)
            .ToArray();

        Assert.Equal(3, afterCutoff.Length);
        Assert.Equal(new DateTime(2000, 1, 1), afterCutoff[0].StartDate);
        Assert.Equal(new DateTime(2000, 1, 8).AddTicks(-1), afterCutoff[0].EndDate);

        Assert.Equal(new DateTime(2000, 1, 8), afterCutoff[1].StartDate);
        Assert.Equal(new DateTime(2000, 1, 15).AddTicks(-1), afterCutoff[1].EndDate);

        Assert.Equal(new DateTime(2000, 1, 15), afterCutoff[2].StartDate);
        Assert.Equal(new DateTime(2000, 1, 16), afterCutoff[2].EndDate);
    }

    [Fact]
    public void DateTimeRange_split_without_overlap_second_strategy()
    {
        var beforeCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 15))
            .SplitDateRangeIntoIntervals(7, OverlapMitigation.SubtractSecond)
            .ToArray();

        Assert.Equal(2, beforeCutoff.Length);
        Assert.Equal(new DateTime(2000, 1, 1), beforeCutoff[0].StartDate);
        Assert.Equal(new DateTime(2000, 1, 8).AddSeconds(-1), beforeCutoff[0].EndDate);

        Assert.Equal(new DateTime(2000, 1, 8), beforeCutoff[1].StartDate);
        Assert.Equal(new DateTime(2000, 1, 15), beforeCutoff[1].EndDate);

        var afterCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 16))
            .SplitDateRangeIntoIntervals(7, OverlapMitigation.SubtractSecond)
            .ToArray();

        Assert.Equal(3, afterCutoff.Length);
        Assert.Equal(new DateTime(2000, 1, 1), afterCutoff[0].StartDate);
        Assert.Equal(new DateTime(2000, 1, 8).AddSeconds(-1), afterCutoff[0].EndDate);

        Assert.Equal(new DateTime(2000, 1, 8), afterCutoff[1].StartDate);
        Assert.Equal(new DateTime(2000, 1, 15).AddSeconds(-1), afterCutoff[1].EndDate);

        Assert.Equal(new DateTime(2000, 1, 15), afterCutoff[2].StartDate);
        Assert.Equal(new DateTime(2000, 1, 16), afterCutoff[2].EndDate);
    }

    [Fact]
    public void DateTimeRange_split_without_overlap_day_strategy()
    {
        var beforeCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 15))
            .SplitDateRangeIntoIntervals(7, OverlapMitigation.SubtractDay)
            .ToArray();

        Assert.Equal(2, beforeCutoff.Length);
        Assert.Equal(new DateTime(2000, 1, 1), beforeCutoff[0].StartDate);
        Assert.Equal(new DateTime(2000, 1, 8).AddDays(-1), beforeCutoff[0].EndDate);

        Assert.Equal(new DateTime(2000, 1, 8), beforeCutoff[1].StartDate);
        Assert.Equal(new DateTime(2000, 1, 15), beforeCutoff[1].EndDate);

        var afterCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 16))
            .SplitDateRangeIntoIntervals(7, OverlapMitigation.SubtractDay)
            .ToArray();

        Assert.Equal(3, afterCutoff.Length);
        Assert.Equal(new DateTime(2000, 1, 1), afterCutoff[0].StartDate);
        Assert.Equal(new DateTime(2000, 1, 8).AddDays(-1), afterCutoff[0].EndDate);

        Assert.Equal(new DateTime(2000, 1, 8), afterCutoff[1].StartDate);
        Assert.Equal(new DateTime(2000, 1, 15).AddDays(-1), afterCutoff[1].EndDate);

        Assert.Equal(new DateTime(2000, 1, 15), afterCutoff[2].StartDate);
        Assert.Equal(new DateTime(2000, 1, 16), afterCutoff[2].EndDate);
    }

    [Fact]
    public void DateTimeRange_throws_on_invalid_range()
    {
        Assert.Throws<ArgumentException>(() => new DateTimeRange(new DateTime(2000, 1, 18), new DateTime(2000, 1, 16)));
    }

    [Fact]
    public void DateTimeRange_split_throws_on_invalid_range()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            DateTimeHelpers.SplitDateRangeIntoIntervals(new DateTime(2000, 1, 18), new DateTime(2000, 1, 16), 7).ToList();
        });
    }

    [Fact]
    public void DateTimeRange_split_throws_on_invalid_overlap_mitigation_without_enumerating()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            DateTimeHelpers.SplitDateRangeIntoIntervals(new DateTime(2000, 1, 1), new DateTime(2000, 1, 16), 7, (OverlapMitigation)22);
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-7)]
    public void DateTimeRange_split_throws_on_interval_less_than_one_day_instead_of_looping_forever(int intervalSizeDays)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            DateTimeHelpers.SplitDateRangeIntoIntervals(new DateTime(2000, 1, 1), new DateTime(2000, 1, 16), intervalSizeDays);
        });
    }

    [Fact]
    public void DateTimeRange_split_handles_ranges_that_end_at_the_maximum_date()
    {
        var ranges = DateTimeHelpers.SplitDateRangeIntoIntervals(DateTime.MaxValue.AddDays(-10), DateTime.MaxValue, 7).ToList();

        Assert.Equal(2, ranges.Count);
        Assert.Equal(DateTime.MaxValue, ranges[^1].EndDate);
    }

    [Fact]
    public void DateTimeRange_split_handles_huge_interval_sizes()
    {
        var ranges = DateTimeHelpers.SplitDateRangeIntoIntervals(DateTime.MinValue, DateTime.MaxValue, int.MaxValue).ToList();

        Assert.Single(ranges);
        Assert.Equal(DateTime.MinValue, ranges[0].StartDate);
        Assert.Equal(DateTime.MaxValue, ranges[0].EndDate);
    }

    [Fact]
    public void DateTimeRange_split_returns_range_when_interval_larger()
    {
        var ranges = DateTimeHelpers.SplitDateRangeIntoIntervals(new DateTime(2000, 1, 1), new DateTime(2000, 1, 15), 20).ToList();

        Assert.Single(ranges);
        Assert.Equal(new DateTime(2000, 1, 1), ranges.First().StartDate);
        Assert.Equal(new DateTime(2000, 1, 15), ranges.First().EndDate);
    }
}
