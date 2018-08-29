using VoidCore.Model.Action.Responses.UserMessage;
using Xunit;

namespace VoidCore.Test.Model.Action.Responses.UserMessage
{
    public class UserMessageExtensionsTests
    {
        [Fact]
        public void ErrorUserMessageGetLogText()
        {
            var logText = new ErrorUserMessage("Hi.").GetLogText();
            var expected = new [] { "ErrorMessage: Hi." };
            Assert.Equal(expected, logText);
        }

        [Fact]
        public void PostSuccessUserMessageGetLogTextWithIntId()
        {
            var logText = new PostSuccessUserMessage("Hi.", "ekdki23890lsdkalk").GetLogText();
            var expected = new [] { "SuccessMessage: Hi.", "EntityId: ekdki23890lsdkalk" };
            Assert.Equal(expected, logText);
        }

        [Fact]
        public void PostSuccessUserMessageGetLogTextWithStringId()
        {
            var logText = new PostSuccessUserMessage("Hi.", 2).GetLogText();
            var expected = new [] { "SuccessMessage: Hi.", "EntityId: 2" };
            Assert.Equal(expected, logText);
        }

        [Fact]
        public void SuccessUserMessageGetLogText()
        {
            var logText = new SuccessUserMessage("Hi.").GetLogText();
            var expected = new [] { "SuccessMessage: Hi." };
            Assert.Equal(expected, logText);
        }
    }
}
