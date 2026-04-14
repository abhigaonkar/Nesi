<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_hr_member_modules_uc_member_usage" EnableTheming="true" Codebehind="uc_member_usage.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>














<style type="text/css">
	.style1
	{
		height: 23px;
	}
	.style3
	{
		height: 267px;
	}
	.style4
	{
		height: 18px;
	}
</style>


<dx:ASPxCallbackPanel ID="usage_cb" runat="server" 
	ClientInstanceName="usage_cb" oncallback="usage_cb_Callback" Width="100%">
	<ClientSideEvents EndCallback="function(s, e) {
gv.SetVisible(true);
if (s.cp_reload_parent==&quot;1&quot;)
{	
	if(parent!=null &amp;&amp; parent.frames.length&gt;0)
		{
		//	parent.location.href=parent.location.href;
		//	parent.set_tabs();
		}
//	parent.opener.location.href = parent.opener.location.href;
}
}" />
	<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table style="width:100%;">
		<tr>
			<td style="text-align: left" width="100%">
				<dx:ASPxLabel ID="lbl_title" runat="server" Font-Names="Arial" Font-Size="16pt" 
					Text="ASPxLabel" Theme="NETheme01">
				</dx:ASPxLabel>
			</td>
			<td style="text-align: left">
				<dx:ASPxButton ID="ASPxButton1" runat="server" OnClick="ASPxButton1_Click" 
					Text="Refresh">
				</dx:ASPxButton>
			</td>
		</tr>
		<tr>
			<td colspan="2" valign="top">
				<dx:ASPxTabControl ID="tc" runat="server" ActiveTabIndex="25" 
					Font-Names="Arial" OnActiveTabChanged="tc_ActiveTabChanged" 
					Width="100%">
					<Tabs>
						<dx:Tab Name="Assigned Tickets">
						</dx:Tab>
						<dx:Tab Name="WOs">
						</dx:Tab>
						<dx:Tab Name="POs">
						</dx:Tab>
						<dx:Tab Name="Reports To">
						</dx:Tab>
						<dx:Tab Name="Payroll Handler">
						</dx:Tab>
						<dx:Tab Name="Quotes">
						</dx:Tab>
						<dx:Tab Name="Account Manager">
						</dx:Tab>
						<dx:Tab Name="Project Manager">
						</dx:Tab>
						<dx:Tab Name="Controls">
						</dx:Tab>
						<dx:Tab Name="ISR">
						</dx:Tab>
						<dx:Tab Name="OSR">
						</dx:Tab>
						<dx:Tab Name="RAM">
						</dx:Tab>
						<dx:Tab Name="MAM">
						</dx:Tab>
						<dx:Tab Name="CISR">
						</dx:Tab>					
						<dx:Tab Name="Quote Process">
						</dx:Tab>
						<dx:Tab Name="Offers Editing">
						</dx:Tab>
						<dx:Tab Name="Reviews to do">
						</dx:Tab>
						<dx:Tab Name="Scheduler">
						</dx:Tab>
						<dx:Tab Name="FAQs">
						</dx:Tab>
						<dx:Tab Name="Cell Phones">
						</dx:Tab>
						<dx:Tab Name="Messageboard">
						</dx:Tab>
						<dx:Tab Name="Inv Locations">
						</dx:Tab>
						<dx:Tab Name="Assets">
						</dx:Tab>
						<dx:Tab Name="RFQs">
						</dx:Tab>
						<dx:Tab Name="WO Freeze">
						</dx:Tab>
						<dx:Tab Name="Ticket Group Man">
						</dx:Tab>
                        <dx:Tab Name="PO Approval Level">
						</dx:Tab>
					</Tabs>
					<ClientSideEvents ActiveTabChanged="function(s, e) {
gv.SetVisible(false);
	usage_cb.PerformCallback();

}" />
					<ActiveTabStyle BackColor="#C8E7FD">
						<Border BorderStyle="None" />
					</ActiveTabStyle>
					<TabStyle BackColor="#EEEEEE">
						<HoverStyle BackColor="#D2D2D2">
						</HoverStyle>
						<Border BorderStyle="None" />
					</TabStyle>
					<BorderBottom BorderStyle="None" />
				</dx:ASPxTabControl>
				<dx:ASPxPanel ID="ASPxPanel1" runat="server" Theme="NETheme01" Width="100%">
					<PanelCollection>
						<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
							<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
								ClientInstanceName="gv" KeyFieldName="id" OnCustomCallback="gv_CustomCallback" 
								Theme="NETheme01" Width="100%">
								<Columns>
									<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
										ShowSelectCheckbox="True" VisibleIndex="1" Width="200px" SelectAllCheckboxMode="AllPages">
									</dx:GridViewCommandColumn>
									<dx:GridViewDataTextColumn Caption=" " FieldName="text" 
										ShowInCustomizationForm="True" VisibleIndex="2">
										<DataItemTemplate>
											<dx:ASPxHyperLink ID="hl_stuff" runat="server" Font-Names="Arial" 
												Font-Size="8pt" 
												NavigateUrl="<%# string.Format(&quot;javascript:boing('{0}', 'stuff', 1100,800)&quot;, Eval(&quot;link&quot;)) %>" 
												Text='<%# Eval("text") %>' Theme="NETheme01">
											</dx:ASPxHyperLink>
										</DataItemTemplate>
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
										ShowInCustomizationForm="True" Visible="False" VisibleIndex="0">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="link" FieldName="link" 
										ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
									</dx:GridViewDataTextColumn>
								</Columns>
								<SettingsBehavior EnableRowHotTrack="True" AllowSelectByRowClick="True" />
								<SettingsPager Mode="ShowAllRecords">
								</SettingsPager>
								<Settings ShowFooter="True" />
								<SettingsDataSecurity AllowDelete="False" AllowEdit="False" 
									AllowInsert="False" />
								<Styles>
									<CommandColumn Spacing="5px">
									</CommandColumn>
								</Styles>
								<Templates>
									<FooterRow>
										<table align="left" style="width:100%;">
											<tr>
												<td style="font-family: Arial">
													With Selected Items:</td>
												<td>
													<dx:ASPxButton ID="btnclose" runat="server" AutoPostBack="False" 
														oninit="btnclose_Init" Text="Close" Theme="NETheme01" Width="100px">
														<ClientSideEvents Click="function(s, e) {
if (confirm('Are you sure you want to close the selected items?'))
{
usage_cb.PerformCallback('close');
}
}" />
													</dx:ASPxButton>
												</td>
												<td>
													&nbsp;</td>
											</tr>
											<tr>
												<td>
													<dx:ASPxComboBox ID="ddl" runat="server" ClientInstanceName="ddl" 
														EnableCallbackMode="True" IncrementalFilteringMode="Contains" oninit="ddl_Init" 
														TextField="text" Theme="NETheme01" ValueField="id" ValueType="System.Int32">
													</dx:ASPxComboBox>
												</td>
												<td>
													<dx:ASPxButton ID="btnreassign" runat="server" AutoPostBack="False" 
														oninit="btnreassign_Init" Text="Reassign" Theme="NETheme01" Width="100px">
														<ClientSideEvents Click="function(s, e) {


if (ddl.GetText()!='')
{
	if (confirm('Are you sure you want to reassign the selected items?'))
{
gv.PerformCallback('Reassign|'+ ddl.GetValue());
}
}
else
{

alert('You must select someone to assign the selected items to');
}
}" />
													</dx:ASPxButton>
												</td>
												<td width="100%">
													&nbsp;</td>
											</tr>
											<tr>
												<td>
													&nbsp;</td>
												<td>
													<dx:ASPxButton ID="btnleave" runat="server" AutoPostBack="False" 
														ClientVisible="False" oninit="btnleave_Init" Text="Leave" Theme="NETheme01" 
														Width="100px">
														<ClientSideEvents Click="function(s, e) {
	if (confirm('Are you sure you want to leave the selected items?'))
{
usage_cb.PerformCallback('leave');
}
}" />
													</dx:ASPxButton>
												</td>
												<td>
													&nbsp;</td>
											</tr>
										</table>
									</FooterRow>
								</Templates>
							</dx:ASPxGridView>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxPanel>
				<br />
			</td>
		</tr>
		<tr>
			<td align="center" class="style1" colspan="2">
			</td>
		</tr>
		<tr>
			<td align="center" colspan="2" class="style4">
				<br />
			</td>
		</tr>
	</table>
		</dx:PanelContent>
</PanelCollection>
</dx:ASPxCallbackPanel>
<p>
	<br />
</p>


