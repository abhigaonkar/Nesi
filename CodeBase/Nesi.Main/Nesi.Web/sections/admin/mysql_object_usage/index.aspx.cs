using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nesi.core;

public partial class mysql_object_usage : System.Web.UI.Page
    {
   // Toolbox _tools;
    NeMember current_user;
    private const int _page_id = 213; // from Page table in DB


    protected void Page_Load(object sender, EventArgs e)
        {
        var _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(_page_id);
        var _q = Request.QueryString;
        var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;

        _tools = new Toolbox();
        if (!IsPostBack && !IsCallback)
            {

            /**
             * Make sure to update the path below for filebase compare. 
             */

            string sourceFolder = @"B:\Dev\Ricky\NESI\branch";
            string searchWord = "";
            var schemaUsage = new DataTable();
            schemaUsage.Columns.Add("Name", typeof(string));
            schemaUsage.Columns.Add("Type", typeof(string));
            schemaUsage.Columns.Add("File_Used", typeof(string));
            schemaUsage.Columns.Add("File_Used_list", typeof(string));
            schemaUsage.Columns.Add("Schema_Used", typeof(string));
            schemaUsage.Columns.Add("Schema_Used_list", typeof(string));            

            var schema_views = new DataTable();
            var schema_routines = new DataTable();
            var isUsed = 0;
            var usedFileName = "";
            schema_views = _tools.getSQL_datatable("SELECT * FROM information_schema.views where table_schema = 'neintranet' ", null);
            schema_routines = _tools.getSQL_datatable("SELECT * FROM information_schema.routines  WHERE routine_schema = 'neintranet' ", null);

            // store all views into datatable

            foreach (DataRow view in schema_views.Rows)
                {
                DataRow Entry = schemaUsage.NewRow();
                Entry["Name"] = view["table_name"].ToString();
                Entry["Type"] = "View";
                Entry["File_Used"] = "0";
                Entry["File_Used_list"] = "";
                Entry["Schema_Used"] = "0";
                Entry["Schema_Used_list"] = "";
                schemaUsage.Rows.Add(Entry);
                }

            foreach (DataRow routine in schema_routines.Rows)
                {
                DataRow Entry = schemaUsage.NewRow();
                Entry["Name"] = routine["routine_name"].ToString();
                Entry["Type"] = routine["routine_type"].ToString();
                Entry["File_Used"] = "0";
                Entry["File_Used_list"] = "";
                Entry["Schema_Used"] = "0";
                Entry["Schema_Used_list"] = "";
                schemaUsage.Rows.Add(Entry);
                }

            // Itrrate through files
            List<string> allFiles = new List<string>();
            AddFileNamesToList(sourceFolder, allFiles);
            foreach (string fileName in allFiles)
                {
                if (!fileName.Contains(".svn"))
                    {
                    string contents = File.ReadAllText(fileName).ToLower();

                    // Iterate through data table to search views
                    foreach (DataRow view in schemaUsage.Rows)
                        {
                        if (contents.Contains(view["Name"].ToString().ToLower()))
                            {
                            view["File_Used"] = "1";
                            view["File_Used_list"] = view["File_Used_list"] + " " + fileName.Replace(sourceFolder, " ") + "; ";
                            }
                        }
                    }
                }

            // Iterate through views and routine definations
            foreach (DataRow view in schema_views.Rows)
                {
                string contents = view["view_definition"].ToString().ToLower();

                // Iterate through data table to search views
                foreach (DataRow su in schemaUsage.Rows)
                    {
                    if (su["Name"].ToString() != view["table_name"].ToString())
                        {
                        if (contents.Contains(su["Name"].ToString().ToLower()))
                            {
                            su["Schema_Used"] = "1";
                            su["Schema_Used_list"] = su["Schema_Used_list"] + " View: " + view["table_name"]  + "; ";
                            }
                    }
                   
                    }


            }

            foreach (DataRow routine in schema_routines.Rows)
                {
                string contents = routine["routine_definition"].ToString().ToLower();

                // Iterate through data table to search views
                foreach (DataRow su in schemaUsage.Rows)
                    {
                    if (su["Name"].ToString() != routine["routine_name"].ToString())
                        {
                        if (contents.Contains(su["Name"].ToString().ToLower()))
                            {
                            su["Schema_Used"] = "1";
                            su["Schema_Used_list"] = su["Schema_Used_list"] + routine["routine_type"].ToString() + ": " + routine["routine_name"] + "; ";
                            }
                        }

                    }


                }


            Session["schemaUsage"] = null;
            Session["schemaUsage"] = schemaUsage;
        }
        gvUsage.DataSource = Session["schemaUsage"];
        gvUsage.DataBind();

    }

    public static void AddFileNamesToList(string sourceDir, List<string> allFiles)
        {

        string[] fileEntries = Directory.GetFiles(sourceDir);
        foreach (string fileName in fileEntries)
            {
            allFiles.Add(fileName);
            }

        //Recursion    
        string[] subdirectoryEntries = Directory.GetDirectories(sourceDir);
        foreach (string item in subdirectoryEntries)
            {
            // Avoid "reparse points"
            if ((File.GetAttributes(item) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                {
                AddFileNamesToList(item, allFiles);
                }
            }

        }
}