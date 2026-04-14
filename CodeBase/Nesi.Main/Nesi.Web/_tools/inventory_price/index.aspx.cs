using System;
//using nesi.bv;
using nesi.core;

public partial class this_price_search : System.Web.UI.Page
{
    NeMember _member;
    NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var QTY = "";
        var _tools = new Toolbox();
        _member = Toolbox.do_handle_authentication(_page_id);
        _company = new NeBusinessUnit(_member.business_unit_id);
        double price = 0;

        var _q = Request.QueryString;
        //System.Collections.Specialized.NameValueCollection _a = Request.QueryString[1];
        var OUTPUT = "";
        _tools.set_XML_header();

        if (_q["q"] != null)
        {
            var newinv = new inventory();
            var compid = _member.business_unit_id;
            try
            {
                compid = Convert.ToInt32(_q["c"]);
            }
            catch
            {
                compid = _member.business_unit_id;
            }
            //newinv.Load(_q["q"], _member.business_unit_id);
            double ammount = 1;
            try
            {
                ammount = Convert.ToDouble(_q["a"]);
            }
            catch
            {
                ammount = 1;
            }
            if (ammount == 0)
                ammount = 1;

            var whatisthis = _q["q"];

            try
            {
                newinv.Load(_q["q"], compid);
                OUTPUT = newinv.description;
            }
            catch
            {
                OUTPUT = "PARTNOTFOUND";
            }
            if (OUTPUT != "" && OUTPUT != "PARTNOTFOUND")
             {
                 if (newinv.is_qty == true)
                     QTY = "QTY";
                 if (whatisthis == "2139")
                 {
                     var pricehold = _q["pr"];
                     price = Convert.ToDouble(pricehold);

                 }
                 else
                 {
                    price = shared.GetSellPrice(newinv.cost_price_branch, newinv.sell_price, QTY, ammount, compid);
                 }
             }
             //else if (OUTPUT == "" || OUTPUT == "PARTNOTFOUND")
             //{
             //    var DSN = "";
             //   DSN = _company.GetBUDSN(compid);
             //    var newbvinv = new InventoryItem();
             //    newbvinv.PartNumber = _q["pn"].Trim();
             //    try
             //    {
             //        newbvinv.Load(DSN, "00", _q["pn"]);
             //        if (newbvinv.ProductCode.Trim() == "QTY")
             //            QTY = "QTY";
             //        price = shared.GetSellPrice(newbvinv.Cost, newbvinv.sell_price, QTY, ammount, compid);
             //    }
             //    catch
             //    {
             //        price = 0;
             //    }
			 //
             //}
            //txtPrice.Text = price.ToString();

            //Inventory Inventroy = new Inventory();
            //Inventroy.Load(_q["q"]);
            Response.Write("<price>" + price + "</price>");
            Response.End();

        }

    }
}
