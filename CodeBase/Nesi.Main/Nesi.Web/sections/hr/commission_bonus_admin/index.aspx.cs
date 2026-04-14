using System;
using System.Data;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using nesi.core;

public partial class commission_bonus_admin_index : System.Web.UI.Page
	{
	NeMember myMember;
	private const int _page_id			= 62; // from Page table in DB
	private const string _page_description	= "Bonuses / Commission Administration";
	private const string Output				= "";

    protected void Page_Load(object sender, EventArgs e)
		{
		#region Variable Declaration
		var function_set				= new Functions();
		var _tools						= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(_page_id);
		function_set.admin					= myMember;
		var _q				= Request.QueryString;

		var menu							= new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(myMember);
		var lbltemp						= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text						= _page_description;
		var Output				= new StringBuilder();
		var business_unit_id					= "";
		int request_id;
		var requested_when				= "";
		var approved						= "";
		var approved_when				= "";
		var note							= "";
		var amount						= "";
		var type							= "";
		var save_type					= "";
		var payperiod_id					= "";
		var payperiod_daterange			= "";
		var member_id					= "";
		var member_name					= "";
		function_set.requested_by			= myMember.id.ToString();
		DataTable _dt;
		var action						= "";
		#endregion Variable Declaration
		if(_q["a"] != null)
			{
			action		= _q["a"];
			}

		if(action.Contains("xml"))
			{
			_tools.set_XML_header();
			}

		
		if(action != "" || (_q["business_unit_id"] != null || _q["member_id"] != null))
			{
			Response.Clear();
			if(action == "handle")
				{
					int.TryParse(_q["request_id"], out request_id);
					int handle_type;
					int.TryParse(_q["type"], out handle_type);
					long ts;
					long.TryParse(_q["ts"], out ts);
					try
						{
						function_set.handle_request(handle_type, ts, request_id);
						Response.Write("SUCCESS");
						}
					catch(Exception ee)
						{
						Response.Write("FAILED - "+ee.Message);
						}
					Response.End();
				}
			else if(action == "xml_requests")
				{
				_dt						= function_set.requests();
													Response.Clear();
													Output.Append(@"<requests>");
													foreach(DataRow _dr in _dt.Rows)
														{
																int.TryParse(_dr["id"].ToString(), out request_id);
																type = _dr["type"].ToString();
																payperiod_id = _dr["payperiod_id"].ToString();
																payperiod_daterange = _dr["payperiod_daterange"].ToString();
																member_name = _dr["member_name"].ToString();
																requested_when = _dr["requested_when"].ToString();
																approved = _dr["approved"].ToString();
																approved_when = _dr["approved_when"].ToString();
																amount = _dr["amount"].ToString();
																note = _dr["note"].ToString();
																var ts = Convert.ToDateTime(_dr["ts"]).Ticks;

																Output.AppendFormat(@"
	<request>
		<id>{0}</id>
		<type>{1}</type>
		<payperiod_id>{9}</payperiod_id>
		<payperiod_daterange>{2}</payperiod_daterange>
		<member_name>{3}</member_name>
		<requested_when>{4}</requested_when>
		<approved>{5}</approved>
		<approved_when>{6}</approved_when>
		<amount>{7}</amount>
		<note>{8}</note>
		<ts>{10}</ts>
	</request>", request_id, type, payperiod_daterange, member_name, requested_when, approved, approved_when, amount, HttpUtility.HtmlEncode(note), payperiod_id, ts);
														}
													Output.Append(@"</requests>");
													Response.Write(Output);
													Response.End();
				}
			}
		else
			{
			Output.Append(@"
	<table cellpadding='0' cellspacing='0'>
		<tr>
			<td class='requests' valign='top'>
			</td>
		</tr>
	</table>
	<script>
			get_requests();
	</script>");
			}
		outer.InnerHtml			= Output.ToString();
		} //ends onload
	}// Ends current_payroll class


public class Functions
	{
	Toolbox _tools					= new Toolbox();
	public string requested_by		= "";
	public NeMember admin;
	
	public bool save_request(string member_id, string payperiod_id, string amount, string note, string type, string requested_by, string save_type, string request_id)
		{
		var sql					= "";
			var paramObjects= new object[] {};
		switch (save_type)
			{
			case "new":
				sql		= @"INSERT INTO payroll_extra_payments (type, payperiod_id, member_id, requested_when, requested_by, approved, amount, note)
VALUES (@v4, @v1, @v0, now(), @v5, -1, @v2, @v3)";
				paramObjects	= new object[]
				{
					member_id, payperiod_id, amount, note, type, requested_by
				};
				break;
			case "edit":
				sql		= @"UPDATE payroll_extra_payments 
SET member_id = @v0, payperiod_id=@v1, amount=@v2, note=@v3 WHERE id = @v5";
				paramObjects = new object[]
				{
					member_id, payperiod_id, amount, note, type, request_id
				}; break;
			}
		try
		{
			
			_tools.getSQL_void(sql,paramObjects);
			return true;
			}
		catch
			{
			return false;
			}
		}
	public void handle_request(int _type, long _ts, int request_id)
		{
		var ts	= Toolbox.doSQL_datetime("SELECT ts FROM payroll_extra_payments WHERE id = @v0", new object[] {request_id});
		if(ts.Ticks > _ts)
			{
			throw new Exception("This request has already been handled.");
			}
		_tools.getSQL_void(@"UPDATE payroll_extra_payments SET approved = @v1, approved_when = now() WHERE id = @v0", new object[] { request_id, _type});
		}
	public DataTable requests()
		{
		return _tools.getSQL_datatable(@"
SELECT 
	a.id,
	a.type,
	CONCAT(b.member_firstname, ' ', b.member_lastname) member_name,
	DATE_FORMAT(a.requested_when, '%m/%d/%Y') requested_when,
	a.payperiod_id,
	CAST(CONCAT(DATE_FORMAT(d.startdate, '%m/%d/%Y'), ' - ', DATE_FORMAT(d.enddate, '%m/%d/%Y')) AS CHAR(50)) payperiod_daterange,
	a.approved,
	DATE_FORMAT(a.approved_when, '%m/%d/%Y @ %h:%i%p') approved_when,
	CONCAT('$',FORMAT(a.amount, 2)) amount,
	a.note,
	a.member_id mem_id,
	a.ts
FROM 
	payroll_extra_payments a
LEFT JOIN
	member b
		ON a.member_id = b.member_id
LEFT JOIN
	member c
		ON c.member_id = " + admin.id + @"
LEFT JOIN
	payperiods d
		ON a.payperiod_id = d.payperiodid
Left join 
	member e 
		on a.requested_by = e.member_id and e.member_status = 'Active'
WHERE 
	a.approved = -1 
	and FIND_IN_SET(a.requested_by, REPORTS_TO(" + admin.id + @"))
ORDER BY
	a.requested_when DESC",null);
	//AND
	//b.payroll_handler = "+admin.id+@"
		}
	public DataTable request(string request_id)
		{
		return _tools.getSQL_datatable(@" SELECT a.id, a.type, a.member_id, a.payperiod_id, b.business_unit_id, a.amount, a.note FROM payroll_extra_payments a LEFT JOIN member b ON a.member_id = b.member_id  WHERE id =@v0", new object[] { request_id });
		}
	public DataTable payperiods()
		{
		return _tools.getSQL_datatable(@"SELECT payperiodid payperiod_id, CONCAT(DATE_FORMAT(startdate, '%m/%d/%Y'), ' - ', DATE_FORMAT(enddate, '%m/%d/%Y')) daterange FROM payperiods  WHERE completed = 0 ORDER BY payperiodid" , null);
		}
	public DataTable member_list(string business_unit_id)
		{
		return _tools.getSQL_datatable(@"SELECT distinct(a.member_id) member_id, a.member_fullname name, a.member_lastname last, a.member_firstname first FROM member a  WHERE a.member_status = 'active' AND a.business_unit_id =@v0 ORDER BY last,first", new object[] { business_unit_id });
		}
	}