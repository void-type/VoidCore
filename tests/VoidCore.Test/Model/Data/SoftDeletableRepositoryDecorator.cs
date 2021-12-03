using Moq;
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

        var repoMock = new Mock<IWritableRepository<TestEntity>>();
        repoMock.Setup(r => r.Remove(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        repoMock.Setup(r => r.RemoveRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        repoMock.Setup(r => r.Update(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        repoMock.Setup(r => r.UpdateRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var date = new DateTime(2001, 2, 12);
        var dateTimeService = new DiscreteDateTimeService(date);

        var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
        currentUserAccessorMock.Setup(c => c.User)
            .Returns(new DomainUser("userName", Array.Empty<string>()));

        var decoratedRepo = repoMock.Object.AddSoftDeletability(dateTimeService, currentUserAccessorMock.Object);

        await decoratedRepo.Remove(entity, default);

        Assert.Equal("userName", entity.DeletedBy);
        Assert.Equal(date, entity.DeletedOn);
        Assert.True(entity.IsDeleted);

        repoMock.Verify(r => r.Update(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Soft_delete_entities_sets_DeletedOn_and_IsDeleted()
    {
        var entities = new List<TestEntity>() { new TestEntity() };

        var repoMock = new Mock<IWritableRepository<TestEntity>>();
        repoMock.Setup(r => r.Remove(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        repoMock.Setup(r => r.RemoveRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        repoMock.Setup(r => r.Update(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        repoMock.Setup(r => r.UpdateRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var date = new DateTime(2001, 2, 12);
        var dateTimeService = new DiscreteDateTimeService(date);

        var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
        currentUserAccessorMock.Setup(c => c.User)
            .Returns(new DomainUser("userName", Array.Empty<string>()));

        var decoratedRepo = repoMock.Object.AddSoftDeletability(dateTimeService, currentUserAccessorMock.Object);

        await decoratedRepo.RemoveRange(entities, default);

        Assert.Equal("userName", entities[0].DeletedBy);
        Assert.Equal(date, entities[0].DeletedOn);
        Assert.True(entities[0].IsDeleted);

        repoMock.Verify(r => r.UpdateRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>()), Times.Once);
        repoMock.VerifyNoOtherCalls();
    }

    public class TestEntity : ISoftDeletable
    {
        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; } = null;
        public bool IsDeleted { get; set; }
    }
}
