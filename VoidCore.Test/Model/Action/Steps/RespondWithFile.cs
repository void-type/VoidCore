using Moq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.File;
using VoidCore.Model.Action.Steps;
using Xunit;

namespace VoidCore.Test.Model.Action.Steps
{
    /// <summary>
    /// Response with a collection of items.
    /// </summary>
    public class RespondWithFileTest
    {
        [Fact]
        public void ResponderCalled()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.WithSuccess(It.IsAny<ISimpleFile>(), It.IsAny<string[]>()));

            new RespondWithFile(new SimpleFile("kdkd", "dkdk"), "log").Perform(responderMock.Object);

            responderMock.Verify(mock => mock.WithSuccess(It.IsAny<ISimpleFile>(), It.IsAny<string[]>()), Times.Once());
        }
    }
}
