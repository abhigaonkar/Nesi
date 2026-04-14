using System;
using System.Collections.Concurrent;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;
using System.Net.WebSockets;
using MySql.Data.MySqlClient;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeMessaging
	/// </summary>
	public class NeMessaging
		{
		public int Message_ID { get; set; }
		public int MessageType_ID { get; set; }
		public int Message_LeftBy_Member_ID { get; set; }
		public DateTime Date { get; set; }
		public string Message_Subject { get; set; }
		public string Message_Body { get; set; }
		public string Message_Status { get; set; }
		public int MessageTo_ID { get; set; }
		public int MessageTo_Message_ID { get; set; }
		public int MessageTo_Member_ID_To { get; set; }
		public string MessageType { get; set; }
		public int Task_ID { get; set; }
		public int Task_Message_ID { get; set; }
		public int Member_ID_Create { get; set; }
		public string Created_Date { get; set; }
		public int Member_ID_Audit { get; set; }
		public string Modified_Date { get; set; }
		public int Member_Task_CC_ID { get; set; }
		public int Member_CC_ID { get; set; }

		public NeMessaging(){}
		public NeMessaging(int _id)
			{
			var dt = Toolbox.doSQL_dt(@" SELECT a.message_id, a.message_body, b.messageto_id, b.messageto_message_id, a.message_leftby_member_id, a.messagetype_id, a.date FROM message a INNER JOIN messageto b on b.messageto_message_id = a.message_id WHERE b.messageto_id = @v0 ", new object[] {  _id } );
			if (dt.Rows.Count == 1)
				{
				init_message_details(dt.Rows[0]);
				}
			}
		private void init_message_details(DataRow _dr)
			{
			Message_ID = Convert.ToInt32(_dr["message_id"]);
			Message_Body = _dr["message_body"].ToString();
			Message_LeftBy_Member_ID = Convert.ToInt32(_dr["message_leftby_member_id"]);
			MessageType_ID = Convert.ToInt32(_dr["messagetype_id"]);
			Date = Convert.ToDateTime(_dr["date"]);
			}
		public double TotalNewmessages(MySqlConnection _conn, int _m_type, int _mem_id)
			{
			return Toolbox.doSQL_double(_conn, @"SELECT COUNT(*) FROM messageto a INNER JOIN message b ON b.message_id = a.messageto_message_id WHERE b.messagetype_id = @v0  AND a.message_status = 'Unread' AND a.messageto_member_id_to = @v1 ", new object[] {  _m_type, _mem_id } );
			}

		public void AddNewMessage()
			{
			Message_ID = Toolbox.doSQL_return_id(@"INSERT INTO Message 
(messagetype_id , date, message_leftby_member_id, message_subject, message_body) 
VALUES (@v0,NOW(),@v1,@v2,@v3)",
new object[] {
MessageType_ID, Message_LeftBy_Member_ID, Message_Subject, Message_Body});
			}
		public void CreateMessageTo()
			{
			Toolbox.doSQL_void(@"INSERT Into MessageTo 
(MessageTo_Message_ID, MessageTo_Member_ID_To)
Values (@v0,@v1)",new object[] { MessageTo_Message_ID, MessageTo_Member_ID_To});
			}
		public void UpdateStatus(int _messid)
			{
			Toolbox.doSQL_void(@"UPDATE MessageTo SET Message_Status = 'Read' Where MessageTo_ID = @v0", _messid);
			}
		public void DeleteMessage(int _messagetoid, int _deltype, int _member_id)
			{
			var str_sel_mto = _deltype == 1 
				? "UPDATE MessageTo SET Message_Status = 'Deleted' Where MessageTo_Message_ID = " + _messagetoid + " AND MessageTo_Member_ID_to = " + _member_id
				: _deltype == 2
					?
					"UPDATE Message  SET  Message_Status = 'Deleted' Where Message_ID = " + _messagetoid
					: "";
			if(str_sel_mto != "")
				{
				Toolbox.doSQL_void(str_sel_mto);
				}
			}
		}
	public class NEVideo
		{
		public int id				{ get; set; }
		public int category_id		{ get; set; }
		public int file_id			{ get; set; }
		public string name			{ get; set; }
		public string description	{ get; set; }

		public NEVideo() {}
		public NEVideo(int _id) 
			{
			init(_id);
			}
		private void init(int _id)
			{
			if(exists(_id))
				{
				var _dt			= Toolbox.doSQL_dt(@"SELECT * FROM video WHERE id = @v0  LIMIT 1", new object[] {  _id } );
				foreach(DataRow _dr in _dt.Rows)
					{
					id					= _id;
					category_id			= (int) _dr["category_id"];
					file_id				= (int) _dr["file_id"];
					name				= (string) _dr["name"];
					description			= (string) _dr["description"];
					}
				}
			}
		public void delete()
			{
			if(id == 0)
				{
				throw new Exception("Video not loaded");
				}
			else
				{
				Toolbox.doSQL_void(@"DELETE FROM filestore.files WHERE id = @v0 LIMIT 1", file_id);
				Toolbox.doSQL_void(@"DELETE FROM video WHERE id = @v0 LIMIT 1", id);
				id			= 0;
				}
			}
		public void save()
			{
			if(id == 0)
				{
				if(exists(name, category_id))
					{
					throw new Exception("A video with this name already exists");
					}
				// INSERT
				Toolbox.doSQL_void(@"
INSERT INTO video 
	(
	category_id,
	name,
	description,
	file_id
	)
VALUES
	(
	@v0,
	@v1,
	@v2,
	@v3
	)", new object[] {

					category_id,							// {0}
					name,				// {1}
					description,		// {2}
					file_id									// {3}
				});
				}
			else
				{
				// UPDATE
				Toolbox.doSQL_void(@"
UPDATE 
	video 
SET
	category_id		= @v0,
	name			= @v1,
	description		= @v2
WHERE
	id = @v3
LIMIT 1", new object[] {

					category_id,						// {0}
					name,			// {1}
					description,	// {2}	
					id									// {3}
				});
				}
			}
		public bool exists(int _id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video WHERE id = @v0", _id) > 0;
			}
		public bool exists(string _name)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video WHERE name = @v0", _name) > 0;
			}
		public bool exists(string _name, int _category_id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video WHERE name = @v0 AND category_id = @v1", new object[] {
				_name, _category_id}) > 0;
			}
		public class Category
			{
			public int id				{ get; set; }
			public string name			{ get; set; }
			public Category(){}
			public Category(int _id)
				{
				if(exists(_id))
					{
					init(_id);
					}
				else
					{
					throw new Exception("Category doesn't exist");
					}
				}
			private void init(int _id)
				{
				if(exists(_id))
					{
					id						= _id;
					var _dt			= Toolbox.doSQL_dt(@"SELECT * FROM video_category WHERE id = @v0  LIMIT 1", new object[] {  _id } );
					foreach(DataRow _dr in _dt.Rows)
						{
						name				= (string) _dr["name"];
						}
					}
				}
			public bool is_linked(int _id)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video WHERE category_id = @v0", _id) > 0;
				}
			public bool exists(int _id)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video_category WHERE id = @v0", _id) > 0;
				}
			public bool exists(string _name)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video_category WHERE name = @v0 ", _name) > 0;
				}
			public bool exists(string _name, int _id)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video_category WHERE name = @v0 AND id != @v1 ", new object[] {
					_name, _id}) > 0;
				}
			public void delete()
				{
				if(id == 0)
					{
					throw new Exception("Category not loaded");
					}
				else if(is_linked(id))
					{
					throw new Exception("This category is linked to existing videos");
					}
				else
					{
					Toolbox.doSQL_void(@"DELETE FROM video_category WHERE id = @v0 LIMIT 1", id);
					id			= 0;
					}
				}
			public void save()
				{
				if(id == 0)
					{
					if(exists(name))
						{
						throw new Exception("This name already exists");
						}
					// INSERT
					Toolbox.doSQL_void(@"
INSERT INTO video_category 
	(
	name
	)
VALUES
	(
	@v0
	)", name);
					}
				else
					{
					if(exists(name, id))
						{
						throw new Exception("This name already exists on a different category");
						}
					// UPDATE
					Toolbox.doSQL_void(@"
UPDATE 
	video_category
SET
	name = @v0
WHERE
	id = @v1
LIMIT 1", new object[] {
						name, id});
					}
				}
			}
		public class Privilege
			{
			public int id				{ get; set; }
			public int category_id		{ get; set; }
			public int membertype_id	{ get; set; }
			public Privilege(){}
			public Privilege(int _id)
				{
				if(exists(_id))
					{
					init(_id);
					}
				else
					{
					throw new Exception("Privilege doesn't exist");
					}
				}
			public Privilege(int _membertype_id, int _category_id)
				{
				if(exists(_membertype_id, _category_id))
					{
					init(_membertype_id, _category_id);
					}
				else
					{
					throw new Exception("Privilege doesn't exist");
					}
				}
			private void init(int _membertype_id, int _category_id)
				{
				var _id				= Toolbox.doSQL_int(@"SELECT id FROM video_privilege WHERE membertype_id = @v0 AND category_id = @v1", new object[] {
					_membertype_id, _category_id});
				init(_id);
				}
			private void init(int _id)
				{
				if(exists(_id))
					{
					id						= _id;
					var _dt			= Toolbox.doSQL_dt(@"SELECT * FROM video_privilege WHERE id = @v0  LIMIT 1", new object[] {  _id } );
					foreach(DataRow _dr in _dt.Rows)
						{
						category_id			= (int) _dr["category_id"];
						membertype_id		= (int) _dr["membertype_id"];
						}
					}
				}
			public bool exists(int _id)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video_privilege WHERE id = @v0", _id) > 0;
				}
			public bool exists(int _membertype_id, int _category_id)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video_privilege WHERE membertype_id = @v0 AND category_id = @v1", new object[] {
					_membertype_id, _category_id}) > 0;
				}
			public void delete()
				{
				if(id == 0)
					{
					throw new Exception("Privilege not loaded");
					}
				else
					{
					Toolbox.doSQL_void(@"DELETE FROM video_privilege WHERE id = @v0 LIMIT 1", id);
					id			= 0;
					}
				}
			public static bool authenticated_for(int _membertype_id, int _video_id)
				{
				var v				= new NEVideo(_video_id);
				var c					= Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video_privilege WHERE category_id = @v0 AND membertype_id = @v1", new object[] {
					v.category_id, _membertype_id});
				return c > 0;
				}
			public void save()
				{
				if(id == 0)
					{
					// INSERT
					Toolbox.doSQL_void(@"
INSERT INTO video_privilege 
	(
	category_id,
	membertype_id
	)
VALUES
	(
	@v0,
	@v1
	)", new object[] {
						category_id, membertype_id});
					}
				else
					{
					// UPDATE
					Toolbox.doSQL_void(@"
UPDATE 
	video_privilege
SET
	category_id		= @v0,
	membertype_id	= @v1
WHERE
	id = @v2
LIMIT 1", new object[] {

						category_id,
						membertype_id, 
						id});
					}
				}
			}
		public class PageLink
			{
			public int id				{ get; set; }
			public int category_id		{ get; set; }
			public int page_id	{ get; set; }
			public PageLink(){}
			public PageLink(int _id)
				{
				if(exists(_id))
					{
					init(_id);
					}
				else
					{
					throw new Exception("Page Link doesn't exist");
					}
				}
			public PageLink(int _page_id, int _category_id)
				{
				if(exists(_page_id, _category_id))
					{
					init(_page_id, _category_id);
					}
				else
					{
					throw new Exception("Page Link doesn't exist");
					}
				}
			private void init(int _page_id, int _category_id)
				{
				var _id				= Toolbox.doSQL_int(@"SELECT id FROM video_page_lnk WHERE page_id = @v0 AND category_id = @v1", new object[] {
					_page_id, _category_id});
				init(_id);
				}
			private void init(int _id)
				{
				if(exists(_id))
					{
					id						= _id;
					var _dt			= Toolbox.doSQL_dt(@"SELECT * FROM video_page_lnk WHERE id = @v0  LIMIT 1", new object[] {  _id } );
					foreach(DataRow _dr in _dt.Rows)
						{
						category_id			= (int) _dr["category_id"];
						page_id				= (int) _dr["page_id"];
						}
					}
				}
			public bool exists(int _id)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video_page_lnk WHERE id = @v0", _id) > 0;
				}
			public bool exists(int _page_id, int _category_id)
				{
				return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM video_page_lnk WHERE page_id = @v0 AND category_id = @v1", new object[] {
					_page_id, _category_id}) > 0;
				}
			public void delete()
				{
				if(id == 0)
					{
					throw new Exception("Page Link not loaded");
					}
				else
					{
					Toolbox.doSQL_void(@"DELETE FROM video_page_lnk WHERE id = @v0 LIMIT 1", id);
					id			= 0;
					}
				}
			public void save()
				{
				if(id == 0)
					{
					// INSERT
					Toolbox.doSQL_void(@"
INSERT INTO video_page_lnk 
	(
	category_id,
	page_id
	)
VALUES
	(
	@v0,
	@v1
	)", new object[] {
						category_id, page_id});
					}
				else
					{
					// UPDATE
					Toolbox.doSQL_void(@"
UPDATE 
	video_page_lnk
SET
	category_id		= @v0,
	page_id	= @v1
WHERE
	id = @v2
LIMIT 1", new object[] {

						category_id,
						page_id, 
						id});
					}
				}
			}

		}
	public static class notification
		{
		public static void remove_user(int _id, HttpApplicationStateBase _application)
			{
			var users			= (ConcurrentDictionary<int, WebSocket>) _application["socket_users"];
			if(users != null)
				{
				WebSocket ows;
				users.TryRemove(_id, out ows);
				}
			}
		public static bool user_exists(int _id, HttpApplicationState _application)
			{
			return user_exists(_id, new HttpApplicationStateWrapper(_application));
			}
		public static bool user_exists(int _id, HttpApplicationStateBase _application)
			{
			var users			= (ConcurrentDictionary<int, WebSocket>) _application["socket_users"];
			return users != null 
				? users.ContainsKey(_id) 
				: false;
			}
		public static void remove_user(int _id, HttpApplicationState _application)
			{
			remove_user(_id, new HttpApplicationStateWrapper(_application));
			}
		public static void broadcast(string _message, HttpApplicationState _application, NeMember _from, NeMember _to)
			{
			broadcast(_message, new HttpApplicationStateWrapper(_application), _from, _to);
			}
		public static void broadcast(string _message, HttpApplicationStateBase _application, NeMember _from, NeMember _to)
			{
			var users			= (ConcurrentDictionary<int, WebSocket>) _application["socket_users"];
			if(users != null)
				{
				var user_message	= _message;
				var sendbuffer		= new ArraySegment<byte>(Encoding.UTF8.GetBytes(user_message));
				foreach(var u in users)
					{
					if (u.Key == _to.id || _to.id == 0 && _from.id != u.Key)
						{
						var this_socket		= u.Value;
						try
							{
							if(this_socket.State == WebSocketState.Open)
								{
								this_socket.SendAsync(sendbuffer, WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(true);
								}
							}
						catch (ObjectDisposedException ode)
							{
							remove_user(u.Key, _application);
							}
						}
					}
				}
			}
		public static void broadcast(string _message, HttpApplicationStateBase _application, NeMember _from)
			{
			broadcast(_message, _application, _from, new NeMember());
			}
		public static void broadcast(string _message, HttpApplicationState _application)
			{
			broadcast(_message, new HttpApplicationStateWrapper(_application), new NeMember());
			}
		public static void broadcast(string _message, HttpApplicationStateBase _application)
			{
			broadcast(_message, _application, new NeMember());
			}
		}
	}