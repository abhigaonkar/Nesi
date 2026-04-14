using System;
using System.Data;
using NESI.Common.Models;
using nesi.core;

public partial class sections_workorder_WorkSheetComparison : System.Web.UI.Page
{
	string quoteno;
	Toolbox _Tools;
    protected void Page_Load(object sender, EventArgs e)
    {
        _Tools = new Toolbox();
        var woid = Request.QueryString["woid"];
		var progress = new NeWOProg(Convert.ToInt32(woid));
		quoteno = progress.QuoteID;
        lblWSMessage.Text = "";
        if (quoteno.Length > 7)
        {
            lblWSMessage.Text = "Old Quote Not Available";
            return;
        }

		if (!IsPostBack)
		{
			Session["wo_comparison"] = null;
		}

		fill_grid();

		/*
        ASPxGridWSList.DataSource = ws;
        ASPxGridWSList.DataBind();

        ASPxGridWOList.DataSource = wo;
        ASPxGridWOList.DataBind();
        wo.Dispose();
        ws.Dispose();
 */ 
    }
	protected void fill_grid()
	{

		if (Session["wo_comparison"] == null)
		{

			var woid = Request.QueryString["woid"];
			var progress = new NeWOProg(Convert.ToInt32(woid));
			_Tools = new Toolbox();
			var sqlwo = "";
			var quoteid = quoteno.Substring(0, 6);
			var quoterev = quoteno.Substring(6, 1);
			if ((progress.Status == OpsWOStatus.Invoiced || progress.Status == OpsWOStatus.WaitingToBeInvoiced))
			{
				sqlwo = "SELECT wo_detail_history_master_id AS 'master_id', wo_detail_history_description AS description, SUM(wo_detail_history_qty_committed) AS on_wo, 0.00 as on_ws, wo_detail_history_price_sell as WO_Sell_Price ,0.00 as EXTD_From_ws, 0.00 as spread FROM wo_detail_history WHERE wo_detail_history_woprog_id = " + progress.woprog_id + " AND wo_detail_history_type != 'Q' AND wo_detail_history_master_id != 0 GROUP BY wo_detail_history_master_id ORDER BY wo_detail_history_master_id";
			}
			else
			{
				sqlwo = "SELECT wo_detail_current_master_id AS 'master_id', wo_detail_current_description AS description, SUM(wo_detail_current_qty_committed) AS on_wo, 0.00 as on_ws, wo_detail_current_price_sell as WO_Sell_Price ,0.00 as EXTD_From_ws, 0.00 as spread FROM wo_detail_current WHERE wo_detail_current_woprog_id = " + progress.woprog_id + " AND wo_detail_current_type != 'Q' AND wo_detail_current_master_id != 0 GROUP BY wo_detail_current_master_id ORDER BY wo_detail_current_master_id";
			}
			var wo = _Tools.getSQL_datatable(sqlwo  , null);
			var skip_row = false;
			var sqlws = "SELECT part_no AS 'master_id', description AS description,0.00 as on_wo, SUM(qty) AS on_ws, 0.00 as WO_Sell_Price ,(sum(Extended_Per)/sum(Qty)) as EXTD_From_ws , 0.00 spread FROM quote_worksheet WHERE quote_id = " + quoteid + "  AND revision = " + quoterev + " and part_no <> '' GROUP BY part_no ORDER BY part_no + 0";
			var ws = _Tools.getSQL_datatable(@"SELECT part_no AS 'master_id', description AS description,0.00 as on_wo, SUM(qty) AS on_ws, 0.00 as WO_Sell_Price ,(sum(Extended_Per)/sum(Qty)) as EXTD_From_ws , 0.00 spread FROM quote_worksheet  WHERE quote_id =@v0 AND revision =@v1  and part_no <> '' GROUP BY part_no ORDER BY part_no + 0 ", new object[] { quoteid,quoterev });
			foreach (DataRow dr in ws.Rows)  // loop through every line on the quote and find it on the quote
			{
				foreach (DataRow dr_wo in wo.Rows)
				{
				//	dr_wo[6] = 0.0;
					if (dr_wo[0].ToString().Equals(dr[0].ToString()))
					{
						dr_wo[3] = dr[3];
						dr_wo[5] = Convert.ToDecimal(dr[5]);
						
						dr_wo[6] = Math.Round((Convert.ToDouble(dr_wo[2]) * Convert.ToDouble(dr_wo[4])) - (Convert.ToDouble(dr[3]) * Convert.ToDouble(dr[5])),2);
						skip_row = true;
					}
				}
				if (skip_row == false)
				{
					
					wo.ImportRow(dr);
				}
				skip_row = false;
			}
			Session["wo_comparison"] = wo;
		}
		ASPxGridView1.DataSource = Session["wo_comparison"];
		ASPxGridView1.DataBind();
	}
}
