using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using nesi.core;
using NESI.BLL.Common.ToolBox.SearchHistory;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Shared.PickList.Inventory;
using NESI.DTO.ViewModels.Shared.PickList.Quote;
using inventory = nesi.core.inventory;

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace NESI.BLL.Pages.Shared.PickList.Quote
{
	public class InventorySearch : PickListQuote
	{
		private const string SETTING_Match_Whole_Word = "Quote_PickList_Match_Whole_Word";
        public int quoteid;

        public InventorySearch(Employee user) : base(user)
		{
			
		}

		public InventorySearch(Employee user, int quote_id, int revision) : base(user, quote_id, revision)
		{
            quoteid = quote_id;


        }

		//		public DataTable Search(string query)
		//		{
		//			return nesi.core.inventory.search_results(query, this.WarehouseBusinessUnit.id, "description", false, false, false);
		//		}

		public string getFullSearchCondition(bool full)
		{
			var cost = !this.member_can_see_cost
				? "0 cost,"
				: "p.cost cost,"; // "if(ifnull(p.cost,0)=0,get_current_cost(a.master_id,g.business_unit_id),p.cost) cost,";

			var is_exclude = !this.member_can_see_cost ? "0 is_exclude," : "f.is_exclude is_exclude,";
			return full
				? $@"p.last_purchased last_purchased,
{cost}
p.cost origin_cost,
p.level cost_level,
p.sell sell,
g.business_unit_id,
{is_exclude} "

				//getsellprice(p.cost,0,true,1,g.business_unit_id) sell,
				//if(ifnull(p.cost,0)=0,get_current_cost(a.master_id,g.business_unit_id),p.cost) origin_cost,
				//if(ifnull(p.level,-1)<0,GET_COST(a.master_id ,g.business_unit_id ,true),p.level) cost_level,
				//getsellprice(if(ifnull(p.cost,0)=0,get_current_cost(a.master_id,g.business_unit_id),p.cost),0,true,1,g.business_unit_id) sell,
				//{is_exclude}
				//"
				:
@"'' last_purchased, 0 cost,0 origin_cost,1 cost_level, 0 sell, ";
		}

		public string SaveMatchWholeWordSetting(string value)
		{
			return SaveUserSetting(SETTING_Match_Whole_Word, value);
		}

		public string GetMatchWholeWordSetting()
		{
			return GetUserSetting(SETTING_Match_Whole_Word);
		}


		public double GetSellPrice(double cost, int bu_id)
		{
			var te = BLL.Common.Cache.Global.TaxEntity.GetValue(BLL.Common.Cache.Global.BusinessUnit.GetValue(bu_id).tax_entity_id);
			return 0;
		}

		public object Search(DTO.ViewModels.Core.DataStringType model)
		{
			var history = new BLL.Common.ToolBox.SearchHistory.InventorySearchHistory(CurrentUser);
			history.Add(model.Data);
			DataTable dt = new DataTable();
            var bu1=new nesi.core.quote(quoteid).business_unit_id;
            
            var bu = new NeBusinessUnit(bu1);
			var type_id = model.type;
			var desc_country = bu.country == "USA" ? "usa" : "cdn";
			var location_id = 0;
			var woprog_id = 0;
			switch (model.type)
			{
				case 0:
				case 1:
				case 2:
					int.TryParse(model.Data, out int search_type);
					var isnNum = int.TryParse(model.Data, out int master_id);
					var show_common = model.type == 1;
					var show_stocked = model.type == 2;
					dt = inventory.search_results_sc(model.Data, bu, isnNum ? "master_id" : "description", true, true, show_stocked, show_common, "0", "0", getFullSearchCondition(model.getfull), model.matchwhole);
					break;
				case 3:
				case 4:
					var select = @"
								SELECT 
									f.tag_id,
									f.tag,
									a.master_id,
									d.desc_full_" + desc_country + @" description,
									c.attribute_id,
									c.attribute,
									b.value,
									b.attribute_value_id,
									IFNULL(g.min_qty, 0.0) min," + getFullSearchCondition(model.getfull) +
@"IFNULL(g.int_onhand_qty, 0) onhand,
IFNULL(g.int_onhand_qty, 0) int_onhand_qty,
IFNULL(g.ext_onhand_qty, 0) ext_onhand_qty,
									g.ts last_used,
								" + (location_id != 0 ? @"(ifnull((Select il.qty from inventory_location il where il.location_master_id = " + location_id + @"  and il.master_id=a.master_id),0.0))" : "9999999999") + @" qty_truck,
								" + (woprog_id != 0 ? @"(select ifnull((Select aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0.0))" : "0.0") + @" qty_cmt_on_wo,
								" + (woprog_id != 0 ? @"(select ifnull((Select aa.wo_detail_current_qty_ordered-aa.wo_detail_current_qty_committed from wo_detail_current aa where aa.wo_detail_current_woprog_id = " + woprog_id + @" and aa.wo_detail_current_master_id=a.master_id limit 1),0.0))" : "0.0") + @"  qty_req_on_wo
								FROM
									inventory_item_detail a
								
								INNER JOIN 
									inventory_attribute_value b ON a.attribute_value_id = b.attribute_value_id
								INNER JOIN 
									inventory_attribute c ON b.attribute_id = c.attribute_id
								INNER JOIN 
									inventory_description d ON d.master_id = a.master_id
								INNER JOIN 
									inventory_item_master e ON e.master_id = a.master_id
								INNER JOIN 
									inventory_tag f ON e.tag_id = f.tag_id
								INNER JOIN 
									inventory_branch g ON e.master_id = g.master_id
								LEFT JOIN 
										inventory_cost p 
											ON a.master_id = p.master_id AND
											p.business_unit_id =g.business_unit_id
";

					var where_clause = @"
								WHERE 
								f.is_exclude=0 and f.allowed_to_stock = 1 and f.active = 1 and 
									e.active = true AND 
									e.master_id = g.master_id AND 
									g.business_unit_id = '" + WarehouseBusinessUnit.id + @"' 
								ORDER BY  IFNULL((SELECT SUM(wo_detail_history_qty_committed) FROM wo_detail_history WHERE wo_detail_history_master_id=a.master_id),0) desc,
									g.wo_usage desc, min DESC";

					if (type_id == 30)  //group
					{
						select += @"
								INNER JOIN 
									inventory_group_dtl h ON 
										e.master_id = h.master_id AND 
										h.group_id = " + model.type_id;
					}
					else if (type_id == 40) //kit
					{
						select += @"
								INNER JOIN 
									inventory_kit_dtl h ON 
										e.master_id = h.inventory_kit_dtl_master_id AND 
										h.inventory_kit_dtl_hdr_id = " + model.Data;
					}
					else if (type_id == 3) //wo
					{
						select += @"
								inner JOIN 
									vw_wo_parts h ON 
										e.master_id = h.master_id AND 
										h.woprog_id = " + model.Data + "";
					}
					else if (type_id == 4) //quote
					{
						select += @"
								INNER JOIN 
									quote_worksheet h ON 
										e.master_id = h.part_no AND 
										h.quote_id = " + model.Data;
					}
					else if (type_id == 5) // vendor no
					{
						select += @"
								INNER JOIN 
									inventory_price ip ON 
										e.master_id = ip.master_id AND 
										ip.vendor_code = '" + model.Data + "'";
					}
					dt = bllToolbox.doSQL_dt(select + where_clause);
					break;
				case 5:
					dt = inventory.search_results_sc(model.Data, bu, "vendor_code", false, false, false, false, "0", "0", getFullSearchCondition(model.getfull));
					break;
				default:
					break;
			}

			if (dt.Rows.Count > 0)
			{

				// var view = new DataView(dt) {Sort = "description"};
				//var main_inv_obj = dt.AsEnumerable()
				//.GroupBy(g => new
				//{
				//	m = g.Field<int>("master_id"),
				//	t = g.Field<int>("tag_id"),
				//	d = g.Field<string>("description").Replace(g.Field<string>("tag") + " - ", ""),
				//	q = g.Field<double>("onhand"),
				//	l = Toolbox.MySQL_shortdt(g.Field<DateTime>("last_used")),
				//	tr = g.Field<object>("qty_truck"),
				//	w = g.Field<decimal>("qty_cmt_on_wo"),
				//	r = g.Field<decimal>("qty_req_on_wo")
				//})
				//.Select(g => new
				//{
				//	g.Key.m,
				//	g.Key.t,
				//	v = g.Select(_z => _z.Field<int>("attribute_value_id")),
				//	a = g.Select(_z => _z.Field<int>("attribute_id")),
				//	g.Key.d,
				//	g.Key.q,
				//	g.Key.l,
				//	g.Key.tr,
				//	g.Key.w,
				//	g.Key.r
				//})
				//.OrderByDescending(o => o.q)
				//.ToList();
				var tags_obj = (from p in dt.AsEnumerable()
								select new
								{
									id = p.Field<int>("tag_id"),
									t = p.Field<string>("tag")
								}).Distinct().ToList();
				var atts_obj = (from p in dt.AsEnumerable()
								select new
								{
									id = p.Field<int>("attribute_id"),
									a = p.Field<string>("attribute")
								}).Distinct().OrderBy(_a => _a.a).ToList();
				var vals_obj = (from p in dt.AsEnumerable()
								select new
								{
									id = p.Field<int>("attribute_value_id"),
									aid = p.Field<int>("attribute_id"),
									v = p.Field<string>("value")
								}).Distinct().OrderBy(_v => _v.v).ToList();


				//				var invList = new List<PickListQuotePartDetail>();
				var master_id_list = (from p in dt.AsEnumerable()
									  select p.Field<int>("master_id")
										  ).Distinct().ToList();
				//				foreach (var id in master_id_list)
				//				{
				//					invList.Add(GetInventoryByMasterId(id));
				//				}
				return new
				{
					tags = tags_obj,
					atts = atts_obj,
					vals = vals_obj,
					data = dt,
					master_id_list
				};
			}
			return null;
		}

		public InventoryPartDetail GetInventoryPartDetail(int master_id, int quote_id, int revision)
		{

			return null;
		}

		//		public InventoryPartDetail[] Search(string query, string origin = "quote", bool wo_usage = false,
		//		bool po_usage = false, bool allow_gl = false, bool allow_nonstock = false, bool is_stocked = false,
		//		int vendor_id = 0)
		//		{
		//			var history = new BLL.Common.ToolBox.SearchHistory.InventorySearchHistory(CurrentUser);
		//			history.Add(query);
		//
		//			var list = new List<InventoryPartDetail>();
		//			var _dt = nesi.core.inventory.search_results(query, this.WarehouseBusinessUnit.id, "description", allow_gl,
		//				allow_nonstock, is_stocked);
		//			// var vendor_id = !string.IsNullOrEmpty(_q["vendor_id"]) && _q["vendor_id"] != "null" ? Convert.ToInt32(_q["vendor_id"]) : 0;
		//			var total = _dt.Rows.Count;
		//			var _inventory = new inventory();
		//			var BusinessUnit = this.WorkingBusinessUnit;
		//			var business_unit_id = BusinessUnit.id;
		//			var show_cost = CurrentUser.AuthenticatedForPrivilege(58);
		//			var show_sell = CurrentUser.AuthenticatedForPrivilege(81);
		//
		//			if (_dt.Rows.Count > 0)
		//			{
		//				var id_list = "";
		//				var _dv = _dt.DefaultView;
		//				_dv.Sort = wo_usage
		//					? "wo_usage DESC, min_qty DESC, onhand DESC"
		//					: po_usage
		//						? "po_usage DESC, min_qty DESC, onhand DESC"
		//						: "wo_usage DESC, min_qty DESC, onhand DESC";
		//				_dt = _dv.ToTable();
		//
		//				foreach (DataRow _dr in _dt.Rows)
		//				{
		//					var woUsage = Convert.ToDouble(_dr["wo_usage"]);
		//					var poUsage = Convert.ToDouble(_dr["po_usage"]);
		//					if (po_usage && poUsage == 0.0)
		//					{
		//						total--;
		//						continue;
		//					}
		//					else if (wo_usage && woUsage == 0.0)
		//					{
		//						total--;
		//						continue;
		//					}
		//					id_list += _dr["master_id"].ToString();
		//					id_list += _dr != _dt.Rows[_dt.Rows.Count - 1] ? "," : "";
		//				}
		//				var _s = DateTime.Now;
		//				if (id_list != "")
		//				{
		//					_inventory.Load(id_list, business_unit_id, true);
		//				}
		//				else
		//				{
		//					_inventory.parts = new System.Collections.ArrayList();
		//				}
		//				var _e = DateTime.Now;
		//				var _t = _e.Subtract(_s);
		//
		//				foreach (inventory i in _inventory.parts)
		//				{
		//					var sub_start = DateTime.Now;
		//					var master_id = i.master_id;
		//					var tag_id = i.tag_id;
		//					if (tag_id != null)
		//					{
		//						var tag_name = HttpUtility.HtmlEncode(_inventory.tag_name);
		//						var description = HttpUtility.HtmlEncode(i.description_full);
		//						var sold_as = BusinessUnit.country == "CDN" ? i.sold_as_canadian_name : i.sold_as_usa_name;
		//						if (description.ToString() == "")
		//						{
		//							description = "No description available for this part";
		//							sold_as = null;
		//						}
		//						double vendor_sell = 0;
		//						var vendor_code = "";
		//						double vendor_qty = 0;
		//						if (vendor_id != 0)
		//						{
		//							var _vendor_info = bllToolbox.doSQL_dt(
		//								@" SELECT a.total, a.vendor_code, a.qty FROM inventory_price a LEFT JOIN vendor b ON a.vendor_id = b.vendor_id WHERE a.master_id = @v0  AND a.business_unit_id = @v2  AND b.vendor_id = @v1  LIMIT 1",
		//								 master_id, vendor_id, BusinessUnit.warehouse_bu_id);
		//							foreach (DataRow _v in _vendor_info.Rows)
		//							{
		//								vendor_sell = Convert.ToDouble(_v["total"]);
		//								vendor_code = _v["vendor_code"].ToString();
		//								vendor_qty = Convert.ToDouble(_v["qty"]);
		//							}
		//						}
		//						var sub_finish = DateTime.Now;
		//						var _sub_diff = sub_finish.Subtract(sub_start);
		//						i.cost_price_branch = show_cost && origin == "workorder"
		//							? Math.Round(i.cost_price_branch, 3)
		//							: show_cost
		//								? i.cost_price_branch
		//								: 0;
		//						i.sell_price = origin == "workorder" && i.cost_price_branch > 0
		//							? shared.GetSellPrice(i.cost_price_branch, 0, i.is_qty, 1)
		//							: i.sell_price;
		//						var shown_vendor_date = "";
		//						try
		//						{
		//							if (i.vendor_price_last_dt != "--" && i.vendor_price_last_dt != "")
		//							{
		//								shown_vendor_date = Convert.ToDateTime(i.vendor_price_last_dt).ToString("yyyy-MM-dd");
		//							}
		//						}
		//						catch
		//						{
		//							shown_vendor_date = "--";
		//						}
		//
		//
		//						var o = new InventoryPartDetail
		//						{
		//							master_id = Convert.ToInt32(master_id),
		//							description = description,
		//							sell_price = i.sell_price,
		//							cost_price_branch = i.cost_price_branch,
		//							tag_id = i.tag_id,
		//							tag_name = tag_name,
		//							sold_as = sold_as,
		//							ttl_ms = i.ttl_ms,
		//							bv_part_number = i.bv_part_number,
		//							business_unit_id = BusinessUnit.warehouse_bu_id,
		//							is_qty = i.is_qty,
		//							vendor_code = vendor_code,
		//							vendor_qty = vendor_qty,
		//							vendor_sell = vendor_sell,
		//							shown_vendor_date = shown_vendor_date,
		//							onhand_qty = i.onhand_qty,
		//							is_exclude = i.is_exclude,
		//							show_sell = show_sell,
		//							show_cost = show_cost,
		//							has_pic = i.has_pic,
		//							has_minmax = (_inventory.n_minmax > 0),
		//							is_stocked = i.is_stocked,
		//							int_onhand_qty = i.int_onhand_qty,
		//							ext_onhand_qty = i.ext_onhand_qty,
		//							wo_usage = i.wo_usage,
		//							po_usage = i.po_usage,
		//							min_qty = i.qty
		//						};
		//
		//						list.Add(o);
		//					}
		//
		//				}
		//			}
		//			return list.ToArray();
		//		}

		public LabelValueInt[] GetRecentWos()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				@"Select woprog_id value,
				concat('0',woprog_bvwo/1,' - ',woprog_customername,' - ',woprog_description) label
				from woprog  where business_unit_id =@v0 order by woprog_id desc limit 100",
				WorkingBusinessUnit.id);
		}

		public LabelValueInt[] GetRecentQuotes()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				@"SELECT DISTINCT quote_master.quote_id value,
				concat(quote_master.quote_id, ' - ' ,customer.customer_name,' - ', quote_master.job_description) label
				FROM quote_worksheet 
				INNER JOIN quote_master ON quote_worksheet.quote_id = quote_master.quote_id 
				INNER JOIN customer ON quote_master.customer_id = customer.customer_id  
				WHERE quote_master.business_unit_id =@v0 ORDER BY quote_master.quote_id DESC limit 400",
				WorkingBusinessUnit.id);
		}
	}


}