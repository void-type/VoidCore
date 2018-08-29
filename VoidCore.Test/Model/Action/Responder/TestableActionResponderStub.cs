using System;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.File;
using VoidCore.Model.Action.Responses.ItemSet;
using VoidCore.Model.Action.Responses.UserMessage;
using VoidCore.Model.Validation;

namespace VoidCore.Test.Model.Action.Responder
{
    internal class TestableActionResponderStub : AbstractActionResponder<string>
    {
        public void SetResponse(string response)
        {
            Response = response;
        }

        public override void WithError(string errorMessage, Exception exception = null, params string[] logMessages)
        {
            throw new NotImplementedException();
        }

        public override void WithError(ErrorUserMessage errorMessage, Exception exception = null, params string[] logMessages)
        {
            throw new NotImplementedException();
        }

        public override void WithSuccess(object resultItem, params string[] logMessages)
        {
            throw new NotImplementedException();
        }

        public override void WithSuccess(ISimpleFile file, params string[] logMessages)
        {
            throw new NotImplementedException();
        }

        public override void WithWarning(IItemSet<IValidationError> validationErrors, params string[] logMessages)
        {
            throw new NotImplementedException();
        }

        public override void WithWarning(string warningMessage, params string[] logMessages)
        {
            throw new NotImplementedException();
        }
    }
}
