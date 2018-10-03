using VoidCore.Model.DomainEvents;
using VoidCore.Model.Validation;

namespace VoidCore.Test.Model.Validation
{
    public class FailureBuilderValidator : ValidatorAbstract<Failure>
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
