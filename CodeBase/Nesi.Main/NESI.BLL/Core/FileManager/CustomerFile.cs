using System.IO;
using System.Linq;
using nesi.core;

namespace NESI.BLL.Core.FileManager
{
	public class CustomerFile : NeFileBase
	{
		public override string BaseFolder => base.FileServer + $@"\TE\TE{CurrentUser.TaxEntityId}\customer_files";
		public override string BasePath => Path.Combine(BaseFolder, $@"C{customer.Customer_ID}-{customer.Customer_Number}-{Clean_filename(customer.Customer_Name)}\[B] - {Clean_filename(address.Addr1)}");

		private readonly NECustomer customer;
		private readonly NEAddress address;

		public CustomerFile(Employee.Employee user,int cust_id, int add_id) : base(user)
		{
			customer = new NECustomer(cust_id);
			address = new NEAddress(add_id);
			this.Validate_folder_contents();
		}

		public sealed override void Validate_folder_contents()
		{
			Verify_folders_exist(BasePath, "Accounts Receivables", "Sales",
				"Correspondance", "Equipment");
		}
	}
}