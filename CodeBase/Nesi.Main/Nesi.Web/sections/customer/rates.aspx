<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="customer_rates" Title="Customer Rates"  EnableTheming="True" Codebehind="rates.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="header_placeholder" Runat="Server">
	<style type="text/css">
		.charge * { text-align: center; }
		.style1 { font-family: Arial, Helvetica, sans-serif; font-size: medium; text-decoration: underline; }
		.tdate,
		.fdate
			{
			font-size:		11px;
			text-align:		center;
			font-family:	arial;
			font-weight:	bold;
			}
		.rates *
			{
			font-size:		11px;
			font-family:	arial;
			}
	</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<asp:ScriptManager ID="sm" runat="server" onasyncpostbackerror="sm_AsyncPostBackError">
	</asp:ScriptManager>
	<asp:UpdatePanel ID="up" runat="server">
		<ContentTemplate>
	<script type="text/javascript">
		function bind_dates()
			{
			$(".fdate").each(function()
				{
				$(this).datepicker(
					{
					dateFormat:	"yy-mm-dd",
					constrainInput: true,
					onSelect:	function(d)
						{
						var tdate		= $(this).parents("tr:first").find(".tdate").datepicker("getDate");
						var fdate		= $(this).datepicker("getDate");
						if(tdate != null && fdate > tdate)
							{
							$(this).val("");
							return false;
							}
						}
					});
				});

			$(".tdate").each(function()
				{
				$(this).datepicker(
					{
					dateFormat:	"yy-mm-dd",
					minDate:	"dateToday",
					constrainInput: true,
					onSelect:	function(d)
						{
						var fdate		= $(this).parents("tr:first").find(".fdate").datepicker("getDate");
						var tdate		= $(this).datepicker("getDate");
						if(fdate != null && fdate > tdate)
							{
							$(this).val("");
							return false;
							}
						}
					});
				});
			}
		$(document).ready(function()
			{
			bind_dates();
			});
	</script>
			<dx:ASPxComboBox ID="combo_company" runat="server" TextField="name" ValueField="id" AutoPostBack="True" onselectedindexchanged="combo_company_SelectedIndexChanged">
			</dx:ASPxComboBox>
			<dx:ASPxGridView ID="gv_rates" runat="server" AutoGenerateColumns="False" 
				DataSourceID="sds_rates" style="text-align: left" KeyFieldName="id" 
				onhtmldatacellprepared="gv_rates_HtmlDataCellPrepared" CssClass="rates" 
				Theme="NETheme01" Width="100%">
				<ClientSideEvents EndCallback="function(s, e) {
	bind_dates();
}" />
				<Columns>
					<dx:GridViewDataTextColumn Caption="Member Type" FieldName="name" 
						VisibleIndex="0" MinWidth="150" Width="100%">
						<Settings AutoFilterCondition="Contains" />
						<DataItemTemplate>
							<div id="name" runat="server" style="overflow-x:hidden;width:100%;white-space:nowrap"><%# Eval("name") %></div>
						</DataItemTemplate>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="From Date" FieldName="from_date" 
						VisibleIndex="1" Width="100px">
						<DataItemTemplate>
							<input type="text" ID="t_fromdate" class='fdate' style="width:100%;" runat="server" value='<%# Eval("from_date") %>' />
						</DataItemTemplate>
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn Caption="To Date" FieldName="to_date" 
						VisibleIndex="2" Width="100px">
						<DataItemTemplate>
							<input type="text" ID="t_todate" class='tdate' style="width:100%;" runat="server" value='<%# Eval("to_date") %>' />
						</DataItemTemplate>
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Reg" FieldName="REG" VisibleIndex="4" 
						Width="65px">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
						<DataItemTemplate>
							<dx:ASPxTextBox ID="t_reg" runat="server" CssClass="charge" Text='<%# Eval("REG") %>' Width="100%">
							</dx:ASPxTextBox>
						</DataItemTemplate>
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="OT" VisibleIndex="5" Width="60px" 
						FieldName="OT">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="DT" VisibleIndex="6" Width="60px" 
						FieldName="DT">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="SP" VisibleIndex="7" Width="60px" 
						FieldName="REGSP">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="OT SP" VisibleIndex="8" Width="60px" 
						FieldName="OTSP">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="DT SP" FieldName="DTSP" VisibleIndex="9"  
						Width="60px">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Save" VisibleIndex="11" Width="105px" 
						MinWidth="10">
						<DataItemTemplate>
							<dx:ASPxButton ID="b_per_save" runat="server" Height="20px" Image-Height="16" Image-Url="~/images/icon/icon[save].gif" Image-Width="16" onclick="b_per_save_Click" Paddings-Padding="0" Width="100px" Text="Save Base" Native="True">
							</dx:ASPxButton>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="id" FieldName="id" Visible="False" 
						VisibleIndex="12">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Last Updated" FieldName="last_updated" 
						VisibleIndex="10" Width="100px" MinWidth="10">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
						</PropertiesDateEdit>
						<CellStyle Wrap="False" HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Normal" FieldName="norm" VisibleIndex="3" 
						Width="50px">
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
				</Columns>
				<SettingsBehavior ColumnResizeMode="Control" />
				<SettingsPager pagesize="200" Visible="False">
				</SettingsPager>
				<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" 
					ShowHeaderFilterButton="True" ShowTitlePanel="True" />
				<SettingsText Title="Customer WIDE Chargeout Rates" />
				<Styles>
					<Header Font-Bold="True" HorizontalAlign="Center">
					</Header>
				</Styles>
			</dx:ASPxGridView>
			<asp:SqlDataSource ID="sds_rates" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="call customer_chargeouts(@customer_id,@business_unit_id);">
				<SelectParameters>
					<asp:QueryStringParameter Name="@customer_id" QueryStringField="customer_id" />
					<asp:SessionParameter Name="@business_unit_id" SessionField="rates_business_unit_id" />
				</SelectParameters>
			</asp:SqlDataSource>
<script type="text/javascript">
	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
	Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
	function EndRequestHandler(sender, args)
		{
		if (args.get_error() != undefined && args.get_error().httpStatusCode == '500')
			{
			var errorMessage = args.get_error().message
			args.set_errorHandled(true);
			alert(errorMessage);
			}
		}
	function BeginReqHandler()
		{
		$(".update_progress").css({"height": $("body").height()+"px", "z-index":$.maxZ()});
		}

</script>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>

