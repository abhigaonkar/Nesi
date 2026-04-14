using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.Script.Serialization;
using System.IO;
using nesi.core;
using System.Web;
using System.Threading;

public partial class modules_layout_control : UserControl
	{
	public NeMember myMember;
	JavaScriptSerializer jSON				= new JavaScriptSerializer();
	public ASPxGridView used_gv				{ get; set; }
	public ASPxGridView UsedGridView
    {
        get { return used_gv; }
        set
        {
            used_gv = value;
            if (used_gv != null)
            {
                // Attach a default callback if no specific callback is provided
                used_gv.CustomCallback += DefaultCustomCallback;
            }
        }
    }

    private void DefaultCustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
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
	public string __page_name				{ get; set; }
	public string __gv_id					= "";
	protected bool is_private				{ get; set; }
	public string GridviewID				{ get; set; }
	public string export_filename			{ get; set; }
	public bool ShowToggle					{ get; set; } = true;
	public bool ShowExcelExport				{ get; set; } = true;
	public bool ShowPDFExport				{ get; set; } = true;
	public string current_layout		    { get; set; }
    protected void Page_Init(object sender, EventArgs e)
		{
		myMember							= Toolbox.do_handle_authentication(1);
		ds_templates.SelectParameters["@member_id"].DefaultValue = myMember.id.ToString();

		//myMember							= new NeMember(Convert.ToInt32(Request.QueryString["member_id"]));
		}
    protected void Page_Load(object sender, EventArgs e)
		{
	    if (!Visible)
		    {
		    return;
		    }
	    if(used_gv == null)
		    {
		    if(!string.IsNullOrEmpty(GridviewID))
			    {
			    used_gv				= (ASPxGridView) Parent.Page.FindControl(GridviewID);
			    if(!h.Contains("gridview_id"))
				    {
				    h.Set("gridview_id", GridviewID);
				    }
			    }
		    else if(h.Contains("gridview_id"))
			    {
			    used_gv			= (ASPxGridView) Parent.Page.FindControl(h.Get("gridview_id").ToString());
			    GridviewID		= string.IsNullOrEmpty(GridviewID) ? h.Get("gridview_id").ToString() : GridviewID;
			    }
		    }
	    if(used_gv != null && !string.IsNullOrEmpty(used_gv.ID))
		    {
		    used_gv.ClientInstanceName									= used_gv.ClientInstanceName == "" && GridviewID != "" ? GridviewID : used_gv.ClientInstanceName != "" ? used_gv.ClientInstanceName : h.Get("gridview_id").ToString();
		    used_gv.SettingsBehavior.EnableCustomizationWindow			= true;
		    used_gv.SettingsPopup.CustomizationWindow.VerticalAlign		= PopupVerticalAlign.TopSides;
		    used_gv.SettingsPopup.CustomizationWindow.HorizontalAlign	= PopupHorizontalAlign.LeftSides;
		    used_gv.Settings.ShowFilterRowMenu							= true;
		    used_gv.Settings.ShowFilterRow								= true;
		    used_gv.Settings.ShowHeaderFilterButton						= true;
		    used_gv.Settings.ShowFilterBar								= GridViewStatusBarMode.Visible;
			used_gv.SettingsPopup.CustomizationWindow.Height			= Unit.Pixel(250);
			used_gv.SettingsPopup.CustomizationWindow.AllowResize		= true;
			used_gv.ClientSideEvents.ColumnResized						= "function(s,e){s.GotoPage(s.GetPageIndex());}";



            //do this for begin and end callback
            if (used_gv.ClientSideEvents.BeginCallback == "")
            {
                used_gv.ClientSideEvents.BeginCallback = "function(s,e){ please_wait('start');}";
            }
            if( used_gv.ClientSideEvents.EndCallback == "")
            {
                //do a please wait. 
                used_gv.ClientSideEvents.EndCallback = "function(s,e){ please_wait('stop');}";
            }

            //check if the code contains please wait before sending the email.
            var beginCallbackContains = used_gv.ClientSideEvents.BeginCallback.ToLower().Contains("please_wait(");
            var endCallbackContains = used_gv.ClientSideEvents.EndCallback.ToLower().Contains("please_wait(");

		    if (!beginCallbackContains || !endCallbackContains)
		    {
		        //send an email that this grid has not been updated yet.
		        SendLoadingPanelError();

		        //if it doesnt stop but it does start. Dont start
		        if (!endCallbackContains && beginCallbackContains)
		        {
                    //Dont start Please_wait, and use default loading panel. 
		            used_gv.ClientSideEvents.BeginCallback = "";
		            used_gv.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.Default;
                }

            }
		    else
		    {
		        used_gv.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.Disabled;
		    }


            
        }

	    var gv						= (ASPxGridView) dde_filter.FindControl("gv_templates");
		ds_templates.SelectParameters["@gv_id"].DefaultValue = used_gv == null ? "" : used_gv.ID;
		try
		    {
		    dde_filter.Text						= gv.GetRowValues(0, "GridviewLayouts_Name") == null ? "" : gv.GetRowValues(0, "GridviewLayouts_Name").ToString();
		    }
	    catch
		    {
		    dde_filter.Text						= "";
		    }
	    bt_toggle_col.Visible			= ShowToggle;
	    var global_excel				= myMember.AuthenticatedForPrivilege(90001);
	    var global_pdf					= myMember.AuthenticatedForPrivilege(90002);
	    if (is_private)
		    {
		    btn_excel.Visible		= global_excel;
		    btn_pdf.Visible		= global_pdf;
		    btn_excel.ClientVisible		= global_excel;
		    btn_pdf.ClientVisible		= global_pdf;
		    }
	    else
		    {
		    btn_excel.Visible			= ShowExcelExport;
		    btn_pdf.Visible				= ShowPDFExport;
		    btn_excel.ClientVisible		= ShowExcelExport;
		    btn_pdf.ClientVisible		= ShowPDFExport;
		    }
		btn_pdf.Visible		= false; // MH: Added because people don't export pdf grids..
	    grab_layout();
		}

	    private void SendLoadingPanelError()
		    {
			Toolbox.do_debug_note($"{used_gv.ClientInstanceName} may not have please_wait() Implemented correctly. <br>{used_gv.ClientSideEvents.BeginCallback}<br><br>{used_gv.ClientSideEvents.EndCallback} - URL: {Request.RawUrl}");
		    }

	    protected void btn_excel_Click(object sender, EventArgs e)
		{
		var n_rows						= 11000000;
		master_exporter.GridViewID		= h.Get("gridview_id").ToString();
		var ii							= master_exporter.GridView.PageCount * master_exporter.GridView.SettingsPager.PageSize;
		if(ii > n_rows)
			{
			Toolbox.FriendlyException(Response, "You have tried to export more rows than are allowed - ("+n_rows.ToString("N0")+")", "history.go(-1)");
			}
		else
			{
			if(master_exporter.GridViewID == "gv_rfq" || master_exporter.GridViewID == "gv_details")
				{
				using (var ms = new MemoryStream())
					{
					master_exporter.WriteXls(ms);
					Response.Clear();
					Response.Buffer = false;
					Response.AppendHeader("Content-Type", "application/xls");
					Response.AppendHeader("Content-Transfer-Encoding", "binary");
					Response.AppendHeader("Content-Disposition", "attachment; filename="+GridviewID+".xls");
					Response.BinaryWrite(ms.GetBuffer());
					Context.ApplicationInstance.CompleteRequest();
					}
				}
			else
				{
				try
					{
					var fileName = string.IsNullOrEmpty(export_filename)
										? $"{master_exporter.GridViewID}-{Toolbox.MySQLNow_short()}-{DateTime.Now.Ticks}.xlsx"
										: $"{Path.GetFileNameWithoutExtension(export_filename)}-{DateTime.Now.Ticks}.xlsx";
					var path						= $"{AppDomain.CurrentDomain.BaseDirectory}\\App_Data\\tempexp{fileName}";
				
					long length = 0;
					using (var fs = new FileStream(path, FileMode.CreateNew))
						{
						master_exporter.WriteXlsx(fs);
						length	= fs.Length;
						}
					Response.Clear();
					Response.Buffer = false;
					Response.AppendHeader("Content-Type", "application/xlsx");
					Response.AppendHeader("Content-Transfer-Encoding", "binary");
					Response.AddHeader("Content-Length", length.ToString());
					Response.AppendHeader("Content-Disposition","attachment; filename="+fileName);
					Response.WriteFile(path);
					Response.End();
					}
				catch (ThreadAbortException )
					{
					//this is special for the Response.end exception
					}
				catch (Exception ee)
					{
					throw ee;
					}
				}
			}

		}
	protected void btn_pdf_Click(object sender, EventArgs e)
		{
		var n_rows						= 11000000;
		master_exporter.GridViewID		= h.Get("gridview_id").ToString();
		var ii							= master_exporter.GridView.PageCount * master_exporter.GridView.SettingsPager.PageSize;
		if(ii > n_rows)
			{
			Toolbox.FriendlyException(Response, "You have tried to export more rows than are allowed - ("+n_rows.ToString("N0")+")", "history.go(-1)");
			}
		else
			{
		master_exporter.GridViewID		= h.Get("gridview_id").ToString();
		if(master_exporter.GridViewID == "gv_rfq" || master_exporter.GridViewID == "gv_details")
			{
			using (var ms = new MemoryStream())
				{
				master_exporter.WritePdf(ms);
				Response.Clear();
				Response.Buffer = false;
				Response.AppendHeader("Content-Type", "application/xls");
				Response.AppendHeader("Content-Transfer-Encoding", "binary");
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + GridviewID + ".pdf");
				Response.BinaryWrite(ms.GetBuffer());
				Context.ApplicationInstance.CompleteRequest();
				}
			}
		else
			{
			try
				{

					var filename = GridviewID;
					if (export_filename != null)
					{
						filename = export_filename;
					}

				if(master_exporter.GridViewID == "gv_orders")
					{
					master_exporter.Landscape		= true;
					master_exporter.GridView.Columns["Vend. Part #"].ExportWidth		= 250;
					master_exporter.GridView.Columns["Vendor"].ExportWidth				= 250;
					master_exporter.GridView.Columns["Default Vendor"].ExportWidth		= 250;
					}

				var filename2 = $"{master_exporter.GridViewID}-{Toolbox.MySQLNow_short()}-{DateTime.Now.Ticks}.pdf";
				var path = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\exports\\" + filename2;
				if (File.Exists(filename2)) File.Delete(filename2);
				var fs = new FileStream(path, FileMode.CreateNew);
				master_exporter.WritePdf(fs);
				var length = fs.Length;
				fs.Close();
				fs.Dispose();
				Response.Redirect("/App_Data/exports/" + filename2);

				//master_exporter.FileName = filename;
				//master_exporter.WritePdfToResponse();
				}
			catch (Exception ee)
				{
				throw ee;
				}
			}
			}
		}

	private void grab_layout()
		{
		if(used_gv != null)
			{
			__gv_id			= used_gv.ID;
			h.Set("gridview_id", __gv_id);
			if(__page_name == "")
				{
				Toolbox.do_debug_note($"Grid detected without __page_name set - __gv_id: {__gv_id}, current URL: {Request.RawUrl}");
				__page_name = __gv_id;
				}
			if(__gv_id != __page_name) // gv_jobcost {bad} != JobCostReport {good}
				{
				// Check for existing GV layouts with incorrect grid id
				var existingLayouts		= NeGridLayouts.ExistingIncorrectLayoutIds(__gv_id);
				// Move all existing incorrect grid id's (__gv_id) to correct grid id (__page_name)
				if(!string.IsNullOrEmpty(existingLayouts))
					{
					NeGridLayouts.CorrectExistingLayoutGridIds(existingLayouts, __page_name);
					CorrectedLayouts.Visible = true;
					}
				}
			GridviewID		= __gv_id;
			}
		else
			{
			var p								= Parent is Page ? Parent : Parent.Page;
			if(!string.IsNullOrEmpty(GridviewID))
				{
				used_gv					= (ASPxGridView) p.FindControl(GridviewID);
				if(!h.Contains("gridview_id"))
					{
					h.Set("gridview_id", GridviewID);
					}
				__gv_id			= GridviewID;
				}
			else if(h.Contains("gridview_id"))
				{
				used_gv			= (ASPxGridView) p.FindControl(h.Get("gridview_id").ToString());
				GridviewID		= string.IsNullOrEmpty(GridviewID) ? h.Get("gridview_id").ToString() : GridviewID;
				__gv_id			= GridviewID;
				}

			}

		current_layout				= used_gv != null ? used_gv.SaveClientLayout() : "";
		}
	protected void gv_filters_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
        var NAMEs				= new object[gv.VisibleRowCount];
        var IDs				= new object[gv.VisibleRowCount];
        var LAYOUTs			= new object[gv.VisibleRowCount];
        var ISOWNERs			= new object[gv.VisibleRowCount];
        var OWNERs				= new object[gv.VisibleRowCount];
        using(var conn = Toolbox.connect())
			{ 
			for(var i = 0; i < gv.VisibleRowCount; i++)
				{
				var memberId			= gv.GetDataRow(i)["GridviewLayouts_member_id"];
				NAMEs[i]				= gv.GetDataRow(i)["GridviewLayouts_Name"];
				IDs[i]					= gv.GetDataRow(i)["gridviewlayouts_id"];
				LAYOUTs[i]				= gv.GetDataRow(i)["GridviewLayout_Layout"];
				ISOWNERs[i]				= memberId.ToString() == myMember.id.ToString();
				var firstname			= memberId == null ? "Not Set" : Toolbox.doSQL_string(conn, @"SELECT IFNULL(MAX(member_nickname), '') FROM member  WHERE member_id =@v0", new object[] { memberId });
				OWNERs[i]				= firstname;
				}
			}
        e.Properties["cpNAMEs"]		= NAMEs;
        e.Properties["cpIDs"]		= IDs;
        e.Properties["cpLAYOUTs"]	= LAYOUTs;
        e.Properties["cpISOWNERs"]	= ISOWNERs;
        e.Properties["cpOWNERs"]	= OWNERs;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		}
	protected void cb_save_template_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
		{
		
		}
	protected void cb_save_template_Callback(object source, CallbackEventArgs e)
		{
		if(e.Parameter.Contains("^"))
			{
			var paras		= e.Parameter.Split('^');
			if(paras[0] == "setdefault")
				{
				try
					{
					int.TryParse(paras[1], out int layoutId);
					var this_company		= new NeBusinessUnit(Convert.ToInt32(Session["working_business_unit_id"]));
					var gridViewLayout		= new NeGridLayouts(layoutId);
					var gridId				= gridViewLayout.GridLayout_Gridid;
					NeGridLayouts.HandleDefaultLayout(layoutId, myMember.id, gridId);
					e.Result				= $"You have successfully set the default layout {gridViewLayout.GridLayout_Name} for this report - {gridId}";
					}
				catch (Exception ee)
					{
					e.Result				= "There was a problem saving this layout as the default. Please contact the IT staff";
					Toolbox.do_errorLog_errorStack(ee);
					}
				}
			}
		else
			{
			var t					= jSON.Deserialize<Template>(e.Parameter);
			var this_company		= new NeBusinessUnit(Convert.ToInt32(Session["working_business_unit_id"]));
			var gl			= string.IsNullOrEmpty(t.id) ? new NeGridLayouts() : new NeGridLayouts(Convert.ToInt32(t.id));
			var copying_layout			= false;
			var returned				= "";
			var gl_user			= new NeMember();
			if(gl.member_id != myMember.id && t.type == "delete")
				{
				returned				= "You cannot delete a layout that isn't yours.";
				e.Result = gl.GridLayoutID + ",invalid," + returned;
				}
			else
				{
				if(!string.IsNullOrEmpty(t.id) && gl.member_id != myMember.id)
					{
					gl_user					= new NeMember((int) gl.member_id);
					gl						= new NeGridLayouts();
					copying_layout			= true;
					}
				switch(t.type)
					{
					case "save":
						returned			= "Successfully saved layout '"+t.name+"'";
					break;
					case "copy":
						returned			= "Successfully copied layout '"+t.name+"'";
					break;
					case "delete":
						returned			= "Successfully deleted layout '"+gl.GridLayout_Name+"'.. refreshing page.";
					break;
					}
				if(t.type == "delete")
					{
					Toolbox.doSQL_void(@"DELETE FROM gridviewlayouts WHERE gridviewlayouts_id =@v0 LIMIT 1 " , new object[] { gl.GridLayoutID});
					e.Result = gl.GridLayoutID + ",," + returned;
					}
				else
					{
					grab_layout();
					gl.GridLayout_Layout		= t.exp == null || t.exp.Trim() == "" ? current_layout : t.exp;
					gl.member_id	= myMember.id;
					if(t.name.Contains("Default") && copying_layout)
						{
						t.name					= gl_user.FirstName+"'s Default";
						var exists				= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(gridviewlayouts_id),0) FROM gridviewlayouts  WHERE gridviewlayouts_name = @v0 AND gridviewlayouts_member_id =@v1 ",
							new object[] { t.name,myMember.id });
						if(exists != 0)
							{
							gl					= new NeGridLayouts(exists);
							}
						}
					gl.GridLayout_Name			= t.name;
					gl.GridLayout_Gridid		= __page_name;
					gl.SaveGridLayout();
					e.Result					= gl.GridLayoutID+","+gl.GridLayout_Name+","+returned;
					}
			}
			}
		}
	protected void master_exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
		{
		var gve	= (ASPxGridViewExporter) sender;
		var gv				= gve.GridView;
		gve.FileName				= GridviewID;
		if(e.RowType == GridViewRowType.Data)
			{
			if(e.Column.Name == "wo_parts" && e.VisibleIndex >= 0)
				{
				var master_id		= gv.GetRowValues(e.VisibleIndex, "master_id").ToString();
				try
					{

					//todo division table doesnt exist
                    var wo_c	= Toolbox.doSQL_dt(@" SELECT b.woprog_id, b.woprog_bvwo, c.customer_name, IFNULL(a.wo_detail_current_qty_ordered,0) - IFNULL(a.wo_detail_current_qty_committed,0) qty_needed, member_name(b.woprog_pm_memberid) pm, CAST(IFNULL(wo_detail_current_date_required,'N/A') AS CHAR) reqdate FROM wo_detail_current a LEFT JOIN woprog b ON a.wo_detail_current_woprog_id = b.woprog_id LEFT JOIN customer c ON b.woprog_customer_id = c.customer_id WHERE wo_detail_current_master_id = @v0  AND business_unit_id = @v1  AND b.woprog_status = 'Open' AND b.woprog_hold != TRUE AND (IFNULL(a.wo_detail_current_qty_ordered,0) - IFNULL(a.wo_detail_current_qty_committed,0)) > 0", new object[] {  master_id, Session["working_business_unit_id"] } );
					var res		= "";
					if(wo_c.Rows.Count > 0)
						{
						// bvwo#, woprog_id, customer name, department
						foreach(DataRow r in wo_c.Rows)
							{
							var wo_id	= r["woprog_id"].ToString();
							var wo_n		= r["woprog_bvwo"].ToString();
							var cust		= r["customer_name"].ToString();
							var qty		= Convert.ToDouble(r["qty_needed"]);
							var pm		= r["pm"].ToString();
							var reqdate	= r["reqdate"].ToString();
							res				+= string.Format("Date Required: {7} | ({4}) {5} - {2} Needed - {6}\n", wo_id, Session["working_business_unit_id"], qty, null, wo_n, cust, pm, reqdate);
							}
						e.Text		= res;
						}
					}
				catch (Exception ee)
					{
					Toolbox.do_errorLog_errorStack(ee);
					throw;
					}
				}
			else if(e.Column.Caption == "Vendor" && e.VisibleIndex >= 0)
				{
				var vendor_name		= Toolbox.do_value_from(gv.GetRowValues(e.VisibleIndex, "vendor_name"), false);
				e.Text					= vendor_name;
				}
			else if(e.Column.Caption == "Vend. Part #" && e.VisibleIndex >= 0)
				{
				var vendor_id		= gv.GetRowValues(e.VisibleIndex, "vendor_id").ToString();
				var vendor_code		= Toolbox.do_value_from(gv.GetRowValues(e.VisibleIndex, "vendor_code"), false);
				e.Text					= vendor_code;
				}
			else if(e.Column.Caption == "Vend. Price" && e.VisibleIndex >= 0)
				{
				double cost				= 0;
				double.TryParse(gv.GetRowValues(e.VisibleIndex, "cost").ToString(), out cost);
				e.Text					= cost.ToString("C2");
				}
			else if(e.Column.Caption == "QTY per" && e.VisibleIndex >= 0)
				{
				double qty_per			= 0;
				double.TryParse(gv.GetRowValues(e.VisibleIndex, "qty_per").ToString(), out qty_per);
				e.Text					= qty_per.ToString("N2");
				}
			else if(e.Column.Caption == "Vend. Lead Time" && e.VisibleIndex >= 0)
				{
				var lead				= 0;
				int.TryParse(gv.GetRowValues(e.VisibleIndex, "lead").ToString(), out lead);
				e.Text					= lead.ToString();
				}
			}
		}
	protected class Template
		{
		public string id     { get; set; }
		public string type   { get; set; }
		public string name   { get; set; }
		public string exp    { get; set; }
		public string gv     { get; set; }
		}
	protected void gv_templates_Load(object sender, EventArgs e)
		{
		var gv			= (ASPxGridView) sender;
		var fullName = myMember.FullName.Replace("'", "''");
        gv.FilterExpression		= "[owner] = '"+ fullName + "'";
		}
	protected void ds_templates_Init(object sender, EventArgs e)
		{
		var ds = (SqlDataSource) sender;
		}

	}
