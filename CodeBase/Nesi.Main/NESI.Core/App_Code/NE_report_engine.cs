using System;
using System.Linq;
using DevExpress.Xpo;
namespace nesi.core
	{
	/// <summary>
	/// Summary description for NE_report_engine
	/// </summary>
	public class report_engine
	{
	public report_engine(){}
	public class report
		{
		public int id { get; set; }
		public string name  { get; set; }
		public int r_engine_category  { get; set; }
		public string query  { get; set; }
		public DateTime created_when  { get; set; }
		public string parameters  { get; set; }

		public report(){}
		public report(int _id)
			{
			using(var uow = new UnitOfWork())
				{
				if(exists(_id, uow))
					{
					load(_id, uow);
					}
				}
			}
		private void load(int _id, Session _uow)
			{
			var r				= _uow.GetObjectByKey<ne_xpo.cs.r_engine_report>(_id);
			id					= _id;
			name				= r.name;
			r_engine_category	= r.r_engine_category.id;
			query				= r.query;
			created_when		= r.created_when;
			parameters			= r.parameters;
			}
		private static bool exists(int _id, Session _uow)
			{
			var b = _uow.GetObjectByKey<ne_xpo.cs.r_engine_report>(_id);
			return b != null;
			}
		public void save()
			{
			using(var uow = new UnitOfWork())
				{
				var r				= id > 0 
										? uow.GetObjectByKey<ne_xpo.cs.r_engine_report>(id) 
										: new ne_xpo.cs.r_engine_report(uow);
				r.query				= query;
				r.r_engine_category	= uow.GetObjectByKey<ne_xpo.cs.r_engine_category>(r_engine_category);
				r.parameters		= parameters;
				r.name				= name;
				r.created_when		= id > 0 ? r.created_when : DateTime.Now;
				r.Save();
				uow.CommitChanges();
				id					= r.id;
				}
			}
		}
	public class category
		{
		public int id { get; set; }
		public string name  { get; set; }
		public category(){}
		public category(int _id)
			{
			using(var uow = new UnitOfWork())
				{
				if(exists(_id, uow))
					{
					load(_id, uow);
					}
				}
			}
		private void load(int _id, Session _uow)
			{
			var c		= _uow.GetObjectByKey<ne_xpo.cs.r_engine_category>(_id);
			id			= _id;
			name		= c.name;
			}
		private static bool exists(int _id, Session _uow)
			{
			var c = _uow.GetObjectByKey<ne_xpo.cs.r_engine_category>(_id);
			return c != null;
			}
		public void save()
			{
			using(var uow = new UnitOfWork())
				{
				var c				= id > 0 
										? uow.GetObjectByKey<ne_xpo.cs.r_engine_category>(id) 
										: new ne_xpo.cs.r_engine_category(uow);
				c.name				= name;
				c.Save();
				uow.CommitChanges();
				id					= c.id;
				}
			}
		}
	public class permission
		{
		public int id { get; set; }
		public int r_engine_report { get; set ;}
		public int member { get; set; }
		public permission(){}
		public permission(int _id)
			{
			using(var uow = new UnitOfWork())
				{
				if(exists(_id, uow))
					{
					load(_id, uow);
					}
				}
			}
		private void load(int _id, Session _uow)
			{
			var p				= _uow.GetObjectByKey<ne_xpo.cs.r_engine_permission>(_id);
			id					= _id;
			r_engine_report		= p.r_engine_report.id;
			member				= Convert.ToInt32(p.member.member_id);
			}
		private static bool exists(int _id, Session _uow)
			{
			var p = _uow.GetObjectByKey<ne_xpo.cs.r_engine_permission>(_id);
			return p != null;
			}
		public static void clear(int _member)
			{
			using(var uow = new UnitOfWork())
				{
				var p			= from x in new XPQuery<ne_xpo.cs.r_engine_permission>(uow)
									where x.member.member_id == _member
								  select x;
				foreach(var p_i in p)
					{
					p_i.Delete();
					}
				uow.CommitChanges();
				}
			}
		public void save()
			{
			using(var uow = new UnitOfWork())
				{
				var p				= id > 0 
										? uow.GetObjectByKey<ne_xpo.cs.r_engine_permission>(id) 
										: new ne_xpo.cs.r_engine_permission(uow);
				p.r_engine_report	= uow.GetObjectByKey<ne_xpo.cs.r_engine_report>(r_engine_report);
				p.member			= uow.GetObjectByKey<ne_xpo.cs.member>(member);
				p.Save();
				uow.CommitChanges();
				id					= p.id;
				}
			}
		}
	}
	}