using System;
using DevExpress.Web;
using nesi.core;

public partial class sections_member_inventory_user_controls_home : System.Web.UI.UserControl
	{
	Toolbox _tools;
	public NeMember current_user { get; set; }
	private const int _page_id			= 43; // from Page table in DB
    public const string _page_name			= "BranchInventory";
	public NeBusinessUnit WorkingBusinessUnit  { get; set; }
	public NeBusinessUnit WarehouseBusinessUnit  { get; set; }

	public ASPxGridView gvmerge	{ get {return gv_merge;}}
	public ASPxGridView gvtocreate { get {return gv_tocreate;}}
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
			}
	public void fill_gv_merge()
		{
		gv_merge.DataSource 	= Toolbox.doSQL_dt(@"
SELECT 
	a.master_id, 
	a.new_id, 
	a.edit_date,
	b.min_qty,
	b.max_qty,
	b.onhand_qty,
	c.name location,
	de_old.description description_old,
	de_new.description description_new
FROM 
	inventory_item_master a
LEFT JOIN 
	inventory_branch b 
	ON  
		a.new_id = b.master_id AND 
		b.business_unit_id = @v0
LEFT JOIN inventory_location c
	ON 
		a.new_id = c.master_id AND 
		c.business_unit_id = @v0
LEFT JOIN inventory_description de_old
	ON a.master_id = de_old.master_id
LEFT JOIN inventory_description de_new
	ON a.new_id = de_new.master_id
WHERE 
	a.tag_id != 718 AND 
	a.new_id IS NOT NULL
GROUP BY a.master_id
ORDER BY a.new_id DESC",new object[] {  WarehouseBusinessUnit.id});
		gv_merge.DataBind();
		}
	public void fill_gv_tocreate()
		{
		var company		= current_user.business_unit_id == 11 ? " a.business_unit_id != 8" : string.Format(" a.business_unit_id = '{0}'", WorkingBusinessUnit.id);
		gv_tocreate.DataSource = Toolbox.doSQL_dt(string.Format(@"
SELECT 
	a.wo_detail_current_id id,
	a.wo_detail_current_date_added date_added,
	e.ddl_name companyname, 
	a.business_unit_id,
	d.member_fullname added_by, 
	a.wo_detail_current_qty_ordered qty_req,
	a.wo_detail_current_woprog_id woprog_id, 
	CAST(a.wo_detail_current_bvwo AS CHAR) bvwo,
	URLDECODE(a.wo_detail_current_notes) notes,
	a.wo_detail_current_date_required date_req,
	URLDECODE(a.wo_detail_current_description) descrip
FROM 
	wo_detail_current a
LEFT JOIN
	woprog b ON a.wo_detail_current_woprog_id = b.woprog_id
LEFT JOIN
	member d ON a.wo_detail_current_added_by = d.member_id
LEFT join
	business_unit e ON b.business_unit_id = e.id 
WHERE 
	a.wo_detail_current_master_id = 777 and 
	{0}", company),null);
		gv_tocreate.DataBind();
		}
	protected void gv_merge_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
		var gv			= (ASPxGridView) sender;
		if(gv.FilterExpression != "")
			{
			_tools.debug_note(gv.FilterExpression);
			}
		}
	protected void gv_merge_CustomFilterExpressionDisplayText(object sender, CustomFilterExpressionDisplayTextEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var gl			= new NeGridLayouts((int) current_user.id, gv.ID);
		gl.GridLayout_Name			= "Default";
		gl.GridLayout_Layout		= e.FilterExpression;
		gl.member_id	= current_user.id; 
		gl.GridLayout_Gridid		= gv.ID;
		gl.SaveGridLayout();
		}
	protected void tb_notes_Init(object sender, EventArgs e)
	{
		var tb = sender as ASPxMemo;
		var container = tb.NamingContainer as GridViewDataItemTemplateContainer;
		tb.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_tocreate.PerformCallback('n|{0}|' + s.GetText()); }}", container.KeyValue);
	}



	protected void tb_desc_Init(object sender, EventArgs e)
	{
		var tb = sender as ASPxMemo;
		var container = tb.NamingContainer as GridViewDataItemTemplateContainer;
		tb.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ gv_tocreate.PerformCallback('n|{0}|' + s.GetText()); }}", container.KeyValue);
	}
	protected void gv_tocreate_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters.StartsWith("n"))
		{
			var key = e.Parameters.Split('|').GetValue(1).ToString();
			var value = e.Parameters.Split('|').GetValue(2).ToString();
			_tools.getSQL_void(@"update wo_detail_current set wo_detail_current_notes = @v0 where wo_detail_current_id =@v1 ", new object[] {
				value,key});
		}
		else if (e.Parameters.StartsWith("d"))
		{
			var key = e.Parameters.Split('|').GetValue(1).ToString();
			var value = e.Parameters.Split('|').GetValue(2).ToString();
			_tools.getSQL_void("update wo_detail_current set wo_detail_current_description = @v0 where wo_detail_current_id =@v1 ", new object[] {
				value,key});
		}
		fill_gv_tocreate();
	}
}