using VoidCore.Domain.Workflow;

namespace VoidCore.Test.Domain.Workflow
{
    public partial class Workflow : WorkflowAbstract<Workflow.State, Workflow.Command>
    {
        public Workflow() : base(optionsBuilder =>
            optionsBuilder
                // Not Started
                .AddTransition(State.NotStarted, Command.Start, State.ApprovalRequested)
                .AddTransition(State.NotStarted, Command.Cancel, State.Cancelled)

                // Approval Requested
                .AddTransition(State.ApprovalRequested, Command.Approve, State.Approved)
                .AddTransition(State.ApprovalRequested, Command.Reject, State.NotStarted)
                .AddTransition(State.ApprovalRequested, Command.Cancel, State.Cancelled)

                // Approved
                .AddTransition(State.Approved, Command.Revoke, State.Revoked)
                .AddTransition(State.Approved, Command.Expire, State.Expired))
        { }

        public enum State
        {
            NotStarted,
            ApprovalRequested,
            Approved,
            Cancelled,
            Revoked,
            Expired
        }

        public enum Command
        {
            Start,
            Approve,
            Reject,
            Cancel,
            Revoke,
            Expire
        }
    }
}
