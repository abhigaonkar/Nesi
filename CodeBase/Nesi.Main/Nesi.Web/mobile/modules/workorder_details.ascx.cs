using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NESI.Common.Models;
using nesi.core;

public partial class mobile_modules_workorder_details : System.Web.UI.UserControl
	{
	public NeMember current_user {get;set;}
	public int woprog_id {get;set;}
	NeWOProg wo;
	bool approve_pm;
	bool approve_bm;
	bool view_cost;
	bool can_see_sell;
	bool can_create_locations;
	bool can_commit;
	protected void Page_Load(object sender, EventArgs e)
		{
		check_preview_buttons();
		}
	public override void DataBind()
		{
		bind_data();
		}
	public void clear()
		{
		ddl_tabs.SelectedIndex		= 0;
		}
	private void check_preview_buttons()
		{
		woprog_id					= Session["mobile_woprog_id"] == null ? 0 : Convert.ToInt32(Session["mobile_woprog_id"]);
		approve_pm					= current_user.AuthenticatedForPrivilege(15);
		approve_bm					= current_user.AuthenticatedForPrivilege(16);
		var show_invoice_preview = false;
		var show_sendreworks		= false;
		bt_invoicepreview.Visible	= show_invoice_preview;
		if(woprog_id > 0 && current_user.id > 0)
			{
			wo							= new NeWOProg(woprog_id);
			show_invoice_preview = (approve_pm || approve_bm) 			;
			uc_analysis._wo = wo;
			uc_analysis._current_user = current_user;
			bt_invoicepreview.Attributes["onclick"]		= string.Format("boing('/sections/reports/invoice_preview/frame.aspx?id={0}', 'Invoice', 820, 850)", woprog_id);
			if(wo.Status != OpsWOStatus.WaitingToBeInvoiced && wo.Status != "Invoiced" && wo.Status!=OpsWOStatus.Open)
				{
				show_sendreworks		= (approve_pm || approve_bm) && (wo.Status == OpsWOStatus.WaitingPMApproval
												|| wo.Status == OpsWOStatus.WaitingBMApproval
												|| wo.Status == OpsWOStatus.QuestionsForPM);
				}
			var li											= ddl_tabs.Items.FindByValue("6");
			li.Enabled										= show_invoice_preview || show_sendreworks;
			bt_sendreworks.Visible							= show_sendreworks;
			}
		ddl_tabs.Items.FindByValue("1").Enabled			= woprog_id > 0;
		ddl_tabs.Items.FindByValue("2").Enabled = (approve_bm || approve_pm || current_user.business_unit_id == 11);
		ddl_tabs.Items.FindByValue("3").Enabled			= woprog_id > 0;
		ddl_tabs.Items.FindByValue("4").Enabled			= woprog_id > 0;
		ddl_tabs.Items.FindByValue("5").Enabled = false; // woprog_id > 0;
		ddl_tabs.Items.FindByValue("6").Enabled			= woprog_id > 0 && (show_invoice_preview || show_sendreworks);
		ddl_tabs.Items.FindByValue("7").Enabled = woprog_id > 0;
		
		}
	private void bind_data()
		{
		check_preview_buttons();
		var active_index			= Convert.ToInt32(ddl_tabs.SelectedValue);
		if (active_index.ToString() != mv_content.ActiveViewIndex.ToString())
		{
			mv_content.ActiveViewIndex = active_index == 3 || active_index == 7 ? mv_content.ActiveViewIndex : active_index;
			load_view(active_index);
		}
		}
	private void load_view(int active_index)
		{
		ddl_tabs.Items.FindByValue("0").Text			= "General";
		ddl_tabs.Items.FindByValue("1").Text			= "Notes";
		ddl_tabs.Items.FindByValue("2").Text = "Analysis";
		ddl_tabs.Items.FindByValue("3").Text			= "Signoff";
		ddl_tabs.Items.FindByValue("4").Text			= "Files";
		ddl_tabs.Items.FindByValue("5").Text			= "Time Entry";
		ddl_tabs.Items.FindByValue("6").Text			= "Invoice Preview";
		ddl_tabs.Items.FindByValue("7").Text = "Line Items";
		switch(active_index)
			{
			case 0:
				//General
				if_wo.Attributes["src"] = "/sections/workorder/mobile_wo/index.aspx?id=" + woprog_id+"&is_mobile=1";
				if_wo.Attributes.Add("onload", "resizeIframe(this);");
				ddl_tabs.Items.FindByValue("0").Text			= "WO #0"+wo.OrderNumber.TrimStart('0')+" - General";
			break;
			case 1:
				load_comments();
				ddl_tabs.Items.FindByValue("1").Text			= "WO #0"+wo.OrderNumber.TrimStart('0')+" - Notes";
			break;

			case 2:
			uc_analysis._current_user = current_user;
				uc_analysis.woprog_id = wo.woprog_id;
				uc_analysis._wo = wo;
				uc_analysis.DataBind();
				uc_analysis.load();
				ddl_tabs.SelectedValue = mv_content.ActiveViewIndex.ToString();
			break;

			case 3:
				ScriptManager.RegisterStartupScript(this, GetType(), ClientID, string.Format("boing('/mobile/index.aspx?a=signoff&woprog_id={0}&from=wo', 'preview', 1024, 768);", woprog_id), true);
				ddl_tabs.SelectedValue		= mv_content.ActiveViewIndex.ToString();
			break;
			case 4:
				// Files
				uc_filemanager.woprog_id = woprog_id;
				uc_filemanager.current_user = current_user;
				uc_filemanager.DataBind();
				mv_content.ActiveViewIndex	= 3;
				ddl_tabs.Items.FindByValue("4").Text			= "WO #0"+wo.OrderNumber.TrimStart('0')+" - Files";
			break;
			case 5:
				timesheet.workorder_id	= woprog_id;
				timesheet.DataBind();
				ddl_tabs.Items.FindByValue("5").Text			= "WO #0"+wo.OrderNumber.TrimStart('0')+" - Time Entry";
			break;
			case 6:
				ddl_tabs.Items.FindByValue("6").Text			= "WO #0"+wo.OrderNumber.TrimStart('0')+" - Invoice Preview";
			break;
			case 7:
				ScriptManager.RegisterStartupScript(this, GetType(), ClientID, string.Format("boing('/mobile/index.aspx?a=part_management2&woprog_id={0}', 'preview', 1024, 768);", woprog_id), true);
				ddl_tabs.SelectedValue		= mv_content.ActiveViewIndex.ToString();
			break;
			
			}
		}
	protected void ddl_tabs_SelectedIndexChanged(object sender, EventArgs e)
		{
		bind_data();
		}
	private void load_comments()
		{
		var comments		= Toolbox.doSQL_dt(@" SELECT b.member_fullname AS name, a.woprogcomment_datetime dt, a.woprogcomment_text comment FROM woprogcomment a LEFT JOIN member b ON a.woprogcomment_member_id = b.member_id LEFT JOIN woprog c ON a.woprogcomment_woprog_id = c.woprog_id WHERE woprogcomment_woprog_id = @v0  AND woprogcomment_deleted = 'F' ", new object[] {  woprog_id } );
		var sb		= new StringBuilder();
		if(comments.Rows.Count == 0)
			{
			sb.Append("<center>No comments (yet)</center>");
			}
		else
			{
			foreach(DataRow dr in comments.Rows)
				{
				var name	= (string) dr["name"];
				var dt		= (DateTime) dr["dt"];
				var comment	= (string) dr["comment"];
				sb.AppendFormat(@"
				<div class='comment_box'>
					<div class='who'>{0}</div>
					<div class='when'>{1}</div>
					<div class='comment'>{2}</div>
				</div>
				", name, Toolbox.MySQL_longdt(dt), comment);
				}
			}
		past_notes.InnerHtml		= sb.ToString();
		}
	protected void cbp_notes_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{{
		var woprog_id = 0;
		if(Session["mobile_woprog_id"] != null)
			{
			int.TryParse(Session["mobile_woprog_id"].ToString(), out woprog_id);
			}
		var woc = new woprog_comment();
		if (e.Parameter.Contains("|"))
			{
			var paras = e.Parameter.Split('|');
			switch (paras[0])
				{
				#region addnote
				case "addnote":
					woc = new woprog_comment
							{
								deleted = "F",
								member_id = current_user.id,
								text = new_note.Text,
								woprog_id = woprog_id
							};
					woc.save();
					new_note.Text = "";
					new_note.Focus();
					load_comments();
				break;
				#endregion addnote
				}
			}
		}
		}
}