<%@ webhandler language="C#" class="site_side_panel_html" %>

using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using nesi.core;

public class site_side_panel_html : IHttpHandler, IReadOnlySessionState
	{
	public void ProcessRequest(HttpContext context)
		{
		using (var conn = Toolbox.connect())
			{
			context.Response.ContentType = "text/plain";
			var _q = context.Request.QueryString;
			var action = _q["a"];
			var current_user = (NeMember)context.Session["profile"];
			if(current_user == null) return;
			var is_control_branch = !current_user.isContact && current_user.business_unit.is_backoffice;
			if (action == "active_users")
				{
				if(!current_user.isContact)
					{
					var sb_whoson = new StringBuilder();
					sb_whoson.Append("<table cellpadding='3'>");
					var dt_whoson = Toolbox.get_whos_on();

					foreach (DataRow dr_whoson in dt_whoson.Rows)
					{
						sb_whoson.AppendFormat("<tr><td nowrap>{0}</td><td>{1}</td></tr>", dr_whoson[0], dr_whoson[1]);
					}
					sb_whoson.Append("</table>");
					context.Response.Write(sb_whoson.ToString());
					}
				}
			else if (action == "upcoming_vacations")
				{
				#region upcoming vacations
				var vac_builder = new StringBuilder();
				vac_builder.Append(@"
<div id='side_vacations' style='font-family:Calibri;'>
<style type='text/css'>
	#side_vacations
		{
font-family:Calibri;
		}
	#side_vacations .c1
		{
		display:	inline-block;
		width:		175px;
		}
	#side_vacations .c2,
	#side_vacations .c3
		{
		display:	inline-block;
		text-align:	center;
		width:		75px;
		}
	#side_vacations .entries
		{
		background-color: #eee;
		border-radius: 5px;
		}
	#side_vacations .b
		{
font-weight:bold;
		font-size:		16px;
		}
	#side_vacations .t
		{
		font-size:		14px;
		color:			#777;
		}
	#side_vacations .h
		{
		font-size:	12px;
		background-color:	#ddd;
		color:			#333;
		padding: 3px;
		}
	#side_vacations .c
		{
		font-size:	12px;
		padding: 3px;
		}
</style>
");
				#region vacation builder
				DataTable vacations_dt_this;
				DataTable vacations_dt_next;
				var query_pre = @"
SELECT 
	b.member_fullname AS _name,
	min(date(a.date_start)) date_start,
	a.date_return,
	b.business_unit_id business_unit_id,
	c.name, b.member_id
FROM
	vacation_master a
	LEFT JOIN member b ON a.member_id = b.Member_ID
	inner JOIN business_unit c ON b.business_unit_id = c.id and find_in_set (c.id,get_visible_business_units_group_concat(" + current_user.id32 + @"))
WHERE
";
				var query_order = @"
ORDER BY c.name,b.member_lastname, b.member_nickname, a.date_start,a.date_return";
				var query_group = @"
GROUP by b.member_fullname,a.date_return,b.business_unit_id,c.name";
					vacations_dt_this = Toolbox.doSQL_dt(conn, string.Format(@"
{0}
((WEEK(CURDATE()) = WEEK(a.date_start)) OR ((WEEK(CURDATE()) = WEEK(a.date_end)))) AND a.status = 3 AND YEAR(CURDATE())=YEAR(a.date_start) and 
b.member_status = 'Active' 
{1}
{2}", query_pre, query_group, query_order),null);

					vacations_dt_next = Toolbox.doSQL_dt(conn, string.Format(@"
{0}
((WEEK(CURDATE()+INTERVAL 1 WEEK) = WEEK(a.date_start)) OR ((WEEK(CURDATE()+ INTERVAL 1 WEEK) = WEEK(a.date_end)))) AND a.status=3 AND YEAR(CURDATE()+INTERVAL 1 WEEK)=YEAR(a.date_start) AND 
b.member_status = 'Active' 
{1}
{2}", query_pre, query_group, query_order),null);

				var branches_dt = Toolbox.doSQL_dt(conn, string.Format(@"SELECT id id, ddl_name name, country country 
from business_unit  
WHERE istest = 'F' AND active = 'T' AND name NOT LIKE 'Master%' AND find_in_set (id,get_visible_business_units_group_concat(" + current_user.id32 + @")) ORDER BY name"), null);
				foreach(DataRow branch in branches_dt.Rows)
					{
					var id					= branch["id"];
					var name				= branch["name"];
					var country				= branch["country"];
                    
					var entries_this		= vacations_dt_this.Select("business_unit_id = "+id );
					var entries_next		= vacations_dt_next.Select("business_unit_id = "+id );
					if(entries_this.Any() || entries_next.Any())
						{
						vac_builder.AppendFormat(@"
		<div class='b'>{0}</div>", name);
						if(entries_this.Any())
							{
							vac_builder.Append(@"
		<div class='t'>This Week</div>
		<div>
			<span class='c1 h'>Name</span>
			<span class='c2 h'>Starts</span>
			<span class='c3 h'>Returns</span>
		</div>
		<div class='entries' >");
							var prev_emp_name			= "";
							foreach (var dr in entries_this)
								{
								var employee = dr["_name"].ToString();
								var used_name = employee == prev_emp_name ? "" : employee;
								var top_border = used_name != "" ? "style='border-top: solid 1px #ddd;'" : "";
								var d_start = dr["date_start"];
								var d_return = dr["date_return"];
								vac_builder.AppendFormat(@"
	<div {3}>
		<span class='c1 c'>{0}</span>
		<span class='c2 c'>{1}</span>
		<span class='c3 c'>{2}</span>
	</div>",
								used_name,
								Toolbox.MySQL_shortdt((DateTime)d_start),
								Toolbox.MySQL_shortdt((DateTime)d_return),
								top_border
								);
								prev_emp_name		= employee;
								}

							vac_builder.Append(@"
		</div><br/>");
							}
						if(entries_next.Any())
							{
							vac_builder.Append(@"
		<div class='t'>Next Week</div>
		<div>
			<span class='c1 h'>Name</span>
			<span class='c2 h'>Starts</span>
			<span class='c3 h'>Returns</span>
		</div>
		<div class='entries' >");
							var prev_emp_name			= "";
							foreach (var dr in entries_next)
								{
								var employee = dr["_name"].ToString();
								var used_name = employee == prev_emp_name ? "" : employee;
								var top_border = used_name != "" ? "style='border-top: solid 1px #ddd;'" : "";
								var d_start = dr["date_start"];
								var d_return = dr["date_return"];
								vac_builder.AppendFormat(@"
	<div {3}>
		<span class='c1 c'>{0}</span>
		<span class='c2 c'>{1}</span>
		<span class='c3 c'>{2}</span>
	</div>",
								used_name,
								Toolbox.MySQL_shortdt((DateTime)d_start),
								Toolbox.MySQL_shortdt((DateTime)d_return),
								top_border
								);
								prev_emp_name		= employee;
								}
							vac_builder.Append(@"
		</div><br/><br/>");
							}
						else
							{
							vac_builder.Append(@"
		</br>");
							}
						}
					}
					context.Response.Write(vac_builder.ToString());
				#endregion vac builder
				#endregion upcoming vacations
				}
			}
		}

	public bool IsReusable
		{
		get
			{
			return false;
			}
		}

	}