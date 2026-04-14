using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using NESI.Common.Exceptions;
using NESI.Common.Extensions;
using NESI.Data.Entities;
using NESI.DTO.Models.Core;
using NESI.DTO.ViewModels.CurrentUser;

namespace NESI.BLL.Core.Member
{
	public abstract class Contact : User.User
	{
		public DTO.Models.Users.Contact ContactProfile { get; }
		public string ContactType { get; set; }

		public string BvNumber { get; set; }
		public int ContactId { get; set; }

	    public override bool IsContact => true;

	    protected Contact(string username) : base(username)
		{
			ContactProfile = AutoMapper.Mapper.Map<DTO.Models.Users.Contact>(Db.contact.FirstOrDefault(l => l.Contact_Email == username));
			GetContactValue();
		}

		protected Contact(int memberId) : base(memberId)
		{
			ContactProfile = AutoMapper.Mapper.Map<DTO.Models.Users.Contact>(Db.contact.FirstOrDefault(l => l.Contact_Cust_ID == memberId));
			GetContactValue();
		}

		protected Contact(DTO.Models.Users.Contact con) : base(con.Contact_NesiMemberID)
		{
			ContactProfile = con;
			GetContactValue();
		}

		protected void GetContactValue()
		{
			if (ContactProfile == null) return;
			this.Password = ContactProfile.Contact_Password;
			this.Email = ContactProfile.Contact_Email;
			this.IsActive = (ContactProfile.Contact_Login_Enabled == 1);
			this.ContactType = ContactProfile.contact_type;
			this.Id = ContactProfile.Contact_NesiMemberID;
			this.PasswordDateChange = DateTime.Now;
			this.MemberDefaultPage = ContactProfile.contact_page_id;
			this.Status = ContactProfile.Contact_Status;
			this.UserName = ContactProfile.Contact_Email;
			this.ContactId = ContactProfile.Contact_ID;
			this.FullName = ContactProfile.Contact_Name;
			this.Pages = GetPages();
			this.Privileges = GetPrivileges();
			this.DefaultPageId = 1;
			this.ForceChangePassword = !ContactProfile.contact_password_set;
			this.FvrPassed = true;
			this.HasFvr = false;
			//this.Profile = ContactProfile;
		}

		/// <summary>
		/// get contact's page privilege
		/// </summary>
		/// <returns></returns>
		private List<Page> GetPages()
		{
			var pages = (from m in Db.contactpage
						 join p in Db.page on m.ContactPage_Page_ID equals p.page_id
						 where m.ContactPage_contact_id == ContactId && (bool)m.active
						 select p).ToList();
			return AutoMapper.Mapper.Map<List<Page>>(pages);
		}

		/// <summary>
		/// get contact's privilege
		/// </summary>
		/// <returns></returns>
		private List<Privilege> GetPrivileges()
		{
			var privilege = (from m in Db.contactpageprivilege
							 join p in Db.privilege on m.ContactPagePrivilege_Privilege_ID equals p.Privilege_ID
							 where m.ContactPagePrivilege_Contact_ID == ContactId && (bool)m.active
							 select p).ToList();
			return AutoMapper.Mapper.Map<List<Privilege>>(privilege);

		}

	    protected sealed override void SaveHashedPassword(string password, DateTime curDateTime)
	    {
            if(password == null || !password.IsValidBase64EncodedString()) 
                throw new ArgumentException(@"Invalid password", nameof(password));

            try
            {
                using (var db = new NESIMySQL())
                {
                    var contactToUpdate = db.contact.SingleOrDefault(c => c.Contact_NesiMemberID == Id);
                    if (contactToUpdate == null)
                    {
                        throw new NesiException("Couldn't update your password. Cannot find contact.");
                    }

                    contactToUpdate.Contact_Password = password;
                    contactToUpdate.contact_password_set = true;
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                Logger.Error($"Unable to update contact password {Id}", dbUpdateException);
                throw new NesiException("Unable to update contact password", dbUpdateException);
            }
	    }
	}
}