using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.Script.Serialization;
using DevExpress.Web;
using System.IO;
using System.Linq;

using System.Data.Common;
using DevExpress.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_member_inventory_incorrect_location_qtys : System.Web.UI.Page
{

	public NeMember myMember;
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	private const int _page_id = 207; // from Page table in DB
	private const string _page_name = "Inventory_Incorrect_Location_qtys";
	NeBusinessUnit this_company;
	Toolbox _tools;
	DataTable shopping_cart_dt;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	bool can_edit = false;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		var _q = Request.QueryString;
		layout.__gv_id = "gv";
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.used_gv = gv;
		h.Set("gridview_id", "gv");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
		var bo_obj = new branch_options(myMember.business_unit_id);
		can_edit = ((myMember.AuthenticatedForPage("49") && bo_obj.stk_adj) || (myMember.id == 8));

        var working_business_unit_id = new NeBusinessUnit( int.Parse( Session["working_business_unit_id"] == null ? myMember.business_unit_id.ToString() : Session["working_business_unit_id"].ToString()));
        var warehouse_business_unit_id = new NeBusinessUnit(working_business_unit_id.warehouse_bu_id);

        hdn_company.Value = warehouse_business_unit_id.id.ToString();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		((IntraDefault)this.Master).page_name = NePage.get_page_name(_page_id);
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		if (!IsPostBack)
		{
			var gl = new NeGridLayouts(myMember.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				var char_count = myMember.FullName.Length + 22;

				gv.LoadClientLayout(gl.GridLayout_Layout);
				gl.member_id = myMember.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;

		}

	}
	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{

		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
	{
		var id = Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "id"));
		var tb = (ASPxTextBox)gv.FindRowCellTemplateControl(e.VisibleIndex,(GridViewDataColumn)gv.Columns["incorrect_qty"],"tb_qty");
		new incorrect_levels().approve(id, myMember.id,Convert.ToDouble(tb.Text));

		var is_valid = true;

		
        var working_business_unit_id = new NeBusinessUnit(int.Parse(Session["working_business_unit_id"] == null ? myMember.business_unit_id.ToString() : Session["working_business_unit_id"].ToString()));
        var warehouse_business_unit_id = new NeBusinessUnit(working_business_unit_id.warehouse_bu_id);
        //var business_unit_id = Convert.ToInt32(Session["working_business_unit_id"]);
        var business_unit_id = warehouse_business_unit_id.id;

        var iil = new incorrect_levels();
		if (id != null)
		{
			iil = new incorrect_levels(Convert.ToInt32(id));
		}
		

		var master_id = iil.master_id;
		var location_master_id = iil.location_id;
		var i = new inventory();
		i.Load(iil.master_id, business_unit_id);
		var ilm = new location_master();
		var il = new location();

		if (location_master_id > 0)
		{
			ilm = new location_master(location_master_id);
		}
		if (master_id > 0)
		{
			il = new location(location_master_id, business_unit_id, master_id);
		}
		double qty = 0;
		var can_convert = double.TryParse(tb.Text, out qty);
		if (!can_convert)
		{
			gv.JSProperties["cp_alert"]="Invalid Quantity Supplied.";
			return;
		}
		else if (qty < 0)
		{
			gv.JSProperties["cp_alert"] = "Negative Quantity Supplied.  This is not allowed";
			return;
		}
		if (il.id > 0 && ilm.id > 0 && master_id > 0 && qty >= 0)
		{
			var ib = new branch(il.master_id, il.business_unit_id);
			var prev_qty = il.qty;
			var this_cost = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  il.master_id, il.business_unit_id } );
			il.log_is_manual = true;
			il.member_id = myMember.id;
			il.alert_worthy = true;
			var diff = il.qty - qty;
			il.qty = qty;
			il.section_id = 1;
			var this_type = prev_qty > qty ? 3 : 2;
			var pre_ib = new branch(il.master_id, il.business_unit_id);
			il.save();
			il.update_branch(this_type, ib.dollar_balance, diff, this_cost, ib);
			var post_ib = new branch(il.master_id, il.business_unit_id);

			il = new location(location_master_id, business_unit_id, master_id);

			iil.approve(iil.id, myMember.id, qty);

			if (myMember.id != il.member_id)
			{
				var email = new NeEMail();
				email.To = new NeMember(Convert.ToInt32(iil.member_id)).NEEmail;
				email.Subject = "Location Qty Update";
				email.Body = "<div style='font-family: arial;'>Part: " + iil.master_id + " " + i.description_full + " in location: " + il.this_master.name + " had its qty updated from " + prev_qty + " to " + qty + ".  You are being notified because your name is on record for reporting the incorrect qty for this part at this location.</div>";
				email.isHTML = true;
				email.Send();
			}

		}
		

		gv.DataBind();

	}
	protected void gv_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			
				//e.Visible = DevExpress.Utils.DefaultBoolean.False;
				if ((gv.GetRowValues(e.VisibleIndex, "ClearedBy_name") == null)&&can_edit)
				{
					e.Visible = DefaultBoolean.True;
				}
				else
				{
					e.Visible =  DefaultBoolean.False;
				}
	
		}
	}
}
