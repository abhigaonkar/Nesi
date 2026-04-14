using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using MySql.Data.MySqlClient;
using nesi.core.print;

namespace nesi.core
{
	/// <summary>
	/// Summary description for member offer
	/// </summary>
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
				comp_details = (string)dr["comp_details"];
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
				notes = (string)dr["notes"];
				reports_to = (int)dr["reports_to"];
				startdate = (DateTime)dr["startdate"];
				status = (string)dr["status"];
				vacation_amount_1 = (double)dr["vacation_amount_1"];
				vacation_amount_2 = (double)dr["vacation_amount_2"];
				vacation_amount_3 = (double)dr["vacation_amount_3"];
				vacation_interval_1 = (int)dr["vacation_interval_1"];
				vacation_interval_2 = (int)dr["vacation_interval_2"];
				vacation_interval_3 = (int)dr["vacation_interval_3"];
				wage = (double)dr["wage"];
			
				business_unit_id = (int)dr["business_unit_id"];
				isapplicant = dr["isapplicant"] == DBNull.Value ? false : Convert.ToBoolean(dr["isapplicant"]);
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
		public static object fill_report(emp_offer report, int moid, int jd)
		{
			var fname = "";
			var lblwearepleased = report.FindControl("lblwearepleased", true) as XRLabel;
			var mo = new NeMemberOffer(moid);
			NeApplicant app;
			NeMember mem;
			NeBusinessUnit comp;
            NeMemberType mt;
			var xrLabel18 = report.FindControl("xrLabel18", true) as XRLabel;
			report.Parameters[0].Value = Convert.ToInt32(mo.id);
			var lbl_hdr = report.FindControl("lblheader", true) as XRLabel;
			var lbl_dear_name = report.FindControl("lbl_dear_name", true) as XRLabel;
			var lblname1 = report.FindControl("lblname1", true) as XRLabel;
			var xrLabel1 = report.FindControl("xrLabel1", true) as XRLabel;
			var xrLabel19 = report.FindControl("xrLabel19", true) as XRLabel;
			var xrLabel5 = report.FindControl("xrLabel5", true) as XRLabel;
			var xrLabel2 = report.FindControl("xrLabel2", true) as XRLabel;
			var xrLabel9 = report.FindControl("xrLabel9", true) as XRLabel;
			var xrLabel10 = report.FindControl("xrLabel10", true) as XRLabel;
			var lbl_termination = report.FindControl("xrLabel23", true) as XRLabel;
			var xrLabel22 = report.FindControl("xrLabel22", true) as XRLabel;
			var xrLabel16 = report.FindControl("xrLabel16", true) as XRLabel;
			var xrLabel13 = report.FindControl("xrLabel13", true) as XRLabel;
			var xrLabel11 = report.FindControl("xrLabel11", true) as XRLabel;
			var xrLabel14 = report.FindControl("xrLabel14", true) as XRLabel;
			var xrLabel21 = report.FindControl("xrLabel21", true) as XRLabel;
			var xrLabel12 = report.FindControl("xrLabel12", true) as XRLabel;
			var xrLabel8 = report.FindControl("xrLabel8", true) as XRLabel;
			var xrLabel7 = report.FindControl("xrLabel7", true) as XRLabel;
			var xrLabel6 = report.FindControl("xrLabel6", true) as XRLabel;
			var xrLabel25 = report.FindControl("xrLabel25", true) as XRLabel;
			var xrLabel15 = report.FindControl("xrLabel15", true) as XRLabel;
			var lblwage = report.FindControl("lblwage", true) as XRLabel;
			var lblcomp = report.FindControl("lblcomp", true) as XRLabel;
			var lblcomp_final = report.FindControl("lblcomp_final", true) as XRLabel;
			var rt_vacation = report.FindControl("rt_vacation", true) as XRLabel;
			var lbl_benefits = report.FindControl("lbl_benefits", true) as XRLabel;
			var lblsigapplicant = report.FindControl("lblsigapplicant", true) as XRLabel;
			var lblsigreports = report.FindControl("lblsigreports", true) as XRLabel;
			var lblsigcompany = report.FindControl("lblsigcompany", true) as XRLabel;
			var pnl_member = report.FindControl("pnl_member", true) as XRPanel;
			var xrPanel1 = report.FindControl("xrPanel1", true) as XRPanel;
			var sig_prelude = report.FindControl("sig_prelude", true) as XRRichText;
			var lbladd1 = report.FindControl("lbladd1", true) as XRLabel;
			var lbladd2 = report.FindControl("lbladd2", true) as XRLabel;
			var xrSubreport_milestones = report.FindControl("xrSubreport_milestones", true) as XRSubreport;
			var detailband = report.FindControl("Detail", true) as DetailBand;
			var xrlbl_governinglaw = report.FindControl("xrlbl_governinglaw", true) as XRLabel;
			var pnl_comp = report.FindControl("pnl_comp", true) as XRPanel;
			var pnl_terms = report.FindControl("pnl_terms", true) as XRPanel;
			var lbl_milestone_header = report.FindControl("lbl_milestone_header", true) as XRLabel;
		    var xrlogo = report.FindControl("xrPictureBox1", true) as XRPictureBox;
			var is_promotion = false;
			var is_wage_increase = false;
			var is_exec_package = 0;
			//		var is_spark = mo.business_id == 10;
			if (mo.reports_to != 0)
			{
				//		lblsigreports.Text = new NeMember((int) mo.reports_to).FullName;
				//		lblsigcompany.Text = new NeMember((int) mo.reports_to).membertype.name;
			}
			lbl_benefits.Text = "";
			xrLabel2.Text = mo.date.ToString("dddd, MMMM dd, yyyy");
			var part_time = mo.part_time ? " part time " : " full time ";
			if (jd == 1)  // if its a job description only
			{
				if (xrPanel1 != null)
				{
					xrPanel1.Visible = false;
				}
				lblwage.Visible = false;
				lbl_benefits.Visible = false;
				rt_vacation.Visible = false;
				detailband.Visible = false;
				xrLabel13.Visible = false;
				xrLabel5.Visible = false;
				lbl_termination.Visible = false;
				xrLabel22.Visible = false;
				xrLabel16.Visible = false;
				xrLabel13.Visible = false;
				xrLabel11.Visible = false;
				xrLabel14.Visible = false;
				xrLabel21.Visible = false;
				xrLabel12.Visible = false;
				xrLabel8.Visible = false;
				xrLabel7.Visible = false;
				xrLabel6.Visible = false;
				xrLabel25.Visible = false;
				xrLabel15.Visible = false;
				lbl_dear_name.Visible = false;
				xrLabel1.Visible = false;
				lblcomp_final.Visible = false;
				lblcomp.Visible = false;
				pnl_comp.Visible = false;
				pnl_terms.Visible = false;
				lbl_milestone_header.Visible = false;
				xrLabel19.Visible = false;
			}
			var months_employed = new TimeSpan(0);
			comp = new NeBusinessUnit(mo.business_unit_id);
            mt = new NeMemberType(mo.membertypeid);

            var membertype_name = mt.name;
			#region EXISTING EMPLOYEES
			if (!mo.isapplicant && mo.applicantid == 0) // if it s an existing employee
			{
				xrLabel11.Visible = false;
				mem = new NeMember(mo.memberid);
				is_exec_package = mem.membertype.exec_severance_package;
				months_employed = System.DateTime.Today.Date.Subtract(Convert.ToDateTime(mem.StartDate));
				// xrLabel1.Text = "Re: Offer of Employment with " + comp.description ;
				if (comp.region == "GTA")
				{

					xrLabel1.Text = "Re: Offer of Employment with " + comp.description;

				}
				else if (comp.region == "DET" || comp.region == "CHR" || comp.region == "FRE")
				{
					{
						xrLabel1.Text = "Re: Offer of Employment with " + comp.description;
					}
				}
				else  // no region
				{
					xrLabel1.Text = "Re: Offer of Employment with " + comp.description;
				}
				lbl_dear_name.Text = "Dear " + mem.FullName + ",";
				lblsigapplicant.Text = mem.FullName;
				lbl_hdr.Text = jd == 1 ? "Job Description" : "Employment Agreement for " + mem.FullName2;
				lblname1.Text = mem.FullName2;
				lbladd1.Text = mem.Address;
				lbladd2.Text = mem.City;
				lblsigapplicant.Text = mem.FullName2;
				if (mem.Prov != "")
				{
					lbladd2.Text += ", " + mem.Prov;
				}
				if (mem.Country != "")
				{
					lbladd2.Text += ", " + mem.Country;
				}
				if (mem.PostalCode != "")
				{
					lbladd2.Text += ", " + mem.PostalCode;
				}
				fname = mem.FirstName;
				pnl_member.Visible = false;
				if (mo.membertypeid != mo.previous_membertype && mo.previous_membertype != 0 && mo.wage > mo.previous_wage)  // if a membertype change happened.. and the wage went up... call it a promotion 
				{
					is_promotion = true;
				}
				else if (mo.wage > mo.previous_wage)  // if a wage increase happened 
				{
					is_wage_increase = true;
				}
				#region We Are Pleased Paragraph
				if (comp.region == "GTA")
				{

					xrLabel9.Visible = false;
					if (mo.reports_to == 0)
					{
						lblwearepleased.Text = @"We are pleased to offer you a" + (is_promotion ? "n employment promotion" : (is_wage_increase ? " wage increase" : "n updated employment agreement")) + " on the terms and conditions set out in this letter. Your position will be " + membertype_name + " and your start date in that role (or new wage) will be " + string.Format("{0:ddd, MMM d, yyyy}", mo.startdate) + " or other such date as is set by the Company in consultation with you.  You will be reporting directly to the Board of Directors.  This is a " + part_time + " position.";
					}
					else
					{
						lblwearepleased.Text = @"We are pleased to offer you a" + (is_promotion ? "n employment promotion" : (is_wage_increase ? " wage increase" : "n updated employment agreement")) + " on the terms and conditions set out in this letter. Your position will be " + membertype_name + " and your start date in that role (or new wage) will be " + string.Format("{0:ddd, MMM d, yyyy}", mo.startdate) + " or other such date as is set by the Company in consultation with you.   You will report directly to " + new NeMember(mo.reports_to).FullName + ", " + get_future_membertype_name(mo.reports_to, mo) + @", or his/her designate. This is a " + part_time + " position.";
					}

				}
				else if (comp.region == "DET" || comp.region == "CHR" || comp.region == "FRE")
				{
					if (mo.reports_to == 0)
					{
						lblwearepleased.Text = comp.name + @" ('The Company') is pleased to offer you a" + (is_promotion ? "n employment promotion" : (is_wage_increase ? " wage increase" : "n updated employment agreement")) + " on the terms and conditions set out in this letter. Your position will be " + membertype_name + " and your start date in that role (or new wage) will be " + string.Format("{0:ddd, MMM d, yyyy}", mo.startdate) + " or other such date as is set by the Company in consultation with you.  You will be reporting directly to the Board of Directors.  Please note that nothing in this employment agreement is designed to modify our 'at-will' employment policy.  This is a " + part_time + " position.";
					}
					else
					{
						lblwearepleased.Text = comp.name + @" ('The Company') is pleased to offer you a" + (is_promotion ? "n employment promotion" : (is_wage_increase ? " wage increase" : "n updated employment agreement")) + " on the terms and conditions set out in this letter. Your position will be " + membertype_name + " and your start date in that role (or new wage) will be " + string.Format("{0:ddd, MMM d, yyyy}", mo.startdate) + " or other such date as is set by the Company in consultation with you.   You will report directly to " + new NeMember(mo.reports_to).FullName + ", " + get_future_membertype_name(mo.reports_to, mo) + @", or his/her designate of the " + get_future_branch(mo.reports_to, mo) + " branch.  Please note that nothing in this employment agreement is designed to modify our 'at-will' employment policy.  This is a " + part_time + " position.";
					}
					lbl_termination.Text = @"Employment with The Company is based on mutual consent, and both the employee and the employer have the right to terminate employment ’at-will’, with or without cause, at any time. This cannot be modified by any oral agreement or representation unless said oral agreement or representation is in writing and signed by the United States Regional Manager or Chief Executive Officer.";
				}
				else  // no region
				{
					lblwearepleased.Text = comp.name + @" is pleased to offer you a" + (is_promotion ? "n employment promotion" : (is_wage_increase ? " wage increase" : "n updated employment agreement")) + " on the terms and conditions set out in this letter. Your position will be " + new NeMemberType(mo.membertypeid).name + " and your start date in that role (or new wage) will be " + string.Format("{0:ddd, MMM d, yyyy}", mo.startdate) + " or other such date as is set by The Company in consultation with you. This is a " + part_time + " position.";
				}
				if (jd == 1)
				{
					report.Parameters["jd"].Value = true;
					lblwearepleased.Text = "The following job description is to be considered as a guideline for your position.  Based on these responsiblities a review of your performance will be conducted by your supervisor as part of our annual process.";
				}
				#endregion
				#region Signature Prelude

				sig_prelude.Text = @"To accept the Company's employment terms and conditions set out in this letter, please sign the Acceptance Clause below and return this letter to me on or before " + mo.startdate.AddDays(-1).ToShortDateString() + @"." + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + "Sincerely," + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + new NeMember(mo.enteredby).FullName + Environment.NewLine + new NeMember(mo.enteredby).membertype.name + Environment.NewLine + comp.name + Environment.NewLine;
				xrLabel18.Text = @"I accept the Company's employment terms and conditions set out in this letter.  I have reviewed this letter carefully and have had the opportunity to obtain independent legal advice.  I understand that this letter is an employment agreement between me and the Company.";
				#endregion
				#region milestones
				/*		DataTable dt_milestones2 = new Memberoffer_Milestones().get_milestones(mo.id);
			if (dt_milestones2 != null)
			{
				if (dt_milestones2.Rows.Count > 0)
				{
					xrLabel11.Visible = true;
					xrSubreport_milestones.Visible = true;
				}
				else
				{
					xrLabel11.Visible = false;
					xrSubreport_milestones.Visible = false;
				}
			}
			else
			{
				xrLabel11.Visible = false;
				xrSubreport_milestones.Visible = false;
			}
*/
				#endregion
				#region Vacation paragraph
				if (mo.vacation_interval_1 == 0)
				{
					rt_vacation.Text = "Initially ";
				}
				else
				{
					rt_vacation.Text = "After " + mo.vacation_interval_1.ToString("N0") + " month(s)";
				}
				if (comp.country == "CDN")
				{


					rt_vacation.Text += ", you will be entitled to " + mo.vacation_amount_1.ToString("P1") + " of vacation time.  Further information about our vacation policy can be obtained from your supervisor.";
					if (months_employed.TotalDays / 30 > mo.vacation_interval_1)
					{
						if (mo.is_salary)
						{

							rt_vacation.Text = "You will be entitled to " + Math.Round(mo.vacation_amount_1 * 50, 0) + " weeks vacation annually based on your employment start date anniversary.  Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company.";
						}
						else
						{
							rt_vacation.Text = "You will be entitled to " + mo.vacation_amount_1.ToString("P1") + " of paid vacation time.  Further information about our vacation policy can be obtained from your supervisor.";
						}
					}
					if (months_employed.TotalDays / 30 >= mo.vacation_interval_2)
					{
						if (mo.is_salary)
						{
							rt_vacation.Text = "You will be entitled to " + Math.Round(mo.vacation_amount_2 * 50, 0) + " weeks vacation annually based on your employment start date anniversary.  Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company.";
						}
						else
						{
							rt_vacation.Text = "You will be entitled to " + mo.vacation_amount_2.ToString("P1") + " of paid vacation time.  Further information about our vacation policy can be obtained from your supervisor.";
						}
					}
					if (months_employed.TotalDays / 30 >= mo.vacation_interval_3)
					{
						if (mo.is_salary)
						{
							rt_vacation.Text = "You will be entitled to " + Math.Round(mo.vacation_amount_3 * 50, 0) + " weeks vacation annually based on your employment start date anniversary.  Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company.";
						}
						else
						{
							rt_vacation.Text = "You will be entitled to " + mo.vacation_amount_3.ToString("P1") + " of paid vacation time.  Further information about our vacation policy can be obtained from your supervisor.";
						}
					}

				}
				else
				{
					rt_vacation.Text += ", you will be entitled to " + mo.vacation_amount_1 + " hours of vacation time in each year of employment.  Further information about our vacation policy can be obtained from your supervisor.";
					if (months_employed.TotalDays / 30 > mo.vacation_interval_1)
					{
						rt_vacation.Text = "You will be entitled to " + mo.vacation_amount_1 + " hours of paid vacation time in each year of employment.  Further information about our vacation policy can be obtained from your supervisor.";
					}
					if (months_employed.TotalDays / 30 >= mo.vacation_interval_2)
					{
						rt_vacation.Text = "You will be entitled to " + mo.vacation_amount_2 + " hours of paid vacation time in each year of employment.  Further information about our vacation policy can be obtained from your supervisor.";
					}
					if (months_employed.TotalDays / 30 >= mo.vacation_interval_3)
					{
						rt_vacation.Text = "You will be entitled to " + mo.vacation_amount_3 + " hours of paid vacation time in each year of employment.  Further information about our vacation policy can be obtained from your supervisor.";
					}
				}
				#endregion
			}
			#endregion
			#region NEW HIRE
			else  // if it's an applicant
			{
				app = new NeApplicant(mo.applicantid);
				is_exec_package = new NeMemberType(app.membertypeid).exec_severance_package;
				lbl_hdr.Text = "Employment Offer for " + app.firstname + " " + app.lastname;
				lblname1.Text = app.firstname + " " + app.lastname;
				lbl_dear_name.Text = "Dear " + app.firstname + ",";
				lbladd1.Text = app.address;
				lbladd2.Text = app.city;
				lblsigapplicant.Text = app.firstname + " " + app.lastname;
				if (app.province != "")
				{
					lbladd2.Text += ", " + app.province;
				}
				if (app.country != "")
				{
					lbladd2.Text += ", " + app.country;
				}
				if (app.postal != "")
				{
					lbladd2.Text += ", " + app.postal;
				}
				fname = app.firstname;
				#region We Are Pleased Paragraph
				if (comp.region == "GTA")
				{

					lblwearepleased.Text = @"We are pleased to offer you employment with the Company on the terms and conditions set out in this letter. Your position will be " + new NeMemberType(mo.membertypeid).name + " and your start date in that role (or new wage) will be " + string.Format("{0:ddd, MMM d, yyyy}", mo.startdate) + " or other such date as is set by the Company in consultation with you.   You will report directly to " + new NeMember(mo.reports_to).FullName + ", " + get_future_membertype_name(mo.reports_to, mo) + @", or his/her designate of the " + comp.name + " branch.";
				}
				else if (comp.region == "DET" || comp.region == "CHR" || comp.region == "FRE")
				{
					lblwearepleased.Text = comp.name + @" ('The Company') is pleased to offer you employment with The Company on the terms and conditions set out in this letter. Your starting position will be " + new NeMemberType(mo.membertypeid).name + " and your start date in that role (or new wage) will be " + string.Format("{0:ddd, MMM d, yyyy}", mo.startdate) + " or other such date as is set by the Company in consultation with you.   You will report directly to " + new NeMember(mo.reports_to).FullName + ", " + get_future_membertype_name(mo.reports_to, mo) + @", or his/her designate of the " + comp.name + " branch.";
				}
				#endregion
				#region Signature Prelude
				sig_prelude.Text = @"To accept The Company's offer of employment on the terms and conditions set out in this letter, please sign the Acceptance Clause below and return this letter to me on or before " + mo.startdate.AddDays(-1).ToShortDateString() + @".  We look forward to you joining our team here at the Company." + Environment.NewLine + Environment.NewLine + "Sincerely," + Environment.NewLine + comp.name + Environment.NewLine + Environment.NewLine + Environment.NewLine + new NeMember(mo.enteredby).FullName + Environment.NewLine + new NeMember(mo.enteredby).membertype.name;
				#endregion
				#region Vacation stuff
				if (comp.country == "CDN")
				{

					if (mo.vacation_interval_1 > 0)
					{
						if (mo.is_salary)
						{
							rt_vacation.Text = "You will be entitled to " + Math.Round(mo.vacation_amount_1 * 50, 0) + " weeks vacation annually, starting from your employment start date.  Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company.";
						}
						else
						{
							rt_vacation.Text = "After " + mo.vacation_interval_1 + " month(s) from your original employment start date, you will be entitled to " + mo.vacation_amount_1.ToString("P1") + " of vacation accrual.  Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company.";
						}
					}
					else // 
					{
						if (mo.is_salary)
						{
							rt_vacation.Text = "Your salary includes " + Math.Round(mo.vacation_amount_1 * 50, 0) + " weeks vacation annually, starting from your employment start date.  Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company. ";
						}
						else
						{
						    if (mo.paytype_id == 6)
						    {
						        rt_vacation.Text = "Initially, you will be entitled to " + mo.vacation_amount_1.ToString("P1") +
                                                   " vacation pay under the Employment Standards Act 2000. Any vacation requests must be submitted in advance to your direct manager and upon approval will be submitted to Payroll as unpaid time off.";
                            }

						    else
						    {
						        rt_vacation.Text = "Initially, you will be entitled to " + mo.vacation_amount_1.ToString("P1") +
						                           " of vacation accrual. Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company. ";
						    }
						}
					}
					if (mo.vacation_amount_2 > mo.vacation_amount_1 && mo.paytype_id != 6)
					{
						if (mo.is_salary)
						{
							rt_vacation.Text += "After " + mo.vacation_interval_2 + " months, your salary will include " + Math.Round(mo.vacation_amount_2 * 50, 0) + " weeks vacation annually.  Your vacation anniversary is your employment start date. Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company. ";
						}
						else
						{
							rt_vacation.Text += "After " + mo.vacation_interval_2 + " months, you will be entitled to " + mo.vacation_amount_2.ToString("P1") + " of paid vacation time. Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company. ";
						}
					}
					if (mo.vacation_amount_3 > mo.vacation_amount_2 && mo.paytype_id != 6)
					{
						if (mo.is_salary)
						{
							rt_vacation.Text += "And after " + mo.vacation_interval_3 + " months, your salary will include " + Math.Round(mo.vacation_amount_3 * 50, 0) + " weeks vacation annually.  Your vacation anniversary is your employment start date. Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company. ";
						}
						else
						{
							rt_vacation.Text += "And after " + mo.vacation_interval_3 + " months, you will be entitled to " + mo.vacation_amount_3.ToString("P1") + " of paid vacation time. Vacation is subject to the approval of your supervisor or his/her designee and must be booked sufficiently in advance to allow for the efficient operation of the company. ";
						}
					}
				}

				else
				{
					if (mo.vacation_interval_1 > 0)
					{
						rt_vacation.Text = "After " + mo.vacation_interval_1 + " month(s) from your original employment start date, you will be entitled to " + mo.vacation_amount_1 + " hours of paid vacation time. ";
					}
					else
					{
						if (mo.is_salary)
						{
							rt_vacation.Text = "Initially, you will be entitled to " + mo.vacation_amount_1 + " hours of paid vacation time. ";
						}
						else
						{
							rt_vacation.Text = "Initially, you will be entitled to " + mo.vacation_amount_1 + " hours of paid vacation time. ";
						}
					}
					if (mo.vacation_amount_2 > mo.vacation_amount_1)
					{
						rt_vacation.Text += "After " + mo.vacation_interval_2 + " months, you will be entitled to " + mo.vacation_amount_2 + " hours of paid vacation time. ";
					}
					if (mo.vacation_amount_3 > mo.vacation_amount_2)
					{
						rt_vacation.Text += "And after " + mo.vacation_interval_3 + " months, you will be entitled to " + mo.vacation_amount_3 + " hours of paid vacation time. ";
					}
				}
				#endregion
			}
			#endregion
			#region milestones
			var dt_milestones = new Memberoffer_Milestones().get_milestones(mo.id);
			if (dt_milestones != null)
			{
				if (dt_milestones.Rows.Count > 0)
				{
					xrLabel11.Visible = true;
					xrSubreport_milestones.Visible = true;
					lbl_milestone_header.Visible = true;
				}
				else
				{
					xrLabel11.Visible = false;
					xrSubreport_milestones.Visible = false;
					lbl_milestone_header.Visible = false;
				}
			}
			else
			{
				xrLabel11.Visible = false;
				xrSubreport_milestones.Visible = false;
				lbl_milestone_header.Visible = false;
			}
			#endregion
			#region wage section
			if (mo.is_salary)
			{
				if (comp.region == "DET" || comp.region == "CHR" || comp.region == "FRE")
				{
					lblwage.Text = "For your services, the Company will pay you at the rate of " + (Math.Round(mo.wage * 80 * 26 / 100, 0) * 100).ToString("C2") + " per year.  This salary is inclusive of your vacation allowance indicated in this document. ";
				}
				else
				{

					lblwage.Text = "For your services, the Company will pay you at the rate of " + (Math.Round(mo.wage * 80 * 26 / 100, 0) * 100).ToString("C2") + " per year.  This salary is inclusive of your vacation allowance indicated in this document.  Your compensation will be paid in accordance with the payroll practices of the Company as in effect and are subject to change from time to time and will be subject to statutory deductions and withholdings.";
				}
			}
			else
			{
				lblwage.Text = "For your services, the Company will pay you at the rate of " + mo.wage.ToString("C2") + " per hour.  Overtime or non-scheduled hours must be approved by your supervisor prior to the working of the hours.   Your compensation will be paid in accordance with the payroll practices of the Company as in effect and are subject to change  from time to time and will be subject to statutory deductions and withholdings.";
			}
			#endregion
			#region comp section

			if (mo.has_comp == 1)
			{
				lblcomp.Text = "You will also be eligible to receive incentive compensation as set out in an Incentive Compensation Plan found in Appendix B of this letter.  You understand and agree that the Company may revise your Incentive Compensation Plan from time to time at it's sole discretion.";
				lblcomp_final.Text = Environment.NewLine + mo.comp_details;
				pnl_comp.Visible = true;
			}
			else
			{
				lblcomp.Text = "";
				lblcomp_final.Text = "";
				pnl_comp.Visible = false;
			}
			if (mo.contract_details != null && mo.contract_details != "")
			{
				lblcomp.Text = "Please visit Appendix B for further details outlining your compensation.  You understand and agree that the Company may revise your Compensation Plan from time to time at it's sole discretion.";
				lblcomp_final.Text = Environment.NewLine + mo.contract_details;
				pnl_comp.Visible = true;
			}

            #endregion
            #region benefits
            if (comp.country == "CDN")
            {
                xrLabel9.Visible = false;
            }
            else
            {
                xrLabel9.Text = "Orientation Period";
                xrLabel10.Text = @"Please note that the first three months of your employment with The Company shall constitute an orientation period. This orientation period will allow you an opportunity to determine if your new job is suitable for you, and your supervisor will have an opportunity to evaluate your work performance. However, the completion of this orientation period does not guarantee employment for any period of time thereafter.  ";
            }

            if (mo.part_time || mo.paytype_id == 6)
            {
              
                lbl_benefits.Text = "You will not be entitled to participate in the benefit plan";
            }
            else
            {
                if (comp.country == "CDN")
                {

                 


                    if (mo.isapplicant)
                    {
                            if (mo.benefits_startdate == null)
                            {
                                lbl_benefits.Text =
                                    "You will be entitled to participate in the " +
                                    new NeMemberType(mo.membertypeid).benefitplan +
                                    " after 3 months of employment.  Further details are available in the Company Employee Handbook.  Long Term Disability & Life Insurance benefits are mandatory.";
                            }
                            else
                            {
                                lbl_benefits.Text = "You will be entitled to participate in the " +
                                                    new NeMemberType(mo.membertypeid).benefitplan + " on " +
                                                    Convert.ToDateTime(mo.benefits_startdate).ToString("yyyy-MM-dd") +
                                                    ".  Long Term Disability & Life Insurance benefits are mandatory.  Further details are available in the Company Employee Handbook. ";
                            }
                        //		lbl_benefits.Text = "You will be entitled to participate in benefits, including medical and dental" +  (mo.benefits_startdate==null?" after 6 months of employment.": " on " + Convert.ToDateTime(mo.benefits_startdate).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        if (months_employed.TotalDays < 90)
                        {
                            if (mo.benefits_startdate == null)
                            {
                                lbl_benefits.Text =
                                    "You will be entitled to participate in the " +
                                    new NeMemberType(mo.membertypeid).benefitplan +
                                    " after 3 months of employment.  Further details are available in the Company Employee Handbook.  Long Term Disability & Life Insurance benefits are mandatory.";
                            }
                            else
                            {
                                lbl_benefits.Text = "You will be entitled to participate in the " +
                                                    new NeMemberType(mo.membertypeid).benefitplan + " on " + Convert.ToDateTime(mo.benefits_startdate).ToString("yyyy-MM-dd") + ".  Long Term Disability & Life Insurance benefits are mandatory.  Further details are available in the Company Employee Handbook. ";
                            }

                        }
                        else
                        {
                            lbl_benefits.Text =
                                "You are entitled to participate in the " +
                                new NeMemberType(mo.membertypeid).benefitplan +
                                ".  Further details are available in the Company Employee Handbook.  Long Term Disability & Life Insurance benefits are mandatory.";
                        }

                    }
                    lbl_benefits.Text += " For your convenience, we will provide you with a benefits summary separately.  The Company reserves the right to change or terminate any benefits from time to time in its discretion." + Environment.NewLine + " Employees using a company vehicle for travel to and from their residence may be subject to additional taxable income as per local and federal tax laws.  ";

                }
                else  // is US employee
                {
                    if (months_employed.TotalDays < 90 && !mo.isapplicant)
                    {
                        lbl_benefits.Text = "You will be eligible for the new hire standard benefit plan on the 1st of the month following 90 days of employment.  ";
                    }
                    //				else if ((months_employed.TotalDays < 730) && (mo.isapplicant == false))
                    //				{
                    //					lbl_benefits.Text = "After 2 years (from your start date), you will be eligble for the " + new NeMemberType(mo.membertypeid).benefitplan + ".";
                    //				}
                    if (mo.isapplicant)
                    {
                        lbl_benefits.Text = "You will be eligible for the new hire standard benefit plan on the 1st of the month following 90 days of employment.  ";
                    }
                    lbl_benefits.Text += " For your convenience, we will provide you with a benefits summary separately.  The Company reserves the right to change or terminate any benefits from time to time in its discretion." + Environment.NewLine + " Employees using a company vehicle for travel to and from their residence may be subject to additional taxable income as per local and federal tax laws.  ";
                }
            }
			#endregion
			#region governing law
			if (comp.region == "GTA")
			{

				xrlbl_governinglaw.Text = "This agreement shall be governed by the laws of the province of Ontario and the laws of Canada in force in Ontario.";

			}
			else if (comp.region == "FRE")
			{
				xrlbl_governinglaw.Text = "This agreement shall be governed by the laws of the state of California and the laws of the United States in force in California.";
			}
			else if (comp.region == "DET")
			{
				xrlbl_governinglaw.Text = "This agreement shall be governed by the laws of the state of Michigan and the laws of the United States in force in Michigan.";
			}
			else if (comp.region == "CHR")
			{
				xrlbl_governinglaw.Text = "This agreement shall be governed by the laws of the state of North Carolina and the laws of the United States in force in North Carolina.";
			}
			else
			{
				xrlbl_governinglaw.Text = "This agreement shall be governed by the laws of the province of Ontario and the laws of Canada in force in Ontario.";
			}
			#endregion
			#region trucks and laptops etc

			if (mo.gets_laptop && mo.gets_phone && mo.gets_vehicle)
			{
				lbl_benefits.Text += "You will be issued a laptop, cell phone and a vehicle.  Issued equipment are considered property of the Company.  You may be held liable for damage to this equipment if you are found to be negligent.  Vehicles are only permitted and insured for company use.  If you are found to be negligent, you will be responsible for accident repairs, speeding charges and other damages to the vehicle.  Should the vehicle be considered a taxable benefit, you will be liable to pay this tax. ";
			}
			else if (mo.gets_laptop && mo.gets_phone && !mo.gets_vehicle)
			{
				lbl_benefits.Text += "You will be issued a laptop and a cell phone.  Issued equipment are considered property of the Company.  You may be held liable for damage to this equipment if you are found to be negligent.  ";
			}
			else if (mo.gets_laptop && !mo.gets_phone && !mo.gets_vehicle)
			{
				lbl_benefits.Text += "You will be issued a laptop.  Issued equipment are considered property of the Company.  You may be held liable for damage to this equipment if you are found to be negligent.  ";
			}
			else if (!mo.gets_laptop && mo.gets_phone && mo.gets_vehicle)
			{
				lbl_benefits.Text += "You will be issued a cell phone and a vehicle.  Issued equipment are considered property of the Company.  You may be held liable for damage to this equipment if you are found to be negligent.  Vehicles are only permitted and insured for company use.  If you are found to be negligent, you will be responsible for accident repairs, speeding charges and other damages to the vehicle.  Should the vehicle be considered a taxable benefit, you will be liable to pay this tax. ";
			}
			else if (!mo.gets_laptop && mo.gets_phone && !mo.gets_vehicle)
			{
				lbl_benefits.Text += "You will be issued a cell phone.  Issued equipment are considered property of the Company.  You may be held liable for damage to this equipment if you are found to be negligent.  ";
			}
			else if (!mo.gets_laptop && !mo.gets_phone && mo.gets_vehicle)
			{
				lbl_benefits.Text += "You will be issued a vehicle.  Vehicles are only permitted and insured for company use.  If you are found to be negligent, you will be responsible for accident repairs, speeding charges and other damages to the vehicle.  Should the vehicle be considered a taxable benefit, you will be liable to pay this tax. ";
			}
			else if (mo.gets_laptop && !mo.gets_phone && mo.gets_vehicle)
			{
				lbl_benefits.Text += "You will be issued a laptop and a vehicle.  Issued equipment are considered property of the Company.  You may be held liable for damage to this equipment if you are found to be negligent.  Vehicles are only permitted and insured for company use.  If you are found to be negligent, you will be responsible for accident repairs, speeding charges and other damages to the vehicle.  Should the vehicle be considered a taxable benefit, you will be liable to pay this tax. ";
			}

            #endregion
            #region TERMINATION TEXT
            #region comp.region GTA
            if (comp.region == "GTA")
			{

				if (mt.exec_severance_package>0)
				{
					lbl_termination.Text = @"
Resignation - If you resign from your employment, you shall provide " + mt.exec_severance_package + @" week(s) written resignation notice to the Company.  The Company may, in its discretion, waive such notice period, subject to the Ontario Employment Standards Act, 2000 or any successor or amended legislation (the ESA).
					
Termination by The Company Without Just Cause - The Company may terminate your employment at any time, without just cause by providing you with the minimum notice and a severance pay equaling 2 weeks (of salary only) per year of fulltime employment with the Company plus " + mt.exec_severance_package + @" week(s), to a maximum of 6 months(if any), plus all other rights, benefits and entitlements that you then have under the Employment Standards Act (Ontario)(the ESA).  You understand and agree that you are not entitled to any further notice or pay in lieu of notice.

Termination by The Company for Just Cause - The Company may terminate your employment at any time, without notice, for just cause, subject to the ESA.

All equipment, property, documents or any other materials of any kind created or used by you in the course of employment, or otherwise furnished by or belonging to the Company or persons doing business with the Company and in your possession or control, including all copies of confidential information (as defined below), will be surrendered by you to the Company, in good condition, promptly upon your termination of employment, irrespective of the time, manner or cause of termination.

Incentive Compensation on Termination - Because incentive compensation is intended to encourage retention of highly-performing employees, if you resign or your employment is terminated by the Company for just cause, you will be deemed to have relinquished any unpaid incentive compensation, whether or not declared by the Company and whether or not you worked the entire fiscal year.  If the Company terminates your employment without just cause, The Company will pay you any incentive compensation earned by you that has already been declared by the Company but has not yet been paid, and you will not be entitled to any additional incentive compensation.
";
				}
				else
				{
					lbl_termination.Text = @"
Resignation - If you resign from your employment, you shall provide two (2) weeks written resignation notice to the Company.  The Company may, in its discretion, waive such notice period, subject to the Ontario Employment Standards Act, 2000 or any successor or amended legislation (the ESA).
					
Termination by The Company Without Just Cause - The Company may terminate your employment at any time, without just cause by providing you with the minimum notice and severance pay (if any) to which you are entitled under the ESA, plus all other rights, benefits and entitlements that you then have under the ESA.  You understand and agree that you are not entitled to any further notice or pay in lieu of notice.

Termination by The Company for Just Cause - The Company may terminate your employment at any time, without notice, for just cause, subject to the ESA.

Incentive Compensation on Termination - Because incentive compensation is intended to encourage retention of highly-performing employees, if you resign or your employment is terminated by the Company for just cause, you will be deemed to have relinquished any unpaid incentive compensation, whether or not declared by the Company and whether or not you worked the entire fiscal year.  If The Company terminates your employment without just cause, the Company will pay you any incentive compensation earned by you that has already been declared by the Company but has not yet been paid, and you will not be entitled to any additional incentive compensation.
";
				}

			}
            #endregion
            #region comp.region NOT GTA
            else
            {
                if (mt.exec_severance_package > 0)
                {
					lbl_termination.Text = @"
Resignation - If you resign from your employment, you shall provide " + mt.exec_severance_package  + @" week(s) written resignation notice to the Company.  The Company may, in its discretion, waive such notice period.

Termination by The Company 'Without Just Cause' - The Company may terminate your employment at any time, without just cause by providing you with the minimum notice and a severance pay equaling 2 weeks (of salary only) per year of fulltime employment with the Company, to a maximum of 6 months(if any), plus all other rights, benefits and entitlements that you then have under local labor law.  You understand and agree that you are not entitled to any further notice or pay in lieu of notice.

Termination by The Company for 'Just Cause' - The Company may terminate your employment at any time, without notice, for just cause, subject to local labor law.

All equipment, property, documents or any other materials of any kind created or used by you in the course of employment, or otherwise furnished by or belonging to the Company or persons doing business with the Company and in your possession or control, including all copies of confidential information (as defined below), will be surrendered by you to the Company, in good condition, promptly upon your termination of employment, irrespective of the time, manner or cause of termination.

Incentive Compensation on Termination - Because incentive compensation is intended to encourage retention of highly-performing employees, if you resign or your employment is terminated by The Company for just cause, you will be deemed to have relinquished any unpaid incentive compensation, whether or not declared by The Company and whether or not you worked the entire fiscal year.  If The Company terminates your employment without just cause, The Company will pay you any incentive compensation earned by you that has already been declared by The Company but has not yet been paid, and you will not be entitled to any additional incentive compensation.
";
				}
				else
				{
					lbl_termination.Text = @"
Resignation - If you resign from your employment, you shall provide two (2) weeks written resignation notice to the Company.  The Company may, in its discretion, waive such notice period, subject to local labor law.

Termination by The Company 'Without Just Cause' - The Company may terminate your employment at any time, without just cause by providing you with the minimum notice and severance pay (if any) to which you are entitled under local labor law.  You understand and agree that you are not entitled to any further notice or pay in lieu of notice.

Termination by The Company for 'Just Cause' - The Company may terminate your employment at any time, without notice, for just cause, subject to local labor law.

Incentive Compensation on Termination - Because incentive compensation is intended to encourage retention of highly-performing employees, if you resign or your employment is terminated by the Company for 'Just Cause', you will be deemed to have relinquished any unpaid incentive compensation, whether or not declared by the Company and whether or not you worked the entire fiscal year.  If the Company terminates your employment 'Without Just Cause', The Company will pay you any incentive compensation earned by you that has already been declared by the Company but has not yet been paid, and you will not be entitled to any additional incentive compensation.
";
				}
			}
            #endregion
            #endregion
            xrLabel19.Text = "Revision: " + mo.last_modified;
		    xrlogo.ImageUrl = Toolbox.app_setting("Domain")+ @"/images/Logos/" + comp.logo_file;
			return report;
		}


		static string get_future_membertype_name(int member_id, NeMemberOffer mo)
		{
			string mt;
			if (member_id != 0)
			{
				mt = Toolbox.doSQL_string(@"Select ifnull((SELECT
mt.membertype_name
FROM
member_offers
INNER JOIN membertype mt ON member_offers.membertypeid = mt.membertype_id
WHERE
member_offers.memberid = @v0  AND
date(member_offers.startdate) <= @v1 AND
date(member_offers.enddate) >= @v1 and 
(member_offers.status = 'Awaiting Start Date' OR
member_offers.status = 'Accepted')
ORDER BY
member_offers.memberid DESC limit 1),
(Select membertype.membertype_name from membertype inner join member on member.member_membertype_id = membertype.membertype_id and member.member_id = @v0))",
new object[]
	{
		member_id,mo.startdate.Date.ToString("yyyy-MM-dd")
	}
);
			}
			else
			{
				mt = "Board Member";
			}
			return mt;
		}
		static string get_future_branch(int member_id, NeMemberOffer mo)
		{
			string mt;
			if (member_id != 0)
			{
				mt = Toolbox.doSQL_string(@"Select ifnull((SELECT
c.name
FROM
member_offers
INNER JOIN business_unit c ON member_offers.business_unit_id = c.id
WHERE
member_offers.memberid =@v0 AND
date(member_offers.startdate) <=@v1 AND
date(member_offers.enddate) >= @v1 and 
(member_offers.status = 'Awaiting Start Date' OR
member_offers.status = 'Accepted')
ORDER BY
member_offers.memberid DESC limit 1),
(Select business_unit.name from business_unit  inner join member on member.business_unit_id = business_unit.id and member.member_id = @v0))",
					new object[]
						{
							member_id,mo.startdate.Date.ToString("yyyy-MM-dd")
						}
						);
			}
			else
			{
				mt = "NESI Services";
			}
			return mt;
		}
		public static object fill_report_sub(subcontract_employment report, NeMemberOffer mo)
		{
			var _tools = new Toolbox();
			var lbl_hdr = report.FindControl("lblheader", true) as XRLabel;
			var lbl_necompany = report.FindControl("lblnecompany", true) as XRLabel;
			var lbl_subcontractor_address = report.FindControl("lbl_subcontractor_address", true) as XRLabel;
			var xrLabel2 = report.FindControl("xrLabel2", true) as XRLabel;
			var xrLabel6 = report.FindControl("xrLabel6", true) as XRLabel;
			var xrLabel7 = report.FindControl("xrLabel7", true) as XRLabel;
			var xrRichText1 = report.FindControl("xrRichText1", true) as XRRichText;
			var xrRichText2 = report.FindControl("xrRichText2", true) as XRRichText;
			var xrRichText4 = report.FindControl("xrRichText4", true) as XRRichText;

		    var xrPictureBox1 = report.FindControl("xrPictureBox1", true) as XRPictureBox;
		    var xrPictureBox2 = report.FindControl("xrPictureBox2", true) as XRPictureBox;


            var vendor = new NEVendor(Convert.ToInt32(mo.vendor_id));
			lbl_hdr.Text = "Contract for Services - " + vendor.Name;
			lbl_necompany.Text = new NeBusinessUnit(mo.business_unit_id).name;
			xrLabel2.Text = vendor.Name + " (" + new NeMember(mo.memberid).FullName + ")";
			lbl_subcontractor_address.Text = vendor.Address.Addr1 + ", " + vendor.Address.City + ", " + vendor.Address.Prov + " - " + (vendor.Address.PhoneNumber != "NA" ? vendor.Address.PhoneNumber : "");
			xrRichText1.Text = xrRichText1.Text.Replace("[SERVICES]", mo.contract_details);
			xrRichText2.Text = xrRichText2.Text.Replace("[START DATE]", mo.startdate.ToString("yyyy-MM-dd"));
			xrRichText2.Text = xrRichText2.Text.Replace("[RATE]", mo.wage.ToString("C2"));
			xrLabel6.Text = new NeMember(mo.memberid).FullName;
			xrLabel7.Text = vendor.Name + " (" + new NeMember(mo.memberid).FullName + ")";

		    xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + new NeBusinessUnit(mo.business_unit_id).logo_file;
		    xrPictureBox2.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + new NeBusinessUnit(mo.business_unit_id).logo_file;


            return report;
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
		public static void create_duplicate(NeMemberOffer _mo)
		{
			var old_id = _mo.id;
			_mo.id = 0;
			_mo.save();
			create_duplicate_memberoffer_cr(old_id, _mo.id);
			create_duplicate_milestones(old_id, _mo.id);
		}
		public static int get_current_agreement(int mid)
		{
			return Toolbox.doSQL_int(@"Select ifnull((Select id from member_offers 
where memberid = @v0 and status = 'Accepted' and startdate <= curdate() order by id desc limit 1),0)", mid);
		}
		public static int get_current_agreement_at_date(int mid, DateTime dt)
		{
			return Toolbox.doSQL_int(@"Select ifnull((Select id from member_offers where memberid = @v0 and 
(status = 'Accepted' or status = 'Previous')  and enddate >=@v1  and startdate <= @v1 order by id desc limit 1),0)",
new object[] { mid, dt.ToString("yyyy-MM-dd") }
);
		}
		public static void processOnboardingFvrs(NeBusinessUnit businessUnitObj, NeTaxEntity taxEntityObj, NeMemberOffer mo)
			{
			var master_dtls = new member_fvr_dtl();
			if(!string.IsNullOrEmpty(businessUnitObj.default_fvr_template_ids) || !string.IsNullOrEmpty(taxEntityObj.default_fvr_template_ids))
				{
				var templateIds = new string[] {};
				if(!string.IsNullOrEmpty(businessUnitObj.default_fvr_template_ids))
					{
					templateIds = !businessUnitObj.default_fvr_template_ids.Contains(",") 
						? new [] {businessUnitObj.default_fvr_template_ids} 
						: businessUnitObj.default_fvr_template_ids.Split(',');
					}
				else if(!string.IsNullOrEmpty(taxEntityObj.default_fvr_template_ids))
					{
					templateIds = !taxEntityObj.default_fvr_template_ids.Contains(",") 
						? new [] {taxEntityObj.default_fvr_template_ids } 
						: taxEntityObj.default_fvr_template_ids.Split(',');
					}
				foreach(var _template_id in templateIds)
					{
					var templateId = Convert.ToInt32(_template_id);
					var hdr        = new member_fvr_template.header(templateId);
					var h          = new member_fvr_hdr
						                 {
						                 req_member_id     = mo.enteredby,
						                 tabs_needed       = string.Join(",", hdr.tabs_needed),
						                 business_unit_ids = mo.business_unit_id.ToString(),
						                 active            = 1,
						                 type              = "NEWHIRE",
						                 expire_date       = DateTime.Now.Date.AddDays(hdr.days_till_expire)
						                 };
					h.save();

					var dtls = new ArrayList();
					foreach (member_fvr_template.detail detail in hdr.dtls)
						{
						if (detail.is_selected)
							{
							var d = new member_fvr_dtl
								        {
								        member_id         = mo.memberid,
								        tab_index_actual  = detail.page_id,
								        member_fvr_hdr_id = h.id,
								        file_id           = detail.file_id,
								        url               = detail.url,
								        upload_required   = detail.upload_required ? 1 : 0
								        };
							dtls.Add(d);
							}
						}
					master_dtls.mass_insert(dtls);
					}
				}
			}
		
		#region collect IT itmes
		public static void send_it_notices(NeMemberOffer mo, NeMember current_user)
		{
			var mo_m = new NeMember(mo.memberid);
			var mt = new NeMemberType(mo.membertypeid);
			var error_text = "Sending Email to IT";
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
				it_notice.To = "it@thatsnew.com";
				it_notice.From = "administrator@thatsnew.com";
				if (mo.isapplicant)
				{
					it_notice.Subject = "New User Created (or old user reinstated), there is information that needs your attention";
				}
				else
				{
					it_notice.Subject = "Check " + mo_m.FullName + "'s IT settings in "+ Toolbox.app_setting("Domain") +"  They just got a new employment agreement";
				}
				it_notice.isHTML = true;
				it_notice.Body = string.Format("The user <b>{0}</b> was set to a(n) {5} by <b>{1}</b> - Member ID ({2}).<br/>The start date is: {4}<br/><b>Items that need your attention:</b><br/><ul>{3}</ul>", mo_m.FullName, current_user.FullName, mo_m.id, items_needed, mo_m.StartDate, mt.name);
				it_notice.Send();
			}
		}
        #endregion

        public static int GetSignedOfferFileID(int _offerID)
        {
            int FileID = 0;
            FileID = Toolbox.doSQL_int(@"Select ifnull((Select id from filestore.files  where folder_id=@v0 and sub_folder_id =@v1),0) ", new object[] { FolderType.Signed, _offerID });
            return FileID;
        }
        public static int GetUnSignedOfferFileID(int _offerID)
        {
            int FileID = 0;
            FileID = Toolbox.doSQL_int(@"Select ifnull((Select id from filestore.files  where folder_id=@v0 and sub_folder_id =@v1),0) ", new object[] { FolderType.Unsigned, _offerID });
            return FileID;
        }
    }
    /// <summary>
    /// Summary description for Memberoffer_Milestones
    /// </summary>
    public class Memberoffer_Milestones
	{
		private Toolbox _tools = new Toolbox();
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
			return Toolbox.doSQL_dt(@"SELECT milestone, addedby, due, ticketid, notes FROM memberoffer_milestones WHERE offerid = @v0  ORDER BY due", new object[] {  _offerid } );
		}
		public void update_milstones_from_old_offers()
		{
			var dt = _tools.getSQL_datatable(@"SELECT id, enteredby, startdate, milestones30, milestones90, milestones180 FROM member_offers"  , null);
			foreach (DataRow dr in dt.Rows)
			{
				Memberoffer_Milestones m;
				if (dr["milestones30"] != null && dr["milestones30"].ToString().Trim() != "")
				{
					m = new Memberoffer_Milestones();
					m.offerid = Convert.ToInt32(dr[0]);
					m.addedby = Convert.ToInt32(dr[1]);
					m.due = Convert.ToDateTime(dr["startdate"]).AddDays(30);
					m.milestone = dr["milestones30"].ToString();
					m.notes = "";
					m.save();
				}
				if (dr["milestones90"] != null && dr["milestones90"].ToString().Trim() != "")
				{
					m = new Memberoffer_Milestones();
					m.offerid = Convert.ToInt32(dr[0]);
					m.addedby = Convert.ToInt32(dr[1]);
					m.due = Convert.ToDateTime(dr["startdate"]).AddDays(90);
					m.milestone = dr["milestones90"].ToString();
					m.notes = "";
					m.save();
				}
				if (dr["milestones180"] != null && dr["milestones180"].ToString().Trim() != "")
				{
					m = new Memberoffer_Milestones();
					m.offerid = Convert.ToInt32(dr[0]);
					m.addedby = Convert.ToInt32(dr[1]);
					m.due = Convert.ToDateTime(dr["startdate"]).AddDays(180);
					m.milestone = dr["milestones180"].ToString();
					m.notes = "";
					m.save();
				}
			}
		}
   
    }
   
 
    public class FolderType
    {
        public const int Signed = 3;
        public const int Unsigned = 4;

    }
}
