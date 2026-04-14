using System;
using AutoMapper.Attributes;
// ReSharper disable InconsistentNaming

namespace NESI.DTO.Models.Quote
{
    [MapsTo(typeof(NESI.Data.Entities.quote_schedule))]
    [MapsFrom(typeof(NESI.Data.Entities.quote_schedule))]
   public  class QuoteSchedule
    {
        public int id { get; set; }
        public int? quoteid { get; set; }
        public int? revision { get; set; }
        public DateTime? dt { get; set; }
        public int? memberid { get; set; }
        public int? pointperson { get; set; }
        public int? estimator { get; set; }
        public double? allowedquotetime { get; set; }
        public DateTime? opendate { get; set; }
        public int? openmid { get; set; }
        public string opennotes { get; set; }
        public DateTime? stage1screening_date { get; set; }
        public int? stage1screening_mid { get; set; }
        public string stage1screening_notes { get; set; }
        public DateTime? schedule_produced_date { get; set; }
        public int? schedule_produced_mid { get; set; }
        public string schedule_produced_notes { get; set; }
        public DateTime? schedule_produced_date_completed { get; set; }
        public DateTime? manpower_information_collected_date { get; set; }
        public int? manpower_information_collected_mid { get; set; }
        public string manpower_information_collected_notes { get; set; }
        public DateTime? manpower_information_collected_date_completed { get; set; }
        public DateTime? customer_info_collected_date { get; set; }
        public int? customer_info_collected_mid { get; set; }
        public string customer_info_collected_notes { get; set; }
        public DateTime? customer_info_collected_date_completed { get; set; }
        public DateTime? market_info_collected_date { get; set; }
        public int? market_info_collected_mid { get; set; }
        public string market_info_collected_notes { get; set; }
        public DateTime? market_info_collected_date_completed { get; set; }
        public DateTime? finance_info_collected_date { get; set; }
        public int? finance_info_collected_mid { get; set; }
        public string finance_info_collected_notes { get; set; }
        public DateTime? finance_info_collected_date_completed { get; set; }
        public DateTime? recon_report_created_date { get; set; }
        public int? recon_report_created_mid { get; set; }
        public string recon_report_created_notes { get; set; }
        public DateTime? recon_report_created_date_completed { get; set; }
        public DateTime? stage4_gono_date { get; set; }
        public string stage4_gono_notes { get; set; }
        public DateTime? stage4_gono_date_completed { get; set; }
        public DateTime? quote_delivery_strategy_date { get; set; }
        public int? quote_delivery_strategy_mid { get; set; }
        public string quote_delivery_strategy_notes { get; set; }
        public DateTime? quote_delivery_strategy_date_completed { get; set; }
        public DateTime? project_estimated_date { get; set; }
        public int? project_estimated_mid { get; set; }
        public string project_estimated_notes { get; set; }
        public DateTime? project_estimated_date_completed { get; set; }
        public DateTime? worksheet_review_date { get; set; }
        public int? worksheet_review_mid { get; set; }
        public string worksheet_review_notes { get; set; }
        public DateTime? worksheet_review_date_completed { get; set; }
        public DateTime? stage6_final_review_date { get; set; }
        public int? stage6_final_review_mid { get; set; }
        public string stage6_final_review_notes { get; set; }
        public DateTime? stage6_final_date_completed { get; set; }
        public DateTime? quote_delivered_date { get; set; }
        public int? quote_delivered_mid { get; set; }
        public string quote_delivered_notes { get; set; }
        public DateTime? quote_delivered_date_completed { get; set; }
        public DateTime? followup1_date { get; set; }
        public int? followup1_mid { get; set; }
        public string followup1_notes { get; set; }
        public DateTime? followup2_date { get; set; }
        public int? followup2_mid { get; set; }
        public string followup2_notes { get; set; }
        public DateTime? convert_or_kill_date { get; set; }
        public int? convert_or_kill_mid { get; set; }
        public string convert_or_kill_notes { get; set; }
        public DateTime? convert_or_kill_date_completed { get; set; }
        public DateTime? post_mortem_complete_date { get; set; }
        public int? post_mortem_complete_mid { get; set; }
        public string post_mortem_complete_notes { get; set; }
        public DateTime? post_mortem_complete_date_completed { get; set; }
        public int? rt1 { get; set; }
        public int? rt2 { get; set; }
        public int? rt3 { get; set; }
        public int? rt4 { get; set; }
        public DateTime? rt1_s4_approved { get; set; }
        public DateTime? rt2_s4_approved { get; set; }
        public DateTime? rt3_s4_approved { get; set; }
        public DateTime? rt4_s4_approved { get; set; }
        public DateTime? rt1_f_approved { get; set; }
        public DateTime? rt2_f_approved { get; set; }
        public DateTime? rt3_f_approved { get; set; }
        public DateTime? rt4_f_approved { get; set; }
        public string estimating_concerns { get; set; }
        public string sell_strategy { get; set; }
    }
}
