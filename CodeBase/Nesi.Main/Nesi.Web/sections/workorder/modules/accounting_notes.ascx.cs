using System;
using nesi.core;

public partial class sections_workorder_modules_accounting_notes : System.Web.UI.UserControl
	{
	public int woprog_id { get; set; }
	protected void Page_Load(object sender, EventArgs e)
		{
		if(!Visible) return;
		DataBind();
		}
	public override void DataBind()
		{
		if(woprog_id > 0)
			{
			accounting_notes.InnerText		= Toolbox.doSQL_string(@"SELECT woprog_glposting_instructions FROM woprog WHERE woprog_id = @v0", woprog_id);
			}
		}
	}