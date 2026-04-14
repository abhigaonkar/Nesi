using System;
using nesi.core;

public partial class this_get_origin : System.Web.UI.Page
{
    NeMember _member;
    NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        _member = Toolbox.do_handle_authentication(_page_id);
        _company = new NeBusinessUnit(_member.business_unit_id);
        var notes = "";

        var _q = Request.QueryString;  //System.Collections.Specialized.NameValueCollection
        //string _a = Request.QueryString[1];
        var OUTPUT = "";
        _tools.set_XML_header();
        var sql = "";
        if (_q["id"] != null)
        {
           
            if (_q["source"] == "workorder")
            {
                sql = string.Format("Select wo_detail_current_origin from wo_detail_current where wo_detail_current_ID = {0}", _q["id"]);
            }
            try
            {
                notes = _tools.getSQL_string (sql,null);
            }
            catch { }

            OUTPUT += string.Format(@"
      <originreturn>{0}</originreturn>",
               notes);			// {0}
            Response.Write(OUTPUT);
            Response.End();

        }

    }
}
