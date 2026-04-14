using System;
using System.Collections.Specialized;
using System.Data;
using nesi.core;

public partial class sections_reports_nesi_error_log_info : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var _q		= Request.QueryString;
		var id				= string.IsNullOrEmpty(_q["id"]) ? "" : _q["id"];

        var dt = Toolbox.doSQL_dt(@" SELECT MAX(e.main_id) id , e.member_id, m.member_fullname, MAX(e.error_timestamp) as dt, e.error_query, e.error_stackTrace, h.host_name, l.line_number, mn.machine_name, mes.message_text, o.origin_path, oq.queryString_string, u.userAgent_string, ip.userIP_address, concat(e.error_origin_id,'-', e.error_message_id) _master_id, ( SELECT COUNT(*) FROM error_log.error WHERE error_message_id = e.error_message_id AND error_origin_id = e.error_origin_id AND error_timestamp > CURDATE() ) AS error_count, ( SELECT COUNT(*) FROM error_log.error left join error_log.error_host h ON h.host_id = error.error_host_id WHERE error_message_id = e.error_message_id AND error_origin_id = e.error_origin_id AND error.error_is_local = false AND h.host_name != 'jordan.nesi.ca' AND h.host_name != 'andy.nesi.ca' AND h.host_name != 'matt.nesi.ca' AND h.host_name != 'iain.nesi.ca' ) AS total_error_count, e.error_is_global FROM error_log.error e left join neintranet.member m ON m.member_id = e.member_id left join error_log.error_host h ON h.host_id = e.error_host_id left join error_log.error_line l ON l.line_id = e.error_line_id left join error_log.error_machine_name mn ON mn.machine_id = e.error_machine_id left join error_log.error_messages mes ON mes.message_id = e.error_message_id left join error_log.error_origin o ON o.origin_id = e.error_origin_id left join error_log.error_origin_queryString oq ON oq.queryString_id = e.error_origin_queryString_id left join error_log.error_useragent u ON u.useragent_id = e.error_useragent_id left join error_log.error_userip ip ON ip.userip_id = e.error_userip_id where main_id = @v0  ", new object[] {  id } );
       // gv_info.DataSource = dt;
       // gv_info.DataBind();
        var dr = dt.Rows[0];
        if (dr["member_fullname"] != "")
        {
            lab_member.Text = dr["member_fullname"] + " (" + dr["member_id"] + ")";
        }
        lab_host.Text = dr["host_name"].ToString();
        lab_date.Text = dr["dt"].ToString();
        lab_server.Text = dr["machine_name"].ToString();
        lab_path.Text = dr["origin_path"] + dr["queryString_string"].ToString();
        lab_uag.Text = dr["userAgent_string"].ToString();
        lab_ip.Text = dr["userIP_address"].ToString();
        lab_message.Text = dr["message_text"].ToString();
        lab_query.Text = dr["error_query"].ToString();
        lab_stack.Text = dr["error_stackTrace"].ToString();

    }
}