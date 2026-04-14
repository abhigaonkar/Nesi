<%@ 
Page Language="C#" 
MasterPageFile	= '../../../IntraDefault.master'
AutoEventWireup="true" 
Inherits="sections_reports_ap_report_index" 
Title			= 'AP REPORT' 

 EnableTheming="True" Codebehind="index.aspx.cs" %>

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
							EditFormatString="yyyy-MM-dd" AnimationType="None" ondatechanged="dteScope_DateChanged" Theme="NETheme01">
						</dx:ASPxDateEdit>
					</td>
					<td nowrap="nowrap" width="100%">
						<dx:ASPxButton ID="btnUpdate" runat="server" Text="Apply" 
							onclick="btnUpdate_Click" Theme="NETheme01">
						</dx:ASPxButton>
					</td>
				</tr>
	</table>
	<br />
    <dx:ASPxGridView ID="Aspxgridview1" runat="server" AutoGenerateColumns="False" 
        KeyFieldName="cust"
        Width="100%" 
		ClientInstanceName="Aspxgridview1" 
		oncustomcallback="Aspxgridview1_CustomCallback" 
		oncustomjsproperties="Aspxgridview1_CustomJSProperties" 
		onhtmleditformcreated="Aspxgridview1_HtmlEditFormCreated" 
		onrowupdating="Aspxgridview1_RowUpdating" onhtmlrowprepared="Aspxgridview1_HtmlRowPrepared" 
		 Theme="NETheme01">
        <SettingsText PopupEditFormCaption="AP Details" />
		<SettingsEditing Mode="PopupEditForm" EditFormColumnCount="4"></SettingsEditing>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" 
			ShowGroupPanel="True" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded" 
			ShowPreview="True" ShowFilterBar="Visible" ShowGroupedColumns="True"></Settings>
		<SettingsText PopupEditFormCaption="Invoices"></SettingsText>

        <Styles>
        	<CommandColumn Spacing="20px" VerticalAlign="Middle" Wrap="False"></CommandColumn>
			<CommandColumnItem Spacing="20px" VerticalAlign="Middle"></CommandColumnItem>
        </Styles>
        

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
			
		</TotalSummary>
		<GroupSummary>
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="sixty" ShowInColumn="30+" 
				ShowInGroupFooterColumn="30+" SummaryType="Sum" ValueDisplayFormat="C2" />
		</GroupSummary>
        <Columns>
            <dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" ShowClearFilterButton="true"   
				Width="25px" Visible="False">
				
				
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="No" FieldName="vend" 
				VisibleIndex="1" ReadOnly="True" Width="40px">
            	<PropertiesTextEdit Width="100px">
				</PropertiesTextEdit>
				<EditFormSettings VisibleIndex="0" />
				<EditCellStyle>
					<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
				</EditCellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Vendor Name" FieldName="name" 
				VisibleIndex="2" ReadOnly="True" Width="75px">
            	<PropertiesTextEdit Width="200px">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<asp:HyperLink ID="hl_customer" runat="server" Font-Bold="True" 
					NavigateUrl="<%# string.Format(&quot;javascript:boing('/#/opens/11/vendors/{0}','Vendor',1035,800);&quot;,Eval(&quot;vendor_id&quot;)) %>"
						Text='<%# HttpUtility.UrlDecode(Eval("name").ToString()) %>' Target="_blank" Font-Size="11px" Width="100%"></asp:HyperLink>
			</DataItemTemplate>
				<EditFormSettings VisibleIndex="1" />
				<EditCellStyle>
					<Border BorderStyle="None" />
<Border BorderStyle="None"></Border>
				</EditCellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Credit Line" FieldName="credit_line" 
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
        	<dx:GridViewDataTextColumn Caption="Overage" FieldName="overage" 
				VisibleIndex="11" Width="40px">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
				<EditFormSettings Visible="False" />

<EditFormSettings Visible="False"></EditFormSettings>

				<CellStyle BackColor="#FFFFCC">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Open POs" FieldName="pos" VisibleIndex="4" 
				ReadOnly="True" Width="40px">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
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
        </Columns>
        <SettingsEditing Mode="PopupEditForm" EditFormColumnCount="1" />
        <SettingsPopup EditForm-HorizontalAlign="Center" EditForm-VerticalAlign="Middle" 
			EditForm-Width="800px" >
<EditForm Width="800px" HorizontalAlign="Center" VerticalAlign="Middle"></EditForm>
		</SettingsPopup>
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
							<dx:ASPxLabel ID="lblCustomerName" runat="server" Text='<%# Eval("name") %>' 
								Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
						<td width="0%">
							&nbsp;</td>
					</tr>
					<tr>
						<td width="100%">
							<dx:ASPxLabel ID="lbltypicalPay" runat="server" Text="ASPxLabel" 
								Font-Names="Arial" Font-Size="12pt" Width="1100px" Theme="NETheme01">
							</dx:ASPxLabel>
						</td>
						<td width="0%">
							&nbsp;</td>
					</tr>
					<tr>
						<td colspan="2" nowrap="nowrap" bgcolor="#3399FF">
							</td>
					</tr>
					<tr>
						<td width="100%">
							&nbsp;</td>
						<td width="0%">
							&nbsp;</td>
					</tr>
					<tr>
					
						<td nowrap="nowrap" style="height: 19px" width="100%">
							
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
                                        ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' Theme="NETheme01" />
                                </div>
                                <div style="float: left; margin-left: 10px">
                                    <dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" Text="Save" Width="100px" Theme="NETheme01"
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

