<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="credit_cards" Title="Credit Cards" EnableTheming="True" Codebehind="credit_cards.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
    <div id="divMenu" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
    <table style="width: 100%;">
		<tr>
			<td>
				<dx:ASPxGridView ID="gv" runat="server" AutoGenerateColumns="False" 
					DataSourceID="sds_credit_cards" KeyFieldName="id" Width="100%" ClientInstanceName="gv" 
					Theme="MaterialCompact" OnRowDeleted="gv_RowDeleted" OnRowInserted="gv_RowInserted" OnRowInserting="gv_RowInserting" >
					<Columns>
						<dx:GridViewCommandColumn ShowDeleteButton="True" ShowEditButton="True" 
							ShowNewButtonInHeader="True" VisibleIndex="0" ShowClearFilterButton="True" Width="70px">
						    <CellStyle Wrap="False">
                            </CellStyle>
						</dx:GridViewCommandColumn>
						<dx:GridViewDataTextColumn FieldName="id"  Visible="False" 
							VisibleIndex="1">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Bank" FieldName="bank" VisibleIndex="3">
						    <Settings HeaderFilterMode="CheckedList" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataDateColumn Caption="Date Issued" FieldName="date_issued" 
							VisibleIndex="4">
							<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
							</PropertiesDateEdit>
						</dx:GridViewDataDateColumn>
						<dx:GridViewDataDateColumn Caption="Expiry" FieldName="date_of_expiry" 
							VisibleIndex="8">
							<PropertiesDateEdit DisplayFormatString="yyyy-MM" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
							</PropertiesDateEdit>
						</dx:GridViewDataDateColumn>
						<dx:GridViewDataTextColumn Caption="Last Four" PropertiesTextEdit-MaxLength="4" FieldName="number" VisibleIndex="9">
							<PropertiesTextEdit MaxLength="4"></PropertiesTextEdit>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Credit Limit" FieldName="credit_limit" 
							VisibleIndex="11">
							<PropertiesTextEdit DisplayFormatString="C2">
							</PropertiesTextEdit>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataComboBoxColumn Caption="Issued To" FieldName="member_id" 
							VisibleIndex="5">
							<PropertiesComboBox DataSourceID="sds_Issued_To_existing" TextField="member_fullname" 
								ValueField="member_id" ValueType="System.Int32">
							</PropertiesComboBox>
						</dx:GridViewDataComboBoxColumn>
						<dx:GridViewDataComboBoxColumn Caption="Issued By" FieldName="issued_by" 
							VisibleIndex="6">
							<PropertiesComboBox DataSourceID="sds_Issued_By" TextField="member_fullname" 
								ValueField="member_id" ValueType="System.Int32">
							</PropertiesComboBox>
						</dx:GridViewDataComboBoxColumn>
						<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="business_unit_id" 
							VisibleIndex="7">
							<PropertiesComboBox DataSourceID="sds_active_business_units" TextField="name" 
								ValueField="id" ValueType="System.Int32">
							</PropertiesComboBox>
						    <Settings HeaderFilterMode="CheckedList" />
						</dx:GridViewDataComboBoxColumn>
						<dx:GridViewDataComboBoxColumn Caption="Type" FieldName="type" VisibleIndex="2">
						<PropertiesComboBox>
						<Items>
						<dx:ListEditItem Text="Visa" Value="Visa" />
						<dx:ListEditItem Text="Master Card" Value="Master Card" />
						<dx:ListEditItem Text="American Express" Value="American Express" />
						</Items>
						</PropertiesComboBox>
						    <Settings HeaderFilterMode="CheckedList" />
						</dx:GridViewDataComboBoxColumn>
					    <dx:GridViewDataComboBoxColumn Caption="Status" FieldName="status" VisibleIndex="12">
                            <PropertiesComboBox>
						<Items>
						<dx:ListEditItem Text="Active" Value="Active" Selected="true" />
						<dx:ListEditItem Text="Cancelled" Value="Cancelled" />
						<dx:ListEditItem Text="On Hold" Value="On Hold" />
						</Items>
						</PropertiesComboBox>
                            <Settings HeaderFilterMode="CheckedList" />
                        </dx:GridViewDataComboBoxColumn>
					</Columns>
					<SettingsBehavior ConfirmDelete="True" />
					<SettingsPager PageSize="30">
                    </SettingsPager>
					<SettingsEditing Mode="Batch">
					</SettingsEditing>
					<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
					<SettingsPopup>
						<EditForm Height="300px" HorizontalAlign="WindowCenter" 
							VerticalAlign="WindowCenter" Width="600px" />
					</SettingsPopup>
				    <SettingsSearchPanel Visible="True" />
				</dx:ASPxGridView>
			</td>
			
		</tr>
		<tr>
			<td>
				<asp:SqlDataSource ID="sds_active_business_units" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
					SelectCommand="Select id,ddl_name name from business_unit  where active = 'T' order by name">
				</asp:SqlDataSource>
				<asp:SqlDataSource ID="sds_Issued_By" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
					SelectCommand="SELECT
		DISTINCT(a.member_id),
		CONCAT(b.ddl_name,': ',a.member_fullname) member_fullname
	FROM
		member a
	INNER JOIN business_unit b ON a.business_unit_id = b.id 					
	 ORDER BY
		member_fullname
"></asp:SqlDataSource>
					<asp:SqlDataSource ID="sds_Issued_To_existing" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
					SelectCommand="
SELECT
		DISTINCT(c.member_id),
		CONCAT(e.ddl_name,': ',c.member_fullname) member_fullname
	FROM
		member c 
	LEFT JOIN 
		credit_cards d ON d.member_id = c.member_id
	INNER JOIN 
		business_unit e ON c.business_unit_id = e.id AND e.active = 'T'
	WHERE c.member_status = 'Active' OR c.member_hrstatus_id = 4
	ORDER BY
		member_fullname">
				</asp:SqlDataSource>
				<asp:SqlDataSource ID="sds_Issued_To_new" runat="server" 
								   ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								   ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								   SelectCommand="
SELECT
		DISTINCT(c.member_id),
		CONCAT(e.ddl_name,': ',c.member_fullname) member_fullname
	FROM
		member c 
	INNER JOIN 
		business_unit e ON c.business_unit_id = e.id AND e.active = 'T'
	WHERE 
		c.member_status = 'Active'
	ORDER BY
		member_fullname">
				</asp:SqlDataSource>
				
				<asp:SqlDataSource ID="sds_credit_cards" runat="server" 
					ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
					DeleteCommand="DELETE FROM [credit_cards] WHERE [id] = ?" 
					InsertCommand="INSERT INTO [credit_cards] ([id], [type], [bank], [date_issued], [member_id], [issued_by], [business_unit_id], [date_of_expiry], [number], [credit_limit],[status]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?,?)" 
					ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
					SelectCommand="SELECT * FROM [credit_cards] a INNER JOIN business_unit b ON a.business_unit_id = b.id INNER JOIN member c ON a.member_id = c.member_id INNER JOIN member d ON a.Issued_by = d.member_id WHERE c.member_status = 'Active' AND b.active = 'T'" 
					UpdateCommand="UPDATE [credit_cards] SET [type] = ?, [bank] = ?, [date_issued] = ?, [member_id] = ?, [issued_by] = ?, [business_unit_id] = ?, [date_of_expiry] = ?, [number] = ?, [credit_limit] = ?, [status]=? WHERE [id] = ?">
					<DeleteParameters>
						<asp:Parameter Name="id" Type="Int32" />
					</DeleteParameters>
					<InsertParameters>
						<asp:Parameter Name="id" Type="Int32" />
						<asp:Parameter Name="type" Type="String" />
						<asp:Parameter Name="bank" Type="String" />
						<asp:Parameter Name="date_issued" Type="DateTime" />
						<asp:Parameter Name="member_id" Type="Int32" />
						<asp:Parameter Name="issued_by" Type="Int32" />
						<asp:Parameter Name="business_unit_id" Type="Int32" />
						<asp:Parameter Name="date_of_expiry" Type="DateTime" />
						<asp:Parameter Name="number" Type="String" />
						<asp:Parameter Name="credit_limit" Type="Double" />
                        <asp:Parameter Name="status" Type="String" />
					</InsertParameters>
					<UpdateParameters>
						<asp:Parameter Name="type" Type="String" />
						<asp:Parameter Name="bank" Type="String" />
						<asp:Parameter Name="date_issued" Type="DateTime" />
						<asp:Parameter Name="member_id" Type="Int32" />
						<asp:Parameter Name="issued_by" Type="Int32" />
						<asp:Parameter Name="business_unit_id" Type="Int32" />
						<asp:Parameter Name="date_of_expiry" Type="DateTime" />
						<asp:Parameter Name="number" Type="String" />
						<asp:Parameter Name="credit_limit" Type="Double" />
						<asp:Parameter Name="id" Type="Int32" />
                        <asp:Parameter Name="status" Type="String" />
					</UpdateParameters>
				</asp:SqlDataSource>
				
			</td>
			
		</tr>
		<tr>
			<td>
				&nbsp;
			</td>
			
		</tr>
	</table>
</asp:Content>

<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	</asp:Content>


