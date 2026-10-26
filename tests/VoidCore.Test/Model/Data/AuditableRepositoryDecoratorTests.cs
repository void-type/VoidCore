using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using Xunit;

namespace VoidCore.Test.Model.Data;

public class AuditableRepositoryDecoratorTests
{
    private static readonly DateTimeOffset s_date = new(2001, 2, 12, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Add_entity_sets_auditable_created_and_updated_properties()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.Add(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(entity));

        var decorator = repoMock.AddAuditability(new FakeTimeProvider(s_date), GetUserAccessor());

        await decorator.Add(entity, default);

        Assert.Equal("userName", entity.CreatedBy);
        Assert.Equal(s_date, entity.CreatedOn);
        Assert.Equal("userName", entity.ModifiedBy);
        Assert.Equal(s_date, entity.ModifiedOn);

        await repoMock.Received(1).Add(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Add_entities_sets_auditable_created_and_updated_properties()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.AddRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decoratedRepo = repoMock.AddAuditability(new FakeTimeProvider(s_date), GetUserAccessor());

        await decoratedRepo.AddRange(entities, default);

        Assert.Equal("userName", entities[0].CreatedBy);
        Assert.Equal(s_date, entities[0].CreatedOn);
        Assert.Equal("userName", entities[0].ModifiedBy);
        Assert.Equal(s_date, entities[0].ModifiedOn);

        await repoMock.Received(1).AddRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_entity_sets_auditable_updated_properties()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.Update(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decoratedRepo = repoMock.AddAuditability(new FakeTimeProvider(s_date), GetUserAccessor());

        await decoratedRepo.Update(entity, default);

        Assert.Equal(default, entity.CreatedBy);
        Assert.Equal(default, entity.CreatedOn);
        Assert.Equal("userName", entity.ModifiedBy);
        Assert.Equal(s_date, entity.ModifiedOn);

        await repoMock.Received(1).Update(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_entities_sets_auditable_updated_properties()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.UpdateRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decoratedRepo = repoMock.AddAuditability(new FakeTimeProvider(s_date), GetUserAccessor());

        await decoratedRepo.UpdateRange(entities, default);

        Assert.Equal(default, entities[0].CreatedBy);
        Assert.Equal(default, entities[0].CreatedOn);
        Assert.Equal("userName", entities[0].ModifiedBy);
        Assert.Equal(s_date, entities[0].ModifiedOn);

        await repoMock.Received(1).UpdateRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Audit_times_are_recorded_in_utc_regardless_of_provider_time_zone()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.Add(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(entity));

        var timeProvider = new FakeTimeProvider(s_date);
        timeProvider.SetLocalTimeZone(TimeZoneInfo.CreateCustomTimeZone("Test", TimeSpan.FromHours(-7), "Test", "Test"));

        var decorator = repoMock.AddAuditability(timeProvider, GetUserAccessor());

        await decorator.Add(entity, default);

        Assert.Equal(TimeSpan.Zero, entity.CreatedOn.Offset);
        Assert.Equal(TimeSpan.Zero, entity.ModifiedOn.Offset);
    }

    private static ICurrentUserAccessor GetUserAccessor()
    {
        var currentUserAccessorMock = Substitute.For<ICurrentUserAccessor>();
        currentUserAccessorMock.GetUser()
            .Returns(new DomainUser("userName", Array.Empty<string>()));
        return currentUserAccessorMock;
    }

    public class TestEntity : IAuditable
    {
        public string CreatedBy { get; set; } = null!;
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTimeOffset ModifiedOn { get; set; }
    }
}
