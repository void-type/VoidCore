using System.Collections.Generic;
using VoidCore.Domain;

namespace VoidCore.Test.Domain.Workflow
{
    public partial class Workflow
    {
        private static readonly IReadOnlyList<string> _approverNames = new List<string> { "John", "Joe" };

        public IResult<State> OnRequestApprovals(Request request)
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

        public IResult<State> OnApprove(Request request, Approval approval)
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

        public IResult<State> OnReject(Request request)
        {
            request.Approvals.Clear();
            return MoveNext(request, Command.Reject);
        }

        public IResult<State> OnCancel(Request request)
        {
            return MoveNext(request, Command.Cancel);
        }

        public IResult<State> OnRevoke(Request request, string revokerName)
        {
            return MoveNext(request, Command.Revoke)
                .TeeOnSuccess(() => SetRevoke(request, revokerName));
        }

        public IResult<State> OnExpire(Request request)
        {
            return MoveNext(request, Command.Expire);
        }

        private IResult<State> MoveNext(Request request, Command command)
        {
            return GetNext(request.State, command)
                .TeeOnSuccess(newState => request.State = newState);
        }

        private void AddApproval(Request request)
        {
            new Approval
            {
                Id = request.Approvals.Count + 1,
                Approver = _approverNames[request.Approvals.Count],
                IsApproved = false
            }
                .Tee(request.Approvals.Add);
        }

        private void SetRevoke(Request request, string revokerName)
        {
            new Revoke
            {
                Id = request.Approvals.Count + 1,
                Revoker = revokerName,
            }
                .Tee(r => request.Revoke = r);
        }
    }
}
