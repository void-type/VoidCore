using Moq;
using System.Collections.Generic;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.ItemSet;
using VoidCore.Model.Action.Steps;
using Xunit;

namespace VoidCore.Test.Model.Action.Steps
{
    public class RespondWithItemSetPageTests
    {
        [Fact]
        public void ResponderCalled()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.WithSuccess(It.IsAny<ItemSetPage<string>>(), It.IsAny<string[]>()));

            new RespondWithItemSetPage<string>(new List<string> { "1", "2" }, 1, 2, "log").Perform(responderMock.Object);

            responderMock.Verify(mock => mock.WithSuccess(It.IsAny<ItemSetPage<string>>(), It.IsAny<string[]>()), Times.Once());
        }
    }
}
