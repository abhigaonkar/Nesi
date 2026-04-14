<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_reports_inventory_by_vendor_index"  MasterPageFile="~/IntraDefault.master" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.XtraReports.v19.2.Web.WebForms, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>







	<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'><div id='divSide' runat='server'></div></asp:Content>
<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'><div id='divMenu' runat='server'></div></asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphMasterBody">
    <script type="text/javascript">
		function calc_weeks()
			{
			var defs			=	{
									interval:	t_amount.GetValue(),
									type:		t_type.GetValue(),
									weeks:		0
									}
			var types			=	{0:0,1:4,2:12}
			cb_alt.PerformCallback("ibv_amount,"+defs.interval);
			cb_alt.PerformCallback("ibv_type,"+defs.type);
			cb_main.PerformCallback("ibv_weeks,"+defs.interval*types[defs.type]);
			}
	</script>
	<div align="left">&nbsp;<br />
		<table style="width:100%;">
			
			<tr>
				<td>
					<dx:ASPxComboBox ID="ddl_company" runat="server" style="text-align: left"   AutoPostBack="True" OnSelectedIndexChanged="ASPxComboBox1_SelectedIndexChanged" TextField="ddl_name" ValueField="id" ValueType="System.Int32">
                    </dx:ASPxComboBox>
                    <br />
                    <lc:LayoutControl runat="server" id="layout" />
					<dx:ASPxGridView ID="gv_history" runat="server" AutoGenerateColumns="False" 
						ClientInstanceName="gv_history" 
						oncustomcallback="gv_history_CustomCallback" 
						oncustomjsproperties="gv_counts_CustomJSProperties" Width="100%" Theme="NETheme01">
						<Columns>
							<dx:GridViewCommandColumn Caption=" " Visible="False" VisibleIndex="0"  ShowClearFilterButton="true"
								Width="1px">
								
							</dx:GridViewCommandColumn>
							<dx:GridViewDataDateColumn Caption="Date" FieldName="dt" VisibleIndex="1" 
								Width="200px">
								<PropertiesDateEdit DisplayFormatString="">
								</PropertiesDateEdit>
							</dx:GridViewDataDateColumn>
							<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="branch" VisibleIndex="2" 
								Width="100px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Master ID" FieldName="master_id" 
								VisibleIndex="3" Width="60px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
                                 <DataItemTemplate>
			<dx:ASPxHyperLink ID="hl_po" runat="server" NavigateUrl="<%# string.Format(&quot;javascript:boing('/sections/member/inventory/index.aspx?a=get&tab=G&id={0}', 'inventory', 1035,800)&quot;, Eval(&quot;master_id&quot;)) %>"
				Text='<%# Eval("master_id") %>'>
			</dx:ASPxHyperLink>
        </DataItemTemplate>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Location" FieldName="location" 
								VisibleIndex="4" Width="125px">
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Member" FieldName="member_name" 
								VisibleIndex="5" Width="100px">
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Qty Manually Adjusted" FieldName="is_manual" 
								VisibleIndex="6" Width="40px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Section" FieldName="section" 
								VisibleIndex="7" Width="100px">
								<CellStyle Wrap="False" horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
								VisibleIndex="8">
								<CellStyle Wrap="False">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Qty Before" FieldName="qty_before" 
								VisibleIndex="9" Width="60px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Qty After" FieldName="qty_after" 
								VisibleIndex="10" Width="50px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Qty Diff" FieldName="qty_diff" 
								VisibleIndex="11" Width="50px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="DB Before" FieldName="db_before" 
								VisibleIndex="12" Width="60px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="DB After" FieldName="db_after" 
								VisibleIndex="13" Width="50px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="DB Diff" FieldName="db_diff" 
								VisibleIndex="14" Width="50px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Cost" FieldName="cost_per" 
								VisibleIndex="15" Width="50px">
								<CellStyle horizontalalign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
						</Columns>
						<SettingsBehavior ColumnResizeMode="Control" />
						<SettingsPager NumericButtonCount="5" PageSize="100">
						</SettingsPager>
						<Settings ShowFilterRow="True" ShowGroupPanel="True" ShowFilterBar="Visible" 
							ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
					</dx:ASPxGridView>
				</td>
				<td>
					&nbsp;</td>
				<td>
					&nbsp;</td>
			</tr>
		
		</table>
		</div>
    
    &nbsp;&nbsp;<dx:ASPxGridViewExporter ID="ex_inv" runat="server" FileName="inv_report" GridViewID="gv_history">
	</dx:ASPxGridViewExporter>
    &nbsp;
</asp:Content>
