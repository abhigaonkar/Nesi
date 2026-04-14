using NESI.BLL.Common.Cache;
using NESI.BLL.Layout.Banner;
using NESI.Data.Entities;
using NESI.DTO.Models.Core;
using NESI.DTO.ViewModels.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NESI.BLL.Core.Employee
{
    public partial class Employee : User.User
	{
		public readonly DTO.Models.Users.Member EmployeeProfile;
		
		public string AssociatedBusinessUnits { get; set; }
		public int BranchManagerId { get; set; }
		public DTO.Models.Users.Member BranchManager { get; set; }

		public DTO.Models.Core.MemberType MemberType { get; set; }
		public Currency Currency { get; set; }
		public int months_with_company { get; set; }
		public DTO.Models.Users.Member ReportToManager { get; set; }

		// public NESI.DTO.Models.Member EmployeeProfile { get; set; }

		public Employee(string username) : base(username)
		{
			//if username include @, then it is a contact.
			if (username.Contains("@"))
			{
				throw new Exception("Contact can not be initialized as an employee.");
			}
			EmployeeProfile = GetProfileByUserName(username);
			GetValue();
		}


		public Employee(int id) : base(id)
		{
			EmployeeProfile = GetProfileByUserID(id);
			GetValue();
		}



		/// <summary>
		/// get all user's property value from profile
		/// </summary>
		private void GetValue()
		{
			var bu = new BLL.Core.BusinessUnit(this);

			if (EmployeeProfile == null) return;
			this.IsLdapUser = Common.Shared.UserManager.IsLdapUser(EmployeeProfile);
			this.UserName = EmployeeProfile.Member_User;
			this.Email = EmployeeProfile.member_neemail;
			this.IsActive = (EmployeeProfile.Member_Status == "Active");
			this.Id = EmployeeProfile.Member_ID;
			this.Pages = GetPages();
			this.Privileges = GetPrivileges();
			this.FullName = EmployeeProfile.member_fullname;
			this.BusinessUnitId = EmployeeProfile.business_unit_id;
			this.BusinessUnit = Global.BusinessUnit.GetValue(EmployeeProfile.business_unit_id.ToString());
			this.TaxEntity = Global.TaxEntity.GetValue(this.BusinessUnit.tax_entity_id.ToString());
			this.TaxEntityId = this.BusinessUnit.tax_entity_id;
			this.BusniessUnitName = this.BusinessUnit.ddl_Name;
			this.VisibleBusinessUnits = bu.GetVisibleBusinessUnits();
			this.VisibleTaxEntities = bu.GetVisibleTaxEntities();
			this.VisibleBusinessUnitIdList = !string.IsNullOrWhiteSpace(this.VisibleBusinessUnits) ? this.VisibleBusinessUnits.Split(',') : new[] { "" };
			this.AssociatedBusinessUnits = bu.GetAssociatedBusinessUnits();
			this.MemberDefaultPage = EmployeeProfile.member_default_page.GetValueOrDefault();
			this.PasswordDateChange = EmployeeProfile.PasswordDateChange.GetValueOrDefault();
			this.Status = EmployeeProfile.Member_Status;
			this.DefaultPageId = EmployeeProfile.member_default_page ?? 1;
			if (this.DefaultPageId == 0) this.DefaultPageId = 1;
			this.ForceChangePassword = DateTime.Now.Subtract(this.PasswordDateChange).Days >
															 BLL.Common.Shared.Configuration.ForcedChangePasswordDays;
			this.FvrPassed = new Fvr(this).Passed();
			this.VisibleBusinessUnitList = (new BLL.Core.BusinessUnit(this)).GetVisibileBusinessUnitDropDownLists();
			this.Company = this.BusniessUnitName;
			this.HasFvr = (new Fvr(this)).GetFvrs()?.Length > 0;
			this.BranchManagerId = bu.GetBranchManagerId();
			this.ReportToManager = GetProfileByUserID(EmployeeProfile.reports_to.GetValueOrDefault());
			this.BranchManager = BranchManagerId == 0 ? this.ReportToManager : GetProfileByUserID(BranchManagerId);
            
			this.MemberType = new BLL.Core.MemberType(EmployeeProfile.member_membertype_id).Entity;
			try
			{
				this.Currency = (Currency)Enum.Parse(typeof(Currency), EmployeeProfile.member_country);
			}
			catch (Exception)
			{
				this.Currency = Currency.CAN;
			}
			this.months_with_company = Common.Shared.DateTimeTools.MonthDifference(EmployeeProfile.member_startdate.GetValueOrDefault(), DateTime.Now);
			this.force_beta = EmployeeProfile.force_beta.GetValueOrDefault(false);
			this.save_global_layout = AuthenticatedForPrivilege(195);
			this.isDeveloper = AuthorizePage(166);
			SetPassword();


		}

		public double GetWage()
		{
			var db = new NESIMySQL();
			return db.Database.SqlQuery<double>(@"SELECT get_wage(@p0)", Id).FirstOrDefault();
		}

		public int CurrentVacationAmount
		{
			get
			{
				var startDate = EmployeeProfile.member_startdate.GetValueOrDefault();

				var monthDiff = Math.Abs(12 * (DateTime.Now.Year - startDate.Year) + DateTime.Now.Month - startDate.Month);

				if (monthDiff >= EmployeeProfile.vacation_interval_3)
				{
					return Convert.ToInt32(EmployeeProfile.vacation_amount_3);
				}
				else if (monthDiff >= EmployeeProfile.vacation_interval_2)
				{
					return Convert.ToInt32(EmployeeProfile.vacation_amount_2);
				}
				else if (monthDiff >= EmployeeProfile.vacation_interval_1)
				{
					return Convert.ToInt32(EmployeeProfile.vacation_amount_1);
				}
				else
				{
					return 0;
				}

			}
		}

		public bool IsSupervisor(int memberId)
		{
			var db = new NESIMySQL();
			return db.Database.SqlQuery<int>(@"SELECT IS_SUPERVISOR(@p1, @p0)", Id, memberId).FirstOrDefault() == 1;
		}

		public bool IsEstimatingManager(int memberId)
		{

			var db = new NESIMySQL();
			return db.Database.SqlQuery<int>(@"SELECT is_estimating_manager(@p0)", memberId).FirstOrDefault() > 0;
		}


		/// <summary>
		/// get employee's page privilege
		/// </summary>
		/// <returns></returns>
		private List<DTO.Models.Core.Page> GetPages()
		{
			var pages = (from m in Db.memberpage where m.memberpage_member_id == Id && (bool)m.active select m.page)
				.ToList();
			return AutoMapper.Mapper.Map<List<DTO.Models.Core.Page>>(pages);
		}

		/// <summary>
		/// get employee's privilege
		/// </summary>
		/// <returns></returns>
		private List<DTO.Models.Core.Privilege> GetPrivileges()
		{
			var privilege = (from m in Db.memberpageprivilege where m.MemberPagePrivilege_Member_ID == Id && (bool)m.active select m.privilege).ToList();
			return AutoMapper.Mapper.Map<List<DTO.Models.Core.Privilege>>(privilege);

		}
	}
}