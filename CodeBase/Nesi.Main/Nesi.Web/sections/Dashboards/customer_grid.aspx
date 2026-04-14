<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_dashboards_customer_grid" Codebehind="customer_grid.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register src="~/modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>





<%@ Register src="../../modules/customer_lastaction.ascx" tagname="customer_lastaction" tagprefix="uc" %>


<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<script type="text/javascript" language="javascript">
function show_lastaction_popup(customer_id, address_id)
	{
	pop_lastaction.Show();
	pop_lastaction.PerformCallback(customer_id+"|"+address_id);
	}
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 150) + 'px';
	
    }
function opencustomer(id) {
        boing("/#/opens/10/customers/" + id, "customer", 1280, 960);
    }
 </script>
							<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select name,id from business_unit  where active = &#39;T&#39;" ID="SqlDataSource22"></asp:SqlDataSource>

							
						
   
	<lc:LayoutControl ID="layout" runat="server" GridviewID="gv_customergrid" ShowExcelExport="True" ShowPDFExport="False" ShowToggle="True" />

							
						
   
	<dx:ASPxGridView ID="gv_customergrid" runat="server" AutoGenerateColumns="False" 
		Width="100%" KeyFieldName="custid" 
		onhtmldatacellprepared="gv_customergrid_HtmlDataCellPrepared1" 
		ClientInstanceName="gv_customergrid" 
		oncustomcallback="gv_customergrid_CustomCallback" oncustomjsproperties="gv_customergrid_CustomJSProperties">
		<TotalSummary>
			<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="sixty" 
				ShowInColumn="AR Over 30 Days" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="thirty" 
				ShowInColumn="AR Current" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="ninety" 
				ShowInColumn="AR Over 60 Days" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="onetwenty" 
				ShowInColumn="AR Over 90 Days" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="onetwentyplus" 
				ShowInColumn="AR Over 120 Days" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="ebalance" 
				ShowInColumn="Total AR" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C0" FieldName="ytd_rev" 
				ShowInColumn="YTD Sales" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="rev_12mos" 
				ShowInColumn="rev_12mos" SummaryType="Sum" />
             <dx:ASPxSummaryItem DisplayFormat="C0" FieldName="rev_24mos" 
				ShowInColumn="rev_24mos" SummaryType="Sum" />
            <dx:ASPxSummaryItem DisplayFormat="c0" FieldName="open_wos" 
				ShowInColumn="Open WOs" SummaryType="Sum" />
		</TotalSummary>
		<Columns>
			<dx:GridViewCommandColumn Visible="False" VisibleIndex="0" ShowClearFilterButton="true" >
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Name" FieldName="name" VisibleIndex="1">
				<Settings AutoFilterCondition="Contains" />
			<DataItemTemplate>
					<a href="javascript:void(-1)" onclick="opencustomer(<%# Eval("customer_id") %>)">
								<%#Container.Text %>
							</a>
						</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="AR Current" FieldName="thirty" 
				VisibleIndex="4">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="AR Over 30 Days" FieldName="sixty" 
				VisibleIndex="5">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="AR Over 60 Days" FieldName="ninety" 
				VisibleIndex="6">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="AR Over 90 Days" FieldName="onetwenty" 
				VisibleIndex="7">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="AR Over 120 Days" FieldName="onetwentyplus" 
				VisibleIndex="8">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Total AR" FieldName="ebalance" 
				VisibleIndex="9">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<Settings AutoFilterCondition="GreaterOrEqual" />
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Open WOs" FieldName="open_wos" 
				Visible="False" VisibleIndex="11">
                <PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Credit Limit" FieldName="credit_limit" 
				VisibleIndex="10">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<Settings AutoFilterCondition="Equals" />
			</dx:GridViewDataTextColumn>
<dx:GridViewDataTextColumn FieldName="callcycle" Caption="Call Cycle" VisibleIndex="3">
	<Settings AutoFilterCondition="Equals" />
	<DataItemTemplate>
		<dx:ASPxTextBox ID="ASPxTextBox1" runat="server" oninit="ASPxTextBox1_Init" 
			Text='<%# Eval("callcycle") %>' Width="50px">
		</dx:ASPxTextBox>
	</DataItemTemplate>
<CellStyle HorizontalAlign="Center"></CellStyle>
</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Last Action" FieldName="lastaction" 
				VisibleIndex="12">
				<Settings HeaderFilterMode="CheckedList" />
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="hl_lastaction" runat="server" NavigateUrl='<%# string.Format("javascript:show_lastaction_popup({0}, {1})", Eval("customer_id"), Eval("address_id")) %>' Text='<%# Eval("lastaction") %>' >
					</dx:ASPxHyperLink>
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Countdown" FieldName="countdown" 
				VisibleIndex="14">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="id" FieldName="custid" Visible="False" 
				VisibleIndex="13">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="2" 
				Width="75px">
				<Settings HeaderFilterMode="CheckedList" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="YTD Sales" FieldName="ytd_rev" 
				VisibleIndex="15">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
		    <dx:GridViewDataTextColumn Caption="$ 12 Mos" FieldName="rev_12mos" VisibleIndex="16">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="$ 24 Mos" FieldName="rev_24mos" VisibleIndex="17">
                <PropertiesTextEdit DisplayFormatString="C2">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager PageSize="20">
		</SettingsPager>
		<Settings ShowFooter="True" ShowFilterBar="Visible" ShowFilterRow="True" 
			ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
		<Styles>
			<Footer HorizontalAlign="Center">
			</Footer>
		</Styles>
	</dx:ASPxGridView>

							
						
   
	<dx:ASPxPopupControl ID="pop_lastaction" runat="server" ClientInstanceName="pop_lastaction" CloseAction="CloseButton" HeaderText="New Last Action Event" Height="200px" Modal="True" PopupAnimationType="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="450px" AllowDragging="True" AutoUpdatePosition="True" onwindowcallback="pop_lastaction_WindowCallback"  ShowShadow="False">
		<ClientSideEvents Shown="function(s, e) {
	window.parent.resizeIframe(window.parent.document.getElementsByClassName('i_customers')[0]);
}" Closing="function(s, e) {
	window.parent.resizeIframe(window.parent.document.getElementsByClassName('i_customers')[0]);
}" />
		<ContentStyle HorizontalAlign="Center">
		</ContentStyle>
		<ModalBackgroundStyle BackColor="Transparent">
		</ModalBackgroundStyle>
		<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">

	<uc:customer_lastaction ID="uc_customer_lastaction" runat="server" />
	<script type="text/javascript">

	</script>
</dx:PopupControlContentControl>
</ContentCollection>
	</dx:ASPxPopupControl>

							
						
   
</asp:Content>

