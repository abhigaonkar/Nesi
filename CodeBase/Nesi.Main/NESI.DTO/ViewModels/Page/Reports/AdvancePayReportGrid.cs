using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("AdvancePayReportGrid")]
    public class AdvancePayReportGrid : ModelBase<AdvancePayReportGrid>
    {
        public long woprog_id { get; set; }
    
        public string parentBranch { get; set; }
        public long jobCostWO { get; set; }

        public string customer { get; set; }
        public string q_id { get; set; }
        public double? quoted_Costs { get; set; }
        public double? main_Labour_Cost { get; set; }
        public double? main_Material_Cost { get; set; }
        public string divorChildBusinessUnit { get; set; }
        public string divorChildCustomerName { get; set; }
        public DateTime? invoiceDate { get; set; }

        public string divtoDivWO { get; set; }
        public string divtoDivWOStatus { get; set; }
        public double? divtoDivTMLabourSell { get; set; }
        public string divtoDivTMMaterialSell { get; set; }
        public string postingNotes { get; set; }
        public string childWO { get; set; }
        public string childWOStatus { get; set; }
        public double? childTMLabourSell { get; set; }
        public double? childMMaterialSell { get; set; }

        public string main_wo_status { get; set; }
        public double? quoted_price { get; set; }
        public double? progress_billed_so_far { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public DateTime? wOProg_InvoiceDate { get; set; }
        public string description { get; set; }
        public double? final_invoiced { get; set; }
        public int? rev { get; set; }
    }
}
