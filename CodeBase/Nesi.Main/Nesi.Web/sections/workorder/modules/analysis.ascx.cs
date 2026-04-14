using System;
using System.Data;
using System.Linq;
using DevExpress.Web;
using System.Collections.Generic;
using System.Text;
using NESI.Common.Models;
using nesi.core;

// ReSharper disable InconsistentNaming

public partial class sections_workorder_modules_analysis : System.Web.UI.UserControl
	{
	double _addpocost;
	double _addposell;

	private bool _can_view_cost,
		_same_dept;

	private const int priv_view_cost = 58;

	public NeMember _current_user { get; set; }
	public NeWOProg _wo           { get; set; }

	bool IncludeOpenPOs;


	public int woprog_id { get; set; }

	protected void Page_Load(object sender, EventArgs e)
		{
		if(!Visible) return;
		}

	public void load()
		{
		var _q = Request.QueryString;
		if(woprog_id == 0 && _q["woprog_id"] == "0") return;
		hdn_woid.Value = woprog_id.ToString();
		IncludeOpenPOs = chkIncludeOpenPOs.Checked;
		if(woprog_id == 0 && _q["woprog_id"] != "0")
			{
			int try_woprog_id;
			int.TryParse(_q["woprog_id"], out try_woprog_id);
			woprog_id = try_woprog_id;
			_wo       = new NeWOProg(woprog_id);
			}
		bv_post._current_user = _current_user;
		if(woprog_id != 0)
			{
			_can_view_cost = _current_user.AuthenticatedForPrivilege(priv_view_cost);
			populate_analysis();
			}
		}


	private void populate_analysis()
		{
		#region variable definition

		using(var con = Toolbox.connect())
			{
			if(!IsPostBack)
				{
				//   _wo.fast_update_header_totals();
				//    NeWOProg.update_header_totals(_wo.woprog_id.ToString(), _wo.business_unit_id, _wo.OrderNumber);
				}

			var    tablename             = "";
			var    i                     = 0;
			var    count_regular     = 0;
			var    count_job_cost    = 0;
			var    count_invis_cred  = 0;
			var    count_quote_price = 0;
			var    count_blended     = 0;
			var    count_comment     = 0;
			var    count_dnc         = 0;
			var    count_blank       = 0;
			double blended           = 0;
			double blended_lab       = 0;
			double blended_mat       = 0;
			double dblms_sub_total       = 0;
			double invis             = 0;
			double invis_lab         = 0;
			double invis_mat         = 0;
			double jct_otal          = 0;
			double quote_sub_total   = 0;
			double quote_sub_total_2 = 0;

			double labour            = 0;
			double mat_to_be_billed  = 0;
			double tmlabour_cost     = 0;
			double tmlabour_sell     = 0;
			double q_labour_cost     = 0;
			double q_material_cost   = 0;
			double BalanceForward	 = 0;
			double q_sell            = 0;
			double q_sell_2          = 0;
			double tmmat_cost        = 0;
			double tmmat_sell        = 0;
			double progress_bill     = 0;
			double progress_bill_2   = 0;
			double dnc               = 0;
			double dnc_lab           = 0;
			double dnc_mat           = 0;
			double dni               = 0;
			double dni_lab           = 0;
			double dni_mat           = 0;
			var    jc_lab                = 0.00;
			double jc_mat                = 0;
			double vis_cred          = 0;
			double vis_cred_lab      = 0;
			double vis_cred_mat      = 0;
			double true_cost             = 0;
			double true_total_quote_cost = 0;
			double true_sell             = 0;
			double true_sell_b           = 0;
			double disc_sell             = 0;
			double benchmark_sell        = 0.0;
			double quote_jobcost         = 0.0;
			var    z                     = 1;
			tablename = "h";
			var linked_wos = Toolbox.doSQL_dt(con, @"CALL change_workorders(@v0 , 0)", new object[] {_wo.woprog_id});
			//		chk_uselinked.Visible = linked_wos.Rows.Count > 0;
			if(_wo.woprog_associate_woprog_id != 0)
				{
				//	div_QuoteTotals.Visible = false;
				benchmark_totals.Visible = false;
				}

			var all_wos    = "";
			var all_quotes = "";

			#endregion variable definition

			var new_total = new NeSalesOrder();
			new_total.getSalesOrderValues(con, _wo.woprog_id);
			if(IncludeOpenPOs)
				{
				#region addpocost definition

				_addpocost = Toolbox.doSQL_double(con,
					@" SELECT IFNULL(SUM((po_details_qty_ordered-a.po_details_qty_received)*a.po_details_cost), 0) FROM po_details_current a INNER JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_line_active = 1 AND a.po_details_woprog_id = @v0  and a.is_gl_account = false AND b.poprog_status > 2 AND b.poprog_status < 7",
					new object[] {_wo.woprog_id});

				#endregion addpocost definition

				#region addposell definition

				if(_wo.use_fixed_material_markup)
					{
					_addposell = Toolbox.doSQL_double(con,
						@" SELECT IFNULL(SUM(a.po_details_cost * @v1 * (po_details_qty_ordered-a.po_details_qty_received)), 0) FROM po_details_current a INNER JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_line_active = 1 AND a.po_details_woprog_id = @v0 and a.is_gl_account = false  AND b.poprog_status > 2 AND b.poprog_status < 7",
						new object[] {_wo.woprog_id, _wo.fixed_material_markup});
					}
				else
					{
					_addposell = Toolbox.doSQL_double(con,
						@" SELECT IFNULL(SUM(GETSELLPRICE(a.po_details_cost,0,true,a.po_details_qty_ordered,a.business_unit_id)*(po_details_qty_ordered-a.po_details_qty_received)), 0) FROM po_details_current a INNER JOIN poprog_header b ON a.po_details_poprog_id = b.poprog_id WHERE a.po_details_line_active = 1 AND a.po_details_woprog_id = @v0 and a.is_gl_account = false  AND b.poprog_status > 2 AND b.poprog_status < 7",
						new object[] {_wo.woprog_id});
					}

				#endregion addposell definition
				}

			// is this a progress bill?  if so, find all the others and find the parent.
			var ids       = new List<int>();
			var quote_ids = new List<int>();
			var idsb      = new List<int>();
			ids.Add(_wo.woprog_id); // add the existing WO as the base work order.
			idsb.Add(_wo.woprog_id);
			if(_wo.QuoteID != "0")
				{
				quote_ids.Add(Convert.ToInt32(_wo.QuoteID));
				}

			double total_labor_wo_cost    = 0;
			double total_material_wo_cost = 0;
			string benchmark_whereclause;


			all_wos = "";

			#region Find Linked work orders

			if(_wo.woprog_associate_woprog_id != 0)
				{
				#region Find sisters, build where clause

				try
					{
					var all_wos1 = Toolbox.doSQL_dt(con, @"SELECT woprog_id FROM woprog  WHERE woprog_associate_woprog_id =@v0",new object[] {_wo.woprog_associate_woprog_id});
					foreach(DataRow dr in all_wos1.Rows)
						{
						var wo_id = Convert.ToInt32(dr["woprog_id"]);
						idsb.Add(wo_id);
						ids.Add(wo_id);
						}
					}
				catch(Exception ee)
					{
					Toolbox.do_errorLog(ee);
					}

				#endregion Find sisters, build where clause
				}
			else
				{
				#region Find children, build where clause

				try
					{
					var all_wos1 = Toolbox.doSQL_dt(con, @"SELECT woprog_id FROM woprog  WHERE woprog_associate_woprog_id =@v0",new object[] {_wo.woprog_id});
					foreach(DataRow dr in all_wos1.Rows)
						{
						var wo_id = Convert.ToInt32(dr["woprog_id"]);
						idsb.Add(wo_id);
						ids.Add(wo_id);
						}
					}
				catch(Exception ee)
					{
					Toolbox.do_errorLog(ee);
					}

				#endregion Find children, build where clause
				}

			if(chk_uselinked.Checked)
				{
				if(linked_wos.Rows.Count > 0)
					{
					foreach(DataRow linked_wo in linked_wos.Rows)
						{
						var quote_id = Convert.ToInt32(linked_wo["quoteid"]);
						if(quote_id != 0)
							{
							quote_ids.Add(quote_id);
							}

						ids.Add(Convert.ToInt32(linked_wo["woprog_id"]));
						}
					}
				}

			#endregion Find Linked work orders

			for(z = 1; z <= 2; z++) // Once through history, then through current.
				{
				all_wos					= string.Join(",", ids.ToArray()); // this is now our list of WOs total.
				all_quotes				= string.Join(",", quote_ids.ToArray());
				benchmark_whereclause	= string.Join(",", idsb.ToArray());
				#region build wo_lines data table

				var wo_lines_sql = string.Format(@"
SELECT 
	a.id,
	a.billtypeid billtype_id,
	a.code,
	a.master_id,
	a.qty_committed,
	a.type,
	a.price_sell* (1-(a.discount/100)) price_sell, 
	(a.qty_committed * a.price_sell * (1-(a.discount/100))) AS total_price, 
	'1' AS HISTORY, 
	(a.qty_committed * a.price_cost) as total_cost, 
	a.discount, 
	a.woprog_id, 
	a.rec_no,
	wo.woprog_quoteid quote_id
FROM 
	wo_detail{0} a
LEFT JOIN 
	wo_detail_lineitem_billtype b ON a.billtypeid = b.wo_lineitem_billtypeid
LEFT JOIN 
	woprog wo on wo.woprog_id = a.woprog_id
WHERE 
	a.woprog_id IN ({1})
ORDER BY 
	rec_no",
					tablename,
					all_wos
					);
				var wo_lines = Toolbox.doSQL_dt(con, wo_lines_sql, null);

				#endregion build wo_lines data table

				foreach(DataRow wo_line in wo_lines.Rows)
					{
					i++;
					var this_billtype_id		= Convert.ToInt32(wo_line["billtype_id"]);
					var this_qty_committed		= Convert.ToDouble(wo_line["qty_committed"]);
					var this_price_sell			= Convert.ToDouble(wo_line["price_sell"]);
					var this_master_id			= Convert.ToInt32(wo_line["master_id"]);
					var this_type				= wo_line["type"].ToString();
					var isLabor					= this_type == "L" || this_master_id >= 990000;
					var isMaterial				= this_type == "M";
					var this_total_cost			= Convert.ToDouble(wo_line["total_cost"]);
					var this_discount			= Convert.ToDouble(wo_line["discount"]);
					total_labor_wo_cost			+= isLabor ? this_total_cost : 0.0;
					total_material_wo_cost		+= isMaterial ? this_total_cost : 0.0;
					this_discount				= (100 - this_discount) / 100;
					var is_credit_or_rebill		= _wo.woprog_iscredit > 0 || _wo.woprog_isrebill > 0;
					switch(this_billtype_id)
						{
						#region 0 - Regular
						case 0:
							count_regular++;
							if(isLabor)
								{
								labour += this_qty_committed * this_price_sell * this_discount;
								tmlabour_cost += _wo.woprog_iscredit > 0 || _wo.woprog_isrebill > 0
									? 0
									: this_total_cost;
								tmlabour_sell += this_qty_committed * this_price_sell;
								}
							else
								{
								mat_to_be_billed += this_qty_committed * this_price_sell * this_discount;
								tmmat_cost += is_credit_or_rebill
									? 0
									: this_total_cost;
								tmmat_sell += this_qty_committed * this_price_sell;
								}

							dblms_sub_total +=this_qty_committed * this_price_sell * this_discount;
						break;
						#endregion 0 - Regular
						#region 1 - Jobcost for Quote
						case 1:
							count_job_cost++;
							if(isLabor)
								{
								jc_lab            += this_qty_committed * this_price_sell;
								q_labour_cost += this_total_cost;
								}
							else
								{
								jc_mat              += this_qty_committed * this_price_sell;
								q_material_cost += this_total_cost;
								}

							jct_otal += this_qty_committed * this_price_sell;
						break;
						#endregion 1 - Jobcost for Quote
						#region 2 - Invisible Credit
						case 2:
							count_invis_cred++;
							if(isLabor)
								{
								invis_lab += this_qty_committed * this_price_sell;
								}
							else
								{
								invis_mat += this_qty_committed * this_price_sell;
								tmmat_cost += is_credit_or_rebill
									? 0
									: this_total_cost;
								tmmat_sell += this_qty_committed * this_price_sell;
								}

							invis       += this_qty_committed * this_price_sell;
							dblms_sub_total += this_qty_committed * this_price_sell;
						break;
						#endregion 2 - Invisible Credit
						#region 3 - Quoted Price
						case 3:
							count_quote_price++;
							quote_sub_total += this_qty_committed * this_price_sell;
							q_sell          += this_qty_committed * this_price_sell;

							q_material_cost += this_total_cost;
						break;
						#endregion 3 - Quoted Price
						#region 4 - Blended
						case 4:
							count_blended++;
							if(isLabor)
								{
								blended_lab += this_qty_committed * this_price_sell;
								}
							else
								{
								blended_mat += this_qty_committed * this_price_sell;
								}

							blended += this_qty_committed * this_price_sell;
						break;
						#endregion 4 - Blended
						#region 5 - Do Not Include
						case 5:
							if(isLabor)
								{
								dni_lab += this_qty_committed * this_price_sell;
								tmlabour_cost += _wo.woprog_iscredit > 0 || _wo.woprog_isrebill > 0
									? 0
									: this_total_cost;
								}
							else
								{
								dni_mat += this_qty_committed * this_price_sell;
								tmmat_cost += is_credit_or_rebill
									? 0
									: this_total_cost;
								tmmat_sell += this_qty_committed * this_price_sell;
								}

							dni += this_qty_committed * this_price_sell;
						break;
						#endregion 5 - Do Not Include
						#region 6 - Comment
						case 6:
							count_comment++;
						break;
						#endregion 6 - Comment
						#region 7 - Visible No Charge
						case 7:
							count_dnc++;
							if(isLabor)
								{
								dnc_lab += this_qty_committed * this_price_sell;
								tmlabour_cost += is_credit_or_rebill
									? 0
									: this_total_cost;
								}
							else
								{
								dnc_mat += this_qty_committed * this_price_sell;
								tmmat_cost += is_credit_or_rebill
									? 0
									: this_total_cost;
								}

							dnc += this_qty_committed * this_price_sell;
						break;
						#endregion 7 - Visible No Charge
						#region 8 - Blank
						case 8:
							count_blank++;
						break;
						#endregion 8 - Blank
						#region 9 - Progress Billing
						case 9:
							if(wo_line["quote_id"].ToString() == "0")
								{
								progress_bill += this_qty_committed * this_price_sell;
								}
						break;
						#endregion 9 - Progress Billing
						#region 10 - Visible Credit
						case 10:
							if(isLabor)
								{
								vis_cred_lab += this_qty_committed * this_price_sell;
								tmlabour_cost += _wo.woprog_iscredit > 0 || _wo.woprog_isrebill > 0
									? 0
									: this_total_cost;
								tmlabour_sell += this_qty_committed * this_price_sell;
								}
							else
								{
								vis_cred_mat += this_qty_committed * this_price_sell;
								tmmat_cost += is_credit_or_rebill
									? 0
									: this_total_cost;
								tmmat_sell += this_qty_committed * this_price_sell;
								}

							vis_cred    += this_qty_committed * this_price_sell;
							dblms_sub_total += this_qty_committed * this_price_sell;
						break;
						#endregion 10 - Visible Credit
						case 11: // quoted new
							q_material_cost += this_total_cost;
							q_sell_2        += this_qty_committed * this_price_sell;

							quote_sub_total_2 += this_qty_committed * this_price_sell;
						break;
						case 12: // progress bill new
							if(wo_line["quote_id"].ToString() == "0")
								{
								q_material_cost += this_total_cost;
								progress_bill_2 += this_qty_committed * this_price_sell;
								}
						break;
						case 13: // balance forward
							BalanceForward += this_qty_committed * this_price_sell;
						break;
						}
					}

				#region true_cost definition

				true_cost += Toolbox.doSQL_double(con, string.Format(@"
SELECT
	IFNULL(SUM(qty_committed * price_cost),0)
FROM
	wo_detail{0}
WHERE
	woprog_id IN ({1}) AND 
	(billtypeid NOT IN (3,6,8) ) ", tablename, all_wos), null);

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

				true_sell += Toolbox.doSQL_double(con, string.Format(@"
SELECT
	IFNULL(sum(round((round(qty_committed * price_sell,5) *((100-discount)/100)),5)),0)
FROM
	wo_detail{0}
WHERE
	woprog_id IN ({1}) AND 
	billtypeid IN (0,2,3,10,11,12,13)", tablename, all_wos), null);


				true_sell_b += Toolbox.doSQL_double(con, string.Format(@"
SELECT
	IFNULL(sum(round((round(qty_committed * price_sell,5) *((100-discount)/100)),5)),0)
FROM
	wo_detail{0}
WHERE
	woprog_id IN ({1}) AND 
	billtypeid IN (0,2,3,10,11,13)", tablename, benchmark_whereclause), null);

				#endregion true_sell definition

				#region disc_sell definition

				disc_sell += Toolbox.doSQL_double(con, string.Format(@"
SELECT
	IFNULL(SUM((qty_committed * price_sell)* (1-(discount/100))),0)
FROM
	wo_detail{0}
WHERE
	woprog_id IN ({1}) AND 
	billtypeid IN (0,2,3,10,11,9)", tablename, all_wos), null);

				#endregion disc_sell definition

				#region benchmark_sell definition

				benchmark_sell += Toolbox.doSQL_double(con, string.Format(@"
SELECT
	IFNULL(ROUND(SUM(qty_committed * price_sell),5),0)
FROM
	wo_detail{0}
WHERE
	woprog_id IN ({1}) AND 
	billtypeid IN (0,1,5,7,10)", tablename, all_wos), null);

				#endregion benchmark_sell definition

				#region quote_jobcost definition

				quote_jobcost += Toolbox.doSQL_double(con, string.Format(@"
SELECT
	IFNULL(SUM(qty_committed * price_sell),0)
FROM
	wo_detail{0}
WHERE
	woprog_id IN ({1}) and 
	billtypeid in (1)", tablename, all_wos), null);

				#endregion quote_jobcost definition

				tablename = "";
				} // end of work order loop
			var hasQuotes = all_quotes.Any();

			if (IncludeOpenPOs)
				{
				true_cost      += _addpocost;
				benchmark_sell += _addposell;
				quote_jobcost  += _addposell;
				}

			if(_can_view_cost)
				{
				if(_current_user.business_unit.is_backoffice)
					{
					div_wrapper_accounting.Visible = true;
					bv_post._wo                    = _wo;
					bv_post.load();
					}
				}
			else
				{
				div_margins.InnerHtml          = "";
				div_wrapper_accounting.Visible = false;
				}

			div_margins_wrapper.Visible = false;

			#region div_Benchmark_Totals population

			//    benchmark_sell = _wo.benchmark_labor_sell + _wo.benchmark_material_sell + _addposell;

			double total_projected_material_sell_on_wo = 0;
			foreach(var woid in all_wos.Split(','))
				{
				total_projected_material_sell_on_wo += Toolbox.doSQL_double(con, @"SELECT GET_TOTAL_REQUIRED_ITEMS_BENCHMARK_SELL(@v0)", new object[] {woid});
				}

			//       if (IncludeOpenPOs)
			//		{
			//			total_projected_material_sell_on_wo += _addposell;
			//		}

			div_Benchmark_Totals.InnerHtml = string.Format(@"
		<table cellpadding='2' cellspacing='0' style='width:100%; font-size:12px;'>
			<tr>
				<td>Total Sell :</td>
				<td style='text-align: right'>{0:N2}</td>
			</tr>
			<tr>
				<td>Benchmark Sell (T+M Sell Value)  :</td>
				<td style='text-align: right'>{1:N2}</td>
			</tr>
			<tr>
				<td></td>
				<td style='text-align: right'><td>
			</tr>
				<tr>
				<td>Total benchmark sell of all req'd labour and materials</td>
				<td style='text-align: right'>{2:N2}<td>
			</tr>
		</table>",
				true_sell_b,
				benchmark_sell,
				total_projected_material_sell_on_wo
				);

			#endregion div_Benchmark_Totals population


			#region div_QuoteTotals population

			double total_material_sell_on_worksheet = 0;
			double total_material_cost_on_worksheet = 0;
			double total_labor_cost_on_worksheet    = 0;
			double total_labor_sell_on_worksheet    = 0;
			var    compare_rows                     = new StringBuilder();
			double quote_margin                     = 0;


			if(hasQuotes)
				{
				foreach(var qid in all_quotes.Split(','))
					{
					var this_quote = new quote(Convert.ToInt32(qid));
					//    true_sell += Convert.ToDouble(this_quote.Price);
					total_material_sell_on_worksheet += NESI.BLL.Pages.Quotes.NeQuote.WorksheetTotalSell(con, this_quote.QuoteID, this_quote.Revision, NESI.BLL.Pages.Quotes.NeQuote.TotalType.Material);
					total_material_cost_on_worksheet += NESI.BLL.Pages.Quotes.NeQuote.WorksheetTotalCost(con, this_quote.QuoteID,this_quote.Revision, NESI.BLL.Pages.Quotes.NeQuote.TotalType.Material);
					total_labor_cost_on_worksheet += NESI.BLL.Pages.Quotes.NeQuote.WorksheetTotalCost(con, this_quote.QuoteID, this_quote.Revision, NESI.BLL.Pages.Quotes.NeQuote.TotalType.Labor);
					total_labor_sell_on_worksheet += NESI.BLL.Pages.Quotes.NeQuote.WorksheetTotalCost(con, this_quote.QuoteID, this_quote.Revision, NESI.BLL.Pages.Quotes.NeQuote.TotalType.Labor);
					}

				var dt_compare_q = Toolbox.doSQL_dt(con, string.Format(@"
SELECT part_no, CONCAT(c.membertype_name,' [', f.name,'] - ', d.abbreviation) name, 
IFNULL(sum(qty),0) qty_q,

    (select IFNULL(SUM(wo_detail_current_qty_committed), 0) FROM wo_detail_current 
    WHERE wo_detail_current_woprog_id in ({1}) AND wo_detail_current_master_id = a.part_no /1) 
+ (select IFNULL(SUM(wo_detail_history_qty_committed), 0) FROM wo_detail_history 
    WHERE wo_detail_history_woprog_id in ({1}) AND wo_detail_history_master_id = a.part_no /1)
qty_w 

FROM quote_worksheet a 
LEFT JOIN membertype_chargeout b on a.part_no = b.id 
LEFT JOIN membertype c ON b.membertype_id = c.membertype_id 
LEFT JOIN paytypehours d on b.paytype_id = d.paytypehours_id 
LEFT join business_unit f ON b.business_unit_id = f.id 
WHERE find_in_set(Concat(a.quote_id,a.revision),'{0}') 
AND part_no >= 990000 
GROUP BY part_no 
ORDER BY c.membertype_name, b.paytype_id", all_quotes, all_wos), null);

				var dt_compare_wo = Toolbox.doSQL_dt(con, string.Format(@"
select 
part_no,
name,
sum(qty) qty

from 
(
(SELECT wo_detail_history_master_id part_no, CONCAT(c.membertype_name,' [', f.name,'] - ', d.abbreviation) name, 
SUM(wo_detail_history_qty_committed) qty 
FROM wo_detail_history a 
LEFT JOIN membertype_chargeout b on a.wo_detail_history_master_id = b.id 
LEFT JOIN membertype c ON b.membertype_id = c.membertype_id 
LEFT JOIN paytypehours d on b.paytype_id = d.paytypehours_id 
LEFT join business_unit f ON b.business_unit_id = f.id 
where wo_detail_history_woprog_id IN ({0}) 
and wo_detail_history_master_id > 990000 
AND wo_detail_history_qty_committed != 0 
group by 
wo_detail_history_master_id) 

UNION

(SELECT wo_detail_current_master_id part_no, CONCAT(c.membertype_name,' [', f.name,'] - ', d.abbreviation) name, 
SUM(wo_detail_current_qty_committed) qty 
FROM wo_detail_current a 
LEFT JOIN membertype_chargeout b on a.wo_detail_current_master_id = b.id 
LEFT JOIN membertype c ON b.membertype_id = c.membertype_id 
LEFT JOIN paytypehours d on b.paytype_id = d.paytypehours_id 
LEFT join business_unit f ON b.business_unit_id = f.id 
where wo_detail_current_woprog_id IN ({0}) 
and wo_detail_current_master_id > 990000 
AND wo_detail_current_qty_committed != 0 
group by 
wo_detail_current_master_id ) ) a

group by a.part_no", all_wos), null);

				var labor_labour = _current_user.business_unit.country == "CDN"
					? "Labour"
					: "Labor";
				if(dt_compare_q.Rows.Count > 0 || dt_compare_wo.Rows.Count > 0)
					{
					compare_rows.AppendFormat(
						@"
				<table width='100%' cellpadding='5' cellspacing='0' style='border:solid 1px #4682B4;'>
					<thead>
						<tr style='background-color:#4682B4;color:#fff;'>
							<th>{0} Type</th>
							<th>On Quote</th>
							<th>On WO</th>
							<th>%</th>
						</tr>
					</thead>
					<tbody>",
						labor_labour);
					}

				if(dt_compare_q.Rows.Count > 0)
					{
					compare_rows.AppendFormat(
						@"
						<tr>
							<td align='left'><u><b>Quoted {0} Totals - Comparing Quoted {0} To WO {0}</b></u></td>
							<td style='border-right:solid 1px #4682B4;'>&nbsp;</td>
							<td style='border-right:solid 1px #007;'>&nbsp;</td>
							<td align='center'>&nbsp;</td>
						</tr>",
						labor_labour);
					foreach(DataRow dr_q in dt_compare_q.Rows)
						{
						var r_part_no = Convert.ToInt32(dr_q["part_no"]);
						var r_name    = dr_q["name"].ToString();
						var r_qty_q   = Convert.ToDouble(dr_q["qty_q"]);
						var r_qty_w   = Convert.ToDouble(dr_q["qty_w"]);
						var used_qty_w = r_qty_w == 0
							? "--"
							: r_qty_w.ToString();
						var used_pct = r_qty_w > 0
							? (r_qty_w / r_qty_q).ToString("P1")
							: "--";
						compare_rows.AppendFormat(
							@"
							<tr>
								<td>({0}) {1}</td><td align='center' style='border-right:solid 1px #4682B4;'>{2:N2}</td>
								<td align='center' style='border-right:solid 1px #4682B4;'>{3}</td>
								<td align='center' nowrap='nowrap'>{4}</td>
							</tr>",
							r_part_no, r_name, r_qty_q, used_qty_w, used_pct);
						}
					}

				if(dt_compare_wo.Rows.Count > 0)
					{
					compare_rows.AppendFormat(
						@"
						<tr>
							<td align='left'><br/><u><b>WO {0} Totals - Showing All {0} Totals on WO</b></u></td>
							<td style='border-right:solid 1px #4682B4;'>&nbsp;</td>
							<td style='border-right:solid 1px #4682B4;'>&nbsp;</td>
							<td align='center'>&nbsp;</td>
						</tr>",
						labor_labour);
					foreach(DataRow dr_wo in dt_compare_wo.Rows)
						{
						var r_part_no = Convert.ToInt32(dr_wo["part_no"]);
						var r_name    = dr_wo["name"].ToString();
						var r_qty_w   = Convert.ToDouble(dr_wo["qty"]);
						var r_qty_q = dt_compare_q.Rows.Count > 0 && dt_compare_q.Select($"part_no = '{r_part_no}'").Any()
							? Convert.ToDouble(dt_compare_q.Select($"part_no = '{r_part_no}'")[0]["qty_q"])
							: 0;
						var used_qty_q = r_qty_q == 0
							? "--"
							: r_qty_q.ToString();
						var used_pct = r_qty_q > 0
							? (r_qty_w / r_qty_q).ToString("P1")
							: "<b>N/A</b>";
						compare_rows.AppendFormat(
							@"
						<tr>
							<td>({0}) {1}</td>
							<td align='center' style='border-right:solid 1px #4682B4;'>{2}</td>
							<td align='center' style='border-right:solid 1px #4682B4;'>{3:N2}</td>
							<td align='center' nowrap='nowrap'>{4}</td>
						</tr>",
							r_part_no, r_name, used_qty_q, r_qty_w, used_pct);
						}
					}

				if(dt_compare_q.Rows.Count > 0 || dt_compare_wo.Rows.Count > 0)
					{
					compare_rows.Append("</tbody>");
					compare_rows.AppendFormat(@"</table>");
					}

				if(dt_compare_q.Rows.Count == 0 && dt_compare_wo.Rows.Count == 0)
					{
					compare_rows.Clear();
					}


				if(total_labor_sell_on_worksheet != 0 || total_material_sell_on_worksheet != 0)
					{
					quote_margin = (true_sell - (total_labor_cost_on_worksheet + total_material_cost_on_worksheet)) / true_sell;
					}
				}

			var total_projected_labor_cost    = 0.0;
			var total_projected_labor_sell    = 0.0;
			var total_projected_material_cost = 0.0;
			var total_projected_material_sell = 0.0;
			var total_wo_labor_cost           = 0.0;
			var total_wo_mat_cost             = 0.0;


			foreach(var woid in all_wos.Split(','))
				{
				total_projected_labor_cost += Toolbox.doSQL_double(con, @"SELECT IFNULL(GET_TOTAL_REQUIRED_LABOUR_COST(@v0),0.0)", new object[] {woid});
				total_projected_labor_sell += Toolbox.doSQL_double(con, @"SELECT IFNULL(GET_TOTAL_REQUIRED_LABOUR_SELL(@v0),0.0)", new object[] {woid});
				total_projected_material_cost += Toolbox.doSQL_double(con, @"SELECT IFNULL(GET_TOTAL_REQUIRED_MATERIAL_COST(@v0),0.0)", new object[] {woid});
				total_projected_material_sell += Toolbox.doSQL_double(con, @"SELECT IFNULL(GET_TOTAL_REQUIRED_MATERIAL_SELL(@v0),0.0)", new object[] {woid});

				total_wo_labor_cost += Toolbox.doSQL_double(con, @"SELECT this_wo_labour_cost FROM woprog WHERE woprog_id = @v0",new object[] {woid});
				total_wo_mat_cost += Toolbox.doSQL_double(con, @"SELECT this_wo_material_cost FROM woprog WHERE woprog_id = @v0",new object[] {woid});
				}

			var show_projected = !Toolbox.Contains(_wo.Status, new [] {"Invoiced", OpsWOStatus.WaitingToBeInvoiced, "Waiting for PO" });
			var show_hours_projected = _wo.Status == OpsWOStatus.Open;
			div_QuoteTotals.InnerHtml = @"
					<table cellpadding='2' cellspacing='0' style='width:100%; font-size:12px;'>";

			if(show_projected)
				{
				div_QuoteTotals.InnerHtml += @"
						<tr>
							<td nowrap='nowrap'>Still To Be Invoiced: </td>
							<td></td>
							<td></td>
							<td align = 'right' >" +string.Format("{0:#,#.#0;-#,#.#0;''}", true_sell - (progress_bill + progress_bill_2) - BalanceForward) + @"</td>
                        </tr>";
				if(BalanceForward > 0)
					{
					div_QuoteTotals.InnerHtml += @"
						<tr>
							<td nowrap='nowrap'>Balance Forward: </td>
							<td></td>
							<td></td>
							<td align = 'right' >" +string.Format("{0:#,#.#0;-#,#.#0;''}", BalanceForward) + @"</td>
                        </tr>";

					}
				div_QuoteTotals.InnerHtml += @"
												<tr>" + (hasQuotes
					                             ? "<td nowrap='nowrap'>Total Job: </td><td></td><td></td><td align='right'>" +string.Format("{0:#,#.#0;-#,#.#0;''}",true_sell) + @" </td>"
					                             : "<td></td>") + @"
						</tr>";
				}
			else
				{
				div_QuoteTotals.InnerHtml += @"
						<tr>
							<td nowrap='nowrap'>Still To Be Invoiced: </td>
							<td></td>
							<td align = 'right' >" +string.Format("{0:#,#.#0;-#,#.#0;''}", true_sell - (progress_bill + progress_bill_2)) + @"</td>
                        </tr>";
				if(BalanceForward > 0)
					{
					div_QuoteTotals.InnerHtml += @"
						<tr>
							<td nowrap='nowrap'>Balance Forward: </td>
							<td></td>
							<td align = 'right' >" +string.Format("{0:#,#.#0;-#,#.#0;''}", BalanceForward) + @"</td>
                        </tr>";

					}
				div_QuoteTotals.InnerHtml += @"
						<tr>" + (hasQuotes
					                             ? "<td nowrap='nowrap'>Total Job: </td><td></td><td align='right'>" + string.Format("{0:#,#.#0;-#,#.#0;''}", true_sell) + @"</td>"
					                             : "<td colspan='3'></td>") + @"
						</tr>";
				}
			var quoteTotalFormat = @"
						<tr>
							<td colspan='4'>
								<table width='100%'>
									<tr style='display:{23}'>
										<td nowrap='nowrap'>Labor Needed:</td>
										<td align='right' nowrap='nowrap'><input style='text-align:right; width:40px;'  name='men_needed' size='2' type='number' min='0' max='200' value='{22:n0}' onkeydown='only_numeric(event)' onblur='fix_men(this.value)' />people for</td> 
										<td align='right' nowrap='nowrap'><input style='text-align:right; width:40px;'  name='days_needed' size='2' type='number' min='0' max='200' value='{21:n0}' onkeydown='only_numeric(event)' onblur='fix_days(this.value)' />days</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td></td>
							<td>&nbsp;</td>
							<td></td>
						</tr>";
			if(show_projected)
			{
				quoteTotalFormat	+= @"
						<tr align='center'>
							<td></td>" + (hasQuotes? @"<td width='30%'  valign='top' style='text-align: right; display:{20};'><b>Quote</br>WorkSheet</b></td>": "<td></td>") +
							@"
							<td width='30%' align='right'><b>Current</br>Work Order(s)</b></td>
							<td width='30%' align='right' valign='top'><b>Projected</br>Work</br>Order(s)</b></td>
						</tr>" + (all_quotes.Length == 0 ? "<tr><td nowrap='nowrap'>Total Sell</td><td></td><td align = 'right' >{0:N2}</td><td align = 'right' >{24:N2}</td></tr>" : "") +
							@"
						<tr>
							<td nowrap='nowrap'>Labour Cost</td>
							<td align='right'>{4:#,#.#0;-#,#.#0;''}</td>
							<td align='right'>{8:#,#.#0;-#,#.#0;''}</td>
							<td align='right' title='Labour projections are extrapolated from days and manpower needed.' >{11:#,#.#0;-#,#.#0;''}</td>
						</tr>
						<tr>
							<td nowrap='nowrap'>Material Cost</td>
							<td align = 'right' >{5:#,#.#0;-#,#.#0;''}</td>
							<td align='right'>{9:#,#.#0;-#,#.#0;''}</td>
							<td align='right' >{12:#,#.#0;-#,#.#0;''}</td>
						</tr>
						<tr>
							<td nowrap='nowrap'>Total Cost</td>
							<td align='right'>{17:#,#.#0;-#,#.#0;''}</td>
							<td align='right'>{19:#,#.#0;-#,#.#0;''}</td>
							<td align='right'>{18:#,#.#0;-#,#.#0;''}</td>
						</tr>
						<tr>
							<td>$ Margin</td>
							<td align='right'>" + (hasQuotes ? @"{10:#,#0.#0}" : @"{10:#,#0.#0;;''}") + @"</td>
							<td align='right'>{2:#,#0.#0}</td>
							<td align='right'>{25:#,#0.#0}</td>
						</tr>
						<tr>
							<td>% Margin</td>
							<td align='right'>" + (hasQuotes ? @"{6:#,#0.#0}" : @"{6:#,#0.#0;;''}") + @"</td>
							<td align='right'>{3:#,#0.#0}</td>
							<td align='right'>{26:#,#0.#0}</td>
						</tr>";
			}
			else
			{
				quoteTotalFormat += @"
						<tr align='center'>
							<td></td>
							" + (hasQuotes ? "<td width='30%'  valign='top' style='text-align: right; display:{20};'><b>Quote</br>WorkSheet</b></td>" : "<td></td>") + @"
							<td width='30%' align='right'><b>Current</br>Work Order(s)</b></td>
							<td></td>
						</tr>" + (all_quotes.Length == 0 ? "<tr><td nowrap='nowrap'>Total Sell</td><td></td><td align = 'right' >{0:#,#.#0;-#,#.#0;''}</td><td></td></tr>" : "") + @"
						<tr>
							<td nowrap='nowrap'>Labour Cost</td>
							<td>{4:$#.##;-$#.##;''}</td>
							<td align='right'>{8:#,#.#0;-#,#.#0;''}</td>
							<td></td>
						</tr>	
						<tr>
							<td nowrap='nowrap'>Material Cost</td>
							<td align='right'>{5:#,#.#0;-#,#.#0;''}</td>
							<td align='right'>{9:#,#.#0;-#,#.#0;''}</td>
							<td></td>
						</tr>
						<tr>
							<td nowrap='nowrap'>Total Cost</td>
							<td align='right'>{17:#,#.#0;-#,#.#0;''}</td>
							<td align='right'>{19:#,#.#0;-#,#.#0;''}</td>
							<td></td>
						</tr>	
						<tr>
							<td>$ Margin</td>
							<td align='right'>" + (hasQuotes ? @"{10:#,#0.#0}" : @"{10:#,#0.#0;;''}") + @"</td>
							<td align='right'>{2:#,#0.#0}</td>
							<td></td>
						</tr>	
						<tr>
							<td>% Margin</td>
							<td align='right'>" + (hasQuotes ? @"{6:#,#0.#0}" : @"{6:#,#0.#0;;''}") + @"</td>
							<td align='right'>{3:#,#0.#0}</td>
							<td></td>
						</tr>";
			}
			quoteTotalFormat += @"
						<tr>
							<td></td>
							<td>&nbsp;</td>
							<td> </td>
							<td> </td>
						</tr>
						<tr>
							<td colspan='4'>{7}</td>
						</tr>
					</table>";
			div_QuoteTotals.InnerHtml += string.Format(quoteTotalFormat,
				true_sell, // {0}
				quote_jobcost, // {1}
				(true_sell - true_cost), // {2}  // margin of work order
				(true_sell == 0 ? 0 : ((100 * (true_sell - true_cost)) / true_sell)), // {3}
				total_labor_cost_on_worksheet, // {4}
				total_material_cost_on_worksheet, // {5}
				(hasQuotes
					? (100 * quote_margin)
					: 0), // {6}
				hasQuotes
					? compare_rows.ToString()
					: "", // {7}
				total_wo_labor_cost, // {8}
				total_wo_mat_cost, // {9}
				hasQuotes
					? true_sell - (total_labor_cost_on_worksheet + total_material_cost_on_worksheet)
					: 0, //10 
				total_projected_labor_cost, //11
				total_projected_material_cost, //12
				0, //13  // 
				0, // {14}
				show_projected, //15
				_wo.woprog_timesheet_percentage, //16
				total_labor_cost_on_worksheet + total_material_cost_on_worksheet, //17
				total_projected_labor_cost + total_projected_material_cost, //18
				true_cost, //19
				hasQuotes
					? "contents"
					: "none", //20
				_wo.no_of_days_left, //21
				_wo.no_of_guys_left, //22
				show_hours_projected
					? "contents"
					: "none", //23
				total_projected_material_sell + total_projected_labor_sell, //24
				(hasQuotes
					? (true_sell - (total_projected_labor_cost + total_projected_material_cost))
					: (total_projected_material_sell + total_projected_labor_sell - (total_projected_labor_cost + total_projected_material_cost))), //25
				(hasQuotes 
					? (true_sell == 0 ? 0 : (100 * (true_sell - (total_projected_labor_cost + total_projected_material_cost + _addpocost))) / true_sell)
					: ((total_projected_material_sell + total_projected_labor_sell) == 0 ? 0 : (100 * (total_projected_material_sell + total_projected_labor_sell - (total_projected_labor_cost + total_projected_material_cost)) / (total_projected_material_sell + total_projected_labor_sell)))) //26
				).Replace(@"\r", "").Replace(@"\n", "");
			#endregion

			#region div_Billtype_Totals population

			div_Billtype_Totals.InnerHtml = $@"
<table cellpadding='2' cellspacing='0' style = 'width:100%; font-size:0.85em;'>
	<tr>
		<td></td>
		<td style='text-align: right'><b>Labor</b></td>
		<td style='text-align: right'><b>Material</b></td>
		<td style='text-align: right'><b>Total</b></td>
	</tr>
	<tr style='text-align: right'>
		<td style='text-align: left'>Invisible Credit:</td>
		<td style='text-align: right'>{invis_lab:N2}</td>
		<td>{invis_mat:N2}</td>
		<td>{invis:N2}</td>
	</tr>
	<tr style='text-align: right'>
		<td style='text-align: left'>Job Cost (for quoted jobs):</td>
		<td style='text-align: right'>{jc_lab:N2}</td>
		<td>{jc_mat:N2}</td>
		<td>{jct_otal:N2}</td>
	</tr>
	<tr style='text-align: right'>
		<td style='text-align: left'>Visible No Charge:</td>
		<td style='text-align: right'>{dnc_lab:N2}</td>
		<td>{dnc_mat:N2}</td>
		<td>{dnc:N2}</td>
	</tr>
	<tr style='text-align: right'>
		<td style='text-align: left'>Do Not Include:</td>
		<td style='text-align: right'>{dni_lab:N2}</td>
		<td>{dni_mat:N2}</td>
		<td>{dni:N2}</td>
	</tr>
	<tr style='text-align: right'>
		<td style='text-align: left'>Visible Credit:</td>
		<td style='text-align: right'>{vis_cred_lab:N2}</td>
		<td>{vis_cred_mat:N2}</td>
		<td>{vis_cred:N2}</td>
	</tr>
	<tr style='text-align: right'>
		<td style='text-align: left'>Regular:</td>
		<td style='text-align: right'>{labour:N2}</td>
		<td>{mat_to_be_billed:N2}</td>
		<td>{labour + mat_to_be_billed:N2}</td>
	</tr>
	<tr style='text-align: right'>
		<td style='text-align: left'>Progress Billed:</td>
		<td style='text-align: right'></td>
		<td></td>
		<td>{progress_bill + progress_bill_2:N2}</td>
	</tr>
	<tr style='text-align: right'>
		<td style='text-align: left'>Quoted:</td>
		<td style='text-align: right'></td>
		<td></td>
		<td>{quote_sub_total + quote_sub_total_2:N2}</td>
	</tr>
	<tr style='text-align: right'>
		<td style='text-align: left'>Balance Forward:</td>
		<td style='text-align: right'></td>
		<td></td>
		<td>{BalanceForward:N2}</td>
	</tr>
</table>";

			#endregion div_Billtype_Totals population

			if(_current_user.business_unit_id != 11 && _current_user.business_unit_id != 48)
				{
				div_wrapper_accounting.Visible = false;
				}
			}
		}

	protected void cb_bench_credit_Callback(object _sender, CallbackEventArgsBase _e)
		{
		if(!string.IsNullOrEmpty(_e.Parameter))
			{
			try
				{
				Toolbox.doSQL_void(@"Update woprog  set benchmark_credit =@v0  where woprog_id =@v1",
					new object[] {_e.Parameter, _wo.woprog_id});
				}
			catch
				{
				}
			}
		}

	protected void chk_uselinked_CheckedChanged(object _sender, EventArgs _e)
		{
		load();
		}

	protected void chkIncludeOpenPOs_CheckedChanged(object _sender, EventArgs _e)
		{
		load();
		}


	protected void cb_quote_Callback(object source, CallbackEventArgs e)
		{
		if(e.Parameter != "")
			{
			var action = e.Parameter.Split('|').GetValue(0).ToString();

			switch(action)
				{
					case "p":

						if(Convert.ToDouble(e.Parameter.Split('|').GetValue(1)) <= 100 &&
						   Convert.ToDouble(e.Parameter.Split('|').GetValue(1)) >= 0)
							{
							Toolbox.doSQL_void(@"update woprog  set WOProg_TimeSheet_Percentage =@v0  where woprog_id =@v1 limit 1 ",
								new object[] {Math.Round(Convert.ToDouble(e.Parameter.Split('|').GetValue(1)), 2), _wo.woprog_id});
							_wo = new NeWOProg(_wo.woprog_id);
							populate_analysis();
							e.Result = div_QuoteTotals.InnerHtml;
							}
						else
							throw new Exception("You must enter a value between 0 and 100");

						break;

					case "m":
						if(Convert.ToDouble(e.Parameter.Split('|').GetValue(1)) >= 0)
							{
							Toolbox.doSQL_void(@"update woprog  set no_of_guys_left =@v0  where woprog_id =@v1 limit 1 ",
								new object[] {Math.Round(Convert.ToDouble(e.Parameter.Split('|').GetValue(1)), 2), _wo.woprog_id});
							_wo = new NeWOProg(_wo.woprog_id);
							populate_analysis();
							e.Result = div_QuoteTotals.InnerHtml;
							}
						else
							throw new Exception("You must enter a value greater or equal to 0");

						break;

						break;

					case "d":
						if(Convert.ToDouble(e.Parameter.Split('|').GetValue(1)) >= 0)
							{
							Toolbox.doSQL_void(@"update woprog  set no_of_days_left =@v0  where woprog_id =@v1 limit 1 ",
								new object[] {Math.Round(Convert.ToDouble(e.Parameter.Split('|').GetValue(1)), 2), _wo.woprog_id});
							_wo = new NeWOProg(_wo.woprog_id);
							populate_analysis();
							e.Result = div_QuoteTotals.InnerHtml;
							}
						else
							throw new Exception("You must enter a value greater or equal to 0");

						break;
				}
			}
		}
	}
