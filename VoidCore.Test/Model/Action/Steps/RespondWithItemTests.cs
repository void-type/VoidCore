using Moq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Steps;
using Xunit;

namespace VoidCore.Test.Model.Action.Steps
{
    public class RespondWithItemTests
    {
        [Fact]
        public void ResponderCalled()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.WithSuccess(It.IsAny<string>(), It.IsAny<string[]>()));

            new RespondWithItem<string>("1", "log").Perform(responderMock.Object);

            responderMock.Verify(mock => mock.WithSuccess(It.IsAny<string>(), It.IsAny<string[]>()), Times.Once());
        }
    }
}
