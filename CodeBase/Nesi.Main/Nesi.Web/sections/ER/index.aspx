<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'sections_er_index' 
			Title			= 'Electronic Repair' 
 Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" tagprefix="dxw" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
<script type ="text/javascript">


function OnCustomerChanged(ddlCustomer)
{
//alert ("hello");
ddl_jobAddress.PerformCallback(ddl_jobCustomer.GetValue().toString());
ddl_jobContact.PerformCallback(ddl_jobCustomer.GetValue().toString());
}

function ddlItemChanged(ddleritem)
{


}




</script>

    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
        &nbsp;</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
          <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
         <script type="text/javascript">       
  


         </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="toplblerror" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Red"></asp:Label><br />
            <dx:ASPxPageControl ID="pagecontrol1" runat="server" ActiveTabIndex="0" Width="100%">
                <TabPages>
                    <dx:TabPage Text="Current Jobs">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxwgv:ASPxGridView ID="grid_currentjobs" runat="server" AutoGenerateColumns="False"
                                    Width="100%" KeyFieldName="erjob_id" OnHtmlEditFormCreated="grid_currentjobs_HtmlEditFormCreated" Font-Names="Arial" OnInitNewRow="grid_currentjobs_InitNewRow" OnRowDeleting="grid_currentjobs_RowDeleting">
                                    <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="erjob_id" VisibleIndex="0">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName" VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Date Entered" FieldName="erjob_DateEntered"
                                            VisibleIndex="2">
                                            <PropertiesTextEdit DisplayFormatString="yyyy-MM-dd">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Make" FieldName="eritem_make" VisibleIndex="3">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Model" FieldName="eritem_model" VisibleIndex="4">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Quoted Price" FieldName="erjob_quotedprice"
                                            VisibleIndex="5">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Benchmark Sell" FieldName="woprog_totalTandM"
                                            VisibleIndex="6">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="BV WO" FieldName="woprog_bvwo" VisibleIndex="7">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="erjob_status" VisibleIndex="8">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Warranty" FieldName="erjob_warranty" VisibleIndex="9">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="10" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"   ShowNewButton="true">
                                            
                                            
                                            
                                            
                                        </dxwgv:GridViewCommandColumn>
                                    </Columns>
                                    <SettingsPager NumericButtonCount="50" PageSize="50">
                                    </SettingsPager>
                                    <Styles>
                                        <Row Wrap="False">
                                        </Row>
                                        <Header BackColor="Maroon" ForeColor="Gainsboro">
                                        </Header>
                                        <Footer BackColor="#FFC0C0">
                                        </Footer>
                                    </Styles>
                                    <StylesPopup EditForm-Header-BackColor="#C00000" EditForm-Header-ForeColor="White" EditForm-Content-Paddings-Padding="2px"/>
                                    <Templates>
                                        <EditForm>
                                            <table style="width: 100%; white-space:nowrap; padding-right: 5px; padding-left: 5px; padding-bottom: 5px; padding-top: 5px;"  >
                                                <tr>
                                                    <td colspan="5">
                                                        &nbsp;<dxe:ASPxLabel ID="lbl_joberror" runat="server" Font-Bold="True" ForeColor="Red">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Job ID</td>
                                                    <td style="width: 131px">
                                                        <dxe:ASPxTextBox ID="txtjob_id" runat="server" Width="100%" ClientInstanceName="txtjob_description" ClientEnabled="False">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                    </td>
                                                    <td style="width: 16px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Status</td>
                                                    <td style="width: 131px">
                                                        <dxe:ASPxComboBox ID="ddl_jobstatus" runat="server" ValueType="System.String" ClientInstanceName="ddl_jobstatus">
                                                            <Items>
                                                                <dxe:ListEditItem Text="Waiting to be Quoted" Value="0" />
                                                                <dxe:ListEditItem Text="Waiting for Approval" Value="1" />
                                                                <dxe:ListEditItem Text="Open" Value="2" />
                                                                <dxe:ListEditItem Text="Being Tested" Value="3" />
                                                                <dxe:ListEditItem Text="Waiting for Parts" Value="4" />
                                                                <dxe:ListEditItem Text="Ready for Pickup" Value="5" />
                                                                <dxe:ListEditItem Text="Closed" Value="6" />
                                                            </Items>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                        ER Item</td>
                                                    <td style="width: 16px">
                                                        <dxe:ASPxComboBox ID="ddl_jobERitem" runat="server" TextField="eritem" ValueField="eritem_id"
                                                            ValueType="System.Int32" ClientInstanceName="ddl_jobERitem" AutoPostBack="True" OnSelectedIndexChanged="ddl_jobERitem_SelectedIndexChanged">
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Customer</td>
                                                    <td style="width: 131px">
                                                        <dxe:ASPxComboBox ID="ddl_jobCustomer" runat="server" CallbackPageSize="40" IncrementalFilteringMode="StartsWith"
                                                            TextField="customer_name" ValueField="customer_id" ValueType="System.Int32" ClientInstanceName="ddl_jobCustomer" EnableCallbackMode="True" EnableSynchronization="False">
                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	 OnCustomerChanged(s);
}" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                        Make</td>
                                                    <td style="width: 16px">
                                                        <dxe:ASPxTextBox ID="txt_jobMake" runat="server" ClientEnabled="False" ClientInstanceName="txt_jobMake"
                                                            Width="170px">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Address</td>
                                                    <td style="width: 131px">
                                                        <dxe:ASPxComboBox ID="ddl_jobAddress" runat="server" OnCallback="ddl_jobAddress_Callback"
                                                            TextField="address" ValueField="address_id" ValueType="System.Int32" EnableCallbackMode="True" ClientInstanceName="ddl_jobAddress">
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                        Model</td>
                                                    <td style="width: 16px">
                                                        <dxe:ASPxTextBox ID="txt_jobModel" runat="server" ClientEnabled="False" ClientInstanceName="txt_jobModel"
                                                            Width="170px">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Contact</td>
                                                    <td style="width: 131px">
                                                        <dxe:ASPxComboBox ID="ddl_jobContact" runat="server" CallbackPageSize="40" IncrementalFilteringMode="StartsWith"
                                                            TextField="contact_name" ValueField="contact_id" ValueType="System.Int32" ClientInstanceName="ddl_jobContact" EnableCallbackMode="True" EnableSynchronization="False" OnCallback="ddl_jobContact_Callback">
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                        Part
                                                        Description</td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="txtjob_description" runat="server" Width="100%" ClientInstanceName="txtjob_description">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Date Entered</td>
                                                    <td style="width: 131px"><dxe:ASPxDateEdit ID="dte_jobDateEntered" runat="server" DisplayFormatString="yyyy-MM-dd"
                                                            EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                                                    </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                        Serial No</td>
                                                    <td>
                                                        <dxe:ASPxTextBox ID="txtSerialNo" runat="server" ClientInstanceName="txtSerialNo"
                                                            Width="170px">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Date Quoted</td>
                                                    <td style="width: 131px">
                                                        <dxe:ASPxTextBox ID="txt_jobDateQuoted" runat="server" DisplayFormatString="yyyy-MM-dd"
                                                            Width="170px" ClientInstanceName="txt_jobDateQuoted">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                    </td>
                                                    <td style="width: 16px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Date Shipped</td>
                                                    <td style="width: 131px">
                                                        <dxe:ASPxDateEdit ID="dte_jobShipped" runat="server" DisplayFormatString="yyyy-MM-dd"
                                                            EditFormat="Custom" EditFormatString="yyyy-MM-dd" ClientInstanceName="dte_jobShipped">
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                    </td>
                                                    <td style="width: 16px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Warranty</td>
                                                    <td style="width: 131px">
                                                        <dxe:ASPxCheckBox ID="chk_jobwarranty" runat="server" Text=" " ClientInstanceName="chk_jobwarranty">
                                                        </dxe:ASPxCheckBox>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                        Repair Price</td>
                                                    <td style="width: 16px">
                                                        <dxe:ASPxTextBox ID="txtJobRepairPrice" runat="server" DisplayFormatString="C2" Width="170px" ClientInstanceName="txtJobRepairPrice">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Shipped Via</td>
                                                    <td style="width: 131px">
                                                        <dxe:ASPxComboBox ID="ddl_JobShippedVia" runat="server" ValueType="System.String" ClientInstanceName="ddl_JobShippedVia">
                                                            <Items>
                                                                <dxe:ListEditItem Text="Customer Pickup" Value="0" />
                                                                <dxe:ListEditItem Text="Sales Drop Off" Value="1" />
                                                                <dxe:ListEditItem Text="Courier" Value="2" />
                                                                <dxe:ListEditItem Text="Electrician Courier" Value="3" />
                                                            </Items>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                    <td style="width: 0px">
                                                        <dxe:ASPxCheckBox ID="chkjobincludenewprice" runat="server" Text="Include New Price"
                                                            TextAlign="Left" Wrap="False">
                                                        </dxe:ASPxCheckBox>
                                                    </td>
                                                    <td style="width: 16px">
                                                        <dxe:ASPxTextBox ID="txtjobnewprice" runat="server" DisplayFormatString="C2" Width="170px" ClientInstanceName="txtjobnewprice">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 139px">
                                                        Notes</td>
                                                    <td colspan="4">
                                                        <dxe:ASPxMemo ID="mem_jobnotes" runat="server" Height="71px" Width="100%" ClientInstanceName="mem_jobnotes">
                                                        </dxe:ASPxMemo>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="1" style="width: 139px">
                                                    </td>
                                                    <td colspan="4">
                                                        <table width="100%">
                                                            <tr>
                                                                <td style="width: 69px">
                                                                    <dxe:ASPxButton ID="btnJobSave" runat="server" OnClick="btnJobSave_Click" Text="Save" ClientInstanceName="btnJobSave">
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                                <td colspan="3" style="text-align: center">
                                                                    <dxe:ASPxButton ID="btnJobPrint" runat="server" OnClick="btnJobPrint_Click" Text="Print Quote"
                                                                        Wrap="False" ClientInstanceName="btnJobPrint">
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                                <td style="width: 85px">
                                                                    <dxe:ASPxButton ID="btnJobCancel" runat="server" OnClick="btnJobCancel_Click" Text="Cancel" ClientInstanceName="btnJobCancel">
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                                <td style="width: 85px; text-align: right">
                                                                    <dxe:ASPxButton ID="btnJobCutWO" runat="server" OnClick="btnJobCutWO_Click" Text="Cut WO"
                                                                        Wrap="False" ClientInstanceName="btnJobCutWO">
                                                                    </dxe:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 50%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </EditForm>
                                    </Templates>
                                    <SettingsEditing Mode="PopupEditForm" />
                                    <SettingsPopup EditForm-HorizontalAlign="Center" EditForm-VerticalAlign="Above"
                                        EditForm-Width="600px" />
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <SettingsText PopupEditFormCaption="Job Information" />
                                </dxwgv:ASPxGridView>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Products">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxwgv:ASPxGridView ID="grid_products" runat="server" AutoGenerateColumns="False"
                                    Width="100%" OnCancelRowEditing="grid_products_CancelRowEditing" OnInitNewRow="grid_products_InitNewRow" OnRowDeleting="grid_products_RowDeleting" OnHtmlEditFormCreated="grid_products_HtmlEditFormCreated" KeyFieldName="eritem_id" Font-Names="Arial">
                                    <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="eritem_id" VisibleIndex="0">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Make" FieldName="eritem_make" VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Model" FieldName="eritem_model" VisibleIndex="2">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Repair Price" FieldName="repairprice" VisibleIndex="3">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="New Price" FieldName="newprice" VisibleIndex="4">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Qty in Stock" FieldName="qtyinstock" VisibleIndex="5">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Last Repaired Date" FieldName="datelastrepaired"
                                            VisibleIndex="6">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Description" FieldName="eritem_decription"
                                            VisibleIndex="7" Width="100%">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="8" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"   ShowNewButton="true">
                                            
                                            
                                            
                                            
                                        </dxwgv:GridViewCommandColumn>
                                    </Columns>
                                    <SettingsEditing Mode="PopupEditForm" />
                                    <SettingsPopup EditForm-HorizontalAlign="WindowCenter" EditForm-VerticalAlign="WindowCenter"/>
                                    <SettingsText PopupEditFormCaption="ER Item" />
                                    <Templates>
                                        <EditForm>
                                            <asp:Label ID="ef_lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label><br />
                                            <dx:ASPxPageControl id="ASPxPageControl1" runat="server" ActiveTabIndex="1" Width="750px">
                                                <tabpages>
<dx:TabPage Text="General Info"><ContentCollection>
<dxw:ContentControl runat="server"><table runat="server" ID="tblgeneral" width="100%"><tr runat="server"><TD runat="server">ER Item ID</TD>
<TD runat="server"><dxe:ASPxTextBox runat="server" Width="170px" HorizontalAlign="Right" ClientEnabled="False" ID="ef_txteritem_id" __designer:wfdid="w61"></dxe:ASPxTextBox>


 </TD>
<TD runat="server"></TD>
</tr>
<tr runat="server"><TD runat="server">Make</TD>
<TD runat="server"><dxe:ASPxTextBox runat="server" Width="170px" ClientInstanceName="ef_txtmake" ID="ef_txtmake" __designer:wfdid="w62"></dxe:ASPxTextBox>


 </TD>
<TD runat="server"></TD>
</tr>
<tr runat="server"><TD runat="server">Model</TD>
<TD runat="server"><dxe:ASPxTextBox runat="server" Width="170px" ClientInstanceName="ef_txtmodel" ID="ef_txtmodel" __designer:wfdid="w63"></dxe:ASPxTextBox>


 </TD>
<TD runat="server"></TD>
</tr>
<tr runat="server"><TD runat="server">Description</TD>
<TD runat="server"><dxe:ASPxTextBox runat="server" Width="170px" ID="ASPxTextBox1" __designer:wfdid="w64"></dxe:ASPxTextBox>


 </TD>
<TD runat="server"></TD>
</tr>
<tr runat="server"><TD runat="server">Repair Price</TD>
<TD runat="server"><dxe:ASPxTextBox runat="server" Width="170px" HorizontalAlign="Right" DisplayFormatString="C2" ClientInstanceName="ef_txtrepairprice" ID="ef_txtrepairprice" __designer:wfdid="w65"></dxe:ASPxTextBox>


 </TD>
<TD runat="server"></TD>
</tr>
<tr runat="server"><TD runat="server">New Price</TD>
<TD runat="server"><dxe:ASPxTextBox runat="server" Width="170px" HorizontalAlign="Right" DisplayFormatString="C2" ClientInstanceName="ef_txtnewprice" ID="ef_txtnewprice" __designer:wfdid="w66"></dxe:ASPxTextBox>


 </TD>
<TD runat="server"></TD>
</tr>
<tr runat="server"><TD runat="server">Qty in Stock</TD>
<TD runat="server"><dxe:ASPxTextBox runat="server" Width="170px" HorizontalAlign="Right" ClientInstanceName="ef_txtqtyinstock" ID="ef_txtqtyinstock" __designer:wfdid="w67"></dxe:ASPxTextBox>


 </TD>
<TD runat="server"></TD>
</tr>
<tr runat="server"><TD runat="server">Date of Last Repair</TD>
<TD runat="server"><dxe:ASPxTextBox runat="server" Width="170px" DisplayFormatString="yyyy:MM:dd" ClientEnabled="False" ID="ef_txtDateofLastRepair" __designer:wfdid="w68"></dxe:ASPxTextBox>


 </TD>
<TD runat="server"></TD>
</tr>
<tr runat="server"><TD runat="server" colspan="2">&nbsp;<TABLE width="100%"><TBODY><TR><TD><dxe:ASPxButton runat="server" Text="Save" ID="ef_btnSave" __designer:wfdid="w69" OnClick="ef_btnSave_Click"></dxe:ASPxButton>


 </TD><TD align=center><dxe:ASPxButton runat="server" Wrap="False" Text="Print Quote" ID="ef_btnPrintQuote" __designer:wfdid="w70" OnClick="ef_btnPrintQuote_Click"></dxe:ASPxButton>


 </TD><TD align=right><dxe:ASPxButton runat="server" Text="Cancel" ID="ef_btnCancel" __designer:wfdid="w71" OnClick="ef_btnCancel_Click"></dxe:ASPxButton>


 </TD></TR><TR><TD></TD><TD align=center></TD><TD align=right></TD></TR><TR><TD></TD><TD align=center></TD><TD align=right></TD></TR></TBODY></TABLE></TD>
<TD runat="server"></TD>
</tr>
</table>


</dxw:ContentControl>
</ContentCollection>
</dx:TabPage>
<dx:TabPage Text="Vendors"><ContentCollection>
<dxw:ContentControl runat="server"><asp:Label runat="server" Font-Bold="True" ForeColor="Red" ID="lbl_vendorerror" __designer:dtid="26458647810801730" __designer:wfdid="w3"></asp:Label>
 &nbsp;&nbsp;<BR />
    <dxwgv:ASPxGridView runat="server" KeyFieldName="er_vendor_data_id" AutoGenerateColumns="False" Width="100%" ID="grid_vendor_data" __designer:wfdid="w77" OnCustomButtonCallback="grid_vendor_data_CustomButtonCallback" OnInitNewRow="grid_vendor_data_InitNewRow">
        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
        <Columns>
<dxwgv:GridViewDataTextColumn FieldName="er_vendor_data_id" ReadOnly="True" Visible="False" VisibleIndex="0">
<EditFormSettings Visible="False"></EditFormSettings>
</dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="er_vendor_data_vendor_id" Caption="Vendor ID" VisibleIndex="1"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="Vendor_Name" Caption="Vendor" VisibleIndex="0"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="er_vendor_data_vendorpart" Caption="Vendor Part Number" VisibleIndex="2"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="er_vendor_data_itemid" Caption="Item ID" Visible="False" VisibleIndex="3"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="er_vendor_data_cost" Caption="Cost" VisibleIndex="4"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataDateColumn FieldName="er_vendor_data_date" Caption="Date Entered" VisibleIndex="5"></dxwgv:GridViewDataDateColumn>
<dxwgv:GridViewDataTextColumn FieldName="vendorphone" Caption="Phone" VisibleIndex="6"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="8" ShowEditButton="true" ShowDeleteButton="true" ShowNewButton="true">





<CustomButtons>
<dxwgv:GridViewCommandColumnCustomButton>
<Image Url="~/images/icon/icon[delete].gif"></Image>
</dxwgv:GridViewCommandColumnCustomButton>
</CustomButtons>
</dxwgv:GridViewCommandColumn>
</Columns>

<SettingsBehavior ConfirmDelete="True"></SettingsBehavior>

<Templates><EditForm>
<TABLE width=300><TBODY><TR><TD style="WIDTH: 82px">Vendor ID</TD><TD style="WIDTH: 58px"><dxe:ASPxTextBox id="txt_vendor_edit_vendor_id" runat="server" Width="170px" __designer:wfdid="w16" ClientEnabled="False"></dxe:ASPxTextBox></TD></TR><TR><TD style="WIDTH: 82px">Vendor</TD><TD style="WIDTH: 58px"><dxe:ASPxComboBox id="ddl_vendor_edit_vendor" runat="server" __designer:wfdid="w17" TextField="vendor_name" ValueField="vendor_id" ValueType="System.Int32" DataSourceID="SqlDataSource1"></dxe:ASPxComboBox></TD></TR><TR><TD style="WIDTH: 82px">Part Number</TD><TD style="WIDTH: 58px"><dxe:ASPxTextBox id="txt_vendor_edit_partnumber" runat="server" Width="170px" __designer:wfdid="w18"></dxe:ASPxTextBox></TD></TR><TR><TD style="WIDTH: 82px">Cost</TD><TD style="WIDTH: 58px"><dxe:ASPxTextBox id="txt_vendor_edit_cost" runat="server" Width="170px" __designer:wfdid="w19" DisplayFormatString="C2"></dxe:ASPxTextBox></TD></TR><TR><TD style="WIDTH: 82px"></TD><TD>&nbsp;<TABLE style="WIDTH: 100%"><TBODY><TR><TD align=left><dxe:ASPxButton id="btnSaveVendor" onclick="btnSaveVendor_Click" runat="server" __designer:wfdid="w20" Text="Save"></dxe:ASPxButton></TD><TD></TD><TD style="WIDTH: 468px" align=right><dxe:ASPxButton id="btnCancelVendor" onclick="btnCancelVendor_Click" runat="server" __designer:wfdid="w21" Text="Cancel"></dxe:ASPxButton>&nbsp;&nbsp;&nbsp; </TD></TR><TR><TD style="HEIGHT: 16px" align=left></TD><TD style="HEIGHT: 16px"></TD><TD style="WIDTH: 468px; HEIGHT: 16px" align=right></TD></TR><TR><TD align=left></TD><TD></TD><TD style="WIDTH: 468px" align=right></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>
</EditForm>
</Templates>
</dxwgv:ASPxGridView>
 </dxw:ContentControl>
</ContentCollection>
</dx:TabPage>
<dx:TabPage Text="Repair History"><ContentCollection>
<dxw:ContentControl runat="server"><dxwgv:ASPxGridView runat="server" AutoGenerateColumns="False" Width="100%" ID="ef_grid_repairhistory" __designer:wfdid="w14"><Columns>
<dxwgv:GridViewDataTextColumn FieldName="erjob_id" Caption="Job ID" VisibleIndex="0"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="erjob_dateentered" Caption="Date" VisibleIndex="1"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="erjob_repairprice" Caption="Repair Price" VisibleIndex="2"></dxwgv:GridViewDataTextColumn>
<dxwgv:GridViewDataTextColumn FieldName="erjob_branch" Caption="Branch" VisibleIndex="3"></dxwgv:GridViewDataTextColumn>
</Columns>
</dxwgv:ASPxGridView>

























</dxw:ContentControl>
</ContentCollection>
</dx:TabPage>
<dx:TabPage Text="Notes"><ContentCollection>
<dxw:ContentControl runat="server"><TABLE width="100%"><TBODY><TR><TD><dxe:ASPxMemo runat="server" Height="150px" Width="100%" ID="ef_txtNotes" __designer:wfdid="w17"></dxe:ASPxMemo>

























</TD></TR><TR><TD><dxe:ASPxButton runat="server" Text="Save" ID="ef_btnSaveNotes" __designer:wfdid="w18"></dxe:ASPxButton>

























</TD></TR><TR><TD style="HEIGHT: 16px"></TD></TR></TBODY></TABLE></dxw:ContentControl>
</ContentCollection>
</dx:TabPage>
<dx:TabPage Text="Files"><ContentCollection>
<dxw:ContentControl runat="server"><TABLE width="100%"><TBODY><TR><TD style="WIDTH: 0%"><dxe:ASPxButton runat="server" Wrap="False" Text="Create Folder" ID="ASPxButton1" __designer:wfdid="w21"></dxe:ASPxButton>

























</TD><TD colSpan=2><dxe:ASPxLabel runat="server" Text="ASPxLabel" Width="100%" ID="ASPxLabel1" __designer:wfdid="w24"></dxe:ASPxLabel>

























</TD></TR><TR><TD colSpan=3></TD></TR><TR><TD style="WIDTH: 11px"></TD><TD style="WIDTH: 16px"></TD><TD style="WIDTH: 23px"></TD></TR></TBODY></TABLE></dxw:ContentControl>
</ContentCollection>
</dx:TabPage>
</tabpages>
                                            </dx:ASPxPageControl>
                                        </EditForm>
                                    </Templates>
                                    <Styles>
                                        <Row Wrap="False">
                                        </Row>
                                        <DetailRow Wrap="False">
                                        </DetailRow>
                                        <Header BackColor="Firebrick" ForeColor="White">
                                        </Header>
                                    </Styles>
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <StylesEditors>
                                        <Style Font-Names="Arial"></Style>
                                    </StylesEditors>
                                  
                                </dxwgv:ASPxGridView>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Text="Repair History">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxwgv:ASPxGridView ID="grid_repairs" runat="server" AutoGenerateColumns="False"
                                    Width="100%">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="WO ID" FieldName="woprog_id" VisibleIndex="0">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Date" FieldName="woprog_cutdatetime" VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Repair Price" FieldName="woprog_invoicednettotal"
                                            VisibleIndex="2">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Warranty" FieldName="woprog_warranty" VisibleIndex="3">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <Styles>
                                        <Row Wrap="False">
                                        </Row>
                                    </Styles>
                                </dxwgv:ASPxGridView>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                </TabPages>
                <ActiveTabStyle BackColor="#C00000">
                </ActiveTabStyle>
                <TabStyle BackColor="#404040" Font-Bold="True" Font-Names="Arial" Height="25px" ForeColor="White">
                    <HoverStyle BackColor="Red">
                    </HoverStyle>
                </TabStyle>
            </dx:ASPxPageControl>
            &nbsp;
            <asp:HiddenField ID="hdnItemid" runat="server" />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                SelectCommand="Select * from vw_activevendors"></asp:SqlDataSource>
            <dx:ASPxPopupControl ID="pop_cutwo" runat="server" ClientInstanceName="pop_cutwo"
                HeaderText="Cut WO" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ModalBackgroundStyle Opacity="50">
                </ModalBackgroundStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table width="100%">
                            <tr>
                                <td colspan="2" style="text-align: center">
                                    <br />
                                    <dxe:ASPxRadioButtonList ID="ASPxRadioButtonList1" runat="server" SelectedIndex="0"
                                        TextWrap="False" ValueType="System.Int32">
                                        <Items>
                                            <dxe:ListEditItem Selected="True" Text="Quoted Price" Value="0" />
                                            <dxe:ListEditItem Text="Time and Material" Value="1" />
                                        </Items>
                                    </dxe:ASPxRadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="Cut" Width="75px">
                                    </dxe:ASPxButton>
                                </td>
                                <td style="text-align: center">
                                    <dxe:ASPxButton ID="ASPxButton3" runat="server" Text="Cancel" Width="75px">
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            &nbsp;
        </ProgressTemplate>
    </asp:UpdateProgress>
    

</asp:Content>
