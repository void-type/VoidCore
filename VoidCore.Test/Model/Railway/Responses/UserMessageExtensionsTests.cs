using VoidCore.Model.Responses.Message;
using Xunit;

namespace VoidCore.Test.Model.Railway.Responses
{
    public class UserMessageExtensionsTests
    {
        [Fact]
        public void PostSuccessUserMessageGetLogTextWithIntId()
        {
            var logText = PostSuccessUserMessage.Create("Hi.", 2).GetLogText();
            var expected = new [] { "Message: Hi.", "EntityId: 2" };
            Assert.Equal(expected, logText);
        }

        [Fact]
        public void PostSuccessUserMessageGetLogTextWithStringId()
        {
            var logText = PostSuccessUserMessage.Create("Hi.", "ekdki23890lsdkalk").GetLogText();
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
