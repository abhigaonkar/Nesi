using System.IO;
using System.Linq;
using nesi.core;

namespace NESI.BLL.Core.FileManager
{
	public class VendorFile : NeFileBase
	{
		public override string BaseFolder => base.FileServer + $@"\TE\TE{CurrentUser.TaxEntityId}\customer_files";
		public override string BasePath => Path.Combine(BaseFolder, $@"V{vendor.Vendor_ID}-{vendor.vendor_number}-{Clean_filename(vendor.Vendor_Name)}");

		private readonly NEVendor vendor;

		public VendorFile(Employee.Employee user,int id) : base(user)
		{
			vendor = new NEVendor(id);
			Validate_folder_contents();
		}

		public sealed override void Validate_folder_contents()
		{
			Verify_folders_exist(BasePath, "Accounts Payable", "Contracts",
				"Correspondance", "Line Cards");
		}
	}
}