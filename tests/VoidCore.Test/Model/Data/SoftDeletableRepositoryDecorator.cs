using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using VoidCore.Model.Time;
using Xunit;

namespace VoidCore.Test.Model.Data;

public class SoftDeletableRepositoryDecorator
{
    [Fact]
    public async Task Soft_delete_entity_sets_DeletedOn_and_IsDeleted()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.Remove(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        repoMock.RemoveRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        repoMock.Update(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        repoMock.UpdateRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var date = new DateTime(2001, 2, 12);
        var dateTimeService = new DiscreteDateTimeService(date);

        var currentUserAccessorMock = Substitute.For<ICurrentUserAccessor>();
        currentUserAccessorMock.User
            .Returns(new DomainUser("userName", Array.Empty<string>()));

        var decoratedRepo = repoMock.AddSoftDeletability(dateTimeService, currentUserAccessorMock);

        await decoratedRepo.Remove(entity, default);

        Assert.Equal("userName", entity.DeletedBy);
        Assert.Equal(date, entity.DeletedOn);
        Assert.True(entity.IsDeleted);

        await repoMock.Received(1).Update(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Soft_delete_entities_sets_DeletedOn_and_IsDeleted()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.Remove(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        repoMock.RemoveRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        repoMock.Update(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        repoMock.UpdateRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var date = new DateTime(2001, 2, 12);
        var dateTimeService = new DiscreteDateTimeService(date);

        var currentUserAccessorMock = Substitute.For<ICurrentUserAccessor>();
        currentUserAccessorMock.User
            .Returns(new DomainUser("userName", Array.Empty<string>()));

        var decoratedRepo = repoMock.AddSoftDeletability(dateTimeService, currentUserAccessorMock);

        await decoratedRepo.RemoveRange(entities, default);

        Assert.Equal("userName", entities[0].DeletedBy);
        Assert.Equal(date, entities[0].DeletedOn);
        Assert.True(entities[0].IsDeleted);

        await repoMock.Received(1).UpdateRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    public class TestEntity : ISoftDeletable
    {
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; } = null;
        public bool IsDeleted { get; set; }
    }
}
