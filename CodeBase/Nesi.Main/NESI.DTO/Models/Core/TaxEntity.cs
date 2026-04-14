using System;
using AutoMapper.Attributes;
// ReSharper disable InconsistentNaming

namespace NESI.DTO.Models.Core
{
	[MapsTo(typeof(NESI.Data.Entities.tax_entity))]
	[MapsFrom(typeof(NESI.Data.Entities.tax_entity))]
	public class TaxEntity
	{
        public int id { get; set; }
        public string public_name { get; set; }
        public string ddl_name { get; set; }
        public string nesi_nickname { get; set; }
        public string tax_id_no { get; set; }
        public bool? is_test { get; set; }
        public string dsn { get; set; }
        public bool? is_active { get; set; }
        public int? YearEnd_Month { get; set; }
        public int? tax1 { get; set; }
        public int? tax2 { get; set; }
        public int? tax3 { get; set; }
        public int? tax4 { get; set; }
        public bool? sync_customers { get; set; }
        public bool? sync_vendors { get; set; }
        public bool? is_holdco { get; set; }
        public bool? uses_folders { get; set; }
        public int? gl_default_revenue { get; set; }
        public int? gl_default_liability { get; set; }
        public int? gl_default_expense { get; set; }
        public int? GL_Default_Subcontract_Expense { get; set; }
        public int? GL_Default_Subcontract_Revenue { get; set; }
        public int? GL_Default_Labour_Expense { get; set; }
        public int? GL_Default_Labour_Revenue { get; set; }
        public int? GL_Default_Material_Expense { get; set; }
        public int? GL_Default_Material_Revenue { get; set; }
        public int? GL_Default_Asset { get; set; }
        public string region { get; set; }
        public string adp_company_code { get; set; }
        public int? batch_n { get; set; }
        public bool? allow_interbu_ts { get; set; }
        public double? default_mileage_rate { get; set; }
        public int? default_fvr_template_id { get; set; }
        public string default_onboarding_email { get; set; }
        public string default_fvr_template_ids { get; set; }
        public double? markup_formula_min_markup { get; set; }
        public double? markup_formula_top { get; set; }
        public double? markup_formula_qty_dim { get; set; }
        public double? markup_formula_offset { get; set; }
        public double? markup_formula_exp { get; set; }
        public double? markup_formula_max { get; set; }
        public bool? uses_wip_accounting { get; set; }
        public int? NetSuite_TE_Internal_Id { get; set; }
        public sbyte? include_owners_in_disc_emails { get; set; }
    }
}