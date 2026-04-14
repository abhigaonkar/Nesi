using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.BLL.Pages.Quotes;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Shared.PickList;
using NESI.DTO.ViewModels.Shared.PickList.Quote;

// ReSharper disable All

namespace NESI.BLL.Pages.Shared.PickList.Quote
{
	public partial class PickListQuote : PickListQuoteBase
	{

		public PickListQuote(Employee user) : base(user)
		{

		}

		public double GetNotIncludeSectionTotal(int id, int rev)
		{
			return bllToolbox.doSQL_double(@"SELECT
  IFNULL(SUM(extended_per), 0)
FROM
  quote_worksheet
WHERE quote_id = @v0
  AND revision = @v1
  AND section_id IN
  (SELECT
    section_id
  FROM
    quote_extratext
  WHERE is_checked = 0
    AND quote_id = @v0
    AND revision = @v1)", id, rev);
		}

		public LabelValueInt[] GetSectionList(int id, int rev)
		{
			var sectionList = bllToolbox.doSQL_List<LabelValueInt>(@"SELECT 
a.id value,
a.section label
FROM quote_section a
left join quote_extratext b
on a.detail_id=b.id
WHERE a.quote_id = @v0  AND a.revision = @v1 and a.is_checked=1 and ifnull(b.is_checked,1)=1
order by ifnull(b.type,0), ifnull(b.line_number,0), a.section
", id, rev);
			if (sectionList == null)
			{
				sectionList = new List<LabelValueInt>();
			}
			sectionList.Insert(0, new LabelValueInt { Value = 0, Label = "All Sections" });
			return sectionList.Where(x => !string.IsNullOrWhiteSpace(x.Label)).ToArray();
		}

		public LabelValueInt[] GetFullSectionList(int id, int rev)
		{
			var sectionList = bllToolbox.doSQL_List<LabelValueInt>(@"
SELECT 
id value,
linetext label
FROM 
quote_extratext 
WHERE quote_id = @v0  AND revision = @v1
order by type, line_number
", id, rev);
			if (sectionList == null)
			{
				sectionList = new List<LabelValueInt>();
			}
			sectionList.Insert(0, new LabelValueInt { Value = 0, Label = "All Sections" });
			return sectionList.Where(x => !string.IsNullOrWhiteSpace(x.Label)).ToArray();
		}
		public PickListQuote(Employee user, int _id, int _rev) : base(user, _id, _rev)
		{
			if (_rev == 0)
			{
				string quote = _id.ToString();
				string id = quote.Substring(0, 6);
				string rev = quote.Substring(6, quote.Length - 6);
				_id = int.Parse(id);
				_rev = int.Parse(rev);
			}

			sources.RemoveAt(8); // remove vendor 
			sources.RemoveAt(7); // remove po
			if (!WorkingBusinessUnit.is_er) sources.RemoveAt(7); // remove repair


			Title = "Quote Parts List for " + _id;

			custNameDisplay = _quote.txtCustomerName + " - " + _quote.txtJobDescription;
			var QuotedBy = new NeMember(Convert.ToInt32(quote_info["quoted_by"]));
			memberNameDisplay = QuotedBy.FullName;
			var q_status = (int)quote_info["status_id"];
			addnewLine = q_status < 6 || q_status == 12;
			var use_current_cost = quote_info["use_current_cost"].ToString() == "True";

			NotIncludeSectionTotal = GetNotIncludeSectionTotal(_id, _rev);

			var isAllowedCost = member_can_see_cost;
			if (((IList)new[] { 1, 2, 5 }).Contains(q_status))
			{
				bllToolbox.doSQL_void(@"UPDATE quote_worksheet SET cost = GET_CURRENT_LABOUR_COST(part_no, @v2 ), ts = NOW() WHERE quote_id = @v0  AND revision = @v1 AND ts < CURDATE() AND part_no BETWEEN 990000 AND 1000000", _id, _rev, buId);
			}
			var see_cost_a = isAllowedCost ? "A.cost" : "IF(IS_EXCLUDE(part_no), A.cost, 0)";
			var see_cost_c = isAllowedCost ? "C.cost" : "IF(IS_EXCLUDE(part_no), C.cost, 0)";
			var used_table = use_current_cost ? "quote_worksheet_current_cost" : "quote_worksheet";
			var is_exculde_a = !isAllowedCost ? "0 is_exclude" : "IS_EXCLUDE(part_no) is_exclude";
			var is_exculde_c = !isAllowedCost ? "0 is_exclude" : "IS_EXCLUDE(part_no) is_exclude";

			//			var sectionList = bllToolbox.doSQL_List<LabelValueInt>(@"SELECT id value,section label FROM quote_section WHERE quote_id = @v0  AND revision = @v1", _id, _rev);
			//
			//			sectionList.Insert(0, new LabelValueInt { Value = 0, Label = "Select section" });

			section = GetSectionList(_id, _rev);

			items =
				bllToolbox.doSQL_Array<PickListItem>(string.Format(@" 
							(SELECT
							  A.quote_id quote_id,
							  A.revision revision,
							  A.id,
							  A.part_no,
							  A.part_no master_id,
							  A.code,
							  A.notes,
							  A.description label,
							  CAST(A.original_sell as DECIMAL(20,2)) sell,
							  CAST({2} as DECIMAL(20,2)) cost,
							  IFNULL(icu.avg_price,0) avg_cost,
                              IFNULL(icu.recommended_price,0) recommended_cost,
							  IFNULL(DATE_FORMAT(icu.week_of, '%Y-%m-%d'), 'N/A') AS date_updated,
							  A.qty,
							  cast((A.original_sell * A.qty * (1- (quote_worksheet_discount / 100)))  as DECIMAL(20,2)) extd,
							  cast(extended_per  as DECIMAL(20,2)) extd2,
							  'PlaceHolder' workorder,
							  quote_worksheet_part_requested include,
							  B.section section_name,
							  b.id section_id,
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
							  ifnull(a.is_checked,1) is_checked,
							  'false' is_selected,
							  IFNULL(inventory_tag.allowed_to_stock,0) allowed_to_stock,
							  IFNULL(inventory_tag.is_commodity,0) is_commodity,
							  false is_gl_account,
							  if(a.line_number=0,a.id,a.line_number) line_number,
							 {3}
							FROM
							  {0} A
							LEFT JOIN (SELECT id, section, is_checked FROM quote_section WHERE quote_id = @v0 AND revision = @v1) b 
								ON a.section_id = b.id
							LEFT JOIN inventory_item_master im
								ON im.master_id = A.part_no
							LEFT JOIN inventory_tag
								ON inventory_tag.tag_id = im.tag_id
                            LEFT JOIN (select internal_org_item_id, avg_price, recommended_price, week_of from item_cost_upload where branch=(select name from business_unit where id=(select business_unit_id from quote_master where quote_id=@v0 and revision=@v1))) icu
								ON icu.internal_org_item_id = A.part_no
							
							WHERE 
								A.quote_id = @v0 AND 
								A.revision = @v1 AND 
							    B.is_checked=1 AND
								b.section REGEXP '^[0-9]' 
							ORDER BY 
								line_number
							)
							UNION
							(
							SELECT
							  c.quote_id quote_id,
							  c.revision revision,
							  C.id,
							  C.part_no,
							  C.part_no master_id,
							  C.code,
							  C.notes,
							  C.description label,
							  cast(C.original_sell as DECIMAL(20,2)) sell,
							  cast({1} as DECIMAL(20,2)) cost,
							  IFNULL(icu.avg_price,0) avg_cost,
                              IFNULL(icu.recommended_price,0) recommended_cost,
							  IFNULL(DATE_FORMAT(icu.week_of, '%Y-%m-%d'), 'N/A') AS date_updated,
							  C.qty,
							  cast((ROUND(C.original_sell, 3) * C.qty * (1- (quote_worksheet_discount / 100)))  as DECIMAL(20,2)) extd,
							  cast(extended_per as DECIMAL(20,2)) extd2, 
							  'PlaceHolder' workorder,
							  quote_worksheet_part_requested include,
							  D.section section_name,
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
							  ifnull(c.is_checked,1) is_checked,
							  'false' is_selected,
							  IFNULL(inventory_tag.allowed_to_stock,0) allowed_to_stock,  
                              IFNULL(inventory_tag.is_commodity,0) is_commodity,
							  false is_gl_account,
							  if(c.line_number=0,c.id,c.line_number) line_number,
							 {4}
							FROM
							  {0} C
							LEFT JOIN (SELECT id, section, is_checked FROM quote_section WHERE quote_id = @v0 AND revision = @v1) D
								ON C.section_id = D.id
							LEFT JOIN inventory_item_master im
								ON im.master_id = C.part_no
							LEFT JOIN inventory_tag
								ON inventory_tag.tag_id = im.tag_id
							LEFT JOIN (select internal_org_item_id, avg_price, recommended_price, week_of from item_cost_upload where branch=(select name from business_unit where id=(select business_unit_id from quote_master where quote_id=@v0 and revision=@v1))) icu
								ON icu.internal_org_item_id = C.part_no
							
							WHERE 
								C.quote_id = @v0 AND 
								C.revision = @v1 AND 
								D.is_checked =1 AND
								D.section REGEXP '^[^0-9]' 	
							ORDER BY 
								line_number
							)", used_table, see_cost_c, see_cost_a, is_exculde_a, is_exculde_c), _id, _rev);
			totalTM = 0;
			totalQuote = 0;
			totalMaterial = 0;
			totalLabor = 0;
			totalKitted = 0;
			linecount = 0;
			custlinecount = 0;
			show_margin = true;
			hourstotal = 0;
			quote_price = Convert.ToDouble(quote_info["quoted_price"]);
			totalCost = 0.0;
			foreach (var row in items)
			{
				linecount++;
				var part_no = 0;

				int.TryParse(row.part_no, out part_no);
				if (row.part_no == "")
				{
					custlinecount++;
				}
				totalTM += row.extd2;
				totalQuote += row.extd;
				totalCost += row.cost * row.qty;
				if (part_no >= 990000 && part_no < 2000000)
				{
					//	totalLabor += Convert.ToDouble(row["extTandM"]);
					totalLabor += row.extd;
					hourstotal += row.qty;
				}
				else if (part_no >= 2000000) // Kit.... we need to dissect it.
				{
					var kit_qty = row.qty;
					var _kit =
						bllToolbox.doSQL_dt(
							@"SELECT inventory_kit_dtl_master_id master_id, inventory_kit_dtl_qty qty 
							FROM inventory_kit_dtl WHERE inventory_kit_dtl_hdr_id = @v0  AND inventory_kit_dtl_active = TRUE", part_no);
					foreach (DataRow _dtl in _kit.Rows)
					{
						var master_id = (int)_dtl["master_id"];
						var qty = Convert.ToDouble(_dtl["qty"]);
						if (master_id >= 990000)
						{
							hourstotal += qty * kit_qty;
						}
					}
					totalKitted += row.extd;
				}
				else
				{
					//	totalMaterial += Convert.ToDouble(row["extTandM"]);
					totalMaterial += row.extd;
				}
			}
			benchextd = totalQuote - totalTM;
			totalMaterialDisp = totalMaterial.ToString("C2");

			if (show_margin == false)
			{
				margAbovCosDisp = "N/A with $0 Cost";
				margAbovCosDisp_dollars = "N/A with $0 Cost";

			}
			else
			{
				if (member_can_see_sell)
				{
					try
					{
						CostDisp = bllToolbox.doSQL_double(@"SELECT IFNULL(sum(cost*qty),0) as margin FROM " + used_table + @"  WHERE quote_id =@v0 AND revision =@v1 and is_checked=1 ", _quote.QuoteID, _quote.Revision).ToString("C2");

						margAbovCosDisp = bllToolbox.doSQL_double(@"SELECT IFNULL(((@v2-sum(cost*qty))/@v2),0) as margin FROM " + used_table + @"  WHERE quote_id =@v0 AND revision =@v1  and is_checked=1", _quote.QuoteID, _quote.Revision, _quote.Price).ToString("P2");
						margAbovCosDisp_dollars = (bllToolbox.doSQL_double(@"SELECT IFNULL(((@v2-sum(cost*qty))/@v2),0) as margin FROM " + used_table + @"  WHERE quote_id =@v0 AND revision =@v1  and is_checked=1", _quote.QuoteID, _quote.Revision, _quote.Price) * Convert.ToDouble(_quote.Price)).ToString("C2");
						var margin_labour = bllToolbox.doSQL_double(@"SELECT IFNULL(((sum(extended_per)-sum(cost*qty))/sum(extended_per)),0) as margin FROM " + used_table + @" WHERE part_no >=990000 and part_no <2000000 and quote_id =@v0 AND revision =@v1  and is_checked=1", _quote.QuoteID, _quote.Revision);
						totalLaborDisp = "(" + margin_labour.ToString("P0") + ")";

						var margin_material = bllToolbox.doSQL_double(@"SELECT IFNULL(((sum(extended_per)-sum(cost*qty))/sum(extended_per)),0) as margin FROM " + used_table + @"  WHERE part_no <990000 and quote_id =@v0 AND revision =@v1  and is_checked=1", _quote.QuoteID, _quote.Revision);
						totalMaterialDisp = "(" + margin_material.ToString("P0") + ")";
					}
					catch
					{
						margAbovCosDisp = "N/A";
						margAbovCosDisp_dollars = "N/A";

					}

				}
			}
		}

		public double totalCost { get; set; }

		public double quote_price { get; set; }

		public string totalMaterialDisp { get; set; }

		public string totalLaborDisp { get; set; }

		public string CostDisp { get; set; }

		public string margAbovCosDisp_dollars { get; set; }

		public string margAbovCosDisp { get; set; }

		public double benchextd { get; set; }

		public double hourstotal { get; set; }

		public bool show_margin { get; set; }

		public int custlinecount { get; set; }

		public int linecount { get; set; }

		public double totalKitted { get; set; }

		public double totalLabor { get; set; }

		public double totalMaterial { get; set; }

		public double totalQuote { get; set; }

		public double totalTM { get; set; }



	}
}