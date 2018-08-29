using Moq;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Steps;
using VoidCore.Model.Data;
using Xunit;

namespace VoidCore.Test.Model.Action.Steps
{
    public class SaveChangesToDataTests
    {
        [Fact]
        public void ResponderCalled()
        {
            var responderMock = new Mock<IActionResponder>();
            var dataMock = new Mock<IPersistable>();
            dataMock.Setup(mock => mock.SaveChanges());

            new SaveChangesToData(dataMock.Object).Perform(responderMock.Object);

            dataMock.Verify(mock => mock.SaveChanges(), Times.Once());
        }
    }
}
