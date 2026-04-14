<%@ Page	Language		="C#" 
			AutoEventWireup	="true" 
			Async			="true" 
			EnableViewState	= "true" 
			Inherits		="sections_customer_index" 
			EnableTheming="True"
			 Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register src="modules/locations.ascx" tagname="locations" tagprefix="uc" %>
<%@ Register src="modules/accounting.ascx" tagname="accounting" tagprefix="uc" %>
<%@ Register src="modules/header.ascx" tagname="header" tagprefix="uc" %>
<%@ Register src="modules/new.ascx" tagname="new" tagprefix="uc" %>

<%@ Register src="../../modules/partner_skills.ascx" tagname="partner_skills" tagprefix="uc1" %>

<link rel="Stylesheet" type="text/css" href="/css/customer.css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<script type="text/javascript">
		var ts_start		= new Date().getTime();
	</script>
	<title>Customer</title>

	
	<meta http-equiv="Pragma" content="no-cache"/>
    <meta http-equiv="Expires" content="-1"/>
    <meta http-equiv="cache-control" content="no-store"/>
	</head>
<body>
	<script type="text/javascript" src="/js/jquery-1.3.2.min.js"></script>
	<script type="text/javascript" src="/js/jquery-ui.js"></script>
	<script type="text/javascript" src="/js/jquery.tip.js"></script>
	<script type="text/javascript" src="/js/functions.js"></script>
	<script type="text/javascript" src="/js/customer.js"></script>
	
   
    <form id="Form1" runat="server" autocomplete="off" >
 <div style="display: none;">
 <input type="text" id="PreventChromeAutocomplete" name="PreventChromeAutocomplete" autocomplete="address-level4" />
</div>
	<script type="text/javascript" language="javascript">
		var _container			= {
									e:		true
									};
		function r_qc1(s,e)
			{
			if(confirm("Are you sure you want to remove QC1 on this customer?"))
				{
				cb_rqc1.PerformCallback();
				cust_main_cbpanel.PerformCallback('load_customer');
				}
			}
		function r_qc2(s,e)
			{
			if(confirm("Are you sure you want to remove QC2 on this customer?"))
				{
				cb_rqc2.PerformCallback();
				cust_main_cbpanel.PerformCallback('load_customer');
				}
			}
		function sync_bv(business_unit_id)
			{
			main_persistence_handler.Set("company_to_sync", business_unit_id);
			cust_main_cbpanel.PerformCallback('sync_customer');
			}
		$(document).ready(function()
			{
			page_obj.plcapture();
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
			});
		function BeginReqHandler()
			{
			please_wait("start");
			}
		function EndReqHandler()
			{
			please_wait("stop");
			}
		function bind_tooltips()
			{
			NE_Customer.resize();
			$(".ttip").each(function()
								{
								$(this).tip();
								});
			}


			function file_download(filename)
			
			
	{
	//alert("Hello");
	$("#downloader").attr("src", "../../download.aspx?file_id=" + filename + "");
	}
	</script>


	<asp:ScriptManager ID="sm" runat="server" EnablePageMethods="true">
	</asp:ScriptManager>
	
		<div id="progress_layer" style="display:none;position:fixed; left: 0px; top: 0px; width: 100%; height:100%; background-color:#fff;opacity:0.5;background-image: url('/images/loading_panel.gif'); background-repeat:no-repeat; background-position: center center;">&nbsp;</div>
		<asp:UpdatePanel ID="up_customer" runat="server">
			<ContentTemplate>
		<input type="hidden" id="pl_id" value="" runat="server" />
	<dx:ASPxCallbackPanel ID="cust_main_cbpanel" runat="server" Height="100%" OnCallback="cust_main_cb" ClientInstanceName="cust_main_cbpanel" meta:resourcekey="cust_main_cbpanelResource1"  HideContentOnCallback="False" Theme="NETheme01">
		<PanelCollection>
			<dx:PanelContent ID="PanelContent1" runat="server" meta:resourcekey="PanelContentResource2">
				<dx:ASPxPanel ID="cust_main_panel" runat="server" ClientInstanceName="cust_main_panel" Height="100%" meta:resourcekey="cust_main_panelResource1">
					<PanelCollection>
						<dx:PanelContent ID="PanelContent2" runat="server" meta:resourcekey="PanelContentResource1">
							<dx:ASPxHiddenField ID="main_persistence_handler" runat="server" ClientInstanceName="main_persistence_handler">
																					</dx:ASPxHiddenField>
												<table cellpadding="0" cellspacing="0" width="100%">
													
													<tr>
														<td align="left" style="height: 50px">
														<table width="100%">
														<tr>
														<td>
														<dx:ASPxButton ID="btn_new" runat="server" AutoPostBack="False" Text="New Customer"
																	UseSubmitBehavior="False" meta:resourcekey="btn_newResource1" Font-Names="Arial" Font-Size="9pt" style="font-family: Arial">
																	<ClientSideEvents Click="function(s, e)
	{
			new_customer_popup.Show();
			new_combo_origin.Focus();
	}" />


																	<Image Url="~/images/icon/icon[file].gif">
																	</Image>
																</dx:ASPxButton>
														</td>											
														<td>
															<dx:ASPxButton ID="btn_postalsearch" runat="server" AutoPostBack="False" Text="Postal Search"
																	UseSubmitBehavior="False" Font-Names="Arial" Font-Size="9pt" style="font-family: Arial">
																	<ClientSideEvents Click="function(s, e)
	{
			boing('./postal_search.aspx', 'postal_search', 350, 600);
	}" />


																	<Image Url="~/images/icon/icon[popup].gif">
																	</Image>
																</dx:ASPxButton>
														
														</td>
														<td width="100%" align="right">
														<dx:ASPxButton ID="cust_search_b" runat="server" Text="Find" Width="100px" 
																	AutoPostBack="False" UseSubmitBehavior="False" VerticalAlign="Middle" 
																	meta:resourcekey="cust_search_bResource1" Font-Names="Arial" Font-Size="9pt">
																<ClientSideEvents Click="function(s, e) {
	cust_page_c.SetActiveTab(cust_page_c.GetTab(0));
	cust_main_cbpanel.PerformCallback('search');
}" />


																<Image Url="~/images/icon/icon[search].gif">
																</Image>
															</dx:ASPxButton>
														
														</td>
														<td><script type="text/javascript">
																function catch_search(obj, e) {
																	var key = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
																	if (key == 13) {
																		cust_page_c.SetActiveTab(cust_page_c.GetTab(0));
																		cust_main_cbpanel.PerformCallback('search');
																		e.returnValue = false;
																	}
																}
																</script>
																<input type="text" id="cust_search_t" runat="server" onkeydown="catch_search(this, event)" />
														</td>
														</tr>
														</table>
														</td>
													</tr>
													<tr>
														<td>
												<dx:ASPxPageControl ID="cust_page_c" runat="server" ActiveTabIndex="0"  
																Width="100%" AutoPostBack="true" ClientInstanceName="cust_page_c" meta:resourcekey="cust_page_cResource1" 
																EnableCallBacks="True" Theme="NETheme01" LoadingPanelStyle-VerticalAlign="Middle" TabStyle-Wrap="True" TabStyle-Height="30px">
													<TabPages>
														<dx:TabPage Text="Search Results">
															<ContentCollection>
																<dx:ContentControl ID="ContentControl1" runat="server">
																	<dx:ASPxGridView ID="search_results_gv" runat="server" AutoGenerateColumns="False"
																		DataSourceID="search_ds" ClientInstanceName="cust_grid" KeyFieldName="customer_id" EnableRowsCache="False" 
																		meta:resourcekey="search_results_gvResource1"   Width="100%" Theme="NETheme01"
																		 OnHtmlDataCellPrepared="search_results_gv_HtmlDataCellPrepared" 
																		OnHtmlRowPrepared="search_results_gv_HtmlRowPrepared">
																		<ClientSideEvents RowClick="function(s, e) {main_persistence_handler.Set(&quot;customer_id&quot;, s.GetRowKey(e.visibleIndex));cust_main_cbpanel.PerformCallback('load_customer');}"></ClientSideEvents>
																		<Columns>
																			<dx:GridViewCommandColumn ShowInCustomizationForm="True" VisibleIndex="0" 
																				Caption=" " Visible="False" ShowClearFilterButton="true">
																				
																			</dx:GridViewCommandColumn>
																			<dx:GridViewDataTextColumn Caption="ID" FieldName="customer_id" Visible="False" VisibleIndex="1"
																				Width="2%">
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="No" FieldName="customer_number" VisibleIndex="1"
																				Width="55px">
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Name" FieldName="customer_name" VisibleIndex="2"
																				Width="250px" SortIndex="0" SortOrder="Ascending">
																				<Settings AutoFilterCondition="Contains" />
																			</dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Address" FieldName="address_" VisibleIndex="3"
                                                                                Width="100%">
                                                                            	<Settings AutoFilterCondition="Contains" />
                                                                            	<HeaderStyle HorizontalAlign="Center" />
																				<CellStyle HorizontalAlign="Left">
																				</CellStyle>
                                                                            </dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Phone" VisibleIndex="4" FieldName="phone" 
																				Width="105px">
																				<HeaderStyle HorizontalAlign="Center" />
																				<CellStyle HorizontalAlign="Center" Wrap="False">
																				</CellStyle>
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Account Manager" FieldName="membername" 
																				ShowInCustomizationForm="True" VisibleIndex="5" Width="80px">
																				<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
																				</PropertiesTextEdit>
																				<Settings AutoFilterCondition="Contains" />
																				<CellStyle Wrap="False">
																				</CellStyle>
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="On Hold" FieldName="hold" 
																				ShowInCustomizationForm="True" VisibleIndex="6" Width="25px">
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Date Added" FieldName="dateadded" 
																				ShowInCustomizationForm="True" VisibleIndex="7" Width="75px">
																				<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
																				</PropertiesTextEdit>
																				<CellStyle Wrap="False">
																				</CellStyle>
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Address Status" FieldName="statuss" 
																				ShowInCustomizationForm="True" VisibleIndex="8" Width="75px">
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="qc1" FieldName="qc1" 
																				ShowInCustomizationForm="True" Visible="False" VisibleIndex="12" Width="75px">
																				<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
																				</PropertiesTextEdit>
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="qc2" FieldName="qc2" 
																				ShowInCustomizationForm="True" Visible="False" VisibleIndex="11" Width="50px">
																				<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
																				</PropertiesTextEdit>
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Customer Status" FieldName="cust_status" 
																				ShowInCustomizationForm="True" VisibleIndex="9" Width="75px">
																			</dx:GridViewDataTextColumn>
																			<dx:GridViewDataTextColumn Caption="Location Type" FieldName="address_table" 
																				ShowInCustomizationForm="True" VisibleIndex="10" Width="75px">
																			</dx:GridViewDataTextColumn>
																		</Columns>
																		<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" />

																		<SettingsPager PageSize="20">
																		</SettingsPager>

<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True"></Settings>

<SettingsText EmptyDataRow="No Results Found"></SettingsText>

																		
																		<Templates>
																			<TitlePanel>
																				<table style="width:100%; white-space:nowrap;">
																					<tr>
																						<td bgcolor="Blue"  style="color: #FFFFFF" width="0%">
																							<strong>This Customer was added in the last month</strong></td>
																						<td  bgcolor="#009900" style="color: #FFFFFF" width="0%">
																							<strong>Customer has not been QC1'd yet</strong>
																						</td>
																						<td  bgcolor="#FF0000" style="color: #FFFFFF" width="0%">
																							<strong>Customer is On Hold</strong>
																						</td>
																						<td width="100%">
																						</td>
																					</tr>
																				</table>
																			</TitlePanel>
																		</Templates><Styles><Header ForeColor="White"></Header></Styles>
																	</dx:ASPxGridView>
																	<asp:SqlDataSource ID="search_ds" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
SelectCommand="
CALL CRM_SEARCH_V2(@criteria, 1, 0, 0)

" OnSelecting="search_ds_Selecting">
																		<SelectParameters>
																			<asp:ControlParameter ControlID="cust_search_t" Name="@criteria" PropertyName="value" />
																		</SelectParameters>
																	</asp:SqlDataSource>
																</dx:ContentControl>
															</ContentCollection>
															<TabImage Url="~/images/icon/icon[search].gif">
															</TabImage>
                                                            
														</dx:TabPage>
														<dx:TabPage Text="Detail">
															<ContentCollection>
																<dx:ContentControl ID="ContentControl2" runat="server">
																
																	<table width="100%" cellpadding="2" cellspacing="0">
																		<tr>
																			<td  colspan="1" style="height:200px;" valign="top">
																				<div style="float:left;">
																					<uc:header ID="uc_header" runat="server" /><asp:SqlDataSource ID="ds_ddl_address" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT address_id value, CONCAT('(',address_type,') - ', URLDECODE(address_addr1)) text FROM address WHERE address_table = 'Customer' AND address_table_id = @customer_id">
																					<SelectParameters>
																					<asp:ControlParameter ControlID="lb_customer_id" Name="@customer_id" PropertyName="value" />
																							
																					</SelectParameters>
																				</asp:SqlDataSource>
																				</div>
																				<div style="float:right;">
																				<table style="font-family: Segoe UI,Calibri; font-size: 12px">
																					<tr>
																						<td align="right">
																							<b>BV Customer #:</b>
																						</td>
																						<td align="left">
																							<dx:ASPxLabel ID="lb_customer_number" runat="server" meta:resourcekey="lb_customer_numberResource1" Theme="NETheme01">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td align="right">
																							<b>Customer ID:</b>
																						</td>
																						<td align="left">
																							<dx:ASPxLabel ID="lb_customer_id" runat="server" meta:resourcekey="lb_customer_idResource1" Theme="NETheme01">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td align="right">
																							<b>Date Added:</b>
																						</td>
																						<td align="left">
																							<dx:ASPxLabel ID="lb_date_added" runat="server" meta:resourcekey="lb_date_addedResource1" Theme="NETheme01">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td align="right">
																							<b>Added By: </b>
																						</td>
																						<td  align="left">
																							<dx:ASPxLabel ID="lb_added_by" runat="server" meta:resourcekey="lb_added_byResource1" Theme="NETheme01">
																							</dx:ASPxLabel>
																						</td>
																					</tr>
																					<tr>
																						<td align="right">
																							<b>QC LV1 Date:</b>
																						</td>
																						<td  align="left">
																							<dx:ASPxLabel ID="lb_qc_date" runat="server" meta:resourcekey="lb_qc_dateResource1" ClientInstanceName="lb_qc_date" Theme="NETheme01">
																							</dx:ASPxLabel>
																							<dx:ASPxHyperLink ID="hl_qc1" runat="server" Theme="NETheme01" ClientInstanceName="lb_qc1" NavigateUrl="javascript:void(0)" ClientSideEvents-Click="r_qc1" Text="Remove QC1" Font-Size="10px">
																								<ClientSideEvents Click="r_qc1"></ClientSideEvents>
																							</dx:ASPxHyperLink>
																							<dx:ASPxCallback ID="cb_qc1" runat="server" ClientInstanceName="cb_rqc1" OnCallback="cb_qc1_callback"></dx:ASPxCallback>
																						</td>
																					</tr>
																					<tr>
																						<td align="right">
																							<b>QC LV2 Date:</b>
																						</td>
																						<td align="left" nowrap="nowrap">
																							<dx:ASPxLabel ID="lb_qc2_date" runat="server" Theme="NETheme01" ClientInstanceName="lb_qc2_date" />
																							<dx:ASPxHyperLink ID="hl_qc2" runat="server" ClientInstanceName="lb_qc2" NavigateUrl="javascript:void(0)" ClientSideEvents-Click="r_qc2" Text="Remove QC2" Font-Size="10px">
																								<ClientSideEvents Click="r_qc2"></ClientSideEvents>
																							</dx:ASPxHyperLink>
																							<dx:ASPxCallback ID="cb_qc2" runat="server" Theme="NETheme01" ClientInstanceName="cb_rqc2" OnCallback="cb_qc2_callback"></dx:ASPxCallback>
																						</td>
																					</tr>
																					<tr>
																						<td  align="right" colspan="2">
																							<dx:ASPxButton ID="bt_qcapprove" runat="server" AutoPostBack="False" ClientInstanceName="bt_qcapprove"
																								ClientVisible="False" Text="QC Level 1" UseSubmitBehavior="False"
																								Width="200px" Theme="NETheme01">
																								<ClientSideEvents Click="function(s, e) {
cb_qc_customer.PerformCallback(&quot;1&quot;);
}" />

																								<Image Url="~/images/icon/icon[approve].gif">
																								</Image>
																							</dx:ASPxButton>
																						</td>
																					</tr>
																					<tr>
																						<td colspan="2"><dx:ASPxButton ID="bt_qc2approve" runat="server" AutoPostBack="False" 
																								ClientInstanceName="bt_qc2approve" ClientVisible="False" Theme="NETheme01" Text="QC Level 2" UseSubmitBehavior="False" Width="200px">
																								<ClientSideEvents Click="function(s, e) {
																									cb_qc_customer.PerformCallback(&quot;2&quot;);
																													}" />
																								<Image Url="~/images/icon/icon[approve].gif">
																								</Image>
																							</dx:ASPxButton>
																						</td>
																						
																					</tr>	
																					<tr>
																						<td>
																							<dx:ASPxImage ID="im_qc_approve" runat="server" ClientInstanceName="im_qc_approve"
																								ClientVisible="False" ImageUrl="~/images/loading_panel.gif">
																							</dx:ASPxImage>
																						</td>
																						<td>
																						</td>
																					</tr>		
																					<tr>
																						<td>
																							<dx:ASPxCallback ID="cb_qc_customer" runat="server" ClientInstanceName="cb_qc_customer"
																								OnCallback="cb_qc_customer_Callback">
																								<ClientSideEvents BeginCallback="function(s, e) {
	im_qc_approve.SetVisible(true);
	bt_qcapprove.SetEnabled(false);
	bt_qc2approve.SetEnabled(false);
}" CallbackComplete="function(s, e) {
	var res = e.result.toString().split(&quot;|&quot;);
	var version = res[0];
	var date	= res[1];
	if(version == &quot;1&quot;)
		{
		lb_qc_date.SetText(res[1]);
		if(lb_qc_date.GetText().match(/\d\d\d\d-\d\d-\d\d/g))
			{
cust_main_cbpanel.PerformCallback('load_customer');
			}
		else
			{
			alert(&quot;There was an error saving&quot;);
			}
		}
	else if(version == &quot;2&quot;)
		{
		lb_qc2_date.SetText(res[1]);
		if(lb_qc2_date.GetText().match(/\d\d\d\d-\d\d-\d\d/g))
			{
cust_main_cbpanel.PerformCallback('load_customer');
			}
		else
			{
			alert(&quot;There was an error saving&quot;);
			}
		}
}" EndCallback="function(s, e) {
	im_qc_approve.SetVisible(false);
}" />

																							</dx:ASPxCallback>
																						</td>
																						<td></td>
																					</tr>
																				</table>
																				</div>
																			</td>
																		</tr>
																		<tr>
																			<td valign="top">
																				<dx:ASPxPageControl ID="right_tabs" runat="server" ActiveTabIndex="0" 
																					Width="100%" ClientInstanceName="right_tabs" 
																					meta:resourcekey="right_tabsResource1" EnableTheming="True" EnableCallbacks="True" AutoPostBack="True" OnActiveTabChanged="right_tabs_ActiveTabChanged" Theme="NETheme01" ContentStyle-BackColor="White">
																					<TabPages>
																						<dx:TabPage Text="Locations" Name="locations">
																							<TabImage Url="~/images/icon/icon[company].gif">
																							</TabImage>
																							<ContentCollection>
																								<dx:ContentControl ID="ContentControl3" runat="server">
																											<uc:locations ID="uc_locations" runat="server" />
																								</dx:ContentControl>
																							</ContentCollection>
																						</dx:TabPage>
																						<dx:TabPage Text="Accounting" Name="accounting">
																							<ContentCollection>
																								<dx:ContentControl ID="ContentControl8" runat="server">
																									<table cellpadding="2" cellspacing="0" width="100%" style="font-family: Arial">
																										<tr>
																											<td width="99%">
																														<uc:accounting ID="uc_accounting" runat="server" />
																											</td>
																										</tr>
																									</table>
																								</dx:ContentControl>
																							</ContentCollection>
																							<TabImage Url="~/images/icon/icon[bank].gif">
																							</TabImage>
																						</dx:TabPage>
																						<dx:TabPage Text="Grids">
																							<TabImage Height="16px" Url="~/images/icon/icon[report].gif" Width="16px">
																							</TabImage>
																							<ContentCollection>
																								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																								
																				<div class='address_list_title'>Active Address
																				</div>
																				
																									<dx:ASPxComboBox ID="ddl_address" DataSourceID="ds_ddl_address" runat="server" ValueType="System.Int32" TextField="text" ValueField="value" Width="100%" AutoPostBack="False">
																									<ClientSideEvents SelectedIndexChanged="function(s, e) {
gv_workorders.Refresh();


}"  />
																									 </dx:ASPxComboBox>
																									<br></br>
																									<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" TabPosition="Left" Width="100%" EnableCallbacks="True" >
																										<TabPages>
																											<dx:TabPage Text="Work Orders">
																												<ContentCollection>
																													<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																									<dx:ASPxGridView ID="gv_workorders" runat="server" Width="100%" 
																										 AutoGenerateColumns="False" 
																										DataSourceID="ds_workorders" Theme="NETheme01"
																										ClientInstanceName="gv_workorders"
																										>
																										<Columns>
																											<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
																												VisibleIndex="0" Width="5px" ShowClearFilterButton="true">
																												
																											</dx:GridViewCommandColumn>
																											<dx:GridViewDataTextColumn Caption="WO#" VisibleIndex="1" Width="80px" SortOrder="Descending" SortIndex="1">
																												<DataItemTemplate>
																													<dx:ASPxHyperLink ID="ASPxHyperLink2" runat="server" Font-Bold="True"  NavigateUrl='<%# string.Format("javascript:NE_Customer.get_wo({0});", Eval("woprog_id")) %>'
																														Text='<%# Eval("wo_number") %>' Font-Names="Arial" Font-Size="9pt">
																													</dx:ASPxHyperLink>
																												</DataItemTemplate>
																												<HeaderStyle HorizontalAlign="Center" />
																												
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataDateColumn Caption="Cut Date" FieldName="cut_dt" VisibleIndex="2"
																												Width="100px" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd">
																												<HeaderStyle HorizontalAlign="Center" />
																												<CellStyle HorizontalAlign="Center" Wrap="False">
																												</CellStyle>
																											</dx:GridViewDataDateColumn>
																											<dx:GridViewDataDateColumn Caption="Close Date" FieldName="close_dt" VisibleIndex="3"
																												Width="100px" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd">
																												<HeaderStyle HorizontalAlign="Center" />
																												<CellStyle HorizontalAlign="Center" Wrap="False">
																												</CellStyle>
																											</dx:GridViewDataDateColumn>
																											<dx:GridViewDataTextColumn Caption="Contact" MinWidth="100" FieldName="contact_name" Visible="True" ShowInCustomizationForm="True" VisibleIndex="4">
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Description" FieldName="wo_description" 
																												VisibleIndex="5" Width="100%">
																												<CellStyle Wrap="False">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Quote #" FieldName="quote_id" VisibleIndex="6"
																												Width="80px" UnboundType="String">
																												<DataItemTemplate>
																													<dx:ASPxHyperLink ID="hl_quote" runat="server" OnInit="hl_quote_Init" 
																														Font-Bold="True" NavigateUrl="javascript:void(0);" 
																														Text='<%# string.Format("&nbsp;{0}", Eval("quote_id")) %>' Font-Names="Arial" 
																														Font-Size="9pt">
																													</dx:ASPxHyperLink>
																												</DataItemTemplate>
																												<HeaderStyle HorizontalAlign="Center" />
																												<CellStyle HorizontalAlign="Center">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Rev" FieldName="rev" Visible="False" 
																												VisibleIndex="7">
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="8" 
																												Width="100px">
																												<HeaderStyle HorizontalAlign="Center" />
																												<CellStyle HorizontalAlign="Center" Wrap="False">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Address" FieldName="addy" 
																												ShowInCustomizationForm="True" VisibleIndex="9" Width="100px">
																												<CellStyle Wrap="False">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																										</Columns>
																										<SettingsBehavior ColumnResizeMode="Control" />

																										<SettingsPager NumericButtonCount="5">
																										</SettingsPager>
																										<Settings ShowFilterRow="True" 
																											VerticalScrollableHeight="400" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />

																										
																									</dx:ASPxGridView>
																									<asp:SqlDataSource ID="ds_workorders" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
																										>
																										<SelectParameters>
																											<asp:SessionParameter Name="@customer_id" SessionField="customer_id" />
																											<asp:ControlParameter Name="address_id" ControlID="ddl_address" />
																										</SelectParameters>
																									</asp:SqlDataSource>
																													</dx:ContentControl>
																												</ContentCollection>
																											</dx:TabPage>
																											<dx:TabPage Text="Quotes">
																												<ContentCollection>
																													<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																									<dx:ASPxGridView ID="gv_quotes" runat="server" Width="100%" 
																										AutoGenerateColumns="False" DataSourceID="ds_quotes" 
																										 Theme="NETheme01" ClientInstanceName="gv_quotes1">
																										<Columns>
																											<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
																												VisibleIndex="0" Width="5px" ShowClearFilterButton="true">
																												
																											</dx:GridViewCommandColumn>
																											<dx:GridViewDataTextColumn Caption="Quote #" FieldName="quote_id" VisibleIndex="1"
																												Width="100px" SortIndex="0" SortOrder="Descending">
																												<DataItemTemplate>
																													<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text='<%# Eval("quote_id") %>' Font-Bold="True" NavigateUrl='<%# string.Format("javascript:NE_Customer.get_quote({0});", Eval("quote_id")) %>'>
																													</dx:ASPxHyperLink>
																												</DataItemTemplate>
																												<CellStyle HorizontalAlign="Center">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Ver" FieldName="revision" VisibleIndex="2" 
																												Width="35px">
																												<CellStyle HorizontalAlign="Center">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataDateColumn Caption="Open Date" FieldName="open_date" VisibleIndex="3"
																												Width="75px" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd">
																												<Settings AutoFilterCondition="Equals" />

																												<CellStyle HorizontalAlign="Center">
																												</CellStyle>
																											</dx:GridViewDataDateColumn>
																											<dx:GridViewDataTextColumn Caption="Description" FieldName="job_description" 
																												VisibleIndex="4">
																												<Settings AutoFilterCondition="Contains" />

																												<HeaderStyle HorizontalAlign="Left" />
																												<CellStyle HorizontalAlign="Left">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Quoted By" FieldName="quoted_by" VisibleIndex="5"
																												Width="200px">
																												<Settings AutoFilterCondition="Contains" />

																												<CellStyle HorizontalAlign="Center">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="6" 
																												Width="125px">
																												<Settings AutoFilterCondition="Equals" />

																												<CellStyle HorizontalAlign="Center">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																										</Columns>
																										<SettingsBehavior EnableRowHotTrack="True" />

																										<SettingsPager NumericButtonCount="5">
																										</SettingsPager>
																										<Settings ShowHeaderFilterButton="True" ShowFilterRow="True" 
																											VerticalScrollableHeight="400" ShowFooter="True" />

																										
																									</dx:ASPxGridView>
																									<asp:SqlDataSource ID="ds_quotes" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 
a.quote_id, 
a.revision,
MEMBER_NAME(a.quoted_by) quoted_by, 
DATE_FORMAT(a.open_date, &quot;%Y-%m-%d&quot;) open_date,
URLDECODE(a.job_description) job_description,
b.status
  FROM quote_master a
LEFT JOIN quote_status b ON a.status_id = b.id
WHERE a.customer_id = @customer_id AND a.quoted_by != 711 AND a.address_id = @address_id AND a.active_revision = true ORDER BY open_date DESC">
																										<SelectParameters>
																											<asp:SessionParameter Name="@customer_id" SessionField="customer_id" />
																											<asp:ControlParameter ControlID="ddl_address" Name="@address_id" PropertyName="Value" />
																										</SelectParameters>
																									</asp:SqlDataSource>
																													</dx:ContentControl>
																												</ContentCollection>
																											</dx:TabPage>
																											<dx:TabPage Text="Phone Calls">
																												<ContentCollection>
																													<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																									<dx:ASPxGridView ID="gv_phonecalls" runat="server" Width="100%" 
																										AutoGenerateColumns="False" 
																										DataSourceID="ds_phonecalls"  Theme="NETheme01" ClientInstanceName="gv_phonecalls">
																										<Columns>
																											<dx:GridViewDataTextColumn Caption="Date" FieldName="phone_log_date" VisibleIndex="0"
																												Width="125px" SortIndex="0" SortOrder="Descending">
																												<DataItemTemplate>
																													<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("phone_log_date", "{0:d}") %>'>
																													</dx:ASPxLabel>
																												</DataItemTemplate>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="From #" FieldName="phone_log_from_number" VisibleIndex="1">
																												<CellStyle HorizontalAlign="Center">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="From Name" FieldName="phone_log_from_name" VisibleIndex="2">
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="To #" FieldName="phone_log_to_number" VisibleIndex="3">
																												<CellStyle HorizontalAlign="Center">
																												</CellStyle>
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="To Name" FieldName="phone_log_to_name" VisibleIndex="4">
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Duration" FieldName="phone_log_duration" VisibleIndex="5"
																												Width="50px">
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" VisibleIndex="6"
																												Width="50px">
																											</dx:GridViewDataTextColumn>
																										</Columns>
																										<SettingsPager NumericButtonCount="100" PageSize="100">
																										</SettingsPager>
																										<Styles>
																											
																											<Cell HorizontalAlign="Center">
																											</Cell>
																										</Styles>
																									</dx:ASPxGridView>
																									<asp:SqlDataSource ID="ds_phonecalls" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL customer_phone_log(@customer_id)">
																										<SelectParameters>
																											<asp:SessionParameter Name="@customer_id" SessionField="customer_id" />
																										</SelectParameters>
																									</asp:SqlDataSource>
																													</dx:ContentControl>
																												</ContentCollection>
																											</dx:TabPage>
																											<dx:TabPage Text="Logins" Visible="false">
																												<ContentCollection>
																													<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																													</dx:ContentControl>
																												</ContentCollection>
																											</dx:TabPage>
																											<dx:TabPage Text="Emails">
																												<ContentCollection>
																													<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																									<dx:ASPxGridView ID="gv_emails" runat="server" Width="100%" ClientInstanceName="gv_emails" 
																										DataSourceID="ds_emails" AutoGenerateColumns="False" Theme="NETheme01">
																									<Columns>
																										<dx:GridViewDataDateColumn Caption="Date" FieldName="date" FixedStyle="Left" VisibleIndex="0" PropertiesDateEdit-DisplayFormatString="yyyy-MM-dd hh:mm:ss" SortOrder="Descending" SortIndex="1">
																										</dx:GridViewDataDateColumn>
																										<dx:GridViewDataTextColumn Caption="From Address" FieldName="from_address" VisibleIndex="1">
																										</dx:GridViewDataTextColumn>
																										<dx:GridViewDataTextColumn Caption="To Address" FieldName="to_address" VisibleIndex="2">
																										</dx:GridViewDataTextColumn>
																										<dx:GridViewDataTextColumn Caption="Subject" FieldName="subject" VisibleIndex="3" Visible="False">
																										</dx:GridViewDataTextColumn>
																									</Columns>

																										<Styles>
																											<Cell Wrap="False">
																											</Cell>
																										</Styles>
																								</dx:ASPxGridView>
																														<asp:SqlDataSource  ID="ds_emails" runat="server"
																														ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																														ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommandType="Text" SelectCommand="Call CUSTEMAILS(@cid)">
																														<SelectParameters>
																														<asp:SessionParameter SessionField="customer_id" Name="@cid" />
																														</SelectParameters> 
																														</asp:SqlDataSource>
																														
																													</dx:ContentControl>
																												</ContentCollection>
																											</dx:TabPage>
																											<dx:TabPage Text="Parts Sold" Visible="false">
																												<ContentCollection>
																													<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																									<dx:ASPxGridViewExporter ID="ex_gv_parts" runat="server" GridViewID="gv_parts">
																									</dx:ASPxGridViewExporter>
																									<dx:ASPxButton ID="bt_parts_excel" runat="server" OnClick="bt_parts_excel_Click" Text="Export">
																										<Image Url="~/images/icon/icon[excel].gif">
																										</Image>
																									</dx:ASPxButton>
																									<dx:ASPxGridView ID="gv_parts" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_parts"  OnHtmlDataCellPrepared="ASPxGridView2_HtmlDataCellPrepared" Width="100%" Theme="NETheme01">
																										<Columns>
																											<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" Visible="False" VisibleIndex="0" ShowClearFilterButton="true">
																												
																											</dx:GridViewCommandColumn>
																											<dx:GridViewDataTextColumn Caption="Master ID" FieldName="masterid" ShowInCustomizationForm="True" VisibleIndex="1" Width="125px">
																												<Settings AutoFilterCondition="BeginsWith" />
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Description" FieldName="description" ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
																												<Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Qty" FieldName="qty" ShowInCustomizationForm="True" VisibleIndex="3" Width="100px">
																												<PropertiesTextEdit DisplayFormatString="N1">
																												</PropertiesTextEdit>
																												<Settings AutoFilterCondition="GreaterOrEqual" />
																											</dx:GridViewDataTextColumn>
																											<dx:GridViewDataTextColumn Caption="Average Sell" FieldName="avg_sell" ShowInCustomizationForm="True" VisibleIndex="4" Width="100px" Visible="False">
																												<PropertiesTextEdit DisplayFormatString="C2">
																												</PropertiesTextEdit>
																											</dx:GridViewDataTextColumn>
																										</Columns>
																										<SettingsBehavior ColumnResizeMode="Control" />
																										<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" />
																									</dx:ASPxGridView>
																													</dx:ContentControl>
																												</ContentCollection>
																											</dx:TabPage>

																											<dx:TabPage Text="Specialties" Visible="true">
																												<ContentCollection>
																													<dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">
																													<uc1:partner_skills ID="partner_skills1" runat="server" />

																													</dx:ContentControl>
																												</ContentCollection>
																											</dx:TabPage>

																										</TabPages>
																										
																									</dx:ASPxPageControl>
																									<br />
																								</dx:ContentControl>
																							</ContentCollection>
																						</dx:TabPage>
																						<dx:TabPage Text="Work Orders" Visible="False">
																							<TabImage Url="~/images/icon/icon[report].gif">
																							</TabImage>
																							<ContentCollection>
																								<dx:ContentControl ID="ContentControl10" runat="server">
																								</dx:ContentControl>
																							</ContentCollection>
																						</dx:TabPage>
																						<dx:TabPage Text="Quotes" Visible="False">
																							<TabImage Url="~/images/icon/icon[edit].gif">
																							</TabImage>
																							<ContentCollection>
																								<dx:ContentControl ID="ContentControl11" runat="server">
																								</dx:ContentControl>
																							</ContentCollection>
																						</dx:TabPage>
																						<dx:TabPage Text="Phone Calls" Visible="False">
																							<TabImage Url="~/images/icon/icon[mobile].gif">
																							</TabImage>
																							<ContentCollection>
																								<dx:ContentControl ID="ContentControl12" runat="server">
																								</dx:ContentControl>
																							</ContentCollection>
																						</dx:TabPage>
																						<dx:TabPage Text="Emails" Visible="False">
																							<TabImage Url="~/images/icon/icon[email].gif">
																							</TabImage>
																							<ContentCollection>
																								<dx:ContentControl ID="ContentControl13" runat="server">
																								</dx:ContentControl>
																							</ContentCollection>
																						</dx:TabPage>
																						<dx:TabPage Text="" Name="bv" Visible="False">
																							<TabImage Url="~/images/icon/icon[bv].gif">
																							</TabImage>
																							<TabStyle BackColor="DimGray" Height="25px" Font-Size="12px">
																							</TabStyle>
																							<ContentCollection>
																								<dx:ContentControl ID="ContentControl14" runat="server">
																									<div style="width: 100%; padding-right: 50px; padding-left: 50px; padding-bottom: 50px; padding-top: 50px;" align="center">
																										<div style="width: 550px;" align="left">
																									<dx:ASPxButton ID="btn_bvrefresh" runat="server" AutoPostBack="False" Text="Refresh"
																										UseSubmitBehavior="False">
																										<ClientSideEvents Click="function(s, e) {
		bv_data.PerformCallback();
}" />
																									</dx:ASPxButton>
																									<dx:ASPxDataView ID="bv_data" runat="server" AllowPaging="False" ColumnCount="4"
																										ItemSpacing="2px" PagerPanelSpacing="2px" RowPerPage="10" Width="700px" ClientInstanceName="bv_data" OnCustomCallback="bv_data_CustomCallback" EmptyDataText=""  meta:resourcekey="bv_dataResource1">
																										<PagerSettings ShowNumericButtons="False">
																										</PagerSettings>
																										<ItemTemplate>
																											<table id="Table1" width="100%" runat="server" style="<%# BVSynced(Container.DataItem) %>">
																												<tr id="Tr1" runat="server">
																													<td id="Td1" colspan="2" runat="server">
																														<dx:ASPxLabel ID="bv_name" runat="server" Font-Bold="True" Font-Underline="True" Text='<%# Eval("bv_name") %>'>
																														</dx:ASPxLabel>
																													</td>
																												</tr>
																												<tr id="Tr2" runat="server">
																													<td id="Td2" style="width: 70px" runat="server">
																														<strong style="font-size: 11px">Cust.Exists?:</strong></td>
																													<td id="Td3" runat="server">
																														<dx:ASPxLabel ID="bv_exists" runat="server" Font-Size="11px" Text='<%# Eval("bv_exists") %>'>
																														</dx:ASPxLabel>
																													</td>
																												</tr>
																												<tr id="Tr3" runat="server">
																													<td id="Td4" runat="server" style="width: 70px">
																														<strong style="font-size: 11px">Addr.Exists?:</strong></td>
																													<td id="Td5" runat="server">
																														<dx:ASPxLabel ID="bv_addr_exists" runat="server" Font-Size="11px" Text='<%# Eval("bv_addr_exists") %>'>
																														</dx:ASPxLabel>
																													</td>
																												</tr>
																												<tr id="Tr4" runat="server">
																													<td id="Td6" style="width: 70px" runat="server">
																														<strong style="font-size: 11px">DSN:</strong></td>
																													<td id="Td7" runat="server">
																														<dx:ASPxLabel ID="dsnbv" runat="server" Font-Size="11px" Text='<%# Eval("bv_dsn") %>'>
																														</dx:ASPxLabel>
																													</td>
																												</tr>
																												<tr id="Tr5" runat="server">
																													<td id="Td8" style="width: 150px" runat="server">
																														<dx:ASPxHyperLink ID="synclink" runat="server" Font-Bold="True" Font-Size="11px"
																															Font-Underline="True" NavigateUrl='<%# "javascript:sync_bv("+Eval("business_unit_id")+");" %>'
																															Text="Sync" ForeColor="Black">
																														</dx:ASPxHyperLink>
																													</td>
																													<td id="Td9" runat="server">
																													</td>
																												</tr>
																											</table>
																										</ItemTemplate>
																										<Paddings Padding="2px" />

																										<ItemStyle Height="50px" >
																											<Paddings Padding="0px" />
																											<Border BorderColor="Black" />
																										</ItemStyle>
																									</dx:ASPxDataView>
																										</div>
																									</div>
																								</dx:ContentControl>
																							</ContentCollection>
																						</dx:TabPage>
																						<dx:TabPage Name="Parts_Sold" Text="Parts Sold" Visible="False">
																							<TabImage Url="~/images/icon/icon[ball].gif">
																							</TabImage>
																							<ContentCollection>
																								<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
																									<br />
																								</dx:ContentControl>
																							</ContentCollection>
																						</dx:TabPage>
																					</TabPages>
																					<ContentStyle>
																						<Paddings Padding="2px"></Paddings>
                                                                                       
																					</ContentStyle>
																					<TabStyle  Height="25px" Width="200px" Paddings-PaddingTop="0px"  Paddings-Padding="6px">

																					</TabStyle>
                                                                                    
																					<ClientSideEvents ActiveTabChanged="NE_Customer.tab_change">

																					</ClientSideEvents>
																				</dx:ASPxPageControl>
																			</td>
																		</tr>
																	</table>
																</dx:ContentControl>
															</ContentCollection>
															<TabImage Url="~/images/icon/icon[attribute_values].gif">
															</TabImage>
                                                          
														</dx:TabPage>
													</TabPages>
													<ContentStyle VerticalAlign="Top">
														<Paddings Padding="2px" />
													</ContentStyle>
													<ClientSideEvents ActiveTabChanged="function(s, e) {
														bind_tooltips();
													}" TabClick="function(s, e)
														{
														var _custid		= main_persistence_handler.Get(&quot;customer_id&quot;);
														if(e.tab.index == 1 &amp;&amp; (_custid == undefined || _custid == null || _custid == &quot;&quot;))
															{
															e.cancel		= true;
															}
														}"></ClientSideEvents>
													<LoadingPanelStyle VerticalAlign="Top"></LoadingPanelStyle>
												</dx:ASPxPageControl>
														</td>
													</tr>
												</table>
		<dx:ASPxPopupControl ID="new_customer_popup" runat="server" ClientInstanceName="new_customer_popup"
			CssClass="pop_up" HeaderText="New Customer" PopupHorizontalAlign="WindowCenter"
			PopupVerticalAlign="TopSides" Width="750px" Modal="False" meta:resourcekey="new_customer_popupResource1" 
								Font-Names="Arial" Font-Size="10pt" PopupAnimationType="None" CloseAction="CloseButton" AllowDragging="True">
			<ContentCollection>
				<dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" meta:resourcekey="PopupControlContentControlResource1">
					<uc:new ID="uc_new" runat="server" />
				</dx:PopupControlContentControl>
			</ContentCollection>
			<HeaderImage Url="~/images/icon/icon[depthead].gif">
			</HeaderImage>
			<HeaderStyle BackColor="#00CC33" Font-Bold="True" ForeColor="White" />
		</dx:ASPxPopupControl>
							<br />
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxPanel>
			</dx:PanelContent>
		</PanelCollection>
		<ClientSideEvents BeginCallback="function(s,e){please_wait('start');}" EndCallback="function(s, e) { please_wait('end');	bind_tooltips();}"></ClientSideEvents>
	</dx:ASPxCallbackPanel>
		&nbsp;
			</ContentTemplate>
		</asp:UpdatePanel>
	</form>
</body>
</html>