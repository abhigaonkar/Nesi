using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Text;
using nesi.core;

public partial class sections_hr_i_modules_employee_information : System.Web.UI.UserControl
	{
	emp_checks chks					= new emp_checks();
	private NeMember _current_user	= new NeMember();
	public NeMember current_user {get {return _current_user;} set {_current_user = value; } }
	private member_fvr_dtl _dtl		= new member_fvr_dtl();
	public member_fvr_dtl dtl {get {return _dtl;} set {_dtl = value; }}
	public int detail_id {get;set;}
	public string template_type {get;set;}
	public string template_subtype {get;set;}
	protected void Page_Init(object sender, EventArgs e)
		{
		if(prov_state != null)
			{
			prov_state.DataSource			= shared.GetProvs();
			prov_state.DataBind();
			}
		if(country != null)
			{
			country.DataSource				= shared.GetCountries();
			country.DataBind();
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		chks.populate_session(Page, ref chks);
		}
	protected void cb_CheckedChanged(object sender, EventArgs e)
		{
		var cb						= (CheckBox) sender;
		if(validate(false))
			{
			dtl								= new member_fvr_dtl((int) Session["employee_information_dtl_id"]);
			var hist			= new member_fvr_history(dtl.id);
			hist.member_fvr_dtl_id			= dtl.id;
			hist.confirmed					= cb.Checked ? 1 : 0;
			chks.new_information			= hist.confirmed == 1;
			Session["emp_checks"]			= chks;
			hist.save();
			if(Toolbox.doSQL_int(@"SELECT COUNT(id) FROM member_fvr_dtl a LEFT JOIN member_fvr_history b ON a.id = b.member_fvr_dtl_id 
WHERE a.member_id = @v0 AND a.member_fvr_hdr_id = @v1 AND IFNULL(b.confirmed, 0) = 0", new object[] { current_user.id, dtl.member_fvr_hdr_id}) == 0)
				{
				ScriptManager.RegisterStartupScript(this, GetType(), "closeme", "opener.location.href = opener.location.href;window.close()", true);
				}
			//ScriptManager.RegisterStartupScript(this, GetType(), "refreshme", "location.reload();", true);
			}
		}
	public void populate(int _id)
		{
		if(is_loaded != null && is_loaded.Value == "0")
			{
			var _tools									= new Toolbox();
			current_user									= Toolbox.do_handle_authentication(1);
			var u						                = new NeMember(current_user.id);
			first_name.Text					                = u.FirstName;
			middle_initial.Text								= u.MiddleInitial;
			last_name.Text					                = u.LastName;
			preferred_name.Text								= u.Nickname;
			address.Text					                = u.Address;
			city.Text						                = u.City;
			apprentice_contract.Text						= u.apprentice_contract;
			zip_code.Text					                = u.PostalCode;
			if (u.Prov != "")
			{
				prov_state.Items.FindByValue(u.Prov).Selected = true;
			}
			else
			{
				prov_state.SelectedIndex = -1;
			}
			country.Items.FindByValue(u.Country).Selected	= true;
			phone_area.Text									= u.PhoneAreaCode;
			phone_pre.Text									= u.PhoneFirst;
			phone_suffix.Text								= u.PhoneLast;
			if(u.cellphone_number_id > 0 && u.business_unit.allow_cell_edit)
			{
				var cell = new NECellphone_Number(u.cellphone_number_id);
				if(cell.number.Length == 12)
				{
					work_area.Text = cell.number.Substring(0, 3);
					work_area.Enabled = false;
					work_pre.Text = cell.number.Substring(4, 3);
					work_pre.Enabled = false;
					work_suffix.Text = cell.number.Substring(8, 4);
					work_suffix.Enabled = false;
				}
			}
			else
				{
				work_area.Enabled = false;
				work_pre.Enabled = false;
				work_suffix.Enabled = false;
				}
			


            birthdate.Text = u.BirthDate;

			
			email_address.Text								= u.Email;
			drivers_license.Text							= u.DriversLicence;
			electric_license.Text							= u.ElectricalLicence;
			emergency_first_name.Text						= u.EmergencyFirstName1;
			emergency_last_name.Text						= u.EmergencyLastName1;
			emergency_phone_area.Text						= u.EmergencyPhoneArea1;
			emergency_phone_pre.Text						= u.EmergencyPhoneFirst1;
			emergency_phone_suffix.Text						= u.EmergencyPhoneLast1;
			extension.Text = u.PhoneExtension;
			extension.Enabled = extension.Text == "";
			work_email.Text = u.NEEmail;
			work_email.Enabled = u.business_unit.allow_email_edit && work_email.Text == "" || work_email.Text.Contains("nomail");
			/*emergency_first_name0.Text = u.EmergencyFirstName2;
			emergency_last_name0.Text = u.EmergencyLastName2;
			emergency_phone_area0.Text = u.EmergencyPhoneArea2;
			emergency_phone_pre0.Text = u.EmergencyPhoneFirst2;
			emergency_phone_suffix0.Text = u.EmergencyPhoneLast2;*/
			dtl												= new member_fvr_dtl(_id);
			cb.Attributes["data-detail_id"]					= dtl.id.ToString();
			cb.Attributes["data-type"]						= template_type;
			cb.Attributes["data-subtype"]					= template_subtype;
			var hist										= new member_fvr_history(dtl.id);
			cb.Checked										= hist.confirmed == 1;
			dtl_id.Value									= _id.ToString();
			Session["employee_information_dtl_id"]			= dtl.id;
			is_loaded.Value									= "1";

			cb.Disabled = !validate(false);

			}
		}
	public bool validate(bool from_external)
		{
		if(!Visible)
			{
			return true;
			}
		var is_valid						= true;
		var errors					= new List<string>();
		var used_firstname				= from_external ? current_user.FirstName : first_name.Text.Trim();
		var used_middleinitial			= from_external ? current_user.MiddleInitial : middle_initial.Text.Trim();
		var used_lastname				= from_external ? current_user.LastName : last_name.Text.Trim();
		var used_address					= from_external ? current_user.Address : address.Text.Trim();
		var used_city					= from_external ? current_user.City : city.Text.Trim();
		var used_postal					= from_external	? current_user.PostalCode : zip_code.Text.Trim();
		var used_p_area 					= from_external ? current_user.PhoneAreaCode : phone_area.Text.Trim();
		var used_p_pre  					= from_external ? current_user.PhoneFirst : phone_pre.Text.Trim();
		var used_p_suff 					= from_external	? current_user.PhoneLast : phone_suffix.Text.Trim();
		var used_birthdate				= from_external ? current_user.BirthDate : birthdate.Text.Trim();
		
		var used_email					= from_external ? current_user.Email : email_address.Text.Trim();
		var used_em_firstname			= from_external ? current_user.EmergencyFirstName1 : emergency_first_name.Text.Trim();
		var used_em_lastname				= from_external ? current_user.EmergencyLastName1 : emergency_last_name.Text.Trim();
		var used_em_p_area				= from_external ? current_user.EmergencyPhoneArea1 : emergency_phone_area.Text.Trim();
		var used_em_p_pre				= from_external ? current_user.EmergencyPhoneFirst1 : emergency_phone_pre.Text.Trim();
		var used_em_p_suff				= from_external ? current_user.EmergencyPhoneLast1 : emergency_phone_suffix.Text.Trim();
		/*var used_em_firstname0 = from_external ? current_user.EmergencyFirstName2 : emergency_first_name0.Text.Trim();
		var used_em_lastname0 = from_external ? current_user.EmergencyLastName2 : emergency_last_name0.Text.Trim();
		var used_em_p_area0 = from_external ? current_user.EmergencyPhoneArea2 : emergency_phone_area0.Text.Trim();
		var used_em_p_pre0 = from_external ? current_user.EmergencyPhoneFirst2 : emergency_phone_pre0.Text.Trim();
		var used_em_p_suff0 = from_external ? current_user.EmergencyPhoneLast2 : emergency_phone_suffix0.Text.Trim();*/
		// Validation
		#region First Name
		if(used_firstname == "")
			{
			errors.Add("First name not supplied");
			is_valid = false;
			}
		#endregion First Name
		#region Middle Initial
		if(used_middleinitial == "")
			{
		//	errors.Add("Middle Initial not supplied");
		//	is_valid = false;
			}
		#endregion Middle Initial
		#region Last Name
		if(used_lastname == "")
			{
			errors.Add("Last name not supplied");
			is_valid = false; 
			}
		#endregion Last Name
		#region Address
		if(used_address == "")
			{
			errors.Add("Address not supplied");
			is_valid = false;
			}
		#endregion Address
		#region City
		if(used_city == "")
			{
			errors.Add("City not supplied");
			is_valid = false;
			}
		#endregion City
		#region Postal
		if(used_postal == "")
			{
			errors.Add("Postal (Zip) Code not supplied");
			is_valid = false;
			}
		#endregion Postal
		#region Home Phone
		if(used_p_area == "")
			{
			errors.Add("Personal phone's area code not supplied");
			is_valid = false;
			}
		else if(used_p_area.Length > 3)
			{
			errors.Add("Personal phone's area code is too long");
			is_valid = false;
			}
		if(used_p_pre == "")
			{
			errors.Add("Personal phone's prefix not supplied");
			is_valid = false;
			}
		else if(used_p_pre.Length > 3)
			{
			errors.Add("Personal phone's prefix is too long");
			is_valid = false;
			}
		if(used_p_suff == "")
			{
			errors.Add("Personal phone's suffix not supplied");
			is_valid = false;
			}
		else if(used_p_suff.Length > 4)
			{
			errors.Add("Personal phone's suffix is too long");
			is_valid = false;
			}
		#endregion Home Phone
		#region Birthdate
		if(used_birthdate == "")
			{
			errors.Add("Birthdate not supplied");
			is_valid = false;
			}
		else
			{
			var try_birthdate	= new DateTime();
			if(!DateTime.TryParse(used_birthdate, out try_birthdate))
				{
				errors.Add("Birthdate was not a valid date - Use format MM/DD/YYYY");
				is_valid = false;
				}
			if(try_birthdate.Year < 1900)
				{
				errors.Add("Birthdate was not a valid date.");
				is_valid = false;
				}
			}
		#endregion Birthdate
		#region Social
		#endregion Social
		#region Email
		if(used_email == "")
			{
			errors.Add("Personal Email not supplied");
			is_valid = false;
			}
		else if(!Toolbox.CheckEmail(used_email))
			{
			errors.Add("Personal Email is not valid");
			is_valid = false;
			}
		#endregion Email
		#region Work Email
		if (current_user.business_unit.allow_email_edit && current_user.NEEmail == "" && work_email != null && work_email.Text.Trim() != "" && !Toolbox.CheckEmail(work_email.Text.Trim()))
		{
			errors.Add("Work Email is not valid");
			is_valid = false;
		}
		#endregion WorkEmail
		//#region Drivers License
		//	if(drivers_license.Text.Trim() == "")
		//		{
		//		errors.Add("Driver's License # not supplied");
		//		is_valid = false;
		//		}
		//#endregion Drivers License
		#region Emergency firstname
		if (used_em_firstname == "")
			{
			errors.Add("Emergency Contact's First Name not supplied");
			is_valid = false;
			}
		#endregion Emergency firstname
		#region  Emergency lastname
		if(used_em_lastname == "")
			{
			errors.Add("Emergency Contact's Last Name not supplied");
			is_valid = false;
			}
		#endregion Emergency lastname
		#region  Emergency phone #
		if(used_em_p_area == "")
			{
			errors.Add("Emergency Contact's phone area code not supplied");
			is_valid = false;
			}
		else if(used_em_p_area.Length > 3)
			{
			errors.Add("Emergency Contact's phone area code is too long");
			is_valid = false;
			}
		if(used_em_p_pre == "")
			{
			errors.Add("Emergency Contact's phone prefix not supplied");
			is_valid = false;
			}
		else if(used_em_p_pre.Length > 3)
			{
			errors.Add("Emergency Contact's phone prefix is too long");
			is_valid = false;
			}
		if(used_em_p_suff == "")
			{
			errors.Add("Emergency Contact's phone suffix not supplied");
			is_valid = false;
			}
		else if(used_em_p_suff.Length > 4)
			{
			errors.Add("Emergency Contact's phone suffix is too long");
			is_valid = false;
			}
		#endregion Emergency phone #

		if(current_user.business_unit.allow_cell_edit && current_user.cellphone_number_id == 0)
		{
			if(work_area != null && work_area.Text.Trim() != "")
			{
				var TestNo = 0;
				int.TryParse(work_area.Text.Trim(), out TestNo);
				if(TestNo == 0)
				{
					errors.Add("The area code supplied for your work phone is blank or invalid");
					is_valid = false;
				}
				else if (work_area.Text.Trim().Length != 3)
				{
					errors.Add("The area code supplied for your work phone isn't 3 digits");
					is_valid = false;
				}
			}
			if (work_pre != null && work_pre.Text.Trim() != "")
			{
				var TestNo = 0;
				int.TryParse(work_pre.Text.Trim(), out TestNo);
				if (TestNo == 0)
				{
					errors.Add("The prefix supplied for your work phone is blank or invalid");
					is_valid = false;
				}
				else if (work_pre.Text.Trim().Length != 3)
				{
					errors.Add("The prefix supplied for your work phone isn't 3 digits");
					is_valid = false;
				}
			}
			if (work_suffix != null && work_suffix.Text.Trim() != "")
			{
				var TestNo = 0;
				int.TryParse(work_suffix.Text.Trim(), out TestNo);
				if (TestNo == 0)
				{
					errors.Add("The suffix (line number) supplied for your work phone is blank or invalid");
					is_valid = false;
				}
				else if(work_suffix.Text.Trim().Length != 4)
				{
					errors.Add("The suffix (line number) supplied for your work phone isn't 4 digits");
					is_valid = false;
				}
			}
		}
        /*
		#region Emergency firstname0
		if (used_em_firstname0 == "")
		{
			errors.Add("Emergency Contact #2's First Name not supplied");
			is_valid = false;
		}
		#endregion

		#region  Emergency lastname0
		if (used_em_lastname0 == "")
		{
			errors.Add("Emergency Contact #2's Last Name not supplied");
			is_valid = false;
		}
		#endregion Emergency lastname0
		#region  Emergency phone #0
		if (used_em_p_area0 == "")
		{
			errors.Add("Emergency Contact #2's phone area code not supplied");
			is_valid = false;
		}
		if (used_em_p_pre0 == "")
		{
			errors.Add("Emergency Contact #2's phone prefix not supplied");
			is_valid = false;
		}
		if (used_em_p_suff0 == "")
		{
			errors.Add("Emergency Contact #2's phone suffix not supplied");
			is_valid = false;
		}
		#endregion Emergency phone #0
    */

		if (!from_external)
			{
			if(!is_valid)
				{
				var report		= new StringBuilder();
				report.Append("<b>Errors</b><br/><ul>");
				for(var i = 0; i < errors.Count; i++)
					{
					report.AppendFormat("<li>{0}</li>", errors[i]);
					}
				report.Append("</ul>");
				error_report.InnerHtml		= report.ToString();
				}
			else
				{
				error_report.InnerHtml		= "";
				}
			}
		if (is_valid)
		{
			if (cb != null)
			{
				cb.Disabled = false;
			}
		}
		return is_valid;

		}
	protected void bt_save_Click(object sender, EventArgs e)
		{
		if(validate(false))
			{
			var email						= "";
			try
				{
				var _tools							= new Toolbox();
				current_user							= Toolbox.do_handle_authentication(1);
				var before		 = Toolbox.dict_create(current_user);
				current_user.FirstName					= first_name.Text; 
				current_user.MiddleInitial				= middle_initial.Text.Trim();
				current_user.LastName					= last_name.Text; 
				current_user.Address					= address.Text; 
				current_user.City						= city.Text; 
				current_user.PostalCode					= zip_code.Text; 
				current_user.Prov						= prov_state.SelectedItem.Value; 
				current_user.Country					= country.SelectedItem.Value; 
				current_user.PhoneAreaCode				= phone_area.Text; 
				current_user.PhoneFirst					= phone_pre.Text; 
				current_user.PhoneLast					= phone_suffix.Text; 
				current_user.BirthDate					= Toolbox.MySQL_shortdt(Convert.ToDateTime(birthdate.Text));
				current_user.NEEmail = work_email.Text;
				
				current_user.Email						= email_address.Text.Trim();
				current_user.NEEmail = current_user.business_unit.allow_email_edit ? work_email.Text.Trim() : current_user.NEEmail;
				current_user.DriversLicence				= drivers_license.Text.Trim(); 
				current_user.ElectricalLicence			= electric_license.Text.Trim();
				current_user.apprentice_contract		= apprentice_contract.Text.Trim();
				current_user.EmergencyFirstName1		= emergency_first_name.Text.Trim();
				current_user.EmergencyLastName1			= emergency_last_name.Text.Trim();
				current_user.EmergencyPhoneArea1		= emergency_phone_area.Text.Trim();
				current_user.EmergencyPhoneFirst1		= emergency_phone_pre.Text.Trim();
				current_user.EmergencyPhoneLast1		= emergency_phone_suffix.Text.Trim();
				current_user.PhoneExtension = extension.Text;
                current_user.Nickname					= preferred_name.Text;
				if (current_user.business_unit.allow_cell_edit && current_user.cellphone_number_id == 0 && work_area.Text.Trim() != "")
				{
					// Save new number 
					var cell = new NECellphone_Number
					{
						active_date = DateTime.Now,
						business_unit_id = current_user.business_unit_id,
						number = work_area.Text +"-"+ work_pre.Text +"-"+ work_suffix.Text,
						status = "Active",
						simcard = ""
						
					};
					cell.save();
					current_user.cellphone_number_id = cell.id;
				}
				current_user.save();

				var after			= Toolbox.dict_create(current_user);
				var diff_from	= before.Except(after).ToDictionary(k => k.Key, v => v.Value);
				var diff_to		= after.Except(before).ToDictionary(k => k.Key, v => v.Value);
				email									= Toolbox.dict_dump(diff_from) + "<b>Changed to</b> <br/>"+Toolbox.dict_dump(diff_to);
				lb_saved.ForeColor						= System.Drawing.Color.Green;
				#region Send Email
				if (email != "" && email != "<b>Changed to</b> <br/>")
					{
					try
						{
						var mail = new NeEMail();
						mail.isHTML = true;
						mail.Subject = "Employee information for " + current_user.FullName + " has been updated via FVR";
						if(string.IsNullOrEmpty(current_user.NEEmail))
							{
							current_user.NEEmail			= current_user.FirstName.Substring(0,1)+current_user.LastName+ "@" + Toolbox.app_setting("DomainForEmail");
							}
						mail.From = current_user.NEEmail;
						mail.To = "payroll@" + Toolbox.app_setting("DomainForEmail");
						mail.Body = email;
						mail.Send();
						}
					catch (Exception ee)
						{
						dtl								= new member_fvr_dtl((int) Session["employee_information_dtl_id"]);
						var hist			= new member_fvr_history(dtl.id);
						hist.member_fvr_dtl_id			= dtl.id;
						hist.confirmed					= 0;
						chks.new_information		= hist.confirmed == 1;
						Session["emp_checks"]			= chks;
						hist.save();
                        _tools.catch_error(ee);
						throw;
						}
					} 
				#endregion
				lb_saved.Text					= "Successfully Saved";
				ScriptManager.RegisterStartupScript(this, this.GetType(), this.ClientID, "fvr.client.employee_information.initial_snapshot(true);", true);
				}
			catch (Exception ee)
				{
				lb_saved.ForeColor				= System.Drawing.Color.Red;
				lb_saved.Text					= ee.Message;
				}
			}
		}
}