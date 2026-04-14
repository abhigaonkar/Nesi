using System;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using DevExpress.Web.Internal;
using nesi.core;

public partial class sections_reports_po_line_grid_index : Page
	{
	Toolbox _tools;
	NeMember current_user;
	JavaScriptSerializer jSON		= new JavaScriptSerializer();
	static int _page_id			= 115;
	static string _page_name		= "POLineGrid";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools			= new Toolbox();
		current_user	= Toolbox.do_handle_authentication(_page_id);
		layout.__page_name					= _page_name;
		if(Session["working_business_unit_id"] != null)
			{
			Session.Remove("working_business_unit_id");
			}
		h									= (ASPxHiddenField) layout.FindControl("h");
		ds_templates						= (SqlDataSource) layout.FindControl("ds_templates");
		dde_filter							= (ASPxDropDownEdit) layout.FindControl("dde_filter");
		panel_export						= (Panel) layout.FindControl("panel_export");
		panel_export.Visible				= true;
		layout.used_gv			= gv_po_line_grid;
		ds_templates.SelectParameters["@page_name"].DefaultValue	= _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue	= current_user.id.ToString();
		_tools.dont_cache_page();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
			var lbltemp = (Label)Page.Master.FindControl("lblHeading");
			lbltemp.Text = "PO Line Grid";
			if (!IsCallback && !IsPostBack)
			{
				var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
				divMenu.InnerHtml = menu.MenuHTML;
				var gl = new NeGridLayouts(current_user.id, _page_name);
				h.Set("gridview_id", "gv_po_line_grid");
				if (gl.GridLayoutID == 0)
				{
					gv_po_line_grid.FilterExpression = string.Format("[business_unit] = '{0}' AND [active] = 1 ", current_user.business_unit.name);
					gl.GridLayout_Layout = gv_po_line_grid.SaveClientLayout();
					gl.member_id = current_user.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					gv_po_line_grid.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				dde_filter.Text = gl.GridLayout_Name;
			   
				var remove_nesi_cut_po		= current_user.business_unit_id == 11 ? "%" : "0";
               
                var dt = _tools.getSQL_datatable("call get_po_line_grid_data(@v0,@v1) ", new object [] { current_user.id, remove_nesi_cut_po });

				gv_po_line_grid.DataSource = dt;
				
				Session["polinegrid_gv"] = dt;
			}
			else
			{
				gv_po_line_grid.DataSource = Session["polinegrid_gv"];
			}

		gv_po_line_grid.DataBind();
		}
	protected void gv_po_line_grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_po_line_grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void gv_po_line_grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		var gv							= (ASPxGridView) sender;
		var index								= e.VisibleIndex;
		var woid								= gv_po_line_grid.GetDataRow(index)["woid"].ToString();
		var companyid						= gv_po_line_grid.GetDataRow(index)["business_unit_id"].ToString();
		var masterid							= gv_po_line_grid.GetDataRow(index)["master_id"].ToString();
		var link = new HyperLinkDisplayControl();
		if (e.KeyValue != null)
		{
			if (e.CellValue != null)
			{
				if (e.DataColumn.FieldName == "wo")
				{
					if (woid != null)
					{
						
										link = e.Cell.Controls[0] as HyperLinkDisplayControl;
				//						link.NavigateUrl = string.Format("javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 'po', 1200,800)", woid, companyid);
				//						link.Target = "blank";
					}

				}

				else if (e.DataColumn.FieldName == "master_id")
				{
					link = e.Cell.Controls[0] as HyperLinkDisplayControl;
					link.NavigateUrl = string.Format("javascript:boing('/sections/member/inventory/index.aspx?a=get&id={0}&tab=G', 'inventory', 1200,800)", masterid);
					link.Target = "blank";
				}
			}
		}
		}
	protected void header_Init(object sender, EventArgs e)
		{
		var lb		= (ASPxLabel) sender;
		lb.Attributes.Add("data-title", lb.Text.Replace("\"", ""));
		lb.CssClass			= "ttip";
		}
}
