using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.Web;
using System.Management;
using nesi.core;

public partial class modules_server_usage : System.Web.UI.UserControl
	{
    public bool show_refresh_check;
    public bool auto_refresh_defult;
    private readonly string wmiUsername = Toolbox.app_setting("ldap_user");
    private readonly string wmiPassword = Toolbox.app_setting("ldap_pass");
    private readonly string domain = Toolbox.app_setting("ldap_domain") + ".local";

    NeMember current_user;

    public class neEventViewer : IComparable<neEventViewer>
    {
        public string CPU { get; set; }
        public string RAM { get; set; }
        public double RAM_dbl { get; set; }
        public double CPU_dbl { get; set; }
        public string Server { get; set; }

        public int CompareTo(neEventViewer other)
        {
            return string.Compare(Server, other.Server, StringComparison.Ordinal);
        }

    }
	
	protected void Page_Init(object sender, EventArgs e)
		{
            ASPxCheckBox1.Visible = show_refresh_check;
            serverUsageTimer.Enabled = true;
            
            
		}
    protected void page_load(object sender, EventArgs e)
    {
        if (!Page.IsCallback)
        {
            fill_grid();


            
        }
    }

    protected void fill_grid()
    {
        var settings = Toolbox.doSQL_dt(@"SELECT * FROM it.dashboard_eventviewer_servers  where show_cpu" , null);
        
            var events = new List<neEventViewer>();
        foreach (DataRow dr in settings.Rows)
        {
            /*
            EventLog eventLog1 = new EventLog();
            eventLog1.MachineName = dr["server_name"].ToString();

            if (Convert.ToBoolean(dr["show_application"]))
            {
                eventLog1.Log = "Application";
                getLog(events, eventLog1);
            }

            if (Convert.ToBoolean(dr["show_system"]))
            {
                eventLog1.Log = "system";
                getLog(events, eventLog1);
            }*/
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
            bool isConnected;
            try
            {

                scope.Connect();
                isConnected = scope.IsConnected;
            }catch {
                isConnected = false;
            }
            
            if (isConnected)
            {
                /* entire day */
                
                

               // string dateTime = getDmtfFromDateTime("09/21/2015 8:50:48"); // DateTime specific
                

                var queryHideSource = "";
                var hideSource = Toolbox.doSQL_dt(@"SELECT * FROM it.dashboard_eventviewer_hide_source  WHERE server_id =@v0", new object[] { dr["id"] });
                foreach (DataRow hideDR in hideSource.Rows)
                {
                    queryHideSource += " AND SourceName != '" + hideDR["source"] + "' ";
                }

                var query = new SelectQuery("SELECT LoadPercentage FROM Win32_Processor");
                var searcher = new ManagementObjectSearcher(scope, query);
                var logs = searcher.Get();

                var cpuUsage = (from ManagementBaseObject log in logs where log["LoadPercentage"] != null select Convert.ToDouble(log["LoadPercentage"])).Sum();
                var neEV = new neEventViewer();
                  //  neEV.date = getDateTimeFromDmtfDate(log["TimeWritten"].ToString());
                    //if (log["LoadPercentage"] != null)
                    neEV.CPU= ( cpuUsage / logs.Count ).ToString("##.#") + "%    ";
                    neEV.CPU_dbl = cpuUsage / logs.Count / 100;
                  //  neEV.message = Toolbox.smallString( log["Message"] + "", 500);
                    //neEV.source = log["SourceName"].ToString();
                   // neEV.user = log["User"].ToString();
                   // neEV.log = log["Logfile"].ToString();
                    neEV.Server = dr["server_name"].ToString().ToUpper();

        //            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallLoadServer", "loadServer('" + neEV.Server + "','" + neEV.Usage + "')", true);
                    var query2 = new SelectQuery("SELECT * FROM Win32_OperatingSystem ");
                    var searcher2 = new ManagementObjectSearcher(scope, query2);
                    var logs2 = searcher2.Get();   
                    foreach (var log2 in logs2)
                    {

                        neEV.RAM = (Convert.ToInt32(log2["FreePhysicalMemory"]) / 1000000.0).ToString("#.##") + " / " + (Convert.ToInt32(log2["TotalVisibleMemorySize"]) / 1000000.00).ToString("#.##") + " GB";
                        neEV.RAM_dbl = (Convert.ToDouble(log2["FreePhysicalMemory"])) / (Convert.ToDouble(log2["TotalVisibleMemorySize"]));
                    }

                   // Toolbox.doSQL_void(@"insert into it.server_usage_log (server_id, cpu_load, ram_load));", dr["id"], neEV.CPU_dbl, Math.Round(neEV.RAM_dbl, 2)));
                    
                events.Add(neEV);

             //   scope.Path = new ManagementPath();
            }

        }

     //   System.Diagnostics.EventLog eventLog1 = new System.Diagnostics.EventLog();

        //EventLog eventLog1 = new EventLog();
        //eventLog1.Log = "Application";
        //eventLog1.MachineName = "ne-azserver-02";
        /*
        int count = 0;
        foreach (neEventViewer neEV in events)
        {
            gv_cpu.JSProperties["cp_" + count + "server"] = neEV.Server.ToString();
            gv_cpu.JSProperties["cp_" + count + "load"] = neEV.Usage.ToString();
            count++;
        }
        */
        events.Sort();
        gv_cpu.DataSource = events;
        gv_cpu.DataBind();


     
        /*
                string strHostName = "ne-rds-03";
                string strUserName = "jallison";
                string strPassword = "jordi992";
                ConnectionOptions options = new ConnectionOptions();
                options.Username = strUserName; options.Password = strPassword;
                ManagementScope mgmtScope = new ManagementScope("\\\\" + strHostName + "\\root\\cimv2", options);
                try
                {
                         SelectQuery query =
                     new SelectQuery("Win32_Processor", "");

                        // Initialize an object searcher with this query
                        ManagementObjectSearcher searcher =
                           new ManagementObjectSearcher(query);

                        // Get the resulting collection and loop through it
                        foreach (ManagementObject envVar in searcher.Get())
                        {
                            System.Diagnostics.Debug.WriteLine("System environment variable {0} = {1}",
                               envVar["Name"], envVar["VariableValue"]);
                        }

                       //Get the CPU %
                        ManagementPath mPath_CPU = new ManagementPath();
                        mPath_CPU.RelativePath = "Win32_Processor";
                        ManagementObject mObject_CPU = new ManagementObject(mgmtScope, mPath_CPU, null);

                        string usage = mObject_CPU["LoadPercentage"].ToString();
              

                        //Memory Available (in MBytes)
                        ManagementPath mPath_Mem = new ManagementPath();
                        mPath_Mem.RelativePath = "Win32_PerfRawData_PerfOS_Memory";
                        ManagementClass mc = new ManagementClass(mgmtScope, mPath_Mem, null);

                        // Disk % 
                        ManagementPath mPath_Disk = new ManagementPath();
                        mPath_Disk.RelativePath = "Win32_PerfRawData_PerfDisk_PhysicalDisk.Name='_total'";
                        ManagementObject mObject_Disk = new ManagementObject(mgmtScope, mPath_Disk, null);
                 
                }
                catch (Exception ee)
                {

                }*/

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


}