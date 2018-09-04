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
            string callbackCreatedBy = null;
            DateTime? callbackCreatedOn = null;
            string callbackModifiedBy = null;
            DateTime? callbackModifiedOn = null;

            var entityMock = new Mock<IAuditable>();
            entityMock.SetupSet(e => e.CreatedBy = It.IsAny<string>()).Callback<string>(value => callbackCreatedBy = value);
            entityMock.SetupSet(e => e.CreatedOn = It.IsAny<DateTime>()).Callback<DateTime>(value => callbackCreatedOn = value);
            entityMock.SetupSet(e => e.ModifiedBy = It.IsAny<string>()).Callback<string>(value => callbackModifiedBy = value);
            entityMock.SetupSet(e => e.ModifiedOn = It.IsAny<DateTime>()).Callback<DateTime>(value => callbackModifiedOn = value);

            var date = new DateTime(2001, 2, 12);
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            dateTimeServiceMock.Setup(d => d.Moment).Returns(date);

            var currentUserMock = new Mock<ICurrentUser>();
            currentUserMock.Setup(c => c.Name).Returns("userName");

            var auditUpdater = new AuditUpdater(dateTimeServiceMock.Object, currentUserMock.Object);

            auditUpdater.Create(entityMock.Object);

            Assert.Equal("userName", callbackCreatedBy);
            Assert.Equal(date, callbackCreatedOn);
            Assert.Equal("userName", callbackModifiedBy);
            Assert.Equal(date, callbackModifiedOn);
        }

        [Fact]
        public void UpdateEntity()
        {
            string callbackCreatedBy = null;
            DateTime? callbackCreatedOn = null;
            string callbackModifiedBy = null;
            DateTime? callbackModifiedOn = null;

            var entityMock = new Mock<IAuditable>();
            entityMock.SetupSet(e => e.CreatedBy = It.IsAny<string>()).Callback<string>(value => callbackCreatedBy = value);
            entityMock.SetupSet(e => e.CreatedOn = It.IsAny<DateTime>()).Callback<DateTime>(value => callbackCreatedOn = value);
            entityMock.SetupSet(e => e.ModifiedBy = It.IsAny<string>()).Callback<string>(value => callbackModifiedBy = value);
            entityMock.SetupSet(e => e.ModifiedOn = It.IsAny<DateTime>()).Callback<DateTime>(value => callbackModifiedOn = value);

            var date = new DateTime(2001, 2, 12);
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            dateTimeServiceMock.Setup(d => d.Moment).Returns(date);

            var currentUserMock = new Mock<ICurrentUser>();
            currentUserMock.Setup(c => c.Name).Returns("userName");

            var auditUpdater = new AuditUpdater(dateTimeServiceMock.Object, currentUserMock.Object);

            auditUpdater.Update(entityMock.Object);

            Assert.Null(callbackCreatedBy);
            Assert.Null(callbackCreatedOn);
            Assert.Equal("userName", callbackModifiedBy);
            Assert.Equal(date, callbackModifiedOn);
        }
    }
}
