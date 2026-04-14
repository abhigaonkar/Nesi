using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Data;
using System.Linq;
using DevExpress.Web;
using DevExpress;
using System.Web.Services;

using System.Web.UI.WebControls;
using System.Web;
using MySql.Data.MySqlClient;
using nesi.core;

public partial class sections_member_inventory_annual_counts : Page
	{
	NeMember current_user;
	static int _page_id = 119;
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	public static NeBusinessUnit WorkingBusinessUnit;
	DataTable master_locations;
	bool write_access = false;
	bool do_remove = false;
    ASPxHiddenField h;
    SqlDataSource ds_templates;
    ASPxDropDownEdit dde_filter;
    Panel panel_export;
    public int update_rows_counter;
	MySqlConnection conn	= new MySqlConnection();
	protected void Page_PreInit(object _sender, EventArgs _e)
		{
		conn				= Toolbox.connect();
		}
	protected void Page_Unload(object _sender, EventArgs _e)
		{
		if (conn.State != ConnectionState.Open) return;
		conn.Close();
		conn.Dispose();
		}
	protected void Page_Init(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		write_access					= current_user.AuthenticatedForPrivilege(188);
		if (!IsCallback && !IsPostBack)
			{
                Session["working_business_unit_id"] = null;
			pc.ActiveTabIndex			= 0;
			}
       
		master_locations				= Toolbox.doSQL_dt(conn,@"SELECT id,name FROM inventory_location_master"  , null);
		bt_export_wo_snapshot.Visible				= write_access;
		gv_remaining.Settings.ShowTitlePanel		= write_access;

        layout.__page_name = "annual_counts";
        layout.used_gv = gv_remaining;
        h = (ASPxHiddenField)layout.FindControl("h");
        ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
        dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
        panel_export = (Panel)layout.FindControl("panel_export");
        panel_export.Visible = true;
        
        h.Set("gridview_id", "gv_remaining");
        ds_templates.SelectParameters["@page_name"].DefaultValue = "annual_counts";
        ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
      
		}
	protected void Page_Load(object sender, EventArgs e)
		{
//		check_annuals();
		var gv = (ASPxGridView)gv_remaining;
		var btnZeroOut = (ASPxButton)gv.FindTitleTemplateControl("btnZeroOut");

        if (gv_schedules.Selection.Count > 0)
        {
			var row = gv_schedules.GetSelectedFieldValues("id")[0];
            Session["working_business_unit_id"] = gv_schedules.GetRowValuesByKeyValue(row, "business_unit_id");
            Session["working_dte_datestart"] = Toolbox.ReturnBlankDateTimeIfNull(gv_schedules.GetRowValuesByKeyValue(row, "start_date")).ToString("yyyy-MM-dd");
            Session["working_dte_dateend"] = Toolbox.ReturnBlankDateTimeIfNull(gv_schedules.GetRowValuesByKeyValue(row, "end_date")).ToString("yyyy-MM-dd");
            WorkingBusinessUnit = new NeBusinessUnit(Session["working_business_unit_id"]);
        }

       
//		handle_toggle_text();

		if (!IsPostBack)
			{
			Session["annual_counts_gv"] = null;
			var gl = new NeGridLayouts(current_user.id, "annual_counts");
			update_rows_counter = 0;
			if (gl.GridLayoutID == 0)
				{
				gl.GridLayout_Layout = gv_remaining.SaveClientLayout();

				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = "annual_counts";
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_remaining.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text = gl.GridLayout_Name;
			}
		}

	protected void cb_saveminmax_Callback(object source, CallbackEventArgs e)
		{
		markertable mmo;
		try
			{
			mmo = jSON.Deserialize<markertable>(e.Parameter);
			}
		catch
			{
			throw new Exception("I think you're trying to save an invalid number.");
			}
		var business_unit_id = Toolbox.doSQL_string(conn, @"SELECT business_unit_id FROM inventory_annual_counts WHERE id =@v0 LIMIT 1", new object[] { mmo.id});
		if (Toolbox.doSQL_int(conn, @"Select count(id) from Inventory_Annual_Counts where (date_transferred is null OR date_transferred > DATE_SUB(NOW(), INTERVAL 1 MONTH))
and markerid = @v0 and business_unit_id = @v1 AND id != @v2 LIMIT 1", new object[] { mmo.markerid, business_unit_id, mmo.id}) > 0)
			{
			throw new Exception("DUPLICATION ALERT...This marker has already been used, within the past month, for this company.");
			}
		if (mmo.onhand < 0)
			{
			throw new Exception("You can not set the onhand qty to a negative number.");
			}
		if (Toolbox.doSQL_int(conn, @"Select count(id) from Inventory_Annual_Counts where (date_transferred is null OR date_transferred > DATE_SUB(NOW(), INTERVAL 1 MONTH))
and business_unit_id = @v0 and locationid = @v1 and masterid = @v2 AND id != @v3 LIMIT 1", new object[] { business_unit_id, mmo.location, mmo.master_id, mmo.id}) > 0)
			{
			throw new Exception("DUPLICATION ALERT...This part has already been saved in this location for this company.");
			}
		Toolbox.doSQL_void(conn, @"update Inventory_Annual_Counts set markerid = @v0,qty=@v1, locationid=@v2 where id = @v3 LIMIT 1", new object[] {
		mmo.markerid, mmo.onhand, mmo.location, mmo.id});

		}
	protected class markertable
		{
		private int _master_id;
		private double _onhand;
		private string _location;
		private int _markerid;
		private int _id;

		public int id { get { return _id; } set { _id = Convert.ToInt32(value); } }
		public int master_id { get { return _master_id; } set { _master_id = Convert.ToInt32(value); } }
		public double onhand { get { return _onhand; } set { _onhand = Convert.ToDouble(value); } }
		public string location { get { return _location; } set { _location = Convert.ToString(value); } }
		public int markerid { get { return _markerid; } set { _markerid = Convert.ToInt32(value); } }

		}
	protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
		{
            Session["annual_counts_gv"] = null;
			var business_unit_id = gv_schedules.GetSelectedFieldValues("business_unit_id")[0];
            Session["working_business_unit_id"] = business_unit_id;
              pc.TabPages[0].Text = gv_schedules.GetRowValuesByKeyValue(gv_schedules.GetSelectedFieldValues("id")[0], "name") + " Items";
			gv_remaining.DataBind();
         
             
		gv_zeroed.DataBind();
		}
	protected void btnZeroOut_Click(object sender, EventArgs e)
		{
		var dt_start = DateTime.Now;
		using (var conn = Toolbox.connect())
			{
			var business_unit_id = Convert.ToInt32(Session["working_business_unit_id"]);
			var dt_remaining2 = Toolbox.doSQL_dt(conn,@"call get_inventory_locations_with_qtys(@v0 ,@v1 ,@v2 )", new object[] {  business_unit_id, Convert.ToDateTime(Session["working_dte_datestart"]).ToString("yyyy-MM-dd"), Convert.ToDateTime(Session["working_dte_dateend"]).ToString("yyyy-MM-dd") } );
			var dt_remaining = dt_remaining2.Select("[post_qty] is null ").CopyToDataTable();
			var i = 0;
			DataTable dt_costs;
			var list_hist = new List<history_vars>();
			foreach (DataRow dr in dt_remaining.Rows)
				{
				var location_master_id = Convert.ToInt32(dr["location_master_id"]);
				var ib = new branch(dr["master_id"], business_unit_id, conn);
				var il = new location(location_master_id, WorkingBusinessUnit.id, Convert.ToInt32(dr["master_id"]), conn);
				var prev_qty = il.qty;
				//  var this_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  il.master_id, il.business_unit_id } );
				il.log_is_manual = true;
				il.member_id = current_user.id;
				il.alert_worthy = false;

				var diff = il.qty - 0;
				if (diff != 0)
					{
					il.qty = 0;
					il.section_id = 10;
					var this_type = prev_qty > 0 ? 3 : 2;
					il.save(conn);
					il.update_branch(this_type, ib.dollar_balance, diff, Convert.ToDouble(dr["cost"]), ib, conn);
					}
				var cost = Convert.ToDouble(dr["cost"]);
				var h = new history_vars
							{
								business_unit_id = WorkingBusinessUnit.id,
								cost = cost,
								location_master_id = location_master_id,
								master_id = (int)dr["master_id"],
								member_id = current_user.id32,
								origin = "zero out",
								post_dollar_balance = 0,
								post_onhandqty = 0,
								pre_dollar_balance = Math.Round(prev_qty * cost, 5, MidpointRounding.AwayFromZero),
								pre_onhandqty = prev_qty,
								qty = 0,
								when_ts = DateTime.Now
							};

				list_hist.Add(h);
				i++;
				}
			if (list_hist.Any())
				{
				add_history_line(list_hist);
				}
			gv_remaining.DataBind();
			gv_zeroed.DataBind();
			}
		var dt_end = DateTime.Now;
		var diff_tot = dt_start.Subtract(dt_end).Seconds;
		}
	
	private void add_history_line(
			int business_unit_id, 
			int master_id, 
			double qty, 
			int location_master_id, 
			int member_id, 
			string origin,

			double previous_cost,
			double prev_dollar_balance,
			double post_dollar_balance,
			double pre_onhand_qty,
			double post_onhand_qty
			)
		{
		Toolbox.doSQL_void(conn, @"
INSERT INTO inventory_annual_counts_history
	(
	business_unit_id,
	masterid,
	qty,
	locationid,
	memberid,
	date_transferred,
	origin,
	cost,
	prev_dollar_balance,
	post_dollar_balance,
	pre_onhand_qty,
	post_onhand_qty
	)
VALUES 
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	NOW(),
	@v5,
	@v6,
	@v7,
	@v8,
	@v9,
	@v10
	)
", new object[] {
	business_unit_id,				// @v0
	master_id,				// {1}
	qty,					// {2}
	location_master_id,		// {3}
	member_id,				// {4}
	origin,					// {5}
	previous_cost,			// {6}
	prev_dollar_balance,	// {7}
	post_dollar_balance,	// {8}
	pre_onhand_qty,			// {9}
	post_onhand_qty			// {10}
 });
		}
	private struct history_vars
		{
		public int business_unit_id             { get; set; }
		public int master_id               { get; set; }
		public double qty                  { get; set; }
		public int location_master_id      { get; set; }
		public int member_id               { get; set; }
		public string origin               { get; set; }
		public double cost                 { get; set; }
		public double pre_dollar_balance   { get; set; }
		public double post_dollar_balance  { get; set; }
		public double pre_onhandqty        { get; set; }
		public double post_onhandqty       { get; set; }
		public DateTime when_ts			   { get; set; }
		}
	private void add_history_line(List<history_vars> list_vars)
		{
		var sb		= new System.Text.StringBuilder();
		sb.Append(@"
INSERT INTO inventory_annual_counts_history
	(
	business_unit_id,
	masterid,
	qty,
	locationid,
	memberid,
	date_transferred,
	origin,

	cost,
	prev_dollar_balance,
	post_dollar_balance,
	pre_onhand_qty,
	post_onhand_qty
	)
VALUES 
");
		for(var i = 0; i < list_vars.Count(); i++)
			{
			var hv		= list_vars[i];
			sb.AppendFormat(@"	
	(
	'{0}',
	'{1}',
	'{2}',
	'{3}',
	'{4}',
	'{11}',
	'{5}',
	'{6}',
	'{7}',
	'{8}',
	'{9}',
	'{10}'
	)", 
	hv.business_unit_id,						// {0}
	hv.master_id,						// {1}
	hv.qty,								// {2}
	hv.location_master_id,				// {3}
	hv.member_id,						// {4}
	hv.origin,							// {5}
	hv.cost,							// {6}
	hv.pre_dollar_balance,				// {7}
	hv.post_dollar_balance,				// {8}
	hv.pre_onhandqty,					// {9}
	hv.post_onhandqty,					// {10}
	Toolbox.MySQL_longdt(hv.when_ts)	// {11}
			);
			if(i < (list_vars.Count() - 1))
				{
				sb.Append(",");
				}
			}
		//FIXING
		Toolbox.doSQL_void(conn, sb.ToString(),null);
		}
	
	protected void btnZeroOut_DataBinding(object sender, EventArgs e)
		{
		var gv = (ASPxGridView)gv_remaining;
		var btnZeroOut = (ASPxButton)gv.FindTitleTemplateControl("btnZeroOut");
		//btnZeroOut.Text = "Update " + gv.VisibleRowCount + " Locations to 0";
        
    //    DataTable dt = Toolbox.doSQL_datatable(@"call get_inventory_locations_with_qtys(@v0 ,@v1 ,@v2 )", new object[] {  c.id, dte_datestart.Date.ToString("yyyy-MM-dd"), dte_dateend.Date.ToString("yyyy-MM-dd") } );
        if (Session["annual_counts_gv"] != null)
        {
            var dt = (DataTable)Session["annual_counts_gv"];
            btnZeroOut.Text = "Update " + dt.Select("[post_qty] is null").Length + " UNCOUNTED locations to 0";
        }
		}
	[WebMethod]
	public static string reset_minmax(int business_unit_id)
		{
		using(var conn = Toolbox.connect())
			{
		if(business_unit_id >= 1)
			{
			Toolbox.doSQL_void(conn,@"UPDATE inventory_location SET min=0,max=0 
WHERE business_unit_id = @v0 AND location_master_id IN (SELECT id FROM inventory_location_master WHERE type_id = '2' AND business_unit_id = @v0)", new object[] { business_unit_id});
			return "SUCCESS";
			}
		else
			{
			return "Failed. Invalid Branch ID";
			}
			}
		
		}
	[WebMethod]
	public static string save_marker(int marker_id, int master_id, int location_master_id, double qty)
		{
		using(var conn = Toolbox.connect())
			{
		var current_user = Toolbox.do_handle_authentication(119);
		var working_business_unit_id = Convert.ToInt32(HttpContext.Current.Session["working_business_unit_id"]);

		if (Toolbox.doSQL_int(conn, @"SELECT count(id) FROM inventory_annual_counts WHERE (date_transferred is null OR date_transferred > DATE_SUB(NOW(), INTERVAL 1 MONTH))
and markerid = @v0 and business_unit_id = @v1", new object[] { marker_id, working_business_unit_id}) > 0)
			{
			throw new Exception("DUPLICATION ALERT...This marker has already been used, within the past month, for this company.");
			}

		if (Toolbox.doSQL_int(conn,@"SELECT count(id) FROM inventory_annual_counts WHERE (date_transferred is null OR date_transferred > DATE_SUB(NOW(), INTERVAL 1 MONTH)) 
and business_unit_id = @v0 and locationid = @v1 and masterid = @v2", new object[] { working_business_unit_id, location_master_id, master_id}) > 0)
			{
			throw new Exception("DUPLICATION ALERT...This part has already been saved (within the past month) in this location for this company.");
			}

		Toolbox.doSQL_void(conn, @"INSERT INTO inventory_annual_counts (business_unit_id,masterid,qty,locationid,markerid,ts,memberid) 
VALUES (@v0,@v1,@v2,@v3,@v4,curdate(),@v5)", new object[] { working_business_unit_id, master_id, qty, location_master_id, marker_id, current_user.id});
		HttpContext.Current.Session["gv_annual_counts"] = null;
		var dt		= Toolbox.doSQL_dt(conn,@" SELECT a.id, a.business_unit_id, a.masterid, a.qty, a.locationid, a.markerid, a.ts, a.memberid, a.date_transferred, b.cost from inventory_annual_counts a LEFT JOIN inventory_cost b ON b.master_id = a.masterid and b.business_unit_id = @v0  WHERE a.business_unit_id = @v0  ORDER BY a.id desc", new object[] {  working_business_unit_id } );
		HttpContext.Current.Session["gv_annual_counts"] = dt;
		return "SUCCESS";
		}
		}
	protected void bt_export_Click(object sender, EventArgs e)
		{
            var c = new NeBusinessUnit(Session["working_business_unit_id"]);
		gv_export.GridViewID	= "gv_remaining";
		gv_export.FileName		= c.name+" - Barcode (Remaining Items)";
		gv_export.WriteXlsxToResponse(true);
		}
	protected void gv_export_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
		{
		var gve	= (ASPxGridViewExporter) sender;
		var gv				= gve.GridView;
		if(e.RowType == GridViewRowType.Data)
			{ 
			if(e.Column.Caption == "Cost" && e.VisibleIndex >= 0)
				{
				e.Text							= "";
				e.TextValue						= "";
				//string master_location_id		= gv.GetRowValues(e.VisibleIndex, "locationid").ToString();
				//DataRow[] dr					= master_locations.Select("id = "+master_location_id);
				//string location_name			= dr == null || dr.Length == 0  ? "???" : dr[0]["name"].ToString();
				//e.Text							= location_name;
				}
			}
		}
	protected void cb_zero_Callback(object source, CallbackEventArgs e)
		{
		if(e.Parameter.Contains("|"))
			{
			var paras				= e.Parameter.Split('|');
			var location_master_id		= Convert.ToInt32(paras[0]);
			var master_id				= Convert.ToInt32(paras[1]);
			var business_unit_id				= Convert.ToInt32(paras[2]);
			var this_cost			= Toolbox.doSQL_double(conn,@"SELECT IFNULL(MAX(cost),0) FROM inventory_cost WHERE master_id = @v0  AND business_unit_id = @v1 ", new object[] {  master_id, business_unit_id } );
			var pre_ib		= new branch(master_id, business_unit_id, conn);
			location.zero_out(10, location_master_id, business_unit_id, master_id, this_cost, current_user.id, conn);
			var post_ib	= new branch(master_id, business_unit_id, conn);
			add_history_line(business_unit_id, master_id, 0, location_master_id, current_user.id32, "zero out", this_cost, pre_ib.dollar_balance, post_ib.dollar_balance, pre_ib.onhand_qty, post_ib.onhand_qty);
			}
		}
	protected void gv_zeroed_DataBound(object sender, EventArgs e)
		{
	//	pc.TabPages[2].Text					= "Zeroed Out Entries ("+gv_zeroed.VisibleRowCount+")"; 
		}
	protected void bt_export_zeroed_Click(object sender, EventArgs e)
		{
            var c = new NeBusinessUnit(Session["working_business_unit_id"]);
		gv_export.GridViewID	= "gv_zeroed";
		gv_export.FileName		= c.name+" - Barcode (Zeroed Out Items)";
		gv_export.WriteXlsxToResponse(true);
		}
	protected void bt_export_wo_snapshot_Click(object sender, EventArgs e)
		{
		var agv						= new ASPxGridView();
        var c = new NeBusinessUnit(Session["working_business_unit_id"]);
		if(c.name == null) return;
		agv.ID									= "gv_openworkorders";
		agv.AutoGenerateColumns					= true;
		var dt							= Toolbox.doSQL_dt(conn , string.Format(@"
		SELECT 
			a.master_id, 
			a.description, 
			a.qty_committed, 
			IF(type = 'L', 0, a.price_cost) price_cost_mat, 
			IF(type = 'M', 0, a.price_cost) price_cost_lab, 
			IF(type = 'L', 0, a.price_cost * a.qty_committed) price_cost_mat_extd, 
			IF(type = 'M', 0, a.price_cost * a.qty_committed) price_cost_lab_extd, 
			b.woprog_bvwo wo_number, 
			b.woprog_customername customer_name, 
			b.woprog_status wo_status, 
			Division_BVdept department,
			billtype,
			(a.qty_committed * a.price_sell) Extd_Sell ,
			IF(IFNULL(d.is_consumable, 0) = 0, False, True) is_consumable
		FROM wo_detail a 
		LEFT JOIN woprog b ON a.woprog_id = b.woprog_id 
		LEFT JOIN inventory_item_master c ON a.master_id = c.master_id
		LEFT JOIN inventory_tag d ON c.tag_id = d.tag_id
		WHERE 
			business_unit_id = '{0}' AND 
			b.woprog_status NOT IN ('Waiting to be invoiced', 'Invoiced', 'Deleted')
		UNION ALL
		SELECT 
			a.master_id, 
			a.description, 
			a.qty_committed, 
			IF(type = 'L', 0, a.price_cost) price_cost_mat, 
			IF(type = 'M', 0, a.price_cost) price_cost_lab, 
			IF(type = 'L', 0, a.price_cost * a.qty_committed) price_cost_mat_extd, 
			IF(type = 'M', 0, a.price_cost * a.qty_committed) price_cost_lab_extd, 
			b.woprog_bvwo wo_number, 
			b.woprog_customername customer_name,
			 b.woprog_status wo_status, 
			 Division_BVdept department,
			 billtype,
			 (a.qty_committed * a.price_sell) Extd_Sell,
			IF(IFNULL(d.is_consumable, 0) = 0, False, True) is_consumable
			FROM 
				wo_detailh a 
			LEFT JOIN 
				woprog b ON a.woprog_id = b.woprog_id 
			LEFT JOIN inventory_item_master c ON a.master_id = c.master_id
			LEFT JOIN inventory_tag d ON c.tag_id = d.tag_id
			WHERE 
				business_unit_id = '{0}' AND 
				b.woprog_status IN ('Waiting to be invoiced')", Session["working_business_unit_id"]),null);

        agv.DataSource							= dt;
		Controls.Add(agv);
		agv.DataBind();
		var gve_wo_snapshot	= new ASPxGridViewExporter();
		gve_wo_snapshot.FileName				= "("+DateTime.Now.ToString("yyyy-MM-dd -- hh mm tt")+") Open WOS - "+c.name.Replace(" ", "_");
		Controls.Add(gve_wo_snapshot);
        gve_wo_snapshot.ExportedRowType			= GridViewExportedRowType.All;
		gve_wo_snapshot.GridViewID				= agv.ID;
		gve_wo_snapshot.WriteXlsxToResponse();
		Controls.Remove(agv);
		Controls.Remove(gve_wo_snapshot);
		}

	protected void gv_remaining_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        update_rows_counter++;
        var master_id = Convert.ToInt32(gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "master_id"));
        // force is_replace logic to look at live data and not the stale gridview data
		var bo_obj					= new branch_options(WorkingBusinessUnit.id);
        var location_master_id		= Convert.ToInt32(gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "location_master_id"));
		var dt_lastedits			= Toolbox.doSQL_dt(@"SELECT MAX(ts)ts FROM 
inventory_annual_counts_history WHERE business_unit_id = @v0  AND masterid IN (@v3) 
AND locationid = @v4  AND DATE(ts) BETWEEN @v1 AND @v2
GROUP BY masterid,locationid ", new object[] {  WorkingBusinessUnit.id,bo_obj.annual_count_start_date, bo_obj.annual_count_end_date, master_id, location_master_id } );
		var last_edit_db			= new DateTime();
		var cached_lastedit			= gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "last_inv_count").ToString();
		var last_edit_dt			= cached_lastedit == "" ? new DateTime() : Convert.ToDateTime(cached_lastedit);
		if(dt_lastedits.Rows.Count > 0)
			{
			last_edit_db			= Toolbox.ReturnBlankDateTimeIfNull(dt_lastedits.Rows[0]["ts"]);
			}
		if(last_edit_db != last_edit_dt)
			{
            throw new Exception(string.Format("Part {0} was updated since this page was last loaded, please refresh.", master_id));
			}
        var is_replace = cached_lastedit == "";
        var newqty = Convert.ToDouble(e.NewValues["newqty"]);
        var cost = Convert.ToDouble(gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "cost"));
       
        var id = gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "id").ToString();
        if (cost == 0)
        {
            cost = Toolbox.doSQL_double(conn,@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  master_id, WorkingBusinessUnit.id } );
        }

        #region update branch inventory locations
        var ib = new branch(master_id, WorkingBusinessUnit.id, conn);
        var il = new location(location_master_id, WorkingBusinessUnit.id, master_id, conn);
        var prev_qty = il.qty;
      //  var this_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  il.master_id, il.business_unit_id } );
        il.log_is_manual = true;
        il.member_id = current_user.id;
        il.alert_worthy = false;
        var diff = is_replace?il.qty - newqty: newqty;
        il.qty = !is_replace ?  il.qty + newqty : newqty;
        il.section_id = 10;
        var this_type = prev_qty > newqty ? 3 : 2;
   //     var pre_ib = new inventory_branch(il.master_id, (int)il.business_unit_id);
        il.save(conn);
       il.update_branch(this_type, ib.dollar_balance, diff, cost, ib, conn);
   //     var post_ib = new inventory_branch(il.master_id, (int)il.business_unit_id);

        #endregion

        #region create annual count log

        var list_hist = new List<history_vars>();
		var h = new history_vars
					{
						business_unit_id = WorkingBusinessUnit.id,
						cost = Convert.ToDouble(cost),
						location_master_id = Convert.ToInt32(gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "location_master_id")),
						master_id = Convert.ToInt32(gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "master_id")),
						member_id = current_user.id32,
						origin = "desktop",
						post_dollar_balance = is_replace ? (newqty*cost) : (newqty + Convert.ToDouble(gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "qty")))*cost,
						post_onhandqty = is_replace ? (newqty) : (newqty + Convert.ToDouble(gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "qty"))),
						pre_dollar_balance = Convert.ToDouble(gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "dollar_balance")),
						pre_onhandqty = Convert.ToDouble(gv_remaining.GetRowValuesByKeyValue(e.Keys[0], "qty")),
						qty = newqty,
						when_ts = DateTime.Now
					};

		list_hist.Add(h);
        add_history_line(list_hist);

        #endregion
   
        e.Cancel = true;

        if (update_rows_counter == e.NewValues.Count)
        {
            update_rows_counter = 0;
            Session["annual_counts_gv"] = null;
        }

    }
    protected void gv_remaining_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.VisibleIndex>=0)
        {
            if (e.DataColumn.FieldName.Equals("post_qty"))
            {
                if (!gv_remaining.GetRowValuesByKeyValue(e.KeyValue,"last_inv_count").ToString().Equals(""))
                {
                    e.Cell.Style["background-repeat"] = "no-repeat";
                    e.Cell.Style["background-position"] = "center right";
                   
                    e.Cell.Style["background-image"] = "/images/icon/icon[greyadd].gif";
                   // e.DataColumn.CellStyle.BackgroundImage.ImageUrl = "~/images/icon/icon[add].gif";
                }
            }
        }
    }
    protected void gv_remaining_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        var gv = (ASPxGridView)sender;
       
        if (e.Parameters != "")
        {
                gv.LoadClientLayout(e.Parameters);
           
        }
        else
        {
            gv.FilterExpression = "";
            for (var i = 0; i < gv.Columns.Count; i++)
            {
                if (gv.Columns[i] is GridViewDataColumn)
                {
                    var col = (GridViewDataColumn)gv.Columns[i];
                    if (col.GroupIndex > -1)
                    {
                        gv.UnGroup(col);
                    }
                    col.Visible = true;
                }
            }
        }
    }
    protected void gv_remaining_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        var gv = (ASPxGridView)sender;
        e.Properties["cpExp"] = gv.SaveClientLayout();

    }

	protected void gv_schedules_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		var dates = new object[gv.VisibleRowCount];
		for (var i = 0; i < gv.VisibleRowCount; i++)
			{
			var id			= Toolbox.ReturnZeroIfNull_int(gv.GetRowValues(i, "id")).ToString();
			var dt_start	= Toolbox.MySQL_shortdt(Toolbox.ReturnBlankDateTimeIfNull(gv.GetRowValues(i, "start_date")));
			var dt_end		= Toolbox.MySQL_shortdt(Toolbox.ReturnBlankDateTimeIfNull(gv.GetRowValues(i, "end_date")));
			dates[i]		= (dt_start==""?"0":dt_start)+"|"+(dt_end==""?"0":dt_end);
			}
		e.Properties["cp_dates"] = dates;
		}
}
