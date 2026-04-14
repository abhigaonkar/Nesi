using System;
using System.Data;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using DevExpress.Web;
using System.Collections.Generic;
using nesi.core;

public partial class backend : System.Web.UI.Page
	{
	NeMember current_user;
	private const int _page_id			= 61; // from Page table in DB
	private const string _page_description	= "Bonuses / Commissions";
	private const string Output				= "";
	int current_payperiod_id				= 0;

	Toolbox _tools						= new Toolbox();
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools											= new Toolbox();
		current_user									= Toolbox.do_handle_authentication(_page_id);
		Session["member_id"]							= current_user.id;
		Session["visibleBU"]                            = new Current_User().visible_business_units;

        current_payperiod_id							= Toolbox.doSQL_int("CALL _payperiod();");
		}
    protected void Page_Load(object sender, EventArgs e)
		{

		var _q				= Request.QueryString;
		DataTable _dt;
		var menu							= new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(current_user);
		var lbltemp						= (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text						= _page_description;


        if (!string.IsNullOrEmpty(_q["a"]) && _q["a"] == "email_request")
			{
			var _id					= string.IsNullOrEmpty(_q["id"]) ? 0 : Convert.ToInt32(_q["id"]);
			var _approve			= string.IsNullOrEmpty(_q["approve"]) ? false : _q["approve"] == "1";
			if(_id > 0)
				{
				var current_approved_status		= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(approved), -1) FROM payroll_extra_payments WHERE id = @v0", _id);
				if(current_approved_status > -1)
					{
					Toolbox.FriendlyPopup(Response, "This request has already been dealt with.<br/><a href='/home.aspx'>Home</a> | <a href='/default.aspx?sign_out=true'>Logout</a>", "", "");
					}
				else
					{
					Toolbox.doSQL_void(@"UPDATE payroll_extra_payments SET approved = @v1, approved_by = @v2 WHERE id = @v0 LIMIT 1", new object[] { _id, _approve, current_user.id});
					var dr			= Toolbox.doSQL_dt(@"SELECT * FROM payroll_extra_payments WHERE id = @v0  LIMIT 1", new object[] {  _id } ).Rows[0];

					var em			= new NeEMail();
					var requester	= new NeMember(Convert.ToInt32(dr["requested_by"]));
					var employee	= new NeMember(Convert.ToInt32(dr["member_id"]));
					var this_type	= dr["type"].ToString();
					var type_of		= this_type == "B" ? "Bonus" : this_type == "C" ? "Commission" : "Manual Payment";
					em.To				= requester.NEEmail;
					em.From				= "noreply@" + Toolbox.app_setting("DomainForEmail");
					em.Subject			= string.Format("The request for {0}'s {1} has been {2} by {3}", employee.FullName, type_of, (_approve ? "approved" : "denied"), current_user.FullName);
					em.Send();
					Toolbox.FriendlyPopup(Response, "This request has been "+(_approve ? " approved" : " denied")+"<br/><a href='/home.aspx'>Home</a> | <a href='/default.aspx?sign_out=true'>Logout</a>", "", "");
					}
				}
			}
		} //ends onload
	protected void gv_requests_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var this_payperiod			= Convert.ToInt32(gv.GetDataRow(e.VisibleIndex)["payperiod_id"]);
		e.Visible					= this_payperiod >= current_payperiod_id;
		}
	protected void edit_employee_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var cb				= (ASPxComboBox) sender;
		cb.DataBind();
		}
	protected void cbp_edit_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var gv				= gv_requests;
		var cbp		= (ASPxCallbackPanel) sender;
		var hid_id			= (HiddenField) cbp.FindControl("hid_id");
		var _id						= hid_id.Value;
		var cb_type		= (ASPxComboBox) cbp.FindControl("edit_type");
		var _type					= cb_type.Value;
		var this_type			= "";
		var cb_branch		= (ASPxComboBox) cbp.FindControl("edit_branch");
		var _branch					= cb_branch.Value;
		var this_branch				= 0;
		var cb_employee	= (ASPxComboBox) cbp.FindControl("edit_employee");
		var _employee				= cb_employee.Value;
		var this_employee			= 0;
		var cb_payperiod	= (ASPxComboBox) cbp.FindControl("edit_payperiod");
		var _payperiod				= cb_payperiod.Value;
		var this_payperiod			= 0;
		var tb_amount		= (ASPxTextBox) cbp.FindControl("edit_amount");
		var _amount					= tb_amount.Value;
		double this_amount			= 0;
		var memo_note			= (ASPxMemo) cbp.FindControl("edit_memo");
		var _note					= memo_note.Value;
		var this_note			= "";
		var lb_notify			= (ASPxLabel) cbp.FindControl("notify");

		// Validate
		var errors			= new List<string>();
		if(_type == null)
			{
			errors.Add("Please provide the type of payment.");
			}
		else
			{
			this_type				= _type.ToString();
			}
		if(_branch == null)
			{
			errors.Add("Please provide the branch this payment is for.");
			}
		else
			{
			int.TryParse(_branch.ToString(), out this_branch);
			}
		if(_employee == null)
			{
			errors.Add("Please provide the employee this payment is going to.");
			}
		else
			{
			int.TryParse(_employee.ToString(), out this_employee);
			}
		if(_payperiod == null)
			{
			errors.Add("Please provide the payperiod this is for.");
			}
		else
			{
			int.TryParse(_payperiod.ToString(), out this_payperiod);
			}
		if(_amount == null || !double.TryParse(_amount.ToString(), out this_amount))
			{
			errors.Add("Please provide a valid amount to pay (Numbers and a decimal point <u>only</u>)");
			}
		if(_note == null)
			{
			errors.Add("Please provide a note as to why you are paying this out.");
			}
		else if(_note.ToString().Length < 10)
			{
			errors.Add("Please provide a (inter) note as to why you are paying this out... 10 character minimum");
			}
		else
			{
			this_note					= _note.ToString();
			}
		if(errors.Count > 0)
			{
			var sb			= new StringBuilder();
			sb.Append("<b>The following errors occured:</b><br/><ul style='color:#f00;'>");
			foreach(var s in errors)
				{
				sb.AppendFormat("<li>{0}</li>",s);
				}
			sb.Append("</ul>");
			lb_notify.Text				= sb.ToString();
			}
		else
			{
			var em							= new NeEMail();
			var employee					= new NeMember((int) this_employee);
			var employee_branch			= employee.business_unit;
			var managers_id = current_user.reports_to;
			var type_of						= this_type == "B" ? "Bonus" : this_type == "C" ? "Commission" : "Manual Payment";
			if (managers_id == 0)
			{
				managers_id = 8;
			}
			var manager					= new NeMember(managers_id);

			if(_id == "0") // Insert
				{
				_id							= Toolbox.doSQL_return_id(@"INSERT INTO payroll_extra_payments 
(type, payperiod_id, member_id, requested_when, requested_by, approved, amount, note) 
VALUES (@v4, @v1, @v0, now(), @v5, -1, @v2, @v3)", new object[] { this_employee, this_payperiod, this_amount, this_note, this_type, current_user.id}).ToString();
				em.From						= current_user.NEEmail;
				var message_body			= string.Format(@"
		<div style='font-family:arial;font-size:12px;'>
		<b>{0}</b> has requested that <b>{1}</b> receive a <b>{2}</b> in the amount of <b style='color:#060;'><i>{3:C2}</i></b>
		<hr>
		<b>Reason Submitted:</b><br>
		{4}
		<p>
		Please log in to Approve or Deny this before next payroll.
		</p>
		</div>", 
				current_user.FullName,
				employee.FullName, 
				type_of, 
				this_amount,
				HttpUtility.HtmlEncode(this_note)
				);

				em.isHTML					= true;
				em.Body						= message_body;
				em.Subject					= (string) string.Format("(NEW) {0} request for {1}", type_of, employee.FullName);
				em.to_member_id				= managers_id;
				em.To						= manager.NEEmail;
				var url_base				= string.Format("/sections/hr/commission_bonus/index.aspx?a=email_request&id={0}&approve=", _id);
				em.URLyes					= url_base+"1";
				em.URLno					= url_base+"0";
				em.file_passport();
				}
			else // Update 
				{
				Toolbox.doSQL_void(@"UPDATE payroll_extra_payments 
SET member_id = @v0, payperiod_id=@v1, amount=@v2, note=@v3, approved = -1 WHERE id = @v5 LIMIT 1",
					new object[] { this_employee, this_payperiod, this_amount, this_note, this_type, _id});
				// Disable all past passports for this request.
				Toolbox.doSQL_void(@"UPDATE passport 
SET active = 0 WHERE `to` = @v0 AND url_yes LIKE '%commission_bonus%' AND url_yes LIKE CONCAT('%?id=',@v1,'&%')", new object[] { manager.NEEmail, _id});

				em.From						= current_user.NEEmail;
				var message_body			= string.Format(@"
		<div style='font-family:arial;font-size:12px;'>
		<u style='color:#f00'>NOTICE: This is an edit to a previous request for this user, you may or may not have dealt with the past request, but the previous email(s) will no longer work... you will need to use this one.</u><br/><br/>

		<b>{0}</b> has requested that <b>{1}</b> receive a <b>{2}</b> in the amount of <b style='color:#060;'><i>{3:C2}</i></b>
		<hr>
		<b>Reason Submitted:</b><br>
		{4}
		<p>
		Please log in to Approve or Deny this before next payroll.
		</p>
		</div>", 
				current_user.FullName,
				employee.FullName, 
				type_of, 
				this_amount,
				HttpUtility.HtmlEncode(this_note)
				);

				em.isHTML					= true;
				em.Body						= message_body;
				em.Subject					= (string) string.Format("(EDITED) {0} request for {1}", type_of, employee.FullName);
				em.to_member_id				= managers_id;
				em.To						= manager.NEEmail;
				var url_base				= string.Format("/sections/hr/commission_bonus/index.aspx?a=email_request&id={0}&approve=", _id);
				em.URLyes					= url_base+"1";
				em.URLno					= url_base+"0";
				em.file_passport();
				cb_branch.ClientEnabled		= false;
				cb_employee.ClientEnabled	= false;
				}
			}
		}
	protected void gv_requests_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var gv				= gv_requests;
		var is_editing				= gv.IsEditing;
		var is_new					= gv.IsNewRowEditing;
		var this_id					= is_new ? "0" : gv.GetDataRow(gv.EditingRowVisibleIndex)["id"];
		var this_type				= is_new ? "B" : gv.GetDataRow(gv.EditingRowVisibleIndex)["_type"];
		var this_branch				= is_new ? current_user.business_unit_id.ToString() : gv.GetDataRow(gv.EditingRowVisibleIndex)["business_unit_id"];
		var this_employee			= is_new ? null : gv.GetDataRow(gv.EditingRowVisibleIndex)["member_id"];
		var this_payperiod			= is_new ? current_payperiod_id.ToString() : gv.GetDataRow(gv.EditingRowVisibleIndex)["payperiod_id"];
		var this_amount				= is_new ? "" : gv.GetDataRow(gv.EditingRowVisibleIndex)["amount"];
		var this_note				= is_new ? "" : gv.GetDataRow(gv.EditingRowVisibleIndex)["note"];
		var member_status		= is_new ? "" : (string) gv.GetDataRow(gv.EditingRowVisibleIndex)["member_status"];

		var cbp		= (ASPxCallbackPanel) gv.FindEditFormTemplateControl("cbp_edit");
		if(cbp != null)
			{
			var cb_type		= (ASPxComboBox) cbp.FindControl("edit_type");
			var cb_branch		= (ASPxComboBox) cbp.FindControl("edit_branch");
			var cb_employee	= (ASPxComboBox) cbp.FindControl("edit_employee");
			var cb_payperiod	= (ASPxComboBox) cbp.FindControl("edit_payperiod");
			var tb_amount		= (ASPxTextBox) cbp.FindControl("edit_amount");
			var memo_note			= (ASPxMemo) cbp.FindControl("edit_memo");
			var lb_notify			= (ASPxLabel) cbp.FindControl("notify");
			var hid_id			= (HiddenField) cbp.FindControl("hid_id");

			cb_type.Value				= this_type == null ? "" : this_type.ToString();
			cb_branch.Value				= this_branch == null ? "" : this_branch.ToString();
			cb_employee.Value			= this_employee == null ? "" : this_employee.ToString();
			cb_payperiod.Value			= this_payperiod == null ? "" : this_payperiod.ToString();
			tb_amount.Value				= this_amount == null ? "" : this_amount.ToString();
			memo_note.Value				= this_note == null ? "" : this_note.ToString();
			hid_id.Value				= this_id == null ? "" : this_id.ToString();
			cb_branch.ClientEnabled		= is_new;
			cb_employee.ClientEnabled	= is_new;

			if(member_status == "Not Active")
				{

				}
			}
		}
	protected void gv_requests_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var gv				= gv_requests;
		var _id						= (int) e.Keys[0];
		var current_approved_status			= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(approved), -1) FROM payroll_extra_payments WHERE id = @v0", _id);
		Toolbox.doSQL_void(@"DELETE FROM payroll_extra_payments WHERE id = @v0 LIMIT 1", _id);

		var this_type					= gv.GetRowValuesByKeyValue(_id, "_type").ToString();
		var this_employee					= Convert.ToInt32(gv.GetRowValuesByKeyValue(_id, "member_id"));
		
		var em							= new NeEMail();
		var employee					= new NeMember((int) this_employee);
		var employee_branch			= employee.business_unit;
		var managers_id					= (int) employee.reports_to;
		var type_of						= this_type == "B" ? "Bonus" : this_type == "C" ? "Commission" : "Manual Payment";

		if(current_approved_status != -1)
			{
			var manager					= new NeMember(managers_id);
			Toolbox.doSQL_void(@"UPDATE passport SET active = 0 WHERE `to` = @v0 AND url_yes LIKE '%commission_bonus%' AND url_yes LIKE CONCAT('%?id=',@v1,'&%')",
				new object[] { manager.NEEmail, _id});
			em.From								= current_user.NEEmail;
			var message_body					= string.Format(@"
			<div style='font-family:arial;font-size:12px;'>
			The request for {1}'s {2} has been deleted by {0}.<br/>
			No further action is needed, and all passports have been disabled.
			</div>", 
			current_user.FullName,
			employee.FullName, 
			type_of
			);

			em.isHTML					= true;
			em.Body						= message_body;
			em.Subject					= (string) string.Format("(DELETED) {0} request for {1}", type_of, employee.FullName);
			em.To						= manager.NEEmail;
			em.Send();
			}
		gv.DataBind();
		e.Cancel				= true;
		}
	protected void gv_requests_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if(e.DataColumn.FieldName == "approved")
			{
			switch(Toolbox.ReturnBlankIfNull_string(e.CellValue).ToString())
				{
				case "Approved":
					e.Cell.BackColor		= System.Drawing.ColorTranslator.FromHtml("#00ff00");
				break;
				case "Denied":
					e.Cell.BackColor		= System.Drawing.ColorTranslator.FromHtml("#ff0000");
					e.Cell.ForeColor		= System.Drawing.Color.White;
				break;
				case "Requested":
					e.Cell.BackColor		= System.Drawing.ColorTranslator.FromHtml("#dddddd");
				break;
				}
			}
		}
	protected void gv_requests_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
		{
		var gv				= gv_requests;
		if(e.VisibleIndex > -1)
			{
			var dr				= gv.GetDataRow(e.VisibleIndex);
			var this_note			= dr["note"].ToString();
			e.Row.Attributes["data-title"] = "Note";
			e.Row.Attributes["data-tooltip"] = this_note;
			e.Row.CssClass				+= " ttip";
			}
		}
}