using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VoidCore.Test.Model.Workflow.Programmatic;

public class ProgrammaticWorkflowTests
{
    [Fact]
    public void ProgrammaticWorkflow_only_follows_configured_transitions()
    {
        var workflow = new ProgApprovalWorkflow();

        var request = new ProgApprovalWorkflowRequest
        {
            Id = 3,
            Cost = 44.4m,
            RequesterName = "Dude Man",
            ApprovalLevelsNeeded = 2
        };

        Assert.Equal(ProgApprovalWorkflow.State.NotStarted, request.State);

        // Request supervisor approval
        workflow.OnRequestApprovals(request);
        Assert.Equal(ProgApprovalWorkflow.State.ApprovalRequested, request.State);

        // Supervisor approval
        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(ProgApprovalWorkflow.State.ApprovalRequested, request.State);

        // Manager rejected, start over
        workflow.OnReject(request);
        Assert.Equal(ProgApprovalWorkflow.State.NotStarted, request.State);

        // Request supervisor approval
        workflow.OnRequestApprovals(request);
        Assert.Equal(ProgApprovalWorkflow.State.ApprovalRequested, request.State);

        // Supervisor approval
        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(ProgApprovalWorkflow.State.ApprovalRequested, request.State);

        // Manager approval
        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(ProgApprovalWorkflow.State.Approved, request.State);

        // Cancellation is not a valid transition.
        var cancellationResult = workflow.OnCancel(request);
        Assert.True(cancellationResult.IsFailed);
        Assert.Equal(ProgApprovalWorkflow.State.Approved, request.State);

        // We can revoke after being approved.
        var revokeResult = workflow.OnRevoke(request, "Bill");
        Assert.True(revokeResult.IsSuccess);
        Assert.Equal(ProgApprovalWorkflow.State.Revoked, request.State);
    }

    [Fact]
    public void ProgrammaticWorkflow_demo_short_circuit_logic_no_approval_needed()
    {
        var workflow = new ProgApprovalWorkflow();

        var request = new ProgApprovalWorkflowRequest
        {
            Id = 4,
            Cost = 44.4m,
            RequesterName = "Another guy",
            ApprovalLevelsNeeded = 0
        };

        // Short-circuit logic because no approval needed: Start, immediately approved
        workflow.OnRequestApprovals(request);
        Assert.Equal(ProgApprovalWorkflow.State.Approved, request.State);

        // We can expire
        workflow.OnExpire(request);
        Assert.Equal(ProgApprovalWorkflow.State.Expired, request.State);
    }

    [Fact]
    public void ProgrammaticWorkflow_demo_short_circuit_logic_no_more_approvers_left()
    {
        var workflow = new ProgApprovalWorkflow();

        var request = new ProgApprovalWorkflowRequest
        {
            Id = 4,
            Cost = 44.4m,
            RequesterName = "Another guy",
            ApprovalLevelsNeeded = 4
        };

        workflow.OnRequestApprovals(request);
        Assert.Equal(ProgApprovalWorkflow.State.ApprovalRequested, request.State);

        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(ProgApprovalWorkflow.State.ApprovalRequested, request.State);

        // Short-circuit logic because no approval needed: Start, immediately approved
        workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved));
        Assert.Equal(ProgApprovalWorkflow.State.Approved, request.State);
    }

    [Fact]
    public void ProgrammaticWorkflow_demo_can_cancel()
    {
        var workflow = new ProgApprovalWorkflow();

        var request = new ProgApprovalWorkflowRequest
        {
            Id = 4,
            Cost = 44.4m,
            RequesterName = "Another guy",
            ApprovalLevelsNeeded = 4
        };

        Assert.True(workflow.OnRequestApprovals(request).IsSuccess);
        Assert.Equal(ProgApprovalWorkflow.State.ApprovalRequested, request.State);

        Assert.True(workflow.OnCancel(request).IsSuccess);
        Assert.Equal(ProgApprovalWorkflow.State.Cancelled, request.State);

        // Request has no valid transition out of a cancelled state.
        Assert.True(workflow.OnApprove(request, request.Approvals.Single(a => !a.IsApproved)).IsFailed);
        Assert.Equal(ProgApprovalWorkflow.State.Cancelled, request.State);

        Assert.True(workflow.OnReject(request).IsFailed);
        Assert.Equal(ProgApprovalWorkflow.State.Cancelled, request.State);

        Assert.True(workflow.OnRevoke(request, "Bill").IsFailed);
        Assert.Equal(ProgApprovalWorkflow.State.Cancelled, request.State);
    }

    [Fact]
    public void ProgrammaticWorkflow_demo_can_get_available_commands_from_state()
    {
        var workflow = new ProgApprovalWorkflow();

        var actualStates = new HashSet<ProgApprovalWorkflowCommand>(workflow.GetAvailableCommands(ProgApprovalWorkflow.State.ApprovalRequested));
        var expectedStates = new HashSet<ProgApprovalWorkflowCommand> {
                ProgApprovalWorkflow.Command.Approve,
                ProgApprovalWorkflow.Command.Reject,
                ProgApprovalWorkflow.Command.Cancel
            };

        Assert.Equal(expectedStates, actualStates);
    }

    [Fact]
    public void ProgrammaticWorkflow_ctor_does_not_allow_duplicate_transitions()
    {
        Assert.Throws<InvalidOperationException>(() => new ProgInvalidWorkflow());
    }

    [Fact]
    public void ProgrammaticWorkflow_ctor_from_data()
    {
        var workflow = new ProgApprovalWorkflow(optionsBuilder =>
            optionsBuilder
                // Not Started
                .AddTransition(ProgApprovalWorkflow.State.NotStarted, ProgApprovalWorkflow.Command.Start, ProgApprovalWorkflow.State.ApprovalRequested)
                .AddTransition(ProgApprovalWorkflow.State.NotStarted, ProgApprovalWorkflow.Command.Cancel, ProgApprovalWorkflow.State.Cancelled)

                // Approval Requested
                .AddTransition(ProgApprovalWorkflow.State.ApprovalRequested, ProgApprovalWorkflow.Command.Approve, ProgApprovalWorkflow.State.Approved)
                .AddTransition(ProgApprovalWorkflow.State.ApprovalRequested, ProgApprovalWorkflow.Command.Reject, ProgApprovalWorkflow.State.NotStarted)
                .AddTransition(ProgApprovalWorkflow.State.ApprovalRequested, ProgApprovalWorkflow.Command.Cancel, ProgApprovalWorkflow.State.Cancelled)

                // Approved
                .AddTransition(ProgApprovalWorkflow.State.Approved, ProgApprovalWorkflow.Command.Revoke, ProgApprovalWorkflow.State.Revoked)
                .AddTransition(ProgApprovalWorkflow.State.Approved, ProgApprovalWorkflow.Command.Expire, ProgApprovalWorkflow.State.Expired));
    }
}
