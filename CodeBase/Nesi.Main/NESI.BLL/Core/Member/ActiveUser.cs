using System;
using Newtonsoft.Json;

namespace NESI.BLL.Core.Member
{
	public class ActiveUser
	{
        
	    [JsonProperty("id")]
		public int Id { get; set; }
	    [JsonProperty("name")]
		public string Name { get; set; }
	    [JsonProperty("fullname")]
		public string FullName { get; set; }
	    [JsonProperty("companyName")]
		public string CompanyName { get; set; }
	    [JsonProperty("businessUnitId")]
		public int BusinessUnitId { get; set; }
	    [JsonProperty("BusinessUnitName")]
		public string BusinessUnitName { get; set; }
	    [JsonProperty("issueTime")]
		public DateTime IssueTime { get; set; }
	    [JsonProperty("activeTime")]
		public string ActiveTime { get; set; }
	    [JsonProperty("expires")]
		public DateTime Expires { get; set; }
	    [JsonProperty("isExpired")]
		public bool IsExpired { get; set; }
	    [JsonProperty("expireSecond")]
		public long ExpireSecond { get; set; }
	    [JsonProperty("photo")]
		public string Photo { get; set; }
	    [JsonProperty("gender")]
		public string Gender { get; set; }
	    [JsonProperty("logo")]
		public string Logo { get; set; }
	    [JsonProperty("icon")]
		public string Icon { get; set; }
	    [JsonProperty("forceChangePassword")]
		public bool ForceChangePassword { get; set; }
	    [JsonProperty("fvrPassed")]
		public bool FvrPassed { get; set; }
	    [JsonProperty("hasFvr")]
		public bool HasFvr { get; set; }
	    [JsonProperty("tax_entity_id")]
		public int tax_entity_id { get; set; }
	    [JsonProperty("isLdapUser")]
		public bool IsLdapUser { get; set; }
	    [JsonProperty("force_beta")]
		public bool force_beta { get; set; }
	    [JsonProperty("save_global_layout")]
		public bool save_global_layout { get; set; }
	    [JsonProperty("is_developer")]
		public bool is_developer { get; set; }

        [JsonProperty("show_daily_approval")]
        public bool show_daily_approval { get; set; }

        /// <summary>
        /// Need constructor for serialization purposes
        /// </summary>
	    public ActiveUser()
	    {
	        
	    }
		public ActiveUser(User.User user)
		{
			this.Id = user.Id;
			this.FullName = user.FullName;
			this.IssueTime = user.IssueTime;
			this.ActiveTime = user.ActiveTime;
			this.IsExpired = user.IsExpired;
			this.Expires = user.ExpiredTime;
			this.ExpireSecond = user.ExpireSecond;
			this.Photo = user.Photo;
			this.Gender = user.Gender;
			this.Logo = user.BusinessUnit.logo_file;
			this.Icon = user.BusinessUnit.logo_file.Replace(".png", "-ico.png");
			this.BusinessUnitId = user.BusinessUnitId;
			this.BusinessUnitName = user.BusniessUnitName;
			this.ForceChangePassword = user.ForceChangePassword;
			this.FvrPassed = user.FvrPassed;
			this.HasFvr = user.HasFvr;
			this.tax_entity_id = user.TaxEntityId;
			this.IsLdapUser = user.IsLdapUser;
			this.force_beta = user.force_beta;
			this.save_global_layout = user.save_global_layout;
			this.is_developer = user.isDeveloper;
            this.show_daily_approval = user.BusinessUnit.show_daily_approval;

			if (user.IsContact)
			{
				this.CompanyName = user.Company;
				this.Name = this.FullName + $"({this.CompanyName})";
				this.force_beta = false;
				this.IsLdapUser = false;
			}
			else
			{
				this.Name = user.FullName + "(" + user.BusniessUnitName + ")";
			}
		}
	}
}