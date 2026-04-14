using System;
using System.Data;
using System.Collections;
using DevExpress.Xpo;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeMemberType
	/// </summary>
	public class NeMemberType
		{
		public bool show_on_ratesheet { get; set; }
		public string benefitplan { get; set; }
		public int org_chart_level { get; set; }
		public int exec_severance_package { get; set; }
		public int benefits_start_default { get; set; }
		public int reports_to { get; set; }
		public int id { get; set; }
		public string name { get; set; }
		public string objective { get; set; }
		public bool active	{ get; set; }
		public bool IsChargeoutOnly { get; set; }
		public bool is_elevated	{ get; set; }
		public bool considered_pm { get; set; }
		public int MemberTypeID
			{
			get { return id; }
			set { id = value; }
			}
		public string MemberTypeName
			{
			get { return name; }
			set { name = value; }
			}
		public bool is_scheduled { get; set; }
		public bool is_oncall { get; set; }
		public bool is_team_leader { get; set; }
		public bool considered_field_staff { get; set; }
		//	public bool is_hiring_manager
		//	{
		//		get { return _is_hiring_manager; }
		//		set { _is_hiring_manager = value; }
		//	}

		public NeMemberType()
			{
			//
			//  
			//
			}
		public NeMemberType(object _id)
			{
			using(var uow = new UnitOfWork())
				{
				var mt		= uow.GetObjectByKey<ne_xpo.cs.membertype>(Convert.ToInt32(_id));
				if(mt != null)
					{
					id							= mt.membertype_id;
					name						= mt.membertype_name;
					active						= mt.active;
					is_elevated					= mt.is_elevated;
					reports_to					= mt.reports_to;
					objective					= mt.objective;
					benefitplan				= mt.benefitplan;
					is_scheduled				= mt.is_scheduled;
					is_team_leader				= mt.is_team_leader;
					show_on_ratesheet			= mt.show_on_ratesheet;
					exec_severance_package		= mt.exec_severance_package;
                    benefits_start_default = mt.benefits_start_default;
					is_oncall					= mt.is_oncall;
					org_chart_level			= mt.org_chart_level;
					considered_pm = mt.considered_pm;
                    considered_field_staff = mt.considered_field_staff;
					IsChargeoutOnly = mt.is_chargeout;
                    }
				}
			}

		public void save()
			{
			using (var uow = new UnitOfWork())
				{
				var mt = id==0?new ne_xpo.cs.membertype(uow): uow.GetObjectByKey<ne_xpo.cs.membertype>(Convert.ToInt32(id));

			
				mt.ratesheet_order = id == 0 ? 99 : mt.ratesheet_order;
				mt.membertype_name=name;
				mt.active=active ;
				mt.is_elevated=is_elevated ;
				mt.reports_to=reports_to;
				mt.objective=objective;
				mt.benefitplan=benefitplan;
				mt.is_scheduled=is_scheduled;
				mt.is_team_leader=is_team_leader;
				mt.show_on_ratesheet=show_on_ratesheet;
				mt.exec_severance_package=exec_severance_package;
				mt.is_oncall=is_oncall;
				mt.org_chart_level=org_chart_level ;
				mt.considered_pm = considered_pm;
                mt.benefits_start_default = benefits_start_default;
                mt.considered_field_staff = considered_field_staff;
				mt.is_chargeout = IsChargeoutOnly;
                mt.Save();
				uow.CommitChanges();
				if (id==0)
					{
					id = mt.membertype_id;
					}
				}


			/*	Toolbox tools = new Toolbox();
		if (MemberTypeID != 0)
		{
			Toolbox.doSQL_void(@"Update membertype  set membertype_name =@v0,active=@v1 ,is_elevated =@v2 , reports_to=@v3 ,show_on_ratesheet=@v4 ,objective=@v5 ,benefitplan=@v6 , mt_id_us = (100000 ,is_team_leader=@v7 ,exec_severance_package=@v8 ,is_oncall=@v9 ,org_chart_level =@v10 ,ticket_vote_weight=@v11   where membertype_id = @v12", new object[] { name,active,is_elevated,reports_to,show_on_ratesheet,objective,
			_benefitplan,membertype_id), is_scheduled=,Convert.ToInt16(_is_scheduled),Convert.ToInt16(_is_team_leader),Convert.ToInt16(_exec_severance_package),Convert.ToInt16(_is_oncall),_org_chart_level,_ticket_vote_weight,MemberTypeID });
		}
		else
		{
			MemberTypeID = (int)Toolbox.doSQL_return_id(@"Insert into membertype (membertype_name,active,is_elevated,reports_to,show_on_ratesheet,objective,benefitplan,is_scheduled,is_team_leader,exec_severance_package,is_oncall,org_chart_level,ticket_vote_weight)  Values(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12)",new object[] { name,active,is_elevated,reports_to,show_on_ratesheet,objective,
			_benefitplan,Convert.ToInt16(_is_scheduled),Convert.ToInt16(_is_team_leader),Convert.ToInt16(_exec_severance_package),Convert.ToInt16(_is_oncall),_org_chart_level,ticket_vote_weight } );
			
			if(show_on_ratesheet)
				{
					Toolbox.doSQL_void(@"UPDATE membertype SET ratesheet_order = ((SELECT MAX(ratesheet_order) FROM membertype WHERE show_on_ratesheet = TRUE AND active = TRUE) + 1), mt_id_us = (100000 + membertype_id) WHERE membertype_id = @v0  LIMIT 1", new object[] {  MemberTypeID } );
				}
		}
	 */

	
			}

		public ArrayList LoadMemberList()
			{
			var dt			= Toolbox.doSQL_dt(@"SELECT * FROM membertype  WHERE active = TRUE" ,null);
			var list			= new ArrayList();
			foreach(DataRow dr in dt.Rows)
				{
				var this_id		= Convert.ToInt32(dr["membertype_id"]);
				var this_name	= dr["membertype_name"].ToString();
				list.Add(new NeMemberType{name = this_name, id = this_id});
				}
			return list;
			}

		public DataTable Get_Supervisors_by_mt_design (int membertype, int _business_unit_id)
			{
			var dt = new DataTable();
			dt.Columns.Add("memberid");
			dt.Columns.Add("member_fullname");
			dt.Columns.Add("membertype_id");
			dt.Columns.Add("business_unit_id");

			for (var i = 0; i < 20; i++)
				{
				var dt1 = Get_List_of_immediate_possible_supervisors(membertype, _business_unit_id);
				if (dt1.Rows.Count > 0)
					{
					foreach (DataRow dr1 in dt1.Rows)
						{
						dt.Rows.Add(dr1);

						}
					membertype = Convert.ToInt32(dt1.Rows[0]["membertype_id"]);
					}
				}
			return dt;
			}


		public DataTable Get_List_of_immediate_possible_supervisors(int membertypeid, int _business_unit_id)
			{
			return Toolbox.doSQL_dt(@"SELECT member.member_id memberid, member.member_fullname member_fullname, member.member_membertype_id membertype_id, member.business_unit_id FROM membertype INNER JOIN member ON membertype.reports_to = member.Member_MemberType_ID INNER JOIN business_unit ON member.business_unit_id = business_unit.id  WHERE membertype.membertype_id = @v0 AND member.business_unit_id = @v1 AND member.Member_Status = 'Active'", new object[] { membertypeid,_business_unit_id });

			}

			public static bool is_supervisor_mt(int membertypeid, int current_memberType_id)
			{
				return Toolbox.doSQL_bool("SELECT IS_SUPERVISOR_MT(@v0,@v1)",new object[] {membertypeid, current_memberType_id });
			}

		public ArrayList LoadRestrictedMemberList()
			{
			var dt			= Toolbox.doSQL_dt(@"SELECT * FROM membertype  WHERE active = TRUE AND is_elevated = FALSE" ,null);
			var list			= new ArrayList();
			foreach(DataRow dr in dt.Rows)
				{
				var this_id		= Convert.ToInt32(dr["membertype_id"]);
				var this_name	= dr["membertype_name"].ToString();
				list.Add(new NeMemberType{name = this_name, id = this_id});
				}
			return list;
			}

		public DataTable LoadApprenticeType()
			{
			//var strSQLSelect = "SELECT member_apprentice_level_id AS levelid, member_apprentice_level_type AS leveltype FROM  member_apprentice_level";
			var apprenticelevel = Toolbox.doSQL_dt(@"SELECT member_apprentice_level_id AS levelid, member_apprentice_level_type AS leveltype FROM member_apprentice_level"  , null);
			return apprenticelevel;

			}
		public double GetChargeout(string chargeoutid)
			{
			var ChargeOut = 0.0;
			try
				{
				ChargeOut = Toolbox.doSQL_double(@"SELECT Chargeout FROM membertype_chargeout WHERE id = @v0 ", new object[] {  chargeoutid } );
				}
			catch
				{
				ChargeOut = 0;
				}
			return ChargeOut;
			}
		public double GetChargeoutforbranch(object _business_unit_id)
			{
			return Toolbox.doSQL_double(@"SELECT IFNULL(MAX(chargeout),0) FROM membertype_chargeout WHERE membertype_id = @v0  and business_unit_id = @v1  AND paytype_id = 1", new object[] {  id, _business_unit_id } );
			}


        public static void save_chargeout_rate(int mt_id, int member_id, double rate, int business_unit_id)
        {
            using (var conn = Toolbox.connect())
            {
            var paytypes = Toolbox.doSQL_dt(conn, "SELECT paytypehours_id, multiplier FROM paytypehours", null); // Create datatable for paytypes
            var dt = Toolbox.doSQL_dt(@"Select id business_unit_id from business_unit  where active = 'T' and find_in_set(id,get_visible_business_units_group_concat(@v0)) ", new object[] { member_id });
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        var cid = Convert.ToInt32(dr["business_unit_id"]);
                        foreach (DataRow drPaytype in paytypes.Rows) // Iterate through pay types
                        {
                            var pid = Convert.ToInt32(drPaytype["paytypehours_id"]); // Use the paytype id
                            var mult = Convert.ToDouble(drPaytype["multiplier"]); // Use the paytype multiplier
                            var x = Toolbox.doSQL_int(conn, @"Select ifnull((select count(membertype_chargeout.id) from membertype_chargeout  where membertype_chargeout.membertype_id=@v0 and  membertype_chargeout.business_unit_id =@v1  and membertype_chargeout.paytype_id=@v2 ),0) ", new object[] { mt_id, cid, pid });
                            if (x == 0)
                            {
                                Toolbox.doSQL_void(conn, @"Insert into membertype_chargeout (membertype_chargeout.membertype_id,membertype_chargeout.business_unit_id,membertype_chargeout.paytype_id,membertype_chargeout.chargeout)  values (@v0,@v1,@v2,@v3)", new object[] { mt_id, cid, pid, (rate * mult) });
                            }
                            else
                            {
                                Toolbox.doSQL_void(conn, @"Update membertype_chargeout  set membertype_chargeout.chargeout =@v0  where membertype_chargeout.membertype_id =@v1 and membertype_chargeout.business_unit_id=@v2  and membertype_chargeout.paytype_id =@v3 and membertype_chargeout.chargeout<=0 ", new object[] { (rate * mult), mt_id, cid, pid });
                                if (cid == business_unit_id)
                                {
                                    Toolbox.doSQL_void(conn, @"Update membertype_chargeout  set membertype_chargeout.chargeout =@v0  where membertype_chargeout.membertype_id =@v1 and membertype_chargeout.business_unit_id=@v2  and membertype_chargeout.paytype_id =@v3 limit 1 ", new object[] { (rate * mult), mt_id, business_unit_id, pid });
                                }
                            }
                        }

                    }
                }
            }
        }

		public DataTable GetBaseRates(string _business_unit_id, DateTime date)
			{
			try
				{
				return Toolbox.doSQL_dt(@" SELECT a.membertype_chargeout_history_rate, a.membertype_chargeout_history_membertype_id, c.membertype_name FROM membertype_chargeout_history a LEFT JOIN membertype_chargeout_history b ON a.membertype_chargeout_history_membertype_id = b.membertype_chargeout_history_membertype_id AND a.membertype_chargeout_history_id < b.membertype_chargeout_history_id AND a.business_unit_id = b.business_unit_id LEFT JOIN membertype c ON a.membertype_chargeout_history_membertype_id = c.membertype_id WHERE a.membertype_chargeout_history_date < @v1  AND a.business_unit_id = @v0  AND b.membertype_chargeout_history_id IS NULL ORDER BY a.membertype_chargeout_history_membertype_id", new object[] {  _business_unit_id, date.ToString("yyyy-MM-dd") } );
				}
			catch (Exception ee)
				{ 
				Toolbox.do_errorLog(ee, "Trying to get base rates");
				return new DataTable(); 
				}
			}
		public double GetAvgBranchCost(int membertypeid, int business_unit_id)
			{
			return Toolbox.doSQL_double(@" SELECT ROUND(IFNULL(AVG(currentwage), 0),2) FROM ( SELECT b.currentwage FROM member a LEFT JOIN memberwage b ON b.memberwage_memberid = a.Member_ID AND b.currentwagetype = 'Hourly' AND b.active = 'true' WHERE a.member_status = 'Active' AND a.business_unit_id = @v0  AND a.member_membertype_id = @v1  GROUP BY a.member_membertype_id) c ", new object[] {  business_unit_id, membertypeid } );
			}

	

		public void mass_history_insert()
			{
			var dt = Toolbox.doSQL_dt(@"SELECT membertype_chargeout.business_unit_id, membertype_chargeout.Chargeout, membertype_chargeout.membertype_id FROM membertype_chargeout  WHERE membertype_chargeout.paytype_id = 1" ,null);
			foreach (DataRow dr in dt.Rows)
				{
				Toolbox.doSQL_void(@"Insert into membertype_chargeout_history 
(membertype_chargeout_history_memberid,membertype_chargeout_history_rate,membertype_chargeout_history_date,membertype_chargeout_history_membertype_id,business_unit_id)
values (8,@v0,curdate(),@v1,@v2)",
new object[] {
 dr["Chargeout"],dr["membertype_id"] , dr["business_unit_id"] });


				}

			}
		public static string get_membertypename(int mt_id)
		{
			return Toolbox.doSQL_string(@"SELECT membertype_name FROM membertype WHERE membertype_id = @v0;", new object[] { mt_id });
		}

		public static void clear_all_auto_reports(int mt_id)
			{
			Toolbox.doSQL_void(@"Delete from membertype_auto_reports where membertype_id =@v0 ", new object[] { mt_id});
			}

		public static void add_to_auto_reports(int mt_id, int auto_report_id)
			{
			Toolbox.doSQL_void(@"Insert into membertype_auto_reports (membertype_id, autoreports_id) values (@v0,@v1)", new object[] { mt_id,auto_report_id});
			}
		public static DataTable get_autoreports_list_for_membertype(int mt_id)
			{
			return Toolbox.doSQL_dt(@"Select autoreports_id from membertype_auto_reports  where membertype_id = @v0", new object[] { mt_id });
			}
	
		}
	}