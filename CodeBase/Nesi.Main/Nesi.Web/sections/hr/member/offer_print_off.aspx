<%@ Page Title="Employee Offer Printout" Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_hr_member_offer_print_off" Codebehind="offer_print_off.aspx.cs" %>

<%@ Register assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
	<style type="text/css">
		.style1
		{
			text-align: center;
            text-wrap:none;
		}
		.style2
		{
			text-align: left;
		}
		.style3
		{
			font-family: Arial, Helvetica, sans-serif;
		}
		.style4
		{
			font-size: small;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<div class="style1" align="center">
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
			<dx:ReportToolbarButton ItemKind="SaveToWindow" Enabled="False" />
			<dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
				<Elements>
					<dx:ListElement Value="pdf" />
				</Elements>
			</dx:ReportToolbarComboBox>
		</Items>
		<Styles>
			<LabelStyle>
			<Margins MarginLeft="3px" MarginRight="3px" />
			</LabelStyle>
		</Styles>
	</dx:ReportToolbar>
        </br>
        <asp:Button ID="btn_view_signed" runat="server" Text="View Signed Copy" OnClick="Button1_Click" Height="40px" Width="150px" />
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_view_unsigned" runat="server" Text="View UnSigned Copy" OnClick="Button2_Click" Height="40px" Width="150px" />
	</div>
	<div class="style2">
		<div class="style2" id="div_pdf_warning" runat="server">
			<span class="style3"><span class="style4">For Chrome users open </span>
			<a href="chrome://plugins/"><span class="style4">chrome://plugins/</span></a><span 
				class="style4"> in your browser and Disable <strong>Chrome PDF</strong> 
			Viewer and Enable <strong>Adobe Reader</strong></span></span><br 
				class="style3" />
		</div>
	<dx:ReportViewer ID="rv" runat="server">
        <paddings paddingleft="30px" />
	</dx:ReportViewer>
	</div>
</asp:Content>

