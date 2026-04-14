using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http.ModelBinding;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Common.Cache;
using NESI.BLL.Common.Shared;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.Common.Models;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet;
using huddle = nesi.core.huddle;
using Ne2WOProg = NESI.BLL.Core.Ne2WOProg;
using User = NESI.DTO.ViewModels.Page.TimeSheet.User;


// ReSharper disable CompareOfFloatsByEqualityOperator
#pragma warning disable 168

namespace NESI.BLL.Pages.Timesheet
{
	public partial class Timesheet : BLLBase
	{
		public bool CanSwitchBusinessUnits { get; set; }
		public bool CanPickAnyDate { get; set; }
		public bool IsPayrollAdmin { get; set; }
		public bool CanDoShopTime { get; set; }
		public bool VacationVisible { get; set; }
		public bool BankVisible { get; set; }
		public PayPeriod PayPeriod { get; set; }
		public Ne2PayPeriod CurrentPayPeriod { get; set; }
		public string MinDate { get; set; }
		public string MaxDate { get; set; }
		public bool IsCurrentPayPeriod { get; set; }
		public bool AllowUnlinkedTimesheet { get; set; }
        public bool AllowVacationWithdrawal { get; set; }
        public string[] PastDays;
		public DefaultFromScheduler DefaultValueFromScheduler;

		public Timesheet()
		{

		}

		public Timesheet(Employee user) : base(user)
		{
			AllowVacationWithdrawal = CurrentUser.BusinessUnit.allow_vac_withd;
			AllowUnlinkedTimesheet = CurrentUser.BusinessUnit.allow_unlinked_timesheet.GetValueOrDefault();
			CanSwitchBusinessUnits = CurrentUser.AuthenticatedForPrivilege(42);
			CanPickAnyDate = CurrentUser.AuthenticatedForPrivilege(29);
			IsPayrollAdmin = CurrentUser.AuthenticatedForPrivilege(165);
			CanDoShopTime = CurrentUser.AuthenticatedForPrivilege(90);
			PayPeriod = new Ne2PayPeriod().PayPeriod();
			CurrentPayPeriod = new Ne2PayPeriod(new Ne2Payroll().Working_pay_period());

			VacationVisible = CurrentUser.AuthorizePage(28) && CurrentUser.MemberType.membertype_id != 85 && !AllowUnlinkedTimesheet;
			BankVisible = !new List<int>(new[] { 2, 3, 4, 5 }).Contains(CurrentUser.EmployeeProfile.paytype_id) && CurrentUser.MemberType.membertype_id != 85 && !AllowUnlinkedTimesheet && CurrentUser.BusinessUnit.allow_bankedpay;
            MinDate = !CanPickAnyDate
                ? DateTime.Today.AddDays(-1).ToShortDateString()
            //    : CurrentPayPeriod.StartDate;
                  : new DateTime(2019,3,3).ToShortDateString(); // we launch the net-suite on this day.
			MaxDate = DateTime.Today.ToShortDateString();
			PastDays = GetPastDays().Select(x => x.Date.ToString("yyyy-MM-dd")).ToArray();
			DefaultValueFromScheduler = GetDefaultValueFromScheduler(DateTime.Today);
		}

		public object getPayperiodTotalHours(Employee user)
		{
			var total = 0.0;
			var total1 = 0.0;
			var total2 = 0.0;
			var ototal = 0.0;
			var ototal1 = 0.0;
			var ototal2 = 0.0;

			var dt_total = 0.0;
			var dt_total1 = 0.0;
			var dt_total2 = 0.0;

			var rtsp_total = 0.0;
			var rtsp_total1 = 0.0;
			var rtsp_total2 = 0.0;


			var otsp_total = 0.0;
			var otsp_total1 = 0.0;
			var otsp_total2 = 0.0;

			var dtsp_total = 0.0;
			var dtsp_total1 = 0.0;
			var dtsp_total2 = 0.0;

			//var vac_total = 0.0;
			//var vac_total1 = 0.0;
			//var vac_total2 = 0.0;

			DataTable employees = null;
			try
			{
				employees = bllToolbox.doSQL_dt(@"CALL REPORT_PAYROLL(@v0 , @v1 , @v2 )", Toolbox.MySQL_shortdt(PayPeriod.StartDate),
					Toolbox.MySQL_shortdt(PayPeriod.EndDate), user.BusinessUnitId);
			}
			catch (Exception e)
			{
				// igonore
				return 0.0;
			}

			if (employees == null || employees.Rows.Count == 0)
			{
				return 0.0;
			}
			var this_employee = (from DataRow dr in employees.Rows where (int)dr["employee_id"] == user.Id select dr).ToList();
			if (this_employee.Count == 0) return 0.0;
			var week1 = -1;
			foreach (var _data in this_employee)
			{
				var week_n = (int)_data["week_n"];
				if (week1 < 0)
				{
					week1 = week_n;
				}
				var wotype = _data["wotype"].ToString();
				if (wotype != "VAC")
				{
					total += (double)_data["rt"];
					ototal += (double)_data["ot"];
					dt_total += (double)_data["dt"];
					rtsp_total += (double)_data["rtsp"];
					otsp_total += (double)_data["otsp"];
					dtsp_total += (double)_data["dtsp"];


					if (week_n == week1)
					{
						total1 += (double)_data["rt"];
						ototal1 += (double)_data["ot"];
						dt_total1 += (double)_data["dt"];
						rtsp_total1 += (double)_data["rtsp"];
						otsp_total1 += (double)_data["otsp"];
						dtsp_total1 += (double)_data["dtsp"];
					}
					else
					{
						total2 += (double)_data["rt"];
						ototal2 += (double)_data["ot"];
						dt_total1 += (double)_data["dt"];
						rtsp_total2 += (double)_data["rtsp"];
						otsp_total2 += (double)_data["otsp"];
						dtsp_total2 += (double)_data["dtsp"];
					}
				}
			}

			return new
			{
				total,
				total1,
				total2,
				ototal,
				ototal1,
				ototal2,
				dt_total,
				dt_total1,
				dt_total2,
				rtsp_total,
				rtsp_total1,
				rtsp_total2,
				otsp_total,
				otsp_total1,
				otsp_total2,
				dtsp_total,
				dtsp_total1,
				dtsp_total2,
			};
		}

		public DefaultFromScheduler GetDefaultValueFromScheduler(DateTime date)
		{

			if (CurrentUser.MemberType.membertype_id == 78 || CurrentUser.MemberType.membertype_id == 114)
			{
				var p = new BLL.Base.ProfileBase(CurrentUser);

				return new DefaultFromScheduler
				{
					UserId = CurrentUser.Id,
					Business_Unit_Id = CurrentUser.BusinessUnitId,
					EntryType = 0,
					Comment = TimesheetAutoComments(CurrentUser.Id, date),
					Hours = 8,
					PayType = 1,
					Customer_Id = Convert.ToInt32(p.GetValueByPropertyName("Timesheet_Customer")),
					Id = Convert.ToInt32(p.GetValueByPropertyName("Timesheet_WorkOrder")),
				};
			}

			var list = _db.DataTable("call get_timesslice_default_from_scheduler(@p0,@p1)", CurrentUser.Id, date.ToString("yy-MM-dd"));
			if (list != null && list.Rows.Count > 0)
			{

				var item = list.Rows[0];

				return new DefaultFromScheduler
				{
					UserId = CurrentUser.Id,
					Date = Convert.ToDateTime(item["_date"]),
					Business_Unit_Id = Convert.ToInt32(item["business_unit_id"]),
					Id = Convert.ToInt32(item["_id"]),
					Customer_Id = Convert.ToInt32(item["customer_id"]),
					Hours = Convert.ToInt32(item["hours"]),
					EntryType = Convert.ToInt32(Enum.Parse(typeof(WoTypeEnum), item["_entry_type"].ToString())),
					PayType = Convert.ToInt32(item["pay_type"]),
					Comment = ""
				};
			}
			return null;
		}

		public List<BusinessUnitDropDownList> FillBusinessUnits()
		{

			var limiter = CanSwitchBusinessUnits
				? "( FIND_IN_SET(b.id, @p0))"
				: CurrentUser.TaxEntity.allow_interbu_ts.GetValueOrDefault()
					? " b.tax_entity_id = " + CurrentUser.TaxEntityId
					: " b.id = " + CurrentUser.BusinessUnitId;
			return _db.Database.SqlQuery<BusinessUnitDropDownList>($@"
SELECT 
	b.*, t.public_name tax_entity_name
FROM 
	business_unit b inner join tax_entity t on b.tax_entity_id=t.id
WHERE 
	b.enable_timesheet = 1 and
	{limiter}
ORDER BY 
	b.ddl_name", CurrentUser.VisibleBusinessUnits).ToList();
		}

		public bool GetEditPermission(object userId, DateTime date)
			{
				var payperiod_id = NePayPeriod.get_payperiod_id(date);
				var thispayperiod = new NePayPeriod(payperiod_id);
				if (payperiod_id <= 0) return true;
				var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM payroll_hours WHERE member_id = @v0  AND payperiod_id = @v1 ",
					new object[] { userId, payperiod_id });
				return c == 0 && !thispayperiod.completed  /*|| (c > 0 && IsPayrollAdmin)*/;
			}

		public List<TimeSheetDay> GetPastDays()
		{
			return GetPastDays(DateTime.Today.AddDays(-Common.Shared.Configuration.TimesheetPastDayLength), DateTime.Today, CurrentUser.Id, CurrentUser.BusinessUnitId);
		}

        public List<TimeSheetDay> GetPastDays(DateTime? startDT)
        {
            return GetPastDays((DateTime)startDT, DateTime.Today, CurrentUser.Id, CurrentUser.BusinessUnitId);
        }

        public List<DateTime> GetStarEndtDate(Employee user)
        {
            List<DateTime> result = new List<DateTime>();
            var startDate = (DateTime)user.EmployeeProfile.member_startdate;
            int daysDiff = ((TimeSpan)(startDate - DateTime.Today)).Days;
            if(daysDiff<(-Common.Shared.Configuration.TimesheetPastDayLength))
            {
                startDate= DateTime.Today.AddDays(-Common.Shared.Configuration.TimesheetPastDayLength);
            }
            result.Add(startDate);
            return result;            
        }

            public List<TimeSheetDay> GetPastDays(DateTime startDate, DateTime endDate, int userId, int buId)
		{
			return _db.Database.SqlQuery<TimeSheetDay>(
				@"Select MT.MemberTime_ID, MT.business_unit_id ,MT.Date ,MT.MemberTime_WorkOrder_ID workorder_id, MT.MemberTime_Cust_No, 
MT.MemberTime_Customer_Name customer_name,if(MemberTime_Mileage = 'false', MT.NumberofHours, 0 ) as Hours,if(MemberTime_Mileage = 'true', MT.NumberofHours, 0) as Miles,
MT.MemberTime_WoComment_ID,MT.MemberTime_PayTypeHours_ID,MT.Created_Date, Hour.Description as HourType,M.Member_User as MTBy, MT.MemberTime_Premium, 
WOC.Comments as Comments 
from ((membertime MT left Join wocomment WOC ON MT.MemberTime_WoComment_ID = WOC.WoComment_ID)
LEFT JOIN paytypehours hour ON MT.MemberTime_PayTypeHours_ID = hour.PayTypeHours_ID) 
LEFT JOIN member M ON MT.business_unit_id = M.Member_ID 
Where MT.Date BETWEEN @p0 
 AND @p1  AND MT.membertime_memberid = @p2  AND MT.business_unit_id=@p3",
				startDate, endDate, userId, buId).ToList();
		}

		public User[] FillUsers(int businessUnitId)
		{
			if (businessUnitId == 0) businessUnitId = CurrentUser.BusinessUnitId;

			var businessUnit = Global.BusinessUnit.GetValue(businessUnitId);
			var taxEntity = Global.TaxEntity.GetValue(businessUnit.tax_entity_id);
			var limiter = "";
			if (taxEntity.allow_interbu_ts.GetValueOrDefault())
			{
				limiter = "b.tax_entity_id = " + businessUnit.tax_entity_id;
			}
			else if (CanSwitchBusinessUnits)
			{
				limiter = "b.id = " + businessUnit.ID;
			}
			else
			{
				limiter = "a.member_id = " + CurrentUser.Id;
			}
			return _db.Database.SqlQuery<User>($@"
SELECT 
	0 id, 
	'Please Select' fullname,
	'' ddl_name,
	'' firstname,
	'' lastname,
	0 orderby
UNION
SELECT 
	a.member_id id,
	CONCAT(b.ddl_name, ' - ',member_fullname) fullname,
	b.ddl_name,
	a.member_nickname firstname,
	a.member_lastname lastname,
	1 orderby
FROM 
	member a
INNER JOIN
	business_unit b ON a.business_unit_id = b.id
WHERE 
	{limiter} AND
	(member_hrstatus_id IN (3,6) or (member_hrstatus_id IN (4,5) and (DATEDIFF(CURDATE(),member_termdate)) < 20))
ORDER BY 
	orderby, ddl_name, firstname,lastname").ToArray();

		}

		public List<DTO.ViewModels.Page.TimeSheet.Timesheet> GetList(int userId, DateTime date)
		{

            using (var conn = Toolbox.connect())
            {
                var list = _db.Database.SqlQuery<DTO.ViewModels.Page.TimeSheet.Timesheet>(@"
  SELECT
	MT.*,
    MT.MemberTime_ID id,
    MT.business_unit_id BusinessUnitId,
    MT.Date,
    MT.MemberTime_WorkOrder_ID WorkorderId,
    MT.MemberTime_Cust_No CustNo,
    MT.MemberTime_Customer_Id CustId, 
    MT.MemberTime_Customer_Name CustName, 
    IF(
        MemberTime_Mileage = 'false',
        MT.NumberofHours,
        0
    ) AS Hours,
    MT.MemberTime_Child_WorkOrder_ID ChildWo,
    IF(
        MemberTime_Mileage = 'true',
        MT.NumberofHours,
        0
    ) AS Miles,
    MT.MemberTime_WoComment_ID WoCommentId,
    MT.MemberTime_PayTypeHours_ID PayTypeId,
    MT.Created_Date CreatedDate,
    Hour.Description AS HourType,
	Hour.paytypehours_id as HourTypeID,
    M.Member_User AS MemberUser,
    m.member_id MemberId,
    MT.MemberTime_Premium MemberPremium,
    WOC.Comments AS Comments,
	MT.woType,
	MT.wo_percent_complete,
	MT.rating,
	MT.ts_lite_paytype_id as TsLitePaytypeId,
	ifnull(w.woprog_bvwo,mt.MemberTime_WorkOrder_ID) bvwo,
    MT.internal_project_id as internal_project_id,
    MemTy.membertype_name,
    MT.scope_id,
    MT.mileage_value,
    MT.mileage_unit,
    MT.prov_id,
    MT.payperiod_id
FROM
    (
        (
            membertime MT
            LEFT JOIN wocomment WOC
                ON MT.MemberTime_WoComment_ID = WOC.WoComment_ID
        )
        LEFT JOIN paytypehours HOUR
            ON MT.MemberTime_PayTypeHours_ID = hour.PayTypeHours_ID
        LEFT JOIN membertype MemTy ON MT.membertype_id = MemTy.membertype_id
    )
    LEFT JOIN member M
        ON MT.membertime_memberid = M.Member_ID
	LEFT JOIN woprog w on w.woprog_id=mt.MemberTime_WorkOrder_ID
WHERE MT.Date = @p1
    AND MT.membertime_memberid = @p0", userId, date).ToList();

                foreach (var item in list)
                {
                    item.ButtonVisible = GetButtonVisible(item);
                    if (item.WOType == "WO" && item.bvwo.StartsWith("0") && !AllowUnlinkedTimesheet)
                    {
                        item.WorkorderId = item.bvwo;
                    }

                    if (string.IsNullOrEmpty(item.membertype_name))
                    {
                        item.membertype_name = "";
                    }
                    if (item.scope_id != null && item.scope_id != 0)
                    {
                        item.scope_name = Toolbox.doSQL_string(conn, @"SELECT task FROM woprog_tasks where id = @v0", new object[] { item.scope_id });
                    }

                    if(item.payperiod_id == null ||  !item.payperiod_id.HasValue)
                    {
                        item.payperiod_id = 0;
                    }

                    item.can_be_transferred = this.CanTransfer(item);

                    // For non-transfered item, always set it to true, so no impact on control part.
                    item.can_delete_transfered_item = this.CanDeleteTransferedItem(item);
                    item.can_edit_transfered_item = this.CanEditTransferedItem(item);
                }


                return list;
            }
		}

        private bool CanTransfer(DTO.ViewModels.Page.TimeSheet.Timesheet item)
        {
            //
            // PM can do it only
            //
            var isCurrentUserPM = CurrentUser.AuthenticatedForPrivilege(15);
            var isCurrentUserBM = CurrentUser.AuthenticatedForPrivilege(16);
            if (!(isCurrentUserPM || isCurrentUserBM) )
            {
                // he/she cannot see the transfer button.
                return false;
            }

            var tts = new TimesheetTransferService(null, null);
            var okayToGo = tts.IsTransferable(item);

            return okayToGo;
        }


        private bool CanEditTransferedItem(DTO.ViewModels.Page.TimeSheet.Timesheet item)
        {
            //
            // For auto generated records by transfer, they can't be edited.
            // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1807/
            //
            var transferedItem = this.IsTransferedRecord(item);
            if (transferedItem)
            {
                return false;
            }

            //
            // Now the record is not a transferred records, so we return true, that means they can be edited anyway.
            //
			
            return true;
        }

        private bool CanDeleteTransferedItem(DTO.ViewModels.Page.TimeSheet.Timesheet item)
        {
            //
            // Only BM can delete the transfered Item.
            //

            //
            // First to check it is a transferred item or not.
            //
            var transferedItem = this.IsTransferedRecord(item);
            if (!transferedItem)
            {
                // This is not a transferred item, so return true... Non-transfer records should not constrain by this.
                return true;
            }

            //
            // Is it a positive one: 
            //
            if (item.Hours <= 0)
            {
                // You can only delete from positive one, due to the chain: positive points to negative, negtie points to original, orignal points to itself.
                return false;
            }

            //
            // Now it is a transfered item, Then check it is a BM or not.
            //
            var isCurrentUserBM = CurrentUser.AuthenticatedForPrivilege(16);
            if (isCurrentUserBM)
            {
                // he/she can see the delete button for transfer item.
                return true;
            }

            // he/she is not a bm, can't see delete button.
            return false;
        }

        private bool OpenupEditingDeletingOfMilageRecord(DTO.ViewModels.Page.TimeSheet.Timesheet item)
        {
            //
            // This is used to make https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1860/ implemented.
            //

            var isCurrentUserBM = CurrentUser.AuthenticatedForPrivilege(16);
            var isCurrentUserPM = CurrentUser.AuthenticatedForPrivilege(15);

            if ( !(isCurrentUserBM || isCurrentUserPM) )
            {
                //
                // No permission, so not able to edit/delete the mileage record.
                //
                return false;
            }


            //
            // Wo status check: Note the function name is a little wired but it really does the work.(_IsTransferable_IsInvoiced).
            //
            TimesheetTransferService tts = new TimesheetTransferService(null, null);
            var r = tts._IsTransferable_IsInvoiced(item);
            if (r)
            {
                // The status is 'Invoiced'.
                return false;
            }

            return true;
        }

        private bool GetButtonVisible(DTO.ViewModels.Page.TimeSheet.Timesheet item)
		{
            //
            // In order to make transfer action available, we open the MinDate to '2019-1-1'. So we need to have
            // a new constrain on edit/delete buttons for past payperiod records.
            // For any records from previous/past payperiod (not in current payperiod), 
            // they should not be edited or deleted.
            //
            // This change also makes it consistent with the information showing up for a past day: Payroll has already been approved for this dat, time can't be edited or deleted.
            // More info check https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1807
            //
            var inprevious = this.IsInPreviousPayPeriod(item);
            if (inprevious)
            {
                //
                // The record is in past Pay Period, but there is an exception for mileage record. // https://spcnesidev.visualstudio.com/Nesi/_workitems/edit/1860/
                //

                if (item.MemberTime_PayTypeHours_ID == 8)
                {
                    // If it is a mileage record, we will do more check in here.
                    var okay = OpenupEditingDeletingOfMilageRecord(item);
                    return okay;
                }

                // Now it is not a mileage record.
                return false;
            }

            //
            // Now the record is in current payperiod, then can follow the existing checks..
            //
			var WO = item.MemberTime_WOProg_id != null ? new NeWOProg((int) item.MemberTime_WOProg_id) : new NeWOProg();
			if (IsPayrollAdmin) return true;
			if (item.CustName == "Human Resources") return false;
			var isSuperVisor = CurrentUser.IsSupervisor(item.MemberId);
			var isPM = WO.intProjectManager == CurrentUser.Id;
			using(var itemEmployee = new Employee(item.MemberId))
				{ 
				var isManager = itemEmployee.BranchManagerId == CurrentUser.Id || itemEmployee.EmployeeProfile.payroll_handler == CurrentUser.Id || WO.woprog_id > 0 && isPM;
				if (WO.Status == OpsWOStatus.Invoiced) return false;
				if (item.MemberId != CurrentUser.Id && !isSuperVisor && !isManager) return false;				
				if (item.Date == DateTime.UtcNow.Date) return true;
				if (!GetEditPermission(item.MemberId, item.Date)) return false;				
				return (PayPeriod.EndDate > DateTime.Now.AddHours(-24) &&
						item.Date < PayPeriod.EndDate &&
						item.Date >= PayPeriod.StartDate) ||
					   (item.Date < CurrentPayPeriod.end_date &&
						item.Date >= CurrentPayPeriod.start_date && isManager) ||
					   (isSuperVisor && !CurrentPayPeriod.is_complete);
				}
		}

        private bool IsInPreviousPayPeriod(DTO.ViewModels.Page.TimeSheet.Timesheet item)
        {
            TimesheetTransferService tts = new TimesheetTransferService(null, null);
            var r = tts._IsTransferable_FromPreviousPayPeriod(item);
            return r;
        }

        private bool IsTransferedRecord(DTO.ViewModels.Page.TimeSheet.Timesheet item)
        {
            return item.OrigMemberTime_ID.HasValue;
        }

		private string GetEntityById(int membertimeId, out membertime mt, out wocomment comment)
		{
			comment = null;
			var entity = (_db.membertime.Where(x => x.MemberTime_ID == membertimeId)).FirstOrDefault();
			mt = entity;
			if (entity == null) return $"Timesheet id:{membertimeId} is not found.";
			comment = (from c in _db.wocomment
					   where c.WoComment_ID == entity.MemberTime_WoComment_ID
					   select c).FirstOrDefault();
			return comment == null ? $"Work order comment id:{entity.MemberTime_WoComment_ID} is not found" : "";
		}

        public membertime GetbyId(int id)
        {
            var entity = (_db.membertime.Where(x => x.MemberTime_ID == id)).FirstOrDefault();
            return entity;
        }

        private string SetInsertEntity(InsertTimeSheetBase model,
			out membertime entity, out wocomment comment)
		{
            var labourMasterId = 0;
            var user = new Employee(model.SelectedUserId);

            var jobTypeLine = new JobTypeLine();
            if (!model.allow_jobtype_selection)
            {
                labourMasterId = GetlabourMasterId(model.SelectedBusinessUnitId, user, model.PayTypeId == 0 ? 1 : model.PayTypeId);
                jobTypeLine.charegeout_id = labourMasterId;
                jobTypeLine.membertype_id = user.MemberType.membertype_id;
            }
            else
            {
                labourMasterId = GetlabourMasterIdForSelectedJobType(model.SelectedBusinessUnitId, user, model.PayTypeId == 0 ? 1 : model.PayTypeId, false, model.selectedJobType, model.selectedJobTypeName);
                jobTypeLine.charegeout_id = labourMasterId;
                jobTypeLine.membertype_id = model.selectedJobType;
            }

			var allowUnlink = GetAllowUnlinkTimesheet(model.SelectedBusinessUnitId);
			var costPrice = allowUnlink ? 0 : FindTrueLabourCost(model.SelectedUserId, model.PayTypeId, model.SelectedBusinessUnitId);
			comment = new wocomment
			{
				wocomment_member_id = model.SelectedUserId,
				comments = model.MemberTime_WoComment,
				Member_ID_Audit = CurrentUser.Id,
				Modified_Date = DateTime.Now,
				Personal = new byte[0],
				PrintComments = new byte[0],
				business_unit_id = model.SelectedBusinessUnitId,
				Created_Date = DateTime.Now
				
			};
			
			entity = new membertime
			{
				MemberTime_Premium = "false",
				membertime_customer_name = model.CustName,
				membertime_workorder_id = model.WoProg_Bvwo,
				MemberTime_CostPrice = costPrice,
				MemberTime_SellPrice = 0,
				Date = model.LocalDate,
				NumberOfHours = model.NumberOfHours,
				rating = model.Rating,
				MemberTime_MemberTypeHours_ID = 0,
				MemberTime_PayTypeHours_ID = model.PayTypeId,
				MemberTime_Customer_ID = model.CustId,
				MemberTime_Cust_No = model.CustNo,
				membertime_memberid = model.SelectedUserId,
				Member_ID_Audit = CurrentUser.Id,
				wo_percent_complete = model.PercentComplete,
				child_shoptime = 0,
				membertype_id = model.PayTypeId == OpsPayType.Mileage || model.PayTypeId == OpsPayType.Travel
									? user.MemberType.membertype_id 
									: jobTypeLine.membertype_id, // job type ...
				membertype_chargeout_id = jobTypeLine.charegeout_id,  // job type ...
                MemberTime_Mileage = "false",
				MemberTime_SRED = "false",
				MemberTime_Warranty = "false",
				MemberTime_Tax1 = 0,
				MemberTime_Tax2 = 0,
				MemberTime_Tax3 = 0,
				MemberTime_Tax4 = 0,
				MemberTime_Child_WOProg_id = 0,
				MemberTime_Child_CompanyID = 0,
				MemberTime_WoComment_Child_ID = 0,
				Created_Date = DateTime.Now,
				Member_ID_Create = user.Id,
				business_unit_id = model.SelectedBusinessUnitId
			};
			return "";
		}

		private double FindTrueLabourCost(int userId, int payTypeId, int buId)
		{
			return _db.Database.SqlQuery<double>(@"SELECT get_wage_cost(@p0,@p1,@p2)", userId, payTypeId, buId).FirstOrDefault();
		}

		private string SaveTimeSheet(Employee user, membertime mt, wocomment comment, string WoProg_Bvwo, TelemRecord record = null, int tsLitePayType = 0, InsertTimeSheetBase timesheetRecord = null)
		{

			if (!GetEditPermission(mt.membertime_memberid, mt.Date)) return "Payroll has already been approved for this date, time can't be edited or deleted by non-payroll staff.";
			//			var allowUnlink = GetAllowUnlinkTimesheet(mt.business_unit_id.GetValueOrDefault());
			//			if (allowUnlink)
			//			{
			//				try
			//				{
			//					_db.wocomment.AddOrUpdate(comment);
			//					_db.SaveChanges();
			//					mt.MemberTime_WoComment_ID = Convert.ToInt32(comment.WoComment_ID);
			//					_db.membertime.AddOrUpdate(mt);
			//					_db.SaveChanges();
			//				}
			//				catch (Exception e)
			//				{
			//					return "Saving information was failed.";
			//				}
			//				return "Time entry has been saved successfully.";
			//			}

			if (record == null) { record = new TelemRecord(); }

			using (var conn = Toolbox.connect())
			{
				// phoneComments1.save_grid();

				// SendPMEmail();
				var wo = new NeWOProg();
				var wo_parent = new NeWOProg();
				var woId = 0;
				var childWoId = 0;
				//If it is a work Order there must be a comment
				if (Toolbox.ReturnZeroIfNull_int(mt.membertime_workorder_id) > 0)
				{
					woId = Convert.ToInt32(mt.membertime_workorder_id);
					wo = new NeWOProg(woId);
				}
				if (Toolbox.ReturnZeroIfNull_int(mt.MemberTime_Child_WorkOrder_ID) > 0)
				{
					childWoId = Convert.ToInt32(mt.MemberTime_Child_WorkOrder_ID);
					wo_parent = new NeWOProg(childWoId);
				}

				#region Variable Definition / Partial error checking

				var is_child_wo = childWoId > 0;
				var loaded_user = new NeMember(Convert.ToInt32(mt.membertime_memberid));
				var MemberCompany = new NeBusinessUnit(Convert.ToInt32(mt.business_unit_id));
				if (mt.WOType == "WO" && !MemberCompany.allow_unlinked_timesheet)
				{
					try
					{
						var test = new NeWOProg(woId);
					}
					catch (Exception ee)
					{
						Toolbox.do_catch_error(ee, 711);
						return ("This work order was not cut in NESI and cannot have time entered on it");
					}
				}

				var labour_master_id = 0;
				try
				{
                  
                        labour_master_id = MemberCompany.allow_unlinked_timesheet ? 0 :
                            Toolbox.doSQL_int(conn, @"SELECT id from membertype_chargeout WHERE membertype_id = @v0 
			AND business_unit_id = @v1  AND paytype_id = @v2 ", new object[] { mt.membertype_id, mt.business_unit_id, mt.MemberTime_PayTypeHours_ID }); // change for job type feature.
                    
                }
				catch
				{
					if (loaded_user.id > 0)
					{
						var bm_email = loaded_user.business_unit.branch_manager.NEEmail;
						var em = new NeEMail
						{
							Subject = "NOTICE: Blank charge out for the member type '" + loaded_user.membertype.name + "'",
							Body =
								$"{loaded_user.FullName} just tried to enter time, but the charge out for their member type is zero... please address this ASAP.",
							To =
								bm_email == ""
									? new NeMember(Convert.ToInt32(loaded_user.business_unit.branch_manager.reports_to)).NEEmail
									: bm_email,
							//Bcc = "it@" + Toolbox.app_setting("DomainForEmail"),
							isHTML = false,
							From = "it@" + Toolbox.app_setting("DomainForEmail")
                        };
						em.Send();
					}
					return $@"Your member type is not set up with a charge out rate in the selected business unit. 
					Until this is corrected, you will not be able to enter time on work orders in this business unit.";
				}

				//check aprent WO also
				if (wo.parent_woprog_id != 0)
				{
					var parentBU = new NeBusinessUnit(new NeWOProg(wo.parent_woprog_id).business_unit_id);


					try
					{
						var anothertest = MemberCompany.allow_unlinked_timesheet
							? 0
							: Toolbox.doSQL_int(conn, @"SELECT id from membertype_chargeout WHERE membertype_id = @v0 
			AND business_unit_id = @v1  AND paytype_id = @v2 ",
								new object[] { mt.membertype_id, parentBU.id, mt.MemberTime_PayTypeHours_ID }); //  change for job type feature.
					}
					catch
					{
						if (loaded_user.id > 0)
						{
							var bm_email = parentBU.branch_manager.NEEmail;
							var em = new NeEMail
							{
								Subject = "NOTICE: Blank charge out for the member type '" + loaded_user.membertype.name + "'",
								Body =
									$"{loaded_user.FullName} just tried to enter time, but the charge out for their member type is zero... please address this ASAP.",
								To =
									bm_email == ""
										? new NeMember(Convert.ToInt32(loaded_user.business_unit.branch_manager.reports_to)).NEEmail
										: bm_email,
								//Bcc = "it@" + Toolbox.app_setting("DomainForEmail"),
								isHTML = false,
								From = "it@" + Toolbox.app_setting("DomainForEmail")
                            };
							em.Send();
						}
						return $@"Your member type is not set up with a charge out rate in the parent business unit. 
					Until this is corrected, you will not be able to enter time on work orders in this business unit.";
					}
				}

				#endregion Variable Definition / Partial error checking

				var _currentUser = new NeMember(CurrentUser.Id);
				if (MemberCompany.allow_unlinked_timesheet || labour_master_id != 0)
				{
					var t = mt.WOType.ToLower();
					var entrytype = t == "wo" ? 1 : t == "quote" ? 2 : t == "telem" ? 3 : t == "shop" ? 4 : 0;
					var parent_customer = wo_parent.woprog_id > 0 ? new NECustomer((int)wo_parent.WOProg_Customer_ID) : new NECustomer();
					var paytypeid = Convert.ToInt32(mt.MemberTime_PayTypeHours_ID);
					try
					{ 
                        var nemembertime = new NeMemberTime.time_entry_vars()
                        {
                            advancement = "",
                            do_advancement = false,
                            do_uncertainty = false,
                            bizdev_calls = Toolbox.ReturnBlankIfNull_string(record.bizdev_calls).ToString(),
                            bizdev_faxes = Toolbox.ReturnBlankIfNull_string(record.bizdev_faxes).ToString(),
                            bizdev_emails = Toolbox.ReturnBlankIfNull_string(record.bizdev_emails).ToString(),
                            bizdev_mailers = Toolbox.ReturnBlankIfNull_string(record.bizdev_mailers).ToString(),
                            bizdev_meetings = Toolbox.ReturnBlankIfNull_string(record.bizdev_meetings).ToString(),
                            bizdev_quoteopps = Toolbox.ReturnBlankIfNull_string(record.bizdev_quoteopps).ToString(),
                            comment = comment.comments,
                            customer_name = mt.membertime_customer_name,
                            customer_number = mt.MemberTime_Cust_No,
                            date = Toolbox.MySQL_shortdt(mt.Date),
                            entering_user = _currentUser,
                            entrytype = entrytype,
                            hours = Convert.ToDouble(mt.NumberOfHours),
                            is_child_wo = is_child_wo,
                            is_shop_time = mt.membertime_shop_type_id > 0,
                            loaded_user = loaded_user,
                            master_id = labour_master_id,
                            parent_business_unit_id = wo_parent.business_unit_id,
                            parent_customer_name = wo_parent.CustomerName,
                            parent_customer_number = parent_customer.Customer_Number ?? "",
                            parent_wo = wo_parent,
                            wo = wo,
                            //jobTags = JobTags.Tokens,
                            parent_woprog_bvwo = MemberCompany.allow_unlinked_timesheet ? "" : wo_parent.OrderNumber,
                            paytype_id = paytypeid,
                            percent_done = Convert.ToDouble(mt.wo_percent_complete),
                            selected_comment_id = Toolbox.ReturnZeroIfNull_int(mt.MemberTime_WoComment_ID.ToString()),
                            selected_comment_text = comment.comments,
                            selected_business_unit_id = Convert.ToInt32(mt.business_unit_id),
                            todays_date = (t == "wo" || t == "quote") ? Toolbox.MySQL_shortdt(mt.Date) : DateTime.Today.ToString("yyyy-MM-dd"),
                            uncertainty = "",
                            woprog_bvwo = WoProg_Bvwo,
                            cost_price = MemberCompany.allow_unlinked_timesheet 
											? 0 
											: new NeWODetailCurrent().FindTrueLabourCost(loaded_user.id, paytypeid, Convert.ToInt32(mt.business_unit_id)),
                            rating = Convert.ToInt16(mt.rating),
                            tsLitePayType = tsLitePayType,
                            internal_project_id = Toolbox.ReturnZeroIfNull_int(mt.internal_project_id),
                            membertype_id = paytypeid == OpsPayType.Mileage || paytypeid == OpsPayType.Travel
												? loaded_user.MemberTypeID 
												: mt.membertype_id.Value, // member type id info
							scope_id =mt.scope_id,
                            prov_id = mt.Prov_Id,
                            mileage_value = mt.mileage_value,
                            mileage_unit = mt.mileage_unit,
							is_prevailing_wage = mt.is_prevailing_wage

                        };

						//NeMemberTime.time_entry_create(ref nemembertime);

                        //
                        // Transfer information checking.
                        //
                        if (timesheetRecord != null && timesheetRecord.IsTransfer == true)
                        {
                            // carry on these info for updating.
                            nemembertime.originalChargeoutId = timesheetRecord.originalChargeoutId;
                            nemembertime.IsTransfer = timesheetRecord.IsTransfer;
                            nemembertime.transferInfo = timesheetRecord.transferInfo;
                            nemembertime.OriginalCost = timesheetRecord.OriginalCost;
                            nemembertime.OriginalPrice = timesheetRecord.OriginalPrice;
                            nemembertime.originalMemberTypeid = timesheetRecord.originalMemberTypeid;
                            nemembertime.orginalChargeoutIdForParentWorkOrderLaborLIine = timesheetRecord.orginalChargeoutIdForParentWorkOrderLaborLIine;
                            nemembertime.orginal_branch_can_see_jobtype = timesheetRecord.orginal_branch_can_see_jobtype;

                            if (nemembertime.originalChargeoutId == nemembertime.master_id)
                            {
                                //
                                // do the same job type work, no need to change the master id, member type id.
                                //

                                if (nemembertime.transferInfo == "N")
                                {
                                    // For negative record
                                }

                                if (nemembertime.transferInfo == "P")
                                {
                                    // For positive record: no changes.
                                }
                            }
                            else
                            {
                                //
                                // if do different job, no changes.
                                //

                                if (nemembertime.transferInfo == "N")
                                {
                                    // For negative record
                                    nemembertime.master_id = nemembertime.originalChargeoutId;
                                    nemembertime.membertype_id = nemembertime.originalMemberTypeid;
                                }

                                if (nemembertime.transferInfo == "P")
                                {
                                    // For positive record: no changes.

                                    if (timesheetRecord.orginal_branch_can_see_jobtype)
                                    {
                                        // The branch can see the jobtype selector, so use what ever from angular side.
                                    }
                                    else
                                    {
                                        nemembertime.master_id = nemembertime.originalChargeoutId;
                                        nemembertime.membertype_id = nemembertime.originalMemberTypeid;
                                    }
                                }
                            }
                        }
                        //
                        // End of transfer
                        //

                        try
                        {
                            NeMemberTime.time_entry_create(ref nemembertime);
                        }
                        catch (Exception)
                        {
                            return "There was an issue saving your changes - Error TS_TX1";
						}

                        if (timesheetRecord != null && timesheetRecord.IsTransfer == true)
                        {
                            timesheetRecord.IdForTransfer = nemembertime.IdForTransfer;
                        }

					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						Toolbox.do_errorLog(e);
						return "Saving time entry failed.";
					}
				}
			}

			return "Time entry has been saved successfully.";
		}


		//private void AddRecord(Employee user, membertime mt, wocomment comment)
		//{
		//	// add Record
		//	var record = _db.wocomment.FirstOrDefault(x =>
		//		x.WorkOrder_ID == mt.MemberTime_WOProg_id.ToString() &&
		//		x.business_unit_id == mt.business_unit_id &&
		//		x.wocomment_member_id == 0
		//	);
		//	var formatComment = $"{DateTime.Now}: {user.FullName} : {mt.NumberOfHours} Hours: {comment.comments}";
		//	if (record != null)
		//	{
		//		record.comments = record.comments + "\n" + formatComment;

		//	}
		//	else
		//	{
		//		record = new wocomment
		//		{
		//			WorkOrder_ID = mt.membertime_workorder_id,
		//			wocomment_member_id = 0,
		//			comments = formatComment,
		//			Personal = new byte[0],
		//			PrintComments = new byte[0],
		//			Member_ID_Audit = CurrentUser.Id,
		//			Created_Date = DateTime.Now,
		//			Modified_Date = DateTime.Now,
		//			business_unit_id = mt.business_unit_id,
		//			woprog_id = Convert.ToInt32(mt.membertime_workorder_id)
		//		};
		//	}
		//	_db.wocomment.AddOrUpdate(record);
		//	try
		//	{
		//		_db.SaveChanges();

		//	}
		//	catch (Exception e)
		//	{
		//		Console.WriteLine(e);
		//	}
		//}

		private string UpdateTimeSheet(Employee user, membertime mt, wocomment comment, TelemRecord record = null)
		{
			// check edit permission.
			if (!GetEditPermission(mt.membertime_memberid, mt.Date)) return "Payroll has already been approved for this date, time can't be edited or deleted by non-payroll staff.";

			if (record == null) record = new TelemRecord();
			try
			{

				var OldMemberTimeDet1 = new NeMemberTime(Convert.ToInt32(mt.MemberTime_ID));
				var oldhours = Convert.ToDouble(OldMemberTimeDet1.NumberOfHours);
				if (oldhours > Convert.ToDouble(mt.NumberOfHours))
				{
					#region Since the person is trying to reduce the number of hours, run a check against their banked pay
					var PayType = Toolbox.doSQL_string(@"SELECT paytype_id FROM member WHERE member_id =@v0 ", new object[] { mt.membertime_memberid });
					if (PayType != "2" && PayType != "3")
					{
						#region Banked Pay Deposit Check
						//Check Hours Banked for this pay period.
						var datecheck = OldMemberTimeDet1.Date;
						var datefix = Convert.ToDateTime(datecheck);
						datecheck = datefix.ToString("yyyy-MM-dd");
						//Get PayPeriod Information
						var payperiodid = Toolbox.doSQL_int(@"SELECT PayperiodID FROM payperiods  WHERE (StartDate <@v0 AND EndDate >@v1  ) OR startdate =@v2  limit 1 ", new object[] { datecheck, datecheck, datecheck });
						var payperiodinfo = new NePayPeriod(Convert.ToInt32(payperiodid));
						var hourstouse = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(hours), 0) AS deposited FROM bankedpay_ledger WHERE type='D' AND member_id = @v0  AND payperiod_id = @v1 ", new object[] { mt.membertime_memberid, payperiodid });
						var hoursintimesheet = Toolbox.doSQL_double(@"SELECT SUM(NumberofHours) FROM membertime 
WHERE MemberTime_PayTypeHours_ID = @v3  
AND Membertime_memberid = @v0  
AND DATE >= @v1  AND DATE <= @v2 ",
new object[] { mt.membertime_memberid, payperiodinfo.StartDate, payperiodinfo.Enddate, mt.MemberTime_PayTypeHours_ID });
						var hoursdifference = hoursintimesheet - oldhours + Convert.ToDouble(mt.NumberOfHours);
						if (hoursdifference < hourstouse)
						{
							return ("You cannot Reduce the hours due to banked time. Please remove banked time and then edit this time entry.");
						}
						#endregion Banked Pay Deposit Check
					}
					#endregion Since the person...
				}


				#region validity check / partial variable declaration
				var loaded_user = new NeMember(Convert.ToInt32(mt.membertime_memberid));
				var MemberCompany = new NeBusinessUnit(Convert.ToInt32(mt.business_unit_id));
				var is_child_wo = false;
				var wo_parent = new NeWOProg();
				//Check if MemberBVNumber Exists in Inventory Table

				var wo_obj = new NeWOProg();
				if (!loaded_user.business_unit.allow_unlinked_timesheet && int.TryParse(mt.membertime_workorder_id, out var woid))
				{
					if (!string.IsNullOrEmpty(mt.membertime_workorder_id))
					{
						wo_obj = new NeWOProg(woid);
					}

					if (wo_obj.Status == OpsWOStatus.WaitingToBeInvoiced || wo_obj.Status == "Invoiced")
					{
						throw new Exception("You Cannot Edit Time On This Work Order As it has been Invoiced");
					}
					if (wo_obj.woprog_hold == 1)
					{
						throw new Exception("You Cannot Edit Time On This Work Order As it is On Hold");
					}
				}
				if (!string.IsNullOrEmpty(mt.MemberTime_Child_WorkOrder_ID))
				{
					is_child_wo = true;
					var customer_id = mt.MemberTime_Customer_ID;
					if (customer_id == 0) throw new Exception("Invalid customer");
					var this_customer = new NECustomer(Convert.ToInt32(customer_id));
					var isnecompany = this_customer.IsNEcompany ? this_customer.NEbusiness_unit_id : 0;

					var id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(woprog_id),0) FROM woprog WHERE Woprog_BVWO = @v0 ", new object[] { mt.MemberTime_Child_WorkOrder_ID });
					if (id == 0) throw new Exception("Unknown work order");
					wo_parent = new NeWOProg(id);

					if (wo_parent.Status == OpsWOStatus.WaitingToBeInvoiced || wo_parent.Status == "Invoiced")
					{
						throw new Exception("You cannot edit time on this work order as it has been invoiced");
					}
					if (wo_parent.Status == "Hold")
					{
						throw new Exception("You cannot edit time on this work order as it is on hold");
					}
				}
				var labour_master_id = 0;
				try
				{
					labour_master_id = MemberCompany.allow_unlinked_timesheet ? 0 : Toolbox.doSQL_int(@"SELECT id from membertype_chargeout WHERE membertype_id = @v0  AND business_unit_id = @v1  AND paytype_id = @v2 ", new object[] { mt.membertype_id, mt.business_unit_id, mt.MemberTime_PayTypeHours_ID });
				}
				catch
				{
					if (loaded_user.id > 0)
					{
						var bm_email = loaded_user.business_unit.branch_manager.NEEmail;
						var em = new NeEMail
						{
							Subject = "NOTICE: Blank charge out for the member type '" + loaded_user.membertype.name + "'",
							Body =
								$"{loaded_user.FullName} just tried to enter time, but the charge out for their member type is zero... please address this ASAP.",
							To =
								bm_email == ""
									? new NeMember(Convert.ToInt32(loaded_user.business_unit.branch_manager.reports_to)).NEEmail
									: bm_email,
							//Bcc = "it@" + Toolbox.app_setting("DomainForEmail"),
							isHTML = false,
							From = "it@" + Toolbox.app_setting("DomainForEmail")
						};
						em.Send();
					}
					return $@"Your member type is not set up with a charge out rate in the selected business unit. 
					Until this is corrected, you will not be able to enter time on work orders in this business unit.";
				}
				#endregion validity check / partial variable declaration

				var t = mt.WOType.ToLower();
				var entrytype = t == "wo" ? 1 : t == "quote" ? 2 : t == "telem" ? 3 : t == "shop" ? 4 : 0;
				var parent_customer = wo_parent.woprog_id > 0 ? new NECustomer((int)wo_parent.WOProg_Customer_ID) : new NECustomer();
				var _currentUser = new NeMember(base.UserId);
				var paytypeid = Convert.ToInt32(mt.MemberTime_PayTypeHours_ID);

				try
				{
					NeMemberTime.time_entry_edit(new NeMemberTime.time_entry_vars()
					{

						membertime_id = Convert.ToInt32(mt.MemberTime_ID),
						old_hours = OldMemberTimeDet1.NumberOfHours,
						old_comment_id = OldMemberTimeDet1.WoCommentID,
						parent_woprog_id = wo_parent.woprog_id,
						wo = wo_obj,
						woprog_id = wo_obj.woprog_id,
						cost_price = OldMemberTimeDet1.MemberTime_CostPrice,


						advancement = "",
						do_advancement = false,
						do_uncertainty = false,
						bizdev_calls = record.bizdev_calls.ToString(),
						bizdev_faxes = record.bizdev_faxes.ToString(),
						bizdev_emails = record.bizdev_emails.ToString(),
						bizdev_mailers = record.bizdev_mailers.ToString(),
						bizdev_meetings = record.bizdev_meetings.ToString(),
						bizdev_quoteopps = record.bizdev_quoteopps.ToString(),
						comment = comment.comments,
						customer_name = mt.membertime_customer_name,
						customer_number = mt.MemberTime_Cust_No,
						date = Toolbox.MySQL_shortdt(mt.Date),
						entering_user = _currentUser,
						entrytype = entrytype,
						hours = Convert.ToDouble(mt.NumberOfHours),
						is_child_wo = is_child_wo,
						is_shop_time = mt.membertime_shop_type_id > 0,
						loaded_user = loaded_user,
						master_id = labour_master_id,
						parent_business_unit_id = wo_parent.business_unit_id,
						parent_customer_name = wo_parent.CustomerName,
						parent_customer_number = parent_customer.Customer_Number ?? "",
						parent_wo = wo_parent,
						//jobTags = JobTags.Tokens,
						parent_woprog_bvwo = MemberCompany.allow_unlinked_timesheet ? "" : wo_parent.OrderNumber,
						paytype_id = paytypeid,
						percent_done = Convert.ToDouble(mt.wo_percent_complete),
						selected_comment_id = Toolbox.ReturnZeroIfNull_int(mt.MemberTime_WoComment_ID.ToString()),
						selected_comment_text = comment.comments,
						selected_business_unit_id = Convert.ToInt32(mt.business_unit_id),
						todays_date = DateTime.Today.ToString("yyyy-MM-dd"),
						uncertainty = "",
						woprog_bvwo = mt.membertime_workorder_id,
						rating = Convert.ToInt16(mt.rating),
						internal_project_id = mt.internal_project_id,
						membertype_id = paytypeid == OpsPayType.Mileage || paytypeid == OpsPayType.Travel
							? loaded_user.MemberTypeID
							: mt.membertype_id.Value, // even the update will not change this, we still pass the correct one.
						scope_id = mt.scope_id,
						prov_id = mt.Prov_Id,
						mileage_value = mt.mileage_value,
						oldmileage_value = OldMemberTimeDet1.old_mileage_value,

						mileage_unit = mt.mileage_unit,
						is_prevailing_wage = mt.is_prevailing_wage
					}) ;

				}
				catch (Exception)
				{
					return "There was an issue saving your changes - Error TS_TX1";
				}
			}
			catch (Exception)
			{
				//TODO: implement log error process.

				return "Updating information failed.";
			}

			return "Information has been updated successfully.";
		}

		public int GetlabourMasterId(int buId, Employee user, int payTypeHour, bool sendEmail = false)
		{
			// if (AllowUnlinkedTimesheet) return 0;
            
			var obj = _db.membertype_chargeout.FirstOrDefault(x =>
				x.membertype_id == user.EmployeeProfile.member_membertype_id &&
				x.business_unit_id == buId &&
				x.paytype_id == payTypeHour);
			if (obj != null) return obj.id;
			if (sendEmail)
			{
				var em = new NeEMail
				{
					Subject = "NOTICE: Blank charge out for the member type '" + user.MemberType.membertype_name + "'",
					Body =
						$"{user.FullName} just tried to enter time, but the charge out for their member type is zero... please address this ASAP.",
					To = user.BranchManager.member_neemail,
					//Bcc = Common.Shared.Configuration.EmailIt,
					isHTML = false,
					From = "admin@" + Toolbox.app_setting("DomainForEmail")
                };
				em.Send();
			}
			return 0;
		}

        public int GetlabourMasterIdForSelectedJobType(int buId, Employee user, int payTypeHour, bool sendEmail, int jobTypeId, string jobTypeName)
        {
            // if (AllowUnlinkedTimesheet) return 0;
            var obj = _db.membertype_chargeout.FirstOrDefault(x =>
                x.membertype_id == jobTypeId &&
                x.business_unit_id == buId &&
                x.paytype_id == payTypeHour);
            if (obj != null) return obj.id;
            if (sendEmail)
            {
                var em = new NeEMail
                {
                    Subject = "NOTICE: Blank charge out for the member type '" + jobTypeName + "'",
                    Body =
                        $"{user.FullName} just tried to enter time, but the charge out for their member type is zero... please address this ASAP.",
                    To = user.BranchManager.member_neemail,
                   // Bcc = Common.Shared.Configuration.EmailIt,
                    isHTML = false,
                    From = "admin@" + Toolbox.app_setting("DomainForEmail")
                };
                em.Send();
            }
            return 0;
        }


        public bool GetAllowUnlinkTimesheet(int buId)
		{
			return Global.BusinessUnit.GetValue(buId).allow_unlinked_timesheet.GetValueOrDefault();
		}

        public DataTable GetProvinceList()
        {
            return bllToolbox.doSQL_dt(@"SELECT prov_desc label,prov_id value, country_code FROM prov");
        }

        public int GetDefaultProvince(int Buid)
        {
            return bllToolbox.doSQL_int(@"SELECT prov_id FROM prov a JOIN business_unit b WHERE a.`prov_abbv` = IF(b.Country='CDN',b.prov,b.state) AND b.`ID` =" + Buid);
        }

        public string GetCountry(int Buid)
        {
            return bllToolbox.doSQL_string(@"SELECT Country FROM business_unit WHERE ID="+ Buid);

        }

        //
        // Transfer - delete
        //
        public TransferDeleteInfor IsDeleteStartFromATransferedRecord(int id, int selectedUserId)
        {
            //
            // We only allow to delete from the positive records, so we can track the pointers chain to delete / update others records.
            //

            // SELECT OrigMemberTime_ID, NumberOfHours, MemberTime_ID  FROM membertime m WHERE m.MemberTime_ID = @v0;

            var transferDeleteInfor = new TransferDeleteInfor();
            transferDeleteInfor.DeleteFromTransferItem = false;

            //
            // Check is it a positive one?
            //
            var positive = this.GetbyId(id);
            if (positive == null || 
                (!positive.OrigMemberTime_ID.HasValue) ||
                (positive.NumberOfHours <= 0 ) )
            {
                return transferDeleteInfor;
            }

            //
            // Get the Negative one
            //
            var negative = this.GetbyId(positive.OrigMemberTime_ID.Value);
            if (negative == null ||
                (!negative.OrigMemberTime_ID.HasValue) ||
                (negative.NumberOfHours >= 0))
            {
                return transferDeleteInfor;
            }

            //
            // Get the Original one
            //
            // NO need to check original one

            //
            // Now the chain is okay
            //
            transferDeleteInfor.positiveMember_id = positive.MemberTime_ID;
            transferDeleteInfor.negatieMember_id = negative.MemberTime_ID;
            transferDeleteInfor.orignalMember_id = negative.OrigMemberTime_ID.Value;

            transferDeleteInfor.DeleteFromTransferItem = true;

            return transferDeleteInfor;
        }


        public string DeleteFromTransferredRecord(int id, int selectedUserId, TransferDeleteInfor info)
        {
            var okay = false;
            if (!info.DeleteFromTransferItem ||
                info.positiveMember_id <= 0 ||
                info.negatieMember_id <=0 ||
                info.orignalMember_id <=0 )
            {
                return "Delete a transferred item cannot be finished due to not set the info correctly";
            }

            //
            // Delete positive first - Step 1
            //
            var positiveResult = this.Delete(info.positiveMember_id, selectedUserId);
            if (!(positiveResult.IndexOf("uccessfully", StringComparison.Ordinal) > -1)) // for shop :Entry has been deleted successfully.
            {
                return positiveResult + " (Positive)";
            }

            //
            // Delete negative second - Step 2
            //
            var negativeResult = this.Delete(info.negatieMember_id, selectedUserId);
            if (!(negativeResult.IndexOf("Successfully", StringComparison.Ordinal) > -1))
            {
                // This is hard part if we are here....
                return negativeResult + " (Negative)" ;
            }

            //
            // clear the original member id for orginal one - Step 3
            //

            // keep ts unchanged.
            var sql = @"
UPDATE
  membertime
SET
  ts = ts,
  OrigMemberTime_ID = @v0
WHERE MemberTime_ID = @v1";

            //
            // Orignal point to itself
            //
            var params4Sql = new object[]
            {
                null, 
                info.orignalMember_id
            };

            try
            {
                // Because we don't point to self, so no need to do this.
                // Toolbox.doSQL_void(sql, params4Sql);
            }
            catch (Exception ex)
            {
                return "Delete a transferred item cannot be finished due to not able to clear out the orignal member id from original record.";
            }

            return "Successfully deleted time entry";
        }
        //
        // End of transfer deletion.
        //

        public string Delete(int id, int selectedUserId)
		{

			var has_child = false;
			using (var conn = Toolbox.connect())
			{
				using (var transaction = conn.BeginTransaction())
				{
					try
					{
						var MemberTimeDet = new NeMemberTime(Convert.ToInt32(id));
						var n_hours = MemberTimeDet.NumberOfHours;
						var paytypehoursid = MemberTimeDet.PayTypeHoursID.ToString();
						var wo_type = MemberTimeDet.WOType.ToUpper();
						var currentUser = new NeMember(base.CurrentUser.Id);
						//Check Work Order Status if not quote or Telem
						if ((wo_type != "QUOTE" && wo_type != "TELEM" && wo_type != "SHOP") && (!currentUser.business_unit.allow_unlinked_timesheet))
						{
							var statuscheck = new NeWOProg(MemberTimeDet.membertime_woprog_id);
							#region Check if work order is in the invoiced, or waiting to be invoiced status
							if (statuscheck.Status == "Invoiced" || statuscheck.Status == OpsWOStatus.WaitingToBeInvoiced)
							{
								transaction.Rollback();
								return ("You Can Not Delete This Entry It is Already Invoiced");
							}
							#endregion
							#region Check if work order is on hold
							if (statuscheck.woprog_hold == 1)
							{
								transaction.Rollback();
								return ("You Can Not Delete This Entry The Work Order is on Hold");
							}
							#endregion

						}
						if (n_hours > 0)
						{
							#region Check Banked Time against the hours being reduced
							var PayType = Toolbox.doSQL_string(conn, @"SELECT paytype_id FROM member  WHERE member_id =@v0", new object[] { selectedUserId });
							if (PayType != "2" && PayType != "3")
							{
								//Check Hours Banked for this pay period.
								var datecheck = MemberTimeDet.Date;
								var datefix = Convert.ToDateTime(datecheck);
								datecheck = datefix.ToString("yyyy-MM-dd");
								//Get PayPeriod Information
								var timeEntriesPayperiodId = Toolbox.doSQL_int(conn, @"SELECT PayperiodID FROM payperiods  WHERE StartDate <=@v0 AND EndDate >@v1  limit 1 ", new object[] { datecheck, datecheck });
								var payperiodinfo = new Ne2PayPeriod(Convert.ToInt32(timeEntriesPayperiodId));
								var currentPayperiod = Toolbox.doSQL_int(conn, @"SELECT payperiodid FROM payperiods WHERE completed = 0  limit 1 ", null);
								if (timeEntriesPayperiodId < currentPayperiod && !currentUser.business_unit.is_backoffice)
								{
									transaction.Rollback();
									return ("Only a member of NESI can delete an entry from a previous pay period.");
								}
								var hourstouse = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(hours), 0) AS deposited FROM bankedpay_ledger WHERE type='D' AND member_id = @v0  AND payperiod_id = @v1 ", new object[] { selectedUserId, timeEntriesPayperiodId });
								if (paytypehoursid != "1")
								{
									hourstouse = 0;
								}
								var hoursintimesheet = Toolbox.doSQL_double(conn, @"SELECT IFNULL(SUM(NumberofHours), 0) FROM membertime WHERE MemberTime_PayTypeHours_ID = @v3  AND membertime_memberid = @v0  AND DATE >= @v1  AND DATE <= @v2 ", new object[] { selectedUserId, payperiodinfo.StartDate, payperiodinfo.Enddate, paytypehoursid });
								var hoursdifference = hoursintimesheet - n_hours;
								if (hoursdifference < hourstouse && hourstouse != 0)
								{
									transaction.Rollback();
									return
										$"You cannot delete this entry, the hours have been allocated against a banked pay deposit... Please remove the banked pay request, first.";
								}
							}
							#endregion
						}
						#region Check if this work order has a child
						if (MemberTimeDet.MemberTime_ChildCompanyID != 0 && !MemberTimeDet.child_shoptime)
						{
							has_child = true;
							var childstatuscheck = new NeWOProg(MemberTimeDet.child_woprog_id);
							#region Check if child work order is invoiced or waiting to be invoiced.
							if (childstatuscheck.Status == "Invoiced" || childstatuscheck.Status == OpsWOStatus.WaitingToBeInvoiced)
							{
								transaction.Rollback();
								return ("You Can Not Delete This Child Entry It is Alreay Invoiced");
							}
							#endregion
							#region Check if child work order is on hold
							if (childstatuscheck.Status == "Hold")
							{
								transaction.Rollback();
								return ("You Cannot Delete This Entry the Child Work Order is on Hold");
							}
							#endregion
						}
						#endregion
						#region Delete Quote Entry
						if (MemberTimeDet.WOType == "Quote")
						{
							MemberTimeDet.MemberIDAudit = currentUser.id;
							MemberTimeDet.DeleteMemberTimeDet(conn, transaction);
							transaction.Commit();
							return "Entry has been deleted successfully.";
						}
						#endregion
						#region Delete Telemarketing Entry
						else if (MemberTimeDet.WOType.ToString().ToUpper() == "TELEM")
						{
							MemberTimeDet.MemberIDAudit = currentUser.id;
							MemberTimeDet.DeleteMemberTimeDet(conn, transaction);
							transaction.Commit();
							return "Entry has been deleted successfully.";
						}
						#endregion
						#region Delete Shop Entry
						else if (MemberTimeDet.WOType.ToString().ToUpper() == "SHOP")
						{
							MemberTimeDet.MemberIDAudit = currentUser.id;
							MemberTimeDet.DeleteMemberTimeDet(conn, transaction);
							transaction.Commit();
							return "Entry has been deleted successfully.";
						}
						#endregion
						#region Delete Workorder Entry
						else
						{
							var this_user = new NeMember(MemberTimeDet.member_id);
							MemberTimeDet.MemberIDAudit = currentUser.id;

							// Get a copy of timesheet record before deleted.
							var previousRecord = new NeMemberTime(MemberTimeDet.MemberTimeID);

							MemberTimeDet.DeleteMemberTimeDet(conn, transaction);
							if ((MemberTimeDet.MemberTime_Mileage == "false") && (!currentUser.business_unit.allow_unlinked_timesheet))
							{
								#region Update wo_detail_current if work order is not a quoted job
								if (MemberTimeDet.WOType != "Quote")
								{
									var temp_wo = new NeWOProg(MemberTimeDet.membertime_woprog_id);
									var temp_chargeout_id = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(id), 0) FROM membertype_chargeout where membertype_id = @v0  and paytype_id = @v1  and business_unit_id = @v2 ", new object[] { MemberTimeDet.membertype_id, MemberTimeDet.MemberTimePayTypeHoursID, temp_wo.business_unit_id });
									var wo_line = new NeWODetailCurrent(MemberTimeDet.member_id, MemberTimeDet.MemberTimePayTypeHoursID,
										MemberTimeDet.membertime_woprog_id, temp_chargeout_id)
									{
										qty_ordered = n_hours * -1,
										qty_committed = n_hours * -1,
										qty_invoiced = n_hours * -1
									};

									var previous = new NeWODetailCurrent(MemberTimeDet.member_id, MemberTimeDet.MemberTimePayTypeHoursID,
										MemberTimeDet.membertime_woprog_id, temp_chargeout_id);

									NeMemberTime.WOCostVisibility(previousRecord, null, previous, wo_line, null);

									wo_line.save(currentUser, "WEBAPI Timesheet.delete()", true, conn, transaction);
									if (has_child && !MemberTimeDet.child_shoptime)
									{
										var their_wo = new NeWOProg(MemberTimeDet.woprog_id);
										if (their_wo.QuoteID == "0")
										{
											var parentWO = new NeWOProg(MemberTimeDet.membertime_child_woprog_id);
											var their_chargeout_id = Toolbox.doSQL_int(conn,
												@"SELECT IFNULL(MAX(id), 0) FROM membertype_chargeout where membertype_id = @v0  and paytype_id = @v1  and business_unit_id = @v2 ",
												new object[]
													{MemberTimeDet.membertype_id, MemberTimeDet.MemberTimePayTypeHoursID, parentWO.business_unit_id});
											var child_line = new NeWODetailCurrent(MemberTimeDet.member_id, MemberTimeDet.MemberTimePayTypeHoursID,
												MemberTimeDet.membertime_child_woprog_id, their_chargeout_id)
											{
												qty_ordered = n_hours * -1,
												qty_committed = n_hours * -1,
												qty_invoiced = n_hours * -1
											};

											var previousParent = new NeWODetailCurrent(MemberTimeDet.member_id, MemberTimeDet.MemberTimePayTypeHoursID,
												MemberTimeDet.membertime_child_woprog_id, their_chargeout_id);
											NeMemberTime.WOCostVisibility(previousRecord, null, previousParent, child_line, wo_line);

											child_line.save(currentUser, "WEBAPI Timesheet.delete", true, conn, transaction);
										}
									}
								}
								#endregion
							}
							else if (MemberTimeDet.MemberTime_Mileage == "true")
							{
								Toolbox.doSQL_void(conn, @" DELETE FROM wo_detail_current Where seg1_id=@v0", new object[] { MemberTimeDet.ID }, transaction);
							}
							transaction.Commit();
							return "Successfully deleted time entry";
						}
						#endregion
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						if (ex.ToString() != "NoLongerOpen")
						{
							return $"The following Error occured  {ex.Message}.";
						}
						else
						{
							return "You Cannot Delete a Time Sheet Entry for a Work Order That is no Longer Open";
						}
					}
				}
			}
		}

		public string TimesheetAutoComments(int userId, DateTime date)
		{
		return  "";
		}



	}

    public class TransferDeleteInfor
    {
        public int orignalMember_id { get; set; }
        public int negatieMember_id { get; set; }
        public int positiveMember_id { get; set; }

        public bool DeleteFromTransferItem { get; set; }

        public TransferDeleteInfor()
        {
            this.DeleteFromTransferItem = false;
        }
    }


}
