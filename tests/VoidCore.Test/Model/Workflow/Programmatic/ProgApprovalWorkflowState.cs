using VoidCore.Model.Functional;

namespace VoidCore.Test.Model.Workflow.Programmatic;

public class ProgApprovalWorkflowState : ValueObject
{
    private readonly string _value;

    public ProgApprovalWorkflowState(string value)
    {
        _value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _value;
    }
}
