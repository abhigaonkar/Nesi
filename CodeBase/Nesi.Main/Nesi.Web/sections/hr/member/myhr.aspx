<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="member_myhr" Title="My HR Page" Codebehind="myhr.aspx.cs" %>


<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>





<%@ Register src="modules/fvr_listing.ascx" tagname="fvr_listing" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	<table style="padding: 20px; border-collapse: collapse;">
		<tr>
			<td align="left" style="padding: 20px" colspan="3">
				<div id="div_stuff" runat="server" style="font-family: Calibri; font-size: small;">
                </div>
			</td>

		</tr>
		<tr>
			<td align="center" style="padding: 20px" bgcolor="#E8E8EC">
				<dx:ASPxButton ID="ASPxButton2" runat="server" 
					Font-Names="Arial" Font-Size="14pt" Height="60px" 
					onclick="ASPxButton2_Click" Text="My Job Description" Width="100%" Theme="NETheme01">
				</dx:ASPxButton>
			</td>

			<td align="center" style="padding: 20px" bgcolor="#E8E8EC">
				<dx:ASPxButton ID="ASPxButton3" runat="server" 
					Font-Names="Arial" Font-Size="14pt" Height="60px" 
					onclick="ASPxButton3_Click" Text="My Current Agreement" Width="100%" Theme="NETheme01">
				</dx:ASPxButton>
			</td>
			<td align="center" style="padding: 20px" bgcolor="#E8E8EC">
				<dx:ASPxButton ID="ASPxButton4" runat="server" 
					Font-Names="Arial" Font-Size="14pt" Height="60px"  onclick="ASPxButton4_Click" 
					Text="My Upcoming Review" Width="100%" Theme="NETheme01">
				</dx:ASPxButton>
			</td>
		</tr>
		<tr>
			<td align="center" style="padding: 20px" bgcolor="#E8E8EC">
				<dx:ASPxButton ID="ASPxButton5" runat="server" 
					Font-Names="Arial" Font-Size="14pt" Height="60px" 
					onclick="ASPxButton5_Click" Text="FVR's I've Reviewed" Width="100%" Theme="NETheme01">
				</dx:ASPxButton>
			</td>
			<td align="center" style="padding: 20px" bgcolor="#E8E8EC">
				<dx:ASPxButton ID="ASPxButton6" runat="server" 
					Font-Names="Arial" Font-Size="14pt" Height="60px"  onclick="ASPxButton6_Click" 
					Text="Previous Agreements" Width="100%" Theme="NETheme01">
				</dx:ASPxButton>
			</td>
			<td align="center" style="padding: 20px" bgcolor="#E8E8EC">
				<dx:ASPxButton ID="ASPxButton7" runat="server" 
					Font-Names="Arial" Font-Size="14pt" Height="60px"  onclick="ASPxButton7_Click" 
					Text="Previous Reviews" Width="100%" Theme="NETheme01">
				</dx:ASPxButton>
			</td>
		</tr>
        <tr>
			<td bgcolor="#E8E8EC" style="padding: 20px" align="center">
				<dx:ASPxButton ID="ASPxButton9" runat="server" 
					Font-Names="Arial" Font-Size="14pt" Height="60px" 
					onclick="btn_print_er" Text="Print Employment Record" Width="100%" Theme="NETheme01" Wrap="True">
				</dx:ASPxButton>
			</td>
			<td align="center" style="padding: 20px" bgcolor="#E8E8EC">
				<dx:ASPxButton ID="ASPxButton10" runat="server" 
					Font-Names="Arial" Font-Size="14pt" Height="60px" 
					onclick="btn_print_er_detailed" Text="Print Detailed Employment Record" Width="100%" Theme="NETheme01" Wrap="True">
				</dx:ASPxButton>
			</td><td bgcolor="#E8E8EC" style="padding: 20px">
				<dx:ASPxButton ID="ASPxButton8" runat="server" 
					Font-Names="Arial" Font-Size="14pt" Height="60px"  onclick="ASPxButton8_Click" 
					Text="New Offer" Width="100%" Visible="False" Theme="NETheme01">
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
	<dx:ASPxPopupControl ID="pop" runat="server" ClientInstanceName="pop" 
		CloseAction="CloseButton" HeaderText="" Height="700px" Modal="True" 
		PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
		Width="800px" Theme="NETheme01">
		<ModalBackgroundStyle Opacity="0">
		</ModalBackgroundStyle>
		<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
		Width="100%" KeyFieldName="id" Visible="False" Theme="NETheme01">
		<Columns>
			<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
				ShowInCustomizationForm="True" Visible="False" VisibleIndex="3">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Date" FieldName="date" 
				ShowInCustomizationForm="True" VisibleIndex="0">
				<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
				ShowInCustomizationForm="True" VisibleIndex="1">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
						oninit="ASPxHyperLink1_Init" Text='<%# Eval("description") %>' Cursor="pointer" />
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="type" FieldName="type" 
				ShowInCustomizationForm="True" Visible="False" VisibleIndex="2">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager Visible="False">
		</SettingsPager>
	</dx:ASPxGridView>
			<uc1:fvr_listing ID="fvr_listing1" runat="server" Visible="False" />
			</dx:PopupControlContentControl>
</ContentCollection>
	</dx:ASPxPopupControl>
	<br />
</asp:Content>


