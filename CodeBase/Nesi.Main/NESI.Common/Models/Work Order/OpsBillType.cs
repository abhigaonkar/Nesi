namespace NESI.Common.Models
{

	//
	// The definition for below from [1124]:
	// US 112 - Preventing costs from being entered on WO Detail & Removal/Replacement of 9595/9596 logic
	//
	public struct OpsBillType
	{
		/// <summary>
		/// 0
		/// </summary>
		public const int Regular = 0;
		/// <summary>
		/// 1
		/// </summary>
		public const int JobcostForQuote = 1;
		/// <summary>
		/// 2
		/// </summary>
		public const int InvisibleCredit = 2;
		/// <summary>
		/// 3
		/// </summary>
		public const int QuotedPriceOld = 3;
		/// <summary>
		/// 4
		/// </summary>
		public const int Blended = 4;
		/// <summary>
		/// 5
		/// </summary>
		public const int DoNotInclude = 5;
		/// <summary>
		/// 6
		/// </summary>
		public const int Comment = 6;
		/// <summary>
		/// 7
		/// </summary>
		public const int VisibleNoCharge = 7;
		/// <summary>
		/// 8
		/// </summary>
		public const int Blank = 8;
		/// <summary>
		/// 9
		/// </summary>
		public const int ProgressBillingOld = 9;
		/// <summary>
		/// 10
		/// </summary>
		public const int VisibleCredit = 10;
		/// <summary>
		/// 11
		/// </summary>
		public const int QuotedPrice = 11;
		/// <summary>
		/// 12
		/// </summary>
		public const int ProgressBilling = 12;
		/// <summary>
		/// 13
		/// </summary>
		public const int BalanceForward = 13;
	}
}