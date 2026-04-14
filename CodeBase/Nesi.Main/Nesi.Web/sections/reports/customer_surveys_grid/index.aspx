<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="customer_surveys_grid" Title="Customer Surveys" EnableTheming="true" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>






<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript" language="javascript">
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 100) + 'px';
	
 }</script>
	<table style="width:100%;">
		<tr>
			<td>
				<lc:LayoutControl ID="layout" runat="server" GridviewID="ASPxGridView1" 
					ShowPDFExport="True" ShowExcelExport="True" ShowToggle="True" Visible="True" __is_private="True" />
			</td>
		</tr>
		<tr>
			<td>
				<dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" Theme="NETheme01"
					KeyFieldName="id" Width="100%" ClientInstanceName="ASPxGridView1" Font-Names="Arial" 
					oncustomcallback="ASPxGridView1_CustomCallback" 
					oncustomjsproperties="ASPxGridView1_CustomJSProperties">
					<TotalSummary>
						<dx:ASPxSummaryItem DisplayFormat="N1" FieldName="rating" ShowInColumn="Rating" 
							ShowInGroupFooterColumn="rating" SummaryType="Average" />
					</TotalSummary>
					<Columns>
						<dx:GridViewCommandColumn Visible="False" VisibleIndex="0" ShowClearFilterButton="true">
							
						</dx:GridViewCommandColumn>
						<dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" 
							VisibleIndex="1" MinWidth="30">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataDateColumn Caption="Date" FieldName="date" VisibleIndex="2" 
							Width="100px" MinWidth="30">
							<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
								EditFormatString="yyyy-MM-dd">
							</PropertiesDateEdit>
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewDataDateColumn>
						<dx:GridViewDataTextColumn Caption="Work Order" FieldName="bvwo" 
							VisibleIndex="3" Width="80px" MinWidth="30">
						    <DataItemTemplate>
						        <dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" 
						                          NavigateUrl="<%# string.Format(&quot;javascript:boing('/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}', 'workorder', 1200,800)&quot;, Eval(&quot;WOProg_ID&quot;), Eval(&quot;business_unit_id&quot;)) %>" 
						                          Text='<%# Eval("bvwo") %>'>
						        </dx:ASPxHyperLink>
						    </DataItemTemplate>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Description" FieldName="woprog_description" 
							VisibleIndex="4" Width="50%" MinWidth="200" CellStyle-Wrap="True">
<CellStyle Wrap="True"></CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Customer" FieldName="cust_name" 
							VisibleIndex="5" Width="100px" MinWidth="100">
							<CellStyle Wrap="False">
							</CellStyle>
						    <DataItemTemplate>
						        <dx:ASPxHyperLink ID="hl_customer" runat="server"  
                                    NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}','customer',1035,800);&quot;,Eval(&quot;customer_id&quot;)) %>" 
                                   
                                    Text='<%# Eval("cust_name").ToString() %>' 
                                    >
						            
						        </dx:ASPxHyperLink>
						    </DataItemTemplate>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Contact" FieldName="name" VisibleIndex="6" 
							Width="100px" MinWidth="30">
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Rating" VisibleIndex="7" 
							Width="75px" MinWidth="75">
							<DataItemTemplate>
								<dx:ASPxRatingControl ID="rc" runat="server" EnableTheming="True" Value='<%# GetRatingValue(Eval("rating")) %>' Enabled="False">
									</dx:ASPxRatingControl>
							</DataItemTemplate>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Notes" FieldName="notes" VisibleIndex="8" 
							Width="50%" MinWidth="200" CellStyle-Wrap="True">
<CellStyle Wrap="True"></CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Area Clean?" FieldName="clean" 
							VisibleIndex="9" Width="60px" MinWidth="30">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Completed On Time?" FieldName="ontime" 
							VisibleIndex="10" Width="80px" MinWidth="30">
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Requested Contact?" FieldName="callme" 
							VisibleIndex="11" Width="80px" MinWidth="30">
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewDataTextColumn>
					    <dx:GridViewDataTextColumn Caption="woprog_id" FieldName="woprog_id" 
					                               VisibleIndex="12" Visible="False">
					    </dx:GridViewDataTextColumn>
					    <dx:GridViewDataTextColumn Caption="business_unit_id" FieldName="business_unit_id" 
					                               VisibleIndex="13" Visible="False">
					    </dx:GridViewDataTextColumn>
					    <dx:GridViewDataTextColumn Caption="customer_id" FieldName="customer_id" 
					                               VisibleIndex="14" Visible="False">
					    </dx:GridViewDataTextColumn>
					</Columns>
					<SettingsBehavior ColumnResizeMode="Control" />
					<SettingsPager PageSize="50">
					</SettingsPager>
					<Settings ShowFilterRow="True" ShowFilterBar="Auto" ShowFilterRowMenu="True" 
						ShowHeaderFilterButton="True" ShowFooter="True" />
					<Styles>
						<Header Wrap="True">
						</Header>
					</Styles>
				</dx:ASPxGridView>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
		</tr>
	</table>
	</asp:Content>

