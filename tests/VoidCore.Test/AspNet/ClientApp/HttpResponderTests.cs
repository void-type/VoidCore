using Microsoft.AspNetCore.Mvc;
using VoidCore.AspNet.ClientApp;
using VoidCore.Model.Domain;
using VoidCore.Model.Responses.Collections;
using VoidCore.Model.Responses.Files;
using VoidCore.Model.Responses.Messages;
using Xunit;

namespace VoidCore.Test.AspNet.ClientApp
{
    public class HttpResponderTests
    {
        [Fact]
        public void RespondWithData()
        {
            var responder = new HttpResponder();
            var response = responder.Respond(PostSuccessUserMessage.Create("success", 2));
            Assert.Equal(200, ((ObjectResult)response).StatusCode);
            Assert.Equal("success", ((PostSuccessUserMessage<int>)((ObjectResult)response).Value).Message);
            Assert.Equal(2, ((PostSuccessUserMessage<int>)((ObjectResult)response).Value).Id);
        }

        [Fact]
        public void RespondWithFailure()
        {
            var result = Result.Fail(new Failure("some fail", "some fail"));
            var responder = new HttpResponder();
            var response = responder.Respond(result);
            Assert.Equal(400, ((ObjectResult)response).StatusCode);
            Assert.Equal(1, ((ItemSet<IFailure>)((ObjectResult)response).Value).Count);
        }

        [Fact]
        public void RespondWithFileFailure()
        {
            var result = Result.Fail<ISimpleFile>(new IFailure[] { new Failure("some fail", "some fail"), new Failure("some fail", "some fail") });
            var responder = new HttpResponder();
            var response = responder.RespondWithFile(result);
            Assert.Equal(400, ((ObjectResult)response).StatusCode);
            Assert.Equal(2, ((ItemSet<IFailure>)((ObjectResult)response).Value).Count);
        }

        [Fact]
        public void RespondWithFileSuccess()
        {
            var file = new SimpleFile("fileContent", "fileName") as ISimpleFile;
            var result = Result.Ok(file);
            var responder = new HttpResponder();
            var response = responder.RespondWithFile(result);
            Assert.Equal("application/force-download", ((FileContentResult)response).ContentType);
            Assert.Equal(file.Name, ((FileContentResult)response).FileDownloadName);
            Assert.Equal(file.Content, ((FileContentResult)response).FileContents);
        }

        [Fact]
        public void RespondWithSuccess()
        {
            var result = Result.Ok();
            var responder = new HttpResponder();
            var response = responder.Respond(result);
            Assert.Equal(200, ((ObjectResult)response).StatusCode);
            Assert.Null(((ObjectResult)response).Value);
        }

        [Fact]
        public void RespondWithTypedFailure()
        {
            var result = Result.Fail<string>(new Failure("some fail", "some fail"));
            var responder = new HttpResponder();
            var response = responder.Respond(result);
            Assert.Equal(400, ((ObjectResult)response).StatusCode);
            Assert.Equal(1, ((ItemSet<IFailure>)((ObjectResult)response).Value).Count);
        }

        [Fact]
        public void RespondWithTypedSuccess()
        {
            var result = Result.Ok("Success Object");
            var responder = new HttpResponder();
            var response = responder.Respond(result);
            Assert.Equal(200, ((ObjectResult)response).StatusCode);
            Assert.Equal("Success Object", ((ObjectResult)response).Value);
        }
    }
}