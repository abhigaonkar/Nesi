using System;
using System.IO;
using NESI.BLL.Common.Cache;

namespace NESI.BLL.Core.FileManager
{
	public class WorkOrderScannedFile : NeFileBase
	{

		private readonly DTO.Models.Core.WoProg _wo;
		private readonly DTO.Models.Core.BusinessUnit _bu;

		public override string BaseFolder => base.FileServer + $@"\TE\TE{_bu.tax_entity_id}";

		public override string BasePath => Path.Combine(BaseFolder, "WOs");

		public WorkOrderScannedFile(int WoId)
		{
			_wo = new Ne2WOProg(WoId).Entity;
			_bu = Global.BusinessUnit.GetValue(Convert.ToInt32(_wo.business_unit_id));
		}

		public string GetPath(string file)
		{
			return Path.Combine(BasePath, file);
		}
	}
}