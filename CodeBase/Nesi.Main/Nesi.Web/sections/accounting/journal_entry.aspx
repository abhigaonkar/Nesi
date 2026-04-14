<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="journal_entry" Title="Journal Entry" Codebehind="journal_entry.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>
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
     <asp:SqlDataSource ID="Sql_te" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" ></asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_gltrans" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
        SelectCommand="SELECT gltrans.gltrans_bv_trans_no AS trans_no, gltrans.gltrans_memo AS memo, gltrans.gltrans_debit AS total_debit, 
        gltrans.gltrans_credit AS total_credit, gltrans.gltrans_business_unit_id AS bu_id, gltrans.gltrans_post_date post_date, gltrans.gltrans_tran_date trans_date, 
        gl_te.tax_entity_id te_id, '' posted_by, gl_te.id gl_te_id
        FROM gl_te INNER JOIN gltrans ON gl_te.id = gltrans.gltrans_gl_id WHERE (gl_te.tax_entity_id = ?id) ORDER BY gltrans.gltrans_bv_trans_no DESC">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddl_te" Name="id" PropertyName="Value" />
        </SelectParameters>
     </asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_member" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select member_id,member_fullname from member order by member_fullname"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_bu" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id,ddl_name from business_unit where tax_entity_id = ?id">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddl_te" Name="id" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_gl_te" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id,concat(account_no,'  ',gl_chart_name) name from gl_te where tax_entity_id = ?te_id">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddl_te" Name="te_id" PropertyName="Value" />
    </SelectParameters></asp:SqlDataSource>
    <br />
    <dx:ASPxComboBox ID="ddl_te" runat="server" AutoPostBack="True" DataSourceID="Sql_te" OnSelectedIndexChanged="ddl_te_SelectedIndexChanged" TextField="ddl_name" ValueField="id" ValueType="System.Int32" Theme="NETheme01">
    </dx:ASPxComboBox>
    <br />
    

    <uc1:layout_control ID="layout" runat="server"  />
    <dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" Theme="NETheme01" Width="100%" DataSourceID="sql_gltrans" ClientInstanceName="gv" OnCustomCallback="gv_CustomCallback" OnCustomJSProperties="gv_CustomJSProperties">
        <TotalSummary>
            <dx:ASPxSummaryItem DisplayFormat="N2" FieldName="total_credit" ShowInColumn="Total Credit" ShowInGroupFooterColumn="Total Credit" SummaryType="Sum" ValueDisplayFormat="N2" />
            <dx:ASPxSummaryItem DisplayFormat="N2" FieldName="total_debit" ShowInColumn="Total Debit" ShowInGroupFooterColumn="Total Debit" SummaryType="Sum" ValueDisplayFormat="N2" />
        </TotalSummary>
        <Columns>
            <dx:GridViewCommandColumn Caption=" " VisibleIndex="0">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Trans No." FieldName="trans_no" VisibleIndex="2">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Memo" FieldName="memo" VisibleIndex="5">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total Debit" FieldName="total_debit" VisibleIndex="6">
                <PropertiesTextEdit DisplayFormatString="N2">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Total Credit" FieldName="total_credit" VisibleIndex="7">
                <PropertiesTextEdit DisplayFormatString="N2">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Trans Date" FieldName="trans_date" VisibleIndex="3">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataComboBoxColumn Caption="Tax Entity" FieldName="te_id" VisibleIndex="1" Visible="False">
                <PropertiesComboBox DataSourceID="sql_te" TextField="ddl_name" ValueField="id" >
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataDateColumn Caption="Post Date" FieldName="post_date" VisibleIndex="4">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd HH:mm:ss">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataComboBoxColumn Caption="Entry By" FieldName="posted_by" VisibleIndex="8" Settings-HeaderFilterMode="CheckedList">
                <PropertiesComboBox DataSourceID="sql_member" TextField="member_fullname" ValueField="member_id" >
                </PropertiesComboBox>

<Settings HeaderFilterMode="CheckedList"></Settings>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="bu_id" VisibleIndex="9" Settings-HeaderFilterMode="CheckedList">
                <PropertiesComboBox DataSourceID="sql_bu" TextField="ddl_name" ValueField="id" >
                </PropertiesComboBox>

<Settings HeaderFilterMode="CheckedList"></Settings>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Account" FieldName="gl_te_id" VisibleIndex="10" Settings-HeaderFilterMode="CheckedList">
                <PropertiesComboBox DataSourceID="sql_gl_te" TextField="name" ValueField="id">
                </PropertiesComboBox>

<Settings HeaderFilterMode="CheckedList"></Settings>
            </dx:GridViewDataComboBoxColumn>
        </Columns>
        <SettingsPager PageSize="50">
        </SettingsPager>
        <Settings ShowFooter="True" />
    </dx:ASPxGridView>
    

</asp:Content>

