using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkOrderMainTabs : WorkOrderEdit
	{
		public WorkOrderMainTabs(Employee user, int buid, string strWo) : base(user, buid, strWo)
		{

		}

		public object Profile()
		{
			
			return new
			{

			};
		}
	}
}