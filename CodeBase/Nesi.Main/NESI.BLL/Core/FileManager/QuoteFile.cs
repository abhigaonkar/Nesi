using System;
using System.IO;
using NESI.BLL.Common.Cache;

namespace NESI.BLL.Core.FileManager
{
	public class QuoteFile : NeFileBase
	{
		private readonly DTO.Models.Core.Quote_Master _quote;
		private readonly DTO.Models.Core.BusinessUnit _bu;

		public override string BaseFolder => base.FileServer + $@"\TE\TE{_bu.tax_entity_id}\quote_store\";

		public override string BasePath => Path.Combine(BaseFolder, _quote.quote_id.ToString());


		public QuoteFile(int quoteId,int revision)
		{
			_quote = new Ne2Quote(quoteId, revision).Entity;
			_bu = Global.BusinessUnit.GetValue(Convert.ToInt32(_quote.business_unit_id));
			this.Validate_folder_contents();
		}

		public sealed override void Validate_folder_contents()
		{
			if (_quote.expected_value >= _bu.quote_level_3_start)
			{
				CreateFolder();
			}
			else
			{
				Verify_folders_exist(BasePath);
			}
		}

		public override void CreateFolder()
		{
			Verify_folders_exist(BasePath, "Finance Recon", "Customer Recon", "Field Recon", "Market Recon", "Post Mortem");
		}
	}
}