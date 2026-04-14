using System;
using System.Data;
using System.Collections.Generic;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for quote schedule table
	/// </summary>
	[Serializable()]
	public class NeQuoteSchedule
		{
		#region privates
		private int _id;
		private int _quoteid;
		private int _revision;
		private int _memberid;
		private int _pointperson;
		private int _estimator;
		private double _allowedquotetime;
		private DateTime _opendate;
		private int _openmid;
		private string _opennotes;
		private DateTime _stage1screening_date;
		private int _stage1screening_mid;
		private string _stage1screening_notes;
		private DateTime _schedule_produced_date;
		private int _schedule_produced_mid;
		private string _schedule_produced_notes;
		private DateTime _manpower_information_collected_date;
		private int _manpower_information_collected_mid;
		private string _manpower_information_collected_notes;
		private DateTime _customer_info_collected_date;
		private int _customer_info_collected_mid;
		private string _customer_info_collected_notes;
		private DateTime _market_info_collected_date;
		private int _market_info_collected_mid;
		private string _market_info_collected_notes;
		private DateTime _finance_info_collected_date;
		private int _finance_info_collected_mid;
		private string _finance_info_collected_notes;
		private DateTime _recon_report_created_date;
		private int _recon_report_created_mid;
		private string _recon_report_created_notes;
		private DateTime _stage4_gono_date;
		private string _stage4_gono_notes;
		private DateTime _quote_delivery_strategy_date;
		private int _quote_delivery_strategy_mid;
		private string _quote_delivery_strategy_notes;
	
		private DateTime _project_estimated_date;
		private int _project_estimated_mid;
		private string _project_estimated_notes;
		private DateTime _worksheet_review_date;
		private int _worksheet_review_mid;
		private string _worksheet_review_notes;
		private DateTime _stage6_final_review_date;
	
		private string _stage6_final_review_notes;
		private DateTime _quote_delivered_date;
		private int _quote_delivered_mid;
		private string _quote_delivered_notes;
		private DateTime _followup1_date;
		private int _followup1_mid;
		private string _followup1_notes;
		private DateTime _followup2_date;
		private int _followup2_mid;
		private string _followup2_notes;
		private DateTime _convert_or_kill_date;
		private int _convert_or_kill_mid;
		private string _convert_or_kill_notes;
		private DateTime _post_mortem_complete_date;
		private int _post_mortem_complete_mid;
		private string _post_mortem_complete_notes;
		private int _rt1=0;
		private int _rt2=0;
		private int _rt3=0;
		private int _rt4=0;
		private DateTime? _rt1_s4_approved = null;
		private DateTime? _rt2_s4_approved = null;
		private DateTime? _rt3_s4_approved = null;
		private DateTime? _rt4_s4_approved = null;
		private DateTime? _rt1_f_approved = null;
		private DateTime? _rt2_f_approved = null;
		private DateTime? _rt3_f_approved = null;
		private DateTime? _rt4_f_approved = null;
		private string _estimating_concerns = "";
		private string _sell_strategy = "";

		private DateTime? _schedule_produced_date_completed = null;
		private DateTime? _manpower_information_collected_date_completed = null;
		private DateTime? _customer_info_collected_date_completed = null;
		private DateTime? _market_info_collected_date_completed = null;
		private DateTime? _finance_info_collected_date_completed = null;
		private DateTime? _recon_report_created_date_completed = null;
		private DateTime? _stage4_gono_date_completed = null;
		private DateTime? _quote_delivery_strategy_date_completed = null;
		private DateTime? _project_estimated_date_completed = null;
		private DateTime? _worksheet_review_date_completed = null;
		private DateTime? _quote_delivered_date_completed = null;
		private DateTime? _stage6_final_date_completed = null;
		private DateTime? _convert_or_kill_date_completed = null;
		private DateTime? _post_mortem_complete_date_completed = null;
	


		#endregion

		#region publics
		public int id { get { return _id; } set { _id = value; } }
		public int quoteid { get { return _quoteid; } set { _quoteid = value; } }
		public int revision { get { return _revision; } set { _revision = value; } }
		public int memberid { get { return _memberid; } set { _memberid = value; } }
		public int pointperson { get { return _pointperson; } set { _pointperson = value; } }
		public int estimator { get { return _estimator; } set { _estimator = value; } }
		public double allowedquotetime { get { return _allowedquotetime; } set { _allowedquotetime = value; } }
		public DateTime opendate { get { return _opendate; } set { _opendate = value; } }
		public int openmid { get { return _openmid; } set { _openmid = value; } }
		public string opennotes { get { return _opennotes; } set { _opennotes = value; } }
		public DateTime stage1screening_date { get { return _stage1screening_date; } set { _stage1screening_date = value; } }
		public int stage1screening_mid { get { return _stage1screening_mid; } set { _stage1screening_mid = value; } }
		public string stage1screening_notes { get { return _stage1screening_notes; } set { _stage1screening_notes = value; } }
		public DateTime schedule_produced_date { get { return _schedule_produced_date; } set { _schedule_produced_date = value; } }
		public int schedule_produced_mid { get { return _schedule_produced_mid; } set { _schedule_produced_mid = value; } }
		public string schedule_produced_notes { get { return _schedule_produced_notes; } set { _schedule_produced_notes = value; } }
		public DateTime manpower_information_collected_date { get { return _manpower_information_collected_date; } set { _manpower_information_collected_date = value; } }
		public int manpower_information_collected_mid { get { return _manpower_information_collected_mid; } set { _manpower_information_collected_mid = value; } }
		public string manpower_information_collected_notes { get { return _manpower_information_collected_notes; } set { _manpower_information_collected_notes = value; } }
		public DateTime customer_info_collected_date { get { return _customer_info_collected_date; } set { _customer_info_collected_date = value; } }
		public int customer_info_collected_mid { get { return _customer_info_collected_mid; } set { _customer_info_collected_mid = value; } }
		public string customer_info_collected_notes { get { return _customer_info_collected_notes; } set { _customer_info_collected_notes = value; } }
		public DateTime market_info_collected_date { get { return _market_info_collected_date; } set { _market_info_collected_date = value; } }
		public int market_info_collected_mid { get { return _market_info_collected_mid; } set { _market_info_collected_mid = value; } }
		public string market_info_collected_notes { get { return _market_info_collected_notes; } set { _market_info_collected_notes = value; } }
		public DateTime finance_info_collected_date { get { return _finance_info_collected_date; } set { _finance_info_collected_date = value; } }
		public int finance_info_collected_mid { get { return _finance_info_collected_mid; } set { _finance_info_collected_mid = value; } }
		public string finance_info_collected_notes { get { return _finance_info_collected_notes; } set { _finance_info_collected_notes = value; } }
		public DateTime recon_report_created_date { get { return _recon_report_created_date; } set { _recon_report_created_date = value; } }
		public int recon_report_created_mid { get { return _recon_report_created_mid; } set { _recon_report_created_mid = value; } }
		public string recon_report_created_notes { get { return _recon_report_created_notes; } set { _recon_report_created_notes = value; } }
		public DateTime stage4_gono_date { get { return _stage4_gono_date; } set { _stage4_gono_date = value; } }
		public string stage4_gono_notes { get { return _stage4_gono_notes; } set { _stage4_gono_notes = value; } }
		public DateTime quote_delivery_strategy_date { get { return _quote_delivery_strategy_date; } set { _quote_delivery_strategy_date = value; } }
		public int quote_delivery_strategy_mid { get { return _quote_delivery_strategy_mid; } set { _quote_delivery_strategy_mid = value; } }
		public string quote_delivery_strategy_notes { get { return _quote_delivery_strategy_notes; } set { _quote_delivery_strategy_notes = value; } }
	
		public DateTime project_estimated_date { get { return _project_estimated_date; } set { _project_estimated_date = value; } }
		public int project_estimated_mid { get { return _project_estimated_mid; } set { _project_estimated_mid = value; } }
		public string project_estimated_notes { get { return _project_estimated_notes; } set { _project_estimated_notes = value; } }
		public DateTime worksheet_review_date { get { return _worksheet_review_date; } set { _worksheet_review_date = value; } }
		public int worksheet_review_mid { get { return _worksheet_review_mid; } set { _worksheet_review_mid = value; } }
		public string worksheet_review_notes { get { return _worksheet_review_notes; } set { _worksheet_review_notes = value; } }
		public DateTime stage6_final_review_date { get { return _stage6_final_review_date; } set { _stage6_final_review_date = value; } }

		public string stage6_final_review_notes { get { return _stage6_final_review_notes; } set { _stage6_final_review_notes = value; } }
		public DateTime quote_delivered_date { get { return _quote_delivered_date; } set { _quote_delivered_date = value; } }
		public int quote_delivered_mid { get { return _quote_delivered_mid; } set { _quote_delivered_mid = value; } }
		public string quote_delivered_notes { get { return _quote_delivered_notes; } set { _quote_delivered_notes = value; } }
		public DateTime followup1_date { get { return _followup1_date; } set { _followup1_date = value; } }
		public int followup1_mid { get { return _followup1_mid; } set { _followup1_mid = value; } }
		public string followup1_notes { get { return _followup1_notes; } set { _followup1_notes = value; } }
		public DateTime followup2_date { get { return _followup2_date; } set { _followup2_date = value; } }
		public int followup2_mid { get { return _followup2_mid; } set { _followup2_mid = value; } }
		public string followup2_notes { get { return _followup2_notes; } set { _followup2_notes = value; } }
		public DateTime convert_or_kill_date { get { return _convert_or_kill_date; } set { _convert_or_kill_date = value; } }
		public int convert_or_kill_mid { get { return _convert_or_kill_mid; } set { _convert_or_kill_mid = value; } }
		public string convert_or_kill_notes { get { return _convert_or_kill_notes; } set { _convert_or_kill_notes = value; } }
		public DateTime post_mortem_complete_date { get { return _post_mortem_complete_date; } set { _post_mortem_complete_date = value; } }
		public int post_mortem_complete_mid { get { return _post_mortem_complete_mid; } set { _post_mortem_complete_mid = value; } }
		public string post_mortem_complete_notes { get { return _post_mortem_complete_notes; } set { _post_mortem_complete_notes = value; } }

		public string estimating_concerns { get { return _estimating_concerns; } set { _estimating_concerns = value; } }
		public string sell_strategy { get { return _sell_strategy; } set { _sell_strategy = value; } }


		public int rt1 { get { return _rt1; } set { _rt1 = value; } }
		public int rt2 { get { return _rt2; } set { _rt2 = value; } }
		public int rt3 { get { return _rt3; } set { _rt3 = value; } }
		public int rt4 { get { return _rt4; } set { _rt4 = value; } }

		public DateTime? rt1_s4_approved { get { return _rt1_s4_approved; } set { _rt1_s4_approved = value; } }
		public DateTime? rt2_s4_approved { get { return _rt2_s4_approved; } set { _rt2_s4_approved = value; } }
		public DateTime? rt3_s4_approved { get { return _rt3_s4_approved; } set { _rt3_s4_approved = value; } }
		public DateTime? rt4_s4_approved { get { return _rt4_s4_approved; } set { _rt4_s4_approved = value; } }

		public DateTime? rt1_f_approved { get { return _rt1_f_approved; } set { _rt1_f_approved = value; } }
		public DateTime? rt2_f_approved { get { return _rt2_f_approved; } set { _rt2_f_approved = value; } }
		public DateTime? rt3_f_approved { get { return _rt3_f_approved; } set { _rt3_f_approved = value; } }
		public DateTime? rt4_f_approved { get { return _rt4_f_approved; } set { _rt4_f_approved = value; } }


		public DateTime? schedule_produced_date_completed { get { return _schedule_produced_date_completed; } set { _schedule_produced_date_completed = value; } }

		public DateTime? manpower_information_collected_date_completed { get { return _manpower_information_collected_date_completed  ; } set { _manpower_information_collected_date_completed = value; } }
		public DateTime? customer_info_collected_date_completed { get { return _customer_info_collected_date_completed   ; } set { _customer_info_collected_date_completed = value; } }
		public DateTime? market_info_collected_date_completed { get { return  _market_info_collected_date_completed  ; } set { _market_info_collected_date_completed = value; } }
		public DateTime? finance_info_collected_date_completed { get { return  _finance_info_collected_date_completed  ; } set { _finance_info_collected_date_completed = value; } }
		public DateTime? recon_report_created_date_completed { get { return  _recon_report_created_date_completed  ; } set { _recon_report_created_date_completed = value; } }
		public DateTime? stage4_gono_date_completed { get { return  _stage4_gono_date_completed  ; } set { _stage4_gono_date_completed = value; } }
		public DateTime? quote_delivery_strategy_date_completed { get { return _quote_delivery_strategy_date_completed   ; } set { _quote_delivery_strategy_date_completed = value; } }
		public DateTime? project_estimated_date_completed { get { return _project_estimated_date_completed   ; } set { _project_estimated_date_completed = value; } }
		public DateTime? stage6_final_date_completed { get { return _stage6_final_date_completed   ; } set { _stage6_final_date_completed = value; } }
		public DateTime? quote_delivered_date_completed { get { return _quote_delivered_date_completed; } set { _quote_delivered_date_completed = value; } }
		public DateTime? convert_or_kill_date_completed { get { return _convert_or_kill_date_completed   ; } set { _convert_or_kill_date_completed = value; } }
		public DateTime? post_mortem_complete_date_completed { get { return _post_mortem_complete_date_completed; } set { _post_mortem_complete_date_completed = value; } }
		public DateTime? worksheet_review_date_completed { get { return _worksheet_review_date_completed; } set { _worksheet_review_date_completed = value; } }
	
		#endregion

		public NeQuoteSchedule() { }
		public NeQuoteSchedule(int id)
			{
			init(_id);
			}

		public NeQuoteSchedule(string quoteid)
			{
			_id = Toolbox.doSQL_int("Select ifnull((Select id from quote_schedule where quoteid =@v0),0)", quoteid);
			if (_id != 0)
				{
				init(_id);
				}
			}

		public bool exists(int id)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(id) FROM quote_schedule WHERE id = @v0", id) > 0;
			}
		private void init(int __id)
			{
			if (exists(__id))
				{
				var dr	= Toolbox.doSQL_dt(@"SELECT * FROM quote_schedule WHERE id = @v0 ", new object[] {  __id } ).Rows[0];
				#region Int
				_id												= Convert.ToInt32(dr["id"]);
				_quoteid										= Convert.ToInt32(dr["quoteid"]);
				_revision										= Convert.ToInt32(dr["revision"]);
				_memberid										= Convert.ToInt32(dr["memberid"]);
				_pointperson									= Convert.ToInt32(dr["pointperson"]);
				_estimator										= Convert.ToInt32(dr["estimator"]);
				_openmid										= Convert.ToInt32(dr["openmid"]);
				_stage1screening_mid							= Convert.ToInt32(dr["stage1screening_mid"]);
				_schedule_produced_mid							= Convert.ToInt32(dr["schedule_produced_mid"]);
				_manpower_information_collected_mid				= Convert.ToInt32(dr["manpower_information_collected_mid"]);
				_customer_info_collected_mid					= Convert.ToInt32(dr["customer_info_collected_mid"]);
				_market_info_collected_mid						= Convert.ToInt32(dr["market_info_collected_mid"]);
				_finance_info_collected_mid						= Convert.ToInt32(dr["finance_info_collected_mid"]);
				_recon_report_created_mid						= Convert.ToInt32(dr["recon_report_created_mid"]);
				_quote_delivery_strategy_mid					= Convert.ToInt32(dr["quote_delivery_strategy_mid"]);
				_project_estimated_mid							= Convert.ToInt32(dr["project_estimated_mid"]);
				_worksheet_review_mid							= Convert.ToInt32(dr["worksheet_review_mid"]);
				_quote_delivered_mid							= Convert.ToInt32(dr["quote_delivered_mid"]);
				_followup1_mid									= Convert.ToInt32(dr["followup1_mid"]);
				_followup2_mid									= Convert.ToInt32(dr["followup2_mid"]);
				_convert_or_kill_mid							= Convert.ToInt32(dr["convert_or_kill_mid"]);
				_post_mortem_complete_mid						= Convert.ToInt32(dr["post_mortem_complete_mid"]);
				_rt1											= Convert.ToInt32(dr["rt1"]);
				_rt2											= Convert.ToInt32(dr["rt2"]);
				_rt3											= Convert.ToInt32(dr["rt3"]);
				_rt4											= Convert.ToInt32(dr["rt4"]);
				#endregion Int
				#region Double
				_allowedquotetime								= Convert.ToDouble(dr["allowedquotetime"]);
				#endregion Double
				#region Strings
				_opennotes										= Toolbox.ReturnBlankIfNull_string(dr["opennotes"]);
				_stage1screening_notes							= Toolbox.ReturnBlankIfNull_string(dr["stage1screening_notes"]);
				_schedule_produced_notes						= Toolbox.ReturnBlankIfNull_string(dr["schedule_produced_notes"]);
				_manpower_information_collected_notes			= Toolbox.ReturnBlankIfNull_string(dr["manpower_information_collected_notes"]);
				_customer_info_collected_notes					= Toolbox.ReturnBlankIfNull_string(dr["customer_info_collected_notes"]);
				_market_info_collected_notes					= Toolbox.ReturnBlankIfNull_string(dr["market_info_collected_notes"]);
				_finance_info_collected_notes					= Toolbox.ReturnBlankIfNull_string(dr["finance_info_collected_notes"]);
				_recon_report_created_notes						= Toolbox.ReturnBlankIfNull_string(dr["recon_report_created_notes"]);
				_stage4_gono_notes								= Toolbox.ReturnBlankIfNull_string(dr["stage4_gono_notes"]);
				_quote_delivery_strategy_notes					= Toolbox.ReturnBlankIfNull_string(dr["quote_delivery_strategy_notes"]);
				_project_estimated_notes						= Toolbox.ReturnBlankIfNull_string(dr["project_estimated_notes"]);
				_worksheet_review_notes							= Toolbox.ReturnBlankIfNull_string(dr["worksheet_review_notes"]);
				_stage6_final_review_notes						= Toolbox.ReturnBlankIfNull_string(dr["stage6_final_review_notes"]);
				_quote_delivered_notes							= Toolbox.ReturnBlankIfNull_string(dr["quote_delivered_notes"]);
				_followup1_notes								= Toolbox.ReturnBlankIfNull_string(dr["followup1_notes"]);
				_convert_or_kill_notes							= Toolbox.ReturnBlankIfNull_string(dr["convert_or_kill_notes"]);
				_post_mortem_complete_notes						= Toolbox.ReturnBlankIfNull_string(dr["post_mortem_complete_notes"]);
				_followup2_notes								= Toolbox.ReturnBlankIfNull_string(dr["followup2_notes"]);
				_estimating_concerns							= Toolbox.ReturnBlankIfNull_string(dr["estimating_concerns"]);
				_sell_strategy									= Toolbox.ReturnBlankIfNull_string(dr["sell_strategy"]);
				#endregion Strings
				#region DateTimes
				_opendate										= Toolbox.ReturnBlankDateTimeIfNull(dr["opendate"]);
				_stage1screening_date							= Toolbox.ReturnBlankDateTimeIfNull(dr["stage1screening_date"]);
				_schedule_produced_date							= Toolbox.ReturnBlankDateTimeIfNull(dr["schedule_produced_date"]);
				_manpower_information_collected_date			= Toolbox.ReturnBlankDateTimeIfNull(dr["manpower_information_collected_date"]);
				_customer_info_collected_date					= Toolbox.ReturnBlankDateTimeIfNull(dr["customer_info_collected_date"]);
				_market_info_collected_date						= Toolbox.ReturnBlankDateTimeIfNull(dr["market_info_collected_date"]);
				_finance_info_collected_date					= Toolbox.ReturnBlankDateTimeIfNull(dr["finance_info_collected_date"]);
				_recon_report_created_date						= Toolbox.ReturnBlankDateTimeIfNull(dr["recon_report_created_date"]);
				_stage4_gono_date								= Toolbox.ReturnBlankDateTimeIfNull(dr["stage4_gono_date"]);
				_quote_delivery_strategy_date					= Toolbox.ReturnBlankDateTimeIfNull(dr["quote_delivery_strategy_date"]);
				_project_estimated_date							= Toolbox.ReturnBlankDateTimeIfNull(dr["project_estimated_date"]);
				_worksheet_review_date							= Toolbox.ReturnBlankDateTimeIfNull(dr["worksheet_review_date"]);
				_stage6_final_review_date						= Toolbox.ReturnBlankDateTimeIfNull(dr["stage6_final_review_date"]);
				_quote_delivered_date							= Toolbox.ReturnBlankDateTimeIfNull(dr["quote_delivered_date"]);
				_followup1_date									= Toolbox.ReturnBlankDateTimeIfNull(dr["followup1_date"]);
				_followup2_date									= Toolbox.ReturnBlankDateTimeIfNull(dr["followup2_date"]);
				_convert_or_kill_date							= Toolbox.ReturnBlankDateTimeIfNull(dr["convert_or_kill_date"]);
				_post_mortem_complete_date						= Toolbox.ReturnBlankDateTimeIfNull(dr["post_mortem_complete_date"]);
				#endregion DateTimes
				#region Nullable DateTimes
				_rt1_s4_approved 								= Toolbox.ReturnNullDateTime(dr["rt1_s4_approved"]);
				_rt2_s4_approved 								= Toolbox.ReturnNullDateTime(dr["rt2_s4_approved"]);
				_rt3_s4_approved 								= Toolbox.ReturnNullDateTime(dr["rt3_s4_approved"]);
				_rt4_s4_approved 								= Toolbox.ReturnNullDateTime(dr["rt4_s4_approved"]);
				_rt1_f_approved 								= Toolbox.ReturnNullDateTime(dr["rt1_f_approved"]);
				_rt2_f_approved 								= Toolbox.ReturnNullDateTime(dr["rt2_f_approved"]);
				_rt3_f_approved 								= Toolbox.ReturnNullDateTime(dr["rt3_f_approved"]);
				_rt4_f_approved 								= Toolbox.ReturnNullDateTime(dr["rt4_f_approved"]);
				_schedule_produced_date_completed 				= Toolbox.ReturnNullDateTime(dr["schedule_produced_date_completed"]);
				_manpower_information_collected_date_completed 	= Toolbox.ReturnNullDateTime(dr["manpower_information_collected_date_completed"]);
				_customer_info_collected_date_completed 		= Toolbox.ReturnNullDateTime(dr["customer_info_collected_date_completed"]);
				_market_info_collected_date_completed 			= Toolbox.ReturnNullDateTime(dr["market_info_collected_date_completed"]);
				_finance_info_collected_date_completed 			= Toolbox.ReturnNullDateTime(dr["finance_info_collected_date_completed"]);
				_recon_report_created_date_completed 			= Toolbox.ReturnNullDateTime(dr["recon_report_created_date_completed"]);
				_stage4_gono_date_completed 					= Toolbox.ReturnNullDateTime(dr["stage4_gono_date_completed"]);
				_quote_delivery_strategy_date_completed 		= Toolbox.ReturnNullDateTime(dr["quote_delivery_strategy_date_completed"]);
				_project_estimated_date_completed 				= Toolbox.ReturnNullDateTime(dr["project_estimated_date_completed"]);
				_worksheet_review_date_completed 				= Toolbox.ReturnNullDateTime(dr["worksheet_review_date_completed"]);
				_stage6_final_date_completed 					= Toolbox.ReturnNullDateTime(dr["stage6_final_date_completed"]);
				_convert_or_kill_date_completed					= Toolbox.ReturnNullDateTime(dr["convert_or_kill_date_completed"]);
				_quote_delivered_date_completed					= Toolbox.ReturnNullDateTime(dr["quote_delivered_date_completed"]);
				_post_mortem_complete_date_completed 			= Toolbox.ReturnNullDateTime(dr["post_mortem_complete_date_completed"]);
				#endregion Nullable DateTimes
				}
			}

		private void manage_list(ref List<int> users, params int[] incoming_ids)
			{
			foreach(var i in incoming_ids)
				{
				if(!users.Contains(i))
					{
					users.Add(i);
					}
				}
			}
		public int[] member_list()
			{
			var users		= new List<int>();
			manage_list(ref users, _estimator, _rt1, _rt2, _rt3, _rt4, _convert_or_kill_mid, _customer_info_collected_mid, _finance_info_collected_mid, _followup1_mid, _followup2_mid, _manpower_information_collected_mid, _market_info_collected_mid, _post_mortem_complete_mid, _project_estimated_mid, _quote_delivered_mid, _quote_delivery_strategy_mid, _recon_report_created_mid, _schedule_produced_mid, _worksheet_review_mid);
			return users.ToArray();
			}

		private string date_handler(DateTime dt)
			{
			return !Toolbox.is_valid_date(dt) ? "NULL" : Toolbox.MySQL_longdt(dt);
			}
		private string date_handler(DateTime? dt)
			{
			return dt == null ? "NULL" : date_handler(Convert.ToDateTime(dt));
			}
		public void save()
			{
			if (_id != 0)
				{
				// Update
				Toolbox.doSQL_void(@"
UPDATE 
	quote_schedule 
SET 
	quoteid												= @v1,
	revision											= @v2,
	memberid											= @v3,
	pointperson											= @v4,
	estimator											= @v5,
	allowedquotetime									= @v6,
	opendate											= @v7,
	openmid												= @v8,
	opennotes											= @v9,
	stage1screening_date								= @v10,
	stage1screening_mid									= @v11,
	stage1screening_notes								= @v12,
	schedule_produced_date								= @v13,
	schedule_produced_mid								= @v14,
	schedule_produced_notes								= @v15,
	manpower_information_collected_date					= @v16,
	manpower_information_collected_mid					= @v17,
	manpower_information_collected_notes				= @v18,
	customer_info_collected_date						= @v19,
	customer_info_collected_mid							= @v20,
	customer_info_collected_notes						= @v21,
	market_info_collected_date							= @v22,
	market_info_collected_mid							= @v23,
	market_info_collected_notes							= @v24,
	finance_info_collected_date							= @v25,
	finance_info_collected_mid							= @v26,
	finance_info_collected_notes						= @v27,
	recon_report_created_date							= @v28,
	recon_report_created_mid							= @v29,
	recon_report_created_notes							= @v30,
	stage4_gono_date									= @v31,
	stage4_gono_notes									= @v32,
	quote_delivery_strategy_date						= @v33,
	quote_delivery_strategy_mid							= @v34,
	quote_delivery_strategy_notes						= @v35,
	project_estimated_date								= @v39,
	project_estimated_mid								= @v40,
	project_estimated_notes								= @v41,
	worksheet_review_date								= @v42,
	worksheet_review_mid								= @v43,
	worksheet_review_notes								= @v44,
	stage6_final_review_date							= @v45,
	stage6_final_review_notes							= @v47,
	quote_delivered_date								= @v48,
	quote_delivered_mid									= @v49,
	quote_delivered_notes								= @v50,
	followup1_date										= @v51,
	followup1_mid										= @v52,
	followup1_notes										= @v53,
	followup2_date										= @v54,
	followup2_mid										= @v55,
	followup2_notes										= @v56,
	convert_or_kill_date								= @v57,
	convert_or_kill_mid									= @v58,
	convert_or_kill_notes								= @v59,
	post_mortem_complete_date							= @v60,
	post_mortem_complete_mid							= @v61,
	post_mortem_complete_notes							= @v62,
	rt1													= @v63,
	rt2													= @v64,
	rt3													= @v65,
	rt4													= @v66,
	rt1_s4_approved										= @v67,
	rt2_s4_approved										= @v68,
	rt3_s4_approved										= @v69,
	rt4_s4_approved										= @v70,
	rt1_f_approved										= @v71,
	rt2_f_approved										= @v72,
	rt3_f_approved										= @v73,
	rt4_f_approved										= @v74,
	estimating_concerns									= @v75,
	sell_strategy										= @v76,
	schedule_produced_date_completed					= @v77,
	manpower_information_collected_date_completed		= @v78,
	customer_info_collected_date_completed				= @v79,
	market_info_collected_date_completed				= @v80,
	finance_info_collected_date_completed				= @v81,
	recon_report_created_date_completed					= @v82,
	stage4_gono_date_completed							= @v83,
	quote_delivery_strategy_date_completed				= @v84,
	project_estimated_date_completed					= @v85,
	stage6_final_date_completed							= @v86,
	convert_or_kill_date_completed						= @v87,
	post_mortem_complete_date_completed					= @v88,
	worksheet_review_date_completed						= @v89,
	quote_delivered_date_completed						= @v90
WHERE 
	id = @v0
LIMIT 1", new object[] {
					_id,															// {0}
					_quoteid,														// {1}
					_revision,														// {2}
					_memberid,														// {3}
					_pointperson,													// {4}
					_estimator,														// {5}
					_allowedquotetime,												// {6}
					date_handler(_opendate),										// {7}-
					_openmid,														// {8}
					_opennotes,									// {9}
					date_handler(_stage1screening_date),							// {10}-
					_stage1screening_mid,											// {11}
					_stage1screening_notes,						// {12}
					date_handler(_schedule_produced_date),							// {13}-
					_schedule_produced_mid,											// {14}
					_schedule_produced_notes,					// {15}
					date_handler(_manpower_information_collected_date),				// {16}-
					_manpower_information_collected_mid,							// {17}
					_manpower_information_collected_notes,		// {18}
					date_handler(_customer_info_collected_date),					// {19}-
					_customer_info_collected_mid,									// {20}
					_customer_info_collected_notes,				// {21}
					date_handler(_market_info_collected_date),						// {22}-
					_market_info_collected_mid,										// {23}
					_market_info_collected_notes,				// {24}
					date_handler(_finance_info_collected_date),						// {25}-
					_finance_info_collected_mid,									// {26}
					_finance_info_collected_notes,				// {27}
					date_handler(_recon_report_created_date),						// {28}-
					_recon_report_created_mid,										// {29}
					_recon_report_created_notes,				// {30}
					date_handler(_stage4_gono_date),								// {31}-
					_stage4_gono_notes,							// {32}
					date_handler(_quote_delivery_strategy_date),					// {33}-
					_quote_delivery_strategy_mid,									// {34}
					_quote_delivery_strategy_notes,				// {35}
					"",																// {36}
					0,																// {37}
					"",																// {38}
					date_handler(_project_estimated_date),							// {39}-
					_project_estimated_mid,											// {40}
					_project_estimated_notes,					// {41}
					date_handler(_worksheet_review_date),							// {42}-
					_worksheet_review_mid,											// {43}
					_worksheet_review_notes,					// {44}
					date_handler(_stage6_final_review_date),						// {45}-
					0,																// {46}
					_stage6_final_review_notes,					// {47}
					date_handler(_quote_delivered_date),							// {48}-
					_quote_delivered_mid,											// {49}
					_quote_delivered_notes,						// {50}
					date_handler(_followup1_date),									// {51}-
					_followup1_mid,													// {52}
					_followup1_notes,							// {53}
					date_handler(_followup2_date),									// {54}-
					_followup2_mid,													// {55}
					_followup2_notes,							// {56}
					date_handler(_convert_or_kill_date),							// {57}-
					_convert_or_kill_mid,											// {58}
					_convert_or_kill_notes,						// {59}
					date_handler(_post_mortem_complete_date),						// {60}-
					_post_mortem_complete_mid,										// {61}
					_post_mortem_complete_notes,				// {62}
					_rt1,															// {63}
					_rt2,															// {64}
					_rt3,															// {65}
					_rt4,															// {66}
					date_handler(_rt1_s4_approved),									// {67}-
					date_handler(_rt2_s4_approved),									// {68}-
					date_handler(_rt3_s4_approved),									// {69}-
					date_handler(_rt4_s4_approved),									// {70}-
					date_handler(_rt1_f_approved),									// {71}-
					date_handler(_rt2_f_approved),									// {72}-
					date_handler(_rt3_f_approved),									// {73}-
					date_handler(_rt4_f_approved),									// {74}-
					_estimating_concerns,						// {75}
					_sell_strategy,								// {76}
					date_handler(_schedule_produced_date_completed),				// {77}-
					date_handler(_manpower_information_collected_date_completed),	// {78}-
					date_handler(_customer_info_collected_date_completed),			// {79}-
					date_handler(_market_info_collected_date_completed),			// {80}-
					date_handler(_finance_info_collected_date_completed),			// {81}-
					date_handler(_recon_report_created_date_completed),				// {82}-
					date_handler(_stage4_gono_date_completed),						// {83}-
					date_handler(_quote_delivery_strategy_date_completed),			// {84}-
					date_handler(_project_estimated_date_completed),				// {85}-
					date_handler(_stage6_final_date_completed),						// {86}-
					date_handler(_convert_or_kill_date_completed),					// {87}-
					date_handler(_post_mortem_complete_date_completed),				// {88}-
					date_handler(_worksheet_review_date_completed),					// {89}-
					date_handler(_quote_delivered_date_completed)					// {90}-
				});
				}
			else
				{
				// Insert
				_id = Toolbox.doSQL_return_id(@"
INSERT INTO quote_schedule 
	(
	quoteid,	
	revision,
	memberid,
	pointperson,
	estimator,
	allowedquotetime,
	opendate,
	openmid,
	opennotes,
	stage1screening_date,
	stage1screening_mid,
	stage1screening_notes,
	schedule_produced_date,
	schedule_produced_mid,
	schedule_produced_notes,
	manpower_information_collected_date,
	manpower_information_collected_mid,
	manpower_information_collected_notes,
	customer_info_collected_date,
	customer_info_collected_mid,
	customer_info_collected_notes,
	market_info_collected_date,
	market_info_collected_mid,
	market_info_collected_notes,
	finance_info_collected_date,
	finance_info_collected_mid,
	finance_info_collected_notes,
	recon_report_created_date,
	recon_report_created_mid,
	recon_report_created_notes,
	stage4_gono_date,
	stage4_gono_notes,
	quote_delivery_strategy_date,
	quote_delivery_strategy_mid,
	quote_delivery_strategy_notes,
	project_estimated_date,
	project_estimated_mid,
	project_estimated_notes,
	worksheet_review_date,
	worksheet_review_mid,
	worksheet_review_notes,
	stage6_final_review_date,
	stage6_final_review_notes,
	quote_delivered_date,
	quote_delivered_mid,
	quote_delivered_notes,
	followup1_date,
	followup1_mid,
	followup1_notes,
	followup2_date,
	followup2_mid,
	followup2_notes,
	convert_or_kill_date,
	convert_or_kill_mid,
	convert_or_kill_notes,
	post_mortem_complete_date,
	post_mortem_complete_mid,
	post_mortem_complete_notes,
	rt1,
	rt2,
	rt3,
	rt4,
	estimating_concerns,
	sell_strategy
	) 
VALUES
	(
	@v1,
	@v2,
	@v3,
	@v4,
	@v5,
	@v6,
	@v7,
	@v8,
	@v9,
	@v10,
	@v11,
	@v12,
	@v13,
	@v14,
	@v15,
	@v16,
	@v17,
	@v18,
	@v19,
	@v20,
	@v21,
	@v22,
	@v23,
	@v24,
	@v25,
	@v26,
	@v27,
	@v28,
	@v29,
	@v30,
	@v31,
	@v32,
	@v33,
	@v34,
	@v35,
	@v39,
	@v40,
	@v41,
	@v42,
	@v43,
	@v44,
	@v45,
	@v47,
	@v48,
	@v49,
	@v50,
	@v51,
	@v52,
	@v53,
	@v54,
	@v55,
	@v56,
	@v57,
	@v58,
	@v59,
	@v60,
	@v61,
	@v62,
	@v63,
	@v64,
	@v65,
	@v66,
	@v67,
	@v68
	)", new object[] {
					_id,					 // {0}
					_quoteid,
					_revision,
					_memberid,
					_pointperson,
					_estimator,// {5}
					_allowedquotetime,
					Toolbox.MySQL_longdt(_opendate),
					_openmid,
					_opennotes,
					Toolbox.MySQL_longdt(_stage1screening_date),// {10}
					_stage1screening_mid,
					_stage1screening_notes,
					Toolbox.MySQL_longdt(_schedule_produced_date),
					_schedule_produced_mid,
					_schedule_produced_notes,// {15}
					Toolbox.MySQL_longdt(_manpower_information_collected_date),
					_manpower_information_collected_mid,
					_manpower_information_collected_notes,
					Toolbox.MySQL_longdt(_customer_info_collected_date),
					_customer_info_collected_mid,// {20}
					_customer_info_collected_notes,
					Toolbox.MySQL_longdt(_market_info_collected_date),
					_market_info_collected_mid,
					_market_info_collected_notes,
					Toolbox.MySQL_longdt(_finance_info_collected_date),// {25}
					_finance_info_collected_mid,
					_finance_info_collected_notes,
					Toolbox.MySQL_longdt(_recon_report_created_date),
					_recon_report_created_mid,
					_recon_report_created_notes,// {30}
					Toolbox.MySQL_longdt(_stage4_gono_date),
					_stage4_gono_notes,
					Toolbox.MySQL_longdt(_quote_delivery_strategy_date),
					_quote_delivery_strategy_mid,
					_quote_delivery_strategy_notes,// {35}
					"",
					0,
					"",
					Toolbox.MySQL_longdt(_project_estimated_date),
					_project_estimated_mid,// {40}
					_project_estimated_notes,
					Toolbox.MySQL_longdt(_worksheet_review_date),
					_worksheet_review_mid,
					_worksheet_review_notes,
					Toolbox.MySQL_longdt(_stage6_final_review_date),// {45}
					0,
					_stage6_final_review_notes,
					Toolbox.MySQL_longdt(_quote_delivered_date),
					_quote_delivered_mid,
					_quote_delivered_notes,// {50}
					Toolbox.MySQL_longdt(_followup1_date),
					_followup1_mid,
					_followup1_notes,
					Toolbox.MySQL_longdt(_followup2_date),
					_followup2_mid,// {55}
					_followup2_notes,
					Toolbox.MySQL_longdt(_convert_or_kill_date),
					_convert_or_kill_mid,
					_convert_or_kill_notes,
					Toolbox.MySQL_longdt(_post_mortem_complete_date),// {60}
					_post_mortem_complete_mid,
					_post_mortem_complete_notes,
					_rt1,
					_rt2,
					_rt3,
					_rt4,
					_estimating_concerns,
					_sell_strategy
				});

				}
			}

		public static void delete(int _id)
			{

			}

		}
	}