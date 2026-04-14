<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/nonFrame.master" Inherits="sections_member_picklist_modules_pop_po_barcodes" Codebehind="pop_po_barcodes.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content runat="server" ContentPlaceHolderID="cphMasterBody">
	<asp:ScriptManager ID="sm" runat="server">
	</asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
					<dx:ASPxCallbackPanel ID="cbp_bc_qty" runat="server" 
						ClientInstanceName="cbp_bc_qty" OnCallback="cbp_bc_qty_Callback" Width="100%">
						<PanelCollection>
							<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
								<dx:ASPxGridView ID="gv_pop_bcqty" runat="server" AutoGenerateColumns="False" 
									ClientInstanceName="gv_pop_bcqty" KeyFieldName="id" 
									Width="100%" Font-Names="Arial" 
									OnHtmlRowPrepared="gv_pop_bcqty_HtmlRowPrepared" OnHtmlDataCellPrepared="gv_pop_bcqty_HtmlDataCellPrepared">
									<Columns>
										<dx:GridViewDataTextColumn Caption="Part #" FieldName="master_id" 
											ShowInCustomizationForm="True" VisibleIndex="1" Width="60px">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
											ShowInCustomizationForm="True" VisibleIndex="2" Width="100%">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Qty To Print" FieldName="qty" 
											ShowInCustomizationForm="True" VisibleIndex="3" Width="80px">
											<DataItemTemplate>
												<dx:ASPxTextBox ID="txtbcqty" runat="server" ClientInstanceName="txtbcqty" 
													Text='<%# Bind("qty") %>' Width="60px">
												</dx:ASPxTextBox>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
											ShowInCustomizationForm="True" VisibleIndex="0" Visible="False">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="WO BC" FieldName="woprog_id" ShowInCustomizationForm="True" VisibleIndex="4" Width="50px">
											<DataItemTemplate>
												<dx:ASPxButton ID="bt_printwo" runat="server" onclick="bt_printwo_Click" Width="25px">
													<Image Url="~/images/icon/icon[print_barcode].GIF">
													</Image>
												</dx:ASPxButton>
											</DataItemTemplate>
										</dx:GridViewDataTextColumn>
									</Columns>
									<SettingsPager Visible="True" PageSize="50">
									</SettingsPager>
									<Settings ShowFooter="True" />
									<Styles>
										<Header Font-Bold="True">
										</Header>
									</Styles>
								</dx:ASPxGridView>
								<dx:ASPxButton ID="btn_printqtybcs" runat="server" OnClick="btn_printqtybcs_Click2" Text="Print">
									<ClientSideEvents Click="function(s, e) {

if (!confirm('Are you sure about printing these Qtys of barcodes?'))
{
e.processOnServer = false;
}
	
}" />
								</dx:ASPxButton>
								<br />
								<dx:ASPxHiddenField ID="hid_pop_bcqty_partslist" runat="server" 
									ClientInstanceName="hid_pop_bcqty_partslist">
								</dx:ASPxHiddenField>
							</dx:PanelContent>
						</PanelCollection>
						<ClientSideEvents EndCallback="function(){alert('Barcodes sent to printer');}" />
					</dx:ASPxCallbackPanel>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>