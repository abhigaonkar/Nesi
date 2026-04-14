using System;
using System.Text;
using DevExpress.XtraCharts;
using System.Data;
using System.Collections.Specialized;
using nesi.core;

public partial class dashboard_modules_wosnapshot : System.Web.UI.Page
	{
	public Toolbox _tools		= new Toolbox();
	NeMember current_user		= new NeMember();
	protected void Page_Load(object sender, EventArgs e)
		{
		_tools					= new Toolbox();
		_tools.dont_cache_page();
		current_user			= Toolbox.do_handle_authentication(32);
		var _q = Request.QueryString;
		var business_unit_id = Toolbox.ReturnZeroIfNull_int(Session["dashboard_business_unit_id"]);
	
		if (business_unit_id == 0)
			{
			Response.Clear();
			Response.Write("<div align='center' style='color:red;font-family:arial;font-weight:bold;'>Branch not set!</div>");
			Response.End();
			}
	
		var _chart_wo_ds        = Toolbox.doSQL_dt(string.Format("select woprog_status Arguement, count(*) Value from woprog where business_unit_id = {0} and woprog_Status not in ('invoiced', 'deleted', 'waiting for po') AND woprog_status is not null group by woprog_status", business_unit_id),null);
		var wo_series              = chart_wo.Series[0];
		wo_series.DataSource          = _chart_wo_ds;
		wo_series.ArgumentDataMember  = "Arguement";
		wo_series.ArgumentScaleType   = ScaleType.Qualitative;
		wo_series.ValueScaleType      = ScaleType.Numerical;
		bm_workorders_title.InnerHtml = "<a href='/wo_prog_edit.aspx?business_unit_id=" + business_unit_id + "'>WO Snapshot</a>";
		wo_series.Visible = _chart_wo_ds.Rows.Count != 0;
		wo_series.ValueDataMembers.AddRange(new string[] { "Value" });

		
		
		var _chart_quote_ds = Toolbox.doSQL_dt(string.Format("SELECT b.status Arguement, count(*) Value FROM quote_master a left join quote_status b on a.status_id = b.id WHERE a.business_unit_id = {0} AND a.status_id NOT IN (6,5,7,8,9) group by a.status_id", business_unit_id),null);
		var quote_series = chart_quote.Series[0];
		quote_series.DataSource = _chart_quote_ds;
		quote_series.ArgumentDataMember = "Arguement";
		quote_series.ArgumentScaleType = ScaleType.Qualitative;
		quote_series.ValueScaleType = ScaleType.Numerical;
		quote_series.Visible = _chart_quote_ds.Rows.Count != 0;
		quote_series.ValueDataMembers.AddRange(new string[] { "Value" });

		
		var _chart_po_ds = Toolbox.doSQL_dt(string.Format("select b.status_type Arguement, count(*) Value from poprog_header a left join poprog_status b on a.poprog_status = b.poprog_status_id where business_unit_id = {0}  and poprog_status NOT IN (4,7,8) AND  poprog_status is not null group by poprog_status", business_unit_id),null);
		var po_series = chart_po.Series[0];
		po_series.DataSource = _chart_po_ds;
		po_series.ArgumentDataMember = "Arguement";
		po_series.ArgumentScaleType = ScaleType.Qualitative;
		po_series.ValueScaleType = ScaleType.Numerical;
		bm_purchaseorders_title.InnerHtml = "<a href='/sections/purchaseorder/po_prog_edit.aspx?business_unit_id=" + business_unit_id + "'>PO Snapshot</a>";
		po_series.ValueDataMembers.AddRange(new string[] { "Value" });
		po_series.Visible = _chart_po_ds.Rows.Count != 0;
		fill_anniversaries(business_unit_id);
		fill_tickets();
		}
	protected void fill_anniversaries(int business_unit_id)
		{
		var target				= anniversaries;
		var dt			= Toolbox.doSQL_dt(@" SELECT member_id id, CONCAT(member_nickname, ' ', member_lastname) name, DATE(member_startdate) startdate, (YEAR(NOW()) - YEAR(member_startdate)) years, DATEDIFF(CONCAT(YEAR(NOW()),'-',MONTH(member_startdate),'-', DAY(member_startdate)), NOW()) diff FROM member WHERE business_unit_id = @v0  AND member_status = 'Active' AND DATEDIFF(CONCAT(YEAR(NOW()),'-',MONTH(member_startdate),'-', DAY(member_startdate)), NOW()) BETWEEN 0 AND 30 ORDER BY diff ASC", new object[] {  business_unit_id } );
		if(dt.Rows.Count > 0)
			{
			var sb				= new StringBuilder();
			sb.Append(@"
<table cellpadding='4' cellspacing='0' class='anniversaries' style='font-size:11px;border:solid 1px #090;border-radius:5px;width:450px;'>
	<thead>
		<tr>
			<th colspan='3' style='background-color:#090;font-weight:bold;color:#fff;'>Upcoming Anniversaries</th>
		</tr>
		<tr style='background-color:#ccc;'>
			<th width='340'>Name</th>
			<th width='75'>Anniversary</th>
			<th width='35' style='width:35px'>Years</th>
		</tr>
	</thead>
	<tbody>
");
			foreach(DataRow dr in dt.Rows)
				{
				sb.AppendFormat(@"
<tr>
	<td align='left' width='340' title=""{0}"" style='border-bottom:solid 1px #ccc;border-right:solid 1px #ccc;'><div style='width:100%;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;'><b>{0}</b></div></td>
	<td align='center' width='75' style='border-bottom:solid 1px #ccc;border-right:solid 1px #ccc;'>{1:MMM d}</td>
	<td align='center' style='border-bottom:solid 1px #ccc;width:35px'>{2}</td>
</tr>
", dr["name"], dr["startdate"], dr["years"]);
				}
			sb.Append(@"
	</tbody>
</table>
");
			target.InnerHtml			= sb.ToString();
			}
		else
			{
			target.InnerHtml			= "No anniversaries in the next 30 days";
			}
		}
	protected void fill_tickets()
		{
		var target				= tickets;
		var dr			= Toolbox.doSQL_dt(@" SELECT (SELECT COUNT(*) FROM ticketheader WHERE ticketheader_member_assigned_id = @v0  AND ticketheader_status_id != 5) assigned, (SELECT COUNT(*) FROM ticketheader WHERE ticketheader_member_assigned_id = @v0  AND ticketheader_status_id != 5 AND WEEK(ticket_header_expected_completion) = WEEK(NOW())) due_user, (SELECT COUNT(*) FROM ticketheader WHERE ticketheader_createdby_member_id = @v0  AND ticketheader_status_id != 5 AND WEEK(ticket_header_expected_completion) = WEEK(NOW())) due_other ", new object[] {  current_user.id  } ).Rows[0];
		var sb					= new StringBuilder();
		sb.AppendFormat(@"
<table cellpadding='4' cellspacing='0' class='anniversaries' style='font-size:11px;border:solid 1px #090;border-radius:5px;width:450px;'>
	<thead>
		<tr>
			<th colspan='2' style='background-color:#090;font-weight:bold;color:#fff;'>Tickets</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td><b><a href=""javascript:boing('/sections/member/tickets/index.aspx?', 'tickets', 1300, 850);""># assigned to you:</a></b></td>
			<td align='center' width='35'>{0}</td>
		</tr>
		<tr>
			<td><b><a href=""javascript:boing('/sections/member/tickets/index.aspx?', 'tickets', 1300, 850);""># due this week:</a></b></td>
			<td align='center'>{1}</td>
		</tr>
		<tr>
			<td><b><a href=""javascript:boing('/sections/member/tickets/index.aspx?', 'tickets', 1300, 850);""># you've assigned, due this week:</a></b></td>
			<td align='center'>{2}</td>
		</tr>
	</tbody>
</table>
", dr["assigned"], dr["due_user"], dr["due_other"]);
			target.InnerHtml			= sb.ToString();
		}
	}