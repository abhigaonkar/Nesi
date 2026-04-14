<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartialBillingWithDetailCost.aspx.cs" Inherits="Nesi.Web.sections.reports.customer_reports.PartialBillingWithDetailCost" MasterPageFile="~/IntraDefault.master" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/modules/layout_control.ascx" TagName="layout_control" TagPrefix="uc" %>


<asp:Content ContentPlaceHolderID="header_placeholder" runat="server">
    <style type="text/css">
        .floatFix {
            float: left;
            margin-right: 10px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            page_obj.update_panel_progress.bind();
        });
        function TestDates() {
            var testDates = dateStart.GetDate() < dateEnd.GetDate();
            if (!testDates) {
                alert("The start date cannot be greater than the end date.");
            }
            return testDates;
        }
        function TestForm(s, e) {
            e.processOnServer = ASPxClientEdit.ValidateGroup('p') && TestDates();
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphMasterBody" runat="server">
    <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
    <uc:layout_control ID="layout" runat="server" GridviewID="gvPartialBilling" />
    <br />
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <br />
            <dx:ASPxComboBox ID="ddlBusinessUnit" runat="server" Caption="Business Unit:" Theme="MaterialCompact" DataSourceID="sdsBusinessUnit" TextField="ddl_name" ValueField="id" ValueType="System.Int32" AutoPostBack="True" OnSelectedIndexChanged="ddlBusinessUnit_SelectedIndexChanged" Width="250px">
                <ValidationSettings ValidationGroup="p">
                    <RequiredField IsRequired="True" />
                </ValidationSettings>
                <CaptionCellStyle Width="150px">
                </CaptionCellStyle>
            </dx:ASPxComboBox>
            <asp:SqlDataSource ID="sdsBusinessUnit"
                runat="server"
                ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                SelectCommand="
SELECT 
	id, 
	ddl_name 
FROM 
	business_unit 
WHERE 
	active = ? 
ORDER BY 
	ddl_name">
                <SelectParameters>
                    <asp:Parameter DefaultValue="T" Name="Active" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <dx:ASPxComboBox ID="ddlCustomer" runat="server" Caption="Customer" DataSourceID="sdsCustomer" EnableTheming="True" TextField="customer_name" Theme="MaterialCompact" ValueField="customer_id" ValueType="System.Int32" Width="250px">
                <ValidationSettings CausesValidation="True" ValidationGroup="p">
                    <RequiredField IsRequired="True" />
                </ValidationSettings>
                <CaptionCellStyle Width="150px">
                </CaptionCellStyle>
            </dx:ASPxComboBox>
            <asp:SqlDataSource ID="sdsCustomer" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT 
	0 customer_id, 
	'All' customer_name, 
	0 order_id 
UNION ALL
SELECT
	DISTINCT b.customer_id,
	b.customer_name,
	1 order_id
FROM
	customer_business_unit a
LEFT JOIN 
	customer b ON a.customer_id = b.customer_id
INNER JOIN 
	woprog c ON b.customer_Id = c.woprog_customer_id
WHERE 
	a.business_unit_id = @business_unit_id
ORDER BY 
	order_id,customer_name">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlBusinessUnit" Name="@business_unit_id" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <dx:ASPxComboBox ID="ddlProjManager" runat="server" Caption="Project Manager" DataSourceID="sdsProjMan" EnableTheming="True" TextField="name" Theme="MaterialCompact" ValueField="id" ValueType="System.Int32" Width="250px">
                <ValidationSettings CausesValidation="True" ValidationGroup="p">
                    <RequiredField IsRequired="True" />
                </ValidationSettings>
                <CaptionCellStyle Width="150px">
                </CaptionCellStyle>
            </dx:ASPxComboBox>
            <asp:SqlDataSource ID="sdsProjMan"
                runat="server"
                ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                SelectCommand="
                SELECT 
					0 id, 
					'All' name, 
					0 order_id
			UNION ALL 
                SELECT 
					member_id id,
					name, 
					1 order_id 
                FROM 
					vw_activepms 
                WHERE 
					business_unit_id=@business_unit_id">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlBusinessUnit" Name="@business_unit_id" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <dx:ASPxDateEdit ID="dateStart" runat="server" ClientInstanceName="dateStart" Caption="Date Start" Theme="MaterialCompact" Width="250px">
                <ValidationSettings CausesValidation="True" ValidationGroup="p">
                    <RequiredField IsRequired="True" />
                </ValidationSettings>
                <CaptionCellStyle Width="150px">
                </CaptionCellStyle>
            </dx:ASPxDateEdit>
            <br />
            <dx:ASPxDateEdit ID="dateEnd" runat="server" ClientInstanceName="dateEnd" Caption="Date End" Theme="MaterialCompact" Width="250px">
                <ValidationSettings CausesValidation="True" ValidationGroup="p">
                    <RequiredField IsRequired="True" />
                </ValidationSettings>
                <CaptionCellStyle Width="150px">
                </CaptionCellStyle>
            </dx:ASPxDateEdit>
            <br />

            <div class="control">
                <asp:SqlDataSource ID="sdstatus" runat="server"
                    ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
                    ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
                    SelectCommand="SELECT id ,name FROM vw_wo_status_partial_cost;"></asp:SqlDataSource>
                <dx:ASPxCheckBoxList ID="cl_wostatus" runat="server"
                    DataSourceID="sdstatus"
                    TextField="name"
                    ValueField="name"
                    ValueType="System.String"
                    Theme="MaterialCompact"
                    RepeatColumns="3"
                    SelectionMode="Multiple"
                    Width="100%"
                    Caption="Wo Status">
                    <ValidationSettings CausesValidation="True" ValidationGroup="p">
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                    <CaptionCellStyle Width="150px">
                    </CaptionCellStyle>
                </dx:ASPxCheckBoxList>

                <dx:ASPxHiddenField ID="hstatus" runat="server" />

                <br />

                <dx:ASPxButton ID="btSubmit" runat="server" Text="Submit" Theme="MaterialCompact" OnClick="btSubmit_Click" CssClass="floatFix">
                    <ClientSideEvents Click="TestForm"></ClientSideEvents>
                </dx:ASPxButton>
                <dx:ASPxButton ID="btExpand" runat="server" OnClick="btExpand_Click" Text="Expand Grid" Theme="MaterialCompact" CssClass="floatFix">
                </dx:ASPxButton>
                <br />
                <br />
                <br />
                <dx:ASPxGridView runat="server" ID="gvPartialBilling" Theme="MaterialCompact" ClientInstanceName="gvPartialBilling" AutoGenerateColumns="False" DataSourceID="sdsPartialBilling" KeyFieldName="line_id" Style="margin-right: 0px" Width="100%" OnSummaryDisplayText="gvPartialBilling_SummaryDisplayText" OnHtmlRowCreated="gvPartialBilling_HtmlRowCreated" OnCustomCallback="gvPartialBilling_CustomCallback" OnCustomJSProperties="gvPartialBilling_CustomJSProperties">

                    <SettingsCustomizationDialog Enabled="True" />

                    <SettingsPager PageSize="200">
                    </SettingsPager>
                    <SettingsBehavior EnableRowHotTrack="True" />
                    <SettingsResizing ColumnResizeMode="Control" />
                    <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />

                    <SettingsPopup>
                        <HeaderFilter MinHeight="140px">
                        </HeaderFilter>
                    </SettingsPopup>

                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Project Manager" FieldName="pm" VisibleIndex="0">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="SparkOps WO" FieldName="wo_description" VisibleIndex="3" GroupIndex="0">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Activity Code" FieldName="activity_code" VisibleIndex="4" GroupIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Group Type" FieldName="group_type" VisibleIndex="5" GroupIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="WO #" FieldName="client_wo" VisibleIndex="6">
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Item/Service Description" FieldName="line_description" VisibleIndex="7">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Quantity" FieldName="qty" VisibleIndex="8">
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Unit Sell Price" FieldName="price" VisibleIndex="9">
                            <PropertiesTextEdit DisplayFormatString="{0:C2}">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Ext'd Sell Price" FieldName="total" VisibleIndex="10">
                            <PropertiesTextEdit DisplayFormatString="{0:C2}">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Unit Cost Price" FieldName="cost" VisibleIndex="11">
                            <PropertiesTextEdit DisplayFormatString="{0:C2}">
                            </PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Ext'd Cost Price" FieldName="extended_unit_cost" VisibleIndex="12">
                            <PropertiesTextEdit DisplayFormatString="{0:C2}"></PropertiesTextEdit>
                            <CellStyle HorizontalAlign="Center"></CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Employee" FieldName="employee" VisibleIndex="13">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Date" FieldName="line_date" VisibleIndex="14">
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="Internal ID" FieldName="internal_id" VisibleIndex="15">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Scope" FieldName="scope" VisibleIndex="16">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Markup" FieldName="markup" VisibleIndex="18">
                            <PropertiesTextEdit DisplayFormatString="C2">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Markup Total" FieldName="markup_total" VisibleIndex="19">
                            <PropertiesTextEdit DisplayFormatString="C2">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Customer" FieldName="customer_name" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Markup %" FieldName="markup_pct" VisibleIndex="17">
                            <PropertiesTextEdit DisplayFormatString="P2">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Total w/ Markup" FieldName="total_w_markup" VisibleIndex="19">
                            <PropertiesTextEdit DisplayFormatString="C2">
                            </PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Customer Ref #" FieldName="customer_ref" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Wo Status" FieldName="wo_status" VisibleIndex="20">
                        </dx:GridViewDataTextColumn>

                    </Columns>

                    <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowHeaderFilterButton="True" ShowFilterRowMenu="True" />
                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="total_w_markup" SummaryType="Sum" DisplayFormat="c" />
                        <dx:ASPxSummaryItem FieldName="total" SummaryType="Sum" DisplayFormat="c" />
                        <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" DisplayFormat="n" />
                        <dx:ASPxSummaryItem FieldName="extended_unit_cost" SummaryType="Sum" DisplayFormat="c" />
                    </TotalSummary>

                    <GroupSummary>
                        <dx:ASPxSummaryItem FieldName="total" SummaryType="Sum" DisplayFormat="c" />
                        <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" DisplayFormat="n" />
                        <dx:ASPxSummaryItem FieldName="extended_unit_cost" SummaryType="Sum" DisplayFormat="c" />
                        <dx:ASPxSummaryItem FieldName="total" SummaryType="Sum" DisplayFormat="c" ShowInGroupFooterColumn="Row Total" />
                        <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" DisplayFormat="n" ShowInGroupFooterColumn="Quantity" />
                        <dx:ASPxSummaryItem FieldName="extended_unit_cost" SummaryType="Sum" DisplayFormat="c" ShowInGroupFooterColumn="Ext'd Unit Cost"/>
                        <dx:ASPxSummaryItem DisplayFormat="c" FieldName="markup" ShowInGroupFooterColumn="Markup" SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="c" FieldName="markup_total" ShowInGroupFooterColumn="Markup Total" SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="c" FieldName="total_w_markup" ShowInGroupFooterColumn="Total w/ Markup" SummaryType="Sum" />
                     
                    </GroupSummary>

                    <Styles>
                        <GroupFooter HorizontalAlign="Center">
                        </GroupFooter>
                    </Styles>

                </dx:ASPxGridView>
                <asp:SqlDataSource ID="sdsPartialBilling" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL sp_master_wo_cost_report_detail(@bu, @customer_id, @project_manager, @datestart, @dateend,@wostatus);">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlBusinessUnit" Name="@bu" PropertyName="Value" />
                        <asp:ControlParameter ControlID="ddlCustomer" Name="@customer_id" PropertyName="Value" DefaultValue="0" />
                        <asp:ControlParameter ControlID="dateStart" Name="@datestart" PropertyName="Value" />
                        <asp:ControlParameter ControlID="dateEnd" Name="@dateend" PropertyName="Value" />
                        <asp:ControlParameter ControlID="ddlProjManager" Name="@project_manager" PropertyName="Value" />
                        <%--<asp:ControlParameter ControlID="cl_wostatus" Name="@wostatus" PropertyName="Value" DefaultValue="Invoiced" />--%>
                    </SelectParameters>
                </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
