<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartialBilling.aspx.cs" Inherits="Nesi.Web.sections.reports.customer_reports.PartialBilling" MasterPageFile="~/IntraDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register src="~/modules/layout_control.ascx" tagname="layout_control" tagprefix="uc" %>


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
		function TestDates()
			{
			var testDates = dateStart.GetDate() < dateEnd.GetDate();
			if (!testDates)
				{
				alert("The start date cannot be greater than the end date.");
				}
			return testDates;
			}
		function TestForm(s, e)
			{
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
            <asp:SqlDataSource  ID="sdsBusinessUnit" 
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
            <asp:SqlDataSource  ID="sdsProjMan" 
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
                    <dx:GridViewDataTextColumn Caption="SparkOps WO" FieldName="wo_description" VisibleIndex="4" GroupIndex="0" >
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Activity Code" FieldName="activity_code" VisibleIndex="5" GroupIndex="1">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Group Type" FieldName="group_type" VisibleIndex="6" GroupIndex="2">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Client PO" FieldName="client_po" VisibleIndex="7">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Cost Element" FieldName="cost_element" VisibleIndex="8">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="WO #" FieldName="client_wo" VisibleIndex="9">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Item/Service Description" FieldName="line_description" VisibleIndex="10">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Quantity" FieldName="qty" VisibleIndex="11">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Price" FieldName="price" VisibleIndex="12">
                        <PropertiesTextEdit DisplayFormatString="{0:C2}">
                        </PropertiesTextEdit>
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Row Total" FieldName="total" VisibleIndex="13">
                        <PropertiesTextEdit DisplayFormatString="{0:C2}">
                        </PropertiesTextEdit>
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Employee" FieldName="employee" VisibleIndex="14">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="Date" FieldName="line_date" VisibleIndex="15">
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="line_id" VisibleIndex="0" Caption="Line ID">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Internal ID" FieldName="internal_id" VisibleIndex="16">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Scope" FieldName="scope" VisibleIndex="17">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Markup" FieldName="markup" VisibleIndex="19">
                    	<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Markup Total" FieldName="markup_total" VisibleIndex="20">
                    	<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Customer" FieldName="customer_name" VisibleIndex="2">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Project Manager" FieldName="pm" VisibleIndex="1">
                    </dx:GridViewDataTextColumn>
                	<dx:GridViewDataTextColumn Caption="Markup %" FieldName="markup_pct" VisibleIndex="18">
						<PropertiesTextEdit DisplayFormatString="P2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Total w/ Markup" FieldName="total_w_markup" VisibleIndex="21">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Customer Ref #" FieldName="customer_ref" VisibleIndex="3">
					</dx:GridViewDataTextColumn>
                </Columns>

                <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True" ShowHeaderFilterButton="True" ShowFilterRowMenu="True" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="total_w_markup" SummaryType="Sum" DisplayFormat="c" />
                    <dx:ASPxSummaryItem FieldName="total" SummaryType="Sum" DisplayFormat="c" />
                    <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" DisplayFormat="n" />
                </TotalSummary>

                <GroupSummary>
                    <dx:ASPxSummaryItem FieldName="total" SummaryType="Sum" DisplayFormat="c" />
                    <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" DisplayFormat="n"   />

                    <dx:ASPxSummaryItem FieldName="total" SummaryType="Sum" DisplayFormat="c" ShowInGroupFooterColumn="Row Total" />
                    <dx:ASPxSummaryItem FieldName="qty" SummaryType="Sum" DisplayFormat="n" ShowInGroupFooterColumn="Quantity"   />
                    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="markup" ShowInGroupFooterColumn="Markup" SummaryType="Sum" />
                    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="markup_total" ShowInGroupFooterColumn="Markup Total" SummaryType="Sum" />
                    <dx:ASPxSummaryItem DisplayFormat="c" FieldName="total_w_markup" ShowInGroupFooterColumn="Total w/ Markup" SummaryType="Sum" />
                </GroupSummary>

                <Styles>
                    <GroupFooter HorizontalAlign="Center">
                    </GroupFooter>
                </Styles>

            </dx:ASPxGridView>
            <asp:SqlDataSource ID="sdsPartialBilling" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="CALL rpt_partialbilling(@bu, @customer_id, @project_manager, @datestart, @dateend);">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlBusinessUnit" Name="@bu" PropertyName="Value" />
                    <asp:ControlParameter ControlID="ddlCustomer" Name="@customer_id" PropertyName="Value" DefaultValue="0" />
                    <asp:ControlParameter ControlID="dateStart" Name="@datestart" PropertyName="Value" />
                    <asp:ControlParameter ControlID="dateEnd" Name="@dateend" PropertyName="Value" />
                    <asp:ControlParameter ControlID="ddlProjManager" Name="@project_manager" PropertyName="Value" />
                </SelectParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
