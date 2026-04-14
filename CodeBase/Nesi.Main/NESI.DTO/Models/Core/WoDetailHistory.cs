using System;
using AutoMapper.Attributes;

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(NESI.Data.Entities.wo_detail_history))]
	[MapsFrom(typeof(NESI.Data.Entities.wo_detail_history))]
	public class WoDetailHistory
    {
        public long wo_detail_history_id { get; set; }
        public long wo_detail_history_woprog_id { get; set; }
        public int wo_detail_history_rec_no { get; set; }
        public string wo_detail_history_type { get; set; }
        public int wo_detail_history_master_id { get; set; }
        public System.DateTime wo_detail_history_date_added { get; set; }
        public System.DateTime wo_detail_history_date_modified { get; set; }
        public string wo_detail_history_description { get; set; }
        public decimal wo_detail_history_qty_committed { get; set; }
        public decimal wo_detail_history_qty_invoiced { get; set; }
        public decimal wo_detail_history_price_cost { get; set; }
        public decimal wo_detail_history_price_sell { get; set; }
        public decimal wo_detail_history_price_unit { get; set; }
        public int wo_detail_history_added_by { get; set; }
        public long wo_detail_history_tax1 { get; set; }
        public long wo_detail_history_tax2 { get; set; }
        public long wo_detail_history_tax3 { get; set; }
        public long wo_detail_history_tax4 { get; set; }
        public Nullable<long> wo_detail_history_company_id { get; set; }
        public long wo_detail_history_bvwo { get; set; }
        public string wo_detail_history_code { get; set; }
        public string wo_detail_history_origin { get; set; }
        public long wo_detail_history_billtypeid { get; set; }
        public string wo_detail_history_issues { get; set; }
        public long memberid { get; set; }
        public long paytypeid { get; set; }
        public string wo_detail_history_notes { get; set; }
        public Nullable<decimal> wo_detail_history_qty_ordered { get; set; }
        public decimal wo_detail_history_discount { get; set; }
        public byte wo_detail_history_track_part { get; set; }
        public Nullable<System.DateTime> wo_detail_history_date_required { get; set; }
        public Nullable<long> wo_detail_history_consignment_id { get; set; }
        public Nullable<int> wo_detail_history_location_id { get; set; }
        public string wo_detail_history_added_by_module { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<bool> blocks_schedule { get; set; }
        public Nullable<int> seg1_id { get; set; }
        public Nullable<int> seg2_id { get; set; }
        public Nullable<int> seg3_id { get; set; }
        public Nullable<int> division_id { get; set; }
        public int business_unit_id { get; set; }
    }
}