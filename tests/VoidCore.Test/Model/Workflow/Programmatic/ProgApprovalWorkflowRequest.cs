using System.Collections.Generic;

namespace VoidCore.Test.Model.Workflow.Programmatic;

/// <summary>
/// This class holds state for an instance of the workflow.
/// </summary>
public class ProgApprovalWorkflowRequest
{
    public int Id { get; set; }
    public decimal Cost { get; set; }
    public string RequesterName { get; set; }
    public ProgApprovalWorkflowState State { get; set; } = ProgApprovalWorkflow.State.NotStarted;
    public List<Approval> Approvals { get; set; } = [];
    public Revoke Revoke { get; set; }
    public int ApprovalLevelsNeeded { get; set; }
    public bool ApprovalsMet => Approvals.Count >= ApprovalLevelsNeeded;
}
