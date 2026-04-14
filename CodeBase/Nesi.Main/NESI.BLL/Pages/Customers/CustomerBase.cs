using System.Data;
using System.Linq;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.Common.Models;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerBase : BLLBase
	{
		public int customer_id { get; set; }
		public bool is_new_allowed { get; set; }
		public bool is_edit_allowed { get; set; }
		public bool is_edit_accountmgr_allowed { get; set; }
		public bool is_admin_authenticated { get; set; }
		public bool is_qc_allowed { get; set; }
		public bool is_edit_past_notes_allowed { get; set; }
		public bool is_back_office { get; set; }
		public bool is_allowed_to_view_chargeout_settings { get; set; }
		
		public CustomerBase(Employee user) : base(user)
		{
			is_new_allowed = CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.AddCustomer);
			is_edit_allowed = CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.EditCustomer);
			is_edit_accountmgr_allowed = CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.ChangeAccountManager);
			is_admin_authenticated = CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.CustomerAdmin);
			is_qc_allowed = CurrentUser.AuthorizePage(OpsPage.QualityControlCustomers);
			is_edit_past_notes_allowed = CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.EditOldNotes);
			is_back_office = CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault();
			is_allowed_to_view_chargeout_settings = CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.ChargeoutSettingsTab);
				}


		public CustomerBase Profile()
		{
			return this;
		}

		public DatatableResult SearchCustomers(DTO.ViewModels.Page.Customers.SearchCriteriaPagination model)
		{
			var dt = bllToolbox.doSQL_dt(@"CALL CRM_SEARCH_V2(@p0, 1, 0, 0)", model.criteria);
			return new DatatableResult()
			{
				Page = model.page,
				PageSize = model.pageSize,
				TotalRecorders = dt.Rows.Count,
				Data = BLL.Common.Shared.DataTablePagination.GetPageData(dt, model.page, model.pageSize, model.sortField, model.sortOrder)
			};
		}

		public DatatableResult PostCodeSearch()
		{
			var dt = bllToolbox.doSQL_dt(@"SELECT a.postal_definer, b.ddl_name FROM territory_mapping a LEFT JOIN business_unit b ON a.business_unit_id = b.id ORDER BY b.name, a.postal_definer");
			return new DatatableResult()
			{
				Page = 0,
				PageSize = dt.Rows.Count,
				TotalRecorders = dt.Rows.Count,
				Data = dt
			};
		}
		public LabelValueInt[] GetRegAccountManagerList()
		{
			var o = bllToolbox.doSQL_Array<LabelValueInt>(
				"SELECT 0 value, 'Please select a Business Development Manager' label union SELECT netsuite_employee_internal_id value, netsuite_employee_name label FROM netsuite_sales_rep WHERE netsuite_isinactive = 0 AND netsuite_issales_rep = 1");
			return o;
		}
		public LabelValueInt[] GetIsrList(int customerID)
		{
			var o = bllToolbox.doSQL_Array<LabelValueInt>(
				"SELECT n.netsuite_employee_internal_id value, n.netsuite_employee_name label FROM netsuite_sales_rep n JOIN customer c ON n.netsuite_employee_internal_id = c.isr WHERE c.customer_id = @v0", customerID);
			return o;
		}
		public LabelValueInt[] GetOsrList(int customerID)
		{
			var o = bllToolbox.doSQL_Array<LabelValueInt>(
				"SELECT n.netsuite_employee_internal_id value, n.netsuite_employee_name label FROM netsuite_sales_rep n JOIN customer c ON n.netsuite_employee_internal_id = c.osr WHERE c.customer_id = @v0", customerID);
			return o;
		}
		public LabelValueInt[] GetAccountManagerList()
		{
			var o = bllToolbox.doSQL_Array<LabelValueInt>(
				(!is_edit_accountmgr_allowed ? "(select 999999 value, 'Not Set' label) union " : "") +
																								@"(SELECT a.member_id value, 
        CONCAT(b.ddl_name, ' - ', member_fullname) label 
        FROM member a LEFT JOIN business_unit b ON a.business_unit_id = b.id WHERE member_status = 'Active' 
        and member_membertype_id IN (4,5,8,11,12,17,27,29,34,35,36,37,38,39,40,41,46,49,52,53,54,55,56,59,60,66,71,75,76,79) 
		ORDER BY member_fullname)
			");
			return o;
		}


		public DataExtra GetCustomerByName(string name)
		{
			var o = bllToolbox.doSQL_Object<DataIdInt>(@"
					SELECT ifnull(COUNT(*),0) value, ifnull(customer_id,0) id FROM customer WHERE customer_name = @v0  LIMIT 1
					", name);

			if (o != null && o.value > 0)
			{
				return new DataExtra
				{
					Data = "success",
					Extra = o
				};
			}

			return new DataExtra
			{
				Data = "null",
				Extra = null
			};
		}

		public DataExtra GetCustomerByPhoneNumber(string phone)
		{
			var o = bllToolbox.doSQL_Object<DataIdInt>(@" SELECT ifnull(COUNT(*),0) value, ifnull(b.address_table_id,0) id 
					FROM phone_numbers a LEFT JOIN address b ON a.phone_numbers_table_id = b.address_id
					WHERE phone_numbers_number = @v0  AND phone_numbers_type = 'address' 
					AND phone_numbers_table_id IN 
					( SELECT d.address_id FROM customer c LEFT JOIN address d ON d.address_table_id = c.customer_id 
					WHERE c.customer_status != 4 ) LIMIT 1", phone);
			if (o != null && o.id > 0 && o.value > 0)
			{
				return new DataExtra
				{
					Data = "success.",
					Extra = bllToolbox.doSQL_Object<LabelValueInt>(@"SELECT customer_name label,customer_id value FROM customer a WHERE customer_id = @v0 LIMIT 1", o.id)
				};
			}

			return new DataExtra
			{
				Data = "null",
				Extra = null
			};
		}


		public LabelValueInt[] GetTaxList(bool include_not_set = false)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				(!is_admin_authenticated || include_not_set ? @"SELECT 0 value, 'Not Set' label UNION " : "") +
				@" SELECT tax_id value, CONCAT(tax_id,' ',tax_name,' - ', tax_percentage, '%') label FROM tax WHERE is_active = 1 ORDER BY value");
		}

		public LabelValueInt[] GetStatusList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				@"Select customer_or_contact_status_id value, customer_or_contact_status label from customer_or_contact_status order by customer_or_contact_status_id"
				);
		}
		public LabelValueInt[] GetGlReceiveList(int buId)
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"
					SELECT b.id value ,CONCAT(a.account_no,'-',a.gl_chart_name) label FROM GL_te a,GL_Group_te b  
					WHERE a.gl_group_id = b.id AND b.number ='120' AND a.tax_entity_id = @v0",
				BLL.Common.Cache.Global.BusinessUnit.GetValue(buId)?.tax_entity_id);
		}

		public int GetAddressId(int cust_id)
		{
			return bllToolbox.doSQL_int(@"SELECT address_id FROM address 
				WHERE address_table = 'Customer' AND address_table_id = @v0 AND address_type = 'B' LIMIT 1", customer_id);
		}

	}
}