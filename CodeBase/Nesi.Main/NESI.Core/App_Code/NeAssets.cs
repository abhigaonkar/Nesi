using System;
using System.Data;
using MySql.Data.MySqlClient;
using NESI.Common.Models;

namespace nesi.core
	{
	/// <summary>
	/// Assets
	/// </summary>
	public class NeAssets
		{
		public int Id { get; set; }
		public string Number { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public int Year { get; set; }
		public string License { get; set; }
		public int BusinessUnitId { get; set; }
		public string Notes { get; set; }
		public int Type { get; set; }
		public int StatusId { get; set; }
		public int Owner { get; set; }
		public string Transponder { get; set; }
		public string VIN { get; set; }
		public double Mileage { get; set; }
		public bool Capped { get; set; }
        public bool ShowOnScheduler { get; set; }
        public string LeaseNumber { get; set; }
        public int ParentAssetId { get; set; }
		public string IconPic { get; set; }
        public string Description { get; set; }
        public decimal Daily { get; set; }
        public decimal Weekly { get; set; }
        public decimal Monthly { get; set; }
        public int CalibrationFrequency { get; set; }
        public DateTime? NextCalibrationDate { get; set; }
        public bool RequiresMaintenance { get; set; }
        public bool NeedsCalibration { get; set; }
        public bool Active { get; set; }
		private MySqlCommand comm;
		private NeAssets OriginalValues { get; set;}

		public NeAssets()
			{

			}
        public NeAssets(int id)
			{
			Load(id);
			}

		public void Load(int id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM assets WHERE assets_id = @v0 ", new object[] {  id } );
			foreach (DataRow dr in dt.Rows)
				{
				Id					    = (int) dr[OpsAsset.DBColumns.assets_id];
				Description			    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.description]);
				IconPic				    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.assets_iconpic]);
				LeaseNumber			    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.lease_no]);
				License				    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.assets_license]);
				Make				    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.assets_make]);
				Model				    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.assets_model]);
				Number				    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.assets_no]);
				Notes				    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.assets_notes]);
				Transponder			    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.transponder]);
				VIN					    = Toolbox.ReturnBlankIfNull_string(dr[OpsAsset.DBColumns.vin_no]);

				BusinessUnitId		    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.business_unit_id]);
				Mileage				    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.mileage]);
				Owner				    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.assets_owner]);
				ParentAssetId		    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.parent_asset_id]);
				StatusId			    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.assets_statusid]);
				Type				    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.assets_type]);
				Year				    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.assets_year]);

				Capped				    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.capped]) == 1;
				ShowOnScheduler		    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.show_on_scheduler]) == 1;
				RequiresMaintenance	    = Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.requires_maintenance]) == 1;
				Active					= Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.active]) == 1;
				NeedsCalibration		= Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.needs_calib]) == 1;
				NextCalibrationDate		= Toolbox.ReturnNullDateTime(dr[OpsAsset.DBColumns.next_calib_date]);
				CalibrationFrequency	= Toolbox.ReturnZeroIfNull_int(dr[OpsAsset.DBColumns.calib_freq]);
				Daily                   = Toolbox.ReturnZeroIfNull_decimal(dr[OpsAsset.DBColumns.daily]);
				Weekly                  = Toolbox.ReturnZeroIfNull_decimal(dr[OpsAsset.DBColumns.weekly]);
				Monthly				    = Toolbox.ReturnZeroIfNull_decimal(dr[OpsAsset.DBColumns.monthly]);
				OriginalValues			= this;
				}
			}
		private void TrackChanges(NeMember changedByMember, NeAssets oldValues, string parameterName, object parameterValue, bool isTracked, bool isFleet)
			{
			if(isTracked && oldValues.Id > 0)
				{
				var log = new NELog {	section_id = isFleet ? OpsLog.Section.AssetsFleet : OpsLog.Section.AssetsTooling, 
										table = OpsLog.Table.Assets, 
										table_id = oldValues.Id, 
										member_id = changedByMember.id,
										business_unit_id = changedByMember.business_unit_id,
										is_manual = true
										};
				switch(parameterName)
					{
					case "no":
						log.action_id = OpsLog.Action.FleetNumberAdjusted;
						log.value_old = oldValues.Number;
						log.value_new = Number;
					break;
					case "make":
						log.action_id = OpsLog.Action.FleetMakeAdjusted;
						log.value_old = oldValues.Make;
						log.value_new = Make;
					break;
					case "model":
						log.action_id = OpsLog.Action.FleetModelAdjusted;
						log.value_old = oldValues.Model;
						log.value_new = Model;
					break;
					case "year":
						log.action_id = OpsLog.Action.FleetYearAdjusted;
						log.value_old = oldValues.Year;
						log.value_new = Year;
					break;
					case "license":
						log.action_id = OpsLog.Action.FleetPlateNumberAdjusted;
						log.value_old = oldValues.License;
						log.value_new = License;
					break;
					case "owner":
						log.action_id = OpsLog.Action.FleetOwnerAdjusted;
						log.value_old = oldValues.Owner == 0 ? OpsGeneralString.Spare : new NeMember(oldValues.Owner).FullName;
						log.value_new = Owner == 0 ? OpsGeneralString.Spare : new NeMember(Owner).FullName;
					break;
					case "transponder":
						log.action_id = OpsLog.Action.FleetTransponderAdjusted;
						log.value_old = oldValues.Transponder;
						log.value_new = Transponder;
					break;
					case "mileage":
						log.action_id = OpsLog.Action.FleetMileageAdjusted;
						log.value_old = oldValues.Mileage;
						log.value_new = Mileage;
					break;
					case "vin_no":
						log.action_id = OpsLog.Action.FleetVinNumberAdjusted;
						log.value_old = oldValues.VIN;
						log.value_new = VIN;
					break;
					case "business_unit_id":
						log.action_id = OpsLog.Action.FleetBranchAdjusted;
						log.value_old = oldValues.BusinessUnitId == 0 ? OpsGeneralString.Blank : new NeBusinessUnit(oldValues.BusinessUnitId).ddl_name;
						log.value_new = BusinessUnitId == 0 ? OpsGeneralString.Blank : new NeBusinessUnit(BusinessUnitId).ddl_name;
					break;
					}
				if(log.value_old != null && log.value_new != null && log.value_old.ToString() != log.value_new.ToString())
					{ 
					log.save();
					}
				}
			comm.Parameters.AddWithValue(OpsGeneralString.QuestionMark+parameterName, parameterValue);
			}
		public void Save(NeMember currentMember)
			{
			var oldValues = Id == 0 ? new NeAssets() : new NeAssets(Id);
			var isFleet = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(is_fleet),0) FROM assets_type WHERE asset_type_id = @v0", new object[]{ Type }) == 1;
			if(!oldValues.NeedsCalibration && NeedsCalibration)
				{
				NextCalibrationDate	= DateTime.Now.Date.AddDays(CalibrationFrequency);
				}

			using(var conn = Toolbox.connect())
				{
				#region Queries
				var sql			= Id == 0 
?
@"
INSERT INTO	assets 
	(
	assets_no,
	assets_make,
	assets_model,
	assets_year,
	assets_license,
	business_unit_id,
	assets_type,
	assets_statusid,
	assets_owner,
	parent_asset_id,
	mileage,
	capped,
	vin_no,
	transponder,
	assets_notes,
	show_on_scheduler,
	lease_no,
	daily,
	weekly,
	monthly,
	description,
	calib_freq,
	next_calib_date,
	requires_maintenance,
	needs_calib,
	active
	)
VALUES
	(
	?no,
	?make,
	?model,
	?year,
	?license,
	?business_unit_id,
	?type,
	?statusid,
	?owner,
	?parent_asset_id,
	?mileage,
	?capped,
	?vin_no,
	?transponder,
	?notes,
	?show_on_scheduler,
	?lease_no,
	?daily,
	?weekly,
	?monthly,
	?description,
	?calib_freq,
	?next_calib_date,
	?requires_maintenance,
	?needs_calib,
	?active
	);
SELECT LAST_INSERT_ID();"
: 
@"
UPDATE 
	assets 
SET 
	assets_no            = ?no,
	assets_make          = ?make,
	assets_model         = ?model,
	assets_year          = ?year,
	assets_license       = ?license,
	business_unit_id     = ?business_unit_id,
	assets_type          = ?type,
	assets_statusid      = ?statusid,
	assets_owner         = ?owner,
	parent_asset_id      = ?parent_asset_id,
	mileage              = ?mileage,
	capped               = ?capped,
	vin_no               = ?vin_no,
	transponder          = ?transponder,
	assets_notes         = ?notes,
	show_on_scheduler    = ?show_on_scheduler,
	lease_no             = ?lease_no,
	daily                = ?daily,
	weekly               = ?weekly,
	monthly              = ?monthly,
	description          = ?description,
	calib_freq           = ?calib_freq,
	next_calib_date      = ?next_calib_date,
	requires_maintenance = ?requires_maintenance,
	needs_calib          = ?needs_calib,
	active               = ?active
 WHERE 
	assets_id = ?id";
				#endregion Queries

				comm = new MySqlCommand(sql, conn);

				TrackChanges(currentMember, oldValues, "no", Number, true, isFleet);
				TrackChanges(currentMember, oldValues, "make", Make, true, isFleet);
				TrackChanges(currentMember, oldValues, "model", Model, true, isFleet);
				TrackChanges(currentMember, oldValues, "year", Year, true, isFleet);
				TrackChanges(currentMember, oldValues, "license", License, true, isFleet);
				TrackChanges(currentMember, oldValues, "business_unit_id", BusinessUnitId, true, isFleet);
				TrackChanges(currentMember, oldValues, "owner", Owner, true, isFleet);
				TrackChanges(currentMember, oldValues, "mileage", Mileage, true, isFleet);
				TrackChanges(currentMember, oldValues, "vin_no", VIN, true, isFleet);
				TrackChanges(currentMember, oldValues, "transponder", Transponder, true, isFleet);


				TrackChanges(currentMember, oldValues, "type", Type, false, isFleet);
				TrackChanges(currentMember, oldValues, "statusid", StatusId, false, isFleet);
				TrackChanges(currentMember, oldValues, "parent_asset_id", ParentAssetId, false, isFleet);
				TrackChanges(currentMember, oldValues, "capped", Capped, false, isFleet);
				TrackChanges(currentMember, oldValues, "notes", Notes, false, isFleet);
				TrackChanges(currentMember, oldValues, "id", Id, false, isFleet);
				TrackChanges(currentMember, oldValues, "show_on_scheduler", ShowOnScheduler, false, isFleet);
				TrackChanges(currentMember, oldValues, "lease_no", LeaseNumber, false, isFleet);
				TrackChanges(currentMember, oldValues, "daily", Daily, false, isFleet);
				TrackChanges(currentMember, oldValues, "weekly", Weekly, false, isFleet);
				TrackChanges(currentMember, oldValues, "monthly", Monthly, false, isFleet);
				TrackChanges(currentMember, oldValues, "description", Description, false, isFleet);
				TrackChanges(currentMember, oldValues, "calib_freq", CalibrationFrequency, false, isFleet);
				TrackChanges(currentMember, oldValues, "next_calib_date", NextCalibrationDate, false, isFleet);
				TrackChanges(currentMember, oldValues, "requires_maintenance", RequiresMaintenance, false, isFleet);
				TrackChanges(currentMember, oldValues, "needs_calib", NeedsCalibration, false, isFleet);
				TrackChanges(currentMember, oldValues, "active", Active, false, isFleet);

				if(Id == 0)
					{
					Id = Convert.ToInt32(comm.ExecuteScalar());
					}
				else
					{ 
					comm.ExecuteNonQuery();
					comm.Dispose();
					}
				}
			}
		public static void UpdateHistoryItem(int id, int assetId, int actionId, int memberId, string memo, DateTime date, double price = 0)
			{
			if(actionId == 0)
				{ actionId = 10; }
			Toolbox.doSQL_void(@"
UPDATE 
	assets_history 
SET 
	assets_history_memberid = @v0,
	assets_history_action   = @v1,
	assets_history_dollars  = @v2,
	assets_history_date     = @v3,
	assets_history_notes    = @v4  
WHERE 
	assets_history_id = @v5", 
			new object[] {
				memberId, 
				actionId,
				price, 
				date,
				memo,
				id
				});
			if(actionId == OpsAsset.HistoryTypes.Calibration)
				{
				var Asset = new NeAssets(assetId);
				if(Asset.NeedsCalibration && Asset.CalibrationFrequency > 0 && IsMaxCalibrationDate(assetId, id))
					{
					Asset.NextCalibrationDate = date.AddDays(Asset.CalibrationFrequency);
					Asset.Save(new NeMember(memberId));
					}
				}
			}
		public static void AddHistoryItem(int assetId, int actionId, int memberId, string memo, DateTime date, double price = 0)
			{
			var id = Toolbox.doSQL_return_id(@"
INSERT INTO assets_history 
	(
	assets_history_assetid,
	assets_history_action,
	assets_history_memberid,
	assets_history_notes,
	assets_history_date,
	assets_history_dollars
	) 
VALUES 
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5
	)", 
			new object[] {
				assetId, 
				actionId,
				memberId, 
				memo, 
				date,
				price
				});
			if(actionId == OpsAsset.HistoryTypes.Calibration)
				{
				var Asset = new NeAssets(assetId);
				if(Asset.NeedsCalibration && Asset.CalibrationFrequency > 0 && IsMaxCalibrationDate(assetId, id))
					{
					Asset.NextCalibrationDate = date.AddDays(Asset.CalibrationFrequency);
					Asset.Save(new NeMember(memberId));
					}
				}
			}
		public static bool IsMaxCalibrationDate(int assetId, int historyId)
			{
			var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM assets_history WHERE assets_history_assetid = @v0 AND assets_history_action = 10", new object[]{ assetId });
			if(c == 0) return true;
			return Toolbox.doSQL_int(@"SELECT assets_history_id FROM assets_history WHERE assets_history_assetid = @v0 AND assets_history_action = 10 ORDER BY assets_history_date DESC, assets_history_id DESC LIMIT 1", new object[]{ assetId }) == historyId;
			}
		public class WorkOrderUsage
			{
			public int Id { get; set; }
			public int WOProgId { get; set; }
			public int AssetId { get; set; }
			public DateTime DateNeeded { get; set; }
			public int AddedBy { get; set; }
			public WorkOrderUsage()
				{
				}
			public WorkOrderUsage(MySqlConnection _conn, int _id)
				{
				Load(_conn, _id);
				}
			public struct AssetWOLine
				{
				public NeWOProg WorkOrderObj { get;set;}
				public double Sell { get;set;}
				public double Qty { get;set;}
				public string Unit { get; set; }
				public int UnitId { get; set; }
				public WorkOrderUsage UsageObj { get; set; }
				public NeAssets AssetObj { get; set; }
				public NeMember MemberObj { get; set; }
				}
			public static void SaveWOLine(AssetWOLine woLine)
				{
				var wodc = new NeWODetailCurrent
					{
					 woprog_id        = woLine.WorkOrderObj.woprog_id,
					 type             = OpsWOLineType.Asset,
					 sell             = woLine.Sell,
					 cost             = 0.00,
					 asset_id         = woLine.UsageObj.Id,
					 asset_unit       = woLine.UnitId,
					 description      = woLine.AssetObj.Description + " - " + woLine.Unit,
					 master_id        = OpsSpecialPart.AssetLine,
					 code             = OpsSpecialPart.AssetLine.ToString(),
					 billtypeid       = woLine.WorkOrderObj.QuoteID == OpsGeneralString.Zero ? 0 : 1,
					 origin           = OpsWOLineOrigin.AssetsTab,
					 business_unit_id = woLine.WorkOrderObj.business_unit_id,
					 added_by         = woLine.MemberObj.id,
					 date_added       = Toolbox.MySQLNow_long(),
					 date_required    = Toolbox.MySQLNow_long(),
					 discount         = 0,
					 qty_committed    = woLine.Qty,
					 qty_invoiced     = woLine.Qty,
					 qty_ordered      = woLine.Qty,
					 is_active        = true,
					 added_by_module  = "Asset Tab",
					 bvwo             = woLine.WorkOrderObj.woprog_id
					};
				wodc.save(woLine.MemberObj, "Asset Tab", true);
				}
			public void Load(MySqlConnection _conn, int _id)
				{
				Id = _id;
				var dt = Toolbox.doSQL_dt(_conn, @"SELECT * FROM woprog_asset WHERE id = @v0", new object[] {_id});
				foreach(DataRow dr in dt.Rows)
					{
					WOProgId   = (int) dr["woprog_id"];
					AssetId    = (int) dr["asset_id"];
					DateNeeded = (DateTime) dr["date_needed"];
					AddedBy    = (int) dr["added_by"];
					}
				}
			public void Save(MySqlConnection _conn)
				{
				if(Id == 0)
					{ 
					Toolbox.doSQL_void(_conn, @"INSERT INTO woprog_asset (woprog_id, asset_id, date_needed, added_by) VALUES (@v0, @v1, @v2, @v3)", new object[]{ WOProgId, AssetId, DateNeeded.Date, AddedBy});
					}
				}
			public void Delete(MySqlConnection _conn)
				{
				if(Id > 0)
					{
					Toolbox.doSQL_void(_conn, @"DELETE FROM woprog_asset WHERE id = @v0 LIMIT 1", new object[]{ Id });
					}
				}
			}
		}
	}