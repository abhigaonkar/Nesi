using System;
using System.Linq;
using nesi.core;
using NESI.BLL.Base;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Shared
{
	public class NewCustomerContact : BLLBase
	{
		public LabelValueInt[] GetCustomerTitles()
		{
			return _db.titles.Select(x => new LabelValueInt()
			{
				Label = x.title_name,
				Value = x.title_id
			}).ToArray();
		}

        public DTO.ViewModels.Page.Shared.CustomerContact GetContact(int Contact_ID)
        {
            var contact = new NEContact(Contact_ID);

            DTO.ViewModels.Page.Shared.CustomerContact model = new DTO.ViewModels.Page.Shared.CustomerContact();

            model.Contact_ID = contact.Contact_ID;
            model.Title = contact.Contact_Title;
            model.Email = contact.Contact_Email;
            model.Phone = contact.Contact_CellPhone;
            model.Name = contact.Contact_Name;
            model.Customer_id = contact.customer_id;

            return model;

        }

        public DTO.ViewModels.Core.DataExtra AddNew(int custId, int addId,DTO.ViewModels.Page.Shared.NewCustomerContact model)
		{
			var this_contact = new NEContact
			{
				Contact_Cust_ID = model.Customer_id,
				Contact_Type = "Customer",
				Contact_Status = "Active",
				Contact_Name = model.Name,
				address_id = addId,
				customer_id = model.Customer_id,
				status = "Active",
				status_id = 1,
				name = model.Name,
				type = "Customer"
			};
			var c = bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM contact WHERE contact_name = @v0  AND contact_cust_id = @v1  AND contact_type = 'Customer'", model.Name, model.Customer_id);
			if (c > 0)
			{
				return new DataExtra("Duplicate Contact Detected");
			}
			this_contact.Contact_Title = model.Title;
			this_contact.Contact_Email = model.Email;
			this_contact.Contact_CellPhone = model.Phone;
			try
			{
				//	var temp_address_id =
				//		Toolbox.doSQL_int(
				//			@"SELECT IFNULL(MAX(address_id),0) FROM address where address_table = 'Customer' AND address_table_id = @v0 ",
				//			new object[] { model.Customer_id });
				//this_contact.address_id = temp_address_id;
				this_contact.AddNEContact(this_contact);
			}
			catch
			{
				return new DataExtra("There was an error saving this contact.");
			}
			return new DTO.ViewModels.Core.DataExtra
			{
				Data = "Contact has been saved successfully.",
				Extra = this_contact.id
			};
		}


        public DTO.ViewModels.Core.DataExtra EditContact(DTO.ViewModels.Page.Shared.CustomerContact model)
        {
            try
            {
                var contact = new NEContact(model.Contact_ID);

                contact.Contact_Name = model.Name;
                contact.Contact_Title = model.Title;
                contact.Contact_Email = model.Email;
                contact.Contact_CellPhone = model.Phone;

                contact.save();
            }
            catch (Exception ex)
            {
                return new DataExtra("There was an error saving this contact.");
            }

            return new DTO.ViewModels.Core.DataExtra
            {
                Data = "Contact has been saved successfully.",
                Extra = model.Contact_ID
            };
        }


    }
}