using VoidCore.Model.Action.Responses.Message;
using Xunit;

namespace VoidCore.Test.Model.Action.Responses
{
    public class UserMessageExtensionsTests
    {
        [Fact]
        public void PostSuccessUserMessageGetLogTextWithIntId()
        {
            var logText = new PostSuccessUserMessage<int>("Hi.", 2).GetLogText();
            var expected = new [] { "Message: Hi.", "EntityId: 2" };
            Assert.Equal(expected, logText);
        }

        [Fact]
        public void PostSuccessUserMessageGetLogTextWithStringId()
        {
            var logText = new PostSuccessUserMessage<string>("Hi.", "ekdki23890lsdkalk").GetLogText();
            var expected = new [] { "Message: Hi.", "EntityId: ekdki23890lsdkalk" };
            Assert.Equal(expected, logText);
        }

        [Fact]
        public void SuccessUserMessageGetLogText()
        {
            var logText = new UserMessage("Hi.").GetLogText();
            var expected = new [] { "Message: Hi." };
            Assert.Equal(expected, logText);
        }
    }
}
