namespace NESI.Common.Models
{
	public struct OpsLog
	{
		public struct Table
			{
			public const string Assets							= "assets";
			public const string BreakTime						= "breaktime";
			public const string CustomerRate					= "customer_rate";
			public const string InventoryBranch					= "inventory_branch";
			public const string InventoryBranchOptions			= "inventory_branch_options";
			public const string InventoryLocation				= "inventory_location";
			public const string InventoryLocationMaster			= "inventory_location_master";
			public const string MemberPage						= "memberpage";
			public const string MemberPagePrivilege				= "memberpageprivilege";
			public const string MemberTypePage					= "membertypepage";
			public const string MemberTypePagePrivilege			= "membertypepageprivilege";
			public const string QuoteMaster						= "quote_master";
			}
		public struct Section
			{
			public const int Unknown                            = 0;
			public const int LocationTab                        = 2;
			public const int AdjustingSellPrice                 = 3;
			public const int QuoteGeneral                       = 4;
			public const int QuoteWorksheet                     = 5;
			public const int WorkOrderDetail                    = 8;
			public const int LocationMaster                     = 9;
			public const int AnnualUpdate                       = 10;
			public const int MobileAdminTab                     = 11;
			public const int MobileTransferTab                  = 12;
			public const int CustomerRates                      = 13;
			public const int ManualAdjustmentMergingOldLocation = 14;
			public const int BranchInventory                    = 15;
			public const int PrivilegeClass                     = 16;
			public const int PageClass                          = 17;
			public const int InvoiceServicesWorkOrder           = 18;
			public const int PartManagement                     = 19;
			public const int TimeSheet                          = 20;
			public const int AssetsFleet                        = 21;
			public const int AssetsTooling                      = 22;
			}
		public struct Action
			{
			public const int AdjustedMinimumQuantity           = 1;
			public const int AdjustedMaximumQuantity           = 2;
			public const int AdjustedLocationQuantity          = 3;
			public const int StatusOfQuoteChanged              = 4;
			public const int AdjustedDollarBalance             = 6;
			public const int AdjustedWoprog_Id                 = 7;
			public const int AdjustedRecNumber                 = 8;
			public const int AdjustedType                      = 9;
			public const int AdjustedMasterId                  = 10;
			public const int AdjustedDateAdded                 = 11;
			public const int AdjustedDateModified              = 12;
			public const int AdjustedDateRequired              = 13;
			public const int AdjustedDescription               = 14;
			public const int AdjustedQuantityCommitted         = 15;
			public const int AdjustedQuantityInvoiced          = 16;
			public const int AdjustedQuantityOrdered           = 17;
			public const int AdjustedPriceCost                 = 18;
			public const int AdjustedPriceSell                 = 19;
			public const int AdjustedPriceUnit                 = 20;
			public const int AdjustedAddedBy                   = 21;
			public const int AdjustedTax1                      = 22;
			public const int AdjustedTax2                      = 23;
			public const int AdjustedTax3                      = 24;
			public const int AdjustedTax4                      = 25;
			public const int AdjustedCompanyId                 = 26;
			public const int AdjustedBvWoNumber                = 27;
			public const int AdjustedCode                      = 28;
			public const int AdjustedOrigin                    = 29;
			public const int AdjustedBillTypeId                = 30;
			public const int AdjustedIssues                    = 31;
			public const int AdjustedMemberId                  = 32;
			public const int AdjustedPayTypeId                 = 33;
			public const int AdjustedNotes                     = 34;
			public const int AdjustedDiscountPercentage        = 35;
			public const int AdjustedTrackPartFlag             = 36;
			public const int AdjustedConsignmentId             = 37;
			public const int ChangedLocationName               = 38;
			public const int ChangedLocationType               = 39;
			public const int AdjustedChargeOutRate             = 40;
			public const int AppendedOldQuantity               = 41;
			public const int AdjustedBranchOnHandQuantity      = 42;
			public const int ActivatedManualAdjustmentPeriod   = 43;
			public const int DeactivatedManualAdjustmentPeriod = 44;
			public const int DeletedUserPrivilege              = 45;
			public const int AddedUserPrivilege                = 46;
			public const int DeletedUserPage                   = 47;
			public const int AddedUserPage                     = 48;
			public const int SendPoReminderEmail               = 49;
			public const int ZeroedOutLocation                 = 50;
			public const int AdjustedBreakTimeRecord           = 51;
			public const int FleetNumberAdjusted               = 52;
			public const int FleetMakeAdjusted                 = 53;
			public const int FleetModelAdjusted                = 54;
			public const int FleetYearAdjusted                 = 55;
			public const int FleetPlateNumberAdjusted          = 56;
			public const int FleetOwnerAdjusted                = 57;
			public const int FleetTransponderAdjusted          = 58;
			public const int FleetMileageAdjusted              = 59;
			public const int FleetVinNumberAdjusted            = 60;
			public const int FleetBranchAdjusted               = 61;
			}
	}
}