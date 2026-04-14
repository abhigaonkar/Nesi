using System;
using System.Linq;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Collections.Specialized;
using System.Collections.Generic;
using nesi.core;

public partial class MemberContactDisplay : System.Web.UI.Page
{
	NeMember current_user;
	NameValueCollection _q;
	Toolbox _tools;
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(1);
		_q = Request.QueryString;
		var view_user = new NeMember(Convert.ToInt32(_q["memid"]));
		if (current_user.isContact && view_user.id != current_user.id)
		{
			Toolbox.FriendlyException(Response, "You do not have access to view this profile.", "window.close()");
		}
		if (current_user.isContact)
		{
			if (!IsPostBack)
			{
				pc.ShowTabs = false;
				pc.TabPages.FindByName("member_info").Visible = false;
				pc.TabPages.FindByName("fvr").Visible = false;
				contact_name.Text = current_user.contact_profile.name;
				contact_email.Text = current_user.contact_profile.email;
				contact_title.Text = current_user.contact_profile.title;
				contact_phone.Text = current_user.contact_profile.cellphone;
				contact_extension.Text = current_user.contact_profile.extension;
				contact_direct.Text = current_user.contact_profile.direct_line;
			}
		}
		else
		{
			pc.TabPages.FindByName("contact_info").Visible = false;
			fvr_listing1.mid = view_user.id32;
			lblFirstName.Text = view_user.FirstName;
			lblLastName.Text = view_user.LastName;
			lblOffice.Text = view_user.business_unit.ddl_name;
			lblPhone.Text = view_user.NECellPhoneNumber;
			row_ne_phone_number.Visible = lblPhone.Text != "() -";
			lblEmail.Text = view_user.NEEmail;
			lblExtension.Text = view_user.PhoneExtension;
			row_ne_phone_ext.Visible = lblExtension.Text != "";
			lblPosition.Text = view_user.membertype.name;
			lblCountry.Text = view_user.Country;

			if (current_user.id == view_user.id)
			{
				txtAddress.Text = view_user.Address;
				txtCity.Text = view_user.City;
				txtPostal.Text = view_user.PostalCode;

				#region Province
				var abbvList = new NeProvince();
				ddlProv.DataSource = abbvList.LoadAbbvList();
				ddlProv.DataBind();
				var SelProv1 = new ListItem();
				SelProv1.Text = "OTHER";
				SelProv1.Value = "OTHER";
				ddlProv.Items.Add(SelProv1);
				ddlProv.SelectedValue = view_user.Prov;
				#endregion Province

				txtHomePhoneArea.Text = view_user.PhoneAreaCode;
				txtHomePhoneFirst.Text = view_user.PhoneFirst;
				txtHomePhoneLast.Text = view_user.PhoneLast;
				txtPersonalEmail.Text = view_user.Email;

				txtAddress.Visible = true;
				txtAddress.Enabled = true;
				txtCity.Visible = true;
				txtPostal.Visible = true;
				ddlProv.Visible = true;
				ddlProv.Enabled = true;
				btnUpdate.Visible = true;
				lblAuditWarn.Visible = true;
				txtPersonalEmail.Visible = true;
				txtReason.Visible = true;
				// Check if user has any past fvrs
				var fvr_count = Toolbox.doSQL_int(@"SELECT COUNT(*)
FROM member_fvr_dtl a LEFT JOIN member_fvr_hdr b ON a.member_fvr_hdr_id = b.id LEFT JOIN member_fvr_history c ON c.member_fvr_dtl_id = a.id WHERE a.member_id = @v0
AND IFNULL(c.confirmed, 0) = 1", current_user.id);

				if (fvr_count == 0)
				{
					pc.TabPages.Remove(pc.TabPages[1]);
				}
			}
			else
			{
				pc.ShowTabs = false;
				pc.Width = Unit.Percentage(100);
				btnUpdate.Visible = false;
				lblAuditWarn.Visible = false;
				row_address.Visible = false;
				row_city.Visible = false;
				row_home_phone.Visible = false;
				row_personal_email.Visible = false;
				row_postal.Visible = false;
				row_provstate.Visible = false;
				row_reason.Visible = false;
				row_apprentice_contract.Visible = false;
				pc.ActiveTabIndex = 0;
			}
		}
	}
	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		var new_info = new NeMember(Session["session"].ToString());
		var old_info = new NeMember(Session["session"].ToString());
		//Update Member Information
		new_info.Address = txtAddress.Text;
		new_info.City = txtCity.Text;
		new_info.PostalCode = txtPostal.Text;
		new_info.Prov = ddlProv.SelectedValue;
		new_info.PhoneAreaCode = txtHomePhoneArea.Text;
		new_info.PhoneFirst = txtHomePhoneFirst.Text;
		new_info.PhoneLast = txtHomePhoneLast.Text;
		new_info.Email = txtPersonalEmail.Text;
		new_info.apprentice_contract = txtapprenticecontract.Text;
		new_info.save();

		//Insert Information into Audit Table for Record and tracking

		try
		{
			var before = Toolbox.dict_create(old_info);
			var after = Toolbox.dict_create(new_info);
			var diff_from = before.Except(after).ToDictionary(k => k.Key, v => v.Value);
			var diff_to = after.Except(before).ToDictionary(k => k.Key, v => v.Value);
			var changes = Toolbox.dict_dump(diff_from) + "<b>Changed to</b> <br/>" + Toolbox.dict_dump(diff_to);
			const string subject = "Employee Contact Information Update";
			var body = string.Format("The user <b>{0}</b> has edited their employee contact information.<br/><b>Here is a list of items changed:</b><br/>{1}<br/><br/><b>Reason Given For Edit:</b><br/>{2}", new_info.FullName, changes, txtReason.Text);

			shared.alert_hr(subject, body);
			
			lblConfirm.Visible = true;
			lblConfirm.Text = "You have successfully updated your contact information";
		}
		catch (Exception ee)
		{
			_tools.catch_error(ee);
			lblConfirm.Visible = true;
			lblConfirm.Text = "There was an error updating your contact information, an email has been sent to our support team with the details, please to not try to submit the data again.";
		}
	}
	protected void gv_past_fvrs_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		var type = gv.GetDataRow(e.VisibleIndex)["type"].ToString();
		var cat_id = Convert.ToInt32(gv.GetDataRow(e.VisibleIndex)["tab_index"]);
		switch (e.DataColumn.Index)
		{
			case 1:
				// Type
				switch (type)
				{
					case "NEWHIRE":
						e.Cell.Text = "New Hire";
						break;
					case "RENEW":
						e.Cell.Text = "Renewal";
						break;
					case "EXIT":
						e.Cell.Text = "Exit Interview";
						break;
				}
				break;
			case 2:
				e.Cell.Text = Toolbox.doSQL_string(@"SELECT name FROM member_fvr_tab WHERE id = @v0", cat_id);
				break;
		}
		//string type			= gv.GetDataRow(e.VisibleIndex)["confirmed"].ToString();
	}
	protected void bt_savecontact_Click(object sender, EventArgs e)
	{
		try
		{
			current_user.contact_profile.name = contact_name.Text;
			current_user.contact_profile.email = contact_email.Text;
			current_user.contact_profile.title = contact_title.Text;
			current_user.contact_profile.cellphone = contact_phone.Text;
			current_user.contact_profile.direct_line = contact_direct.Text;
			current_user.contact_profile.extension = contact_extension.Text;
			current_user.contact_profile.save();
			lb_status.ForeColor = System.Drawing.Color.Green;
			lb_status.Text = "Saved";
		}
		catch (Exception ee)
		{
			_tools.catch_error(ee);
			lb_status.ForeColor = System.Drawing.Color.Red;
			lb_status.Text = "An error occurred while saving your data<br/>Please do not try it again; an email has been sent to our IT department while the details of this error.";
		}

	}
}
