using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NESI.Common;

namespace NESI.BLL.Pages.Reports
{

	/// <summary>
	/// The functions which can be used for aggreation
	/// </summary>
	public enum AggregateFunction
	{
		Sum,
		Avg,
		Count,
		Max,
		Min
	}

	/// <summary>
	/// The class which will have properties of function to be performed and on which field
	/// </summary>
	public class DataTableAggregateFunction
	{
		/// <summary>
		/// The function to be performed
		/// </summary>
		public AggregateFunction enmFunction { get; set; }

		/// <summary>
		/// Performed for which column
		/// </summary>
		public string ColumnName { get; set; }

		/// <summary>
		/// What should be the name after output
		/// </summary>
		public string OutPutColumnName { get; set; }
	}

	public static class DataTableGrouping
	{
		//public static DataTable GetGroupedBy(this DataTable dt, string columnNamesInDt, string groupByColumnNames, string typeOfCalculation, string calculationColumn)
		//{

		//    //Add columns which you want to group by
		//    IList<string> _groupByColumnNames = new List<string>();
		//    _groupByColumnNames.Add("State");
		//    _groupByColumnNames.Add("City");

		//    //Functions you want to perform on which fields
		//    IList<DataTableAggregateFunction> _fieldsForCalculation = new List<DataTableAggregateFunction>();
		//    _fieldsForCalculation.Add(new DataTableAggregateFunction() { enmFunction = AggregateFunction.Avg, ColumnName = "Population", OutPutColumnName = "PopulationAvg" });
		//    _fieldsForCalculation.Add(new DataTableAggregateFunction() { enmFunction = AggregateFunction.Sum, ColumnName = "Population", OutPutColumnName = "PopulationSum" });
		//    _fieldsForCalculation.Add(new DataTableAggregateFunction() { enmFunction = AggregateFunction.Count, ColumnName = "Population", OutPutColumnName = "PopulationCount" });
		//    _fieldsForCalculation.Add(new DataTableAggregateFunction() { enmFunction = AggregateFunction.Max, ColumnName = "Year", OutPutColumnName = "YearMax" });
		//    _fieldsForCalculation.Add(new DataTableAggregateFunction() { enmFunction = AggregateFunction.Min, ColumnName = "Year", OutPutColumnName = "YearMin" });

		//    //Return its own if the column names are empty
		//    if (columnNamesInDt == string.Empty || groupByColumnNames == string.Empty)
		//    {
		//        return dt;
		//    }
		//    string[] multipleGroup = groupByColumnNames.Split(',');            
		//    //Once the columns are added find the distinct rows and group it bu the numbet
		//    DataTable _dt = dt.DefaultView.ToTable(true, multipleGroup);

		//    //The column names in data table
		//    string[] _columnNamesInDt = columnNamesInDt.Split(',');

		//    for (int i = 0; i < _columnNamesInDt.Length; i = i + 1)
		//    {
		//        if (!multipleGroup.Contains(_columnNamesInDt[i]))
		//        {
		//            _dt.Columns.Add(_columnNamesInDt[i]);
		//        }
		//    }


		//    //Gets the collection and send it back
		//    for (int i = 0; i < _dt.Rows.Count; i = i + 1)
		//    {
		//        for (int j = 0; j < _columnNamesInDt.Length; j = j + 1)
		//        {
		//            if (!multipleGroup.Contains(_columnNamesInDt[j]))
		//            {
		//                //_dt.Rows[i][j] = dt.Compute(typeOfCalculation + "(" + calculationColumn + ")", groupByColumnNames + " = '" + _dt.Rows[i][groupByColumnNames].ToString() + "'");
		//                if (typeOfCalculation == "Count")
		//                    _dt.Rows[i][j] = dt.AsEnumerable().Count(p =>
		//                        Convert.ToString(p[groupByColumnNames])
		//                            .Equals(Convert.ToString(_dt.Rows[i][groupByColumnNames]), StringComparison.OrdinalIgnoreCase));
		//            }
		//        }
		//    }

		//    return _dt;
		//}

		/// <summary>
		/// Group by DataTable
		/// </summary>
		/// <param name="_dtSource"></param>
		/// <param name="_groupByColumnNames"></param>
		/// <param name="_fieldsForCalculation"></param>
		/// <returns></returns>
		public static DataTable GetGroupedBy(this DataTable _dtSource, IList<string> _groupByColumnNames, IList<DataTableAggregateFunction> _fieldsForCalculation)
		{
			string sortColumnsName = "";
			//Once the columns are added find the distinct rows and group it bu the numbet
			_dtSource.DefaultView.RowFilter = "";
			DataTable _dtReturn = _dtSource.DefaultView.ToTable(true, _groupByColumnNames.ToArray());


			//The column names in data table
			foreach (DataTableAggregateFunction _calculatedField in _fieldsForCalculation)
			{
				if (!_dtReturn.Columns.Contains(_calculatedField.OutPutColumnName))
				{
					_dtReturn.Columns.Add(_calculatedField.OutPutColumnName);
				}
			}

			//Gets the collection and send it back
			for (int i = 0; i < _dtReturn.Rows.Count; i = i + 1)
			{

				string _filterString = string.Empty;
				for (int j = 0; j < _groupByColumnNames.Count; j = j + 1)
				{
					#region Gets the filter string
					string value = Convert.ToString(_dtReturn.Rows[i][_groupByColumnNames[j]]);

					sortColumnsName = _groupByColumnNames[j] + ((j == 0) ? "" : ",");

					if (!string.IsNullOrEmpty(value))
					{
						value = value.Replace("'", "''");
					}
					if (j > 0)
					{
						_filterString += " AND ";
					}

					if (_dtReturn.Columns[_groupByColumnNames[j]].DataType == typeof(System.Int32))
					{
						_filterString += _groupByColumnNames[j] + " = " + value + "";
					}
					else if (_dtReturn.Columns[_groupByColumnNames[j]].DataType == typeof(System.DateTime))
					{
						if (string.IsNullOrEmpty(value))
						{
							_filterString += _groupByColumnNames[j] + " is null ";
						}
						else
						{
							_filterString += _groupByColumnNames[j] + " = #" + value + "#";
						}
					}
					else
					{
						if (string.IsNullOrWhiteSpace(value))
						{
							_filterString += _groupByColumnNames[j] + " is null ";
						}
						else
						{
							_filterString += _groupByColumnNames[j] + " = '" + value + "'";
						}
					}


					#endregion

					#region Compute the aggregate command

					foreach (DataTableAggregateFunction _calculatedField in _fieldsForCalculation)
					{
						if (_calculatedField.ColumnName == _groupByColumnNames[j])
							_dtReturn.Rows[i][_calculatedField.OutPutColumnName] = _dtSource.Compute(Convert.ToString(_calculatedField.enmFunction) + "(" + _calculatedField.ColumnName + ")", _filterString);
					}
					#endregion
				}

			}

			var dv = _dtReturn.DefaultView;
			dv.Sort = String.Join(",", _groupByColumnNames) + " asc";

			return dv.ToTable();
		}

		public static DataTable GetGroupedBySummary(this DataTable _dtSource, IList<string> _groupByColumnNames, IList<DataTableAggregateFunction> _fieldsForCalculation, IList<string> untouched, BodyParams param)
		{
			string sortColumnsName = "";
			string level = "level";
			string des = "des";
			int levelcount = _groupByColumnNames.Count;
			//Once the columns are added find the distinct rows and group it bu the numbet
			_dtSource.DefaultView.RowFilter = "";
			DataTable _dtReturn = _dtSource.DefaultView.ToTable(true, _groupByColumnNames.ToArray());


			//The column names in data table
			foreach (var u in param.columns)
			{
				if (!_dtReturn.Columns.Contains(u.ToLower()))
				{
					_dtReturn.Columns.Add(u.ToLower());
				}
			}

			if (!_dtReturn.Columns.Contains(level))
			{
				_dtReturn.Columns.Add(level);
			}
			if (!_dtReturn.Columns.Contains(des))
			{
				_dtReturn.Columns.Add(des);
			}

			foreach (DataTableAggregateFunction _calculatedField in _fieldsForCalculation)
			{
				if (!_dtReturn.Columns.Contains(_calculatedField.OutPutColumnName))
				{
					_dtReturn.Columns.Add(_calculatedField.OutPutColumnName);
				}
			}

			//Gets the collection and send it back
			for (int i = 0; i < _dtReturn.Rows.Count; i = i + 1)
			{



				string _filterString = string.Empty;
				for (int j = 0; j < _groupByColumnNames.Count; j = j + 1)
				{
					#region Gets the filter string
					string value = Convert.ToString(_dtReturn.Rows[i][_groupByColumnNames[j]]);

					sortColumnsName = _groupByColumnNames[j] + ((j == 0) ? "" : ",");

					if (!string.IsNullOrEmpty(value))
					{
						value = value.Replace("'", "''");
					}
					if (j > 0)
					{
						_filterString += " AND ";
					}

					if (_dtReturn.Columns[_groupByColumnNames[j]].DataType == typeof(System.Int32))
					{
						_filterString += _groupByColumnNames[j] + " = " + value + "";
					}
					else if (_dtReturn.Columns[_groupByColumnNames[j]].DataType == typeof(System.DateTime))
					{
						if (string.IsNullOrEmpty(value))
						{
							_filterString += _groupByColumnNames[j] + " is null ";
						}
						else
						{
							_filterString += _groupByColumnNames[j] + " = #" + value + "#";
						}
					}
					else
					{
						if (string.IsNullOrWhiteSpace(value))
						{
							_filterString += _groupByColumnNames[j] + " is null ";
						}
						else
						{
							_filterString += _groupByColumnNames[j] + " = '" + value + "'";
						}
					}

				
					#endregion

				}

				#region Compute the aggregate command

				foreach (DataTableAggregateFunction _calculatedField in _fieldsForCalculation)
				{
					_dtReturn.Rows[i][_calculatedField.OutPutColumnName] = _dtSource.Compute(Convert.ToString(_calculatedField.enmFunction) + "(" + _calculatedField.ColumnName + ")", _filterString);
				}

				_dtReturn.Rows[i][level] = levelcount;
				_dtReturn.Rows[i][des] = "";
				#endregion
			}

			var dv = _dtReturn.DefaultView;
			dv.Sort = String.Join(",", _groupByColumnNames) + " asc";

			return dv.ToTable();
		}

	}
}

