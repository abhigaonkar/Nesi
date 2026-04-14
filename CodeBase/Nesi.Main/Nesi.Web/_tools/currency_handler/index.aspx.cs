using System;
using nesi.core;

public partial class this_currency_handler : System.Web.UI.Page
{
    private const int _page_id = 1; // from Page table in DB

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        var _q = Request.QueryString;

        if ((_q["date"] != null) && (_q["currency"] != "")&&(_q["rate"]!=null))
        {
			_tools.getSQL_void(@"Insert into currency (date,currency,per_usd)  values(@v0,@v1,@v2)",new object[] { _q["date"],_q["currency"],_q["rate"] } );
        }
		Response.Write("success");
        Response.End();

    }
}
