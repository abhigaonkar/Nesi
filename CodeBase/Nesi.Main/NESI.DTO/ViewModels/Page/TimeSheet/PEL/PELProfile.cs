using System.Collections.Generic;
using System.Data;
using NESI.Data.Entities;

namespace NESI.DTO.ViewModels.Page.TimeSheet.PEL
{
	public class PELProfile
	{
		public List<vacation> historyList { get; set; }
		public PELSummary summary { get; set; }

	}
}