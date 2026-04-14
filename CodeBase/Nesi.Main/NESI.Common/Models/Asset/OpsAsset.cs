namespace NESI.Common.Models
	{
	public struct OpsAsset
		{
		public struct HistoryTypes
			{
			public const int OilChange = 1;
			public const int BodyRepair = 2;
			public const int Brakes = 3;
			public const int Suspension = 4;
			public const int FiberglassRepair = 5;
			public const int TireChange = 6;
			public const int ACRepair = 7;
			public const int BatteryChange = 8;
			public const int RepairMaintenance = 9;
			public const int Calibration = 10;
			}
		public struct DBColumns
			{
			public const string assets_id				= "assets_id";
			public const string assets_no				= "assets_no";
			public const string assets_make				= "assets_make";
			public const string assets_model			= "assets_model";
			public const string assets_year				= "assets_year";
			public const string assets_license			= "assets_license";
			public const string assets_companyid		= "assets_companyid";
			public const string assets_type				= "assets_type";
			public const string assets_notes			= "assets_notes";
			public const string assets_statusid			= "assets_statusid";
			public const string assets_iconpic			= "assets_iconpic";
			public const string assets_owner			= "assets_owner";
			public const string vin_no					= "vin_no";
			public const string transponder				= "transponder";
			public const string mileage					= "mileage";
			public const string capped					= "capped";
			public const string parent_asset_id			= "parent_asset_id";
			public const string business_unit_id		= "business_unit_id";
			public const string ts						= "ts";
			public const string show_on_scheduler		= "show_on_scheduler";
			public const string lease_no				= "lease_no";
			public const string daily					= "daily";
			public const string weekly					= "weekly";
			public const string monthly					= "monthly";
			public const string description				= "description";
			public const string calib_freq				= "calib_freq";
			public const string next_calib_date			= "next_calib_date";
			public const string requires_maintenance	= "requires_maintenance";
			public const string needs_calib				= "needs_calib";
			public const string active					= "active";
			public const string master_id				= "master_id";
			}
		}
	}