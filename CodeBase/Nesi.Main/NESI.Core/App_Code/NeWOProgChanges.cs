using System.Data;

namespace nesi.core
{
	/// <summary>
	/// Summary description for SalesOrder.
	/// </summary>
	public class NeWOProgChanges
	{
		public int WOProgChanges_WasBillingType { get; set; }
		public int WoProgChanges_IsBillingType { get; set; }
		public int WoProgChanges_ID { get; set; }
		public int WoProgChanges_WOProg_ID { get; set; }
		public string WoProgChanges_BVWO { get; set; }
		public string WoProgChanges_BVWORec { get; set; }
		public string WoProgChanges_DateTime { get; set; }
		public int WoProgChanges_Modified_Member_ID { get; set; }
		public int WoProgChanges_WOProgComment_ID { get; set; }
		public string WoProgChanges_WasPartNo { get; set; }
		public string WoProgChanges_WasPrice { get; set; }
		public string WoProgChanges_WasQty { get; set; }
		public string WoProgChanges_IsPartNo { get; set; }
		public string WoProgChanges_IsPrice { get; set; }
		public string WoProgChanges_IsQty { get; set; }
		public string WoProgChanges_IsDesc { get; set; }
		public string WoProgChanges_WasDesc { get; set; }
		public string WoProgChanges_DeleteFlag { get; set; }
		public string ModifiedByFullName { get; set; }
		public string FullWOComment { get; set; }
		public int business_unit_id { get; set; }
		public string WOProgChanges_OrderedQty { get; set; }
		public int WOProgChanges_ManualPriceChange { get; set; }

		public double cost_before { get; set; }
		public double cost_ca { get; set; }
		public double cost_after { get; set; }
		public double qty_comm_before { get; set; }
		public double qty_comm_ca { get; set; }
		public double qty_comm_after { get; set; }
		public double was_req_qty { get; set; }

		public void AddtoWOProgChanges()
		{
			WoProgChanges_ID = Toolbox.doSQL_return_id(@"
INSERT INTO woprogchanges 
	(
	woprogchanges_woprog_id,
	woprogchanges_bvwo,
	woprogchanges_bvworec,
	woprogchanges_modifiedmemberid,
	woprogchanges_woprogcomment_id,
	woprogchanges_waspartno,
	woprogchanges_wasprice,
	woprogchanges_wasqty,
	woprogchanges_ispartno,
	woprogchanges_isprice,
	woprogchanges_isqty,
	woprogchanges_deleteflag,
	woprogchanges_isdescription,
	woprogchanges_wasdescription,
	woprogchanges_comments,
	business_unit_id,
	woprogchanges_wasbillingtype,
	woprogchanges_isbillingtype,
	woprogchanges_orderedqty, 
	woprogchanges_manualpricechange,
	was_req_qty,

	cost_before,
	cost_ca,
	cost_after,
	qty_comm_before,
	qty_comm_ca,
	qty_comm_after
	) 
values 	(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26)",
new object[] {
				WoProgChanges_WOProg_ID,
				WoProgChanges_BVWO,
				WoProgChanges_BVWORec,
				WoProgChanges_Modified_Member_ID,
				WoProgChanges_WOProgComment_ID,
				WoProgChanges_WasPartNo,
				WoProgChanges_WasPrice,
				WoProgChanges_WasQty,
				WoProgChanges_IsPartNo,
				WoProgChanges_IsPrice,
				WoProgChanges_IsQty,
				WoProgChanges_DeleteFlag,
				WoProgChanges_IsDesc,
				WoProgChanges_WasDesc,
				FullWOComment,
				business_unit_id,
				WOProgChanges_WasBillingType,
				WoProgChanges_IsBillingType,
				WOProgChanges_OrderedQty,
				WOProgChanges_ManualPriceChange,
				was_req_qty,

				cost_before,
				cost_ca,
				cost_after,
				qty_comm_before,
				qty_comm_ca,
				qty_comm_after
			});
			Toolbox.doSQL_void(@"CALL wo_line_changes_indiv(@v0)", WoProgChanges_ID);
		}

		public bool CheckPartHistory(string strWO, string strPartNo)
		{
			var exists = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM woprogchanges 
WHERE WoProgChanges_BVWO = @v0
AND WoProgChanges_IsPartNo = @v1 
AND WoProgChanges_IsQty = 0", new object[] { strWO, strPartNo });
			return exists == 1;
		}
	}
	public class bingo_data
	{
		public int id { get; set; }
		public int detail_id { get; set; }
		public int woprog_id { get; set; }
		public int master_id { get; set; }
		public double before_qty { get; set; }
		public double before_cost { get; set; }
		public double before_sell { get; set; }
		public double after_qty { get; set; }
		public double after_cost { get; set; }
		public double after_sell { get; set; }
		public double ca_qty { get; set; }
		public double ca_cost { get; set; }
		public double ca_sell { get; set; }
		public int woprogchanges_id { get; set; }

		public bingo_data() { }

		public bingo_data(int _id)
		{
			if (id != 0)
			{
				var dt = Toolbox.doSQL_dt(@"SELECT * FROM bingo_data  WHERE id =@v0", new object[] { _id });
				if (dt.Rows.Count > 0)
				{
					id = _id;
					var dr = dt.Rows[0];
					detail_id = Toolbox.ReturnZeroIfNull_int(dr["detail_id"]);
					woprog_id = Toolbox.ReturnZeroIfNull_int(dr["woprog_id"]);
					master_id = Toolbox.ReturnZeroIfNull_int(dr["master_id"]);
					woprogchanges_id = Toolbox.ReturnZeroIfNull_int(dr["woprogchanges_id"]);
					before_qty = Toolbox.ReturnZeroIfNull_double(dr["before_qty"]);
					before_cost = Toolbox.ReturnZeroIfNull_double(dr["before_cost"]);
					before_sell = Toolbox.ReturnZeroIfNull_double(dr["before_qty"]);
					after_qty = Toolbox.ReturnZeroIfNull_double(dr["after_qty"]);
					after_cost = Toolbox.ReturnZeroIfNull_double(dr["after_cost"]);
					after_sell = Toolbox.ReturnZeroIfNull_double(dr["after_sell"]);
					ca_qty = Toolbox.ReturnZeroIfNull_double(dr["ca_qty"]);
					ca_cost = Toolbox.ReturnZeroIfNull_double(dr["ca_cost"]);
					ca_sell = Toolbox.ReturnZeroIfNull_double(dr["ca_sell"]);
				}
			}
		}

		//TODO This is a bad query - woprogchanges shouldn't be referenced... and the query is badly formed.
		public bingo_data get_from_woprog_and_detail(int _woprog_id, int _detail_id, int _master_id)
		{
			var b = new bingo_data();
			var _id = Toolbox.doSQL_int(@"Select ifnull((select id from bingo_data 
where detail_id = @v0  
and woprog_id = @v1   
and master_id=@v2 limit 1",
new object[] {
	_detail_id,
	_woprog_id,
	_master_id
	});

			if (_id != 0)
			{
				b = new bingo_data(_id);
			}
			return b;

		}
		//TODO This is a bad query - woprogchanges shouldn't be referenced... and the query is badly formed.
		public bingo_data get_from_woprog_changes_id(int _woprog_changes)
		{
			var b = new bingo_data();
			var _id = Toolbox.doSQL_int(@"Select ifnull((select id from bingo_data where woprogchanges_id=@v0 limit 1),0)", _woprog_changes);
			if (_id != 0)
			{
				b = new bingo_data(_id);
			}
			return b;

		}

		public void save()
		{
			if (id == 0)
			{
				Toolbox.doSQL_void(@"Insert into bingo_data (
detail_id,
woprog_id,
master_id,
before_qty,
before_cost,
before_sell,
after_qty,
after_cost,
after_sell,
ca_qty,
ca_cost,
ca_sell,
woprogchanges_id) values
(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12)",
					new object[] {
detail_id,
woprog_id,
master_id,
before_qty,
before_cost,
before_sell,
after_qty,
after_cost,
after_sell,
ca_qty,
ca_cost,
ca_sell,
woprogchanges_id
});

			}
			else
			{
				Toolbox.doSQL_void(@"update bingo_data set
detail_id=@v0 ,
woprog_id=@v1 ,
masterid=@v2 ,
before_qty=@v3 ,
before_cost=@v4 ,
before_sell=@v5 ,
after_qty=@v6 ,
after_cost=@v7 ,
after_sell=@v8 ,
ca_qty=@v9 ,
ca_cost=@v10 ,
ca_sell=@v11 ,
woprogchanges_id=@v12  
where id =@v13 ",
					new object[] {
						detail_id,
						woprog_id,
						master_id,
						before_qty,
						before_cost,
						before_sell,
						after_qty,
						after_cost,
						after_sell,
						ca_qty,
						ca_cost,
						ca_sell,
						woprogchanges_id,
						id
						}
);
			}

		}
	}
}