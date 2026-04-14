using NESI.DTO.ViewModels.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DTO.ViewModels.Page.PurchaseOrder
{
    public class AddCreditCard
    {
        public int id { get; set; }
        public int member_id { get; set; }
        public DateTime date_requested { get; set; }
        public DateTime date_purchased { get; set; }
        public bool approved { get; set; }
        public string seller_id { get; set; }
        public CreditCardCurrency currency { get; set; }
        public double amount { get; set; }
        public int master_id { get; set; }
        public int expense_category_id { get; set; }
        public string status { get; set; }
        public string file_ext { get; set; }
        public string file_mime { get; set; }
        public int approved_by { get; set; }
        public int business_unit_id { get; set; }
        public int credit_card_id { get; set; }
        public string receipt_number { get; set; }
        public string item_text { get; set; }
        public int woprog_id { get; set; }
        public int customer_id { get; set; }
        public bool has_file { get; set; }
        public string file_path { get; set; }
        public string file_name { get; set; }
        public DateTime? approved_date { get; set; }
        public DateTime? paid_date { get; set; }
        public string Wo_number { get; set; }
    }
}
