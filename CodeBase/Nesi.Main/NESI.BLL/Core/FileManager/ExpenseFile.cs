using System.IO;

namespace NESI.BLL.Core.FileManager
{
	public class ExpenseFile : NeFileBase
	{
		public override string BaseFolder => base.FileServer + $@"\TE\TE{CurrentUser.TaxEntityId}\expense_receipts";
		public override string BasePath => Path.Combine(BaseFolder, "");


		public ExpenseFile(Employee.Employee user): base(user)
		{
			base.Validate_folder_contents();
		}

	
	}
}