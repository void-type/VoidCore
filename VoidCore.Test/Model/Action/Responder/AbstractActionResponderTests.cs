using Xunit;

namespace VoidCore.Test.Model.Action.Responder
{
    public class AbstractActionResponderTests
    {
        [Fact]
        public void IsResponseCreatedFalseWhenResponseDefault()
        {
            var responder = new TestableActionResponderStub();

            responder.SetResponse(null);

            Assert.False(responder.IsResponseCreated);
        }

        [Fact]
        public void IsResponseCreatedTrueWhenResponseNotDefault()
        {
            var responder = new TestableActionResponderStub();

            responder.SetResponse("");

            Assert.True(responder.IsResponseCreated);
        }
    }
}