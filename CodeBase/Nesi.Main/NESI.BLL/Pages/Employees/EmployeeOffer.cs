using nesi.core;
using NESI.BLL.Common.Cache;
using NESI.BLL.Core.Employee;
using NESI.BLL.Core.FileManager;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Employees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NESI.BLL.Pages.Employees
{
    public class EmployeeOffer : EmployeeEdit
	{
		public int offer_id { get; set; }
		public int applicant_id { get; set; }
		public bool is_applicant { get; set; }
		public bool can_see_all_offers { get; set; }
		public bool can_edit_offers { get; set; }
		public string status { get; set; }
		public bool in_development => status == "In development";
		public bool is_supervisor { get; set; }
		public bool is_elevated { get; set; }
		protected readonly NeMember current_user;
		protected NeApplicant applicant;
	    public int has_compl { get; set; }
	    public int report_to { get; set; }
	    public int show_accept_with_compl_plan { get; set; }

		public bool BUMatches { get; set; }

	    public EmployeeOffer(Employee currentuser) : base(currentuser)
		{

		}

		public EmployeeOffer(Employee currentuser, bool isapplicant, int mid, int applicantid, int offerid) : base(currentuser, mid)
		{
			is_applicant = isapplicant;
			applicant_id = applicantid;
			if (is_applicant)
			{
				applicant = applicant_id > 0 ? new NeApplicant(applicant_id) : new NeApplicant();
			}
			offer_id = offerid;
			can_see_all_offers = currentuser.AuthenticatedForPrivilege(152);
			can_edit_offers = currentuser.AuthenticatedForPrivilege(129);
			var m = offerid > 0 ? new NeMemberOffer(offerid) : new NeMemberOffer();

		    var createdBy = new NeMember(m.enteredby); // Martin
		    var supervisorOfCreatedBy = NeMember.is_supervisor(createdBy.id, currentuser.Id);
		    if (m.has_comp == 1)
		    {
		        if (supervisorOfCreatedBy)
		        {
		            show_accept_with_compl_plan = 1;
		        }
		        else
		        {
		            show_accept_with_compl_plan = 0;
		        }
		    }		
		    has_compl = m.has_comp;
		    report_to = m.reports_to;
            status = m.status;
			current_user = new NeMember(UserId);
			var mt = m.membertypeid > 0 ? new NeMemberType(m.membertypeid) : new NeMemberType();
			is_supervisor = current_user.AuthenticatedForPrivilege(130) ||
							(NeMemberType.is_supervisor_mt(m.membertypeid, CurrentUser.MemberType.membertype_id) && m.business_unit_id == CurrentUser.BusinessUnitId) ||
							(NeMember.is_supervisor(m.enteredby, current_user.id) && NeMember.is_supervisor(m.reports_to, current_user.id));
			is_elevated = CurrentUser.MemberType.is_elevated.GetValueOrDefault();
			if (is_elevated && offerid > 0)
			{
				var group_id_current_user =
					(from gid in _db.tax_entity_group_link where gid.tax_entity_id == CurrentUser.TaxEntityId select gid.tax_entity_group_id).FirstOrDefault();
				var te2_id = BLL.Common.Cache.Global.BusinessUnit.GetValue(m.business_unit_id).tax_entity_id;
				var group_id_offer = (from gid in _db.tax_entity_group_link
									  where gid.tax_entity_id == te2_id
									  select gid.tax_entity_group_id).FirstOrDefault();
				is_elevated = (group_id_offer > 0 && group_id_offer > 0 && group_id_offer == group_id_current_user);
			}
			if (!is_applicant && mid != 0)
			{
                #region Will this offer change the branch of an employee
                var EmployeeBeingOfferred = new NeMember(mid);
				BUMatches = EmployeeBeingOfferred.business_unit_id ==  m.business_unit_id;
                #endregion Will this offer change the branch of an employee

                #region Is this offer accessible by the Current User
                if (n1_member.business_unit_id != CurrentUser.BusinessUnitId && !isowner &&
					!NeMember.is_supervisor(member_id, UserId) && UserId != member_id && !can_see_all_offers)
				{
					can_access = false;
					error_message = "You can only access users from your branch";
				}
				if (offerid > 0)
				{
					if (UserId != new NeBusinessUnit(m.business_unit_id).branch_manager.id && m.enteredby != UserId
						&& !can_see_all_offers && !can_view_employment_agreement_tab && m.memberid != UserId && m.reports_to != UserId && n1_member.reports_to != UserId &&
						!isowner)
					{
						can_access = false;
						error_message = "Sorry, you do not have the credentials to view this page";
					}
					else
					{
						if (!can_see_all_offers && m.memberid == UserId && current_user.reports_to != 0)
						{
							can_access = false;
							error_message = "Sorry, you do not have the credentials to view this page";
						}

					}
				}
				if (!can_edit_offers)
				{
					can_access = false;
					error_message = "Sorry, you do not have the credentials to view this page";
				}
                #endregion Is this offer accessible by the Current User
            }


        }

		public DataExtra SaveSignBack(string file)
		{
			var f = new EmployeeSignBackFileStore(CurrentUser, offer_id);
			f.Save(file);
			return new DataExtra("File has been uploaded successfully.", GetSignBackProfile());
		}

		public DataExtra DeleteSignBack(int id)
		{
			bllToolbox.doSQL_void(@"Delete from filestore.files where id = @v0 limit 1", id);
			return new DataExtra("File has been deleted successfully.", GetSignBackProfile());
		}

		public object GetSignBackProfile()
		{
			var fileList = bllToolbox.doSQL_Array<LabelValueInt>(@"Select 
filestore.files.id value, 
CONCAT(filestore.files.name,'.',filestore.files.ext,' from ',DATE_FORMAT(filestore.files.dt,'%Y-%m-%d')) label 
from filestore.files  where filestore.files.folder_id = 3 and filestore.files.sub_folder_id =@v0", offer_id);

			return new
			{
				can_access,
				error_message,
				is_backoffice,
				in_development,
				fileList
			};
		}




		public object GetExtraProfile()
		{
			var mo = new NeMemberOffer(offer_id);
			var o = new EmployeeOfferExtra();
			o = (EmployeeOfferExtra)MapperFrom(o, mo);
			return new
			{
				can_access,
				error_message,
				is_backoffice,
				in_development,
				entity = o
			};
		}

		public DataExtra SaveExtras(EmployeeOfferExtra model)
		{
			var mo = new NeMemberOffer(offer_id);
			mo = (NeMemberOffer)MapperFrom(mo, model);
			mo.save();
			return new DataExtra("Extras has been saved successfully.", GetExtraProfile());
		}

		private string GetHistoryNotes()
		{
			return !can_access ? string.Empty : bllToolbox.doSQL_string(@"select member_offers.redo_notes from member_offers  where id =@v0", offer_id);
		}

        /// <summary>
        /// Set up the info that will be returned back to client side (angular). 
        /// </summary>
        /// <param name="withConfirmOverwriteInfo">1: Set flag of needing overwrite existing accepted offer; 0: no set this flag.</param>
        /// <returns></returns>
        public object GetNotes(int withConfirmOverwriteInfo)
		{
            var hnotes = offer_id == 0 ? "" : GetHistoryNotes();
			return new
			{
				is_supervisor,
				is_elevated,
				can_access,
				error_message,
				is_backoffice,
				in_development,
				status,
				entity = new EmployeeOfferNotes()
				{
					member_id = this.member_id,
					offer_id = this.offer_id,
					new_notes = "",
					history_notes = !can_access ? string.Empty : hnotes,
				},

			    // get ismgr - get_bm == current user id
			    // has comple from offer
			    // offer.report to == "current user id"
                has_compl,
                report_to,
                current_user_id = current_user.id,
                manager_id = Toolbox.doSQL_int(@"select get_bm(@v0 )", new object[] { current_user.id }),
			    multipleAcceptedOffers = withConfirmOverwriteInfo,
                show_accept_with_compl_plan
            };
		}

		public DataExtra SaveNotes(EmployeeOfferNotes model)
		{
			if (!can_access) return new DataExtra();
			var hnotes = offer_id == 0 ? "" : GetHistoryNotes();
			var newnotes = System.DateTime.Today.ToString("yyyy-MM-dd") + " - " + CurrentUser.FullName + System.Environment.NewLine
						   + model.new_notes + System.Environment.NewLine + hnotes;
			bllToolbox.doSQL_void(@"update member_offers 
				set redo_notes = @v0 
				where id = @v1 ", newnotes, offer_id);

			return new DataExtra()
			{
				Data = "Notes has been added successfully.",
				Extra = GetNotes(0)
			};
		}

		public object GetMileStoneProfile()
		{
			var ms = bllToolbox.doSQL_dt(@"Select * from memberoffer_milestones  where memberoffer_milestones.offerid = @v0", offer_id);
			var entity = new EmployeeMileStone { due = DateTime.Today };
			return
				new
				{
					can_access,
					error_message,
					entity,
					mileStoneList = ms
				};
		}

		public DataExtra SaveMileStone(DTO.ViewModels.Page.Employees.EmployeeMileStone model)
		{
			var mm = model.id > 0 ? new Memberoffer_Milestones(model.id) : new Memberoffer_Milestones();
			mm.offerid = offer_id;
			mm.addedby = UserId;
			mm.added = DateTime.Today;
			mm.due = model.due;
			mm.milestone = model.milestone;
			mm.save();
			return new DataExtra("Milestone has been saved successfully.", GetMileStoneProfile());
		}

		public DataExtra DeleteMileStone(int id)
		{
			bllToolbox.doSQL_void(@"Delete from memberoffer_milestones where id =@v0 limit 1", id);
			return new DataExtra("Milestone has been deleted successfully.", GetMileStoneProfile());
		}

		public DataExtra SaveDetail(EmployeeOfferDetail model)
		{
			var mo = model.id == 0 ? new NeMemberOffer() : new NeMemberOffer(model.id);
			var old_moid = GetOldMoId();
			if (model.id == 0 && old_moid > 0)
			{
				if (old_moid > 0)
				{ 
					var old_mo = new NeMemberOffer(old_moid);
					mo = (NeMemberOffer)MapperFrom(mo, old_mo);
					mo.id = model.id;// resetting after applying old member offer template
					mo.notes = "";
					mo.previous_membertype = old_mo.membertypeid;
					mo.previous_wage = old_mo.wage;
					//	mo.benefits_startdate = old_mo.benefits_startdate;
					mo.date = DateTime.Today;
                   
                    mo.enteredby = UserId;
					mo.status = "In Development";
					mo.is_signed = false;
				}
			}
            var business_unit = new NeBusinessUnit(model.business_unit_id);
            var is_CDN = business_unit.country == "CDN";
            var Tax_Entity = new NeTaxEntity(business_unit.tax_entity_id);
            if (model.id == 0 && old_moid == 0)
			{
				mo.date = DateTime.Today;
				mo.enteredby = UserId;
				mo.status = "In Development";
				mo.is_signed = false;     
                mo.comp_details = "";
				mo.has_comp = 0;
				mo.notes = "";
			}

            if (!string.IsNullOrEmpty(Tax_Entity.region))
            {
                mo.vacation_interval_1 = (int)Tax_Entity.vac_freq_1;
                mo.vacation_amount_1 = (double)Tax_Entity.vac_amount_1;
                mo.vacation_interval_2 = (int)Tax_Entity.vac_freq_2;
                mo.vacation_interval_3 = (int)Tax_Entity.vac_freq_3;
                mo.vacation_amount_2 = (double)Tax_Entity.vac_amount_2;
                mo.vacation_amount_3 = (double)Tax_Entity.vac_amount_3;
            }

            mo.business_unit_id = model.business_unit_id;
            mo.membertypeid = model.membertypeid;
            mo.reports_to = model.reports_to;
            if (model.enddate.HasValue)
            {
                mo.enddate = (DateTime)model.enddate;
            }
            if (model.startdate.HasValue)
            {
                mo.startdate = (DateTime)model.startdate;
            }
            if (model.isapplicant && old_moid == 0)
			{
				mo.applicantid = model.applicantid;
				mo.isapplicant = true;
			}
			else if (old_moid == 0)
			{
				mo.memberid = member_id;
				mo.isapplicant = false;
				mo.applicantid = 0;
			}
			else
			{
				mo.memberid = old_moid != 0 ? new NeMemberOffer(old_moid).memberid : 0;
			}

			mo.save();
			if (model.isapplicant && model.id == 0)
			{
				var dtmm = bllToolbox.doSQL_dt(@"Select * from new_milestone");
				foreach (DataRow dr in dtmm.Rows)
				{
					var mm = new Memberoffer_Milestones
					{
						milestone = dr["new_milestone"].ToString(),
						due = mo.startdate.AddDays(Convert.ToInt32(dr["days_from_startdate"])),
						addedby = mo.enteredby,
						offerid = mo.id
					};
					mm.save();
				}
			}

			offer_id = mo.id;

			if (!string.IsNullOrEmpty(model.notes))

			{
				var m = new EmployeeOfferNotes()
				{
					new_notes = model.notes,
				};
				this.SaveNotes(m);
			}
			return new DataExtra("Detail has been saved successfully.", GetDetailProfile());
		}

		public object GetDetailProfile()
		{
			var memberTypeList = GetMemberTypeList();
			var reportsToList = GetVisibleUserList(UserId, member_id);
			var visibleBusinessUnitList = CurrentUser.VisibleBusinessUnitLabelValueList;
			var mo = new NeMemberOffer(offer_id);
			//var applicant = new NeApplicant(mo.applicantid);
			var entity = new EmployeeOfferDetail();
			entity = (EmployeeOfferDetail)MapperFrom(entity, mo);
			if (is_applicant)
			{
				entity.fullname = applicant.firstname + " " + applicant.lastname;
			}
			else
			{
				entity.fullname = n1_member?.FullName;
			}
			entity.author = new NeMember(Convert.ToInt32(mo.enteredby)).FullName;
			entity.start_minDate = !is_applicant ? ConvertToDateTime(n1_member?.StartDate) : DateTime.Today;
			if (offer_id == 0)
			{

				entity.memberid = member_id;
				entity.isapplicant = is_applicant;
				entity.applicantid = applicant_id;
				entity.author = CurrentUser.FullName;
				entity.business_unit_id = n1_member?.business_unit_id ?? current_user.business_unit_id;
				DateTime f_date = new DateTime();
				if (!is_applicant)
				{
					var old_moid = GetOldMoId();
					if (old_moid > 0)
					{
						var old_mo = new NeMemberOffer(old_moid);
						entity = (EmployeeOfferDetail)MapperFrom(entity, old_mo);
						var emp = new NeMember(member_id);
						var pp_id = NePayPeriod.get_payperiod_id(old_mo.enddate);
						var pp = new NePayPeriod(pp_id);
						entity.startdate = ConvertToDateTime(pp.Enddate).AddDays(1);                  
						f_date = bllToolbox.doSQL_datetime(@"SELECT get_fiscal_year_start_date(@v0)", entity.business_unit_id)
							.AddYears(1);                       
						entity.business_unit_id = emp.business_unit_id;
						entity.reports_to =emp.reports_to;
					}
				}
				else
				{
					entity.startdate = DateTime.Today;
					if (is_applicant)
					{
						f_date = ConvertToDateTime(bllToolbox.doSQL_string(@"SELECT get_fiscal_year_start_date(@v0)", applicant.business_unit_id))
							.AddYears(1);
					}
					else
					{
						f_date = ConvertToDateTime(bllToolbox.doSQL_string(@"SELECT get_fiscal_year_start_date(@v0)", n1_member?.business_unit_id))
							.AddYears(1);

						entity.benefits_startdate = n1_member.paytype_id > 3 ? DateTime.Today.AddYears(5) : n1_member.benefits_startdate;
					}
				}

				if (f_date.Subtract(System.DateTime.Today).TotalDays < 45 || f_date < entity.startdate)
				{
					var temp_f_date = f_date;
					f_date = ConvertToDateTime(new NePayPeriod(NePayPeriod.get_payperiod_id(f_date.AddYears(1))).Enddate);
					if (f_date < entity.startdate)
					{
						f_date = ConvertToDateTime(new NePayPeriod(NePayPeriod.get_payperiod_id(temp_f_date.AddYears(2))).Enddate);
					}
				}
				else
				{
					f_date = ConvertToDateTime(new NePayPeriod(NePayPeriod.get_payperiod_id(f_date)).Enddate);
				}

				entity.enddate = f_date;

				entity.startdate = null;

				entity.id = 0;
				entity.enteredby = UserId;
				entity.status = "In Development";
			}
			
			var bm_id = GetBMId(entity.business_unit_id);
			if (bm_id > 0 && entity.reports_to == 0)
			{
				entity.reports_to = bm_id;
			}
			

			var payperiod_startdate = Convert.ToDateTime(new NePayPeriod(NePayPeriod.get_payperiod_id(DateTime.Today)).start_date);
			if (entity.membertypeid == 0)
			{
				entity.membertypeid = memberTypeList[0].Value;
			}
			return new
			{
				can_access,
				error_message,
				isowner,
				in_development,
				status,
				entity,
				memberTypeList,
				reportsToList,
				notes = GetHistoryNotes(),
				visibleBusinessUnitList,
				payperiod_startdate,
			};
		}


		public int GetBMId(int bu_id)
		{
			if (bu_id <= 0)
			{
				return 0;
			}

			var bu = new NeBusinessUnit(bu_id);
			return bu.branch_manager?.id ?? 0;
		}

		private int GetOldMoId()
		{
			if (!is_applicant)
			{
				return bllToolbox.doSQL_int(@"Select 
ifnull((Select id from member_offers  where memberid =@v0 and (status = 'Accepted' or status = 'Previous') 
order by id desc limit 1),0)", member_id);
			}
			else
			{
				return bllToolbox.doSQL_int(@"Select 
ifnull((Select id from member_offers  where applicantid =@v0 and memberid!=0
order by id desc limit 1),0)", applicant_id);
			}
		}

	    private List<string> GetStatusOfOffers()
	    {
	        var o = default(EmployeeEmploymentAgreements);
	        var list = new List<string> { };

            if (!is_applicant)
	        {
	            o = new BLL.Pages.Employees.EmployeeEmploymentAgreements(CurrentUser, member_id);
	        }
	        else
	        {
	            o = new BLL.Pages.Employees.EmployeeEmploymentAgreements(CurrentUser, true, applicant_id);
	        }

	        var existingOffers =  o.GetList();
	        if (existingOffers == null) return list;
	        foreach (DataRow row in existingOffers.Rows)
	        {
	            if (row.Table.Columns.Contains("status"))
	            {
	                list.Add(Convert.ToString(row["status"]));
	            }
	        }

	        return list;
	    }

        /// <summary>
        /// copy responsiblities from membertype to memberoffer_cr
        /// </summary>
        protected void prepopulate_crs()
		{
			var mo = new NeMemberOffer(offer_id);

			var dt = bllToolbox.doSQL_dt(@"SELECT * FROM core_responsibilities
INNER JOIN membertype_responsibilities 
ON membertype_responsibilities.core_responsibility_id = core_responsibilities.id 
WHERE core_responsibilities.`status` = 'Active' 
AND membertype_responsibilities.membertype_id =@v0", mo.membertypeid);
			foreach (DataRow dr in dt.Rows)
			{
				Insert_responsibility_from_datarow(dr);
			}
		}


		private int GetOldMemberOfferId()
		{
			return bllToolbox.doSQL_int(
				@"select id from member_offers  where memberid =@v0 and member_offers.status = 'Accepted' order by member_offers.enddate desc limit 1",
				member_id);
		}

		public DataExtra Copy_From_Previous(bool selected)
		{
			var old_moid = GetOldMemberOfferId();

			if (old_moid > 0)
			{
				var dt = bllToolbox.doSQL_dt(@"SELECT * FROM core_responsibilities
WHERE core_responsibilities.`status` = 'Active' AND  id IN (SELECT memberoffer_crid   FROM memberoffer_cr WHERE memberoffer_moid=@p0)", old_moid);
				foreach (DataRow dr in dt.Rows)
				{
					if (selected)
					{
						Insert_responsibility_from_datarow(dr);
					}
					else
					{
						bllToolbox.doSQL_void(@"delete from memberoffer_cr where memberoffer_moid =@v0 and memberoffer_crid=@v1", offer_id, dr["id"]);
					}
				}
			}
			return new DataExtra("success", GetResponsibilitiesList());
		}

		private void Insert_responsibility_from_datarow(DataRow dr)
		{
			if (bllToolbox.doSQL_int(
					@"select count(memberoffer_cr_id) from memberoffer_cr  where memberoffer_crid =@v0 and memberoffer_moid =@v1 ",
					dr["id"], offer_id) == 0)
			{
				bllToolbox.doSQL_void(@"insert into memberoffer_cr 
(memberoffer_crid, memberoffer_moid, cr_wording,daily,weekly,monthly,quarterly,annually,as_required) 
values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8)",
					dr["id"], offer_id, dr["core_responsibility"],
					dr["daily"], dr["weekly"], dr["monthly"],
					dr["quarterly"], dr["annually"],
					dr["as_required"]);
			}
		}


		public DataExtra Select_One_Responsibilities(bool is_checked, int cr_id)
		{
			if (is_checked)
			{
				var dt = bllToolbox.doSQL_dt(@"SELECT * FROM core_responsibilities 
INNER JOIN membertype_responsibilities 
ON membertype_responsibilities.core_responsibility_id = core_responsibilities.id  
WHERE core_responsibilities.id =@v0 limit 1 ", cr_id);
				if (dt.Rows.Count > 0)
				{
					Insert_responsibility_from_datarow(dt.Rows[0]);
				}
			}
			else
			{
				bllToolbox.doSQL_void(@"delete from memberoffer_cr where memberoffer_moid =@v0 and memberoffer_crid=@v1", offer_id, cr_id);
			}
			return
				new DataExtra("success", GetResponsibilitiesList());
		}

		public DataExtra Select_All_Responsibilities(bool selected_all)
		{
			if (selected_all)
			{
				prepopulate_crs();
			}
			else
			{
				bllToolbox.doSQL_void(@"delete from memberoffer_cr where memberoffer_moid =@v0 ", offer_id);
			}
			return
				new DataExtra("success", GetResponsibilitiesList());
		}

		public LabelValueInt[] GetMemberOffCrList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				@"select memberoffer_cr_id label, memberoffer_cr_id value from memberoffer_cr where memberoffer_moid=@v0",
				offer_id);
		}

		public EmployeeOfferResposibility[] GetResponsibilitiesList()
		{
			var mo = new NeMemberOffer(offer_id);
			var list = bllToolbox.doSQL_Array<EmployeeOfferResposibility>(@"call proc_get_cr_listing (@p0, @p1, 0)",
				mo.membertypeid, mo.id);
			var offerList = GetMemberOffCrList();
			foreach (var o in list)
			{
				o.is_checked = offerList.Count(x => x.Value == o.memberoffer_cr_id.GetValueOrDefault(0)) > 0;
			}
			return list;
		}

		public object GetResponsibilitiesProfile()
		{
			return new
			{
				can_access,
				error_message,
				is_backoffice,
				has_old_offer_id = GetOldMemberOfferId() > 0,
				list = GetResponsibilitiesList(),
				entity = new { isnull = true }
			};
		}

		public LabelValueInt[] GetBonusTypeList(int memberType)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"
Select 0 value,'Not  Set' label 
union 
SELECT bonus_type.id  value,
bonus_type.bonus_type label 
FROM bonus_type 
INNER JOIN bonus_membertype_link 
ON bonus_membertype_link.bonus_type_id = bonus_type.id
and bonus_membertype_link.membertype_id =@v0  and bonus_type.status = 1", memberType);
		}


		public DataExtra SaveWage(EmployeeOfferWage model)
		{
            if (model.paytype_id != 6)
            {
                // For NONE 'co-op students(paytype = 6)', wage must be greater than zero.
                if (model.wage <= 0.0)
                {
                    return new DataExtra("Wage cannot be zero or less.", null);
                }
            }
            else
            {
                // For 'co-op students(paytype = 6)', wage could be zero but can't be less than zero.
                if (model.wage < 0.0)
                {
                    return new DataExtra("Wage cannot be less than zero.", null);
                }
            }

			if (model.is_CDN)
			{
				model.vacation_amount_1 = Math.Round(model.vacation_amount_1 / 100, 4);
				model.vacation_amount_2 = Math.Round(model.vacation_amount_2 / 100, 4);
				model.vacation_amount_3 = Math.Round(model.vacation_amount_3 / 100, 4);
			}

            if(model.bonus_amount < 1 && model.bonus_amount != 0)
            {
                return new DataExtra("Percent must be a whole number.", null);
            }
            else
            {
                model.bonus_amount = model.bonus_amount / 100;
            }
			var mo = new NeMemberOffer(model.id);
			mo = (NeMemberOffer)MapperFrom(mo, model);
			mo.is_salary = model.paytype_id == 2 || model.paytype_id == 3;
			mo.has_comp = string.IsNullOrWhiteSpace(mo.comp_details) ? 0 : 1;
            
			mo.save();
            
			return new DataExtra("Wage has been saved successfully.", GetWageProfile());
            
        }

		public object GetWageProfile()
		{
			var mo = new NeMemberOffer(offer_id);
			var entity = new EmployeeOfferWage();
			var paytypeList = GetPayTypeList();
			var bonusTypeList = GetBonusTypeList(mo.membertypeid);
			var vendorList = bllToolbox.doSQL_List<LabelValueInt>(@"
(SELECT '0' `value`,'Not Set' label) UNION
(SELECT
vendor.Vendor_ID `value`,
vendor.Vendor_Name label
FROM
vendor
where Vendor_Active = 1
order by label)
");
			entity = (EmployeeOfferWage)MapperFrom(entity, mo);

			entity.bonus_amount_minValue = bllToolbox.doSQL_double(@"Select ifnull(min_amount,0) from bonus_type  where id =@v0 ", mo.bonus_type);
			entity.bonus_amount_maxValue = bllToolbox.doSQL_double(@"Select ifnull(max_amount,0) from bonus_type  where id =@v0", mo.bonus_type);
			entity.bonus_amount_tootip = bllToolbox.doSQL_string(@"select tooltip from bonus_type  where id =@v0", mo.bonus_type);
			entity.is_CDN = (Global.BusinessUnit.GetValue(mo.business_unit_id).Country == "CDN");
			if (mo.bonus_type > 0)
			{
				entity.comp_details = bllToolbox.doSQL_string(@"select IFNULL(MAX(offer_verbiage), '') from bonus_type  where id =@v0", mo.bonus_type);
				entity.comp_details =
					entity.comp_details.Replace("{branch}", Global.BusinessUnit.GetValue(mo.business_unit_id).Name);
				if (entity.bonus_margin_threshold > 0)
				{
					entity.comp_details = entity.comp_details.Replace("{margin}", entity.bonus_margin_threshold.ToString("C2"));
				}

				if (entity.bonus_netincome_threshold > 0)
				{
					entity.comp_details = entity.comp_details.Replace("{netincome}", entity.bonus_netincome_threshold.ToString("C2"));
				}
				if (entity.bonus_revenue_threshold > 0)
				{
					entity.comp_details = entity.comp_details.Replace("{revenue}", entity.bonus_revenue_threshold.ToString("C2"));
				}
				if (entity.bonus_netincome_highwater > 0)
				{
					entity.comp_details =
						entity.comp_details.Replace("{highwatermark}", entity.bonus_netincome_highwater.ToString("C2"));
				}
				if (entity.bonus_amount > 0)
				{
					entity.comp_details =
						entity.comp_details.Replace("{bonus_amount}", entity.bonus_amount.ToString("P0"));
				}

			}

			entity.minwage_1 = bllToolbox.doSQL_double(@"SELECT ifnull(min(m.WAGE),0) 
from currentwage m,membertype  where m.member_status = 'Active' 
and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID =@v1 ", mo.membertypeid, mo.business_unit_id);
			entity.maxwage_1 = bllToolbox.doSQL_double(@"SELECT ifnull(max(m.WAGE),0) 
from currentwage m,membertype  where m.member_status = 'Active' 
and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID =@v1 ", mo.membertypeid, mo.business_unit_id);
			entity.avgwage_1 = bllToolbox.doSQL_double(@"SELECT ifnull(avg(m.WAGE),0) 
from currentwage m,membertype  where m.member_status = 'Active' 
and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID =@v1 ", mo.membertypeid, mo.business_unit_id);
			entity.minwage_2 = bllToolbox.doSQL_double(@"SELECT ifnull(min(m.WAGE),0) 
from currentwage m,membertype  where m.member_status = 'Active' 
and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID <>8", mo.membertypeid);
			entity.maxwage_2 = bllToolbox.doSQL_double(@"SELECT ifnull(max(m.WAGE),0) 
from currentwage m,membertype  where m.member_status = 'Active' 
and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID <>8", mo.membertypeid);
			entity.avgwage_2 = bllToolbox.doSQL_double(@"SELECT ifnull(avg(m.WAGE),0) 
from currentwage m,membertype  where m.member_status = 'Active' 
and m.mt = membertype.membertype_id and m.mt =@v0 and m.business_unit_ID <>8", mo.membertypeid);
			var startdate = n1_member != null ? Convert.ToDateTime(n1_member.StartDate) : mo.startdate;
			entity.benefits_startdate = mo.benefits_startdate.GetValueOrDefault(startdate.AddMonths(3));
			entity.startdate = mo.startdate;
			entity.start_wage =
				bllToolbox.doSQL_double(
					@"select ifnull((Select currentwage from memberwage  where memberwage_memberid =@v0 order by date limit 1),0)",
					UserId);
			entity.wage_notes = GetWageNote(mo.wage, mo.paytype_id, mo);
			if (n1_member != null)
			{
				entity.current_wage = (new NeWage(n1_member.id)).current_wage;
			}
			else
			{
				entity.current_wage = 0;
			}

			if (entity.is_CDN)
			{
				entity.vacation_amount_1 = Math.Round(entity.vacation_amount_1 * 100, 2);
				entity.vacation_amount_2 = Math.Round(entity.vacation_amount_2 * 100, 2);
				entity.vacation_amount_3 = Math.Round(entity.vacation_amount_3 * 100, 2);
			}
			var result = new 
			{
				can_access,
				error_message,
				isowner,
				paytypeList,
				entity,
				bonusTypeList,
				vendorList,
			};

            //this will display the whole number the user has entered on the front end 
            result.entity.bonus_amount =entity.bonus_amount * 100;
            return result;
		}
		public EmployeeOfferWageNotes GetWageNote(double _wage, int type, NeMemberOffer mo = null)
		{
			if (mo == null) mo = new NeMemberOffer(offer_id);
			var mtype = mo.membertypeid;
			var _bu_id = mo.business_unit_id;
			var paytypeid = type;
			var previous_wage = 0.0;
			if (n1_member != null)
			{
				previous_wage = (new NeWage(n1_member.id)).current_wage;
			}
			var salarywage = 0.0;
			var margin = 0.0;
			var increase = 0.0;
			var chargeout =
				bllToolbox.doSQL_double(
					@"Select ifnull((Select chargeout from membertype_chargeout  where paytype_id = 1 and membertype_id =@v0 and business_unit_id =@v1),0) ",
					mtype, _bu_id);
			if (chargeout > 0)
			{
				if (paytypeid == 2 || paytypeid == 3)
				{
					salarywage = Math.Round(_wage * 20.80, 0) * 100;
				}
				margin = Math.Round((chargeout - _wage * 1.15) / chargeout, 4);

				if (previous_wage > 0)
				{
					increase = Math.Round((_wage - previous_wage) / previous_wage, 4);
				}
				else
				{
					increase = 0;
				}
			}
			return new EmployeeOfferWageNotes
			{
				salarywage = salarywage,
				margin = margin,
				increase = increase,
				chargeout = chargeout
			};
		}

		public LabelValueInt createEmployeeFromApplicant(NeMemberOffer mo)
		{
			#region created the user here.. not on the index page

			var n_info = mo.memberid == 0 ? new NeMember() : new NeMember(mo.memberid);
			// create new username and password


			// Does username exist in any other users?
			if (Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member WHERE member_user = @v0 AND member_id != @v1", new object[] {
						applicant.firstname.Substring(0, 1).ToLower() + applicant.lastname.ToLower(), member_id}) > 0) // if another user with the exact same name exists..
			{
				n_info.Username = applicant.firstname.Replace(" ", "").ToLower() + applicant.lastname.Replace(" ", "").ToLower() + mo.applicantid;
				n_info.Password = applicant.firstname.Substring(0, 1).ToLower() + applicant.lastname + mo.id;
			}
			else
			{
				n_info.Username = applicant.firstname.Replace(" ", "").Substring(0, 1).ToLower() + applicant.lastname.Replace(" ", "").ToLower();
				n_info.Password = applicant.firstname.Substring(0, 1).ToLower() + applicant.lastname.Replace(" ", "") + mo.id;
			}



			n_info.FirstName = applicant.firstname;
			n_info.LastName = applicant.lastname;
			n_info.Address = applicant.address;
			n_info.City = applicant.city;
			n_info.Prov = applicant.province;
			n_info.Country = applicant.country;
			n_info.PostalCode = applicant.postal;
			n_info.Email = applicant.email;
			n_info.empnotes = applicant.notes;

			//		n_info.hrstatus_id					= id == 0 ? 1 : Convert.ToInt32(ddl_hrstatus.SelectedValue) == 3 && o_info.hrstatus_id != 3 ? 6 : Convert.ToInt32(ddl_hrstatus.SelectedValue);
			n_info.hrstatus_id = 1;
			n_info.NEEmail = "nomail@thatsnew.com";
			n_info.benefits_startdate = mo.benefits_startdate;
			if (mo.paytype_id > 3 && mo.paytype_id != 7) // is it a subcontractor?
			{
				n_info.benefits_startdate = mo.startdate.AddYears(50);
				n_info.timetostat = 9999;
				n_info.receive_stat_pay = 0;
			}
			else
			{
				n_info.timetostat = n_info.Country == "CAN" ? 0 : 90;
				n_info.receive_stat_pay = 1;
			}

			n_info.vacation_amount_1 = (decimal)mo.vacation_amount_1;
			n_info.vacation_amount_2 = (decimal)mo.vacation_amount_2;
			n_info.vacation_amount_3 = (decimal)mo.vacation_amount_3;
			n_info.vacation_interval_1 = mo.vacation_interval_1;
			n_info.vacation_interval_2 = mo.vacation_interval_2;
			n_info.vacation_interval_3 = mo.vacation_interval_3;
			n_info.reports_to = mo.reports_to;
			// if their reports to IS a payroll handler, set it accordingly
			n_info.payroll_handler = mo.reports_to > 0 && new NeMember(mo.reports_to).AuthenticatedForPage(47)
										? mo.reports_to
										: new NeBusinessUnit(mo.business_unit_id).branch_manager.id;
			n_info.PhoneAreaCode = "";
			n_info.PhoneFirst = "";
			n_info.PhoneLast = "";
			//		n_info.Username = applicant.firstname.Substring(0, 1).ToLower() + applicant.lastname;
			//		n_info.Password = applicant.firstname.Substring(0, 1).ToLower() + applicant.lastname + mo.id;
			n_info.StartDate = Toolbox.MySQL_shortdt(mo.startdate);
			n_info.TerminateDate = "2099-12-31";
			n_info.business_unit_id = mo.business_unit_id;


			n_info.Status = "Active";
			n_info.MemberTypeID = mo.membertypeid;
			n_info.is_CAN_boardmember = false;
			n_info.is_US_boardmember = false;
			n_info.Nickname = applicant.firstname;
			n_info.paytype_id = mo.paytype_id == 0 ? 1 : mo.paytype_id;


			if (applicant.cellphone.Length == 10)
			{
				n_info.pecell_area = applicant.cellphone.Substring(0, 3);
				n_info.pecell_pref = applicant.cellphone.Substring(3, 3);
				n_info.pecell_suff = applicant.cellphone.Substring(6, 4);
			}


			n_info.part_time = mo.part_time;
			//if (n_info.id == 0)  // if it's a new user
			//{
			try
			{
				n_info.save();
			}
			catch (Exception ee)
			{
				return new LabelValueInt(ee.Message, -1);
			}
			//}
			#region Kick off PreLive function
			mo.memberid = n_info.id;
			mo.save();
			NeMemberOffer.go_prelive(mo.id, current_user, "hired");
			var a = new NeApplicant(mo.applicantid) { becomes_memberid = n_info.id };
			a.save();
			return new LabelValueInt("success", mo.memberid);
			#endregion

			#endregion


		}

		private void clear_preview_file()
		{
			bllToolbox.doSQL_void(@"DELETE FROM filestore.files  WHERE folder_id=4 AND sub_folder_id = @v0", offer_id);
		}

        /// <summary>
        /// Get all offers for current user.
        /// </summary>
        /// <returns></returns>
	    private List<KeyValuePair<int, string>> GetOffers()
	    {
	        var o = default(EmployeeEmploymentAgreements);
	        var list = new List<KeyValuePair<int, string>> { };

	        if (!is_applicant)
	        {
	            o = new BLL.Pages.Employees.EmployeeEmploymentAgreements(CurrentUser, member_id);
	        }
	        else
	        {
	            o = new BLL.Pages.Employees.EmployeeEmploymentAgreements(CurrentUser, true, applicant_id);
	        }

	        var existingOffers = o.GetList();
	        if (existingOffers == null) return list;
	        foreach (DataRow row in existingOffers.Rows)
	        {
	            if (row.Table.Columns.Contains("status"))
	            {
	                string status = Convert.ToString(row["status"]);
	                int offerid = Convert.ToInt32(row["id"]);
                    list.Add(new KeyValuePair<int, string>(offerid, status));
	            }
	        }

	        return list;
	    }

        /// <summary>
        /// Make the current offer become the accepted one.
        /// </summary>
        /// <param name="s">The status that what to be changed to, here it is 'accepted'</param>
        /// <param name="data2">The confirm info from client (angular)</param>
        /// <returns></returns>
        public DataExtra ChangeStatus_OverwriteAcceptedOffer(string s, string data2)
        {
            // List all offers then check as below shows up:
            // (1) set 'accepted' offer to 'previous'
            // (2) set 'previous' offer to 'past'.
            var offers = GetOffers();
            foreach (var offer in offers)
            {
                if (offer.Value.ToLower() == "Accepted".ToLower())
                {
                    var mo = new NeMemberOffer(offer.Key);
                    mo.status = "Previous";
                    mo.save();
                }

                if (offer.Value.ToLower() == "Previous".ToLower())
                {
                    var mo = new NeMemberOffer(offer.Key);
                    mo.status = "Past";
                    mo.save();
                }
            }

            // Call existing function to change the status for current offer.
            return ChangeStatus(s);
        }

        public DataExtra ChangeStatus(string s)
		{
			var mo = new NeMemberOffer(offer_id);
			var em = new NeEMail();

            if (!(s == "In Development" || s == "Closed"))
            {
                if (mo.paytype_id == 0)
                {
                    return new DataExtra($"Please select an employment type for this offer", GetNotes(0));
                }
            }

			switch (s)
			{
				case "Closed":
					bllToolbox.doSQL_void(@"Delete from memberoffer_cr where memberoffer_cr.memberoffer_moid =@v0", offer_id);
					break;
				case "In Development":
					clear_preview_file();
					break;
				case "Send for Approval":
					try
					{
						var hl = string.Format(@"<a href ='{0}/#/opens/127/employees/" + mo.memberid + "/offer/" + mo.id + "' target='blank' >Click Here</a>", Toolbox.app_setting("Domain"));
						em.isHTML = true;
						em.Body = "<div font-face='Arial'>";
						if (mo.isapplicant)
						{
							em.Subject = "Applicant Offer Awaiting Your Approval";
							em.Body += "Please review the offer for " + new NeApplicant(mo.applicantid).firstname + " " +
									   new NeApplicant(mo.applicantid).lastname + " in " + new NeBusinessUnit(mo.business_unit_id).name +
									   ".";
						}
						else
						{
							em.Subject = "Employment agreement Awaiting Your Approval";
							em.Body += "Please review the agreement for " + new NeMember(mo.memberid).FullName2 + " in " +
									   new NeBusinessUnit(mo.business_unit_id).name + ".";
						}
						try
						{
							var final_review = 0;
							if (mo.reports_to == mo.enteredby)
							{
								final_review = Convert.ToInt32(new NeMember(Convert.ToInt32(mo.reports_to)).reports_to);
							}
							else
							{
								final_review = Convert.ToInt32(NeMember.is_supervisor(mo.reports_to, mo.enteredby) ?
									new NeMember(Convert.ToInt32(mo.enteredby)).reports_to
									:
									new NeMember(Convert.ToInt32(mo.reports_to)).reports_to);
							}

							if (final_review == 0)
							{
								if (mo.business_unit_id == 3)
								{
									final_review = 18;
								}
								else if (System.DateTime.Today < new DateTime(2016, 1, 1))
								{
									final_review = 19;
								}
								else
								{
									final_review =
										bllToolbox.doSQL_int(@"Select ifnull((Select member_id from member  where member_membertype_id = 35 limit 1),0)");
								}
							}
							em.To = new NeMember(final_review == 0 ? mo.reports_to : final_review).NEEmail;
						}
						catch
						{
							em.To = "debug@" + Toolbox.app_setting("DomainForEmail");
							em.Subject = "Offer Awaiting Your Approval - because something is broke";
						}
						em.From = current_user.NEEmail;
						em.Body += System.Environment.NewLine + hl;
						em.Send();

						mo.status = "Waiting for Approval";
						status = mo.status;
						mo.save();
						return new DataExtra($"Offer status has been changed successfully.", GetNotes(0));
					}
					catch
					{
						// ignored
						return new DataExtra($"Offer status has been changed  failed.", GetNotes(0));
					}
				case "Accepted":
					if (mo.status != "Released")
					{
						return new DataExtra("The offer must bear the status 'Released' in order for the offer to be accepted", GetNotes(0));
					}

				    var existingOffers = GetStatusOfOffers();

				    var old_mid = GetOldMoId();
					var old_memberOffer = old_mid != 0 ? new NeMemberOffer(old_mid) : new NeMemberOffer();
					if (mo.isapplicant && old_mid == 0)
					{
                        // In case of "there is already having an [Awaiting start date] offer, we should not create an employee again becasue it is already in there.
                        // Also in this case, no coming offer will be becoming "Accepted" or "Awaiting Start Date".
					    var existingAwaitingStartDateCount = existingOffers.Count(item => string.Equals(item, "Awaiting Start Date", StringComparison.CurrentCultureIgnoreCase));
					    if (existingAwaitingStartDateCount > 0)
					    {
					        return new DataExtra("Only one offer may be set to \"Awaiting Start Date\"", GetNotes(0));
					    }

                        var r = createEmployeeFromApplicant(mo);
						if (r.Label != "success")
						{
							return new DataExtra(r.Label, GetNotes(0));
						}
						member_id = r.Value;
						is_applicant = false;
						status = "Accepted";
						return new DataExtra("Offer status has been changed  successfully.", GetNotes(0));
					}
					else
					{
					    if (mo.startdate > System.DateTime.Today)
					    {
                            // The current coming offer will be becoming "Awaiting Start Date" offer if and only if there are no existing "Awaiting Start Date" offers.
                            var existingAwaitingStartDateCount = existingOffers.Count(item => string.Equals(item, "Awaiting Start Date", StringComparison.CurrentCultureIgnoreCase));
					        if (existingAwaitingStartDateCount > 0)
					        {
					            return new DataExtra("Only one offer may be set to \"Awaiting Start Date\"", GetNotes(0));
					        }
                        }
					    else
					    {
                            // The current coming offer will be becoming "Accepted" offer if and only if there are no existing "Awaiting Start Date" and "accepted" offers
					        var existingAwaitingStartDateCount = existingOffers.Count(item => string.Equals(item, "Awaiting Start Date", StringComparison.CurrentCultureIgnoreCase));
					        if (existingAwaitingStartDateCount > 0)
					        {
					            return new DataExtra("There is already an offer awaiting start date. You must expire that offer first before proceeding with this offer.", GetNotes(0));
					        }

                            var existingAcceptedCount = existingOffers.Count(item =>
					            string.Equals(item, "Accepted", StringComparison.CurrentCultureIgnoreCase));
					        if (existingAcceptedCount > 0)
					        {
					            return new DataExtra("This employee already has a current accepted offer, would you like this offer to be effective right away?", GetNotes(1));
					        }
                        }

					    status = "Accepted";

						if (mo.isapplicant && old_mid != 0)
						{
							n1_member = new NeMember(old_memberOffer.memberid);
							member_id = old_memberOffer.memberid;
						}
						if (n1_member != null && (n1_member.hrstatus_id == 5 || n1_member.hrstatus_id == 4))
						{
							NeMemberOffer.go_prelive(mo.id, current_user, "reinstated");
						}
						em.isHTML = true;
						em.Bcc = "payroll@" + Toolbox.app_setting("DomainForEmail");


						if (mo.startdate > System.DateTime.Today)
						{
							mo.status = "Awaiting Start Date";
							em.Subject = "Employment agreement for " + n1_member?.FullName +
										 " has been accepted. But will not go into effect until " + mo.startdate.ToString("yyyy-MM-dd");
							em.isHTML = true;
							var str_body = @"<table style='width: 100%; font-family: Arial; font-size: small;'>
				<tr>
					<td bgcolor='Lime' colspan='2'>
						Employment Agreement Accepted</td>
				</tr>
				<tr>
					<td colspan='2'>
						The system will update their details automatically on " + mo.startdate.ToString("yyyy-MM-dd") + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Employee:</b></td>
					<td nowrap='nowrap' width='100%'>
						<a href='" + Toolbox.app_setting("Domain") + "/#/opens/127/employees/" + member_id + "'>" + n1_member?.FullName2 + @"  (" +
										   member_id + @")</a>
					</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Branch:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + new NeBusinessUnit(mo.business_unit_id).name + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Agreement:</b></td>
					<td nowrap='nowrap' width='100%'>
						<a href='" + Toolbox.app_setting("Domain") + "/#/opens/127/employees/" + member_id + "/offer/" + mo.id + "'>" + mo.id + @"</a>
					</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Agreement Start Date:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + mo.startdate.ToLongDateString() + @"</td>
				</tr>
				<tr>
					<td nowrap='nowrap'>
						<b>Agreement End Date:</b></td>
					<td nowrap='nowrap' width='100%'>
						" + mo.enddate.ToLongDateString() + @"</td>
				</tr>
			</table>";

							em.Body = str_body;
							try
							{
								if (n1_member != null)
								{
									em.To = new NeBusinessUnit(n1_member.business_unit_id).branch_manager.NEEmail;
								}
							}
							catch (Exception ee)
							{
								Toolbox.do_errorLog(ee);
								em.To = "debug@" + Toolbox.app_setting("DomainForEmail");
								em.Body = "This Email has been sent to Debug because The system couldnt find a branch manager.<br>" + str_body;
							}
							em.From = current_user.NEEmail;
							em.Send();

						}
						else // if the offer is already to be put in effect starting now
						{
							mo.status = "Accepted";
							NeMemberOffer.go_live(mo.id);
						}
					}
					mo.save();
					status = mo.status;

					return new DataExtra($"This offer has been accepted and " + Toolbox.app_setting("Domain") + " files have been updated successfully.", GetNotes(0));
				default:
					break;
			}
			mo.status = s;
			mo.save();

			status = s;
			return new DataExtra($"Offer has been {s}  successfully.", GetNotes(0));
		}
		 public DataExtra ReturnErrorMessage()
		{
			return new DataExtra("There exists credit card transactions for this employee which have not integrated into NetSuite. Please try again later.",GetNotes(0));
		}
    }
}