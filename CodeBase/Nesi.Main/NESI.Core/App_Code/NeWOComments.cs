using System;
using System.Data;
using System.Collections;
using NESI.Common.Models;
using MySql.Data.MySqlClient;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeWOComments
	/// </summary>
	public class NeWOComments
		{
		private int _WoComment_Child_ID=0;
		private string _Modified_Date;
		private string _WOType;
		private Toolbox _tools			= new Toolbox();
		public NeWOComments()
			{
			HoursWorked = "";
			company_child_id = 0;
			PrintComments = 0;
			//
			//  
			//
			}
		public int WoCommentID { get; set; }
		public int WoCommentFirstID { get; set; }
		public int WoComment_Member_ID { get; set; }
		public string WorkOrderID { get; set; }
		public int woprog_id { get; set; }
		public string WorkOrderChildID { get; set; }

		public string Comments { get; set; }
		public string HoursWorked { get; set; }
		public int Personal { get; set; }
		public int PrintComments { get; set; }
		public int MemberIDAudit { get; set; }
		public string CreatedDate { get; set; }
		public int business_unit_id { get; set; }
		public int company_child_id { get; set; }
		public string EntryDate { get; set; }
		public NeWOComments(int id)
			{
			HoursWorked = "";
			company_child_id = 0;
			PrintComments = 0;
			var dt = Toolbox.doSQL_dt(@"SELECT WOComment_ID, Comments FROM WOComment Where WoComment_Member_ID != 0 and WOComment_ID = @v0 ", new object[] {  id } );
			if(dt.Rows.Count == 1)
				{
				InitComment(dt.Rows[0]);
				}
			}
		private void InitComment(DataRow dr)
			{
			WoCommentID = Convert.ToInt32(dr["wocomment_id"]);
			Comments = dr["Comments"].ToString();
			}
    
		public ArrayList LoadWOCommentsList(int selwono, string WO_Type, int selmember, int selbusiness_unit_id)
			{
			//string strWOcomments = "SELECT WoComment_ID, WorkOrder_ID, Comments FROM wocomment Where WorkOrder_ID ='" + selwono.ToString().Trim() + "'";
			//string strWOcomments = "Select WC.* From wocomment WC inner join membertime MT on WC.wocomment_member_id = MT.business_unit_id where WC.personal = '1' and WC.WorkOrder_ID = '" + selwono.ToString().Trim() + "' and MT.WOType ='" + WO_Type + "'"
			//+ " union Select WC.* from wocomment WC inner join membertime MT on WC.wocomment_ID = MT.MemberTime_WOComment_ID where WC.personal <> '1' and WC.WorkOrder_ID = '" + selwono.ToString().Trim() + "' and MT.WOType ='" + WO_Type + "'";
			//string strWOcomments = "Select WC.* From wocomment WC inner join membertime MT on WC.wocomment_member_id = MT.business_unit_id where WC.personal = '1' and WC.WorkOrder_ID = '" + selwono.ToString().Trim() + "' and MT.WOType ='" + WO_Type + "'"
			//+ " union Select WC.* from wocomment WC, membertime MT where WC.personal <> '1' and WC.WorkOrder_ID = '" + selwono.ToString().Trim() + "' and MT.WOType ='" + WO_Type + "'";
        
			/*string strWOcomments = "Select WC.* From wocomment WC "
                               + " Left join membertime MT on WC.wocomment_member_id = '" + selmember + "' and MT.WOType = '" + WO_Type + "' where WC.personal = '1' and WC.wocomment_member_id <> '0' and MT.Membertime_Mileage ='false' and WC.WOComment_Company_ID =" + selbusiness_unit_id + " and WC.WorkOrder_ID = '" + selwono.ToString().Trim()
                               + "' union Select WC.* from wocomment WC left join membertime MT on WC.WorkOrder_ID = MT.MemberTime_WorkOrder_ID and MT.WOType = '" + WO_Type + "' where WC.personal <> '1' and MT.Membertime_Mileage ='false' and WC.wocomment_member_id <> '0' and WC.WorkOrder_ID ='" + selwono.ToString().Trim() + "'"; */
			var _dt			= Toolbox.doSQL_dt(@"SELECT a.wocomment_id id, a.comments comment FROM wocomment a LEFT JOIN membertime b ON a.wocomment_id = b.MemberTime_WoComment_ID WHERE (a.woprog_id = @v0 OR IF(@v0 < 100000, b.wotype = 'Shop', FALSE)) AND a.wocomment_member_id != 0 AND a.business_unit_id =@v1 and a.wocomment_member_id = @v2  order by comments", new object[] {  selwono, selbusiness_unit_id, selmember } );
			var list			= new ArrayList();
			//Due to error in database entry we have to get the last comment to deal with this;
			var hold			= new ArrayList();
			var y					= 0;
			var comment_copy		= "";
			foreach (DataRow _dr in _dt.Rows)
				{
				var id				= Convert.ToInt32(_dr["id"]);
				var comment		= _dr["comment"].ToString();
				if (comment.ToUpper().Trim() == comment_copy.ToUpper().Trim()) continue;
				comment_copy = comment;
				var item = new NeWOComments();
				item.WoCommentID = id;
				item.Comments = _tools.value_from(comment);
				list.Add(item);
				y++;
				hold.Add(id);
				}
			if (y != 0)
				{
				try
					{
					y = y - 1;
					WoCommentFirstID = Convert.ToInt32(hold[y]);
					}catch{}
				}
			return list;
			}
		public void UpdateWOComment(int memid, int commentid, MySqlConnection connection = null, MySqlTransaction transaction = null)
		{
			Comments.Replace("\"", "");
			Comments.Replace("\'", "");
			Comments.Replace(",", " and ");

			if (connection == null || transaction == null)
				Toolbox.doSQL_void(
				@"Update WOCOMMENT Set Comments =@v0  ,Member_ID_Audit =@v1  ,Modified_Date = NOW() Where WOCOMMENT_ID = @v2 ",
				new object[] { Comments, MemberIDAudit, commentid });
			else
				Toolbox.doSQL_void(connection,
				@"Update WOCOMMENT Set Comments =@v0  ,Member_ID_Audit =@v1  ,Modified_Date = NOW() Where WOCOMMENT_ID = @v2 ",
				new object[] { Comments, MemberIDAudit, commentid }, transaction);
		}
		public void AddWOComment(int mem_id, int iUpdate, out int _WoComment_ID, out int _WoComment_Child_ID, bool ZeroCommentOnly, int __woprog_id
					  , MySqlConnection connection = null, MySqlTransaction transaction = null)
		{
			_WoComment_Child_ID = 0;
			_WoComment_ID = 0;
			woprog_id = __woprog_id;
			var ChildFlag = 0;

			if (!ZeroCommentOnly)
			{
				if (connection == null || transaction == null)
					_WoComment_ID = UpdateComments(mem_id, ChildFlag);
				else
					_WoComment_ID = UpdateComments(mem_id, ChildFlag, connection, transaction);
				_WoComment_Child_ID = 0;
				if (company_child_id != 0)
				{
					ChildFlag = 1;
					if (connection == null || transaction == null)
						_WoComment_Child_ID = UpdateComments(mem_id, ChildFlag);
					else
						_WoComment_Child_ID = UpdateComments(mem_id, ChildFlag, connection, transaction);
				}
			}

			//Insert another Record
			//check status of work oder
			var wostatus = "";
			var childwostatus = "";
			try
			{
				if (!string.IsNullOrEmpty(WorkOrderID))
				{
					if (connection == null || transaction == null)
						wostatus = _tools.getSQL_string(@"select ifnull((SELECT IFNULL(MAX(woprog_status), '') FROM woprog WHERE woprog_bvwo = @v0  AND business_unit_id = @v1 ),'')", new object[] { WorkOrderID, business_unit_id });
					else
						wostatus = _tools.getSQL_string(connection, @"select ifnull((SELECT IFNULL(MAX(woprog_status), '') FROM woprog WHERE woprog_bvwo = @v0  AND business_unit_id = @v1 ),'')", new object[] { WorkOrderID, business_unit_id });
				}
			}
			catch { }
			var shoptimecount = 0;
			try
			{

				if (!string.IsNullOrEmpty(WorkOrderID))
					if (connection == null || transaction == null)
						shoptimecount = _tools.getSQL_int(@"SELECT count(*) FROM membertime_shop_link WHERE ref_num = @v0 ", new object[] { WorkOrderID });
					else
						shoptimecount = _tools.getSQL_int(connection, @"SELECT count(*) FROM membertime_shop_link WHERE ref_num = @v0 ", new object[] { WorkOrderID });
			}
			catch { }
			//check if there is already a record then update it.
			ChildFlag = 0;
			if (wostatus != OpsWOStatus.Open && shoptimecount == 0)
				AddToRecord(mem_id, ChildFlag, iUpdate, connection, transaction);
			if (company_child_id != 0)
			{
				if (!string.IsNullOrEmpty(WorkOrderChildID))
				{
					try
					{
						if (!string.IsNullOrEmpty(WorkOrderChildID))
						{
							childwostatus = _tools.getSQL_string(connection, @"Select ifnull((SELECT woprog_status FROM woprog WHERE woprog_bvwo = @v0  AND business_unit_id = @v1 ),'')", new object[] { WorkOrderChildID, company_child_id });
						}
					}
					catch { }
				}
				ChildFlag = 1;
				if (childwostatus != OpsWOStatus.Open)
					AddToRecord(mem_id, ChildFlag, iUpdate, connection, transaction);
			}

		}
		public int UpdateComments(int mem_id, int ChildFlag, MySqlConnection connection = null, MySqlTransaction transaction = null)
		{
			var used_c_id = ChildFlag == 0 ? business_unit_id.ToString() : company_child_id.ToString();
			var used_w_id = ChildFlag == 0 ? WorkOrderID : WorkOrderChildID;
			string id;
			if (connection == null || transaction == null)
				id = _tools.returnSQL_id(@" INSERT INTO wocomment ( workorder_id, wocomment_member_id, comments, personal, printcomments, member_id_audit, created_date, modified_date, business_unit_id , woprog_id ) VALUES ( @v0 , @v6 , @v1 , @v2 , @v3 , @v4 , NOW(), NOW(), @v5 ,@v7  )",
				new object[] { used_w_id, Comments, Personal, PrintComments, mem_id, used_c_id, WoComment_Member_ID, woprog_id });
			else
				id = _tools.returnSQL_id(@" INSERT INTO wocomment ( workorder_id, wocomment_member_id, comments, personal, printcomments, member_id_audit, created_date, modified_date, business_unit_id , woprog_id ) VALUES ( @v0 , @v6 , @v1 , @v2 , @v3 , @v4 , NOW(), NOW(), @v5 ,@v7  )",
				new object[] { used_w_id, Comments, Personal, PrintComments, mem_id, used_c_id, WoComment_Member_ID, woprog_id }, connection, transaction);
			return Convert.ToInt32(id);
		}
		public void AddToRecord(int mem_id, int ChildFlag, int iUpdate, MySqlConnection connection = null, MySqlTransaction transaction = null)
		{
			var namename = new NeMember(mem_id);
			var format_comment = string.Format("{0}: {1}: {2} Hours: {3}", EntryDate, namename.FullName, HoursWorked, Comments);
			var used_c_id = ChildFlag == 0 ? business_unit_id.ToString() : company_child_id.ToString();
			var used_w_id = ChildFlag == 0 ? WorkOrderID : WorkOrderChildID;
			var exists = _tools.getSQL_int(connection, @"SELECT COUNT(*) FROM wocomment WHERE workorder_id = @v0  AND business_unit_id = @v1  AND wocomment_member_id = '0'", new object[] { used_w_id, used_c_id });

			if (exists > 0)
			{

				if (connection == null || transaction == null)
					_tools.getSQL_void(@" UPDATE wocomment SET comments = CONCAT(comments,'\n ',@v0) , modified_date = NOW() WHERE workorder_id = @v1  AND wocomment_member_id = 0 AND business_unit_id = @v2 ",
					new object[] { format_comment, used_w_id, used_c_id });
				else
					_tools.getSQL_void(@" UPDATE wocomment SET comments = CONCAT(comments,'\n ',@v0) , modified_date = NOW() WHERE workorder_id = @v1  AND wocomment_member_id = 0 AND business_unit_id = @v2 ",
					new object[] { format_comment, used_w_id, used_c_id }, connection, transaction);

			}
			else
			{
				if (connection == null || transaction == null)
					_tools.getSQL_void(@" INSERT INTO wocomment ( workorder_id, wocomment_member_id, comments, personal, printcomments, member_id_audit, created_date, modified_date, business_unit_id,woprog_id ) VALUES ( @v0 , 0, @v1 , @v2 , @v3 , @v4 , NOW(), NOW(), @v5 ,@v6  )",
					new object[] { used_w_id, format_comment, Personal, PrintComments, mem_id, used_c_id, woprog_id });
				else
					_tools.getSQL_void(@" INSERT INTO wocomment ( workorder_id, wocomment_member_id, comments, personal, printcomments, member_id_audit, created_date, modified_date, business_unit_id,woprog_id ) VALUES ( @v0 , 0, @v1 , @v2 , @v3 , @v4 , NOW(), NOW(), @v5 ,@v6  )",
					new object[] { used_w_id, format_comment, Personal, PrintComments, mem_id, used_c_id, woprog_id }, connection, transaction);

			}
		}

		public void AddBusinessDevelopmentWOComment(int mem_id, int iUpdate, out int _WoComment_ID, MySqlConnection conn = null, MySqlTransaction transaction= null)
			{
			_WoComment_ID = 0;
			var ChildFlag = 0;
			Comments.Replace("\"", "");
			Comments.Replace("\'", "");
			Comments.Replace(",", " and ");

			_WoComment_ID = UpdateComments(mem_id, ChildFlag, conn, transaction); 
			}
		}
	}