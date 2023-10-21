using VoidCore.Model.Workflow.Programmatic;

namespace VoidCore.Test.Model.Workflow.Programmatic;

public partial class ProgInvalidWorkflow : ProgrammaticWorkflow<ProgApprovalWorkflowState, ProgApprovalWorkflowCommand>
{
    public ProgInvalidWorkflow() : base(optionsBuilder =>
        optionsBuilder
            // Not Started
            .AddTransition(State.NotStarted, Command.Start, State.ApprovalRequested)
            .AddTransition(State.NotStarted, Command.Cancel, State.Cancelled)
            .AddTransition(State.NotStarted, Command.Cancel, State.NotStarted))
    { }

    public struct State
    {
        public static readonly ProgApprovalWorkflowState NotStarted = new ProgApprovalWorkflowState("NotStarted");
        public static readonly ProgApprovalWorkflowState ApprovalRequested = new ProgApprovalWorkflowState("ApprovalRequested");
        public static readonly ProgApprovalWorkflowState Approved = new ProgApprovalWorkflowState("Approved");
        public static readonly ProgApprovalWorkflowState Cancelled = new ProgApprovalWorkflowState("Cancelled");
        public static readonly ProgApprovalWorkflowState Revoked = new ProgApprovalWorkflowState("Revoked");
        public static readonly ProgApprovalWorkflowState Expired = new ProgApprovalWorkflowState("Expired");
    }

    public struct Command
    {
        public static readonly ProgApprovalWorkflowCommand Start = new ProgApprovalWorkflowCommand("Start");
        public static readonly ProgApprovalWorkflowCommand Approve = new ProgApprovalWorkflowCommand("Approve");
        public static readonly ProgApprovalWorkflowCommand Reject = new ProgApprovalWorkflowCommand("Reject");
        public static readonly ProgApprovalWorkflowCommand Cancel = new ProgApprovalWorkflowCommand("Cancel");
        public static readonly ProgApprovalWorkflowCommand Revoke = new ProgApprovalWorkflowCommand("Revoke");
        public static readonly ProgApprovalWorkflowCommand Expire = new ProgApprovalWorkflowCommand("Expire");
    }
}
