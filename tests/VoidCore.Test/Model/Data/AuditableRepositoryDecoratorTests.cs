using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidCore.Model.Auth;
using VoidCore.Model.Data;
using VoidCore.Model.Time;
using Xunit;

namespace VoidCore.Test.Model.Data
{
    public class AuditableRepositoryDecoratorTests
    {
        [Fact]
        public async Task Add_entity_sets_auditable_created_and_updated_properties()
        {
            var entity = new TestEntity();

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.Add(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(entity));

            var date = new DateTime(2001, 2, 12);
            var dateTimeService = new DiscreteDateTimeService(date);

            var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
            currentUserAccessorMock.Setup(c => c.User)
                .Returns(new DomainUser("userName", Array.Empty<string>()));

            var decorator = repoMock.Object.AddAuditability(dateTimeService, currentUserAccessorMock.Object);

            await decorator.Add(entity, default);

            Assert.Equal("userName", entity.CreatedBy);
            Assert.Equal(date, entity.CreatedOn);
            Assert.Equal("userName", entity.ModifiedBy);
            Assert.Equal(date, entity.ModifiedOn);

            repoMock.Verify(r => r.Add(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Add_entities_sets_auditable_created_and_updated_properties()
        {
            var entities = new List<TestEntity>() { new TestEntity() };

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.AddRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var date = new DateTime(2001, 2, 12);
            var dateTimeService = new DiscreteDateTimeService(date);

            var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
            currentUserAccessorMock.Setup(c => c.User)
                .Returns(new DomainUser("userName", Array.Empty<string>()));

            var decoratedRepo = repoMock.Object.AddAuditability(dateTimeService, currentUserAccessorMock.Object);

            await decoratedRepo.AddRange(entities, default);

            Assert.Equal("userName", entities[0].CreatedBy);
            Assert.Equal(date, entities[0].CreatedOn);
            Assert.Equal("userName", entities[0].ModifiedBy);
            Assert.Equal(date, entities[0].ModifiedOn);

            repoMock.Verify(r => r.AddRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Update_entity_sets_auditable_updated_properties()
        {
            var entity = new TestEntity();

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.Update(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var date = new DateTime(2001, 2, 12);
            var dateTimeService = new DiscreteDateTimeService(date);

            var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
            currentUserAccessorMock.Setup(c => c.User)
                .Returns(new DomainUser("userName", Array.Empty<string>()));

            var decoratedRepo = repoMock.Object.AddAuditability(dateTimeService, currentUserAccessorMock.Object);

            await decoratedRepo.Update(entity, default);

            Assert.Equal(default, entity.CreatedBy);
            Assert.Equal(default, entity.CreatedOn);
            Assert.Equal("userName", entity.ModifiedBy);
            Assert.Equal(date, entity.ModifiedOn);

            repoMock.Verify(r => r.Update(It.IsAny<TestEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Update_entities_sets_auditable_updated_properties()
        {
            var entities = new List<TestEntity>() { new TestEntity() };

            var repoMock = new Mock<IWritableRepository<TestEntity>>();
            repoMock.Setup(r => r.UpdateRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var date = new DateTime(2001, 2, 12);
            var dateTimeService = new DiscreteDateTimeService(date);

            var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
            currentUserAccessorMock.Setup(c => c.User)
                .Returns(new DomainUser("userName", Array.Empty<string>()));

            var decoratedRepo = repoMock.Object.AddAuditability(dateTimeService, currentUserAccessorMock.Object);

            await decoratedRepo.UpdateRange(entities, default);

            Assert.Equal(default, entities[0].CreatedBy);
            Assert.Equal(default, entities[0].CreatedOn);
            Assert.Equal("userName", entities[0].ModifiedBy);
            Assert.Equal(date, entities[0].ModifiedOn);

            repoMock.Verify(r => r.UpdateRange(It.IsAny<List<TestEntity>>(), It.IsAny<CancellationToken>()), Times.Once);
            repoMock.VerifyNoOtherCalls();
        }

        public class TestEntity : IAuditable
        {
            public string CreatedBy { get; set; } = null;
            public DateTime CreatedOn { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime ModifiedOn { get; set; }
        }
    }
}
