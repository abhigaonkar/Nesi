<%@ Page Language="C#" AutoEventWireup="true"   Inherits="sections_purchaseorder_POOrderSlip" EnableTheming="True" Codebehind="POOrderSlip.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>






<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	    <asp:scriptmanager id="sm" runat="server"></asp:scriptmanager>
    <dx:ASPxPopupControl id="popupemail" runat="server" ClientInstanceName="popupemail"
            CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="" Theme="NETheme01"><contentcollection>
				<dx:PopupControlContentControl runat="server">
					<dx:ASPxRoundPanel runat="server" HeaderText="Send it!" Width="600px" ID="ASPxRoundPanel1">
						<ContentPaddings PaddingLeft="9px" PaddingTop="6px" PaddingRight="9px" PaddingBottom="6px"></ContentPaddings>
						<ClientSideEvents Init="function(s, e) {
	loadingPanel.Hide();
}"></ClientSideEvents>
						<PanelCollection>
							<dx:PanelContent runat="server">
								<table width="600" cellpadding="2">
									<tbody>
                                        <tr>
<td style="width: 80px">
												<span style="font-family: Calibri">From:</span>
											</td>
                                            <td colspan="2">
<dx:ASPxTextBox runat="server" Width="277px" ID="txtFromEmailAddress" ClientInstanceName="txtFromEmailAddress" ClientEnabled="false" Theme="NETheme01">
												</dx:ASPxTextBox>
                                                </td>
                                        </tr>
										<tr>
											<td style="width: 80px">
												<span style="font-family: Calibri">To:</span>
											</td>
											<td colspan="2">
												
												<dx:ASPxComboBox ID="cbEmailList" runat="server" TextField="email" ValueField="id" ValueType="System.String" Width="277px" Theme="NETheme01">
													
												</dx:ASPxComboBox>
												<dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Select Emails from Contacts" Theme="NETheme01">
												</dx:ASPxLabel>
											</td>
										</tr>
										<tr>
											<td style="width: 80px">
												<span style="font-family: Calibri">Cc:</span>
											</td>
											<td colspan="2">
												<dx:ASPxTextBox runat="server" Width="277px" ID="txtEmailCC" Theme="NETheme01">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td style="width: 80px">
												Bcc:
											</td>
											<td colspan="2"><span style="font-family: Calibri">
												(Use this to send to multiple Vendors.&nbsp; Use a ; to separate)<br /></span>
												<dx:ASPxTextBox runat="server" Width="100%" ID="txtEmailBCC" Height="40px" Theme="NETheme01">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td style="width: 80px">
												<span style="font-family: Calibri">Subject:</span>
											</td>
											<td colspan="2">
												<dx:ASPxTextBox runat="server" Width="100%" ID="txtSubject" Theme="NETheme01">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td style="width: 80px">
												<span style="font-family: Calibri">Body:</span>
											</td>
											<td colspan="2">
												<dx:ASPxMemo runat="server" Height="80px" Width="100%" ClientInstanceName="memoBody" ID="memoBody" Theme="NETheme01">
													<BackgroundImage HorizontalPosition="center" VerticalPosition="center" />
													<BackgroundImage HorizontalPosition="center" VerticalPosition="center"></BackgroundImage>
												</dx:ASPxMemo>
											</td>
										</tr>
										<tr>
											<td style="width: 80px">
												<dx:ASPxButton runat="server" Text="Send" ID="btnSendEmail" OnClick="btnSendEmail_Click" Theme="NETheme01">
													<ClientSideEvents Click="function(s, e) {
	loadingPanel.Show();
}"></ClientSideEvents>
												</dx:ASPxButton>
											</td>
											<td style="width: 50px">
												<dx:ASPxCheckBox runat="server" Wrap="False" Text="CC to me..." ID="chksendtome" Theme="NETheme01">
												</dx:ASPxCheckBox>
											</td>
											<td style="width: 750px">
												<dx:ASPxTextBox runat="server" Width="250px" ID="txtmyemail" Theme="NETheme01">
												</dx:ASPxTextBox>
											</td>
										</tr>
										<tr>
											<td colspan="3">
												&nbsp;
											</td>
										</tr>
									</tbody>
								</table>
							</dx:PanelContent>
						</PanelCollection>
						<Border BorderColor="#68AFFD" BorderStyle="Solid" BorderWidth="1px"></Border>
					</dx:ASPxRoundPanel>
				</dx:PopupControlContentControl>
			</contentcollection>
            <HeaderStyle BackColor="Transparent" />
        </dx:ASPxPopupControl>&nbsp;
        <dx:ASPxLoadingPanel ID="loadingPanel" runat="server" ClientInstanceName="loadingPanel"
            Modal="True" Text="Sending Purchase Order via Email&hellip;" Theme="NETheme01">
        </dx:ASPxLoadingPanel>
        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Larger"
            ForeColor="Red" Visible="False"></asp:Label><br />
        <asp:Label ID="lblPOStatus" runat="server" Font-Names="Calibri" Text="PO Status:"></asp:Label>
        <asp:Label ID="lblPOStatusDisp" runat="server" Font-Names="Calibri"></asp:Label>
        
        <dx:reporttoolbar id="ReportToolbar1" runat="server" reportviewer="<%# ReportViewer1 %>"
            showdefaultbuttons="False" Width="100%"><Items>
<dx:ReportToolbarButton ItemKind="Search"></dx:ReportToolbarButton>
<dx:ReportToolbarSeparator></dx:ReportToolbarSeparator>
<dx:ReportToolbarButton ItemKind="PrintReport"></dx:ReportToolbarButton>
<dx:ReportToolbarButton ItemKind="PrintPage"></dx:ReportToolbarButton>
<dx:ReportToolbarSeparator></dx:ReportToolbarSeparator>
<dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage"></dx:ReportToolbarButton>
<dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage"></dx:ReportToolbarButton>
<dx:ReportToolbarLabel ItemKind="PageLabel"></dx:ReportToolbarLabel>
<dx:ReportToolbarComboBox Width="65px" ItemKind="PageNumber"></dx:ReportToolbarComboBox>
<dx:ReportToolbarLabel ItemKind="OfLabel"></dx:ReportToolbarLabel>
<dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount"></dx:ReportToolbarTextBox>
<dx:ReportToolbarButton ItemKind="NextPage"></dx:ReportToolbarButton>
<dx:ReportToolbarButton ItemKind="LastPage"></dx:ReportToolbarButton>
<dx:ReportToolbarSeparator></dx:ReportToolbarSeparator>
<dx:ReportToolbarButton ItemKind="SaveToDisk"></dx:ReportToolbarButton>
<dx:ReportToolbarButton ItemKind="SaveToWindow"></dx:ReportToolbarButton>
<dx:ReportToolbarComboBox Width="70px" ItemKind="SaveFormat"><Elements>
<dx:ListElement Value="pdf"></dx:ListElement>
<%--<dx:ListElement Value="xls"></dx:ListElement>
<dx:ListElement Value="xlsx"></dx:ListElement>
<dx:ListElement Value="rtf"></dx:ListElement>
<dx:ListElement Value="mht"></dx:ListElement>
<dx:ListElement Value="html"></dx:ListElement>
<dx:ListElement Value="txt"></dx:ListElement>
<dx:ListElement Value="csv"></dx:ListElement>
<dx:ListElement Value="png"></dx:ListElement>--%>
</Elements>
</dx:ReportToolbarComboBox>
<dx:ReportToolbarButton Name="Email" Text="Email" />
</Items>

<Styles>

<LabelStyle>
<Margins MarginLeft="3px" MarginRight="3px"></Margins>
</LabelStyle>
</Styles>
            <ClientSideEvents ItemClick="function(s, e) {
	if (e.item.name == 'Email')
	popupemail.Show();
}" />
</dx:reporttoolbar>
    
    </div>
        <dx:reportviewer id="ReportViewer1" runat="server">
            <paddings paddingleft="20px" />
        </dx:reportviewer>
    </form>
</body>
</html>
