using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Customers;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerSales : CustomerBase
	{
		public int address_id { get; set; }

		public CustomerSales(Employee user) : base(user)
		{
		}

		public CustomerSales(Employee user, int cust_id, int add_id) : base(user)
		{
			this.customer_id = cust_id;
			this.address_id = add_id;
		}


		public void SetExtraProperties(CustomerSalesProperties csp)
		{
			var _company = new NeBusinessUnit(CurrentUser.BusinessUnitId);

			csp.found = CurrentUser.AuthenticatedForPrivilege(132);
			csp.last_invoice = bllToolbox.doSQL_string(@"SELECT GET_DAYSSINCELASTINVOICE(@v0, @v1)", customer_id, address_id);
			csp.last_fiscal = bllToolbox.doSQL_double(@"SELECT IFNULL(SUM(woprog_invoicednettotal), 0) FROM woprog 
				WHERE woprog_customer_id = @v0  AND woprog_address_id = @v4  AND business_unit_id = @v1  AND woprog_invoicedate BETWEEN @v2  AND @v3 ",
				customer_id, CurrentUser.BusinessUnitId,
				_company.fiscal_start_previous.ToString("yyyy-MM-dd"),
				_company.fiscal_end_previous.ToString("yyyy-MM-dd"),
				address_id).ToString("C2");
			csp.current_fiscal = bllToolbox.doSQL_double(@" SELECT IFNULL(SUM(woprog_invoicednettotal), 0) FROM
				woprog WHERE woprog_customer_id = @v0  AND woprog_address_id = @v4 
				AND business_unit_id = @v1  AND woprog_invoicedate BETWEEN @v2  AND @v3 ",
				customer_id, _company.id, _company.fiscal_start_current.ToString("yyyy-MM-dd"),
				_company.fiscal_end_current.ToString("yyyy-MM-dd"), address_id).ToString("C2");
			var branch_fiscals = shared.GetBV7DSNs();
			double ytd = 0;
			double lytd = 0;
			foreach (DataRow branch_fiscal in branch_fiscals.Rows)
			{
				if (branch_fiscal["id"].ToString() != "8")
				{
					var b = new NeBusinessUnit(branch_fiscal["id"]);
					ytd += bllToolbox.doSQL_double(@" SELECT IFNULL(SUM(woprog_invoicednettotal), 0) FROM woprog 
WHERE woprog_customer_id = @v0  AND woprog_address_id = @v3  AND business_unit_id = @v1  
AND woprog_invoicedate BETWEEN @v2  AND CURDATE()", customer_id, b.id, Toolbox.MySQL_shortdt(b.fiscal_start_current), address_id);
					lytd += bllToolbox.doSQL_double(@" SELECT IFNULL(SUM(woprog_invoicednettotal), 0)
FROM woprog WHERE woprog_customer_id = @v0  
AND woprog_address_id = @v3  AND business_unit_id = @v1  AND woprog_invoicedate BETWEEN @v2  AND @v3 ",
customer_id, b.id, Toolbox.MySQL_shortdt(b.fiscal_start_previous), Toolbox.MySQL_shortdt(b.fiscal_end_previous), address_id);
				}
			}
			csp.ytd = ytd.ToString("C2");
			csp.lytd = lytd.ToString("C2");
		}

		public new object Profile()
		{
			var csp = AutoMapper.Mapper.Map<CustomerSalesProperties>(new customer_sales_properties(address_id));

			SetExtraProperties(csp);
			var sql = @"
				SELECT 
					customer_or_contact_status_id value, 
					customer_or_contact_status label
				FROM 
					customer_or_contact_status
				WHERE
					customer_or_contact_status_id IN (";
			switch (csp.status_id)
			{
				case 0:
					sql += "0,3,5,6,7,8,9";
					break;
				case 1:
					sql += "1,2,3,5,6,8,9";
					break;
				case 2:
					sql += "2,3,5,6,9";
					break;
				case 3:
					sql += "0,3,5,6,7,8";
					break;
				case 4:
					sql += "4";
					break;
				case 5:
					sql += "5";
					break;
				case 6:
					sql += "1,6";
					break;
				case 7:
					sql += "0,3,5,6,7,8";
					break;
				case 8:
					sql += "3,5,6,7,8";
					break;
				case 9:
					sql += "0,3,5,6,7,8,9";
					break;
			}
			sql += @")
ORDER BY 
	customer_or_contact_status_order, value";

			const string sqlPm = @"SELECT 0 value,'Please Select a User' label,0 orderby
UNION
SELECT a.member_id value, CONCAT(b.name, ' - ', member_fullname) label, 1 orderby 
FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE member_status = 'Active'
and member_membertype_id IN (17,5,11,12,8,25,4,29,46,34,49,36,38,39,75) ORDER BY orderby, label";

			const string sqlcontacts = @"SELECT contact_id value,contact_name label, 1 orderby
FROM contact WHERE contact_type='Customer'
AND contact_cust_id = @p0 AND contact_status = 'Active' AND address_id = @p1
UNION select 0 value,'Unknown' label, 0 orderby order by orderby, label";

			const string sqlOrigin = @"SELECT id value, name label FROM customer_origin ORDER BY label";


			const string sqlAm =
				@"Call get_account_managers(@v0)";

			const string sqlHistoryActionList =
				@"SELECT history_action_type_id value,history_action_type_action label FROM history_action_type WHERE history_action_type_action NOT LIKE '%edit' ORDER BY label";
			const string sqlHistoryActionMemberList =
				@"SELECT member_id value, member_fullname label FROM member WHERE member_status = 'Active' ORDER BY label";

			var statusList = bllToolbox.doSQL_Array<LabelValueInt>(sql);
			var pmList = bllToolbox.doSQL_Array<LabelValueInt>(sqlPm);
			var contactList = bllToolbox.doSQL_Array<LabelValueInt>(sqlcontacts, customer_id, address_id);
			var originList = bllToolbox.doSQL_Array<LabelValueInt>(sqlOrigin);
			
		    var amList = new List<LabelValueInt>{ new LabelValueInt("Please select an Account Manager", 0) };
            var amDataTable = bllToolbox.doSQL_dt(sqlAm, address_id);
		    if (amDataTable != null)
		    {
		        foreach (DataRow dr in amDataTable.Rows)
		        {
		            var id = (int) dr["id"];
		            var fn = (string)dr["fn"];
		            if (id != 0)
		            {
		                amList.Add(new LabelValueInt(fn, id));
                    }
		        }
		    }

		    var historyActionList = bllToolbox.doSQL_Array<LabelValueInt>(sqlHistoryActionList);
			var historyActionMemberList = bllToolbox.doSQL_Array<LabelValueInt>(sqlHistoryActionMemberList);
			var visibleBusinessUnitList = CurrentUser.VisibleBusinessUnitLabelValueList;
			var historyList = GetHistory();
			return new
			{
				lists = new
				{
					statusList,
					pmList,
					contactList,
					originList,				
					amList,
					historyActionList,
					historyActionMemberList,
					visibleBusinessUnitList,
					historyList
				},
				data = csp
			};
		}

		public DataTable GetHistory()
		{
			return bllToolbox.doSQL_dt(@"
SELECT
	c.member_fullname member,
	a.customer_history_date date,
	b.history_action_type_action action,
	a.customer_history_notes notes,
	d.name origin
FROM
	customer_history a
INNER JOIN 
	history_action_type b 
		ON a.customer_history_action = b.history_action_type_id 
LEFT JOIN 
	member c 
		ON a.customer_history_memberid = c.member_id 
LEFT JOIN 
	customer_origin d 
		ON a.origin = d.id
WHERE 
	a.customer_history_custid = @v0 AND
	a.address_id = @v1
ORDER BY 
	a.customer_history_date desc, a.customer_history_id desc
		", customer_id, address_id);
		}

		public DataExtra Save(CustomerSalesProperties model)
		{

			var this_customer = new NECustomer((int)customer_id);
			var this_address = new NEAddress(address_id);
			var csp = new customer_sales_properties(address_id);

			var temp_discount = csp.discount_pct;
			if (csp.discount_pct != 0 && temp_discount < csp.discount_pct)
			{
				var email = new NeEMail
				{
					Subject = this_customer.Customer_Name + " has had their discount raised to " + csp.discount_pct + "% by " +
							  CurrentUser.FullName
				};
				var bms = Toolbox.doSQL_dt(@"SELECT member_neemail e FROM member  WHERE member_membertype_id IN (5,29) AND member_status = 'Active'", null);
				var addresses = new List<string>();
				foreach (DataRow dr in bms.Rows)
				{
					addresses.Add(dr["e"].ToString());
				}
				email.To = addresses.Aggregate((x, y) => x + ";" + y);
				email.From = "CustomerDiscounts@thatsnew.com";
				email.Send();
				NECustomer.add_to_customer_history(customer_id, CurrentUser.Id, System.DateTime.Now,
					$"Customer discount changed from {temp_discount}% to {csp.discount_pct}%, for location: {this_address.Addr1}", 6, 1, address_id);
			}

			csp =(customer_sales_properties) MapperFrom(csp, model);
			csp.next_followup_date = model.next_followup_date ?? new DateTime();
			csp.save();
			var r = AutoMapper.Mapper.Map<CustomerSalesProperties>(csp);
			SetExtraProperties(r);
			return new DataExtra()
			{
				Data = "Sales settings have been saved successfully.",
				Extra = r
			};
		}

		public DataExtra SaveHistory(CustomerHistory model)
		{
			var ch = new customer_history();
			ch = (customer_history) MapperFrom(ch, model);
			//  AutoMapper.Mapper.Map<customer_history>(model);
			ch.save();

			return new DataExtra()
			{
				Data = "Histroy has been saved successfully.",
				Extra = GetHistory()
			};
		}
	}


}