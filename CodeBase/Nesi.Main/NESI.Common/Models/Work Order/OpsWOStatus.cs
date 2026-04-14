using System;

namespace NESI.Common.Models
{

    public struct OpsWOStatus
    {
        public const string ClosedReassigned            = "Closed/Reassigned";
        public const string Deleted                     = "Deleted";
        public const string InitialPrep                 = "Initial Prep";
        public const string Invoiced                    = "Invoiced";
        public const string Open                        = "Open";
        public const string QuestionsForPM              = "Questions For PM";
        public const string Rework                      = "Rework";
        public const string JustScanned                 = "Just Scanned";
        public const string WaitingBMApproval           = "Waiting BM Approval";
        public const string WaitingForParts             = "Waiting for Parts";
        public const string WaitingForPO                = "Waiting For PO";
        public const string WaitingPMApproval           = "Waiting PM Approval";
        public const string WaitingParentBMApproval     = "Waiting Parent BM Approval";
        public const string WaitingToBeInvoiced         = "Waiting To Be Invoiced";
		public enum Enums
			{
			ClosedReassigned,
			Deleted,
			InitialPrep,
			Invoiced,
            JustScanned,
			Open,
			QuestionsForPM,
			Rework,
			WaitingBMApproval,
			WaitingForParts,
			WaitingForPO,
			WaitingPMApproval,
			WaitingParentBMApproval,
			WaitingToBeInvoiced
			}
    }
}