using VoidCore.Model.Domain;
using VoidCore.Model.Validation;

namespace VoidCore.Test.Model.Validation
{
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
