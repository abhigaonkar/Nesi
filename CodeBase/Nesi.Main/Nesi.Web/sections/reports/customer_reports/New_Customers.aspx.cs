using System;
using System.Data;
//using nesi.bv;
using nesi.core;

public partial class new_customers : System.Web.UI.Page
	{
	public NeMember myMember;

	private const int _page_id = 66; // from Page table in DB
	private const string _page_description = "Reports / New Customers";

	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();

		if (!IsPostBack)
			{
			var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
			divMenu.InnerHtml = menu.MenuHTML;
			divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);


			
            ddlCompany.DataSource = NeBusinessUnit.units_filtered(new Current_User().visible_business_units);
			ddlCompany.DataBind();
			ddlCompany.SelectedValue = myMember.business_unit_id.ToString();


			// if (myMember.AuthenticatedForPrivilege(_page_id, "51"))
			//{
			ddlCompany.Enabled = true;
			//}
			}
		}
	protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
		{
		GridView1.DataSource = null;
		GridView1.DataBind();
		totSale.Visible = false;
		TotCust.Visible = false;
		lblTotSale.Visible = false;
		lblCustNum.Visible = false;


		}
	protected void Button1_Click(object sender, EventArgs e)
		{
		var year = 0;
		var month = 0;
		year = DateTime.Now.Year;
		month = DateTime.Now.Month;
		var strFirstMonthDay = year + "-" + month.ToString("d2") + "-01";
		var strBVFirstMonthDay = year + month.ToString("d2") + "01";
		var branch = new NeBusinessUnit(ddlCompany.SelectedValue);
		var strFirstYearDay = Toolbox.MySQL_shortdt(branch.fiscal_start_current);
		var strBVFirstyearDay = "";
	//	var strARBalance = "";

		var TotalCustomers = 0;
		double TotalYear = 0;


		
		var strSQLSelectNewCust = "";
		//string strSQLSelectCurCust = "";
		var strCompInfo = "";
	//	var strBVARSql = "";
		var DSN = "";
		var strCountry = "";
		var strSelectCustomerMonDetails = "";
		var strSQLOpenCount = "";


		var myDataTable = new DataTable();

		myDataTable.Columns.Add("CustomerName", typeof(string));
		myDataTable.Columns.Add("MTDSales", typeof(string));
		myDataTable.Columns.Add("YTDSales", typeof(string));
		myDataTable.Columns.Add("FirstOpen", typeof(string));
		myDataTable.Columns.Add("TotalOpen", typeof(string));
		myDataTable.Columns.Add("TotalOpenDol", typeof(string));
		myDataTable.Columns.Add("ARBal", typeof(string));
		myDataTable.Columns.Add("is_qc1", typeof(string));
		myDataTable.Columns.Add("is_qc2", typeof(string));

		
		var business_unit			= new NeBusinessUnit(ddlCompany.SelectedValue);
		strCountry = business_unit.country;
		var strMonSelCriteria = "";
		var strMonSelCriteria2 = "";
		var strMonSelCriteria3 = "";
		if (chkMnth.Checked == true)
			{
			var strDate = ddlMonth.SelectedValue;
			var myear = DateTime.Now.Year;
			if (strCountry == "CAN")
				{
				if (Convert.ToInt32(strDate) >= 1 && Convert.ToInt32(strDate) <= 5)
					{
					myear = myear - 1;
					}
				}
			var monthdate = Convert.ToDateTime(myear + "-" + strDate + "-01");
			monthdate = monthdate.AddMonths(1).AddDays(-1);
			var strMonthDay = monthdate.ToString("yyyy-MM-dd");
			strMonSelCriteria = " AND Customer_Company_AddedTime < '" + strMonthDay + "' ";
			strMonSelCriteria2 = " AND WOProg_CutDateTime  <= '" + strMonthDay + "' ";
			strMonSelCriteria3 = " AND WOProg_InvoiceDate  <= '" + strMonthDay + "' ";
			}

		

		strSQLSelectNewCust = "SELECT Customer_ID, Customer_Number, urldecode(Customer_Name) customer_name, IF(customer_qc_member_id IS NOT NULL, 'True', 'False') is_qc1,  IF(customer_qc2_member_id IS NOT NULL, 'True', 'False') is_qc2 ";
		strSQLSelectNewCust += "From customer, customer_company ";
		strSQLSelectNewCust += "WHERE Customer_Company_AddedTime > '" + strFirstYearDay + "' " + strMonSelCriteria;
		strSQLSelectNewCust += "AND customer_company.business_unit_id =" + ddlCompany.SelectedValue;
		strSQLSelectNewCust += " AND Customer_Company_Customer_ID = Customer_ID ";
		strSQLSelectNewCust += " ORDER BY Customer_Name";
		
		var dtNewCompany = Toolbox.doSQL_dt(strSQLSelectNewCust,null);
		try
			{
			foreach (DataRow drNewCompany in dtNewCompany.Rows)
				{
				TotalCustomers++;
				var is_qc1 = drNewCompany["is_qc1"].ToString();
				var is_qc2 = drNewCompany["is_qc2"].ToString();
				var strSelectCustomerDetails = "";
				var strSelectFirstSale = "";
				var strSelectCustomerQuoteDetails = "";
				var strOpenCount = "";
				DateTime firstdate;
				var strFirstSaleDate = "";
				double dblTot = 0;
				double dblMonTot = 0;
		//		double dblARBal = 0;
				double dblOpenTotal = 0;
				var customer_id = Convert.ToInt32(drNewCompany["customer_id"]);
				strSQLOpenCount = "SELECT COUNT(*) FROM woprog WHERE WOProg_Status != 'Invoiced'  AND WOProg_Customer_ID=" + customer_id + " AND WOProg_CutDateTime  >= '" + strFirstYearDay + "'" + strMonSelCriteria2 + " AND business_unit_id=" + business_unit.id;
				strOpenCount = Toolbox.doSQL_int(strSQLOpenCount).ToString();

				strSelectCustomerDetails = "SELECT IFNULL(SUM(WOProg_InvoicedNetTotal),0) FROM woprog ";
				strSelectCustomerDetails += "WHERE WOProg_Status='Invoiced' AND WOProg_Customer_ID=" + customer_id;
				strSelectCustomerDetails += " AND WOProg_CutDateTime  >= '" + strFirstYearDay + "'" + strMonSelCriteria2 + " AND business_unit_id=" + business_unit.id;
				//strSelectCustomerDetails = "SELECT SUM(BVSUBTOTAL) FROM SALES_HISTORY_HEADER WHERE CUST_NO = " + drNewCompany[1] + " AND IN_DATE >= '" + strBVFirstyearDay + "'";


				TotalYear += Toolbox.doSQL_double(strSelectCustomerDetails, null);

				strSelectCustomerMonDetails = "SELECT IFNULL(SUM(WOProg_InvoicedNetTotal),0) FROM woprog ";
				strSelectCustomerMonDetails += "WHERE WOProg_Status='Invoiced' AND WOProg_Customer_ID=" + customer_id;
				strSelectCustomerMonDetails += " AND WOProg_InvoiceDate  >= '" + strFirstMonthDay + "'" + strMonSelCriteria3 + " AND business_unit_id=" + business_unit.id;

				dblMonTot = Toolbox.doSQL_double(strSelectCustomerMonDetails, null);

				strSelectCustomerQuoteDetails = "SELECT IFNULL(SUM(WOProg_QuotedAmount),0) quoted_amount, IFNULL(SUM(WOProg_LabourTotalSell),0)labor_total, IFNULL(SUM(WOProg_MaterialTotalSell),0) material_total FROM woprog ";
				strSelectCustomerQuoteDetails += "WHERE WOProg_Status!='Invoiced' AND WOProg_CutDateTime  >= '" + strFirstYearDay + "' " + strMonSelCriteria2 + " AND WOProg_Customer_ID=" + customer_id;
				strSelectCustomerQuoteDetails += " AND business_unit_id=" + business_unit.id;

				var dtQuote = Toolbox.doSQL_dt(strSelectCustomerQuoteDetails, null);
				var drQuote = dtQuote.Rows[0];
				try
					{
					if (Convert.ToDouble(drQuote[0].ToString()) == 0)
						{
						dblOpenTotal = Convert.ToDouble(drQuote[1].ToString()) + Convert.ToDouble(drQuote[2].ToString());
						}
					else
						{
						dblOpenTotal = Convert.ToDouble(drQuote[0].ToString());
						}

					}
				catch { dblOpenTotal = 0; }


				strSelectFirstSale = "SELECT WOProg_CutDateTime FROM woprog WHERE WOProg_Customer_ID=" + customer_id+ " AND business_unit_id=" + business_unit.id + " ORDER BY WOProg_CutDateTime LIMIT 1";
				
				try
					{
					firstdate = Toolbox.doSQL_datetime(@"SELECT WOProg_CutDateTime FROM woprog  WHERE WOProg_Customer_ID=@v0 AND business_unit_id=@v1  ORDER BY WOProg_CutDateTime LIMIT 1", new object[] { customer_id,business_unit.id });
					strFirstSaleDate = firstdate.ToString("MM-dd-yy");
					}
				catch
					{
					strFirstSaleDate = "NA";
					}


/*
				if (chkARSel.Checked == true)
					{
					using (var bv_conn = BVDB.connect(business_unit.DSN))
						{
						strBVARSql = "SELECT SUM(AR.BALANCE) FROM AR_TRANSACTIONS AS AR WHERE AR.BALANCE <> 0 AND AR.CODE = 'I' AND LTRIM(RTRIM(AR.CUST)) = '" + drNewCompany[1].ToString().Trim() + "'";


						
						try
							{
							dblARBal = BVDB.getSQL_double(bv_conn,@"SELECT SUM(AR.BALANCE) FROM AR_TRANSACTIONS AS AR  WHERE AR.BALANCE <> 0 AND AR.CODE = 'I' AND LTRIM(RTRIM(AR.CUST)) =?", new object[] { drNewCompany[1].ToString().Trim() });
							}
						catch
							{
							dblARBal = 0;
							}

						strARBalance = dblARBal.ToString();
						}

					}
				else
					{
					dblARBal = 0;
					strARBalance = "Not Included";
					}
*/
				myDataTable.Rows.Add(drNewCompany[2].ToString(),
									 dblMonTot.ToString(),
									 dblTot.ToString(),
									 strFirstSaleDate,
									 strOpenCount,
									 dblOpenTotal.ToString(),
		//							 strARBalance,
									 is_qc1,
									 is_qc2);

				}

			totSale.Visible = true;
			TotCust.Visible = true;
			lblTotSale.Visible = true;
			lblCustNum.Visible = true;
			lblTotSale.Text = TotalYear.ToString();
			lblCustNum.Text = TotalCustomers.ToString();

			//}
			}
		catch (Exception ex) { throw; }
		GridView1.DataSource = myDataTable;
		GridView1.DataBind();
		}
	}
