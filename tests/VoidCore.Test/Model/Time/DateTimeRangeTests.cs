using System;
using System.Linq;
using VoidCore.Model.Time;
using Xunit;

namespace VoidCore.Test.Model.Time
{
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
        public void DateTimeRange_split_without_overlap()
        {
            var beforeCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 16))
                .SplitDateRangeIntoIntervals(7, OverlapMitigation.Day)
                .ToArray();

            Assert.Equal(2, beforeCutoff.Length);
            Assert.Equal(new DateTime(2000, 1, 1), beforeCutoff[0].StartDate);
            Assert.Equal(new DateTime(2000, 1, 8), beforeCutoff[0].EndDate);
            Assert.Equal(new DateTime(2000, 1, 9), beforeCutoff[1].StartDate);
            Assert.Equal(new DateTime(2000, 1, 16), beforeCutoff[1].EndDate);

            var afterCutoff = new DateTimeRange(new DateTime(2000, 1, 1), new DateTime(2000, 1, 17))
                .SplitDateRangeIntoIntervals(7, OverlapMitigation.Day)
                .ToArray();

            Assert.Equal(3, afterCutoff.Length);
            Assert.Equal(new DateTime(2000, 1, 1), afterCutoff[0].StartDate);
            Assert.Equal(new DateTime(2000, 1, 8), afterCutoff[0].EndDate);
            Assert.Equal(new DateTime(2000, 1, 9), afterCutoff[1].StartDate);
            Assert.Equal(new DateTime(2000, 1, 16), afterCutoff[1].EndDate);
            Assert.Equal(new DateTime(2000, 1, 17), afterCutoff[2].StartDate);
            Assert.Equal(new DateTime(2000, 1, 17), afterCutoff[2].EndDate);
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
    }
}
