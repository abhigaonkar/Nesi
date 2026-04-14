using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NEMemberTypePrivilege
	/// </summary>
	public class NEMemberTypePrivilege
		{
		private List<int> _TypePage;
		private List<int> _TypePrivilege;

		public int MemberTypePageID { get; set; }
		public int MemberTypePageTypeID { get; set; }
		public int MemberTypePagePageID { get; set; }
		public int MemberTypePagePrivilegeID { get; set; }
		public int MemberTypePagePrivilegeMemberTypePageID { get; set; }
		public int MemberTypePagePrivilegePrivilegeID { get; set; }
		public int MemberTypePagePrivilegeTypeID { get; set; }


	


		public void TypePages(int id)
			{
			var _typeprivs	= Toolbox.doSQL_dt(@"SELECT membertypepage_page_id page_id FROM membertypepage  WHERE membertypepage_type_id =@v0", new object[] { id });
			_TypePage = new List<int>();
			foreach(DataRow _typepriv in _typeprivs.Rows)
				{
				_TypePage.Add(Convert.ToInt32(_typepriv["page_id"]));
				}
			}
		public void TypePrivileges(int id)
			{
			var _typeprivs	= Toolbox.doSQL_dt(@"SELECT membertypepageprivilege_privilege_id privilege_id FROM membertypepageprivilege  WHERE membertypepageprivilege_type_id =@v0", new object[] { id });
			_TypePrivilege = new List<int>();
			foreach(DataRow _typepriv in _typeprivs.Rows)
				{
				_TypePrivilege.Add(Convert.ToInt32(_typepriv["privilege_id"]));
				}
			}
		public bool AuthenticatedForTypePage(int PageID)
			{
			try
				{
				return _TypePage.Any(row => row == PageID);
				}
			catch
				{
				return false;
				}
			}
		public bool AuthenticatedForTypePrivilege(int PrivilegeID)
			{
			try
				{
				return _TypePrivilege.Any(row => PrivilegeID == row);
				}
			catch
				{
				return false;
				}
			}
		public string CascadeMemberChanges(string typeID)
			{
			var members		= Toolbox.doSQL_dt(@"SELECT member_id id FROM member WHERE member_status = 'Active' AND member_membertype_id = @v0 ", new object[] {  typeID } );
			var counter				= 0;
			foreach(DataRow member in members.Rows)
				{
				SetMemberPrivileges(member["id"].ToString());
				counter++;
				}
			return string.Format("({0}) Employees had their privileges update to this member type.", counter);
			}
		public void ClearPrivileges(object member_id)
			{
			Toolbox.doSQL_void(@"DELETE FROM memberpageprivilege WHERE memberpageprivilege_member_id = @v0", new object[] { member_id});
			Toolbox.doSQL_void(@"DELETE FROM memberpage WHERE memberpage_member_id = @v0",new object[] { member_id});
			}
		public void SetBaseLoginPrivilege(object member_id)
			{
			// Make sure this is a new user.
			var c		= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM memberpage WHERE memberpage_member_id =@v0 ", new object[] { member_id}); 
			if(c == 0)
				{
				Toolbox.doSQL_void(@"INSERT INTO memberpage (memberpage_member_id,memberpage_page_id) VALUES (@v0,'1')", new object[] { member_id});;
				}
			}
		public string SetMemberPrivileges(object member_id)
			{
			var strMessage = "";
			try
				{
				var dt		= Toolbox.doSQL_dt(@"SELECT member_membertype_id FROM member WHERE member_id=@v0 ", new object[] {  member_id } );
				foreach(DataRow dr in dt.Rows)
					{
					var membertype_id	= Convert.ToInt32(dr["member_membertype_id"]);
					// delete old data before inserts
					ClearPrivileges(member_id);

					//Find The Page's This Member Type Has Access to.
					var membertype_page_dt	= Toolbox.doSQL_dt(@"SELECT membertypepage_id id, membertypepage_page_id page_id FROM membertypepage WHERE membertypepage_type_id = @v0 ", new object[] {  membertype_id } );
					foreach(DataRow membertype_dr in membertype_page_dt.Rows)
						{
						var id				= Convert.ToInt32(membertype_dr["id"]);
						var page_id			= Convert.ToInt32(membertype_dr["page_id"]);

						//Give employee Access to this Page & get last id
						var memberpage_id	= Toolbox.doSQL_return_id(@"INSERT INTO memberpage (memberpage_member_id,memberpage_page_id) VALUES (@v0,@v1)",new object[] { member_id, page_id});

						//Find All Privilieges associated with that page for that member type
						var page_priv_dt	= Toolbox.doSQL_dt(@" SELECT membertypepageprivilege_privilege_id privilege_id FROM membertypepageprivilege WHERE membertypepageprivilege_membertypepage_id = @v0  AND membertypepageprivilege_type_id = @v1  ", new object[] {  id, membertype_id } );
						foreach(DataRow priv_dr in page_priv_dt.Rows)
							{
							var priv_id			= Convert.ToInt32(priv_dr["privilege_id"]);
							//Give employee those Privileges associated with the member type
							Toolbox.doSQL_void(@"
INSERT INTO memberpageprivilege 
	(
	memberpageprivilege_memberpage_id,
	memberpageprivilege_privilege_id,
	memberpageprivilege_member_id
	) 
VALUES 
	(@v0,@v1,@v2)",
new object[] { memberpage_id, priv_id, member_id});
							}
						}
					}
				}
			catch
				{
				strMessage = "There Was an Error Attempting to Apply this Privilege Change";
				}
			return strMessage;
			}
		}
	}