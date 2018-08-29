using VoidCore.Model.Validation;

namespace VoidCore.Test.Model.Validation
{
    internal class TestableValidator : AbstractValidator<bool>
    {
        public TestableValidator(bool isSuppressed)
        {
            _isSuppressed = isSuppressed;
        }

        protected override void BuildRules(bool isValid)
        {
            CreateRule("test is invalid", "testField")
                .ValidWhen(isValid)
                .ValidWhen(true)
                .ExceptWhen(_isSuppressed)
                .ExceptWhen(false);
        }

        private readonly bool _isSuppressed;
    }
}
