using System;
using System.Data;
using System.Linq;
using DevExpress.Web;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NESI.Common.Models;
using nesi.core;

public partial class sections_workorder_modules_bv_post : System.Web.UI.UserControl
{	

double _addpocost;
double _addposell;

			 


public NeMember _current_user { get; set; }
public NeWOProg _wo { get; set; }

bool IncludeOpenPOs;


    public NeBusinessUnit _bu;
	public int woprog_id { get; set; }
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!Visible) return;
		
	}

	public void load()
	{
	var _q = Request.QueryString;
	hdn_woid.Value = woprog_id.ToString();

	if(woprog_id == 0 && _q["woprog_id"] != "0")
		{
		int try_woprog_id;
		int.TryParse(_q["woprog_id"], out try_woprog_id);
		woprog_id = try_woprog_id;
		_wo       = new NeWOProg(woprog_id);
		_bu       = new NeBusinessUnit(_wo.business_unit_id);

		}
	    if ((woprog_id == 0) && (_wo.woprog_id != 0))
	        {
	        woprog_id = _wo.woprog_id;
			   _bu = new NeBusinessUnit(_wo.business_unit_id);
	        }
	    if (woprog_id != 0)
        { 
            populate_analysis();
		}
	}


	protected void populate_analysis()
	{
		


        using (var conn	= Toolbox.connect())
        {
			
            div_requires_wo_copy.Visible = Toolbox.doSQL_int(conn, @"Select requires_wo_copy from customer  where customer_id =@v0 limit 1 ", new object[] { _wo.WOProg_Customer_ID }) == 1;




            #region old posting notification method
            #region variable definition
            var tablename = "";
            var i = 0;
            var int_count_regular = 0;
            var int_count_job_cost = 0;
            var int_count_invis_cred = 0;
            var int_count_quote_price = 0;
            var int_count_blended = 0;
            var int_count_commnet = 0;
            var int_count_dnc = 0;
            var int_count_blank = 0;
            double dbl_blended = 0;
            double dbl_blended_lab = 0;
            double dbl_blended_mat = 0;
            double dblms_sub_total = 0;
            double dbl_invis = 0;
            double dbl_invis_lab = 0;
            double dbl_invis_mat = 0;
            double dbl_jct_otal = 0;
            double dbl_quote_sub_total = 0;
            double dbl_quote_sub_total_2 = 0;

            double dbl_labour = 0;
            double dbl_mat_to_be_billed = 0;
            double dbl_tmlabour_cost = 0;
            double dbl_tmlabour_sell = 0;
            double dbl_q_labour_cost = 0;
            double dbl_q_material_cost = 0;
            double dbl_q_sell = 0;
            double dbl_q_sell_2 = 0;
            double dbl_tmmat_cost = 0;
            double dbl_tmmat_sell = 0;
            double dbl_progress_bill = 0;
            double dbl_progress_bill_2 = 0;
            double dbl_dnc = 0;
            double dbl_dnc_lab = 0;
            double dbl_dnc_mat = 0;
            double dbl_dni = 0;
            double dbl_dni_lab = 0;
            double dbl_dni_mat = 0;
            var jc_lab = 0.00;
            double jc_mat = 0;
            double dbl_vis_cred = 0;
            double dbl_vis_cred_lab = 0;
            double dbl_vis_cred_mat = 0;
            double true_cost = 0;
            double true_total_quote_cost = 0;
            double true_sell = 0;
            double true_sell_b = 0;
            double disc_sell = 0;
            double benchmark_sell = 0;
            double quote_jobcost = 0;
            var z = 1;
            tablename = "history";
            var linked_wos = Toolbox.doSQL_dt(conn, @"CALL associated_workorders_new(@v0 , 0)", new object[] { _wo.woprog_id });
            //		chk_uselinked.Visible = linked_wos.Rows.Count > 0;
            if (_wo.woprog_associate_woprog_id != 0)
            {
                uc_accounting_notes.woprog_id = _wo.woprog_associate_woprog_id;
                uc_accounting_notes.DataBind();
            }
            else
            {
                uc_accounting_notes.Visible = false;
            }
            var where_clause = "";
            #endregion variable definition


            var new_total = new NeSalesOrder();
            var success = new_total.getSalesOrderValues(_wo.woprog_id.ToString());
            if (IncludeOpenPOs)
            {
                #region addpocost definition
                _addpocost = Toolbox.doSQL_double(conn, @" SELECT IFNULL(SUM((po_details_qty_ordered-a.po_details_qty_received)*a.po_details_cost), 0) FROM po_details_current a INNER JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_line_active = 1 AND a.po_details_woprog_id = @v0 and a.is_gl_account = false  AND b.poprog_status > 2 AND b.poprog_status < 7", new object[] { _wo.woprog_id });
                #endregion addpocost definition
                #region addposell definition
                if (_wo.use_fixed_material_markup)
                {
                    _addposell = Toolbox.doSQL_double(conn, @" SELECT IFNULL(SUM(a.po_details_cost * @v1 * (po_details_qty_ordered-a.po_details_qty_received)), 0) FROM po_details_current a INNER JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_line_active = 1 AND a.po_details_woprog_id = @v0 and a.is_gl_account = false  AND b.poprog_status > 2 AND b.poprog_status < 7", new object[] { _wo.woprog_id, _wo.fixed_material_markup });
                }
                else
                {
                    _addposell = Toolbox.doSQL_double(conn, @" SELECT IFNULL(SUM(GETSELLPRICE(a.po_details_cost,0,true,a.po_details_qty_ordered,a.business_unit_id)*(po_details_qty_ordered-a.po_details_qty_received)), 0) FROM po_details_current a INNER JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_line_active = 1 AND a.po_details_woprog_id = @v0 and a.is_gl_account = false  AND b.poprog_status > 2 AND b.poprog_status < 7", new object[] { _wo.woprog_id });
                }
                #endregion addposell definition
            }
            // is this a progress bill?  if so, find all the others and find the parent.
            var ids = new List<int>();
            var idsb = new List<int>();
            ids.Add(_wo.woprog_id);
            idsb.Add(_wo.woprog_id);
            double total_labor_wo_cost = 0;
            double total_material_wo_cost = 0;
            string benchmark_whereclause;
            for (z = 1; z <= 2; z++) // Once through history, then through current.
            {
                where_clause = "";
                benchmark_whereclause = "";

                where_clause = string.Join(",", ids.ToArray());
                benchmark_whereclause = string.Join(",", idsb.ToArray());

                #region build wo_lines data table
                var wo_lines_sql = string.Format(@"
SELECT 
	wo_detail_{0}_id id,
	wo_detail_{0}_billtypeid billtype_id,
	wo_detail_{0}_code code,
	wo_detail_{0}_master_id master_id,
	wo_detail_{0}_qty_committed qty_committed,
	wo_detail_{0}_type type,
	wo_detail_{0}_price_sell* (1-(wo_detail_{0}_discount/100)) price_sell, 
	(wo_detail_{0}_qty_committed * wo_detail_{0}_price_sell * (1-(wo_detail_{0}_discount/100))) AS total_price, 
	'1' AS HISTORY, 
	(wo_detail_{0}_qty_committed * wo_detail_{0}_price_cost) as total_cost, 
	wo_detail_{0}_discount as discount, 
	wo_detail_{0}_woprog_id as woprog_id, 
	wo_detail_{0}_rec_no as rec_no 
FROM 
	wo_detail_{0} a
LEFT JOIN 
	wo_detail_lineitem_billtype b 
		ON a.wo_detail_{0}_billtypeid = b.wo_lineitem_billtypeid
WHERE 
	wo_detail_{0}_woprog_id IN ({1})
ORDER BY 
	wo_detail_{0}_rec_no",
                             tablename,         // {0}
                             where_clause       // {1}
                             );
                var wo_lines = Toolbox.doSQL_dt(conn, wo_lines_sql, null);
                #endregion build wo_lines data table
                foreach (DataRow wo_line in wo_lines.Rows)
                {
                    i++;
                    var id = wo_line["id"];
                    var woprog_id = wo_line["woprog_id"];
                    var rec_no = wo_line["rec_no"];
                    var this_billtype_id = Convert.ToInt32(wo_line["billtype_id"]);
                    var this_qty_committed = Convert.ToDouble(wo_line["qty_committed"]);
                    var this_price_sell = Convert.ToDouble(wo_line["price_sell"]);
                    var this_code = wo_line["code"].ToString().ToLower();
                    var this_type = wo_line["type"].ToString();
                    var this_total_cost = Convert.ToDouble(wo_line["total_cost"]);
                    var this_master_id = Convert.ToInt32(wo_line["master_id"]);
                    var this_discount = Convert.ToDouble(wo_line["discount"]);
                    total_labor_wo_cost += this_type == "L" ? this_total_cost : 0;
                    total_material_wo_cost += this_type == "M" ? this_total_cost : 0;
                    this_discount = (100 - this_discount) / 100;
                    var is_credit_or_rebill = _wo.woprog_iscredit > 0 || _wo.woprog_isrebill > 0;
                    switch (this_billtype_id)
                    {
                        #region 0 - Regular
                        case 0:
                            int_count_regular++;
                            if (this_type == "L")
                            {
                                dbl_labour += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3) * this_discount, 2);
                                dbl_tmlabour_cost += _wo.woprog_iscredit > 0 || _wo.woprog_isrebill > 0 ? 0 : Toolbox.do_Round(this_total_cost, 2);
                                dbl_tmlabour_sell += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3), 2);
                            }
                            else
                            {
                                dbl_mat_to_be_billed += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3) * this_discount, 2);
                                dbl_tmmat_cost += is_credit_or_rebill ? 0 : Toolbox.do_Round(this_total_cost, 2);
                                dbl_tmmat_sell += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3), 2);
                            }
                            dblms_sub_total += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3) * this_discount, 2);
                            break;
                        #endregion 0 - Regular
                        #region 1 - Jobcost for Quote
                        case 1:
                            int_count_job_cost++;
                            if (this_type == "L")
                            {
                                jc_lab += this_qty_committed * this_price_sell;
                                dbl_q_labour_cost += this_total_cost;
                            }
                            else
                            {
                                jc_mat += this_qty_committed * this_price_sell;
                                dbl_q_material_cost += this_total_cost;
                            }
                            dbl_jct_otal += this_qty_committed * this_price_sell;
                            break;
                        #endregion 1 - Jobcost for Quote
                        #region 2 - Invisible Credit
                        case 2:
                            int_count_invis_cred++;
                            if (this_type == "L")
                            {
                                dbl_invis_lab += this_qty_committed * this_price_sell;
                            }
                            else
                            {
                                dbl_invis_mat += this_qty_committed * this_price_sell;
                                dbl_tmmat_cost += is_credit_or_rebill ? 0 : this_total_cost;
                                dbl_tmmat_sell += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3), 2);
                            }
                            dbl_invis += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3), 2);
                            dblms_sub_total += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3), 2);
                            break;
                        #endregion 2 - Invisible Credit
                        #region 3 - Quoted Price
                        case 3:
                            int_count_quote_price++;
                            dbl_quote_sub_total += this_qty_committed * this_price_sell;
                            dbl_q_sell += this_qty_committed * this_price_sell;

                            dbl_q_material_cost += this_total_cost;
                            break;
                        #endregion 3 - Quoted Price
                        #region 4 - Blended
                        case 4:
                            int_count_blended++;
                            if (this_type == "L")
                            {
                                dbl_blended_lab += this_qty_committed * this_price_sell;
                            }
                            else
                            {
                                dbl_blended_mat += this_qty_committed * this_price_sell;
                            }
                            dbl_blended += this_qty_committed * this_price_sell;
                            break;
                        #endregion 4 - Blended
                        #region 5 - Do Not Include
                        case 5:
                            if (this_type == "L")
                            {
                                dbl_dni_lab += this_qty_committed * this_price_sell;
                                dbl_tmlabour_cost += _wo.woprog_iscredit > 0 || _wo.woprog_isrebill > 0 ? 0 : this_total_cost;
                            }
                            else
                            {
                                dbl_dni_mat += this_qty_committed * this_price_sell;
                                dbl_tmmat_cost += is_credit_or_rebill ? 0 : this_total_cost;
                                dbl_tmmat_sell += this_qty_committed * this_price_sell;
                            }
                            dbl_dni += this_qty_committed * this_price_sell;
                            break;
                        #endregion 5 - Do Not Include
                        #region 6 - Comment
                        case 6:
                            int_count_commnet++;
                            break;
                        #endregion 6 - Comment
                        #region 7 - Visible No Charge
                        case 7:
                            int_count_dnc++;
                            if (this_type == "L")
                            {
                                dbl_dnc_lab += this_qty_committed * this_price_sell;
                                dbl_tmlabour_cost += is_credit_or_rebill ? 0 : this_total_cost;
                            }
                            else
                            {
                                dbl_dnc_mat += this_qty_committed * this_price_sell;
                                dbl_tmmat_cost += is_credit_or_rebill ? 0 : this_total_cost;
                            }
                            dbl_dnc += this_qty_committed * this_price_sell;
                            break;
                        #endregion 7 - Visible No Charge
                        #region 8 - Blank
                        case 8:
                            int_count_blank++;
                            break;
                        #endregion 8 - Blank
                        #region 9 - Progress Billing
                        case 9:
                            dbl_progress_bill += this_qty_committed * this_price_sell;
                            break;
                        #endregion 9 - Progress Billing
                        #region 10 - Visible Credit
                        case 10:
                            if (this_type == "L")
                            {
                                dbl_vis_cred_lab += this_qty_committed * this_price_sell;
                                dbl_tmlabour_cost += _wo.woprog_iscredit > 0 || _wo.woprog_isrebill > 0 ? 0 : this_total_cost;
                                dbl_tmlabour_sell += this_qty_committed * this_price_sell;
                            }
                            else
                            {
                                dbl_vis_cred_mat += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3), 2);
                                dbl_tmmat_cost += is_credit_or_rebill ? 0 : this_total_cost;
                                dbl_tmmat_sell += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3), 2);
                            }
                            dbl_vis_cred += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3), 2);
                            dblms_sub_total += Toolbox.do_Round(Toolbox.do_Round(this_qty_committed * this_price_sell, 3), 2);
                            break;
                        #endregion 10 - Visible Credit
                        case 11:  // quoted new
                            dbl_q_material_cost += this_total_cost;
                            dbl_q_sell_2 += this_qty_committed * this_price_sell;

                            dbl_quote_sub_total_2 += this_qty_committed * this_price_sell;
                            break;
                        case 12:  // progress bill new
                            dbl_q_material_cost += this_total_cost;
                            dbl_progress_bill_2 += this_qty_committed * this_price_sell;
                            break;

                    }
                }
                #region true_cost definition
                true_cost += Toolbox.doSQL_double(conn, string.Format(@"
SELECT
	IFNULL(SUM(wo_detail_{0}_qty_committed * wo_detail_{0}_price_cost),0)
FROM
	wo_detail_{0}
WHERE
	wo_detail_{0}_woprog_id IN ({1}) AND 
	(wo_detail_{0}_billtypeid NOT IN (3,6,8) ) ", tablename, where_clause), null);
                #endregion true_cost definition

                #region true_quote_total_cost definition
                /*	true_total_quote_cost += Toolbox.do_double(string.Format(@"
SELECT
	IFNULL(SUM(wo_detail_{0}_qty_committed * wo_detail_{0}_price_cost),0)
FROM
	wo_detail_{0}
WHERE
	wo_detail_{0}_woprog_id IN ({1}) AND 
	(wo_detail_{0}_billtypeid NOT IN (3,6,8,9,11,12) ) ", tablename, where_clause));
			*/
                true_total_quote_cost = _wo.woprog_materialcost + _wo.woprog_labourcost;

                #endregion true_quote_total_cost definition

                #region true_sell definition

                true_sell += Toolbox.doSQL_double(conn, string.Format(@"
SELECT
	IFNULL(sum(round((round(wo_detail_{0}_qty_committed * wo_detail_{0}_price_sell,3) *((100-wo_detail_{0}_discount)/100)),2)),0)
FROM
	wo_detail_{0}
WHERE
	wo_detail_{0}_woprog_id IN ({2}) AND 
	wo_detail_{0}_billtypeid IN (0,2,3,10,11,12)", tablename, _wo.woprog_id, where_clause), null);


                true_sell_b += Toolbox.doSQL_double(conn, string.Format(@"
SELECT
	IFNULL(sum(round((round(wo_detail_{0}_qty_committed * wo_detail_{0}_price_sell,3) *((100-wo_detail_{0}_discount)/100)),2)),0)
FROM
	wo_detail_{0}
WHERE
	wo_detail_{0}_woprog_id IN ({2}) AND 
	wo_detail_{0}_billtypeid IN (0,2,3,10,11)", tablename, _wo.woprog_id, benchmark_whereclause), null);
                #endregion true_sell definition
                #region disc_sell definition
                disc_sell += Toolbox.doSQL_double(conn, string.Format(@"
SELECT
	IFNULL(SUM((wo_detail_{0}_qty_committed * wo_detail_{0}_price_sell)* (1-(wo_detail_{0}_discount/100))),0)
FROM
	wo_detail_{0}
WHERE
	wo_detail_{0}_woprog_id IN ({2}) AND 
	wo_detail_{0}_billtypeid IN (0,2,3,10,11,9)", tablename, _wo.woprog_id, where_clause), null);
                #endregion disc_sell definition
                #region benchmark_sell definition
                benchmark_sell += Toolbox.doSQL_double(conn, string.Format(@"
SELECT
	IFNULL(sum(round((round(wo_detail_{0}_qty_committed * wo_detail_{0}_price_sell,3)),2)),0)
FROM
	wo_detail_{0}
WHERE
	wo_detail_{0}_woprog_id IN ({2}) AND 
	wo_detail_{0}_billtypeid IN (0,1,5,7,10)", tablename, _wo.woprog_id, where_clause), null);
                #endregion benchmark_sell definition
                #region quote_jobcost definition
                quote_jobcost += Toolbox.doSQL_double(conn, string.Format(@"
SELECT
	IFNULL(SUM(wo_detail_{0}_qty_committed * wo_detail_{0}_price_sell),0)
FROM
	wo_detail_{0}
WHERE
	wo_detail_{0}_woprog_id IN ({2}) and 
	wo_detail_{0}_billtypeid in (1)", tablename, _wo.woprog_id, where_clause), null);
                #endregion quote_jobcost definition
                tablename = "current";
            }  // end of work order loop


            if (_current_user.business_unit.is_backoffice || _current_user.id == 711)
            {
                NeBusinessUnit bu = new NeBusinessUnit(_wo.business_unit_id);
                #region div_bvposted population

                try
                {
                    lbl_accounting_hdr.InnerHtml = "BV Post : " + _bu.ddl_name + " (Dept: " + _bu.gl_div + ")";
                }
                catch(Exception ee)
                {
					Toolbox.do_errorLog_errorStack(ee);
                }

                if (Toolbox.doSQL_int(conn, "Select count(gl_special_account_te_link.id) from gl_special_account_te_link inner join gl_te on gl_te.id = gl_special_account_te_link.gl_te_id where gl_te.tax_entity_id = @v0", new object[] { _bu.tax_entity_id }) > 0)
                {
                    div_bvposted.InnerHtml =
                        @"<table cellpadding='2' cellspacing='0' style = 'width:100%; font-family:Calibri; font-size:15px;'>";
                   /* if (_wo.currency_id != bu.default_currency)
                    {
                        if (_wo.Status != "Invoiced" && _wo.Status != OpsWOStatus.WaitingToBeInvoiced && _wo.Status != "Waiting For PO")
                        {
                            if (_wo.invoiced_currency_rate == 0)
                            {
                                var _rate = new NECurrency(_wo.currency_id).rate_to_usd;
                                if (bu.default_currency == 1)
                                {
                                    _rate = 1 / new NECurrency(_wo.currency_id).rate_to_usd;
                                }
                                else
                                {
                                    _rate = 1 / new NECurrency(bu.default_currency).rate_to_usd * new NECurrency(_wo.currency_id).rate_to_usd;
                                }
                                Toolbox.doSQL_void(conn, "update woprog set invoiced_currency_rate = " + _rate + " where woprog_id = " + _wo.woprog_id, new object[]{});
                                _wo = new NeWOProg(_wo.woprog_id);
                            }

                            div_bvposted.InnerHtml += "<tr><td>Curr:</td><td>" + new NECurrency(_wo.currency_id).currency + "</td><td align='right' nowrap='nowrap'> at " + Math.Round(_wo.invoiced_currency_rate, 2) + "</td><td align='right' nowrap='nowrap'> Per USD</td></tr>";
                        }
                    }*/

                        div_bvposted.InnerHtml += "<tr><td><b>Div</b></td><td><b>Account</b></td><td align='right'><b>Debit</b></td><td align='right'><b>Credit</b></td></tr>";

                    DataTable dt_post = Toolbox.doSQL_dt(conn, "Call get_gl_postings_for_wo(@v0)", new object[] { _wo.woprog_id });
                    double total_debit = 0;
                    double total_credit = 0;
                    foreach (DataRow dr_post in dt_post.Rows)
                    {
                        div_bvposted.InnerHtml +=
                            @"<tr><td>" + dr_post["bu"] + @"</td><td valign='top'>" + dr_post["account_no"] +
                            @"</td><td valign='top' align='right'>" + Convert.ToDouble(dr_post["debit"]).ToString("n2") + @"</td><td valign='top' align='right'>" + Convert.ToDouble(dr_post["credit"]).ToString("n2") + @"</td></tr>";
                        total_debit += Convert.ToDouble(dr_post["debit"]);
                        total_credit += Convert.ToDouble(dr_post["credit"]);
                    }
                    div_bvposted.InnerHtml += @"<tr><td></td><td>Total:<td valign='top' align='right'>" + total_debit.ToString("n2") + @"</td><td valign='top' align='right'>" + total_credit.ToString("n2") + @"</td></tr>";
                    div_bvposted.InnerHtml += @"</table>";
                }
                else
                    div_bvposted.InnerHtml = "";
				btn_invoice.Visible		= bu.DSN == "" && _wo.Status.ToLower() == "waiting to be invoiced";
                div_bvposted.InnerHtml += string.Format(@"
            </br></br><table cellpadding='2' cellspacing='0' style = 'width:100%; font-family:Calibri; font-size:15px;'>
				<tr>
					<td>Currency:</td>
					<td style='text-align: right'>{11}</td>
				</tr>
				<tr>
					<td>TM Labour Cost :</td>
					<td style='text-align: right'>{0:N2}</td>
					<td></td>
				</tr>
				<tr>
					<td>TM Labour Sell :</td>
					<td style='text-align: right'>{1:N2}</td>
					<td></td>
				</tr>
				<tr>
					<td>TM Material Cost :</td>
					<td style='text-align: right'>{2:N2}</td>
					<td></td>
				</tr>
				<tr>
					<td>TM Material Sell :</td>
					<td style='text-align: right'>{3:N2}</td>
					<td></td>
				</tr>
				<tr>
					<td>Quoted Labour Cost :</td>
					<td style='text-align: right'>{4:N2}</td>
					<td></td>
				</tr>
				<tr>
					<td>Quoted Material Cost :</td>
					<td style='text-align: right'>{5:N2}</td>
					<td></td>
				</tr>
				<tr>
					<td>Quoted Sell :</td>
					<td style='text-align: right'>{6:N2}</td>
					<td></td>
				</tr>
				<tr>
					<td>Self Access Tax Material :</td>
					<td style='text-align: right'>{7:N2}</td>
					<td></td>
				</tr>
				
				<tr>
					<td>Self Access Tax Total :</td>
					<td style='text-align: right'>{8:N2}</td>
					<td></td>
				</tr>
<tr>
					<td>Sales Tax Material :</td>
					<td style='text-align: right'>{9:N2}</td>
					<td></td>
				</tr>
				
				<tr>
					<td>Sales Tax Total :</td>
					<td style='text-align: right'>{10:N2}</td>
					<td></td>
				</tr>
			</table>",
                    dbl_tmlabour_cost, //0
                    new_total.LaborPriceSell, //1
                    dbl_tmmat_cost, //2	
                    new_total.MaterialPriceSell, //3
                    dbl_q_labour_cost, //4
                    dbl_q_material_cost, //5
                    dbl_q_sell + dbl_progress_bill_2 + dbl_q_sell_2, //6
                    new_total.SATAX_MATERIAL, //7
                    new_total.SATAX_MATERIAL +
                    new_total.SATAX_LABOUR, //8
                    new_total.TAX_MATERIAL, //9
                    new_total.TAX_LABOUR + new_total.TAX_MATERIAL, //10
                    Toolbox.doSQL_string(conn, @"Select ifnull((Select currency from currency  where id =@v0),'Not Set')",
                        new object[] { _wo.currency_id })
                );

                var post_instructions = _wo.woprog_glposting_instructions;
                if (_wo.woprog_associate_woprog_id > 0)
                {
                    // This is not supposed to fill in from the job cost, that goes into the user control accounting_notes.ascx
                    post_instructions = Toolbox.doSQL_string(conn, @"SELECT IFNULL(woprog_glposting_instructions, '') FROM woprog WHERE woprog_id = @v0 ", new object[] { _wo.woprog_id });
                }
                mem_posted_notes.Text = post_instructions;

                #endregion div_bvposted population
            }


            if (_current_user.business_unit_id!=11 && _current_user.business_unit_id!=48)
            {
                lbl_accounting_hdr.Visible = false;
                div_accounting.Visible = false;
            }

#endregion


        }

	}
	

	

	protected void cb_posting_Callback(object _sender, CallbackEventArgsBase _e)
	{
		
		
			var mem = (ASPxMemo)mem_posted_notes;
			var target_wo_id = _wo.woprog_associate_woprog_id > 0 ? _wo.woprog_associate_woprog_id : _wo.woprog_id;
			Toolbox.doSQL_void(@"Update woprog  set woprog_glposting_instructions =@v0  where woprog_id =@v1", new object[] { mem.Text,target_wo_id });
	
	}

	protected void btn_invoice_OnClick(object _sender, EventArgs _e)
		{
		}
	}