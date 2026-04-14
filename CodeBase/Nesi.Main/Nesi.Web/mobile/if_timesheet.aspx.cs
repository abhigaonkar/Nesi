using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mobile_if_timesheet : System.Web.UI.Page
	{
	protected void Page_Init(object sender, EventArgs e)
		{
		var _q		= Request.QueryString;
		int id;
		int.TryParse(_q["id"], out id);
		timesheet.workorder_id	= id;
		}
	}