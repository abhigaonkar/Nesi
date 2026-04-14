using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Utils;
using MySql.Data.MySqlClient;
using DevExpress.Web;
using DevExpress.Web.Data;
using Image = System.Web.UI.WebControls.Image;
using DevExpress.XtraPrintingLinks;
//using nesi.bv;
using nesi.core;
using System.Web.Services;
using NESI.Common.Models;

public partial class sections_member_picklist_pikclist : Page
{
    #region Initial Variable Declaration

    private NeMember current_user;
    private NeBusinessUnit temp_comp;
    private readonly int _page_id = 1;
    private const string strViewCostPriv = "58"; //View Costs
    private const string strViewLabCostPriv = "93"; //View Labour costs
    private const string strViewCostPage = "12";
    private const string _strChangePricePage = "12"; // Change Price
    private const string _strChangePricePriv = "30";
    private const string _strDeletePage = "12"; //Deleting Work Order Row
    private const string _strDeletePriv = "65";
    private const string _strPrivBlenInv = "63"; //Setting Blended
    private const string _strPageBlenInv = "12";
    private DataTable dt_excludes;
    private int newtableid;
    private bool lockdesc;
    private int oldrecno;
    private int newrecno;
    private int main_id;
    private int main_rev;
    private string id;
    private string rev;
    private bool is_adding;
    private bool member_can_see_cost;
    private bool member_can_see_sell;
    private bool member_can_edit_sell;
    private bool member_can_see_labour_cost;
    private bool member_can_discount;
    private bool doBv;
    private bool can_receive;
    private string newsectionid;
    private string oldsectionid;
    private string newworkorderid;
    private bool new_is_gl_account;
    private string oldworkorderid;
    private string newpartnumber = "0";
    private string oldpartnumber = "0";
    private string newvendorpartnumber;
    private string oldvendorpartnumber;
    private string newdescription;
    private string olddescription;
    private string transferbvwo = "0";
    private string originbvwo;
    private double newqty;
    private double comnewqty;
    private double oldqty;
    private string origin = "";
    private int xfer_comm_recv_select_id;
    private int xfer_wo_select_id;
    private int xfer_internal_select_id;
    private int xfer_external_select_id;

    private double xfer_qty_wo;
    private double xfer_qty_internal;
    private double xfer_qty_external;
    private double xfer_combined_qty;
    private double xfer_to_original_qty;
    private double oldsell;
    private double newsell;
    private double oldquoteextd;
    private double newquoteextd;
    private string newdatereq = "";
    private string olddatereq = "";
    private double neworigsell;
    private double oldorigsell;
    private double transfersell;
    private string tablename = "current";
    private string transferwoprogid = "";
    private bool newinclude;
    private int cost_level;
    private double newcost;
    private double oldcost;
    private double newqtyperpart = 1;
    private double oldqtyperpart;
    private string newDateExpected = "";
    private string oldDateExpected = "";
    private double new_qty_committed;
    private double old_qty_committed;
    private string newNotes = "";
    private string newbilltype = "";
    private string oldbilltype = "";
    private string transfernote = "";
    private int newtax1;
    private int newtax2;
    private int newtax3;
    private int newtax4;
    private bool FlagPartcode = true;
    private double poqty;
    private int po_status;
    private string notesforwoaudit = "";
    private string newdivisionid = "0";
    private string olddivisionid = "0";
    private double newdiscount;
    private double olddiscount;
    private double DiscountAmount;
    private int NewTrack;
    private int OldTrack;
    private string po_n = "";
    private Label header_label;
    private Label headersub_label;
    private bool chk_workorder_transfer;
    private bool can_edit_part_cost;
    private HtmlImage header_print;
    private HtmlImage header_printwo;
    private HtmlImage header_printwo_unf;
    private NameValueCollection _q;
    private bool isDifferentSection;
    public NeBusinessUnit WorkingBusinessUnit { get; set; }
    public NeBusinessUnit WarehouseBusinessUnit { get; set; }
    private readonly DataSet _handlers = new DataSet();
    private readonly JavaScriptSerializer jSON = new JavaScriptSerializer();
    private DataTable dt_gl_tes;
    private DataTable dt_expense_categories;
    private DataTable dt_additional_categories;


    #endregion Initial Variable Declaration

    private Control ctrlname;
    private bool auth_new_location;
    private MySqlConnection conn { get; set; }
    private string processingWorkOrderStatus { get; set; }
    private int processingWorkOrderId { get; set; }

    /// <summary>
    /// [1075] [Nesi UAT] We need to control the visibility of the "All Items Complete" button on a PO a bit better.
    /// PoCompleteState is used to keep track of the active state for each of po line item.
    /// The devExpress part is using a DataItemTemplate which can't be updated in the postback. (can be set in page-init or page-load events)
    /// The idea is to push a list of po lines to the browser side and let it to update the image.
    /// </summary>
    public class PoCompleteState
    {
        public int poLineId { get; set; }
        public int poLineState { get; set; }
        public bool received { get; set; }
    }
    public List<PoCompleteState> poLineCompleteInfo { get; set; }
    // End of

    protected class xfer_array
    {
        private int _id;
        private int _part_id;
        private double _qty;
        private bool _use_wo;
        private int _dest_id;

        public int id
        {
            get { return _id; }
            set { _id = Convert.ToInt32(value); }
        }

        public int part_id
        {
            get { return _part_id; }
            set { _part_id = Convert.ToInt32(value); }
        }

        public double qty
        {
            get { return _qty; }
            set { _qty = Convert.ToDouble(value); }
        }

        public string origin { get; set; }

        public string type { get; set; }

        public bool use_wo
        {
            get { return _use_wo; }
            set { _use_wo = Convert.ToBoolean(value); }
        }

        public int dest_id
        {
            get { return _dest_id; }
            set { _dest_id = Convert.ToInt32(value); }
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        conn = Toolbox.connect();
        _q = Request.QueryString;
        var returned = "";
        id = string.IsNullOrEmpty(_q["id"]) ? "" : _q["id"];
        int.TryParse(id, out main_id);
        origin = _q["origin"];
        hidOrigin.Value = origin;
        rev = _q["rev"] != null ? _q["rev"] : "1";
        int.TryParse(rev, out main_rev);
        current_user = Toolbox.do_handle_authentication(_page_id);
        hidID.Value = id;
        hidRev.Value = rev;
        hidCompanyID.Value = current_user.business_unit_id.ToString();
        WorkingBusinessUnit = new NeBusinessUnit(hidCompanyID.Value);
        doBv = WorkingBusinessUnit.DSN != "";
        WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
        Session["warehouse_bu_id"] = WarehouseBusinessUnit.id;
        hidWorkingBusinessUnitID.Value = WorkingBusinessUnit.id.ToString();
        hidWarehouseBusinessUnitID.Value = WarehouseBusinessUnit.id.ToString();
        hidCurrentLoginUser.Value = current_user.id.ToString();

        header_label = rp_Main.FindControl("lblHeaderText") as Label;
        headersub_label = rp_Main.FindControl("lblHeaderText_sub") as Label;

        header_print = rp_Main.FindControl("ImgBtn_Print") as HtmlImage;
        header_printwo = rp_Main.FindControl("ImgBtn_PrintWO") as HtmlImage;
        header_printwo_unf = rp_Main.FindControl("ImgBtn_PrintWOUnf") as HtmlImage;
        member_can_see_cost = current_user.AuthenticatedForPrivilege(58);
        member_can_edit_sell = current_user.AuthenticatedForPrivilege(30);
        member_can_see_sell = current_user.AuthenticatedForPrivilege(81);
        member_can_see_labour_cost = current_user.AuthenticatedForPrivilege(93);
        member_can_discount = current_user.AuthenticatedForPrivilege(94);
        can_receive = current_user.AuthenticatedForPrivilege(189);

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

                        var sell = Math.Round(shared.GetSellPrice(Math.Abs(cost), 0, true, qty, WorkingBusinessUnit.id32), 2);
                        var extd_sell = cost < 0 ? Math.Round(qty * sell, 2) * -1 : Math.Round(qty * sell, 2);
                        var extd_cost = Math.Round(qty * cost, 2);
                        returned = string.Format(@"{{
'status': 'success',
'sell': {0},
'ext_sell': {1},
'ext_cost': {2},
'ext_per':{3}
}}", sell, extd_sell, extd_cost, 0).Replace('\'', '"');
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

                #region kitted_handler

                case "kitted_handler":
                    if (_q["id"] != null)
                    {
                        var kit_exists =
                            Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_kit_hdr WHERE inventory_kit_hdr_id = @v0 ", new object[] { _q["id"] }) == 1;
                        if (kit_exists)
                        {
                            var k_contents = Toolbox.doSQL_dt(conn, @" SELECT inventory_kit_dtl_master_id master_id, if(inventory_kit_dtl_master_id >= 990000, inventory_labor_desc(inventory_kit_dtl_master_id),full_part_description(inventory_kit_dtl_master_id, false, @v1 )) descr, inventory_kit_dtl_qty qty FROM inventory_kit_dtl WHERE inventory_kit_dtl_hdr_id = @v0  AND inventory_kit_dtl_active = true", new object[] { _q["id"], current_user.business_unit.country });
                            if (k_contents.Rows.Count > 0)
                            {
                                returned = "{\"items\":[";
                                foreach (DataRow _dr in k_contents.Rows)
                                {
                                    var master_id = Convert.ToInt32(_dr["master_id"]);
                                    returned += string.Format(@"{{
""master_id"": ""{0}"",
""description"": ""{1}"",
""qty"": ""{2}""
}},",
                                        _dr["master_id"],
                                        Toolbox.do_value_from(_dr["descr"], true),
                                        _dr["qty"]);
                                }
                                returned = returned.TrimEnd(',') + "]}";
                            }
                            else
                            {
                                returned = @"{""status"":""error"",""message"":""Kit Doesn't Contain Parts""}";
                            }
                        }
                        else
                        {
                            returned = @"{""status"":""error"",""message"":""Kit Doesn't Exist""}";
                        }
                    }
                    else
                    {
                        returned = @"{""status"":""error""}";
                    }
                    Response.Write(returned);
                    break;

                #endregion kitted_handler

                #region refactor_handler

                case "refactor_handler":
                    try
                    {
                        refactor_prices();
                        Response.Write("SUCCESS");
                    }
                    catch (Exception ee)
                    {
                        Response.Write(ee.ToString());
                    }
                    break;

                #endregion refactor_handler

                #region print_all_handler

                case "print_all_handler":
                    if (id != "")
                    {
                        try
                        {
                            if (Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM po_details_current  WHERE po_details_poprog_id =@v0", new object[] { id }) == 0)
                            {
                                throw new Exception("There are no parts to print.");
                            }
                            var query = "";
                            var _business_unit_id = Toolbox.doSQL_string(conn, @"SELECT business_unit_id FROM poprog_header  WHERE poprog_id =@v0", new object[] { id });
                            switch (origin)
                            {
                                case "purchaseorder":
                                    query =
                                        "SELECT DISTINCT(po_details_part_no) master_id FROM po_details_current WHERE po_details_poprog_id = '" + id +
                                        "'";
                                    break;
                            }
                            var i = new inventory();
                            var printable_bcs = Toolbox.doSQL_dt(conn, query, null);
                            foreach (DataRow printable_bc in printable_bcs.Rows)
                            {
                                i.Load(printable_bc["master_id"], _business_unit_id);
                                //		i.print_barcode_label(1);
                                Thread.Sleep(500);
                            }
                            Response.Write("SUCCESS");
                        }
                        catch (Exception ee)
                        {
                            Response.Write(ee.Message);
                        }
                    }
                    break;

                #endregion print_all_handler

                #region transfer_data

                case "transfer_data":
                    Toolbox.do_set_XML_header(Response);
                    if (!string.IsNullOrEmpty(_q["id"]) && !string.IsNullOrEmpty(_q["type"]))
                    {
                        var type = _q["type"];
                        id = Toolbox.do_value_from(_q["id"], false);
                        var where_clause = id.Contains(",") ? " IN (" + id + ")" : " = " + id;
                        var _dt = Toolbox.doSQL_dt(conn, string.Format(@"
SELECT 
	wo_detail_current_id line_id,
	wo_detail_current_master_id part_id, 
	wo_detail_current_qty_committed qty,
	wo_detail_current_origin origin 
FROM 
	wo_detail_current 
WHERE 
	wo_detail_current_id {0} AND wo_detail_current_qty_committed > 0", where_clause), null);
                        var sb = new StringBuilder();
                        sb.Append("<transfer_data>");
                        foreach (DataRow _dr in _dt.Rows)
                        {
                            var line_id = _dr["line_id"].ToString();
                            var part_id = _dr["part_id"].ToString();
                            var qty = _dr["qty"].ToString();
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
                    Response.Write(
                        Toolbox.doSQL_string(conn, @"SELECT IFNULL(MIN(qty),0) FROM inventory_location WHERE location_master_id = @v1  AND master_id = @v0  AND business_unit_id = @v2  LIMIT 1", new object[] { _this_mas_id, _this_loc_id, _this_com_id }));
                    break;

                #endregion location_qty

                #region block_schedule

                case "block_schedule":
                    var row_id = _q["row_id"];
                    var this_woprog_id = Convert.ToInt32(_q["woprog_id"]);
                    var blocking_schedule = Convert.ToInt32(_q["blocking_schedule"]) == 1;
                    try
                    {
                        var wodc = new NeWODetailCurrent(Convert.ToInt32(row_id));
                        wodc.qty_committed = 0;
                        wodc.qty_ordered = 0;
                        wodc.qty_invoiced = 0;
                        wodc.blocks_schedule = blocking_schedule;
                        wodc.save(current_user, "picklist.aspx - blocks_schedule", true);
                        var n_blocks =
                            Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND blocks_schedule = 1", new object[] { this_woprog_id });
                        var current_status =
                            Toolbox.doSQL_string(conn, @"SELECT woprog_status FROM woprog WHERE woprog_id = @v0 ", new object[] { this_woprog_id });

                        if (n_blocks > 0 && current_status != "Waiting for Parts") // Add Lock WO Status
                        {
                            // SET WO status
                            Toolbox.doSQL_void(conn, @"UPDATE woprog SET woprog_status = 'Waiting for Parts' WHERE woprog_id = @v0  LIMIT 1", new object[] { this_woprog_id });
                            // ADD history line
                            NeWOProg.add_history(this_woprog_id, current_user.id, "Waiting for Parts", Toolbox.MySQLNow_long());
                        }
                        else if (n_blocks == 0 && current_status == "Waiting for Parts") // Release  Lock status
                        {
                            // SET WO status
                            Toolbox.doSQL_void(conn, @"UPDATE woprog SET woprog_status = 'Open' WHERE woprog_id = @v0  LIMIT 1", new object[] { this_woprog_id });
                            // ADD history line
                            NeWOProg.add_history(this_woprog_id, current_user.id, OpsWOStatus.Open, Toolbox.MySQLNow_long());
                        }
                        Response.Write("SUCCESS");
                    }
                    catch (Exception ee)
                    {
                        Response.Write(ee.ToString());
                    }
                    break;

                    #endregion block_schedule
            }
            Response.End();
        }
        if (_q["origin"] != null && _q["id"] != null)
        {
            var _paras = Page.Request.Params.Get("__CALLBACKPARAM");
            var _line_id = "";
            var l_id = 0;
            if (!string.IsNullOrEmpty(_paras) && _paras.Contains("|"))
            {
                try
                {
                    var is_normal = _paras.Split('|').Length == 7 || _paras.Split('|').Length == 5;
                    _line_id = is_normal ? "" : _paras.Split('|').Length > 3 ? _paras.Split('|')[3].TrimEnd(';') : "";
                }
                catch
                {
                    _line_id = "";
                }
                if (!int.TryParse(_line_id, out l_id))
                {
                    _line_id = "";
                }
            }

            #region handler_setter

            switch (_q["origin"])
            {

                #region purchaseorder

                case "purchaseorder":
                    var _po_detail = new DataTable();
                    if (!string.IsNullOrEmpty(_q["id"]) && _q["id"] != "0")
                    {
                        var _po_detail_q = @"
SELECT 
	po_details_id id,
	po_details_notes notes 
FROM 
	po_details_current";
                        _po_detail_q += _line_id != ""
                            ? "\nWHERE po_details_id = " + _line_id
                            : "\nWHERE po_details_poprog_id = " + _q["id"];
                        _po_detail = Toolbox.doSQL_dt(_po_detail_q, null);
                        _po_detail.TableName = "po_detail";
                        _handlers.Tables.Add(_po_detail.Copy());
                        hidCompanyID.Value =
                            Toolbox.doSQL_string(conn, @"SELECT business_unit_id FROM poprog_header WHERE poprog_id = @v0 ", new object[] { id });

                        WorkingBusinessUnit = new NeBusinessUnit(hidCompanyID.Value);
                        WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
                        Session["warehouse_bu_id"] = WarehouseBusinessUnit.id;
                        hidWorkingBusinessUnitID.Value = WorkingBusinessUnit.id.ToString();
                        hidWarehouseBusinessUnitID.Value = WarehouseBusinessUnit.id.ToString();

                    }
                    break;

                #endregion purchaseorder

                #region quote

                case "quote":

                    var sections =
                        Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_section WHERE quote_id = @v0  AND revision = @v1 ", new object[] { _q["id"], _q["rev"] });
                    sections.TableName = "quote_section";
                    _handlers.Tables.Add(sections.Copy());
                    var line_items =
                        Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1 ", new object[] { _q["id"], _q["rev"] });
                    line_items.TableName = "quote_worksheet";
                    _handlers.Tables.Add(line_items.Copy());
                    var curr_cost_line_items =
                        Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_worksheet_current_cost WHERE quote_id = @v0  AND revision = @v1 ", new object[] { _q["id"], _q["rev"] });
                    curr_cost_line_items.TableName = "quote_worksheet_current_cost";
                    _handlers.Tables.Add(curr_cost_line_items.Copy());
                    hidCompanyID.Value =
                        Toolbox.doSQL_string(conn, @"SELECT business_unit_id FROM quote_master WHERE quote_id = @v0  LIMIT 1", new object[] { id });
                    WorkingBusinessUnit = new NeBusinessUnit(hidCompanyID.Value);
                    WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
                    Session["warehouse_bu_id"] = WarehouseBusinessUnit.id;
                    hidWorkingBusinessUnitID.Value = WorkingBusinessUnit.id.ToString();
                    hidWarehouseBusinessUnitID.Value = WarehouseBusinessUnit.id.ToString();
                    break;

                    #endregion
            }

            #endregion handler_setter
        }
        can_edit_part_cost = current_user.AuthenticatedForPrivilege(139);
        auth_new_location = current_user.AuthenticatedForPrivilege(117);
        if (!auth_new_location && origin != "Quote")
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
	a.business_unit_id = @business_unit_id AND 
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
	b.business_unit_id = @business_unit_id AND 
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
			a.master_id = @master_id AND a.business_unit_id = @business_unit_id AND b.type_id = 1
WHERE
	b.business_unit_id = @business_unit_id)
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
			b.master_id = @master_id AND b.business_unit_id = @business_unit_id
WHERE
	a.business_unit_id = @business_unit_id AND 
	a.type_id = 2)			
ORDER BY type_id ASC, qty DESC, max DESC";

            #endregion !auth_new_location
        }
        chkNewPart.Enabled = false;

        ((ASPxDateEdit)Popup_GroupSelect.FindControl("date_required")).Visible = false;
        ((Label)Popup_GroupSelect.FindControl("lb_required")).Visible = false;
        ListItem kitted_part;
        switch (origin)
        {
            #region purchaseorder

            case "purchaseorder":
                //   UPDATEPROGRESS1.Visible = false;
                var poprogress = new NePOProg(main_id);
                hidCompanyID.Value = poprogress.business_unit_id.ToString();

                WorkingBusinessUnit = new NeBusinessUnit(hidCompanyID.Value);
                WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
                Session["warehouse_bu_id"] = WarehouseBusinessUnit.id;
                hidWorkingBusinessUnitID.Value = WorkingBusinessUnit.id.ToString();
                hidWarehouseBusinessUnitID.Value = WarehouseBusinessUnit.id.ToString();

                hidVendorID.Value = poprogress.poprog_vendor_id.ToString();
                var DeptSql =
                    string.Format(@"(SELECT 0 AS Division_ID, 'N/A' AS Division_Name) UNION (SELECT id, ddl_name AS Division_Name
														FROM business_unit
														WHERE id = {0}
														AND Active = 'T'
														ORDER BY ddl_name)", hidCompanyID.Value);
                SqlDeptDataSource.SelectCommand = DeptSql;
                SqlDeptDataSource2.SelectCommand = DeptSql;
                btnPrintAll.Visible = true;
                addtoggle(new[] { 1, 7, 10, 14, 15, 16, 17, 18, 21, 22, 23, 24, 25, 26, 27 }, false);
                gridtoggle(new[] { 2, 7, 11, 12, 16, 17, 18, 21, 23, 25, 26, 28, 31, 35 }, false);
                //throw new Exception(agv.Columns[8].Caption);
                hidForceClickable.Value = "true";

                rp_Main.Visible = true;
                pnl_header.Visible = false;
                header_label.Visible = false;
                header_print.Visible = false;
                header_printwo.Visible = false;
                header_printwo_unf.Visible = false;
                rp_Main.HeaderStyle.BackColor = Color.Transparent;
                lblCusotmerName.Visible = false;
                lblCustNameDisp.Visible = false;
                txtGroupName.Visible = false;
                lblMemberName.Visible = false;
                lblMembNameDisp.Visible = false;
                txtGroupNotes.Visible = false;
                lblQuoted.Visible = false;
                lblQuoted1.Visible = false;
                lblQuotedDisp1.Visible = false;
                lblQuotedDisp.Visible = false;
                ASPxDateExpected.Text = string.IsNullOrEmpty(poprogress.poprog_expected_order_date)
                    ? ""
                    : poprogress.poprog_expected_order_date;
                lblTotalCustom.Visible = false;
                lblTotalCustomDisp.Visible = false;
                lblMargAbovCos.Visible = false;
                lblMargAbovCosDisp_dollars.Visible = false;
                lblMargAbovCos_dollars.Visible = false;
                if ((poprogress.poprog_status >= OpsPOStatus.WaitingToBeClosed || poprogress.poprog_status == OpsPOStatus.ReceivedWaitingforInvoice) && poprogress.poprog_status != OpsPOStatus.APProblems)
                {
                    agv.Columns[33].Visible = false;
                }
                //agv.Columns[9].Visible = false;
                //agv.Columns[10].Visible = false;
                agv.Columns[6].Caption = "Qty Ordered";
                agv.Columns[9].Caption = "Ord'd";
                agv.Columns[9].Width = 65;
                agv.Columns[10].Caption = "Rec'd";
                agv.Columns[10].Width = 60;
                agv.Columns[1].Caption = "Location/WO";
                agv.Columns[22].Width = 17;
                agv.Columns[14].Width = 68;
                agv.Columns[20].Width = 40;


                ddlMatType.Items.RemoveAt(1);
                kitted_part = new ListItem("Past PO", "7");
                ddlMatType.Items.Add(kitted_part);
                //	    ddlMatType.Items.Add(new ListItem("Work Orders", "6"));
                ddlMatType.Items.Add(new ListItem("Vendor RFQ", "8"));
                ddlMatType.DataBind();
                agv.Columns[24].Visible = poprogress.poprog_status == OpsPOStatus.IssuedWaitingforPackingSlip;
                //agv.Columns[5].CellStyle.Wrap = DefaultBoolean.True;
                agv.Columns[1].CellStyle.Wrap = DefaultBoolean.False;
                var c_statuses_past_notissued = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM poprogstatus where poprogstatus_poprog_id = @v0  AND poprogstatus_status NOT IN ('Not Issued', 'AP Problems')", new object[] { poprogress.poprog_id });
                pnl_addnewline.Visible = poprogress.poprog_status == OpsPOStatus.NotIssued || poprogress.poprog_status == OpsPOStatus.APProblems && c_statuses_past_notissued == 0;
                agv.Columns[19].Visible = !new List<int>(new[] { 1, 2, 4, 5, 6, 7, 8, 9 }).Contains(poprogress.poprog_status);
                var gridview = agv;
                var c = gridview.Columns[8] as GridViewDataTextColumn;
                c.PropertiesTextEdit.DisplayFormatString = "#,###.0000";
                var c2 = gridview.Columns[9] as GridViewDataTextColumn;
                c2.PropertiesTextEdit.DisplayFormatString = "#,###.0000";
                TextCost.ClientEnabled = true;
                TextDescription.Enabled = true;
                ddlMemberName.Visible = false;
                ddlMemberType.Visible = false;
                ddlLabourChargeType.Visible = false;
                lblTM_Sell.Visible = false;
                lbl_benchextddiff.Visible = false;
                lb_benchextddiff.Visible = false;
                TextSell.Visible = false;
                lblTMExtd.Text = "Ext'd";
                lblTMExtd.Visible = true;
                TextTMExtd.Visible = true;
                lblQuotedExtd.Visible = false;
                TextQuotedExtd.Visible = false;
                lblInclude.Visible = false;
                chkNewPart.Visible = false;
                lbVpic.Visible = true;
                lblDate_Expected.Visible = true;
                //ASPxDateExpected.Width = Unit.Pixel(40);
                ASPxDateExpected.Visible = true;
                lblQtyPerPart.Visible = true;
                txtQtyPerPart.Visible = true;
                // populate_sds_workorder_ddl(hidCompanyID.Value, GetGLs(hidCompanyID.Value));
                populate_sds_workorder_ddl(hidCompanyID.Value, GetExp(hidCompanyID.Value));
                fill_edit_form_workorder_ddl();
                if (poprogress.poprog_woprog_id.ToString() != "0")
                {
                    var wo = new NeWOProg(poprogress.poprog_woprog_id);
                    if (!wo.IsAdvancedStatus && !wo.IsTerminated)
                    {
                        ddlWorkOrder.Value = poprogress.poprog_woprog_id.ToString();
                    }

                    this.processingWorkOrderId = poprogress.poprog_woprog_id;
                    this.processingWorkOrderStatus = wo.Status;
                }

                try
                {
                    ASPxDateExpected.Text = poprogress.poprog_expected_received_date;
                }
                catch
                {
                }
                FillForPO(main_id);
                if (!can_receive)
                {
                    gridtoggle(new[] { 0, 1, 19 }, false);
                }

                dt_gl_tes = Toolbox.doSQL_dt("Select * from gl_te where tax_entity_id = " + WorkingBusinessUnit.tax_entity_id, null);
                dt_expense_categories = Toolbox.doSQL_dt("Select * from expense_category", null);
                dt_additional_categories = Toolbox.doSQL_dt("SELECT po_details_woprog_id,b.gl_chart_name,c.expense_category_id FROM po_details_current a LEFT JOIN gl_te b ON a.po_details_woprog_id = b.id LEFT JOIN expense_category c " +
                    " ON b.gl_chart_name = c.Name WHERE a.is_gl_account GROUP BY b.account_no HAVING c.expense_category_id IS NULL ", null);
                break;

            #endregion

            #region quote

            case "quote":
                header_print.Visible = true;
                header_printwo.Visible = false;
                header_printwo_unf.Visible = false;
                lbVpic.Visible = false;
                rp_Main.Visible = true;
                Title = "Quote worksheet for " + id;
                rp_Main.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#4682B4");
                if (
                    Toolbox.doSQL_int(conn, @"SELECT COUNT(quote_id) FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { id, rev }) != 1)
                {
                    throw new Exception("Quote Doesn't Exist");
                }
                var quote_info =
                    Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id, address_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
                var companyid = quote_info["business_unit_id"].ToString();
                var quotediscount = quote_info["quote_master_apply_discount"].ToString();
                var c_id = Convert.ToInt32(quote_info["customer_id"]);
                var a_id = Toolbox.ReturnZeroIfNull_int(quote_info["address_id"]);
                if (a_id == 0)
                {
                    a_id =
                        Toolbox.doSQL_int(conn, @"SELECT address_id FROM address WHERE address_table = 'Customer' AND address_table_id = @v0  AND address_type = 'B'", new object[] { c_id });
                }
                var division_id = Convert.ToInt32(quote_info["division_id"]);
                hidCompanyID.Value = companyid;
                WorkingBusinessUnit = new NeBusinessUnit(hidCompanyID.Value);
                WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
                Session["warehouse_bu_id"] = WarehouseBusinessUnit.id;
                hidWorkingBusinessUnitID.Value = WorkingBusinessUnit.id.ToString();
                hidWarehouseBusinessUnitID.Value = WarehouseBusinessUnit.id.ToString();

                ASPxTextDiscount.BorderStyle = BorderStyle.None;
                ASPxTextDiscount.BackColor = System.Drawing.Color.Transparent;

                chkQuoteDiscount.Visible = current_user.AuthenticatedForPrivilege(94);
                chkQuoteDiscount.Checked = quotediscount == "1";
                DiscountAmount = 0;
                hidWODiscount.Value = quotediscount == "1" ? DiscountAmount.ToString() : "0";
                ASPxTextDiscount.Text = DiscountAmount.ToString();
                hidForceClickable.Value = "";
                header_label.Text = string.Format("Worksheet for Q {0} V{1}", id, rev);
                add_bc4.Width = "75px";
                add_bc19.Width = "25px";
                add_bc2.Width = add_bc8.Width = add_bc9.Width = add_bc10.Width = add_bc13.Width = add_bc14.Width = "50px";
                agv.Settings.ShowPreview = false;
                lblVendorPartNo.Visible = false;

                ddlMatType.Items.Add(new ListItem("Kitted", "3"));
                ddlMatType.Items.Add(new ListItem("Group", "4"));
                ddlMatType.Items.Add(new ListItem("Quotes", "5"));
                ddlMatType.Items.Add(new ListItem("Work Orders", "6"));
                if (WorkingBusinessUnit.is_er)
                {
                    ddlMatType.Items.Add(new ListItem("Repair", "9"));
                }
                ddlMatType.DataBind();

                TextCost.ClientEnabled = true;
                SqlDataSource1.SelectCommand =
                    string.Format(
                        "SELECT a.id, a.section AS section, a.detail_id, (SELECT COUNT(*) FROM quote_worksheet b WHERE b.section_id = a.id) dependants FROM quote_section a WHERE a.quote_id = {0} AND a.revision = {1} AND LENGTH(TRIM(a.section)) > 0 ORDER BY section",
                        main_id, rev);
                Populate_SectionDDL(id, rev);
                if (!agv.IsEditing)
                {
                    FillforQuote(main_id, main_rev);

                }

                Populate_LabourDDLs(hidCompanyID.Value);
                dupe_check();
                break;

            #endregion

            #region groupings

            case "groupings":
                rp_Main.FindControl("ImgBtn_Print").Visible = false;
                rp_Main.FindControl("ImgBtn_PrintWO").Visible = false;
                rp_Main.HeaderStyle.BackColor = Color.LimeGreen;
                rp_Main.Visible = true;
                pnl_header.Visible = true;
                pnl_addnewline.Visible = true;
                pnl_gv.Visible = true;
                lbl_benchextddiff.Visible = false;
                lb_benchextddiff.Visible = false;
                lblsections.Visible = false;
                lblWorkOrder.Visible = false;
                hidForceClickable.Value = "";
                //grouping
                addtoggle(new[] { 1, 3, 5, 7, 8, 9, 11, 12, 13, 14, 15, 16, 18, 21, 22, 23, 24, 25, 26, 27 }, false);
                gridtoggle(
                    new[] { 1, 2, 4, 6, 8, 9, 10, 11, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 },
                    false);
                //gridtoggle(new int[] { 0, 5, 7, 12 }, true);

                add_bc4.Width = "120px";
                add_bc10.Width = "75px";
                add_bc17.Width = "30px";
                add_bc19.Width = "30px";
                kitted_part = new ListItem("Group", "4");
                ddlMatType.Items.Add(kitted_part);
                kitted_part = new ListItem("Work Orders", "6");
                ddlMatType.Items.Add(kitted_part);
                kitted_part = new ListItem("Quotes", "5");
                ddlMatType.Items.Add(kitted_part);
                ddlBillType.DataBind();
                agv.Columns[0].Width = Unit.Pixel(100);
                agv.Settings.ShowPreview = false;

                lblMargAbovCos.Visible = false;
                lblMargAbovCosDisp.Visible = false;

                lblMargAbovCosDisp_dollars.Visible = false;
                lblMargAbovCos_dollars.Visible = false;
                lblQuoted.Visible = false;
                lblQuoted1.Visible = false;
                lblQuotedDisp.Visible = false;
                lblQuotedDisp1.Visible = false;
                lblTM.Visible = false;
                lbl_benchextddiff.Visible = false;
                lblTMDisp.Visible = false;
                lblTotalItems.Visible = false;
                lblTotalCustom.Visible = false;
                lblTotalCustomDisp.Visible = false;
                lblTotalItemsDisp.Visible = false;

                lblCost.Visible = false;
                lblCostDisp.Visible = false;

                ddlWorkOrder.Visible = false;
                lblVendorPartNo.Visible = false;
                TextVendorPartNo.Visible = false;
                lblQuotedExtd.Visible = false;
                TextQuotedExtd.Visible = false;
                lblInclude.Visible = false;
                lbVpic.Visible = true;
                lblTMExtd.Visible = false;
                lbl_benchextddiff.Visible = false;
                LabelCost.Visible = false;
                lblQty.Visible = false;
                lblTM_Sell.Text = "Sell";
                TextQuotedExtd.Visible = false;
                TextTMExtd.Visible = false;
                TextSell.ClientEnabled = false;
                TextCost.Visible = false;
                TextQty.Visible = false;
                header_print.Visible = false;
                header_printwo.Visible = false;
                header_printwo_unf.Visible = false;
                lblCusotmerName.Text = "Group Name:";
                lblMemberName.Text = "Notes:";
                txtGroupNotes.Visible = true;
                txtGroupName.Visible = true;
                btnSaveHeader.Visible = true;
                if (id != "")
                {
                    txtGroupName.Text =
                        Toolbox.doSQL_string(conn, @"SELECT IFNULL(name, 'Not Set') FROM inventory_group_hdr  WHERE id=@v0", new object[] { id });
                    txtGroupNotes.Text =
                        Toolbox.doSQL_string(conn, @"SELECT IFNULL(notes, '') FROM inventory_group_hdr  WHERE id=@v0", new object[] { id });
                    header_label.Text = "Group Name: (" + id + ") " + txtGroupName.Text;
                    FillforGroupings(main_id);
                }
                else
                {
                    pnl_addnewline.Visible = false;
                    pnl_gv.Visible = false;
                    header_label.Text = "Group Name: Not Named Yet";
                }
                agv.KeyFieldName = "id";
                agv.FindControl("");
                break;

            #endregion

            #region alternates

            case "alternates":
                rp_Main.FindControl("ImgBtn_Print").Visible = false;
                rp_Main.FindControl("ImgBtn_PrintWO").Visible = false;
                rp_Main.HeaderStyle.BackColor = Color.LimeGreen;
                rp_Main.Visible = true;
                pnl_header.Visible = false;
                pnl_addnewline.Visible = true;
                pnl_gv.Visible = true;
                hidForceClickable.Value = "";
                ddlMatType.Visible = false;
                lblsections.Visible = false;
                lblWorkOrder.Visible = false;
                lblPartType.Visible = false;
                lbl_benchextddiff.Visible = false;
                lb_benchextddiff.Visible = false;
                agv.Columns[0].Width = Unit.Pixel(100);
                addtoggle(new[] { 1, 3, 5, 7, 8, 9, 11, 12, 13, 14, 15, 16, 18, 21, 22, 23, 24, 25, 26, 27 }, false);
                gridtoggle(
                    new[] { 1, 2, 4, 6, 8, 9, 10, 11, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 },
                    false);
                add_bc4.Width = "120px";
                add_bc10.Width = "75px";
                add_bc17.Width = "30px";
                add_bc19.Width = "30px";
                agv.Settings.ShowPreview = false;

                lblMargAbovCos.Visible = false;
                lblMargAbovCosDisp.Visible = false;
                lblMargAbovCosDisp_dollars.Visible = false;
                lblMargAbovCos_dollars.Visible = false;
                lblQuoted.Visible = false;
                lblQuoted1.Visible = false;
                lblQuotedDisp1.Visible = false;
                lblQuotedDisp.Visible = false;
                lblTM.Visible = false;
                lbl_benchextddiff.Visible = false;
                lblTMDisp.Visible = false;
                lblTotalItems.Visible = false;
                lblTotalCustom.Visible = false;
                lblTotalCustomDisp.Visible = false;
                lblTotalItemsDisp.Visible = false;

                lblCost.Visible = false;
                lblCostDisp.Visible = false;
                ddlWorkOrder.Visible = false;
                lblVendorPartNo.Visible = false;
                TextVendorPartNo.Visible = false;
                lblQuotedExtd.Visible = false;
                TextQuotedExtd.Visible = false;
                lblInclude.Visible = false;
                TextDescription.Enabled = true;
                TextCost.Visible = false;
                CheckBoxInclude.Visible = false;
                chkNewPart.Visible = false;
                lblNewLine.Visible = false;
                chkNewPart.Visible = false;
                lbVpic.Visible = true;
                lblTMExtd.Visible = false;
                lbl_benchextddiff.Visible = false;
                LabelCost.Visible = false;
                lblQty.Visible = false;
                lblTM_Sell.Text = "Sell";
                TextQuotedExtd.Visible = false;
                TextTMExtd.Visible = false;
                TextPartNo.Visible = true;
                lblMatDescription.Visible = true;
                btn_Save.Visible = true;
                TextSell.Visible = true;
                TextSell.ClientEnabled = false;
                TextCost.Visible = false;
                btn_Clear.Visible = true;
                TextQty.Visible = false;
                header_print.Visible = false;
                header_printwo.Visible = false;
                header_printwo_unf.Visible = false;
                header_label.Text = "Alternates: ";
                agv.KeyFieldName = "part_no";
                agv.FindControl("");
                header_label.Text = "Alternates for: " + Toolbox.doSQL_string(conn, @"SELECT full_part_description(inventory_item_master.master_id,1,@v1) FROM inventory_item_master WHERE inventory_item_master.master_id=@v0", new object[] { id, NeBusinessUnit.GetbuCountry(current_user.business_unit_id.ToString()) });
                FillforAlternates(main_id);
                break;

            #endregion

            #region kitted

            case "kitted":
                lblCost.Visible = false;
                lblCostDisp.Visible = false;

                lblMargAbovCos.Visible = false;
                lblMargAbovCosDisp.Visible = false;
                lblMargAbovCosDisp_dollars.Visible = false;
                lblMargAbovCos_dollars.Visible = false;
                rp_Main.FindControl("ImgBtn_Print").Visible = false;
                rp_Main.FindControl("ImgBtn_PrintWO").Visible = false;
                rp_Main.HeaderStyle.BackColor = Color.LimeGreen;
                rp_Main.Visible = true;
                lbl_benchextddiff.Visible = false;
                lb_benchextddiff.Visible = false;
                pnl_header.Visible = true;
                pnl_addnewline.Visible = true;
                pnl_gv.Visible = true;
                txtGroupName.Visible = true;
                txtGroupNotes.Visible = true;
                btnSaveHeader.Visible = true;
                TextSell.ClientEnabled = false;
                TextCost.Visible = false;
                btn_Clear.Visible = true;
                header_print.Visible = false;
                header_printwo.Visible = false;
                header_printwo_unf.Visible = false;
                hidForceClickable.Value = "";
                addtoggle(new[] { 1, 3, 5, 7, 9, 11, 12, 14, 14, 15, 16, 18, 21, 22, 23, 24, 25, 26, 27 }, false);
                ddlMatType.Items.Add(new ListItem("Group", "4"));
                ddlMatType.Items.Add(new ListItem("Quotes", "5"));
                ddlMatType.Items.Add(new ListItem("Work Orders", "6"));
                ddlMatType.Items.Add(new ListItem("Purchase Orders", "7"));
                ddlMatType.DataBind();
                gridtoggle(new[] { 1, 2, 4, 8, 10, 11, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 },
                    false);
                agv.Columns[0].Width = Unit.Pixel(100);
                agv.Settings.ShowPreview = false;
                agv.KeyFieldName = "part_no";
                lblCusotmerName.Text = "Kitted Name: ";
                lblMemberName.Text = "Notes: ";
                if (id != "")
                {
                    txtGroupName.Text =
                        Toolbox.doSQL_string(conn, @"SELECT inventory_kit_hdr_name FROM inventory_kit_hdr WHERE inventory_kit_hdr_id=@v0 ", new object[] { id });
                    header_label.Text = txtGroupName.Text;
                    header_label.Text = string.Format("Kitted Name: {0}", header_label.Text);
                    txtGroupNotes.Text =
                        Toolbox.doSQL_string(conn, @"SELECT inventory_kit_hdr_note FROM inventory_kit_hdr WHERE inventory_kit_hdr_id=@v0 ", new object[] { id });
                    FillforKitted(main_id);
                }
                else
                {
                    header_label.Text = "Kitted Part: Not Named Kitted Part";
                    pnl_addnewline.Visible = false;
                    pnl_gv.Visible = false;
                }
                break;

            #endregion

            #region rfq

            case "rfq":
                //pnl_header.Visible					= false;
                header_print.Visible = false;
                header_printwo.Visible = false;
                header_printwo_unf.Visible = false;
                lblerrorLabel.Visible = false;
                add_lc1.Visible = false;
                agv.Settings.ShowPreview = false;
                lblQty.Text = "QTY";
                header_label.Text = "RFQ Line Items";
                ddlMatType.Items.RemoveAt(1);
                ddlMatType.Items.Add(new ListItem("Group", "4"));
                ddlMatType.Items.Add(new ListItem("Quotes", "5"));
                ddlMatType.Items.Add(new ListItem("Work Orders", "6"));
                ddlMatType.Items.Add(new ListItem("Purchase Orders", "7"));
                ddlMatType.DataBind();
                lbl_benchextddiff.Visible = false;
                lb_benchextddiff.Visible = false;
                FillForRFQ(main_id);
                break;

                #endregion rfq
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        loading_time.InnerHtml = "";
        var startCounter = DateTime.Now.TimeOfDay.TotalSeconds;
        Toolbox.do_add_css(Page, "/css/picklist.css");
        Toolbox.do_add_css(Page, "/css/jquery.tip.css");
        Toolbox.do_add_css(Page, "/css/jquery_custom_mods.css");
        Toolbox.do_add_css(Page, "/css/autocomplete.css");
        lb_currentfilter.Visible = !string.IsNullOrEmpty(_q["section_id"]);
        div_refactor.Style.Add("display", "none");

        Session.Add("member_id", current_user.id.ToString());
        var iframe = _q["iframe"] != null ? _q["iframe"] : "";

        //		TextCost .Attributes.Add("onkeydown", "only_numeric(event);");
        TextSell.Attributes.Add("onkeydown", "only_numeric(event);");
        txtQtyPerPart.Attributes.Add("onkeydown", "only_numeric(event);");
        TextQty.Attributes.Add("onkeydown", "only_numeric(event);");
        TextCost.ClientSideEvents.KeyDown = "cost_keydown";
        TextCost.ClientSideEvents.TextChanged = "cost_handler";

        //TextCost.Attributes.Add("onblur", "cost_handler(this)");
        if (origin != "workorder" && origin != "quote" && origin != "purchaseorder")
        {
            hidCompanyID.Value = current_user.business_unit_id.ToString();

            WorkingBusinessUnit = new NeBusinessUnit(hidCompanyID.Value);
            WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
            Session["warehouse_bu_id"] = WarehouseBusinessUnit.id;
            hidWorkingBusinessUnitID.Value = WorkingBusinessUnit.id.ToString();
            hidWarehouseBusinessUnitID.Value = WarehouseBusinessUnit.id.ToString();
        }
        if (!current_user.AuthenticatedForPrivilege(60))
        {
            TextSell.ClientVisible = false;
            TextTMExtd.ClientVisible = false;
        }

        fill_edit_form_workorder_ddl();
        if (origin != "quote")
        {
            lblTotalLabor.Visible =
                lblTotalMaterial.Visible =
                    lblTotalMaterialDisp.Visible =
                        lblTotalLaborDisp.Visible = lblTotalQuotedLaborDisp.Visible = lblTotalQuotedLabor.Visible = false;
        }
        var event_target = Page.Request.Params.Get("__EVENTTARGET");
        //show_debug_column_info();
        if (id == "" && !Toolbox.Contains(origin, new[] { "kitted", "groupings" }))
        {
            Toolbox.FriendlyException(Response, "Not a valid request", "");
        }
        if (!IsPostBack && !cbp_sections.IsCallback)
        {
            clear_boxes(true);
            if (add_lc1 != null)
            {
                add_lc1.Width = "0";
                add_lc1.Height = "0";
                add_lc1.Visible = false;
            }

            if (iframe != "yes")
            {
                ScriptManager1.SetFocus(TextPartNo.ClientID);
            }
            chkApplyDiscount.Visible = current_user.AuthenticatedForPrivilege(94);
        }

        #region If the gridview was the control, refresh the page, otherwise don't

        var callback_param = Page.Request.Params.Get("__CALLBACKPARAM");
        if (string.IsNullOrEmpty(callback_param))
        {
            callback_param = "";
        }
        if (event_target != null && !cbp_sections.IsCallback)
        {
            ctrlname = Page.FindControl(event_target);
            if (ctrlname != null)
            {
                //if ((ctrlname.ID != "ddlMatType") && (ctrlname.ID != "ddlMemberName") && (ctrlname.ID != "ddlMemberType") && (ctrlname.ID != "ddlLabourChargeType") && (ctrlname.ID != "TextPartNo") && (ctrlname.ID != "TextQty") && (ctrlname.ID != "TextCost") && (ctrlname.ID != "ddlSection"))
                //	{
                if (agv.EditingRowVisibleIndex == -1) // If we aren't editing.
                {
                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "window.opener.location.reload();", true);
                    switch (origin)
                    {
                        case "quote":
                            FillforQuote(main_id, main_rev);
                            break;
                        case "purchaseorder":
                            FillForPO(main_id);
                            break;
                        case "kitted":
                            FillforKitted(main_id);
                            break;
                        case "groupings":
                            FillforGroupings(main_id);
                            break;
                        case "alternates":
                            FillforAlternates(main_id);
                            break;
                        case "rfq":
                            FillForRFQ(main_id);
                            break;
                    }
                }
                //	}
            }
            else if (main_rev != 0 || main_id != 0)
            {
                var control = GetAsyncPostBackControlID();
                switch (origin)
                {
                    case "quote":

                        //if (!IsCallback)
                        //    {
                        FillforQuote(main_id, main_rev);
                        //    }
                        break;
                    case "purchaseorder":
                        if (
                            !control.Contains("btnUpdatePORecQty")
                            )
                        {
                            FillForPO(main_id);
                        }
                        break;
                    case "kitted":
                        FillforKitted(main_id);
                        break;
                    case "groupings":
                        FillforGroupings(main_id);
                        break;
                    case "alternates":
                        FillforAlternates(main_id);
                        break;
                    case "rfq":
                        FillForRFQ(main_id);
                        break;
                }
            }
        }

        #endregion

        set_header_widths();

        header_printwo.Attributes["onclick"] =
            string.Format("boing('/sections/workorder/wo_shopping_cart.aspx?woid={0}', 'print', 750, 960);", main_id);
        header_printwo_unf.Attributes["onclick"] =
            string.Format("boing('/sections/workorder/wo_shopping_cart.aspx?woid={0}&unf=1', 'print', 750, 960);", main_id);
        header_print.Attributes["onclick"] =
            string.Format(
                "boing('/sections/reports/print_quote_worksheet/index.aspx?origin={0}&quoteid={1}&revision={2}&stuff=85&Q={3}&T={4}', 'print', 1024, 960);",
                origin, main_id, rev, lblQuotedDisp.Text, lblTMDisp.Text);


    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        conn.Dispose();
    }

    private void d_note(bool start, string extra)
    {
        /*
		StackTrace st = new StackTrace ();
		StackFrame sf = st.GetFrame (1);
		MethodBase currentMethodName = sf.GetMethod();
		string suff		= start ? " Start:"+extra+"\n" : " End:"+extra+"\n";
		Debug.Write(currentMethodName.Name+" "+DateTime.Now.ToString("HH:mm:ss.ffff")+suff);
		 */
    }

    private DataTable dt_master_locations;
    private string _wo_status = "";
    private bool _process_locations = true;

    private void set_location_ds(object sender, EventArgs e)
    {
        if (origin == "quote") return;
        var grid = (ASPxGridView)sender;
        var workorder_column = (GridViewDataColumn)grid.Columns["workorder"];
        var start = grid.VisibleStartIndex;
        var end = grid.VisibleRowCount <= 25 ? grid.VisibleRowCount : (grid.PageIndex + 1) * grid.SettingsPager.PageSize;
        if (end > grid.VisibleRowCount)
        {
            end = grid.VisibleRowCount;
        }
        //if(excludes == null || IsCallback)
        //	{
        var part_numbers = new List<string>();
        for (var wi = start; wi < end; wi++)
        {
            var part_nu = grid.GetRowValues(wi, "part_no").ToString() == ""
                ? 0
                : Convert.ToInt32(grid.GetRowValues(wi, "part_no").ToString());
            if (part_nu < 900000 && part_nu != 0)
            {
                part_numbers.Add(grid.GetRowValues(wi, "part_no").ToString());
            }
        }
        if (part_numbers.Count > 0)
        {
            dt_excludes =
                Toolbox.doSQL_dt(conn, string.Format(@"SELECT a.master_id, b.is_exclude, b.allowed_to_stock,  a.tag_id, b.is_rental  
FROM inventory_item_master a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id WHERE a.master_id IN ({0})", string.Join(",", part_numbers.ToArray())), null);
        }
        //	}
        if ((dt_master_locations == null || IsCallback || is_adding) && part_numbers.Count > 0)
        {
            var woprog_status = origin == "workorder"
                ? Toolbox.doSQL_string(conn, @"SELECT woprog_status FROM woprog WHERE woprog_id = @v0  LIMIT 1", new object[] { main_id })
                : "";
            var csv_parts = string.Join(",", part_numbers.ToArray());
            if (origin == "workorder" && (woprog_status == OpsWOStatus.Invoiced || woprog_status == OpsWOStatus.WaitingToBeInvoiced))
            {
                _wo_status = woprog_status;
                _process_locations = false;
            }
            if (_process_locations)
            {

                dt_master_locations = auth_new_location && origin == "workorder"
                     ? Toolbox.doSQL_dt(conn, string.Format(
                         @" SELECT a.master_id, b.id, CONCAT(IF(b.type_id = 1, 'In - ', 'Ex - '), b.name,' - (', IFNULL(c.qty,0), ')') name, 
IFNULL(c.qty, 0) qty, b.type_id, b.name rawname FROM inventory_item_master a LEFT JOIN inventory_location_master b ON b.business_unit_id = @v0 
LEFT JOIN inventory_location c ON a.master_id = c.master_id AND b.id = c.location_master_id WHERE a.master_id IN ({0}) GROUP BY master_id, id ORDER BY master_id,id", csv_parts),
                         new object[] { WarehouseBusinessUnit.id })
                     : auth_new_location && origin == "purchaseorder"
                         ? Toolbox.doSQL_dt(conn, string.Format(
                             @" SELECT d.master_id, d.id, CONCAT(IF(d.type_id = 1, 'In - ', 'Ex - '), d.name,' - (', IFNULL(c.qty,0), ')') name, 
IFNULL(c.qty, 0) qty, d.type_id, d.rawname FROM ( SELECT a.master_id, b.id, b.type_id, b.name, b.name rawname
FROM inventory_item_master a, inventory_location_master b 
WHERE a.master_id IN ({0}) 
AND b.business_unit_id = @v0  
GROUP BY master_id, id 
ORDER BY master_id ASC,b.type_id ASC,id ASC ) d 
LEFT JOIN (SELECT * FROM inventory_location WHERE master_id IN ({0}) 
AND business_unit_id = @v0 ) c 
ON d.id = c.location_master_id
AND d.master_id = c.master_id ", csv_parts),
                             new object[] { WarehouseBusinessUnit.id })

                         : Toolbox.doSQL_dt(conn, string.Format(
                             @" (SELECT IFNULL(a.master_id, 0) master_id, b.id, 
CONCAT(IF(b.type_id = 1, 'In - ', 'Ex - '), CONCAT(b.name),' (',IFNULL(a.qty,0),')') name, 
IFNULL(a.qty, 0) qty, b.type_id, b.name rawname FROM inventory_location a 
LEFT JOIN inventory_location_master b ON b.id = a.location_master_id 
AND a.master_id IN ({0}) 
AND a.business_unit_id = @v0  
AND b.type_id = 1 WHERE b.business_unit_id = @v0  
AND IFNULL(a.master_id, 0) != 0 order by a.max,a.min DESC, b.name ) 
UNION (SELECT IFNULL(b.master_id, 0) master_id, a.id, 
CONCAT(IF(a.type_id = 1, 'In - ', 'Ex - '), CONCAT(a.name),' (',IFNULL(b.qty,0),')') name,
IFNULL(b.qty, 0) qty, a.type_id, a.name rawname 
FROM inventory_location_master a 
LEFT JOIN inventory_location b 
ON a.id = b.location_master_id 
AND b.master_id IN ({0}) 
AND b.business_unit_id = @v0  
WHERE a.business_unit_id = @v0  
AND a.type_id = 2 AND IFNULL(b.master_id, 0) != 0
order by b.max,b.min DESC, a.name )", csv_parts),
         new object[] { WarehouseBusinessUnit.id });

            }
        }
        for (var i = start; i < end; i++)
        {
            var part_no = 0;
            if (grid.GetRowValues(i, "part_no") != null)
            {
                int.TryParse(grid.GetRowValues(i, "part_no").ToString(), out part_no);
            }

            var ds_locations = (SqlDataSource)grid.FindRowCellTemplateControl(i, workorder_column, "ds_locations");
            var location_combo = (ASPxComboBox)grid.FindRowCellTemplateControl(i, workorder_column, "combo_location");
            var workorder_link = (HtmlContainerControl)grid.FindRowCellTemplateControl(i, workorder_column, "wo_link");
            if (origin == "purchaseorder" && part_no > 0)
            {
                #region purchase order

                //string location_id		= agv.GetRowValues(e.VisibleIndex, "section_id").ToString();
                //string location_name	= agv.GetRowValues(e.VisibleIndex, "workorder").ToString();
                //e.Cell.Text				= Toolbox.do_value_from(location_name, false);
                //e.Cell.Text					= e.Cell.fun
                //workorder_link.Visible			= false;
                var rec_column = (GridViewDataColumn)agv.Columns["emptyval"];
                double qty_ordered = 0;
                double.TryParse(grid.GetRowValues(i, "qty").ToString(), out qty_ordered);
                double qty_recvd = 0;
                double.TryParse(grid.GetRowValues(i, "emptyval").ToString(), out qty_recvd);
                var line_active = grid.GetRowValues(i, "active").ToString() == "1";
                if (line_active)
                {
                    if (part_no < 900000 && rec_column.Visible)
                    {
                        if (dt_excludes != null && dt_excludes.Rows.Count > 0 && location_combo != null)
                        {
                            var dr = dt_excludes.Select("master_id = '" + part_no + "'");
                            location_combo.RenderIFrameForPopupElements = DefaultBoolean.True;
                            location_combo.CallbackPageSize = 10;
                            location_combo.DropDownRows = 10;
                            if (dr.Length > 0 && dr[0].ItemArray.Length > 0 && dr[0]["is_exclude"].ToString() == "0")
                            {
                                var default_location =
                                    Toolbox.doSQL_int(conn, @"SELECT IFNULL(location_master_id, 0) FROM poprog_header  WHERE poprog_id =@v0", new object[] { id });

                                location_combo.ValueField = "id";
                                location_combo.TextField = "name";
                                var location_set = false;
                                if (auth_new_location)
                                {
                                    var this_length =
                                        dt_master_locations.Select("master_id = '" + part_no + "' OR master_id = 0",
                                            "type_id ASC,qty DESC, rawname ASC").Length;
                                    if (this_length > 0)
                                    {
                                        location_combo.DataSourceID = "";
                                        location_combo.DataSource =
                                            dt_master_locations.Select("master_id = '" + part_no + "' OR master_id = 0",
                                                "type_id ASC,qty DESC, rawname ASC").CopyToDataTable();
                                        location_combo.DataBind();
                                        location_combo.Native = this_length <= 10;
                                    }
                                }
                                else
                                {
                                    var this_length =
                                        dt_master_locations.Select("master_id = '" + part_no + "'", "type_id ASC,qty DESC, rawname ASC").Length;
                                    if (this_length > 0)
                                    {
                                        location_combo.DataSourceID = "";
                                        location_combo.DataSource =
                                            dt_master_locations.Select("master_id = '" + part_no + "'", "type_id ASC,qty DESC, rawname ASC")
                                                .CopyToDataTable();
                                        location_combo.DataBind();
                                        location_combo.Native = this_length <= 10;
                                    }
                                }

                                #region Defaulting selected index

                                // Select the default if it exists
                                try
                                {
                                    location_combo.SelectedIndex = default_location == 0
                                        ? 0
                                        : location_combo.Items.FindByValue(default_location).Index;
                                    location_set = default_location != 0;
                                }
                                catch (Exception ee)
                                {
                                    location_combo.SelectedIndex = 0;
                                    location_set = false;
                                    //throw new Exception("Setting the locations for part #:"+part_no+" failed."+default_location);

                                }
                                if (!location_set)
                                {
                                    // Default to the location with a min/max
                                    var c = Toolbox.doSQL_int(conn, @" SELECT COUNT(*) FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.master_id = @v0  AND a.business_unit_id = @v1  AND b.type_id = 1", new object[] { part_no, WarehouseBusinessUnit.id });
                                    if (c > 0)
                                    {
                                        default_location = Toolbox.doSQL_int(conn, @" SELECT b.id FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.master_id = @v0  AND a.business_unit_id = @v1  AND b.type_id = 1 ORDER BY a.max desc, a.min desc, b.name LIMIT 1", new object[] { part_no, WarehouseBusinessUnit.id });
                                        if (location_combo.Items.FindByValue(default_location) != null)
                                        {
                                            location_combo.SelectedIndex = default_location == 0
                                                ? 0
                                                : location_combo.Items.FindByValue(default_location).Index;
                                        }
                                        else
                                        {
                                            location_combo.SelectedIndex = 0;
                                        }
                                    }
                                }

                                #endregion Defaulting selected index
                            }
                            else if (location_combo != null)
                            {
                                location_combo.Visible = false;
                            }
                        }
                    }
                    else if (location_combo != null)
                    {
                        location_combo.Visible = false;
                    }
                }
                else if (location_combo != null)
                {
                    location_combo.Visible = false;
                }

                #endregion purchase order
            }
            else if (location_combo != null)
            {
                location_combo.Visible = false;
            }
        }
    }

    protected void set_header_widths()
    {
        add_bc2.Width = "45px";
        add_bc18.Width = "60px";
        add_bc16.Width = "35px";
        add_bc19.Width = "22px";
        add_bc25.Width = "25px";
        add_bc26.Width = "85px";
        add_bc4.Width = "50px";
        add_bc11.Width = "85px";
        if (origin == "purchaseorder")
        {
            add_hc3.Width = "100px";
            add_hc5.Width = "150px";
            add_hc21.Width = "100px";
        }
        ddlBillType.Width = Unit.Pixel(75);
        add_bc23.Width = "35px";
        add_bc6.Style.Add("min-width", "50px");
        add_bc17.Width = add_bc19.Width = "25px";
        add_bc8.Width = add_bc9.Width = add_bc10.Width = add_bc13.Width = add_bc24.Width = "48px";
        add_bc22.Width = "40px";
        add_bc27.Width = "70px";
    }

    public string GetAsyncPostBackControlID()
    {
        var smUniqueId = ScriptManager.GetCurrent(Page).UniqueID;
        var smFieldValue = Request.Form[smUniqueId];

        if (!string.IsNullOrEmpty(smFieldValue) && smFieldValue.Contains("|"))
        {
            return smFieldValue.Split('|')[1];
        }

        return string.Empty;
    }

    protected void addtoggle(int[] col_n, bool show)
    {
        if (col_n.Length == 0)
        {
            for (var i = 1; i < 27; i++)
            {
                FindControlRecursive(Page, string.Format("add_hc{0}", i)).Visible = show;
                FindControlRecursive(Page, string.Format("add_bc{0}", i)).Visible = show;
            }
        }
        for (var i = 0; i < col_n.Length; i++)
        {
            try
            {
                FindControlRecursive(Page, string.Format("add_hc{0}", col_n[i])).Visible = show;
            }
            catch
            {
                throw new Exception(col_n[i].ToString());
            }
            FindControlRecursive(Page, string.Format("add_bc{0}", col_n[i])).Visible = show;
        }
    }

    private void footer_save_toggle(bool show_client)
    {
        footer_save_toggle(show_client, agv);
    }

    private void footer_save_toggle(bool show_client, ASPxGridView gv)
    {
        var button_id = show_client ? "btnUpdatePORecQty_client" : "btnUpdatePORecQty";
        var gvdc = (GridViewDataColumn)gv.Columns[19];
        var footer_save = (ASPxButton)gv.FindFooterCellTemplateControl(gvdc, button_id);
        if (footer_save != null)
        {
            if (show_client)
            {
                var p = new NePOProg(main_id);
                button_id = "btnUpdatePORecQty_client";
            }
            which_footer_save.Value = button_id;
        }
    }

    protected void gridtoggle(int[] col_n, bool show)
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
            agv.Columns[col_n[i]].Visible = show;
        }
    }

    protected void gridtoggle(string[] cols, bool show)
    {
        if (cols.Length == 0)
        {
            for (var i = 0; i < agv.Columns.Count; i++)
            {
                agv.Columns[i].Visible = show;
            }
        }
        for (var i = 0; i < cols.Length; i++)
        {
            agv.Columns[cols[i]].Visible = show;
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
        var master_id = 0;
        is_adding = true;
        switch (origin)
        {
            #region Groupings

            case "groupings":
                master_id = 0;
                try
                {
                    master_id = Convert.ToInt32(TextPartNo.Text);
                }
                catch
                {
                    update_error("You can only insert valid part numbers into groups");
                }
                if (checkinv.part_exists(master_id) || ddlMatType.SelectedValue == "2")
                {
                    if (
                        Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_group_dtl WHERE group_id = @v0  AND master_id = @v1 ", new object[] { main_id, master_id }) > 0)
                    {
                        update_error("This part already exists in this group.");
                    }
                    else
                    {
                        Toolbox.doSQL_void(conn, @"INSERT INTO inventory_group_dtl (group_id, master_id, inventory_group_dtl_notes,inventory_group_dtl_memberid,dt) VALUES (@v0 ,@v1 ,@v2 ,@v3 ,now())", new object[] { main_id, TextPartNo.Text, hidNotes.Value, current_user.id });
                        TextPartNo.Text = "";
                        TextDescription.Text = "";
                        TextSell.Text = "0.00";
                        TextDescription.Focus();
                        hidNotes.Value = "";
                        FillforGroupings(main_id);
                        clear_boxes(false);
                        return;
                    }
                }
                else
                {
                    update_error("You can only insert valid part numbers into groups");
                    TextDescription.Text = "";
                    TextDescription.Focus();
                    return;
                }
                break;

            #endregion

            #region Alternates

            case "alternates":
                try
                {
                    master_id = Convert.ToInt32(TextPartNo.Text);
                }
                catch
                {
                    update_error("You can only use valid alternate part numbers");
                }
                if (checkinv.part_exists(master_id))
                {
                    if (Toolbox.doSQL_int(conn, @" SELECT COUNT(*) FROM inventory_alternate WHERE inventory_alternate_origin_master_id = @v0  AND inventory_alternate_alternate_master_id = @v1 ", new object[] { main_id, master_id }) > 0)
                    {
                        update_error("This part already exists as an alternate for " + master_id);
                    }
                    else
                    {
                        Toolbox.doSQL_void(conn, @" INSERT INTO inventory_alternate ( inventory_alternate_origin_master_id, inventory_alternate_alternate_master_id, inventory_alternate_memberid, inventory_alternate_notes ) VALUES ( @v0 , @v1 , @v2 , @v3  )", new object[] { main_id, TextPartNo.Text, current_user.id, hidNotes.Value });
                        var _alternate_exists =
                            Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_alternate WHERE inventory_alternate_origin_master_id = @v0  AND inventory_alternate_alternate_master_id = @v1 ", new object[] { TextPartNo.Text, main_id });
                        if (_alternate_exists == 0)
                        {
                            Toolbox.doSQL_void(conn, @" INSERT INTO inventory_alternate ( inventory_alternate_origin_master_id, inventory_alternate_alternate_master_id, inventory_alternate_memberid, inventory_alternate_notes ) VALUES ( @v0 , @v1 , @v2 , @v3  )", new object[] { TextPartNo.Text, main_id, current_user.id, hidNotes.Value });
                        }
                        TextPartNo.Text = "";
                        TextDescription.Text = "";
                        TextSell.Text = "0.00";
                        hidNotes.Value = "";
                        FillforAlternates(main_id);
                    }
                }
                else
                {
                    update_error("You can only insert valid part numbers into alternates");
                    TextDescription.Text = "";
                }
                break;

            #endregion

            #region RFQ

            case "rfq":
                var pl = new rfq_part_list();
                pl.rfq_header_id = main_id;
                pl.master_id = Convert.ToInt32(TextPartNo.Text);
                pl.qty = Convert.ToDouble(TextQty.Text);
                pl.note = hidNotes.Value;
                pl.required_date = DateRequired.Date;
                pl.save();
                clear_boxes(true);
                FillForRFQ(main_id);
                break;

            #endregion RFQ

            #region Quote/Work order/purchase order/kitted

            case "workorder":
            case "quote":
            case "purchaseorder":
            case "kitted":

                #region Part Number

                newpartnumber = TextPartNo.Text;
                if (newpartnumber == string.Empty && origin != "quote" && origin != "kitted" && origin != "purchaseorder" &&
                    ddlMatType.SelectedValue != "9" && ddlLabourChargeType.Visible == false)
                {
                    if (ddlBillType.Value.ToString() != "6")
                    {
                        update_error("Please Make Sure You Have A Part Number");
                        return;
                    }
                }
                if (origin == "purchaseorder")
                {
                    var poprog = new NePOProg(main_id);
                    var c_statuses_past_notissued = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM poprogstatus where poprogstatus_poprog_id = @v0  AND poprogstatus_status NOT IN ('Not Issued', 'AP Problems')", new object[] { main_id });
                    if (poprog.poprog_status != OpsPOStatus.NotIssued && (poprog.poprog_status == OpsPOStatus.APProblems && c_statuses_past_notissued != 0))
                    {
                        throw new Exception("You cannot add a line to a PO that has been issued.");
                    }
                }
                var partno = 0;
                var wo_n = 0;
                if (ddlWorkOrder.SelectedItem != null && ddlWorkOrder.SelectedItem.Value != null)
                {
                    int.TryParse(ddlWorkOrder.SelectedItem.Value.ToString(), out wo_n);
                }
                if (newpartnumber != string.Empty && ddlMatType.SelectedValue != "9" && int.TryParse(newpartnumber, out partno))
                {
                    if (!checkinv.part_exists(newpartnumber) && partno < 990000)
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
                if (ddlMatType.SelectedValue == "9" && wo_n != 0)
                {
                    var ico = new consignment(wo_n);
                    newpartnumber = ico.master_id.ToString();
                    partno = ico.master_id;
                }
                else if (ddlMatType.SelectedValue == "9" && wo_n != 0)
                {
                    update_error("There Is An Issue With This Part Number.");
                    return;
                }
                if (checkinv.part_exists(newpartnumber))
                {
                    checkinv.Load(newpartnumber, WarehouseBusinessUnit.id);
                }
                if (origin == "purchaseorder" &&
                    Toolbox.Contains(wo_n, new[] { 9999999, 9999998, 9999997 }) &&
                    (checkinv.is_exclude || partno == OpsSpecialPart.SubContractor)
                    )
                {
                    update_error("You cannot purchase an inventory exclude, please talk to finance.");
                    return;
                }

                #endregion Part Number

                #region Vendor Part Number

                newvendorpartnumber = TextVendorPartNo.Text;
                if (newvendorpartnumber.Length > 34)
                {
                    update_error("You Cannot Have a Vendor Part Number more than 34 characters int");
                    ScriptManager1.SetFocus(TextVendorPartNo);
                    return;
                }

                #endregion Vendor Part Number

                #region Section ID

                if (ddlSection.Visible && origin == "quote")
                {
                    newsectionid = ddlSection.SelectedItem != null && ddlSection.SelectedItem.Value != null
                        ? ddlSection.SelectedItem.Value.ToString()
                        : "0";
                    if (newsectionid == "0")
                    {
                        update_error("Please Select A Section");
                        return;
                    }
                }

                #endregion Section ID

                #region Work Order ID

                try
                {
                    new_is_gl_account = string.IsNullOrEmpty(hidWo.Value) ? false : true;
                    //new_is_gl_account = ddlWorkOrder.Text.StartsWith("GL");
                    newworkorderid = ddlWorkOrder.Value != null ? ddlWorkOrder.Value.ToString() : "0";
                }
                catch
                {
                    newworkorderid = "0";
                }

                #endregion Work Order ID

                #region Description

                if (partno == 777)
                {
                    newdescription = TextDescription.Text;
                }
                else if (newpartnumber != "" && partno < 990000)
                {
                    checkinv.Load(partno, WarehouseBusinessUnit.id);
                    if (origin == "purchaseorder" && checkinv.is_exclude)
                    {
                        newdescription = TextDescription.Text;
                    }
                    else if ((origin == "workorder" || origin == "quote") && checkinv.is_exclude)
                    {
                        newdescription = TextDescription.Text;
                    }
                    else if (ddlMatType.SelectedValue == "9")
                    {
                        newdescription = TextDescription.Text;
                    }
                    else
                    {
                        newdescription = checkinv.description_full;
                    }
                }
                else if (newpartnumber != "" && partno >= 990000 && ddlMemberName.Visible && partno < 2000000)
                {
                    //This is a Work Order and a specific member is being
                    //added, it has a part number.
                    newdescription = GetLabourDescription();
                }
                else if (newpartnumber != "" && ddlMatType.SelectedValue == "2")
                {
                    //This is a quote and a member type is being added and it
                    //has a part number
                    NeBusinessUnit branch;
                    switch (origin)
                    {
                        case "workorder":
                            var wo = new NeWOProg(main_id);
                            branch = new NeBusinessUnit(Convert.ToInt32(wo.business_unit_id));
                            break;
                        case "quote":
                            var business_unit_id =
                                Toolbox.doSQL_string(conn, @"SELECT business_unit_id FROM quote_master  WHERE quote_id =@v0 AND revision =@v1 ", new object[] { id, rev });
                            branch = new NeBusinessUnit(Convert.ToInt32(business_unit_id));
                            break;
                        default:
                            branch = current_user.business_unit;
                            break;
                    }
                    newdescription = ddlMemberType.SelectedItem + " " + ddlLabourChargeType.Text + " hours " + branch.labour_labor;
                }
                else if (partno >= 2000000)
                {
                    newdescription = ddlKittedParts.Text;
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

                if (origin == "workorder" && DateRequired.Date != null)
                {
                    newdatereq = Toolbox.MySQL_shortdt(DateRequired.Date);
                }

                #endregion Date required

                #region Cost

                var comqty = TextComQty.Text == "" ? 0 : Convert.ToDouble(TextComQty.Text);
                if (partno != 777 && partno != 0)
                {
                    if (origin == "purchaseorder")
                    {
                        if (TextCost.Text != "")
                        {
                            try
                            {
                                newcost = Convert.ToDouble(TextCost.Text);
                            }
                            catch
                            {
                                update_error("There is an Issue with the cost you set");
                                ScriptManager1.SetFocus(TextCost);
                                return;
                            }
                        }
                        else
                        {
                            newcost = 0;
                        }
                    }
                    else if (origin == "workorder" || origin == "quote")
                    {
                        if (partno < 990000)
                        {
                            /**
                             * Please leave hidCompanyId.value as it is as it's been converted to warehouse BU in function 
                             * */
                            newcost =
                                Toolbox.doSQL_double(conn, @"SELECT GET_COST_AT_QTY(@v0 ,@v1 ,0, @v2 )", new object[] { partno, WarehouseBusinessUnit.id, origin == "workorder" ? comqty : TextQty.Value == null || TextQty.Value.ToString() == "" ? 1 : TextQty.Value });
                            if (checkinv.is_exclude || partno == 0)
                            {
                                cost_level = 0;
                                if (TextCost.Text != "")
                                {
                                    newcost = Convert.ToDouble(TextCost.Text);
                                }
                                else
                                {
                                    newcost = 0;
                                }
                            }
                            else
                            {
                                cost_level = Toolbox.doSQL_int(conn, @"SELECT GET_COST(@v0 ,@v1 ,true)", new object[] { partno, WarehouseBusinessUnit.id });
                            }
                        }
                        else if (partno >= 990000 && partno < 2000000)
                        {
                            cost_level = 0;
                            if (origin == "quote")
                            {
                                try
                                {
                                    newcost =
                                        Toolbox.doSQL_double(conn, @"SELECT GET_CURRENT_LABOUR_COST(@v0 ,@v1 )", new object[] { partno, hidCompanyID.Value });
                                }
                                catch (Exception ee)
                                {

                                    throw;
                                }
                            }
                            else
                            {
                                newcost = 0;
                            }
                        }
                        else if (partno >= 2000000 && origin == "quote")
                        {
                            cost_level = 0;
                            newcost = Toolbox.doSQL_double(conn, @"select get_cost(@v0 ,@v1 , false)", new object[] { partno, WarehouseBusinessUnit.id });
                        }
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
                    //	if (this_origin != "workorder")
                    //	{
                    //		newqty = Convert.ToDouble(TextQty.Text);
                    //		if (newqty == 0)
                    //		{
                    //			update_error("Invalid Qty: 0");
                    //			this.ScriptManager1.SetFocus(TextQty);
                    //			return;
                    //		}
                    //	}
                    //	else
                    //	{
                    //		if ((TextComQty.Text == null) || (TextComQty.Text == ""))
                    //		{
                    //			TextComQty.Text = "0";
                    //		}
                    //		newqty = Convert.ToDouble(TextComQty.Text);
                    //		if (newqty == 0)
                    //		{
                    //			update_error("Invalid Qty: 0");
                    //			this.ScriptManager1.SetFocus(TextComQty);
                    //			return;
                    //		}
                    //	}
                    if (TextQty.Text == null || TextQty.Text == "")
                    {
                        TextQty.Text = "0";
                    }
                    newqty = Convert.ToDouble(TextQty.Text);
                    if (newqty == 0 && comqty == 0)
                    {
                        update_error("Invalid Required Quantity of 0, please fix.");
                        ScriptManager1.SetFocus(TextQty);
                        return;
                    }

                    if (origin == "purchaseorder")
                    {
                        //
                        // 060- Purchase Orders Cannot Contain Positive and Negative Quantities - Fisrt Part about creating new line items for PO.
                        // If the order quantity of first line item is positive, all others must be positive.
                        //
                        var dt_check_qty_per = Toolbox.doSQL_dt(conn, @"SELECT * FROM po_details_current  where po_details_poprog_id =@v0", new object[] { main_id });
                        if (dt_check_qty_per != null && dt_check_qty_per.Rows != null && dt_check_qty_per.Rows.Count > 0)
                        {
                            var first = dt_check_qty_per.Rows[0];
                            var qtyOrderOfFirst = Convert.ToInt32(first["po_details_qty_ordered"]);

                            if (qtyOrderOfFirst > 0 && newqty < 0)
                            {
                                // First one is poistive, but later stuff are negative
                                update_error("This purchase order cannot have negative required quantities.");
                                ScriptManager1.SetFocus(TextQty);
                                return;
                            }

                            if (qtyOrderOfFirst < 0 && newqty > 0)
                            {
                                // First one is negative, but later stuff are positive
                                update_error("This purchase order cannot have positive required quantities.");
                                ScriptManager1.SetFocus(TextQty);
                                return;
                            }
                        }
                    }

                    if (origin == "workorder")
                    {
                        if (TextComQty.Text == null || TextComQty.Text == "")
                        {
                            TextComQty.Text = "0";
                        }
                        comnewqty = Convert.ToDouble(TextComQty.Text);
                    }
                }
                catch
                {
                    if (origin == "purchaseorder" && newpartnumber == "")
                    {
                    }
                    else if (origin == "workorder" && ddlBillType.Value.ToString() == "6")
                    {
                    }
                    else
                    {
                        update_error("Invalid Qty");
                        ScriptManager1.SetFocus(TextQty);
                        return;
                    }
                }

                #endregion Quantity

                #region Original Sell Price

                if (partno != 777)
                {
                    try
                    {
                        var used_cost = newcost == 0 & checkinv.cost_price_branch > 0 ? checkinv.cost_price_branch : newcost;
                        if (origin == "workorder")
                        {
                            double exists_chk = 0;
                            try
                            {
                                exists_chk =
                                    Toolbox.doSQL_double(conn, @"SELECT ifnull(sum(wo_detail_current_qty_committed),0) FROM wo_detail_current WHERE wo_detail_current_master_id = @v0  AND wo_detail_current_woprog_id = @v1  ", new object[] { partno, main_id });
                            }
                            catch
                            {
                            }

                            neworigsell = checkinv.is_exclude || partno >= 990000
                                ? Toolbox.do_Round(Convert.ToDouble(TextSell.Text), 2)
                                : shared.GetSellPrice(used_cost, 0, true, comnewqty + exists_chk, WorkingBusinessUnit.id32);
                        }
                        else if (origin == "purchaseorder")
                        {
                            neworigsell = 0;
                        }
                        else
                        {
                            if (checkinv.is_exclude || partno >= 990000)
                            {
                                double.TryParse(TextSell.Text, out neworigsell);
                            }
                            else
                            {
                                neworigsell = shared.GetSellPrice(used_cost, 0, true, newqty, WorkingBusinessUnit.id32);
                            }
                        }
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

                if (partno != 777)
                {
                    try
                    {
                        // If the TM Extended Sell doesn't equal the Quoted Extended Sell and the origin isn't kitted or purchase order and the material type isn't ER
                        if (TextTMExtd.Text != TextQuotedExtd.Text && newqty != 0 && origin != "kitted" && origin != "purchaseorder" &&
                            origin != "workorder" && ddlMatType.SelectedValue != "9")
                        {
                            newsell = Toolbox.do_Round(Convert.ToDouble(TextQuotedExtd.Text) / newqty, 2);
                            if (newsell < newcost && (origin == "quote" || origin == "workorder"))
                            {

                            }
                        }
                        else
                        {
                            var used_cost = newcost == 0 & checkinv.cost_price_branch > 0 ? checkinv.cost_price_branch : newcost;
                            newsell = checkinv.is_exclude || partno >= 990000
                                ? neworigsell
                                : shared.GetSellPrice(used_cost, 0, true, newqty, WorkingBusinessUnit.id32);
                            // Get Chargeout to compare.
                            if (partno >= 990000 && partno < 2000000 && origin == "quote")
                            {
                                var quo = new quote(Convert.ToInt32(main_id + rev));
                                double ChargeOut = 0;
                                try
                                {
                                    if (partno == 1000000)
                                    {
                                        ChargeOut = 0;
                                    }
                                    else
                                    {
                                        ChargeOut = Toolbox.doSQL_double(conn, @"CALL CUSTOMER_CHARGEOUT(@v0 , @v1 )", new object[] { quo.cust_id, partno });
                                    }
                                }
                                catch
                                {
                                    ChargeOut = 0;
                                }
                                if (newsell != ChargeOut)
                                {
                                    newsell = ChargeOut;
                                }
                            }
                            if (newsell < newcost && (origin == "quote" || origin == "workorder"))
                            {
                            }
                        }
                    }
                    catch
                    {
                        update_error("The Sell Price is not Valid");
                        return;
                    }
                    try
                    {
                        if (newsell == 0 && partno < 900000 && origin != "purchaseorder" && ddlBillType.Value.ToString() != "6")
                        {
                            update_error("You cannot add a part with a 0 sell price");
                            return;
                        }
                    }
                    catch
                    {
                        if (newsell == 0 && partno < 900000 && origin != "purchaseorder")
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

                #region Quantity Per

                // try
                // {
                //   newboughtin = txtBoughtIn.Text;
                // }
                // catch { }
                double.TryParse(txtQtyPerPart.Text, out newqtyperpart);
                if (origin == "purchaseorder" && newpartnumber != "" && newqtyperpart == 0)
                {
                    update_error("A PO Item must has a qty per part value");
                    return;
                }
                if (origin != "purchaseorder")
                {
                    newqtyperpart = 1;
                }

                #endregion Quantity Per

                #region Date Expected

                try
                {
                    newDateExpected = ASPxDateExpected.Text;
                }
                catch
                {
                }
                if (newDateExpected == "")
                {
                    if (origin == "purchaseorder")
                    {
                        update_error("A PO Item must have an Expected Date Specified");
                        return;
                    }
                }

                #endregion Date Expected

                #region Notes

                newNotes = hidNotes.Value;
                try
                {
                    if (CheckBoxInclude.Checked)
                        newinclude = true;
                }
                catch
                {
                    newinclude = false;
                }

                #endregion Notes

                #region Department

                if (origin == "purchaseorder")
                {
                    if (checkinv.tag_id == "843" && !new_is_gl_account)
                    {
                        update_error("An NE G/L cannot be billed to a work order");
                        return;
                    }
                }

                #endregion

                #region Quoted Extended Sell

                if (origin == "quote")
                {
                    double.TryParse(TextQuotedExtd.Text, out newquoteextd);
                    if (newquoteextd == 0)
                    {
                        newquoteextd = newsell * new_qty_committed;
                    }
                }

                #endregion Quoted Extended Sell

                xfer_comm_recv_select_id = addline_location.Visible && Convert.ToString(addline_location.Value) != ""
                    ? Convert.ToInt32(addline_location.Value)
                    : 0;

                #region Add Line / DataSource Refreshers

                if (origin == "quote")
                {
                    // should we apply discounts?
                    var quote_row =
                        Toolbox.doSQL_dt(conn, @"SELECT * FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { main_id, rev }).Rows[0];
                    var a_id = Convert.ToInt32(quote_row["address_id"]);
                    var c_id = Convert.ToInt32(quote_row["customer_id"]);
                    if (a_id == 0)
                    {
                        a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                    }
                    var app_disc = Toolbox.ReturnZeroIfNull_int(quote_row["quote_master_apply_discount"]);
                    if (app_disc == 1 && partno < 990000)
                    {
                        newdiscount = 0;
                    }
                    AddNewQuoteSectionLine();
                    isDifferentSection = newsectionid != oldsectionid && !string.IsNullOrEmpty(_q["section_id"]);
                    SqlDataSource1.SelectCommand =
                        string.Format(
                            @"SELECT a.id, a.section, a.detail_id, (SELECT COUNT(*) FROM quote_worksheet b WHERE b.section_id = a.id) dependants FROM quote_section a WHERE a.quote_id = {0} AND a.revision = {1} AND LENGTH(TRIM(a.section)) > 0 ORDER BY section",
                            main_id, rev);
                    FillforQuote(main_id, main_rev);
                }
                else if (origin == "purchaseorder")
                {
                    AddPOLine(main_id);
                    FillForPO(main_id);
                }
                else if (origin == "kitted")
                {
                    Toolbox.doSQL_void(conn, @" INSERT INTO inventory_kit_dtl ( inventory_kit_dtl_hdr_id, inventory_kit_dtl_master_id, inventory_kit_dtl_member_id, inventory_kit_dtl_notes, inventory_kit_dtl_qty, inventory_kit_dtl_edited_dt, inventory_kit_dtl_active ) VALUES ( @v0 , @v1 , @v2 , @v3 , @v4 , curdate(), 1 )", new object[] { main_id, TextPartNo.Text, current_user.id, hidNotes.Value, TextQty.Text });
                    ddlKittedParts.SelectedIndex = -1;
                    TextPartNo.Text = "";
                    FillforKitted(main_id);
                }

                #endregion Add Line / DataSource Refreshers

                break;

                #endregion Quote/Work order/purchase order/kitted
        }
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
        //      	update_error("");
        //
        TextPartNo.Text = "";
        TextDescription.Text = "";
        TextDescription.ToolTip = "";

        #region $(this).inventory attach

        TextDescription.Attributes.Add("onfocus", @"description_onfocus(this)");
        TextDescription.Attributes.Add("onblur", "desc_resize(this, false)");
        //	TextDescription.Attributes.Add("onkeyup", "description_keyup(this)");

        #endregion $(this).inventory attach

        TextQty.Text = "";
        TextQty.Enabled = true;
        TextQuotedExtd.Text = "";
        TextSell.Text = "";
        TextTMExtd.Text = "";
        TextCost.Text = "";
        TextVendorPartNo.Text = "";
        TextComQty.Text = "";
        TextAvailQty.Text = "0";
        update_error("");
        update_info("");
        if (ddlMatType.SelectedIndex == 2 && origin == "quote" && ddlKittedParts.SelectedIndex > -1)
        {
            TextPartNo.Text = ddlKittedParts.Value.ToString();
        }
        if (ddlMatType.SelectedValue == "9" && origin == "quote")
        {
            ddlWorkOrder.SelectedIndex = 0;
        }
        else if (origin == "quote")
        {
            try
            {
                var quote_info =
                    Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id, address_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
                var companyid = quote_info["business_unit_id"].ToString();
                var c_id = Convert.ToInt32(quote_info["customer_id"]);
                var a_id = Convert.ToInt32(quote_info["address_id"]);
                if (a_id == 0)
                {
                    a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                }
                var quotediscount = quote_info["quote_master_apply_discount"].ToString();
                var division_id = Convert.ToInt32(quote_info["division_id"]);
                hidCompanyID.Value = companyid;
                chkQuoteDiscount.Checked = quotediscount == "1";
                DiscountAmount = 0;
                hidWODiscount.Value = quotediscount == "1" ? DiscountAmount.ToString() : "0";
                ASPxTextDiscount.Text = DiscountAmount.ToString();
            }
            catch (Exception ee)
            {
                Toolbox.FriendlyException(Response, "This is not a valid quote #", "history.go(-1)");
            }
        }
        if (origin == "quote" || origin == "purchaseorder")
        {
            TextCost.ClientEnabled = true;
        }
        if (origin == "workorder")
        {
            ASPxTextDiscount.Text = hidWODiscount.Value;

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
                        var s = Toolbox.doSQL_string(conn, @"Select WOProg_Expected_StartDate from woprog  where woprog_id =@v0", new object[] { main_id });
                        if (s != "")
                        {
                            if (Convert.ToDateTime(s) > DateRequired.Date)
                            {
                                DateRequired.Date = Convert.ToDateTime(s);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                DateRequired.Date = DateRequired.Date;
            }
            DateRequired.MinDate = DateTime.Today;
        }
        DateRequired.ToolTip = DateRequired.Date.ToLongDateString();

        CheckBoxInclude.Checked = true;
        chkNewPart.Checked = false;
        chkTrackPart.Checked = false;
        hidNotes.Value = "";
        txtQtyPerPart.Text = "";
        lbVpic.ImageUrl = "~/images/EmptyPicture.JPG";
        try
        {
            txtQuoteLabDesc.Text = "";
        }
        catch
        {
        }
        if (ddlMatType.SelectedItem.Text == "Labr")
        {
            Populate_LabourDDLs(hidCompanyID.Value);
            Fill_Labour_Totals();
        }
        if (!full_reset)
        {
            // MH: I am not completely sure why I added this in here but it seems like overkill... if we have troubles with items not resetting, this is the culprit.
            ddlMatType_SelectedIndexChanged(ddlMatType, null);
        }
    }

    protected void lbaddsection_Click(object sender, EventArgs e)
    {
        ASPxPopupSectionAdd.ShowOnPageLoad = true;
    }

    protected void lbVpic_Click(object sender, EventArgs e)
    {
        var _id = TextPartNo.Text;
        if (_id != "")
        {
            Image1.ImageUrl = string.Format("~/_tools/inventory_picture/index.aspx?id={0}&is_master=true", _id);
            ASPxPopupControlPicture.ShowOnPageLoad = true;
        }
    }

    protected void ddlMatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear_boxes(true);
        add_lc1.Visible = false;

        #region Material (1)

        if (ddlMatType.SelectedValue == "1") // material
        {
            TextQty.Enabled = true;
            btn_Save.Enabled = true;
            if (origin != "purchaseorder")
            {
                if (origin == "workorder")
                {
                    addtoggle(new[] { 6, 4, 9, 23, 24 }, true);
                }
                else if (origin != "rfq")
                {
                    addtoggle(new[] { 6, 4, 9 }, true);
                    addtoggle(new[] { 3 }, false);
                }
            }
            else if (origin != "rfq")
            {
                addtoggle(new[] { 3, 1, 7, 10, 14, 15, 16, 18, 22, 23, 24, 25, 26 }, false);
                addtoggle(new[] { 2, 3, 4, 5, 6, 8, 9, 11, 12, 13, 17, 19, 20, 21 }, true);
            }
            else
            {
                addtoggle(new[] { 1, 3, 5, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 23, 24, 25, 26 }, true);
                //add_hc4.Width	= "100px";
            }
            addtoggle(new[] { 7 }, false);
            ddlWorkOrder.DataSourceID = "sds_add_workorder_ddl";
            //  populate_sds_workorder_ddl(hidCompanyID.Value, GetGLs(hidCompanyID.Value));
            populate_sds_workorder_ddl(hidCompanyID.Value, GetExp(hidCompanyID.Value));

            lblWorkOrder.Text = "Work Order";
            ddlLabourChargeType.SelectedIndex = -1;
            ddlMemberName.Enabled = false;
            if (origin == "quote")
            {
                var quote_info =
                    Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id, address_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
                var companyid = quote_info["business_unit_id"].ToString();
                var quotediscount = quote_info["quote_master_apply_discount"].ToString();
                var c_id = Convert.ToInt32(quote_info["customer_id"]);
                var a_id = Toolbox.ReturnZeroIfNull_int(quote_info["address_id"]);
                var division_id = Convert.ToInt32(quote_info["division_id"]);
                hidCompanyID.Value = companyid;
                chkQuoteDiscount.Checked = quotediscount == "1";
                if (a_id == 0)
                {
                    a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                }
                DiscountAmount = 0;
                hidWODiscount.Value = quotediscount == "1" ? DiscountAmount.ToString() : "0";
                ASPxTextDiscount.Text = DiscountAmount.ToString();
            }
            ddlLabourChargeType.Enabled = false;
            ScriptManager1.SetFocus(TextDescription);
        }
        #endregion Material (1)
        #region Labour (2)

        else if (ddlMatType.SelectedValue == "2") // labour
        {
            addtoggle(new[] { 3, 6, 4, 9, 23, 24, 27 }, false);
            addtoggle(new[] { 7 }, true);
            TextQty.Enabled = true;
            btn_Save.Enabled = true;
            ddlLabourChargeType.Visible = true;
            ddlLabourChargeType.Enabled = true;
            ddlKittedParts.Visible = false;
            if (origin == "quote")
            {
                var quote_info =
                    Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id, address_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
                var companyid = quote_info["business_unit_id"].ToString();
                var quotediscount = quote_info["quote_master_apply_discount"].ToString();
                var c_id = Convert.ToInt32(quote_info["customer_id"]);
                var a_id = Toolbox.ReturnZeroIfNull_int(quote_info["address_id"]);
                var division_id = Convert.ToInt32(quote_info["division_id"]);
                hidCompanyID.Value = companyid;
                chkQuoteDiscount.Checked = quotediscount == "1";
                if (a_id == 0)
                {
                    a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                }
                DiscountAmount = 0;
                hidWODiscount.Value = quotediscount == "1" ? DiscountAmount.ToString() : "0";
                ASPxTextDiscount.Text = hidWODiscount.Value;
            }
            if (origin != "quote" && origin != "kitted" && origin != "groupings" && origin != "workorder")
            {
                ddlMemberType.Visible = false;
                ddlMemberName.Enabled = true;
                ddlMemberName.Visible = true;
                ScriptManager1.SetFocus(ddlMemberName);
            }
            else
            {
                ddlMemberType.Visible = true;
                ddlMemberName.Enabled = false;
                ddlMemberName.Visible = false;
                if (origin == "quote" || origin != "workorder")
                {
                    txtQuoteLabDesc.Width = 300;
                    add_lc1.Visible = true;
                    add_lc1.Height = "40";
                }
                ScriptManager1.SetFocus(ddlMemberType);
            }
            //		this.ScriptManager1.SetFocus(ddlMemberName);
            Populate_LabourDDLs(hidCompanyID.Value);
            Fill_Labour_Totals();
        }
        #endregion Labour (2)
        #region Kitted (3)

        else if (ddlMatType.SelectedValue == "3") // kitted
        {
            btn_Save.Enabled = true;
            addtoggle(new[] { 3, 6, 4, 9, 17, 23, 24 }, false);
            addtoggle(new[] { 7 }, true);
            TextQty.Enabled = true;
            lblLabourDescription.Visible = true;
            lblVendorPartNo.Visible = false;
            TextVendorPartNo.Visible = false;
            lblLabourDescription.Text = "Choose Kit";
            if (origin == "quote")
            {
                var quote_info =
                    Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id, address_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
                var companyid = quote_info["business_unit_id"].ToString();
                var quotediscount = quote_info["quote_master_apply_discount"].ToString();
                var c_id = Convert.ToInt32(quote_info["customer_id"]);
                var a_id = Toolbox.ReturnZeroIfNull_int(quote_info["address_id"]);
                var division_id = Convert.ToInt32(quote_info["division_id"]);
                hidCompanyID.Value = companyid;
                chkQuoteDiscount.Checked = quotediscount == "1";
                if (a_id == 0)
                {
                    a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                }
                DiscountAmount = 0;
                hidWODiscount.Value = quotediscount == "1" ? DiscountAmount.ToString() : "0";
                ASPxTextDiscount.Text = hidWODiscount.Value;
            }
            ddlLabourChargeType.Visible = false;
            ddlMemberType.Visible = false;
            ddlKittedParts.Visible = true;
            ddlMemberName.Enabled = false;
            ddlMemberName.Visible = false;
            TextSell.ClientEnabled = false;
            ddlKittedParts.CssClass = "add_kit";
            btn_Save.Enabled = true;
            TextQty.Enabled = true;
            lbVpic.Visible = false;
            ScriptManager1.SetFocus(ddlKittedParts);
            Populate_KittedDDLs();
            TextQty.Text = "1";
            Fill_kitted_Totals();
        }
        #endregion Kitted (3)
        #region Groups (4)

        else if (ddlMatType.SelectedValue == "4") // Grab parts from a group
        {
            btn_Save.Enabled = true;
            addtoggle(new[] { 3, 6, 4, 9, 23, 24 }, false);
            addtoggle(new[] { 7 }, true);
            ddlKittedParts.Visible = true;
            lblLabourDescription.Text = "Choose Group";
            ddlLabourChargeType.Visible = false;
            ddlMemberType.Visible = false;
            btn_Save.Enabled = false;
            TextQty.Enabled = false;
            TextSell.ClientEnabled = false;
            if (origin == "quote")
            {
                var quote_info =
                    Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id, address_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
                var companyid = quote_info["business_unit_id"].ToString();
                var quotediscount = quote_info["quote_master_apply_discount"].ToString();
                var c_id = Convert.ToInt32(quote_info["customer_id"]);
                var a_id = Toolbox.ReturnZeroIfNull_int(quote_info["address_id"]);
                var division_id = Convert.ToInt32(quote_info["division_id"]);
                hidCompanyID.Value = companyid;
                chkQuoteDiscount.Checked = quotediscount == "1";
                if (a_id == 0)
                {
                    a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                }
                DiscountAmount = 0;
                hidWODiscount.Value = quotediscount == "1" ? DiscountAmount.ToString() : "0";
                ASPxTextDiscount.Text = hidWODiscount.Value;
            }
            ddlMemberName.Enabled = false;
            ddlMemberName.Visible = false;
            ScriptManager1.SetFocus(ddlKittedParts);
            btn_Save.Enabled = false;
            Populate_GroupSelectDDL();
            TextQty.Text = "0";
        }
        #endregion Groups (4)
        #region Quote (5)

        else if (ddlMatType.SelectedValue == "5") // Grab parts from a Quote
        {
            btn_Save.Enabled = true;
            addtoggle(new[] { 3, 6, 4, 9 }, false);
            addtoggle(new[] { 7 }, true);
            ddlKittedParts.Visible = true;
            ddlKittedParts.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            ddlKittedParts.CallbackPageSize = 20;
            ddlKittedParts.Width = Unit.Percentage(100);
            lblLabourDescription.Text = "Choose Quote";
            ddlLabourChargeType.Visible = false;
            ddlMemberType.Visible = false;
            btn_Save.Enabled = false;
            TextQty.Enabled = false;
            TextSell.ClientEnabled = false;
            if (origin == "quote")
            {
                var quote_info =
                    Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id, address_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
                var companyid = quote_info["business_unit_id"].ToString();
                var quotediscount = quote_info["quote_master_apply_discount"].ToString();
                var c_id = Convert.ToInt32(quote_info["customer_id"]);
                var a_id = Toolbox.ReturnZeroIfNull_int(quote_info["address_id"]);
                var division_id = Convert.ToInt32(quote_info["division_id"]);
                hidCompanyID.Value = companyid;
                chkQuoteDiscount.Checked = quotediscount == "1";
                if (a_id == 0)
                {
                    a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                }
                DiscountAmount = 0;
                hidWODiscount.Value = quotediscount == "1" ? DiscountAmount.ToString() : "0";
                ASPxTextDiscount.Text = hidWODiscount.Value;
            }
            ddlMemberName.Enabled = false;
            ddlMemberName.Visible = false;
            ddlKittedParts.Text = "";
            ScriptManager1.SetFocus(ddlKittedParts);
            if (origin == "workorder" || origin == "rfq")
            {
                Populate_QuoteSelectDDL();
            }
            else if (origin == "quote" || origin == "groupings")
            {
                ddlMemberType.Enabled = true;
                ddlMemberType.Visible = true;
                Populate_membertypeDDL();
            }
            TextQty.Text = "0";
        }
        #endregion Quote (5)
        #region Other_WorkOrder (6)

        else if (ddlMatType.SelectedValue == "6") // Grab parts from a Work Order
        {
            btn_Save.Enabled = true;
            addtoggle(new[] { 3, 4, 5, 6, 9, 23, 24 }, false);
            addtoggle(new[] { 7 }, true);
            // ddlKittedParts is used for the work orders... ddlmembertype is used for the customers
            ddlKittedParts.Items.Clear();
            ddlKittedParts.Visible = true;
            ddlKittedParts.Text = "";
            ddlLabourChargeType.Visible = false;
            lblLabourDescription.Text = "Choose Work Order";
            ddlMemberType.Visible = false;
            btn_Save.Enabled = false;
            TextQty.Enabled = false;
            TextSell.ClientEnabled = false;
            ddlMemberName.Enabled = false;
            if (origin == "quote")
            {
                var quote_info =
                    Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id, address_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
                var companyid = quote_info["business_unit_id"].ToString();
                var quotediscount = quote_info["quote_master_apply_discount"].ToString();
                var c_id = Convert.ToInt32(quote_info["customer_id"]);
                var a_id = Toolbox.ReturnZeroIfNull_int(quote_info["address_id"]);
                var division_id = Convert.ToInt32(quote_info["division_id"]);
                hidCompanyID.Value = companyid;
                chkQuoteDiscount.Checked = quotediscount == "1";
                if (a_id == 0)
                {
                    a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                }
                DiscountAmount = 0;
                hidWODiscount.Value = quotediscount == "1" ? DiscountAmount.ToString() : "0";
                ASPxTextDiscount.Text = hidWODiscount.Value;
            }
            ddlMemberName.Visible = false;
            ddlMemberType.Enabled = true;
            ddlMemberType.Visible = true;
            Populate_membertypeDDL();
            TextQty.Text = "0";
        }
        #endregion Other_WorkOrder (6)
        #region Purchase Order (7)

        else if (ddlMatType.SelectedValue == "7") // Grab parts from a purchase Order
        {
            btn_Save.Enabled = true;
            addtoggle(new[] { 4, 5, 6, 9, 23, 24 }, false);
            addtoggle(new[] { 7 }, true);
            // ddlKittedParts is used for the purchase orders... ddlmembertype is used for the vendors
            ddlKittedParts.Items.Clear();
            ddlKittedParts.Visible = true;
            ddlKittedParts.Text = "";
            ddlLabourChargeType.Visible = false;
            ddlMemberType.Visible = false;
            btn_Save.Enabled = false;
            lblLabourDescription.Text = "Choose Purchase Order";
            TextQty.Enabled = false;
            TextSell.ClientEnabled = false;
            ddlMemberName.Enabled = false;
            ddlMemberName.Visible = false;
            ddlMemberType.Enabled = true;
            ddlMemberType.Visible = true;
            Populate_membertypeDDL();
            TextQty.Text = "0";
        }
        #endregion
        #region Vendor RFQ (8)

        else if (ddlMatType.SelectedValue == "8")
        {
            lblWorkOrder.Text = "Vendor RFQ";
            btn_Save.Enabled = true;
            addtoggle(new[] { 1, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 }, false);
            addtoggle(new[] { 7, 2 }, true);
            ddlKittedParts.Visible = true;
            ddlKittedParts.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            ddlKittedParts.CallbackPageSize = 20;
            ddlKittedParts.Width = Unit.Percentage(100);
            lblLabourDescription.Text = "Choose Vendor RFQ";
            ddlMatType.Width = Unit.Percentage(100);
            ddlLabourChargeType.Visible = false;
            ddlMemberType.Visible = false;
            btn_Save.Enabled = false;
            TextQty.Enabled = false;
            TextSell.ClientEnabled = false;
            if (origin == "quote")
            {
                var quote_info =
                    Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id, address_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
                var companyid = quote_info["business_unit_id"].ToString();
                var quotediscount = quote_info["quote_master_apply_discount"].ToString();
                var c_id = Convert.ToInt32(quote_info["customer_id"]);
                var a_id = Toolbox.ReturnZeroIfNull_int(quote_info["address_id"]);
                var division_id = Convert.ToInt32(quote_info["division_id"]);
                hidCompanyID.Value = companyid;
                chkQuoteDiscount.Checked = quotediscount == "1";
                if (a_id == 0)
                {
                    a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                }
                DiscountAmount = 0;
                hidWODiscount.Value = quotediscount == "1" ? DiscountAmount.ToString() : "0";
                ASPxTextDiscount.Text = hidWODiscount.Value;
            }
            ddlMemberName.Enabled = false;
            ddlMemberName.Visible = false;
            ddlKittedParts.Text = "";
            ScriptManager1.SetFocus(ddlKittedParts);
            Populate_RFQSelectDDL();
            TextQty.Text = "0";
        }
        #endregion Vendor RFQ (8)
        #region Repair (9)

        else if (ddlMatType.SelectedValue == "9") // Repair Material type (only for er work orders)
        {
            TextCost.ClientEnabled = false;
            TextCost.Text = "";
            TextQty.Text = "1";
            TextQty.Enabled = false;
            btn_Save.Enabled = true;
            ASPxTextDiscount.Text = "0";
            addtoggle(new[] { 4, 7 }, false);
            addtoggle(new[] { 3, 6 }, true);
            var quote_info =
                Toolbox.doSQL_dt(conn, @"SELECT business_unit_id,quote_master_apply_discount,customer_id, division_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev }).Rows[0];
            lblWorkOrder.Text = "Master ID";
            var _parts =
                Toolbox.doSQL_dt(conn, @"SELECT a.id woprog_id, CAST(CONCAT(a.master_id, ' - ', b.description, ' :: Serial# - ', a.serial, ' :: Repair ID - ', a.id) AS CHAR) workorder FROM inventory_consignment a LEFT JOIN inventory_description b ON a.master_id = b.master_id  WHERE a.customer_id =@v0 AND a.business_unit_id =@v1  AND a.status = 'Waiting to be Quoted'", new object[] { quote_info["customer_id"], hidCompanyID.Value });
            ddlWorkOrder.DataSource = _parts;
            ddlWorkOrder.DataSourceID = "";
            ddlWorkOrder.DataBind();
            ddlWorkOrder.Items.Insert(0, new ListEditItem("Select a Part", 0));
            ddlWorkOrder.SelectedIndex = 0;
        }

        #endregion Repair (9)
    }

    protected void ddlMemberName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMatType.SelectedValue == "2")
        {
            var customer_id = 0;
            switch (origin)
            {
                case "quote":
                    var qu = new quote(main_id);
                    customer_id = qu.cust_id;
                    break;
                case "workorder":
                    var wo = new NeWOProg(main_id);
                    customer_id = wo.WOProg_Customer_ID;
                    break;
            }
            try
            {
                var paytypeid = ddlLabourChargeType.Value.ToString();
                var memberid = ddlMemberName.Value.ToString();
                var addmember = new NeMember(Convert.ToInt32(memberid));
                var GetPart = "";
                try
                {
                    GetPart =
                        Toolbox.doSQL_string(conn, @"SELECT id from membertype_chargeout WHERE membertype_id = @v0  AND business_unit_id = @v1  AND paytype_id = @v2 ", new object[] { addmember.MemberTypeID, addmember.business_unit, paytypeid });
                }
                catch
                {
                    GetPart = "1000000";
                }
                var ChargeOut = "";
                try
                {
                    ChargeOut = Toolbox.doSQL_string(conn, @"CALL CUSTOMER_CHARGEOUT(@v0 , @v1 )", new object[] { customer_id, GetPart });
                }
                catch
                {
                    ChargeOut = "0";
                }
                TextPartNo.Text = GetPart;
                TextSell.Text = ChargeOut;
                TextTMExtd.Text = Convert.ToString(Convert.ToDouble(TextSell.Text) * Toolbox.do_ConvertToDouble(TextQty.Text));
                TextQuotedExtd.Text = Convert.ToString(Convert.ToDouble(TextSell.Text) * Toolbox.do_ConvertToDouble(TextQty.Text));
                ScriptManager1.SetFocus(TextQty);
            }
            catch
            {
            }
        }
    }

    protected void fill_edit_form_workorder_ddl()
    {
        ds_wo_edit_form.SelectCommand = string.Format(@"{1}(SELECT  WOProg_ID, 
REPLACE(CONCAT(WOProg_BVWO,' - ', WOProg_CustomerName, ' ', LEFT(WOProg_Description,40), ' - ', ddl_name, ' - ',woprog_status), '\n', '') AS WorkOrder 
FROM woprog left join business_unit ON woprog.business_unit_id = business_unit.id
WHERE 
business_unit_id='{0}' AND 
WOProg_BVWO != 'Not Entered' and 
woprog_status NOT IN ('Invoiced', 'Waiting To Be Invoiced', 'Waiting For PO', 'Waiting BM Approval', 'Waiting PM Approval', 'Waiting Approval','Deleted','Waiting Parent BM  Approval') AND 
woprog_hold = 0 AND
woprog_isrebill = 0 AND 
woprog_iscredit = 0 AND 
labor_only = false AND
WOProg_Associate_WOprog_ID = 0 AND
TRIM(woprog_description) != ''  
ORDER BY woprog_id desc, WOProg_CustomerName LIMIT 1000)", hidCompanyID.Value, GetExp(hidCompanyID.Value));
    }

    protected void TextSell_TextChanged(object sender, EventArgs e)
    {
        if (origin != "quote")
        {
            try
            {
                double newselling = 0;
                double.TryParse(TextSell.Text, out newselling);
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
                TextQuotedExtd.Text = newextended.ToString();
                //this.ScriptManager1.SetFocus(chkNewPart);
                if (TextPartNo.Text == "2139")
                {
                    TextCost.ClientEnabled = true;
                    TextSell.ClientEnabled = true;
                    addline_location.ClientEnabled = false;
                }
            }
            catch
            {
            }
        }
    }

    protected void imgbnotesupdate_Click(object sender, EventArgs e)
    {
        var sql = "";
        if (hidNotesID.Value == "0")
        {
            hidNotes.Value = textNewNote.Text;
        }
        else
        {
            if (origin == "purchaseorder")
            {
                Toolbox.doSQL_void(
                            @"UPDATE po_details_current SET po_details_notes = CONCAT(IFNULL(po_details_notes, ''),'\n--\n [',@v2,'] - ', NOW(),'\n ',@v0) WHERE po_details_id =@v1",
                    new object[] { textNewNote.Text, hidNotesID.Value, current_user.FullName });
            }
            else if (origin == "alternates")
            {
                Toolbox.doSQL_void(
                            @"UPDATE inventory_alternate SET inventory_alternate_notes = CONCAT(IFNULL(inventory_alternate_notes, ''),'\n--\n [',@v2,'] - ', NOW(),'\n ', @v0) WHERE inventory_alternate_id =@v1",
                    new object[] { textNewNote.Text, hidNotesID.Value, current_user.FullName });
            }
            else if (origin == "groupings")
            {

                Toolbox.doSQL_void(
                    @"UPDATE inventory_group_dtl SET inventory_group_dtl_notes =  CONCAT(IFNULL(inventory_group_dtl_notes, ''),'\n--\n [',@v2,'] - ', NOW(),'\n ', @v0) WHERE id = @v1",
                new object[] { textNewNote.Text, hidNotesID.Value, current_user.FullName });
            }
            else if (origin == "quote")
            {
                Toolbox.doSQL_void(
                            @"UPDATE quote_worksheet SET notes =   CONCAT(IFNULL(notes, ''),'\n--\n [',@v2,' ] - ', NOW(),'\n ', @v0 ) WHERE id = @v1",
                    new object[] { textNewNote.Text, hidNotesID.Value, current_user.FullName });
            }
            else if (origin == "workorder")
            {
                var current_note =
                    Toolbox.doSQL_string(conn, @"SELECT IFNULL(wo_detail_current_notes,'') FROM wo_detail_current  WHERE wo_detail_current_id =@v0", new object[] { hidNotesID.Value });
                var this_note = current_note.Contains("A_")
                    ? current_note.Substring(0, current_note.IndexOf('|')) + "|" + textNewNote.Text
                    : textNewNote.Text;
                Toolbox.doSQL_void(conn, @"UPDATE wo_detail_current SET wo_detail_current_notes = CONCAT(IFNULL(wo_detail_current_notes, ''),'\n--\n [',@v2,' ] - ', NOW(),'\n', @v0 )
WHERE wo_detail_current_id = @v1 ", new object[] { this_note, hidNotesID.Value, current_user.FullName });
            }
            else if (origin == "rfq")
            {
                Toolbox.doSQL_void(conn, @"UPDATE rfq_part_list SET note = CONCAT(IFNULL(note, ''),'\n--\n [',@v2,' ] - ', NOW(),'\n ', @v0 ) WHERE id = @v1  LIMIT 1",
                    new object[] { textNewNote.Text, hidNotesID.Value, current_user.FullName });
            }

            try
            {

                update_error("");
                switch (origin)
                {
                    case "purchaseorder":
                        save_polineitem_note_toBV("Line Item Notes");
                        FillForPO(main_id);
                        break;
                    case "alternates":
                        FillforAlternates(main_id);
                        break;
                    case "groupings":
                        FillforGroupings(main_id);
                        break;
                    case "quote":
                        FillforQuote(main_id, main_rev);
                        break;
                    case "rfq":
                        FillForRFQ(main_id);
                        break;
                }
            }
            catch
            {
                update_error("Unable to save note");
            }
        }
        textNewNote.Text = "";
        ASPxpuNotes.ShowOnPageLoad = false;
        var sc = ScriptManager.GetCurrent(Page);
        ScriptManager.RegisterClientScriptBlock(stuff, stuff.GetType(), "runtime",
            "<script type='text/javascript'>setTimeout('agv.PerformCallback()', 100);</script>", false);
    }

    protected void Edit_Note_Click(object sender, EventArgs e)
    {
        if (origin == "purchaseorder")
        {
            Toolbox.doSQL_void(
                @"UPDATE po_details_current SET po_details_notes = @v0 WHERE po_details_id =@v1",
                new object[] { note_history.Text, hidNotesID.Value, current_user.FullName });
        }
        else if (origin == "alternates")
        {
            Toolbox.doSQL_void(
                @"UPDATE inventory_alternate SET inventory_alternate_notes =  @v0 WHERE inventory_alternate_id =@v1",
                new object[] { note_history.Text, hidNotesID.Value, current_user.FullName });
        }
        else if (origin == "groupings")
        {

            Toolbox.doSQL_void(
                @"UPDATE inventory_group_dtl SET inventory_group_dtl_notes =   @v0 WHERE id = @v1",
                new object[] { note_history.Text, hidNotesID.Value, current_user.FullName });
        }
        else if (origin == "quote")
        {
            Toolbox.doSQL_void(
                @"UPDATE quote_worksheet SET notes =    @v0  WHERE id = @v1",
                new object[] { note_history.Text, hidNotesID.Value, current_user.FullName });
        }
        else if (origin == "rfq")
        {
            Toolbox.doSQL_void(conn, @"UPDATE rfq_part_list SET note =  @v0  WHERE id = @v1  LIMIT 1",
                new object[] { note_history.Text, hidNotesID.Value, current_user.FullName });
        }


        ASPxpuNotes.ShowOnPageLoad = false;
        var sc = ScriptManager.GetCurrent(Page);
        ScriptManager.RegisterClientScriptBlock(stuff, stuff.GetType(), "runtime",
            "<script type='text/javascript'>setTimeout('agv.PerformCallback()', 100);</script>", false);
    }

    protected void imgbtnPOLineActive_Click(object sender, EventArgs e)
    {
        var preState = false;
        var postState = false;

        if (textNewNote.Text == "")
        {
            //	throw new Exception("Enter A Reason for setting inactive or active");
        }

        var sql2 = "";
        if (origin == "purchaseorder")
        {
            preState = this.IsPOCompleted();

            sql2 =
                string.Format(
                    @"UPDATE po_details_current SET po_details_line_active = IF(po_details_line_active = 0,1,0) WHERE po_details_id = {0}",
                    hidNotesID.Value);
            //Remove or Add Part to Work Order
            try
            {
                Toolbox.doSQL_void(conn, @"UPDATE po_details_current SET po_details_line_active = IF(po_details_line_active = 0,1,0) WHERE po_details_id = @v0 ", new object[] { hidNotesID.Value });
                update_error("");
                int iscomplete;
                if (
                    (iscomplete =
                        Toolbox.doSQL_int(conn, @"Select po_details_line_active from po_details_current WHERE po_details_id = @v0 ", new object[] { hidNotesID.Value })) == 1)
                {
                    textNewNote.Text = "Part Marked Incomplete " + DateTime.Now + Environment.NewLine + textNewNote.Text;
                }
                else
                {
                    textNewNote.Text = "Part Marked Complete: " + DateTime.Now + Environment.NewLine + textNewNote.Text;
                }

                Toolbox.doSQL_void(conn, @"UPDATE po_details_current SET po_details_notes = CONCAT(IFNULL(po_details_notes, ''),'\n--\n ','[',@v2,']',' -', NOW(),'\n', @v0 ) WHERE po_details_id = @v1 ", new object[] { textNewNote.Text, hidNotesID.Value, current_user.FullName });
                save_polineitem_note_toBV("complete");
                FillForPO(main_id);
            }
            catch (Exception ex)
            {
                update_error("Unable to make part incomplete " + ex);
            }
            var rowscomplete = 0;
            var linecounter = 0;
            var poprog = new NePOProg(main_id);
            var tempcompany = new NeBusinessUnit(Convert.ToInt32(poprog.business_unit_id));
            //if (doBv)
            //{
            //    var objPO = new PurchaseOrder(tempcompany.DSN, poprog.poprog_bvpo);
            //
            //    var Details = Toolbox.doSQL_dt(conn, @"SELECT po_details_line_active FROM po_details_current  WHERE po_details_part_no <> 0 and po_details_poprog_id=@v0", new object[] { poprog.poprog_id });
            //    foreach (DataRow row in Details.Rows)
            //    {
            //        if (row["po_details_line_active"].ToString() == "0")
            //            rowscomplete++;
            //        linecounter++;
            //    }
            //    objPO.VendorOrderNumber = rowscomplete + " of " + linecounter + " Complete";
            //    objPO.Save(tempcompany.DSN, current_user.Initials);
            //}
        }
        textNewNote.Text = "";
        ASPxpuNotes.ShowOnPageLoad = false;
        var sc = ScriptManager.GetCurrent(Page);

        if (origin == "purchaseorder")
        {
            postState = this.IsPOCompleted();
        }

        bool statusChanged = preState != postState;
        if (statusChanged)
        {
            string code = string.Format("<script type='text/javascript'>setTimeout('agv.PerformCallback(); ShowPOInvoiceButton(\"{0}\")', 100);</script>", postState ? "Yes" : "No");
            ScriptManager.RegisterClientScriptBlock(stuff, stuff.GetType(), "runtime", code, false);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(stuff, stuff.GetType(), "runtime",
                "<script type='text/javascript'>setTimeout('agv.PerformCallback()', 100);</script>", false);
        }
    }

    protected void TextVendorPartNo_TextChanged(object sender, EventArgs e)
    {
        var duplicate_lists = "!!!INVALID VENDOR CODE!!! Other Part(s) using this Vendor Code:";
        var Vendor_duplicate = Toolbox.doSQL_dt(conn, @" SELECT DISTINCT(master_id) master_id FROM inventory_price WHERE vendor_code = @v0  AND master_id != @v1  AND vendor_id = @v2 ", new object[] { TextVendorPartNo.Text, TextPartNo.Text, hidVendorID.Value });
        foreach (DataRow dr in Vendor_duplicate.Rows)
        {
            var desc =
                Toolbox.doSQL_string(conn, @"SELECT FULL_PART_DESCRIPTION(@v0,0)", new object[] { Convert.ToInt32(dr[0]), NeBusinessUnit.GetbuCountry(hidCompanyID.Value) });
            duplicate_lists += dr[0] + "-" + desc + Environment.NewLine;
            TextPartNo.Text = dr[0].ToString();
            TextDescription.Text = desc;
        }
        if (Vendor_duplicate.Rows.Count > 0)
        {
            throw new Exception(duplicate_lists);
        }
        lblerrorLabel.Visible = false;
        ScriptManager1.SetFocus(TextQty);
    }

    protected void b_refresh_Click(object sender, EventArgs e)
    {
    }

    protected void PartTransfer(object sender, EventArgs e)
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
            var transfer_to_woprog_id = 0;
            transferwoprogid = xfer_wo_combo.Value != null && xfer_wo_qty.Text != "0"
                ? xfer_wo_combo.Value.ToString()
                : "9999999";
            int.TryParse(transferwoprogid, out transfer_to_woprog_id);
            xfer_qty_wo = xfer_wo_qty.Text == "" ? 0 : Convert.ToDouble(xfer_wo_qty.Text);
            xfer_qty_internal = xfer_internal_qty.Text == "" ? 0 : Convert.ToDouble(xfer_internal_qty.Text);
            xfer_qty_external = xfer_external_qty.Text == "" ? 0 : Convert.ToDouble(xfer_external_qty.Text);
            xfer_external_select_id = xfer_external_combo.Value != null ? Convert.ToInt32(xfer_external_combo.Value) : 0;
            xfer_internal_select_id = xfer_internal_combo.Value != null ? Convert.ToInt32(xfer_internal_combo.Value) : 0;
            xfer_wo_select_id = xfer_wo_combo.Value != null ? Convert.ToInt32(xfer_wo_combo.Value) : 0;
            xfer_combined_qty = xfer_qty_wo + xfer_qty_internal + xfer_qty_external;
            transfernote = txtTransferReason.Text;
            var is_stocktransfer = new List<string>(new[] { "9999999", "9999998", "9999997" }).Contains(transferwoprogid);
            if (transferwoprogid != "")
            {
                transferbvwo = is_stocktransfer
                    ? "STOCKTRANSFER"
                    : Toolbox.doSQL_string(conn, @"SELECT woprog_BVWO FROM woprog  WHERE woprog_id =@v0", new object[] { transferwoprogid });
            }
            transferbvwo = (xfer_qty_internal > 0 || xfer_qty_external > 0) && xfer_qty_wo == 0 ? "STOCKTRANSFER" : transferbvwo;
            var wo_qty_committed = Convert.ToDouble(hidTranQTY.Value);
            newtableid = Convert.ToInt32(wo_line_id);
            var details = new NeWODetailCurrent(newtableid);
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
                    var pohisttable = Toolbox.doSQL_dt(conn, @" SELECT IFNULL(woprogchanges_isqty, 0) is_qty, IFNULL(IF(woprogchanges_wasqty = '', null, woprogchanges_wasqty), 0) was_qty FROM woprogchanges WHERE woprogchanges_woprog_id = @v0  AND woprogchanges_ispartno = @v1  AND woprogchanges_comments NOT LIKE '%Transferred To Work Order:%'", new object[] { main_id, details.master_id });
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

            newpartnumber = details.master_id.ToString();
            // because its a new entry on the new work order, make the new part and old part the same.
            oldpartnumber = details.master_id.ToString();
            var exists = inv.part_exists(newpartnumber);
            // Not sure this check will ever be hit...
            if (!exists && details.master_id < 990000)
            {
                update_error("You Cannot Transfer this Part it does not Exist in Inventory");
            }
            var to_details = new NeWODetailCurrent();
            if (details.master_id < 990000) // if its not a labour part
            {
                inv.Load(newpartnumber, WarehouseBusinessUnit.id);
                if (!inv.is_exclude && !is_stocktransfer)
                {
                    // Not Exclude, get the TO work order line
                    var to_line_id =
                        Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(wo_detail_current_id), 0) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transfer_to_woprog_id, newpartnumber });
                    if (to_line_id > 0)
                    {
                        to_details = new NeWODetailCurrent(to_line_id);
                        xfer_to_original_qty = to_details.qty_committed;
                    }
                }
            }
            if (inv.is_qty && details.master_id < 990000)
            {
                var remainsendqty = wo_qty_committed == 0 ? 1 : wo_qty_committed;
                neworigsell = Math.Round(shared.GetSellPrice(details.cost, 0, inv.is_qty, remainsendqty, WorkingBusinessUnit.id32), 2);
                // Should these sell prices be to the second decimal place?
                transfersell =
                    Math.Round(shared.GetSellPrice(details.cost, 0, inv.is_qty, xfer_combined_qty + to_details.qty_committed, WorkingBusinessUnit.id32), 2);
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
            newqty = details.qty_ordered;
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
            AddNewWorkOrderLine(details.woprog_id, transfer_to_woprog_id, details.master_id, false, false, false);
            xfer_wo_qty.Text = "";
            hidTranQTY.Value = "";
            pop_xfer.ShowOnPageLoad = false;
        }
        catch (Exception ee)
        {
            throw;
        }
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
            transferwoprogid = xfer_wo_combo.Value != null ? xfer_wo_combo.Value.ToString() : "9999999";
            xfer_qty_wo = xfer_wo_qty.Text == "" ? 0 : Convert.ToDouble(xfer_wo_qty.Text);
            xfer_qty_internal = xfer_internal_qty.Text == "" ? 0 : Convert.ToDouble(xfer_internal_qty.Text);
            xfer_qty_external = xfer_external_qty.Text == "" ? 0 : Convert.ToDouble(xfer_external_qty.Text);
            xfer_external_select_id = xfer_external_combo.Value != null ? Convert.ToInt32(xfer_external_combo.Value) : 0;
            xfer_internal_select_id = xfer_internal_combo.Value != null ? Convert.ToInt32(xfer_internal_combo.Value) : 0;
            xfer_wo_select_id = xfer_wo_combo.Value != null ? Convert.ToInt32(xfer_wo_combo.Value) : 0;
            xfer_combined_qty = xfer_qty_wo + xfer_qty_internal + xfer_qty_external;
            transfernote = txtTransferReason.Text;
            if (transferwoprogid != "")
            {
                transferbvwo = new List<string>(new[] { "9999999", "9999998", "9999997" }).Contains(transferwoprogid)
                    ? "STOCKTRANSFER"
                    : Toolbox.doSQL_string(conn, @"SELECT woprog_BVWO FROM woprog  WHERE woprog_id =@v0", new object[] { transferwoprogid });
            }
            transferbvwo = (xfer_qty_internal > 0 || xfer_qty_external > 0) && xfer_qty_wo == 0 ? "STOCKTRANSFER" : transferbvwo;
            var is_stock_transfer = transferbvwo == "STOCKTRANSFER";
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
                    var pohisttable = Toolbox.doSQL_dt(conn, @" SELECT IFNULL(woprogchanges_isqty, 0) is_qty, IFNULL(IF(woprogchanges_wasqty = '', null, woprogchanges_wasqty), 0) was_qty FROM woprogchanges WHERE woprogchanges_woprog_id = @v0  AND woprogchanges_ispartno = @v1  AND woprogchanges_comments NOT LIKE '%Transferred To Work Order:%'", new object[] { main_id, details.master_id });
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
            newpartnumber = details.master_id.ToString();
            // because its a new entry on the new work order, make the new part and old part the same.
            oldpartnumber = details.master_id.ToString();
            var exists = inv.part_exists(newpartnumber);
            // Not sure this check will ever be hit...
            if (!exists && details.master_id < 990000)
            {
                update_error("You Cannot Transfer this Part it does not Exist in Inventory");
            }
            if (details.master_id < 990000) // if its not a labour part
            {
                inv.Load(newpartnumber, WarehouseBusinessUnit.id);
            }
            if (inv.is_qty && details.master_id < 990000)
            {
                var remainsendqty = wo_qty_committed == 0 ? 1 : wo_qty_committed;
                neworigsell = Math.Round(shared.GetSellPrice(details.cost, 0, inv.is_qty, remainsendqty, WorkingBusinessUnit.id32), 2);
                // Should these sell prices be to the second decimal place?
                transfersell = Math.Round(shared.GetSellPrice(details.cost, 0, inv.is_qty, xfer_combined_qty, WorkingBusinessUnit.id32), 2);
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
            newqty = details.qty_ordered;
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
                AddNewWorkOrderLine(details.woprog_id, 0, details.master_id, false, false, true);
                if (xfer_combined_qty > 0)
                {
                    Toolbox.doSQL_void(conn, @"UPDATE wo_detail_current SET wo_detail_current_qty_ordered = if(wo_detail_current_qty_ordered - @v1  < 0, 0,wo_detail_current_qty_ordered - @v1 ) WHERE wo_detail_current_id = @v0 ", new object[] { wo_line_id, xfer_combined_qty });
                }
            }
            catch
            {
            }
            xfer_wo_qty.Text = "";
            hidTranQTY.Value = "";
            pop_xfer.ShowOnPageLoad = false;
        }
        catch (Exception ee)
        {
            throw;
        }
    }

    protected void ImageButtonTaxUpdate_Click(object sender, ImageClickEventArgs e)
    {
    }

    #endregion

    #region DDL filler

    protected void Populate_SectionDDL(string _id, string _rev)
    {
        var Sections =
            Toolbox.doSQL_dt(conn, @"SELECT a.id, a.s section, a.detail_id, (SELECT COUNT(id) FROM quote_worksheet b WHERE b.section_id = a.id) dependants FROM ( SELECT id, detail_id, section s FROM quote_section a WHERE a.quote_id = @v0  AND a.revision = @v1 ) a WHERE LENGTH(TRIM(a.s)) > 0 ORDER BY a.s", new object[] { _id, _rev });
        SqlDataSource1.SelectCommand =
            string.Format(
                "SELECT a.id, a.s section, a.detail_id, (SELECT COUNT(id) FROM quote_worksheet b WHERE b.section_id = a.id) dependants FROM ( SELECT id, detail_id, section s FROM quote_section a WHERE a.quote_id = {0} AND a.revision = {1}) a WHERE LENGTH(TRIM(a.s)) > 0 ORDER BY a.s",
                _id, _rev);
        ddlSection.DataSource = Sections;
        ddlSection.DataBind();
        var lic = new ListEditItem("Select Section", "0");
        ddlSection.Items.Insert(0, lic);
        if (ddlSection.Items.Count > 0 && _q["section_id"] == null)
        {
            ddlSection.SelectedIndex = 1;
        }
        else
        {
            ddlSection.SelectedItem = ddlSection.Items.FindByValue(_q["section_id"]);
        }
    }

    protected void Populate_LabourDDLs(string companyid)
    {
        if (origin == "kitted" || origin == "groupings")
        {
            ddlMemberType.DataSource = Toolbox.doSQL_dt("SELECT MemberType_ID AS ID, MemberType_Name AS MemberType FROM membertype WHERE MemberType_Name NOT LIKE '%Do Not%' and active =1 and (is_scheduled=1 or is_team_leader=1 or considered_pm=1 or show_on_ratesheet=1) order by Membertype_Name", null);
            ddlMemberType.DataBind();
            ddlMemberType.Value = 2;
        }
        ddlLabourChargeType.DataSource = Toolbox.doSQL_dt("SELECT PayTypeHours_ID, Description FROM paytypehours order by PayTypeHours_ID", null);
        ddlLabourChargeType.DataBind();
        ddlLabourChargeType.SelectedIndex = 0;
    }

    protected void Populate_GroupSelectDDL()
    {
        var strsql = "";

        strsql = "SELECT ID AS ID, Name FROM inventory_group_hdr order by name";
        ddlKittedParts.DataSource = Toolbox.doSQL_dt(strsql, null);
        ddlKittedParts.DataBind();
        var lic = new ListEditItem("Select Group", "0");
        ddlKittedParts.Items.Insert(0, lic);

        ddlKittedParts.SelectedIndex = 0;
    }

    protected void Populate_QuoteSelectDDL()
    {
        var strsql = "";
        DataTable dt;
        if (origin == "rfq")
        {
            strsql = @"
SELECT 
	CAST(CONCAT(quote_id,revision) AS UNSIGNED) as ID, 
	CAST(CONCAT(quote_id,' V',IFNULL(revision,0),' ',CUSTOMER_NAME(customer_id),' - ',IFNULL(job_description, '')) AS CHAR) as Name
FROM 
	quote_master qm
WHERE 
	status_id NOT IN (9,7) AND
	customer_id IS NOT NULL AND
	business_unit_id = @v0 AND 
	(SELECT COUNT(id) FROM quote_worksheet qw WHERE qw.quote_id = qm.quote_id AND qw.revision = qm.revision and CONCAT('',part_no * 1) = part_no) > 0
order by 
	quote_id,revision desc";

            ddlKittedParts.DataSource = Toolbox.doSQL_dt(strsql, new object[] { hidCompanyID.Value });
            ddlKittedParts.DataBind();
            var lic = new ListEditItem("Select Version", "0");
            ddlKittedParts.Items.Insert(0, lic);
            ddlKittedParts.SelectedIndex = 0;
        }
    }

    protected void Populate_RFQSelectDDL()
    {
        var strsql = "";
        if (origin != "rfq")
        {
            object id = _q["id"];
            var poprogress = new NePOProg(main_id);
            strsql = @"
SELECT
	id,
	CONCAT(id,' - ',name) name
FROM
	rfq_header
WHERE
	(status = 'SENT' OR status = 'CLOSED') AND
	business_unit_id = @v0 AND
	id IN (SELECT rfq_header_id FROM rfq_vendor_list WHERE vendor_id = @v1)
ORDER BY date_close DESC";
            ddlKittedParts.DataSource = Toolbox.doSQL_dt(strsql, new object[] { hidCompanyID.Value, poprogress.poprog_vendor_id });
        }
        else if (origin == "rfq")
        {
            strsql = @"
SELECT
	id,
	CONCAT(id,' - ',name) name
FROM
	rfq_header
WHERE
	(status = 'SENT' OR status = 'CLOSED')
ORDER BY date_close DESC";
            ddlKittedParts.DataSource = Toolbox.doSQL_dt(strsql, null);
        }

        ddlKittedParts.DataBind();
        var lic = new ListEditItem("Select Vendor RFQ", "0");
        ddlKittedParts.Items.Insert(0, lic);
        ddlKittedParts.SelectedIndex = 0;
    }

    protected void Populate_OtherQuoteSelectDDL(int customer_id)
    {
        //     NeWOProg wo = new NeWOProg(main_id);
        var strsql = "";
        if (origin == "quote" || origin == "groupings" || origin == "rfq")
        {
            strsql = @"SELECT cast(concat(quote_master.quote_id,quote_master.revision)as unsigned) as ID, 
concat(cast(quote_master.quote_id as CHAR),' V',Cast(quote_master.revision as CHAR),'  ',quote_master.job_description) as Name
FROM quote_master WHERE quote_master.customer_id = @v0 order by id desc";
            ddlKittedParts.DataSource = Toolbox.doSQL_dt(strsql, new object[] { customer_id });
            ddlKittedParts.DataBind();
            var lic = new ListEditItem("Select Quote", "0");
            ddlKittedParts.Items.Insert(0, lic);

            ddlKittedParts.SelectedIndex = 0;
        }
    }

    protected void Populate_OtherWorkOrderSelectDDL(int customerid)
    {
        var strsql = "";
        if (origin == "workorder" || origin == "quote" || origin == "groupings" || origin == "rfq")
        {
            strsql = string.Format(@"
SELECT 
	a.WOProg_ID AS ID, 
	CONCAT('(0',CAST(a.woprog_bvwo AS UNSIGNED),') ', TRIM(IFNULL(a.woprog_description,'Unknown Description')),'  (', IF(COUNT(b.wo_detail_current_id)=0,COUNT(c.wo_detail_history_id),COUNT(b.wo_detail_current_id)),' items)') AS NAME
FROM 
	woprog a
LEFT JOIN 
	wo_detail_current b 
		ON a.WOProg_ID = b.wo_detail_current_woprog_id
LEFT JOIN 
	wo_detail_history c 
		ON a.woprog_id = c.wo_detail_history_woprog_id
LEFT JOIN inventory_item_master d ON b.wo_detail_current_master_id = d.master_id 
LEFT JOIN inventory_tag e ON d.tag_id = e.tag_id 
LEFT JOIN inventory_item_master f ON c.wo_detail_history_master_id = f.master_id 
LEFT JOIN inventory_tag g ON f.tag_id = g.tag_id 

WHERE 
	a.woprog_customer_id = {0} AND
	COALESCE(b.wo_detail_current_master_id, c.wo_detail_history_master_id) < 990000 AND 
	COALESCE(b.wo_detail_current_master_id, c.wo_detail_history_master_id) != 2139 AND
	COALESCE(d.active, f.active) = 1
GROUP BY 
	a.woprog_id 
HAVING 
	COUNT(b.wo_detail_current_id) > 0 OR COUNT(c.wo_detail_history_id) > 0
ORDER BY 
	woprog_bvwo DESC", customerid);


            ddlKittedParts.DataSource = Toolbox.doSQL_dt(conn, @" SELECT a.WOProg_ID AS ID, CONCAT('(0',CAST(a.woprog_bvwo AS UNSIGNED),') ', TRIM(IFNULL(a.woprog_description,'Unknown Description')),' (', IF(COUNT(b.wo_detail_current_id)=0,COUNT(c.wo_detail_history_id),COUNT(b.wo_detail_current_id)),' items)') AS NAME FROM woprog a LEFT JOIN wo_detail_current b ON a.WOProg_ID = b.wo_detail_current_woprog_id LEFT JOIN wo_detail_history c ON a.woprog_id = c.wo_detail_history_woprog_id LEFT JOIN inventory_item_master d ON b.wo_detail_current_master_id = d.master_id LEFT JOIN inventory_tag e ON d.tag_id = e.tag_id LEFT JOIN inventory_item_master f ON c.wo_detail_history_master_id = f.master_id LEFT JOIN inventory_tag g ON f.tag_id = g.tag_id WHERE a.woprog_customer_id = @v0  AND COALESCE(b.wo_detail_current_master_id, c.wo_detail_history_master_id) < 990000 AND COALESCE(b.wo_detail_current_master_id, c.wo_detail_history_master_id) != 2139 AND COALESCE(d.active, f.active) = 1 GROUP BY a.woprog_id HAVING COUNT(b.wo_detail_current_id) > 0 OR COUNT(c.wo_detail_history_id) > 0 ORDER BY woprog_bvwo DESC", new object[] { customerid });
            ddlKittedParts.DataBind();
            var lic = new ListEditItem("Select Work Order", "0");
            ddlKittedParts.Items.Insert(0, lic);

            ddlKittedParts.SelectedIndex = 0;
        }
    }

    protected void Populate_OtherPurchaseOrderSelectDDL(int vendorid)
    {
        var remove_nesi_cut = current_user.business_unit.is_backoffice ? "" : " AND ph.nesi_cut_po = false";
        if (origin == "purchaseorder" || origin == "rfq")
        {
            ddlKittedParts.DataSource = Toolbox.doSQL_dt(string.Format(@" SELECT ph.poprog_id AS ID, CONCAT('(',co.name,') ', ph.poprog_bvpo,'-',if(ph.poprog_order_description = '', 'No Description',ph.poprog_order_description),' ', COUNT(pc.po_details_poprog_id), ' Items') AS Name FROM poprog_header ph LEFT JOIN po_details_current pc ON ph.poprog_id = pc.po_details_poprog_id LEFT join business_unit co ON ph.business_unit_id = co.id
WHERE ph.poprog_vendor_id = @v0 
{0}  GROUP BY ph.poprog_id HAVING COUNT(pc.po_details_poprog_id) > 0 ORDER BY ph.poprog_bvpo", remove_nesi_cut), new object[] { vendorid });
            ddlKittedParts.DataBind();
            var lic = new ListEditItem("Select Purchase Order", "0");
            ddlKittedParts.Items.Insert(0, lic);

            ddlKittedParts.SelectedIndex = 0;
        }
    }

    protected void Populate_memberDDL()
    {
        //     NeWOProg wo = new NeWOProg(main_id);
        var strsql = "";
        if (ddlMatType.SelectedValue == "2") // if labout is the matl type
        {
            strsql = @"select member_id, CONCAT(ddl_name, '-', member_firstname, ' ' ,member_lastname) AS MemberName FROM member left join business_unit on member.business_unit_id = business_unit.id 
where member_business_unit_id = " + hidCompanyID + " AND member_status = 'Active' ORDER BY business_unit_id, member_firstname, member_lastname";
            ddlMemberName.DataSource = Toolbox.doSQL_dt(conn, @"select member.member_id, CONCAT(ddl_name, '-', member_firstname, ' ' ,member_lastname) AS MemberName FROM member left join business_unit on member.business_unit_id = business_unit.id where business_unit_id =@v0 AND member_status = 'Active' ORDER BY business_unit_id, member_firstname, member_lastname", new object[] { hidCompanyID });
            ddlMemberName.DataBind();
            ddlMemberName.SelectedIndex = 0;
        }
    }

    protected void Populate_membertypeDDL()
    {
        //     NeWOProg wo = new NeWOProg(main_id);
        var strsql = "";
        if (ddlMatType.SelectedValue == "2") // if labout is the matl type
        {
            strsql = "select member_id, CONCAT(ddl_name, '-', member_firstname, ' ' ,member_lastname) AS MemberName " +
                    "FROM member left join business_unit on member.business_unit_id = business_unit.id " +
                    "where " +
                    "member_business_unit_id = @v0 " +
                    "AND member_status = 'Active' " +
                    "ORDER BY business_unit_id, member_firstname, member_lastname";
            ddlMemberName.DataSource = Toolbox.doSQL_dt(strsql, new object[] { hidCompanyID.Value });
            ddlMemberName.TextField = "MemberName";
            ddlMemberName.ValueField = "member_id";
            ddlMemberName.DataBind();
            ddlMemberName.SelectedIndex = 0;
        }
        else if (ddlMatType.SelectedValue == "5") // if other quote is the matl type
        {
            strsql = "SELECT c.Customer_ID AS ID, c.Customer_Name AS MemberType FROM "
                    + "customer AS c  "
                    + "Inner Join quote_master ON c.Customer_ID = quote_master.customer_id "
                    + "WHERE quote_master.business_unit_id =@v0 GROUP BY c.Customer_ID ORDER BY MemberType ASC";
            ddlMemberType.DataSource = Toolbox.doSQL_dt(strsql, new object[] { hidCompanyID.Value });
            ddlMemberType.DataBind();
            var lic = new ListEditItem("Select Customer", "0");
            ddlMemberType.Items.Insert(0, lic);
            ddlMemberType.SelectedIndex = 0;
        }
        else if (ddlMatType.SelectedValue == "6") // if other work order is the matl type
        {
            strsql = @"
SELECT 
	d.customer_ID as ID, 
	d.customer_name as Membertype 
FROM 
	woprog a
LEFT JOIN 
	wo_detail_current b 
		ON	a.WOProg_ID = b.wo_detail_current_woprog_id and 
			b.wo_detail_current_master_id <990000 and 
			b.wo_detail_current_master_id <> 0 
LEFT JOIN 
	wo_detail_history c 
		ON	a.WOProg_ID = c.wo_detail_history_woprog_id and 
			c.wo_detail_history_master_id < 990000 and 
			c.wo_detail_history_master_id <> 0
LEFT JOIN
	customer d
		ON a.woprog_customer_id = d.customer_id
WHERE 
	a.business_unit_id = @v0 AND 
	a.woprog_status NOT IN ('', 'Deleted') AND
	a.woprog_customer_id IS NOT NULL
GROUP BY 
	a.WOProg_customer_id 
ORDER BY 
	d.customer_name";


            var _dt = Toolbox.doSQL_dt(strsql, new object[] { hidCompanyID.Value });
            ddlMemberType.DataSource = _dt;
            ddlMemberType.DataBind();
            var lic = new ListEditItem("Select Customer", "0");
            ddlMemberType.Items.Insert(0, lic);
            ddlMemberType.SelectedIndex = 0;
        }
        else if (ddlMatType.SelectedValue == "7") // if other purchase order is the matl type
        {
            if (origin == "rfq")
            {
                strsql = @"
SELECT  
	ph.poprog_vendor_id as ID, 
	v.vendor_name as Membertype 
FROM
	poprog_header ph
JOIN 
	vendor v
		ON v.vendor_id = ph.poprog_vendor_id
LEFT JOIN 
	po_details_current pc
		ON pc.po_details_poprog_id = ph.poprog_id
WHERE 
	ph.business_unit_id = @v0 
GROUP BY 
	ph.poprog_vendor_id
ORDER BY 
	v.vendor_name";

                //HAVING (COUNT(po_details_current.po_details_poprog_id) > 0)

                ddlMemberType.DataSource = Toolbox.doSQL_dt(strsql, new object[] { hidCompanyID.Value });
                ddlMemberType.DataBind();
                var lic = new ListEditItem("Select Vendor", "0");
                ddlMemberType.Items.Insert(0, lic);
                ddlMemberType.SelectedIndex = 0;
            }
            else
            {
                strsql = string.Format(@"SELECT  poprog_header.POProg_vendor_ID as ID, vendor.vendor_name as Membertype FROM
				poprog_header JOIN vendor on(vendor.vendor_id = poprog_header.POProg_vendor_ID)
				LEFT JOIN po_details_current on (po_details_current.po_details_poprog_id = poprog_header.poprog_id)
				WHERE business_unit_id = {0} AND poprog_id = {1}
				GROUP BY poprog_header.POProg_vendor_ID
				ORDER BY vendor.vendor_name", hidCompanyID.Value, main_id);

                //HAVING (COUNT(po_details_current.po_details_poprog_id) > 0)

                ddlMemberType.DataSource = Toolbox.doSQL_dt(conn, @"SELECT poprog_header.POProg_vendor_ID as ID, vendor.vendor_name as Membertype FROM poprog_header JOIN vendor on (vendor.vendor_id = poprog_header.POProg_vendor_ID) LEFT JOIN po_details_current on (po_details_current.po_details_poprog_id = poprog_header.poprog_id) WHERE poprog_header.business_unit_id = @v0  AND poprog_id = @v1  GROUP BY poprog_header.POProg_vendor_ID ORDER BY vendor.vendor_name", new object[] { hidCompanyID.Value, main_id });
                ddlMemberType.DataBind();
                var lic = new ListEditItem("Select Vendor", "0");
                ddlMemberType.Items.Insert(0, lic);
                ddlMemberType.SelectedIndex = 0;
            }
        }
    }

    protected void Populate_KittedDDLs()
    {
        var strsql = "";
        if (origin == "quote" || origin == "workorder" || origin == "rfq")
        {
            strsql =
                string.Format(
                    @"(SELECT inventory_kit_hdr_ID AS ID, CONCAT('Your Kit - ', inventory_kit_hdr_Name )AS Name FROM inventory_kit_hdr WHERE inventory_kit_hdr_created_by = '{0}' order by inventory_kit_hdr_name) UNION
						(SELECT inventory_kit_hdr_ID AS ID, inventory_kit_hdr_Name AS Name FROM inventory_kit_hdr WHERE inventory_kit_hdr_created_by != '{0}' order by inventory_kit_hdr_name)",
                    current_user.id32);

            ddlKittedParts.DataSource = Toolbox.doSQL_dt(conn, @"(SELECT inventory_kit_hdr_ID AS ID, CONCAT('Your Kit - ', inventory_kit_hdr_Name )AS Name FROM inventory_kit_hdr WHERE inventory_kit_hdr_created_by = @v0  order by inventory_kit_hdr_name) UNION (SELECT inventory_kit_hdr_ID AS ID, inventory_kit_hdr_Name AS Name FROM inventory_kit_hdr WHERE inventory_kit_hdr_created_by != @v0  order by inventory_kit_hdr_name)", new object[] { current_user.id32 });
            ddlKittedParts.DataBind();
            if (origin == "workorder")
            {
                var lic = new ListEditItem("Select Kit", "0");
                ddlKittedParts.Items.Insert(0, lic);
            }
            ddlKittedParts.SelectedIndex = 0;
        }
    }

    #endregion DDL fille

    #region GridViewFill

    protected void FillforKitted(int _id)
    {
        agv.Columns[1].Visible = false;
        agv.Columns[4].Visible = false;
        agv.Columns[33].Visible = false;
        var intID = Convert.ToInt32(_id);

        double totalTM = 0;
        double totalQuote = 0;
        var linecount = 0;
        var custlinecount = 0;
        var sql = string.Format(@"
SELECT 
	A.inventory_kit_dtl_id as id, 
	0 as section_id, 
	A.inventory_kit_dtl_master_id as part_no, 
	0 vend_part_no, 
	FULL_PART_DESCRIPTION_WITH_LABOUR(A.inventory_kit_dtl_master_id,1,'{0}') description, 
	IF(A.inventory_kit_dtl_master_id<990000,GetSellPrice(GET_CURRENT_COST(A.inventory_kit_dtl_master_id,{1}),0,1,A.inventory_kit_dtl_qty, {1}),GetLabourSell(A.inventory_kit_dtl_master_id)) sell, 
	Inv_Max_Cost(A.inventory_kit_dtl_master_id,{1}) cost, 
	A.inventory_kit_dtl_qty qty, 
	if(A.inventory_kit_dtl_master_id<990000,GetSellPrice(GET_CURRENT_COST(A.inventory_kit_dtl_master_id,{1}),0,1,A.inventory_kit_dtl_qty, {1}),GetLabourSell(A.inventory_kit_dtl_master_id))*A.inventory_kit_dtl_qty extTandM,
	0 extended_per, 
	'PlaceHolder'  workorder, 
	A.inventory_kit_dtl_active include, 
	0 section, 
	0 sectionid, 
	'0' dateex, 
	0  qtyrec, 
	0 wo_detail_current_billtypeid, 
	0 emptyval, 
	1.0 qty_per_part, 
	1 wo_detail_current_rec_no,
	0 as active,
	0 AS Division_ID,
	0 AS wo_detail_current_discount,
	0 AS qty_avail,
	0 AS track_part,
	'' origin,
	'' reqdate , 0 Trans, false is_gl_account
FROM 
	inventory_kit_dtl A 
WHERE 
	A.inventory_kit_dtl_hdr_id = {2} 
ORDER BY inventory_kit_dtl_id",
            current_user.business_unit.country,
            hidCompanyID.Value,
            _id
            );

        var kit = Toolbox.doSQL_dt(conn, @" SELECT
  A.inventory_kit_dtl_id AS id,
  0 AS section_id,
  A.inventory_kit_dtl_master_id AS part_no,
  0 vend_part_no,
  FULL_PART_DESCRIPTION_WITH_LABOUR (
    A.inventory_kit_dtl_master_id,
    1,
    @v0
  ) description,
  IF(A.inventory_kit_dtl_master_id < 990000,
    GetSellPrice (	GET_CURRENT_COST (A.inventory_kit_dtl_master_id,@v1),
					0,
					1,
					A.inventory_kit_dtl_qty,
					@v1),
    GetLabourSell (A.inventory_kit_dtl_master_id)
  ) sell,
	GET_CURRENT_COST (A.inventory_kit_dtl_master_id,@v1) cost,
  A.inventory_kit_dtl_qty qty,
  IF(
    A.inventory_kit_dtl_master_id < 990000,
    GetSellPrice (	GET_CURRENT_COST (A.inventory_kit_dtl_master_id,@v1),
      0,
      1,
      A.inventory_kit_dtl_qty,
		@v1
    ),
    GetLabourSell (A.inventory_kit_dtl_master_id)
  ) * A.inventory_kit_dtl_qty extTandM,
  0 extended_per,
  'PlaceHolder' workorder,
  A.inventory_kit_dtl_active include,
  0 section,
  0 sectionid,
  '0' dateex,
  0 qtyrec,
  0 wo_detail_current_billtypeid,
  0 emptyval,
  1.0 qty_per_part,
  1 wo_detail_current_rec_no,
  0 AS active,
  0 AS Division_ID,
  0 AS wo_detail_current_discount,
  0 AS qty_avail,
  0 AS track_part,
  '' origin,
  '' reqdate,
  0 Trans,
  FALSE is_gl_account
FROM
  inventory_kit_dtl A
WHERE A.inventory_kit_dtl_hdr_id = @v2
ORDER BY inventory_kit_dtl_id", new object[] { current_user.business_unit.country, hidCompanyID.Value, _id });
        agv.KeyFieldName = "id";
        agv.DataSource = kit;
        agv.DataBind();
        foreach (DataRow row in kit.Rows)
        {
            linecount++;
            if (row[2].ToString() == "")
            {
                custlinecount++;
            }
            try
            {
                totalTM += Convert.ToDouble(row[8].ToString());
            }
            catch
            {
            }
            try
            {
                totalQuote += Convert.ToDouble(row[9].ToString());
            }
            catch
            {
            }
        }

        lblTotalItemsDisp.Text = linecount.ToString();
        lblTotalCustomDisp.Text = custlinecount.ToString();
        lblQuotedDisp.Text = totalQuote.ToString("C2");
        lblQuoted1.Visible = false;
        lblQuotedDisp1.Visible = false;
        lblTMDisp.Text = totalTM.ToString("C2");
        Title = "Kitted Parts List";
    }

    protected void FillforQuote(int _id, int _rev)
    {

        gridtoggle(new[] { 1, 4, 16 }, false);
        var intID = Convert.ToInt32(string.Concat(_id, _rev));
        Title = "Quote Parts List for " + _id;
        var quote = new quote(intID);
        lblCustNameDisp.Text = quote.txtCustomerName + " - " + quote.txtJobDescription;
        var QuotedBy = new NeMember(Convert.ToInt32(quote.quoted_by));
        lblMembNameDisp.Text = QuotedBy.FullName;
        agv.Columns[24].Caption = "Margin";
        var section_id = _q["section_id"] != null ? _q["section_id"] : "";
        var current_filter = agv.FilterExpression;
        var q_status = Toolbox.doSQL_int(conn, @"SELECT status_id FROM quote_master  WHERE quote_id =@v0 and revision =@v1 ", new object[] { _id, _rev });
        pnl_addnewline.Visible = q_status < 6 || q_status == 12;
        var use_current_cost =
            Toolbox.doSQL_string(conn, @"SELECT use_current_cost FROM quote_master  WHERE quote_id =@v0 and revision=@v1 ", new object[] { _id, _rev }) ==
            "True";

        double totalTM = 0;
        double totalQuote = 0;
        double totalMaterial = 0;
        double totalLabor = 0;
        double totalKitted = 0;
        var linecount = 0;
        var custlinecount = 0;
        var isAllowedCost = member_can_see_cost;
        if ((new int[] { 1, 2, 5 }).Contains(q_status))
        {
            Toolbox.doSQL_void(conn, @"UPDATE quote_worksheet SET cost = GET_CURRENT_LABOUR_COST(part_no, @v2 ), ts = NOW() WHERE quote_id = @v0  AND revision = @v1 AND ts < CURDATE() AND part_no BETWEEN 990000 AND 1000000", new object[] { main_id, rev, hidCompanyID.Value });
        }
        var see_cost_a = isAllowedCost ? "A.cost" : "IF(IS_EXCLUDE(part_no), A.cost, 0)";
        var see_cost_c = isAllowedCost ? "C.cost" : "IF(IS_EXCLUDE(part_no), C.cost, 0)";
        var used_table = use_current_cost ? "quote_worksheet_current_cost" : "quote_worksheet";

        var quoteworksheet = new DataTable();
        quoteworksheet = Toolbox.doSQL_dt(conn, string.Format(@" 
(SELECT
  A.id,
  A.section_id,
  A.part_no,
  A.code vend_part_no,
  A.description description,
  A.original_sell sell,
  {2} cost,
  A.qty,
  (A.original_sell * A.qty * (1- (quote_worksheet_discount / 100))) extTandM,
  extended_per,
  'PlaceHolder' workorder,
  quote_worksheet_part_requested include,
  B.section section,
  b.id sectionid,
  '1' dateex,
  1 AS qtyrec,
  0 AS emptyval,
  0 wo_detail_current_billtypeid,
  1.0 AS qty_per_part,
  1 AS wo_detail_current_rec_no,
  CONCAT(ROUND(((extended_per - (A.cost * A.qty)) / extended_per) * 100,0),'%') AS active,
  0 AS Division_ID,
  quote_worksheet_discount AS wo_detail_current_discount,
  0 AS qty_avail,
  0 AS track_part,
  '' origin,
  '' reqdate,
  0 Trans,
  cost_level,
  IFNULL(inventory_tag.allowed_to_stock,0) allowed_to_stock, false is_gl_account
FROM
  {0} A
LEFT JOIN (SELECT id, section FROM quote_section WHERE quote_id = @v0 AND revision = @v1) b 
	ON a.section_id = b.id
LEFT JOIN inventory_item_master im
    ON im.master_id = A.part_no
LEFT JOIN inventory_tag
    ON inventory_tag.tag_id = im.tag_id
WHERE 
	A.quote_id = @v0 AND 
	A.revision = @v1 AND 
	b.section REGEXP '^[0-9]'
ORDER BY 
	CAST(b.section AS SIGNED), a.id
LIMIT 2000
)
UNION
(
SELECT
  C.id,
  C.section_id,
  C.part_no,
  C.code vend_part_no,
  C.description description,
  C.original_sell sell,
  {1} cost,
  C.qty,
  ROUND((ROUND(C.original_sell, 3) * C.qty * (1- (quote_worksheet_discount / 100))),2) extTandM,
  extended_per,
  'PlaceHolder' workorder,
  quote_worksheet_part_requested include,
  D.section section,
  D.id sectionid,
  '1' dateex,
  1 AS qtyrec,
  0 AS emptyval,
  0 wo_detail_current_billtypeid,
  1.0 AS qty_per_part,
  1 AS wo_detail_current_rec_no,
  CONCAT(ROUND(((extended_per - (c.cost * c.qty)) / extended_per) * 100,0),'%') ACTION,
  0 AS Division_ID,
  quote_worksheet_discount AS wo_detail_current_discount,
  0 AS qty_avail,
  0 AS track_part,
  '' origin,
  '' AS reqdate,
  0,
  cost_level,
  IFNULL(inventory_tag.allowed_to_stock,0) allowed_to_stock,  false is_gl_account
FROM
  {0} C
LEFT JOIN (SELECT id, section FROM quote_section WHERE quote_id = @v0 AND revision = @v1) D
    ON C.section_id = D.id
LEFT JOIN inventory_item_master im
    ON im.master_id = C.part_no
LEFT JOIN inventory_tag
    ON inventory_tag.tag_id = im.tag_id
WHERE 
	C.quote_id = @v0 AND 
	C.revision = @v1 AND 
	D.section REGEXP '^[^0-9]'
ORDER BY 
	D.section, c.id
)", used_table, see_cost_c, see_cost_a), new object[] { id, rev });
        foreach (DataRow dr in quoteworksheet.Rows)
        {
            if (dr["description"].ToString().Contains("%"))
            {
                dr["description"] = Toolbox.do_value_from(dr["description"], false);
                dr.AcceptChanges();
            }
        }
        agv.DataSource = quoteworksheet;
        agv.DataBind();
        var section_name = "";
        //old		gridtoggle(new[] {1, 4, 11, 12, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 25, 26, 27, 29, 30, 31, 32, 33, 35}, false);
        gridtoggle(new[] { 1, 4, 11, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 25, 26, 27, 29, 30, 31, 32, 33, 35 }, false);

        if (!IsPostBack && !IsCallback)
        {
            //old			addtoggle(new[] {3, 5, 7, 11, 12, 15, 16, 17, 18, 21, 23, 24, 25, 26, 27}, false);
            addtoggle(new[] { 3, 5, 7, 11, 12, 15, 16, 17, 18, 21, 23, 24, 25, 26, 27 }, false);
        }
        if (section_id != "")
        {
            try
            {
                section_name =
                    Toolbox.doSQL_string(conn, @"SELECT section FROM quote_section WHERE id = @v0 ", new object[] { section_id });
            }
            catch
            {
                Response.Redirect(string.Format("./pikclist.aspx?id={0}&rev={1}&origin=quote", _q["id"], _q["rev"]), true);
            }
            agv.AutoFilterByColumn(agv.Columns["Sections"], section_name);
            lb_currentfilter.InnerHtml =
                string.Format(
                    @"The sections column has been pre-filtered with the supplied section: <b>""{0}""</b> - <a href='./pikclist.aspx?origin=quote&id={1}&rev={2}' style='color:#000;'>Remove filter</a>",
                    section_name, main_id, main_rev);
            lb_currentfilter.Visible = true;
            ddlSection.SelectedItem = ddlSection.Items.FindByValue(section_id);
        }
        else if (!IsPostBack && !IsCallback)
        {
            agv.FilterExpression = string.Empty;
            agv.DataSource = quoteworksheet;
            agv.DataBind();
            lb_currentfilter.Visible = false;
        }
        if (isDifferentSection)
        {
            agv.FilterExpression = string.Empty;
            agv.DataSource = quoteworksheet;
            agv.DataBind();
            lb_currentfilter.Visible = false;
            isDifferentSection = false;
        }
        if (!current_user.AuthenticatedForPrivilege(81) || !member_can_see_cost)
        {
            gridtoggle(new[] { 24 }, false);
        }
        var show_margin = true;
        double hourstotal = 0;

        foreach (DataRow row in quoteworksheet.Rows)
        {
            linecount++;
            var part_no = 0;

            int.TryParse(row["part_no"].ToString(), out part_no);
            if (row["part_no"].ToString() == "")
            {
                custlinecount++;
            }
            totalTM += Convert.ToDouble(row["extTandM"]);
            totalQuote += Convert.ToDouble(row["extended_per"]);
            if (part_no >= 990000 && part_no < 2000000)
            {
                //	totalLabor += Convert.ToDouble(row["extTandM"]);
                totalLabor += Convert.ToDouble(row["extended_per"]);
                hourstotal += Convert.ToDouble(row["qty"]);
            }
            else if (part_no >= 2000000) // Kit.... we need to dissect it.
            {
                var kit_qty = Convert.ToDouble(row["qty"]);
                var _kit =
                    Toolbox.doSQL_dt(conn,
                        @"SELECT inventory_kit_dtl_master_id master_id, inventory_kit_dtl_qty qty FROM inventory_kit_dtl WHERE inventory_kit_dtl_hdr_id = @v0  AND inventory_kit_dtl_active = TRUE",
                        new object[] { part_no });
                foreach (DataRow _dtl in _kit.Rows)
                {
                    var master_id = (int)_dtl["master_id"];
                    var qty = Convert.ToDouble(_dtl["qty"]);
                    if (master_id >= 990000)
                    {
                        hourstotal += qty * kit_qty;
                    }
                }
                totalKitted += Convert.ToDouble(row["extended_per"]);
            }
            else
            {
                //	totalMaterial += Convert.ToDouble(row["extTandM"]);
                totalMaterial += Convert.ToDouble(row["extended_per"]);
            }

            lblTotalItemsDisp.Text = linecount.ToString();
            lblTotalCustomDisp.Text = custlinecount.ToString();
            lblQuotedDisp.Text = totalQuote.ToString("C2");
            lblQuotedDisp1.Text = Convert.ToDouble(quote.Price).ToString("C2");
            lblTMDisp.Text = totalTM.ToString("C2");
            lbl_benchextddiff.Text = (totalQuote - totalTM).ToString("C2");
            lblTotalMaterialDisp.Text = totalMaterial.ToString("C2");
            lblTotalLaborDisp.Text = totalLabor.ToString("C2");
            lblTotalKittedDisp.Text = totalKitted.ToString("C2");
            lblTotalQuotedLaborDisp.Text = hourstotal.ToString();
        }
        if (show_margin == false)
        {
            lblMargAbovCosDisp.Text = "N/A with $0 Cost";
            lblMargAbovCosDisp_dollars.Text = "N/A with $0 Cost";

        }
        else
        {
            if (current_user.AuthenticatedForPrivilege(81))
            {
                try
                {
                    lblCostDisp.Text = Toolbox.doSQL_double(conn, @"SELECT IFNULL(sum(cost*qty),0) as margin FROM " + used_table + @"  WHERE quote_id =@v0 AND revision =@v1 ", new object[] { quote.QuoteID, quote.Revision }).ToString("C2");

                    lblMargAbovCosDisp.Text = Toolbox.doSQL_double(conn, @"SELECT IFNULL(((@v2-sum(cost*qty))/@v2),0) as margin FROM " + used_table + @"  WHERE quote_id =@v0 AND revision =@v1 ", new object[] { quote.QuoteID, quote.Revision, quote.Price }).ToString("P2");
                    lblMargAbovCosDisp_dollars.Text = (Toolbox.doSQL_double(conn, @"SELECT IFNULL(((@v2-sum(cost*qty))/@v2),0) as margin FROM " + used_table + @"  WHERE quote_id =@v0 AND revision =@v1 ", new object[] { quote.QuoteID, quote.Revision, quote.Price }) * Convert.ToDouble(quote.Price)).ToString("C2");
                    var margin_labour = Toolbox.doSQL_double(conn, @"SELECT IFNULL(((sum(extended_per)-sum(cost*qty))/sum(extended_per)),0) as margin FROM " + used_table + @" WHERE part_no >=990000 and part_no <2000000 and quote_id =@v0 AND revision =@v1 ", new object[] { quote.QuoteID, quote.Revision });
                    lblTotalLaborDisp.Text += "(" + margin_labour.ToString("P0") + ")";

                    var margin_material = Toolbox.doSQL_double(conn, @"SELECT IFNULL(((sum(extended_per)-sum(cost*qty))/sum(extended_per)),0) as margin FROM " + used_table + @"  WHERE part_no <990000 and quote_id =@v0 AND revision =@v1 ", new object[] { quote.QuoteID, quote.Revision });
                    lblTotalMaterialDisp.Text += "(" + margin_material.ToString("P0") + ")";


                }
                catch
                {
                    lblMargAbovCosDisp.Text = "N/A";
                    lblMargAbovCosDisp_dollars.Text = "N/A";

                }

            }
        }
    }

    protected void FillforPartSelect(int _id, int _rev)
    {
        //agv.Columns[1].Visible = false;
        //agv.Columns[4].Visible = false;
        // agv.Columns[16].Visible = false;
        var intID = Convert.ToInt32(string.Concat(_id, _rev));
        var quote = new quote(intID);
        // lblCustNameDisp.Text = quote.txtCustomerName + " - " + quote.txtJobDescription;
        var QuotedBy = new NeMember(Convert.ToInt32(quote.quoted_by));
        lblMembNameDisp.Text = QuotedBy.FullName;

        pnl_header.Visible = false;
        pnl_addnewline.Visible = false;

        var sql =
            "SELECT A.id, A.section_id, A.part_no, A.code AS vend_part_no, A.description, A.original_sell AS sell, A.cost, A.qty, ";
        sql += "(A.original_sell * A.qty) AS extTandM, extended_per, 'PlaceHolder' AS workorder, ";
        sql += "'1' AS include, B.section, b.id AS sectionid, ";
        sql +=
            "'1' AS dateex, 1 as qtyrec, 0 AS wo_detail_current_billtypeid, 0 Trans, '' origin,0 AS emptyval, 1.0 AS qty_per_part, 1 AS wo_detail_current_rec_no, 0 as active,  0 AS Division_ID, 0 AS wo_detail_current_discount, 0 AS qty_avail, 0 AS track_part, '' reqdate ";
        sql += "FROM quote_worksheet AS A, quote_section AS B ";
        sql += "WHERE A.quote_id = @v0";
        sql += " AND A.revision = @v1";
        sql += " AND A.section_id = B.id ";
        sql += "ORDER BY A.section_id, A.id";

        var quoteworksheet = Toolbox.doSQL_dt(conn, sql, new object[] { _id, _rev });
        agv.DataSource = quoteworksheet;
        agv.DataBind();


        /*foreach (DataRow row in quoteworksheet.Rows)
		{
			linecount++;
			if (row[2].ToString() == "")
			{
				custlinecount++;
			}
			try
			{
				totalTM += Convert.ToDouble(row[8].ToString());
			}
			catch { }
			try
			{
				totalQuote += Convert.ToDouble(row[9].ToString());
			}
			catch { }
		}*/

        // lblTotalItemsDisp.Text = linecount.ToString();
        // lblTotalCustomDisp.Text = custlinecount.ToString();
        // lblQuotedDisp.Text = totalQuote.ToString("C2");
        //lblTMDisp.Text = totalTM.ToString("C2");

        //  Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", javaScript, true);
        // ClientScript.RegisterStartupScript(Page.GetType(), "SomestartupScript", "window.opener.location.reload();");
    }

    protected void FillForPO(int _id)
    {
        var progress = new NePOProg(Convert.ToInt32(_id));
        //	    ddlWorkOrder.ToolTip = "GL Accounts are selected at the tax entity level on the GL Structure Admin Page";
        if (progress.poprog_status >= OpsPOStatus.IssuedWaitingforPackingSlip)
        {
            note_history.ReadOnly = true;
            btn_edit_note.Visible = false;
        }
        po_n = progress.poprog_bvpo;
        po_status = progress.poprog_status;
        //lblCustNameDisp.Text = progress. + " - " + progress.Description;
        //NeMember QuotedBy = new NeMember(Convert.ToInt32(progress.intProjectManager));
        //lblMembNameDisp.Text = QuotedBy.FullName;
        var sql = string.Format(@"
SELECT
	po_details_id AS id,
	1 AS section_id,
	po_details_part_no AS part_no,
	po_details_vendor_part_no AS vend_part_no,
	po_details_description AS description,
	(po_details_qty_ordered * po_details_cost) AS extTandM,
	0 AS sell,
	po_details_qty_ordered AS qty,
	po_details_cost AS cost,
	(po_details_cost * po_details_qty_received) AS extended_per,
	po_details_woprog_id AS workorder,
	'1' AS include,
	'none' AS section,
	0 AS sectionid,
	po_details_date_expected AS dateex,
	po_details_qty_received AS qtyrec,
	0 AS wo_detail_current_billtypeid,
	0.0 AS emptyval,
	po_details_vendor_qty_per AS qty_per_part,
	po_details_rec_no AS wo_detail_current_rec_no,
	po_details_line_active AS active,
	po_details_division_id AS Division_ID,
	0 AS wo_detail_current_discount,
	0 AS qty_avail,
	0 AS track_part,
	'' AS reqdate,
	'' AS origin,
	0 AS Trans,
is_gl_account
FROM
	po_details_current 
WHERE 
	po_details_poprog_id = {0} 
ORDER BY 
	po_details_rec_no", _id);
        double totalOrdered = 0;
        double totalRec = 0;
        var linecount = 0;
        var wotable = Toolbox.doSQL_dt(conn, @" SELECT po_details_id AS id, 1 AS section_id, po_details_part_no AS part_no, po_details_vendor_part_no AS vend_part_no, 
po_details_description AS description, (po_details_qty_ordered * po_details_cost) AS extTandM, 0 AS sell, po_details_qty_ordered AS qty, po_details_cost AS cost, 
(po_details_cost * po_details_qty_received) AS extended_per, po_details_woprog_id AS workorder, '1' AS include, 'none' AS section, 0 AS sectionid,
po_details_date_expected AS dateex, po_details_qty_received AS qtyrec, 0 AS wo_detail_current_billtypeid, 0.0 AS emptyval, po_details_vendor_qty_per AS qty_per_part,
po_details_rec_no AS wo_detail_current_rec_no, po_details_line_active AS active, po_details_division_id AS Division_ID, 0 AS wo_detail_current_discount, 0 AS qty_avail,
0 AS track_part, '' AS reqdate, '' AS origin, 0 AS Trans, is_gl_account ,po_details_reference
FROM po_details_current 
WHERE po_details_poprog_id = @v0  ORDER BY po_details_rec_no", new object[] { _id });
        var test = "";
        foreach (DataRow row in wotable.Rows)
        {
            test = Toolbox.do_value_from(row["description"], false);
            row["description"] = test;
            row.AcceptChanges();

            test = Toolbox.do_value_from(row["vend_part_no"], false);
            row["vend_part_no"] = test;
            row.AcceptChanges();
            linecount++;

            try
            {
                totalOrdered += Convert.ToDouble(row[5].ToString());
            }
            catch
            {
            }
            try
            {
                totalRec += Convert.ToDouble(row[9].ToString());
            }
            catch
            {
            }
        }
        //
        // [1075] [Nesi UAT] We need to control the visibility of the "All Items Complete" button on a PO a bit better.
        // Below code is to keep a copy of current po line item complete/active status.
        // And the poLineCompleteInfo will only be used when receiving po line items.
        //
        this.poLineCompleteInfo = new List<PoCompleteState> { };
        try
        {
            if (wotable != null && wotable.Rows != null)
            {
                foreach (DataRow row in wotable.Rows)
                {
                    var item = new PoCompleteState { };

                    item.poLineId = Convert.ToInt32(row["id"].ToString());
                    item.poLineState = Convert.ToInt32(row["active"].ToString());
                    item.received = NePOProg.ReceiptsExist(_id, Convert.ToDouble(row["po_details_reference"]));
                    poLineCompleteInfo.Add(item);
                }
            }
        }
        catch (Exception ee)
        {
            Toolbox.do_errorLog(ee, "Could not cache poLineCompleteInfo");
        }


        //lblTotalItemsDisp.Text = linecount.ToString();
        //lblTotalCustomDisp.Text = custlinecount.ToString();
        //lblQuotedDisp.Text = totalQuote.ToString("C2");
        //lblTMDisp.Text = totalTM.ToString("C2");

        lblTM.Text = "Total Ordered";
        lblTMDisp.Text = totalOrdered.ToString("C2");
        lblCost.Text = "Total Received";
        lblCostDisp.Text = totalRec.ToString("C2");
        //lblTotalItems
        lblTotalItemsDisp.Text = linecount.ToString();
        pop_xfer.Enabled = false;
        ASPxpuHistory.Enabled = false;
        Popup_GroupSelect.Enabled = pnl_addnewline.Visible;
        ASPxPopupControlPicture.Enabled = false;
        gv_PartsDeleted.Visible = false;
        popup.Enabled = false;
        ASPxPopupSectionAdd.Enabled = false;
        agv.DataSource = wotable;
        dt_master_locations = null;
        agv.DataBind();
        rp_Main.ShowHeader = true;
        fill_edit_form_workorder_ddl();
        footer_save_toggle(true);
        updatePOTotal();
        //     ((DevExpress.Web.GridViewDataComboBoxColumn)agv.Columns["workorder"]).PropertiesComboBox.DataSource = SqlDataSource3;       
        Title = "PO Parts List";



    }

    protected void FillForRFQ(int _id)
    {
        var sql = string.Format(@"
SELECT
	a.id,
	1 section_id,
	a.master_id part_no,
	'' vend_part_no,
	full_part_description(a.master_id, true, c.country) description,
	0 extTandM,
	0 sell,
	qty qty,
	0 cost,
	0 qty_avail,
	0 extended_per,
	'' workorder,
	1 dateex,
	0 qtyrec,
	1 include,
	'' section,
	0 sectionid,
	0 wo_detail_current_billtypeid,
	0 emptyval,
	0 qty_per_part,
	'' wo_detail_current_rec_no,
	0 active,
	0 Division_ID,
	0 wo_detail_current_discount,
	0 AS track_part,
	'' origin,
	required_date reqdate, 
	0 Trans
, false is_gl_account
FROM
	rfq_part_list a
LEFT JOIN
	rfq_header b ON a.rfq_header_id = b.id
LEFT join
	business_unit c ON b.business_unit_id = c.id
WHERE
	a.rfq_header_id = {0}", _id);
        if (ctrlname == null ||
            ctrlname.ID != "ddlMatType" && ctrlname.ID != "ddlKittedParts" && ctrlname.ID != "ddlMemberType")
        {
            addtoggle(new[] { 1, 3, 5, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 21, 22, 23, 24, 25, 27 }, false);
            addtoggle(new[] { 2, 4, 6, 8, 19, 20, 26 }, true);
            gridtoggle(
                new[]
                    {1, 2, 4, 7, 8, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 34, 35},
                false);
            gridtoggle(new[] { 3, 5, 6, 13, 33 }, true);
            ddlMatType.SelectedIndex = 0;
        }
        add_hc4.Width = "100px";
        pnl_header.Visible = false;
        header_print.Visible = false;
        header_printwo.Visible = false;
        header_printwo_unf.Visible = false;
        lblQty.Text = "QTY";
        add_lc1.Visible = false;
        agv.Settings.ShowPreview = false;
        agv.DataSource = Toolbox.doSQL_dt(conn, sql, null);
        agv.DataBind();
        Title = "RFQ Parts List for " + _id;
    }

    protected void FillforGroupings(int _id)
    {
        agv.Columns[33].Visible = false;
        Title = "Group Parts List";
        var sql = string.Format(@"
SELECT 
	b.id id, 
	1 section_id, 
	b.master_id part_no, 
	'None' vend_part_no, 
	`full_part_description_with_labour`(b.master_id,1,'{0}') description, 
	0 extTandM, 
	proc_GetInvSellPrice(b.master_id,{1}) sell, 
	0 qty, 
	0 cost, 
	0 extended_per, 
	'PlaceHolder' workorder,
	'1' dateex, 
	1 qtyrec, 
	0 wo_detail_current_billtypeid, 
	'1' include, 
	'none' section, 
	0 sectionid, 
	0 emptyval, 
	1.0 qty_per_part, 
	1 wo_detail_current_rec_no,
	0 as active,
	0 AS Division_ID,
	0 as wo_detail_current_discount,
	0 AS qty_avail,
	0 AS track_part,
	'' origin,
	'' AS reqdate , 0 Trans , false is_gl_account 
FROM 
	inventory_group_dtl b  
WHERE 
	group_id = {2} 
ORDER BY b.id",
            current_user.business_unit.country,
            hidCompanyID.Value,
            _id
            );

        var _dt = Toolbox.doSQL_dt(conn, @" SELECT b.id id, 1 section_id, b.master_id part_no, 'None' vend_part_no, `full_part_description_with_labour`(b.master_id,1,@v0 ) description, 0 extTandM, proc_GetInvSellPrice(b.master_id,@v1 ) sell, 0 qty, 0 cost, 0 extended_per, 'PlaceHolder' workorder, '1' dateex, 1 qtyrec, 0 wo_detail_current_billtypeid, '1' include, 'none' section, 0 sectionid, 0 emptyval, 1.0 qty_per_part, 1 wo_detail_current_rec_no, 0 as active, 0 AS Division_ID, 0 as wo_detail_current_discount, 0 AS qty_avail, 0 AS track_part, '' origin, '' AS reqdate , 0 Trans, false is_gl_account FROM inventory_group_dtl b WHERE group_id = @v2  ORDER BY b.id", new object[] { current_user.business_unit.country, hidCompanyID.Value, _id });
        agv.DataSource = _dt;
        agv.DataBind();
    }

    protected void FillforAlternates(int _id)
    {
        agv.Columns[33].Visible = false;
        Title = "Alternate Parts List for " + _id;
        var _dt = Toolbox.doSQL_dt(conn, @" SELECT inventory_Alternate_ID id, 1 section_id, a.master_id part_no, 'None' vend_part_no, full_part_description(a.master_id,1,@v0 ) description, 0 extTandM, proc_GetInvSellPrice(a.master_id,@v1 ) sell, 0 qty, 0 cost, 0 extended_per, 'PlaceHolder' workorder, '1' dateex, 1 qtyrec, '1' include, 'none' section, 0 sectionid, 0 wo_detail_current_billtypeid, 0 emptyval, 1.0 qty_per_part, 1 wo_detail_current_rec_no, 0 as active, 0 AS Division_ID, 0 AS wo_detail_current_dicount, 0 AS qty_avail, 0 AS track_part, '' origin, '' AS reqdate, 0 Trans, false is_gl_account FROM inventory_item_master a INNER JOIN inventory_alternate b ON a.master_id = b.inventory_alternate_alternate_master_id WHERE inventory_Alternate_origin_master_id = @v2  ORDER BY inventory_Alternate_id", new object[] { current_user.business_unit.country, hidCompanyID.Value, _id });
        agv.KeyFieldName = "id";
        agv.DataSource = _dt;
        agv.DataBind();
    }

    #endregion

    #region GridviewRow Updating And Deleting

    protected void agv_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
    {
        try
        {
            double oldext = 0;
            double newext = 0;
            var editingkey = e.Keys[0];
            newtableid = Convert.ToInt32(editingkey.ToString());
            var this_inv = new inventory();
            if (e.NewValues.Contains("wo_detail_current_rec_no") && e.NewValues["wo_detail_current_rec_no"] != null)
            {
                newrecno = Convert.ToInt32(e.NewValues["wo_detail_current_rec_no"].ToString());
                oldrecno = Convert.ToInt32(e.OldValues["wo_detail_current_rec_no"].ToString());
            }
            else if (origin == "purchaseorder")
            {
                newrecno = Convert.ToInt32(e.NewValues["wo_detail_current_rec_no"].ToString());
                oldrecno = Convert.ToInt32(e.OldValues["wo_detail_current_rec_no"].ToString());
            }
            if (e.NewValues.Contains("part_no") && e.NewValues["part_no"] != null)
            {
                newpartnumber = e.NewValues["part_no"].ToString();
                oldpartnumber = e.OldValues["part_no"].ToString();
                if (origin == "purchaseorder")
                {
                    if (newpartnumber != "0" && oldpartnumber == "0")
                    {
                        throw new Exception("You cannot change the part number on a comment line");
                    }
                    if (newpartnumber == "0" && oldpartnumber != "0")
                    {
                        throw new Exception("You cannot change a valid part line to a comment line");
                    }
                }
            }
            var this_part_no = Convert.ToInt32(newpartnumber);
            if (this_part_no > 0)
            {
                if (this_part_no < 990000)
                {
                    this_inv.Load(this_part_no, WarehouseBusinessUnit.id);
                }
            }


            ///TODO: These should be ran through an ifnull check, not a empty try/catch.
            if (e.NewValues.Contains("sectionid") && e.NewValues["sectionid"] != null)
            {
                newsectionid = e.NewValues["sectionid"].ToString();
                oldsectionid = e.OldValues["sectionid"].ToString();
            }
            var woprog_id = 0;
            if (e.NewValues.Contains("workorder") && e.NewValues["workorder"] != null)
            {
                newworkorderid = e.NewValues["workorder"].ToString();
                oldworkorderid = e.OldValues["workorder"].ToString();
            }
            if (origin == "purchaseorder")
            {
                //If work order combo box is disabled the new work order id will be 0, in this case use the old one, which is correct
                if (newworkorderid == "0")
                {
                    newworkorderid = oldworkorderid;
                }
                int.TryParse(newworkorderid, out woprog_id);
                new_is_gl_account = woprog_id < 20000;
            }
            hidWo.Value = newworkorderid;
            if (e.NewValues.Contains("dateex") && e.NewValues["dateex"] != null)
            {
                newDateExpected = e.NewValues["dateex"].ToString();
                oldDateExpected = e.OldValues["dateex"].ToString();
            }
            if ((origin == "workorder" || origin == "quote") && Convert.ToInt32(newpartnumber) < 990000)
            {
                newdiscount = Convert.ToDouble(e.NewValues["wo_detail_current_discount"]);
                olddiscount = Convert.ToDouble(e.OldValues["wo_detail_current_discount"]);
            }
            if (e.NewValues.Contains("vend_part_no") && e.NewValues["vend_part_no"] != null)
            {
                newvendorpartnumber = e.NewValues["vend_part_no"].ToString();
                oldvendorpartnumber = e.OldValues["vend_part_no"].ToString();
            }

            if (e.NewValues.Contains("cost") && e.NewValues["cost"] != null)
            {
                double.TryParse(e.NewValues["cost"].ToString(), out newcost);
                double.TryParse(e.OldValues["cost"].ToString(), out oldcost);
                if (origin == "workorder")
                {
                    newcost = can_edit_part_cost
                        ? Convert.ToDouble(e.NewValues["cost"])
                        : Toolbox.doSQL_double(conn, @"Select wo_detail_current_price_cost from wo_detail_current  where wo_detail_current_id =@v0", new object[] { newtableid });
                }
                else if (origin == "purchaseorder")
                {
                    if (newpartnumber != "0" && newcost == 0)
                    {
                        throw new Exception("You cannot edit a line item's cost to zero");
                    }
                   
                }
                else if (origin == "quote")
                {
                    // Are they allowed to see quote? If not it may need to be supplied.
                    if (!member_can_see_cost || newcost == 0 && this_inv.part_is_loaded)
                    {
                        newcost = this_inv.cost_price_branch;
                        oldcost = this_inv.cost_price_branch;
                    }
                }
            }

            if (e.NewValues.Contains("description") && e.NewValues["description"] != null)
            {
                newdescription = e.NewValues["description"].ToString();
                olddescription = e.OldValues["description"].ToString();
                if (oldpartnumber == "777" && newpartnumber != "777")
                {
                    newdescription = this_inv.description;
                }
            }

            if (e.NewValues.Contains("qtyrec") && e.NewValues["qtyrec"] != null)
            {
                new_qty_committed = Convert.ToDouble(e.NewValues["qtyrec"].ToString());
                old_qty_committed = Convert.ToDouble(e.OldValues["qtyrec"].ToString());
            }
            if (e.NewValues.Contains("qty") && e.NewValues["qty"] != null)
            {
                newqty = Convert.ToDouble(e.NewValues["qty"]);
                oldqty = Convert.ToDouble(e.OldValues["qty"]);
                if (old_qty_committed > 0)
                {
                    if (newqty < oldqty && newqty < old_qty_committed && old_qty_committed != 0)
                    {
                        throw new Exception("Adjusting the quantity ordered to be below the current level of quantity received is not allowed.");
                    }
                }
                else
                {
                    if (newqty > oldqty && newqty > old_qty_committed && old_qty_committed != 0)
                    {
                        throw new Exception("Adjusting the quantity ordered to be above the current level of quantity received is not allowed.");
                    }
                }
            }


            if (e.NewValues.Contains("extended_per") && e.NewValues["extended_per"] != null)
            {
                newquoteextd = Convert.ToDouble(e.NewValues["extended_per"].ToString().Replace(",", ""));
                oldquoteextd = Convert.ToDouble(e.OldValues["extended_per"].ToString().Replace(",", ""));
            }

            if (e.NewValues.Contains("reqdate") && Convert.ToString(e.NewValues["reqdate"]) != null &&
                Convert.ToString(e.OldValues["reqdate"]) != "")
            {
                newdatereq = Toolbox.MySQL_shortdt(Convert.ToDateTime(e.NewValues["reqdate"]));
                olddatereq = Toolbox.MySQL_shortdt(Convert.ToDateTime(e.OldValues["reqdate"]));
            }

            if (e.NewValues.Contains("extTandM") && e.NewValues["extTandM"] != null)
            {
                newext = Convert.ToDouble(e.NewValues["extTandM"].ToString().Replace(",", ""));
                oldext = Convert.ToDouble(e.OldValues["extTandM"].ToString().Replace(",", ""));
            }

            if (e.NewValues.Contains("qty_per_part") && e.NewValues["qty_per_part"] != null)
            {
                newqtyperpart = Convert.ToDouble(e.NewValues["qty_per_part"].ToString());
                oldqtyperpart = Convert.ToDouble(e.OldValues["qty_per_part"].ToString());
            }

            if (e.NewValues.Contains("sell") && e.NewValues["sell"] != null && origin == "Quote")
            {
                if (newquoteextd != newext && newqty != 0)
                {
                    newsell = newquoteextd / newqty;
                }
                else
                {
                    newsell = Convert.ToDouble(e.NewValues["sell"].ToString().Replace(",", ""));
                }
            }
            else if (e.NewValues.Contains("sell") && e.NewValues["sell"] != null)
            {
                newsell = Convert.ToDouble(e.NewValues["sell"].ToString());
                oldsell = Convert.ToDouble(e.OldValues["sell"].ToString());
            }

            if (e.NewValues.Contains("sell"))
            {
                neworigsell = Convert.ToDouble(e.NewValues["sell"].ToString().Replace(",", ""));
                oldorigsell = Convert.ToDouble(e.OldValues["sell"].ToString().Replace(",", ""));
            }

            if (e.NewValues.Contains("include") && e.NewValues["include"] != null)
            {
                if (e.NewValues["include"].ToString() == "1")
                {
                    newinclude = true;
                }
                else
                {
                    newinclude = false;
                }
                if (e.OldValues["include"].ToString() == "1")
                {
                }
            }
            else
            {
                newinclude = false;
            }

            if (e.NewValues.Contains("wo_detail_current_billtypeid") && e.NewValues["wo_detail_current_billtypeid"] != null)
            {
                newbilltype = e.NewValues["wo_detail_current_billtypeid"].ToString();
                oldbilltype = e.OldValues["wo_detail_current_billtypeid"].ToString();
            }
            else
            {
                newbilltype = "0";
            }

            if (e.NewValues.Contains("Division_ID") && e.NewValues["Division_ID"] != null)
            {
                newdivisionid = e.NewValues["Division_ID"].ToString();
                olddivisionid = e.OldValues["Division_ID"].ToString();
            }

            if (e.NewValues.Contains("track_part") && e.NewValues["track_part"] != null)
            {
                NewTrack = Convert.ToInt32(e.NewValues["track_part"].ToString());
                OldTrack = Convert.ToInt32(e.OldValues["track_part"].ToString());
            }

            if (origin == "kitted")
            {
                UpdateKittedLine();
                FillforKitted(main_id);
            }
            else if (origin == "purchaseorder")
            {
                try
                {
                    UpdatePOLine();
                }
                catch (PositiveNegativeException ex)
                {
                    throw ex;
                }
                catch (Exception ee)
                {
                    throw new Exception("Error during po updating: " + ee);
                }
                FillForPO(main_id);
                populate_sds_workorder_ddl(hidCompanyID.Value, GetExp(hidCompanyID.Value));
                fill_edit_form_workorder_ddl();
                SqlDeptDataSource2.SelectCommand =
                    string.Format(@"(SELECT 0 AS Division_ID, 'N/A' AS Division_Name) UNION (SELECT id Division_ID, ddl_name AS Division_Name
														FROM business_unit
														WHERE id = {0} ORDER BY id)", hidCompanyID.Value);
            }
            else if (origin == "rfq")
            {
                var pl = new rfq_part_list(Convert.ToInt32(editingkey));
                pl.qty = newqty;
                pl.required_date = newdatereq == "" ? null : (DateTime?)Convert.ToDateTime(newdatereq);
                pl.save();
                FillForRFQ(main_id);
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "binder", "bind_tooltips();", true);
            agv.JSProperties["cplineid"] = "";
            e.Cancel = true;
            agv.CancelEdit();
        }
        catch (Exception ee)
        {
            Toolbox.do_errorLog(ee);
            throw;
        }
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
        switch (origin)
        {
            #region Quote

            case "quote":
                // string PartReq = Toolbox.doSQL_string(conn,@"SELECT quote_worksheet_part_requested FROM quote_worksheet  WHERE id =@v0", new object[] { Deleteparttableid });
                //if(PartReq == "1")
                //  throw new Exception("You Cannot Delete an Item For Which There is a part request.");
                var _quote = new quote(main_id);
                _quote.load_worksheet_row(Convert.ToInt32(editingkey));
                if (_quote.worksheet_consignment_id != 0)
                {
                    var _consign = new consignment(_quote.worksheet_consignment_id);
                    _consign.load();
                    _consign.status = consignment.StatusType.WaitingToBeQuoted;
                    _consign.save();
                }
                Toolbox.doSQL_void(conn, @"DELETE FROM quote_worksheet  WHERE id =@v0 limit 1 ", new object[] { Deleteparttableid });

                FillforQuote(main_id, main_rev);
                SqlDataSource1.SelectCommand =
                    string.Format(
                        "SELECT a.id, a.section, a.detail_id, (SELECT COUNT(*) FROM quote_worksheet b WHERE b.section_id = a.id) dependants FROM quote_section a WHERE a.quote_id = {0} AND a.revision = {1} AND LENGTH(TRIM(a.section)) > 0 ORDER BY section",
                        main_id, rev);

                break;

            #endregion

            case "kitted":
                Toolbox.doSQL_void(conn, @"DELETE FROM inventory_kit_dtl  WHERE inventory_kit_dtl_id =@v0 limit 1 ", new object[] { Deleteparttableid });
                FillforKitted(main_id);
                break;
            case "groupings":
                Toolbox.doSQL_void(conn, @"DELETE FROM inventory_group_dtl WHERE group_id = @v0  AND id = @v1  LIMIT 1", new object[] { main_id, e.Keys[0] });
                FillforGroupings(main_id);
                break;
            case "alternates":
                Toolbox.doSQL_void(conn, @"DELETE FROM inventory_Alternate WHERE inventory_Alternate_ID = @v0  LIMIT 1", new object[] { e.Keys[0] });
                FillforAlternates(main_id);
                break;
            case "rfq":
                var pl = new rfq_part_list(Convert.ToInt32(e.Keys[0]));
                pl.delete();
                FillForRFQ(main_id);
                break;
            case "purchaseorder":
                var progress = new NePOProg(main_id);
                var strdel = "DELETE FROM po_details_current WHERE po_details_id = " + Deleteparttableid + " LIMIT 1";
                if (progress.poprog_status != OpsPOStatus.NotIssued)
                {
                    throw new Exception("You can not delete a part from a PO that has already been ordered or approved for order");
                }
                Toolbox.doSQL_void(conn, @"DELETE FROM po_details_current  WHERE po_details_id =@v0 limit 1 ", new object[] { Deleteparttableid });
                updatePOTotal();
                var details = new NEPO_Details_Current();
                FillForPO(main_id);
                var javaScript = "parent.location.href = parent.location.href;";
                ScriptManager.RegisterStartupScript(this, GetType(), "Update", javaScript, true);
                break;
        }
        e.Cancel = true;
        agv.CancelEdit();
        if (origin != "purchaseorder" && origin != "workorder")
        {
            //string javaScript = "window.opener.location.reload();";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Update", javaScript, true);
        }
        else if (origin == "workorder")
        {
            //   string javaScript = "parent.opener.location.reload();";
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "Update", javaScript, true);
        }
    }

    protected void agv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {

        if (e.Parameters == "delete_multiple")
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
                    var column_chkbox = (GridViewDataColumn)agv.Columns["select"];
                    var chkbox = (CheckBox)agv.FindRowCellTemplateControl(i, column_chkbox, "chk_indiv");
                    if (chkbox.Checked)
                    {
                        var part_no = agv.GetRowValues(i, "part_no").ToString();
                        var qty_committed = Convert.ToDouble(agv.GetRowValues(i, "qtyrec"));
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

    }

    #endregion

    #region Adding And Updating Lines

    protected void AddNewQuoteSectionLine()
    {
        var quotelines = new quote();
        quotelines.worksheet_section_id = Convert.ToInt32(newsectionid);
        quotelines.worksheetquote_id = main_id;
        quotelines.worksheet_rev = Convert.ToInt32(rev);
        quotelines.worksheet_sell = Toolbox.do_Round(newsell, 3);
        quotelines.worksheet_partno = newpartnumber == null ? "" : newpartnumber;
        ;
        quotelines.worksheet_originalsell = Toolbox.do_Round(neworigsell, 3);
        quotelines.worksheet_extendedper = Toolbox.do_Round(newquoteextd, 3);
        quotelines.worksheet_description = newdescription.Replace("'", "");
        quotelines.worksheet_description += " " + txtQuoteLabDesc.Text.Replace("'", "");
        quotelines.worksheet_member_id = Convert.ToInt32(current_user.id);
        quotelines.worksheet_cost = Toolbox.do_Round(newcost, 3);
        quotelines.worksheet_code = newvendorpartnumber == null ? "" : newvendorpartnumber;
        quotelines.cost_level = cost_level;

        var setqty = Convert.ToDouble(newqty);
        quotelines.worksheet_qty = setqty;
        if (ddlMatType.SelectedValue == "9") // if the part is a Repair
        {
            quotelines.worksheet_consignment_id = Convert.ToInt32(ddlWorkOrder.Value);
            quotelines.worksheet_qty = 1;
        }
        var discountablepart = 0;
        int.TryParse(newpartnumber, out discountablepart);

        if (
            chkQuoteDiscount.Checked =
                true && (discountablepart < 990000 || discountablepart >= 2000000) && discountablepart != 0 &&
                ddlMatType.SelectedValue != "9")
        {
            try
            {
                quotelines.quote_worksheet_discount = Convert.ToDouble(ASPxTextDiscount.Text);
            }
            catch
            {
                quotelines.quote_worksheet_discount = 0;
            }
        }
        else
        {
            quotelines.quote_worksheet_discount = 0;
        }
        if (ddlMatType.SelectedValue == "9")
        {
            var ico = new consignment(Convert.ToInt32(ddlWorkOrder.SelectedItem.Value));
            ico.status = consignment.StatusType.Quoted;
            ico.save();
        }
        quotelines.AddNewWorkSheetLine();
        //ddlMatType.SelectedIndex			= 0;
        addtoggle(new int[] { }, true);
        gridtoggle(new int[] { }, true);
        addtoggle(new[] { 3, 5, 7, 11, 12, 15, 16, 17, 18, 21, 23, 24, 25, 26 }, false);
        // what the F@#$% is this doing here?  i mean seriously...
        //gridtoggle(new int[]{1,4,12,14,15,16,17,18,19,20,21,22,23,24,25,26,27,29,30,31,32}, false);
        if (newpartnumber != "")
        {
            if (Convert.ToInt32(newpartnumber) < 990000)
            {
                UpdateSellPriceForMultipleParts(Convert.ToInt32(newpartnumber), Convert.ToDouble(setqty));
            }
        }
    }

    protected void UpdateQuoteLine()
    {
        var inv = new inventory();
        var sectionid = newsectionid;
        var quotelines = new quote();
        quotelines.load_worksheet_row(newtableid);
        quotelines.worksheet_section_id = Convert.ToInt32(sectionid);
        quotelines.worksheetquote_id = Convert.ToInt32(main_id);
        quotelines.worksheet_rev = Convert.ToInt32(rev);
        quotelines.worksheet_sell = Math.Round(newsell, 3);
        quotelines.worksheet_partno = newpartnumber == null ? "" : newpartnumber;
        var master_id = 0;
        int.TryParse(newpartnumber, out master_id);
        if (master_id > 0 && master_id < 990000)
        {
            inv.Load(master_id, WarehouseBusinessUnit.id);
        }
        var is_exclude = master_id != 0 ? true : inv.is_exclude;
        quotelines.worksheet_cost = member_can_see_cost ? Toolbox.do_Round(newcost, 3) : quotelines.worksheet_cost;
        if (inv.part_is_loaded)
        {
            var this_old_sell = shared.GetSellPrice(quotelines.worksheet_cost, 0, is_exclude, oldqty, WorkingBusinessUnit.id32);
            var this_new_sell = shared.GetSellPrice(quotelines.worksheet_cost, 0, is_exclude, newqty, WorkingBusinessUnit.id32);
            if (oldqty != newqty) // :PATCH
            {
                // Need to check if the ~extd was being refactored while the user pressed the save button
                if (newquoteextd == oldquoteextd || newquoteextd == 0)
                {
                    // The extd sell needs to be set to use the new quantity.
                    neworigsell = this_new_sell;
                    newsell = this_new_sell;
                    newquoteextd = newsell * newqty;
                }
            }
        }
        else if (newqty != oldqty)
        {
            newquoteextd = newsell * newqty;
        }
        quotelines.worksheet_originalsell = Toolbox.do_Round(neworigsell, 3);
        quotelines.worksheet_sell = Math.Round(newsell, 3);
        quotelines.worksheet_extendedper = Toolbox.do_Round(newquoteextd, 3);
        quotelines.worksheet_description = newdescription.Replace("'", "");
        quotelines.worksheet_code = newvendorpartnumber == null ? "" : newvendorpartnumber;
        quotelines.worksheet_member_id = Convert.ToInt32(current_user.id);
        quotelines.worksheet_qty = newqty;
        quotelines.quote_worksheet_discount = newdiscount;
        quotelines.worksheet_id = newtableid;
        quotelines.worksheet_ts = DateTime.Now;
        quotelines.quote_worksheet_part_requested = newinclude && Convert.ToInt32(newpartnumber) != 0 ? 1 : 0;

        #region old code

        /*if (newinclude != oldinclude && Convert.ToInt32(newpartnumber) != 0 && (Convert.ToInt32(newpartnumber) < 990000 || Convert.ToInt32(newpartnumber) >= 2000000))
		{
			//Make sure this quote has been linked to a work order before
			//making part request.
			string wobvwo = "";
			try
			{
				wobvwo = Toolbox.doSQL_string(conn,@"SELECT wo FROM quote_master  where quote_id=@v0 AND revision=@v1 ", new object[] { main_id,rev });

			}
			catch(Exception ex){}
			if (wobvwo == "" || wobvwo == "0")
			{
				update_error("You Cannot Make a Part Request if this quote is not linked to a work order");
				return;
			}
			string woprogid = "";
			try
			{
				wobvwo = wobvwo.PadLeft(10, '0');
				woprogid = Toolbox.doSQL_string(conn,@"SELECT woprog_id from WOProg  WHERE WOProg_BVWO =@v0 AND business_unit_id=@v1 ", new object[] { wobvwo,hidCompanyID.Value });
			}
			catch (Exception ex) { }
			if (woprogid == "" || woprogid == "0")
			{
				update_error("There  is an issue with the workorder that this quote is linked to and the part request cannot be made at this time.");
				return;

			}
			
			//A part has been included or removed from quote work sheet
			NEPartRequest request = new NEPartRequest();
			request.pr_details_current_companyid = Convert.ToInt32(hidCompanyID.Value);
			request.pr_details_current_origin = 2; //Quote Work Sheet Origin
			request.pr_details_current_originid = Convert.ToInt32(main_id);
			request.pr_details_current_originlineid = newtableid;
			request.pr_details_current_woid = Convert.ToInt32(woprogid);
			request.pr_details_current_memberid = Convert.ToInt32(myMember.id);
			request.pr_details_current_auditmemberid = Convert.ToInt32(myMember.id);
			request.pr_details_current_poid = 0;
			request.pr_details_current_polineid = 0;
			int intpartnumber = 0;
			try
			{
			intpartnumber = Convert.ToInt32(newpartnumber);
			}
			catch{}
		
			request.pr_details_current_alternate_master_id = 0;
			request.pr_details_current_statusid = 1;
		
			request.pr_details_current_notes = "";


			if (newinclude)
			{
				if (intpartnumber < 990000)
				{
					//Not Kitted
					request.pr_details_current_masterid = intpartnumber;
					request.pr_details_current_qty = newqty;
					request.NEPartLineAdd();
				}
				else if (intpartnumber >= 2000000)
				{
					//Kitted
					DataTable PartList = Toolbox.doSQL_dt(conn,@"SELECT inventory_kit_dtl_master_id, inventory_kit_dtl_qty FROM inventory_kit_dtl  WHERE inventory_kit_dtl_master_id < 990000 AND inventory_kit_dtl_hdr_id =@v0", new object[] { newpartnumber });
					foreach (DataRow kitrow in PartList.Rows)
					{
						request.pr_details_current_masterid = Convert.ToInt32(kitrow[0].ToString());
						kitqty = Convert.ToDouble(kitrow[1].ToString());
						request.pr_details_current_qty = kitqty * newqty;
						request.NEPartLineAdd();
					}
				}


			}
			else
			{
				request.RemoveQuotePartRequest(newtableid.ToString(), main_id);
			}
		}*/

        #endregion old code

        quotelines.UpdateWorkSheetLine();
        if (newpartnumber != "" && Convert.ToInt32(newpartnumber) < 990000)
        {
            UpdateSellPriceForMultipleParts(Convert.ToInt32(newpartnumber), Convert.ToDouble(newqty));
        }
        isDifferentSection = newsectionid != oldsectionid && !string.IsNullOrEmpty(_q["section_id"]);
        ;
    }

    protected void UpdateKittedLine()
    {
        var detailsupdate = "UPDATE inventory_kit_dtl SET inventory_kit_dtl_qty = " + newqty +
                            ",inventory_kit_dtl_edited_dt=curdate(),inventory_kit_dtl_member_id=" + current_user.id +
                            " WHERE inventory_kit_dtl_id = " + newtableid;
        Toolbox.doSQL_void(conn, @"UPDATE inventory_kit_dtl  SET inventory_kit_dtl_qty =@v0,inventory_kit_dtl_edited_dt=curdate(),inventory_kit_dtl_member_id=@v1   WHERE inventory_kit_dtl_id =@v2", new object[] { newqty, current_user.id, newtableid });
        FillforKitted(main_id);
    }


    protected void AddNewWorkOrderLine(int orig_woprog_id, int xfer_to_woprog_id, int master_id, bool adding, bool exclude,
                                        bool commit_wo)
    {
        using (var conn = Toolbox.connect())
        {
            var billtype = 0;
            var recnumber = 0;
            var RecordNumTwo = 0;
            var laboourcodetest = 0;
            var quote = "";
            var strwoid = "";
            var CompID = hidCompanyID.Value;
            double oldvalue = 0;
            double currently_on_WO = 0;
            var is_exclude = false;
            var PriceChanged = false;
            var inv = new inventory();
            var recordnumbertosave = 0;
            var sales = new NeSalesOrder();
            var orig_wo = new NeWOProg(main_id);
            var xfer_to_wo = new NeWOProg();
            if (xfer_to_woprog_id > 0 && !Toolbox.Contains(xfer_to_woprog_id, new[] { 9999999, 9999998, 9999997 }))
            {
                xfer_to_wo = new NeWOProg(xfer_to_woprog_id);
            }
            if (int.TryParse(newpartnumber, out master_id) && master_id < 990000 && newpartnumber != "2139")
            // only check for an existing part if the part is not labour
            {
                currently_on_WO =
                    Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(wo_detail_current_qty_committed), 0) FROM wo_detail_current WHERE wo_detail_current_master_id = @v0  AND wo_detail_current_woprog_id = @v1 ", new object[] { master_id, main_id });
            }
            if (orig_wo.Status == OpsWOStatus.Invoiced || orig_wo.Status == OpsWOStatus.WaitingToBeInvoiced)
            {
                throw new Exception(
                    "You can not add or alter an item on this work order because it has been invoiced/is waiting to be invoiced.");
            }
            if (inv.part_exists(master_id))
            {
                if (inv_i != null)
                {
                    inv = inv_i[master_id.ToString()];
                }
                else
                {
                    inv.Load(master_id, CompID);
                }
                var tag = new inventory.tag(inv.tag_id);
                is_exclude = tag.is_exclude;
            }
            var company = new NeBusinessUnit();
            var orig_detail = new NeWODetailCurrent();
            var xfer_to_detail = new NeWODetailCurrent();
            originbvwo = orig_wo.OrderNumber;

            originbvwo = orig_wo.OrderNumber;
            if (!adding)
            {
                orig_detail = new NeWODetailCurrent(newtableid);
                if (orig_detail.origin.Contains("PO")) // if the original line came from a po
                {
                    if (orig_detail.qty_committed > new_qty_committed && !chk_workorder_transfer)
                    {
                        throw new Exception(
                            "You cannot reduce the committed quantity of Parts Transferred From A PO... Use the transfer button instead.");
                    }
                }
            }
            var xfer_to_wodc_id = 0;
            if (xfer_to_woprog_id > 0 && !Toolbox.Contains(xfer_to_woprog_id, new[] { 9999999, 9999998, 9999997 }))
            {
                // Definitely a transfer
                xfer_to_wo = new NeWOProg(xfer_to_woprog_id);
                // We need to check if the part already exists on the TO work order.
                var c_exists_xfer_to =
                    Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { xfer_to_woprog_id, master_id });
                if (!inv.is_exclude && c_exists_xfer_to == 1)
                {
                    xfer_to_wodc_id =
                        Toolbox.doSQL_int(conn, @"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { xfer_to_woprog_id, master_id });
                    xfer_to_detail = new NeWODetailCurrent(xfer_to_wodc_id);
                }
            }
            quote = orig_wo.QuoteID;
            strwoid = id;
            if (adding)
            {
                if (ddlBillType.ClientVisible)
                {
                    billtype = Convert.ToInt32(ddlBillType.Value);
                }
                else
                {
                    billtype = quote == "0" ? 0 : 1;
                }
            }
            else
            {
                billtype = Convert.ToInt32(newbilltype);
            }
            sales.BillingTypeID = billtype;
            try
            {
                laboourcodetest = Convert.ToInt32(newpartnumber);
            }
            catch
            {
            }
            double comm_qty_diff = 0;
            var comm_qty_total = new_qty_committed - orig_detail.qty_committed;
            var partid = newpartnumber;
            if (!is_exclude && master_id < 990000 && !agv.IsEditing)
            {
                var c =
                    Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { main_id, inv.master_id });
                if (c == 1)
                {
                    newtableid =
                        Toolbox.doSQL_int(conn, @"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { main_id, inv.master_id });
                    orig_detail.GetLineDetails(newtableid.ToString());
                    oldqty = orig_detail.qty_ordered;
                }
            }
            if (!adding)
            {
                comm_qty_diff = new_qty_committed - orig_detail.qty_committed;
            }
            else
            {
                comm_qty_diff = 0; // newqty;
                double.TryParse(TextComQty.Text, out comm_qty_diff);
                comm_qty_total = comm_qty_diff;

                if (ddlMatType.SelectedValue == "2") // Labor
                {
                    comm_qty_diff = new_qty_committed - orig_detail.qty_committed;
                    comm_qty_total = comm_qty_diff;
                }
            }
            if (inv.master_id == "2139" && sales.BillingTypeID == 9 && new_qty_committed > orig_detail.qty_committed)
            {
                var q_id = 0;
                var q_rev = 0;
                nesi.core.quote.splice(orig_wo.QuoteID, out q_id, out q_rev);
                var quoted_amount = nesi.core.quote.quoted_amount(conn, q_id);
                var total_billed = NeWOProg.total_billed(conn, main_id);
                if (neworigsell > 0)
                {
                    if ((total_billed >= quoted_amount || quoted_amount - total_billed + new_qty_committed * neworigsell > quoted_amount) &&
                        new_qty_committed > 0)
                    {
                        throw new Exception("Unable to bill more than the quoted amount");
                    }
                }
                else
                {
                    if ((total_billed >= quoted_amount || quoted_amount - total_billed + new_qty_committed * quoted_amount > quoted_amount) &&
                        new_qty_committed > 0)
                    {
                        throw new Exception("Unable to bill more than the quoted amount");
                    }
                }
            }
            var desc = newdescription;
            var quantity = newqty.ToString();


            recordnumbertosave = newrecno;
            if ((sales.BillingTypeID == 2 || sales.BillingTypeID == 10) && new_qty_committed > 0)
            {
                throw new Exception("A credit cannot have a positive quantity.");
            }
            if (oldrecno != newrecno && !adding) // if you're not adding a line.
            {
                var linecount =
                    Toolbox.doSQL_int(conn, @"SELECT COUNT(*) + 2 FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0 ", new object[] { main_id }); // find the next line on the work order
                if (newrecno > 1 && newrecno < linecount)
                {
                    //Update Record Numbers
                    //NeWODetailCurrent details = new NeWODetailCurrent();
                    //details.SwitchRecNo(oldrecno, newrecno, Convert.ToInt32(main_id), newtableid);
                    recordnumbertosave = newrecno;
                }
            }
            sales.forcenewpartline = chkNewPart.Checked;
            if (chkTrackPart.Checked && adding)
            {
                NewTrack = 1;
            }
            else if (!chkTrackPart.Checked && adding)
            {
                NewTrack = 0;
            }

            sales.PartNo = master_id.ToString();
            if (!adding)
            {
                sales.RecNum = recordnumbertosave.ToString();
            }
            var twoweeksfromtoday = Toolbox.MySQL_shortdt(DateTime.Now.AddDays(14));
            //		string used_date_required = progress.woprog_Expected_StartDate > DateTime.Now.AddDays(14) ? Toolbox.MySQL_shortdt(progress.woprog_Expected_StartDate) : twoweeksfromtoday;

            var used_date_required = Convert.ToDateTime(Session["picklist_date"]).ToString("yyyy-MM-dd");
            sales.DateRequired = newdatereq != "" && newdatereq != orig_detail.date_required
                ? newdatereq
                : orig_detail.date_required == "" ? used_date_required : orig_detail.date_required;
            sales.Quantity = comm_qty_diff;
            var dont_force_comm_pop = false;
            sales.Notes = hidNotes.Value;
            sales.ActualQuantity = adding ? comm_qty_total : new_qty_committed;

            #region Costing

            var parts_cost_is_editable = Toolbox.Contains(master_id, new[] { 2139, OpsSpecialPart.SubContractor }) ||
                                        master_id >= 990000 && master_id < 2000000;
            var use_po_cost = false;
            var price = neworigsell.ToString();
            // IF the old received quantity is less than the new received quantity...
            if (orig_detail.qty_committed < new_qty_committed && origin == "purchaseorder")
            {
                // Check if this item was purchased via a po
                double po_cost = 0;
                var po_count =
                    Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM po_details_current 
WHERE po_details_part_no = @v0  
AND po_details_woprog_id = @v1  
AND po_details_qty_received > 0 and po_details_current.is_gl_account = false", new object[] { master_id, strwoid });
                // If there is only one PO
                if (po_count == 1 && commit_wo)
                {
                    var temp_qty = sales.ActualQuantity == 0 ? 1 : sales.ActualQuantity;
                    po_cost =
                        Toolbox.doSQL_double(conn, @"SELECT po_details_cost / po_details_vendor_qty_per 
FROM po_details_current WHERE po_details_part_no = @v0  
AND po_details_woprog_id = @v1 and po_details_current.is_gl_account = false
AND po_details_qty_received > 0", new object[] { master_id, strwoid });
                    sales.CostOverRide =
                        Toolbox.doSQL_double(conn, @"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { strwoid, master_id, comm_qty_total, po_cost });
                    use_po_cost = true;
                }
            }
            var current_cost = !inv.is_exclude && inv.allowed_to_stock
                ? Toolbox.doSQL_double(conn, @"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, 1)", new object[] { partid, WarehouseBusinessUnit.id })
                : 0;
            var chk_uncommit_stocked = inv.cost_price_branch > 0 && orig_detail.qty_committed > new_qty_committed &&
                                        inv.allowed_to_stock;
            var chk_2139_costchange = oldcost != newcost && can_edit_part_cost && master_id == 2139;
            var chk_costchange = oldcost != newcost && can_edit_part_cost;
            var chk_part_is_labor = master_id >= 990000;
            var chk_part_isnt_labor = master_id < 990000;
            var chk_rentals = !inv.allowed_to_stock && !inv.is_exclude && chk_part_isnt_labor;
            var chk_real_material_part = inv.allowed_to_stock && !is_exclude;
            var chk_cost_different_from_inv = oldcost != current_cost;

            var bd = new bingo_data();
            bd.before_cost = orig_detail.cost;
            bd.before_qty = orig_detail.qty_committed;
            bd.before_sell = orig_detail.sell;
            bd.detail_id = orig_detail.id;
            bd.master_id = master_id;
            bd.woprog_id = orig_detail.woprog_id;
            if (chk_workorder_transfer)
            {
                if (chk_part_is_labor)
                {
                    throw new Exception("You can't transfer labor lines between work orders");
                }
                if (chk_part_isnt_labor && (chk_real_material_part || chk_rentals))
                {
                    sales.CostOverRide =
                        Toolbox.doSQL_double(conn, @"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { strwoid, master_id, sales.ActualQuantity, orig_detail.cost });
                }
                else if (inv.is_exclude) // New lines, don't blend, don't refactor sell price
                {
                    sales.CostOverRide = orig_detail.cost;
                }
            }
            // Not a transfer, it a non-labor part #, not from PO, isn't an exclude, is a part we can stock, quantity is changing, old cost doesn't equal new cost
            else if (chk_part_isnt_labor && !use_po_cost && chk_real_material_part && Math.Abs(comm_qty_total) > 0 &&
                    chk_cost_different_from_inv)
            {
                var temp_qty = sales.ActualQuantity == 0 ? 1 : sales.ActualQuantity;
                bd.ca_cost = current_cost;
                bd.ca_qty = sales.ActualQuantity;
                sales.CostOverRide = comm_qty_total < 0
                    ? orig_detail.cost
                    : Toolbox.doSQL_double(conn, @"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { strwoid, master_id, comm_qty_total, current_cost });
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
                sales.CostOverRide =
                    Toolbox.doSQL_double(conn, @"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )", new object[] { master_id, WarehouseBusinessUnit.id, temp_qty });
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

            #endregion Costing

            if (!member_can_see_cost && chk_real_material_part)
            {
                if (sales.CostOverRide > 0)
                {
                    // Set because there is a cost override set
                    neworigsell = shared.GetSellPrice(sales.CostOverRide, 0, inv.is_qty, sales.ActualQuantity, WorkingBusinessUnit.id32);
                    // recalculate sell based on the cost.. because we can't trust whatever is on the front page
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
                else // Use existing line cost
                {
                    neworigsell = shared.GetSellPrice(orig_detail.cost, 0, inv.is_qty, sales.ActualQuantity, WorkingBusinessUnit.id32);
                    // recalculate sell based on the cost.. because we can't trust whatever is on the front page
                    bd.ca_sell = neworigsell;
                    bd.ca_qty = sales.ActualQuantity; // There isn't a CA here.
                }
            }
            if (!member_can_see_cost && master_id >= 990000 || neworigsell == 0 && master_id >= 990000)
            {
                neworigsell = shared.GetLabourSell(master_id);
                bd.ca_sell = neworigsell;
                bd.ca_qty = orig_detail.qty_committed; // There isn't a CA here.
            }
            if (neworigsell != oldorigsell && agv.IsEditing)
            {
                sales.ManualChange = true;
            }
            else if (newqty != oldqty && adding && orig_detail.id > 0 && !is_exclude)
            {
                sales.ManualChange = true;
            }
            //		sales.RetailCost = 
            // Check if it exists already on WO

            var is_manual_price_change = false;
            var exists_chk =
                Toolbox.doSQL_int(conn, @"SELECT CAST(IFNULL(SUM(wo_detail_current_qty_committed),0) AS UNSIGNED) 
FROM wo_detail_current 
WHERE wo_detail_current_master_id = @v0  
AND wo_detail_current_woprog_id = @v1 ", new object[] { partid, strwoid });
            sales.SellOverride = orig_detail.sell;
            if (master_id != 2139)
            {
                if (adding && TextSell.Text != "" && Convert.ToDouble(TextSell.Text) != Convert.ToDouble(price) &&
                    (exists_chk == 0 || is_exclude) && Convert.ToDouble(TextSell.Text) > 0)
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
                else if (chk_rentals)
                {
                    sales.SellOverride = shared.GetSellPrice(sales.CostOverRide, 0, inv.is_qty, sales.ActualQuantity, WorkingBusinessUnit.id32);
                }
                else if (Convert.ToDouble(price) > 0 && !adding && !chk_workorder_transfer && agv.IsEditing)
                {
                    sales.SellOverride = Convert.ToDouble(price);
                    sales.ManualChange = true;
                    is_manual_price_change = true;
                }
                else if (adding && exists_chk > 0 && !is_exclude && master_id >= 990000)
                {
                    sales.SellOverride = neworigsell;
                }
                else if (!adding && exists_chk > 0 && !is_exclude && master_id >= 990000 && sales.SellOverride == 0)
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
            if (sales.SellOverride == 0 && master_id >= 990000)
            {
                throw new Exception(
                    "You cannot set a sell price for a labor line as zero. You must use the appropriate billtype instead.");
            }
            if (sales.SellOverride != 0 && sales.SellOverride < sales.CostOverRide && can_edit_part_cost &&
                !parts_cost_is_editable)
            {
                throw new Exception("Cannot change the sell price to be less than the cost price.");
            }
            // if (adding && myMember.business_unit.is_backoffice)
            //     sales.ActualQuantity = newqty;
            bd.after_qty = sales.ActualQuantity;
            bd.after_cost = sales.CostOverRide;
            bd.after_sell = sales.SellOverride;
            bd.save();
            sales.partlineid = newtableid.ToString();
            sales.working_line_id = newtableid;
            sales.Desc = desc;
            sales.TrackPart = NewTrack;
            try
            {
                sales.CustomerDiscount = newdiscount;
            }
            catch
            {
            }
            sales.OrderedQuantity = chk_workorder_transfer && transferbvwo != "STOCKTRANSFER" || sales.Quantity < 0
                ? comm_qty_diff
                : newqty;
            if (!new List<int>(new[] { 0, 1, 2, 10 }).Contains(sales.BillingTypeID))
            {
                sales.CustomerDiscount = 0;
                if (!adding)
                {
                    var discup =
                        string.Format(
                            @"UPDATE wo_detail_current SET wo_detail_current_discount = 0 WHERE wo_detail_current_id = {0} LIMIT 1",
                            newtableid);
                    try
                    {
                        Toolbox.doSQL_void(conn, @"UPDATE wo_detail_current SET wo_detail_current_discount = 0 WHERE wo_detail_current_id = @v0  LIMIT 1", new object[] { newtableid });
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                if (!adding && oldbilltype != newbilltype)
                {
                    var discountamt = "0";
                    if (chkApplyDiscount.Checked)
                    {
                        var woline =
                            Toolbox.doSQL_dt(conn, @"SELECT WOProg_Customer_ID c_id, woprog_address_id a_id FROM woprog WHERE WOProg_ID = @v0 ", new object[] { main_id }).Rows[0];
                        var c_id = Convert.ToInt32(woline["c_id"]);
                        var a_id = Convert.ToInt32(woline["a_id"]);
                        if (a_id == 0)
                        {
                            a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                        }

                        DiscountAmount = 0;
                        discountamt = DiscountAmount.ToString();
                        var discup =
                            string.Format(@"UPDATE wo_detail_current SET wo_detail_current_discount = {0} WHERE wo_detail_current_id = {1}",
                                discountamt, newtableid);
                        try
                        {
                            Toolbox.doSQL_void(conn, @"UPDATE wo_detail_current SET wo_detail_current_discount = @v0  WHERE wo_detail_current_id = @v1 ", new object[] { discountamt, newtableid });
                        }
                        catch (Exception ee)
                        {

                        }
                    }
                }
            }


            if (adding)
            {
                sales.SalesTax1 = orig_wo.woprog_tax1;
                sales.SalesTax2 = orig_wo.woprog_tax2;
                sales.SalesTax3 = orig_wo.woprog_tax3;
                sales.SalesTax4 = orig_wo.woprog_tax4;
            }
            var checkbilltype = "";
            if (adding)
            {
                try
                {
                    checkbilltype = ddlBillType.Value.ToString();
                }
                catch (Exception ee) // whenever work is open
                {
                    if (master_id != 2139)
                    {
                        checkbilltype = orig_wo.QuoteID != "0" ? "1" : "0";
                    }
                    else
                    {
                        checkbilltype = "3";
                    }

                }
                newbilltype = checkbilltype;
            }
            else
            {
                checkbilltype = newbilltype;
            }

            #region Check Quote Information

            //Check For Quote Information
            //How Many Job Cost for Quotes on This Work Order
            var JobstcostCount = 0;
            try
            {
                JobstcostCount =
                    Toolbox.doSQL_int(conn, @"SELECT COUNT(wo_detail_current_id) FROM wo_detail_current WHERE wo_detail_current_billtypeid = 1 AND wo_detail_current_woprog_id = @v0  and wo_detail_current_consignment_id != 0", new object[] { main_id });
            }
            catch
            {
            }
            var QuoteLineCount = 0;
            try
            {
                QuoteLineCount =
                    Toolbox.doSQL_int(conn, @"SELECT COUNT(wo_detail_current_id) FROM wo_detail_current WHERE wo_detail_current_master_id = 2139 AND wo_detail_current_woprog_id = @v0 ", new object[] { main_id });
            }
            catch
            {
            }
            if (inv.allowed_to_stock)
            {
                var il_m = new location_master();
                if (xfer_qty_internal != 0 || xfer_qty_external != 0) // Direct xfer
                {
                    if (xfer_qty_external > 0)
                    {
                        //xfer_external_select_id
                        il_m = new location_master(xfer_external_select_id);
                        sales.uncommitting_to_location = il_m.name;
                    }
                    if (xfer_qty_internal > 0)
                    {
                        //xfer_internal_select_id
                        il_m = new location_master(xfer_internal_select_id);
                        sales.uncommitting_to_location = string.IsNullOrEmpty(sales.uncommitting_to_location) ? il_m.name : "," + il_m.name;
                    }
                }
                else if (!chk_workorder_transfer && (new_qty_committed != 0 || comm_qty_diff != 0) &&
                        new_qty_committed != orig_detail.qty_committed) // Committing & Uncommitting
                {
                    //xfer_comm_recv_select_id
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
            }
            if (newbilltype == "1" && QuoteLineCount == 0)
            {
                throw new Exception("You can not set an item for job cost for quote if there is not a quote line on a work order.");
            }
            if (newpartnumber == "2139" && newbilltype != "3" && JobstcostCount != 0)
            {
                throw new Exception(
                    "You can not change a quote line`s bill type if there are job costs for quote bill types on the work order.");
            }

            #endregion Check Quote Information

            if (checkbilltype == "4")
            {
                if (current_user.AuthenticatedForPrivilege(Convert.ToInt32(_strPrivBlenInv)) == false)
                {
                    throw new Exception("You Are Not Allowed to Set an Item to Blended or Inventory");
                }
            }
            var credit = checkbilltype == "2" || checkbilltype == "10" || checkbilltype == "5";
            if ((is_exclude || exclude) && sales.CostOverRide == 0)
            {
                sales.CostOverRide = newcost;
            }
            if (sales.SellOverride == 0 && inv.master_id != "777")
            {
            }

            //Check To See if this Will Create A negative Value on the Work Order
            var chkSql = "Select 0";
            object[] paramObjects = null;
            if (!adding)
            {
                chkSql =
                    "SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current WHERE wo_detail_current_id = @v0";
                paramObjects = new object[] { newtableid };
            }
            else if (adding && !exclude && !chkNewPart.Checked)
            {
                chkSql = "SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current WHERE wo_detail_current_master_id = @v0 AND wo_detail_current_woprog_id = @v1";
                paramObjects = new object[] { partid, main_id };
            }
            else if (exclude || sales.forcenewpartline)
            {
                if (newqty < 0 && !credit)
                {
                    throw new Exception("You cannot add a negative qty as a new line.");
                }
            }

            try
            {
                oldvalue = Toolbox.doSQL_double(conn, chkSql, paramObjects);
            }
            catch
            {
            }

            //
            // Key: 'part != 9595' excludes all 9595 parts, no matter which ones: subcontractor, expense....
            //
            if (oldvalue + comm_qty_diff < 0 && !credit && partid != "0" && partid != "2139" &&
                partid != OpsSpecialPart.SubContractor.ToString() &&
                partid != OpsSpecialPart.CompanyCreditCardExpense.ToString() &&
                partid != OpsSpecialPart.ExpenseReimbursement.ToString() &&
                partid != OpsSpecialPart.PerDiem.ToString() &&
                partid != OpsSpecialPart.NewChildWO.ToString())
            {
                throw new Exception("You Cannot Make this Change, it will result in a negative committed qty on this workorder");
            }
            //sales.ActualQuantity = actualquantity;
            //sales.Desc = Toolbox.do_value_to(desc);
            if (master_id >= 990000 && !adding)
            {
                sales.forcenewpartline = true;
            }
            sales.OrderedQuantity = adding && sales.OrderedQuantity == 0 && sales.Quantity > 0
                ? sales.Quantity
                : sales.OrderedQuantity;
            if (chk_workorder_transfer && transferbvwo != "STOCKTRANSFER")
            {
                sales.transfer_to_line_id = Convert.ToInt32(transferbvwo);
            }
            var linetype = master_id < 990000 ? "M" : master_id == 2139 ? "Q" : "L";

            if (linetype == "L" && !is_adding)
            {
                sales.trackmemberid = orig_detail.memberid;
                sales.trackpaytypeid = orig_detail.paytypeid;
            }
            recnumber = sales.SavePart(orig_wo.woprog_id, "FALSE", WorkingBusinessUnit.DSN, current_user.FullName, current_user.id, adding, "");
            sales.OrderedQuantity = newqty;
            double checkcommit = 0;
            if (TextComQty != null)
            {
                double.TryParse(TextComQty.Text, out checkcommit);
            }

            var transfer_to_wo = new NeWOProg();
            if (chk_workorder_transfer && transferbvwo != "STOCKTRANSFER")
            {
                transfer_to_wo = new NeWOProg(Convert.ToInt32(hidCompanyID.Value), transferbvwo);
            }
            if (checkcommit != 0 && adding && !dont_force_comm_pop)
            {
                new_qty_committed = checkcommit;
            }
            if (!chk_workorder_transfer && (new_qty_committed != 0 || comm_qty_diff != 0) &&
                new_qty_committed != orig_detail.qty_committed)
            {
                #region Is not a Work Order Transfer, Just pulling from stock

                if (master_id < 990000 && !is_exclude && inv.allowed_to_stock && master_id != OpsSpecialPart.MiscMaterial)
                {
                    var from_stock = new Nestock_transfer();
                    from_stock.type_id = 1; // This is to designate that it is going TO a work order, from generic stock
                    from_stock.master_id = master_id;
                    from_stock.from_location_id = xfer_comm_recv_select_id != 0 ? xfer_comm_recv_select_id : 0;
                    // This should never be zero... Cover it in the class, if it's zero, it will pull from the largest qty internal location.
                    from_stock.from_id = xfer_comm_recv_select_id != 0 ? xfer_comm_recv_select_id : 0;
                    // This should never be zero... Cover it in the class, if it's zero, it will pull from the largest qty internal location.
                    from_stock.to_id = main_id;
                    from_stock.quantity = orig_detail.qty_committed - new_qty_committed * newqtyperpart;
                    from_stock.description = newdescription;
                    from_stock.member_id = current_user.id;
                    from_stock.note = "Quantity committed to work order";
                    from_stock.business_unit_id = Convert.ToInt32(hidCompanyID.Value);
                    from_stock.cost = orig_detail.cost != 0 && new_qty_committed < orig_detail.qty_committed && newtableid != 0
                        ? orig_detail.cost
                        : Toolbox.doSQL_double(conn, @"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] { partid, hidCompanyID.Value });
                    if (xfer_comm_recv_select_id != 0)
                    {
                        try
                        {
                            // Get Location Name
                            var location_name =
                                Toolbox.doSQL_string(conn, @"SELECT name FROM inventory_location_master WHERE id = @v0 ", new object[] { xfer_comm_recv_select_id });
                            orig_detail.notes = "Committed from Location: " + location_name;
                        }
                        catch (Exception ee)
                        {

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

                PriceChanged = false;
                if (chk_workorder_transfer && transferbvwo != "STOCKTRANSFER")
                {
                    #region Is a work order to work order transfer

                    sales.Quantity = xfer_qty_wo;
                    sales.ActualQuantity = xfer_qty_wo;
                    sales.OriginWorkOrder = orig_wo.OrderNumber;
                    sales.OrderedQuantity = xfer_qty_wo;
                    sales.line_origin = "Transferred From: " + orig_wo.OrderNumber;
                    var c_exists_target_wo =
                        Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transfer_to_wo.woprog_id, inv.master_id });
                    if (is_exclude || c_exists_target_wo == 0)
                    {
                        // New Line on to WO
                        sales.transfer_to_line_id = 0;
                    }
                    else if (c_exists_target_wo > 1)
                    {
                        throw new Exception(
                            "There are multiple lines on the transfer to work order, and this part is not an inventory exclude part... this should never happen.");
                    }
                    else
                    {
                        sales.transfer_to_line_id =
                            Toolbox.doSQL_int(conn, @"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transfer_to_wo.woprog_id, inv.master_id });
                    }
                    if (!is_exclude)
                    {
                        var c =
                            Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM wo_detail_current WHERE wo_detail_current_bvwo = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transferbvwo, inv.master_id });
                        if (c == 1) // Should only be a max of one on the destination work order
                        {
                            // We need the TO WO line
                            var wodc_to = new NeWODetailCurrent();
                            var to_line_id =
                                Toolbox.doSQL_string(conn, @"SELECT wo_detail_current_id FROM wo_detail_current WHERE wo_detail_current_bvwo = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { transferbvwo, inv.master_id });
                            wodc_to.GetLineDetails(to_line_id);
                            bd = new bingo_data();
                            bd.before_cost = wodc_to.cost;
                            bd.before_qty = wodc_to.qty_committed;
                            bd.before_sell = wodc_to.sell;
                            bd.master_id = wodc_to.master_id;
                            bd.ca_cost = sales.CostOverRide;
                            bd.ca_qty = sales.ActualQuantity;
                            bd.ca_sell = wodc_to.sell;
                            sales.CostOverRide =
                                Toolbox.doSQL_double(conn, @"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { wodc_to.woprog_id, master_id, sales.ActualQuantity, sales.CostOverRide });
                            sales.SellOverride = shared.GetSellPrice(sales.CostOverRide, 0, inv.is_qty,
                                wodc_to.qty_committed + sales.ActualQuantity, WorkingBusinessUnit.id32);
                            bd.after_cost = sales.CostOverRide;
                            bd.after_qty = wodc_to.qty_committed + sales.ActualQuantity;
                            bd.after_sell = sales.SellOverride;
                            bd.detail_id = wodc_to.id;
                            bd.woprog_id = wodc_to.woprog_id;
                            bd.save();
                        }
                    }
                    // Saving the xfer to work order

                    RecordNumTwo = sales.SavePart(transfer_to_wo.woprog_id, "TRUE", WorkingBusinessUnit.DSN, current_user.FullName, current_user.id, adding, "");

                    #endregion Is a work order to work order transfer
                }

                #endregion Handle WO transfers
            }
            if (xfer_qty_internal != 0)
            {
                #region Handle Internal transfers - From WO to Specified Internal Location

                if (master_id < 990000 && inv.allowed_to_stock)
                {
                    var st_internal = new Nestock_transfer();
                    st_internal.from_id = main_id;
                    st_internal.to_id = xfer_internal_select_id;
                    st_internal.type_id = 3; // 3 = Internal Location
                    st_internal.master_id = master_id;
                    st_internal.to_location_id = xfer_internal_select_id;
                    st_internal.quantity = xfer_qty_internal;
                    st_internal.description = newdescription;
                    st_internal.member_id = current_user.id;
                    st_internal.note = transfernote;
                    st_internal.business_unit_id = Convert.ToInt32(hidCompanyID.Value);
                    st_internal.cost = oldcost;
                    st_internal.Nestock_transfer_save();
                }

                #endregion Handle Internal transfers
            }
            if (xfer_qty_external != 0)
            {
                #region Handle External transfers - From WO to Specified External Location

                if (master_id < 990000 && inv.allowed_to_stock)
                {
                    var st_external = new Nestock_transfer();
                    st_external.from_id = main_id;
                    st_external.to_id = xfer_external_select_id;
                    st_external.to_location_id = xfer_external_select_id;
                    st_external.type_id = 4; // 4 = External Location
                    st_external.master_id = master_id;
                    st_external.quantity = xfer_qty_external;
                    st_external.description = newdescription;
                    st_external.member_id = current_user.id;
                    st_external.note = transfernote;
                    st_external.business_unit_id = Convert.ToInt32(hidCompanyID.Value);
                    st_external.cost = oldcost;
                    st_external.Nestock_transfer_save();
                }

                #endregion Handle External transfers
            }
            notesforwoaudit = orig_detail.notes;
            //AuditSave(orig_wo.OrderNumber, Convert.ToString(recnumber), CompID, strwoid, adding, is_manual_price_change, commit_wo);
            if (chk_workorder_transfer && transferbvwo != "STOCKTRANSFER")
            {
                AuditSave(transferbvwo, Convert.ToString(RecordNumTwo), CompID, transferwoprogid, adding, sales.ManualChange,
                    commit_wo);
            }
            if (currently_on_WO > 0 && chkNewPart.Checked)
            {
            }
            NeWOProg.update_header_totals(strwoid, Convert.ToInt32(CompID), orig_wo.OrderNumber);
            if (chk_workorder_transfer && transferbvwo != "STOCKTRANSFER")
            {
                NeWOProg.update_header_totals(transferwoprogid, Convert.ToInt32(CompID), transferbvwo);
            }
            TextCost.ClientEnabled = false;
        }
    }

    protected int LabourCodeAdding(string Partid)
    {
        var recno = 0;
        var memberid = ddlMemberName.Value.ToString();
        var paytypeid = ddlLabourChargeType.Value.ToString();
        var MasterID = 0;
        var LastLabour = 0;
        var LastQuote = 0;
        var intFirstPart = 0;
        var strLabor = "Labour";
        var detail = new NeWODetailCurrent();
        var WorkOrderDetails = new DataTable();
        var MySQLworkorder = new NeWOProg(main_id);
        MasterID = Convert.ToInt32(Partid);
        var addmember = new NeMember(Convert.ToInt32(memberid));
        strLabor = NeBusinessUnit.GetbuCountry(addmember.business_unit.ToString()) == "CDN" ? "Labour" : "Labor";
        var Paytype = Toolbox.doSQL_string(conn, @"SELECT Description FROM paytypehours  WHERE PayTypeHours_ID =@v0", new object[] { paytypeid });
        var descript = string.Format("{0} Hours {1}: {2}", addmember.FullName, strLabor, Paytype);
        var ChargeOut = "0";
        double dblTrueLabCost = 0;
        ChargeOut = Partid == "1000000" ? "0" : neworigsell.ToString();

        var wo_line_id = NeMemberTime.wo_line_id(memberid, paytypeid, MySQLworkorder.woprog_id);
        if (wo_line_id > 0)
        {
            detail = new NeWODetailCurrent(wo_line_id);
        }
        var strDetailsSelect =
            string.Format(
                "SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = {0} ORDER BY wo_detail_current_rec_no",
                MySQLworkorder.woprog_id);
        WorkOrderDetails = Toolbox.doSQL_dt(conn, @"SELECT * FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  ORDER BY wo_detail_current_rec_no", new object[] { MySQLworkorder.woprog_id });

        var previousqty = detail.qty_committed;
        detail.added_by = current_user.id;
        detail.date_added = Toolbox.MySQLNow_long();
        detail.date_modified = Toolbox.MySQLNow_long();
        detail.description = descript;
        detail.master_id = MasterID;
        dblTrueLabCost = detail.FindTrueLabourCost(Convert.ToInt32(memberid), Convert.ToInt32(paytypeid), MySQLworkorder.business_unit_id);
        detail.cost = dblTrueLabCost == 0 ? 0 : dblTrueLabCost;
        detail.sell = Convert.ToDouble(ChargeOut);
        detail.unit = detail.sell;
        detail.qty_committed = newqty + previousqty;
        detail.qty_invoiced = newqty + previousqty;
        if (hidCompanyID.Value != "3") // looks like if the branch is NOT charlotte, bill tax on labour.
        {
            detail.tax1 = MySQLworkorder.woprog_tax1;
            detail.tax2 = MySQLworkorder.woprog_tax2;
            detail.tax3 = MySQLworkorder.woprog_tax3;
            detail.tax4 = MySQLworkorder.woprog_tax4;
        }
        else
        {
            detail.tax1 = 0;
            detail.tax2 = 0;
            detail.tax3 = 0;
            detail.tax4 = 0;
        }
        detail.woprog_id = MySQLworkorder.woprog_id;
        detail.bvwo = Convert.ToInt32(MySQLworkorder.OrderNumber);
        detail.business_unit_id = MySQLworkorder.business_unit_id;
        detail.type = "L";
        detail.code = Partid;
        detail.origin = "Manually Added";
        detail.issues = "";
        recno = detail.rec_no;
        if (MySQLworkorder.QuoteID == "0")
        {
            detail.billtypeid = 0;
        }
        else
        {
            detail.billtypeid = 1;
            detail.tax1 = 0;
            detail.tax2 = 0;
            detail.tax3 = 0;
            detail.tax4 = 0;
        }
        detail.memberid = Convert.ToInt32(memberid);
        detail.paytypeid = Convert.ToInt32(paytypeid);

        if (wo_line_id == 0)
        {
            detail.rec_no = WorkOrderDetails.Rows.Count + 2;

            foreach (DataRow row in WorkOrderDetails.Rows)
            {
                if (row["wo_detail_current_code"].ToString().Contains("QUOTE") ||
                    row["wo_detail_current_master_id"].ToString() == "2139")
                {
                    LastQuote = Convert.ToInt32(row["wo_detail_current_rec_no"].ToString());
                }
                else if (row["wo_detail_current_code"].ToString().StartsWith("LB") || row["wo_detail_current_type"].ToString() == "L")
                {
                    LastLabour = Convert.ToInt32(row["wo_detail_current_rec_no"].ToString());
                }
                else if (intFirstPart == 0)
                {
                    intFirstPart = Convert.ToInt32(row["wo_detail_current_rec_no"].ToString());
                }
            }
        }
        detail.save(current_user, "picklist - LabourCodeAdding", true);
        WorkOrderDetails.Dispose();
        return recno;
    }

    protected int LabourCodeEditing(string part)
    {
        var progress = new NeWOProg(main_id);
        var detail = new NeWODetailCurrent(newtableid);
        var actualquantity = new_qty_committed - old_qty_committed;
        detail.code = newpartnumber;
        detail.master_id = Convert.ToInt32(newpartnumber);
        detail.description = newdescription;
        detail.qty_committed = actualquantity;
        detail.qty_invoiced = actualquantity;
        detail.sell = neworigsell;
        detail.discount = newdiscount;
        detail.unit = detail.sell;
        detail.tax1 = progress.woprog_tax1;
        detail.tax2 = progress.woprog_tax2;
        detail.tax3 = progress.woprog_tax3;
        detail.tax4 = progress.woprog_tax4;
        detail.issues = "";
        detail.billtypeid = Convert.ToInt32(newbilltype);
        detail.cost = chk_workorder_transfer ? oldcost : newcost;
        detail.save(current_user, "picklist - LabourCodeEditing", true);
        CheckLabourAgainstTimeSheet(newtableid.ToString(), progress.OrderNumber, newqty);
        return detail.rec_no;
    }

    protected void CheckLabourAgainstTimeSheet(string partlineid, string WONum, double quantity)
    {
        try
        {
            var getmembercost = "SELECT wo_detail_current_price_cost FROM wo_detail_current WHERE wo_detail_current_id=" +
                                partlineid;
            double timecost = 0;
            try
            {
                timecost = Toolbox.doSQL_double(conn, @"SELECT wo_detail_current_price_cost FROM wo_detail_current  WHERE wo_detail_current_id=@v0", new object[] { partlineid });
            }
            catch
            {
            }
            var getmemberprice = "SELECT wo_detail_current_price_sell FROM wo_detail_current WHERE wo_detail_current_id=" +
                                partlineid;
            double timeprice = 0;
            try
            {
                timeprice = Toolbox.doSQL_double(conn, @"SELECT wo_detail_current_price_sell FROM wo_detail_current  WHERE wo_detail_current_id=@v0", new object[] { partlineid });
            }
            catch
            {
            }
            double chargingprice = 0;
            var customer_id = 0;
            switch (origin)
            {
                case "quote":
                    var qu = new quote(main_id);
                    customer_id = qu.cust_id;
                    break;
                case "workorder":
                    var wo = new NeWOProg(main_id);
                    customer_id = wo.WOProg_Customer_ID;
                    break;
            }
            try
            {
                chargingprice = Toolbox.doSQL_double(conn, @"CALL CUSTOMER_CHARGEOUT(@v0 , @v1 )", new object[] { customer_id, newpartnumber });
            }
            catch
            {
            }

            var getMemberID = "SELECT memberid FROM wo_detail_current WHERE wo_detail_current_id=" + partlineid;
            var _id = Toolbox.doSQL_string(conn, @"SELECT memberid FROM wo_detail_current  WHERE wo_detail_current_id=@v0", new object[] { partlineid });
            var getpaytypeid = "SELECT paytypeid FROM wo_detail_current WHERE wo_detail_current_id=" + partlineid;
            var paytypeid = Toolbox.doSQL_string(conn, @"SELECT paytypeid FROM wo_detail_current  WHERE wo_detail_current_id=@v0", new object[] { partlineid });
            var strHoursSum =
                "SELECT IFNULL(SUM(NumberofHours),0) FROM membertime WHERE MemberTime_Mileage = 'false' AND MemberTime_WorkOrder_ID = '" + WONum + "' AND membertime_memberid = "
                + _id + " AND MemberTime_PayTypeHours_ID = " + paytypeid;

            var totalhours = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(NumberofHours),0) FROM membertime  WHERE MemberTime_Mileage = 'false' AND MemberTime_WorkOrder_ID =@v0 AND membertime_memberid =@v1  AND MemberTime_PayTypeHours_ID =@v2 ", new object[] { WONum, _id, paytypeid });
        }
        catch
        {
        }
    }

    protected string GetLabourDescription()
    {
        var descript = "";
        try
        {
            var business_unit_id = "";
            switch (_q["origin"])
            {
                case "workorder":
                    var wo = new NeWOProg(main_id);
                    business_unit_id = wo.business_unit_id.ToString();
                    break;
                case "quote":
                    business_unit_id =
                        Toolbox.doSQL_string(conn, @"SELECT business_unit_id FROM quote_master  WHERE quote_id =@v0 AND revision =@v1 ", new object[] { id, rev });
                    break;
                default:
                    business_unit_id = "1";
                    break;
            }
            var memberid = ddlMemberName.Value.ToString();
            var paytypeid = ddlLabourChargeType.Value.ToString();
            var strLabor = NeBusinessUnit.GetbuCountry(business_unit_id) == "CDN" ? "Labour" : "Labor";
            var addmember = new NeMember(Convert.ToInt32(memberid));
            var Paytype = Toolbox.doSQL_string(conn, @"SELECT description FROM paytypehours  WHERE paytypehours_id =@v0", new object[] { paytypeid });
            descript = addmember.FullName + " Hours " + strLabor + ": " + Paytype;
        }
        catch
        {
        }
        return descript;
    }

    private void AuditSave(string WoNum, string recnum, string CompID, string woid, bool addnew, bool ManualPriceChange)
    {
        AuditSave(WoNum, recnum, CompID, woid, addnew, ManualPriceChange, false);
    }

    private void AuditSave(string WoNum, string recnum, string CompID, string woid, bool addnew, bool ManualPriceChange,
                            bool commit_wo)
    {
        var changes = new NeWOProgChanges();
        changes.WoProgChanges_BVWO = WoNum;
        changes.WoProgChanges_BVWORec = recnum;
        changes.WoProgChanges_WOProg_ID = Convert.ToInt32(woid);
        changes.WoProgChanges_Modified_Member_ID = current_user.id;
        changes.WOProgChanges_ManualPriceChange = ManualPriceChange && transferbvwo != WoNum ? 1 : 0;

        var _checksell = shared.GetSellPrice(newcost, 0, true, new_qty_committed, WorkingBusinessUnit.id32);

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
            catch
            {
            }
        }
        changes.FullWOComment = notesforwoaudit;
        changes.WOProgChanges_OrderedQty = (oldqty + newqty).ToString();
        changes.was_req_qty = oldqty;
        var _isqty = Convert.ToDouble(changes.WoProgChanges_IsQty);
        var _wasqty = Convert.ToDouble(changes.WoProgChanges_WasQty);
        var _diff = _wasqty - _isqty;

        try
        {
            xfer_combined_qty = Convert.ToDouble(changes.WoProgChanges_IsQty) - Convert.ToDouble(changes.WoProgChanges_WasQty);
            xfer_combined_qty = Math.Abs(xfer_combined_qty);
        }
        catch
        {
        }
        if (transferbvwo == WoNum) // if it's actually a transfer
        {
            changes.FullWOComment += " - Transferred " + xfer_qty_wo + " from work order: " + originbvwo + " " + transfernote;
            changes.WoProgChanges_IsQty = xfer_qty_wo.ToString();
        }
        else
        {
            if (transferbvwo != "0")
            {
                if (transferbvwo == "STOCKTRANSFER")
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
                    changes.FullWOComment += " - Transferred " + xfer_combined_qty + " to work order: " + transferbvwo + " " +
                                            transfernote;
                }
            }
        }

        if (commit_wo)
        {
            var po_number = Toolbox.doSQL_int(conn, @"SELECT poprog_bvpo FROM poprog_header  WHERE poprog_id =@v0", new object[] { _q["id"] });
            changes.FullWOComment +=
                string.Format("Committed directly to WO upon receipt of PO#: {0} - Changing the commit qty from {1} to {2}",
                    po_number, changes.WoProgChanges_WasQty, changes.WoProgChanges_IsQty);
            changes.WoProgChanges_WasPartNo = changes.WoProgChanges_IsPartNo;
        }
        changes.business_unit_id = Convert.ToInt32(CompID);
        try
        {
            changes.AddtoWOProgChanges();
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.ToString());
        }
    }

    protected void AddPOLine(int poid)
    {
        var AddInvPart = new inventory();
        var vendor = new NEVendor();
        var this_company = new NeBusinessUnit(WarehouseBusinessUnit.id);
        var poprog = new NePOProg(main_id);

        //string checkforvendorpart = "SELECT vendor_code FROM inventory_price WHERE business_unit_id = '"+poprog.business_unit_id+"' AND master_id = " + newpartnumber + " AND vendor_id='" + vendor.GetVendorBVNO(poprog.poprog_vendor_id.ToString()) + "'";
        //string ProperVendorCode = "";
        //try
        //	{
        //	ProperVendorCode = Toolbox.doSQL_string(conn,@checkforvendorpart  , null);
        //	}
        //catch { }

        if (TextPartNo.Text == OpsSpecialPart.MiscMaterial.ToString())
        {
            update_error("You can not add a Pure Revenue Line for a Purchase Order.");
            return;
        }
        if (TextVendorPartNo.Text == "" && newpartnumber != "")
        {
            update_error("You Must Enter a Vendor Part Number for this part before you can add it.");
            return;
        }
        if (newqtyperpart == 0 && newpartnumber != "")
        {
            update_error("You Must Have a Quantity Per Vendor Part");
            return;
        }
        if (newcost == 0 && newpartnumber != "")
        {
            update_error("There must be a cost associated with this part.");
            return;
        }
        //newworkorderid = ASPxComboBox2.Value.ToString();
        if (newworkorderid == "0")
        {
            update_error("There must be a work order associated with this part.");
            return;
        }

        if (poprog.poprog_woprog_id == 0 && newworkorderid != "9999999" && newworkorderid != "9999998" &&
            newworkorderid != "9999997" && !new_is_gl_account)
        {
            var woprog = new NeWOProg(Convert.ToInt32(newworkorderid));
            var sql = "UPDATE poprog_header SET poprog_woprog_id = " + woprog.woprog_id + ", poprog_customer_id = " + woprog.WOProg_Customer_ID + "  WHERE poprog_id = " + main_id;

            Toolbox.doSQL_void(conn, @"UPDATE poprog_header  SET poprog_woprog_id =@v0, poprog_customer_id =@v1   WHERE poprog_id =@v2", new object[] { woprog.woprog_id, woprog.WOProg_Customer_ID, main_id });

            //		sql = "UPDATE PURCHASE_ORDER_HEADR SET VEND_PO = '" + woprog.OrderNumber + "' WHERE NUMBER = '" + poprog.poprog_bvpo + "'";
            //		try
            //			{
            //			Toolbox.doSQL_void(conn,@sql  , null);
            //			}
            //		catch (Exception ex)
            //			{
            //			string exceptionthrown = ex.ToString();
            //			string hold = "pause";
            //			}
        }

        newtax1 = this_company.Tax1 > 0 ? 1 : 0;
        newtax2 = this_company.Tax2 > 0 ? 1 : 0;
        newtax3 = this_company.Tax3 > 0 ? 1 : 0;
        newtax4 = this_company.Tax4 > 0 ? 1 : 0;
        //newnotes = TextNotes.Text.Replace("'", "");
        try
        {
            AddNewPOLine();
        }
        catch (Exception ee)
        {
            lblerrorLabel.Visible = true;
            lblerrorLabel.Text = ee.ToString();
            throw new Exception(ee.ToString());
        }
    }

    protected void UpdatePOLine()
    {
        var podetails = new NEPO_Details_Current();
        var PoHeader = new NePOProg(main_id);
        var company = new NeBusinessUnit(WarehouseBusinessUnit.id);
        var details = new NEPO_Details_Current();
        details.po_details_current_line(newtableid);

        var newinv = new inventory();
        if (!newinv.part_exists(newpartnumber) && newpartnumber != "0")
        {
            newinv.Load(newpartnumber, company.id);
            //   PurchaseOrder.UnlockPO(company.DSN, poprogress.poprog_bvpo, myMember.Initials);
            throw new Exception("That Part Number Does Not Exist In the System");
        }
        if (agv.EditingRowVisibleIndex > -1)
        {
            var editing_id = Convert.ToInt32(agv.GetRowValues(agv.EditingRowVisibleIndex, "id"));
            if (!allowed_ids.Contains(editing_id))
            {
                allowed_ids.Add(editing_id);
            }
        }
        //newinv.Load(newpartnumber, poprogress.business_unit_id.ToString());
        //Check WOrk Order

        if (newworkorderid != "9999999" && newworkorderid != "9999998" && newworkorderid != "9999997" && !details.is_gl_account && !new_is_gl_account &&
            new_qty_committed != old_qty_committed)
        {
            var statusofwo = Toolbox.doSQL_string(conn, @"SELECT WOProg_Status FROM woprog  WHERE woprog_id =@v0", new object[] { newworkorderid });
            //		var woholdselect = "SELECT WOProg_Hold FROM woprog WHERE woprog_id = " + newworkorderid;
            var hold = Toolbox.doSQL_int(conn, @"SELECT WOProg_Hold FROM woprog  WHERE woprog_id =@v0", new object[] { newworkorderid });
            var labor_only = Toolbox.doSQL_bool(@"SELECT labor_only FROM woprog  WHERE woprog_id =@v0", new object[] { newworkorderid });
            if (new List<string>(new[]
                                    {
                                       OpsWOStatus.Invoiced,
                                       OpsWOStatus.WaitingToBeInvoiced,
                                       OpsWOStatus.WaitingParentBMApproval,
                                       OpsWOStatus.WaitingForPO,
                                       OpsWOStatus.WaitingBMApproval,
                                       OpsWOStatus.WaitingPMApproval,
                                       OpsWOStatus.Deleted
                                    }).Contains(statusofwo) || hold == 1)
            {
                var msg = hold == 1
                    ? "You Cannot Receive parts for a Work Order that is on hold"
                    : "You Cannot Receive parts for a Work Order of Status: " + statusofwo;
                update_error(msg);
                lblerrorLabel.Text = msg;
                throw new Exception(msg);
            }
            if (labor_only == true)
            {
                var msg = "You Cannot Receive parts for a Work Order that is set to Labor Only";

                update_error(msg);
                lblerrorLabel.Text = msg;
                throw new Exception(msg);
            }
        }
        if (newcost != oldcost && Toolbox.Contains(PoHeader.poprog_status, new[]
                {
                OpsPOStatus.ApprovedtoOrder,
                OpsPOStatus.IssuedWaitingforPackingSlip,
                OpsPOStatus.ReceivedWaitingforInvoice,
                OpsPOStatus.Questions
                })) // Has cost edit and PO is an advanced state
        {
            var mostRecentApprovedValue = NePOProg.GetMostRecentApprovedValue(PoHeader.poprog_id);
            var totalCostBase = NePOProg.TotalCostWithoutLine(PoHeader.poprog_id, newtableid);
            var zeroTotalCost = false;
            if (totalCostBase == 0)
            {
                totalCostBase = NePOProg.TotalCost(PoHeader.poprog_id);
                zeroTotalCost = true;
            }
            var costRemoval = Convert.ToDecimal(oldcost) * Convert.ToDecimal(oldqty);
            var costAddition = Convert.ToDecimal(newcost) * Convert.ToDecimal(newqty);
            var totalCostWithNewCost = zeroTotalCost ? costAddition : totalCostBase + costAddition;
            //var editTolerance = Math.Abs(mostRecentApprovedValue * company.EditTolerance);

            #region "Purchaser limit For PO amount update"
            var PurchaserLimit = Toolbox.doSQL_int("SELECT IFNULL(max(amount_to),0) from business_unit_po_dist WHERE   business_unit_id = @v0  AND member_id =@v1 ORDER BY amount_from", new object[] { Convert.ToInt32(hidCompanyID.Value), Convert.ToInt32(current_user.id) } );
            if (newcost > oldcost)
            {
                // If PurchaserLimit is 0 (meaning no data found for this business unit):
                if (PurchaserLimit == 0)
                {
                    throw new Exception(
                        "Unable to complete update, as current user does not have PO approval limit set for Business Unit."
                    );

                }

                // If PurchaserLimit is greater than 0, check if total cost exceeds the limit
                if (Math.Abs(totalCostWithNewCost) > PurchaserLimit)
                {
                    throw new Exception(
                        "Unable to complete update, as resulting PO total exceeds current user’s PO approval limit of " + PurchaserLimit + " dollars for Business Unit."
                    );
                }
            }
        }
        #endregion
        //if (doBv)
        //{
        //    var lockinfo = PurchaseOrder.CheckPOLock(company.DSN, poprogress.poprog_bvpo);
        //    if (lockinfo != "")
        //    {
        //        throw new Exception("This PO is Locked By User " + lockinfo + " and cannot be Changed at this moment");
        //    }
        //    PurchaseOrder.LockOrder(company.DSN, poprogress.poprog_bvpo, current_user.Initials);
        //}
        try
        {
            var qtytodate = details.po_details_qty_received;


            if (newpartnumber != "")
            {
                podetails.po_details_part_no = Convert.ToInt32(newpartnumber);
            }
            else
            {
                podetails.po_details_part_no = 0;
            }
            podetails.po_details_vendor_part_no = newvendorpartnumber;
            podetails.po_details_description = newdescription;


            //podetails.po_details_notes = newnotes;
            podetails.po_details_qty_orderd = podetails.po_details_part_no == 0 ? 0 : newqty;

            if ((details.po_details_qty_orderd > 0 && podetails.po_details_qty_orderd < 0) ||
                (details.po_details_qty_orderd < 0 && podetails.po_details_qty_orderd > 0))
            {
                // #463 for editing, if the previous is positve, then the new one must be positive...
                // All should be positive or negative.
                throw new PositiveNegativeException(
                    string.Format("NESI shall not accept both positive and negative quantities for Purchase Order {0}",
                        main_id));
            }

            podetails.po_details_qty_received = podetails.po_details_part_no == 0 ? 0 : new_qty_committed;
            podetails.po_details_vendor_qty_per = newqtyperpart;
            podetails.po_details_sell_price = 0;

            podetails.po_details_cost = newcost;

            podetails.po_details_tax1 = company.Tax1 > 0 ? 1 : 0;
            podetails.po_details_tax2 = company.Tax2 > 0 ? 1 : 0;
            podetails.po_details_tax3 = company.Tax3 > 0 ? 1 : 0;
            podetails.po_details_tax4 = company.Tax4 > 0 ? 1 : 0;
            podetails.po_details_poprog_id = Convert.ToInt32(main_id.ToString());
            podetails.is_gl_account = new_is_gl_account;
            podetails.expense_category_id = Toolbox.ReturnZeroIfNull_int(hidWo.Value) == 0 && podetails.is_gl_account == true ? 0 : Convert.ToInt32(hidWo.Value);
            if (newworkorderid == "0")
            {
                podetails.po_details_woprog_id = details.po_details_woprog_id; // if work order is left at 0, set it to what it was..
            }
            else
            {
                podetails.po_details_woprog_id = Convert.ToInt32(newworkorderid);
            }

            podetails.po_details_date_added = Toolbox.MySQLNow_long();
            podetails.po_details_date_modified = Toolbox.MySQLNow_long();
            podetails.po_details_date_expected = newDateExpected;
            podetails.po_details_add_member_id = current_user.id32;
            podetails.po_details_audit_member_id = current_user.id32;
            podetails.po_details_id = newtableid;
            podetails.business_unit_id = PoHeader.business_unit_id;
            podetails.po_details_notes = podetails.po_details_notes;
            if (podetails.po_details_qty_orderd == podetails.po_details_qty_received && podetails.po_details_qty_orderd != 0)
            {
                podetails.po_details_line_active = 0;
            }
            podetails.PO_Details_Update();

            //try
            //{
            //    details.MoveToBVPORecord(details.po_details_poprog_id.ToString(), current_user.id.ToString());
            //}
            //catch (Exception ex)
            //{
            //    lblerrorLabel.Visible = true;
            //    lblerrorLabel.Text = "There was an issue trying to update BV: " + ex;
            //}
            // Not affecting the committed value, it did have something committed beforehand, not a stock PO line AND __________
            if (old_qty_committed == new_qty_committed && oldcost != newcost &&
                (newinv.allowed_to_edit_after_issue || allowed_ids.Contains(newtableid)) && old_qty_committed != 0 &&
                podetails.woprog_id > 200000 && !new_is_gl_account && !Toolbox.Contains(podetails.woprog_id, new[] { 9999997, 9999998, 9999999 }))
            {
                Toolbox.doSQL_void(conn, @"update po_details_current  set po_details_notes=@v0  where po_details_id=@v1 limit 1 ",
                    new object[] { current_user.FullName +
                                   " modified cost of " + details.po_details_part_no + " after PO was issued and parts were received from " +
                                   oldcost.ToString("c2") + " to " + newcost.ToString("c2"),newcost.ToString("c2"),newtableid });

                var historyupdate = "INSERT INTO poprog_notes (poprog_id, eventtext, date, member_id) VALUES(@v0,@v1,now(),@v2)";

                try
                {
                    Toolbox.doSQL_void(conn, historyupdate, new object[] {
                        main_id,("Modified cost of " + details.po_details_part_no +
                                 " after PO was issued and parts were received from " + oldcost + " to " + newcost),current_user.id
                        });
                }
                catch
                {
                }
                var actualqty = new_qty_committed - old_qty_committed;
                var totqty = actualqty + old_qty_committed;
                // ok find the part on the work order and update the cost and sell etc.
                //PO 0002004086-2 added:1 2016-01-04

                var keyValue =
                    Toolbox.doSQL_int(conn, @"
Select IFNULL(MAX(wo_detail_current_id),0)
from wo_detail_current inner join woprog on woprog.woprog_id = wo_detail_current_woprog_id 
and woprog.woprog_status not in('Waiting For PO','Waiting To Be Invoiced','Invoiced')  
where wo_detail_current_woprog_id =@v0 and wo_detail_current_master_id =@v1  
AND wo_detail_current_origin like CONCAT('%',@v2,'-',@v3,'%')", new object[] { newworkorderid, details.po_details_part_no, PoHeader.poprog_bvpo, details.po_details_rec_no });
                if (keyValue > 0)
                {
                    var wodc = new NeWODetailCurrent(keyValue);
                    wodc.qty_ordered = 0;
                    wodc.qty_committed = 0;
                    wodc.qty_invoiced = 0;
                    wodc.cost = newcost / newqtyperpart;
                    wodc.sell = shared.GetSellPrice(wodc.cost, 0, true, newqty - oldqty, WorkingBusinessUnit.id32);
                    wodc.save(current_user, "/sections/member/picklist/pikclist.aspx.cs - edited cost after issued and received", true);
                    //FillForPO(podetails.po_details_poprog_id);
                }
                else
                {
                    updatePOTotal();
                }
            }
            else if (old_qty_committed != new_qty_committed && newpartnumber != "0")
            {
                if (new_qty_committed > newqty)
                {
                    //Create Note
                    var historyupdate = "INSERT INTO poprog_notes ";
                    historyupdate += "(poprog_id, eventtext, date, member_id) ";
                    historyupdate += "VALUES(@v0,@v1,now(),@v2)";
                    try
                    {
                        Toolbox.doSQL_void(conn, historyupdate, new object[] {
                            main_id,("More parts were received than were ordered for part " + details.po_details_part_no ),current_user.id
                            });
                    }
                    catch
                    {
                    }
                }
                var actualqty = new_qty_committed - old_qty_committed;
                var totqty = actualqty + old_qty_committed;
                //try
                //{
                //    details.MoveToBVPORecord(details.po_details_poprog_id.ToString(), current_user.id.ToString());
                //}
                //catch (Exception ex)
                //{
                //    lblerrorLabel.Visible = true;
                //    lblerrorLabel.Text = "There was an issue trying to update BV: " + ex;
                //}
                try
                {
                    var realqty = new_qty_committed - qtytodate;
                    var part_no = 0;
                    int.TryParse(podetails.part_no, out part_no);

                    if (!((podetails.po_details_woprog_id == 9999999 || podetails.po_details_woprog_id == 9999998 ||
                           podetails.po_details_woprog_id == 9999997)) && part_no > 0)
                    {
                        try
                        {
                            var vprow = new vendor_price_row(part_no, WarehouseBusinessUnit.id, PoHeader.poprog_vendor_id,
                                podetails.vendor_part_no);

                            #region update vendor pricing

                            vprow.master_id = part_no;
                            vprow.member_id = current_user.id;
                            vprow.vendor_code = podetails.po_details_vendor_part_no;
                            vprow.business_unit_id = PoHeader.business_unit_id;
                            vprow.cost = podetails.cost / podetails.po_details_vendor_qty_per;
                            vprow.total = podetails.cost;
                            vprow.qty = podetails.po_details_vendor_qty_per;
                            vprow.vendor_id = PoHeader.poprog_vendor_id;
                            vprow.origin = "PO: " + PoHeader.poprog_bvpo;
                            vprow.save();

                            #endregion update vendor pricing
                        }
                        catch
                        {
                        }
                        if (!podetails.is_gl_account)
                        {
                            details.MoveToWOProgDetails(PoHeader.poprog_bvpo, details.po_details_poprog_id.ToString(),
                                newtableid.ToString(), current_user.id.ToString(), realqty);
                        }
                    }
                    // Making it so the receiving of po items ALWAYS updates inventory.
                    var fromstock = new Nestock_transfer();

                    fromstock.stock_transfer_type = new_qty_committed > 0 ? 2 : 5; //Origin ID for Purchase
                    fromstock.stock_transfer_master_id = podetails.po_details_part_no;
                    if (realqty > 0)
                    {
                        fromstock.stock_transfer_from_id = main_id;
                        fromstock.stock_transfer_quantity = realqty * newqtyperpart;
                    }
                    else
                    {
                        fromstock.stock_transfer_to_id = main_id;
                        fromstock.stock_transfer_quantity = realqty * newqtyperpart;
                    }

                    fromstock.stock_transfer_description = newdescription;
                    fromstock.stock_transfer_member_id = current_user.id;
                    fromstock.stock_transfer_note = "PO Quantity Updated";
                    fromstock.stock_transfer_business_unit_id = Convert.ToInt32(WarehouseBusinessUnit.id);
                    fromstock.stock_transfer_cost = newcost / newqtyperpart;
                    fromstock.po_line_id = Convert.ToInt32(newtableid);
                    if (fromstock.stock_transfer_quantity != 0)
                    {
                        fromstock.Nestock_transfer_save();
                    }
                }
                catch (Exception ex2)
                {
                    podetails.po_details_qty_received = old_qty_committed;
                    podetails.po_details_line_active = 1;
                    podetails.PO_Details_Update();
                    throw new Exception("There was an issue trying to update the work order: " + ex2);
                }


                //Let PM know part has arrived.
                if (!new_is_gl_account && details.woprog_id > 0)
                {
                    //		NePOProg.send_email_to_trackers(details.po_details_woprog_id.ToString(), "PO " + poprogress.poprog_bvpo + " - Parts Received", myMember);

                    var wo = new NeWOProg(details.woprog_id);
                    SendEmail(1, details.po_details_woprog_id.ToString(), wo.intProjectManager);
                }
            }
        }
        catch (PositiveNegativeException ex)
        {
            throw ex;
        }
        catch (Exception excep)
        {
            throw new Exception(excep.ToString());
        }
        finally
        {
            //  if (doBv)
            //  {
            //      PurchaseOrder.UnlockPO(company.DSN, poprogress.poprog_bvpo, current_user.Initials);
            //  }
        }
        if (oldvendorpartnumber != newvendorpartnumber && PoHeader.poprog_status != OpsPOStatus.NotIssued && PoHeader.poprog_status != OpsPOStatus.ApprovedtoOrder &&
            PoHeader.poprog_status != OpsPOStatus.Questions)
        {
            var vendorbvno =
                Toolbox.doSQL_string(conn, @"SELECT Vendor_Number FROM Vendor WHERE Vendor_ID = @v0 ", new object[] { PoHeader.poprog_vendor_id });
            //ak			Toolbox.doSQL_void(conn,@"UPDATE inventory_price SET vendor_code = @v0 , edited_dt = now() WHERE master_id = @v1  AND business_unit_id = @v2  AND qty = @v3  AND vendor_id = @v4 ", new object[] {  newvendorpartnumber, newpartnumber, hidCompanyID.Value, newqtyperpart, poprogress.poprog_vendor_id } );
        }

        updatePOTotal();
        //InsertGridInformation(main_id);
    }

    protected void AddNewPOLine()
    {
        var strMemberName = current_user.Username;
        var intMemberID = current_user.id;
        //Inventory neInv = new Inventory();
        //neInv.Load(newpartno, hidCompanyID.Value.ToString());
        //myMember = new NeMember(Session["session"].ToString());
        var podetails = new NEPO_Details_Current();
        podetails.po_details_part_no = newpartnumber != "" ? Convert.ToInt32(newpartnumber) : 0;
        podetails.po_details_vendor_part_no = newvendorpartnumber;
        podetails.po_details_description = newdescription;
        podetails.po_details_notes = newNotes;
        //po_details_qty_ordered
        podetails.po_details_qty_orderd = newqty;
        podetails.po_details_qty_received = 0;
        podetails.po_details_sell_price = 0;
        podetails.po_details_cost = newcost;
        if (podetails.po_details_part_no == 0)
        {

            if (hidWo.Value == null)
            {
                podetails.po_details_cost = 0;
                podetails.po_details_qty_orderd = 0;
            }
            podetails.po_details_line_active = 0;
        }




        var canContinue = this.CanAddNewPOFromPastWo(main_id, podetails.po_details_qty_orderd);
        if (!canContinue)
        {
            throw new PositiveNegativeException(
                string.Format("NESI shall not accept both positive and negative quantities for Purchase Order {0}",
                    main_id));
        }

        podetails.po_details_tax1 = newtax1;
        podetails.po_details_tax2 = newtax2;
        podetails.po_details_tax3 = newtax3;
        podetails.po_details_tax4 = newtax4;
        podetails.po_details_poprog_id = Convert.ToInt32(main_id.ToString());
        podetails.po_details_woprog_id = Convert.ToInt32(newworkorderid);


        podetails.is_gl_account = new_is_gl_account;

        podetails.po_details_date_added = Toolbox.MySQLNow_long();
        podetails.po_details_date_modified = Toolbox.MySQLNow_long();
        podetails.po_details_date_expected = newDateExpected;
        podetails.po_details_add_member_id = Convert.ToInt32(intMemberID);
        podetails.po_details_audit_member_id = Convert.ToInt32(intMemberID);
        podetails.business_unit_id = Convert.ToInt32(hidCompanyID.Value);
        podetails.po_details_vendor_qty_per = newpartnumber != "" ? newqtyperpart : 0;
        podetails.expense_category_id = Toolbox.ReturnZeroIfNull_int(hidWo.Value) == 0 && podetails.is_gl_account || string.IsNullOrEmpty(hidWo.Value)
                                       ? 0
                                       : Convert.ToInt32(hidWo.Value); //Convert.ToInt32(ddlWorkOrder.Value);
        // Check to make sure this won't result in a 0 sell when transferred to a work order.
        if (podetails.po_details_cost / podetails.po_details_vendor_qty_per <= 0.0005)
        {
            throw new Exception("The qty per is too high, is this a per one cost?");
        }
        podetails.PO_Details_Save();
        var linecount =
            Toolbox.doSQL_int(conn, @"Select count(po_details_id) from po_details_current  WHERE po_details_poprog_id =@v0", new object[] { main_id });
        if (linecount == 1)
        {
            agv.DataBind();
        }
        updatePOTotal();
    }


    private bool CanAddNewPOFromPastWo(int main_id, double quantityToBeAdded)
    {
        var list = new List<POLineItem> { };

        var dt_check_qty_per = Toolbox.doSQL_dt(@"SELECT * FROM po_details_current  where po_details_poprog_id =@v0", new object[] { main_id });
        if (dt_check_qty_per == null || dt_check_qty_per.Rows == null || dt_check_qty_per.Rows.Count == 0)
        {
            // NO Line Items inside, so can add new line itmes.
            return true;
        }

        foreach (DataRow dr in dt_check_qty_per.Rows)
        {
            list.Add(new POLineItem
            {
                po_details_id = Convert.ToInt32(dr["po_details_id"]),
                po_details_qty_ordered = Convert.ToDouble(dr["po_details_qty_ordered"])
            });
        }

        var positive = list.Where(item => item.po_details_qty_ordered > 0).Count();
        var negative = list.Where(item => item.po_details_qty_ordered < 0).Count();

        if (quantityToBeAdded > 0 && negative > 0)
        {
            // Existing negative numbers & want to add a positive item
            return false;
        }

        if (quantityToBeAdded < 0 && positive > 0)
        {
            // Existing postive numbers & want to add a negative item
            return false;
        }

        return true;
    }

    protected void updatePOTotal()
    {
        try
        {
            var POTotal = Toolbox.doSQL_double(conn, @"Select ifnull((SELECT SUM(po_details_qty_ordered * po_details_cost) FROM po_details_current  WHERE po_details_poprog_id =@v0),0) ", new object[] { main_id });
            POTotal = Math.Round(POTotal, 2);
            var POrecTotal = Toolbox.doSQL_double(conn, @"Select ifnull((SELECT SUM(po_details_qty_received * po_details_cost) FROM po_details_current  WHERE po_details_poprog_id =@v0),0) ", new object[] { main_id });
            POrecTotal = Math.Round(POrecTotal, 2);

            Toolbox.doSQL_void(conn, @"UPDATE poprog_header  SET poprog_total_cost =@v0,poprog_total_recCost =@v1 , poprog_ts=poprog_ts   WHERE poprog_id =@v2", new object[] { POTotal, POrecTotal, main_id });
        }
        catch (Exception ee)
        {
            Toolbox.do_errorLog_errorStack(ee);
        }
    }

    #endregion

    #region Send Email

    protected void SendEmail(int messagetype, string woid, int to_member_id)
    {
        var fromemail = "";
        var toemail = "";
        var ccemail = "";
        var subject = "";
        var body = "";

        if (messagetype == 1)
        {
            //Send Mail to Project Manager Letting them Know that their Part(s) has arrived
            var PMID = 0;
            var compid = "";
            var BVWO = "";
            var customername = "";
            var progress = new NeWOProg(Convert.ToInt32(woid));
            //		string intcosql = "SELECT Internal_CompanyNo_ID FROM internal_companyno WHERE Internal_CompanyNo_Intranet_CustID = " + progress.WOProg_Customer_ID.ToString();
            //		string intcompid = "";
            //		try
            //			{
            //			intcompid = Toolbox.doSQL_string(conn,@intcosql  , null);
            //			}
            //		catch { }
            //		if (intcompid != "")
            //			return;

            var poprogress = new NePOProg(main_id);
            PMID = to_member_id;
            compid = progress.business_unit_id.ToString();
            BVWO = progress.OrderNumber;
            customername = progress.CustomerName;
            try
            {
                toemail = Toolbox.doSQL_string(conn, @"SELECT member_neemail FROM member  WHERE member_id =@v0", new object[] { PMID });
            }
            catch
            {
            }
            try
            {
                ccemail =
                    Toolbox.doSQL_string(conn, @"SELECT member_neemail FROM member  WHERE member_membertype_id = 9 AND member_status='Active' AND member_business_unit_id=@v0 limit 1 ", new object[] { compid });
            }
            catch
            {
            }
            fromemail = ccemail;
            subject = "Parts for WO:" + BVWO + " for " + customername + " have arrived into stock.";
            body = "Your part: " + newpartnumber + ": " + newdescription + " has arrived for the job. ";
            body += new_qty_committed + " have arrived to date, of " + newqty + " ordered.\n";
            body += "The part(s) was ordered " + poprogress.poprog_order_placed_date;
        }

        if (toemail != "")
        {
            try
            {
                var e = new NeEMail();
                //		e.To = toemail;
                e.To = "debug@" + Toolbox.app_setting("DomainForEmail");
                e.From = fromemail == "" ? "admin@" + Toolbox.app_setting("DomainForEmail") : fromemail;
                e.Subject = subject;
                e.Body = body;
                e.CC = ccemail;
                e.Bcc = "debug@" + Toolbox.app_setting("DomainForEmail");
                e.Send();
            }
            catch
            {
            }
        }
        else
        {
            var e = new NeEMail();
            e.To = "mhyde@thatsnew.com";
            e.From = fromemail == "" ? "administrator@thatsnew.com" : fromemail;
            e.Subject = subject;
            e.Body = body;
            e.Send();
        }
    }

    #endregion

    #region Editor Initilization

    protected void agv_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {

        var grid = (ASPxGridView)sender;
        var editingkey = e.KeyValue;
        var editrowlineid = Convert.ToInt32(editingkey);
        var consignment_id = 0;
        var _is_progress = false;
        double poQtyReceived = 0;
        double poReference = 0;
        var dt = Toolbox.doSQL_dt(@"SELECT po_details_qty_received,po_details_reference FROM po_details_current WHERE po_details_id=@v0", new object[] { editingkey });
        foreach (DataRow porow in dt.Rows)
        {
            poQtyReceived = Convert.ToDouble(porow["po_details_qty_received"]);
            poReference = Convert.ToDouble(porow["po_details_reference"]);
        }
        var value = grid.GetRowValues(e.VisibleIndex, "part_no");
        var part_no = 0;
        var row_origin = grid.GetRowValues(e.VisibleIndex, "origin").ToString();
        int.TryParse(value.ToString(), out part_no);
        var is_expense = row_origin.Contains("Expense");
        var _quote = new quote();


        var poprogress = new NePOProg();
        ;
        if (origin == "purchaseorder")
        {
            poprogress = new NePOProg(main_id);
        }

        #region part_no

        if (e.Column.FieldName == "part_no")
        {
            FlagPartcode = Convert.ToString(e.Value) != "";
            try
            {
                //partnum = Convert.ToInt32(e.Value);
            }
            catch
            {
            }

            /*if (partnum >= 990000 && this_origin == "workorder")
				{
					lockdesc = true;
				}
				else
				{*/
            lockdesc = false;
            //}
            if (origin == "purchaseorder" && poprogress.poprog_status == OpsPOStatus.NotIssued)
            {
                e.Editor.ClientEnabled = false;
            }
            else if (origin == "quote")
            {
                e.Editor.ClientEnabled = false;
            }
            else if (part_no == 777 && current_user != null && current_user.AuthenticatedForPrivilege(71))
            {
                e.Editor.ClientEnabled = false;
            }
            else
            {
                //e.Editor.Enabled = false;
                e.Editor.ClientEnabled = false;
            }
        }

        #endregion part_no

        #region qty_per_part

        if (e.Column.FieldName == "qty_per_part")
        {
            if (origin == "purchaseorder")
            {
                // try
                // {
                //  poQtyReceived = Toolbox.doSQL_double(conn, poqtyrecstr, null);
                //  }
                // catch
                //  {
                //  }
            }
            if (origin == "purchaseorder" && poQtyReceived == 0)
            {
                e.Editor.ClientEnabled = true;
            }
            else
            {
                e.Editor.ClientEnabled = false;
            }
        }

        #endregion qty_per_part

        #region vend_part_no

        if (e.Column.FieldName == "vend_part_no")
        {
            /*try
            {
                poQtyReceived = Toolbox.doSQL_double(conn, poqtyrecstr, null);
            }
            catch
            {
            }*/
            if (origin == "purchaseorder" && poQtyReceived != 0)
            {
                e.Editor.ReadOnly = true;
            }
        }

        #endregion vend_part_no

        #region cost

        if (e.Column.FieldName == "cost")
        {
            e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
            if (origin == "purchaseorder")
            // && poprogress.poprog_status == 1)|| (this_origin == "purchaseorder" && poprogress.edit_after_issue == 1))
            {

                if (NePOProg.ReceiptsExist(poprogress.poprog_id, poReference))
                {
                    e.Editor.ClientEnabled = false;
                    return;
                }
                var inv = new inventory();
                if (part_no != 0 && part_no < 990000)
                {
                    inv.Load(part_no, _quote.business_unit_id);
                }
                /*  try
                  {
                      poQtyReceived = Toolbox.doSQL_double(conn, poqtyrecstr, null);
                  }
                  catch
                  {
                  }*/
                //e.Editor.ReadOnly = false;
                //e.Editor.Enabled = true;
                if (part_no != 0)
                {
                    if (inv.part_is_loaded)
                    {
                        e.Editor.ClientEnabled = true;
                    }
                    else
                    {
                        e.Editor.ClientEnabled = poQtyReceived == 0;
                    }
                }
                else if (part_no == 0)
                {
                    e.Editor.ClientVisible = false;
                }
            }
        }

        #endregion cost

        #region sell

        if (e.Column.FieldName == "sell" && origin == "workorder")
        {
            e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
            if (member_can_edit_sell && member_can_see_sell && !_is_progress)
            {
                //if (part_no != 2139)
                //	{

                e.Editor.ReadOnly = false;
                e.Editor.ClientEnabled = true;
                //}
            }
        }

        #endregion sell

        #region description

        if (e.Column.FieldName == "description")
        {
            if (lockdesc || origin == "rfq" || consignment_id != 0)
            {
                e.Editor.ClientEnabled = false;
            }

            if (origin == "purchaseorder")
            {
                var qtyrecd =
                    Toolbox.doSQL_double(conn, @"SELECT po_details_qty_received FROM po_details_current  WHERE po_details_id=@v0", new object[] { editrowlineid });
                if (qtyrecd != 0)
                {
                    //e.Editor.ReadOnly = true;
                    // e.Editor.Enabled = false;
                    e.Editor.ClientEnabled = false;
                }
            }
        }
        /*if (e.Column.FieldName == "qtyrec")
			{
				try
				{
					poQtyReceived = Convert.ToDouble(e.Value);
				}
				catch { }
				if(poprogress.poprog_status == 1 || (poQtyReceived >= poqty))
				e.Editor.Enabled = false;
			}*/

        #endregion description

        #region qty

        if (e.Column.FieldName == "qty")
        {
            if (!Toolbox.Contains(poprogress.poprog_status, new int[] { OpsPOStatus.NotIssued, OpsPOStatus.Questions, OpsPOStatus.APProblems }))
            {
                e.Editor.ClientEnabled = false;
                return;
            }
            e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
            e.Editor.Attributes["data-line_id"] = e.KeyValue.ToString();
            try
            {
                poqty = Convert.ToDouble(e.Value);
            }
            catch
            {
            }
            /*  if (origin == "purchaseorder")
              {
                  try
                  {
                      poQtyReceived = Toolbox.doSQL_double(conn, poqtyrecstr, null);
                  }
                  catch
                  {
                  }
              }*/
            if (origin != "purchaseorder")
            {
                e.Editor.ClientEnabled = false;
            }
            if (consignment_id != 0)
            {
                e.Editor.ClientEnabled = false;
            }
        }

        #endregion qty

        #region qtyrec

        if (e.Column.FieldName == "qtyrec" && origin == "purchaseorder")
        {
            if (!Toolbox.Contains(poprogress.poprog_status, new int[] { OpsPOStatus.NotIssued, OpsPOStatus.Questions, OpsPOStatus.APProblems }))
            {
                e.Editor.ClientEnabled = false;
                return;
            }
            try
            {
                agv.JSProperties["cpOrigValue"] = e.Value.ToString();
                agv.JSProperties["cplineid"] = e.KeyValue;
            }
            catch
            {
            }
            if (origin == "purchaseorder")
            {
                e.Editor.ClientEnabled = false;
            }
            if (consignment_id != 0)
            {
                e.Editor.ClientEnabled = false;
            }
        }

        #endregion qtyrec

        #region  emptyval  // qty received to date?

        // MH -- This is the text field for the committing via work order... badly named field. It should always be hidden on edit.
        if (e.Column.FieldName == "emptyval" && (origin == "workorder" || origin == "purchaseorder"))
        {
            e.Editor.ClientVisible = false;
        }

        #endregion emptyval

        #region workorder

        if (e.Column.FieldName == "workorder" && origin == "purchaseorder")
        {
            var qtyrecd =
                Toolbox.doSQL_double(conn, @"SELECT po_details_qty_received FROM po_details_current  WHERE po_details_id=@v0", new object[] { editrowlineid });
            var list = new List<int> { 1, 3 }.ToArray();
            if (qtyrecd != 0 ||
                (qtyrecd == 0 && !Toolbox.Contains(poprogress.poprog_status, list))
                )
            {
                e.Editor.ClientEnabled = false;
            }

        }
        else if (e.Column.FieldName == "workorder" && origin == "workorder")
        {
            e.Editor.ClientVisible = false;
        }

        #endregion workorder

        #region extended_per

        if (e.Column.FieldName == "extended_per")
        {
            e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
            if (origin == "purchaseorder")
            {
                e.Editor.ClientEnabled = false;
            }
            if (origin == "workorder")
            {
                e.Editor.Visible = false;
            }
        }

        #endregion extended_per

        #region extTandM

        if (e.Column.FieldName == "extTandM")
        {
            e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
            e.Editor.Value = string.Format("{0:N3}", e.Value).Replace(",", "");
            if (member_can_edit_sell)
            {
                e.Editor.ClientEnabled = true;
            }
            else
            {
                e.Editor.ClientEnabled = false;
                //	e.Editor.ClientEnabled = false;
            }
            if (origin == "purchaseorder")
            {
                e.Editor.ClientEnabled = false;
            }
        }

        #endregion extTandM

        #region wo_detail_current_rec_no

        if (e.Column.FieldName == "wo_detail_current_rec_no" && origin == "purchaseorder")
        {
            e.Editor.ClientEnabled = false;
        }

        #endregion wo_detail_current_rec_no

        #region division_id

        if (e.Column.FieldName == "Division_ID")
        {
            if (origin == "purchaseorder")
            {
                var qtyrecd =
                    Toolbox.doSQL_double(conn, @"SELECT po_details_qty_received FROM po_details_current  WHERE po_details_id=@v0", new object[] { editrowlineid });
                if (qtyrecd != 0)
                {
                    //e.Editor.ReadOnly = true;
                    // e.Editor.Enabled = false;
                    e.Editor.ClientEnabled = false;
                }
            }
        }

        #endregion division_id

        #region wo_detail_current_discount

        if (e.Column.FieldName == "wo_detail_current_discount")
        {
            e.Editor.Attributes.Add("onkeydown", "only_numeric(event);");
            if (!member_can_discount || _is_progress || part_no >= 990000 || is_expense)
            {
                e.Editor.ClientEnabled = false;
            }
        }

        #endregion wo_detail_current_discount

        #region qty_avail

        if (e.Column.FieldName == "qty_avail")
        {
            e.Editor.ClientVisible = origin != "workorder";
            e.Editor.ClientEnabled = false;
        }

        #endregion qty_avail

        #region billtype

        if (e.Column.FieldName == "wo_detail_current_billtypeid")
        {
            e.Editor.ClientEnabled = member_can_see_sell && member_can_edit_sell;
        }

        #endregion

        #region select

        if (e.Column.Name == "select")
        {
            e.Editor.Visible = false;
        }

        #endregion

        #region trans

        if (e.Column.FieldName == "Trans")
        {
            e.Editor.Visible = false;
        }

        #endregion

        #region reqdate

        if (e.Column.FieldName == "reqdate")
        {
            e.Editor.ClientEnabled = !is_expense;
        }

        #endregion


    }

    protected void agv_ParseValue(object sender, ASPxParseValueEventArgs e)
    {
        if (e.FieldName == "workorder")
        {

            try
            {
                e.Value = e.Value == null ? 0 : int.Parse(e.Value.ToString());
            }
            catch
            {
                e.Value = 0;
            }
            if (e.FieldName == "emptyval")
            {
                try
                {
                    e.Value = Convert.ToDecimal(e.Value);
                }
                catch
                {
                }
            }
        }
    }

    protected void agv_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
    {
        var grid = (ASPxGridView)sender;
        wo_status_str = wo_status_str == "" && origin == "workorder"
            ? Toolbox.doSQL_string(conn, @"SELECT woprog_status FROM woprog WHERE woprog_id = @v0 ", new object[] { main_id })
            : wo_status_str;
        var column_chkbox = (GridViewDataColumn)grid.Columns["select"];
        switch (e.Column.Name)
        {
            case "transfer":
                if (origin == "workorder" && !new List<string>(new[]
                                                                    {
                                                                           OpsWOStatus.Invoiced,
                                                                           OpsWOStatus.WaitingToBeInvoiced,
                                                                           OpsWOStatus.WaitingParentBMApproval,
                                                                           OpsWOStatus.WaitingForPO,
                                                                           OpsWOStatus.WaitingBMApproval,
                                                                           OpsWOStatus.WaitingPMApproval
                                                                        }).Contains(wo_status_str) && column_chkbox.Visible)
                {
                    e.Cell.Text =
                        "<div class=\'date_req_container dxbButton\' style=\'cursor:pointer;padding:3px;width:22px;height:22px\'><img src=\'/images/icon/transfer.jpg\' style=\'cursor:pointer;padding:3px;width:16px;height:16px\' align=\'absmiddle\' title=\'Mass Transfer\' onclick=\'transfer_multiple_toggle(this)\'/></div>";
                }
                break;
            case "required_date":
                if (origin == "workorder" && !new List<string>(new[]
                                                                    {
                                                                           OpsWOStatus.Invoiced,
                                                                           OpsWOStatus.WaitingToBeInvoiced,
                                                                           OpsWOStatus.WaitingParentBMApproval,
                                                                           OpsWOStatus.WaitingForPO,
                                                                           OpsWOStatus.WaitingBMApproval,
                                                                           OpsWOStatus.WaitingPMApproval
                                                                        }).Contains(wo_status_str) && column_chkbox.Visible)
                {
                    e.Cell.Text =
                        "<div class=\'date_req_container dxbButton\' style=\'cursor:pointer;padding:3px;width:22px;height:22px\'><img src=\'/images/icon/icon[calendar].gif\' style=\'cursor:pointer;padding:3px;width:16px;height:16px\' align=\'absmiddle\' title=\'Mass Edit Required Date(s)\' onclick=\'reqdate_edit(this)\'/></div>";
                }
                break;
            case "billtype_id":
                if (origin == "workorder" && member_can_see_sell && member_can_edit_sell && !new List<string>(new[]
                                                                                                                {
                                                                                                                      OpsWOStatus.Invoiced ,
                                                                                                                      OpsWOStatus.WaitingToBeInvoiced ,
                                                                                                                      OpsWOStatus.WaitingParentBMApproval ,
                                                                                                                      OpsWOStatus.WaitingForPO ,
                                                                                                                      OpsWOStatus.WaitingPMApproval
                                                                                                                    }).Contains(wo_status_str) && column_chkbox.Visible)
                {
                    try
                    {
                        e.Cell.Text =
                            "<div class=\'billtype_container dxbButton\' style=\'cursor:pointer;padding:3px;width:22px;height:22px\'><img src=\'/images/icon/icon[accounting].gif\' title=\'Mass set bill type(s)\'  style=\'cursor:pointer;padding:3px;width:16px;height:16px\' align=\'absmiddle\' alt=\'Mass set bill type(s)\' onclick=\'billtype_edit(this)\'/></div>";
                    }
                    catch (Exception ee)
                    {

                    }
                }
                break;
        }
    }

    protected void agv_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType == GridViewRowType.Preview)
        {
            e.Row.BackColor = last_color;
        }
        else
        {
            last_color = e.Row.BackColor == Color.Empty ? Color.White : e.Row.BackColor;
        }
    }

    protected void agv_DataBound(object sender, EventArgs e)
    {

        set_location_ds(sender, e);

    }

    private Dictionary<int, string> wo_tooltips = new Dictionary<int, string>();

    private string wo_status_str = "";
    private readonly Dictionary<int, string> section_names = new Dictionary<int, string>();

    protected void agv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        //StackTrace st = new StackTrace ();
        //StackFrame sf = st.GetFrame (0);
        //MethodBase currentMethodName = sf.GetMethod();
        //Debug.Write(currentMethodName.Name+" "+DateTime.Now.ToString("U")+" Start\n");

        var grid = (ASPxGridView)sender;
        var wo_id = 0;
        var is_gl_account = false;
        var row_id = grid.GetRowValues(e.VisibleIndex, "id");
        var col_workorders = grid.Columns["workorder"];
        var this_row = grid.GetDataRow(e.VisibleIndex);

        var this_wo_id = this_row == null || this_row["workorder"] == null ? "" : this_row["workorder"].ToString();
        if (col_workorders.Visible && this_wo_id != "")
        {
            int.TryParse(this_wo_id, out wo_id);
        }
        if (origin == "purchaseorder")
        {
            bool this_is_gl_account = this_row == null || this_row["is_gl_account"] == null
                ? false
                : (bool)this_row["is_gl_account"];
            bool.TryParse(this_is_gl_account.ToString(), out is_gl_account);
        }
        var part_no = 0;
        if (grid.GetRowValues(e.VisibleIndex, "part_no") != null)
        {
            int.TryParse(grid.GetRowValues(e.VisibleIndex, "part_no").ToString(), out part_no);
        }

        var cost = origin == "purchaseorder" ? Toolbox.doSQL_double(@"SELECT po_details_cost FROM po_details_current WHERE po_details_id=@v0", new object[] { row_id }) : 0;

        if (dt_excludes == null)
        {
            var part_numbers = new List<string>();
            for (var wi = 0; wi < grid.VisibleRowCount; wi++)
            {
                var part_nu = grid.GetRowValues(wi, "part_no").ToString() == ""
                    ? 0
                    : Convert.ToInt32(grid.GetRowValues(wi, "part_no").ToString());
                if (part_nu < 900000 && part_nu != 0)
                {
                    part_numbers.Add(grid.GetRowValues(wi, "part_no").ToString());
                }
            }
            if (part_numbers.Count > 0)
            {
                dt_excludes =
                    Toolbox.doSQL_dt(conn, string.Format(@"SELECT a.master_id, b.is_exclude, b.allowed_to_stock, a.tag_id, b.is_rental  
FROM inventory_item_master a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id WHERE a.master_id IN ({0} )", string.Join(",", part_numbers.ToArray())), null);
            }
        }

        var workorder_column = (GridViewDataColumn)grid.Columns["workorder"];
        var location_combo =
            (ASPxComboBox)grid.FindRowCellTemplateControl(e.VisibleIndex, workorder_column, "combo_location");
        var column_chkbox = (GridViewDataColumn)grid.Columns["select"];
        var c_value = 0;
        switch (e.DataColumn.FieldName)
        {
            #region description

            case "description":
                try
                {
                    e.Cell.CssClass += " description";
                    e.Cell.Style.Add("min-width", "150px");
                    e.Cell.ToolTip = Toolbox.ReturnBlankIfNull_string(e.CellValue);
                    if (origin == "purchaseorder" && part_no > 0 && !Toolbox.Contains(wo_id, new[] { 9999999, 9999998, 9999997 }) &&
                        wo_id > 30000)
                    {
                        // Valid Work Order Connection
                        wo_status_str = Toolbox.doSQL_string(conn, @"SELECT woprog_status FROM woprog WHERE woprog_id = @v0 ", new object[] { wo_id });
                        var table_prefix = new List<string>(new[] { OpsWOStatus.Invoiced, OpsWOStatus.WaitingToBeInvoiced }).Contains(wo_status_str)
                            ? "history"
                            : "current";
                        var line_exists =
                            Toolbox.doSQL_int(conn, string.Format(@"SELECT COUNT(wo_detail_{0}_id) FROM wo_detail_{0}  WHERE wo_detail_{0}_woprog_id = @v0  AND wo_detail_{0}_master_id = @v1 ", table_prefix),
                            new object[] { wo_id, part_no }) > 0;
                        e.Cell.ForeColor = line_exists ? Color.Green : Color.Red;
                        e.Cell.ToolTip = line_exists ? e.Cell.ToolTip : "*Does not already exist on work order - " + e.Cell.ToolTip;

                    }
                }
                catch (Exception ee)
                {
                }
                break;

            #endregion description

            #region workorder

            case "workorder":
                e.Cell.Style.Add("overflow", "hidden");
                if (origin == "workorder")
                {
                    e.Cell.ToolTip = "Select where the parts came from";
                }
                else if (origin == "purchaseorder")
                {

                    var hh = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Cell.Controls[0].Controls[0].FindControl("wo_link");


                    if (dt_expense_categories.Select("expense_category_id=" + Toolbox.ReturnBlankIfNull_string(e.CellValue)).Length > 0 || dt_additional_categories.Select("po_details_woprog_id=" + Toolbox.ReturnBlankIfNull_string(e.CellValue)).Length > 0)
                    {
                        if (dt_expense_categories.Select("expense_category_id=" + Toolbox.ReturnBlankIfNull_string(e.CellValue)).Length > 0)
                        {
                            hh.InnerHtml = "Exp-" + dt_expense_categories.Select("expense_category_id=" + Toolbox.ReturnBlankIfNull_string(e.CellValue))[0]["Name"];
                        }
                        else if (dt_additional_categories.Select("po_details_woprog_id=" + Toolbox.ReturnBlankIfNull_string(e.CellValue)).Length > 0)
                        {
                            hh.InnerHtml = "Exp-" + dt_gl_tes.Select("id=" + Toolbox.ReturnBlankIfNull_string(e.CellValue))[0]["account_no"] + " " + dt_additional_categories.Select("po_details_woprog_id=" + Toolbox.ReturnBlankIfNull_string(e.CellValue))[0]["gl_chart_name"];
                        }
                        e.Cell.ToolTip = hh.InnerHtml;
                    }
                    else if (e.CellValue.ToString().StartsWith("9999999"))
                    {
                        hh.InnerHtml = "ORDER FOR STOCK";

                    }
                    else
                    {
                        wo_status_str = wo_status_str == "" && origin == "workorder"
                            ? Toolbox.doSQL_string(conn, @"SELECT woprog_status FROM woprog WHERE woprog_id = @v0 ", new object[] { main_id })
                            : wo_status_str;

                        if (wo_id != 0 && !is_gl_account && !new List<int>(new[] { 9999999, 9999998, 9999997 }).Contains(wo_id))
                        {
                            var wo_tooltip = "";
                            try
                            {

                                var dt_wo_parts = Toolbox.doSQL_dt(conn,
                                    @"SELECT wo_detail_current.wo_detail_current_master_id, round(sum(wo_detail_current.wo_detail_current_qty_ordered),2) qty_ord, round(sum(wo_detail_current.wo_detail_current_qty_committed),2) qty_com, round(sum(wo_detail_current.wo_detail_current_qty_ordered-wo_detail_current.wo_detail_current_qty_committed),2) unfulfilled FROM wo_detail_current  WHERE wo_detail_current.wo_detail_current_woprog_id =@v0 and wo_detail_current.wo_detail_current_master_id =@v1  group by wo_detail_current.wo_detail_current_master_id",
                                    new object[] { this_wo_id, part_no });
                                if (dt_wo_parts.Rows.Count > 0)
                                {
                                    wo_tooltip = "Part " + part_no + " - Required Qty: " + dt_wo_parts.Rows[0]["qty_ord"] +
                                                 "  Committed Qty: " +
                                                 dt_wo_parts.Rows[0]["qty_com"] + "  Unfulfilled Qty: " +
                                                 dt_wo_parts.Rows[0]["unfulfilled"] + "  WO ";
                                }
                                else
                                {
                                    wo_tooltip = "Part " + part_no + " Not on work order yet. ";
                                }
                                wo_tooltip +=
                                    Toolbox.do_value_from(
                                        Toolbox.doSQL_string(conn,
                                            @"SELECT CONCAT(WOProg_BVWO,' - ', WOPRog_CustomerName, ' ', WOProg_Description, ' - ', ddl_name) FROM woprog, business_unit WHERE business_unit_id = id and woprog_id = @v0 ",
                                            new object[] { wo_id }), false);


                                e.Cell.ToolTip = wo_tooltip;


                            }
                            catch
                            {
                                wo_tooltip = "There was an error retrieving tool tip information for the attached workorder";
                            }


                        }
                        else
                        {
                            e.Cell.ToolTip = Toolbox.ReturnBlankIfNull_string(e.CellValue).ToString();
                        }



                    }

                    //   hh.InnerHtml ="";


                }
                break;

            #endregion workorder

            #region vend_part_no

            case "vend_part_no":
                try
                {
                    e.Cell.Attributes.Add("onclick", "picklist.vendor_part_toggle(this)");
                    e.Cell.ToolTip = Toolbox.ReturnBlankIfNull_string(e.CellValue).ToString();
                }
                catch
                {
                }
                break;

            #endregion vend_part_no

            #region sectionid

            case "sectionid":
                try
                {
                    int.TryParse(Toolbox.ReturnZeroIfNull_int(e.CellValue).ToString(), out c_value);
                    if (!section_names.ContainsKey(c_value))
                    {
                        var section_name =
                            Toolbox.do_value_from(
                                Toolbox.doSQL_string(conn, @"SELECT section FROM quote_section WHERE id = @v0 ", new object[] { e.CellValue }), false);
                        section_names.Add(c_value, section_name);
                    }

                    e.Cell.ToolTip = section_names[c_value];
                }
                catch
                {
                }
                break;

            #endregion sectionid

            #region extended_per

            case "extended_per":
                if (origin == "quote")
                {
                    try
                    {
                        var value1 = grid.GetRowValues(e.VisibleIndex, "extTandM");
                        if (Math.Round(Convert.ToDouble(value1), 2) >
                            Math.Round(Toolbox.ReturnZeroIfNull_double(e.CellValue), 2) + Math.Round(Toolbox.ReturnZeroIfNull_double(e.CellValue), 2) * 0.05)
                        {
                            e.Cell.ForeColor = Color.Red;
                            e.Cell.ToolTip = "Quoted Price less than Benchmark Sell";
                        }
                        if (Math.Round(Convert.ToDouble(value1), 2) <
                            Math.Round(Toolbox.ReturnZeroIfNull_double(e.CellValue), 2) - Math.Round(Toolbox.ReturnZeroIfNull_double(e.CellValue), 2) * 0.05)
                        {
                            e.Cell.ForeColor = Color.DarkGreen;
                            e.Cell.ToolTip = "Quoted Price greater than Benchmark Sell";
                        }
                    }
                    catch
                    {
                    }
                }

                break;

            #endregion extended_per

            #region emptyval

            case "emptyval":
                try
                {
                    var billtype_id = 0;
                    if (grid.GetRowValues(e.VisibleIndex, "wo_detail_current_billtypeid") != null)
                        if (agv.GetRowValues(e.VisibleIndex, "active") != null &&
                            agv.GetRowValues(e.VisibleIndex, "active").ToString() == "0" && origin != "workorder" || part_no == 2139 ||
                            part_no == 777)
                        {
                            int.TryParse(grid.GetRowValues(e.VisibleIndex, "wo_detail_current_billtypeid").ToString(), out billtype_id);
                        }
                    if (agv.GetRowValues(e.VisibleIndex, "active") != null &&
                        agv.GetRowValues(e.VisibleIndex, "active").ToString() == "0" && origin != "workorder" || part_no == 777 || (part_no == 0 && !is_gl_account) ||
                        new List<int>(new[] { 3, 9, 11, 12 }).Contains(billtype_id))
                    {
                        var gv = (GridViewDataColumn)agv.Columns["emptyval"];
                        var editor = (TextBox)agv.FindRowCellTemplateControl(e.VisibleIndex, gv, "txtRecSelectQty");
                        if (editor != null)
                        {
                            bool skip = false;
                            var qtyOrder = grid.GetRowValues(e.VisibleIndex, "qty");
                            var complete = grid.GetRowValues(e.VisibleIndex, "active") == null ? "0" : agv.GetRowValues(e.VisibleIndex, "active").ToString();
                            if (qtyOrder != null && complete == "1")
                            {
                                var value = qtyOrder.ToString();
                                if (!string.IsNullOrEmpty(value))
                                {
                                    double v = 0;
                                    if (double.TryParse(value, out v))
                                    {
                                        if (v > 0)
                                        {
                                            skip = true;
                                        }
                                    }
                                }
                            }

                            if (!skip)
                            {
                                editor.Enabled = false;
                            }
                        }
                    }
                }
                catch
                {
                }
                break;

            #endregion emptyval

            #region wo_detail_current_billtypeid

            case "wo_detail_current_billtypeid":
                break;

            #endregion wo_detail_current_billtypeid

            #region Trans

            case "Trans":
                var commit_qty = Convert.ToDouble(agv.GetRowValues(e.VisibleIndex, "qtyrec"));
                e.Cell.ToolTip = "Transfer Part to Another Work Order";
                if (part_no >= 990000)
                {
                    e.Cell.Text = "";
                }
                else
                {
                    if (commit_qty <= 0 || part_no == 2139 || this_row["origin"].ToString().Contains("Expense"))
                    {
                        e.Cell.Text = "";
                        e.Cell.ToolTip = "Must have more than 0 Committed to Transfer";
                    }
                }
                break;

            #endregion Trans

            #region Required Date

            case "reqdate":
                if (origin == "workorder" && !new List<string>(new[]
                                                                    {
                                                                          OpsWOStatus.Invoiced,
                                                                          OpsWOStatus.WaitingToBeInvoiced,
                                                                          OpsWOStatus.WaitingForPO,
                                                                          OpsWOStatus.WaitingParentBMApproval,
                                                                          OpsWOStatus.WaitingBMApproval,
                                                                          OpsWOStatus.WaitingPMApproval
                                                                        }).Contains(wo_status_str) && column_chkbox.Visible)
                {
                    var this_datereq = Toolbox.ReturnNullDateTime(e.CellValue) == null
                        ? ""
                        : Convert.ToDateTime(e.CellValue).ToString("MM/dd/yy");
                    e.Cell.Text = string.Format(@"<b title=""{0}"">{0}</b>", this_datereq);
                }
                break;

            #endregion Required Date

            #region part_no

            case "part_no":
                //e.Cell.Style.Add("min-width", "100px");
                //e.Cell.Style.Add("max-width", "100px");
                e.Cell.CssClass += " part_no";
                try
                {
                    //e.Cell.Style.Add("text-align", "center");
                    if (origin == "quote")
                    {
                        #region kit

                        if (part_no >= 2000000 && e.DataColumn.FieldName == "part_no")
                        {
                            var _desc = (GridViewDataColumn)grid.Columns["description"];
                            var tooltipdiv = grid.FindRowCellTemplateControl(e.VisibleIndex, _desc, "tooltip") as HtmlContainerControl;

                            var this_image = grid.FindRowCellTemplateControl(e.VisibleIndex, _desc, "kitted_container") as Image;
                            if (this_image != null)
                            {
                                temp_comp = new NeBusinessUnit(current_user.business_unit_id);
                                this_image.Visible = true;
                                this_image.CssClass = "kitted_image";
                                var qty = Convert.ToInt32(grid.GetDataRow(e.VisibleIndex)["qty"]);
                                var sql = @"
SELECT 
inventory_kit_dtl_master_id master_id, 
FULL_PART_DESCRIPTION_WITH_LABOUR(inventory_kit_dtl_master_id,1,@v0) AS description, 
IF(inventory_kit_dtl_master_id < 990000,
	GETSELLPRICE(
		GET_CURRENT_COST(inventory_kit_dtl_master_id,@v1),
		PROC_GETINVSELLPRICE(inventory_kit_dtl_master_id,@v1),
		1,
		inventory_kit_dtl_qty,
        @v1),
	GETLABOURSELL(inventory_kit_dtl_master_id)
	) sell, 
inventory_kit_dtl_qty*@v3 qty, 
IF(inventory_kit_dtl_master_id < 990000,
	GETSELLPRICE(
		GET_CURRENT_COST(inventory_kit_dtl_master_id,@v1),
		PROC_GETINVSELLPRICE(inventory_kit_dtl_master_id,@v1),
		1,
		inventory_kit_dtl_qty,
        @v1),
	GETLABOURSELL(inventory_kit_dtl_master_id)
	) * inventory_kit_dtl_qty extTandM, 
IF(inventory_kit_dtl_master_id < 990000,
	GETSELLPRICE(
		GET_CURRENT_COST(inventory_kit_dtl_master_id,@v1),
		PROC_GETINVSELLPRICE(inventory_kit_dtl_master_id,@v1),
		1,
		inventory_kit_dtl_qty * @v3,
        @v1),
	GETLABOURSELL(inventory_kit_dtl_master_id)
	) * inventory_kit_dtl_qty * @v3 extd
FROM 
inventory_kit_dtl
WHERE 
inventory_kit_dtl_hdr_id = @v2
ORDER BY 
inventory_kit_dtl_id";
                                var _kit_contents = Toolbox.doSQL_dt(conn, sql,
                            new object[] { NeBusinessUnit.GetbuCountry(current_user.business_unit_id.ToString()), // {0}
								hidCompanyID.Value, // {1}
								part_no, // {2}
								qty // {3}
								}
                                    );
                                var _tip = new StringBuilder();
                                _tip.Append(@"
<table style='width:450px;' cellpadding='0' cellspacing='0'>
	<thead>
		<tr>
			<th width='50' align='center'>Part #</th>
			<th width='300'>Description</th>
			<th width='50' align='center'>Qty</th>
			<th width='50' align='center'>Bench</th>
			<th width='50' align='center'>Extd</th>
		</tr>
	</thead>
<tbody>");
                                foreach (DataRow _kit in _kit_contents.Rows)
                                {
                                    var _master_id = _kit["master_id"].ToString();
                                    var _description = Toolbox.do_value_from(_kit["description"]);
                                    var _sell = Convert.ToDouble(_kit["extTandM"]);
                                    var _qty = Convert.ToDouble(_kit["qty"]);
                                    var _extd = Convert.ToDouble(_kit["extd"]);
                                    _tip.AppendFormat(@"
	<tr>
		<td width='50' valign='top' align='center'>{0}</td>
		<td width='300' valign='top' style='white-space: normal'>{1}</td>
		<td width='50' valign='top' align='center'>{2}</td>
		<td width='50' valign='top' align='center'>{3}</td>
		<td width='50' valign='top' align='center'>{4}</td>
	</tr>", _master_id, _description, _qty, _sell.ToString("C2"), _extd.ToString("C2"));
                                }
                                _tip.Append(@"
</tbody>
</table>");
                                tooltipdiv.InnerHtml = _tip.ToString();
                            }
                        }

                        #endregion kit
                    }
                    if (part_no > 0 && part_no < 100000)
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
                            if (dr.Length > 0 && dr[0].ItemArray.Length > 0)
                            {
                                try
                                {
                                    var is_rental = Convert.ToBoolean(dr[0]["is_rental"]);
                                    if (is_rental)
                                    {
                                        part_is_exclude = "true";
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                        }
                        e.Cell.Text = "<a href=\"javascript:boing('../inventory/index.aspx?a=get&tab=G&id=" + part_no +
                                    "', 'inventory', 1280,768 )\" data-allowed_to_stock='" + part_allowed_to_stock + "' data-is_exclude='" +
                                    part_is_exclude + "' class='part_link'>" + part_no + "</a>";
                    }
                }
                catch (Exception ee)
                {
                    Toolbox.do_errorLog_errorStack(ee);
                }
                break;

            #endregion part_no

            #region qty_rec

            case "qtyrec":
                e.Cell.Text = "<span class='qtyrec'>" + Toolbox.ReturnBlankIfNull_string(e.CellValue) + "</span>";
                break;

            #endregion qty_avail

            #region qty_avail

            case "qty_avail":
                e.Cell.CssClass += " qty_avail";
                try
                {
                    workorder_column = (GridViewDataColumn)agv.Columns["workorder"];
                    location_combo = (ASPxComboBox)agv.FindRowCellTemplateControl(e.VisibleIndex, workorder_column, "combo_location");
                    var mid = Convert.ToInt32(agv.GetRowValues(e.VisibleIndex, "part_no"));
                    if (location_combo != null && location_combo.Visible && location_combo.SelectedIndex > -1)
                    {
                        // Get available qty from that location
                        var available_qty =
                            Toolbox.doSQL_double(conn, @"SELECT IFNULL(MIN(qty), 0) FROM inventory_location WHERE location_master_id = @v0  AND master_id = @v1  AND business_unit_id = @v2 ", new object[] { location_combo.Value, mid, WarehouseBusinessUnit.id });
                        e.Cell.Text = available_qty.ToString();
                    }
                    else
                    {
                        e.Cell.Text = "0";
                    }

                    //
                    // Key: 'part != 9595' excludes all 9595 parts, no matter which ones: subcontractor, expense....
                    //
                    if (mid < 990000 && mid != 2139 &&
                        mid != OpsSpecialPart.SubContractor &&
                        mid != OpsSpecialPart.CompanyCreditCardExpense &&
                        mid != OpsSpecialPart.ExpenseReimbursement &&
                        mid != OpsSpecialPart.NewChildWO && mid != 777
                        && location_combo != null && location_combo.Visible &&
                        location_combo.SelectedIndex > -1)
                    {
                        double n = 0;
                        double.TryParse(e.Cell.Text, out n);
                        e.Cell.ForeColor = Color.White;
                        e.Cell.BackColor = n > 0 ? Color.FromArgb(0, 128, 0) : Color.FromArgb(128, 0, 0);
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

            case "active":
                if (origin == "quote" || origin == "workorder")
                {
                    e.Cell.Text = grid.GetRowValues(e.VisibleIndex, "active") == null
                        ? ""
                        : grid.GetRowValues(e.VisibleIndex, "active").ToString();
                }
                break;

            #endregion active

            #region cost

            case "cost":

                if (origin == "quote")
                {
                    var cost_level = this_row == null || this_row["cost_level"] == null ? 0 : (int)this_row["cost_level"];
                    var bg = new Color();
                    var fg = new Color();
                    switch (cost_level)
                    {
                        case 0: // Non Existant
                            bg = ColorTranslator.FromHtml("#fff");
                            fg = ColorTranslator.FromHtml("#000");
                            break;
                        case 1: // Moving
                            bg = ColorTranslator.FromHtml("#090");
                            fg = ColorTranslator.FromHtml("#fff");
                            break;
                        case 2: // PO Cut to Inventory within 2 years
                            bg = ColorTranslator.FromHtml("#1D7373");
                            fg = ColorTranslator.FromHtml("#fff");
                            break;
                        case 3: // Price table for branch
                            bg = ColorTranslator.FromHtml("#099");
                            fg = ColorTranslator.FromHtml("#fff");
                            break;
                        case 4: // PO cut to work order
                            bg = ColorTranslator.FromHtml("#5ccccc");
                            fg = ColorTranslator.FromHtml("#007");
                            break;
                        case 5: // PO cut to Regional inventory
                            bg = ColorTranslator.FromHtml("#a66f00");
                            fg = ColorTranslator.FromHtml("#fff");
                            break;
                        case 6: // PO cut to Country Inventory
                            bg = ColorTranslator.FromHtml("#bf8f30");
                            fg = ColorTranslator.FromHtml("#fff");
                            break;
                        case 7: // Regional price table;
                            bg = ColorTranslator.FromHtml("#f96");
                            fg = ColorTranslator.FromHtml("#fff");
                            break;
                        case 8: // Country price table
                            bg = ColorTranslator.FromHtml("#bf3030");
                            fg = ColorTranslator.FromHtml("#fff");
                            break;
                        case 9: // PO cut to other country
                            bg = ColorTranslator.FromHtml("#a60000");
                            fg = ColorTranslator.FromHtml("#fff");
                            break;
                        case 10: // Other country price table
                            bg = ColorTranslator.FromHtml("#f00");
                            fg = ColorTranslator.FromHtml("#fff");
                            break;
                    }
                    e.Cell.BackColor = bg;
                    e.Cell.ForeColor = fg;
                    e.Cell.Style["font-weight"] = "bold";
                }
                if (part_no == 0 && cost == 0)
                {
                    e.Cell.Text = "";
                }
                break;

                #endregion cost
        }
        switch (e.DataColumn.Name)
        {
            #region commit_wo

            case "commit_wo":
                if (origin == "workorder")
                {
                    e.Cell.ToolTip = "Send this work order into WAITING FOR PARTS status...";
                    var cb = (CheckBox)grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "cb_commit");
                    if (cb != null)
                    {
                        cb.Visible = origin == "workorder" && part_no != 2139 && part_no < 990000 && !new List<string>(new[]
                                                                                                                            {
                                                                                                                                  OpsWOStatus.Invoiced ,
                                                                                                                                  OpsWOStatus.WaitingToBeInvoiced
                                                                                                                                }).Contains(wo_status_str) || origin == "purchaseorder";
                        if (cb.Visible && origin == "workorder")
                        {
                            // Add onclick event to the checkbox for blocking the scheduler.
                            cb.Attributes["onclick"] = "picklist.wo.block_scheduler(this, " + row_id + ");";
                            var current_checked_val = Convert.ToInt32(grid.GetRowValues(e.VisibleIndex, "emptyval"));
                            cb.Checked = current_checked_val == 1;
                        }
                    }
                }
                break;

            #endregion

            #region Actions

            case "Actions":
                e.Cell.Visible = false;
                break;

            #endregion

            #region select

            case "select":
                if (origin == "workorder")
                {
                    var part_origin = agv.GetDataRow(e.VisibleIndex)["origin"].ToString();
                    var cb1 = (CheckBox)grid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "chk_indiv");
                    if (cb1 != null)
                    {
                        cb1.Visible = part_no != 2139 && !(part_no >= 990000 && part_origin.Contains("Timesheet"));
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

        if (origin != "workorder")
        {
            e.Visible = DefaultBoolean.False;
        }
        else
        {
            switch (e.ButtonID)
            {
                case "delete_multiple":
                    var rec_qty = Convert.ToDouble(gv.GetRowValues(e.VisibleIndex, "qtyrec"));
                    var id_master = Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "part_no"));
                    if (rec_qty != 0 || id_master == 2139)
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
            }
        }
    }

    // Needed prep work so we aren't querying the database on each commandbutton being initialized.
    // This will create an object cache
    private Dictionary<int, wo_link> wo_obj;
    private readonly List<int> allowed_ids = new List<int>();

    private struct wo_link
    {
        public int po_line_id { get; set; }
        public string status { get; set; }
        public List<part_link> parts { get; set; }
    }

    private class part_link
    {
        public int master_id { get; set; }
        public string origin { get; set; }
    }

    private void populate_wo_statuses()
    {
        wo_obj = new Dictionary<int, wo_link>();
        var workorders = new List<int>();
        for (var i = 0; i < agv.VisibleRowCount; i++)
        {
            var wo_id = agv.GetDataRow(i) == null ? 0 : Convert.ToInt32(agv.GetDataRow(i)["workorder"]);
            if (wo_id > 200000 && !Toolbox.Contains(wo_id, new[] { 9999997, 9999998, 9999999 }))
            {
                if (!workorders.Contains(wo_id))
                {
                    workorders.Add(wo_id);
                }
            }
        }
        if (workorders.Count > 0)
        {
            var workorder_csv = string.Join(",", workorders);
            var dt =
                Toolbox.doSQL_dt(conn, string.Format(@"SELECT woprog_id id, woprog_status status FROM woprog WHERE woprog_id IN ({0})", workorder_csv), null);
            foreach (DataRow dr in dt.Rows)
            {
                var wo_id = Convert.ToInt32(dr["id"]);
                var status = dr["status"].ToString();
                if (!wo_obj.ContainsKey(wo_id))
                {
                    var link = new wo_link();
                    link.status = status;
                    var o_links = new List<part_link>();
                    var table_name = Toolbox.Contains(status, new[] { OpsWOStatus.WaitingToBeInvoiced, OpsWOStatus.Invoiced })
                        ? "wo_detail_history"
                        : "wo_detail_current";
                    var sub_dt =
                        Toolbox.doSQL_dt(conn, string.Format(@"SELECT {0}_master_id master_id, {0}_origin origin FROM {0}  WHERE {0}_woprog_id = @v0 ", table_name), new object[] { wo_id });
                    foreach (DataRow sub_dr in sub_dt.Rows)
                    {
                        var pl = new part_link();
                        pl.master_id = Convert.ToInt32(sub_dr["master_id"]);
                        pl.origin = sub_dr["origin"].ToString();
                        o_links.Add(pl);
                    }
                    link.parts = o_links;
                    wo_obj.Add(wo_id, link);
                }
            }
        }
    }

    private void control_allowed_rows(ASPxGridView gv, int _id, ASPxGridViewCommandButtonEventArgs e)
    {
        var woidObject = gv.GetRowValues(e.VisibleIndex, "workorder");
        int woidObjectId = 0;
        if (int.TryParse(woidObject.ToString(), out woidObjectId))
        {
            if (woidObjectId >= 1000000 && woidObjectId < 2000000 && this.processingWorkOrderId == woidObjectId)
            {
                if (NeWOProg.InHistoricStatus(this.processingWorkOrderStatus))
                {
                    e.Visible = false;
                    return;
                }
            }
        }

        e.Enabled = true;
        if (po_status == 4)
        {
            e.Image.ToolTip = "Cannot edit while the PO is waiting for invoice";
        }
        else if (!Toolbox.Contains(po_status, new[] { 8 })) // Don't allow cancelled PO's to be edited.
        {
            var _wo_id = gv.GetRowValues(e.VisibleIndex, "workorder");
            var _part_no = gv.GetRowValues(e.VisibleIndex, "part_no");
            var _rec_no = gv.GetRowValues(e.VisibleIndex, "wo_detail_current_rec_no");
            var qty_received = Convert.ToDouble(gv.GetRowValues(e.VisibleIndex, "qtyrec"));
            int part_no, wo_id = 0;
            if (_part_no != null)
            {
                int.TryParse(_part_no.ToString(), out part_no);
                if (_wo_id != null)
                {
                    int.TryParse(_wo_id.ToString(), out wo_id);
                    if (!Toolbox.Contains(wo_id, new[] { 9999997, 9999998, 9999999 }))
                    {
                        var status = wo_obj.ContainsKey(wo_id) ? wo_obj[wo_id].status : "";
                        var can_edit_after =
                            Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(a.allowed_to_edit_after_issue), 1) FROM inventory_tag a INNER JOIN inventory_item_master b ON b.tag_id = a.tag_id  WHERE b.master_id=@v0", new object[] { part_no }) == 1;
                        var wo_adv_status = Toolbox.Contains(status, new[] { OpsWOStatus.WaitingForPO, OpsWOStatus.WaitingToBeInvoiced, OpsWOStatus.Invoiced });
                        if (!can_edit_after && qty_received != 0)
                        {
                            if (wo_adv_status) // In a status that should not be edited
                            {
                                e.Enabled = false;
                                e.Image.ToolTip = "Line cannot be edited - The work order attached to this line is in the status: " + status;
                            }
                            else if (wo_obj.ContainsKey(wo_id) && wo_obj[wo_id].parts.Any(x => x.master_id == part_no))
                            // Must be a 1:1 link from PO to WO -- This block will test that
                            {
                                // Do multiple lines exist for this one part? (inventory excludes)
                                var multiple_test = wo_obj[wo_id].parts.Where(x => x.master_id == part_no);
                                var can_edit = true;
                                var can_edit_why = "";
                                var multiple_c = multiple_test.Count();
                                if (multiple_c == 1) // This indicates that there are multiple parts from this PO on the connected work order
                                {
                                    var p = multiple_test.SingleOrDefault();
                                    if (p.origin.Contains("\n")) // Multiple lines... need to test this
                                    {
                                        var origins = p.origin.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                                        // Origins are delimited by carriage returns
                                        foreach (var o in origins)
                                        {
                                            if (o.Trim() != "" && !o.Contains(po_n))
                                            // If this part has nothing on the work order linking (origin-wise) to this PO... don't allow edit
                                            {
                                                can_edit = false;
                                                can_edit_why = "Linked WO line did not come (only) from this PO";
                                            }
                                        }
                                    }
                                }
                                if (!can_edit)
                                {
                                    e.Enabled = false;
                                    e.Image.ToolTip = can_edit_why;
                                }
                                else
                                {
                                    allowed_ids.Add(_id); // For later use
                                }
                            }
                            else
                            {
                                allowed_ids.Add(_id); // For later use
                            }
                        }
                    }
                    else if (qty_received != 0)
                    {
                        e.Enabled = false;
                        e.Image.ToolTip =
                            "Line cannot be edited - This PO is in a advanced status, linked to stock and has a received quantity";
                    }
                    else
                    {
                        allowed_ids.Add(_id); // For later use
                    }
                }
                else
                {
                    allowed_ids.Add(_id); // For later use
                }
            }
            else
            {
                e.Enabled = false;
                e.Image.ToolTip = "Not a valid part number";
            }
        }
        else
        {
            e.Enabled = false;
            e.Image.ToolTip = "Line cannot be edited";
        }
    }

    protected void agv_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    {
        var gv = (ASPxGridView)sender;
        if (wo_obj == null && origin == "purchaseorder")
        {
            populate_wo_statuses();
        }
        if (e.ButtonType == ColumnCommandButtonType.Delete)
        {
            switch (origin)
            {
                case "purchaseorder":

                    if (!pnl_addnewline.Visible)
                    {
                        e.Visible = false;
                    }
                    if (Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "active")) == 0) // if the line is NOT active.. 
                    {
                        e.Visible = false;
                    }
                    if (poLineCompleteInfo != null && poLineCompleteInfo.Count() > 0)
                    {
                        var _id = Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "id"));

                        var was_received = from r in poLineCompleteInfo
                                           where r.poLineId == _id
                                           select r.received;
                        if (was_received.FirstOrDefault())
                        {
                            e.Visible = false;
                        }
                    }
                    break;
                case "quote":
                    e.Visible = pnl_addnewline.Visible;
                    break;
            }
        }
        if (e.ButtonType == ColumnCommandButtonType.Edit)
        {
            var _id = Convert.ToInt32(gv.GetRowValues(e.VisibleIndex, "id"));
            if (origin == "purchaseorder")
            {
                control_allowed_rows(gv, _id, e);
            }
            else if (origin == "quote")
            {
                e.Visible = pnl_addnewline.Visible;
            }
            else if (!member_can_edit_sell)
            {
                var grid = (ASPxGridView)sender;
                var converted_part = 0;
                if (grid.GetRowValues(e.VisibleIndex, "part_no") != null)
                {
                    int.TryParse(grid.GetRowValues(e.VisibleIndex, "part_no").ToString(), out converted_part);
                    if (converted_part >= 900000)
                    {
                        e.Visible = false;
                    }
                }
                else
                {
                    e.Visible = false;
                }
            }
        }
    }

    #endregion

    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            var context = HttpContext.Current;
            if ((string.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                string.IsNullOrEmpty(windowFeatures))
            {
                context.Response.Redirect(url, true);
            }
            else
            {
                var page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);
                string script;
                if (!string.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }
                script = string.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
    }

    /// <summary>
    ///     Populates the SQLDataSource that is used for the workorder drop down in the add line.
    /// </summary>
    /// <param name="business_unit_id"></param>
    /// <param name="div_stock"></param>
    private DateTime ts_update;

    protected void populate_sds_workorder_ddl(object business_unit_id, object div_stock)
    {
        switch (ddlMatType.SelectedValue)
        {
            case "8":
                sds_add_workorder_ddl.SelectCommand = string.Format(@"
SELECT
	id,
	CONCAT(id,' - ',name) name
FROM
	rfq_header
WHERE
	status = 'Closed' AND
	business_unit_id = {0} ORDER BY date_close DESC", hidCompanyID.Value);
                ddlWorkOrder.ValueField = "id";
                ddlWorkOrder.TextField = "name";
                break;
            default:
                var _populate = true;
                // this_origin == "purchaseorder" && new NePOProg(main_id).poprog_shipping_method != 2 ? true : false;
                div_stock = _populate ? div_stock : "";
                sds_add_workorder_ddl.SelectCommand = string.Format(@"
{1}
	(
	SELECT 
		woprog_id, 
		CONCAT(TRIM(LEADING '0' FROM woprog_bvwo),' - ', woprog_customername, ' ', LEFT(woprog_description,40), ' - ', ddl_name, ' - ',woprog_status) AS workorder 
            
	FROM 
		woprog, 
		business_unit 
	WHERE 
		business_unit_id = id AND 
		business_unit_id = {0} AND 
		woprog_status NOT IN ('Invoiced', 'Waiting To Be Invoiced', 'Waiting For PO', 'Waiting BM Approval', 'Waiting PM Approval', 'Waiting Approval','Deleted','Waiting Parent BM Approval') AND 
		woprog_bvwo != 'Not Entered' AND 
		woprog_associate_woprog_id = 0 AND 
		woprog_hold = 0 AND
		woprog_isrebill = 0 AND 
		woprog_iscredit = 0
	ORDER BY 
		business_unit_id, 
		woprog_customername
	)", business_unit_id, div_stock);
                ddlWorkOrder.ValueField = "woprog_id";
                ddlWorkOrder.TextField = "workorder";
                break;
        }
    }

    protected void agv_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
    {
        if (origin == "quote")
        {
            FillforQuote(main_id, main_rev);
        }

    }

    protected void agv_CancelRowEditing(object sender, ASPxStartRowEditingEventArgs e)
    {
        if (origin == "quote")
        {
            //	    Session["quote_ws_refresh"] = null;
            FillforQuote(main_id, main_rev);
        }
        else if (origin == "rfq")
        {
            agv.Settings.ShowPreview = false;
        }
    }

    protected void ASPxPopupSectionAdd_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(rev))
        {
            lst_addsections_section.DataSource =
                Toolbox.doSQL_dt(conn, @"SELECT a.id, a.section, a.detail_id, (SELECT COUNT(*) FROM quote_worksheet b WHERE b.section_id = a.id) dependants FROM quote_section a WHERE a.quote_id = @v0  AND a.revision = @v1  AND LENGTH(TRIM(a.section)) > 0 ORDER BY section", new object[] { main_id, rev });
            lst_addsections_section.DataBind();
            ASPxPopupSectionAdd.PopupVerticalAlign = PopupVerticalAlign.WindowCenter;
            /*
			lst_addsections_section.Height = Unit.Pixel(((lst_addsections_section.Items.Count) * 21)+27);
			ASPxPopupSectionAdd.Height = Unit.Pixel((lst_addsections_section.Items.Count) * 21 + 200);
			if (lst_addsections_section.Height.Value > Unit.Pixel(320).Value)
			{
				lst_addsections_section.Height = Unit.Pixel(320);
				ASPxPopupSectionAdd.Height = Unit.Pixel(420);
			}
			*/
        }
    }

    #region Handler Code

    private readonly bool handler_use_current_cost = false;
    private bool handler_use_current_cost_set;

    protected string note_handler(object container)
    {
        var _q = Request.QueryString;
        //	Toolbox _tools = new Toolbox();
        var origin = _q["origin"] ?? "";
        var c = container as GridViewDataItemTemplateContainer;
        var label_text = "";
        var row_id = c != null ? c.KeyValue.ToString() : "";
        var _c = 0;
        var _n = "";
        if (!string.IsNullOrEmpty(row_id) && c != null)
        {
            switch (origin)
            {
                case "purchaseorder":
                    _c = _handlers.Tables["po_detail"].Select("id = " + row_id + " AND notes <> ''").Length;
                    _n = _c > 0 ? _handlers.Tables["po_detail"].Select("id = " + row_id)[0]["notes"].ToString() : "";
                    break;
                case "workorder":
                    try
                    {
                        _c = _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id + " AND notes <> ''").Length;
                    }
                    catch
                    {
                        _c = 0;
                    }
                    _n = _c > 0 ? _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id)[0]["notes"].ToString() : "";
                    break;
                case "quote":
                    var use_current_cost = handler_use_current_cost_set
                        ? handler_use_current_cost
                        : Toolbox.doSQL_string(conn, @"SELECT use_current_cost FROM quote_master  WHERE quote_id =@v0 and revision=@v1 ", new object[] { _q["id"], _q["rev"] }) == "True";
                    if (!handler_use_current_cost_set)
                    {
                        handler_use_current_cost_set = true;
                    }
                    var used_table = use_current_cost ? "quote_worksheet_current_cost" : "quote_worksheet";
                    _c = _handlers.Tables[used_table].Select("id = " + row_id + " AND notes <> ''").Length;
                    _n = _c > 0 ? _handlers.Tables[used_table].Select("id = " + row_id)[0]["notes"].ToString() : "";
                    break;
                case "groupings":
                    _c =
                        Toolbox.doSQL_int(conn, @"SELECT IFNULL(LENGTH(inventory_group_dtl_notes),0) FROM inventory_group_dtl where id =@v0 ", new object[] { row_id });
                    _n = _c > 0
                        ? Toolbox.doSQL_string(conn, @"SELECT inventory_group_dtl_notes FROM inventory_group_dtl  where id =@v0", new object[] { row_id })
                        : "";
                    break;
                case "alternates":
                    _c =
                        Toolbox.doSQL_int(conn, @"SELECT IFNULL(LENGTH(inventory_alternate_notes),0)FROM inventory_alternate where inventory_alternate_id =@v0 ", new object[] { row_id });
                    _n = _c > 0
                        ? Toolbox.doSQL_string(conn, @"SELECT inventory_alternate_notes FROM inventory_alternate where inventory_alternate_id =@v0 ", new object[] { row_id })
                        : "";
                    break;
                case "kitted":
                    _c =
                        Toolbox.doSQL_int(conn, @"SELECT IFNULL(LENGTH(inventory_kit_dtl_notes),0) FROM inventory_kit_dtl where inventory_kit_dtl_id =@v0 ", new object[] { row_id });
                    _n = _c > 0
                        ? Toolbox.doSQL_string(conn, @"SELECT inventory_kit_dtl_notes FROM inventory_kit_dtl where inventory_kit_dtl_id =@v0 ", new object[] { row_id })
                        : "";
                    break;
                case "rfq":
                    _c = Toolbox.doSQL_int(conn, @"SELECT IFNULL(LENGTH(note),0) FROM rfq_part_list where id =@v0 ", new object[] { row_id });
                    _n = _c > 0
                        ? Toolbox.do_value_from(
                            Toolbox.doSQL_string(conn, @"SELECT note FROM rfq_part_list where id =@v0  LIMIT 1", new object[] { row_id }), true)
                        : "";
                    break;
            }
        }
        var div = _c > 0
            ? string.Format(@" class='opt' data-tooltip=""{0}"" data-width='450' data-title='Notes'", Toolbox.do_value_from(_n))
            : "";
        label_text = _c > 0
            ? "<img src='/images/fullnotes.jpg' border='0' " + div + "/>"
            : "<img src='/images/icon/icon[note_blank].gif' border='0' Width='14' Height='18'/>";
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
        var div = "";
        var _c = 0;
        if (row_id != "" && row_id != null)
        {
            switch (origin)
            {
                case "workorder":
                    try
                    {
                        _c = _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id + " AND issues <> ''").Length;
                        issue = _c > 0
                            ? _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id)[0]["issues"].ToString()
                                .Replace("\n", "<br/>")
                            : "";
                    }
                    catch
                    {
                    }
                    break;
            }
        }
        div = string.Format(@" class='opt' data-tooltip=""{0}"" data-width='500' data-title='Issues'", issue);
        label_text = issue != ""
            ? "<img src='/images/icon/icon[attention].gif' width='16' height='16' border='0' " + div + "/>"
            : "<img src='/images/icon/icon[approve].gif' width='16' height='16' border='0'/>";
        return label_text;
    }

    private DataTable po_dt;

    protected string po_stock_wo_track(object container)
    {
        var label_text = "";
        var _q = Request.QueryString;
        var _tools = new Toolbox();
        var origin = _q["origin"] ?? "";
        var c = container as GridViewDataItemTemplateContainer;
        var row_id = c.KeyValue.ToString();
        var div = "";
        var WorkOrders = new DataRow[1];
        var tooltip = "";
        switch (origin)
        {
            case "purchaseorder":
                if (po_dt == null)
                {
                    po_dt = Toolbox.doSQL_dt(conn, @" SELECT a.po_details_id, c.poprog_id, b.wo_detail_current_bvwo, d.woprog_customername customer, a.po_details_part_no, 
b.wo_detail_current_qty_ordered, b.wo_detail_current_qty_committed 
FROM po_details_current a 
LEFT JOIN poprog_header c ON a.po_details_poprog_id = c.poprog_id 
LEFT JOIN wo_detail_current b ON a.po_details_part_no = b.wo_detail_current_master_id AND b.wo_detail_current_qty_committed < b.wo_detail_current_qty_ordered AND b.business_unit_id = c.business_unit_id 
LEFT JOIN woprog d ON b.wo_detail_current_woprog_id = d.woprog_id  
WHERE (a.po_details_woprog_id IN (9999999,9999998,9999997)||(a.is_gl_account=true)) AND a.po_details_woprog_id IS NOT NULL AND b.wo_detail_current_bvwo IS NOT NULL 
AND a.po_details_poprog_id =@v0", new object[] { _q["id"] });
                    po_dt.TableName = "po_details_current";
                }
                WorkOrders = po_dt.Select("po_details_id = " + row_id);
                break;
        }
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
                tooltip =
                    string.Format(
                        @"<table width='100%'><thead><tr><th>WO</th><th>Customer</th><th>Req</th><th>Comm</th></thead></tr><tbody>{0}</tbody></table>",
                        tooltip);
            }
            div = string.Format(@" class='opt' data-tooltip=""{0}"" data-width='500' data-title='Requiring This Part'", tooltip);
        }
        label_text = tooltip != ""
            ? "<img src='/images/icon/icon[details].gif' width='16' height='16' border='0' " + div + " />"
            : "";
        return label_text;
    }

    private DataTable noteinactive_dt = new DataTable();

    protected string noteinactive_handler(object container)
    {
        var _q = Request.QueryString;
        //		Toolbox _tools = new Toolbox();
        var origin = _q["origin"] ?? "";
        var c = container as GridViewDataItemTemplateContainer;
        var label_text = "";
        var row_id = c.KeyValue.ToString();
        var _c = 0;
        if (row_id != "" && row_id != null)
        {
            switch (origin)
            {
                case "purchaseorder":
                    if (noteinactive_dt.Rows.Count == 0)
                    {
                        noteinactive_dt.TableName = "po_details_current";
                        noteinactive_dt =
                            Toolbox.doSQL_dt(conn, @"SELECT po_details_id, po_details_line_active FROM po_details_current  where po_details_poprog_id =@v0", new object[] { _q["id"] });
                    }
                    _c = row_id == ""
                        ? 0
                        : Convert.ToInt32(noteinactive_dt.Select("po_details_id =" + row_id)[0]["po_details_line_active"]);

                    // [1075] [Nesi UAT] We need to control the visibility of the "All Items Complete" button on a PO a bit better.
                    // Save the ID in browser, so we can relocate the element faster.
                    label_text = _c == 0
                        ? string.Format("<img src='/images/icon/icon[ok_notyet].gif' width='16' height='16' border='0' id = '{0}' />", row_id)
                        : string.Format("<img src='/images/icon/icon[ok].gif' width='16' height='16' border='0' id = '{0}' />", row_id);

                    break;
            }
        }
        //          label_text = _c == 0 ? "<asp:ImageButton ID='imgbtnPOLineActive" + row_id + "' runat='server' Height='16px' ImageUrl='/images/icon/icon[ok].gif' Width='16px' OnClick='imgbtnPOLineActive_Click'/>" : "<asp:ImageButton ID='imgbtnPOLineActive" + row_id + "' runat='server' Height='16px' ImageUrl='/images/icon/icon[ok_notyet].gif' Width='16pcx' OnClick='imgbtnPOLineActive_Click'/>";

        return label_text;
    }

    protected string picture_handler(object container)
    {
        var label_text = "";

        if (origin != "workorder" && origin != "quote")
        {
            var _tools = new Toolbox();
            var c = container as GridViewDataItemTemplateContainer;
            var master_id = "";
            try
            {
                master_id = DataBinder.Eval(c.DataItem, "part_no").ToString();
            }
            catch
            {
                master_id = "";
            }
            var _pic_no = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_picture WHERE master_id = @v0 ", new object[] { master_id });
            label_text = _pic_no > 0
                ? "<img src='/images/fullpicture.jpg' width='14' height='18' border='0'/>"
                : "<img src='/images/emptypicture.jpg' width='14' height='18' border='0'/>";
        }
        else if (origin == "quote")
        {
            var _tools = new Toolbox();
            var c = container as GridViewDataItemTemplateContainer;
            var master_id = 0;
            var _id = 0;
            var is_stock = "";
            try
            {
                master_id = Convert.ToInt32(DataBinder.Eval(c.DataItem, "part_no") == "" ? 0 : DataBinder.Eval(c.DataItem, "part_no"));
                _id = Convert.ToInt32(DataBinder.Eval(c.DataItem, "id"));
                is_stock = DataBinder.Eval(c.DataItem, "allowed_to_stock").ToString();
                if (is_stock == "1")
                {
                    var _pic_no = Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_picture WHERE master_id = @v0 ", new object[] { master_id });
                    label_text = _pic_no > 0
                        ? "<img src='/images/fullpicture.jpg' width='14' height='18' border='0'/>"
                        : "<img src='/images/emptypicture.jpg' width='14' height='18' border='0'/>";
                }
                else
                {
                    if (master_id < 990000)
                    {
                        var has_file = Toolbox.doSQL_int(conn, @"SELECT IF(IFNULL(MAX(has_file), 0) = 1, 1, 0) FROM quote_worksheet WHERE id = @v0 ", new object[] { _id });

                        label_text = has_file == 1 ? "<img src='/images/icon/icon[file-full].gif' width='14' height='16' border='0'/>" : "<img src='/images/icon/icon[file].gif' width='14' height='16' border='0'/>";
                    }
                    else
                    {
                        label_text = "";
                    }
                }
            }
            catch
            {
                master_id = 0;
            }




        }
        return label_text;
    }

    private DataTable dt_history;

    protected string history_handler(object container)
    {
        //StackTrace st = new StackTrace ();
        //StackFrame sf = st.GetFrame (0);
        //MethodBase currentMethodName = sf.GetMethod();
        //Debug.Write(currentMethodName.Name+" "+DateTime.Now.ToString("U")+" Start\n");
        var _q = Request.QueryString;
        //		Toolbox _tools = new Toolbox();
        var origin = _q["origin"] ?? "";
        var c = container as GridViewDataItemTemplateContainer;
        var label_text = "";
        var row_id = c.KeyValue.ToString();
        var div = "";
        var sb = new StringBuilder();
        if (dt_history == null && main_id != 0)
        {
            dt_history = Toolbox.doSQL_dt(conn, string.Format(@"SELECT wo_detail_{0}_id id, wo_detail_{0}_rec_no rec_no, wo_detail_{0}_master_id master_id, wo_detail_{0}_woprog_id woprog_id FROM wo_detail_{0}
WHERE wo_detail_{0}_woprog_id = @v0 ", tablename), new object[] { main_id });

        }
        if (row_id != "" && row_id != null && dt_history != null && dt_history.Rows.Count > 0)
        {
            switch (origin)
            {
                case "workorder":
                    try
                    {
                        var this_dr = dt_history.Select("id = " + row_id);
                        if (this_dr != null && this_dr.Length > 0)
                        {
                            var recno = this_dr[0]["rec_no"].ToString();
                            var histpartno = this_dr[0]["master_id"].ToString();
                            var histwoid = this_dr[0]["woprog_id"].ToString();
                            // The old procedure is the direct call to wo_line_changes
                            //DataTable _history_dt = Toolbox.doSQL_dt(conn,@"CALL wo_line_changes(@v0 , @v1 )", new object[] {  histwoid, recno } );
                            if (_handlers.Tables["history"] != null && _handlers.Tables["history"].Select("rec_no =" + recno).Count() > 0)
                            {
                                var _history_dt = _handlers.Tables["history"].Select("rec_no =" + recno).CopyToDataTable();
                                if (_history_dt.Rows.Count > 0)
                                {
                                    sb.Append("<table cellspacing='0' cellpadding='3'>");
                                    foreach (DataRow _history_dr in _history_dt.Rows)
                                    {
                                        var _dt = _history_dr["dt"].ToString();
                                        var _the_change = HttpUtility.HtmlEncode(_history_dr["the_change"].ToString());
                                        sb.AppendFormat(
                                            @"<tr><td valign='top' width='125'><strong>{0}</strong></td><td valign='top' style='text-align:left'>{1}</td></tr>",
                                            _dt, _the_change);
                                    }
                                    sb.Append("</table>");
                                }
                                else
                                {
                                    sb.Append("");
                                }
                                if (sb.ToString() != "")
                                {
                                    div = string.Format(@" class='opt' data-tooltip=""{0}"" data-width='500' data-title='History'", sb);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    break;
            }
        }
        label_text = sb.ToString() != ""
            ? "<img src='/images/icon/icon[book_mark].png' width='16' height='16' border='0'" + div + " />"
            : "<img src='/images/icon/icon[book_empty].png' width='16' height='16' border='0'/>";
        //Debug.Write(currentMethodName.Name+" "+DateTime.Now.ToString("U")+" End\n");
        return label_text;
    }

    protected string origin_handler(object container)
    {
        var _q = Request.QueryString;
        //	Toolbox _tools = new Toolbox();
        var origin = _q["origin"] ?? "";
        var c = container as GridViewDataItemTemplateContainer;
        var label_text = "";
        var row_id = c.KeyValue.ToString();
        var _woprog_id = "";
        var div = "";
        var _c = 0;
        var _note = "";
        var _partno = "";
        if (row_id != "" && row_id != null)
        {
            switch (origin)
            {
                case "workorder":
                    try
                    {
                        _c = _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id + " AND origin <> ''").Length;
                        _note = _c > 0
                            ? _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id)[0]["origin"].ToString()
                                .Replace("\n", "<br/>")
                            : "";
                        _partno = _c > 0
                            ? _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id)[0]["master_id"].ToString()
                            : "";
                        _woprog_id = _c > 0
                            ? _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id)[0]["woprog_id"].ToString()
                            : "";
                    }
                    catch
                    {
                    }
                    break;
            }
        }
        if (origin == "workorder")
        {
            if (_note != "")
            {
                div = string.Format(@" class='opt' data-tooltip=""{0}"" data-width='500' data-title='Origin'", _note);
                if (_note == "Manually Added")
                {
                    label_text = "<img src='/images/icon/icon[browse].gif' width='16' height='16' border='0' " + div + " />";
                }
                else if (_note == "Scanner Added")
                {
                    label_text = "<img src='/images/icon/icon[print_barcode].GIF' width='16' height='16' border='0'" + div + " />";
                }
                else if (_note == "Entered From Timesheet")
                {
                    label_text = "<img src='/images/icon/icon[calendar].gif' width='16' height='16' border='0'" + div + " />";
                }
                else if (_note.Contains("PO"))
                {
                    var poprog_id = 0;

                    // SubContractor            = 55555;
                    if (_partno == OpsSpecialPart.SubContractor.ToString())
                    {
                        // check to see if a PO references this work order
                        var _po_c =
                            Toolbox.doSQL_int(conn, @"SELECT COUNT(DISTINCT(po_details_poprog_id)) FROM po_details_current WHERE po_details_woprog_id = @v0  AND po_details_part_no = '55555'", new object[] { _woprog_id });
                        if (_po_c == 1)
                        {
                            poprog_id =
                                Toolbox.doSQL_int(conn, @"SELECT po_details_poprog_id FROM po_details_current WHERE po_details_woprog_id = @v0  AND po_details_part_no = '55555'", new object[] { _woprog_id });
                        }
                    }
                    label_text = poprog_id > 1
                        ? string.Format(
                            @"<a href=""javascript:boing('/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={1}', 'po', 1280,800)""><img src='/images/icon/icon[shipping].gif' width='16' height='16' border='0'{0} /></a>",
                            div, poprog_id)
                        : "<img src='/images/icon/icon[shipping].gif' width='16' height='16' border='0'" + div + " />";
                }
                else if (_note.Contains("Transferred"))
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
        }
        //label_text = _c > 0 ? "<img src='/images/fullnotes.jpg' border='0'/>" : "<img src='/images/emptynotes.jpg' border='0'/>";
        return label_text;
    }

    protected string tax_handler(object container)
    {
        var c = container as GridViewDataItemTemplateContainer;
        var _q = Request.QueryString;
        //	Toolbox _tools = new Toolbox();
        var origin = _q["origin"] ?? "";
        var label_text = "";
        var tax_info = "";
        var div = "";
        var row_id = c.KeyValue.ToString();
        var _c = 0;
        DataRow _taxrow = null;
        if (row_id != "" && row_id != null)
        {
            switch (origin)
            {
                case "workorder":
                    try
                    {
                        _c = _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id).Length;
                        _taxrow = _c > 0 ? _handlers.Tables["wo_detail_" + tablename].Select("id = " + row_id)[0] : null;
                    }
                    catch
                    {
                    }
                    break;
            }
        }
        if (_taxrow != null)
        {
            if ((int)_taxrow["tax1"] != 0)
            {
                tax_info += string.Format("Tax 1 - {0}<br/>", _taxrow["tax1_name"]);
            }
            if ((int)_taxrow["tax2"] != 0)
            {
                tax_info += string.Format("Tax 2 - {0}<br/>", _taxrow["tax2_name"]);
            }
            if ((int)_taxrow["tax3"] != 0)
            {
                tax_info += string.Format("Tax 3 - {0}<br/>", _taxrow["tax3_name"]);
            }
            if ((int)_taxrow["tax4"] != 0)
            {
                tax_info += string.Format("Tax 4 - {0}<br/>", _taxrow["tax4_name"]);
            }
            tax_info = tax_info.TrimEnd(',');
            tax_info = tax_info != "" ? tax_info : "No Taxes Set";
            div = string.Format(@" class='opt' data-tooltip=""{0}"" data-width='500' data-title='Set Taxes'", tax_info);
            label_text = "<img src='/images/icon/icon[attribute_values].gif' width='16' height='16' " + div + " border='0' />";
        }
        return label_text;
    }

    #endregion

    protected void ddlMemberType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMatType.SelectedValue == "2") // if mat type is labour
        {
            Fill_Labour_Totals();
            ScriptManager1.SetFocus(ddlLabourChargeType);
        }
        else if (ddlMatType.SelectedValue == "5") // if mat type is quotes...
        {
            if (ddlMemberType.SelectedIndex > 0)
            {
                Populate_OtherQuoteSelectDDL(Convert.ToInt32(ddlMemberType.Value));
            }
        }

        else if (ddlMatType.SelectedValue == "6") // if mat type is work orders..
        {
            if (ddlMemberType.SelectedIndex > 0)
            {
                Populate_OtherWorkOrderSelectDDL(Convert.ToInt32(ddlMemberType.Value));
            }
        }
        else if (ddlMatType.SelectedValue == "7") // if mat type is purchase orders..)
        {
            Populate_OtherPurchaseOrderSelectDDL(Convert.ToInt32(ddlMemberType.Value));
        }
    }

    protected void ddlLabourChargeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fill_Labour_Totals();
        ScriptManager1.SetFocus(TextQty);
    }

    protected void Fill_kitted_Totals()
    {
        try
        {
            var sellprice = "";
            try
            {
                sellprice =
                    Toolbox.doSQL_string(conn, @"Select IFNULL(GetKittedSell(@v0,@v1,@v2), 0)", new object[] { TextQty.Text, hidCompanyID.Value, ddlKittedParts.Value });
            }
            catch
            {
                sellprice = "0";
            }
            TextPartNo.Text = ddlKittedParts.Value.ToString();
            TextSell.Text = sellprice;

            TextTMExtd.Text = Convert.ToString(Convert.ToDouble(TextSell.Text) * Toolbox.do_ConvertToDouble(TextQty.Text));
            TextQuotedExtd.Text = Convert.ToString(Convert.ToDouble(TextSell.Text) * Toolbox.do_ConvertToDouble(TextQty.Text));
        }
        catch
        {
        }
    }

    protected void Fill_Labour_Totals()
    {
        try
        {
            var paytypeid = ddlLabourChargeType.Value.ToString();
            var GetPart = "";
            try
            {
                if (ddlMemberType.Visible)
                {
                    var membertypeid = ddlMemberType.Value;
                    GetPart =
                        Toolbox.doSQL_string(conn, @"SELECT IFNULL(MAX(id), 1000000) from membertype_chargeout WHERE membertype_id = @v0  AND business_unit_id = @v1  AND paytype_id = @v2 ", new object[] { membertypeid, hidCompanyID.Value, paytypeid });
                }
                else
                {
                    if (ddlMemberName.Value != null)
                    {
                        var memberid = ddlMemberName.Value.ToString();
                        var addmember = new NeMember(Convert.ToInt32(memberid));
                        GetPart =
                            Toolbox.doSQL_string(conn, @"SELECT id from membertype_chargeout WHERE membertype_id = @v0  AND business_unit_id = @v1  AND paytype_id = @v2 ", new object[] { addmember.MemberTypeID, addmember.business_unit, paytypeid });
                    }
                }
            }
            catch
            {
                GetPart = "1000000";
            }
            var customer_id = 0;
            switch (origin)
            {
                case "quote":
                    var qu = new quote(main_id);
                    customer_id = qu.cust_id;
                    break;
                case "workorder":
                    var wo = new NeWOProg(main_id);
                    customer_id = wo.WOProg_Customer_ID;
                    break;
            }
            var ChargeOut = "";
            try
            {
                if (GetPart != "" && GetPart != "1000000")
                {
                    ChargeOut = Toolbox.doSQL_string(conn, @"CALL CUSTOMER_CHARGEOUT(@v0 , @v1 )", new object[] { customer_id, GetPart });
                }
                else
                {
                    ChargeOut = "0";
                }
            }
            catch
            {
                ChargeOut = "0";
            }
            TextPartNo.Text = GetPart;
            TextSell.Text = ChargeOut;
            TextTMExtd.Text = Convert.ToString(Convert.ToDouble(TextSell.Text) * Toolbox.do_ConvertToDouble(TextQty.Text));
            TextQuotedExtd.Text = Convert.ToString(Convert.ToDouble(TextSell.Text) * Toolbox.do_ConvertToDouble(TextQty.Text));
        }
        catch (Exception ee)
        {

        }
    }

    protected void btnSaveHeader_Click(object sender, EventArgs e)
    {
        if (origin == "kitted")
        {
            if (txtGroupName.Text != "")
            {
                try
                {
                    if (txtGroupName.Text.Length <= 70)
                    {
                        lblerrorLabel.Visible = false;
                        if (main_id == 0)
                        {
                            main_id = Toolbox.doSQL_return_id(conn, @"INSERT INTO inventory_kit_hdr (inventory_kit_hdr_name, inventory_kit_hdr_created_by, inventory_kit_hdr_created_dt, inventory_kit_hdr_edited_dt,inventory_kit_hdr_note) values (@v0 ,@v1 ,now(),now(),@v2 )", new object[] { txtGroupName.Text, current_user.id, txtGroupNotes.Text });
                        }
                        else
                        {
                            Toolbox.doSQL_void(conn, @"UPDATE inventory_kit_hdr set inventory_kit_hdr_name = @v0 , inventory_kit_hdr_edited_dt = now(),inventory_kit_hdr_note = @v1  WHERE inventory_kit_hdr_id=@v2 ", new object[] { txtGroupName.Text, txtGroupNotes.Text, main_id });
                        }
                        txtGroupName.Text =
                            Toolbox.doSQL_string(conn, @"SELECT IFNULL(inventory_kit_hdr_name, 'Not Set') FROM inventory_kit_hdr  WHERE inventory_kit_hdr_id=@v0", new object[] { main_id });
                        txtGroupNotes.Text =
                            Toolbox.doSQL_string(conn, @"SELECT IFNULL(inventory_kit_hdr_note, '') FROM inventory_kit_hdr  WHERE inventory_kit_hdr_id=@v0", new object[] { main_id });
                        pnl_addnewline.Visible = true;
                        pnl_gv.Visible = true;
                        header_label.Text = "Kitted Name: (" + main_id + ") " + txtGroupName.Text;
                        //string javaScript = "window.opener.location.reload();";
                        // ScriptManager.RegisterStartupScript(this, this.GetType(), "Update", javaScript, true);
                        Response.Redirect("pikclist.aspx?origin=kitted&id=" + main_id);
                    }
                    else
                    {
                        lblerrorLabel.Text = "The Name can only be 70 characters long";
                        lblerrorLabel.Visible = true;
                    }
                }
                catch
                {
                }
            }
        }
        else if (origin == "groupings")
        {
            if (txtGroupName.Text != "")
            {
                try
                {
                    var is_new = false;
                    if (main_id == 0)
                    {
                        main_id =
                            Convert.ToInt32(
                                Toolbox.doSQL_return_id(conn, @"Insert into inventory_group_hdr (name, member_id, created_dt, edited_dt,notes) values (@v0 ,@v1 ,now(),now(),@v2 )", new object[] { txtGroupName.Text, current_user.id, txtGroupNotes.Text }));
                        is_new = true;
                    }
                    else
                    {
                        Toolbox.doSQL_void(conn, @"UPDATE inventory_group_hdr SET name = @v0 , edited_dt = now(), notes = @v1  WHERE id=@v2 ", new object[] { txtGroupName.Text, txtGroupNotes.Text, main_id });
                    }
                    if (is_new)
                    {
                        Response.Redirect("~/sections/member/picklist/pikclist.aspx?origin=groupings&id=" + main_id, true);
                    }
                    else
                    {
                        txtGroupName.Text =
                            Toolbox.doSQL_string(conn, @"SELECT IFNULL(name, 'Not Set') FROM inventory_group_hdr  WHERE id=@v0", new object[] { main_id });
                        txtGroupNotes.Text =
                            Toolbox.doSQL_string(conn, @"SELECT IFNULL(notes, '') FROM inventory_group_hdr  WHERE id=@v0", new object[] { main_id });
                        pnl_addnewline.Visible = true;
                        pnl_gv.Visible = true;
                        header_label.Text = "Group Name: (" + main_id + ") " + txtGroupName.Text;
                    }
                    //  string javaScript = "window.opener.location.reload();";
                    //  ScriptManager.RegisterStartupScript(this, this.GetType(), "Update", javaScript, true);
                }
                catch
                {
                }
            }
        }
    }

    protected void dupe_check()
    {
        var possible_dupes = new DataTable();
        switch (origin)
        {
            case "quote":
                possible_dupes = Toolbox.doSQL_dt(conn, @" SELECT part_no, SUM(c) c FROM ( SELECT part_no, COUNT(DISTINCT sell) c, sell FROM quote_worksheet aa LEFT JOIN inventory_item_master bb ON CAST(aa.part_no as UNSIGNED) = bb.master_id LEFT JOIN inventory_tag cc ON bb.tag_id = cc.tag_id WHERE aa.part_no != '' AND CAST(aa.part_no AS UNSIGNED) > 0 AND CAST(aa.part_no AS UNSIGNED) < 990000 AND cc.is_exclude = false AND aa.quote_id = @v0  AND aa.revision = @v1  GROUP BY part_no,sell ) a GROUP BY part_no HAVING c > 1", new object[] { id, rev });
                break;
            case "workorder":
                //	possible_dupes	= Toolbox.doSQL_dt(conn,@"SELECT sum(wo_detail_current_qty_committed) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] {  id, master_id } );
                break;
        }
        if (possible_dupes.Rows.Count > 0)
        {
            div_refactor.Style["display"] = "block";
        }
    }

    protected void ddlKittedParts_SelectedIndexChanged(object sender, EventArgs e)
    {
        var query = "";
        var tempcountry = NeBusinessUnit.GetbuCountry(hidCompanyID.Value);
        var header_text = "";
        if (ddlMatType.SelectedValue == "3" && origin != "workorder") // if its kitted and its a quote...
        {
            Fill_kitted_Totals();
        }
        if (origin == "quote" && ddlMatType.SelectedValue == "3")
        {
            return;
        }

        #region workorder

        if (ddlMatType.SelectedValue == "3" && origin == "workorder") // if mat type is kitted and it's a work order...
        {
            if (ddlKittedParts.SelectedIndex > 0)
            {
                var date = (ASPxDateEdit)Popup_GroupSelect.FindControl("date_required");
                ((Label)Popup_GroupSelect.FindControl("lb_required")).Visible = true;
                date.Visible = true;
                date.Date = DateTime.Now.AddDays(10);
                query = string.Format(@"
SELECT 
	inventory_kit_dtl_id AS groupselectid, 
	inventory_kit_dtl_master_id AS master_id, 
	FULL_PART_DESCRIPTION_WITH_LABOUR(inventory_kit_dtl_master_id,false,'{0}') as description, 
	NULL as Qty,
	0 RepairID 
FROM 
	inventory_kit_dtl 
WHERE 
	Inventory_kit_dtl_master_id < 990000 and 
	inventory_kit_dtl_hdr_id = {1}", tempcountry, ddlKittedParts.Value);
                if (origin != "rfq")
                {
                    try
                    {
                        Grid_GroupSelect.Columns["RepairID"].Visible = true;
                    }
                    catch
                    {
                    }
                }
                header_text = "Parts for: " + ddlKittedParts.Text;
            }
        }
        #endregion workorder
        #region group

        else if (ddlMatType.SelectedValue == "4") // if group is selected...
        {
            if (ddlSection.SelectedIndex != -1 || origin != "quote")
            {
                if (ddlKittedParts.SelectedIndex > 0)
                {
                    if (origin == "workorder")
                    {
                        query =
                            string.Format(
                                "Select id AS groupselectid, master_id, full_part_description_with_labour(master_id,false,'{0}') as description, null as Qty, 0 as RepairID, null as chk from Inventory_group_dtl where group_id = {1} and master_id<990000 ORDER BY id ASC",
                                tempcountry, ddlKittedParts.Value);
                    }
                    else
                    {
                        query =
                            string.Format(
                                "Select id AS groupselectid, master_id, full_part_description_with_labour(master_id,false,'{0}') as description, null as Qty, 0 as RepairID, null as chk from Inventory_group_dtl where group_id = {1} ORDER BY id ASC",
                                tempcountry, ddlKittedParts.Value);
                    }
                    //header_text = "Group Listing";
                }
            }
            else
            {
                lblerrorLabel.Text = "You must select a section before continuing to the group quick select";
                lblerrorLabel.Visible = true;
                ScriptManager1.SetFocus(ddlSection);
            }
        }
        #endregion group
        #region quote

        else if (ddlMatType.SelectedValue == "5") // if mat'l source is quote
        {
            if (ddlSection.SelectedIndex != -1 || origin != "quote")
            {
                if (ddlKittedParts.SelectedIndex > 0)
                {
                    if (origin == "workorder")
                    {
                        query = string.Format("CALL ds_quoteparts({0}, {1})", ddlKittedParts.Value.ToString().Remove(6),
                            ddlKittedParts.Value.ToString().Substring(6, 1));
                    }
                    else
                    {
                        query =
                            string.Format(@"Select a.id AS groupselectid, a.part_no as master_id, a.description as description, a.Qty, 0 RepairID 
from quote_worksheet a
left join quote_section b
ON a.section_id = b.id

where a.part_no!='' and a.part_no is not null and a.quote_id = {0} and a.revision = {1}
order by  CAST(b.section as SIGNED INTEGER) ASC, a.id
", ddlKittedParts.Value.ToString().Remove(6), ddlKittedParts.Value.ToString().Substring(6, 1));
                    }

                    try
                    {
                        Grid_GroupSelect.Columns["RepairID"].Visible = true;
                    }
                    catch
                    {
                    }
                    header_text = "Quote Worksheet Listing";
                    var date = (ASPxDateEdit)Popup_GroupSelect.FindControl("date_required");
                    ((Label)Popup_GroupSelect.FindControl("lb_required")).Visible = true;
                    date.Visible = true;
                    date.Date = DateTime.Now.AddDays(10);
                }
            }
        }
        #endregion quote
        #region other workorder

        else if (ddlMatType.SelectedValue == "6") // if you're grabbing from another work order
        {
            if (ddlSection.Value != null || origin != "quote")
            {
                if (ddlKittedParts.Value != null)
                {
                    var is_invoiced =
                        Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM woprog WHERE woprog_id = @v0  AND woprog_status in ('Invoiced', 'Waiting to be Invoiced')", new object[] { ddlKittedParts.Value }) > 0;
                    var table_name = is_invoiced ? "history" : "current";
                    query = string.Format(@"
SELECT 
  a.wo_detail_{0}_id AS groupselectid,
  a.wo_detail_{0}_master_id AS master_id,
  a.wo_detail_{0}_description AS description,
  a.wo_detail_{0}_qty_committed AS Qty,
  0 RepairID 
FROM
  wo_detail_{0} a
INNER JOIN inventory_item_master b ON a.wo_detail_{0}_master_id = b.master_id 
INNER JOIN inventory_tag c ON b.tag_id = c.tag_id 
WHERE 
	a.wo_detail_{0}_woprog_id = {1} AND
	a.wo_detail_{0}_master_id < 990000 AND 
	a.wo_detail_{0}_master_id != 2139 AND
	b.active = 1", table_name, ddlKittedParts.Value);
                    header_text = "Work Order Parts Listing";
                }
            }
        }
        #endregion other workorder
        #region PO

        else if (ddlMatType.SelectedValue == "7") // if you're grabbing from another purchase order
        {
            if (origin == "rfq")
            {
                query = string.Format(@"
SELECT 
	pc.po_details_id AS groupselectid,
	pc.po_details_part_no AS master_id,
	pc.po_details_description AS description,
	pc.po_details_qty_ordered AS Qty
FROM
	po_details_current pc
	Inner Join inventory_item_master im ON pc.po_details_part_no = im.master_id
	Inner Join inventory_tag it ON im.tag_id = it.tag_id
WHERE
	pc.po_details_poprog_id = {0} AND
	pc.po_details_part_no <  990000 AND
	ifnull(pc.po_details_part_no,0) <>  0 AND
	it.active =  '1' and
	it.tag_id <> 571 AND
	it.is_exclude = FALSE
", ddlKittedParts.Value);
            }
            else
            {
                query = string.Format(@"SELECT po_details_current.po_details_id AS groupselectid,
po_details_current.po_details_part_no AS master_id,
po_details_current.po_details_description AS description,
po_details_qty_ordered AS Qty,
0 as RepairID
FROM
po_details_current
Inner Join inventory_item_master ON po_details_current.po_details_part_no = inventory_item_master.master_id
Inner Join inventory_tag ON inventory_item_master.tag_id = inventory_tag.tag_id
WHERE
po_details_current.po_details_poprog_id = {0} AND
po_details_current.po_details_part_no <  990000 AND
ifnull(po_details_current.po_details_part_no,0) <>  0 AND
inventory_tag.active =  '1' and
inventory_tag.tag_id <> 571", ddlKittedParts.Value);
            }
        }
        #endregion PO
        #region RFQ

        else if (ddlMatType.SelectedValue == "8")
        {
            var comp = new NeBusinessUnit(hidCompanyID.Value);
            if (origin != "rfq")
            {
                query = string.Format(@"
SELECT 
	id groupselectid, 
	master_id, 
	FULL_PART_DESCRIPTION(master_id, TRUE, '{2}') description, 
	qty qty,
0 as RepairID
FROM 
	rfq_lineitem 
WHERE 
	rfq_header_id = {0} AND
	vendor_id = {1}", ddlKittedParts.Value, hidVendorID.Value, comp.country);
            }
            else if (origin == "rfq")
            {
                query = string.Format(@"
SELECT 
	id groupselectid, 
	master_id, 
	FULL_PART_DESCRIPTION(master_id, TRUE, '{1}') description, 
	qty,
0 as RepairID
FROM 
	rfq_lineitem 
WHERE 
	rfq_header_id = {0}", ddlKittedParts.Value, comp.country);
            }
            header_text = "Request for Quote Lines.";
        }

        #endregion RFQ

        sds_groupselect.SelectCommand = query;
        ViewState["sds_groupselect_datasource"] = query;
        Grid_GroupSelect.DataBind();
        if (Grid_GroupSelect.VisibleRowCount > 0)
        {
            lblerrorLabel.Text = "";
            Popup_GroupSelect.HeaderText = header_text;
            Popup_GroupSelect.ShowOnPageLoad = true;
        }
        else if (origin != "quote" && ddlMatType.SelectedValue != "3")
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "alert",
                "alert('There are no usable parts');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, typeof(UpdatePanel), "alert",
                "alert('There are no usable parts');", true);
        }
    }

    protected void callbackPanel_Callback(object sender, CallbackEventArgsBase e)
    {
        var _id = e.Parameter;
        var masterid = "";
        var PicSelect = "";
        if (origin == "quote")
        {
            PicSelect = Toolbox.doSQL_string(conn, @"SELECT part_no FROM quote_worksheet  WHERE id =@v0", new object[] { _id });
            if (PicSelect != "" && PicSelect != "0")
            {
                var inv = new inventory();
                inv.Load(PicSelect, hidCompanyID.Value);
                if (inv.allowed_to_stock)
                {
                    Image2.ImageUrl = "~/_tools/inventory_picture/index.aspx?id=" + PicSelect + "&is_master=true";
                    if_popup_part_files.Visible = false;
                }
                else
                {
                    Image2.Visible = false;
                    if_popup_part_files.Visible = true;
                    if_popup_part_files.Attributes["Src"] = "~/FileManager.aspx?parent_page=quote_worksheet_files&id=" + _id;
                    if_popup_part_files.DataBind();

                }
            }
            else
            {
                Image2.Visible = false;
                if_popup_part_files.Visible = true;
                if_popup_part_files.Attributes["Src"] = "~/FileManager.aspx?parent_page=quote_worksheet_files&id=" + _id;
                if_popup_part_files.DataBind();
            }
        }
        else if (origin == "groupings")
        {
            PicSelect = Toolbox.doSQL_string(conn, @"SELECT master_id FROM inventory_group_dtl  WHERE id =@v0", new object[] { _id });
            var inv = new inventory();
            inv.Load(PicSelect, hidCompanyID.Value);
            if (inv.allowed_to_stock)
            {
                Image2.ImageUrl = "~/_tools/inventory_picture/index.aspx?id=" + PicSelect + "&is_master=true";
                if_popup_part_files.Visible = false;
            }
            else
            {
                Image2.Visible = false;
                Image2.ImageUrl = "";
            }
        }
        else if (origin == "kitted")
        {

            PicSelect = Toolbox.doSQL_string(conn, @"SELECT inventory_kit_dtl_master_id FROM inventory_kit_dtl  WHERE inventory_kit_dtl_id =@v0", new object[] { _id });
            if (Convert.ToInt32(PicSelect) < 990000)
            {
                var inv = new inventory();
                inv.Load(PicSelect, hidCompanyID.Value);
                if (inv.allowed_to_stock)
                {
                    Image2.ImageUrl = "~/_tools/inventory_picture/index.aspx?id=" + PicSelect + "&is_master=true";
                    if_popup_part_files.Visible = false;
                }
                else
                {
                    Image2.Visible = false;
                    Image2.ImageUrl = "";
                }
            }
        }
        else
        {
            Image2.ImageUrl = "";
        }
    }

    private Dictionary<string, inventory> inv_i;
    private List<string> part_list;

    protected void popup_addpart(object sender, EventArgs e)
    {
        try
        {
            var ValidPricing = true;
            var SavePO = true;
            var errormsg = "";
            var fields = Grid_GroupSelect.Columns["RepairID"].Visible
                ? new[] { "chk", "groupselectid", "master_id", "Qty", "description", "RepairID" }
                : new[] { "chk", "groupselectid", "master_id", "Qty", "description" };
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
            if (keyvalues.Count == 0)
            {
                throw new Exception("Please make sure to select/check which rows you want to include");
            }
            var mergeCheck =
                Toolbox.doSQL_dt(conn, string.Format(@"SELECT master_id, new_id FROM inventory_item_master where master_id in ({0})", string.Join(",", part_list.ToArray())), null);
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
                        mergeCheck_all = Toolbox.doSQL_dt(conn, @"SELECT master_id, new_id FROM inventory_item_master", null);
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
            list_load.Load(part_csv, Convert.ToInt32(WarehouseBusinessUnit.id), true);
            inv_i = new Dictionary<string, inventory>();
            foreach (inventory i in list_load.parts)
            {
                inv_i.Add(i.master_id, i);
            }
            foreach (object[] key in keyvalues)
            {
                var gvcc = (GridViewCommandColumn)Grid_GroupSelect.Columns["chk"];
                /// TODO: This may cause issues in the future, moving to v2017
                //var gvcc_chkbox = gvcc.SelectButton;
                var gv = (GridViewDataColumn)Grid_GroupSelect.Columns["Qty"];
                var index = Grid_GroupSelect.FindVisibleIndexByKeyValue(key[1]);
                var editor = (TextBox)Grid_GroupSelect.FindRowCellTemplateControl(index, gv, "txtGroupSelectQty");
                string qty = qty = editor != null && editor.Text != "" ? Convert.ToDouble(editor.Text).ToString() : "0";
                var qty_dbl = Convert.ToDouble(qty);
                var lineid = key[1].ToString();
                var partno = key[2].ToString();

                if (changeItems.ContainsKey(partno))
                {
                    partno = changeItems[partno];
                }

                var description = key[4].ToString();
                var consignmentid = Grid_GroupSelect.Columns["RepairID"].Visible ? Convert.ToInt32(key[5]) : 0;
                if (partno == "0" && origin == "workorder")
                    continue;

                if (partno != "" && qty_dbl > 0)
                {
                    if (Convert.ToInt32(partno) < 900000 && partno != "0")
                    {
                        //		checkinv = inv_i[partno];
                        //		partno = checkinv.master_id;
                        partno = inv_i[partno].master_id;
                    }

                    #region this_origin == quote

                    if (origin == "quote")
                    {
                        double newsell = 0;
                        double newcost = 0;
                        ValidPricing = true;
                        try
                        {
                            if (Convert.ToInt32(partno) < 990000)
                            {
                                newsell =
                                    Toolbox.doSQL_double(conn, @"Select GetSellPrice(get_current_cost(@v0 ,@v1 ),proc_GetInvSellPrice(@v0 ,@v1 ),true,@v2, @v1 )", new object[] { partno, hidCompanyID.Value, qty });
                                newcost = Toolbox.doSQL_double(conn, @"Select get_current_cost(@v0 ,@v1 )", new object[] { partno, hidCompanyID.Value });
                            }
                            else
                            {
                                newsell = Toolbox.doSQL_double(conn, @"Select proc_GetInvSellPrice(@v0 ,@v1 )", new object[] { partno, hidCompanyID.Value });

                                double ChargeOut = 0;
                                var q = new quote(main_id);
                                try
                                {
                                    ChargeOut = Toolbox.doSQL_double(conn, @"CALL CUSTOMER_CHARGEOUT(@v0 , @v1 )", new object[] { q.cust_id, partno });
                                }
                                catch
                                {
                                    ChargeOut = newsell;
                                }
                                newsell = ChargeOut;


                                newcost = Toolbox.doSQL_double(conn, @"select get_current_labour_cost(@v0 ,@v1 )", new object[] { partno, hidCompanyID.Value });
                            }
                        }
                        catch
                        {
                            ValidPricing = false;
                        }
                        if (ValidPricing && ddlSection.SelectedItem.Value.ToString() != "0")
                        {
                            var quotelines = new quote();
                            quotelines.worksheet_section_id = Convert.ToInt32(ddlSection.SelectedItem.Value);
                            quotelines.worksheetquote_id = main_id;
                            quotelines.worksheet_rev = Convert.ToInt32(rev);
                            quotelines.worksheet_sell = Math.Round(newsell, 2);
                            quotelines.worksheet_partno = partno == null ? "" : partno;
                            ;
                            var setqty = Convert.ToDouble(qty);
                            quotelines.worksheet_member_id = Convert.ToInt32(current_user.id);
                            quotelines.worksheet_qty = setqty;
                            quotelines.worksheet_originalsell = Math.Round(newsell, 2);
                            quotelines.worksheet_extendedper = Math.Round(quotelines.worksheet_originalsell * setqty, 2);
                            quotelines.worksheet_description = description;
                            quotelines.worksheet_cost = Math.Round(newcost, 2);
                            quotelines.worksheet_code = "";
                            quotelines.cost_level = Toolbox.doSQL_int(conn, @"SELECT GET_COST(@v0 ,@v1 ,true)", new object[] { partno, WarehouseBusinessUnit.id });
                            quotelines.AddNewWorkSheetLine();
                        }
                        else if (ddlSection.Items.Count == 1)
                        {
                            throw new Exception("Parts cannot transfer to quote because you need to first create a section");
                        }
                        else if (ValidPricing && ddlSection.SelectedItem.Value.ToString() == "0")
                        {
                            errormsg +=
                                string.Format(
                                    "Part No {0} could not transfer to quote because you did not chose a section for these lines.<br/>", partno);
                        }
                        else
                        {
                            errormsg += string.Format("Part No {0} could not transfer to quote due to invalid pricing information.<br/>",
                                partno);
                        }
                    }
                    #endregion this_origin == quote
                    #region this_origin == workorder

                    else if (origin == "workorder")
                    {
                        if (consignmentid == 0)
                        {
                            if (Convert.ToInt32(partno) < 900000)
                            {
                                newpartnumber = inv_i[partno].master_id;
                                newqty = Convert.ToDouble(qty);
                                neworigsell =
                                    Toolbox.doSQL_double(conn, @"Select GetSellPrice(get_current_cost(@v0 ,@v1 ),0,true,@v2 )", new object[] { inv_i[partno].master_id, hidCompanyID.Value, qty });
                                newdescription = description;
                                if (((ASPxDateEdit)Popup_GroupSelect.FindControl("date_required")).Visible)
                                {
                                    newdatereq = Toolbox.MySQL_shortdt(((ASPxDateEdit)Popup_GroupSelect.FindControl("date_required")).Date);
                                }
                                AddNewWorkOrderLine(main_id, 0, Convert.ToInt32(inv_i[partno].master_id), true, inv_i[partno].is_exclude, false);
                            }
                        }
                        else
                        {
                            var dt = Toolbox.doSQL_dt(conn, @"Select * from quote_worksheet  where id =@v0", new object[] { key[0] });
                            var wo = new NeWOProg(main_id);
                            var record_number = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                var newwoline = new NeWODetailCurrent();
                                record_number = newwoline.GetLineCount(wo.woprog_id);
                                record_number++;
                                newwoline.tax1 = wo.woprog_tax1;
                                newwoline.tax2 = wo.woprog_tax1;
                                newwoline.tax3 = wo.woprog_tax1;
                                newwoline.tax4 = wo.woprog_tax1;
                                newwoline.description = dr["description"].ToString();
                                newwoline.master_id = 2139;
                                newwoline.code = "QUOTE";
                                newwoline.business_unit_id = wo.business_unit_id;
                                newwoline.qty_ordered = 1;
                                newwoline.qty_committed = 1;
                                newwoline.qty_invoiced = 1;
                                newwoline.cost = .01;
                                newwoline.sell = Convert.ToDouble(dr["extended_per"]);
                                newwoline.unit = newwoline.sell;
                                newwoline.woprog_id = Convert.ToInt32(wo.woprog_id);
                                newwoline.bvwo = Convert.ToInt32(wo.OrderNumber);
                                newwoline.origin = "Automatically Added";
                                newwoline.billtypeid = 3;
                                newwoline.rec_no = record_number;
                                newwoline.type = "Q";
                                newwoline.consignment_id = Convert.ToInt32(dr["consignment_id"]);
                                try
                                {
                                    newwoline.save(current_user, "/sections/member/picklist/pikclist.aspx.cs - popup_addpart #1", false);
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
                    #endregion workorder
                    #region this_origin == purchaseorder

                    else if (origin == "purchaseorder")
                    {
                        if (Convert.ToInt32(partno) < 990000)
                        {
                            try
                            {
                                newcost = Toolbox.doSQL_double(conn, @"Select Inv_max_cost(@v0 ,@v1 )", new object[] { partno, WarehouseBusinessUnit.id });
                            }
                            catch
                            {
                                newcost =
                                    Toolbox.doSQL_double(conn, @"SELECT po_details_cost FROM po_details_current where po_details_id = @v0 ", new object[] { lineid });
                            }
                        }
                        SavePO = true;
                        new_is_gl_account = string.IsNullOrEmpty(hidWo.Value) ? false : true; //ddlWorkOrder.Text.StartsWith("EXP");
                        if (ddlMatType.SelectedValue != "8")
                        {
                            try
                            {

                                newworkorderid = ddlWorkOrder.Value != null ? ddlWorkOrder.Value.ToString() : "0";
                            }
                            catch
                            {
                                newworkorderid = "0";
                                errormsg += string.Format("Part No {0} Not transferred to PO because Work Order Not Selected.<br/>", partno);
                            }


                        }



                        //		inv_i[partno].Load(partno, hidCompanyID.Value);
                        newpartnumber = partno;
                        newqty = Convert.ToDouble(qty);
                        newdescription = description;
                        if (ddlMatType.SelectedValue == "8") // From RFQ
                        {
                            try
                            {
                                var li = new rfq_lineitem(Convert.ToInt32(lineid));
                                newvendorpartnumber = li.vendor_code;
                                newqtyperpart = li.qty;
                                newcost = li.vendor_price / li.qty;
                                if (SavePO)
                                {
                                    newworkorderid = "9999999";
                                    AddNewPOLine();
                                }
                            }
                            catch (Exception ee)
                            {
                                errormsg += string.Format("Part No {0} Not transferred to quote due to {1}.<br/>", partno, ee);
                            }
                        }
                        else
                        {
                            try
                            {
                                newvendorpartnumber =
                                    Toolbox.doSQL_string(conn, @"SELECT po_details_vendor_part_no FROM po_details_current where po_details_id = @v0 ", new object[] { lineid });
                                newqtyperpart =
                                    Toolbox.doSQL_double(conn, @"SELECT po_details_vendor_qty_per FROM po_details_current where po_details_id = @v0 ", new object[] { lineid });
                                if (newworkorderid != "0" && SavePO)
                                {
                                    AddNewPOLine();
                                }
                            }
                            catch (PositiveNegativeException ex)
                            {
                                throw ex;
                            }
                            catch (Exception POEX)
                            {
                                errormsg += string.Format("Part No {0} Not transferred to quote due to {1}.<br/>", partno, POEX);
                            }
                        }
                    }
                    #endregion purchaseorder
                    #region this_origin == groupings

                    else if (origin == "groupings")
                    {
                        Toolbox.doSQL_void(conn, @"INSERT INTO inventory_group_dtl (group_id, master_id, inventory_group_dtl_notes,inventory_group_dtl_memberid,dt) VALUES (@v0 ,@v1 ,@v2 ,@v3 ,now())", new object[] { main_id, Convert.ToInt32(partno), null, current_user.id });
                        newpartnumber = partno;
                        newqty = Convert.ToDouble(qty);
                        newdescription = description;
                    }
                    #endregion groupings
                    #region this_origin == kitted

                    else if (origin == "kitted")
                    {
                        //		inv_i[partno].Load(partno, hidCompanyID.Value);
                        newpartnumber = partno;
                        newqty = Convert.ToDouble(qty);
                        Toolbox.doSQL_void(conn, @" INSERT INTO inventory_kit_dtl ( inventory_kit_dtl_hdr_id, inventory_kit_dtl_master_id, inventory_kit_dtl_member_id, inventory_kit_dtl_qty, inventory_kit_dtl_edited_dt, inventory_kit_dtl_active ) VALUES ( @v0 , @v1 , @v2 , @v3 , curdate(), 1 )", new object[] { main_id, newpartnumber, current_user.id, newqty });
                    }
                    #endregion kitted
                    #region this_origin == alternates

                    else if (origin == "alternates")
                    {
                        newpartnumber = partno;
                        newqty = Convert.ToDouble(qty);
                        newdescription = description;
                    }
                    #endregion alternates
                    #region this_origin == RFQ

                    else if (origin == "rfq")
                    {
                        var pl = new rfq_part_list();
                        pl.rfq_header_id = main_id;
                        if (ddlMatType.SelectedValue == "6")
                        {
                            // Check status of work order
                            var woprog_id = Convert.ToInt32(ddlKittedParts.Value);
                            var wo_p = new NeWOProg(woprog_id);
                            var table_prefix = wo_p.Status == OpsWOStatus.WaitingToBeInvoiced || wo_p.Status == OpsWOStatus.Invoiced
                                ? "wo_detail_history"
                                : "wo_detail_current";
                            newdatereq =
                                Toolbox.doSQL_string(conn, string.Format(@"SELECT IFNULL(MAX({0}_date_required),'') FROM {0}  WHERE {0}_woprog_id = @v0  AND {0}_master_id = @v1 ", table_prefix), new object[] { woprog_id, partno });
                            pl.required_date = newdatereq == "" ? null : (DateTime?)Convert.ToDateTime(newdatereq);
                        }
                        pl.master_id = Convert.ToInt32(partno);
                        pl.qty = Convert.ToDouble(qty);
                        pl.save();
                    }

                    #endregion RFQ
                }
            }
            Grid_GroupSelect.Selection.UnselectAll();
            ddlKittedParts.SelectedIndex = 0;
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
            switch (origin)
            {
                case "quote":
                    FillforQuote(main_id, main_rev);
                    break;
                case "kitted":
                    FillforKitted(main_id);
                    break;
                case "groupings":
                    FillforGroupings(main_id);
                    break;
                case "purchaseorder":
                    FillForPO(main_id);
                    break;
                case "alternates":
                    FillforAlternates(main_id);
                    break;
                case "rfq":
                    FillForRFQ(main_id);
                    break;
            }
        }
    }

    /// <summary>
    ///     Receives multiple rows
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <!-- used to be named btnUpdatePORecQty_Click -->
    protected void btnUpdatePORecQty_Click(object sender, EventArgs e)
    {
        mass_receive(sender, e);
    }


    protected void mass_receive(object sender, EventArgs e)
    {
        var bb = (ASPxButton)sender;
        var g = agv;
        var did_lock = false;
        var items = new List<POLineItem> { };
        var preState = false;
        var postState = false;

        #region purchase order

        if (origin == "purchaseorder")
        {
            preState = this.IsPOCompleted();
            double qty_rec = 0;
            var _complete = "0";
            double qtyRecHold = 0;
            double totqty = 0;
            var recordschanged = 0;
            var _oneshot = true;
            var bvpo = "";
            var wos = "";
            var email_body =
                "<table style='font-size:12px; text-align: center; font-family: Arial; border-collapse: collapse;' cellpadding='5'><tr><th style='text-align:left'>Part No</th><th style='text-align:left'>Description</th><th>Qty Received</th></tr>";
            var details = new NEPO_Details_Current();
            var newinv = new inventory();
            var company = new NeBusinessUnit(Convert.ToInt32(WarehouseBusinessUnit.id));
            var poprogress = new NePOProg(main_id);
            var column_location = (GridViewDataColumn)agv.Columns["workorder"];
            try
            {
                var start = g.VisibleStartIndex;
                var end = g.VisibleRowCount;
                // Check stuff out first.
                var error_list = new StringBuilder();
                var should_process = true;
                var dt_check_qty_per = Toolbox.doSQL_dt(conn, @"SELECT * FROM po_details_current  where po_details_poprog_id =@v0", new object[] { main_id });
                foreach (DataRow dr in dt_check_qty_per.Rows)
                {
                    var this_rec_n = Convert.ToInt32(dr["po_details_rec_no"]);
                    var this_cost = Convert.ToDouble(dr["po_details_cost"]);
                    var this_part_n = Convert.ToInt32(dr["po_details_part_no"]);
                    var this_qty_per = Convert.ToInt32(dr["po_details_vendor_qty_per"]);
                    if (this_cost / this_qty_per <= 0.0001 && this_part_n != 0)
                    {
                        error_list.AppendFormat(
                            @"<br/>Rec #{0} - Part #{1} has an invalid vendor qty per ({2}) - If its cost, {3}, were divided by this qty per, it would result in a zero sell price.",
                            this_rec_n, this_part_n, this_qty_per, this_cost);
                        should_process = false;
                    }

                    items.Add(new POLineItem
                    {
                        po_details_id = Convert.ToInt32(dr["po_details_id"]),
                        po_details_qty_received = Convert.ToDouble(dr["po_details_qty_received"]),
                        po_details_qty_ordered = Convert.ToDouble(dr["po_details_qty_ordered"]),

                        // for 1090 [US 111] Negative item receipts (Purchase Order)
                        isChangeAllowed = false,
                        po_details_poprog_id = Convert.ToInt32(dr["po_details_poprog_id"]),
                        po_details_woprog_id = Convert.ToInt32(dr["po_details_woprog_id"]),
                        po_details_part_no = Convert.ToInt32(dr["po_details_part_no"]),
                    });
                }
                update_error(should_process ? "" : error_list + "<br/><br/>");
                if (!should_process)
                {
                    return;
                }

                for (var i = start; i < end; i++)
                {
                    var keyValue = g.GetRowValues(i, agv.KeyFieldName);
                    var gv = (GridViewDataColumn)g.Columns["emptyval"];

                    var tb_qty_rec = (TextBox)g.FindRowCellTemplateControl(i, gv, "txtRecSelectQty");
                    qty_rec = 0;
                    if (tb_qty_rec != null)
                    {
                        double.TryParse(tb_qty_rec.Text, out qty_rec);
                    }

                    foreach (var item in items)
                    {
                        if (keyValue != null && (Convert.ToInt32(keyValue) == item.po_details_id))
                        {
                            item.po_details_qty_received_from_ui = qty_rec;

                            // Calculate the final received number after applying this change.
                            item.sum = item.po_details_qty_received + item.po_details_qty_received_from_ui;
                            break;
                        }
                    }
                }

                // 060- Purchase Orders Cannot Contain Positive and Negative Quantities
                // [PO] User should not be able to receive items on a Negative PO
                //
                //                    |
                //                    |  Changed to bottom
                //                    V
                //
                // 1047 [NESI UAT] Regular Purchase Order Item Receipts
                // -------------------------------------------------------------------
                // On regular purchase orders in NESI. That is on purchase orders that have all positive lines.
                // The amount of quantity received can be reduced as long as the total does not go below zero.
                //

                //  Here all qty ordered can all be positive or all be negative.
                var countOfPositive = items.Where(x => x.po_details_qty_received_from_ui + x.po_details_qty_received > 0).Count();
                var countOfNegative = items.Where(x => x.po_details_qty_received_from_ui + x.po_details_qty_received < 0).Count();

                var countOfSummaryPositive = items.Where(x => x.sum > 0).Count();
                var countOfSummaryNegative = items.Where(x => x.sum < 0).Count();

                if ((items[0].po_details_qty_ordered > 0 && countOfSummaryNegative > 0) ||
                     (items[0].po_details_qty_ordered < 0 && countOfPositive > 0))
                {
                    // All should be positive or negative.
                    throw new Exception(
                        string.Format("NESI shall not accept both positive and negative quantities for Purchase Order {0}",
                            main_id));
                }

                //
                // [1090] [US 111] Negative item receipts (Purchase Order)
                //
                if (items[0].po_details_qty_ordered > 0)
                {
                    // 
                    // The below code will check each of attempt updating from the po line items.
                    // Any of error from one of the po line item will stop others updates. In other words, all need to pass the checks.
                    //
                    var updateDateTimeForPOLines = DateTime.Now;
                    if (!this.IsItemReceiptChangesAllowed(items, updateDateTimeForPOLines))
                    {
                        // 3.	If any of the above are not true, an error a message should be displayed to the user stating
                        // that “A negative purchase order is required to reverse this transaction”.
                        throw new Exception(
                            string.Format("A negative purchase order is required to reverse this transaction {0}",
                                main_id));
                    }
                }

                for (var i = start; i < end; i++)
                {
                    var keyValue = g.GetRowValues(i, agv.KeyFieldName);

                    var gv = (GridViewDataColumn)g.Columns["emptyval"];
                    var cb_commit_wo =
                        (CheckBox)g.FindRowCellTemplateControl(i, (GridViewDataColumn)g.Columns["commit_wo"], "cb_commit");
                    var location = (ASPxComboBox)agv.FindRowCellTemplateControl(i, column_location, "combo_location");
                    var commit_wo = false;
                    if (cb_commit_wo != null)
                    {
                        commit_wo = cb_commit_wo.Checked;
                    }

                    _complete = agv.GetRowValues(i, "active") == null ? "0" : agv.GetRowValues(i, "active").ToString();

                    var tb_qty_rec = (TextBox)g.FindRowCellTemplateControl(i, gv, "txtRecSelectQty");
                    qty_rec = 0;
                    if (tb_qty_rec != null)
                    {
                        double.TryParse(tb_qty_rec.Text, out qty_rec);
                    }

                    #region if the user is receiving more than 0 and the line is active

                    if (qty_rec != 0 && _complete == "1")
                    {

                        if (_oneshot)
                        {
                            var podetails = new NEPO_Details_Current();
                            // if (doBv)
                            // {
                            //     var lockinfo = PurchaseOrder.CheckPOLock(company.DSN, poprogress.poprog_bvpo);
                            //     if (lockinfo != "" && !lockinfo.Contains("MH"))
                            //     {
                            //         throw new Exception("This PO is Locked By User " + lockinfo + " and cannot be Changed at this moment");
                            //     }
                            //     PurchaseOrder.UnlockPO(company.DSN, poprogress.poprog_bvpo, current_user.Initials);
                            //     PurchaseOrder.LockOrder(company.DSN, poprogress.poprog_bvpo, current_user.Initials);
                            // }
                            did_lock = true;
                            bvpo = poprogress.poprog_bvpo;

                            _oneshot = false;
                        }

                        recordschanged++;
                        details.po_details_current_line(Convert.ToInt32(keyValue));

                        if (!commit_wo)
                        {
                            //
                            // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1363/
                            // All the rental items should be committed to workorder.
                            //
                            var dt = Toolbox.doSQL_dt(conn,
                                @"SELECT a.tag_id, b.is_rental FROM inventory_item_master a LEFT JOIN inventory_tag b ON a.tag_id = b.tag_id WHERE a.master_id = @v0 ",
                                new[] { details.po_details_part_no.ToString() });
                            if (dt != null && dt.Rows.Count == 1)
                            {
                                var is_rental = Convert.ToBoolean(dt.Rows[0]["is_rental"]);
                                if (is_rental)
                                {
                                    // Rental items must be committed to wo.
                                    commit_wo = true;  // commit to wo
                                }
                            }
                        }

                        if (details.po_details_part_no > 0)
                        {
                            newinv.Load(details.po_details_part_no, WarehouseBusinessUnit.id);
                        }
                        var GLAccName = details.is_gl_account ?
                            Toolbox.doSQL_string(conn, @"SELECT IFNULL(MAX(gl_te.account_no),'') FROM gl_te WHERE gl_te.id = @v0 ", new object[] { details.po_details_woprog_id }) : "";

                        #region CHECKPOINT: If this isn't a stock work order, check the status of the work order for validity

                        if (!new List<int>(new[] { 9999999, 9999998, 9999997 }).Contains(details.po_details_woprog_id) &&
                            commit_wo && !details.is_gl_account)
                        {
                            var dr =
                                Toolbox.doSQL_dt(conn, @"SELECT woprog_status,woprog_bvwo,woprog_hold FROM woprog WHERE woprog_id = @v0 ", new object[] { details.po_details_woprog_id }).Rows[0];
                            var statusofwo = dr["woprog_status"].ToString();
                            var wobvwoofwo = dr["WOProg_BVWO"].ToString();
                            var hold = Convert.ToBoolean(dr["woprog_hold"]);
                            if (new List<string>(new[]
                                                    {
                                                       OpsWOStatus.Invoiced,
                                                       OpsWOStatus.WaitingToBeInvoiced,
                                                       OpsWOStatus.WaitingForPO,
                                                       OpsWOStatus.WaitingBMApproval,
                                                       OpsWOStatus.WaitingPMApproval,
                                                       OpsWOStatus.WaitingParentBMApproval,
                                                       OpsWOStatus.Deleted
                                                    }).Contains(statusofwo) || hold)
                            {
                                if (newinv.tag_id != "843")
                                {
                                    throw new Exception(
                                        string.Format("You Cannot Receive parts for a Work Order: {0} because it has Status of : {1} or is on hold",
                                            wobvwoofwo, statusofwo));
                                }
                            }
                        }

                        #endregion CHECKPOINT: If this isn't a stock work order, check the status of the work order for validity

                        old_qty_committed = details.po_details_qty_received;
                        totqty = details.po_details_qty_received + qty_rec;
                        newworkorderid = details.po_details_woprog_id.ToString();
                        newpartnumber = details.po_details_part_no.ToString();
                        email_body +=
                            string.Format("<tr><td style='text-align:left'>{0}</td><td style='text-align:left'>{1}</td><td>{2}</td></tr>",
                                newpartnumber, details.po_details_description, qty_rec);

                        try
                        {
                            newdescription = Toolbox.doSQL_string(conn, @"SELECT full_part_description(@v0 ,true,'CDN')", new object[] { newpartnumber });
                        }
                        catch
                        {
                        }
                        new_qty_committed = qty_rec;
                        newqty = details.po_details_qty_orderd;
                        details.po_details_id = Convert.ToInt32(keyValue.ToString());

                        #region Save Notes

                        if (details.woprog_id != 0 || !commit_wo)
                        {
                            try
                            {
                                xfer_comm_recv_select_id = location.Visible ? Convert.ToInt32(location.Value) : 0;
                                var temp_location = xfer_comm_recv_select_id > 0 ? new location(xfer_comm_recv_select_id) : new location();
                                var location_name = xfer_comm_recv_select_id > 0 ? new location_master(temp_location.location_master_id).name : "";

                                var destination = new List<int>(new[] { 9999999, 9999998, 9999997 }).Contains(details.woprog_id) || !commit_wo && !details.is_gl_account
                                    ? "to stock - Location: " + location_name
                                    : details.is_gl_account
                                        ? "to GL Account - " + GLAccName
                                        : "to WO - " + Toolbox.doSQL_string(conn, @"SELECT woprog_bvwo FROM woprog  WHERE woprog_id =@v0", new object[] { details.woprog_id });
                                var eventtext = string.Format("{0}: {1} received {2}", details.po_details_part_no, qty_rec, destination);
                                //if (doBv)
                                //{
                                //    var notes = new bv_notes
                                //    {
                                //        prog = poprogress.poprog_bvpo,
                                //        item = "PORD",
                                //        n_user = current_user.Initials,
                                //        subject = string.Format("{0}: {1} received", details.po_details_part_no, qty_rec),
                                //        detail = eventtext
                                //    };
                                //    notes.Save(company.DSN);
                                //}
                                var prognotes = new NePOProg
                                {
                                    eventtext = eventtext,
                                    notes_poprogid = poprogress.poprog_id,
                                    poprog_bvpo = poprogress.poprog_bvpo,
                                    notes_memberid = current_user.id,
                                    business_unit_id = company.id
                                };
                                prognotes.SaveNotes();
                            }
                            catch (Exception ee)
                            {

                            }
                        }

                        #endregion Save Notes

                        qty_rec = details.po_details_qty_received + qty_rec;
                        qtyRecHold = details.po_details_qty_received;
                        details.po_details_qty_received = qty_rec;

                        #region add note if the quantity received is greater than the quantity ordered

                        if (qty_rec > details.po_details_qty_orderd)
                        {
                            //Create Note
                            var eventtext = string.Format("{0} received {1} when the PO had {2} on order.", details.po_details_part_no,
                                qty_rec, details.po_details_qty_orderd);
                            try
                            {
                                Toolbox.doSQL_void(conn, @" INSERT INTO poprog_notes ( poprog_id, eventtext, date, member_id ) VALUES ( @v0 , @v1 , now(), @v2  )", new object[] { main_id, eventtext, current_user.id });
                            }
                            catch (Exception ee)
                            {

                                throw;
                            }
                        }

                        #endregion add note if the quantity received is greater than the quantity ordered

                        details.po_details_line_active = details.po_details_qty_orderd == details.po_details_qty_received ? 0 : 1;

                        #region save to purchase order

                        try
                        {

                            details.PO_Details_Update();

                        }
                        catch (Exception ee)
                        {

                            throw;
                        }

                        #endregion save to purchase order
                        Toolbox.do_debug_note("PO_RCV_DEBUG: - business_unit_id: " + company.id + " poprogress.poprog_bvpo:" +
                                        poprogress.poprog_bvpo + " details.po_details_poprog_id: " + details.po_details_poprog_id +
                                        " details.po_details_id:" + details.po_details_id + " myMember.id:" + current_user.id + " new_qty_committed:" +
                                        new_qty_committed + " commit_wo:" + commit_wo + " checkbox:" +
                                        (cb_commit_wo == null ? "null" : Toolbox.dict_dump(Toolbox.dict_create(cb_commit_wo))));
                        try
                        {
                            if (commit_wo && !details.is_gl_account && !Toolbox.Contains(details.woprog_id, new[] { 999997, 999998, 999999 }) &&
                                details.woprog_id > 20000)
                            {
                                #region save & commit to work order

                                Toolbox.do_debug_note("business_unit_id: " + company.id + " poprogress.poprog_bvpo:" + poprogress.poprog_bvpo +
                                                " details.po_details_poprog_id: " + details.po_details_poprog_id + " details.po_details_id:" +
                                                details.po_details_id + " myMember.id:" + current_user.id + " new_qty_committed:" + new_qty_committed +
                                                " commit_wo:" + commit_wo);

                                details.MoveToWOProgDetails(poprogress.poprog_bvpo,
                                    details.po_details_poprog_id.ToString(), details.po_details_id.ToString(), current_user.id.ToString(),
                                    new_qty_committed, commit_wo);

                                var wo_n = "";
                                var recnumber = "";
                                try
                                {
                                    wo_n =
                                        Toolbox.doSQL_string(conn, @"SELECT woprog_bvwo FROM woprog WHERE woprog_id = @v0 ", new object[] { details.woprog_id });
                                    recnumber =
                                        Toolbox.doSQL_string(conn, @"SELECT wo_detail_current_rec_no FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  and wo_detail_current_master_id = @v1 ", new object[] { details.woprog_id, details.po_details_part_no });
                                }
                                catch (Exception ee)
                                {
                                    throw ee;
                                }

                                #endregion save & commit to work order

                                #region save audit trail

                                try
                                {
                                    AuditSave(wo_n, recnumber, company.id.ToString(), details.woprog_id.ToString(), false, false, commit_wo);
                                    if (!wos.Contains(details.woprog_id.ToString()))
                                    {
                                        wos += details.woprog_id + ",";
                                    }
                                }
                                catch (Exception ee)
                                {

                                    throw;
                                }

                                #endregion save audit trail
                            }
                            else if (details.woprog_id < 999990 && details.woprog_id > 200000)
                            {
                                #region save zero entry to work order
                                try
                                {

                                    details.MoveToWOProgDetails(poprogress.poprog_bvpo, details.po_details_poprog_id.ToString(), details.po_details_id.ToString(), current_user.id.ToString(), (new_qty_committed * details.po_details_vendor_qty_per));

                                }
                                catch (Exception ee)
                                {
                                    details.po_details_qty_received = old_qty_committed;
                                    details.po_details_line_active = 1;
                                    details.PO_Details_Update();

                                    throw;
                                }
                                #endregion save to work order
                            }

                            try
                            {
                                #region if we aren't committing directly to the workorder, update stock info

                                xfer_comm_recv_select_id = location.Visible ? Convert.ToInt32(location.Value) : 0;
                                if (!details.is_gl_account && details.woprog_id > 200000 && details.po_details_part_no != OpsSpecialPart.MiscMaterial)
                                {
                                    var stock_xfer = new Nestock_transfer();
                                    stock_xfer.master_id = details.po_details_part_no;
                                    if (new_qty_committed > 0) // receiving in goes INTO inventory or WO
                                    {
                                        if (commit_wo)
                                        {
                                            stock_xfer.from_id = main_id;
                                            stock_xfer.to_id = details.woprog_id;
                                            stock_xfer.type_id = 6; // From PO TO WO
                                        }
                                        else
                                        {
                                            stock_xfer.from_id = main_id;
                                            stock_xfer.to_id = xfer_comm_recv_select_id;
                                            stock_xfer.to_location_id = stock_xfer.to_id;
                                            stock_xfer.type_id = 2; // From PO TO inventory
                                        }
                                    }
                                    else // receiving back
                                    {
                                        if (commit_wo)
                                        {
                                            stock_xfer.from_id = details.woprog_id;
                                            stock_xfer.to_id = main_id;
                                            stock_xfer.type_id = 7; // From WO TO PO
                                        }
                                        else
                                        {
                                            stock_xfer.from_id = xfer_comm_recv_select_id;
                                            stock_xfer.from_location_id = stock_xfer.from_id;
                                            stock_xfer.to_id = main_id;
                                            stock_xfer.type_id = 5; // From Inventory TO ether
                                        }
                                    }
                                    stock_xfer.quantity = new_qty_committed * details.po_details_vendor_qty_per; //quantity that is received into stock
                                    stock_xfer.description = newdescription;
                                    stock_xfer.member_id = current_user.id;
                                    stock_xfer.note = "PO Quantity Updated";
                                    stock_xfer.business_unit_id = Convert.ToInt32(WarehouseBusinessUnit.id);
                                    stock_xfer.cost = details.po_details_cost / details.po_details_vendor_qty_per;
                                    stock_xfer.po_line_id = details.po_details_id;
                                    if (stock_xfer.quantity != 0)
                                    {
                                        stock_xfer.Nestock_transfer_save();
                                    }
                                }

                                #endregion if we aren't committing directly to the workorder, update stock info
                            }
                            catch (Exception ee)
                            {

                                throw;
                            }
                        }
                        catch (Exception ex2)
                        {
                            #region throw errors

                            details.po_details_qty_received = qtyRecHold;
                            details.po_details_line_active = 1;
                            try
                            {
                                details.PO_Details_Update();
                            }
                            catch (Exception ee)
                            {

                                throw;
                            }
                            Toolbox.do_debug_note(ex2);
                            throw;

                            #endregion throw errors
                        }
                    } // end if (qtyRec != 0 && _complete == "1")
                    else
                    {

                    }

                    #endregion if the user is receiving more than 0 and the line is active
                } // end for loop

                if (recordschanged > 0)
                {
                    //Toolbox.do_debug_note("Before Move to BVPORecord");

                    // details.MoveToBVPORecord(id, current_user.id.ToString());


                    wos = wos.TrimEnd(',');
                    if (wos.Length > 0)
                    {
                        var wos_split = wos.Split(',');
                        foreach (var w in wos_split)
                        {
                            var ww = new NeWOProg(Convert.ToInt32(w));
                            email_body +=
                                "</table></br>Contact your purchaser to find out where the parts are.  Most likely they are at the shop on the job shelf.";
                            NePOProg.send_email_to_trackers(id,
                                "Your parts ordered for WO " + ww.OrderNumber + " for " + ww.CustomerName + " have been received", current_user,
                                email_body);
                        }
                    }

                    //Toolbox.do_debug_note("After Move to BVPORecord");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //if (doBv && did_lock)
                //{
                //    PurchaseOrder.UnlockPO(company.DSN, poprogress.poprog_bvpo, current_user.Initials);
                //}
                FillForPO(main_id);
                populate_sds_workorder_ddl(hidCompanyID.Value, GetExp(hidCompanyID.Value));
                fill_edit_form_workorder_ddl();
                SqlDeptDataSource2.SelectCommand =
                    string.Format(@"(SELECT 0 AS Division_ID, 'N/A' AS Division_Name) UNION (SELECT id Division_ID, ddl_name AS Division_Name
																FROM business_unit
																WHERE id = {0}
																ORDER BY id)", hidCompanyID.Value);
            }
            //updatePOTotal();
        }

        #endregion purchase order

        if (origin == "purchaseorder")
        {
            postState = this.IsPOCompleted();
        }

        // Serialize the list so the browser can use it.
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        var data = serializer.Serialize(this.poLineCompleteInfo);

        bool statusChanged = preState != postState;
        if (statusChanged)
        {
            string code = string.Format("bind_tooltips();ShowPOInvoiceButton(\"{0}\");updatePoLineCompleteState({1});", postState ? "Yes" : "No", data);
            ScriptManager.RegisterStartupScript(this, GetType(), "binder", code, true);
        }
        else
        {
            string code = string.Format("bind_tooltips();updatePoLineCompleteState({0});", data);
            ScriptManager.RegisterStartupScript(this, GetType(), "binder", code, true);
        }

        //Toolbox.do_debug_note("End ASPxButton1_Click");
    }

    protected void save_polineitem_note_toBV(string subject)
    {
        try
        {
            if (origin == "purchaseorder")
            {
                var po = new NePOProg(main_id);
                var note_builder = "";


                var dt =
                    Toolbox.doSQL_dt(conn, @"Select po_details_rec_no, po_details_part_no, po_details_vendor_part_no, po_details_notes, po_details_line_active from po_details_current  where po_details_poprog_id =@v0", new object[] { main_id });
                foreach (DataRow dr in dt.Rows)
                {
                    var complete = "Incomplete";
                    if (dr[4].ToString() == "0")
                    {
                        complete = "Complete";
                    }
                    note_builder += dr[0] + " -- " + complete + "- " + dr[1] + " (" + dr[2] + ") ---" +
                                    dr[3].ToString().Replace("'", "").Replace("\"", "") + @"
";
                }

                //if (doBv && note_builder != "")
                //{
                //    var notes = new bv_notes();
                //    notes.subject = "Line Item Notes";
                //    notes.item = po.poprog_bvpo;
                //    notes.prog = "PORD";
                //    notes.n_user = current_user.Initials;
                //    notes.detail = Toolbox.do_value_from(note_builder).Replace("'", "").Replace("\"", "");
                //    notes.Save(new NeBusinessUnit(hidCompanyID.Value).DSN);
                //}
            }
        }
        catch
        {
        }
    }

    protected void UpdateSellPriceForMultipleParts(int master_id, double qty_provided)
    {
        //If a part is on a quote worksheet multiple times that sell price
        //needs to be updated each time a new part is added to reflect the
        //quantity discount on all the parts
        var inv_obj = new inventory();
        if (master_id == 0 || !inv_obj.part_exists(master_id))
        {
            return;
        }
        double qty_total = 0;
        switch (origin)
        {
            case "quote":
                qty_total =
                    Toolbox.doSQL_double(conn, @"SELECT SUM(qty) FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1  AND part_no = @v2 ", new object[] { id, rev, master_id });
                break;
            case "workorder":
                qty_total =
                    Toolbox.doSQL_double(conn, @"SELECT sum(wo_detail_current_qty_committed) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { id, master_id });
                break;
        }
        if (qty_total > qty_provided)
        {
            //Update all prices
            //exists_popup.ShowOnPageLoad		= (_origin == "quote");
            try
            {
                inv_obj.Load(master_id, WarehouseBusinessUnit.id);
                //exists_master_id.Value			= inv_obj.master_id;
                if (!inv_obj.is_exclude)
                {
                    div_refactor.Style["display"] = "block";
                    agv.JSProperties.Add("cpInfo", "show_refactor");
                    /*
					double mysql_cost		= Toolbox.doSQL_double(conn,@"SELECT get_current_cost(@v0 , @v1 )", new object[] {  inv_obj.master_id, inv_obj.business_unit_id } );
					double obj_cost			= inv_obj.cost_price_branch_highest;
					double current_cost		= mysql_cost == 0 && obj_cost > 0 ? obj_cost : mysql_cost;
					double sellprice		= Math.Round(NeShared.GetSellPrice(current_cost, inv_obj.sell_price, inv_obj.is_qty, qty_total), 5);
					switch (_origin)
						{
						case "quote":
							Toolbox.doSQL_void(conn,@" UPDATE quote_worksheet SET original_sell = @v0  WHERE quote_id = @v1  AND revision = @v2  AND part_no = @v3 ", new object[] {  sellprice, id, rev, inv_obj.master_id  } );
						break;
						case "workorder":
							Toolbox.doSQL_void(conn,@" UPDATE wo_detail_current SET wo_detail_current_price_sell = @v0  WHERE wo_detail_current_woprog_id = @v1  AND wo_detail_current_master_id = @v2 ", new object[] {  sellprice, id, inv_obj.master_id  } );
						break;
						}
					 */
                }
            }
            catch (Exception ex2)
            {
                throw new Exception(ex2.ToString());
            }
        }
    }

    protected void lst_addsections_section_Callback(object sender, CallbackEventArgsBase e)
    {
        Populate_SectionDDL(_q["id"], _q["rev"]);
    }

    protected void cbp_sections_Callback(object sender, CallbackEventArgsBase e)
    {
        var _id = Convert.ToInt32(persist_sections["id"]);
        var _section_name = persist_sections["section_name"].ToString();
        var _type = persist_sections["type"].ToString();
        if (_type == "edit" || _type == "add")
        {
            var isNew = _id == 0 || _type == "add";
            if (isNew)
            {
                Toolbox.doSQL_void(conn, @" INSERT INTO quote_section ( quote_id, revision, section, picklist_controlled ) VALUES ( @v0 , @v1 , @v2 , 1 )", new object[] { _q["id"], _q["rev"], _section_name });
            }
            else
            {
                Toolbox.doSQL_void(conn, @" UPDATE quote_section SET section = @v1 , picklist_controlled = 1 WHERE id = @v0  LIMIT 1", new object[] { _id, _section_name });
            }
        }
        else if (_type == "delete")
        {
            if (int.TryParse(persist_sections["id"].ToString(), out _id))
            {
                Toolbox.doSQL_void(conn, @"DELETE FROM quote_section WHERE id = @v0  LIMIT 1", new object[] { _id });
                persist_sections["last_deleted_id"] = _id.ToString();
                if (persist_sections.Contains("ddlselected") && persist_sections["ddlselected"].ToString() != string.Empty)
                {
                    if (_id == Convert.ToInt32(persist_sections["ddlselected"]))
                    {
                        persist_sections["ddlselected"] = string.Empty;
                    }
                }
            }
            else
            {
                throw new Exception("ID is not numerical");
            }
        }
    }

    protected void ddlSection_Callback(object sender, CallbackEventArgsBase e)
    {
        Populate_SectionDDL(_q["id"], _q["rev"]);
    }

    protected void refactor_prices()
    {
        var current_parts = new DataTable();
        var business_unit_id = "";
        switch (origin)
        {
            case "quote":
                // SubContractor = 55555;
                current_parts =
                    Toolbox.doSQL_dt(conn, @"SELECT part_no master_id, count(*) c FROM (SELECT part_no, COUNT(part_no) c, sell FROM quote_worksheet WHERE part_no NOT IN ('', '55555') AND quote_id = @v0  AND revision = @v1  AND CAST(part_no AS UNSIGNED) < 990000 AND CAST(part_no AS UNSIGNED) > 0 GROUP BY part_no,sell) a GROUP BY part_no HAVING c > 1", new object[] { id, rev });
                business_unit_id =
                    Toolbox.doSQL_string(conn, @"SELECT business_unit_id FROM quote_master WHERE quote_id=@v0  AND revision = @v1 ", new object[] { id, rev });
                hidCompanyID.Value = business_unit_id;
                break;
            case "workorder":
                //	current_parts	= Toolbox.doSQL_dt(conn,@"SELECT sum(wo_detail_current_qty_committed) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] {  id, master_id } );
                break;
        }
        double total_on = 0;
        double used_sell = 0;
        var inv_obj = new inventory();
        if (!inv_obj.is_exclude)
        {
            if (current_parts.Rows.Count > 0)
            {
                foreach (DataRow _dr in current_parts.Rows)
                {
                    var master_id = _dr["master_id"].ToString();
                    try
                    {
                        inv_obj.Load(master_id, WarehouseBusinessUnit.id);
                    }
                    catch (Exception ee)
                    {

                        throw;
                    }
                    var mysql_cost =
                        Toolbox.doSQL_double(conn, @"SELECT get_current_cost(@v0 , @v1 )", new object[] { master_id, inv_obj.business_unit_id });
                    var obj_cost = inv_obj.cost_price_branch_highest;
                    var current_cost = mysql_cost == 0 && obj_cost > 0 ? obj_cost : mysql_cost;
                    switch (origin)
                    {
                        case "quote":
                            total_on =
                                Toolbox.doSQL_double(conn, @"SELECT SUM(qty) FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1  AND part_no = @v2 ", new object[] { id, rev, master_id });
                            used_sell = shared.GetSellPrice(current_cost, 0, inv_obj.is_qty, total_on, WorkingBusinessUnit.id32);
                            Toolbox.doSQL_void(conn, @" UPDATE quote_worksheet SET sell = @v0 , original_sell = @v0 , extended_per = qty * @v0  WHERE quote_id = @v1  AND revision = @v2  AND part_no = @v3 ", new object[] { used_sell, id, rev, master_id });
                            break;
                        case "workorder":
                            total_on =
                                Toolbox.doSQL_double(conn, @"SELECT sum(wo_detail_current_qty_committed) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1 ", new object[] { id, master_id });
                            used_sell = shared.GetSellPrice(current_cost, 0, inv_obj.is_qty, total_on, WorkingBusinessUnit.id32);
                            Toolbox.doSQL_void(conn, @" UPDATE wo_detail_current SET wo_detail_current_price_sell = @v0  WHERE wo_detail_current_woprog_id = @v1  AND wo_detail_current_master_id = @v2 ", new object[] { used_sell, id, master_id });
                            break;
                    }
                }
            }
        }
    }


    protected string GetGLs(string _business_unit_id)
    {
        var sql_addon = string.Format("(SELECT gl_te.id AS WOProg_ID, Concat('GL: ',account_no,' - ',gl_chart_name) AS WorkOrder FROM gl_te inner join gl_group_te g on g.id = gl_te.gl_group_id where g.type = 'X' and gl_te.tax_entity_id = {0} and gl_te.see_on_po_gl_list=1) UNION (SELECT 9999999 AS WOProg_ID, \'ORDER FOR STOCK\' AS WorkOrder) UNION ", new NeBusinessUnit(_business_unit_id).tax_entity_id);

        return sql_addon;
    }

    protected string GetExp(string _business_unit_id)
    {
        var sql_addon = string.Format("(Select expense_category_id as WOProg_ID, Name AS WorkOrder from expense_category where Active = 1) UNION (SELECT 9999999 AS WOProg_ID, \'ORDER FOR STOCK\' AS WorkOrder) UNION ", new NeBusinessUnit(_business_unit_id).tax_entity_id);

        return sql_addon;
    }


    protected void chkApplyDiscount_CheckedChanged(object sender, EventArgs e)
    {
        if (origin == "quote")
        {
            double quotediscountamt = 0;
            var quotediscountflag = "0";
            if (chkQuoteDiscount.Checked)
            {
                var woline =
                    Toolbox.doSQL_dt(conn, @"SELECT customer_id c_id, address_id a_id FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ", new object[] { main_id, rev }).Rows[0];
                var c_id = Convert.ToInt32(woline["c_id"]);
                var a_id = Convert.ToInt32(woline["a_id"]);
                if (a_id == 0)
                {
                    a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
                }

                DiscountAmount = 0;
                hidWODiscount.Value = DiscountAmount.ToString();
                quotediscountamt = Convert.ToDouble(DiscountAmount);
                quotediscountflag = "1";
            }
            else
            {
                hidWODiscount.Value = "0";
                quotediscountamt = 0;
                quotediscountflag = "0";
            }
            // All material parts
            Toolbox.doSQL_void(conn, @"UPDATE quote_worksheet AS a, inventory_item_master AS b, inventory_tag AS c SET quote_worksheet_discount = @v0  WHERE a.part_no = b.master_id AND b.tag_id = c.tag_id AND CAST(if(a.part_no > 0,a.part_no,0) AS SIGNED) < 990000 AND a.part_no != '0' AND a.Part_no != '' AND is_exclude = 0 AND a.quote_id = @v1  AND a.revision = @v2 ", new object[] { quotediscountamt, main_id, rev });
            // All kits
            Toolbox.doSQL_void(conn, @"UPDATE quote_worksheet SET quote_worksheet_discount = @v0  WHERE CAST(if(part_no > 0,part_no,0) AS SIGNED) >= 2000000 AND quote_id = @v1  AND revision = @v2 ", new object[] { quotediscountamt, main_id, rev });
            // For handling addline stuff
            var extd = TextQuotedExtd.Text != "" ? Convert.ToDouble(TextQuotedExtd.Text) : 0;
            var _extd = TextTMExtd.Text != "" ? Convert.ToDouble(TextTMExtd.Text) : 0;
            if (extd > 0 && _extd > 0)
            {
                TextTMExtd.Text = chkQuoteDiscount.Checked ? Convert.ToString(extd - extd * (quotediscountamt / 100)) : extd.ToString();
            }


            Toolbox.doSQL_void(conn, @"UPDATE quote_master SET quote_master_apply_discount = @v0  WHERE quote_id = @v1  AND revision = @v2 ", new object[] { quotediscountflag, main_id, rev });
            ASPxTextDiscount.Text = quotediscountamt.ToString();
            FillforQuote(main_id, main_rev);
        }
    }

    protected void TextVendorPartNo_Callback(object sender, CallbackEventArgsBase e)
    {
        TextVendorPartNo.DataBind();
        if (TextVendorPartNo.Items.Count > 0)
        {
            TextVendorPartNo.SelectedIndex = 0;
        }
    }

    protected void cbp_vendor_line_Callback(object sender, CallbackEventArgsBase e)
    {
        if (e.Parameter.Contains("|"))
        {
            var p = e.Parameter.Split('|');
            var type = p[0];
            var value = p[1].Trim();
            lb_new_vendor_line_error.Text = "";
            var master_id = TextPartNo.Text;
            var vendor_id = hidVendorID.Value;
            var business_unit_id = Convert.ToInt32(WarehouseBusinessUnit.id);
            switch (type)
            {
                case "vendor_info":
                    lb_new_vendor_line_error.Text = "";
                    var c_this =
                        Toolbox.doSQL_int(conn, @"SELECT COUNT(*) FROM inventory_price  WHERE master_id =@v0 AND vendor_id =@v1  AND vendor_code =@v2  AND business_unit_id =@v3 ", new object[] { master_id, vendor_id, t_new_vendor_part_number.Text.Trim(), WarehouseBusinessUnit.id });
                    if (c_this > 0)
                    {
                        lb_new_vendor_line_error.Text +=
                            "<div style='font-family:Tahoma;font-size:9pt;color:#f00;margin-left:7px;width:auto;'>* This vendor part # is already linked to part # " + master_id + ".</div>";
                    }
                    var c_other =
                        Toolbox.doSQL_dt(conn, @"SELECT
inventory_price.master_id,
inventory_description.description
FROM
inventory_price
INNER JOIN inventory_description ON inventory_price.master_id = inventory_description.master_id WHERE inventory_price.master_id !=@v0 AND vendor_id =@v1  AND vendor_code =@v2  AND business_unit_id =@v3 ", new object[] { master_id, vendor_id, t_new_vendor_part_number.Text.Trim(), WarehouseBusinessUnit.id });
                    if (c_other.Rows.Count > 0)
                    {
                        lb_new_vendor_line_error.Text +=
                            @"<div style='font-family:Tahoma;font-size:9pt;color:#f00;margin-left:7px;width:auto;'>* This vendor part # is already linked to the following part(s):
							<table cellpadding='10'>";
                        foreach (DataRow d in c_other.Rows)
                        {



                            lb_new_vendor_line_error.Text += "<tr><td width=50px><a href=\"javascript:boing('../inventory/index.aspx?a=get&tab=G&id=" + d["master_id"] +
                                                             "', 'inventory', 1280,768 )\" class='part_link'>" + d["master_id"] + "</a></td><td width=100%>" + d["description"] + "</td></tr>";

                        }

                        lb_new_vendor_line_error.Text += "</table></div>";
                    }
                    double cost_test;
                    t_new_vendor_cost.Text = double.TryParse(t_new_vendor_cost.Text, out cost_test) && cost_test > 0
                        ? cost_test.ToString()
                        : "";
                    double qty_test;
                    t_new_vendor_qty.Text = double.TryParse(t_new_vendor_qty.Text, out qty_test) && qty_test > 0
                        ? qty_test.ToString()
                        : "";
                    t_new_vendor_cost.Focus();
                    break;
                case "save":
                    if (lb_new_vendor_line_error.Text == "")
                    {
                        var inv = new inventory();
                        inv.Load(master_id, business_unit_id);
                        var vpr = new vendor_price_row();
                        vpr.cost = Convert.ToDouble(t_new_vendor_cost.Value) / Convert.ToDouble(t_new_vendor_qty.Text);
                        vpr.total = t_new_vendor_cost.Value;
                        vpr.master_id = master_id;
                        vpr.vendor_id = vendor_id;
                        vpr.vendor_code = t_new_vendor_part_number.Text.Trim();
                        vpr.qty = t_new_vendor_qty.Text;
                        vpr.member_id = current_user.id;
                        vpr.business_unit_id = business_unit_id;
                        vpr.save();
                        t_new_vendor_cost.Text = "";
                        t_new_vendor_qty.Text = "";
                    }
                    break;
            }
        }
    }

    protected void TextVendorPartNo_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
    {
        var cb = (ASPxComboBox)sender;
        var costs = new object[cb.Items.Count];
        var qtyper = new object[cb.Items.Count];
        for (var i = 0; i < cb.Items.Count; i++)
        {
            var id = cb.Items[i].Value.ToString();
            var vpr = Toolbox.doSQL_dt(conn, @"SELECT total, qty FROM inventory_price  WHERE id =@v0 limit 1 ", new object[] { id }).Rows[0];
            costs[i] = vpr["total"].ToString();
            qtyper[i] = vpr["qty"].ToString();
        }
        e.Properties["cpCOSTS"] = costs;
        e.Properties["cpQTYPER"] = qtyper;
    }

    protected void TextWorkOrder_CustomJSProperties(object sender, CustomJSPropertiesEventArgs e)
    {
        var cb = (ASPxComboBox)sender;
        var flags = new object[cb.Items.Count];
        string ids = "";

        for (var i = 0; i < cb.Items.Count; i++)
        {
            var id = cb.Items[i].Value.ToString();
            if (ids != "")
            {
                ids = ids + "," + id;
            }
            else
            {
                ids = id;
            }
        }

        var sql = $@"Select * from ((Select expense_category_id as WOProg_ID, Name AS WorkOrder, 1 as Flag  from expense_category) UNION (SELECT 9999999 AS WOProg_ID, 'ORDER FOR STOCK' AS WorkOrder, 2 as Flag) UNION (
	SELECT 
		woprog_id, 
		CONCAT(TRIM(LEADING '0' FROM woprog_bvwo),' - ', woprog_customername, ' ', LEFT(woprog_description,40), ' - ', ddl_name, ' - ',woprog_status) AS workorder ,
        3 as Flag
	FROM 
		woprog, 
		business_unit 
	WHERE 
		business_unit_id = id AND 
		
		woprog_status NOT IN ('Invoiced', 'Waiting To Be Invoiced', 'Waiting For PO', 'Waiting BM Approval', 'Waiting PM Approval', 'Waiting Approval','Deleted','Waiting Parent BM Approval') AND 
		woprog_bvwo != 'Not Entered' AND 
		woprog_associate_woprog_id = 0 AND 
		woprog_hold = 0 AND
		woprog_isrebill = 0 AND 
		woprog_iscredit = 0
	ORDER BY 
		business_unit_id, 
		woprog_customername
	)) as tb where woprog_id in ({ids})";

        var dt = Toolbox.doSQL_dt(sql);

        for (var i = 0; i < dt.Rows.Count; i++)
        {
            var id = cb.Items[i].Value.ToString();

            if (dt.Rows[i]["WOProg_ID"].ToString() == id)
            {
                flags[i] = dt.Rows[i]["Flag"];
            }

        }
        e.Properties["cpFLAG"] = flags;

    }

    protected void rb_list_Init(object sender, EventArgs e)
    {
        var g = agv;
        var pop = (ASPxPopupControl)g.FindTitleTemplateControl("pop_commit");
        var pan = (ASPxPanel)pop.FindControl("panel_pop");
    }

    protected void agv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        if (origin == "purchaseorder")
        {
            var gv = (ASPxGridView)sender;
            var is_wo = new object[gv.VisibleRowCount];
            for (var i = 0; i < gv.VisibleRowCount; i++)
            {
                var wo_n = Convert.ToInt32(gv.GetRowValues(i, "workorder"));
                is_wo[i] = wo_n < 9000000;
            }
            e.Properties["cp_is_wo"] = is_wo;
        }
    }

    protected void agv_Init(object sender, EventArgs e)
    {

        var gv = (ASPxGridView)sender;
        var o = string.IsNullOrEmpty(_q["origin"]) ? "" : _q["origin"];
        if (o == "purchaseorder" || o == "workorder")
        {
            var show_client = o == "purchaseorder";
            if (o == "purchaseorder" && !string.IsNullOrEmpty(id))
            {
                show_client = Toolbox.doSQL_int(conn, @"SELECT poprog_shipping_method FROM poprog_header  WHERE poprog_id =@v0", new object[] { _q["id"] }) == 2;
            }
            footer_save_toggle(show_client, gv);
        }
    }

    protected void gv_xfer_multiple_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        var grid = (ASPxGridView)sender;
        if (e.DataColumn.Name == "location")
        {
            var part_no = Convert.ToInt32(grid.GetRowValues(e.VisibleIndex, "part_n"));
            var location_column = (GridViewDataColumn)grid.Columns["location"];
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
                //this_ds.SelectCommand					= string.Format(@"SELECT a.id,a.qty, CONCAT(IF(b.type_id = 1, 'INT - ', 'EXT - '), b.name, ' (QTY: ', a.qty, ')') name FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = '{0}' AND a.master_id = '{1}' ORDER BY b.type_id ASC, a.qty DESC, a.min DESC, a.timestamp DESC", hidCompanyID.Value, part_no);
            }
            else
            {
                this_ds.SelectCommand =
                    string.Format(
                        @"SELECT b.id,a.qty, CONCAT(IF(b.type_id = 1, 'INT - ', 'EXT - '), IFNULL(b.name,''), ' (QTY: ', IFNULL(a.qty,0), ')') name FROM inventory_location a LEFT JOIN inventory_location_master b ON a.location_master_id = b.id WHERE a.business_unit_id = '{0}' AND a.master_id = '{1}' AND b.name IS NOT NULL ORDER BY b.type_id ASC, a.qty DESC, a.min DESC, a.timestamp DESC",
                        WarehouseBusinessUnit.id, part_no);
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
                    Session["xfer_business_unit_id"] = WarehouseBusinessUnit.id;
                    var is_ext = auth_new_location
                        ? !xfa.use_wo ? new location_master(xfa.dest_id).type_id == 2 : false
                        : !xfa.use_wo ? new location_master(xfa.dest_id).type_id == 2 : false;
                    xfer_wo_combo.Text = xfa.use_wo ? this_wo.OrderNumber + " " + this_wo.Description + "-" + this_wo.Status : "";
                    xfer_internal_combo.Text = !xfa.use_wo && !is_ext ? new location_master(xfa.dest_id).name : "";
                    xfer_external_combo.Text = !xfa.use_wo && is_ext ? new location_master(xfa.dest_id).name : "";
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
                    try
                    {
                        PartTransfer(xfer_save, null);
                        this_sb.Append("<li> Successfully transferred (" + xfa.part_id + ").");
                    }
                    catch (Exception ee)
                    {
                        this_sb.Append("<li> Could not transfer (" + xfa.part_id + ") - Reason:" + ee);
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
            if (type == "M")
            {
                // This means that there are multiple transfers that are going to happen and to prep the data.
                var xf = new DataTable("r");
                xf.Columns.Add("id", typeof(int));
                xf.Columns.Add("part_n", typeof(int));
                xf.Columns.Add("qty", typeof(double));
                xf.Columns.Add("origin", typeof(string));
                xfer_multiple.ClientVisible = true;
                xfer_single.ClientVisible = false;
                foreach (var xfa in xfer_items)
                {
                    if ( /*xfa.part_id < 900000 && */ xfa.part_id != 2139)
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
                xfer_internal_combo.DataSource =
                    Toolbox.doSQL_dt(conn, @"SELECT id,name FROM inventory_location_master  where name = '0.0.0' AND business_unit_id =@v0", new object[] { WarehouseBusinessUnit.id });
            }
            else
            {
                // This is the handler for the single line transfer
                var xfa = xfer_items[0];
                var m_id = xfa.part_id;
                xfer_single.ClientVisible = true;
                xfer_multiple.ClientVisible = false;
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
                xfer_internal_combo.ClientEnabled = m_id < 900000 && m_id != 2139;
                // && myMember.AuthenticatedForPrivilege(15, 104);
                xfer_external_combo.ClientEnabled = m_id < 900000 && m_id != 2139;
                xfer_internal_qty.ClientEnabled = m_id < 900000 && m_id != 2139; // && myMember.AuthenticatedForPrivilege(15, 104);
                xfer_external_qty.ClientEnabled = m_id < 900000 && m_id != 2139;
                xfer_wo_qty.Text = "0";
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

    protected void show_debug_column_info()
    {
        Response.Write("<hr>Addline<hr>");
        for (var i = 1; i < 27; i++)
        {
            var g = (HtmlTableCell)FindControlRecursive(Page, string.Format("add_hc{0}", i));
            Response.Write(i + "=" + g.Attributes["data-name"] + "<br/>");
        }
        Response.Write("<hr>Grid<hr>");
        for (var i = 0; i < agv.Columns.Count; i++)
        {
            Response.Write(i + "=" + agv.Columns[i].Caption + " -- Visible Index:" + agv.Columns[i].VisibleIndex + "<br/>");
        }
    }

    protected void addline_location_cb(object sender, CallbackEventArgsBase e)
    {
        Session["addline_master_id"] = e.Parameter;
        addline_location.DataBind();
    }

    private Color last_color = Color.White;

    protected void Script_error(object sender, AsyncPostBackErrorEventArgs e)
    {
        ScriptManager1.AsyncPostBackErrorMessage = e.Exception.Message;
    }

    protected void combo_location_PreRender(object sender, EventArgs e)
    {
    }

    protected void Grid_GroupSelect_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.Caption == "Qty")
        {
            var grid = (ASPxGridView)sender;
            var qty_column = (GridViewDataColumn)grid.Columns["qty"];
            var part_no = grid.GetRowValues(e.VisibleIndex, "master_id").ToString();
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

    protected void btn_use_current_cost_click(object sender, EventArgs e)
    {
        var current_status = Toolbox.doSQL_string(conn, @"CALL quote_toggle_cost(@v0,@v1)", new object[] { main_id, rev });
        btn_use_current_cost.Text = current_status == "Toggled to True" ? "Show \"As Was Prices\"" : "Show \"Today's Prices\"";
        btn_Save.Enabled = current_status != "Toggled to True";
        agv.Columns[0].Visible = current_status != "Toggled to True";
        FillforQuote(main_id, main_rev);
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

    protected void bnExport_Click(object sender, EventArgs e)
    {
        var btn = sender as ASPxButton;

        var ps = new DevExpress.XtraPrinting.PrintingSystem();
        var lnk = new DevExpress.XtraPrinting.PrintableComponentLink(ps);
        lnk.Component = ASPxGridViewExporter1;

        var compositeLink = new CompositeLink(ps);
        compositeLink.Links.AddRange(new object[] { lnk });
        compositeLink.CreateDocument();

        var stream = new MemoryStream();
        var type = string.Empty;

        compositeLink.PrintingSystem.ExportToXls(stream);
        type = "xls";
        Session["agv_ExportStreame"] = stream;
        Session["agv_export_type"] = type;

        ScriptManager.RegisterStartupScript(this, GetType(), "binder2", "please_wait('stop'); hid_btn_exporter.DoClick();", true);
        Page.Response.End();
        //	Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "hid_btn_exporter.DoClick();", true);

    }
    /*	protected void btnXlsxExport_Click(object sender, EventArgs e)
		{

			ASPxGridViewExporter1.WriteXlsToResponse();
			Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", " please_wait('stop');", true); ;
		}
	 */
    protected void btnXlsxExport_Click(object sender, EventArgs e)
    {
        // var stream = Session["agv_ExportStreame"] as MemoryStream;

        //
        // For bug 1972 : https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1972/
        //
        // Always to calculate this when sending to here.
        //
        var ps = new DevExpress.XtraPrinting.PrintingSystem();
        var lnk = new DevExpress.XtraPrinting.PrintableComponentLink(ps);
        lnk.Component = ASPxGridViewExporter1;

        var compositeLink = new CompositeLink(ps);
        compositeLink.Links.AddRange(new object[] { lnk });
        compositeLink.CreateDocument();

        var stream = new MemoryStream();
        compositeLink.PrintingSystem.ExportToXls(stream);
        //
        // End of the bug
        //        

        // var type = Session["agv_export_type"].ToString(); No need to pass this .
        WriteToResponse(agv.ID, true, "xls", stream);
    }
    protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, MemoryStream stream)
    {
        if (Page == null || Page.Response == null) return;
        var disposition = saveAsFile ? "attachment" : "inline";
        Page.Response.Clear();
        Page.Response.Buffer = false;
        Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
        Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
        Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, HttpUtility.UrlEncode(fileName).Replace("+", "%20"), fileFormat));
        Page.Response.BinaryWrite(stream.ToArray());

        ScriptManager.RegisterStartupScript(this, GetType(), "binder1", "please_wait('stop');", true); Page.Response.End();
    }


    private bool IsPOCompleted()
    {
        int status = Toolbox.doSQL_int(@"SELECT poprog_status FROM poprog_header  WHERE poprog_id = @v0",
            new object[] { main_id });

        if (status != OpsPOStatus.IssuedWaitingforPackingSlip)
        {
            return false;
        }

        // Issued-Waiting for Packing Slip

        bool allSet = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM po_details_current WHERE po_details_poprog_id = @v0 AND po_details_line_active = 1", new object[] { main_id }) == 0;
        return allSet;
    }

    /// <summary>
    /// [US 111] Negative item receipts (Purchase Order)
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private bool IsItemReceiptChangesAllowed(List<POLineItem> list, DateTime updatedDateTime)
    {
        //
        // Based on the design, the date range will be in current month. 
        //
        DateTime firstDayOfUpdateMonth = new DateTime(updatedDateTime.Year, updatedDateTime.Month, 1, 0, 0, 0);

        DateTime lastDayDateOfUpdateMonth = updatedDateTime;
        if (updatedDateTime.Month < 12)
        {
            //
            // On Jan to November...
            //

            // The first day of next month
            lastDayDateOfUpdateMonth = new DateTime(updatedDateTime.Year, updatedDateTime.Month + 1, 1, 0, 0, 0);
            // the last day of this month.
            lastDayDateOfUpdateMonth = lastDayDateOfUpdateMonth.AddSeconds(-1);
        }
        else
        {
            // On december of this year.
            lastDayDateOfUpdateMonth = new DateTime(updatedDateTime.Year, updatedDateTime.Month, 31, 23, 59, 59);
        }

        //
        // Check each of po line items.
        //
        foreach (var item in list)
        {
            // We follow the same logic from existing code to check whether will do the update based on the value from UI for each of po line item.
            if (item.po_details_qty_received_from_ui < 0)
            {
                // Receive a negative number.
                item.isChangeAllowed = this.IsNegativeItemReceiptChangesAllowedForThisPOLineItem(item, firstDayOfUpdateMonth, lastDayDateOfUpdateMonth, updatedDateTime);
            }
            else
            {
                //
                // Now the received number from UI may be ZERO or greater than ZERO.
                //
                // (1) In case that no received quantity is updated (the value from UI is ZERO), this po line item will not impact the afterwards updates.
                // (2) Positive number will not impact the netsuite side in terms of deleting the netsuite transaction record.
                //
                item.isChangeAllowed = true;
            }
        }

        var countOfNotPassingCheck = list.Where(item => item.isChangeAllowed == false).Count();
        if (countOfNotPassingCheck > 0)
        {
            return false;
        }

        // All pass the check.
        return true;
    }

    private bool IsNegativeItemReceiptChangesAllowedForThisPOLineItem(POLineItem item, DateTime firstDay, DateTime lastDay, DateTime updatedDateTime)
    {
        /*
         * 2.	When a user tries to enter in a negative item receipt, this should only be allowed if all of the following conditions are met:
            a.	There was a previous positive item receipt on this line
            b.	The previous item receipt is in the same accounting period as the current date
            c.	The previous item receipt has isMigrated set to 0
            d.	The quantity of the previous item receipt is greater than or equal to the amount being deducted

        [d] is handled by this function.
        [c] is handled in the query:   AND isMigratedFlag = 0
        [b] is hanlded in the query:   AND dateofchange BETWEEN @v3 AND @v4
        [a] is handled by the outer if condition.
         */

        //
        // b.	The previous item receipt is in the same accounting period as the current date
        // Check the 'Today' is between the account period that is not finished/closed.
        //
        // [Q] is the accounting period monthly based organized? 
        //

        //
        // Bug 2004: Unable to unreceive on PO
        // When at the last day, it may not be working due to the time part of 'end_date' is 00:00:00.
        //
        var queryForAccountingPeriod = @"
SELECT
  *
FROM
  accounting_period
WHERE is_quarter = 0
  AND is_year = 0
  AND DATE(@v0) between start_date and end_date
";
        var parametersForAccountingPeriod = new object[]
        {
            updatedDateTime
        };

        var dt_accounting_period = Toolbox.doSQL_dt(queryForAccountingPeriod, parametersForAccountingPeriod);
        if (dt_accounting_period == null || dt_accounting_period.Rows == null || dt_accounting_period.Rows.Count == 0)
        {
            return false;
        }

        //
        // Now there is at least one record in accounting_period table.
        //

        var query = @"
SELECT
  *
FROM
  poprog_part_history
WHERE poprog_id = @v0
  AND woprog_id = @v1
  AND part_no = @v2
  AND dateofchange BETWEEN @v3 AND @v4
  AND isMigratedFlag = 0
  ORDER BY part_history_id ASC;
";

        var parameters = new object[]
        {
            item.po_details_poprog_id,
            item.po_details_woprog_id,
            item.po_details_part_no,
            firstDay,
            lastDay
        };

        var dt_history = Toolbox.doSQL_dt(query, parameters);
        if (dt_history == null || dt_history.Rows == null || dt_history.Rows.Count == 0)
        {
            // If no records in the date ranges, it will not allow the update. (nothing committed in that month)
            return false;
        }

        // start and end of the numbers in the date ranges.
        double startQuantity = 0;
        double endQuantity = 0;

        // There is one record. 
        if (dt_history.Rows.Count == 1)
        {
            var row = dt_history.Rows[0];
            startQuantity = Convert.ToDouble(row["prev_qty_received"]);
            endQuantity = Convert.ToDouble(row["qty_received"]);
        }
        else
        {
            var firstRow = dt_history.Rows[0];
            var lastRow = dt_history.Rows[dt_history.Rows.Count - 1];

            startQuantity = Convert.ToDouble(firstRow["prev_qty_received"]);
            endQuantity = Convert.ToDouble(lastRow["qty_received"]);
        }

        // How many committed in that month.
        double committedNumberInUpdatingMonth = endQuantity - startQuantity;

        // ABS value from the UI.
        double absFromUI = Math.Abs(item.po_details_qty_received_from_ui);

        //
        // d.The quantity of the previous item receipt is greater than or equal to the amount being deducted
        //
        if (absFromUI <= committedNumberInUpdatingMonth)
        {
            return true;
        }

        return false;
    }

    //
    // end of [US 111] Negative item receipts (Purchase Order)
    // 

    public class PriceOption
    {
        public string businessUnitId { get; set; }
        public string businessUnit { get; set; }
        public string purchaseDate { get; set; }
        public string packageCost { get; set; }
        public string quantityPer { get; set; }
        public string part { get; set; }
        public string vendor { get; set; }
        public string associatedBUId { get; set; }
        public int currentUser { get; set; }

        public int valid { get; set; }

        public string code { get; set; }
        public string total { get; set; }

        public PriceOption()
        {
            this.valid = 0;
        }
    }

    public class MaterialInfo
    {
        public string businessUnit { get; set; }
        public string checkpart { get; set; }
        public string cost { get; set; }
        public string qtyper { get; set; }
        public int currentUser { get; set; }
        public string poID { get; set; }
    }

    public class InventoryCopyInfo
    {
        public string fromBusinessUnit { get; set; }
        public string vendor { get; set; }
        public string part { get; set; }
        public string code { get; set; }
        public string cost { get; set; }
        public string total { get; set; }
        public string qtyPer { get; set; }
        public string toBusinessUnit { get; set; }
        public int currentUser { get; set; }
    }

    public class NewInventory
    {
        public string code { get; set; }
        public string idOfCode { get; set; }
        public string cost { get; set; }
        public string qtyPer { get; set; }
        public int valid { get; set; }
        public string total { get; set; }

        public NewInventory()
        {
            this.valid = 0;
        }
    }

    [WebMethod]
    public static PriceOption CheckLowPrice(MaterialInfo materialInfo)
    {
        var po = new PriceOption();

        // Validation
        if (materialInfo == null ||
            string.IsNullOrWhiteSpace(materialInfo.checkpart) ||
            string.IsNullOrWhiteSpace(materialInfo.businessUnit) ||
            string.IsNullOrWhiteSpace(materialInfo.cost) ||
            string.IsNullOrWhiteSpace(materialInfo.poID) ||
            materialInfo.currentUser <= 0
            )
        {
            return po;
        }

        return GetLowPriceFromOtherBUsFromSameTaxEntity(materialInfo);
    }

    private static PriceOption GetLowPriceFromOtherBUsFromSameTaxEntity(MaterialInfo m)
    {
        var po = new PriceOption();
        var WorkingBusinessUnit = new NeBusinessUnit(int.Parse(m.businessUnit));
        var poprog = new NePOProg(Convert.ToInt32(m.poID));

        // Get all BUs from the same tax entity
        var queryToGetBUs = @"SELECT id FROM business_unit WHERE tax_entity_id = @v0";
        var buList = "";
        var tableForBUs = Toolbox.doSQL_dt(queryToGetBUs, new object[] { WorkingBusinessUnit.tax_entity_id });
        if (tableForBUs == null || tableForBUs.Rows == null || tableForBUs.Rows.Count == 0)
        {
            return po;
        }

        foreach (DataRow r in tableForBUs.Rows)
        {
            var id = r["id"].ToString();
            buList = buList == "" ? id : (buList + "," + id);
        }

        // The vendor part number check should only be checking within the last month
        string today = DateTime.Now.AddDays(1).ToString();
        string theDayBeforeOneMonth = DateTime.Now.AddMonths(-1).ToString();

        // Check to see this only work on items greater than a penny
        if (Convert.ToDouble(m.cost) <= 0.01)
        {
            return po;
        }

        // Get lower price from other BUs
        var query = @"
SELECT
	a.master_id,
	a.id,
	UPPER(a.vendor_code) CODE,
	a.cost,
	a.qty quantityPer,
	a.business_unit_id,
	b.ddl_Name name,
	a.vendor_id,
	a.total,
	e.dateofchange
FROM
	inventory_price a
INNER JOIN business_unit b ON 
	b.id = a.business_unit_id
INNER JOIN po_details_current c ON 
	c.po_details_part_no = a.master_id AND 
    c.po_details_vendor_part_no = a.vendor_code
INNER JOIN poprog_header d ON 
	d.poprog_id = c.po_details_poprog_id AND 
    d.business_unit_id = a.business_unit_id
INNER JOIN poprog_part_history e ON 
	e.poprog_id = c.po_details_poprog_id AND 
    e.part_no = a.master_id AND 
    e.vendor_part_no = a.vendor_code
WHERE 
	a.vendor_id = @v0 AND 
    a.master_id = @v1 AND 
    a.total < @v2 AND 
    FIND_IN_SET(a.business_unit_id, @v3) AND 
    (e.dateofchange >= @v4 AND e.dateofchange <= @v5) AND 
    a.business_unit_id != @v6
ORDER BY 
	cost ASC, e.dateofchange DESC
LIMIT 1";

        var table = Toolbox.doSQL_dt(query, new object[]
        {
            poprog.poprog_vendor_id,
            m.checkpart,
            m.cost,
            buList,
            theDayBeforeOneMonth,
            today,
            m.businessUnit
        });

        if (table == null || table.Rows == null || table.Rows.Count == 0)
        {
            return po;
        }

        var row = table.Rows[0];
        po.valid = 1;
        po.part = m.checkpart;
        po.businessUnitId = row["business_unit_id"].ToString();
        po.businessUnit = row["Name"].ToString();
        po.packageCost = row["cost"].ToString();
        po.purchaseDate = row["dateofchange"].ToString();
        po.quantityPer = row["quantityPer"].ToString();
        po.code = row["CODE"].ToString();
        po.currentUser = m.currentUser;
        po.vendor = poprog.poprog_vendor_id.ToString();
        po.associatedBUId = m.businessUnit;
        po.total = row["total"].ToString();

        return po;
    }

    [WebMethod]
    public static NewInventory CopyInventoryItem(InventoryCopyInfo inventoryCopyInfo)
    {
        NewInventory inventory = new NewInventory();

        if (inventoryCopyInfo == null ||
            string.IsNullOrWhiteSpace(inventoryCopyInfo.fromBusinessUnit) ||
            string.IsNullOrWhiteSpace(inventoryCopyInfo.part) ||
            string.IsNullOrWhiteSpace(inventoryCopyInfo.toBusinessUnit))
        {
            return inventory;
        }

        inventory = CopyItem(inventoryCopyInfo);
        return inventory;
    }


    private static NewInventory CopyItem(InventoryCopyInfo inventoryCopyInfo)
    {
        NewInventory inventory = new NewInventory();
        var vpr = new vendor_price_row();
        vpr.cost = Convert.ToDouble(inventoryCopyInfo.cost); // / Convert.ToDouble(inventoryCopyInfo.qtyPer);
        vpr.total = Convert.ToDouble(inventoryCopyInfo.total);
        vpr.master_id = inventoryCopyInfo.part;
        vpr.vendor_id = inventoryCopyInfo.vendor;
        vpr.vendor_code = inventoryCopyInfo.code;
        vpr.qty = inventoryCopyInfo.qtyPer;
        vpr.member_id = inventoryCopyInfo.currentUser;
        vpr.business_unit_id = Convert.ToInt32(inventoryCopyInfo.toBusinessUnit);

        var result = vpr.save();
        if (!result)
        {
            inventory.valid = 0;
            return inventory;
        }

        var item = new vendor_price_row(inventoryCopyInfo.part, inventoryCopyInfo.toBusinessUnit, inventoryCopyInfo.vendor);
        inventory.valid = 1;
        inventory.cost = inventoryCopyInfo.cost;
        inventory.idOfCode = item.price_id.ToString();
        inventory.code = inventoryCopyInfo.code;
        inventory.qtyPer = inventoryCopyInfo.qtyPer;
        inventory.total = inventoryCopyInfo.total;

        return inventory;
    }


    private class POLineItem
    {
        public int po_details_id { get; set; }
        public double po_details_qty_ordered { get; set; }
        public double po_details_qty_received { get; set; }
        public double po_details_qty_received_from_ui { get; set; }
        public double sum { get; set; }

        public bool isChangeAllowed { get; set; }
        public int po_details_poprog_id { get; set; }
        public int po_details_woprog_id { get; set; }
        public int po_details_part_no { get; set; } // 9595, 52001...
    }

    class PositiveNegativeException : Exception
    {
        public PositiveNegativeException()
        {
        }

        public PositiveNegativeException(string message)
            : base(message)
        {
        }

        public PositiveNegativeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}