namespace NESI.Common.Models
	{
	public struct OpsPOStatus
		{
		/// <summary>
		/// ID #1
		/// </summary>
		public const int NotIssued = 1;

		/// <summary>
		/// ID #2
		/// </summary>
		public const int WaitingBMApproval = 2;

		/// <summary>
		/// ID #3
		/// </summary>
		public const int IssuedWaitingforPackingSlip = 3;

		/// <summary>
		/// ID #4
		/// </summary>
		public const int ReceivedWaitingforInvoice = 4;

		/// <summary>
		/// ID #5
		/// </summary>
		public const int ApprovedtoOrder = 5;

		/// <summary>
		/// ID #6
		/// </summary>
		public const int WaitingToBeClosed = 6;

		/// <summary>
		/// ID #7
		/// </summary>
		public const int Closed = 7;

		/// <summary>
		/// ID #8
		/// </summary>
		public const int Cancelled = 8;

		/// <summary>
		/// ID #9
		/// </summary>
		public const int Questions = 9;

		/// <summary>
		/// ID #10
		/// </summary>
		public const int APProblems = 10;

		/// <summary>
		/// ID #11
		/// </summary>
		public const int IssuedWaitingforVendorConfirmation = 11;

		/// <summary>
		/// ID #12
		/// </summary>
		public const int IssuedWaitingforCompleteDelivery = 12;

		}
	}

