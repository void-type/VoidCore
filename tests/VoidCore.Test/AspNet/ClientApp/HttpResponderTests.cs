using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Functional;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Files;
using VoidCore.Model.Responses.Messages;
using Xunit;

namespace VoidCore.Test.AspNet.ClientApp;

public class HttpResponderTests
{
    [Fact]
    public void Respond_with_data()
    {
        var response = HttpResponder.Respond(EntityMessage.Create("success", 2));
        Assert.Equal(200, ((ObjectResult)response).StatusCode);
        Assert.Equal("success", ((EntityMessage<int>)((ObjectResult)response).Value!).Message);
        Assert.Equal(2, ((EntityMessage<int>)((ObjectResult)response).Value!).Id);
    }

    [Fact]
    public void Respond_with_failure()
    {
        var result = Result.Fail(new Failure("some fail", "some fail"));
        var response = HttpResponder.Respond(result);
        Assert.Equal(400, ((ObjectResult)response).StatusCode);
        Assert.Equal(1, ((ItemSet<IFailure>)((ObjectResult)response).Value!).Count);
    }

    [Fact]
    public void Respond_with_file_failure()
    {
        var result = Result.Fail<SimpleFile>(new Failure("some fail", "some fail"), new Failure("some fail", "some fail"));
        var response = HttpResponder.RespondWithFile(result);
        Assert.Equal(400, ((ObjectResult)response).StatusCode);
        Assert.Equal(2, ((ItemSet<IFailure>)((ObjectResult)response).Value!).Count);
    }

    [Fact]
    public void Respond_with_file_success()
    {
        var file = new SimpleFile("fileContent", "fileName");
        var result = Result.Ok(file);
        var response = HttpResponder.RespondWithFile(result);
        Assert.Equal("application/force-download", ((FileContentResult)response).ContentType);
        Assert.Equal(file.Name, ((FileContentResult)response).FileDownloadName);
        Assert.Equal(file.Content.AsBytes, ((FileContentResult)response).FileContents);
    }

    [Fact]
    public void Respond_with_success()
    {
        var result = Result.Ok();
        var response = HttpResponder.Respond(result);
        Assert.Equal(200, ((ObjectResult)response).StatusCode);
        Assert.Null(((ObjectResult)response).Value);
    }

    [Fact]
    public void Respond_with_typed_failure()
    {
        var result = Result.Fail<string>(new Failure("some fail", "some fail"));
        var response = HttpResponder.Respond(result);
        Assert.Equal(400, ((ObjectResult)response).StatusCode);
        Assert.Equal(1, ((ItemSet<IFailure>)((ObjectResult)response).Value!).Count);
    }

    [Fact]
    public void Respond_with_typed_success()
    {
        var result = Result.Ok("Success Object");
        var response = HttpResponder.Respond(result);
        Assert.Equal(200, ((ObjectResult)response).StatusCode);
        Assert.Equal("Success Object", ((ObjectResult)response).Value);
    }
}
