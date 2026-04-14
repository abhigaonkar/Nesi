using System;
using System.Collections.Generic;
using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Customers;
using NESI.Common.Models;

namespace NESI.BLL.Pages.Customers
{
	public class CustomerAccountingSettings : CustomerBase
	{
		public LabelValueString[] sell_levelList { get; set; }
		public LabelValueInt[] credit_typeList { get; set; }
		public LabelValueString[] statementList { get; set; }
		public LabelValueInt[] default_invocie_typeList { get; set; }
		public CustomerAccountingSetting setting { get; set; }

        public LabelValueInt[] terms_List { get; set; }

		public bool is_allowed_to_switch_chargeout_bu { get; set; }
        public CustomerAccountingSettings(Employee user) : base(user)
		{

		}
		public CustomerAccountingSettings(Employee user, int cust_id) : base(user)
		{
			this.customer_id = cust_id;
		
		}

		public DataTable GetRatesList(int buId, int address_id = 0)
		{
			return bllToolbox.doSQL_dt(@"call customer_chargeouts_n2(@p0,@p1,@p2)", customer_id, buId, address_id);
		}

		public object GetRates(int address_id)
		{
			is_allowed_to_switch_chargeout_bu = CurrentUser.AuthenticatedForPrivilege(OpsPrivilege.ChargeoutSettingsBusinessUnitSwitch);
			var business_unit_list = bllToolbox.doSQL_Array<LabelValueInt>(@"
			SELECT a.id value, a.ddl_name label FROM business_unit a inner join tax_entity b on a.tax_entity_id = b.id  WHERE b.is_active AND a.name NOT like 'MASTER%' ORDER BY a.name
			");

            return new
			{
				business_unit_list,
				selected_business_unit_id = CurrentUser.BusinessUnitId,
				table_data = GetRatesList(CurrentUser.BusinessUnitId, address_id),
				is_allowed_to_switch_chargeout_bu

            };
		}

        public object GetOverriddeSetting(int buid)
        {
            //
            // Get the override setting.
            //
            var allow_rate_modifications = bllToolbox.doSQL_bool(@"SELECT allow_rate_modifications FROM business_unit WHERE ID = " + buid);

            return new
            {
                buid = buid,
                allow_rate_modifications = allow_rate_modifications
            };
        }

        public DataExtra SaveRate(CustomerRate model)
		{
			var paytypes = new List<string>(new[] { "RT", "OT", "DT", "RTSP", "OTSP", "DTSP" });
			var paytype_id = 1;
			var multiplier = new List<double>(new[] { 1, 1.5, 2, 1.1, 1.65, 2.2 });

			foreach (var paytype in paytypes)
			{
				var base_chargeout_id = Toolbox.doSQL_int(@"SELECT id 
FROM membertype_chargeout WHERE membertype_id = @v0 AND paytype_id = @v1 AND business_unit_id = @v2", new object[] {
					model.id, paytype_id, model.business_unit_id});
				var row_id = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(id),0) 
FROM customer_rate WHERE base_chargeout_id = @v0 AND customer_id = @v1 and address_id=@v2", new object[] { base_chargeout_id, customer_id, model.address_id });
				var charge = new customer_chargeout
				{
					id = row_id,
					chargeout = model.reg * multiplier[paytype_id - 1],
					base_chargeout_id = base_chargeout_id,
					customer_id = customer_id,
					member_id = UserId,
					business_unit_id = model.business_unit_id,
					from_date = model.from_date,
					to_date = model.to_date,
					last_updated = System.DateTime.Today,
					address_id = model.address_id
				};

                if (model.overrideflag)
                {
                    //
                    // we need to override the price by using the one from angualr side.
                    //
                    charge.chargeout = this.GetChargeoutFromAngular(paytype, model, charge.chargeout);

                }

				charge.save();
				paytype_id++;
			}
			return new DataExtra()
			{
				Data = "Rate has been saved successfully.",
				Extra = System.DateTime.Today
			};
		}
		public DataExtra DeleteRate(CustomerRate model)
		{
			var paytypes = new List<string>(new[] { "RT", "OT", "DT", "RTSP", "OTSP", "DTSP" });
			var paytype_id = 1;

			foreach (var paytype in paytypes)
			{
				var base_chargeout_id = Toolbox.doSQL_int(@"SELECT id 
FROM membertype_chargeout WHERE membertype_id = @v0 AND paytype_id = @v1 AND business_unit_id = @v2", new object[] {
					model.id, paytype_id, model.business_unit_id});
				Toolbox.doSQL_void(@"delete FROM customer_rate WHERE base_chargeout_id = @v0 AND customer_id = @v1 and address_id=@v2", new object[] { base_chargeout_id, customer_id, model.address_id });
				paytype_id++;
			}
			return new DataExtra()
			{
				Data = "Rate has been reseted successfully.",
				Extra = GetRatesList(CurrentUser.BusinessUnitId, model.address_id)
			};
		}

		public CustomerAccountingARNotes ARNotes()
		{
			var c = new NECustomer(customer_id);

			return new CustomerAccountingARNotes
			{
				customer_id = customer_id,
				customer_arnotes = c.customer_arnotes,
				memo = c.Memo
			};
		}

		public string SaveARNotes(CustomerAccountingARNotes model)
		{
			var c = new NECustomer(model.customer_id)
			{
				customer_arnotes = model.customer_arnotes,
				Memo = model.memo
			};
			c.Save();
			return "Customer notes saved successfully.";
		}


		public string SaveSetting(CustomerAccountingSetting model)
		{
			var c = new NECustomer(model.customer_id)
			{
				Member_ID = CurrentUser.Id,
				Address = { SellPrice = model.sell_level },
				TaxPrompt = model.tax_prompt ? "T" : "F",
				Credit_Type = model.credit_type,
				CreditLimit = model.credit_limit,
				customer_creditdays = model.customer_creditdays,
                customer_term_id=model.customer_term_id,
                StatementType = model.statement_type,
				InvoiceType = model.invoice_type,
				ApplyFinanceCharges = model.apply_finance_charges ? "T" : "F",
				customer_autostatements = model.customer_autostatements ? 1 : 0,
				customer_autostatement_address = model.customer_autostatement_address,
				customer_autostatement_ccaddress = model.customer_autostatement_ccaddress,
				customer_invoice_address = model.customer_invoice_address,
				customer_invoice_ccaddress = model.customer_invoice_ccaddress,
				default_invoicetype = model.default_invoicetype.GetValueOrDefault(0),
				customer_auto_invoice = model.customer_auto_invoice ? 1 : 0,
				requires_wo_copy = model.requires_wo_copy
			};
			c.Save();
			c.Address.Save();

			return "Customer setting saved successfully.";
		}
		public new CustomerAccountingSettings Profile()
		{
			sell_levelList = GetSell_LevelList();
			credit_typeList = GetCreditTypeList();
			statementList = GetStatementList();
			default_invocie_typeList = GetDefaultIvoiceTypeList();
			setting = GetSetting();
            terms_List = GetTermsList();
			
            return this;
		}

		public CustomerAccountingSetting GetSetting()
		{
			var o = new CustomerAccountingSetting();
			if (customer_id > 0)
			{
				var c = new NECustomer(customer_id);
				o.sell_level = c.Address.SellPrice == null ? "" : c.Address.SellPrice.PadLeft(2, '0');
                o.customer_id = customer_id;
				o.credit_type = c.Credit_Type;
				o.tax_prompt = c.TaxPrompt == "T";
				o.credit_limit = c.CreditLimit;
				o.customer_creditdays = c.customer_creditdays;
                o.customer_term_id = c.customer_term_id;
				o.statement_type = c.StatementType;
				o.invoice_type = c.InvoiceType;
				o.apply_finance_charges = c.ApplyFinanceCharges == "T";
				o.customer_auto_invoice = c.customer_auto_invoice == 1;
				o.customer_autostatements = c.customer_autostatements == 1;
				o.customer_autostatement_address = c.customer_autostatement_address;
				o.customer_autostatement_ccaddress = c.customer_autostatement_ccaddress;
				o.customer_invoice_address = c.customer_invoice_address;
				o.customer_invoice_ccaddress = c.customer_invoice_ccaddress;
				o.default_invoicetype = c.default_invoicetype;
				o.requires_wo_copy = c.requires_wo_copy;
				o.overallmargin = !CurrentUser.AuthenticatedForPrivilege(81) ? "--" : bllToolbox.doSQL_double(@"SELECT customer_margin(@v0 )", customer_id).ToString("P2");
			}
			else
			{
				o.customer_id = -1;
				o.sell_level = "01";
				o.credit_type = 0;
				o.statement_type = "F";
				o.invoice_type = "F";
				o.default_invoicetype = 0;
				o.customer_creditdays = 30;
			}


			return o;
		}

		public LabelValueInt[] GetDefaultIvoiceTypeList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(@"Select id value,invoice_type label from customer_default_invoice_types");
		}

		public LabelValueString[] GetStatementList()
		{
			return new LabelValueString[]
			{
				new LabelValueString {Label = "Email",Value="E"},
				new LabelValueString {Label = "Print Form",Value="F"},
				new LabelValueString {Label = "Print Form and Email",Value="B"},
				new LabelValueString {Label = "Not Required",Value="N"}
			};
		}

		public LabelValueInt[] GetCreditTypeList()
		{
			return new LabelValueInt[]
			{
				new LabelValueInt {Label = "Unlimited",Value=1},
				new LabelValueInt {Label = "No Credit",Value=0},
				new LabelValueInt {Label = "Limit",Value=2},
				new LabelValueInt {Label = "Credit Card Only",Value=3}
			};
		}

		public LabelValueString[] GetSell_LevelList()
		{
			var list = new List<LabelValueString>();
			for (var i = 0; i < 20; i++)
			{
				var o = i.ToString().PadLeft(2, '0');
				list.Add(new LabelValueString { Label = o, Value = o });
			}
			return list.ToArray();
		}

        public  LabelValueInt[] GetTermsList()
        {
            var o = bllToolbox.doSQL_Array<LabelValueInt>(@"Select Term_ID value,  CAST(CONCAT(term_code,' - ', term_desc) AS CHAR) label from term where Active=1");
            return o;
          
        }

        private double GetChargeoutFromAngular(string paytype, CustomerRate model, double chargeout)
        {
            if (paytype.ToUpper() == "RT")
            {
                return model.reg;
            }

            if (paytype.ToUpper() == "OT")
            {
                return model.ot;
            }

            if (paytype.ToUpper() == "DT")
            {
                return model.dt;
            }

            if (paytype.ToUpper() == "RTSP")
            {
                return model.regsp;
            }

            if (paytype.ToUpper() == "OTSP")
            {
                return model.otsp;
            }

            if (paytype.ToUpper() == "DTSP")
            {
                return model.dtsp;
            }

            return chargeout;
        }

	}
}