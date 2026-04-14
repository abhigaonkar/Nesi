using System.IO;
using System.Linq;
using nesi.core;

namespace NESI.BLL.Core.FileManager
{
	public class ApplicantFile : NeFileBase
	{
		public override string BaseFolder => base.FileServer + $@"\TE\TE{taxEntityId}\applicant_files";
		public override string BasePath => Path.Combine(BaseFolder, $@"A{applicant_id}");

		private readonly int taxEntityId;
		private readonly int applicant_id;

		public ApplicantFile(int applicant_id)
		{
			this.applicant_id = applicant_id;
			var a = new NeApplicant(applicant_id);
			taxEntityId = a.business_unit.tax_entity_id;
			this.Validate_folder_contents();
		}

		public sealed override void Validate_folder_contents()
		{
			Verify_folders_exist(BasePath, "Certificates", "Resumes",
				"Tests", "HR");
		}
	}
}