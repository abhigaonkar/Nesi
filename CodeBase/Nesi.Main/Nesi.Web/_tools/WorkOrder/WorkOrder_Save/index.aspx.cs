using System;
using System.Web.UI;
using nesi.core;

public partial class this_workOrder_save_opening_page : Page
{
    NeMember _member;
    NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var QTY = "";
        var _type = "material";
        var _tools = new Toolbox();
        _member = Toolbox.do_handle_authentication(_page_id);
        _company = new NeBusinessUnit(_member.business_unit_id);
        double price = 0;
        double originalprice = 0;

        var _q = Request.QueryString;
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
            double amount = 0.0;
            try
            {
                amount = Convert.ToDouble(_q["a"]);
            }
            catch
            {
                amount = 1;
            }
            if (amount == 0)
                amount = 1;

            try
            {
               originalprice = Convert.ToDouble(_q["pr"]);

            }
            catch
            {

            }
            var whatisthis = _q["q"];

            if (whatisthis == "null")
            {

                price = originalprice;
            }
            else
            {
                try
                {
                    if (Convert.ToInt32(whatisthis) >= 2000000)
                    {
                        _type = "kitted";
                    }
                    else if (Convert.ToInt32(whatisthis) >= 990000)
                    {
                        _type = "labour";
                    }
                }
                catch
                {}
                if (_type != "kitted")
                {
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
                        if (newinv.is_qty)
                            QTY = "QTY";
                        /*if (whatisthis == "2139")
                        {
                            string pricehold = _q["pr"].ToString();
                            price = Convert.ToDouble(pricehold);

                        }
                        else
                        {*/
                        if (QTY == "QTY")
                        {
                            price =  shared.GetSellPrice(newinv.cost_price_branch, newinv.sell_price, QTY, amount, compid);
                        }
                        else
                        {
                            price = originalprice;
                        }
                        //}
                    }
                    else
                    {
                        price = originalprice;
                    }
                }
                else
                {
                    try
                    {
					price = _tools.getSQL_double(@"Select GetKittedSell(@v0 ,@v1 ,@v2 )", new object[] {  amount, compid, Convert.ToInt32(whatisthis) } );
                    }
                    catch
                    {
                        price = originalprice;
                    }

                }
            }
			Response.Write(string.Format("<response><price>{0:N2}</price><is_exclude>{1}</is_exclude></response>", price, newinv.is_exclude));
            Response.End();

        }
    }
}
