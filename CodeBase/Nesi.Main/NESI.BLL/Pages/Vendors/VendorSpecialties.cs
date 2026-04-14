using System.Data;
using NESI.BLL.Core.Employee;
using NESI.BLL.Pages.Customers;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorSpecialties : VendorEditBase
	{
		public VendorSpecialties(Employee user, int id) : base(user, id)
		{
		}

		public new object Profile()
		{
			return new
			{
				list = bllToolbox.doSQL_dt(@"SELECT *,MEMBER_NAME(added_by) added_by_name FROM partner_skills WHERE `table` = 'Vendor' AND table_id = @p0",
				vendor_id)
			};
		}

		public DataExtra Save(DataIdString model)
		{
			if (model.id == 0)
			{
				bllToolbox.doSQL_void(@"INSERT INTO `partner_skills` (`skill`, `added_by`, `date_added`, `table`, `table_id`)
					VALUES (@p0,@p1,curdate(),'Vendor',@p2)", model.value, UserId, vendor_id);
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