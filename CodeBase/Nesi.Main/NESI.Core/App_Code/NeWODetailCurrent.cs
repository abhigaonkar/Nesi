using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using DevExpress.Xpo;
using System.Linq;
using ne_xpo.cs;
using NESI.Common.Models;

namespace nesi.core
	{
	/// <summary>
	/// Interface for viewing and manipulating data in the wo_detail_current table
	/// </summary>
	public class NeWODetailCurrent
		{
	#region Variable Declaration
		public string added_by_module { get; set; }
		public string issues { get; set; }
		public double qty_ordered { get; set; }
		public int id { get; set; }
		public int woprog_id { get; set; }
		public int rec_no { get; set; }
		public string type { get; set; }
		public int master_id { get; set; }
		public string date_added { get; set; }
		public string date_modified { get; set; }
		public string date_required { get; set; }
		public string description { get; set; }
		public double qty_committed { get; set; }
		public double qty_invoiced { get; set; }
		public string notes { get; set; }
		public double cost { get; set; }
		public double sell { get; set; }
		public double unit { get; set; }
		public int added_by { get; set; }
		public int tax1 { get; set; }
		public int tax2 { get; set; }
		public int tax3 { get; set; }
		public int tax4 { get; set; }
		public int business_unit_id { get; set; }
		public int bvwo { get; set; }
		public int billtypeid { get; set; }
		public int consignment_id { get; set; }
		public int memberid { get; set; }
		public int paytypeid { get; set; }
		public string code { get; set; }
		public string origin { get; set; }
		public double discount { get; set; }
		public int track_part { get; set; }
		public int location_id { get; set; }
		public bool is_active { get; set; }
		public bool blocks_schedule { get; set; }

		// Handlers for transfers and receivals
		public bool is_transferring_from { get; set; }
		public bool is_receiving_from { get; set; }
		public bool is_transferring_to { get; set; }
		public string receiving_from_bvpo { get; set; }
		public string transferring_from_bvwo { get; set; }
		public string transferring_to_bvwo { get; set; }
		public string committing_from_location { get; set; }
		public string uncommitting_to_location { get; set; }
		public int child_woprog_id {get; set;}
        public int? seg1_id { get; set; }
		public int? asset_id { get; set; }
		public int? asset_unit { get; set; }
		public bool PartiallyBilled { get; set; }

		public string ActivityCode { get; set; }
		public string CostElement { get; set; }
		public string ClientWO { get; set; }
		public string ClientPO { get; set; }


		public NeWODetailCurrent() { }
		public NeWODetailCurrent(int _id)
		{
			load(_id);
		}
		public NeWODetailCurrent(int _id, MySqlConnection connection = null)
		{
			load(_id, connection);
		}

		public NeWODetailCurrent(int _member_id, object _paytype_id, object _woprog_id, object _labor_master_id, NeMemberType _membertype = null, int parentWO = 0)
		{
				var _membertype_id = 0;
				if(_membertype != null)
					{
					_membertype_id = _membertype.id;
					}
				var chargeoutId = Toolbox.ReturnZeroIfNull_int(_labor_master_id);
				if(parentWO != 0)
					{ 
					var businessUnitId = Toolbox.doSQL_int(@"SELECT business_unit_id FROM woprog WHERE woprog_id = @v0", new object[]{parentWO});
					if(_membertype_id != 0)
						{
						var chargeoutObj = new Chargeout("MT", businessUnitId, _membertype_id, Toolbox.ReturnZeroIfNull_int(_paytype_id));
						chargeoutId = chargeoutObj.id;
						_labor_master_id = chargeoutId;
						}
					if(chargeoutId == 0)
						{
						throw new Exception("There was an issue trying to retrieve the chargeout ID for this labor entry.");
						}
					}

                var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_type = 'L' AND memberid = @v0  AND paytypeid = @v1  AND wo_detail_current_woprog_id = @v2  AND wo_detail_current_origin = 'Entered From Timesheet' AND wo_detail_current_master_id = @v3 ", new [] { _member_id, _paytype_id, _woprog_id, _labor_master_id });
                if (c == 1)
                {
                    var _id = Toolbox.doSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_type = 'L' AND memberid = @v0  AND paytypeid = @v1  AND wo_detail_current_woprog_id = @v2  AND wo_detail_current_origin = 'Entered From Timesheet' AND wo_detail_current_master_id = @v3", new [] { _member_id, _paytype_id, _woprog_id, _labor_master_id });
                    load(_id);
                }
                else // Prep the submitted variables
                {
                    memberid = _member_id;
                    paytypeid = Toolbox.ReturnZeroIfNull_int(_paytype_id);
                    woprog_id = Toolbox.ReturnZeroIfNull_int(_woprog_id);
                    master_id = Toolbox.ReturnZeroIfNull_int(_labor_master_id);
                }
            
            
        }
        public NeWODetailCurrent(NeMemberTime mt, int _labor_master_id,bool _is_parent=false)
        {
            var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_type='K' AND seg1_id=@v0", new object[] { mt.ID });
            if (c == 1)
            {
                var _id = Toolbox.doSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_type='K' AND seg1_id=@v0", new object[] { mt.ID});
                load(_id);
            }
			else if (c == 2) // its an interco, target the child
			{
			    var _id = _is_parent
							? Toolbox.doSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_type='K' AND seg1_id=@v0 ANd wo_detail_current_woprog_id = @v1", new object[] { mt.ID ,mt.membertime_child_woprog_id})
							: Toolbox.doSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_type='K' AND seg1_id=@v0 ANd wo_detail_current_woprog_id = @v1", new object[] { mt.ID ,mt.membertime_woprog_id});
                load(_id);
			}
            else
            {
                memberid = mt.member_id;
                paytypeid = Toolbox.ReturnZeroIfNull_int(mt.PayTypeHoursID);
                woprog_id = Toolbox.ReturnZeroIfNull_int(mt.woprog_id);
                master_id = Toolbox.ReturnZeroIfNull_int(_labor_master_id);
            }

        }
		public static double CurrentCommittedQuantity(int _id)
			{
			return Toolbox.doSQL_double(@"SELECT IFNULL(SUM(qty_committed),0) FROM wo_detail WHERE id = @v0", new object[] { _id });
			}

		public static double CurrentCommittedQuantity(int _woprog_id, int _master_id)
			{
			return Toolbox.doSQL_double(@"SELECT IFNULL(SUM(qty_committed),0) FROM wo_detail WHERE woprog_id = @v0 AND master_id = @v1", new object[] { _woprog_id, _master_id });
			}

            #endregion Variable Declaration
		#region Void (13)
		private void AdjustParentLaborCost(NeMember user, bool is_manual, NeWOProg childWo, MySqlConnection conn = null, MySqlTransaction transaction = null)
			{
			var children = NeWOProg.GetChildren(childWo.parent_woprog_id);
			if(children.Length <= 0) return;
			var chargeOut        = new Chargeout(master_id);
			var openChildren     = NeWOProg.GetOpenChildren(childWo.parent_woprog_id, false);
			var invoicedChildren = NeWOProg.GetInvoicedChildren(childWo.parent_woprog_id, false);
			var singleChildEntry = children.Length == 1 && children[0] == childWo.woprog_id;

			var childOpenRows     = Toolbox.doSQL_dt(@"SELECT id, price_sell, qty_committed FROM wo_detail WHERE FIND_IN_SET(woprog_id, @v0) AND memberid = @v1 AND membertype_id = @v2 AND paytypeid = @v3 AND billtypeid != @v4", new object[]{string.Join(",", openChildren), memberid, chargeOut.membertype_id, paytypeid, OpsBillType.DoNotInclude});
			var childInvoicedRows = Toolbox.doSQL_dt(@"SELECT id, price_sell, qty_committed FROM wo_detailh WHERE FIND_IN_SET(woprog_id, @v0) AND memberid = @v1 AND membertype_id = @v2 AND paytypeid = @v3 AND billtypeid != @v4", new object[]{string.Join(",", invoicedChildren), memberid, chargeOut.membertype_id, paytypeid, OpsBillType.DoNotInclude});
			var countRows = childOpenRows.Rows.Count + childInvoicedRows.Rows.Count;
			var totalSell = default(double);
			var totalQty = default(double);
			foreach(DataRow dr in childOpenRows.Rows)
				{
				var subQty = Convert.ToDouble(dr["qty_committed"]);
				var subSell = Convert.ToDouble(dr["price_sell"]);
				totalSell += subQty*subSell;
				totalQty += subQty;
				}
			foreach(DataRow dr in childInvoicedRows.Rows)
				{
				var subQty  = Convert.ToDouble(dr["qty_committed"]);
				var subSell = Convert.ToDouble(dr["price_sell"]);
				totalSell += subQty*subSell;
				totalQty += subQty;
				}
			var averageLaborCost = singleChildEntry 
										? billtypeid == OpsBillType.DoNotInclude 
											? 0 
											: sell 
										: countRows == 0 
											? 0
											: totalSell / totalQty;
			var line = new NeWODetailCurrent(memberid, paytypeid, childWo.parent_woprog_id, master_id, new NeMemberType(chargeOut.membertype_id), childWo.parent_woprog_id);
			if(line.id == 0 || line.PartiallyBilled) return;

			// These need to be inverted due to how quantities work when saving WO lines.
			line.qty_committed = countRows == 0 ? line.qty_committed * -1 : totalQty - line.qty_committed;
			line.qty_ordered = countRows == 0 ? line.qty_ordered * -1 :totalQty - line.qty_ordered;
			line.qty_invoiced = countRows == 0 ? line.qty_invoiced * -1 :totalQty - line.qty_invoiced;

			line.cost = averageLaborCost;
			line.save(user, "", is_manual, conn, transaction);
			} 
		public void save(NeMember user, string by_module, bool is_manual, MySqlConnection conn = null, MySqlTransaction transaction = null)
			{
			using(var my_conn = Toolbox.connect())
				{
				var my_comm = new MySqlCommand { Connection = conn ?? my_conn };
			var is_updating = id != 0;
			var prev_values = new NeWODetailCurrent();
			var WorkOrderHeader = new NeWOProg(woprog_id);

            if (!is_updating)
			{
				//Insert
				// Prep a few variables
				if (conn == null)
					rec_no = Toolbox.doSQL_int(my_conn, @"SELECT IFNULL(MAX(wo_detail_current_rec_no), 1)+1 FROM wo_detail_current WHERE wo_detail_current_woprog_id=@v0 ", new object[] { woprog_id });
				else
					rec_no = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(wo_detail_current_rec_no), 1)+1 FROM wo_detail_current WHERE wo_detail_current_woprog_id=@v0 ", new object[] { woprog_id });
					//TODO: Refactor the date_required_check... don't think it's necessary anymore.
					date_required_check(null);
				if (master_id == OpsSpecialPart.QuoteLine)
				{
					if (!WorkOrderHeader.IsRebill && !WorkOrderHeader.IsCredit && !Toolbox.Contains(billtypeid, new[] {	OpsBillType.ProgressBillingOld, 
																						OpsBillType.QuotedPrice, 
																						OpsBillType.ProgressBilling }))
					{
						billtypeid = OpsBillType.QuotedPrice;
					}
					else if ((WorkOrderHeader.IsRebill || WorkOrderHeader.IsCredit) && !Toolbox.Contains(billtypeid, new[] {		OpsBillType.InvisibleCredit, 
																															OpsBillType.ProgressBillingOld, 
																															OpsBillType.VisibleCredit, 
																															OpsBillType.ProgressBilling, 
																															OpsBillType.QuotedPrice
																															}))
						{
						billtypeid = OpsBillType.ProgressBilling; // Not part of 
						}
				}
				my_comm.CommandText = @"
INSERT INTO wo_detail_current
	(
    wo_detail_current_woprog_id,
    wo_detail_current_rec_no,
    wo_detail_current_type,
    wo_detail_current_master_id,
    wo_detail_current_date_added,
    wo_detail_current_description,
    wo_detail_current_qty_committed,
    wo_detail_current_qty_invoiced,
    wo_detail_current_price_cost,
    wo_detail_current_price_sell,
    wo_detail_current_price_unit,
    wo_detail_current_added_by,
    business_unit_id,
    wo_detail_current_bvwo,
    wo_detail_current_code,
    wo_detail_current_origin,
    wo_detail_current_billtypeid,
    wo_detail_current_issues,
    memberid,
    paytypeid,
    wo_detail_current_notes,
    wo_detail_current_qty_ordered,
    wo_detail_current_discount,
    wo_detail_current_track_part,
    wo_detail_current_date_required,
    wo_detail_current_consignment_id,
    wo_detail_current_location_id,
    wo_detail_current_added_by_module,
    is_active,
    seg1_id,
	asset_id,
	asset_unit,
	child_woprog_id
	) 
VALUES
    (
    @woprog_id,
    @rec_no,
    @type,
    @master_id,
	NOW(),
    @description,
    @qty_committed,
    @qty_invoiced,
    @price_cost,
    @price_sell,
    @price_unit,
    @added_by,
    @business_unit_id,
    @bvwo,
    @code,
    @origin,
    @billtypeid,
    @issues,
    @memberid,
    @paytypeid,
    @notes,
    @qty_ordered,
    @discount,
    @track_part,
    @date_required,
    @consignment_id,
    @location_id,
    @added_by_module,
    @is_active,
    @seg1_id,
	@asset_id,
	@asset_unit,
	@child_woprog_id
    )";
			}
			else
			{
				prev_values = new NeWODetailCurrent(id, conn ?? my_conn);
				//Update
				my_comm.CommandText = @"
UPDATE 
    wo_detail_current 
SET
    wo_detail_current_woprog_id			= @woprog_id,
    wo_detail_current_rec_no			= @rec_no,
    wo_detail_current_type				= @type,
    wo_detail_current_master_id			= @master_id,
    wo_detail_current_description		= @description,
    wo_detail_current_qty_committed		= @qty_committed,
    wo_detail_current_qty_invoiced		= @qty_invoiced,
    wo_detail_current_price_cost		= @price_cost,
    wo_detail_current_price_sell		= @price_sell,
    wo_detail_current_price_unit		= @price_unit,
    business_unit_id					= @business_unit_id,
    wo_detail_current_bvwo				= @bvwo,
    wo_detail_current_code				= @code,
    wo_detail_current_origin			= @origin,
    wo_detail_current_billtypeid		= @billtypeid,
    wo_detail_current_issues			= @issues,
    paytypeid							= @paytypeid,
    wo_detail_current_notes				= @notes,
    wo_detail_current_qty_ordered		= @qty_ordered,
    wo_detail_current_discount			= @discount,
    wo_detail_current_track_part		= @track_part,
    wo_detail_current_date_required		= @date_required,
    wo_detail_current_consignment_id	= @consignment_id,
    wo_detail_current_location_id		= @location_id,
    is_active							= @is_active,
    seg1_id								= @seg1_id,
	asset_id							= @asset_id,
	asset_unit							= @asset_unit,
	activity_code						= @activity_code,
	cost_element						= @cost_element,
	client_wo							= @client_wo,
	client_po							= @client_po,
	child_woprog_id						= @child_woprog_id
WHERE 
	wo_detail_current_id = @id";
			}
			//TODO: Refactor the date_required_check... don't think it's necessary anymore.
			date_required_check(null);
			if (master_id == OpsSpecialPart.QuoteLine)
				{
				if (WorkOrderHeader.IsProgressBill && !Toolbox.Contains(billtypeid, new[] {	
																							OpsBillType.InvisibleCredit, 
																							OpsBillType.ProgressBillingOld, 
																							OpsBillType.VisibleCredit, 
																							OpsBillType.ProgressBilling
																							}))
					{
					billtypeid = OpsBillType.ProgressBilling;
					}
				else if (!WorkOrderHeader.IsRebill && !WorkOrderHeader.IsCredit && !Toolbox.Contains(billtypeid, new[] {		OpsBillType.ProgressBillingOld, 
																														OpsBillType.QuotedPrice, 
																														OpsBillType.ProgressBilling }))
					{
					billtypeid = prev_values.billtypeid == OpsBillType.ProgressBilling 
									? OpsBillType.ProgressBilling 
									: OpsBillType.QuotedPrice;
					}
				else if ((WorkOrderHeader.IsRebill || WorkOrderHeader.IsCredit) && !Toolbox.Contains(billtypeid, new[] {		OpsBillType.InvisibleCredit, 
																														OpsBillType.ProgressBillingOld, 
																														OpsBillType.VisibleCredit, 
																														OpsBillType.ProgressBilling }))
					{
					billtypeid =	billtypeid == OpsBillType.ProgressBilling || 
									billtypeid == OpsBillType.QuotedPrice ? 
										billtypeid : 
										prev_values.billtypeid;
					}
				}
			sell = sell == 0 
					? WorkOrderHeader.use_fixed_material_markup
						? WorkOrderHeader.fixed_material_markup*cost
						: shared.GetSellPrice(cost, 0, true, qty_committed, business_unit_id) 
							: sell;
			date_required_check(id);

			var changes = new wo_detail_changes(prev_values, is_updating, ref my_comm, by_module, user, is_manual)
				{
				is_receiving_from = is_receiving_from,
				is_transferring_from = is_transferring_from,
				is_transferring_to = is_transferring_to,
				receiving_from_bvpo = receiving_from_bvpo,
				transferring_from_bvwo = transferring_from_bvwo,
				transferring_to_bvwo = transferring_to_bvwo,
				committing_from_location = committing_from_location,
				uncommitting_to_location = uncommitting_to_location
				};

			changes.add("@added_by", prev_values.added_by, added_by);
			changes.add("@added_by_module", prev_values.added_by_module, by_module);
			changes.add("@billtypeid", prev_values.billtypeid, billtypeid);

			changes.add("@bvwo", prev_values.bvwo, bvwo);
			changes.add("@code", prev_values.code, code);
			changes.add("@business_unit_id", prev_values.business_unit_id, business_unit_id);
			changes.add("@consignment_id", prev_values.consignment_id, consignment_id);
			changes.add("@date_added", prev_values.date_added, date_added);
			changes.add("@date_required", prev_values.date_required, date_required);
			changes.add("@description", prev_values.description, description);
			changes.add("@discount", prev_values.discount, discount);
			changes.add("@id", prev_values.id, id);
			changes.add("@is_active", prev_values.is_active, is_active);
			changes.add("@issues", prev_values.issues, issues);
			changes.add("@location_id", prev_values.location_id, location_id);
			changes.add("@master_id", prev_values.master_id, master_id);
			changes.add("@memberid", prev_values.memberid, memberid);
			changes.add("@notes", prev_values.notes, notes);
			changes.add("@origin", prev_values.origin, origin);
			changes.add("@paytypeid", prev_values.paytypeid, paytypeid);
			changes.add("@price_cost", prev_values.cost, cost);
			changes.add("@price_sell", prev_values.sell, sell);
			changes.add("@price_unit", prev_values.unit, unit);
			changes.add("@qty_committed", prev_values.qty_committed, qty_committed);
            changes.add("@qty_invoiced", prev_values.qty_invoiced, qty_invoiced);
            changes.add("@qty_ordered", prev_values.qty_ordered, qty_ordered);
			changes.add("@rec_no", prev_values.rec_no, rec_no);
			changes.add("@track_part", prev_values.track_part, track_part);
			changes.add("@type", prev_values.type, type);
			changes.add("@woprog_id", prev_values.woprog_id, woprog_id);
            changes.add("@seg1_id", prev_values.seg1_id, seg1_id);
            changes.add("@asset_id", prev_values.asset_id, asset_id);
            changes.add("@asset_unit", prev_values.asset_unit, asset_unit);

            changes.add("@activity_code", prev_values.ActivityCode, ActivityCode);
            changes.add("@cost_element", prev_values.CostElement, CostElement);
            changes.add("@client_wo", prev_values.ClientWO, ClientWO);
			changes.add("@client_po", prev_values.ClientPO, ClientPO);
            changes.add("@child_woprog_id", prev_values.child_woprog_id, child_woprog_id);

            changes.wodc.woprog_id = woprog_id;
			id = changes.process(conn, transaction);

			var conditionSellChangeNotExclude	= Math.Abs(prev_values.sell - sell) > 0.01 && is_manual
													&& billtypeid != OpsBillType.DoNotInclude 
													&& prev_values.billtypeid != OpsBillType.DoNotInclude;
			var conditionMovingToExclude		= prev_values.billtypeid != OpsBillType.DoNotInclude && billtypeid == OpsBillType.DoNotInclude;
			var conditionMovingOutOfExclude		= prev_values.billtypeid == OpsBillType.DoNotInclude && billtypeid != OpsBillType.DoNotInclude;

			if((type == OpsWOLineType.Labor || type == OpsWOLineType.Mileage) && 
				WorkOrderHeader.IsChild &&
				!WorkOrderHeader.IsQuoted && 
				WorkOrderHeader.IsIntercompany && 
				(conditionSellChangeNotExclude || conditionMovingToExclude ||conditionMovingOutOfExclude)
				)
					{ 
					AdjustParentLaborCost(user, is_manual, WorkOrderHeader, conn, transaction);
					}
				}
			}
		private class wo_detail_changes
		{
			private bool is_updating = false;
			private bool is_manual = false;

			public bool is_transferring_from { get; set; }
			public bool is_receiving_from { get; set; }
			public bool is_transferring_to { get; set; }
			public string receiving_from_bvpo { get; set; }
			public string transferring_from_bvwo { get; set; }
			public string transferring_to_bvwo { get; set; }
			public string committing_from_location { get; set; }
			public string uncommitting_to_location { get; set; }


			private MySqlCommand comm = new MySqlCommand();
			private NeMember user = new NeMember();
			private string by_module = "";
			public NeWODetailCurrent wodc = new NeWODetailCurrent();

			public wo_detail_changes() { }
			public wo_detail_changes(NeWODetailCurrent _wodc, bool _is_updating, ref MySqlCommand _comm, string _by_module, NeMember _user, bool _is_manual)
			{
				is_updating = _is_updating;
				comm = _comm;
				user = _user;
				by_module = _by_module;
				wodc = _wodc;
				is_manual = _is_manual;
			}
			private List<string> shown_changes = new List<string>();
			private List<string> hidden_changes = new List<string>();
			public void add(string parameter_name, object old_value, object new_value)
			{
				if (is_updating || Toolbox.Contains(parameter_name, new string[] { "@qty_committed", "@qty_ordered" }))
				{
					double old_qty, new_qty, true_qty = 0;
					int int_old_val, int_new_val = 0;
					var str_old_val = old_value == null ? "" : old_value.ToString().Trim();
					var str_new_val = new_value == null ? "" : new_value.ToString().Trim();
					int.TryParse(str_old_val, out int_old_val);
					int.TryParse(str_new_val, out int_new_val);
					var is_different = str_old_val != str_new_val; // This is set because of quantity changes, they never equal the same... though the majority of the values do change.
					switch (parameter_name)
					{
						// These shouldn't need anything special to check them... just add them to the collection.
						case "@added_by":
						case "@id":
						case "@bvwo":
						case "@code":
						case "@business_unit_id":
						case "@date_added":
						case "@issues":
						case "@memberid":
						case "@paytypeid":
						case "@price_unit":
						case "@type":
						case "@woprog_id":
                        case "@seg1_id":
						case "@activity_code":
						case "@cost_element":
						case "@client_wo":
						case "@client_po":
						case "@child_woprog_id":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							break;
						case "@added_by_module":
							if (is_updating)
							{
								comm.Parameters.AddWithValue(parameter_name, old_value);
							}
							break;
						// This isn't really used in NESI, but is still used while communicating with BV, this honestly should be phased out.. doesn't require tracking.
						case "@qty_invoiced":
							old_qty = Convert.ToDouble(old_value);
							new_qty = Convert.ToDouble(new_value);
							true_qty = old_qty + new_qty;
							// Don't need to record the change in history as this should always match the committed quantity.
							comm.Parameters.AddWithValue(parameter_name, true_qty);
							break;
						// Not tracked actively, but still want to record changes
						case "@consignment_id":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (int_old_val != int_new_val)
							{
								hidden_changes.Add($"Set the consignment id to {new_value}");
							}
							break;
						case "@discount":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (int_old_val != int_new_val)
							{
								hidden_changes.Add($"Changed the discount percentage from {old_value} to {new_value}");
							}
							break;
						case "@is_active":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (int_old_val != int_new_val)
							{
								hidden_changes.Add($"Changed the active flag from {old_value} to {new_value}");
							}
							break;
						case "@location_id":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (int_old_val != int_new_val)
							{
								hidden_changes.Add($"Changed the location id from {old_value} to {new_value}");
							}
							break;
						case "@master_id":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (int_old_val != int_new_val)
							{
								hidden_changes.Add($"Changed the part number from {old_value} to {new_value}");
							}
							break;
						case "@notes":
							var old_note = str_old_val;
							var new_note = str_new_val;
							new_value = wodc.notes != new_note && new_note != "" ? wodc.notes + "\n " + new_note : new_note;
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (old_note != new_note && new_note != "")
							{
								hidden_changes.Add($"Set the notes to {new_value}");
							}
							break;
						case "@origin":
							var old_origin = str_old_val;
							var new_origin = str_new_val;
							new_value = wodc.origin != new_origin && new_origin != "" ? wodc.origin + " " + new_origin : new_origin;
							if (new_origin == "")
							{
								new_origin = old_origin;
							}
							comm.Parameters.AddWithValue(parameter_name, new_origin);
							if (old_origin != new_origin && new_origin != "")
							{
								hidden_changes.Add($"Changed the origin from {old_origin} to {new_origin}");
							}
							break;
						case "@rec_no":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (int_old_val != int_new_val)
							{
								hidden_changes.Add($"Changed the record number from {old_value} to {new_value}");
							}
							break;
						case "@track_part":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (int_old_val != int_new_val)
							{
								hidden_changes.Add($"Set the track part flag to {new_value}");
							}
							break;




						// Tracked actively
						case "@billtypeid":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (int_old_val != int_new_val)
							{
								var bt_old = old_value == null ? new billtype {name = "NULL"} : new billtype((int)old_value);
								var bt_new = old_value == null ? new billtype {name = "NULL"} : new billtype((int)new_value);
								shown_changes.Add($"Changed the billtype from {bt_old.name} to {bt_new.name}");
							}
							break;
						case "@date_required":
							comm.Parameters.AddWithValue(parameter_name, str_new_val == "" ? null : new_value);
							if (str_old_val != str_new_val)
							{
								shown_changes.Add($"Changed the required date to {new_value}");
							}
							break;
						case "@description":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							if (is_different)
							{
								shown_changes.Add($"Changed the description from {old_value} to {new_value}");
							}
							break;
						case "@price_cost":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							var old_cost = Math.Round(Convert.ToDouble(old_value), 4);
							var new_cost = Math.Round(Convert.ToDouble(new_value), 4);
							if (old_cost != new_cost && is_manual)
							{
								shown_changes.Add($"Changed the cost price from {old_cost:C2} to {new_cost:C2}");
							}
							break;
						case "@price_sell":
							comm.Parameters.AddWithValue(parameter_name, new_value);
							var old_sell = Math.Round(Convert.ToDouble(old_value), 4);
							var new_sell = Math.Round(Convert.ToDouble(new_value), 4);
							if (old_sell != new_sell && is_manual)
							{
								shown_changes.Add($"Manually changed the sell price from {old_sell:C2} to {new_sell:C2}");
							}
							break;
						// Don't need to check if the values changed for any of the quantities, it always will.
						case "@qty_ordered":
							old_qty = Convert.ToDouble(old_value);
							new_qty = Convert.ToDouble(new_value);
							true_qty = old_qty + new_qty;
							if (is_manual)
							{
								if (old_qty != 0 && new_qty != 0)
								{
									shown_changes.Add(new_qty > 0
										? $"Required another {Math.Abs(new_qty)}"
										: $"Removed {Math.Abs(new_qty)} from required");
								}
								else if (new_qty != 0)
								{
									shown_changes.Add($"Required {new_qty}");
								}
							}
							comm.Parameters.AddWithValue(parameter_name, true_qty);
							break;
						case "@qty_committed":
							old_qty = Convert.ToDouble(old_value);
							new_qty = Convert.ToDouble(new_value);
							true_qty = old_qty + new_qty;
							var abs_qty = Math.Abs(new_qty);
							if (old_qty != 0 && new_qty != 0)
							{
								if (is_transferring_to && new_qty < 0 && transferring_to_bvwo != "") // Transferring from WO to WO, and this is the originating WO
								{
									shown_changes.Add($"Transferred {abs_qty} to WO: {transferring_to_bvwo} from this WO");
								}
								else if (is_transferring_from && new_qty > 0 && transferring_from_bvwo != "") // Transferring from WO to WO, and this is the destination WO
								{
									shown_changes.Add($"Transferred {abs_qty} from WO: {transferring_from_bvwo} to this WO");
								}
								else if (is_receiving_from && new_qty > 0 && receiving_from_bvpo != "") // PO is committing to the WO
								{
									shown_changes.Add($"Received {abs_qty} from PO: {receiving_from_bvpo}");
								}
								else if (is_receiving_from && new_qty < 0 && receiving_from_bvpo != "") // PO is taking items out of the committed 
								{
									shown_changes.Add($"Unreceived {abs_qty} to PO: {receiving_from_bvpo}");
								}
								else if (new_qty > 0 && !string.IsNullOrEmpty(committing_from_location))
								{
									shown_changes.Add($"Committed another {abs_qty} from Location: {committing_from_location}");
								}
								else if (new_qty < 0 && !string.IsNullOrEmpty(uncommitting_to_location))
								{
									shown_changes.Add($"Uncommitted {abs_qty} to Location: {uncommitting_to_location}");
								}
								else // Handles all other stock committals/uncommitals
								{
									shown_changes.Add(new_qty > 0
										? $"Committed another {abs_qty}"
										: $"Uncommitted {abs_qty}");
								}
							}
							else if (new_qty != 0 && !string.IsNullOrEmpty(committing_from_location))
							{
								shown_changes.Add($"Committed {abs_qty} from Location: {committing_from_location}");
							}
							else if (new_qty != 0)
							{
								shown_changes.Add(new_qty > 0
									? $"Committed {Math.Abs(new_qty)}"
									: $"Uncommitted {Math.Abs(new_qty)}");
							}
							comm.Parameters.AddWithValue(parameter_name, true_qty);
							break;
					}
				}
				else
				{
					comm.Parameters.AddWithValue(parameter_name, new_value);
				}
			}
			public int process(MySqlConnection connectionForTransaction = null, MySqlTransaction transaction = null)
			{
				using(var conn = Toolbox.connect())
					{ 
				var returned = wodc.id;
				try
				{
					if (!is_updating)
					{
						// Track added / when / by whom & what module
						comm.ExecuteNonQuery();
						comm.CommandText = "SELECT LAST_INSERT_ID()";
						returned = Convert.ToInt32(comm.ExecuteScalar().ToString());
						wodc.id = returned;
						var shown = shown_changes.Count > 0 ? $"{user.FullName} - {string.Join(", ", shown_changes)}"
							: "";
						var hidden = hidden_changes.Count > 0 ? $"{user.FullName} - {string.Join(", ", hidden_changes)}"
							: "";
						if (shown != "")
						{
							Toolbox.doSQL_void(connectionForTransaction ?? conn, @"INSERT INTO woprogchanges_snapshot (member_id, for_tooltip, woprog_id, rec_no, dt, the_change, wo_detail_current_id) VALUES (@v0 , 1, @v1 , @v2 , NOW(), @v3 , @v4 )", 
								new object[] { user.id, wodc.woprog_id, wodc.rec_no, shown, wodc.id }, transaction);
						}
						if (hidden != "")
						{
							Toolbox.doSQL_void(connectionForTransaction ?? conn, @"INSERT INTO woprogchanges_snapshot (member_id, for_tooltip, woprog_id, rec_no, dt, the_change, wo_detail_current_id) VALUES (@v0 , 0, @v1 , @v2 , NOW(), @v3 , @v4 )", 
								new object[] { user.id, wodc.woprog_id, wodc.rec_no, hidden, wodc.id }, transaction);
						}
					}
					else
					{
						var shown = shown_changes.Count > 0 ? $"{user.FullName} - {string.Join(", ", shown_changes)}"
							: "";
						var hidden = hidden_changes.Count > 0 ? $"{user.FullName} - {string.Join(", ", hidden_changes)}"
							: "";
						if (shown != "")
						{
							Toolbox.doSQL_void(connectionForTransaction ?? conn, @"INSERT INTO woprogchanges_snapshot (member_id, for_tooltip, woprog_id, rec_no, dt, the_change, wo_detail_current_id) VALUES (@v0 , 1, @v1 , @v2 , NOW(), @v3 , @v4 )",
								new object[] { user.id, wodc.woprog_id, wodc.rec_no, shown, wodc.id }, transaction);
						}
						if (hidden != "")
						{
							Toolbox.doSQL_void(connectionForTransaction ?? conn, @"INSERT INTO woprogchanges_snapshot (member_id, for_tooltip, woprog_id, rec_no, dt, the_change, wo_detail_current_id) VALUES (@v0 , 0, @v1 , @v2 , NOW(), @v3 , @v4 )",
								new object[] { user.id, wodc.woprog_id, wodc.rec_no, hidden, wodc.id }, transaction);
						}
						comm.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					Toolbox.do_errorLog_errorStack(ex);

					if(transaction != null)
                     {
						transaction.Rollback();
                     }

					throw;
				}
				return returned;
					}
			}
		}
		public DataTable GetTimeSheetPartLine(object mid, object ptid, object woid, object _master_id)
		{
			return Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_current WHERE memberid = @v0  AND paytypeid = @v1  AND wo_detail_current_woprog_id = @v2  AND wo_detail_current_master_id = @v3  LIMIT 1", new object[] { mid, ptid, woid, _master_id });
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_wo_id"></param>
		/// <returns></returns>
		/// <remarks>Used in 1 place other than here</remarks>
		public static Toolbox.boolstr can_move_to_current(int _wo_id)
		{
			var this_boolstr = new Toolbox.boolstr();
			var history_count = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_history WHERE wo_detail_history_woprog_id =@v0 ", new object[] { _wo_id });
			var current_count = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id =@v0 ", new object[] { _wo_id });
			if (current_count > 0)
			{
				this_boolstr.success = false;
				this_boolstr.message = "Lines exist already for this work order in the current table";
			}
			else if (history_count == 0)
			{
				this_boolstr.success = false;
				this_boolstr.message = "No lines exist that can be moved.";
			}
			else
			{
				this_boolstr.success = true;
				this_boolstr.message = "Lines can be moved";
			}
			return this_boolstr;
		}
		/// <summary>
		/// For checking the wo_detail_current_date_required field, and if it's blank supply a default date of two weeks from today
		/// </summary>
		/// <param name="_id"></param>
		/// <remarks>Used in 1 places other than here</remarks>
		private void date_required_check(object _id, MySqlConnection connection = null)
		{
			if (string.IsNullOrEmpty(date_required))
			{
				// get the current date required
				if (_id != null)
					if (connection == null)
						date_required = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(wo_detail_current_date_required), '') FROM wo_detail_current  WHERE wo_detail_current_id =@v0 limit 1 ", new object[] { _id });
					else
						date_required = Toolbox.doSQL_string(connection, @"SELECT IFNULL(MAX(wo_detail_current_date_required), '') FROM wo_detail_current  WHERE wo_detail_current_id =@v0 limit 1 ", new object[] { _id });
				else
					date_required = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="_wo_id"></param>
		/// <remarks>Used in 2 places other than here</remarks>
		public static void move_to_current(int _wo_id)
		{
            try
            {
                Toolbox.doSQL_void("CALL WO_DETAIL_MOVE_LINES(@v0,@v1)", new object[] { _wo_id, "current" });
            }
            catch (Exception ee)
            {
                throw new Exception("Work order lines could not be moved - Reason given: " + ee);
            }
		}
		public static void restore_from_log(int _line_id)
		{
			using (var conn = Toolbox.connect())
			{
				var c = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM log.wo_detail WHERE id = @v0  AND event = 'DELETE'", new object[] { _line_id });
				if (c != 1)
				{
					return;
				}
				var dr = Toolbox.doSQL_dt(conn, @"SELECT * FROM log.wo_detail WHERE id = @v0  AND event = 'DELETE'", new object[] { _line_id }).Rows[0];
				var wodc = new NeWODetailCurrent
				{
					added_by = Convert.ToInt32(dr["added_by"]),
					added_by_module = dr["added_by_module"].ToString(),
					billtypeid = Convert.ToInt32(dr["billtypeid"]),
					//blocks_schedule = Convert.ToBoolean(dr["blocks_schedule"]),
					bvwo = Convert.ToInt32(dr["bvwo"]),
					code = dr["code"].ToString(),
					consignment_id = Convert.ToInt32(dr["consignment_id"]),
					cost = Convert.ToDouble(dr["price_cost"]),
					//currency_id = Convert.ToInt32(dr["currency_id"]),
					date_added = dr["date_added"].ToString(),
					date_modified = dr["date_modified"].ToString(),
					date_required = dr["date_required"].ToString(),
					description = dr["description"].ToString(),
					discount = Convert.ToDouble(dr["discount"]),
					//is_active = Convert.ToBoolean(dr["is_active"]),
					woprog_id = Convert.ToInt32(dr["woprog_id"]),
					unit = Convert.ToDouble(dr["price_unit"]),
					type = dr["type"].ToString(),
					track_part = Convert.ToInt32(dr["track_part"]),
					sell = Convert.ToDouble(dr["price_sell"]),
					rec_no = Convert.ToInt32(dr["rec_no"]),
					qty_ordered = Convert.ToDouble(dr["qty_ordered"]),
					qty_committed = Convert.ToDouble(dr["qty_committed"]),
					qty_invoiced = Convert.ToDouble(dr["qty_invoiced"]),
					paytypeid = Convert.ToInt32(dr["paytypeid"]),
					master_id = Convert.ToInt32(dr["master_id"]),
					memberid = Convert.ToInt32(dr["memberid"]),
					issues = dr["issues"].ToString(),
					origin = dr["origin"].ToString(),
					notes = dr["notes"].ToString(),
					location_id = Convert.ToInt32(dr["location_id"])
				};
				wodc.save(new NeMember(wodc.added_by), dr["added_by_module"].ToString(), false);
			}
		}
		public static void reorder_lines(int wo_id, MySqlConnection connection = null, MySqlTransaction transaction = null)
		{
			var _tools = new Toolbox();
			if (HttpContext.Current.Application["wos_processing"] == null)
			{
				HttpContext.Current.Application["wos_processing"] = new List<int>();
			}
			var wos_processing = (List<int>)HttpContext.Current.Application["wos_processing"];
			if (!wos_processing.Contains(wo_id))
			{
				try
				{
					wos_processing.Add(wo_id);
					var dt_detail = Toolbox.doSQL_dt(@"SELECT * FROM (SELECT wo_detail_current_id id, wo_detail_current_master_Id master_id, wo_detail_current_rec_no rec_no, wo_detail_current_type TYPE,'' ACTION, 0 processed, 
0 to_rec_n FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'Q' 
ORDER BY wo_detail_current_date_added, wo_detail_current_master_id) asdf UNION
 SELECT * FROM (SELECT wo_detail_current_id id, wo_detail_current_master_Id master_id, wo_detail_current_rec_no rec_no, wo_detail_current_type TYPE,'' ACTION, 0 processed,
  0 to_rec_n FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'L'
   ORDER BY wo_detail_current_date_added, wo_detail_current_master_id) asdf  UNION
 SELECT * FROM (SELECT wo_detail_current_id id, wo_detail_current_master_Id master_id, wo_detail_current_rec_no rec_no, wo_detail_current_type TYPE,'' ACTION, 0 processed,
  0 to_rec_n FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'K'
   ORDER BY wo_detail_current_date_added, wo_detail_current_master_id) asdf  UNION
 SELECT * FROM (SELECT wo_detail_current_id id, wo_detail_current_master_Id master_id, wo_detail_current_rec_no rec_no, wo_detail_current_type TYPE,'' ACTION, 0 processed,
  0 to_rec_n FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'A'
   ORDER BY wo_detail_current_date_added, wo_detail_current_master_id) asdf  UNION 
   SELECT * FROM (SELECT wo_detail_current_id id, wo_detail_current_master_Id master_id, wo_detail_current_rec_no rec_no, wo_detail_current_type TYPE,'' ACTION, 0 processed,
    0 to_rec_n FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = 'M' 
    ORDER BY wo_detail_current_date_added, wo_detail_current_master_id) asdf", new object[] { wo_id });
					var dt_woprogchanges = Toolbox.doSQL_dt(@"SELECT *, '' action, 0 to_rec_n FROM woprogchanges WHERE woprogchanges_woprog_id = @v0 ", new object[] { wo_id });
					var dt_woprogchanges_ss = Toolbox.doSQL_dt(@"SELECT *, '' action, 0 to_rec_n FROM woprogchanges_snapshot WHERE woprog_id = @v0 ", new object[] { wo_id });
					var start_d = DateTime.Now;
					var i = 2;
					var temp_row_i = 1;
					foreach (DataRow dr in dt_detail.Rows)
					{
						var id = Convert.ToInt32(dr["id"]);
						var type = dr["type"].ToString();
						var real_rec_no = Convert.ToInt32(dr["rec_no"]);
						if (real_rec_no != i)
						{
							dr["to_rec_n"] = i;
							dr["action"] = "U";
							var temp_rec_no = real_rec_no * -1; // Create the negative value of the rec_no
																// Check dt if any other rows have the same rec_no
							var c = dt_detail.Select($"rec_no = {real_rec_no} AND id <> {id} AND processed = 0").Count();
							// Are there any records that have the NEW rec_no?
							var item_in_target_rec_no = dt_detail.Select(
															$"rec_no = {i} AND id <> {id} AND processed = 0").Count() > 0;
							// Duplicates exist for this rec #... multiple rec #0, or rec #10, etc.
							if (c > 0)
							{
								// Need to duplicate history lines for this rec
								foreach (var sub_changes_dr in dt_woprogchanges.Select(
									$@"woprogchanges_bvworec = '{real_rec_no}' AND woprogchanges_id > 50000")) // 50000 is the cap for duplicate history fields added
								{
									if (Convert.ToInt32(sub_changes_dr["woprogchanges_id"]) > 50000) // Need to recheck because we are inserting rows in this loop
									{
										var temp_dr = dt_woprogchanges.NewRow();
										temp_dr["woprogchanges_id"] = temp_row_i;
										temp_dr["woprogchanges_woprog_id"] = sub_changes_dr["woprogchanges_woprog_id"];
										temp_dr["woprogchanges_bvwo"] = sub_changes_dr["woprogchanges_bvwo"];
										temp_dr["woprogchanges_bvworec"] = i.ToString();
										temp_dr["woprogchanges_datetime"] = sub_changes_dr["woprogchanges_datetime"];
										temp_dr["woprogchanges_modifiedmemberid"] = sub_changes_dr["woprogchanges_modifiedmemberid"];
										temp_dr["woprogchanges_woprogcomment_id"] = sub_changes_dr["woprogchanges_woprogcomment_id"];
										temp_dr["woprogchanges_waspartno"] = sub_changes_dr["woprogchanges_waspartno"];
										temp_dr["woprogchanges_wasprice"] = sub_changes_dr["woprogchanges_wasprice"];
										temp_dr["woprogchanges_wasqty"] = sub_changes_dr["woprogchanges_wasqty"];
										temp_dr["woprogchanges_ispartno"] = sub_changes_dr["woprogchanges_ispartno"];
										temp_dr["woprogchanges_isprice"] = sub_changes_dr["woprogchanges_isprice"];
										temp_dr["woprogchanges_isqty"] = sub_changes_dr["woprogchanges_isqty"];
										temp_dr["woprogchanges_deleteflag"] = sub_changes_dr["woprogchanges_deleteflag"];
										temp_dr["woprogchanges_isdescription"] = sub_changes_dr["woprogchanges_isdescription"];
										temp_dr["woprogchanges_wasdescription"] = sub_changes_dr["woprogchanges_wasdescription"];
										temp_dr["woprogchanges_comments"] = sub_changes_dr["woprogchanges_comments"];
										temp_dr["business_unit_id"] = sub_changes_dr["business_unit_id"];
										temp_dr["woprogchanges_wasbillingtype"] = sub_changes_dr["woprogchanges_wasbillingtype"];
										temp_dr["woprogchanges_isbillingtype"] = sub_changes_dr["woprogchanges_isbillingtype"];
										temp_dr["woprogchanges_orderedqty"] = sub_changes_dr["woprogchanges_orderedqty"];
										temp_dr["woprogchanges_manualpricechange"] = sub_changes_dr["woprogchanges_manualpricechange"];
										temp_dr["was_req_qty"] = sub_changes_dr["was_req_qty"];
										temp_dr["action"] = "I";
										dt_woprogchanges.Rows.Add(temp_dr);
										temp_row_i++;
									}
								}

								foreach (var sub_changes_ss_dr in dt_woprogchanges_ss.Select(
									$@"rec_no = '{real_rec_no}' AND id > 50000")) // 50000 is the cap for duplicate history fields added
								{
									if (Convert.ToInt32(sub_changes_ss_dr["id"]) > 50000) // Need to recheck because we are inserting rows in this loop
									{
										var temp_dr = dt_woprogchanges_ss.NewRow();
										temp_dr["id"] = temp_row_i;
										temp_dr["rec_no"] = i.ToString();
										temp_dr["member_id"] = sub_changes_ss_dr["member_id"];
										temp_dr["for_tooltip"] = sub_changes_ss_dr["for_tooltip"];
										temp_dr["woprog_id"] = sub_changes_ss_dr["woprog_id"];
										temp_dr["dt"] = sub_changes_ss_dr["dt"];
										temp_dr["the_change"] = sub_changes_ss_dr["the_change"];
										temp_dr["action"] = "I";
										dt_woprogchanges_ss.Rows.Add(temp_dr);
										temp_row_i++;
									}
								}
							}
							else
							{
								// Simple move, set the to_rec_n to be the new rec_n, and the action to "U" for all entries with an id > 50000
								foreach (var sub_changes_dr in dt_woprogchanges.Select(
									$@"woprogchanges_bvworec = '{real_rec_no}' AND woprogchanges_id > 50000"))
								{
									if (Convert.ToInt32(sub_changes_dr["woprogchanges_id"]) > 50000) // Need to recheck because we are inserting rows in this loop
									{
										sub_changes_dr["action"] = "U";
										sub_changes_dr["to_rec_n"] = i;
									}
								}
								foreach (var sub_changes_ss_dr in dt_woprogchanges_ss.Select(
									$@"rec_no = '{real_rec_no}' AND id > 50000")) // 50000 is the cap for duplicate history fields added
								{
									if (Convert.ToInt32(sub_changes_ss_dr["id"]) > 50000) // Need to recheck because we are inserting rows in this loop
									{
										sub_changes_ss_dr["action"] = "U";
										sub_changes_ss_dr["to_rec_n"] = i;
									}
								}
							}
						}
						dr["processed"] = 1;
						i++;
					}

					// Process wo_detail_current -- Update
					foreach (var dr in dt_detail.Select("action = 'U'"))
					{
						if (connection == null || transaction == null)
							Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_date_modified = wo_detail_current_date_modified, wo_detail_current_rec_no = @v0  WHERE wo_detail_current_id = @v1  LIMIT 1", new object[] { dr["to_rec_n"], dr["id"] });
						else
							Toolbox.doSQL_void(connection, @"UPDATE wo_detail_current SET wo_detail_current_date_modified = wo_detail_current_date_modified, wo_detail_current_rec_no = @v0  WHERE wo_detail_current_id = @v1  LIMIT 1", new object[] { dr["to_rec_n"], dr["id"] }, transaction);
					}
					// Process woprogchanges -- Update
					foreach (var dr in dt_woprogchanges.Select("action = 'U'"))
					{
						if (connection == null || transaction == null)
							Toolbox.doSQL_void(@"UPDATE woprogchanges SET woprogchanges_bvworec = @v0  WHERE woprogchanges_id = @v1  LIMIT 1", new object[] { dr["to_rec_n"], dr["woprogchanges_id"] });
						else
							Toolbox.doSQL_void(connection, @"UPDATE woprogchanges SET woprogchanges_bvworec = @v0  WHERE woprogchanges_id = @v1  LIMIT 1", new object[] { dr["to_rec_n"], dr["woprogchanges_id"] }, transaction);
					}
					// Process woprogchanges -- Insert
					var i_insert_woprogchanges = 0;
					var sb_woprogchanges = new StringBuilder(@"
	INSERT INTO woprogchanges
	(
	woprogchanges_woprog_id,
	woprogchanges_bvwo,
	woprogchanges_bvworec,
	woprogchanges_datetime,
	woprogchanges_modifiedmemberid,
	woprogchanges_woprogcomment_id,
	woprogchanges_waspartno,
	woprogchanges_wasprice,
	woprogchanges_wasqty,
	woprogchanges_ispartno,
	woprogchanges_isprice,
	woprogchanges_isqty,
	woprogchanges_deleteflag,
	woprogchanges_isdescription,
	woprogchanges_wasdescription,
	woprogchanges_comments,
	business_unit_id,
	woprogchanges_wasbillingtype,
	woprogchanges_isbillingtype,
	woprogchanges_orderedqty,
	woprogchanges_manualpricechange,
	was_req_qty
	)
VALUES");
					List<object> paramObjects = new List<object>();
					foreach (var dr in dt_woprogchanges.Select("action = 'I'"))
					{
						sb_woprogchanges.AppendFormat($@"
	(
	@v{(i_insert_woprogchanges * 21 + 0).ToString()},
	@v{(i_insert_woprogchanges * 21 + 1).ToString()},
	@v{(i_insert_woprogchanges * 21 + 2).ToString()},
	NOW(),
	@v{(i_insert_woprogchanges * 21 + 3).ToString()},
	@v{(i_insert_woprogchanges * 21 + 4).ToString()},
	@v{(i_insert_woprogchanges * 21 + 5).ToString()},
	@v{(i_insert_woprogchanges * 21 + 6).ToString()},
	@v{(i_insert_woprogchanges * 21 + 7).ToString()},
	@v{(i_insert_woprogchanges * 21 + 8).ToString()},
	@v{(i_insert_woprogchanges * 21 + 9).ToString()},
	@v{(i_insert_woprogchanges * 21 + 10).ToString()},
	@v{(i_insert_woprogchanges * 21 + 11).ToString()},
	@v{(i_insert_woprogchanges * 21 + 12).ToString()},
	@v{(i_insert_woprogchanges * 21 + 13).ToString()},
	@v{(i_insert_woprogchanges * 21 + 14).ToString()},
	@v{(i_insert_woprogchanges * 21 + 15).ToString()},
	@v{(i_insert_woprogchanges * 21 + 16).ToString()},
	@v{(i_insert_woprogchanges * 21 + 17).ToString()},
	@v{(i_insert_woprogchanges * 21 + 18).ToString()},
	@v{(i_insert_woprogchanges * 21 + 19).ToString()},
	@v{(i_insert_woprogchanges * 21 + 20).ToString()}
	),");

						paramObjects.Add(dr["woprogchanges_woprog_id"] == DBNull.Value
							? "NULL"
							: dr["woprogchanges_woprog_id"]);                  // {0}
						paramObjects.Add(dr["woprogchanges_bvwo"] == DBNull.Value
							? "NULL"
							: dr["woprogchanges_bvwo"]);                         // {1}
						paramObjects.Add(dr["woprogchanges_bvworec"] == DBNull.Value ? "NULL" : dr["woprogchanges_bvworec"]);                     // {2}
						paramObjects.Add(dr["woprogchanges_modifiedmemberid"] == DBNull.Value ? "NULL" : dr["woprogchanges_modifiedmemberid"]);// {4}
						paramObjects.Add(dr["woprogchanges_woprogcomment_id"] == DBNull.Value ? "NULL" : dr["woprogchanges_woprogcomment_id"]);// {5}
						paramObjects.Add(dr["woprogchanges_waspartno"] == DBNull.Value ? "NULL" : dr["woprogchanges_waspartno"]);                 // {6}
						paramObjects.Add(dr["woprogchanges_wasprice"] == DBNull.Value ? "NULL" : dr["woprogchanges_wasprice"]);               // {7}
						paramObjects.Add(dr["woprogchanges_wasqty"] == DBNull.Value ? "NULL" : dr["woprogchanges_wasqty"]);                   // {8}
						paramObjects.Add(dr["woprogchanges_ispartno"] == DBNull.Value ? "NULL" : dr["woprogchanges_ispartno"]);               // {9}
						paramObjects.Add(dr["woprogchanges_isprice"] == DBNull.Value ? "NULL" : dr["woprogchanges_isprice"]);                     // {10}
						paramObjects.Add(dr["woprogchanges_isqty"] == DBNull.Value ? "NULL" : dr["woprogchanges_isqty"]);                         // {11}
						paramObjects.Add(dr["woprogchanges_deleteflag"] == DBNull.Value ? "NULL" : dr["woprogchanges_deleteflag"]);           // {12}
						paramObjects.Add(dr["woprogchanges_isdescription"] == DBNull.Value ? "NULL" : dr["woprogchanges_isdescription"]);         // {13}
						paramObjects.Add(dr["woprogchanges_wasdescription"] == DBNull.Value ? "NULL" : dr["woprogchanges_wasdescription"]);   // {14}
						paramObjects.Add(dr["woprogchanges_comments"] == DBNull.Value ? "NULL" : dr["woprogchanges_comments"]);               // {15}
						paramObjects.Add(dr["business_unit_id"] == DBNull.Value ? "NULL" : dr["business_unit_id"]);               // {16}
						paramObjects.Add(dr["woprogchanges_wasbillingtype"] == DBNull.Value ? "NULL" : dr["woprogchanges_wasbillingtype"]);   // {17}
						paramObjects.Add(dr["woprogchanges_isbillingtype"] == DBNull.Value ? "NULL" : dr["woprogchanges_isbillingtype"]);         // {18}
						paramObjects.Add(dr["woprogchanges_orderedqty"] == DBNull.Value ? "NULL" : dr["woprogchanges_orderedqty"]);           // {19}
						paramObjects.Add(dr["woprogchanges_manualpricechange"] == DBNull.Value ? "NULL" : dr["woprogchanges_manualpricechange"]); // {20}
						paramObjects.Add(dr["was_req_qty"] == DBNull.Value
							? "NULL"
							: dr["was_req_qty"]);                                   // {21}


						i_insert_woprogchanges++;
					}
					if (i_insert_woprogchanges > 0)
					{
						Toolbox.doSQL_void(sb_woprogchanges.ToString().TrimEnd(','), paramObjects.ToArray());
					}
					// Process woprogchanges_snapshot -- Update
					foreach (var dr in dt_woprogchanges_ss.Select("action = 'U'"))
					{
						Toolbox.doSQL_void(@"UPDATE woprogchanges_snapshot SET rec_no = @v0  WHERE id = @v1  LIMIT 1", new object[] { dr["to_rec_n"], dr["id"] });
					}
					// Process woprogchanges_snapshot -- Insert
					var sb_ss = new StringBuilder();
					sb_ss.Append(@"
INSERT INTO woprogchanges_snapshot
	(
	member_id,
	for_tooltip,
	woprog_id,
	rec_no,
	dt,
	the_change
	)
VALUES");
					var i_insert_ss = 0;
					List<object> paramObjects2 = new List<object>();
					foreach (var dr in dt_woprogchanges_ss.Select("action = 'I'"))
					{
						sb_ss.AppendFormat($@"(
	@v{(i_insert_ss * 6 + 0).ToString()},
	@v{(i_insert_ss * 6 + 1).ToString()},
	@v{(i_insert_ss * 6 + 2).ToString()},
	@v{(i_insert_ss * 6 + 3).ToString()},
	@v{(i_insert_ss * 6 + 4).ToString()},
	@v{(i_insert_ss * 6 + 5).ToString()}
	),"
							);

						paramObjects2.Add(dr["member_id"] == DBNull.Value ? "NULL" : dr["member_id"]);
						paramObjects2.Add(dr["for_tooltip"] == DBNull.Value ? "NULL" : dr["for_tooltip"]);
						paramObjects2.Add(dr["woprog_id"] == DBNull.Value ? "NULL" : dr["woprog_id"]);
						paramObjects2.Add(dr["rec_no"] == DBNull.Value ? "NULL" : dr["rec_no"]);
						paramObjects2.Add(dr["dt"] == DBNull.Value ? "NULL" : Toolbox.MySQL_longdt((DateTime)dr["dt"]));
						paramObjects2.Add(dr["the_change"] == DBNull.Value ? "NULL" : dr["the_change"]);

						i_insert_ss++;
					}
					if (i_insert_ss > 0)
					{
						Toolbox.doSQL_void(sb_ss.ToString().TrimEnd(','),paramObjects2.ToArray());
					}
				}
				catch (Exception ee)
				{
					_tools.catch_error(ee);
				}
				wos_processing.Remove(wo_id);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lineid"></param>
		/// <remarks>Used in 23 places other than here</remarks>
		public void GetLineDetails(string lineid)
		{
			load(Convert.ToInt32(lineid));
		}
		public void load(int _id, MySqlConnection connection = null)
		{
			id = _id;
			DataTable lineitem;
			if (connection == null)
				lineitem = Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_current  WHERE wo_detail_current_id =@v0", new object[] { _id });
			else
				lineitem = Toolbox.doSQL_dt(connection, @"SELECT * FROM wo_detail_current  WHERE wo_detail_current_id =@v0", new object[] { _id });
			foreach (DataRow row in lineitem.Rows)
			{
				woprog_id = Convert.ToInt32(row["wo_detail_current_woprog_id"]);
				rec_no = Convert.ToInt32(row["wo_detail_current_rec_no"]);
				type = row["wo_detail_current_type"].ToString();
				master_id = Convert.ToInt32(row["wo_detail_current_master_id"]);
				date_required = row["wo_detail_current_date_required"].ToString() != string.Empty ? Toolbox.MySQL_shortdt(Convert.ToDateTime(row["wo_detail_current_date_required"])) : "";
				date_added = Convert.ToDateTime(row["wo_detail_current_date_added"].ToString()).ToString("yyyy-MM-dd");
				date_modified = row["wo_detail_current_date_modified"].ToString();
				description = row["wo_detail_current_description"].ToString();
				qty_committed = Convert.ToDouble(row["wo_detail_current_qty_committed"]);
				qty_invoiced = Convert.ToDouble(row["wo_detail_current_qty_invoiced"]);
				cost = Convert.ToDouble(row["wo_detail_current_price_cost"]);
				sell = Convert.ToDouble(row["wo_detail_current_price_sell"]);
				unit = sell;
				added_by = Convert.ToInt32(row["wo_detail_current_added_by"]);
				bvwo = Convert.ToInt32(row["wo_detail_current_bvwo"]);
				code = row["wo_detail_current_code"].ToString();
				origin = row["wo_detail_current_origin"].ToString();
				billtypeid = Convert.ToInt32(row["wo_detail_current_billtypeid"]);
				issues = row["wo_detail_current_issues"].ToString();
				memberid = Convert.ToInt32(row["memberid"]);
				paytypeid = Convert.ToInt32(row["paytypeid"].ToString());
				qty_ordered = Convert.ToDouble(row["wo_detail_current_qty_ordered"]);
				discount = Convert.ToDouble(row["wo_detail_current_discount"]);
				consignment_id = Convert.ToInt32(row["wo_detail_current_consignment_id"]);
				track_part = Convert.ToInt32(row["wo_detail_current_track_part"]);
				notes = row["wo_detail_current_notes"].ToString();
				added_by_module = Toolbox.ReturnBlankIfNull_string(row["wo_detail_current_added_by_module"]);
				blocks_schedule = Toolbox.ReturnZeroIfNull_int(row["blocks_schedule"]) == 1;
                seg1_id = Toolbox.ReturnZeroIfNull_int(row["seg1_id"]);
				business_unit_id = Convert.ToInt32(row["business_unit_id"]);
				asset_id= Toolbox.ReturnZeroIfNull_int(row["asset_id"]);
				asset_unit = Toolbox.ReturnZeroIfNull_int(row["asset_unit"]);
				ActivityCode = Toolbox.ReturnBlankIfNull_string(row["activity_code"]);
				CostElement = Toolbox.ReturnBlankIfNull_string(row["cost_element"]);
				ClientPO = Toolbox.ReturnBlankIfNull_string(row["client_po"]);
				ClientWO = Toolbox.ReturnBlankIfNull_string(row["client_wo"]);
                PartiallyBilled = Toolbox.ReturnZeroIfNull_int(row["partially_billed"]) == 1;
				child_woprog_id = Convert.ToInt32(row["child_woprog_id"]);
			}
		}
        public void LoadByWoIdRecNo(int woprog_id, int rec_no)
        { 
            var lineitem = Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_rec_no = @v1", new object[] { woprog_id, rec_no });
            foreach (DataRow row in lineitem.Rows)
            {
                id = Convert.ToInt32(row["wo_detail_current_id"]);
                woprog_id = Convert.ToInt32(row["wo_detail_current_woprog_id"]);
                rec_no = Convert.ToInt32(row["wo_detail_current_rec_no"]);
                type = row["wo_detail_current_type"].ToString();
                master_id = Convert.ToInt32(row["wo_detail_current_master_id"]);
                date_required = row["wo_detail_current_date_required"].ToString() != string.Empty ? Toolbox.MySQL_shortdt(Convert.ToDateTime(row["wo_detail_current_date_required"])) : "";
                date_added = Convert.ToDateTime(row["wo_detail_current_date_added"].ToString()).ToString("yyyy-MM-dd");
                date_modified = row["wo_detail_current_date_modified"].ToString();
                description = row["wo_detail_current_description"].ToString();
                qty_committed = Convert.ToDouble(row["wo_detail_current_qty_committed"]);
                qty_invoiced = Convert.ToDouble(row["wo_detail_current_qty_invoiced"]);
                cost = Convert.ToDouble(row["wo_detail_current_price_cost"]);
                sell = Convert.ToDouble(row["wo_detail_current_price_sell"]);
                unit = sell;
                added_by = Convert.ToInt32(row["wo_detail_current_added_by"]);
                business_unit_id = (int)row["business_unit_id"];
                bvwo = Convert.ToInt32(row["wo_detail_current_bvwo"]);
                code = row["wo_detail_current_code"].ToString();
                origin = row["wo_detail_current_origin"].ToString();
                billtypeid = Convert.ToInt32(row["wo_detail_current_billtypeid"]);
                issues = row["wo_detail_current_issues"].ToString();
                memberid = Convert.ToInt32(row["memberid"]);
                paytypeid = Convert.ToInt32(row["paytypeid"].ToString());
                qty_ordered = Convert.ToDouble(row["wo_detail_current_qty_ordered"]);
                discount = Convert.ToDouble(row["wo_detail_current_discount"]);
                consignment_id = Convert.ToInt32(row["wo_detail_current_consignment_id"]);
                track_part = Convert.ToInt32(row["wo_detail_current_track_part"]);
                notes = row["wo_detail_current_notes"].ToString();
                added_by_module = Toolbox.ReturnBlankIfNull_string(row["wo_detail_current_added_by_module"]);
                blocks_schedule = Toolbox.ReturnZeroIfNull_int(row["blocks_schedule"]) == 1;
				//TODO: Load the newly added fields/columns if this method is to be used again
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line_id"></param>
        /// <param name="part_no"></param>
        /// <param name="myMember"></param>
        public static void delete_workorder_line(int line_id, object part_no, NeMember myMember)
		{
			var wodc = new NeWODetailCurrent(line_id);
			var wo_id = wodc.woprog_id;
			if (wodc.blocks_schedule)
			{
				throw new Exception(
					$"Cannot delete Part #{wodc.master_id}, Rec #{wodc.rec_no} while the blocks schedule flag is set.");
			}
			#region Check Quote Information
			//Check For Quote Information
			//How Many Job Cost for Quotes on This Work Order
			var JobstcostCount = 0;
			try
			{
				JobstcostCount = Toolbox.doSQL_int(@"SELECT COUNT(wo_detail_current_id) FROM wo_detail_current WHERE wo_detail_current_billtypeid = 1 AND wo_detail_current_woprog_id = @v0 ", new object[] { wo_id });
			}
			catch (Exception ee)
			{
				Toolbox.do_errorLog(ee, "Error finding jobcost count");
			}
			if (	part_no.ToString() == "2139" && 
					JobstcostCount != 0 && 
					wodc.billtypeid != OpsBillType.ProgressBillingOld && 
					wodc.billtypeid != OpsBillType.ProgressBilling && 
					wodc.qty_committed == 0 && 
					wodc.consignment_id == 0)
			{
				if (Toolbox.doSQL_int(@"SELECT COUNT(id) FROM wo_detail WHERE woprog_id = @v0 AND billtypeid IN (3,11)", new object[] { wo_id }) <= 1)
				{
					throw new Exception("You can not delete a quote line if there are job costs for quote bill types on the work order.");
				}
			}
			#endregion
			#region Make sure you can delete the line.... throw errors if not

			var wo = new NeWOProg(wo_id);
			if (wo.woprog_iscredit == 0 && wo.woprog_isrebill == 0)
			{
				if (wodc.master_id >= OpsSpecialPart.LaborThreshold &&
						wodc.origin != OpsWOLineOrigin.ManuallyAdded &&
						wodc.origin != OpsWOLineOrigin.ExpenseReimbursement &&
						wodc.origin != OpsWOLineOrigin.PerDiemExpense &&
						wodc.origin != OpsWOLineOrigin.CompanyCreditCardExpense &&
						!myMember.business_unit.is_backoffice)
				{
					throw new Exception("You are not allowed to delete labour that has been added via timesheets or been transferred, please use billtypes to modify");
				}

				if (wodc.origin != null && wodc.origin.Contains("PO"))
				{
					try
					{
						var inv = new inventory();
						var bu = new NeBusinessUnit(wodc.business_unit_id);
						inv.Load(wodc.master_id, bu.warehouse_bu_id);
						var _o = wodc.origin;
						_o = _o.Substring(_o.IndexOf("PO") + 3, 10);
						if (inv.is_exclude)
						{
							if (Toolbox.doSQL_int(@"Select poprog_status from poprog_header  where poprog_bvpo =@v0", new object[] { _o }) != 8)
							{
								throw new Exception("You Cannot Delete an Item Transferred From an OPEN PO.");
							}
						}
						else
						{
							if (wodc.qty_committed != 0)
							{
								throw new Exception("You Cannot Delete an Item Transferred From a PO when there is still a Committed qty here.");
							}
							if (Toolbox.doSQL_int(@"Select poprog_status from poprog_header  where poprog_bvpo =@v0", new object[] { _o }) != 8)
							{
								throw new Exception("You Cannot Delete an Item Transferred From an OPEN PO.");
							}
						}
					}
					catch (Exception ee)
					{
						if (ee == null || ee.Message == "")
						{
							throw new Exception("You Cannot Delete an Item Transferred From an OPEN PO.");
						}
						else
						{
							throw new Exception(ee.Message);
						}
					}

				}
			}
			#endregion Make sure you can delete the line.... throw errors if not

			reorder_lines(wo_id); // Do a quick reorder
			wodc = new NeWODetailCurrent(line_id); // Re-instantiate the line details

			// Delete old change history for this line... not needed any more
			Toolbox.doSQL_void(@"DELETE FROM woprogchanges WHERE woprogchanges_woprog_id = @v0  AND woprogchanges_bvworec = @v1 ", new object[] { wo_id, wodc.rec_no });
			Toolbox.doSQL_void(@"DELETE FROM woprogchanges_snapshot WHERE woprog_id = @v0  AND rec_no = @v1 ", new object[] { wo_id, wodc.rec_no });

			var rows_above_this = Toolbox.doSQL_dt(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_rec_no > @v1  ORDER BY wo_detail_current_rec_no", new object[] { wodc.woprog_id, wodc.rec_no });
			// The idea here is we're removed rec #3 out of the list of recs (2,3,4,5,6,7,8) 
			// We need to process each one of (4,5,6,7,8) individually.
			NeWODetailCurrent above_row;
			foreach (DataRow row in rows_above_this.Rows)
			{
				above_row = new NeWODetailCurrent(Convert.ToInt32(row["wo_detail_current_id"]));

				// Affect woprogchanges
				Toolbox.doSQL_void(@"UPDATE woprogchanges SET woprogchanges_bvworec = @v0  WHERE woprogchanges_woprog_id = @v1  AND woprogchanges_bvworec = @v2 ", new object[] { above_row.rec_no - 1, wo_id, above_row.rec_no });

				// Affect woprogchanges_snapshot
				Toolbox.doSQL_void(@"UPDATE woprogchanges_snapshot SET rec_no = @v0  WHERE woprog_id = @v1  AND rec_no = @v2 ", new object[] { above_row.rec_no - 1, wo_id, above_row.rec_no });

				// Affect row itself
				above_row.rec_no--;
				above_row.qty_committed = 0;
				above_row.qty_ordered = 0;
				above_row.qty_invoiced = 0;
				above_row.save(myMember, "delete_workorder_line - Reordering records", false);
			}

			if (wodc.consignment_id != 0 && wodc.origin != "Expense Reimbursement" && wodc.origin != "Per Diem Expense" && wodc.origin != "Company Credit Card Expense")
			{
				var consign = new consignment(wodc.consignment_id);
				consign.load();
				consign.status = consignment.StatusType.WaitingToBeQuoted;
				consign.save();
				Toolbox.doSQL_void(@"DELETE FROM quote_worksheet  where consignment_id != 0 AND consignment_id =@v0 limit 1 ", new object[] { wodc.consignment_id });
			}

			Toolbox.doSQL_void(@"DELETE FROM wo_detail_current  WHERE wo_detail_current_id =@v0 limit 1 ", new object[] { line_id });
		}
		#endregion
		/// <summary>
		/// Issue Checker 
		/// </summary>
		/// <param name="_detail"></param>
		/// <param name="_woprog"></param>
		/// <param name="_inventory"></param>
		/// <param name="_issueList"></param>
		public static void IssueChecker(NeWODetailCurrent _detail, NeWOProg _woprog, inventory _inventory, out List<string> _issueList)
			{
			_issueList = new List<string>();
			var currentCommitted = _detail.id == 0
										? _inventory.is_exclude
											? 0
											: CurrentCommittedQuantity(_detail.woprog_id, _detail.master_id)
										: CurrentCommittedQuantity(_detail.id);
			var combinedCommitted = _detail.qty_committed + currentCommitted;
			if (_detail.cost > _detail.sell)
				{
				if (!Toolbox.Contains(_detail.billtypeid, new [] {	OpsBillType.Comment, 
																	OpsBillType.VisibleNoCharge, 
																	OpsBillType.Blank
																	}))
					{
					_issueList.Add(OpsWOLineIssue.CostLessThanSell);
					}
				}
			if (_detail.master_id != 0 && combinedCommitted == 0)
				{
				if (combinedCommitted == 0)
					{
					if (!Toolbox.Contains(_detail.billtypeid, new [] {	OpsBillType.Comment, 
																		OpsBillType.VisibleNoCharge, 
																		OpsBillType.Blank, 
																		OpsBillType.DoNotInclude
																		}))
						{
						_issueList.Add(OpsWOLineIssue.ZeroCommitted);
						}
					}
				}
			if (_detail.master_id != 0 && combinedCommitted < 0)
				{
				if (!Toolbox.Contains(_detail.billtypeid, new [] {	OpsBillType.Comment,
																	OpsBillType.VisibleNoCharge,
																	OpsBillType.Blank,
																	OpsBillType.VisibleCredit,
																	OpsBillType.InvisibleCredit
																	}))
					{
					_issueList.Add(OpsWOLineIssue.NegativeCommitted);
					}
				}
			if (_detail.master_id == OpsSpecialPart.MakeThisPart)
				{
				_issueList.Add(OpsWOLineIssue.TemporaryPartUsed);
				}
			if (_detail.sell < 0)
				{
				if (!Toolbox.Contains(_detail.billtypeid, new [] {	OpsBillType.Comment,
																	OpsBillType.VisibleNoCharge,
																	OpsBillType.Blank
																	}))
					{
					_issueList.Add(OpsWOLineIssue.SellLessThanZero);
					}
				}
			var minSellPrice = _woprog.use_fixed_material_markup
											? _woprog.fixed_material_markup * _detail.cost
											: shared.GetSellPrice(_detail.cost, 0, true, combinedCommitted, _detail.business_unit_id);
			if (_detail.sell < minSellPrice)
				{
				if (!Toolbox.Contains(_detail.billtypeid, new [] {	OpsBillType.Comment,
																	OpsBillType.VisibleNoCharge,
																	OpsBillType.Blank
																	}))
					{
					_issueList.Add(string.Format(OpsWOLineIssue.SellLessThanMinSellPrice, minSellPrice.ToString("C2")));
					}
				}

			if (_detail.type == OpsWOLineType.Labor)
				{
				if(_detail.id <= 0) return;
				var totalHours = NeMemberTime.TotalPayTypeHours(_detail.woprog_id, _detail.memberid, _detail.paytypeid);
				if (totalHours != combinedCommitted)
					{
					_issueList.Add(string.Format(OpsWOLineIssue.LaborTimeSheetDiscrepancy, totalHours));
					}
				}
			}
		#region Double (2)
		/// <summary>
		///		Retrieve the cost for the selected BU id
		/// </summary>
		/// <param name="_memberID"></param>
		/// <param name="_paytype"></param>
		/// <param name="_business_unit_id"></param>
		/// <returns></returns>	
		public double FindTrueLabourCost(int _memberID, int _paytype, int _business_unit_id)
		{
			return Toolbox.doSQL_double(@"SELECT get_wage_cost(@v0,@v1,@v2)", new object[] { _memberID, _paytype, _business_unit_id });
		}
		#endregion
		#region String (2)	
		/// <summary>
		/// 
		/// </summary>
		/// <param name="woprogid"></param>
		/// <returns></returns>
		/// <remarks>Used in 4 places other than here</remarks>
		public int GetLineCount(object woprogid)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 ", new object[] { woprogid });
		}
		/// <summary>
		/// Returns a line count for a specific type of WO line.
		/// </summary>
		/// <param name="woprogid"></param>
		/// <returns></returns>
		public int GetTypedLineCount(object woprogid, string type)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_type = @v1 ", new object[] { woprogid, type });
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mid"></param>
		/// <param name="ptid"></param>
		/// <param name="woid"></param>
		/// <returns></returns>
		/// <remarks>Used in 4 places other than here</remarks>
		public string GetRecordNumber(string mid, string ptid, string woid)
		{
			var recno = "";
			var strSQL = string.Format("SELECT IFNULL(MAX(wo_detail_current_rec_no), '') FROM wo_detail_current WHERE memberid ='" + mid + "' AND paytypeid = '" + ptid + "' AND wo_detail_current_woprog_id = " + woid);
			try
			{
				recno = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(wo_detail_current_rec_no), '') FROM wo_detail_current  WHERE memberid =@v0 AND paytypeid =@v1  AND wo_detail_current_woprog_id =@v2 ", new object[] { mid,ptid,woid });
			}
			catch (Exception ee)
			{
				new Toolbox().catch_error(ee);
			}
			return recno;
		}
		#endregion
		#region DataTable (3)	
		/// <summary>
		/// Only retrieves the rec number... stupid, stupid method... breaks the committing of parts.
		/// </summary>
		/// <param name="part"></param>
		/// <param name="woid"></param>
		/// <returns></returns>
		/// <remarks>Used in 1 place other than here</remarks>
		public DataTable GetPartLine(string part, string woid)
		{
			var table = new DataTable();
			var strSQL = "SELECT * FROM wo_detail_current WHERE wo_detail_current_master_id ='" + part + "' AND wo_detail_current_woprog_id = " + woid;
			table = Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_current  WHERE wo_detail_current_master_id =@v0 AND wo_detail_current_woprog_id =@v1 ", new object[] { part,woid });
			if (table.Rows.Count == 1)
			{
				foreach (DataRow row in table.Rows)
				{
					rec_no = Convert.ToInt32(row["wo_detail_current_rec_no"]);
				}
			}
			return table;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="Desc"></param>
		/// <param name="woid"></param>
		/// <returns></returns>
		/// <remarks>Used in 3 places other than here</remarks>
		public DataTable GetPartLineDesc(string Desc, string woid)
		{
			var table = new DataTable();
			table = Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_current  WHERE wo_detail_current_description =@v0 OR wo_detail_current_description =@v1  AND wo_detail_current_woprog_id =@v2  limit 1 ", new object[] { Desc,
				Toolbox.do_value_to(Desc),woid });
			if (table.Rows.Count == 1)
			{
				foreach (DataRow row in table.Rows)
				{
					rec_no = Convert.ToInt32(row["wo_detail_current_rec_no"]);
				}
			}
			return table;
		}
		#endregion
		#region Int (12)	
		/// <summary>
		/// 
		/// </summary>
		/// <param name="woid"></param>
		/// <returns></returns>
		/// <remarks>Used in 4 places other than here</remarks>
		public int DeleteWODetailCurrentFromTimeSheetTemp(string woid)
		{
			return Toolbox.doSQL_affectedrows(@" UPDATE wo_detail_current SET wo_detail_current_date_modified = NOW(), wo_detail_current_qty_committed = wo_detail_current_qty_committed + @v0 , wo_detail_current_qty_invoiced = wo_detail_current_qty_invoiced + @v1 , wo_detail_current_qty_ordered = wo_detail_current_qty_ordered + @v0  WHERE wo_detail_current_woprog_id = @v2  AND wo_detail_current_origin = 'Entered From Timesheet' AND memberid = @v3  AND paytypeid = @v4  ", new object[] { qty_committed, qty_invoiced, woid, memberid, paytypeid });
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="woid"></param>
		/// <param name="masterid"></param>
		/// <returns></returns>
		/// <remarks>Used in 2 places other than here</remarks>
		public int UpdateWODetailCurrentSellPriceCascade(string woid, string masterid)
		{
			return Toolbox.doSQL_affectedrows(@" UPDATE wo_detail_current SET wo_detail_current_price_sell = @v0 , wo_detail_current_price_unit = @v0  WHERE wo_detail_current_woprog_id = @v1  AND wo_detail_current_master_id = @v2 ", new object[] { sell, woid, masterid });
		}
		#endregion
		public class billtype
		{
			int id { get; set; }
			public bool show_on_invoice { get; set; }
			public string name { get; set; }
			public string description { get; set; }
			public billtype() { }
			public billtype(int _id)
			{
				if (exists(_id))
				{
					load(_id);
				}
			}
			private void load(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.wo_detail_lineitem_billtype>(_id);
					id = _id;
					show_on_invoice = x.wo_lineitem_billtype_showoninvoice;
					name = x.wo_lineitem_billtype_name;
					description = x.wo_lineitem_billtype_description;
				}

			}
			public bool exists(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					return uow.GetObjectByKey<ne_xpo.cs.wo_detail_lineitem_billtype>(_id) != null;
				}

			}
			public void save()
			{
				using (var uow = new UnitOfWork())
				{
					var bt = id == 0
						? new ne_xpo.cs.wo_detail_lineitem_billtype(uow)
						: uow.GetObjectByKey<ne_xpo.cs.wo_detail_lineitem_billtype>(id);
					bt.wo_lineitem_billtype_showoninvoice = show_on_invoice;
					bt.wo_lineitem_billtype_description = description;
					bt.wo_lineitem_billtype_name = name;
					bt.Save();
					uow.CommitChanges();
					id = bt.wo_lineitem_billtypeid;
				}

			}
			public void delete()
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.wo_detail_lineitem_billtype>(id);
					x.Delete();
					uow.CommitChanges();
				}
			}
		}

		public static bool IsPartiallyBilled(int _masterId, int _woprogId)
			{
			return Toolbox.doSQL_int(@"SELECT IFNULL(MAX(partially_billed), 0) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v1 AND wo_detail_current_master_id = @v0", new object[]{_masterId, _woprogId}) == 1;
			}
		public static void TogglePartiallyBilled(int _lineId)
			{
			Toolbox.doSQL_void(@"
			SELECT 1 INTO @disable_triggers;
			UPDATE wo_detail_current SET wo_detail_current_date_modified = wo_detail_current_date_modified, partially_billed = 1 - partially_billed WHERE wo_detail_current_id = @v0 LIMIT 1;
			SELECT null INTO @disable_triggers;", new object[]{_lineId});
			}
		}
}