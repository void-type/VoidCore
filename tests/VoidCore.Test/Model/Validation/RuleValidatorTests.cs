using System.Linq;
using VoidCore.Model.Domain;
using Xunit;

namespace VoidCore.Test.Model.Validation
{
    public class RuleValidatorTests
    {
        [Fact]
        public void FailuresAreBuiltProperly()
        {
            var result = new FailureBuilderValidator().Validate(new Failure("message", "uiHandle"));

            Assert.Equal(2, result.Failures.Select(f => f.Message).Where(m => m == "message").Count());
            Assert.Equal(2, result.Failures.Select(f => f.Message).Where(m => m == "hardCodedMessage").Count());
            Assert.Equal(2, result.Failures.Select(f => f.UiHandle).Where(m => m == "uiHandle").Count());
            Assert.Equal(2, result.Failures.Select(f => f.UiHandle).Where(m => m == "hardCodedUiHandle").Count());
        }

        [Fact]
        public void ProperFailureIsReturnedOnFailure()
        {
            var result = new ValidatorLogicTestValidator().Validate("match");

            Assert.Equal("violated", result.Failures.Single().Message);
        }

        [Theory]
        [InlineData(false, false, false, true, false)]
        [InlineData(false, false, true, false, false)]
        [InlineData(false, false, true, true, false)]
        [InlineData(false, true, false, false, true)]
        [InlineData(false, true, false, true, true)]
        [InlineData(false, true, true, false, true)]
        [InlineData(false, true, true, true, false)]
        [InlineData(true, false, false, false, true)]
        [InlineData(true, false, false, true, true)]
        [InlineData(true, false, true, false, true)]
        [InlineData(true, false, true, true, false)]
        [InlineData(true, true, false, false, true)]
        [InlineData(true, true, false, true, true)]
        [InlineData(true, true, true, true, false)]
        public void RuleViolatesAndSuppressesProperly(bool invalidWhen1, bool invalidWhen2, bool isSuppressed1, bool isSuppressed2, bool failureExpected)
        {
            var result = new RuleLogicTestValidator(invalidWhen2, isSuppressed1, isSuppressed2).Validate(invalidWhen1);

            Assert.NotEqual(failureExpected, result.IsSuccess);
            Assert.Equal(failureExpected, result.IsFailed);
            Assert.Equal(failureExpected, result.Failures.Any());
        }

        [Fact]
        public void ValidationSuccessWhenNoInvalidConditionAdded()
        {
            var result = new NoValidConditionTestValidator().Validate(1);

            Assert.True(result.IsSuccess);
            Assert.False(result.Failures.Any());
        }

        [Fact]
        public void ValidatorWorksOnDerivedTypesFailure()
        {
            var result = new InheritedEntityValidator().Validate(new DerivedEntity() { SomeProperty = "" });

            Assert.True(result.IsFailed);
            Assert.True(result.Failures.Any());
        }

        [Fact]
        public void ValidatorWorksOnDerivedTypesSuccess()
        {
            var result = new InheritedEntityValidator().Validate(new DerivedEntity() { SomeProperty = "valid" });

            Assert.True(result.IsSuccess);
            Assert.False(result.Failures.Any());
        }
    }
}
