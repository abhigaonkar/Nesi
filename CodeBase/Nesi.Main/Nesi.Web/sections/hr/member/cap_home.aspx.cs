using System;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Data;
using System.Web.UI;
using nesi.core;

public partial class sections_hr_master_cap_home : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 174;
	private const string _page_name = "cap_home";
	private const string _training_log_name = _page_name + "training_log";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	Toolbox _tools;
	private bool can_edit = false;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		can_edit = current_user.AuthenticatedForPrivilege(155);
		LayoutControl.__page_name = _training_log_name;
		LayoutControl.used_gv = gv_training;
		h = (ASPxHiddenField)LayoutControl.FindControl("h");
		ds_templates = (SqlDataSource)LayoutControl.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)LayoutControl.FindControl("dde_filter");
		panel_export = (Panel)LayoutControl.FindControl("panel_export");
		panel_export.Visible = true;
		h.Set("gridview_id", "gv_training");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _training_log_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	    Session["visibleBU"] = new Current_User().visible_business_units;

    }

	protected void Page_Load(object sender, EventArgs e)
	{
		var _q = Request.QueryString;
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
	
		ddl_member.ClientEnabled = current_user.AuthenticatedForPrivilege(155);
		
		if (!IsPostBack)
		{
			Session["sg1"] = null;
			hdnmid.Value = current_user.id.ToString();
			ddl_member.Value = current_user.id;

			ddlmt.DataSource = _tools.getSQL_datatable(@"Select member_offers.id + 10000 id, Concat('Current Employment Agreement') _name, 1 ord from member_offers where member_offers.memberid =@v0 and member_offers.`status` = 'Accepted' UNION Select membertype_id id, membertype_name _name, 2 ord from membertype where active=1 order by ord,_name", new object[] { ddl_member.Value });
			ddlmt.DataBind();
			ddlmt.SelectedIndex = ddlmt.Items.FindByValue(Convert.ToInt32(new NeMember(Convert.ToInt32(hdnmid.Value)).MemberTypeID)).Index;

			var gl = new NeGridLayouts(current_user.id, _training_log_name);
			if (gl.GridLayoutID == 0)
			{
				gl.GridLayout_Layout = gv_training.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _training_log_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			else
			{
				gv_training.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
			}
			dde_filter.Text = gl.GridLayout_Name;

		}
		hdnmid.Value = ddl_member.Value.ToString();
		
		fill_grid();


	}
	protected void fill_grid()
	{
		ddlmt.DataBind();

		#region if its an agreement

		if (Session["sg1"] == null)
		{
			if (ddlmt.SelectedIndex == 0)
			{
				Session["sg1"] = _tools.getSQL_datatable(@"SELECT distinct 0 x, member.Member_ID AS member_id, business_unit.id, member.member_membertype_id membertype_id, Concat(mtr.mt_cr_priority,'. ',core_responsibilities.core_responsibility) AS cr, core_responsibilities.id AS crid, cert1.certificate_name AS cert, cert1.id certid, ifnull(datediff((cert1_history.date,new object[] { ddl_member.Value,interval (cert1.expires) day ), curdate()),0) expires, '' acquire, if((ifnull(datediff((cert1_history.date,interval (cert1.expires) day ), curdate()),0)<=0),1,0) expired_count, if((ifnull(datediff((cert1_history.date,interval (cert1.expires) day ), curdate()),0)<=90)&&(ifnull(datediff((cert1_history.date,interval (cert1.expires) day ), curdate()),0)>0),1,0) almost_expired_count, mtr.mt_cr_priority cr_priority, thh.date training_date FROM member INNER JOIN business_unit ON member.business_unit_id = business_unit.id AND business_unit.id != 8 INNER JOIN membertype ON member.Member_MemberType_ID = membertype.membertype_id AND member.Member_Status = 'Active' LEFT JOIN member_offers _mo on member.member_id = _mo.memberid and _mo.status = 'Accepted' LEFT JOIN memberoffer_cr ON _mo.id = memberoffer_cr.memberoffer_moid inner JOIN core_responsibilities ON memberoffer_cr.memberoffer_crid = core_responsibilities.id and core_responsibilities.status = 'Active' left join membertype_responsibilities mtr on mtr.membertype_id = _mo.membertypeid and mtr.core_responsibility_id = memberoffer_cr.memberoffer_crid LEFT JOIN cr_certificates ON core_responsibilities.id = cr_certificates.cr_id Inner JOIN certificates AS cert1 ON cr_certificates.certificates_id = cert1.id and cert1.`status` = 'Active' LEFT JOIN certificate_history AS cert1_history ON cert1.id = cert1_history.certificate_id and cert1_history.member_id = member.member_id and cert1_history.id = (Select certificate_history.id from certificate_history }  where certificate_history.certificate_id = cert1.id and certificate_history.member_id = member.member_id order by certificate_history.date desc limit 1) LEFT JOIN certificate_training_link as ctl on ctl.certificate_id = cert1.id LEFT JOIN training_header theader on ctl.training_header_id = theader .id LEFT JOIN training_header_history thh on theader.id = thh.training_header_id and thh.member_id = member.member_id and thh.date>curdate() and thh.id = (Select training_header_history.id from training_header_history where training_header_history.member_id = member.Member_ID and training_header_history.training_header_id = theader.id order by training_header_history.date desc limit 1) INNER Join cap_training_schedule cts on theader.id = cts.training_header_id and cts.date>=curdate() WHERE core_responsibilities.core_responsibility != '' and member.member_id =@v0 ORDER BY mtr.mt_cr_priority, crid ASC, cert ASC", new object[] { ddl_member.Value });
			}
		#endregion
			#region if its a membertype comparison
			else
			{
				var mem = new NeMember(Convert.ToInt32(ddl_member.Value));

				Session["sg1"] = _tools.getSQL_datatable(@"SELECT distinct
0 x,
member.Member_ID AS member_id,
member.business_unit_id business_unit_id,
member.member_membertype_id membertype_id,
Concat(mtr.mt_cr_priority,'. ',core_responsibilities.core_responsibility) AS cr,
core_responsibilities.id AS crid,
cert1.certificate_name AS cert,
cert1.id certid,
ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0) expires,
'' acquire,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=0),1,0) expired_count,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=90)&&(ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)>0),1,0) almost_expired_count,
mtr.mt_cr_priority cr_priority,
thh.date training_date

FROM
          member
INNER JOIN business_unit ON member.business_unit_id = business_unit.id
INNER JOIN membertype ON member.Member_MemberType_ID = membertype.membertype_id
AND member.Member_Status = 'Active'
LEFT JOIN member_offers _mo on member.member_id = _mo.memberid and _mo.status = 'Accepted' 
LEFT JOIN memberoffer_cr ON _mo.id = memberoffer_cr.memberoffer_moid
inner JOIN core_responsibilities ON memberoffer_cr.memberoffer_crid = core_responsibilities.id and core_responsibilities.status = 'Active' 
left join membertype_responsibilities mtr on mtr.membertype_id = _mo.membertypeid and mtr.core_responsibility_id = memberoffer_cr.memberoffer_crid
LEFT JOIN cr_certificates ON core_responsibilities.id = cr_certificates.cr_id
Inner JOIN certificates AS cert1 ON cr_certificates.certificates_id = cert1.id and cert1.`status` = 'Active'
LEFT JOIN certificate_history AS cert1_history ON cert1.id = cert1_history.certificate_id and cert1_history.member_id = member.member_id and cert1_history.id = (Select certificate_history.id from certificate_history where certificate_history.certificate_id = cert1.id and certificate_history.member_id = member.member_id order by certificate_history.date desc limit 1)
LEFT JOIN certificate_training_link as ctl on ctl.certificate_id = cert1.id
LEFT JOIN training_header theader on ctl.training_header_id = theader .id
LEFT JOIN training_header_history thh on theader.id = thh.training_header_id and thh.member_id = member.member_id and thh.date>curdate() and thh.id = (Select training_header_history.id from training_header_history where training_header_history.member_id = member.Member_ID and training_header_history.training_header_id = theader.id order by training_header_history.date desc limit 1)
INNER Join cap_training_schedule cts on theader.id = cts.training_header_id and cts.date>=curdate()

WHERE
          core_responsibilities.core_responsibility != '' and member.member_id = @v0
ORDER BY
mtr.mt_cr_priority,
crid ASC,
cert ASC", new object[] { ddl_member.Value });

			}
			#endregion

			var dt = (DataTable)Session["sg1"];
			var x = 0;
			foreach (DataRow dr in dt.Rows)
			{
				dr.BeginEdit();
				dr[0] = x;
				dr.AcceptChanges();
				dr.EndEdit();
				x++;
			}
			Session["sg1"] = dt;

		}
	
		gv.DataSource = Session["sg1"];
		gv.DataBind();
		gv.GroupBy(gv.Columns["cr"]);
		gv.SortBy(gv.Columns["cr_priority"], 0);
		gv.FocusedRowIndex = -1;

	}
	protected void gv_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{

		var rowCount = Convert.ToInt32(gv.GetGroupSummaryValue(e.VisibleIndex, gv.GroupSummary["expired_count"]));
		var rowCount_almost = Convert.ToInt32(gv.GetGroupSummaryValue(e.VisibleIndex, gv.GroupSummary["almost_expired_count"]));
		if (e.RowType == DevExpress.Web.GridViewRowType.Group)
		{
			if (rowCount > 0)
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF6666"); //red

			}
			else if (rowCount_almost > 0)
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF66");  // bright yellow
			}
			else
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");  // white
			}
		}
		else if(e.VisibleIndex >= 0)
		{
			var expires = gv.GetRowValues(e.VisibleIndex, "expires") == DBNull.Value ? 0 : Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "expires"));
			var training_date = gv.GetRowValues(e.VisibleIndex,"training_date") == DBNull.Value ? "" : gv.GetRowValues(e.VisibleIndex,"training_date").ToString();
			if (expires <= 0)
			{

				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFCCCC"); // pink
				if (training_date!="")
				{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFEBEB"); // light pink
				}
			}
			else if (expires <= 90)
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");  // light yellow
			}
			else
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");  // white
			}
		}

	}
	protected void gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex > 0)
		{
			if (e.DataColumn.FieldName == "acquire")
			{
				var cert_id = gv.GetRowValuesByKeyValue(e.KeyValue, "certid").ToString();

				var div_acquire = (ASPxPanel)gv.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "div_acquire");
				var lbl = new ASPxLabel();
				//				HtmlContainerControl d = new HtmlContainerControl();
				DataTable dt;
				//				d.InnerHtml = "<table border-collapse>";

				dt = _tools.getSQL_datatable(@"SELECT training_header_history.id, training_header_history.member_id, if(cts.date is null,'Not Available',concat('Already Enrolled for Training : ',training_header.name,' ',date(cts.date), ' from ',cts.start_time,' to ',cts.end_time,' at ',cts.location, ' Seats Left: ',cts.max_fill-(Select count(training_header_history.id) from training_header_history  where training_header_history.cap_training_schedule_id = cts.id))) schedule_link, cts.id cts_id FROM certificate_training_link INNER JOIN training_header_history ON certificate_training_link.training_header_id = training_header_history.training_header_id INNER JOIN cap_training_schedule cts ON training_header_history.cap_training_schedule_id = cts.id INNER JOIN training_header on training_header_history.training_header_id = training_header.id WHERE certificate_training_link.certificate_id =@v0 AND training_header_history.member_id =@v1  and training_header_history.date > Curdate()", new object[] { cert_id,ddl_member.Value });

				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr in dt.Rows)
					{
						//						lbl = new ASPxLabel();
						//						lbl.Theme = "NETheme01";
						//						lbl.Font.Bold = false;
						//						lbl.Text = dr["schedule_link"].ToString();
						//						div_acquire.Controls.Add(lbl);

						var hl = new ASPxHyperLink();
						hl.Theme = "NETheme01";
						hl.Font.Bold = false;
						hl.Font.Underline = true;
						hl.Text = dr["schedule_link"].ToString();
						hl.Cursor = "pointer";
						//						hl.ClientSideEvents.Click = String.Format("function (s,e) {{gv.PerformCallback('enroll|{0}|{1}');}}", e.KeyValue, dr["cts_id"]);
						hl.ClientSideEvents.Click = string.Format("function (s,e) {{pop_t.Show();pop_t.PerformCallback('review|{0}|{1}');}}", e.KeyValue, dr["cts_id"]);

						div_acquire.Controls.Add(hl);

					}
					return;
				}
				else
				{
					dt = _tools.getSQL_datatable(@"SELECT cts.id cts_id, concat('Training : ',theader.`name`) training_name, if(cts.date is null, if(theader.name is null, if(cert1.how_to_acquire is null or cert1.how_to_acquire = '','Contact Org Dev',cert1.how_to_acquire), concat('No training has been arranged yet')) ,concat('Training Date: ',date(cts.date), ' from ',cts.start_time,' to ',cts.end_time,' at ',cts.location, ' Seats Left: ',cts.max_fill-(Select count(training_header_history.id) from training_header_history  where training_header_history.cap_training_schedule_id = cts.id))) schedule_link from certificates AS cert1 LEFT JOIN certificate_training_link as ctl on ctl.certificate_id = cert1.id LEFT JOIN training_header theader on ctl.training_header_id = theader.id Left Join cap_training_schedule cts on theader.id = cts.training_header_id and cts.date>=curdate() where cert1.`status` = 'Active' and cert1.id =@v0 ORDER BY theader.id ASC", new object[] { cert_id });
					foreach (DataRow dr in dt.Rows)
					{
						if (div_acquire.Controls.Count > 0)
						{
							div_acquire.Controls.Add(new LiteralControl("<br />"));
						}
						if (dr["training_name"].ToString() == "")
						{
							//							d.InnerHtml += "<tr><td>" + dr["schedule_link"] + "</td></tr>";
							lbl = new ASPxLabel();
							lbl.Theme = "NETheme01";
							lbl.Font.Bold = false;
							lbl.Text = dr["schedule_link"].ToString();
							div_acquire.Controls.Add(lbl);
						}
						else
						{

							//		d.InnerHtml += "<tr><td>" + dr["training_name"] + "</td><td>";
							lbl = new ASPxLabel();
							lbl.Theme = "NETheme01";
							lbl.Font.Bold = false;
							lbl.Text = dr["training_name"].ToString();
							div_acquire.Controls.Add(lbl);
							//					
							div_acquire.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;"));
							if (dr["schedule_link"].ToString().Contains("Training Date"))
							{
								var hl = new ASPxHyperLink();
								hl.Theme = "NETheme01";
								hl.Font.Bold = false;
								hl.Font.Underline = true;
								hl.Text = dr["schedule_link"].ToString();
								hl.Cursor = "pointer";
								//								hl.ClientSideEvents.Click = String.Format("function (s,e) {{gv.PerformCallback('enroll|{0}|{1}');}}", e.KeyValue, dr["cts_id"]);
								hl.ClientSideEvents.Click = string.Format("function (s,e) {{pop_t.Show();pop_t.PerformCallback('enroll|{0}|{1}');}}", e.KeyValue, dr["cts_id"]);
								div_acquire.Controls.Add(hl);
							}
							else
							{
								lbl = new ASPxLabel();
								lbl.Theme = "NETheme01";
								lbl.Font.Bold = false;
								lbl.Text = dr["schedule_link"].ToString();
								div_acquire.Controls.Add(lbl);
							}

						}

					}


				}

			}
		}
	}
	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters.Contains("enroll"))
		{
			var cts_id = e.Parameters.Split('|').GetValue(2).ToString();
			var gv_row = e.Parameters.Split('|').GetValue(1).ToString();
			var member_id = gv.GetRowValuesByKeyValue(gv_row, "member_id").ToString();
			var business_unit_id = gv.GetRowValuesByKeyValue(gv_row, "business_unit_id").ToString();
			var membertype_id = gv.GetRowValuesByKeyValue(gv_row, "membertype_id").ToString();
			var cth = new NeCapTraining_History();
			var cts = new NeCapTraining_Schedule(Convert.ToInt32(cts_id));
			cth.member_id = Convert.ToInt32(member_id);
			cth.date = cts.date;
			cth.business_unit_id = Convert.ToInt32(business_unit_id);
			cth.end_time = cts.end_time;
			cth.start_time = cts.start_time;
			cth.training_header_id = cts.training_header_id;
			cth.cap_training_schedule_id = cts.id;
			cth.membertype_id = Convert.ToInt32(membertype_id);
			cth.save();
			try
			{
				var email = new NeEMail();
				email.To = new NeMember(Convert.ToInt32(member_id)).business_unit.branch_manager.NEEmail;
				var org_dev = _tools.getSQL_string(@"select ifnull((Select member_neemail from member  where member_membertype_id = 40 and member_status = 'Active' order by member_id limit 1),'')" , null);
				var trainer = _tools.getSQL_string(@"select ifnull((Select member_neemail from member  where member_membertype_id = 64 and member_status = 'Active' order by member_id limit 1),'')" , null);
				var members_email = new NeMember(Convert.ToInt32(member_id)).NEEmail.Contains("nomail") ? new NeMember(Convert.ToInt32(member_id)).Email : new NeMember(Convert.ToInt32(member_id)).NEEmail;

				email.CC = org_dev + ";" + trainer + ";" + (new NeMember(Convert.ToInt32(member_id)).reports_to != 0 ? new NeMember(new NeMember(Convert.ToInt32(member_id)).reports_to).NEEmail : "");
				email.Subject = "Cap Training Scheduled for " + new NeMember(Convert.ToInt32(member_id)).FullName + " on " + cts.date.ToString("yyyy-MM-dd");
				email.Body = "Cap Training: " + new NeCapTraining(cth.training_header_id).name + System.Environment.NewLine;
				email.Body += "Date: " + cts.date.ToString("yyyy-MM_dd") + System.Environment.NewLine;
				email.Body += "Start Time: " + cts.start_time.Hours + ":" + cts.start_time.Minutes.ToString().PadRight(2, '0') + System.Environment.NewLine;
				email.Body += "End Time: " + cts.end_time.Hours + ":" + cts.end_time.Minutes.ToString().PadRight(2, '0') + System.Environment.NewLine;
				email.Body += "Location: " + cts.location;
				email.From = "admin@" + Toolbox.app_setting("DomainForEmail");
				email.Send();
				NeCapTraining_Schedule.send_calendar_request(cth.id);


			}
			catch { }


			gv.JSProperties["cp_alert"] = "Training Has Been Booked... ";

		}



		Session["sg1"] = null;
		fill_grid();

		gv.JSProperties["cp_refresh"] = "1";
	}
	protected void gv_training_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		if (e.ButtonType == ColumnCommandButtonType.Delete)
		{
			e.Text = "Cancel";
			var d = gv_training.GetRowValues(e.VisibleIndex, "date");
			if (d != null && Convert.ToDateTime(d).Date < System.DateTime.Today)
			{
				e.Visible = false;

			}
		}
	}
	protected void gv_training_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		var member_id = hdnmid.Value;
		var cth = new NeCapTraining_History(Convert.ToInt32(e.Keys[0]));
		var cts = new NeCapTraining_Schedule(Convert.ToInt32(cth.cap_training_schedule_id));
		var members_email = new NeMember(Convert.ToInt32(member_id)).NEEmail.Contains("nomail") ? new NeMember(Convert.ToInt32(member_id)).Email : new NeMember(Convert.ToInt32(member_id)).NEEmail;

		//	_tools.getSQL_void(@"Delete from training_header_history  where id =@v0", new object[] { e.Keys[0] });

		NeCapTraining_Schedule.send_cancel_calendar(cth.id);
		cth.delete(cth.id);
		e.Cancel = true;
		gv_training.CancelEdit();

		gv_training.JSProperties["cp_gv_refresh"] = "1";

	}
	protected void ddlmt_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["sg1"] = null;
		fill_grid();

	}
	protected void ddl_member_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["sg1"] = null;
		hdnmid.Value = ddl_member.Value.ToString();
		fill_grid();

	}
	protected void pop_t_WindowCallback(object source, PopupWindowCallbackArgs e)
	{
		//pop_t.PerformCallback('review|gv ID|training schedule ID')
		//pop_t.PerformCallback('enroll|gv ID|training schedule ID')
		NeCapTraining_Schedule cts;
		if (!e.Parameter.Contains("go") && !e.Parameter.Contains("kill"))
		{
			cts = new NeCapTraining_Schedule(Convert.ToInt32(e.Parameter.Split('|').GetValue(2)));
		}
		else
		{
			cts = new NeCapTraining_Schedule(Convert.ToInt32(e.Parameter.Split('|').GetValue(1)));

		}
		var m = new NeMember(Convert.ToInt32(hdnmid.Value));
		lbl_pop_t_id.Text = cts.id.ToString();
		txt_training_date.Text = cts.date.ToString("yyyy-MM-dd");
		txt_training_end.Text = cts.end_time.ToString("T");
		txt_training_start.Text = cts.start_time.ToString("T");
		txt_training_name.Text = new NeCapTraining(Convert.ToInt32(cts.training_header_id)).name;
		mem_location.Text = cts.location;
		mem_training_notes.Text = new NeCapTraining(Convert.ToInt32(cts.training_header_id)).notes;
		var email = new NeEMail();
		if (e.Parameter.Contains("review"))
		{
			btn_pop_cancel.ClientVisible = true;
			btn_pop_action.Text = "Email Myself";
		}
		else if (e.Parameter.Contains("enroll"))
		{
			btn_pop_cancel.ClientVisible = false;
			btn_pop_action.Text = "Enroll";
		}
		else if (e.Parameter.Contains("kill"))
		{
			var member_id = hdnmid.Value;
			email = new NeEMail();

			var x = _tools.getSQL_int(@"Select id from training_header_history  where training_header_id =@v0 and member_id =@v1  and cap_training_schedule_id =@v2  limit 1 ", new object[] { cts.training_header_id,hdnmid.Value,cts.id });
			var cth = new NeCapTraining_History(x);
			email.To = m.business_unit.branch_manager.NEEmail;
			var org_dev = _tools.getSQL_string(@"select ifnull((Select member_neemail from member  where member_membertype_id = 40 and member_status = 'Active' order by member_id limit 1),'')" , null);
			var trainer = _tools.getSQL_string(@"select ifnull((Select member_neemail from member  where member_membertype_id = 64 and member_status = 'Active' order by member_id limit 1),'')" , null);
			email.CC = org_dev + ";" + trainer + ";" + (m.reports_to != 0 ? new NeMember(m.reports_to).NEEmail : "");
			email.Subject = "Cap Training Scheduled for " + m.FullName + " on " + cts.date.ToString("yyyy-MM-dd") + " was just cancelled by " + current_user.FullName;
			email.Body = "Cap Training: " + new NeCapTraining(cts.training_header_id).name + System.Environment.NewLine;
			email.Body += "Date: " + cts.date.ToString("yyyy-MM_dd") + System.Environment.NewLine;
			email.Body += "Start Time: " + cts.start_time.Hours + ":" + cts.start_time.Minutes.ToString().PadRight(2, '0') + System.Environment.NewLine;
			email.Body += "End Time: " + cts.end_time.Hours + ":" + cts.end_time.Minutes.ToString().PadRight(2, '0') + System.Environment.NewLine;
			email.Body += "Location: " + cts.location;
			email.From = "admin@" + Toolbox.app_setting("DomainForEmail");
			email.Send();
	
			
			NeCapTraining_Schedule.send_cancel_calendar(cth.id);
			cth.delete(cth.id);
	//		gv_training.JSProperties["cp_gv_refresh"] = "1";
			pop_t.JSProperties["cp_alert"] = "You have removed yourself from the training";
			pop_t.JSProperties["cp_after"] = "";
		}
		else if (e.Parameter.Contains("go"))
		{
			
			try
			{
				email = new NeEMail();
				var members_email = m.NEEmail.Contains("nomail") ? m.Email : m.NEEmail;
				NeCapTraining_History cth;
				if (e.Parameter.Split('|').GetValue(2).ToString() == "Enroll")
				{
					cth = new NeCapTraining_History();

					cth.member_id = m.id;
					cth.date = cts.date;
					cth.business_unit_id = (int)m.business_unit_id;
					cth.end_time = cts.end_time;
					cth.start_time = cts.start_time;
					cth.training_header_id = cts.training_header_id;
					cth.cap_training_schedule_id = cts.id;
					cth.membertype_id = (int)m.MemberTypeID;
					cth.save();

					email.To = m.business_unit.branch_manager.NEEmail;
					var org_dev = _tools.getSQL_string(@"select ifnull((Select member_neemail from member  where member_membertype_id = 40 and member_status = 'Active' order by member_id limit 1),'')" , null);
					var trainer = _tools.getSQL_string(@"select ifnull((Select member_neemail from member  where member_membertype_id = 64 and member_status = 'Active' order by member_id limit 1),'')" , null);
					email.CC = org_dev + ";" + trainer + ";" + (m.reports_to != 0 ? new NeMember(m.reports_to).NEEmail : "");
					pop_t.JSProperties["cp_alert"] = "You have enrolled for the training";
					email.Subject = "Cap Training Scheduled for " + m.FullName + " on " + cts.date.ToString("yyyy-MM-dd");
					email.Body = "Cap Training: " + new NeCapTraining(cts.training_header_id).name + System.Environment.NewLine;
					email.Body += "Date: " + cts.date.ToString("yyyy-MM_dd") + System.Environment.NewLine;
					email.Body += "Start Time: " + cts.start_time.Hours + ":" + cts.start_time.Minutes.ToString().PadRight(2, '0') + System.Environment.NewLine;
					email.Body += "End Time: " + cts.end_time.Hours + ":" + cts.end_time.Minutes.ToString().PadRight(2, '0') + System.Environment.NewLine;
					email.Body += "Location: " + cts.location;
					email.From = "admin@" + Toolbox.app_setting("DomainForEmail");
					email.Send();
					NeCapTraining_Schedule.send_calendar_request(cth.id);
				}
				else
				{
					try
					{
						
						var x = _tools.getSQL_int(@"Select id from training_header_history  where training_header_id =@v0 and member_id =@v1  and cap_training_schedule_id =@v2  limit 1 ", new object[] { cts.training_header_id,hdnmid.Value,cts.id });
						cth = new NeCapTraining_History(x);
						NeCapTraining_Schedule.send_calendar_request(cth.id);
						pop_t.JSProperties["cp_alert"] = "Email sent";
					}
					catch { }
				}


				
				pop_t.JSProperties["cp_after"] = "";
			}
			catch
			{

				pop_t.JSProperties["cp_alert"] = "Something went wrong!";
			}

		}
	}
	protected void gv_training_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_training_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters == "")
		{
			gv_training.FilterExpression = "";
			for (var i = 0; i < gv_training.Columns.Count; i++)
			{
				if (gv_training.Columns[i] is GridViewDataColumn)
				{
					var col = (GridViewDataColumn)gv_training.Columns[i];
					if (col.GroupIndex > -1)
					{
						gv_training.UnGroup(col);
					}
					col.Visible = true;
				}
			}
		}
	}
}

	

	