using System.Linq;
using VoidCore.Domain;
using VoidCore.Domain.RuleValidator;
using Xunit;

namespace VoidCore.Test.Domain
{
    public class RuleValidatorTests
    {
        [Fact]
        public void FailuresAreBuiltProperly()
        {
            var result = new FailureBuilderValidator().Validate(new Failure("message", "uiHandle"));

            Assert.Equal(2, result.Failures.Select(f => f.Message).Count(m => m == "message"));
            Assert.Equal(2, result.Failures.Select(f => f.Message).Count(m => m == "hardCodedMessage"));
            Assert.Equal(2, result.Failures.Select(f => f.UiHandle).Count(m => m == "uiHandle"));
            Assert.Equal(2, result.Failures.Select(f => f.UiHandle).Count(m => m == "hardCodedUiHandle"));
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
        [InlineData(false, true, false, true, false)]
        [InlineData(false, true, true, false, false)]
        [InlineData(false, true, true, true, false)]
        [InlineData(true, false, false, false, true)]
        [InlineData(true, false, false, true, false)]
        [InlineData(true, false, true, false, false)]
        [InlineData(true, false, true, true, false)]
        [InlineData(true, true, false, false, true)]
        [InlineData(true, true, false, true, false)]
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
            var result = new InheritedEntityValidator().Validate(new DerivedEntity { SomeProperty = string.Empty });

            Assert.True(result.IsFailed);
            Assert.True(result.Failures.Any());
        }

        [Fact]
        public void ValidatorWorksOnDerivedTypesSuccess()
        {
            var result = new InheritedEntityValidator().Validate(new DerivedEntity { SomeProperty = "valid" });

            Assert.True(result.IsSuccess);
            Assert.False(result.Failures.Any());
        }

        private class Entity
        {
            public string SomeProperty { get; set; }
        }

        private class DerivedEntity : Entity { }

        private class ValidatorLogicTestValidator : RuleValidatorAbstract<string>
        {
            public ValidatorLogicTestValidator()
            {
                CreateRule(new Failure("violated", "violated field name"))
                    .InvalidWhen(stg => stg != "not match");

                CreateRule(new Failure("valid", "validField"))
                    .InvalidWhen(stg => stg != "match");
            }
        }

        private class RuleLogicTestValidator : RuleValidatorAbstract<bool>
        {
            public RuleLogicTestValidator(bool isValid2, bool isSuppressed1, bool isSuppressed2)
            {
                CreateRule(new Failure("test is invalid", "testField"))
                    .InvalidWhen(isValid1 => isValid1)
                    .InvalidWhen(isValid1 => isValid2)
                    .ExceptWhen(isValid1 => isSuppressed1)
                    .ExceptWhen(isValid1 => isSuppressed2);
            }
        }

        private class NoValidConditionTestValidator : RuleValidatorAbstract<int>
        {
            public NoValidConditionTestValidator()
            {
                CreateRule(new Failure("implicit success", "implicit success"));

                CreateRule(new Failure("implicit success", "implicit success"))
                    .ExceptWhen(number => false);

                CreateRule(new Failure("implicit success", "implicit success"))
                    .ExceptWhen(number => true);
            }
        }

        private class InheritedEntityValidator : RuleValidatorAbstract<Entity>
        {
            public InheritedEntityValidator()
            {
                CreateRule(new Failure("invalid", "someProperty"))
                    .InvalidWhen(v => string.IsNullOrWhiteSpace(v.SomeProperty));
            }
        }

        private class FailureBuilderValidator : RuleValidatorAbstract<Failure>
        {
            public FailureBuilderValidator()
            {
                CreateRule(v => new Failure($"{v.Message}", "hardCodedUiHandle"))
                    .InvalidWhen(v => true);

                CreateRule(v => new Failure("hardCodedMessage", $"{v.UiHandle}"))
                    .InvalidWhen(v => true);

                CreateRule(v => new Failure($"{v.Message}", $"{v.UiHandle}"))
                    .InvalidWhen(v => true);

                CreateRule(v => new Failure("hardCodedMessage", "hardCodedUiHandle"))
                    .InvalidWhen(v => true);
            }
        }
    }
}
