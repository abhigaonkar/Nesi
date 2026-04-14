using System;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class sections_assets_Asset_history : System.Web.UI.Page
	{


		
    NeMember current_user;
    private static int _PAGE_ID = 60;  //tooling and assets Page
	private const string _page_name = "Asset_History";

	static string default_filter = "";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;
	private static Regex _numeric	= new Regex(@"^\d+$");
	private List<int> business_unit_employees;

	protected void Page_Init(object sender, EventArgs e)
	{
		current_user = Toolbox.do_handle_authentication(_PAGE_ID);
		layout.__page_name = _page_name;
		layout.used_gv = gv_assets_history;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		h.Set("gridview_id", "gv_assets_history");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();

	    sqlcompanys.SelectCommand = "Select id, ddl_name company from business_unit where ddl_name is not null and id in (" +
	                                new Current_User().visible_business_units + ") order by ddl_name";
        sqlcompanys.DataBind();
        ASPxComboBox1.DataBind();

	    }
	
	protected void Page_Load(object sender, EventArgs e)
		{
		var _q			= Request.QueryString;
		
			if (!IsPostBack)
			{
				var gl = new NeGridLayouts(current_user.id, _page_name);
				if (gl.GridLayoutID == 0)
				{
					//default_filter = "page1|filter52|[branch] = " + current_user.business_unit_id + " And [statusid] <> 4|visible14|t0|f1|t2|t3|t4|t5|t6|t7|t8|t9|t10|t11|t12|t13|width14|75px|25px|120px|100px|60px|40px|90px|100%|70px|80px|60px|70px|80px|60px";
					gv_assets_history.LoadClientLayout(default_filter);
					gl.GridLayout_Layout = default_filter;
					gl.member_id = current_user.id;
					gl.GridLayout_Name = "Default";
					gl.GridLayout_Gridid = _page_name;
					gl.SaveGridLayout();

					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				else
				{
					gv_assets_history.LoadClientLayout(gl.GridLayout_Layout);
					h.Set("ID", gl.GridLayoutID);
					h.Set("NAME", gl.GridLayout_Name);
				}
				dde_filter.Text = gl.GridLayout_Name;

				if (Session["working_business_unit_id"] == null || !current_user.AuthenticatedForPrivilege(106))
				{
					Session.Add("working_business_unit_id", current_user.business_unit_id.ToString());
				}
				ASPxComboBox1.Value = Convert.ToInt32(Session["working_business_unit_id"]);
				gv_assets_history.FilterExpression = "[branch]=" + Convert.ToInt32(ASPxComboBox1.Value);
			}
			
		}
	protected void cp_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{

	}
	protected void ucImage_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
	{
		Session["uploadedFileData"] = e.UploadedFile.FileBytes;
	}


    protected void gv_assets_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	protected void gv_assets_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		e.Properties["cpExp"] = gv.SaveClientLayout();
	}


	
	
	protected void gv_assets_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
	{
		if (!gv_assets_history.IsNewRowEditing)
		{
			var x = gv_assets_history.GetRowValues(gv_assets_history.EditingRowVisibleIndex, "assets_iconpic").ToString();
			var i = (ASPxImage)gv_assets_history.FindEditFormTemplateControl("img1");

		    var row_id = (int)gv_assets_history.GetRowValues(gv_assets_history.EditingRowVisibleIndex, "assets_history_assetid");
		    var exp = new NeAssets((Int16)row_id);
		    var fileServerExt = NeTaxEntity.BaseFolder(exp.BusinessUnitId, true);
		    var pathExt = fileServerExt + @"/asset_pics/" + exp.IconPic;

            i.ImageUrl = pathExt;
		}
	}

	protected void gv_assets_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{

		var gv = (ASPxGridView)sender;
		var mem = Convert.ToInt32(e.NewValues["assets_history_memberid"]);
		var action = Convert.ToInt32(e.NewValues["assets_history_action"]);
		var dollars = Convert.ToDouble(e.NewValues["assets_history_dollars"]);
		var dte = Convert.ToDateTime(e.NewValues["assets_history_date"]);
		var id = Convert.ToInt32(gv.GetRowValues(gv.EditingRowVisibleIndex, "ID"));
		var notes = e.NewValues["assets_history_notes"] == null ? "" : e.NewValues["assets_history_notes"].ToString();

		Toolbox.doSQL_void(@"Update assets_history 
set assets_history_memberid=@v0  ,
assets_history_action=@v1 ,
assets_history_dollars=@v2 ,
assets_history_date=@v3 ,
assets_history_notes = @v4   
where assets_history_id = @v5",new object[] { mem,action,dollars,dte.ToString("yyyy - MM - dd"),notes, id});

		e.Cancel = true;
		gv.CancelEdit();
		

	}
	protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["working_business_unit_id"] = ASPxComboBox1.Value.ToString();
		var business_unit_id		= Convert.ToInt32(ASPxComboBox1.Value);
		gv_assets_history.FilterExpression = "[branch]=" +business_unit_id ;
		gv_assets_history.DataBind();
	}
	
	
	protected void gv_history_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		Toolbox.doSQL_void(@"Delete from assets_history where assets_history_id =@v0 ", new object[] { e.Keys[0]});
		e.Cancel = true;
		gv.CancelEdit();
		gv.DataBind();
		
	}
	protected void dte_addhistory_DataBound(object sender, EventArgs e)
	{

		var dte = (ASPxDateEdit)sender;
		dte.Date = System.DateTime.Today;
	}
	protected void ddl_addhistorymember_DataBound(object sender, EventArgs e)
	{
		var cb = (ASPxComboBox)sender;
		cb.Value = current_user.id;
	}
	protected void ddlaction_DataBound(object sender, EventArgs e)
	{
		var cb = (ASPxComboBox)sender;
		cb.Value = 1;
	}

	protected void gv_assets_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
	{
		var dte = Convert.ToDateTime(e.NewValues["assets_history_date"]);
		var mem = e.NewValues["assets_history_notes"] == null ? "" : e.NewValues["assets_history_notes"].ToString();
		var ddlaction = Convert.ToInt32(e.NewValues["assets_history_action"]);
		var ddlmember = Convert.ToInt32(e.NewValues["assets_history_memberid"]);
		var txtdollars = Convert.ToDouble(e.NewValues["assets_history_dollars"]);
		var assetid = Convert.ToInt32(e.NewValues["assets_history_assetid"]);


		if ((dte != null))
		{
			Toolbox.doSQL_void(@"Insert into assets_history 
(assets_history_assetid,assets_history_action,assets_history_memberid,assets_history_notes,assets_history_date,assets_history_dollars)
values (@v0,@v1,@v2,@v3,@v4,@v5)", new object[] { assetid,ddlaction,ddlmember, mem , dte.Date.ToString("yyyy-MM-dd") ,txtdollars });
			gv_assets_history.DataBind();
			

		}
		else
		{
			throw new Exception("Missing Information");
		}








		gv_assets_history.CancelEdit();
		e.Cancel = true;

	}


	protected void ASPxPopupControl1_WindowCallback(object source, PopupWindowCallbackArgs e)
	{

	}

	protected void gv_assets_history_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		    var row_id = gv_assets_history.GetRowValues(e.VisibleIndex, "assets_history_assetid") == null ? 0 : Convert.ToInt16(gv_assets_history.GetRowValues(e.VisibleIndex, "assets_history_assetid"));

		    switch (e.DataColumn.Caption)
		        {
		            case "Image":
		                //var exp = new payroll.expense(row_id);
		                var exp = new NeAssets((Int16)row_id);

		                // Check if it has a file
		                if (exp.IconPic != "")
		                    {
		                    //    // retrive business unit of the user who created record
		                    //    var memberCreatedRecord = new NeMember(exp.id_member);

		                    //    // Check if file exists
		                    var fileServer = NeTaxEntity.BaseFolder(exp.BusinessUnitId, false);
		                    var fileServerExt = NeTaxEntity.BaseFolder(exp.BusinessUnitId, true);
		                    var path = fileServer + @"/asset_pics/" + exp.IconPic;
		                    var pathExt = fileServerExt + @"/asset_pics/" + exp.IconPic;

		                    e.Cell.Text = File.Exists(path) ? string.Format(@"<img src='{0}' />", pathExt) : "";
		                    }
		                break;

		        }
    }
	}
