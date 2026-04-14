using NESI.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NESI.BLL.Base;
using System.Data;
using MySql.Data.MySqlClient;
using System;
using System.Reflection;
using System.Runtime.Caching;
using NESI.Common;
using NESI.DTO.ViewModels.Page.Reports;

namespace NESI.BLL.Pages.Reports
{
	public class Layout : BLLBase
	{
		public async Task<Response<NESI.DTO.ViewModels.Page.Reports.GridViewLayout>> GetLayouts(string gridId, int memberId, BLL.Core.Employee.Employee user, QueryParam queryParam = null)
		{
			var layouts = new List<NESI.DTO.ViewModels.Page.Reports.GridViewLayout>();
			var response = new Response<NESI.DTO.ViewModels.Page.Reports.GridViewLayout>();
			var grid_query = "";

			var sql_global = @"
								SELECT g.GridviewLayouts_id AS id, 
                                g.GridviewLayouts_Member_ID AS memberId, 
                                g.GridviewLayouts_Name AS 'name',
                                g.GridviewLayout_Layout AS layout,
                                g.is_default AS isDefault,
                                g.gridviewlayouts_gridid AS gridId,
                                'Admin' AS firstName,
                                'Admin' AS fullName,
                                '' AS ddlName
                                FROM gridviewlayouts g
							    WHERE g.GridviewLayouts_Member_ID=0
								AND g.GridviewLayouts_GridID = '" + gridId + @"' AND LENGTH(g.GridviewLayout_Layout) > 0  
                                AND SUBSTRING(g.GridviewLayout_Layout,1,1) = '{' AND SUBSTRING(g.GridviewLayout_Layout,LENGTH(g.GridviewLayout_Layout),1) = '}'
			";

			if (memberId > 0)
			{
				grid_query = @"SELECT g.GridviewLayouts_id AS id, 
                                g.GridviewLayouts_Member_ID AS memberId, 
                                g.GridviewLayouts_Name AS 'name',
                                g.GridviewLayout_Layout AS layout,
                                g.is_default AS isDefault,
                                g.gridviewlayouts_gridid AS gridId,
                                m.member_firstname AS firstName,
                                m.member_fullname AS fullName,
                                b.ddl_Name AS ddlName
                                FROM gridviewlayouts g
                                INNER JOIN member m ON g.gridviewlayouts_member_id=m.member_id
								INNER JOIN business_unit b on m.business_unit_id=b.id
                                WHERE gridviewlayouts_member_id = " + memberId + @"
                                AND GridviewLayouts_GridID = '" + gridId + @"' AND LENGTH(g.GridviewLayout_Layout) > 0  
                                AND SUBSTRING(g.GridviewLayout_Layout,1,1) = '{' AND SUBSTRING(g.GridviewLayout_Layout,LENGTH(g.GridviewLayout_Layout),1) = '}'
								and find_in_set(m.business_unit_id,@p0) 
                                ORDER BY b.ddl_Name ASC, m.member_fullname ASC, g.is_default DESC ";


			}
			else if (memberId == -3) //all
			{
				grid_query = @"SELECT g.GridviewLayouts_id AS id, 
                                g.GridviewLayouts_Member_ID AS memberId, 
                                g.GridviewLayouts_Name AS 'name',
                                g.GridviewLayout_Layout AS layout,
                                g.is_default AS isDefault,
                                g.gridviewlayouts_gridid AS gridId,
                                m.member_firstname AS firstName,
                                m.member_fullname AS fullName,
                                b.ddl_Name AS ddlName
                                FROM gridviewlayouts g
                                INNER JOIN member m ON g.gridviewlayouts_member_id=m.member_id
								INNER JOIN business_unit b on m.business_unit_id=b.id
                                WHERE GridviewLayouts_GridID = '" + gridId + @"' AND LENGTH(g.GridviewLayout_Layout) > 0 
                                AND SUBSTRING(g.GridviewLayout_Layout,1,1) = '{' AND SUBSTRING(g.GridviewLayout_Layout,LENGTH(g.GridviewLayout_Layout),1) = '}'
								and find_in_set(m.business_unit_id,@p0) 
                                ORDER BY b.ddl_Name ASC, m.member_fullname ASC, g.is_default DESC ";


			}
			else if (memberId == -2) //blank
			{
				grid_query = @"SELECT g.GridviewLayouts_id AS id, 
                                g.GridviewLayouts_Member_ID AS memberId, 
                                g.GridviewLayouts_Name AS 'name',
                                g.GridviewLayout_Layout AS layout,
                                g.is_default AS isDefault,
                                g.gridviewlayouts_gridid AS gridId,
                                m.member_firstname AS firstName,
                                m.member_fullname AS fullName,
                                b.ddl_Name AS ddlName
                                FROM gridviewlayouts g
                                INNER JOIN member m ON g.gridviewlayouts_member_id=m.member_id
								INNER JOIN business_unit b on m.business_unit_id=b.id
                                WHERE GridviewLayouts_GridID = '" + gridId + @"' AND LENGTH(g.GridviewLayout_Layout) > 0
								and find_in_set(m.business_unit_id,@p0) 
                                    AND gridviewlayout_layout is NULL 
                                ORDER BY b.ddl_Name ASC, m.member_fullname ASC";

			}
			else if (memberId == -1) //non-blank
			{
				grid_query = @"SELECT g.GridviewLayouts_id AS id, 
                                g.GridviewLayouts_Member_ID AS memberId, 
                                g.GridviewLayouts_Name AS 'name',
                                g.GridviewLayout_Layout AS layout,
                                g.is_default AS isDefault,
                                g.gridviewlayouts_gridid AS gridId,
                                m.member_firstname AS firstName,
                                m.member_fullname AS fullName,
                                b.ddl_Name AS ddlName
                                FROM gridviewlayouts g
                                INNER JOIN member m ON g.gridviewlayouts_member_id=m.member_id
								INNER JOIN business_unit b on m.business_unit_id=b.id
                                WHERE GridviewLayouts_GridID = '" + gridId + @"' AND LENGTH(g.GridviewLayout_Layout) > 0
                                AND SUBSTRING(g.GridviewLayout_Layout,1,1) = '{' AND SUBSTRING(g.GridviewLayout_Layout,LENGTH(g.GridviewLayout_Layout),1) = '}'
								and find_in_set(m.business_unit_id,@p0) 
                                    AND gridviewlayout_layout is NOT NULL
                                ORDER BY b.ddl_Name ASC, m.member_fullname ASC";

			}

			grid_query = $"({sql_global}) union ({grid_query})";
			var dbResult = bllToolbox.doSQL_dt(grid_query, user.VisibleBusinessUnits);
			try
			{
				//	using (MySqlConnection conn = (MySqlConnection)_db.Database.Connection)
				//	{
				//		using (MySqlCommand cmd = new MySqlCommand(grid_query, conn))
				//		{
				//			cmd.CommandTimeout = 300;
				//			cmd.CommandType = CommandType.Text;
				//			using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
				//			{
				//				sda.Fill(dbResult);
				//			}
				//		}
				//	}

				layouts = ConvertDataTableToCustomList(dbResult, layouts);
				if (layouts != null && layouts.Count > 0)
				{
					layouts = layouts.Select(p => new NESI.DTO.ViewModels.Page.Reports.GridViewLayout()
					{
						id = p.id,
						memberId = p.memberId,
						name = p.name,
						layout = p.layout,
						isDefault =
						p.isDefault,
						gridId = p.gridId,
						firstName = p.firstName,
						fullName = p.fullName,
                        ddlName = p.ddlName

					}).ToList();

				}
			}
			catch (System.Exception ex)
			{

			}

			//total count
			//response.TotalCount = layouts.Count;

			response.data = layouts;

			//if (queryParam == null)
			//{
			//    response.data = layouts;
			//}
			//else
			//{
			//    //pagination
			//    response.data = layouts.Skip((queryParam.page_count - 1) * queryParam.page_size)
			//                    .Take(queryParam.page_size)
			//                    .ToList();
			//}


			return response;
		}



		public async Task<List<NESI.DTO.ViewModels.Page.Reports.GridViewLayout>> GetMemberLayoutById(int layoutId)
		{
			return _db.gridviewlayouts.Where(p => p.GridviewLayouts_id == layoutId).Select(p => new NESI.DTO.ViewModels.Page.Reports.GridViewLayout() { id = p.GridviewLayouts_id, memberId = p.GridviewLayouts_Member_ID, name = p.GridviewLayouts_Name, layout = p.GridviewLayout_Layout, isDefault = p.is_default, gridId = p.gridviewlayouts_gridid }).ToList();
		}


		public async Task<bool> UpdateLayout(NESI.DTO.ViewModels.Page.Reports.GridViewLayout layout)
		{
			var gridviewlayout = _db.gridviewlayouts.First(p => p.GridviewLayouts_id == layout.id);

			gridviewlayout.GridviewLayouts_Name = layout.name;
			gridviewlayout.is_default = layout.isDefault;
			gridviewlayout.GridviewLayout_Layout = layout.layout;
			return await SaveChanges();

		}

		public async Task<bool> UpdateLayoutStatus(NESI.DTO.ViewModels.Page.Reports.GridViewLayout layout, string gridLayoutId, int memberId, BLL.Core.Employee.Employee user)
		{

			var response = await this.GetLayouts(gridLayoutId, memberId, user);




			if (response == null)
			{
				return false;
			}

			foreach (NESI.DTO.ViewModels.Page.Reports.GridViewLayout memberLayout in response.data)
			{
				memberLayout.isDefault = 0;
				await UpdateLayout(memberLayout);
			}
			if (layout.memberId != 0)
			{
				var gridviewlayout = _db.gridviewlayouts.First(p => p.GridviewLayouts_id == layout.id);
				gridviewlayout.GridviewLayouts_Name = layout.name;
				gridviewlayout.GridviewLayouts_Member_ID = layout.memberId;
				gridviewlayout.is_default = layout.isDefault;
				gridviewlayout.GridviewLayout_Layout = layout.layout;
			}
			return await SaveChanges();

		}

		private async Task<bool> SaveChanges()
		{
			var updated = await _db.SaveChangesAsync();
			return true;
		}

		public async Task<int> AddLayout(NESI.DTO.ViewModels.Page.Reports.GridViewLayout layout)
		{
			var gridviewlayouts = _db.gridviewlayouts.Add(new gridviewlayouts() { GridviewLayouts_Member_ID = layout.memberId, GridviewLayouts_Name = layout.name, is_default = layout.isDefault, gridviewlayouts_gridid = layout.gridId, GridviewLayout_Layout = layout.layout });
			await SaveChanges();
			return gridviewlayouts.GridviewLayouts_id;

		}

		public async Task<bool> DeleteLayout(int gridlayoutId)
		{
			var gridviewlayout = _db.gridviewlayouts.First(p => p.GridviewLayouts_id == gridlayoutId);
			if (gridviewlayout != null)
			{
				_db.gridviewlayouts.Remove(gridviewlayout);
				return await SaveChanges();
			}
			else
				return false;


		}

		public object GetAllLayoutMembers(string gridId, BLL.Core.Employee.Employee user)
		{
			List<MemberWithUnit> memberDetails = new List<MemberWithUnit>();

			string query = @"SELECT 	m.member_id AS Member_ID, 
	                            m.member_fullname AS member_fullname, 
	                            m.member_user AS Member_User, 
	                            m.member_status AS Member_Status, 
	                            m.member_firstName AS Member_FirstName, 
	                            m.member_lastName AS Member_LastName, 
	                            m.business_unit_id AS business_unit_id,
                                b.ddl_Name AS ddlName
                            FROM member m
                            INNER JOIN business_unit b ON m.business_unit_id=b.id
                            WHERE member_status='Active' and (m.member_id=@p2 OR find_in_set(business_unit_id,@p1) and  m.member_id IN (
                            SELECT DISTINCT (GridviewLayouts_Member_ID) FROM gridviewlayouts
                            WHERE GridviewLayouts_GridID = @p0 AND LENGTH(GridviewLayout_Layout) > 0  
                            AND SUBSTRING(GridviewLayout_Layout,1,1) = '{' AND SUBSTRING(GridviewLayout_Layout,LENGTH(GridviewLayout_Layout),1) = '}'))
                            ORDER BY  b.ddl_Name ASC, m.member_fullname ASC ";

		    DataTable dbResult = bllToolbox.doSQL_dt(query, gridId, user.VisibleBusinessUnits, user.Id);
			memberDetails = ConvertDataTable<MemberWithUnit>(dbResult);
		    var test = (from m in memberDetails
		        select new {Member_ID = m.Member_ID, member_fullname = m.member_fullname, Member_Status = m.Member_Status, ddlName = m.ddlName });

            return test.ToList() ;
		}

		private static List<T> ConvertDataTable<T>(DataTable dt)
		{
			List<T> data = new List<T>();
			foreach (DataRow row in dt.Rows)
			{
				T item = GetItem<T>(row);
				data.Add(item);
			}
			return data;
		}
		private static T GetItem<T>(DataRow dr)
		{
			Type temp = typeof(T);
			T obj = Activator.CreateInstance<T>();

			foreach (DataColumn column in dr.Table.Columns)
			{
				foreach (PropertyInfo pro in temp.GetProperties())
				{
					if (pro.Name == column.ColumnName)
						pro.SetValue(obj, dr[column.ColumnName], null);
					else
						continue;
				}
			}
			return obj;
		}
		private static bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
		}

		private List<NESI.DTO.ViewModels.Page.Reports.GridViewLayout> ConvertDataTableToCustomList(DataTable dt, List<NESI.DTO.ViewModels.Page.Reports.GridViewLayout> layouts)
		{

			foreach (DataRow row in dt.Rows)
			{
				NESI.DTO.ViewModels.Page.Reports.GridViewLayout layout = new DTO.ViewModels.Page.Reports.GridViewLayout();

				if (row["id"] != DBNull.Value)
				{
					layout.id = Convert.ToInt32(row["id"]);
				}
				if (row["name"] != DBNull.Value)
				{
					layout.name = Convert.ToString(row["name"]);
				}
				if (row["isDefault"] != DBNull.Value)
				{
					layout.isDefault = Convert.ToInt32(row["isDefault"]);
				}

				if (row["firstName"] != DBNull.Value)
				{
					layout.firstName = Convert.ToString(row["firstName"]);
				}
				if (row["gridId"] != DBNull.Value)
				{
					layout.gridId = Convert.ToString(row["gridId"]);
				}
				if (row["layout"] != DBNull.Value)
				{
					layout.layout = Convert.ToString(row["layout"]);
				}
				if (row["fullName"] != DBNull.Value)
				{
					layout.fullName = Convert.ToString(row["fullName"]);
				}

				if (row["memberId"] != DBNull.Value)
				{
					layout.memberId = (long?)(row.IsNull("memberId") ? (long?)null : Convert.ToInt64(row["memberId"]));
				}
                if (row["ddlName"] != DBNull.Value)
                {
                    layout.ddlName = Convert.ToString(row["ddlName"]);
                }

                layouts.Add(layout);
			}

			return layouts;
		}

	}
}
