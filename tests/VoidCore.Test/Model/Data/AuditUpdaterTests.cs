using Moq;
using System;
using VoidCore.Model.ClientApp;
using VoidCore.Model.Data;
using VoidCore.Model.Time;
using Xunit;

namespace VoidCore.Test.Model.Data
{
    public class AuditUpdaterTests
    {
        [Fact]
        public void CreateEntity()
        {
            var entity = new TestEntity();
            var date = new DateTime(2001, 2, 12);
            var dateTimeService = new DiscreteDateTimeService(date);

            var currentUserMock = new Mock<ICurrentUserAccessor>();
            currentUserMock.Setup(c => c.Name).Returns("userName");

            var auditUpdater = new AuditUpdater(dateTimeService, currentUserMock.Object);

            auditUpdater.Create(entity);

            Assert.Equal("userName", entity.CreatedBy);
            Assert.Equal(date, entity.CreatedOn);
            Assert.Equal("userName", entity.ModifiedBy);
            Assert.Equal(date, entity.ModifiedOn);
        }

        [Fact]
        public void UpdateEntity()
        {
            var entity = new TestEntity();
            var date = new DateTime(2001, 2, 12);
            var dateTimeService = new DiscreteDateTimeService(date);

            var currentUserMock = new Mock<ICurrentUserAccessor>();
            currentUserMock.Setup(c => c.Name).Returns("userName");

            var auditUpdater = new AuditUpdater(dateTimeService, currentUserMock.Object);

            auditUpdater.Update(entity);

            Assert.Null(entity.CreatedBy);
            Assert.Equal(default(DateTime), entity.CreatedOn);
            Assert.Equal("userName", entity.ModifiedBy);
            Assert.Equal(date, entity.ModifiedOn);
        }

        internal class TestEntity : IAuditable
        {
            public string CreatedBy { get; set; } = null;
            public DateTime CreatedOn { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime ModifiedOn { get; set; }
        }
    }
}
