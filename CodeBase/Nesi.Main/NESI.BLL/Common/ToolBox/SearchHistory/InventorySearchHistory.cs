using NESI.BLL.Base;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Common.ToolBox.SearchHistory
{
	public class InventorySearchHistory : ProfileCategoryBase
	{
		public InventorySearchHistory(Employee user) : base(user, "InventorySearchHistory", 30)
		{

		}
	}
}