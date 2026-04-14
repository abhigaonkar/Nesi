using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using nesi.core;

public partial class Payroll_Report : System.Web.UI.Page
	{
	NeMember myMember;
	private const int _page_id			= 14; // from Page table in DB
	private const string _page_description	= "Payroll Reports";
	private const string Output				= "";

    protected void Page_Load(object sender, EventArgs e)
		{
		var this_report			= new NePayrollReport();
		var _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();

		var menu							= new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(myMember);
		var lbltemp						= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text						= _page_description;


		var Output						= "&nbsp;";
		var can_see_wage					= false;//myMember.AuthenticatedForPrivilege(3,101);
		var reader						= new DataTable();
		var sub_reader						= new DataTable();
		var available_reports			= "";
		var member_id					= "";
		var business_unit_id					= "";
		var name					= "";
		var member_fname					= "";
		var member_lname					= "";
		var member_name					= "";
		var report_id					= "";
		var report_abbr					= "";
		var report_name					= "";
		var company_selector				= "";

		var action						= "";
		if(Request.QueryString["a"] != null)
			{
			action		= Request.QueryString["a"];
			}

		if(action.Contains("xml"))
			{
			Response.Clear();
			HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
			HttpContext.Current.Response.Cache.SetValidUntilExpires(false); 
			HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
			HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			HttpContext.Current.Response.Cache.SetNoStore();
			Response.ContentType	= "text/xml";
			Response.Write("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>"); 
			Output						= "";
			}

		var _reports				= this_report.available_reports();
		if(_reports.Rows.Count > 0)
			{
			available_reports				= @"
	<select class='_reports' onchange=""location.href='./index.aspx?r='+this.value"" multiple>";
			foreach(DataRow dr in _reports.Rows)
				{
				report_id					= dr["id"].ToString();
				report_name					= dr["name"].ToString();
				available_reports			+= string.Format(@"
		<option value='{0}'>{1}", report_id, report_name);
				}
			available_reports				+= @"
	</select>";
			}
		reports.InnerHtml					= available_reports;

//---------------------------------------------------------------------------------------------------------------------------------//
//---------------------------------------------------------------------------------------------------------------------------------//


		if(Request.QueryString["a"] != null)
			{
			switch(action)
				{
//---------------------------------------------------------------------------------------------------------------------------------//
				case "xml_member":					business_unit_id				= Request.QueryString["business_unit_id"];
													reader					= this_report.member_list(business_unit_id);
													Output					+= @"
<results>";
													foreach(DataRow dr in reader.Rows)
														{
														member_id			= dr["member_id"].ToString();
														member_name			= dr["name"].ToString();
														Output				+= string.Format(@"
	<member id=""{0}"" name=""{1}"" />", member_id, member_name);
														}
													Output					+= @"
</results>";
													reader.Dispose();
													Response.Write(Output);
													Response.End();
				break;
//---------------------------------------------------------------------------------------------------------------------------------//
				// E1 = Employee by Type
				case "xml_E1":						business_unit_id				= Request.QueryString["business_unit_id"];
													reader					= this_report.report_E1_totals(business_unit_id);
													Output					+= @"
<results>";
													foreach(DataRow dr in reader.Rows)
														{
														var type			= dr["type"].ToString();
														var type_id		= dr["type_id"].ToString();
														var count		= dr["count"].ToString();
														Output				+= string.Format(@"
	<membertype type=""{0}"" count=""{1}"">", type, count);
														sub_reader			= this_report.report_E1_members(business_unit_id, type_id);
														foreach(DataRow sub_dr in sub_reader.Rows)
															{
															member_id		= sub_dr["member_id"].ToString();
															member_fname	= sub_dr["fname"].ToString();
															member_lname	= sub_dr["lname"].ToString();
															Output			+= string.Format(@"
		<member id=""{0}"" first_name=""{1}"" last_name=""{2}"" />", member_id, member_fname, member_lname);
															}
														Output				+= @"
	</membertype>";
														}
													Output					+= @"
</results>";
													reader.Dispose();
													Response.Write(Output);
													Response.End();
				break;
//---------------------------------------------------------------------------------------------------------------------------------//
				// E2 = Employee - Wage Report
				case "xml_E2":						business_unit_id					= Request.QueryString["business_unit_id"];
													reader						= this_report.member_list(business_unit_id);
													Output						+= @"
<results>";
													
													foreach(DataRow dr in reader.Rows)
														{
														member_id				= dr["member_id"].ToString();
														sub_reader				= this_report.report_E2_details(member_id);
														foreach(DataRow sub_dr in sub_reader.Rows)
															{
															member_fname		= sub_dr["fname"].ToString();
															member_lname		= sub_dr["lname"].ToString();
															var previous		= can_see_wage ? sub_dr["previous"].ToString() : "--";
															var last_raise	= sub_dr["last_raise"].ToString();
															var current		= can_see_wage ? sub_dr["current"].ToString() : "--";
															Output				+= string.Format(@"
	<member firstname=""{0}"" lastname=""{1}"" previous=""{2}"" last_raise=""{3}"" current=""{4}"" />", member_fname, member_lname, previous, last_raise, current);
															}
														}
													Output					+= @"
</results>";
													reader.Dispose();
													Response.Write(Output);
													Response.End();
				break;
//---------------------------------------------------------------------------------------------------------------------------------//
				// E3 = Employee - Length of Service
				case "xml_E3":						business_unit_id				= Request.QueryString["business_unit_id"];
													reader					= this_report.report_E3_members(business_unit_id);
													Output					+= @"
<results>";
													foreach(DataRow dr in reader.Rows)
														{
														member_fname		= dr["fname"].ToString();
														member_lname		= dr["lname"].ToString();
														var startdate		= dr["startdate"].ToString();
														var paytype			= dr["paytype"].ToString();
														var length			= dr["length"].ToString();
														Output				+= string.Format(@"
	<member firstname=""{0}"" lastname=""{1}"" startdate=""{2}"" paytype=""{3}"" length=""{4}"" />", member_fname, member_lname, startdate, paytype, length);
														}
													Output					+= @"
</results>";
													reader.Dispose();
													Response.Write(Output);
													Response.End();
				break;
//---------------------------------------------------------------------------------------------------------------------------------//
				}
			}


//---------------------------------------------------------------------------------------------------------------------------------//
//---------------------------------------------------------------------------------------------------------------------------------//


		if(Request.QueryString["r"] != null)
			{
			report_id						= Request.QueryString["r"];
			report_abbr						= this_report.report_abbr(report_id);
			report_name						= this_report.report_name(report_id);
			Output							= string.Format(@"
	<div class='report_name'>{0}</div>
	<div id='{1}'>", report_name, report_abbr);
			switch(report_abbr)
				{
//---------------------------------------------------------------------------------------------------------------------------------//
				case "E1":	// Employee By Type
					reader					= this_report.company_list();
					company_selector		= @"
						<select class='company_list' onchange='if(this.value != 0){report_E1(this.value);}'>
							<option value='0'>Please choose a Business Unit";
					if(reader.Rows.Count > 0)
						{
						foreach(DataRow dr in reader.Rows)
							{
							business_unit_id			= dr["id"].ToString();
							name		= dr["ddl_name"].ToString();
							company_selector	+= string.Format(@"
							<option value='{0}'>{1}", business_unit_id, name);
							}
						}
					else
						{
						
						}
					company_selector			+= @"
						</select>";
					Output						+= company_selector;
					Output						+= "<table cellspacing='0' cellpadding='5' id='E1_rowset'></table>";
				break;
//---------------------------------------------------------------------------------------------------------------------------------//
				case "E2":	// Employee - Wage Report
					reader					= this_report.company_list();
					company_selector		= @"
						<select class='company_list' onchange='if(this.value != 0){report_E2(this.value);}'>
							<option value='0'>Please choose a company";
					if(reader.Rows.Count > 0)
						{
						foreach(DataRow dr in reader.Rows)
							{
							business_unit_id			= dr["id"].ToString();
							name		= dr["ddl_name"].ToString();
							company_selector	+= string.Format(@"
							<option value='{0}'>{1}", business_unit_id, name);
							}
						}
					else
						{
						
						}
					company_selector			+= @"
						</select>";
					Output						+= company_selector;
					Output						+= "<table cellspacing='0' cellpadding='5' id='E2_rowset'></table>";
				break;
//---------------------------------------------------------------------------------------------------------------------------------//
				case "E3":	// Employee Length of Service
					reader					= this_report.company_list();
					company_selector		= @"
						<select class='company_list' onchange='if(this.value != 0){report_E3(this.value);}'>
							<option value='0'>Please choose a company";
					if(reader.Rows.Count > 0)
						{
						foreach(DataRow dr in reader.Rows)
							{
							business_unit_id			= dr["id"].ToString();
							name		= dr["ddl_name"].ToString();
							company_selector	+= string.Format(@"
							<option value='{0}'>{1}", business_unit_id, name);
							}
						}
					else
						{
						
						}
					company_selector			+= @"
						</select>";
					Output						+= company_selector;
					Output						+= "<table cellspacing='0' cellpadding='5' id='E3_rowset'></table>";
				break;
//---------------------------------------------------------------------------------------------------------------------------------//
				}
					Output						+= @"
	</div>";

			}


		report.InnerHtml			= Output;
		} //ends onload
	}// Ends current_payroll class









public class NePayrollReport
	{
	string sql;
	string output;

	public DataTable member_list(string business_unit_id)
		{
		
		sql							= "SELECT distinct(a.member_id) member_id, a.member_fullname name, a.member_lastname last, a.member_nickname first FROM member a WHERE a.member_status = 'active' AND a.business_unit_id = "+business_unit_id+" ORDER BY last,first";
		return Toolbox.doSQL_dt(@"SELECT distinct(a.member_id) member_id, a.member_fullname name, a.member_lastname last, a.member_nickname first FROM member a  WHERE a.member_status = 'active' AND a.business_unit_id =@v0 ORDER BY last,first", new object[] { business_unit_id });
		}

	public DataTable available_reports()
		{
		sql							= "SELECT * FROM report ORDER BY name";
		return Toolbox.doSQL_dt(@"SELECT * FROM report ORDER BY name"  , null);
		}

	public string report_abbr(string report_id)
		{
		sql							= "SELECT abbr FROM report WHERE id = "+report_id;
		output						= Toolbox.doSQL_string(@"SELECT abbr FROM report  WHERE id =@v0", new object[] { report_id });
		return output;
		}

	public string report_name(string report_id)
		{
		sql							= "SELECT name FROM report WHERE id = "+report_id;
		output						= Toolbox.doSQL_string(@"SELECT name FROM report  WHERE id =@v0", new object[] { report_id });
		if(output == "")
			{
			output			= "NONEXISTANT REPORT ID";
			}

		return output;
		}

	public DataTable company_list()
		{
		
		sql							= "SELECT * from business_unit  WHERE enable_timesheet = 1";
		return Toolbox.doSQL_dt(@"SELECT * from business_unit  WHERE enable_timesheet = 1" , null);
		}

	public DataTable report_E1_totals(string business_unit_id)
		{
		
		sql							= @"
select 
	b.membertype_name type,	
	b.membertype_id type_id,
	count(a.member_id) count 
from 
	member a 
left join 
	membertype b 
		on a.member_membertype_id = b.membertype_id 
where 
	a.business_unit_id = "+business_unit_id+@" and 
	a.member_status = 'active' 
group by 
	a.member_membertype_id";
		return Toolbox.doSQL_dt(@" select b.membertype_name type, b.membertype_id type_id, count(a.member_id) count from member a left join membertype b on a.member_membertype_id = b.membertype_id  where a.business_unit_id =@v0 and a.member_status = 'active' group by a.member_membertype_id", new object[] { business_unit_id });
		}

	public DataTable report_E1_members(string business_unit_id, string type_id)
		{
		
		sql							= "SELECT distinct(a.member_id) member_id, a.member_fullname name, a.member_lastname lname, a.member_nickname fname FROM member a WHERE a.member_status = 'active' AND a.business_unit_id = "+business_unit_id+" AND a.member_membertype_id = "+type_id+" ORDER BY lname,fname";
		return Toolbox.doSQL_dt(@"SELECT distinct(a.member_id) member_id, a.member_fullname name, a.member_lastname lname, a.member_nickname fname FROM member a  WHERE a.member_status = 'active' AND a.business_unit_id =@v0 AND a.member_membertype_id =@v1  ORDER BY lname,fname", new object[] { business_unit_id,type_id });
		}
	
	public DataTable report_E2_details(string member_id)
		{
		
		sql							= string.Format(@"
SELECT 
(SELECT member_nickname FROM member WHERE member_id = {0}) fname, 
(SELECT member_lastname FROM member WHERE member_id = {0}) lname, 
IFNULL((SELECT IF(currentwage > 100, ROUND(currentwage / 80, 2), currentwage) FROM memberwage WHERE memberwage_memberid = {0} ORDER BY currentwage DESC LIMIT 1,1), 0.00) previous, 
IFNULL((SELECT DATE_FORMAT(date, '%m/%d/%Y') FROM memberwage WHERE memberwage_memberid = {0} ORDER BY currentwage DESC LIMIT 1,1), '00/00/0000') last_raise, 
IFNULL((SELECT IF(currentwage > 100, ROUND(currentwage / 80, 2), currentwage) FROM memberwage WHERE memberwage_memberid = {0} ORDER BY currentwage DESC LIMIT 0,1), 0.00) current", member_id);
		return Toolbox.doSQL_dt(@" SELECT (SELECT member_nickname FROM member WHERE member_id = @v0 ) fname, (SELECT member_lastname FROM member WHERE member_id = @v0 ) lname, IFNULL((SELECT IF(currentwage > 100, ROUND(currentwage / 80, 2), currentwage) FROM memberwage WHERE memberwage_memberid = @v0  ORDER BY currentwage DESC LIMIT 1,1), 0.00) previous, IFNULL((SELECT DATE_FORMAT(date, '%m/%d/%Y') FROM memberwage WHERE memberwage_memberid = @v0  ORDER BY currentwage DESC LIMIT 1,1), '00/00/0000') last_raise, IFNULL((SELECT IF(currentwage > 100, ROUND(currentwage / 80, 2), currentwage) FROM memberwage WHERE memberwage_memberid = @v0  ORDER BY currentwage DESC LIMIT 0,1), 0.00) current", new object[] {  member_id } );
		}

	public DataTable report_E3_members(string business_unit_id)
		{
		
		sql							= "select a.member_nickname fname, a.member_lastname lname, DATE_FORMAT(a.member_startdate, '%m/%d/%Y') startdate, IFNULL(b.paytype, '-') paytype, round(datediff(current_date(), member_startdate)/360, 2) length from member a left join currentwage b on a.member_id = b.member_id where a.business_unit_id = "+business_unit_id+" and a.member_status = 'active' ORDER BY lname,fname";
		return Toolbox.doSQL_dt(@"select a.member_nickname fname, a.member_lastname lname, DATE_FORMAT(a.member_startdate, '%m/%d/%Y') startdate, IFNULL(b.paytype, '-') paytype, round(datediff(current_date(), member_startdate)/360, 2) length from member a left join currentwage b on a.member_id = b.member_id  where a.business_unit_id =@v0 and a.member_status = 'active' ORDER BY lname,fname", new object[] { business_unit_id });
		}
}