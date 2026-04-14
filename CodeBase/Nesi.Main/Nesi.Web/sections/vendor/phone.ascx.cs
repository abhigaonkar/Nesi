using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using System.Text;
using nesi.core;

public partial class sections_vendor_modules_phone : System.Web.UI.UserControl
	{
	NeMember current_user;
	public int address_id  
		{ 
		get {var _address_id = 0; if(ViewState["address_id"] == null){return _address_id;}else{int.TryParse(ViewState["address_id"].ToString(), out _address_id); return _address_id; }} 
		set {ViewState["address_id"] = value.ToString();}
		}
	public void Initialize()
		{
			hdnaddressid.Value = address_id.ToString();
			ds_phones.DataBind();
		}
	protected void Page_Init(object sender, EventArgs e)
		{
		var _tools		= new Toolbox();
		current_user		= Toolbox.do_handle_authentication(1);
		gv_phones.Enabled							= current_user.AuthenticatedForPrivilege(115);
		}
	protected void Page_PreRender(object sender, EventArgs e)
	{
		hdnaddressid.Value = address_id.ToString();
			ds_phones.DataBind();
	}
	protected void btn_save_Click(object sender, EventArgs e)
		{
		var ddl_type			= (DropDownList) gv_phones.FindEditFormTemplateControl("ddl_type");
		var tb_number				= (TextBox) gv_phones.FindEditFormTemplateControl("tb_number");
		var error_report		= (HtmlTableCell) gv_phones.FindEditFormTemplateControl("error_report");
		var phone_numbers_id			= 0;
		var is_new						= gv_phones.IsNewRowEditing;
		if(!is_new)
			{
			phone_numbers_id			= Convert.ToInt32(gv_phones.GetDataRow(gv_phones.EditingRowVisibleIndex)["id"]);
			}
		error_report.InnerHtml			= "";
		var errors				= new List<string>();
		var is_error					= false;
		#region Number formatting / Checking
		var phone_number				= Toolbox.regex_only_numbers().Replace(tb_number.Text, "");
		#region Basic check
		if (phone_number.Length != 10)
			{
			is_error					= true;
			errors.Add("Not a valid phone number, it needs to have 10 numbers");
			tb_number.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			phone_number				= phone_number.Substring(0,3)+" "+phone_number.Substring(3,3)+" "+phone_number.Substring(6,4);
			tb_number.Text				= phone_number;
			tb_number.Style["border"]	= "solid 1px #ccc";
			}
		#endregion Basic check
		#region Communications Type Check
		if (ddl_type.SelectedValue == "")
			{
			is_error					= true;
			errors.Add("Please select a communications type");
			ddl_type.Style["border"]	= "solid 1px #f00";
			}
		else
			{
			ddl_type.Style["border"]	= "solid 1px #ccc";
			}
		#endregion Communications Type Check
		#endregion Number formatting / Checking
		if (is_error)
			{
			// Required elements have not been met.. not proceeding, deliver the error report.
			var sb			= new StringBuilder();
			sb.Append("<div class='title'>There are errors with your submission</div>");
			foreach(var err in errors)
				{
				sb.AppendFormat("<div class='err'>&bullet; {0}</div>",err);
				}
			error_report.InnerHtml		= sb.ToString();
			}
		else
			{
			NePhoneNumbers p;
			if(is_new)
				{
				p						= new NePhoneNumbers();
				p.table_id				= address_id;
				p.type					= "Address";
				p.comm_type				= ddl_type.SelectedValue;
				p.number				= phone_number;
				p.is_default			= false;
				p.is_active				= true;
				p.Save();
				}
			else
				{
				p						= new NePhoneNumbers(phone_numbers_id);
				p.number				= phone_number;
				p.comm_type				= ddl_type.SelectedValue;
				p.Save();
				}
			gv_phones.CancelEdit();
			gv_phones.DataBind();
			}
		}
	protected void btn_cancel_Click(object sender, EventArgs e)
		{
		gv_phones.CancelEdit();
		}
	protected void gv_phones_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
	{
		e.Visible = current_user.AuthenticatedForPrivilege(115);
	}
}