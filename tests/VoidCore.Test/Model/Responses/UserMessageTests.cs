using System;
using VoidCore.Model.Responses.Messages;
using Xunit;

namespace VoidCore.Test.Model.Responses
{
    public class UserMessageTests
    {
        [Fact]
        public void UserMessageProperties()
        {
            var message = new UserMessage("hi");
            Assert.Equal("hi", message.Message);
        }

        [Fact]
        public void PostSuccessUserMessageIntegerProperties()
        {
            var message = PostSuccessUserMessage.Create("hi", 2);
            Assert.Equal("hi", message.Message);
            Assert.Equal(2, message.Id);
        }

        [Fact]
        public void PostSuccessUserMessageStringProperties()
        {
            var message = PostSuccessUserMessage.Create("hi", "2");
            Assert.Equal("hi", message.Message);
            Assert.Equal("2", message.Id);
        }

        [Fact]
        public void PostSuccessUserMessageGuidProperties()
        {
            var guid = Guid.NewGuid();
            var message = PostSuccessUserMessage.Create("hi", guid);
            Assert.Equal("hi", message.Message);
            Assert.IsType<Guid>(message.Id);
            Assert.Equal(guid, message.Id);
        }
    }
}
