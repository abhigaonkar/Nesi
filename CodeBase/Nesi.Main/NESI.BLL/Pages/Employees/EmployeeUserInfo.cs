using System;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using dto = NESI.DTO.ViewModels.Page.Employees.EmployeeUserInfo;
using NESI.Common.Models;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeUserInfo : EmployeeEdit
	{
		public EmployeeUserInfo(Employee user) : base(user)
		{

		}

		public EmployeeUserInfo(Employee user, int member_id) : base(user, member_id)
		{
			can_access = tab_enabled[0];
		}

		public double GetChargeout(int buid, int mtid)
		{
			return bllToolbox.doSQL_double(@"SELECT IFNULL(MAX(chargeout), 0) 
FROM membertype_chargeout WHERE business_unit_id = @v0  AND membertype_id = @v1  AND paytype_id = 1", buid, mtid);
		}

		public override object Profile()
		{
			var user = new NeMember(member_id);

			var provList = GetProvList();
			var businessUnitList = VisibleBusinessUnit();
			var businessUnit_enabled = CurrentUser.AuthenticatedForPrivilege(122);
			var memberTypeList = GetMemberTypeList();
			var payrollHandlerList = GetPayrollHandlerList();
			var reportsToList = GetReportsToList();
			var payTypelist = GetPayTypeList();
			var locationList =
				GetExtLocationList(user.business_unit_id != 0 ? user.business_unit_id : CurrentUser.BusinessUnitId);
			var newHire_visible = user.id != 0 && CurrentUser.AuthenticatedForPrivilege(120);
			var payroll_verified_enabled = false;
			var membertype_enabled = CurrentUser.MemberType.membertype_id == 40;
			var statusList = new LabelValueString[2];
			statusList[0] = new LabelValueString { Label = "Not Active", Value = "Not Active" };
			statusList[1] = new LabelValueString { Label = "Active", Value = "Active" };
			var hasdepentantsList = new LabelValueString[2];
			hasdepentantsList[0] = new LabelValueString { Label = "Yes", Value = "Yes" };
			hasdepentantsList[1] = new LabelValueString { Label = "No", Value = "No" };
			var countryList = new LabelValueString[3];
			countryList[0] = new LabelValueString { Label = "Canada", Value = "CAN" };
			countryList[1] = new LabelValueString { Label = "USA", Value = "USA" };
			countryList[2] = new LabelValueString { Label = "OTHER", Value = "OTHER" };

			if (user.reports_to == 100000)
			{
				user.reports_to = bllToolbox.doSQL_int(@"select ifnull((Select reports_to from member_offers  where memberid =@v0 order by enddate desc limit 1),100000)", user.id);
			}
			if (user.reports_to == 100000)
			{
				user.reports_to = Convert.ToInt32(user.payroll_handler);
			}

			var entity = (dto)MapperFrom(new dto(), user); //AutoMapper.Mapper.Map<dto>(user);
            entity.TerminateDate = string.IsNullOrEmpty(entity.TerminateDate) ? "2099-12-31" : entity.TerminateDate;
            entity.Start_Date = Convert.ToDateTime(entity.StartDate);
			entity.reporting_text = NeMember.get_reporting_line(entity.id);
			entity.Email = entity.Email.ToLower();
			entity.NEEmail = entity.NEEmail.ToLower();
			//var memberwage_c = bllToolbox.doSQL_int(@"SELECT COUNT(memberwage_id) FROM memberwage WHERE memberwage_memberid = @v0 ", user.id);

			entity.is_backoffice = CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault();

			//entity.memberwage = user.id == 0
			//	? 0
			//	: memberwage_c > 0
			//		? bllToolbox.doSQL_double(@"SELECT currentwage FROM memberwage WHERE memberwage_memberid = @v0  ORDER BY Date DESC LIMIT 1", user.id)
			//		: 0;
			entity.memberwage = GetChargeout(entity.business_unit_id, entity.MemberTypeID);

			var reports_to_enabled = NeMember.is_supervisor(user.id, CurrentUser.Id) ||
									is_branch_hr && CurrentUser.BusinessUnitId == user.business_unit_id ||
									is_depart_hr ||
									ispayroll;

			var personal_detail_visible = CurrentUser.AuthenticatedForPrivilege(37);

			var if_type = "";
			if (issupervisor || CurrentUser.AuthenticatedForPrivilege(156))
			{
				if_type = "direct";
			}
			else if (CurrentUser.isDeveloper) // release tools page privilege
			{
				if_type = "sysadmin";
			}
			else if (CurrentUser.AuthorizePage(44) && CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault())
			{
				if_type = "payroll";
			}

			if (user.id != 0)
			{
				payroll_verified_enabled = NeMember.to_do.exists(CurrentUser.Id, "payroll", user.id, Toolbox.connect());
			}

                var IsBM = user.business_unit_id == CurrentUser.BusinessUnitId
                    && CurrentUser.MemberType.membertype_id == 5 && user.Report_To_List(CurrentUser.Id).Contains(user.id);

            var terminate_visible =
				user.Status == "Active" && (
					isowner || issupervisor || ispayroll ||
					can_term_all ||
					CurrentUser.AuthorizePage(44) &&
					entity.is_backoffice || IsBM
                    );
			var hrStatusList = bllToolbox.doSQL_Array<LabelValueInt>(@"Select member_hrstatus.id value, member_hrstatus.status label from member_hrstatus");
			return new
			{
				is_backoffice,
				entity,
				provList,
				businessUnitList,
				memberTypeList,
				payrollHandlerList,
				reportsToList,
				payTypelist,
				locationList,
				statusList,
				hrStatusList,
				hasdepentantsList,
				countryList,
				businessUnit_enabled,
				newHire_visible,
				if_type,
				personal_detail_visible,
				payroll_verified_enabled,
				membertype_enabled,
				terminate_visible,
				reports_to_enabled,
				can_edit_user
			};
		}

		protected LabelValueInt[] GetPayrollHandlerList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				@" SELECT a.member_id value,
CONCAT(b.name, ' - ',a.member_fullname) label
FROM member a LEFT JOIN business_unit b on a.business_unit_id = b.id 
WHERE FIND_IN_SET(a.member_id, GET_POSSIBLE_PAYROLL_HANDLERS(@v0))
AND a.member_status = 'Active' 
AND a.member_id IN (SELECT memberpage_member_id FROM memberpage WHERE memberpage_page_id = 44) 
ORDER BY b.name, a.member_lastname, a.member_nickname",
				member_id);
		}

		protected LabelValueInt[] GetReportsToList()
		{
			var list = bllToolbox.doSQL_List<LabelValueInt>(@"call get_possible_supervisors_n2(@p0)", member_id);
			list.Add(new LabelValueInt() { Label = "Board of Directors", Value = 0 });
			var pos = list.FindIndex(x => x.Value == member_id);
			if (pos > -1)
			{
				list.RemoveAt(pos);
			}
			return list.ToArray();
		}

		protected LabelValueInt[] GetExtLocationList(int buid)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"(select 0 id, 'Not Set' label) union (
			SELECT id value,urldecode(name) as label FROM inventory_location_master WHERE type_id = 2 and business_unit_id = @p0  ORDER BY name)
			", buid);
		}


		public string Reset_Todo(string type)
		{
			bllToolbox.doSQL_void(@"Delete from member_todo_list_helper  where type=@v0 and type_id =@v1", type, member_id);
			return "Success.";
		}

		public DataExtra Save(dto model)
		{
#pragma warning disable CS0252 // Possible unintended reference comparison; left hand side needs cast
			if (CheckReportsTo(model.reports_to.ToString())?.Data != "success")
#pragma warning restore CS0252 // Possible unintended reference comparison; left hand side needs cast
			{
				throw new Exception("Validation error.");
			}
			var user = new NeMember(member_id);
			var o_user = new NeMember(member_id);
            var BusinessUnit = new NeBusinessUnit(user.business_unit_id);
            var taxentity = new NeTaxEntity(BusinessUnit.tax_entity_id);


            user = (NeMember)MapperFrom(user, model);
			if (member_id == 0)
			{
				user.timetostat = user.Country == "CAN" ? 0 : 90;
				user.NEEmail = "nomail@thatsnew.com";
                if (!string.IsNullOrEmpty(taxentity.region))
                {
                    user.vacation_amount_1 = taxentity.vac_freq_1;
                    user.vacation_amount_2 = taxentity.vac_freq_2;
                    user.vacation_amount_3 = taxentity.vac_freq_3;
                    user.vacation_interval_1 = (int)taxentity.vac_amount_1;
                    user.vacation_interval_2 = (int)taxentity.vac_amount_2;
                    user.vacation_interval_3 = (int)taxentity.vac_amount_3;
                }
    //            user.vacation_amount_1 = (decimal)(user.Country == "CAN" ? 0.04 : 40);
				//user.vacation_amount_2 = (decimal)(user.Country == "CAN" ? 0.06 : 60);
				//user.vacation_amount_3 = (decimal)(user.Country == "CAN" ? 0.08 : 120);
				//user.vacation_interval_1 = (user.Country == "CAN" ? 0 : 12);
				//user.vacation_interval_2 = (user.Country == "CAN" ? 60 : 36);
				//user.vacation_interval_3 = (user.Country == "CAN" ? 120 : 60);
				user.member_default_location =
					model.business_unit_id == 11 || model.business_unit_id == 48 ? 0 : model.member_default_location;
			}
			user.StartDate = Toolbox.MySQL_shortdt(model.Start_Date);
            user.TerminateDate = string.IsNullOrEmpty(model.TerminateDate) ? "2099-12-31": model.TerminateDate;
            user.BirthDate = string.IsNullOrEmpty(model.BirthDate) ? Toolbox.MySQL_shortdt(default(DateTime)) : model.BirthDate;
			// Check if there are any credit card transaction yet to integrate into NetSuite before the bu changes
			if (o_user.business_unit_id != model.business_unit_id && NECredit_card_purchase.HasUnintegratedTransactions(member_id))
			{
				return new DataExtra("There are credit card transactions for this employee which have not integrated into NetSuite, business unit can not be changed untill integration has completed. Please try again later.");
			}
            if (o_user.hrstatus_id == 1 && user.hrstatus_id != 1)
			{
				try
				{
					var status_name = bllToolbox.doSQL_string(@"SELECT status FROM member_hrstatus WHERE id = @v0  LIMIT 1", model.hrstatus_id);
					var em = new NeEMail
					{
						Subject = "(" + CurrentUser.FullName + ") has moved (" + user.FullName + ") to the HR status of '" + status_name +
								  "'",
						To = user.business_unit.branch_manager.NEEmail,
						From = CurrentUser.Email
					};

					em.Send();
				}
				catch (Exception ee)
				{
					return new DataExtra(
						$"There was an error sending a message to the branch manager of the HR status change: <div class='dtl'>{ee}</div>");
				}
			}

			var s = MemberSave(user, model, "User information");

			if (user.hrstatus_id != o_user.hrstatus_id)
			{
				try
				{
					var typepriv = new NEMemberTypePrivilege();
					switch (user.hrstatus_id)
					{
						case 1:  // New
							typepriv.SetBaseLoginPrivilege(user.id);
							break;
						case 3:  // Approved
						case 6:  // Probation
							if (o_user.hrstatus_id != 6)
							{
								typepriv.SetMemberPrivileges(user.id);
							}
							break;
						case 4:  // Pending Closure 
						case 5:  // Past
							typepriv.ClearPrivileges(user.id);
							break;
					}
				}
				catch (Exception ee)
				{
					return new DataExtra($"There was a problem switching this user's hr status: <div class='dtl'>{ee}</div>");
				}
			}

			if (o_user.MemberTypeID != user.MemberTypeID || o_user.Status == "Not Active" && user.Status == "Active")
			{
				try
				{
					//NEMemberTypePrivilege createpriv = new NEMemberTypePrivilege();
					//createpriv.SetMemberPrivileges(n_info.id);
					// Doing this because it's using a method to clear privileges when setting the new member type privileges
					// This method was originally intended for use only when terminating a user, not when they were still active
					// So I am choosing to patch the issue for the time being versus doing a broader fix.
					//Fix the problem where user's status are cleared upon changing their member types.
					user.Status = "Active";
					user.save();
				}
				catch (Exception ee)
				{
					return new DataExtra(
						$"There was a problem switching this user's member type: <div class='dtl'>{ee}</div>");
				}
			}

			try
			{
				var before = Toolbox.dict_create(o_user);
				var after = Toolbox.dict_create(user);
				var diff_from = before.Except(after).ToDictionary(k => k.Key, v => v.Value);
				var diff_to = after.Except(before).ToDictionary(k => k.Key, v => v.Value);

				if (diff_from.Count > 0)
				{
					var changes = Toolbox.dict_dump(diff_from) + "<b>Changed to</b> <br/>" + Toolbox.dict_dump(diff_to);
					var hr_notice = new NeEMail
					{
						To = EmailID.Payroll + Toolbox.app_setting("DomainForEmail") + ";" + EmailID.HR +  Toolbox.app_setting("DomainForEmail") ,
						From = EmailID.Administrator +  Toolbox.app_setting("DomainForEmail"),
						Subject = "User edit notice",
						isHTML = true,
						Body =
							$"The user <b>{user.FullName}</b> was edited by <b>{CurrentUser.FullName}</b> - Member ID ({user.id}).<br/><b>Here is a list of items changed:</b><br/>{changes}"
					};

					hr_notice.Send();
				}
			}
			catch (Exception)
			{
				//
			}

			return s;
		}


		public LabelValueInt CheckDuplicateSIN(string value)
		{
			var user = new NeMember(member_id);
			if (user.MemberTypeID != 4 && user.hrstatus_id > 2)
			{
				return bllToolbox.doSQL_Object<LabelValueInt>(
					@"SELECT member_id value,CONCAT(member_nickname , ' ' , member_lastname) label FROM member WHERE member_id!=@p0 and member_sin = @p1  LIMIT 1",
					member_id, value);
			}
			else
			{
				return null;
			}
		}

		public DataExtra CheckReportsTo(string value)
		{
			var user = new NeMember(member_id);
			var id = Convert.ToInt32(value);
			var iduser=new NeMember(id);
			var report_text = NeMember.get_reporting_line(id);
			if (value == "" || NeMember.Check_for_circular_org_chart(member_id,id))
			{
				return new DataExtra("failed", report_text);
			}
			else
			{
				return new DataExtra("success", report_text);
			}
		}

		public Task<DataExtra> PrintBarCodeAsync(int copy)
		{
			return Task.Run(() =>
			{
				try
				{
					n1_member.print_barcode_label(copy);
					return new DataExtra("success", null);

				}
				catch (Exception)
				{
					return new DataExtra("failed", null);
				}
			});
		}

	}
}