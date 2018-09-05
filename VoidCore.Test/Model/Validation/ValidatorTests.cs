using System.Linq;
using Xunit;

namespace VoidCore.Test.Model.Validation
{
    public class ValidatorTests
    {
        [Theory]
        [InlineData(false, false, false, true, false)]
        [InlineData(false, false, true, false, false)]
        [InlineData(false, false, true, true, true)]
        [InlineData(false, true, false, false, false)]
        [InlineData(false, true, false, true, false)]
        [InlineData(false, true, true, false, false)]
        [InlineData(false, true, true, true, true)]
        [InlineData(true, false, false, false, false)]
        [InlineData(true, false, false, true, false)]
        [InlineData(true, false, true, false, false)]
        [InlineData(true, false, true, true, true)]
        [InlineData(true, true, false, false, true)]
        [InlineData(true, true, false, true, true)]
        [InlineData(true, true, true, true, true)]

        public void RuleViolatesAndSuppressesProperly(bool isValid1, bool isValid2, bool isSuppressed1, bool isSuppressed2, bool successExpected)
        {
            var result = new RuleLogicTestValidator(isValid2, isSuppressed1, isSuppressed2).Validate(isValid1);

            Assert.Equal(successExpected, result.IsSuccess);
            Assert.NotEqual(successExpected, result.IsFailed);
            Assert.NotEqual(successExpected, result.Failures.Any());
        }

        [Fact]
        public void ProperFailureIsReturnedOnFailure()
        {
            var result = new ValidatorLogicTestValidator().Validate("match");

            Assert.Equal("violated", result.Failures.Single().Message);
        }

        [Fact]
        public void ValidationSuccessWhenNoValidConditionAdded()
        {
            var result = new NoValidConditionTestValidator().Validate(1);

            Assert.True(result.IsSuccess);
            Assert.False(result.Failures.Any());
        }

        [Fact]
        public void ValidationSuccessWhenNoValidConditionAdded2()
        {
            var result = new NoValidConditionTestValidator().Validate(1);

            Assert.True(result.IsSuccess);
            Assert.False(result.Failures.Any());
        }

        [Fact]
        public void ValidatiorWorksOnDerivedTypes()
        {
            var result = new InheritedValidator().Validate(new DerivedEntity() { SomeProperty = "valid" });

            Assert.True(result.IsSuccess);
            Assert.False(result.Failures.Any());
        }
    }
}
