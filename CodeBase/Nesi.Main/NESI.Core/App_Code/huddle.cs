using System;
using System.Linq;
using DevExpress.Xpo;

namespace nesi.core
	{
	public class huddle
		{
		public int id { get; set; }
		public string name  { get; set; }
		public string description  { get; set; }
		public int created_by  { get; set; }
		public int captain { get; set; }
		public bool active  { get; set; }
		public DateTime dt_created  { get; set; }
		public DateTime date_of_huddle { get; set; }

		public huddle(){}
		public huddle(int _id)
			{
			if(exists(_id))
				{
				load(_id);
				}
			}
		private void load(int _id)
			{
			using(var uow = new UnitOfWork())
				{
				var t				= uow.GetObjectByKey<ne_xpo.cs.huddle>(_id);
				id					= _id;
				name				= t.name;
				dt_created			= t.dt_created;
				created_by			= t.created_by.member_id == null ? 0 : t.created_by.member_id;
				captain				= t.captain == null ? 0 : t.captain.member_id;
				active				= t.active;
				description			= t.description;
				date_of_huddle = t.date_of_huddle;
				}

			}

		private bool exists(int _id)
			{
			using(var uow = new UnitOfWork())
				{
				var t			= uow.GetObjectByKey<ne_xpo.cs.huddle>(_id);
				return t != null;
				}		
			}
		public void save()
			{
			using(var uow = new UnitOfWork())
				{
				var t			= id == 0 
					? new ne_xpo.cs.huddle(uow)
					: uow.GetObjectByKey<ne_xpo.cs.huddle>(id);
				t.name						= name;
				t.dt_created				= id == 0 ? DateTime.Now : dt_created;
				t.created_by				= uow.GetObjectByKey<ne_xpo.cs.member>(created_by);
				t.description				= description;
				t.captain					= uow.GetObjectByKey<ne_xpo.cs.member>(captain);
				t.active					= active;
				t.date_of_huddle = date_of_huddle;
				t.Save();
				uow.CommitChanges();
				id							= t.id;
				}
			}

		public class user
			{
			public int id  { get; set; }
			public int huddle  { get; set; }
			public int member  { get; set; }
			public string color  { get; set; }

			public user(){}
			public user(int _id)
				{
				if(exists(_id))
					{
					load(_id);
					}
				}
			public user(int _huddle_id, int _member_id)
				{
				if(exists(_huddle_id, _member_id))
					{
					load(_huddle_id, _member_id);
					}
				else
					{
					huddle		= _huddle_id;
					member		= _member_id;
					}
				}
		
			private void load(int _huddle_id, int _member_id)
				{
				using(var uow = new UnitOfWork())
					{
					var t		=	from x in new XPQuery<ne_xpo.cs.huddle_user>(uow)
						where 
						x.huddle.id == _huddle_id && 
						x.member.member_id == _member_id
						select x;
					var i				= t.FirstOrDefault();
					if(i != null)
						{
						id				= i.id;
						huddle			= i.huddle.id;
						member			= i.member.member_id;
						color			= i.color;
						}
					}
				}
			private void load(int _id)
				{
				using(var uow = new UnitOfWork())
					{
					var t			= uow.GetObjectByKey<ne_xpo.cs.huddle_user>(_id);
					huddle			= t.huddle.id;
					member			= t.member.member_id;
					color			= t.color;
					id				= _id;
					}
				}
			public bool exists(int _huddle_id, int _member_id)
				{
				using(var uow = new UnitOfWork())
					{
					return (from x in new XPQuery<ne_xpo.cs.huddle_user>(uow)
								where 
								x.huddle == uow.GetObjectByKey<ne_xpo.cs.huddle>(_huddle_id) && 
								x.member == uow.GetObjectByKey<ne_xpo.cs.member>(_member_id)
								select x).Count() > 0;
					}
				}
			public bool exists(int _id)
				{
				using(var uow = new UnitOfWork())
					{
					var t			= uow.GetObjectByKey<ne_xpo.cs.huddle_user>(_id);
					return t != null;
					}		
				}
			public void save()
				{
				using(var uow = new UnitOfWork())
					{
					var t	= id == 0 
						? new ne_xpo.cs.huddle_user(uow)
						: uow.GetObjectByKey<ne_xpo.cs.huddle_user>(id);
					t.huddle							= uow.GetObjectByKey<ne_xpo.cs.huddle>(huddle);
					t.member							= uow.GetObjectByKey<ne_xpo.cs.member>(member);
					t.color								= color;
					t.Save();
					uow.CommitChanges();
					id									= t.id;
					}
				}
			public void delete()
				{
				using(var uow = new UnitOfWork())
					{
					var t			= uow.GetObjectByKey<ne_xpo.cs.huddle_user>(id);
					t.Delete();
					uow.CommitChanges();
					}
				}
			}
		public class task
			{
			public int id              { get; set; }
			public int huddle          { get; set; }
			public int order_n         { get; set; }
			public int created_by     { get; set; }
			public int huddle_source   { get; set; }
			public int source_id       { get; set; }
			public string linetext     { get; set; }
			public string link         { get; set; }
			public string description  { get; set; }
			public int status          { get; set; }
			public int assigned_to     { get; set; }
			public string to_be_done     { get; set; }
			public DateTime date_due   { get; set; }
		
			//public static List<string> statuses { get { return new List<string>(){"Not Started", "In Progress", "Completed","Pushed"}; } }
			public task(){}
			public task(int _id)
				{
				if(exists(_id))
					{
					load(_id);
					}
				}
			public bool exists(int _id)
				{
				using(var uow = new UnitOfWork())
					{
					var t			= uow.GetObjectByKey<ne_xpo.cs.huddle_task>(_id);
					return t != null;
					}	
				}
			private void load(int _id)
				{
				using(var uow = new UnitOfWork())
					{
					var t			= uow.GetObjectByKey<ne_xpo.cs.huddle_task>(_id);
					id				= t.id;
					huddle			= t.huddle.id;
					order_n			= t.order_n;
					created_by		= t.created_by.member_id;
					huddle_source	= t.huddle_source.id;
					source_id		= t.source_id;
					link			= t.link;
					description		= t.description;
					status			= t.status;
					assigned_to		= t.assigned_to.id;
					date_due		= t.date_due;
					linetext		= t.linetext;
					to_be_done = t.to_be_done;
					}

				}
			public void save()
				{
				using(var uow = new UnitOfWork())
					{
					var t		= id == 0 
						? new ne_xpo.cs.huddle_task(uow)
						: uow.GetObjectByKey<ne_xpo.cs.huddle_task>(id);
					t.huddle					= uow.GetObjectByKey<ne_xpo.cs.huddle>(huddle);
					t.assigned_to				= uow.GetObjectByKey<ne_xpo.cs.huddle_user>(assigned_to);
					t.created_by				= uow.GetObjectByKey<ne_xpo.cs.member>(created_by);
					t.huddle_source				= uow.GetObjectByKey<ne_xpo.cs.huddle_source>(huddle_source);
					t.order_n					= order_n;
					t.source_id					= source_id;
					t.link						= link;
					t.description				= description;
					t.status					= status;
					t.linetext					= linetext;
					t.to_be_done = to_be_done;
					t.date_due					= date_due;
					t.Save();
					uow.CommitChanges();
					id							= t.id;
					}
				}
			public void delete()
				{
				using(var uow = new UnitOfWork())
					{
					var t			= uow.GetObjectByKey<ne_xpo.cs.huddle_task>(id);
					t.Delete();
					uow.CommitChanges();
					}
				}
			public class comment
				{
				public int id					{ get; set; }
				public int huddle_task          { get; set; }
				public int created_by			{ get; set; }
				public DateTime created_dt		{ get; set; }
				public string text				{ get; set; } // Had to name this text because the name of the column is comment, and so is the name of this class.
		
				public comment(){}
				public comment(int _id)
					{
					if(exists(_id))
						{
						load(_id);
						}
					}
				public bool exists(int _id)
					{
					using(var uow = new UnitOfWork())
						{
						var htc			= uow.GetObjectByKey<ne_xpo.cs.huddle_task_comments>(_id);
						return htc != null;
						}	
					}
				private void load(int _id)
					{
					using(var uow = new UnitOfWork())
						{
						var htc			= uow.GetObjectByKey<ne_xpo.cs.huddle_task_comments>(_id);
						id				= htc.id;
						huddle_task		= htc.huddle_task.id;
						created_by		= htc.created_by.member_id;
						created_dt		= htc.created_dt;
						text			= htc.comment;
						}

					}
				public void save()
					{
					using(var uow = new UnitOfWork())
						{
						var htc		= id == 0 
							? new ne_xpo.cs.huddle_task_comments(uow)
							: uow.GetObjectByKey<ne_xpo.cs.huddle_task_comments>(id);
						htc.huddle_task							= uow.GetObjectByKey<ne_xpo.cs.huddle_task>(huddle_task);
						htc.created_by							= uow.GetObjectByKey<ne_xpo.cs.member>(created_by);
						htc.created_dt							= created_dt == null ? DateTime.Now : created_dt;
						htc.comment								= text;
						htc.Save();
						uow.CommitChanges();
						id							= htc.id;
						}
					}
				}
			}
		}
	}