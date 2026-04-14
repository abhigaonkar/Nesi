using System.Linq;

namespace NESI.BLL.Core.Member
{
	public class UserFactory
	{
		private readonly Data.Entities.NESIMySQL _db;

		public UserFactory()
		{
			_db = new Data.Entities.NESIMySQL();
		}

		public User.User GetUser(string userName)
		{
			return userName.Contains("@") ? (User.User)GetContact(userName) : new Employee.Employee(userName);
		}

		public User.User GetUser(int memberId)
		{
			return memberId > 100000 ? (User.User)GetContact(memberId) : new Employee.Employee(memberId);
		}

		public Contact GetContact(string userName)
		{
			var con = _db.contact.FirstOrDefault(x => x.Contact_Email == userName);
			return con == null ? null : GetContact(con);
		}

		public Contact GetContact(int memberId)
		{
			var con = _db.contact.FirstOrDefault(x => x.Contact_NesiMemberID == memberId);
			return con == null ? null : GetContact(con);
		}

		public static Contact GetContact(Data.Entities.contact con)
		{
			var conModel = AutoMapper.Mapper.Map<DTO.Models.Users.Contact>(con);
			switch (con.contact_type)
			{
				case "Customer":
					return new Customer(conModel);
				case "Vendor":
					return new Vendor(conModel); 
				default:
					return null;
			}
		}
	}
}