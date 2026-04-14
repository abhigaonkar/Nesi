using System;
using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NEContact
	/// </summary>
	public class NECredit_card_purchase
		{
		public int id { get; set; }
		public int member_id { get; set; }
		public DateTime date_requested { get; set; }
		public DateTime date_purchased { get; set; }
		public bool approved { get; set; }
		public int seller_id { get; set; }
		public string currency { get; set; }
		public double amount { get; set; }
        public double total { get; set; }
		public int master_id { get; set; }
		public string status { get; set; }
		public string file_ext { get; set; }
		public string file_mime { get; set; }
		public int approved_by { get; set; }
		public int business_unit_id { get; set; }
		public int credit_card_id { get; set; }
        public int origin_id { get; set; }
		public string receipt_number { get; set; }
		public string item_text { get; set; }
		public int woprog_id { get; set; }
		public int customer_id { get; set; }
        public bool has_file { get; set; }
		public DateTime? approved_date { get; set; }
		public DateTime? paid_date { get; set; }
        public int expense_category_id { get; set; }



        public NECredit_card_purchase(int _id)
			{
			var dt = Toolbox.doSQL_dt(@"SELECT * FROM Credit_card_purchase WHERE ID = @v0 ", new object[] {  _id } );
			foreach (DataRow dr in dt.Rows)
				{
				id					= _id;
				member_id			= Convert.ToInt32(dr["member_id"]);
				date_requested		= Convert.ToDateTime(dr["date_requested"]);
				date_purchased		= Convert.ToDateTime(dr["date_purchased"]);
				approved			= Convert.ToBoolean(dr["approved"]);
				seller_id			= Convert.ToInt32(dr["seller_id"]);
				business_unit_id			= Convert.ToInt32(dr["business_unit_id"]);
				currency			= Convert.ToString(dr["currency"]);
				amount				= Convert.ToDouble(dr["amount"]);
                total               = Convert.ToDouble(dr["total"]);
				woprog_id			= dr["woprog_id"] != DBNull.Value ? Convert.ToInt32(dr["woprog_id"]) : 0;
				customer_id			= dr["customer_id"] != DBNull.Value ? Convert.ToInt32(dr["customer_id"]) : 0;
				master_id			= dr["master_id"] != DBNull.Value ? Convert.ToInt32(dr["master_id"]) : 0;
                expense_category_id = dr["expense_category_id"] != DBNull.Value ? Convert.ToInt32(dr["expense_category_id"]) : 0;
                has_file			= dr["has_file"] != DBNull.Value && Convert.ToBoolean(dr["has_file"]);
				file_ext			= dr["file_ext"] != DBNull.Value ? Convert.ToString(dr["file_ext"]) : "";
				file_mime			= dr["file_mime"] != DBNull.Value ? Convert.ToString(dr["file_mime"]) : "";
				approved_by			= dr["approved_by"] != DBNull.Value ? Convert.ToInt32(dr["approved_by"]) : 0;
				credit_card_id		= dr["credit_card_id"] != DBNull.Value ? Convert.ToInt32(dr["credit_card_id"]) : 0;
                origin_id           = Toolbox.ReturnZeroIfNull_int(dr["origin_id"]);
                receipt_number		= dr["receipt_number"] != DBNull.Value ? Convert.ToString(dr["receipt_number"]) : "";
				item_text			= dr["item_text"] != DBNull.Value ? Convert.ToString(dr["item_text"]) : "";
				status				= Toolbox.ReturnBlankIfNull_string(dr["status"]);
				if (dr["paid_date"] != DBNull.Value && dr["paid_date"].ToString() != "")
					{
					paid_date = Convert.ToDateTime(dr["paid_date"]);
					}
				if (dr["approved_date"] != DBNull.Value && dr["approved_date"].ToString() != "")
					{
					approved_date = Convert.ToDateTime(dr["approved_date"]);
					}
				}
			}


	 public static int indexCCpurchaseinWODC(int _id)
		{
			if (_id == 0) return 0;
			else
			{
				var result= Toolbox.doSQL_int(@"
SELECT 
	wo_detail_current_id 
FROM 
	wo_detail_current 
WHERE
	wo_detail_current_consignment_id = @v0 AND
	wo_detail_current_origin = 'Company Credit Card Expense'
	", new object[] { _id });
				return result;
			}
		}

		public void save()
			{

			if (id!=0)
				{
				Toolbox.doSQL_void(@"
UPDATE 
	credit_card_purchase 
SET 
	member_id = @v0,
	date_requested = @v1,
	date_purchased = @v2,
	approved = @v3,
	seller_id = @v4,
	currency = @v5,
	amount = @v6,
	woprog_id = @v7,
	customer_id = @v8,
	master_id = @v9,
	has_file = @v10,
	file_ext = @v11,
	file_mime = @v12,
	approved_by = @v13,
	credit_card_id = @v14,
	receipt_number = @v16 ,
	item_text = @v17,
	woprog_id = @v18,
	customer_id = @v19,
	approved_date = @v20,
	paid_date = @v21,
	business_unit_id = @v22,
	status = @v23,
    expense_category_id=@v24,
    origin_id=@v25,
    total=@v26
WHERE
	id=@v15
", new object[] {
					member_id ,
					Toolbox.MySQL_longdt(date_requested),
					Toolbox.MySQL_longdt(date_purchased),
					approved,
					seller_id,
					currency,
					amount,
					woprog_id,
					customer_id,
					master_id,
					has_file,
					file_ext,
					file_mime,
					approved_by,
					credit_card_id,
					id,
					receipt_number,
					item_text,
					woprog_id,
					customer_id,
					approved_date !=null ? Toolbox.MySQL_longdt((DateTime) approved_date) : "NULL",
					paid_date !=null ? Toolbox.MySQL_longdt((DateTime) paid_date) : "NULL",
					business_unit_id,
					status,
                    expense_category_id,
                    origin_id,
                    total
                });
				}
			else
				{
				id = Toolbox.doSQL_return_id(@"
INSERT INTO credit_card_purchase 
	(
	member_id,
	date_requested,
	date_purchased,
	approved,
	seller_id,
	currency,
	amount,
	woprog_id,
	customer_id,
	master_id,
	has_file,
	file_ext,
	file_mime,
	approved_by,
	credit_card_id,
	receipt_number,
	item_text,
	approved_date,
	paid_date,
	business_unit_id,
	status,
    expense_category_id,
    origin_id,
    total
	) 
VALUES 
	(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23)",
	new object[] {
					member_id ,
					Toolbox.MySQL_longdt(date_requested),
					Toolbox.MySQL_longdt(date_purchased),
					approved,
					seller_id,
					currency,
					amount,
					woprog_id,
					customer_id,
					master_id,
					has_file,
					file_ext,
					file_mime,
					approved_by,
					credit_card_id,
					receipt_number,
					item_text,
					approved_date!=null ? Toolbox.MySQL_longdt((DateTime) approved_date) : "NULL",
					paid_date!=null? Toolbox.MySQL_longdt((DateTime) paid_date) :"NULL",
					business_unit_id,
					status,
                    expense_category_id,
                    origin_id,
                    total
                });
				}
			}

		public int new_seller(string name, int member_id)
			{
			return Toolbox.doSQL_return_id(@"INSERT INTO expense_seller (name_seller, added_by, added_dt)
VALUES (@v0,@v1, NOW())",new object[] { name, member_id});
			}

		public bool exists(int _id)
			{
			return Toolbox.doSQL_int("SELECT COUNT(*) FROM Credit_card_purchase WHERE id = @v0", _id) > 0;
			}
		public void delete()
			{
			if (exists(id) && approved == false)
				{
				Toolbox.doSQL_void(@"DELETE FROM Credit_card_purchase WHERE id = @v0 LIMIT 1", id);
				}
			}
		public static bool HasUnintegratedTransactions(int member_id)
		{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM credit_card_purchase a INNER JOIN woprog b ON a.woprog_id = b.woprog_id where a.member_id = @v0 AND a.netsuite_internal_id IS NULL and b.netsuite_salesorder_lineUniqueKey IS NOT NULL and woprog_status != 'Invoiced' ",
									new object[] { member_id }) > 0;
			
		}

		public NECredit_card_purchase()
			{
			//
			//  
			//
			}
		}
	}