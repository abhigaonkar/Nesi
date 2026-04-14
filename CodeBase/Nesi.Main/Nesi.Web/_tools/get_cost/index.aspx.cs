using System;
using nesi.core;

public partial class this_get_cost : System.Web.UI.Page
{
    NeMember _member;
 //   NeBusinessUnit _company;
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        _member = Toolbox.do_handle_authentication(_page_id);
  //      _company = new NeBusinessUnit(_member.business_unit_id);
        double cost = 0;

        var _q = Request.QueryString;
        var OUTPUT = "";
        _tools.set_XML_header();

        if ((_q["q"] != null) && (_q["q"] != ""))
        {
            cost = _tools.getSQL_double(@"SELECT get_current_cost(@v0,@v1)",new object[] { _q["q"],_q["c"] } );


            var newinv = new inventory();
            newinv.Load(_q["q"], _q["c"]);
            if (newinv.is_exclude)
                cost = 0;
        }
        OUTPUT += string.Format(@"
      <cost>{0}</cost>",
              cost);			// {0}
        Response.Write(OUTPUT);
        Response.End();

    }
}
