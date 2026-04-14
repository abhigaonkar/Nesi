using NESI.BLL.Base;
using NESI.BLL.Common.Shared;
using NESI.Common.Extensions;
using NESI.Common.Password;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NESI.BLL.Pages.ReleaseSystem
{
    public class PasswordEnhance : BLLBase
    {

        public string Generator_RollBack_Password_SQL_Script(member m)
        {
            return $@"Update member	set member_pass='{m.member_pass}',member_windows_password='{m.member_windows_password}'	where member_id={m.Member_ID};";
        }

        public string Generator_Update_Password_SQL_Script(member m)
        {
            var pass = "";
            try
            {
                pass = m.member_pass.IsValidBase64EncodedString() ? Encryption.DecryptPassword(m.member_pass) : m.member_pass;
            }
            catch
            {
                pass = m.member_pass;
            }
            var hashPassword = PasswordHasher.HashPassword(pass);
            return $@"Update member set member_pass='{hashPassword}',member_windows_password='{hashPassword}' where member_id={m.Member_ID};";
        }



        public DataString GenerateMemberScripts()
        {

            var sb_update = new StringBuilder();
            var sb_rollback = new StringBuilder();
            var connectString = _db.Database.Connection.ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectString);
            var server = builder.DataSource;

            sb_update.Append(
$@"
/*
    Generate Time:	{DateTime.Now}
    Server:			{server}
    Total Lines:	{_db.member.Count()}
*/
");

            sb_rollback.Append(
$@"
/*
    Generate Time:	{DateTime.Now}
    Server:			{server}
    Total Lines:	{_db.member.Count()}
*/
");
            foreach (var member in _db.member)
            {
                sb_update.AppendLine(Generator_Update_Password_SQL_Script(member));
                sb_rollback.AppendLine(Generator_RollBack_Password_SQL_Script(member));
            }

            return new DataString
            {
                Data = sb_update.ToString(),
                Data2 = sb_rollback.ToString()
            };
        }
    }
}