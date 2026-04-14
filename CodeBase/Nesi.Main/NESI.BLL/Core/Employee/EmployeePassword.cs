using NESI.BLL.Common.Shared;
using NESI.Common.Exceptions;
using NESI.Common.Extensions;
using NESI.Data.Entities;
using System;
using System.Data.Entity.Infrastructure;
using System.DirectoryServices;

namespace NESI.BLL.Core.Employee
{
    public partial class Employee
    {
        private void SetPassword()
        {
            Password = EmployeeProfile.member_pass;
        }

        public override string SyncLdap(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return "Password is required.";
            }
            if (EmployeeProfile.member_ldap_user != null || EmployeeProfile.member_ldap_user != "")
            {
                if (!UserManager.IsCorrectPasswordForUser(this, password))
                {
                    return "Password is incorrect!";
                }
                var adAdmin = new DirectoryEntry(Common.Shared.Configuration.LdapPath, Common.Shared.Configuration.LdapUser,
                     Common.Shared.Configuration.LdapPass, AuthenticationTypes.Secure);

                var adSearch = new DirectorySearcher(adAdmin) { Filter = $"(SAMAccountName={EmployeeProfile.member_ldap_user})" };
                var adResult = adSearch.FindOne();
                if (adResult != null)
                {
                    var adUser = adResult.GetDirectoryEntry();
                    adUser.Invoke("SetPassword", password);
                    adUser.CommitChanges();
                    return "Password has been synced successfully.";
                }

                return "Couldn't update your remote desktop / email password.";
            }

            return "User not associated with LDAP";
        }

        protected sealed override void SaveHashedPassword(string password, DateTime curDateTime)
        {
            if (password == null || !password.IsValidBase64EncodedString())
                throw new ArgumentException(@"Invalid password", nameof(password));
            try
            {
                using (var db = new NESIMySQL())
                {
                    var memberToUpdate = db.member.Find(Id);
                    if (memberToUpdate == null)
                    {
                        throw new NesiException("Couldn't update your password.");
                    }
                    memberToUpdate.member_pass = password;
                    memberToUpdate.PasswordDateChange = curDateTime;
                    memberToUpdate.request_password_reset_flag = false;
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                Logger.Error($"Unable to update employee password {Id}", dbUpdateException);
                throw new NesiException("Unable to update employee password", dbUpdateException);
            }
        }
    }
}