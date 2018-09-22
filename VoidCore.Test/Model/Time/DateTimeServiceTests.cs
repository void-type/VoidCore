using System;
using VoidCore.Model.Time;
using Xunit;

namespace VoidCore.Test.Model.Time
{
    public class DateTimeServiceTests
    {
        [Fact]
        public void NowDateTimeServiceTest()
        {
            var service = new NowDateTimeService();

            var expectedStart = DateTime.Now;
            var actual = service.Moment;
            var expectedEnd = DateTime.Now;

            Assert.InRange(actual, expectedStart, expectedEnd);
            Assert.Equal(DateTimeKind.Local, actual.Kind);
        }

        [Fact]
        public void UtcNowDateTimeServiceTest()
        {
            var service = new UtcNowDateTimeService();

            var expectedStart = DateTime.UtcNow;
            var actual = service.Moment;
            var expectedEnd = DateTime.UtcNow;

            Assert.InRange(actual, expectedStart, expectedEnd);
            Assert.Equal(DateTimeKind.Utc, actual.Kind);
        }

        [Fact]
        public void DiscreteDateTimeServiceTest()
        {
            var service = new DiscreteDateTimeService(new DateTime(2000, 4, 2));

            var actual = service.Moment;

            Assert.Equal(new DateTime(2000, 4, 2), actual);
            Assert.Equal(DateTimeKind.Unspecified, actual.Kind);
        }
    }
}
