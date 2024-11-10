using NSubstitute;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Data;
using Xunit;

namespace VoidCore.Test.Model.Data;

public class RepositoryDecoratorAbstractTests
{
    [Fact]
    public async Task RepositoryDecoratorAbstract_adds_entity()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.AddAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(entity));

        var decorator = new EmptyDecorator(repoMock);

        await decorator.AddAsync(entity, default);

        await repoMock.Received(1).AddAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_adds_entities()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.AddRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.AddRangeAsync(entities, default);

        await repoMock.Received(1).AddRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_updates_entity()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.UpdateAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.UpdateAsync(entity, default);

        await repoMock.Received(1).UpdateAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_updates_entities()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.UpdateRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.UpdateRangeAsync(entities, default);

        await repoMock.Received(1).UpdateRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_removes_entity()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.RemoveAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.RemoveAsync(entity, default);

        await repoMock.Received(1).RemoveAsync(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_removes_entities()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.RemoveRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.RemoveRangeAsync(entities, default);

        await repoMock.Received(1).RemoveRangeAsync(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    public class TestEntity { }

    public class EmptyDecorator : RepositoryDecoratorAbstract<TestEntity>
    {
        public EmptyDecorator(IWritableRepository<TestEntity> innerRepository) : base(innerRepository) { }
    }
}
