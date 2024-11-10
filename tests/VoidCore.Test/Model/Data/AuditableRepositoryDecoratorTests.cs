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

public class AuditableRepositoryDecoratorTests
{
    [Fact]
    public async Task Add_entity_sets_auditable_created_and_updated_properties()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.AddAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(entity));

        var date = new DateTime(2001, 2, 12);
        var dateTimeService = new DiscreteDateTimeService(date);

        var currentUserAccessorMock = Substitute.For<ICurrentUserAccessor>();
        currentUserAccessorMock.User
            .Returns(new DomainUser("userName", Array.Empty<string>()));

        var decorator = repoMock.AddAuditability(dateTimeService, currentUserAccessorMock);

        await decorator.AddAsync(entity, default);

        Assert.Equal("userName", entity.CreatedBy);
        Assert.Equal(date, entity.CreatedOn);
        Assert.Equal("userName", entity.ModifiedBy);
        Assert.Equal(date, entity.ModifiedOn);

        await repoMock.Received(1).AddAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Add_entities_sets_auditable_created_and_updated_properties()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.AddRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var date = new DateTime(2001, 2, 12);
        var dateTimeService = new DiscreteDateTimeService(date);

        var currentUserAccessorMock = Substitute.For<ICurrentUserAccessor>();
        currentUserAccessorMock.User
            .Returns(new DomainUser("userName", Array.Empty<string>()));

        var decoratedRepo = repoMock.AddAuditability(dateTimeService, currentUserAccessorMock);

        await decoratedRepo.AddRangeAsync(entities, default);

        Assert.Equal("userName", entities[0].CreatedBy);
        Assert.Equal(date, entities[0].CreatedOn);
        Assert.Equal("userName", entities[0].ModifiedBy);
        Assert.Equal(date, entities[0].ModifiedOn);

        await repoMock.Received(1).AddRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_entity_sets_auditable_updated_properties()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.UpdateAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var date = new DateTime(2001, 2, 12);
        var dateTimeService = new DiscreteDateTimeService(date);

        var currentUserAccessorMock = Substitute.For<ICurrentUserAccessor>();
        currentUserAccessorMock.User
            .Returns(new DomainUser("userName", Array.Empty<string>()));

        var decoratedRepo = repoMock.AddAuditability(dateTimeService, currentUserAccessorMock);

        await decoratedRepo.UpdateAsync(entity, default);

        Assert.Equal(default, entity.CreatedBy);
        Assert.Equal(default, entity.CreatedOn);
        Assert.Equal("userName", entity.ModifiedBy);
        Assert.Equal(date, entity.ModifiedOn);

        await repoMock.Received(1).UpdateAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Update_entities_sets_auditable_updated_properties()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.UpdateRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var date = new DateTime(2001, 2, 12);
        var dateTimeService = new DiscreteDateTimeService(date);

        var currentUserAccessorMock = Substitute.For<ICurrentUserAccessor>();
        currentUserAccessorMock.User
            .Returns(new DomainUser("userName", Array.Empty<string>()));

        var decoratedRepo = repoMock.AddAuditability(dateTimeService, currentUserAccessorMock);

        await decoratedRepo.UpdateRangeAsync(entities, default);

        Assert.Equal(default, entities[0].CreatedBy);
        Assert.Equal(default, entities[0].CreatedOn);
        Assert.Equal("userName", entities[0].ModifiedBy);
        Assert.Equal(date, entities[0].ModifiedOn);

        await repoMock.Received(1).UpdateRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    public class TestEntity : IAuditable
    {
        public string CreatedBy { get; set; } = null;
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
