<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_tickets" EnableTheming="true" Codebehind="tickets.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />

<style type="text/css">
	html,body
		{
		width:			100%;
		height:			100%;
		}
	.gv_row td
		{
		border-top:	solid 2px #fff !important;
		}
	.gv_row td.issue
		{
		font-size:		1.25em;
		}
	.text_row
		{
		margin-top:		15px;
		margin-bottom:	5px;
		}
	.if_modal
		{
		width:				100%;
		height:				100%;
		position:			absolute;
		left:				0px;
		top:				0px;
		background-color:	#fff;
		display:			none;
		padding-top
		}
	.if_modal .if_header
		{
		padding-top:		10px;
		padding-bottom:		10px;
		}
	.if_modal .if_header button
		{
		font-size:			1.5em;
		width:				50%;
		}
	.if_modal #if_newticket,
	.if_modal #if_existingticket
		{
		background-color:	#fff;
		overflow-y:			auto;
		background-repeat:	no-repeat;
		width:				100%;
		height:				100%;
		position:			absolute;
		left:				0;
		top:				0;
		background-position: center	50px;
		}
	.title_row
		{
		font-size:			13pt;
		margin-top:			10px;
		width:				98%;
		background-color:	#c0c0c0;
		color:				#fff;
		font-weight:		bold;
		font-family:		arial;
		padding:			10px 0px 10px 0px;
		}
	.popup_row
		{

		}
</style>
<script type="text/javascript" src="/mobile/js/ticket.js"></script>
<dx:ASPxCallbackPanel ID="ticket_cbp" Width="100%" runat="server" ClientInstanceName="ticket_cbp" oncallback="ticket_cbp_Callback">
	<PanelCollection>
		<dx:PanelContent>
			<center>
		<div class="if_modal existing">
			<div class="if_header" align="center"><button type="button" class="white_text aligncenter" onclick="mobile_ticket.unload();">&lt; Back</button></div>
			<iframe frameborder="0" width="100%" height="100%" id="if_existingticket"></iframe>
		</div>
		<div class="if_modal new">
			<div class="if_header" align="center"><button type="button" class="white_text aligncenter" onclick="mobile_ticket.unload();">&lt; Back</button></div>
			<iframe frameborder="0" width="95%" height="100%" id="if_newticket"></iframe>
		</div>
			<dx:ASPxButton ID="bt_new" runat="server" AutoPostBack="false" cssclass="whitetext aligncenter" Text="New Ticket" native="true" Width="98%" Font-Size="1.5em" UseSubmitBehavior="false">
				<ClientSideEvents Click="function(s,e){mobile_ticket.new_ticket.go();}" />
			</dx:ASPxButton>
			<dx:ASPxTextBox ID="text_search" ClientInstanceName="text_search" runat="server" native="true" Width="98%" Font-Size="2em">
			</dx:ASPxTextBox>
			<dx:ASPxButton ID="bt_search" ClientInstanceName="bt_search" runat="server" cssclass="whitetext aligncenter" AutoPostBack="false" native="true" Text="Search" Width="98%" Font-Size="1.5em">
				<ClientSideEvents Click="mobile_ticket.search" />
			</dx:ASPxButton>
		<br />
			<dx:ASPxButton ID="bt_cancel" ClientVisible="false" ClientInstanceName="bt_cancel" native="true" runat="server" AutoPostBack="false" Text="Cancel Search" Width="98%" Font-Size="1.5em" UseSubmitBehavior="false">
				<ClientSideEvents Click="mobile_ticket.cancelsearch" />
			</dx:ASPxButton>
		<div class="title_row" id="waiting_title" runat="server">Tickets In My Court</div>
		<dx:ASPxGridView ID="gv_waiting" runat="server" Width="98%" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared">
			<Columns>
				<dx:GridViewDataTextColumn Caption="#" FieldName="ordered_id" VisibleIndex="0" Width="1%">
					<DataItemTemplate>
						<div ID="bt" runat="server" class="button" onclick="<%# string.Format(&quot;mobile_ticket.load({0}, true)&quot;, Eval(&quot;id&quot;)) %>"><%# Eval("id") %></div>
					</DataItemTemplate>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Issue" FieldName="issue" VisibleIndex="2" Width="99%">
					<DataItemTemplate>
						<div id="issue" runat="server">&nbsp;<%# Eval("issue") %></div>
						<div id="status" runat="server" style="font-size:0.75em; color: #777;">&nbsp;Status: <%# Eval("status") %> &nbsp;/&nbsp; Priority: <%# Eval("priority") %></div>
					</DataItemTemplate>
					<CellStyle CssClass="issue"></CellStyle>
				</dx:GridViewDataTextColumn>
			</Columns>
			<SettingsBehavior AllowDragDrop="False" />
			<SettingsPager NumericButtonCount="5" CurrentPageNumberFormat="{0}">
			</SettingsPager>
			<StylesPager>
				<CurrentPageNumber BackColor="SteelBlue" ForeColor="White">
					<Paddings PaddingBottom="3px" PaddingLeft="14px" PaddingRight="14px" PaddingTop="3px" />
				</CurrentPageNumber>
				<Pager Font-Size="12px">
				</Pager>
			</StylesPager>
			<Border BorderWidth="0px" />
			<Styles>
				<Row CssClass="gv_row">
				</Row>
				<Header BackColor="SteelBlue" Font-Bold="True" Font-Size="11px" ForeColor="White" HorizontalAlign="Center">
					<Border BorderColor="Silver" />
				</Header>
				<Cell>
					<Paddings Padding="2px" />
					<Border BorderWidth="0px" />
				</Cell>
				<PagerBottomPanel BackColor="White">
				</PagerBottomPanel>
			</Styles>
			<Border BorderWidth="0px"></Border>
		</dx:ASPxGridView>
		<div class="title_row" id="mytickets_title" runat="server">Tickets I Started</div>
		<dx:ASPxGridView ID="gv_mytickets" runat="server" Width="98%" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared">
			<Columns>
				<dx:GridViewDataTextColumn Caption="#" FieldName="ordered_id" VisibleIndex="0" Width="1%">
					<DataItemTemplate>
						<div ID="bt" runat="server" class="button" onclick="<%# string.Format(&quot;mobile_ticket.load({0}, true)&quot;, Eval(&quot;id&quot;)) %>"><%# Eval("id") %></div>
					</DataItemTemplate>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Issue" FieldName="issue" VisibleIndex="2" Width="99%">
					<DataItemTemplate>
						<div id="issue" runat="server">&nbsp;<%# Eval("issue") %></div>
						<div id="status" runat="server" style="font-size:0.75em; color: #777;">&nbsp;Status: <%# Eval("status") %> &nbsp;/&nbsp; Priority: <%# Eval("priority") %></div>
					</DataItemTemplate>
					<CellStyle CssClass="issue"></CellStyle>
				</dx:GridViewDataTextColumn>
			</Columns>
			<SettingsBehavior AllowDragDrop="False" />
			<SettingsPager NumericButtonCount="5" CurrentPageNumberFormat="{0}">
			</SettingsPager>
			<StylesPager>
				<CurrentPageNumber BackColor="SteelBlue" ForeColor="White">
					<Paddings PaddingBottom="3px" PaddingLeft="14px" PaddingRight="14px" PaddingTop="3px" />
				</CurrentPageNumber>
				<Pager Font-Size="12px">
				</Pager>
			</StylesPager>
			<Border BorderWidth="0px" />
			<Styles>
				<Row CssClass="gv_row">
				</Row>
				<Header BackColor="SteelBlue" Font-Bold="True" Font-Size="11px" ForeColor="White" HorizontalAlign="Center">
					<Border BorderColor="Silver" />
				</Header>
				<Cell>
					<Paddings Padding="2px" />
					<Border BorderWidth="0px" />
				</Cell>
				<PagerBottomPanel BackColor="White">
				</PagerBottomPanel>
			</Styles>
		</dx:ASPxGridView>
		<div class="title_row" id="newtickets_title" runat="server">Unassigned Tickets</div>
		<dx:ASPxGridView ID="gv_newtickets" runat="server" Width="98%" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared">
			<Columns>
				<dx:GridViewDataTextColumn Caption="#" FieldName="ordered_id" VisibleIndex="0" Width="1%">
					<DataItemTemplate>
						<div ID="bt" runat="server" class="button" onclick="<%# string.Format(&quot;mobile_ticket.load({0}, true)&quot;, Eval(&quot;id&quot;)) %>"><%# Eval("id") %></div>
					</DataItemTemplate>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Issue" FieldName="issue" VisibleIndex="2" Width="99%">
					<DataItemTemplate>
						<div id="issue" runat="server">&nbsp;<%# Eval("issue") %></div>
						<div id="status" runat="server" style="font-size:0.75em; color: #777;">&nbsp;Status: <%# Eval("status") %> &nbsp;/&nbsp; Priority: <%# Eval("priority") %></div>
					</DataItemTemplate>
					<CellStyle CssClass="issue"></CellStyle>
				</dx:GridViewDataTextColumn>
			</Columns>
			<SettingsBehavior AllowDragDrop="False" />
			<SettingsPager NumericButtonCount="5" CurrentPageNumberFormat="{0}">
			</SettingsPager>
			<StylesPager>
				<CurrentPageNumber BackColor="SteelBlue" ForeColor="White">
					<Paddings PaddingBottom="3px" PaddingLeft="14px" PaddingRight="14px" PaddingTop="3px" />
				</CurrentPageNumber>
				<Pager Font-Size="12px">
				</Pager>
			</StylesPager>
			<Border BorderWidth="0px" />
			<Styles>
				<Row CssClass="gv_row">
				</Row>
				<Header BackColor="SteelBlue" Font-Bold="True" Font-Size="11px" ForeColor="White" HorizontalAlign="Center">
					<Border BorderColor="Silver" />
				</Header>
				<Cell>
					<Paddings Padding="2px" />
					<Border BorderWidth="0px" />
				</Cell>
				<PagerBottomPanel BackColor="White">
				</PagerBottomPanel>
			</Styles>
		</dx:ASPxGridView>
		<div class="title_row" id="yourgroup_title" runat="server">Open Tickets From My Group</div>
		<dx:ASPxGridView ID="gv_yourgroup" runat="server" Width="98%" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared">
			<Columns>
				<dx:GridViewDataTextColumn Caption="#" FieldName="ordered_id" VisibleIndex="0" Width="1%">
					<DataItemTemplate>
						<div ID="bt" runat="server" class="button" onclick="<%# string.Format(&quot;mobile_ticket.load({0}, true)&quot;, Eval(&quot;id&quot;)) %>"><%# Eval("id") %></div>
					</DataItemTemplate>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Issue" FieldName="issue" VisibleIndex="2" Width="99%">
					<DataItemTemplate>
						<div id="issue" runat="server">&nbsp;<%# Eval("issue") %></div>
						<div id="status" runat="server" style="font-size:0.75em; color: #777;">&nbsp;Status: <%# Eval("status") %> &nbsp;/&nbsp; Priority: <%# Eval("priority") %></div>
					</DataItemTemplate>
					<CellStyle CssClass="issue"></CellStyle>
				</dx:GridViewDataTextColumn>
			</Columns>
			<SettingsBehavior AllowDragDrop="False" />
			<SettingsPager NumericButtonCount="5" CurrentPageNumberFormat="{0}">
			</SettingsPager>
			<StylesPager>
				<CurrentPageNumber BackColor="SteelBlue" ForeColor="White">
					<Paddings PaddingBottom="3px" PaddingLeft="14px" PaddingRight="14px" PaddingTop="3px" />
				</CurrentPageNumber>
				<Pager Font-Size="12px">
				</Pager>
			</StylesPager>
			<Border BorderWidth="0px" />
			<Styles>
				<Row CssClass="gv_row">
				</Row>
				<Header BackColor="SteelBlue" Font-Bold="True" Font-Size="11px" ForeColor="White" HorizontalAlign="Center">
					<Border BorderColor="Silver" />
				</Header>
				<Cell>
					<Paddings Padding="2px" />
					<Border BorderWidth="0px" />
				</Cell>
				<PagerBottomPanel BackColor="White">
				</PagerBottomPanel>
			</Styles>
		</dx:ASPxGridView>

			</center>
		</dx:PanelContent>
	</PanelCollection>
</dx:ASPxCallbackPanel>