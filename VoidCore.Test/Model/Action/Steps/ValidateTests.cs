using Moq;
using System.Collections.Generic;
using VoidCore.Model.Action.Responder;
using VoidCore.Model.Action.Responses.ItemSet;
using VoidCore.Model.Action.Steps;
using VoidCore.Model.Validation;
using Xunit;

namespace VoidCore.Test.Model.Action.Steps
{
    public class ValidateTests
    {
        [Fact]
        public void ValidateCalledWhenError()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.WithWarning(It.IsAny<IItemSet<IValidationError>>(), It.IsAny<string[]>()));

            var validatorMock = new Mock<IValidator<string>>();
            validatorMock.Setup(mock => mock.Validate("item"))
                .Returns(new List<IValidationError>() { new ValidationError("error", "fieldName") });

            new Validate<string>(validatorMock.Object, "item", "log").Perform(responderMock.Object);

            validatorMock.Verify(mock => mock.Validate("item"), Times.Once());
            responderMock.Verify(mock => mock.WithWarning(It.IsAny<IItemSet<IValidationError>>(), It.IsAny<string[]>()), Times.Once());
        }

        [Fact]
        public void ValidateNotCalledWhenNoError()
        {
            var responderMock = new Mock<IActionResponder>();
            responderMock.Setup(mock => mock.WithWarning(It.IsAny<IItemSet<IValidationError>>(), It.IsAny<string[]>()));

            var validatorMock = new Mock<IValidator<string>>();
            validatorMock.Setup(mock => mock.Validate("item"))
                .Returns(new List<IValidationError>());

            new Validate<string>(validatorMock.Object, "item", "log").Perform(responderMock.Object);

            validatorMock.Verify(mock => mock.Validate("item"), Times.Once());
            responderMock.Verify(mock => mock.WithWarning(It.IsAny<IItemSet<IValidationError>>(), It.IsAny<string[]>()), Times.Never());
        }
    }
}
