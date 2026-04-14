using log4net;
using NESI.BLL.Pages.Reports;
using NESI.Common;
using NESI.Common.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NESI.DataProcessor
{
    public class DataProcesserEngine
	{
        public static Response<T> GetProcessedDataFromDatatable<T, T1>(BodyParams param, T1 builder)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			var response = new Response<T>();
			List<T> filteredCollection;

			if (!string.IsNullOrEmpty(param.globalfilter) && param.globalfilter.Length > 0)
			{
				filteredCollection = GetGlobalSearchList<T, T1>(builder, param);
			}
			else
			{
				filteredCollection = GetDataTableList<T, T1>(param, builder);
			}

			response.data = filteredCollection;
			response.TotalCount = builder.GetRecordCountFromDataTable;
			response.ColumnSummary = builder.GetColumnSummary;

			return response;
		}

		private static DataTable ConvertDatetimeToDate(DataTable table)
		{
			using (DataTable dtTemp = table.Copy())
			{
				foreach (DataColumn column in dtTemp.Columns)
				{
					if (column.DataType == typeof(DateTime) || column.DataType == typeof(DateTime?))
					{
						foreach (DataRow dr in dtTemp.Rows)
						{
							if (!string.IsNullOrEmpty(Convert.ToString(dr[column])))
							{
								dr[column] = Convert.ToDateTime(dr[column]).ToString("MM/dd/yyyy");
							}
						}
					}
				}
				return dtTemp;
			}
		}

		private static List<T> GetDataTableList<T, T1>(BodyParams param, T1 builder)
		   where T : class, IModelBase
		   where T1 : IModelGenerator<T>
		{
			List<T> wolist = new List<T>();

			var wo = Activator.CreateInstance<T>();

			var reportSchema = builder.GetSchemaFromStore();
			if (reportSchema == null)
			{
				reportSchema = wo.GetSchema();
				builder.SetSchemaStore(reportSchema);
			}

			var dbResultCache = builder.GetDataFromStore();
			var dbResult = dbResultCache; //.DefaultView.ToTable();
			DataTable filteredTable;


			dbResult.CaseSensitive = false;

			//filter result for filter builder

			if (!string.IsNullOrEmpty(param.filterBuilder))
			{

				var filter = new Filter();
				var filterExpression = filter.ConvertStringToQuery(param.filterBuilder, reportSchema);
				filterExpression = filterExpression + GetOtherFilterExpression(param, reportSchema);

				var defaultView = dbResult.DefaultView;
                try
                {
                    defaultView.RowFilter = filterExpression;
                    filteredTable = defaultView.ToTable();
                }
                catch (Exception ex)
                {
                    string msg = string.Format("Raw filter string = [{0}], translated filter string = [{1}] ", param.filterBuilder, filterExpression);
                    throw new ArgumentException(msg, ex);
                }
            }
			// FILTER THE RESULT
			else if (param.column_filter != null && param.column_filter.Length > 0)
			{
				var filterExpression1 = string.Empty;
				var filterExpression2 = string.Empty;

				IList<Filter> filterComplexDateTime = param.column_filter.Where(x => x.Operation == OpertaionType.complexDateTime).ToList();

				IList<Filter> filterOthers = param.column_filter.Where(x => x.Operation != OpertaionType.complexDateTime).ToList();

				if (filterComplexDateTime.Count > 0)
				{
					int i = 0;

					foreach (Filter fl in filterComplexDateTime)
					{
						fl.filterType = "datetime";

						if (i == 0)
							filterExpression1 = fl.GetExpression();
						else
						{
							var fl5 = fl.GetExpression();
							var tmp = $"({filterExpression1} OR {fl5})";
							filterExpression1 = tmp;
						}
						i++;
					}
				}

				if (filterOthers.Count > 0)
				{
					foreach (var filter in filterOthers)
					{
						if (!string.IsNullOrEmpty(filterExpression2))
						{
							var schemaCol = reportSchema.columns.First(sc => sc.name == filter.coulumnname);

							filter.filterType = schemaCol.rawType;
							filterExpression2 += " AND " + filter.GetExpression();
						}

						else
						{
							var schemaCol = reportSchema.columns.First(sc => sc.name == filter.coulumnname);

							filter.filterType = schemaCol.rawType;
							filterExpression2 = filter.GetExpression();
						}
					}
				}

				string filterFinal = "";
				if (!string.IsNullOrEmpty(filterExpression1) && !string.IsNullOrEmpty(filterExpression2))
				{
					filterFinal = $"({filterExpression1} AND {filterExpression2})";
				}
				else if (string.IsNullOrEmpty(filterExpression1))
					filterFinal = filterExpression2;
				else
					filterFinal = filterExpression1;

				var defaultView = dbResult.DefaultView;
				defaultView.RowFilter = filterFinal;
				filteredTable = defaultView.ToTable();
			}
			else
			{
				filteredTable = dbResult; //.DefaultView.ToTable();
			}

			// dbResult.CaseSensitive = true;
			builder.ProcessExtra(filteredTable);

			if (param.column_groupBy != null && param.column_groupBy.Length > 0)
			{
				//	filteredTable = ConvertDatetimeToDate(filteredTable);

				var summycolumns = reportSchema.columns.Where(p => string.IsNullOrWhiteSpace(p.summary) == false).ToList();

				var groupSummycolumns = reportSchema.columns.Where(p => string.IsNullOrWhiteSpace(p.groupSummary) == false).OrderBy(p => p.groupSummaryOrdinal).ToList();

				if (summycolumns.Count > 0)
				{
					builder.GetColumnSummary = new Dictionary<string, string>();

					foreach (var prop in summycolumns)
					{
						builder.GetColumnSummary.Add(prop.name, filteredTable.Compute($"{prop.summary}({prop.name})", "").ToString());
					}
				}

				return GroupResultSet(param, builder, wolist, dbResult, filteredTable, groupSummycolumns, reportSchema);
			}
			else
			{
				builder.GetRecordCountFromDataTable = filteredTable.Rows.Count;
				filteredTable = SortResult(param, filteredTable);

				var result = filteredTable.AsEnumerable();
				var lstDatarows = result.Skip((param.page_count - 1) * param.page_size).Take(param.page_size).ToList();

				var summycolumns = reportSchema.columns.Where(p => string.IsNullOrWhiteSpace(p.summary) == false).ToList();

				if (summycolumns.Count > 0)
				{
					builder.GetColumnSummary = new Dictionary<string, string>();

					foreach (var prop in summycolumns)
					{
						builder.GetColumnSummary.Add(prop.name, filteredTable.Compute($"{prop.summary}({prop.name})", "").ToString());
					}
				}
				return builder.ConvertToList(lstDatarows, wolist);
			}
		}


		private static string GetOtherFilterExpression(BodyParams param, IReport reportSchema)
		{
			var filter = new Filter();
			var filterExpression = "";
			if (param.otherFilterBuilders != null && param.otherFilterBuilders.Length > 0)
			{
				var i = 0;
				foreach (var otherfilter in param.otherFilterBuilders)
				{
					if (!string.IsNullOrEmpty(otherfilter))
					{
						var otherExpression = filter.ConvertStringToQuery(otherfilter, reportSchema);
						filterExpression = filterExpression + " " + param.otherFilterConditions[i] + " " + otherExpression;
					}
					i++;
				}
			}
			return filterExpression;
		}

		private static DataTable SortResult(BodyParams param, DataTable filteredTable, List<string> sortColumns = null)
		{
			if (param.column_sort != null)
			{

				string sortStr;
				DataView dvSorted = new DataView(filteredTable);
				if (sortColumns == null)
					sortStr = string.Join(",", param.column_sort.Select(s => s.GetExpression()).ToArray());
				else
					sortStr = string.Join(",", param.column_sort.Where(p => sortColumns.Contains(p.ColumnName)).Select(s => s.GetExpression()).ToArray());
				dvSorted.Sort = sortStr;
				filteredTable = dvSorted.ToTable();
			}

			return filteredTable;
		}

		private static List<T> GroupResultSet<T, T1>(BodyParams param, T1 builder, List<T> wolist, DataTable dbResult, DataTable filteredTable, List<Schema> groupSumary, IReport schema)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			List<T> topResult = new List<T>();
			List<string> groups = param.column_groupBy.ToList();
			List<string> expandGroups = new List<string>();

			IList<BLL.Pages.Reports.DataTableAggregateFunction> fieldsForCalculation = new List<BLL.Pages.Reports.DataTableAggregateFunction>();
			int expandCount = 0;
			if (param.expand_by != null)
			{
				if (param.expand_by.Count > 0)
				{
					expandCount = param.expand_by[0].Count();
				}
			}
			int groupCount = 0;
			var filterExpression = "";
			int expandCounter = 0;
			int topSearchIndex = 0;
			List<string> metadata = new List<string>();
			foreach (var item in groups)
			{
				if (expandCount < groupCount++ && groups.Count != 1)
					break;
				expandGroups.Add(item);
				//var groupedList = builder.GetGroupFromStore(item);
				//if (groupedList == null)
				//{
				var groupedList = GroupRecords(wolist, param, expandGroups, filteredTable, fieldsForCalculation, item, builder, schema);
				//	builder.SetGroupStore(item, groupedList);
				//}
				if (groupCount == 1)
				{
					T[] listArray = new T[groupedList.Count];
					groupedList.CopyTo(listArray);
					topResult.AddRange(listArray);
				}
				if (param.expand_by != null && param.expand_by.Count > 0)
				{
					if (groups.Count > 1 && groupCount == 1)
						continue;
					topSearchIndex = 0;
					var expandArr = param.expand_by[0];
					var expItem = expandArr[expandCounter++];
					if (!string.IsNullOrEmpty(filterExpression))
						filterExpression += " AND " + expItem.GetExpression();
					else
						filterExpression = expItem.GetExpression();

					List<T> insertedGroupList = new List<T>();

					if (!(groups.Count == 1 && expandCount == 1))
					{
						for (int index = 0; index < groupedList.Count; index++)
						{
							var expMatch = true;
							expMatch = MatchPropertyValue(groupedList[index], expandCounter, expandArr);
							//Get expansion data.
							if (expMatch)
							{
								insertedGroupList.Add(groupedList[index]);
							}
						}
					}
					topSearchIndex = SearchMatchingIndex(topResult, expandCounter, expandArr);
					metadata = new List<string>();
					if (topSearchIndex < topResult.Count)
					{
						metadata.AddRange(topResult[topSearchIndex].Metadata.ToArray());
						topResult.RemoveAt(topSearchIndex);
					}

					if (insertedGroupList.Count > 0)
					{
						if (topSearchIndex < topResult.Count)
							topResult.InsertRange(topSearchIndex, insertedGroupList);
						else
							topResult.AddRange(insertedGroupList);
					}
				}
			}
			if (expandCount > 0)
			{
				InsertExpandedRows(param, builder, filteredTable, topResult, groupCount, ref filterExpression, ref expandCounter, ref topSearchIndex, metadata);
			}
			builder.GetRecordCountFromDataTable = topResult.Count();
			var p = topResult.Skip((param.page_count - 1) * param.page_size)
				.Take(param.page_size).ToList();

			if (groupSumary != null && groupSumary.Count > 0 && (p != null && p.Count > 0))
			{
				var key = "summary" + param.page_size + "_" + param.page_count;
				var o = builder.GetGroupSummaryFromStore(key);
				if (o == null)
				{
					o = CalculateGroupSummary(filteredTable, groups, groupSumary, p, builder, schema, param, expandGroups);
					builder.SetGroupSummaryStore(key, o);
				}
				builder.GroupSummary = o;
			}

			return p;
		}


		private static List<DataTableAggregateFunction> GetFieldsForCalculation(List<Schema> groupSumary)
		{
			//        Sum,           Avg,       Count,        Max,        Min
			List<DataTableAggregateFunction> fieldsForCalculation = new List<DataTableAggregateFunction> { };
			foreach (var g in groupSumary)
			{
				AggregateFunction fun = AggregateFunction.Sum;
				if (g.groupSummary.ToLower() == AggregateFunction.Sum.ToString().ToLower())
				{
					fun = AggregateFunction.Sum;
				}

				if (g.groupSummary.ToLower() == AggregateFunction.Avg.ToString().ToLower())
				{
					fun = AggregateFunction.Avg;
				}

				if (g.groupSummary.ToLower() == AggregateFunction.Count.ToString().ToLower())
				{
					fun = AggregateFunction.Count;
				}

				if (g.groupSummary.ToLower() == AggregateFunction.Max.ToString().ToLower())
				{
					fun = AggregateFunction.Max;
				}

				if (g.groupSummary.ToLower() == AggregateFunction.Min.ToString().ToLower())
				{
					fun = AggregateFunction.Min;
				}

				fieldsForCalculation.Add(new DataTableAggregateFunction()
				{
					enmFunction = fun,
					ColumnName = g.name,
					OutPutColumnName = $"{g.name}_{fun.ToString()}"
				});
			}

			return fieldsForCalculation;
		}

		private static DataTable CalculateGroupSummary<T, T1>(DataTable filteredTable, List<string> groups, List<Schema> groupSumary, List<T> resultData, T1 builder, IReport schema, BodyParams param, List<string> expandGroups)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			List<DataTableAggregateFunction> fieldsForCalculation = GetFieldsForCalculation(groupSumary);
			if (fieldsForCalculation.Count == 0)
			{
				return null;
			}

			//
			// Group summary calculation for each level of group: ( business_unit), (business_unit, pm )
			//
			List<string> groupByColulmns = new List<string>();
			var finalGroupsummaryTable = default(DataTable);


			for (int i = 0; i < groups.Count; i++)
			{
				// Get current will-be-grouped column.
				string currentColumn = groups[i];

				// Get next level group columns.
				groupByColulmns.Add(currentColumn);

				// Get untouched columns
				List<string> untouchedColumns = new List<string>();
				foreach (var group in groups)
				{
					if (!groupByColulmns.Contains(group))
					{
						untouchedColumns.Add(group);
					}
				}

				var filters = "";

				var item_index = expandGroups.FindIndex(x => x == currentColumn);
				if (param.expand_by != null && param.expand_by.Count > 0)
				{
					foreach (var es in param.expand_by)
					{
						if (es != null && es.Length > 0)
						{
							var count = 0;
							foreach (var e in es)
							{
								if (count < item_index)
								{
									if (e.value != null)
									{
										filters += e.coulumnname + "='" + e.value.ToString().Replace("'", "''") + "' AND ";
									}
									else
									{
										filters += "(" + e.coulumnname + "='' OR " + e.coulumnname + " is null) AND ";
									}
								}
								count++;
							}
						}
					}

				}

				if (!string.IsNullOrEmpty(filters))
				{
					var dataView = filteredTable.DefaultView;
					dataView.RowFilter = filters + "1=1";
					//dataView = dataView.ToTable().DefaultView;
					filteredTable = dataView.ToTable();
				}
				// Calculation based on current group level.


				var groupsummaryTable = builder.GetFilterDataFromStore("shriked_" + currentColumn + "_" + filters);
				if (groupsummaryTable == null)
				{

					////var shrinkedFilteredTable = ShrinkedFilteredTable(dataView, groupByColulmns, resultData, schema);
					groupsummaryTable = filteredTable.GetGroupedBySummary(groupByColulmns, fieldsForCalculation, untouchedColumns, param);
					builder.SetFilterDataStore("shriked_" + filters, groupsummaryTable);
				}

				if (i == 0)
				{
					finalGroupsummaryTable = groupsummaryTable ?? new DataTable();

				}
				else
				{
					for (var j = 0; j < groupsummaryTable.Rows.Count; j++)
					{
						try
						{
							var row = finalGroupsummaryTable.NewRow();
							foreach (var u in finalGroupsummaryTable?.Columns)
							{
								if (finalGroupsummaryTable.Columns.Contains(u.ToString().ToLower()) && groupsummaryTable.Columns.Contains(u.ToString().ToLower()))
								{
									row[u.ToString().ToLower()] = groupsummaryTable.Rows[j][u.ToString().ToLower()];
								}
							}
							finalGroupsummaryTable.Rows.Add(row);

						}
						catch (Exception ex)
						{
							string e = ex.Message;
						}

					}
				}
				//for (var j = 0; j < finalGroupsummaryTable?.Rows.Count; j++)
				//{
				//	if (param.expand_by != null && param.expand_by.Count > 0)
				//	{
				//		foreach (var es in param.expand_by)
				//		{
				//			if (es != null && es.Length > 0)
				//			{
				//				foreach (var e in es)
				//				{
				//					if (!string.IsNullOrEmpty(e.value?.ToString()))
				//					{

				//						if (finalGroupsummaryTable.Columns.Contains(e.coulumnname))
				//						{
				//							finalGroupsummaryTable.Rows[j][e.coulumnname] = e.value;
				//						}
				//					}
				//				}
				//			}
				//		}

				//	}
				//}

			}

			var shrinkedTable = ShrinkDown(groups, "", groups, finalGroupsummaryTable, resultData);

			CalculateDes(shrinkedTable, groupSumary, fieldsForCalculation);

			return shrinkedTable;
		}

		private static DataTable ShrinkedFilteredTable<T>(DataView defaultView, List<string> groupByColulmns, List<T> resultData, IReport schema)
		where T : class, IModelBase
		{
			string filterExpression = "";

			//(|model^equals^1100^ And |manufacturer^equals^Liebert^) Or (|model^equals^3385^ And |manufacturer^equals^AB^)

			for (int i = 0; i < resultData.Count; i++)
			{
				// Begin
				string subFilter = "(";
				bool first = true;
				foreach (var g in groupByColulmns)
				{
					// |model^equals^1100^
					string data = "";
					var row = resultData[i];
					var v = row.GetType().GetProperties().Single(p => p.Name == g).GetValue(row, null);
					if (v == null)
					{
						data = "";
					}
					else
					{
						data = v.ToString();
					}

					string s = "|" + g + "^" + "equals" + "^" + data + "^";
					if (first)
					{
						subFilter = subFilter + s;
						first = false;
					}
					else
					{
						subFilter = subFilter + " And " + s;
					}
				}

				// End
				subFilter = subFilter + ")";

				// Combine all filters
				if (i == 0)
				{
					filterExpression = subFilter;
				}
				else
				{
					filterExpression = filterExpression + " Or " + subFilter;
				}
			}

			// Do the filter job
			var filter = new Filter();
			var filterExpress = filter.ConvertStringToQuery(filterExpression, schema);

			defaultView.RowFilter = filterExpress;
			var filteredTable = defaultView.ToTable();

			return filteredTable;
		}

		private static DataTable ShrinkDown<T>(List<string> groups, string currentgroupColumn, List<string> groupByColulmns, DataTable groupsummaryTable, List<T> resultData)
			 where T : class, IModelBase
		{

			if (resultData == null || resultData.Count == 0)
			{
				return null;
			}

			if (groupsummaryTable == null || groupsummaryTable.Rows.Count == 0)
			{
				return null;
			}

			DataTable shrinkedTable = groupsummaryTable.DefaultView.ToTable();
			shrinkedTable.Rows.Clear();

			foreach (var row in resultData)
			{
				//
				// For current row, get related colulmns value for each of groupByColulmns
				//
				List<object> valuesFromData = new List<object>();
				foreach (var columnName in groupByColulmns)
				{
					var v = row.GetType().GetProperties().Single(p => p.Name == columnName).GetValue(row, null);
					valuesFromData.Add(v);
				}

				// 
				// Find the matched one.
				//
				foreach (var summaryInfo in groupsummaryTable.AsEnumerable())
				{
					List<object> values = new List<object>();
					foreach (var columnName in groupByColulmns)
					{
						if (groupsummaryTable.Columns.Contains(columnName))
						{
							if (summaryInfo[columnName] is DBNull)
							{
								values.Add(null);
							}
							else
							{
								values.Add(summaryInfo[columnName]);
							}
						}
					}

					bool matched = false;
					for (int j = 0; j < valuesFromData.Count; j++)
					{
						if (valuesFromData[j] == values[j])
						{
							matched = true; // minimal match is okay
							break;
						}

						try
						{
							if (valuesFromData[j].ToString() == values[j].ToString())
							{
								matched = true;
								break;
							}
						}
						catch (Exception ex)
						{
							string e = ex.Message;
						}

						try
						{
							if ((string.IsNullOrWhiteSpace((string)valuesFromData[j]) && string.IsNullOrWhiteSpace((string)values[j])))
							{
								matched = true;
								break;
							}
						}
						catch (Exception ex)
						{
							string e = ex.Message;
						}
					}

					if (matched)
					{
						shrinkedTable.Rows.Add(summaryInfo.ItemArray);
					}
					else
					{
						continue;
					}
				}

			}

			return shrinkedTable;
		}

		private static void CalculateDes(DataTable shrinkedTable, List<Schema> groupSumary, List<DataTableAggregateFunction> fieldsForCalculation)
		{
			if (groupSumary == null || groupSumary.Count == 0 || shrinkedTable == null || shrinkedTable.Rows == null || shrinkedTable.Rows.Count == 0)
			{
				return;
			}

			//int total = 0;
			//string format = "";
			//for (int i = 0; i < groupSumary.Count; i++)
			//{
			//    if (i == 0)
			//    {
			//        format = string.Format("{0}: {1}", groupSumary[i].header, "{" + i + "}");
			//    }
			//    else
			//    {
			//        format = format + ", " + string.Format("{0}: {1}", groupSumary[i].header, "{" + i + "}");
			//    }
			//}

			//format = " (" + format + ")";

			for (int i = 0; i < shrinkedTable.Rows.Count; i++)
			{
				// For each row of data
				string des = "";
				var row = shrinkedTable.Rows[i];
				for (int j = 0; j < groupSumary.Count; j++)
				{
					string colum = fieldsForCalculation[j].OutPutColumnName;
					var col = row[colum];
					string data = FormatData(col, groupSumary[j]);

					var subDes = $"{groupSumary[j].header}: {data}";
					if (j == 0)
					{
						des = subDes;
					}
					else
					{
						des = des + ", " + subDes;
					}
				}

				des = " (" + des + ")";
				shrinkedTable.Rows[i]["des"] = des;
			}
		}

		private static string FormatData(object data, Schema schema)
		{
			if (schema.rawType.ToLower() == "currency")
			{
				decimal d = 0;
				if (!decimal.TryParse(data.ToString(), out d))
				{
					d = 0;
				}

				if (d < 0)
				{
					return $"{d:C2}";
				}

				return $"{d:C2}";
			}

			if (schema.rawType.ToLower() == "number")
			{
				decimal d = 0;
				if (!decimal.TryParse(data.ToString(), out d))
				{
					d = 0;
				}

				if (string.IsNullOrWhiteSpace(schema.displayFormat))
				{
					return $"{d:N0}";
				}
				else
				{
					return $"{d:N}";
				}
			}

			return data.ToString();
		}

		private static void InsertExpandedRows<T, T1>(BodyParams param, T1 builder, DataTable dbResult, List<T> topResult, int groupCount, ref string filterExpression, ref int expandCounter, ref int topSearchIndex, List<string> metadata)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			if (expandCounter < param.expand_by[0].Count())
			{
				var expItem = param.expand_by[0][expandCounter++];
				if (!string.IsNullOrEmpty(filterExpression))
					filterExpression += " AND " + expItem.GetExpression();
				else
					filterExpression = expItem.GetExpression();
			}
			if (expandCounter == groupCount)
			{
				if (expandCounter > 1)
				{
					topSearchIndex = SearchMatchingIndex(topResult, expandCounter, param.expand_by[0]);
					if (topSearchIndex < topResult.Count)
						topResult.RemoveAt(topSearchIndex);
				}
				List<T> resultExpanded;
				resultExpanded = GetExpandedRows<T, T1>(param, builder, dbResult, filterExpression, metadata.ToArray());
				if (resultExpanded != null && resultExpanded.Count > 0)
				{
					if (topSearchIndex < topResult.Count)
						topResult.InsertRange(topSearchIndex, resultExpanded);
					else
						topResult.AddRange(resultExpanded);
				}
			}
		}

		private static int SearchMatchingIndex<T>(List<T> topResult, int expandCounter, Filter[] expandArr)
		{
			int topSearchIndex = 0;
			for (int topIndex = 0; topIndex < topResult.Count; topIndex++)
			{
				bool matchTop = MatchPropertyValue(topResult[topIndex], expandCounter, expandArr);
				if (matchTop)
					break;
				topSearchIndex++;
			}

			return topSearchIndex;
		}

		private static bool MatchPropertyValue<T>(T topResult, int expandCounter, Filter[] expandArr)
		{
			var matchTop = true;
			for (int expandIndex = 0; expandIndex < expandCounter; expandIndex++)
			{
				var propertyInfo = topResult.GetType().GetProperty(expandArr[expandIndex].coulumnname);
				var value = expandArr[expandIndex].value;
				var prop = propertyInfo.GetValue(topResult, null);
				var stringValue = Convert.ToString(value);
				var stringProp = Convert.ToString(prop);
				if (value == null && prop == null)
					continue;
				else if ((value == null && prop != null) || (value != null && prop == null))
				{
					matchTop = false;
					break;
				}
				if ((propertyInfo.PropertyType == typeof(DateTime?) || propertyInfo.PropertyType == typeof(DateTime)) && !string.IsNullOrEmpty(Convert.ToString(value)) && !string.IsNullOrEmpty(stringProp))
				{
					if (DateTime.Parse(stringValue) != DateTime.Parse(stringProp))
					{

						matchTop = false;
						break;
					}
				}
				else
				{
					string[] boolMatch = new string[] { "true", "false" };
					if (!stringProp.Equals(stringValue,
						boolMatch.Contains(stringValue.ToLower())
							? StringComparison.OrdinalIgnoreCase
							: StringComparison.InvariantCulture))
					{
						matchTop = false;
						break;
					}
				}
			}
			return matchTop;
		}

		private static List<T> GroupRecords<T, T1>(List<T> wolist, BodyParams param, List<string> expandGroups,
			DataTable filteredTable, IList<DataTableAggregateFunction> fieldsForCalculation,
			string item, T1 builder, IReport schema)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			fieldsForCalculation.Add(new DataTableAggregateFunction()
			{
				enmFunction = AggregateFunction.Count,
				ColumnName = item,
				OutPutColumnName = item + "_Count"
			});

			List<string> sortColumns = fieldsForCalculation.Select(p => p.ColumnName).ToList();

			var filters = "";

			var item_index = expandGroups.FindIndex(x => x == item);
			if (param.expand_by != null && param.expand_by.Count > 0)
			{
				foreach (var es in param.expand_by)
				{
					if (es != null && es.Length > 0)
					{
						var count = 0;
						foreach (var e in es)
						{
							if (count < item_index)
							{
								if (e.value != null)
								{
									filters += e.coulumnname + "='" + e.value.ToString().Replace("'", "''") + "' AND ";
								}
								else
								{
									filters += "(" + e.coulumnname + "='' OR " + e.coulumnname + " is null) AND ";
								}
							}
							count++;
						}
					}
				}

			}
			var filterExpression = "";
			if (!string.IsNullOrEmpty(param.filterBuilder))
			{
				var filter = new Filter();
				filterExpression = filter.ConvertStringToQuery(param.filterBuilder, schema);
				filterExpression = filterExpression + GetOtherFilterExpression(param, schema);
			}

			var groupedData = builder.GetGroupSummaryFromStore(filters + filterExpression);
			if (groupedData == null)
			{
				var dt = string.IsNullOrEmpty(filters) ? filteredTable : (new DataView(filteredTable) { RowFilter = filters + "1=1" }).ToTable();
				groupedData = dt.GetGroupedBy(expandGroups, fieldsForCalculation);
				builder.SetGroupSummaryStore(filters, groupedData);
			}
			groupedData = SortResult(param, groupedData, sortColumns);

			var result = groupedData.AsEnumerable();
			wolist.Clear();
			var groupedList = builder.ConvertToList(result.ToList(), wolist, true);
			return groupedList;
		}

		private static List<T> GetExpandedRows<T, T1>(BodyParams param, T1 builder, DataTable dtResult, string filterExpression, string[] metadata = null)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			List<T> wolist = new List<T>();
			dtResult.DefaultView.RowFilter = "";
			var defaultView = dtResult.DefaultView;
			defaultView.RowFilter = filterExpression;
			DataTable filteredTable = defaultView.ToTable();
			// DataTable filteredTable = resultTable;
			/*hot fix for exact match incase of space in grouping*/
			if (filteredTable != null && filteredTable.Rows.Count > 0)
			{
				if (!string.IsNullOrEmpty(filterExpression))
				{
					if (param.expand_by != null && param.expand_by.Count > 0)
					{
						var item = param.expand_by[0][param.expand_by[0].Length - 1];
						string value = Convert.ToString(item.value);
						DateTime dtTmp;
						if (!DateTime.TryParse(value, out dtTmp))
						{
							string[] type = new string[] { "true", "false" };
							if (item.Operation == OpertaionType.equals && !type.Contains(value.ToLower()))
							{
								int length = filteredTable.Rows.Count - 1;
								while (length >= 0)
								{
									DataRow dataRow = filteredTable.Rows[length--];
									if (Convert.ToString(dataRow[item.coulumnname]) != value)
										filteredTable.Rows.Remove(dataRow);
								}
							}
						}
					}
				}
			}

			filteredTable = SortResult(param, filteredTable);
			return builder.ConvertToList(filteredTable.AsEnumerable().ToList(), wolist, false, metadata);
		}

		private static List<T> GetGlobalSearchList<T, T1>(T1 builder, BodyParams param)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			string likeStatement = " Like '%{0}%'";
			DataTable dtResult = builder.GetDataFromStore();
			dtResult = SortResult(param, dtResult);
			dtResult.CaseSensitive = false;
			StringBuilder query = new StringBuilder();
			string filterString;
			List<T> wolist = new List<T>();

			//            int colCount = dtResult.Columns.Count;
			int colCount = builder.SetColumnList.Count;

			try
			{
				for (int i = 0; i < colCount; i++)
				{
					string colName = builder.SetColumnList[i];
					query.Append(string.Concat("Convert(", colName, ", 'System.String')", likeStatement));


					if (i != colCount - 1)
						query.Append(" OR ");
				}

				filterString = Convert.ToString(query);

				string currFilter = string.Format(filterString, param.globalfilter);

				DataView dv = dtResult.DefaultView;
				dv.RowFilter = "";
				dv.RowFilter = currFilter;
				DataTable _dtTmp = dv.ToTable();
				DataTable filteredTable;

				// If there is any column filter search then apply that on group filter data.

				if (param.column_filter != null && param.column_filter.Length > 0)
				{
					string filterExpression = string.Empty;
					foreach (var filter in param.column_filter)
					{
						if (!string.IsNullOrEmpty(filterExpression))
							filterExpression += " AND " + filter.GetExpression();
						else
							filterExpression = filter.GetExpression();

					}


					var defaultView = _dtTmp.DefaultView;
					defaultView.RowFilter = filterExpression;
					filteredTable = defaultView.ToTable();

					builder.GetRecordCountFromDataTable = filteredTable.Rows.Count;
					builder.ProcessExtra(filteredTable);

					var lstDataRows = builder.ConvertToList(filteredTable.AsEnumerable().ToList(), wolist);
					var retRows = lstDataRows.AsEnumerable().Skip((param.page_count - 1) * param.page_size).Take(param.page_size).ToList();

					return retRows;
				}
				else
				{
					builder.GetRecordCountFromDataTable = _dtTmp.Rows.Count;
					builder.ProcessExtra(_dtTmp);

					var lstDataRows = builder.ConvertToList(_dtTmp.AsEnumerable().ToList(), wolist);
					var retRows = lstDataRows.AsEnumerable().Skip((param.page_count - 1) * param.page_size).Take(param.page_size).ToList();

					return retRows;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return wolist;
			}
			finally
			{
				dtResult.DefaultView.RowFilter = "";
			}
		}


		public static List<string> GetDistinct<T, T1>(T1 builder, BodyParams param)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			List<T> filteredCollection;

			if (!string.IsNullOrEmpty(param.globalfilter) && param.globalfilter.Length > 0)
			{
				filteredCollection = GetGlobalSearchList<T, T1>(builder, param);
			}
			else
			{
				filteredCollection = GetDataTableList<T, T1>(param, builder);
			}
			var colName = param.Selectby[0];
			var lstDatarows = filteredCollection.Select(colName).Distinct();
			var wo = Activator.CreateInstance<T>();

			var reportSchema = wo.GetSchema();

			var schemaCol = reportSchema.columns.First(sc => sc.name == colName);

			var strDistinct = new List<string>();
			var count = 0;
			foreach (var row in lstDatarows)
			{

				var r = Convert.ToString(row).Trim();
				if (schemaCol.rawType.ToLower() == "boolean")
				{
					strDistinct.Add(r == "1" ? "Checked" : "Unchecked");
				}
				else if (!string.IsNullOrEmpty(r))
				{
					strDistinct.Add(r);
				}
				count++;
				if (count > 2500)
				{
					strDistinct = strDistinct.OrderBy(x => x).ToList();

					return strDistinct;
				}
			}

			strDistinct = strDistinct.OrderBy(x => x).ToList();
			return strDistinct;
		}

		public static List<string> GetDistinctFromAll<T, T1>(T1 builder, BodyParams param)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			DataTable dbResult;
			List<T> wolist = new List<T>();

			dbResult = builder.GetDataFromStore();
			var colName = param.Selectby[0];

			var wo = Activator.CreateInstance<T>();

			var reportSchema = wo.GetSchema();

			var schemaCol = reportSchema.columns.Where(sc => sc.name == colName).First();


			DataView view = new DataView(dbResult);
			DataTable distinctValues = view.ToTable(true, colName);

			var result = distinctValues.AsEnumerable();
			var lstDatarows = result.Skip((param.page_count - 1) * param.page_size).Take(param.page_size).ToList();

			List<string> strDistinct = new List<string>();
			foreach (DataRow row in lstDatarows)
			{
				if (row[0] != DBNull.Value)
				{
					if (schemaCol.rawType.ToLower() == "boolean")
					{
						strDistinct.Add((Convert.ToString(row[0]).Trim() == "1" ? "Checked" : "Unchecked"));
					}
					else
						strDistinct.Add(Convert.ToString(row[0]).Trim());
				}
			}

			return strDistinct;
		}

		public static Response<T> GetProcessedData<T, T1>(BodyParams param, T1 builder)
		where T : class, IModelBase,new()
			where T1 : IModelGenerator<T>

		{

			var response = new Response<T>();
			List<T> filteredCollection = new List<T>(); ;
			Expression<Func<T, bool>> match = null;
			//if (param.expand != null && param.expand.Length > 0)
			//{
			//    var newparam=(QueryParam)param.Clone();
			//    var lst = param.column_filter.ToList();
			//  var toggle=  param.expand.Select(p => new Filter() { coulumnname = p.coulumnname, value = p.value, Operation = OpertaionType.notEquals });
			//    lst.AddRange(toggle);
			//    newparam.column_filter = lst.ToArray();
			//    match = GetFilterQuery<T>(newparam);
			//}
			//else
			match = GetFilterQuery<T>(param);
			IQueryable<T> query;
			if (param.globalfilter != null && param.globalfilter.Length > 0)
			{
				query = builder.GlobalSearch(param.globalfilter, match);
			}
			else
				query = builder.GetQueryable(match);

			// query = query.ToList().AsQueryable();

			if ((param.column_groupBy == null || param.column_groupBy.Length == 0) && (param.summary_by == null || param.summary_by.Length == 0))
			{
				response.TotalCount = query.Count();

				var pagedquery = query.ApplySortingPaging<T>(param.column_sort, param.page_count, param.page_size);
				filteredCollection = pagedquery.ToList();
			}
			else
			{
                if (param.column_groupBy != null && param.column_groupBy.Length > 0)
				{
					int count = 0;
					IQueryable groupedData = GetGroupbyQuery(param, query, out count);
                    
					response.TotalCount = count;

					foreach (var item in groupedData)
					{
                        filteredCollection.Add(ObjectHelper.Cast<T>(item));
					}

					filteredCollection = ProcessSummary(filteredCollection, param);

					if (param.expand_by != null && param.expand_by.Count > 0)
					{
						GetExpandedRows(param, builder, response, ref filteredCollection, ref match, ref query);

					}
				}
				else
				{
					response.data = PatchCodeToBeOptimized(param, query);
					return response;
				}

			}

			response.data = filteredCollection;

			return response;



		}

		private static void GetExpandedRows<T, T1>(BodyParams param, T1 builder, Response<T> response, ref List<T> filteredCollection, ref Expression<Func<T, bool>> match, ref IQueryable<T> query)
			where T : class, IModelBase
			where T1 : IModelGenerator<T>
		{
			var tobeskipped = param.page_size * (param.page_count - 1);
			var lst = param.column_filter.ToList();
			int totalExpandedCountToPage = 0;
			int calctobeskipped = 0;
			foreach (var item in param.expand_by)
			{
				lst.AddRange(item);
				param.column_filter = lst.ToArray();


				match = GetFilterQuery<T>(param);


				var groupeditem = filteredCollection.AsEnumerable().AsQueryable().Where(match).FirstOrDefault();


				var index = filteredCollection.IndexOf(groupeditem);
				if (groupeditem != null)
				{
					query = builder.GetQueryable(match);
					response.TotalCount += query.Count() - 1;



					filteredCollection.Remove(groupeditem);



					calctobeskipped = tobeskipped == 0 ? 0 : tobeskipped - (index + 1 + totalExpandedCountToPage);
					var pagedquery = query.ApplySortingPaging<T>(param.column_sort).Skip(calctobeskipped).Take(param.page_size);
					var data = pagedquery.ToList();


					totalExpandedCountToPage += groupeditem.Count;



					data.ForEach(p =>
					{
						var child = p;

						child.Metadata = groupeditem.Metadata;
						child.Count = groupeditem.Count;
					});
					filteredCollection.InsertRange(index, data);
				}
				if (index + totalExpandedCountToPage >= param.page_size)
				{
					break;
				}
			}

			param.column_sort = param.column_groupBy.Select(p => new SortOrder(p)).ToArray();

			filteredCollection = filteredCollection.AsQueryable().Skip(param.page_size * (param.page_count - 1) - (calctobeskipped > 0 ? calctobeskipped - 1 : 0)).Take(param.page_size).ToList();
		}

		private static List<T> ProcessSummary<T>(List<T> filteredCollection, BodyParams param) where T : class, IModelBase
		{
			var objT = Activator.CreateInstance<T>();
			var rep = objT.GetSchema();

			var summycolumns = rep.columns.Where(p => string.IsNullOrWhiteSpace(p.summary) == false).Select(p => typeof(T).GetProperty(p.name)).ToList();

			if (summycolumns.Count > 0)
			{
				SetSummaryOfGroupsInMeta(ref filteredCollection, summycolumns);
				SetSummaryIfParentGroupsInMeta(ref filteredCollection, param, summycolumns);

			}

			return filteredCollection;
		}

		private static void SetSummaryIfParentGroupsInMeta<T>(ref List<T> filteredCollection, BodyParams param, List<PropertyInfo> summycolumns) where T : class, IModelBase
		{
			var meta = typeof(T).GetProperty("Metadata");
			for (int i = 1; i < param.column_groupBy.Length; i++)
			{
				var arr = param.column_groupBy.ToArray().Take(param.column_groupBy.Length - i).ToList();
				var groups = filteredCollection.AsQueryable().GroupByMany(arr.ToArray());
				foreach (var item in groups)
				{
					string sum = string.Empty;
					foreach (var prop in summycolumns)
					{

						sum +=
							$"{item.Items.Select(p => Convert.ChangeType(Convert.ToString(prop.GetValue(p)), prop.PropertyType)).Select(p => Convert.ToInt32(p)).ToList().Sum()},";
					}
					foreach (var item1 in item.Items)
					{
						var lst = (List<string>)meta.GetValue(item1);
						lst.Add(sum.Trim(','));

					}
				}
			}
		}

		private static PropertyInfo SetSummaryOfGroupsInMeta<T>(ref List<T> filteredCollection, List<PropertyInfo> summycolumns) where T : class, IModelBase
		{
			var meta = typeof(T).GetProperty("Metadata");
			var count = typeof(T).GetProperty("Count");
			foreach (T item in filteredCollection)
			{

				string sum = string.Empty;
				foreach (var prop in summycolumns)
				{
					if (summycolumns.IndexOf(prop) == 0)
						count.SetValue(item, Convert.ToInt32(prop.GetValue(item)));


					sum += $"{prop.GetValue(item)},";
				}

				meta.SetValue(item, new List<string> { sum.Trim(',') });

			}

			return meta;
		}

		private static List<T1> PatchCodeToBeOptimized<T1>(BodyParams param, IQueryable<T1> query) 
		    where T1 : class,new()
		{
			List<T1> lst = new List<T1>();
			var dictionary = new Dictionary<string, object>();// (IDictionary<string, object>)obj;
			foreach (var item in param.summary_by)
			{
				//System.Dynamic.ExpandoObject obj = new System.Dynamic.ExpandoObject();
				//dynamic a= new System.Dynamic.ExpandoObject();
				var arr = item.Split(':');





				if (arr[1].Equals("Count", StringComparison.OrdinalIgnoreCase))
				{
					dictionary.Add(arr[0], query.Count());
				}
				if (arr[1].Equals("Sum", StringComparison.OrdinalIgnoreCase))
				{
					dictionary.Add(arr[0], query.Sum(arr[0]));
				}
				//if (arr[1].Equals("Average", StringComparison.OrdinalIgnoreCase))
				//{
				//    dictionary.Add(arr[0], query.Average(arr[0]));
				//}


			}
			lst.Add(ObjectHelper.Cast<T1>(dictionary));


			return lst;
		}

		public static List<string> GetDistinct<T>(BodyParams param, IQueryable<T> query)
	  where T : class, IModelBase

		{

			List<string> filteredCollection = new List<string>(); ;
			query = GetFilterQuery<T>(param, query);

			param.column_groupBy = param.Selectby.Take(1).ToArray();
			int count = 0;
			IQueryable groupedData = GetGroupbyQuery(param, query, out count);


			var column = param.column_groupBy.FirstOrDefault();
			foreach (dynamic item in groupedData)
			{
				filteredCollection.Add(Convert.ToString(item.GetType().GetProperty(column).GetValue(item, null)));
			}



			return filteredCollection;
		}

		private static IQueryable GetGroupbyQuery<T1>(BodyParams param, IQueryable<T1> query, out int count) where T1 : class
		{
			string summryexpression = string.Empty;
			if (param.summary_by != null && param.summary_by.Length > 0)
			{
				summryexpression = "," + string.Join(",", param.summary_by.Select(p =>
									   $"{p.Split(':')[1]}({(p.Split(':')[1].Equals("Count", StringComparison.OrdinalIgnoreCase) ? null : p.Split(':')[0])}) as {p.Split(':')[0]}"));
			}

			IQueryable groupedData = null;
			string keys = null;
			if (param.column_groupBy != null && param.column_groupBy.Length > 0)
			{
				var bareKeys = string.Join(",", param.column_groupBy);
				var orderedKeys = param.column_groupBy.Select(p => p + (param.column_sort.Any(x => x.ColumnName == p && x.ColumnOrder == SortOrder.Order.Descending) ? " descending" : string.Empty));

				var bareOrderedKeys = string.Join(",", orderedKeys);
				var keyColl = param.column_groupBy.Select(p => "Key." + p);
				keys = string.Join(",", keyColl);
				groupedData = query.GroupBy($"new({bareKeys})", "it");
				count = groupedData.Count();
				groupedData = groupedData.Select($"new({keys}{summryexpression})").OrderBy(bareOrderedKeys);
				if (param.expand_by != null && param.expand_by.Count > 0)
				{
					return DataProcesserEngine.ApplyPaging(groupedData, param.page_count * param.page_size);
				}
				else
				{

					var dataQuery = DataProcesserEngine.ApplyPaging(groupedData, param.page_count, param.page_size);
					return dataQuery;
				}
			}
			else
			{
				groupedData = query.GroupBy(x => 1);
				groupedData = groupedData.Select($"new({summryexpression.Trim(',')})");
				count = groupedData.Count();
				return groupedData;
			}




		}

		private static IQueryable<T> GetFilterQuery<T>(BodyParams param, IQueryable<T> query)

			where T : class, IModelBase
		{



			if (param.column_filter != null && param.column_filter.Length > 0)
			{
				var objT = Activator.CreateInstance<T>();
				var rep = objT.GetSchema();

				var filters = rep.columns.Where(p => p.type.Equals("System.String")).Select(p => p.name);
				var list = param.column_filter.Where(p => filters.Contains(p.coulumnname)).Select(p => new Filter() { coulumnname = p.coulumnname, Operation = OpertaionType.notEquals }).ToList();
				list.AddRange(param.column_filter);

				var deleg = ExpressionBuilder.GetExpression<T>(list.ToArray()).Compile();


				query = query.Where<T>(deleg).AsQueryable();
			}

			return query;
		}

		private static Expression<Func<T, bool>> GetFilterQuery<T>(BodyParams param)

		 where T : class, IModelBase
		{



			if (param.column_filter != null && param.column_filter.Length > 0)
			{
				var objT = Activator.CreateInstance<T>();
				var rep = objT.GetSchema();

				var filters = rep.columns.Where(p => p.type.Equals("System.String")).Select(p => p.name);
				var list = param.column_filter.Where(p => filters.Contains(p.coulumnname)).Select(p => new Filter() { coulumnname = p.coulumnname, Operation = OpertaionType.notEquals }).ToList();
				list.AddRange(param.column_filter);

				var deleg = ExpressionBuilder.GetExpression<T>(list);


				return deleg;
			}
			return null;

			//return query;
		}


		private static IQueryable<T> ApplyGlobalFilterIfAny<T>(BodyParams param, IQueryable<T> query)

		  where T : class, IModelBase
		{



			if (param.globalfilter != null && param.globalfilter.Length > 0)
			{
				var objT = Activator.CreateInstance<T>();
				var rep = objT.GetSchema();

				var filters = rep.columns.Where(p => p.type.Equals("System.String")).Select(p => new Filter() { coulumnname = p.name, value = param.globalfilter, Operation = OpertaionType.contains }).ToList();
				var lst = filters.Select(p => new Filter() { coulumnname = p.coulumnname, Operation = OpertaionType.notEquals }).ToList();
				lst.AddRange(filters);
				var deleg = ExpressionBuilder.GetExpression<T>(lst.OrderBy(p => p.coulumnname).ToList(), true).Compile();


				query = query.Where<T>(deleg).AsQueryable();
			}

			return query;
		}

		private static IQueryable ApplyPaging(IQueryable query, int pageNumber, int numberRecords)
		{
			return query.Skip((pageNumber - 1) *
		numberRecords).Take(numberRecords);
		}

		private static IQueryable ApplyPaging(IQueryable query, int take)
		{
			return query.Take(take);
		}

		//private static string GetParameter(HttpRequestMessage Request, string query)
		//{
		//    return Request.GetQueryNameValuePairs().Where(nv => nv.Key == query).Select(nv => nv.Value).FirstOrDefault();
		//}

		public static IReport ExtractSchema<T>(T custbl) where T : IModelSchemaGenerator
		{
			string schema;
			;
			var lsSchema = custbl.GetSchema();

			return lsSchema;
		}

	}


}
