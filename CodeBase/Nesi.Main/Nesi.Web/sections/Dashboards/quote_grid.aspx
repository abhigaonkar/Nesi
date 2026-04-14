<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_dashboards_quote_grid" Codebehind="quote_grid.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register src="~/modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>


<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
<script type="text/javascript" language="javascript">
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 0) + 'px';
	
    }
    function opencustomer(id) {
        boing("/#/opens/10/customers/" + id, "customer", 1280, 960);
    }
  
 </script>
							<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select name,id from business_unit  where active = &#39;T&#39;" ID="SqlDataSource22"></asp:SqlDataSource>

							
						
   
	<dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
		Width="100%" KeyFieldName="q_id" 
		onhtmlrowprepared="ASPxGridView1_HtmlRowPrepared" 
		ClientInstanceName="ASPxGridView1" 
		oncustomcallback="ASPxGridView1_CustomCallback" Font-Names="Arial">
		<TotalSummary>
			<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="pipeline" 
				ShowInColumn="Amount Added To Pipeline" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="c0" FieldName="price" ShowInColumn="Amount" 
				SummaryType="Sum" />
		</TotalSummary>
		<Columns>
			<dx:GridViewCommandColumn Visible="False" VisibleIndex="0" ShowClearFilterButton="true" >
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Customer" FieldName="name" VisibleIndex="1" 
				Width="100px">
				<CellStyle Wrap="False">
				</CellStyle>
				<Settings AutoFilterCondition="Contains" />
				<DataItemTemplate>
							<a href="javascript:void(-1)" onclick="opencustomer(<%# Eval("customer_id") %>)">
								<%#Container.Text %>
							</a>
						</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="desc" 
				VisibleIndex="3" Width="100%">
				<Settings AutoFilterCondition="Contains" />
				<CellStyle HorizontalAlign="Left" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Amount" FieldName="price" 
				VisibleIndex="4" Width="100px">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<Settings AutoFilterCondition="GreaterOrEqual" />
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Status" FieldName="status" 
				VisibleIndex="5" Width="125px">
				<Settings HeaderFilterMode="CheckedList" />
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Expected Work Completion Date" FieldName="comp_date" 
				VisibleIndex="6" Width="120px">
				<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesTextEdit>
				<Settings AutoFilterCondition="LessOrEqual" />
				<DataItemTemplate>
					<dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" AnimationType="None" 
						DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd" 
						Font-Names="Arial" oninit="ASPxDateEdit1_Init" Value='<%# Eval("comp_date") %>' 
						Width="110px">
					</dx:ASPxDateEdit>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Expected Due Date" FieldName="date_due" 
				VisibleIndex="6" Width="120px">
				<PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
				</PropertiesTextEdit>
				<Settings AutoFilterCondition="LessOrEqual" />
				<DataItemTemplate>
					<dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" AnimationType="None" 
						DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd" 
						Font-Names="Arial" oninit="ASPxDateEdit2_Init" Value='<%# Eval("date_due") %>' 
						Width="110px">
					</dx:ASPxDateEdit>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Chance %" FieldName="chance" 
				VisibleIndex="7" Width="75px">
				<PropertiesTextEdit DisplayFormatString="p0">
				</PropertiesTextEdit>
				<Settings HeaderFilterMode="CheckedList" />
				<DataItemTemplate>
					<dx:ASPxSpinEdit ID="ASPxSpinEdit1" runat="server" DisplayFormatString="p0" 
						Font-Names="Arial" Height="19px" Increment="0.3" MaxValue="1" Number="0" 
						oninit="ASPxSpinEdit1_Init" Value='<%# Eval("chance") %>' Width="60px" />
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Amount Added To Pipeline" FieldName="pipeline" 
				VisibleIndex="9" Width="100px">
				<PropertiesTextEdit DisplayFormatString="c0">
				</PropertiesTextEdit>
				<Settings AutoFilterCondition="GreaterOrEqual" />
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Quote" FieldName="q_id" VisibleIndex="2" 
				Width="75px">
				<CellStyle HorizontalAlign="Center" Wrap="False">
				</CellStyle>
				<Settings AutoFilterCondition="Equals" />
				<DataItemTemplate>
					<asp:HyperLink  runat="server" Font-Bold="True"  NavigateUrl="<%# string.Format(&quot;javascript:boing('/#/opens/65/quotes/{0}/{1}','quote',1035,800);&quot;,Eval(&quot;q_id&quot;),Eval(&quot;rev&quot;)) %>" Text='<%# Eval("q_id").ToString() %>' Width="100%"></asp:HyperLink>
								
						</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Why Chance" FieldName="why_chance" 
				VisibleIndex="8" Width="200px">
				<Settings HeaderFilterMode="CheckedList" />
				<DataItemTemplate>
					<dx:ASPxComboBox ID="ASPxComboBox1" runat="server" 
						DataSourceID="SqlDataSource1" Font-Names="Arial" oninit="ASPxComboBox1_Init" 
						TextField="quote_chance_name" Value='<%# Eval("quote_chance_id") %>' 
						ValueField="quote_chance_id" ValueType="System.Int32">
					</dx:ASPxComboBox>
					<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
						ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
						ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
						SelectCommand="SELECT * FROM quote_chance"></asp:SqlDataSource>
				</DataItemTemplate>
				<CellStyle Wrap="False">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="quote_id" FieldName="quote_id" 
				Visible="False" VisibleIndex="11">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="rev" FieldName="rev" 
				Visible="False" VisibleIndex="11">
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="quote_chance_id" 
				FieldName="quote_chance_id" Visible="False" VisibleIndex="10">
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsBehavior ColumnResizeMode="Control" />
		<SettingsPager PageSize="20">
		</SettingsPager>
		<Settings ShowFooter="True" ShowFilterBar="Visible" ShowFilterRow="True" 
			ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
		<Styles>
			<Header Wrap="True">
			</Header>
			<Footer HorizontalAlign="Center">
			</Footer>
		</Styles>
	</dx:ASPxGridView>

							
						
   
</asp:Content>

