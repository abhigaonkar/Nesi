<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="tax_entities_business_units" Title="Business_units" Codebehind="business_units.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    
    
    
    <asp:HiddenField ID="hdn_id" runat="server" Value="1" />
    <asp:SqlDataSource ID="sql_bu" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" DeleteCommand="DELETE FROM [business_unit] WHERE [ID] = ?" 
        InsertCommand="INSERT INTO 
        [business_unit] 
        ( [Name], [Description], [IsTest], [address], [city], [Prov], [state], [postal], [Country], 
        [PhoneArea], [PhoneFirst], [PhoneLast], [PhoneAltArea], [PhoneAltFirst], [PhoneAltLast], [FaxArea], [FaxFirst], [FaxLast], [Member_ID],
         [DateTime], [Ord], [wopath], [popath], [rootdirectory], [pathtimesheet],  [workorderprinter], [laserprinter], [barcodeprinter], 
        [Active], [has_inventory], [EmailCollections], [EmailTimesheets], [EmailExpediteReport], [EmailIncomeStatement], [IsRegional_Branch], [region], [TaxLabour], [TaxQuotedJobs],
         [selfassess_tax], [tax_material_only_wos], [TaxMaterial], [fvr_lockout], [uses_quote_process], [quote_level_2_start], [quote_level_3_start], [mktphone_area], [mktphone_first], [mktphone_last], 
        [email_rush], [email_rfq], [email_service], [email_consult], [email_job], [use_benchmark_for_pm_comp], [show_ratepanel], [perdiem_rate], [default_material_sell_gl], [default_labour_sell_gl],
         [default_currency], [nesi_fee], [timezone_id], [tax_entity_id], [ddl_Name], [old_div], [old_company_id], [allow_open_scheduling], [gl_div],[logo_file],
        [allow_unlinked_timesheet],[is_service],[is_backoffice],[use_er_wo_logic],[acting_manager],[acting_purchaser],[acting_right_hand],[acting_safety_officer],
        [is_er],[is_panelshop],[warehouse_bu_id],[enable_timesheet],[is_corporate],[contractor_license],[masters_license],[ESA_id],[TSSA_id],[CSA_id],[allow_fixed_labour],[allow_fixed_markup],[allow_mixed_chargeouts],[netsuite_bu_internal_id]) 
        VALUES 
        (?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
         ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
         ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
         ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
         ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, 
         ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
         ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
         ?, ?, ?, ?, ?, ? ,?, ?, ?, ?,
         ?, ?, ?, ?, ?, ?, ?, ?, ?,?)" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
        SelectCommand="SELECT * FROM [business_unit] where tax_entity_id = ?id" 
        UpdateCommand="UPDATE [business_unit] SET [Name] = ?, [Description] = ?, [IsTest] = ?, [address] = ?, [city] = ?, [Prov] = ?, [state] = ?, [postal] = ?, 
        [Country] = ?, [PhoneArea] = ?, [PhoneFirst] = ?, [PhoneLast] = ?, [PhoneAltArea] = ?, [PhoneAltFirst] = ?, [PhoneAltLast] = ?, [FaxArea] = ?, [FaxFirst] = ?,
         [FaxLast] = ?, [Member_ID] = ?, [DateTime] = ?, [Ord] = ?, [wopath] = ?, [popath] = ?, [rootdirectory] = ?, [pathtimesheet] = ?,  [workorderprinter] = ?, 
        [laserprinter] = ?, [barcodeprinter] = ?, [Active] = ?, [has_inventory] = ?, [EmailCollections] = ?, [EmailTimesheets] = ?, [EmailExpediteReport] = ?, [EmailIncomeStatement] = ?, 
        [IsRegional_Branch] = ?, [region] = ?, [TaxLabour] = ?, [TaxQuotedJobs] = ?, [selfassess_tax] = ?, [tax_material_only_wos] = ?, [TaxMaterial] = ?, [fvr_lockout] = ?,
         [uses_quote_process] = ?, [quote_level_2_start] = ?, [quote_level_3_start] = ?, [mktphone_area] = ?, [mktphone_first] = ?, [mktphone_last] = ?, [email_rush] = ?, 
        [email_rfq] = ?, [email_service] = ?, [email_consult] = ?, [email_job] = ?, [use_benchmark_for_pm_comp] = ?, [show_ratepanel] = ?, [perdiem_rate] = ?, 
        [default_material_sell_gl] = ?, [default_labour_sell_gl] = ?, [default_currency] = ?, [nesi_fee] = ?, [timezone_id] = ?, [tax_entity_id] = ?, [ddl_Name] = ?, 
        [old_div] = ?, [old_company_id] = ?, [allow_open_scheduling] = ?, [gl_div] = ?, [logo_file]=?, [allow_unlinked_timesheet]=? ,
        [is_service]= ?,[is_backoffice]= ?,[use_er_wo_logic]= ?,[acting_manager]= ?,[acting_purchaser]= ?,[acting_right_hand]= ?,[acting_safety_officer]= ?,
        [is_er]=?, [is_panelshop]=?, [warehouse_bu_id]=?, [enable_timesheet]=?, [is_corporate]=?,
        [contractor_license]=?, [masters_license]=?, [ESA_id]=?, [TSSA_id]=?, [CSA_id]=?, [allow_fixed_labour]=?,[allow_fixed_markup]=?, [allow_mixed_chargeouts] = ?,[netsuite_bu_internal_id]=?
        WHERE [ID] = ?">
  
          <DeleteParameters>
              <asp:Parameter Name="ID" Type="Int32" />
           
        </DeleteParameters>
        <InsertParameters>
           
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="IsTest" Type="String" DefaultValue="F" />
            <asp:Parameter Name="address" Type="String" />
            <asp:Parameter Name="city" Type="String" />
            <asp:Parameter Name="Prov" Type="String" />
            <asp:Parameter Name="state" Type="String" />
            <asp:Parameter Name="postal" Type="String" />
            <asp:Parameter Name="Country" Type="String" />
            <asp:Parameter Name="PhoneArea" Type="String" />
            <asp:Parameter Name="PhoneFirst" Type="String" />
            <asp:Parameter Name="PhoneLast" Type="String" />
            <asp:Parameter Name="PhoneAltArea" Type="String" />
            <asp:Parameter Name="PhoneAltFirst" Type="String" />
            <asp:Parameter Name="PhoneAltLast" Type="String" />
            <asp:Parameter Name="FaxArea" Type="String" />
            <asp:Parameter Name="FaxFirst" Type="String" />
            <asp:Parameter Name="FaxLast" Type="String" />
            <asp:Parameter Name="Member_ID" Type="Int32" />
            <asp:Parameter Name="DateTime" Type="DateTime" />
            <asp:Parameter Name="Ord" Type="Int32" />
            <asp:Parameter Name="wopath" Type="String" />
            <asp:Parameter Name="popath" Type="String" />
            <asp:Parameter Name="rootdirectory" Type="String" />
            <asp:Parameter Name="pathtimesheet" Type="String" />
          
            <asp:Parameter Name="workorderprinter" Type="String" />
            <asp:Parameter Name="laserprinter" Type="String" />
            <asp:Parameter Name="barcodeprinter" Type="String" />
            <asp:Parameter Name="Active" Type="String" DefaultValue="T" />
            <asp:Parameter Name="has_inventory" Type="Int32" DefaultValue="1" />
            <asp:Parameter Name="EmailCollections" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="EmailTimesheets" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="EmailExpediteReport" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="EmailIncomeStatement" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="IsRegional_Branch" Type="String"  DefaultValue="False" />
            <asp:Parameter Name="region" Type="String" />
            <asp:Parameter Name="TaxLabour" Type="Int32" DefaultValue="1"/>
            <asp:Parameter Name="TaxQuotedJobs" Type="Int32" DefaultValue="1" />
            <asp:Parameter Name="selfassess_tax" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="tax_material_only_wos" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="TaxMaterial" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="fvr_lockout" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="uses_quote_process" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="quote_level_2_start" Type="Double" DefaultValue="150000" />
            <asp:Parameter Name="quote_level_3_start" Type="Double" DefaultValue="500000" />
            <asp:Parameter Name="mktphone_area" Type="Int32" />
            <asp:Parameter Name="mktphone_first" Type="Int32" />
            <asp:Parameter Name="mktphone_last" Type="Int32" />
            <asp:Parameter Name="email_rush" Type="String" />
            <asp:Parameter Name="email_rfq" Type="String" />
            <asp:Parameter Name="email_service" Type="String" />
            <asp:Parameter Name="email_consult" Type="String" />
            <asp:Parameter Name="email_job" Type="String" />
            <asp:Parameter Name="use_benchmark_for_pm_comp" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="show_ratepanel" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="perdiem_rate" Type="Double" DefaultValue="40" />
            <asp:Parameter Name="default_material_sell_gl" Type="String" />
            <asp:Parameter Name="default_labour_sell_gl" Type="String" />
            <asp:Parameter Name="default_currency" Type="Int32" DefaultValue="2"/>
            <asp:Parameter Name="nesi_fee" Type="Double" DefaultValue="0.06"/>
            <asp:Parameter Name="timezone_id" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="tax_entity_id" Type="Int32" />
            <asp:Parameter Name="ddl_Name" Type="String" />
            <asp:Parameter Name="old_div" Type="Int32" />
            <asp:Parameter Name="old_company_id" Type="Int32" />
            <asp:Parameter Name="allow_open_scheduling" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="gl_div" Type="String" />
            <asp:Parameter Name="logo_file" Type="String" />
            <asp:Parameter Name="allow_unlinked_timesheet" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="is_service" Type="Int16" DefaultValue="1" />
            <asp:Parameter Name="is_backoffice" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="use_er_wo_logic" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="acting_manager" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="acting_purchaser" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="acting_right_hand" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="acting_safety_officer" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="is_er" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="is_panelshop" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="warehouse_bu_id" Type="Int32" DefaultValue="0"  />
            <asp:Parameter Name="enable_timesheet" Type="Int16" DefaultValue="1" />
            <asp:Parameter Name="is_corporate" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="contractor_license" Type="String" />
             <asp:Parameter Name="masters_license" Type="String" />
             <asp:Parameter Name="ESA_id" Type="String" />
             <asp:Parameter Name="TSSA_id" Type="String" />
             <asp:Parameter Name="CSA_id" Type="String" />
            <asp:Parameter Name="allow_fixed_labour" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="allow_fixed_markup" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="allow_mixed_chargeouts" Type="Int16" DefaultValue="0" />
             <asp:Parameter Name="netsuite_bu_internal_id" Type="String" DefaultValue="  " />
            

        </InsertParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="hdn_id" Name="id" PropertyName="Value" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="IsTest" Type="String" />
            <asp:Parameter Name="address" Type="String" />
            <asp:Parameter Name="city" Type="String" />
            <asp:Parameter Name="Prov" Type="String" />
            <asp:Parameter Name="state" Type="String" />
            <asp:Parameter Name="postal" Type="String" />
            <asp:Parameter Name="Country" Type="String" />
            <asp:Parameter Name="PhoneArea" Type="String" />
            <asp:Parameter Name="PhoneFirst" Type="String" />
            <asp:Parameter Name="PhoneLast" Type="String" />
            <asp:Parameter Name="PhoneAltArea" Type="String" />
            <asp:Parameter Name="PhoneAltFirst" Type="String" />
            <asp:Parameter Name="PhoneAltLast" Type="String" />
            <asp:Parameter Name="FaxArea" Type="String" />
            <asp:Parameter Name="FaxFirst" Type="String" />
            <asp:Parameter Name="FaxLast" Type="String" />
            <asp:Parameter Name="Member_ID" Type="Int32" />
            <asp:Parameter Name="DateTime" Type="DateTime" />
            <asp:Parameter Name="Ord" Type="Int32" />
            <asp:Parameter Name="wopath" Type="String" />
            <asp:Parameter Name="popath" Type="String" />
            <asp:Parameter Name="rootdirectory" Type="String" />
            <asp:Parameter Name="pathtimesheet" Type="String" />
          
            <asp:Parameter Name="workorderprinter" Type="String" />
            <asp:Parameter Name="laserprinter" Type="String" />
            <asp:Parameter Name="barcodeprinter" Type="String" />
            <asp:Parameter Name="Active" Type="String" DefaultValue="T" />
            <asp:Parameter Name="has_inventory" Type="Int32" DefaultValue="1" />
            <asp:Parameter Name="EmailCollections" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="EmailTimesheets" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="EmailExpediteReport" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="EmailIncomeStatement" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="IsRegional_Branch" Type="String"  DefaultValue="False" />
            <asp:Parameter Name="region" Type="String" />
            <asp:Parameter Name="TaxLabour" Type="Int32" DefaultValue="1"/>
            <asp:Parameter Name="TaxQuotedJobs" Type="Int32" DefaultValue="1" />
            <asp:Parameter Name="selfassess_tax" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="tax_material_only_wos" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="TaxMaterial" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="fvr_lockout" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="uses_quote_process" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="quote_level_2_start" Type="Double" DefaultValue="150000" />
            <asp:Parameter Name="quote_level_3_start" Type="Double" DefaultValue="500000" />
            <asp:Parameter Name="mktphone_area" Type="Int32" />
            <asp:Parameter Name="mktphone_first" Type="Int32" />
            <asp:Parameter Name="mktphone_last" Type="Int32" />
            <asp:Parameter Name="email_rush" Type="String" />
            <asp:Parameter Name="email_rfq" Type="String" />
            <asp:Parameter Name="email_service" Type="String" />
            <asp:Parameter Name="email_consult" Type="String" />
            <asp:Parameter Name="email_job" Type="String" />
            <asp:Parameter Name="use_benchmark_for_pm_comp" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="show_ratepanel" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="perdiem_rate" Type="Double" DefaultValue="40" />
            <asp:Parameter Name="default_material_sell_gl" Type="String" />
            <asp:Parameter Name="default_labour_sell_gl" Type="String" />
            <asp:Parameter Name="default_currency" Type="Int32" DefaultValue="2"/>
            <asp:Parameter Name="nesi_fee" Type="Double" DefaultValue="0.06"/>
            <asp:Parameter Name="timezone_id" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="tax_entity_id" Type="Int32" />
            <asp:Parameter Name="ddl_Name" Type="String" />
            <asp:Parameter Name="old_div" Type="Int32" />
            <asp:Parameter Name="old_company_id" Type="Int32" />
            <asp:Parameter Name="allow_open_scheduling" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="gl_div" Type="String" />
            <asp:Parameter Name="logo_file" Type="String" />
            <asp:Parameter Name="allow_unlinked_timesheet" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="is_service" Type="Int16" DefaultValue="1" />
            <asp:Parameter Name="is_backoffice" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="use_er_wo_logic" Type="Int16" DefaultValue="0"/>
            <asp:Parameter Name="acting_manager" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="acting_purchaser" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="acting_right_hand" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="acting_safety_officer" Type="Int32" DefaultValue="0"/>
            <asp:Parameter Name="is_er" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="is_panelshop" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="warehouse_bu_id" Type="Int32" DefaultValue="0"  />
            <asp:Parameter Name="enable_timesheet" Type="Int16" DefaultValue="1" />
            <asp:Parameter Name="is_corporate" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="contractor_license" Type="String" />
             <asp:Parameter Name="masters_license" Type="String" />
             <asp:Parameter Name="ESA_id" Type="String" />
             <asp:Parameter Name="TSSA_id" Type="String" />
             <asp:Parameter Name="CSA_id" Type="String" />
            <asp:Parameter Name="allow_fixed_labour" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="allow_fixed_markup" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="allow_mixed_chargeouts" Type="Int16" DefaultValue="0" />
            <asp:Parameter Name="ID" Type="Int32" />
            <asp:Parameter Name="netsuite_bu_internal_id" Type="String" DefaultValue="  " />

        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_member" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" ></asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_gl" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_te" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id,ddl_name _name from tax_entity order by public_name" ></asp:SqlDataSource>
    <asp:SqlDataSource ID="sql_currency" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select id,currency curr from currency"></asp:SqlDataSource>
<asp:SqlDataSource ID="sql_bu2" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" ></asp:SqlDataSource>

    <dx:ASPxGridView ID="gv_bu" runat="server" AutoGenerateColumns="False" DataSourceID="sql_bu" KeyFieldName="ID" Theme="NETheme01" Width="100%" OnRowInserted="gv_bu_RowInserted" OnRowUpdated="gv_bu_RowUpdated" OnStartRowEditing="gv_bu_StartRowEditing">
        <Columns>
            <dx:GridViewCommandColumn ShowDeleteButton="False" ShowEditButton="True" ShowNewButtonInHeader="False" VisibleIndex="0">
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="2" ReadOnly="True">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Description" Visible="False" VisibleIndex="3">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="address" Visible="False" VisibleIndex="5" ReadOnly="True"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="city" Visible="False" VisibleIndex="6" ReadOnly="True"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Prov" Visible="False" VisibleIndex="7" ReadOnly="True"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="state" Visible="False" VisibleIndex="8"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="postal" Visible="False" VisibleIndex="9" ReadOnly="True"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PhoneArea" Visible="False" VisibleIndex="11"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PhoneFirst" Visible="False" VisibleIndex="12"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PhoneLast" Visible="False" VisibleIndex="13"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PhoneAltArea" Visible="False" VisibleIndex="14"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PhoneAltFirst" Visible="False" VisibleIndex="15"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PhoneAltLast" Visible="False" VisibleIndex="16"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaxArea" Visible="False" VisibleIndex="17"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaxFirst" Visible="False" VisibleIndex="18"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="FaxLast" Visible="False" VisibleIndex="19"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Member_ID" Visible="False" VisibleIndex="20"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="DateTime" Visible="False" VisibleIndex="21"> <EditFormSettings Visible="True" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Ord" Visible="False"  VisibleIndex="22"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="wopath" Visible="False"  VisibleIndex="23"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="popath" Visible="False" VisibleIndex="24"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="rootdirectory" Visible="False" VisibleIndex="25"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="pathtimesheet" Visible="False" VisibleIndex="26"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="workorderprinter" Visible="False" VisibleIndex="27"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="laserprinter" Visible="False" VisibleIndex="28"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="barcodeprinter" Visible="False" VisibleIndex="29"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="EmailCollections" Visible="False" VisibleIndex="32"> 
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="EmailTimesheets" Visible="False"  VisibleIndex="33"> 
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="EmailExpediteReport" Visible="False"  VisibleIndex="34"> 
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="EmailIncomeStatement" Visible="False" VisibleIndex="35"> 
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn FieldName="region" Visible="False" VisibleIndex="38"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="fvr_lockout" Visible="False" VisibleIndex="44"> <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="uses_quote_process" Visible="False" VisibleIndex="45"> <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn FieldName="quote_level_2_start" Visible="False" VisibleIndex="46"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="quote_level_3_start" Visible="False" VisibleIndex="47"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="mktphone_area" Visible="False" VisibleIndex="48"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="mktphone_first" Visible="False" VisibleIndex="49"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="mktphone_last" Visible="False" VisibleIndex="50"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="email_rush" Visible="False" VisibleIndex="51"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="email_rfq" Visible="False" VisibleIndex="52"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="email_service" Visible="False" VisibleIndex="53"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="email_consult" Visible="False" VisibleIndex="54"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="email_job" Visible="False" VisibleIndex="55"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="show_ratepanel" Visible="False" VisibleIndex="57"> <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn FieldName="perdiem_rate" Visible="False" VisibleIndex="58"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="nesi_fee" Visible="False" VisibleIndex="62"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="timezone_id" Visible="False" VisibleIndex="63"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ddl_Name" VisibleIndex="65" Caption="List Name"> 
                <PropertiesTextEdit>
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="old_div" Visible="False" VisibleIndex="66" ReadOnly="True" > <EditFormSettings Visible="True"  />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="old_company_id" Visible="False" VisibleIndex="67" ReadOnly="True"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="gl_div" VisibleIndex="69" ReadOnly="True"> 
                <PropertiesTextEdit>
                    <MaskSettings Mask="000" />
                    <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                        <RegularExpression ValidationExpression="\d{3}" />
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="logo_file" VisibleIndex="71"> <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn FieldName="allow_unlinked_timesheet"  VisibleIndex="36"> 
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataComboBoxColumn FieldName="default_material_sell_gl" Visible="False" VisibleIndex="59">
                <PropertiesComboBox DataSourceID="sql_gl" TextField="_name" ValueField="id">
                   
                </PropertiesComboBox>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="default_labour_sell_gl" Visible="False" VisibleIndex="60">
                <PropertiesComboBox DataSourceID="sql_gl" TextField="_name" ValueField="id">
                    
                </PropertiesComboBox>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn FieldName="default_currency" Visible="False" VisibleIndex="61" ReadOnly="True">
                <PropertiesComboBox DataSourceID="sql_currency" TextField="curr" ValueField="id">
                </PropertiesComboBox>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Tax Entity" FieldName="tax_entity_id" Visible="False" VisibleIndex="64" ReadOnly="True">
                <PropertiesComboBox DataSourceID="sql_te" TextField="_name" ValueField="id">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Acting Manager" FieldName="acting_manager" VisibleIndex="70" PropertiesComboBox-DataSourceID="sql_member" PropertiesComboBox-ValueField="id" PropertiesComboBox-TextField="_name">
                <PropertiesComboBox DataSourceID="sql_member" TextField="_name" ValueField="id" ValueType="System.Int32">
                </PropertiesComboBox>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Acting Purchaser" FieldName="acting_purchaser" Visible="False" VisibleIndex="72" ReadOnly="True">
                <PropertiesComboBox DataSourceID="sql_member" TextField="_name" ValueField="id">
                </PropertiesComboBox>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Acting Right Hand" FieldName="acting_right_hand" Visible="False" VisibleIndex="73">
                <PropertiesComboBox DataSourceID="sql_member" TextField="_name" ValueField="id">
                </PropertiesComboBox>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataComboBoxColumn Caption="Acting Safety Officer" FieldName="acting_safety_officer" Visible="False" VisibleIndex="75">
                <PropertiesComboBox DataSourceID="sql_member" TextField="_name" ValueField="id">
                </PropertiesComboBox>
                <EditFormSettings Visible="False" />
            </dx:GridViewDataComboBoxColumn>
            <dx:GridViewDataCheckColumn FieldName="IsTest" VisibleIndex="4">
                <PropertiesCheckEdit AllowGrayedByClick="False" ValueChecked="T" ValueType="System.Char" ValueUnchecked="F">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="Active" VisibleIndex="30" ReadOnly="True">
                <PropertiesCheckEdit ValueChecked="T" ValueType="System.Char" ValueUnchecked="F">
                    <ValidationSettings>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="has_inventory" Visible="False" VisibleIndex="31">
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="IsRegional_Branch" Visible="False" VisibleIndex="37">
                <PropertiesCheckEdit ValueChecked="True" ValueType="System.String" ValueUnchecked="False">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="TaxLabour" Visible="False" VisibleIndex="39">
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="TaxQuotedJobs" Visible="False" VisibleIndex="40">
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="selfassess_tax" Visible="False" VisibleIndex="41">
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="tax_material_only_wos" Visible="False" VisibleIndex="42">
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="TaxMaterial" Visible="False" VisibleIndex="43">
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="use_benchmark_for_pm_comp" Visible="False" VisibleIndex="56">
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn FieldName="allow_open_scheduling" Visible="False" VisibleIndex="68">
                <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                </PropertiesCheckEdit>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="Is Service" FieldName="is_service" Visible="False" VisibleIndex="74">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="Is BackOffice" FieldName="is_backoffice" Visible="False" VisibleIndex="77">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="Use ER WO Logic" FieldName="use_er_wo_logic" Visible="False" VisibleIndex="76" ReadOnly="True">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataComboBoxColumn FieldName="Country" Visible="False" VisibleIndex="10" ReadOnly="True">
                <PropertiesComboBox>
                    <Items>
                        <dx:ListEditItem Text="Canada" Value="CDN" />
                        <dx:ListEditItem Text="USA" Value="USA" />
                    </Items>
                </PropertiesComboBox>
                <EditFormSettings Visible="True" />
            </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataCheckColumn Caption="is_panelshop" FieldName="is_panelshop" Visible="False" VisibleIndex="78">
            <EditFormSettings Visible="True" />
        </dx:GridViewDataCheckColumn>
        <dx:GridViewDataCheckColumn Caption="is_er" FieldName="is_er" Visible="False" VisibleIndex="79" >
            <EditFormSettings Visible="True" />
        </dx:GridViewDataCheckColumn>
        <dx:GridViewDataComboBoxColumn Caption="warehouse_bu_id" FieldName="warehouse_bu_id" Visible="False" VisibleIndex="80">
            <PropertiesComboBox DataSourceID="sql_bu2" TextField="_name" ValueField="id">
            </PropertiesComboBox>
            <EditFormSettings Visible="True" />
        </dx:GridViewDataComboBoxColumn>
        <dx:GridViewDataCheckColumn Caption="enable_timesheet" FieldName="enable_timesheet" Visible="False" VisibleIndex="81" >
            <EditFormSettings Visible="True" />
        </dx:GridViewDataCheckColumn>
        <dx:GridViewDataCheckColumn Caption="is_corporate" FieldName="is_corporate" Visible="false" VisibleIndex="82" >
            <EditFormSettings Visible="True" />
        </dx:GridViewDataCheckColumn>
        <dx:GridViewDataTextColumn FieldName="contractor_license" Visible="False" VisibleIndex="83"> <EditFormSettings Visible="True" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="masters_license" Visible="False" VisibleIndex="84"> <EditFormSettings Visible="True" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ESA_id" Visible="False" VisibleIndex="85"> <EditFormSettings Visible="True" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="TSSA_id" Visible="False" VisibleIndex="86"> <EditFormSettings Visible="True" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="CSA_ID" Visible="False" VisibleIndex="87"> <EditFormSettings Visible="True" />
        </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn Caption="allow_fixed_labour" FieldName="allow_fixed_labour" Visible="false" VisibleIndex="88" >
            <EditFormSettings Visible="True" />
        </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="allow_fixed_markup" FieldName="allow_fixed_markup" Visible="false" VisibleIndex="89" >
            <EditFormSettings Visible="True" />
        </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="allow_mixed_chargeouts" FieldName="allow_mixed_chargeouts" Visible="false" VisibleIndex="90" >
            <EditFormSettings Visible="True" />
        </dx:GridViewDataCheckColumn>
            <dx:GridViewDataTextColumn Caption="netsuite_bu_internal_id" FieldName="netsuite_bu_internal_id" VisibleIndex="91"  Visible="true">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            

            <dx:GridViewDataTextColumn Caption="default_fvr_template_id" FieldName="default_fvr_template_id" VisibleIndex="92">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="default_onboarding_email" FieldName="default_onboarding_email" VisibleIndex="93">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="default_fvr_template_ids" FieldName="default_fvr_template_ids" VisibleIndex="94">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="remit_to_address" FieldName="remit_to_address" VisibleIndex="96">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="final_payroll_approver" FieldName="final_payroll_approver" VisibleIndex="97">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="vendor_request_email" FieldName="vendor_request_email" VisibleIndex="98">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="customer_request_email" FieldName="customer_request_email" VisibleIndex="99">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn Caption="is_NESIonly" FieldName="is_NESIonly" VisibleIndex="100">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="sync_customers_vendors_flag" FieldName="sync_customers_vendors_flag" VisibleIndex="101">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="sync_WOs_POs_flag" FieldName="sync_WOs_POs_flag" VisibleIndex="102">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="uses_payroll" FieldName="uses_payroll" VisibleIndex="103">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="allow_email_edit" FieldName="allow_email_edit" VisibleIndex="104">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            <dx:GridViewDataCheckColumn Caption="allow_cell_edit" FieldName="allow_cell_edit" VisibleIndex="105">
                <EditFormSettings Visible="True" />
            </dx:GridViewDataCheckColumn>
            

        </Columns>
        <SettingsPager Mode="ShowAllRecords">
        </SettingsPager>
    </dx:ASPxGridView>
    
</asp:Content>

