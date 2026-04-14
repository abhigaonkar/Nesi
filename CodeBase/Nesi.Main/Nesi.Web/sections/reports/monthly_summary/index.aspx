<%@
Page	Language		= 'C#'
MasterPageFile	= '../../../IntraDefault.master'
AutoEventWireup	= 'true'
Inherits        = 'sections_reports_monthly_summary_index '
Title			= 'Monthly Summary' 
EnableTheming = 'True'
 Codebehind="index.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>
<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
    <div id='divSide' runat='server'>
    </div>
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
    <div id='divMenu' runat='server'></div>
</asp:Content>

<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>

    <script type="text/javascript">

    </script>


    <table width="500" cellpadding="2" cellspacing="0">
    <tr>
        <td nowrap="nowrap">Tax Entity:</td>
        <td align="right" width="100%"><dx:ASPxComboBox runat="server" ID="combo_tax_entity" Theme="NETheme01" Width="100%" ValueField="id" TextField="ddl_name" ValueType="System.Int32"  AutoPostBack="true" OnSelectedIndexChanged="combo_tax_entity_SelectedIndexChanged"></dx:ASPxComboBox>
        </td>
    </tr>
        <tr><td nowrap="nowrap">Business Units:</td><td> <dx:ASPxCheckBoxList ID="cl_companies" runat="server" 
                         RepeatColumns="3" TextField="ddl_name" 
                         Theme="NETheme01" ValueField="id" ClientInstanceName="cl_companies">
    </dx:ASPxCheckBoxList></td></tr>
   
</table>



    <br />
    <asp:DropDownList ID="select_enddate" CssClass="enddate" runat="server" AutoPostBack="True" OnSelectedIndexChanged="select_dept_SelectedIndexChanged"></asp:DropDownList>
    <br />
    <br />
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
        <asp:ListItem Value="0">Detail Compare to Budget</asp:ListItem>
        <asp:ListItem Value="1">Detail Compare to Last Year</asp:ListItem>
        <asp:ListItem Value="2">Summary (To Budget)</asp:ListItem>
        <asp:ListItem Value="3">Summary (To Last Year)</asp:ListItem>
    </asp:RadioButtonList>
    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="gv1">
    </dx:ASPxGridViewExporter>
    
    <dx:ASPxButton ID="ASPxButton1" runat="server" onclick="btn_go_Click" 
                   Text="GO -&gt;" Theme="NETheme01">
    </dx:ASPxButton>

    <dx:ASPxButton ID="ASPxButton2" runat="server" OnClick="ASPxButton2_Click" Text="Export" Theme="NETheme01">
        <Image Url="~/images/icon/icon[excel].gif">
        </Image>
    </dx:ASPxButton>
    <br />
	
	
    <br />
    <div style="font-family:Calibri; font-weight:bold; font-size:medium;" id="div_results" runat="server">


      
    </div>
    <dx:ASPxGridView ID="gv1" runat="server" 
                     ClientInstanceName="gv1" Theme="NETheme01" Width="100%" 
                     OnCustomCallback="gv_customer_assets_CustomCallback" 
                     OnCustomJSProperties="gv_customer_assets_CustomJSProperties" 
                     OnHtmlDataCellPrepared="gv1_HtmlDataCellPrepared" EnableRowsCache="False" EnableViewState="False"  >
        <SettingsBehavior ColumnResizeMode="Control" />
        <SettingsPager Mode="ShowAllRecords">
        </SettingsPager>
        <Settings ColumnMinWidth="20" ShowFooter="True" ShowGroupFooter="VisibleAlways" 
                  ShowGroupPanel="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupedColumns="True" ShowHeaderFilterButton="True" />
        <Styles>
            <Header Wrap="True">
            </Header>
            <Footer Font-Bold="True">
            </Footer>
            <GroupFooter Font-Bold="True">
            </GroupFooter>
        </Styles>
    </dx:ASPxGridView>


</asp:Content>
<asp:Content ID="Content4" runat="server" contentplaceholderid="header_placeholder">
    <style type="text/css">
        .auto-style1 {
            text-align: center;
            height: 17px;
        }
        .auto-style2 {
            height: 17px;
        }
        .auto-style3 {
            text-align: center;
        }
        .auto-style4 {
            height: 23px;
        }
    </style>
</asp:Content>

