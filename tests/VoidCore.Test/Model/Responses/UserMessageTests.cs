using System;
using VoidCore.Model.Responses.Messages;
using Xunit;

namespace VoidCore.Test.Model.Responses;

public class UserMessageTests
{
    [Fact]
    public void UserMessage_properties()
    {
        var message = new UserMessage("hi");
        Assert.Equal("hi", message.Message);
    }

    [Fact]
    public void UserMessage_integer_properties()
    {
        var message = EntityMessage.Create("hi", 2);
        Assert.Equal("hi", message.Message);
        Assert.Equal(2, message.Id);
    }

    [Fact]
    public void UserMessage_string_properties()
    {
        var message = EntityMessage.Create("hi", "2");
        Assert.Equal("hi", message.Message);
        Assert.Equal("2", message.Id);
    }

    [Fact]
    public void UserMessage_guid_properties()
    {
        var guid = Guid.NewGuid();
        var message = EntityMessage.Create("hi", guid);
        Assert.Equal("hi", message.Message);
        Assert.Equal(guid, message.Id);
    }
}
