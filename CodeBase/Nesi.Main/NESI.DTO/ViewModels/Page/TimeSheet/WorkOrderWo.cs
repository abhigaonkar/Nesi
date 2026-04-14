// ReSharper disable InconsistentNaming


using System.Collections.Generic;

namespace NESI.DTO.ViewModels.Page.TimeSheet
{

    public class Scope
    {
        public int id { get; set; }
        public string name { get; set; }
    }
	public class WorkOrderWo
	{
		public string descript { get; set; }
		public int woprog_id { get; set; }
		public string wo { get; set; }
		public string customername { get; set; }
		public string status { get; set; }
		public int customer_id { get; set; }
		public string customer_number { get; set; }
        public List<Scope> scopes { get; set; }

		public string Label => getFullDescription();
		public int Value => woprog_id;

        public string CutPO { get; set; }

        public int address_id { get; set; }

        internal string getFullDescription()
		{
			if (woprog_id== 0 || descript == null) return "";
			var description = descript.ToString().TrimEnd();
			var custname = customername.ToString().TrimEnd();
			if (custname.Length > 30)
			{
				custname = custname.Substring(0, 30);
			}
			return $"{wo} - {custname} - {description.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ")} - {status}";
		}
	}

}