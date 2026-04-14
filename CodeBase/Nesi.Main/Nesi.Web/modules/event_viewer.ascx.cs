using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using DevExpress.Web;
using System.Management;
using nesi.core;

public partial class modules_event_viewer : System.Web.UI.UserControl
	{
    public bool show_refresh_check;
    public bool auto_refresh_defult;
    private readonly string wmiUsername = Toolbox.app_setting("ldap_user");
    private readonly string wmiPassword = Toolbox.app_setting("ldap_pass");
    private readonly string domain = Toolbox.app_setting("ldap_domain") + ".local";

    NeMember current_user;
    private Toolbox _tools;

    public class neEventViewer : IComparable<neEventViewer>
    {
        public string date { get; set; }
        public string type { get; set; }
        public string source { get; set; }
        public string user { get; set; }
        public string message { get; set; }
        public string server { get; set; }
        public string log { get; set; }

        public int CompareTo(neEventViewer other)
        {
            return other.date.CompareTo(date);
        }

    }
	
	protected void Page_Init(object sender, EventArgs e)
		{
            ASPxCheckBox1.Visible = show_refresh_check;
            eventViewerTimer.Enabled = true;
            _tools = new Toolbox();
        // Already authenticated on the main dashboard page...
//            if (!HttpContext.Current.Request.UserHostAddress.Contains("72.14.168.138") &&
//                 !HttpContext.Current.Request.UserHostAddress.Contains("23.96.59.193") &&
//                 !HttpContext.Current.Request.IsLocal)
//            {
//                current_user = Toolbox.do_handle_authentication(1);
                
//            }
		}
    protected void page_load(object sender, EventArgs e)
    {
        if (!this.Page.IsCallback)
        {
            fill_grid();
        }
    }

    protected void fill_grid()
    {
        var settings = Toolbox.doSQL_dt(@"SELECT * FROM it.dashboard_eventviewer_servers  where show_application or show_system" , null);
        
            var events = new List<neEventViewer>();
        foreach (DataRow dr in settings.Rows)
        {
            var conOpt = new ConnectionOptions();
            conOpt.Impersonation = ImpersonationLevel.Impersonate;
            conOpt.EnablePrivileges = true;
            if (dr["server_name"].ToString() != "ne-azserver-02")
            {
                conOpt.Username = wmiUsername;
                conOpt.Password = wmiPassword;
                conOpt.Authority = string.Format("ntlmdomain:{0}", domain);
            }

            var scope = new ManagementScope(string.Format(@"\\{0}\ROOT\CIMV2", dr["server_name"] + "." + domain), conOpt);
            try
            {

                scope.Connect();
            }
            catch
            {

            }
            var isConnected = scope.IsConnected;
            if (isConnected)
            {
                
                var dateTime = getDmtfFromDateTime(DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0)));

               // string dateTime = getDmtfFromDateTime("09/21/2015 8:50:48"); // DateTime specific
                var queryLogFile = "";
                if (Convert.ToBoolean(dr["show_application"]))
                {
                    queryLogFile += " Logfile = 'Application'";
                }
                if (Convert.ToBoolean(dr["show_system"]))
                {
                    if (queryLogFile.Length != 0)
                    {
                        queryLogFile += " OR ";
                    }
                    queryLogFile += "Logfile = 'system' ";
                }
                if (Convert.ToBoolean(dr["show_system"]))
                {
                    if (queryLogFile.Length != 0)
                    {
                        queryLogFile += " OR ";
                    }
                    queryLogFile += "Logfile = 'system' ";
                }

                var queryType= "";
                if (!Convert.ToBoolean(dr["show_critical"]))
                {
                    queryType += "AND Type != 'critical' ";
                }
                if (!Convert.ToBoolean(dr["show_error"]))
                {
                    queryType += "AND Type != 'error' ";
                }
                if (!Convert.ToBoolean(dr["show_warning"]))
                {
                    queryType += "AND Type != 'warning' ";
                }
                if (!Convert.ToBoolean(dr["show_information"]))
                {
                    queryType += "AND Type != 'information' ";
                }
                if (!Convert.ToBoolean(dr["show_verbose"]))
                {
                   
                    queryType += "AND Type != 'verbose' ";
                }

                var queryHideSource = "";
                var hideSource = Toolbox.doSQL_dt(@"SELECT * FROM it.dashboard_eventviewer_hide_source  WHERE server_id =@v0", new object[] { dr["id"] });
                foreach (DataRow hideDR in hideSource.Rows)
                {
                    queryHideSource += " AND SourceName != '" + hideDR["source"] + "' ";
                }

                var query = new SelectQuery("Select * from Win32_NTLogEvent Where ( " + queryLogFile + " ) and TimeGenerated >='" + dateTime + "' " + queryType + queryHideSource);
                var searcher = new ManagementObjectSearcher(scope, query);
                var logs = searcher.Get();
                foreach (var log in logs)
                {

                    var neEV = new neEventViewer();
                    neEV.date = getDateTimeFromDmtfDate(log["TimeWritten"].ToString());
                    neEV.type = log["Type"].ToString();
                    neEV.message = Toolbox.smallString( log["Message"] + "", 500);
                    neEV.source = log["SourceName"].ToString();
                   // neEV.user = log["User"].ToString();
                    neEV.log = log["Logfile"].ToString();
                    neEV.server = dr["server_name"].ToString().ToUpper();

                    events.Add(neEV);

                }
                events.Sort();
                scope.Path = new ManagementPath();
            }

        }

     //   System.Diagnostics.EventLog eventLog1 = new System.Diagnostics.EventLog();

        //EventLog eventLog1 = new EventLog();
        //eventLog1.Log = "Application";
        //eventLog1.MachineName = "ne-azserver-02";
        
      

        

        gv.DataSource = events;
        gv.DataBind();
        gv.Columns["message"].Visible = false;
        gv.Columns["user"].Visible = false;


     
     

    }


    private static string getDmtfFromDateTime(DateTime dateTime)
    {
        return ManagementDateTimeConverter.ToDmtfDateTime(dateTime);
    }

    private static string getDmtfFromDateTime(string dateTime)
    {
        var dateTimeValue = Convert.ToDateTime(dateTime);
        return getDmtfFromDateTime(dateTimeValue);
    }

    private static string getDateTimeFromDmtfDate(string dateTime)
    {
        return ManagementDateTimeConverter.ToDateTime(dateTime).ToString();
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
    }

    protected void gv_watch_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "type")
        {
            var val = Toolbox.ReturnBlankIfNull_string(e.CellValue).ToString();
            if (val == "Warning")
            {
                e.Cell.Style["background-color"] = "#ccf";
                e.Cell.Style["color"] = "#000";
                e.Cell.Style["font-weight"] = "bold";
            }
            if (val == "Information")
            {
                e.Cell.Style["background-color"] = "#cfc";
                e.Cell.Style["color"] = "#000";
                e.Cell.Style["font-weight"] = "normal";
            }
            if (val == "Error")
            {
                e.Cell.Style["background-color"] = "#fcc";
                e.Cell.Style["color"] = "#000";
                e.Cell.Style["font-weight"] = "normal";
            }
        }
    }

}