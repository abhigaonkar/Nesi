<%@ Page Language="C#" MasterPageFile="../../../IntraDefault.master" AutoEventWireup="true" Inherits="VacationAdmin" Title="Vacation Administration" Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>

 

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
<div id="divSide" runat="server">
    <asp:SqlDataSource ID="Users" runat="server"></asp:SqlDataSource>
</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
<div id="divMenu" runat="server"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    
    <br />
    <uc1:layout_control ID="lc" runat="server" />
    <dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv" KeyFieldName="vacation_id" OnCustomButtonInitialize="gv_CustomButtonInitialize" OnHtmlDataCellPrepared="gv_HtmlDataCellPrepared" Theme="NETheme01" Width="100%" OnCustomButtonCallback="gv_CustomButtonCallback" OnCustomCallback="gv_CustomCallback" OnCustomJSProperties="gv_CustomJSProperties" SettingsEditing-Mode="Inline" OnCellEditorInitialize="gv_CellEditorInitialize" OnRowInserting="gv_RowInserting" OnRowUpdating="gv_RowUpdating" OnCommandButtonInitialize="gv_CommandButtonInitialize">
        <SettingsPager PageSize="100">
        </SettingsPager>
       
       
        <Columns>
                <dx:GridViewCommandColumn ButtonRenderMode="Link" ButtonType="Link" Caption=" " ShowClearFilterButton="True" ShowInCustomizationForm="True" VisibleIndex="0" ShowCancelButton="True" ShowUpdateButton="True" ShowEditButton="True">
                <CustomButtons>
                    <dx:GridViewCommandColumnCustomButton ID="Approve" Text=" ">
                        <Image Url="~/images/icon/icon[approve].gif">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                    <dx:GridViewCommandColumnCustomButton ID="Deny" Text=" ">
                        <Image Url="~/images/icon/icon[deny].gif">
                        </Image>
                    </dx:GridViewCommandColumnCustomButton>
                </CustomButtons>
                <CellStyle Wrap="False" VerticalAlign="Middle">
                </CellStyle>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="id" FieldName="vacation_id" ShowInCustomizationForm="True" Visible="False" VisibleIndex="16">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Payroll ID" FieldName="Member_Payroll_ID" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Employee" FieldName="full_name" ShowInCustomizationForm="True" VisibleIndex="2">
                <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Business Unit" FieldName="ddl_name" ShowInCustomizationForm="True" VisibleIndex="3">
                <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Status" FieldName="status" ShowInCustomizationForm="True" VisibleIndex="4">
                <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Date Start" FieldName="date_start" VisibleIndex="5">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn Caption="Date End" FieldName="date_end" VisibleIndex="6">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Requested On" FieldName="date_insert" VisibleIndex="7">
                
                <EditFormSettings Visible="False" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Date Returning" FieldName="date_return" VisibleIndex="8">
                <PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataComboBoxColumn Caption="Payment Type" FieldName="payment_method_id" VisibleIndex="9">
                <PropertiesComboBox DataSourceID="Sqlvacation_pay_type" TextField="method" ValueField="method_id">
                </PropertiesComboBox>
                <SettingsHeaderFilter Mode="CheckedList">
                </SettingsHeaderFilter>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataComboBoxColumn>
            
             <dx:GridViewDataTextColumn Caption="_ph_count" Visible="False" FieldName="_ph_count" EditFormSettings-Visible="False" ShowInCustomizationForm="True" VisibleIndex="12">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
             <dx:GridViewDataTextColumn Caption="Amount (Hours)" FieldName="amount" ShowInCustomizationForm="True" VisibleIndex="13" CellStyle-HorizontalAlign="Center">
                 <EditFormSettings Visible="True" />

<CellStyle HorizontalAlign="Center"></CellStyle>
            </dx:GridViewDataTextColumn>
              <dx:GridViewDataTextColumn Caption="Payrol Complete" Visible="False" EditFormSettings-Visible="False" FieldName="completed" ShowInCustomizationForm="True" VisibleIndex="14">
<EditFormSettings Visible="False"></EditFormSettings>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataMemoColumn Caption="Notes" FieldName="comments" VisibleIndex="15">
                <PropertiesMemoEdit Rows="4">
                </PropertiesMemoEdit>
                <EditItemTemplate>
                    <dx:ASPxCallback ID="cb_notes" runat="server" ClientInstanceName="cb_notes" OnCallback="cb_notes_Callback">
                        <ClientSideEvents EndCallback="function(s, e) {
	if (s.cp_message!=null &amp;&amp; s.cp_message!=&quot;&quot;)
{
alert(s.cp_message);
s.cp_message = null;
}
}" />
                    </dx:ASPxCallback>
                    <table style="width: 100%;">
                        <tr>
                            <td width="100%">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("comments") %>' Theme="NETheme01" Width="100%">
                                </dx:ASPxLabel>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxTextBox ID="tb_notes" runat="server" ClientInstanceName="tb_notes" Theme="NETheme01" Width="100%">
                                </dx:ASPxTextBox>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btn_save_note" runat="server" AutoPostBack="False" ClientInstanceName="btn_save_note" Text="Add Note" Theme="NETheme01">
                                    <ClientSideEvents Click="function(s, e) {
	cb_notes.PerformCallback(tb_notes.GetText());
}" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </dx:GridViewDataMemoColumn>
        </Columns>

<SettingsEditing Mode="EditForm" EditFormColumnCount="1"></SettingsEditing>

          <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" ShowHeaderFilterButton="True" ShowStatusBar="Visible" ShowFilterBar="Visible" ColumnMinWidth="20" />
        <SettingsResizing ColumnResizeMode="Control" />
<SettingsSearchPanel Visible="True" />
 

        <Styles>
            <CommandColumn VerticalAlign="Middle">
            </CommandColumn>
            <CommandColumnItem VerticalAlign="Middle">
            </CommandColumnItem>
            <EditFormCell VerticalAlign="Top">
            </EditFormCell>
            <EditFormTable VerticalAlign="Top">
            </EditFormTable>
        </Styles>
 

    </dx:ASPxGridView>
    <asp:SqlDataSource ID="Sqlvacation_pay_type" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select * from vacation_payment_method"></asp:SqlDataSource>
				<table id="vacation_admin" cellpadding='0' cellspacing='0' >
					<tr>	
						<td class='r' rowspan='2' id='Content' runat="server" valign='top' align="center"></td>
					</tr>	
				</table>
</asp:Content>