using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
	public class CustomerSurveysGrid : BLLGridBase<DTO.ViewModels.Page.Reports.CustomerSurveysGrid>
	{
		public CustomerSurveysGrid()
		{
		}

		public CustomerSurveysGrid(Employee user) : base(user)
		{
		}

		public CustomerSurveysGrid(Employee user, BodyParams param) : base(user, param, new object[] { })
		{
			string querystring = @"SELECT wo_survey.id id, wo_survey.contact_id, wo_survey.date date, woprog.WOProg_BVWO bvwo, 
woprog.woprog_description, woprog.woprog_customername cust_name, contact.Contact_Name name, ifnull(wo_survey.rating, 0.0) rating, wo_survey.notes, 
if(wo_survey.clean=1,'Yes','No') clean, if(wo_survey.ontime=1,'Yes','No') ontime, if(wo_survey.callme=1,'Yes','No') callme ,woprog.woprog_id,woprog.business_unit_id, woprog.woprog_customer_id customer_id
FROM wo_survey 
INNER JOIN woprog ON wo_survey.woprog_id = woprog.WOProg_ID 
INNER JOIN contact ON wo_survey.contact_id = contact.Contact_ID
";

			if (user.ExtraType == "customer")
			{
				var customer = (Core.Member.Customer)user.ExtraUser;
				if (customer != null)
				{
					this.query = querystring + " where wo_survey.contact_id = " + customer.CustomerId;
				}
			}
			else
			{
				this.query = querystring + " where find_in_set(woprog.business_unit_id,'{bu_ids}')";
			}
		}
	}
}
