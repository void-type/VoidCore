using VoidCore.Model.Time;
using Xunit;

namespace VoidCore.Test.Model.Time;

public class DateTimeServiceTests
{
    [Fact]
    public void Now_returns_now_in_local_time()
    {
        var service = new NowDateTimeService();

        var expectedStart = DateTime.Now;
        var actual = service.Moment;
        var expectedEnd = DateTime.Now;

        Assert.InRange(actual, expectedStart, expectedEnd);
        Assert.Equal(DateTimeKind.Local, actual.Kind);

        var expectedStartOffset = DateTimeOffset.Now;
        var actualOffset = service.MomentWithOffset;
        var expectedEndOffset = DateTimeOffset.Now;

        Assert.InRange(actualOffset, expectedStartOffset, expectedEndOffset);
    }

    [Fact]
    public void UtcNow_returns_now_in_utc()
    {
        var service = new UtcNowDateTimeService();

        var expectedStart = DateTime.UtcNow;
        var actual = service.Moment;
        var expectedEnd = DateTime.UtcNow;

        Assert.InRange(actual, expectedStart, expectedEnd);
        Assert.Equal(DateTimeKind.Utc, actual.Kind);

        var expectedStartOffset = DateTimeOffset.Now;
        var actualOffset = service.MomentWithOffset;
        var expectedEndOffset = DateTimeOffset.Now;

        Assert.InRange(actualOffset, expectedStartOffset, expectedEndOffset);
    }

    [Fact]
    public void Discrete_returns_the_specified_time()
    {
        var expected = new DateTime(2000, 4, 2, 0, 0, 0);
        var expectedOffset = new DateTimeOffset(expected);

        var service = new DiscreteDateTimeService(expected, expectedOffset);

        var actual = service.Moment;

        Assert.Equal(expected, actual);
        Assert.Equal(DateTimeKind.Unspecified, actual.Kind);

        var actualOffset = service.MomentWithOffset;

        Assert.Equal(expectedOffset, actualOffset);
    }
}
