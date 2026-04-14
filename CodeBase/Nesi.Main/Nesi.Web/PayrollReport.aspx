<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="PayrollReport" Title="Payroll Report" Codebehind="PayrollReport.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
<div id="divSide" runat="server">
    <asp:SqlDataSource ID="Users" runat="server"></asp:SqlDataSource>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
<div id="divMenu" runat="server"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
		<script type="text/javascript">
			$(document).ready(function()
									{
									$(document).find('.member').each(function()
																		{
																		$(this).tablesorter();
																		});
									$('.submit_button').click(function()
																{
																var payperiod_id		= $('.ddl_payperiod').val();
																var business_unit_id			= $('.ddl_branch').val();
																var start_date			= $('.text_start_date').val();
																var end_date			= $('.text_end_date').val();
																location.href			= "payrollreport.aspx?payperiod_id="+payperiod_id+"&business_unit_id="+business_unit_id+"&start_date="+start_date+"&end_date="+end_date;
																});
									});
		</script>
		<table>
			<tr>
				<td style="width: 100px">
					<strong>Business Unit:</strong></td>
				<td style="width: 318px">
					<asp:DropDownList ID="ddl_branch" CssClass='ddl_branch' runat="server" Width="100%">
					</asp:DropDownList></td>
			</tr>
			<tr>
				<td colspan="2" style="height: 36px" valign="bottom">
					<span style="font-size: 8pt"><strong>Choose a Pay Period</strong></span></td>
			</tr>
			<tr>
				<td style="width: 100px; height: 23px;">
					<strong>Pay Period:</strong></td>
				<td style="width: 318px; height: 23px;">
					<asp:DropDownList ID="ddl_payperiod" CssClass='ddl_payperiod' runat="server" Width="100%">
					</asp:DropDownList></td>
			</tr>
			<tr>
				<td colspan="2" style="height: 29px" valign="bottom">
					<span style="font-size: 8pt"><strong>or choose a date range, this will supercede the
						chosen pay period</strong></span></td>
			</tr>
			<tr>
				<td style="width: 100px">
					<strong>Start Date:</strong></td>
				<td style="width: 318px">
					<asp:TextBox ID="text_start_date" CssClass='text_start_date' runat="server" Width="100px"></asp:TextBox></td>
			</tr>
			<tr>
				<td style="width: 100px">
					<strong>End Date:</strong></td>
				<td style="width: 318px">
					<asp:TextBox ID="text_end_date" CssClass='text_end_date' runat="server" Width="100px"></asp:TextBox></td>
			</tr>
			<tr>
				<td colspan="2">
					<button class='submit_button' type="button">
						Search</button>
					<button type="button" onclick='window.print();'>Print Page</button></td>
			</tr>
		</table>
		<br /><br />
	<div id='div_payroll_report' runat="server"> &nbsp;</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
    &nbsp;
</asp:Content>

