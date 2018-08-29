using Moq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.UserMessage;
using VoidCore.Model.Action.Steps;
using Xunit;

namespace VoidCore.Test.Model.Action.Steps
{
    public class RespondWithSuccessMessageTests
    {
        [Fact]
        public void ResponderCalled()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.WithSuccess(It.IsAny<SuccessUserMessage>(), It.IsAny<string[]>()));

            new RespondWithSuccessMessage("message", "log").Perform(responderMock.Object);

            responderMock.Verify(mock => mock.WithSuccess(It.IsAny<SuccessUserMessage>(), It.IsAny<string[]>()), Times.Once());
        }
    }
}
