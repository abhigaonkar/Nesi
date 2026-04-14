using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class MasterOutstandingInvoicesGrid : BLLGridBase<DTO.ViewModels.Page.Reports.MasterOutstandingInvoicesGrid>
    {
        public MasterOutstandingInvoicesGrid()
        {
        }

        public MasterOutstandingInvoicesGrid(Employee user) : base(user)
        {
        }

        public MasterOutstandingInvoicesGrid(Employee user, BodyParams param, string cust_id, string te_id, string origin) : base(user, param, new object[] { })
        {
            var adder = "";
            if (!this.CurrentUser.AuthenticatedForPrivilege(110))
            {
                adder = " and tax_entity_id = " + this.CurrentUser.BusinessUnit.tax_entity_id;
            }
            if ((origin.ToLower() == "ar_report") && (cust_id != null) && (cust_id != "") && (te_id != null) && (te_id != ""))
            {
                //	gv_invoices.Width = Unit.Pixel(870);
                adder += " and woprog_customer_id = " + cust_id + " and tax_entity_id = " + te_id;
            }

            string query = @"
SELECT
	customer_collection_status.id,
	a.woprog_id,
	a.wOProg_BVWO,
	a.wOProg_CustomerName,
	c.contact_name,
	c.contact_email,
	customer_collection_status,
	a.WOProg_CustPO wOProg_CustPO,
	business_unit.ddl_name business_unit,
	a.wOProg_InvoicedNetTotal,
	a.woprog_ExpectedCheckRun,
	IFNULL(a.woprog_invoice_collection_status, 0) woprog_invoice_collection_status,
	a.wOProg_InvoiceNo,
	a.wOProg_InvoiceDate,
	a.woprog_invoice_paid_date,
	(SELECT IFNULL(DATEDIFF(curdate(), (SELECT MAX(Date) FROM membertime WHERE membertime_woprog_id = a.woprog_id)), 0) FROM woprog WHERE woprog_id = a.woprog_id) invoice_lag,
	date_add(a.WOProg_InvoiceDate, interval a.woprog_dayscredit DAY)  as exp_pay,
	a.woprog_customer_id, a.woprog_invoicebalance as balance,
	projman.member_fullname pm,
	a.business_unit_id, datediff(curdate(),WOProg_InvoiceDate) as dayssince,
	(Select ar_notes_note from ar_notes where ar_notes_woprogid = a.woprog_id order by ar_notes_id desc limit 1) as ar_notes_note,
	(Select ar_notes_ts from ar_notes where ar_notes_woprogid = a.woprog_id order by ar_notes_id desc limit 1) as note_date,
	address_phonefull as phone,
	concat(customer_invoice_address,',',customer_invoice_ccaddress,',',customer_autostatement_address,',',customer_autostatement_ccaddress) as customer_emails,
	customer.customer_number as cust_no, 
	csp_m.member_fullname acct_manager ,
	(SELECT IFNULL(DATEDIFF(woprog_opendatetime, (SELECT MAX(Date) FROM membertime WHERE membertime_woprog_id = a.woprog_id)), 0) FROM woprog WHERE woprog_id = a.woprog_id) scan_time,
	(SELECT IFNULL(DATEDIFF(woprog_invoicedate, woprog_opendatetime), 0) FROM woprog WHERE woprog_id = a.woprog_id) processing_time,
  a.woprog_dayscredit terms,
if(" + !this.CurrentUser.AuthenticatedForPrivilege(58) + @",0,woprog_laborcost) lab_cost, 
	if(" + !this.CurrentUser.AuthenticatedForPrivilege(58) + @",0,woprog_materialcost) mat_cost,
customer.customer_creditdays cust_terms,
business_unit.tax_entity_id te_id,
invoice_collection_status.invoice_collection_status_name
FROM
	woprog a 
INNER JOIN 
	customer ON 
		 a.woprog_customer_id = customer.Customer_ID
LEFT JOIN 
	customer_collection_status ON  
		customer.customer_collection_status = customer_collection_status.ID
LEFT JOIN  
	invoice_collection_status ON  
		a.woprog_invoice_collection_status = invoice_collection_status.invoice_collection_status_id
INNER join  
	business_unit ON  
		a.business_unit_id = business_unit.id
INNER JOIN  
	address on  
		customer.customer_id=address.address_table_id and address_table = 'Customer' and address_type = 'B' 
LEFT JOIN  
	customer_sales_properties csp ON  
		address.address_id = csp.address_id 
LEFT JOIN  
	member csp_m ON  
		csp.account_manager = csp_m.member_id  
LEFT JOIN
	member projman ON
		a.woprog_pm_memberid = projman.member_id
LEFT JOIN
	contact c ON a.woprog_contact_id = c.contact_id
WHERE
	a.woprog_invoice_paid_date is null AND
	a.WOProg_Status = 'Invoiced' AND
a.WOProg_InvoicedNetTotal > 0 AND
	a.business_unit_id NOT IN (8,36,26) AND
    find_in_set(business_unit.id,'{bu_ids}') ";

            this.query = query + adder;
        }

        public DataExtra ConfirmPaidDate(ConfirmPaidList data)
        {
            DataExtra result = new DataExtra()
            {
                Data = "Confirmed Paid Dates have been updated successfully",
                Extra = ""
            };

            if (data == null || data.confirmedPaidDate == null || data.woprogIDs == null || data.woprogIDs.Count == 0)
            {
                result.Data = "No data updated.";
                result.Extra = "";
                return result;
            }

            try
            {
                foreach (var item in data.woprogIDs)
                {
                    bllToolbox.doSQL_void(
                        @"update woprog  set woprog_ExpectedCheckRun =@v0  where woprog_id =@v1 limit 1",
                        new object[] { data.confirmedPaidDate, item });
                }

                ClearCache();
            }
            catch (Exception ex)
            {
                result.Data = "Failed to update Confirmed Paid Date.";
                result.Extra = ex.Message;
            }

            return result;
        }

        public List<InvoiceNotes> GetInvoiceNotes(long woID)
        {
            List<InvoiceNotes> list = new List<InvoiceNotes>();
            var data = Toolbox.doSQL_dt(
                       @"SELECT ar_notes_id as id, get_name(ar_notes_memberid) AS member,ar_notes_ts AS notedate, urldecode(ar_notes_note) AS note FROM ar_notes WHERE ar_notes_woprogid = @v0 ORDER BY ar_notes_id DESC",
                       new object[] { woID });

            if (data != null && data.Rows != null && data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    InvoiceNotes r = new InvoiceNotes {
                        id = Convert.ToInt32(data.Rows[i]["id"]),
                        member = Convert.ToString(data.Rows[i]["member"]),
                        notedate = Convert.ToString(data.Rows[i]["notedate"]),
                        note = Convert.ToString(data.Rows[i]["note"]),
                        noteForEditing = Convert.ToString(data.Rows[i]["note"])
                    };

                    if (!string.IsNullOrEmpty(r.note))
                    {
                        r.note = r.note.Replace("\n", "<br/>");
                    }
                    list.Add(r);
                }
            }

            return list;
        }

        public DataExtra AddOrUpdateNote(EditNote note)
        {
            DataExtra result = new DataExtra()
            {
                Data = "Notes have been added successfully",
                Extra = ""
            };

            try
            {
                if (note.noteId > 0)
                {
                    // Add a new note
                    bllToolbox.doSQL_void(
                        @"Update ar_notes  set ar_notes_memberid =@v0, ar_notes_note =@v1 ,ar_notes_ts =@v2   where ar_notes_id =@v3",
                        new object[] { this.CurrentUser.Id, note.note, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), note.noteId });
                    result.Data = "Notes have been updated successfully";
                }
                else
                {
                    bllToolbox.doSQL_void(
                        @"Insert into ar_notes (ar_notes_memberid,ar_notes_note,ar_notes_woprogid,ar_notes_ts)  values (@v0,@v1,@v2,@v3)",
                        new object[] { this.CurrentUser.Id, note.note, note.woId, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                }
            }
            catch (Exception ex)
            {
                result.Data = "Failed to add or update notes";
            }

            return result;
        }

        public DataExtra UpdateSingleOutStandingInvoice(DTO.ViewModels.Page.Reports.MasterOutstandingInvoicesGrid invoice)
        {
            DataExtra result = new DataExtra()
            {
                Data = "Information has been saved successfully",
                Extra = invoice
            };

            try
            {
                var status = string.IsNullOrWhiteSpace(invoice.customer_collection_status) ? 0 : int.Parse(invoice.customer_collection_status);
                var inv_status = string.IsNullOrWhiteSpace(invoice.woprog_invoice_collection_status) ? 0 : int.Parse(invoice.woprog_invoice_collection_status);
                var confirmed = string.IsNullOrWhiteSpace(invoice.woprog_ExpectedCheckRun) ? "" : Convert.ToDateTime(invoice.woprog_ExpectedCheckRun).ToString("yyyy-MM-dd");
                var custpo = invoice.wOProg_CustPO;

                var wo = new NeWOProg(Convert.ToInt32(invoice.woprog_id));
                if (string.IsNullOrWhiteSpace(custpo))
                {
                    custpo = wo.PONumber;
                }

                var cust = new NECustomer((int)wo.WOProg_Customer_ID)
                {
                    customer_collection_status = status,
                    Member_ID = this.CurrentUser.Id
                };

                cust.Save();

                var exp = "";
                if (confirmed != "")
                {
                    exp = ",woprog_ExpectedCheckRun='" + confirmed + "'";
                }
                else
                {
                    exp = ",woprog_ExpectedCheckRun=null";
                }

                Toolbox.doSQL_void(@"update woprog  set woprog_invoice_collection_status=@v0 " + exp + ", WOProg_CustPO=@v1   where woprog_id =@v2", 
                    new object[] { inv_status, custpo, invoice.woprog_id });

                if (wo.woprog_invoice_collection_status != inv_status)
                {
                    var invoice_status = Toolbox.doSQL_string(@"Select invoice_collection_status_name from invoice_collection_status  where invoice_collection_status_id =@v0", new object[] { inv_status });
                    if (invoice_status.Trim() == "")
                    {
                        Toolbox.doSQL_void(@"Insert into ar_notes (ar_notes_memberid,ar_notes_note,ar_notes_woprogid,ar_notes_ts)  values (@v0,'Invoice Status Cleared',@v1,@v2)",
                            new object[] { this.CurrentUser.Id, wo.woprog_id, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                    }
                    else
                    {
                        var v1 = "Invoice Status changed to " + invoice_status;
                        Toolbox.doSQL_void(@"Insert into ar_notes (ar_notes_memberid,ar_notes_note,ar_notes_woprogid,ar_notes_ts)  values (@v0,@v1,@v2,@v3)",
                            new object[] { this.CurrentUser.Id, v1, wo.woprog_id, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                    }
                }

                if (confirmed != "")
                {
                    invoice.woprog_ExpectedCheckRun = confirmed;
                }

                ClearCache();
            }
            catch (Exception ex)
            {
                result.Data = string.Format("[{0}] Invoice can't be updated.", invoice.wOProg_InvoiceNo);
                result.Extra = ex.Message;
            }

            return result;
        }


        public LabelValueInt[] GetCustomerStatus()
        {
            var statuses = bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT ID value, status_name label FROM customer_collection_status");
            foreach (var item in statuses)
            {
                if (item.Label.Trim() == "")
                {
                    item.Label = "　";
                }
            }
            return statuses;
        }

        public LabelValueInt[] GetInvoiceStatus()
        {
            var statuses = bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT invoice_collection_status_id value, invoice_collection_status_name label FROM invoice_collection_status");
            foreach (var item in statuses)
            {
                if (item.Label.Trim() == "")
                {
                    item.Label = "　";
                }
            }
            return statuses;
        }

    }

    public class ConfirmPaidList
    {
        public DateTime confirmedPaidDate { get; set; }
        public List<long> woprogIDs {get;set;}
    }

    public class InvoiceNotes
    {
        public long id { get; set; }
        public string member { get; set; }
        public string notedate { get; set; }
        public string note { get; set; }
        public string noteForEditing { get; set; }
    }

    public class EditNote
    {
        public string note { get; set; }
        public long noteId { get; set; }
        public long woId { get; set; }
    }
}
