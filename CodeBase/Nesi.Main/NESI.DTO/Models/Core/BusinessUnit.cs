using System;
using AutoMapper.Attributes;
// ReSharper disable InconsistentNaming

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(NESI.Data.Entities.business_unit))]
	[MapsFrom(typeof(NESI.Data.Entities.business_unit))]
	public class BusinessUnit
	{
        //DONT DELETE
	    public string tax_entity_name { get; set; }
	    public string tax_no { get; set; }


        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsTest { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string Prov { get; set; }
        public string state { get; set; }
        public string postal { get; set; }
        public string Country { get; set; }
        public string PhoneArea { get; set; }
        public string PhoneFirst { get; set; }
        public string PhoneLast { get; set; }
        public string PhoneAltArea { get; set; }
        public string PhoneAltFirst { get; set; }
        public string PhoneAltLast { get; set; }
        public string FaxArea { get; set; }
        public string FaxFirst { get; set; }
        public string FaxLast { get; set; }
        public int? Member_ID { get; set; }
        public DateTime? DateTime { get; set; }
        public int? Ord { get; set; }
        public string wopath { get; set; }
        public string popath { get; set; }
        public string rootdirectory { get; set; }
        public string pathtimesheet { get; set; }
        public string Revenue { get; set; }
        public string workorderprinter { get; set; }
        public string laserprinter { get; set; }
        public string barcodeprinter { get; set; }
        public string Active { get; set; }
        public int? has_inventory { get; set; }
        public int? enable_timesheet { get; set; }
        public bool? EmailCollections { get; set; }
        public bool? EmailTimesheets { get; set; }
        public bool? EmailExpediteReport { get; set; }
        public bool? EmailIncomeStatement { get; set; }
        public string IsRegional_Branch { get; set; }
        public string region { get; set; }
        public int? TaxLabour { get; set; }
        public int? TaxQuotedJobs { get; set; }
        public int? selfassess_tax { get; set; }
        public int? tax_material_only_wos { get; set; }
        public int? TaxMaterial { get; set; }
        public bool? fvr_lockout { get; set; }
        public bool? uses_quote_process { get; set; }
        public double? quote_level_2_start { get; set; }
        public double? quote_level_3_start { get; set; }
        public int? mktphone_area { get; set; }
        public int? mktphone_first { get; set; }
        public int? mktphone_last { get; set; }
        public string email_rush { get; set; }
        public string email_rfq { get; set; }
        public string email_service { get; set; }
        public string email_consult { get; set; }
        public string email_job { get; set; }
        public int? use_benchmark_for_pm_comp { get; set; }
        public bool? show_ratepanel { get; set; }
        public double? perdiem_rate { get; set; }
        public string default_material_sell_gl { get; set; }
        public string default_labour_sell_gl { get; set; }
        public int? default_currency { get; set; }
        public double? nesi_fee { get; set; }
        public int? timezone_id { get; set; }
        public int tax_entity_id { get; set; }
        public string ddl_Name { get; set; }
        public int? old_div { get; set; }
        public int? old_company_id { get; set; }
        public int? allow_open_scheduling { get; set; }
        public string gl_div { get; set; }
        public string old_dsn { get; set; }
        public bool? is_panelshop { get; set; }
        public bool? is_er { get; set; }
        public int? warehouse_bu_id { get; set; }
        public string logo_file { get; set; }
        public bool? is_service { get; set; }
        public bool? is_backoffice { get; set; }
        public bool? use_er_wo_logic { get; set; }
        public bool? allow_unlinked_timesheet { get; set; }
        public int? acting_manager { get; set; }
        public int? acting_purchaser { get; set; }
        public int? acting_right_hand { get; set; }
        public int? acting_safety_officer { get; set; }
        public bool? is_corporate { get; set; }
        public string web_domain { get; set; }
        public string header_color { get; set; }
        public sbyte? send_surveys { get; set; }
        public int? GL_Default_Material_Revenue { get; set; }
        public int? GL_Default_Material_Expense { get; set; }
        public int? GL_Default_Labour_Revenue { get; set; }
        public int? GL_Default_Labour_Expense { get; set; }
        public int? GL_Default_Subcontract_Revenue { get; set; }
        public int? GL_Default_Subcontract_Expense { get; set; }
        public int? NetSuite_BU_Internal_Id { get; set; }
        public int? default_fvr_template_id { get; set; }
        public string default_onboarding_email { get; set; }
        public string default_fvr_template_ids { get; set; }
        public string contractor_license { get; set; }
        public string masters_license { get; set; }
        public string ESA_id { get; set; }
        public string TSSA_id { get; set; }
        public string CSA_ID { get; set; }
        public bool? allow_fixed_labour { get; set; }
        public bool? allow_fixed_markup { get; set; }
        public bool? allow_mixed_chargeouts { get; set; }
        public bool? allow_email_edit { get; set; }
        public bool? allow_cell_edit { get; set; }
        public string remit_to_address { get; set; }
        public bool? uses_payroll { get; set; }

        public bool show_daily_approval { get; set; }
        public bool allow_bankedpay { get; set; }

        public bool allow_vac_withd { get; set; }

    }
}