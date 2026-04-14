using Spire.Xls;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Net.Http;
using NESI.Common;
using NESI.DataProcessor;
using System.Net.Http.Headers;
using System.Net;
using System.Globalization;
using iTextSharp.text.html.simpleparser;
using System.Text;
using NESI.Common.Interface;
using System.Web.Security.AntiXss;

namespace Export
{
    public static class ExportHelper
    {
        #region Public Methods
        /// <summary>
        /// ExportFile
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dataTable"></param>
        /// <param name="groupBy"></param>
        public static void ExportExcelFile(Worksheet sheet, DataTable dataTable, Schema[] schema, string groupBy = "")
        {
            DataColumn[] exportedColumns = null;
            if (dataTable.ChildRelations != null && dataTable.ChildRelations.Count > 0)
            {
                exportedColumns = GetParentColumns(dataTable, schema);
            }
            else
            {
                exportedColumns = GetParentColumns(dataTable, schema);
            }
            if (string.IsNullOrEmpty(groupBy))
            {
                //insert master table with out grouping
                sheet.InsertDataTable(dataTable, true, 1, 1, -1, -1, exportedColumns, false);
            }
        }
        /// <summary>
        /// ExportExcelFileWithGroup
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dataTable"></param>
        /// <param name="ds"></param>
        /// <param name="moduleName"></param>
        /// <param name="groupBy"></param>
        public static void ExportExcelFileWithGroupData(Worksheet sheet, DataTable dataTable, DataSet ds, string moduleName, string[] groupBy, Schema[] schema)
        {
            DataColumn[] exportedColumns = null;
            if (dataTable.ChildRelations != null && dataTable.ChildRelations.Count > 0)
            {
                exportedColumns = GetParentColumns(dataTable, schema);
            }
            else
            {
                exportedColumns = GetParentColumns(dataTable, schema);
            }

            if (string.IsNullOrEmpty(groupBy[0]))
                //insert master table with out grouping
                sheet.InsertDataTable(dataTable, true, 1, 1, -1, -1, exportedColumns, false);
            else
            {
                GetFileByGroupName(dataTable, sheet, exportedColumns, ds, moduleName, groupBy);
            }
        }
        /// <summary>
        /// GetGroupListItems
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="gpName"></param>
        /// <returns></returns>
        public static List<string> GetGroupListItems(DataTable dataTable, string gpName)
        {
            var groupItems = dataTable.DefaultView.ToTable(true, gpName); //.Select("[" + gpName + "]", gpName + " ASC ");

            //var groupItems = from dt in dataTable.AsEnumerable()
            //                 select dt.Field<string>(gpName);

            List<string> gpList = new List<string>();

            foreach (DataRow row in groupItems.Rows)
                foreach (var gp in row.ItemArray)
                {
                    if (!gpList.Contains(Convert.ToString(gp)))
                        gpList.Add(Convert.ToString(gp));
                }
            return gpList;
        }
        /// <summary>
        /// ExportExcelFile
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="sheet"></param>
        /// <param name="ds"></param>
        /// <param name="moduleName"></param>
        /// <param name="groupBy"></param>
        public static void ExportExcelFile(DataTable dataTable, Worksheet sheet, DataSet ds, string moduleName, string[] groupBy)
        {
            int gpId = 0;
            int colIndex = 1;
            int rowIndex = 2;
            sheet.PageSetup.IsSummaryRowBelow = false;

            foreach (DataRow dr in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    dataTable.Columns[i].ReadOnly = false;
                    if (String.IsNullOrEmpty(Convert.ToString(dr[i])))
                    {
                        dr[i] = String.Empty;
                    }
                }
            }


            XElement xelement = XElement.Parse(ds.GetXml());
            //very 1st group item
            var nameList = GetGroupListItems(dataTable, groupBy[gpId]);
            if (groupBy.Count() > 1)
            {
                if (groupBy.Count() > (gpId + 1))
                {
                    int startIndex1stItem;
                    //2nd group item
                    var childPtItems = GetGroupListItems(dataTable, groupBy[gpId + 1]);

                    foreach (string cust in nameList)
                    {
                        sheet.SetCellValue(rowIndex, 1, groupBy[gpId] + " : " + cust);
                        sheet.Range[rowIndex, 1, rowIndex, 1].Style.Font.IsBold = true;

                        startIndex1stItem = rowIndex;
                        rowIndex += 1;

                        if (childPtItems.Count > 0)
                        {
                            int startIndex2ndItem;
                            int emptyItem = 0;
                            foreach (string flag in childPtItems)
                            {
                                var element2ndItem = (from nm in xelement.Elements(moduleName)
                                                      where (nm.Elements(groupBy[gpId]).Any() ? nm.Element(groupBy[gpId]).Value == cust : false) && (nm.Elements(groupBy[gpId + 1]).Any() ? (String.IsNullOrEmpty(nm.Element(groupBy[gpId + 1]).Value) ? "" : nm.Element(groupBy[gpId + 1]).Value) == flag : false)
                                                      select nm).ToList();

                                if (element2ndItem.Count() > 0)
                                {
                                    sheet.SetCellValue(rowIndex, 1, groupBy[gpId + 1] + " : " + flag);
                                    sheet.Range[rowIndex, 1, rowIndex, 1].Style.Font.IsBold = true;
                                    sheet.Range[rowIndex, 1, rowIndex, 1].Style.HorizontalAlignment = HorizontalAlignType.Left;

                                    startIndex2ndItem = rowIndex;
                                    rowIndex += 1;

                                    if (groupBy.Count() > (gpId + 2))
                                    {
                                        //3rdGroup
                                        var childPt3rdItems = ExportHelper.GetGroupListItems(dataTable, groupBy[gpId + 2]);
                                        if (childPt3rdItems.Count > 0)
                                        {
                                            foreach (string chThrdItem in childPt3rdItems)
                                            {
                                                if (groupBy.Count() > (gpId + 3))
                                                {

                                                }
                                                else
                                                {
                                                    int startIndex3rdItem;
                                                    //child elements for active flag
                                                    var activeElements = (from nm in xelement.Elements(moduleName)
                                                                          where (nm.Elements(groupBy[gpId]).Any() ? nm.Element(groupBy[gpId]).Value == cust : false) && (nm.Elements(groupBy[gpId + 1]).Any() ? nm.Element(groupBy[gpId + 1]).Value == flag : false)
                                                                          && (nm.Elements(groupBy[gpId + 2]).Any() ? nm.Element(groupBy[gpId + 2]).Value == chThrdItem : false)
                                                                          select nm).ToList();

                                                    if (activeElements.Count > 0)
                                                    {
                                                        sheet.SetCellValue(rowIndex, 1, groupBy[gpId + 2] + " : " + chThrdItem);
                                                        sheet.Range[rowIndex, 1, rowIndex, 1].Style.Font.IsBold = true;
                                                        sheet.Range[rowIndex, 1, rowIndex, 1].Style.HorizontalAlignment = HorizontalAlignType.Left;

                                                        startIndex3rdItem = rowIndex;
                                                        rowIndex += 1;

                                                        foreach (XElement xEle in activeElements)
                                                        {
                                                            foreach (XElement childEle in xEle.Elements())
                                                            {
                                                                if (childEle.Name != groupBy[0] && childEle.Name != groupBy[1] && childEle.Name != groupBy[2])
                                                                {
                                                                    sheet.SetCellValue(rowIndex, colIndex, childEle.Value);
                                                                    colIndex = colIndex + 1;
                                                                }
                                                            }
                                                            colIndex = 1;
                                                            rowIndex += 1;
                                                        }
                                                        //apply groups
                                                        sheet.GroupByRows(startIndex3rdItem + 1, rowIndex - 1, true);
                                                    }
                                                    else
                                                    {
                                                        emptyItem += 1;
                                                    }
                                                }
                                            }
                                        }
                                        //2nd item grouping
                                        if (emptyItem != 0)
                                            rowIndex = rowIndex + 1;

                                        sheet.GroupByRows(startIndex2ndItem + 1, rowIndex - 1, true);
                                    }
                                    //when group by has 2 items
                                    else if (groupBy.Count() == (gpId + 2))
                                    {
                                        //child elements for active flag

                                        var activeElements = (from nm in xelement.Elements(moduleName)
                                                              where (nm.Elements(groupBy[gpId]).Any() ? nm.Element(groupBy[gpId]).Value == cust : false) && (nm.Elements(groupBy[gpId + 1]).Any() ? nm.Element(groupBy[gpId + 1]).Value == flag : false)
                                                              select nm).ToList();

                                        if (activeElements.Count > 0)
                                        {
                                            foreach (XElement xEle in activeElements)
                                            {
                                                foreach (XElement childEle in xEle.Elements())
                                                {
                                                    if (childEle.Name != groupBy[0] && childEle.Name != groupBy[1])
                                                    {
                                                        sheet.SetCellValue(rowIndex, colIndex, childEle.Value);
                                                        colIndex = colIndex + 1;
                                                    }
                                                }
                                                colIndex = 1;
                                                rowIndex += 1;
                                            }
                                            //apply groups
                                            sheet.GroupByRows(startIndex2ndItem + 1, rowIndex - 1, true);
                                        }
                                    }
                                }



                            }
                        }
                        //1st item grouping
                        sheet.GroupByRows(startIndex1stItem + 1, rowIndex - 1, true);
                    }

                }
            }
            //only for one group by item
            else if (groupBy.Count() == 1)
            {
                int start1stIndex;
                foreach (string cust in nameList)
                {
                    var names = (from nm in xelement.Elements(moduleName)
                                 where (string)nm.Element(groupBy[0]) == cust
                                 select nm).ToList();

                    if (names.Count > 0)
                    {
                        sheet.SetCellValue(rowIndex, 1, groupBy[gpId] + " : " + cust);
                        sheet.Range[rowIndex, 1, rowIndex, 1].Style.Font.IsBold = true;
                        sheet.Range[rowIndex, 1, rowIndex, 1].Style.HorizontalAlignment = HorizontalAlignType.Left;

                        start1stIndex = rowIndex;
                        rowIndex += 1;

                        foreach (XElement xEle in names)
                        {
                            foreach (XElement childEle in xEle.Elements())
                            {
                                if (childEle.Name != groupBy[0])
                                {
                                    sheet.SetCellValue(rowIndex, colIndex, childEle.Value);
                                    colIndex = colIndex + 1;
                                }
                            }

                            colIndex = 1;
                            rowIndex += 1;
                        }
                        //apply groups
                        sheet.GroupByRows(start1stIndex + 1, rowIndex - 1, true);
                    }
                }
            }
        }

        /// <summary>
        /// ExportToPdfWithGroupData
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="groupBy"></param>
        /// <param name="headerText"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static string ExportToPdfWithGroupData(DataTable dataTable, string[] groupBy, string headerText, Schema[] schema)
        {
            int gpId = 0;
            int rowIndex = 2;

            string tempFileName = "~/App_Data/" + "tempPdf-" + Guid.NewGuid().ToString() + ".pdf";
            string filePath = System.Web.HttpContext.Current.Server.MapPath(tempFileName);
            //var pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0.0f);
            var pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
            var htmlparser = new HTMLWorker(pdfDoc);

            var nameList = GetGroupListItems(dataTable, groupBy[gpId]);
            int colspan = Convert.ToInt32(dataTable.Columns.Count) - groupBy.Count();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    var sb = new StringBuilder();
                    sb.Append("<table border='1' style='width: 100%;'>");
                    sb.Append("<tr>");
                    sb.Append("<td align=Center bgcolor=#3984b8 style='color:#ffffff;' colspan=" + colspan + ">");
                    sb.Append("<b>" + headerText + "</b>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    //Header Columns
                    for (int col = groupBy.Count(); col < dataTable.Columns.Count; col++)
                    {
                        sb.Append("<td style='width: 10%; color:#ffffff;' bgcolor=#3984b8>");
                        sb.Append("<b>" + dataTable.Columns[col].Caption + "</b>");
                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");

                    if (groupBy.Count() > 1)
                    {
                        if (groupBy.Count() > (gpId + 1))
                        {
                            //2nd group item
                            var childPtItems = GetGroupListItems(dataTable, groupBy[gpId + 1]);

                            foreach (string cust in nameList)
                            {
                                var grpBy1 = schema.Where(s => s.name.ToUpper() == groupBy[gpId].ToUpper())
                                    .Select(p => p.header).FirstOrDefault();

                                sb.Append("<tr>");
                                sb.Append("<td colspan=" + colspan + ">");
                                sb.Append("<b>" + (string.IsNullOrEmpty(grpBy1) ? groupBy[0] : grpBy1) + ":" + cust + "</b>");
                                sb.Append("</td>");
                                sb.Append("</tr>");

                                rowIndex += 1;

                                if (childPtItems.Count > 0)
                                {
                                    int startIndex2ndItem;
                                    int emptyItem = 0;
                                    foreach (string flag in childPtItems)
                                    {
                                        string cust11 = cust;
                                        var srchStr1 = "";
                                        var srchStr2 = "";

                                        if (string.IsNullOrEmpty(cust11))
                                        srchStr1 = $"{groupBy[gpId].Replace("'", "''")} IS NULL";
                                        else
                                        srchStr1 = $"{groupBy[gpId].Replace("'", "''")} = '{Convert.ToString(cust11).Replace("'", "''")}'";

                                        if (string.IsNullOrEmpty(flag))
                                        srchStr2 = $"{groupBy[gpId + 1].Replace("'", "''")} IS NULL";
                                        else
                                        srchStr2 = $"{groupBy[gpId + 1].Replace("'", "''")} = '{Convert.ToString(flag).Replace("'", "''")}'";

                                        var rows = dataTable
                                            .Select($"{srchStr1} AND {srchStr2}")
                                            .ToList();

                                        //var rows = dataTable.AsEnumerable()
                                        //    .Where(
                                        //        r =>
                                        //            r.Field<string>(groupBy[gpId]) ==
                                        //            Convert.ToString(cust11) &&
                                        //            r.Field<string>(groupBy[gpId + 1]) ==
                                        //            Convert.ToString(flag)).ToList();

                                        DataTable dt1 = new DataTable();
                                        if (rows.Any())
                                        {
                                            dt1 = rows.CopyToDataTable();
                                        }
                                        if (dt1.Rows.Count > 0)
                                        {
                                            var grpBy2 = schema.Where(s => s.name.ToUpper() == groupBy[gpId + 1].ToUpper())
                                                .Select(p => p.header).FirstOrDefault();

                                            sb.Append("<tr>");
                                            sb.Append("<td colspan=" + colspan + ">");
                                            sb.Append("<b>" + (string.IsNullOrEmpty(grpBy2) ? groupBy[0] : grpBy2) + " : " + flag + "</b>");
                                            sb.Append("</td>");
                                            sb.Append("</tr>");

                                            startIndex2ndItem = rowIndex;
                                            rowIndex += 1;

                                            if (groupBy.Count() > (gpId + 2))
                                            {
                                                //3rdGroup
                                                var childPt3rdItems = GetGroupListItems(dataTable, groupBy[gpId + 2]);
                                                if (childPt3rdItems.Count > 0)
                                                {
                                                    foreach (string chThrdItem in childPt3rdItems)
                                                    {
                                                        if (groupBy.Count() > (gpId + 3))
                                                        {

                                                        }
                                                        else
                                                        {
                                                            int startIndex3rdItem;

                                                            string flag1 = flag;
                                                            string item3 = chThrdItem;

                                                            var rows3 = new List<DataRow>();

                                                            var srchStr11 = "";
                                                            var srchStr21 = "";
                                                            var srchStr31 = "";

                                                            if (string.IsNullOrEmpty(cust11))
                                                            srchStr11 = $"{groupBy[gpId].Replace("'", "''")} IS NULL";
                                                            else
                                                            srchStr11 = $"{groupBy[gpId].Replace("'", "''")} = '{Convert.ToString(cust11).Replace("'", "''")}'";

                                                            if (string.IsNullOrEmpty(flag1))
                                                            srchStr21 = $"{groupBy[gpId + 1].Replace("'", "''")} IS NULL";
                                                            else
                                                            srchStr21 = $"{groupBy[gpId + 1].Replace("'", "''")} = '{Convert.ToString(flag1).Replace("'", "''")}'";

                                                            if (string.IsNullOrEmpty(item3))
                                                            srchStr31 = $"{groupBy[gpId + 2].Replace("'", "''")} IS NULL";
                                                            else
                                                            srchStr31 = $"{groupBy[gpId + 2].Replace("'", "''")} = '{Convert.ToString(item3).Replace("'", "''")}'";




                                                            if (cust11 != null && flag1 != null && item3 != null)
                                                                rows3 = dataTable.Select($"{srchStr11} AND {srchStr21} AND {srchStr31}").ToList();

                                                            //var rows3 = dataTable.AsEnumerable()
                                                            //    .Where(
                                                            //        r =>
                                                            //            cust11 != null &&
                                                            //            (flag1 != null &&
                                                            //             (item3 != null &&
                                                            //              (r.Field<string>(groupBy[gpId]) ==
                                                            //               Convert.ToString(cust11) &&
                                                            //               r.Field<string>(groupBy[gpId + 1]) ==
                                                            //               Convert.ToString(flag1)
                                                            //               &&
                                                            //               r.Field<string>(groupBy[gpId + 2]) ==
                                                            //               Convert.ToString(item3))))).ToList();

                                                            var dt2 = new DataTable();
                                                            if (rows3.Any())
                                                            {
                                                                dt2 = rows.CopyToDataTable();
                                                            }
                                                            if (dt2.Rows.Count > 0)
                                                            {
                                                                var grpBy3 = schema.Where(s => s.name.ToUpper() == groupBy[gpId + 2].ToUpper())
                                                                    .Select(p => p.header).FirstOrDefault();

                                                                sb.Append("<tr>");
                                                                sb.Append("<td colspan=" + colspan +
                                                                          ">");
                                                                sb.Append("<b>" + (string.IsNullOrEmpty(grpBy3) ? groupBy[0] : grpBy3) + " : " + item3 + "</b>");
                                                                sb.Append("</td>");
                                                                sb.Append("</tr>");

                                                                startIndex3rdItem = rowIndex;
                                                                rowIndex += 1;


                                                                for (int i = 0; i < dt2.Rows.Count; i++)
                                                                {
                                                                    sb.Append("<tr>");
                                                                    for (int j = groupBy.Count(); j < dt2.Columns.Count; j++)
                                                                    {
                                                                        if (dt1.Rows[i][j] != null)
                                                                        {
                                                                            sb.Append("<td style='width: 10%;'>");
                                                                            sb.Append("" + AntiXssEncoder.HtmlEncode(Convert.ToString(dt1.Rows[i][j]), true) + "");
                                                                            sb.Append("</td>");
                                                                        }
                                                                    }
                                                                    sb.Append("</tr>");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                emptyItem += 1;
                                                            }
                                                        }
                                                    }
                                                }
                                                //2nd item grouping
                                                if (emptyItem != 0)
                                                    rowIndex = rowIndex + 1;
                                            }
                                            //when group by has 2 items
                                            else if (groupBy.Count() == (gpId + 2))
                                            {
                                                string flag1 = flag;
                                                string cust2 = cust;

                                                var srchStr111 = "";
                                                var srchStr211 = "";

                                                if (string.IsNullOrEmpty(cust2))
                                                srchStr111 = $"{groupBy[gpId].Replace("'", "''")} IS NULL";
                                                else
                                                srchStr111 = $"{groupBy[gpId].Replace("'", "''")} = '{Convert.ToString(cust2).Replace("'", "''")}'";

                                                if (string.IsNullOrEmpty(flag1))
                                                srchStr211 = $"{groupBy[gpId + 1].Replace("'", "''")} IS NULL";
                                                else
                                                srchStr211 = $"{groupBy[gpId + 1].Replace("'", "''")} = '{Convert.ToString(flag1).Replace("'", "''")}'";

                                                var rows4 = dataTable.Select($"{srchStr111} AND {srchStr211}").ToList();

                                                //var rows4 = dataTable.AsEnumerable()
                                                //    .Where(
                                                //        r =>
                                                //            r.Field<string>(groupBy[gpId]) ==
                                                //            Convert.ToString(cust2) &&
                                                //            r.Field<string>(groupBy[gpId + 1]) ==
                                                //            Convert.ToString(flag1)).ToList();

                                                var dt2 = new DataTable();
                                                if (rows4.Any())
                                                {
                                                    dt2 = rows.CopyToDataTable();
                                                }
                                                if (dt2.Rows.Count > 0)
                                                {

                                                    for (int i = 0; i < dt2.Rows.Count; i++)
                                                    {
                                                        sb.Append("<tr>");
                                                        for (int j = groupBy.Count(); j < dt2.Columns.Count; j++)
                                                        {
                                                            if (dt1.Rows[i][j] != null)
                                                            {
                                                                sb.Append("<td style='width: 10%;'>");
                                                                sb.Append("" + AntiXssEncoder.HtmlEncode(Convert.ToString(dt1.Rows[i][j]), true) + "");
                                                                sb.Append("</td>");
                                                            }
                                                        }
                                                        sb.Append("</tr>");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (groupBy.Count() == 1)
                    {
                        foreach (var cust in nameList)
                        {
                            string cust1 = Convert.ToString(cust);
                            var srchStr = "";

                            if (string.IsNullOrEmpty(cust1))
                            srchStr = $"{groupBy[0].Replace("'", "''")} IS NULL";
                            else
                            srchStr = $"{groupBy[0].Replace("'", "''")} = '{cust1.Replace("'","''")}'";

                            var rows = dataTable.Select(srchStr).ToList();

                            //List<DataRow> rows = dataTable.AsEnumerable()
                            //    .Where(r => r.Field<string>(groupBy[0]) ==Convert.ToString(cust1)).ToList();

                            var dt1 = new DataTable();

                            if (rows != null && rows.Count > 0)
                            {

                                dt1 = rows.CopyToDataTable();

                            }

                            var grpBy4 = schema.Where(s => s.name.ToUpper() == groupBy[0].ToUpper())
                                .Select(p => p.header).FirstOrDefault();

                            sb.Append("<tr>");
                            sb.Append("<td colspan=" + colspan + ">");
                            sb.Append("<b>" + (string.IsNullOrEmpty(grpBy4) ? groupBy[0] : grpBy4) + ":" + cust1 + "</b>");
                            sb.Append("</td>");
                            sb.Append("</tr>");

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                sb.Append("<tr>");
                                for (int j = groupBy.Count(); j < dataTable.Columns.Count; j++)
                                {
                                    if (dt1.Rows[i][j] != null)
                                    {
                                        sb.Append("<td style='width: 10%;'>");
                                        sb.Append("" + AntiXssEncoder.HtmlEncode(Convert.ToString(dt1.Rows[i][j]), true) + "");
                                        sb.Append("</td>");
                                    }
                                }
                                sb.Append("</tr>");
                            }
                        }
                    }

                    sb.Append("</table>");

                    var sr = new StringReader(Convert.ToString(sb.ToString()));

                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    stream.Close();
                    return tempFileName;
                }
                
            return "";
        }
        /// <summary>
        /// ExportToExcelInit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <param name="grouparray"></param>
        /// <param name="fileName"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static HttpResponseMessage ExportToExcelInit<T>(Response<T> result, BodyParams param, string[] grouparray, string fileName, string moduleName, Schema[] schema) where T : class, IModelBase
        {
            NesiDataTableParser dtp = new NesiDataTableParser();
            var datatable = dtp.GetDataTableFromObjects<T>(result.data);


            var flatList = string.Join(",", schema.Select(x => x.name));

            DataView view = new DataView(datatable);
            DataTable interimTable = view.ToTable(false, flatList.Split(','));

            datatable = null;
            datatable = interimTable;


            param.column_groupBy = grouparray;
            DataSet ds = new DataSet();
            ds.Tables.Add(datatable);

            //ramainng only selected collumns 
            var selectCollumnArray = (string[])param.Selectby.Clone();
            datatable = SelectedCollumns(datatable, param.column_groupBy, selectCollumnArray);

            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];

            if (param.column_groupBy.Count() > 0)
            {
                workbook.DataSorter.SortColumns.Add(4, OrderBy.Ascending);
                workbook.DataSorter.SortColumns.Add(6, OrderBy.Ascending);
                workbook.DataSorter.SortColumns.Add(7, OrderBy.Ascending);
                ExportExcelFileWithGroupData(sheet, ds.Tables[0], ds, moduleName, param.column_groupBy, schema);
            }
            else
            {
                ExportExcelFile(sheet, ds.Tables[0], schema);
            }

            sheet.AllocatedRange.AutoFitColumns();
            sheet.AllocatedRange.HorizontalAlignment = HorizontalAlignType.Left;
            sheet.AllocatedRange.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Medium;
            sheet.AllocatedRange.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Medium;
            sheet.AllocatedRange.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Medium;
            sheet.AllocatedRange.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Medium;

            //MemoryStream ms = new MemoryStream();
            //workbook.SaveToStream(ms);
            //byte[] buffer = new byte[16 * 1024];
            //buffer = ms.ToArray();

            string tempFileName = "~/App_Data/" + "temp-" + Guid.NewGuid().ToString();
            string filePath = System.Web.HttpContext.Current.Server.MapPath(tempFileName);
            workbook.SaveToFile(@filePath, ExcelVersion.Version2010);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            response.Content = new StreamContent(stream, 1024);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xlsx");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName + ".xlsx";
            return response;
        }

        /// <summary>
        /// ExportToPDFInit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <param name="grouparray"></param>
        /// <param name="fileName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static HttpResponseMessage ExportToPDFInit(DataTable result, BodyParams param, string[] grouparray, string fileName, string headerText, Schema[] schema)
        {
            //NesiDataTableParser dtp = new NesiDataTableParser();
            //var datatable = dtp.GetDataTableFromObjects<T>(result.data);

            var flatList = string.Join(",", schema.Where(x => result.Columns.Contains(x.name)).Select(x => x.name));

            DataView view = new DataView(result);
            var datatable = view.ToTable(false, flatList.Split(','));


            //var datatable = result;
            param.column_groupBy = grouparray;

            //ramaning only selected collumns 
            var selectCollumnArray = (string[])param.Selectby.Clone();
            datatable = SelectedCollumns(datatable, param.column_groupBy, selectCollumnArray);

            if (grouparray.Count() > 0 && !string.IsNullOrEmpty(grouparray[0]))
            {
                datatable = SortDataAndSetOrdinal(datatable, grouparray);
            }
            if (param.column_sort != null && param.column_sort.Length > 0)
            {
				
                DataView dv = datatable.DefaultView;
				
	            var sortStr = string.Join(",", param.column_sort
		            .Where(x=> param.columns.Contains(x.ColumnName)).Select(s => s.GetExpression()).ToArray());
                dv.Sort = sortStr;
                datatable = dv.ToTable();
                dv.Dispose();
                dv = null;
            }

            datatable = param.page_size != null && param.page_size > 0 ? datatable.AsEnumerable().Skip(0).Take(param.page_size).CopyToDataTable() : datatable;

            //var flatList = string.Join(",", schema.Where(x => datatable.Columns.Contains(x.name)).Select(x => x.name));

            //DataView view = new DataView(datatable);
            //DataTable interimTable = view.ToTable(false, flatList.Split(','));

            //datatable = null;

            datatable = GetDisplayColumnsHeader(datatable, schema);

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            byte[] buffer;
            var ms = new MemoryStream();
            if (param.column_groupBy.Count() > 0)
            {
                string tempFileName = ExportToPdfWithGroupData(datatable, param.column_groupBy, headerText, schema);
                string filePath = System.Web.HttpContext.Current.Server.MapPath(tempFileName);
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                response.Content = new StreamContent(stream, 1024);
            }
            else
            {
                ms = GeneratePDF(datatable, headerText);
                buffer = ms.ToArray();
                response.Content = new StreamContent(ms);
                response.Content = new ByteArrayContent(buffer);//Use your byte array
            }
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName + ".pdf";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        /// <summary>
        /// GeneratePDF
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        /// 
        /// <summary>
        /// ExportToPDFInit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="param"></param>
        /// <param name="grouparray"></param>
        /// <param name="fileName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static HttpResponseMessage ExportToPDFInit<T>(Response<T> result, BodyParams param, string[] grouparray, string fileName, string headerText, Schema[] schema) where T : class
        {
            NesiDataTableParser dtp = new NesiDataTableParser();
            var datatable = dtp.GetDataTableFromObjects<T>(result.data);
            param.column_groupBy = grouparray;

            //ramaning only selected collumns 
            var selectCollumnArray = (string[])param.Selectby.Clone();
            datatable = SelectedCollumns(datatable, param.column_groupBy, selectCollumnArray);

            if (grouparray.Count() > 0 && !string.IsNullOrEmpty(grouparray[0]))
            {
                datatable = SortDataAndSetOrdinal(datatable, grouparray);
            }
            if (param.column_sort.Length > 0)
            {
                DataView dv = datatable.DefaultView;
                var sortStr = string.Join(",", param.column_sort.Select(s => s.GetExpression()).ToArray());
                dv.Sort = sortStr;
                datatable = dv.ToTable();
                dv.Dispose();
                dv = null;
            }
            var flatList = string.Join(",", schema.Where(x => datatable.Columns.Contains(x.name)).Select(x => x.name));

            DataView view = new DataView(datatable);
            DataTable interimTable = view.ToTable(false, flatList.Split(','));

            datatable = null;

            datatable = GetDisplayColumnsHeader(interimTable, schema);

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            byte[] buffer;
            var ms = new MemoryStream();
            if (param.column_groupBy.Count() > 0)
            {
                string tempFileName = ExportToPdfWithGroupData(interimTable, param.column_groupBy, headerText, schema);
                string filePath = System.Web.HttpContext.Current.Server.MapPath(tempFileName);
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                response.Content = new StreamContent(stream, 1024);
            }
            else
            {
                ms = GeneratePDF(interimTable, headerText);
                buffer = ms.ToArray();
                response.Content = new StreamContent(ms);
                response.Content = new ByteArrayContent(buffer);//Use your byte array
            }
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName + ".pdf";
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return response;
        }

        /// <summary>
        /// GeneratePDF
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static System.IO.MemoryStream GeneratePDF(DataTable dataTable, string Name)
        {
            try
            {
                string[] columnNames = (from dc in dataTable.Columns.Cast<DataColumn>()
                                        select dc.ColumnName).ToArray();
                int Cell = 0;
                int count = columnNames.Length;
                object[] array = new object[count];

                dataTable.Rows.Add(array);

                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                System.IO.MemoryStream mStream = new System.IO.MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, mStream);
                int cols = dataTable.Columns.Count;
                int rows = dataTable.Rows.Count;

                HeaderFooter header = new HeaderFooter(new Phrase(Name), false);

                // Remove the border that is set by default  
                header.Border = iTextSharp.text.Rectangle.TITLE;

                // Align the text: 0 is left, 1 center and 2 right.  
                header.Alignment = Element.ALIGN_CENTER;
                //pdfDoc.Header = header;
                pdfDoc.AddHeader("Header", "Header Text");
                // Header.  
                pdfDoc.Open();
                iTextSharp.text.Table pdfTable = new iTextSharp.text.Table(cols, rows);
                pdfTable.BorderWidth = 1; pdfTable.Width = 100;
                pdfTable.Padding = 1; pdfTable.Spacing = 4;

                //Document header
                Cell headerCol = new Cell();
                Chunk headerChunk = new Chunk();
                headerCol.BackgroundColor = new iTextSharp.text.Color(ColorTranslator.FromHtml("#3984b8"));
                iTextSharp.text.Font ColFontHeader = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE);

                headerChunk = new Chunk(Name, ColFontHeader);
                headerCol.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCol.Colspan = cols;
                //headerCol.HorizontalAlignment = PdfCell.ALIGN_MIDDLE;
                headerCol.Add(headerChunk);
                pdfTable.AddCell(headerCol);
                //end doc h

                //creating table headers  
                for (int i = 0; i < cols; i++)
                {
                    Cell cellCols = new Cell();
                    Chunk chunkCols = new Chunk();
                    cellCols.BackgroundColor = new iTextSharp.text.Color(ColorTranslator.FromHtml("#3984b8"));
                    iTextSharp.text.Font ColFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE);

                    chunkCols = new Chunk(dataTable.Columns[i].Caption, ColFont);

                    cellCols.Add(chunkCols);
                    pdfTable.AddCell(cellCols);
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dataTable);
                //creating table data (actual result)   
                for (int k = 0; k < rows; k++)
                {
                    for (int j = 0; j < cols; j++)
                    {

                        Cell cellRows = new Cell();

                        if (k % 2 == 0)
                        {
                            cellRows.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#cccccc")); ;
                        }
                        else { cellRows.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#ffffff")); }
                        iTextSharp.text.Font RowFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                        string val = "";
                        if (dataTable.Columns[j].ColumnName.ToLower().Contains("date"))
                        {
                            val = Convert.ToString(dataTable.Rows[k][j]);
                            if (!String.IsNullOrEmpty(val))
                            {
                                DateTime tm = DateTime.Today;
                                if (DateTime.TryParse(val, out tm))
                                    val = tm.ToString("yyyy/MM/dd");
                            }
                        }
                        else
                            val = Convert.ToString(dataTable.Rows[k][j]);

                        val = AntiXssEncoder.HtmlEncode(val, true);
                        Chunk chunkRows = new Chunk(val, RowFont);
                        cellRows.Add(chunkRows);

                        pdfTable.AddCell(cellRows);
                    }
                }
                pdfDoc.Add(pdfTable);
                pdfDoc.Close();

                return mStream;
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        #endregion
        #region Private Methods
        private static DataTable SelectedCollumns(DataTable datatable, string[] paramGroupby, string[] paramSelectby)
        {
            //ramainng only selected collumns 
            var selectCollumnArray = paramSelectby;
            if (selectCollumnArray.Length > 0)
            {
                string[] result = paramGroupby.Union(selectCollumnArray).ToArray();
                string[] columnNames = datatable.Columns.Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToArray();
                string[] deleteCollumns = columnNames.Except(result, StringComparer.OrdinalIgnoreCase).ToArray();
                for (int i = 0; i < deleteCollumns.Count(); i++)
                {
                    datatable.Columns.Remove(deleteCollumns[i]);
                }
            }
            return datatable;
            // end
        }
        private static DataColumn[] GetParentColumns(DataTable dataTable, Schema[] schema)
        {
            List<DataColumn> exportedColumnList = new List<DataColumn>();

            if (dataTable.Columns != null)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    exportedColumnList.Add(column);
                }
            }
            foreach (var col in exportedColumnList)
            {
                DataColumn col1 = col;

                foreach (Schema t in schema.Where(t => col1.ColumnName != null && col1.ColumnName.ToUpper() == t.name.ToUpper()))
                {
                    col.Caption = t.header;
                }
            }
            return exportedColumnList.ToArray();
        }
        private static DataTable GetDisplayColumnsHeader(DataTable dataTable, Schema[] schema)
        {
            DataTable d = new DataTable();
            foreach (DataColumn item in dataTable.Columns)
            {
                foreach (Schema s in schema.Where(s => item != null && item.ColumnName.ToUpper() == s.name.ToUpper()))
                {
                    item.Caption = s.header;
                }
            }
            return dataTable;
        }
        private static void GetFileByGroupName(DataTable dataTable, Worksheet sheet, DataColumn[] exportedColumns, DataSet ds, string moduleName, string[] groupBy)
        {
            sheet.InsertDataTable(new DataTable(), true, 1, 1, -1, -1, GetGroupByColumns(exportedColumns, groupBy), false);
            ExportExcelFile(dataTable, sheet, ds, moduleName, groupBy);
        }
        private static DataColumn[] GetGroupByColumns(DataColumn[] exportedColumns, string[] groupBy)
        {
            List<DataColumn> exportedColumnList
                = new List<DataColumn>();
            int gpId = 0;

            foreach (DataColumn column in exportedColumns)
            {
                if (groupBy.Count() == gpId + 1)
                {
                    if (!column.ColumnName.Contains(groupBy[0]))
                    {
                        exportedColumnList.Add(column);
                    }
                }
                else if (groupBy.Count() == (gpId + 2))
                {
                    if (!column.ColumnName.Contains(groupBy[0]) && !column.ColumnName.Contains(groupBy[1]))
                    {
                        exportedColumnList.Add(column);
                    }
                }
                else if (groupBy.Count() == (gpId + 3))
                {
                    if (!column.ColumnName.Contains(groupBy[0]) && !column.ColumnName.Contains(groupBy[1]) && !column.ColumnName.Contains(groupBy[2]))
                    {
                        exportedColumnList.Add(column);
                    }
                }

            }

            return exportedColumnList.ToArray();
        }
        private static DataTable SortDataAndSetOrdinal(DataTable datatable, string[] grouparray)
        {

            string sCategories = string.Join(",", grouparray);
            DataView dv = datatable.DefaultView;
            dv.Sort = sCategories;
            datatable = dv.ToTable();

            for (int i = 0; i < grouparray.Length; i++)
            {
                //int col = datatable.Columns[groupBy[i]]; //dictionary[groupBy[i]];
                string collumnName = grouparray[i];
                datatable.Columns[collumnName].SetOrdinal(i);
            }
            return datatable;
        }
        private static string GetSummaryValue(Dictionary<string, string> dict, DataTable dataTable, string field, string fieldValue)
        {
            string calculatedValue = "";
            var fieldData = dataTable.AsEnumerable()
                                                .Where(
                                                    r =>
                                                        r.Field<string>(field) ==
                                                        Convert.ToString(fieldValue)).ToList();
            var dt2 = new DataTable();
            if (fieldData.Any())
            {
                dt2 = fieldData.CopyToDataTable();
            }

            foreach (var item in dict)
            {
                if (item.Value == "Count")
                {
                    KeyValuePair<string, string> item1 = item;
                    int count = dt2.AsEnumerable().Where(x => Convert.ToString(x[item1.Key]) != "").ToList().Count();
                    calculatedValue = calculatedValue + ":" + count;
                }
            }
            return calculatedValue;
        }
        #endregion
    }


}
