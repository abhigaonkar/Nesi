using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Microsoft.Ajax.Utilities;
using nesi.core;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Shared;

namespace NESI.BLL.Base
{
	public abstract class BLLBase
	{

		public bool loaded => true;
		public int UserId => CurrentUser?.Id ?? 0;
		public int BusinessUnitId => CurrentUser?.BusinessUnitId ?? 0;

		protected NESIMySQL _db;
		protected readonly Employee CurrentUser;
		protected BLLToolbox bllToolbox;

		protected BLLBase()
		{
			_db = new NESIMySQL();
			bllToolbox = new BLLToolbox(_db);
		}

		protected BLLBase(Employee user)
		{
			_db = new NESIMySQL();
			bllToolbox = new BLLToolbox(_db);
			CurrentUser = user;
		}

		protected List<int> GetReportsToAllList(int user_id)
		{
			var temp_list = bllToolbox.doSQL_string(@"SELECT IFNULL(reports_to_all(@v0 ), '')", user_id);
			return temp_list.Contains(",")
				? temp_list.Split(',').Select(int.Parse).ToList()
				: temp_list != ""
					? new List<int> { Convert.ToInt32(temp_list) }
					: new List<int>();
		}

		public LabelValueInt[] VisibleBusinessUnit()
		{
			return CurrentUser.VisibleBusinessUnitList
				.Select(x => new LabelValueInt() { Label = x.ddl_name, Value = x.Id })
				.OrderBy(x => x.Label)
				.ToArray();
		}

		public LabelValueInt[] VisibleTaxentity()
		{
			return CurrentUser.VisibleBusinessUnitList
				.Select(x => new LabelValueInt() { Label = x.tax_entity_name, Value = x.tax_entity_id })
				.DistinctBy(x=> x.Value)
				.OrderBy(x => x.Label)
				.ToArray();
		}

		public List<DTO.Models.Core.BusinessUnit> GetVisibleBusinessUnitList(string str_list)
		{
			var list = string.IsNullOrEmpty(str_list)? Enumerable.Empty<string>().ToArray() : str_list.Split(',');
			var o = Global.BusinessUnit.GetActiveList()
				.Where(b => b.ID != CurrentUser.BusinessUnitId && list.Contains(b.ID.ToString()))
				.OrderBy(x => x.ddl_Name)
				.ToList();
			o.Insert(0, Global.BusinessUnit.GetValue(CurrentUser.BusinessUnitId));
			return o;
		}


		public static List<DTO.Models.Core.BusinessUnit> GetVisibleBusinessUnitList2(string[] list)
		{
			var o = Global.BusinessUnit.GetActiveList()
				.Where(b => list.Contains(b.ID.ToString()))
				.OrderBy(x => x.ddl_Name)
				.ToList();
			return o;
		}

		public static List<DTO.ViewModels.Core.BusinessUnitDropDownList> GetVisibileBusinessUnitDropDownLists2(string[] list)
		{
			return AutoMapper.Mapper.Map<List<DTO.ViewModels.Core.BusinessUnitDropDownList>>(GetVisibleBusinessUnitList2(list));
		}

		public List<DTO.ViewModels.Core.BusinessUnitDropDownList> GetVisibileBusinessUnitDropDownLists(string str_list)
		{
			return AutoMapper.Mapper.Map<List<DTO.ViewModels.Core.BusinessUnitDropDownList>>(GetVisibleBusinessUnitList(str_list));
		}

		public bool IsVisibleTaxentity(int te)
		{
			var o = VisibleTaxentity();
			return o.Any(x => x.Value == te);
		}

		public LabelValueInt[] GetProjectManagerList(int buId)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"
					SELECT 
					a.member_id value,
					CONCAT(b.name, ' - ', member_fullname) label
				FROM 
					Member  a
				LEFT JOIN
				 business_unit b ON a.business_unit_id = b.id
				left join membertype mt on a.member_membertype_id = mt.membertype_id
				WHERE 
					(a.business_unit_id = @p0 AND 
					member_status = 'Active') OR
					(mt.considered_pm AND 
					member_status = 'Active') 
				ORDER BY 
					b.name, a.member_firstname,a.member_lastname
				", buId);
		}

		public LabelValueInt[] GetVisibleUserList(int user_id, int offer_member_id)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"call get_visible_users_n2(@p0,@p1)", new object[] { user_id,offer_member_id});
		}

		public LabelValueInt[] GetMemberTypeList(bool isactive = true)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT membertype_id value, membertype_name label FROM membertype " + (isactive ? " WHERE active = true AND is_chargeout = 0" : ""));
		}

		public LabelValueInt[] GetPayTypeList(bool include_not_set = false)
		{
			var o = bllToolbox.doSQL_List<LabelValueInt>(@"SELECT id value, paytype label FROM member_paytype");
			if (include_not_set)
			{
				o.Insert(0, new LabelValueInt() { Label = "Not Set", Value = 0 });
			}

			return o.ToArray();
		}

		public LabelValueString[] GetCountryList()
		{
			return NeCountry.get_list().Select(x => new LabelValueString() { Label = x.name, Value = x.code }).ToArray();
		}

		public LabelValueString[] GetCountryList2()
		{
			return new[]
			{
				new LabelValueString("Canada","CAN"),
				new LabelValueString("USA","USA"),
				new LabelValueString("Other","Other")
			};
		}

		public LabelValueString[] GetProvList()
		{
			return bllToolbox.doSQL_Array<LabelValueString>(@"SELECT prov_desc label,prov_abbv value FROM prov");
		}



		public string SaveHomeLayout(OrderHomeLayout model, string prefix)
		{
			var profile = new NESI.BLL.Base.ProfileBase(CurrentUser);
			profile.SetPropertyValue($"{prefix}HomeLayout_selected_business_units",
				profile.JoinIntArrayToString(model.selected_businessUnits));
			profile.SetPropertyValue($"{prefix}HomeLayout_openned_tax_entities",
				profile.JoinIntArrayToString(model.openned_tax_entities));
			return "Layout has been saved Successfully.";
		}

		public string SaveUserSetting(string name, string value)
		{
			var profile = new NESI.BLL.Base.ProfileBase(CurrentUser);
			profile.SetPropertyValue(name, value);
			return "Setting has been saved Successfully.";
		}

		public string GetUserSetting(string name)
		{
			var profile = new NESI.BLL.Base.ProfileBase(CurrentUser);
			return profile.GetValueByPropertyName(name);
		}

		protected LabelValueInt[] GetLabelValueIntListFromSQL(string sql, params object[] args)
		{
			return _db.Database.SqlQuery<LabelValueInt>(sql, args).ToArray();
		}

		protected LabelValueString[] GetLabelValueStringListFromSQL(string sql, params object[] args)
		{
			return _db.Database.SqlQuery<LabelValueString>(sql, args).ToArray();
		}

		protected T[] GetObjectListFromSQL<T>(string sql, params object[] args)
		{
			return _db.Database.SqlQuery<T>(sql, args).ToArray();
		}

		public class BLLToolbox
		{
			private readonly NESIMySQL _tdb;

			public BLLToolbox(NESIMySQL db)
			{
				_tdb = db;
			}

			public string MySQLNow_long()
			{
				return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			}

			public string MySQLNow_short()
			{
				return DateTime.Now.ToString("yyyy-MM-dd");
			}

			public string MySQL_shortdt(DateTime _dt)
			{
				return _dt.Year < 1900 ? "" : _dt.ToString("yyyy-MM-dd");
			}

			public string ConvertVtoP(string sql)
			{
				return sql.Replace("@v", "@p");
			}

			public int doSQL_return_id(string sql, params object[] paramObjects)
			{
				return doSQL_int(sql + ";SELECT LAST_INSERT_ID()", paramObjects);
			}

			public double doSQL_double(string sql, params object[] paramsObjects)
			{
				return doSQL_Object<double>(sql, paramsObjects);
			}

		    public double? doSQL_doubleOrNull(string sql, params object[] paramsObjects)
		    {
		        return doSQL_Object<double?>(sql, paramsObjects);
		    }


            public int doSQL_int(string sql, params object[] paramsObjects)
			{
				return doSQL_Object<int>(sql, paramsObjects);
			}

			public long doSQL_long(string sql, params object[] paramsObjects)
			{
				return doSQL_Object<long>(sql, paramsObjects);
			}

			public string doSQL_string(string sql, params object[] paramsObjects)
			{
				return doSQL_Object<string>(sql, paramsObjects);
			}

			public DateTime doSQL_datetime(string sql, params object[] paramsObjects)
			{
				return doSQL_Object<DateTime>(sql, paramsObjects);
			}

			public bool doSQL_bool(string sql, params object[] paramsObjects)
			{
				return doSQL_Object<bool>(sql, paramsObjects);
			}

			public DataTable doSQL_dt(string sql, params object[] paramsObjects)
			{
				return _tdb.DataTable(ConvertVtoP(sql), paramsObjects);
			}

			public DataTable doSQL_dtFaster(string sql, params object[] paramsObjects)
			{
				return _tdb.DataTableFaster(ConvertVtoP(sql), paramsObjects);
			}
			public void doSQL_void(string sql, params object[] paramsObjects)
			{
				_tdb.Database.ExecuteSqlCommand(ConvertVtoP(sql), paramsObjects);
			}

			public T[] doSQL_Array<T>(string sql, params object[] paramsObjects)
			{
				return _tdb.Database.SqlQuery<T>(ConvertVtoP(sql), paramsObjects).ToArray();
			}

			public List<T> doSQL_List<T>(string sql, params object[] paramsObjects)
			{
				return _tdb.Database.SqlQuery<T>(ConvertVtoP(sql), paramsObjects).ToList();
			}

			public T doSQL_Object<T>(string sql, params object[] paramsObjects)
			{
				return _tdb.Database.SqlQuery<T>(ConvertVtoP(sql), paramsObjects).FirstOrDefault();
			}

			public byte[] doSQL_BLOB(string sql, params object[] paramsObjects)
			{
				var _BLOBstring = doSQL_string(sql, paramsObjects);
				if (string.IsNullOrEmpty(_BLOBstring)) return null;
				var _BLOB = Convert.FromBase64String(_BLOBstring);
				return _BLOB;
			}
		}


		public static object MapperFrom(object to_object, object from_object)
		{
			var dtoType = from_object.GetType();
			var sourceType = to_object.GetType();
			var props = new List<PropertyInfo>(dtoType.GetProperties());
			var sourceProps = new List<PropertyInfo>(sourceType.GetProperties());

			foreach (var prop in props)
			{
				if (sourceProps.FirstOrDefault(x => string.Equals(x.Name, prop.Name, StringComparison.CurrentCultureIgnoreCase)) == null) continue;
				var property = sourceType.GetProperty(prop.Name, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
				if (property == null) continue;
				if (!property.CanWrite) continue;
				var propertyValue = prop.GetValue(from_object, null);
				var propertyType = property.PropertyType;
				var value = "";
				if (propertyValue != null)
				{
					value = propertyValue.ToString();
				}
				var typeCode = System.Type.GetTypeCode(propertyType);
				switch (typeCode)
				{
					case TypeCode.Int16:
						if (short.TryParse(value, out var dint16))
						{
							property.SetValue(to_object, dint16, null);
						}
						break;

					case TypeCode.UInt16:
						if (ushort.TryParse(value, out var duint16))
						{
							property.SetValue(to_object, duint16, null);
						}
						break;
					case TypeCode.Int32:
						if (int.TryParse(value, out var dint32))
						{
							property.SetValue(to_object, dint32, null);
						}
						break;
					case TypeCode.UInt32:
						if (uint.TryParse(value, out var duint32))
						{
							property.SetValue(to_object, duint32, null);
						}
						break;
					case TypeCode.UInt64:
						if (UInt64.TryParse(value, out var duint64))
						{
							property.SetValue(to_object, duint64, null);
						}
						break;
					case TypeCode.Int64:
						if (long.TryParse(value, out var dint64))
						{
							property.SetValue(to_object, dint64, null);
						}

						break;
					case TypeCode.Single:
						if (float.TryParse(value, out var dsingle))
						{
							property.SetValue(to_object, dsingle, null);
						}

						break;
					case TypeCode.Double:
						if (double.TryParse(value, out var ddouble))
						{
							property.SetValue(to_object, ddouble, null);
						}

						break;
					case TypeCode.Decimal:
						if (decimal.TryParse(value, out var ddecimal))
						{
							property.SetValue(to_object, ddecimal, null);
						}

						break;
					case TypeCode.Char:
						if (char.TryParse(value, out var dchar))
						{
							property.SetValue(to_object, dchar, null);
						}

						break;
					case TypeCode.String:
						property.SetValue(to_object, value, null);
						break;
					case TypeCode.Boolean:
						if (bool.TryParse(value, out var dbool))
						{
							property.SetValue(to_object, dbool, null);
						}

						break;
					case TypeCode.SByte:
						if (SByte.TryParse(value, out var dsbyte))
						{
							property.SetValue(to_object, dsbyte, null);
						}

						break;
					case TypeCode.Byte:
						if (byte.TryParse(value, out var dbyte))
						{
							property.SetValue(to_object, dbyte, null);
						}

						break;
					case TypeCode.DateTime:
						if (DateTime.TryParse(value, out var d))
						{
							property.SetValue(to_object, d, null);
						}

						break;
					case TypeCode.Empty:
					case TypeCode.DBNull:
						property.SetValue(to_object, "", null);
						break;
					case TypeCode.Object:
						if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
						{
							if (Guid.TryParse(value, out var dguid))
							{
								property.SetValue(to_object, dguid, null);
							}
						}
						if (propertyType == typeof(DateTime?))
						{
							if (DateTime.TryParse(value, out var ndate))
							{
								property.SetValue(to_object, ndate, null);
							}
						}
						if (propertyType == typeof(int?))
						{
							if (int.TryParse(value, out var nint32))
							{
								property.SetValue(to_object, nint32, null);
							}
						}
						break;
					default:
						property.SetValue(to_object, value, null);
						break;
				}
			}

			return to_object;
		}

		public string ConvertDataTableToHTML(DataTable dt)
		{
			var html = "<table>";
			//add header row
			html += "<tr>";
			for (var i = 0; i < dt.Columns.Count; i++)
				html += "<td>" + dt.Columns[i].ColumnName + "</td>";
			html += "</tr>";
			//add rows
			for (var i = 0; i < dt.Rows.Count; i++)
			{
				html += "<tr>";
				for (var j = 0; j < dt.Columns.Count; j++)
					html += "<td>" + dt.Rows[i][j] + "</td>";
				html += "</tr>";
			}
			html += "</table>";
			return html;
		}

		public DateTime ConvertToDateTime(string dt)
		{
			if (string.IsNullOrEmpty(dt))
			{
				return DateTime.Now;
			}

			return DateTime.TryParse(dt, out var ret_dt) ? ret_dt : DateTime.Now;
		}

	}
}
