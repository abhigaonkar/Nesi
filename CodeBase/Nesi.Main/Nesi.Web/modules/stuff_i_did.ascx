<%@ Control Language="C#" AutoEventWireup="true" Inherits="modules_stuff_i_did" Codebehind="stuff_i_did.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<dx:ASPxGridView ID="gv_journal" runat="server" AutoGenerateColumns="False" Width="100%">
    <Columns>
        <dx:GridViewDataTextColumn Caption="id" FieldName="id" Visible="False" VisibleIndex="0">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTimeEditColumn Caption="Time" FieldName="dt" SortIndex="0" SortOrder="Ascending" VisibleIndex="1">
            <PropertiesTimeEdit DisplayFormatString="">
            </PropertiesTimeEdit>
        </dx:GridViewDataTimeEditColumn>
        <dx:GridViewDataTextColumn Caption="Action" FieldName="action" VisibleIndex="2">
        </dx:GridViewDataTextColumn>
    </Columns>
    <SettingsPager PageSize="25">
    </SettingsPager>
</dx:ASPxGridView>

