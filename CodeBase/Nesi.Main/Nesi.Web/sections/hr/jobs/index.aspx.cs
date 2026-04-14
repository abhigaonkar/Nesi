using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using nesi.core;

public partial class sections_hr_jobs_index : Page
	{
	protected Toolbox _tools;
	protected NeMember current_user;
	protected void Page_Init(object sender, EventArgs e)
		{
		_tools						= new Toolbox();
		_tools.connection_string	= ds_jobs.ConnectionString;
		}
	protected void Page_Load(object sender, EventArgs e)
		{
		current_user				= Toolbox.do_handle_authentication(98);
		side_menu.InnerHtml			= new NeMenu(current_user, 98).MenuHTML;
		}
	protected void cbp_addedit_Callback(object sender, CallbackEventArgsBase e)
		{
		var cbp			= sender as ASPxCallbackPanel;
		var hid_id				= cbp.FindControl("hid_id") as HiddenField;
		var isNew						= (hid_id.Value == "");
		var pc				= cbp.FindControl("pc") as ASPxPageControl;
		var html_description	= pc.TabPages[0].FindControl("html_description") as ASPxHtmlEditor;
		var html_reqs		= pc.TabPages[1].FindControl("html_requirements") as ASPxHtmlEditor;
		var html_contactinfo	= pc.TabPages[2].FindControl("html_contactinfo") as ASPxHtmlEditor;
		var t_title				= cbp.FindControl("t_title") as ASPxTextBox;
		var t_order				= cbp.FindControl("t_order") as ASPxTextBox;
		var date_expiration	= cbp.FindControl("date_expiration") as ASPxDateEdit;
		var checkpoint				= "";
		checkpoint						+= t_title.Text.Trim() == "" ? "\nTitle not provided" : ""; 
		checkpoint						+= t_order.Text.Trim() == "" ? "\nPlease supply an order number" : "";
		checkpoint						+= t_title.Text.Trim() == "" ? "\nTitle not provided" : "";
		checkpoint						+= date_expiration.Text.Trim() == "" ? "\nPlease provide an expiration date" : "";
		checkpoint						+= html_description == null || HttpUtility.HtmlDecode(html_description.Html).Trim() == "" ? "\nPlease provide a job description" : "";
		checkpoint						+= html_reqs == null || HttpUtility.HtmlDecode(html_reqs.Html).Trim() == "" ? "\nPlease provide job requirements" : "";
		checkpoint						+= html_contactinfo == null || HttpUtility.HtmlDecode(html_contactinfo.Html).Trim() == "" ? "\nPlease provide some contact info" : "";
		if(checkpoint != "")
			{
			throw new Exception(checkpoint);
			}
		else
			{
			try
				{
				switch(isNew)
					{
					case false:
						_tools.getSQL_void(@"
UPDATE 
	wp_newelec_jobs 
SET 
	title = @v0,
	displayorder = @v1,
	description = @v2,
	requirements = @v3,
	contactinfo = @v4,
	expirationdate = @v6
WHERE
	id = @v5
LIMIT 1
		", new object[] {
						Toolbox.MySQL_safe(t_title.Text), 
						t_order.Text, 
						Toolbox.MySQL_safe(html_description.Html), 
						Toolbox.MySQL_safe(html_reqs.Html), 
						Toolbox.MySQL_safe(html_contactinfo.Html), 
						hid_id.Value,
						Toolbox.MySQL_shortdt(Convert.ToDateTime(date_expiration.Text))
						});
					break;
					case true:
						_tools.getSQL_void(@"
INSERT INTO wp_newelec_jobs 
	(
	active,
	title,
	displayorder,
	description,
	requirements,
	contactinfo,
	expirationdate
	)
VALUES
	(
	false,
	@v0,
	@v1,
	@v2,
	@v3,
	@v4,
	@v5
	)", new object[] {
						Toolbox.MySQL_safe(t_title.Text), 
						t_order.Text, 
						Toolbox.MySQL_safe(html_description.Html), 
						Toolbox.MySQL_safe(html_reqs.Html), 
						Toolbox.MySQL_safe(html_contactinfo.Html), 
						Toolbox.MySQL_shortdt(Convert.ToDateTime(date_expiration.Text))
						});
					break;
					}
				}
			catch (Exception ee)
				{
				throw ee;
				}
			}
		}
	protected void checkbox_handler_Callback(object source, CallbackEventArgs e)
		{
		var vars			= e.Parameter.Split(',');
		var row_id			= vars[0];
		var which_column		= vars[1];
		var ischeck			= vars[2];
		_tools.getSQL_void(string.Format("UPDATE wp_newelec_jobs SET {0} = {1} WHERE id = {2} LIMIT 1", which_column, ischeck, row_id));
		}
	protected void gv_jobs_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridViewCustomButtonCallbackEventArgs e)
		{
		_tools.getSQL_void(string.Format("DELETE FROM wp_newelec_jobs WHERE id = '{0}' LIMIT 1", gv_jobs.GetRowValues(e.VisibleIndex, "id")));
		gv_jobs.DataBind();
		}
}
