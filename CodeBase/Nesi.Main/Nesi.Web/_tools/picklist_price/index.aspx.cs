using System;
using System.Collections.Specialized;
using nesi.core;

public partial class this_picklist_price_index : System.Web.UI.Page
	{
	private NeMember current_user;
	Toolbox _tools;
	bool member_can_see_cost;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools			= new Toolbox();
		current_user	= Toolbox.do_handle_authentication(1);
		member_can_see_cost	= current_user.AuthenticatedForPrivilege(58);
		_tools.dont_cache_page();
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		var QTY = "";
		var origin = "";
		var id = "";
		var rev = "";
		var lineid = "0";
		double price = 0;
		double original_sell = 0;
		double supplied_cost = 0;

		var _q = Request.Form;
		var OUTPUT = "";
		_tools.set_XML_header();

			var _master_id	= _q["master_id"];
			var inv		= new inventory();
			var business_unit_id		= string.IsNullOrEmpty(_q["business_unit_id"]) ? current_user.business_unit.warehouse_bu_id : Convert.ToInt32(_q["business_unit_id"]);
			var businessUnit		= new NeBusinessUnit(business_unit_id);
			business_unit_id		= businessUnit.warehouse_bu_id;
			var quantity		= string.IsNullOrEmpty(_q["qty"]) || _q["qty"] == "null"  ? 1 : Convert.ToDouble(_q["qty"]);
			var amounthold	= string.IsNullOrEmpty(_q["qty"]) || _q["qty"] == "null"  ? 1 : Convert.ToDouble(_q["qty"]);
			quantity			= quantity == 0 ? 1 : quantity;
			amounthold			= amounthold == 0 ? 1 : amounthold;
			original_sell		= string.IsNullOrEmpty(_q["original_sell"]) || _q["original_sell"] == "null" ? 0 : Convert.ToDouble(_q["original_sell"]);
			id					= string.IsNullOrEmpty(_q["id"]) ? "" : _q["id"];
			origin				= string.IsNullOrEmpty(_q["origin"]) ? "" : _q["origin"];
			rev					= string.IsNullOrEmpty(_q["rev"]) ? "" : _q["rev"];
			lineid				= string.IsNullOrEmpty(_q["lineid"]) ? "0" : _q["lineid"];
			supplied_cost		= string.IsNullOrEmpty(_q["cost"]) ? 0 : Convert.ToDouble(_q["cost"]);
			double used_cost	= 0;
			double used_sell	= 0;
			var _type		= "material";


			if (_master_id == "labor")
				{
				used_sell = original_sell;
				}
			else
				{
				var master_id			= 0;
				int.TryParse(_master_id, out master_id);
				_type					= master_id >= 2000000 ? "kitted" : master_id >= 990000 ? "labour" : _type;
				var part_exists		= false;
				if (_type != "kitted")
					{
					#region not kitted
					if(master_id != 0)
						{
						part_exists		= inv.part_exists(master_id);
						if(part_exists)
							{
							inv.Load(master_id, business_unit_id);
							}
						}
					if(master_id == 2139)
						{
						used_sell		= original_sell;
						}
					else
						{
						if (part_exists)
							{
							//get Qty of all same parts
							double current_qty = 0;
							if (!inv.is_exclude)
								{
								switch (origin)
									{
									case "quote":
										current_qty = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(qty), 0) FROM quote_worksheet WHERE quote_id = @v0  AND revision = @v1  AND part_no = @v2  AND id != @v3 ", new object[] {  id, rev, master_id, lineid } );
									break;
									case "workorder":
										current_qty = Toolbox.doSQL_double(@"SELECT IFNULL(SUM(wo_detail_current_qty_committed), 0) FROM wo_detail_current WHERE wo_detail_current_woprog_id = @v0  AND wo_detail_current_master_id = @v1  and wo_detail_current_id != @v2 ", new object[] {  id, master_id, lineid } );
									break;
									}
								}

							quantity	= quantity + current_qty;
							used_cost	= Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )", new object[] {  master_id, business_unit_id, quantity } );// newinv.cost_price_branch;
							used_sell	= shared.GetSellPrice(used_cost, 0, inv.is_qty, quantity, business_unit_id);

							if(used_cost == 0.01 && supplied_cost > 0.01)
								{
								used_cost	= supplied_cost;
								used_sell	= shared.GetSellPrice(supplied_cost, 0, inv.is_qty, quantity, business_unit_id);
								}
							}
						else
							{
							used_cost		= supplied_cost;
							used_sell		= original_sell;
							}
						}
					#endregion not kitted
					}
				else
					{
					try
						{
						used_sell = Toolbox.doSQL_double(@"SELECT GETKITTEDSELL(@v0 ,@v1 ,@v2 )", new object[] {  quantity, business_unit_id, master_id } );
						}
					catch
						{
						used_sell = original_sell;
						}
					}
				}
			Response.Write(string.Format(@"
<response>
	<cost_price>{0}</cost_price>
	<sell_price>{1:F3}</sell_price>
	<is_exclude>{2}</is_exclude>
	<allowed_to_stock>{3}</allowed_to_stock>
</response>", (member_can_see_cost || origin == "quote" ? used_cost : 0), used_sell, inv.is_exclude, inv.allowed_to_stock));
			Response.End();
		}
	}
