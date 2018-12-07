using Moq;
using System;
using VoidCore.Model.Data;
using VoidCore.Model.Time;
using VoidCore.Model.Users;
using Xunit;

namespace VoidCore.Test.Model.Data
{
    public class SoftDeleteUpdaterTests
    {
        [Fact]
        public void DeleteAndUnDeleteEntity()
        {
            var entity = new TestEntity();
            var date = new DateTime(2001, 2, 12);
            var dateTimeService = new DiscreteDateTimeService(date);

            var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
            currentUserAccessorMock.Setup(c => c.User)
                .Returns(new DomainUser("userName", new string[] { }));

            var auditUpdater = new SoftDeleteUpdater(dateTimeService, currentUserAccessorMock.Object);

            auditUpdater.Delete(entity);

            Assert.Equal("userName", entity.DeletedBy);
            Assert.Equal(date, entity.DeletedOn);

            auditUpdater.UnDelete(entity);

            Assert.Null(entity.DeletedBy);
            Assert.False(entity.DeletedOn.HasValue);
        }

        internal class TestEntity : ISoftDeletable
        {
            public string DeletedBy { get; set; }
            public DateTime? DeletedOn { get; set; } = null;
        }
    }
}
