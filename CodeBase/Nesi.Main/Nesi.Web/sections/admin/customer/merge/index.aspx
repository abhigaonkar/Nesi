<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_admin_customer_merge_index" MasterPageFile="~/IntraDefault.master" EnableEventValidation="False" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content ContentPlaceHolderID="header_placeholder" runat="server">
	<style type="text/css">
		.dxgvDataRow:nth-child(even)
			{
			background-color:			#ddd;
			}
		.dxgvDataRow:hover
			{
			background-color:			#9ef;
			cursor:						pointer;
			}
	</style>
</asp:Content>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				<div id='divSide' runat='server'>
					<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
				</div>
</asp:Content>
<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphMasterBody" runat="server">
    <div>
    <asp:Label ID="Lbl1" runat="server" Font-Bold="True">Duplicate Phone Numbers</asp:Label>



		<dx:ASPxGridView ID="gv_duplicatephones" runat="server" AutoGenerateColumns="False" KeyFieldName="ACID" DataSourceID="ds_phone" OnCustomJSProperties="gv_duplicatephones_CustomJSProperties" ClientInstanceName="gv_duplicatephones" Width="100%">
			<GroupSummary>
				<dx:ASPxSummaryItem FieldName="PhoneA" ShowInColumn="1st Customer Phone" ShowInGroupFooterColumn="1st Customer Phone" SummaryType="Count" />
			</GroupSummary>
			<Columns>
				<dx:GridViewDataTextColumn FieldName="AID" VisibleIndex="1" Visible="false">
					<Settings AutoFilterCondition="Contains" />
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="BID" VisibleIndex="2" Visible="false">
					<Settings AutoFilterCondition="Contains" />
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="PhoneA" VisibleIndex="3" Caption="1st Customer Phone">
					<Settings AutoFilterCondition="Contains" />
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="PhoneB" VisibleIndex="4" Caption="2nd Customer Phone">
					<Settings AutoFilterCondition="Contains" />
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="ANum" VisibleIndex="5" Caption="1st Customer Number">
					<Settings AutoFilterCondition="Contains" />
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="BNum" VisibleIndex="6" Caption="2nd Customer Number">
					<Settings AutoFilterCondition="Contains" />
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="BCID" VisibleIndex="7" Visible="false">
					<Settings AutoFilterCondition="Contains" />
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="ANAME" VisibleIndex="8" Caption="1st Customer Name">
					<Settings AutoFilterCondition="Contains" />
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn FieldName="BNAME" VisibleIndex="9" Caption="2nd Customer Name">
					<Settings AutoFilterCondition="Contains" />
				</dx:GridViewDataTextColumn>
			</Columns>
			<SettingsPager PageSize="50">
			</SettingsPager>
			<Settings ShowFilterRow="True" ShowGroupPanel="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
			<ClientSideEvents RowClick="function(s, e) 
{
var id		= gv_duplicatephones.cpids[e.visibleIndex];
boing(&quot;./cust_info.aspx?custid=&quot;+id, &quot;Merge&quot;, 800, 600);
}" />
			<Styles>
				<Header HorizontalAlign="Center">
				</Header>
			</Styles>
		</dx:ASPxGridView>



        <asp:SqlDataSource ID="ds_phone" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
            ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
            SelectCommand="
SELECT 
	A.address_id AS AID, 
	b.address_id AS BID,
	CONCAT(A.address_phonefull,A.address_type) AS PhoneA,
	CONCAT(B.address_phonefull,B.address_type) AS PhoneB,
	C.customer_number AS ANum, 
	D.customer_number AS BNum,
	C.customer_ID AS ACID,
	D.customer_ID AS BCID,
	C.customer_Name AS ANAME, 
	D.customer_Name AS BNAME
FROM 
	address as A, 
	address as B, 
	customer as C, 
	customer as D
WHERE 
	A.address_type = b.address_type AND
	A.address_phonefull = B.address_phonefull AND 
	A.address_phonefull != '' AND 
	B.address_phonefull != '' AND 
	A.address_phonefull != '000 000 0000' AND 
	B.address_phonefull != '000 000 0000' AND 
	A.address_phonefull != '905 827 2555' AND 
	B.address_phonefull != '905 827 2555' AND 
	A.address_phonefull != '905 689 1845' AND 
	B.address_phonefull != '905 689 1845' AND 
	A.address_phonefull != '800 204 4153' AND 
	B.address_phonefull != '800 204 4153' AND 
	A.address_phonefull != '905 827 4255' AND 
	B.address_phonefull != '905 827 4255' AND 
	A.address_id != b.address_id AND 
	A.Address_Table_ID = C.customer_ID AND 
	B.Address_Table_ID = D.customer_ID AND 
	A.Address_Table = 'customer' AND 
	B.Address_Table = 'customer'
GROUP BY 
	A.address_id
ORDER BY 
	C.customer_Name" EnableCaching="True" CacheKeyDependency="ds_depend">
        </asp:SqlDataSource>
        <br />
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Visible="True">Duplicate Names</asp:Label>
        <dx:ASPxGridView ID="gv_duplicatenames" KeyFieldName="customer_id" runat="server" AutoGenerateColumns="False" DataSourceID="ds_name" oncustomjsproperties="gv_duplicatenames_CustomJSProperties" ClientInstanceName="gv_duplicatenames" Width="100%">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="customer_id" ReadOnly="True" VisibleIndex="0" Caption="ID" Visible="False">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="customer_name" VisibleIndex="1" Caption="1st Customer Name">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="customer_number" VisibleIndex="2" Caption="1st Customer Number">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="customer_name1" VisibleIndex="3" Caption="2nd Customer Name">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="customer_number1" VisibleIndex="4" Caption="2nd Customer Number">
                </dx:GridViewDataTextColumn>
            </Columns>
        	<SettingsPager PageSize="50">
			</SettingsPager>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
			<ClientSideEvents RowClick="function(s, e) 
{
var id		= gv_duplicatenames.cpids[e.visibleIndex];
boing(&quot;./cust_info.aspx?namecustid=&quot;+id, &quot;Merge&quot;, 800, 600);
}" />
            <Styles>
				<Header HorizontalAlign="Center">
				</Header>
			</Styles>
        </dx:ASPxGridView>
        <asp:SqlDataSource ID="ds_name" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
            ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
            SelectCommand="call report_qc_customername(0, 0);" EnableCaching="True" CacheKeyDependency="ds_depend">
        </asp:SqlDataSource>
        <br />
        <dx:ASPxLoadingPanel id="ASPxLoadingPanel1" runat="server" ClientInstanceName="ASPxLoadingPanel1"
            Modal="True">
        </dx:ASPxLoadingPanel>
    
    </div>
</asp:Content>