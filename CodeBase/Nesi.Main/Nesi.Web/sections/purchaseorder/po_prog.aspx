<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_purchaseorder_po_prog" Title="Purchase Order Progress" EnableTheming="True" Codebehind="po_prog.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register src="../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>



<ASP:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</ASP:CONTENT>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <dx:ASPxComboBox ID="ddlcompany" runat="server" ValueType="System.Int32" AutoPostBack="True" OnSelectedIndexChanged="ddlcompany_SelectedIndexChanged" TextField="name" Theme="NETheme01" ValueField="id"></dx:ASPxComboBox>
    
<div id="divCoSummary" runat="server">

</div>
    <asp:PlaceHolder ID="plhGraph" runat="server"></asp:PlaceHolder>
    
	<br />
	<br />
	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
			<lc:LayoutControl ID="layout" runat="server" GridviewID="gv_posummary" />
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:UpdateProgress ID="UpdateProgress1" runat="server" 
				AssociatedUpdatePanelID="UpdatePanel1">
			</asp:UpdateProgress>
		    <dx:ASPxComboBox ID="cb_branches" runat="server" 
		                     ClientInstanceName="cb_branches" DataSourceID="ods_companies" 
		                     OnDataBound="cb_branches_DataBound" TextField="ddl_name" ValueField="id" Theme="NETheme01">
		        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	cb_company.PerformCallback(s.GetValue());
}" />
		    </dx:ASPxComboBox>
		    <asp:ObjectDataSource ID="ods_companies" runat="server" 
		                          SelectMethod="LoadMy_business_units" TypeName="nesi.core.NeBusinessUnit">
		    </asp:ObjectDataSource>
			<dx:ASPxCallback ID="cb_company" runat="server" ClientInstanceName="cb_company" 
				OnCallback="cb_company_Callback">
				<ClientSideEvents EndCallback="function(s, e) {
	gv_posummary.Refresh();
}" />
			</dx:ASPxCallback>
<dx1:ASPxCallbackPanel ID="cb" runat="server" ClientInstanceName="cb" 
				oncallback="cb_Callback" Width="200px">
			</dx1:ASPxCallbackPanel>
			
			<dx:ASPxGridView ID="gv_posummary" runat="server" AutoGenerateColumns="False" 
				ClientInstanceName="gv_posummary" DataSourceID="SqlDataSource1" 
				Font-Names="Arial" Font-Size="8pt" 
				KeyFieldName="poprog_id" onclientlayout="gv_posummary_ClientLayout" 
				oncustombuttoncallback="gv_posummary_CustomButtonCallback" 
				oncustomcallback="gv_posummary_CustomCallback" 
				oncustomjsproperties="gv_posummary_CustomJSProperties" Width="100%" Theme="NETheme01">
				<Columns>
					<dx:GridViewCommandColumn ButtonType="Image" Caption=" " Visible="False" ShowClearFilterButton="true" 
						VisibleIndex="1" Width="1px">
						
						<CustomButtons>
							<dx:GridViewCommandColumnCustomButton ID="printbc">
								<Image Url="~/images/icon/icon[print_barcode].GIF">
								</Image>
							</dx:GridViewCommandColumnCustomButton>
						</CustomButtons>
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn FieldName="Branch" Caption="Business Unit" Visible="False" VisibleIndex="2" 
						Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="poprog_id" SortIndex="0" SortOrder="Descending" Caption="PO"
						VisibleIndex="3" Width="90px">
						<Settings AutoFilterCondition="Contains" />
						<DataItemTemplate>
							<asp:HyperLink ID="hl_wo" runat="server" Theme="NETheme01"
								NavigateUrl='<%# "/redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}", Eval("poprog_id"))) %>' 
								Target="_blank" Text='<%# Eval("PO") %>'></asp:HyperLink>
						</DataItemTemplate>
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Vendor" VisibleIndex="4" Width="100px">
						<EditCellStyle Wrap="False">
						</EditCellStyle>
						<DataItemTemplate>
							<asp:HyperLink ID="hl_vendor" runat="server"  Theme="NETheme01" 
								NavigateUrl='<%# "/redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/sections/vendor/frame.aspx?vendor_id={0}&first_tab=true", Eval("vendor_id"))) %>' 
								Target="_blank" Text='<%# HttpUtility.UrlDecode(Eval("Vendor").ToString()) %>' 
								Width="100%"></asp:HyperLink>
						</DataItemTemplate>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn FieldName="Cut" VisibleIndex="5" Width="100px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn FieldName="Issued" VisibleIndex="6" Width="100px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn FieldName="Required" VisibleIndex="7" Width="100px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="8" 
						Width="100%">
						<PropertiesTextEdit EncodeHtml="False">
						</PropertiesTextEdit>
						<Settings AutoFilterCondition="Contains" />
						<DataItemTemplate>
							<dx:ASPxLabel ID="text_container" runat="server" Theme="NETheme01" Font-Size="11px" 
								OnDataBound="text_container_Init" 
								Text='<%# Eval("Description") %>' 
								Width="100%" Wrap="False">
							</dx:ASPxLabel>
						</DataItemTemplate>
						<CellStyle HorizontalAlign="Left" Wrap="False">
							<Paddings PaddingRight="0px" />
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="9" Width="50px">
						<Settings HeaderFilterMode="CheckedList" />
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Shipping" VisibleIndex="10" Width="50px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Cost" VisibleIndex="11" Width="75px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Rec Cost" VisibleIndex="12" Width="75px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Purchaser" VisibleIndex="13" Width="50px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					
					<dx:GridViewDataTextColumn FieldName="poprog_id" Visible="False" 
						VisibleIndex="18" Width="5px" ShowInCustomizationForm="False">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Vendor ID" FieldName="vendor_id" Visible="False" 
						VisibleIndex="17" ShowInCustomizationForm="False">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataComboBoxColumn Caption="AP Status" FieldName="poprog_apstatus" 
						VisibleIndex="15" Width="80px">
						<PropertiesComboBox EnableFocusedStyle="False">
						</PropertiesComboBox>
						<Settings HeaderFilterMode="CheckedList" />
						<DataItemTemplate>
							<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" 
								DataSourceID="SqlDataSource2" oninit="ASPxComboBox2_Init" 
								TextField="apstatus_name" Value='<%# Eval("poprog_apstatus") %>' 
								ValueField="apstatus_id" ValueType="System.Int32" Width="100%" Theme="NETheme01">
							</dx:ASPxComboBox>
						</DataItemTemplate>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataTextColumn Caption="AP Notes" FieldName="apnotes" 
						VisibleIndex="16" Width="200px">
						<Settings AllowSort="False" />
						<DataItemTemplate>
							<dx:ASPxMemo ID="mem" runat="server" Height="15px" oninit="mem_Init" 
								Text='<%# Eval("apnotes") %>' Width="100%" Theme="NETheme01">
								<ClientSideEvents Init="function(s, e) {
	 var text = s.GetText().replace(/\r/g, '');
	if (text.length &gt;1 )
		{
			s.SetHeight(40);
			s.GetMainElement().style.backgroundColor = &quot;yellow&quot;
			s.GetInputElement().style.backgroundColor = &quot;yellow&quot;
		}

}" />
							</dx:ASPxMemo>
						</DataItemTemplate>
						<CellStyle>
							<Paddings Padding="0px" />
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="# of Lines" FieldName="n_lines" Visible="False" VisibleIndex="0" Width="50px">
					</dx:GridViewDataTextColumn>
				</Columns>
				<SettingsBehavior ColumnResizeMode="Control" />
				<SettingsPager NumericButtonCount="20" PageSize="20">
				</SettingsPager>
				<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
					ShowHeaderFilterButton="True" />
				
			</dx:ASPxGridView>
			<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL report_po_progress(?cid)">
				<SelectParameters>
					<asp:SessionParameter Name="cid" SessionField="po_working_business_unit_id" />
				</SelectParameters>
			</asp:SqlDataSource>
			<dx:ASPxHiddenField ID="hh" runat="server" ClientInstanceName="hh">
			</dx:ASPxHiddenField>
<br />
		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from apstatus"></asp:SqlDataSource>
	<br />

				&nbsp;<br />

 </ASP:CONTENT>

