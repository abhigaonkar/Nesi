using System;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_reports_invoicing_delays_index : Page
	{
	Toolbox _tools;
	NeMember current_user;
	JavaScriptSerializer jSON		= new JavaScriptSerializer();
	static int _page_id			= 192;
	static string _page_name		= "Invoicing_times";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools			= new Toolbox();
		current_user	= Toolbox.do_handle_authentication(_page_id);
		layout.__page_name					= _page_name;
		if(Session["working_business_unit_id"] != null)
			{
			Session.Remove("working_business_unit_id");
			}
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		layout.used_gv						= gv_invoicing_delays;
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= current_user.id.ToString();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsCallback && !IsPostBack)
			{
				var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
				divMenu.InnerHtml = menu.MenuHTML;
				var gl = new NeGridLayouts(current_user.id, _page_name);
				h.Set("gridview_id", "gv_invoicing_delays");
				if (gl.GridLayoutID == 0)
				{
					gv_invoicing_delays.FilterExpression = "";
					gl.GridLayout_Layout = gv_invoicing_delays.SaveClientLayout();
					gl.member_id = current_user.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					gv_invoicing_delays.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				dde_filter.Text = gl.GridLayout_Name;
					 
		//		DataTable dt = _tools.getSQL_datatable(@"Select * from customer_rates"  , null);

		//		gv_customer_rates.DataSource = dt;

		//		Session["gv_customer_rates"] = dt;
			}
			else
			{
		//		gv_customer_rates.DataSource = Session["gv_customer_rates"];
			}

        sqlcompany.SelectCommand = "Select id,ddl_name name from business_unit where id in(" + new Current_User().visible_business_units + ")";


            fill_grid();
		}

	protected void gv_customer_rates_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{

		}

	protected void fill_grid()
	{


		var dt	= Toolbox.doSQL_dt(String.Format(@"
Select a.woprog_id id,
business_unit.ddl_name business_unit, 
member_fullname pm, 
a.woprog_customername customer,
(Select timestampdiff(HOUR,a.WOProg_OpenDateTime,ifnull((Select membertime.Date from membertime where membertime.MemberTime_WOProg_id = a.woprog_id and membertime.Date<=a.WOProg_OpenDateTime order by membertime.Date desc limit 1
),a.WOProg_OpenDateTime)))/-24 to_scan,

ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting PM Approval' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting PM Approval' limit 1
),0)/-24 pm_app,

ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting BM Approval' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting BM Approval' limit 1
),0)/-24 bm_app,

ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting Parent BM Approval' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting Parent BM Approval' limit 1
),0)/-24 bm_parent_app,

ifnull((
Select sum(timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Rework' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime)) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Rework' group by w.WOProgStatus_WOProg_ID
),0)/-24 rework,

ifnull((
Select sum(timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Questions For PM' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime)) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Questions For PM' group by w.WOProgStatus_WOProg_ID
),0)/-24 questions,

ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Initial Prep' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Initial Prep' limit 1
),0)/-24 time_prep,

ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting For PO' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting For PO' limit 1
),0)/-24 cust_po,

ifnull((
Select sum(timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting To Be Invoiced' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime)) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting To Be Invoiced' group by w.WOProgStatus_WOProg_ID
),0)/-24 to_be_invoiced,

(Select timestampdiff(HOUR,a.WOProg_CloseDateTime,
(ifnull((Select membertime.Date 
from membertime 
where membertime.MemberTime_WOProg_id = a.woprog_id and membertime.Date<=a.WOProg_OpenDateTime 
order by membertime.Date desc limit 1)
,a.WOProg_OpenDateTime)
)))
/-24 total_time,


ifnull((
Select sum(timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting To Be Invoiced' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime)) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting To Be Invoiced' group by w.WOProgStatus_WOProg_ID
),0)/-24+

ifnull((
Select sum(timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Rework' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime)) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Rework' group by w.WOProgStatus_WOProg_ID
),0)/-24+

ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Initial Prep' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Initial Prep' limit 1
),0)/-24 
total_nesi,

(Select timestampdiff(HOUR,a.WOProg_OpenDateTime,ifnull((Select membertime.Date from membertime where membertime.MemberTime_WOProg_id = a.woprog_id and membertime.Date<=a.WOProg_OpenDateTime order by membertime.Date desc limit 1),a.WOProg_OpenDateTime)))/-24
+
ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting PM Approval' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting PM Approval' limit 1
),0)/-24+
ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting BM Approval' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting BM Approval' limit 1
),0)/-24+
ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting Parent BM Approval' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting Parent BM Approval' limit 1
),0)/-24+

ifnull((
Select sum(timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Questions For PM' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime)) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Questions For PM' group by w.WOProgStatus_WOProg_ID
),0)/-24+

ifnull((
Select timestampdiff(HOUR,(Select WOProgStatus_DateTime from woprogstatus where WOProgStatus_Status!='Waiting For PO' and WOProgStatus_DateTime>w.WOProgStatus_DateTime and  WOProgStatus_WOProg_ID =  a.WOProg_ID order by WOProgStatus_DateTime limit 1),WOProgStatus_DateTime) from woprogstatus w where w.WOProgStatus_WOProg_ID = a.WOProg_ID and w.WOProgStatus_Status = 'Waiting For PO' limit 1
),0)/-24
 total_branch,
a.WOProg_InvoicedNetTotal rev,

a.woprog_bvwo bvwo,
a.woprog_invoicedate invoice_date,
a.woprog_cutdatetime wo_cut,
a.WOProg_Status wo_status,
woprog_description,
date(ifnull((Select membertime.Date from membertime where membertime.MemberTime_WOProg_id = a.woprog_id and membertime.Date<=a.WOProg_OpenDateTime order by membertime.Date desc limit 1),a.WOProg_OpenDateTime)) last_hour

from  
woprog a inner join business_unit on a.business_unit_id = business_unit.id inner join member on a.woprog_pm_memberid = member.Member_ID
where business_unit.id IN ({0}) AND ( a.woprog_invoicedate >= '2016-01-01' or (a.woprog_invoicedate is null and a.WOProg_Status <> 'Deleted') )", new Current_User().visible_business_units),null);

		gv_invoicing_delays.DataSource = dt;
		gv_invoicing_delays.DataBind();
	}

	protected void gv_invoicing_delays_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
	}
	protected void gv_invoicing_delays_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv				= (ASPxGridView) sender;
		if(e.Parameters != "")
			{
			gv.LoadClientLayout(e.Parameters);
			}
		else
			{
			gv.FilterExpression		= "";
			for(var i = 0; i < gv.Columns.Count; i++)
				{
				if (gv.Columns[i] is GridViewDataColumn)
					{
					var col = (GridViewDataColumn) gv.Columns[i];
					if (col.GroupIndex > -1)
						{
						gv.UnGroup(col);
						}
					col.Visible = true;
					}
				}
			}
	}
}
