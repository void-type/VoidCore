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
            Invalid("test is invalid", "test")
                .When(!isValid)
                .ExceptWhen(_isSuppressed);
        }

        private readonly bool _isSuppressed;
    }
}
