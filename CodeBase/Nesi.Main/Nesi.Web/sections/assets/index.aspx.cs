using System;
using System.IO;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_assets_index : System.Web.UI.Page
	{
	NeMember current_user;
	private static int _PAGE_ID = 60; //tooling and assets Page
	private const string _page_name = "MainAssets";

	static string default_filter = "";
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	ASPxButton btn_export;
	ASPxButton btn_excel;


	protected void Page_Init(object sender, EventArgs e)
		{
		current_user = Toolbox.do_handle_authentication(_PAGE_ID);
		ds_business_units.SelectCommand = "Select id, IFNULL(ddl_name, description) name from business_unit where id in (" + new Current_User().visible_business_units + ") and active ='T' order by name";
		ds_business_units.DataBind();

		layout.__page_name = _page_name;
		layout.myMember = current_user;
		layout.used_gv = gv_assets;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		btn_export = (ASPxButton)layout.FindControl("btn_pdf");
		btn_export.Visible = false;
		btn_excel = (ASPxButton)layout.FindControl("btn_excel");
		btn_excel.Visible = false;
		h.Set("gridview_id", "gv_assets");
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		sqlparent_assets.SelectCommand = @"";

		}

	protected void Page_Load(object sender, EventArgs e)
		{
		if (!IsPostBack)
			{
			if (Session["working_business_unit_id"] == null || !current_user.AuthenticatedForPrivilege(106))
				{
				Session.Add("working_business_unit_id", current_user.business_unit_id.ToString());
				}
			var gl = new NeGridLayouts(Convert.ToInt32(current_user.id), _page_name);
			if (gl.GridLayoutID == 0)
				{
				gv_assets.FilterExpression = "[business_unit_id]=" + Convert.ToInt32(Session["working_business_unit_id"]);
				gl.GridLayout_Layout = gv_assets.SaveClientLayout();
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{
				gv_assets.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			dde_filter.Text = gl.GridLayout_Name;

			}
		gv_assets.DataBind();
		}

	protected void cp_Callback(object sender, CallbackEventArgsBase e)
		{
		}

	protected void ucImage_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
		{
		Session["uploadedFileData"] = e.UploadedFile.FileBytes;
		}

	protected void gv_assets_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{

		var row_id = gv_assets.GetRowValues(e.VisibleIndex, "assets_ID") == null ? 0 : Convert.ToInt16(gv_assets.GetRowValues(e.VisibleIndex, "assets_ID"));

		switch (e.DataColumn.Caption)
			{
			case "Pic":
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


	protected void upload_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
		{
		var fu = (ASPxUploadControl)sender;
		var file = fu.UploadedFiles[0];
		var id = Toolbox.ReturnZeroIfNull_int(gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_ID"));
		var AssetObj = new NeAssets(id);
		var ext = Path.GetExtension(file.FileName);
		var filename = id + ext;
		if (fu.UploadedFiles[0].FileName != null)
			{
			Toolbox.doSQL_void(@"UPDATE assets SET assets_iconpic=@v0 WHERE assets_id=@v1 ", new object[] { filename, id });
			}
		var fileServer = NeTaxEntity.BaseFolder(AssetObj.BusinessUnitId, false);
		var fileServerExt = NeTaxEntity.BaseFolder(AssetObj.BusinessUnitId, true);
		file.SaveAs(fileServer + @"\asset_pics\" + filename, true);
		e.CallbackData = fileServerExt + "/asset_pics/" + filename;
		}

	protected void gv_assets_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var pcAsset = (ASPxPageControl) gv_assets.FindEditFormTemplateControl("pc_Asset");
		var pcScheduleLog = (ASPxPageControl)pcAsset.FindControl("pcScheduleLog");
		var gv_history = (ASPxGridView)pcScheduleLog.FindControl("gv_history");
		var gv_calibration = (ASPxGridView)pcScheduleLog.FindControl("gv_calibration");

		var sdsAssetSchedule = (SqlDataSource)pcAsset.FindControl("sdsAssetSchedule");
		var sdsHistoryCalibration = (SqlDataSource)pcAsset.FindControl("sdsHistoryCalibration");

		var edit_branch					= (ASPxComboBox) pcAsset.FindControl("edit_branch");
		var edit_owner					= (ASPxComboBox) pcAsset.FindControl("edit_owner");
		var edit_type					= (ASPxComboBox) pcAsset.FindControl("edit_type");
		var edit_status					= (ASPxComboBox) pcAsset.FindControl("edit_status");
		var edit_make					= (ASPxTextBox)	pcAsset.FindControl("edit_make");
		var edit_model					= (ASPxTextBox)	pcAsset.FindControl("edit_model");
		var edit_transponder			= (ASPxTextBox)	pcAsset.FindControl("edit_transponder");
		var edit_mileage				= (ASPxTextBox)	pcAsset.FindControl("edit_mileage");
		var edit_vin					= (ASPxTextBox)	pcAsset.FindControl("edit_vin");
		var edit_year					= (ASPxTextBox)	pcAsset.FindControl("edit_year");
		var edit_leaseno				= (ASPxTextBox)	pcAsset.FindControl("edit_leaseno");
		var edit_daily					= (ASPxTextBox)	pcAsset.FindControl("edit_daily");
		var edit_weekly					= (ASPxTextBox)	pcAsset.FindControl("edit_weekly");
		var edit_monthly				= (ASPxTextBox)	pcAsset.FindControl("edit_monthly");
		var edit_calibrationfrequency	= (ASPxTextBox)	pcAsset.FindControl("edit_calibrationfrequency");
		var edit_nextcalibrationdate	= (ASPxDateEdit) pcAsset.FindControl("edit_nextcalibrationdate");
		var edit_capped					= (ASPxCheckBox) pcAsset.FindControl("edit_capped");
		var edit_active					= (ASPxCheckBox) pcAsset.FindControl("edit_active");
		var edit_requiresmaintenance	= (ASPxCheckBox) pcAsset.FindControl("edit_requiresmaintenance");
		var edit_needscalibration		= (ASPxCheckBox) pcAsset.FindControl("edit_needscalibration");
		var edit_showonscheduler		= (ASPxCheckBox) pcAsset.FindControl("edit_showonscheduler");
		var edit_description			= (ASPxMemo) pcAsset.FindControl("edit_description");
		var edit_notes					= (ASPxMemo) pcAsset.FindControl("edit_notes");
		var edit_serial_license			= (ASPxTextBox)pcAsset.FindControl("edit_serial_license");
		var edit_number					= (ASPxTextBox)pcAsset.FindControl("edit_number");

		if (!gv_assets.IsNewRowEditing)
			{
			var row_id = (int)gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_ID");
			var ThisAsset						= new NeAssets(row_id);
			edit_branch.Value					= ThisAsset.BusinessUnitId;
			edit_owner.Value					= ThisAsset.Owner;
			edit_type.Value						= ThisAsset.Type;
			edit_status.Value					= ThisAsset.StatusId;
			edit_make.Text						= ThisAsset.Make;
			edit_model.Text						= ThisAsset.Model;
			edit_transponder.Text				= ThisAsset.Transponder;
			edit_mileage.Text					= ThisAsset.Mileage == 0 ? "" : ThisAsset.Mileage.ToString();
			edit_vin.Text						= ThisAsset.VIN;
			edit_year.Text						= ThisAsset.Year == 0 ? "" : ThisAsset.Year.ToString();
			edit_leaseno.Text					= ThisAsset.LeaseNumber;
			edit_daily.Text						= ThisAsset.Daily == 0 ? "" : ThisAsset.Daily.ToString();
			edit_weekly.Text					= ThisAsset.Weekly == 0 ? "" : ThisAsset.Weekly.ToString();
			edit_monthly.Text					= ThisAsset.Monthly == 0 ? "" : ThisAsset.Monthly.ToString();
			edit_calibrationfrequency.Text		= ThisAsset.CalibrationFrequency == 0 ? "" : ThisAsset.CalibrationFrequency.ToString();
			edit_nextcalibrationdate.Date		= ThisAsset.NextCalibrationDate ?? default(DateTime);
			edit_capped.Checked					= ThisAsset.Capped;
			edit_active.Checked					= ThisAsset.Active;
			edit_requiresmaintenance.Checked	= ThisAsset.RequiresMaintenance;
			edit_needscalibration.Checked		= ThisAsset.NeedsCalibration;	
			edit_showonscheduler.Checked 		= ThisAsset.ShowOnScheduler;
			edit_description.Text 				= ThisAsset.Description;
			edit_notes.Text						= ThisAsset.Notes;
			edit_serial_license.Text			= ThisAsset.License;
			edit_number.Text				    = ThisAsset.Number;
			sdsAssetSchedule.SelectParameters[0].DefaultValue = row_id.ToString();
			gv_history.DataBind();

			sdsHistoryCalibration.SelectParameters[0].DefaultValue = row_id.ToString();
			gv_calibration.DataBind();

			if (gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_iconpic") != null)
				{
				var x = gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_iconpic").ToString();
				var i = (ASPxImage)pcAsset.FindControl("img1");

				var exp = new NeAssets(row_id);
				var fileServerExt = NeTaxEntity.BaseFolder(exp.BusinessUnitId, true);
				var pathExt = fileServerExt + @"/asset_pics/" + exp.IconPic;

				i.ImageUrl = pathExt;
				}
			var gv_hist = (ASPxGridView)pcScheduleLog.FindControl("gv_history");
			if (gv_hist != null && gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_ID") != null)
				{
				gv_hist.FilterExpression = "[assets_history_assetid] = " +
										   gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_ID");
				}

			}
		else
			{
			pcAsset.TabPages[1].ClientVisible = false;
			pcAsset.TabPages[2].ClientVisible = false;
			Session["assetid"] = null;
			}
		}

	protected void gv_assets_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		Save(e);
		}
	private void Save(DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		var Cancel = e.Cancel;
		Save(ref Cancel);
		e.Cancel = Cancel;
		}
	private void Save(DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var Cancel = e.Cancel;
		Save(ref Cancel);
		e.Cancel = Cancel;
		}
	private void Save(ref bool Cancel)
		{
		var pcAsset = (ASPxPageControl) gv_assets.FindEditFormTemplateControl("pc_Asset");
		
		var row_id						= gv_assets.IsEditing && !gv_assets.IsNewRowEditing
											? (int) gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_ID") 
											: 0;

		var edit_branch					= (ASPxComboBox) pcAsset.FindControl("edit_branch");
		var edit_owner					= (ASPxComboBox) pcAsset.FindControl("edit_owner");
		var edit_type					= (ASPxComboBox) pcAsset.FindControl("edit_type");
		var edit_status					= (ASPxComboBox) pcAsset.FindControl("edit_status");
		var edit_make					= (ASPxTextBox)	pcAsset.FindControl("edit_make");
		var edit_model					= (ASPxTextBox)	pcAsset.FindControl("edit_model");
		var edit_transponder			= (ASPxTextBox)	pcAsset.FindControl("edit_transponder");
		var edit_mileage				= (ASPxTextBox)	pcAsset.FindControl("edit_mileage");
		var edit_vin					= (ASPxTextBox)	pcAsset.FindControl("edit_vin");
		var edit_year					= (ASPxTextBox)	pcAsset.FindControl("edit_year");
		var edit_leaseno				= (ASPxTextBox)	pcAsset.FindControl("edit_leaseno");
		var edit_daily					= (ASPxTextBox)	pcAsset.FindControl("edit_daily");
		var edit_weekly					= (ASPxTextBox)	pcAsset.FindControl("edit_weekly");
		var edit_monthly				= (ASPxTextBox)	pcAsset.FindControl("edit_monthly");
		var edit_calibrationfrequency	= (ASPxTextBox)	pcAsset.FindControl("edit_calibrationfrequency");
		var edit_nextcalibrationdate	= (ASPxDateEdit) pcAsset.FindControl("edit_nextcalibrationdate");
		var edit_capped					= (ASPxCheckBox) pcAsset.FindControl("edit_capped");
		var edit_active					= (ASPxCheckBox) pcAsset.FindControl("edit_active");
		var edit_requiresmaintenance	= (ASPxCheckBox) pcAsset.FindControl("edit_requiresmaintenance");
		var edit_needscalibration		= (ASPxCheckBox) pcAsset.FindControl("edit_needscalibration");
		var edit_showonscheduler		= (ASPxCheckBox) pcAsset.FindControl("edit_showonscheduler");
		var edit_description			= (ASPxMemo) pcAsset.FindControl("edit_description");
		var edit_notes					= (ASPxMemo) pcAsset.FindControl("edit_notes");
		var edit_serial_license			= (ASPxTextBox)pcAsset.FindControl("edit_serial_license");
		var edit_number					= (ASPxTextBox)pcAsset.FindControl("edit_number");
		var AssetObj = new NeAssets
			{
			Id = row_id,
			BusinessUnitId = (int)edit_branch.Value,
			Owner = (int)edit_owner.Value,
			Type = (int) edit_type.Value,
			StatusId = (int) edit_status.Value,
			Make = edit_make.Text,
			Model = edit_model.Text,
			Transponder = edit_transponder.Text,
			Mileage = Toolbox.ReturnZeroIfNull_int(edit_mileage.Text),
			VIN = edit_vin.Text,
			Year = Toolbox.ReturnZeroIfNull_int(edit_year.Text),
			LeaseNumber = edit_leaseno.Text,
			Daily = Toolbox.ReturnZeroIfNull_decimal(edit_daily.Text),
			Weekly = Toolbox.ReturnZeroIfNull_decimal(edit_weekly.Text),
			Monthly = Toolbox.ReturnZeroIfNull_decimal(edit_monthly.Text),
			CalibrationFrequency = Toolbox.ReturnZeroIfNull_int(edit_calibrationfrequency.Text),
			NextCalibrationDate = edit_nextcalibrationdate.Date,
			Capped = edit_capped.Checked,
			Active = edit_active.Checked,
			RequiresMaintenance = edit_requiresmaintenance.Checked,
			NeedsCalibration = edit_needscalibration.Checked,
			ShowOnScheduler = edit_showonscheduler.Checked,
			Description = edit_description.Text,
			Notes = edit_notes.Text,
			License = edit_serial_license.Text,
			Number= edit_number.Text
			};


		if (string.IsNullOrWhiteSpace(AssetObj.Description))
			{
			var typeName = "";
			if (AssetObj.Type > 0)
				{
				var sql = @"select asset_type_name from assets_type where asset_type_id = @v0";
				typeName = Toolbox.doSQL_string(sql, new object[] { AssetObj.Type });
				}

			//Default the description on current assets using this format - {TYPE} - {MAKE} {MODEL}
			var newDes = string.Format("{0} - {1} {2}", typeName, AssetObj.Make, AssetObj.Model);
			AssetObj.Description = newDes;
			}

		AssetObj.Save(current_user);


		gv_assets.CancelEdit();
		gv_assets.DataBind();
		if(row_id == 0)
			{ 
			var x = gv_assets.FindVisibleIndexByKeyValue(AssetObj.Id);
			gv_assets.StartEdit(x);
			}
		Cancel = true;
		}

	protected void gv_history_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		var dte = (ASPxDateEdit)gv.FindTitleTemplateControl("dte_addhistory");
		var mem = (ASPxMemo)gv.FindTitleTemplateControl("memaddhistorynotes");
		var ddlaction = (ASPxComboBox)gv.FindTitleTemplateControl("ddlaction");
		var ddlmember = (ASPxComboBox)gv.FindTitleTemplateControl("ddl_addhistorymember");
		var txtdollars = (ASPxTextBox)gv.FindTitleTemplateControl("txtdollars");

		if (string.IsNullOrWhiteSpace(txtdollars.Text))
			{
			txtdollars.Text = "0.0";
			// throw new Exception("Price is required");
			}

			Toolbox.doSQL_void(
				@"Insert into assets_history 
(assets_history_assetid,assets_history_action,assets_history_memberid,assets_history_notes,assets_history_date,assets_history_dollars) 
values (@v0,@v1,@v2,@v3,@v4,@v5)", new object[] {
				gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_ID") , ddlaction.Value ,
				ddlmember.Value , mem.Text, dte.Date.ToString("yyyy-MM-dd"),
				txtdollars.Text});
			gv.DataBind();
			dte.Date = DateTime.Today;
			mem.Text = "";
			txtdollars.Text = "";
			ddlmember.Value = Convert.ToInt32(current_user.id);
		}
	protected void gv_calibration_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv         = (ASPxGridView)sender;
		var assetId    = Convert.ToInt32(gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_ID"));
		var dte        = (ASPxDateEdit)gv.FindTitleTemplateControl("dte_addhistory_calibration");
		var mem        = (ASPxMemo)gv.FindTitleTemplateControl("memaddhistorynotes_calibration");
		var ddlaction  = (ASPxComboBox)gv.FindTitleTemplateControl("ddlaction_calibration");
		var ddlmember  = (ASPxComboBox)gv.FindTitleTemplateControl("ddl_addhistorymember_calibration");
		var txtdollars = (ASPxTextBox)gv.FindTitleTemplateControl("txtdollars_calibration");
		var actionId   = Convert.ToInt32(ddlaction.Value);
		var memberId   = Convert.ToInt32(ddlmember.Value);
		var price	   = Toolbox.ReturnZeroIfNull_double(txtdollars.Text);

		NeAssets.AddHistoryItem(assetId, actionId, memberId, mem.Text, dte.Date, price);
		gv.DataBind();
		dte.Date = DateTime.Today;
		mem.Text = "";
		txtdollars.Text = "";
		ddlmember.Value = Convert.ToInt32(current_user.id);
		gv_assets.DataBind();
		}
	protected void gv_history_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		Toolbox.doSQL_void(@"Delete from assets_history where assets_history_id = @v0", new object[] { e.Keys[0] });
		e.Cancel = true;
		gv.CancelEdit();
		gv.DataBind();
		}

	protected void dte_addhistory_DataBound(object sender, EventArgs e)
		{
		var dte = (ASPxDateEdit)sender;
		dte.Date = DateTime.Today;
		}

	protected void ddl_addhistorymember_DataBound(object sender, EventArgs e)
		{
		var cb = (ASPxComboBox)sender;
		cb.Value = Convert.ToInt32(current_user.id);
		}


	protected void gv_assets_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		Save(e);
		}

	protected void gv_history_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var gv			= (ASPxGridView)sender;
		var assetId		= Convert.ToInt32(gv_assets.GetRowValues(gv_assets.EditingRowVisibleIndex, "assets_ID"));
		var mem			= Convert.ToInt32(e.NewValues["assets_history_memberid"]);
		var action		= Convert.ToInt32(e.NewValues["assets_history_action"]);
		var dollars		= Convert.ToDouble(e.NewValues["assets_history_dollars"]);
		var dte			= Convert.ToDateTime(e.NewValues["assets_history_date"]);
		var id			= Convert.ToInt32(gv.GetRowValues(gv.EditingRowVisibleIndex, "assets_history_id"));
		var notes		= Toolbox.ReturnBlankIfNull_string(e.NewValues["assets_history_notes"]);

		NeAssets.UpdateHistoryItem(id, assetId, action, mem, notes, dte, dollars);

		e.Cancel = true;
		gv.CancelEdit();
		gv_assets.DataBind();
		}

	protected void gv_assets_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		//
		// Check to see whether it is referenced by asset_customer_rate.
		//
		var query = @"SELECT COUNT(1) FROM asset_customer_rate WHERE asset_id = @v0";
		var count = Toolbox.doSQL_int(query, new object[] { e.Keys[0] });
		if (count > 0)
			{
			throw new Exception("This asset is used by other customer, you cannot delete it.");
			}

		Toolbox.doSQL_void(@"Delete from assets where assets_id = @v0", new object[] { e.Keys[0] });
		e.Cancel = true;
		gv_assets.CancelEdit();
		gv_assets.DataBind();
		}

	protected void edit_save_Click(object sender, EventArgs e)
		{


		}

	protected void pc_Asset_Callback(object sender, CallbackEventArgsBase e)
		{
		gv_assets_HtmlEditFormCreated(null, null);
		}
	}

