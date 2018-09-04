using VoidCore.Model.Validation;

namespace VoidCore.Test.Model.Validation
{
    internal class ValidatorLogicTestValidator : ValidatorAbstract<string>
    {
        protected override void BuildRules()
        {
            CreateRule("violated", "violated field name")
                .ValidWhen(stg => stg == "not match");

            CreateRule("valid", "validField")
                .ValidWhen(stg => stg == "match");
        }
    }
}
