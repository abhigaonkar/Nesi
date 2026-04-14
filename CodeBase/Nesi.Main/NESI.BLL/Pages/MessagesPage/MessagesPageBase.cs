using System;
using System.Data;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading;
using System.Web.UI.WebControls;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.MessagesPage
{
	public class MessagesPageBase : BLLBase
	{

		public MessagesPageBase(Employee user) : base(user)
		{

		}

		public LabelValueInt[] GetUserList()
		{
			return bllToolbox.doSQL_List<LabelValueInt>(@"
	SELECT
			c.member_id value,
			CONCAT(c.member_fullname, ' - ', f.name) label
				FROM
			page a
			INNER JOIN
			memberpageprivilege b
			INNER JOIN
			member c ON b.memberpageprivilege_member_id = c.Member_ID
			INNER JOIN
			memberpage d ON d.memberpage_member_id = c.member_id AND d.memberpage_page_id = a.page_id
			INNER JOIN
			privilege e ON b.memberpageprivilege_privilege_id = e.privilege_id AND e.privilege_page_id = a.page_id
			INNER join
			business_unit f ON c.business_unit_id = f.id and find_in_set(f.id,@p0)
			WHERE
			c.member_status = 'Active' AND
			a.page_id = '29' AND
			e.privilege_id = '24'
			ORDER BY
			f.id ASC,
				c.member_nickname ASC,
				c.member_lastname ASC
			", CurrentUser.VisibleBusinessUnits).ToArray();
		}


		public object ReadMessage(int type, int messageTo_id)
		{
			var getMessageDetails = new NeMessaging(messageTo_id);
			if (type == 1)
			{
				var changeStatus = new NeMessaging();
				changeStatus.UpdateStatus(messageTo_id);
			}
			var path = new BLL.Core.FileManager.MessageAttachFile(new Employee(getMessageDetails.Message_LeftBy_Member_ID)).BasePath;
			var files = new System.IO.DirectoryInfo(path).GetFiles(getMessageDetails.Message_ID + ".*");
			var file = "";
			if (files.Length > 0)
			{
				file = files[0].FullName;
			}

			var ids = bllToolbox
				.doSQL_List<int>(
					@"select messageto_member_id_to from messageto where messageto_message_id=@p0 and messageto_member_id_to != @p1",
					getMessageDetails.Message_ID, UserId);
			ids.Add(getMessageDetails.Message_LeftBy_Member_ID);
			return new
			{
				body = getMessageDetails.Message_Body,
				ids = ids.ToArray(),
				fullName = file,
				extension = System.IO.Path.GetExtension(file)
			};
		}

		public DataTable GetDeleted()
		{
			return bllToolbox.doSQL_dt(@"
(SELECT
  MTo.MessageTo_ID,
  MTo.MessageTo_Message_ID,
  MTo.MessageTo_Member_ID_To,
  M.Message_ID,
  member_name (m.Message_LeftBy_Member_ID) from_name,
  member_name(Mto.MessageTo_Member_ID_To) to_name,
  M.MessageType_ID,
  M.Message_LeftBy_Member_ID,
  DATE_FORMAT(M.Date, '%Y-%m-%d %H:%i:%s') AS DATE,
  M.Message_Subject AS SUBJECT,
  MType.MessageType AS MessType,
  MTo.Message_Status AS STATUS,
  CONCAT(
    Mem.Member_FirstName,
    ' ',
    Mem.Member_LastName
  ) AS ToFrom,
  3 TYPE
FROM
  messageto MTo
  INNER JOIN Message M
    ON M.Message_ID = MTo.MessageTo_Message_ID
  INNER JOIN Member Mem
    ON Mem.Member_ID = M.Message_LeftBy_Member_ID
  INNER JOIN MessageType MType
    ON MType.MessageType_ID = M.MessageType_ID
WHERE M.MessageType_ID = 2
  AND MTo.MessageTo_Member_ID_To = @p0
  AND MTo.Message_Status = 'Deleted'
ORDER BY MessageTo_ID DESC)
UNION
(SELECT
  MTo.MessageTo_ID,
  MTo.MessageTo_Message_ID,
  MTo.MessageTo_Member_ID_To,
  M.Message_ID,
  member_name(M.Message_LeftBy_Member_ID) from_name, 
  member_name (mem.member_id) to_name,
  M.MessageType_ID,
  M.Message_LeftBy_Member_ID,
  M.Date AS DATE,
  M.Message_Subject AS SUBJECT,
  MType.MessageType AS MessType,
  MTo.Message_Status AS STATUS,
  CONCAT(
    Mem.Member_FirstName,
    ' ',
    Mem.Member_LastName
  ) AS ToFrom,
  3 TYPE
FROM
  messageto MTo
  INNER JOIN Message M
    ON M.Message_ID = MTo.MessageTo_Message_ID
  INNER JOIN Member Mem
    ON Mem.Member_ID = MTo.MessageTo_Member_ID_To
  INNER JOIN MessageType MType
    ON MType.MessageType_ID = M.MessageType_ID
WHERE M.MessageType_ID = 2
  AND M.Message_LeftBy_Member_ID = @p0
  AND MTo.message_sent_deleted = 1
ORDER BY MessageTo_ID DESC)
",
				UserId);
		}

		public DataTable GetInboxMessages(bool isDelete)
		{
			return bllToolbox.doSQL_dt(@"SELECT 
					MTo.MessageTo_ID, 
					MTo.MessageTo_Message_ID, 
					MTo.MessageTo_Member_ID_To, 
  member_name (m.Message_LeftBy_Member_ID) from_name,
  member_name(Mto.MessageTo_Member_ID_To) to_name,
					M.Message_ID, 
					M.MessageType_ID, 
					M.Message_LeftBy_Member_ID, 
					 DATE_FORMAT(M.Date, '%Y-%m-%d %H:%i:%s') AS DATE, 
					M.Message_Subject as Subject, 
					MType.MessageType as MessType , 
					MTo.Message_Status as Status, 
					Concat(Mem.Member_FirstName,' ',Mem.Member_LastName) as ToFrom,
				    1 type
					FROM messageto MTo 
					Inner join Message M 
						on M.Message_ID = MTo.MessageTo_Message_ID 
					Inner Join Member Mem 
						on Mem.Member_ID = M.Message_LeftBy_Member_ID 
					Inner Join MessageType MType 
						on MType.MessageType_ID =  M.MessageType_ID
					WHERE M.MessageType_ID = 2 
						AND MTo.MessageTo_Member_ID_To = @v0 "
						+ (isDelete ? @"AND MTo.Message_Status = 'Deleted'" : @"AND MTo.Message_Status != 'Deleted'") +
						@"ORDER BY MessageTo_ID DESC",
						UserId);
		}

		public DataTable GetOutboxMessages(bool isDelete)
		{
			return bllToolbox.doSQL_dt(@"SELECT 
					MTo.MessageTo_ID, 
					MTo.MessageTo_Message_ID, 
					MTo.MessageTo_Member_ID_To,
					M.Message_ID, 
  member_name (m.Message_LeftBy_Member_ID) from_name,
  member_name(Mto.MessageTo_Member_ID_To) to_name,
					M.MessageType_ID, 
					M.Message_LeftBy_Member_ID,
					 DATE_FORMAT(M.Date, '%Y-%m-%d %H:%i:%s') AS DATE,
					M.Message_Subject as Subject,
					MType.MessageType as MessType ,
					MTo.Message_Status as Status, 
					Concat(Mem.Member_FirstName,' ',Mem.Member_LastName) as ToFrom,
				    2 type
					FROM messageto MTo 
					Inner join Message M 
						on M.Message_ID = MTo.MessageTo_Message_ID 
					Inner Join Member Mem 
						on Mem.Member_ID = MTo.MessageTo_Member_ID_To  
					Inner Join MessageType MType 
						on MType.MessageType_ID =  M.MessageType_ID
					WHERE M.MessageType_ID = 2 
						AND M.Message_LeftBy_Member_ID = @p0 "
					 + (isDelete ? @" AND MTo.message_sent_deleted = 1" : @" AND MTo.message_sent_deleted = 0 ") +
					@" ORDER BY MessageTo_ID DESC",
					UserId);
		}

		public string SendMessage(DTO.ViewModels.Page.MessagesPage.SendMessage model)
		{
			var newMessage = new NeMessaging()
			{
				Message_LeftBy_Member_ID = UserId,
				MessageType_ID = 2,
				Message_Subject = model.subject,
				Message_Body = model.body,
				Date = DateTime.Now
			};
			newMessage.AddNewMessage();
			var newId = newMessage.Message_ID;
			var path = new BLL.Core.FileManager.MessageAttachFile(CurrentUser).BasePath;
			var filename = System.IO.Path.Combine(path, CurrentUser.Guid, model.file_name);
			if (!string.IsNullOrEmpty(model.file_name))
			{
				if (System.IO.File.Exists(filename))
				{
					var newfile = System.IO.Path.Combine(path, newId + System.IO.Path.GetExtension(filename));
					if (System.IO.File.Exists(newfile))
					{
						System.IO.File.Delete(newfile);
					}
					var files = new System.IO.DirectoryInfo(path).GetFiles(newId + ".*");
					if (files.Length > 0)
					{
						foreach (var file in files)
						{
							System.IO.File.Delete(file.FullName);
						}
					}

					System.IO.File.Copy(filename, newfile);
					//try
					//{
					//	System.IO.File.Delete(System.IO.Path.Combine(path, CurrentUser.Guid));
					//}
					//catch (Exception e)
					//{
					//	Console.WriteLine(e);
					//}
				}
			}
			foreach (var memberId in model.to)
			{
				var newMessageTo = new NeMessaging()
				{
					MessageTo_Message_ID = newId,
					MessageTo_Member_ID_To = memberId,
				};
				newMessageTo.CreateMessageTo();
				// sendEmail(newMessage, newMessageTo, newfile);
				ThreadPool.QueueUserWorkItem(delegate { sendEmail(newMessage, newMessageTo, string.IsNullOrEmpty(model.file_name) ? "" : filename); });

			}

			return $"The message was sent successfully to {model.to.Length} person.";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messageIds"></param>
		/// <param name="type">1 inbox, 2 sent box</param>
		/// <returns></returns>
		public string deleteMessages(int[] messageIds, int type)
		{

			foreach (var id in messageIds)
			{
				if (type == 1)
				{
					bllToolbox.doSQL_void("update messageTo set message_status='Deleted' where messageto_id=@p0", id);
				}
				else
				{
					bllToolbox.doSQL_void("update messageTo set message_sent_deleted=1 where messageto_id=@p0", id);
				}
			}
			return messageIds.Length > 1 ? $"{messageIds.Length} message were deleted successfully." : $"The message was deleted successfully.";

		}

		public void sendEmail(NeMessaging message, NeMessaging messageTo, string file)
		{
			var mail = new NeEMail();
			var toMember = new NeMember(messageTo.MessageTo_Member_ID_To);
			var fromMember = new NeMember(message.Message_LeftBy_Member_ID);
			var to_mail = !string.IsNullOrEmpty(toMember.NEEmail) ? toMember.NEEmail : toMember.Email;
			mail.From = !string.IsNullOrEmpty(fromMember.NEEmail) ? fromMember.NEEmail : fromMember.Email;
			mail.To = to_mail;
			mail.Subject = message.Message_Subject;
			mail.isHTML = true;
		    var host = Toolbox.app_setting("Domain"); // the format like: https://ops.spark......
            mail.Body = $@"
<b>You have a new message from SparkOps.</b><br/><br/>
------------------------------------------------------------<br/>
<div class='margin: 20px;border: 1px solid black'>
From: {fromMember.FullName}<br/>
Date: {message.Date}<br/>
Message: <br/><br/>
------------------------------------------------------------<br/>
{message.Message_Body.Replace("\n", "<br/>")}
</div>
------------------------------------------------------------<br/><br/>

<a href='{host}'>Please login SparkOps to view and reply the message</a>
";
			if (!string.IsNullOrEmpty(file))
			{
				try
				{
					mail.Attachment = new Attachment(file);
				}
				catch (Exception)
				{
					//
				}
			}
			mail.Send();
		}
	}
}