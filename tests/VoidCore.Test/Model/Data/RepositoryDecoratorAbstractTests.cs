using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Data;
using Xunit;

namespace VoidCore.Test.Model.Data
{
    public class RepositoryDecoratorAbstractTests
    {
        [Fact]
        public async Task AddEntity()
        {
            var entity = new TestEntity();

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.Add(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(entity));

            var decorator = new EmptyDecorator(repoMock.Object);

            await decorator.Add(entity);

            repoMock.Verify(r => r.Add(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddEntities()
        {
            var entities = new List<TestEntity>() { new TestEntity() };

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.AddRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var decorator = new EmptyDecorator(repoMock.Object);

            await decorator.AddRange(entities);

            repoMock.Verify(r => r.AddRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateEntity()
        {
            var entity = new TestEntity();

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.Update(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var decorator = new EmptyDecorator(repoMock.Object);

            await decorator.Update(entity);

            repoMock.Verify(r => r.Update(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateEntities()
        {
            var entities = new List<TestEntity>() { new TestEntity() };

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.UpdateRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var decorator = new EmptyDecorator(repoMock.Object);

            await decorator.UpdateRange(entities);

            repoMock.Verify(r => r.UpdateRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task RemoveEntity()
        {
            var entity = new TestEntity();

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.Remove(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var decorator = new EmptyDecorator(repoMock.Object);

            await decorator.Remove(entity);

            repoMock.Verify(r => r.Remove(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task RemoveEntities()
        {
            var entities = new List<TestEntity>() { new TestEntity() };

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.RemoveRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var decorator = new EmptyDecorator(repoMock.Object);

            await decorator.RemoveRange(entities);

            repoMock.Verify(r => r.RemoveRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        public class TestEntity { }

        public class EmptyDecorator : RepositoryDecoratorAbstract<TestEntity>
        {
            public EmptyDecorator(IWritableRepository<TestEntity> innerRepository) : base(innerRepository) { }
        }
    }
}
