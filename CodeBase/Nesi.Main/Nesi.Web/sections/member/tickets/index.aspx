<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/nonFrame.master" Inherits="sections_member_tickets_index" Title="Tickets Column View" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="header_placeholder">
    <style type="text/css">
		.legend {
			font-size: 11px;
		}

			.legend .icon {
				display: inline-block;
				width: 23px;
				text-align: center;
				padding: 2px;
				margin-right: 2px;
			}

				.legend .icon.b {
					color: #000;
				}

				.legend .icon.w {
					color: #fff;
				}

			.legend .r {
				margin: 1px;
			}
		.column
			{
			}
		.column .panel
			{
			width: 100%;
			}
		.column .panel .gv
			{
			width: 100%;
			}
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" runat="Server">
    <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
			<script type="text/javascript">
				function bind_tooltips() {
					$(".ttip").each(function() {
						$(this).tip();
					});
				}


				$(document).ready(function() {
					bind_tooltips();
					page_obj.update_panel_progress.bind();
					Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bind_tooltips);
				});
			</script>


			<div>
				<table width="100%" cellpadding="2" cellspacing="0">
					<tr style="vertical-align:top;">
						<td width="12%" class="column">
							<dx:ASPxRoundPanel ID="rp_menu" runat="server" HeaderText="Menu" BackColor="White"  ContentHeight="200px" cssclass="panel">
								<PanelCollection>
									<dx:PanelContent runat="server">
										<table style="width: 100%;">
											<tr>
												<td>
													<asp:HyperLink ID="HyperLink11" runat="server" Font-Bold="False" Font-Size="Small" NavigateUrl="/sections/member/tickets/CreateIssue.aspx?from=col">Raise a New Ticket</asp:HyperLink>
												</td>
											</tr>
											<tr>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxLabel ID="ASPxLabel1" runat="server" ClientVisible="False" Font-Underline="True" ForeColor="Blue" Text="Manager Settings" Cursor="pointer">
														<ClientSideEvents Click="function(s, e) {popman.Show();}" />
													</dx:ASPxLabel>
												</td>
											</tr>
											<tr>
												<td>
													<asp:HyperLink ID="HyperLink12" runat="server" Font-Bold="False"
														 Font-Size="Small"
														NavigateUrl="~/sections/member/tickets/list_view.aspx">List View</asp:HyperLink>

												</td>
											</tr>
											<tr>
												<td>&nbsp;</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxMemo ID="memsearch" runat="server" ClientInstanceName="memsearch" 
														Height="71px"  Width="100%" NullText="Enter Keywords or the Ticket ID">
														<NullTextStyle ForeColor="Silver">
														</NullTextStyle>
													</dx:ASPxMemo>
												</td>
											</tr>
											<tr>
												<td class="style1">
													<dx:ASPxButton ID="btnsearch" runat="server" Width="100%"
														Text="Search" AutoPostBack="False">
														<ClientSideEvents Click="function(s, e) {popsearch.Show();gv_search.PerformCallback(memsearch.GetText());}" />
													</dx:ASPxButton>
												</td>
											</tr>
										</table>
									</dx:PanelContent>
								</PanelCollection>
								<ContentPaddings Padding="2px" />
								<Border BorderColor="#68AFFD" BorderStyle="Solid" BorderWidth="1px" />
								<HeaderStyle BackColor="#68AFFD" Font-Bold="True"  ForeColor="White">
									<Border BorderStyle="None" />
									<BorderBottom BorderStyle="None" />
								</HeaderStyle>
								
							</dx:ASPxRoundPanel>
							<br />
							<dx:ASPxRoundPanel ID="rp_filter" runat="server"  cssclass="panel" HeaderText="Filter" BackColor="White"  ContentHeight="250px">
								<PanelCollection>
									<dx:PanelContent runat="server">
										<table style="width: 100%;">
											<tr>
												<td>Only Show Before This Due Date:</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxDateEdit ID="dteduebefore" runat="server" Width="100%" 
														DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
													</dx:ASPxDateEdit>
												</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxCheckBox ID="chkshowall" runat="server" CheckState="Checked" 
														Text="Or Show All Dates" TextAlign="Left" Checked="True">
													</dx:ASPxCheckBox>
												</td>
											</tr>
											<tr>
												<td>Show View For:</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxComboBox ID="ddluser" runat="server" DataSourceID="SqlDataSource5"
														AnimationType="None" 
														Height="16px" IncrementalFilteringMode="StartsWith"
														TextField="member_name" ValueField="member_id" ValueType="System.Int32"
														Width="100%" EnableCallbackMode="True">
													</dx:ASPxComboBox>
												</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxComboBox ID="ddlgroup" runat="server" ClientInstanceName="ddlgroup"
														AnimationType="None" EnableCallbackMode="True"
														 Height="16px"
														IncrementalFilteringMode="StartsWith" TextField="ticket_group_name"
														ValueField="ticket_group_id" ValueType="System.Int32" Width="100%">
													</dx:ASPxComboBox>
												</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxComboBox ID="ddltype" runat="server" ClientInstanceName="ddlgroup"
														AnimationType="None"
														 Height="16px"
														IncrementalFilteringMode="StartsWith" TextField="tickettype_name"
														ValueField="tickettype_id" ValueType="System.Int32" Width="100%">
													</dx:ASPxComboBox>
												</td>
											</tr>
											<tr>
												<td nowrap="nowrap">&nbsp;</td>
											</tr>
											<tr>
												<td>
													<asp:LinkButton ID="lblfilter" runat="server" 
														Font-Size="Small" OnClick="lblfilter_click" Width="100%">Apply Filter</asp:LinkButton>
												</td>
											</tr>
										</table>
									</dx:PanelContent>
								</PanelCollection>
								<ContentPaddings Padding="2px" />
								<Border BorderColor="#68AFFD" BorderStyle="Solid" BorderWidth="1px" />
								<HeaderStyle BackColor="#68AFFD" Font-Bold="True"  ForeColor="White">
									<Border BorderStyle="None" />
									<BorderBottom BorderStyle="None" />
								</HeaderStyle>
								
							</dx:ASPxRoundPanel>
							<br />
							<dx:ASPxRoundPanel ID="rp_legend" runat="server"  cssclass="panel" HeaderText="Legend" BackColor="White"  ContentHeight="210px">
								<PanelCollection>
									<dx:PanelContent runat="server">
										<div class="legend">
											<div class='r'><span class="icon b" style="background-color: #f60;">&nbsp;</span>Due within a week</div>
											<div class='r'><span class="icon b" style="background-color: #f00;">&nbsp;</span>Over Due</div>
											<div class='r'><span class="icon b" style="background-color: #fff;">NA</span>Not Assigned</div>
											<div class='r'><span class="icon b" style="background-color: #ffc;">A</span>Assigned</div>
											<div class='r'><span class="icon b" style="background-color: #9c6;">R</span>Read</div>
											<div class='r'><span class="icon b" style="background-color: #0cf;">IP</span>In Progress</div>
											<div class='r'><span class="icon w" style="background-color: #00f;">WU</span>Waiting for User to Respond</div>
											<div class='r'><span class="icon b" style="background-color: #0f0;">AU</span>Answered by User</div>
											<div class='r'><span class="icon w" style="background-color: #f3c;">OC</span>Waiting for Opener to Close</div>
										    <div class='r'><span class="icon w" style="background-color: #ee8e4a;color:#fff;">BR</span>Waiting for Beta Release</div><div class='r'><span class="icon w" style="background-color: #808;color:#fff;">FR</span>Waiting for Final Release</div>
											<div class='r'><span class="icon w" style="background-color: #000;">C</span>Closed</div>
										</div>
									</dx:PanelContent>
								</PanelCollection>
								<ContentPaddings Padding="2px" />
								<Border BorderColor="#68AFFD" BorderStyle="Solid" BorderWidth="1px" />
								<HeaderStyle BackColor="#68AFFD" Font-Bold="True"  ForeColor="White">
									<Border BorderStyle="None" />
									<BorderBottom BorderStyle="None" />
								</HeaderStyle>
								
							</dx:ASPxRoundPanel>
							<br />
							<asp:HiddenField ID="hdnmemid" runat="server" />

							<asp:SqlDataSource runat="server"
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
								SelectCommand="Select 0 as member_id, 'Not Assigned' as member_name UNION Select member_id, member_fullname member_name from member where member_status = 'Active' order by member_name"
								ID="SqlDataSource5"></asp:SqlDataSource>

						</td>
						<td width="22%" class="column">
							<dx:ASPxRoundPanel ID="rp_waitingonme" runat="server" ContentHeight="300px" HeaderText="Waiting for Me" BackColor="White" cssclass="panel">
								<HeaderStyle BackColor="#00FF00">
									<Border BorderColor="#CCFFCC" />
									<BorderBottom BorderStyle="None" />
								</HeaderStyle>
								<PanelCollection>
									<dx:PanelContent runat="server">
										<dx:ASPxGridView ID="gvinmycourt" runat="server" AutoGenerateColumns="False"
											OnHtmlDataCellPrepared="gvinmycourt_HtmlDataCellPrepared" Width="100%"
											 KeyFieldName="ticketheader_id" OnHtmlRowCreated="gv_HtmlRowCreated" styles-cell-paddings-padding="2px" cssclass="gv">
											<Columns>
												<dx:GridViewDataTextColumn FieldName="priority_id" Name="ind"
													 VisibleIndex="0" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn FieldName="status_id" VisibleIndex="1" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Name="ticket" VisibleIndex="2" Width="100%" FieldName="ticket">
													<PropertiesTextEdit DisplayFormatString="{0}">
													</PropertiesTextEdit>
													<DataItemTemplate>
														<dx:ASPxLabel ID="ASPxLabel2" runat="server" OnDataBound="ASPxLabel2_DataBound"
															Text='<%# "0:" + Eval("ticket") %>' Width="100%" >
														</dx:ASPxLabel>
													</DataItemTemplate>
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsBehavior AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
											<SettingsPager Visible="False" Mode="ShowAllRecords" />
											<Settings VerticalScrollableHeight="280" VerticalScrollBarMode="Auto" ShowColumnHeaders="False" />
										</dx:ASPxGridView>
									</dx:PanelContent>
								</PanelCollection>
								<Border BorderColor="#CCFFCC" BorderStyle="Solid" BorderWidth="1px" />
							</dx:ASPxRoundPanel>
							<dx:ASPxRoundPanel ID="rp_waiting" runat="server" BackColor="White" ContentHeight="416px" HeaderText="Stuff Im Waiting For"  cssclass="panel">
								<HeaderStyle BackColor="#3626FB" ForeColor="White" VerticalAlign="Middle">
									<BorderBottom BorderStyle="None" />
								</HeaderStyle>
								<PanelCollection>
									<dx:PanelContent runat="server">
										<dx:ASPxGridView ID="gvistarted" runat="server" AutoGenerateColumns="False" KeyFieldName="ticketheader_id" OnHtmlDataCellPrepared="gvinmycourt_HtmlDataCellPrepared" Width="100%" OnHtmlRowCreated="gv_HtmlRowCreated" cssclass="gv">
											<Columns>
												<dx:GridViewDataTextColumn  FieldName="priority_id" Name="ind" VisibleIndex="0" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn  FieldName="status_id"
													 VisibleIndex="1" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn  FieldName="ticket" Name="ticket" VisibleIndex="3" >
													<PropertiesTextEdit DisplayFormatString="{0}">
													</PropertiesTextEdit>
													<DataItemTemplate>
														<dx:ASPxLabel ID="ASPxLabel4" runat="server" OnDataBound="ASPxLabel2_DataBound" Text='<%# "1:" + Eval("ticket") %>' Width="100%" >
														</dx:ASPxLabel>
													</DataItemTemplate>
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsPager Mode="ShowAllRecords" Visible="False" />
											<Settings ShowColumnHeaders="False" VerticalScrollableHeight="375" VerticalScrollBarMode="Auto" />
										</dx:ASPxGridView>
									</dx:PanelContent>
								</PanelCollection>
								<Border BorderColor="#3626FB" BorderStyle="Solid" BorderWidth="1px" />
							</dx:ASPxRoundPanel>
						</td>
						<td width="22%" class="column">
							<dx:ASPxRoundPanel ID="rp_workingon" runat="server"
								HeaderText="Stuff Im Working On"  cssclass="panel" ContentHeight="750px"
								 BackColor="White">
								<HeaderStyle BackColor="#00FFFF">
									<Border BorderColor="#FFD379" />
									<BorderBottom BorderStyle="None" />
								</HeaderStyle>
								<PanelCollection>
									<dx:PanelContent runat="server">
										<dx:ASPxGridView ID="gv_progres" runat="server" AutoGenerateColumns="False"
											 KeyFieldName="ticketheader_id" OnHtmlRowCreated="gv_HtmlRowCreated" cssclass="gv"
											OnHtmlDataCellPrepared="gvinmycourt_HtmlDataCellPrepared" Width="100%">
											<Columns>
												<dx:GridViewDataTextColumn  FieldName="priority_id" Name="ind"
													 VisibleIndex="0" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn  FieldName="status_id" VisibleIndex="1" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn  FieldName="ticket" Name="ticket" VisibleIndex="3" Width="100%">
													<PropertiesTextEdit DisplayFormatString="{0}">
													</PropertiesTextEdit>
													<DataItemTemplate>
														<dx:ASPxLabel ID="ASPxLabel7" runat="server" OnDataBound="ASPxLabel2_DataBound" Width="100%" >
														</dx:ASPxLabel>
													</DataItemTemplate>
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsBehavior AllowSelectByRowClick="True"
												AllowSelectSingleRowOnly="True" />
											<SettingsPager Mode="ShowAllRecords" Visible="False">
											</SettingsPager>
											<Settings ShowColumnHeaders="False" VerticalScrollableHeight="700"
												VerticalScrollBarMode="Auto" />
										</dx:ASPxGridView>
									</dx:PanelContent>
								</PanelCollection>
								<Border BorderColor="#00FFFF" BorderStyle="Solid" BorderWidth="1px" />
							</dx:ASPxRoundPanel>
						</td>
						<td width="22%" class="column">
							<dx:ASPxRoundPanel ID="rp_myresponsiblity" runat="server" HeaderText="My Responsibility (All Dates)" cssclass="panel" ContentHeight="750px"  Width="100%"  BackColor="White">
								<HeaderStyle BackColor="#A8D3FF">
									<BorderBottom BorderStyle="None" />
								</HeaderStyle>
								<PanelCollection>
									<dx:PanelContent runat="server">
										<dx:ASPxGridView ID="gvcadence" runat="server" AutoGenerateColumns="False"  KeyFieldName="ticketheader_id" OnHtmlRowCreated="gv_HtmlRowCreated" cssclass="gv" OnHtmlDataCellPrepared="gvinmycourt_HtmlDataCellPrepared" Width="100%">
											<Columns>
												<dx:GridViewDataTextColumn FieldName="priority_id" Name="ind" VisibleIndex="0" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn FieldName="status_id" VisibleIndex="1" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn FieldName="ticket" Name="ticket" VisibleIndex="2" width="90%">
													<DataItemTemplate>
														<dx:ASPxLabel ID="ASPxLabel5" runat="server" OnDataBound="ASPxLabel2_DataBound" Width="100%" />
													</DataItemTemplate>
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsBehavior AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
											<SettingsPager Mode="ShowAllRecords" Visible="False" />
											<Settings ShowColumnHeaders="False" VerticalScrollableHeight="700" VerticalScrollBarMode="Auto" />
										</dx:ASPxGridView>
									</dx:PanelContent>
								</PanelCollection>
								<Border BorderColor="#A8D3FF" BorderStyle="Solid" BorderWidth="1px" />
							</dx:ASPxRoundPanel>
						</td>
						<td width="22%" class="column">
							<dx:ASPxRoundPanel ID="rp_closedtickets" runat="server" HeaderText="My Tickets Closed Last Week" OnHtmlRowCreated="gv_HtmlRowCreated" cssclass="panel" ContentHeight="300px" Width="100%"  BackColor="White">
								<HeaderStyle BackColor="#DEDEDE">
									<BorderBottom BorderStyle="None" />
								</HeaderStyle>
								<PanelCollection>
									<dx:PanelContent runat="server">
										<dx:ASPxGridView ID="gv_closed0" runat="server" AutoGenerateColumns="False" KeyFieldName="ticketheader_id" OnHtmlDataCellPrepared="gvinmycourt_HtmlDataCellPrepared" OnHtmlRowCreated="gv_HtmlRowCreated" Width="100%">
											<Columns>
												<dx:GridViewDataTextColumn  FieldName="priority_id" Name="ind" VisibleIndex="0" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn  FieldName="status_id" VisibleIndex="1" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn  FieldName="ticket" Name="ticket" VisibleIndex="3" width="84%">
													<PropertiesTextEdit DisplayFormatString="{0}">
													</PropertiesTextEdit>
													<DataItemTemplate>
														<dx:ASPxLabel ID="ASPxLabel8" runat="server" OnDataBound="ASPxLabel2_DataBound" Width="100%" >
														</dx:ASPxLabel>
													</DataItemTemplate>
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsPager Mode="ShowAllRecords" Visible="False">
											</SettingsPager>
											<Settings ShowColumnHeaders="False" VerticalScrollableHeight="280"
												VerticalScrollBarMode="Auto" />
										</dx:ASPxGridView>
									</dx:PanelContent>
								</PanelCollection>
								<Border BorderColor="#8B8B8B" BorderStyle="Solid" BorderWidth="1px" />
							</dx:ASPxRoundPanel>
							<dx:ASPxRoundPanel ID="rp_watching" runat="server" HeaderText="Other Tickets Im Watching"
								 cssclass="panel" ContentHeight="300px"  BackColor="White">
								<HeaderStyle BackColor="#DEDEDE">
									<BorderBottom BorderStyle="None" />
								</HeaderStyle>
								<PanelCollection>
									<dx:PanelContent runat="server">
										<dx:ASPxGridView ID="gv_watching" runat="server" AutoGenerateColumns="False" OnHtmlRowCreated="gv_HtmlRowCreated"
											 KeyFieldName="ticketheader_id"
											OnHtmlDataCellPrepared="gvinmycourt_HtmlDataCellPrepared" Width="100%">
											<Columns>
												<dx:GridViewDataTextColumn  FieldName="priority_id" Name="ind"
													 VisibleIndex="0" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn  FieldName="status_id"
													 VisibleIndex="1" width="23px" minwidth="20">
													<CellStyle HorizontalAlign="Center">
														<Paddings Padding="0px" />
													</CellStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn  FieldName="ticket" Name="ticket"
													 VisibleIndex="3" Width="100%">
													<PropertiesTextEdit DisplayFormatString="{0}">
													</PropertiesTextEdit>
													<DataItemTemplate>
														<dx:ASPxLabel ID="ASPxLabel6" runat="server" OnDataBound="ASPxLabel2_DataBound"
															Text='<%# "5:" + Eval("ticket") %>' Width="100%" >
														</dx:ASPxLabel>
													</DataItemTemplate>
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsBehavior AllowSelectByRowClick="True"
												AllowSelectSingleRowOnly="True" />
											<SettingsPager Mode="ShowAllRecords" Visible="False">
											</SettingsPager>
											<Settings ShowColumnHeaders="False" VerticalScrollableHeight="385"
												VerticalScrollBarMode="Auto" />
										</dx:ASPxGridView>
									</dx:PanelContent>
								</PanelCollection>
								<Border BorderColor="#8B8B8B" BorderStyle="Solid" BorderWidth="1px" />
							</dx:ASPxRoundPanel>
						</td>
					</tr>

				</table>

				<dx:ASPxPopupControl ID="popsearch" runat="server"
					ClientInstanceName="popsearch" HeaderText="Search Results" Modal="True"
					PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
					Width="900px" ShowPageScrollbarWhenModal="True" AllowDragging="True"
					Theme="NETheme01">
					<ModalBackgroundStyle Opacity="0">
					</ModalBackgroundStyle>
					<ContentCollection>
						<dx:PopupControlContentControl runat="server">
							<dx:ASPxGridView ID="gv_search" runat="server" AutoGenerateColumns="False"  KeyFieldName="ticketheader_id"
								Width="100%" ClientInstanceName="gv_search"
								OnBeforeColumnSortingGrouping="gv_search_BeforeColumnSortingGrouping"
								OnCustomCallback="gv_search_CustomCallback"
								OnPageIndexChanged="gv_search_PageIndexChanged" Theme="NETheme01">
								<ClientSideEvents EndCallback="function(s, e) {
	if(s.cpredir != &quot;&quot;)
		{
		var ticket_id = memsearch.GetText();
		boing(&quot;ticketpage.aspx?issue=&quot;+ticket_id, &quot;ticket&quot;+ticket_id, 1100, 750); 
		popsearch.Hide();
		memsearch.SetText('');
		}
}" />
								<Columns>
									<dx:GridViewCommandColumn  Visible="False"
										VisibleIndex="0" Width="2px">
										
									</dx:GridViewCommandColumn>
									<dx:GridViewDataHyperLinkColumn Caption="Ticket" FieldName="RaisedIssue"
										 VisibleIndex="1" Width="100%">
										<Settings AutoFilterCondition="Contains" />
										<DataItemTemplate>
											<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
												NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/tickets/ticketpage.aspx?issue={0}', 'tickets1',800,750)&quot;,Eval(&quot;ticketheader_id&quot;)) %>"
												Text='<%# Eval("RaisedIssue") %>'>
											</dx:ASPxHyperLink>
										</DataItemTemplate>
									</dx:GridViewDataHyperLinkColumn>
									<dx:GridViewDataDateColumn FieldName="DateCreated"
										 VisibleIndex="2" Width="80px">
										<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
										</PropertiesDateEdit>
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataDateColumn>
									<dx:GridViewDataTextColumn Caption="Pertaining To" FieldName="PageName"
										 VisibleIndex="3" Width="80px">
									    <Settings HeaderFilterMode="CheckedList" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn FieldName="Status" 
										VisibleIndex="4" Width="60px">
									    <Settings HeaderFilterMode="CheckedList" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Type" FieldName="TypeOfTicket"
										 VisibleIndex="5" Width="60px">
									    <Settings HeaderFilterMode="CheckedList" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Assigned To" FieldName="AsignedTo"
										 VisibleIndex="6" Width="60px">
									    <Settings HeaderFilterMode="CheckedList" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn FieldName="Priority" 
										VisibleIndex="7" Width="60px">
									    <Settings HeaderFilterMode="CheckedList" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataProgressBarColumn FieldName="Relevance" Name="Relevance"
										ReadOnly="True"  VisibleIndex="8" Width="70px">
										<PropertiesProgressBar CustomDisplayFormat="" Height="" Width="60px">
											<Style ForeColor="#FF8000">
					</Style>
										</PropertiesProgressBar>
										<CellStyle HorizontalAlign="Center">
										</CellStyle>
									</dx:GridViewDataProgressBarColumn>
									<dx:GridViewDataTextColumn Caption="Group" FieldName="groupname"
										 VisibleIndex="9" Width="50px">
										<Settings HeaderFilterMode="CheckedList" />
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
								</Columns>
								<SettingsBehavior ColumnResizeMode="Control" />
								<Settings ShowFilterRow="True" ShowGroupPanel="True"
									ShowHeaderFilterButton="True" UseFixedTableLayout="True" ShowFilterRowMenu="True" />
								<SettingsSearchPanel Visible="True" />
								<Styles>
									<Header ImageSpacing="5px" SortingImageSpacing="5px">
									</Header>
									<LoadingPanel ImageSpacing="10px">
									</LoadingPanel>
								</Styles>
								<StylesEditors>
									<ProgressBar Height="25px">
									</ProgressBar>
								</StylesEditors>
							</dx:ASPxGridView>
						</dx:PopupControlContentControl>
					</ContentCollection>
				</dx:ASPxPopupControl>

				<br />
				<asp:HiddenField ID="hidVMI" runat="server" />
				<asp:HiddenField ID="hidAdminAction" runat="server" />
				<dx:ASPxPopupControl ID="popman" runat="server" ClientInstanceName="popman"
					HeaderText="Manager Settings" AppearAfter="0" Modal="True" PopupHorizontalAlign="WindowCenter"
					PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True"
					AllowDragging="True" PopupAnimationType="None" Theme="NETheme01">
					<ModalBackgroundStyle Opacity="0">
					</ModalBackgroundStyle>
					<ContentCollection>
						<dx:PopupControlContentControl runat="server">

							<table style="width: 100%;">
								<tr>
									<td colspan="2">&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td colspan="3">
										<dx:ASPxGridView ID="ASPxGridView2" runat="server" AutoGenerateColumns="False"
											 KeyFieldName="ticketmanager_id"
											OnRowDeleting="ASPxGridView2_RowDeleting"
											OnRowInserting="ASPxGridView2_RowInserting" Width="450px" Theme="NETheme01">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
											<Columns>
												<dx:GridViewCommandColumn ButtonType="Image" ShowClearFilterButton="true" ShowDeleteButton="true" ShowNewButton="true" 
													 VisibleIndex="0" Width="60px">
													
													
													
													<CellStyle Wrap="False">
													</CellStyle>
												</dx:GridViewCommandColumn>
												<dx:GridViewDataComboBoxColumn Caption="Ticket Assignees"
													FieldName="ticketmanager_member_id" 
													SortIndex="1" SortOrder="Ascending" VisibleIndex="1" Width="125px">
													<PropertiesComboBox DataSourceID="SqlDataSource2" AnimationType="None"
														EnableCallbackMode="True" IncrementalFilteringMode="StartsWith"
														TextField="membername" ValueField="member_id" ValueType="System.Int32">
													</PropertiesComboBox>
													<Settings SortMode="DisplayText" />
													<CellStyle Wrap="False">
													</CellStyle>
												</dx:GridViewDataComboBoxColumn>
												<dx:GridViewDataComboBoxColumn Caption="Group"
													FieldName="ticketmanager_group_id"  SortIndex="0"
													SortOrder="Ascending" VisibleIndex="2" Width="150px">
													<PropertiesComboBox DataSourceID="SqlDataSource4" TextField="ticket_group_name"
														ValueField="ticket_group_id" ValueType="System.Int32">
													</PropertiesComboBox>
													<Settings SortMode="DisplayText" />
													<CellStyle Wrap="False">
													</CellStyle>
												</dx:GridViewDataComboBoxColumn>
											</Columns>
											<SettingsPager Mode="ShowAllRecords">
											</SettingsPager>
											<Settings ShowFilterRow="True" ShowHeaderFilterBlankItems="False" ShowFilterBar="Visible" />
											<Templates>
												<EditForm>
													<dx:ASPxGridViewTemplateReplacement ID="Editors" runat="server"
														ReplacementType="EditFormEditors" />
													<div style="margin-top: 10px">
														<div style="float: left; margin-left: 5px">
															<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false"
																ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>'
																Text="Cancel" Width="100px" />
														</div>
														<div style="float: right; margin-right: 5px">
															<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false"
																ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>'
																CssClass="input" Text="Add" Width="100px" />
														</div>
													</div>
												</EditForm>
											</Templates>
										</dx:ASPxGridView>
										<asp:SqlDataSource ID="SqlDataSource4" runat="server"
											ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
											ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
										<asp:SqlDataSource ID="SqlDataSource2" runat="server"
											ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
											ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
											SelectCommand="Select member_fullname as membername,member_id from member where member_status = 'Active' order by member_fullname"></asp:SqlDataSource>
									</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td colspan="2">&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td nowrap="nowrap">
										<dx:ASPxButton ID="btn_save" runat="server" OnClick="btn_save_Click"
											Text="Save" Width="80px">
										</dx:ASPxButton>
									</td>
									<td nowrap="nowrap">
										<dx:ASPxButton ID="btnCancel" runat="server"
											Text="Cancel" Width="80px">
											<ClientSideEvents Click="function(s, e) {
	popman.Hide();
}" />
										</dx:ASPxButton>
									</td>
									<td width="100%">&nbsp;</td>
									<td width="100%">&nbsp;</td>
								</tr>
							</table>
						</dx:PopupControlContentControl>
					</ContentCollection>
				</dx:ASPxPopupControl>

			</div>
		
		</ContentTemplate>
	</asp:UpdatePanel>
<div class="wiki_help_class" id="wiki_help" runat="server" visible="true">
    <img alt="Help for this page" src='/images/icon/icon[help].gif' width='20' height='20' title="Help available for this page" id="wiki_help_img" runat="server" />
</div>
</asp:Content>

