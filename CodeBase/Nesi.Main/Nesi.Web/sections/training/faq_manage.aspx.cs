using System;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxHtmlEditor;
using nesi.core;

public partial class sections_training_faq_manage : System.Web.UI.Page
	{
	private NeMember current_user;
	private const int _page_id		= 146; // from Page table in DB
	private const string _page_name		= "FAQ Management";
	Toolbox _tools;

	protected void Page_Init(object sender, EventArgs e)
		{
		_tools								= new Toolbox();
		current_user						= Toolbox.do_handle_authentication(_page_id);
		var menu							= new NeMenu(current_user, Convert.ToInt32(_page_id));
		divMenu.InnerHtml					= menu.MenuHTML;
		divSide.InnerHtml					= shared.PrintSidePanelHTML(current_user);
		_tools.dont_cache_page();
		}
	protected void Page_Load(object sender, EventArgs e)
		{

		}
	protected void gv_sections_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var name			= (HtmlInputText) gv.FindEditFormTemplateControl("section_tb_name");
		var id						= (int) gv.GetDataRow(gv.EditingRowVisibleIndex)["id"];
		var section			= new faq_section(id);
		section.name				= name.Value;
		section.save();
		e.Cancel					= true;
		gv.CancelEdit();
		gv.DataBind();
		}
	protected void gv_sections_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var name			= (HtmlInputText) gv.FindEditFormTemplateControl("section_tb_name");
		var section			= new faq_section();
		section.name				= name.Value;
		section.save();
		e.Cancel					= true;
		gv.CancelEdit();
		gv.DataBind();
		}
	protected void gv_sections_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		if(gv.EditingRowVisibleIndex >= 0)
			{
			var name			= (HtmlInputText) gv.FindEditFormTemplateControl("section_tb_name");
			name.Value					= gv.GetDataRow(gv.EditingRowVisibleIndex)["name"].ToString();
			}
		}
	protected void gv_sections_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		e.Properties["cpIsEdit"] = ((ASPxGridView)sender).IsEditing;
		}
	protected void gv_faq_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		e.Properties["cpIsEdit"] = ((ASPxGridView)sender).IsEditing;
		}
	protected void gv_sections_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var id						= (int)  e.Keys[0];
		var section			= new faq_section(id);
		section.delete(id);
		e.Cancel					= true;
		gv.CancelEdit();
		gv.DataBind();
		}
	protected void gv_faq_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var html			= (ASPxHtmlEditor) gv.FindEditFormTemplateControl("html_body");
		var subject		= (HtmlInputText) gv.FindEditFormTemplateControl("faq_tb_subject");
		var cb					= (CheckBox) gv.FindEditFormTemplateControl("cb_visible");
		var section		= (DropDownList)  gv.FindEditFormTemplateControl("ddl_sections");
		if(gv.EditingRowVisibleIndex >= 0)
			{
			html.Html				= gv.GetDataRow(gv.EditingRowVisibleIndex)["body"].ToString();
			cb.Checked				= Convert.ToBoolean(gv.GetDataRow(gv.EditingRowVisibleIndex)["customer_visible"]);
			section.SelectedValue	= gv.GetDataRow(gv.EditingRowVisibleIndex)["section_id"].ToString();
			subject.Value			= gv.GetDataRow(gv.EditingRowVisibleIndex)["subject"].ToString();
			}
		else
			{
			html.Html				= "";
			subject.Value			= "";
			cb.Checked				= false;
			section.SelectedIndex	= 0;
			}
		}
	protected void gv_faq_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var html			= (ASPxHtmlEditor) gv.FindEditFormTemplateControl("html_body");
		var subject		= (HtmlInputText) gv.FindEditFormTemplateControl("faq_tb_subject");
		var section		= (DropDownList)  gv.FindEditFormTemplateControl("ddl_sections");
		var cb					= (CheckBox) gv.FindEditFormTemplateControl("cb_visible");
		var f						= new faq();
		f.customer_visible			= cb.Checked;
		f.subject					= subject.Value;
		f.section_id				= Convert.ToInt32(section.SelectedValue);
		f.body						= html.Html;
		f.save();
		e.Cancel					= true;
		gv.CancelEdit();
		gv.DataBind();
		}
	protected void gv_faq_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var html			= (ASPxHtmlEditor) gv.FindEditFormTemplateControl("html_body");
		var subject		= (HtmlInputText) gv.FindEditFormTemplateControl("faq_tb_subject");
		var cb					= (CheckBox) gv.FindEditFormTemplateControl("cb_visible");
		var section		= (DropDownList)  gv.FindEditFormTemplateControl("ddl_sections");
		var id						= (int) gv.GetDataRow(gv.EditingRowVisibleIndex)["id"];
		var f						= new faq(id);
		f.customer_visible			= cb.Checked;
		f.subject					= subject.Value;
		f.section_id				= Convert.ToInt32(section.SelectedValue);
		f.body						= html.Html;
		f.save();
		e.Cancel					= true;
		gv.CancelEdit();
		gv.DataBind();
		}
	protected void gv_faq_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
		{
		var gv				= (ASPxGridView) sender;
		var id						= (int)  e.Keys[0];
		var f						= new faq(id);
		f.delete(id);
		e.Cancel					= true;
		gv.CancelEdit();
		gv.DataBind();
		}
}