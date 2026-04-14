using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace NESI.DTO.Mapper
{
	public static class DataTableMapper
	{
		public static List<T> Map<T>(DataTable dt)
		{
			return (from DataRow row in dt.Rows select GetItem<T>(row)).ToList();
		}
		public static T GetItem<T>(DataRow dr)
		{
			var temp = typeof(T);
			var obj = Activator.CreateInstance<T>();

			foreach (DataColumn column in dr.Table.Columns)
			{
				foreach (var pro in temp.GetProperties())
				{
					if (string.Equals(pro.Name, column.ColumnName, StringComparison.CurrentCultureIgnoreCase))
						pro.SetValue(obj, dr[column.ColumnName] is DBNull ? null: dr[column.ColumnName], null);
					else
						continue;
				}
			}
			return obj;
		}
	}
}