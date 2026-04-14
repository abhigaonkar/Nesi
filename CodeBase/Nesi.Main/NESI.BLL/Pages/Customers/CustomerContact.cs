using System;
using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerContact : CustomerBase
	{
		public int address_id { get; set; }
        public bool Isprivilege { get; set; }

        public CustomerContact(Employee user) : base(user)
		{
		}

		public CustomerContact(Employee user, int cust_id, int add_id) : base(user)
		{
			this.customer_id = cust_id;
			this.address_id = add_id;
            this.Isprivilege = CurrentUser.AuthenticatedForPrivilege(99);
		}

		public new object Profile()
		{
			return new
			{
				lists = new
				{
					statusList = GetStatusList(),
					privilegeTemplateList = GetPrivilegeTamplateList(),
					activeList = new[]
					{
						new LabelValueString {Label = "Active", Value = "Active"},
						new LabelValueString {Label = "InActive", Value = "InActive"}
					},
					locationList = GetLocationList(),
                    this.Isprivilege
                },
				data = GetContactList(),
              
			};
		}

		public LabelValueInt[] GetLocationList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				@"SELECT '0' value, 'Select an Address' label 
					UNION
					SELECT address_id value, 
					CONCAT('(', address_type, ') - ', URLDECODE(address_addr1)) label
					FROM address WHERE address_table IN('Customer', 'Worksite') 
					AND address_table_id = @p0", customer_id);
		}

		public LabelValueInt[] GetPrivilegeTamplateList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				@"SELECT id value, name label FROM pageprivilege_template WHERE type = 2");
		}

		public DTO.ViewModels.Page.Customers.CustomerContact[] GetContactList()
		{
			return bllToolbox.doSQL_Array<DTO.ViewModels.Page.Customers.CustomerContact>(@"SELECT
			a.contact_name,
			a.name_first,
			a.name_last,
			a.contact_id,
			a.contact_cellphone,
			a.contact_title,
			a.contact_directline,
			a.contact_extension,
			a.contact_email,
			a.contact_password,
			a.contact_status,
			a.facebook,
			a.twitter,
			a.linkedin,
			a.contact_login_enabled login_enabled,
	        b.customer_or_contact_status contact_status_label,
			a.contact_status_id as contact_status_id,
			a.address_id,
			a.contact_cust_id customer_id,
			a.stopsurveys,
			(select count(woprog.WOProg_ID) from woprog where woprog.woprog_customer_id = a.contact_cust_id and WOProg_Contact_ID = a.contact_id and a.contact_type = 'Customer') wos,
			(select count(quote_master.quote_id) from quote_master where quote_master.customer_id = a.contact_cust_id and quote_master.Contact_ID = a.contact_id and a.contact_type = 'Customer') quotes
				FROM
			contact a
			LEFT JOIN
			customer_or_contact_status b ON
			b.customer_or_contact_status_id = a.contact_status_id
			WHERE
			a.contact_cust_id = @p0 AND
			a.address_id = @p1 AND
			a.contact_type = 'Customer'", customer_id, address_id);
		}

		public DataExtra SaveContact(DTO.ViewModels.Page.Customers.CustomerContact model)
		{

			var contact = new NEContact();
			if (model.contact_id != 0)
			{
				contact = new NEContact(model.contact_id);
			}
			contact.name = model.contact_name;
			contact.name_first = model.name_first;
			contact.name_last = model.name_last;
			contact.status = model.contact_status;
			contact.login_enabled = model.login_enabled ? 1 : 0;
			contact.status_id = model.contact_status_id;
			contact.Contact_Status = model.contact_status;
			contact.extension = model.contact_extension;
			contact.cellphone = model.contact_cellphone;
			contact.direct_line = model.contact_directline;
			contact.title = model.contact_title;
			contact.email = model.contact_email;
			contact.type = "Customer";
			contact.customer_id = customer_id;
			contact.address_id = address_id;
			contact.facebook = model.facebook;
			contact.twitter = model.twitter;
			contact.linkedin = model.linkedin;
			contact.stopsurveys = model.stopsurveys;
			contact.save();
			if (!contact.stopsurveys)
			{
				if (bllToolbox.doSQL_int(
						@"Select count(contactpage_id) from contactpage  where contactpage_page_id = 161 and contactpage_contact_id =@v0",
						contact.Contact_ID) == 0)
				{
					bllToolbox.doSQL_void(
						@"insert into contactpage (contactpage_contact_id,contactpage_page_id,contactpage_member_id) values(@v0,@v1,@v2)",
						contact.Contact_ID, 161, (Convert.ToInt32(contact.Contact_ID) + 100000));
				}
			}
			if (contact.login_enabled == 1)
			{
				check_home_page_access(contact.Contact_ID);
			}
			new CustomerHomeSearch(CurrentUser).ClearCache();
			return new DataExtra()
			{
				Data = "Success.",
				Extra = contact
			};
		}

		protected void check_home_page_access(int contactid)
		{
			if (contactid != 0)
			{
				var co = new NEContact(contactid);
				var user = new NeMember(co.nesi_member_id);
				if (!user.AuthenticatedForPage(1))
				{
					if (!NEUserPage.exists((int)contactid, 1, 2))
					{
						var NE_up = new NEUserPage
						{
							admin = new NeMember(UserId),
							user = user,
							page_id = 1,
							user_id = (int)contactid,
							type_id = 2
						};
						// type 2  = contact
						NE_up.save();
					}
				}

			}
		}

		public DataExtra Delete(int contactid)
		{
			var errMssg = NEContact.delete(contactid);
            if (string.IsNullOrWhiteSpace(errMssg))
            {
                new CustomerHomeSearch(CurrentUser).ClearCache();
                return new DataExtra()
                {
                    Data = "success.",
                    Extra = null
                };
            }
            else
            {
                return new DataExtra()
                {
                    Data = errMssg,
                    Extra = null
                };
            }
		}

		public bool Is_DuplicateName(string name, int contactid = 0)
		{
			return bllToolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact
					WHERE contact_name LIKE CONCAT('%',@v0,'%') AND contact_cust_id = @v1 and contact_id!=@v3", name.Trim(), customer_id, address_id, contactid) > 0;
		}

		public bool Is_DuplicateEmail(string email, int contactid = 0)
		{
			return bllToolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact WHERE contact_email LIKE CONCAT('%',@v0,'%')  AND contact_cust_id = @v1  and contact_id!=@v3", email.Trim(), customer_id, address_id, contactid) > 0;
		}

		public bool Is_DuplicateCellPhone(string phone, int contactid = 0)
		{
			return bllToolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact WHERE contact_cellphone LIKE CONCAT('%',@v0,'%') AND contact_cust_id = @v1  and contact_id!=@v3", phone.Trim(), customer_id, address_id, contactid) > 0;
		}

		public bool Is_DuplicateDirectLine(string phone, int contactid = 0)
		{
			return bllToolbox.doSQL_int(@"SELECT COUNT(contact_id) FROM contact WHERE contact_directline LIKE CONCAT('%',@v0,'%')  AND contact_cust_id = @v1   and contact_id!=@v3", phone.Trim(), customer_id, address_id, contactid) > 0;
		}
	}
}