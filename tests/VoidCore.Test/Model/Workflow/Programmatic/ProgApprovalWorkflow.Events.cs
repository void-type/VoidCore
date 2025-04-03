using VoidCore.Model.Functional;

namespace VoidCore.Test.Model.Workflow.Programmatic;

/// <summary>
/// This partial defines events to mutate workflow state. State is stored in ProgrammaticApprovalWorkflowRequest.
/// These events should maintain valid state.
/// </summary>
public partial class ProgApprovalWorkflow
{
    private static readonly IReadOnlyList<string> _approverNames = new List<string> { "John", "Joe" };

    public IResult<ProgApprovalWorkflowState> OnRequestApprovals(ProgApprovalWorkflowRequest request)
    {
        return MoveNext(request, Command.Start)
            .Then(nextState =>
            {
                if (request.ApprovalsMet)
                {
                    return MoveNext(request, Command.Approve);
                }

                return Result.Ok(nextState);
            })
            .TeeOnSuccess(() => AddApproval(request));
    }

    public IResult<ProgApprovalWorkflowState> OnApprove(ProgApprovalWorkflowRequest request, Approval approval)
    {
        var canContinue = GetNext(request.State, Command.Approve);

        if (canContinue.IsFailed)
        {
            return canContinue;
        }

        if (request.ApprovalsMet || _approverNames.Count - request.Approvals.Count < 1)
        {
            return MoveNext(request, Command.Approve);
        }

        approval.IsApproved = true;
        AddApproval(request);
        return Result.Ok(request.State);
    }

    public IResult<ProgApprovalWorkflowState> OnReject(ProgApprovalWorkflowRequest request)
    {
        request.Approvals.Clear();
        return MoveNext(request, Command.Reject);
    }

    public IResult<ProgApprovalWorkflowState> OnCancel(ProgApprovalWorkflowRequest request)
    {
        return MoveNext(request, Command.Cancel);
    }

    public IResult<ProgApprovalWorkflowState> OnRevoke(ProgApprovalWorkflowRequest request, string revokerName)
    {
        return MoveNext(request, Command.Revoke)
            .TeeOnSuccess(() => SetRevoke(request, revokerName));
    }

    public IResult<ProgApprovalWorkflowState> OnExpire(ProgApprovalWorkflowRequest request)
    {
        return MoveNext(request, Command.Expire);
    }

    private IResult<ProgApprovalWorkflowState> MoveNext(ProgApprovalWorkflowRequest request, ProgApprovalWorkflowCommand command)
    {
        return GetNext(request.State, command)
            .TeeOnSuccess(newState => request.State = newState);
    }

    private static void AddApproval(ProgApprovalWorkflowRequest request)
    {
        new Approval
        {
            Id = request.Approvals.Count + 1,
            Approver = _approverNames[request.Approvals.Count],
            IsApproved = false
        }
            .Tee(request.Approvals.Add);
    }

    private static void SetRevoke(ProgApprovalWorkflowRequest request, string revokerName)
    {
        new Revoke
        {
            Id = request.Approvals.Count + 1,
            Revoker = revokerName,
        }
            .Tee(r => request.Revoke = r);
    }
}
