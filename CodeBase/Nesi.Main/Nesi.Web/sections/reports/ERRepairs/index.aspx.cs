using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class ERRepairs : System.Web.UI.Page
	{
	private NeMember myMember;
	private const int _page_id		= 110; // from Page table in DB
    private const string _page_name = "ERRepairs";
    static string default_filter = "";
	Toolbox _tools;
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	
    protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(_page_id);
		layout.__page_name					= _page_name;
		layout.used_gv						= gv_er;
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		h.Set("gridview_id", "gv_er");
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= myMember.id.ToString();
		}
    
	protected void Page_Load(object sender, EventArgs e)
		{
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);

		if (!IsPostBack)
			{
			var gl	= new NeGridLayouts(myMember.id, _page_name);
			if(gl.GridLayoutID == 0)
				{
                default_filter = "page1|filter[branch_id] = " + myMember.business_unit_id;
				gv_er.LoadClientLayout(default_filter);
                gl.GridLayout_Layout = "page1|filter[branch_id] = " + myMember.business_unit_id;
				gl.member_id	= myMember.id;
				gl.GridLayout_Name			= "Default";
				gl.GridLayout_Gridid		= _page_name;
				gl.SaveGridLayout();
				
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_er.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text					= gl.GridLayout_Name;
			}
		}


	protected void Aspxgridview1_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
		{
		if (gv_er.GetRowValues(e.VisibleIndex, "notes").ToString().Length > 0)
			{
			// some condition
			// hide the Edit button
			if (e.ButtonType == ColumnCommandButtonType.Edit)
				{
				e.Image.Url = "~/images/FullNotes.JPG";
				e.Image.ToolTip = gv_er.GetRowValues(e.VisibleIndex, "notes").ToString();
				}
			}
		}

	protected void Aspxgridview1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		
		if (e.DataColumn.FieldName == "notes" || e.DataColumn.FieldName == "description")
			{
			e.Cell.ToolTip = Toolbox.ReturnBlankIfNull_string(e.CellValue);
			}
		}

	protected void hl_quote_Init(object sender, EventArgs e)
		{
		var container = ((ASPxHyperLink) sender).NamingContainer as GridViewDataItemTemplateContainer;
		var this_quote_id						= gv_er.GetDataRow(container.VisibleIndex)["quote_id"].ToString();
		var this_revision						= gv_er.GetDataRow(container.VisibleIndex)["rev"].ToString();
		((ASPxHyperLink) sender).NavigateUrl		= string.Format("javascript:q({0}, {1})", this_quote_id, this_revision);
		}
    protected void hl_wo_Init(object sender, EventArgs e)
    {
        var container = ((ASPxHyperLink)sender).NamingContainer as GridViewDataItemTemplateContainer;
        var this_woprog_id = gv_er.GetDataRow(container.VisibleIndex)["woprog_id"].ToString();
        ((ASPxHyperLink)sender).NavigateUrl = string.Format("javascript:w({0})", this_woprog_id);
    }
    protected void hl_part_Init(object sender, EventArgs e)
    {
        var container = ((ASPxHyperLink)sender).NamingContainer as GridViewDataItemTemplateContainer;
        var this_master_id = gv_er.GetDataRow(container.VisibleIndex)["master_id"].ToString();
        ((ASPxHyperLink)sender).NavigateUrl = string.Format("javascript:i({0})", this_master_id);
    }
	protected void cbp_edit_note_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
      }
    protected void gv_er_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
    protected void gv_er_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		if(e.Parameters != "")
			{
			gv.LoadClientLayout(e.Parameters);
			}
		else
			{
			gv.FilterExpression		= "";
			for(var i = 0; i < gv.Columns.Count; i++)
				{
				if (gv.Columns[i] is GridViewDataColumn)
					{
					var col = (GridViewDataColumn) gv.Columns[i];
					if (col.GroupIndex > -1)
						{
						gv.UnGroup(col);
						}
					col.Visible = true;
					}
				}
			}
		}
   
}
