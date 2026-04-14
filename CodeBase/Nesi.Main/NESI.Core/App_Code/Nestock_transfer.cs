using System;
using System.Data;
using System.Web;
using NESI.Common.Models;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for stock_transfer_history
	/// </summary>
	public class Nestock_transfer
		{
		#region privates
		private int _stock_transfer_id;
		private int _stock_transfer_from_id = 0;
		private int _stock_transfer_to_id = 0;
		private int _stock_transfer_type;
		private int _stock_transfer_master_id;
		private double _stock_transfer_quantity;
		private double _stock_transfer_cost;
		private string _stock_transfer_description = "";
		private string _stock_transfer_note = "";
		private DateTime _stock_transfer_date_added = DateTime.Now;
		private int _polineid = 0;
		private int _stock_transfer_log_section_id = 0;
		private void UpdatePartPricing()
			{
			var inv					= new inventory();
			var poprog_id					= Toolbox.doSQL_int(@"SELECT IFNULL(MAX(po_details_poprog_id),0) 
FROM po_details_current WHERE po_details_id =@v0", po_line_id);
			if(poprog_id > 0)
				{
				var poprog					= new NePOProg(poprog_id);
				var vendor					= new NEVendor();
				var vprow			= new vendor_price_row();
				var Details				= Toolbox.doSQL_dt(@" SELECT po_details_id, po_details_part_no, URLDECODE(po_details_vendor_part_no) AS po_details_vendor_part_no, URLDECODE(po_details_description) AS po_details_description, po_details_woprog_id, po_details_qty_ordered, po_details_qty_received, po_details_cost, po_details_tax1, po_details_tax2, po_details_tax3, po_details_tax4, po_details_date_expected, URLDECODE(po_details_notes) po_details_notes, po_details_vendor_qty_per,is_gl_account FROM po_details_current  WHERE po_details_id =@v0", new object[] { po_line_id });
				foreach (DataRow row in Details.Rows)
					{
					var this_part_no			= row["po_details_part_no"].ToString();
					var this_cost			= Convert.ToDouble(row["po_details_cost"]);
					var this_qty_per			= Convert.ToDouble(row["po_details_vendor_qty_per"]);
					var this_vendor_code		= row["po_details_vendor_part_no"].ToString();
					    var is_gl_account = Convert.ToBoolean(row["is_gl_account"]);
                        /**
                         * Ricky Patel 31/05/2017
                         * Retrived warehouse business unit ID for inventory
                         */
					    var WorkingBusinessUnit = new NeBusinessUnit(poprog.business_unit_id);
					    var WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);

                    #region Setting the Vendor And Branching Pricing
                    #region save vendor row in inventory
                    vprow						= new vendor_price_row(this_part_no, WarehouseBusinessUnit.id, poprog.poprog_vendor_id, this_vendor_code);
					vprow.master_id				= stock_transfer_master_id;
					vprow.member_id				= member_id;
					vprow.vendor_code			= this_vendor_code;
					//vprow.business_unit_id			= poprog.business_unit_id;
					vprow.business_unit_id			= WarehouseBusinessUnit.id;
					vprow.cost					= this_cost / this_qty_per;
					vprow.total					= this_cost;
					vprow.qty					= this_qty_per;
					vprow.vendor_id				= poprog.poprog_vendor_id;
					vprow.origin				= "PO: " + poprog.poprog_bvpo;
					try
						{
						vprow.save();
						}
					catch (Exception exvp)
						{
						Toolbox.do_errorLog_errorStack(exvp);
						}
					#endregion save vendor row in inventory
					#endregion
					}
				}
			}
		private double dollar_balance { get; set; }
		private double qty_balance { get; set; }
		#endregion privates
		#region publics
		#region logical/readable accessors
		public int id { get { return _stock_transfer_id; } set { _stock_transfer_id = value; } }
		public int from_id { get { return _stock_transfer_from_id; } set { _stock_transfer_from_id = value; } }
		public int to_id { get { return _stock_transfer_to_id; } set { _stock_transfer_to_id = value; } }
		public int type_id { get { return _stock_transfer_type; } set { _stock_transfer_type = value; } }
		public int master_id { get { return _stock_transfer_master_id; } set { _stock_transfer_master_id = value; } }
		public int business_unit_id { get; set;  }
		public int member_id { get; set; }
		public double quantity { get { return _stock_transfer_quantity; } set { _stock_transfer_quantity = value; } }
		public double cost { get { return _stock_transfer_cost; } set { _stock_transfer_cost = value; } }
		public string description { get { return _stock_transfer_description; } set { _stock_transfer_description = value; } }
		public string note { get { return _stock_transfer_note; } set { _stock_transfer_note = value; } }
		public int from_location_id { get; set; }
		public int to_location_id { get; set; }
		#endregion logical/readable accessors
		public int stock_transfer_id
			{
			get
				{
				return _stock_transfer_id;
				}
			set
				{
				if (_stock_transfer_id == value)
					return;
				_stock_transfer_id = value;
				}
			}
		public int stock_transfer_from_id
			{
			get
				{
				return _stock_transfer_from_id;
				}
			set
				{
				if (_stock_transfer_from_id == value)
					return;
				_stock_transfer_from_id = value;
				}
			}
		public int stock_transfer_to_id
			{
			get
				{
				return _stock_transfer_to_id;
				}
			set
				{
				if (_stock_transfer_to_id == value)
					return;
				_stock_transfer_to_id = value;
				}
			}


		/// <summary>
		/// section_if of log section to help distinguish where the action derived from
		/// </summary>
		public int stock_transfer_log_section_id
			{
			get
				{
				return _stock_transfer_log_section_id;
				}
			set
				{
				if (_stock_transfer_log_section_id == value)
					return;
				_stock_transfer_log_section_id = value;
				}

			}

		/// <summary>
		/// Integer based ID which pulls from the table stock_transfer_type
		/// <para>As of August 22, 2011:</para>
		/// <para>Type 1 = To Work Orders</para>
		/// <para>Type 2 = To Purchase Orders</para>
		/// <para>Type 3 = To Internal Location</para>
		/// <para>Type 4 = To External Location</para>
		/// </summary>
		public int stock_transfer_type
			{
			get
				{
				return _stock_transfer_type;
				}
			set
				{
				if (_stock_transfer_type == value)
					return;
				_stock_transfer_type = value;
				}
			}
		public int stock_transfer_member_id
			{
			get; set;
			}
		public int stock_transfer_business_unit_id
			{
			get; set;
			}
		public int stock_transfer_master_id
			{
			get
				{
				return _stock_transfer_master_id;
				}
			set
				{
				if (_stock_transfer_master_id == value)
					return;
				_stock_transfer_master_id = value;
				}
			}
		public double stock_transfer_quantity
			{
			get
				{
				return _stock_transfer_quantity;
				}
			set
				{
				if (_stock_transfer_quantity == value)
					return;
				_stock_transfer_quantity = value;
				}
			}
		public double stock_transfer_cost
			{
			get
				{
				return _stock_transfer_cost;
				}
			set
				{
				if (_stock_transfer_cost == value)
					return;
				_stock_transfer_cost = value;
				}
			}
		public string stock_transfer_description
			{
			get
				{
				return _stock_transfer_description;
				}
			set
				{
				if (_stock_transfer_description == value)
					return;
				_stock_transfer_description = value;
				}
			}
		public string stock_transfer_note
			{
			get
				{
				return _stock_transfer_note;
				}
			set
				{
				if (_stock_transfer_note == value)
					return;
				_stock_transfer_note = value;
				}
			}
		public DateTime stock_transfer_date_added
			{
			get
				{
				return _stock_transfer_date_added;
				}
			set
				{
				if (_stock_transfer_date_added == value)
					return;
				_stock_transfer_date_added = value;
				}
			}
		public int po_line_id
			{
			get
				{
				return _polineid;
				}
			set
				{
				_polineid = value;
				}
			}

		public void Nestock_transfer_save()
			{
			try
				{
				var bu			= new NeBusinessUnit(business_unit_id);
				var inv			= new inventory();
				var br			= new branch(master_id, bu.warehouse_bu_id);
				if (inv.part_exists(master_id) && master_id != 0)
					{
					#region For Internal Quantity / Dollar Balance Tracking
					inv.Load(master_id, bu.warehouse_bu_id);
					if(inv.is_exclude && inv.id != OpsSpecialPart.MiscMaterial || inv.Tag.id == OpsSpecialTag.GL)
						{
						return;
						}
					var prev_qty				= inv.onhand_qty;
					var prev_balance			= inv.dollar_balance;
					var abs_qty_incoming		= Math.Abs(quantity);
					if (prev_balance == 0 && prev_qty != 0)
					{
						prev_balance = Toolbox.doSQL_double(@"SELECT GET_CURRENT_COST(@v0 , @v1 )", new object[] {  master_id, business_unit_id } ) * prev_qty;
						}
					var default_location_id = 0;
					if (type_id != 6 && type_id != 7)
						{
						qty_balance = prev_qty + quantity;
						dollar_balance = prev_balance + cost * quantity;
						var location_count = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM (SELECT a.id, a.qty FROM inventory_location a LEFT JOIN 
inventory_location_master b ON a.location_master_id = b.id WHERE 
a.master_id = @v0 AND a.business_unit_id = @v1 AND b.type_id = 1 ORDER BY a.qty DESC) subs order by qty DESC",
							new object[] { master_id, business_unit_id});
						if (location_count > 0)
							{
							default_location_id = Toolbox.doSQL_int(@"SELECT id FROM 
(SELECT a.id, a.qty FROM inventory_location a LEFT JOIN inventory_location_master b 
ON a.location_master_id = b.id 
WHERE a.master_id = @v0 AND a.business_unit_id = @v1 AND b.type_id = 1 ORDER BY a.qty DESC) subs order by qty DESC LIMIT 1",
								new object[] { master_id, business_unit_id});
							}
						if (default_location_id == 0)
							{
							var temp_il = to_location_id != 0 ? new location(to_location_id, business_unit_id, master_id) : new location();
							to_location_id = to_location_id != 0 ? to_location_id : temp_il.id != null && temp_il.id != 0 ? (int)temp_il.id : 0;
							temp_il = from_location_id != 0 ? new location(from_location_id, business_unit_id, master_id) : new location();
							from_location_id = from_location_id != 0 ? from_location_id : temp_il.id != null && temp_il.id != 0 ? (int)temp_il.id : 0;
							}
						}
					else
						{
						qty_balance = prev_qty;
						dollar_balance = prev_balance;

						}
					var t						= 0;
					#endregion For Internal Quantity / Dollar Balance Tracking
					#region Internal/External Stock Update
					switch(type_id)
						{
							case 1:	// Coming from inventory to a work order
								// The direction of movement is determined by if the quantity is positive or not.
								// Positive: Will be a removal from inventory (3)
								// Negative: Will be an addition to inventory (2)
								if(from_location_id == 0 && to_id != 0) // to_id not being zero indicates its going from stock to the work order.. so we need to supply a default highest location for that part/branch for the FROM location
									{
									from_location_id		= default_location_id;
									from_id					= from_location_id;
									}
								var il_wo			= new location(from_location_id, business_unit_id, master_id);
								il_wo.qty					+= quantity;
								il_wo.section_id = _stock_transfer_log_section_id!=0?stock_transfer_log_section_id:2;
								il_wo.member_id				= member_id;
								il_wo.log_is_manual			= false;
								il_wo.save();
								if(to_location_id == 0 && to_id == 0) // to_id being zero indicates its going from the work order back to stock.. so we need to supply a default highest location for that part/branch for the TO location
									{
									to_location_id			= default_location_id;
									to_id					= to_location_id;
									type_id					= 3;
									}
								t							= quantity > 0
									? 2 
									: 3;
								Toolbox.do_debug_note(string.Format("il_wo.update_branch({0},{1}, {2}, {3}, {4});{5}", master_id, t, prev_balance, abs_qty_incoming, cost, type_id));
								if(il_wo.id != 0)
									{
									il_wo.update_branch(t, prev_balance, abs_qty_incoming, cost, br);
									}
								break;
							case 2:
							case 5:
								// mass receiving po
								// type_id: 2 - PO to Inventory
								// type_id: 5 - Inventory to PO (aka ether); an unreceive 
						
								if(to_location_id == 0 && to_id == 0)
									{
									to_location_id			= default_location_id;
									to_id					= to_location_id;
									}
								var il_po			= type_id == 5 
									? new location(from_location_id, business_unit_id, master_id) 
									: new location(to_location_id, business_unit_id, master_id);
								il_po.qty					+= quantity;
								il_po.section_id			= _stock_transfer_log_section_id!=0?stock_transfer_log_section_id:2;
								il_po.member_id				= member_id;
								il_po.log_is_manual			= false;
								il_po.save();
								t							= type_id == 2
									? 2 
									: 3;

								Toolbox.do_debug_note(string.Format("il_wo.update_branch({0}, {1}, {2}, {3});{4}", t, prev_balance, quantity, cost, type_id));
								if(il_po.id != 0 && il_po.master_id != 0)
									{
									il_po.update_branch(t, prev_balance, Math.Abs(quantity), cost, br);
									}
								break;
							case 3:
							case 4:	// If this is going to a internal (3) or external location (4) // only used for popup transfer on picklist
								var il_to			= new location(to_location_id, business_unit_id, master_id);
								il_to.log_is_manual			= true;
								il_to.member_id				= member_id;
								il_to.section_id			= HttpContext.Current == null ? 8 : HttpContext.Current.Request.Url.AbsoluteUri.Contains("mobile") ? 12 : 8;
								il_to.qty					+= quantity;
								il_to.save();
								Toolbox.do_debug_note(string.Format("il_wo.update_branch({0}, {1}, {2}, {3});{4}", t, prev_balance, quantity, cost, type_id));
								if(il_to.id != 0)
									{
									il_to.update_branch(2, prev_balance, quantity, cost, br);
									}
								break;
							case 6:
							case 7:

								break;
						}
					#endregion Internal/External Stock Update
					save();
					if(po_line_id != 0)
						{
						UpdatePartPricing();
						}
					}
				else if(inv.is_exclude)
					{

					}
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				}
			}
		public void save()
			{
			#region insert 
			if (stock_transfer_id == 0)
				{
				Toolbox.doSQL_void(@"
INSERT INTO stock_transfer
	(
	stock_transfer_from_id,
	stock_transfer_to_id,
	stock_transfer_type,
	stock_transfer_master_id,
	business_unit_id,
	stock_transfer_member_id,
	stock_transfer_quantity,
	stock_transfer_cost,
	stock_transfer_description,
	stock_transfer_note,
	stock_transfer_date_added,
	stock_transfer_dollar_balance,
	stock_transfer_qty_balance
	)
VALUES
	(
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5,
	@v6,
	@v7,
	@v8,
	@v9,
	NOW(),
	@v10,
	@v11
	)", new object[] {
					from_id,							// {0}
					to_id,								// {1}
					type_id,							// {2}
					master_id,							// {3}
					business_unit_id,							// {4}
					member_id,							// {5}
					quantity,							// {6}
					cost,								// {7}
					description,	// {8}
					note,			// {9}
					dollar_balance,						// {10}
					qty_balance							// {11}
				});
				}
			#endregion insert 
			#region update - Probably won't be used, but thought I would throw this in here
			#endregion update - Probably won't be used, but thought I would throw this in here
			}
		#endregion publics
		}
	}