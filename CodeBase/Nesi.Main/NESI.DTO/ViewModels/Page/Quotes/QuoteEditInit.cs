
using System.Data;
using System.Text;
using NESI.DTO.ViewModels.Core;

namespace NESI.DTO.ViewModels.Page.Quotes
{
	public class QuoteEditInit
	{
		public bool print_draft_show;
		public bool HasError { get; set; }
		public string ErrorMessage { get; set; }
		public string ErrorTitle { get; set; }
		public bool PostMortemt { get; set; }
		public bool Popstage { get; set; }
		public bool Received_button_disabled { get; set; }
		public bool Hide_price_to { get; set; }
		public string wo_link { get; set; }
		public bool show_workseet_tab { get; set; }
		public string why_revised { get; set; }
		public string business_unit_id { get; set; }
        public string revenue_line_id { get; set; }

        public string quote_id { get; set; }
		public LabelValueInt[] Revisions { get; set; }

        public LabelValueInt[] revenueLines { get; set; }

        public string customer_id { get; set; }
		public string contact_id { get; set; }
		public string customer_contact => contact_id;
		public string open_date { get; set; }
		public string date_due { get; set; }
		public string date_expected_start { get; set; }
		public string job_description { get; set; }
		public string cust_spec_doc { get; set; }
		public string quoted_by { get; set; }
		public string quoted_by_name { get; set; }
		public string last_print_date { get; set; }
		public string verified_date { get; set; }
		public string hours_spent { get; set; }
		public string dollars_spent { get; set; }
		public string quoted_price { get; set; }
		public string price_to { get; set; }
		public LabelValueInt[] pricetype_select { get; set; }
		public string last_fax_date { get; set; }
		public string takeoff_price { get; set; }
		public string exp_podate { get; set; }
		public int currency { get; set; }
		public long ts_ticks { get; set; }
		public bool follow_up { get; set; }
        public bool is_tm { get; set; }
		public LabelValueInt[] build_percent_down { get; set; }
		public string tm_pricing { get; set; }
		public string percent_down { get; set; }
		public string net_due { get; set; }
		public string customer_term { get; set; }
		public string customer_name { get; set; }
		public string current_quoter { get; set; }
		public LabelValueInt[] contact_select { get; set; }
		public bool us_currency { get; set; }
		public bool inflation_term { get; set; }
		public string address_addr1 { get; set; }
		public string address_phoneNumber { get; set; }
		public string address_faxNumber { get; set; }
		public string contact_email { get; set; }
		public QuoteDetail[] divs1 { get; set; }
		public QuoteDetail[] divs2 { get; set; }
		public string quote_status_button { get; set; }
		public string status_id { get; set; }
		public string prev_status_id { get; set; }
		public string revision { get; set; }
		public string not_used { get; set; }
		public string po { get; set; }
		public string wo { get; set; }
		public double tm_pricing_total { get; set; }
		public string title_options { get; set; }
		public LabelValueInt[] company_list { get; set; }
		public string kill_reason__map { get; set; }
		public string competitor_id { get; set; }
		public string quoted_business_unit_id { get; set; }
		public string address_addr2 { get; set; }
		public string address_addr3 { get; set; }
		public string address_addr4 { get; set; }
		public string address_city { get; set; }
		public string address_prov { get; set; }
		public string address_postal { get; set; }
		public string address_country { get; set; }
		public string contact_cellPhone { get; set; }
		public string contact_extension { get; set; }
		public LabelValueInt[] term_options { get; set; }
		public string completion_date { get; set; }
		public string search_pan { get; set; }
		public string status_health { get; set; }
		public string status_title { get; set; }
		public string quoter_locked_str { get; set; }
		public string quoted_business_unit_id_locked { get; set; }
		public double worksheet_total { get; set; }
		public string show_help_checked { get; set; }
		public string active_revision { get; set; }
		public string service_report { get; set; }
		public bool include_title { get; set; }
		public string status { get; set; }
		public LabelValueInt[] address_options { get; set; }
        public string build_canned_descriptions { get; set; }
		public object build_percent_spinner { get; set; }
		public object expected_value_update { get; set; }
		public bool strategy_tab { get; set; }
		public string post_mortem_button { get; set; }
		public string print_buttons { get; set; }
		public LabelValueInt[] initial_quoted_by_list { get; set; }
		public bool is_Qced { get; set; }
		public bool is_actived { get; set; }
		public int chance_winning { get; set; }
		public string chance_winning_reason { get; set; }
		public string chance_winning_note { get; set; }
		public LabelValueInt[] chance_winning_why { get; set; }
		public string pricetype_id { get; set; }
		public double expected_value { get; set; }
		public string Po_link { get; set; }
		public bool privilege61 { get; set; }
		public bool quoter_locked_bool { get; set; }
		public int address_id { get; set; }
		public DataTable exist_quotes { get; set; }
		public bool uses_quote_process { get; set; }
		public bool service_report_hider { get; set; }
		public bool service_report_disabled { get; set; }
		public DataTable details { get; set; }
		public DataTable notes { get; set; }
		public DataTable followup_history { get; set; }
		public bool edit_disabled { get; set; }

		public string period_valid { get; set; }
		public LabelValueInt[] period_valid_value { get; set; }

        public string netsuite_estimate_internal_id { get; set; }

        public bool IsIntegratedWithNs => !string.IsNullOrWhiteSpace(netsuite_estimate_internal_id) && netsuite_estimate_internal_id !="NULL";

        public int opportunity_internal_id { get; set; }

        public bool parallel_bid { get; set; }

		public LabelValueInt[] bdm_options { get; set; }

		public LabelValueInt[] acting_bdm_options { get; set; }

		public int? bdm { get; set; }

        public int? acting_bdm{ get; set; }

        public bool isBdmReadOnly{ get; set; }
	}
}
