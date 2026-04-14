using System;
using System.Web.UI;
using DevExpress.Web;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Generic;
using nesi.core;
using NESI.Common.Models;

public partial class sections_member_scheduler_oncall : System.Web.UI.UserControl
	{
	private int lastInsertedAppointmentId;
	public NeMember mymember;
	public Toolbox _tools;
	private static Regex _numeric = new Regex(@"^\d+$");
	public string callback_control;
	public bool can_edit_all;
	protected void Page_Init()
		{
		mymember = Toolbox.do_handle_authentication(OpsPage.OnCallSchedule);
		can_edit_all = mymember.AuthenticatedForPrivilege(OpsPrivilege.EditAllAppointments);
		sqlbranch.SelectCommand = "Select id business_unit_id, ddl_name name from business_unit where id in(" +
								  new Current_User().visible_business_units + ")";
		sqlbranch.DataBind();
		ddlbranch.DataBind();
		if (!Page.IsPostBack)
			{
			ddlbranch.Value = mymember.business_unit_id;
			}
		}
	protected void Page_Load()
		{
		if (!IsPostBack)
			{
			ASPxCalendar2.VisibleDate = new DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, 1);
			ddlbranch.Value = mymember.business_unit_id;
			ASPxGridView1.DataBind();
			ASPxGridView1.Selection.SelectAll();
			Session["oncall_dates"] = null;
			Session["oncall_dates_backup"] = null;
			}
		default_view();
		}
	protected void default_view()
		{
		if (chk_backup.Checked)
			{
			if (Session["oncall_dates_backup"] == null)
				{
				var dt = Toolbox.doSQL_dt(@"Select *,color,member_fullname _name, 
member.member_id member_id from oncall_schedule inner join member on member.member_id = oncall_schedule.member_id and oncall_schedule.business_unit_id =@v0  
where is_backup =@v1", new object[] { ddlbranch.Value, chk_backup.Checked });
				Session["oncall_dates_backup"] = dt;
				}
			}
		else
			{
			if (Session["oncall_dates"] == null)
				{
				var dt = Toolbox.doSQL_dt(@"Select *,color,member_fullname _name, 
member.member_id member_id from oncall_schedule inner join member on member.member_id = oncall_schedule.member_id 
and oncall_schedule.business_unit_id =@v0  where is_backup =@v1",
new object[] { ddlbranch.Value, chk_backup.Checked });
				Session["oncall_dates"] = dt;
				}
			}
		var where = "";
		var keys = ASPxGridView1.GetSelectedFieldValues(new string[] { ASPxGridView1.KeyFieldName });
		for (var i = 0; i < keys.Count; i++)
			{
			where += keys[i] + ",";
			}
		where = "(" + where.TrimEnd(',') + ")";
		ASPxCalendar2.JSProperties["cp_stuff"] = where;
		}
	protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
		{
		if (!chk_backup.Checked)
			Session["oncall_dates"] = null;
		else
			Session["oncall_dates_backup"] = null;
		ASPxGridView1.DataBind();
		ASPxGridView1.Selection.SelectAll();
		default_view();
		ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "RenderCombo", "cb_cal.PerformCallback();", true);
		}
	protected void ddlpm_SelectedIndexChanged(object sender, EventArgs e)
		{
		default_view();
		}
	protected void ASPxCalendar1_DayCellPrepared(object sender, CalendarDayCellPreparedEventArgs e)
		{
		var dt = chk_backup.Checked ? (DataTable)Session["oncall_dates_backup"] : (DataTable)Session["oncall_dates"];
		var x = 0;
		var where = ASPxCalendar2.JSProperties["cp_stuff"].ToString();
		if (where.Length > 2)
			{
			var dr = dt.Select("[date] = '" + e.Date.ToString("yyyy-MM-dd") + "' and [member_id] in " + where);
			foreach (var dr1 in dr)
				{
				e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml(dr1["color"].ToString());
				e.Cell.ToolTip = dr1["_name"].ToString();
				}
			}
		}
	protected void ddl_viewresources_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
	protected void cb_color_change_Callback(object source, CallbackEventArgs e)
		{
		if (e.Parameter.Length > 0)
			{
			if (e.Parameter.Split('|').Length > 1)
				{
				var color = e.Parameter.Split('|').GetValue(1).ToString();
				var key = e.Parameter.Split('|').GetValue(0).ToString();
				var m = new NeMember(Convert.ToInt32(key));
				m.color = color;
				m.save();
				}
			}
		}
	protected void ASPxColorEdit1_Init(object sender, EventArgs e)
		{
		var ce = sender as ASPxColorEdit;
		var container = ce.NamingContainer as GridViewDataItemTemplateContainer;
		ce.ClientSideEvents.ColorChanged = string.Format("function (s, e) {{ cb_color_change.PerformCallback('{0}|' + s.GetColor()); }}", container.KeyValue);
		}
	protected void cb_cal_Callback1(object sender, CallbackEventArgsBase e)
		{
		if (e.Parameter.Length > 0)
			{
			if (e.Parameter.Split('|').Length == 2)
				{
				if (e.Parameter.Split('|').GetValue(0).ToString() == "save")
					{
					var mem = e.Parameter.Split('|').GetValue(1).ToString();
					if (ASPxCalendar2.SelectedDates.Count > 0)
						{
						foreach (var d in ASPxCalendar2.SelectedDates)
							{
							var oc = new NeOncallSchedule
								{
								added_by = mymember.id,
								business_unit_id = (int)ddlbranch.Value,
								date = d,
								member_id = Convert.ToInt32(mem),
								is_backup = chk_backup.Checked
								};
							oc.save();
							if (!chk_backup.Checked)
								Session["oncall_dates"] = null;
							else
								Session["oncall_dates_backup"] = null;
							Session["on_call_schedule"] = null;
							}
						default_view();
						}
					}
				}
			else if (e.Parameter == "clear")
				{
				foreach (var d in ASPxCalendar2.SelectedDates)
					{
					_tools.getSQL_void(@"Delete from oncall_schedule  where business_unit_id =@v0 and date =@v1  and is_backup=@v2  limit 1 ", new object[] { ddlbranch.Value, d.ToString("yyyy-MM-dd"), chk_backup.Checked });
					if (!chk_backup.Checked)
						Session["oncall_dates"] = null;
					else
						Session["oncall_dates_backup"] = null;
					}
				default_view();
				}
			}
		}
	protected void chk_backup_CheckedChanged(object sender, EventArgs e)
		{
		if (chk_backup.Checked)
			{
			Session["oncall_dates_backup"] = null;
			}
		else
			{
			Session["oncall_dates"] = null;
			}
		default_view();
		}
	protected void ASPxGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if (!chk_backup.Checked)
			{
			if (Session["oncall_dates"] != null)
				{
				var dt = (DataTable)Session["oncall_dates"];
				var dr = dt.Select("[member_id] = " + e.KeyValue + " and [date]>'" + Toolbox.MySQLNow_long() + "'");
				if (dr.Length > 0)
					{
					e.Cell.Font.Bold = true;
					}
				}
			}
		else
			{
			if (Session["oncall_dates_backup"] != null)
				{
				var dt = (DataTable)Session["oncall_dates_backup"];
				var dr = dt.Select("[member_id] = " + e.KeyValue + " and [date]>'" + Toolbox.MySQLNow_long() + "'");
				if (dr.Length > 0)
					{
					e.Cell.Font.Bold = true;
					}
				}
			}
		}
	}
