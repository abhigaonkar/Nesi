using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using nesi.core;

public partial class Dashboard : System.Web.UI.Page
{

	NeMember current_user;
	private const int _page_id = 32; // from Page table in DB
	static string _page_name = "Dashboard";
	Toolbox _tools;
	NameValueCollection _q;
	bool company_switcher = false;
    bool branch_wide_dashboard = false;
    ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;

	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
        hdn_mid.Value = current_user.id.ToString();
		layout.used_gv = gv_dashoverview;
		layout.__page_name = _page_name;
		_tools.add_css("/css/dashboard.css");
		_q = Request.QueryString;
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		((IntraDefault)this.Master).page_name = NePage.get_page_name(_page_id);
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		if (Session["dashboard_business_unit_id"] == null)
		{
			Session["dashboard_business_unit_id"] = current_user.business_unit_id;
		}
		if (Session["dashboard_onlyonebranch"] == null)
		{
			Session["dashboard_onlyonebranch"] = false;
		}
		#region IFRAME control
		ifr_customers.Attributes["onload"] = "handle_if_load(this);";
		ifr_drilldown.Attributes["onload"] = "handle_if_load(this);";
		ifr_grids.Attributes["onload"] = "handle_if_load(this);";
		ifr_gross.Attributes["onload"] = "handle_if_load(this);";
		ifr_hourutilization.Attributes["onload"] = "handle_if_load(this);";
		ifr_inventory.Attributes["onload"] = "handle_if_load(this);";
		ifr_margin.Attributes["onload"] = "handle_if_load(this);";
		ifr_net.Attributes["onload"] = "handle_if_load(this);";
		ifr_rev.Attributes["onload"] = "handle_if_load(this);";
		ifr_salespipeline.Attributes["onload"] = "handle_if_load(this);";
		ifr_wo.Attributes["onload"] = "handle_if_load(this);";
		ifr_worstcustomers.Attributes["onload"] = "handle_if_load(this);";
        ifr_headcount.Attributes["onload"] = "handle_if_load(this);";

		#endregion IFRAME control
		business_unit_id.Value = Session["dashboard_business_unit_id"].ToString();
		company_switcher = current_user.AuthenticatedForPrivilege(76);
	    branch_wide_dashboard = current_user.AuthenticatedForPrivilege(149);
        manager_title.InnerText = "Dashboard";
		#region Company Switcher
		 if (company_switcher || branch_wide_dashboard)
		{
			company_switch_div.Visible = true;
			populate_companies();
			Session["show_only_branch"] = "x";
            show_all_link.Visible = true;

        }
		else if (!company_switcher)
		{
			company_switch_div.Visible = false;
			Session["show_only_branch"] = current_user.business_unit_id.ToString();
			Response.Redirect("/sections/dashboards/frame.aspx?page_id=152");
            show_all_link.Visible = false;

        }

        #endregion Company Switcher

        h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	}
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(_q["a"]))
		{
			var action = _q["a"];
			switch (action)
			{
				#region set_threshold
				case "set_threshold":
					var type = _q["type"];
					var subtype = _q["subtype"];
					var bad = Convert.ToDouble(_q["bad"]);
					var good = Convert.ToDouble(_q["good"]);
					// Check Existance
					var n = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM dashboard_thresholds WHERE member_id = @v0  AND business_unit_id = @v1  AND type = @v2  AND subtype = @v3 ", new object[] { current_user.id, current_user.business_unit_id, _q["type"], _q["subtype"] });
					if (n == 0)
					{
						// Doesn't Exist
						Toolbox.doSQL_void(@" INSERT INTO dashboard_thresholds ( member_id, business_unit_id, type, subtype, good_threshold, bad_threshold ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , @v5  )", new object[] { current_user.id, current_user.business_unit_id, type, subtype, good, bad });
					}
					else
					{
						// Exists
						Toolbox.doSQL_void(@" UPDATE dashboard_thresholds SET good_threshold = @v0 , bad_threshold = @v1  WHERE business_unit_id = @v2  AND member_id = @v3  AND type = @v4  AND subtype = @v5  LIMIT 1", new object[] { good, bad, current_user.business_unit_id, current_user.id, type, subtype });
					}
					Toolbox.QuickReponse(Response, "SUCCESS");
					break;
				#endregion set_threshold
				#region switch_company
				case "switch_company":
					if (!string.IsNullOrEmpty(_q["business_unit_id"]))
					{
						Session["dashboard_business_unit_id"] = Convert.ToInt32(_q["business_unit_id"]);
						Toolbox.QuickReponse(Response, "SUCCESS");
					}
					else
					{
						Toolbox.FriendlyException(Response, "You have tried accessing this function incorrectly", "/default.aspx");
					}
					break;
				#endregion switch_company
			
				#region load_branch
				case "load_branch":
					if (!string.IsNullOrEmpty(_q["business_unit_id"]))
					{
						Session["dashboard_business_unit_id"] = Convert.ToInt32(_q["business_unit_id"]);
						Session["dashboard_onlyonebranch"] = true;
					}
					Response.Redirect("./dashboard.aspx?page_id=32", true);
					break;
				#endregion load_all
				#region load_all
				case "load_all":
					Session["dashboard_onlyonebranch"] = false;
					Response.Redirect("./dashboard.aspx?page_id=32", true);
					break;
				#endregion load_all
				case "xml_divisions":

					break;
			}
		}
		company_switch.Attributes["onchange"] = "switch_company(this);";
       
		panel_overview.Visible = string.IsNullOrEmpty(_q["load_branch"]) && (bool)Session["dashboard_onlyonebranch"] == false;
		
		panel_bm.Visible = !panel_overview.Visible;

        cbo_dtes.DataSource = _tools.getSQL_datatable("Select distinct date(dt) _id, date(dt) _date from dashboard_snapshot_hist order by id desc limit 365", null);
        cbo_dtes.DataBind();
        if (!IsPostBack)
        {
            cbo_dtes.Value = System.DateTime.Today.ToString("yyyy-MM-dd");
        }

		if (panel_overview.Visible)
		{
			//populate_overview();
			h.Set("gridview_id", "gv_dashoverview");
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gl.GridLayout_Layout = gv_dashoverview.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				if (!IsPostBack)
				{
					gv_dashoverview.LoadClientLayout(gl.GridLayout_Layout);
				}
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
		}
	}
	protected void populate_overview()
	{
		var target = ov_body;
		var html = new StringBuilder();
		html.Append(@"
<table class='data' cellpadding='0' cellspacing='0'>
	<thead>
		<tr>
			<th>Branch</th>
			<th>Revenue<br/>MTD</th>
			<th>Revenue<br/>YTD</th>
			<th>Labor<br/>MTD</th>
			<th>Labor<br/>YTD</th>
			<th>Material<br/>MTD</th>
			<th>Material<br/>YTD</th>
			<th>Gross<br/>MTD</th>
			<th>Gross<br/>YTD</th>
			<th>Inve. Bal</th>
			<th>AR &gt; 90</th>
			<th>HU<br/>MTD</th>
			<th>HU<br/>YTD</th>
			<th>Customers<br/>MTD</th>
			<th>Customers<br/>YTD</th>
		</tr>
	</thead>
	<tbody>
		
");
		var dt = Toolbox.doSQL_dt(@"SELECT * FROM dashboard_snapshot_daily  where business_unit_id = 0", null);
		lbl_last_updated.Text = "Last Updated: " + Convert.ToDateTime(Toolbox.doSQL_string(@"SELECT MAX(dt) FROM dashboard_snapshot_daily")).ToString("G");
		double tot_rev_mtd = 0;
		double tot_rev_ytd = 0;
		double tot_lab_mtd = 0;
		double tot_lab_ytd = 0;
		double tot_mat_mtd = 0;
		double tot_mat_ytd = 0;
		double tot_gro_mtd = 0;
		double tot_gro_ytd = 0;
		double tot_inv = 0;
		double tot_ar = 0;
		double tot_cust_mtd = 0;
		double tot_cust_ytd = 0;

		foreach (DataRow r in dt.Rows)
		{
			var c = new NeBusinessUnit(r["business_unit_id"]);
			var rev_mtd = Convert.ToDouble(r["rev_mtd"]);
			var rev_ytd = Convert.ToDouble(r["rev_ytd"]);
			var lab_mtd_dol = Convert.ToDouble(r["lab_mtd_dol"]);
			var lab_mtd_pct = Convert.ToDouble(r["lab_mtd_pct"]);
			var lab_ytd_dol = Convert.ToDouble(r["lab_ytd_dol"]);
			var lab_ytd_pct = Convert.ToDouble(r["lab_ytd_pct"]);
			var mat_mtd_dol = Convert.ToDouble(r["mat_mtd_dol"]);
			var mat_mtd_pct = Convert.ToDouble(r["mat_mtd_pct"]);
			var mat_ytd_dol = Convert.ToDouble(r["mat_ytd_dol"]);
			var mat_ytd_pct = Convert.ToDouble(r["mat_ytd_pct"]);
			var gross_mtd_dol = Convert.ToDouble(r["gross_mtd_dol"]);
			var gross_mtd_pct = Convert.ToDouble(r["gross_mtd_pct"]);
			var gross_ytd_dol = Convert.ToDouble(r["gross_ytd_dol"]);
			var gross_ytd_pct = Convert.ToDouble(r["gross_ytd_pct"]);
			var inv_balance = Convert.ToDouble(r["inv_balance"]);
			var ar_90 = Convert.ToDouble(r["ar_90"]);
			var hu_mtd = Convert.ToDouble(r["hu_mtd"]);
			var hu_ytd = Convert.ToDouble(r["hu_ytd"]);
			var cust_mtd = Convert.ToInt32(r["cust_mtd"]);
			var cust_ytd = Convert.ToInt32(r["cust_ytd"]);
			tot_rev_mtd += rev_mtd;
			tot_rev_ytd += rev_ytd;
			tot_lab_mtd += lab_mtd_dol;
			tot_lab_ytd += lab_ytd_dol;
			tot_mat_mtd += mat_mtd_dol;
			tot_mat_ytd += mat_ytd_dol;
			tot_gro_mtd += gross_mtd_dol;
			tot_gro_ytd += gross_ytd_dol;
			tot_inv += inv_balance;
			tot_ar += ar_90;
			tot_cust_mtd += cust_mtd;
			tot_cust_ytd += cust_ytd;
			html.AppendFormat(@"
		<tr>
			<td data-column='Business Unit' class='branch'><a href='./dashboard.aspx?a=load_branch&business_unit_id={0}'>{1}</a></td>
			<td data-column='rev_mtd'>{2:C0}</td>
			<td data-column='rev_ytd'>{3:C0}</td>
			<td data-column='labor_mtd'><div class='l'>{10:C0}</div><div class='r'>{11}</div></td>
			<td data-column='labor_mtd'><div class='l'>{12:C0}</div><div class='r'>{13}</div></td>
			<td data-column='material_mtd'><div class='l'>{14:C0}</div><div class='r'>{15}</div></td>
			<td data-column='material_ytd'><div class='l'>{16:C0}</div><div class='r'>{17}</div></td>
			<td data-column='gross_mtd'><div class='l'>{6:C0}</div><div class='r'>{7}</div></td>
			<td data-column='gross_ytd'><div class='l'>{8:C0}</div><div class='r'>{9}</div></td>
			<td data-column='inve_bal'>{20:C0}</td>
			<td data-column='ar_90'>{21:C0}</td>
			<td data-column='hu_mtd'>{18}</td>
			<td data-column='hu_ytd'>{19}</td>
			<td data-column='cust_mtd'>{4}</td>
			<td data-column='cust_ytd'>{5}</td>
		</tr>
", c.id,
			c.name,
			dollarize(rev_mtd),
			dollarize(rev_ytd),
			cust_mtd,
			cust_ytd,
			dollarize(gross_mtd_dol),
			percentize(gross_mtd_pct),
			dollarize(gross_ytd_dol),
			percentize(gross_ytd_pct),
			dollarize(lab_mtd_dol),
			percentize(lab_mtd_pct),
			dollarize(lab_ytd_dol),
			percentize(lab_ytd_pct),
			dollarize(mat_mtd_dol),
			percentize(mat_mtd_pct),
			dollarize(mat_ytd_dol),
			percentize(mat_ytd_pct),
			percentize(hu_mtd),
			percentize(hu_ytd),
			dollarize(inv_balance),
			dollarize(ar_90)
			);
		}
		html.AppendFormat(@"
	</tbody>
	<tfoot>
		<tr>
			<td>&nbsp;</td>
			<td>{0:C0}</td>
			<td>{1:C0}</td>
			<td>{2:C0}</td>
			<td>{3:C0}</td>
			<td>{4:C0}</td>
			<td>{5:C0}</td>
			<td>{6:C0}</td>
			<td>{7:C0}</td>
			<td>{8:C0}</td>
			<td>{9:C0}</td>
			<td>&nbsp;</td>
			<td>&nbsp;</td>
			<td>{10:N0}</td>
			<td>{11:N0}</td>
		</tr>
	</tfoot>", tot_rev_mtd, tot_rev_ytd, tot_lab_mtd, tot_lab_ytd, tot_mat_mtd, tot_mat_ytd, tot_gro_mtd, tot_gro_ytd, tot_inv, tot_ar, tot_cust_mtd, tot_cust_ytd);
		html.Append(@"
<table>
<script>
	$(document).ready(function()
		{
		$('.data').tablesorter();
		});
</script>

");
		target.InnerHtml = html.ToString();
	}
	private string dollarize(double figure)
	{
		return figure > 0 ? figure.ToString("C0") : figure < 0 ? figure.ToString("C0") : "<font class='zero'>$0.00</font>";
	}
	private string percentize(double figure)
	{
		if (figure > 1 || figure < -1)
		{
			figure = figure / 100;
		}
		return figure > 0 ? figure.ToString("P0") : figure < 0 ? "-" + Math.Abs(figure).ToString("P0") : "<font class='zero'>0%</font>";
	}
	protected void gv_dashoverview_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_dashoverview_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_dashoverview_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		var t = (ASPxSummaryItem)e.Item;
		if (t.FieldName == "lab_mtd_pct")
		{
			e.TotalValue = 0;
			/*
			ASPxSummaryItem wo_total = (sender as ASPxGridView).TotalSummary["wo_total"];
			ASPxSummaryItem gross_profit = (sender as ASPxGridView).TotalSummary["gross_profit"];
			Decimal total = Convert.ToDecimal(((ASPxGridView)sender).GetTotalSummaryValue(wo_total));
			Decimal profit = Convert.ToDecimal(((ASPxGridView)sender).GetTotalSummaryValue(gross_profit));
			if (total != 0)
				{
				e.TotalValue = profit / total;
				}
			else
				{
				e.TotalValue = 0;
				}
			*/
		}
	}
	protected void gv_dashoverview_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.IsTotalSummary)
		{
			var t_rev_mtd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["rev_mtd"]));
			var t_rev_ytd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["rev_ytd"]));
            var t_rev_bmtd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["rev_bmtd"]));
            var t_rev_bytd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["rev_bytd"]));
            var t_lab_mtd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["lab_mtd_dol"]));
			var t_lab_ytd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["lab_ytd_dol"]));
			var t_mat_mtd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["mat_mtd_dol"]));
			var t_mat_ytd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["mat_ytd_dol"]));
			var t_gross_mtd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["gross_mtd_dol"]));
			var t_gross_ytd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["gross_ytd_dol"]));
            var t_net_mtd =   Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["net_mtd_dol"]));
            var t_net_ytd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["net_ytd_dol"]));
            var t_net_bmtd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["net_bmtd_dol"]));
            var t_net_bytd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["net_bytd_dol"]));
            var t_gross_bmtd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["gm_bmtd"]));
            var t_gross_bytd = Convert.ToDouble(gv.GetTotalSummaryValue(gv.TotalSummary["gm_bytd"]));
            /*
             net_mtd_dol,
	net_mtd_pct,
	net_ytd_dol,
	net_ytd_pct,
	net_bmtd_dol,
	net_bmtd_pct,
	net_bytd_dol,
	net_bytd_pct*/

            double pct_out = 0;
			if (e.Item.FieldName == "lab_mtd_pct")
			{
				double.TryParse((t_lab_mtd / t_rev_mtd).ToString(), out pct_out);
				e.Text = pct_out.ToString("P1");
			}
			if (e.Item.FieldName == "lab_ytd_pct")
			{
				double.TryParse((t_lab_ytd / t_rev_ytd).ToString(), out pct_out);
				e.Text = pct_out.ToString("P1");
			}
			if (e.Item.FieldName == "mat_mtd_pct")
			{
				double.TryParse((t_mat_mtd / t_rev_mtd).ToString(), out pct_out);
				e.Text = pct_out.ToString("P1");
			}
			if (e.Item.FieldName == "mat_ytd_pct")
			{
				double.TryParse((t_mat_ytd / t_rev_ytd).ToString(), out pct_out);
				e.Text = pct_out.ToString("P1");
			}
			if (e.Item.FieldName == "gross_mtd_pct")
			{
				double.TryParse((t_gross_mtd / t_rev_mtd).ToString(), out pct_out);
				e.Text = pct_out.ToString("P1");
			}
			if (e.Item.FieldName == "gross_ytd_pct")
			{
				double.TryParse((t_gross_ytd / t_rev_ytd).ToString(), out pct_out);
				e.Text = pct_out.ToString("P1");
			}
            if (e.Item.FieldName == "net_mtd_pct") 
            {
                double.TryParse((t_net_mtd / t_rev_mtd).ToString(), out pct_out);
                e.Text = pct_out.ToString("P1");
            }
            if (e.Item.FieldName == "net_ytd_pct")
            {
                double.TryParse((t_net_ytd / t_rev_ytd).ToString(), out pct_out);
                e.Text = pct_out.ToString("P1");
            }
            if (e.Item.FieldName == "net_bmtd_pct")
            {
                double.TryParse((t_net_bmtd / t_rev_bmtd).ToString(), out pct_out);
                e.Text = pct_out.ToString("P1");
            }
            if (e.Item.FieldName == "net_bytd_pct")
            {
                double.TryParse((t_net_bytd / t_rev_bytd).ToString(), out pct_out);
                e.Text = pct_out.ToString("P1");
            }
            if (e.Item.FieldName == "gm_pbmtd")
            {
                double.TryParse((t_gross_bmtd / t_rev_bmtd).ToString(), out pct_out);
                e.Text = pct_out.ToString("P1");
            }
            if (e.Item.FieldName == "gm_pbytd")
            {
                double.TryParse((t_gross_bytd / t_rev_bytd).ToString(), out pct_out);
                e.Text = pct_out.ToString("P1");
            }
        }
	}

	private void populate_companies()
	{
		company_switch.DataTextField = "name";
		company_switch.DataValueField = "business_unit_id";

        company_switch.DataSource = Toolbox.doSQL_dt(string.Format(@"Call get_daily_snapshot_at_date({0},curdate())", current_user.id32), null);
	//	company_switch.DataSource = Toolbox.doSQL_dt(string.Format(@"SELECT id, ddl_name name from business_unit  WHERE active = 'T' AND find_in_set(id,{0}) ORDER BY name", new Current_User().visible_business_units), null);

		company_switch.DataBind();
        if (company_switch.Items.FindByValue(Session["dashboard_business_unit_id"].ToString()) != null)
        {
            var comp_li = company_switch.Items.FindByValue(Session["dashboard_business_unit_id"].ToString());
            comp_li.Selected = true;
            comp_li.Text = ">> " + comp_li.Text;
        }
	}


    protected void cbo_dtes_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
