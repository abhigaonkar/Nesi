<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_workorder_modules_analysis" Codebehind="analysis.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="accounting_notes.ascx" tagname="accounting_notes" tagprefix="uc1" %>
<%@ Register Src="~/sections/workorder/modules/bv_post.ascx" TagPrefix="uc1" TagName="bv_post" %>



<link type="text/css" href="/css/analysis.css" rel="Stylesheet" />
	
	<style runat="server" type="text/css" id="css"></style>
<script type="text/javascript" src="/js/wo_analysis.js"></script>

<script type="text/javascript">

   
</script>

	<asp:HiddenField ID="hdn_woid" runat="server" />
<table cellpadding="8" width="100%" style="vertical-align: top" cellspacing="0">
	<tr>
		<td style="width: 50%">
			<asp:CheckBox ID="chk_uselinked" runat="server" Checked="True" OnCheckedChanged="chk_uselinked_CheckedChanged" Text="Include Change Orders (See Linked WOs)" AutoPostBack="True" />
		</td>
		<td valign="top"></td>
	</tr>
	<tr>
		<td style="width: 50%">
			<asp:CheckBox ID="chkIncludeOpenPOs" runat="server" OnCheckedChanged="chk_uselinked_CheckedChanged" Text="Include PO's Waiting to be Received in Analysis" AutoPostBack="True" Visible="true" />
		</td>
		<td valign="top"></td>
	</tr>
</table>




<div class="wrapper_">
	<div class="header_margin" id="div_margins_wrapper" runat="server">
		Margins
		<div class="panel_">
			<div id="div_margins" runat="server"></div>
		</div>

		<div class="header_benchmark">Benchmark Sell Analysis (Total Job)</div>
		<div class="panel_">


			<div id="benchmark_totals" runat="server">
				<dx:ASPxCallbackPanel ID="cb_bench_credit" runat="server"
					ClientInstanceName="cb_bench_credit" ClientVisible="False"
					OnCallback="cb_bench_credit_Callback" Width="200px">
					<PanelCollection>
						<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
							<table style="width: 100%;">
								<tr>
									<td>
										<strong>Benchmark Credit:</strong></td>
									<td>
										<dx:ASPxTextBox ID="txt_benchmark_credit0" runat="server"
											Style="color: #FF0000" Width="70px">
											<ClientSideEvents TextChanged="function(s, e) {
	cb_bench_credit.PerformCallback(s.GetText());
}" />
										</dx:ASPxTextBox>
									</td>
									<td>&nbsp;</td>
								</tr>
							</table>
						</dx:PanelContent>
					</PanelCollection>
				</dx:ASPxCallbackPanel>
				<div id="div_Benchmark_Totals" runat="server">
					<br />
				</div>
			</div>
		</div>
	</div>
</div>


<div id="div_quote_wrapper" runat="server" class="wrapper_">
	<dx:ASPxCallback ID="cb_quote" ClientInstanceName="cb_quote" runat="server" OnCallback="cb_quote_Callback">
		<ClientSideEvents CallbackComplete="function(s, e) {
$('.div_QuoteTotals').html(e.result);	
please_wait('stop');
}"
			BeginCallback="function(s, e) {
	please_wait('start');
}" />
	</dx:ASPxCallback>
	<div class="header_quote"><b>Margin Analysis</b></div>
	<div class="panel_">
		<div id="div_QuoteTotals" class="div_QuoteTotals" runat="server"></div>
	</div>
</div>
<div class="wrapper_">
	<div class="header_billtype">Billtype Totals</div>
	<div class="panel_">
		<div id="div_Billtype_Totals" runat="server"></div>
	</div>
</div>

   <div id ="div_wrapper_accounting" class="wrapper_accounting" runat="server" >
       <uc1:bv_post runat="server" ID="bv_post" />
</div>

 
   
       
                           




