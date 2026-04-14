using MySql.Data.MySqlClient;
using nesi.core;
using NESI.BLL.Common.Shared;
using NESI.BLL.Core.Employee;
using NESI.BLL.EmbeddedFiles;
using NESI.BLL.EmbeddedResources;
using NESI.Common.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using NESI.Common.Models;
// ReSharper disable InconsistentNaming

namespace NESI.BLL.Pages.Employees
{
    public class NeMemberOffer
	{
		public bool gets_barcodescanner { get; set; }
		public bool gets_businesscards { get; set; }
		public bool gets_directdeposit { get; set; }
		public bool gets_laptop { get; set; }
		public bool gets_neemail { get; set; }
		public bool gets_phone { get; set; }
		public bool gets_phoneext { get; set; }
		public bool gets_vehicle { get; set; }
		public bool isapplicant { get; set; }
		public bool is_salary { get; set; }
		public bool is_signed { get; set; }
		public bool part_time { get; set; }
		public int vendor_id { get; set; }
		public string contract_details { get; set; }
		public DateTime date { get; set; }
		public DateTime enddate { get; set; }
		public DateTime startdate { get; set; }
		public DateTime? benefits_startdate { get; set; }
		public double bonus_amount { get; set; }
		public double bonus_revenue_threshold { get; set; }
		public double bonus_netincome_threshold { get; set; }
		public double bonus_margin_threshold { get; set; }
		public double bonus_netincome_highwater { get; set; }
		public double vacation_amount_1 { get; set; }
		public double vacation_amount_2 { get; set; }
		public double vacation_amount_3 { get; set; }
		public double wage { get; set; }
		public int applicantid { get; set; }
		public int bonus_type { get; set; }
		public int business_id { get; set; }
		public int business_unit_id { get; set; }

		public int enteredby { get; set; }
		public int has_comp { get; set; }
		public int id { get; set; }
		public int memberid { get; set; }
		public int membertypeid { get; set; }
		public int paytype_id { get; set; }
		public int reports_to { get; set; }
		public int vacation_interval_1 { get; set; }
		public int vacation_interval_2 { get; set; }
		public int vacation_interval_3 { get; set; }
		public string comp_details { get; set; }
		public string milestones180 { get; set; }
		public string milestones30 { get; set; }
		public string milestones90 { get; set; }
		public string notes { get; set; }
		public string status { get; set; }
		public int previous_membertype { get; set; }
		public double previous_wage { get; set; }
		public DateTime last_modified { get; set; }
		public NeMemberOffer() { }
		public NeMemberOffer(int _id)
		{
			id = _id;
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM member_offers  WHERE id =@v0", new object[] { _id });
			if (dt.Rows.Count > 0)
			{
				var dr = dt.Rows[0];
				comp_details = dr["comp_details"].ToString();
				business_id = (int)dr["business_unit_id"];
				date = (DateTime)dr["date"];
				enddate = (DateTime)dr["enddate"];
				enteredby = (int)dr["enteredby"];
				gets_laptop = Convert.ToBoolean(dr["gets_laptop"]);
				gets_phone = Convert.ToBoolean(dr["gets_phone"]);
				gets_vehicle = Convert.ToBoolean(dr["gets_vehicle"]);
				gets_neemail = Convert.ToBoolean(dr["gets_neemail"]);
				gets_phoneext = Convert.ToBoolean(dr["gets_phoneext"]);
				gets_businesscards = Convert.ToBoolean(dr["gets_businesscards"]);
				gets_barcodescanner = Convert.ToBoolean(dr["gets_barcodescanner"]);
				gets_directdeposit = Convert.ToBoolean(dr["gets_directdeposit"]);
				has_comp = (int)dr["has_comp"];
				is_salary = Convert.ToBoolean(dr["is_salary"]);
				is_signed = Convert.ToBoolean(dr["is_signed"]);
				memberid = (int)dr["memberid"];
				membertypeid = (int)dr["membertypeid"];
				notes = dr["notes"].ToString();
				reports_to = (int)dr["reports_to"];
				startdate = (DateTime)dr["startdate"];
				status = dr["status"].ToString();

				vacation_amount_1 = (double)dr["vacation_amount_1"];
				vacation_amount_2 = (double)dr["vacation_amount_2"];
				vacation_amount_3 = (double)dr["vacation_amount_3"];
				vacation_interval_1 = (int)dr["vacation_interval_1"];
				vacation_interval_2 = (int)dr["vacation_interval_2"];
				vacation_interval_3 = (int)dr["vacation_interval_3"];
				wage = (double)dr["wage"];

				business_unit_id = (int)dr["business_unit_id"];
				isapplicant = dr["isapplicant"] != DBNull.Value && Convert.ToBoolean(dr["isapplicant"]);
				applicantid = dr["applicantid"] == DBNull.Value ? 0 : (int)dr["applicantid"];
				milestones30 = dr["milestones30"].ToString();
				milestones90 = dr["milestones90"].ToString();
				milestones180 = dr["milestones180"].ToString();
				paytype_id = (int)dr["paytype_id"];
				bonus_type = (int)dr["bonus_type"];
				bonus_amount = (double)dr["bonus_amount"];
				bonus_netincome_threshold = (double)dr["bonus_netincome_threshold"];
				bonus_margin_threshold = (double)dr["bonus_margin_threshold"];
				bonus_revenue_threshold = (double)dr["bonus_revenue_threshold"];
				bonus_netincome_highwater = (double)dr["bonus_netincome_highwater"];
				last_modified = dr["last_modified"] != DBNull.Value ? Convert.ToDateTime(dr["last_modified"]) : DateTime.Now;
				if (dr["benefits_startdate"] != DBNull.Value)
				{
					benefits_startdate = Convert.ToDateTime(dr["benefits_startdate"]);
				}
				part_time = (bool)dr["part_time"];
				vendor_id = (int)dr["vendor_id"];
				contract_details = dr["contract_details"] == DBNull.Value ? "" : (string)dr["contract_details"];
				var previous_accepted = Toolbox.doSQL_int(@"Select ifnull((select id from member_offers " +
														  "where memberid =@v0  and status in('Accepted','Previous')" +
														  " and startdate < @v1 and id != @v2 order by id desc limit 1),0)",
														  new object[]
														  {
															  memberid,startdate, id
														  }
														  );
				var previous_wageid = Toolbox.doSQL_int(@"Select ifnull((select memberwage_id from memberwage 
where memberwage_memberid= @v0 and date < @v1 and currentwage !=0 order by  memberwage_id desc limit 1),0)",
					new object[]
					{
						memberid,startdate
					}
);
				if (previous_wageid != 0)
				{
					var p_wage = new NeWage(previous_wageid.ToString());
					previous_membertype = p_wage.membertype_id;
					previous_wage = p_wage.current_wage;
				}
				else if (previous_accepted != 0)
				{
					var previous_mo = new NeMemberOffer(previous_accepted);
					previous_membertype = previous_mo.membertypeid;
					previous_wage = previous_mo.previous_wage;
				}
				else
				{
					previous_membertype = 0;
					previous_wage = 0;
				}
			}
		}
        
		public void save()
		{
			var _tools = new Toolbox();
			var my_conn = new MySqlConnection();
			my_conn.ConnectionString = _tools.connection_string;
			my_conn.Open();
			var my_comm = new MySqlCommand();
			my_comm.Connection = my_conn;
			if (id == 0)  // insert a new record
			{
				my_comm.CommandText = @"
INSERT INTO member_offers
	(
	comp_details,
	business_unit_id,
	date,
	enddate,
	enteredby,
	gets_laptop,
	gets_phone,
	gets_vehicle,
	gets_neemail,
	gets_phoneext,
	gets_businesscards,
	gets_barcodescanner,
	gets_directdeposit,
	has_comp,
	is_salary,
	memberid,
	membertypeid,
	notes,
	reports_to,
	startdate,
	status,
	vacation_amount_1,
	vacation_amount_2,
	vacation_amount_3,
	vacation_interval_1,
	vacation_interval_2,
	vacation_interval_3,
	wage,
	
	isapplicant,
	applicantid,
	milestones30,
	milestones90,
	milestones180,
	paytype_id,
	bonus_type,
	bonus_amount,
	bonus_netincome_threshold,
	bonus_margin_threshold,
	bonus_revenue_threshold,
	benefits_startdate,
	bonus_netincome_highwater,
	part_time,
	vendor_id,
	contract_details,
	last_modified
	) 
VALUES
	(
	@comp_details,
	@business_unit_id,
	@date,
	@enddate,
	@enteredby,
	@gets_laptop,
	@gets_phone,
	@gets_vehicle,
	@gets_neemail,
	@gets_phoneext,
	@gets_businesscards,
	@gets_barcodescanner,
	@gets_directdeposit,
	@has_comp,
	@is_salary,
	@memberid,
	@membertypeid,
	@notes,
	@reports_to,
	@startdate,
	@status,
	@vacation_amount_1,
	@vacation_amount_2,
	@vacation_amount_3,
	@vacation_interval_1,
	@vacation_interval_2,
	@vacation_interval_3,
	@wage,
	
	@isapplicant,
	@applicantid,
	@milestones30,
	@milestones90,
	@milestones180,
	@paytype_id,
	@bonus_type,
	@bonus_amount,
	@bonus_netincome_threshold,
	@bonus_margin_threshold,
	@bonus_revenue_threshold,
	@benefits_startdate,
	@bonus_netincome_highwater,
	@part_time,
	@vendor_id,
	@contract_details,
	now()
	)";
			}
			else   // update existing record
			{
				my_comm.CommandText = @"
UPDATE
	member_offers
SET
	comp_details = @comp_details,
    business_unit_id=@business_unit_id,
	date = @date,
	enddate = @enddate,
	enteredby = @enteredby,
	gets_laptop = @gets_laptop,
	gets_phone = @gets_phone,
	gets_vehicle = @gets_vehicle,
	gets_neemail = @gets_neemail,
	gets_phoneext = @gets_phoneext,
	gets_businesscards = @gets_businesscards,
	gets_barcodescanner	= @gets_barcodescanner,
	gets_directdeposit = @gets_directdeposit,
	has_comp = @has_comp,
	is_salary = @is_salary,
	memberid = @memberid,
	membertypeid = @membertypeid,
	notes = @notes,
	reports_to = @reports_to,
	startdate = @startdate,
	status = @status,
	vacation_amount_1 = @vacation_amount_1,
	vacation_amount_2 = @vacation_amount_2,
	vacation_amount_3 = @vacation_amount_3,
	vacation_interval_1 = @vacation_interval_1,
	vacation_interval_2 = @vacation_interval_2,
	vacation_interval_3 = @vacation_interval_3,
	wage = @wage,
	
	isapplicant = @isapplicant,
	applicantid = @applicantid,
	milestones30 = @milestones30,
	milestones90 = @milestones90,
	milestones180 = @milestones180,
	paytype_id = @paytype_id,
	bonus_type = @bonus_type,
	bonus_amount = @bonus_amount,
	bonus_netincome_threshold = @bonus_netincome_threshold,
	bonus_margin_threshold = @bonus_margin_threshold,
	bonus_revenue_threshold = @bonus_revenue_threshold,
	benefits_startdate = @benefits_startdate,
	bonus_netincome_highwater = @bonus_netincome_highwater,
	part_time = @part_time ,
	vendor_id = @vendor_id,
	contract_details = @contract_details
WHERE 
	id = @id
LIMIT 1";
			}
			my_comm.Parameters.AddWithValue("@comp_details", comp_details);
			my_comm.Parameters.AddWithValue("@business_unit_id", business_unit_id);
			my_comm.Parameters.AddWithValue("@date", Toolbox.MySQL_shortdt(date));
			my_comm.Parameters.AddWithValue("@enddate", Toolbox.MySQL_shortdt(enddate));
			my_comm.Parameters.AddWithValue("@enteredby", enteredby);
			my_comm.Parameters.AddWithValue("@gets_laptop", gets_laptop);
			my_comm.Parameters.AddWithValue("@gets_phone", gets_phone);
			my_comm.Parameters.AddWithValue("@gets_vehicle", gets_vehicle);
			my_comm.Parameters.AddWithValue("@gets_neemail", gets_neemail);
			my_comm.Parameters.AddWithValue("@gets_phoneext", gets_phoneext);
			my_comm.Parameters.AddWithValue("@gets_businesscards", gets_businesscards);
			my_comm.Parameters.AddWithValue("@gets_barcodescanner", gets_barcodescanner);
			my_comm.Parameters.AddWithValue("@gets_directdeposit", gets_directdeposit);
			my_comm.Parameters.AddWithValue("@has_comp", has_comp);
			my_comm.Parameters.AddWithValue("@is_salary", is_salary);
			my_comm.Parameters.AddWithValue("@memberid", memberid);
			my_comm.Parameters.AddWithValue("@membertypeid", membertypeid);
			my_comm.Parameters.AddWithValue("@notes", notes);
			my_comm.Parameters.AddWithValue("@reports_to", reports_to);
			my_comm.Parameters.AddWithValue("@startdate", Toolbox.MySQL_shortdt(startdate));
			my_comm.Parameters.AddWithValue("@status", status);
			my_comm.Parameters.AddWithValue("@vacation_amount_1", vacation_amount_1);
			my_comm.Parameters.AddWithValue("@vacation_amount_2", vacation_amount_2);
			my_comm.Parameters.AddWithValue("@vacation_amount_3", vacation_amount_3);
			my_comm.Parameters.AddWithValue("@vacation_interval_1", vacation_interval_1);
			my_comm.Parameters.AddWithValue("@vacation_interval_2", vacation_interval_2);
			my_comm.Parameters.AddWithValue("@vacation_interval_3", vacation_interval_3);
			my_comm.Parameters.AddWithValue("@wage", wage);

			my_comm.Parameters.AddWithValue("@isapplicant", isapplicant);
			my_comm.Parameters.AddWithValue("@applicantid", applicantid);
			my_comm.Parameters.AddWithValue("@milestones30", milestones30);
			my_comm.Parameters.AddWithValue("@milestones90", milestones90);
			my_comm.Parameters.AddWithValue("@milestones180", milestones180);
			my_comm.Parameters.AddWithValue("@paytype_id", paytype_id);
			my_comm.Parameters.AddWithValue("@bonus_type", bonus_type);
			my_comm.Parameters.AddWithValue("@bonus_amount", bonus_amount);
			my_comm.Parameters.AddWithValue("@bonus_netincome_threshold", bonus_netincome_threshold);
			my_comm.Parameters.AddWithValue("@bonus_margin_threshold", bonus_margin_threshold);
			my_comm.Parameters.AddWithValue("@bonus_revenue_threshold", bonus_revenue_threshold);
			my_comm.Parameters.AddWithValue("@benefits_startdate", benefits_startdate);
			my_comm.Parameters.AddWithValue("@bonus_netincome_highwater", bonus_netincome_highwater);
			my_comm.Parameters.AddWithValue("@id", id); // 
			my_comm.Parameters.AddWithValue("@part_time", part_time); // 
			my_comm.Parameters.AddWithValue("@vendor_id", vendor_id); // 
			my_comm.Parameters.AddWithValue("@contract_details", contract_details); // 
			try
			{
				my_comm.ExecuteNonQuery();
				if (id == 0)
				{
					my_comm.CommandText = "SELECT LAST_INSERT_ID()";
					id = Convert.ToInt32(my_comm.ExecuteScalar());
				}
			}
			catch (Exception ex)
			{
				_tools.catch_error(ex);
				throw;
			}
			finally
			{
				if (my_conn.State == ConnectionState.Open)
				{
					my_conn.Close();
				}
			}
		}
		public static void create_duplicate_memberoffer_cr(int _old_id, int _new_id)
		{
			Toolbox.doSQL_void(@"INSERT INTO memberoffer_cr 
	(
	memberoffer_crid,
	memberoffer_moid,
	cr_wording,
	annually,
	quarterly,
	weekly,
	as_required,
	daily,
	monthly
	)
SELECT 
	memberoffer_crid,
	@v1,
	cr_wording,
	annually,
	quarterly,
	weekly,
	as_required,
	daily,
	monthly
FROM 
	memberoffer_cr
WHERE
	memberoffer_moid = @v0
", new object[] { _old_id, _new_id });
		}
		public static void create_duplicate_milestones(int _old_id, int _new_id)
		{
			Toolbox.doSQL_void(@"
INSERT INTO memberoffer_milestones 
	(
	milestone,
	addedby,
	offerid,
	added,
	due,
	ticketid,
	notes,
	completed
	)
SELECT 
	milestone,
	addedby,
	@v1,
	NOW(),
	due,
	ticketid,
	notes,
	completed
FROM 
	memberoffer_milestones
WHERE
	offerid = @v0
", new object[] { _old_id, _new_id });
		}

		/// <summary>
		/// This will create, or reinstate the nemember profile
		/// </summary>
		/// <param name="_mo_id"></param>
		/// <param name="_current_user"></param>
		/// <param name="_title"></param>
		public static void go_prelive(int _mo_id, NeMember _current_user, string _title)
		{
			check_current_payperiod();
			using (var conn = Toolbox.connect())
			{
				var error_text = "";
				var mo = new NeMemberOffer(_mo_id);
				var isSubContractor = Toolbox.Contains(mo.paytype_id, new int[] { 4, 5 });
				var mo_m = new NeMember(mo.memberid)
				{
					hrstatus_id = 1,
					Status = "Active",
					TerminateDate = "2099-01-01",
					StartDate = mo.startdate.ToString("yyyy-MM-dd"),
					business_unit_id = mo.business_unit_id,
					payroll_handler = new NeBusinessUnit(mo.business_unit_id).branch_manager.id
				};
				var is_canadian = mo_m.Country == "CDN" || mo_m.Country == "CAN";
				if (isSubContractor)
				{
					mo_m.benefits_startdate = mo.startdate.AddYears(50);
					mo_m.timetostat = 9999;
					mo_m.receive_stat_pay = 0;
				}
				else
				{
					mo_m.timetostat = is_canadian ? 0 : 90;
				}
				mo_m.save();
				var businessUnitObj = new NeBusinessUnit(mo.business_unit_id);
				var taxEntityObj = new NeTaxEntity(businessUnitObj.tax_entity_id);
				error_text = "updating member info";
				#region clear out old termination stuff
				var term_id = Toolbox.doSQL_int(conn, "SELECT IFNULL((SELECT id FROM emp_trm WHERE trm_member_id = @v0),0)", new object[] { mo_m.id });
				if (term_id != 0)
				{
					Toolbox.doSQL_void(conn, "DELETE FROM emp_trm_chklist_lnk WHERE trm_id =@v0 ", new object[] { term_id });
					Toolbox.doSQL_void(conn, "DELETE FROM emp_trm WHERE id  =@v0 ", new object[] { term_id });
				}
				#endregion
				try
				{
					#region update applicant info from the offer
					if (mo.isapplicant)
					{
						try
						{
							var w = new NeWage
							{
								member_id = mo_m.id,
								date = mo.startdate.ToString("yyyy-MM-dd"),
								current_wage = mo.wage,
								date_next_raise = mo.enddate.ToString("yyyy-MM-dd"),
								comment = "Wage set from employee agreement: " + mo.id,
								member_id_audit = Convert.ToInt32(mo.enteredby),
								member_id_added_by = Convert.ToInt32(mo.enteredby),
								business_unit_id = mo_m.business_unit_id,
								bonus_type = Convert.ToInt32(mo.bonus_type),
								bonus_amount = Convert.ToDouble(mo.bonus_amount),
								membertype_id = Convert.ToInt32(mo_m.MemberTypeID)
							};
							w.save();
							error_text = "overwrite wages";
							error_text = "updating app info";
							var app = new NeApplicant(mo.applicantid)
							{
								homephone = mo_m.PhoneAreaCode + mo_m.PhoneFirst + mo_m.PhoneLast,
								address = mo_m.Address,
								cellphone = mo_m.NECellPhoneArea + mo_m.NECellPhoneFirst + mo_m.NECellPhoneLast,
								city = mo_m.City,
								country = mo_m.Country,
								email = mo_m.Email,
								firstname = mo_m.FirstName,
								lastname = mo_m.LastName,
								postal = mo_m.PostalCode,
								province = mo_m.Prov
							};
							app.save();
						}
						catch (Exception ee)
						{
							Toolbox.do_errorLog(ee);
						}
					}
					#endregion
					error_text = "setting base privileges";
					var createpriv = new NEMemberTypePrivilege();
					createpriv.SetBaseLoginPrivilege(mo_m.id.ToString());
					#region collect IT itmes
					send_it_notices(mo, _current_user);
					#endregion
					#region collect HR items
					var items_needed = "";
					if (mo.gets_vehicle) // driving NE Vehicle
					{
						items_needed += "<li>User will be driving a Company vehicle<br/>";
					}
					if (mo.gets_businesscards) // This employee will need business cards
					{
						items_needed += "<li>User needs business cards<br/>";
					}
					if (mo.gets_directdeposit) // This employee would like direct deposit (upload void cheque/check or bank notification)
					{
						items_needed += "<li>User would like direct deposit<br/>";
					}
					var hr_notice = new NeEMail
					{
						To =   EmailID.Payroll + Toolbox.app_setting("DomainForEmail") +";" + EmailID.HR + Toolbox.app_setting("DomainForEmail"),
						From =  EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
						isHTML = true
					};
					if (mo.isapplicant)
					{
						if (items_needed != "")
						{
							hr_notice.Subject = "New User Created, there is information that needs your attention";
							hr_notice.Body = string.Format(@"
The user <b>{0}</b> was set to a(n) {4} by <b>{1}</b> - Member ID ({2}) in the {5} branch.<br/>
<b>Items that need your attention:</b><br/>
<ul>
{3}
</ul><br/>
Their start date is set for: {6}",
								mo_m.FullName,
								_current_user.FullName,
								mo_m,
								items_needed,
								mo_m.membertype.name,
								mo_m.business_unit.name,
								mo.startdate.ToString("yyyy-MM-dd"));
						
						}
						else
						{
							hr_notice.Subject = "New User Created";
							hr_notice.Body = $@"The user <b>{mo_m.FullName}</b> was {_title} by <b>{_current_user.FullName}</b>.<br/> 
												<ul><li>Member-id : {mo_m.id}</li>
													<li>Branch : {mo_m.business_unit.name}</li>
													<li>Hired Date : {mo.startdate.ToString("yyyy-MM-dd")}
												</ul>";
							
						}
						hr_notice.Send();
					}
					#endregion
					#region send email to branch managers
					hr_notice = new NeEMail
					{
						To = "branchmanagers@" + Toolbox.app_setting("DomainForEmail"),
						CC = EmailID.Payroll + Toolbox.app_setting("DomainForEmail") + EmailID.HR +  Toolbox.app_setting("DomainForEmail"),
						From = EmailID.Administrator + Toolbox.app_setting("DomainForEmail")
                    };
					if (mo.isapplicant)
					{
						var this_branch = new NeBusinessUnit(mo_m.business_unit_id);
						if (this_branch.acting_right_hand > 0)
						{
							hr_notice.CC += new NeMember(this_branch.acting_right_hand).NEEmail;
						}
						hr_notice.Subject = "New Hire (" + this_branch.name + ")";
						hr_notice.isHTML = true;
						var membertype = new NeMemberType(mo_m.MemberTypeID);
						hr_notice.Body = string.Format("{0} has been hired as a(n) {1} effective {2:m}", mo_m.FullName, membertype.name, mo_m.StartDate);
						//	hr_notice.Send();
					}
					#endregion
					Toolbox.doSQL_void(conn, @"update member_offers 
set isapplicant = 0, 
memberid = @v0,
status ='Awaiting Start Date'
where status ='Released' and id = @v1",
new object[] { mo_m.id, mo.id }
);
					#region move applicant files over
					if (mo.isapplicant)
					{
						Toolbox.doSQL_void(conn, "update applicants set status ='Hired', becomes_memberid= @v0" + mo_m.id + " where id = @v1", new object[] { mo_m.id, mo.applicantid });
						var appid = Toolbox.doSQL_int(conn, "select ifnull((Select id from applicants where becomes_memberid =@v0 limit 1),0)", new object[] { mo_m.id });
						if (appid != 0)
						{
							try
							{
								var files = new NeFiles();

								files.copy_applicant_to_member(appid, Convert.ToInt32(mo_m.id));
							}
							catch (Exception exxxx)
							{
								var em = new NeEMail
								{
									To = EmailID.Debug  + Toolbox.app_setting("DomainForEmail"),
									Subject = "something is busted when trying to move applicant files to member files",
									Body = error_text + Environment.NewLine + exxxx.ToString(),
									From = EmailID.Admin + Toolbox.app_setting("DomainForEmail")
                                };
								em.Send();
							}
						}
					}
					#endregion
					var master_dtls = new member_fvr_dtl();
					var enteredby = new NeMember(mo.enteredby);
                    var newEmployee = new Employee(mo.memberid);

				    var this_reports_to_obj = new NeMember(newEmployee.ReportToManager.Member_ID);
				    
                    #region Send FVR packet
                    if (!string.IsNullOrEmpty(businessUnitObj.default_fvr_template_ids) || !string.IsNullOrEmpty(taxEntityObj.default_fvr_template_ids))
					{
						var templateIds = new string[] { };
						if (!string.IsNullOrEmpty(businessUnitObj.default_fvr_template_ids))
						{
							templateIds = !businessUnitObj.default_fvr_template_ids.Contains(",")
												? new[] { businessUnitObj.default_fvr_template_ids }
												: businessUnitObj.default_fvr_template_ids.Split(',');
						}
						else if (!string.IsNullOrEmpty(taxEntityObj.default_fvr_template_ids))
						{
							templateIds = !taxEntityObj.default_fvr_template_ids.Contains(",")
												? new[] { taxEntityObj.default_fvr_template_ids }
												: taxEntityObj.default_fvr_template_ids.Split(',');
						}
						foreach (var _template_id in templateIds)
						{
							var templateId = Convert.ToInt32(_template_id);
							var hdr = new member_fvr_template.header(templateId);
							var h = new member_fvr_hdr
							{
								req_member_id = mo.enteredby,
								tabs_needed = string.Join(",", hdr.tabs_needed),
								business_unit_ids = mo.business_unit_id.ToString(),
								active = 1,
								type = "NEWHIRE",
								expire_date = DateTime.Now.Date.AddDays(hdr.days_till_expire)
							};
							h.save();

							var dtls = new ArrayList();
							foreach (member_fvr_template.detail detail in hdr.dtls)
							{
								if (detail.is_selected)
								{
									var d = new member_fvr_dtl
									{
										member_id = mo.memberid,
										tab_index_actual = detail.page_id,
										member_fvr_hdr_id = h.id,
										file_id = detail.file_id,
										url = detail.url,
										upload_required = detail.upload_required ? 1 : 0
									};
									dtls.Add(d);
								}
							}
							master_dtls.mass_insert(dtls);
						}
					}
					if (newEmployee.EmployeeProfile.Member_Email != "" && newEmployee.ReportToManager.member_neemail != "" && newEmployee.ReportToManager.member_fullname != "" && newEmployee.EmployeeProfile.member_hrstatus_id <= 2)  // only send if the emplyee is new or in initial setup
					{
						try
						{
						    #region Send Email

						    SendGoLiveEmail(newEmployee, taxEntityObj, this_reports_to_obj, mo_m);

						    #endregion Send Email
						}
						catch (Exception ee)
						{
							Toolbox.do_errorLog_errorStack(ee);
							throw;
						}
					}
					#endregion
					// wages get set when they actually go live
					// reviews get created when they actually go live
					// 
					if (mo.startdate <= DateTime.Today)
					{
						go_live(mo.id);
					}
				}
				catch (Exception exxx)
				{
					var em = new NeEMail
					{
						To = EmailID.Debug  + Toolbox.app_setting("DomainForEmail"),
						Subject = "something is busted on the go prelive function",
						Body = " Offer :" + _mo_id + " -- " + error_text + Environment.NewLine + exxx,
						From = EmailID.Admin + Toolbox.app_setting("DomainForEmail")
					};
					em.Send();
				}
			}
		}

	    public static void SendGoLiveEmail(Employee newEmployee, NeTaxEntity taxEntityObj, NeMember this_reports_to_obj,
	        NeMember mo_m)
	    {
	        if (newEmployee.ReportToManager.Member_ID > 0)
	        {
	            // Prep Email
	            var onboardNotice = new NeEMail
	            {
	                To = newEmployee.EmployeeProfile.Member_Email,
	                From = newEmployee.ReportToManager.member_neemail,
	                Subject = "Welcome to Spark Power!",
	                isHTML = true,
	                Body = BuildOnboardingBody(newEmployee, this_reports_to_obj, mo_m)
	            };
	            onboardNotice.Send();
	            shared.alert_payroll("FVR packet sent to new hire - " + mo_m.FullName,
	                "Please be advised, " + Toolbox.app_setting("Domain") +  " has just sent an onboarding packet to " + mo_m.FirstName + " via their offer.");
	        }
	    }

	    public static string BuildOnboardingBody(Employee newEmployee, NeMember this_reports_to_obj, NeMember mo_m)
	    {
            string onboardingBody;
	        TemplateParameterFormat format = TemplateParameterFormat.SingleSquareBrace;
            if (!string.IsNullOrEmpty(newEmployee.BusinessUnit.default_onboarding_email))
	        {
	            onboardingBody = newEmployee.BusinessUnit.default_onboarding_email;
	        }
            else if (!string.IsNullOrEmpty(newEmployee.TaxEntity.default_onboarding_email))
	        {
	            onboardingBody = newEmployee.TaxEntity.default_onboarding_email;
            }
	        else
	        {
	            onboardingBody =
	                AssemblyFileLoader.LoadFile<Core.User.User, EmbeddedBLLResourceMarker>("NewNesiCredentials.html");
	            format = TemplateParameterFormat.DoubleBrace;

	        }

            DateTime.TryParse(mo_m.StartDate, out var startDate);
            var isFutureStartDate = true;
            if (startDate < DateTime.Now.Date)
            {
                startDate = DateTime.Now.Date;
                isFutureStartDate = false;
            }
            
	        var token = HttpUtility.UrlEncode(
	            UserManager.GetPasswordResetToken(newEmployee, startDate.AddDays(isFutureStartDate ? 1 : 14)));
	        var dict = new Dictionary<string, object>
	        {
	            {"USERNAME", newEmployee.UserName},
	            {"REPORTSTO", this_reports_to_obj.FullName},
	            {"REPORTSTOCELL", this_reports_to_obj.NECellPhoneNumber},
	            {"PUBLICNAME", newEmployee.TaxEntity.public_name},
	            {"FIRSTNAME", mo_m.FirstName},
	            {"LASTNAME", mo_m.LastName},
	            {"FULLNAME", mo_m.FullName},
	            {"REPORTSTOEMAIL", this_reports_to_obj.NEEmail},
	            {"MEMBERTYPENAME", mo_m.membertype.name},
	            {"token", token},
	            {"hostname", Configuration.HostName},
	        };
	        onboardingBody = TemplateParser.Parse(onboardingBody, dict, format, TemplateParseValidationMode.EnsureAllTokensFound);
	        return onboardingBody;
	    }

	    #region collect IT itmes
		public static void send_it_notices(NeMemberOffer mo, NeMember current_user)
		{
			var mo_m = new NeMember(mo.memberid);
			var mt = new NeMemberType(mo.membertypeid);
		    var items_needed = "";
			if (mo.gets_neemail) // email address
			{
				items_needed += "<li>User needs an email address setup<br/>";
			}
			if (mo.gets_phone) // cell phone
			{
				items_needed += "<li>User needs a cell phone<br/>";
			}
			if (mo.gets_phoneext) // phone extension
			{
				items_needed += "<li>User needs an altigen phone extension<br/>";
			}
			if (mo.gets_barcodescanner) // phone extension
			{
				items_needed += "<li>User needs a barcode scanner<br/>";
			}
			if (items_needed != "")
			{
				var it_notice = new NeEMail();
				it_notice.To = EmailID.Help + Toolbox.app_setting("DomainForEmail");
				it_notice.From = EmailID.Administrator +Toolbox.app_setting("DomainForEmail");
				//	it_notice.Bcc = "aketelaars@newelectric.com";
				if (mo.isapplicant)
				{
					it_notice.Subject = "New User Created (or old user reinstated), there is information that needs your attention";
				}
				else
				{
					it_notice.Subject = "Check " + mo_m.FullName + "'s IT settings in "+ Toolbox.app_setting("Domain") + "  They just got a new employment agreement";
				}
				it_notice.isHTML = true;
				it_notice.Body = string.Format("The user <b>{0}</b> was set to a(n) {5} by <b>{1}</b> - Member ID ({2}).<br/>The start date is: {4}<br/><b>Items that need your attention:</b><br/><ul>{3}</ul>", mo_m.FullName, current_user.FullName, mo_m.id, items_needed, mo_m.StartDate, mt.name);
				it_notice.Send();
			}
		}
		#endregion
		public static void check_current_payperiod()
		{
			// Check if the user's branch is mid-payroll... if so throw an exception.
			var payperiod = new NePayPeriod();
			var current_payperiod_id = payperiod.CurrentPayPeriod();
			payperiod = new NePayPeriod(current_payperiod_id);
			if (DateTime.Now > payperiod.end_date) // This means that the current date/time is greater than the current pay period's enddate
			{
				throw new Exception("An offer cannot be pushed forward while a pay period is being approved");
			}
		}
		/// <summary>
		/// This requires the nemember profile already be set up.
		/// </summary>
		/// <param name="_mo_id"></param>
		public static void go_live(int _mo_id)
		{
			check_current_payperiod();
			using (var conn = Toolbox.connect())
			{
				var error_text = "load classes";
				try
				{
					var mo = new NeMemberOffer(_mo_id);
					var employee = new NeMember(mo.memberid);
					var prev_employee = new NeMember(mo.memberid);
					var isPastOrPending = employee.hrstatus_id == 4 || employee.hrstatus_id == 5;
					if (isPastOrPending)
					{
						go_prelive(_mo_id, new NeMember(mo.enteredby), "reinstated");
						return;
					}


					employee = new NeMember(mo.memberid);
					var prev_branch = prev_employee.business_unit;
					var curr_branch = new NeBusinessUnit(mo.business_unit_id);
					// We need to fix existing OPEN work orders that haven't hit approval
					//if (prev_employee.MemberTypeID != mo.membertypeid && prev_employee.business_unit_id == mo.business_unit_id)
					//{
                    //    NeMember.HasZeroChargeouts(mo.memberid, prev_employee.MemberTypeID, mo.membertypeid, employee, curr_branch);
//						var rowsToUpdate = Toolbox.doSQL_dt(conn, @"
//SELECT 
//	a.wo_detail_current_id id,
//	IFNULL(d.id,0) new_chargeoutid,
//	IFNULL(d.chargeout, 0.0) new_chargeout
//FROM 
//	wo_detail_current a 
//LEFT JOIN 
//	woprog b ON a.wo_detail_current_woprog_id = b.woprog_id 
//LEFT JOIN 
//	membertype_chargeout c ON c.id = a.wo_detail_current_master_id 
//LEFT JOIN
//	membertype_chargeout d ON d.business_unit_id = a.business_unit_id AND d.membertype_id = @v2 AND d.paytype_id = a.paytypeid
//WHERE 
//	b.woprog_status IN ('Open', 'Initial Prep', 'Questions for PM', 'Rework') AND 
//	a.wo_detail_current_type = 'L' AND 
//	a.memberid = @v0 AND
//	c.membertype_id = @v1
//	", new object[] { mo.memberid, prev_employee.MemberTypeID, mo.membertypeid });
//						var hasZeroChargeouts = false;
//						foreach (DataRow dr in rowsToUpdate.Rows)
//						{
//						    var newChargeoutId = Convert.ToInt32(dr["new_chargeoutid"]);
//							var newChargeout = Convert.ToDouble(dr["new_chargeout"]);
//							// update row to new master id & code to new chargeout id, price sell & price_unit to new chargeout price... don't touch cost
//							if (newChargeout == 0 || newChargeoutId == 0)
//							{
//								hasZeroChargeouts = true;
//							}
//						}


//						if (hasZeroChargeouts)
//						{
//							var newMemberType = new NeMemberType(mo.membertypeid);
//							var emailNotification = new NeEMail();
//							emailNotification.To = curr_branch.branch_manager.NEEmail;
//							emailNotification.From = "noreply@" + Toolbox.app_setting("DomainForEmail");
//							emailNotification.CC = "hr@" + Toolbox.app_setting("DomainForEmail");
//							emailNotification.Bcc = "mhyde@" + Toolbox.app_setting("DomainForEmail");
//							emailNotification.Subject = "Blank chargeout(s) detected, in your business unit, for member type '" + newMemberType.name + "'";
//							emailNotification.Body = string.Format(@"
//While {0}'s employee profile was being updated via their offer, it was noticed that the chargeouts were not set up fully for their new member type - '{1}' in your business unit ({2}). 
//Please see that these new chargeouts are setup as soon as possible, until then this employee cannot move to their new member type.", employee.FullName, newMemberType.name, curr_branch.ddl_name);
//							emailNotification.Send();
//							throw new Exception("Not proceeding. Blank chargeout detected while trying to move to the new member type... An email has been sent to " + curr_branch.branch_manager.FullName + " to correct this.");
//						}
//						else
//						{
//							foreach (DataRow dr in rowsToUpdate.Rows)
//							{
//								var rowId = Convert.ToInt32(dr["id"]);
//								var newChargeoutId = Convert.ToInt32(dr["new_chargeoutid"]);
//								var newChargeout = Convert.ToDouble(dr["new_chargeout"]);
//								// update row to new master id & code to new chargeout id, price sell & price_unit to new chargeout price... don't touch cost
//								Toolbox.doSQL_void(conn, @"
//UPDATE 
//	wo_detail_current 
//SET 
//	wo_detail_current_date_modified = wo_detail_current_date_modified, 
//	wo_detail_current_master_id = @v0, 
//	wo_detail_current_code = @v0, 
//	wo_detail_current_price_sell = @v1, 
//	wo_detail_current_price_unit = @v1 
//WHERE 
//	wo_detail_current_id = @v2 
//LIMIT 1", new object[] { newChargeoutId, newChargeout, rowId });
//							}
//						}
	//}


					error_text = "load HR people";
					var first_hr_member = Toolbox.doSQL_int(conn, "Select get_lowest_level_with_privilege(37,11)", null);
					if (first_hr_member != 0)
					{
						error_text = "add to todo helper";
						NeMember.to_do.add(first_hr_member, DateTime.Now, "Update ADP with new payroll data", "/#/opens/127/employees/" + employee.id, "payroll", employee.id, conn);
					}
					error_text = "populate member fields with member offer fields";
					var em = new NeEMail();
					#region member static info
					employee.business_unit_id = mo.business_unit_id;
					if (prev_branch.id != employee.business_unit_id)
					{
						// Changing business units, need to send a notice to payroll@ and HR@
						shared.alert_payroll("Business Unit Change for " + employee.FullName, string.Format("Per an accepted offer for {0}, their Business Unit has been changed from '{1}' to '{2}' effective immediately", employee.Nickname, prev_branch.name, curr_branch.name));
						shared.alert_hr("Business Unit Change for " + employee.FullName, string.Format("Per an accepted offer for {0}, their Business Unit has been changed from '{1}' to '{2}' effective immediately", employee.Nickname, prev_branch.name, curr_branch.name));
					
					}
					employee.paytype_id = mo.paytype_id;
					employee.MemberTypeID = mo.membertypeid;
					employee.paytype_id = mo.paytype_id;
					employee.gets_barcodescanner = mo.gets_barcodescanner;
					employee.gets_businesscards = mo.gets_businesscards;
					employee.gets_directdeposit = mo.gets_directdeposit;
					employee.gets_laptop = mo.gets_laptop;
					employee.gets_neemail = mo.gets_neemail;
					employee.gets_phone = mo.gets_phone;
					employee.gets_phoneext = mo.gets_phoneext;
					employee.gets_vehicle = mo.gets_vehicle;
					employee.comp_details = mo.comp_details.Length > 2400 ? mo.comp_details.Substring(0, 2400) : mo.comp_details;
					employee.offer_notes = mo.notes;
					#region add to todo list if vacations stuff has changed
					if (
						employee.vacation_amount_1 != Convert.ToDecimal(mo.vacation_amount_1) ||
						employee.vacation_amount_2 != Convert.ToDecimal(mo.vacation_amount_2) ||
						employee.vacation_amount_2 != Convert.ToDecimal(mo.vacation_amount_2) ||
						employee.vacation_interval_1 != mo.vacation_interval_1 ||
						employee.vacation_interval_2 != mo.vacation_interval_2 ||
						employee.vacation_interval_3 != mo.vacation_interval_3
					)
					{
						error_text = "add to todo helper";
						if (first_hr_member != 0)
						{
							NeMember.to_do.add(first_hr_member, DateTime.Now, "Update ADP with new Vacation Settings", "/#/opens/127/employees/" + employee.id, "payroll", employee.id, conn);
						}
					}
					// These are percents, and need to be 0.08 or 0.06... not whole numbers
					var country = employee.business_unit.country;
					var is_canadian = country == "CDN" || country == "CAN";
					employee.vacation_amount_1 = mo.vacation_amount_1 > 1 && is_canadian ? Convert.ToDecimal(1 / mo.vacation_amount_1) : Convert.ToDecimal(mo.vacation_amount_1);
					employee.vacation_amount_2 = mo.vacation_amount_2 > 1 && is_canadian ? Convert.ToDecimal(1 / mo.vacation_amount_2) : Convert.ToDecimal(mo.vacation_amount_2);
					employee.vacation_amount_3 = mo.vacation_amount_3 > 1 && is_canadian ? Convert.ToDecimal(1 / mo.vacation_amount_3) : Convert.ToDecimal(mo.vacation_amount_3);
					employee.vacation_interval_1 = mo.vacation_interval_1;
					employee.vacation_interval_2 = mo.vacation_interval_2;
					employee.vacation_interval_3 = mo.vacation_interval_3;
					#endregion
					employee.reports_to = mo.reports_to;
					employee.benefits_startdate = mo.benefits_startdate ?? mo.startdate.AddMonths(3);
					if (mo.paytype_id > 3 && mo.paytype_id != 7) // is it a subcontractor?
					{
						employee.benefits_startdate = mo.startdate.AddYears(50);
						employee.timetostat = 9999;
						employee.receive_stat_pay = 0;
					}
					else
					{
						employee.timetostat = is_canadian ? 0 : 90;
						employee.receive_stat_pay = 1;
					}
					employee.part_time = mo.part_time;
					error_text = "save member";
					employee.save();
					#endregion
					error_text = "load wage stuff";
					mo.status = "Accepted";
					#region update wage
					var old_wage = new NeWage(employee.id).current_wage;
					var new_wage = mo.wage;
					var w = new NeWage
					{
						member_id = employee.id,
						date = mo.startdate.ToString("yyyy-MM-dd"),
						current_wage = mo.wage,
						date_next_raise = mo.enddate.ToString("yyyy-MM-dd"),
						comment = "Wage change from employee agreement: " + mo.id,
						member_id_audit = mo.enteredby,
						member_id_added_by = mo.enteredby,
						business_unit_id = mo.business_unit_id,
						bonus_type = mo.bonus_type,
						bonus_amount = mo.bonus_amount,
						membertype_id = Convert.ToInt32(employee.MemberTypeID)
					};
					error_text = "overwrite wages";
					w.save();
					#endregion
					#region Capture Title Changes
					var TitleChangeNotification = "";
					if(prev_employee.MemberTypeID != employee.MemberTypeID)
					{
					
						TitleChangeNotification = @"<tr>
					<td nowrap='nowrap'>
						<b>Old Title:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + NeMemberType.get_membertypename(prev_employee.MemberTypeID) + @"</td>
				</tr><tr>
					<td nowrap='nowrap'>
						<b>New Title:</b></td>
					<td nowrap='nowrap' width='100%'>
						" +  NeMemberType.get_membertypename(employee.MemberTypeID) + @"</td>
				</tr>";
					}
                    #endregion Capture Title Changes
                    error_text = "prepare email";
					em.Subject = "Employment agreement for " + employee.FullName + " has become active as of " + mo.startdate.ToLongDateString();
					em.isHTML = true;
					var str_body = @"<table style='width: 100%; font-family: Arial; font-size: small;'>
				<tr>
					<td bgcolor='Red' colspan='2'>
						Employment Agreement IN EFFECT</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Employee:</b></td>
					<td nowrap='nowrap' width='100%'>
						<a href='" + Toolbox.app_setting("Domain") + "/#/opens/127/employees/" + employee.id + "'>" + employee.FullName2 + @"  (" + employee.id + @")</a>
					</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Branch:</b></td>
					<td nowrap='nowrap' width='100%'>
						" +  curr_branch.name + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Agreement:</b></td>
					<td nowrap='nowrap' width='100%'>
						<a href='" + Toolbox.app_setting("Domain") + "/sections/hr/member/member_offer.aspx?id=" + mo.id + "'>" + mo.id + @"</a>
					</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Agreement Start Date:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + mo.startdate.ToLongDateString() + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Previous Branch?:</b></td>
					<td nowrap='nowrap' width='100%'>
					" + (curr_branch.id != prev_branch.id ? prev_branch.name : " -- ") + @"
					</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Previous Reports To:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + (prev_employee.reports_to > 0 ? new NeMember(prev_employee.reports_to).FullName : " -- ") + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Current Reports To:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + (employee.reports_to > 0 ? new NeMember(employee.reports_to).FullName : " -- ") + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Agreement End Date:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + mo.enddate.ToLongDateString() + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Wage Was:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + old_wage.ToString("C2") + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>New Wage:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + new_wage.ToString("C2") + @"</td>
				</tr>
					" +TitleChangeNotification+ @"
			</table>";

					em.Body = str_body;
					Toolbox.doSQL_void(conn, @"update member_offers 
set status ='Previous' where status ='Accepted' and memberid =@v0
and isapplicant=0 and id != @v1
AND startdate < @v2",
new object[] {
mo.memberid, mo.id, Toolbox.MySQL_shortdt(mo.startdate)});
					Toolbox.doSQL_void(conn, @"update member_offers set status ='Accepted' where id = @v0", new object[] { mo.id });


					#region Clear out old reviews
					error_text = "close all other offers";
					error_text = "close all old reviews";
					// delete any other unfinished reviews
					Toolbox.doSQL_void(conn, @"update emp_review 
INNER JOIN member_offers ON emp_review.offerid = member_offers.id AND (member_offers.`status` = 'Previous' or member_offers.`status` = 'Closed') 
set emp_review.`status`='Closed' WHERE emp_review.`status` = 'In Development' and emp_review.member_id =@v0", new object[] { mo.memberid });



					#endregion

					#region create review into the future

					error_text = "Create new review";
					NeEmpReview.create_review(mo.id, mo.enddate.AddDays(-30), mo.reports_to);

					#endregion
					error_text = "save offer status";
					mo.save();
					
					em.To = EmailID.Payroll  + Toolbox.app_setting("DomainForEmail");
					em.Bcc = EmailID.HR +Toolbox.app_setting("DomainForEmail");
					if (employee.reports_to != 0)
					{
						em.To = new NeMember(employee.reports_to).NEEmail;
						em.CC = EmailID.Payroll + Toolbox.app_setting("DomainForEmail");
					}
					em.From = EmailID.Admin + Toolbox.app_setting("DomainForEmail");
					error_text = "send out email";
					em.Send();
				}
				catch (Exception exxx)
				{
					var em = new NeEMail
					{
						To = EmailID.Debug + Toolbox.app_setting("DomainForEmail"),
						
						Subject = "something is busted on the auto offer update",
						Body = "member_offer_id:" + _mo_id + Environment.NewLine + error_text + Environment.NewLine + exxx.ToString(),
						From = EmailID.Admin + Toolbox.app_setting("DomainForEmail")
                    };
					em.Send();
				}
			}
		}
    }
	/// <summary>
	/// Summary description for Memberoffer_Milestones
	/// </summary>
	public class Memberoffer_Milestones
	{
		private readonly Toolbox _tools = new Toolbox();
		public int id { get; set; }
		public int addedby { get; set; }
		public int ticketid { get; set; }
		public int offerid { get; set; }
		public string milestone { get; set; }
		public string notes { get; set; }
		public DateTime due { get; set; }
		public DateTime added { get; set; }
		public Memberoffer_Milestones() { }
		public Memberoffer_Milestones(int _id)
		{
			id = _id;
			var dt = _tools.getSQL_datatable(@"Select * from memberoffer_milestones  where id =@v0", new object[] { id });
			if (dt.Rows.Count > 0)
			{
				var dr = dt.Rows[0];
				added = (DateTime)dr["added"];
				addedby = (int)dr["addedby"];
				due = (DateTime)dr["due"];
				ticketid = (int)dr["ticketid"];
				milestone = (string)dr["milestone"];
				notes = dr["notes"] != DBNull.Value ? (string)dr["notes"] : "";
				offerid = (int)dr["offerid"];
			}
		}
		public void save()
		{
			var _tools = new Toolbox();
			var my_conn = new MySqlConnection();
			my_conn.ConnectionString = _tools.connection_string;
			my_conn.Open();
			var my_comm = new MySqlCommand();
			my_comm.Connection = my_conn;
			my_comm.CommandText = id == 0
				? @"
INSERT INTO memberoffer_milestones 
	(
	added,
	addedby,
	due,
	ticketid,
	milestone,
	notes,
	offerid
	) 
VALUES
	(
	CURDATE(),
	@addedby,
	@due,
	@ticketid,
	@milestone,
	@notes,
	@offerid
	)"
				: @"
UPDATE 
	memberoffer_milestones 
SET 
	due       = @due,
	ticketid  = @ticketid,
	milestone = @milestone,
	notes     = @notes,
	offerid   = @offerid
WHERE 
	id = @id
LIMIT 1";
			my_comm.Parameters.AddWithValue("@addedby", addedby);
			my_comm.Parameters.AddWithValue("@due", Toolbox.MySQL_shortdt(due));
			my_comm.Parameters.AddWithValue("@ticketid", ticketid);
			my_comm.Parameters.AddWithValue("@milestone", milestone);
			my_comm.Parameters.AddWithValue("@notes", notes);
			my_comm.Parameters.AddWithValue("@offerid", offerid);
			my_comm.Parameters.AddWithValue("@id", id);
			try
			{
				my_comm.ExecuteNonQuery();
				if (id == 0)
				{
					my_comm.CommandText = "SELECT LAST_INSERT_ID()";
					id = Convert.ToInt32(my_comm.ExecuteScalar());
				}
			}
			catch (Exception ex)
			{
				_tools.catch_error(ex);
				throw;
			}
			finally
			{
				if (my_conn.State == ConnectionState.Open)
				{
					my_conn.Close();
				}
			}
		}
		public DataTable get_milestones(int _offerid)
		{
			var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM memberoffer_milestones WHERE offerid = @v0", _offerid);
			if (c == 0)
			{
				return null;
			}
			return Toolbox.doSQL_dt(@"SELECT milestone, addedby, due, ticketid, notes FROM memberoffer_milestones WHERE offerid = @v0  ORDER BY due", new object[] { _offerid });
		}
	}
}