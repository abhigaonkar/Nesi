<%@ 
Page Language="C#" 
MasterPageFile	= '../../../IntraDefault.master'
AutoEventWireup="true" 
Inherits="sections_reports_ar_report_index" 
Title			= 'AR REPORT' 
EnableTheming="True"   
 Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				<div id='divSide' runat='server'>
				</div>
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>

<script type="text/javascript">

 function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 30) + 'px';
	
 }

	function email(id) {
	boing("/sections/reports/invoice_preview/index.aspx?id="+ id, 'invoice_preview', 850, 850);
	}

		function bind_tooltips()
			{
			$(".ttip").each(function()
				{
				$(this).tip();
				});
			}
		$(document).ready(function()
			{
			bind_tooltips();
		});


</script>

	<lc:LayoutControl runat="server" id="layout" />
	<br />
			<table style="width:100%;">
				<tr>
					<td width="0%">
			<asp:DropDownList ID="ddlCompany" runat="server" datatextfield="ddl_name" 
				datavaluefield="id" 
				OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
    </asp:DropDownList>
					</td>
					<td width="0%">
						&nbsp;</td>
					<td nowrap="nowrap" width="0">
						<dx:ASPxDateEdit ID="dteScope" runat="server" ClientInstanceName="dteScope" 
							DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
							EditFormatString="yyyy-MM-dd" AnimationType="None" ondatechanged="dteScope_DateChanged">
						</dx:ASPxDateEdit>
					</td>
			<!--		<td nowrap="nowrap" width="100%">
						<dx:ASPxButton ID="btnUpdate" runat="server" Text="Apply" 
							onclick="btnUpdate_Click">
						</dx:ASPxButton>
					</td> -->
				</tr>
	</table>
	<br />
    <dx:ASPxGridView ID="Aspxgridview1" runat="server" AutoGenerateColumns="False" Font-Names="Arial"
        Font-Size="9pt" KeyFieldName="cust"
        Width="100%" 
		ClientInstanceName="Aspxgridview1" 
		oncustomcallback="Aspxgridview1_CustomCallback" 
		oncustomjsproperties="Aspxgridview1_CustomJSProperties" 
		onhtmleditformcreated="Aspxgridview1_HtmlEditFormCreated" 
		onrowupdating="Aspxgridview1_RowUpdating" onhtmlrowprepared="Aspxgridview1_HtmlRowPrepared" 
		Theme="NETheme01">
        <SettingsText PopupEditFormCaption="Invoices" />

<SettingsEditing Mode="PopupEditForm" EditFormColumnCount="4"></SettingsEditing>

<SettingsPopup EditForm-HorizontalAlign="Center" EditForm-VerticalAlign="WindowCenter" EditForm-Width="1000px"/>

<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" 
			ShowGroupPanel="True" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded" 
			ShowPreview="True" ShowFilterBar="Visible" ShowGroupedColumns="True"></Settings>

<SettingsText PopupEditFormCaption="Invoices"></SettingsText>

        <Styles>
            <Header BackColor="#0066FF" ForeColor="White" Font-Bold="True" 
				Font-Names="Arial" Font-Size="9pt" Wrap="True">
            </Header>
        	<CommandColumn Spacing="20px" VerticalAlign="Middle" Wrap="False">
			</CommandColumn>
			<CommandColumnItem Spacing="20px" VerticalAlign="Middle">
			</CommandColumnItem>
        </Styles>
        <StylesPopup EditForm-Header-BackColor="#0000CC" EditForm-Header-ForeColor="White" EditForm-Header-Font-Bold="true" EditForm-Header-Font-Names="Arial" EditForm-Content-Paddings-Padding="10px" EditForm-Content-Wrap="False"
        EditForm-MainArea-BackColor="#E1E1E0" EditForm-MainArea-Wrap="true" EditForm-MainArea-Paddings-Padding="10px"/>
        <ClientSideEvents EndCallback="function(s, e) {
	bind_tooltips(); please_wait('stop');
}" />
        <TotalSummary>
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="sixty" ShowInColumn="30+" 
				ShowInGroupFooterColumn="30+" SummaryType="Sum" ValueDisplayFormat="C2" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="thirty" 
				ShowInColumn="Current" ShowInGroupFooterColumn="Current" SummaryType="Sum" 
				ValueDisplayFormat="C2" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="ninety" ShowInColumn="60+" 
				ShowInGroupFooterColumn="60+" SummaryType="Sum" ValueDisplayFormat="C2" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="onetwenty" ShowInColumn="90+" 
				ShowInGroupFooterColumn="90+" SummaryType="Sum" ValueDisplayFormat="C2" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="onetwentyplus" 
				ShowInColumn="120+" ShowInGroupFooterColumn="120+" SummaryType="Sum" 
				ValueDisplayFormat="C2" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="overage" 
				ShowInColumn="Overage" ShowInGroupFooterColumn="Overage" SummaryType="Sum" 
				ValueDisplayFormat="C2" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="ebalance" 
				ShowInColumn="Balance" ShowInGroupFooterColumn="Balance" SummaryType="Sum" 
				ValueDisplayFormat="C2" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="wos" ShowInColumn="Open" 
				ShowInGroupFooterColumn="Open" SummaryType="Sum" ValueDisplayFormat="C2" />
			<dx:ASPxSummaryItem DisplayFormat="Avg: {0:0}" FieldName="avg5" 
				ShowInColumn="Avg Lag(d)" SummaryType="Average" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="next14" 
				ShowInColumn="Confirmed" ShowInGroupFooterColumn="Confirmed" SummaryType="Sum" 
				ValueDisplayFormat="C2" />
		</TotalSummary>
		<GroupSummary>
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="sixty" ShowInColumn="30+" 
				ShowInGroupFooterColumn="30+" SummaryType="Sum" ValueDisplayFormat="C2" />
		</GroupSummary>
        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[zoom].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
        <Columns>
            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
				ShowSelectCheckbox="True" VisibleIndex="0" Width="25px" ShowEditButton="true" ShowClearFilterButton="true" >
				
				
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="No" FieldName="cust" 
				VisibleIndex="1" ReadOnly="True" Width="40px">
            	<PropertiesTextEdit Width="100px">
				</PropertiesTextEdit>
				<EditFormSettings VisibleIndex="0" />
				<EditCellStyle>
					<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
				</EditCellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Customer Name" FieldName="name" 
				VisibleIndex="2" ReadOnly="True" Width="75px">
            	<PropertiesTextEdit Width="200px">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<asp:HyperLink ID="hl_customer" runat="server" Font-Bold="True" 
					NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}','Customer',1035,800);&quot;,Eval(&quot;customer_id&quot;)) %>"
						Text='<%# HttpUtility.UrlDecode(Eval("name").ToString()) %>' Target="_blank" Font-Size="11px" Width="100%"></asp:HyperLink>
			</DataItemTemplate>
				<EditFormSettings VisibleIndex="1" />
				<EditCellStyle>
					<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
				</EditCellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Credit Limit" FieldName="credit_limit" 
				VisibleIndex="3" UnboundType="Decimal" Width="30px">
                <PropertiesTextEdit DisplayFormatString="C2" Width="100px">
                </PropertiesTextEdit>
            	<EditFormSettings VisibleIndex="2" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Current" FieldName="thirty" 
				VisibleIndex="5" UnboundType="Decimal" Width="40px">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            	<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="30+" FieldName="sixty" VisibleIndex="6" 
				UnboundType="Decimal" Width="40px">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            	<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="60+" FieldName="ninety" VisibleIndex="7" 
				UnboundType="Decimal" Width="40px">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            	<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="90+" FieldName="onetwenty" VisibleIndex="8" 
				UnboundType="Decimal" Width="40px">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            	<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="120+" FieldName="onetwentyplus" 
				VisibleIndex="9" UnboundType="Decimal" Width="40px">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            	<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>

            	<CellStyle BackColor="#FFDFDF">
				</CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Balance" FieldName="ebalance" 
				VisibleIndex="10" UnboundType="Decimal" Width="40px">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            	<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>

            	<CellStyle BackColor="#CCFFCC">
				</CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn Caption="On Hold" FieldName="OnHold"
                VisibleIndex="12" UnboundType="String" Width="25px">
                <PropertiesCheckEdit ValueChecked="T" ValueType="System.String" 
					ValueUnchecked="F" AllowGrayedByClick="False">
                </PropertiesCheckEdit>
            	<EditFormSettings VisibleIndex="5" />
            </dx:GridViewDataCheckColumn>
        	<dx:GridViewDataTextColumn Caption="Overage" FieldName="overage" 
				VisibleIndex="11" Width="40px">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
				<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>

				<CellStyle BackColor="#FFFFCC">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Open" FieldName="wos" VisibleIndex="4" 
				ReadOnly="True" Width="40px">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
				<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
        	<dx:GridViewDataMemoColumn Caption="customer_notes" FieldName="customer_notes" 
				Visible="False" VisibleIndex="23">
				<PropertiesMemoEdit Height="30px">
				</PropertiesMemoEdit>
				<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataMemoColumn>
			<dx:GridViewDataSpinEditColumn Caption="Credit(d)" FieldName="creditdays" 
				VisibleIndex="13" Width="30px">
				<PropertiesSpinEdit DisplayFormatString="g" Increment="10" MaxValue="180" 
					NumberType="Integer" Width="50px">
				</PropertiesSpinEdit>
				<EditFormSettings VisibleIndex="3" />
			</dx:GridViewDataSpinEditColumn>
			<dx:GridViewDataMemoColumn Caption="bv_cust_notes" FieldName="bv_cust_notes" 
				Visible="False" VisibleIndex="22">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataMemoColumn>
			<dx:GridViewDataTextColumn Caption="customer_salesnotes" 
				FieldName="customer_salesnotes" Visible="False" VisibleIndex="17">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="customer_memo" FieldName="customer_memo" 
				Visible="False" VisibleIndex="15">
				<EditFormSettings Visible="False" />
<EditFormSettings Visible="False"></EditFormSettings>
			</dx:GridViewDataTextColumn>
        	<dx:GridViewDataComboBoxColumn Caption="Status" FieldName="status_" 
				VisibleIndex="14" Width="50px">
				<PropertiesComboBox DataSourceID="sql_status" TextField="status_name" 
					ValueField="ID" ValueType="System.Int32">
					<Items>
						<dx:ListEditItem Text=" " />
						<dx:ListEditItem Text="Cheque is En Route" />
						<dx:ListEditItem Text="Action Required - See Notes" />
						<dx:ListEditItem Text="Collections and Bankruptcy" />
						<dx:ListEditItem Text="Intercompany" />
						<dx:ListEditItem Text="History of Slow Payment" />
						<dx:ListEditItem Text="Payments/Credit on Account" />
						<dx:ListEditItem Text="Customer on Hold until Payement" />
						<dx:ListEditItem Text="10% Holdback" />
						<dx:ListEditItem Text="BM Said Don't Chase" />
					</Items>
				</PropertiesComboBox>
				<EditFormSettings VisibleIndex="4" />
			</dx:GridViewDataComboBoxColumn>
        	<dx:GridViewDataTextColumn Caption="Days to Pay" FieldName="nextexpectedpay" 
				VisibleIndex="16" Width="40px">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
        	<dx:GridViewDataTextColumn Caption="Avg Lag(d)" FieldName="avg5" 
				VisibleIndex="18" Width="25px">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
        	<dx:GridViewDataTextColumn Caption="Confirmed" FieldName="next14" 
				VisibleIndex="19" Width="40px">
				<EditFormSettings Visible="False" />
			</dx:GridViewDataTextColumn>
        	<dx:GridViewDataMemoColumn Caption="whyhold" FieldName="cust_whyhold" 
				Visible="False" VisibleIndex="21">
				<PropertiesMemoEdit Height="30px" Width="500px">
				</PropertiesMemoEdit>
				<EditFormSettings Caption=" Why is this customer on hold?" Visible="True" 
					VisibleIndex="6" />
				<EditFormCaptionStyle Wrap="True">
				</EditFormCaptionStyle>
			</dx:GridViewDataMemoColumn>
			<dx:GridViewDataTextColumn Caption="On Hold By" FieldName="cust_whohold" 
				ReadOnly="True" Visible="False" VisibleIndex="20" Width="20px">
				<PropertiesTextEdit Width="150px">
				</PropertiesTextEdit>
				<EditFormSettings Caption="Member" Visible="True" VisibleIndex="7" />
			</dx:GridViewDataTextColumn>
        	<dx:GridViewDataTextColumn Caption="Note Date" FieldName="note_date" VisibleIndex="80">
			</dx:GridViewDataTextColumn>
        </Columns>
        <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1" />
        <SettingsPopup EditForm-HorizontalAlign="Center" EditForm-VerticalAlign="Middle" 
			EditForm-Width="800px" />
        <Settings ShowFilterRow="True" ShowGroupPanel="True" ShowPreview="True" 
			ShowFilterBar="Visible" ShowFilterRowMenu="True" 
			ShowHeaderFilterButton="True" ShowFooter="True" ShowGroupedColumns="True" 
			ShowGroupFooter="VisibleIfExpanded" />
        <SettingsBehavior AutoFilterRowInputDelay="5000" EnableRowHotTrack="True" 
			ColumnResizeMode="Control" />

<SettingsBehavior ProcessFocusedRowChangedOnServer="True" 
			AutoFilterRowInputDelay="5000" EnableRowHotTrack="True"></SettingsBehavior>

        <SettingsPager NumericButtonCount="15" PageSize="15">
        </SettingsPager>
    	<Templates>
			<EditForm>
				<table rules=none style="width:100%; white-space:nowrap;">
					<tr>
						<td>
							<dx:ASPxLabel ID="lblCustomerName" runat="server" Text='<%# Eval("name") %>'>
							</dx:ASPxLabel>
						</td>
						<td width="0%">
							&nbsp;</td>
					</tr>
					<tr>
						<td width="100%">
							<dx:ASPxLabel ID="lbltypicalPay" runat="server" Text="ASPxLabel" 
								Font-Names="Arial" Font-Size="12pt" Width="1100px">
							</dx:ASPxLabel>
						</td>
						<td width="0%">
							&nbsp;</td>
					</tr>
					<tr>
						<td colspan="2" nowrap="nowrap" bgcolor="#3399FF">
							<iframe id = "gv_invoice_frame" runat="server" width="1200px" marginheight="0" frameborder="0" allowtransparency="true"></iframe></td>
					</tr>
					<tr>
						<td width="100%">
							&nbsp;</td>
						<td width="0%">
							&nbsp;</td>
					</tr>
					<tr>
					
						<td nowrap="nowrap" style="height: 19px" width="100%">
							<iframe ID="frame_notes" runat="server" frameborder="0" marginheight="0" 
								name="I1" width="1200px" allowtransparency="true"></iframe>
						</td>
						<td nowrap="nowrap" style="height: 19px" width="0%">
							&nbsp;</td>
					</tr>
					<tr>
						<td nowrap="nowrap" width="100%" style="white-space:nowrap">
							&nbsp;</td>
						<td nowrap="nowrap" style="white-space:nowrap" width="0%">
							&nbsp;</td>
					</tr>
				</table>
				 <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors"
                                runat="server">
								 </dx:ASPxGridViewTemplateReplacement>
                           
                            <div style="margin-top: 10px; margin-bottom: 20px; padding-bottom:20px;">
                                <div style="float: left; margin-left: 5px">
                                    <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel" Width="100px"
                                        ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' />
                                </div>
                                <div style="float: left; margin-left: 10px">
                                    <dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" Text="Save" Width="100px" visible="false" Enabled="false"
                                        CssClass="input" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' />
                                </div>
                            </div>
							
			</EditForm>
		</Templates>
    </dx:ASPxGridView>
            <dx:ASPxGridViewExporter id="exporter" runat="server" GridViewID="Aspxgridview1"
                Landscape="True" PaperKind="Tabloid">
            </dx:ASPxGridViewExporter>
            <asp:SqlDataSource ID="sql_status" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="SELECT [ID], [status_name] FROM [customer_collection_status]">
	</asp:SqlDataSource>
            <br />
	&nbsp;&nbsp;
    
</asp:Content>
<asp:Content ID="Content4" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style2
		{
			height: 5px;
		}
	</style>
</asp:Content>

