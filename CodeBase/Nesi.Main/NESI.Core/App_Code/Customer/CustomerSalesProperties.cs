using DevExpress.Xpo;
using System;
using System.Linq;
using System.Web;

namespace nesi.core
	{

	public class customer_sales_properties
	{
		public customer_sales_properties previous_values { get; set; }
		public int customer_id { get; set; }
		public int industry { get; set; }
		public string sector { get; set; }
		public string naics_code { get; set; }
		public int employee_size { get; set; }
		public string do_at_location { get; set; }
		public string affiliated_companies { get; set; }
		public string known_suppliers { get; set; }
		public string known_competitors { get; set; }
		public string why_choose { get; set; }
		public int origin { get; set; }
		public int address_id { get; set; }
		public int member_id { get; set; }
		public int status_id { get; set; }
		public int call_cycle { get; set; }
		public int year_end { get; set; }
		public int project_mgr_member_id { get; set; }
		public int controls_mgr_member_id { get; set; }
		public int discount_pct { get; set; }
		public int decision_maker { get; set; }
		public string account_code { get; set; }
		public DateTime next_followup_date { get; set; }
		public string next_followup_notes { get; set; }
		public double job_budget_threshold { get; set; }
		public bool po_required { get; set; }
		public bool confirmed_po_required { get; set; }
		public bool confirmed_tax_exempt { get; set; }

		public int isr_member_id { get; set; }
		public int osr_member_id { get; set; }
		public int ram_member_id { get; set; }
		public int mam_member_id { get; set; }
		public int cisr_member_id { get; set; }
		public int am_member_id { get; set; }

		public string notes_public { get; set; }
		public string notes_sales { get; set; }

		public customer_sales_properties() { }
		public customer_sales_properties(int _address_id)
		{
			if (HttpContext.Current != null && HttpContext.Current.Session != null)
			{
				var _session = HttpContext.Current.Session;
				if (_session["session"] != null)
				{
					var current_user = _session["profile"] == null ? new NeMember(_session["session"].ToString()) : (NeMember)_session["profile"];
					member_id = current_user.id;
				}
			}
			else
			{
				member_id = 1;
			}
			address_id = _address_id;
			if (exists(_address_id))
			{
				load(_address_id);
			}
		}
		public bool exists(int _address_id)
		{
			using (var uow = new UnitOfWork())
			{
				var s = from x in new XPQuery<ne_xpo.cs.customer_sales_properties>(uow)
						where x.address_id == _address_id
						select x;
				return s.Count() > 0;
			}
		}
		public void save()
		{
			if (exists(address_id))
			{
				// Update
				Toolbox.doSQL_void(@" UPDATE customer_sales_properties SET industry = @v1 , sector = @v2 , naics_code = @v3 , employee_size = @v4 , do_at_location = @v5 , affiliated_companies = @v6 , known_suppliers = @v7 , known_competitors = @v8 , why_choose = @v9 , origin = @v10 , status_id = @v12 , call_cycle = @v13 , year_end = @v14 , project_mgr_member_id = @v15 , controls_mgr_member_id = @v16 , discount_pct = @v17 , decision_maker = @v18 , account_code = @v19 , next_followup_date = @v20 , next_followup_notes = @v21 , job_budget_threshold = @v22 , po_required = @v23 , confirmed_po_required = @v24 , confirmed_tax_exempt = @v25 , isr_member_id = @v26 , osr_member_id = @v27 , ram_member_id = @v28 , mam_member_id = @v29 , cisr_member_id = @v30 , notes_sales = @v31 , notes_public = @v32 , account_manager = @v33  WHERE address_id = @v11  LIMIT 1 ", 
					new object[] { customer_id, industry, sector, naics_code, employee_size, do_at_location, affiliated_companies,
						known_suppliers, known_competitors, why_choose, origin, address_id, status_id, call_cycle,
						year_end,
						project_mgr_member_id == 0 ? "NULL" : project_mgr_member_id.ToString(),
						controls_mgr_member_id == 0 ? "NULL" : controls_mgr_member_id.ToString(),
						discount_pct, decision_maker, account_code,
						next_followup_date.Year < 1900 ? "NULL" : Toolbox.MySQL_shortdt(next_followup_date),
						next_followup_notes, job_budget_threshold, po_required, confirmed_po_required, confirmed_tax_exempt,
						isr_member_id == 0 ? "NULL" : isr_member_id.ToString(),
						osr_member_id == 0 ? "NULL" : osr_member_id.ToString(),
						ram_member_id == 0 ? "NULL" : ram_member_id.ToString(),
						mam_member_id == 0 ? "NULL" : mam_member_id.ToString(),
						cisr_member_id == 0 ? "NULL" : cisr_member_id.ToString(),
						notes_sales, notes_public,
						am_member_id == 0 ? "NULL" : am_member_id.ToString()});
				if (previous_values != null)
				{
					log_changes();
				}
			}
			else
			{
				// Insert
				Toolbox.doSQL_void(@" INSERT INTO customer_sales_properties 
( customer_id, address_id, industry, sector, naics_code, employee_size, do_at_location, affiliated_companies, known_suppliers, known_competitors, why_choose, 
origin, status_id, call_cycle, year_end, project_mgr_member_id, controls_mgr_member_id, discount_pct, decision_maker, account_code, next_followup_date, next_followup_notes, 
job_budget_threshold, po_required, confirmed_po_required, confirmed_tax_exempt, isr_member_id, osr_member_id, ram_member_id, mam_member_id, cisr_member_id, notes_sales, notes_public, account_manager )
VALUES ( @v0 , @v11 , @v1 , @v2 , @v3 , @v4 , @v5 , @v6 , @v7 , @v8 , @v9 , @v10 , @v12 , @v13 , @v14 , @v15 , @v16 , @v17 , @v18 , @v19 , @v20 , @v21 , @v22 , 
@v23 , @v24 , @v25 , @v26 , @v27 , @v28 , @v29 , @v30 , @v31 , @v32 , @v33  ) ", 
new object[] { customer_id, industry, sector, naics_code, employee_size, do_at_location, affiliated_companies,
	known_suppliers, known_competitors, why_choose, origin, address_id, status_id, call_cycle, year_end,
	project_mgr_member_id == 0 ? "NULL" : project_mgr_member_id.ToString(),
	controls_mgr_member_id == 0 ? "NULL" : controls_mgr_member_id.ToString(),
	discount_pct, decision_maker, account_code,
	next_followup_date.Year < 1900 ? "NULL" : Toolbox.MySQL_shortdt(next_followup_date),
	next_followup_notes, job_budget_threshold, po_required, confirmed_po_required, confirmed_tax_exempt,
	isr_member_id == 0 ? "NULL" : isr_member_id.ToString(),
	osr_member_id == 0 ? "NULL" : osr_member_id.ToString(),
	ram_member_id == 0 ? "NULL" : ram_member_id.ToString(),
	mam_member_id == 0 ? "NULL" : mam_member_id.ToString(),
	cisr_member_id == 0 ? "NULL" : cisr_member_id.ToString(),
	notes_sales, notes_public,
	am_member_id == 0 ? "NULL" : am_member_id.ToString() });
			}
		}
		private void log_changes()
		{
			if (sector != previous_values.sector)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.sector, sector), 24, origin, address_id);
			}
			if (industry != previous_values.industry)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.industry, industry), 25, origin, address_id);
			}
			if (naics_code != previous_values.naics_code)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.naics_code, naics_code), 26, origin, address_id);
			}
			if (employee_size != previous_values.employee_size)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.employee_size, employee_size), 27, origin, address_id);
			}
			if (do_at_location != previous_values.do_at_location)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.do_at_location, do_at_location), 28, origin, address_id);
			}
			if (affiliated_companies != previous_values.affiliated_companies)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.affiliated_companies, affiliated_companies), 29, origin, address_id);
			}
			if (known_suppliers != previous_values.known_suppliers)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.known_suppliers, known_suppliers), 30, origin, address_id);
			}
			if (known_competitors != previous_values.known_competitors)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.known_competitors, known_competitors), 31, origin, address_id);
			}
			if (why_choose != previous_values.why_choose)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.why_choose, why_choose), 32, origin, address_id);
			}
			if (origin != previous_values.origin)
			{
				var name_old = previous_values.cisr_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT name FROM customer_origin WHERE id = @v0  LIMIT 1", new object[] { previous_values.origin });
				var name_new = Toolbox.doSQL_string(@"SELECT name FROM customer_origin WHERE id = @v0  LIMIT 1", new object[] { origin });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.origin, name_new, origin), 33, origin, address_id);
			}
			if (status_id != previous_values.status_id)
			{
				var name_old = previous_values.cisr_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT customer_or_contact_status FROM customer_or_contact_status WHERE customer_or_contact_status_id = @v0  LIMIT 1", new object[] { previous_values.status_id });
				var name_new = Toolbox.doSQL_string(@"SELECT customer_or_contact_status FROM customer_or_contact_status WHERE customer_or_contact_status_id = @v0  LIMIT 1", new object[] { status_id });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.status_id, name_new, status_id), 18, origin, address_id);
			}
			if (call_cycle != previous_values.call_cycle)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.call_cycle, call_cycle), 34, origin, address_id);
			}
			if (year_end != previous_values.year_end)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.year_end, year_end), 35, origin, address_id);
			}
			if (project_mgr_member_id != previous_values.project_mgr_member_id)
			{
				var name_old = previous_values.project_mgr_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { previous_values.project_mgr_member_id });
				var name_new = Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { project_mgr_member_id });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.project_mgr_member_id, name_new, project_mgr_member_id), 36, origin, address_id);
			}
			if (controls_mgr_member_id != previous_values.controls_mgr_member_id)
			{
				var name_old = previous_values.controls_mgr_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { previous_values.controls_mgr_member_id });
				var name_new = Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { controls_mgr_member_id });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.controls_mgr_member_id, name_new, controls_mgr_member_id), 37, origin, address_id);
			}
			if (discount_pct != previous_values.discount_pct)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}%' \nNew Value: '{1}%'", previous_values.discount_pct, discount_pct), 38, origin, address_id);
			}
			if (decision_maker != previous_values.decision_maker)
			{
				var name_old = previous_values.decision_maker == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT contact_name FROM contact WHERE contact_id = @v0  LIMIT 1", new object[] { previous_values.decision_maker });
				var name_new = Toolbox.doSQL_string(@"SELECT contact_name FROM contact WHERE contact_id = @v0  LIMIT 1", new object[] { decision_maker });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.decision_maker, name_new, decision_maker), 39, origin, address_id);
			}
			if (account_code != previous_values.account_code)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.account_code, account_code), 40, origin, address_id);
			}
			if (next_followup_date != previous_values.next_followup_date)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.next_followup_date, next_followup_date), 42, origin, address_id);
			}
			if (next_followup_notes != previous_values.next_followup_notes)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.next_followup_notes, next_followup_notes), 42, origin, address_id);
			}
			if (job_budget_threshold != previous_values.job_budget_threshold)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.job_budget_threshold, job_budget_threshold), 43, origin, address_id);
			}
			if (po_required != previous_values.po_required)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.po_required, po_required), 44, origin, address_id);
			}
			if (confirmed_po_required != previous_values.confirmed_po_required)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.confirmed_po_required, confirmed_po_required), 45, origin, address_id);
			}
			if (confirmed_tax_exempt != previous_values.confirmed_tax_exempt)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.confirmed_tax_exempt, confirmed_tax_exempt), 46, origin, address_id);
			}
			if (isr_member_id != previous_values.isr_member_id)
			{
				var name_old = previous_values.isr_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { previous_values.isr_member_id });
				var name_new = Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { isr_member_id });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", name_old, previous_values.isr_member_id, name_new, isr_member_id), 47, origin, address_id);
			}
			if (osr_member_id != previous_values.osr_member_id)
			{
				var name_old = previous_values.osr_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { previous_values.osr_member_id });
				var name_new = Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { osr_member_id });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.osr_member_id, name_new, osr_member_id), 48, origin, address_id);
			}
			if (ram_member_id != previous_values.ram_member_id)
			{
				var name_old = previous_values.ram_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { previous_values.ram_member_id });
				var name_new = Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { ram_member_id });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.ram_member_id, name_new, ram_member_id), 49, origin, address_id);
			}
			if (mam_member_id != previous_values.mam_member_id)
			{
				var name_old = previous_values.mam_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { previous_values.mam_member_id });
				var name_new = Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { mam_member_id });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.mam_member_id, name_new, mam_member_id), 50, origin, address_id);
			}
			if (cisr_member_id != previous_values.cisr_member_id)
			{
				var name_old = previous_values.cisr_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { previous_values.cisr_member_id });
				var name_new = Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { cisr_member_id });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.cisr_member_id, name_new, cisr_member_id), 51, origin, address_id);
			}
			if (notes_sales != previous_values.notes_sales)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.notes_sales, notes_sales), 52, origin, address_id);
			}
			if (notes_public != previous_values.notes_public)
			{
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}' \nNew Value: '{1}'", previous_values.notes_public, notes_public), 53, origin, address_id);
			}
			if (am_member_id != previous_values.am_member_id)
			{
				var name_old = previous_values.am_member_id == 0 ? "Blank" : Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { previous_values.am_member_id });
				var name_new = Toolbox.doSQL_string(@"SELECT MEMBER_NAME(@v0 )", new object[] { am_member_id });
				NECustomer.add_to_customer_history(customer_id, member_id, DateTime.Now, string.Format("Old Value: '{0}'({1}) \nNew Value: '{2}'({3})", name_old, previous_values.am_member_id, name_new, am_member_id), 54, origin, address_id);
			}
		}
		private void load(int _address_id)
		{
			using (var uow = new UnitOfWork())
			{
				var _csps = from x in new XPQuery<ne_xpo.cs.customer_sales_properties>(uow)
							where x.address_id == _address_id
							select x;
				if (_csps.Count() == 1)
				{
					var _csp = _csps.First();
					sector = _csp.sector;
					industry = _csp.industry;
					naics_code = _csp.naics_code;
					employee_size = _csp.employee_size;
					do_at_location = _csp.do_at_location;
					affiliated_companies = _csp.affiliated_companies;
					known_suppliers = _csp.known_suppliers;
					known_competitors = _csp.known_competitors;
					why_choose = _csp.why_choose;
					origin = _csp.origin;
					status_id = _csp.status_id;
					call_cycle = _csp.call_cycle;
					year_end = _csp.year_end;
					project_mgr_member_id = _csp.project_mgr_member_id == null ? 0 : Convert.ToInt32(_csp.project_mgr_member_id);
					controls_mgr_member_id = _csp.controls_mgr_member_id == null ? 0 : Convert.ToInt32(_csp.controls_mgr_member_id);
					discount_pct = _csp.discount_pct;
					decision_maker = _csp.decision_maker;
					account_code = _csp.account_code;
					next_followup_date = _csp.next_followup_date;
					next_followup_notes = _csp.next_followup_notes;
					job_budget_threshold = _csp.job_budget_threshold;
					po_required = _csp.po_required;
					confirmed_po_required = _csp.confirmed_po_required;
					confirmed_tax_exempt = _csp.confirmed_tax_exempt;
					customer_id = _csp.customer_id;
					isr_member_id = _csp.isr_member_id == null ? 0 : Convert.ToInt32(_csp.isr_member_id);
					osr_member_id = _csp.osr_member_id == null ? 0 : Convert.ToInt32(_csp.osr_member_id);
					ram_member_id = _csp.ram_member_id == null ? 0 : Convert.ToInt32(_csp.ram_member_id);
					mam_member_id = _csp.mam_member_id == null ? 0 : Convert.ToInt32(_csp.mam_member_id);
					cisr_member_id = _csp.cisr_member_id == null ? 0 : Convert.ToInt32(_csp.cisr_member_id);
					am_member_id = _csp.account_manager == null ? 0 : Convert.ToInt32(_csp.account_manager);

					notes_sales = _csp.notes_sales;
					notes_public = _csp.notes_public;

					previous_values = new customer_sales_properties
					{
						sector = sector,
						industry = industry,
						naics_code = naics_code,
						employee_size = employee_size,
						do_at_location = do_at_location,
						affiliated_companies = affiliated_companies,
						known_suppliers = known_suppliers,
						known_competitors = known_competitors,
						why_choose = why_choose,
						origin = origin,
						status_id = status_id,
						call_cycle = call_cycle,
						year_end = year_end,
						project_mgr_member_id = project_mgr_member_id,
						controls_mgr_member_id = controls_mgr_member_id,
						discount_pct = discount_pct,
						decision_maker = decision_maker,
						account_code = account_code,
						next_followup_date = next_followup_date,
						next_followup_notes = next_followup_notes,
						job_budget_threshold = job_budget_threshold,
						po_required = po_required,
						confirmed_po_required = confirmed_po_required,
						confirmed_tax_exempt = confirmed_tax_exempt,
						customer_id = customer_id,
						isr_member_id = isr_member_id,
						osr_member_id = osr_member_id,
						ram_member_id = ram_member_id,
						mam_member_id = mam_member_id,
						cisr_member_id = cisr_member_id,
						am_member_id = am_member_id,
						notes_sales = notes_sales,
						notes_public = notes_public
					};
				}
			}
		}
	}
	}
