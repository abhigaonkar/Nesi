<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Theme="NETheme01" Inherits="sections_hr_member_cellphones " Title="Cell Phones" Codebehind="cellphones.aspx.cs" %>


<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>



<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
	<div id="divMenu" runat="server">
    	A</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
	<div id="divSide" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">

	<script type="text/javascript" language="javascript">
function resizeIframe(obj)
 {
   obj.style.height = (obj.contentWindow.document.body.scrollHeight + 100) + 'px';
	
 }
	
 </script>
 	<dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" 
		Width="100%" EnableCallBacks="True">
		<TabPages>
			<dx:TabPage Text="Cell Phones">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
						<lc:LayoutControl ID="layout" runat="server" __is_private="True" 
							GridviewID="gv_cellphones" ShowToggle="True" />
						<dx:ASPxGridView ID="gv_cellphones" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_cellphones" Font-Names="Arial" KeyFieldName="id" 
							OnCustomCallback="gv_cellphones_CustomCallback" 
							OnCustomJSProperties="gv_cellphones_CustomJSProperties" 
							OnHtmlEditFormCreated="gv_cellphones_HtmlEditFormCreated1" 
							OnRowInserting="gv_cellphones_RowInserting" 
							OnRowUpdating="gv_cellphones_RowUpdating" 
							OnStartRowEditing="gv_cellphones_StartRowEditing" Width="100%" 
							OnRowDeleting="gv_cellphones_RowDeleting">
							<SettingsCommandButton>
								<EditButton Text="Edit" Image-Height="16px" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" >
<Image Height="16px" Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
								</EditButton>
								<NewButton Text="New" Image-Height="16px" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" >
<Image Height="16px" Width="16px" Url="~/images/icon/icon[add].gif"></Image>
								</NewButton>
								<DeleteButton Text="Delete" Image-Height="16px" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" >
<Image Height="16px" Width="16px" Url="~/images/icon/icon[delete].gif"></Image>
								</DeleteButton>
							</SettingsCommandButton>
							<Columns>
								<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
									VisibleIndex="0" ButtonType="Image" MinWidth="20" ShowDeleteButton="True" ShowEditButton="True" ShowClearFilterButton="true" ShowNewButton="True" ShowUpdateButton="True">
									
									
									
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="ID" FieldName="id" MinWidth="15" 
									ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="1" Visible="False">
									<CellStyle BackColor="#FFFFCC">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Number" FieldName="number" MinWidth="15" 
									ShowInCustomizationForm="True" VisibleIndex="2" ReadOnly="True">
									<EditFormSettings Visible="False" />
									<DataItemTemplate>
										<dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Arial" 
											onprerender="ASPxLabel1_PreRender" Text='<%# Eval("number") %>'>
										</dx:ASPxLabel>
									</DataItemTemplate>
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="IMEI" FieldName="imei" MinWidth="15" 
									ShowInCustomizationForm="True" VisibleIndex="3">
									<CellStyle BackColor="#FFFFCC">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="OS" FieldName="os" MinWidth="15" 
									ShowInCustomizationForm="True" VisibleIndex="4">
									<CellStyle BackColor="#FFFFCC">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataComboBoxColumn Caption="Phone Status" 
									FieldName="cellphone_status_id" MinWidth="15" ShowInCustomizationForm="True" 
									VisibleIndex="5">
									<PropertiesComboBox DataSourceID="SqlDataSource2" 
										TextField="name" ValueField="id" 
										ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings FilterMode="DisplayText" SortMode="DisplayText" />
									<CellStyle Wrap="False" BackColor="#FFFFCC">
									</CellStyle>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn Caption="Simcard" FieldName="simcard" MinWidth="15" 
									ShowInCustomizationForm="True" VisibleIndex="8" ReadOnly="True">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataComboBoxColumn Caption="Member" FieldName="member_id" 
									MinWidth="15" ShowInCustomizationForm="True" VisibleIndex="9">
									<PropertiesComboBox DataSourceID="SqlDataSource3" TextField="_name" 
										ValueField="member_id" ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings FilterMode="DisplayText" SortMode="DisplayText" />
									<EditFormSettings Visible="False" />
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="business_unit" 
									MinWidth="15" ShowInCustomizationForm="True" VisibleIndex="10">
									<PropertiesComboBox DataSourceID="sqlcomp" TextField="name" 
										ValueField="id" ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings FilterMode="DisplayText" SortMode="DisplayText" />
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn Caption="Member Status" FieldName="member_status" 
									MinWidth="15" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="11">
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="HR Status" FieldName="hr_status" 
									MinWidth="15" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="12">
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataComboBoxColumn Caption="Carrier" FieldName="carrier_id" 
									MinWidth="15" ShowInCustomizationForm="True" VisibleIndex="6">
									<PropertiesComboBox DataSourceID="SqlDataSource1" TextField="name" 
										ValueField="id" ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings FilterMode="DisplayText" SortMode="DisplayText" />
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataDateColumn Caption="Activated" FieldName="activedate" 
									MinWidth="15" ShowInCustomizationForm="True" VisibleIndex="7">
									<PropertiesDateEdit DisplayFormatString="yyyy-M-dd" EditFormat="Custom" 
										EditFormatString="yyyy-MM-dd">
									</PropertiesDateEdit>
									<CellStyle BackColor="#FFFFCC" Wrap="False">
									</CellStyle>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataDateColumn Caption="Last Updated" FieldName="last_modified" 
									MinWidth="15" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="13">
									<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
										EditFormatString="yyyy-MM-dd">
									</PropertiesDateEdit>
									<EditFormSettings Visible="False" />
									<CellStyle BackColor="#FFFFCC" Wrap="False">
									</CellStyle>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataTextColumn Caption="History" FieldName="history" MinWidth="15" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="15">
									<EditFormSettings ColumnSpan="2" Visible="True" />
									<EditItemTemplate>
										<templates>
											<editform>
												<dx:ASPxGridView ID="gv_history" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_history" DataSourceID="SqlDataSource4" Width="100%">
													<Columns>
														<dx:GridViewDataDateColumn Caption="Date" FieldName="dt" VisibleIndex="0">
															<propertiesdateedit displayformatstring="" editformat="Custom" 
																editformatstring="yyyy-MM-dd">
			</propertiesdateedit>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataTextColumn Caption="Member" FieldName="_name" VisibleIndex="1">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Action" FieldName="notes" VisibleIndex="2">
														</dx:GridViewDataTextColumn>
													</Columns>
												</dx:ASPxGridView>
											</editform>
										</templates>
									</EditItemTemplate>
								</dx:GridViewDataTextColumn>
								
							</Columns>
							<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
								ConfirmDelete="True" EnableCustomizationWindow="True" />
							<SettingsPager PageSize="50">
							</SettingsPager>
							<SettingsEditing Mode="PopupEditForm" />
							<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
								ShowFilterRowMenuLikeItem="True" ShowFooter="True" ShowGroupPanel="True" 
								ShowHeaderFilterButton="True" ColumnMinWidth="15" />
							<SettingsPopup>
								<EditForm Height="800px" HorizontalAlign="WindowCenter" 
									VerticalAlign="WindowCenter" Width="900px" />
							</SettingsPopup>
							
							<StylesPopup>
								<EditForm>
									<Content>
										<Paddings Padding="5px" />
									</Content>
								</EditForm>
							</StylesPopup>
						</dx:ASPxGridView>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Phone Numbers and SIM cards">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
											
						<dx:ASPxGridView ID="gv_numbers" runat="server" AutoGenerateColumns="False" 
							ClientInstanceName="gv_numbers" Font-Names="Arial" KeyFieldName="id" 
							OnCustomCallback="gv_numbers_CustomCallback" 
							OnCustomJSProperties="gv_numbers_CustomJSProperties" 
							OnHtmlEditFormCreated="gv_numbers_HtmlEditFormCreated1" 
							OnRowInserting="gv_numbers_RowInserting" 
							OnRowUpdating="gv_numbers_RowUpdating" 
							OnStartRowEditing="gv_numbers_StartRowEditing" Width="100%" OnRowDeleting="gv_numbers_RowDeleting">
							<SettingsCommandButton>
									<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" >
<Image Url="~/images/icon/icon[edit].gif"></Image>
									</EditButton>
									<NewButton Image-Url="~/images/icon/icon[add].gif" >
<Image Url="~/images/icon/icon[add].gif"></Image>
									</NewButton>
									<DeleteButton Image-Url="~/images/icon/icon[delete].gif" >
<Image Url="~/images/icon/icon[delete].gif"></Image>
									</DeleteButton>
							</SettingsCommandButton>
							<Columns>
								<dx:GridViewCommandColumn Caption=" " ShowInCustomizationForm="True" 
									VisibleIndex="0" ButtonType="Image" ShowCancelButton="true" ShowUpdateButton="true" ShowDeleteButton="true" ShowNewButton="true" ShowEditButton="true" ShowClearFilterButton="true">
									
									
									
									
								</dx:GridViewCommandColumn>
								<dx:GridViewDataTextColumn Caption="ID" FieldName="id"  Visible="true" MinWidth="15" 
									ReadOnly="True" ShowInCustomizationForm="False" VisibleIndex="1">
									<CellStyle BackColor="#CCFFCC">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="Number" FieldName="number" MinWidth="15" 
									ShowInCustomizationForm="True" VisibleIndex="2">
									<EditFormSettings Visible="True" />
									<DataItemTemplate>
										<dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Names="Arial" 
											onprerender="ASPxLabel1_PreRender" Text='<%# Eval("number") %>'>
										</dx:ASPxLabel>
									</DataItemTemplate>
									<CellStyle BackColor="#CCFFCC" Wrap="False">
									</CellStyle>
									<PropertiesTextEdit>
                <MaskSettings Mask="999-000-0000" />
            </PropertiesTextEdit>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="IMEI" FieldName="imei" MinWidth="15" 
									ShowInCustomizationForm="True" VisibleIndex="3">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="OS" FieldName="os" MinWidth="15" 
									ShowInCustomizationForm="True" VisibleIndex="4">
									<EditFormSettings Visible="False" />
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataComboBoxColumn Caption="Phone Status" 
									FieldName="cellphone_status_id" MinWidth="15" ShowInCustomizationForm="True" 
									VisibleIndex="5">
									<PropertiesComboBox DataSourceID="SqlDataSource2" 
										TextField="name" ValueField="id" 
										ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings FilterMode="DisplayText" SortMode="DisplayText" />
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataComboBoxColumn Caption="Carrier" FieldName="carrier_id" 
									MinWidth="15" ShowInCustomizationForm="True" VisibleIndex="6">
									<PropertiesComboBox DataSourceID="SqlDataSource1" TextField="name" 
										ValueField="id" ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings FilterMode="DisplayText" SortMode="DisplayText" />
									<CellStyle BackColor="#CCFFCC">
									</CellStyle>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataDateColumn Caption="Activated" FieldName="activedate" 
									MinWidth="15" ShowInCustomizationForm="True" VisibleIndex="7">
									<PropertiesDateEdit DisplayFormatString="yyyy-M-dd" EditFormat="Custom" 
										EditFormatString="yyyy-MM-dd">
									</PropertiesDateEdit>
									<CellStyle BackColor="#CCFFCC" Wrap="False">
									</CellStyle>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataTextColumn Caption="Simcard" FieldName="simcard" MinWidth="15" 
									ShowInCustomizationForm="True" VisibleIndex="8">
									<CellStyle BackColor="#CCFFCC">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataComboBoxColumn Caption="Member" FieldName="member_id" 
									MinWidth="15" ShowInCustomizationForm="True" VisibleIndex="9" ReadOnly="True">
									<PropertiesComboBox DataSourceID="SqlDataSource3" TextField="_name" 
										ValueField="member_id" ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings FilterMode="DisplayText" SortMode="DisplayText" />
									<EditFormSettings Visible="False" />
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="business_unit" 
									MinWidth="15" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="10">
									<PropertiesComboBox DataSourceID="sqlcomp" TextField="name" 
										ValueField="id" ValueType="System.Int32">
									</PropertiesComboBox>
									<Settings FilterMode="DisplayText" SortMode="DisplayText" />
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn Caption="Member Status" FieldName="member_status" 
									MinWidth="15" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="11">
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataTextColumn Caption="HR Status" FieldName="hr_status" 
									MinWidth="15" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="12">
									<EditFormSettings Visible="False" />
									<CellStyle Wrap="False">
									</CellStyle>
								</dx:GridViewDataTextColumn>
								<dx:GridViewDataDateColumn Caption="Last Updated" FieldName="last_modified" 
									MinWidth="15" ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="13">
									<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
										EditFormatString="yyyy-MM-dd">
									</PropertiesDateEdit>
									<EditFormSettings Visible="False" />
									<CellStyle BackColor="#CCFFCC" Wrap="False">
									</CellStyle>
								</dx:GridViewDataDateColumn>
								<dx:GridViewDataComboBoxColumn Caption="SIM Status" FieldName="sim_status" 
									MinWidth="15" ShowInCustomizationForm="True" VisibleIndex="14">
									<PropertiesComboBox>
										<Items>
											<dx:ListEditItem Text="Active" Value="Active" />
											<dx:ListEditItem Text="Not Active" Value="Not Active" />
                                            <dx:ListEditItem Text="On Holiday" Value="On Holiday" />
										</Items>
									</PropertiesComboBox>
									<CellStyle BackColor="#CCFFCC" Wrap="False">
									</CellStyle>
								</dx:GridViewDataComboBoxColumn>
								<dx:GridViewDataTextColumn Caption="History" FieldName="history" MinWidth="15" 
									ShowInCustomizationForm="True" Visible="False" VisibleIndex="15" ReadOnly="True">
									<EditFormSettings ColumnSpan="2" Visible="True" />
									<EditItemTemplate>
										<templates>
											<editform>
												<dx:ASPxGridView ID="gv_history1" runat="server" AutoGenerateColumns="False" 
													ClientInstanceName="gv_history1" DataSourceID="SqlDataSource5" Width="100%">
													<Columns>
														<dx:GridViewDataDateColumn Caption="Date" FieldName="dt" VisibleIndex="0">
															<propertiesdateedit displayformatstring="" editformat="Custom" 
																editformatstring="yyyy-MM-dd">
			</propertiesdateedit>
														</dx:GridViewDataDateColumn>
														<dx:GridViewDataTextColumn Caption="Member" FieldName="_name" VisibleIndex="1">
														</dx:GridViewDataTextColumn>
														<dx:GridViewDataTextColumn Caption="Action" FieldName="notes" VisibleIndex="2">
														</dx:GridViewDataTextColumn>
													</Columns>
												</dx:ASPxGridView>
											</editform>
										</templates>
									</EditItemTemplate>
								</dx:GridViewDataTextColumn>
								
							</Columns>
							<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
								ConfirmDelete="True" />
							<SettingsPager PageSize="50">
							</SettingsPager>
							
							<SettingsEditing Mode="PopupEditForm" />
							<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowFilterRowMenu="True" 
								ShowFilterRowMenuLikeItem="True" ShowFooter="True" ShowGroupPanel="True" 
								ShowHeaderFilterButton="True" />
							<SettingsPopup>
								<EditForm Height="800px" HorizontalAlign="WindowCenter" 
									VerticalAlign="WindowCenter" Width="900px" />
							</SettingsPopup>
							<StylesPopup>
								<EditForm>
									<Content>
										<Paddings Padding="5px" />
									</Content>
								</EditForm>
							</StylesPopup>
						</dx:ASPxGridView>
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
			<dx:TabPage Text="Bar Code Scanners">
				<ContentCollection>
					<dx:ContentControl runat="server" SupportsDisabledAttribute="True">
					</dx:ContentControl>
				</ContentCollection>
			</dx:TabPage>
		</TabPages>
	</dx:ASPxPageControl>
	<br />
	<br />



			<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from cellphone_carrier"></asp:SqlDataSource>
	<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from cellphone_status"></asp:SqlDataSource>
	<asp:SqlDataSource ID="SqlDataSource4" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										
		SelectCommand="Select *, get_name(member_id) _name from cellphone_history where cellphone_history.cellphone_id =  ?id order by dt desc">
		<SelectParameters>
			<asp:ControlParameter ControlID="hdnid" Name="id" PropertyName="Value" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="SqlDataSource5" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										
		SelectCommand="Select *, get_name(member_id) _name from cellphone_number_history where cellphone_number_history.cellphone_number_id =  ?id order by dt desc">
		<SelectParameters>
			<asp:ControlParameter ControlID="hdnnid" Name="id" PropertyName="Value" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:HiddenField ID="hdnid" runat="server" />
	<asp:HiddenField ID="hdnnid" runat="server" />
	<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select member_id,get_name(member_id) _name from member">
	</asp:SqlDataSource>



			<br />
	<asp:SqlDataSource ID="sqlcomp" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select id,ddl_name name from business_unit ">
	</asp:SqlDataSource>
	<br />
	<br />
</asp:Content>

