using System.IO;
using System.Linq;
using nesi.core;

namespace NESI.BLL.Core.FileManager
{
	public class MemberFile : NeFileBase
	{
		public override string BaseFolder => base.FileServer + $@"\TE\TE{taxEntityId}\member_files";
		public override string BasePath => Path.Combine(BaseFolder, $@"M{member_id}");

		private readonly int member_id;
		private readonly int taxEntityId;

		public MemberFile(int member_id)
		{
			this.member_id = member_id;
			var member = new NeMember(member_id);
			this.taxEntityId = member.business_unit.tax_entity_id;
			this.Validate_folder_contents();
		}

		public sealed override void Validate_folder_contents()
		{
			Verify_folders_exist(BasePath, "Certificates", "Resumes",
				"Tests", "HR");
		}

		public void CopyFromApplicantFiles()
		{
			var appid = bllToolbox.doSQL_int(@"select ifnull((Select id from applicants  where becomes_memberid =@v0 limit 1),0) ", member_id);
			if (appid == 0) return;

			var appfile = new ApplicantFile(appid);
			var dirs = Directory.GetDirectories(appfile.BasePath, @"*", SearchOption.AllDirectories);
			foreach (var d in dirs) // loop through each applicant folder
			{
				var dir = Path.GetFileName(d);
				var files = Directory.GetFiles(d);
				if (string.IsNullOrEmpty(dir)) continue;
				var member_sub_dir_path = Path.Combine(BasePath, dir);
				Verify_folders_exist(member_sub_dir_path);
				foreach (var file in files) // copy files into the member folder
				{
					var name = Path.GetFileName(file);
					var dest_path = Path.Combine(member_sub_dir_path, name);
					File.Copy(file, dest_path, true);
				}
			}
		}
	}
}
