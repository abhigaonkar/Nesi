<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true"  Inherits="sections_purchaseorder_posearch" Title="PO Search Page" EnableTheming="True" Codebehind="posearch.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
    <div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
    <div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" runat="Server">
    <asp:ScriptManager ID="sm" runat="server">
	</asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
	<asp:Panel ID="panSearch" runat="server" DefaultButton="btnSearch">
		<table border="0" cellpadding="2" cellspacing="0" style="width: 610px">
			<tr>
				<td>
					Search for a Purchase Order by PO No/PoProgID or Vendor Name<br />
					<asp:Label ID="InfoLabel" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Red"></asp:Label>
				</td>
			</tr>
			<tr>
				<td style="height: 29px">
					<asp:DropDownList ID="ddlCompany" runat="server" />
					<asp:TextBox ID="txtSearch" runat="server" CausesValidation="True"></asp:TextBox>&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />&nbsp;
				</td>
			</tr>
		</table>
	</asp:Panel>
	<p>
	</p>
	<div id="divResults" runat="server">
		<dx:ASPxGridView ID="GridViewPO" runat="server" AutoGenerateColumns="False" Visible="False" Width="100%" Theme="NETheme01">
			<SettingsPager Mode="ShowAllRecords">
			</SettingsPager>
			<Columns>
				<dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0" Caption=" ">
				</dx:GridViewCommandColumn>
				<dx:GridViewDataHyperLinkColumn FieldName="poprog_id" Caption="View" 
					VisibleIndex="1" Width="30px">
					<PropertiesHyperLinkEdit ImageUrl="~/images/icon/icon[details].gif" NavigateUrlFormatString="po_prog_add.aspx?poprogid={0}&amp;action=show">
					</PropertiesHyperLinkEdit>
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataHyperLinkColumn>
				<dx:GridViewDataTextColumn FieldName="poprog_bvpo" Caption="PO BVPO#" 
					VisibleIndex="3" Width="60px">
					<PropertiesTextEdit DisplayFormatString="{0}">
					</PropertiesTextEdit>
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="poprog_order_description" 
					Caption="PO Description" VisibleIndex="4" Width="100%">
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="Vendor_Name" Caption="Vendor Name" 
					VisibleIndex="5" Width="100px">
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Status" FieldName="PO_Status" 
					VisibleIndex="6" Width="80px">
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataDateColumn Caption="PO Date" FieldName="poprog_cutdate" 
					VisibleIndex="7" Width="125px">
					<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
					</PropertiesDateEdit>
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataDateColumn>
				<dx:GridViewDataTextColumn Caption="PO Amount" FieldName="poprog_total_cost" 
					VisibleIndex="8" Width="100px">
					<PropertiesTextEdit DisplayFormatString="C2">
					</PropertiesTextEdit>
					<CellStyle Wrap="False">
					</CellStyle>
				</dx:GridViewDataTextColumn>
			    <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="ddl_name" VisibleIndex="2" Width="90px">
                </dx:GridViewDataTextColumn>
			</Columns>
			<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
				ShowFooter="True" ShowHeaderFilterButton="True" />
		</dx:ASPxGridView>
		<br />
		<asp:Label ID="ResultLabel" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Large" Text="Label" Visible="False"></asp:Label>
	</div>
		
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
