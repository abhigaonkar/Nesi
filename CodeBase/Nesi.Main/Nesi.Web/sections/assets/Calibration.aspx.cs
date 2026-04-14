using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using DevExpress.Web;
using nesi.core;


public partial class sections_assets_Asset_Calibration : System.Web.UI.Page
	{

	NeMember current_user;
	private static int _PAGE_ID = 60;  //tooling and assets Page
	private const string _page_name = "Asset_Calibration";
	NameValueCollection q;

	private static Regex _numeric = new Regex(@"^\d+$");

	protected void Page_Init(object sender, EventArgs e)
		{
		q = Request.QueryString;
		current_user = Toolbox.do_handle_authentication(_PAGE_ID);
		sqlcompanys.SelectCommand = "Select Id, IFNULL(ddl_name, description) company from business_unit where id in (" + new Current_User().visible_business_units + ") order by company";
		gv_assets_Calibration.DataBind();
		}

	protected void Page_Load(object sender, EventArgs e)
		{
		if(!IsPostBack)
			{
            gv_assets_Calibration.FilterExpression = "[branch]=" + current_user.business_unit_id;
			}
		}
	protected void cp_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{

		}
	protected void ucImage_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
		{
		}


	protected void gv_assets_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
		{
		}
	protected void gv_assets_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
		{
		}
		

	protected void dte_addhistory_DataBound(object sender, EventArgs e)
		{
		}
	protected void ddl_addhistorymember_DataBound(object sender, EventArgs e)
		{
		}
	protected void ddlaction_DataBound(object sender, EventArgs e)
		{
		}






	}

