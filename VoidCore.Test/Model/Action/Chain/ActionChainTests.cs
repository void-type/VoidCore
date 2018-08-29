using Moq;
using System;
using VoidCore.Model.Action.Chain;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Steps;
using Xunit;

namespace VoidCore.Test.Model.Action.Chain
{
    public class ActionChainTests
    {
        [Fact]
        public void CaptureException()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.WithError(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>()));

            var stepMock = new Mock<IActionStep>();
            stepMock.Setup(s => s.Perform(responderMock.Object)).Throws(new Exception("Test Exception"));

            new ActionChain(responderMock.Object).Execute(stepMock.Object);

            stepMock.Verify(s => s.Perform(responderMock.Object), Times.Once());
            responderMock.Verify(mock => mock.WithError(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void DontExecuteBecauseResponseCreated()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.IsResponseCreated).Returns(true);

            var stepMock = new Mock<IActionStep>();
            stepMock.Setup(mock => mock.Perform(responderMock.Object));

            var chain = new ActionChain(responderMock.Object);
            chain.Execute(stepMock.Object);

            stepMock.Verify(mock => mock.Perform(responderMock.Object), Times.Never());
        }

        [Fact]
        public void ExecuteStep()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.IsResponseCreated).Returns(false);

            var stepMock = new Mock<IActionStep>();
            stepMock.Setup(mock => mock.Perform(responderMock.Object));

            var chain = new ActionChain(responderMock.Object);
            chain.Execute(stepMock.Object);

            stepMock.Verify(mock => mock.Perform(responderMock.Object), Times.Once());
        }
    }
}
