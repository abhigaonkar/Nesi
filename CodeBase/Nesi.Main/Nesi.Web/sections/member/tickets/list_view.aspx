<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/nonFrame.master" Inherits="sections_member_tickets_Copy_of_index" Title="Ticket List View" Codebehind="list_view.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register src="../../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>
<asp:Content ID="header" ContentPlaceHolderID="header_placeholder" runat="server">
	<script type="text/javascript" src="/js/tickets.js"></script>
    <script type="text/javascript" src="/js/functions.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<style type="text/css">
		.menu_head
			{
			font-weight:		bold;
			font-size:			1.1em;
			}
		.menu_link
			{
			font-size:			1em;
			margin-top:			3px;
			}
		.menu_link a
			{
			margin-left:		5px;
			color:				#069;
			}
		.menu_link a:hover
			{
			color:				#000;
			}
	</style>
	<script type="text/javascript">
		function bind_tooltips() {
			$(".ttip").each(function () {
				$(this).tip();
			});
		}
		
		function catch_enter(obj, e)
			{
			var key = e.which||e.keyCode;
			if(key == 13)
				{
				e.preventDefault();
				$("#search_button").click();
				$(obj).blur();
				}
			}

		$(document).ready(function () {
			bind_tooltips();
		});


		function is_int(value) {
			if ((parseFloat(value) == parseInt(value)) && !isNaN(value)) {
				return true;
			} else {
				return false;
			}
		}
		function new_ticket()
			{
			pop_new.Show();
			$("#if_newframe").attr("src", "./CreateIssue.aspx?from=list");
			}
		function search_tickets(obj)
			{
			var query		= $.trim($(".search_box").val());
			if(query == "")
				{
				alert("Please enter a search query");
				}
			else
				{
				location.href	= "./list_view.aspx?a=Search&q="+query;
				}
			}
	</script>

    <div>
    <table width="100%">
    <tr style="vertical-align:top">
      <td style="width: 175px" nowrap="nowrap">
		  <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" Width="200px" HeaderText="Search" BackColor="White" Height="100px" ContentHeight="100px" >
			  <PanelCollection>
				  <dx:PanelContent runat="server">
					  <table width="100%" cellpadding="2" cellspacing="0">
						  <tr>
							  <td><b>Keyword(s):</b></td>
						  </tr>
						  <tr>
							  <td>
								  <input type="text" runat="server" class="search_box" id="txtSearchBox" onkeydown="catch_enter(this, event)" />
							  </td>
						  </tr>
						  <tr>
							  <td>
								  <button type="button" id="search_button" onclick="search_tickets(this);">Submit</button>
								  <button type="button" id="clear_button" onclick="location.href='./list_view.aspx'">Clear</button>
							  </td>
						  </tr>
					  </table>
					  <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red" Visible="False" Width="150px"></asp:Label>
				  </dx:PanelContent>
			  </PanelCollection>
			  <ContentPaddings PaddingBottom="6px" PaddingLeft="9px" PaddingRight="9px" PaddingTop="6px" />
			  <Border BorderColor="#4682B4" BorderStyle="Solid" BorderWidth="1px" />
			  <ContentPaddings Padding="2px"></ContentPaddings>
			  <HeaderStyle BackColor="#4682B4" Font-Bold="True" Font-Names="Arial" ForeColor="White">
				  <Border BorderStyle="None" />
				  <BorderTop BorderColor="#B5D8FF" BorderStyle="Solid" BorderWidth="1px" />
				  <Border BorderStyle="None"></Border>

				  <BorderTop BorderColor="#B5D8FF" BorderStyle="Solid" BorderWidth="1px"></BorderTop>
			  </HeaderStyle>
			  

			  <Border BorderColor="#4682B4" BorderStyle="Solid" BorderWidth="1px"></Border>
		  </dx:ASPxRoundPanel>
		<br />
        <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" HeaderText="Member Section" BackColor="White"  Font-Names="Arial">
            <PanelCollection>
                <dx:PanelContent runat="server">
                    <div class="menu_link" id="hl_newticket" runat="server"><a href='javascript:void(0);' onclick="new_ticket();">Raise New Ticket</a></div>
					<div class="menu_link" id="hl_myissues" runat="server"><a href='./list_view.aspx?a=GetMyTickets'>Tickets I Started</a></div>
					<div class="menu_link" id="hl_mycourt" runat="server"><a id="mycourt_link" runat="server" href='./list_view.aspx?a=GetMyCourt'>Tickets In My Court</a></div>
					<div class="menu_link" id="hl_allissues" runat="server"><a href='./list_view.aspx?a=GetAllTickets'>View All Tickets</a></div>
					<div class="menu_link" id="hl_columnview" runat="server"><a href='./index.aspx'>Column View</a></div>
                  </dx:PanelContent>
            </PanelCollection>
            <ContentPaddings PaddingBottom="6px" PaddingLeft="9px" PaddingRight="9px" PaddingTop="6px" />
            <Border BorderColor="#4682B4" BorderStyle="Solid" BorderWidth="1px" />
            <HeaderStyle BackColor="#4682B4" Font-Bold="True" Font-Names="Arial" ForeColor="White">
                <Border BorderStyle="None" />
                <BorderTop BorderColor="#B5D8FF" BorderStyle="Solid" BorderWidth="1px" />
            </HeaderStyle>
            
        </dx:ASPxRoundPanel>
		<br />
        <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="Admin Section"
            Width="100%" Visible="False" BackColor="White" 
			>
            <PanelCollection>
                <dx:PanelContent runat="server">
					<div class="menu_head">Tickets Assigned to Me:</div>
					<div class="menu_link" id="hl_dashboard" visible="false" runat="server"><a href='list_view.aspx?a=Dashboard'>Dashboard</a></div>
					<div class="menu_link" id="hl_allopenmygroup" runat="server"><a href='list_view.aspx?a=GetMyAssignedTickets'>View All Open</a></div>
					<div class="menu_link" id="hl_duethisweek" runat="server"><a href='./list_view.aspx?a=GetMyAssignedTicketsthisweek'>Due This Week</a></div>
					<br />
					<div id="pnl_mygroup" runat="server" visible="false">
						<div class="menu_head">My Group:</div>
						<div class="menu_link" id="hl_grp_unassigned" runat="server"><a href='./list_view.aspx?a=GetUnassignedTickets'>Unassigned Tickets</a></div>
						<div class="menu_link" id="hl_grp_opentickets" runat="server"><a href='./list_view.aspx?a=GetAllOpenTicketsfromGroup'>Open Tickets</a></div>
						<div class="menu_link" id="hl_grp_duethisweek" runat="server"><a href='./list_view.aspx?a=GetAllOpenTicketsfromGroupthisweek'>Due This Week</a></div>
						<div class="menu_link" id="hl_grp_managersettings" runat="server"><a href='javascript:void(0);' onclick="pop_managersettings.Show();">Manager Settings</a></div>
					</div>
					<br />
					<div id="pnl_everything" runat="server">
						<div class="menu_head">Everything:</div>
						<div class="menu_link" id="hl_ev_opentickets" runat="server"><a href='./list_view.aspx?a=GetAllOpenTickets'>View All Open Tickets</a></div>
						<div class="menu_link" id="hl_ev_closedtickets" runat="server"><a href='./list_view.aspx?a=GetAllClosedTickets'>View All Closed Tickets</a></div>
						<div class="menu_link" id="hl_ev_ticketreleases" runat="server"><a href='./list_view.aspx?a=GetTicketReleases'>View Ticket Releases</a></div>
					</div>
					<br />
					<div id="pnl_plan17" runat="server" visible="false">
						<div class="menu_head">Plan 17:</div>
						<div class="menu_link" id="hl_ltquarterly" runat="server"><a href="./list_view.aspx?a=GetAllLTTickets">View LT Quarterly Objectives</a></div>
					</div>
                </dx:PanelContent>
            </PanelCollection>
            <ContentPaddings PaddingBottom="6px" PaddingLeft="9px" PaddingTop="6px" PaddingRight="9px" />
            <HeaderStyle BackColor="#4682B4" Font-Bold="True" Font-Names="Arial" ForeColor="White">
                <BackgroundImage Repeat="RepeatX" />
                <Border BorderStyle="None" />
                <BorderTop BorderColor="#B5D8FF" BorderStyle="Solid" BorderWidth="1px" />
            </HeaderStyle>
            <Border BorderColor="#4682B4" BorderStyle="Solid" BorderWidth="1px" />
            
        </dx:ASPxRoundPanel>
        <dx:ASPxRoundPanel ID="rpplan17" runat="server" Width="100%" 
			  HeaderText="Plan 17" BackColor="White" 
			  ClientVisible="False">
            <PanelCollection>
                <dx:PanelContent runat="server">
					<asp:LinkButton ID="blMyCourt0" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Small" OnClick="blthisquarter_Click" ToolTip="List all tickets due this quarter">View This Quarter</asp:LinkButton>
                    <br />
                    <asp:LinkButton ID="blAllIssues0" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Small" OnClick="blAllplan17_Click">View All</asp:LinkButton>
                  </dx:PanelContent>
            </PanelCollection>
            <ContentPaddings PaddingBottom="6px" PaddingLeft="9px" PaddingRight="9px" PaddingTop="6px" />
            <Border BorderColor="#FF8040" BorderStyle="Solid" BorderWidth="1px" />
            <HeaderStyle BackColor="#FF8040" Font-Bold="True" Font-Names="Arial" 
				ForeColor="White" VerticalAlign="Middle">
                <Border BorderStyle="None" />
                <BorderLeft BorderStyle="None" />
			<BorderRight BorderStyle="None" />
			<BorderBottom BorderStyle="None" />
            </HeaderStyle>
            
        </dx:ASPxRoundPanel>
      </td>
      <td id="ticket_list_view" rowspan="4" width="100%">
        			<lc:LayoutControl ID="layout" runat="server" GridviewID="gv_tickets" ShowExcelExport="True" ShowPDFExport="True" ShowToggle="True" />


        <dx:ASPxGridView ID="gv_tickets" runat="server" AutoGenerateColumns="False" 
						KeyFieldName="ticketheader_id" ClientInstanceName="gv_tickets" CssClass="grid"
						Width="100%" OnHtmlDataCellPrepared="gv_tickets_HtmlDataCellPrepared" 
						OnCustomCallback="gv_tickets_CustomCallback" 
						OnCustomJSProperties="gv_tickets_CustomJSProperties" Theme="NETheme01">
            <GroupSummary>
				<dx:ASPxSummaryItem FieldName="RaisedBy" ShowInColumn="Raised By" SummaryType="Count" />
				<dx:ASPxSummaryItem FieldName="AsignedTo" ShowInColumn="AsignedTo" SummaryType="Count" />
			</GroupSummary>
            <Columns>
				<dx:GridViewDataTextColumn Visible="False" VisibleIndex="0" Caption="Vote" FieldName="v"
					Width="30px" MinWidth="30">
						<DataItemTemplate>
							<div class="iconv wrapper" id="icon_holder_wrapper" runat="server">
							</div>
						</DataItemTemplate>
						<Settings ShowFilterRowMenu="False" AllowAutoFilter="False" AllowHeaderFilter="False" />
                        <CellStyle HorizontalAlign="Center" />
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="ID" FieldName="ticketheader_id" VisibleIndex="1"
					Width="50px" SortIndex="1" SortOrder="Ascending">
					<Settings ShowFilterRowMenu="False" />
					<CellStyle Font-Bold="False" HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Ticket" FieldName="RaisedIssue" 
					 VisibleIndex="2" Width="100%" MinWidth="250">
					<DataItemTemplate>
						<dx:ASPxHyperLink ID="hl" runat="server" Font-Names="Arial" >
						</dx:ASPxHyperLink>
					</DataItemTemplate>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataDateColumn FieldName="DateCreated" VisibleIndex="3" 
					Width="90px" Visible="False">
					<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
					</PropertiesDateEdit>
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataDateColumn>
				<dx:GridViewDataTextColumn Caption="Last Updated" FieldName="ticketheader_modified_date"
					VisibleIndex="4" Width="90px">
					<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
					</PropertiesTextEdit>
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="PageName" VisibleIndex="6" Width="80px">
					<Settings HeaderFilterMode="CheckedList" />
					<CellStyle HorizontalAlign="Center" Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="7" Width="60px">
					<Settings HeaderFilterMode="CheckedList" />
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="TypeOfTicket" VisibleIndex="8" 
					Width="60px">
					<Settings HeaderFilterMode="CheckedList" />
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Assigned To" FieldName="AsignedTo" VisibleIndex="9"
					Width="90px">
					<Settings HeaderFilterMode="CheckedList" />
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Raised By" FieldName="RaisedBy" VisibleIndex="10"
					Width="75px">
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="Priority" Name="priority" VisibleIndex="11"
					Width="50px" SortIndex="0" SortOrder="Ascending">
					<Settings SortMode="DisplayText" HeaderFilterMode="CheckedList" />
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
            	<dx:GridViewDataTextColumn Caption="Group" FieldName="groupname" 
					ShowInCustomizationForm="True" VisibleIndex="12" Width="60px">
					<Settings HeaderFilterMode="CheckedList" />
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
            	<dx:GridViewDataDateColumn Caption="Committed Date" FieldName="exp_fin" 
					ShowInCustomizationForm="True" VisibleIndex="13" Width="120px">
					<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
						EditFormatString="yyyy-MM-dd">
					</PropertiesDateEdit>
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataDateColumn>
            	<dx:GridViewDataTextColumn Caption="Release #" FieldName="release_id" 
					MinWidth="50" ShowInCustomizationForm="True" VisibleIndex="5" Width="50px">
					<CellStyle HorizontalAlign="Center"/>
				</dx:GridViewDataTextColumn>
            </Columns>
            <SettingsPopup>
				<HeaderFilter Height="600px" MinHeight="100px" MinWidth="200px" />
			</SettingsPopup>
			
			
           
            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" 
				ShowHeaderFilterButton="True" ShowTitlePanel="True" ShowGroupPanel="True" 
				ShowFilterBar="Visible" />
            <SettingsPager PageSize="50" AlwaysShowPager="True">
            </SettingsPager>
			<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" />
			<StylesEditors>
				<ProgressBar Height="25px">
				</ProgressBar>
			</StylesEditors>
        </dx:ASPxGridView>



          <dx:ASPxPopupControl ID="pop_new" runat="server" ClientInstanceName="pop_new" HeaderText="New Ticket" Height="768px" Width="1024px" CloseAction="CloseButton" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
		  	<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True"><iframe id="if_newframe" src="about:blank" width="1024" height="768" frameborder="0"></iframe></dx:PopupControlContentControl>
</ContentCollection>
<ContentStyle><Paddings Padding="0px" /></ContentStyle>
		  </dx:ASPxPopupControl>
          <dx:ASPxPopupControl ID="pop_managersettings" runat="server" HeaderText="Manager Settings" ClientInstanceName="pop_managersettings" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" PopupAnimationType="None">
			  <ModalBackgroundStyle BackColor="Transparent">
			  </ModalBackgroundStyle>
			  <ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<table style="width:100%;">
		<tr>
			<td>
				<dx:ASPxGridView ID="gv_assignees" runat="server" AutoGenerateColumns="False" Font-Names="Arial" KeyFieldName="ticketmanager_id" OnRowDeleting="ASPxGridView2_RowDeleting" OnRowInserting="ASPxGridView2_RowInserting" Width="450px">
					
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
					<Columns>
						<dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowInCustomizationForm="True" VisibleIndex="0" Width="60px" ShowNewButton="true" ShowDeleteButton="true" ShowClearFilterButton="true">
							
							
							
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewCommandColumn>
						<dx:GridViewDataComboBoxColumn Caption="Ticket Assignees" FieldName="ticketmanager_member_id" ShowInCustomizationForm="True" SortIndex="1" SortOrder="Ascending" VisibleIndex="1" Width="125px">
							<PropertiesComboBox DataSourceID="sds_assignees" AnimationType="None" EnableCallbackMode="True" IncrementalFilteringMode="StartsWith" TextField="membername" ValueField="member_id" ValueType="System.Int32">
							</PropertiesComboBox>
							<Settings SortMode="DisplayText" />
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewDataComboBoxColumn>
						<dx:GridViewDataComboBoxColumn Caption="Group" FieldName="ticketmanager_group_id" ShowInCustomizationForm="True" SortIndex="0" SortOrder="Ascending" VisibleIndex="2" Width="150px">
							<PropertiesComboBox DataSourceID="sds_ticketgroup" TextField="ticket_group_name" ValueField="ticket_group_id" ValueType="System.Int32">
							</PropertiesComboBox>
							<Settings SortMode="DisplayText" />
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewDataComboBoxColumn>
					</Columns>
					<SettingsPager Mode="ShowAllRecords">
					</SettingsPager>
					<Settings ShowFilterRow="True" ShowHeaderFilterBlankItems="False" />
					<Templates>
						<EditForm>
							<dx:ASPxGridViewTemplateReplacement ID="Editors" runat="server" ReplacementType="EditFormEditors" />
							<div style="margin-top: 10px">
								<div style="float: left; margin-left: 5px">
									<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' Text="Cancel" Width="100px" />
								</div>
								<div style="float: right; margin-right: 5px">
									<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' CssClass="input" Text="Add" Width="100px" />
								</div>
							</div>
						</EditForm>
					</Templates>
				</dx:ASPxGridView>
				<asp:SqlDataSource ID="sds_ticketgroup" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
				<asp:SqlDataSource ID="sds_assignees" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select member_fullname as membername,member_id from member where member_status = 'Active' order by member_nickname"></asp:SqlDataSource>
			</td>
		</tr>
	</table>
				  </dx:PopupControlContentControl>
</ContentCollection>
		  </dx:ASPxPopupControl>
      </td>
    </tr>
    <tr style="vertical-align:top">
      <td nowrap="nowrap" valign="top">
          &nbsp;</td>
    </tr>
    <tr style="vertical-align:top">
    <td valign="top">
        &nbsp;</td>
    </tr>
    
    <tr style="vertical-align:top">
    <td>
        &nbsp;</td>
    </tr>
    
    </table>
    
    </div>
    
</asp:Content>

