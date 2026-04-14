using System;
using System.Data.OleDb;
using System.Data;
using System.Web.UI;
using System.Web.Configuration;
using nesi.core;

public partial class mc : Page
{
	NeMember current_user;
	private const int _page_id = 83; // from Page table in DB
	private const string _page_name = "MasterCustomer";
	Toolbox _tools = new Toolbox();
	
	bool can_view_dollar_totals = false;
	public int ddl_selected;

	bool can_edit_reps = false;

	protected void Page_Init(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			Session["currentSource"] = null;
		}
		if (Session["currentSource"] != null)
		{
			switch (Session["currentSource"].ToString())
			{
				case "Datasource1":
					BindToDS1();
					break;
				case "Datasource2":
					BindToDS2();
					break;
			}
		}
	}
	private string GetFieldInfoFromQuery(string p_strQuery, string p_strConnectionString)
	{
		var l_adapterSchema = new OleDbDataAdapter(p_strQuery, p_strConnectionString);
		var g_dtSchema = new DataTable();
		l_adapterSchema.FillSchema(g_dtSchema, SchemaType.Source);
		var l_strColumnSchema = "<FLDINF>";
		foreach (DataColumn l_dcolum in g_dtSchema.Columns)
		{
			l_strColumnSchema = string.Concat(l_strColumnSchema, "<FLD>");
			l_strColumnSchema = string.Concat(l_strColumnSchema, "<COL>", l_dcolum.ColumnName, "</COL>");
			l_strColumnSchema = string.Concat(l_strColumnSchema, "<DATTYP>", l_dcolum.DataType.ToString().ToLower().Replace("system.", ""), "</DATTYP>");
			l_strColumnSchema = string.Concat(l_strColumnSchema, "</FLD>");
		}
		l_strColumnSchema += "</FLDINF>";
		return l_strColumnSchema;
	}

	private void BindToDS1()
	{
	}
	private void BindToDS2()
	{
	}
	protected void ASPxGridView1_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters == "1")
		{
			Session["currentSource"] = "Datasource1";
			BindToDS1();
		}
		if (e.Parameters == "2")
		{
			Session["currentSource"] = "Datasource2";
			BindToDS2();
		}
	}
}