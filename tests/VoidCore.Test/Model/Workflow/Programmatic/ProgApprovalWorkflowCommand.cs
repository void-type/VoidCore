using VoidCore.Model.Functional;

namespace VoidCore.Test.Model.Workflow.Programmatic;

public class ProgApprovalWorkflowCommand : ValueObject
{
    private readonly string _value;

    public ProgApprovalWorkflowCommand(string value)
    {
        _value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _value;
    }
}
