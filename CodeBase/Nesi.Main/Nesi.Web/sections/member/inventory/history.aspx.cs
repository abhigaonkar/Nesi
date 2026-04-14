using System;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using DevExpress.Web;
using nesi.core;

public partial class sections_member_inventory_history : System.Web.UI.Page
	{
	public NeMember myMember;
	JavaScriptSerializer jSON				= new JavaScriptSerializer();
	private const int _page_id			= 1; // from Page table in DB
    public string selected_master;
    private const string _page_name			= "BranchInventoryHistory";
	protected bool is_purchaser				= false;
	Toolbox _tools;
	
    protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		myMember							= Toolbox.do_handle_authentication(_page_id);
		var _q				= Request.QueryString;
		if(myMember.id == 711)
			{
			gv_history.Columns[8].Visible	= true;
			}
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		}
	protected void gv_history_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
		{
		if(e.DataColumn.FieldName == "location")
			{
			var name		= Toolbox.ReturnBlankIfNull_string(e.CellValue).ToString();
			if(name.Contains("(Deleted Location - ID#: "))
				{
				e.Cell.Style.Add("color", "#999");
				e.Cell.Style.Add("text-decoration", "line-through");
				}
			}
		}
}
