<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits        = 'sections_reports_cash_balance_index'
			Title			= 'Cash Balance' 
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


  
    <br />


    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" Visible="False" ForeColor="red"
               Text=""></asp:Label> <br />
<asp:Label ID="name_branch" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="24pt"
					Text="Branch Name"></asp:Label>
	
	


    <br />
    <uc1:layout_control ID="layout" runat="server"  />
    <dx:ASPxGridView ID="gv1" runat="server" AutoGenerateColumns="True" 
        ClientInstanceName="gv1" Theme="NETheme01" Width="100%" 
        OnCustomCallback="gv_customer_assets_CustomCallback" 
			OnCustomJSProperties="gv_customer_assets_CustomJSProperties" 
        OnDataBound="gv1_DataBound" 
        OnHtmlDataCellPrepared="gv1_HtmlDataCellPrepared"  >
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
