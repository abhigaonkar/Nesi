using System;
using System.Data;
using MySql.Data.MySqlClient;
using nesi.core;

public partial class mobile_modules_dashboard : System.Web.UI.UserControl
	{
	Toolbox _tools;
	NeMember current_user;
	private bool _company_wide = false;
	private bool _region_wide = false;
	private bool _branch_wide = false;
	private bool _base_level = false;

	protected void Page_Init(object sender, EventArgs e)
		{
			_tools = new Toolbox();
			current_user = new NeMember(Session["session"].ToString());
			_company_wide = current_user.AuthenticatedForPrivilege(146);
			_region_wide = current_user.AuthenticatedForPrivilege(147);
			_branch_wide = current_user.AuthenticatedForPrivilege(149);
			_base_level = current_user.AuthenticatedForPrivilege(150);
		}

	protected void Page_Load(object sender, EventArgs e)
		{
		_tools				= new Toolbox();
		if(!IsPostBack)
			{
			
			}
		fill_page();	
	
		}
	protected void fill_page()
	{
quote_row.Visible = false;
wo_row.Visible = false;
		if (_base_level || _branch_wide || _region_wide || _company_wide)
		{

			quote_row.Visible = true;
			wo_row.Visible = true;

			#region quote
			Label3.Text = _tools.getSQL_int(@"SELECT Count(quote_master.quote_id) FROM quote_master  WHERE quote_master.quoted_by =@v0 AND quote_master.status_id in (1,2,3)", new object[] { current_user.id }).ToString();

			Label4.Text = _tools.getSQL_int(@"SELECT Count(quote_master.quote_id) FROM quote_master  WHERE quote_master.quoted_by =@v0 AND quote_master.status_id = 4", new object[] { current_user.id }).ToString();



			#endregion

			#region wo

			if (current_user.MemberTypeID == 5)
			{
				HyperLink4.Text = _tools.getSQL_int(@"SELECT Count(woprog.WOProg_ID) FROM woprog  WHERE woprog.woprog_pm_memberid =@v0 AND woprog.WOProg_Status IN (11,6,7)", new object[] { current_user.id }).ToString();
			}
			else
			{
				

				HyperLink4.Text = _tools.getSQL_int(@"SELECT Count(woprog.WOProg_ID) FROM woprog  WHERE woprog.woprog_pm_memberid =@v0 AND woprog.WOProg_Status IN (11,6)", new object[] { current_user.id }).ToString();
			}
			HyperLink3.Text = _tools.getSQL_double(@"Select ifnull((SELECT sum(wmargingy.WOProg_InvoicedNetTotal - wmargingy.WOProg_LaborCost - wmargingy.WOProg_MaterialCost)/sum(wmargingy.WOProg_InvoicedNetTotal) FROM woprog wmargingy  WHERE wmargingy.woprog_pm_memberid =@v0 AND wmargingy.WOProg_InvoiceDate>get_fiscal_year_start_date(@v1)),0)", new object[] { current_user.id,current_user.business_unit_id }).ToString("p1");
			#endregion

		}

		#region hu
		var p = new NePayPeriod().CurrentPayPeriod();
		double wo = 0;
		double quote = 0;
		double shop = 0;
		double tot = 0;
		double hu = 0;
		var startdate = new NePayPeriod(Convert.ToInt32(p)).StartDate;

		var dt = _tools.getSQL_datatable(@" SELECT SUM(a.numberofhours), a.wotype FROM membertime a INNER JOIN member b ON a.membertime_memberid = b.member_id  WHERE a.date >=@v0 AND a.membertime_child_companyid != b.business_unit_id AND a.membertime_child_companyid = 0 AND a.membertime_memberid =@v1 GROUP BY a.business_unit_id, a.wotype", new object[] { startdate,current_user.id });
		foreach (DataRow dr in dt.Rows)
		{
			if (dr[1].ToString().ToUpper() == "QUOTE")
			{
				quote += Convert.ToDouble(dr[0]);
			}
			else if (dr[1].ToString().ToUpper() == "WO")
			{
				wo += Convert.ToDouble(dr[0]);
			}
			else
			{
				shop += Convert.ToDouble(dr[0]);
			}

			tot += Convert.ToDouble(dr[0]);

		}
		if (tot > 0)
		{
			hu = wo / tot;
		}
		HyperLink1.Text = hu.ToString("p0");
		HyperLink2.Text = wo.ToString("n1");
		#endregion

		#region tickets
		var tickets = new NETickets();
		var x = Toolbox.doSQL_dt(@"CALL GETTICKETS(@v0 , 'GetMyCourt')", new object[] {  current_user.id } ).Rows.Count;
		Label7.Text = "0";
		if (x > 0)
		{
			Label7.Text = x.ToString();
		}
		x = _tools.getSQL_int(@"SELECT Count(ticketheader.ticketheader_id) FROM ticketheader  WHERE ticketheader.ticketheader_createdby_member_id =@v0 AND ticketheader.ticketheader_status_id IN (1,2,6,8,9)", new object[] { current_user.id });
		Label8.Text = x.ToString();


		#endregion

		#region messages
		var NewTotalMessages = new NeMessaging();
		using (var _conn = Toolbox.connect())
			{
			var totalpersonal = (NewTotalMessages.TotalNewmessages(_conn, 1, current_user.id)).ToString();
			var totaladmin = (NewTotalMessages.TotalNewmessages(_conn,2, current_user.id)).ToString();
			var SystemNo = (NewTotalMessages.TotalNewmessages(_conn,3, current_user.id)).ToString();
			var memberName = current_user.FullName;
			Label9.Text = Convert.ToString(Convert.ToDouble(totalpersonal) + Convert.ToDouble(totaladmin) + Convert.ToDouble(SystemNo));
			}
		
		#endregion
	}


}		 