<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_dashboards_wo_grid" Codebehind="wo_grid.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register src="~/modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>


<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>


<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<script type="text/javascript" language="javascript">
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 0) + 'px';
	
 }
 </script>
							<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select name,business_unit_id from business_unit  where active = &#39;T&#39;" ID="SqlDataSource2"></asp:SqlDataSource>

							
						
   
	<dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
		Width="100%" KeyFieldName="woprog_id" 
		onhtmlrowprepared="ASPxGridView1_HtmlRowPrepared" 
		oncustomcallback="ASPxGridView1_CustomCallback1" 
		ClientInstanceName="ASPxGridView1" 
		onhtmldatacellprepared="ASPxGridView1_HtmlDataCellPrepared1" 
		onsummarydisplaytext="ASPxGridView1_SummaryDisplayText" 
		style="text-align: left">
		<TotalSummary>
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="tobebilled" 
				ShowInColumn="To Be Billed" SummaryType="Sum" ValueDisplayFormat="C0" />
			<dx:ASPxSummaryItem DisplayFormat="P1" FieldName="margin" ShowInColumn="Margin" 
				SummaryType="Sum" ValueDisplayFormat="P1" />
		
				<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="dollars_margin" 
				ShowInColumn="Margin $" ShowInGroupFooterColumn="dollars_margin" 
				SummaryType="Sum" ValueDisplayFormat="C0" />
		</TotalSummary>
		<Columns>
			<dx:GridViewCommandColumn Visible="False" VisibleIndex="0" ShowClearFilterButton="true" >
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="WO" FieldName="bvwo" 
				VisibleIndex="1" Width="80px">
			<DataItemTemplate>
							<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
				NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 'workorder', 1200,800)&quot;, Eval(&quot;woprog_id&quot;), Eval(&quot;cid&quot;)) %>" 
				Text='<%# Eval("bvwo") %>'>
				
				</dx:ASPxHyperLink>
						</DataItemTemplate>
				<FooterCellStyle HorizontalAlign="Left">
				</FooterCellStyle>
				<FooterTemplate>
					<dx1:ASPxLabel ID="lblcount" runat="server" ClientInstanceName="lblcount">
						<ClientSideEvents Init="function(s, e) {
	s.SetText(ASPxGridView1.GetVisibleRowsOnPage());
}" />
					</dx1:ASPxLabel>
				</FooterTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
				VisibleIndex="3" Width="100%">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Left" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
				VisibleIndex="4" Width="80px">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Left" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="To Be Billed" FieldName="tobebilled" 
				VisibleIndex="5" Width="80px">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Right">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Right">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Margin" FieldName="margin" 
				VisibleIndex="6" Width="60px">
				<PropertiesTextEdit DisplayFormatString="p1">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Center">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Customer" FieldName="Customer" 
				VisibleIndex="2" Width="150px">

<CellStyle HorizontalAlign="Left" Wrap="False"></CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Expected Sales" FieldName="exp_sales_value" 
				VisibleIndex="7" Width="100px">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" oninit="ASPxTextBox1_Init" 
						Width="80px" Text='<%# Eval("exp_sales_value") %>' HorizontalAlign="Right" 
						style="text-align: right">
					</dx:ASPxTextBox>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<FooterCellStyle HorizontalAlign="Right">
				</FooterCellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="id" FieldName="woprog_id" VisibleIndex="8" 
				Visible="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="cid" FieldName="cid" VisibleIndex="8" 
				Visible="False">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Expected End Date" 
				FieldName="exp_completion_date" VisibleIndex="9" Width="120px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
				<DataItemTemplate>
					<dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" 
						ClientInstanceName="ASPxDateEdit1" DisplayFormatString="yyyy-MM-dd" 
						oninit="ASPxDateEdit1_Init" Width="105px" Value='<%# Eval("exp_completion_date") %>' 
						EditFormat="Custom" EditFormatString="yyyy-MM-dd">
					</dx:ASPxDateEdit>
				</DataItemTemplate>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="Days to Process" 
				FieldName="days_to_process" VisibleIndex="11" Width="90px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataDateColumn Caption="Invoice Date" FieldName="inv_date" 
				MinWidth="20" VisibleIndex="13" Width="80px">
				<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
					EditFormatString="yyyy-MM-dd">
				</PropertiesDateEdit>
			</dx:GridViewDataDateColumn>
			<dx:GridViewDataTextColumn Caption="Margin $" 
				FieldName="dollars_margin" VisibleIndex="14" Width="90px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager PageSize="20" Mode="ShowAllRecords">
		</SettingsPager>
		<Settings ShowFooter="True" ShowFilterRow="True" ShowFilterBar="Auto" 
			ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
		<Styles>
			<Footer HorizontalAlign="Center">
			</Footer>
		</Styles>
	</dx:ASPxGridView>

							
						
   
</asp:Content>

<asp:Content ID="Content4" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">

.dxeBase
{
	font: 12px Tahoma, Geneva, sans-serif;
}
	</style>
</asp:Content>


