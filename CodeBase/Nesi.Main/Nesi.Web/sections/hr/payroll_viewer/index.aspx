<%@ page language="C#" masterpagefile="../../../IntraDefault.master" autoeventwireup="true" inherits="payroll_viewer" title="Payroll Viewer" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/modules/layout_control.ascx" TagPrefix="uc1" TagName="layout_control" %>


<asp:content id="Content1" contentplaceholderid="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server"></div>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphMasterBody" runat="Server">
					<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="0" Width="100%">
						<TabPages>
							<dx:TabPage Text="Overview">
								<ContentCollection>
									<dx:ContentControl runat="server">
	<div id='payroll_viewer'>
		<script type="text/javascript" src="/js/payroll_viewer.js"></script>
		<asp:scriptmanager id="sm" runat="server" enablepagemethods="true"></asp:scriptmanager>
		<table class="framework" cellpadding="0" cellspacing="0">
			<tr>
				<td valign="top" id="status" class="status" runat="server">
					<div id='room'>
						<div id='header' onclick="location.href='/sections/hr/payroll_viewer/index.aspx';">&lt;&lt; Payroll Overview Home</div>
						<div id="branch_selector" runat="server">
							
						</div>
					</div>
				</td>
				<td valign="top" align="center" id="detail" class="detail" runat="server">
                    <script src="../../../js/table2excel/table2excel.js"></script>
	<script>
		$(document).ready(function()
			{
			$('#info').tablesorter(
									{
										headers:
											{
											4:{sorter:'currency'},
											5:{sorter:'currency'},
											6:{sorter:'currency'},
											7:{sorter:'currency'},
											8:{sorter:'currency'},
											9:{sorter:'currency'},
											10:{sorter:'currency'},
											11:{sorter:'currency'},
											12:{sorter:'currency'},
											13:{sorter:'currency'},
											14:{sorter:'currency'},
											15:{sorter:'currency'},
											16:{sorter:'currency'},
											17:{sorter:'currency'},
											18:{sorter:'currency'},
											19:{sorter:'currency'},
											20:{sorter:'currency'},
											21:{sorter:'currency'},
											22:{sorter:'currency'},
											23:{sorter:'currency'}
											}
									}
            );
               $("#btnex").click(function () {
                var filename = $(".companyname").html();
                var period= $(".payperiod_of").html().replace("For the payperiod of:"," ");
  $("#info").table2excel({
    // exclude CSS class
    exclude: ".noExl",
    name: filename+period,
    filename: filename+period, //do not include extension
    fileext: ".xls" // file extension
  }); 
});
			});
	</script>

					<div id="viewer_home" runat="server" visible="false">
						
					</div>
					<div id="viewer_table" runat="server" visible="false">
						<div class='companyname' id="name" runat="server"></div>
						<div class='finalapproval' id="finalapproval" runat="server"></div>
						<div class='payperiod_of' id="payperiod_of" runat="server"></div>
						<table cellpadding='1' cellspacing='0' id="info">
							<thead>
								<tr>
                                    <th data-coln="1">PAYROLL ID</th>
									<th data-coln="2">Employee<i>(first)</i></th>
									<th data-coln="3">Employee<i>(last)</i></th>
									<th data-coln="4">HR Status</th>
									<th data-coln="5">Pay Type</th>
									<th data-coln="6">Dept. Head</th>
									<th data-coln="7">Wage</th>
									<th data-coln="8">RT</th>
									<th data-coln="9">OT</th>
									<th data-coln="10">DT</th>
									<th data-coln="11">RT sp</th>
									<th data-coln="12">OT sp</th>
									<th data-coln="13">DT sp</th>
									<th data-coln="14">Unpaid<i>Vacation</i></th>
									<th data-coln="15">Unpaid<i>Sick Day</i></th>
									<th data-coln="16">Paid<i>Sick Day</i></th>
									<th data-coln="17">Unpaid<i>Other</i></th>
									<th data-coln="18">Birthday</th>
									<th data-coln="19">Bereavement</th>
									<th data-coln="20">Stat Pay</th>
									<th data-coln="21">Vacation</th>
									<th data-coln="22">Expense</th>
                                    <th data-coln="23">Per Diems</th>
									<th data-coln="24">Commission</th>
									<th data-coln="25">Miscellaneous</th>
									<th data-coln="26">Bonus</th>
									<th data-coln="27">Bank<i>Deposited</i></th>
									<th data-coln="28">Bank<i>Withdrawn</i></th>
									<th data-coln="29">Bank<i>Paidout</i></th>
									<th data-coln="30">Bank<i>Deducted</i></th>
                                    <th data-coln="31">$ TOTAL</th>
								</tr>
							</thead>
							<tbody runat="server" id="viewer_body">
							</tbody>
							<tfoot runat="server" id="viewer_foot">
		
							</tfoot>
						</table>
						<div id="div_outstanding" runat="server">
							
						</div>
					</div>
				</td>
			</tr>
		</table>
	</div>
									</dx:ContentControl>
								</ContentCollection>
							</dx:TabPage>
							<dx:TabPage Text="Files">
								<ContentCollection>
									<dx:ContentControl runat="server">
										<dx:ASPxFileManager ID="fm" runat="server">
											<Settings RootFolder="~/" ThumbnailFolder="~/Thumb/" />
											<SettingsFileList View="Details">
											</SettingsFileList>
											<SettingsEditing AllowCopy="True" AllowCreate="True" AllowDownload="True" AllowMove="True" AllowRename="True" TemporaryFolder="~/App_Data/FileManagerTemp" />
											<SettingsUpload>
												<AdvancedModeSettings EnableMultiSelect="True">
												</AdvancedModeSettings>
											</SettingsUpload>
										</dx:ASPxFileManager>
									</dx:ContentControl>
								</ContentCollection>
							</dx:TabPage>
							<dx:TabPage Text="Payroll Summary">
								<ContentCollection>
									<dx:ContentControl runat="server">
										<uc1:layout_control runat="server" ID="layout" GridviewID="gv_summary" />
										<dx:ASPxGridView ID="gv_summary" runat="server" DataSourceID="ds_summary" AutoGenerateColumns="False" KeyFieldName="unit" Theme="NETheme01" OnCustomCallback="gv_summary_CustomCallback" OnCustomJSProperties="gv_summary_CustomJSProperties">

											<Settings ShowFooter="True" ShowGroupFooter="VisibleAlways" />

											<Columns>
												<dx:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" Caption="Business Unit" FieldName="unit" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="0">
													<EditFormSettings Visible="False" />

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="RT Hours" FieldName="rt_hours" ShowInCustomizationForm="True" VisibleIndex="1">
													<PropertiesTextEdit DisplayFormatString="{0:N2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="OT Hours" FieldName="ot_hours" ShowInCustomizationForm="True" VisibleIndex="2">
													<PropertiesTextEdit DisplayFormatString="{0:N2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="DT Hours" FieldName="dt_hours" ShowInCustomizationForm="True" VisibleIndex="3">
													<PropertiesTextEdit DisplayFormatString="{0:N2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="RTsp Hours" FieldName="rtsp_hours" ShowInCustomizationForm="True" VisibleIndex="4">
													<PropertiesTextEdit DisplayFormatString="{0:N2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="OTsp Hours" FieldName="otsp_hours" ShowInCustomizationForm="True" VisibleIndex="5">
													<PropertiesTextEdit DisplayFormatString="{0:N2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="DTsp Hours" FieldName="dtsp_hours" ShowInCustomizationForm="True" VisibleIndex="6">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="RT Total" FieldName="rt_total" ShowInCustomizationForm="True" VisibleIndex="7">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="OT Total" FieldName="ot_total" ShowInCustomizationForm="True" VisibleIndex="8">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="DT Total" FieldName="dt_total" ShowInCustomizationForm="True" VisibleIndex="9">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="RTsp Total" FieldName="rtsp_total" ShowInCustomizationForm="True" VisibleIndex="10">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="OTsp Total" FieldName="otsp_total" ShowInCustomizationForm="True" VisibleIndex="11">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="DTsp Total" FieldName="dtsp_total" ShowInCustomizationForm="True" VisibleIndex="12">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Stat Total" FieldName="stat_total" ShowInCustomizationForm="True" VisibleIndex="13">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Vacation Total" FieldName="vacation_total" ShowInCustomizationForm="True" VisibleIndex="14">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Expense Total" FieldName="expense_total" ShowInCustomizationForm="True" VisibleIndex="15">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Total" FieldName="total" ShowInCustomizationForm="True" VisibleIndex="16">
													<PropertiesTextEdit DisplayFormatString="{0:C2}">
													</PropertiesTextEdit>
													<CellStyle HorizontalAlign="Center" />
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsPager Mode="ShowAllRecords">
												
											</SettingsPager>
											<TotalSummary>
												<dx:ASPxSummaryItem FieldName="rt_hours" ShowInColumn="RT Hours" ShowInGroupFooterColumn="RT Hours" SummaryType="Sum" DisplayFormat="{0:N2}" />
												<dx:ASPxSummaryItem FieldName="ot_hours" ShowInColumn="OT Hours" ShowInGroupFooterColumn="OT Hours" SummaryType="Sum"  DisplayFormat="{0:N2}"/>
												<dx:ASPxSummaryItem FieldName="dt_hours" ShowInColumn="DT Hours" ShowInGroupFooterColumn="DT Hours" SummaryType="Sum" DisplayFormat="{0:N2}" />
												<dx:ASPxSummaryItem FieldName="rtsp_hours" ShowInColumn="RTsp Hours" ShowInGroupFooterColumn="RTsp Hours" SummaryType="Sum" DisplayFormat="{0:N2}" />
												<dx:ASPxSummaryItem FieldName="otsp_hours" ShowInColumn="OTsp Hours" ShowInGroupFooterColumn="OTsp Hours" SummaryType="Sum" DisplayFormat="{0:N2}" />
												<dx:ASPxSummaryItem FieldName="dtsp_hours" ShowInColumn="DTsp Hours" ShowInGroupFooterColumn="DTsp Hours" SummaryType="Sum" DisplayFormat="{0:N2}" />

												<dx:ASPxSummaryItem FieldName="rt_total" ShowInColumn="RT Total" ShowInGroupFooterColumn="RT Total" SummaryType="Sum" DisplayFormat="{0:C2}" />
												<dx:ASPxSummaryItem FieldName="ot_total" ShowInColumn="OT Total" ShowInGroupFooterColumn="OT Total" SummaryType="Sum" DisplayFormat="{0:C2}" />
												<dx:ASPxSummaryItem FieldName="dt_total" ShowInColumn="DT Total" ShowInGroupFooterColumn="DT Total" SummaryType="Sum" DisplayFormat="{0:C2}" />
												<dx:ASPxSummaryItem FieldName="rtsp_total" ShowInColumn="RTsp Total" ShowInGroupFooterColumn="RTsp Total" SummaryType="Sum" DisplayFormat="{0:C2}" />
												<dx:ASPxSummaryItem FieldName="otsp_total" ShowInColumn="OTsp Total" ShowInGroupFooterColumn="OTsp Total" SummaryType="Sum" DisplayFormat="{0:C2}" />
												<dx:ASPxSummaryItem FieldName="dtsp_total" ShowInColumn="DTsp Total" ShowInGroupFooterColumn="DTsp Total" SummaryType="Sum" DisplayFormat="{0:C2}" />

												<dx:ASPxSummaryItem FieldName="stat_total" ShowInColumn="Stat Total" ShowInGroupFooterColumn="Stat Total" SummaryType="Sum" DisplayFormat="{0:C2}" />
												<dx:ASPxSummaryItem FieldName="vacation_total" ShowInColumn="Vacation Total" ShowInGroupFooterColumn="Vacation Total" SummaryType="Sum" DisplayFormat="{0:C2}" />
												<dx:ASPxSummaryItem FieldName="expense_total" ShowInColumn="Expense Total" ShowInGroupFooterColumn="Expense Total" SummaryType="Sum" DisplayFormat="{0:C2}" />
												<dx:ASPxSummaryItem FieldName="total" ShowInColumn="Total" ShowInGroupFooterColumn="Total" SummaryType="Sum" DisplayFormat="{0:C2}" />
											</TotalSummary>
											<Styles Footer-HorizontalAlign="Center" Footer-Font-Size="1.25em" Header-HorizontalAlign="Center">
<Header HorizontalAlign="Center"></Header>

<Footer HorizontalAlign="Center" Font-Size="1.25em"></Footer>
											</Styles>
										</dx:ASPxGridView>
										<asp:SqlDataSource ID="ds_summary" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL get_payperiod_totals(?pp_id)">
											<SelectParameters>
												<asp:QueryStringParameter Name="pp_id" QueryStringField="P" />
											</SelectParameters>
										</asp:SqlDataSource>
									</dx:ContentControl>
								</ContentCollection>
							</dx:TabPage>
						</TabPages>
					</dx:ASPxPageControl>
</asp:content>

