<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="Emailed_Account_Statements_index" Title="Emailed Account Statements" EnableTheming ="true" Codebehind="index.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"	Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>



<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<script type="text/javascript">
		
	</script>
	
            &nbsp;<table style="width:100%;">
		<tr>
			<td>
	<lc:LayoutControl ID="layout" runat="server" is_private="True" GridviewID="gv" /> 
	
            </td>
		</tr>
		<tr>
			<td>
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select emaillog.*, customer.customer_id, customer.customer_name,
DATEDIFF(curdate(),emaillog_timestamp) days_since from emaillog left join customer on customer.customer_autostatement_address = TRIM(TRAILING ';' from emaillog_to) where emaillog_from=&quot;noreply@nesi.ca&quot; and emaillog.emaillog_subject like &quot;*Account Statement*&quot;"></asp:SqlDataSource>
				<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
					ClientInstanceName="gv" oncustomcallback="gv_CustomCallback" 
					oncustomjsproperties="gv_CustomJSProperties" Width="100%" Theme="NETheme01" DataSourceID="SqlDataSource1" KeyFieldName="emaillog_id">
					<Columns>
						<dx:GridViewCommandColumn VisibleIndex="0" Caption=" " Width="60px" ShowClearFilterButton="true">
							
						</dx:GridViewCommandColumn>
						<dx:GridViewDataTextColumn  Caption="Customer" FieldName="customer_name" 
							VisibleIndex="1" Width="20%">
							<CellStyle Wrap="False">
							</CellStyle>
							<EditFormSettings Visible="False" />
							<DataItemTemplate>
					<asp:HyperLink ID="hl_customer" runat="server" Font-Bold="True" 
					NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/customer/index.aspx?customer_id={0}','Customer',1035,800);&quot;,Eval(&quot;customer_id&quot;)) %>"
						Text='<%# HttpUtility.UrlDecode(Eval("customer_name").ToString()) %>' Target="_blank" Font-Size="11px" Width="100%"></asp:HyperLink>
			</DataItemTemplate>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Emailed To" FieldName="emaillog_to" 
							VisibleIndex="2" Width="40%">
							<EditFormSettings Visible="False" />
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						
						<dx:GridViewDataDateColumn Caption="Date" FieldName="emaillog_timestamp" VisibleIndex="3" 
							Width="200px">
							<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss">
							</PropertiesDateEdit>
							<EditFormSettings Visible="False" />
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewDataDateColumn>
						<dx:GridViewDataTextColumn Visible="False" Caption="customer_id" FieldName="customer_id" 
							VisibleIndex="4" Width="300px">
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewDataTextColumn>
					    <dx:GridViewDataTextColumn Caption="Statement" FieldName="emaillog_body" VisibleIndex="5" Width="100%" Visible="False">
                            <EditFormSettings Visible="True" />
                        </dx:GridViewDataTextColumn>
					    <dx:GridViewDataTextColumn Caption="id" FieldName="emaillog_id" Visible="False" VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
					    <dx:GridViewDataTextColumn Caption="Days Since" FieldName="days_since" VisibleIndex="7" Width="70px">
                        </dx:GridViewDataTextColumn>
					</Columns>
					<SettingsBehavior ColumnResizeMode="Control" />
					<SettingsPager PageSize="100">
					</SettingsPager>
					<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" 
						ShowHeaderFilterButton="True" ShowFilterBar="Visible" ShowFooter="True" />
						<SettingsText CommandEdit="View" />
                    <SettingsDetail ShowDetailRow="True" />
						<Templates>
						    <DetailRow>
                                <dx:ASPxHtmlEditor ID="ASPxHtmlEditor1" runat="server" ClientEnabled="False" Height="400px" Html='<%# Eval("emaillog_body").ToString() %>' ToolbarMode="None" Width="100%">
                                </dx:ASPxHtmlEditor>
                            </DetailRow>
						<FooterRow>
						<table style="width:100%;">
							<tr>
								<td>
									<dx:ASPxLabel ID="lblcount" runat="server" ClientInstanceName="lblcount">
										<ClientSideEvents Init="function(s, e) {
	s.SetText(gv.GetVisibleRowsOnPage());
}" />
									</dx:ASPxLabel>
								</td>
								<td>
									&nbsp;</td>
								<td>
									&nbsp;</td>
							</tr>
						</table>
					</FooterRow>
					</Templates>
				</dx:ASPxGridView>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;</td>
		</tr>
	</table>
	&nbsp; 
			</asp:Content>




