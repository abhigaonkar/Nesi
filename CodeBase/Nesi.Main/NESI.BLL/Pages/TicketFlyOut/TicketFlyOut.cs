using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.TicketFlyOut
{
	public class TicketFlyOut : BLLBase
	{

		public TicketFlyOut()
		{

		}
		public TicketFlyOut(Employee user) : base(user)
		{

		}

		public LabelValueInt[] GetddlGroups()
		{
			return (from x in _db.ticket_group
					select new LabelValueInt()
					{
						Label = x.ticket_group_name,
						Value = x.ticket_group_id
					}).ToArray();
		}
		public LabelValueInt[] GetddlTypes(int groupId)
		{
			return (from x in _db.ticket_group_type_link
					join y in _db.tickettype on x.ticket_type_id equals y.tickettype_id
					where x.ticket_group_id == groupId
					select new LabelValueInt()
					{
						Label = y.tickettype_name,
						Value = y.tickettype_id
					}).ToArray();
		}
		public LabelValueInt[] GetddlPerts(int groupId)
		{
			return (from x in _db.ticketpage
					where x.ticketpage_ticket_group_id == groupId
					select new LabelValueInt()
					{
						Label = x.ticketpage_name,
						Value = x.ticketpage_id
					}).ToArray();
		}

		public int GetTicketPageId(int groupId, int pageId)
		{
			return (from x in _db.ticketpage
					where x.ticketpage_ticket_group_id == groupId && x.page_id == pageId
					select x.ticketpage_id
				).FirstOrDefault();
		}


		public DataTable GetRelatedTickets(int pageid)
		{
			if (pageid == 0) return null;

			var dt = bllToolbox.doSQL_dt(@"
				SELECT a.ticketheader_id id, 
				1 initial_order, 
				IFNULL(a.ticketheader_createdby_member_id, 0) created_by, 
				IFNULL(a.ticketheader_member_assigned_id, 0) assigned_to, 
				IFNULL(e.ticket_group_administrator, 0) admin_id, 
				IFNULL(a.ticketheader_private, 0) is_private, 
				CAST(IFNULL(GROUP_CONCAT(f.member_id), '') AS CHAR(1000)) watchers, 
				a.ticketheader_issue name, 
				c.ticketstatus_status status, 
				c.ticketstatus_id status_id, 
				a.ticketheader_created_date dt_created, 
				a.ticketheader_issuetype type, 
				'false' is_watching,
				if(a.ticketheader_issuetype = 3 AND a.ticketheader_status_id NOT IN (5,7,10,11,12),total_votes(a.ticketheader_id), -1) v, 
				(SELECT COUNT(*) FROM ticket_vote WHERE ticketheader_id = a.ticketheader_id AND member_id = @v1 ) voted 
				FROM ticketheader a LEFT JOIN ticketpage b on a.ticketheader_module_id = b.ticketpage_id 
				LEFT JOIN ticketstatus c ON a.ticketheader_status_id = c.ticketstatus_id 
				LEFT JOIN ticketpage d ON a.ticketheader_module_id = d.ticketpage_id 
				LEFT JOIN ticket_group e ON d.ticketpage_ticket_group_id = e.ticket_group_id 
				LEFT JOIN ticket_memberview f ON a.ticketheader_id = f.ticket_id 
				WHERE b.page_id = @v0  AND a.ticketheader_private = 0 
				and a.ticketheader_status_id NOT IN (5,7) 
				AND a.ticketheader_issue NOT LIKE '%milestone%' 
				GROUP BY a.ticketheader_id 
				UNION SELECT a.ticketheader_id id, 
				2 initial_order, 
				IFNULL(a.ticketheader_createdby_member_id, 0) created_by, 
				IFNULL(a.ticketheader_member_assigned_id, 0) assigned_to, 
				IFNULL(e.ticket_group_administrator, 0) admin_id, 
				IFNULL(a.ticketheader_private, 0) is_private, 
				CAST(IFNULL(GROUP_CONCAT(f.member_id), '') AS CHAR(1000)) watchers, 
				a.ticketheader_issue name, 
				c.ticketstatus_status status, 
				c.ticketstatus_id status_id, 
				a.ticketheader_created_date dt_created, 
				a.ticketheader_issuetype type, 
				'false' is_watching,
				if(a.ticketheader_issuetype = 3 AND a.ticketheader_status_id NOT IN (5,7,10,11,12),total_votes(a.ticketheader_id), -1) v, 
				(SELECT count(*) FROM ticket_vote WHERE ticketheader_id = a.ticketheader_id AND member_id = @v1 ) voted 
				FROM ticketheader a LEFT JOIN ticketpage b on a.ticketheader_module_id = b.ticketpage_id 
				LEFT JOIN ticketstatus c ON a.ticketheader_status_id = c.ticketstatus_id 
				LEFT JOIN ticketpage d ON a.ticketheader_module_id = d.ticketpage_id 
				LEFT JOIN ticket_group e ON d.ticketpage_ticket_group_id = e.ticket_group_id 
				LEFT JOIN ticket_memberview f ON a.ticketheader_id = f.ticket_id 
				WHERE b.page_id = @v0  
				AND a.ticketheader_status_id IN (5,7) 
				AND a.ticketheader_issue NOT LIKE '%milestone%' 
				GROUP BY a.ticketheader_id", pageid, UserId);

			var temp_dt = dt.Copy();
			temp_dt.Clear();
			foreach (DataRow dr in dt.Rows)
			{
				var is_private = Convert.ToBoolean(dr["is_private"]);
				var do_add = true;
				if (is_private)
				{
					var admin_id = Convert.ToInt32(dr["admin_id"]);
					var assigned_to = Convert.ToInt32(dr["assigned_to"]);
					var created_by = Convert.ToInt32(dr["created_by"]);
					var watcher_list = new List<int>();
					var watchers = dr["watchers"].ToString();
					if (watchers != "")
					{
						watcher_list = watchers.Split(',').Select(int.Parse).ToList();
					}
					dr["is_watching"] = watcher_list.Contains(UserId);
					do_add = Toolbox.Contains(UserId, new int[] { created_by, assigned_to, admin_id }) || watcher_list.Contains(UserId);
				}
				if (do_add)
				{
					temp_dt.ImportRow(dr);
				}
			}
			temp_dt.DefaultView.Sort = "initial_order ASC, dt_created DESC";
			return temp_dt;
		}

		public string GetFileBlob(string name)
		{
			var path = new BLL.Core.FileManager.TicketFile(CurrentUser).GetUserTempPath();
			path = System.IO.Path.Combine(path, name);
			if (!System.IO.File.Exists(path)) return string.Empty;

			using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				var bytes = new byte[file.Length];
				file.Read(bytes, 0, (int)file.Length);
				return System.Convert.ToBase64String(bytes);
			}
		}

		public string Save(DTO.ViewModels.Page.TicketFlyOut.AddTicket model)
		{
			var cExists = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(ticketheader_id),0) FROM ticketheader WHERE ticketheader_issue = @v0", model.Subject);
			if (cExists > 0)
			{
				return ($@"This same subject exists on ticket #{cExists}</a>. 
				You may want to add yourself as a viewer to that ticket, or if it is closed, have the ticket group administrator re-open it."
				);
			}
			var current_user = new NeMember(UserId);
			var t = new NETickets
			{
				issue = model.Subject,
				createdby_member_id = CurrentUser.Id,
				issuetype = model.Type,
				module_id = model.Pert,
				is_private = model.Is_private,
                priority_id = 1
            };
			t.save();
			if (t.id > 0)
			{
				var ti = new NETickets.ticketissue
				{
					ticketheader_id = t.id,
					message = model.body == "" ? "(Message body not provided)" : model.body,
					created_member_id = CurrentUser.Id
				};
				ti.save();
				var fileSaved = false;


				if (!string.IsNullOrEmpty(model.File) && model.Has_file)
				{
					var image = Convert.FromBase64String(GetFileBlob(model.File));
					NETickets.ticketissue.add_attachment(ti.id, image, image.Length, ti.id.ToString(), "png");
					fileSaved = true;
				}
				if (fileSaved)
				{
					ti.uploaded_file = ti.id + ".png";
					ti.save();
				}
				NETickets.send_new_ticket_alert(t.id, current_user, t.issue, ti.message);
			}
			return "Ticket has been saved successfully.";
		}
	}
}