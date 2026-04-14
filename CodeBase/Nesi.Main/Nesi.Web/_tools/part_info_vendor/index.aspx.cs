using System;
using nesi.core;

public partial class _tools_part_info_vendor_index : System.Web.UI.Page
{
    Toolbox _Tools = new Toolbox();
    protected void Page_Load(object sender, EventArgs e)
    {
        var poid = Request.QueryString["id"];
        var partno = Request.QueryString["partid"]; ;
        var business_unit_id = Request.QueryString["compid"];
        var sentvendorpartno = Request.QueryString["vendpartno"];
        var VendPart = "";
        var vendorcost = "";
        var vendorqty = "";
        var OUTPUT = "";
        var checkvendorpart = "";
        _Tools.set_XML_header();
        var progress = new NePOProg(Convert.ToInt32(poid));
        var vendno = progress.poprog_vendor_id.ToString();
        try
        {
            VendPart = _Tools.getSQL_string(@"Select ifnull((SELECT vendor_code FROM inventory_price  WHERE master_id =@v0 AND vendor_id=@v1  AND business_unit_id =@v2  order by inventory_price.edited_dt desc limit 1),'')", new object[] { partno,vendno,business_unit_id });
        }
        catch { }
        try
        {
			vendorcost = _Tools.getSQL_string(@"Select ifnull((SELECT total FROM inventory_price  WHERE master_id =@v0 AND vendor_id=@v1  AND business_unit_id =@v2  order by inventory_price.edited_dt desc limit 1),'')", new object[] { partno,vendno,business_unit_id });
			//vendorcost = _Tools.getSQL_string(@"SELECT get_current_cost_v1(@v0,@v1)",new object[] { partno,business_unit_id } );
			}
		catch 
        {
        }
        try
        {
			vendorqty = _Tools.getSQL_string(@"Select ifnull((SELECT qty FROM inventory_price  WHERE master_id =@v0 AND vendor_id=@v1  AND business_unit_id =@v2  order by inventory_price.edited_dt desc limit 1),'')", new object[] { partno,vendno,business_unit_id });
        }
        catch { }
        try
        {
            if (sentvendorpartno != null)
            {
                checkvendorpart = _Tools.getSQL_string(@"SELECT IFNULL( ( SELECT id FROM inventory_price  WHERE master_id !=@v0 AND vendor_id=@v1  AND business_unit_id =@v2  AND vendor_code=@v3  limit 1),'')", new object[] { partno,vendno,business_unit_id,_Tools.value_to(sentvendorpartno) });
            }
        }
        catch { }

        OUTPUT += string.Format(@"
      <purchaseorder vednpartno               = ""{0}""
       vendorcost                             = ""{1}""
       vendorqty                              = ""{2}""
       checkvendorpart                        = ""{3}""/>",
               VendPart,									// {0}
               vendorcost,                                     //{1}
               vendorqty,                                    //{2}
               checkvendorpart);                             //3

        Response.Write(OUTPUT);
        Response.End();
    }
}
