using DevExpress.Web;
using nesi.core;
using System;
using System.Data;
using NESI.BLL.Pages.HomePage;

public partial class modules_auto_bingo : System.Web.UI.UserControl
	{
	NeMember current_user;

	
	protected void Page_Init(object sender, EventArgs e)
		{
		}
	protected void Page_Load(object sender, EventArgs e)
	{
	    TimeSpan start = new TimeSpan(6, 0, 0); //6am
	    TimeSpan end = new TimeSpan(23, 0, 0); //11pm
	    TimeSpan now = DateTime.Now.TimeOfDay;

	    if ((now > start) && (now < end))
	    {
	        var dt = new DataTable();
	        dt = new HomePageBase(null).GetAutoBingoList();
	        gv_watch.DataSource = dt;
	        gv_watch.DataBind();
        }
	}

	protected void gv_watch_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
	{
		if (e.DataColumn.Name == "count" && e.VisibleIndex > 0)
		{
			var val = Toolbox.ReturnZeroIfNull_int(e.CellValue);
			if (val > 0)
			{
				e.Cell.Style["background-color"] = "#fcc";
				e.Cell.Style["color"] = "#000";
				e.Cell.Style["font-weight"] = "bold";
			}
			if (val == 0)
			{
				e.Cell.Style["background-color"] = "#cfc";
				e.Cell.Style["color"] = "#000";
				e.Cell.Style["font-weight"] = "normal";
			}
		}
	}

}