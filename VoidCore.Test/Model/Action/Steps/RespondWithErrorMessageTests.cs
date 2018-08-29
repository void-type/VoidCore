using Moq;
using System;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.UserMessage;
using VoidCore.Model.Action.Steps;
using Xunit;

namespace VoidCore.Test.Model.Action.Steps
{
    public class RespondWithErrorMessageTests
    {
        [Fact]
        public void ResponderCalled()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.WithError(It.IsAny<ErrorUserMessage>(), It.IsAny<Exception>(), It.IsAny<string[]>()));

            new RespondWithErrorMessage("message", new Exception(), "log").Perform(responderMock.Object);

            responderMock.Verify(mock => mock.WithError(It.IsAny<ErrorUserMessage>(), It.IsAny<Exception>(), It.IsAny<string[]>()), Times.Once());
        }
    }
}
