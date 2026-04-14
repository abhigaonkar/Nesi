using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.Xpo;
using ne_xpo.cs;

namespace nesi.core
{
	/// <summary>
	/// The NESI ticket system
	/// </summary>
	public class NETickets
	{
		#region Get Set Variables
		//Ticket Header
		public int createdby_member_id { get; set; }
		public int id { get; set; }
		public int issuetype { get; set; }
		public int member_assigned_id { get; set; }
		public int waiting_on_member_id { get; set; }
		public int module_id { get; set; }
		public int objective_owner { get; set; }
		public int priority_id { get; set; }
		public int status_id { get; set; }

		public double expected_hours { get; set; }
		public double hours_worked { get; set; }

		public string issue { get; set; }
		public string roi { get; set; }
		public string symptom { get; set; }
		public string cause { get; set; }
		public string solution { get; set; }
		public string notes { get; set; }
		public string deliverable { get; set; }

		public DateTime created_date { get; set; }
		public DateTime modified_date { get; set; }
		public DateTime expected_completion { get; set; }
		public DateTime assigned_date { get; set; }
		public DateTime closed_date { get; set; }

		public bool is_private { get; set; }
		public bool on_beta { get; set; }

		public ticketheader xpo_ref { get; set; }
		#endregion
		public NETickets() { }
		public NETickets(int _id)
		{
			if (exists(_id))
			{
				load(_id);
			}
		}
		public bool exists(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				var th = uow.GetObjectByKey<ticketheader>(_id);
				return th != null;
			}
		}
		private void load(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				var th = uow.GetObjectByKey<ticketheader>(_id);
				createdby_member_id = th.ticketheader_createdby_member_id == null ? 0 : th.ticketheader_createdby_member_id.member_id;
				member_assigned_id = th.ticketheader_member_assigned_id == null ? 0 : th.ticketheader_member_assigned_id.member_id;
				objective_owner = th.ticketheader_objective_owner == null ? 0 : th.ticketheader_objective_owner.member_id;
				created_date = th.ticketheader_created_date;
				modified_date = th.ticketheader_modified_date;
				module_id = th.ticketheader_module_id == null ? 0 : Convert.ToInt32(th.ticketheader_module_id.ticketpage_id);
				status_id = th.ticketheader_status_id == null ? 0 : Convert.ToInt32(th.ticketheader_status_id.ticketstatus_id);
				is_private = th.ticketheader_private;
				expected_completion = th.ticket_header_expected_completion;
				expected_hours = th.ticket_header_expected_hours;
				issue = th.ticketheader_issue;
				assigned_date = th.ticketheader_assigned_date;
				closed_date = th.ticketheader_closed_date;
				hours_worked = th.ticketheader_hours_worked;
				priority_id = th.ticketheader_priority_id == null ? 0 : Convert.ToInt32(th.ticketheader_priority_id.ticketpriority_id);
				deliverable = th.ticketheader_deliverable;
				roi = th.ticketheader_roi;
				cause = th.ticketheader_cause;
				solution = th.ticketheader_solution;
				symptom = th.ticketheader_symptom;
				notes = th.ticketheader_notes;
				on_beta = th.on_beta;
				issuetype = th.ticketheader_issuetype.tickettype_id;
				//release_id					= th.release_id == null ? 0 : th.release_id.id;
				waiting_on_member_id = th.ticketheader_waitingon_member_id == null ? 0 : th.ticketheader_waitingon_member_id.member_id;
				id = _id;
				xpo_ref = th;
			}
		}
		public void save()
		{
			if (id == 0)
			{
				if (member_assigned_id != 0)
				{
					if (member_assigned_id == createdby_member_id)
					{
						status_id = 6;  // if the created is assigning it to himself set it to read
					}
					else
					{
						status_id = 8;  // if its being assigned to another user
					}
					assigned_date = DateTime.Now;
				}
				else
				{
					status_id = 1;
				}
			}
			using (var uow = new UnitOfWork())
			{
				var th = id == 0
					? new ticketheader(uow)
					: uow.GetObjectByKey<ticketheader>(id);
				th.ticketheader_issue = issue;
				th.ticketheader_issuetype = uow.GetObjectByKey<tickettype>(issuetype);
				th.ticketheader_createdby_member_id = uow.GetObjectByKey<member>(createdby_member_id);
				th.ticketheader_created_date = id == 0 ? DateTime.Now : th.ticketheader_created_date;
				th.ticketheader_modified_date = DateTime.Now;
				th.ticketheader_module_id = uow.GetObjectByKey<ne_xpo.cs.ticketpage>(module_id);
				th.ticketheader_status_id = uow.GetObjectByKey<ticketstatus>(status_id);
				th.ticketheader_waitingon_member_id = uow.GetObjectByKey<member>(waiting_on_member_id);
				th.ticketheader_private = is_private;
				th.ticket_header_expected_hours = expected_hours;
				th.ticketheader_roi = roi;
				th.ticketheader_symptom = symptom;
				th.ticketheader_cause = cause;
				th.ticketheader_solution = solution;
				th.ticketheader_deliverable = deliverable;
				th.ticketheader_notes = notes;
				th.ticketheader_closed_date = closed_date;
				th.on_beta = on_beta;
				th.ticketheader_member_assigned_id = member_assigned_id > 0
					? uow.GetObjectByKey<member>(member_assigned_id)
					: null;
				th.ticketheader_objective_owner = uow.GetObjectByKey<member>(objective_owner);

				if (uow.GetObjectByKey<ticketpriority>(priority_id) == null)
				{
					var _tools = new Toolbox();
					HttpRequest req = null;
					if (HttpContext.Current != null)
					{
						req = HttpContext.Current.Request;
					}
					var current_req = "";
					if (req != null)
					{
						current_req = Toolbox.dict_dump(Toolbox.dict_create(req));
					}
					_tools.catch_error(new Exception(string.Format(@"Null priority caught\n Ticket #{0},\n Issue: {1},\n Current Request Information (if possible):\n {2}", id, issue, current_req)));
				}
				th.ticketheader_priority_id = uow.GetObjectByKey<ticketpriority>(priority_id);
				if (th.ticketheader_priority_id == null)
				{
					th.ticketheader_priority_id = uow.GetObjectByKey<ticketpriority>(1);
				}
				//th.release_id								= uow.GetObjectByKey<ne_xpo.cs.releases>(release_id);
				th.ticket_header_expected_completion = expected_completion;
				th.Save();

				try
				{
					uow.CommitChanges();
				}
				catch (Exception ee)
					{
					if (ee.Message.Contains("\\x"))
					{
						throw new Exception("If you are trying to paste text from a Microsoft Word application, like Outlook, please use the 'Paste from Word' button in the ticket body editor");
					}

					throw;
					}
				id = th.ticketheader_id;
			}
		}
		public static IOrderedEnumerable<ticketissue> get_ticket_issues(int ticket_id)
		{
			using (var uow = new UnitOfWork())
			{
				return (from t in new XPQuery<ticketissues>(uow)
						where
						t.ticketissues_ticketheader_id == uow.GetObjectByKey<ticketheader>(ticket_id)
						select new ticketissue
							       {
							id = t.ticketissues_id,
							created_member_id = t.ticketissues_created_member_id.member_id,
							created_date = t.ticketissues_created_date,
							message = t.ticketissues_message,
							uploaded_file = t.ticketissues_uploaded_file
						}).ToList().OrderByDescending(y => y.id);
			}
		}
		public static void delete_viewer_from_ticket(int _id)
		{
			using (var uow = new UnitOfWork())
			{
				var tmv = uow.GetObjectByKey<ne_xpo.cs.ticket_memberview>(_id);
				if (tmv != null)
				{
					tmv.Delete();
				}
				uow.CommitChanges();
			}
		}
		public static void add_viewer_to_ticket(int new_member_id, NeMember submitted_by_user, int ticket_id)
		{
			var _tools = new Toolbox();
			if (_tools.getSQL_int(@"Select count(id) from ticket_memberview  where ticket_id =@v0 and member_id =@v1 ", new object[] { ticket_id,new_member_id }) == 0)
			{
				using (var uow = new UnitOfWork())
				{
					var tmv = new ne_xpo.cs.ticket_memberview(uow);
					tmv.member_id = uow.GetObjectByKey<member>(new_member_id);
					tmv.ticket_id = uow.GetObjectByKey<ticketheader>(ticket_id);
					tmv.Save();
					uow.CommitChanges();
				}
				try
				{
                    if (submitted_by_user.id32 != new_member_id)
                    {
                        var ticket = new NETickets(ticket_id);
						if(DateTime.Now.Hour > 20)
							{
							return;
							}
	                    var email = new NeEMail
		                                {
		                                Subject = "You have been added as viewer to ticket " + ticket.id +
		                                          "-" + ticket.issue + "  by " + submitted_by_user.FullName,
		                                From   = submitted_by_user.NEEmail,
		                                To     = new NeMember(new_member_id).NEEmail,
		                                isHTML = true
		                                };
	                    var body = new StringBuilder();
                        body.AppendFormat(@"
<div style='font-family:arial;font-size:12px'>
	<b>Ticket: </b><a href='{4}/sections/member/tickets/ticketpage.aspx?issue={0}&is_n1=true' target = 'blank'>{0}</a><br/>
	<b>Subject: </b>{1}<br/>
	<b>Created By: </b>{2}<br/>
	<b>Status: </b>{3}<br/><br/>",
                            ticket_id,
                            ticket.issue,
                            ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname,
                            ticket.xpo_ref.ticketheader_status_id.ticketstatus_status,
                            Toolbox.app_setting("Domain")
                        );
                        using (var uow = new UnitOfWork())
                        {
                            var ticket_list = get_ticket_issues(ticket_id);
                            foreach (var ti in ticket_list)
                            {
                                var user = uow.GetObjectByKey<member>(ti.created_member_id);
                                body.AppendFormat(@"
<b>{0:yyyy-MM-dd}</b> - {1}
{2}<br/><br/>
", ti.created_date, user.member_fullname, ti.message);
                            }
                        }

                        email.Body = body.ToString();
                        email.Send();
                    }
				}
				catch (Exception ee)
				{
					_tools.catch_error(ee);
				}
			}
		}
		public DataTable getticketstatuslist()
		{
			return Toolbox.doSQL_dt(@"SELECT ticketstatus_id AS ID, ticketstatus_status AS NAME FROM ticketstatus"  , null);
		}
		public DataTable getticketpagelist()
		{
			return Toolbox.doSQL_dt(@"SELECT ticketpage_id AS ID, ticketpage_name AS NAME FROM ticketpage"  , null);
		}
		public DataTable getticketpriotiylist()
		{
			return Toolbox.doSQL_dt(@"SELECT ticketpriority_id AS ID, ticketpriority_name AS NAME FROM ticketpriority"  , null);
		}
		public static DataTable GetCreatedissues(string query, int current_user)
		{
			query = query.ToLower();
			using (var uow = new UnitOfWork())
			{
				// Relevance Level 0 (RL0) - It's a ticket # and the ticket exists --
				// Relevance Level 1 (RL1) - Exact search for phrase in ticket subject
				// Relevance Level 2 (RL2) - Exact search for phrase in ticket issue list
				// Relevance Level 3 (RL3) - Match all words provided (as whole words), in a ticket
				// Relevance Level 4 (RL4) - Match all words provided (can be partial words), in a ticket

				var results = new Dictionary<int, List<int>>();
				results.Add(0, new List<int>());
				results.Add(1, new List<int>());
				results.Add(2, new List<int>());
				results.Add(3, new List<int>());
				results.Add(4, new List<int>());

				var search_types = new Dictionary<int, string>();
				search_types.Add(0, "Exact - Ticket #");
				search_types.Add(1, "Exact - Ticket Subject");
				search_types.Add(2, "Exact - Ticket Issue");
				search_types.Add(3, "Exact - All words exist");
				search_types.Add(4, "Partial - All words exist");

				var relevances = new Dictionary<int, int>();
				relevances.Add(0, 100);
				relevances.Add(1, 100);
				relevances.Add(2, 100);
				relevances.Add(3, 100);
				relevances.Add(4, 80);

				var t = new NETickets();
				var ticket_id = 0;
				int.TryParse(query, out ticket_id);
				if (ticket_id > 0 && t.exists(ticket_id))
				{
					// RL0 - Just need to return the ticket
					results[0].Add(ticket_id);
				}

				var queries = query.Split(' ');
				var curr_user = (from y in new XPQuery<member>(uow)
								 where y.member_id == current_user
								 select y).FirstOrDefault();
				var dict_head = new Dictionary<int, string>();
				var dict_issues = new Dictionary<int, string>();
				var dict_full = new Dictionary<int, string>();
				var dt_headers = from y in new XPQuery<ticketheader>(uow)
								 where
								 !y.ticketheader_private ||
								 (
									 y.ticketheader_private &&
									 (
										 y.ticketheader_createdby_member_id == curr_user ||
										 y.ticketheader_member_assigned_id == curr_user ||
										 y.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator == curr_user
									 )
								 )
								 select new
								 {
									 id = y.ticketheader_id,
									 text = y.ticketheader_issue.Trim().ToLower()
								 };
				var dt_issues = from y in new XPQuery<ticketissues>(uow)
								where
								!y.ticketissues_ticketheader_id.ticketheader_private ||
								(
									y.ticketissues_ticketheader_id.ticketheader_private &&
									(
										y.ticketissues_ticketheader_id.ticketheader_createdby_member_id == curr_user ||
										y.ticketissues_ticketheader_id.ticketheader_member_assigned_id == curr_user ||
										y.ticketissues_ticketheader_id.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator == curr_user
									)
								)
								select new
								{
									id = y.ticketissues_ticketheader_id.ticketheader_id,
									text = y.ticketissues_message.Trim().ToLower()
								};

				foreach (var dr in dt_headers)
				{
					dict_head[dr.id] = dr.text;
					dict_full[dr.id] = dr.text;
					dict_issues[dr.id] = "";
				}
				// Do a level 1 check
				var level_1_chk = from y in dict_head where y.Value.Contains(query) select new { id = y.Key };
				foreach (var l1c in level_1_chk)
				{
					if (!results[0].Contains(l1c.id))
					{
						results[1].Add(l1c.id);
					}
				}

				foreach (var dr in dt_issues)
				{
					if (dict_full.ContainsKey(dr.id))
					{
						dict_issues[dr.id] += " " + dr.text;
						dict_full[dr.id] += " " + dr.text;
					}
				}
				var level_2_chk = from y in dict_issues where y.Value.Contains(query) select new { id = y.Key };
				foreach (var l2c in level_2_chk)
				{
					if (!results[0].Contains(l2c.id) && !results[1].Contains(l2c.id))
					{
						results[2].Add(l2c.id);
					}
				}

				var level_3_chk = from y in dict_full select new { id = y.Key, message = y.Value };
				foreach (var q in queries)
				{
					level_3_chk = from y in level_3_chk where y.message.Contains(" " + q + " ") select y;
				}
				foreach (var l3c in level_3_chk)
				{
					if (!results[0].Contains(l3c.id) && !results[1].Contains(l3c.id) && !results[2].Contains(l3c.id))
					{
						results[3].Add(l3c.id);
					}
				}

				var level_4_chk = from y in dict_full select new { id = y.Key, message = y.Value };
				foreach (var q in queries)
				{
					level_4_chk = from y in level_4_chk where y.message.Contains(q) select y;
				}
				foreach (var l4c in level_4_chk)
				{
					if (!results[0].Contains(l4c.id) && !results[1].Contains(l4c.id) && !results[2].Contains(l4c.id) && !results[3].Contains(l4c.id))
					{
						results[4].Add(l4c.id);
					}
				}
				var final_select = "";
				var select_template = @"
SELECT 
	'{0}' TypeOf, 
	a.ticketheader_id id,
	a.ticketheader_id, 
	a.ticketheader_issue RaisedIssue,
	a.ticketheader_issue issue,
	a.ticketheader_modified_date, 
	a.ticketheader_created_date DateCreated, 
	b.ticketpage_id pageid, 
	b.ticketpage_name PageName, 
	c.ticketstatus_status Status, 
	d.tickettype_id typeid, 
	d.tickettype_name TypeOfTicket, 
	f.member_fullname AsignedTo, 
	g.member_fullname RaisedBy, 
	e.ticketpriority_id priority_id,
	e.ticketpriority_name Priority,
	CAST({2} AS DECIMAL(5,2)) Relevance, 
	h.ticket_group_id groupid,
	h.ticket_group_name groupname,
	a.ticket_header_expected_completion exp_fin, 
	a.release_id,
	a.ticketheader_status_id status_id,
	if(a.ticketheader_issuetype = 3 AND a.ticketheader_status_id NOT IN (5,7,10,11,12),total_votes(a.ticketheader_id), -1) v
FROM 
	ticketheader AS a 
LEFT JOIN 
	member AS f ON 
		a.ticketheader_member_assigned_id = f.member_id
LEFT JOIN
	ticketpage AS b ON 
		a.ticketheader_module_id = b.ticketpage_id 
LEFT JOIN
	ticketstatus AS c ON  
		a.ticketheader_status_id = c.ticketstatus_id
LEFT JOIN 
	tickettype AS d ON 
		a.ticketheader_issuetype = d.tickettype_id
LEFT JOIN 
	ticketpriority AS e ON  
		a.ticketheader_priority_id = e.ticketpriority_id 
LEFT JOIN 
	member AS g ON  
		a.ticketheader_createdby_member_id = g.member_id
LEFT JOIN 
	ticket_group h ON 
		b.ticketpage_ticket_group_id = h.ticket_group_id 
WHERE 
	a.ticketheader_id IN ({1})
UNION";
				var sb = new StringBuilder();
				foreach (var k in results)
				{
					var search_type = search_types[k.Key];
					var relevance = relevances[k.Key];
					var ids = new List<int>();
					foreach (var i in k.Value)
					{
						ids.Add(i);
					}
					if (ids.Count() > 0)
					{
						final_select += string.Format(select_template, search_type, string.Join(",", ids), relevance);
					}
				}
				if (final_select == "")
				{
					final_select += string.Format(select_template, "No Results", "NULL", 0);
				}
				final_select = final_select.Substring(0, final_select.Length - 5) + " ORDER BY DateCreated DESC, Relevance ASC"; ;
				return Toolbox.doSQL_dt(final_select,null);
			}
		}
		public DataTable GetTicketManagerList(int groupid)
		{
			var sqlSelect = "SELECT member_id AS ID, get_name(member_id) AS NAME FROM member, ticketmanager WHERE ticketmanager_member_id = member_id and ticketmanager_group_id = " + groupid + " and member_status = 'Active' order by member_fullname";
			var datamanagers = Toolbox.doSQL_dt(@"SELECT member_id AS ID, get_name(member_id) AS NAME FROM member, ticketmanager  WHERE ticketmanager_member_id = member_id and ticketmanager_group_id =@v0 and member_status = 'Active' order by member_fullname", new object[] { groupid });
			return datamanagers;
		}
		public void save_with_history(NeMember member)
		{
			var prev_ticket = new NETickets(id);
			using (var uow = new UnitOfWork())
			{
				if (status_id == 5 && prev_ticket.status_id != 5)
				{
					closed_date = DateTime.Now;
				}
				save();
				#region Expected Completion
				if (prev_ticket.expected_completion != expected_completion)
				{
					if (prev_ticket.expected_completion.Year > 2000)
					{
						if ((prev_ticket.expected_completion.Subtract(DateTime.Now).Days < 7) && (member.id == member_assigned_id))
						{
							var issue = new ticketissue();
							issue.ticketheader_id = id;
							issue.message = "\n Firm Completion Date Set by " + member.FullName + " to " + expected_completion.ToString("yyyy-MM-dd");
							issue.created_member_id = member.id;
							issue.was_date = prev_ticket.expected_completion;
							issue.new_date = expected_completion;
							issue.save();
						}
					}
				}
				#endregion
				#region Status
				var prev_status = uow.GetObjectByKey<ticketstatus>(prev_ticket.status_id);
				var this_status = uow.GetObjectByKey<ticketstatus>(status_id);

				var pre_status_text = prev_status.ticketstatus_id == 3  // status 3  = waiting for user response
					? (prev_ticket.xpo_ref.ticketheader_waitingon_member_id == null
						? "Waiting for reply from " + prev_ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname
						: "Waiting for reply from " + prev_ticket.xpo_ref.ticketheader_waitingon_member_id.member_fullname)
					: prev_status.ticketstatus_id == 9
						? (prev_ticket.xpo_ref.ticketheader_waitingon_member_id == null
							? "Answered by " + prev_ticket.xpo_ref.ticketheader_createdby_member_id.member_fullname
							: "Answered by " + prev_ticket.xpo_ref.ticketheader_waitingon_member_id.member_fullname)
						: prev_ticket.xpo_ref.ticketheader_status_id.ticketstatus_status;
				var new_status_text = this_status.ticketstatus_id == 3   // status 3  = waiting for user response
                    ? "Waiting for reply from " + new NeMember(waiting_on_member_id).FullName
					: this_status.ticketstatus_id == 9   // status 9  = answered by user
                        ? "Answered by " + new NeMember(waiting_on_member_id).FullName
						: this_status.ticketstatus_status;
				if (prev_ticket.status_id != status_id)
				{
					var issue = new ticketissue();
					issue.ticketheader_id = id;
					issue.message = status_id == 5   // status 5  = closed
                        ? "This ticket was manually closed by " + member.FullName
						: "Manually updated status from '" + pre_status_text + "' to '" + new_status_text + "'";
					issue.created_member_id = member.id;
					issue.save();
				}
				#endregion


				if (prev_ticket.member_assigned_id != member_assigned_id && status_id != 5) // if the ticket assignee has been changed and the ticket is not closed
				{
					if (member_assigned_id == 0)  // if the user is set to no one, reset the status to unassigned
					{
						status_id = 1;
						save();
					}
					else
					{
						if (member_assigned_id == createdby_member_id || member_assigned_id == member.id)
						{
							status_id = 6;
							save();
						}
						else
						{
							status_id = 8;
							save();
						}
					}

					#region Send The Assigned User An Email
					if (member_assigned_id != 0 && member_assigned_id != member.id)
					{
						var assigned_user = uow.GetObjectByKey<member>(member_assigned_id);
						var created_user = uow.GetObjectByKey<member>(createdby_member_id);
					var mail = new NeEMail
						           {
						           To      = assigned_user.member_neemail,
						           Subject = "I just assigned you ticket: " + id,
						           From    = member.NEEmail,
						           isHTML  = true,
						           Body = string.Format(@"
<div style='font-family:arial;font-size:12px;'>
	<div><b>Ticket:</b> <a href='{8}/sections/member/tickets/ticketpage.aspx?issue={0}&is_n1=true' target = 'blank'>{0}</a></div>
	<div><b>Subject:</b>{1}</div>
	<div><b>Created By:</b> {2} </div>
	<div><b>Status:</b> {3}</div>
	<div><b>Priority:</b> {4}</div>
	<div><b>Assigned to:</b> {5}</div>
	<div><b>Expected Hours: </b> {6}</div>
	<div><b>Expected Completion Date: </b> {7}</div>
</div>
",
							           id, // {0}
							           issue, // {1}
							           created_user.member_fullname, // {2}
							           new_status_text, // {3}
							           xpo_ref.ticketheader_priority_id == null
								           ? "Not Set"
								           : xpo_ref.ticketheader_priority_id.ticketpriority_name, // {4}
							           xpo_ref.ticketheader_member_assigned_id == null
								           ? "Not Set"
								           : xpo_ref.ticketheader_member_assigned_id.member_fullname, // {5}
							           xpo_ref.ticket_header_expected_hours == 0
								           ? "Not Set"
								           : xpo_ref.ticket_header_expected_hours.ToString(), // {6}
							           xpo_ref.ticket_header_expected_completion.Year < 2000
								           ? "Not Set"
								           : Toolbox.MySQL_shortdt(xpo_ref.ticket_header_expected_completion), // {7}
                                           Toolbox.app_setting("Domain")
                                       )
						           };
					mail.Send();
					}
					#endregion
				}
				else
				{
					SendMessage(member.id);
				}
			}
		}
		public static void send_new_ticket_alert(int ticket_id, NeMember _user, string _subject, string _body)
		{
			var t = new NETickets(ticket_id);
			var owner = t.xpo_ref.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator;
			var ticket_module	= new ticketpage(t.module_id);
			if (Convert.ToInt32(owner.member_id) != _user.id)
			{
			var mail = new NeEMail
				           {
				           To      = owner.member_neemail,
				           Subject = "A New Ticket Has Been Created: Ticket ID - " + ticket_id,
				           From    = "administrator@" + Toolbox.app_setting("DomainForEmail"),
				           isHTML  = true,
				           Body = string.Format(@"
<div style='font-family:arial;font-size:12px'>
<b>Ticket:</b> <a href='{5}/sections/member/tickets/ticketpage.aspx?issue={3}&is_n1=true' target = 'blank'>{3}</a><br/>
<u><b>Created By: </b></u><br/>
{0}<br/><br/>
<u><b>Regarding Page/Module:</br></u><br/>
{4}<br/><br/>
<u><b>Subject:</b></u><br/>
{1}<br/><br/>
<u><b>Issue Details:</b></u><br/>
{2}<br/> <br/>
Please visit the ticket system for more details.</div>", _user.FullName, _subject, _body, ticket_id,
					           ticket_module.name, Toolbox.app_setting("Domain"))
				           };
			mail.Send();
			}
		}
		public void SendMessage(int memberid)
		{
			if(DateTime.Now.Hour > 20)
				{
				return;
				}
			var _tools = new Toolbox();
			using (var uow = new UnitOfWork())
			{
				var user = uow.GetObjectByKey<member>(memberid);
				var th = uow.GetObjectByKey<ticketheader>(id);

				//Who do we send this message to?
				#region Format Message Informataion For body
				var issues = (from i in new XPQuery<ticketissues>(uow)
							  where i.ticketissues_ticketheader_id.ticketheader_id == th.ticketheader_id
							  select new
							  {
								  issue_id = i.ticketissues_id,
								  create_date = i.ticketissues_created_date,
								  fullname = i.ticketissues_created_member_id.member_fullname,
								  message = i.ticketissues_message.Replace("\n", "<br/>")
							  }).OrderByDescending(x => x.issue_id);
				var body = new StringBuilder();
				var this_status = uow.GetObjectByKey<ticketstatus>(status_id);
				var new_status_text = this_status.ticketstatus_id == 3
					? "Waiting for reply from " + new NeMember(waiting_on_member_id).FullName
					: this_status.ticketstatus_id == 9
						? "Answered by " + new NeMember(waiting_on_member_id).FullName
						: this_status.ticketstatus_status;
				body.AppendFormat(@"
<div style='font-family:arial;font-size:12px;'>
	<div><b>Ticket:</b> <a href='{8}/sections/member/tickets/ticketpage.aspx?issue={0}&is_n1=true' target = 'blank'>{0}</a></div>
	<div><b>Subject:</b>{1}</div>
	<div><b>Created By:</b> {2} </div>
	<div><b>Status:</b> {3}</div>
	<div><b>Priority:</b> {4}</div>
	<div><b>Assigned to:</b> {5}</div>
	<div><b>Expected Hours: </b> {6}</div>
	<div><b>Expected Completion Date: </b> {7}</div>
</div>
<br />
<br />
<div style='font-family:arial;font-size:12px;'>
",
					th.ticketheader_id,                                                                                                         // {0}
					th.ticketheader_issue,                                                                                                      // {1}
					th.ticketheader_createdby_member_id.member_fullname,                                                                        // {2}
					new_status_text,                                                                                                            // {3}
					th.ticketheader_priority_id == null ? "Not Set" : th.ticketheader_priority_id.ticketpriority_name,                          // {4}
					th.ticketheader_member_assigned_id == null ? "Not Set" : th.ticketheader_member_assigned_id.member_fullname,                // {5}
					th.ticket_header_expected_hours == 0 ? "Not Set" : th.ticket_header_expected_hours.ToString(),                          // {6}
					th.ticket_header_expected_completion.Year < 2000 ? "Not Set" : Toolbox.MySQL_shortdt(th.ticket_header_expected_completion),      // {7}
                    Toolbox.app_setting("Domain")
                );
				foreach (var i in issues)
				{
					body.AppendFormat(@"
<br/>
<div>
<b>{1}</b> - {0:dddd, dd MMMM yyyy HH:mm}<br/>
{2}
</div>
<hr/>",
						i.create_date,
						i.fullname,
						i.message.Trim()
					);
				}
				body.Append(@"
</div>
</div>");
				#endregion

				try
				{
					var mail = new NeEMail
					{
						To = "",
						CC = "",
						Body = body.ToString(),
						isHTML = true,
						From = user.member_neemail,
						Subject = string.Format("I've updated ticket {0} - {1}", th.ticketheader_id, th.ticketheader_issue)
					};
					var is_creator = user.member_id == th.ticketheader_createdby_member_id.member_id;
					var is_assignee = th.ticketheader_member_assigned_id != null && user.member_id == th.ticketheader_member_assigned_id.member_id && th.ticketheader_member_assigned_id.member_id != 0;
					var is_group_admin = user.member_id == th.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_id;

					#region send email to creater
					if (!is_creator)
					{
						mail.To = th.ticketheader_createdby_member_id.member_neemail + ";";
					}

					#endregion
					#region send email to assignee
					if (th.ticketheader_member_assigned_id != null && !is_assignee)
					{
						mail.To += th.ticketheader_member_assigned_id.member_neemail + ";";
					}
					#endregion
					#region send email to group manager
					if (!is_group_admin)
					{
						mail.To += th.ticketheader_module_id.ticketpage_ticket_group_id.ticket_group_administrator.member_neemail + ";";
					}
					#endregion
					#region send email to watchers
					var watchers = (from w in new XPQuery<ne_xpo.cs.ticket_memberview>(uow)
									where w.ticket_id.ticketheader_id == th.ticketheader_id && w.member_id.member_id != user.member_id
									select new { email = w.member_id.member_neemail }).ToList().Distinct();
					foreach (var watcher in watchers)
					{
						if (!mail.To.Contains(watcher.email) && !mail.CC.Contains(watcher.email))
						{
							mail.CC += watcher.email + ";";
						}
					}
					#endregion
					if ((mail.To != "")&&(this_status.ticketstatus_id!=5))
					{
						mail.Send();
					}
				}
				catch (Exception ee)
				{
					_tools.catch_error(ee);
					throw new Exception("The program Failed to Email the user about this change.");
				}
			}
		}
		public class ticket_task
		{
			public int id { get; set; }
			public int ticket_id { get; set; }
			public string task { get; set; }
			public bool complete { get; set; }
			public int order { get; set; }
			public double cost { get; set; }
			public int assignee { get; set; }
			public ticket_task() { }
			public ticket_task(int _id)
			{
				if (exists(_id))
				{
					load(_id);
				}
			}
			public class ticket_task_uc : UserControl
			{
				public int id { get; set; }
				public string body { get; set; }
				public bool complete { get; set; }
				public bool enabled { get; set; }
				public int assignee { get; set; }
				public Dictionary<int, string> users { get; set; }
			}
			private void load(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ticket_tasks>(_id);
					if (x != null)
					{
						id = x.id;
						ticket_id = x.ticket_id.ticketheader_id;
						task = x.task;
						complete = x.complete;
						order = x.P_order;
						cost = x.cost;
						if (x.assignee != null)
						{
							assignee = Convert.ToInt32(x.assignee.member_id);
						}
					}
				}

			}
			public bool exists(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ticket_tasks>(_id);
					return x != null;
				}

			}
			public int next_order(int _id)
			{

				return (Toolbox.doSQL_int(@"Select ifnull((Select _order from ticket_tasks where ticket_id =@v0 order by _order desc limit 1),0)", _id) + 1);

				/*	using(UnitOfWork uow = new UnitOfWork())
				{
				var x			= (from o in new XPQuery<ne_xpo.cs.ticket_tasks>(uow)
									where o.ticket_id == uow.GetObjectByKey<ne_xpo.cs.ticketheader>(_id) select new {ord = o.P_order}).Max(y => y.ord);
				return Convert.ToInt32(x+1);
		 * 
				}*/

			}
			public void save()
			{
				using (var uow = new UnitOfWork())
				{
					var x = id == 0
						? new ticket_tasks(uow)
						: uow.GetObjectByKey<ticket_tasks>(id);
					x.ticket_id = uow.GetObjectByKey<ticketheader>(ticket_id);
					x.task = task;
					x.complete = complete;
					x.P_order = id == 0
						? next_order(ticket_id)
						: x.P_order;
					x.cost = cost;
					if (assignee > 0)
					{
						x.assignee = uow.GetObjectByKey<member>(assignee);
					}
					x.Save();
					uow.CommitChanges();
					if (id == 0)
					{
						id = x.id;
					}
				}

			}
			public static void delete(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ticket_tasks>(_id);
					if (x != null)
					{
						x.Delete();
					}
					uow.CommitChanges();
				}

			}
		}
		public class ticketpage
		{
			/*
		ticketpage_id				=> id
		ticketpage_name				=> name
		ticketpage_ticket_group_id	=> group_id
		page_id						=> page_id
		 */
			public int id { get; set; }
			public string name { get; set; }
			public int group_id { get; set; }
			public int page_id { get; set; }
			public ticketpage() { }
			public ticketpage(int _id)
			{
				if (exists(_id))
				{
					load(_id);
				}
			}
			private void load(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.ticketpage>(_id);
					if (x != null)
					{
						id = _id;
						name = x.ticketpage_name;
						group_id = x.ticketpage_ticket_group_id.ticket_group_id;
						if(x.page_id != null)
							{
							page_id = x.page_id.page_id;
							}
					}
				}

			}
			public bool exists(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.ticketpage>(_id);
					return x != null;
				}

			}
			public void save()
			{
				using (var uow = new UnitOfWork())
				{
					var x = id == 0
						? new ne_xpo.cs.ticketpage(uow)
						: uow.GetObjectByKey<ne_xpo.cs.ticketpage>(id);
					x.ticketpage_name = name;
					x.page_id = uow.GetObjectByKey<page>(page_id);
					x.ticketpage_ticket_group_id = uow.GetObjectByKey<ticket_group>(group_id);
					x.Save();
					uow.CommitChanges();
					if (id == 0)
					{
						id = x.ticketpage_id;
					}
				}

			}
			public static void delete(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.ticketpage>(_id);
					if (x != null)
					{
						x.Delete();
					}
					uow.CommitChanges();
				}

			}
		}
		public class ticketmanager
		{
			public int id { get; set; }
			public int member_id { get; set; }
			public int group_id { get; set; }
			public ticketmanager() { }
			public ticketmanager(int _id)
			{
				if (exists(_id))
				{
					load(_id);
				}
			}
			private void load(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.ticketmanager>(_id);
					if (x != null)
					{
						id = _id;
						member_id = x.ticketmanager_member_id.member_id;
						group_id = x.ticketmanager_group_id.ticket_group_id;
					}
				}

			}
			public bool exists(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.ticketmanager>(_id);
					return x == null;
				}

			}
			public void save()
			{
				using (var uow = new UnitOfWork())
				{
					var x = id == 0
						? new ne_xpo.cs.ticketmanager(uow)
						: uow.GetObjectByKey<ne_xpo.cs.ticketmanager>(id);
					x.ticketmanager_member_id = uow.GetObjectByKey<member>(member_id);
					x.ticketmanager_group_id = uow.GetObjectByKey<ticket_group>(group_id);
					x.Save();
					uow.CommitChanges();
					if (id == 0)
					{
						id = x.ticketmanager_id;
					}
				}

			}
			public static void delete(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.ticketmanager>(_id);
					if (x != null)
					{
						x.Delete();
					}
					uow.CommitChanges();
				}

			}
		}
		public class ticketissue
		{
			/*
		Table Structure
		ticketissues_id
		ticketissues_ticketheader_id
		ticketissues_message
		ticketissues_created_member_id
		ticketissues_created_date
		ticketissues_uploaded_file
		ticketissues_was_date
		ticketissues_new_date
		 */
			public int id { get; set; }
			public int ticketheader_id { get; set; }
			public int created_member_id { get; set; }
			public string uploaded_file { get; set; }
			public DateTime created_date { get; set; }
			public DateTime was_date { get; set; }
			public DateTime new_date { get; set; }
			public string message { get; set; }
			public static string attachment_folder => shared.UNC_ticket_attachments();
		    public ticketissue() { }
			public ticketissue(int _id)
			{
				if (exists(_id))
				{
					load(_id);
				}
			}
			public bool exists(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					return uow.GetObjectByKey<ticketissues>(_id) != null;
				}
			}
			private void load(int _id)
			{
				var dr = Toolbox.doSQL_dt(@"SELECT * FROM ticketissues WHERE ticketissues_id = @v0 ", new object[] {  _id } ).Rows[0];
				id = _id;
				ticketheader_id = Convert.ToInt32(dr["ticketissues_ticketheader_id"]);
				created_member_id = (int)dr["ticketissues_created_member_id"];
				uploaded_file = dr["ticketissues_was_date"] == DBNull.Value ? "" : dr["ticketissues_uploaded_file"].ToString();
				created_date = Convert.ToDateTime(dr["ticketissues_created_date"]);
				if (dr["ticketissues_was_date"] != DBNull.Value)
				{
					was_date = (DateTime)dr["ticketissues_was_date"];
				}
				if (dr["ticketissues_new_date"] != DBNull.Value)
				{
					new_date = (DateTime)dr["ticketissues_new_date"];
				}
				message = (string)dr["ticketissues_message"];
			}
			public void save()
			{
				using (var uow = new UnitOfWork())
				{
					var ti = id == 0
						? new ticketissues(uow)
						: uow.GetObjectByKey<ticketissues>(id);
					ti.ticketissues_ticketheader_id = uow.GetObjectByKey<ticketheader>(ticketheader_id);
					ti.ticketissues_message = message;
					ti.ticketissues_created_member_id = uow.GetObjectByKey<member>(created_member_id);
					ti.ticketissues_created_date = id == 0 ? DateTime.Now : ti.ticketissues_created_date;
					ti.ticketissues_uploaded_file = uploaded_file ?? "";
					ti.ticketissues_was_date = was_date;
					ti.ticketissues_new_date = new_date;
					ti.Save();
					try
					{
						uow.CommitChanges();
					}
					catch (Exception ee)
						{
						if (ee.Message.Contains("\\x"))
						{
							throw new Exception("If you are trying to paste text from a Microsoft Word application, like Outlook, please use the 'Paste from Word' button in the ticket body editor");
						}

						throw;
						}
					id = ti.ticketissues_id;
				}
			}
			public static void add_attachment(int issue_id, byte[] content, int content_length, string filename, string extension)
			{
				if (extension.Contains("."))
				{
					extension = extension.Replace(".", "");
				}
				var path = Path.Combine(attachment_folder, filename + "." + extension);
				File.WriteAllBytes(path, content);
			}
		}
		public class ticket_memberview
		{
			/*
		Table Structure
		id
		ticket_id
		member_id
		 */
			public int id { get; set; }
			public int ticket_id { get; set; }
			public int member_id { get; set; }
			public ticket_memberview() { }
			public ticket_memberview(int _id)
			{
				if (exists(_id))
				{
					load(_id);
				}
			}
			public ticket_memberview(int _ticket_id, int _member_id)
			{
				var _id = exists(_ticket_id, _member_id);
				if (_id > 0)
				{
					load(_id);
				}
			}
			public bool exists(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					return uow.GetObjectByKey<ne_xpo.cs.ticket_memberview>(_id) != null;
				}
			}
			public int exists(int _ticket_id, int _member_id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = (from w in new XPQuery<ne_xpo.cs.ticket_memberview>(uow)
							 where w.ticket_id.ticketheader_id == _ticket_id && w.member_id.member_id == _member_id
							 select new
							 {
							 w.id
							 }).FirstOrDefault();
					return x == null ? 0 : x.id;
				}
			}
			private void load(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.ticket_memberview>(_id);
					id = x.id;
					member_id = x.member_id.member_id;
					ticket_id = x.ticket_id.ticketheader_id;
				}
			}
			public void save()
			{
				using (var uow = new UnitOfWork())
				{
					var tmv = id == 0
						? new ne_xpo.cs.ticket_memberview(uow)
						: uow.GetObjectByKey<ne_xpo.cs.ticket_memberview>(id);
					tmv.ticket_id = uow.GetObjectByKey<ticketheader>(ticket_id);
					tmv.member_id = uow.GetObjectByKey<member>(member_id);
					tmv.Save();
					uow.CommitChanges();
					id = tmv.id;
				}
			}
			public void delete()
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.ticket_memberview>(id);
					if (x != null)
					{
						x.Delete();
					}
					uow.CommitChanges();
				}
			}
		}
		public class ticket_vote
		{
			/*
		Table Structure
		id
		ticket_id
		member_id
		 */
			public int id { get; set; }
			public int ticketheader_id { get; set; }
			public int member_id { get; set; }
			public DateTime dt { get; set; }
			public ticket_vote() { }
			public ticket_vote(int _id)
			{
				if (exists(_id))
				{
					load(_id);
				}
			}
			public ticket_vote(int _ticket_id, int _member_id)
			{
				var _id = exists(_ticket_id, _member_id);
				if (_id > 0)
				{
					load(_id);
				}
			}
			public bool exists(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					return uow.GetObjectByKey<ne_xpo.cs.ticket_vote>(_id) != null;
				}
			}
			public int exists(int _ticket_id, int _member_id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = (from w in new XPQuery<ne_xpo.cs.ticket_vote>(uow)
							 where w.ticketheader_id.ticketheader_id == _ticket_id && w.member_id.member_id == _member_id
							 select new
							 {
							 w.id
							 }).FirstOrDefault();
					return x == null ? 0 : x.id;
				}
			}
			private void load(int _id)
			{
				using (var uow = new UnitOfWork())
				{
					var x = uow.GetObjectByKey<ne_xpo.cs.ticket_vote>(_id);
					id = x.id;
					member_id = x.member_id.member_id;
					ticketheader_id = x.ticketheader_id.ticketheader_id;
					dt = x.dt;
				}
			}
			public void save()
			{
				if (exists(ticketheader_id, member_id) == 0)
				{
					using (var uow = new UnitOfWork())
					{
						var tv = id == 0
							? new ne_xpo.cs.ticket_vote(uow)
							: uow.GetObjectByKey<ne_xpo.cs.ticket_vote>(id);
						tv.ticketheader_id = uow.GetObjectByKey<ticketheader>(ticketheader_id);
						tv.member_id = uow.GetObjectByKey<member>(member_id);
						tv.dt = DateTime.Now;
						tv.Save();
						uow.CommitChanges();
						id = tv.id;
					}
				}
			}
			public void delete()
			{
				if (exists(ticketheader_id, member_id) > 0)
				{
					using (var uow = new UnitOfWork())
					{
						var x = uow.GetObjectByKey<ne_xpo.cs.ticket_vote>(id);
						if (x != null)
						{
							x.Delete();
						}
						uow.CommitChanges();
					}
				}
			}
		}
	}
}