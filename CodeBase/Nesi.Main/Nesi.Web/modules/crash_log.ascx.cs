using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.Web;
using nesi.core;

public partial class modules_crash_log : System.Web.UI.UserControl
	{

    public bool auto_refresh_defult;
    public bool isMessageBoard = true;
    public string pageName = "195";
    NeMember current_user;
    private Toolbox _tools;
	protected void Page_Init(object sender, EventArgs e)
		{
            //layoutforCrashLog.Visible = show_layout_control;
            crashTimer.Enabled = true;
            
            _tools = new Toolbox();
            
            if (!isMessageBoard)
            {
                crashTimer.Enabled = false;
                gv_crashLog.SettingsPager.PageSize = 50;

            }
		}
    protected void page_load(object sender, EventArgs e)
    {
        fill_grid();

    }
    protected void fill_grid()
    {


        var dt = Toolbox.doSQL_dt(@"SELECT
  MAX(e.main_id) id,
  e.member_id,
  m.member_fullname,
  MAX(e.error_timestamp) AS dt,
  e.error_query,
  e.error_stackTrace,
  h.host_name,
  l.line_number,
  mn.machine_name,
  mes.message_text,
  o.origin_path,
  oq.queryString_string,
  u.userAgent_string,
  ip.userIP_address,
  CONCAT(
    e.error_origin_id,
    '-',
    e.error_message_id
  ) _master_id,
  (SELECT
    COUNT(*)
  FROM
    error_log.error
  WHERE error_message_id = e.error_message_id
    AND error_origin_id = e.error_origin_id
    AND error_timestamp > CURDATE()) AS error_count,
  (SELECT
    COUNT(*)
  FROM
    error_log.error
    LEFT JOIN error_log.error_host h
      ON h.host_id = error.error_host_id
  WHERE error_message_id = e.error_message_id
    AND error_origin_id = e.error_origin_id
    AND error.error_is_local = FALSE
    AND h.host_name != 'jordan.nesi.ca'
    AND h.host_name != 'andy.nesi.ca'
    AND h.host_name != 'matt.nesi.ca'
    AND h.host_name != 'iain.nesi.ca'
    AND h.host_name != '') AS total_error_count,
  e.error_is_global
FROM
  error_log.error e
  LEFT JOIN neintranet.member m
    ON m.member_id = e.member_id
  LEFT JOIN error_log.error_host h
    ON h.host_id = e.error_host_id
  LEFT JOIN error_log.error_line l
    ON l.line_id = e.error_line_id
  LEFT JOIN error_log.error_machine_name mn
    ON mn.machine_id = e.error_machine_id
  LEFT JOIN error_log.error_messages mes
    ON mes.message_id = e.error_message_id
  LEFT JOIN error_log.error_origin o
    ON o.origin_id = e.error_origin_id
  LEFT JOIN error_log.error_origin_queryString oq
    ON oq.queryString_id = e.error_origin_queryString_id
  LEFT JOIN error_log.error_useragent u
    ON u.useragent_id = e.error_useragent_id
  LEFT JOIN error_log.error_userip ip
    ON ip.userip_id = e.error_userip_id
WHERE e.error_is_local = FALSE
  AND e.error_timestamp > CURDATE() + INTERVAL 3 HOUR
  AND h.host_name != 'jordan.nesi.ca'
  AND h.host_name != 'andy.nesi.ca'
  AND h.host_name != 'matt.nesi.ca'
  AND h.host_name != 'iain.nesi.ca'
  AND h.host_name != 'devbeta.nesi.ca'
  AND h.host_name != ''
  AND h.host_name NOT LIKE '%localhost%'
  AND message_text NOT LIKE '%401%'
GROUP BY e.error_message_id,
  e.error_origin_id
ORDER BY error_count DESC
LIMIT 200", null);

        dt.Columns.Add("ferror_stackTrace");

foreach (DataRow dr in dt.Rows)
{

    var errors = dr["error_stackTrace"].ToString();
   var printError = "";

   var inst2 = new List<int>();
   var indexs = 0;
   while (indexs >= 0)
   {
       if (errors.Length >= 5)
       {
           indexs = errors.IndexOf(":line ", indexs + 1);
           if (indexs >= 0)
               inst2.Add(indexs);
       }
       else
       {
           indexs = -1;
       }
   }
   var first = true;
   foreach (var value in inst2)
   {
       if (value == -1)
       {
           break;
       }
       var index = 0;
       while (index >= 0)
       {
           index = errors.IndexOf("c:\\inetpub", index + 1);
           if (index > value || first)
           {
               first = false;
               var end = errors.IndexOf("at ", index + 1);
               printError += "<br>";
               if (end == -1)
               {
                   printError += errors.Substring(index);
               }
               else
               {
                   printError += errors.Substring(index+1, end - index);
               }
               break;
           }

       }



   }
   errors = errors.Replace("at ", "at ");

   var lines = errors.Split(new string[] { "at " }, StringSplitOptions.None);
   var useLine = "";

   foreach (var str in lines)
   {
       if (str.Contains(":line "))
       {
           if (!str.Contains("Toolbox.catch_error") && !str.Contains("Toolbox.do_errorLog") &&
               !str.Contains("Toolbox.get_stackTrace") && !str.Contains("c:\\Windows\\Microsoft.NET"))
           {
               var b1 = str.IndexOf("(", 1);
               var b2 = str.LastIndexOf(")");

               var remove = str.Substring(b1, (b2 - b1 + 1));

               var newStr = str.Replace(remove, "");

               var firstIn = newStr.IndexOf(" in ");
               var firstDot = newStr.IndexOf(".");

               var toBack = newStr.Substring(firstDot + 1, firstIn - firstDot - 1) + "()";
               newStr = newStr.Substring(firstIn + 4);
               newStr += " - " + toBack;

               useLine += newStr + "<br>";
           }
       }
   }

   //c.PhoneExtension = "<br>"+ useLine;

   var finalStack = "\n" + useLine;
   finalStack = finalStack.Replace(" at", "\nat");

   finalStack = dr["message_text"] + finalStack;
   dr["ferror_stackTrace"] = finalStack;
}

var tErrors = dt.Rows.Count;


gv_crashLog.SettingsText.Title = "Crash Report &emsp; Since 12:00 AM&emsp;&emsp;&emsp;Total Crashes: <b><span id='crashCount' style='color: red;'>" + tErrors + "</span></b>";
gv_crashLog.DataSource = dt;
gv_crashLog.DataBind();



    }




    protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        var gv = (ASPxGridView)sender;
        if (e.Parameters != "")
        {
            if (e.Parameters == "refresh")
            {
                fill_grid();


            }
            else
            {
                gv.LoadClientLayout(e.Parameters);
            }

        }
        else
        {
            gv.FilterExpression = "";
            for (var i = 0; i < gv.Columns.Count; i++)
            {
                if (gv.Columns[i] is GridViewDataColumn)
                {
                    var col = (GridViewDataColumn)gv.Columns[i];
                    if (col.GroupIndex > -1)
                    {
                        gv.UnGroup(col);
                    }
                    col.Visible = true;
                }
            }
        }
    }
    protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        var gv = (ASPxGridView)sender;
        e.Properties["cpExp"] = gv.SaveClientLayout();
    }
    protected void gv_detail_BeforePerformDataSelect(object sender, EventArgs e)
    {
        Session["_master_ID"] = (sender as ASPxGridView).GetMasterRowKeyValue();

        SqlDataSource2.DataBind();
    }
    protected void gv_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
       // if (gv_crashLog.GetRowValues(e.VisibleIndex, "error_is_global") != null && Convert.ToBoolean(gv_crashLog.GetRowValues(e.VisibleIndex, "error_is_global")))
      //  {
        //    e.Row.ForeColor = System.Drawing.Color.Red;
      //  }
      //  else
      //  {
            e.Row.ForeColor = System.Drawing.Color.Black;
       // }
    }

    protected void gv_HtmlRowPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
       // if (gv_crashLog.GetRowValues(e.VisibleIndex, "error_is_global") != null && Convert.ToBoolean(gv_crashLog.GetRowValues(e.VisibleIndex, "error_is_global")))
       // {
         //   e.Cell.ForeColor = System.Drawing.Color.Red;
      //  }
      //  else
     //   {
            e.Cell.ForeColor = System.Drawing.Color.Black;
      //  }
    }

}