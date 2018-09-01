using VoidCore.Model.Validation;

namespace VoidCore.Test.Model.Validation
{
    internal class NoValidConditionTestValidator : AbstractValidator<int>
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
}
