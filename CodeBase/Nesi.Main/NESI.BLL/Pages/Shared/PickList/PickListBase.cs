using System;
using System.Collections.Generic;
using System.Data;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Shared.PickList;

namespace NESI.BLL.Pages.Shared.PickList
{
	public class PickListBase : BLLBase
	{
		public PickListItem[] items { get; set; }

		public LabelValueInt[] section { get; set; }
		public LabelValueInt[] sectionFilter { get; set; }

		public string Title { get; set; }
		public string custNameDisplay { get; set; }
		public string memberNameDisplay { get; set; }
		public bool addnewLine { get; set; }

		public bool can_receive { get; set; }

		public bool member_can_discount { get; set; }

		public bool member_can_see_labour_cost { get; set; }

		public bool member_can_see_sell { get; set; }

		public bool member_can_edit_sell { get; set; }

		public bool member_can_see_cost { get; set; }

		public bool can_edit_specific_section_button { get; set; }


		protected NeBusinessUnit WorkingBusinessUnit { get; set; }
		protected NeBusinessUnit WarehouseBusinessUnit { get; set; }
		public string labourName { get; set; }
		public List<LabelValueInt> sources { get; set; }

		public int buId { get; set; }
		public double NotIncludeSectionTotal { get; set; }

		public PickListBase(Employee user) : base(user)
		{
			labourName = CurrentUser.BusinessUnit.Country.Equals("USA") ? "Labor" : "Labour";
			sources = new List<LabelValueInt>
			{
				new LabelValueInt() {Label = "Select source", Value = 0},
				new LabelValueInt() {Label = "Material", Value = 1},
				new LabelValueInt() {Label = labourName, Value = 2},
				new LabelValueInt() {Label = "Kitted", Value = 3},
				new LabelValueInt() {Label = "Group", Value = 4},
				new LabelValueInt() {Label = "Quotes", Value = 5},
				new LabelValueInt() {Label = "Work Orders", Value = 6},
				new LabelValueInt() {Label = "Purchase Orders", Value = 7},
				new LabelValueInt() {Label = "Vendor RFQ", Value = 8},
				new LabelValueInt() {Label = "Repair", Value = 9},
				new LabelValueInt() {Label = "Custom Line", Value = 10}
			};

			member_can_see_cost = CurrentUser.AuthenticatedForPrivilege(58);
			member_can_edit_sell = CurrentUser.AuthenticatedForPrivilege(30);
			member_can_see_sell = CurrentUser.AuthenticatedForPrivilege(81);
			member_can_see_labour_cost = CurrentUser.AuthenticatedForPrivilege(93);
			member_can_discount = CurrentUser.AuthenticatedForPrivilege(94);
			can_receive = CurrentUser.AuthenticatedForPrivilege(189);
			can_edit_specific_section_button = CurrentUser.AuthenticatedForPrivilege(214);
		}

		public bool Can_Edit_SpecificSection(int buid)
		{
			var bu = new NeBusinessUnit(buid);
			var pm = bu.branch_manager.id;
			return CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault() || (pm > 0 && (UserId == pm || CurrentUser.IsSupervisor(pm))) || CurrentUser.IsEstimatingManager(UserId) || can_edit_specific_section_button == true;
		}

		public DataTable GetSpecificSections(int bu_id, int type)
		{
			return bllToolbox.doSQL_dt(
				@"Select id value, notes label, 0 is_checked from specific_notes where business_unit_id =@p0 and is_deleted=0 and type=@p1"
//+ (Can_Edit_SpecificSection(bu_id) ? @" UNION SELECT 0 value, '' label, 0 is_checked " : "")
, bu_id, type);
		}

		public string AddSpeicficSection(string notes, int bu_id, int type)
		{
			var notesTrimmed = notes.Trim();
			bllToolbox.doSQL_void(@"Insert into specific_notes (notes_name,notes,added_by,business_unit_id, type) values (@p0,@p1,@p2,@p3,@p4)",
				notesTrimmed.Length > 60 ? notesTrimmed.Substring(0, 60) : notesTrimmed, notesTrimmed, UserId, bu_id, type);
			return "Section has been added successfully.";
		}

		public string UpdateSpeicficSection(string notes, int id)
		{
			var notesTrimmed = notes.Trim();
			bllToolbox.doSQL_void(@"Update specific_notes set notes_name=@p0,notes=@p1,modified_by=@p2 where id=@p3",
				notesTrimmed.Length > 60 ? notesTrimmed.Substring(0, 60) : notesTrimmed, notesTrimmed, UserId, id);
			return "Section has been updated successfully.";
		}

		public string DeleteSpeicficSection(int id)
		{
			bllToolbox.doSQL_void(@"Update specific_notes set is_deleted=1,deleted_by=@p0 where id=@p1",
				UserId, id);
			return "Section has been deleted successfully.";
		}


		public LabelValueInt[] GetMemberTypes(int bu_id)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				@"SELECT DISTINCT m.MemberType_ID AS value, m.MemberType_Name AS label 
				FROM membertype m inner join membertype_chargeout c
				on m.membertype_id=c.membertype_id
				WHERE m.MemberType_Name NOT LIKE '%Do Not%' and m.active =1 
				and (m.is_scheduled=1 or m.is_team_leader=1 or m.considered_pm=1 or m.show_on_ratesheet=1) 
				and c.business_unit_id=@v0
				order by IFNULL((SELECT SUM(wo_detail_history_qty_committed) FROM wo_detail_history WHERE wo_detail_history_master_id=c.id),0) desc, m.Membertype_Name", bu_id);
		}

		//public LabelValueInt[] getPayTypeHours()
		//{
		//	return bllToolbox.doSQL_Array<LabelValueInt>(
		//		@"SELECT PayTypeHours_ID, Description FROM paytypehours order by PayTypeHours_ID");
		//}

		protected const string MEMBERTYPE_HOURS = @"SELECT
			c.business_unit_id,
			get_customer_chargeout(@p2,c.id) `chargeout`,
			c.`id` master_id,
			c.`membertype_id`,
			c.`paytype_id`,
			t.`membertype_name`,
			p.`Description` paytype_name,
			p.`multiplier`,
			null qty,
			'true' is_checked
			FROM
				membertype_chargeout c
				INNER JOIN membertype t
				ON  c.`membertype_id` = t.`membertype_id`
			INNER JOIN paytypehours p
			ON c.`paytype_id` = p.paytypehours_id ";

		protected const string MEMBERTYPE_HOURS2 = @"SELECT
			c.business_unit_id,
			c.id `chargeout`,
			c.`id` master_id,
			c.`membertype_id`,
			c.`paytype_id`,
			t.`membertype_name`,
			p.`Description` paytype_name,
			p.`multiplier`,
			null qty,
			'true' is_checked
			FROM
				membertype_chargeout c
				INNER JOIN membertype t
				ON  c.`membertype_id` = t.`membertype_id`
			INNER JOIN paytypehours p
			ON c.`paytype_id` = p.paytypehours_id ";
		public int GetCustomerId(int qId, int rev)
		{
			return bllToolbox.doSQL_int(@"select customer_id from quote_master where quote_id=@p0 and revision=@p1", qId, rev);

		}

		public PickListMemberTypeLaborHour[] GetMemberTypeHours(int bu_id, int membertype_id, int quoteId, int revision)
		{
			var list = bllToolbox.doSQL_List<PickListMemberTypeLaborHour>(
				MEMBERTYPE_HOURS + " WHERE c.business_unit_id = @v0 and c.membertype_id=@v1", bu_id, membertype_id, GetCustomerId(quoteId, revision));
			//			list.ForEach(x => x.chargeout = this.member_can_see_cost && this.member_can_see_labour_cost ? x.chargeout : 0);
			return list.ToArray();
		}


		public LabelValueInt[] GetKittedDDLs()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"(SELECT 
			inventory_kit_hdr_ID AS value, 
			CONCAT('Your Kit - ', inventory_kit_hdr_Name ) AS label
			FROM inventory_kit_hdr WHERE inventory_kit_hdr_created_by = @v0 
			order by inventory_kit_hdr_name) 
			UNION
			(SELECT inventory_kit_hdr_ID AS value, 
			inventory_kit_hdr_Name AS label 
			FROM inventory_kit_hdr WHERE inventory_kit_hdr_created_by != @v0  
			order by inventory_kit_hdr_name)", UserId);
		}

		public PicklistGroupItem[] GetPartsByKitted(int buid, int id)
		{
			var list = bllToolbox.doSQL_List<PicklistGroupItem>(
				@" SELECT 
				inventory_kit_dtl_master_id master_id, 
				if(inventory_kit_dtl_master_id >= 990000, inventory_labor_desc(inventory_kit_dtl_master_id),full_part_description(inventory_kit_dtl_master_id, false, @v1 )) description, 
				inventory_kit_dtl_qty qty,
				'true' is_checked,
GET_COST_at_qty(a.inventory_kit_dtl_master_id,@p2,0,1) cost,
GET_COST_at_qty(a.inventory_kit_dtl_master_id ,@p2,1,1) cost_level,
getsellprice(GET_COST_at_qty(a.inventory_kit_dtl_master_id,@p2,0,1),0,true,1,@p2) sell,
ifnull(h.onhand_qty,0) onhand,
ifnull(h.int_onhand_qty,0) int_onhand_qty,
ifnull(h.ext_onhand_qty,0) ext_onhand_qty,
0 extd,
0 extd2,
'' section_id,
0 discount
				FROM inventory_kit_dtl  a
left join inventory_branch h on  h.master_id=a.inventory_kit_dtl_master_id and h.business_unit_id=@p2 
				WHERE inventory_kit_dtl_hdr_id = @v0  
				AND inventory_kit_dtl_active = true",
				id, CurrentUser.BusinessUnit.Country, buid
				);
			return CheckItemPreviliges(list);
		}


		public LabelValueInt[] GetGroupDDL()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT ID AS value, Name as label FROM inventory_group_hdr order by name");
		}

		public PicklistGroupItem[] GetPartsByGroup(int buid, int id)
		{
			var query =
				@"Select a.master_id, 
				full_part_description_with_labour(a.master_id,false,@v1) as description, 
				0 as qty,
				'true' as is_checked,
GET_COST_at_qty(a.master_id,@p2,0,1) cost,
GET_COST_at_qty(a.master_id ,@p2,1,1) cost_level,
getsellprice(GET_COST_at_qty(a.master_id,@p2,0,1),0,true,1,@p2) sell,
ifnull(h.onhand_qty,0) onhand,
ifnull(h.int_onhand_qty,0) int_onhand_qty,
ifnull(h.ext_onhand_qty,0) ext_onhand_qty,
0 extd,
0 extd2,
'' section_id,
0 discount
				from Inventory_group_dtl a 
left join inventory_branch h on  h.master_id=a.master_id and h.business_unit_id=@p2 
where a.group_id = @v0 ORDER BY id ASC";
			var list = bllToolbox.doSQL_List<PicklistGroupItem>(query, id, NeBusinessUnit.GetbuCountry(buid.ToString()), buid);
			return CheckItemPreviliges(list);
		}


		public PicklistGroupItem[] CheckItemPreviliges(List<PicklistGroupItem> list)
		{
			if (!member_can_see_cost)
			{
				list.ForEach(x => x.cost = 0);
			}
			if (!member_can_see_sell)
			{
				list.ForEach(x => x.sell = 0);
			}
			list.ForEach(x => x.extd = x.sell * x.qty);
			return list.ToArray();
		}

		public LabelValueInt[] GetCustomersByBuId(int buid)
		{
			var strsql = @"SELECT c.Customer_ID AS value, c.Customer_Name AS label FROM 
						 customer AS c  
						 Inner Join quote_master ON c.Customer_ID = quote_master.customer_id 
						 WHERE (quote_master.quoted_by=@p0 or find_in_set(quote_master.business_unit_id, @p1)) GROUP BY c.Customer_ID ORDER BY label ASC";
			return bllToolbox.doSQL_Array<LabelValueInt>(strsql, UserId, CurrentUser.VisibleBusinessUnits);
		}

		const string SEARCH_QUOTE = @"SELECT DISTINCT cast(concat(quote_master.quote_id,quote_master.revision) as unsigned) as value, 
			concat(b.ddl_name,' - ', cast(quote_master.quote_id as CHAR),'v',Cast(quote_master.revision as CHAR),' - ',quote_master.job_description) as label
			FROM quote_master 
inner join business_unit b on quote_master.business_unit_id=b.id
			where  (quote_master.quoted_by=@p1 or find_in_set(quote_master.business_unit_id, @p2)) and {0}
			order by label";

		public LabelValueInt[] GetCustomerQuotesByCustomerId(int custId)
		{

			return bllToolbox.doSQL_Array<LabelValueInt>(
				string.Format(SEARCH_QUOTE, " customer_id=@v0")
				, custId, UserId, CurrentUser.VisibleBusinessUnits);
		}

		public LabelValueInt[] GetQuotesByQuery(string query)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				string.Format(SEARCH_QUOTE, @"
			CONCAT(
			CAST(quote_master.quote_id AS CHAR),
			' V',
			CAST(quote_master.revision AS CHAR),
			'  ',
			quote_master.job_description
				) like CONCAT('%',@p0,'%')")
				, query, UserId, CurrentUser.VisibleBusinessUnits);
		}

		public PicklistGroupItem[] GetQuoteParts(int buid, string partno)
		{
			var sql = $@"Select distinct a.id AS groupselectid,
					a.part_no as master_id,
					a.description as description,
					a.qty, 
					0 RepairID,
					'true' is_checked,
GET_COST_at_qty(a.part_no,@p1,0,1) cost,
GET_COST_at_qty(a.part_no ,@p1 ,1,1) cost_level,
getsellprice(GET_COST_at_qty(a.part_no,@p1,0,1),0,true,1,@p1) sell,
ifnull(h.onhand_qty,0) onhand,
ifnull(h.int_onhand_qty,0) int_onhand_qty,
ifnull(h.ext_onhand_qty,0) ext_onhand_qty,
0 extd,
0 extd2,
b.section section_name,
a.section_id section_id,
0 discount
					from quote_worksheet a
left join inventory_branch h on  h.master_id=a.part_no and h.business_unit_id=@v2 
					left join quote_section b ON a.section_id = b.id
					where a.part_no!='' 
					and a.part_no is not null
					and a.quote_id = @v0 and 
					a.revision = @v1
					order by  CAST(b.section as SIGNED INTEGER) ASC, a.id";
			var list = bllToolbox.doSQL_List<PicklistGroupItem>(sql, partno.Remove(6), partno.Substring(6, 1), buid);
			return CheckItemPreviliges(list);

		}

		const string SEARCH_WOS = @"
					SELECT 
						a.WOProg_ID AS value, 
						CONCAT('(0',CAST(a.woprog_bvwo AS UNSIGNED),') ', TRIM(IFNULL(a.woprog_description,'Unknown Description')),'  (', IF(COUNT(b.wo_detail_current_id)=0,COUNT(c.wo_detail_history_id),COUNT(b.wo_detail_current_id)),' items)') AS label
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
						{0} AND
						COALESCE(b.wo_detail_current_master_id, c.wo_detail_history_master_id) < 990000 AND 
						COALESCE(b.wo_detail_current_master_id, c.wo_detail_history_master_id) != 2139 AND
						COALESCE(d.active, f.active) = 1
					GROUP BY 
						a.woprog_id 
					HAVING 
						COUNT(b.wo_detail_current_id) > 0 OR COUNT(c.wo_detail_history_id) > 0
					ORDER BY 
						woprog_bvwo DESC";

		public LabelValueInt[] GetCustomerWosByCustomerId(int custId)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
			string.Format(SEARCH_WOS, "a.woprog_customer_id=@v0"), custId);
		}

		public LabelValueInt[] GetWosByQuery(string query)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				string.Format(SEARCH_WOS, @"(a.woprog_description LIKE CONCAT('%',@v0,'%') OR a.woprog_bvwo LIKE CONCAT('%',@v0,'%'))")
				, query);
		}

		public PicklistGroupItem[] GetWoParts(int buid, string partno)
		{
			var is_invoiced =
				bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM woprog WHERE woprog_id = @v0  AND woprog_status in ('Invoiced', 'Waiting to be Invoiced')", partno) > 0;
			var table_name = is_invoiced ? "history" : "current";
			var sql = string.Format(@"
					SELECT 
					  a.wo_detail_{0}_id AS groupselectid,
					  a.wo_detail_{0}_master_id AS master_id,
					  a.wo_detail_{0}_description AS description,
					  a.wo_detail_{0}_qty_committed AS Qty,
					  0 RepairID,
					  'true' is_checked,
GET_COST_at_qty(a.wo_detail_{0}_master_id,@p1,0,1) cost,
GET_COST_at_qty(a.wo_detail_{0}_master_id ,@p1 ,1,1) cost_level,
getsellprice(GET_COST_at_qty(a.wo_detail_{0}_master_id,@p1,0,1),0,true,1,@p1) sell,
ifnull(h.onhand_qty,0) onhand,
ifnull(h.int_onhand_qty,0) int_onhand_qty,
ifnull(h.ext_onhand_qty,0) ext_onhand_qty,
0 extd,
0 extd2,
'' section_id,
0 discount					FROM
					  wo_detail_{0} a
					INNER JOIN inventory_item_master b ON a.wo_detail_{0}_master_id = b.master_id 
					INNER JOIN inventory_tag c ON b.tag_id = c.tag_id 
inner join inventory_branch h on  h.master_id=b.master_id and h.business_unit_id=@v1 
					WHERE 
						a.wo_detail_{0}_woprog_id =@v0 AND
						a.wo_detail_{0}_master_id < 990000 AND 
						a.wo_detail_{0}_master_id != 2139 AND
						b.active = 1", table_name);
			var list = bllToolbox.doSQL_List<PicklistGroupItem>(sql, partno, buid);
			return CheckItemPreviliges(list);
		}


		const string SEARCH_PARTS = @"
	SELECT 
		a.master_id,
		a.master_id value,
		IFNULL(a.active, 0) active,
		CAST(IFNULL((Select p.po_details_date_added from po_details_current p INNER join poprog_header po on p.po_details_poprog_id = po.poprog_id and p.business_unit_id = @v1 where po_details_part_no = @v0 order by p.po_details_date_added desc limit 1)  , '--') AS CHAR(20)) vendor_price_last_dt,
		a.tag_id,
		e.tag,
		e.canadian_sold_as,
		a.old_id,
		a.new_id,
		e.usa_sold_as,
		f.unit canadian_sold_as_name,
		g.unit usa_sold_as_name,
		e.is_qty,
		e.is_exclude,
		e.is_static_sellprice,
		IFNULL(b.benchmark, 0) benchmark,
		IFNULL(b.vendor_id, '0') vendor_id,
		FORMAT(IFNULL(b.cost, 0), 5) cost,
		IFNULL(c.sellprice, 0) sellprice,
		IFNULL(b.qty, 1) qty,
		k.description,
		CONCAT(a.master_id,'-',k.description) label,
		k.desc_full_{0} description_full,
		IFNULL(h.onhand_qty, 0) onhand_qty,
		IFNULL(h.dollar_balance, 0) dollar_balance,
		COUNT(i.id) n_pics,
		(SELECT COUNT(id) FROM inventory_location WHERE master_id = a.master_id AND business_unit_id = @v1 AND min > 0 AND max > 0) n_minmax,
		(SELECT count(*) FROM inventory_location WHERE master_id = @v0 AND business_unit_id = @v1 AND (int_min > 0 OR ext_min > 0)) > 0 is_stocked,
e.allowed_to_stock,
h.ext_onhand_qty,
h.int_onhand_qty,
h.wo_usage wo_usage,
h.po_usage po_usage,
e.allowed_to_edit_after_issue
	FROM 
		inventory_item_master a 
	LEFT JOIN 
		inventory_price b 
			ON a.master_id = b.master_id AND
			b.business_unit_id = @v1
	LEFT JOIN 
		inventory_sellprice c 
			ON a.master_id = c.master_id AND
			c.business_unit_id = @v1
	LEFT JOIN
		inventory_tag e
			ON e.tag_id = a.tag_id
	LEFT JOIN
		inventory_sold_as f
			ON e.canadian_sold_as = f.id
	LEFT JOIN
		inventory_sold_as g
			ON e.usa_sold_as = g.id
	LEFT JOIN
		inventory_branch as h
			ON a.master_id = h.master_id AND
			h.business_unit_id = @v1
	LEFT JOIN
		inventory_picture i
			ON a.master_id = i.master_id
	LEFT JOIN
		inventory_price_history j ON
			j.master_id = a.master_id AND
			j.business_unit_id = @v1
	LEFT JOIN
		inventory_description k ON
			k.master_id = a.master_id

	WHERE 
		{1}
	ORDER BY b.cost";


		public LabelValueInt[] GetPartsBySearchNumber(string query)
		{
			var country = this.WarehouseBusinessUnit.country;
			var sql = string.Format(SEARCH_PARTS, country, "a.master_id like CONCAT('%', @v0, '%') "); //OR k.description like CONCAT('%', @v0, '%')
			return bllToolbox.doSQL_Array<LabelValueInt>(sql, query, WarehouseBusinessUnit.id);
		}

		public DataTable GetPartsByMasterId(int master_id)
		{
			var country = this.WarehouseBusinessUnit.country;
			var sql = string.Format(SEARCH_PARTS, country, "a.master_id =@v0");
			return bllToolbox.doSQL_dt(sql, master_id, WarehouseBusinessUnit.id);
		}

		public byte[] GetInventroyPicutre(int masterId)
		{
			var _image = bllToolbox.doSQL_BLOB(
								 @"SELECT CAST(UNCOMPRESS(picture) AS CHAR) pic FROM inventory_picture  WHERE master_id =@v0 AND active ORDER BY insert_dt DESC limit 1",
								 masterId) ?? bllToolbox.doSQL_BLOB(@"SELECT CAST(UNCOMPRESS(picture) AS CHAR) pic FROM inventory_picture  WHERE master_id is null AND active");

			return _image;

		}
	}
}