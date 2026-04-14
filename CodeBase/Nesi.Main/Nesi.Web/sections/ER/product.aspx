<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../nonFrame.master'
			AutoEventWireup	= 'true'
			Inherits		= 'product' 
			Title			= 'Electronic Repair' 
 Codebehind="product.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" tagprefix="dxw" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxp" %>
    




<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
          <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
         <script type="text/javascript">       
  


         </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            &nbsp;<dxp:ASPxPanel ID="pnl_select" runat="server" Width="100%">
                <PanelCollection>
                    <dxp:PanelContent runat="server">
                        <table style="font-family: Arial">
                            <tr>
                                <td style="width: 20px; height: 16px">
                                    Make:</td>
                                <td style="width: 16px; height: 16px">
                                    <dxe:ASPxComboBox ID="ddlMake" runat="server" ValueType="System.String">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td style="width: 34px; height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20px">
                                    Model:</td>
                                <td style="width: 16px">
                                    <dxe:ASPxComboBox ID="ddlModel" runat="server" ValueType="System.String">
                                    </dxe:ASPxComboBox>
                                </td>
                                <td style="width: 34px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20px">
                                </td>
                                <td style="width: 16px">
                                    &nbsp;</td>
                                <td style="width: 34px">
                                </td>
                            </tr>
                        </table>
                    </dxp:PanelContent>
                </PanelCollection>
            </dxp:ASPxPanel>
            <dxp:ASPxPanel ID="pnl_Addnew" runat="server" Width="100%">
                <PanelCollection>
                    <dxp:PanelContent runat="server">
                    </dxp:PanelContent>
                </PanelCollection>
            </dxp:ASPxPanel>
            <asp:Label ID="lblerror" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                ForeColor="Red"></asp:Label><br />
            <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="4" Width="600px">
                <TabPages>
                    <dx:TabPage Text="General Info">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            ER Item ID:</td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txteritemid" runat="server" ClientEnabled="False" Width="170px">
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Make:</td>
                                        <td>
                                            <asp:TextBox ID="txt_Make" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Model:</td>
                                        <td>
                                            <asp:TextBox ID="txt_Model" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Description:</td>
                                        <td>
                                            <asp:TextBox ID="txtDescription" runat="server" Height="50px" Width="300px" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Last Repair Job:</td>
                                        <td>
                                            <asp:HyperLink ID="lnk_lastJob" runat="server">HyperLink</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Price New:</td>
                                        <td>
                                            <asp:TextBox ID="txt_newprice" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Price Repair:</td>
                                        <td>
                                            <asp:TextBox ID="txtrepairprice" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Qty In Stock:</td>
                                        <td>
                                            <asp:TextBox ID="txtQtyinStock" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxe:ASPxButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click">
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btnCreateQuote" runat="server" Text="Create Quote" OnClick="btnCreateQuote_Click">
                                            </dxe:ASPxButton>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Vendors">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxwgv:ASPxGridView ID="grid_vendors" runat="server" Width="100%" AutoGenerateColumns="False">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="Vendor ID" FieldName="vendor_id" VisibleIndex="0">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Vendor" FieldName="Vendor_name" VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Phone" FieldName="vendor_phone" VisibleIndex="2">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Part No" FieldName="vendor_partno" VisibleIndex="3">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Price" FieldName="vendor_price" VisibleIndex="4">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Date" FieldName="Date_added" VisibleIndex="5">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                </dxwgv:ASPxGridView>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Repair History">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxwgv:ASPxGridView ID="grid_repairs" runat="server" Width="100%" AutoGenerateColumns="False">
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn Caption="ER job ID" FieldName="erjob_id" VisibleIndex="0">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Work Order" FieldName="woprog_bvwo" VisibleIndex="1">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Date" FieldName="erjob_dateentered" VisibleIndex="2">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="erjob_status" VisibleIndex="3">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Price" FieldName="erjob_repairprice" VisibleIndex="4">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Warranty" FieldName="erjob_warranty" VisibleIndex="5">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Notes">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <dxe:ASPxTextBox ID="ASPxTextBox1" runat="server" Height="400px" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </td>
                                        <td style="width: 0px">
                                            <dxe:ASPxButton ID="btnsavenotes" runat="server" OnClick="btnsavenotes_Click" Text="Save"
                                                Width="75px">
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Files">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 31px">
                                            <dxe:ASPxButton ID="btnCreatePath" runat="server" OnClick="btnCreatePath_Click" Text="Create Project Folder"
                                                Wrap="False">
                                            </dxe:ASPxButton>
                                        </td>
                                        <td style="width: 100%">
                                            <dxe:ASPxLabel ID="lblFolderPath" runat="server" Text="lblpath" Width="100%">
                                            </dxe:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" rowspan="2">
                                            <dx:ASPxFileManager ID="ASPxFileManager1" runat="server">
                                                <settings rootfolder="~\" thumbnailfolder="~\Thumb\"></settings>
                                                <settingsediting allowcreate="True" allowdelete="True" allowrename="True"></settingsediting>
                                            </dx:ASPxFileManager>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                </table>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                </TabPages>
            </dx:ASPxPageControl>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            &nbsp;
        </ProgressTemplate>
    </asp:UpdateProgress>
    

</asp:Content>
