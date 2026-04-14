<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_phoneComments" Codebehind="phoneComments.ascx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/modules/layout_control.ascx" TagPrefix="uc1" TagName="layout_control" %>

<style>
    .statusBar a:first-child {
        display: none;
    }
</style>

<uc1:layout_control runat="server" ID="layout" />
<br />

<dx:ASPxGridView ID="gv_phoneComments" runat="server" Font-Names="Arial"
    Font-Size="9pt" Theme="NETheme01"
    ClientInstanceName="gv_phoneComments"
    OnCustomCallback="gv_phoneComments_CustomCallback"
    OnCustomJSProperties="gv_phoneComments_CustomJSProperties"
    DataSourceID="SqlDataSource1" AutoGenerateColumns="False" KeyFieldName="phone_log_id" OnCellEditorInitialize="gv_phoneComments_CellEditorInitialize">
    <SettingsText PopupEditFormCaption="Invoices" />




    <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True"
        ShowGroupPanel="True" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded"
        ShowPreview="True" ShowFilterBar="Visible" ShowGroupedColumns="True"></Settings>

    <SettingsText PopupEditFormCaption="Invoices"></SettingsText>


    <Columns>
        <dx:GridViewDataDateColumn FieldName="phone_log_date" VisibleIndex="1" Width="100px" Caption="Date" ReadOnly="True">
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataTextColumn FieldName="phone_log_from_number" VisibleIndex="2" Width="115px" Caption="From Number" ReadOnly="True" EditFormSettings-Visible="False">
            <EditFormSettings Visible="False"></EditFormSettings>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="phone_log_to_number" VisibleIndex="4" Width="115px" Caption="To Number" ReadOnly="True" EditFormSettings-Visible="False">
            <EditFormSettings Visible="False"></EditFormSettings>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="phone_log_from_name" VisibleIndex="3" Caption="From Name" Width="200px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="phone_log_to_name" VisibleIndex="5" Caption="To Name" Width="200px">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="phone_log_duration" VisibleIndex="6" Caption="Call Duration (Seconds)" Width="165px" ReadOnly="True" EditFormSettings-Visible="False">
            <EditFormSettings Visible="False"></EditFormSettings>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="phone_log_time" VisibleIndex="7" Visible="False" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="phone_log_direction" VisibleIndex="8" Visible="False" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="imei_ds" VisibleIndex="9" Visible="False" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="calltime_nb" VisibleIndex="10" Visible="False" ReadOnly="True">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="calltype_fg" VisibleIndex="11" ReadOnly="True" Visible="False">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="numbertype_fg" VisibleIndex="12" Visible="False" ReadOnly="true">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataMemoColumn Caption="Comments" FieldName="notes" VisibleIndex="13" Width="250px">
        </dx:GridViewDataMemoColumn>
    </Columns>
    <SettingsEditing Mode="Batch" EditFormColumnCount="0">
        <BatchEditSettings ShowConfirmOnLosingChanges="False" />
    </SettingsEditing>

    <Settings ShowFilterRow="True" ShowGroupPanel="True" ShowPreview="True"
        ShowFilterBar="Visible" ShowFilterRowMenu="True"
        ShowHeaderFilterButton="True" ShowFooter="True" ShowGroupedColumns="True"
        ShowGroupFooter="VisibleIfExpanded" />
    <SettingsBehavior AutoFilterRowInputDelay="5000" EnableRowHotTrack="True"
        ColumnResizeMode="Control" />
    <SettingsBehavior ProcessFocusedRowChangedOnServer="True"
        AutoFilterRowInputDelay="5000" EnableRowHotTrack="True"></SettingsBehavior>
    <SettingsPager NumericButtonCount="50" PageSize="30">
    </SettingsPager>
    <Styles>
        <StatusBar CssClass="statusBar">
        </StatusBar>
    </Styles>
</dx:ASPxGridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server"
    ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
    SelectCommand="SELECT * FROM neintranet.phone_log where phone_log_date >= curdate() - interval 8 month  order by phone_log_date desc"
    UpdateCommand="UPDATE phone_log SET phone_log_from_name =?phone_log_from_name, phone_log_to_name = ?phone_log_to_name, notes =?notes where phone_log_id = ?phone_log_id" OnLoad="Page_Load">
    <UpdateParameters>
        <asp:Parameter Name="phone_log_id" />
        <asp:Parameter Name="phone_log_from_name" />
        <asp:Parameter Name="phone_log_to_name" />
        <asp:Parameter Name="notes" />
    </UpdateParameters>
</asp:SqlDataSource>

