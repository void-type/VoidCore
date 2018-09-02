using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Action.Railway;
using VoidCore.Model.Action.Responses.File;
using Xunit;

namespace VoidCore.Test.AspNet
{
    public class HttpResponderTests
    {
        [Fact]
        public void RespondWithDataSuccess()
        {
            var result = Result.Ok();
            var responder = new HttpResponder();
            var response = responder.Respond(result);
            Assert.Equal(200, ((ObjectResult) response).StatusCode);
        }

        [Fact]
        public void RespondWithDataFailure()
        {
            var result = Result.Fail(new Failure("some fail", "some fail"));
            var responder = new HttpResponder();
            var response = responder.Respond(result);
            Assert.Equal(400, ((ObjectResult) response).StatusCode);
        }

        [Fact]
        public void RespondWithFileSuccess()
        {
            var file = new SimpleFile("fileContent", "fileName");
            var result = Result.Ok(file);
            var responder = new HttpResponder();
            var response = responder.RespondWithFile(result);
            Assert.Equal("application/force-download", ((FileContentResult) response).ContentType);
            Assert.Equal(file.Name, ((FileContentResult) response).FileDownloadName);
            Assert.Equal(file.Content, ((FileContentResult) response).FileContents);
        }

        [Fact]
        public void RespondWithFileFailure()
        {
            var result = Result.Fail<SimpleFile>(new Failure("some fail", "some fail"));
            var responder = new HttpResponder();
            var response = responder.RespondWithFile(result);
            Assert.Equal(400, ((ObjectResult) response).StatusCode);
        }
    }
}
