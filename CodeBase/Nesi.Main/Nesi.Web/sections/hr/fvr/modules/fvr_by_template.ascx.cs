using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using DevExpress.Web;
using nesi.core;
using NESI.Common.Exceptions;

public partial class sections_hr_fvr_modules_fvr_by_template : System.Web.UI.UserControl
{
	Toolbox _tools;
	NeMember current_user = new NeMember();
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(1);
	    ds_users.SelectCommand =
	        @"SELECT a.member_id id, CONCAT('(', b.ddl_name, ') - ', a.member_fullname) name FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE a.member_status = 'Active' and a.member_id != 1316 and b.id in(" +
	        new Current_User().visible_business_units + ") ORDER BY b.ddl_name, a.member_lastname, a.member_nickname";

	    }
	protected void Page_Load(object sender, EventArgs e)
	{

	}
	protected void cbp_templatesave_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		cbp_templatesave.JSProperties["cpSaved"] = "";
		try
		{
			if (e.Parameter == "save")
			{
				var users = new List<int>();
				foreach (ListEditItem li in list_employees.SelectedItems)
				{
					users.Add(Convert.ToInt32(li.Value));
				}
				var template_id = Convert.ToInt32(combo_templates.Value);
				var hdr = new member_fvr_template.header(template_id);
				var list_types = new List<string>() { "", "NEWHIRE", "EXIT", "RENEW" };
				var dt_users = Toolbox.doSQL_dt(string.Format(@"SELECT DISTINCT(business_unit_id) business_unit_id 
FROM member WHERE member_id IN ({0})", string.Join(",", users)), null);

				var business_unit_ids = new List<int>();
				foreach (DataRow dr_user in dt_users.Rows)
				{
					business_unit_ids.Add(Convert.ToInt32(dr_user["business_unit_id"]));
				}

			var h = new member_fvr_hdr
				        {
				        req_member_id     = current_user.id,
				        tabs_needed       = string.Join(",", hdr.tabs_needed),
				        business_unit_ids = string.Join(",", business_unit_ids),
				        active            = 1,
				        type              = list_types[hdr.type_id],
				        expire_date       = DateTime.Now.Date.AddDays(hdr.days_till_expire)
				        };
			h.save();

				var dt_user_details = Toolbox.doSQL_dt(string.Format(@" SELECT a.member_id, COUNT(b.id) c, a.Member_Email email, 
a.member_user user, a.member_pass pass, a.reports_to reports_to_id, a.member_hrstatus_id hrstatus_id, IFNULL(c.member_neemail, '') reports_to_email,
IFNULL(c.member_fullname, '') reports_to_name, IFNULL(d.number, '') reports_to_cell, a.business_unit_id
FROM member a LEFT JOIN member_fvr_dtl b ON a.member_id = b.member_id LEFT 
JOIN member c ON a.reports_to = c.member_id LEFT JOIN cellphone_number d 

ON c.cellphone_number_id = d.id WHERE a.member_id IN ({0}) GROUP BY a.member_id ", string.Join(",", users)), null);

				var dtls = new ArrayList();
				var master_dtls = new member_fvr_dtl();
				var list_sent_emails = new List<int>();
				foreach (var u in users)
				{
					var dr_userdetail = dt_user_details.Select("member_id = " + u)[0];
					var this_employee = new NeMember(u);
					var c = Convert.ToInt32(dr_userdetail["c"]);
					var this_email = dr_userdetail["email"].ToString();
					var this_reports_to_email = dr_userdetail["reports_to_email"].ToString();
					var this_user = dr_userdetail["user"].ToString();
					var this_pass = dr_userdetail["pass"].ToString();
					var this_reports_to = Convert.ToInt32(dr_userdetail["reports_to_id"]);
					var this_hrstatus_id = Convert.ToInt32(dr_userdetail["hrstatus_id"]);
					var this_reports_to_name = dr_userdetail["reports_to_name"].ToString();
					var this_reports_to_cell = dr_userdetail["reports_to_cell"].ToString();
                    var this_business_unit = new NeBusinessUnit(dr_userdetail["business_unit_id"]);
					foreach (member_fvr_template.detail detail in hdr.dtls)
					{
						if (detail.is_selected)
						{
							var d = new member_fvr_dtl();
							d.member_id = (int)u;
							if (c == 0 && hdr.type_id == 1 && !list_sent_emails.Contains(u))  // if an FVR has never been sent, type 1 is NEW, type 3 is RENEW
							{
								if (this_email != "" && this_reports_to_email != "" && this_reports_to_name != "" && this_hrstatus_id <= 2 && !this_employee.sent_welcome_email)  // only send if the emplyee is new or in initial setup
								{
									try
									{
										#region Send Email
										if (this_reports_to > 0)
										{
										    throw new NesiException();
											var hr_notice = new NeEMail();
											hr_notice.To = this_email;
											hr_notice.From = "noreply@" + Toolbox.app_setting("DomainForEmail");
											hr_notice.CC = this_reports_to_email + ";" + current_user.NEEmail;
											hr_notice.Subject = "Welcome to Spark Power Corp!";
											hr_notice.isHTML = true;
											hr_notice.Body = string.Format(@"
		<div style='font-family:arial;font-size:12px;'>
		You have been given login credentials for our company intranet at" + Toolbox.app_setting("Domain") + @" </br></br>
		<b>Username:</b> {0}</br>
		<b>Password:</b> {1}</br>
		</br>
		Upon logging on the first time, you will be required to change your password. <br/>
		You should also see a page outlining our onboarding package.   <br/>
		This should be completed either before or on your first day.   <br/>
		We welcome you to " + new NeTaxEntity(this_business_unit.tax_entity_id).public_name + @" and look forward to working with you!</br>
		</br>
		If you have any questions, please do not hestiate to contact:<br />
		<b>{2}</b> at <b>{3}</b>.
		</div>
		",
												this_user,              // {0}
												this_pass,              // {1}
												this_reports_to_name,   // {2}
												this_reports_to_cell != "" ? this_reports_to_cell : this_reports_to_email   // {3}
												);
											hr_notice.Send();
											this_employee.sent_welcome_email = true;
											this_employee.save();
										}
										#endregion Send Email
									}
									catch (Exception ee)
									{
										Toolbox.do_errorLog_errorStack(ee);
										throw;
									}
								}
								list_sent_emails.Add(u);
							}
							d.tab_index_actual = detail.page_id;
							d.member_fvr_hdr_id = h.id;
							d.file_id = detail.file_id;
							d.url = detail.url;
							d.upload_required = detail.upload_required ? 1 : 0;
							dtls.Add(d);
						}
					}
				}
				master_dtls.mass_insert(dtls);
				//list_employees.UnselectAll();
				combo_templates.SelectedIndex = -1;
				cbp_templatesave.JSProperties["cpSaved"] = "true";
			}
		}
		catch (Exception ee)
		{
			cbp_templatesave.JSProperties["cpSaved"] = ee.ToString();
		}
	}

}