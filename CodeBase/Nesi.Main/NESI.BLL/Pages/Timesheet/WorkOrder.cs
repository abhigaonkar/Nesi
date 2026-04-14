using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using nesi.core;
using NESI.BLL.Common.Shared;
using NESI.BLL.Core;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.TimeSheet;
using IronPdf;
using System.IO;
using System.Configuration;
using NESI.BLL.Pages.Shared.Reports;

namespace NESI.BLL.Pages.Timesheet
{
	public partial class Timesheet
	{
		private readonly string[] _su = {
			"'Open'", // [0]
			"'Initial Prep'", // [1]
			"'Waiting PM Approval'", // [2]
			"'Rework'", // [3]
			"'Questions For PM'", // [4] -- Never allow
			"'Waiting For PO'", // [5] -- Never allow
			"'Waiting BM Approval'", // [6]
			"'Waiting To Be Invoiced'", // [7] -- Never allow
			"'Invoiced'", // [8] -- Never allow
			"'Hold'", // [9] -- Never allow
			"'Deleted'", // [10] -- Never allow
			"'Open Vendor POs'", // [11]
			"'Waiting for Parts'" // [12]
		};

		private string[] _sw = {
			"'Invoiced'",					// 0
			"'Waiting BM Approval'",		// 1
			"'Waiting To Be Invoiced'",		// 2
			"'Waiting PM Approval'",		// 3
			"'Questions For PM'",			// 4
			"'Waiting For PO'",				// 5
			"'Waiting Approval'",			// 6
			"'Hold'",						// 7
			"'Initial Prep'",				// 8
			"'Rework'",						// 9
			"'In Progress'",                // 10
			"'Open Vendor POs'",			// 11
			"'Waiting for Parts'"			// 12
		};

        public int GetAddressIdbyContactId(int contactId)
        {
            int result = _db.contact.Where(x => x.Contact_Status == "Active" && x.Contact_ID == contactId).Select(x => x.address_id).FirstOrDefault();

            return result;

        }
       
        public LabelValueInt[] GetCustomerContactByCustomerId(int id)
        {

 
            LabelValueInt[] result1 = { new LabelValueInt { Label = "Please send email recipient", Value = 0 }, new LabelValueInt { Label = "Multiple Recipients",Value = 1 }  };

            LabelValueInt[] result2 = _db.contact.Where(x => x.Contact_Status == "Active" && x.Contact_Cust_ID == id).OrderBy(x => x.Contact_Name)
                .Select(x => new LabelValueInt()
                {
                    Label = x.Contact_Name,
                    Value = x.Contact_ID
                }
                ).ToArray();

            var result = result1.Union(result2).ToArray();
            return result;


            //// @"Select contact_id id, contact_name _name from contact  where contact_cust_id =@v0 and contact_status = 'Active' order by contact_name"
            //return _db.contact.Where(x => x.Contact_Status == "Active" && x.Contact_Cust_ID == id).OrderBy(x => x.Contact_Name)
            //    .Select(x => new LabelValueInt()
            //    {
            //        Label = x.Contact_Name,
            //        Value = x.Contact_ID
            //    }
            //    ).ToArray();
        }


        public WorkOrderCustomer[] GetCustomerWorkOrderList(int buId, Employee user)
		{
			var approveBm = user.AuthenticatedForPrivilege(16);
			var approvePm = user.AuthenticatedForPrivilege(15);


			var usedList = $"{_su[0]},{_su[12]}";
			if (approveBm)
			{
				usedList = $"{_su[0]},{_su[1]},{_su[2]},{_su[3]},{_su[6]},{_su[12]}";
			}
			if (approvePm && !approveBm
			) // Approve BM supercedes PM approval... BM's might have this privilege, so don't reset it
			{
				usedList = $"{_su[0]},{_su[1]},{_su[2]},{_su[3]},{_su[12]}";
			}
			var sql = $@"
SELECT 
	DISTINCT(a.woprog_customer_id) woprog_customer_id, 
	b.customer_id, 
	a.woprog_customername,
	b.customer_number_int customer_number, 
	b.customer_name,
	a.woprog_id ,
	a.woprog_bvwo,
	a.parent_woprog_id 
FROM woprog a
LEFT JOIN
	customer b ON a.woprog_customer_id = b.customer_id
LEFT JOIN
	woprog c ON a.parent_woprog_id = c.woprog_id 
LEFT JOIN 
	customer d ON c.woprog_customer_id = d.customer_id
WHERE 
	a.business_unit_id = {buId} AND 
	a.woprog_status IN ({usedList}) AND 
	a.woprog_bvwo != 'Not Entered' AND 
	a.woprog_hold = 0 AND
	a.woprog_isrebill = 0 AND
	a.woprog_iscredit = 0 AND
    a.mat_only=false and
	b.customer_qc_member_id IS NOT NULL AND 
	b.customer_hold = 'F' AND 
	IF(a.parent_woprog_id > 1,
		(
		c.woprog_status NOT IN ('Invoiced','Waiting BM Approval','Waiting To Be Invoiced','Waiting PM Approval','Questions For PM','Waiting For PO','Waiting Approval','Hold','Initial Prep','In Progress') AND
		c.woprog_status != 'Deleted' AND
		c.woprog_associate_woprog_id = 0 AND
		c.woprog_bvwo != 'Not Entered' AND 
		d.customer_hold = 'F' AND 
		c.woprog_hold = 0 AND 
		d.customer_qc_member_id IS NOT NULL AND
		c.woprog_isrebill = 0	
		), TRUE) 
ORDER BY 
	a.woprog_customername,
	a.woprog_bvwo
	";
			return _db.Database.SqlQuery<WorkOrderCustomer>(sql).ToArray();
		}

		protected string getUsedList(Employee user)
		{
			var openList = user.AuthenticatedForPrivilege(15) || user.AuthenticatedForPrivilege(16) ? $"{_sw[0]},{_sw[1]},{_sw[2]},{_sw[7]},{_sw[4]}" : "";
			var usedList = $"{_sw[0]},{_sw[1]},{_sw[2]},{_sw[3]},{_sw[4]},{_sw[5]},{_sw[6]},{_sw[7]},{_sw[8]},{_sw[9]},{_sw[10]}";
			if (openList != "")
			{
				usedList = openList;
			}
			return usedList;
		}





		private string getCustomerIdQuery(int customerid)
		{
			return customerid == 0 ? "b.customer_id" : customerid.ToString();
		}

		private string getUsedListWithProcessList(Employee user)
		{
			var processList = user.AuthenticatedForPrivilege(91) ? $"{_sw[0]},{_sw[1]},{_sw[2]},{_sw[4]},{_sw[5]},{_sw[6]},{_sw[7]}"
				: "";
			return processList != "" ? processList : getUsedList(user);
		}


		private const string VIEW_WOPROG_LIST = @"
				SELECT Distinct
					a.woprog_description descript, 
					a.woprog_id,
					IF(a.woprog_id < 1000000, a.woprog_bvwo, LPAD(a.woprog_id,10, '0')) wo, 
					a.woprog_customername customername, 
					a.woprog_status status,
					b.customer_id,
					b.customer_number,
                    a.woprog_CustPO CutPO,
                    a.WOProg_Address_ID address_id
				FROM 
					woprog a
				LEFT JOIN
					customer b ON a.woprog_customer_id = b.customer_id
				LEFT JOIN
					woprog c ON a.parent_woprog_id = c.woprog_id
				LEFT JOIN 
					customer d ON c.woprog_customer_id = d.customer_id";


		public WorkOrderWo GetWOPROGById(int woId)
		{
			var sql = $@"{VIEW_WOPROG_LIST} WHERE a.woprog_id = {woId}";

			return _db.Database.SqlQuery<WorkOrderWo>(sql).FirstOrDefault();
		}

        public DataTable GetTaskList(int woId)
        {
            var sql = Toolbox.doSQL_dt($@"SELECT id,task FROM woprog_tasks WHERE woprog_id in ( @v0 ) AND IFNULL(complete,0) = 0", new object[] { woId });

            return sql;
        }



		public WorkOrderWo[] LoadCustWOPROGWIPLIST(int buId, Employee loginUser, Employee user, int customerId)
		{
			var usedList = getUsedListWithProcessList(loginUser);
			var strCustWo = $@"{VIEW_WOPROG_LIST}
					WHERE 
						b.customer_id = {getCustomerIdQuery(customerId)} AND 
						a.business_unit_id = {buId} AND 
						a.woprog_status NOT IN ({usedList}) AND
						a.woprog_status != 'Deleted' AND
						a.woprog_associate_woprog_id = 0 AND
						a.woprog_bvwo != 'Not Entered' AND 
						b.customer_hold = 'F' AND 
						a.woprog_hold = 0 AND 
						b.customer_qc_member_id IS NOT NULL AND
						a.woprog_isrebill = 0 AND
						a.woprog_iscredit = 0 AND
                        a.mat_only=FALSE AND                       
						IF(a.parent_woprog_id > 1,
							(
							c.woprog_status NOT IN ({usedList}) AND
							c.woprog_status != 'Deleted' AND
							c.woprog_associate_woprog_id = 0 AND
							c.woprog_bvwo != 'Not Entered' AND 
							d.customer_hold = 'F' AND 
							c.woprog_hold = 0 AND 
							d.customer_qc_member_id IS NOT NULL AND
							c.woprog_isrebill = 0	
                            ), true)
					ORDER BY 
						a.woprog_customername
						";



            var list = _db.Database.SqlQuery<WorkOrderWo>(strCustWo).ToArray();

            foreach(var e in list)
            {
                var woTaskList = GetTaskList(e.woprog_id);
                e.scopes = new List<Scope>();
                foreach (DataRow row in woTaskList.Rows)
                {
                    var r1 = new Scope
                    {
                        id = Convert.ToInt32(row["id"]),
                        name = row["task"].ToString()

                    };
                    e.scopes.Add(r1);
                }
                
            }
            return list;

		}

        public string[] LoadContactList(int customerId)
        {
            string sql = $@"SELECT 0 id, 'No Email' NAME, '' email UNION SELECT 1 id, 'Multiple Recipients' NAME, '' email UNION (SELECT contact_id id, IFNULL(contact_name, '') NAME, IFNULL(contact_email, '') email FROM contact WHERE contact_type = 'Customer' AND contact_cust_id = {customerId} ORDER BY contact_name ASC)";
            return _db.Database.SqlQuery<string>(sql).ToArray();
        }

		public bool CommentDisabled(int woId)
		{
			return (new NeWOProg(woId)).enableEditingScopeOnTimesheet;
		}

        public bool IsPrevailingWage(int timesheetId)
        {
            return Toolbox.doSQL_bool("SELECT is_prevailing_wage FROM membertime WHERE MemberTime_ID = @v0", new object[] { timesheetId });
        }

        public bool IsPrevailingWageEnabledForWo(int woId)
        {
            return new NeWOProg(woId).enable_prevailing_wages;
        }

        public WorkOrderComment[] LoadWOCommentsList
			(int buId, int userId, int woId)
		{


			var list = _db.Database.SqlQuery<WorkOrderComment>(
				@"SELECT 
				a.wocomment_id id, 
				a.comments comment 
				FROM wocomment a LEFT JOIN membertime b 
				ON a.wocomment_id = b.MemberTime_WoComment_ID WHERE
				(a.woprog_id = @p0 OR IF(@p0 < 100000, b.wotype = 'Shop', FALSE))
				AND a.wocomment_member_id != 0 AND a.business_unit_id =@p1 
				AND a.wocomment_member_id = @p2  order by comments
",
				woId, buId, userId).ToList();

			string commentCopy = "";
			foreach (var item in list)
			{
				if (item.comment.ToUpper().Trim() == commentCopy) continue;
				commentCopy = item.comment.ToUpper().Trim();
				item.comment = Value.value_from(item.comment.Trim());
			}
			var list2 = _db.Database.SqlQuery<WorkOrderComment>(@"
SELECT
  id,
  task `comment`
FROM
  woprog_tasks 
WHERE `woprog_id` = @p0
ORDER BY `_order`,
  id 
", woId).ToList();
			var count = 0;
			var list3 = new List<WorkOrderComment>();

			foreach (var item in list2)
			{
				count++;
				item.comment = count + ". " + item.comment;
				list3.Add(item);
			}
			count = 0;

			foreach (var item in list)
			{
				count++;
				var c = item.comment;
				if (c.Length >= 7)
				{
					c = item.comment.Substring(0, 7);
				}
				if (list3.FindIndex(x => x.comment.StartsWith(c) || x.comment == item.comment) < 0)
				{
					list3.Add(item);
				}
			}
			return list3.ToArray();
		}

		public string UpdateWorkOrder(Employee user, UpdateTimeSheetWorkOrder model)
		{
            // Limit daily cumulative time entry to 24 hours
            var error = ExceedMaxHoursOnOneDay_UpdateCase(user, model);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            var ret = GetEntityById(model.MemberTime_ID, out membertime entity, out wocomment comment);
			if (!string.IsNullOrEmpty(ret)) return ret;

			comment.comments = model.MemberTime_WoComment;
			comment.Member_ID_Audit = user.Id;
			entity.wo_percent_complete = model.PercentComplete;
			entity.NumberOfHours = model.NumberOfHours;
			entity.rating = model.Rating;
			entity.membertime_workorder_id = model.SelectedWorkOrderId.ToString();
            entity.scope_id = model.scope_id;
            entity.Prov_Id = model.prov_id;
            entity.mileage_value = model.mileage_value;
            entity.mileage_unit = model.mileage_unit;
            entity.is_prevailing_wage = model.is_prevailing_wage;
           
            return UpdateTimeSheet(user, entity, comment);
		}
        public static void UpdateChargeOut(Employee user, InsertTimeSheetWorkOrder model, ref int existingLines, int masterid)
        {
            using (var conn = Toolbox.connect())
            {

                    DataTable dt = Toolbox.doSQL_dt(conn, @"SELECT wo_detail_current_master_id 
                                                                    FROM wo_detail_current
                                                                    WHERE wo_detail_current_woprog_id= @v0 AND memberid = @v1 AND paytypeid =@v2 ", new object[] { model.SelectedWorkOrderId, user.Id, model.PayTypeId });
                    foreach (DataRow dr in dt.Rows)
                    {
                        if ((int)dr["wo_detail_current_master_id"] != masterid)
                        {
                            var existingMasterId = (int)dr["wo_detail_current_master_id"];
                            //var existingMasterId = existingLines == 0 ? 0 : Toolbox.doSQL_dt(conn, @"SELECT wo_detail_current_master_id 
                            //                                                FROM wo_detail_current
                            //                                                WHERE wo_detail_current_woprog_id= @v0 AND memberid = @v1 AND paytypeid =@v2 ", new object[] { model.SelectedWorkOrderId, user.Id, model.PayTypeId });
                            //if (existingMasterId != masterid)
                            //{
                            // fix the existing records / change the cost to match the resultant cost against laboermasterid 
                            var cost = Toolbox.doSQL_double(conn, @"SELECT get_cost( @v0 , @v1 , false)", new object[] { masterid, model.SelectedBusinessUnitId });
                            var sell = Toolbox.doSQL_double(conn, @"SELECT IFNULL(MAX(chargeout), 0) 
                            FROM membertype_chargeout WHERE business_unit_id = @v0  AND id=@v1  AND paytype_id = @v2", new object[] { model.SelectedBusinessUnitId, masterid, model.PayTypeId });
                            Toolbox.doSQL_void(conn, @"UPDATE wo_detail_current SET  wo_detail_current_date_modified = NOW() ,wo_detail_current_price_cost=@v0 , wo_detail_current_price_sell=@v1 , wo_detail_current_price_unit=@v1 WHERE wo_detail_current_master_id=@v2 AND wo_detail_current_woprog_id=@v3 AND memberid =@v4", new object[] { cost, sell, existingMasterId, model.SelectedWorkOrderId, user.Id });
                            // * maintain timestamp on line (date_modified)
                           // existingLines = Toolbox.doSQL_affectedrows(@"UPDATE wo_detail_current SET wo_detail_current_date_modified = NOW() WHERE wo_detail_current_master_id=@v0 AND wo_detail_current_woprog_id=@v1 AND memberid =@v2 AND paytypeid =@v3", new object[] { existingMasterId, model.SelectedWorkOrderId, user.Id, model.PayTypeId });
                            //}
                        }
                    }
                
            }
        }
        public string SaveWorkOrder(Employee user, InsertTimeSheetWorkOrder model)
		{
            // Limit daily cumulative time entry to 24 hours
            if (!model.IsTransfer)
            {
                // Not checking for transfer....
                var error = ExceedMaxHoursOnOneDay_InsertCase(user, model);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    return error;
                }
            }

                 var allowUnlinked = GetAllowUnlinkTimesheet(model.SelectedBusinessUnitId);
          
                var labormasterid = GetlabourMasterId(model.SelectedBusinessUnitId, user, model.PayTypeId, !allowUnlinked);
                if (labormasterid == 0 && !allowUnlinked)
                {
                    return @"This member type is not set up with a charge out rate in the selected business unit. <br/>
				Until this is corrected, you will not be able to enter time on work orders in this business unit.";
                }

                if (labormasterid != 0 && !allowUnlinked)
                {
                    var charge_out_rate = Toolbox.doSQL_double(@"CALL CUSTOMER_CHARGEOUT(@v0 ,@v1 )", new object[] { model.SelectedCustomerId, labormasterid });
                    if (charge_out_rate == 0)
                    {
                        return @"This member type is not set up with a charge out rate in the selected business unit. <br/>
				    Until this is corrected, you will not be able to enter time on work orders in this business unit.";
                    }
                }

                //
                // New check required by [1540 Allow for membertype selection on the timesheet]
                // Before this point, we always checking the employee's profile member type setting up. this is no hurt.
                //
                var laborMasterIdForSelectedJobType = 0;
                if ((!allowUnlinked) &&
                     model.allow_jobtype_selection &&
                     (model.selectedJobType != user.EmployeeProfile.member_membertype_id))
                {
                    // Selected job type is not same as the employee profile setting one
                    laborMasterIdForSelectedJobType = GetlabourMasterIdForSelectedJobType(model.SelectedBusinessUnitId, user, model.PayTypeId, !allowUnlinked, model.selectedJobType, model.selectedJobTypeName);
                    if (laborMasterIdForSelectedJobType == 0)
                    {
                        return @"The selected Job type is not set up with a charge out rate in the selected business unit. <br/>
				Until this is corrected, you will not be able to enter time on work orders in this business unit.";
                    }
                }

                //
                // Charge out rate should not be ZERO. Otherwise there will be a labor line with zero unit price and the sell price will be obtained by 
                // calling [sell = sell == 0 ? use_fixed_markup?fixed_markup*cost: shared.GetSellPrice(cost, 0, true, qty_committed, business_unit_id) : sell;]
                //
                if (laborMasterIdForSelectedJobType != 0 && (!allowUnlinked))
                {
                    var charge_out_rate = Toolbox.doSQL_double(@"CALL CUSTOMER_CHARGEOUT(@v0 ,@v1 )", new object[] { model.SelectedCustomerId, laborMasterIdForSelectedJobType });
                    if (charge_out_rate == 0)
                    {
                        return @"This member type is not set up with a charge out rate in the selected business unit. <br/>
				    Until this is corrected, you will not be able to enter time on work orders in this business unit.";
                    }
                }


                //
                // Put the master id as grouping stuff when allowing multiple member type.
                // 
                int masterID = labormasterid;
                if (model.allow_jobtype_selection)
                {
                    masterID = laborMasterIdForSelectedJobType;
                }

                var existingLines = bllToolbox.doSQL_int(@"SELECT count(*) from wo_detail_current WHERE wo_detail_current_woprog_id= @v0 AND memberid = @v1 AND paytypeid =@v2 AND wo_detail_current_master_id=@v3 ", new object[] { model.SelectedWorkOrderId, user.Id, model.PayTypeId, masterID });
                if (existingLines > 1 && model.PayTypeId!=8)
                {
                    return "There is an error with this work order, you should not have multiple labor entries for the same pay type";
                }

                if (!allowUnlinked)
                {
                    // From Matt: if they don't have a wage on file, they cannot add workorder time.
                    var existingAtiveWageRecords = bllToolbox.doSQL_int(
                        @"SELECT COUNT(1) FROM memberwage WHERE memberwage_memberid = @v0 AND active = 'true'",
                        new object[] { user.Id });
                    if (existingAtiveWageRecords == 0)
                    {
                        return "There isn't an active wage record for the selected user";
                    }
                }

            
            // Timesheet.UpdateChargeOut(user, model, ref existingLines, laboermasterid);
          
           
            model.WoProg_Bvwo = model.SelectedWorkOrderId.ToString();

			var res = SetInsertEntity(model, out membertime entity, out wocomment comment);
			if (!string.IsNullOrWhiteSpace(res)) return res;
			entity.WOType = "WO";
            entity.scope_id = model.scope_id;
            entity.Prov_Id = model.prov_id;
            entity.mileage_value = model.mileage_value;
            entity.mileage_unit = model.mileage_unit;
            entity.is_prevailing_wage = model.is_prevailing_wage;

			comment.WorkOrder_ID = model.SelectedWorkOrderId.ToString().PadLeft(10, '0');
			comment.woprog_id = model.SelectedWorkOrderId;
          
            model.WoProg_Bvwo = allowUnlinked ? model.SelectedWorkOrderName : model.SelectedWorkOrderId.ToString();
            
			// save selected wo/ customer to user profile.
			if (user.MemberType.membertype_id == 78 || user.MemberType.membertype_id == 114)
			{
				var p = new BLL.Base.ProfileBase(user);
				p.SetPropertyValue("Timesheet_Customer", model.SelectedCustomerId.ToString());
				p.SetPropertyValue("Timesheet_WorkOrder", model.SelectedWorkOrderId.ToString());
			}


			//get customerId from woprog
			var wo = new Ne2WOProg(model.SelectedWorkOrderId).Entity;
			if (wo != null)
			{
				entity.MemberTime_Customer_ID = Convert.ToInt32(wo.woprog_customer_id);
				entity.MemberTime_Cust_No = new Ne2Customer(model.CustId).Entity.Customer_Number;
			}
			else
			{
				entity.MemberTime_Customer_ID = 0;
				entity.MemberTime_Cust_No = model.SelectedCustomerName;
			}
			entity.MemberTime_WOProg_id = model.SelectedWorkOrderId;
			entity.membertime_shop_type_id = 0;
			var Wo = new Ne2WOProg(model.SelectedWorkOrderId);
            var selectedUser = new Employee(model.SelectedUserId);

		    if (model.SelectedWorkOrderId > 0 && selectedUser.BusinessUnitId != wo.business_unit_id)
		    {
		        return "Cannot save time entry, this employee does not belong to the same business unit as this work order.";
		    }

		    var woId = Wo.Entity?.parent_woprog_id.GetValueOrDefault();
			if (woId > 1)
			{

				var customer = new Ne2Customer(Convert.ToInt32(Wo.Entity.woprog_customer_id));
				// if customer is ne company then get parent work order
				if (customer.IsFound && customer.IsNECompany)
				{
					// get parent work order
					var parentWo = new Ne2WOProg(Wo.Entity.parent_woprog_id.GetValueOrDefault());

					// get parent workorder customer
					var parentCustomer = new Ne2Customer(Convert.ToInt32(parentWo.Entity.woprog_customer_id));

					entity.MemberTime_Child_Cust_No = parentCustomer.Entity.Customer_Number;
					entity.membertime_child_customer_name = parentCustomer.Entity.customer_name;
					entity.MemberTime_WoComment_Child_ID = 0;

					entity.MemberTime_Child_WorkOrder_ID = parentWo.Entity.woprog_id.ToString();
					entity.MemberTime_Child_WOProg_id = Convert.ToInt32(parentWo.Entity.woprog_id);
					entity.MemberTime_Child_CompanyID = parentWo.Entity.business_unit_id;
					entity.child_business_unit_id = parentWo.Entity.business_unit_id;
					if ( model.allow_jobtype_selection && (!allowUnlinked))
                    {
                        var name = parentWo.Entity.woprog_customername;
						//
						// Still need to check parent wo's chargeoutID and chargeout Value.
						//
						int laborMasterIdForSelectedJobType_parent = GetlabourMasterIdForSelectedJobType(parentWo.Entity.business_unit_id.Value, user, model.PayTypeId, false, model.selectedJobType, model.selectedJobTypeName);
						if (laborMasterIdForSelectedJobType_parent == 0)
                        {
                            return @"The selected Job type is not set up with a charge out rate in the selected business unit for custom: " + name + "." +
                                   @"<br/>until this is corrected, you will not be able to enter time on work orders in this business unit.";
                        }

                        var charge_out_rate_parent = Toolbox.doSQL_double(@"CALL CUSTOMER_CHARGEOUT(@v0 ,@v1 )", new object[] { model.SelectedCustomerId, laborMasterIdForSelectedJobType_parent });
                        if (charge_out_rate_parent == 0)
                        {
                            return @"The selected Job type is not set up with a charge out rate in the selected business unit for custom: " + name + "." +
                                   @"<br/>until this is corrected, you will not be able to enter time on work orders in this business unit.";
                        }
                    }
                }
			}

			var ret = SaveTimeSheet(user, entity, comment, model.WoProg_Bvwo, null, model.SelectedTsLiteType, model);
			if (ret.IndexOf("success", StringComparison.Ordinal) > -1)
			{

			}
			return ret;
		}

        private string ExceedMaxHoursOnOneDay_InsertCase(Employee user, InsertTimeSheetWorkOrder model)
        {
            if (model.PayTypeId == 8)
            {
                // mileage will not be checked by here.
                return "";
            }

			CumulativeParameter parameter = new CumulativeParameter
				{
				OnWhichDate = model.Date,
				ForWho = user.Id,
				action = TimesheetAction.Add,
				newValue = model.NumberOfHours,
				oldValue = 0
				};

			var result = this.GetCumulativeInformation(parameter);
            if (result.DoesItExceedMaxValue)
            {
                return result.Errors[0].error;
            }
            else
            {
                // Not Exceeds
                return "";
            }
        }

        private string ExceedMaxHoursOnOneDay_UpdateCase(Employee user, UpdateTimeSheetWorkOrder model)
        {
			CumulativeParameter parameter = new CumulativeParameter
				{
				OnWhichDate = model.Date,
				ForWho = model.SelectedUserId,
				action = TimesheetAction.Edit,
				newValue = model.NumberOfHours
				};
			var record = this.GetbyId(model.MemberTime_ID);
            parameter.oldValue = record.NumberOfHours.Value;

            if (record.MemberTime_PayTypeHours_ID == 8)
            {
                // mileage will not be checked by here.
                return "";
            }


            var result = this.GetCumulativeInformation(parameter);
            if (result.DoesItExceedMaxValue)
            {
                return result.Errors[0].error;
            }
            else
            {
                // Not Exceeds
                return "";
            }
        }

        public string SaveSignature(InsertSignature model,int WOID,bool IsdailySignoff,DateTime SelectedDate)
        {
			NESignature Newsignature = new NESignature
				{
				id = 0,
				contact_id = Toolbox.ReturnZeroIfNull_int(model.customer_contact),
				send_to_contact_ids = model.send_to_contact_ids,
				created_dt = DateTime.Now,
				graphic = Convert.FromBase64String(model.signatureField.Split(',')[1]),
				printed_name = model.printname,
				table = "woprog",
				table_id = WOID,
				member = CurrentUser.Id
				};


			Newsignature.save(!IsdailySignoff);


			string result = SaveToPDF(model.signatureField,WOID, model.printname, IsdailySignoff,SelectedDate)
								? "Daily signoff has been saved successfully."
								: "Daily signoff failed to save.";
            return result;
        }


        public byte[] ReviewPDF(int WOID, bool IsdailySignoff,DateTime SelectedDate)
        {
           // var PDF_path = "";
            //var Filname = "";
            NeWOProg WO = new NeWOProg(WOID);
            NeBusinessUnit Bu = new NeBusinessUnit(WO.business_unit_id);
			var ProjectFolder = new NeFiles().GetProjectFolder(WOID);
			var PdfFile = PdfFileName(Bu, WOID, SelectedDate, IsdailySignoff, true);
			var currentFile = $@"{ProjectFolder}\signature_files\{PdfFile}";
			if(File.Exists(currentFile))
				{ 
				//MH - Return existing 
				return File.ReadAllBytes(currentFile);
				}
            var htmlToPdf = IronPdf_Report.SetHtmlToPdf();
            var Header_html = DailyProjectSummary.GetDailyProjectHeader(WO.woprog_id, Toolbox.ReturnBlankIfNull_string(CurrentUser.FullName).ToString());
            var Labour_html = DailyProjectSummary.GetLabourTable(WO.woprog_id, SelectedDate);
            var GeneralDescriptionOfWork_html = DailyProjectSummary.GetGeneralDescriptionofWork(WO.woprog_id);
            var MiscellaneousHTML = DailyProjectSummary.GetMiscellaneousTable(WO.woprog_id, SelectedDate);
            var RentalHTML = DailyProjectSummary.GeRentalsTable(WO.woprog_id, SelectedDate);

            NeMember PM = new NeMember(WO.woprog_sp_memberid);

            var Approvals_html = DailyProjectSummary.GetApprovalsTable("", PM.FullName, CurrentUser.FullName, "");
            var html = $@"{Header_html}<br>
                        <font size='2' face='Arial'>MANPOWER</font>
                        {Labour_html}
                        <br>
                        <font size='2' face='Arial'>GENERAL DESCRIPTION OF WORK</font>
                        {GeneralDescriptionOfWork_html}
                        <br>
                        <font size='2' face='Arial'>MISCELLANEOUS</font>
                        {MiscellaneousHTML}
                        <br>
                        <font size='2' face='Arial'>RENTALS</font>
                        {RentalHTML}
                        <br>
                        <font size='2'  face='Arial'>APPROVALS</font>
                        {Approvals_html}";

            htmlToPdf.RenderHtmlAsPdf(html).MetaData.Title = PdfFile;


            return htmlToPdf.RenderHtmlAsPdf(html).BinaryData;
   
        }



        public bool SaveToPDF(string img,int WOID,string printname,bool IsDailySignoff,DateTime SelectedDate)
        {
		string teSignoffFolder = "";
            try
            {

                var IronPdf_Report = new IronPdf_Report();
                var htmlToPdf = IronPdf_Report.SetHtmlToPdf();
                var WO = new NeWOProg(WOID);
                var Contact = new NEContact(WO.woprog_Contact_ID);
                var Bu = new NeBusinessUnit(WO.business_unit_id);
                teSignoffFolder = DailyProjectSummary.GetSignoffFilePath(WO.business_unit_id, WOID);
                var Header_html = DailyProjectSummary.GetDailyProjectHeader(WO.woprog_id, Toolbox.ReturnBlankIfNull_string(CurrentUser.FullName).ToString());
                var Labour_html = DailyProjectSummary.GetLabourTable(WO.woprog_id, SelectedDate);
                var GeneralDescriptionOfWork_html = DailyProjectSummary.GetGeneralDescriptionofWork(WO.woprog_id);
                var MiscellaneoueHTML=DailyProjectSummary.GetMiscellaneousTable(WO.woprog_id, SelectedDate);
                var RentalHTML = DailyProjectSummary.GeRentalsTable(WO.woprog_id, SelectedDate);
                NeMember PM = new NeMember(WO.woprog_sp_memberid);

                var Approvals_html = DailyProjectSummary.GetApprovalsTable(img, PM.FullName, CurrentUser.FullName, printname);
                var html = $@"{Header_html}<br>
                        <font size='2' face='Arial'>MANPOWER</font>
                        {Labour_html}
                        <br>
                        <font size='2' face='Arial'>GENERAL DESCRIPTION OF WORK</font>
                        {GeneralDescriptionOfWork_html}
                        <br>
                        <font size='2' face='Arial'>MISCELLANEOUS</font>
                        {MiscellaneoueHTML}
                        <br>
                        <font size='2' face='Arial'>RENTALS</font>
                        {RentalHTML}
                        <br>
                        <font size='2'  face='Arial'>APPROVALS</font>
                        {Approvals_html}";


                return IronPdf_Report.SavePdf(htmlToPdf, teSignoffFolder, PdfFileName(Bu, WOID, SelectedDate, IsDailySignoff, true), html);

            }
            catch (Exception ex)
            {
               // throw ex;
			   Toolbox.do_errorLog(ex, $@"Signoff path: {teSignoffFolder} -- {WOID}");
               return false;
            }

           // return false;
        }
		public static string PdfFileName(NeBusinessUnit bu, int woId, DateTime dt, bool dailySignOff, bool includeExtension = false)
			{
			var suffix = dailySignOff ? "-Daily_Signoff" : "-Final_SignOff";
			var ext = includeExtension ? ".pdf" : "";
			return $"{bu.name}-{woId}-{dt.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)}{suffix}{ext}";
			}

		public LabelValueInt[] GetTsLitePayTypes(int buid)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT id value, ts_lite_paytype label from membertime_ts_lite_paytypes where business_unit_id=@p0 order by ts_lite_paytype", buid);
		}

        //
        // Handle multiple entries.
        //
        public BatchInsertionResult SaveMultipleWorkOrder(Employee user, InsertTimeSheetWorkOrder model)
        {
            BatchInsertionResult bir = new BatchInsertionResult { };

            if (model.timesheetHourTypeList == null || model.timesheetHourTypeList.Count() == 0)
            {
                bir.okay = 0;
                bir.result = "No data provided!";
                return bir;
            }

            foreach (var record in model.timesheetHourTypeList)
            {
                model.NumberOfHours = record.hours;
                model.PayTypeId = record.hourTypeId;

                var result = SaveWorkOrder(user, model);
                TimesheetTransactionRecord ttr = new TimesheetTransactionRecord
                {
                    hourTypeId = record.hourTypeId,
                    hourType = record.hourType,
                    hours = record.hours,
                    error = result
                };

                if (result.IndexOf("success", StringComparison.Ordinal) == -1)
                {
                    // This one failed
                    bir.okay = 0;
                    bir.result = "Some timesheet record(s) were not created, please check below info:";

                    // Add the error to list.
                    bir.list.Add(ttr);
                }
            }

            return bir;
        }


        public ContactEmailOutput SendEmailToContact(ContactEmailInput input)
        {
            var ret = new ContactEmailOutput() { msg = "", okay = false };

            //
            // Validation on basic parameters.
            //
            if (input == null || input.woid == 0 || input.date == null || string.IsNullOrEmpty(input.contacts))
            {
                ret.msg = "Input not valid.";
                return ret;
            }

            //
            // Get contacts email adddress based on their ids.
            //
            var contactIds = input.contacts.Split(',');
            var recipients = SendEmailToContact_Email(contactIds.ToList());
            if (recipients == null || recipients.Count == 0)
            {
                ret.msg = "No email address found for selected contact.";
                return ret;
            }

            //
            // Get the pdf file by using the existing function.
            //
            bool isDailySignoff = true;
            var onlydate = new DateTime(input.date.Year, input.date.Month, input.date.Day, 0,0, 0);
            var bytes = this.ReviewPDF(input.woid, isDailySignoff, onlydate);
            if (bytes == null || bytes.Length == 0)
            {
                ret.msg = "No PDF file was found or generated.";
                return ret;
            }

            //
            // Get the from based on workorder.
            //
            var from = this.GetProjectMangerEmailAsFromBasedonWorkorder(input.woid);
            if (string.IsNullOrWhiteSpace(from))
            {
                ret.msg = "Cannot find project manager email.";
                return ret;
            }

            //
            // The code to send email go here.
            //
            var esi = new EmailSendingInfo()
            {
                recipients = recipients,
                send_result = false,
                msg = "",
                pdf = bytes,
                woid = input.woid,
                date = input.date,
                from = from
            };
            esi = this.SendEmailToContact_SendEmail(esi);

            if (esi.send_result)
            {
                ret.okay = true;
                ret.msg = "Emails were sent out successfully.";
            }
            else
            {
                ret.okay = false;
                ret.msg = esi.msg;
            }
            return ret;
        }

        private string GetProjectMangerEmailAsFromBasedonWorkorder(int woid)
        {
            var wo = new NeWOProg(woid);
            if (wo == null)
            {
                return "";
            }

            var employeeid = wo.intProjectManager;
            if(employeeid == 0)
            {
                return "";
            }

            var employee = new Employee(employeeid);
            if (employee == null)
            {
                return "";
            }

            return employee.Email;
        }

        private EmailSendingInfo SendEmailToContact_SendEmail(EmailSendingInfo info)
        {
            //
            // Flat recipients;
            //
            var to = "";
            if (info.recipients.Count == 1)
            {
                to = info.recipients[0];
            }
            else
            {
                to = string.Join(";", info.recipients);
            }

            try
            {
                // 
                // Prepare pdf file from stream
                //
                MemoryStream s = new MemoryStream(info.pdf);

                // file name
                string file = info.woid.ToString() + "-- " + info.date.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) +  "-Daily_Signoff.pdf";

                var attachment = new System.Net.Mail.Attachment(s, file);

                var m = new NeEMail
                {
                    From =info.from,
                    To = to,
                    Subject = "For Approval",
                    Attachment = attachment
                };

                m.Send();

                info.send_result = true;
            }
            catch (Exception ex)
            {
                info.send_result = false;
                info.msg = ex.Message;
            }

            return info;
        }

        private List<string> SendEmailToContact_Email(List<string> ids)
        {
            var list = new List<string> { };
            if (ids == null || ids.Count == 0)
            {
                return list;
            }

            foreach (var id in ids)
            {
                int emid = 0;
                var okay = int.TryParse(id, out emid);
                if (!okay || emid == 0)
                {
                    continue;
                }

                var employee = new NEContact(emid);
                if (employee == null || string.IsNullOrEmpty(employee.Contact_Email))
                {
                    continue;
                }

                list.Add(employee.Contact_Email);
            }

            return list;
        }

    }

    #region return data
    public class TimesheetTransactionRecord
    {
        public int hourTypeId { get; set; }
        public string hourType { get; set; }
        public double hours { get; set; }

        public string error { get; set; }
    }

    public class BatchInsertionResult
    {
        public int okay { get; set; }
        public string result { get; set; }
        public List<TimesheetTransactionRecord> list { get; set; }

        public BatchInsertionResult()
        {
            result = "Time entres have been saved successfully.";
            okay = 1;
            this.list = new List<TimesheetTransactionRecord> { };
        }
    }



    #endregion

    #region for sending email
    public class ContactEmailInput
    {
        public int woid { get; set; }
        public DateTime date { get; set; }
        public string contacts { get; set; }
    }

    public class ContactEmailOutput
    {
        public bool okay { get; set; }
        public string msg { get; set; }
    }

    public class EmailSendingInfo
    {
        public bool send_result { get; set; }
        public string msg { get; set; }
        public int woid { get; set; }
        public DateTime date { get; set; }
        public List<string> recipients { get; set; }
        public byte[] pdf { get; set; }   
        public string from { get; set; }
    }
    #endregion

}