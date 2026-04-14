<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_dashboards_index" Title="Target Dashboards"  EnableTheming="True" Codebehind="index.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register src="~/modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>





<%@ Register src="modules/bonus_panel.ascx" tagname="bonus_panel" tagprefix="uc1" %>





<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <script type="text/javascript" language="javascript">

	
			function open_offer(id) {
				boing2("/sections/hr/member/offer_print_off.aspx?moid=" + id,"offer review", 900, 700);
			}



function resizeIframe(obj) {

try{
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 5) + 'px';
   }
   catch (err){}

  }

  function CurrencyFormatted(amount) {
  	var i = parseFloat(amount);
  	if (isNaN(i)) { i = 0.00; }
  	var minus = '';
  	if (i < 0) { minus = '-'; }
  	i = Math.abs(i);
  	i = parseInt((i + .005) * 100);
  	i = i / 100;
  	s = new String(i);
  	if (s.indexOf('.') < 0) { s += '.00'; }
  	if (s.indexOf('.') == (s.length - 2)) { s += '0'; }
  	s = minus + s;
  	return s;
  }

 </script>



							<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"  ID="SqlDataSource2"></asp:SqlDataSource>

							<table cellpadding="5px" width="100%"><tr><td>
							<dx:ASPxRoundPanel ID="rp_main" runat="server" 
		BackColor="#80FF80" HeaderText="Target Dashboard" Width="100%" Font-Bold="True" Font-Names="Arial" 
								Font-Size="11pt" ForeColor="#006600">
								<HeaderStyle BackColor="#00FF00">
								<BorderBottom BorderStyle="None" />
<BorderBottom BorderStyle="None"></BorderBottom>
								</HeaderStyle>
								
								
								
								
								
								
								<HeaderTemplate>
									<table style="width:100%;">
										<tr>
											<td nowrap="nowrap" style="padding-right: 50px">
												Target Dashboard</td>
											<td width="100%">
												<dx:ASPxDateEdit ID="dte" runat="server" AutoPostBack="True" 
													ClientInstanceName="dte" DisplayFormatString="yyyy-MMMM" 
													EditFormat="Custom" EditFormatString="yyyy-MMMM" 
													ondatechanged="dte_DateChanged" Caption="Month" ForeColor="Black">
												</dx:ASPxDateEdit>
											</td>
											<td align="right">
												<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" 
													ClientInstanceName="ASPxComboBox2" AutoPostBack="True" ClientVisible="False" 
													onselectedindexchanged="ASPxComboBox2_SelectedIndexChanged">
												</dx:ASPxComboBox>
											</td>
										</tr>
									</table>
								</HeaderTemplate>
								<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<lc:LayoutControl ID="layout" runat="server" GridviewID="ASPxGridView1" 
		is_private="True" ShowExcelExport="False" ShowPDFExport="False" 
		ShowToggle="True" />
	<dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
		ClientInstanceName="ASPxGridView1" Font-Names="Arial" KeyFieldName="member_id" 
		OnCustomCallback="ASPxGridView1_CustomCallback"
		OnCustomJSProperties="ASPxGridView1_CustomJSProperties" Width="100%" 
		OnHtmlDataCellPrepared="ASPxGridView1_HtmlDataCellPrepared" 
		>
		<ClientSideEvents SelectionChanged="function(s, e) {
	cb_pm.PerformCallback(s.GetRowKey(e.visibleIndex));
}" />
		<SettingsBehavior ColumnResizeMode="NextColumn" 
			AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" 
			 />

		<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
			ShowFooter="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" 
			ColumnMinWidth="20" />

		<TotalSummary>
			<dx:ASPxSummaryItem DisplayFormat="N0" FieldName="openwos" 
				ShowInColumn="Open WOs" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="rev_mtd" 
				ShowInColumn="Revenue MTD" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="next_thirty_days" 
				ShowInColumn="Rev Next 30 Days" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="next_sixty_days" 
				ShowInColumn="Rev Next 60 Days" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="dollars_per_quoted_hour" 
				ShowInColumn="$$ Per Quoted hour" SummaryType="Average" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="mtd_target" 
				ShowInColumn="Rev Target MTD" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="ytd" 
				ShowInColumn="Revenue YTD" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="target_rev_fytd" 
				ShowInColumn="Rev Target YTD" SummaryType="Sum" />
		</TotalSummary>
		<Columns>
			<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="branch" 
				ShowInCustomizationForm="True" VisibleIndex="0" Width="60px">
				<PropertiesComboBox DataSourceID="SqlDataSource2" TextField="name" 
					ValueField="id" ValueType="System.Int32">
				</PropertiesComboBox>
				<Settings FilterMode="DisplayText" />

<Settings FilterMode="DisplayText"></Settings>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Project Manager" FieldName="pm" 
				ShowInCustomizationForm="True" VisibleIndex="1" Width="75px">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Membertype" FieldName="membertype_name" 
				MinWidth="20" ShowInCustomizationForm="True" VisibleIndex="2" Width="50px">
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Rev YTD" FieldName="ytd" 
				ShowInCustomizationForm="True" VisibleIndex="3" Width="75px">
				<PropertiesTextEdit DisplayFormatString="C0">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
						oninit="ASPxHyperLink5_Init" Cursor="pointer" >
					</dx:ASPxHyperLink>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Rev YTD Target " FieldName="target_rev_fytd" 
				ShowInCustomizationForm="True" VisibleIndex="4" Width="75px">
				<PropertiesTextEdit DisplayFormatString="C0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center" Wrap="False" BackColor="#FFFFCC">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Mar YTD" FieldName="margin_ytd" 
				ShowInCustomizationForm="True" VisibleIndex="5" Width="60px">
				<PropertiesTextEdit DisplayFormatString="P1">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
						oninit="ASPxHyperLink6_Init" Cursor="pointer">
					</dx:ASPxHyperLink>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Rev MTD" FieldName="rev_mtd" 
				ShowInCustomizationForm="True" VisibleIndex="6" Width="60px">
				<PropertiesTextEdit DisplayFormatString="C0">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
						oninit="ASPxHyperLink3_Init" Cursor="pointer">
					</dx:ASPxHyperLink>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Rev MTD Target" FieldName="mtd_target" 
				ShowInCustomizationForm="True" VisibleIndex="7" Width="60px">
				<PropertiesTextEdit DisplayFormatString="C0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center" BackColor="#FFFFCC">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Mar MTD" FieldName="margin_mtd" 
				ShowInCustomizationForm="True" VisibleIndex="8" Width="60px">
				<PropertiesTextEdit DisplayFormatString="P1">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
						oninit="ASPxHyperLink3_Init" Cursor="pointer">
					</dx:ASPxHyperLink>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Open WOs" FieldName="openwos" 
				ShowInCustomizationForm="True" VisibleIndex="9" Width="50px">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
						oninit="ASPxHyperLink1_Init" Text='<%# Eval("openwos") %>' Cursor="pointer">
					</dx:ASPxHyperLink>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Rev Next 30 Days" 
				FieldName="next_thirty_days" ShowInCustomizationForm="True" VisibleIndex="10" 
				Width="75px">
				<PropertiesTextEdit DisplayFormatString="C0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Rev Next 60 Days" 
				FieldName="next_sixty_days" ShowInCustomizationForm="True" VisibleIndex="11" 
				Width="75px">
				<PropertiesTextEdit DisplayFormatString="C0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Avg Days To Invoice (MTD)" 
				FieldName="avg_days_to_invoice" ShowInCustomizationForm="True" 
				ToolTip="The amount of time in days from the last day a work order was worked on until it was invoiced" 
				VisibleIndex="12" Width="60px">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
						oninit="ASPxHyperLink2_Init" Text='<%# Eval("avg_days_to_invoice") %>' Cursor="pointer">
					</dx:ASPxHyperLink>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			
			<dx:GridViewDataTextColumn FieldName="member_id" ShowInCustomizationForm="True" 
				Visible="False" VisibleIndex="13">
				<DataItemTemplate>
					<%# "<a class='dxeHyperlink2' href='index.aspx?drilldown=ytd&id=" + Eval("member_id") + "'>" + Eval("ytd") + "</a>"%>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Mar MTD Target" FieldName="target_mar_mtd" 
				ShowInCustomizationForm="True" VisibleIndex="14" Width="60px">
				<PropertiesTextEdit DisplayFormatString="P1">
				</PropertiesTextEdit>
				<CellStyle BackColor="#FFFFCC" HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Mar YTD Target" FieldName="target_mar_fytd" 
				ShowInCustomizationForm="True" VisibleIndex="15" Width="60px">
				<PropertiesTextEdit DisplayFormatString="P1">
				</PropertiesTextEdit>
				<CellStyle BackColor="#FFFFCC" HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Avg Days to Inv Target (MTD)" 
				FieldName="avg_days_to_invoice_tar" MinWidth="20" 
				ShowInCustomizationForm="True" VisibleIndex="16" Width="70px">
				<CellStyle BackColor="#FFFFCC">
				</CellStyle>
			</dx:GridViewDataTextColumn>
		</Columns>

<SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" ColumnResizeMode="NextColumn"></SettingsBehavior>

		<SettingsPager Mode="ShowAllRecords">
		</SettingsPager>

<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowFooter="True" ShowFilterBar="Visible"></Settings>

		<Styles>
			<TitlePanel ForeColor="Black">
			</TitlePanel>
			<FocusedRow ForeColor="Black">
			</FocusedRow>
		</Styles>
		<Templates>
			<TitlePanel>
				<table style="width: 400px; color: #000000;">
					<tr>
						<td style="text-align: center">
							Cell Legend</td>
						<td style="text-align: center">
							Coloumn Legend</td>
						<td>
							&nbsp;</td>
					</tr>
					<tr>
						<td bgcolor="#FFD9D9" 
							style="text-align: center; font-size: x-small; color: #000000;">
							<span>This &quot;Actual&quot; is below the target</span></td>
						<td bgcolor="#FFFFCC" class="style5" style="text-align: center">
							Target Columns</td>
						<td>
							&nbsp;</td>
					</tr>
				</table>
			</TitlePanel>
		</Templates>
	</dx:ASPxGridView>
									</dx:PanelContent>
</PanelCollection>
								<Border BorderColor="#00FF00" BorderStyle="Solid" BorderWidth="1px" />

<Border BorderColor="Lime" BorderStyle="Solid" BorderWidth="1px"></Border>
								</dx:ASPxRoundPanel></td></tr></table>
							<dx:ASPxCallbackPanel ID="cb_pm" runat="server" ClientInstanceName="cb_pm" 
								oncallback="cb_pm_Callback" Width="100%" Theme="NETheme01">
								<PanelCollection>
<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
	<table cellpadding="5px" width="100%"><tr><td ID="td_incentive">
		<uc1:bonus_panel ID="bonus_panel1" runat="server" Visible="False" />
	</td></tr>
		<tr>
			<td>
				<dx:ASPxRoundPanel ID="rp_customers" runat="server" BackColor="#82FFFF" 
					Font-Bold="True" Font-Names="Arial" Font-Size="11pt" ForeColor="#0033CC" 
					HeaderText="Customers Assigned To Selected Member" Width="100%">
					<HeaderStyle BackColor="Cyan">
					<BorderBottom BorderStyle="None" />
					</HeaderStyle>
					
					
					
					
					
					
					<PanelCollection>
						<dx:PanelContent runat="server">
							<iframe ID="i_customers" runat="server" class="i_customers" frameborder="0" 
								height="100" width="100%"></iframe>
						</dx:PanelContent>
					</PanelCollection>
					<Border BorderColor="#82FFFF" BorderStyle="Solid" BorderWidth="1px" />
				</dx:ASPxRoundPanel>
			</td>
		</tr>
	</table><table cellpadding="5px" width="100%"><tr><td>
	<dx:ASPxRoundPanel ID="rp_customers0" runat="server" BackColor="#FFFFC4" 
		Font-Bold="True" Font-Names="Arial" Font-Size="11pt" ForeColor="#663300" 
		HeaderText="Opportunities" Width="100%">
		<HeaderStyle BackColor="Yellow">
		<BorderBottom BorderStyle="None" />
<BorderBottom BorderStyle="None"></BorderBottom>
		</HeaderStyle>
		
		
		
		
		
		
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<iframe ID="i_quotes" runat="server" frameborder="0" height="100" width="100%">
				</iframe>
			</dx:PanelContent>
		</PanelCollection>
		<Border BorderColor="#FFFFC4" BorderStyle="Solid" BorderWidth="1px" />

<Border BorderColor="#FFFFC4" BorderStyle="Solid" BorderWidth="1px"></Border>
	</dx:ASPxRoundPanel></td></tr>
		<tr>
			<td>
				<dx:ASPxRoundPanel ID="rp_workorders" runat="server" BackColor="#EFEFEF" 
					Font-Bold="True" Font-Names="Arial" Font-Size="11pt" ForeColor="Gray" 
					HeaderText="Work Orders to Deal With" Width="100%" Theme="Default">
					<HeaderStyle BackColor="#EAEAEA">
					<BorderBottom BorderStyle="None" />
<BorderBottom BorderStyle="None"></BorderBottom>
					</HeaderStyle>
					
					<PanelCollection>
						<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
							<dx:ASPxGridView ID="gv_wos" runat="server" AutoGenerateColumns="False" 
								ClientInstanceName="gv_wos" Font-Names="Arial" 
								OnCustomCallback="gv_wos_CustomCallback" Width="100%" KeyFieldName="id" 
								OnSummaryDisplayText="gv_wos_SummaryDisplayText" 
								OnHtmlDataCellPrepared="gv_wos_HtmlDataCellPrepared" Theme="NETheme01">

<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

<Settings ShowFilterRow="True"></Settings>
								<TotalSummary>
									<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="tobebilled" 
										ShowInColumn="To Be Billed" SummaryType="Sum" />
									<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="exp_sales_value" 
										ShowInColumn="Expected Sales Value" SummaryType="Sum" />
									<dx:ASPxSummaryItem DisplayFormat="p1" FieldName="margin" ShowInColumn="margin" 
										SummaryType="Average" />
								</TotalSummary>
								<Columns>
									<dx:GridViewCommandColumn ShowInCustomizationForm="True" Visible="False" ShowClearFilterButton="true" 
										VisibleIndex="0">
										
									</dx:GridViewCommandColumn>
									<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
										ShowInCustomizationForm="True" Visible="False" VisibleIndex="9">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="cid" FieldName="cid" 
										ShowInCustomizationForm="True" Visible="False" VisibleIndex="10">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="WO" FieldName="bvwo" 
										ShowInCustomizationForm="True" VisibleIndex="1" Width="80px">
										<Settings AutoFilterCondition="BeginsWith" />
<Settings AutoFilterCondition="BeginsWith"></Settings>
										<DataItemTemplate>
							<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
				NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 'workorder', 1200,800)&quot;, Eval(&quot;id&quot;), Eval(&quot;cid&quot;)) %>" 
				Text='<%# Eval("bvwo") %>' Cursor="pointer" Font-Bold="True" Font-Names="Arial" Theme="NETheme01">
				
				</dx:ASPxHyperLink>
						</DataItemTemplate>

									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
										ShowInCustomizationForm="True" VisibleIndex="3" Width="100%">
										<Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Customer" FieldName="customer" 
										ShowInCustomizationForm="True" VisibleIndex="2" Width="200px">
										<Settings AutoFilterCondition="Contains" />
<Settings AutoFilterCondition="Contains"></Settings>

										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
										ShowInCustomizationForm="True" VisibleIndex="4" Width="100px">
										<Settings HeaderFilterMode="CheckedList" />
<Settings HeaderFilterMode="CheckedList"></Settings>

										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Expected Completion Date" 
										FieldName="exp_completion_date" ShowInCustomizationForm="True" VisibleIndex="5" 
										Width="120px">
										<Settings AutoFilterCondition="LessOrEqual" />
<Settings AutoFilterCondition="LessOrEqual"></Settings>
										<DataItemTemplate>
											<dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" 
												DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
												EditFormatString="yyyy-MM-dd" oninit="ASPxDateEdit1_Init" 
												Value='<%# Eval("exp_completion_date") %>' Width="100px">
											</dx:ASPxDateEdit>
										</DataItemTemplate>
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Expected Sales Value" 
										FieldName="exp_sales_value" ShowInCustomizationForm="True" VisibleIndex="6" 
										Width="120px">
										<Settings AutoFilterCondition="GreaterOrEqual" />
<Settings AutoFilterCondition="GreaterOrEqual"></Settings>
										<DataItemTemplate>
											<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" oninit="ASPxTextBox1_Init" 
												Text='<%# Eval("exp_sales_value") %>' Width="80px">
											</dx:ASPxTextBox>
										</DataItemTemplate>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="To Be Billed" FieldName="tobebilled" 
										ShowInCustomizationForm="True" VisibleIndex="7" Width="100px">
										<Settings AutoFilterCondition="GreaterOrEqual" />
<Settings AutoFilterCondition="GreaterOrEqual"></Settings>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Margin" FieldName="margin" 
										ShowInCustomizationForm="True" VisibleIndex="8" Width="70px">
										<PropertiesTextEdit DisplayFormatString="p1">
										</PropertiesTextEdit>
										<Settings AutoFilterCondition="LessOrEqual" />

<Settings AutoFilterCondition="LessOrEqual"></Settings>
									</dx:GridViewDataTextColumn>
								    <dx:GridViewDataDateColumn Caption="Cut Date" FieldName="woprog_cutdate" ShowInCustomizationForm="True" VisibleIndex="11" Width="90px">
                                        <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                        </PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
								</Columns>
								<SettingsBehavior ColumnResizeMode="Control" />

								<SettingsPager Mode="ShowAllRecords" Visible="False">
								</SettingsPager>
								<Settings ShowFilterRow="True" ShowFilterBar="Visible" ShowFilterRowMenu="True" 
									ShowFooter="True" ShowHeaderFilterButton="True" />

							</dx:ASPxGridView>
						</dx:PanelContent>
					</PanelCollection>
					<Border BorderStyle="None" />


				</dx:ASPxRoundPanel>
			</td>
		</tr>
		<tr>
			<td>
				<br />
				<dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" Height="700px" 
					Modal="True" PopupHorizontalAlign="WindowCenter" 
					PopupVerticalAlign="WindowCenter" Width="1000px" ShowPageScrollbarWhenModal="True" 
					Theme="NETheme01">

<Border BorderStyle="None"></Border>
					<ClientSideEvents PopUp="function(s, e) {
	resizeIframe(this);
}" />
<ClientSideEvents PopUp="function(s, e) {
	resizeIframe(this);
}"></ClientSideEvents>

					<ContentStyle BackColor="#FFCC99">
					</ContentStyle>
					<HeaderStyle BackColor="#FF9933" Font-Bold="True" Font-Names="Arial" 
						Font-Size="11pt">
					<Paddings Padding="8px" />
					<Border BorderStyle="None" />
<Paddings Padding="8px"></Paddings>

<Border BorderStyle="None"></Border>
					</HeaderStyle>
					<ContentCollection>
						<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
							<iframe ID="i_drill" runat="server" frameborder="0" height="700px" 
								name="i_drill" width="100%"></iframe>
						</dx:PopupControlContentControl>
					</ContentCollection>
					<Border BorderStyle="None" />

				</dx:ASPxPopupControl>
				<br />
			</td>
		</tr>
	</table>
									</dx:PanelContent>
</PanelCollection>
							</dx:ASPxCallbackPanel>
	
   
							
</asp:Content>

<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
    <style type="text/css">
		.style5
		{
			font-size: x-small;
		}
	</style>
</asp:Content>


