using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_dashboards_customer_grid : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 32;
	Toolbox _tools;

	private string _page_name = "New_Dashboard_customers";
	private NeBusinessUnit c;
	private bool _company_wide = false;
	private bool _region_wide = false;
	private bool _department_wide = false;
	private bool _branch_wide = false;
	private bool _base_level = false;
	protected NameValueCollection _q;

	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;

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
		layout.used_gv = gv_customergrid;
		if (Cache["ds_depend"] == null)
		{
			Cache["ds_depend"] = DateTime.Now;
		}
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;

		h.Set("gridview_id", "gv_customergrid");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();

	}
	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;
		c = new NeBusinessUnit(_q["cid"]);
		if ((!IsPostBack) && (!IsCallback))
		{
			var gl = new NeGridLayouts(current_user.id, _page_name);
			if (gl.GridLayoutID == 0)
			{
				gv_customergrid.FilterExpression = "Not [status] In ('Dead', 'Extinct', 'Merged')";
				gl.GridLayout_Layout = gv_customergrid.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_customergrid.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;
			Session["gv_new_dashboard_customers"] = null;
		}
		fill_grid();
	}

	protected void fill_grid()
	{
		var str_cust_list = _tools.getSQL_string(string.Format(@"
SELECT 
	IFNULL(
		(
		SELECT 
			GROUP_CONCAT(distinct a.customer_number) 
		FROM 
			customer a 
		LEFT JOIN 
			address b ON a.customer_id = b.address_table_id AND b.address_table = 'Customer' 
		LEFT JOIN 
			customer_sales_properties c ON a.customer_id = c.customer_id AND b.address_id = c.address_id 
		WHERE 
			(c.project_mgr_member_id = '{0}' ) AND 
			c.status_id != 6 and c.status_id !=4
		GROUP BY 
			c.project_mgr_member_id
		),'')", _q["mid"]), null);

		if (str_cust_list == "")
		{
			return;
		}

		if (Session["gv_new_dashboard_customers"] != null)
		{


		}
		else
		{// put in limiting logic here for whatever levels they can see.

			//	string str_cust_list = _tools.getSQL_string(@"Select GROUP_CONCAT(customer.Customer_Number) from customer  where Customer_Account_Manager=@v0", new object[] { _q["mid"],group by Customer_Account_Manager" });
			var dt = _tools.getSQL_datatable("select a.customer_number as cust, a.customer_name as name, 0 as credit_limit , " +
						 "0 as thirty,  " +
						 "0 as sixty, " +
						 "0 as ninety, " +
						 "0 as onetwenty, " +
						 "0 as onetwentyplus, " +
						 "0 as ebalance, " +
						 "'F' as OnHold, 0.00 as open_wos,'' as status, '' as Invoices, 0.0 as custid, 0 as creditdays, '' as customer_memo, '' as name_branch, '' as customer_notes, '' as customer_salesnotes,'' as status_ , '' as customer_id,90.0 as nextexpectedpay,0 as avg5, '' as arnotes , '' as cust_whyhold, '' as cust_whohold, " +
						 "0 as callcycle, '' as lastaction, 0 as countdown, 0 as customer_id, 0 as address_id, 0.0 as ytd_rev, 0.0 as rev_12mos, 0.0 as rev_24mos " +
						 "from CUSTOMER a " +
                         "where find_in_set(a.customer_number,@v0) ", new object[] { str_cust_list}
						 );

			//var sql = "select AR.CUST as cust, '' as name, CU.CREDIT_LINE as credit_limit , " +
			//			 "sum (case when (Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) <=30 then balance else 0 end) as thirty,  " +
			//			 "sum (case when ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) <=60) and ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >30) then balance else 0 end) as sixty, " +
			//			 "sum (case when ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) <=90) and ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >60) then balance else 0 end) as ninety, " +
			//			 "sum (case when ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) <=120) and ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >90) then balance else 0 end) as onetwenty, " +
			//			 "sum (case when ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >120) and ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >120) then balance else 0 end) as onetwentyplus, " +
			//			 "sum (case when (Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) <=30 then balance else 0 end)+ " +
			//			 "sum (case when ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) <=60) and ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >30) then balance else 0 end)+ " +
			//			 "sum (case when ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) <=90) and ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >60) then balance else 0 end)+ " +
			//			 "sum (case when ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) <=120) and ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >90) then balance else 0 end)+ " +
			//			 "sum (case when ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >120) and ((Datediff(day,Concat(Concat(concat(Concat(Left(ARF_DATE,4),'-'),Substring(ARF_DATE,5,2)),'-'),RIGHT(ARF_DATE,2)),Curdate())) >120) then balance else 0 end) as ebalance, " +
			//			 "'F' as OnHold, 0.00 as open_wos,'' as status, '' as Invoices, 0.0 as custid, 0 as creditdays, '' as customer_memo, '' as name_branch, '' as customer_notes, '' as customer_salesnotes,'' as status_ , '' as customer_id,90.0 as nextexpectedpay,0 as avg5, '' as arnotes , '' as cust_whyhold, '' as cust_whohold, " +
			//			 "0 as callcycle, '' as lastaction, 0 as countdown, 0 as customer_id, 0 as address_id, CU.YTD_SALES as ytd_rev, 0.0 as rev_12mos, 0.0 as rev_24mos " +
			//			 "from CUSTOMER AS CU,AR_TRANSACTIONS as AR " +
			//			 "where AR.CUST = CU.CUS_NO and (CODE='I' or CODE='C') and cu.cus_no in('"+ str_cust_list + "')   " +
			//			 "group by AR.CUST,CU.NAME,CU.CREDIT_LINE,CU.HOLD,CU.YTD_SALES ";
			//var dt_bv_ar = _tools.getSQL_datatable(sql, c.DSN, null);

			// TODO: Break this out into working per location... right now this is only going to work on the billing address.
			foreach (DataRow dr in dt.Rows)
			{
				var cust_details = _tools.getSQL_datatable(@" SELECT d.customer_or_contact_status, IFNULL(a.customer_id,0) id, IFNULL(a.customer_frequency_days,0) cf, IFNULL(a.customer_creditlimit,0) cl, a.customer_id, b.address_id FROM customer a LEFT JOIN address b ON a.customer_id = b.address_table_id AND b.address_table = 'Customer' LEFT JOIN customer_sales_properties c ON a.customer_id = c.customer_id AND b.address_id = c.address_id LEFT JOIN customer_or_contact_status AS d ON c.status_id = d.customer_or_contact_status_id  WHERE customer_number =@v0 limit 1 ", new object[] { dr["cust"] });

				var custid = Convert.ToInt32(cust_details.Rows[0]["id"]);
				dr["callcycle"] = Convert.ToInt32(cust_details.Rows[0]["cf"]);
				dr["countdown"] = _tools.getSQL_int(@"SELECT CAST((TO_DAYS((IFNULL((select customer_history.Customer_History_Date from customer_history  where Customer_History_CustID = a.customer_id and Customer_History_Action not in (3,6,18,19,20,21,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54) order by Customer_History_Date desc limit 1),(CURDATE() - INTERVAL 365 DAY)) + INTERVAL IFNULL(a.customer_frequency_days,0) DAY)) - TO_DAYS(CURDATE())) AS SIGNED) countdown from customer a where a.customer_number =@v0", new object[] { dr["cust"] });
				dr["lastaction"] = _tools.getSQL_string(@"select ifnull((SELECT history_action_type.History_Action_Type_Action FROM customer_history INNER JOIN history_action_type ON customer_history.Customer_History_Action = history_action_type.History_Action_Type_ID  WHERE customer_history.Customer_History_CustID =@v0 and Customer_History_Action not in (3,6,18,19,20,21,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54) ORDER BY customer_history.Customer_History_date DESC LIMIT 1),'None')", new object[] { custid });
				dr["rev_12mos"] = _tools.getSQL_double(@"Select ifnull((Select sum(woprog_invoicednettotal) from woprog  where woprog_customer_id =@v0 and woprog_invoicedate > (curdate()-interval 12 month)),0)", new object[] { custid });
				dr["rev_24mos"] = _tools.getSQL_double(@"Select ifnull((Select sum(woprog_invoicednettotal) from woprog  where woprog_customer_id =@v0 and woprog_invoicedate > (curdate()-interval 24 month)),0)", new object[] { custid });
				dr["open_wos"] = _tools.getSQL_double(@"Select ifnull((Select sum(woprog_stilltobebilled) from woprog  where woprog_customer_id =@v0 and woprog_status !='Invoiced'),0)", new object[] { custid });
				dr["custid"] = custid;
				dr["credit_limit"] = Convert.ToDouble(cust_details.Rows[0]["cl"]);
				dr["status"] = cust_details.Rows[0]["customer_or_contact_status"].ToString();
				dr["customer_id"] = cust_details.Rows[0]["customer_id"];
				dr["address_id"] = cust_details.Rows[0]["address_id"];
				//var tt = dt_bv_ar.Select("cust='" + dr["cust"] + "'");
				//if (tt.Length > 0)
				//{
				//	dr["thirty"] = tt[0]["thirty"];
				//	dr["sixty"] = tt[0]["sixty"];
				//	dr["ninety"] = tt[0]["ninety"];
				//	dr["onetwenty"] = tt[0]["onetwenty"];
				//	dr["onetwentyplus"] = tt[0]["onetwentyplus"];
				//	dr["ebalance"] = tt[0]["ebalance"];
				//	dr["ytd_rev"] = tt[0]["ytd_rev"];
				//}
				dt.DefaultView.Sort = "countdown asc";
				//	Session["gv_new_dashboard_customers"] = dt.DefaultView.Sort="countdown asc";
				Session["gv_new_dashboard_customers"] = dt;
			}

		}

		gv_customergrid.DataSource = Session["gv_new_dashboard_customers"];
		gv_customergrid.DataBind();
	}


	protected void gv_customergrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}


	protected void gv_customergrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{

	}

	protected void gv_customergrid_HtmlDataCellPrepared1(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if (e.DataColumn.FieldName == "countdown")
			{
				if (Convert.ToInt32(e.GetValue("countdown")) <= 0)
				{
					e.Cell.BackColor = System.Drawing.Color.Red;
					e.Cell.ToolTip = "You are past due to make contact with this customer";
				}
				else if (Convert.ToInt32(e.GetValue("countdown")) <= 5)
				{
					e.Cell.BackColor = System.Drawing.Color.Salmon;
					e.Cell.ToolTip = "Less than 5 days to make contact with this customer";
				}
			}
			else if (e.DataColumn.FieldName == "onetwentyplus")
			{
				if (Convert.ToInt32(e.GetValue("onetwentyplus")) > 5)
				{
					//		e.Cell.BackColor = System.Drawing.Color.Red;
					//		e.Cell.ToolTip = "Over 120 days Balance above 0";
				}

			}
			else if (e.DataColumn.FieldName == "credit_limit")
			{
				if ((Convert.ToInt32(e.GetValue("ebalance")) + Convert.ToInt32(e.GetValue("open_wos"))) > Convert.ToInt32(e.GetValue("credit_limit")))
				{
					e.Cell.BackColor = System.Drawing.Color.MistyRose;
					e.Cell.ToolTip = "Credit Limit Breached by all open wos and outstanding ar";
				}
			}
			else if (e.DataColumn.FieldName == "ebalance")
			{
				if ((Convert.ToInt32(e.GetValue("ebalance")) + Convert.ToInt32(e.GetValue("open_wos"))) > Convert.ToInt32(e.GetValue("credit_limit")))
				{
					e.Cell.BackColor = System.Drawing.Color.Red;
					e.Cell.ToolTip = "Credit Limit Breached by all open wos and outstanding ar";
				}

			}
		}
	}
	protected void ASPxTextBox1_Init(object sender, EventArgs e)
	{
		var txtsv = sender as ASPxTextBox;
		var container = txtsv.NamingContainer as GridViewDataItemTemplateContainer;
		txtsv.ClientSideEvents.TextChanged = string.Format(@"function (s, e) {{gv_customergrid.PerformCallback('{0}|' + s.GetValue() + '|s'); }}", container.KeyValue);
	}

	protected void gv_customergrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.Parameters.Length > 0)
		{
			var p = e.Parameters.Split('|');
			if (p.Length > 1)
			{
				if (p[2] == "s")
				{

					try
					{
						_tools.getSQL_void(@"update customer set customer_frequency_days =@v0  where customer_id =@v1  limit 1",
							new object[] {
Convert.ToInt32(p[1]),p[0]
								});
						Session["gv_new_dashboard_customers"] = null;
						fill_grid();
					}
					catch { throw new Exception("Invalid frequency of days set.. make sure its a number with no decimal or letters"); }

				}
				else if (p[2] == "la")
				{
					Session["gv_new_dashboard_customers"] = null;
					fill_grid();
				}
				else
				{
					gv.LoadClientLayout(e.Parameters);
				}


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
	}


	protected void pop_lastaction_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		if (e.Parameter.Contains("|"))
		{
			var props = e.Parameter.Split('|');
			uc_customer_lastaction.customer_id = Convert.ToInt32(props[0]);
			uc_customer_lastaction.address_id = Convert.ToInt32(props[1]);
			// This javascript snippet is passed to the user control to perform tasks after saving.
			uc_customer_lastaction.javascript_closeaction = "alert('Event Saved');pop_lastaction.Hide();gv_customergrid.PerformCallback('0|0|la');";
			uc_customer_lastaction.DataBind();
		}
		else
		{
			throw new Exception("Not a valid callback");
		}
	}
}
