using System;
using System.Data;
using System.Linq;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Quotes
{
	public class QuoteNew : QuoteBLLBase
	{
		public LabelValueInt[] BusinessUnitList { get; set; }

        // public LabelValueInt[] RevenueLine { get; set; }

        public LabelValueInt[] UserListInBusinessUnit { get; set; }
		public LabelValueInt[] Qualifiers { get; set; }

		public QuoteNew(Employee user) : base(user)
		{
			BusinessUnitList = GetBusinessUnitList();
            // RevenueLine=GetRevenueLine();
            UserListInBusinessUnit = GetUserListByBusinessUnit(user.BusinessUnitId, false).ToArray();
			Qualifiers = GetQualifiers();
		}


		public DTO.ViewModels.Core.DataExtra Save(DTO.ViewModels.Page.Quotes.QuoteNew model)
		{
			var _tools = new Toolbox();
			var quote_value = model.Expected_value;
			var quote_id = "0";
			var q = new nesi.core.quote
			{
				business_unit_id = model.Quote_businessUnit,
                // revenue_line_id= model.revenueLine,
				bdm =model.bdm,
                contact_id = model.Customer_contact,
				cust_id = model.Customer_id,
			    date_due = model.Date_due.ToString("yyyy-MM-dd"),
			    date_expected_start = model.Date_expected_start.ToString("yyyy-MM-dd"),
                quoted_by = model.Managed_by,
				completion_date = model.Date_invoiced.ToString("yyyy-MM-dd"),
				txtJobDescription = model.Job_description,
				address_id = model.Customer_address,
				pct_chance = model.Chance_winning
			};

			var expected_value = model.Expected_value;
			var addr = new NEAddress(q.address_id);

			var comp = new NeBusinessUnit(model.Quote_businessUnit);
			var cust = new NECustomer(model.Customer_id);

            var customer_term_id = cust.customer_term_id;
            


            if (comp.uses_quote_process == 1)
			{
				quote_id = _tools.getSQL_string(@"CALL StartQuote_Process_beta(@v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6 , @v7,@v8,@v9,@v10 )", new object[] { q.quoted_by, q.cust_id, q.contact_id, q.date_due, q.date_expected_start, q.txtJobDescription, q.business_unit_id, quote_value,q.revenue_line_id, customer_term_id,q.bdm });
			}
			else
			{
				quote_id = _tools.getSQL_string(@"CALL StartQuote_beta(@v0 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6,@v7,@v8 )", new object[] { q.quoted_by, q.cust_id, q.contact_id, q.date_due, q.date_expected_start, q.txtJobDescription, q.business_unit_id,q.revenue_line_id, customer_term_id });
			}
			var status_id = comp.uses_quote_process == 1 && expected_value >= comp.quote_level_2_start ? 10 : 1;
            //#2807 added - setting the BDM from the customer
            //var bdm = GetBDMByCustomerId(q.cust_id,q.business_unit_id);

			Toolbox.doSQL_void(@"UPDATE quote_master SET expected_value = @v0 , completion_date = @v1 , address_id = @v3 , status_id = @v4 , pct_chance=@v5, bdm = @v6  WHERE quote_id = @v2  AND active_revision = 1 LIMIT 1", new object[] { expected_value, q.completion_date, quote_id, q.address_id, status_id, q.pct_chance, q.bdm });

			var body = @"<style type='text/css'>
table.stat td { font-family: Arial, Helvetica, sans-serif; font-size:9pt; }
table.sstat td { font-family: Arial, Helvetica, sans-serif; font-size:9pt; }

</style><table class='stat'><tr><td><b>Opportunity Details:</b></td><td></td></tr>";
			body += "<tr><td><b>Quote ID:</b></td><td><a href='" + Toolbox.app_setting("Domain") + "/sections/member/quote/index.aspx?a=g&quote_id=" + quote_id + "&revision=1'>" + quote_id + "</a></td></tr>";
			body += "<tr><td><b>Business Unit:</b></td><td>" + model.Business_unit_name + "</td></tr>";
			body += "<tr><td><b>Customer:</b></td><td> " + model.Customer_name + "</td></tr>";
			body += "<tr><td><b>Contact:</b></td><td> " + model.Customer_contact_name + "</td></tr>";
			body += "<tr><td><b>Raised By:</b></td><td> " + model.Managed_by_name + "</td></tr>";
			body += "<tr><td><b>BDM:</b></td><td> " + model.bdm + "</td></tr>";
			body += "<tr><td><b>Due Date:</b></td><td> " + q.date_due + "</td></tr>";
			body += "<tr><td><b>Due Date:</b></td><td> " + q.date_expected_start + "</td></tr>";
			body += "<tr><td><b>Exp Comp Date:</b></td><td> " + q.completion_date + "</td></tr>";
			body += "<tr><td><b>Exp Value:</b></td><td> " + expected_value.ToString("C2") + "</td></tr>";
			body += "<tr><td><b>Chance of Winning:</b></td><td> " + q.pct_chance + "%</td></tr>";
			body += "<tr><td><b>Description:</b></td><td> " + q.txtJobDescription + "</td></tr>";
			body += "</td></tr></table><table class='sstat'><tr>";
			var emaillist = "";

			foreach (var l in Qualifiers)
			{
				var qq = l.Value;
				var qu = l.Label;
				var i = !(model.Qualifiers is null) && model.Qualifiers.Contains(l.Value);

				Toolbox.doSQL_void(@"Insert into quote_filter (quote_id,date,memberid,question_id,result)  values(@v0,@v1,@v2,@v3,@v4)", new object[] { quote_id, System.DateTime.Today.ToString("yyyy-MM-dd"), current_user.id, qq, i });
				if (Toolbox.doSQL_int(@"Select kills_quote from quote_filter_questions  where id =@v0", new object[] { qq }) > 0)
				{
					if (!i)
					{
						body += "<td>" + qu + "</td><td>" + "No" + "</td></tr>";
					}
				}
				else
				{
					body += "<td>" + qu + "</td><td>" + (i ? "Yes" : "No") + "</td></tr>";
				}

			}
			body += "</table>";
			var ee = new NeEMail { Subject = "Opportunity Alert" };

			#region companies not on quote process
			if (comp.uses_quote_process != 1)
			{
				if (current_user.id != comp.branch_manager.id)
				{
					emaillist += comp.branch_manager.NEEmail + ";";
				}
				try
				{
					if (quote_value >= 149000)  // if it's big enough, add the bm's boss and hector and andy to the list
					{
						// send this email with a passport link back into the first approval screen
						if (comp.branch_manager.reports_to != 0)
						{
							emaillist += new NeMember((int)comp.branch_manager.reports_to).NEEmail + ";";
						}
						
					}
				}
				catch (Exception eee) { _tools.catch_error(eee); }
			}
            #endregion
            // This will be changed over to the "Director of sales" at some point
            var hasDirOfSales= Toolbox.doSQL_int("SELECT COUNT(*) FROM member WHERE member_membertype_id = 81 AND member_status = 'Active'", new object[] { })>0;
            if (hasDirOfSales)
            {
                emaillist += Toolbox.doSQL_string("SELECT GROUP_CONCAT(member_neemail SEPARATOR ';') FROM member WHERE member_membertype_id = 81 AND member_status = 'Active'", new object[] { }) ;
            }
			// We should always be notifying the reps when a quote is cut, per ticket 5472
			//ee.CC += addr.csp.am_member_id != 0 ? new NeMember((int)addr.csp.am_member_id).NEEmail + ";" : "";
		//	ee.CC += addr.csp.osr_member_id != 0 ? new NeMember((int)addr.csp.osr_member_id).NEEmail + ";" : "";
			ee.CC += addr.csp.ram_member_id != 0 ? new NeMember((int)addr.csp.ram_member_id).NEEmail + ";" : "";
		//	ee.CC += addr.csp.isr_member_id != 0 ? new NeMember((int)addr.csp.isr_member_id).NEEmail + ";" : "";

			#region companies on the quote process
			if (comp.uses_quote_process == 1)
			{
				if (quote_value > comp.quote_level_3_start)
				{
					emaillist += new NeMember((int)comp.branch_manager.reports_to).NEEmail + ";";
					ee.Subject = "Level 3 quote requires " + new NeMember((int)comp.branch_manager.reports_to).FullName + "'s approval prior to starting.";
					
				}
				else if (quote_value > comp.quote_level_2_start)
				{
					emaillist += comp.branch_manager.NEEmail;
					ee.Subject = "Level 2 quote requires " + comp.branch_manager.FullName + "'s approval prior to starting.";
				}
				else
				{
					if (comp.branch_manager != current_user)
					{
						emaillist += comp.branch_manager.NEEmail + ";";
					}
				}

			}
			#endregion


			ee.To = emaillist;
			ee.isHTML = true;
			ee.Body = body;
			if (emaillist != "")
			{
				ee.Send();
			}

			if (model.quoter_locked)
			{
				LockQuote(quote_id);
			}

			return new DTO.ViewModels.Core.DataExtra
			{
				Data = "Quote has been saved successfully.",
				Extra = quote_id
			};
		}

        public string GetCustomerNameByCustomerId(int id)
        {
			return Toolbox.doSQL_string(@"SELECT customer_name FROM customer WHERE customer_id = @v0 ", new object[] { id });
		}

        public LabelValueInt[] GetQualifiers()
		{
			return _db.quote_filter_questions.Where(x => x.status == "Active").Select(x => new LabelValueInt()
			{
				Label = x.filter_question,
				Value = x.id
			}).ToArray();
		}

		public LabelValueInt[] FilterCustomer(string filter, string customerId,string buid,string quote_id,string version)
		{
		    var customer_id = 0;
		    if (!string.IsNullOrWhiteSpace(customerId))
		    {
		        customer_id = Convert.ToInt32(customerId);
		    }

            var visibleBusinessUnit = buid; // default value 

           


            string sql1 = @"
SELECT
  VALUE,
  Label
FROM
  (SELECT DISTINCT
    customer.Customer_ID VALUE,
    CONCAT(
      '(',
      customer.Customer_Number,
      ') ',
      customer.Customer_Name,
      (
        IF(
          customer.Customer_Hold = 'T',
          '- On Hold',
          '- Active'
        )
      )
    ) Label
  FROM
    customer
    INNER JOIN customer_business_unit
      ON customer_business_unit.customer_id = customer.customer_id
  WHERE CONCAT(
      '(',
      customer.Customer_Number,
      ')',
      customer.Customer_Name,
      (
        IF(
          customer.Customer_Hold = 'T',
          '- On Hold',
          '- Active'
        )
      )
    ) LIKE CONCAT('%', @p0, '%')
    AND FIND_IN_SET(
      customer_business_unit.business_unit_id,
      @p1
    ) and customer.Active=true
 ";

            const string sql2 = @"UNION
  SELECT DISTINCT
    customer.Customer_ID VALUE,
    CONCAT(
      '(',
      customer.Customer_Number,
      ') ',
      customer.Customer_Name,
      (
        IF(
          customer.Customer_Hold = 'T',
          '- On Hold',
          '- Active'
        )
      )
    ) Label
  FROM
    customer
  WHERE customer.customer_id = @p2 and customer.Active=true) a
ORDER BY a.Label";


            if (Convert.ToInt32(quote_id) > 0 && Convert.ToInt32(version)==1)
            {
                int statusId = Toolbox.doSQL_int(@"SELECT status_id FROM neintranet.quote_master Where quote_id=@v0", quote_id);
                if (statusId != 1)  // exlude status of quote is "Waiting to be Quoted"(id=1)
                {
                    sql1 = sql1 + " and customer_business_unit.customer_id= @p2  ";
                }
            }

             string sql = sql1 + sql2;

            return _db.Database.SqlQuery<LabelValueInt>(sql, filter, visibleBusinessUnit, customer_id).ToArray();
		}

		public LabelValueInt[] GetCustomerAddressByCustomerId(int id)
		{
            return _db.address.Where(x => (x.address_table == "Customer" || x.address_table == "Worksite") && x.address_table_id == id && x.Active == true)
				.Select(x => new LabelValueInt()
				{
					Label = x.Address_Addr1 + " " + x.Address_City,
					Value = x.address_id
				}
				).ToArray();
		}

		public LabelValueInt[] GetCustomerContactByCustomerId(int id)
		{
			// @"Select contact_id id, contact_name _name from contact  where contact_cust_id =@v0 and contact_status = 'Active' order by contact_name"
			return _db.contact.Where(x => x.Contact_Status == "Active" && x.Contact_Cust_ID == id).OrderBy(x => x.Contact_Name)
				.Select(x => new LabelValueInt()
				{
					Label = x.Contact_Name,
					Value = x.Contact_ID
				}
				).ToArray();
		}

		public DataTable GetQuotesByCustomerId(int id)
		{
			const string sql = @"SELECT quote_master.quote_id AS quoteid,
						quote_master.active_revision AS rev, 
						quote_status.`status`, 
						get_name(quote_master.quoted_by) AS quotedby, 
						quote_master.job_description AS description, 
						quote_master.customer_id 
						FROM quote_master INNER JOIN quote_status ON quote_master.status_id = quote_status.id 
						where status_id <6 and quote_master.customer_id =@p0";
			return _db.DataTable(sql, id);
		}

		public object GetBDMByCustomerId(int customerId, int buid)
		{
			//Get RAM from customer_business_unit or fallback to customer.ram based on key account
			const string ramSql = @"
					SELECT 
					 CAST(
							CASE
									WHEN cbu.ram IS NOT NULL THEN cbu.ram
									WHEN cbu.ram IS NULL AND c.ram IS NOT NULL AND (c.key_account = 0 OR c.key_account IS NULL) THEN c.ram
								    WHEN cbu.ram IS NOT NULL  AND c.key_account = 1 THEN cbu.ram
							 ELSE 0
							END AS SIGNED
						 ) AS ram_id
				FROM customer_business_unit cbu
				INNER JOIN customer c ON c.customer_id = cbu.customer_id
				WHERE cbu.customer_id = @v0
				  AND cbu.business_unit_id = @v1
				LIMIT 1";

			// Get the RAM ID
			int ramId = Toolbox.doSQL_int(ramSql, new object[] { customerId, buid });
			int? selectedRamId = ramId > 0 ? (int?)ramId : null;

			// 2️⃣ Get full BDM list
			const string allBdmSql = @"
        SELECT netsuite_employee_internal_id AS Value,
               netsuite_employee_name AS Label
        FROM netsuite_sales_rep
        WHERE netsuite_isinactive = 0
          AND netsuite_issales_rep = 1;
    ";

			var bdmList = bllToolbox.doSQL_Array<LabelValueInt>(allBdmSql, new object[] { }).ToArray();

			// 3️⃣ Reorder list to put selected RAM first if exists
			if (selectedRamId.HasValue)
			{
				bdmList = bdmList
					.OrderByDescending(x => x.Value == selectedRamId.Value) // RAM first
					.ThenBy(x => x.Label) // Then alphabetical
					.ToArray();
			}
			else
			{
				bdmList = bdmList.OrderBy(x => x.Label).ToArray(); // Alphabetical if no RAM
			}

			// 4️⃣ Return list + selected value
			return new
			{
				BdmList = bdmList,
				SelectedBdm = selectedRamId
			};
		}




	}
}