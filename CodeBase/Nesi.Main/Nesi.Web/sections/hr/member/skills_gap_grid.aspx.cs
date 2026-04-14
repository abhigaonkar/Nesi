using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Data;
using nesi.core;

public partial class sections_hr_master_skills_gap_grid : System.Web.UI.Page
{
	NeMember current_user;
	private int page_id = 156;
	private const string _page_name = "skillsgap";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	Toolbox _tools;
	static string default_filter = "";
	private bool can_edit = false;
	protected void page_init(object sender, EventArgs e)
	{
_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(page_id);
		can_edit = current_user.AuthenticatedForPrivilege(155);
layout.__page_name = _page_name;
default_filter = "page1|group6|sort6|a2|a3|a20|a4|a6|a14|conditions9|1|3|2|3|3|3|4|3|5|3|8|3|13|9|16|3|20|7|visible22|f8|f2|t3|t1|t9|t15|t0|f19|f18|t14|t13|t16|f17|t10|t7|t6|t4|t5|t12|t11|t22|f23|width22|50px|92px|203px|141px|316px|99px|400px|110px|108px|95px|82px|98px|111px|154px|176px|159px|447px|222px|92px|107px|90px|162px";

		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.used_gv = cb;
		h.Set("gridview_id", "cb");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		
		
		var _q = Request.QueryString;
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = new NePage().get_page_desc(Convert.ToInt32(page_id));
		var menu = new NeMenu(current_user, Convert.ToInt32(page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);

	
		if (!IsPostBack)
		{
			chk_mt.Checked = true;
				var gl = new NeGridLayouts(current_user.id, _page_name);
				if (gl.GridLayoutID == 0)
				{
					cb.LoadClientLayout(default_filter);
					gl.GridLayout_Layout = default_filter;
					gl.GridLayout_Layout = cb.SaveClientLayout();
					gl.member_id = current_user.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					cb.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				dde_filter.Text = gl.GridLayout_Name;
			
			Session["tl"] = null;
		}

		filltree();


	}
	
	protected void filltree()
	{
		if (Session["tl"] == null)
		{
			if (can_edit)
			{
				if (chk_mt.Checked)
				{
					Session["tl"] = _tools.getSQL_datatable(@"SELECT
(select @rownum:=@rownum+1) x,
member.Member_ID AS memberid,
Concat(
                    member.member_nickname,
                    ' ',
                    member.Member_LastName
          ) AS `name`,
business_unit.name AS branch,
membertype.membertype_name AS mt,
core_responsibilities.core_responsibility AS cr,
'' AS cert,
core_responsibilities.id AS crid,
cert1.certificate_name AS cert_req,
cert1.is_internal cert_is_internal,
cert1_history.date cert_date,
cert1_history.score cert_score,
cert1_history.date + interval (cert1.expires) day cert_exp_date,
cert1.id cert_id,
cert1_history.id cert_historyid,
ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0) cert_exp,
theader.id training_id,
if(theader.`name`=null,'',Concat('Training: ',theader.`name`)) training_name,

thh.date training_date,
cert1.how_to_acquire acquire,
date(cts.date) pos_training_date,
cts.location cert_location,
cts.max_fill max_fill,
(Select count(training_header_history.id) from training_header_history where training_header_history.cap_training_schedule_id = cts.id) enrolled,
ifnull(mt_c.mt_cr_priority,11) _mt_cr_priority,
if(ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)=0,1,0) is_expired,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=0),1,0) expired_count,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=90)&&(ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)>0),1,0) almost_expired_count,
business_unit.id business_unit_id
FROM
          member
INNER join business_unit ON member.business_unit_id = business_unit.id
AND business_unit.id != 8
INNER JOIN membertype ON member.Member_MemberType_ID = membertype.membertype_id
AND member.Member_Status = 'Active'
LEFT JOIN member_offers _mo on member.member_id = _mo.memberid and _mo.status = 'Accepted' 
LEFT JOIN memberoffer_cr ON _mo.id = memberoffer_cr.memberoffer_moid
inner JOIN core_responsibilities ON memberoffer_cr.memberoffer_crid = core_responsibilities.id and core_responsibilities.status = 'Active' 
inner join membertype_responsibilities mt_c on core_responsibilities.id = mt_c.core_responsibility_id and mt_c.membertype_id = membertype.membertype_id
LEFT JOIN cr_certificates ON core_responsibilities.id = cr_certificates.cr_id
Inner JOIN certificates AS cert1 ON cr_certificates.certificates_id = cert1.id and cert1.`status` = 'Active'
LEFT JOIN certificate_history AS cert1_history ON cert1.id = cert1_history.certificate_id and cert1_history.member_id = member.member_id and cert1_history.id = (Select certificate_history.id from certificate_history where certificate_history.certificate_id = cert1.id and certificate_history.member_id = member.member_id order by certificate_history.date desc limit 1)
LEFT JOIN certificate_training_link as ctl on ctl.certificate_id = cert1.id
LEFT JOIN training_header theader on ctl.training_header_id = theader .id
LEFT JOIN training_header_history thh on theader.id = thh.training_header_id and thh.member_id = member.member_id and thh.date>curdate() and thh.id = (Select training_header_history.id from training_header_history where training_header_history.member_id = member.Member_ID and training_header_history.training_header_id = theader.id order by training_header_history.date desc limit 1)
Left Join cap_training_schedule cts on theader.id = cts.training_header_id and cts.date>=curdate(),
(SELECT @rownum:=0) v
WHERE
          core_responsibilities.core_responsibility != '' 
ORDER BY
crid ASC,
cert_req ASC", null);
				}
				else
				{
					Session["tl"] = _tools.getSQL_datatable(@"SELECT
(select @rownum:=@rownum+1) x,
member.Member_ID AS memberid,
Concat(
                    member.member_nickname,
                    ' ',
                    member.Member_LastName
          ) AS `name`,
business_unit.name AS branch,
membertype.membertype_name AS mt,
core_responsibilities.core_responsibility AS cr,
'' AS cert,
core_responsibilities.id AS crid,
cert1.certificate_name AS cert_req,
cert1.is_internal cert_is_internal,
cert1_history.date cert_date,
cert1_history.score cert_score,
cert1_history.date + interval (cert1.expires) day cert_exp_date,
cert1.id cert_id,
cert1_history.id cert_historyid,
ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0) cert_exp,
theader.id training_id,
if(theader.`name`=null,'',Concat('Training: ',theader.`name`)) training_name,

thh.date training_date,
cert1.how_to_acquire acquire,
date(cts.date) pos_training_date,
cts.location cert_location,
cts.max_fill max_fill,
(Select count(training_header_history.id) from training_header_history where training_header_history.cap_training_schedule_id = cts.id) enrolled,
ifnull(mt_c.mt_cr_priority,11) _mt_cr_priority,
if(ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)=0,1,0) is_expired,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=0),1,0) expired_count,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=90)&&(ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)>0),1,0) almost_expired_count,
business_unit.id business_unit_id


FROM
          member
INNER join business_unit ON member.business_unit_id = business_unit.id AND business_unit.id != 8
INNER JOIN membertype ON member.Member_MemberType_ID = membertype.membertype_id AND member.Member_Status = 'Active'
inner join membertype_responsibilities mt_c on mt_c.membertype_id = membertype.membertype_id 
inner JOIN core_responsibilities ON core_responsibilities.id = mt_c.core_responsibility_id  and core_responsibilities.status = 'Active'
LEFT JOIN cr_certificates ON core_responsibilities.id = cr_certificates.cr_id
Inner JOIN certificates AS cert1 ON cr_certificates.certificates_id = cert1.id and cert1.`status` = 'Active'
LEFT JOIN certificate_history AS cert1_history ON cert1.id = cert1_history.certificate_id and cert1_history.member_id = member.member_id and cert1_history.id = (Select certificate_history.id from certificate_history where certificate_history.certificate_id = cert1.id and certificate_history.member_id = member.member_id order by certificate_history.date desc limit 1)
LEFT JOIN certificate_training_link as ctl on ctl.certificate_id = cert1.id
LEFT JOIN training_header theader on ctl.training_header_id = theader .id
LEFT JOIN training_header_history thh on theader.id = thh.training_header_id and thh.member_id = member.member_id and thh.date>curdate() and thh.id = (Select training_header_history.id from training_header_history where training_header_history.member_id = member.Member_ID and training_header_history.training_header_id = theader.id order by training_header_history.date desc limit 1)
Left Join cap_training_schedule cts on theader.id = cts.training_header_id and cts.date>=curdate(),
(SELECT @rownum:=0) v
WHERE
          core_responsibilities.core_responsibility != '' 
ORDER BY
crid ASC,
cert_req ASC", null);
				}

			}
			else
			{
				var all_reports = NeMember.get_allreports(current_user.id);
				var member_str = "";
				foreach (DataRow ar in all_reports.Rows)
				{
					member_str += " or member.member_id = " + ar["member_id"] ;
				}

				if (chk_mt.Checked)
				{
					Session["tl"] = _tools.getSQL_datatable(@"SELECT
(select @rownum:=@rownum+1) x,
member.Member_ID AS memberid,
Concat(
                    member.member_nickname,
                    ' ',
                    member.Member_LastName
          ) AS `name`,
business_unit.name AS branch,
membertype.membertype_name AS mt,
core_responsibilities.core_responsibility AS cr,
'' AS cert,
core_responsibilities.id AS crid,
cert1.certificate_name AS cert_req,
cert1.is_internal cert_is_internal,
cert1_history.date cert_date,
cert1_history.score cert_score,
cert1_history.date + interval (cert1.expires) day cert_exp_date,
cert1.id cert_id,
cert1_history.id cert_historyid,
ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0) cert_exp,
theader.id training_id,
if(theader.`name`=null,'',Concat('Training: ',theader.`name`)) training_name,

thh.date training_date,
cert1.how_to_acquire acquire,
date(cts.date) pos_training_date,
cts.location cert_location,
cts.max_fill max_fill,
(Select count(training_header_history.id) from training_header_history where training_header_history.cap_training_schedule_id = cts.id) enrolled,
ifnull(mt_c.mt_cr_priority,11) _mt_cr_priority,
if(ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)=0,1,0) is_expired,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=0),1,0) expired_count,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=90)&&(ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)>0),1,0) almost_expired_count,
business_unit.id business_unit_id
FROM
          member
INNER join business_unit ON member.business_unit_id = business_unit.id
AND business_unit.id != 8
INNER JOIN membertype ON member.Member_MemberType_ID = membertype.membertype_id
AND member.Member_Status = 'Active'
LEFT JOIN member_offers _mo on member.member_id = _mo.memberid and _mo.status = 'Accepted' 
LEFT JOIN memberoffer_cr ON _mo.id = memberoffer_cr.memberoffer_moid
inner JOIN core_responsibilities ON memberoffer_cr.memberoffer_crid = core_responsibilities.id and core_responsibilities.status = 'Active'
inner join membertype_responsibilities mt_c on core_responsibilities.id = mt_c.core_responsibility_id and mt_c.membertype_id = membertype.membertype_id 
LEFT JOIN cr_certificates ON core_responsibilities.id = cr_certificates.cr_id
Inner JOIN certificates AS cert1 ON cr_certificates.certificates_id = cert1.id and cert1.`status` = 'Active'
LEFT JOIN certificate_history AS cert1_history ON cert1.id = cert1_history.certificate_id and cert1_history.member_id = member.member_id and cert1_history.id = (Select certificate_history.id from certificate_history where certificate_history.certificate_id = cert1.id and certificate_history.member_id = member.member_id order by certificate_history.date desc limit 1)
LEFT JOIN certificate_training_link as ctl on ctl.certificate_id = cert1.id
LEFT JOIN training_header theader on ctl.training_header_id = theader .id
LEFT JOIN training_header_history thh on theader.id = thh.training_header_id and thh.member_id = member.member_id and thh.date>curdate() and thh.id = (Select training_header_history.id from training_header_history where training_header_history.member_id = member.Member_ID and training_header_history.training_header_id = theader.id order by training_header_history.date desc limit 1)
Left Join cap_training_schedule cts on theader.id = cts.training_header_id and cts.date>=curdate(),
(SELECT @rownum:=0) v
WHERE
          core_responsibilities.core_responsibility != '' and 
(
member.member_id = " + current_user.id + member_str + @"
)
ORDER BY
crid ASC,
cert_req ASC",null);
				}
				else
				{
					Session["tl"] = _tools.getSQL_datatable(@"SELECT
(select @rownum:=@rownum+1) x,
member.Member_ID AS memberid,
Concat(
                    member.member_nickname,
                    ' ',
                    member.Member_LastName
          ) AS `name`,
business_unit.name AS branch,
membertype.membertype_name AS mt,
core_responsibilities.core_responsibility AS cr,
'' AS cert,
core_responsibilities.id AS crid,
cert1.certificate_name AS cert_req,
cert1.is_internal cert_is_internal,
cert1_history.date cert_date,
cert1_history.score cert_score,
cert1_history.date + interval (cert1.expires) day cert_exp_date,
cert1.id cert_id,
cert1_history.id cert_historyid,
ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0) cert_exp,
theader.id training_id,
if(theader.`name`=null,'',Concat('Training: ',theader.`name`)) training_name,

thh.date training_date,
cert1.how_to_acquire acquire,
date(cts.date) pos_training_date,
cts.location cert_location,
cts.max_fill max_fill,
(Select count(training_header_history.id) from training_header_history where training_header_history.cap_training_schedule_id = cts.id) enrolled,
ifnull(mt_c.mt_cr_priority,11) _mt_cr_priority,
if(ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)=0,1,0) is_expired,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=0),1,0) expired_count,
if((ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)<=90)&&(ifnull(datediff((cert1_history.date + interval (cert1.expires) day ), curdate()),0)>0),1,0) almost_expired_count,
business_unit.id business_unit_id
FROM
          member
INNER join business_unit ON member.business_unit_id = business_unit.id
AND business_unit.id != 8
INNER JOIN membertype ON member.Member_MemberType_ID = membertype.membertype_id
AND member.Member_Status = 'Active'
inner join membertype_responsibilities mt_c on mt_c.membertype_id = membertype.membertype_id 
inner JOIN core_responsibilities ON core_responsibilities.id = mt_c.core_responsibility_id  and core_responsibilities.status = 'Active'

LEFT JOIN cr_certificates ON core_responsibilities.id = cr_certificates.cr_id
Inner JOIN certificates AS cert1 ON cr_certificates.certificates_id = cert1.id and cert1.`status` = 'Active'
LEFT JOIN certificate_history AS cert1_history ON cert1.id = cert1_history.certificate_id and cert1_history.member_id = member.member_id and cert1_history.id = (Select certificate_history.id from certificate_history where certificate_history.certificate_id = cert1.id and certificate_history.member_id = member.member_id order by certificate_history.date desc limit 1)
LEFT JOIN certificate_training_link as ctl on ctl.certificate_id = cert1.id
LEFT JOIN training_header theader on ctl.training_header_id = theader .id
LEFT JOIN training_header_history thh on theader.id = thh.training_header_id and thh.member_id = member.member_id and thh.date>curdate() and thh.id = (Select training_header_history.id from training_header_history where training_header_history.member_id = member.Member_ID and training_header_history.training_header_id = theader.id order by training_header_history.date desc limit 1)
Left Join cap_training_schedule cts on theader.id = cts.training_header_id and cts.date>=curdate(),
(SELECT @rownum:=0) v
WHERE
          core_responsibilities.core_responsibility != '' and 
(
member.member_id = " + current_user.id + member_str + @"
)
ORDER BY
crid ASC,
cert_req ASC",null);

				}

			}

	//	ASPxGridView1.GroupBy(ASPxGridView1.Columns["branch"]);
	//	ASPxGridView1.GroupBy(ASPxGridView1.Columns["mt"]);
	//	ASPxGridView1.GroupBy(ASPxGridView1.Columns["name"]);
	//	ASPxGridView1.GroupBy(ASPxGridView1.Columns["cr"]);
			cb.DataSource = Session["tl"];
			cb.DataBind();
	//		cb.Columns["branch"].Visible = false;
	//		cb.Columns["name"].Visible = false;
	//		cb.Columns["mt"].Visible = false;
			
	//		cb.Columns[0].Visible = false;
	//		cb.GroupBy(cb.Columns["cr"]);
	//		cb.GroupBy(cb.Columns["cert_req"]);
			
			cb.Columns["cert_req"].Width = Unit.Pixel(400);

		}
		else
		{
	cb.DataSource = Session["tl"];
		cb.DataBind();
		}

	//	tl.DataSource = Session["tl"];
	//	tl.DataBind();
	//	tl.CollapseAll();
		
	

		
		
	}
	
	protected void ASPxTextBox1_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxTextBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_review.PerformCallback('{0}|' + s.GetValue()+ '|q'); }}", container.KeyValue);

	}
	protected void ASPxComboBox2_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ gv_review.PerformCallback('{0}|' + s.GetValue()+ '|s'); }}", container.KeyValue);

	}

	protected void tl_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
	{

	}
	
	protected void ASPxGridView1_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		var rowCount = Convert.ToInt32(cb.GetGroupSummaryValue(e.VisibleIndex, cb.GroupSummary["expired_count"]));
		var rowCount_almost = Convert.ToInt32(cb.GetGroupSummaryValue(e.VisibleIndex, cb.GroupSummary["almost_expired_count"]));
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
		else
		{
			var expires = cb.GetRowValues(e.VisibleIndex, "cert_exp") == DBNull.Value ? 0 : Convert.ToInt32(cb.GetRowValues(e.VisibleIndex, "cert_exp"));
			if (expires <= 0)
			{
				e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFCCCC"); // pink
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
/*

		if (e.RowType == GridViewRowType.Group)
		{
			int expires = cb.GetRowValues(e.VisibleIndex, "cert_exp") == DBNull.Value ? 0 : Convert.ToInt32(cb.GetRowValues(e.VisibleIndex, "cert_exp"));

			if (expires <= 90)
			{
				e.Row.BackColor = System.Drawing.Color.Red;
			}
			else
			{
				e.Row.BackColor = System.Drawing.Color.Green;
			}
		}
 */
	}
	protected void ASPxGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
	/*		#region cert
			if (e.DataColumn.FieldName == "cert")
			{

				GridViewDataColumn workorder_column = (GridViewDataColumn)cb.Columns["cert"];
				HtmlContainerControl div = (HtmlContainerControl)cb.FindRowCellTemplateControl(e.VisibleIndex, workorder_column, "div_cert");
				int memberid = Convert.ToInt32(cb.GetRowValues(e.VisibleIndex, "memberid"));
				DataTable dt = _tools.get_datatable(@"SELECT
cert1.certificate_name AS cert1,
cert2.certificate_name AS cert2,
cert1_history.date cert1_history_date,
cert1_history.notes cert1_history_notes,
cert1_history.score cert1_history_score,
cert2_history.date cert2_history_date,
cert2_history.notes cert2_history_notes,
cert2_history.score cert2_history_score,
cert1_history.date + interval (cert1.expires) day cert1_expires,
cert1.notes cert1_notes,
cert2_history.date + interval (cert2.expires) day cert2_expires,
cert2.notes cert2_notes
FROM
certificate_skill_link
LEFT JOIN certificates AS cert1 ON certificate_skill_link.certificate_id = cert1.id
LEFT JOIN certificates AS cert2 ON certificate_skill_link.alternate_certificate_id = cert2.id
LEFT JOIN certificate_history AS cert1_history ON cert1.id = cert1_history.certificate_id and cert1_history.member_id = " + memberid + @"
LEFT JOIN certificate_history AS cert2_history ON cert2.id = cert2_history.certificate_id and cert2_history.member_id = " + memberid + @"
WHERE
certificate_skill_link.skill_id = " + skillid + " order by cert1.id,(cert1_history.date + interval (cert1.expires) day) desc,(cert2_history.date + interval (cert2.expires) day) desc");
				if (dt.Rows.Count > 0)
				{
				DateTime date1 = new DateTime(1);
						DateTime date2 = new DateTime(1);
	string temp = "";
	string cert = dt.Rows[0]["cert1"].ToString() ;
					div.InnerHtml = "<table style='padding: 2px 10px 2px 10px; font-family: Arial' cellpadding='0'cellspacing='0' width='700px''><tr><th align='left'>Certificate Required</th><th>Expiry Date</th><th align='left'>Alternate Certification</th><th>Expiry Date</th></tr>";
					foreach (DataRow dr in dt.Rows)
					{
						if (dr["cert1"].ToString() != cert)
						{
							if (temp != "")
							{
								div.InnerHtml += temp;
							}
							else
							{
								div.InnerHtml += "<tr><td>" + dr["cert1"] + "</td><td align='center'></td><td>" + dr["cert2"] + "</td><td  align='center'></td></tr>";
							}
							cert = dr["cert1"].ToString();
						}
						
						if ((dr["cert1_expires"] != DBNull.Value ? Convert.ToDateTime(dr["cert1_expires"]) : new DateTime(1)) > date1)
						{
							date1 = Convert.ToDateTime(dr["cert1_expires"]);
							string temp_date = dr["cert2_expires"] != DBNull.Value ? Convert.ToDateTime(dr["cert2_expires"]).ToString("yyyy-MM-dd") : "";
							temp = "<tr><td>" + dr["cert1"] + "</td><td align='center'>" + date1.ToString("yyyy-MM-dd") + "</td><td>" + dr["cert2"] + "</td><td  align='center'>" + temp_date + "</td></tr>";

						}

						if ((dr["cert2_expires"] != DBNull.Value ? Convert.ToDateTime(dr["cert2_expires"]) : new DateTime(1)) > date2)
						{
							date2 = Convert.ToDateTime(dr["cert2_expires"]);
							string temp_date2 = dr["cert1_expires"] != DBNull.Value ? Convert.ToDateTime(dr["cert1_expires"]).ToString("yyyy-MM-dd") : "";
							if (date2 > date1)
							{
								temp = "<tr><td>" + dr["cert1"] + "</td><td align='center'>" + temp_date2 + "</td><td>" + dr["cert2"] + "</td><td  align='center'>" + date2 + "</td></tr>";
							}
						}
								
						
					}
					if (temp != "")
					{
						div.InnerHtml += temp;
					}
					else
					{
						div.InnerHtml += "<tr><td>" + cert + "</td><td align='center'></td><td>" + dt.Rows[0]["cert2"].ToString() + "</td><td  align='center'></td></tr>";
					}
					div.InnerHtml += "</table>";
				}
			}
			#endregion
			*/
			

		}
	}
	protected void ASPxGridView1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		
			var gv				= (ASPxGridView) sender;

			if (e.Parameters != "")
			{
				
				if (e.Parameters[0].ToString() == "x")
				{
					if (!can_edit)
					{
						throw new Exception("Not Authorized");
					}
					try
					{
						var p = e.Parameters.Split('|');
						var id = p[1];
						var data = p[2];
						if ((p[0] == "xad")||(p[0]=="xcd"))
						{
							_tools.getSQL_void(@"Update certificate_history set date=@v0  where id =@v1" ,
								new object[] {
								Convert.ToDateTime(p[2]).ToString("yyyy-MM-dd"), id
									});
						}
						else if ((p[0] == "xas") || (p[0] == "xcs"))
						{
							_tools.getSQL_void("Update certificate_history set score=@v0 where id =@v1",
								new object[] {
									Convert.ToInt32(p[2]), id
								});
					}
						else if ((p[0] == "xcei") || (p[0] == "xaei"))
						{
							_tools.getSQL_void(@"Update certificate_history set external_id=@v0 where id =@v1",
								new object[] {
									p[2], id
								});
					}

					}
					catch { throw new Exception("something is busted in the under workings"); }
					Session["tl"] = null;
					filltree();
				}
				else
				{
						gv.LoadClientLayout(e.Parameters);
				}
			}
			else
			{
				gv.FilterExpression = "";
				for (var i = 0; i < gv.Columns.Count; i++)
				{
					if (gv.Columns[i] is GridViewDataColumn)
					{
						var col = (GridViewDataColumn)gv.Columns[i];
						if (col.GroupIndex > -1)
						{
							gv.UnGroup(col);
						}
						col.Visible = true;
					}
				}
			}
			
	}
	protected void ASPxGridView1_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	

	protected void banc_Init(object sender, EventArgs e)
	{
		var b = sender as ASPxHyperLink;
		var container = b.NamingContainer as GridViewDataItemTemplateContainer;
		var cert_id = container.Grid.GetRowValues(container.VisibleIndex, "cert_id").ToString();
		var memberid = container.Grid.GetRowValues(container.VisibleIndex, "memberid").ToString();
		var mem = container.Grid.GetRowValues(container.VisibleIndex, "name").ToString();
		var cert = container.Grid.GetRowValues(container.VisibleIndex, "cert_req").ToString();
		var location = container.Grid.GetRowValues(container.VisibleIndex, "cert_location").ToString();
//		if (container.Grid.GetRowValues(container.VisibleIndex, "training_hl").ToString() == "Up To Date")
//		{
//			b.ClientEnabled = false;
//			b.Cursor = "";
//		}
		b.ClientSideEvents.Click = string.Format(@"function (s, e) {{ pop_schedule.Show(); 
																	  lblcert0.SetText('{0}');
																	  lblmember0.SetText('{1}');
																	   txtcrid0.SetText('{2}');
																	  txtmemid0.SetText('{3}');
lbllocation.SetText('{4}');
	ddl_training.PerformCallback();

	}}", cert,mem,cert_id,memberid,location.Replace("'","`").Replace('"','`'));
	}

	protected void cd_Init1(object sender, EventArgs e)
	{
		var cd = sender as ASPxDateEdit;
		var container = cd.NamingContainer as GridViewDataItemTemplateContainer;
		var cert_historyid = container.Grid.GetRowValues(container.VisibleIndex, "cert_historyid").ToString();
		cd.ClientSideEvents.DateChanged = string.Format("function (s, e) {{ cb.PerformCallback('xcd|{0}|' + s.GetText()); }}",cert_historyid);

	}
	protected void ad_Init(object sender, EventArgs e)
	{
		var cd = sender as ASPxDateEdit;
		var container = cd.NamingContainer as GridViewDataItemTemplateContainer;
		var cert_historyid = container.Grid.GetRowValues(container.VisibleIndex, "alt_historyid").ToString();
		cd.ClientSideEvents.DateChanged = string.Format("function (s, e) {{ cb.PerformCallback('xad|{0}|' + s.GetText()); }}", cert_historyid);
	}
	protected void tbacs_Init(object sender, EventArgs e)
	{
		var cd = sender as ASPxTextBox;
		var container = cd.NamingContainer as GridViewDataItemTemplateContainer;
	var cert_historyid = container.Grid.GetRowValues(container.VisibleIndex, "alt_historyid").ToString();
		cd.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb.PerformCallback('xas|{0}|' + s.GetText()); }}", cert_historyid);
	}
	protected void tbcs_Init(object sender, EventArgs e)
	{
		var cd = sender as ASPxTextBox;
		var container = cd.NamingContainer as GridViewDataItemTemplateContainer;
	var cert_historyid = container.Grid.GetRowValues(container.VisibleIndex, "cert_historyid").ToString();
		cd.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb.PerformCallback('xcs|{0}|' + s.GetText()); }}", cert_historyid);
	}
	protected void tbcei_Init(object sender, EventArgs e)
	{
		var cd = sender as ASPxTextBox;
		var container = cd.NamingContainer as GridViewDataItemTemplateContainer;
	var cert_historyid = container.Grid.GetRowValues(container.VisibleIndex, "cert_historyid").ToString();
		cd.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb.PerformCallback('xcei|{0}|' + s.GetText()); }}", cert_historyid);
	}
	protected void tbaei_Init(object sender, EventArgs e)
	{
		var cd = sender as ASPxTextBox;
		var container = cd.NamingContainer as GridViewDataItemTemplateContainer;
		var cert_historyid = container.Grid.GetRowValues(container.VisibleIndex, "alt_historyid").ToString();
		cd.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb.PerformCallback('xaei|{0}|' + s.GetText()); }}", cert_historyid);
	}

	protected void cbb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		if (!can_edit)
		{
			throw new Exception("You Not Authorized to Issue Certifications");
		}
		if (dtecert.Text == "")
		{
			throw new Exception("Date is missing");
		}
object score = null;
		try
		{
			 score = txtscore.Text == "" ? "0" : Convert.ToInt32(txtscore.Text).ToString();
		}
		catch
		{
			throw new Exception("Illegal number in the score.. must be an integer between 0 and 100 or empty");
		}
		try
		{
			var fieldValues = cb.GetSelectedFieldValues(new string[] { "cert_id", "memberid","cert_date","business_unit_id" });
			
			foreach (object[] item in fieldValues)
			{
				if (_tools.getSQL_int(@"Select count(id) from certificate_history  where certificate_id =@v0 and member_id =@v1  and date(date) =@v2 ", 
					new object[] { item[0].ToString(),item[1].ToString(),dtecert.Date.ToString("yyyy-MM-dd") }) == 0)
				{
					_tools.getSQL_void(@"Insert into certificate_history
(certificate_id,member_id,business_unit_id,date,score,external_id,notes) 
values(@v0,@v1,@v2,@v3,@v4,@v5,@v6)", new object[] {
						item[0].ToString(),
					item[1].ToString(),
					item[3].ToString(),
					dtecert.Date.ToString("yyyy-MM-dd"),
					score,
					extid.Text,

					memnotes.Text });
				}
			}

			
			txtscore.Text = "";
			extid.Text = "";
		
			memnotes.Text = "";
			cbb.JSProperties["cp_close"] = "close";
		}
		catch (Exception ee)
		{
			throw new Exception("Couldn't add new certificate date");
		}
	}
	protected void pop_PopupWindowCommand(object source, DevExpress.Web.PopupControlCommandEventArgs e)
	{
	//	SqlDataSource1.DataBind();
	}
	protected void ASPxButton3_Init(object sender, EventArgs e)
	{
		if (!can_edit)
		{
			var b = (ASPxButton)sender;
			b.ClientVisible = false;
		}
	}
	protected void cbb0_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{

	}
	
	protected void pop_schedule_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
	{
		int memid;
		int sched_id;
		if (e.Parameter.Length > 0)
		{
			memid = Convert.ToInt32(e.Parameter.Split('|').GetValue(1));
			sched_id = Convert.ToInt32(e.Parameter.Split('|').GetValue(0));

		


		var cts = new NeCapTraining_Schedule(sched_id);
		var ct = new NeCapTraining_History();
		ct.training_header_id = cts.training_header_id;
		ct.date = cts.date;
		ct.start_time = cts.start_time;
		ct.end_time = cts.end_time;
		ct.cap_training_schedule_id = cts.id;
		ct.member_id = Convert.ToInt32(memid);
		ct.save();
		}
		

	}
	
	protected void ddl_training_Callback1(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		ddl_training.DataBind();
	}
	protected void cb_CustomGroupDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
	{
		if (e.VisibleRowIndex >= 0)
		{
			if (e.Column.FieldName == "cert_req")
			{
	//			

				var stuff = "";
				if (cb.GetRowValues(e.VisibleRowIndex, "cert_date") != DBNull.Value && (cb.GetRowValues(e.VisibleRowIndex, "cert_date") != ""))
				{
					stuff = " - Last Date: " + Convert.ToDateTime(cb.GetRowValues(e.VisibleRowIndex, "cert_date")).ToString("yyyy-MM-dd") + " and it expires in " + cb.GetRowValues(e.VisibleRowIndex, "cert_exp") + " days";
				}
				e.DisplayText = e.Value + stuff;
				
			}
		}
	}
	protected void chk_mt_CheckedChanged(object sender, EventArgs e)
	{
		Session["tl"] = null;
		filltree();
	}
}
	

	