<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="advance_pay_reports_index" Title="Advance Pay Report" Codebehind="index.aspx.cs" %>


<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>


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

    <asp:SqlDataSource ID="sql_main" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
woprog.business_unit_id AS ```Parent Branch```,
woprog.woprog_id AS `Job Cost WO`,
woprog.woprog_customername AS Customer,
woprog.woprog_department AS `'Main WO Dept'`,
        woprog.woprog_description description,
left(woprog.woprog_quoteid,LENGTH( trim(BOTH '' from woprog.woprog_quoteid))-1) AS q_id,
right(woprog.woprog_quoteid,1) AS rev,
round(Get_QuoteWorksheet_Cost(if(left(woprog.woprog_quoteid,LENGTH( trim(BOTH '' from woprog.woprog_quoteid))-1)='',0,left(woprog.woprog_quoteid,LENGTH( trim(BOTH '' from woprog.woprog_quoteid))-1))
,right(woprog.woprog_quoteid,1)),2) AS Quoted_Costs,
woprog.WOProg_LaborCost AS Main_Labour_Cost,
woprog.WOProg_MaterialCost AS Main_Material_Cost,
wo_child.woprog_company_id AS `'Div or Child Branch'`,
wo_child.woprog_customername AS `'Div or Child Customer Name'`,
wo_child.woprog_invoicedate `Invoice date`,
if(left(wo_child.woprog_customername,3)='DIV',wo_child.woprog_id,'') AS `Div to Div WO`,
if(left(wo_child.woprog_customername,3)='DIV',wo_child.WOProg_Status,'') AS `Div to Div WO Status`,
if(left(wo_child.woprog_customername,3)='DIV',wo_child.this_wo_labour_bench,0) AS `Div to Div TM Labour Sell`,
if(left(wo_child.woprog_customername,3)='DIV',wo_child.this_wo_material_bench,'') AS `Div to Div TM Material Sell`,
wo_child.woprog_glposting_instructions AS `Posting Notes`,
if(left(wo_child.woprog_customername,3)!='DIV',wo_child.woprog_id,'') AS `Child WO`,
if(left(wo_child.woprog_customername,3)!='DIV',wo_child.WOProg_Status,'') AS `Child WO Status`,
if(left(wo_child.woprog_customername,3)!='DIV' AND woprog.woprog_quoteid = '0',wo_child.this_wo_labour_bench,0) AS `Child TM Labour Sell`,
if(left(wo_child.woprog_customername,3)!='DIV' AND woprog.woprog_quoteid = '0',wo_child.this_wo_material_bench,0) AS `Child TM Material Sell`,
wo_child.woprog_department AS ```Div or Child WO Dept```,
woprog.woprog_status as main_wo_status,
woprog.WOProg_QuotedAmount quoted_price,
        (Select sum(a.WOProg_InvoicedNetTotal) from woprog a where a.WOProg_Associate_WOProg_ID = woprog.woprog_id) progress_billed_so_far,
woprog.woprog_expected_startdate startdate,
woprog.woprog_expected_enddate enddate,
        woprog.WOProg_InvoiceDate,
        woprog.WOProg_QuotedAmount-(Select sum(a.WOProg_InvoicedNetTotal) from woprog a where a.WOProg_Associate_WOProg_ID = woprog.woprog_id) final_invoiced

FROM
woprog
left JOIN woprog AS wo_child ON woprog.woprog_id = wo_child.parent_woprog_id

WHERE
(wo_child.WOProg_InvoiceDate >= '2017-01-01') 
or 
((select count(b.woprog_id) from woprog b where b.WOProg_Associate_WOProg_ID=woprog.woprog_id and b.WOProg_Associate_WOProg_ID!=0)>0  )
or (woprog.WOProg_InvoiceDate>='2017-01-01' and woprog.woprog_quoteid>0)

ORDER BY
woprog.woprog_bvwo ASC"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_branches" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="select id, ddl_name from business_unit"></asp:SqlDataSource>
    <asp:HiddenField ID="hdn_cid" runat="server" />
    <uc1:layout_control ID="layout" runat="server" />
    <dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv" DataSourceID="sql_main" KeyFieldName="Job Cost WO" OnCustomCallback="gv_CustomCallback" OnCustomJSProperties="gv_CustomJSProperties" Theme="NETheme01" Width="100%">
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="Quoted_Costs" ShowInColumn="Quoted_Costs" SummaryType="Sum" DisplayFormat="{0:n2}" />
            <dx:ASPxSummaryItem FieldName="Main_Labour_Cost" ShowInColumn="Main_Labour_Cost" SummaryType="Sum" DisplayFormat="{0:n2}" />
            <dx:ASPxSummaryItem FieldName="Main_Material_Cost" ShowInColumn="Main_Material_Cost" SummaryType="Sum" DisplayFormat="{0:n2}" />
            <dx:ASPxSummaryItem FieldName="Div to Div TM Labour Sell" ShowInColumn="Div to Div TM Labour Sell" SummaryType="Sum" DisplayFormat="{0:n2}" />
            <dx:ASPxSummaryItem FieldName="Div to Div TM Material Sell" ShowInColumn="Div to Div TM Material Sell" SummaryType="Sum" DisplayFormat="{0:n2}" />
            <dx:ASPxSummaryItem FieldName="Child TM Labour Sell" ShowInColumn="Child TM Labour Sell" SummaryType="Sum" DisplayFormat="{0:n2}" />
            <dx:ASPxSummaryItem FieldName="Child TM Material Sell" ShowInColumn="Child TM Material Sell" SummaryType="Sum" DisplayFormat="{0:n2}" />
            

        </TotalSummary>
        <Columns>
            <dx:GridViewCommandColumn Caption=" " ShowClearFilterButton="True" VisibleIndex="0">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="Job Cost WO" ReadOnly="True" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Customer" VisibleIndex="3">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Quote" FieldName="q_id" VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="rev" Visible="False" VisibleIndex="6">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Quoted_Costs" VisibleIndex="7">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Main_Labour_Cost" VisibleIndex="8">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Main_Material_Cost" VisibleIndex="9">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Div or Child Customer Name" FieldName="'Div or Child Customer Name'" VisibleIndex="12">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="Invoice date" VisibleIndex="13">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Div to Div WO" VisibleIndex="14">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Div to Div WO Status" VisibleIndex="15">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Div to Div TM Labour Sell" VisibleIndex="16">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Div to Div TM Material Sell" VisibleIndex="17">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Posting Notes" VisibleIndex="18">
                <CellStyle Wrap="True">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Child WO" VisibleIndex="19">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Child WO Status" VisibleIndex="20">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Child TM Labour Sell" VisibleIndex="21">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Child TM Material Sell" VisibleIndex="22">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn Caption="Parent Business Unit" FieldName="`Parent Branch`" VisibleIndex="1">
                <PropertiesComboBox DataSourceID="sql_branches" TextField="ddl_name" ValueField="id" ValueType="System.Int32">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Div or Child Business Unit" FieldName="'Div or Child Branch'" VisibleIndex="10">
                <PropertiesComboBox DataSourceID="sql_branches" TextField="ddl_name" ValueField="id" ValueType="System.Int32">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataTextColumn Caption="Main WO Status" FieldName="main_wo_status" VisibleIndex="23">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Quoted Price" FieldName="quoted_price" VisibleIndex="24">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Progress Billed So Far" FieldName="progress_billed_so_far" VisibleIndex="25">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Est Start Date" FieldName="startdate" VisibleIndex="26">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn Caption="Est End Date" FieldName="enddate" VisibleIndex="27">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn Caption="Invoice Date" FieldName="WOProg_InvoiceDate" VisibleIndex="28">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="WO Description" FieldName="description" VisibleIndex="29">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Final Invoice Amt" FieldName="final_invoiced" VisibleIndex="30">
            </dx:GridViewDataTextColumn>
            
        </Columns>
        <SettingsPager Mode="ShowAllRecords">
        </SettingsPager>
        <Settings ColumnMinWidth="20" ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" ShowHeaderFilterButton="True" />
        <SettingsSearchPanel Visible="True" />
    </dx:ASPxGridView>

</asp:Content>

