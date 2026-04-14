using System;
using System.Data;
using System.Runtime.Remoting.Channels;
using System.Web;
using nesi.core;
using NESI.Common.Models;

namespace core
{
	public class TodoList
	{
		public static void getToDoList(DataTable dt_final, NeMember myMember)
		{
			using (var conn = Toolbox.connect())
			{
				DataTable dt;
				// MH (2014-07-30): I added a limiter of the gridview's page size to the queries/datatable.. no filtering, no paging means it's a ton of unneeded processing.. which makes this page slow.
				// If we want to show more rows, or add filtering/paging... these queries need to be made very fast.
				var ps = 20;

				if (myMember.MemberTypeID == 42) // if the person is the IT person
				{
					#region Missing LDAPs

					dt = Toolbox.doSQL_dt(conn,
						@"SELECT member.member_id id, FUN_TIME (member_dateadded) date_insert, member_windows_password, member_ldap_user, 

Concat(bu.ddl_name,' - ',member.member_fullname) member_fullname, gets_neemail, member_neemail 
FROM member 
inner join business_unit bu on bu.id = member.business_unit_id 
WHERE member_status = 'Active' AND ( gets_neemail = 1 OR (member_neemail LIKE '%newelectric.com' and member_neemail != 'nomail@newelectric.com') ) AND ( member_windows_password = '' OR member_ldap_user = '' ) and business_unit_id !=8 LIMIT 20",
						null);
					foreach (DataRow dr in dt.Rows)
					{
						var id = dr["id"];

						var dt_insert = dr["date_insert"].ToString();
						dt_final.Rows.Add("/#/opens/127/employees/" + id,
							"LDAP Needs Adjusting for " + dr["member_fullname"], dt_insert, "",
							"LDAP for " + dr["member_fullname"],
							$@"127/employees/{id}/it",
							id);
					}

					#endregion Missing LDAPs

					#region missing in Cellphone table

					//					dt = Toolbox.doSQL_dt(conn, @"SELECT a.vacation_id id, FUN_TIME(a.date_insert) date_insert, date_start, date_return, b.member_fullname FROM vacation_master a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.status = 1 AND b.reports_to=@v0  ORDER BY a.date_start", new object[] {  myMember.id } );
					//			foreach (DataRow dr in dt.Rows)
					//			{
					//				var id = dr["id"];
					//				DateTime dt_start = Convert.ToDateTime(dr["date_start"]);
					//				DateTime dt_return = Convert.ToDateTime(dr["date_return"]);
					//				string dt_insert = dr["date_insert"].ToString();
					//				dt_final.Rows.Add("/sections/hr/vacation_admin/index.aspx#v" + id, string.Format("Starts: {0:yyyy-MM-dd}, Returns: {1:yyyy-MM-dd}", dt_start, dt_return), dt_insert, "", "Vacation Request for " + dr["member_fullname"]);
					//			}

					#endregion terminations to complete

					#region person is missing extension

					/*			dt = Toolbox.doSQL_dt(conn, @"SELECT a.vacation_id id, FUN_TIME(a.date_insert) date_insert, date_start, date_return, b.member_fullname FROM vacation_master a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.status = 1 AND b.reports_to=@v0  ORDER BY a.date_start", new object[] {  myMember.id } );
                foreach (DataRow dr in dt.Rows)
                {
                    var id = dr["id"];
                    DateTime dt_start = Convert.ToDateTime(dr["date_start"]);
                    DateTime dt_return = Convert.ToDateTime(dr["date_return"]);
                    string dt_insert = dr["date_insert"].ToString();
                    dt_final.Rows.Add("/sections/hr/vacation_admin/index.aspx#v" + id, string.Format("Starts: {0:yyyy-MM-dd}, Returns: {1:yyyy-MM-dd}", dt_start, dt_return), dt_insert, "", "Vacation Request for " + dr["member_fullname"]);
                }
        */

					#endregion terminations to complete

					#region person is missing email

					/*			dt = Toolbox.doSQL_dt(conn, @"SELECT a.vacation_id id, FUN_TIME(a.date_insert) date_insert, date_start, date_return, b.member_fullname FROM vacation_master a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.status = 1 AND b.reports_to=@v0  ORDER BY a.date_start", new object[] {  myMember.id } );
                foreach (DataRow dr in dt.Rows)
                {
                    var id = dr["id"];
                    DateTime dt_start = Convert.ToDateTime(dr["date_start"]);
                    DateTime dt_return = Convert.ToDateTime(dr["date_return"]);
                    string dt_insert = dr["date_insert"].ToString();
                    dt_final.Rows.Add("/sections/hr/vacation_admin/index.aspx#v" + id, string.Format("Starts: {0:yyyy-MM-dd}, Returns: {1:yyyy-MM-dd}", dt_start, dt_return), dt_insert, "", "Vacation Request for " + dr["member_fullname"]);
                }
        */

					#endregion terminations to complete

					#region pending HR closure

					dt = Toolbox.doSQL_dt(conn,
						@"SELECT distinct emp_trm.trm_member_id id, member.reports_to, fun_time(member.Member_TermDate) date_insert, 
Concat(bu.ddl_name,' - ',member.member_fullname) member_fullname
FROM emp_trm_chklist_lnk INNER JOIN emp_trm ON emp_trm_chklist_lnk.trm_id = emp_trm.id 
INNER JOIN member ON emp_trm.trm_member_id = member.Member_ID 
INNER JOIN emp_trm_chklist_opt ON emp_trm_chklist_lnk.opt_id = emp_trm_chklist_opt.id 
inner join business_unit bu on bu.id = member.business_unit_id 
WHERE member.member_hrstatus_id = 4 AND emp_trm_chklist_lnk.is_checked = 0 AND type = 'SysAdmin' LIMIT 20", null);

					//			dt = Toolbox.doSQL_dt(conn, @"SELECT member.member_id id, FUN_TIME(member_termdate) date_insert, member.member_fullname FROM member WHERE member.member_hrstatus_id = 4 AND (reports_to=@v0  or " + myMember.AuthenticatedForPrivilege(37) + ") ORDER BY member_termdate", new object[] {  myMember.id } );
					foreach (DataRow dr in dt.Rows)
					{
						var id = dr["id"];
						var dt_insert = dr["date_insert"].ToString();
						dt_final.Rows.Add("/sections/hr/member/if_termination.aspx?id=" + id + "&type=Direct",
							string.Format(
								"Their HR status shouldn't be Pending IT Closure still , on the member page, complete thier IT checklist and send them to PAST status."),
							dt_insert, "", "Pending Closure HR Status required for " + dr["member_fullname"],
							$@"127/employees/{id}/termination",
							id
							);
					}

					#endregion
				}


				#region  quote process

				var dt_quotes = new quote().get_quote_todo_list(myMember.id);
				foreach (DataRow dr_quotes in dt_quotes.Rows)
				{
					dt_final.ImportRow(dr_quotes);
				}

				#endregion


				if ((myMember.AuthenticatedForPrivilege(15) || myMember.AuthenticatedForPrivilege(16)))
				{
					#region purchase orders that need to be closed because they are holding up the invoice

					dt = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT
	WOProg_ID AS id,
	WOProg_BVWO AS bvwo,
	business_unit_id,
	woprog_customername AS customer,
	woprog_description AS description,
			WOProg_Status,
	woprog_pm_memberid AS mid,
	woprog_ts,
 (SELECT COUNT(po_details_id) FROM poprog_header, po_details_current WHERE po_details_current.is_gl_account =false and poprog_status IN (1,2,5,3,9) AND po_details_woprog_id = a.woprog_id AND po_details_line_active = 1 AND poprog_id = po_details_poprog_id) po_count
FROM
woprog a 
where business_unit_id = " + myMember.business_unit_id + @"  and 
(
woprog_status not in ('Open','Invoiced','Deleted') and 
(SELECT COUNT(po_details_id) FROM poprog_header, po_details_current WHERE po_details_current.is_gl_account =false and poprog_status IN (1,2,5,3,9) AND po_details_woprog_id = a.woprog_id AND po_details_line_active = 1 AND poprog_id = po_details_poprog_id) > 0
)
LIMIT {1}", myMember.id, ps), null);
					foreach (DataRow dr in dt.Rows)
					{
						if ((dr["mid"].ToString().Equals(myMember.id.ToString())))
						{
							var url = "/redir.aspx?url=" + HttpContext.Current.Server.UrlEncode(
										  "%2Fwo_prog_frame.aspx?action=show%2526woprog_id=" + dr["id"] + "%2526business_unit_id=" +
										  dr["business_unit_id"]);
							dt_final.Rows.Add(url,
								"Has " + dr["po_count"].ToString() + " vendor pos open that are holding up invoicing",
								Toolbox.doSQL_string(conn, "Select fun_time(@v0)",
									new object[] { Convert.ToDateTime(dr["woprog_ts"]).ToString("yyyy-MM-dd HH:mm:ss") }), "",
								"WO: " + dr["bvwo"].ToString().TrimStart('0') + " for " + dr["customer"]);
						}
					}

					#endregion
				}

				if ((myMember.MemberTypeID == 9))
				{
					#region purchase orders that need to be closed because they are holding up the invoice

					dt = Toolbox.doSQL_dt(conn, string.Format(@"
		Select 
d.vendor_name,
a.poprog_bvpo,
a.poprog_id,
c.woprog_customername,
c.woprog_bvwo,
fun_time(c.WOProg_OpenDateTime) poprog_ts
from
po_details_current b 
inner join poprog_header a on b.po_details_poprog_id = a.poprog_id
inner join woprog c on b.po_details_woprog_id = c.WOProg_ID
inner join vendor d on a.poprog_vendor_id = d.Vendor_ID
where 
b.is_gl_account =false and
woprog_status not in ('Open','Invoiced','Deleted') and 
a.poprog_status IN (1,2,5,3,9) and 
b.po_details_line_active = 1 and 
c.business_unit_id = " + myMember.business_unit_id + @"
group by 
c.WOProg_ID limit {0}", ps), null);
					foreach (DataRow dr in dt.Rows)
					{
						dt_final.Rows.Add("/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=" + dr["poprog_id"],
							"PO is holding up Invoicing!", dr["poprog_ts"], "",
							"PO " + dr["poprog_bvpo"].ToString().TrimStart('0') + " for " + dr["vendor_name"] + " on WO " +
							dr["woprog_bvwo"]);
					}

					#endregion
				}


				if ((myMember.AuthenticatedForPrivilege(15) || myMember.AuthenticatedForPrivilege(16)))
				{
					#region work orders waiting for PMs

					dt = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT
	WOProg_ID AS id,
	WOProg_BVWO AS bvwo,
	business_unit_id,
	woprog_customername AS customer,
	woprog_description AS description,
	woprog_expected_enddate AS exp_completion_date,
	WOProg_Status,
	woprog_pm_memberid AS mid,
	woprog_ts
 FROM
woprog a 
where business_unit_id =" + myMember.business_unit_id + @" and woprog_pm_memberid = '{0}' and 
(
(woprog_status in ('Questions For PM','Waiting PM Approval','Waiting For PO')) or 
(woprog_status = 'Open' and woprog_expected_enddate < curdate()) 
)
LIMIT {1}", myMember.id, ps), null);
					foreach (DataRow dr in dt.Rows)
					{
						if (dr["WOProg_Status"].ToString() == "Questions For PM")
						{

							var url = "/redir.aspx?url=" + HttpContext.Current.Server.UrlEncode(
										  "%2Fwo_prog_frame.aspx?action=show%2526woprog_id=" + dr["id"] +
										  "%2526business_unit_id=" +
										  dr["business_unit_id"]);

							dt_final.Rows.Add(
								url, "Waiting for you to answer a question",
								Toolbox.doSQL_string(conn, "Select fun_time(@v0)",
									new object[] { Convert.ToDateTime(dr["woprog_ts"]).ToString("yyyy-MM-dd HH:mm:ss") }), "",
								"WO: " + dr["bvwo"].ToString().TrimStart('0') + " for " + dr["customer"]);
						}
						else if (dr["WOProg_Status"].ToString() == "Waiting PM Approval")
						{

							var url = "/redir.aspx?url=" + HttpContext.Current.Server.UrlEncode(
										  "%2Fwo_prog_frame.aspx?action=show%2526woprog_id=" + dr["id"] +
										  "%2526business_unit_id=" +
										  dr["business_unit_id"]);

							dt_final.Rows.Add(
								url, "Waiting for you to approve",
								Toolbox.doSQL_string(conn, "Select fun_time(@v0)",
									new object[] { Convert.ToDateTime(dr["woprog_ts"]).ToString("yyyy-MM-dd HH:mm:ss") }), "",
								"WO: " + dr["bvwo"].ToString().TrimStart('0') + " for " + dr["customer"]);
						}
						else if (dr["WOProg_Status"].ToString() == "Waiting For PO")
						{
							var url = "/redir.aspx?url=" + HttpContext.Current.Server.UrlEncode(
										  "%2Fwo_prog_frame.aspx?action=show%2526woprog_id=" + dr["id"] +
										  "%2526business_unit_id=" +
										  dr["business_unit_id"]);
							dt_final.Rows.Add(
								url, "You need to get a PO from the customer.",
								Toolbox.doSQL_string(conn, "Select fun_time(@v0)",
									new object[] { Convert.ToDateTime(dr["woprog_ts"]).ToString("yyyy-MM-dd HH:mm:ss") }), "",
								"WO: " + dr["bvwo"].ToString().TrimStart('0') + " for " + dr["customer"]);
						}
					}

					#endregion
				}

				if (myMember.AuthenticatedForPrivilege(16) &&
					(myMember.MemberTypeID == 5 || myMember.MemberTypeID == 67 || myMember.MemberTypeID == 38))
				{
					#region work orders waiting for BMs

					dt = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT
	WOProg_ID AS id,
	WOProg_BVWO AS bvwo,
	business_unit_id,
	woprog_customername AS customer,
	woprog_description AS description,
	woprog_expected_enddate AS exp_completion_date,
	WOProg_Status,
	woprog_pm_memberid AS mid,
	woprog_ts
 FROM
woprog a 
where business_unit_id =" + myMember.business_unit_id + @"  and woprog_status in ('Waiting BM Approval')
LIMIT {1}", myMember.id, ps), null);
					foreach (DataRow dr in dt.Rows)
					{
						var url = "/redir.aspx?url=" + HttpContext.Current.Server.UrlEncode(
							"%2Fwo_prog_frame.aspx?action=show%2526woprog_id=" + dr["id"] +
							"%2526business_unit_id=" +
							dr["business_unit_id"]);

						dt_final.Rows.Add(
							url,
							"WO waiting for you to approve",
							Toolbox.doSQL_string(conn, "Select fun_time(@v0)",
								new object[] { Convert.ToDateTime(dr["woprog_ts"]).ToString("yyyy-MM-dd HH:mm:ss") }), "",
							"WO: " + dr["bvwo"].ToString().TrimStart('0') + " for " + dr["customer"]);
					}

					#endregion
				}


				#region purchase orders

				dt = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT DISTINCT 
	a.poprog_id,
	a.poprog_cutdate po_date,
	a.poprog_bvpo,
	a.poprog_status,
	a.poprog_order_description po_description,
	b.vendor_name,
	  a.poprog_ack_req,
     a.poprog_ack_req_rec, 
    a.poprog_ship_note_req, 
    a.poprog_ship_note_req_rec,
	a.nesi_cut_po,
	a.poprog_apstatus,
	fun_time(a.poprog_ts) poprog_ts,
a.poprog_hasproblem_notes,
	g.apstatus_name ,
a.poprog_cutby_member_id  
FROM 
	poprog_header a 
LEFT JOIN vendor b 
	ON b.Vendor_ID = a.poprog_vendor_id 
LEFT JOIN 
	apstatus g 
	ON a.poprog_apstatus = g.apstatus_id	
inner join business_unit_po_dist cpd on cpd.amount_to>=a.poprog_total_cost	and (cpd.member_id = {2} AND  FIND_IN_SET({2},(Select get_supervisors(a.poprog_cutby_member_id))))
WHERE 
	a.poprog_status = 2 AND 
	a.business_unit_id = {0}
LIMIT {1}
", myMember.business_unit_id, ps, myMember.id), null);
				foreach (DataRow dr in dt.Rows)
				{
					if (dr["po_description"].ToString().Length > 40)
					{
						dr["po_description"] = dr["po_description"].ToString().Substring(0, 40);
					}
					dt_final.Rows.Add("/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=" + dr["poprog_id"],
						"PO requires your approval", dr["poprog_ts"], "",
						"PO " + dr["poprog_bvpo"].ToString().TrimStart('0') + " for " + dr["vendor_name"] + "-" +
						dr["po_description"].ToString().Replace(",", "").Replace("'", ""));
				}

				#endregion


				#region invalid bin qtys

				if ((myMember.MemberTypeID == 5) || (myMember.MemberTypeID == 9) || (myMember.MemberTypeID == 28))
				{
					dt = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT
inventory_incorrect_levels.id,
EnteredBy.member_fullname AS EnteredBy_name,
inventory_incorrect_levels.date AS date_entered,
inventory_incorrect_levels.reported_qty,
inventory_incorrect_levels.notes,
inventory_incorrect_levels.master_id,
Location.`name` AS location_name,
inventory_incorrect_levels.incorrect_qty,
inventory_description.description,
inventory_location.qty QtyinStock
FROM
inventory_incorrect_levels
LEFT JOIN member AS EnteredBy ON EnteredBy.Member_ID = inventory_incorrect_levels.member_id
LEFT JOIN member AS ClearedBy ON ClearedBy.Member_ID = inventory_incorrect_levels.cleared_by
INNER JOIN inventory_location_master AS Location ON inventory_incorrect_levels.location_id = Location.id
INNER JOIN inventory_description ON inventory_incorrect_levels.master_id = inventory_description.master_id 
            INNER JOIN inventory_location ON Location.id = inventory_location.location_master_id AND inventory_incorrect_levels.master_id = inventory_location.master_id 
where Location.business_unit_id = {0} and (cleared_by is null or cleared_by =0)
order by inventory_incorrect_levels.id desc
LIMIT {1}
", myMember.business_unit_id, ps), null);
					foreach (DataRow dr in dt.Rows)
					{
						dt_final.Rows.Add("/sections/member/inventory/incorrect_location_qtys.aspx?page=207",
							"Invalid Qty in Location", dr["date_entered"], "",
							"Part No " + dr["master_id"].ToString() + " in " + dr["location_name"] + " was reported by " +
							dr["EnteredBy_name"].ToString() + " to have " + dr["reported_qty"].ToString() +
							" but the system is showing " + dr["QtyinStock"].ToString());
					}
				}

				#endregion


			//	#region tickets
			//
			//	dt = Toolbox.doSQL_dt(conn, string.Format(
			//		@"SELECT ticketheader_id,a.ticketheader_modified_date md,c.ticketstatus_status,a.ticketheader_issue
			// FROM ticketheader AS a LEFT JOIN member AS f ON a.ticketheader_member_assigned_id = f.member_id, ticketpage AS b, ticketstatus AS c, tickettype AS d, ticketpriority AS e, member AS g, ticket_group as h 
			// WHERE ticketheader_module_id = ticketpage_id 
			// AND ticketheader_status_id = ticketstatus_id and ((ticketheader_createdby_member_id=" + myMember.id +
			//		" AND ticketstatus_id =7) OR (ticketheader_waitingon_member_id=" + myMember.id + " AND ticketstatus_id=3)" +
			//		" OR (ticketstatus_id=9 AND ticketheader_member_assigned_id='' AND h.ticket_group_administrator = " +
			//		myMember.id + ") or(ticketstatus_id = 1 and h.ticket_group_administrator = " + myMember.id +
			//		@") or (ticketstatus_id in (8,9) and ticketheader_member_assigned_id= " + myMember.id + @")) 
			// AND ticketheader_issuetype = tickettype_id 
			// AND ticketheader_priority_id = ticketpriority_id and b.ticketpage_ticket_group_id = h.ticket_group_id 
			// AND ticketheader_createdby_member_id = g.member_id 
			// ORDER BY ticketheader_priority_id,ticketheader_id LIMIT {0}", ps), null);
			//	foreach (DataRow dr in dt.Rows)
			//	{
			//		dt_final.Rows.Add("/sections/member/tickets/ticketpage.aspx?issue=" + dr["ticketheader_id"],
			//			"Ticket requires your attention : " + dr["ticketstatus_status"],
			//			Toolbox.doSQL_string(conn, "Select fun_time(@v0)",
			//				new object[] { Convert.ToDateTime(dr["md"]).ToString("yyyy-MM-dd HH:mm:ss") }), "",
			//			dr["ticketheader_issue"]);
			//	}
			//
			//	#endregion

				// Disabling this till we can renovate the CAP page, Devops task https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/2346
				/*
				if (myMember.AuthenticatedForPage(174))
				{
					#region certificates

					dt = NeCapTraining.get_missing_certs_for_member(myMember.id);

					if (dt.Rows.Count > 0)
					{
						dt_final.Rows.Add("/sections/hr/member/cap_home.aspx?page=174", "Certificates Expiring",
							"In " + dt.Rows[0]["cert_exp"] + " days", "", "Take me to the CAP home page");
					}

					#endregion
				}
				*/

				#region reviews

				dt = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT
	a.id,
	a.status,
	a.member_id,
	FUN_TIME(a.date) f_date,
	b.member_fullname,
	a.reviewed_by_id
FROM
	emp_review a
LEFT JOIN
	member b ON
		a.member_id = b.member_id
WHERE
	a.status != 'Delivered' and a.status != 'Closed' and 
	curdate() > DATE_SUB(a.date, interval 1 month) and 
	a.reviewed_by_id = {0} and b.member_status = 'Active'
ORDER BY 
	a.date
LIMIT {1}", myMember.id, ps), null);
				foreach (DataRow dr in dt.Rows)
				{
					// applying the same check for as in review list for member page
					if (     
							  (Convert.ToInt32(dr["reviewed_by_id"]) == myMember.id) 
							||(myMember.AuthenticatedForPrivilege(OpsPrivilege.ViewAllReviews))
							||(NeMember.is_supervisor(Convert.ToInt32(dr["member_id"]), myMember.id))
							||((Convert.ToInt32(dr["member_id"]) == myMember.id) && (dr["status"].ToString() == "Delivered"))
						  
						)
					{ 					
						dt_final.Rows.Add(
						"/sections/hr/member/review_list_for_member.aspx?id=" + dr["id"] + "&memberid=" + dr["member_id"],
						"Employee Review needs to be completed and delivered", dr["f_date"], "",
						"Review for " + dr["member_fullname"]);
					}
				}

				#endregion

				#region member offers

				dt = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT
	a.id,
	a.status,
	a.enteredby,
	a.isapplicant,
	a.applicantid,
	a.memberid,
	a.reports_to,
	FUN_TIME(a.date) f_date,
	b.firstname,
	b.lastname,
	c.member_fullname
FROM
	member_offers a
LEFT JOIN
	applicants b ON a.applicantid = b.id
LEFT JOIN
	member c ON a.memberid = c.member_id 
WHERE
	a.status = 'Waiting for Approval' AND 
(
(
	
		(a.reports_to = a.enteredby)&&({0}=(Select reports_to from member where member_id=a.reports_to limit 1))
	
)
or
(
	FIND_IN_SET(a.reports_to, REPORTS_TO(a.enteredby)) and ({0}=(Select reports_to from member where member_id=a.enteredby limit 1))
)
or
(
	FIND_IN_SET(a.enteredby, REPORTS_TO(a.reports_to)) and ({0}=(Select reports_to from member where member_id=a.reports_to limit 1))
) 
)
order by a.date LIMIT 20", myMember.id),
					null); // Reports_to function  returns everyone BELOW.  So this query will return results where the reports_to and entered_by are both BELOW the current user.
				foreach (DataRow dr in dt.Rows)
				{
					var firstname = dr["firstname"];
					var lastname = dr["lastname"];
					var date = dr["f_date"];
					var id = dr["id"];
					var is_applicant = dr["isapplicant"] != DBNull.Value && Convert.ToBoolean(dr["isapplicant"]);
					var memberid = Toolbox.ReturnZeroIfNull_int(dr["memberid"]);
					var fullname = dr["member_fullname"];
					if (is_applicant)
					{
						dt_final.Rows.Add("/sections/hr/member/member_offer.aspx?id=" + id + "&isapplicant=1",
							"Applicant offer needs to be approved",
							date,
							"",
							"Offer for " + firstname + " " + lastname,
								$@"138/applicants/{dr["applicantid"]}/offer/{id}",
								$@"{dr["applicantid"]},{id}");
					}
					else
					{
						dt_final.Rows.Add("/sections/hr/member/member_offer.aspx?id=" + id,
							"Employee agreement needs to be approved", date, "", "Agreement for " + fullname,
							$@"127/employees/{memberid}/offer/{id}",
						$@"{memberid},{id}"
							);
					}
				}

				#endregion


				#region open agreements past start dates

				dt = Toolbox.doSQL_dt(conn,
					@" SELECT a.id, a.status, a.enteredby, a.isapplicant, a.applicantid, a.memberid, a.reports_to, FUN_TIME(a.date) f_date, b.firstname, b.lastname, c.member_fullname
FROM member_offers a 
LEFT JOIN applicants b ON a.applicantid = b.id
LEFT JOIN member c ON a.memberid = c.member_id 
WHERE a.startdate<CURDATE() 
	AND a.status NOT IN ('Accepted', 'Previous','Closed','Past') 
	AND a.enteredby=@v0  
	AND c.business_unit_id!=8 
ORDER BY a.date LIMIT 20",
					new object[]
					{
				myMember.id
					}); // Reports_to function  returns everyone BELOW.  So this query will return results where the reports_to and entered_by are both BELOW the current user.
				foreach (DataRow dr in dt.Rows)
				{
					var firstname = dr["firstname"];
					var lastname = dr["lastname"];
					var date = dr["f_date"];
					var id = dr["id"];
					var is_applicant = dr["isapplicant"] == DBNull.Value ? false : Convert.ToBoolean(dr["isapplicant"]);
					var memberid = Toolbox.ReturnZeroIfNull_int(dr["memberid"]);
					var fullname = dr["member_fullname"];
					if (is_applicant)
					{
						dt_final.Rows.Add("/sections/hr/member/member_offer.aspx?id=" + id + "&isapplicant=1",
							"Stale applicant offer needs to be finalized", date, "", "Offer for " + firstname + " " + lastname,
							$@"138/applicants/{dr["applicantid"]}/offer/{id}",
							$@"{dr["applicantid"]},{id}");
					}
					else
					{
						dt_final.Rows.Add("/sections/hr/member/member_offer.aspx?id=" + id,
							"Employee agreement needs to be finalized", date, "", "Agreement for " + fullname,
							$@"127/employees/{memberid}/offer/{id}",
							$@"{memberid},{id}");
					}
				}

				#endregion

				#region vacations

				if (myMember.business_unit.id == 11)
				{
					dt = Toolbox.doSQL_dt(conn,
						@"SELECT a.vacation_id id, FUN_TIME(a.date_insert) date_insert, date_start, date_return, b.member_fullname FROM vacation_master a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.status = 1 AND b.business_unit_id =@v0  and b.reports_to =@v1  ORDER BY a.date_return, b.member_fullname LIMIT 20",
						new object[] { myMember.business_unit_id, myMember.id });
				}
				else
				{
					var temp_vacation = myMember.AuthenticatedForPage(5);
					dt = Toolbox.doSQL_dt(conn,
						@"SELECT a.vacation_id id, FUN_TIME(a.date_insert) date_insert, date_start, date_return, b.member_fullname FROM vacation_master a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.status = 1 AND b.business_unit_id =@v0  and (@v1 ) and FIND_IN_SET(a.member_id, REPORTS_TO(@v2 )) ORDER BY a.date_return, b.member_fullname LIMIT 20",
						new object[] { myMember.business_unit_id, temp_vacation, myMember.id });
				}
				foreach (DataRow dr in dt.Rows)
				{
					var id = dr["id"];
					var dt_start = Convert.ToDateTime(dr["date_start"]);
					var dt_return = Convert.ToDateTime(dr["date_return"]);
					var dt_insert = dr["date_insert"].ToString();
					var url = "/redir.aspx?url=" + HttpContext.Current.Server.UrlEncode(
							"/sections/hr/vacation_admin/index.aspx?vacation_id=" + dr["id"]);
					dt_final.Rows.Add(url,
						string.Format("Starts: {0:yyyy-MM-dd}, Returns: {1:yyyy-MM-dd}", dt_start, dt_return), dt_insert, "",
						"Vacation Request for " + dr["member_fullname"]);
				}

				#endregion vacations


				#region bonus approvals

				var count_of_bonuses = Toolbox.doSQL_string(conn, @"Select ifnull((
SELECT fun_time(a.requested_when) requested_when
FROM 
	payroll_extra_payments a
LEFT JOIN
	member b
		ON a.member_id = b.member_id
LEFT JOIN
	member c
		ON c.member_id = " + myMember.id + @"
LEFT JOIN
	payperiods d
		ON a.payperiod_id = d.payperiodid
Left join 
	member e 
		on a.requested_by = e.member_id and e.member_status = 'Active'
WHERE 
	a.approved = -1 
	and b.member_status = 'Active' 
and " + myMember.AuthenticatedForPage(62) + @"
	and ((e.reports_to = " + myMember.id + @" or (e.reports_to=0 and e.member_id = " + myMember.id +
																  @")) or (e.reports_to is null and FIND_IN_SET(a.member_id, REPORTS_TO(" +
																  myMember.id + @"))))
order by a.requested_when limit 1),'') LIMIT 20
", null);
				if (count_of_bonuses != "")
				{
					dt_final.Rows.Add("/sections/hr/commission_bonus_admin/index.aspx", "Some bonuses need to be approved",
						count_of_bonuses, "", "Take me to the bonus admin page");
				}

				#endregion bonus approvals


				if (myMember.business_unit_id != 11 && myMember.business_unit_id != 48 && (myMember.AuthenticatedForPage(179)))
				{
					#region oncall schedule

					var count_of_missingoncalls = Toolbox.doSQL_dt(conn,
						@"select ifnull((select min(oncall_schedule.date) from oncall_schedule  
WHERE oncall_schedule.date>=curdate() AND
oncall_schedule.business_unit_id = " + myMember.business_unit_id + @"),'None Set') end_date,

ifnull((SELECT
count(id)
FROM
oncall_schedule
WHERE oncall_schedule.date>=curdate() and 
oncall_schedule.date < (curdate()+interval 14 day) AND
oncall_schedule.business_unit_id = " + myMember.business_unit_id + " order by date ),0) Limit 20", null);

					if (Convert.ToInt32(count_of_missingoncalls.Rows[0][1]) < 28)
					{
						dt_final.Rows.Add("/sections/member/scheduler/index4.aspx", "Missing On Call Schedules",
							count_of_missingoncalls.Rows[0][0], "", "Take me to the On Call admin page");
					}

					#endregion bonus approvals
				}


				#region Payroll People

				var first_hr_member = Toolbox.doSQL_string(conn,
					@"SELECT group_concat(member_id) FROM member where member_membertype_id in (57,72) and member_status = 'active'",
					null); // Getting payroll administrator
				var super_of_first_hr_member = Toolbox.doSQL_string(conn,
					@"Select group_concat(reports_to) from member  where find_in_set(member_id,@v0) and member_status = 'Active'",
					new object[] { first_hr_member });
				if (IsMemberIDInHRList(myMember.id, first_hr_member) || 
                    IsMemberIDInHRList(myMember.id, super_of_first_hr_member)
				) // can the user see the personal INFO section of the member index page
				{
					#region employees that have had agreement changes requiring an update with payroll software

					dt = Toolbox.doSQL_dt(conn, @"
SELECT b.member_id id, FUN_TIME(DATE) date_insert, Concat(bu.ddl_name,' - ', c.member_fullname) member_fullname, DATE,a.member_id h_member_id,a.type_id,b.reports_to 
FROM member_todo_list_helper a
inner join member c ON a.type_id = c.member_id and c.member_status = 'Active'
LEFT JOIN member b ON b.member_id = a.member_id 

inner join business_unit bu on bu.id = c.business_unit_id
WHERE a.type='payroll' AND (a.member_id = @v0  OR b.reports_to = @v0 ) and FIND_IN_SET( c.business_unit_id, get_visible_business_units_group_concat (@v0))  ORDER BY DATE",
						new object[] { myMember.id });
					foreach (DataRow dr in dt.Rows)
					{
						var id = dr["type_id"];
						var dt_insert = dr["date_insert"].ToString();
						if (first_hr_member.Contains(myMember.id.ToString()))
						{
							dt_final.Rows.Add("/#/opens/127/employees/" + id,
								string.Format(
									"Payroll Service (ADP) data needs to be reviewed and then the `data verified` button pressed in their file in " + Toolbox.app_setting("Domain")  + "->employees "),
								dt_insert, "", "Payroll Data for " + dr["member_fullname"], $@"127/employees/{id}/userinfo", id);
						}
						else if ((super_of_first_hr_member.Contains(myMember.id.ToString())) &&
								 DateTime.Today > Convert.ToDateTime(dr["date"]).AddDays(6))
						{
							dt_final.Rows.Add("/#/opens/127/employees/" + id,
								string.Format(
									"Payroll Service (ADP) data needs to be reviewed and then the `data verified` button pressed in their file in "+ Toolbox.app_setting("Domain") + "->employees "),
								dt_insert, "", "Payroll Data for " + dr["member_fullname"], $@"127/employees/{id}/userinfo", id);
						}
					}

					#endregion

					#region employees with HR status of NEW after their start date, should be probation - send to HR

					dt = Toolbox.doSQL_dt(conn,
						@"SELECT member.member_id id, FUN_TIME(member_startdate + interval 3 month) date_insert, member_startdate, Concat(bu.ddl_name,' - ', member.member_fullname) member_fullname,(member_startdate + interval 3 month) eff_date 
FROM member inner join business_unit bu on bu.id = member.business_unit_id
WHERE member_status = 'Active' and member_hrstatus_id = 6 and member.business_unit_id !=8 and curdate() > (member_startdate + interval 3 month) and 
FIND_IN_SET(
    member.business_unit_id,
    get_visible_business_units_group_concat (@v0)
  ) ORDER BY member_startdate", new object[] { myMember.id });
					foreach (DataRow dr in dt.Rows)
					{
						var id = dr["id"];
						var dt_insert = dr["date_insert"].ToString();
						if (first_hr_member.Contains(myMember.id.ToString()))
						{
							dt_final.Rows.Add("/#/opens/127/employees/" + id,
								string.Format(
									"Their HR status shouldn't be probation still, on the member page, update their status."),
								dt_insert, "", "Change of HR status required for " + dr["member_fullname"], $@"127/employees/{id}/userinfo", id);
						}
						else if ((super_of_first_hr_member.Contains(myMember.id.ToString())) &&
								 DateTime.Today > Convert.ToDateTime(dr["eff_date"]).AddDays(6))
						{
							dt_final.Rows.Add("/#/opens/127/employees/" + id,
								string.Format(
									"Their HR status shouldn't be probation still, on the member page, update their status."),
								dt_insert, "", "Change of HR status required for " + dr["member_fullname"], $@"127/employees/{id}/userinfo", id);
						}
					}

					#endregion

					#region employees what have don't have a benefit ID after thier benefits entitlement date

					dt = Toolbox.doSQL_dt(conn,
						@"SELECT member.member_id id, FUN_TIME(benefits_startdate) date_insert, Concat(bu.ddl_name,' - ', member.member_fullname) member_fullname,benefits_startdate 
FROM member inner join business_unit bu on bu.id = member.business_unit_id
WHERE member_status = 'Active' and benefits_id is null and curdate() > benefits_startdate and member.business_unit_id !=8 and 
member_hrstatus_id = 3   AND FIND_IN_SET(
    member.business_unit_id,
    get_visible_business_units_group_concat (@v0)
  ) ORDER BY member_startdate", new object[] { myMember.id });
					foreach (DataRow dr in dt.Rows)
					{
						var id = dr["id"];
						var dt_insert = dr["date_insert"].ToString();
						if (first_hr_member.Contains(myMember.id.ToString()))
						{
							dt_final.Rows.Add("/#/opens/127/employees/" + id,
								string.Format(
									"They have survived long enough to be granted benefits, please update their file with our benefits provider"),
								dt_insert, "", dr["member_fullname"] + " needs their benefits enabled", $@"127/employees/{id}/userinfo", id);
						}
						else if ((super_of_first_hr_member.Contains(myMember.id.ToString())) &&
								 DateTime.Today > Convert.ToDateTime(dr["benefits_startdate"]).AddDays(6))
						{
							dt_final.Rows.Add("/#/opens/127/employees/" + id,
								string.Format(
									"They have survived long enough to be granted benefits, please update their file with our benefits provider"),
								dt_insert, "", dr["member_fullname"] + " needs their benefits enabled", $@"127/employees/{id}/userinfo", id);
						}
					}

					#endregion

					#region employees that have don't have a payroll id 

					dt = Toolbox.doSQL_dt(conn,
						@"SELECT member.member_id id, FUN_TIME(member_startdate) date_insert, Concat(bu.ddl_name,' - ', member.member_fullname) member_fullname,member_startdate 
FROM member 
inner join business_unit bu on bu.id = member.business_unit_id 
WHERE member_status = 'Active' and (Member_Payroll_ID is null or Member_Payroll_ID='') and member.business_unit_id !=8   AND FIND_IN_SET(
    member.business_unit_id,
    get_visible_business_units_group_concat (@v0)
  ) ORDER BY member_startdate", new object[] { myMember.id });
					foreach (DataRow dr in dt.Rows)
					{
						var id = dr["id"];
						var dt_insert = dr["date_insert"].ToString();

						if (first_hr_member.Contains(myMember.id.ToString()))
						{
							dt_final.Rows.Add("/#/opens/127/employees/" + id,
								string.Format("Missing payroll id number"),
								dt_insert, "", dr["member_fullname"] + " needs their payroll id number filled in", $@"127/employees/{id}/userinfo", id);
						}
						else if ((super_of_first_hr_member.Contains(myMember.id.ToString())) &&
								 DateTime.Today > Convert.ToDateTime(dr["member_startdate"]).AddDays(6))
						{
							dt_final.Rows.Add("/#/opens/127/employees/" + id,
								string.Format("Missing payroll id number"),
								dt_insert, "", dr["member_fullname"] + " needs their payroll id number filled in", $@"127/employees/{id}/userinfo", id);
						}
					}

					#endregion

					#region pending HR closure

					dt = Toolbox.doSQL_dt(conn,
						@"SELECT distinct emp_trm.trm_member_id id, member.reports_to, FUN_TIME(member.Member_TermDate) date_insert, Concat(bu.ddl_name,' - ', member.member_fullname) member_fullname 
FROM emp_trm_chklist_lnk 
INNER JOIN emp_trm ON emp_trm_chklist_lnk.trm_id = emp_trm.id 
INNER JOIN member ON emp_trm.trm_member_id = member.Member_ID 
INNER JOIN emp_trm_chklist_opt ON emp_trm_chklist_lnk.opt_id = emp_trm_chklist_opt.id 
inner join business_unit bu on bu.id = member.business_unit_id
WHERE member.member_hrstatus_id = 4 AND emp_trm_chklist_lnk.is_checked = 0 AND type = 'Payroll'   AND FIND_IN_SET(
    member.business_unit_id,
    get_visible_business_units_group_concat (@v0)
  ) ORDER BY member_termdate ", new object[] { myMember.id });

					//			dt = Toolbox.doSQL_dt(@"SELECT member.member_id id, FUN_TIME(member_termdate) date_insert, member.member_fullname FROM member WHERE member.member_hrstatus_id = 4 AND (reports_to=@v0  or " + myMember.AuthenticatedForPrivilege(37) + ") ORDER BY member_termdate", new object[] {  myMember.id } );
					foreach (DataRow dr in dt.Rows)
					{
						var id = dr["id"];
						var dt_insert = dr["date_insert"].ToString();
						dt_final.Rows.Add("/sections/hr/member/if_termination.aspx?id=" + id + "&type=Direct",
							string.Format(
								"Their HR status shouldn't be Pending Closure still, on the member page, complete thier PAYROLL checklist and send them to PAST status."),
							dt_insert, "", "Pending Closure HR Status required for " + dr["member_fullname"], $@"127/employees/{id}/termination", id);
					}

					#endregion
				}

				#endregion

				#region training is coming up

				dt = Toolbox.doSQL_dt(conn,
					@"SELECT
  training_header_history.id id,
  training_header.`name` `Training_Name`,
  fun_time (training_header_history.date) `TDate`,
  training_header_history.start_time `starttime`,
  training_header_history.end_time `endtime`,
  cap_training_schedule.location `Location`,
  member.Member_ID `memberid`
FROM
  training_header
  INNER JOIN training_header_history
    ON training_header_history.training_header_id = training_header.id
  INNER JOIN member
    ON training_header_history.member_id = member.Member_ID
  INNER JOIN cap_training_schedule
    ON training_header_history.cap_training_schedule_id = cap_training_schedule.id
WHERE training_header_history.date > CURDATE()
  AND training_header_history.date < CURDATE() + INTERVAL 15 DAY
  AND member.Member_ID = @v0
  AND FIND_IN_SET(
    member.business_unit_id,
    get_visible_business_units_group_concat (@v0)
  )
ORDER BY training_header_history.date DESC
LIMIT 20", new object[] { myMember.id });
				foreach (DataRow dr in dt.Rows)
				{
					var id = dr["id"];


					var dt_insert = dr["TDate"].ToString();

					dt_final.Rows.Add("/sections/hr/member/cap_home.aspx?page=174",
						string.Format("Location: {0} - Start Time: {1}", dr["Location"], dr["starttime"]), dt_insert, "",
						"Training: " + dr["Training_Name"] + " is coming up");
				}

				#endregion training is coming up

				#region person is missing truck asset id

				/*			dt = Toolbox.doSQL_dt(conn,@"SELECT a.vacation_id id, FUN_TIME(a.date_insert) date_insert, date_start, date_return, b.member_fullname FROM vacation_master a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.status = 1 AND b.reports_to=@v0  ORDER BY a.date_start", new object[] {  myMember.id } );
                foreach (DataRow dr in dt.Rows)
                {
                    var id = dr["id"];
                    DateTime dt_start = Convert.ToDateTime(dr["date_start"]);
                    DateTime dt_return = Convert.ToDateTime(dr["date_return"]);
                    string dt_insert = dr["date_insert"].ToString();
                    dt_final.Rows.Add("/sections/hr/vacation_admin/index.aspx#v" + id, string.Format("Starts: {0:yyyy-MM-dd}, Returns: {1:yyyy-MM-dd}", dt_start, dt_return), dt_insert, "", "Vacation Request for " + dr["member_fullname"]);
                }
        */

				#endregion person is missing truck asset id

				#region pending HR closure for your reports

				//		if ((myMember.business_unit.branch_manager.id == myMember.id)||(myMember.business_unit.is_backoffice))
				//		{

				dt = Toolbox.doSQL_dt(conn,
					@"SELECT distinct emp_trm.trm_member_id id, member.reports_to, fun_time(member.Member_TermDate) date_insert, Concat(bu.ddl_name,' - ',member.member_fullname) member_fullname 
FROM emp_trm_chklist_lnk INNER JOIN emp_trm ON emp_trm_chklist_lnk.trm_id = emp_trm.id 
INNER JOIN member ON emp_trm.trm_member_id = member.Member_ID 
INNER JOIN emp_trm_chklist_opt ON emp_trm_chklist_lnk.opt_id = emp_trm_chklist_opt.id 

inner join business_unit bu on bu.id = member.business_unit_id 

WHERE member.member_hrstatus_id = 4 and member.Member_TermDate < curdate()-interval 14 day
AND emp_trm_chklist_lnk.is_checked = 0 AND type = 'Direct' 
and is_supervisor(member.member_id, @v0) AND FIND_IN_SET(
                member.business_unit_id,
                get_visible_business_units_group_concat(@v0)
                    ) Limit 20", new object[] { myMember.id });

				//			dt = Toolbox.doSQL_dt(@"SELECT member.member_id id, FUN_TIME(member_termdate) date_insert, member.member_fullname FROM member WHERE member.member_hrstatus_id = 4 AND (reports_to=@v0  or " + myMember.AuthenticatedForPrivilege(37) + ") ORDER BY member_termdate", new object[] {  myMember.id } );
				foreach (DataRow dr in dt.Rows)
				{
					var id = dr["id"];
					var dt_insert = dr["date_insert"].ToString();
					dt_final.Rows.Add("/sections/hr/member/if_termination.aspx?id=" + id + "&type=Direct",
						string.Format(
							"Their HR status shouldn't be Pending Closure still, on the member page, complete thier DIRECT checklist and send them to PAST status."),
						dt_insert, "", "Pending Closure HR Status required for " + dr["member_fullname"],
					$@"127/employees/{id}/termination",
					id);
				}
				//}

				#endregion

				#region stale customers

				dt = Toolbox.doSQL_dt(conn,
					@"SELECT a.customer_id id, customer.customer_name, b.customer_history_memberid member_id, b.customer_history_date date_insert, a.History_Action_Type_Action last_action, datediff(curdate(),b.customer_history_date) days_since FROM ( SELECT customer_history_custid customer_id, MAX(customer_history_id) history_id, h.History_Action_Type_Action FROM customer_history inner join history_action_type h on h.History_Action_Type_ID = customer_history_action where customer_history_action in (1,2,4,5,7,8,9,10,11,12,17,22) GROUP BY customer_history_custid ) a left join customer on a.customer_id = customer.customer_id left join customer_sales_properties on customer_sales_properties.customer_id = customer.customer_id LEFT JOIN customer_history b ON a.history_id = b.customer_history_id where customer_sales_properties.account_manager=@v0  and customer.Customer_Status not in (1,2,4,5,6) and customer.Customer_Hold = 'F' and datediff(curdate(),b.customer_history_date)>365 order by datediff(curdate(),b.customer_history_date) desc limit 10",
					new object[] { myMember.id });
				foreach (DataRow dr in dt.Rows)
				{
					var id = dr["id"];
					var dt_insert = Convert.ToDateTime(dr["date_insert"]).ToString("yyyy-MM-dd");
					dt_final.Rows.Add("/#/opens/10/customers/" + id,
						"Customer has not been contacted in " + dr["days_since"] +
						" days AND you are set as the account manager for them", dt_insert, "", dr["customer_name"],
						"10/customers", id);
				}

				#endregion

				#region applicants clean up

				/*			dt = Toolbox.doSQL_dt(@"SELECT a.vacation_id id, FUN_TIME(a.date_insert) date_insert, date_start, date_return, b.member_fullname FROM vacation_master a LEFT JOIN member b ON a.member_id = b.member_id WHERE a.status = 1 AND b.reports_to=@v0  ORDER BY a.date_start", new object[] {  myMember.id } );
                foreach (DataRow dr in dt.Rows)
                {
                    var id = dr["id"];
                    DateTime dt_start = Convert.ToDateTime(dr["date_start"]);
                    DateTime dt_return = Convert.ToDateTime(dr["date_return"]);
                    string dt_insert = dr["date_insert"].ToString();
                    dt_final.Rows.Add("/sections/hr/vacation_admin/index.aspx#v" + id, string.Format("Starts: {0:yyyy-MM-dd}, Returns: {1:yyyy-MM-dd}", dt_start, dt_return), dt_insert, "", "Vacation Request for " + dr["member_fullname"]);
                }
        */

				#endregion


				if ((myMember.AuthenticatedForPage(190) && myMember.business_unit_id == 11))
				{
					#region Credit Card Clean Expiries

					dt = Toolbox.doSQL_dt(conn,
						@"SELECT FUN_TIME(a.date_of_expiry) date_insert, b.member_fullname FROM credit_cards a LEFT JOIN member b ON a.member_id = b.member_id WHERE b.member_status='Active' and a.`status` = 'Active' and a.date_of_expiry < (curdate() + interval 1 month)   AND FIND_IN_SET(
    b.business_unit_id,
    get_visible_business_units_group_concat (@v0)
  ) ORDER BY a.date_of_expiry Limit 20", new object[] { myMember.id });
					foreach (DataRow dr in dt.Rows)
					{
						var dt_insert = dr["date_insert"].ToString();
						dt_final.Rows.Add("/sections/accounting/credit_cards.aspx",
							"Credit Card Expiring for " + dr["member_fullname"], dt_insert, "",
							"Credit Card is expiring on " + dt_insert);
					}

					#endregion

					#region Credit Card Cancellatios from Terminated People

					dt = Toolbox.doSQL_dt(conn,
						@"SELECT FUN_TIME (b.Member_TermDate) date_insert, b.member_fullname FROM credit_cards a LEFT JOIN member b ON a.member_id = b.member_id WHERE b.member_status = 'Not Active' and a.`status` = 'Active' 
  AND FIND_IN_SET(
    b.business_unit_id,
    get_visible_business_units_group_concat (@v0)
  ) ORDER BY b.Member_TermDate Limit 20", new object[] { myMember.id });
					foreach (DataRow dr in dt.Rows)
					{
						var dt_insert = dr["date_insert"].ToString();
						dt_final.Rows.Add("/sections/accounting/credit_cards.aspx",
							"Credit Card needs to be cancelled for " + dr["member_fullname"] +
							" because they are no longer Active employees", dt_insert, "",
							"Employment term date was " + dt_insert);
					}

					#endregion
				}

				#region Probation Period Action

				dt = Toolbox.doSQL_dt(conn,
					@" select member_id, fun_time(member_startdate + INTERVAL 90 day) prob_over, datediff(member_startdate + INTERVAL 90 DAY, curdate()) days,member_fullname from member where `member_hrstatus_id` = 6 AND DATEDIFF(member_startdate + INTERVAL 90 DAY, CURDATE()) >= 0 AND DATEDIFF(member_startdate + INTERVAL 90 DAY, CURDATE()) <= 14 and reports_to = @v0 
  AND FIND_IN_SET(
    member.business_unit_id,
    get_visible_business_units_group_concat (@v0)
  ) Limit 20", new object[] { myMember.id });
				foreach (DataRow dr in dt.Rows)
				{
					var id = dr["member_id"];
					var dt_insert = dr["prob_over"].ToString();
					var day = Convert.ToInt32(dr["days"]);
					var days = "";
					if (day >= 2)
					{
						days = day + " days";
					}
					else
					{
						days = day + " day";
					}
					dt_final.Rows.Add("/#/opens/127/employees/" + id,
						"Probation period over in " + days + " for " + dr["member_fullname"], dt_insert, "",
						"Probation almost over for " + dr["member_fullname"],
						$@"127/employees/{id}/userinfo",
						id
						);
				}

				#endregion

				#region quote follow_up
				dt = Toolbox.doSQL_dt(@"
						SELECT a.quote_id, a.revision, a.pricetype_id, a.price_to, 
						c.type pricetype, 
						FUN_TIME(a.open_date) open_date, 
						a.quoted_price, 
						a.customer_id, a.job_description, IFNULL(a.allowed_to_quote, 0) allowed_to_quote,
						IFNULL(a.allowed_to_quote_2, 0) allowed_to_quote_2, 
						IFNULL(a.expected_value, 0) expected_value, 
						IFNULL(d.uses_quote_process, 0) uses_quote_process, 
						IFNULL(d.quote_level_2_start, 0) quote_level_2_start, 
						IFNULL(d.quote_level_3_start, 0) quote_level_3_start, 
						a.quoted_by, e.member_fullname, d.ddl_name 
						FROM quote_master a LEFT JOIN quote_follow_up b ON a.quote_id = b.quote_id AND a.revision = b.revision and b.is_done=0
						LEFT JOIN quote_pricetype c ON a.pricetype_id = c.id
						INNER JOIN business_unit d ON a.business_unit_id = d.id 
						LEFT JOIN member e ON a.quoted_by = e.member_id WHERE a.quoted_by = @v0 AND a.active_revision = 1  
						AND b.schedule_date <= CURDATE() AND a.status_id NOT IN (6, 7, 8, 9) 
						GROUP BY quote_id, revision", new object[] { myMember.id });

				foreach (DataRow dr in dt.Rows)
				{
					var id = dr["quote_id"];
					var rev = dr["revision"];
					var dt_insert = dr["open_date"];
					dt_final.Rows.Add(string.Format("/#/opens/65/quotes/{0}/{1}", id, rev),
						$"Quote # {id} need to be followed up", dt_insert, "", "Follow up quote: " + id,
						$@"65/quotes/{id}/{rev}",
						$@"{id},{rev}"
						);
				}
				#endregion
			}
		}

        private static bool IsMemberIDInHRList(int memberID, string hrList)
        {
            if (string.IsNullOrEmpty(hrList) || memberID == 0 )
            {
                return false;
            }

            var idList = hrList.Split(',');
            foreach (var val in idList)
            {
                int id = 0;
                if (int.TryParse(val, out id))
                {
                    if (id == memberID)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
	}
}