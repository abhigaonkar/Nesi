using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Vendors
{
	public class VendorNESIContactsGrid : BLLGridBase<DTO.ViewModels.Page.Vendors.VendorNESIContactsGrid>
	{
		public VendorNESIContactsGrid()
		{

		}

		public VendorNESIContactsGrid(Employee user) : base(user)
		{

		}

		public VendorNESIContactsGrid(Employee user, BodyParams param, int vendor_id) : base(user, param)
		{
			this.query_params = new object[] { vendor_id };
			this.query = $@"SELECT
  contact_cust_id vendor_id,
  contact_name,
  contact_id,
  contact_Cellphone,
  contact_directline,
  contact_extension,
  contact_email,
  contact_password,
  contact_status,
  contact_login_enabled as login_enable,
  customer_or_contact_status_id
FROM
  contact,
  customer_or_contact_status
WHERE contact_cust_id = @p0
  AND contact_type = 'Vendor'
  AND customer_or_contact_status_id = contact_status_id";
		}

		public DataExtra Save(DTO.ViewModels.Page.Vendors.VendorNESIContactsGrid model)
		{
			if (model.login_enable)
			{
				if (string.IsNullOrEmpty(model.contact_email))
				{
					return new DataExtra("If NESI login is Active, the email can't be BLANK");
				}

				if (string.IsNullOrEmpty(model.contact_password))
				{
					return new DataExtra("If NESI login is Active, the password can't be BLANK");
				}

			}

			var contact = model.contact_id.GetValueOrDefault(0) == 0 ? new NEContact() : new NEContact(model.contact_id.GetValueOrDefault());
			contact = (NEContact)MapperFrom(contact, model);
			contact.Contact_Login_enabled = model.login_enable ? 1 : 0;
			contact.Contact_Type = "Vendor";
			contact.Contact_Cust_ID = model.vendor_id;
			if (string.IsNullOrEmpty(model.contact_status))
			{
				contact.Contact_Status = "Active";
			}
			if (model.contact_id.GetValueOrDefault(0) == 0)
			{
				var x = 0;
				x = bllToolbox.doSQL_int(@"Select count(contact_id) from contact  
where contact_name like  CONCAT('%',@v0,'%')  and contact_cust_id=@v1 ", model.contact_name, model.vendor_id);
				if (x > 0)
				{
					return new DataExtra("Sorry a contact with this name already exists for this vendor.");
				}
				if (!string.IsNullOrEmpty(model.contact_email))
				{
					x = bllToolbox.doSQL_int(@"Select count(contact_id)
from contact  where contact_email like  CONCAT('%',@v0,'%') 
and contact_cust_id=@v1 ", model.contact_email, model.vendor_id);
					if (x > 0)
					{
						return new DataExtra("Sorry a contact with this email already exists for this vendor.");
					}
				}
				if (!string.IsNullOrEmpty(model.contact_directline))
				{
					x = bllToolbox.doSQL_int(@"Select count(contact_id) from contact  
where replace(replace(replace(REPLACE(contact_directline,'-',''),'(',''),')',''),' ','') 
like  CONCAT('%',@v0,'%')  and contact_cust_id=@v1 ", model.contact_directline.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim(), model.vendor_id);
					if (x > 0)
					{
						return new DataExtra("Sorry a contact with this direct line already exists for this vendor.");
					}
				}
				contact.AddNEContact(contact);
			}
			else
			{
				contact.save();
			}
		//	var NSI = new NetSuite_Integration.Vendor();
			var vendobj = new NEVendor(model.vendor_id);
		//	NSI.SyncNetSuite(vendobj, CurrentUser.TaxEntityId);
			return new DataExtra("Vendor has been saved successfully.");
		}

		public DataExtra Delete(int id)
		{
			var x = 0;
			x = bllToolbox.doSQL_int(@"Select count(woprog_id) from woprog  where woprog_contact_id =@v0", id);
			if (x > 0)
			{
				return new DataExtra("You can't delete this contact, it's found on " + x + " work order(s). You need to merge it.");
			}
			x = 0;
			x = bllToolbox.doSQL_int(@"Select count(quote_id) from quote_master  where contact_id =@v0", id);
			if (x > 0)
			{
				return new DataExtra("You can't delete this contact, it's found on " + x + " quote(s).  you need to merge it.");
			}
			try
			{
				bllToolbox.doSQL_void(@"Delete from contact  where contact_id =@v0 limit 1 ", id);
			}
			catch
			{
				return new DataExtra("This thing crashed during the deletion of this contact!!");
			}
			return new DataExtra("Vendor has been deleted successfully.");
		}


		public LabelValueString[] GetStatusList()
		{
			return new[]
			{
				new LabelValueString("Active"),
				new LabelValueString("InActive"),
			};
		}
	}
}