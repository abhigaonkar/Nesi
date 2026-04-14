<%@ Page Title="It Dashboard Settings" Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="sections_admin_dashboard_settings_index" EnableTheming="True" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">

    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
        ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
        SelectCommand="SELECT id, server_name, show_application, show_system, show_critical, show_warning, show_error, show_verbose, show_information, show_cpu FROM it.dashboard_eventviewer_servers"
        UpdateCommand="update it.dashboard_eventviewer_servers 
        set [server_name] = ?server_name,
        [show_application] = ?show_application,
        [show_system] = ?show_system ,
        [show_critical] = ?show_critical ,
        [show_verbose] = ?show_verbose ,
        [show_information] = ?show_information ,
        [show_warning] = ?show_warning ,
        [show_error] = ?show_error ,
        [show_cpu] = ?show_cpu
        where [id] = ?id"
        InsertCommand="INSERT INTO it.dashboard_eventviewer_servers
        ([server_name], [show_application],[show_system],[show_critical], [show_verbose], [show_information],[show_warning], [show_error],[show_cpu]) 
        VALUES (?server_name,
         ?show_application,
         ?show_system,
         ?show_critical,
         ?show_verbose,
         ?show_information,
         ?show_warning,
         ?show_error,
         ?show_cpu)"
        DeleteCommand="DELETE FROM it.dashboard_eventviewer_servers where [id] = ?id limit 1">

        <UpdateParameters>
            <asp:Parameter Name="id" />
            <asp:Parameter Name="server_name" />
            <asp:Parameter Name="show_application" />
            <asp:Parameter Name="show_system" />
            <asp:Parameter Name="enabled" />
            <asp:Parameter Name="show_critical" />
            <asp:Parameter Name="show_verbose" />
            <asp:Parameter Name="show_information" />
            <asp:Parameter Name="show_warning" />
            <asp:Parameter Name="show_error" />
            <asp:Parameter Name="show_cpu" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="id" />
            <asp:Parameter Name="server_name" />
            <asp:Parameter Name="show_application" />
            <asp:Parameter Name="show_system" />
            <asp:Parameter Name="enabled" />
            <asp:Parameter Name="show_critical" />
            <asp:Parameter Name="show_verbose" />
            <asp:Parameter Name="show_information" />
            <asp:Parameter Name="show_warning" />
            <asp:Parameter Name="show_error" />
            <asp:Parameter Name="show_cpu" />
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="id" />
        </DeleteParameters>

    </asp:SqlDataSource>

    <dx:ASPxGridView ID="gv_settings" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2"
        KeyFieldName="id" Theme="NETheme01"
        OnCustomButtonCallback="gv_settings_CustomButtonCallback" OnCustomCallback="gv_settings_CustomCallback">

        <Columns>
            <dx1:GridViewCommandColumn ShowDeleteButton="True" ShowNewButtonInHeader="True" VisibleIndex="0">
            </dx1:GridViewCommandColumn>
            <dx1:GridViewDataTextColumn FieldName="id" VisibleIndex="1" ReadOnly="True">
                <EditFormSettings Visible="False" />
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataTextColumn FieldName="server_name" VisibleIndex="2">
            </dx1:GridViewDataTextColumn>
            <dx1:GridViewDataCheckColumn FieldName="show_application" VisibleIndex="4">
            </dx1:GridViewDataCheckColumn>
            <dx1:GridViewDataCheckColumn FieldName="show_system" VisibleIndex="5">
            </dx1:GridViewDataCheckColumn>
            <dx1:GridViewDataCheckColumn FieldName="show_critical" VisibleIndex="6">
            </dx1:GridViewDataCheckColumn>
            <dx1:GridViewDataCheckColumn FieldName="show_warning" VisibleIndex="7">
            </dx1:GridViewDataCheckColumn>
            <dx1:GridViewDataCheckColumn FieldName="show_error" VisibleIndex="8">
            </dx1:GridViewDataCheckColumn>
            <dx1:GridViewDataCheckColumn FieldName="show_verbose" VisibleIndex="9">
            </dx1:GridViewDataCheckColumn>
            <dx1:GridViewDataCheckColumn FieldName="show_information" VisibleIndex="10">
            </dx1:GridViewDataCheckColumn>
            <dx1:GridViewDataCheckColumn FieldName="show_cpu" VisibleIndex="3">
            </dx1:GridViewDataCheckColumn>
        </Columns>
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
        </SettingsEditing>
    </dx:ASPxGridView>

</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="header_placeholder" runat="server">
    <link rel="Stylesheet" type="text/css" href="/css/customer.css" />
    <style type="text/css">
        .options {
            background: -moz-linear-gradient(top, rgba(204,204,204,0.65) 0%, rgba(204,204,204,0.64) 1%, rgba(0,0,0,0) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(204,204,204,0.65)), color-stop(1%,rgba(204,204,204,0.64)), color-stop(100%,rgba(0,0,0,0))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#a6cccccc', endColorstr='#00000000',GradientType=0 ); /* IE6-9 */
        }

        .fleft {
            float: left;
            display: block;
        }

            .fleft .table {
                background-color: #fff;
                border: solid 1px #999;
                margin: 5px;
                border-radius: 3px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
</asp:Content>
