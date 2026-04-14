using System;
using nesi.core;

public partial class this_sell_price : System.Web.UI.Page
	{
	NeMember _member;
	int _business_unit_id = 0;
	double _qty, _cost, _sell = 0;

	protected void Page_Load(object sender, EventArgs e)
		{
		var _q = Request.QueryString;
		_member = Toolbox.do_handle_authentication(1);
		_business_unit_id = _member.business_unit_id;
		double.TryParse(_q["qty"], out _qty);
		double.TryParse(_q["cost"], out _cost);

		if(!string.IsNullOrEmpty(_q["bu_id"]))
			{
			int.TryParse(_q["bu_id"], out _business_unit_id);
			}

		_sell = shared.GetSellPrice(_cost, 0, true, _qty, _business_unit_id);
		Toolbox.do_set_XML_header(Response);
		Response.Write(string.Format(@"<PriceReturn price=""{0}""/>",_sell));
		}
	}
