using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.Reports
{
    [ModelDefination("MasterOutstandingInvoicesGrid")]
    public class MasterOutstandingInvoicesGrid : ModelBase<MasterOutstandingInvoicesGrid>
    {
        public string wOProg_InvoiceNo { get; set; }
        public string wOProg_InvoiceDate { get; set; }
        public string business_unit { get; set; }
        public string wOProg_CustomerName { get; set; }
        public string customer_collection_status { get; set; }
        public double wOProg_InvoicedNetTotal { get; set; }
        public string exp_pay { get; set; }
        public string woprog_ExpectedCheckRun { get; set; }
        public string woprog_invoice_collection_status { get; set; }
        public string wOProg_BVWO { get; set; }
        public string wOProg_CustPO { get; set; }
        public string ar_notes_note { get; set; }
        public long dayssince { get; set; }
        public long cust_no { get; set; }
        public double balance { get; set; }
        public double mat_cost { get; set; }
        public double lab_cost { get; set; }
        public string note_date { get; set; }
        public string terms { get; set; }
        public string cust_terms { get; set; }
        public string acct_manager { get; set; }
        public string contact_email { get; set; }
        public string contact_name { get; set; }
        public string customer_emails { get; set; }
        public long invoice_lag { get; set; }
        public long id { get; set; }
        public string phone { get; set; }
        public string pm { get; set; }
        public long processing_time { get; set; }
        public long scan_time { get; set; }
        public long woprog_id {get;set;}

        public string invoice_collection_status_name { get; set; }
        
    }
}
