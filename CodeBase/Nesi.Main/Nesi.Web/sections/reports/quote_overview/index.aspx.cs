using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using nesi.core;

public partial class sections_reports_quote_overview_index : System.Web.UI.Page
	{
	public NeMember myMember;
	private const int _page_id			= 81; // from Page table in DB
	private const string _page_description	= "Reports / Quote Overview";
	protected void Page_Load(object sender, EventArgs e)
		{
		var _tools						= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(_page_id);
		_tools.dont_cache_page();
		var _q				= Request.QueryString;
		object get_company					= myMember.business_unit_id;
		if(_q["business_unit_id"] != null)
			{
			get_company						= _q["business_unit_id"];
			}
		var this_company				= new NeBusinessUnit(myMember.business_unit_id);
		var menu							= new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(myMember);
		var lbltemp						= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text						= _page_description;
		var output						= "";
		DataTable _dt;
		try
			{
			_dt									= _tools.getSQL_datatable(@"
SELECT 
	quote_id,
	revision,
	date_format(open_date, '%m/%d/%Y') open_date, 
	customer_id,
	customer_name(customer_id) customer, 
	IFNULL(contact_name(contact_id), '&lt;No Contact&gt;') contact, 
	IFNULL(proper(job_description), 'N/A') job_description,
	quoted_by member_id,
	get_name(quoted_by) quoted_by, 
	quoted_price, 
	IFNULL(date_format(last_fax_date, '%m/%d/%Y'), '--') last_fax_date, 
	IFNULL(date_format(verified_date, '%m/%d/%Y'), '--') verified_date,
	IFNULL((SELECT date_format(MAX(schedule_date), '%m/%d/%Y') FROM quote_follow_up a WHERE a.quote_id = b.quote_id AND a.revision = b.revision), '--') follow_up_date
FROM 
	quote_master b
WHERE 
	status_id = 4 AND 
	quoted_by in 
		(
		SELECT 
			member_id
		FROM 
			member 
		WHERE 
			business_unit_id =@v0
		)
ORDER BY quoted_by, open_date", new object[] { get_company});
			}
		catch
			{
			_dt									= new DataTable();
			}
		output								= @"
		<select onchange=""location.href='./index.aspx?business_unit_id='+this.value"" style='font-weight:bold;width:100%;'>";


		    var visibleBusinessUnits = Toolbox.doSQL_dt(@"CALL get_visible_business_units(@v0 )", new object[] {  myMember.id } );
        visibleBusinessUnits.DefaultView.Sort = "[ddl_name] ASC";
        foreach (DataRow _company in visibleBusinessUnits.Rows)
			{
			var selected					= "";
			if(_company["id"].ToString() == get_company.ToString())
				{
				selected					= " selected";
				}
			else
				{
				selected					= "";
				}
			    var tmpCompanyID = _tools.getSQL_double( @"SELECT IFNULL(SUM(quoted_price), 0) FROM quote_master  WHERE status_id = 4 AND quoted_by IN (SELECT member_id FROM member WHERE business_unit_id ="+ _company["id"] + ")",null  );

            output += string.Format(@"
			<option value='{0}'{2}>{1} - {3}</option>",
				_company["id"],
				_company["ddl_name"],
				selected, 
                tmpCompanyID
                );
			}
		output								+= @"
		</select>
		<table id='this_report' width='100%' cellspacing='0' cellpadding='3'>
			<thead>
				<tr style='background-color:#000;color:#fff;'>
					<th>Date</th>
					<th>Customer</th>
					<th>Contact</th>
					<th>Phone #</th>
					<th>Price</th>
					<th>Date Sent</th>
					<th>Date Verified</th>
					<th>Follow Up Date</th>
				</tr>
			</thead>
			<tfoot>
				<tr>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
					<td></td>
				</tr>
			</tfoot>
			<tbody>
				";
		object quoted_by			= null;
		if(_dt.Rows.Count > 0)
			{
			foreach(DataRow _dr in _dt.Rows)
				{
				var quote_id				= _dr["quote_id"];
				var version				= _dr["revision"];
				var open_date			= _dr["open_date"];
				var customer				= _dr["customer"];
				var customer_id			= Convert.ToInt32(_dr["customer_id"]);
				var temp_customer	= new NECustomer(customer_id);
				object phone_number			= temp_customer.Address.PhoneNumber;
				var contact				= _dr["contact"];
				var job_description		= _dr["job_description"];
				if(quoted_by == null || quoted_by.ToString() != _dr["quoted_by"].ToString())
					{
					output					+= string.Format(@"
					<tr>
						<td colspan='9' style='background-color:#ccc;padding:5px;'><b>{0}</b> - <b style='background-color:#090;padding:3px;border:solid 1px #0f0;color:#0f0;'>{1}</b> - {2} Current Quote(s)</td>
					</tr>
					", _dr["quoted_by"],
					_tools.Monetize(_tools.getSQL_double(@"SELECT IFNULL(SUM(quoted_price), 0) FROM quote_master  WHERE quoted_by =? AND status_id = 4", new object[] { _dr["member_id"] })),
					_tools.getSQL_string(@"SELECT COUNT(quote_id) FROM quote_master  WHERE quoted_by =? AND status_id = 4", new object[] { _dr["member_id"] }));
					}
				quoted_by					= _dr["quoted_by"];
				var quoted_price			= _dr["quoted_price"];
				var last_fax_date		= _dr["last_fax_date"];
				var verified_date		= _dr["verified_date"];
				var follow_up_date		= _dr["follow_up_date"];
				output						+= string.Format(@"
					<span class='info'>
					<tr>
						<td colspan='9' style='border-top:solid 1px #ccc;'><a href='#{0}' name='{0}' onclick=""boing('/#/opens/65/quotes/{0}/{1}', 'quote', 1035, 750);""><b>{0} V{1}</b></a> - <b style='color:#090;'>({6})</b></td>
					</tr>
					<tr class='info'>
						<td>{2}</td>
						<td><a href='#cu{0}' name='cu{0}' onclick=""boing('/sections/customer/index.aspx?customer_id={11}', 'customer', 1035, 750);""><b>{3}</b></a></td>
						<td>{4}</td>
						<td align='center' class='phone_number'>{5}</td>
						<td class='quoted_price'><b style='color:#090;'>{7}</b></td>
						<td align='center'>{8}</td>
						<td align='center'>{9}</td>
						<td align='center'>{10}</td>
					</tr>
					</span>", 
					quote_id,							// 0
					version,							// 1
					open_date,							// 2
					customer,							// 3
					contact,							// 4
					phone_number,						// 5
					_tools.value_from(job_description),	// 6
					_tools.Monetize(quoted_price),		// 7
					last_fax_date,						// 8
					verified_date,						// 9
					follow_up_date,						// 10
					customer_id							// 11
					);
				}
			}
		else
			{
			output							+= @"
			<tr>
				<td colspan='9' style='padding:10px' align='center' valign='middle'><b>No current quotes.</b></td>
			</tr>
			";
			}
		output								+= @"
			</tbody>
		</table>";
		report.InnerHtml	= output;
		}
	}
