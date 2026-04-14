<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/nonFrame.master" Theme="NETheme01" Inherits="sections_member_tickets_ticketpage" Codebehind="ticketpage.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>


<asp:content ContentPlaceHolderID="header_placeholder" ID="header_ph" runat="server">
	<link type="text/css" href="/css/tickets.css" rel="Stylesheet" />
	<script type="text/javascript" src="/js/jquery.paste.js"></script>
	<style runat="server" type="text/css" id="css"></style>
	<asp:literal ID="viewport" runat="server"></asp:literal>
</asp:content>
<asp:content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript" src="/js/tickets.js"></script>
	<div id="bt_mobile_back" runat="server" visible="false" class="al_center"><button type="button" class="whitetext aligncenter" onclick="if(confirm('Are you sure you want to leave?')){location.href = '/mobile/index.aspx?a=tickets';}" style="font-size:1.5em;background-color:#4682b4;color:#fff;width:96%;margin-top:5px;height:35px;border:0;">Back</button></div>
	<iframe width="1" height="1" frameborder="0" id="downloader"></iframe>
	<dx:aspxcallback runat="server" ID="cb_request_reopen" ClientInstanceName="cb_request_reopen" OnCallback="cb_request_reopen_Callback">
	<ClientSideEvents BeginCallback="function(s,e){please_wait('start');}" EndCallback="function(s,e){please_wait('end');}" CallbackComplete="function(s,e){alert('Request processed successfully');pop_reopen_ticket.Hide();location.href=location.href;}" />
	</dx:aspxcallback>
	<div id="ticket_detail">
	<dx:aspxcallbackpanel ID="client_cbp_left" runat="server" ClientInstanceName="client_cbp_left" OnCallback="client_cbp_CallBack">
		<ClientSideEvents BeginCallback="function(s,e){please_wait('start');}" EndCallback="function(s,e){ if(s.cpRefresh != null){location.reload();} else {please_wait('end');}}" />
		<PanelCollection>
			<dx:PanelContent>
				<div class="col_empty push_left">
					<div class="col client_details" id="col_clientdetails" runat="server">
						<div class="title" onclick="handle_collapse(this);">Details</div>
						<div class="body" id="client_details_body" runat="server">
							<div class="row"><b class="b">Ticket #:</b><dx:ASPxLabel ID="lbl_client_number" runat="server" Text=""></dx:ASPxLabel></div>
							<div class="row"><b class="b">Subject:</b><dx:ASPxLabel ID="lbl_client_subject" runat="server" Text=""></dx:ASPxLabel></div>
							<div class="row" id="client_row_vote" runat="server"><b class="b">Vote:</b><span class="iconv wrapper" id="client_icon_holder_wrapper" runat="server"></span> </div>
							<div class="row"><b class="b">Group:</b><dx:ASPxLabel ID="lbl_client_group" runat="server" Text=""></dx:ASPxLabel></div>
							<div class="row"><dx:aspxbutton text="Move to my group" autopostback="false" width="100%" id="bt_movegroup" runat="server"><clientsideevents click="function(s,e){if(confirm('Are you sure this is in the wrong group?')){client_cbp_left.PerformCallback('movegroup');}}" /></dx:aspxbutton></div>
							<div class="row"><dx:aspxbutton text="Assign to me" autopostback="false" width="100%" id="bt_assigntome" runat="server"><clientsideevents click="function(s,e){if(confirm('Are you sure this should be assigned to you?')){client_cbp_left.PerformCallback('assigntome');}}" /></dx:aspxbutton></div>
							<div class="row"><b class="b">Pertaining To:</b><dx:ASPxLabel ID="lbl_client_pertaining" runat="server" Text=""></dx:ASPxLabel></div>
							<div class="row"><b class="b">Type:</b><dx:ASPxLabel ID="lbl_client_type" runat="server" Text=""></dx:ASPxLabel></div>
							<div class="row"><b class="b">Status:</b><dx:ASPxLabel ID="lbl_client_status" runat="server" Text=""></dx:ASPxLabel></div>
							<div class="row"><b class="b">Created By:</b><dx:ASPxLabel ID="lbl_client_created" runat="server" Text=""></dx:ASPxLabel></div>
							<div class="row"><b class="b">Assigned To:</b><dx:ASPxLabel ID="lbl_client_assigned" runat="server" Text=""></dx:ASPxLabel></div>
							<div class="row"><b class="b">Follow:</b><dx:ASPxCheckBox ID="chk_client_follow" ClientInstanceName="chk_client" ClientEnabled="false" runat="server" Text="">
								<ClientSideEvents CheckedChanged="function(s,e){cb_client_follow.PerformCallback(s.GetChecked())}" />
							</dx:ASPxCheckBox></div>
							<div class="row" id="row_requestreopen_client" runat="server"><dx:ASPxHyperLink ID="hl_requestreopen_client" runat="server" Font-Bold="True" ForeColor="#FF9900" ClientVisible="False" Text="Request Reopen of this ticket" NavigateUrl="javascript:void(0);">
								<ClientSideEvents Click="function(s,e){if(confirm('Are you sure you want to request that this ticket be reopened?')){cb_request_reopen.PerformCallback();}}" />
							</dx:ASPxHyperLink></div>
							<dx:ASPxCallback runat="server" ID="cb_client_follow" ClientInstanceName="cb_client_follow" OnCallback="cb_client_follow_Callback">
							<ClientSideEvents BeginCallback="function(s,e){please_wait('start');}" EndCallback="function(s,e){please_wait('end');}" />
							</dx:ASPxCallback>
						</div>
					</div>
				</div>
			</dx:PanelContent>
		</PanelCollection>
	</dx:aspxcallbackpanel>
	<div class="col_empty push_left">
	<div class="col details" id="col_details" runat="server">
		<div class="title" onclick="handle_collapse(this);">Details</div>
		<div class="body">
			<dx:aspxcallbackpanel ID="cb" runat="server" ClientInstanceName="cb" OnCallback="cb_Callback" Width="100%">
				<ClientSideEvents BeginCallback="function(s,e){please_wait('start');}" EndCallback="function(s, e) {please_wait('stop');handle_ticket_updated(false, s, e);popnewchat.PerformCallback();}" />
				<PanelCollection>
					<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
						<div style="max-height: 800px; width: 100%;" class="details">
							<div id="row_duplicate" runat="server" class="br">
								<div class="l">&nbsp;</div>
								<div class="r">
									<dx:ASPxButton ID="bt_duplicate" runat="server" AutoPostBack="False" Text="Duplicate Ticket" Width="100%" Visible="False">
										<Image Url="~/images/icon/icon[replicate].gif">
										</Image>
									</dx:ASPxButton>
								</div>
							</div>
							<div class="row" id="row_close_ticket" runat="server"><dx:ASPxHyperLink ID="hl_closeticket" runat="server" Font-Bold="True" ForeColor="Red" ClientVisible="False" Text="Close This Ticket" NavigateUrl="javascript:void(0);"><ClientSideEvents Click="function(s,e){close_ticket.Show();close_comment.Focus();}" /></dx:ASPxHyperLink></div>
							<div class="row" id="row_requestreopen" runat="server"><dx:ASPxHyperLink ID="hl_requestreopen" runat="server" Font-Bold="True" ForeColor="#FF9900" ClientVisible="False" Text="Request Reopen of this ticket" NavigateUrl="javascript:void(0);">
								<ClientSideEvents Click="function(s,e){pop_reopen_ticket.Show();}" />
							</dx:ASPxHyperLink></div>
							<div class="br">
								<div class="l">Ticket #</div>
								<div class="r"><dx:ASPxLabel ID="lblIssueIDDisp" runat="server" ClientInstanceName="lblIssueIDDisp" Font-Size="9pt" Text="ASPxLabel"></dx:ASPxLabel></div>
							</div>
								<div id="row_created_date" runat="server" class="br">
									<div class="l">Created</div>
									<div class="r"><dx:ASPxLabel ID="lblDateCreateDisp" EncodeHtml="false" runat="server"></dx:ASPxLabel></div>
								</div>
								<div id="row_assigned_date" runat="server" class="br">
									<div class="l">Assigned </div>
									<div class="r"><dx:ASPxLabel ID="lblasndateDisp" EncodeHtml="false" runat="server"></dx:ASPxLabel></div>
								</div>
								<div id="row_modified_date" runat="server" class="br">
									<div class="l">Last Modified</div>
									<div class="r"><dx:ASPxLabel ID="lblDateModDisplay" EncodeHtml="false" runat="server"></dx:ASPxLabel></div>
								</div>
								<div id="row_closed_date" runat="server" class="br">
									<div class="l">Closed Date</div>
									<div class="r"><dx:ASPxLabel ID="lblCloseDisp" EncodeHtml="false" runat="server"></dx:ASPxLabel></div>
								</div>
								<div class="br">
									<div class="l">Private</div>
									<div class="r"><dx:ASPxCheckBox ID="chkPrivate" runat="server" ClientEnabled="False"></dx:ASPxCheckBox></div>
								</div>
							<div class="br" id="row_vote" runat="server">
								<div class="l">Vote:</div>
								<div class="r">
									<div class="iconv wrapper" id="icon_holder_wrapper" runat="server">
									</div>
								</div>
							</div>
							<div class="br">
								<div class="l">Subject</div>
								<div class="r">
									<dx:ASPxMemo ID="memo_subject" runat="server" ClientInstanceName="memo_subject" ClientEnabled="False" Height="71px" width="100%">
									</dx:ASPxMemo>
								</div>
							</div>	

							<div id="admin_details" runat="server">
								<div class="br">
									<div class="l">Created By</div>
									<div class="r">
										<dx:ASPxComboBox ID="combo_created" runat="server" TextField="name" 
											ValueField="id" ClientEnabled="False" EnableCallbackMode="True"  width="100%"
											IncrementalFilteringMode="StartsWith" ValueType="System.Int32" ClientInstanceName="combo_created">
										</dx:ASPxComboBox>
									</div>
								</div>
								<div class="br">
									<div class="l">Group</div>
									<div class="r">
										<dx:ASPxComboBox ID="combo_group" runat="server" ClientEnabled="False" Width="100%" ClientInstanceName="combo_group" AnimationType="None" OnCallback="combo_group_Callback1" TextField="name" ValueField="id" ValueType="System.Int32">
											<ClientSideEvents SelectedIndexChanged="function(s, e) {cb.PerformCallback('group_change');}" />
										</dx:ASPxComboBox>
									</div>
								</div>
								<div class="br">
									<div class="l">Pertaining To</div>
									<div class="r">
										<dx:ASPxComboBox ID="ddl_page" runat="server" ClientEnabled="False" ClientInstanceName="ddl_page" Width="100%" AnimationType="None" OnCallback="ddl_page_Callback" TextField="ticketpage_name" ValueField="ticketpage_id" ValueType="System.Int32" NullText="Select Page">
										</dx:ASPxComboBox>
									</div>
								</div>
								<div class="br">
									<div class="l">Issue Type</div>
									<div class="r">
										<dx:ASPxComboBox ID="ddl_type" runat="server" ClientInstanceName="ddl_type" ClientEnabled="False" Width="100%" OnCallback="ddl_type_Callback" DataSourceID="SqlDataSource3" AnimationType="None" TextField="tickettype_name" ValueField="tickettype_id" ValueType="System.Int32" NullText="Select Type">
										</dx:ASPxComboBox>
										<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT Distinct
tickettype.tickettype_id,
tickettype.tickettype_name
FROM
tickettype
INNER JOIN ticket_group_type_link ON tickettype.tickettype_id = ticket_group_type_link.ticket_type_id
INNER JOIN ticketpage ON ticketpage.ticketpage_ticket_group_id = ticket_group_type_link.ticket_group_id 
WHERE ticketpage_ticket_group_id = ?gid">
											<SelectParameters>
												<asp:ControlParameter ControlID="combo_group" Name="gid" PropertyName="Value" />
											</SelectParameters>
										</asp:SqlDataSource>
									</div>
								</div>
								<div class="br">
									<div class="l">Priority</div>
									<div class="r"><dx:ASPxComboBox ID="combo_priority" runat="server" ClientEnabled="False" ValueField="id" TextField="name" ValueType="System.Int32" NullText="Select Priority" ClientInstanceName="combo_priority" width="100%"></dx:ASPxComboBox></div>
								</div>
								<div class="br">
									<div class="l">Assigned To</div>
									<div class="r"><dx:ASPxComboBox ID="combo_assigned" runat="server" AutoPostBack="False" ClientEnabled="false" Width="100%" ValueField="id" TextField="name" ValueType="System.Int32" NullText="Select Assignee" ClientInstanceName="combo_assigned"></dx:ASPxComboBox></div>
								</div>
								<div class="br" id="row_objective_owner" runat="server" visible="false">
									<div class="l">Objective Owner</div>
									<div class="r">
										<dx:ASPxComboBox ID="combo_objectiveowner" runat="server" EnableCallbackMode="True" Width="100%" IncrementalFilteringMode="StartsWith" TextField="name" ValueField="id" ValueType="System.Int32">
										</dx:ASPxComboBox>
									</div>
								</div>
								<br class="br"/>
								<div style="display:none;">
									<div class="l">Hours Spent</div>
									<div class="r"><dx:ASPxLabel ID="lblhoursdisp" runat="server"></dx:ASPxLabel></div>
								</div>
								<div class="br" style="display:none;">
									<div class="l">Expected Hours</div>
									<div class="r"><dx:ASPxTextBox ID="txtexphours" runat="server" ClientEnabled="False" ClientInstanceName="txtexphours" width="50px"></dx:ASPxTextBox></div>
								</div>
								<div class="br">
									<div class="l">Status</div>
									<div class="r"><dx:ASPxComboBox ID="combo_status" runat="server" ValueField="id" TextField="name" ValueType="System.Int32" ClientInstanceName="combo_status" width="100%" NullText="Select Status"><ClientSideEvents SelectedIndexChanged="local_ticket.handle_status" /></dx:ASPxComboBox></div>
								</div>
								<div class="br row_waiting" id="row_waiting" runat="server">
									<div class="l">Waiting On Whom?</div>
									<div class="r"><dx:ASPxComboBox ID="combo_waitinguser" runat="server" ValueField="Key" TextField="Value" ValueType="System.Int32" ClientInstanceName="combo_waitinguser" NullText="Select User"></dx:ASPxComboBox></div>
								</div>
								<div class="br" style="display:none;">
									<div class="l">Commited Date</div>
									<div class="r">
										<dx:ASPxDateEdit ID="dtefinish" runat="server" ClientInstanceName="dtefinish" DisplayFormatString="yyyy-MM-dd" width="100%" EditFormat="Custom" EditFormatString="yyyy-MM-dd" NullText="Unknown">
										</dx:ASPxDateEdit>
									</div>
								</div>
								<div class="br" id="row_release" style="display:none;" runat="server">
									<div class="l val_top">Release</div>
									<div class="r"><dx:ASPxLabel ID="lblrelease" runat="server" Text="Not Set"></dx:ASPxLabel></div>
								</div>
							</div>
							<div class="br">
								<div>
									<dx:ASPxButton ID="bt_saveticket" runat="server" AutoPostBack="False" Width="100%" Text="Update">
										<ClientSideEvents Click="function(s, e) {cb.PerformCallback('save');}" />
									</dx:ASPxButton>
								</div>
							</div>
							<div class="br">
								<dx:ASPxLabel ID="lblError0" runat="server" Font-Bold="True" ForeColor="Red"></dx:ASPxLabel>
							</div>
						</div>
					</dx:PanelContent>
				</PanelCollection>
			</dx:aspxcallbackpanel>
		</div>
	</div>
	</div>
	<div class="col_empty push_left">
	<div class="col tasks" id="col_tasks" runat="server">
		<div class="title" onclick="handle_collapse(this);">Tasks</div>
		<div class="body">
			<dx:aspxmemo runat="server" ID="memo_new_task" ClientInstanceName="memo_new_task" Width="100%" ClientEnabled="false" Height="100px"></dx:aspxmemo>
			<dx:aspxbutton runat="server" ID="bt_add_task" Width="100%" AutoPostBack="false" ClientEnabled="false" Text="Add Task">
				<ClientSideEvents Click="function(s,e){task_list.PerformCallback('new|'+memo_new_task.GetText())}" />
			</dx:aspxbutton>
			<div style="height:5px;"></div>
			<dx:aspxcallbackpanel ID="task_list" ClientInstanceName="task_list" runat="server" OnCallback="task_list_Callback">
				<ClientSideEvents BeginCallback="function(s,e){please_wait('Start');}" EndCallback="function(s,e){if(s.cpResult = 'SUCCESS'){memo_new_task.SetText('');memo_new_task.Focus();}please_wait('Stop');delete s.cpResult;}" />
			</dx:aspxcallbackpanel>
		<br />
		</div>
	</div>
	<div class="col trackers push_left" id="col_trackers" runat="server">
		<div class="title" onclick="handle_collapse(this);">Trackers</div>
		<div class="body">
			<dx:aspxcallbackpanel ID="cb_task" runat="server" ClientInstanceName="cb_task" oncallback="cb_task_Callback">
				<PanelCollection>
					<dx:PanelContent>
						<div class="tracker_row">
							<dx:ASPxComboBox ID="combo_availableusers" ClientInstanceName="combo_availableusers" runat="server" AutoPostBack="false" ClientEnabled="False" Width="100%" CssClass="combo_trackers" ValueField="id" TextField="name" DataSourceID="sds_availableusers">
								<ClientSideEvents Init="function(s,e){var obj = s.GetInputElement();obj.placeholder = 'Select user to add';}" />
							</dx:ASPxComboBox>
							<asp:SqlDataSource ID="sds_availableusers" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.member_id id,concat(a.member_fullname, ' (',business_unit.name,')') name FROM member a inner join business_unit on business_unit.id = a.business_unit_id WHERE a.member_status = 'Active' AND a.member_id NOT IN (SELECT member_id FROM ticket_memberview WHERE ticket_id = ?ticketid) ORDER BY a.member_lastname, member_nickname">
								<SelectParameters>
									<asp:QueryStringParameter QueryStringField="issue" Name="ticketid" />
								</SelectParameters>
							</asp:SqlDataSource>
						</div>
						<div class="tracker_row">
							<dx:ASPxButton ID="bt_savetracker" runat="server" ClientEnabled="False" AutoPostBack="False" Width="100%" Text="Add Tracker">
								<ClientSideEvents Click="function(s, e) {cb_task.PerformCallback('addtracker|'+combo_availableusers.GetValue());}" />
							</dx:ASPxButton>
						</div>
						<div class="tracker_row">
							<dx:ASPxListBox ID="list_trackers"  ClientInstanceName="list_trackers" ClientEnabled="False" runat="server" AutoPostBack="false" ValueField="id" TextField="name" DataSourceID="sds_trackers" Width="100%" Height="150px">
								<ClientSideEvents SelectedIndexChanged="function(s, e) {cb_task.PerformCallback('removetracker|'+s.GetValue());}" />
							</dx:ASPxListBox>
							<asp:SqlDataSource ID="sds_trackers" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, concat(b.member_fullname, ' (',business_unit.ddl_name,')') name FROM ticket_memberview a LEFT JOIN member b ON a.member_id = b.member_id inner join business_unit on business_unit.id = b.business_unit_id WHERE a.ticket_id =?ticketid">
								<SelectParameters>
									<asp:QueryStringParameter QueryStringField="issue" Name="ticketid" />
								</SelectParameters>
							</asp:SqlDataSource>
						</div>
					</dx:PanelContent>
				</PanelCollection>
				<ClientSideEvents EndCallback="function(s,e){cb.PerformCallback('refresh');combo_waitingonuser_popup.PerformCallback();}" />
			</dx:aspxcallbackpanel>
		</div>

	</div>
	</div>
	<div class="col_empty push_right">
	<div class="col chat" id="col_chat" runat="server">
		<dx:aspxpagecontrol ID="pc_chat" ClientInstanceName="pc_chat" runat="server" Width="100%" oncallback="pc_chat_Callback">
			<ClientSideEvents ActiveTabChanged="handle_chat_tabchange" />
			<TabPages>
				<dx:TabPage Text="Chat">
					<ContentCollection>
						<dx:ContentControl>
							<div class="body">
								<dx:ASPxLabel ID="lblInformation" Width="99%" CssClass="chat" EncodeHtml="false" runat="server"></dx:ASPxLabel>
							</div>
								<div class="br fixed">
									<dx:ASPxButton ID="btnaddchat" runat="server" AutoPostBack="False" Width="100%" ClientEnabled="False" Text="Add to Chat">
										<ClientSideEvents Click="function(s, e) {popnewchat.Show(); if(typeof(txtMessage) != 'undefined'){txtMessage.Focus();} else {mobile_chat_message.Focus();}}" />
									</dx:ASPxButton>
								</div>
								<div class="br fixed">
									<dx:ASPxButton ID="btn_files" runat="server" AutoPostBack="False" Width="100%" Visible="False" Text="Attachments">
										<ClientSideEvents Click="function(s, e) {ticket_files.Show();}" />
									</dx:ASPxButton>
								</div>
						</dx:ContentControl>
					</ContentCollection>
				</dx:TabPage>
				<dx:TabPage Text="Symptom" Visible="false">
					<ContentCollection>
						<dx:ContentControl>
							<dx:ASPxMemo Width="99%" ID="memo_symptom" ClientInstanceName="memo_symptom" runat="server" Height="550px"></dx:ASPxMemo>
							<dx:ASPxButton ID="btn_symptom" runat="server" AutoPostBack="False" Width="100%" Text="Save Symptom">
								<ClientSideEvents Click="function(s, e) {handle_sidechat('symptom');}" />
							</dx:ASPxButton>
						</dx:ContentControl>
					</ContentCollection>
				</dx:TabPage>
				<dx:TabPage Text="Cause" Visible="false">
					<ContentCollection>
						<dx:ContentControl>
							<dx:ASPxMemo Width="99%" ID="memo_cause" ClientInstanceName="memo_cause" runat="server" Height="550px"></dx:ASPxMemo>
							<dx:ASPxButton ID="btn_cause" runat="server" AutoPostBack="False" Width="100%" Text="Save Cause">
								<ClientSideEvents Click="function(s, e) {handle_sidechat('cause');}" />
							</dx:ASPxButton>
						</dx:ContentControl>
					</ContentCollection>
				</dx:TabPage>
				<dx:TabPage Text="Solution" Visible="false">
					<ContentCollection>
						<dx:ContentControl>
							<dx:ASPxMemo Width="99%" ID="memo_solution" ClientInstanceName="memo_solution" runat="server" Height="550px"></dx:ASPxMemo>
							<dx:ASPxButton ID="btn_solution" runat="server" AutoPostBack="False" Width="100%" Text="Save Solution">
								<ClientSideEvents Click="function(s, e) {handle_sidechat('solution');}" />
							</dx:ASPxButton>
						</dx:ContentControl>
					</ContentCollection>
				</dx:TabPage>
				<dx:TabPage Text="ROI" Visible="false">
					<ContentCollection>
						<dx:ContentControl>
							<dx:ASPxMemo Width="99%" ID="memo_roi" ClientInstanceName="memo_roi" runat="server" Height="550px"></dx:ASPxMemo>
							<dx:ASPxButton ID="btn_roi" runat="server" AutoPostBack="False" Width="100%" Text="Save ROI">
								<ClientSideEvents Click="function(s, e) {handle_sidechat('roi');}" />
							</dx:ASPxButton>
						</dx:ContentControl>
					</ContentCollection>
				</dx:TabPage>
				<dx:TabPage Text="" Visible="false" tabstyle-width="35px">
					<ContentCollection>
						<dx:ContentControl>
							<dx:ASPxMemo Width="99%" ID="memo_notes" ClientInstanceName="memo_notes" runat="server" Height="550px"></dx:ASPxMemo>
							<dx:ASPxButton ID="btn_notes" runat="server" AutoPostBack="False" Width="100%" Text="Save Notes">
								<ClientSideEvents Click="function(s, e) {handle_sidechat('notes');}" />
							</dx:ASPxButton>
						</dx:ContentControl>
					</ContentCollection>
					<tabimage url="/images/icon/icon[note].gif" width="14" height="18"></tabimage>
				</dx:TabPage>
			</TabPages>
			<TabStyle Width="70px" />
			<ContentStyle Paddings-Padding="2px"></ContentStyle>
		</dx:aspxpagecontrol>
	</div>
	</div>
	<div runat="server" id="js_handler"></div>
		<dx:aspxpopupcontrol ID="close_ticket" ClientInstanceName="close_ticket" runat="server" HeaderText="Closing Comments" Width="500px" Modal="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"   AnimationType="None" Font-Size="16pt">
			<HeaderStyle BackColor="#68AFFD" Font-Bold="False" ForeColor="White" />
			<ContentCollection>
				<dx:PopupControlContentControl runat="server">
					<dx:ASPxMemo runat="server" Rows="4" Width="480px" ID="txtCloseComment" ClientInstanceName="close_comment"></dx:ASPxMemo>
					<br />
					<asp:Button runat="server" Text="Submit" ID="btnClosingSubmit" OnClick="btnClosingSubmit_Click" OnClientClick="return confirm('Are you sure you want to close this ticket?')"></asp:Button>
				</dx:PopupControlContentControl>
			</ContentCollection>
			<ContentStyle VerticalAlign="Top">
			</ContentStyle>
			<ModalBackgroundStyle BackColor="Transparent"></ModalBackgroundStyle>
		</dx:aspxpopupcontrol>
		<dx:aspxpopupcontrol ID="pop_reopen_ticket" ClientInstanceName="pop_reopen_ticket" runat="server" HeaderText="Why do you want this ticket reopened?" Width="500px" Modal="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"   AnimationType="None" Font-Size="16pt">
			<HeaderStyle BackColor="#68AFFD" Font-Bold="False" ForeColor="White" />
			<ContentCollection>
				<dx:PopupControlContentControl runat="server">
					<dx:ASPxMemo runat="server" Rows="4" Width="480px" ID="memo_whyreopen" ClientInstanceName="memo_whyreopen"></dx:ASPxMemo>
					<br />
					<dx:ASPxButton runat="server" AutoPostBack="false" Text="Request Reopen" ID="btn_reopenticket" Width="100%">
						<ClientSideEvents Click="handle_reopen" />
					</dx:ASPxButton>
				</dx:PopupControlContentControl>
			</ContentCollection>
			<ContentStyle VerticalAlign="Top">
			</ContentStyle>
			<ModalBackgroundStyle BackColor="Transparent"></ModalBackgroundStyle>
			<ClientSideEvents Shown="function(s,e){memo_whyreopen.Focus();}" />
		</dx:aspxpopupcontrol>
		<dx:aspxpopupcontrol ID="popnewchat" runat="server" ClientInstanceName="popnewchat" HeaderText="Add to the Discussion..." 
		PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="true" CloseAction="CloseButton"
			PopupVerticalAlign="TopSides" Modal="True" AllowDragging="True" AppearAfter="0" PopupAnimationType="None" onwindowcallback="popnewchat_WindowCallback">
			<HeaderStyle BackColor="#3366CC" Font-Bold="False" 
				Font-Size="16pt" ForeColor="White" >
			<Paddings PaddingBottom="0px" PaddingTop="0px" />
			</HeaderStyle>
			<ModalBackgroundStyle BackColor="Transparent">
			</ModalBackgroundStyle>
			<clientsideevents endcallback="function(s,e){bind_pastebox();}" />
			<ContentCollection>
				<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
				<dx:ASPxLabel ID="lblError" runat="server" Font-Bold="True" Visible="false" ForeColor="Red"></dx:ASPxLabel>
	
				<div id="chat_pop" runat="server">
					<div class="br">
						<div>
							<dx:ASPxHtmlEditor ID="txtMessage" runat="server" Height="270px" Width="98%" ClientInstanceName="txtMessage" Settings-AllowContextMenu="default">
								<clientsideevents htmlchanged="local_ticket.handle_unload.check" />
<Settings AllowContextMenu="Default"></Settings>

								<SettingsImageUpload UploadImageFolder="">
								</SettingsImageUpload>
								<SettingsDocumentSelector>
									<ToolbarSettings ShowCreateButton="False" ShowDeleteButton="False" 
										ShowFilterBox="False" ShowMoveButton="False" ShowPath="False" 
										ShowRefreshButton="False" ShowRenameButton="False" />
								</SettingsDocumentSelector>
							</dx:ASPxHtmlEditor>
							<dx:aspxmemo id="mobile_chat_message" runat="server" visible="false" clientinstancename="mobile_chat_message" width="100%" height="200px">

							</dx:aspxmemo>
						</div>
					</div>
					<div class="br" id="paste_row" runat="server">
						<div ID="pastebox" runat="server" style="font-family: Arial, Helvetica, sans-serif" valign="top">
							<dx:ASPxTextBox ID="tb_pastebox" runat="server" CssClass="tb_pastebox" 
								Height="270px" Native="True" ReadOnly="True" Width="95%">
							</dx:ASPxTextBox>
						</div>
					</div>
					<div class="br shiftdown">
						<div class="l">Include a File?:</div>
						<div class="r">
							<input runat="server" ID="hiddenshot" type="hidden" class="fileblob" />
							<asp:FileUpload runat="server" CssClass="mainfileupload" ID="filMyFile" EnableViewState="False"></asp:FileUpload>

						</div>
					</div>
					<div class="br" id="popup_hours" runat="server">
						<div class="l">Time spent on this in hours:</div>
						<div class="r">
							<asp:TextBox ID="txtHours" runat="server" Width="25px"></asp:TextBox>
						</div>
					</div>
					<div id="popup_row_manually_set_status" runat="server" class="br">
						<div class="l">Manually Set Status:</div>
						<div class="r">
							<dx:ASPxComboBox ID="combo_status_popup" runat="server" ValueField="id" TextField="name" ValueType="System.Int32">
								<ClientSideEvents SelectedIndexChanged="local_ticket.handle_status" />
							</dx:ASPxComboBox>
						</div>
					</div>
					<div id="popup_row_waitingon_user" runat="server" class="br popup_waitingon_user" style="display:none;">
						<div class="l">Select User:</div>
						<div class="r">
							<dx:ASPxComboBox ID="combo_waitingonuser_popup" runat="server" ValueField="Key" ClientInstanceName="combo_waitingonuser_popup" TextField="Value" OnCallback="combo_waitingonuser_popup_Callback" ValueType="System.Int32">
							</dx:ASPxComboBox>
						</div>
					</div>
					<div class="br">
						<div>
							<dx:ASPxButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Width="100%" Text="Submit">
								<clientsideevents click="local_ticket.handle_unload.handle_submit" />
							</dx:ASPxButton>
						</div>
					</div>
				</div>
	
				</dx:PopupControlContentControl>
				</ContentCollection>
		</dx:aspxpopupcontrol>
		<dx:aspxpopupcontrol ID="pop_ticket_files" runat="server" ClientInstanceName="ticket_files" Maximized="true" HeaderText="Ticket Attachments" PopupAnimationType="None">
			<ContentStyle Paddings-Padding="0px"></ContentStyle>
			<ContentCollection>
				<dx:PopupControlContentControl runat="server">
					<iframe frameborder="0" id="if_files" class="if_files" runat="server" src="" style="width:100%;height:700px;"></iframe>
				</dx:PopupControlContentControl>
			</ContentCollection>
		</dx:aspxpopupcontrol>
	</div>
<div class="wiki_help_class" id="wiki_help" runat="server" visible="true">
    <img alt="Help for this page" src='/images/icon/icon[help].gif' width='20' height='20' title="Help available for this page" id="wiki_help_img" runat="server" />
</div>
</asp:content>

