using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class top_twenty : System.Web.UI.Page
	{
	public NeMember current_user;
	private const int _page_id = 71; // from Page table in DB
	private const string _page_description = "Reports / Top Twenty Customers By Sales";
	string start_date;
	string last_year = DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
	bool can_see_gross_margin = false;
	bool can_see_cost_info = false;
	bool approval_bm =false;
	Toolbox _tools;
	NeBusinessUnit this_company;

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_page_id);
		can_see_gross_margin = current_user.AuthenticatedForPrivilege(81);
		approval_bm = current_user.AuthenticatedForPrivilege(16);
		can_see_cost_info = current_user.AuthenticatedForPrivilege(58);
		_tools.dont_cache_page();
		var business_unit_id = ddlCompany.SelectedValue != "" ? Convert.ToInt32(ddlCompany.SelectedValue) : current_user.business_unit_id;
		this_company = new NeBusinessUnit(business_unit_id);

		}
	protected void Page_Load(object sender, EventArgs e)
		{


		if (!IsPostBack)
			{
			var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);



            ddlCompany.DataSource = NeBusinessUnit.units_filtered(new Current_User().visible_business_units);
			ddlCompany.DataBind();
			ddlCompany.SelectedValue = current_user.business_unit_id.ToString();


			// if (current_user.AuthenticatedForPrivilege(_page_id, "51"))
			//{
			ddlCompany.Enabled = true;
			//}

			}
		fill_grid();
		var show_grossmargin_info			= current_user.AuthenticatedForPrivilege(81) && !current_user.isContact;
		if(!show_grossmargin_info)
			{
			gv_top_customers.Columns["overall_margin"].Visible						= false;
			gv_top_customers.Columns["overall_margin"].ShowInCustomizationForm		= false;
			}
		}
	protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
		{


		}
	protected void Button1_Click(object sender, EventArgs e)
		{
		fill_grid();

		}

	protected void fill_grid()
		{

		var _tools = new Toolbox();

		if (!can_see_gross_margin)
			{
			gv_top_customers.Columns["YTD Profit"].Visible = false;
			gv_top_customers.Columns["Last YTD Profit"].Visible = false;
			chkProfitSel.Visible = false;
			}
		var year = 0;
		var month = 0;
		year = DateTime.Now.Year;
		month = DateTime.Now.Month;
		var strBVFirstMonthDay = year + month.ToString("d2") + "01";
		var strmysqlFirstMonthDay = year + month.ToString("d2") + "01";
		var strBVFirstyearDay = "";
		var strBVFirstLastYearDay = "";
		var strmysqlFirstyearDay = "";
		var strmysqlFirstLastYearDay = "";
		var strSelCriteria = "";
		var strTopNumber = "";


		strTopNumber = radio_top_choice.SelectedItem.Value;
		


		strBVFirstyearDay = this_company.fiscal_start_current.ToString("yyyyMMdd");
		strmysqlFirstyearDay = Toolbox.MySQL_shortdt(this_company.fiscal_start_current);
		strBVFirstLastYearDay = this_company.fiscal_start_previous.ToString("yyyyMMdd");
		strmysqlFirstLastYearDay = Toolbox.MySQL_shortdt(this_company.fiscal_start_previous);

		start_date = strmysqlFirstLastYearDay;

		if (chkProfitSel.Checked == true)
			{
			strSelCriteria = "SUM(woprog_invoicednettotal-(woprog_materialcost + woprog_laborcost))";
			}
		else
			{
			strSelCriteria = "SUM(woprog_invoicednettotal)";
			}


		var strsql = string.Format(@"
SELECT 
	woprog_customer_id as cust_id, 
	woprog_customername as Customer_Name,  
	SUM(woprog_invoicednettotal) as YTD_Sales, 
	SUM(woprog_invoicednettotal-(woprog_materialcost + woprog_laborcost)) as YTD_Profit,
	b.margin overall_margin
FROM 
	woprog a
LEFT JOIN
	customer b ON a.woprog_customer_id = b.customer_id
WHERE 
	a.business_unit_id = '{0}' AND 
	woprog_status = 'Invoiced' AND 
	woprog_invoicedate >= '{1}' 
GROUP BY 
	woprog_customername 
ORDER BY 
	{2} DESC 
LIMIT {3}",
		ddlCompany.SelectedValue,
		strmysqlFirstyearDay,
		strSelCriteria, radio_top_choice.Text
		);

		gv_top_customers.DataSource = _tools.getSQL_datatable(strsql, null );
		gv_top_customers.DataBind();

		if (gv_top_customers.Columns.Count <= 6)
			{
			// Past 4 years
			month = DateTime.Now.Month;
			var this_year = DateTime.Now.Year;
			#region this year minus 1
			var this_year_minus_1 = this_company.fiscal_current_year - 1;
			var gvc_1 = new GridViewDataColumn();
			gvc_1.Caption = this_year_minus_1 + " Sales";
			gvc_1.Name = "dynamic_sales";
			gvc_1.CellStyle.HorizontalAlign = HorizontalAlign.Center;
			gvc_1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			gv_top_customers.Columns.Add(gvc_1);
			#endregion this year minus 1
			#region this year minus 2
			var this_year_minus_2 = this_company.fiscal_current_year - 2;
			var gvc_2 = new GridViewDataColumn();
			gvc_2.Caption = this_year_minus_2 + " Sales";
			gvc_2.Name = "dynamic_sales";
			gvc_2.CellStyle.HorizontalAlign = HorizontalAlign.Center;
			gvc_2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			gv_top_customers.Columns.Add(gvc_2);
			#endregion this year minus 2
			#region this year minus 3
			var this_year_minus_3 = this_company.fiscal_current_year - 3;
			var gvc_3 = new GridViewDataColumn();
			gvc_3.Caption = this_year_minus_3 + " Sales";
			gvc_3.Name = "dynamic_sales";
			gvc_3.CellStyle.HorizontalAlign = HorizontalAlign.Center;
			gvc_3.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			gv_top_customers.Columns.Add(gvc_3);
			#endregion this year minus 3
			#region this year minus 4
			var this_year_minus_4 = this_company.fiscal_current_year - 4;
			var gvc_4 = new GridViewDataColumn();
			gvc_4.Caption = this_year_minus_4 + " Sales";
			gvc_4.Name = "dynamic_sales";
			gvc_4.CellStyle.HorizontalAlign = HorizontalAlign.Center;
			gvc_4.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
			gv_top_customers.Columns.Add(gvc_4);
			#endregion this year minus 4
			}


		}

	protected void gv_top_customers_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
		{
		var grid = (ASPxGridView)sender;

		var cust_id = 0;
		int.TryParse(grid.GetDataRow(e.VisibleIndex)["cust_id"].ToString(), out cust_id);

		if (e.DataColumn.Caption == "Last YTD Sales")
			{
			var month = DateTime.Now.Month;
			var base_year = DateTime.Now.Year;
			var base_day = DateTime.Now.Day;
			var start_year = this_company.fiscal_start_previous.Year;
			start_date = Toolbox.MySQL_shortdt(this_company.fiscal_start_previous);
			var end_year = this_company.fiscal_end_previous.Year;
			var end_date = end_year + "-" + month + "-" + base_day;
			e.Cell.Text = _tools.getSQL_double(@"Select PROC_GETCUSTOMERSALES_FORPERIOD(@v0,@v1,@v2,@v3)",new object[] { start_date,end_date,ddlCompany.SelectedValue,cust_id } ).ToString("C2");
			}
		else if (e.DataColumn.Caption == "Last YTD Profit")
			{
			e.Cell.Text = _tools.getSQL_double(@"Select PROC_GETCUSTOMERPROFIT_FORPERIOD(@v0,@v1,@v2,@v3)",new object[] { start_date,last_year,ddlCompany.SelectedValue,cust_id } ).ToString("C2");
			}
		if (e.DataColumn.Name == "dynamic_sales")
			{
			var month = DateTime.Now.Month;
			var base_year = Convert.ToInt32(e.DataColumn.Caption.Replace(" Sales", ""));
			var start_year = this_company.fiscal_start_current.Year;
			start_date = Toolbox.MySQL_shortdt(this_company.fiscal_start_current);
			var end_year = this_company.fiscal_end_current.Year;
			var end_date = Toolbox.MySQL_shortdt(this_company.fiscal_end_current);
			// get year
			e.Cell.Text = _tools.getSQL_double(@"SELECT PROC_GETCUSTOMERSALES_FORPERIOD(@v0,@v1,@v2,@v3)",new object[] { start_date,end_date,ddlCompany.SelectedValue,cust_id } ).ToString("C2");
			}
		if (e.Cell.Text == "$0.00")
			{
			e.Cell.Style.Add("color", "#ccc");
			}
		if (e.Cell.Text.Contains("("))
			{
			e.Cell.Style.Add("color", "#f00");
			}

		}
	protected void radio_top_choice_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
}