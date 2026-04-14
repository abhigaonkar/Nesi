using System;
using System.Collections.Generic;
using System.Web;
using MySql.Data.MySqlClient;
using NESI.BLL.Base;
using NESI.DTO.Models.Core;

namespace NESI.BLL.Core
{
	public class Ne2Expense : BLLBase
	{
		public double amount { get; set; }
		public int approved { get; set; }
		public int customer_id { get; set; }
		public string customer_name { get; set; }
		public DateTime date_purchased { get; set; }
		public DateTime date_requested { get; set; }
		public DateTime date_start { get; set; }
		public DateTime date_end { get; set; }
		public int id_expense { get; set; }
		public int id_member { get; set; }
		public int id_payperiod { get; set; }
		public int id_seller { get; set; }
		public string name_seller { get; set; }
		public string item_text { get; set; }
		public string category { get; set; }
		public string receipt_number { get; set; }
		public int woprog_id { get; set; }
		public int master_id { get; set; }
		public int approved_by { get; set; }
		public bool has_file { get; set; }
		public double distance { get; set; }
		public string unit_distance { get; set; }
		public string attendees { get; set; }
		public string file_ext { get; set; }
		public string file_mime { get; set; }
		public string currency { get; set; }

		public Ne2Expense()
		{
		}

		public Ne2Expense(int _id)
		{
			id_expense = _id;
			if (exists(_id))
			{
				load();
			}
			else
			{
				throw new Exception("Expense ID doesn't exists");
			}
		}

		public int new_seller(string _name, int _member_id)
		{
			return bllToolbox.doSQL_return_id(
				@"INSERT INTO expense_seller (name_seller, added_by, added_dt) VALUES (@v0, @v1, NOW())", _name, _member_id);
		}

		private void load()
		{
			var e_row = bllToolbox
				.doSQL_dt(
					@" SELECT a.id_member, IFNULL(a.approved, 0) approved, a.receipt_number, a.date_requested, a.date_purchased, a.date_start, a.date_end, a.id_payperiod, a.id_seller, (SELECT b.name_seller FROM expense_seller b WHERE b.id_seller = a.id_seller) name_seller, URLDECODE(b.description) category, a.customer_id, c.customer_name, a.woprog_id, a.item_text, a.master_id, a.amount, a.has_file, a.distance, a.unit_distance, IFNULL(a.attendees, '') attendees, IFNULL(a.file_ext, '') file_ext, IFNULL(a.file_mime, '') file_mime, IFNULL(a.currency, '') currency, a.approved_by FROM expense_reimbursement a LEFT JOIN inventory_description b ON a.master_id = b.master_id LEFT JOIN customer c ON a.customer_id = c.customer_id WHERE id_expense = @v0  LIMIT 1",
					id_expense).Rows[0];
			//id_member = (int) e_row["id_member"];
			id_member = int.Parse(e_row["id_member"].ToString());
			if (e_row["approved"] != DBNull.Value)
			{
				approved = Convert.ToInt32(e_row["approved"]);
			}
			receipt_number = e_row["receipt_number"].ToString();
			date_requested = Convert.ToDateTime(e_row["date_requested"]);
			date_purchased = Convert.ToDateTime(e_row["date_purchased"]);
			date_start = e_row["date_start"] == DBNull.Value ? date_purchased : Convert.ToDateTime(e_row["date_start"]);
			date_end = e_row["date_end"] == DBNull.Value ? date_purchased : Convert.ToDateTime(e_row["date_end"]);
			id_payperiod = Convert.ToInt32(e_row["id_payperiod"]);
			id_seller = Convert.ToInt32(e_row["id_seller"]);
			name_seller = e_row["name_seller"].ToString();
			customer_id = Convert.ToInt32(e_row["customer_id"]);
			customer_name = e_row["customer_name"].ToString();
			woprog_id = Convert.ToInt32(e_row["woprog_id"]);
			master_id = Convert.ToInt32(e_row["master_id"]);
			category = e_row["category"].ToString();
			item_text = e_row["item_text"].ToString();
			amount = Convert.ToDouble(e_row["amount"]);
			has_file = Convert.ToBoolean(e_row["has_file"]);
			distance = Convert.ToDouble(e_row["distance"]);
			unit_distance = e_row["unit_distance"].ToString();
			attendees = e_row["attendees"].ToString();
			file_ext = e_row["file_ext"].ToString();
			file_mime = e_row["file_mime"].ToString();
			currency = e_row["currency"].ToString();
			approved_by = Convert.ToInt32(e_row["approved_by"]);
		}

		public bool exists(int _id)
		{
			return bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM expense_reimbursement WHERE id_expense = @v0", _id) > 0;
		}

		public void delete()
		{
			// Delete from WO
			// Send email
			if (exists(id_expense) && approved == -1)
			{
				bllToolbox.doSQL_void(@"DELETE FROM expense_reimbursement WHERE id_expense = @v0 LIMIT 1", id_expense);
			}
		}

		public void new_perdiem()
		{

		}

		public void save()
		{
			var is_new = id_expense == 0;
			var sql = "";
			if (!is_new)
			{
				#region edit

				sql = @"
UPDATE 
	expense_reimbursement 
SET 
	id_member = @v0, 
	approved = @v1,
	receipt_number = @v2,
	date_purchased = @v3,
	date_start = @v12,
	date_end = @v13,
	id_payperiod = @v4,
	id_seller = @v5,
	customer_id = @v6,
	woprog_id = @v7,
	item_text = @v9,
	amount = @v10,
	master_id = @v11,
	has_file = @v14,
	distance = @v15,
	unit_distance = @v16,
	attendees = @v17,
	file_ext = @v18,
	file_mime = @v19,
	currency = @v20,
	approved_by = @v21
WHERE
	id_expense = @v8
LIMIT 1
	";
				var paramObjects = new object[]
				{
					id_member, // {0}
					approved, // {1}
					receipt_number, // {2}
					bllToolbox.MySQL_shortdt(date_purchased), // {3}
					id_payperiod, // {4}
					id_seller, // {5}
					customer_id, // {6}
					woprog_id, // {7}
					id_expense, // {8}
					item_text, // {9}
					amount, // {10}
					master_id, // {11}
					bllToolbox.MySQL_shortdt(date_start), // {12}
					bllToolbox.MySQL_shortdt(date_end), // {13}	
					has_file, // {14}
					distance, // {15}
					unit_distance, // {16}
					attendees, // {17}
					file_ext, // {18}
					file_mime, // {19}
					currency, // {20}
					approved_by // {21}
				};
				bllToolbox.doSQL_void(sql, paramObjects);

				#endregion
			}
			else
			{
				#region insert

				sql = @"
INSERT INTO expense_reimbursement
	(
	id_member, 
	approved,
	receipt_number,
	date_requested,
	date_purchased,
	id_payperiod,
	id_seller,
	customer_id,
	woprog_id,
	item_text,
	amount,
	master_id,
	date_start,
	date_end,
	has_file,
	distance,
	unit_distance,
	attendees,
	file_ext,
	file_mime,
	currency,
	approved_by
	)
VALUES
	(
	@v0, 
	-1,
	@v1,
	NOW(),
	@v2,
	@v3,
	@v4,
	@v5,
	@v6,
	@v7,
	@v8,
	@v9,
	@v10,
	@v11,
	@v12,
	@v13,
	@v14,
	@v15,
	@v16,
	@v17,
	@v18,
	@v19
	)";
				var paramObjects = new object[]
				{
					id_member, // {0}
					receipt_number, // {1}
					bllToolbox.MySQL_shortdt(date_purchased), // {2}
					id_payperiod, // {3}
					id_seller, // {4}
					customer_id, // {5}
					woprog_id, // {6}
					item_text, // {7}
					amount, // {8}
					master_id, // {9}
					bllToolbox.MySQL_shortdt(date_start), // {10}
					bllToolbox.MySQL_shortdt(date_end), // {11}
					has_file, // {12}
					distance, // {13}
					unit_distance, // {14}
					attendees, // {15}
					file_ext, // {16}
					file_mime, // {17}
					currency, // {18}
					approved_by // {20}
				};
				id_expense = bllToolbox.doSQL_return_id(sql, paramObjects);

				#endregion
			}
			load();
		}


	}

}