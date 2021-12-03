using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VoidCore.Test.Model.Workflow;

public class WorkflowTests
{
    [Fact]
    public void Workflow_only_follows_configured_transitions()
    {
        var workflow = new Workflow();

        var request = new Request
        {
            Id = 3,
            Cost = 44.4m,
            RequesterName = "Dude Man",
            ApprovalLevelsNeeded = 2
        };

        Assert.Equal(Workflow.State.NotStarted, request.State);

        // Request supervisor approval
        workflow.OnRequestApprovals(request);
        Assert.Equal(Workflow.State.ApprovalRequested, request.State);

        // Supervisor approval
        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(Workflow.State.ApprovalRequested, request.State);

        // Manager rejected, start over
        workflow.OnReject(request);
        Assert.Equal(Workflow.State.NotStarted, request.State);

        // Request supervisor approval
        workflow.OnRequestApprovals(request);
        Assert.Equal(Workflow.State.ApprovalRequested, request.State);

        // Supervisor approval
        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(Workflow.State.ApprovalRequested, request.State);

        // Manager approval
        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(Workflow.State.Approved, request.State);

        // Cancellation is not a valid transition.
        var cancellationResult = workflow.OnCancel(request);
        Assert.True(cancellationResult.IsFailed);
        Assert.Equal(Workflow.State.Approved, request.State);

        // We can revoke after being approved.
        var revokeResult = workflow.OnRevoke(request, "Bill");
        Assert.True(revokeResult.IsSuccess);
        Assert.Equal(Workflow.State.Revoked, request.State);
    }

    [Fact]
    public void Workflow_demo_short_circuit_logic_no_approval_needed()
    {
        var workflow = new Workflow();

        var request = new Request
        {
            Id = 4,
            Cost = 44.4m,
            RequesterName = "Another guy",
            ApprovalLevelsNeeded = 0
        };

        // Short-circuit logic because no approval needed: Start, immediately approved
        workflow.OnRequestApprovals(request);
        Assert.Equal(Workflow.State.Approved, request.State);

        // We can expire
        workflow.OnExpire(request);
        Assert.Equal(Workflow.State.Expired, request.State);
    }

    [Fact]
    public void Workflow_demo_short_circuit_logic_no_more_approvers_left()
    {
        var workflow = new Workflow();

        var request = new Request
        {
            Id = 4,
            Cost = 44.4m,
            RequesterName = "Another guy",
            ApprovalLevelsNeeded = 4
        };

        workflow.OnRequestApprovals(request);
        Assert.Equal(Workflow.State.ApprovalRequested, request.State);

        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(Workflow.State.ApprovalRequested, request.State);

        // Short-circuit logic because no approval needed: Start, immediately approved
        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(Workflow.State.Approved, request.State);
    }

    [Fact]
    public void Workflow_demo_can_cancel()
    {
        var workflow = new Workflow();

        var request = new Request
        {
            Id = 4,
            Cost = 44.4m,
            RequesterName = "Another guy",
            ApprovalLevelsNeeded = 4
        };

        Assert.True(workflow.OnRequestApprovals(request).IsSuccess);
        Assert.Equal(Workflow.State.ApprovalRequested, request.State);

        Assert.True(workflow.OnCancel(request).IsSuccess);
        Assert.Equal(Workflow.State.Cancelled, request.State);

        // Request has no valid transition out of a cancelled state.
        Assert.True(workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved)).IsFailed);
        Assert.Equal(Workflow.State.Cancelled, request.State);

        Assert.True(workflow.OnReject(request).IsFailed);
        Assert.Equal(Workflow.State.Cancelled, request.State);

        Assert.True(workflow.OnRevoke(request, "Bill").IsFailed);
        Assert.Equal(Workflow.State.Cancelled, request.State);
    }

    [Fact]
    public void Workflow_demo_can_get_available_commands_from_state()
    {
        var workflow = new Workflow();

        var actualStates = new HashSet<Workflow.Command>(workflow.GetAvailableCommands(Workflow.State.ApprovalRequested));
        var expectedStates = new HashSet<Workflow.Command> {
                Workflow.Command.Approve,
                Workflow.Command.Reject,
                Workflow.Command.Cancel
            };

        Assert.Equal(expectedStates, actualStates);
    }

    [Fact]
    public void Workflow_demo_do_not_allow_duplicate_transitions()
    {
        Assert.Throws<InvalidOperationException>(() => new InvalidWorkflow());
    }
}
