<%@ Control Language="C#" AutoEventWireup="true" Inherits="mobile_modules_new_ticket" Codebehind="new_ticket.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:updatepanel id="up" runat="server" clientidmode="Static">
	<triggers>
		<asp:postbacktrigger controlid="bt_cutticket" />
	</triggers>
	<contenttemplate>
		<center>
		<link type="text/css" rel="stylesheet" href="/mobile/css/tickets.css" />
		<script type="text/javascript" src="/mobile/js/ticket.js"></script>
		<div id="ticket_created" runat="server" visible="false" class="ticket_created" align="center">
			<br />
			<div class="title">Thank you, ticket <%= ticket_id %> has been created.</div>
			<br />
			How would you like to proceed?
			<br />
			<br />

			<button class="button lg" type="button" onclick="mobile_ticket.load(<%= ticket_id %>, false);">Go to ticket <%= ticket_id %></button>
			<button class="button lg" type="button" onclick="mobile_ticket.new_ticket.go();">New Ticket</button>
			<button class="button lg" type="button" onclick="location.href='/mobile/index.aspx?a=tickets'">Your Tickets</button>
			<button class="button lg" type="button" onclick="location.href='/mobile/index.aspx'">Home Page</button>
		</div>
		<div id="new_ticket" class="new_ticket" visible="false" runat="server" align="center">
			<button class="button whitetext" type="button" onclick="location.href='/mobile/index.aspx?a=tickets'">Back</button>
			<div id="group_selector"><asp:dropdownlist ID="ddl_group" runat="server" autopostback="true" datatextfield="name" datavaluefield="id" CssClass="selector lg" ondatabound="ddl_group_DataBound" onselectedindexchanged="ddl_group_SelectedIndexChanged"></asp:dropdownlist></div>
			<div id="subgroup_selector"><asp:dropdownlist ID="ddl_subgroup" runat="server" autopostback="true" datatextfield="name" datavaluefield="id" enabled="false" CssClass="selector lg" ondatabound="ddl_subgroup_DataBound" onselectedindexchanged="ddl_subgroup_SelectedIndexChanged"></asp:dropdownlist></div>
			<div id="type_selector"><asp:dropdownlist ID="ddl_type" runat="server" CssClass="selector lg" datatextfield="name" datavaluefield="id" enabled="false" ondatabound="ddl_type_DataBound"></asp:dropdownlist></div>
			<div><asp:textbox type="text" id="tb_topic" class="textbox lg" runat="server" placeholder="Topic" /></div>
			<div><asp:button cssclass="whitetext aligncenter" id="bt_check" runat="server" onclick="bt_check_Click" text="Check for Related Tickets" /></div>
			<br />
			<div id="related_tickets" runat="server" visible="false">
				<dx:ASPxGridView ID="gv_related" runat="server" Width="100%" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared" enablecallbacks="false">
					<Columns>
						<dx:GridViewDataTextColumn Caption="#" FieldName="ordered_id" VisibleIndex="0" Width="1%">
							<DataItemTemplate>
								<div ID="bt" runat="server" class="ticket_button" onclick="<%# string.Format(&quot;mobile_ticket.load({0})&quot;, Eval(&quot;id&quot;)) %>"><%# Eval("id") %></div>
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
				<br />
				<div><asp:button cssclass="button whitetext" id="bt_startnew" runat="server" visible="false" text="No, this is a new ticket." onclick="bt_startnew_Click" /></div>
		</div>
			<br />
			<br />
			<div id="recent_tickets" runat="server" align="center">
				<div class="title lg">20 Most Recent (Open) Tickets</div>
					<dx:ASPxGridView ID="gv_recent" runat="server" Width="100%" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared" enablecallbacks="false">
					<Columns>
						<dx:GridViewDataTextColumn Caption="#" FieldName="ordered_id" VisibleIndex="0" Width="1%">
							<DataItemTemplate>
								<div ID="bt" runat="server" class="ticket_button" onclick="<%# string.Format(&quot;mobile_ticket.load({0})&quot;, Eval(&quot;id&quot;)) %>"><%# Eval("id") %></div>
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
						<settingstext emptydatarow="No recent tickets" />
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
			</div>
			<div id="fields" runat="server" class="fields" style="display:none;">
				<asp:textbox id="ticket_body" runat="server" textmode="MultiLine"  cssclass="ticket_body"></asp:textbox>
				<b class="lg">Add Attachment?: </b>&nbsp;<asp:fileupload id="ticket_file" runat="server" /><br /><br />
				<asp:checkbox id="chk_private" runat="server" text="Private Ticket?" cssclass="lg" />
				<br /><br /><br /><br />
				<asp:button id="bt_cutticket" runat="server" text="Cut Ticket" cssclass="whitetext aligncenter" onclientclick="return mobile_ticket.new_ticket.check();" onclick="bt_cutticket_Click" />
			</div>
		</div>
		<br /><br /><br /><br />
		</center>
	</contenttemplate>
</asp:updatepanel>