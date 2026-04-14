using System;
using System.Web.UI;
using nesi.core;

public partial class _tools_get_transfer_data_index : Page
{
    NeMember _member;
    NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB
    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        _member = Toolbox.do_handle_authentication(_page_id);
        _company = new NeBusinessUnit(_member.business_unit_id);
        var partid = "";
        var qtyofpart = "";
        var originofpart = "";

        var _q = Request.QueryString;  //System.Collections.Specialized.NameValueCollection
        //string _a = Request.QueryString[1];
        var OUTPUT = "";
        _tools.set_XML_header();
        var sql = "";
        if (_q["id"] != null)
			{
			try
				{
				partid = _tools.getSQL_string(@"SELECT wo_detail_current_master_id FROM wo_detail_current  WHERE wo_detail_current_id =@v0", new object[] { _q["id"] });
				}
			catch
				{
				partid = _tools.getSQL_string(@"SELECT wo_detail_history_master_id FROM wo_detail_history  WHERE wo_detail_history_id =@v0", new object[] { _q["id"] });
				}
			try
				{
				qtyofpart = _tools.getSQL_string(@"SELECT wo_detail_current_qty_committed FROM wo_detail_current  WHERE wo_detail_current_id =@v0", new object[] { _q["id"] });
				}
			catch
				{
				qtyofpart = _tools.getSQL_string(@"SELECT wo_detail_history_qty_committed FROM wo_detail_history  WHERE wo_detail_history_id =@v0", new object[] { _q["id"] });
				}
			try
				{
				originofpart = _tools.getSQL_string(@"SELECT wo_detail_current_origin FROM wo_detail_current  WHERE wo_detail_current_id =@v0", new object[] { _q["id"] });
				}
			catch
				{
				originofpart = _tools.getSQL_string(@"SELECT wo_detail_history_origin FROM wo_detail_history  WHERE wo_detail_history_id =@v0", new object[] { _q["id"] });
				}
			}


        OUTPUT += string.Format(@"
         <transferdata partid                  = ""{0}"" 
          qtyofpart                             = ""{1}""
          originofpart                          = ""{2}""/>",
                partid,										
                qtyofpart,
                originofpart);                                   
        Response.Write(OUTPUT);
        Response.End();
    }
}
