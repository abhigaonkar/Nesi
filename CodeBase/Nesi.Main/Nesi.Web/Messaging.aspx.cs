using System;
using System.Data;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using nesi.core;

public partial class Messaging : System.Web.UI.Page
	{

	NeMember myMember;
	private const int _page_id = 29; // from Page table in DB
	//private const string _page_description = 
	protected void Page_Load(object sender, EventArgs e)
		{

		//This page should never run. Alert jordan
		Toolbox.RedirectToN2(Response, Request, _page_id);

        var _tools = new Toolbox();
		myMember = Toolbox.do_handle_authentication(_page_id);
		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);

		var lbltemp = (Label)Page.Master.FindControl("lblHeading");
		if (menu.PageDescription != null)
			{
			lbltemp.Text = menu.PageDescription;
			}
		//        lbltemp.Text = _page_description;

		if (!IsPostBack)
		    {

		    drpUsers.DataSource = _tools.getSQL_datatable(string.Format(@"SELECT

			    c.member_id,
			    CONCAT(c.member_fullname, ' - ', f.name) member_fullname
			        FROM

			    page a
			    INNER JOIN

			    memberpageprivilege b
			    INNER JOIN

			    member c ON b.memberpageprivilege_member_id = c.Member_ID
			    INNER JOIN

			    memberpage d ON d.memberpage_member_id = c.member_id AND d.memberpage_page_id = a.page_id
			    INNER JOIN

			    privilege e ON b.memberpageprivilege_privilege_id = e.privilege_id AND e.privilege_page_id = a.page_id
			    INNER join

			    business_unit f ON c.business_unit_id = f.id and f.id in ({0})
			    WHERE

			    c.member_status = 'Active' AND

			    a.page_id = '29' AND

			    e.privilege_id = '24'
			    ORDER BY

			    f.id ASC,
			        c.member_nickname ASC,
			        c.member_lastname ASC", new Current_User().visible_business_units),null);


            drpMessageType.DataSource = LoadMessageType();
			drpMessageType.DataBind();
			var lic = new ListItem("All Messages", "0");
			drpMessageType.Items.Insert(0, lic);

			MultiView1.ActiveViewIndex = 1;

			MessageTab(1);
			Session["meesage_reply_id"] = null;
			//MultiView1.ActiveViewIndex = 0;
			//Menu1.Items[1].ImageUrl = "/images/messaging/button/button[summary][selected].gif";
			//MessageTab(0);




			}
		    drpUsers.DataBind();
        //btnCC.Attributes.Add("onClick", "return openwindow(" + myMember.id + ");");

    }
	public DataTable LoadMessageType()
		{
		return Toolbox.doSQL_dt(@"SELECT messagetype_id, messagetype FROM messagetype"  , null);
		}
	public DataTable MessageSummary(int _mem_id, int _messagestype, int selecttab)
		{
		var str_messages = "";
		switch (selecttab)
			{
			case 1://Received Messages
				str_messages = "SELECT MTo.MessageTo_ID, MTo.MessageTo_Message_ID, MTo.MessageTo_Member_ID_To, member_name(mem.member_id) fullname, M.Message_ID, M.MessageType_ID, M.Message_LeftBy_Member_ID, M.Date as Date, M.Message_Subject as Subject, MType.MessageType as MessType , MTo.Message_Status as Status, Concat(Mem.Member_FirstName,\' \',Mem.Member_LastName) as ToFrom  FROM messageto MTo Inner join Message M on M.Message_ID = MTo.MessageTo_Message_ID Inner Join Member Mem on Mem.Member_ID = M.Message_LeftBy_Member_ID Inner Join MessageType MType on MType.MessageType_ID =  M.MessageType_ID";
				str_messages = _messagestype == 0 
								? string.Format("{0} WHERE MTo.MessageTo_Member_ID_To = {1} AND MTo.Message_Status != 'Deleted' ORDER BY MessageTo_ID DESC", str_messages, _mem_id) 
								: string.Format("{0} WHERE M.MessageType_ID = {1} AND MTo.MessageTo_Member_ID_To = {2} && MTo.Message_Status != 'Deleted' ORDER BY MessageTo_ID DESC", str_messages, _messagestype, _mem_id);
			break;
			case 2: //Sent Messages
				str_messages = "SELECT MTo.MessageTo_ID, MTo.MessageTo_Message_ID, MTo.MessageTo_Member_ID_To, M.Message_ID, member_name(mem.member_id) fullname, M.MessageType_ID, M.Message_LeftBy_Member_ID, M.Date as Date, M.Message_Subject as Subject, MType.MessageType as MessType , IF(Trim(MTo.Message_Status) = \'Deleted\' , \'Read\', MTo.Message_Status) Status, Concat(Mem.Member_FirstName,\' \',Mem.Member_LastName) as ToFrom  FROM messageto MTo Inner join Message M on M.Message_ID = MTo.MessageTo_Message_ID Inner Join Member Mem on Mem.Member_ID = MTo.MessageTo_Member_ID_To  Inner Join MessageType MType on MType.MessageType_ID =  M.MessageType_ID";
				str_messages = _messagestype == 0 
									? string.Format("{0} WHERE M.Message_LeftBy_Member_ID = {1} && M.Message_Status != 'Deleted' ORDER BY MessageTo_ID DESC", str_messages, _mem_id) 
									: string.Format("{0} WHERE M.MessageType_ID = {1} AND M.Message_LeftBy_Member_ID = {2} && M.Message_Status != 'Deleted' ORDER BY MessageTo_ID DESC", str_messages, _messagestype, _mem_id);
			break;
			}
		return Toolbox.doSQL_dt(str_messages  , null);
		}
	protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
		{
		//MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
		//MultiView1.ActiveViewIndex = 0;
		//int i = 0;
		//Make the selected menu item reflect the correct imageurl
		//for (i = 0; i <= Menu1.Items.Count - 1; i++)
		//{
			pnlreply.Visible = false;
		int menu_id;
		menu_id = (int.Parse(e.Item.Value)); /// Which Menu is selected
		ViewState["menuid"] = Convert.ToString(menu_id);
		if (int.Parse(e.Item.Value) == 0)
			{
			MultiView1.ActiveViewIndex = 0;

			MessageTab(int.Parse(e.Item.Value));


			}
		else
			{

			}
		if (int.Parse(e.Item.Value) == 1)
			{
			MultiView1.ActiveViewIndex = 1;

			MessageTab(int.Parse(e.Item.Value));

			}
		else
			{


			}
		if (int.Parse(e.Item.Value) == 2)
			{
			MultiView1.ActiveViewIndex = 1;

			MessageTab(int.Parse(e.Item.Value));

			}
		else
			{

			}
		if (int.Parse(e.Item.Value) == 3)
			{
			MultiView1.ActiveViewIndex = 1;

			MessageTab(int.Parse(e.Item.Value));

			}
		else
			{


			}
		}

	private void MessageTab(int selectedTab)
		{
		NeMember mymember;
		mymember = new NeMember(Session["session"].ToString());

		pnlMessageDetails.Visible = false;
		pnlMessageGrid.Visible = true;
		drpMessageType.AutoPostBack = true;


		if (selectedTab != 0)
			{
			var MessageList = new NeMessaging();

			switch (selectedTab)
				{
				case 1:   // Inbox
				ViewState["menuid"] = "1";
				MessageGrid.DataSource = MessageSummary(mymember.id, Convert.ToInt16(drpMessageType.SelectedValue), 1);
				MessageGrid.DataBind();
				drpMessageType.SelectedIndex = 0;
				lblToFrom.Text = "From";
				btnCancel.Text = "Close";
				
				btnCC.Visible = false;
				txtCCUsers.Visible = true;
				btnSend.Visible = false;
				errorlabel.Text = "";
				GridviewSummary.Visible = false;
				break;
				case 2:   //Outbox
				ViewState["menuid"] = "2";
				MessageGrid.DataSource = MessageSummary(mymember.id, Convert.ToInt16(drpMessageType.SelectedValue), 2);
				MessageGrid.DataBind();
				drpMessageType.SelectedIndex = 0;
				lblToFrom.Text = "To";
				btnCancel.Text = "Close";
				btnCC.Visible = false;
				txtCCUsers.Visible = true;
				btnSend.Visible = false;
				errorlabel.Text = "";
				GridviewSummary.Visible = false;
				break;
				case 3:    // New message
				drpMessageType.AutoPostBack = false;
				ViewState["menuid"] = "3";

				txtCCUsers.Text = "";
				txtMessage.Text = "";
				txtSubject.Text = "";

				drpUsers.Visible = true;
				errorlabel.Text = "";
				
				var lic5 = new ListItem("Select User", "0");
				drpUsers.Items.Insert(0, lic5);
				drpMessageType.SelectedIndex = 2;
				pnlMessageGrid.Visible = false;
				pnlMessageDetails.Visible = true;
				btnCC.Visible = true;
				txtCCUsers.Visible = true;
				lblToFrom.Text = "To";
				btnCancel.Text = "Cancel";
				btnSend.Visible = true;
				txtDate.ReadOnly = true;
				txtDate.Text = DateTime.Now.ToString();
				GridviewSummary.Visible = false;
				break;
				}
			}
		else
			{
			errorlabel.Text = "";
			var NewTotalMessages = new NeMessaging();
			drpMessageType.SelectedIndex = 0;
			/*
							OdbcConnection conn = NeDB.getCon();
							string strMessages = "SELECT messagetype.MessageType, messageto.Message_Status, Count(messageto.Message_Status) AS CountOfMessage_Status " +
												 "FROM (message RIGHT JOIN messagetype ON message.MessageType_ID = messagetype.MessageType_ID) LEFT JOIN messageto ON message.Message_ID = messageto.MessageTo_ID " +
												 "GROUP BY messagetype.MessageType, messageto.Message_Status, messageto.MessageTo_Member_ID_To " +
												 "HAVING (((messageto.MessageTo_Member_ID_To)=" + mymember.id + ")) ORDER BY messagetype.MessageType";


							OdbcCommand comMessages = new OdbcCommand(strMessages, conn);
							OdbcDataReader drMessages = comMessages.ExecuteReader();
							if (drMessages.HasRows)
							{
								GridviewSummary.DataSource = drMessages;
								GridviewSummary.DataBind();
								GridviewSummary.Visible = true;
							}
							conn.Close();                    
			*/
			using (var conn = Toolbox.connect())
				{
				lblPersonalNo.Text = (NewTotalMessages.TotalNewmessages(conn, 1, mymember.id)).ToString();
			    lblAdminNo.Text = (NewTotalMessages.TotalNewmessages(conn,2, mymember.id)).ToString();
			    lblSystemNo.Text = (NewTotalMessages.TotalNewmessages(conn,3, mymember.id)).ToString();
			        
			    lblTotalNew.Text = Convert.ToString(Convert.ToDouble(lblPersonalNo.Text) + Convert.ToDouble(lblAdminNo.Text) + Convert.ToDouble(lblSystemNo.Text));
				}

			}
		if (selectedTab == 1 || selectedTab == 2)
			{
			txtDate.ReadOnly = true;
			txtMessage.ReadOnly = true;
			txtSubject.ReadOnly = true;
			txtCCUsers.ReadOnly = true;
			}
		else
			{
			txtMessage.ReadOnly = false;
			txtSubject.ReadOnly = false;
			}
		}

	protected void MessageGrid_SelectedIndexChanged(object sender, EventArgs e)
		{
		pnlMessageDetails.Visible = true;
		drpUsers.Visible = false;

		var ChangeStatus = new NeMessaging();
		if (ViewState["menuid"].ToString() == "1")
			{
			ChangeStatus.UpdateStatus(Convert.ToInt32(MessageGrid.DataKeys[MessageGrid.SelectedIndex].Value));
			BindMessageGrid();
			}

		var GetMessageDetails = new NeMessaging(Convert.ToInt32(MessageGrid.DataKeys[MessageGrid.SelectedIndex].Value));
		Session["meesage_reply_id"] = Convert.ToInt32(MessageGrid.DataKeys[MessageGrid.SelectedIndex].Value);

		txtDate.Text = (MessageGrid.Rows[MessageGrid.SelectedIndex].Cells[0]).Text;
		txtCCUsers.Text = (MessageGrid.Rows[MessageGrid.SelectedIndex].Cells[1]).Text;
		txtSubject.Text = (MessageGrid.Rows[MessageGrid.SelectedIndex].Cells[2]).Text;
		txtMessage.Text = GetMessageDetails.Message_Body;
		btnreply.Visible = true;
		}
	protected void drpMessageType_SelectedIndexChanged(object sender, EventArgs e)
		{
		if (ViewState["menuid"].ToString() != "3")
			{
			var MessageList = new NeMessaging();
			NeMember mymember;
			mymember = new NeMember(Session["session"].ToString());
			MessageGrid.DataSource = MessageSummary(mymember.id, Convert.ToInt16(drpMessageType.SelectedValue), Convert.ToInt16(ViewState["menuid"].ToString()));
			MessageGrid.DataBind();
			}
		}
	protected void MessageGrid_RowDataBound(object sender, GridViewRowEventArgs e)
		{
		int menu_id;
		menu_id = Convert.ToInt32(ViewState["menuid"]);

		try
			{
			if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
				{
				if (e.Row.RowType == DataControlRowType.Header)
					{
					switch (menu_id)
						{
						case 1:
						e.Row.Cells[1].Text = "From";
						break;
						case 2:
						e.Row.Cells[1].Text = "To";
						break;
						}
					}
				if (e.Row.Cells[5].Visible && e.Row.RowType == DataControlRowType.DataRow)
					{
					var delButton = (Button)e.Row.Cells[5].Controls[2];
					delButton.OnClientClick = string.Format("return confirm('Are you certain you want to delete the selected entry?');");
					}
				}
			else if (e.Row.RowType == DataControlRowType.Footer)
				{
				}
			}
		catch { }
		}
	protected void btnCancel_Click(object sender, EventArgs e)
		{
		txtMessage.Text = "";
		txtSubject.Text = "";
		txtCCUsers.Text = "";
		if (ViewState["menuid"].ToString() != "3")
			{
			pnlMessageDetails.Visible = false;
			}
		}
	protected void btnSend_Click(object sender, EventArgs e)
		{
		var NewMessage = new NeMessaging();
		var messagecreated = true;
		var error_text = "unknown";
		try
			{

			NeMember mymember;
			mymember = new NeMember(Session["session"].ToString());
			NewMessage.Message_LeftBy_Member_ID = mymember.id;
			NewMessage.MessageType_ID = Convert.ToInt32(drpMessageType.SelectedValue);
			NewMessage.Message_Subject = txtSubject.Text;
			NewMessage.Message_Body = txtMessage.Text;
			NewMessage.Date = System.DateTime.Now;
			NewMessage.AddNewMessage();

			}
		catch (Exception ex)
			{
			error_text = ex.Message;
			messagecreated = false;
			}
		finally
			{
			if (messagecreated)
				{
				int messageidnew;
				var chkIDs = "";
				var strTotalError = "";
				var strTotalSuccess = "";
				messageidnew = NewMessage.Message_ID;

				if (Session["AllMessageIds"] != null)
					{
					chkIDs = Session["AllMessageIds"].ToString();
					chkIDs = chkIDs + "," + drpUsers.SelectedValue;
					}
				else
					{
					chkIDs = drpUsers.SelectedValue;
					}
				var arrUserID = chkIDs.Split(',');
				var Count = 0;


				for (Count = 0; Count < arrUserID.Length; Count++)
					{
					var UID = arrUserID[Count];
					var NewMessageTo = new NeMessaging();
					NewMessageTo.MessageTo_Message_ID = messageidnew;
					NewMessageTo.MessageTo_Member_ID_To = Convert.ToInt32(UID);
					try
						{
						var Curmember = new NeMember(Convert.ToInt32(UID));
						NewMessageTo.CreateMessageTo();
						strTotalSuccess = strTotalSuccess + Curmember.Username + ", ";
						}
					catch (Exception ee)
						{
						NeMember Curmember;
						Curmember = new NeMember(NewMessageTo.MessageTo_Member_ID_To.ToString());
						strTotalError = strTotalError + Curmember.Username + ", ";
						}

					}
				var messageresult = "";

				if (strTotalError != "")
					{
					strTotalError = strTotalError.Substring(0, strTotalError.Length - 1);
					messageresult = "The message failed to be sent to the following Users " + strTotalError + ". Please Try again";
					}
				else
					{

					}
				if (strTotalSuccess == "")
					{
					errorlabel.Text = messageresult;
					Session["AllMessageIds"] = null;
					}
				else
					{
					strTotalSuccess = strTotalSuccess.Substring(0, strTotalSuccess.Length - 1);
					errorlabel.Text = "The Message was sent successfully to " + strTotalSuccess + " ." + messageresult;
					Session["AllMessageIds"] = null;
					}

				}
			else
				errorlabel.Text = "Message Not sent " + error_text;

			}

		}
	protected void MessageGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
		pnlMessageDetails.Visible = false;
		var mymember = new NeMember();
		mymember = new NeMember(Session["session"].ToString());
		var MemberID = mymember.id;

		try
			{
			errorlabel.Text = "";
			var DelMessage = new NeMessaging();
			/*          int menuid;
            
					   menuid = Convert.ToInt16(ViewState["menuid"].ToString());
					   switch (menuid)
					   {
						   case 1:
							   DelMessage.DeleteMessage(Convert.ToInt32(MessageGrid.DataKeys[e.RowIndex].Value), Convert.ToInt16(ViewState["menuid"].ToString()));            
							   break;
						   case 2:
			 */
			var GetMessage = new NeMessaging(Convert.ToInt32(MessageGrid.DataKeys[e.RowIndex].Value));
			DelMessage.DeleteMessage(GetMessage.Message_ID, Convert.ToInt16(ViewState["menuid"].ToString()), MemberID);
			/*                 break;
					 }

			 */


			}
		catch (Exception ex)
			{
			errorlabel.Text = "The Following Error Occured. " + ex.Message + ". Please Check with Administrator.";
			}
		BindMessageGrid();
		}
	protected void BindMessageGrid()
		{
		var MessageList = new NeMessaging();
		NeMember mymember;
		mymember = new NeMember(Session["session"].ToString());
		MessageGrid.DataSource = MessageSummary(mymember.id, Convert.ToInt16(drpMessageType.SelectedValue), Convert.ToInt16(ViewState["menuid"].ToString()));
		MessageGrid.DataBind();
		}

	protected void MessageGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
		MessageGrid.PageIndex = e.NewPageIndex;

		BindMessageGrid();

		}



	protected void btnCC_Click(object sender, EventArgs e)
		{
		//NeMember myMember = new NeMember(myMember.id);
		var mlist = new NeMember();
        MemberListGrid.DataSource = mlist.LoadComMemberList(myMember.business_unit_id);
        MemberListGrid.DataBind();
		ASPxPopupControl1.ShowOnPageLoad = true;
		}

	protected void btnSelectUsers_Click(object sender, EventArgs e)
		{
		var strTotalIDs = "";
		var user_names = "";



		for (var i = 0; i < MemberListGrid.Rows.Count; i++)
			{
			var row = MemberListGrid.Rows[i];
			var isChecked = ((CheckBox)MemberListGrid.Rows[i].FindControl("chkMember")).Checked;

			if (isChecked)
				{
				var mymember = new NeMember(int.Parse(row.Cells[1].Text));
				strTotalIDs = strTotalIDs + mymember.id + ",";
				user_names = user_names + mymember.Username + ",";
				}
			}
		if (user_names.Length >= 1)
			{
			user_names = user_names.Substring(0, user_names.Length - 1);  // takes the last comma off the back 
			Session["AllMessageIds"] = strTotalIDs.Substring(0, strTotalIDs.Length - 1);
			}
		txtCCUsers.Text = user_names;
		ASPxPopupControl1.ShowOnPageLoad = false;

		}
	protected void chkSendAll_CheckedChanged(object sender, EventArgs e)
		{
		try
			{
			var chk = true;
			if (!chkSendAll.Checked)
				{
				chk = false;
				}
			foreach (GridViewRow drgItem in MemberListGrid.Rows)
				{
				var tempCheck = (CheckBox)drgItem.FindControl("chkMember");
				if (tempCheck != null)
					{
					tempCheck.Checked = chk;
					}
				}
			}
		catch (Exception ex)
			{
			throw ex;
			}
		}
	protected void btnReply_Click(object sender, EventArgs e)
	{
		var m = new NeMessaging(Convert.ToInt32(Session["meesage_reply_id"]));

		drpUsers.DataBind();
		drpUsers0.DataBind();
		pnlreply.Visible = true;
		drpUsers0.SelectedValue = m.Message_LeftBy_Member_ID.ToString();
		pnlMessageDetails.Visible = false;
		
		txtSubject0.Text = txtSubject.Text;
		txtMessage0.Enabled = true;
		txtMessage0.Text = System.Environment.NewLine + System.Environment.NewLine + "Original Message: " + string.Format("{0:F}", m.Date) +System.Environment.NewLine + System.Environment.NewLine + txtMessage.Text;

	}
	protected void btnSend0_Click(object sender, EventArgs e)
	{
		var NewMessage = new NeMessaging();
		var m = new NeMessaging(Convert.ToInt32(Session["meesage_reply_id"]));
		var messagecreated = true;
		var error_text = "unknown";
		try
		{

			NeMember mymember;
			mymember = new NeMember(Session["session"].ToString());
			NewMessage.Message_LeftBy_Member_ID = mymember.id;
			NewMessage.MessageType_ID = Convert.ToInt32(m.MessageType_ID);
			NewMessage.Message_Subject = txtSubject0.Text;
			NewMessage.Message_Body = txtMessage0.Text;
			NewMessage.Date = System.DateTime.Now;
			NewMessage.AddNewMessage();

			m.DeleteMessage(m.Message_ID, 1, myMember.id);
			MessageGrid.DataBind();
		}
		catch (Exception ex1)
		{
			error_text = ex1.Message;
			messagecreated = false;
		}
		finally
		{
			if (messagecreated)
			{
				int messageidnew;
				var chkIDs = "";
				var strTotalError = "";
				var strTotalSuccess = "";
				messageidnew = NewMessage.Message_ID;

				if (Session["AllMessageIds"] != null)
				{
					chkIDs = Session["AllMessageIds"].ToString();
					chkIDs = chkIDs + "," + drpUsers0.SelectedValue;
				}
				else
				{
					chkIDs = drpUsers0.SelectedValue;
				}
				var arrUserID = chkIDs.Split(',');
				var Count = 0;


				for (Count = 0; Count < arrUserID.Length; Count++)
				{
					var UID = arrUserID[Count];
					var NewMessageTo = new NeMessaging();
					NewMessageTo.MessageTo_Message_ID = messageidnew;
					NewMessageTo.MessageTo_Member_ID_To = Convert.ToInt32(UID);
					try
					{
						var Curmember = new NeMember(Convert.ToInt32(UID));
						NewMessageTo.CreateMessageTo();
						strTotalSuccess = strTotalSuccess + Curmember.Username + ", ";
					}
					catch (Exception ee)
					{
						NeMember Curmember;
						Curmember = new NeMember(NewMessageTo.MessageTo_Member_ID_To.ToString());
						strTotalError = strTotalError + Curmember.Username + ", ";
					}

				}
				var messageresult = "";

				if (strTotalError != "")
				{
					strTotalError = strTotalError.Substring(0, strTotalError.Length - 1);
					messageresult = "The message failed to be sent to the following Users " + strTotalError + ". Please Try again";
				}
				else
				{

				}
				if (strTotalSuccess == "")
				{
					errorlabel.Text = messageresult;
					Session["AllMessageIds"] = null;
				}
				else
				{
					strTotalSuccess = strTotalSuccess.Substring(0, strTotalSuccess.Length - 1);
					errorlabel.Text = "The Message was sent successfully to " + strTotalSuccess + " ." + messageresult;
					Session["AllMessageIds"] = null;
				}

			}
			else
				errorlabel.Text = "Message Not sent " + error_text;

		}

	}
	protected void btnCancel0_Click(object sender, EventArgs e)
	{
		txtMessage0.Text = "";
		txtSubject0.Text = "";
		
		if (ViewState["menuid"].ToString() != "3")
		{
			pnlreply.Visible = false;

		}
	}
}
