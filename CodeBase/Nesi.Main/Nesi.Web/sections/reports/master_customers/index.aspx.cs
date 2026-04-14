using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Collections.Generic;
using nesi.core;
using NESI.Common.Models;

public partial class master_customers : Page
	{
	NeMember current_user;
	private const int _page_id = 83; // from Page table in DB
	private const string _page_name = "MasterCustomer";
	Toolbox _tools = new Toolbox();
	ASPxHiddenField h;
	SqlDataSource ds_templates;
	ASPxDropDownEdit dde_filter;
	Panel panel_export;
	bool can_view_dollar_totals = false;
	public int ddl_selected;
	NameValueCollection _q;
	bool can_edit_reps = false;

	protected void Page_Init(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		//	myMember = Toolbox.do_handle_authentication(_page_id);
		current_user = Toolbox.do_handle_authentication(OpsPage.WorkOrders);
		_q = Request.QueryString;
		layout.__gv_id = "gv_MasterCustomers";
		layout.__page_name = _page_name;
		h = (ASPxHiddenField)layout.FindControl("h");
		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
		panel_export = (Panel)layout.FindControl("panel_export");
		panel_export.Visible = true;
		layout.used_gv			= gv_MasterCustomers;
		can_view_dollar_totals = current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewTotalTimeAndMaterialValues);
		can_edit_reps = current_user.AuthenticatedForPrivilege(OpsPrivilege.ManageSalesReps);
		h.Set("gridview_id", "gv_MasterCustomers");
		sds_masterCustomer.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();

		if(!string.IsNullOrEmpty(_q["a"]))
			{
			customer_sales_properties this_csp;
			var customer_id			= 0;
			var address_id			= 0;
			var status_id			= 0;
			switch(_q["a"])
				{
				case "handle_note":
					int.TryParse(_q["customer_id"], out customer_id);
					int.TryParse(_q["address_id"], out address_id);
					var note				= _q["note"].Trim();
					if(address_id > 0 && customer_id > 0 && note != "")
						{
						this_csp					= new customer_sales_properties((int) address_id);
						this_csp.notes_sales		= System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " "+current_user.FullName+" - "+note+"\n"+this_csp.notes_sales+"\n";
						this_csp.save();
					
						Toolbox.QuickReponse(Response, "SUCCESS");
						}
					else
						{
						Toolbox.QuickReponse(Response, "Saving of note failed.");
						}
					
					
				break;
				case "handle_status":
					int.TryParse(_q["customer_id"], out customer_id);
					int.TryParse(_q["address_id"], out address_id);
					int.TryParse(_q["status_id"], out status_id);
					if(address_id > 0 && customer_id > 0 && status_id > 0)
						{
						this_csp					= new customer_sales_properties((int)address_id);
						var prev_status_id			= this_csp.status_id;
						this_csp.status_id			= status_id;
						this_csp.save();
						if(statuses == null)
							{
							populate_statuses();
							}
						var ch			= new customer_history();
						ch.action_id				= 18;
						ch.address_id				= (int) address_id;
						ch.customer_id				= customer_id;
						ch.date						= DateTime.Now;
						ch.member_id				= current_user.id;
						var status_prev			= statuses.Select("id = "+prev_status_id)[0]["status"].ToString();
						var status_now			= statuses.Select("id = "+status_id)[0]["status"].ToString();
						ch.notes					= string.Format("Manual status change from (" + status_prev + ") to ("+status_now+") [master_customer_grid]");
						ch.save();
						
						Toolbox.QuickReponse(Response, "SUCCESS");
						}
					else
						{
						Toolbox.QuickReponse(Response, "Saving of status failed.");
						}
				break;
				}
			}
		var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "Master Customer Report";
		if (!IsPostBack)
			{
			var gl = new NeGridLayouts(current_user.id, _page_name);
			/*if (gl.GridLayoutID == 0)
				{
			//	var char_count		= current_user.FullName.Length + 22;
				// gl.GridLayout_Layout = "page1|filter"+char_count+"|[Account_Manager] = '" + current_user.FullName + "'|conditions1|7|5|visible21|t0|t16|t15|t1|t10|t12|t11|t17|t14|t6|t7|t18|t9|t8|t5|f18|t3|t13|t4|t2|t19|width20|20px|43px|44px|97px|271px|36px|74px|50px|50px|79px|50px|58px|92px|42px|88px|e|91px|60px|55px|56px|35px";
				gv_MasterCustomers.LoadClientLayout(gl.GridLayout_Layout);
				gl.member_id = current_user.id;
				gl.GridLayout_Name = "Default";
				gl.GridLayout_Gridid = _page_name;
				gl.SaveGridLayout();

				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
				}
			else
				{*/
				gv_MasterCustomers.LoadClientLayout(gl.GridLayout_Layout);
				h.Set("ID", gl.GridLayoutID);
				h.Set("NAME", gl.GridLayout_Name);
					//	}
			dde_filter.Text = gl.GridLayout_Name;
			
			((ASPxDateEdit)gv_MasterCustomers.FindHeaderTemplateControl(gv_MasterCustomers.Columns["revfromdate"], "dte_from")).Date = DateTime.Now;
			((ASPxDateEdit)gv_MasterCustomers.FindHeaderTemplateControl(gv_MasterCustomers.Columns["revfromdate"], "dte_to")).Date = DateTime.Now;

			
		
			}
	
		
		var show_grossmargin_info = current_user.AuthenticatedForPrivilege(81) && !current_user.isContact;
		if (!show_grossmargin_info)
			{
			gv_MasterCustomers.Columns["overall_margin"].Visible = false;
			gv_MasterCustomers.Columns["overall_margin"].ShowInCustomizationForm = false;
			}
	}
	protected void Page_Load(object sender, EventArgs e)
		{
		}
	
	protected void Button2_Click(object sender, EventArgs e)
		{
		}
	protected void Button1_Click(object sender, EventArgs e)
		{
		gv_MasterCustomers.CancelEdit();
		}
	protected void gv_MasterCustomers_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{

		e.Cancel = true;

		gv_MasterCustomers.CancelEdit();
		
		}
	protected void gv_MasterCustomers_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		var location			= gv_MasterCustomers.GetDataRow(e.VisibleIndex)["location"].ToString();
		var fn				= e.DataColumn.FieldName;
		switch(fn)
			{
			case "Next Date":
                if (Toolbox.ReturnNullDateTime(e.CellValue) != null)
                {
                    if (Convert.ToDateTime(e.CellValue) < System.DateTime.Today.Date)
                    {
                        e.Cell.ForeColor = System.Drawing.Color.Red;
                    }
                }
			break;

            case "lytd_rev":
            case "ytd_rev":
            case "6months":
			case "24months":
			case "12months":
			case "revfromdate":
				if (!can_view_dollar_totals)
					{
					e.Cell.Text = "0";
					}
			break;
			case "Notes":
				e.Cell.ToolTip = e.CellValue.ToString();
			break;
			}
		}
	protected void gv_MasterCustomers_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var gv			= (ASPxGridView) sender;
		var address_id			= 0;
		var customer_id			= 0;
		var dr				= gv.GetDataRow(gv.EditingRowVisibleIndex);
		if(dr != null && gv.IsEditing)
			{
			var tmp_customer_id		 = dr["ID"];
			var tmp_address_id		 = dr["address_id"];
			if(tmp_address_id == null || tmp_customer_id == null)
				{
				throw new Exception("There is an issue getting needed information.");
				}
			else
				{
				int.TryParse(tmp_customer_id.ToString(), out customer_id);
				int.TryParse(tmp_address_id.ToString(), out address_id);
				var uc_sales			= (sections_customer_modules_sales) gv_MasterCustomers.FindEditFormTemplateControl("uc_sales");
				if(address_id > 0 && customer_id > 0 && uc_sales != null)
					{
					uc_sales.customer_id	= customer_id;
					uc_sales.address_id		= (int)address_id;
					uc_sales.init();
					}
				else
					{
					throw new Exception("Cannot target edit form controls");
					}
				}
			}
		}
	protected void Grid_Customer_History_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var gv = (GridViewDataColumn)gv_MasterCustomers.Columns["Action History"];
		var rowIndex = gv_MasterCustomers.EditingRowVisibleIndex;
		var value = gv_MasterCustomers.GetRowValues(rowIndex, new string[] { "ID" });

		var gv2 = (ASPxGridView)gv_MasterCustomers.FindEditRowCellTemplateControl(gv, "Grid_Customer_History");

		_tools.getSQL_void(@"Delete from customer_history  where customer_history_id = @v0", new object[] { Convert.ToInt32(e.Keys[0]) });
		e.Cancel = true;
		gv2.CancelEdit();
		var dt = _tools.getSQL_datatable(@"SELECT vw_customer_history_all.* from vw_customer_history_all  WHERE vw_customer_history_all.Customer_History_CustID =@v0", new object[] { value });
		gv2.DataSource = dt;
		gv2.DataBind();
		}
	protected void LinkButton1_Click(object sender, EventArgs e)
		{
		var index = (((LinkButton)sender).NamingContainer as GridViewDataRowTemplateContainer).VisibleIndex;
		var val = gv_MasterCustomers.GetRowValues(index, "ID");
		var progress = new NECustomer(Convert.ToInt32(val));
		ScriptManager.RegisterStartupScript(this, this.GetType(), "open_", "boing('../../customer/frame.aspx?Customer_id=" + progress.Customer_ID + "&business_unit_id=" + progress.business_unit_id + "','wo',950,800)", true);
		}
	protected void gv_MasterCustomers_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		var gv				= (ASPxGridView)sender;
        var IDs				= new object[gv.VisibleRowCount];
        for(var i = 0; i < gv.VisibleRowCount; i++)
        	{
			IDs[i]					= gv.GetRowValues(i, "address_id");
        	}
		e.Properties["cpid"]		= IDs;
		e.Properties["cpExp"]		= gv.SaveClientLayout();
		e.Properties["cpfromdate"] = Toolbox.MySQLNow_short();
		e.Properties["cptodate"] = Toolbox.MySQLNow_short();
		}
	protected void gv_MasterCustomers_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		var gv = (ASPxGridView)sender;
		if (e.Parameters != "" && e.Parameters != "new" && (e.Parameters.Length < 5 || e.Parameters.Length == 5 || (e.Parameters.Length > 5 && e.Parameters.Substring(0, 4) != "page")))
			{
			// Multi-save
			var parameter_array	= e.Parameters.Split('|');
			var action				= parameter_array[0];
			var value_string			= parameter_array[1];
			var value_int				= 0;
			int.TryParse(value_string, out value_int);
			var address_ids	= gv_MasterCustomers.GetSelectedFieldValues("address_id");
            // This is for tidying up any rows that are no longer visible.
            for (var i = address_ids.Count - 1; i >= 0 ; i--) 
				{
				if(gv.FindVisibleIndexByKeyValue(address_ids[i]) == DevExpress.Data.DataController.InvalidRow)
					{
					address_ids.RemoveAt(i);
					}
				}
			if(address_ids.Count > 0)
				{
				foreach(var _address_id in address_ids)
					{
					var address_id		= 0;
					if(_address_id != null)
						{
						int.TryParse(_address_id.ToString(), out address_id);
						}
					if(address_id > 0)
						{
						var addr					= new NEAddress(address_id);
						var csp	= new customer_sales_properties((int)address_id);
						switch(action)
							{
							case "a": // Account Code
								csp.account_code			= value_string;
								csp.save();
							break;
							case "am": // Acct Manager
								csp.am_member_id			= value_int;
								csp.save();
							break;
							case "m": // Major Acct Manager
								csp.mam_member_id			= value_int;
								csp.save();
							break;
							case "p": // Project Manager
								csp.project_mgr_member_id	= value_int;
								csp.save();
							break;
							case "c": // Controls Manager
								csp.controls_mgr_member_id	= value_int;
								csp.save();
							break;
							}
						}
					}
			
				}
			}
		else if(e.Parameters != "" && e.Parameters != "new" && (e.Parameters.Length > 5 && e.Parameters.Substring(0, 4) == "page"))
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
	DataTable statuses;
	private void populate_statuses()
		{
		statuses			= Toolbox.doSQL_dt(@"SELECT customer_or_contact_status_id id, customer_or_contact_status status FROM customer_or_contact_status"  , null);
		}
	protected void ASPxComboBox2_Init(object sender, EventArgs e)
		{
	var cb_status = (DropDownList)sender;
				var container = cb_status.NamingContainer as GridViewDataItemTemplateContainer;
			if (gv_MasterCustomers.GetDataRow(container.VisibleIndex) != null)
			{
			
				var _status_id = gv_MasterCustomers.GetDataRow(container.VisibleIndex)["Status_x"];
				var customer_id = gv_MasterCustomers.GetDataRow(container.VisibleIndex)["ID"].ToString();
				var address_id = gv_MasterCustomers.GetDataRow(container.VisibleIndex)["address_id"].ToString();
				var status_id = 0;
				int.TryParse(_status_id.ToString(), out status_id);
				if (statuses == null)
				{
					populate_statuses();
				}
				var list = "";
				switch (status_id)
				{
					case 0:
						list += "0,3,5,6,7,8,9";
						break;
					case 1:
						list += "1,2,3,5,6,8,9";
						break;
					case 2:
						list += "2,3,5,6,9";
						break;
					case 3:
						list += "0,3,5,6,7,8";
						break;
					case 4:
						list += "4";
						break;
					case 5:
						list += "5,2";
						break;
					case 6:
						list += "6";
						break;
					case 7:
						list += "0,3,5,6,7,8";
						break;
					case 8:
						list += "3,5,6,7,8";
						break;
					case 9:
						list += "0,3,5,6,7,8,9";
						break;
				}
				cb_status.DataSource = statuses.Select("id IN (" + list + ")").CopyToDataTable();
				cb_status.DataBind();
				cb_status.SelectedValue = status_id.ToString();
				cb_status.Attributes.Add("onchange", "handle_status(this)");
				cb_status.Attributes.Add("data-cid", customer_id);
				cb_status.Attributes.Add("data-aid", address_id);
			}
		}
	protected void cb_action_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		var parameter_string			= e.Parameter;
		if (parameter_string != "")
			{
			var parameter_array		= parameter_string.Split('|');
			var action					= parameter_array[0];
			var address_id					= Convert.ToInt32(parameter_array[1]);
			var value_string				= parameter_array[2];
			var value_int					= 0;
			int.TryParse(value_string, out value_int);
			var addr					= new NEAddress(address_id);
			var csp	= new customer_sales_properties((int)address_id);
			var customer_id					= addr.Table_ID;
			switch(action)
				{
				case "c":
					csp.account_code			= value_string;
					csp.save();
				break;
				case "o":
					csp.osr_member_id			= value_int;
					csp.save();
				break;
				case "i":
					csp.isr_member_id			= value_int;
					csp.save();
				break;
				case "r":
					csp.ram_member_id			= value_int;
					csp.save();
				break;
				case "p":
					csp.project_mgr_member_id	= value_int;
					csp.save();
				break;
				}
                
			}
		}
	protected void mem_Init(object sender, EventArgs e)
		{
			//if (gv_MasterCustomers.IsCallback)
			//{
				var due = sender as ASPxMemo;
				var container = due.NamingContainer as GridViewDataItemTemplateContainer;
				var customer_id = gv_MasterCustomers.GetDataRow(container.VisibleIndex)["ID"].ToString();
				var address_id = gv_MasterCustomers.GetDataRow(container.VisibleIndex)["address_id"].ToString();
				due.Attributes.Add("onclick", "note_show(this)");
				due.Attributes.Add("data-cid", customer_id);
				due.Attributes.Add("data-aid", address_id);
				due.Attributes.Add("wrap", "off");
			//}
		}
	protected void mem_PreRender(object sender, EventArgs e)
		{
			//if (!IsCallback)
			//{
				var due = sender as ASPxMemo;
				var container = due.NamingContainer as GridViewDataItemTemplateContainer;
				var dr = gv_MasterCustomers.GetDataRow(container.VisibleIndex);
				if(dr != null)
					{
					var customer_id = dr["ID"].ToString();
					var address_id = dr["address_id"].ToString();
					due.Attributes.Add("onclick", "note_show(this)");
					due.Attributes.Add("data-cid", customer_id);
					due.Attributes.Add("data-aid", address_id);
					due.Attributes.Add("wrap", "off");
					}
			//}
		}
	protected void cbddl_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
		{
		var cb								= (ASPxComboBox) sender;
		var container = cb.NamingContainer as GridViewDataItemTemplateContainer;
		var address_id								= gv_MasterCustomers.GetDataRow(container.VisibleIndex)["address_id"].ToString();
		e.Properties["cpIndex"]						= container.VisibleIndex;
		e.Properties["cpAddress_id"]				= address_id;
		}
	protected void dte_button_Click(object sender, EventArgs e)
		{

		var tmp_from		= 	((ASPxDateEdit)gv_MasterCustomers.FindHeaderTemplateControl(gv_MasterCustomers.Columns["revfromdate"], "dte_from")).Date;
		var tmp_to = ((ASPxDateEdit)gv_MasterCustomers.FindHeaderTemplateControl(gv_MasterCustomers.Columns["revfromdate"], "dte_to")).Date;
		
		
//		ScriptManager.RegisterStartupScript(up, typeof (UpdatePanel), "toggle", "toggle_options($('#options_button'), true);", true); // Keeps the options panel open.
		}
//	protected void rev_button_Click(object sender, EventArgs e)
	//	{
	//	rev_error.Text		= "";
	//	fill_grid(true);
	//	ScriptManager.RegisterStartupScript(up, typeof (UpdatePanel), "toggle", "toggle_options($('#options_button'), true);", true); // Keeps the options panel open.
	//	}
	protected void gv_MasterCustomers_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
		{
	//	ASPxGridView grid		= (ASPxGridView) sender;
	//	GridViewDataColumn gvc = (GridViewDataColumn)grid
	//	string fn = gvc.FieldName;
//	
//		switch(fn)
//			{
//			case "osr":
//			case "isr":
//			case "ram":
//			case "am":
//			case "pm":
//			case "controlsguy":
//				e.Cell.Text		= can_edit_reps ? e.Cell.Text : "";
//			break;
	//		}
		}

	protected void footer_ddl_control_Init(object sender, EventArgs e)
	{
		var ddl = (ASPxComboBox)sender;
		ddl.ClientEnabled = can_edit_reps;
	}
	protected void footer_btn_control_Init(object sender, EventArgs e)
	{
		var ddl = (ASPxButton)sender;
		ddl.ClientEnabled = can_edit_reps;
	}
	
}
