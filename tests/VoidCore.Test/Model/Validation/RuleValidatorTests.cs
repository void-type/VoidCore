using System.Linq;
using VoidCore.Model.Domain;
using VoidCore.Model.Validation;
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

        internal class ValidatorLogicTestValidator : RuleValidatorAbstract<string>
        {
            protected override void BuildRules()
            {
                CreateRule("violated", "violated field name")
                    .InvalidWhen(stg => stg != "not match");

                CreateRule("valid", "validField")
                    .InvalidWhen(stg => stg != "match");
            }
        }

        internal class RuleLogicTestValidator : RuleValidatorAbstract<bool>
        {
            public RuleLogicTestValidator(bool isValid2, bool isSuppressed1, bool isSuppressed2)
            {
                _isValid2 = isValid2;
                _isSuppressed1 = isSuppressed1;
                _isSuppressed2 = isSuppressed2;
            }

            protected override void BuildRules()
            {
                CreateRule("test is invalid", "testField")
                    .InvalidWhen(isValid1 => isValid1)
                    .InvalidWhen(isValid1 => _isValid2)
                    .ExceptWhen(isValid1 => _isSuppressed1)
                    .ExceptWhen(isValid1 => _isSuppressed2);
            }

            private readonly bool _isValid2;
            private readonly bool _isSuppressed1;
            private readonly bool _isSuppressed2;
        }

        internal class NoValidConditionTestValidator : RuleValidatorAbstract<int>
        {
            protected override void BuildRules()
            {
                CreateRule("implicit success", "implicit success");

                CreateRule("implicit success", "implicit success")
                    .ExceptWhen(number => false);

                CreateRule("implicit success", "implicit success")
                    .ExceptWhen(number => true);
            }
        }

        internal class InheritedEntityValidator : RuleValidatorAbstract<Entity>
        {
            protected override void BuildRules()
            {
                CreateRule("invalid", "someProperty")
                    .InvalidWhen(v => string.IsNullOrWhiteSpace(v.SomeProperty));
            }
        }

        internal class Entity
        {
            public string SomeProperty { get; set; }
        }

        internal class DerivedEntity : Entity
        {

        }

        internal class FailureBuilderValidator : RuleValidatorAbstract<Failure>
        {
            protected override void BuildRules()
            {
                CreateRule(v => $"{v.Message}", "hardCodedUiHandle")
                    .InvalidWhen(v => true);

                CreateRule("hardCodedMessage", v => $"{v.UiHandle}")
                    .InvalidWhen(v => true);

                CreateRule(v => $"{v.Message}", v => $"{v.UiHandle}")
                    .InvalidWhen(v => true);

                CreateRule("hardCodedMessage", "hardCodedUiHandle")
                    .InvalidWhen(v => true);
            }
        }
    }
}
