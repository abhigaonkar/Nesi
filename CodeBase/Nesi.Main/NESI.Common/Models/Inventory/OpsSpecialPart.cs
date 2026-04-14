namespace NESI.Common.Models
	{

	//
	// The definition for below from [1124]:
	// US 112 - Preventing costs from being entered on WO Detail & Removal/Replacement of 9595/9596 logic
	//
	public struct OpsSpecialPart
		{
		/// <summary>
		/// 777
		/// </summary>
		public const int MakeThisPart = 777;
		/// <summary>
		/// 2139
		/// </summary>
		public const int QuoteLine = 2139;
		/// <summary>
		/// 9595
		/// </summary>
		public const int OldSubContractor = 9595;
		/// <summary>
		/// 55000
		/// </summary>
		public const int NonInventoryMaterial = 55000;
		/// <summary>
		/// 55555
		/// </summary>
		public const int SubContractor = 55555;
		/// <summary>
		/// 55556
		/// </summary>
		public const int ExpenseReimbursement = 55556;
		/// <summary>
		/// 55557
		/// </summary>
		public const int PerDiem = 55557;
		/// <summary>
		/// 55558
		/// </summary>
		public const int CompanyCreditCardExpense = 55558;
		/// <summary>
		/// 55559
		/// </summary>
		public const int NewChildWO = 55559;
		/// <summary>
		/// 55560
		/// </summary>
		public const int MiscMaterial = 55560;
		/// <summary>
		/// 60000
		/// </summary>
		public const int AssetLine = 60000;
		/// <summary>
		/// 990000
		/// </summary>
		public const int LaborThreshold = 990000; // MH: This is fairly arbitrary and will need to be redone... for now I want to at least normalize the range

		}
	}