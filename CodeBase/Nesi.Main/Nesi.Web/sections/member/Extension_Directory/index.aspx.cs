using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using DevExpress.Web;
using nesi.core;

public partial class Extension_Directory : System.Web.UI.Page
	{
    NeMember current_user;
   
    protected void Page_Init(object sender, EventArgs e)
    {
        var _q = Request.QueryString;
        var _tools = new Toolbox();

        current_user = Toolbox.do_handle_authentication(1);
		ds_extensions.SelectParameters[0].DefaultValue	= current_user.id.ToString();
    }





	protected void btnSave_Click(object sender, EventArgs e)
		{
		Toolbox.doSQL_void(@"DELETE FROM quick_extensions WHERE quick_extensions_mymember_id =@v0 " , current_user.id);
		var values				= new List<string>();
		for(var i = 0;i < gv_extensions.VisibleRowCount;i++)
			{
			var cb				= (ASPxCheckBox) gv_extensions.FindRowCellTemplateControl(i, (GridViewDataColumn) gv_extensions.Columns["_selected"], "cb");
			var member_id				= gv_extensions.GetRowValues(i, "member_id");
			if(cb.Checked)
				{
				values.Add(string.Format("({0},{1})", current_user.id, member_id));
				}
			}
		if(values.Count > 0)
			{
			    //TODO: LL FIXING SQL Security
            var values_output			= values.Aggregate((a, x) => a + ", " + x);
			Toolbox.doSQL_void(@"INSERT INTO quick_extensions (quick_extensions_mymember_id,quick_extensions_member_id) VALUES "+values_output);
			gv_extensions.DataBind();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "closer", @"window.opener.location.href = window.opener.location.href;window.close();", true);
			error_msg.Text			= "";
			}
		else
			{
			error_msg.Text			= "Selection cleared.";
			}
		}


    protected void fillgv()
    {
        //Toolbox _tools = new Toolbox();
        //string sql = @"";

        //gv_extensions.DataSource = _tools.getSQL_datatable(@sql  , null);
        //gv_extensions.DataBind();
    }

    protected void ASPxGridView1_DataBound(object sender, EventArgs e)
    {
		/*
        this.ASPxGridView1.Selection.UnselectAll();
        for (int i = 0; i < this.ASPxGridView1.VisibleRowCount; i++)
            if (this.ASPxGridView1.GetRowValues(i, "_Selected") != null)
                if (Convert.ToInt32(this.ASPxGridView1.GetRowValues(i, "_Selected")) == 1)
                    this.ASPxGridView1.Selection.SelectRow(i);
		 */
    }
	protected void ASPxGridView1_HtmlCommandCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableCommandCellEventArgs e)
		{

		}
	protected void ASPxGridView1_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
		{
		}
}
