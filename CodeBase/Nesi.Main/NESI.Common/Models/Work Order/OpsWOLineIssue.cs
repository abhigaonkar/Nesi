namespace NESI.Common.Models
	{
	    public struct OpsWOLineIssue
		    {
			public const string CostLessThanSell          = "The Cost is Higher than the Sell Price - Loss of Money";
			public const string ZeroCommitted             = "Quantity Committed is zero";
			public const string NegativeCommitted         = "Negative Quantity Alert";
			public const string TemporaryPartUsed         = "Temporary part used (777)";
			public const string SellLessThanZero          = "Sellprice less than 0";
			public const string SellLessThanMinSellPrice  = "The sellprice is less than the required sellprice of: {0}";
			public const string LaborTimeSheetDiscrepancy = "Time Sheet Discrepency, time from time sheet is: {0}";
			}
	}