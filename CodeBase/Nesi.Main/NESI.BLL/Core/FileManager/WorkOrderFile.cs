using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using nesi.core;
using NESI.BLL.Common.Cache;
using NESI.DTO.Models.Core;

namespace NESI.BLL.Core.FileManager
{
	public class WorkOrderFile : NeFileBase
	{
		private readonly DTO.Models.Core.WoProg _wo;
		private readonly DTO.Models.Core.BusinessUnit _bu;


		public WorkOrderFile(int WoId)
		{
			_wo = new Ne2WOProg(WoId).Entity;
			_bu = Global.BusinessUnit.GetValue(Convert.ToInt32(_wo.business_unit_id));
			this.Validate_folder_contents();
		}

		public override string BaseFolder => base.FileServer + $@"\TE\TE{_bu.tax_entity_id}\ProjectFolders\";

		public override string BasePath => Path.Combine(BaseFolder, $"WO{_wo.woprog_id}-{_wo.woprog_bvwo}-{Clean_filename(_wo.woprog_customername)}");

		
		public sealed override void Validate_folder_contents()
		{
			var process_quote = false;
			var quote_path = "";
			var wo_quote_path = "";
			var fsDirectoryInfo = new DirectoryInfo(base.FileServer);
			if(!fsDirectoryInfo.Exists) return;
			if (_wo.woprog_quoteid != "" && Convert.ToInt32(_wo.woprog_quoteid) > 100000)
			{
				quote_path = string.Format(base.FileServer + @"\quote_store\{0}\", _wo.woprog_quoteid.Substring(0, 6))
					.Substring(0, 20);
				wo_quote_path = Path.Combine(BasePath, $"From Quote ({_wo.woprog_quoteid.Substring(0, 6)})");
				process_quote = true;
			}
			// Make sure the work order folder exists
			CreateFolder();

			// Check if quote folder already exists
			if (!process_quote || !Directory.Exists(quote_path)) return;
			// If so check if folder has contents
			if (Directory.GetFiles(quote_path).Length > 0)
			{
				Directory.Move(quote_path, wo_quote_path);
			}
		}

		public override void CreateFolder()
		{
			Verify_folders_exist(BasePath,	NeFiles.WoFolder.AccountsReceivables, 
											NeFiles.WoFolder.PartSpecs,
											NeFiles.WoFolder.Correspondance, 
											NeFiles.WoFolder.Equipment, 
											NeFiles.WoFolder.PLCandHMI, 
											NeFiles.WoFolder.Schematics,
											NeFiles.WoFolder.Pictures,
											NeFiles.WoFolder.Safety,
											NeFiles.WoFolder.SignatureFiles,
											NeFiles.WoFolder.ExpenseReceipts,
											NeFiles.WoFolder.CreditCardReceipts,
											NeFiles.WoFolder.CustomerPurchaseOrder
											);
		}

	

	}
}