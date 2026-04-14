using System;
using System.Data;
using System.Drawing;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Utils;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web;
using NESI.Common.Models;
using nesi.core;

public partial class master_workorders : Page
	{

	private NeMember myMember;
	private const string _page_name = "MasterWorkOrder";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	static string default_filter = "";
	bool view_dollar_totals		= false;
	bool view_total_time		= false;
    bool view_costs = false;

	protected void Page_Init(object sender, EventArgs e)
		{
		myMember = Toolbox.do_handle_authentication(OpsPage.MasterWorkOrderGrid);
		layout.__page_name = _page_name;
		if (Session["working_business_unit_id"] != null)
			{
			Session.Remove("working_business_unit_id");
			}
		var char_count		= myMember.Nickname.Length + myMember.business_unit.name.Length+ 83;
		default_filter = string.Format("page1|filter{2}|[Status] Not Like '%Invoiced%' And StartsWith([PM], '{0}') And [business_unit] = '{1}'|conditions3|8|8|14|4|15|5|visible30|t0|t16|t15|t1|t2|t3|t11|t13|t4|t5|t7|t17|t18|t6|t10|t22|t12|t19|t14|t20|t9|t21|t23|t8|t24|t25|t26|t27|t28|t29|width25|32px|59px|59px|72px|162px|259px|59px|59px|78px|69px|59px|59px|59px|59px|91px|59px|59px|59px|59px|59px|174px|59px|59px|59px|50px|25px|25px|25px|25px|25px", myMember.Nickname, myMember.business_unit.name, char_count);
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.used_gv			= gv_workorders;

		h.Set("gridview_id", "gv_workorders");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
		if (Cache["ds_depend"] == null)
			{
			Cache["ds_depend"] = DateTime.Now;
			}
		view_dollar_totals			= myMember.AuthenticatedForPrivilege(OpsPrivilege.ViewDollarTotalsAllBranches);
		view_total_time				= myMember.AuthenticatedForPrivilege(OpsPrivilege.ViewTotalTimeAndMaterialValues);
        view_costs = myMember.AuthenticatedForPrivilege(OpsPrivilege.ViewCostInformationOnWorkOrders);
    }
	protected void Page_Load(object sender, EventArgs e)
		{
		var _q = Request.QueryString;
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Master Work Order Report";

		if (!IsPostBack && !IsCallback)
			{
			Session["master_wo_gv"] = null;
			}
		populate_grid();
		if (!IsCallback && !IsPostBack)
			{

			var gl = new NeGridLayouts(myMember.id, _page_name);

			var temp_company = !string.IsNullOrEmpty(_q["business_unit_id"]) ? new NeBusinessUnit(_q["business_unit_id"]) : myMember.business_unit;
			var branch_filter = !string.IsNullOrEmpty(_q["business_unit_id"]) ? string.Format("[business_unit] = '{0}'", temp_company.ddl_name) : "";
			var status = !string.IsNullOrEmpty(_q["status"]) ? string.Format(" [Status] = '{0}'", Server.UrlDecode(_q["status"])) : "";
			if (gl.GridLayoutID != 0 && _q["business_unit_id"] == null)
				{
				gv_workorders.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				dde_filter.Text = gl.GridLayout_Name;
				}
			else if (_q["business_unit_id"] != null && _q["status"] != null)
				{
				var char_count			= branch_filter.Length+status.Length+5;
				gv_workorders.LoadClientLayout(string.Format("page1|sort1|a3|filter{2}|{0} AND {1}|conditions3|8|8|14|4|15|5|visible30|t0|t16|t15|t1|t2|t3|t11|t13|t4|t5|t7|t17|t18|t6|t10|t22|t12|t19|t14|t20|t9|t21|t23|t8|t24|t25|t26|t27|t28|t29width25|32px|59px|59px|72px|162px|259px|59px|59px|78px|69px|59px|59px|59px|59px|91px|59px|59px|59px|59px|59px|174px|59px|59px|59px|50px|25px|25px|25px|25px|25px", branch_filter, status, char_count));
				gv_workorders.SettingsPager.Mode = GridViewPagerMode.ShowPager;
				}
			else
				{
				gv_workorders.LoadClientLayout(default_filter);
				gl.GridLayout_Layout = default_filter;
				gl.member_id = myMember.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				dde_filter.Text = gl.GridLayout_Name;
				}
			if (!myMember.AuthenticatedForPrivilege(OpsPrivilege.ViewGrossMarginInformation))
				{
				gv_workorders.Columns["GrossMargin"].Visible = false;
				}
			}

		}
	protected void populate_grid()
		{
			
		if (Session["master_wo_gv"] == null)
			{
				Session["itotal"] = 0;
				Session["irows"] = 0;
			var chk		= chk_includeprogress.Checked ? 1 : 0;
			var dt = Toolbox.doSQL_dt(@"CALL report_wo_master_V1(@v0 ,@v1, @v2, @v3 )", new object[] {  chk, myMember.id, view_dollar_totals, view_total_time } );
			Session["master_wo_gv"] = dt;
			}

		gv_workorders.DataSource = Session["master_wo_gv"];
		gv_workorders.DataBind();
		}
	protected void gv_workorders_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
		{
		if (e.IsGroupSummary)
			{
			var total			= Convert.ToDouble(e.Value);
			var field_name		= e.Item.FieldName;
			if (field_name == "JobCost")
				{
				e.Text = string.Format("Benchmark Sell: {0:C}", total);
				}
			else if (field_name == "QuotedAmount")
				{
				var newText = string.Format("Quoted: {0:C}", total);
				e.Text = newText;
				}
		
			else if (field_name == "Remainder")
			{
				var margin = "0%";
				var newText = string.Format("Left on Quoted Jobs: {0:C}", total);
				e.Text = newText + ", Margin on Quoted Jobs: " + margin;
			}
			else if (field_name == "to_be_billed")
			{
				e.Text = string.Format("Still to be billed: {0:C}", total);
			}
			else if (field_name == "Invoiced_Amount")
			{
				var newText = string.Format("Invoiced Amount: {0:C}", total);
				e.Text = newText;
			}
			}
		
		
		
		}
	protected void gv_workorders_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if (e.DataColumn.FieldName == "TMMargin" || e.DataColumn.FieldName == "Remainder" || e.DataColumn.FieldName == "to_be_billed" || e.DataColumn.FieldName == "wo_total" || e.DataColumn.FieldName == "gross_profit")
			{
			decimal	cv		= 0;
			decimal.TryParse(e.CellValue.ToString(), out cv);
			if (!view_total_time)
				{
				e.Cell.Text = "0";
				}
			else if (cv < 0)
				{
				e.Cell.ForeColor = Color.Red;
				}
			}
        else if (!myMember.AuthenticatedForPrivilege(OpsPrivilege.ViewTotalTimeAndMaterialValues)&&((e.DataColumn.FieldName == "lab_cost")||(e.DataColumn.FieldName == "mat_cost")))
        {
            e.Cell.Text = "";
        }
		else if (e.DataColumn.FieldName == "Notes" || e.DataColumn.FieldName == "Description")
			{
			try
				{
				e.Cell.ToolTip =Toolbox.ReturnBlankIfNull_string(e.CellValue);
				}
			catch
				{
				}
			}
		else if ((e.DataColumn.FieldName == "Invoiced_Amount") || (e.DataColumn.FieldName == "QuotedAmount") || (e.DataColumn.FieldName == "JobCost") || (e.DataColumn.FieldName == "Expected") || (e.DataColumn.FieldName == "Sales") || (e.DataColumn.FieldName == "to_be_billed") || (e.DataColumn.FieldName == "Progress") || (e.DataColumn.FieldName == "TMMargin") || (e.DataColumn.FieldName == "GrossMargin"))
			{
			if (!view_dollar_totals || !view_total_time)
				{
				e.Cell.Text = "0";
				}
			}
		    // No longer needed... runs in the stored procedure.
        //else if ((gv_workorders.Columns["invoice_lag"].Visible == true) && (e.DataColumn.FieldName == "invoice_lag"))
        //{
        //	if (gv_workorders.GetRowValues(e.VisibleIndex, "WOProg_InvoiceNo").ToString() != "")
        //	{
        //				
        //		e.Cell.Text = _tools.getSQL_int(@"select ifnull(datediff(woprog.WOProg_OpenDateTime,max(membertime.Date)),0) from membertime,woprog  where membertime.MemberTime_WOProg_id =@v0 and woprog.woprog_id =@v1 " "WOID")).ToString(, new object[] { gv_workorders.GetRowValues(e.VisibleIndex,"WOID"),gv_workorders.GetRowValues(e.VisibleIndex });
        //		Session["itotal"] = Convert.ToDouble(e.Cell.Text) + Convert.ToDouble(Session["itotal"]);
        //		Session["irows"] = Convert.ToInt16(Session["irows"]) + 1;
        //	}
        //	
        //}




    }
    protected void LinkButton1_Click(object sender, EventArgs e)
		{
		var index = (((LinkButton)sender).NamingContainer as GridViewDataRowTemplateContainer).VisibleIndex;
		var val = gv_workorders.GetRowValues(index, "WOID");
		var progress = new NeWOProg(Convert.ToInt32(val));
		ScriptManager.RegisterStartupScript(this, GetType(), "open_", "boing('../../../../wo_prog_frame.aspx?action=show&woprog_id=" + progress.woprog_id + "&business_unit_id=" + progress.business_unit_id + "&fromwo=yes','wo',950,800)", true);
		}
	protected void gv_workorders_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
		{
		try
			{
			if (gv_workorders.GetRowValues(e.VisibleIndex, "Notes").ToString().Length > 0)
				{
				// some condition
				// hide the Edit button
				if (e.ButtonType == ColumnCommandButtonType.Edit)
					e.Image.Url = "~/images/FullNotes.JPG";
				}
			}
		catch
			{

			}

		}

	    protected void txt_exp_hours_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
	        {
	        var container = ((ASPxTextBox)sender).NamingContainer as GridViewDataItemTemplateContainer;
	        var this_woprog_id = Convert.ToInt32(gv_workorders.GetDataRow(container.VisibleIndex)["WOID"]);
	        e.Properties.Add("cpwoprog_id", this_woprog_id);
	        }
	    protected void cb_exp_hours_Callback(object source, DevExpress.Web.CallbackEventArgs e)
	        {
	        var w = jSON.Deserialize<wo_exp_hours>(e.Parameter);
	        Toolbox.doSQL_void(@"UPDATE woprog SET woprog_exp_labor = @v0  WHERE woprog_id = @v1  LIMIT 1", new object[] { w.exp_hours, w.woprog_id });
	        if (Session["master_wo_gv"] != null)
	            {
	            DataTable dt = (DataTable)Session["master_wo_gv"];
	            DataRow dr = dt.Select("woprog_id=" + w.woprog_id)[0];
	            dr.BeginEdit();
	            dr["exp_hours"] = w.exp_hours;
	            dr.AcceptChanges();
	            Session["master_wo_gv"] = dt;
	            populate_grid();
	            }

	        }
    public class wo_exp_hours
    {
        private string _woprog_id;
        private string _exp_hours;
        public string woprog_id { get { return _woprog_id; } set { _woprog_id = value; } }
        public string exp_hours { get { return _exp_hours; } set { _exp_hours = value; } }
    }

    protected void ddl_ram_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
    {
        var container = ((ASPxComboBox)sender).NamingContainer as GridViewDataItemTemplateContainer;
        var this_woprog_id = Convert.ToInt32(gv_workorders.GetDataRow(container.VisibleIndex)["WOID"]);
        e.Properties.Add("cpwoprog_id", this_woprog_id);
    }
    protected void ddl_ram_Callback(object source, DevExpress.Web.CallbackEventArgs e)
    {
        var w = jSON.Deserialize<wo_acting_ram>(e.Parameter);
        Toolbox.doSQL_void(@"UPDATE woprog SET acting_ram = @v0  WHERE woprog_id = @v1  LIMIT 1", new object[] { w.acting_ram, w.woprog_id });
        if (Session["master_wo_gv"] != null)
        {
            DataTable dt = (DataTable)Session["master_wo_gv"];
            DataRow dr = dt.Select("woprog_id=" + w.woprog_id)[0];
            dr.BeginEdit();
            dr["acting_ram"] = w.acting_ram;
            dr.AcceptChanges();
            Session["master_wo_gv"] = dt;
            populate_grid();
        }

    }
    public class wo_acting_ram
    {
        private string _woprog_id;
        private string _acting_ram;
        public string woprog_id { get { return _woprog_id; } set { _woprog_id = value; } }
        public string acting_ram { get { return _acting_ram; } set { _acting_ram = value; } }
    }


    protected void de_expected_end_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
		{
		var container = ((ASPxDateEdit)sender).NamingContainer as GridViewDataItemTemplateContainer;
		var this_woprog_id = Convert.ToInt32(gv_workorders.GetDataRow(container.VisibleIndex)["WOID"]);
		e.Properties.Add("cpwoprog_id", this_woprog_id);
		}
	protected void cb_expected_date_Callback(object source, DevExpress.Web.CallbackEventArgs e)
		{
		var w = jSON.Deserialize<wo_end_date>(e.Parameter);
		var Wo = new NeWOProg(Convert.ToInt32(w.woprog_id));
		if(Wo.business_unit_id == 52)
			{
			throw new Exception("Updates are not allowed on this business unit.");
			}
		Toolbox.doSQL_void(@"UPDATE woprog SET woprog_expected_enddate = @v0  WHERE woprog_id = @v1  LIMIT 1", new object[] {  Toolbox.MySQL_shortdt(w.end_date), w.woprog_id } );
		    if (Session["master_wo_gv"] != null)
		        {
		        DataTable dt = (DataTable)Session["master_wo_gv"];
		        DataRow dr = dt.Select("woprog_id=" + w.woprog_id)[0];
                dr.BeginEdit();
		        dr["Expected_EndDate"] = Toolbox.MySQL_shortdt(w.end_date);
                dr.AcceptChanges();
		        Session["master_wo_gv"] = dt;
                populate_grid();
		        }

		    }

    public class wo_end_date
		{
		private string _woprog_id;
		private DateTime _end_date;
		public string woprog_id { get { return _woprog_id; } set { _woprog_id = value; } }
		public DateTime end_date { get { return _end_date; } set { _end_date = Convert.ToDateTime(value); } }
		}
	protected void gv_workorders_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
		if (!myMember.AuthenticatedForPrivilege(OpsPrivilege.ViewGrossMarginInformation))
			{
			gv.Columns["GrossMargin"].Visible = false;
			}
		}
	protected void ASPxButton4_Click(object sender, EventArgs e)
		{
		ASPxMemo newnote;
		ASPxTextBox woprogid;

		//  ASPxGridView grid = (ASPxGridView)sender;
		newnote = (ASPxMemo)gv_workorders.FindEditFormTemplateControl("ASPxMemo1");
		woprogid = (ASPxTextBox)gv_workorders.FindEditFormTemplateControl("txtWOPROGID");

		var notes = new NeWoProgNotes(Convert.ToInt32(woprogid.Text), "W");
		notes.woprog_project_notes_woprogid = Convert.ToInt32(woprogid.Text);
		notes.woprog_project_notes_notes = newnote.Text;
		notes.woprog_project_notes_memberid = Convert.ToInt32(myMember.id);
		notes.woprog_project_notes_Type = "W";
		notes.SaveWOProgProjectNote();
		gv_workorders.CancelEdit();
		//	gv_workorders.DataBind();
		Session["master_wo_gv"] = null;
		populate_grid();
		}
	protected void gv_workorders_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
		}
	protected void gv_workorders_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var wo = new NeWOProg(Convert.ToInt32(e.Keys["WOID"]));


		if (e.NewValues["Description"] != e.OldValues["Description"])
			{
			wo.Description = e.NewValues["Description"].ToString();
			Session["master_wo_gv"] = null;
			}
		if (e.NewValues["Expected_EndDate"] != e.OldValues["Expected_EndDate"])
			{
			wo.woprog_Expected_EndDate = Convert.ToDateTime(e.NewValues["Expected_EndDate"]);
			Session["master_wo_gv"] = null;
			}
		if (e.NewValues["Expected_Sales"] != e.OldValues["Expected_Sales"])
			{
			wo.woprog_expected_sales_value = Convert.ToDouble(e.NewValues["Expected_Sales"]);
			Session["master_wo_gv"] = null;
			}
		if (Session["master_wo_gv"] == null)
			{
			wo.SaveWorkOrder();
			}

		if (e.NewValues["Notes"] != null)
			{
			var notes = new NeWoProgNotes(Convert.ToInt32(e.Keys["WOID"]), "W");
			notes.woprog_project_notes_woprogid = Convert.ToInt32(e.Keys["WOID"]);
			notes.woprog_project_notes_notes =e.NewValues["Notes"].ToString();
			notes.woprog_project_notes_memberid = Convert.ToInt32(myMember.id);
			notes.woprog_project_notes_Type = "W";
			notes.SaveWOProgProjectNote();
			Session["master_wo_gv"] = null;
			}

		if (Session["master_wo_gv"] == null)
			{
			populate_grid();
			}
		e.Cancel = true;
		gv_workorders.CancelEdit();


		}
	protected void gv_workorders_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
		{
		if (e.ButtonID == "invoice")
			{
			if (gv_workorders.GetRowValues(e.VisibleIndex, "WOProg_InvoiceNo") == null)
				{
				e.Visible = DefaultBoolean.False;
				}
			else if (gv_workorders.GetRowValues(e.VisibleIndex, "WOProg_InvoiceNo").ToString() == "")
				{
				e.Visible = DefaultBoolean.False;
				}


			}
		}
	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		try
			{
			var sql = "";
			var p = e.Parameter.Split('|');
			if (p[2] == "n")
				{

				if (Toolbox.doSQL_affectedrows(@"Update woprog_project_notes  set woprog_project_notes_notes=@v0  where woprog_project_notes_woprogid=@v1 and woprog_project_notes_Type = 'W' limit 1", 
					new object[] { p[1],p[0] }) == 0)
					{
					Toolbox.doSQL_affectedrows(@"Insert into woprog_project_notes (woprog_project_notes_notes,woprog_project_notes_woprogid,woprog_project_notes_Type,
woprog_project_notes_datetime,woprog_project_notes_memberid)  values (@v0,@v1,'W',curdate(),@v2)",
new object[] { p[1],p[0],myMember.id } );
					}
				Session["cb_happened"] = "true";
				}
			}
		catch
			{
			throw new Exception("Couldn't Update Task");
			}
		}
	protected void memnote_Init(object sender, EventArgs e)
		{
		var due = sender as ASPxMemo;
		var container = due.NamingContainer as GridViewDataItemTemplateContainer;
		due.Attributes.Add("onclick", "note_show(this)");
		}
	protected void gv_workorders_DataBinding(object sender, EventArgs e)
		{
		if (Session["master_wo_gv"] != null)
			{
			gv_workorders.DataSource = Session["master_wo_gv"];
			}

		}
	protected void gv_workorders_PageIndexChanged(object sender, EventArgs e)
		{
		if (Session["cb_happened"] != null)
			{
			Session["master_wo_gv"] = null;
			populate_grid();
			Session["cb_happened"] = null;
			}
		}
	protected void gv_workorders_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
		{
		var t = (ASPxSummaryItem)e.Item;
		
    // Initialization.
	

		if (t.FieldName == "GrossMargin")
			{
			var wo_total = (sender as ASPxGridView).TotalSummary["wo_total"];
			var gross_profit = (sender as ASPxGridView).TotalSummary["gross_profit"];
			var total = Convert.ToDecimal(((ASPxGridView)sender).GetTotalSummaryValue(wo_total));
			var profit = Convert.ToDecimal(((ASPxGridView)sender).GetTotalSummaryValue(gross_profit));
			if (total != 0)
				{
				e.TotalValue = profit / total;
				}
			else
				{
				e.TotalValue = 0;
				}
			}
	
		}
	protected void memnote_PreRender(object sender, EventArgs e)
		{
		var dtecomp = sender as ASPxMemo;
		var container = dtecomp.NamingContainer as GridViewDataItemTemplateContainer;

		if (dtecomp.Text.Length < 1)
			{
			dtecomp.Height = Unit.Pixel(20);
			}
		else
			{
			dtecomp.BackColor = System.Drawing.Color.LightYellow;
			dtecomp.Height = Unit.Pixel(40);
			}
		}
 
    protected void gv_workorders_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
		if (e.VisibleIndex > -1)
			{
			var sc = gv_workorders.GetRowValues(e.VisibleIndex, "sc").ToString();
			var hold = gv_workorders.GetRowValues(e.VisibleIndex, "hold").ToString();
			if (sc == "1")
				{
				e.Row.BackColor = System.Drawing.Color.LightYellow;
				}
			if (hold == "1")
				{
				e.Row.BackColor = System.Drawing.Color.LightPink;
				}
			}
		}
	[WebMethod]
	public static void update_lbr(double hrs, int woprog_id)
		{
		if(woprog_id > 10000 && hrs >= 0)
			{
			Toolbox.doSQL_void(@"UPDATE woprog SET woprog_exp_labor = @v0  WHERE woprog_id = @v1  LIMIT 1", new object[] {  hrs, woprog_id } );
			HttpContext.Current.Session["master_wo_gv"] = null;
			}
		else if(hrs < 0)
			{
			throw new Exception("You cannot work negative hours...");
			}
		}
	protected void gv_workorders_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
		{
		if (e.VisibleIndex >= 0)
			{
			if(e.Column is GridViewDataTextColumn)
				{
				var gvr = (GridViewDataTextColumn)e.Column;

				if (gvr.FieldName == "invoice_lag")
					{
					e.Cell.Text = (Convert.ToDouble(Session["itotal"]) / Convert.ToInt16(Session["irows"])).ToString();
					}
				}
			}
		}
	[WebMethod]
	public static void update_note(string note, int woprog_id)
		{
		var current_user		= new NeMember(HttpContext.Current.Session["session"].ToString());
		if(woprog_id > 10000)
			{
			// Does it exist?
			var wopn		= new NeWoProgNotes(woprog_id, "W");
			wopn.woprogid			= woprog_id;
			var temp_note		= Toolbox.MySQLNow_long()+ ": "+current_user.FullName+" - "+note+"\n"+wopn.notes+"\n";
			wopn.notes				= temp_note;
			wopn.memberid			= current_user.id32;
			wopn.Type				= "W";
			wopn.SaveWOProgProjectNote();
			HttpContext.Current.Session["master_wo_gv"] = null;
			}
		}
	protected void chk_includeprogress_CheckedChanged(object sender, EventArgs e)
		{
			Session["master_wo_gv"] = null;
			populate_grid();
		}

	protected void gv_workorders_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
	{
		if (e.Column.FieldName == "QuotedGrossMargin")
		{
			(e.Editor as ASPxTextBox).DisplayFormatString = "P3";
		}
	}
}
