<%@ Page Title="Print Review" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_hr_member_print_review" Codebehind="print_review.aspx.cs" %>

<%@ Register assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <dx:ReportToolbar ID="rv_toolbar" runat="server" ReportViewerID="rv" 
		ShowDefaultButtons="False" Width="100%">
		<Items>
			<dx:ReportToolbarButton ItemKind="Search" />
			<dx:ReportToolbarSeparator />
			<dx:ReportToolbarButton ItemKind="PrintReport" />
			<dx:ReportToolbarButton ItemKind="PrintPage" />
			<dx:ReportToolbarSeparator />
			<dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
			<dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
			<dx:ReportToolbarLabel ItemKind="PageLabel" />
			<dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
			</dx:ReportToolbarComboBox>
			<dx:ReportToolbarLabel ItemKind="OfLabel" />
			<dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
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
					<dx:ListElement Value="html" />
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
	<dx:ReportViewer ID="rv" runat="server">
		<Border BorderStyle="Solid" BorderWidth="1px" />
	</dx:ReportViewer>
</asp:Content>

