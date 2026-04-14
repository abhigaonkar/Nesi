using System.IO;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkOrderFileBase : BLLBase
	{
		public int business_unit_id { get; set; }

		public WorkOrderFileBase(Employee user) : base(user)
		{
			
		}

	

		public LabelValueString[] GetNameList()
		{
			return bllToolbox.doSQL_Array<LabelValueString>(@"
SELECT woprog_bvwo value, 
CONCAT(woprog_bvwo,' - ', woprog_customername) label 
FROM woprog 
WHERE woprog_closedatetime IS NULL 
AND woprog_status='Open' 
AND woprog_bvwo != 'Not Entered' 
AND woprog_hold = 0 
AND business_unit_id=@v0  
ORDER BY WOProg_CustomerName, WOProg_BVWO
", business_unit_id);
		}
	}
}