using VoidCore.Model.Validation;

namespace VoidCore.Test.Model.Validation
{
    public class InheritedValidator : ValidatorAbstract<Entity>
    {
        protected override void BuildRules()
        {
            CreateRule("invalid", "someProperty")
                .InvalidWhen(v => string.IsNullOrWhiteSpace(v.SomeProperty));
        }
    }

    public class Entity
    {
        public string SomeProperty { get; set; }
    }

    public class DerivedEntity : Entity
    {

    }
}
