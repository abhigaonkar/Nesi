using System;

namespace NESI.DTO.ViewModels.Page.Customers
{
	public class CustomerRate : CustomerBase
	{
		public int id { get; set; }
		public int business_unit_id { get; set; }
		public double reg { get; set; }
		public DateTime from_date { get; set; }
		public DateTime to_date { get; set; }

        public double ot { get; set; }
        public double dt { get; set; }
        public double regsp { get; set; }
        public double otsp { get; set; }
        public double dtsp { get; set; }
        public Boolean overrideflag { get; set; }
    }
}