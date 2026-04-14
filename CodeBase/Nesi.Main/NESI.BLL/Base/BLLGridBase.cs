using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.Common.Interface;
using Ninject.Infrastructure.Language;
using WebGrease.Css.Extensions;

namespace NESI.BLL.Base
{
	public class BLLGridBase<T> : BLLBase, IModelGenerator<T> where T : class, IModelBase, new()
	{
		protected readonly Common.Cache.DatatableCacher cacher = Common.Cache.Global.Datatable;
		protected Common.Cache.IReportCacher schema_cacher = Common.Cache.Global.IReport;
		protected Common.Cache.MemoryCacher<List<T>> group_cacher = new Common.Cache.MemoryCacher<List<T>>();

		public List<string> SetColumnList { get; set; }
		public Dictionary<string, string> GetColumnSummary { get; set; }
		public int GetRecordCountFromDataTable { get; set; }
		public object GroupSummary { get; set; }
		public object CaculationsOnAllColumns { get; set; }

		protected readonly BodyParams param;
		protected string query = "";
		protected object[] query_params;


		protected bool donotcache = false;
		public virtual object ProcessExtra(DataTable table)
		{
			return null;
		}


		protected string memCacheKey => typeof(T) + CurrentUser.Guid + CurrentUser.Id + "_" + query_params_string();


		protected string query_params_string()
		{
			var r = "";
			if (query_params != null && query_params.Length > 0)
			{
				foreach (var q in query_params)
				{
					if (q != null)
					{
						r += q.ToString();
					}
				}
			}
			r += bu_ids_string;
			return r;
		}
		public string bu_ids_string
		{
			get
			{
				if (string.IsNullOrEmpty(param?.bu_ids) || param.bu_ids.Length <= 3)
				{
					return CurrentUser.BusinessUnitId.ToString();
				}
				else
				{
					var list = param.bu_ids.Substring(3).Split(',');
					return string.Join(",", list.Where(bu => CurrentUser.VisibleBusinessUnits.Contains(bu)).ToArray());
				}
			}
		}

		public string bu_ids_type
		{
			get
			{
				if (string.IsNullOrEmpty(param?.bu_ids))
				{
					return "[N]";
				}
				else
				{
					if (param.bu_ids.StartsWith("[") && param.bu_ids.Length > 3)
					{
						return param.bu_ids.Substring(0, 3);
					}
					else
					{
						return param.bu_ids;
					}
				}
			}
		}

		class ExtraData
		{
			public object GroupSummary { get; set; }

			public object CaculationsOnAllColumns { get; set; }
		}

		public virtual object GetProcessExtra()
		{
			var gs = new ExtraData
			{
				GroupSummary = this.GroupSummary,
				CaculationsOnAllColumns = this.CaculationsOnAllColumns
			};

			return gs;
		}

		public string sortbyString()
		{
			var sortby = "";
			this.param.column_sort.ForEach(x => { sortby += x.ToString(); });
			return sortby;
		}

		public DataTable GetFilterDataFromStore(string item)
		{
			// return null;

			return cacher.GetValue(memCacheKey + "_filterData" + item);
		}

		public void SetFilterDataStore(string item, DataTable cache)
		{
			cacher.Set(memCacheKey + "_filterData" + item, cache);
		}

		public DataTable GetGroupSummaryFromStore(string item)
		{
			return cacher.GetValue(GetGroupKey(item));
		}

		public void SetGroupSummaryStore(string item, DataTable cache)
		{
			cacher.Set(GetGroupKey(item), cache);
		}






		public BLLGridBase()
		{

		}

		public BLLGridBase(Employee user) : base(user)
		{

		}


		public BLLGridBase(Employee user, BodyParams param) : base(user)
		{
			this.param = param;
			checkParam();
		}

		public BLLGridBase(Employee user, BodyParams param, object[] queyrParams) : base(user)
		{
			this.param = param;
			query_params = queyrParams;
			if (checkParam())
			{
				ClearCache();
			};
		}

		private bool checkParam()
		{
			return (donotcache || (param?.refreshCache != null && param.refreshCache.Value));
		}

		public void ClearCache(object[] ps = null)
		{
			if (ps != null)
			{
				query_params = ps;
			}
			cacher.Clear(memCacheKey);
			schema_cacher.Clear(memCacheKey);
			group_cacher.Clear(memCacheKey);
		}

		public IReport GetSchemaFromStore()
		{
			return donotcache ? null : schema_cacher.GetValue(memCacheKey);
		}


		protected string GetGroupKey(string item)
		{
			var subkey = "_Group_";
			if (param.column_groupBy != null && param.column_groupBy.Length > 0)
			{
				subkey = subkey + param.column_groupBy[0];
			}
			if (param.expand_by != null && param.expand_by.Count > 0)
			{
				foreach (var es in param.expand_by)
				{
					if (es != null && es.Length > 0)
					{
						foreach (var e in es)
						{

							subkey += e.coulumnname + "_" + e.value;

						}
					}
				}

			}
			if (param.column_sort != null && param.column_sort.Length > 0)
			{
				foreach (var s in param.column_sort)
				{
					subkey += s.ToString();
				}
			}
			return memCacheKey + subkey + param.filterBuilder + "_" + item;
		}

		public List<T> GetGroupFromStore(string item)
		{
			return donotcache ? null : group_cacher.GetValue(GetGroupKey(item));
		}

		public void SetGroupStore(string item, List<T> cache)
		{
			group_cacher.Set(GetGroupKey(item), cache);
		}

		public void SetSchemaStore(IReport cache)
		{
			schema_cacher.Set(memCacheKey, cache);
		}

		public List<string> GetDistinct(BodyParams _param)
		{
			var res = GetDataFromStore();
			var colName = _param.Selectby[0];
			return (res.Rows.Cast<DataRow>().Select(row => row[colName].ToString())).Distinct().ToList();
		}


		public List<T> ConvertToList(List<DataRow> dtRows, List<T> wolist, bool addGroupbyMeta = false,
			string[] metadata = null)
		{
			foreach (var row in dtRows)
			{
				var obj = new T();
				var t = obj.GetType();
				var properties = t.GetProperties();
				foreach (var property in properties)
				{
					var columnName = property.Name.ToLower();
					var value = row.Table.Columns.Contains(columnName) ? row[columnName].ToString() : "";

					var propertyType = property.PropertyType;
					var typeCode = Type.GetTypeCode(propertyType);
					switch (typeCode)
					{
						case TypeCode.Int16:
							if (short.TryParse(value, out var dint16))
							{
								property.SetValue(obj, dint16, null);
							}
							break;
						case TypeCode.UInt16:
							if (ushort.TryParse(value, out var duint16))
							{
								property.SetValue(obj, duint16, null);
							}
							break;
						case TypeCode.Int32:
							if (int.TryParse(value, out var dint32))
							{
								property.SetValue(obj, dint32, null);
							}
							break;
						case TypeCode.UInt32:
							if (uint.TryParse(value, out var duint32))
							{
								property.SetValue(obj, duint32, null);
							}
							break;
						case TypeCode.UInt64:
							if (UInt64.TryParse(value, out var duint64))
							{
								property.SetValue(obj, duint64, null);
							}
							break;
						case TypeCode.Int64:
							if (long.TryParse(value, out var dint64))
							{
								property.SetValue(obj, dint64, null);
							}
							break;
						case TypeCode.Single:
							if (float.TryParse(value, out var dsingle))
							{
								property.SetValue(obj, dsingle, null);
							}

							break;
						case TypeCode.Double:
							if (double.TryParse(value, out var ddouble))
							{
								property.SetValue(obj, ddouble, null);
							}

							break;
						case TypeCode.Decimal:
							if (decimal.TryParse(value, out var ddecimal))
							{
								property.SetValue(obj, ddecimal, null);
							}

							break;
						case TypeCode.Char:
							if (char.TryParse(value, out var dchar))
							{
								property.SetValue(obj, dchar, null);
							}

							break;
						case TypeCode.String:
							property.SetValue(obj, value, null);
							break;
						case TypeCode.Boolean:
							if (value == "0")
							{
								value = "False";
							}

							if (value == "1")
							{
								value = "True";
							}

							if (bool.TryParse(value, out var dbool))
							{
								property.SetValue(obj, dbool, null);
							}


							break;
						case TypeCode.SByte:
							if (SByte.TryParse(value, out var dsbyte))
							{
								property.SetValue(obj, dsbyte, null);
							}
							break;
						case TypeCode.Byte:
							if (byte.TryParse(value, out var dbyte))
							{
								property.SetValue(obj, dbyte, null);
							}
							break;
						case TypeCode.DateTime:
							if (DateTime.TryParse(value, out var d))
							{
								property.SetValue(obj, d, null);
							}
							break;
						case TypeCode.Empty:
						case TypeCode.DBNull:
							property.SetValue(obj, "", null);
							break;
						case TypeCode.Object:
							if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
							{
								if (Guid.TryParse(value, out var dguid))
								{
									property.SetValue(obj, dguid, null);
								}
							}

							if (propertyType == typeof(DateTime?))
							{
								if (DateTime.TryParse(value, out var d1))
								{
									property.SetValue(obj, d1, null);
								}
							}
							if (propertyType == typeof(int?))
							{
								if (int.TryParse(value, out var intd1))
								{
									property.SetValue(obj, intd1, null);
								}
								else
								{
									property.SetValue(obj, null, null);
								}
							}
							if (propertyType == typeof(double?))
							{
								if (double.TryParse(value, out var doubled))
								{
									property.SetValue(obj, doubled, null);
								}
								else
								{
									property.SetValue(obj, null, null);
								}
							}
							if (propertyType == typeof(bool?))
							{
								if (bool.TryParse(value, out var boold))
								{
									property.SetValue(obj, boold, null);
								}
								else
								{
									property.SetValue(obj, false, null);
								}
							}
							break;
						default:
							property.SetValue(obj, value, null);
							break;
					}
				}

				if (metadata == null)
				{
					if (param.column_groupBy != null && param.column_groupBy.Length > 0 && addGroupbyMeta)
					{
						obj.Metadata = new List<string>();
						for (var i = param.column_groupBy.Length; i < dtRows[0].Table.Columns.Count; i++)
						{
							obj.Metadata.Add(Convert.ToString(row[i]));
						}
					}
				}
				else
				{
					obj.Metadata = new List<string>();
					obj.Metadata.AddRange(metadata);
				}
				wolist.Add(obj);
			}
			return wolist;
		}

		public DataTable GetDatafromSource()
		{
			if (bu_ids_type == "[S]")
			{
				query = query.Replace("{bu_ids}", bu_ids_string);
			}
			var dt = ExtraFilterDataTable(bllToolbox.doSQL_dt(query, query_params));
			cacher.Delete(memCacheKey);
			cacher.Set(memCacheKey, dt);
			return dt;
		}

		public virtual DataTable ExtraFilterDataTable(DataTable dt)
		{
			return dt;
		}


		public DataTable GetDataFromStore()
		{
			if (checkParam())
			{
				return GetDatafromSource();
			}
			return cacher.GetValue(memCacheKey) ?? GetDatafromSource();
		}

		public IQueryable<T> GetQueryable(Expression<Func<T, bool>> match)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> GlobalSearch(string searchText, Expression<Func<T, bool>> match)
		{
			throw new NotImplementedException();
		}

	}
}