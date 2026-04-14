using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Data;
using MySql.Data.MySqlClient;
//using nesi.bv;
using nesi.core;
using NESI.Common.Models;

public partial class sections_workorder_picklist : Page
{
	#region Initial Variable Declaration
	NeMember current_user;
	NeBusinessUnit temp_comp;
	int _page_id = 1;
	private bool priv_nesi_invoiced_edit;
	string callback_param = "";
	int newtableid;
	bool lockdesc;
	int oldrecno;
	int newrecno;
	int main_id;
	int main_rev = 0;
	string id;
	bool is_adding;
	bool member_can_see_cost;
	bool member_can_see_sell;
	bool member_can_edit_sell;
	bool member_can_see_labour_cost;
	bool member_can_discount;
	bool member_can_toggle_summarize_labor;
	bool isallowed_jobcosts_negative;
	bool can_commit;
	string newworkorderid;
	string oldworkorderid;
	string newpartnumber = "0";
	string oldpartnumber = "0";
	string oldvendorpartnumber;
	string newdescription;
	string olddescription;
	string transferbvwo = "0";
	string originbvwo;
	double newReqQuantity;
	double newCommittedQuantity;
	double oldqty;
	int xfer_comm_recv_select_id;
	int xfer_wo_select_id;
	int xfer_internal_select_id;
	int xfer_external_select_id;

	double xfer_qty_wo;
	double xfer_qty_internal;
	double xfer_qty_external;
	double xfer_combined_qty;
	double xfer_to_original_qty;
	double oldsell;
	double newsell;
	double oldquoteextd;
	double newquoteextd;
	string newdatereq = "";
	string olddatereq = "";
	double neworigsell;
	double oldorigsell;
	double transfersell;
	string TableSuffix = "";
	string transferwoprogid = "";
	int cost_level;
	double newcost;
	double oldcost;
	double newqtyperpart = 1;
	double oldqtyperpart;
	string newDateExpected = "";
	string oldDateExpected = "";
	double new_qty_committed;
	double old_qty_committed;
	string newNotes = "";
	string newbilltype = "";
	string oldbilltype = "";
	string transfernote = "";
	bool FlagPartcode = true;
	double poqty;
	string notesforwoaudit = "";

	double newdiscount;
	double olddiscount;
	double DiscountAmount;
	int NewTrack;
	int OldTrack;
	private string ActivityCode { get; set; }
	private string CostElement { get; set; }
	private string ClientWO { get; set; }
	private string ClientPO { get; set; }
	string sql;
	private bool CanTogglePartialBillLines { get; set; }
	private bool issummarize_labor {get; set;}
	private int rec_no_from_new_wo_line_item;
	private int wo_detail_current_id_target;

	Label header_label;
	Label headersub_label;
	bool chk_workorder_transfer;
	bool can_edit_part_cost;
	HtmlImage header_print;
	HtmlImage header_printwo;
	HtmlImage header_printwo_unf;
	NameValueCollection _q;
	List<int> ReportsToList;
	DataSet _handlers = new DataSet();
	JavaScriptSerializer jSON = new JavaScriptSerializer();
	#endregion Initial Variable Declaration
	Control ctrlname;
	bool auth_new_location;
	double qtytoadd = 0;
	private NeWOProg wo { get; set; }
	private NeBusinessUnit WorkingBusinessUnit { get; set; }
	private NeBusinessUnit WarehouseBusinessUnit { get; set; }
	private bool IsBackOffice = false;
	public bool is_exclude { get; set; }
	private double Xfer_to_original_qty { get => xfer_to_original_qty; set => xfer_to_original_qty = value; }
	private struct GVColumns
		{
		public const string LineId				= "id";
		public const string LineType			= "linetype";
		public const string Select				= "select";
		public const string Location			= "location";
		public const string RecNo				= "rec_no";
		public const string MasterId			= "master_id";
		public const string Description			= "description";
		public const string QtyRequired			= "qty";
		public const string QtyReceiving		= "qty_receiving";
		public const string QtyCommittedToDate	= "qtyrec";
		public const string Cost				= "cost";
		public const string Sell				= "sell";
		public const string Discount			= "discount";
		public const string ExtTandM			= "exttandm";
		public const string Commit				= "commit_wo";
		public const string Unfulfilled			= "unfulfilled";
		public const string DateRequired		= "required_date";
		public const string Notes				= "notes";
		public const string Transfer			= "transfer";
		public const string History				= "history";
		public const string Origin				= "origin";
		public const string Issues				= "issues";
		public const string Margin				= "margin"; 
		public const string AvailableQuantity	= "qty_avail"; 
		public const string TrackPart			= "track_part";
		public const string Buttons				= "buttons";
		public const string Barcode				= "brcd";
		public const string MembertypeName		= "membertypename";
		public const string BillType			= "billtype_id";
		public const string MemberId			= "memberid";
		public const string ActivityCode		= "activity_code";
		public const string CostElement			= "cost_element";
		public const string ClientWO			= "client_wo";
		public const string ClientPO			= "client_po";
		public const string labourTooltip		= "labourTooltip";
		public const string Active				= "is_active";
		public const string PartiallyBilled		= "partially_billed";
		}
	private struct AddColumns
		{
		/// <summary>
		/// 2
		/// </summary>
		public const int MaterialType			= 2;
		/// <summary>
		/// 4
		/// </summary>
		public const int PartNo					= 4;
		/// <summary>
		/// 6
		/// </summary>
		public const int Description			= 6;
		/// <summary>
		/// 7
		/// </summary>
		public const int DDLs					= 7;
		/// <summary>
		/// 8
		/// </summary>
		public const int QuantityRequired		= 8;
		/// <summary>
		/// 24
		/// </summary>
		public const int QuantityCommitted		= 24;
		/// <summary>
		/// 27
		/// </summary>
		public const int Location				= 27;
		/// <summary>
		/// 23
		/// </summary>
		public const int QuantityAvailable		= 23;
		/// <summary>
		/// 6
		/// </summary>
		public const int PriceCost				= 9;
		/// <summary>
		/// 10
		/// </summary>
		public const int PriceSell				= 10;
		/// <summary>
		/// 13
		/// </summary>
		public const int PriceExtended			= 13;
		/// <summary>
		/// 26
		/// </summary>
		public const int RequiredDate			= 26;
		/// <summary>
		/// 20
		/// </summary>
		public const int SaveLine				= 20;
		}
	protected struct xfer_array
	{

		public int id { get; set; }
		public int part_id { get; set; }
		public double qty { get; set; }
		public string origin { get; set; }
		public string type { get; set; }
		public bool use_wo { get; set; }
		public int dest_id { get; set; }

	} 
	protected void Page_Init(object sender, EventArgs e)
	{
		_q = Request.QueryString;
		Toolbox.do_dont_cache_page(Response);
		var returned = "";
		id = string.IsNullOrEmpty(_q["id"]) ? "" : _q["id"];
		int.TryParse(id, out main_id);
		if (id == "" && string.IsNullOrEmpty(_q["row_id"]) && string.IsNullOrEmpty(_q["a"]) || id != "" && !id.Contains(",") && main_id == 0)
		{
			Toolbox.FriendlyException(Response, "Not a valid request", "");
		}
		callback_param = Request.Params.Get("__CALLBACKPARAM");
		current_user = Toolbox.do_handle_authentication(_page_id);
		IsBackOffice = current_user.business_unit.is_backoffice;
		ReportsToList = NeMember.ReportsToList(current_user.id);
		// IF trying to manipulate just this page while debugging, uncomment the below line, comment the handle_auth line and navigate to the picklist page directly... quicker.
		//current_user = new NeMember(Convert.ToInt32(_q["member_id"]));
		//Session["global_visible_business_units"] = "1,4,7,38,22,26";
		hidID.Value = id;
		wo = new NeWOProg(main_id);
		WorkingBusinessUnit = new NeBusinessUnit(wo.business_unit_id);
		WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
		Session["warehouse_bu_id"] = WarehouseBusinessUnit.id;
		hidWorkingBusinessUnitID.Value = WorkingBusinessUnit.id.ToString();
		hidWarehouseBusinessUnitID.Value = WarehouseBusinessUnit.id.ToString();
		hidfixed_labour.Value = wo.use_fixed_labour_rate.ToString();
		hidfixed_labour_value.Value = wo.fixed_labour_rate.ToString();
		hidfixed_markup.Value = wo.use_fixed_material_markup.ToString();
		hidfixed_markup_value.Value = wo.fixed_material_markup.ToString();
		header_print = ImgBtn_Print;
		header_printwo = ImgBtn_PrintWO;
		header_printwo_unf = ImgBtn_PrintWOUnf;
		CanTogglePartialBillLines = current_user.AuthenticatedForPrivilege(OpsPrivilege.CanTogglePartialBillLines);
		member_can_see_cost = current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewCostInformationOnWorkOrders);
		member_can_edit_sell = current_user.AuthenticatedForPrivilege(OpsPrivilege.ChangeWoLinePrice);
		member_can_see_sell = current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewGrossMarginInformation);
		member_can_see_labour_cost = current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewLabourCostsOnWorkOrders);
		member_can_discount = current_user.AuthenticatedForPrivilege(OpsPrivilege.DiscountEntryCheckSelection);
		member_can_toggle_summarize_labor = current_user.AuthenticatedForPrivilege(OpsPrivilege.ApprovalBm);

		can_commit = current_user.AuthenticatedForPrivilege(OpsPrivilege.CanCommitParts);
		((HtmlContainerControl)Page.Master.FindControl("jqueryspellcheckjs")).Visible = false;
		if (_q["a"] != null)
		{
			Response.Clear();
			Toolbox.do_set_plain_header(Response);
			switch (_q["a"])
			{
				#region cost_handler
				case "cost_handler":
					if (_q["qty"] != null && _q["cost"] != null)
					{
						var qty = Convert.ToDouble(_q["qty"]);
						double cost = 0;
						if (_q["cost"] != "")
						{
							double.TryParse(_q["cost"], out cost);
						}

						var sell = wo.use_fixed_material_markup ? Math.Round(wo.fixed_material_markup * cost, 2) : Math.Round(shared.GetSellPrice(Math.Abs(cost), 0, true, qty, WorkingBusinessUnit.id32), 2);
						var extd_sell = cost < 0 ? Math.Round(qty * sell, 2) * -1 : Math.Round(qty * sell, 2);
						var extd_cost = Math.Round(qty * cost, 2);
						returned = string.Format(@"{{
'status': 'success',
'sell': {0},
'ext_sell': {1},
'ext_cost': {2}
}}", sell, extd_sell, extd_cost).Replace('\'', '"');
					}
					else
					{
						returned = @"{""status"":""error""}";
					}
					Response.Write(returned);
					break;
				#endregion cost_handler
				#region repair_info
				case "repair_info":
					if (_q["id"] != null)
					{
						var ico = new consignment(Convert.ToInt32(_q["id"]));
						var sell = ico.repair_price;
						var extd_sell = ico.repair_price;
						returned = string.Format(@"{{
'status': 'success',
'sell': {0},
'ext_sell': {1}
}}", sell, extd_sell).Replace('\'', '"');
					}
					else
					{
						returned = @"{""status"":""error""}";
					}
					Response.Write(returned);
					break;
				#endregion repair_info
				#region transfer_data
				case "transfer_data":
					Toolbox.do_set_XML_header(Response);
					if (!string.IsNullOrEmpty(_q["id"]) && !string.IsNullOrEmpty(_q["type"]))
					{
						var type = _q["type"];
						id = Toolbox.do_value_from(_q["id"], false);
						var where_clause = id.Contains(",") ? " IN (" + id + ")" : " = " + id;
						var _dt = Toolbox.doSQL_dt(string.Format(@" SELECT wo_detail_current_id line_id, wo_detail_current_master_id part_id, wo_detail_current_qty_committed qty, wo_detail_current_origin origin
FROM wo_detail_current WHERE wo_detail_current_id {0} AND wo_detail_current_qty_committed > 0", where_clause), null);
						var sb = new StringBuilder();
						sb.Append("<transfer_data>");
						foreach (DataRow _dr in _dt.Rows)
						{
							var line_id = _dr["line_id"].ToString();
							var part_id = _dr["part_id"].ToString();
							var qty = _dr[GVColumns.QtyRequired].ToString();
							sb.AppendFormat(@"
			<line>
				<id>{0}</id>
				<part_id>{1}</part_id> 
				<qty>{2}</qty>
				<origin>{3}</origin>
			</line>",
							line_id,
							part_id,
							qty,
							_dr["origin"].ToString().Trim()
							);
						}
						sb.Append("</transfer_data>");
						Response.Write(sb.ToString());
					}
					else
					{
						Response.Write("Invalid Request");
					}
					Response.End();
					break;
				#endregion transfer_data
				#region location_qty
				case "location_qty":
					object _this_mas_id = _q["master_id"];
					object _this_loc_id = _q["location_id"];
					object _this_com_id = _q["business_unit_id"];
					Response.Write(Toolbox.doSQL_string(@"SELECT IFNULL(MIN(qty),0) FROM inventory_location WHERE location_master_id = @v1  AND master_id = @v0  AND business_unit_id = @v2  LIMIT 1", new[] { _this_mas_id, _this_loc_id, _this_com_id }));
					break;
				#endregion location_qty
			}
			Response.End();
		}
		if (!IsPostBack)

			{

			Session["summarized_lines_" + main_id] = WorkingBusinessUnit.summarize_labor;



			}

		issummarize_labor = Session["summarized_lines_" + main_id] != null 
								? (bool)Session["summarized_lines_" + main_id] 
								: WorkingBusinessUnit.summarize_labor;
		var _line_id = "";
		var l_id = 0;
		if (!string.IsNullOrEmpty(callback_param) && callback_param.Contains("|"))
		{
			try
			{
				var is_normal = callback_param.Split('|').Length == 7 || callback_param.Split('|').Length == 5;
				_line_id = is_normal ? "" : callback_param.Split('|').Length > 3 ? callback_param.Split('|')[3].TrimEnd(';') : "";
			}
			catch (Exception ee)
			{
				_line_id = "";
				Toolbox.do_errorLog_errorStack(ee);
			}
			if (!int.TryParse(_line_id, out l_id))
			{
				_line_id = "";
			}
		}
		#region handler_setter

		var wo_current_status = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(woprog_status),'--') FROM woprog WHERE woprog_id = @v0  LIMIT 1", new object[] { id });
		TableSuffix = wo_current_status == OpsWOStatus.Invoiced || wo_current_status == OpsWOStatus.WaitingToBeInvoiced ? "h" : "";
		if (wo_current_status != OpsWOStatus.WaitingToBeInvoiced && wo_current_status != OpsWOStatus.Invoiced)
		{
			#region current
			var _wo_detail_current = new DataTable();
			var _wo_detail_current_q = @"
SELECT 
	a.id, 
	IF(IFNULL(a.notes,'') LIKE 'A_%|%', SUBSTRING(notes FROM LOCATE('|', notes)+1) ,TRIM(notes)) notes, 
	ISSUE_CHECK(a.id, true) issues,  
	TRIM(a.origin) origin, 
	a.master_id master_id, 
	a.woprog_id woprog_id, 
	a.type type,
	b.woprog_customer_id,
	b.woprog_address_id,
	IF(a.master_id NOT IN (55555, 55556, 55557, 55558, 55559, 55560) && a.master_id < 990000, (SELECT IFNULL(SUM(po_details_qty_ordered-po_details_qty_received),0) FROM po_details_current WHERE po_details_woprog_id=a.woprog_id and po_details_current.is_gl_account = false AND po_details_part_no = a.master_id AND po_details_line_active=1), 0) qty_on_order,
	IF(a.master_id NOT IN (55555, 55556, 55557, 55558, 55559, 55560) && a.master_id < 990000, (SELECT GROUP_CONCAT(DISTINCT bb.poprog_bvpo) FROM po_details_current aa LEFT JOIN poprog_header bb ON aa.po_details_poprog_id = bb.poprog_id WHERE aa.po_details_woprog_id=a.woprog_id and aa.is_gl_account = false AND aa.po_details_part_no = a.master_id AND aa.po_details_line_active=1), '') pos
FROM 
	wo_detail a
LEFT JOIN
	woprog b
	ON a.woprog_id = b.woprog_id
LEFT JOIN
	address c
	ON b.woprog_address_id = c.address_id";
			_wo_detail_current_q += _line_id != "" ? "\nWHERE a.id = " + _line_id : "\nWHERE a.woprog_id = " + id;
			_wo_detail_current = Toolbox.doSQL_dt(_wo_detail_current_q, null);
			_wo_detail_current.TableName = "wo_detail";
			if (_handlers != null && _handlers.Tables["wo_detail"] == null)
			{
				_handlers.Tables.Add(_wo_detail_current.Copy());
			}

			#endregion current
		}
		else
		{
			#region history
			var _wo_detail_history = new DataTable();
			var _wo_detail_history_q = @"
SELECT 
	a.id, 
	TRIM(a.notes) notes, 
	ISSUE_CHECK(a.id, false) issues, 
	TRIM(a.origin) origin, 
	a.master_id master_id, 
	a.woprog_id woprog_id, 
	a.type type,
	b.woprog_customer_id,
	b.woprog_address_id,
	IF(a.master_id NOT IN (55555, 55556, 55557, 55558, 55559, 55560) && a.master_id != 9596 && a.master_id < 990000, (SELECT IFNULL(SUM(po_details_qty_ordered-po_details_qty_received),0) FROM po_details_current WHERE po_details_woprog_id=a.woprog_id and po_details_current.is_gl_account = false AND po_details_part_no = a.master_id AND po_details_line_active=1), 0) qty_on_order,
	IF(a.master_id NOT IN (55555, 55556, 55557, 55558, 55559, 55560) && a.master_id != 9596 && a.master_id < 990000, (SELECT GROUP_CONCAT(DISTINCT bb.poprog_bvpo) FROM po_details_current aa LEFT JOIN poprog_header bb ON aa.po_details_poprog_id = bb.poprog_id WHERE aa.po_details_woprog_id=a.woprog_id and aa.is_gl_account = false AND aa.po_details_part_no = a.master_id AND aa.po_details_line_active=1), '') pos
FROM 
	wo_detailh a
LEFT JOIN
	woprog b
	ON a.woprog_id = b.woprog_id
LEFT JOIN
	address c
	ON b.woprog_address_id = c.address_id";
			_wo_detail_history_q += _line_id != "" ? "\n WHERE a.id = " + _line_id : "\n WHERE a.woprog_id = " + id;
			_wo_detail_history = Toolbox.doSQL_dt(_wo_detail_history_q, null);
			_wo_detail_history.TableName = "wo_detailh";
			_handlers.Tables.Add(_wo_detail_history.Copy());
			#endregion history
		}
		// To disable new history procedure, comment the following 3 lines out.
		var line_changes = Toolbox.doSQL_dt(@"SELECT * FROM woprogchanges_snapshot WHERE woprog_id = @v0  AND for_tooltip = 1", new object[] { id });
		line_changes.TableName = "history";
		_handlers.Tables.Add(line_changes.Copy());
		#endregion handler_setter
		can_edit_part_cost = current_user.AuthenticatedForPrivilege(OpsPrivilege.EditWorkOrderLineCost);
		auth_new_location = current_user.AuthenticatedForPrivilege(OpsPrivilege.CanMakeNewLocations);
		if (!auth_new_location)
		{
			#region !auth_new_location
			ds_internal_locations.SelectCommand = @"
SELECT 
	b.id, 
	CONCAT(b.name,' - (', IFNULL(a.qty,0), ')') name, 
	IFNULL(a.qty,0) qty 
FROM 
	inventory_location a 
LEFT JOIN 
	inventory_location_master b ON a.location_master_id = b.id
WHERE 
	a.business_unit_id = @warehouse_bu_id AND 
	b.type_id = 1 AND
	a.master_id = @master_id";
			ds_external_locations.SelectCommand = @"
SELECT 
	a.id, 
	CONCAT(a.name,' - (', IFNULL(b.qty,0), ')') name, 
	IFNULL(b.qty,0) qty 
FROM 
	inventory_location_master a
LEFT JOIN 
	inventory_location b  ON b.location_master_id = a.id
WHERE 
	b.business_unit_id = @warehouse_bu_id AND 
	a.type_id = 2 AND
	b.master_id = @master_id";

			ds_addline_location.SelectCommand = @"
(SELECT
	b.id,
	CONCAT(IF(b.type_id = 1, 'INT - ', 'EXT - '), CONCAT(b.name),' (',IFNULL(a.qty,0),')')  name,
	IFNULL(a.qty, 0) qty,
	a.max,
	b.type_id
FROM
	inventory_location a
LEFT JOIN
	inventory_location_master b 
		ON	b.id = a.location_master_id AND 
			a.master_id = @master_id AND 
			a.business_unit_id = @warehouse_bu_id AND 
			b.type_id = 1
WHERE
	b.business_unit_id = @warehouse_bu_id)
UNION
(SELECT
	a.id,
	CONCAT(IF(a.type_id = 1, 'INT - ', 'EXT - '), CONCAT(a.name),' (',IFNULL(b.qty,0),')')  name,
	IFNULL(b.qty, 0) qty,
	b.max,
	a.type_id
FROM
	inventory_location_master a
LEFT JOIN
	inventory_location b 
		ON	a.id = b.location_master_id AND 
			b.master_id = @master_id AND 
			b.business_unit_id = @warehouse_bu_id
WHERE
	a.business_unit_id = @warehouse_bu_id AND 
	a.type_id = 2)			
ORDER BY type_id ASC, qty DESC, max DESC";
			#endregion !auth_new_location
		}

		((ASPxDateEdit)Popup_GroupSelect.FindControl("date_required")).Visible = false;
		((Label)Popup_GroupSelect.FindControl("lb_required")).Visible = false;

		btn_Summarizelabor.Visible = member_can_see_labour_cost && current_user.business_unit.summarize_labor;
		//this.btn_Summarizelabor.ClientEnabled = current_user.business_unit.summarize_labor;
	   // this.chkSummarizelabor.Checked = issummarize_labor;

		hidForceClickable.Value = "";
		//					rp_Main.Visible = true;
		//					rp_Main.HeaderStyle.BackColor = System.Drawing.Color.Orange;
		//			header_label.Text = string.Format("{0}: BVWO {1}-{2}", temp_comp.name, woprogress.OrderNumber, woprogress.CustomerName);
		//			headersub_label.Text = woprogress.Description.Trim().Replace("\n\n", "\n");
		lblMemberName.Text = "Project Manager:";
		ImgBtn_Print.Visible = false;
		//		this.FindControl("ImgBtn_Print").Visible = false;
			DiscountAmount = 0;
			hidWODiscount.Value = "0";
		pnl_header.Visible = false;
		pnl_WODetails.Visible = true;
		WOTotalsPanel.Visible = true;
		if (member_can_see_sell || current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewDollarTotalsAllBranches)) // can the user see cost info?
		{
			TotalsTable.Visible = true;
		}
		pnl_addnewline.Visible = true;
		if (wo.IsProgressBill)
		{
			//pnl_addnewline.Visible = false;

		}
		if (wo.woprog_hold == 1 || wo.labor_only)
		{
			pnl_addnewline.Visible = false;
			addline_visible_reason.InnerHtml = "<div style='font-size:15px;text-align:center;font-family:arial black;font-weight:bold;color:red;padding:10px;'>You cannot add items to this work order while it is on hold or it is set to Labor Only.</div>";
			agv.Columns[GVColumns.Select].Visible = false;
		}

		priv_nesi_invoiced_edit = (wo.Status == OpsWOStatus.Invoiced || wo.Status == OpsWOStatus.WaitingForPO || wo.Status == OpsWOStatus.WaitingToBeInvoiced) && IsBackOffice && member_can_see_cost && member_can_see_labour_cost;
		if (wo.Status == OpsWOStatus.WaitingToBeInvoiced ||
			wo.Status == OpsWOStatus.WaitingParentBMApproval ||
			wo.Status == OpsWOStatus.WaitingForPO ||
			wo.Status == OpsWOStatus.Invoiced || 
			wo.Status == OpsWOStatus.Deleted ||
			wo.Status == OpsWOStatus.ClosedReassigned)
		{


			addline_visible_reason.InnerHtml =
				"<div style='font-size:15px;text-align:center;font-family:arial;font-weight:bold;color:red;padding:10px;'>You cannot add items to this work order while it is in the status \"" +
				wo.Status + "\".</div>";
			pnl_addnewline.Visible = false;

			agv.Columns[GVColumns.Select].Visible = false;
			btn_Save.Visible = false;


		}
		if ((wo.Status == OpsWOStatus.WaitingForPO || wo.Status == OpsWOStatus.WaitingParentBMApproval) && IsBackOffice)
		{
			pnl_addnewline.Visible = false;
			agv.Columns[GVColumns.Select].Visible = true;
		}
		if (priv_nesi_invoiced_edit)
		{
			pnl_addnewline.Visible = true;
			addline_visible_reason.InnerHtml = "<div style='font-size:15px;text-align:center;font-family:arial;font-weight:bold;color:red;padding:10px;'>You can only add costs to this work order as it invoiced, or about to be invoiced.</div>";
			addline_visible_reason.Visible = false;
		}

		agv.Settings.ShowFilterRow = true;


		agv.Columns[GVColumns.Transfer].Visible = !wo.IsRebill && !wo.IsCredit;
		//	agv.TotalSummary.Clear();
		if (new List<string>(new[]{ OpsWOStatus.Invoiced,
													OpsWOStatus.WaitingToBeInvoiced,
													OpsWOStatus.WaitingForPO,
													OpsWOStatus.WaitingParentBMApproval
													}).Contains(wo.Status))  // if the work order is already invoiced
		{
			if (priv_nesi_invoiced_edit)
			{
				gridtoggle(new[] {	GVColumns.Select, 
									GVColumns.Location, 
									GVColumns.QtyCommittedToDate, 
									GVColumns.Unfulfilled, 
									GVColumns.Transfer, 
									GVColumns.AvailableQuantity, 
									GVColumns.TrackPart,
									GVColumns.Buttons,
									GVColumns.TrackPart
									}, false);
			}
			else
			{
				gridtoggle(new[] {	GVColumns.Select, 
									GVColumns.Location, 
									GVColumns.QtyReceiving, 
									GVColumns.Unfulfilled, 
									GVColumns.Transfer, 
									GVColumns.AvailableQuantity, 
									GVColumns.TrackPart, 
									GVColumns.MembertypeName,
									GVColumns.Buttons,
									GVColumns.TrackPart
									}, false);
			}
		}
		else if (new List<string>(new[]{    OpsWOStatus.WaitingParentBMApproval,
													OpsWOStatus.WaitingPMApproval,
													OpsWOStatus.Rework,
													OpsWOStatus.InitialPrep,
													OpsWOStatus.QuestionsForPM,
													OpsWOStatus.WaitingForPO
													}).Contains(wo.Status))  // if the work order is being processed or it has been processed
		{
			gridtoggle(new[] {	GVColumns.Unfulfilled, 
								GVColumns.TrackPart}, false);

		}
		gridtoggle(new [] { GVColumns.ActivityCode, GVColumns.ClientPO, GVColumns.ClientWO, GVColumns.CostElement}, WorkingBusinessUnit.ShowCustomBillingColumns);
		if (!member_can_see_sell || !current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewCostInformationOnWorkOrders))
		{
			gridtoggle(new[] { GVColumns.Margin }, false);
		}
		(agv.Columns[GVColumns.Unfulfilled] as GridViewDataTextColumn).PropertiesTextEdit.DisplayFormatString = "##";
		(agv.Columns[GVColumns.Unfulfilled] as GridViewDataTextColumn).EditFormSettings.Visible = DefaultBoolean.False;

		// 1009 [NESI QA] Transfers should be limited to WOs within you Sub.
		// var associated_business_unit_ids = NeBusinessUnit.warehouse_associated_business_unit_ids(WorkingBusinessUnit.warehouse_bu_id);
		var associated_business_unit_ids = "";
		if (wo != null && wo.business_unit_id > 0)
		{
			associated_business_unit_ids = wo.business_unit_id.ToString();
		}

		ds_open_workorders.SelectCommand = string.Format(@"
(SELECT 
	woprog_id woprogid, 
	CAST(CONCAT('0', woprog_bvwo/1, ' ',woprog_customername, '-', woprog_description, '-', woprog_status) AS CHAR) WorkOrder 
FROM 
	woprog 
WHERE 
	find_in_set(business_unit_id, '{0}') AND 
	woprog_status NOT IN ('Invoiced','Waiting To Be Invoiced', 'Waiting PM Approval', 'Waiting BM Approval','Questions For PM','Deleted','Waiting For PO','Waiting Parent BM Approval' ,'Closed/Reassigned') AND
	woprog_bvwo != 'Not Entered' AND 
	woprog_associate_woprog_id = 0 AND 
	woprog_hold = 0 AND
	woprog_iscredit = 0 AND
	woprog_isrebill = 0 AND
	woprog_id != {1} 
ORDER BY 
	woprog_customername,
	woprog_bvwo)
", associated_business_unit_ids, main_id);

		lblQuoted.Text = "Work Order Total:";
		if (wo.Status != OpsWOStatus.Open)
		{
			if ((	wo.Status == OpsWOStatus.Invoiced || 
					wo.Status == OpsWOStatus.WaitingForPO || 
					wo.Status == OpsWOStatus.WaitingToBeInvoiced
					) && wo.IsJobCost)
			{
				//addtoggle(new[] {	2, 5, 7, 11, 12, 14, 15, 16, 17, 19, 21, 22, 23, 25, 26, 27 }, false);



			}
			else
			{
				addtoggle(new[] { AddColumns.DDLs }, false);
			}

		}
		else
		{
			addtoggle(new[] { AddColumns.DDLs }, false);
		}
		Session["addline_business_unit_id"] = WorkingBusinessUnit.id;
		Session["addline_master_id"] = "";


		lblQuoted.Visible = false;
		lblCost.Visible = false;
		lblCost.Width = 0;
		lblTotalCustom.Visible = false;
		lblTotalCustomDisp.Visible = false;
		lblMargAbov.Visible = false;
		lblMargAbovCos.Visible = false;
		lblMargAbovCosDisp.Visible = false;
		lblTotalMargAbovDisp.Visible = false;
		//	lbCommentPopup.Visible = true;
		//	lbCommentPopup.Text = @"<a href='javascript:boing(""/sections/workorder/wocomments.aspx?woid=" + id + @"&hold=0"", ""PurchaseOrderPickList"", 675, 525);'>Comments</a>";

		btn_comment.Visible = true;
		btn_comment.Attributes.Add("onclick", "javascript:boing('/sections/workorder/wocomments.aspx?woid=" + id + @"&hold=0', 'PurchaseOrderPickList', 675, 525);");

		lblTM_Sell.Text = "Sell";
		TextCost.ClientEnabled = false;
		TextDescription.Enabled = true;

		if (wo.IsProgressBill)
		{
			if (!IsBackOffice)
			{
				pnl_addnewline.ClientVisible = false;
				agv.Columns[GVColumns.BillType].Visible = false;
			}
		}
		
		TextSell.ClientEnabled = current_user.AuthenticatedForPrivilege(OpsPrivilege.ChangeWoLinePrice) && wo.Status != OpsWOStatus.Open;
		if ((wo.Status == OpsWOStatus.Invoiced
				|| wo.Status == OpsWOStatus.WaitingToBeInvoiced
				|| wo.Status == OpsWOStatus.WaitingForPO 
				|| wo.Status == OpsWOStatus.WaitingParentBMApproval
				) && !wo.IsProgressBill)
		{
			hidWODelete.Value = "false";
			agv.Columns[GVColumns.QtyReceiving].Visible = false;
		}
		else
		{
			if (IsBackOffice)
			{

				btnSet0qtystoNoCharge.Visible = true;
				btnSetItemsToJobCostforQuote.Visible = true;
				btnSetToRegular.Visible = true;
				b_warranty.Visible = true;
				//SetallitemstoDoNotInclude.Visible = true;
			}
			hidWODelete.Value = "true";
		}

		if (wo.woprog_id > 0 && wo.woprog_iscredit == 1)
		{
			// This is the credit wo, we will have load 4, 5 and 6 as Matt suggested.
			// The 1 and 2 are hard coded in aspx page.
			hiddenIsCreditWorkerOrder.Value = "1";
		}
		else
		{
			ddlMatType.Items.Add(new ListItem("Group", "4"));
			ddlMatType.Items.Add(new ListItem("Quotes", "5"));
			ddlMatType.Items.Add(new ListItem("Work Orders", "6"));
			hiddenIsCreditWorkerOrder.Value = "0";
		}
		
		FillForWo(main_id);
		ddlMatType.DataBind();
		lblQuoted.Visible = false;
		lblQuotedDisp.Visible = false;
		lblTM.Visible = false;

		lbl_benchextddiff.Visible = false;
		lblTMDisp.Visible = false;
		WOTotalsPanel.Visible = true;
		lblLineTotals.Visible = true;
		lblLabour1.Visible = true;
		lblMaterial1.Visible = true;
		lblTotal1.Visible = true;
		lblLabour2.Visible = true;
		lblMaterial2.Visible = true;
		lblTotal2.Visible = true;
		lblBTTotals.Visible = true;
		header_printwo.Visible = true;
		header_printwo_unf.Visible = true;
		if (wo.QuoteID == "0" && wo.woprog_ERID == 0)
		{
			lblLineTotals.Visible = true;
			lblLabour1.Visible = true;
			lblMaterial1.Visible = true;
			lblTotal1.Visible = true;
			lblRegular1.Visible = true;
			lblRegularLabour2.Visible = true;
			lblRegularMaterial2.Visible = true;
			lblRegularTotal2.Visible = true;
			lblVNC1.Visible = true;
			lblVNCLabour2.Visible = true;
			lblVNCMaterial2.Visible = true;
			lblVNCTotal2.Visible = true;
			lblBlended1.Visible = false;
			lblBlendedLabour2.Visible = false;
			lblBlendedMaterial2.Visible = false;
			lblBlendedTotal2.Visible = false;
			lblICR1.Visible = true;
			lblICRLabour2.Visible = true;
			lblICRMaterial2.Visible = true;
			lblICRTotal2.Visible = true;
			lblDNI1.Visible = true;
			lblDNILabour2.Visible = true;
			lblDNIMaterial2.Visible = true;
			lblDNITotal2.Visible = true;
			lblVC1.Visible = true;
			lblVCLabour2.Visible = true;
			lblVCMaterial2.Visible = true;
			lblVCTotal2.Visible = true;
		}
		else
		{
			lblLineTotals.Text = "Bill Types";
			lblRegular1.Text = "Benchmark Totals:";
			lblLabour1.Visible = true;
			lblMaterial1.Visible = true;
			lblTotal1.Visible = true;
			lblRegular1.Visible = true;
			lblRegularLabour2.Visible = true;
			lblRegularMaterial2.Visible = true;
			lblRegularTotal2.Visible = true;
			lblVNC1.Text = "Extra Totals:";
			lblVNC1.Visible = true;
			lblVNCLabour2.Visible = true;
			lblVNCMaterial2.Visible = true;
			lblVNCTotal2.Visible = true;
		}
		isallowed_jobcosts_negative = wo.QuoteID != "0" && member_can_see_labour_cost && IsBackOffice;



	}
	protected void Page_Load(object sender, EventArgs e)
	{

		Toolbox.do_add_css(Page, "/css/picklist.css");
		Toolbox.do_add_css(Page, "/css/jquery.tip.css");
		Toolbox.do_add_css(Page, "/css/jquery_custom_mods.css");
		Toolbox.do_add_css(Page, "/css/autocomplete.css");
		
		Session.Add("member_id", current_user.id.ToString());
		var iframe = _q["iframe"] != null ? _q["iframe"] : "";
		TextCost.Attributes.Add("onkeydown", "only_numeric(event);");
		TextCost.ClientSideEvents.TextChanged = "addline_cost_handler";
		//TextCost.Attributes.Add("onblur", "cost_handler(this)");
		if (!current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewTotalTimeAndMaterialValues))
		{
			TextSell.ClientVisible = false;
			TextTMExtd.ClientVisible = false;
		}

		lblTotalLabor.Visible = lblTotalMaterial.Visible = lblTotalMaterialDisp.Visible = lblTotalLaborDisp.Visible = lblTotalQuotedLaborDisp.Visible = lblTotalQuotedLabor.Visible = false;
		var event_target = Page.Request.Params.Get("__EVENTTARGET");
		//show_debug_column_info();
		if (!IsPostBack)
		{
			clear_boxes(true);

			if (iframe != "yes")
			{
				ScriptManager1.SetFocus(TextPartNo.ClientID);
			}
		}
		
		#region If the gridview was the control, refresh the page, otherwise don't
		if (string.IsNullOrEmpty(callback_param))
		{
			callback_param = "";
		}
		if (!string.IsNullOrEmpty(event_target))
		{
			//_tools.debug_note(this_origin);
			ctrlname = Page.FindControl(event_target);
			if (ctrlname != null)
			{
				if (agv.EditingRowVisibleIndex == -1) // If we aren't editing.
				{
					FillForWo(main_id);
				}
			}
			else if (main_rev != 0 || main_id != 0)
			{
				var control = GetAsyncPostBackControlID();
				if (!control.Contains("btnUpdatePORecQty") && !control.Contains("bt_mass_date_req") && !control.Contains("bt_mass_billtype_edit") && !callback_param.Contains("delete_multiple"))
				{
					FillForWo(main_id);
				}
			}
		}
		#endregion
		set_header_widths();

		header_printwo.Attributes["onclick"] = string.Format("boing('/sections/workorder/wo_shopping_cart.aspx?woid={0}', 'print', 750, 960);", main_id);
		header_printwo_unf.Attributes["onclick"] = string.Format("boing('/sections/workorder/wo_shopping_cart.aspx?woid={0}&unf=1', 'print', 750, 960);", main_id);
		//header_print.Attributes["onclick"]			= string.Format("boing('/sections/reports/print_quote_worksheet/index.aspx?origin={0}&quoteid={1}&revision={2}&stuff=85&Q={3}&T={4}', 'print', 1024, 960);", origin,main_id,rev,lblQuotedDisp.Text, lblTMDisp.Text);
		if (!can_commit)
		{
			gridtoggle(new[] { GVColumns.Location, GVColumns.QtyReceiving, GVColumns.TrackPart, GVColumns.MembertypeName }, false);
			TextComQty.ReadOnly = true;
		}

	}
	private DataTable dt_master_locations;
	private string _wo_status = "";
	private bool _process_locations = true;
	/// <summary>
	/// (Matt) I offloaded this query to its own method to simplify the set_location_ds method.
	/// </summary>
	/// <param name="csv_parts"></param>
	/// <returns></returns>
	private string location_ds_query(string csv_parts)
	{
		/*
		SELECT b.location_master_id, b.master_id, b.qty, b.min, b.max FROM 
		inventory_location b 
			WHERE
				b.master_id  IN ({0}) AND 
				b.business_unit_id = {1}
		) b ON a.id = b.location_master_id
		 */
		return auth_new_location ? string.Format(@"
	SELECT 
		a.master_id,
		b.id, 
		CONCAT(IF(b.type_id = 1, 'In - ', 'Ex - '), b.name,' - (', IFNULL(c.qty,0), ')') name,
		IFNULL(c.qty, 0) qty,
		b.type_id,
		b.name rawname,
		c.min,
		c.max 
	FROM 
		inventory_item_master a 
	LEFT JOIN 
		inventory_location_master b ON b.business_unit_id = {1}
	LEFT JOIN
		inventory_location c ON a.master_id = c.master_id AND b.id = c.location_master_id
	WHERE 
		a.master_id IN ({0})
	GROUP BY master_id, id
	ORDER BY master_id,id",
									csv_parts,
									WarehouseBusinessUnit.id
									) : string.Format(@"
	(SELECT
		IFNULL(a.master_id, 0) master_id,
	b.id,
	CONCAT(IF(b.type_id = 1, 'In - ', 'Ex - '), CONCAT(b.name),' (',IFNULL(a.qty,0),')')  name,
	IFNULL(a.qty, 0) qty,
	b.type_id,
		b.name rawname,
		a.min,
		a.max 
FROM
	inventory_location a
INNER JOIN
	inventory_location_master b 
		ON	b.id = a.location_master_id AND 
			a.master_id IN ({0}) AND a.business_unit_id = {1} AND b.type_id = 1
LEFT JOIN
	business_unit c ON a.business_unit_id = c.id
WHERE
	 b.business_unit_id = {1} AND
	IFNULL(a.master_id, 0) != 0
order by a.max,a.min DESC, b.name
) 
UNION
(SELECT
		IFNULL(b.master_id, 0) master_id,
	a.id,
	CONCAT(IF(a.type_id = 1, 'In - ', 'Ex - '), CONCAT(a.name),' (',IFNULL(b.qty,0),')')  name,
	IFNULL(b.qty, 0) qty,
	a.type_id,
		a.name rawname,
		b.min,
		b.max
FROM
	inventory_location_master a
INNER JOIN
	(
	SELECT 
		bb.location_master_id, 
		bb.master_id, 
		bb.qty, 
		bb.min, 
		bb.max 
	FROM 
		inventory_location bb 
	WHERE 
		bb.master_id IN ({0}) AND 
		bb.business_unit_id = {1}
	) b ON a.id = b.location_master_id
LEFT JOIN
	business_unit c ON a.business_unit_id = c.id
WHERE
	a.business_unit_id = {1} AND 
	a.type_id = 2 AND
	IFNULL(b.master_id, 0) != 0
order by b.max,b.min DESC, a.name
)", csv_parts, WarehouseBusinessUnit.id);
	}


	private void set_location_ds(object sender, EventArgs e)
	{
		var agv = (ASPxGridView)sender;
		var workorder_column = (GridViewDataColumn)agv.Columns[GVColumns.Location];   
		var start = agv.VisibleStartIndex;
		var end = agv.VisibleRowCount <= agv.SettingsPager.PageSize
												? agv.VisibleRowCount
												: (agv.PageIndex + 1) * agv.SettingsPager.PageSize;
		if (end > agv.VisibleRowCount)
		{
			end = agv.VisibleRowCount;
		}
		var part_numbers = new List<string>();
		for (var wi = start; wi < end; wi++)
		{
			var part_nu = agv.GetRowValues(wi, GVColumns.MasterId).ToString() == ""
							? 0
							: Convert.ToInt32(agv.GetRowValues(wi, GVColumns.MasterId).ToString());
			if (part_nu < 900000 && part_nu != 0)
			{
				part_numbers.Add(agv.GetRowValues(wi, GVColumns.MasterId).ToString());
			}
		}
		if (part_numbers.Count > 0)
		{
			dt_excludes = Toolbox.doSQL_dt(string.Format(@"SELECT a.master_id, b.is_exclude, IFNULL(b.allowed_to_stock, 0) allowed_to_stock FROM inventory_item_master a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id WHERE a.master_id IN ({0})", string.Join(",", part_numbers.ToArray())), null);
		}
		if ((dt_master_locations == null || IsCallback || is_adding) && part_numbers.Count > 0)
		{
			var woprog_status = Toolbox.doSQL_string(@"SELECT woprog_status FROM woprog WHERE woprog_id = @v0  LIMIT 1", new object[] { main_id });
			var csv_parts = string.Join(",", part_numbers.ToArray());
			if (woprog_status == OpsWOStatus.Invoiced || woprog_status == OpsWOStatus.WaitingToBeInvoiced)
			{
				_wo_status = woprog_status;
				_process_locations = false;
			}
			if (_process_locations)
			{
				dt_master_locations = Toolbox.doSQL_dt(location_ds_query(csv_parts), null);
			}
		}
		for (var i = start; i < end; i++)
		{
			var origin = Toolbox.ReturnBlankIfNull_string(agv.GetRowValues(i, GVColumns.Origin));
			var isImportedWIP = origin.Contains(OpsWOLineOrigin.ImportedWIP);
			var dataRow = agv.GetDataRow(i);
			var partiallyBilled = dataRow != null && Toolbox.ReturnZeroIfNull_int(dataRow[GVColumns.PartiallyBilled]) == 1;
			// The idea here is to loop through each visible row and set the location combo's data source dynamically from a preloaded data table
			var part_no = 0;
			var initial_filter = "";
			var default_sort = "type_id ASC, qty DESC, min ASC, max ASC, rawname ASC";
			if (agv.GetRowValues(i, GVColumns.MasterId) != null)
			{
				int.TryParse(agv.GetRowValues(i, GVColumns.MasterId).ToString(), out part_no);
				initial_filter = auth_new_location
											? "master_id = '" + part_no + "' OR master_id = 0"
											: "master_id = '" + part_no + "'";
			}
			var location_combo = (ASPxComboBox)agv.FindRowCellTemplateControl(i, workorder_column, "combo_location");          
				if (location_combo != null && (location_combo.Items.Count == 0 || IsPostBack))
				{
					var show_location = false;
					if (_process_locations)
					{
						if (part_no < OpsSpecialPart.LaborThreshold)
						{
							if (dt_excludes != null && dt_excludes.Rows.Count > 0)
							{
								var dr = dt_excludes.Select("master_id = '" + part_no + "'");
								if (dr.Count() > 0 && dr[0].ItemArray.Length > 0)
								{
									var is_exclude = Convert.ToInt32(dr[0]["is_exclude"]);
									var allowed_to_stock = Convert.ToInt32(dr[0]["allowed_to_stock"]);
									if (is_exclude == 0 && allowed_to_stock == 1 && !partiallyBilled)
									{
										var dr_locations = dt_master_locations.Select(initial_filter, default_sort);
										if (dr_locations.Length > 0)
										{
											location_combo.DataSource = dr_locations.CopyToDataTable();
											location_combo.DataBind();
										}
										if (callback_param != null || !IsPostBack)
										{
											location_combo.SelectedIndex = 0;
										}
										location_combo.Native = location_combo.Items.Count < 50;
										show_location = true;                                
									}
								}
							}
						}
					}
					location_combo.Visible = show_location;
					location_combo.EnableCallbackMode = false;
			}
			if (location_combo != null && isImportedWIP)
			{
					location_combo.ClientEnabled = false;
			}

		}
	}

	private void set_header_widths()
	{
		add_bc2.Width = "45px";
		add_bc26.Width = "85px";
		add_bc4.Width = "50px";
		add_bc23.Width = "35px";
		add_bc6.Style.Add("min-width", "50px");
		add_bc8.Width = add_bc9.Width = add_bc10.Width = add_bc13.Width = add_bc24.Width = "48px";
		add_bc27.Width = "70px";
	}

	private string GetAsyncPostBackControlID()
	{
		var smUniqueId = ScriptManager.GetCurrent(Page).UniqueID;
		var smFieldValue = Request.Form[smUniqueId];

		if (!string.IsNullOrEmpty(smFieldValue) && smFieldValue.Contains("|"))
		{
			return smFieldValue.Split('|')[1];
		}

		return "";
	}
	private void addtoggle(int[] col_n, bool show)
		{
		if (col_n.Length == 0)
			{
			for (var i = 1; i < 27; i++)
				{
				var hc = FindControlRecursive(Page, $"add_hc{i}");
				if(hc != null)
					{ 
					hc.Visible = show;
					}
				var bc = FindControlRecursive(Page, $"add_bc{i}");
				if(bc != null)
					{ 
					bc.Visible = show;
					}
				}
			}
		for (var i = 0; i < col_n.Length; i++)
			{
			try
				{
				var hc = FindControlRecursive(Page, $"add_hc{col_n[i]}");
				if(hc != null)
					{ 
					hc.Visible = show;
					}
				var bc = FindControlRecursive(Page, $"add_bc{col_n[i]}");
				if(bc != null)
					{ 
					bc.Visible = show;
					}
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog(ee, "There was an issue toggling the visibility state for an add line column ");
				throw new Exception($"There was an issue toggling the visibility state for an add line column ({i})");
				}
			}
		}
	private void fillCustomers()
		{
		cbAddlineCustomer.DataSource = Toolbox.doSQL_dt(@"
SELECT 
	d.customer_ID as ID, 
	d.customer_name as Name,
	COUNT(b.wo_detail_current_id) count_b,
	COUNT(c.wo_detail_history_id) count_c
FROM 
	woprog a
LEFT JOIN 
	wo_detail_current b 
		ON	a.WOProg_ID = b.wo_detail_current_woprog_id and 
			b.wo_detail_current_type = 'M' 
LEFT JOIN 
	wo_detail_history c 
		ON	a.WOProg_ID = c.wo_detail_history_woprog_id and 
			c.wo_detail_history_type = 'M'
LEFT JOIN
	customer d
		ON a.woprog_customer_id = d.customer_id
WHERE 
	a.business_unit_id = @v0 AND 
	a.woprog_status NOT IN ('', 'Deleted','Closed/Reassigned') AND
	a.woprog_customer_id IS NOT NULL
GROUP BY 
	a.WOProg_customer_id 
HAVING 
	count_b > 0 OR count_c > 0
ORDER BY 
	d.customer_name", new object[] { WorkingBusinessUnit.id });;
		cbAddlineCustomer.DataBind();
		var lic = new ListEditItem("Select Customer", 0);
		cbAddlineCustomer.Items.Insert(0, lic);
		cbAddlineCustomer.SelectedIndex = 0;
		}
	private void footer_save_toggle(bool show_client)
	{
		footer_save_toggle(show_client, agv);
	}
	private void footer_save_toggle(bool show_client, ASPxGridView gv)
	{
		var button_id = show_client 
								? "btnUpdatePORecQty_client" 
								: "btnUpdatePORecQty";
		var gvdc = (GridViewDataColumn)gv.Columns[GVColumns.QtyReceiving];
		var footer_save = (ASPxButton)gv.FindFooterCellTemplateControl(gvdc, button_id);
		if (footer_save != null)
		{
			if (show_client)
			{
				button_id = "btnUpdatePORecQty_client";
			}
			which_footer_save.Value = button_id;
		}
	}
	private void gridtoggle(string[] col_n, bool show)
	{
		if (col_n.Length == 0)
		{
			for (var i = 0; i < agv.Columns.Count; i++)
			{
				agv.Columns[i].Visible = show;
			}
		}
		for (var i = 0; i < col_n.Length; i++)
		{
			var col = agv.Columns[col_n[i]];
			col.Visible = show;
		}
	}
	private static Control FindControlRecursive(Control parent, string _id)
	{
		var control_found = parent.FindControl(_id);
		var i = 0;
		while (control_found == null && i < parent.Controls.Count)
		{
			control_found = FindControlRecursive(parent.Controls[i++], _id);
		}
		return control_found;
	}
	#region Button Press Code
	private void update_error(string err)
	{
		ErrorLabel.InnerHtml = err;
		ErrorLabel.Style["display"] = err == "" ? "none" : "block";
	}
	private void update_info(string nfo)
	{
		InformationLabel.InnerHtml = nfo;
		InformationLabel.Style["display"] = nfo == "" ? "none" : "block";
	}
	protected void add_line_save(object sender, EventArgs e)
	{
		var checkinv = new inventory();
		update_error("");
		is_adding = true;
		#region Quote/Work order/purchase order/kitted
		#region Part Number
		newpartnumber = TextPartNo.Text;
		var partno = 0;
		int.TryParse(newpartnumber, out partno);
		// Capturing list of member types from BM to PM level - Task 1718 Ability to add Pure Revenue Lines
		var dtlist = Toolbox.doSQL_dt("SELECT membertype_id id FROM membertype WHERE reports_to IN(5,38) AND active", null);
		var isPMorAbove = false;
		
		if (partno == OpsSpecialPart.MiscMaterial)
		{ 
		foreach (DataRow dr in dtlist.Rows)
		{
			var thisMembertypeId = (int)dr["id"];
			var consideredPM = Toolbox.doSQL_bool("SELECT considered_pm FROM membertype WHERE membertype_id = @v0", new object[] { current_user.MemberTypeID });
			isPMorAbove = consideredPM || NeMemberType.is_supervisor_mt(thisMembertypeId,current_user.MemberTypeID);
		}
		}
		// Check for the level above BM - Task 1718
		if (!isPMorAbove)
		{
			isPMorAbove = current_user.MemberTypeID==OpsMemberTypes.ProjectManager || NeMemberType.is_supervisor_mt(OpsMemberTypes.ProjectManager, current_user.MemberTypeID);
		}
		 var canAddMiscMat = IsBackOffice || isPMorAbove || current_user.membertype.reports_to == OpsMemberTypes.BranchManager;
		if(partno == OpsSpecialPart.MiscMaterial && !Toolbox.Contains(wo.Status, new[] { OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced, OpsWOStatus.WaitingForPO }) && !canAddMiscMat)
		{            
			update_error("You are not allowed to add Pure Revenue Line to this Work order");
			return;
		}

		if (newpartnumber != "" && ddlMatType.SelectedValue != "9" &&  partno != 0)
		{
			if (!checkinv.part_exists(newpartnumber) && partno < OpsSpecialPart.LaborThreshold)
			{
				update_error("The Part Number You Are Trying To Use Does Not Exist.");
				return;
			}
		}
		else if (TextDescription.Text == "" && ddlMatType.SelectedValue != "9")
		{
			update_error("There Is An Issue With This Part Number.");
			return;
		}

		if (checkinv.part_is_Rental_Item(newpartnumber))
		{
			update_error("Rental Items cannot be added from work orders.");
			return;
		}

		if (checkinv.part_exists(newpartnumber))
		{
			checkinv.Load(newpartnumber, WarehouseBusinessUnit.id);
		}
		if (partno == OpsSpecialPart.QuoteLine && wo.IsIntercompany)
		{
			update_error("Cannot add manual quote lines to intercompany work orders");
			return;
		}
		if(checkinv.Tag.id == OpsSpecialTag.NonStockItem && newCommittedQuantity > 0)
		#endregion Part Number
		#region Description
		if (partno == OpsSpecialPart.MakeThisPart)
		{
			newdescription = TextDescription.Text;
		}
		else if (newpartnumber != "" && partno < OpsSpecialPart.LaborThreshold)
		{
			checkinv.Load(partno, WarehouseBusinessUnit.id);
			if (checkinv.is_exclude)
			{
				newdescription = TextDescription.Text;
			}
			else if (ddlMatType.SelectedValue == "9")
			{
				newdescription = TextDescription.Text;
			}
			else
			{
				newdescription = checkinv.description;
			}
		}
		else
		{
			newdescription = TextDescription.Text;
		}
		if (newdescription == "")
		{
			update_error("You must have a description for a part");
			ScriptManager1.SetFocus(TextDescription);
			return;
		}
		#endregion Description
		#region Date required
		if (DateRequired.Date != null)
		{
			newdatereq = Toolbox.MySQL_shortdt(DateRequired.Date);
		}
		#endregion Date required
		#region Cost
		var comqty = TextComQty.Text == "" ? 0 : Convert.ToDouble(TextComQty.Text);
		if (partno != OpsSpecialPart.MakeThisPart)
		{
			if (partno < OpsSpecialPart.LaborThreshold)
			{
				newcost = Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 ,@v1 ,0, @v2 )", new object[] { partno, WarehouseBusinessUnit.id, comqty });
				if (checkinv.is_exclude || partno == 0)
				{
					cost_level = 0;
					newcost = TextCost.Text != "" ? Convert.ToDouble(TextCost.Text) : 0;
				}
				else
				{
					cost_level = Toolbox.doSQL_int(@"SELECT GET_COST(@v0 ,@v1 ,true)", new object[] { partno, WarehouseBusinessUnit.id });
				}
			}
			else if (partno >= OpsSpecialPart.LaborThreshold && partno < 2000000)
			{
				cost_level = 0;
				newcost = 0;
			}


		}
		else
		{
			cost_level = 0;
			newcost = 0;
		}
		if (newcost < 0)
		{
			update_error("Costs cannot be less than 0. If this is a credit, enter a negative quantity and a positive cost.");
			ScriptManager1.SetFocus(TextCost);
			return;
		}
		#endregion Cost
		#region Quantity
		try
		{
			if (TextQty.Text == null || TextQty.Text == "")
			{
				TextQty.Text = "0";
				if (ddlMatType.SelectedValue == "2")
				{
					if (TextComQty.Text != null && TextComQty.Text != "")
					{
						TextQty.Text = TextComQty.Text;
					}
				}

			}
			if(!checkinv.is_exclude && newCommittedQuantity<0)
			{
				update_error("Negative committed quantities are only allowed on inventory exclude parts, please fix.");
				ScriptManager1.SetFocus(TextQty);
				return;

			}

			newReqQuantity = Convert.ToDouble(TextQty.Text);
			if (newReqQuantity == 0 && comqty == 0)
			{
				update_error("Invalid Required Quantity of 0, please fix.");
				ScriptManager1.SetFocus(TextQty);
				return;
			}

			if (TextComQty.Text == null || TextComQty.Text == "")
			{
				TextComQty.Text = "0";
			}
			newCommittedQuantity = Convert.ToDouble(TextComQty.Text);
		}
		catch
		{
				update_error("Invalid Qty");
				ScriptManager1.SetFocus(TextQty);
				return;
		}
		#endregion Quantity
		#region Original Sell Price
		if (partno != OpsSpecialPart.MakeThisPart)
		{
			try
			{
				var used_cost = newcost == 0 & checkinv.cost_price_branch > 0 ? checkinv.cost_price_branch : newcost;
				double exists_chk = 0;
				try
				{
					exists_chk = Toolbox.doSQL_double(@"SELECT ifnull(sum(wo_detail_current_qty_committed),0) FROM wo_detail_current WHERE wo_detail_current_master_id = @v0  AND wo_detail_current_woprog_id = @v1  ", new object[] { partno, main_id });
				}
				catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }

				neworigsell = checkinv.is_exclude || partno >= OpsSpecialPart.LaborThreshold ? Toolbox.do_Round(Convert.ToDouble(TextSell.Text), 2) : wo.use_fixed_material_markup ? wo.fixed_material_markup * used_cost : shared.GetSellPrice(used_cost, 0, true, newCommittedQuantity + exists_chk, WorkingBusinessUnit.id32);

			}
			catch
			{
				update_error("There is a problem with the Sell price");
				return;
			}
		}
		else
		{
			neworigsell = 0;
		}
		#endregion Original Sell Price
		#region Sell Price
		if (partno != OpsSpecialPart.MakeThisPart)
		{
			try
			{
				// If the TM Extended Sell doesn't equal the Quoted Extended Sell and the origin isn't kitted or purchase order and the material type isn't ER
				var used_cost = newcost == 0 & checkinv.cost_price_branch > 0 ? checkinv.cost_price_branch : newcost;
				newsell = checkinv.is_exclude || partno >= OpsSpecialPart.LaborThreshold
							? neworigsell
							: wo.use_fixed_material_markup
								? used_cost * wo.fixed_material_markup
								: shared.GetSellPrice(used_cost, 0, true, newCommittedQuantity, WorkingBusinessUnit.id32);
			}
			catch
			{
				update_error("The Sell Price is not Valid");
				return;
			}
			try
			{
				//
				// partno = SpecialParts.MiscMaterial will not constrain by this rule.
				// This is for adding a zero cost wo line item when status is invoiced.
				//
				if (newsell == 0 && partno < 900000 && partno != OpsSpecialPart.MiscMaterial)
				{
					update_error("You cannot add a part with a 0 sell price");
					return;
				}
			}
			catch
			{
				if (newsell == 0 && partno < 900000)
				{
					update_error("You cannot add a part with a 0 sell price");
					return;
				}
			}
		}
		else
		{
			newsell = 0;
		}
		#endregion Sell Price

		#region Notes
		newNotes = hidNotes.Value;
		#endregion Notes

		xfer_comm_recv_select_id = addline_location.Visible && Convert.ToString(addline_location.Value) != "" ? Convert.ToInt32(addline_location.Value) : 0;
		
		if(xfer_comm_recv_select_id > 0) // Meaning they have selected a location.
			{ 
			var invLocationMaster = new location_master(xfer_comm_recv_select_id);
			var invLocation = new location(checkinv.id, invLocationMaster);

			if(checkinv.Tag.id == OpsSpecialTag.NonStockItem && (invLocation.qty <= 0 || newCommittedQuantity > invLocation.qty ))
				{
				update_error(invLocation.qty == 0 
								? "You cannot add a non-stock part from a location with a zero quantity against it."
								: "You cannot add a quantity for a non-stock part, directly, that is greater than the quantity in stock.");
				return;
				}
			}
		#region Add Line / DataSource Refreshers
		// should we apply discounts?
		var wo_row = Toolbox.doSQL_dt(@"SELECT * FROM woprog WHERE woprog_id = @v0 ", new object[] { main_id }).Rows[0];
		var app_disc = Toolbox.ReturnZeroIfNull_int(wo_row["woprog_apply_discount"]);
		var c_id = Toolbox.ReturnZeroIfNull_int(wo_row["woprog_customer_id"]);
		var a_id = Toolbox.ReturnZeroIfNull_int(wo_row["woprog_address_id"]);
		if (a_id == 0)
		{
			a_id = NECustomer.Get_Default_Billing_Address_Id(c_id);
		}
		if (app_disc == 1 && partno < OpsSpecialPart.LaborThreshold)
		{
			newdiscount = 0;
		}
		if (partno < OpsSpecialPart.LaborThreshold)
		{
			var current_c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current  WHERE wo_detail_current_master_id =@v0 AND wo_detail_current_master_id != 2139 AND wo_detail_current_woprog_id =@v1 ", new object[] { partno, main_id });
			if (current_c > 0 && newCommittedQuantity > 0 && !checkinv.is_exclude)
			{
				update_error("This part already exists as a line item.<br/> You must commit through the receive (Qty Rec'd) column below.");
				return;
			}
		}
		SaveWorkOrderLine(0, partno, true, checkinv.is_exclude, true);
		FillForWo(main_id);
		#endregion Add Line / DataSource Refreshers
		#endregion Quote/Work order/purchase order/kitted
		if (ErrorLabel.InnerText == "")
		{
			clear_boxes(false);
		}
		ScriptManager.RegisterStartupScript(this, GetType(), "binder", "update_grid()", true);
		ScriptManager1.SetFocus(TextPartNo);
	}

	protected void add_line_clear(object sender, ImageClickEventArgs e)
	{
		clear_boxes(true);
	}
	protected void clear_boxes(bool full_reset)
	{
		var status = Toolbox.doSQL_string(@"Select WOProg_status from woprog  where woprog_id =@v0", new object[] { main_id });
		TextPartNo.Text = "";
		TextDescription.Text = "";
		TextDescription.ToolTip = "";
		#region $(this).inventory attach

		if (status != OpsWOStatus.Invoiced && status != OpsWOStatus.WaitingForPO && status != OpsWOStatus.WaitingToBeInvoiced)
		{
			TextDescription.Attributes.Add("onfocus", @"description_onfocus(this)");
		}

		TextDescription.Attributes.Add("onblur", "desc_resize(this, false)");
		#endregion $(this).inventory attach
		TextQty.Text = "";
		TextQty.Enabled = true;
		TextSell.Text = "";
		TextTMExtd.Text = "";
		TextCost.Text = "";
		TextComQty.Text = "";
		TextAvailQty.Text = "0";
		update_error("");
		update_info("");

		Session.Remove("addline_master_id");
		addline_location.DataBind();
		if (full_reset)
		{
			if (Session["picklist_date"] == null)
			{
				Session["picklist_date"] = DateTime.Today.AddDays(10).ToString("yyyy-MM-dd");
			}
			DateRequired.Date = Convert.ToDateTime(Session["picklist_date"]);
			if (main_id != 0)
			{
				try
				{
					var s = Toolbox.doSQL_string(@"Select WOProg_Expected_StartDate from woprog  where woprog_id =@v0", new object[] { main_id });
					if (s != "")
					{
						if (Convert.ToDateTime(s) > DateRequired.Date)
						{
							DateRequired.Date = Convert.ToDateTime(s);
						}
					}

				}
				catch
				{ }

			}
		}
		else
		{
			DateRequired.Date = DateRequired.Date;
		}
		DateRequired.MinDate = DateTime.Today;

		DateRequired.ToolTip = DateRequired.Date.ToLongDateString();

		hidNotes.Value = "";
		if (!full_reset)
		{
			// MH: I am not completely sure why I added this in here but it seems like overkill... if we have troubles with items not resetting, this is the culprit.
			ddlMatType_SelectedIndexChanged(ddlMatType, null);
		}



		if (status == OpsWOStatus.Invoiced || status == OpsWOStatus.WaitingForPO || status == OpsWOStatus.WaitingToBeInvoiced)
		{
			TextQty.Text = "1";
			ddlMatType.Enabled = false;
			TextCost.ClientEnabled = true;
			TextPartNo.Text = OpsSpecialPart.MiscMaterial.ToString();
			TextPartNo.Enabled = false;
			TextComQty.Text = "1";
			TextSell.ClientEnabled = IsBackOffice;

			//
			// a.	Users can add lines with a price, but the cost must be $0.00.
			//
			TextCost.Text = "0.0";
			TextCost.ClientEnabled = false;

			//
			// The code is need a  sell price, what we do is to set it to zero. The default is sell price on ui can't be edited.
			//
			TextSell.Text = "0.0";

			/*
			 * 5.	The Work Order Details screen should be updated so that privileged users can only add $0.00 cost lines
			 * after a Work Order has been set to “Waiting to Be Invoiced” or “Invoiced” statuses.
			 *
			 * b.	Use part # 55560, but make sure only employees in a business unit marked as is_backoffice = 1 can add these to a WO
			 *
			 */
			// Now you have a invoiced/waitToInvocie wo, you can add a empty wo if you are from backoffice.
			btn_Save.Visible = IsBackOffice;
		}

	}
	protected void ddlMatType_SelectedIndexChanged(object sender, EventArgs e)
	{
		clear_boxes(true);


		matTypeGroup.Visible     = ddlMatType.SelectedValue == "4";
		matTypeQuote.Visible     = ddlMatType.SelectedValue == "5";
		matTypeWorkOrder.Visible = ddlMatType.SelectedValue == "6";


		#region Material (1)
		if (ddlMatType.SelectedValue == "1")  // material
		{
			addtoggle(new[] {	AddColumns.MaterialType,
								AddColumns.PartNo,  
								AddColumns.Description, 
								AddColumns.QuantityRequired,
								AddColumns.QuantityCommitted, 
								AddColumns.Location, 
								AddColumns.QuantityAvailable,
								AddColumns.PriceCost,
								AddColumns.PriceSell,
								AddColumns.PriceExtended,
								AddColumns.RequiredDate,
								AddColumns.SaveLine
								}, true);
			addtoggle(new[] { AddColumns.DDLs }, false);
			ScriptManager1.SetFocus(TextDescription);
		}
		#endregion Material (1)
		#region Groups (4)
		else if (ddlMatType.SelectedValue == "4")   // Grab parts from a group
		{
			addtoggle(new[] {	AddColumns.PartNo,  
								AddColumns.Description, 
								AddColumns.QuantityRequired,
								AddColumns.QuantityCommitted, 
								AddColumns.Location, 
								AddColumns.QuantityAvailable,
								AddColumns.PriceCost,
								AddColumns.PriceSell,
								AddColumns.PriceExtended,
								AddColumns.RequiredDate,
								AddColumns.SaveLine}, false);
			addtoggle(new[] {	AddColumns.MaterialType,
								AddColumns.DDLs }, true);
			lblLabourDescription.Text = "Choose Group";
			ScriptManager1.SetFocus(cbAddlineGroup);
			Populate_GroupSelectDDL();
		}
		#endregion Groups (4)
		#region Quote (5)
		else if (ddlMatType.SelectedValue == "5")   // Grab parts from a Quote
		{
			addtoggle(new[] {	AddColumns.PartNo,  
								AddColumns.Description, 
								AddColumns.QuantityRequired,
								AddColumns.QuantityCommitted, 
								AddColumns.Location, 
								AddColumns.QuantityAvailable,
								AddColumns.PriceCost,
								AddColumns.PriceSell,
								AddColumns.PriceExtended,
								AddColumns.RequiredDate,
								AddColumns.SaveLine}, false);
			addtoggle(new[] {	AddColumns.MaterialType,
								AddColumns.DDLs }, true);
			lblLabourDescription.Text = "Choose Quote";
			ScriptManager1.SetFocus(cbAddlineQuote);
			Populate_QuoteSelectDDL();
		}
		#endregion Quote (5)
		#region Other_WorkOrder (6)
		else if (ddlMatType.SelectedValue == "6")   // Grab parts from a Work Order
		{
			addtoggle(new[] {	AddColumns.PartNo,  
								AddColumns.Description, 
								AddColumns.QuantityRequired,
								AddColumns.QuantityCommitted, 
								AddColumns.Location, 
								AddColumns.QuantityAvailable,
								AddColumns.PriceCost,
								AddColumns.PriceSell,
								AddColumns.PriceExtended,
								AddColumns.RequiredDate,
								AddColumns.SaveLine}, false);
			addtoggle(new[] {	AddColumns.MaterialType,
								AddColumns.DDLs }, true);
			lblLabourDescription.Text = "Choose Work Order";
			ScriptManager1.SetFocus(cbAddlineCustomer);
			fillCustomers();
		}
		#endregion Other_WorkOrder (6)
	}
	protected void fill_edit_form_workorder_ddl()
	{
	}
	protected void TextSell_TextChanged(object sender, EventArgs e)
	{
		int.TryParse(TextPartNo.Text, out var masterId);
		double.TryParse(TextSell.Text, out var newselling);
		double quantityvalue = 0;
		if (TextQty.Value != null)
		{
			double.TryParse(TextQty.Value.ToString(), out quantityvalue);
		}
		double discountnum = 0;
		if (hidWODiscount != null)
		{
			double.TryParse(hidWODiscount.Value, out discountnum);
		}
		discountnum = discountnum == 0 ? 1 : discountnum / 100;
		var newextended = newselling * quantityvalue * discountnum;
		TextTMExtd.Text = newextended.ToString();

		if (masterId == OpsSpecialPart.QuoteLine || masterId == OpsSpecialPart.MiscMaterial)
		{
			TextCost.ClientEnabled = false;
			TextSell.ClientEnabled = true;
			addline_location.ClientEnabled = false;
		}
	}
	protected void imgbnotesupdate_Click(object sender, EventArgs e)
	{
		var sql = "";
		if (hidNotesID.Value == "0")
		{
			hidNotes.Value = textNotes.Text;
		}
		else
		{
			var current_note = Toolbox.doSQL_string(@"SELECT IFNULL(wo_detail_current_notes,'') FROM wo_detail_current  WHERE wo_detail_current_id =@v0", new object[] { hidNotesID.Value });
			var this_note = current_note.Contains("A_") ? current_note.Substring(0, current_note.IndexOf('|')) + "|" + textNotes.Text : textNotes.Text;
			sql = string.Format(@"UPDATE wo_detail_current SET wo_detail_current_notes =  CONCAT(IFNULL(wo_detail_current_notes, ''),'\n--\n[{2}] -', NOW(),'\n', ""{0}"") WHERE wo_detail_current_id = {1}", this_note, hidNotesID.Value, current_user.FullName);

			try
			{
				Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_notes = CONCAT(IFNULL(wo_detail_current_notes, ''),'\n--\n[',@v2,' ] -', NOW(),'\n', @v0 ) WHERE wo_detail_current_id = @v1 ", new object[] { this_note, hidNotesID.Value, current_user.FullName });
				update_error("");
				FillForWo(main_id);
			}
			catch
			{
				update_error("Unable to save note");
			}
		}
		textNotes.Text = "";
		ASPxpuNotes.ShowOnPageLoad = false;
		var sc = ScriptManager.GetCurrent(Page);
		ScriptManager.RegisterClientScriptBlock(stuff, stuff.GetType(), "runtime", "<script type='text/javascript'>setTimeout('agv.PerformCallback()', 100);</script>", false);

	}
	protected void b_refresh_Click(object sender, EventArgs e)
	{
	}
	protected void PartTransfer(object sender, EventArgs e)
	{
		var wo_line_id = "";
		wo_line_id = hidttwoprogid.Value;
		if (xfer_internal_combo.DataSource == null)
		{
			xfer_internal_combo.Value = xfer_internal_hid.Value;
		}
		if (xfer_external_combo.DataSource == null)
		{
			xfer_external_combo.Value = xfer_external_hid.Value;
		}
		xfer_wo_combo.DataBind();
		transferwoprogid			= xfer_wo_combo.Value != null && xfer_wo_qty.Text != "0" ? xfer_wo_combo.Value.ToString() : "9999999";
		int.TryParse(transferwoprogid, out int transfer_to_woprog_id);
		xfer_qty_wo					= xfer_wo_qty.Text == "" ? 0 : Convert.ToDouble(xfer_wo_qty.Text);
		xfer_qty_internal			= xfer_internal_qty.Text == "" ? 0 : Convert.ToDouble(xfer_internal_qty.Text);
		xfer_qty_external			= xfer_external_qty.Text == "" ? 0 : Convert.ToDouble(xfer_external_qty.Text);
		xfer_external_select_id		= xfer_external_combo.Value != null ? Convert.ToInt32(xfer_external_combo.Value) : 0;
		xfer_internal_select_id		= xfer_internal_combo.Value != null ? Convert.ToInt32(xfer_internal_combo.Value) : 0;
		xfer_wo_select_id			= xfer_wo_combo.Value != null ? Convert.ToInt32(xfer_wo_combo.Value) : 0;
		xfer_combined_qty			= xfer_qty_wo + xfer_qty_internal + xfer_qty_external;
		transfernote				= txtTransferReason.Text;
		var is_stocktransfer		= new List<string>(new[] { "9999999", "9999998", "9999997" }).Contains(transferwoprogid);
		if (transferwoprogid != "")
		{
			transferbvwo = is_stocktransfer
								? OpsSpecialWorkOrder.StockTransfer
								: Toolbox.doSQL_string(@"SELECT woprog_BVWO FROM woprog  WHERE woprog_id =@v0", new object[] { transferwoprogid });
		}
		transferbvwo              = (xfer_qty_internal > 0 || xfer_qty_external > 0) && xfer_qty_wo == 0 ? OpsSpecialWorkOrder.StockTransfer : transferbvwo;
		newtableid                = Convert.ToInt32(wo_line_id);
		var details               = new NeWODetailCurrent(newtableid);
		var inv                   = new inventory();
		var qty_remaining         = details.qty_committed - xfer_combined_qty;
		#region PO Transfer Check
		var xfer_master_id        = Session["xfer_master_id"].ToString();
		var xfer_business_unit_id = Session["xfer_business_unit_id"].ToString();
		#endregion PO Transfer Check
		newpartnumber             = details.master_id.ToString(); // because its a new entry on the new work order, make the new part and old part the same.
		oldpartnumber             = details.master_id.ToString();
		var exists                = inv.part_exists(newpartnumber);
		// Not sure this check will ever be hit...
		if (!exists && details.master_id < OpsSpecialPart.LaborThreshold)
		{
			update_error("You Cannot Transfer this Part - It does not Exist in Inventory");
			return;
		}
		var to_details = new NeWODetailCurrent();
		if (details.master_id < OpsSpecialPart.LaborThreshold) // if its not a labour part
		{
			inv.Load(newpartnumber, WarehouseBusinessUnit.id);
			if (!inv.is_exclude && !is_stocktransfer)
			{
				// Not Exclude, get the TO work order line
				var to_line_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(wo_detail_current_id), 0) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transfer_to_woprog_id, newpartnumber });
				if (to_line_id > 0)
				{
					to_details = new NeWODetailCurrent(to_line_id);
					Xfer_to_original_qty = to_details.qty_committed;
				}
			}
		}
		if (inv.is_qty && details.master_id < OpsSpecialPart.LaborThreshold)
		{
			var remainsendqty = qty_remaining == 0 ? 1 : qty_remaining;
			neworigsell = Math.Round(wo.use_fixed_material_markup ? details.cost * wo.fixed_material_markup : shared.GetSellPrice(details.cost, 0, inv.is_qty, remainsendqty, WorkingBusinessUnit.id32), 2); // Should these sell prices be to the second decimal place?
			transfersell = Math.Round(wo.use_fixed_material_markup ? details.cost * wo.fixed_material_markup : shared.GetSellPrice(details.cost, 0, inv.is_qty, xfer_combined_qty + to_details.qty_committed, WorkingBusinessUnit.id32), 2);
			oldsell = neworigsell;
		}
		else // if the item is a labour item..
		{
			neworigsell = details.sell;
			transfersell = neworigsell;
			oldsell = neworigsell;
		}
		newworkorderid         = details.woprog_id.ToString();
		newdescription         = details.description;
		oldorigsell            = details.sell;
		olddescription         = details.description;
		newReqQuantity         = details.qty_ordered;
		oldqty                 = details.qty_ordered;
		new_qty_committed      = qty_remaining;
		old_qty_committed      = details.qty_committed;
		newbilltype            = details.billtypeid.ToString();
		oldbilltype            = details.billtypeid.ToString();
		oldcost                = details.cost;
		oldrecno               = details.rec_no;
		newrecno               = details.rec_no;
		OldTrack               = details.track_part;
		NewTrack               = details.track_part;
		chk_workorder_transfer = true;

		double commmittedValue = xfer_combined_qty;
		double remainingValue  = qty_remaining;

		//
		// [1107] Do not allow expenses, per diems, cc purchases to be transferred if the isMigratedFlag is true, or if the pay period has been processed
		// The code here is to do the check before transfer.
		// 
		var okay = IsExpensePerDiemsCCPurchaseAvailableForTransferIfTheItemIsConsidered(details.origin, details.consignment_id, details.woprog_id, newtableid, DateTime.Now, true);
		if (!okay)
		{
			throw new Exception("The status of the purchase order connected to this work order line has been updated - please refresh your page");
		}

		// 
		// Call IsTransfeAvailableIfPoBehideIt before transfer - instancely checking required from Matt in demo.
		//
		if (IsTransferAvailableIfPoBehindIt(details.woprog_id, details.origin, details.master_id))
		{
			SaveWorkOrderLine(transfer_to_woprog_id, details.master_id, false, false, false);
		}
		else
		{
			// 
			// Multiple transfer is only for Inventory-item, so IsTransfeAvailableIfPoBehideIt is always letting it go.
			// 
			throw new Exception("The status of the purchase order connected to this work order line has been updated - please refresh your page");
		}

		//
		// 1007 [NESI QA] Transfers of PO lines are not updating the original PO. (AddNewLIne -> Split -> FillForWo)
		//
		SplitPOLineItem(details, transfer_to_woprog_id, xfer_master_id, commmittedValue, remainingValue, newtableid, rec_no_from_new_wo_line_item, current_user, wo_detail_current_id_target);

		FillForWo(main_id);
		xfer_wo_qty.Text = "";
		hidTranQTY.Value = "";
		pop_xfer.ShowOnPageLoad = false;
	}
	protected void ToStock(object sender, EventArgs e)
	{
		try
		{
			var wo_line_id = "";

			wo_line_id = hidttwoprogid.Value;
			if (xfer_internal_combo.DataSource == null)
			{
				xfer_internal_combo.Value = xfer_internal_hid.Value;
			}
			if (xfer_external_combo.DataSource == null)
			{
				xfer_external_combo.Value = xfer_external_hid.Value;
			}
			xfer_wo_combo.DataBind();
			transferwoprogid        = xfer_wo_combo.Value != null ? xfer_wo_combo.Value.ToString() : "9999999";
			xfer_qty_wo             = xfer_wo_qty.Text == "" ? 0 : Convert.ToDouble(xfer_wo_qty.Text);
			xfer_qty_internal       = xfer_internal_qty.Text == "" ? 0 : Convert.ToDouble(xfer_internal_qty.Text);
			xfer_qty_external       = xfer_external_qty.Text == "" ? 0 : Convert.ToDouble(xfer_external_qty.Text);
			xfer_external_select_id = xfer_external_combo.Value != null ? Convert.ToInt32(xfer_external_combo.Value) : 0;
			xfer_internal_select_id = xfer_internal_combo.Value != null ? Convert.ToInt32(xfer_internal_combo.Value) : 0;
			xfer_wo_select_id       = xfer_wo_combo.Value != null ? Convert.ToInt32(xfer_wo_combo.Value) : 0;
			xfer_combined_qty       = xfer_qty_wo + xfer_qty_internal + xfer_qty_external;
			transfernote            = txtTransferReason.Text;
			if (transferwoprogid != "")
			{
				transferbvwo = new List<string>(new[] { "9999999", "9999998", "9999997" }).Contains(transferwoprogid)
												? OpsSpecialWorkOrder.StockTransfer
												: Toolbox.doSQL_string(@"SELECT woprog_BVWO FROM woprog  WHERE woprog_id =@v0", new object[] { transferwoprogid });
			}
			transferbvwo = (xfer_qty_internal > 0 || xfer_qty_external > 0) && xfer_qty_wo == 0 ? OpsSpecialWorkOrder.StockTransfer : transferbvwo;
			var is_stock_transfer = transferbvwo == OpsSpecialWorkOrder.StockTransfer;
			var wo_qty_committed = Convert.ToDouble(hidTranQTY.Value);
			var details = new NeWODetailCurrent();
			var inv = new inventory();
			details.GetLineDetails(wo_line_id);
			var testqty = details.qty_committed - xfer_combined_qty;
			if (testqty != wo_qty_committed)
			{
				wo_qty_committed = testqty;
			}
			#region PO Transfer Check
			if (details.origin.Contains("PO"))
			{
				var quantity = xfer_combined_qty + wo_qty_committed;
				if (quantity != details.qty_committed)
				{
					//Check Part History
					var pohisttable = Toolbox.doSQL_dt(@" SELECT IFNULL(woprogchanges_isqty, 0) is_qty, IFNULL(IF(woprogchanges_wasqty = '', null, woprogchanges_wasqty), 0) was_qty FROM woprogchanges WHERE woprogchanges_woprog_id = @v0  AND woprogchanges_ispartno = @v1  AND woprogchanges_comments NOT LIKE '%Transferred To Work Order:%'", new object[] { main_id, details.master_id });
					double added_qty = 0;
					double was_qty = 0;
					double is_qty = 0;
					foreach (DataRow row in pohisttable.Rows)
					{
						is_qty = Convert.ToDouble(row["is_qty"]);
						was_qty = Convert.ToDouble(row["was_qty"]);
						added_qty += is_qty - was_qty;
					}
					if (wo_qty_committed - details.qty_committed + added_qty < 0)
					{
						throw new Exception("This Item Was transferred from a PO, please make sure all the items are accounted for");
					}
				}
			}
			var xfer_master_id = Session["xfer_master_id"].ToString();
			var xfer_business_unit_id = Session["xfer_business_unit_id"].ToString();
			#endregion PO Transfer Check
			newtableid = Convert.ToInt32(wo_line_id);
			newpartnumber = details.master_id.ToString(); // because its a new entry on the new work order, make the new part and old part the same.
			oldpartnumber = details.master_id.ToString();
			var exists = inv.part_exists(newpartnumber);
			// Not sure this check will ever be hit...
			if (!exists && details.master_id < OpsSpecialPart.LaborThreshold)
			{
				update_error("You Cannot Transfer this Part it does not Exist in Inventory");
			}
			if (details.master_id < OpsSpecialPart.LaborThreshold) // if its not a labour part
			{
				inv.Load(newpartnumber, WarehouseBusinessUnit.id);
			}
			if (inv.is_qty && details.master_id < OpsSpecialPart.LaborThreshold)
			{
				var remainsendqty = wo_qty_committed == 0 ? 1 : wo_qty_committed;
				neworigsell = Math.Round(wo.use_fixed_material_markup ? details.cost * wo.fixed_material_markup : shared.GetSellPrice(details.cost, 0, inv.is_qty, remainsendqty, WorkingBusinessUnit.id32), 2); // Should these sell prices be to the second decimal place?
				transfersell = Math.Round(wo.use_fixed_material_markup ? details.cost * wo.fixed_material_markup : shared.GetSellPrice(details.cost, 0, inv.is_qty, xfer_combined_qty, WorkingBusinessUnit.id32), 2);
				oldsell = neworigsell;
			}
			else // if the item is a labour item..
			{
				neworigsell = details.sell;
				transfersell = neworigsell;
				oldsell = neworigsell;
			}
			newworkorderid = details.woprog_id.ToString();
			newdescription = details.description;
			oldorigsell = details.sell;
			olddescription = details.description;
			newReqQuantity = details.qty_ordered;
			oldqty = details.qty_ordered;
			new_qty_committed = wo_qty_committed;
			old_qty_committed = details.qty_committed;
			newbilltype = details.billtypeid.ToString();
			oldbilltype = details.billtypeid.ToString();
			oldcost = details.cost;
			oldrecno = details.rec_no;
			newrecno = details.rec_no;
			OldTrack = details.track_part;
			NewTrack = details.track_part;
			chk_workorder_transfer = true;
			try
			{
				SaveWorkOrderLine(0, details.master_id, false, false, true);
				if (xfer_combined_qty > 0)
				{
					Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_qty_ordered = if(wo_detail_current_qty_ordered - @v1  < 0, 0,wo_detail_current_qty_ordered - @v1 ) WHERE wo_detail_current_id = @v0 ", new object[] { wo_line_id, xfer_combined_qty });
				}
			}
			catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
			FillForWo(main_id);
			xfer_wo_qty.Text = "";
			hidTranQTY.Value = "";
			pop_xfer.ShowOnPageLoad = false;
		}
		catch (Exception ee)
		{
			throw;
		}
	}
	#endregion
	#region DDL filler
	private void Populate_GroupSelectDDL()
	{
		cbAddlineGroup.DataSource = Toolbox.doSQL_dt(@"SELECT id, name FROM inventory_group_hdr ORDER BY name", null);
		cbAddlineGroup.DataBind();
		cbAddlineGroup.Items.Insert(0, new ListEditItem("Select Group", "0"));
		cbAddlineGroup.SelectedIndex = 0;
	}
	private void Populate_QuoteSelectDDL()
	{
		DataTable dt;
		cbAddlineQuote.Items.Clear();
		if (wo.IsQuoted)
		{
			dt = Toolbox.doSQL_dt("SELECT CAST(concat(quote_master.quote_id,quote_master.revision)as unsigned) as ID,concat(cast(quote_master.quote_id as CHAR),' V',Cast(quote_master.revision as CHAR),' ',quote_master.job_description) as Name FROM quote_master  WHERE quote_master.quote_id =@v0 order by quote_master.revision desc", new object[] { wo.QuoteID.Remove(6) });

			foreach (DataRow dr in dt.Rows)
			{
				var li = new ListEditItem(dr[1].ToString(), dr[0].ToString());
				cbAddlineQuote.Items.Add(li);
			}
		}
		else
		{
			cbAddlineQuote.DataSource = Toolbox.doSQL_dt(@" SELECT CAST(CONCAT(quote_id,revision) AS UNSIGNED) as ID, CAST(CONCAT(quote_id,' V',IFNULL(revision,0),' ',CUSTOMER_NAME(customer_id),' - ',URLDECODE(IFNULL(job_description, ''))) AS CHAR) as Name FROM quote_master qm WHERE status_id NOT IN (9,7) AND customer_id IS NOT NULL AND business_unit_id = @v0  AND (SELECT COUNT(id) FROM quote_worksheet qw WHERE qw.quote_id = qm.quote_id AND qw.revision = qm.revision AND CONCAT('',part_no * 1) = part_no) > 0 order by quote_id,revision desc", new object[] { wo.business_unit_id });
			cbAddlineQuote.DataBind();
		}
		cbAddlineQuote.Items.Insert(0, new ListEditItem("Select Quote", "0"));
		cbAddlineQuote.SelectedIndex = 0;

	}
	private void Populate_OtherWorkOrderSelectDDL(int customerid)
	{
		cbAddlineWorkOrder.DataSource = Toolbox.doSQL_dt(@" 
SELECT
  a.WOProg_ID AS ID,
  CONCAT('(0',CAST(a.woprog_bvwo AS UNSIGNED),') ',IFNULL(a.woprog_description,'Unknown Description'),' (',IF(COUNT(b.wo_detail_current_id) = 0,COUNT(c.wo_detail_history_id),COUNT(b.wo_detail_current_id)),' items)') AS NAME
FROM
  woprog a
  LEFT JOIN wo_detail_current b
	ON a.WOProg_ID = b.wo_detail_current_woprog_id AND 
	b.wo_detail_current_master_id < 990000 AND 
	IFNULL(b.wo_detail_current_master_id,0) NOT IN (0, 2139)
  LEFT JOIN wo_detail_history c
	ON a.woprog_id = c.wo_detail_history_woprog_id AND 
	c.wo_detail_history_master_id < 990000 AND 
	IFNULL(c.wo_detail_history_master_id,0) NOT IN (0, 2139)
WHERE a.woprog_customer_id = @v0
GROUP BY a.woprog_id
HAVING 
	COUNT(b.wo_detail_current_id) > 0  OR 
	COUNT(c.wo_detail_history_id) > 0
ORDER BY woprog_bvwo DESC", new object[] { customerid });
		cbAddlineWorkOrder.DataBind();
		cbAddlineWorkOrder.Items.Insert(0,  new ListEditItem("Select Work Order", "0"));

		cbAddlineWorkOrder.SelectedIndex = 0;
	}
	#endregion DDL fille
	#region GridViewFill
	protected void FillForWo(int _id)
	{
		if (Toolbox.doSQL_int(@"select count(woprogcomment_id) from woprogcomment  where woprogcomment_woprog_id =@v0", new object[] { _id }) > 0)
		{
			btn_comment.Attributes.Add("src", "~/images/icon/icon[note].gif");
		}
		else
		{
			btn_comment.Attributes.Add("src", "~/images/icon/icon[note_blank].gif");
		}

		var dr_wo = Toolbox.doSQL_dt(@"SELECT * FROM woprog WHERE woprog_id = @v0  LIMIT 1", new object[] { main_id }).Rows[0];
		var wo_status = dr_wo["woprog_status"].ToString();
		var wo_custname = dr_wo["woprog_customername"].ToString();
		var wo_description = dr_wo["woprog_description"].ToString();
		var wo_quoteid = dr_wo["woprog_quoteid"].ToString();
		var wo_erid = dr_wo["woprog_erid"].ToString();
		var wo_pm = dr_wo["woprog_pm_memberid"].ToString();
		var wo_pmname = Toolbox.doSQL_string(@"SELECT member_fullname FROM member WHERE member_id = @v0  LIMIT 1", new object[] { wo_pm });
		lblCustNameDisp.Text = wo_custname + " - " + wo_description;
		lblMembNameDisp.Text = wo_pmname;

		btn_Summarizelabor.Text = (WorkingBusinessUnit.summarize_labor && issummarize_labor) ? "Expand Labor" : "Summarize labor";
		//throw new Exception(SqlDataSource3.SelectCommand);
		double totalTM = 0;
		double totalQuote = 0;
		var linecount = 0;
		var custlinecount = 0;
		var partnumbertest = 0;

		var wotable = Toolbox.doSQL_dt($"CALL DS_PICKLIST({_id}, {WorkingBusinessUnit.summarize_labor && issummarize_labor})", null);

		bool isAllowedCost, isAllowedLaborCost = false;
		isAllowedCost = current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewCostInformationOnWorkOrders);
		isAllowedLaborCost = current_user.AuthenticatedForPrivilege(OpsPrivilege.ViewLabourCostsOnWorkOrders);
		bool IsLabourVisible = false;
		foreach (DataRow row in wotable.Rows)
		{
			if(row[GVColumns.LineType].ToString()==OpsWOLineType.Labor)
			{
				IsLabourVisible = true;
			}
			int.TryParse(row[GVColumns.MasterId].ToString(), out partnumbertest);
			linecount++;
			if (isAllowedCost)
			{
				if (!isAllowedLaborCost && partnumbertest >= OpsSpecialPart.LaborThreshold)
				{
					row[GVColumns.Cost] = DBNull.Value;
				}
			}
			else
			{
				row[GVColumns.Cost] = DBNull.Value;
			}
			if (!member_can_see_sell)
			{
				row[GVColumns.Sell] = 0;
				row[GVColumns.ExtTandM] = 0;
			}
			double temp_extTM = 0;
			double.TryParse(row[GVColumns.ExtTandM].ToString(), out temp_extTM);
			totalTM += temp_extTM;
			totalQuote += temp_extTM;
			row.AcceptChanges();
		}
		btn_Summarizelabor.Visible = IsLabourVisible && member_can_see_labour_cost && WorkingBusinessUnit.summarize_labor;
		pop_billtype_edit.Enabled = member_can_see_sell && member_can_edit_sell;
		ASPxpuHistory.Enabled = false;

		lblTotalItemsDisp.Text = linecount.ToString();
		lblTotalCustomDisp.Text = custlinecount.ToString();
		lblQuotedDisp.Text = totalQuote.ToString("C2");

		UpdateWorkOrderTotals(_id, wo_quoteid, wo_erid);

		var location_column = (GridViewDataColumn)agv.Columns[GVColumns.Location];
		location_column.Settings.AllowAutoFilter = DefaultBoolean.False;
		Session["flag_html_prepared"] = null;


		agv.DataSource = wotable;
		dt_master_locations = null;
		agv.DataBind();


		Title = "WO Parts List for " + dr_wo["woprog_bvwo"];
		footer_save_toggle(false);
		existingItems.InnerHtml = $@"Existing Rows -	<a href='#' onclick=""applyFilter(0);"">All {wotable.Rows.Count} Rows</a>, 
														<a href='#' onclick=""applyFilter('Q');"">{wotable.Select("linetype = 'Q'").Length} Quote</a>, 
														<a href='#' onclick=""applyFilter('L');"">{wotable.Select("linetype = 'L'").Length} Labor</a>, 
														<a href='#' onclick=""applyFilter('M');"">{wotable.Select("linetype = 'M'").Length} Material</a>, 
														<a href='#' onclick=""applyFilter('K');"">{wotable.Select("linetype = 'K'").Length} Mileage</a>, 
														<a href='#' onclick=""applyFilter('A');"">{wotable.Select("linetype = 'A'").Length} Asset</a>";

	}
	#endregion
	#region GridviewRow Updating And Deleting
	protected void agv_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
	{
		double oldext = 0;
		double newext = 0;
		var editingkey = e.Keys[0];
		newtableid = Convert.ToInt32(editingkey.ToString());
		var this_inv = new inventory();
		var currentLine = new NeWODetailCurrent(newtableid);
		if (e.NewValues.Contains(GVColumns.RecNo) && e.NewValues[GVColumns.RecNo] != null)
		{
			newrecno = Convert.ToInt32(e.NewValues[GVColumns.RecNo].ToString());
			oldrecno = Convert.ToInt32(e.OldValues[GVColumns.RecNo].ToString());
		}
		if (e.NewValues.Contains(GVColumns.MasterId) && e.NewValues[GVColumns.MasterId] != null)
		{
			newpartnumber = e.NewValues[GVColumns.MasterId].ToString();
			oldpartnumber = e.OldValues[GVColumns.MasterId].ToString();
		}
		var this_part_no = Convert.ToInt32(newpartnumber);
		if (this_part_no > 0)
		{
			if (this_part_no < OpsSpecialPart.LaborThreshold)
			{
				this_inv.Load(this_part_no, WarehouseBusinessUnit.id);
			}
		}


		if (e.NewValues.Contains(GVColumns.Location) && e.NewValues[GVColumns.Location] != null)
		{
			newworkorderid = e.NewValues[GVColumns.Location].ToString();
			oldworkorderid = e.OldValues[GVColumns.Location].ToString();
		}
		newdiscount = Convert.ToDouble(e.NewValues[GVColumns.Discount]);
		olddiscount = Convert.ToDouble(e.OldValues[GVColumns.Discount]);

		if (e.NewValues.Contains(GVColumns.Cost) && e.NewValues[GVColumns.Cost] != null)
		{
			double.TryParse(e.NewValues[GVColumns.Cost].ToString(), out newcost);
			double.TryParse(e.OldValues[GVColumns.Cost].ToString(), out oldcost);
			newcost = can_edit_part_cost ? Convert.ToDouble(e.NewValues[GVColumns.Cost]) : Toolbox.doSQL_double(@"Select wo_detail_current_price_cost from wo_detail_current  where wo_detail_current_id =@v0", new object[] { newtableid });
		}
		else if (currentLine.type == OpsWOLineType.Labor)
		{
			newcost = currentLine.cost;
		}

		if (e.NewValues.Contains(GVColumns.Description) && e.NewValues[GVColumns.Description] != null)
		{
			newdescription = e.NewValues[GVColumns.Description].ToString();
			olddescription = e.OldValues[GVColumns.Description].ToString();
			if (oldpartnumber == OpsSpecialPart.MakeThisPart.ToString() && newpartnumber != OpsSpecialPart.MakeThisPart.ToString())
			{
				newdescription = this_inv.description;
			}
		}

		if (e.NewValues.Contains(GVColumns.QtyRequired) && e.NewValues[GVColumns.QtyRequired] != null && currentLine.type != OpsWOLineType.Labor)
		{
			newReqQuantity = Convert.ToDouble(e.NewValues[GVColumns.QtyRequired]);
			oldqty = Convert.ToDouble(e.OldValues[GVColumns.QtyRequired]);
			newReqQuantity = newReqQuantity - oldqty;
			oldqty = 0;
		}
		else if (currentLine.type == OpsWOLineType.Labor)
		{
			oldqty = 0;
			newReqQuantity = 0;
		}

		if (e.NewValues.Contains(GVColumns.QtyReceiving) && e.NewValues[GVColumns.QtyReceiving] != null && currentLine.type != OpsWOLineType.Labor)
		{
			new_qty_committed = Convert.ToDouble(e.NewValues[GVColumns.QtyReceiving].ToString());
			old_qty_committed = Convert.ToDouble(e.OldValues[GVColumns.QtyReceiving].ToString());
			if (new_qty_committed != old_qty_committed)
			{
				this_inv.Load(newpartnumber, WarehouseBusinessUnit.id);
			}
		}
		else if (currentLine.type == OpsWOLineType.Labor)
		{
			old_qty_committed = 0;
			new_qty_committed = 0;
		}

		if (e.NewValues.Contains(GVColumns.DateRequired) && Convert.ToString(e.OldValues[GVColumns.DateRequired]) != "")
		{
			newdatereq = Toolbox.MySQL_shortdt(Convert.ToDateTime(e.NewValues[GVColumns.DateRequired]));
			olddatereq = Toolbox.MySQL_shortdt(Convert.ToDateTime(e.OldValues[GVColumns.DateRequired]));
		}


		if (e.OldValues.Contains(GVColumns.Sell) && e.OldValues[GVColumns.Sell] != null)
		{
			oldsell = Convert.ToDouble(e.OldValues[GVColumns.Sell].ToString());
			oldorigsell = oldsell;
		}

		if (e.NewValues.Contains(GVColumns.Sell) && member_can_edit_sell)
		{
			var newSell = e.NewValues[GVColumns.Sell];
			if (newSell == null)
			{
				throw new Exception("Please provide a valid sell price.");
			}
			double.TryParse(newSell.ToString(), out neworigsell);
			if (neworigsell <= 0)
			{
				throw new Exception("Please provide a sell price greater than zero");
			}
		}
		else
		{
			newsell = oldsell;
		}

		if (e.NewValues.Contains(GVColumns.BillType) && e.NewValues[GVColumns.BillType] != null)
		{
			newbilltype = e.NewValues[GVColumns.BillType].ToString();
			oldbilltype = e.OldValues[GVColumns.BillType].ToString();
		}
		else
		{
			newbilltype = "0";
		}

		if (e.NewValues.Contains(GVColumns.TrackPart) && e.NewValues[GVColumns.TrackPart] != null)
			{
			NewTrack = Convert.ToInt32(e.NewValues[GVColumns.TrackPart].ToString());
			OldTrack = Convert.ToInt32(e.OldValues[GVColumns.TrackPart].ToString());
			}
		if (e.NewValues.Contains(GVColumns.ActivityCode) && e.NewValues[GVColumns.ActivityCode] != null)
			{
			ActivityCode = e.NewValues[GVColumns.ActivityCode].ToString();
			}
		if (e.NewValues.Contains(GVColumns.CostElement) && e.NewValues[GVColumns.CostElement] != null)
			{
			CostElement = e.NewValues[GVColumns.CostElement].ToString();
			}
		if (e.NewValues.Contains(GVColumns.ClientWO) && e.NewValues[GVColumns.ClientWO] != null)
			{
			ClientWO = e.NewValues[GVColumns.ClientWO].ToString();
			}
		if (e.NewValues.Contains(GVColumns.ClientPO) && e.NewValues[GVColumns.ClientPO] != null)
			{
			ClientPO = e.NewValues[GVColumns.ClientPO].ToString();
			}
		SaveWorkOrderLine(0, this_part_no, false, false, false);
		FillForWo(main_id);
		ScriptManager.RegisterStartupScript(this, GetType(), "binder", "bind_tooltips();", true);
		agv.JSProperties["cplineid"] = "";
		e.Cancel = true;
		agv.CancelEdit();
	}
	protected void delete_workorder_line(int line_id, object part_no)
	{
		if (hidWODelete.Value == "false")
		{
			throw new Exception("You Cannot Delete Items From This Work Order.");
		}
		NeWODetailCurrent.delete_workorder_line(line_id, part_no, current_user);
	}
	protected void agv_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
	{
		var grid = (ASPxGridView)sender;
		var editingkey = e.Keys[0];
		var Deleteparttableid = editingkey.ToString();
		#region workorder
		delete_workorder_line(Convert.ToInt32(Deleteparttableid), e.Values[GVColumns.MasterId]);
		FillForWo(main_id);
		#endregion workorder
		e.Cancel = true;
		agv.CancelEdit();
	}
	protected void agv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		var action = e.Parameters.Contains("|")
						? e.Parameters.Split('|')[0]
						: e.Parameters;
		var lineIndex	= e.Parameters.Contains("|")
						? Convert.ToInt32(e.Parameters.Split('|')[1])
						: 0;
		if (action == "delete_multiple")
		{
			var start = agv.VisibleStartIndex;
			var end = agv.VisibleRowCount;
			for (var i = start; i < end; i++)
			{
				var keyValue = agv.GetRowValues(i, agv.KeyFieldName) == null ? "" : agv.GetRowValues(i, agv.KeyFieldName).ToString();
				var key = 0;
				int.TryParse(keyValue, out key);
				if (key > 0)
				{
					var column_chkbox = (GridViewDataColumn)agv.Columns[GVColumns.Select];
					var chkbox = (CheckBox)agv.FindRowCellTemplateControl(i, column_chkbox, "chk_indiv");
					if (chkbox != null && chkbox.Checked)
					{
						var part_no = agv.GetRowValues(i, GVColumns.MasterId).ToString();
						var qty_committed = Convert.ToDouble(agv.GetRowValues(i, GVColumns.QtyCommittedToDate));
						if (qty_committed == 0)
						{
							try
							{
								delete_workorder_line(Convert.ToInt32(keyValue), part_no);
							}
							catch (Exception ee)
							{
								throw new Exception(ee.Message);
							}
						}
					}
				}
			}
		}
	else if (action == "partially_billed")
		{
		var keyValue = agv.GetRowValues(lineIndex, agv.KeyFieldName) == null 
							? 0 
							: Convert.ToInt32(agv.GetRowValues(lineIndex, agv.KeyFieldName));
		NeWODetailCurrent.TogglePartiallyBilled(keyValue);
		}
	FillForWo(main_id);
	}
	#endregion
	#region Adding And Updating Lines
	private void SaveWorkOrderLine(int xfer_to_woprog_id, int master_id, bool adding, bool exclude, bool commit_wo)
	{
		using (var conn = Toolbox.connect())
		{
			var billtype = 0;
			var recnumber = 0;
			var RecordNumTwo = 0;
			var laboourcodetest = 0;
			var quote = "";
			var strwoid = "";
			double oldvalue = 0;
			double currently_on_WO = 0;
			var is_exclude = false;
			var is_invoiced = false;
			var inv = new inventory();
			var recordnumbertosave = 0;
			var sales = new NeSalesOrder();
			var comp = new NeBusinessUnit(wo.business_unit_id);
			var xfer_to_wo = new NeWOProg();
			if (xfer_to_woprog_id > 0 && !Toolbox.Contains(xfer_to_woprog_id, new[] { 9999999, 9999998, 9999997 }))
			{
				xfer_to_wo = new NeWOProg(xfer_to_woprog_id);
			}
			if (int.TryParse(newpartnumber, out master_id) && master_id < OpsSpecialPart.LaborThreshold && newpartnumber != OpsSpecialPart.QuoteLine.ToString())  // only check for an existing part if the part is not labour
			{
				currently_on_WO = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(wo_detail_current_qty_committed), 0) FROM wo_detail_current WHERE wo_detail_current_master_id = @v0  AND wo_detail_current_woprog_id = @v1 ", new object[] { master_id, main_id });
			}
			if(adding)
				{
				newdescription = string.IsNullOrEmpty(TextDescription.Text) ? null : TextDescription.Text;
				}
			#region Work Orders in Invoiced, Waiting to Be Invoiced, or Waiting for PO
			if (Toolbox.Contains(wo.Status, new[] { OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced, OpsWOStatus.WaitingForPO }))
			{
				xfer_to_wo = new NeWOProg(main_id);

				/*
				 *b.	Use part # 55560, but make sure only employees in a business unit marked as is_backoffice = 1 can add these to a WO.
				 */
		   
				if ( newpartnumber != OpsSpecialPart.MiscMaterial.ToString() || !IsBackOffice )
				{
					throw new Exception("You can not add or alter an item on this work order because it has been invoiced/is waiting to be invoiced.");
				}

				// check to see if wo made it to the history table or not
				if (newdescription.Length > 80)
				{
					newdescription = newdescription.Substring(0, 79);
				}
				var max_rec = 0;
				max_rec = Toolbox.doSQL_int(@"Select ifnull((Select max(wo_detail_history_rec_no) from wo_detail_history  where wo_detail_history_woprog_id=@v0),0) ", new object[] { wo.woprog_id });

				if (wo.Status == OpsWOStatus.Invoiced)
				{
					is_invoiced = true;
				}
				else
				{
					is_invoiced = false;
				}

				#region detail rows are actually in the history tables in BV and nesi

				if (adding)
				{
					var h = new NeWODetailHistory
					{
						price_cost = newcost,
						price_sell = neworigsell, // ber able to add a negative sell based on story update.
						price_unit = 0,
						qty_committed = newReqQuantity,
						qty_ordered = newReqQuantity,
						added_by = current_user.id32,
						bvwo = Convert.ToInt32(wo.OrderNumber),
						code = OpsSpecialPart.MiscMaterial.ToString(),
						business_unit_id = wo.business_unit_id,
						consignment_id = 0,
						date_added = DateTime.Today,
						description = newdescription,
						discount = 0,
						master_id = OpsSpecialPart.MiscMaterial,
						origin = "After_Close",
						rec_no = max_rec + 1,
						track_part = false,
						type = OpsWOLineType.Material,
						woprog_id = wo.woprog_id
					};

					h.save();
				}
				else
				{
					var h = new NeWODetailHistory(newtableid);
					h.price_cost = newcost;
					h.save();
				}

				#endregion



				NeWOProg.update_header_totals(wo.woprog_id.ToString(), wo.business_unit_id, wo.OrderNumber);
				FillForWo(main_id);
				return;

			}
			#endregion Work Orders in Invoiced, Waiting to Be Invoiced, or Waiting for PO
			if (inv.part_exists(master_id))
			{
				if (inv_i != null)
				{
					inv = inv_i[master_id.ToString()];
				}
				else
				{
					inv.Load(master_id, WarehouseBusinessUnit.id);
				}
				var tag = new inventory.tag(inv.tag_id);
				is_exclude = tag.is_exclude;
			}

			var consignment_id = 0;
			var original = "";
			var company = new NeBusinessUnit();
			var orig_detail = new NeWODetailCurrent();
			var xfer_to_detail = new NeWODetailCurrent();
			originbvwo = wo.OrderNumber;

			originbvwo = wo.OrderNumber;
			if (!adding)
			{
				orig_detail = new NeWODetailCurrent(newtableid);
				original = orig_detail.origin;
				consignment_id = orig_detail.consignment_id;
				if (orig_detail.origin.Contains("PO"))  // if the original line came from a po
				{
					if (new_qty_committed != 0 && orig_detail.qty_committed > new_qty_committed && !chk_workorder_transfer)
					{
						throw new Exception("You cannot reduce the committed quantity of Parts Transferred From A PO... Use the transfer button instead.");
					}
				}
			}
			var xfer_to_wodc_id = 0;
			if (xfer_to_woprog_id > 0 && !Toolbox.Contains(xfer_to_woprog_id, new[] { 9999999, 9999998, 9999997 }))
			{
				// Definitely a transfer
				xfer_to_wo = new NeWOProg(xfer_to_woprog_id);
				// We need to check if the part already exists on the TO work order.
				var c_exists_xfer_to = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { xfer_to_woprog_id, master_id });
				if (!inv.is_exclude && c_exists_xfer_to == 1)
				{
					xfer_to_wodc_id = Toolbox.doSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { xfer_to_woprog_id, master_id });
					xfer_to_detail = new NeWODetailCurrent(xfer_to_wodc_id);
				}
			}
			var DSN = company.GetBUDSN(WorkingBusinessUnit.id);
			quote = wo.QuoteID;
			strwoid = id;
			billtype = agv.IsEditing || newtableid > 0 // As this is touched by both the addline, committing and the edit functionality, need to check this out.
					? oldbilltype == newbilltype // If this isn't changing:
						? Convert.ToInt32(oldbilltype) // Leave it
						: Convert.ToInt32(newbilltype) // Or change it
					: wo.IsQuoted // This means it's not a new line, so it checks for the default billtype
						? OpsBillType.JobcostForQuote // If it's quoted, then use JobCostForQuote
						: OpsBillType.Regular; // Otherwise, the default is regular.
			if (wo.IsProgressBill && !Toolbox.Contains(billtype, new[] { OpsBillType.DoNotInclude, OpsBillType.ProgressBillingOld, OpsBillType.ProgressBilling }))
			{
				if (wo.IsCredit || wo.IsRebill)
				{
					throw new Exception("Only progress bill & credit (visible/invisible) billtypes can be used on a credit/rebill progress billing");
				}
				throw new Exception("Only progress bill billtypes can be used on a progress billing");
			}
			sales.BillingTypeID = billtype;
			int.TryParse(newpartnumber, out laboourcodetest);
			var CommittedChanged = new_qty_committed != 0 || xfer_to_woprog_id > 0 || qtytoadd != 0;
			var RequiredChanged = newReqQuantity != 0 || xfer_to_woprog_id > 0;
			double comm_qty_diff = 0;
			var comm_qty_total =  CommittedChanged ? new_qty_committed - orig_detail.qty_committed : 0;
			var partid = newpartnumber;
			if (!is_exclude && master_id < OpsSpecialPart.LaborThreshold && !agv.IsEditing)
			{
				var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { main_id, inv.master_id });
				if (c == 1)
				{
					newtableid = Toolbox.doSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { main_id, inv.master_id });
					orig_detail.GetLineDetails(newtableid.ToString());
					oldqty = orig_detail.qty_ordered;
				}
			}
			if (!adding)
			{
				comm_qty_diff = CommittedChanged ? new_qty_committed - orig_detail.qty_committed : 0;
			}
			else
			{
				comm_qty_diff = 0; // newqty;
				double.TryParse(TextComQty.Text, out comm_qty_diff);
				comm_qty_total = comm_qty_diff;

				if (ddlMatType.SelectedValue == "2" && !adding) // Labor -- Matt, check for ddlmattype = 2

				{
					comm_qty_diff = CommittedChanged ? new_qty_committed - orig_detail.qty_committed : 0;
					comm_qty_total = comm_qty_diff;
				}
			}
			if (inv.master_id == OpsSpecialPart.QuoteLine.ToString() && sales.BillingTypeID == OpsBillType.ProgressBillingOld && new_qty_committed > orig_detail.qty_committed)
			{
				var q_id = 0;
				var q_rev = 0;
				nesi.core.quote.splice(wo.QuoteID, out q_id, out q_rev);
				var quoted_amount = nesi.core.quote.quoted_amount(conn, q_id);
				var total_billed = NeWOProg.total_billed(conn, main_id);
				if (neworigsell > 0)
				{
					if ((total_billed >= quoted_amount || quoted_amount - total_billed + new_qty_committed * neworigsell > quoted_amount) && new_qty_committed > 0)
					{
						throw new Exception("Unable to bill more than the quoted amount");
					}
				}
				else
				{
					if ((total_billed >= quoted_amount || quoted_amount - total_billed + new_qty_committed * quoted_amount > quoted_amount) && new_qty_committed > 0)
					{
						throw new Exception("Unable to bill more than the quoted amount");
					}
				}
			}
			var desc = newdescription;


			recordnumbertosave = newrecno;
			if ((sales.BillingTypeID == OpsBillType.InvisibleCredit || sales.BillingTypeID == OpsBillType.VisibleCredit) && new_qty_committed > 0)
			{
				throw new Exception("A credit cannot have a positive quantity.");
			}
			if (oldrecno != newrecno && !adding)  // if you're not adding a line.
			{
				var linecount = Toolbox.doSQL_int(@"SELECT COUNT(*) + 2 FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 ", new object[] { main_id });  // find the next line on the work order
				if (newrecno > 1 && newrecno < linecount)
				{
					//Update Record Numbers
					//NeWODetailCurrent details = new NeWODetailCurrent();
					//details.SwitchRecNo(oldrecno, newrecno, Convert.ToInt32(main_id), newtableid);
					recordnumbertosave = newrecno;
				}
			}

			sales.PartNo = master_id.ToString();
			if (!adding)
			{
				sales.RecNum = recordnumbertosave.ToString();
			}
			var twoweeksfromtoday = Toolbox.MySQL_shortdt(DateTime.Now.AddDays(14));
			//		string used_date_required = progress.woprog_Expected_StartDate > DateTime.Now.AddDays(14) ? Toolbox.MySQL_shortdt(progress.woprog_Expected_StartDate) : twoweeksfromtoday;

			var used_date_required = Convert.ToDateTime(Session["picklist_date"]).ToString("yyyy-MM-dd");
			sales.DateRequired = newdatereq != "" && newdatereq != orig_detail.date_required ? newdatereq : orig_detail.date_required == "" ? used_date_required : orig_detail.date_required;
			sales.Quantity = comm_qty_diff;
			var dont_force_comm_pop = false;
			sales.Notes = hidNotes.Value;
			sales.ActualQuantity = adding ? comm_qty_total : new_qty_committed;
			#region Costing
			var parts_cost_is_editable = Toolbox.Contains(master_id, new[] { OpsSpecialPart.QuoteLine, OpsSpecialPart.SubContractor }) || master_id >= OpsSpecialPart.LaborThreshold && master_id < 2000000;
			var use_po_cost = false;
			var price = neworigsell.ToString();
			var current_cost = !inv.is_exclude && inv.allowed_to_stock ? Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, 1)", new object[] { partid, WarehouseBusinessUnit.id }) : 0;
			var chk_uncommit_stocked = inv.cost_price_branch > 0 && orig_detail.qty_committed > new_qty_committed && inv.allowed_to_stock;
			var chk_2139_costchange = oldcost != newcost && can_edit_part_cost && master_id == OpsSpecialPart.QuoteLine;
			var chk_costchange = oldcost != newcost && can_edit_part_cost;
			var chk_part_is_labor = master_id >= OpsSpecialPart.LaborThreshold;
			var chk_part_isnt_labor = master_id < OpsSpecialPart.LaborThreshold;
			var chk_rentals = !inv.allowed_to_stock && !inv.is_exclude && chk_part_isnt_labor;
			var chk_real_material_part = inv.allowed_to_stock && !is_exclude;
			var chk_cost_different_from_inv = oldcost != current_cost;

		var bd = new bingo_data
					{
					before_cost = orig_detail.cost,
					before_qty  = orig_detail.qty_committed,
					before_sell = orig_detail.sell,
					detail_id   = orig_detail.id,
					master_id   = master_id,
					woprog_id   = orig_detail.woprog_id
					};
		if (chk_workorder_transfer)
			{
				if (chk_part_is_labor)
				{
					throw new Exception("You can't transfer labor lines between work orders");
				}
				if (chk_part_isnt_labor && (chk_real_material_part || chk_rentals))
				{
					sales.CostOverRide = Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { strwoid, master_id, sales.ActualQuantity, orig_detail.cost });
				}
				else if (inv.is_exclude) // New lines, don't blend, don't refactor sell price
				{
					sales.CostOverRide = orig_detail.cost;
				}
			}
			// Not a transfer, it a non-labor part #, not from PO, isn't an exclude, is a part we can stock, quantity is changing, old cost doesn't equal new cost
			else if (chk_part_isnt_labor && !use_po_cost && chk_real_material_part && Math.Abs(comm_qty_total) > 0 && chk_cost_different_from_inv)
			{
				var temp_qty = sales.ActualQuantity == 0 ? 1 : sales.ActualQuantity;
				bd.ca_cost = current_cost;
				bd.ca_qty = sales.ActualQuantity;
				sales.CostOverRide = comm_qty_total < 0 ? orig_detail.cost : Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { strwoid, master_id, comm_qty_total, current_cost });
				bd.after_qty = orig_detail.qty_committed + sales.ActualQuantity;
			}
			// Returning item to stock - Uncommitting
			else if (chk_uncommit_stocked)
			{
				sales.CostOverRide = oldcost; // Keep cost, will need to refactor the sell price.
				bd.ca_cost = oldcost;
			}
			// Manually changing cost... only for 2139 lines
			else if (chk_2139_costchange)
			{
				sales.CostOverRide = newcost;
				bd.ca_cost = newcost;
			}
			// Rentals
			else if (chk_rentals)
			{
				var temp_qty = sales.ActualQuantity == 0 ? 1 : sales.ActualQuantity;
				sales.CostOverRide = Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )", new object[] { master_id, WarehouseBusinessUnit.id, temp_qty });
				bd.ca_cost = sales.CostOverRide; // There isn't a CA here.
			}
			else if (chk_part_is_labor && can_edit_part_cost)
			{
				if (chk_costchange)
				{
					sales.CostOverRide = newcost;
					bd.ca_cost = newcost; // There isn't a CA here.
				}
				else
				{
					sales.CostOverRide = oldcost;
					bd.ca_cost = oldcost; // There isn't a CA here.
				}
			}
			else if (newtableid > 0)
			{
				sales.CostOverRide = orig_detail.cost;
				bd.ca_cost = orig_detail.cost; // There isn't a CA here.
			}
			else if (ddlMatType.SelectedValue == "5" && inv.is_exclude)
			{
				sales.CostOverRide = newcost;
				sales.ManualChange = true;
			}
			#endregion Costing





			if (!member_can_see_cost && chk_real_material_part)
			{
				if (sales.CostOverRide > 0)
				{
					// Set because there is a cost override set
					neworigsell = wo.use_fixed_material_markup 
									? sales.CostOverRide * wo.fixed_material_markup 
									: shared.GetSellPrice(sales.CostOverRide, 0, inv.is_qty, sales.ActualQuantity, WorkingBusinessUnit.id);  // recalculate sell based on the cost.. because we can't trust whatever is on the front page
					bd.ca_sell = neworigsell;
					bd.ca_qty = sales.ActualQuantity; // There isn't a CA here.
				}
				else if (neworigsell != 0 && neworigsell != oldorigsell)
				{
					sales.ManualChange = agv.IsEditing;
					sales.SellOverride = neworigsell;
					bd.ca_sell = neworigsell;
					bd.ca_qty = orig_detail.qty_committed; // There isn't a CA here.
				}
				else// Use existing line cost
				{
					neworigsell = wo.use_fixed_material_markup 
									? orig_detail.cost * wo.fixed_material_markup 
									: shared.GetSellPrice(orig_detail.cost, 0, inv.is_qty, sales.ActualQuantity, WorkingBusinessUnit.id);  // recalculate sell based on the cost.. because we can't trust whatever is on the front page
					bd.ca_sell = neworigsell;
					bd.ca_qty = sales.ActualQuantity; // There isn't a CA here.
				}
			}
			if (neworigsell != oldorigsell && agv.IsEditing)
			{
				sales.ManualChange = true;
				newsell = neworigsell;
			}
			if (neworigsell == oldorigsell && agv.IsEditing)
			{
				// no change on sell price, users just click the save button.
				// we will not have the code of sales.ManualChange = true, becasue no change on sell price.
				newsell = neworigsell;
			}

			else if (newReqQuantity != oldqty && adding && orig_detail.id > 0 && !is_exclude)
			{
				sales.ManualChange = true;
			}
			//		sales.RetailCost = 
			// Check if it exists already on WO

			var exists_chk = Toolbox.doSQL_int(@"SELECT CAST(IFNULL(SUM(wo_detail_current_qty_committed),0) AS UNSIGNED) FROM wo_detail_current WHERE wo_detail_current_master_id = @v0  AND wo_detail_current_woprog_id = @v1 ", new object[] { partid, strwoid });
			if (exists_chk != 0 && sales.ActualQuantity < 0 && !inv.is_exclude && sales.BillingTypeID != OpsBillType.DoNotInclude)
			{
				throw new Exception("You cannot add multiple credit lines for the same part. Find the part in the list and modify the Com Qty there.");
			}


			sales.SellOverride = orig_detail.sell;
			if (master_id != OpsSpecialPart.QuoteLine)
			{
				if (adding && TextSell.Text != "" && Convert.ToDouble(TextSell.Text) != Convert.ToDouble(price) && (exists_chk == 0 || is_exclude) && Convert.ToDouble(TextSell.Text) > 0)
				{
					sales.SellOverride = Convert.ToDouble(TextSell.Text);
				}
				else if (neworigsell > 0 && exists_chk == 0) // neworigsell is greater than zero and nothing has been committed yet.
				{
					sales.SellOverride = neworigsell;
				}
				else if (oldsell != newsell && exists_chk == 0)
				{
					sales.SellOverride = newsell;
				}
				else if (chk_workorder_transfer)
				{
					sales.SellOverride = 0;// This will force the sell price to be refactored in the class.
					sales.ManualChange = false;
				}
				  else if (master_id == OpsSpecialPart.MiscMaterial)// Pure revenue lines to have sell overridden
				{
					sales.SellOverride = neworigsell;
				}
				else if (chk_rentals)
				{
					sales.SellOverride = sales.ManualChange
											? neworigsell
											: shared.GetSellPrice(sales.CostOverRide, 0, inv.is_qty, sales.ActualQuantity, WorkingBusinessUnit.id32);
				}
				else if (Convert.ToDouble(price) > 0 && !adding && !chk_workorder_transfer && agv.IsEditing)
				{
					sales.SellOverride = Convert.ToDouble(price);
					sales.ManualChange = true;
				}
				else if (adding && exists_chk > 0 && !is_exclude && master_id >= OpsSpecialPart.LaborThreshold)
				{
					sales.SellOverride = neworigsell;
				}
				else if (!adding && exists_chk > 0 && !is_exclude && master_id >= OpsSpecialPart.LaborThreshold && sales.SellOverride == 0)
				{
					sales.SellOverride = neworigsell;
				}
			}
			else if (adding)
			{
				sales.SellOverride = Convert.ToDouble(TextSell.Text);
			}
			else if (oldsell != newsell)
			{
				sales.SellOverride = newsell;
			}
			else
			{
				// no change on sell price, users just click the save button.
				sales.SellOverride = oldsell;
			}

			if (sales.SellOverride == 0 && master_id >= OpsSpecialPart.LaborThreshold)
			{
				throw new Exception("You cannot set a sell price for a labor line as zero. You must use the appropriate billtype instead.");
			}
			if (sales.SellOverride != 0 && sales.SellOverride < sales.CostOverRide && can_edit_part_cost && !parts_cost_is_editable && master_id != OpsSpecialPart.MiscMaterial)
			{
				throw new Exception("Cannot change the sell price to be less than the cost price.");
			}
			// if (adding && myMember.business_unit_id == 11)
			//     sales.ActualQuantity = newqty;
			bd.after_qty = sales.ActualQuantity;
			bd.after_cost = sales.CostOverRide;
			bd.after_sell = sales.SellOverride;
			bd.save();
			sales.partlineid = newtableid.ToString();
			sales.working_line_id = newtableid;
			sales.Desc = desc;
			sales.TrackPart = NewTrack;
			sales.ActivityCode = ActivityCode;
			sales.CostElement = CostElement;
			sales.ClientWO = ClientWO;
			sales.ClientPO = ClientPO;
			sales.CustomerDiscount = newdiscount;

			sales.OrderedQuantity = RequiredChanged ? (chk_workorder_transfer && transferbvwo != OpsSpecialWorkOrder.StockTransfer || sales.Quantity < 0 
										? comm_qty_diff 
										: newReqQuantity)
										: 0;
			if (sales.OrderedQuantity != 0 && newReqQuantity != oldqty) // Adding more required
			{
				sales.ManualChange = true;
			}
			if (!new List<int>(new[] { OpsBillType.Regular, OpsBillType.JobcostForQuote, OpsBillType.InvisibleCredit, OpsBillType.VisibleCredit}).Contains(sales.BillingTypeID))
			{
				sales.CustomerDiscount = 0;
				if (!adding)
				{
					try
					{
						Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_discount = 0 WHERE wo_detail_current_id = @v0  LIMIT 1", new object[] { newtableid.ToString() });
					}
					catch (Exception ee)
					{
						Toolbox.do_errorLog_errorStack(ee);
					}
				}
			}


			var checkbilltype = wo.IsQuoted ? 1 : 0;

			#region Check Quote Information

			//Check For Quote Information
			//How Many Job Cost for Quotes on This Work Order
			var JobCostCount = Toolbox.doSQL_int(@"SELECT COUNT(id) FROM wo_detail WHERE billtypeid = @v1 AND woprog_id = @v0", new object[] { main_id, OpsBillType.JobcostForQuote });
			var QuoteLineCount = Toolbox.doSQL_int(@"SELECT COUNT(id) FROM wo_detail WHERE master_id = @v1 AND billtypeid = @v2  AND woprog_id = @v0 ", new object[] { main_id, OpsSpecialPart.QuoteLine, OpsBillType.QuotedPrice });
			if (inv.allowed_to_stock)
			{
				var il_m = new location_master();
				if (xfer_qty_internal != 0 || xfer_qty_external != 0) // Direct xfer
				{
					try
					{
						if (xfer_qty_external > 0)
						{//xfer_external_select_id
							il_m = new location_master(xfer_external_select_id);
							sales.uncommitting_to_location = il_m.name;
						}
						if (xfer_qty_internal > 0)
						{//xfer_internal_select_id
							il_m = new location_master(xfer_internal_select_id);
							sales.uncommitting_to_location = string.IsNullOrEmpty(sales.uncommitting_to_location) ? il_m.name : "," + il_m.name;
						}
					}
					catch
					{
						if (xfer_qty_external > 0)
						{//xfer_external_select_id
							il_m = new location_master(new location(xfer_external_select_id).location_master_id);
							sales.uncommitting_to_location = il_m.name;
						}
						if (xfer_qty_internal > 0)
						{//xfer_internal_select_id
							il_m = new location_master(new location(xfer_internal_select_id).location_master_id);
							sales.uncommitting_to_location = string.IsNullOrEmpty(sales.uncommitting_to_location) ? il_m.name : "," + il_m.name;
						}
					}
				}
				else if (!chk_workorder_transfer && (new_qty_committed != 0 || comm_qty_diff != 0) && new_qty_committed != orig_detail.qty_committed) // Committing & Uncommitting
				{//xfer_comm_recv_select_id
					try
					{
						if (new_qty_committed > 0)
						{
							il_m = new location_master(xfer_comm_recv_select_id);
							sales.committing_from_location = il_m.name;
						}
						else if (new_qty_committed < 0)
						{
							il_m = new location_master(xfer_comm_recv_select_id);
							sales.uncommitting_to_location = il_m.name;
						}

					}
					catch
					{
						if (new_qty_committed > 0)
						{
							il_m = new location_master(new location(xfer_comm_recv_select_id).location_master_id);
							sales.committing_from_location = il_m.name;
						}
						else if (new_qty_committed < 0)
						{
							il_m = new location_master(new location(xfer_comm_recv_select_id).location_master_id);
							sales.uncommitting_to_location = il_m.name;
						}
					}
				}
			}
			if (newbilltype == "1" && QuoteLineCount == 0)
			{
				throw new Exception("You can not set an item for job cost for quote if there is not a quote line on a work order.");
			}
			if (newpartnumber == OpsSpecialPart.QuoteLine.ToString() && oldbilltype != newbilltype && newbilltype != "3" && JobCostCount != 0)
			{
				throw new Exception("You can not change a quote line`s bill type if there are job costs for quote bill types on the work order.");
			}

			#endregion Check Quote Information

			if (checkbilltype == OpsBillType.Blended)
			{
				if (!current_user.AuthenticatedForPrivilege(OpsPrivilege.SelectBlendedOrInventoryAsABillingType))
				{
					throw new Exception("You Are Not Allowed to Set an Item to Blended or Inventory");
				}
			}
			var credit = checkbilltype == OpsBillType.InvisibleCredit || checkbilltype == OpsBillType.VisibleCredit || checkbilltype == OpsBillType.DoNotInclude;
			if ((is_exclude || exclude) && sales.CostOverRide == 0)
			{
				sales.CostOverRide = newcost;
			}
			if (sales.SellOverride == 0 && inv.master_id != OpsSpecialPart.MakeThisPart.ToString())
			{
				//_tools.page_author		= new NeMember(711);
				//_tools.catch_error(new Exception(string.Format(@"Zero Sellprice alert, here is the info:\n{0}", Toolbox.dict_dump(Toolbox.dict_create(sales)).Replace("<br/>", "\n"))));
			}

			//Check To See if this Will Create A negative Value on the Work Order
			var chkSql = "Select 0";
			if (!adding)
			{
				try
				{
					oldvalue = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current WHERE wo_detail_current_id = @v0 ", new object[] { newtableid });
				}
				catch (Exception ee)
				{
					Toolbox.do_errorLog_errorStack(ee);
				}
			}
			else if (adding && !exclude)
			{
				try
				{
					oldvalue = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current WHERE wo_detail_current_master_id = @v0  AND wo_detail_current_woprog_id = @v1 ", new object[] { partid, main_id });
				}
				catch (Exception ee)
				{
					Toolbox.do_errorLog_errorStack(ee);
				}
			}
			else if (exclude || sales.forcenewpartline)
			{
				if (newReqQuantity < 0 && !credit)
				{
					throw new Exception("You cannot add a negative qty as a new line.");
				}
			}

			if (oldvalue >=0 && oldvalue + comm_qty_diff < 0 && !isallowed_jobcosts_negative && !credit &&
				master_id != 0 &&
				!Toolbox.Contains(master_id, new[]{ OpsSpecialPart.QuoteLine,
													OpsSpecialPart.SubContractor, 
													OpsSpecialPart.CompanyCreditCardExpense,
													OpsSpecialPart.ExpenseReimbursement,
													OpsSpecialPart.NewChildWO,
													OpsSpecialPart.PerDiem}))
			{
				throw new Exception("You Cannot Make this Change, it will result in a negative committed qty on this workorder");
			}
			//sales.ActualQuantity = actualquantity;
			//sales.Desc = Toolbox.do_value_to(desc);
			if (orig_detail.type==OpsWOLineType.Labor && !adding)
			{
				sales.forcenewpartline = true;
			}
			sales.OrderedQuantity = adding && sales.OrderedQuantity == 0 && sales.Quantity > 0 ? sales.Quantity : sales.OrderedQuantity;
			if (chk_workorder_transfer && transferbvwo != "STOCKTRANSFER")
			{
				sales.transfer_to_line_id = Convert.ToInt32(transferbvwo);
			}
			var linetype = master_id < OpsSpecialPart.LaborThreshold ? OpsWOLineType.Material : master_id == OpsSpecialPart.QuoteLine ? OpsWOLineType.Quote : orig_detail.type;

			if (linetype == OpsWOLineType.Labor || linetype == OpsWOLineType.Mileage && !is_adding)
			{
				sales.trackmemberid = orig_detail.memberid;
				sales.trackpaytypeid = orig_detail.paytypeid;
			}
			recnumber = sales.SavePart(wo.woprog_id, "FAL1037931E", DSN, current_user.FullName, current_user.id, adding, "", false, 0, original, true);
			sales.OrderedQuantity = newReqQuantity;
			double checkcommit = 0;
			if (TextComQty != null)
			{
				double.TryParse(TextComQty.Text, out checkcommit);
			}

			var transfer_to_wo = new NeWOProg();
			if (chk_workorder_transfer && transferbvwo != OpsSpecialWorkOrder.StockTransfer)
			{
				transfer_to_wo = new NeWOProg(xfer_to_woprog_id); // Use ID
																  //transfer_to_wo = new NeWOProg(WorkingBusinessUnit.id, transferbvwo);
			}
			if (checkcommit != 0 && adding && !dont_force_comm_pop)
			{
				new_qty_committed = checkcommit;
			}
			if (!chk_workorder_transfer && (new_qty_committed != 0 || comm_qty_diff != 0) && new_qty_committed != orig_detail.qty_committed)
			{
				#region Is not a Work Order Transfer, Just pulling from stock
				if (master_id < OpsSpecialPart.LaborThreshold && 
						(
							!is_exclude && inv.allowed_to_stock || 
							!inv.allowed_to_stock && new_qty_committed > 0 || 
							master_id == OpsSpecialPart.MiscMaterial
							))
				{
					var inv_location = xfer_comm_recv_select_id > 0 ? new location_master(xfer_comm_recv_select_id) : new location_master();
					var used_bid = xfer_comm_recv_select_id > 0 ? inv_location.business_unit_id : WarehouseBusinessUnit.id;
				var from_stock = new Nestock_transfer
									{
									type_id   = 1,
									master_id = master_id,
									from_location_id = xfer_comm_recv_select_id != 0 ? xfer_comm_recv_select_id : 0,
									from_id = xfer_comm_recv_select_id != 0 ? xfer_comm_recv_select_id : 0,
									to_id = main_id,
									quantity = orig_detail.qty_committed - new_qty_committed * newqtyperpart,
									description      = newdescription,
									member_id        = current_user.id,
									note             = "Quantity committed to work order",
									business_unit_id = used_bid,
									cost = orig_detail.cost != 0 && new_qty_committed < orig_detail.qty_committed && newtableid != 0
										? orig_detail.cost
										: Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {partid, used_bid})
									};
				// This is to designate that it is going TO a work order, from generic stock
				// This should never be zero... Cover it in the class, if it's zero, it will pull from the largest qty internal location.
				if (xfer_comm_recv_select_id != 0)
					{
						try
						{
							// Get Location Name
							var location_name = Toolbox.doSQL_string(@"SELECT name FROM inventory_location_master WHERE id = @v0 ", new object[] { xfer_comm_recv_select_id });
							orig_detail.notes = "Committed from Location: " + location_name;
						}
						catch (Exception ee)
						{
							Toolbox.do_errorLog_errorStack(ee);
						}
					}
					if (from_stock.quantity != 0)
					{
						from_stock.Nestock_transfer_save();
					}
				}
				#endregion
			}
			if (xfer_qty_wo != 0)
			{
				#region Handle WO transfers
				if (chk_workorder_transfer && transferbvwo != OpsSpecialWorkOrder.StockTransfer)
				{
					#region Is a work order to work order transfer
					sales.Quantity = xfer_qty_wo;
					sales.ActualQuantity = xfer_qty_wo;
					sales.OriginWorkOrder = wo.OrderNumber;
					sales.OrderedQuantity = xfer_qty_wo;
					sales.line_origin = "Transferred From: " + wo.OrderNumber;
					var c_exists_target_wo = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transfer_to_wo.woprog_id, inv.master_id });
					if (is_exclude || c_exists_target_wo == 0)
					{
						// New Line on to WO
						sales.transfer_to_line_id = 0;
					}
					else if (c_exists_target_wo > 1)
					{
						throw new Exception("There are multiple lines on the transfer to work order, and this part is not an inventory exclude part... this should never happen.");
					}
					else
					{
						sales.transfer_to_line_id = Toolbox.doSQL_int(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transfer_to_wo.woprog_id, inv.master_id });
					}
					if (!is_exclude)
					{
						var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_bvwo = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transferbvwo, inv.master_id });
						if (c == 1) // Should only be a max of one on the destination work order
						{
							// We need the TO WO line
							var wodc_to = new NeWODetailCurrent();
							var to_line_id = Toolbox.doSQL_string(@"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_bvwo = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transferbvwo, inv.master_id });
							wodc_to.GetLineDetails(to_line_id);
						bd = new bingo_data
								{
								before_cost = wodc_to.cost,
								before_qty  = wodc_to.qty_committed,
								before_sell = wodc_to.sell,
								master_id   = wodc_to.master_id,
								ca_cost     = sales.CostOverRide,
								ca_qty      = sales.ActualQuantity,
								ca_sell     = wodc_to.sell
								};
							sales.CostOverRide = Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { wodc_to.woprog_id, master_id, sales.ActualQuantity, sales.CostOverRide });
							sales.SellOverride = wo.use_fixed_material_markup ? sales.CostOverRide * wo.fixed_material_markup : 0;
							sales.ManualChange = false;
							bd.after_cost = sales.CostOverRide;
							bd.after_qty = wodc_to.qty_committed + sales.ActualQuantity;
							bd.after_sell = sales.SellOverride;
							bd.detail_id = wodc_to.id;
							bd.woprog_id = wodc_to.woprog_id;
							bd.save();
						}
					}
					// Saving the xfer to work order

					RecordNumTwo = sales.SavePart(transfer_to_wo.woprog_id, "TRUE", DSN, current_user.FullName, current_user.id, adding, "", false, consignment_id, original, true);
					rec_no_from_new_wo_line_item = RecordNumTwo;
					wo_detail_current_id_target = sales.wo_detail_current_Id;

					if ( (master_id == OpsSpecialPart.ExpenseReimbursement || master_id == OpsSpecialPart.PerDiem) && (original == "Expense Reimbursement" || original == "Per Diem Expense") && consignment_id > 0)
					{
						// Go to expense_reimbursement table to change the woProg_id to new wo#
						Toolbox.doSQL_void(@"update expense_reimbursement set woprog_id=@v0 where id_expense=@v1", new object[] { transferbvwo, consignment_id });
					}

					if (master_id == OpsSpecialPart.CompanyCreditCardExpense && (original == "Company Credit Card Expense") && consignment_id > 0)
					{
						// Go to expense_reimbursement table to change the woProg_id to new wo#
						Toolbox.doSQL_void(@"UPDATE credit_card_purchase SET woprog_id = @v0 WHERE id = @v1", new object[] { transferbvwo, consignment_id });
					}

					#endregion Is a work order to work order transfer
				}
				#endregion Handle WO transfers
			}
			if (xfer_qty_internal != 0)
			{
				#region Handle Internal transfers - From WO to Specified Internal Location
				if (master_id < OpsSpecialPart.LaborThreshold && inv.allowed_to_stock)
				{
					var inv_location = new location_master(xfer_internal_select_id);
					var st_internal = new Nestock_transfer
						{
						from_id = main_id,
						to_id = (int)inv_location.id,
						type_id = 3,    // 3 = Internal Location
						master_id = master_id,
						to_location_id = (int)inv_location.id,
						quantity = xfer_qty_internal,
						description = newdescription,
						member_id = current_user.id,
						note = transfernote,
						business_unit_id = inv_location.business_unit_id,
						cost = oldcost
						};
					st_internal.Nestock_transfer_save();
				}
				#endregion Handle Internal transfers
			}
			if (xfer_qty_external != 0)
			{
				#region Handle External transfers - From WO to Specified External Location
				if (master_id < OpsSpecialPart.LaborThreshold && inv.allowed_to_stock)
				{
					var inv_location = new location_master(xfer_external_select_id);
					var st_external = new Nestock_transfer
						{
						from_id = main_id,
						to_id = (int)inv_location.id,
						to_location_id = (int)inv_location.id,
						type_id = 4,    // 4 = External Location
						master_id = master_id,
						quantity = xfer_qty_external,
						description = newdescription,
						member_id = current_user.id,
						note = transfernote,
						business_unit_id = inv_location.business_unit_id,
						cost = oldcost
						};
					st_external.Nestock_transfer_save();
				}
				#endregion Handle External transfers
			}
			notesforwoaudit = orig_detail.notes;
			//AuditSave(wo.OrderNumber, Convert.ToString(recnumber), CompID, strwoid, adding, is_manual_price_change, commit_wo);
			if (chk_workorder_transfer && transferbvwo != OpsSpecialWorkOrder.StockTransfer)
			{
				AuditSave(transferbvwo, Convert.ToString(RecordNumTwo), WarehouseBusinessUnit.id, transferwoprogid, adding, sales.ManualChange, commit_wo);
			}

			NeWOProg.update_header_totals(strwoid, WorkingBusinessUnit.id, wo.OrderNumber);
			if (chk_workorder_transfer && transferbvwo != OpsSpecialWorkOrder.StockTransfer)
			{
				NeWOProg.update_header_totals(transferwoprogid, WorkingBusinessUnit.id, transferbvwo);
			}
			TextCost.ClientEnabled = false;
		}
	}
	protected void CheckLabourAgainstTimeSheet(string partlineid, string WONum, double quantity)
	{
		try
		{
			var getmembercost = "SELECT wo_detail_current_price_cost FROM wo_detail_current WHERE wo_detail_current_id=" + partlineid;
			double timecost = 0;
			try
			{
				timecost = Toolbox.doSQL_double(@"SELECT wo_detail_current_price_cost FROM wo_detail_current  WHERE wo_detail_current_id=@v0", new object[] { partlineid });
			}
			catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
			var getmemberprice = "SELECT wo_detail_current_price_sell FROM wo_detail_current WHERE wo_detail_current_id=" + partlineid;
			double timeprice = 0;
			try
			{
				timeprice = Toolbox.doSQL_double(@"SELECT wo_detail_current_price_sell FROM wo_detail_current  WHERE wo_detail_current_id=@v0", new object[] { partlineid });
			}
			catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
			double chargingprice = 0;
			var customer_id = 0;
			customer_id = wo.WOProg_Customer_ID;
			try
			{
				chargingprice = Toolbox.doSQL_double(@"CALL CUSTOMER_CHARGEOUT(@v0 , @v1 )", new object[] { customer_id, newpartnumber });
			}
			catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }

			var getMemberID = "SELECT memberid FROM wo_detail_current WHERE wo_detail_current_id=" + partlineid;
			var _id = Toolbox.doSQL_string(@"SELECT memberid FROM wo_detail_current  WHERE wo_detail_current_id=@v0", new object[] { partlineid });
			var getpaytypeid = "SELECT paytypeid FROM wo_detail_current WHERE wo_detail_current_id=" + partlineid;
			var paytypeid = Toolbox.doSQL_string(@"SELECT paytypeid FROM wo_detail_current  WHERE wo_detail_current_id=@v0", new object[] { partlineid });
			var strHoursSum = "SELECT IFNULL(SUM(NumberofHours),0) FROM membertime WHERE MemberTime_Mileage = 'false' AND MemberTime_WorkOrder_ID = '" + WONum + "' AND business_unit_id = " + _id + " AND MemberTime_PayTypeHours_ID = " + paytypeid;

			var totalhours = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(NumberofHours),0) FROM membertime  WHERE MemberTime_Mileage = 'false' AND MemberTime_WorkOrder_ID =@v0 AND business_unit_id =@v1  AND MemberTime_PayTypeHours_ID =@v2 ", new object[] { WONum, _id, paytypeid });
		}
		catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
	}
	private void AuditSave(string WoNum, string recnum, int CompID, string woid, bool addnew, bool ManualPriceChange) { AuditSave(WoNum, recnum, CompID, woid, addnew, ManualPriceChange, false); }
	private void AuditSave(string WoNum, string recnum, int CompID, string woid, bool addnew, bool ManualPriceChange, bool commit_wo)
	{
		var changes = new NeWOProgChanges();
		changes.WoProgChanges_BVWO = WoNum;
		changes.WoProgChanges_BVWORec = recnum;
		changes.WoProgChanges_WOProg_ID = Convert.ToInt32(woid);
		changes.WoProgChanges_Modified_Member_ID = current_user.id;
		changes.WOProgChanges_ManualPriceChange = ManualPriceChange && transferbvwo != WoNum ? 1 : 0;

		var _checksell = wo.use_fixed_material_markup ? wo.fixed_material_markup * newcost : shared.GetSellPrice(newcost, 0, true, new_qty_committed, WorkingBusinessUnit.id32);

		if (!addnew)
		{
			changes.WoProgChanges_WasPartNo = oldpartnumber;
			changes.WoProgChanges_WasPrice = oldorigsell.ToString();
		}
		else
		{
			changes.WoProgChanges_WasPrice = _checksell.ToString();
		}
		changes.WoProgChanges_WasPartNo = oldpartnumber;
		changes.WoProgChanges_WasPrice = oldorigsell.ToString();
		changes.WoProgChanges_WasQty = transferbvwo == WoNum ? "0" : old_qty_committed.ToString();
		changes.WoProgChanges_WasDesc = olddescription;
		changes.WOProgChanges_WasBillingType = string.IsNullOrEmpty(oldbilltype) ? 0 : Convert.ToInt32(oldbilltype);

		changes.WoProgChanges_IsPrice = neworigsell.ToString();
		changes.WoProgChanges_IsPartNo = newpartnumber;
		changes.WoProgChanges_IsQty = new_qty_committed.ToString();
		changes.WoProgChanges_DeleteFlag = "false";
		changes.WoProgChanges_IsDesc = newdescription;
		changes.WoProgChanges_IsBillingType = string.IsNullOrEmpty(newbilltype) ? 0 : Convert.ToInt32(newbilltype);
		if (notesforwoaudit != "")
		{
			try
			{
				notesforwoaudit = Toolbox.do_value_from(notesforwoaudit);
			}
			catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
		}
		changes.FullWOComment = notesforwoaudit;
		changes.WOProgChanges_OrderedQty = (oldqty + newReqQuantity).ToString();
		changes.was_req_qty = oldqty;
		var _isqty = Convert.ToDouble(changes.WoProgChanges_IsQty);
		var _wasqty = Convert.ToDouble(changes.WoProgChanges_WasQty);
		var _diff = _wasqty - _isqty;

		try
		{
			xfer_combined_qty = Convert.ToDouble(changes.WoProgChanges_IsQty) - Convert.ToDouble(changes.WoProgChanges_WasQty);
			xfer_combined_qty = Math.Abs(xfer_combined_qty);
		}
		catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
		if (transferbvwo == WoNum)  // if it's actually a transfer
		{
			changes.FullWOComment += " - Transferred " + xfer_qty_wo + " from work order: " + originbvwo + " " + transfernote;
			changes.WoProgChanges_IsQty = xfer_qty_wo.ToString();
		}
		else
		{
			if (transferbvwo != "0")
			{
				if (transferbvwo == OpsSpecialWorkOrder.StockTransfer)
				{
					double _int = 0;
					double _ext = 0;
					var _int_str = xfer_internal_qty.Value != null ? xfer_internal_qty.Value.ToString() : "0";
					var _ext_str = xfer_external_qty.Value != null ? xfer_external_qty.Value.ToString() : "0";
					double.TryParse(_int_str, out _int);
					double.TryParse(_ext_str, out _ext);
					var location = transferbvwo;
					if (_diff == _int)
					{
						location = xfer_internal_combo.Text.Split('-')[0];
					}
					else if (_diff == _ext)
					{
						location = xfer_external_combo.Text.Split('-')[0];
					}
					changes.FullWOComment += " - Transferred " + xfer_combined_qty + " to location: " + location + " " + transfernote;
				}
				else
				{
					changes.FullWOComment += " - Transferred " + xfer_combined_qty + " to work order: " + transferbvwo + " " + transfernote;
				}
			}
		}

		if (commit_wo)
		{
			var po_number = Toolbox.doSQL_int(@"SELECT poprog_bvpo FROM poprog_header  WHERE poprog_id =@v0", new object[] { id });
			changes.FullWOComment += string.Format("Committed directly to WO upon receipt of PO#: {0} - Changing the commit qty from {1} to {2}", po_number, changes.WoProgChanges_WasQty, changes.WoProgChanges_IsQty);
			changes.WoProgChanges_WasPartNo = changes.WoProgChanges_IsPartNo;
		}
		changes.business_unit_id = Convert.ToInt32(CompID);
		changes.AddtoWOProgChanges();
	}
	#endregion
	#region Send Email
	protected void SendEmail(int messagetype, string woid, int to_member_id)
	{
		//Send Mail to Project Manager Letting them Know that their Part(s) has arrived
		var progress = new NeWOProg(Convert.ToInt32(woid));
		var poprogress = new NePOProg(main_id);
		var customername = progress.CustomerName;
		var	toemail = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(member_neemail), '') FROM member WHERE member_id = @v0", new object[] { to_member_id });
		var	ccemail = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(member_neemail), '') FROM member WHERE member_membertype_id = 9 AND member_status='Active' AND business_unit_id=@v0 limit 1 ", new object[] { progress.business_unit_id });
		var fromemail = ccemail;
		var subject = $@"Parts for WO:{progress.OrderNumber} for {customername} have arrived into stock.";
		var body = $@"
		Your part: {newpartnumber}: {newdescription} has arrived for the job. 
		{new_qty_committed} have arrived to date, of {newReqQuantity} ordered.
		The part(s) was ordered {poprogress.poprog_order_placed_date}";

		if (toemail != "")
			{
			try
				{
				var e = new NeEMail
					{
					To = toemail,
					From = fromemail == "" ? "admin@" + Toolbox.app_setting("DomainForEmail") : fromemail,
					Subject = subject,
					Body = body,
					CC = ccemail
					};
				e.Send();
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog(ee, "Problem sending part tracking email");
				}
			}
		else
			{
			try
				{
				var e = new NeEMail
					{
					To = "mhyde@thatsnew.com",
					From = fromemail == "" ? "administrator" + Toolbox.app_setting("DomainForEmail") : fromemail,
					Subject = subject,
					Body = body
					};
				e.Send();
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog(ee, "Problem sending part tracking email");
				}
			}
	}
	#endregion
	#region Editor Initilization
	protected void agv_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
	{

		var grid = (ASPxGridView)sender;
		var editingkey = e.KeyValue;
		var editrowlineid = editingkey.ToString();
		int.TryParse(grid.GetRowValues(e.VisibleIndex, GVColumns.MasterId).ToString(), out int part_no);
		var linetype = grid.GetRowValues(e.VisibleIndex, GVColumns.LineType).ToString();
		var row_origin = grid.GetRowValues(e.VisibleIndex, GVColumns.Origin).ToString();
		var billTypeId = Toolbox.ReturnZeroIfNull_int(grid.GetRowValues(e.VisibleIndex, GVColumns.BillType));
	var is_expense_line = row_origin == OpsWOLineOrigin.ExpenseReimbursement || row_origin == OpsWOLineOrigin.PerDiemExpense;
		var is_credit_card = row_origin == OpsWOLineOrigin.CompanyCreditCardExpense;
		var isImportedWIP = row_origin.Contains(OpsWOLineOrigin.ImportedWIP);
		var isImportedCOGS = row_origin.Contains(OpsWOLineOrigin.ImportedCOGS);
		var field = e.Column.FieldName;
		var consignment_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(wo_detail_current_consignment_id), 0) FROM wo_detail_current  WHERE wo_detail_current_id =@v0", new object[] { editrowlineid });
		var _is_progress = Toolbox.doSQL_int(@"Select woprog_associate_woprog_id from woprog  where woprog_id =@v0", new object[] { main_id }) > 0;
		if (field == GVColumns.MasterId)
		{
			FlagPartcode = Convert.ToString(e.Value) != "";
			lockdesc = false;
			if (part_no == OpsSpecialPart.MakeThisPart && current_user != null && current_user.AuthenticatedForPrivilege(OpsPrivilege.PurchaserAccess))
			{
				e.Editor.ClientEnabled = false;
			}
			else
			{
				//e.Editor.Enabled = false;
				e.Editor.ClientEnabled = false;
			}
		}
		if (field == GVColumns.Cost)
		{

			e.Editor.Visible = linetype == OpsWOLineType.Labor 
								? member_can_see_labour_cost 
								: member_can_see_cost;
			e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
			var isLaborOrQuote = part_no == OpsSpecialPart.QuoteLine || part_no >= OpsSpecialPart.LaborThreshold;
			e.Editor.ClientEnabled = priv_nesi_invoiced_edit && 
									 ( part_no == OpsSpecialPart.SubContractor || 
									   part_no == OpsSpecialPart.CompanyCreditCardExpense || 
									   part_no == OpsSpecialPart.ExpenseReimbursement ||
									   part_no == OpsSpecialPart.PerDiem ||
									   part_no == OpsSpecialPart.NewChildWO
									  ) || 
									 !priv_nesi_invoiced_edit && can_edit_part_cost && isLaborOrQuote;
			if (priv_nesi_invoiced_edit || isLaborOrQuote) // Need to detach the cost marking up the sell
			{
				var editor = e.Editor as ASPxTextBox;
				editor.ClientSideEvents.TextChanged = "";
			}
			if(isImportedWIP|| isImportedCOGS)
			{
				e.Editor.Visible = false;
			}
		}
		if (field == GVColumns.Sell && !priv_nesi_invoiced_edit)
		{
			if (member_can_edit_sell && member_can_see_sell && !_is_progress)
			{
				e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
				e.Editor.ReadOnly = false;
				e.Editor.ClientEnabled = true;
			}
		}
		else if (field == GVColumns.Sell)
		{
			e.Editor.ReadOnly = true;
			e.Editor.ClientEnabled = false;
		}
		if (field == GVColumns.Description && (lockdesc || consignment_id != 0 || priv_nesi_invoiced_edit) && part_no!= OpsSpecialPart.PerDiem && part_no!= OpsSpecialPart.ExpenseReimbursement && part_no!= OpsSpecialPart.CompanyCreditCardExpense)
		{
			e.Editor.ClientEnabled = false;
		}
		if(Toolbox.Contains(field, new []{	GVColumns.RecNo, 
											GVColumns.LineType
											}))
			{
			e.Editor.ClientEnabled = false;
			}
		if (field == GVColumns.QtyRequired)
		{
			e.Editor.Attributes["data-line_id"] = e.KeyValue.ToString();
			e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
			try
			{
				poqty = Convert.ToDouble(e.Value);
			}
			catch (Exception ee)
			{
				Toolbox.do_errorLog(ee, "Issue converting qty req");
			}
			e.Editor.ClientEnabled = !is_expense_line && !is_credit_card && !priv_nesi_invoiced_edit && !isImportedCOGS && !isImportedWIP;
		}
		if (field == GVColumns.QtyCommittedToDate)
		{
			try
			{
				e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
				agv.JSProperties["cpOrigValue"] = e.Value.ToString();
				agv.JSProperties["cplineid"] = e.KeyValue;
			}
			catch (Exception ee)
			{
				Toolbox.do_errorLog(ee, "Issue assigning JS properties / JS Event to committed to date field");
			}
			if (part_no != OpsSpecialPart.QuoteLine || is_expense_line || is_credit_card)
			{
				e.Editor.ClientEnabled = false;
			}
			if (consignment_id != 0)
			{
				e.Editor.ClientEnabled = false;
			}
		}
		if (field == GVColumns.ExtTandM)
		{
			e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
			e.Editor.Value = string.Format("{0:N3}", e.Value).Replace(",", "");
			e.Editor.ClientEnabled = false;
		}
		if (field == GVColumns.Discount)
		{
			e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
			if (!member_can_discount || _is_progress || part_no >= OpsSpecialPart.LaborThreshold || is_expense_line || is_credit_card || priv_nesi_invoiced_edit || isImportedWIP || isImportedCOGS)
			{
				e.Editor.ClientEnabled = false;
			}
		}
		if (field == GVColumns.BillType)
		{
			var allowedBillTypes = BillTypeMovement.AllowedBillTypes(billTypeId, wo);
			if(allowedBillTypes.Any())
				{
				if(!allowedBillTypes.Contains(billTypeId))
					{ 
					allowedBillTypes.Add(billTypeId); // Need to ensure that if the billtype doesn't exist already in the allowed list, that it's textual counterpart is maintained.
					}
				SqlDataSourceBillTypes.SelectCommand = $"SELECT wo_lineitem_billtypeid, wo_lineitem_billtype_name FROM wo_detail_lineitem_billtype WHERE wo_lineitem_billtypeid IN ({string.Join(",", allowedBillTypes)}) ";
				}
			if(e.Editor is ASPxComboBox billTypeSelector)
				{
				billTypeSelector.DataBind();
				}
			e.Editor.ClientEnabled = member_can_see_sell && member_can_edit_sell && !priv_nesi_invoiced_edit && allowedBillTypes.Any();
		}
		if (field == GVColumns.DateRequired)
		{
			e.Editor.ClientEnabled = !is_expense_line && !is_credit_card && !priv_nesi_invoiced_edit && isImportedWIP && isImportedCOGS;
		}
		if (Toolbox.Contains(e.Column.Name.ToLower(), new[] {		GVColumns.Transfer, 
																	GVColumns.RecNo, 
																	GVColumns.Unfulfilled, 
																	GVColumns.QtyReceiving, 
																	GVColumns.Notes, 
																	GVColumns.Location, 
																	GVColumns.Select, 
																	GVColumns.History, 
																	GVColumns.Origin, 
																	GVColumns.Margin, 
																	GVColumns.TrackPart,
																	GVColumns.AvailableQuantity,
																	GVColumns.ExtTandM
																	}))
		{
			e.Editor.Visible = false;
		}
		if(field == GVColumns.ActivityCode)
			{
			((ASPxTextBox) e.Editor).NullText		= "Activity Code";
			}
		if (field == GVColumns.CostElement)
			{
			((ASPxTextBox) e.Editor).NullText		= "Cost Element";
			}
		if (field == GVColumns.ClientWO)
			{
			((ASPxTextBox) e.Editor).NullText		= "Client WO#";
			}
		if (field == GVColumns.ClientPO)
			{
			((ASPxTextBox) e.Editor).NullText		= "Client PO#";
			}
		}
	protected void agv_ParseValue(object sender, ASPxParseValueEventArgs e)
	{
		if (e.FieldName == GVColumns.Location)
		{
			try
			{
				e.Value = e.Value == null ? 0 : int.Parse(e.Value.ToString());
			}
			catch
			{
				e.Value = 0;
			}
			if (e.FieldName == GVColumns.QtyReceiving)
			{
				try
				{
					e.Value = Convert.ToDecimal(e.Value);
				}
				catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
			}
		}
	}
	protected void agv_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
	{
		var grid = (ASPxGridView)sender;
		wo_status_str = wo_status_str == "" ? Toolbox.doSQL_string(@"SELECT woprog_status FROM woprog WHERE woprog_id = @v0 ", new object[] { main_id }) : wo_status_str;
		var column_chkbox = (GridViewDataColumn)grid.Columns[GVColumns.Select];
		switch (e.Column.Name)
		{
			case GVColumns.Transfer:
				if (!new List<string>(new[]{
													OpsWOStatus.Invoiced,
													OpsWOStatus.WaitingToBeInvoiced,
													OpsWOStatus.WaitingParentBMApproval,
													OpsWOStatus.WaitingForPO,
													OpsWOStatus.WaitingBMApproval,
													OpsWOStatus.WaitingPMApproval
													}).Contains(wo_status_str) && column_chkbox.Visible)
				{
					e.Cell.Text = "<div class=\'date_req_container dxbButton\' style=\'cursor:pointer;padding:3px;width:22px;height:22px\'><img src=\'/images/icon/transfer.jpg\' style=\'cursor:pointer;padding:3px;width:16px;height:16px\' align=\'absmiddle\' title=\'Mass Transfer\' onclick=\'transfer_multiple_toggle(this)\'/></div>";
				}
				break;
			case GVColumns.DateRequired:
				if (!new List<string>(new[]{
													OpsWOStatus.Invoiced,
													OpsWOStatus.WaitingToBeInvoiced,
													OpsWOStatus.WaitingParentBMApproval,
													OpsWOStatus.WaitingForPO,
													OpsWOStatus.WaitingBMApproval,
													OpsWOStatus.WaitingPMApproval
													}).Contains(wo_status_str) && column_chkbox.Visible)
				{
					e.Cell.Text = "<div class=\'date_req_container dxbButton\' style=\'cursor:pointer;padding:3px;width:22px;height:22px\'><img src=\'/images/icon/icon[calendar].gif\' style=\'cursor:pointer;padding:3px;width:16px;height:16px\' align=\'absmiddle\' title=\'Mass Edit Required Date(s)\' onclick=\'reqdate_edit(this)\'/></div>";
				}
				break;
			case GVColumns.BillType:
				return;
				if (member_can_see_sell && member_can_edit_sell && !new List<string>(new[]{
													OpsWOStatus.Invoiced,
													OpsWOStatus.WaitingToBeInvoiced,
													OpsWOStatus.WaitingParentBMApproval,
													OpsWOStatus.WaitingForPO
													}).Contains(wo_status_str) && column_chkbox.Visible)
				{
					try
					{
						e.Cell.Text = "<div class=\'billtype_container dxbButton\' style=\'cursor:pointer;padding:3px;width:22px;height:22px\'><img src=\'/images/icon/icon[accounting].gif\' title=\'Mass set bill type(s)\'  style=\'cursor:pointer;padding:3px;width:16px;height:16px\' align=\'absmiddle\' alt=\'Mass set bill type(s)\' onclick=\'billtype_edit(this)\'/></div>";
					}
					catch (Exception ee)
					{
						Toolbox.do_errorLog_errorStack(ee);
					}
				}
				break;
		}
	}
	protected void agv_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
	{
		if(e.VisibleIndex < 0) return;
		var partiallyBilled = Toolbox.ReturnZeroIfNull_int(agv.GetDataRow(e.VisibleIndex)[GVColumns.PartiallyBilled]) == 1;
		if (agv.IsEditing && agv.EditingRowVisibleIndex == e.VisibleIndex)
		{
			e.Row.BackColor = Color.Coral;		
		}
		if(partiallyBilled)
			{
			e.Row.BackColor = Color.FromArgb(0,255,0);
			}

	}
	protected void agv_DataBound(object sender, EventArgs e)
	{
		if (agv.VisibleRowCount == 0 ) return;	
		set_location_ds(agv, null);
		
	}
	Dictionary<int, string> wo_tooltips = new Dictionary<int, string>();
	DataTable dt_excludes;
	string wo_status_str = "";
	protected void agv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		var grid = (ASPxGridView)sender;
		var wo_id = 0;
		var row_id = grid.GetRowValues(e.VisibleIndex, GVColumns.LineId);
		var col_workorders = grid.Columns[GVColumns.Location];
		var origin = Toolbox.ReturnBlankIfNull_string(grid.GetRowValues(e.VisibleIndex, GVColumns.Origin));
		var labourTooltip = Toolbox.ReturnBlankIfNull_string(grid.GetRowValues(e.VisibleIndex, GVColumns.labourTooltip));
		var this_row = grid.GetDataRow(e.VisibleIndex);
		var linetype = grid.GetRowValues(e.VisibleIndex, GVColumns.LineType).ToString();
		var part_no = 0;
		if (grid.GetRowValues(e.VisibleIndex, GVColumns.MasterId) != null)
		{
			int.TryParse(grid.GetRowValues(e.VisibleIndex, GVColumns.MasterId).ToString(), out part_no);
		}
		wo_status_str = this_row[GVColumns.Location].ToString();

		if (wo_id != 0 && wo_id > 20000 && !new List<int>(new[] { 9999999, 9999998, 9999997 }).Contains(wo_id))
		{
			var wo_tooltip = Toolbox.doSQL_string(@"SELECT CONCAT(WOProg_BVWO,' - ', WOPRog_CustomerName, ' ', WOProg_Description, ' - ', name) FROM woprog w, business_unit b WHERE w.business_unit_id = b.Id and woprog_id = @v0 ", new object[] { wo_id });
			if (e.DataColumn.FieldName.Equals(GVColumns.Location))
			{
				e.Cell.ToolTip = wo_tooltip;
			}

			//	wo_tooltips.Add(wo_id, wo_tooltip);
		}

		if (dt_excludes == null)
		{
			var part_numbers = new List<string>();
			for (var wi = 0; wi < grid.VisibleRowCount; wi++)
			{
				var part_nu = grid.GetRowValues(wi, GVColumns.MasterId).ToString() == "" ? 0 : Convert.ToInt32(grid.GetRowValues(wi, GVColumns.MasterId).ToString());
				if (linetype != "L" && part_nu != 0)
				{
					part_numbers.Add(grid.GetRowValues(wi, GVColumns.MasterId).ToString());
				}
			}
			if (part_numbers.Count > 0)
			{
				dt_excludes = Toolbox.doSQL_dt(string.Format(@"SELECT a.master_id, b.is_exclude, b.allowed_to_stock FROM inventory_item_master a
LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id WHERE a.master_id IN ({0})", string.Join(",", part_numbers.ToArray())), null);
			}
		}

		var workorder_column = (GridViewDataColumn)grid.Columns[GVColumns.Location];
		var location_combo = (ASPxComboBox)grid.FindRowCellTemplateControl(e.VisibleIndex, workorder_column, "combo_location");
		var column_chkbox = (GridViewDataColumn)grid.Columns[GVColumns.Select];
		var is_expense_line = origin == OpsWOLineOrigin.ExpenseReimbursement || origin == OpsWOLineOrigin.CompanyCreditCardExpense || origin == OpsWOLineOrigin.PerDiemExpense;
		var is_credit_card = origin == OpsWOLineOrigin.CompanyCreditCardExpense;
		var isImportedWIP = origin.Contains(OpsWOLineOrigin.ImportedWIP);
		var isImportedCOGS = origin.Contains(OpsWOLineOrigin.ImportedCOGS);
		var isInventoryNonStock = false;
		var internalId = Toolbox.ReturnBlankIfNull_string(this_row["internal_id"]);
		var partiallyBilled = Toolbox.ReturnZeroIfNull_int(this_row[GVColumns.PartiallyBilled]) == 1;
		switch (e.DataColumn.FieldName)
		{
			case GVColumns.RecNo: 
				e.Cell.ToolTip = linetype == OpsWOLineType.Labor && issummarize_labor 
									? "NetSuite ID: Not showing while summarized" 
									: $@"NetSuite ID: {(internalId == "" 
														? "Not synced" 
														: internalId)}";
			break;
			#region cost
			case GVColumns.Cost:
					var costtext = (ASPxLabel)grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "FieldCostTextBox");
					if (linetype == OpsWOLineType.Labor && current_user.business_unit.summarize_labor && issummarize_labor)
					{
					   costtext.Visible = false;
					}
					else
					{
					   costtext.Visible = member_can_see_cost;
					}

			  break;
			#endregion cost
			#region description
			case GVColumns.Description:
				try
				{
					e.Cell.CssClass += " description";
					e.Cell.Style.Add("min-width", "150px");
					e.Cell.ToolTip =Toolbox.ReturnBlankIfNull_string(e.CellValue);
					if (!string.IsNullOrWhiteSpace(linetype) && linetype.ToUpperInvariant() == OpsWOLineType.Labor)
					{
						e.Cell.ToolTip =labourTooltip;
					}
				}
				catch (Exception ee) { }
				break;
			#endregion description
			#region workorder
			case GVColumns.Location:
				e.Cell.Style.Add("overflow", "hidden");
				e.Cell.ToolTip = "Select where the parts came from";
				break;
			#endregion workorder
			#region qty_receiving
			case GVColumns.QtyReceiving:
				var billtype_id = 0;
				var invPart = new inventory();
				invPart.Load(part_no, WorkingBusinessUnit.id32);
				isInventoryNonStock = invPart.Tag.id == OpsSpecialTag.NonStockItem;

				if (grid.GetRowValues(e.VisibleIndex, GVColumns.BillType) != null)
					if (part_no == OpsSpecialPart.QuoteLine || part_no == OpsSpecialPart.MakeThisPart)
					{
						int.TryParse(grid.GetRowValues(e.VisibleIndex, GVColumns.BillType).ToString(), out billtype_id);
					}
			 

				if (	part_no == OpsSpecialPart.MakeThisPart || part_no == OpsSpecialPart.NewChildWO ||
						Toolbox.Contains(linetype, new []{	OpsWOLineType.Asset, 
																OpsWOLineType.Labor, 
																OpsWOLineType.Mileage
																}) || 
						Toolbox.Contains(billtype_id, new []{	OpsBillType.QuotedPriceOld,
																	OpsBillType.ProgressBillingOld,
																	OpsBillType.QuotedPrice,
																	OpsBillType.ProgressBilling
																	}) || 
						part_no != OpsSpecialPart.MiscMaterial && invPart.is_exclude && origin.Contains("PO 000") ||
						is_credit_card || 
						is_expense_line || 
						isImportedCOGS || 
						isImportedWIP || 
						partiallyBilled || 
						isInventoryNonStock
						)

				{
					var gv = (GridViewDataColumn)agv.Columns[GVColumns.QtyReceiving];
					var editor = (TextBox)grid.FindRowCellTemplateControl(e.VisibleIndex, gv, "txtRecSelectQty");
					if (editor != null)
					{
						editor.Enabled = false;
					}
				}
				break;
			#endregion qty_receiving
			#region wo_detail_current_billtypeid
			case GVColumns.BillType:
				break;
			#endregion wo_detail_current_billtypeid
			#region Trans
			case GVColumns.Transfer:
				var commit_qty = Convert.ToDouble(agv.GetRowValues(e.VisibleIndex, GVColumns.QtyCommittedToDate));
				e.Cell.ToolTip = "Transfer Part to Another Work Order";
				// Pure Revenue lines should not be allowed to transfer
				if (	linetype == OpsWOLineType.Labor || 
						linetype == OpsWOLineType.Asset ||
						linetype == OpsWOLineType.Mileage ||
						partiallyBilled ||
						(linetype == OpsWOLineType.Material && Toolbox.Contains(part_no, new int[] {OpsSpecialPart.MiscMaterial, OpsSpecialPart.PerDiem, OpsSpecialPart.ExpenseReimbursement, OpsSpecialPart.CompanyCreditCardExpense}))
						)
				{
					e.Cell.Text = "";
				}
				else
				{
					if (commit_qty <= 0 || part_no == OpsSpecialPart.QuoteLine)
					{
						e.Cell.Text = "";
						e.Cell.ToolTip = "Must have more than 0 Committed to Transfer";
					}
				}

				//
				// If the PO is closed, the item cannot be transferred; a negative Purchase Order is required, so the used should be warned/informed at this point.
				//
				if (!IsTransferAvailableIfPoBehindIt(main_id, origin, part_no))
				{
					// Matt found:
					// By default .Cell.Text is = <a href="javascript:void(0);" onclick="TransferItems('<%# Container.KeyValue %>', false)"><img alt="Transfer" src="/images/icon/transfer.jpg" width="16" height="16" /></a>
					// set it to "" to remove the inner html.
					e.Cell.Text = "";
				}

				//
				// [1903] Removing transfer capability on imported lines.
				//
				if(origin.Contains(OpsWOLineOrigin.ImportedWIP) || origin.Contains(OpsWOLineOrigin.ImportedCOGS))
					{
					e.Cell.Text = "";
					}
				// [1107] Do not allow expenses, per diems, cc purchases to be transferred if the isMigratedFlag is true, or if the pay period has been processed
				// The code here is to control the 'transfer' button: showing or not showing.
				//
				var okay = IsExpensePerDiemsCCPurchaseAvailableForTransferIfTheItemIsConsidered(origin, 0, main_id,(int)row_id, DateTime.Now, false);
				if (!okay)
				{
					// The expense suff is not allowed to be transfered.
					e.Cell.Text = "";
				}

				break;
			#endregion Trans
			#region Required Date
			case GVColumns.DateRequired:
				if (!new List<string>(new[]{
													OpsWOStatus.Invoiced,
													OpsWOStatus.WaitingToBeInvoiced,
													OpsWOStatus.WaitingForPO,
													OpsWOStatus.WaitingParentBMApproval,
													OpsWOStatus.WaitingPMApproval,
													OpsWOStatus.WaitingBMApproval
													}).Contains(wo_status_str) && column_chkbox.Visible)
				{
				   // var this_datereq = e.CellValue == null || e.CellValue == DBNull.Value ? "" : Convert.ToDateTime(e.CellValue).ToString("MM/dd/yy");
					var this_datereq =Toolbox.ReturnNullDateTime(e.CellValue)==null ? "" : Convert.ToDateTime(e.CellValue).ToString("MM/dd/yy");
					e.Cell.Text = string.Format(@"<b title=""{0}"">{0}</b>", this_datereq);
				}
				break;
			#endregion Required Date
			#region part_no
			case GVColumns.MasterId:
				//e.Cell.Style.Add("min-width", "100px");
				//e.Cell.Style.Add("max-width", "100px");
				e.Cell.CssClass += " part_no";
				//var linetype = grid.GetRowValues(e.VisibleIndex, "linetype").ToString();
		   
				try
				{
					 if (linetype==OpsWOLineType.Material)
					{
						var part_is_exclude = "false";
						var part_allowed_to_stock = "false";
						if (dt_excludes != null && dt_excludes.Rows.Count > 0)
						{
							var dr = dt_excludes.Select("master_id = '" + part_no + "'");
							if (dr.Length > 0 && dr[0].ItemArray.Length > 0 && dr[0]["is_exclude"].ToString() == "1")
							{
								part_is_exclude = "true";

							}
							if (dr.Length > 0 && dr[0].ItemArray.Length > 0 && dr[0]["allowed_to_stock"].ToString() == "1")
							{
								part_allowed_to_stock = "true";
							}
						}

						e.Cell.Text =
							$"<a href=\"javascript:boing('/sections/member/inventory/index.aspx?a=get&tab=G&id={part_no}', 'inventory', 1280,768 )\" data-allowed_to_stock='{part_allowed_to_stock}' data-is_exclude='{part_is_exclude}' class='part_link'>{part_no}</a>";
					}
					else if (linetype==OpsWOLineType.Labor)
					{
						e.Cell.ToolTip = grid.GetRowValues(e.VisibleIndex, GVColumns.MembertypeName) != DBNull.Value 
											? grid.GetRowValues(e.VisibleIndex, GVColumns.MembertypeName).ToString() 
											: "";
					}
				}
				catch (Exception ee)
				{
					Toolbox.do_errorLog(ee, "HtmlDataCellPrepared - Master ID Block");
				}
				break;
			#endregion part_no
			#region qty_rec
			case GVColumns.QtyCommittedToDate:
				e.Cell.Text = $"<span class='qtyrec'>" + Toolbox.ReturnBlankIfNull_string(e.CellValue) + "</span>";
				break;
			#endregion qty_avail
			#region qty_avail
			case GVColumns.AvailableQuantity:
				e.Cell.CssClass += " qty_avail";
				try
				{
					workorder_column = (GridViewDataColumn)agv.Columns[GVColumns.Location];
					location_combo = (ASPxComboBox)agv.FindRowCellTemplateControl(e.VisibleIndex, workorder_column, "combo_location");
					var mid = Convert.ToInt32(agv.GetRowValues(e.VisibleIndex, GVColumns.MasterId));
					if (location_combo != null && location_combo.Visible && location_combo.SelectedIndex > -1)
					{
						// Get available qty from that location
						var available_qty = Toolbox.doSQL_double(@"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND master_id = @v1  AND business_unit_id = @v2 ", new[] { location_combo.Value, mid, WarehouseBusinessUnit.id });
						e.Cell.Text = available_qty.ToString();
					}
					else
					{
						e.Cell.Text = "0";
					}
					if (mid < OpsSpecialPart.LaborThreshold && mid != OpsSpecialPart.QuoteLine && 
					   (mid != OpsSpecialPart.SubContractor && mid != OpsSpecialPart.CompanyCreditCardExpense && mid != OpsSpecialPart.ExpenseReimbursement && mid != OpsSpecialPart.NewChildWO && mid != OpsSpecialPart.PerDiem) 
						&& mid != OpsSpecialPart.MakeThisPart && location_combo != null && location_combo.Visible && location_combo.SelectedIndex > -1)
					{
						double.TryParse(e.Cell.Text, out var n);
						e.Cell.ForeColor = n > 0 ? Color.Black : n < 0 ? Color.Red : Color.DarkGray;
						e.Cell.BackColor = n > 0 
											? Color.FromArgb(0, 255, 0) 
											: e.Cell.BackColor;
					}
					else
					{
						e.Cell.Text = "";
					}
					e.Cell.Font.Bold = true;
					e.Cell.Style.Add("text-align", "center");
				}
				catch (Exception ee)
				{
					//e.Cell.Text	 = ee.ToString();
				}
				break;
			#endregion qty_avail
			#region active
			case GVColumns.Margin:
				var SeeMargin = true;
				if(linetype == OpsWOLineType.Labor)
					{
					var memberid	= (int) grid.GetDataRow(e.VisibleIndex)[GVColumns.MemberId];
					var employee	= new NeMember(memberid);
					SeeMargin		= member_can_see_labour_cost && (memberid == current_user.id 									 // If it's their own labor line, show the margin
										|| ReportsToList.Contains(memberid) 														 // If the employee is subservient, show the margin
										|| current_user.id == wo.intProjectManager 													 // If the current user is the project manager of the work order 
											&& current_user.MemberTypeID != employee.MemberTypeID 									 // AND They don't share a member type id (project managers on the same job) 
											&& current_user.membertype.reports_to != employee.membertype.reports_to 				 // AND They don't share a reports to (project manager / automation project manager)
											&& !NeMemberType.is_supervisor_mt(current_user.MemberTypeID, employee.MemberTypeID));	 // AND The employee isn't in the hierarchy above the project manager (PM / BM)
					}
				e.Cell.Text = grid.GetRowValues(e.VisibleIndex, GVColumns.Margin) == null || !SeeMargin
								? "" 
								: grid.GetRowValues(e.VisibleIndex, GVColumns.Margin).ToString();
			break;
				#endregion active
		}
		switch (e.DataColumn.Name)
		{
			#region Notes 
			case GVColumns.Notes:
				if (linetype == OpsWOLineType.Labor && current_user.business_unit.summarize_labor && issummarize_labor)
				{
					e.Cell.Text = "";// its no longer a server side control, which is why we are emptying it
				}         
				break;
			#endregion
			#region select
			case GVColumns.Select:
				string part_origin = agv.GetDataRow(e.VisibleIndex)["origin"].ToString();
				var cb1 = (CheckBox)grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chk_indiv");
				if (cb1 != null)
				{
					cb1.Visible = part_no != OpsSpecialPart.QuoteLine && !(part_no >= OpsSpecialPart.LaborThreshold && part_origin.Contains("Timesheet"));
					if (dt_excludes != null)
					{
						var dr = dt_excludes.Select("master_id = '" + part_no + "'");
						if (dr.Length > 0 && dr[0].ItemArray.Length > 0 && dr[0]["is_exclude"].ToString() == "1" )
						{
							cb1.Visible = false;
						}
					}

				   

					//
					// Code check here
					//

					//
					// If checking the line of code 610-ish you will find below one, that means rebill and credit is not allowed to transfer.
					//
					//         agv.Columns["Trans"].Visible = wo.woprog_isrebill == 0 && wo.woprog_iscredit == 0;
					//
					if (cb1.Visible == true && (wo.woprog_isrebill == 0 && wo.woprog_iscredit == 0) )
					{
						//
						// �	If the PO is closed, the item cannot be transferred; a negative Purchase Order is required, so the used should be warned/informed at this point.
						//
						cb1.Visible = IsTransferAvailableIfPoBehindIt(main_id, part_origin, part_no);
					}
					if (isImportedWIP || isImportedCOGS || partiallyBilled)
					{
						cb1.Visible = false;
					}
				}
				break;
				#endregion
		}


		//Debug.Write(currentMethodName.Name+" "+DateTime.Now.ToString("U")+" End\n");
	}
	

	protected void agv_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if(e.VisibleIndex < 0 || issummarize_labor)
			{
			e.Visible = DefaultBoolean.False;
			return;
			}
		var origin = (string) gv.GetDataRow(e.VisibleIndex)[GVColumns.Origin];
		var isPartiallyBilled = Toolbox.ReturnZeroIfNull_int(gv.GetDataRow(e.VisibleIndex)[GVColumns.PartiallyBilled]) == 1;
		switch (e.ButtonID)
			{
			case "delete_multiple":
				var rec_qty = Convert.ToDouble(gv.GetRowValues(e.VisibleIndex, GVColumns.QtyCommittedToDate));
				var id_master = Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, GVColumns.MasterId));
				if (rec_qty != 0 || id_master == OpsSpecialPart.QuoteLine)
					{
					e.Visible = DefaultBoolean.False;
					}
				else
					{
					e.Image.Height = Unit.Pixel(0);
					e.Image.Width = Unit.Pixel(0);
					e.Image.AlternateText = "delete_multiple";
					}
			break;
			case "partially_billed": 
				e.Visible = WorkingBusinessUnit.AllowPartialBilling && origin != OpsWOLineOrigin.ImportedCOGS ? DefaultBoolean.True : DefaultBoolean.False;
				if(e.Visible == DefaultBoolean.False)
					return;

				e.Enabled = CanTogglePartialBillLines;
				e.Image.Url = isPartiallyBilled 
								? "~/images/icon/checkbox_checked.png" 
								: "~/images/icon/checkbox.png";
			break;
			}
	}
	protected void agv_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (e.VisibleIndex == -1)
			{
			return;
			}
		var dataRow = gv.GetDataRow(e.VisibleIndex);
		var partiallyBilled = dataRow != null && Toolbox.ReturnZeroIfNull_int(dataRow[GVColumns.PartiallyBilled]) == 1;
		var lineType = gv.GetRowValues(e.VisibleIndex, GVColumns.LineType).ToString();
		var part_no = Toolbox.ReturnZeroIfNull_int(gv.GetRowValues(e.VisibleIndex, GVColumns.MasterId));
		// var cost = gv.GetRowValues(e.VisibleIndex, "cost").ToString();
		if ((lineType == OpsWOLineType.Labor || lineType == OpsWOLineType.Mileage) && WorkingBusinessUnit.summarize_labor && issummarize_labor)
			{
			e.Visible = false;
			return;
			}

		if (lineType == OpsWOLineType.Asset)
			{
			e.Visible = false;
			return;
			}
		if(partiallyBilled)
			{
			e.Visible = false;
			return;
			}
	e.Visible = true;

		var origin = gv.GetRowValues(e.VisibleIndex, GVColumns.Origin).ToString();
		var isImportedWIP = origin.Contains(OpsWOLineOrigin.ImportedWIP);
		var isImportedCOGS = origin.Contains(OpsWOLineOrigin.ImportedCOGS);
		if (e.ButtonType == ColumnCommandButtonType.Delete)
			{
			var committed_value = Convert.ToDouble(gv.GetRowValues(e.VisibleIndex, GVColumns.QtyCommittedToDate));
			var has_committed_parts = committed_value > 0;
			var is_expense_line = origin == OpsWOLineOrigin.ExpenseReimbursement || origin == OpsWOLineOrigin.PerDiemExpense;
			var is_credit_card = origin == OpsWOLineOrigin.CompanyCreditCardExpense;
			var isTs = origin == OpsWOLineOrigin.EnteredFromTimesheet;
			//Is it a Progress billing/Downpayment line?
			if(lineType == OpsWOLineType.Quote)
			{
				e.Visible = false;
				return;
			}
			if(isTs || isImportedCOGS || isImportedWIP)
				{
				e.Visible = false;
				return;
				}
			if (has_committed_parts || is_expense_line || is_credit_card)
				{
				e.Image.Url = "/images/icon/icon[delete-grey].gif";
				e.Enabled = false;
				e.Image.ToolTip = is_expense_line
									? "Cannot delete an expense line. Transfer it, or change the billtype"
									: is_credit_card
										? "Cannot delete a credit card line"
										: "Cannot delete lines with committed parts";
				}
			}

		if (e.ButtonType != ColumnCommandButtonType.Edit) return;
		if (!member_can_edit_sell)
			{
			if (part_no > 0)
				{
				if (part_no >= OpsSpecialPart.LaborThreshold || isImportedCOGS)
					{
					e.Visible = false;
					}
				}
			else
				{
				e.Visible = false;
				}
			}
		else if (isImportedCOGS)
			{
			e.Visible = false;
			}
		}
	#endregion
	DateTime ts_update;

	double cost;
	double sell;

	protected void agv_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
	{


	}

	protected void agv_OnSummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
	{
		var field_name = e.Item.FieldName;
		if (e.IsTotalSummary && member_can_see_cost && member_can_see_labour_cost)
		{
			double total = 0;
			double total2 = 0;
			for (var i = 0; i < agv.VisibleRowCount; i++)
			{
				total += Convert.ToDouble(agv.GetRowValues(i,GVColumns.Cost)) * Convert.ToDouble(agv.GetRowValues(i, GVColumns.QtyCommittedToDate));
				total2 += Convert.ToDouble(agv.GetRowValues(i, GVColumns.Sell)) * Convert.ToDouble(agv.GetRowValues(i, GVColumns.QtyCommittedToDate));

			}


			if (field_name == GVColumns.Cost)
			{
				e.Text = $"{total:C}";
			}
			else if (field_name == GVColumns.Sell)
			{
				e.Text = $"{total2:C}";
			}

		}
		else
		{
			if (field_name == GVColumns.Cost)
			{
				e.Text = "";
			}
			else if (field_name == GVColumns.Sell)
			{
				e.Text = "";
			}
		}



	}


	/// <summary>
	/// Updates the work order totals and labels for various billing types, such as regular, VNC, blended, ICR, DNI, and VC.
	/// This method calculates the totals for labor and material costs and displays them in the corresponding labels.
	/// The method also checks if the work order status is "Invoiced" or "WaitingToBeInvoiced" to determine the table type.
	/// </summary>
	protected void UpdateWorkOrderTotals(int linewoid, string quoteid, string erid)
	{
		if (ts_update == null || ts_update.Year == 1)
		{
			double AllItemTotals = 0;
			double AllLabItemTotals = 0;
			double AllMatItemsTotals = 0;
			double itemtotals = 0;
			double itemlabtotals = 0;
			double itemmattotals = 0;
			var status = "";
			try
			{
				status = Toolbox.doSQL_string(@"SELECT woprog_status FROM woprog WHERE woprog_id = @v0 ", new object[] { linewoid });
			}
			catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
			var tabletype = status != OpsWOStatus.Invoiced && status != OpsWOStatus.WaitingToBeInvoiced ? "" : "h";

			var SQLBase = @"
SELECT 
	IFNULL(round(SUM(round(qty_committed * price_sell,5) * (1 - (discount/100))),5), 0) AS Linetotal 
FROM 
	wo_detail{0} 
WHERE 
	woprog_id = {1} AND 
	(type {2}= 'L' {5} code {3} LIKE 'LB%' {5} master_id {4} 990000) AND 
	billtypeid ={6}";
			if (quoteid == "0" && erid == "0")
			{
				try { itemlabtotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "0"), null); }
				catch { itemlabtotals = 0; }
				try { itemmattotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "0"), null); }
				catch { itemmattotals = 0; }
				lblRegularLabour2.Text = itemlabtotals.ToString("C2");
				lblRegularMaterial2.Text = itemmattotals.ToString("C2");
				itemtotals = itemlabtotals + itemmattotals;
				AllItemTotals += itemtotals;
				AllLabItemTotals += itemlabtotals;
				AllMatItemsTotals += itemmattotals;
				lblRegularTotal2.Text = itemtotals.ToString("C2");

				try { itemlabtotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "7"), null); }
				catch { itemlabtotals = 0; }
				try { itemmattotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "7"), null); }
				catch { itemmattotals = 0; }
				lblVNCLabour2.Text = itemlabtotals.ToString("C2");
				lblVNCMaterial2.Text = itemmattotals.ToString("C2");
				itemtotals = itemlabtotals + itemmattotals;
				AllItemTotals += itemtotals;
				AllLabItemTotals += itemlabtotals;
				AllMatItemsTotals += itemmattotals;
				lblVNCTotal2.Text = itemtotals.ToString("C2");


				try { itemlabtotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "4"), null); }
				catch { itemlabtotals = 0; }
				try { itemmattotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "4"), null); }
				catch { itemmattotals = 0; }
				lblBlendedLabour2.Text = itemlabtotals.ToString("C2");
				lblBlendedMaterial2.Text = itemmattotals.ToString("C2");
				itemtotals = itemlabtotals + itemmattotals;
				AllItemTotals += itemtotals;
				AllLabItemTotals += itemlabtotals;
				AllMatItemsTotals += itemmattotals;
				lblBlendedTotal2.Text = itemtotals.ToString("C2");


				try { itemlabtotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "2"), null); }
				catch { itemlabtotals = 0; }
				try { itemmattotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "2"), null); }
				catch { itemmattotals = 0; }
				lblICRLabour2.Text = itemlabtotals.ToString("C2");
				lblICRMaterial2.Text = itemmattotals.ToString("C2");
				itemtotals = itemlabtotals + itemmattotals;
				AllItemTotals += itemtotals;
				AllLabItemTotals += itemlabtotals;
				AllMatItemsTotals += itemmattotals;
				lblICRTotal2.Text = itemtotals.ToString("C2");

				try { itemlabtotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "5"), null); }
				catch { itemlabtotals = 0; }
				try { itemmattotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "5"), null); }
				catch { itemmattotals = 0; }
				lblDNILabour2.Text = itemlabtotals.ToString("C2");
				lblDNIMaterial2.Text = itemmattotals.ToString("C2");
				itemtotals = itemlabtotals + itemmattotals;
				AllItemTotals += itemtotals;
				AllLabItemTotals += itemlabtotals;
				AllMatItemsTotals += itemmattotals;
				lblDNITotal2.Text = itemtotals.ToString("C2");

				try { itemlabtotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "10"), null); }
				catch { itemlabtotals = 0; }
				try { itemmattotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "10"), null); }
				catch { itemmattotals = 0; }
				lblVCLabour2.Text = itemlabtotals.ToString("C2");
				lblVCMaterial2.Text = itemmattotals.ToString("C2");
				itemtotals = itemlabtotals + itemmattotals;
				AllItemTotals += itemtotals;
				AllLabItemTotals += itemlabtotals;
				AllMatItemsTotals += itemmattotals;
				lblVCTotal2.Text = itemtotals.ToString("C2");

			}
			else
			{
				try { itemlabtotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "1"), null); }
				catch { itemlabtotals = 0; }
				try { itemmattotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "1"), null); }
				catch { itemmattotals = 0; }
				lblRegularLabour2.Text = itemlabtotals.ToString("C2");
				lblRegularMaterial2.Text = itemmattotals.ToString("C2");
				itemtotals = itemlabtotals + itemmattotals;
				AllItemTotals += itemtotals;
				AllLabItemTotals += itemlabtotals;
				AllMatItemsTotals += itemmattotals;
				lblRegularTotal2.Text = itemtotals.ToString("C2");

				try { itemlabtotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "", "", ">=", "OR", "0"), null); }
				catch { itemlabtotals = 0; }
				try { itemmattotals = Toolbox.doSQL_double(string.Format(SQLBase, tabletype, linewoid, "!", "NOT", "<", "AND", "0"), null); }
				catch { itemmattotals = 0; }
				lblVNCLabour2.Text = itemlabtotals.ToString("C2");
				lblVNCMaterial2.Text = itemmattotals.ToString("C2");
				itemtotals = itemlabtotals + itemmattotals;
				AllItemTotals += itemtotals;
				AllLabItemTotals += itemlabtotals;
				AllMatItemsTotals += itemmattotals;
				lblVNCTotal2.Text = itemtotals.ToString("C2");

			}
			lblLabour2.Text = AllLabItemTotals.ToString("C2");
			lblMaterial2.Text = AllMatItemsTotals.ToString("C2");
			lblTotal2.Text = AllItemTotals.ToString("C2");
			// st("update wo totals end");
			ts_update = DateTime.Now;
		}
	}
	protected void agv_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
	{
		FillForWo(main_id);
	}
	#region Handler Code
	protected string note_handler(object container)
	{
		var _q = Request.QueryString;
		var origin = _q["origin"] ?? "";
		var c = container as GridViewDataItemTemplateContainer;
		var label_text = "";
		var row_id = c != null ? c.KeyValue.ToString() : "";
		var _c = 0;
		var _n = "";
		if (!string.IsNullOrEmpty(row_id) && c != null)
		{
			try
			{
				_c = _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id + " AND notes <> ''").Length;
			}
			catch
			{
				_c = 0;
			}
			_n = _c > 0 ? _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id)[0]["notes"].ToString() : "";
		}
		var div = _c > 0 ? string.Format(@" class='opt' data-tooltip=""{0}"" data-width='450' data-title='Notes'", Toolbox.do_value_from(_n)) : "";
		label_text = _c > 0 ? "<img src='/images/fullnotes.jpg' border='0' " + div + "/>" : "<img src='/images/icon/icon[note_blank].gif' border='0' Width='16' Height='16'/>";
		return label_text;
	}
	protected string issue_handler(object container)
	{
		var _q = Request.QueryString;
		//		Toolbox _tools = new Toolbox();
		var origin = _q["origin"] ?? "";
		var c = container as GridViewDataItemTemplateContainer;
		var label_text = "";
		var row_id = c.KeyValue.ToString();
		var issue = "";
		var type = "";
		var div = "";
		var _c = 0;
		if (row_id != "" && row_id != null)
		{
			try
			{
				_c = _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id + " AND issues <> ''").Length;
				issue = _c > 0 ? _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id)[0]["issues"].ToString().Replace("\n", "<br/>") : "";  
			}
			catch
			{
			}
		}
		div = string.Format(@" class='opt' data-tooltip=""{0}"" data-width='500' data-title='Issues'", issue);
		var dataRows = _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id);
		if (!current_user.business_unit.summarize_labor || (current_user.business_unit.summarize_labor && !issummarize_labor) || (current_user.business_unit.summarize_labor && issummarize_labor && dataRows.Length > 0 && dataRows[0]["type"].ToString()!=OpsWOLineType.Labor))
		label_text = issue != "" ? "<img src='/images/icon/icon[attention].gif' width='16' height='16' border='0' " + div + "/>" : "<img src='/images/icon/icon[approve].gif' width='16' height='16' border='0'/>";
		return label_text;
	}
	protected string po_stock_wo_track(object container)
	{
		var label_text = "";
		var _q = Request.QueryString;
		var origin = _q["origin"] ?? "";
		var c = container as GridViewDataItemTemplateContainer;
		var row_id = c.KeyValue.ToString();
		var div = "";
		var WorkOrders = new DataRow[1];
		var tooltip = "";
		if (WorkOrders.Length > 0 && WorkOrders[0] != null)
		{
			foreach (var WO in WorkOrders)
			{
				tooltip += string.Format(@"<tr><td>{0}</td><td>{3}</td><td>{1}</td><td>{2}</td></tr>",
									WO["wo_detail_current_bvwo"],
									WO["wo_detail_current_qty_ordered"],
									WO["wo_detail_current_qty_committed"],
									WO["customer"]);
			}
			if (tooltip != "")
			{
				tooltip = string.Format(@"<table width='100%'><thead><tr><th>WO</th><th>Customer</th><th>Req</th><th>Comm</th></thead></tr><tbody>{0}</tbody></table>", tooltip);
			}
			div = string.Format(@" class='opt' data-tooltip=""{0}"" data-width='500' data-title='Requiring This Part'", tooltip);
		}
		label_text = tooltip != "" ? "<img src='/images/icon/icon[details].gif' width='16' height='16' border='0' " + div + " />" : "";
		return label_text;
	}
	DataTable dt_history;
	protected string history_handler(object container)
	{
		//StackTrace st = new StackTrace ();
		//StackFrame sf = st.GetFrame (0);
		//MethodBase currentMethodName = sf.GetMethod();
		//Debug.Write(currentMethodName.Name+" "+DateTime.Now.ToString("U")+" Start\n");
		var _q = Request.QueryString;
		var origin = _q["origin"] ?? "";
		var c = container as GridViewDataItemTemplateContainer;
		var label_text = "";
		var row_id = c.KeyValue.ToString();
		var div = "";
		var sb = new StringBuilder();
		if (dt_history == null && main_id != 0)
		{
			dt_history = Toolbox.doSQL_dt(string.Format(@"SELECT id, rec_no, master_id, woprog_id,type FROM wo_detail{0} WHERE woprog_id = @v0", TableSuffix), new object[] { main_id });
		}
		if (row_id != "" && row_id != null && dt_history != null && dt_history.Rows.Count > 0)
		{
			try
			{
				var this_dr = dt_history.Select("id = " + row_id);
				if (this_dr != null && this_dr.Length > 0)
				{
					var recno = this_dr[0]["rec_no"].ToString();
					var currentUserSummarizeLabor = current_user.business_unit.summarize_labor;
					var lineType = Toolbox.ReturnBlankIfNull_string(this_dr[0]["type"]);
					// The old procedure is the direct call to wo_line_changes
					//DataTable _history_dt = Toolbox.doSQL_dt(@"CALL wo_line_changes(@v0 , @v1 )", new object[] {  histwoid, recno } );
					if (_handlers.Tables["history"] != null && _handlers.Tables["history"].Select("rec_no =" + recno).Length > 0)
					{
						var _history_dt = _handlers.Tables["history"].Select("rec_no =" + recno).CopyToDataTable();
						if (_history_dt.Rows.Count > 0)
						{
							sb.Append("<table cellspacing='0' cellpadding='3'>");
							foreach (DataRow _history_dr in _history_dt.Rows)
							{
								var _dt = _history_dr["dt"].ToString();
								var _the_change = HttpUtility.HtmlEncode(_history_dr["the_change"].ToString());
								sb.AppendFormat(@"	<tr>
														<td valign='top' width='125'><strong>{0}</strong></td>
														<td valign='top' style='text-align:left'>{1}</td>
													</tr>", _dt, _the_change);
							}
							sb.Append("</table>");
						}
						else
						{
							sb.Append("");
						}
						if (sb.ToString() != "")
						{
							div = $@" class='opt' data-tooltip=""{sb}"" data-width='500' data-title='History'";
						}
					}
					if(lineType != OpsWOLineType.Labor && (!currentUserSummarizeLabor || !issummarize_labor || issummarize_labor ))
						   label_text = sb.ToString() != "" 
								? "<img src='/images/icon/icon[book_mark].png' width='16' height='16' border='0'" + div + " />" 
								: "<img src='/images/icon/icon[book_empty].png' width='16' height='16' border='0'/>";
				}
			}
			catch
			{
			}
		   
		}
	   
		//Debug.Write(currentMethodName.Name+" "+DateTime.Now.ToString("U")+" End\n");
		return label_text;

	}
	protected string origin_handler(object container)
	{
		var _q = Request.QueryString;
		var c = container as GridViewDataItemTemplateContainer;
		var label_text = "";
		var row_id = c.KeyValue.ToString();
		var _woprog_id = "";
		var div = "";
		var _c = 0;
		double _on_order = 0;
		var _pos = "";
		var _origin = "";
		var _partno = "";
		if (row_id != "" && row_id != null)
		{
			_c = _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id + " AND origin <> ''").Length;
			_origin = _c > 0 ? _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id)[0]["origin"].ToString().Replace("\n", "<br/>") : "";
			_partno = _c > 0 ? _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id)[0]["master_id"].ToString() : "";
			_woprog_id = _c > 0 ? _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id)[0]["woprog_id"].ToString() : "";
			_on_order = _c > 0 ? Convert.ToDouble(_handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id)[0]["qty_on_order"]) : 0;
			_pos = _on_order > 0 ? _handlers.Tables["wo_detail" + TableSuffix].Select("id = " + row_id)[0]["pos"].ToString() : "";
		}
		if (_on_order > 0)
		{
			div = string.Format(@" class='opt' data-tooltip=""{0}"" data-width='500' data-title='Origin' ", _origin + "</br>" + _on_order + " parts still waiting to be received on PO(s) - " + _pos);
			label_text = "<img src='/images/icon/icon[tags].gif' width='16' height='16' border='0' " + div + " />";
		}
		
		else if (_origin != "")
		{
			div = string.Format(@" class='opt' data-tooltip=""{0}"" data-width='500' data-title='Origin'", _origin);
			if (_origin == "Manually Added")
			{
				label_text = "<img src='/images/icon/icon[browse].gif' width='16' height='16' border='0' " + div + " />";
			}
			else if(_origin.Contains("Import"))
				{
				label_text = "<img src='/images/icon/icon[lock].gif' width='16' height='16' border='0' " + div + " />";
				}
			else if (_origin == "Scanner Added")
			{
				label_text = "<img src='/images/icon/icon[print_barcode].GIF' width='16' height='16' border='0'" + div + " />";
			}
			else if (_origin == "Entered From Timesheet")
			{
				label_text = "<img src='/images/icon/icon[calendar].gif' width='16' height='16' border='0'" + div + " />";
			}
			else if (_origin.Contains("PO"))
			{
				// SubContractor            = 55555;
				var poprog_id = 0;
				if (_partno == OpsSpecialPart.SubContractor.ToString())
				{
					// check to see if a PO references this work order
					var _po_c = Toolbox.doSQL_int(@"SELECT COUNT(DISTINCT(po_details_poprog_id)) FROM po_details_current WHERE po_details_woprog_id = @v0  AND po_details_part_no = '55555' and po_details_current.is_gl_account=false", new object[] { _woprog_id });
					if (_po_c == 1)
					{
						poprog_id = Toolbox.doSQL_int(@"SELECT po_details_poprog_id FROM po_details_current WHERE po_details_woprog_id = @v0  AND po_details_part_no = '55555' and po_details_current.is_gl_account = false", new object[] { _woprog_id });
					}
				}
				label_text = poprog_id > 1
								? string.Format(@"<a href=""javascript:boing('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={1}', 'po', 1280,800)""><img src='/images/icon/icon[shipping].gif' width='16' height='16' border='0'{0} /></a>", div, poprog_id)
								: "<img src='/images/icon/icon[shipping].gif' width='16' height='16' border='0'" + div + " />";
			}
			else if (_origin.Contains("Transferred"))
			{
				label_text = "<img src='/images/icon/icon[left].gif' width='16' height='16' border='0'" + div + " />";
			}

			else
			{
				label_text = "<img src='/images/icon/icon[details].gif' width='16' height='16' border='0'" + div + " />";
			}
		}
		else
		{
			label_text = "<img src='/images/icon/icon[details].gif' width='16' height='16' border='0'/>";
		}
		//label_text = _c > 0 ? "<img src='/images/fullnotes.jpg' border='0'/>" : "<img src='/images/emptynotes.jpg' border='0'/>";
		return label_text;
	}
	#endregion

	protected void cbAddlineGroup_SelectedIndexChanged(object sender, EventArgs e)
		{
		var part_list = Toolbox.doSQL_string(@"SELECT IFNULL(GROUP_CONCAT(DISTINCT master_id), '010') FROM wo_detail  WHERE woprog_id =@v0", new object[] { id });
		var query = $"Select id AS groupselectid, master_id, urldecode(full_part_description_with_labour(master_id,false,'{WarehouseBusinessUnit.country}')) as description, null as Qty, 0 as RepairID, null as chk from Inventory_group_dtl where group_id = {cbAddlineGroup.Value} and master_id<990000 AND master_id NOT IN ({part_list}) ORDER BY id ASC";
		
		sds_groupselect.SelectCommand           = query;
		ViewState["sds_groupselect_datasource"] = query;
		Grid_GroupSelect.DataBind();
		if (Grid_GroupSelect.VisibleRowCount > 0)
			{
			lblerrorLabel.Text               = "";
			Popup_GroupSelect.HeaderText     = "Group Listing";
			Popup_GroupSelect.ShowOnPageLoad = true;
			}
		else if (ddlMatType.SelectedValue != "3")
			{
			ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "alert", "alert('There are no usable parts');", true);
			}
		}
	protected void cbAddlineQuote_SelectedIndexChanged(object sender, EventArgs e)
		{
		var query = $"CALL ds_quoteparts({cbAddlineQuote.Value.ToString().Remove(6)}, {cbAddlineQuote.Value.ToString().Substring(6, 1)})";
		
		sds_groupselect.SelectCommand           = query;
		ViewState["sds_groupselect_datasource"] = query;
		Grid_GroupSelect.DataBind();
		try { Grid_GroupSelect.Columns["RepairID"].Visible = true; }
		catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }
		var date = (ASPxDateEdit)Popup_GroupSelect.FindControl("date_required");
		((Label)Popup_GroupSelect.FindControl("lb_required")).Visible = true;
		date.Visible                                                  = true;
		date.Date                                                     = DateTime.Now.AddDays(10);
		if (Grid_GroupSelect.VisibleRowCount > 0)
			{
			lblerrorLabel.Text               = "";
			Popup_GroupSelect.HeaderText     = "Quote Worksheet Listing";
			Popup_GroupSelect.ShowOnPageLoad = true;
			}
		else
			{
			ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "alert", "alert('There are no usable parts');", true);
			}
		}
	protected void cbAddlineCustomer_SelectedIndexChanged(object sender, EventArgs e)
		{
		var customerId = (int)((ASPxComboBox)sender).Value;
		Populate_OtherWorkOrderSelectDDL(customerId);
		}
	protected void cbAddlineWorkOrder_SelectedIndexChanged(object sender, EventArgs e)
	{
	var is_invoiced = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE woprog_id = @v0  AND woprog_status in ('Invoiced', 'Waiting to be Invoiced')", new[] { cbAddlineWorkOrder.Value }) > 0;
	var table_name  = is_invoiced ? "h" : "";
	var query = $@"
SELECT 
	a.id AS groupselectid,
	a.master_id AS master_id,
	a.description AS description,
	a.qty_committed AS Qty,
	0 RepairID 
FROM
	wo_detail{table_name} a
INNER JOIN 
	inventory_item_master b ON a.master_id = b.master_id 
INNER JOIN 
	inventory_tag c ON b.tag_id = c.tag_id 
WHERE 
	a.woprog_id = {cbAddlineWorkOrder.Value} AND 
	a.master_id < 990000 AND 
	c.active = '1' AND 
	c.tag_id <> 571 AND 
	IFNULL(a.master_id, 0) NOT IN (0, 2139)";
		sds_groupselect.SelectCommand = query;
		ViewState["sds_groupselect_datasource"] = query;
		Grid_GroupSelect.DataBind();
		if (Grid_GroupSelect.VisibleRowCount > 0)
		{
			lblerrorLabel.Text = "";
			Popup_GroupSelect.HeaderText = "Work Order Parts Listing";
			Popup_GroupSelect.ShowOnPageLoad = true;
		}
		else 
		{
			ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "alert", "alert('There are no usable parts');", true);
		}
	}


	Dictionary<string, inventory> inv_i;
	protected void popup_addpart(object sender, EventArgs e)
	{
		try
		{
			var errormsg = "";
			var fields = Grid_GroupSelect.Columns["RepairID"].Visible ?
				new[] { "chk", "groupselectid", "master_id", GVColumns.QtyRequired, "description", "RepairID" } :
				new[] { "chk", "groupselectid", "master_id", GVColumns.QtyRequired, "description" };
			var list_load = new inventory();
			//			Inventory checkinv = new Inventory();

			var keyvalues = Grid_GroupSelect.GetSelectedFieldValues(fields);
			var part_list = new List<string>();
			foreach (object[] key in keyvalues)
			{
				var partno = key[2].ToString();
				if (partno != "0" && partno != "" && !part_list.Contains(partno))
				{
					part_list.Add(partno);
				}
			}
			//todo build list

			var mergeCheck = Toolbox.doSQL_dt(string.Format(@"SELECT master_id, new_id FROM inventory_item_master where master_id in ({0})", string.Join(",", part_list.ToArray())), null);
			var mergeCheck_all = new DataTable();
			var changeItems = new Dictionary<string, string>();
			foreach (DataRow dr in mergeCheck.Rows)
			{
				var chk_master_id = Convert.ToInt32(dr["master_id"]);
				var chk_new_id = Toolbox.ReturnZeroIfNull_int(dr["new_id"]);
				var new_master_id = chk_master_id;


				while (chk_new_id > 0)
				{
					if (mergeCheck_all.Rows.Count == 0)
					{
						mergeCheck_all = Toolbox.doSQL_dt(@"SELECT master_id, new_id FROM inventory_item_master", null);
					}
					var this_id = mergeCheck_all.Select("master_id = " + new_master_id)[0]["new_id"];
					chk_new_id = this_id is DBNull ? 0 : Convert.ToInt32(this_id);
					if (chk_new_id > 0)
						new_master_id = Convert.ToInt32(this_id);
				}
				var i = part_list.IndexOf(chk_master_id.ToString());
				part_list[i] = new_master_id.ToString();
				changeItems.Add(chk_master_id.ToString(), new_master_id.ToString());

			}
			var part_csv = string.Join(",", part_list);
			list_load.Load(part_csv, WarehouseBusinessUnit.id, true);
			inv_i = new Dictionary<string, inventory>();
			foreach (inventory i in list_load.parts)
			{
				inv_i.Add(i.master_id, i);
			}
			foreach (object[] key in keyvalues)
			{
				var gvcc = (GridViewCommandColumn)Grid_GroupSelect.Columns["chk"];
				var gv = (GridViewDataColumn)Grid_GroupSelect.Columns[GVColumns.QtyRequired];
				var index = Grid_GroupSelect.FindVisibleIndexByKeyValue(key[1]);
				var editor = (TextBox)Grid_GroupSelect.FindRowCellTemplateControl(index, gv, "txtGroupSelectQty");
				string qty = qty = editor != null && editor.Text != "" ? Convert.ToDouble(editor.Text).ToString() : "0";
				var qty_dbl = Convert.ToDouble(qty);
				var lineid = key[1].ToString();
				var partno = key[2].ToString();
				var mattype_id = ddlMatType.SelectedValue;
				if (changeItems.ContainsKey(partno))
				{
					partno = changeItems[partno];
				}

				var description = key[4].ToString();
				if (partno == "0")
					continue;
				var is_exclude = inv_i[partno].is_exclude;
				var consignmentid = Grid_GroupSelect.Columns["RepairID"].Visible ? Convert.ToInt32(key[5]) : 0;
				if (partno != "" && qty_dbl > 0)
				{
					if (Convert.ToInt32(partno) < 900000)
					{
						partno = inv_i[partno].master_id;
					}
					if (consignmentid == 0)
					{
						if (Convert.ToInt32(partno) < OpsSpecialPart.LaborThreshold)
						{
							newpartnumber = inv_i[partno].master_id;
							newReqQuantity = Convert.ToDouble(qty);
							newdescription = description;
							if (mattype_id == "5")
							{
								if (is_exclude)
								{
									var quote_line_dt = Toolbox.doSQL_dt(@"SELECT * FROM quote_worksheet WHERE id = @v0 ", new object[] { lineid });
									var quote_line_dr = quote_line_dt.Rows[0];
									newcost = Convert.ToDouble(quote_line_dr["cost"]);
									neworigsell = wo.use_fixed_material_markup ? newcost * wo.fixed_material_markup : Convert.ToDouble(quote_line_dr["sell"]);

								}
								else
								{
									newcost = Toolbox.doSQL_double(@"Select get_current_cost(@v0 ,@v1 )", new object[] { inv_i[partno].master_id, WarehouseBusinessUnit.id });
									neworigsell = wo.use_fixed_material_markup ? newcost * wo.fixed_material_markup : Toolbox.doSQL_double(@"Select GetSellPrice(get_current_cost(@v0 ,@v1 ),0,true,@v2,@v3 )", new object[] { inv_i[partno].master_id, WarehouseBusinessUnit.id, qty, wo.business_unit_id });
								}
							}
							else
							{
								newcost = Toolbox.doSQL_double(@"Select get_current_cost(@v0 ,@v1 )", new object[] { inv_i[partno].master_id, WarehouseBusinessUnit.id });
								neworigsell = wo.use_fixed_material_markup ? newcost * wo.fixed_material_markup : Toolbox.doSQL_double(@"Select GetSellPrice(get_current_cost(@v0 ,@v1 ),0,true,@v2,@v3 )", new object[] { inv_i[partno].master_id, WarehouseBusinessUnit.id, qty, wo.business_unit_id });
							}
							if (((ASPxDateEdit)Popup_GroupSelect.FindControl("date_required")).Visible)
							{
								newdatereq = Toolbox.MySQL_shortdt(((ASPxDateEdit)Popup_GroupSelect.FindControl("date_required")).Date);
							}
							SaveWorkOrderLine(0, Convert.ToInt32(inv_i[partno].master_id), true, inv_i[partno].is_exclude, false);
						}
					}
					else
					{
						var dt = Toolbox.doSQL_dt(@"Select * from quote_worksheet  where id =@v0", new[] { key[0] });
						var record_number = 0;
						foreach (DataRow dr in dt.Rows)
						{
							var newwoline = new NeWODetailCurrent
							{
								description = dr["description"].ToString(),
								master_id = OpsSpecialPart.QuoteLine,
								code = "QUOTE",
								business_unit_id = wo.business_unit_id,
								qty_ordered = 1,
								qty_committed = 1,
								qty_invoiced = 1,
								cost = .01,
								sell = Convert.ToDouble(dr["extended_per"]),
								unit = Convert.ToDouble(dr["extended_per"]),
								woprog_id = Convert.ToInt32(wo.woprog_id),
								bvwo = Convert.ToInt32(wo.OrderNumber),
								origin = "Automatically Added",
								billtypeid = OpsBillType.QuotedPrice,
								type = OpsWOLineType.Quote,
								consignment_id = Convert.ToInt32(dr["consignment_id"])
							};
							record_number = newwoline.GetLineCount(wo.woprog_id);
							record_number++;
							newwoline.rec_no = record_number;
							try
							{
								newwoline.save(current_user, "/sections/workorder/picklist.aspx.cs - popup_addpart #1", false);
							}
							catch (Exception ex)
							{
								throw new Exception(ex.ToString());
							}
							var consign = new consignment(Convert.ToInt32(dr["consignment_id"]));
							consign.load();
							consign.status = consignment.StatusType.InProcess;
							consign.save();
						}
						record_number++;

					}
				}
			}
			Grid_GroupSelect.Selection.UnselectAll();
			Popup_GroupSelect.ShowOnPageLoad = false;
			if (errormsg != "")
			{
				update_error(errormsg);
			}
			dt_master_locations = null;

		}
		catch (Exception ee)
		{
			Toolbox.do_errorLog_errorStack(ee);
			throw;
		}
		finally
		{
			FillForWo(main_id);
		}
	}
	protected void btnUpdatePORecQty_Click(object sender, EventArgs e)
	{
		mass_receive(sender, e);
	}
	protected void mass_set_date_req(object sender, EventArgs e)
	{
		var start = agv.VisibleStartIndex;
		var end = agv.VisibleRowCount;
		for (var i = start; i < end; i++)
		{
			var keyvalues = agv.GetRowValues(i, agv.KeyFieldName);
			if (keyvalues != null)
			{
				var keyValue = Convert.ToInt32(keyvalues);
				var column_chkbox = (GridViewDataColumn)agv.Columns[GVColumns.Select];
				var chkbox = (CheckBox)agv.FindRowCellTemplateControl(i, column_chkbox, "chk_indiv");
				if (chkbox != null && chkbox.Checked)
				{
					var wodc = new NeWODetailCurrent(keyValue)
						{
						date_required = Toolbox.MySQL_shortdt(mass_date_req.Date),
						qty_committed = 0,
						qty_invoiced = 0,
						qty_ordered = 0
						};
					wodc.save(current_user, "/sections/workorder/picklist.aspx.cs - mass_set_date_req", true);
				}
			}
		}
		mass_date_req.Text = "";
		FillForWo(main_id);
	}
	protected void mass_set_billtype(object sender, EventArgs e)
	{
		var start = agv.VisibleStartIndex;
		var end = agv.VisibleRowCount;
		for (var i = start; i < end; i++)
		{
			var keyvalues = agv.GetRowValues(i, agv.KeyFieldName);
			if (keyvalues != null)
			{
				var keyValue = Convert.ToInt32(keyvalues);
				var column_chkbox = (GridViewDataColumn)agv.Columns[GVColumns.Select];
				var chkbox = (CheckBox)agv.FindRowCellTemplateControl(i, column_chkbox, "chk_indiv");
				if (chkbox != null && chkbox.Checked)
				{
					var wodc = new NeWODetailCurrent(keyValue);
					wodc.billtypeid = Convert.ToInt32(mass_billtype_edit.Value);
					if ((wodc.billtypeid == OpsBillType.InvisibleCredit || wodc.billtypeid == OpsBillType.VisibleCredit) && wodc.qty_committed > 0)
					{
						throw new Exception(string.Format("You cannot set a billtype to a credit with a positive quantity (Part {0}, Rec # {1}).", wodc.master_id, wodc.rec_no));
					}
					wodc.qty_committed = 0;
					wodc.qty_invoiced = 0;
					wodc.qty_ordered = 0;
					wodc.save(current_user, "/sections/workorder/picklist.aspx.cs - mass_set_billtype", true);
				}
			}
		}
		mass_billtype_edit.SelectedIndex = -1;
		FillForWo(main_id);
	}
	protected void mass_receive(object sender, EventArgs e)
	{
		var bb = (ASPxButton)sender;
		var g = agv;
		var column_location = (GridViewDataColumn)g.Columns[GVColumns.Location];
		var column_receive = (GridViewDataColumn)g.Columns[GVColumns.QtyReceiving];
		dt_master_locations = null;
		set_location_ds(g, e);
		try
		{
			#region workorder
			double discountqty = 0;
			var msg = "";
			var newinv = new inventory();
			var details = new NeWODetailCurrent();
			try
			{
				var start = agv.VisibleStartIndex;
				var end = agv.VisibleRowCount;
				for (var i = start; i < end; i++)
				{
					var keyValue = agv.GetRowValues(i, agv.KeyFieldName);
					details = new NeWODetailCurrent(Convert.ToInt32(keyValue));
					var receive_amount = (TextBox)agv.FindRowCellTemplateControl(i, column_receive, "txtRecSelectQty");
					var location = (ASPxComboBox)agv.FindRowCellTemplateControl(i, column_location, "combo_location");
					/* 
					 I realize this is a crude way to get the location value... 
					 There was an issue where the location parts were being committed from couldn't be trusted, the databinding to the location boxes 
					 is quick, but very singular in use. As a result some selected indexes/ values were being nulled out... until there is a better way 
					 this works everytime.

					 -Matt 2016-01-03
					 */
					if (location != null && location.Visible)
					{
						foreach (string ee in Request.Form)
						{
							var n = ee.Replace("ctl00$cphMasterBody$pnl_gv$agv$", "");
							var id = location.ClientID.Replace("_combo", "$combo");
							if (n.EndsWith("combo_location") && id.Contains(n))
							{
								var index = 0;
								var ret_val = Request.Form[ee];
								if (ret_val.Contains(" - "))
								{
									var li = location.Items.FindByText(ret_val);
									if (li != null)
									{
										li.Selected = true;
									}
								}
								else
								{
									if (int.TryParse(ret_val, out index))
									{
										location.SelectedIndex = index;
									}
								}
							}
						}
					}
					qtytoadd = 0;
					if (receive_amount != null)
					{
						double.TryParse(receive_amount.Text, out qtytoadd);
					}
					//continue; // Use this when you don't want the commit to succeed... debugging location stuff.
					if (qtytoadd != 0)
					{
						xfer_comm_recv_select_id = location.Visible && location.Value != null ? Convert.ToInt32(location.Value) : 0;
						if (!details.origin.Contains("From Timesheet"))
						{
							if (newinv.part_exists(details.master_id))
							{
								newinv.Load(details.master_id, WarehouseBusinessUnit.id);
							}
							new_qty_committed = qtytoadd + details.qty_committed;
							old_qty_committed = details.qty_committed;
							discountqty = new_qty_committed - old_qty_committed <= 0 ? 1 : new_qty_committed - old_qty_committed;
							newReqQuantity = 0;
							oldqty = details.qty_ordered;
							newpartnumber = details.master_id.ToString();
							oldpartnumber = details.master_id.ToString();
							newtableid = Convert.ToInt32(keyValue);
							newbilltype = details.billtypeid.ToString();
							oldbilltype = details.billtypeid.ToString();
							newdescription = Toolbox.do_value_from(details.description, false);
							olddescription = Toolbox.do_value_from(details.description, false);
							oldorigsell = details.sell;
							oldcost = details.cost;
							newcost = details.cost;
							newrecno = details.rec_no;
							oldrecno = details.rec_no;
							OldTrack = details.track_part;
							NewTrack = details.track_part;
							neworigsell = wo.use_fixed_material_markup ? details.cost * wo.fixed_material_markup : shared.GetSellPrice(details.cost, 0, newinv.is_qty, new_qty_committed, WorkingBusinessUnit.id32);
							int.TryParse(newinv.master_id, out int master_id);

							try
							{
								SaveWorkOrderLine(0, master_id, false, newinv.is_exclude, true);
							}
							catch (Exception ex3)
							{
								msg += string.Format("Part {0}, Rec no {1} was not updated, please try to manually update it.<div style='font-size:10px;font-weight:normal;'>Error Given: {2}</div>", newpartnumber, newrecno, ex3);
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				throw new Exception("There was an issue trying to update: " + ex2);
			}
			if (msg != "")
			{
				update_error(msg);
			}
			else
			{
				//
				update_error("");
			}
			//return;
			FillForWo(main_id);
			#endregion workorder
		}
		catch (Exception ee)
		{
			Toolbox.do_errorLog_errorStack(ee);
			throw;
		}
		ScriptManager.RegisterStartupScript(this, GetType(), "binder", "bind_tooltips();", true);
		//_tools.debug_note("End ASPxButton1_Click");
	}
	protected void btnXlsxExport_Click(object sender, EventArgs e)
	{
		ASPxGridViewExporter1.FileName = "WO: " + id + " Details";
		ASPxGridViewExporter1.WriteXlsToResponse();


	}

	protected void chkApplyDiscount_CheckedChanged(object sender, EventArgs e)
	{
	}
	protected void TextVendorPartNo_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
	{
		var cb = (ASPxComboBox)sender;
		var costs = new object[cb.Items.Count];
		var qtyper = new object[cb.Items.Count];
		for (var i = 0; i < cb.Items.Count; i++)
		{
			var id = cb.Items[i].Value.ToString();
			var vpr = Toolbox.doSQL_dt(@"SELECT total, qty FROM inventory_price  WHERE id =@v0 limit 1 ", new object[] { id }).Rows[0];
			costs[i] = vpr["total"].ToString();
			qtyper[i] = vpr[GVColumns.QtyRequired].ToString();
		}
		e.Properties["cpCOSTS"] = costs;
		e.Properties["cpQTYPER"] = qtyper;
	}
	protected void rb_list_Init(object sender, EventArgs e)
	{
		var g = agv;
		var pop = (ASPxPopupControl)g.FindTitleTemplateControl("pop_commit");
		var pan = (ASPxPanel)pop.FindControl("panel_pop");
	}
	protected void agv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
	{

		var gv = (ASPxGridView)sender;
		var line_ids = new object[gv.VisibleRowCount];
		for (var i = 0; i < gv.VisibleRowCount; i++)
		{
			var line_id = Convert.ToInt32(gv.GetRowValues(i, GVColumns.LineId));
			line_ids[i] = line_id;
		}
		e.Properties["cp_line_ids"] = line_ids;
	}
	protected void agv_Init(object sender, EventArgs e)
	{
		footer_save_toggle(false, (ASPxGridView)sender);
	}
	protected void gv_xfer_multiple_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		var grid = (ASPxGridView)sender;
		if (e.DataColumn.Name == "location")
		{
			var part_no = Convert.ToInt32(grid.GetRowValues(e.VisibleIndex, "part_n"));
			var location_column = (GridViewDataColumn)grid.Columns[GVColumns.Location];
			var location_combo = (ASPxComboBox)grid.FindRowCellTemplateControl(e.VisibleIndex, location_column, "loc");
			var this_ds = (SqlDataSource)grid.FindRowCellTemplateControl(e.VisibleIndex, location_column, "loc_ds");
			if (auth_new_location)
			{
				this_ds.SelectCommand = string.Format(@"
	SELECT 
		a.id,
		IFNULL(b.qty,0) qty, 
		CONCAT(IF(a.type_id = 1, 'INT - ', 'EXT - '), a.name, ' (QTY: ', IFNULL(b.qty,0), ')') name 
	FROM 
		inventory_location_master a
	LEFT JOIN
		inventory_location b ON
			a.id = b.location_master_id AND
			b.business_unit_id = a.business_unit_id AND
			b.master_id = {1}
	WHERE 
		a.business_unit_id = {0}
	ORDER BY 
		a.type_id ASC, 
		b.qty DESC, 
		b.min DESC",
								WarehouseBusinessUnit.id,
								part_no
								);
			}
			else
			{
				this_ds.SelectCommand = string.Format(@"SELECT b.id,a.qty, CONCAT(IF(b.type_id = 1, 'INT - ', 'EXT - '), IFNULL(b.name,''), ' (QTY: ', IFNULL(a.qty,0), ')') name FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = '{0}' AND a.master_id = '{1}' AND b.name IS NOT NULL ORDER BY b.type_id ASC, a.qty DESC, a.min DESC, a.timestamp DESC", WarehouseBusinessUnit.id, part_no);
			}
			this_ds.DataBind();
			location_combo.SelectedIndex = 0;
		}
	}
	protected void gv_xfer_multiple_Init(object sender, EventArgs e)
	{
		var gv = (ASPxGridView)sender;
		if (Session["picklist_xfer_dt"] != null)
		{
			gv_xfer_multiple.DataSource = (DataTable)Session["picklist_xfer_dt"];
			gv_xfer_multiple.DataBind();
		}
	}
	protected void xfer_cbp_Callback(object sender, CallbackEventArgsBase e)
	{
		if(dt_excludes == null)
		{
			set_location_ds(agv, null);
		}
		if (e.Parameter.Contains("|"))
		{
			// Having a pipe in the parameters means it's posting data back.
			var vars = e.Parameter.Split('|');
			var destination_reason = vars[0];
			var xfer_items = jSON.Deserialize<IList<xfer_array>>(vars[1]);
			var this_sb = new StringBuilder();

			foreach (var xfa in xfer_items)
			{
				if (xfa.qty > 0)
				{
					hidttwoprogid.Value = xfa.id.ToString();
					var this_wo = new NeWOProg();
					if (xfa.use_wo)
					{
						this_wo = new NeWOProg(Convert.ToInt32(xfa.dest_id));
					}
					Session["xfer_master_id"] = xfa.part_id;
					var inventoryItem = new inventory();
					var doTransfer = true;
					var transferDeniedReason = "";
					if(inventoryItem.part_exists(xfa.part_id))
						{ 
						inventoryItem.Load(xfa.part_id, WarehouseBusinessUnit.id);

						Session["xfer_business_unit_id"] = WarehouseBusinessUnit.id;
						var is_ext = auth_new_location
															? !xfa.use_wo && new location_master(xfa.dest_id).type_id == 2
															: !xfa.use_wo && new location_master(new location(xfa.dest_id).location_master_id).type_id == 2;
						xfer_wo_combo.Text = xfa.use_wo ? this_wo.OrderNumber + " " + this_wo.Description + "-" + this_wo.Status : "";
						xfer_internal_combo.Text = auth_new_location
															? !xfa.use_wo && !is_ext ? new location_master(xfa.dest_id).name : ""
															: !xfa.use_wo && !is_ext ? new location_master(new location(xfa.dest_id).location_master_id).name : "";
						xfer_external_combo.Text = auth_new_location
															? !xfa.use_wo && is_ext ? new location_master(xfa.dest_id).name : ""
															: !xfa.use_wo && is_ext ? new location_master(new location(xfa.dest_id).location_master_id).name : "";
						xfer_wo_combo.Value = xfa.use_wo ? xfa.dest_id.ToString() : "";
						xfer_internal_combo.Value = !xfa.use_wo && !is_ext ? xfa.dest_id.ToString() : "";
						xfer_external_combo.Value = !xfa.use_wo && is_ext ? xfa.dest_id.ToString() : "";
						xfer_internal_hid.Value = !xfa.use_wo && !is_ext ? xfa.dest_id.ToString() : "";
						xfer_external_hid.Value = !xfa.use_wo && is_ext ? xfa.dest_id.ToString() : "";
						xfer_wo_qty.Text = xfa.use_wo ? xfa.qty.ToString() : "0";
						xfer_internal_qty.Text = !xfa.use_wo && !is_ext ? xfa.qty.ToString() : "0";
						xfer_external_qty.Text = !xfa.use_wo && is_ext ? xfa.qty.ToString() : "0";
						hidTranQTY.Value = xfa.qty.ToString();
						txtTransferReason.Text = destination_reason;
						if(inventoryItem.Tag.id == OpsSpecialTag.NonStockItem && !xfa.use_wo)
							{
							doTransfer           = false;
							transferDeniedReason = "Inventory non-stock items may not be transferred to stock.";
							}
						}
					else
						{
						doTransfer = false;
						transferDeniedReason = "Part does not exist";
						}
					if(doTransfer)
						{ 
						try
							{
							PartTransfer(xfer_save, null);
							this_sb.Append("<li> Successfully transferred (" + xfa.part_id + ").");
							}
						catch (Exception ee)
							{
							this_sb.Append("<li> Error transferring (" + xfa.part_id + ") - Reason:" + ee);
							Toolbox.do_errorLog_errorStack(ee);
							}
						}
					else
						{
						this_sb.Append($"<li> Could not transfer ({xfa.part_id}) - Reason: {transferDeniedReason}");
						}
				}
			}
			if (this_sb.Length > 0)
			{
				xfer_results.ClientVisible = true;
				lb_xfer_results.Text = "<ul>" + this_sb + "</ul>";
			}
			else
			{
				xfer_multiple.ClientVisible = true;
				xfer_single.ClientVisible = false;
			}
			gv_xfer_multiple.DataSource = (DataTable)Session["picklist_xfer_dt"];
			gv_xfer_multiple.DataBind();
			xfer_wo_qty.ClientVisible = false;
			xfer_internal_qty.ClientVisible = false;
			xfer_external_qty.ClientVisible = false;
			tbl_xfer_qty_wo_hdr.InnerText = "";
			tbl_xfer_qty_int_hdr.InnerText = "";
			tbl_xfer_qty_ext_hdr.InnerText = "";
			xfer_save.ClientVisible = false;
		}
		else
		{
			// Populating the CBP per request.
			Session["xfer_business_unit_id"] = WarehouseBusinessUnit.id;
			Session["xfer_master_id"] = e.Parameter;
			var xfer_items = jSON.Deserialize<IList<xfer_array>>(e.Parameter);
			var type = xfer_items[0].type;
			if (type == OpsWOLineType.Material)
			{
				// This means that there are multiple transfers that are going to happen and to prep the data.
				var xf = new DataTable("r");
				xf.Columns.Add("id", typeof(int));
				xf.Columns.Add("part_n", typeof(int));
				xf.Columns.Add(GVColumns.QtyRequired, typeof(double));
				xf.Columns.Add("origin", typeof(string));
				xfer_multiple.ClientVisible = true;
				xfer_single.ClientVisible = false;
				foreach (var xfa in xfer_items)
				{
					if (/*xfa.part_id < 900000 && */ xfa.part_id != OpsSpecialPart.QuoteLine)
					{
						xf.Rows.Add(xfa.id, xfa.part_id, xfa.qty, xfa.origin);
					}
				}
				Session["picklist_xfer_dt"] = xf;
				gv_xfer_multiple.DataSource = xf;
				gv_xfer_multiple.DataBind();
				xfer_wo_qty.ClientVisible = false;
				xfer_internal_qty.ClientVisible = false;
				xfer_external_qty.ClientVisible = false;
				tbl_xfer_qty_wo_hdr.InnerText = "";
				tbl_xfer_qty_int_hdr.InnerText = "";
				tbl_xfer_qty_ext_hdr.InnerText = "";
				xfer_wo_combo.ClientSideEvents.SelectedIndexChanged = "function(s,e){/*testing*/}";
				xfer_internal_combo.ClientSideEvents.SelectedIndexChanged = "";
				xfer_external_combo.ClientSideEvents.SelectedIndexChanged = "";
				xfer_external_combo.ClientVisible = false;
				xfer_external_lb.ClientVisible = false;
				xfer_internal_lb.ClientVisible = false;
				xfer_internal_combo.ClientVisible = false;
				xfer_save.ClientVisible = true;



				xfer_save.ClientSideEvents.Click = "function(s,e){handle_xfer_save(s,e);}";
				xfer_internal_combo.DataSourceID = "";
				xfer_internal_combo.DataSource = Toolbox.doSQL_dt(@"SELECT id,name FROM inventory_location_master  where name = '0.0.0' AND business_unit_id =@v0", new object[] { WarehouseBusinessUnit.id });
			}
			else
			{
				// This is the handler for the single line transfer
				var xfa = xfer_items[0];
				var m_id = xfa.part_id;
				xfer_single.ClientVisible = true;
				xfer_multiple.ClientVisible = false;

				var dr = dt_excludes.Select("master_id = '" + m_id + "'");


				if (dr.Length > 0 && dr[0].ItemArray.Length > 0 && dr[0]["is_exclude"].ToString() == "1")
				{
					trBranchLocation.Style.Add("display","none");
					trtxtBranchLocation.Style.Add("display","none");
					trInternalBranchLocation.Style.Add("display","none");
					trtxtInternalBranchLocation.Style.Add("display","none");
				}
				else
				{

					trBranchLocation.Style.Add("display","table-row");
					trtxtBranchLocation.Style.Add("display","table-row");
					trInternalBranchLocation.Style.Add("display","table-row");
					trtxtInternalBranchLocation.Style.Add("display","table-row");
				}

				Session["xfer_master_id"] = xfa.part_id;
				Session["xfer_business_unit_id"] = WarehouseBusinessUnit.id;
				hidorigttamt.Value = xfa.qty.ToString();
				hidTTPartNo.Value = xfa.part_id.ToString();
				hidttwoprogid.Value = xfa.id.ToString();
				lblTranPartID.Text = xfa.part_id.ToString();
				lblTranQty.Text = xfa.qty.ToString();
				hidmaxqty.Value = xfa.qty.ToString();
				lblTTOrigin.Text = xfa.origin;
				xfer_save.ClientVisible = true;
				xfer_internal_combo.ClientEnabled = m_id < 900000 && m_id != OpsSpecialPart.QuoteLine;
				xfer_external_combo.ClientEnabled = m_id < 900000 && m_id != OpsSpecialPart.QuoteLine;
				xfer_internal_qty.ClientEnabled = m_id < 900000 && m_id != OpsSpecialPart.QuoteLine;
				xfer_external_qty.ClientEnabled = m_id < 900000 && m_id != OpsSpecialPart.QuoteLine;

				bool canBeSplit = IsLineItemCanBeSplit(xfa.id, xfa.part_id, xfa.origin);
				if (canBeSplit)
				{
					xfer_wo_qty.Text = "0";
				}
				else
				{
					// Non-inventory item
					// Locked the number
					xfer_wo_qty.Text = xfa.qty.ToString();
					xfer_wo_qty.Enabled = false;
				}

				xfer_internal_qty.Text = "0";
				xfer_external_qty.Text = "0";
				xfer_wo_combo.ClientSideEvents.SelectedIndexChanged = @"function(s, e) {	
		var master_id = $(""lblTranPartID"").text() / 1;
	var wo = s.GetValue();
	if(master_id >= 990000 && (wo == 9999999 || wo == 9999998 || wo == 9999997))
		{
		alert(""Cannot transfer labour from/to stock"");
		s.SetSelectedIndex(-1);
		}
}";
				xfer_internal_combo.ClientSideEvents.SelectedIndexChanged = @"function(s, e) {
	var master_id = $(""lblTranPartID"").text() / 1;
	var wo = s.GetValue();
	if(master_id >= 990000 && (wo == 9999999 || wo == 9999998 || wo == 9999997))
		{
		alert(""Cannot transfer labour from/to stock"");
		s.SetSelectedIndex(-1);
		}
	else
		{
		$('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_xfer_internal_hid').val(s.GetValue());
		}
}";
				xfer_external_combo.ClientSideEvents.SelectedIndexChanged = @"function(s, e) {
	var master_id = $(""lblTranPartID"").text() / 1;
	var wo = s.GetValue();
	if(master_id >= 990000 && (wo == 9999999 || wo == 9999998 || wo == 9999997))
		{
		alert(""Cannot transfer labour from/to stock"");
		s.SetSelectedIndex(-1);
		}
	else
		{
		$('#ctl00_cphMasterBody_pop_xfer_xfer_cbp_xfer_external_hid').val(s.GetValue());
		}
}";
				xfer_save.ClientSideEvents.Click = @"function(s, e)
	{
	var exter		= xfer_external_qty.GetValue() == """" ? 0 : xfer_external_qty.GetValue() / 1;
	var inter		= xfer_internal_qty.GetValue() == """" ? 0 : xfer_internal_qty.GetValue() / 1;
	var wo			= xfer_wo_qty.GetValue() == """" ? 0 : xfer_wo_qty.GetValue() / 1;
	var max 		= $(""#ctl00_cphMasterBody_pop_xfer_xfer_cbp_xfer_single_hidmaxqty"").val() / 1;
	if(exter + inter + wo > max)
		{
		e.processOnServer	= false;
		alert(""You are trying to transfer more than exists on line items (""+max+"")."");
		}
	if(exter < 0 || inter < 0 || wo < 0)
		{
		 e.processOnServer	= false;
		alert(""You are trying to transfer a negative amount"");
		}
	if(exter == 0 && inter == 0 && wo == 0)
		{
		 e.processOnServer	= false;
		alert(""You are trying to transfer a quantity of zero"");
		}
	if(	(xfer_wo_combo.GetText() == """" && wo > 0) ||
		(xfer_internal_combo.GetText() == """" && inter > 0) ||
		(xfer_external_combo.GetText() == """" && exter > 0)
		)
		{
		 e.processOnServer	= false;
		alert(""A destination hasn't been selected to transfer to."");
		}
	}";
			}
			xfer_internal_combo.DataBind();
			xfer_external_combo.DataBind();
			xfer_wo_combo.SelectedIndex = -1;
			xfer_internal_combo.SelectedIndex = -1;
			xfer_external_combo.SelectedIndex = -1;
		}
	}
	protected void addline_location_cb(object sender, CallbackEventArgsBase e)
	{
		Session["addline_master_id"] = e.Parameter;
		addline_location.DataBind();
	}
	Color last_color = Color.White;
	protected void combo_location_PreRender(object sender, EventArgs e)
	{

	}
	protected void Grid_GroupSelect_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.Caption == GVColumns.QtyRequired)
		{
			var grid = (ASPxGridView)sender;
			var qty_column = (GridViewDataColumn)grid.Columns[GVColumns.QtyRequired];
			var part_no = grid.GetRowValues(e.VisibleIndex, GVColumns.MasterId).ToString();
			if (part_no == "")
			{
				var tb = (TextBox)grid.FindRowCellTemplateControl(e.VisibleIndex, qty_column, "txtGroupSelectQty");
				if (tb != null)
				{
					tb.Enabled = false;
				}
			}
		}
	}
	protected void Grid_GroupSelect_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
		{
			var grid = (ASPxGridView)sender;
			var part_no = grid.GetRowValues(e.VisibleIndex, "master_id").ToString();
			if (part_no == "")
			{
				e.Enabled = false;
			}
		}
	}
	protected void sds_groupselect_Load(object sender, EventArgs e)
	{
		var sds = (SqlDataSource)sender;
		var ds = ViewState["sds_groupselect_datasource"];
		if (ds != null)
		{
			sds.SelectCommand = ViewState["sds_groupselect_datasource"].ToString();
		}
		else
		{
			ViewState["sds_groupselect_datasource"] = null;
		}
	}

	protected void b_warranty_Click(object _sender, EventArgs _e)
	{
		Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_billtypeid = 7  WHERE wo_detail_current_woprog_id=@v0", new object[] { main_id });

	}

	protected void btnSetItemsToJobCostforQuote_Click(object _sender, EventArgs _e)
	{
		var has_quoted = Toolbox.doSQL_int(@"Select count(wo_detail_current_id) from wo_detail_current  where wo_detail_current_woprog_id=@v0 and wo_detail_current_master_id = 2139", new object[] { main_id });
		if (has_quoted <= 0)
		{
			throw new Exception("The work order must contain a quote line item in order for everything to be set to job cost billtype.");

		}
		Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_billtypeid = 1  WHERE wo_detail_current_billtypeid = 0 AND wo_detail_current_woprog_id=@v0", new object[] { main_id });
		FillForWo(main_id);
	}
	protected void btnSet0qtystoNoCharge_Click(object _sender, EventArgs _e)
	{
		Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_billtypeid = 5  WHERE wo_detail_current_qty_committed = 0 AND wo_detail_current_woprog_id=@v0", new object[] { main_id });
		FillForWo(main_id);
	}

	protected void btnSummarizelabor_Click(object _sender, EventArgs _e)
	{
		if (btn_Summarizelabor.Text== "Summarize labor")
		{
			issummarize_labor = true;
			btn_Summarizelabor.Text = "Expand Labor";
		}
		else
		{
			issummarize_labor = false;
			btn_Summarizelabor.Text = "Summarize labor";
		}
		Session["summarized_lines_" + main_id] = issummarize_labor;
		FillForWo(main_id);
	}

		protected void SetallitemstoDoNotInclude_Click(object _sender, EventArgs _e)
	{
		Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_billtypeid = 5  WHERE wo_detail_current_woprog_id=@v0", new object[] { main_id });
		try
		{
			Toolbox.doSQL_void(@"INSERT INTO event_table (dateoccured,event_text,member_id)  VALUES(now(),'Billing Type Set to Do Not Include for@v0,@v1)", new object[] { main_id, current_user.id });
		}
		catch (Exception ee)
		{
			Toolbox.do_errorLog_errorStack(ee);
		}
		FillForWo(main_id);
	}
	protected void btnSetToRegular_Click(object _sender, EventArgs _e)
	{
		Toolbox.doSQL_void(@"UPDATE wo_detail_current SET wo_detail_current_billtypeid = 0  WHERE wo_detail_current_billtypeid = 1 AND wo_detail_current_woprog_id=@v0", new object[] { main_id });
		FillForWo(main_id);
	}

	private bool IsLineItemCanBeSplit(int id, int part_id, string origion)
	{
		//
		// https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1008/
		// what about non-inventory items that are added to a work order directly, and not through a PO? Should this rule also apply to these lines?
		//
		if ((string.IsNullOrEmpty(origion)) ||
			 (!origion.StartsWith("PO")))
		{
			// Here origion is empty. (it may be 'Manualkly Added' or 'PO 00020619002-4 added:17 2018-1-1')
			// and if it is  not "PO.....", it can be split.
			return true;
		}

		//
		// Query to see whether it is a inventory-item.
		//

		var query = @"
SELECT
 COUNT(*)
FROM
  inventory_item_master a
  INNER JOIN inventory_tag b
	ON a.tag_Id = b.tag_Id
WHERE b.is_subcontractor = FALSE
  AND b.is_shipping = FALSE
  AND b.is_other = FALSE
  AND b.allowed_to_stock = TRUE
  AND a.master_id =@v0";

		// Parameter
		var parameters = new object[] { part_id };

		// check the count.
		var count = Toolbox.doSQL_int(query, parameters);
		if (count > 0)
		{
			// it is inventory item, can be split.
			return true;
		}

	// it is non-inventory itm can't be split.
	return false;
	}


	/// <summary>
	/// This is the entry routine to call in order to split the po item.
	/// Main job is to pass the values.
	/// </summary>
	/// <param name="details"></param>
	/// <param name="transfer_to_woprog_id"></param>
	/// <param name="xfer_master_id"></param>
	/// <param name="xfer_combined_qty"></param>
	/// <param name="qty_remaining"></param>
	/// <returns></returns>
	private bool SplitPOLineItem(NeWODetailCurrent details, int transfer_to_woprog_id, string xfer_master_id, double xfer_combined_qty, double qty_remaining, int wo_line_id, int rec_no_from_new_wo_line_item,
		NeMember current_user, int target_wo_line_id)
	{
		//
		// 1007 [NESI QA] Transfers of PO lines are not updating the original PO.
		// Based on the task, only appling this rule to any wo line items that originally created from a PO or can be traced back from a PO.
		//
		if (details == null ||
			string.IsNullOrEmpty(details.origin) ||
			(!details.origin.Contains("PO")))
		{
			return false;
		}

		// If the wo line item is associated with a po line item.

		//
		// Prepare inputs in below section.
		//
		var splitWorker = new SplitWorker
			{
			original_wo_id			= details.woprog_id,
			origin					= details.origin,
			original_wo_line_id		= wo_line_id,
			current_user			= current_user,
			target_wo_id			= transfer_to_woprog_id,// When its value is 9999999, we still insert a new line item in po but the woID will be 9999999.
			target_wo_rec_no		= rec_no_from_new_wo_line_item,
			IsTransferToLocation	= transfer_to_woprog_id == 9999999,
			target_wo_line_id		= target_wo_line_id,  //another name is wo_detail_current_id;
			CanBeSplit				= IsLineItemCanBeSplit(details.id, int.Parse(xfer_master_id), details.origin),// Get the knowledge of whether it is can be split.
			xfer_master_id			= xfer_master_id,
			xfer_combined_qty		= xfer_combined_qty,
			qty_remaining			= qty_remaining
			};

		//
		// Call it now.
		//
		bool result = splitWorker.HandlePOLineItemAfterTransfer();
		return result;
	}

	/// <summary>
	/// This function is to hanlde the task of 1107:
	/// Do not allow expenses, per diems, cc purchases to be transferred if the isMigratedFlag is true, or if the pay period has been processed.
	/// This function also returns the person who will get paid.
	/// </summary>
	/// <param name="origin"></param>
	/// <param name="consignmentId"></param>
	/// <param name="workOrderId"></param>
	/// <param name="wo_detail_current_id"></param>
	/// <param name="isConsignmentIdProvided">true: consignmentId has been provided; false: not provided but can be obtained by using wo_detail_current_id </param>
	/// <returns></returns>
	private bool IsExpensePerDiemsCCPurchaseAvailableForTransferIfTheItemIsConsidered(string origin, int consignmentId, int workOrderId, int wo_detail_current_id, DateTime checkPointTime, bool isConsignmentIdProvided)
	{
		/*
			 Below are the contents from 1107:
			 =========================================================================
			 Transferring parts that originate from:
				Expenses
				Per Diems
				Credit Card Purchases

			 Two checkpoints.
			 --------------------------------------------------------------------------
			   (1) Should not be allowed if the associated isMigratedFlag is true for the line.
			   (2) These also should not be allowed to transfer if they are referring to a pay period that has been already processed.
		 */

		//
		// Preconditon check: only applying this when it is 'Expenses/Per Diems/Credit Card Purchases' line item.
		//
		var originateFrom = IsExpensePerDiemsCCPurchaseAvailable_TransferringPartOriginateFrom(origin);
		var fallingInThisFeature = (originateFrom == OriginType.ExpenseReimbursement ||
									originateFrom == OriginType.PerDiemExpense ||
									originateFrom == OriginType.CompanyCreditCardExpense);
		if (!fallingInThisFeature)
		{
			//
			// This item is not a 'Expenses/Per Diems/Credit Card Purchases', so let it go and continue.
			// In other words, we don't consider this wo line item in here.
			//
			return true;
		}

		//
		// When loading at page creation duration, the gridview will not provide the consignment column, so we need to load by this function.
		//
		if (!isConsignmentIdProvided)
		{
			consignmentId = ExpensePerDiemsCCPurchaseAvailable_LoadConsignmentId(wo_detail_current_id);
		}

		if (consignmentId == 0)
		{
			//
			// If no consignmentId, we can't let it go.
			// So far our code is running well.
			//
			return false;
		}

		// Check point 1
		int whoWillGetPaid = 0;
		var isMigratedFlagSet = IsExpensePerDiemsCCPurchaseAvailable_IsMigratedFlagSet(originateFrom, consignmentId, workOrderId, out whoWillGetPaid);
		if (isMigratedFlagSet)
		{
			return false;
		}

		// check point 2
		var isPayPeriodProcessed = IsExpensePerDiemsCCPurchaseAvailble_IsPayPeriodProcessed(checkPointTime, whoWillGetPaid);
		if (isPayPeriodProcessed)
		{
			return false;
		}

		//
		// Now this is available at current checkpoint time.
		//
		return true;
	}


	/// <summary>
	/// Caculate the transferring parts that originate from by checking the origin.
	/// </summary>
	/// <param name="origin"></param>
	/// <returns></returns>
	private OriginType IsExpensePerDiemsCCPurchaseAvailable_TransferringPartOriginateFrom(string origin)
	{
		if (string.IsNullOrWhiteSpace(origin))
		{
			return OriginType.Default;
		}

		if (origin.ToLowerInvariant() == OpsWOLineOrigin.ExpenseReimbursement.ToLowerInvariant())
		{
			return OriginType.ExpenseReimbursement;
		}

		if (origin.ToLowerInvariant() == OpsWOLineOrigin.PerDiemExpense.ToLowerInvariant())
		{
			return OriginType.PerDiemExpense;
		}

		if (origin.ToLowerInvariant() == OpsWOLineOrigin.CompanyCreditCardExpense.ToLowerInvariant())
		{
			return OriginType.CompanyCreditCardExpense;
		}

		return OriginType.Default;
	}


	/// <summary>
	/// Get the consignmentId by using the primary key 'wo_detail_current_id'.
	/// </summary>
	/// <param name="wo_detail_current_id"></param>
	/// <returns></returns>
	private int ExpensePerDiemsCCPurchaseAvailable_LoadConsignmentId(int wo_detail_current_id)
	{
		// Aways validates first.
		if (wo_detail_current_id == 0)
		{
			return 0;
		}

		// Query
		var query = @"SELECT  wo_detail_current_consignment_id FROM wo_detail_current  WHERE wo_detail_current_id = @v0";

		// Parameter
		var parameters = new object[] { wo_detail_current_id };

		// check the count.
		var consignmentId = Toolbox.doSQL_int(query, parameters);

		return consignmentId;
	}

	/// <summary>
	/// Checkpoint: (1) Should not be allowed if the associated isMigratedFlag is true for the line.
	/// </summary>
	/// <param name="origin"></param>
	/// <param name="consignmentId"></param>
	/// <param name="workOrderId"></param>
	/// <returns>Return ture: Already migrated. Cannot be transfered.</returns>
	private bool IsExpensePerDiemsCCPurchaseAvailable_IsMigratedFlagSet(OriginType originFrom, int consignmentId, int workOrderId, out int paidToWhom)
	{
		//
		// Here we base on the OriginType to access the different tables, and the consignmentId will be the primary value for the related table.
		// ExpenseReimbursement and PerDiemExpense: use expense_reimbursement, the primery key is [id_expense].
		// CompanyCreditCardExpens: use credit_card_purchase, the primary key is [id].
		//

		paidToWhom = 0;

		// Get a query
		var query = "";
		if (originFrom == OriginType.ExpenseReimbursement || originFrom == OriginType.PerDiemExpense)
		{
			query = @"SELECT isMigratedFlag, id_member AS 'paidTo' FROM expense_reimbursement WHERE woprog_id = @v0 AND id_expense = @v1";
		}

		if (originFrom == OriginType.CompanyCreditCardExpense)
		{
			query = @"SELECT isMigratedFlag, member_id AS 'paidTo' FROM credit_card_purchase WHERE woprog_id  = @v0 AND id = @v1";
		}

		// Normorized the parameters.
		var parameters = new object[] { workOrderId, consignmentId };

		var dt = Toolbox.doSQL_dt(query, parameters);
		if (dt == null || dt.Rows == null || dt.Rows.Count != 1)
		{
			//
			// we are using primary key in our query, should be only one in there.
			// If more....Data is wrong, not allowed.
			//
			return true;
		}

		DataRow row = dt.Rows[0];

		// Get who will get paid.
		paidToWhom = Convert.ToInt32(row["paidTo"]);

		// The isMigratedFlag column type is nullable.
		// So we treat null as false value, that means isMigratedFlag = false.
		if (row["isMigratedFlag"] == null || (row["isMigratedFlag"] is DBNull) )
		{
			// Not set, so can go. (as long as it is not TRUE)
			return false;
		}

		var isMigratedFlag = Convert.ToBoolean(row["isMigratedFlag"]);
		if (isMigratedFlag)
		{
			// The migrated flag is set, so can't be transfered.
			return true;
		}

		// The migrated flag not set, that means it is not handled by netsuite yet.
		return false;
	}

	/// <summary>
	/// (2) These also should not be allowed to transfer if they are referring to a pay period that has been already processed.
	/// </summary>
	/// <param name="checkPointTime"></param>
	/// <param name="whoWillGetPaid"></param>
	/// <returns>Returns True: Pay Period was processed, so can't be transfered.</returns>
	private bool IsExpensePerDiemsCCPurchaseAvailble_IsPayPeriodProcessed(DateTime checkPointTime, int whoWillGetPaid)
	{
		//
		// There are two checks following one by one.
		//
		// (1) If the associated pay period is set to completed = true.
		// (2) If the employee has a row in payroll_hours for that pay period, as the pay period might currently being approved.
		//

		//
		// (1) If the associated pay period is set to completed = true.
		//
		var queryForFirstCheck = @"SELECT * FROM payperiods WHERE @v0 BETWEEN StartDate AND EndDate";
		var parametersForFirstCheck = new object[] { checkPointTime };

		var dt = Toolbox.doSQL_dt(queryForFirstCheck, parametersForFirstCheck);
		if (dt == null || dt.Rows == null )
		{
			// No records now, so let it go becasue this is not processed by Payment sysem on payment period level.
			return false;
		}

		//
		// There are at least one record, so fetch the first one. (Should always be one.)
		//
		DataRow row = dt.Rows[0];
		int payPeriod = Convert.ToInt32(row["PayperiodID"]);

		int completed = 0;
		if (row["completed"] == null || (row["completed"] is DBNull))
		{
			// If it is null value, the completed is initialized to 0 as not finished yet.
		}
		else
		{
			completed = Convert.ToInt32(row["completed"]);
		}

		if (completed == 1)
		{
			// The related pay period is processed at this checking time, so return ture.
			return true;
		}

		//
		// Now the pay period is not processed but the payment maybe be processed. Do the checke 2.
		//
		// (2) If the employee has a row in payroll_hours for that pay period, as the pay period might currently being approved.
		//
		var queryForSecondCheck = @"SELECT * FROM payroll_hours WHERE payperiod_id = @v0 AND member_id = @v1";
		var parametersForSecondCheck = new object[] { payPeriod, whoWillGetPaid };

		var dt2 = Toolbox.doSQL_dt(queryForSecondCheck, parametersForSecondCheck);
		if (dt2 == null || dt2.Rows == null || dt2.Rows.Count == 0)
		{
			// No records now, so let it go becasue this is not processed by Payment sysem in payroll hours level.
			return false;
		}

		// Now we have at least 1 record, so it is processed.
		return true;
	}

	/// <summary>
	/// If the PO is closed, the item cannot be transferred; a negative Purchase Order is required, so the used should be warned/informed at this point. (This function does this)
	/// </summary>
	/// <param name="wo_id"></param>
	/// <param name="origin"></param>
	/// <param name="partno"></param>
	/// <returns></returns>
	private bool IsTransferAvailableIfPoBehindIt(int wo_id, string origin, int partno)
	{
		/*
		  From Patrick: What we think should be happening:
			�	If the PO is closed, the item cannot be transferred; a negative Purchase Order is required, so the used should be warned/informed at this point. (This function does this)
			�	Inventory items don't really need to be split as they will go into inventory (by ItemsCanGoThroughTheSplitProcess function)
			�	Subcontractor, material, other lines cannot be split (full transfers only), and only from open POs (Done in previous task)

		  From Patrik:
		  ------------------------------------------
		   If it's an inventory item (is_allowed_to_stock=true), it should be fine to allow. 
		   Anything else (material, shipping, subcontractor, other) should not be allowed if the PO is closed.

		 */

		//
		// Validation checks first.
		//
		if (wo_id == 0 || string.IsNullOrEmpty(origin) || partno == 0)
		{
			return true;
		}

		//
		// Check the orgin see whether it is coming from a po or more pos.
		//
		if (!origin.Contains("PO"))
		{
			// this wo line item is not from po, so allow the action of transfer due to this feature only applied the po stuff (CanTransferBeDoneIfPOBehideIt).
			return true;
		}

		//
		// 100% saying:  The origin has a 'PO.....' content now based on above checking.
		//
		if (IsLineItemCanBeSplit(0, partno, origin))
		{
			// The item can be split, it is inventory item.
			// If it's an inventory item (is_allowed_to_stock=true), it should be fine to allow. 
			return true;
		}

		//
		// Now we have at least one po line item connected to this wo line item, we need to load all the po_current_detais row.
		//
		SplitWorker sw = new SplitWorker();
		sw.poLineList = sw.PopulatePOInfo(origin, wo_id);
		sw.LoadPOLineItems();

		//
		// The exisiting code may do sth wrong, no allowed will be better.
		//
		if (sw.PoLineItems == null || sw.PoLineItems.Count == 0)
		{
			// If have PO but not have fully info: not allowed to changed.
			return false;
		}

		// 
		// Load all the po's status.
		//
		List<int> poStatusList = new List<int> { };
		foreach (var item in sw.PoLineItems)
		{
			if(item.po_details_poprog_id == 0) continue;
			var poprog_status = Toolbox.doSQL_int(@"SELECT poprog_status FROM poprog_header  WHERE poprog_id =@v0", new object[] { item.po_details_poprog_id });
			poStatusList.Add(poprog_status);
		}

		// 
		// Check the status.
		//
		foreach (var status in poStatusList)
		{

			/*
			 *poprog_status_id	status_type
							1	Not Issued
							2	Waiting BM Approval
							3	Issued-Waiting for Packing Slip
							4	Received-Waiting for Invoice
							5	Approved to Order
							6	Waiting to be Closed
							7	Closed-Paid
							8	Cancelled
							9	Questions
							10	AP Problems
							11	Issued-Waiting for Vendor Confirmation
							12	Issued-Waiting for Complete Delivery
									 *
			 */

			if (status == 7) // added more status in  here.
			{
				// one of them is closed.
				return false;
			}
		}

		//
		// All connected POs are not closed, okay to the transfer action.
		//
		return true;
	}

}

public class Origin
{
	// columns that exist in po_details_current.
	public int po_details_poprog_id { get; set; }
	public int po_details_rec_no { get; set; }
	public string addedDate { get; set; }
	public int po_details_woprog_id { get; set; } // this one denote: which work order connnecting to.
}

public class SplitWorker
{
	#region Input from outside of world

	public NeMember current_user { get; set; }
	public string origin { get; set; }
	public int original_wo_id { get; set; }
	public int original_wo_line_id { get; set; }

	public int target_wo_id { get; set; } // can be 99999999 for location transfering.
	public int target_wo_line_id { get; set; }
	public bool IsTransferToLocation { get; set; }
	public int target_wo_rec_no { get; set; }

	public bool CanBeSplit { get; set; }

	// What kind of thing.
	public string xfer_master_id { get; set; }

	// Transferred quantity.
	public double xfer_combined_qty { get; set; }

	// Remain quantity.
	public double qty_remaining { get; set; }
	#endregion

	//
	// Below two about the PO Line Items that related to this origional/source work order wo#1. (Transfer direction: wo#1 ==> wo#2/location? )
	//
	public List<Origin> poLineList { get; set; }
	public List<NEPO_Details_Current> PoLineItems { get; set; }

	//
	// Original work Order: wo#1
	//
	private NeWODetailCurrent OrgionalSourceWo_Detail_Current { get; set; }

	//
	// Target/new work order wo#2 if IsTransferToLocation is false.
	//
	private NeWODetailCurrent TargetNewWo_Detail_Current { get; set; }
	private object Stock_transfer_Record { get; set; }
	private readonly string RecNoPlaceHolder = "<X>";
	private readonly string POIDPlaceHolder = "<D>";

	//
	// UpdateItems: any item in this list will have their work order info updated.
	//
	private List<NEPO_Details_Current> UpdateItems { get; set; }
	//
	// ToBeSplitItems: any item in this list will be split. It can only be one or zero in this list.
	//
	private List<NEPO_Details_Current> ToBeSplitItems { get; set; }
	//
	// NewAddedItems: the new created line time when we have ONE item in ToBeSplitItems.
	//
	private List<NEPO_Details_Current> NewAddedItems { get; set; }
	//
	// RemainItems: Still belowing to the wo1 line item after transfer.
	//
	private List<NEPO_Details_Current> RemainItems { get; set; }

	private bool PlanToSourceWoLineItemDeleted { get; set; }

	private bool ItemsCanGoThroughTheSplitProcess()
	{
		/*
		  From Patrick: What we think should be happening:
			�	If the PO is closed, the item cannot be transferred; a negative Purchase Order is required, so the used should be warned/informed at this point. (check before transfer)
			�	Inventory items don't really need to be split as they will go into inventory 
			�	Subcontractor, material, other lines cannot be split (full transfers only), and only from open POs (Done in previous task)

		 */

		//
		// The way to set it by calling below function.
		// splitWorker.CanBeSplit = this.IsLineItemCanBeSplit(details.id, int.Parse(xfer_master_id), details.origin);
		//
		if (CanBeSplit)
		{
			//
			//  �	Inventory items don't really need to be split as they will go into inventory.
			//

			// Inventory Item can be split; non-inventory item can not be split: 9595, shipping or others.
			return false;
		}
		
		// This is a Non-Inventory Item, so go through the split process.
		return true;
	}

	public bool HandlePOLineItemAfterTransfer()
	{
		//
		// In terms of how many line items can be selected for transfering, we have TWO groups:
		// (1) Transfer ONE line item only.
		// (2) Transfer TWO or more line items.
		//
		// For the group of '(1) Transfer ONE line item only': The function of [PartTransfer] will be called.
		// For the group of '(2) Transfer TWO or more line items': The function of [xfer_cbp_Callback] will be called.Then inside of it the 
		//                                                          [PartTransfer] function will be called for each of the line item.
		// Based on above, this function will be called inside of [PartTransfer].
		//

		//
		// Check whether the Item can go through the Split process.
		//
		if (!ItemsCanGoThroughTheSplitProcess())
		{
			return false;
		}

		// Load all required info.
		if (!LoadData())
		{
			return false;
		}

		//
		// May delete the wo line item for wo#1 if the committed quantity is 0 by using "delete_workorder_line" api, then we can call reorder without any problems..
		//
		// If we delete it from here, we will not delete afterwards. what happen if the transaction failed, so can't do this.
		//
		PlanToSourceWoLineItemDeleted = false;

		// Make a plan
		if (!SplitPlanning())
		{
			return false;
		}

		// Run above plan.
		if (!SplitPlanExecuting())
		{
			return false;
		}

		if (PlanToSourceWoLineItemDeleted)
		{
			// wo line items already deleted. 
			NeWODetailCurrent.reorder_lines(original_wo_id);
		}

		return true;
	}

	#region private
	private bool LoadData()
	{
		// Load the origional work order Line Item after transfering.
		if (!LoadOriginalWoLineItem())
		{
			return false;
		}

		// Load the target stuff (maybe a new wo line item or a location sth?)
		if (!LoadTargetInfo())
		{
			return false;
		}

		// Populate the pointers to PO line items from origin of the wo#1 line item. (transfer direction: wo#1 ==> wo#2)
		poLineList = PopulatePOInfo(origin, original_wo_id);
		if (poLineList.Count == 0)
		{
			return false;
		}

		// Load all PO Line Items that related to this single WO line item.
		LoadPOLineItems();
		if (PoLineItems.Count == 0)
		{
			return false;
		}

		return true;
	}

	private bool LoadOriginalWoLineItem()
	{
		if (original_wo_line_id == 0)
		{
			return false;
		}

		try
		{
			OrgionalSourceWo_Detail_Current = new NeWODetailCurrent(original_wo_line_id);
		}
		catch (Exception ex)
		{
			return false;
		}

		return true;
	}

	private bool LoadTargetInfo()
	{
		if (!IsTransferToLocation)
		{
			// Transfer to another work order.
			if (target_wo_id == 0)
			{
				return false;
			}

			try
			{
				TargetNewWo_Detail_Current = new NeWODetailCurrent();
				// this.TargetNewWo_Detail_Current.LoadByWoIdRecNo(this.target_wo_id, this.target_wo_rec_no);
				TargetNewWo_Detail_Current = new NeWODetailCurrent(target_wo_line_id);
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		// Now it is transfered to a location.
		// Todo:

		return true;
	}

	public void LoadPOLineItems()
	{
		//
		// Question: Only fetch from this table po_details_current?
		//

		/* The related po line items to this wo line item may be like as following:

		 po_details_poprog_id	po_details_rec_no	po_details_woprog_id	po_details_part_no
		 =======================================================================================
					2061902	                    2	1043911	                52312
					2061903	                    2	1043911	                52312
					2061904	                    5	1043911	                52312

		 */

		PoLineItems = new List<NEPO_Details_Current> { };
		try
		{
			foreach (var line in poLineList)
			{
				var poCurrentRecord = new NEPO_Details_Current();
				poCurrentRecord.LoadByPoIdRecNoWoId(line.po_details_poprog_id, line.po_details_rec_no, line.po_details_woprog_id);
				PoLineItems.Add(poCurrentRecord);
			}
		}
		catch (Exception ex)
		{
			string err = ex.Message;
			PoLineItems = new List<NEPO_Details_Current>();
		}

		//
		// order this by po_details_qty_received descending way.
		//
		// we can change the orders of items if required.
		PoLineItems = PoLineItems.OrderByDescending(el => el.po_details_qty_received).ToList();
	}

	public List<Origin> PopulatePOInfo(string data, int woid)
	{
		//
		//Rule: string.Format("PO {0}-{3} added:{1} {2:yyyy-MM-dd}\n", bvpo, woqty, DateTime.Now, po_recno);
		//PO 0002061900-2 added:12 2019-01-29PO 0002061901 - 6 added: 88 2019-01-30
		//PO 0002061900-3 added:5 2019-01-29
		//

		var list = new List<Origin> { };

		if (string.IsNullOrWhiteSpace(data) || woid == 0)
		{
			return list;
		}

		var patternToOneRecord = @"PO (?<po>(.*)) added(.*)"; ;
		var matchCollection = Regex.Matches(data, patternToOneRecord, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		if (matchCollection == null || matchCollection.Count == 0)
		{
			return list;
		}

		var patternOfPopulate = @"PO (?<po>[0-9]+?)-(?<rec>[0-9]+?) added:[^d]* (?<date>(.*)?)";
		Regex regex = new Regex(patternOfPopulate, RegexOptions.None);
		var tempPo = 0;
		var tempRec = 0;
		for (int i = 0; i < matchCollection.Count; i++)
		{
			string oneRecord = matchCollection[i].Value;
			var match = regex.Match(oneRecord);
			if (match.Success)
			{
				var po = match.Groups["po"].Value;
				var rec = match.Groups["rec"].Value;
				string dateData = "";
				dateData = match.Groups["date"].Value;
				dateData = dateData.Replace("\n", "");

				var poOkay = int.TryParse(po, out tempPo);
				var recOkay = int.TryParse(rec, out tempRec);

				if (poOkay && recOkay)
				{
					var origin = new Origin
					{
						po_details_poprog_id = tempPo,
						po_details_rec_no = tempRec,
						po_details_woprog_id = woid,
						addedDate = dateData
					};

					//
					// When received the po little b little, PO info will be duplicated.
					// PO 0002061928-2 added:3 2019-02-11PO 0002061928-2 added: 3 2019 - 02 - 11
					//
					bool found = false;
					found = list.Where(x => x.po_details_poprog_id == tempPo && x.po_details_rec_no == tempRec).Count() > 0;

					if (!found)
					{
						list.Add(origin);
					}
				}
			}
		}

		return list;
	}

	private bool SplitPlanning()
	{
		//
		// This code addresses the problem of splitting PO items after transferring them.
		// In terms of the types of WO/PO line items, we have two types: Inventory type or Non-Inventory type.
		// Non-inventory items can't be split when transferred (you will see the number will be locked on the page), but Inventory items can be split.
		// ==================================================================================================================
		// This algorithm handles both types of transfers. It introduces four key members: UpdateItems, ToBeSplitItems, NewAddedItems, and RemainItems (focus on first 3).
		// (1) For Non-Inventory transfer, UpdateItems will only have one item, and there will be no items in ToBeSplitItems and NewAddedItems.
		// (2) For Inventory transfer, UpdateItems will have one or more items, ToBeSplitItems will have zero or one item, and NewAddedItems will have one if ToBeSplitItems has one.
		// (3) Repointing: Calculate the origin for both the original WO line item and the newly added one.

		/*
		Below is the data before transferring. It can help understand what happens if we transfer 109 parts of '52312 Drive - AC Drive - VFD/Inve', tagged by (*****).
		After applying this algorithm, we may have:
		[1] The UpdateItems will hold '410219': po_details_woprog_id = wo#2.
		[2] ToBeSplitItems will hold '410212': po_details_qty_ordered = 28, po_details_qty_received = 28.
		[3] NewAddedItems will hold a new line item: po_details_woprog_id = wo#2, po_details_qty_ordered = 10, po_details_qty_received = 10.
		[4] RemainItems will hold nothing in this case.
		[5] Repointing: wo#1.origin = "PO 0002061902-2 added:28 [Today]"; wo#2.origin ="PO 0002061902-X added:28 [Today]PO 0002061903-5 added:99 [Today]", X = 6.

		Note:
		(1) The committed quantity is 137, and it is the summary of 38 and 99 from the above two different PO lines. More generally, the number from the WO line item (137) may be greater than the summary of
		related PO lines because users can add more committed quantity from the WO line item directly. If we still run this algorithm, it would be like transferring the item from manually added ones than PO Lines.
		But in the real environment, it may not be this case. (We may check the relationship between the manually added number and transferred number: If the manually added number can cover the transferred number, no need to do the split thing; but once not, we will fall into the work of doing this split thing.)
		(2) When transferring to locations (in/out), we will pick up the 99999999 as the work order ID and still run this algorithm.
		(3) If fully transferring, we may set wo#1.origin = "" from [5].
		(4) How to keep the origin info completely? For wo#2.origin = [new one] + [old one]; for wo#1.origin = <all PO lines info>, that means "manually added / xx Transferred from yyyy" will be gone.

		In order to reduce changes to the PO lines (or touch less PO line items), we will check the first PO line with MAX committed value, then one by one based on committed quantity.
		This algorithm guarantees this by sorting the 'PoLineItems' list when loading. By doing this, we will touch fewer PO lines.


		po_details_current.po_details_poprog_id = 2061902:
		po_details_id | po_details_rec_no | po_details_part_no | po_details_description              | po_details_woprog_id | po_details_qty_ordered | po_details_qty_received
		-------------------------------------------------------------------------------------------------------------------------------------------------------------------
		410212        | 2                 | 52312              | Drive - AC Drive - VFD/Inve..        | 1043911              | 38.00                  | 38.00   (*****)
		410213        | 3                 | 9595               | Sub Contractor ...                   | 1043911              | 3.00                   | 3.00
		410214        | 4                 | 5216               | Shipping - Freight Charge...         | 1043911              | 8.00                   | 8.00
		410215        | 5                 | 9595               | Sub Contractor test 002              | 1043911              | 7.00                   | 7.00

		po_details_current.po_details_poprog_id = 2061903:
		po_details_id | po_details_rec_no | po_details_part_no | po_details_description              | po_details_woprog_id | po_details_qty_ordered | po_details_qty_received
		-------------------------------------------------------------------------------------------------------------------------------------------------------------------
		410216        | 2                 | 9595               | Sub Contractor test 003              | 1043911              | 9.00                   | 9.00
		410217        | 3                 | 9595               | Sub Contractor test 004              | 1043911              | 55.00                  | 55.00
		410218        | 4                 | 5216               | Shipping - Freight Charge...         | 1043911              | 7.00                   | 7.00
		410219        | 5                 | 52312              | Drive - AC Drive - VFD/Inve..        | 1043911              | 99.00                  | 99.00   (*****)

		wo_detail_current.wo_detail_current_id = 1269192:
		woprog_id | rec_no | wo__master_id | wo_detail_current_description         | wo_.._qty_committed | wo_detail_current_origin  
		------------------------------------------------------------------------------------------------------------------------------------
		1043911   | 2      | 52312        | Drive - AC Drive - VFD/Inverte..        | 137.00              | PO 0002061902-2 added:38 2019-01-31PO 0002061903-5 added:99 2019-01-31


		 */

		ToBeSplitItems = new List<NEPO_Details_Current> { };
		NewAddedItems = new List<NEPO_Details_Current> { };
		UpdateItems = new List<NEPO_Details_Current> { };
		RemainItems = new List<NEPO_Details_Current> { };

		bool canCover = ManuallyAddedCanCoverTransfer();
		if (canCover)
		{
			return false;
		}

		double sum = 0;
		double requiredFromCurrent = 0;
		bool done = false;
		for (int i = 0; i < PoLineItems.Count; i++)
		{
			var current = PoLineItems[i];
			if (i == 0)
			{
				sum = current.po_details_qty_received;
			}
			else
			{
				sum += current.po_details_qty_received;
			}

			if (sum < xfer_combined_qty)
			{
				UpdateItems.Add(current);
			}

			if (sum == xfer_combined_qty && (!done))
			{
				UpdateItems.Add(current);
				done = true;
				continue; // for remain items.
			}

			if (sum > xfer_combined_qty && (!done))
			{
				ToBeSplitItems.Add(current);
				double before = sum - current.po_details_qty_received;
				requiredFromCurrent = xfer_combined_qty - before;
				done = true;
				continue;
			}

			if (done)
			{
				RemainItems.Add(current);
			}
		}

		SplitPlanning_UpdateInfo(requiredFromCurrent);

		return true;
	}

	private bool ManuallyAddedCanCoverTransfer()
	{
		/*
		 we may check the relationship between manually added number and transfered number: If manually added number can cover the transfered number, no need to do the
			split thing;

		 Note: This is only happening for INVENTORY items.

		 */

		// When running into here, the transfter was job done.  We say the left committed number in this work order: LEFT, say the conbribution from all PO line itmes is CONTR.
		double left = OrgionalSourceWo_Detail_Current.qty_committed;
		double contribution = PoLineItems.Sum(x => x.po_details_qty_received);

		if (left >= contribution)
		{
			return true;
		}

		// Left is less than contribution, must split one.
		return false;
	}

	private void SplitPlanning_UpdateInfo(double required)
	{
		//
		// To be updated po line items.
		//
		foreach (var record in UpdateItems)
		{
			record.po_details_woprog_id = target_wo_id;
		}

		//
		// To be split po line item.
		//
		if (ToBeSplitItems.Count == 1)
		{
			var beSplitRecord = ToBeSplitItems[0];
			beSplitRecord.po_details_qty_received = beSplitRecord.po_details_qty_received - required;
			beSplitRecord.po_details_qty_orderd = beSplitRecord.po_details_qty_orderd - required;

			var newAdded = new NEPO_Details_Current();
			newAdded.po_details_qty_received = required;
			newAdded.po_details_qty_orderd = required;
			newAdded.po_details_woprog_id = target_wo_id; // wo2
			newAdded.po_details_rec_no = 0;
			newAdded.po_details_poprog_id = beSplitRecord.po_details_poprog_id;

			// for other info we can borrow from beSplitRecord.
			NewAddedItems.Add(newAdded);
		}

		//
		// origin info for two wo1, and wo2.
		//
		DateTime now = DateTime.Now;

		//
		// We need to keep the previous one for the wo2.origin, here we od: [calculated po list] + "\n" + "old origin".
		//
		var contributedItemToNewAddedWoLine = new List<NEPO_Details_Current> { };
		contributedItemToNewAddedWoLine.AddRange(UpdateItems);
		contributedItemToNewAddedWoLine.AddRange(NewAddedItems);
		if (!IsTransferToLocation)
		{
			if (string.IsNullOrWhiteSpace(TargetNewWo_Detail_Current.origin))
			{
				TargetNewWo_Detail_Current.origin = CalculateOrigin(contributedItemToNewAddedWoLine, now);
			}
			else
			{
				TargetNewWo_Detail_Current.origin = CalculateOrigin(contributedItemToNewAddedWoLine, now) + "\n" +  TargetNewWo_Detail_Current.origin;
			}
		}

		//
		// [Q] How to keep the origin info completely: the wo#1 will start as a PO line item, may not be 'Manually added' item (Question?). Other Items can be transferted to here by adding 'XXX Transferred From: 0001036131'
		// What happen if we transfer it again to other.
		// search: ' var onerowtable = Toolbox.doSQL_dt(@"SELECT * FROM wo_detail_current  WHERE wo_detail_current_woprog_id =@v0 AND wo_detail_current_master_id =@v1
		//    and wo_detail_current_origin like CONCAT('%PO',@v2,'-',@v3,'%')", new object[] { strwoid, master_id, poprog.poprog_bvpo, po_recno });'
		// [A] As Matt explained, there is only here using the origin. My code will keep all the related PO info in there, so we can replace the old one with new one.
		//
		var stillContributedToOldWoLine = new List<NEPO_Details_Current> { };
		stillContributedToOldWoLine.AddRange(RemainItems);
		stillContributedToOldWoLine.AddRange(ToBeSplitItems);
		OrgionalSourceWo_Detail_Current.origin = CalculateOrigin(stillContributedToOldWoLine, now);
	}

	private string CalculateOrigin(List<NEPO_Details_Current> list, DateTime date)
	{
		//
		//Rule: string.Format("PO {0}-{3} added:{1} {2:yyyy-MM-dd}\n", bvpo, woqty, DateTime.Now, po_recno);
		//PO 0002061900-2 added:12 2019-01-29PO 0002061901 - 6 added: 88 2019-01-30
		//PO 0002061900-3 added:5 2019-01-29
		//

		string format = "PO {0}-{3} added:{1} {2:yyyy-MM-dd}\n";
		string seperator = "\n";

		string origin = "";

		foreach (var item in list)
		{
			string recNo = item.po_details_rec_no.ToString();
			string poid = item.po_details_poprog_id.ToString();
			if (item.po_details_rec_no == 0)
			{
				// The to be added po line not inserted, need a placeholder now. But the poid is know by the item here.
				recNo = RecNoPlaceHolder;
				// poid = this.POIDPlaceHolder;
			}

			var addedDate = date;
			foreach (var po in poLineList)
			{
				// Get the date.
				if (po.po_details_poprog_id == item.po_details_rec_no && po.po_details_rec_no == item.po_details_rec_no)
				{
					addedDate = Convert.ToDateTime(po.addedDate);
					break;
				}
			}

			var origionFromItem = string.Format(format, "000" + poid, item.po_details_qty_received, addedDate, recNo);

			if (string.IsNullOrEmpty(origin))
			{
				origin = origionFromItem;
			}
			else
			{
				origin += seperator + origionFromItem;
			}
		}

		return origin;
	}

	private bool SplitPlanExecuting()
	{
		//
		// Pay attention to RecNoPlaceHolder...
		//
		string UpdateItemsSQl = "";
		object[] UpdateItemsParamObject = new object[] { };

		string NewAddedItemsSql = "";
		object[] NewAddedItemsParamObjects = new object[] { };
		string sqlToGetKey = "";
		string sqlToUpdateReference = "";
		int newRec_no = 0;

		string ToBeSplitItemsSQl = "";
		object[] ToBeSplitItemsParamObjects = new object[] { };

		string oldOrigionSQl = "";
		object[] oldOrigionParamObjects = new object[] { };

		string newWoOriginSQl = "";
		object[] newWoOriginParamObjects = new object[] { };

		//
		// The function is to conver the high level change requirements to executeable sqls with parameters within one transaction.
		//

		//
		// Generate NewAddedItems related sql
		//
		GenerateSqlForAddingNewPOLIne(out NewAddedItemsSql, out sqlToGetKey, out sqlToUpdateReference, out NewAddedItemsParamObjects, out newRec_no);

		//
		// Generate ToBeSplitItems related sql
		//
		GenerateSqlForSplitPoLineItem(out ToBeSplitItemsSQl, out ToBeSplitItemsParamObjects);

		//
		// Generate UpdateItems related sql
		//
		GenerateSqlForUpdateItems(out UpdateItemsSQl, out UpdateItemsParamObject);

		//
		// Generate Origin related sql
		//
		GenerateSqlFoOldOrigin(out oldOrigionSQl, out oldOrigionParamObjects);
		GenerateSqlFoNewOrigin(out newWoOriginSQl, out newWoOriginParamObjects, newRec_no, "");

		var done = doSQL_dt_Split(
			UpdateItemsSQl, UpdateItemsParamObject,
			NewAddedItemsSql, NewAddedItemsParamObjects, sqlToGetKey, sqlToUpdateReference, newRec_no,
			ToBeSplitItemsSQl, ToBeSplitItemsParamObjects,
			oldOrigionSQl, oldOrigionParamObjects,
			newWoOriginSQl, newWoOriginParamObjects
			);
		return done;
	}

	private bool GenerateSqlForUpdateItems(out string UpdateItemsSQl, out object[] UpdateItemsParamObject)
	{
		UpdateItemsSQl = "";
		UpdateItemsParamObject = new object[] { };

		if (UpdateItems.Count == 0)
		{
			return true;
		}


		var list = "";
		foreach (var item in UpdateItems)
		{
			if (string.IsNullOrEmpty(list))
			{
				list = item.po_details_id.ToString();
				UpdateItemsSQl = "Update po_details_current set po_details_woprog_id = " + UpdateItems[0].po_details_woprog_id + " where po_details_id = " + item.po_details_id.ToString() + ";";
			}
			else
			{
				list += "," + item.po_details_id.ToString();

				UpdateItemsSQl += "Update po_details_current set po_details_woprog_id = " + UpdateItems[0].po_details_woprog_id + " where po_details_id = " + item.po_details_id.ToString() + ";";
			}
		}

		UpdateItemsParamObject = new object[]
		{
		};

		return true;
	}

	private bool GenerateSqlForAddingNewPOLIne(out string newAddedItemsSql, out string sqlToGetKey, out string sqlToUpdateReference, out object[] newAddedItemsParamObjects, out int newRec_no)
	{
		newAddedItemsSql = "";
		sqlToGetKey = "";
		sqlToUpdateReference = "";
		newAddedItemsParamObjects = new object[] { };
		newRec_no = 0;

		if (NewAddedItems.Count == 0)
		{
			// No need to add a new po line item.
			return true;
		}

		var existing = ToBeSplitItems[0];
		var newIem = NewAddedItems[0];

		var lines = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM po_details_current  WHERE po_details_poprog_id =@v0", new object[] { existing.po_details_poprog_id });
		var po_details_rec_no = lines + 2;
		var po_details_reference = 1;

		newRec_no = po_details_rec_no;

		newAddedItemsSql = @"INSERT INTO po_details_current (
  po_details_reference,
  po_details_rec_no,
  po_details_part_no,
  po_details_vendor_part_no,
  po_details_description,
  po_details_notes,
  po_details_qty_ordered,
  po_details_qty_received,
  po_details_cost,
  po_details_sell_price,
  po_details_poprog_id,
  po_details_woprog_id,
  po_details_date_added,
  po_details_date_modified,
  po_details_date_expected,
  po_details_add_member_id,
  po_details_audit_member_id,
  po_details_vendor_qty_per,
  to_wo,
  business_unit_id,
  is_gl_account,
  expense_category_id,
  po_details_line_active
)
VALUES
  (@v0,
  @v1,
  @v2,
  @v3,
  @v4,
  @v5,
  @v6,
  @v7,
  @v8,
  @v9,
  @v10,
  @v11,
  @v12,
  @v13,
  @v14,
  @v15,
  @v16,
  @v17,
  @v18,
  @v19,
  @v20,
  @v21,
  @v22,
  @v23,
  @v24,
  @v25,
  @v26
)";

		newAddedItemsParamObjects = new object[]
		{
			// existing.po_details_reference,
			po_details_reference,

			// existing.po_details_rec_no,
			po_details_rec_no,

			existing.po_details_part_no,
			existing.po_details_vendor_part_no,
			existing.po_details_description,
			existing.po_details_notes,

			// existing.po_details_qty_orderd,
			newIem.po_details_qty_orderd,
			// existing.po_details_qty_received,
			newIem.po_details_qty_received,

			existing.po_details_cost,
			existing.po_details_sell_price,
			existing.po_details_tax1,
			existing.po_details_tax2,
			existing.po_details_tax3,
			existing.po_details_tax4,


			existing.po_details_poprog_id,

			// existing.po_details_woprog_id,
			newIem.po_details_woprog_id,

			existing.po_details_date_added,
			existing.po_details_date_modified,
			existing.po_details_date_expected,
			existing.po_details_add_member_id,
			existing.po_details_audit_member_id,
			existing.po_details_vendor_qty_per,
			existing.to_wo,
			existing.business_unit_id,
			existing.is_gl_account,
			existing.expense_category_id,
			0 // po_details_line_active should be false
		};

		sqlToGetKey = "SELECT LAST_INSERT_ID()";
		sqlToUpdateReference = "update po_details_current set po_details_reference= <Ref> where po_details_id = <Ref>";

		return true;
	}

	private bool GenerateSqlForSplitPoLineItem(out string ToBeSplitItemsSQl, out object[] ToBeSplitItemsParamObjects)
	{
		ToBeSplitItemsSQl = @"
Update po_details_current
Set po_details_qty_received = @v0, po_details_qty_ordered = @v1
Where po_details_id = @v2
";
		ToBeSplitItemsParamObjects = new object[]
		{
		};

		if (NewAddedItems.Count == 0)
		{
			return true;
		}

		var existing = ToBeSplitItems[0];
		ToBeSplitItemsParamObjects = new object[]
		{
			existing.po_details_qty_received,
			existing.po_details_qty_orderd,
			existing.po_details_id
		};

		return true;
	}

	private bool GenerateSqlFoOldOrigin(out string oldOrigionSQl, out object[] oldOrigionParamObjects)
	{
		oldOrigionParamObjects = new object[]
		{
		};

		oldOrigionSQl =
@"UPDATE
  wo_detail_current
SET
  wo_detail_current_origin = @v0
WHERE wo_detail_current_id = @v1";

		oldOrigionParamObjects = new object[]
		{
			OrgionalSourceWo_Detail_Current.origin,
			OrgionalSourceWo_Detail_Current.id
		};

		if (RemainItems.Count == 0 && ToBeSplitItems.Count == 0 && OrgionalSourceWo_Detail_Current.qty_committed == 0)
		{
			//
			// In this case, if we want to delete the orignial wo line item, wo can do here. But before this, we need to check how to get the rec_no for wo line item.
			//

			// this.OrgionalSourceWo_Detail_Current.qty_committed == 0": wo line item contributed by 1 po line + manually added items based on the algorithm, when still have 1 or more in the origin wo line item,
			// we can't delete it.

			oldOrigionSQl = "delete from wo_detail_current WHERE wo_detail_current_id = @v0";

			oldOrigionParamObjects = new object[]
			{
				OrgionalSourceWo_Detail_Current.id
			};

			PlanToSourceWoLineItemDeleted = true;
		}

		return true;
	}

	private bool GenerateSqlFoNewOrigin(out string newWoOriginSQl, out object[] newWoOriginParamObjects, int rec_no, string poID)
	{
		newWoOriginParamObjects = new object[]
		{

		};

		newWoOriginSQl = "";

		if (IsTransferToLocation)
		{
			// NO new wo line item created.
			return true;
		}

		newWoOriginSQl =
			@"UPDATE
  wo_detail_current
SET
  wo_detail_current_origin = @v0
WHERE wo_detail_current_id = @v1";

		TargetNewWo_Detail_Current.origin =
			TargetNewWo_Detail_Current.origin.Replace(RecNoPlaceHolder, rec_no.ToString());

		// this.TargetNewWo_Detail_Current.origin = this.TargetNewWo_Detail_Current.origin.Replace(this.POIDPlaceHolder, poID);

		newWoOriginParamObjects = new object[]
		{
			TargetNewWo_Detail_Current.origin,
			TargetNewWo_Detail_Current.id
		};

		return true;
	}

	/// <summary>
	/// Do all the database related operations in here.
	/// </summary>
	/// <param name="UpdateItemsSQl"></param>
	/// <param name="UpdateItemsParamObjects"></param>
	/// <param name="NewAddedItemsSql"></param>
	/// <param name="NewAddedItemsParamObjects"></param>
	/// <param name="sqlToGetKey"></param>
	/// <param name="sqlToUpdateReference"></param>
	/// <param name="newRec_no"></param>
	/// <param name="ToBeSplitItemsSQl"></param>
	/// <param name="ToBeSplitItemsParamObjects"></param>
	/// <param name="oldOrigionSQl"></param>
	/// <param name="oldOrigionParamObjects"></param>
	/// <param name="newWoOriginSQl"></param>
	/// <param name="newWoOriginParamObjects"></param>
	/// <returns></returns>
	private bool doSQL_dt_Split(
		string UpdateItemsSQl, object[] UpdateItemsParamObjects,
		string NewAddedItemsSql, object[] NewAddedItemsParamObjects, string sqlToGetKey, string sqlToUpdateReference, int newRec_no,
		string ToBeSplitItemsSQl, object[] ToBeSplitItemsParamObjects,
		string oldOrigionSQl, object[] oldOrigionParamObjects,
		string newWoOriginSQl, object[] newWoOriginParamObjects
		)
	{
		//
		// ref: https://dev.mysql.com/doc/dev/connector-net/8.0/html/M_MySql_Data_MySqlClient_MySqlConnection_BeginTransaction.htm
		// ref: https://dev.mysql.com/doc/refman/5.6/en/myisam-storage-engine.html (MyISAM not support transaction)
		// Table wo_detail_current: MyISAM
		// Table po_details_current: InnoDB
		//

		bool done = false;

		using (var conn = Toolbox.do_open_conn())
		{
			using (var comm = conn.CreateCommand())
			{
				using (var transaction = conn.BeginTransaction()) // wo_current_detail: MyISAM engine, not support transaction
				{
					comm.Transaction = transaction;
					try
					{
						int key = 0;
						if (NewAddedItems.Count == 1)
						{
							//
							// Insert the new po line item.
							//
							comm.CommandText = NewAddedItemsSql;
							comm.Parameters.Clear();
							Toolbox.AddParametersToComm(comm, NewAddedItemsParamObjects);
							comm.ExecuteNonQuery();

							// Get ID 
							comm.CommandText = sqlToGetKey;
							comm.Parameters.Clear();
							comm.ExecuteNonQuery();
							key = Convert.ToInt32(comm.ExecuteScalar());

							// Update the reference
							sqlToUpdateReference = sqlToUpdateReference.Replace("<Ref>", key.ToString());
							comm.CommandText = sqlToUpdateReference;
							comm.Parameters.Clear();
							comm.ExecuteNonQuery();

							//
							// Update the split one
							//
							comm.CommandText = ToBeSplitItemsSQl;
							comm.Parameters.Clear();
							Toolbox.AddParametersToComm(comm, ToBeSplitItemsParamObjects);
							comm.ExecuteNonQuery();
						}

						// Update part.
						if (!string.IsNullOrWhiteSpace(UpdateItemsSQl))
						{
							comm.CommandText = UpdateItemsSQl;
							comm.Parameters.Clear();
							Toolbox.AddParametersToComm(comm, UpdateItemsParamObjects);
							comm.ExecuteNonQuery();
						}

						// Update Origin - old
						if (!string.IsNullOrWhiteSpace(oldOrigionSQl))
						{
							if (oldOrigionSQl.Contains("delete"))
							{
								//
								// Because transaction not supported
								// throw new Exception("You Cannot Delete an Item Transferred From an OPEN PO.");
								// int line = (int)(oldOrigionParamObjects[0]);
								// NeWODetailCurrent.delete_workorder_line(line, this.xfer_master_id, this.current_user);
								//
							}

						//
							comm.CommandText = oldOrigionSQl;
							comm.Parameters.Clear();
							Toolbox.AddParametersToComm(comm, oldOrigionParamObjects);
							comm.ExecuteNonQuery();
						}

						// Update Origin - new
						if (!string.IsNullOrWhiteSpace(newWoOriginSQl))
						{
							comm.CommandText = newWoOriginSQl;
							comm.Parameters.Clear();
							string s = newWoOriginParamObjects[0] as string;
							object[] newParas = new object[]
							{
								s,
								newWoOriginParamObjects[1]
							};
							Toolbox.AddParametersToComm(comm, newParas);
							comm.ExecuteNonQuery();
						}

						//
						// Fault injection
						//
						// int zero = 0;
						// int fi = 123 / zero;

						// commit at once
						transaction.Commit();

						// Set it to true
						done = true;
					}
					catch (Exception ee)
					{
						transaction.Rollback();
						done = false;
					}
				}
			}
		}

		return done;
	}
	#endregion
}