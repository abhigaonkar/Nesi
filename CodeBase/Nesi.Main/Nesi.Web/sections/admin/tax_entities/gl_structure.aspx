<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="gl_structure" Title="Edit GL Accounts" Codebehind="gl_structure.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

<asp:SqlDataSource ID="sql_gl" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select * from gl_te left join gl_group_te on gl_group_te.id = gl_te.gl_group_id where gl_te.tax_entity_id = ?id order by gl_group_te.number,gl_te.account_no ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="id" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:HiddenField ID="hdn_tax_ent_it" runat="server" Value="1" />
<asp:SqlDataSource ID="sql_group" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id, concat(number,'-',type,'-',`desc`) number from gl_group_te where tax_entity_id = ?id order by gl_group_te.number">
    <SelectParameters>
        <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="id" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sql_subgroup" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select * from gl_subgroup_te where tax_entity_id = ?id">
    <SelectParameters>
        <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="id" PropertyName="Value" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sql_currency" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select * from currency"></asp:SqlDataSource>
<asp:SqlDataSource ID="sql_te" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id,ddl_name public_name from tax_entity"></asp:SqlDataSource>
<asp:SqlDataSource ID="sql_old_bvs_data" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="select distinct old_dsn name, old_dsn id from business_unit">
    <SelectParameters>
        <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="te_id" PropertyName="Value" />
    </SelectParameters>

</asp:SqlDataSource>
<asp:SqlDataSource ID="sql_old_bvs" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select dsn from tax_entity">
</asp:SqlDataSource>
<asp:SqlDataSource ID="sql_groups" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select *,concat(gl_group_te.number,'-',gl_group_te.desc) group_name from gl_group_te where tax_entity_id = ?id order by gl_group_te.number"
      InsertCommand="INSERT INTO gl_group_te (number, `desc`, `type`, tax_entity_id) VALUES (@number, @desc, @type, @id)">
    <SelectParameters>
        <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="id" PropertyName="Value" />
    </SelectParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="id" PropertyName="Value" />
        <asp:Parameter Name="number" />
        <asp:Parameter Name="desc" />
        <asp:Parameter Name="type" />
    </InsertParameters>
</asp:SqlDataSource>
<dx:ASPxPageControl ID="pc" runat="server" ActiveTabIndex="1" ClientInstanceName="pc" Width="100%" OnActiveTabChanged="pc_ActiveTabChanged" AutoPostBack="True">
    <TabPages>
        <dx:TabPage Text="Groups">
            <ContentCollection>
                <dx:ContentControl runat="server">
               
                    <table style="width:100%; ">
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td width="100%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="btn_clear_unused0" runat="server" OnClick="btn_clear_unused_groups_Click" Text="Clear Unused Groups" Enabled="False">
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="btn_copy_groups_from_bv" runat="server" OnClick="btn_copy_groups_from_bv_Click" Text="Copy Groups From BV" Enabled="False">
                                </dx:ASPxButton>
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <dx:ASPxGridView ID="gv_groups" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_groups" DataSourceID="sql_groups" KeyFieldName="id" Theme="NETheme01" Width="100%">
                        <Columns>
                            <dx:GridViewCommandColumn ShowDeleteButton="False" ShowEditButton="False" ShowInCustomizationForm="True" ShowNewButtonInHeader="False" VisibleIndex="0">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <EditFormSettings Visible="False" />

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="number" ShowInCustomizationForm="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="desc" ShowInCustomizationForm="True" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="type" ShowInCustomizationForm="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="tax_entity_id" ShowInCustomizationForm="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager Mode="ShowAllRecords">
                        </SettingsPager>
                        <Settings ShowGroupPanel="True" />
                        <Styles>
                            <Header Wrap="True">
                            </Header>
                        </Styles>
                    </dx:ASPxGridView>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
        <dx:TabPage Text="Accounts">
            <ContentCollection>
                <dx:ContentControl runat="server">
                    <dx:ASPxButton ID="btn_sync0" runat="server" Text="Sync nesi and bv GLs" OnClick="btn_sync_Click" ClientVisible="False"></dx:ASPxButton>
                      
                    <table style="width:100%;  table-layout: auto; border-collapse: collapse; empty-cells: hide; ">
                        <tr>
                            <td align="right">
                                <dx:ASPxComboBox ID="ddl_structure_from_legacy" runat="server" Caption="Merge structure from this BV" DataSourceID="sql_old_bvs" TextField="dsn" ValueField="dsn" Width="200px" ClientVisible="false">
                                </dx:ASPxComboBox>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btn_merge_from_legacy_bv" runat="server" OnClick="btn_merge_from_legacy_bv_Click" Text="GO" ClientVisible="False">
                                </dx:ASPxButton>
                            </td>
                            <td width="100%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <dx:ASPxComboBox ID="ddl_data_from_legacy" runat="server" Caption="Import IS GL Data FROM THIS BV" DataSourceID="sql_old_bvs_data" TextField="name" ValueField="id" Width="200px" ClientVisible="False">
                                </dx:ASPxComboBox>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btn_copy_data_from_legacy" runat="server" OnClick="btn_copy_data_from_legacy_Click" Text="GO" ClientVisible="False">
                                </dx:ASPxButton>
                            </td>
                            <td width="100%">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1"></td>
                            <td class="auto-style1"></td>
                            <td width="100%" class="auto-style1"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" class="auto-style1">
                            </td>
                            <td class="auto-style1">
                            </td>
                            <td class="auto-style1"></td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxButton ID="btn_clear_unused" runat="server"  OnClick="btn_clear_unused_Click" Text="Clear Unused GLs" ClientVisible="False">
                                </dx:ASPxButton>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btn_clear_unused1" runat="server"  OnClick="btn_clear__Click" Text="CLEAR ALL GLs for this Tax Entity" ClientVisible="False">
                                </dx:ASPxButton>
                            </td>
                            <td style="text-align: right">
                                <dx:ASPxButton ID="btn_copy_data_from_consol_div" runat="server" OnClick="btn_copy_bs_data_from_bv_Click" Text="SYNC balance Sheet Accounts" ClientVisible="False">
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                    <dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv" DataSourceID="sql_gl" KeyFieldName="id" OnCellEditorInitialize="gv_CellEditorInitialize" OnCommandButtonInitialize="gv_CommandButtonInitialize"  Theme="NETheme01" Width="100%" OnHtmlEditFormCreated="gv_HtmlEditFormCreated" OnRowDeleting="gv_RowDeleting" OnRowInserting="gv_RowInserting" OnRowUpdating="gv_RowUpdating">
                        <Columns>
                            <dx:GridViewCommandColumn ShowDeleteButton="False" ShowEditButton="False" ShowInCustomizationForm="True" ShowNewButtonInHeader="False" VisibleIndex="0" Caption=" ">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="id" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Visible="False">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="tax_entity_id" ShowInCustomizationForm="True" VisibleIndex="2" ReadOnly="True" CellStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">
                                <EditFormSettings Visible="False" />

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="account_no" ShowInCustomizationForm="True" VisibleIndex="3" PropertiesTextEdit-MaxLength="5" CellStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">
<PropertiesTextEdit MaxLength="5"></PropertiesTextEdit>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="gl_chart_name" ShowInCustomizationForm="True" VisibleIndex="4" PropertiesTextEdit-MaxLength="55">
<PropertiesTextEdit MaxLength="55"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn FieldName="type" ShowInCustomizationForm="True" VisibleIndex="5" ReadOnly="True" CellStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">
                                <PropertiesComboBox>
                                    <Items>
                                        <dx:ListEditItem Text="R" Value="R" />
                                        <dx:ListEditItem Text="X" Value="X" />
                                        <dx:ListEditItem Text="A" Value="A" />
                                        <dx:ListEditItem Text="L" Value="L" />
                                    </Items>
                                </PropertiesComboBox>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataComboBoxColumn>
                            
                            <dx:GridViewDataTextColumn FieldName="InitCheque" ShowInCustomizationForm="True" VisibleIndex="10" PropertiesTextEdit-MaxLength="10" CellStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">
<PropertiesTextEdit MaxLength="10"></PropertiesTextEdit>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="gl_comments" ShowInCustomizationForm="True" VisibleIndex="11" PropertiesTextEdit-MaxLength="255">
<PropertiesTextEdit MaxLength="255"></PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn FieldName="gl_group_id" ShowInCustomizationForm="True" VisibleIndex="6">
                                <PropertiesComboBox DataSourceID="sql_groups" TextField="group_name" ValueField="id" ValueType="System.Int32">
                                </PropertiesComboBox>
                                <EditItemTemplate>
                                    <dx:ASPxComboBox ID="ddl_group_edit" runat="server" TextField="name" Theme="NETheme01"  ValueField="id" ValueType="System.Int32">
                                    </dx:ASPxComboBox>
                                </EditItemTemplate>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataComboBoxColumn FieldName="currency_id" ShowInCustomizationForm="True" VisibleIndex="13" CellStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center">
                                <PropertiesComboBox DataSourceID="sql_currency" TextField="currency" ValueField="id" ValueType="System.Int32">
                                </PropertiesComboBox>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataCheckColumn FieldName="is_active" ShowInCustomizationForm="True" VisibleIndex="12">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="is_sales" ShowInCustomizationForm="True" VisibleIndex="9" ReadOnly="True">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="is_bank" ShowInCustomizationForm="True" VisibleIndex="7"  ReadOnly="True">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn Caption="Has History" ShowInCustomizationForm="True" VisibleIndex="14">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataComboBoxColumn FieldName="GL_Designation" ShowInCustomizationForm="True" VisibleIndex="8" CellStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" ReadOnly="True">
                                <PropertiesComboBox>
                                    <Items>
                                        <dx:ListEditItem Text="D" Value="D" />
                                        <dx:ListEditItem Text="C" Value="C" />
                                    </Items>
                                </PropertiesComboBox>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataCheckColumn FieldName="is_mileage" ShowInCustomizationForm="True" VisibleIndex="15">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataCheckColumn FieldName="see_on_po_gl_list" ShowInCustomizationForm="True" VisibleIndex="16">
                            </dx:GridViewDataCheckColumn>
                              <dx:GridViewDataTextColumn FieldName="netsuite_gl_id" ShowInCustomizationForm="True" VisibleIndex="17">
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsPager Mode="ShowAllRecords">
                        </SettingsPager>
                        <SettingsEditing Mode="PopupEditForm">
                        </SettingsEditing>
                        <Settings ShowGroupPanel="True" />
                        <SettingsPopup>
                            <EditForm HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" />
                        </SettingsPopup>
                        <Styles>
                            <Header Wrap="True">
                            </Header>
                        </Styles>
                    </dx:ASPxGridView>
                    <br />
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
        <dx:TabPage Name="Special Accounts" Text="Special Accounts">
            <ContentCollection>
                <dx:ContentControl runat="server">
                    <dx:ASPxGridView ID="gv_sa" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_sa" KeyFieldName="id" Theme="NETheme01" Width="100%" OnCustomCallback="gv_sa_CustomCallback">
                        <Columns>
                            <dx:GridViewCommandColumn Caption=" " ShowClearFilterButton="True" ShowInCustomizationForm="True" VisibleIndex="0">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn Caption="id" FieldName="id" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Special Account" FieldName="name" ShowInCustomizationForm="True" VisibleIndex="2">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Type" FieldName="_type" ShowInCustomizationForm="True" VisibleIndex="4">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Description" FieldName="description" ShowInCustomizationForm="True" VisibleIndex="3">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn Caption="GL Account" FieldName="gl_te_id" ShowInCustomizationForm="True" VisibleIndex="5">
                                <PropertiesComboBox TextField="account_no" ValueField="gl_te_id">
                                </PropertiesComboBox>
                                <DataItemTemplate>
                                    <dx:ASPxComboBox ID="ddl_gl" runat="server" OnInit="ddl_gl_Init" TextField="_name" Value='<%# Eval("gl_te_id") %>' ValueField="id" ValueType="System.Int32" Width="100%" Enabled="False">
                                    </dx:ASPxComboBox>
                                </DataItemTemplate>
                            </dx:GridViewDataComboBoxColumn>
                        </Columns>
                        <SettingsPager Mode="ShowAllRecords">
                        </SettingsPager>
                        <SettingsEditing Mode="EditForm">
                        </SettingsEditing>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
                    </dx:ASPxGridView>
                    <asp:SqlDataSource ID="sql_gl_A" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select 0 id, 'NONE' _name

union

(
SELECT
	gl_te.id,
	Concat(
		gl_te.account_no,
		'-',
		gl_te.gl_chart_name
	) _name
FROM
	gl_te inner join gl_group_te gg on gg.id = gl_te.gl_group_id
WHERE
	gg.type = 'A'
AND gl_te.tax_entity_id = ?te_id order BY gl_chart_name)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="te_id" PropertyName="Value" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sql_gl_L" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select 0 id, 'NONE' _name

union

(SELECT
	gl_te.id,
	Concat(
		gl_te.account_no,
		'-',
		gl_te.gl_chart_name
	) _name
FROM
	gl_te inner join gl_group_te gg on gg.id = gl_te.gl_group_id
WHERE
	gg.type = 'L'
AND gl_te.tax_entity_id = ?te_id order BY gl_chart_name)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="te_id" PropertyName="Value" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sql_gl_R" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select 0 id, 'NONE' _name

union

(SELECT
	gl_te.id,
	Concat(
		gl_te.account_no,
		'-',
		gl_te.gl_chart_name
	) _name
FROM
	gl_te inner join gl_group_te gg on gg.id = gl_te.gl_group_id
WHERE
	gg.type = 'R'
AND gl_te.tax_entity_id = ?te_id order BY gl_chart_name)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="te_id" PropertyName="Value" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="sql_gl_X" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select 0 id, 'NONE' _name

union

(SELECT
	gl_te.id,
	Concat(
		gl_te.account_no,
		'-',
		gl_te.gl_chart_name
	) _name
FROM
	gl_te inner join gl_group_te gg on gg.id = gl_te.gl_group_id
WHERE
	gg.type = 'X'
AND gl_te.tax_entity_id = ?te_id order BY gl_chart_name)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdn_tax_ent_it" Name="te_id" PropertyName="Value" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
    </TabPages>
</dx:ASPxPageControl>
<dx:ASPxPopupControl ID="pop_frombv_groups" runat="server" HeaderText="GL Groups found in BV" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="700px" ScrollBars="Auto" Width="900px">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <div id="div_groups_inbv" runat="server">
        
            </div><dx:ASPxButton ID="btn_gomerge_frombv0" runat="server" Text="GO-&gt;" OnClick="btn_gomerge_frombv_groupsClick">
            </dx:ASPxButton>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<br />
<br />
<br />
<dx:ASPxPopupControl ID="pop_frombv" runat="server" HeaderText="GL Accounts found in BV" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="700px" ScrollBars="Auto" Width="900px">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <div id="div_inbv" runat="server">
        
            </div><dx:ASPxButton ID="btn_gomerge_frombv" runat="server" Text="GO-&gt;" OnClick="btn_gomerge_frombv_Click">
            </dx:ASPxButton>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

</asp:Content>



<asp:Content ID="Content5" runat="server" contentplaceholderid="header_placeholder">
    <style type="text/css">
        .auto-style1 {
            height: 18px;
        }
    </style>
</asp:Content>





