<%@ Page Language="C#" AutoEventWireup="true" Theme="" MasterPageFile="~/nonFrame.master" Inherits="sections_hr_member_vacation_import" Codebehind="vacation_import.aspx.cs" %>

<asp:Content ContentPlaceHolderID="cphMasterBody" ID="body" runat="server">
	<script type="text/javascript" src="/js/functions.js"></script>
	<div style="padding: 25px;">
		Format of csv file must be as follows:
		<ul>
			<li>No headers</li>
			<li>Only numbers and decimals</li>
			<li>Each line needs to be formatted as <b style="color: #f90;">payroll_id</b>,<b style="color: #090;">dollars or hours</b> - <b style="color: #f90;">###</b>,<b style="color: #090;">##.##</b> - i.e. <b style="color: #f90;">5555</b>,<b style="color: #090;">120.00</b> (payroll id <b style="color: #f90;">5555</b>, <b style="color: #090;">120</b> hours/dollars) </li>
		</ul>
		<div align="center">
			<table cellpadding="2" cellspacing="0" width="400">
				<tr>
					<td>
						<asp:ListBox Width="100%" SelectionMode="Multiple" Height="250px" ID="ddl_branches" runat="server" DataSourceID="sds_branches" DataTextField="name" DataValueField="id"></asp:ListBox>
						<asp:SqlDataSource ID="sds_branches" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT * FROM vw_active_business_units"></asp:SqlDataSource>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Button Text="Download Template for Branch (using current figures)" Width="100%" runat="server" autopostback="true" ID="bt_template" OnClick="bt_template_Click" /></td>
				</tr>
			</table>
			<br />
			<br />
			<br />
			<br />
			<table width="100%">
				<tr>
					<td style="width: 50%" align="right">
						<asp:FileUpload ID="uc" runat="server" /></td>
					<td style="width: 50%" align="left">
						<asp:Button ID="bt_upload" runat="server" autopostback="true" Text="Submit" OnClick="bt_upload_Click" /></td>
				</tr>
			</table>
			<div id="results" runat="server" style="width: 100%;" align="left"></div>
		</div>
	</div>
</asp:Content>
