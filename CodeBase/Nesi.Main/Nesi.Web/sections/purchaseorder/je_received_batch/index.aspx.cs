using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_purchaseorder_je_received_batch_index : System.Web.UI.Page
{
//	Toolbox _tools;
//	NeMember current_user;
//	static int _page_id = 217;
//	static string _page_name = "AP_JE_Batch";
//	ASPxHiddenField h;
//	SqlDataSource ds_templates;
//	ASPxDropDownEdit dde_filter;
//	Panel panel_export;

//	protected void Page_Init(object sender, EventArgs e)
//	{
//		_tools = new Toolbox();
//		current_user = Toolbox.do_handle_authentication(_page_id);
	
//		layout.__page_name = _page_name;
//		h = (ASPxHiddenField)layout.FindControl("h");
//		ds_templates = (SqlDataSource)layout.FindControl("ds_templates");
//		dde_filter = (ASPxDropDownEdit)layout.FindControl("dde_filter");
//		panel_export = (Panel)layout.FindControl("panel_export");
//		panel_export.Visible = true;
        
//		layout.used_gv = gv;
//		h.Set("gridview_id", "gv");
//		ds_templates.SelectParameters["@page_name"].DefaultValue = _page_name;
//		ds_templates.SelectParameters["@member_id"].DefaultValue = current_user.id.ToString();
//	}
//	protected void Page_Load(object sender, EventArgs e)
//	{
//		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
//		lbltemp.Text = "AP Received Batch JE";
//        if (!IsCallback && !IsPostBack)
//        {
//            ddl_company.Value = current_user.business_unit_id;
//            var menu = new NeMenu(current_user, Convert.ToInt32(_page_id));
//            divMenu.InnerHtml = menu.MenuHTML;
//            var gl = new NeGridLayouts(current_user.id, _page_name);
//            if (gl.GridLayoutID == 0)
//            {
//                gv.FilterExpression = "";
//                gl.GridLayout_Layout = gv.SaveClientLayout();
//                gl.member_id = current_user.id;
//                gl.GridLayout_Name = "Default";
//                gl.GridLayout_Gridid = _page_name;
//                gl.SaveGridLayout();

//                h.Set("ID", gl.GridLayoutID);
//                h.Set("NAME", gl.GridLayout_Name);
//            }
//            else
//            {
//                gv.LoadClientLayout(gl.GridLayout_Layout);
//                h.Set("ID", gl.GridLayoutID);
//                h.Set("NAME", gl.GridLayout_Name);
//            }
//            dde_filter.Text = gl.GridLayout_Name;
//            Session["rpt_ap_received_je_batch"] = null;
//        }
//	    Sql_companies.SelectCommand = string.Format("CALL get_visible_business_units({0})", current_user.id);
//        fill_grid();
//        layout.export_filename = "ap_batch_" + System.DateTime.Today.ToString("yyyyMMdd") + "_" + ddl_company.Text;
//	}
//	protected void fill_grid()
//	{
		
//	//	gv.DataBind();
//        if (Session["rpt_ap_received_je_batch"] == null)
//        {

//            var dt_bv = _tools.getSQL_datatable(@"select acct_no,j_date,trans_no,debit_amt,credit_amt,gl_currency,tran_date,BVGLMEMOWHO from GL_TRANSACTIONS  where tran_date>=?", new NeBusinessUnit(ddl_company.Value).DSN, new object[] { System.DateTime.Today.AddDays(-5).ToString("yyyyMMdd") });

//           var dt = _tools.getSQL_datatable(string.Format(@"
//SELECT
//    poprog_header.business_unit_id,
//    CONCAT(
//        'N',
//        poprog_part_history.part_history_id
//    ) AS trans_no,
//    poprog_part_history.dateofchange AS transaction_date,
//    CONCAT(
//        CONCAT(
//            'N',
//            poprog_part_history.part_history_id
//        ),
//        '-PO ',
//        poprog_header.poprog_id,
//        '-',
//        po_details_current.po_details_rec_no,
//        ' Received ',
//        poprog_part_history.diff_qty_received
//    ) memo,
//    IF (
//        poprog_part_history.diff_qty_received * poprog_part_history.cost >= 0,
//        IF(
//            po_details_current.po_details_woprog_id = 9999999,
//            '13015',
//            IF(
//                po_details_current.is_gl_account = false,
//                '13015',
//                gl_te.account_no
//            )
//        ),
//        '13315'
//    ) gl_account,
//    poprog_part_history.diff_qty_received * poprog_part_history.cost debit,
//    0 credit,
//    poprog_part_history.reference_no po_detail_id,
//    poprog_header.poprog_country_code_currency != business_unit.default_currency diff_currency,
//    0 in_bv,
//    CONCAT(
//        CONCAT(
//            'N',
//            poprog_part_history.part_history_id
//        ),
//        '-PO '
//    ) _id
//FROM
//    poprog_part_history
//    INNER JOIN poprog_header
//        ON poprog_part_history.poprog_id = poprog_header.poprog_id
//    INNER JOIN business_unit
//        ON business_unit.id = poprog_header.business_unit_id
//    LEFT JOIN woprog
//        ON poprog_part_history.woprog_id = woprog.woprog_id and poprog_part_history.is_gl_account = false
//    INNER JOIN po_details_current
//        ON po_details_current.po_details_poprog_id = poprog_header.poprog_id
//        AND poprog_part_history.reference_no = po_details_current.po_details_id
//    LEFT JOIN gl_te
//        ON gl_te.id = po_details_current.po_details_woprog_id and po_details_current.is_gl_account = true
//WHERE poprog_part_history.dateofchange >= CURDATE() - INTERVAL 5 DAY
//    AND poprog_header.business_unit_id = {0}
//    AND diff_qty_received != 0
//UNION
//SELECT
//    poprog_header.business_unit_id,
//    CONCAT(
//        'N',
//        poprog_part_history.part_history_id
//    ) AS trans_no,
//    poprog_part_history.dateofchange AS transaction_date,
//    CONCAT(
//        CONCAT(
//            'N',
//            poprog_part_history.part_history_id
//        ),
//        '-PO ',
//        poprog_header.poprog_id,
//        '-',
//        po_details_current.po_details_rec_no,
//        ' Received ',
//        poprog_part_history.diff_qty_received
//    ) memo,
//    IF (
//        poprog_part_history.diff_qty_received * poprog_part_history.cost < 0,
//        IF(
//            po_details_current.po_details_woprog_id = 9999999,
//            '13015',
//            IF(
//                po_details_current.is_gl_account = false,
//                '13015',
//                gl_te.account_no
//            )
//        ),
//        '13315'
//    ) gl_account,
//    0 debit,
//    poprog_part_history.diff_qty_received * poprog_part_history.cost credit,
//    poprog_part_history.reference_no po_detail_id,
//    poprog_header.poprog_country_code_currency != business_unit.default_currency diff_currency,
//    0 in_bv,
//    CONCAT(
//        CONCAT(
//            'N',
//            poprog_part_history.part_history_id
//        ),
//        '-PO '
//    ) _id
//FROM
//    poprog_part_history
//    INNER JOIN poprog_header
//        ON poprog_part_history.poprog_id = poprog_header.poprog_id
//    INNER JOIN business_unit
//        ON business_unit.id = poprog_header.business_unit_id
//    LEFT JOIN woprog
//        ON poprog_part_history.woprog_id = woprog.woprog_id and poprog_part_history.is_gl_account = false
//    INNER JOIN po_details_current
//        ON po_details_current.po_details_poprog_id = poprog_header.poprog_id
//        AND poprog_part_history.reference_no = po_details_current.po_details_id
//    LEFT JOIN gl_te
//        ON gl_te.id = po_details_current.po_details_woprog_id and po_details_current.is_gl_account = true
//WHERE poprog_part_history.dateofchange >= CURDATE() - INTERVAL 5 DAY
//    AND poprog_header.business_unit_id = {0}
//    AND diff_qty_received != 0
//ORDER BY po_detail_id,
//    transaction_date,
//    ABS(debit) DESC
//", ddl_company.Value),null);
//            foreach (DataRow dr in dt.Rows)
//            {
//                if (dt_bv.Select("trim(acct_no)='" + dr["gl_account"].ToString().Trim() + "' and BVGLMEMOWHO = '" + dr["_id"] + "' and tran_date='" + Convert.ToDateTime(dr["transaction_date"]).ToString("yyyyMMdd") + "' and debit_amt=" + dr["debit"] + " and credit_amt=" + dr["credit"]).Length > 0)
//                {
//                    dr.BeginEdit();
//                    dr["in_bv"] = 1;
//                    dr.AcceptChanges();

//                }
//            }

//            Session["rpt_ap_received_je_batch"] = dt;
//            dt = null;
//            dt_bv = null;
//        }
//        gv.DataSource = Session["rpt_ap_received_je_batch"];
//        gv.DataBind();
        
//	}


//	protected void gv_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
//	{
//		var gv = (ASPxGridView)sender;
//		e.Properties["cpExp"] = gv.SaveClientLayout();
//	}
//	protected void gv_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
//	{
//		var gv = (ASPxGridView)sender;
//		if (e.Parameters != "")
//		{
//			gv.LoadClientLayout(e.Parameters);
//		}
//		else
//		{
//			gv.FilterExpression = "";
//			for (var i = 0; i < gv.Columns.Count; i++)
//			{
//				if (gv.Columns[i] is GridViewDataColumn)
//				{
//					var col = (GridViewDataColumn)gv.Columns[i];
//					if (col.GroupIndex > -1)
//					{
//						gv.UnGroup(col);
//					}
//					col.Visible = true;
//				}
//			}
//		}
//	}





   
//    protected void ddl_company_SelectedIndexChanged(object sender, EventArgs e)
//    {
// Session["rpt_ap_received_je_batch"] = null;
//        fill_grid();
//        layout.export_filename = "ap_batch_" + System.DateTime.Today.ToString("yyyyMMdd") + "_" + ddl_company.Text;
//    }
}