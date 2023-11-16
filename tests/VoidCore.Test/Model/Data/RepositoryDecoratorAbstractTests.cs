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
        repoMock.Add(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(entity));

        var decorator = new EmptyDecorator(repoMock);

        await decorator.Add(entity, default);

        await repoMock.Received(1).Add(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_adds_entities()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.AddRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.AddRange(entities, default);

        await repoMock.Received(1).AddRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_updates_entity()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.Update(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.Update(entity, default);

        await repoMock.Received(1).Update(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_updates_entities()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.UpdateRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.UpdateRange(entities, default);

        await repoMock.Received(1).UpdateRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_removes_entity()
    {
        var entity = new TestEntity();

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.Remove(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.Remove(entity, default);

        await repoMock.Received(1).Remove(Arg.Any<TestEntity>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RepositoryDecoratorAbstract_removes_entities()
    {
        var entities = new List<TestEntity>() { new() };

        var repoMock = Substitute.For<IWritableRepository<TestEntity>>();
        repoMock.RemoveRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var decorator = new EmptyDecorator(repoMock);

        await decorator.RemoveRange(entities, default);

        await repoMock.Received(1).RemoveRange(Arg.Any<List<TestEntity>>(), Arg.Any<CancellationToken>());
    }

    public class TestEntity { }

    public class EmptyDecorator : RepositoryDecoratorAbstract<TestEntity>
    {
        public EmptyDecorator(IWritableRepository<TestEntity> innerRepository) : base(innerRepository) { }
    }
}
