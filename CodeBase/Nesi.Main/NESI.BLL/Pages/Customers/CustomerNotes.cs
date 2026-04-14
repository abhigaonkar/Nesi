using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerNotes : CustomerBase
	{
		public int address_id { get; set; }
		public string customer_memo { get; set; }
		public string customer_workorder_memo { get; set; }
		public string customer_arnotes { get; set; }
		public string customer_salesnotes { get; set; }
		public string public_notes { get; set; }
		private readonly NECustomer thisCustomer;
		private readonly customer_sales_properties thisCsp;
		public bool is_billAddress { get; set; }

		public CustomerNotes(Employee user) : base(user)
		{
		}

		public CustomerNotes(Employee user, int cust_id, int add_id) : base(user)
		{
			this.customer_id = cust_id;
			this.address_id = add_id;
			thisCustomer = new NECustomer(customer_id);
			thisCsp = new customer_sales_properties(address_id);
			customer_arnotes = thisCustomer.customer_arnotes;
			customer_salesnotes = thisCsp.notes_sales;
			customer_memo = thisCustomer.Memo;
			customer_workorder_memo = thisCustomer.WorkOrder_Memo;
			public_notes = thisCsp.notes_public;

			is_billAddress = (new NEAddress(address_id)).Type == "B";
		}

		public DataTable GetArEmails()
		{
			return bllToolbox.doSQL_dt(@"CALL CUSTCOLLECTIONEMAILS(@v0)", customer_id);
		}

		public DataTable GetInvoiceNotes()
		{
			return bllToolbox.doSQL_dt(@" SELECT
				a.`ar_notes_id` AS `id`,
				a.ar_notes_ts as `Date`,
				a.ar_notes_note as `Note`, 
				c.member_fullname as `By`,
				b.woprog_invoiceno as Invoice,
				b.woprog_bvwo as `wo` 
				FROM ar_notes a INNER JOIN woprog b ON a.ar_notes_woprogid = b.woprog_id 
				LEFT JOIN member c ON a.ar_notes_memberid = c.member_id 
				WHERE b.woprog_customer_id =@v0 ORDER BY a.ar_notes_ts DESC",
				customer_id);
		}

		public DataExtra Save(DTO.ViewModels.Core.DataIdString model)
		{
			switch (model.id)
			{
				case 1: // save ar notes
					thisCustomer.customer_arnotes = model.value;
					thisCustomer.Save();
					return new DataExtra()
					{
						Data = "Ar notes have been saved successfully.",
						Extra = model.value
					};
				case 2: // save_invoicing_instructions
					thisCustomer.Memo = model.value;
					thisCustomer.Save();
					return new DataExtra()
					{
						Data = "Invoicing instructions have been saved successfully.",
						Extra = model.value
					};
				case 3: // add_public_note
					thisCsp.notes_public = "[" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + CurrentUser.FullName + " - " + model.value + "\n" + thisCsp.notes_public + "\n";
					thisCsp.save();
					return new DataExtra()
					{
						Data = "Pulbic notes have been saved successfully.",
						Extra = thisCsp.notes_public
					};
				case 4: // add_sales_note
					thisCsp.notes_sales = "[" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + CurrentUser.FullName + " - " + model.value + "\n" + thisCsp.notes_sales + "\n";
					thisCsp.save();
					return new DataExtra()
					{
						Data = "Sales notes have been saved successfully.",
						Extra = thisCsp.notes_sales
					};
				case 5: // save_sales_note
					thisCsp.notes_sales = model.value;
					thisCsp.save();
					return new DataExtra()
					{
						Data = "Sales notes have been saved successfully.",
						Extra = thisCsp.notes_sales
					};
				case 6: // save_workorder_instructions
					thisCustomer.WorkOrder_Memo = model.value;
					thisCustomer.Save();
					return new DataExtra()
					{
						Data = "Workorder instructions have been saved successfully.",
						Extra = model.value
					};
			}
			return null;
		}
	}
}