using System;
using AutoMapper.Attributes;

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(NESI.Data.Entities.wo_detail_current))]
	[MapsFrom(typeof(NESI.Data.Entities.wo_detail_current))]
	public class WoDetailCurrent
    {

        public long wo_detail_current_id { get; set; }
        public int wo_detail_current_woprog_id { get; set; }
        public Nullable<int> wo_detail_current_rec_no { get; set; }
        public string wo_detail_current_type { get; set; }
        public int wo_detail_current_master_id { get; set; }
        public System.DateTime wo_detail_current_date_added { get; set; }
        public System.DateTime wo_detail_current_date_modified { get; set; }
        public string wo_detail_current_description { get; set; }
        public decimal wo_detail_current_qty_committed { get; set; }
        public decimal wo_detail_current_qty_invoiced { get; set; }
        public decimal wo_detail_current_price_cost { get; set; }
        public decimal wo_detail_current_price_sell { get; set; }
        public decimal wo_detail_current_price_unit { get; set; }
        public int wo_detail_current_added_by { get; set; }
        public int wo_detail_current_tax1 { get; set; }
        public int wo_detail_current_tax2 { get; set; }
        public int wo_detail_current_tax3 { get; set; }
        public int wo_detail_current_tax4 { get; set; }
        public Nullable<long> wo_detail_current_company_id { get; set; }
        public long wo_detail_current_bvwo { get; set; }
        public string wo_detail_current_code { get; set; }
        public string wo_detail_current_origin { get; set; }
        public Nullable<int> wo_detail_current_billtypeid { get; set; }
        public string wo_detail_current_issues { get; set; }
        public int memberid { get; set; }
        public long paytypeid { get; set; }
        public string wo_detail_current_notes { get; set; }
        public Nullable<decimal> wo_detail_current_qty_ordered { get; set; }
        public decimal wo_detail_current_discount { get; set; }
        public byte wo_detail_current_track_part { get; set; }
        public Nullable<System.DateTime> wo_detail_current_date_required { get; set; }
        public Nullable<int> wo_detail_current_consignment_id { get; set; }
        public Nullable<int> wo_detail_current_location_id { get; set; }
        public string wo_detail_current_added_by_module { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<bool> blocks_schedule { get; set; }
        public int? seg1_id { get; set; }
        public Nullable<int> seg2_id { get; set; }
        public Nullable<int> seg3_id { get; set; }
        public Nullable<int> division_id { get; set; }
        public int business_unit_id { get; set; }
	}
}