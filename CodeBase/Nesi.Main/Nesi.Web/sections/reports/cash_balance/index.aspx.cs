using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using nesi.core;

public partial class sections_reports_cash_balance_index : System.Web.UI.Page
{
    public NeMember myMember;

    private const int _page_id = 72; // from Page table in DB
    private const string _page_description = "Cash Balances";
	static string _page_name = "CashBalances";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;

	protected void Page_Init(object sender,EventArgs e)
	{
		var _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		layout.used_gv = gv1;
		h.Set("gridview_id", "gv1");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();
		_tools.dont_cache_page();
	}

    protected void Page_Load(object sender, EventArgs e)
    {
        
		var _tools = new Toolbox();
		if (!IsCallback && !IsPostBack)
		{
			var gl = new NeGridLayouts(myMember.id, _page_name);
				if (gl.GridLayoutID == 0)
				{
					gl.GridLayout_Layout = gv1.SaveClientLayout();
					gl.member_id = myMember.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					gv1.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				dde_filter.Text = gl.GridLayout_Name;
			
		}
		_tools.dont_cache_page();
        if (!IsPostBack)
        {
            var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
            divMenu.InnerHtml = menu.MenuHTML;
            divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
			Session["cash_balances"] = null;
        }
		fill_grid();

    }

	protected void fill_grid()
	{
	 //   try
	 //       {
	 //       lblError.Visible = false;
  //          if (Session["cash_balances"] == null)
		//{
		    
		//        Session["cash_balances"] = email_reports.get_cash_report(myMember.id);
		        

		//    }

		//var dt = (DataTable)Session["cash_balances"];
		
		//gv1.DataSource = Session["cash_balances"];
		//gv1.DataBind();

		//try
		//	{
		//	name_branch.Text = dt.Rows.Count>0 && Toolbox.ReturnNullDateTime(dt.Rows[0]["last_updated"]) != null ?
  //              "Last Updated: " + Toolbox.doSQL_string(@"Select fun_time(@v0)", new object[] { Convert.ToDateTime( Toolbox.ReturnNullDateTime(dt.Rows[0]["last_updated"])).ToString("yyyy-MM-dd HH:mm:ss")})
		//									: "";
		//	}
		//catch
		//	{
		//	name_branch.Text = "";
		//	}

		//if (!IsPostBack)
		//{
		//gv1.ExpandAll();           
		//}
	 //       }
	 //   catch (Exception e)
	 //       {
	 //       Toolbox.do_errorLog(e);
	 //       // throw;
	 //      lblError.Text = "Error occured while connecting to BV database.";
	 //       lblError.Visible = true;

	 //       }

    }


	protected void gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		
	}

	protected void gv_customer_assets_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}
	protected void gv_customer_assets_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "")
		{
			gv.LoadClientLayout(e.Parameters);
			gv1.ExpandAll();
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



	protected void gv1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.VisibleIndex >= 0)
		{
			if ((e.DataColumn.FieldName.Substring(0, 1).Equals("1")||(e.DataColumn.FieldName.Equals("Total"))))
			{
				e.Cell.Text = Toolbox.ReturnBlankIfNull_string(e.CellValue)==" "? " ": string.Format("C2", e.CellValue);
				if (e.DataColumn.FieldName.Equals("Total"))
				{
					e.Cell.Font.Bold = true;
				}
			}
		}
	}
	protected void gv1_DataBound(object sender, EventArgs e)
	{
		gv1.GroupSummary.Clear();
		gv1.TotalSummary.Clear();
		foreach (GridViewDataColumn gvc in gv1.Columns)
		{
			if (gvc.FieldName.Length > 0)
			{
				if (gvc.FieldName.Substring(0, 1).Equals("1") || gvc.FieldName.Equals("Total"))
				{
					var a = new ASPxSummaryItem();
					a.FieldName = gvc.FieldName;
					a.DisplayFormat = "C2";
					a.ShowInColumn = "Country";
					a.SummaryType = DevExpress.Data.SummaryItemType.Sum;
					a.ShowInGroupFooterColumn = gvc.FieldName;
					gv1.GroupSummary.Add(a);

					var aa = new ASPxSummaryItem();
					aa.FieldName = gvc.FieldName;
					aa.DisplayFormat = "C2";
					aa.ShowInColumn = "Holdco";
					aa.SummaryType = DevExpress.Data.SummaryItemType.Sum;
					aa.ShowInGroupFooterColumn = gvc.FieldName;
					gv1.GroupSummary.Add(aa);

					var b = new ASPxSummaryItem();
					b.FieldName = gvc.FieldName;
					b.DisplayFormat = "C2";
					b.ShowInColumn = gvc.FieldName;
					b.SummaryType = DevExpress.Data.SummaryItemType.Sum;
					b.ShowInGroupFooterColumn = gvc.FieldName;
					gv1.TotalSummary.Add(b);
					
				}

			}

		}
		if (gv1.Columns["Country"] != null)
		{
	//		gv1.GroupBy(gv1.Columns["Country"], 0);
		}
		if (gv1.Columns["Holdco"] != null)
		{
	//		gv1.GroupBy(gv1.Columns["Holdco"], 1);
		}
		gv1.ExpandAll();
	}
}
