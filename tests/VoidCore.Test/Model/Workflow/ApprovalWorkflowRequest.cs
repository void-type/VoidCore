namespace VoidCore.Test.Model.Workflow;

/// <summary>
/// This class holds state for an instance of the workflow.
/// </summary>
public class ApprovalWorkflowRequest
{
    public int Id { get; set; }
    public decimal Cost { get; set; }
    public string RequesterName { get; set; } = string.Empty;
    public ApprovalWorkflow.State State { get; set; } = ApprovalWorkflow.State.NotStarted;
    public List<Approval> Approvals { get; set; } = [];
    public Revoke? Revoke { get; set; }
    public int ApprovalLevelsNeeded { get; set; }
    public bool ApprovalsMet => Approvals.Count >= ApprovalLevelsNeeded;
}
