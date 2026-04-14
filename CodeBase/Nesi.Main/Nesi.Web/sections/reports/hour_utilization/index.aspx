<%@ Page Language='C#' MasterPageFile='../../../IntraDefault.master' AutoEventWireup='true' Inherits='sections_reports_hour_utilization_index' Title='Hour Utilization' EnableTheming="true" Codebehind="index.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' runat='Server'>
	<div id='divSide' runat='server'>
		<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
	</div>
</asp:Content>
<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' runat='Server'>
	<div id='divMenu' runat='server'>
	</div>
</asp:Content>
<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' runat='Server'>
	<asp:ScriptManager runat="server" ID="sm"></asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
		
	<script type="text/javascript">

		document.getElementsByClassName = function (cl)
		{
			var retnode = [];
			var myclass = new RegExp('\\b' + cl + '\\b');
			var elem = this.getElementsByTagName('*');
			for (var i = 0; i < elem.length; i++)
			{
				var classes = elem[i].className;
				if (myclass.test(classes)) retnode.push(elem[i]);
			}
			return retnode;
		};


		function ToggleRow(id)
		{
			$("."+id).each(function()
				{
				var current_disp	= $(this).css("display");
				if(current_disp == "none")
					{
					$(this).show(0);
					}
				else
					{
					$(this).hide(0);
					}
				});
		}
		$(document).ready(function () {
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
		});
		function EndReqHandler() {
			please_wait("stop");
		}
		function BeginReqHandler() {
			please_wait("start");
		}
	</script>
	<asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Height="20px" Width="231px"></asp:Label>
	<br />
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id,name from business_unit  where active = 'T' ORDER BY name"></asp:SqlDataSource>
	<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" Width="100%" Theme="NETheme01" AutoPostBack="True" OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged" EnableCallBacks="True">
		<TabPages>
			<dx:TabPage Text="By Customer">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table style="width: 100%;" >
							<tr>
								<td style="padding-left: 10px">
									Starting Date
								</td>
								<td>
									<dx:ASPxDateEdit ID="dte_start_by_customer" runat="server" AllowUserInput="False" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" ></dx:ASPxDateEdit>
								</td>
								<td style="padding-left: 10px">
									Ending Date
								</td>
								<td >
									<dx:ASPxDateEdit ID="dte_end_by_customer" runat="server" AllowUserInput="False" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd"></dx:ASPxDateEdit>
								</td>
								<td width="100%">
									&nbsp;</td>
							</tr>
							<tr>
								<td nowrap="nowrap" style="padding-left: 10px">
									<asp:Label ID="lblCompany" runat="server" Text="Select Business Unit"></asp:Label>
								</td>
								<td>
									<asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="True" DataTextField="name" DataValueField="id" Enabled="False" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
									</asp:DropDownList>
								</td>
								<td align="left" nowrap="nowrap" style="padding-left: 10px">
								    <asp:Label ID="lblEmployee" runat="server" Text="Select Employee"></asp:Label>
								</td>
								<td nowrap="nowrap">
									<asp:DropDownList ID="ddlUsers" runat="server" DataTextField="member_fullname" DataValueField="member_id">
                                    </asp:DropDownList>
								</td>
								<td width="100%">
									&nbsp;</td>
								<td>
									&nbsp;
								</td>
								<td>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td colspan="5" style="padding-left: 10px">
									<asp:Label ID="lblCurrentView" runat="server" Font-Italic="False" Font-Size="14pt" Width="439px" Font-Names="Calibri"></asp:Label>
									<br />
									- Does not include Co-Op Students Time<br />- No time has been added for branch managers time...
								</td>
								<td>
									&nbsp;
								</td>
								<td>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:Button ID="bt_submit" runat="server" OnClick="bt_submit_Click" Style="border-top-style: dotted; border-right-style: dotted; border-left-style: dotted; border-bottom-style: dotted" Text="Go -&gt;" />
								</td>
								<td>
									&nbsp;
								</td>
								<td>
									&nbsp;
								</td>
								<td>
									&nbsp;
								</td>
								<td>
									&nbsp;
								</td>
								<td>
									&nbsp;
								</td>
							</tr>
						</table>
						<table border="0" cellpadding="0" cellspacing="0" class="framework">
							<tr>
								<td id="Td1" runat="server" align="center" valign="top">
								</td>
							</tr>
							<tr>
								<td valign='top' align='center' id='detail' runat='server'>
								</td>
							</tr>
						</table>
						<br />
						<table border="0" cellpadding="0" cellspacing="0" class="framework">
							<tr>
								<td id="wos" runat="server" align="center" valign="top">
								</td>
							</tr>
						</table>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="By Member">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<table>
							<tr>
								<td>
									&nbsp;
								</td>
								<td nowrap="nowrap">
									Starting Date
								</td>
								<td>
									<dx:ASPxDateEdit ID="dte_start_by_member" runat="server" ClientInstanceName="dtestart" DisplayFormatString="yyyy-MM-dd" AnimationType="None">
									</dx:ASPxDateEdit>
								</td>
								<td>
									&nbsp;
								</td>
								<td nowrap="nowrap">
									Ending Date
								</td>
								<td>
									<dx:ASPxDateEdit ID="dt_end_by_member" runat="server" ClientInstanceName="dteEnd" DisplayFormatString="yyyy-MM-dd" AnimationType="None">
									</dx:ASPxDateEdit>
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
								<td>
									Business Unit
								</td>
								<td>
									<dx:ASPxListBox ID="ddlbymemberbranch" runat="server" ClientInstanceName="ddlbymemberbranch" DataSourceID="SqlDataSource1" SelectionMode="Multiple" AnimationType="None" TextField="name" ValueField="id" ValueType="System.Int32">
									</dx:ASPxListBox>
								</td>
								<td>
									&nbsp;
								</td>
								<td>
									&nbsp;
								</td>
								<td>
									&nbsp;
								</td>
							</tr>
						</table>
						<asp:Label ID="lblCurrentView0" runat="server" Font-Italic="False" Font-Size="14pt" Width="439px" Font-Names="Calibri"></asp:Label>
						<br />
						<asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Style="border-top-style: dotted; border-right-style: dotted; border-left-style: dotted; border-bottom-style: dotted" Text="Go -&gt;" />
						<br />
						<br />
						<lc:LayoutControl runat="server" id="layout" GridviewID="gv_hu" />
						<dx:ASPxGridView ID="gv_hu" ClientInstanceName="gv_hu" runat="server" AutoGenerateColumns="False" OnHtmlRowPrepared="ASPxGridView1_HtmlRowPrepared" OnSummaryDisplayText="ASPxGridView1_SummaryDisplayText" Width="100%" OnCustomCallback="gv_hu_CustomCallback" OnCustomJSProperties="gv_hu_CustomJSProperties" Theme="NETheme01" SettingsBehavior-ColumnResizeMode="Control">
							<GroupSummary>
								<dx:ASPxSummaryItem DisplayFormat="P1" FieldName="membername" ShowInColumn="Member" SummaryType="Custom" ValueDisplayFormat="P1" />
							</GroupSummary>
							<SettingsBehavior ColumnResizeMode="Control" />
							<Columns>
								<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" VisibleIndex="0" Width="1px" ShowClearFilterButton="true">
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="Member" FieldName="membername" ShowInCustomizationForm="True" VisibleIndex="1" Width="150px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" ShowInCustomizationForm="True" VisibleIndex="2" Width="150px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Date" FieldName="Date" ShowInCustomizationForm="True" VisibleIndex="4" Width="100px">
									<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
									</PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Customer" FieldName="MemberTime_Customer_Name" ShowInCustomizationForm="True" VisibleIndex="5" Width="200px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Type" FieldName="MemberTime_PayTypeHours_ID" ShowInCustomizationForm="True" VisibleIndex="6" Width="50px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Shop" FieldName="WOType" ShowInCustomizationForm="True" VisibleIndex="7" Width="50px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Hours" FieldName="NumberOfHours" ShowInCustomizationForm="True" VisibleIndex="8" Width="30px">
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="ID" FieldName="membertime_memberid" ShowInCustomizationForm="False" Visible="False" VisibleIndex="9">
								</dx:GridViewDataTextColumn>
							</Columns>
							<SettingsPager Mode="ShowAllRecords">
							</SettingsPager>
							<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" ShowHeaderFilterButton="True" />
						</dx:ASPxGridView>
						<br />
						<br />
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		    <dx:TabPage Text="Projected">
                <ContentCollection>
                    <dx:ContentControl runat="server">
                        <table style="width:100%;">
                            <tr>
                                <td align="right" width="250px">
                                    <dx:ASPxTextBox ID="txt_reported" runat="server" Caption="Billable Days Reported" ReadOnly="True" Theme="NETheme01" Width="100px">
                                        <CaptionSettings Position="Left" />
                                    </dx:ASPxTextBox>
                                </td>
                                <td width="100%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" width="250px">
                                    <dx:ASPxTextBox ID="txt_daysinmonth" runat="server" Caption="Billable days in the Month" ReadOnly="True" Theme="NETheme01" Width="100px">
                                    </dx:ASPxTextBox>
                                </td>
                                 <td width="100%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" width="250px">
                                    <dx:ASPxTextBox ID="txt_perc_reported" runat="server" Caption="Reported" ReadOnly="True" Theme="NETheme01" Width="100px">
                                        <CaptionSettings Position="Left" />
                                    </dx:ASPxTextBox>
                                </td>
                                  <td width="100%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2" width="100%">
                                    <lc:LayoutControl ID="_layout" runat="server" GridviewID="gv_hu_projected" />
                                    <dx:ASPxGridView ID="gv_hu_projected" runat="server" AutoGenerateColumns="False" OnHtmlDataCellPrepared="ASPxGridView1_HtmlDataCellPrepared" Theme="NETheme01" Width="100%" OnFocusedRowChanged="ASPxGridView1_FocusedRowChanged" KeyFieldName="business_unit_id" EnableCallBacks="False" ClientInstanceName="gv_hu_projected" OnCustomCallback="gv_hu_CustomCallback" OnCustomJSProperties="gv_hu_CustomJSProperties">
                                        <TotalSummary>
                                            <dx:ASPxSummaryItem FieldName="this_month" ShowInColumn="Billable To Date" SummaryType="Sum" DisplayFormat="N0" />
                                            <dx:ASPxSummaryItem FieldName="next_month" ShowInColumn="Projected" SummaryType="Sum" DisplayFormat="N0"/>
                                            <dx:ASPxSummaryItem FieldName="last_year" ShowInColumn="last Year" SummaryType="Min" DisplayFormat="N0"/>
                                            <dx:ASPxSummaryItem FieldName="last_month" ShowInColumn="Last Month" SummaryType="Sum" DisplayFormat="N0"/>
                                        </TotalSummary>
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" ShowInCustomizationForm="True" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Billable To Date" FieldName="this_month" ShowInCustomizationForm="True" VisibleIndex="1">
                                                <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Projected" ShowInCustomizationForm="True" VisibleIndex="2">
                                                 <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Last Month" FieldName="last_month" ShowInCustomizationForm="True" VisibleIndex="3">
                                                 <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="last Year" FieldName="last_year" ShowInCustomizationForm="True" VisibleIndex="4">
                                                 <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="M-o-M Change"  ShowInCustomizationForm="True" VisibleIndex="5" >
                                                 <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Y-o-Y Change"  ShowInCustomizationForm="True" VisibleIndex="6" >
                                                 <CellStyle HorizontalAlign="Center">
                                                </CellStyle>
                                            </dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn Caption="business_unit_id" FieldName="business_unit_id" ShowInCustomizationForm="True" VisibleIndex="7" visible="false">
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior AllowFocusedRow="True" ProcessFocusedRowChangedOnServer="True" AllowSelectSingleRowOnly="True" />
                                        <SettingsPager Mode="ShowAllRecords">
                                        </SettingsPager>
                                        <Settings ShowFooter="True" ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
                                        <Styles>
                                            <Header HorizontalAlign="Center">
                                            </Header>
                                            <Footer Font-Bold="True" HorizontalAlign="Center">
                                            </Footer>
                                        </Styles>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="hist" runat="server">
                                    </div>
                                </td>
                               
                            </tr>
                        </table>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
		</TabPages>
	</dx:ASPxPageControl>
		</ContentTemplate>
	    <Triggers>
            <asp:PostBackTrigger ControlID="ASPxPageControl1" />
        </Triggers>
	</asp:UpdatePanel>
</asp:Content>
