using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.NewLaborRates
{
    public interface INewLaborRate
    {
        List<GenericLaborRateRecord> GetNewLaborRateList(GetNewLaborRateListParameter parameter);
        UpdateResult CreateNewLaborRate(UpdateParameter parameter);
        UpdateResult DeleteNewLaborRate(DeleteParameter parameter);
    }

    public class GetNewLaborRateListParameter
    {
        public int businessUnitId { get;set; }
        public int customerId { get; set; }
    }

    /*
    public class NewLaborRateRecord
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int based_chargeout_id { get; set; }
        public double charegout { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public int address_id { get; set; }

        public int paytype_id { get; set; }
        public int business_unit { get; set; }
        public int membertype_id { get; set; }
        public int chargeout_id { get; set; }
    }
    */


    public class GenericLaborRateRecord
    {
        public int customer_id { get; set; }
        public int business_unit { get; set; }

        public int paytype_id { get; set; } // trave/mileage
        public decimal chargeout { get; set; }
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }

        public string paytype { get; set; }

        public bool visible { get; set; }
        public bool valid { get; set; } // some branch may not set up mileage...
    }

    public class UpdateParameter : GenericLaborRateRecord
    {
        public int action { get; set; } // may be to add or update.
    }

    public class DeleteParameter : GenericLaborRateRecord
    {
        public int action { get; set; }
    }

    public class UpdateResult
    {
        public bool okay { get; set; }
        public string error { get; set; }
    }
}
