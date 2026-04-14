<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" EnableTheming = "True"  Theme="NETheme01" Inherits="nesi_conversion" Title="Nesi Conversion" Codebehind="index.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
                <div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>
    <style type="text/css">
	    .gv_override {
            background-color:transparent !important;
            
	    }
    </style>

    <asp:SqlDataSource ID="sql_corps" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
nesi_conversion_table.id,
nesi_conversion_table.page_id,
nesi_conversion_table.stuff_to_fix,
nesi_conversion_table.member_id,
nesi_conversion_table.priority
FROM
nesi_conversion_table" 
        InsertCommand="insert into nesi_conversion_table (page_id,member_id,stuff_to_fix) values(@page_id,@member_id,@stuff_to_fix)" 
        UpdateCommand="update nesi_conversion_table set stuff_to_fix=@stuff_to_fix,member_id=@member_id,priority=@priority where id = @id limit 1"
        DeleteCommand="delete from nesi_conversion_table where id=@id"
        >
        <InsertParameters>
            <asp:Parameter Name="page_id" />
            <asp:Parameter Name="member_id" />
            <asp:Parameter Name="stuff_to_fix" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="stuff_to_fix" />
            <asp:Parameter Name="member_id" />
            <asp:Parameter Name="priority" />
            <asp:Parameter Name="id" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="id" />
        </DeleteParameters>
    </asp:SqlDataSource>

       <asp:SqlDataSource ID="Sqlmember" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
        SelectCommand="select
member.member_id id,
 member.member_fullname name 
from member where member_status='Active' ">


    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_page" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
        SelectCommand="select
page.page_id id,
 page.page_name name 
from page ">


    </asp:SqlDataSource>
    <uc1:layout_control ID="layout_control" runat="server" />
    <dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv" DataSourceID="sql_corps" KeyFieldName="id" Width="100%" SettingsBehavior-ColumnResizeMode="Control">
        <Columns>
            <dx:GridViewCommandColumn Caption=" " ShowClearFilterButton="True" ShowDeleteButton="True" ShowNewButtonInHeader="True" VisibleIndex="0" Width="60px">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" VisibleIndex="1" Caption="ID" Visible="False">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataComboBoxColumn Caption="Page" FieldName="page_id" VisibleIndex="2" Width="150px">
                <PropertiesComboBox DataSourceID="sql_page" TextField="name" ValueField="id" ValueType="System.Int32">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Delegated to" FieldName="member_id" VisibleIndex="4" Width="100px">
                <PropertiesComboBox DataSourceID="Sqlmember" TextField="name" ValueField="id">
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataMemoColumn Caption="Stuff to Fix" FieldName="stuff_to_fix" VisibleIndex="3" Width="100%">
            </dx:GridViewDataMemoColumn>
            <dx:GridViewDataSpinEditColumn Caption="Priority" FieldName="priority" VisibleIndex="5" Width="100px" CellStyle-Wrap="False" HeaderStyle-Wrap="False">
                <PropertiesSpinEdit DisplayFormatString="" MaxValue="10" NumberFormat="Custom" NumberType="Integer" Style-Wrap="False" Height="20px">
                    <ValidationSettings Display="None" ErrorDisplayMode="None" ErrorTextPosition="Bottom">
                    </ValidationSettings>
<Style Wrap="False"></Style>
                </PropertiesSpinEdit>

<HeaderStyle Wrap="False"></HeaderStyle>

<CellStyle Wrap="False"></CellStyle>
            </dx:GridViewDataSpinEditColumn>
        
        </Columns>
        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
        <SettingsPager Mode="ShowAllRecords">
        </SettingsPager>
        <SettingsEditing Mode="Batch">
        </SettingsEditing>
        <Settings ColumnMinWidth="20" ShowFilterBar="Visible" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowFilterRow="True" />
        <SettingsCommandButton>
            <NewButton>
                <Image Url="~/images/icon/icon[add].gif">
                </Image>
            </NewButton>
            <UpdateButton ButtonType="Link" Text="Save">
            </UpdateButton>
            <CancelButton ButtonType="Link" Text="Cancel">
            </CancelButton>
            <EditButton>
                <Image Url="~/images/icon/icon[edit].gif">
                </Image>
            </EditButton>
            <DeleteButton>
                <Image Url="~/images/icon/icon[delete].gif">
                </Image>
            </DeleteButton>
            
        </SettingsCommandButton>
        <SettingsSearchPanel Visible="True" />
        <Styles>
            <Cell VerticalAlign="Top">
            </Cell>
        </Styles>
    </dx:ASPxGridView>
	
</asp:Content>
