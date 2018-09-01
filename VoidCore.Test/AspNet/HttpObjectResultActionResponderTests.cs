using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using VoidCore.AspNet.Action;
using VoidCore.Model.Action.Railway;
using VoidCore.Model.Action.Responses.File;
using VoidCore.Model.Action.Responses.ItemSet;
using VoidCore.Model.Action.Responses.UserMessage;
using VoidCore.Model.Logging;
using Xunit;

namespace VoidCore.Test.AspNet
{
    public class HttpObjectResultActionResponderTests
    {
        [Fact]
        public void RespondWithErrorObject()
        {
            var loggingMock = BuildLoggingMock();

            var responder = new HttpObjectResultActionResponder(loggingMock.Object);

            responder.WithError(new ErrorUserMessage("hi"));
            var objectResult = responder.Response as ObjectResult;

            loggingMock.Verify(logger => logger.Error(It.IsAny<Exception>(), It.IsAny<string[]>()), Times.Once());
            Assert.True(responder.IsResponseCreated);
            Assert.NotNull(objectResult);
            Assert.IsType<ErrorUserMessage>(objectResult.Value);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public void RespondWithErrorString()
        {
            var loggingMock = BuildLoggingMock();

            var responder = new HttpObjectResultActionResponder(loggingMock.Object);

            responder.WithError("hi");
            var objectResult = responder.Response as ObjectResult;

            loggingMock.Verify(logger => logger.Error(It.IsAny<Exception>(), It.IsAny<string[]>()), Times.Once());
            Assert.True(responder.IsResponseCreated);
            Assert.NotNull(objectResult);
            Assert.IsType<ErrorUserMessage>(objectResult.Value);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public void RespondWithSuccessFile()
        {
            var loggingMock = BuildLoggingMock();

            var responder = new HttpObjectResultActionResponder(loggingMock.Object);

            var file = new SimpleFile("content", "name");

            responder.WithSuccess(file);
            var objectResult = responder.Response as FileContentResult;

            loggingMock.Verify(logger => logger.Info(It.IsAny<string[]>()), Times.Once());
            Assert.True(responder.IsResponseCreated);
            Assert.NotNull(objectResult);
            Assert.Equal("application/force-download", objectResult.ContentType);
            Assert.Equal(file.Name, objectResult.FileDownloadName);
            Assert.Equal(file.Content, objectResult.FileContents);
        }

        [Fact]
        public void RespondWithSuccessObject()
        {
            var loggingMock = BuildLoggingMock();

            var responder = new HttpObjectResultActionResponder(loggingMock.Object);

            responder.WithSuccess(new SuccessUserMessage("hi"));
            var objectResult = responder.Response as ObjectResult;

            loggingMock.Verify(logger => logger.Info(It.IsAny<string[]>()), Times.Once());
            Assert.True(responder.IsResponseCreated);
            Assert.NotNull(objectResult);
            Assert.IsType<SuccessUserMessage>(objectResult.Value);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public void RespondWithWarningString()
        {
            var loggingMock = BuildLoggingMock();

            var responder = new HttpObjectResultActionResponder(loggingMock.Object);

            responder.WithWarning("hi");
            var objectResult = responder.Response as ObjectResult;

            loggingMock.Verify(logger => logger.Warn(It.IsAny<string[]>()), Times.Once());
            Assert.True(responder.IsResponseCreated);
            Assert.NotNull(objectResult);
            Assert.IsType<ItemSet<IFailure>>(objectResult.Value);

            var itemSet = objectResult.Value as ItemSet<IFailure>;

            Assert.NotNull(objectResult);
            Assert.NotNull(itemSet);
            Assert.Equal("hi", itemSet.Items.Single().ErrorMessage);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public void RespondWithWarningValidationErrors()
        {
            var loggingMock = BuildLoggingMock();

            var responder = new HttpObjectResultActionResponder(loggingMock.Object);

            responder.WithWarning(new ItemSet<IFailure>(new List<IFailure>
            {
                new Failure("hi", "field"),
                new Failure("hia", "field"),
                new Failure("his", "field"),
                new Failure("hie", "field"),
                new Failure("hif", "field"),
            }));
            var objectResult = responder.Response as ObjectResult;

            loggingMock.Verify(logger => logger.Warn(It.IsAny<string[]>()), Times.Once());
            Assert.True(responder.IsResponseCreated);
            Assert.NotNull(objectResult);
            Assert.IsType<ItemSet<IFailure>>(objectResult.Value);

            var itemSet = objectResult.Value as ItemSet<IFailure>;

            Assert.NotNull(objectResult);
            Assert.NotNull(itemSet);
            Assert.Equal("hi", itemSet.Items.First().ErrorMessage);
            Assert.Equal(5, itemSet.Count);
            Assert.Equal(400, objectResult.StatusCode);
        }

        private Mock<ILoggingService> BuildLoggingMock()
        {
            var loggingMock = new Mock<ILoggingService>();
            loggingMock.Setup(logger => logger.Error(It.IsAny<Exception>(), It.IsAny<string[]>()));
            loggingMock.Setup(logger => logger.Info(It.IsAny<string[]>()));
            loggingMock.Setup(logger => logger.Warn(It.IsAny<string[]>()));
            return loggingMock;
        }
    }
}
