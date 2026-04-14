using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using NESI.BLL.Common.Shared;
using NESI.Data.Entities;
using NESI.DTO.Models.Core;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Core.User
{
	public abstract partial class User : IDisposable
	{

        public bool IsLdapUser { get; protected set; }
		/// for contact from contact tables
		/// </summary>
		public bool isDeveloper { get; protected set; }
		protected readonly NESIMySQL Db = new NESIMySQL();
		/// <summary>
		/// user login name
		/// for employe from member table member_user
		public string UserName { get; set; }
		public string VisibleBusinessUnits { get; set; }
		public string VisibleTaxEntities { get; set; }
		public User ExtraUser { get; set; }
		public string ExtraType { get; set; }

		public virtual string SyncLdap(string password)
		{
			return "";
		}

		/// <summary>
		/// User's password
		/// </summary>
		public string Password { get; set; }

		public string Email { get; set; }
		/// <summary>
		/// If user is Active
		/// </summary>
		public bool IsActive { get; set; }

		public string Guid { get; set; }
        /// <summary>
        /// if user is a contact user?(vendor or customer) 
        /// </summary>
        /// <remarks>This is not correct OOD, leaving for backward compatibility</remarks>
        public virtual bool IsContact => false;

	    public bool FvrPassed { get; set; }
		public bool HasFvr { get; set; }
		public bool save_global_layout { get; set; }

        /// <summary>
        ///user id, member_id or contact_id 
        /// </summary>
        public int Id { get; set; }
		public bool force_beta { get; set; }

		public string Status { get; set; }

		/// <summary>
		/// Token expired time, assigned when user login to get token
		/// </summary>
		public DateTime ExpiredTime { get; set; }

		protected DateTime _pingTime;
		public DateTime PingTime
		{
			get => _pingTime > LastActiveTime ? _pingTime : LastActiveTime;
			set => _pingTime = value;
		}

		/// <summary>
		/// Token issued time, assigned when user login to get token
		/// </summary>
		public DateTime IssueTime { get; set; }

		public DateTime LastActiveTime { get; set; }

		public void ResetLastActiveTime(HttpActionContext actionContext)
		{
			var url = actionContext.Request.RequestUri.AbsolutePath.ToLower();
			if (url.IndexOf("api/Layout/AllActive".ToLower(), StringComparison.Ordinal) < 0 &&
				url.IndexOf("api/CurrentUser/Ping".ToLower(), StringComparison.Ordinal) < 0
				)
			{
				LastActiveTime = DateTime.Now;
			}
		}
		/// <summary>
		/// user's fullname show in active user status window
		/// </summary>
		public string FullName { get; set; }

        public DateTime EmploymentStartDate { get; set; }

		/// <summary>
		/// user's business unit shows in active user status window
		/// </summary>
		public string BusniessUnitName { get; set; }

		public int BusinessUnitId { get; set; }

		public DTO.Models.Core.BusinessUnit BusinessUnit { get; set; }
		public TaxEntity TaxEntity { get; set; }

		public int TaxEntityId { get; set; }

		public int DefaultPageId { get; set; }
        
        /// <summary>
        /// user's active time when user online
        /// </summary>
        public string ActiveTime => OnlineUser.CaculateActiveTime(LastActiveTime);
		public string Company { get; set; }

		public string[] VisibleBusinessUnitIdList { get; set; }
		public List<DTO.ViewModels.Core.BusinessUnitDropDownList> VisibleBusinessUnitList { get; set; }
		public DTO.ViewModels.Core.LabelValueInt[] VisibleBusinessUnitLabelValueList => VisibleBusinessUnitList.Select(x => new LabelValueInt() { Label = x.ddl_name, Value = x.Id }).ToArray();

		public string Gender { get; set; }

		public string Photo { get; set; }

		/// <summary>
		/// if the token expired?
		/// </summary>
		public bool IsExpired => ExpiredTime < DateTime.Now;


		public long ExpireSecond => Convert.ToInt64(new TimeSpan(ExpiredTime.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks)
			.TotalSeconds);

		/// <summary>
		/// User's privileges
		/// </summary>
		public List<Privilege> Privileges { get; set; }


		public int MemberDefaultPage { get; set; }
		public DateTime PasswordDateChange { get; set; }

		/// <summary>
		/// User's page privilege
		/// </summary>
		public List<Page> Pages { get; set; }

        protected User(int userId)
		{
			this.Id = userId;
			SetIssueTime();
		}

	    protected User(string username)
	    {
	        this.UserName = username;
	        SetIssueTime();
	    }
        
        public bool IsVisibleBusinessUnitId(int id)
		{
			return VisibleBusinessUnitIdList.Contains(id.ToString());
		}

		/// <summary>
		/// set issuetime and expiredtime when user created
		/// </summary>
		public void SetIssueTime()
		{
			try
			{
				this.ExpiredTime = DateTime.Now.AddMinutes(NESI.BLL.Common.Shared.Configuration.TokenExpiredTime);
				this.IssueTime = DateTime.Now;
				this.LastActiveTime = this.IssueTime;
			}
			catch (Exception)
			{
				// Console.WriteLine(e);
			}
		}

		protected DTO.Models.Users.Member GetProfileByUserID(int id)
		{
			return AutoMapper.Mapper.Map<DTO.Models.Users.Member>(Db.member.FirstOrDefault(l => l.Member_ID == id));
		}

		protected DTO.Models.Users.Member GetProfileByUserName(string name)
		{
			return AutoMapper.Mapper.Map<DTO.Models.Users.Member>(Db.member.FirstOrDefault(l => l.Member_User == name));
		}



		public void Dispose()
		{
			Db?.Dispose();
		}



	}
}