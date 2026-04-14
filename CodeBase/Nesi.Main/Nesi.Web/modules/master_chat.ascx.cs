using System;
using System.Data;
using System.Text;
using nesi.core;

public partial class modules_master_chat : System.Web.UI.UserControl
{
	public NeMember current_user  { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
		
		if (current_user != null && current_user.isContact)
			{
			cb_chat.Visible			= false;
			}
		else
			{
			fill_chat();
			}
    }
	protected void cb_chat_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
		{
		if (e.Parameter != null)
			{
			try
				{
				if (d_txtaddchat.Text != "")
					{
					Toolbox.doSQL_void(@"insert into messageboard_chat (messageboard_memberid,messageboard_date,business_unit_id,messageboard_text) 
values(@v0,curdate(),@v1,@v2)",
						new object[] {

							current_user.id,
							ASPxComboBox1.Value,
							d_txtaddchat.Text

						}
);
					d_txtaddchat.Text = "";

					}
				}
			catch { }
			}
		fill_chat();
		}
	protected void fill_chat()
		{
			if (current_user != null && !current_user.isContact)
				{
				ASPxComboBox1.DataSource = Toolbox.doSQL_dt(@"Select id, name from business_unit where find_in_set( id, '" + new Current_User().visible_business_units + "' )"  , null);
				ASPxComboBox1.DataBind();
				if(ASPxComboBox1.Value == null)
					{
					ASPxComboBox1.Value = current_user.business_unit_id;
					}
				d_mem_chat.Text = "";
				var sbChat = new StringBuilder();
				d_mem_chat.ToolTip = d_mem_chat.Text;
				var dt = Toolbox.doSQL_dt(@" SELECT a.messageboard_date, b.member_fullname name, a.messageboard_text FROM messageboard_chat a LEFT JOIN member b ON a.messageboard_memberid = b.member_id WHERE a.business_unit_id = @v0  and a.messageboard_date > DATE_SUB(CURDATE(), INTERVAL 6 MONTH) ORDER BY a.messageboard_chat_id DESC LIMIT 10", new object[] {  ASPxComboBox1.Value } );
				if(dt.Rows.Count > 0)
					{
				foreach (DataRow dr in dt.Rows)
					{
					sbChat.Append(Convert.ToDateTime(dr["messageboard_date"]).ToString("yyyy-MM-dd") + "  " + dr["name"] + "\n");
					sbChat.AppendFormat("{0}\n\n", dr["messageboard_text"]);
					}
					d_mem_chat.Text	= sbChat.ToString();
					d_mem_chat.ToolTip = d_mem_chat.Text;
					}
				}
			else
				{
				d_mem_chat.Visible = false;
				d_txtaddchat.Visible = false;
				}
		}

}