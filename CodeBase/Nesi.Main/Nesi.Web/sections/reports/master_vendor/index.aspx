<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true"  Inherits="sections_reports_master_vendor_index" Title="Vendor Listing" EnableTheming="true" Theme="NETheme01" Codebehind="index.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<%@ Register src="../../../modules/layout_control.ascx" tagname="layout_control" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <uc1:layout_control ID="layout" runat="server" />
   		<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
vendor.Vendor_ID,
vendor.vendor_number,
vendor.Vendor_Name,
if(vendor.Vendor_CPRS='T',true,false) CPRS,
vendor.Vendor_QC_DateTime AS qc_date,
vendor.Vendor_Active,
if(vendor.is_partner=1,true,false) is_partner,
vendor.vendor_notes,
(select ifnull((Select sum(poprog_header.poprog_total_recCost) from poprog_header where poprog_header.poprog_vendor_id = vendor.vendor_id),0)) AS total_purchase,
(select ifnull((Select sum(poprog_header.poprog_total_recCost) from poprog_header where poprog_header.poprog_vendor_id = vendor.vendor_id AND poprog_header.poprog_cutdate&gt;CURDATE()-INTERVAL 370 DAY),0)) AS last_12_months,
(select ifnull((Select COUNT(poprog_header.poprog_id) from poprog_header where poprog_header.ap_problem_last_notice_sent is not null AND (poprog_header.poprog_cutdate&gt;CURDATE()-INTERVAL 370 DAY) and poprog_header.poprog_vendor_id = vendor.vendor_id),0)) AS problems_12mo,
concat(address.Address_Addr1,', ',address.Address_Addr2) address,
address.Address_City,
address.Address_Prov,
address.Address_Postal,
address.Address_Country,
concat('(',address.Address_PhoneArea,') ',address.Address_PhoneFirst,'-',address.Address_PhoneLast) phone,
address.Address_Web,
vendor.nesi_member_id nesi_member_id
FROM
vendor
INNER JOIN address ON vendor.Vendor_ID = address.address_table_id AND address.address_table = 'Vendor'" 
               UpdateCommand="update vendor set Vendor_CPRS=?CPRS,vendor_notes=ifnull(?vendor_notes,''),is_partner=?is_partner, nesi_member_id = ?nesi_member_id where Vendor_ID=?Vendor_ID limit 1; 
               update address set Address_Web=ifnull(?Address_Web,'') where address.address_table = 'Vendor' and address.address_table_id=?Vendor_ID limit 1 
               ">
               <UpdateParameters>
		<asp:Parameter Name="CPRS" Type="String" />
		<asp:Parameter Name="Vendor_ID" Type="Int32" />
		<asp:Parameter Name="vendor_notes" Type="String" />
        <asp:Parameter Name="is_partner" Type="Boolean" />
        <asp:Parameter Name="Address_Web" Type="String" />
                   <asp:Parameter Name="nesi_member_id" Type="Int32" />
	</UpdateParameters>

   		</asp:SqlDataSource>
   		<asp:SqlDataSource ID="sql_nesi_members" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select member_id,member_fullname from member where business_unit_id = 11 and member_status = 'Active' order by member_fullname"></asp:SqlDataSource>
   		<dx:ASPxGridView ID="gv_vendors" runat="server" autogeneratecolumns="False" 
				ClientInstanceName="gv_vendors" Font-Names="Arial" 
				KeyFieldName="Vendor_ID" OnCustomCallback="gv_vendors_CustomCallback" 
				OnCustomJSProperties="gv_vendors_CustomJSProperties" 
				width="100%" SettingsPager-PageSize="50" DataSourceID="SqlDataSource1">
				<Columns>
                    <dx:GridViewDataTextColumn Caption="id" FieldName="Vendor_ID" VisibleIndex="0" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="BV ID" FieldName="vendor_number" VisibleIndex="1" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Name" FieldName="Vendor_Name" VisibleIndex="2" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                        <DataItemTemplate>
							<dx:ASPxHyperLink ID="hl_po" runat="server" 
								NavigateUrl="<%# string.Format(&quot;javascript:boing('/#/opens/11/vendors/{0}', 'vendor', 1035,800)&quot;, Eval(&quot;Vendor_ID&quot;)) %>" 
								Text='<%# Eval("Vendor_Name") %>'>
							</dx:ASPxHyperLink>
						</DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Active" FieldName="Vendor_Active" VisibleIndex="5" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Total Purchases" FieldName="total_purchase" VisibleIndex="8" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Last 12 Months" FieldName="last_12_months" VisibleIndex="9" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                        <PropertiesTextEdit DisplayFormatString="C2">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Problems Last 12 Months" FieldName="problems_12mo" VisibleIndex="10" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Address" FieldName="address" VisibleIndex="11" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="City" FieldName="Address_City" VisibleIndex="12" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="State/Prov" FieldName="Address_Prov" VisibleIndex="13" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Postal" FieldName="Address_Postal" VisibleIndex="14" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Country" FieldName="Address_Country" VisibleIndex="15" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Phone" FieldName="phone" VisibleIndex="16" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataCheckColumn FieldName="CPRS" VisibleIndex="3">
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataDateColumn Caption="QC Date" FieldName="qc_date" VisibleIndex="4" ReadOnly="True">
                        <EditFormSettings Visible="False" />
                        <PropertiesDateEdit DisplayFormatString="">
                        </PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataCheckColumn Caption="Is Partner" FieldName="is_partner" VisibleIndex="6">
                    </dx:GridViewDataCheckColumn>
                    <dx:GridViewDataMemoColumn Caption="Notes" FieldName="vendor_notes" VisibleIndex="7">
                    </dx:GridViewDataMemoColumn>
                  
                        <dx:GridViewDataTextColumn Caption="WWW" FieldName="Address_Web" 
														ShowInCustomizationForm="True" VisibleIndex="17" >
                    <DataItemTemplate>

															<dx:ASPxHyperLink ID="hl_name" runat="server" Theme="NETheme01" NavigateUrl="javascript:void();" OnInit="hl_web_Init" Text='<%# Eval("Address_Web") %>'>
															</dx:ASPxHyperLink>
														</DataItemTemplate>
                   </dx:GridViewDataTextColumn>
                    <dx:GridViewDataComboBoxColumn Caption="Nesi Account Manager" FieldName="nesi_member_id" VisibleIndex="18" Width="50px">
                        <PropertiesComboBox DataSourceID="sql_nesi_members" TextField="member_fullname" ValueField="member_id" ValueType="System.Int32">
                        </PropertiesComboBox>
                        <EditFormSettings Visible="True" />
                    </dx:GridViewDataComboBoxColumn>
                </Columns>
				<SettingsBehavior AutoExpandAllGroups="True" ColumnResizeMode="Control" 
					EnableRowHotTrack="True" EnableCustomizationWindow="True"/>
				<Styles>
					<Cell Wrap="False">
					</Cell>
				    <BatchEditCell BackColor="#FFFFCC">
                    </BatchEditCell>
				</Styles>
				<SettingsPager PageSize="50">
				</SettingsPager>
           

				<SettingsEditing Mode="Batch">
                </SettingsEditing>
           

				<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
					ShowGroupPanel="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" />

        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" 
					CustomizationWindow-VerticalAlign="TopSides" CustomizationWindow-Height="250px" 
					CustomizationWindow-HorizontalOffset="5" CustomizationWindow-VerticalOffset="5">


			<CustomizationWindow Height="250px" HorizontalAlign="LeftSides" 
				HorizontalOffset="5" VerticalAlign="TopSides" VerticalOffset="5" />
				</SettingsPopup>


			</dx:ASPxGridView>
    <p>
        <br />
    </p>
    <p>
    </p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Visible="false" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
</asp:Content>

