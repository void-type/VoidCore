using VoidCore.Model.Workflow;

namespace VoidCore.Test.Model.Workflow;

public partial class InvalidWorkflow : WorkflowAbstract<InvalidWorkflow.State, InvalidWorkflow.Command>
{
    public InvalidWorkflow() : base(optionsBuilder =>
        optionsBuilder
            // Not Started
            .AddTransition(State.NotStarted, Command.Start, State.ApprovalRequested)
            .AddTransition(State.NotStarted, Command.Cancel, State.Cancelled)
            .AddTransition(State.NotStarted, Command.Cancel, State.NotStarted))
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
