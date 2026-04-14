using System;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class sections_dashboards_quote_grid : System.Web.UI.Page
	{
		NeMember current_user;
		private int page_id = 32;
		Toolbox _tools;
	
	private NeBusinessUnit c;
	private bool _company_wide = false;
	private bool _region_wide = false;
	private bool _department_wide = false;
	private bool _branch_wide = false;
	private bool _base_level = false;
	protected NameValueCollection _q;

	protected void Page_Init()
	{
		_q = Request.QueryString;
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		_company_wide = current_user.AuthenticatedForPrivilege(146);
		_region_wide = current_user.AuthenticatedForPrivilege(147);
		_department_wide = current_user.AuthenticatedForPrivilege(148);
		_branch_wide = current_user.AuthenticatedForPrivilege(149);
		_base_level = current_user.AuthenticatedForPrivilege(150);
	    _tools.dont_cache_page();
		if (Cache["ds_depend"] == null)
		{
			Cache["ds_depend"] = DateTime.Now;
		}

	}
		protected void Page_Load(object sender, EventArgs e)
		{
			var _q = Request.QueryString;
			c = new NeBusinessUnit(_q["cid"]);
			if ((!IsPostBack)&&(!IsCallback))
			{
				Session["gv_new_dashboard_quotes"] = null;
			}
			fill_grid();
		}

		protected void fill_grid()
		{
			if (Session["gv_new_dashboard_quotes"] == null)
			{
				Session["gv_new_dashboard_quotes"] = _tools.getSQL_datatable(@"SELECT customer.Customer_id custid, customer.Customer_Name AS `name`, quote_v2.quote_id q_id, urldecode(quote_v2.job_description) `desc`, quote_v2.quoted_price price, quote_status.`status`, quote_v2.revision `rev`, quote_v2.quote_n, quote_v2.customer_id, quote_master.pct_chance/100 chance, quote_master.pct_chance_note chance_note, quote_master.completion_date comp_date, quote_chance.quote_chance_name why_chance, quote_chance.quote_chance_id, round(quote_master.pct_chance*quote_v2.quoted_price/100,0) pipeline, quote_master.date_due FROM customer INNER JOIN quote_v2 ON quote_v2.customer_id = customer.Customer_ID INNER JOIN quote_status ON quote_v2.status_id = quote_status.id INNER JOIN quote_master ON quote_v2.quote_id = quote_master.quote_id AND quote_v2.revision = quote_master.revision INNER JOIN quote_chance ON quote_master.pct_chance_reason = quote_chance.quote_chance_id  WHERE quote_v2.quoted_by =@v0 AND quote_v2.business_unit_id =@v1  and quote_status.id Not IN (6,8,7,9) order by abs(datediff((curdate()+interval 57 day),quote_master.date_due))", new object[] { _q["mid"],_q["cid"] });

			}
			else
			{
				
			}
			ASPxGridView1.DataSource = Session["gv_new_dashboard_quotes"];
			ASPxGridView1.DataBind();
		}

		
		protected void ASPxGridView1_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
			var gv = (ASPxGridView)sender;
			e.Properties["cpExp"] = gv.SaveClientLayout();
		}
		protected void ASPxGridView1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
			if (e.Parameters.Length > 0)
			{
				var p = e.Parameters.Split('|');
				if (p.Length > 1)
				{
					if (p[2] == "p")
					{
						_tools.getSQL_void(@"update quote_master set quote_master.pct_chance = (" + p[1] + "*100) where quote_master.quote_id = " + p[0] + " limit 1");
						Session["gv_new_dashboard_quotes"] = null;
						fill_grid();
					}
					else if (p[2] == "d")
					{
						_tools.getSQL_void("update quote_master set quote_master.completion_date = '" + p[1] + "' where quote_master.quote_id = " + p[0] + " limit 1");
						Session["gv_new_dashboard_quotes"] = null;
						fill_grid();
					}
					else if (p[2] == "c")
					{
						_tools.getSQL_void("update quote_master set quote_master.pct_chance_reason = '" + p[1] + "' where quote_master.quote_id = " + p[0] + " limit 1");
						Session["gv_new_dashboard_quotes"] = null;
						fill_grid();
					}
					else if (p[2] == "u")
					{
						_tools.getSQL_void("update quote_master set quote_master.date_due = '" + p[1] + "' where quote_master.quote_id = " + p[0] + " limit 1");
						Session["gv_new_dashboard_quotes"] = null;
						fill_grid();
					}
				}
			}
		}

		protected void ASPxGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
			
		}
		protected void ASPxGridView1_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
		{
			if (e.VisibleIndex >= 0 && e.GetValue("comp_date") != null && e.GetValue("comp_date") != DBNull.Value)
			{

				if (Convert.ToDateTime(e.GetValue("comp_date")) <= (System.DateTime.Today.AddDays(0)))
				{
					e.Row.BackColor = System.Drawing.Color.Red;
				}
				else if (Convert.ToDateTime(e.GetValue("comp_date")) <= (System.DateTime.Today.AddDays(7)))
				{
					e.Row.BackColor = System.Drawing.Color.Salmon;
				}
			}
			
		}
		protected void ASPxSpinEdit1_Init(object sender, EventArgs e)
		{
			var ASPxSpinEdit1 = sender as ASPxSpinEdit;
			var container = ASPxSpinEdit1.NamingContainer as GridViewDataItemTemplateContainer;
			ASPxSpinEdit1.ClientSideEvents.ValueChanged = string.Format("function (s, e) {{ ASPxGridView1.PerformCallback('{0}|' + s.GetValue()+ '|p'); }}", container.KeyValue);

		}

		protected void ASPxDateEdit1_Init(object sender, EventArgs e)
		{
			var dateedit = sender as ASPxDateEdit;
			var container = dateedit.NamingContainer as GridViewDataItemTemplateContainer;
			dateedit.ClientSideEvents.DateChanged = string.Format(@"function (s, e) {{ 
var jsDate = s.GetDate();

var year = jsDate.getFullYear(); 
var month = jsDate.getMonth(); 
var day = jsDate.getDate();   

var myDate = year;
myDate += '/';
myDate += month+1;
myDate += '/';
myDate += day;

ASPxGridView1.PerformCallback('{0}|' + myDate + '|d'); }}", container.KeyValue);
		}
		protected void ASPxDateEdit2_Init(object sender, EventArgs e)
		{
			var dateedit = sender as ASPxDateEdit;
			var container = dateedit.NamingContainer as GridViewDataItemTemplateContainer;
			dateedit.ClientSideEvents.DateChanged = string.Format(@"function (s, e) {{ 
var jsDate = s.GetDate();

var year = jsDate.getFullYear(); 
var month = jsDate.getMonth(); 
var day = jsDate.getDate();   

var myDate = year;
myDate += '/';
myDate += month+1;
myDate += '/';
myDate += day;

ASPxGridView1.PerformCallback('{0}|' + myDate + '|u'); }}", container.KeyValue);
		}
		protected void ASPxComboBox1_Init(object sender, EventArgs e)
		{
			var cb = sender as ASPxComboBox;
			var container = cb.NamingContainer as GridViewDataItemTemplateContainer;
			cb.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ ASPxGridView1.PerformCallback('{0}|' + s.GetValue()+ '|c'); }}", container.KeyValue);

		}
}
