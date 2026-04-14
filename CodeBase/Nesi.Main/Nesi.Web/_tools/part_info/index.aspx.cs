using System;
using System.Collections.Specialized;
using nesi.core;

public partial class part_info_index : System.Web.UI.Page
{
    NeMember _current_user;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var _start = DateTime.Now;
        var _tools = new Toolbox();
        _current_user = Toolbox.do_handle_authentication(_page_id);
        var _inventory = new inventory();
        var func = new this_functions();
        var _q = Request.QueryString;
        var onhand = "0";
        var OUTPUT = "";

        _tools.set_XML_header();
        //_tools.set_plain_header();
        Response.Clear();
        _inventory.current_user = _current_user;
        if (_q["a"] != null && _q["id"] != null && _q["business_unit_id"] != null)
        {
            var WorkingBusinessUnit = new NeBusinessUnit(_q["business_unit_id"]);
            var WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);

            switch (_q["a"])
            {
                #region g
                case "g":
                    try
                    {
                        //
                        // Pass the origin so we can know where is the calling from: from the 'line items' tab of workorder or others.
                        //
                       // _inventory.Load(_q["id"], WarehouseBusinessUnit.id, _q["origin"]);
                        _inventory.Load(_q["id"], WarehouseBusinessUnit.id);
                        if (_inventory.active)
                        {
                            var sentSellPrice = _inventory.sell_price;
                            try { onhand = _tools.getSQL_string(@"SELECT IFNULL(MAX(onhand_qty),0) FROM inventory_branch WHERE master_id=@v0  AND business_unit_id=@v1 ", new object[] { _inventory.master_id, WarehouseBusinessUnit.id }); }
                            catch { }
                            if (_inventory.sell_price == 0 && !_inventory.is_exclude)
                            {
                                sentSellPrice = shared.GetSellPrice(Math.Round(_inventory.cost_price_branch, 3), 0, true, 1, WorkingBusinessUnit.id32);
                            }

                            var _length = DateTime.Now.Subtract(_start);
                            var origin = !string.IsNullOrEmpty(_q["origin"]) ? _q["origin"] : "";
                            if (origin != "")
                            {
                                switch (origin)
                                {
                                    case "purchaseorder":
                                        _inventory.cost_price_branch = 0;
                                        if(!_inventory.Tag.IncludeSearch)
                                            {
                                            throw new Exception("This part # is not allowed to be added directly to a purchase order");
                                            }
                                        break;
                                    case "workorder":
                                        var is_rental = _inventory.part_is_Rental_Item(_q["id"]);
                                        if (is_rental)
                                        {
                                            throw new Exception("Rental items cannot be added from workorder.");
                                        }
                                        if(!_inventory.Tag.IncludeSearch)
                                            {
                                            throw new Exception("This part # is not allowed to be added directly to a work order");
                                            }
                                        break;
                                }
                            }
                            OUTPUT = string.Format(@"<response>
	<description_full>{0}</description_full>
	<description>{1}</description>
	<cost_price>{6}</cost_price>
	<sell_price>{2}</sell_price>
	<loadtime>{3:N2}s</loadtime>
	<exclude_part>{4}</exclude_part>
	<onhand>{5}</onhand>
	<should_be_part>{8}</should_be_part>
	<tag_id>{7}</tag_id>
</response>",
        _inventory.description_full.Replace("&", "&amp;"),
        _inventory.description.Replace("&", "&amp;"),
        sentSellPrice,
        (_length.TotalMilliseconds / 1000),
        _inventory.is_exclude,
        onhand,
        _current_user.AuthenticatedForPrivilege(58) ? Math.Round(_inventory.allowed_to_stock ? _inventory.cost_price_branch : Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, 1)", new object[] { _inventory.master_id, _inventory.business_unit_id }), 3) : 0,
        _inventory.tag_id,
        _inventory.master_id
    );
                        }
                        else
                        {
                            throw new Exception("Part not active");
                        }
                    }
                    catch (Exception ee)
                    {
                        OUTPUT = string.Format("<response><error>{0}</error></response>", ee.Message);
                    }
                    break;
                #endregion g
                #region s
                case "s":
                    try
                    {
                        if (_q["origin"] == null || _q["qty"] == null || _q["sell"] == null || _q["cost"] == null)
                        {
                            OUTPUT = "<error>Bad Request due to incomplete parameters</error>";
                        }
                        else
                        {
                            var master_id = _q["id"] != "" ? Convert.ToInt32(_q["id"]) : 0;
                            var business_unit_id = WarehouseBusinessUnit.id.ToString();
                            var origin = _q["origin"];
                            var quantity = _q["qty"] != "" ? Convert.ToDouble(_q["qty"]) : 0;
                            var sellprice = _q["sell"] != "" ? Convert.ToDouble(_q["sell"]) : 0;
                            var extended_quoted = quantity * Math.Round(sellprice, 2);
                            double extended_tm = 0;
                            var cost = Convert.ToDouble(_q["cost"]);
                            var exclduecode = "";
                            if (_inventory.part_exists(master_id))
                            {
                                _inventory.Load(master_id, business_unit_id);
                                if (_inventory.is_qty)
                                {
                                    sellprice = Math.Round(shared.GetSellPrice(_inventory.cost_price_branch, _inventory.sell_price, _inventory.is_qty, quantity, WorkingBusinessUnit.id32), 2);
                                    extended_quoted = quantity * Math.Round(sellprice, 2);
                                }
                                if (origin == "workorder")
                                {

                                    try
                                    {
                                        exclduecode = _tools.getSQL_string(@"SELECT Inv_Exclude_Code FROM inv_exclude WHERE Inv_Exclude_Code = @v0 ", new object[] { master_id });
                                    }
                                    catch
                                    {
                                        exclduecode = "";
                                    }
                                    if (exclduecode != "" || _inventory.is_exclude == true)
                                    {
                                        if (_current_user.AuthenticatedForPrivilege(58))
                                        {
                                            cost = 0;
                                            sellprice = 0;
                                        }
                                    }
                                    if (_current_user.AuthenticatedForPrivilege(58))
                                    {
                                        cost = _inventory.cost_price_branch;
                                    }
                                    else
                                    {
                                        cost = 0;
                                    }
                                }
                                else if (origin == "purchaseorder")
                                {
                                    try
                                    {
                                        var orderext = cost * quantity;
                                        extended_tm = orderext;
                                    }
                                    catch
                                    {
                                        extended_tm = 0;
                                    }
                                }
                            }
                            else if (master_id >= 2000000)
                            {
                                try
                                {
                                    sellprice = _tools.getSQL_double(@"Select getKittedSell(@v0 ,@v1 ,@v2 )", new object[] { quantity, business_unit_id, master_id });
                                }
                                catch
                                {
                                    sellprice = 0;
                                }
                                extended_quoted = quantity * sellprice;
                            }

                            if (origin != "purchaseorder")
                            {
                                extended_tm = extended_quoted;
                            }
                            OUTPUT = string.Format(@"<response>
<sell_price>{0}</sell_price>
<cost_price>{1}</cost_price>
<extended_tm>{2}</extended_tm>
<extended_quoted>{3}</extended_quoted>
</response>", sellprice, cost, extended_tm, extended_quoted);
                        }
                    }
                    catch (Exception ee)
                    {
                        OUTPUT = string.Format("<response><error>{0}</error></response>", ee.Message);
                    }
                    break;
                #endregion s
                #region k
                case "k":

                    break;
                    #endregion k
            }
        }
        else
        {
            OUTPUT = "<error>Bad Request due to no parameters sent</error>";
        }
        Response.Write(OUTPUT);
        Response.End();
    }


    public class this_functions
    {
    }

}