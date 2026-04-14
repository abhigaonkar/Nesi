using System;
using System.Data;

namespace NESI.BLL.Common.Shared
{
	public static class DataTablePagination
	{
		public static DataTable GetPageData(DataTable dt, int page, int pageSize, string sortField, int sortOrder)
		{
			var dv=new DataView(dt) {Sort = sortField + (sortOrder ==1 ? " ASC" : " DESC")};
			
			var newDt = new DataTable();
			foreach (var column in dt.Columns)
			{
				newDt.Columns.Add(new DataColumn(column.ToString()));
			}

			dt = dv.ToTable();
			for (var i = page * pageSize; i < Math.Min(dt.Rows.Count, (page + 1) * pageSize); i++)
			{
				var dr = dt.Rows[i];
				var newRow = newDt.NewRow();
				foreach (var column in dt.Columns)
				{
					newRow[column.ToString()] = dt.Rows[i][column.ToString()];
				}
				newDt.Rows.Add(newRow);
			}
			return newDt;
		}
	}
}