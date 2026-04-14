using System;
using System.Data;
using nesi.core;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Shared.PickList.Quote
{
	public class PickListQuoteBase : PickListBase
	{
		protected nesi.core.quote _quote;
		public bool QuoteDiscount { get; set; }
		public double DiscountAmount { get; set; }
		protected DataRow quote_info;
		

		public PickListQuoteBase(Employee user) : base(user)
		{
			
		}


		public PickListQuoteBase(Employee user, int _id, int _rev) : base(user)
		{
			var dt = bllToolbox.doSQL_dt(@"SELECT *
			FROM quote_master WHERE quote_id = @v0 and revision=@v1 LIMIT 1", _id, _rev);
			if (dt.Rows.Count == 0)
			{
				throw new Exception("Quote Doesn't Exist");
			}
			quote_info = dt.Rows[0];
			buId = (int)quote_info["business_unit_id"];
			var quotediscount = quote_info["quote_master_apply_discount"].ToString();
			QuoteDiscount = quotediscount == "1";
			var c_id = Convert.ToInt32(quote_info["customer_id"]);
			var a_id = Toolbox.ReturnZeroIfNull_int(quote_info["address_id"]);
			if (a_id == 0)
			{
				//				a_id =
				//					bllToolbox.doSQL_int(@"SELECT address_id FROM address WHERE address_table = 'Customer' AND address_table_id = @v0  AND address_type = 'B'", c_id);
				a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
			}

			DiscountAmount = 0;

			WorkingBusinessUnit = new NeBusinessUnit(buId);
			WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);
			var intID = Convert.ToInt32(string.Concat(_id, _rev));
			_quote = new quote(_id);
		}
		
	}
}