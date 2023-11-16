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

    public readonly struct State
    {
        public static readonly ProgApprovalWorkflowState NotStarted = new("NotStarted");
        public static readonly ProgApprovalWorkflowState ApprovalRequested = new("ApprovalRequested");
        public static readonly ProgApprovalWorkflowState Approved = new("Approved");
        public static readonly ProgApprovalWorkflowState Cancelled = new("Cancelled");
        public static readonly ProgApprovalWorkflowState Revoked = new("Revoked");
        public static readonly ProgApprovalWorkflowState Expired = new("Expired");
    }

    public readonly struct Command
    {
        public static readonly ProgApprovalWorkflowCommand Start = new("Start");
        public static readonly ProgApprovalWorkflowCommand Approve = new("Approve");
        public static readonly ProgApprovalWorkflowCommand Reject = new("Reject");
        public static readonly ProgApprovalWorkflowCommand Cancel = new("Cancel");
        public static readonly ProgApprovalWorkflowCommand Revoke = new("Revoke");
        public static readonly ProgApprovalWorkflowCommand Expire = new("Expire");
    }
}
