using System.Linq;
using NESI.BLL.Base;

namespace NESI.BLL.Core
{
	public class Ne2Payroll : BLLBase
	{
		public int Working_pay_period()
		{
			return _db.Database.SqlQuery<int>("CALL _payperiod").FirstOrDefault();
		}
		public int current_pay_period()
		{
			return Working_pay_period();
		}
	}
}
