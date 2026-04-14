using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeWODetailHistory
	/// </summary>
	public class NeWODetailHistory
		{
		public int id					  { get; set; }
		public int woprog_id			  { get; set; }
		public int rec_no				  { get; set; }
		public string type				  { get; set; }
		public int master_id			  { get; set; }
		public DateTime date_added		  { get; set; }
		public DateTime date_modified	  { get; set; }
		public string description		  { get; set; }
		public double qty_committed		  { get; set; }
		public double qty_invoiced		  { get; set; }
		public double qty_ordered		  { get; set; }
		public double price_cost		  { get; set; }
		public double price_sell		  { get; set; }
		public double price_unit		  { get; set; }
		public int added_by				  { get; set; }
		public int tax1					  { get; set; }
		public int tax2					  { get; set; }
		public int tax3					  { get; set; }
		public int tax4					  { get; set; }
		public int business_unit_id			  { get; set; }
		public int bvwo					  { get; set; }
		public string code				  { get; set; }
		public string origin			  { get; set; }
		public int billtypeid			  { get; set; }
		public string issues			  { get; set; }
		public int memberid				  { get; set; }
		public int paytypeid			  { get; set; }
		public string notes				  { get; set; }
		public double discount			  { get; set; }
		public bool track_part			  { get; set; }
		public DateTime date_required	  { get; set; }
		public int consignment_id		  { get; set; }
		public int location_id			  { get; set; }
		public string added_by_module	  { get; set; }
		public bool is_active			  { get; set; }
		public bool blocks_schedule		  { get; set; }
		public string gl_account { get; set; }
		public int currency_id { get; set; }

		public NeWODetailHistory(){}
		public NeWODetailHistory(int _id)
			{
			load(_id);
			}

		public static Toolbox.boolstr can_move_to_history(int _woprog_id)
			{
			var this_boolstr		= new Toolbox.boolstr();
			var history_count					= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_history WHERE wo_detail_history_woprog_id =@v0", _woprog_id);
			var current_count					= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id =@v0", _woprog_id);
			if(history_count > 0)
				{
				this_boolstr.success			= false;
				this_boolstr.message			= "Lines exist already for this work order in the history table";
				}
			else if(current_count == 0)
				{
				this_boolstr.success			= false;
				this_boolstr.message			= "No lines exist that can be moved.";
				}
			else
				{
				this_boolstr.success			= true;
				this_boolstr.message			= "Lines can be moved";
				}
			return this_boolstr;
			}

		public static void move_to_history(int _woprog_id)
			{

            try
            {
                var _conn = Toolbox.connect();

                Toolbox.doSQL_void(_conn, "call wo_detail_move_lines(@v0,@v1)", new object[] { _woprog_id ,"history" });
                
            }
            catch (Exception ee)
            {
                throw new Exception("Work order lines could not be moved - Reason given: " + ee);
            }
			}

        /*
         * Why No need to consider any changes to below function in terms of 1275:  https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1275/
         * ---------------------------------------------------------------------------------------------------------------------
         * This one (below save()) is only called from 'section\workorder\picklist.aspx.cs' file
         * when the work order status is in one of [OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced, "Waiting For PO"]
         * to add a 55560 line item.
         *
         * Based on this, no any netsuite_** info will be provided from the picklist page. In other words the history record is generated directly not by moving.
         */
        public void save()
			{
			var _tools				= new Toolbox();
			// Only insert is allowed.
			var my_conn		= new MySqlConnection();
			my_conn.ConnectionString	= _tools.connection_string;
			my_conn.Open();
			var my_comm		= new MySqlCommand();
			my_comm.Connection			= my_conn;
			if(id == 0)
				{
				my_comm.CommandText			= @"
INSERT INTO wo_detail_history 
	(
    wo_detail_history_woprog_id,
    wo_detail_history_rec_no,
    wo_detail_history_type,
    wo_detail_history_master_id,
    wo_detail_history_date_added,
    wo_detail_history_date_modified,
    wo_detail_history_description,
    wo_detail_history_qty_committed,
    wo_detail_history_qty_invoiced,
    wo_detail_history_price_cost,
    wo_detail_history_price_sell,
    wo_detail_history_price_unit,
    wo_detail_history_added_by,
    wo_detail_history_tax1,
    wo_detail_history_tax2,
    wo_detail_history_tax3,
    wo_detail_history_tax4,
    business_unit_id,
    wo_detail_history_bvwo,
    wo_detail_history_code,
    wo_detail_history_origin,
    wo_detail_history_billtypeid,
    wo_detail_history_issues,
    memberid,
    paytypeid,
    wo_detail_history_notes,
    wo_detail_history_qty_ordered,
    wo_detail_history_discount,
    wo_detail_history_track_part,
    wo_detail_history_date_required,
    wo_detail_history_consignment_id,
    wo_detail_history_location_id,
    wo_detail_history_added_by_module,
    is_active,
    blocks_schedule
	) 
VALUES
    (
	@woprog_id,
	@rec_no,
	@type,
	@master_id,
	@date_added,
	NOW(),
	@description,
	@qty_committed,
	@qty_invoiced,
	@price_cost,
	@price_sell,
	@price_unit,
	@added_by,
	@tax1,
	@tax2,
	@tax3,
	@tax4,
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
	@blocks_schedule
    )";
				}
			else
				{
				my_comm.CommandText	= @"
UPDATE 
	wo_detail_history 
SET
	wo_detail_history_woprog_id		   = @woprog_id,
	wo_detail_history_rec_no		   = @rec_no,
	wo_detail_history_type			   = @type,
	wo_detail_history_master_id		   = @master_id,
	wo_detail_history_date_added	   = @date_added,
	wo_detail_history_date_modified	   = @date_modified,
	wo_detail_history_description	   = @description,
	wo_detail_history_qty_committed	   = @qty_committed,
	wo_detail_history_qty_invoiced	   = @qty_invoiced,
	wo_detail_history_price_cost	   = @price_cost,
	wo_detail_history_price_sell	   = @price_sell,
	wo_detail_history_price_unit	   = @price_unit,
	wo_detail_history_added_by		   = @added_by	,
	wo_detail_history_tax1			   = @tax1,
	wo_detail_history_tax2			   = @tax2,
	wo_detail_history_tax3			   = @tax3,
	wo_detail_history_tax4			   = @tax4,
	business_unit_id				   = @business_unit_id,
	wo_detail_history_bvwo			   = @bvwo,
	wo_detail_history_code			   = @code,
	wo_detail_history_origin		   = @origin,
	wo_detail_history_billtypeid	   = @billtypeid,
	wo_detail_history_issues		   = @issues,
	memberid						   = @memberid,
	paytypeid						   = @paytypeid,
	wo_detail_history_notes			   = @notes,
	wo_detail_history_qty_ordered	   = @qty_ordered,
	wo_detail_history_discount		   = @discount,
	wo_detail_history_track_part	   = @track_part,
	wo_detail_history_date_required	   = @date_required,
	wo_detail_history_consignment_id   = @consignment_id,
	wo_detail_history_location_id	   = @location_id,
	wo_detail_history_added_by_module  = @added_by_module,
	is_active						   = @is_active,
	blocks_schedule					   = @blocks_schedule
WHERE 
	wo_detail_history_id = @id 
LIMIT 1
";
				}
			my_comm.Parameters.AddWithValue("@id", id);
			my_comm.Parameters.AddWithValue("@woprog_id", woprog_id);
			my_comm.Parameters.AddWithValue("@rec_no", rec_no);
			my_comm.Parameters.AddWithValue("@type", type);
			my_comm.Parameters.AddWithValue("@master_id", master_id);
			my_comm.Parameters.AddWithValue("@date_added", date_added);
			my_comm.Parameters.AddWithValue("@description", description);
			my_comm.Parameters.AddWithValue("@qty_committed", qty_committed);
			my_comm.Parameters.AddWithValue("@qty_invoiced", qty_invoiced);
			my_comm.Parameters.AddWithValue("@price_cost", price_cost);
			my_comm.Parameters.AddWithValue("@price_sell", price_sell);
			my_comm.Parameters.AddWithValue("@price_unit", price_unit);
			my_comm.Parameters.AddWithValue("@added_by", added_by);
			my_comm.Parameters.AddWithValue("@tax1", tax1);
			my_comm.Parameters.AddWithValue("@tax2", tax2);
			my_comm.Parameters.AddWithValue("@tax3", tax3);
			my_comm.Parameters.AddWithValue("@tax4", tax4);
			my_comm.Parameters.AddWithValue("@business_unit_id", business_unit_id);
			my_comm.Parameters.AddWithValue("@bvwo", bvwo);
			my_comm.Parameters.AddWithValue("@code", code);
			my_comm.Parameters.AddWithValue("@origin", origin);
			my_comm.Parameters.AddWithValue("@billtypeid", billtypeid);
			my_comm.Parameters.AddWithValue("@issues", issues);
			my_comm.Parameters.AddWithValue("@memberid", memberid);
			my_comm.Parameters.AddWithValue("@paytypeid", paytypeid);
			my_comm.Parameters.AddWithValue("@notes", notes);
			my_comm.Parameters.AddWithValue("@qty_ordered", qty_ordered);
			my_comm.Parameters.AddWithValue("@discount", discount);
			my_comm.Parameters.AddWithValue("@track_part", track_part);
			my_comm.Parameters.AddWithValue("@date_required", date_required);
			my_comm.Parameters.AddWithValue("@consignment_id", consignment_id);
			my_comm.Parameters.AddWithValue("@location_id", location_id);
			my_comm.Parameters.AddWithValue("@added_by_module", added_by_module);
			my_comm.Parameters.AddWithValue("@is_active", is_active);
			my_comm.Parameters.AddWithValue("@blocks_schedule", blocks_schedule);
		
			try
				{
				my_comm.ExecuteNonQuery();
				if(id == 0)
					{
					my_comm.CommandText		= "SELECT LAST_INSERT_ID()";
					id						= Convert.ToInt32(my_comm.ExecuteScalar());
					}
				}
			catch (Exception ex)
				{
				_tools.catch_error(ex);
				throw;
				}
			}
		public bool exists(int _id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_history WHERE wo_detail_history_id = @v0 ", _id) == 1;
			}
		private void load(int _id)
			{
			if(exists(_id))
				{
				var dt	    = Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_history WHERE wo_detail_history_id = @v0 ", new object[] {  _id } );
				var dr		    = dt.Rows[0];
				id				    = _id;
				woprog_id			= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_woprog_id"]);
				rec_no				= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_rec_no"]);
				type				= Toolbox.ReturnBlankIfNull_string(dr["wo_detail_history_type"]);
				master_id			= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_master_id"]);
				date_added			= Toolbox.ReturnBlankDateTimeIfNull(dr["wo_detail_history_date_added"]);
				date_required		= Toolbox.ReturnBlankDateTimeIfNull(dr["wo_detail_history_date_required"]);
				description			= Toolbox.ReturnBlankIfNull_string(dr["wo_detail_history_description"]);
				qty_committed		= Toolbox.ReturnZeroIfNull_double(dr["wo_detail_history_qty_committed"]);
				qty_invoiced		= Toolbox.ReturnZeroIfNull_double(dr["wo_detail_history_qty_invoiced"]);
				price_cost			= Toolbox.ReturnZeroIfNull_double(dr["wo_detail_history_price_cost"]);
				price_sell			= Toolbox.ReturnZeroIfNull_double(dr["wo_detail_history_price_sell"]);
				price_unit			= price_sell;
				added_by			= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_added_by"]);
				tax1				= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_tax1"]);
				tax2				= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_tax2"]);
				tax3				= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_tax3"]);
				tax4				= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_tax4"]);
				business_unit_id			= Toolbox.ReturnZeroIfNull_int(dr["business_unit_id"]);
				bvwo				= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_bvwo"]);
				code				= Toolbox.ReturnBlankIfNull_string(dr["wo_detail_history_code"]);
				origin				= Toolbox.ReturnBlankIfNull_string(dr["wo_detail_history_origin"]);
				billtypeid			= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_billtypeid"]);
				issues				= Toolbox.ReturnBlankIfNull_string(dr["wo_detail_history_issues"]);
				memberid			= Toolbox.ReturnZeroIfNull_int(dr["memberid"]);
				paytypeid			= Toolbox.ReturnZeroIfNull_int(dr["paytypeid"]);
				qty_ordered			= Toolbox.ReturnZeroIfNull_double(dr["wo_detail_history_qty_ordered"]);
				discount			= Toolbox.ReturnZeroIfNull_double(dr["wo_detail_history_discount"]);
				track_part			= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_track_part"]) == 1;
				consignment_id		= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_consignment_id"]);
				added_by_module		= Toolbox.ReturnBlankIfNull_string(dr["wo_detail_history_added_by_module"]);
				location_id			= Toolbox.ReturnZeroIfNull_int(dr["wo_detail_history_location_id"]);
				is_active			= Toolbox.ReturnZeroIfNull_int(dr["is_active"]) == 1;
				blocks_schedule		= Toolbox.ReturnZeroIfNull_int(dr["blocks_schedule"]) == 1;
				}
			}
		public static DataTable Lines(int _woprog_id)
			{
			return Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_history WHERE wo_detail_history_woprog_id = @v0  ORDER BY wo_detail_history_rec_no", new object[] {  _woprog_id } );
			}

		}
	}