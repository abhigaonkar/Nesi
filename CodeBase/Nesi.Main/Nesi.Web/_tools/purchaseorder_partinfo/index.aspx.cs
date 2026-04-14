using System;
using nesi.core;

public partial class _tools_purchaseorder_partinfo_index : System.Web.UI.Page
{
    NeMember _member;
    NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
//        string QTY = "";
        var _tools = new Toolbox();
        _member = Toolbox.do_handle_authentication(_page_id);
        _company = new NeBusinessUnit(_member.business_unit_id);
//        double price = 0;
        var description = "";
        var vendorpartno = "";
        var vendorcost = "";
        var _q = Request.QueryString;
        //System.Collections.Specialized.NameValueCollection _a = Request.QueryString[1];
        var OUTPUT = "";
       // DataTable _dt;
        _tools.set_XML_header();

        if (_q["q"] != null)
        {
            var newinv = new inventory();
            var compid = _member.business_unit_id;
            var vendid = "";
            try
            {
                compid = Convert.ToInt32(_q["c"]);
            }
            catch
            {
                compid = _member.business_unit_id;
            }
            try
            {
                vendid = _q["v"];
            }
            catch
            {

            }
            

            var whatisthis = _q["q"];
           // string vendno = "";
            var VendPart = "";
            //string checkthis = "";
            var vendorqty = "";
            try
            {
                newinv.Load(_q["q"], compid);
                description = newinv.description;
                description = _tools.value_from(description);
                try
                {
                    VendPart = _tools.getSQL_string(@"SELECT vendor_code FROM inventory_price  WHERE master_id =@v0 AND vendor_id=@v1 ", new object[] { _q["q"],vendid });
                    vendorcost = _tools.getSQL_string(@"SELECT cost FROM inventory_price  WHERE master_id =@v0 AND vendor_id=@v1 ", new object[] { _q["q"],vendid });
                    vendorqty = _tools.getSQL_string(@"SELECT qty FROM inventory_price  WHERE master_id =@v0 AND vendor_id=@v1 ", new object[] { _q["q"],vendid });
                    
                }
                catch(Exception ex)
                {
                    var value = ex.ToString();
//                    string hold = "pause";
                }
                if (VendPart != "")
                {
                   vendorpartno = VendPart;
                }
                else
                {
                    vendorpartno = "";

                }

            }
            catch
            {
                description = "Not Found";
                vendorpartno = "Not Found";
                vendorqty = "Not Found";
            }

            OUTPUT += string.Format(@"
      <purchaseorder description              = ""{0}"" 
       vednpartno                             = ""{1}""
       vendorcost                             = ""{2}""
       vendorqty                              = ""{3}""/>",
                description,										// {0}
                vendorpartno,									// {1}
                vendorcost,                                     //{2}
                vendorqty);                                    //{3}

            Response.Write(OUTPUT);
            Response.End();

        }

    
    }
}
