<%@ Page Language="C#" AutoEventWireup="true" Inherits="service_report" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>








<%@ Register Assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
	<style type="text/css">
		.style1 {
			font-family: Arial, Helvetica, sans-serif;
			font-size: small;
		}
	</style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
		<asp:ScriptManager ID="sc_main" runat="server">
		</asp:ScriptManager>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
			<ProgressTemplate>
				<span class="style1"><strong>Updating...</strong></span>
			</ProgressTemplate>
		</asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<dx:ASPxMemo ID="text_description" runat="server" Height="71px" Width="100%">
				</dx:ASPxMemo>
				<dx:ASPxButton ID="b_update_description" runat="server" Text="Update Description" onclick="b_update_description_Click">
				</dx:ASPxButton>
		<br />
        <dx:ReportToolbar ID="ReportToolbar1" runat="server" ReportViewer="<%# report_viewer %>"
            ShowDefaultButtons="False" Width="100%">
            <Items>
                <dx:ReportToolbarButton ItemKind="Search" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="PrintReport" Name="PrintReport" />
                <dx:ReportToolbarButton ItemKind="PrintPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                <dx:ReportToolbarLabel ItemKind="PageLabel" />
                <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                </dx:ReportToolbarComboBox>
                <dx:ReportToolbarLabel ItemKind="OfLabel" />
                <dx:ReportToolbarTextBox ItemKind="PageCount" />
                <dx:ReportToolbarButton ItemKind="NextPage" />
                <dx:ReportToolbarButton ItemKind="LastPage" />
                <dx:ReportToolbarSeparator />
                <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                    <Elements>
                        <dx:ListElement Value="pdf" />
                        <dx:ListElement Value="xls" />
                        <dx:ListElement Value="xlsx" />
                        <dx:ListElement Value="rtf" />
                        <dx:ListElement Value="mht" />
                        <dx:ListElement Value="txt" />
                        <dx:ListElement Value="csv" />
                        <dx:ListElement Value="png" />
                    </Elements>
                </dx:ReportToolbarComboBox>
            </Items>
            <Styles>
                <LabelStyle>
                    <Margins MarginLeft="3px" MarginRight="3px" />
                </LabelStyle>
            </Styles>
        </dx:ReportToolbar>
        <dx:ReportViewer ID="report_viewer" ClientInstanceName="report_viewer" runat="server">
            <Border BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
        </dx:ReportViewer>
        &nbsp;
			</ContentTemplate>
		</asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
