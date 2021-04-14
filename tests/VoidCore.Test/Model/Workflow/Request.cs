using System.Collections.Generic;

namespace VoidCore.Test.Model.Workflow
{
    public partial class Request
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public string RequesterName { get; set; }
        public Workflow.State State { get; set; } = Workflow.State.NotStarted;
        public List<Approval> Approvals { get; set; } = new List<Approval>();
        public Revoke Revoke { get; set; }
        public int ApprovalLevelsNeeded { get; set; }
        public bool ApprovalsMet => Approvals.Count >= ApprovalLevelsNeeded;
    }
}
