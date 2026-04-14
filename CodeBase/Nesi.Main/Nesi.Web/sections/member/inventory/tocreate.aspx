<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/IntraDefault.master" Inherits="sections_member_inventory_tocreate" Codebehind="tocreate.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
<div id="divSide" runat="server">
    <asp:SqlDataSource ID="Users" runat="server"></asp:SqlDataSource>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
<div id="divMenu" runat="server"></div></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<dx:ASPxGridView ID="gv_tocreate" runat="server" AutoGenerateColumns="False" DataSourceID="ds_tocreate" Width="100%" Theme="NETheme01">
		<Columns>
			<dx:GridViewCommandColumn VisibleIndex="0" ShowClearFilterButton="true" >
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Business Unit" FieldName="companyname" VisibleIndex="2" Width="150px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Added By" FieldName="added_by" VisibleIndex="3" Width="150px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="WO" FieldName="bvwo" VisibleIndex="4" Width="100px">
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" Text='<%# Eval("bvwo") %>' NavigateUrl='<%# string.Format("/wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}", Eval("woprog_id"), Eval("business_unit_id")) %>' Target="_blank" />
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Date Added" FieldName="date_added" VisibleIndex="1" Width="150px">
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="descrip" VisibleIndex="5">
				<CellStyle HorizontalAlign="Left">
				</CellStyle>
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
		<Styles>
			<Header HorizontalAlign="Center">
			</Header>
		</Styles>
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="ds_tocreate" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
wo_detail_current_date_added date_added,
business_unit.ddl_name companyname, 
business_unit_id,
member_name(wo_detail_current_added_by) added_by, 
wo_detail_current_woprog_id woprog_id, 
CAST(wo_detail_current_bvwo AS CHAR) bvwo,
wo_detail_current_description descrip
FROM 
wo_detail_current inner join business_unit on wo_detail_current.business_unit_id = business_unit.id WHERE wo_detail_current_master_id = 777 AND FIND_IN_SET(business_unit_id, @visible_bu_list) ;
">
		<SelectParameters>
		
			<asp:Parameter Name="@visible_bu_list" />
		
		</SelectParameters>
	</asp:SqlDataSource>
	<br />
</asp:Content>
