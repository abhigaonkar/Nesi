using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NESI.Common;

namespace NESI.DataProcessor
{
    public static class NesiQueryBuilder
    {

        
        //DTO.ViewModels.Page.CustomerAsset
        public static DataSet CallDataProcessor<T>(List<T> list, BodyParams param,  out DataTable dtSummary) where T:class
        {
            DataSet dsquery = new DataSet();
            NesiDataTableParser dtp = new NesiDataTableParser();
            DataTable dt = dtp.GetDataTableFromObjects<T>(list);
        
          
           
            //  http://localhost:53450/api/Page/CustomerAsset?groupby=manufacturer,addedby&filtercolumn=model&filtervalue=Komatsu&sortby=name&sortorder=ASC&selectcolumn=model,Description,manufacturer,address_id,addedby,active,name&pagesize=5&pagecount=5&innerdata=1
          
          
           
          
            

            dsquery = NesiQueryBuilder.ReturnQyeryData<NESI.DTO.ViewModels.Page.Reports.CustomerAsset>(param, dt,   out dtSummary);
            return dsquery;
        }

        //public static object CallDataProcessor<T1>(List<IModelGenerator<object>> ls, QueryParam param, out DataTable dtSummary) where T1 : class,IModelBase
        //{
        //    throw new NotImplementedException();
        //}

        public static DataSet ReturnQyeryData<T>( BodyParams param,DataTable dt, out  DataTable dtSummary,string InnerDataSetRequested =null)
        {

            DataTable uniqueCols = dt.DefaultView.ToTable(true, param.Selectby.ToArray());
            DataSet ds = new DataSet();
            DataTable dtReturn = new DataTable();           
            if (param.column_filter != null)
            {
                dtReturn = GetFilteredData(uniqueCols, param.column_filter , param.column_sort );
            }
            else
            {
                dtReturn = dt.Copy();
            }

            dtSummary = dtReturn.Clone();
            if (param.summary_by != null && param.summary_by.Count() >0)
            {
                DataRow newRow = GetSummary(param.summary_by, dtReturn, ref  dtSummary);
                dtSummary.Rows.Add(newRow);

            }

            //if (!CheckNullOrEmpty(column_sort) && !CheckNullOrEmpty(column_sortorder))
            //{
            //    dtReturn = GetSortedData(dt,column_sort, column_sortorder);
            //}

            if (param.column_groupBy !=null && param.column_groupBy.Count()>0)
            {
              //  ds = GetGroupByData(dtReturn, param.column_groupBy, param.childdata, param.summary, ref dtSummary);
            }
            else
            {
                ds.Tables.Add(dtReturn);
            }

            if (!CheckNullOrEmpty(param.page_size) && !CheckNullOrEmpty(param.page_count))
            {
                ds = GetPaginatedData(ds, param.page_size, param.page_count);
            }
            
            return ds;
        }

        private static DataRow GetSummary(string[] summary, DataTable dtInput,ref DataTable dtOutput,string expression="")
        {
            var newRow = dtOutput.NewRow();
            foreach (var item in summary)
            {
                var key = item.Split(':')[0];
                var summmaryType = string.Format("{1}({0})", key, item.Split(':')[1]);
                newRow[key] = dtInput.Compute(summmaryType, expression);

            }

            return newRow;
        }

        public static DataSet GetGroupByData(DataTable dt, string[] column_groupBy, bool getInnerData,string[] lsSummary, ref DataTable dtSummary, string InnerDataSetRequested = null)
        {
            DataSet ds = new DataSet();
            if (column_groupBy.Count() != 0)
            {
                DataTable uniqueCols = GetDistictColumns(dt, column_groupBy);
                string expressionFormat = string.Empty;

                foreach (DataColumn item in uniqueCols.Columns)
                {

                    expressionFormat += item.ColumnName + "=" + "'{" + item.Ordinal + "}' And ";

                }



                expressionFormat = expressionFormat.Substring(0, expressionFormat.Length - 4);

                foreach (DataRow item in uniqueCols.Rows)
                {
                    try
                    {


                        string expression = string.Format(expressionFormat, item.ItemArray);

                        if (lsSummary != null && lsSummary.Count() > 0)
                        {

                            var newRow = GetSummary(lsSummary, dt, ref dtSummary, expression);

                            for (int i = 0; i < column_groupBy.Count(); i++)
                            {
                                newRow[column_groupBy[i]] = item.ItemArray[i];
                            }

                            dtSummary.Rows.Add(newRow);

                        }


                        var foundRows = dt.Select(expression);
                        var dtNew = dt.Clone();
                        dtNew.TableName = expression;
                        ds.Tables.Add(dtNew);
                        if (getInnerData || expression == InnerDataSetRequested)
                        {
                            foreach (DataRow newRow in foundRows)
                            {
                                dtNew.Rows.Add(newRow.ItemArray);
                            }
                            dtNew.AcceptChanges();
                        }
                        else
                        {
                            var newRow = dtNew.NewRow();
                            foreach (DataColumn column in uniqueCols.Columns)
                            {
                                newRow[column.ColumnName] = item[column.ColumnName];

                            }
                            dtNew.Rows.Add(newRow);


                            dtNew.AcceptChanges();
                        }
                    }
                    catch 
                    { }
                }
                ds.AcceptChanges();
                return ds;


            }
            else
            {
                return ds;
            } 
            
        }

        private static DataTable GetDistictColumns(DataTable dt, string[] column_groupBy)
        {
            DataTable uniqueCols = dt.DefaultView.ToTable(true, column_groupBy.ToArray());

            uniqueCols = (uniqueCols.AsEnumerable().Select(delegate (DataRow r)
            {

                DataRow nr = uniqueCols.NewRow();

                foreach (DataColumn item in uniqueCols.Columns)
                {
                    nr.SetField(item.ColumnName, r[item.ColumnName] == DBNull.Value ? "" : r[item.ColumnName]);
                    nr.SetField(item.ColumnName, Convert.ToString(r[item.ColumnName]).Contains("'") ? Convert.ToString(r[item.ColumnName]).Replace("'", "''") : r[item.ColumnName]);
                }
                return nr;
            })).CopyToDataTable();
            return uniqueCols;
        }

        public static DataTable GetFilteredData(DataTable dt, Filter[] column_filter, Common.SortOrder[] sortBy)
        {
            //DataTable FilterCols = dt.DefaultView.ToTable(true, column_filter.ToArray());
            DataView dv = new DataView();
            dv = dt.DefaultView;
            string expressionFormat = string.Empty;
            if (column_filter != null && column_filter.Count() != 0)
            {
                foreach (Filter flt in column_filter)
                {
                    if (!CheckNullOrEmpty(flt.coulumnname))
                    {

                        expressionFormat += flt.GetExpression() + " And ";

                    }
                }
                expressionFormat = expressionFormat.Substring(0, expressionFormat.Length - 4);

                dv.RowFilter = expressionFormat;



            }
            SortDattable(sortBy, dv);
            return dv.ToTable();
        }

        private static void SortDattable(Common.SortOrder[] sortBy, DataView dv)
        {
            if (sortBy != null && sortBy.Count() != 0)
            {

                var sortByExpression = string.Join(",", sortBy.Select(p => p.GetExpression()));


                dv.Sort = sortByExpression;
            }
        }

        //public static DataTable GetSortedData(DataTable dt, string sortBy, string sort_order)
        //{                
        //    string expressionFormat = string.Empty;


        //    else
        //    {
        //        return dt;
        //    }                 

        //}
        public static DataSet GetPaginatedData(DataSet ds,int page_size, int page_count)
        {
            
            DataView dv = new DataView();
            DataSet dsNew = new DataSet();
            if (!CheckNullOrEmpty(page_size) && !CheckNullOrEmpty(page_count))
            {
                int count = page_size * page_count;
                int tablecount = ds.Tables.Count;
                int tcount = 0;
                if(count>tablecount)
                {
                    tcount = tablecount;
                    
                }
                else
                {
                    tcount = count;
                }
                for (int i = 0; i < tcount; i++)
                {
                    DataTable dtReturn = ds.Tables[i].Clone();
                    var rows=ds.Tables[i].Rows.OfType<DataRow>().Skip(page_size * page_count - page_size).Take(page_size);
                    foreach (var item in rows)
                    {
                        dtReturn.Rows.Add(item.ItemArray);
                    }
                  
                    //  dv = GetTopDataViewRows(ds.Tables[i].DefaultView, tcount);
                    //dtReturn = dv.Table.Copy();
                    dsNew.Tables.Add(dtReturn);
                }
                return dsNew;
            }
            else
            {
                return ds;
            }        
              
        }
        public static DataView GetTopDataViewRows(DataView dv, Int32 n)
        {
            DataTable dt = dv.Table.Copy();
            for (int i = 0; i < n - 1; i++)
            {
                if (i >= dv.Count)
                {
                    break;
                }
                dt.ImportRow(dv[i].Row);
            }
            return new DataView(dt, dv.RowFilter, dv.Sort, dv.RowStateFilter);
        }
        public static bool CheckNullOrEmpty<T>(T value)
        {
            if (typeof(T) == typeof(string))
                return string.IsNullOrEmpty(value as string);

            return value == null || value.Equals(default(T));
        }
    }


}
