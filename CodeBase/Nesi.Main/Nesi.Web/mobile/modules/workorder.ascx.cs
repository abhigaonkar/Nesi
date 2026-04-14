using System;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web;
using System.Text;
using NESI.Common.Models;
using nesi.core;

public partial class mobile_modules_workorder : shared.mobile_subpage
	{
	Toolbox _tools;
	NeMember _current_user;
	bool _approve_pm;
	bool _approve_bm;
	bool _view_cost;
	bool _can_see_sell;
	bool _can_create_locations;
	bool _can_commit;
	public int workorder_id {get; set;}
	protected void Page_Init(object _sender, EventArgs _e)
		{
		_tools											= new Toolbox();
		_current_user									= Toolbox.do_handle_authentication(1);
		uc_workorder_details.current_user				= _current_user;
		_approve_pm										= _current_user.AuthenticatedForPrivilege(15);
		_approve_bm										= _current_user.AuthenticatedForPrivilege(16);
		_view_cost										= _current_user.AuthenticatedForPrivilege(58);
		_can_create_locations							= _current_user.AuthenticatedForPrivilege(117);
		_can_commit										= _current_user.AuthenticatedForPrivilege(190);
		_can_see_sell = _current_user.AuthenticatedForPrivilege(60);
		sds_progress.SelectParameters[0].DefaultValue	= _current_user.business_unit_id.ToString();
		sds_toapprove.SelectParameters[0].DefaultValue	= _current_user.business_unit_id.ToString();
		sds_toapprove.SelectParameters[1].DefaultValue	= _approve_bm ? "1" : "0";
		sds_toapprove.SelectParameters[2].DefaultValue	= _current_user.id.ToString();
		sds_signoff.SelectParameters[0].DefaultValue	= _current_user.business_unit_id.ToString();
		sds_signoff.SelectParameters[1].DefaultValue	= _approve_bm ? "1" : "0";
		sds_signoff.SelectParameters[2].DefaultValue	= _current_user.id.ToString();
		//scan_box.Attributes["placeholder"]				= "Scan Here";
		if(Parent.NamingContainer.ToString().Contains("part_management"))
			{
			new_wo.Visible			= false;
			}
		}
	public override void DataBind()
		{
		do_databind();
		}
	private void do_databind()
		{
		var workorder_preset			= workorder_id != 0;
		new_wo.Visible					= !workorder_preset;
		ddl_tab.Items[0].Enabled		= !workorder_preset;
		ddl_tab.Items[1].Enabled		= !workorder_preset;
		ddl_tab.Items[2].Enabled		= !workorder_preset;

		if(workorder_id > 0)
			{
			load_workorder(workorder_id);
			}
		}
	protected void Page_Load(object _sender, EventArgs _e)
		{
		var _q			= Request.QueryString;
		if(!Page.IsCallback && !Page.IsPostBack && string.IsNullOrEmpty(_q["woprog_id"]))
			{
			Session["mobile_woprog_id"] = "0";
			Session["mobile_working_tab"] = null;
			load_tab(0);
			return;
			}
		if(IsPostBack && Convert.ToInt32(Session["mobile_woprog_id"]) != 0)
			{
			load_tab(3);
			load_workorder(Convert.ToInt32(Session["mobile_woprog_id"]));
			return;
			}
		if(!string.IsNullOrEmpty(_q["woprog_id"]))
			{
			load_tab(3);
			Session["mobile_woprog_id"] = _q["woprog_id"];
			load_workorder(Convert.ToInt32(_q["woprog_id"]));
			return;
			}
		}
	protected void txtbc_TextChanged(object _sender, EventArgs _e)
		{
		}
	private void load_workorder(int _woprog_id)
		{
	//	int.TryParse(ddlwo.Value.ToString(), out woprog_id);
		if (_woprog_id==0)
		{
			Session["mobile_woprog_id"] = "0";
		}
		if(_woprog_id > 0)
			{
			bt_back.Visible						= workorder_id == 0;
			ddl_tab.Visible						= false;
			uc_workorder_details.DataBind();
			Session["mobile_woprog_id"]			= _woprog_id.ToString();
			Session["mobile_woprog_id"] = _woprog_id.ToString();
			
			var wo				= new NeWOProg(_woprog_id);
			var addy			= new NEAddress(wo.woprog_Address_ID);
	//		lbl_bvwo.Text			= wo.OrderNumber;
	//		lbl_customer.Text		= wo.CustomerName;
	//		lbl_status.Text			= wo.Status;
			var service_address	= addy.Addr1;
			service_address			+= addy.Addr2.Trim().Length > 0 ? "<br/>" + addy.Addr2 : "";
			service_address			+= addy.Addr3.Trim().Length > 0 ? "<br/>" + addy.Addr3 : "";
			service_address			+= addy.Addr4.Trim().Length > 0 ? "<br/>" + addy.Addr4 : "";
	//		lbl_address.Text		= string.Format(@"<a target='_blank' href='//maps.google.com/?q={0} {1}, {2}, {3}'>{0}<br/>{1},{2},{3}</a>",
	//						service_address,
	//						addy.City,
	//						addy.Prov,
	//						addy.Postal
	//						);
	//		lbl_desc.Text			= wo.Description;
	//		lbl_contact.Text		= new NEContact(wo.woprog_Contact_ID).name;
	//		lbl_pm.Text				= new NeMember(wo.intProjectManager).FullName;
	//		lbl_po.Text				= Regex.IsMatch(wo.PONumber, @"_+")  // Normalizing the length of PO's that only have underscores
	//									? "______________" 
	//									: wo.PONumber.Trim() == ""
	//										? "Not Set"
	//										: wo.PONumber;
	//		lbl_margin.Text			= wo.woprog_grossmargin.ToString("P0");
	//		lbl_margin.Visible		= current_user.AuthenticatedForPrivilege(81);
	//		lbl_total.Text			= wo.net_total.ToString("C2");
	//		lbl_total.Visible		= current_user.AuthenticatedForPrivilege(81);
			//if_invoice.Attributes["src"]	= ""+woprog_id+"&html=1";
			//edit_signoff.Visible	=	(wo.Status != NeWOProg.wo_status[NeWOProg.woprog_status_enum.WaitingToBeInvoiced] 
			//							&& wo.Status != NeWOProg.wo_status[NeWOProg.woprog_status_enum.Invoiced]);
			gv_inprogress.DataBind();
			gv_toapprove.DataBind();
			//uc_filemanager.DataBind();
			}
		}
	protected void gv_parts_HtmlDataCellPrepared(object _sender, ASPxGridViewTableDataCellEventArgs _e)
		{
		if (_e.DataColumn.FieldName == "comt")
			{
			if (Toolbox.ReturnZeroIfNull_double(_e.CellValue).ToString() == "0.00")
				{
				_e.Cell.Font.Bold = true;

				}
			}
		}
	protected void gv_HtmlRowCreated(object _sender, ASPxGridViewTableRowEventArgs _e)
		{
		var gv			= (ASPxGridView) _sender;
		if(gv.ID == "gv_toapprove")
			{
			if(_e.VisibleIndex >= 0)
				{
				var dr			= gv_toapprove.GetDataRow(_e.VisibleIndex);
				var status		= dr["status"].ToString();
				var _class		= status == "Waiting BM Approval" ? "bmapproval" : status == "Waiting PM Approval" ? "pmapproval" : "";
				_e.Row.CssClass	= _class;
				}
			}
		if (_e.RowType == GridViewRowType.Data || _e.RowType == GridViewRowType.Filter)
			{
			_e.Row.Height = Unit.Pixel(50);
			}
		}
	protected void gv_inprogress_AutoFilterCellEditorInitialize(object _sender, ASPxGridViewEditorEventArgs _e)
		{
		}
	protected void gv_inprogress_SelectionChanged(object _sender, EventArgs _e)
		{

		}
	protected void pc_Callback(object _sender, CallbackEventArgsBase _e)
		{
		}
	protected void cbp_Callback(object _sender, CallbackEventArgsBase _e)
		{
		var woprog_id = 0;
		if(Session["mobile_woprog_id"] != null && Session["mobile_woprog_id"].ToString() != "0")
			{
			int.TryParse(Session["mobile_woprog_id"].ToString(), out woprog_id);
			}
		var original_wodc = new NeWODetailCurrent();
		var wodc = new NeWODetailCurrent();
		NeWOProg wo;
		var il = new location();
		var ilm = new location_master();
		var woc = new woprog_comment();
		double qty, absqty, current_qty, current_sell, sell = 0;
		if (_e.Parameter.Contains("|"))
			{
			var paras = _e.Parameter.Split('|');
			switch (paras[0])
				{
				case "cancel":
					int.TryParse(Session["mobile_woprog_id"].ToString(), out woprog_id);
					//pc.TabPages.FindByName("edit_part").ClientVisible = false;
					load_workorder(woprog_id);
					break;
				#region edit_po
				case "edit_po":
					int.TryParse(Session["mobile_woprog_id"].ToString(), out woprog_id);
					//pc.TabPages.FindByName("edit_part").ClientVisible = false;
					wo = new NeWOProg(woprog_id);
					//			wo.PONumber						= lbl_po.Text;
					wo.woprog_custpo_member_id = _current_user.id;
					wo.woprog_custpo_dt = DateTime.Now;
					wo.SaveWorkOrder();
					load_workorder(woprog_id);
					break;
				#endregion edit_po
				#region reworks
				case "reworks":
					int.TryParse(Session["mobile_woprog_id"].ToString(), out woprog_id);
					//pc.TabPages.FindByName("notes").ClientVisible = true;

					wo = new NeWOProg(woprog_id);

				    try
				        {
				        NeWOProg.check_before_move(wo, OpsWOStatus.Rework);
				        }
				    catch (Exception check_move_error)
				        {
				      Toolbox.FriendlyException(this.Response,check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
				        return;
				        }

					NeWOProg.move_status(wo, _current_user, "", OpsWOStatus.Enums.Rework);

					woc = new woprog_comment();
					woc.deleted = "F";
					woc.member_id = _current_user.id;
					woc.text = memo_rework_reason.Text;
					woc.woprog_id = woprog_id;
					woc.save();

					gv_inprogress.DataBind();
					gv_toapprove.DataBind();

					memo_rework_reason.Text = "";
					popup_reworks.ShowOnPageLoad = false;
					load_workorder(woprog_id);
					break;
				#endregion reworks
				#region close_wo
				case "close_wo":
					Session["mobile_woprog_id"] = "";
					load_workorder(0);
					break;
				#endregion close_wo
				#region view
				case "view":
					int.TryParse(paras[1], out woprog_id);
				//	uc_filemanager.woprog_id = woprog_id;
					Session["mobile_woprog_id"] = woprog_id.ToString();
					load_workorder(woprog_id);
					load_tab(3);
					break;
				#endregion view
				#region addnote
				case "addnote":
					wo = new NeWOProg(woprog_id);
				//	pc.TabPages.FindByName("notes").ClientVisible = true;
					//pc.ActiveTabIndex = 5;
					//woc = new woprog_comment
					//		{
					//			deleted = "F",
					//			member_id = current_user.id,
					//			text = new_note.Text,
					//			woprog_id = woprog_id
					//		};
					//woc.save();
					//new_note.Text = "";
					//new_note.Focus();
					load_workorder(woprog_id);
					break;
				#endregion addnote
				}
			}
		}
	protected void gv_parts_HtmlRowCreated(object _sender, ASPxGridViewTableRowEventArgs _e)
		{
		_e.Row.Attributes["onclick"]			= "cbp.PerformCallback('editpart|"+_e.KeyValue+"')";
		_e.Row.Height						= _e.RowType == GridViewRowType.Data ? Unit.Pixel(35) : _e.Row.Height;
		}
	protected void cb_process_Callback(object _source, CallbackEventArgs _e)
		{

		}



	private void load_tab(int _index)
		{
		mv_tabs.ActiveViewIndex	= _index;
		switch(_index)
			{
			case 0:
				gv_inprogress.DataBind();
			break;
			case 1:
				gv_signoff.DataBind();
			break;
			case 2:
				gv_toapprove.DataBind();
			break;
			}
		if(_index < 3)
			{
			Session["mobile_working_tab"]		= _index;
			}
		}
	protected void ddl_tab_SelectedIndexChanged(object sender, EventArgs e)
		{
		load_tab(Convert.ToInt32(ddl_tab.SelectedValue));
		}
	protected void bt_back_Click(object sender, EventArgs e)
		{
		Session["mobile_woprog_id"]			= "0";
		bt_back.Visible						= false;
		ddl_tab.Visible						= true;
		load_workorder(0);
		uc_workorder_details.clear();
		mv_tabs.ActiveViewIndex				= Convert.ToInt32(Session["mobile_working_tab"]);
		load_tab(mv_tabs.ActiveViewIndex);
		}
}