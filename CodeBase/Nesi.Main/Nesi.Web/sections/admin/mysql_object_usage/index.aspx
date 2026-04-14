<%@ Page Language="C#" MasterPageFile='~/IntraDefault.master' AutoEventWireup="true" Inherits="mysql_object_usage" Codebehind="index.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/sections/reports/../../modules/layout_control.ascx" TagName="layout_control" TagPrefix="uc1" %>

<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
    <div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' runat='Server'>
<uc1:layout_control ID="layout" runat="server" />
<asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>
			
<div id='income_statement'>
<table class='framework' border='0' cellpadding='0' cellspacing='0'>
    <tr>
        <td valign='top' align='center' id='detail' runat='server'></td>
    </tr>
</table>
<div id='errormsg'>
<table width="100%" cellpadding="2" cellspacing="0">
<tr>
<td valign="top" style="height: 392px" class="style7">
<dx:ASPxGridView
    ID="gvUsage" Theme="NETheme01" runat="server" AutoGenerateColumns="True"
    Font-Names="Arial" 
    Font-Size="8pt" >
<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control"
                  AutoExpandAllGroups="True" />
<SettingsPager Mode="ShowAllRecords">
</SettingsPager>
<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="true"
          ShowHeaderFilterButton="True" ShowFooter="True" GridLines="None" GroupFormat="{1} {2}"
          ShowGroupFooter="VisibleAlways" UseFixedTableLayout="True" ColumnMinWidth="30" />
<Styles GroupButtonWidth="1">
    <GroupRow BackColor="White">
    </GroupRow>
    <RowHotTrack BackColor="#FFCCCC">
    </RowHotTrack>
    <Footer HorizontalAlign="Right">
    </Footer>
    <GroupFooter BackColor="White">
        <Border BorderStyle="None" />
    </GroupFooter>
    <GroupPanel>
        <Border BorderStyle="None" />
    </GroupPanel>
</Styles>
</dx:ASPxGridView>
</td>
</tr>
</table>
</div>
</div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
