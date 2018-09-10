using System.Linq;
using Xunit;

namespace VoidCore.Test.Model.Validation
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData(false, false, false, true, false)]
        [InlineData(false, false, true, false, false)]
        [InlineData(false, false, true, true, false)]
        [InlineData(false, true, false, false, false)]
        [InlineData(false, true, false, true, false)]
        [InlineData(false, true, true, false, false)]
        [InlineData(false, true, true, true, false)]
        [InlineData(true, false, false, false, false)]
        [InlineData(true, false, false, true, false)]
        [InlineData(true, false, true, false, false)]
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
        public void ProperFailureIsReturnedOnFailure()
        {
            var result = new ValidatorLogicTestValidator().Validate("match");

            Assert.Equal("violated", result.Failures.Single().Message);
        }

        [Fact]
        public void ValidationSuccessWhenNoInvalidConditionAdded()
        {
            var result = new NoValidConditionTestValidator().Validate(1);

            Assert.True(result.IsSuccess);
            Assert.False(result.Failures.Any());
        }

        [Fact]
        public void ValidatiorWorksOnDerivedTypes()
        {
            var result = new InheritedEntityValidator().Validate(new DerivedEntity() { SomeProperty = "valid" });

            Assert.True(result.IsSuccess);
            Assert.False(result.Failures.Any());
        }
    }
}
