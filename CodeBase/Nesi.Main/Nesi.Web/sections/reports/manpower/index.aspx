<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_reports_manpower_index" Title="Manpower" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>





<asp:Content ID="head" ContentPlaceHolderID="header_placeholder" runat="server">
	<style type="text/css">
		.column1
			{
			background-color:	#DDEBF7;
			}
		.column2
			{
			background-color:	#BDD7EE;
			}
		.column3
			{
			background-color:	#9BC2E6;
			}
		.dxeCaption_NETheme01
			{
			font-size:			12px !important;
			}
	</style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">

				<dx:ASPxCallbackPanel ID="cbp" runat="server" ClientInstanceName="cbp" oncallback="cbp_Callback">
					<PanelCollection>
						<dx:PanelContent>
	<table>
		<tr>
			<td valign="top">
				<dx:ASPxComboBox ID="ddl_month" Caption="Starting Month" runat="server" TextField="yearmonth" ValueField="n" ValueType="System.Int32" Width="100%">
						<CaptionSettings Position="Top" />
						<CaptionStyle Font-Size="14px" Font-Names="Segoe UI',Helvetica,'Droid Sans',Tahoma,Geneva,sans-serif">
						</CaptionStyle>
				</dx:ASPxComboBox>
			</td>
			<td rowspan="3" valign="top">
						<br />
			<table cellpadding="2" cellspacing="0" width="780" style="font:12px 'Segoe UI',Helvetica,'Droid Sans',Tahoma,Geneva,sans-serif;">
				<thead>
					<tr>
						<th width="25%"></th>
						<th width="15%" align="center" class="column1"><dx:ASPxLabel ID="lb_month1_header" runat="server" ForeColor="Black" Font-Bold="True"></dx:ASPxLabel></th>
						<th width="15%" align="center" class="column2"><dx:ASPxLabel ID="lb_month2_header" runat="server" ForeColor="Black" Font-Bold="True"></dx:ASPxLabel></th>
						<th width="15%" align="center" class="column3"><dx:ASPxLabel ID="lb_month3_header" runat="server" ForeColor="Black" Font-Bold="True"></dx:ASPxLabel></th>
						<th></th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td>Expected Quote Revenue ($)</td>
						<td align="center" class="column1"><div class="opt1" data-tooltip='Quote value * percent chance of winning / number of months spanning the job.'><dx:ASPxLabel ID="lb_expquoterev_month1" 
								ForeColor="Black" runat="server" Theme="NETheme01"></dx:ASPxLabel></div></td>
						<td align="center" class="column2"><div class="opt1" data-tooltip='Quote value * percent chance of winning / number of months spanning the job.'><dx:ASPxLabel ID="lb_expquoterev_month2" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td align="center" class="column3"><div class="opt1" data-tooltip='Quote value * percent chance of winning / number of months spanning the job.'><dx:ASPxLabel ID="lb_expquoterev_month3" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td><div class="opt1"><dx:ASPxLabel ID="lb_expquoterev_formula" runat="server" EncodeHtml="false" 
								ClientVisible="False"></dx:ASPxLabel></td>
					</tr>
					<tr>
						<td>Expected T+M Revenue ($)</td>
						<td align="center" class="column1"><div class="opt1" data-tooltip='Average revenue from time and meterial jobs for the previous 3 months.'><dx:ASPxLabel ID="lb_tmrev_month1" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td align="center" class="column2"><div class="opt1" data-tooltip='Average revenue from time and meterial jobs for the previous 3 months.'><dx:ASPxLabel ID="lb_tmrev_month2" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td align="center" class="column3"><div class="opt1" data-tooltip='Average revenue from time and meterial jobs for the previous 3 months.'><dx:ASPxLabel ID="lb_tmrev_month3" 
								ForeColor="Black" runat="server" 
								
								></dx:ASPxLabel></div></td>
						<td><div class="opt1"><dx:ASPxLabel ID="lb_tmrev_formula" runat="server" EncodeHtml="false" 
								ClientVisible="False"></dx:ASPxLabel></td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td class="column1">&nbsp;</td>
						<td class="column2">&nbsp;</td>
						<td class="column3">&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>Expected Billable Hours (hr)</td>
						<td align="center" class="column1"><div class="opt1" data-tooltip='Hours found on worksheets for amount of quotes expected to win + average of hours billed on time and meterial jobs over the past 3 months.'><dx:ASPxLabel ID="lb_billablehours_month1" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td align="center" class="column2"><div class="opt1" data-tooltip='Hours found on worksheets for amount of quotes expected to win + average of hours billed on time and meterial jobs over the past 3 months.'><dx:ASPxLabel ID="lb_billablehours_month2" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td align="center" class="column3"><div class="opt1" data-tooltip='Hours found on worksheets for amount of quotes expected to win + average of hours billed on time and meterial jobs over the past 3 months.'><dx:ASPxLabel ID="lb_billablehours_month3" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td><div class="opt1"><dx:ASPxLabel ID="lb_billablehours_formula" runat="server" EncodeHtml="false" 
								ClientVisible="False"></dx:ASPxLabel></td>
					</tr>
					<tr>
						<td>Expected Utilization (%)</td>
						<td align="center" class="column1"><div class="opt1" data-tooltip='Expected hours billable / total hours paid for over the month'><dx:ASPxLabel ID="lb_utilization_month1" ForeColor="Black" runat="server"></dx:ASPxLabel></div></td>
						<td align="center" class="column2"><div class="opt1" data-tooltip='Expected hours billable / total hours paid for over the month'><dx:ASPxLabel ID="lb_utilization_month2" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td align="center" class="column3"><div class="opt1"><dx:ASPxLabel ID="lb_utilization_month3" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td><div class="opt1"><dx:ASPxLabel ID="lb_utilization_formula" runat="server" EncodeHtml="false" 
								ClientVisible="False"></dx:ASPxLabel></div></td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td class="column1">&nbsp;</td>
						<td class="column2">&nbsp;</td>
						<td class="column3">&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>People Needed</td>
						<td align="center" class="column1"><div class="opt1" data-tooltip='Assuming a 90% hour utilization rate, and the above expected billable hours, the needed people count = billable hours * 1/HU% /8 hours/ 22 days = Number of people over the month.'><dx:ASPxLabel ID="lb_totalheadcount_month1" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td align="center" class="column2"><div class="opt1" data-tooltip='Assuming a 90% hour utilization rate, and the above expected billable hours, the needed people count = billable hours * 1/HU% /8 hours/ 22 days = Number of people over the month.'><dx:ASPxLabel ID="lb_totalheadcount_month2" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td align="center" class="column3"><div class="opt1" data-tooltip='Assuming a 90% hour utilization rate, and the above expected billable hours, the needed people count = billable hours * 1/HU% /8 hours/ 22 days = Number of people over the month.'><dx:ASPxLabel ID="lb_totalheadcount_month3" 
								ForeColor="Black" runat="server" 
								></dx:ASPxLabel></div></td>
						<td><div class="opt1"><dx:ASPxLabel ID="lb_totalheadcount_formula" runat="server" EncodeHtml="false" 
								ClientVisible="False"></dx:ASPxLabel></div></td>
					</tr>
					<tr>
						<td>Current People Count</td>
						<td align="center" class="column1"><div class="opt1" data-tooltip='Current people count in the branch less the branch manager.'><dx:ASPxLabel ID="lb_actualheadcount_month1" ForeColor="Black" runat="server"></dx:ASPxLabel></div></td>
						<td align="center" class="column2"><div class="opt1" data-tooltip='Current people count in the branch less the branch manager.'><dx:ASPxLabel ID="lb_actualheadcount_month2" ForeColor="Black" runat="server"></dx:ASPxLabel></div></td>
						<td align="center" class="column3"><div class="opt1" data-tooltip='Current people count in the branch less the branch manager.'><dx:ASPxLabel ID="lb_actualheadcount_month3" ForeColor="Black" runat="server"></dx:ASPxLabel></div></td>
						<td><div class="opt1"><dx:ASPxLabel ID="lb_actualheadcount_formula" runat="server" EncodeHtml="false" 
								ClientVisible="False"></dx:ASPxLabel></div></td>
					</tr>
					<tr>
						<td>&nbsp;Surplus/(Deficit)</td>
						<td align="center" class="column1"><div class="opt1"><dx:ASPxLabel ID="lb_surpdeficit_month1" ForeColor="Black" runat="server"></dx:ASPxLabel></div></td>
						<td align="center" class="column2"><div class="opt1"><dx:ASPxLabel ID="lb_surpdeficit_month2" ForeColor="Black" runat="server"></dx:ASPxLabel></div></td>
						<td align="center" class="column3"><div class="opt1"><dx:ASPxLabel ID="lb_surpdeficit_month3" ForeColor="Black" runat="server"></dx:ASPxLabel></div></td>
						<td><div class="opt1"><dx:ASPxLabel ID="lb_surpdeficit_formula" runat="server" EncodeHtml="false" 
								ClientVisible="False"></dx:ASPxLabel></div></td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td class="column1">&nbsp;</td>
						<td class="column2">&nbsp;</td>
						<td class="column3">&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>For Reference - Scheduled on scheduler</td>
						<td align="center" class="column1"><div class="opt1"><dx:ASPxLabel ID="lb_reference_month1" 
								ForeColor="Black" runat="server" 
								ToolTip="This is the number of people currently scheduled in the scheduler module of nesi.ca"></dx:ASPxLabel></div></td>
						<td align="center" class="column2"><div class="opt1"><dx:ASPxLabel ID="lb_reference_month2" 
								ForeColor="Black" runat="server" 
								ToolTip="This is the number of people currently scheduled in the scheduler module of nesi.ca"></dx:ASPxLabel></div></td>
						<td align="center" class="column3"><div class="opt1"><dx:ASPxLabel ID="lb_reference_month3" 
								ForeColor="Black" runat="server" 
								ToolTip="This is the number of people currently scheduled in the scheduler module of nesi.ca"></dx:ASPxLabel></div></td>
						<td><div class="opt1"><dx:ASPxLabel ID="lb_reference_formula" runat="server" EncodeHtml="false" 
								ClientVisible="False"></dx:ASPxLabel></div></td>
					</tr>
				</tbody>
			</table>
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxCheckBoxList ID="cbl_branch" runat="server" ValueField="id" TextField="name"  ValueType="System.Int32" Caption="Business Unit:" RepeatColumns="1" Width="250px">
					<CaptionSettings Position="Top" />
					<CaptionStyle Font-Names="Segoe UI',Helvetica,'Droid Sans',Tahoma,Geneva,sans-serif" Font-Size="12px" ForeColor="#777777">
					</CaptionStyle>
				</dx:ASPxCheckBoxList>
			</td>
		</tr>
		<tr>
			<td valign="top"><br /><br />
				<dx:ASPxButton ID="bt_sub" Text="Go -&gt;" runat="server" AutoPostBack="false" Width="100%">
					<ClientSideEvents Click="function(s, e) {
	cbp.PerformCallback();
}" />
				</dx:ASPxButton>
			</td>
		</tr>
	</table>
	<br />
						</dx:PanelContent>
					</PanelCollection>
					<ClientSideEvents EndCallback="function(){page_obj.bindtips();}" />
				</dx:ASPxCallbackPanel>
	</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

