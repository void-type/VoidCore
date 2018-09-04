using VoidCore.Model.Validation;

namespace VoidCore.Test.Model.Validation
{
    internal class RuleLogicTestValidator : ValidatorAbstract<bool>
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
                .ValidWhen(isValid1 => isValid1)
                .ValidWhen(isValid1 => _isValid2)
                .ExceptWhen(isValid1 => _isSuppressed1)
                .ExceptWhen(isValid1 => _isSuppressed2);
        }

        private readonly bool _isValid2;
        private readonly bool _isSuppressed1;
        private readonly bool _isSuppressed2;
    }
}
