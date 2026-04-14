using System;
using nesi.core;

public partial class modules_journal_entries : System.Web.UI.UserControl
{	Toolbox _tools;
	public int woprog_id { get; set; }
	protected void Page_Load(object sender, EventArgs e)
	{
		_tools = new Toolbox();
		hdn_tasks_woid.Value = woprog_id.ToString();
		if (!IsPostBack)
		{
			//Fill dropdownlist with all projects names

		}
	}


}