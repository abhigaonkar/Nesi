using System;
using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Customers;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerEdit : CustomerBase
	{
		public bool is_exist { get; set; }
		public CustomerEditProfile profile { get; set; }
		public LabelValueInt[] AccountManagerList { get; set; }
		public LabelValueInt[] ProjectManagerList { get; set; }
		public LabelValueInt[] StatusList { get; set; }
		public LabelValueInt[] RegAccountManagerList { get; set; }
		public LabelValueInt[] ProjectIsrList { get; set; }
		public LabelValueInt[] ProjectOsrList { get; set; }

		public new CustomerEdit Profile()
		{
			is_exist = bllToolbox.doSQL_bool(@"SELECT COUNT(*) FROM customer WHERE customer_id = @v0  LIMIT 1", customer_id);
			if (is_exist)
			{
				var _customer = new NECustomer(customer_id);
				//var _company = new NeBusinessUnit(_customer.business_unit_id);
				profile = GetProfile(_customer);
				AccountManagerList = GetAccountManagerList();
				ProjectManagerList = GetProjectManagerList(_customer.business_unit_id);
				StatusList = GetStatusList();
				RegAccountManagerList = GetRegAccountManagerList();
				ProjectIsrList = GetIsrList(customer_id);
				ProjectOsrList = GetOsrList(customer_id);
			}
			return this;
		}

		private DataTable GetWorkOrders(string address_id)
		{
			return bllToolbox.doSQL_dt($@"
 SELECT
			a.woprog_id,
			a.WOProg_CutDateTime cut_dt,
				c.contact_name,
			a.WOProg_CloseDateTime close_dt,
				a.woprog_bvwo wo_number,
				a.woprog_description wo_description,
				a.woprog_status status,
				IF(a.WOProg_QuoteID = 0 OR a.woprog_quoteid < 100000, NULL, SUBSTRING(a.WOProg_QuoteID, 1, 6)) quote_id,
			IF(a.WOProg_QuoteID = 0 OR a.woprog_quoteid < 100000, NULL, SUBSTRING(a.WOprog_quoteid, 7, 3)) rev,
			a.business_unit_id,
			a.woprog_bvwo,
			CONCAT(b.Address_Addr1, ',', b.Address_City) addy
				FROM
			woprog a
			left join
			address b on b.address_id = a.woprog_address_id
			LEFT JOIN
			contact c on a.woprog_contact_id = c.contact_id
			WHERE
			find_in_set(a.business_unit_id, @p0) and
			a.woprog_customer_id = @p1 AND
			a.woprog_bvwo != '' and
				(({address_id} != '' and {address_id} != 0 and {address_id} is not null and a.woprog_address_id = {address_id})
			or ({address_id} = 0) or ({address_id} = '') or ({address_id} is null))
			ORDER BY a.woprog_ID DESC", CurrentUser.VisibleBusinessUnits, customer_id);
		}

		public CustomerEditProfile GetProfile(NECustomer _customer)
		{
			var address_id = GetAddressId(_customer.Customer_ID);

            var o = new CustomerEditProfile
            {
                customer_id = _customer.Customer_ID,
                customer_number = string.IsNullOrEmpty(_customer.Customer_Number) ? "N/A" : _customer.Customer_Number,
                added_by = bllToolbox.doSQL_string(@"SELECT IFNULL(MAX(member_fullname), 'Not Set') FROM member WHERE member_id = '{0}' LIMIT 1", _customer.InitMember_ID),
                date_added = _customer.CreatedDateTime,
                qc_dt1 = Toolbox.MySQL_shortdt(_customer.QC_DateTime),
                qc_dt2 = Toolbox.MySQL_shortdt(_customer.QC2_DateTime),
                business_unit_id = _customer.business_unit_id,
                customer_name = _customer.Customer_Name,
                why_hold = _customer.customer_whyhold,
                status_id = _customer.Customer_Status,
                is_partner = _customer.is_partner,
                is_on_hold = _customer.Hold == "T",
                who_hold = (new NeMember(_customer.customer_whohold)).FullName,
                account_manager_id = _customer.Account_Manager,
                project_manager_id = _customer.Project_Manager,
                addresses = GetAddresses(_customer.Customer_ID),
                total_all = GetTotal(0),
                total_1_year = GetTotal(1),
                total_2_year = GetTotal(2),
				reg_account_manager_id = _customer.s_regional_account_manager,
				isr = _customer.s_inside_sales_rep,
				osr = _customer.s_outside_sales_rep,
				active = _customer.active,
				key_account = _customer.key_account ?? false

		};
			o.qc_dt1_text = o.customer_number == "N/A" || o.qc_dt1 == "0001-01-01" || o.qc_dt1 == "" ? "" : o.qc_dt1;
			o.qc_dt2_text = o.customer_number == "N/A" || o.qc_dt2 == "0001-01-01" || o.qc_dt2 == "" ? "" : o.qc_dt2;
			//o.qc1approve_visible = o.qc_dt1_text == "N/A" && is_qc_allowed;
			//o.qc2approve_visible = o.qc_dt2_text == "N/A" && is_qc_allowed;
			//o.qc1_visible = !o.qc1approve_visible && is_qc_allowed;
			//o.qc2_visible = !o.qc2approve_visible && is_qc_allowed;

			if (_customer.Address.csp == null) return o;

			o.account_manager_id = _customer.Address.csp.am_member_id;
			o.project_manager_id = _customer.Address.csp.project_mgr_member_id;
			o.business_unit_id = bllToolbox.doSQL_int(@"SELECT ifnull(a.business_unit_id,0)
				FROM member a WHERE a.member_id = @v0 LIMIT 1", _customer.Address.csp.project_mgr_member_id);
			return o;
		}

		public DataTable GetAddresses(int custId)
		{
			return bllToolbox.doSQL_dt(@"SELECT
a.Address_ID AS address_id,
			IF (
					  address_type = 'B',
					  'Billing',
					  'Shipping'
			) AS addr_type,
			a.Address_Desc AS description,
			a.Address_Addr1 AS addr1,
			a.Address_Addr2 AS addr2,
			a.Address_Addr3 AS addr3,
			a.Address_Addr4 AS addr4,
			CONCAT(a.Address_Addr1,' ',a.Address_Addr2,' ',a.Address_Addr3,' ',a.Address_Addr4,',',a.Address_City,' ',a.Address_Prov,' ', a.Address_Postal, ' ',a.Address_Country) AS addr,
			a.Address_City AS city,
			a.Address_Prov AS provstate,
			a.Address_Postal AS postal,
			a.Address_Country AS country,
			IFNULL(c.customer_or_contact_status, 'Not Set') AS statuss,
			member.member_fullname AS am,
			business_unit.name AS branch,
			a.address_table 
			FROM
			address AS a
			LEFT JOIN customer_sales_properties AS b ON a.Address_ID = b.address_id
			LEFT JOIN customer_or_contact_status AS c ON b.status_id = c.customer_or_contact_status_id
			LEFT JOIN customer_sales_properties ON customer_sales_properties.customer_id = b.customer_id AND customer_sales_properties.address_id = b.address_id
			LEFT JOIN member ON member.Member_ID = customer_sales_properties.account_manager
			LEFT JOIN business_unit ON member.business_unit_id = business_unit.id
			WHERE
					  (address_table = 'Customer' or address_table='Worksite') AND address_table_id =@p0 ", custId);
		}

		public CustomerEdit(Employee user) : base(user)
		{
		}

		public CustomerEdit(Employee user, int cust_id) : base(user)
		{
			this.customer_id = cust_id;
		}

		public DataExtra Save(DTO.ViewModels.Page.Customers.CustomerEdit model)
		{
			var this_customer = new NECustomer((int)customer_id);
			if (this_customer.Customer_Status != 4 && model.customer_id == 4)
			{
				return new DataExtra("Customer status can not be set to merged.");
			}
			var prev_hold_status = this_customer.Hold == "T";
			var prev_name = this_customer.Customer_Name;
			this_customer.Customer_Name = model.customer_name;
			var address_id = Toolbox.doSQL_int(@"SELECT address_id FROM address 
				WHERE address_table = 'Customer' AND address_table_id = @v0 AND address_type = 'B' LIMIT 1", customer_id);
			var this_hold = model.is_on_hold;
			this_customer.Member_ID = UserId;
			this_customer.Customer_Status = model.status_id;
			if (this_hold != prev_hold_status)
			{
				model.why_hold = model.is_on_hold ? model.why_hold : "";
				this_customer.customer_whohold = UserId;
				this_customer.customer_whyhold = model.why_hold;
				NECustomer.add_to_customer_history(customer_id, UserId, System.DateTime.Now, this_customer.customer_whyhold, (prev_hold_status ? 20 : 19), 1, address_id);
			}
			if (is_edit_accountmgr_allowed)
			{
				this_customer.Account_Manager = model.account_manager_id;
			}
			this_customer.Project_Manager = model.project_manager_id;
			this_customer.Hold = this_hold ? "T" : "F";
			this_customer.is_partner = model.is_partner;
			this_customer.Save();
			new CustomerHomeSearch(CurrentUser).ClearCache();
			if (prev_name != model.customer_name)
			{
				shared.alert_ar($"FYI: Customer Name Change - #{this_customer.Customer_Number}",
					$"{CurrentUser.FullName} has changed customer #{this_customer.Customer_Number}'s name from <b>{prev_name}</b> to <b>{model.customer_name}</b>");
			}
			return new DataExtra()
			{
				Data = "Customer has been saved successfully.",
				Extra = new
				{
					is_on_hold = model.is_on_hold,
					why_hold = model.why_hold
				}
			};
		}

		public double GetTotal(int type)
		{
			var sql = "";
			switch (type)
			{
				case 0:
					sql = "";
					break;
				case 1:
					sql = " AND woprog_invoicedate > CURDATE() - INTERVAL 1 YEAR";
					break;
				case 2:
					sql = " AND woprog_invoicedate > CURDATE() - INTERVAL 2 YEAR";
					break;
			}
			sql = $@"SELECT
				  IFNULL(SUM(woprog_invoicednettotal), 0) AS total
				FROM
				  woprog
				WHERE woprog_customer_id=@p0  {sql}
				GROUP BY woprog_customer_id";
			return bllToolbox.doSQL_double(sql, customer_id);
		}

		public object GetAccounting(int custId, int addressId)
		{
			var c = new NECustomer(custId);
			var a = new NEAddress(addressId);
			var taxList = GetTaxList(true);
			var glReceiveList = GetGlReceiveList(c.business_unit_id);
			var o = new CustomerAccounting
			{
				customer_id = custId,
				address_id = addressId
			};
			if (addressId <= 0)
			{
				o.gl_account = "";
				o.tax_1 = 999999;
				o.tax_2 = 999999;
				o.tax_3 = 999999;
				o.tax_4 = 999999;
			}
			else
			{
				o.gl_account = a.RVAccount;
				o.tax_1 = a.Tax1;
				o.tax_2 = a.Tax2;
				o.tax_3 = a.Tax3;
				o.tax_4 = a.Tax4;
				o.taxex_1 = a.Tax1Exempt;
				o.taxex_2 = a.Tax2Exempt;
				o.taxex_3 = a.Tax3Exempt;
				o.taxex_4 = a.Tax4Exempt;
			}

			return new
			{
				taxList,
				glReceiveList,
				accounting = o
			};
		}


		public object GetAddress(int custId, int addressId)
		{
			var provList = GetProvList();
			var countryList = GetCountryList();
			if (addressId <= 0)
			{
				return new
				{
					provList,
					countryList,
					address = new CustomerAddress
					{
						address_prov = "ON",
						address_country = "CDN",
						table = "Customer",
						customer_id = custId,
						address_id = 0,
						description = "",
						address_line_1 = "",
						address_line_2 = "",
						address_line_3 = "",
						address_line_4 = "",
						address_city = "",
						address_postalcode = "",
						phone_area = "",
						phone_prefix = "",
						phone_suffix = "",
						phone_ext = "",
						fax_area = "",
						fax_prefix = "",
						fax_suffix = "",
						gps_coordinates = "",
						website = "",
						facebook = "",
						twitter = "",
						linkedin = "",
                        Active=false,
                    }
				};
			}
			var o = new NEAddress(addressId);
			return
				new
				{
					provList,
					countryList,
					address = new CustomerAddress
					{
						customer_id = o.Table_ID,
						address_id = addressId,
						table = o.Table,
						description = o.Desc,
						address_line_1 = o.Addr1,
						address_line_2 = o.Addr2,
						address_line_3 = o.Addr3,
						address_line_4 = o.Addr4,
						address_city = o.City,
						address_prov = o.Prov,
						address_postalcode = o.Postal,
						address_country = o.Country,
						phone_area = o.PhoneArea,
						phone_prefix = o.Phonefirst,
						phone_suffix = o.PhoneLast,
						phone_ext = o.PhoneExt,
						fax_area = o.FaxArea,
						fax_prefix = o.FaxFirst,
						fax_suffix = o.FaxLast,
						gps_coordinates = o.GPS_Coords,
						website = o.Web,
						facebook = o.facebook,
						twitter = o.twitter,
						linkedin = o.linkedin,
                        Active=o.Active
					}
				};
		}

		public string SaveAccounting(CustomerAccounting model)
		{
			var a = model.address_id <= 0 ? new NEAddress() : new NEAddress(model.address_id);
			a.Tax1 = model.tax_1;
			a.Tax2 = model.tax_2;
			a.Tax3 = model.tax_3;
			a.Tax4 = model.tax_4;
			a.Tax1Exempt = string.IsNullOrEmpty(model.taxex_1) ? "" : model.taxex_1;
			a.Tax2Exempt = string.IsNullOrEmpty(model.taxex_2) ? "" : model.taxex_2;
			a.Tax3Exempt = string.IsNullOrEmpty(model.taxex_3) ? "" : model.taxex_3;
			a.Tax4Exempt = string.IsNullOrEmpty(model.taxex_4) ? "" : model.taxex_4;

			a.Save();
			//try
			//{
			//	NEAddress.sync_bvs(a, new NeMember(UserId));
			//}
			//catch (Exception e)
			//{
			//	Console.Write(e.Message);
			//}
			return "Address has been saved Successfully.";
		}


		public DataExtra SaveAddress(CustomerAddress model)
		{
			var a = model.address_id <= 0 ? new NEAddress() : new NEAddress(model.address_id);
			a.Table = model.table;
			a.Table_ID = model.customer_id;
			a.RVAccount = "00000";
			a.Desc = model.description;
			a.Addr1 = model.address_line_1;
			a.Addr2 = model.address_line_2;
			a.Addr3 = model.address_line_3;
			a.Addr4 = model.address_line_4;
			a.City = model.address_city;
			a.Prov = model.address_prov;
			a.Country = model.address_country;
			a.Postal = model.address_postalcode;
			a.Type = model.address_id <= 0 ? "S" : a.Type;
			a.PhoneArea = model.phone_area;
			a.Phonefirst = model.phone_prefix;
			a.PhoneLast = model.phone_suffix;
			a.PhoneExt = model.phone_ext;
			a.FaxArea = model.fax_area;
			a.FaxFirst = model.fax_prefix;
			a.FaxLast = model.fax_suffix;
			a.Web = model.website;
			a.GPS_Coords = model.gps_coordinates;
			a.facebook = model.facebook;
			a.twitter = model.twitter;
			a.linkedin = model.linkedin;
            a.Active = model.Active;
			a.Save();
			new CustomerHomeSearch(CurrentUser).ClearCache();


			// create customer address folder for new address
			if (model.address_id <= 0)
			{

			}
			//try
			//{
			//	NEAddress.sync_bvs(a, new NeMember(UserId));
			//}
			//catch (Exception e)
			//{
			//	Console.Write(e.Message);
			//}

			return new DataExtra()
			{
				Data = "Address saved successfully.",
				Extra = GetAddresses(model.customer_id)
			};
		}

		public DataExtra ChangeQC(int type)
		{
			switch (type)
			{
				case 1:
					return ApproveQC1();
				case 2:
					return ApproveQC2();
				case 3:
					return RemoveQC1();
				case 4:
					return RemoveQC2();
			}
			return null;
		}

		public DataExtra RemoveQC1()
		{
			bllToolbox.doSQL_void(@"
					UPDATE 
						customer 
					SET 
						customer_qc_datetime = NULL, 
						customer_qc_member_id = NULL 
					WHERE 
						customer_id = @v0 
					", customer_id);
			return new DataExtra()
			{
				Data = "Success.",
				Extra = ""
			};
		}

		public DataExtra RemoveQC2()
		{
			bllToolbox.doSQL_void(@"
					UPDATE 
						customer 
					SET 
						customer_qc2_datetime = NULL, 
						customer_qc2_member_id = NULL 
					WHERE 
						customer_id = @v0 
					", customer_id);
			return new DataExtra()
			{
				Data = "Success.",
				Extra = ""
			};
		}

		public DataExtra ApproveQC1()
		{
			bllToolbox.doSQL_void(@"
					UPDATE 
						customer 
					SET 
						customer_qc_datetime = NOW(), 
						customer_qc_member_id = @v0 
					WHERE 
						customer_id = @v1
					", UserId, customer_id);
			return new DataExtra()
			{
				Data = "Success.",
				Extra = bllToolbox.doSQL_string(@" SELECT DATE_FORMAT(customer_qc_datetime, '%Y-%m-%d') FROM customer WHERE customer_id = @v0  LIMIT 1", customer_id)
			};
		}


		public DataExtra ApproveQC2()
		{
			bllToolbox.doSQL_void(@"
					UPDATE 
						customer 
					SET 
						customer_qc2_datetime = NOW(), 
						customer_qc2_member_id = @v0 
					WHERE 
						customer_id = @v1
					", UserId, customer_id);
			return new DataExtra()
			{
				Data = "Success.",
				Extra = bllToolbox.doSQL_string(@" SELECT DATE_FORMAT(customer_qc2_datetime, '%Y-%m-%d') FROM customer WHERE customer_id = @v0  LIMIT 1", customer_id)
			};
		}
	}
}