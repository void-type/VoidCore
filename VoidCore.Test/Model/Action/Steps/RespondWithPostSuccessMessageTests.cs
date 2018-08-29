using Moq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.UserMessage;
using VoidCore.Model.Action.Steps;
using Xunit;

namespace VoidCore.Test.Model.Action.Steps
{
    public class RespondWithPostSuccessMessageTests
    {
        [Fact]
        public void ResponderCalled()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.WithSuccess(It.IsAny<PostSuccessUserMessage>(), It.IsAny<string[]>()));

            new RespondWithPostSuccessMessage("message", "1", "log").Perform(responderMock.Object);

            responderMock.Verify(mock => mock.WithSuccess(It.IsAny<PostSuccessUserMessage>(), It.IsAny<string[]>()), Times.Once());
        }
    }
}
