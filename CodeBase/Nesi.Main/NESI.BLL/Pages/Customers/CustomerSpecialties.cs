using System.Data;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerSpecialties : CustomerBase
	{
		public CustomerSpecialties(Employee user) : base(user)
		{
		}

		public CustomerSpecialties(Employee user, int cust_id) : base(user)
		{
			this.customer_id = cust_id;
		}

		public new DataTable Profile()
		{
			return bllToolbox.doSQL_dt(@"SELECT *,MEMBER_NAME(added_by) added_by_name FROM partner_skills WHERE `table` = 'customer' AND table_id = @p0",
				customer_id);
		}

		public DataExtra Save(DataIdString model)
		{
			if (model.id == 0)
			{
				bllToolbox.doSQL_void(@"INSERT INTO `partner_skills` (`skill`, `added_by`, `date_added`, `table`, `table_id`)
					VALUES (@p0,@p1,curdate(),'Customer',@p2)", model.value, UserId, customer_id);
			}
			else
			{
				bllToolbox.doSQL_void(@"update partner_skills set skill=@p0, date_added=curdate(), added_by=@p1 where id = @p2",
					model.value, UserId, model.id);
			}
			return new DataExtra()
			{
				Data = "Skill has been saved successfully.",
				Extra = Profile()
			};
		}

		public DataExtra Delete(int id)
		{
			bllToolbox.doSQL_void(@"DELETE from partner_skills where id = @p0", id);
			return new DataExtra()
			{
				Data = "Skill has been deleted successfully.",
				Extra = Profile()
			};
		}

	}
}