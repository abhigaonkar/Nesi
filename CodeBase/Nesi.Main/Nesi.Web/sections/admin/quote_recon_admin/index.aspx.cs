using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_admin_quote_recon_admin_index : Page
	{
	Toolbox _tools;
	protected void Page_Load(object sender, EventArgs e)
		{
		var _q			= Request.QueryString;
		_tools					= new Toolbox();
		var current_user			= Toolbox.do_handle_authentication(165);

		var menu = new NeMenu(current_user, Convert.ToInt32(165));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(current_user);
		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		lbltemp.Text = "User Map Interface";
		
		}

	


	
	protected void ASPxMemo1_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxMemo;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.TextChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('q|{0}|' + s.GetText()); }}", container.KeyValue);

	}
	protected void ASPxComboBox2_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxComboBox;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.SelectedIndexChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('t|{0}|' + s.GetText()); }}", container.KeyValue);

	}
	protected void ASPxSpinEdit2_Init(object sender, EventArgs e)
	{
		var ddl = sender as ASPxSpinEdit;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.ValueChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('y|{0}|' + s.GetValue()); }}", container.KeyValue);

	}
	protected void ASPxSpinEdit2_Init1(object sender, EventArgs e)
	{
		var ddl = sender as ASPxSpinEdit;
		var container = ddl.NamingContainer as GridViewDataItemTemplateContainer;
		ddl.ClientSideEvents.ValueChanged = string.Format("function (s, e) {{ cb_chkprivate.PerformCallback('n|{0}|' + s.GetValue()); }}", container.KeyValue);

	}
	protected void gvrecon_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
	{
		if (e.Parameters.Length > 1)
		{
			if (e.Parameters[0].ToString() == "a")
			{
				var mem = e.Parameters.Split('|').GetValue(1).ToString();
				var type = e.Parameters.Split('|').GetValue(2).ToString();
				var ifyes = e.Parameters.Split('|').GetValue(3).ToString();
				var ifno= e.Parameters.Split('|').GetValue(4).ToString();
				_tools.getSQL_void(@"Insert into quote_process_questions (question,type,status,ifyes,ifno) 
values(@v0,@v1,@v2,@v3)", new object[] { mem ,type ,"Active", ifyes,ifno});

			}

		}

	}
	protected void gvrecon_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
	{

	}
	protected void cb_chkprivate_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		if (e.Parameter.Length > 1)
		{
			var id = e.Parameter.Split('|').GetValue(1).ToString();
			var value = e.Parameter.Split('|').GetValue(2).ToString();

			if (e.Parameter[0].ToString() == "q")
			{
				_tools.getSQL_void("update quote_process_questions set question=@v0 where id =@v1 ", new object[] { value, id});
			}
			else if (e.Parameter[0].ToString() == "t")
			{
				_tools.getSQL_void("update quote_process_questions set type=@v0 where id =@v1 ", new object[] { value, id });
			}
			else if (e.Parameter[0].ToString() == "y")
			{
				_tools.getSQL_void("update quote_process_questions set ifyes=@v0 where id =@v1 ", new object[] { value, id });
			}
			else if (e.Parameter[0].ToString() == "n")
			{
				_tools.getSQL_void("update quote_process_questions set ifno=@v0 where id = @v1 ", new object[] { value, id });
			}
		}
	}
}
