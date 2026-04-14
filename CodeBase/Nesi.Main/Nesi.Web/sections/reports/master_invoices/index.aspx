<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true"  Inherits="sections_reports_master_invoices_index" Title="Master Invoices" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>



<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<asp:content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">
		function email(id) {
			boing("/sections/reports/invoice_preview/index.aspx?id=" + id, 'invoice_preview', 850, 850);
		}
		</script>
	<lc:layoutcontrol runat="server" id="layout" is_private="True" />
	<br />
	<dx:aspxgridview id="gv_invoices" runat="server" autogeneratecolumns="False"
		width="100%" ClientInstanceName="gv_invoices" 
		OnCustomJSProperties="gv_invoices_CustomJSProperties" 
		OnCustomCallback="gv_invoices_CustomCallback" KeyFieldName="WOProg_ID" 
		onhtmleditformcreated="gv_invoices_HtmlEditFormCreated" 
		onrowupdating="gv_invoices_RowUpdating" Font-Size="9pt" 
		onhtmlrowprepared="gv_invoices_HtmlRowPrepared" Theme="NETheme01">

<ClientSideEvents CustomButtonClick="function(s, e) {
	if(e.buttonID == &#39;invoice&#39;){
                    var rowVisibleIndex = e.visibleIndex;
                    var rowKeyValue;
					s.GetRowValues( e.visibleIndex,&#39;WOProg_ID&#39;,email);
				//	alert(rowKeyValue);
				//	email(rowKeyValue);
}
}"></ClientSideEvents>

		<TotalSummary>
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="WOProg_InvoicedNetTotal" 
				ShowInColumn="Invoice Amt" ShowInGroupFooterColumn="Customer" SummaryType="Sum" 
				ValueDisplayFormat="C0" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="lab_cost" 
				ShowInColumn="Lab Cost" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="mat_cost" 
				ShowInColumn="Mat Cost" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="balance" 
				ShowInColumn="Balance" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="N1" FieldName="invoice_lag" 
				ShowInColumn="Invoice Lag" SummaryType="Average" />
		</TotalSummary>
        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
		<Columns>
			<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0"  ShowEditButton="true" 
				Width="50px" ShowSelectCheckbox="True">
				
				<CustomButtons>
					<dx:GridViewCommandColumnCustomButton ID="invoice">
						<Image Url="~/images/icon/icon[email].gif">
						</Image>
					</dx:GridViewCommandColumnCustomButton>
				</CustomButtons>
				<CellStyle VerticalAlign="Middle">
				</CellStyle>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn FieldName="ID" Visible="False" VisibleIndex="14" 
				ReadOnly="True">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="WOProg_ID" Visible="False" 
				VisibleIndex="16" ReadOnly="True">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Work Order" FieldName="WOProg_BVWO" 
				VisibleIndex="11" Width="80px" ReadOnly="True" MinWidth="10">
				<PropertiesTextEdit>
					<Style>
						<Border BorderStyle="None" />
					</Style>
				</PropertiesTextEdit>
				<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
				<DataItemTemplate>
							<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
								NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 'workorder', 1200,800)&quot;, Eval(&quot;WOProg_ID&quot;), Eval(&quot;business_unit_id&quot;)) %>" 
								Text='<%# Eval("WOProg_BVWO") %>'>
							</dx:ASPxHyperLink>
						</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Customer" FieldName="WOProg_CustomerName" 
				VisibleIndex="5" Width="90px" ReadOnly="True" MinWidth="10">
				<PropertiesTextEdit>
					<Style>
						<Border BorderStyle="None" />
					</Style>
				</PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataComboBoxColumn Caption="Customer Status" 
				FieldName="customer_collection_status" VisibleIndex="6" Width="100px" MinWidth="10">
				<PropertiesComboBox DataSourceID="sql_cust_status" TextField="status_name" 
					ValueField="ID" ValueType="System.Int32">
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Cust PO" FieldName="WOProg_CustPO" 
				VisibleIndex="12" Width="50px" MinWidth="10">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="business_unit" 
				VisibleIndex="3" Width="50px" ReadOnly="True" MinWidth="10">
				<PropertiesTextEdit>
					<Style>
						<Border BorderStyle="None" />
					</Style>
				</PropertiesTextEdit>
				<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Invoice Amt" 
				FieldName="WOProg_InvoicedNetTotal" VisibleIndex="7" Width="90px" ReadOnly="True" 
				MinWidth="10">
				<PropertiesTextEdit DisplayFormatString="C2">
					<Style>
						<Border BorderStyle="None" />
					</Style>
				</PropertiesTextEdit>
				<CellStyle Font-Bold="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Confirmed" 
				FieldName="woprog_ExpectedCheckRun" VisibleIndex="9" Width="80px" MinWidth="10">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd" DisplayFormatInEditMode="True">
				</PropertiesDateEdit>
				<CellStyle BackColor="#CCFFCC">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataComboBoxColumn Caption="Invoice Status" 
				FieldName="woprog_invoice_collection_status" VisibleIndex="10" Width="90px" 
				MinWidth="10">
				<PropertiesComboBox DataSourceID="sql_inv_status" 
					TextField="invoice_collection_status_name" 
					ValueField="invoice_collection_status_id" ValueType="System.Int32">
				</PropertiesComboBox>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Invoice" FieldName="WOProg_InvoiceNo" 
				VisibleIndex="1" Width="80px" ReadOnly="True" MinWidth="10">
				<PropertiesTextEdit>
					<Style>
						<Border BorderStyle="None" />
					</Style>
				</PropertiesTextEdit>
				<CellStyle BackColor="#FFFFCC">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Invoice Date" 
				FieldName="WOProg_InvoiceDate" VisibleIndex="2" Width="80px" ReadOnly="True" 
				MinWidth="10">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
					<Style>
						<Border BorderStyle="None" />
					</Style>
				</PropertiesDateEdit>
				<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataDateColumn FieldName="woprog_invoice_paid_date" Visible="False" 
				VisibleIndex="28" ReadOnly="True" ShowInCustomizationForm="False">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataDateColumn Caption="Exp Pay Date" FieldName="exp_pay" 
				VisibleIndex="8" Width="80px" ReadOnly="True" MinWidth="10">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
					<Style>
						<Border BorderStyle="None" />
					</Style>
				</PropertiesDateEdit>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn FieldName="woprog_customer_id" Visible="False" 
				VisibleIndex="26" ReadOnly="True" ShowInCustomizationForm="False">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn FieldName="business_unit_id" Visible="False" 
				VisibleIndex="27" ReadOnly="True" ShowInCustomizationForm="False">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Invoice Notes" FieldName="ar_notes_note" 
				VisibleIndex="13" Width="100%" MinWidth="10">
				<EditFormSettings ColumnSpan="4" />
<EditFormSettings ColumnSpan="4"></EditFormSettings>
				<EditItemTemplate>
					<asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
					<dx:ASPxGridView ID="gv_invoicenotes" runat="server" 
												AutoGenerateColumns="False" ClientInstanceName="gv_invoicenotes" DataSourceID="sql_invnotes" 
												KeyFieldName="ar_notes_id" Font-Names="Arial" onrowinserting="gv_invoicenotes_RowInserting" 
												onrowupdating="gv_invoicenotes_RowUpdating" Theme="NETheme01">
                        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
												<Columns>
													<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0"  ShowEditButton="true" ShowNewButton="true"
														Width="40px">
														
														
														<CellStyle Wrap="False">
														</CellStyle>
													</dx:GridViewCommandColumn>
													<dx:GridViewDataTextColumn Caption="Date" FieldName="_date" VisibleIndex="1" 
														Width="150px">
														<EditFormSettings Visible="False" />
														<EditCellStyle Wrap="False">
														</EditCellStyle>
														<CellStyle Wrap="False">
														</CellStyle>
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataTextColumn Caption="Member" FieldName="member" VisibleIndex="2" 
														Width="100px">
														<EditFormSettings Visible="False" />
													</dx:GridViewDataTextColumn>
													<dx:GridViewDataMemoColumn Caption="Notes" FieldName="note" VisibleIndex="3" 
														Width="300px">
														<PropertiesMemoEdit Height="150px" Width="300px"><Style BackColor="#FFFFCC" Font-Names="Arial"><Paddings Padding="4px" /></Style></PropertiesMemoEdit>
														<EditFormSettings Caption=" " />
														<CellStyle BackColor="#FFFFCC">
														</CellStyle>
													</dx:GridViewDataMemoColumn>
													<dx:GridViewDataTextColumn FieldName="ar_notes_id" Visible="False" 
														VisibleIndex="4">
													</dx:GridViewDataTextColumn>
												</Columns>
												<SettingsEditing Mode="PopupEditForm" />
                                                <SettingsPopup EditForm-Height="250px" 
													EditForm-HorizontalAlign="WindowCenter" EditForm-Modal="True" 
													EditForm-VerticalAlign="WindowCenter" EditForm-Width="400px"/>
													<Templates>
								<EditForm>
													 <dx:ASPxGridViewTemplateReplacement ID="Editors1" ReplacementType="EditFormEditors"
                                runat="server">
                            </dx:ASPxGridViewTemplateReplacement>
                            <div style="margin-top: 10px; margin-bottom: 20px; padding-bottom:20px;">
                                <div style="float: left; margin-left: 5px">
                                    <dx:ASPxButton ID="btnNoteCancel" runat="server" AutoPostBack="false" Text="Cancel" Width="100px"
                                        ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' />
                                </div>
                                <div style="float: left; margin-left: 10px">
                                    <dx:ASPxButton ID="btnNoteSave" runat="server" AutoPostBack="false" Text="Save" Width="100px"
                                        CssClass="input" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' />
                                </div>
                            </div></EditForm></Templates></dx:ASPxGridView>
							<asp:SqlDataSource ID="sql_invnotes" runat="server" 
												ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
												ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
												
												SelectCommand="Select ar_notes_id, get_name(ar_notes_memberid) as member,ar_notes_ts as _date, urldecode(ar_notes_note) as note from ar_notes where ar_notes_woprogid = ?woid order by ar_notes_id desc" 
												onselecting="sql_invnotes_Selecting">
												<SelectParameters>
													<asp:Parameter Name="woid" />
												</SelectParameters>
											</asp:SqlDataSource>
				</EditItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Days Since" FieldName="dayssince" 
				ReadOnly="True" VisibleIndex="15" Width="60px" MinWidth="10" ToolTip="Days since invoiced">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="email" FieldName="customer_emails" 
				ReadOnly="True" Visible="False" VisibleIndex="25" Width="60px">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="phone" FieldName="phone" ReadOnly="True" 
				Visible="False" VisibleIndex="24" Width="60px">
				<EditFormSettings Visible="True" />
<EditFormSettings Visible="True"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Customer No" FieldName="cust_no" 
				VisibleIndex="17" Width="50px">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Balance" FieldName="balance" 
				ReadOnly="True" VisibleIndex="18" Width="40px" MinWidth="10">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Mat Cost" FieldName="mat_cost" 
				VisibleIndex="19" Width="60px">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
				<CellStyle BackColor="#FFCC99">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Lab Cost" FieldName="lab_cost" 
				VisibleIndex="20" Width="60px">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
				<CellStyle BackColor="#CCFF66">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Invoice Lag" FieldName="invoice_lag" 
				MinWidth="15" 
				ToolTip="Days taken to prepare the invoice from the last day worked on the work order" 
				Visible="False" VisibleIndex="23">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="PM" FieldName="pm" MinWidth="20" 
				Visible="False" VisibleIndex="22">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Note Date" FieldName="note_date" 
				MinWidth="10" VisibleIndex="21" Width="75px">
				<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Account Manager" FieldName="acct_manager" 
				Visible="False" VisibleIndex="74" MinWidth="50">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Scan Time" FieldName="scan_time" 
				ToolTip="Shows the number of days between the last hour worked and when the work order was scanned" 
				Visible="False" VisibleIndex="76">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Processing Time" FieldName="processing_time" ToolTip="Shows the elapsed time from then the work order was scanned to when it was invoiced." Visible="False" VisibleIndex="80">
			</dx:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="WO Terms" FieldName="terms" 
				VisibleIndex="78" Width="100px">
			</dx1:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Cust Terms" FieldName="cust_terms" 
				VisibleIndex="79" Width="100px">
			</dx1:GridViewDataTextColumn>
			<dx1:gridviewdatatextcolumn caption="Contact Name" fieldname="contact_name" visible="False" visibleindex="81">
			</dx1:gridviewdatatextcolumn>
			<dx1:gridviewdatatextcolumn caption="Contact Email" fieldname="contact_email" visible="False" visibleindex="82">
			</dx1:gridviewdatatextcolumn>
		</Columns>

<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
			ProcessFocusedRowChangedOnServer="True" EnableCustomizationWindow="True"></SettingsBehavior>

<SettingsPager PageSize="50"></SettingsPager>

		<SettingsEditing Mode="PopupEditForm" EditFormColumnCount="4" 
			/>

<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowFilterRowMenu="True" 
			ShowFilterBar="Visible" ShowHeaderFilterButton="True" ShowFooter="True" 
			HorizontalScrollBarMode="Visible" ColumnMinWidth="10" />
		<SettingsText PopupEditFormCaption="Edit Invoice Collection" />

<SettingsEditing Mode="PopupEditForm" EditFormColumnCount="4"></SettingsEditing>

<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowFilterRowMenu="True" 
			ShowHeaderFilterButton="True" ShowFooter="True" ShowFilterBar="Visible" 
			HorizontalScrollBarMode="Visible"></Settings>

<SettingsText PopupEditFormCaption="Edit Invoice Collection"></SettingsText>

        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" 
			CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" 
			CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5"
            EditForm-HorizontalAlign="WindowCenter" 
			EditForm-VerticalAlign="WindowCenter" EditForm-Width="900px"
        >


<EditForm Width="900px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter"></EditForm>

<CustomizationWindow Height="250px" HorizontalAlign="LeftSides" VerticalAlign="TopSides" HorizontalOffset="5" VerticalOffset="5"></CustomizationWindow>
		</SettingsPopup>


			<ClientSideEvents
				CustomButtonClick="function(s, e) {
	if(e.buttonID == 'invoice'){
                    var rowVisibleIndex = e.visibleIndex;
                    var rowKeyValue;
					s.GetRowValues( e.visibleIndex,'WOProg_ID',email);
				//	alert(rowKeyValue);
				//	email(rowKeyValue);
}
}"/>
		<Images>
			<LoadingPanel Url="~/images/loading_panel.gif">
			</LoadingPanel>
		</Images>
		<Styles>
			<Cell Wrap="False">
			</Cell>
			<CommandColumn Wrap="False">
			</CommandColumn>
			<CommandColumnItem>
				<Paddings PaddingLeft="5px" />
<Paddings PaddingLeft="5px"></Paddings>
			</CommandColumnItem>
		</Styles>
		<Templates>
			<EditForm>
				 <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors"
                                runat="server">
								 </dx:ASPxGridViewTemplateReplacement>
                           
                            <div style="margin-top: 10px; margin-bottom: 20px; padding-bottom:20px;">
                                <div style="float: left; margin-left: 5px">
                                    <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel" Width="100px"
                                        ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' />
                                </div>
                                <div style="float: left; margin-left: 10px">
                                    <dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" Text="Save" Width="100px"
                                        CssClass="input" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' />
                                </div>
                            </div>
							
			</EditForm>
			<TitlePanel>
				<table style="width: 100%; white-space:normal;">
					<tr>
						<td style="background-color: #FFCCCC; white-space:normal;" width="100px">
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
						<td align="left" width="100px" nowrap="nowrap">
							In Dispute</td>
						<td nowrap="nowrap" rowspan="2" valign="middle">
							<strong style="padding: 0px">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; With Selected:&nbsp;&nbsp;&nbsp;&nbsp; </strong></td>
						<td style=" margin: 0px; padding: 0px;" width="300px" rowspan="2" 
							valign="middle">
							<table cellpadding="0" style="width: 100%; white-space: nowrap;">
								<tr>
									<td style="margin: 0px; padding: 0px; text-align: left; white-space: nowrap;">
										Confirmed Paid Date:&nbsp;
									</td>
									<td style="margin: 0px; padding: 0px;">
										<dx:ASPxDateEdit ID="dte_multiconfirm" runat="server" 
											ClientInstanceName="dte_multiconfirm" DisplayFormatString="yyyy-MM-dd" 
											EditFormat="Custom" EditFormatString="yyyy-MM-dd" Height="10px" Width="110px">
											<Paddings Padding="0px" />
										</dx:ASPxDateEdit>
									</td>
								</tr>
							</table>
						</td>
						<td rowspan="2" style=" margin: 0px; padding: 0px;" valign="middle">
							<dx:ASPxButton ID="btn_apply_selected" runat="server" Text="Apply" 
								Height="10px" onclick="btn_apply_selected_Click">
							</dx:ASPxButton>
						</td>
						<td rowspan="4" valign="top">
							&nbsp;</td>
						<td width="100%" rowspan="4">
							&nbsp;</td>
					</tr>
					<tr>
						<td style="background-color: #FFFF00" width="50px">
							&nbsp;</td>
						<td align="left" width="100px" nowrap="nowrap">
							Do Not Contact</td>
					</tr>
					<tr>
						<td style="background-color: #99FF99" width="50px">
							&nbsp;</td>
						<td align="left" width="100px" nowrap="nowrap">
							Write Off</td>
						<td nowrap="nowrap">
							&nbsp;</td>
						<td nowrap="nowrap">
							&nbsp;</td>
						<td nowrap="nowrap">
							&nbsp;</td>
					</tr>
					<tr>
						<td style="background-color: #99CCFF" width="50px">
							&nbsp;</td>
						<td align="left" width="100px" nowrap="nowrap">
							Branch Manager</td>
						<td nowrap="nowrap">
							&nbsp;</td>
						<td nowrap="nowrap">
							&nbsp;</td>
						<td nowrap="nowrap">
							&nbsp;</td>
					</tr>
				</table>
			</TitlePanel>
		</Templates>
</dx:aspxgridview>
	<asp:sqldatasource ID="sql_cust_status" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="SELECT [ID], [status_name] FROM [customer_collection_status]">
	</asp:sqldatasource>
	<asp:sqldatasource ID="sql_inv_status" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		
		SelectCommand="SELECT invoice_collection_status_id, invoice_collection_status_name FROM invoice_collection_status">
	</asp:sqldatasource>
</asp:content>


