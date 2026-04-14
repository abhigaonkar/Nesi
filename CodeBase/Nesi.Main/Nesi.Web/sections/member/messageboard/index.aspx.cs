using System;
using DevExpress.Web;
using nesi.core;

public partial class messageboard : System.Web.UI.Page
{
    NeMember myMember;
    Toolbox _Tools = new Toolbox();
    int _page_id = 91;
    protected void Page_Load(object sender, EventArgs e)
        {

        SqlDataSource1.SelectCommand =
            "select 0 as id,'All Business Units' as name union SELECT ID, name FROM business_unit where id in (" +
            new Current_User().visible_business_units + ")";
        sqlcompanys.SelectCommand = SqlDataSource1.SelectCommand;
        SqlDataSource1.DataBind();
        sqlcompanys.DataBind();

        ASPxFileManager1.Settings.RootFolder = Toolbox.app_setting("UNC_messageboard_images");
        ASPxFileManager1.Settings.ThumbnailFolder = Toolbox.app_setting("UNC_messageboard_images");

       
        myMember = Toolbox.do_handle_authentication(_page_id);

        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		hdncompanyid.Value = myMember.business_unit_id.ToString();
        if (!IsPostBack)
        {
			ddlcompany.Value = myMember.business_unit_id;

        }
	//	 if (myMember.AuthenticatedForPrivilege(111))
	//	   {
	//		   ASPxPageControl1.TabPages[1].ClientVisible=true;
	//	   }
	//	   else
	//	   {
	//		   ASPxPageControl1.TabPages[1].ClientVisible=false;
	//	   }
		 if (ASPxPageControl1.ActiveTabPage.Index == 0)
		 {
			 fill_grid();
		 }
		 else if (ASPxPageControl1.ActiveTabPage.Index == 1)
		 {

		 }
		 else if (ASPxPageControl1.ActiveTabPage.Index == 2)
		 {

		 }


    }
    protected void fill_grid()
    {
            var strsql = "Select * from vw_digital_signage order by digitalsignage_id desc";
            ASPxGridView1.DataSource = _Tools.getSQL_datatable(strsql,null);
            ASPxGridView1.DataBind();
            ASPxGridView1.FilterEnabled = true;
            ASPxGridView1.FilterExpression = !IsPostBack && !IsCallback ? string.Format("[business_unit_id]=" + myMember.business_unit_id + " or [business_unit_id]=0") : ASPxGridView1.FilterExpression;
    }

    
    protected void ASPxGridView1_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        



        var strsql = "Insert into digitalsignage (digitalsignage_text,digitalsignageStartDate,digitalsignageEndDate,digitalsignageRecurringeveryYear," +
						"digitalsignage_Active,digitalsignage_DateofEvent,business_unit_id,digitalsignage_eventTitle,digitalsignage_auditMemberid) values " +
                        "('" + _Tools.value_to(e.NewValues["DigitalSignage_Text"]) + "'," +
                        "'" + Convert.ToDateTime(e.NewValues["DigitalSignageStartDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                        "'" + Convert.ToDateTime(e.NewValues["DigitalSignageEndDate"]).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                         Convert.ToInt32(e.NewValues["DigitalSignageRecurringEveryYear"]) + "," +
                         Convert.ToInt32(e.NewValues["DigitalSignage_Active"]) + "," +
                        "'" + Convert.ToDateTime(e.NewValues["DigitalSignage_DateofEvent"]).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                         Convert.ToInt32(e.NewValues["business_unit_id"]) + "," +
                        "'" + _Tools.value_to(e.NewValues["DigitalSignage_EventTitle"]) + "'," +
                         +myMember.id + ")";
                         
        _Tools.getSQL_void(@"Insert into digitalsignage (digitalsignage_text,digitalsignageStartDate,digitalsignageEndDate,digitalsignageRecurringeveryYear, digitalsignage_Active,digitalsignage_DateofEvent,business_unit_id,digitalsignage_eventTitle,digitalsignage_auditMemberid)  values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8)",new object[] { _Tools.value_to(e.NewValues["DigitalSignage_Text"]),Convert.ToDateTime(e.NewValues["DigitalSignageStartDate"]).ToString("yyyy-MM-dd HH:mm:ss"),Convert.ToDateTime(e.NewValues["DigitalSignageEndDate"]).ToString("yyyy-MM-dd HH:mm:ss"),Convert.ToInt32(e.NewValues["DigitalSignageRecurringEveryYear"]),Convert.ToInt32(e.NewValues["DigitalSignage_Active"]),Convert.ToDateTime(e.NewValues["DigitalSignage_DateofEvent"]).ToString("yyyy-MM-dd HH:mm:ss"),Convert.ToInt32(e.NewValues["business_unit_id"]),_Tools.value_to(e.NewValues["DigitalSignage_EventTitle"]),myMember.id } );
        e.Cancel = true;
        ASPxGridView1.CancelEdit();

        fill_grid();

    }
    protected void ASPxGridView1_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        int edittingkey = Convert.ToInt16(e.Keys[0]);
        var strsql = "Delete from digitalsignage where digitalSignage_id = " + edittingkey;
        _Tools.getSQL_void(@"Delete from digitalsignage  where digitalSignage_id =@v0", new object[] { edittingkey });
        e.Cancel = true;
        ASPxGridView1.CancelEdit();
        fill_grid();
    }
    protected void ASPxGridView1_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        int edittingkey = Convert.ToInt16(e.Keys[0]);
        var strsql = "Update digitalsignage set digitalsignage_text = '" + _Tools.value_to(e.NewValues["DigitalSignage_Text"]) 
						+ "',digitalsignageStartDate = '" + Convert.ToDateTime(e.NewValues["DigitalSignageStartDate"]).ToString("yyyy-MM-dd HH:mm:ss") 
						+ "',digitalsignageEndDate = '" + Convert.ToDateTime(e.NewValues["DigitalSignageEndDate"]).ToString("yyyy-MM-dd HH:mm:ss") 
						+ "',digitalsignageRecurringeveryYear = " + Convert.ToInt32(e.NewValues["DigitalSignageRecurringEveryYear"]) 
						+ ",digitalsignage_Active = " + Convert.ToInt32(e.NewValues["DigitalSignage_Active"]) 
						+ ",digitalsignage_DateofEvent = '" + Convert.ToDateTime(e.NewValues["DigitalSignage_DateofEvent"]).ToString("yyyy-MM-dd HH:mm:ss") 
						+ "',business_unit_id = " + Convert.ToInt32(e.NewValues["business_unit_id"]) 
						+ ",digitalsignage_eventTitle = '" + _Tools.value_to(e.NewValues["DigitalSignage_EventTitle"]) 
						+ "',digitalsignage_auditMemberid = " + myMember.id + " where digitalsignage_id = " + edittingkey;

        _Tools.getSQL_void(@"Update digitalsignage  set digitalsignage_text =@v0,digitalsignageStartDate =@v1 ,digitalsignageEndDate =@v2 ,digitalsignageRecurringeveryYear =@v3 ,digitalsignage_Active =@v4 ,digitalsignage_DateofEvent =@v5 ,business_unit_id =@v6 ,digitalsignage_eventTitle =@v7 ,digitalsignage_auditMemberid =@v8   where digitalsignage_id =@v9", new object[] { _Tools.value_to(e.NewValues["DigitalSignage_Text"]),Convert.ToDateTime(e.NewValues["DigitalSignageStartDate"]).ToString("yyyy-MM-dd HH:mm:ss"),Convert.ToDateTime(e.NewValues["DigitalSignageEndDate"]).ToString("yyyy-MM-dd HH:mm:ss"),Convert.ToInt32(e.NewValues["DigitalSignageRecurringEveryYear"]),Convert.ToInt32(e.NewValues["DigitalSignage_Active"]),Convert.ToDateTime(e.NewValues["DigitalSignage_DateofEvent"]).ToString("yyyy-MM-dd HH:mm:ss"),Convert.ToInt32(e.NewValues["business_unit_id"]),_Tools.value_to(e.NewValues["DigitalSignage_EventTitle"]),myMember.id,edittingkey });
        e.Cancel = true;
        ASPxGridView1.CancelEdit();

        fill_grid();
    }


	protected void cb_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		var tb = (ASPxTextBox)gv_chat.FindTitleTemplateControl("txtaddnote");
		if (e.Parameter == "add_note")
		{
			_Tools.getSQL_void(@"Insert into messageboard_chat (messageboard_memberid,messageboard_date,messageboard_text,business_unit_id)  values (@v0,curdate(),@v1,@v2)",new object[] { myMember.id,_Tools.value_to(tb.Text),ddlcompany.Value } );
			gv_chat.DataBind();
		}
	}
	protected void gv_chat_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
	{
		try
		{
			_Tools.getSQL_void(@"Update messageboard_chat  set messageboard_memberid=@v0,messageboard_text=@v1   where messageboard_chat.messageboard_chat_id =@v2", new object[] { myMember.id,_Tools.value_to(e.NewValues["text"]),e.Keys[0] });
		}
		catch
		{

		}
		e.Cancel = true;
		gv_chat.CancelEdit ();
	}
	protected void gv_chat_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
	{
		try
		{
			_Tools.getSQL_void(@"delete from messageboard_chat  where messageboard_chat.messageboard_chat_id =@v0", new object[] { e.Keys[0] });
		}
		catch
		{

		}
		e.Cancel = true;
		gv_chat.CancelEdit();
	}
}
