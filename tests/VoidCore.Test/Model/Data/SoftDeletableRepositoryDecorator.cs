using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using Xunit;

namespace VoidCore.Test.Model.Data;

public class SoftDeletableRepositoryDecorator
{
    private static readonly DateTimeOffset s_date = new(2001, 2, 12, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Soft_delete_entity_sets_DeletedOn_and_IsDeleted()
    {
        var entity = new TestEntity();

        var repoMock = GetRepoMock();

        var decoratedRepo = repoMock.AddSoftDeletability(new FakeTimeProvider(s_date), GetUserAccessor());

        await decoratedRepo.Remove(entity, default);

        Assert.Equal("userName", entity.DeletedBy);
        Assert.Equal(s_date, entity.DeletedOn);
        Assert.Equal(TimeSpan.Zero, entity.DeletedOn!.Value.Offset);
        Assert.True(entity.IsDeleted);

        await repoMock.Received(1).Update(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Soft_delete_entities_sets_DeletedOn_and_IsDeleted()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = GetRepoMock();

        var decoratedRepo = repoMock.AddSoftDeletability(new FakeTimeProvider(s_date), GetUserAccessor());

        await decoratedRepo.RemoveRange(entities, default);

        Assert.Equal("userName", entities[0].DeletedBy);
        Assert.Equal(s_date, entities[0].DeletedOn);
        Assert.True(entities[0].IsDeleted);

        await repoMock.Received(1).UpdateRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    private static IWritableRepository<TestEntity> GetRepoMock()
    {
        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.Remove(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        repoMock.RemoveRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        repoMock.Update(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        repoMock.UpdateRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        return repoMock;
    }

    private static ICurrentUserAccessor GetUserAccessor()
    {
        var currentUserAccessorMock = Substitute.For<ICurrentUserAccessor>();
        currentUserAccessorMock.GetUser()
            .Returns(new DomainUser("userName", Array.Empty<string>()));
        return currentUserAccessorMock;
    }

    public class TestEntity : ISoftDeletable
    {
        public string DeletedBy { get; set; } = string.Empty;
        public DateTimeOffset? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
