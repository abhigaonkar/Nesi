using System;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;
using System.IO;
using System.Text;
using nesi.core;

public partial class sections_tax_entities_index : System.Web.UI.Page
	{


		
    NeMember current_user;
    private static int _PAGE_ID = 222;  
	private const string _page_name = "Tax_Entities";

	static string default_filter = "";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;
	private static Regex _numeric	= new Regex(@"^\d+$");
	Toolbox _tools;

 
	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		current_user = Toolbox.do_handle_authentication(_PAGE_ID);
		layout.__page_name = _page_name;
		layout.used_gv = gv_te;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		h.Set("gridview_id", "gv_te");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();


	}
	
	protected void Page_Load(object sender, EventArgs e)
		{
		var _q			= Request.QueryString;
		current_user = Toolbox.do_handle_authentication(_PAGE_ID);
        var menu = new NeMenu(current_user, Convert.ToInt32(_PAGE_ID));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
       
       

        if (!IsPostBack)
			{
				
				var gl = new NeGridLayouts(current_user.id, _page_name);
				if (gl.GridLayoutID == 0)
				{
		//			default_filter = "page1|filter[business_unit_id] = " + current_user.business_unit_id + " And [assets_statusid] <> 4|visible11|t0|f1|t2|t3|t4|t5|t6|t7|t8|t9|t10|width11|75px|30px|30px|100px|100px|50px|100px|50px|75px|100px|137px";
		//			gv_assets.LoadClientLayout(default_filter);
		//			gl.GridLayout_Layout = "page1|filter[business_unit_id] = " + current_user.business_unit_id + " And [assets_statusid] <> 4|visible11|t0|f1|t2|t3|t4|t5|t6|t7|t8|t9|t10|width11|75px|30px|30px|100px|100px|50px|100px|50px|75px|100px|137px";
					gl.GridLayout_Layout = gv_te.SaveClientLayout();
					gl.member_id = current_user.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					gv_te.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				dde_filter.Text = gl.GridLayout_Name;

			
				
			}
			
			gv_te.DataBind();
		}
	protected void cp_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{

	}
	protected void ucImage_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
	{
		Session["uploadedFileData"] = e.UploadedFile.FileBytes;
	}

	protected void gv_te_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "")
		{
			gv.LoadClientLayout(e.Parameters);
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
	protected void gv_te_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}










    protected void pop_files_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
        var te_id = e.Parameter;
        div_files.InnerHtml += "<iframe src='/filemanager.aspx?parent_page=_protected&id=" + te_id + "' frameborder='no' width='100%' height='500px' scrolling='auto'></iframe>";


    }

    protected void gv_te_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {

        //    if (e.NewValues["tax1"] != e.OldValues["tax1"])
        //    {
        //        _tools.getSQL_void(@"update CTRL_MAIN set BVSLSTAXNO1=" + e.NewValues["tax1"] + " where TABLE_NO = '1' and TABLE_LIT = '' ", new NeTaxEntity(e.Keys[0]).DSN, null);
        //    }
        //    if (e.NewValues["tax2"] != e.OldValues["tax2"])
        //    {
        //    _tools.getSQL_void(@"update CTRL_MAIN set BVSLSTAXNO2=" + e.NewValues["tax2"] + " where TABLE_NO = '1' and TABLE_LIT = '' ", new NeTaxEntity(e.Keys[0]).DSN, null);
        //}
        //    if (e.NewValues["tax3"] != e.OldValues["tax3"])
        //    {
        //    _tools.getSQL_void(@"update CTRL_MAIN set BVSLSTAXNO3=" + e.NewValues["tax3"] + " where TABLE_NO = '1' and TABLE_LIT = '' ", new NeTaxEntity(e.Keys[0]).DSN, null);
        //}
        //    if (e.NewValues["tax4"] != e.OldValues["tax4"])
        //    {
        //    _tools.getSQL_void(@"update CTRL_MAIN set BVSLSTAXNO4=" + e.NewValues["tax4"] + " where TABLE_NO = '1' and TABLE_LIT = '' ", new NeTaxEntity(e.Keys[0]).DSN, null);
        //}
      

    }
}
