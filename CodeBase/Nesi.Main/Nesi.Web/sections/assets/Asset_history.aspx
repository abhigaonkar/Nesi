<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_assets_Asset_history"  Title="Asset History" EnableTheming="True" Codebehind="Asset_history.aspx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>




<%@ Register src="../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
   
	<dx:ASPxCallbackPanel ID="cp" runat="server" ClientInstanceName="cp" 
		oncallback="cp_Callback">
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
				<table>
					<tr>
						<td>
							<dx:ASPxComboBox ID="ASPxComboBox1" runat="server" DataSourceID="sqlcompanys" 
								TextField="company" ValueField="id" ValueType="System.Int32" AutoPostBack="True" 
								OnSelectedIndexChanged="ASPxComboBox1_SelectedIndexChanged" Theme="NETheme01">
							</dx:ASPxComboBox>
							<lc:LayoutControl ID="layout" runat="server" is_private="True" 
								GridviewID="gv_assets_history" />
							<asp:SqlDataSource ID="sqlcompanys" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								>
							</asp:SqlDataSource>
							<asp:SqlDataSource ID="sqlmembers" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								
								SelectCommand="select a.member_id, a.member_fullname as membername from member a inner join business_unit b on a.business_unit_id = b.id where a.business_unit_id = @cid and a.member_status = 'Active' order by membername">
								<SelectParameters>
									<asp:ControlParameter ControlID="ASPxComboBox1" Name="cid" 
										PropertyName="Value" />
								</SelectParameters>
							</asp:SqlDataSource>
							<asp:SqlDataSource ID="sqlassettypes" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								SelectCommand="SELECT asset_type_id, asset_type_name FROM assets_type ORDER BY asset_type_name">
							</asp:SqlDataSource>
							<asp:SqlDataSource ID="sqlstatus" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								SelectCommand="SELECT asset_status_id, asset_status_status FROM asset_status">
							</asp:SqlDataSource>
							<asp:SqlDataSource ID="sqlassets" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								
								
								
								SelectCommand="SELECT assets_ID, Concat(assets_Year,' ',assets_Make,'-',assets_Model,' No:',assets_no) name from assets where business_unit_id = @cid order by Concat(assets_Year,' ',assets_Make,'-',assets_Model,' No:',assets_no)">
								<SelectParameters>
									<asp:ControlParameter ControlID="ASPxComboBox1" Name="cid" 
										PropertyName="Value" />
								</SelectParameters>
							</asp:SqlDataSource>
							<asp:SqlDataSource ID="sqlhistorytype" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								SelectCommand="SELECT asset_history_action_id, asset_history_action_name FROM asset_history_action">
							</asp:SqlDataSource>
							<asp:SqlDataSource ID="history" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								
								SelectCommand="SELECT assets_history.assets_history_id AS ID, assets_history.assets_history_date, assets_history.assets_history_assetid, assets_history.assets_history_memberid, assets_history.assets_history_dollars, assets_history.assets_history_action, urldecode(assets_history.assets_history_notes) assets_history_notes, assets.assets_owner AS owner, assets.assets_statusid AS statusid, assets.assets_type AS typeid, assets.business_unit_id AS branch, assets.assets_No AS asset_no,assets.assets_iconpic FROM assets_history INNER JOIN assets ON assets_history.assets_history_assetid = assets.assets_ID
where assets.business_unit_id = ?cid">
								<SelectParameters>
									<asp:ControlParameter ControlID="ASPxComboBox1" Name="cid" 
										PropertyName="Value" />
								</SelectParameters>
							</asp:SqlDataSource>
						</td>
						
					</tr>
					<tr>
						<td>
							<dx:ASPxGridView ID="gv_assets_history" runat="server" AutoGenerateColumns="False" 
								ClientInstanceName="gv_assets_history" DataSourceID="history" 
								KeyFieldName="ID" OnCustomCallback="gv_assets_CustomCallback" 
								OnCustomJSProperties="gv_assets_CustomJSProperties" Width="100%" OnHtmlDataCellPrepared="gv_assets_history_HtmlDataCellPrepared"
								OnHtmlEditFormCreated="gv_assets_HtmlEditFormCreated" OnRowUpdating="gv_assets_RowUpdating" 
								Font-Names="Arial" OnRowInserting="gv_assets_RowInserting" OnRowDeleting="gv_history_RowDeleting" 
								 Theme="NETheme01">
                                <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
								<Columns>
									<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
										ShowInCustomizationForm="True" VisibleIndex="0" Width="75px" 
ShowEditButton="true" ShowDeleteButton="true"    ShowNewButton="true">
										
										
										
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewCommandColumn>
									<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
										ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True" Visible="False" Width="25px">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataDateColumn Caption="Date" FieldName="assets_history_date" 
										ShowInCustomizationForm="True" VisibleIndex="3" Width="120px">
										<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" Width="120px">
										</PropertiesDateEdit>
									</dx:GridViewDataDateColumn>
									<dx:GridViewDataComboBoxColumn Caption="Asset" 
										FieldName="assets_history_assetid" ShowInCustomizationForm="True" 
										VisibleIndex="4" Width="100px">
										<PropertiesComboBox DataSourceID="sqlassets" TextField="name" 
											ValueField="assets_ID" ValueType="System.Int32" Width="150px">
										</PropertiesComboBox>
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataComboBoxColumn Caption="Employee" 
										FieldName="assets_history_memberid" ShowInCustomizationForm="True" 
										VisibleIndex="5" Width="60px">
										<PropertiesComboBox DataSourceID="sqlmembers" TextField="membername" 
											ValueField="member_id" ValueType="System.Int32" Width="150px">
										</PropertiesComboBox>
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataTextColumn Caption="Cost" FieldName="assets_history_dollars" 
										ShowInCustomizationForm="True" VisibleIndex="6" Width="40px">
										<EditFormSettings/>
										<PropertiesTextEdit DisplayFormatString="C2" Width="150px">
										</PropertiesTextEdit>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataComboBoxColumn Caption="Action" 
										FieldName="assets_history_action" ShowInCustomizationForm="True" 
										VisibleIndex="7" Width="90px">
										<PropertiesComboBox DataSourceID="sqlhistorytype" 
											TextField="asset_history_action_name" ValueField="asset_history_action_id" 
											ValueType="System.Int32" Width="150px">
										</PropertiesComboBox>
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataMemoColumn Caption="Notes" FieldName="assets_history_notes" 
										ShowInCustomizationForm="True" VisibleIndex="8" Width="100%">
										<PropertiesMemoEdit Width="600px">
											
											<Style BackColor="#FFFFCC">
											</Style>
											
										</PropertiesMemoEdit>
										<EditFormSettings ColumnSpan="4" />
									</dx:GridViewDataMemoColumn>
									<dx:GridViewDataComboBoxColumn Caption="Asset Owner" FieldName="owner" 
										ShowInCustomizationForm="True" VisibleIndex="9" Width="70px">
										<PropertiesComboBox DataSourceID="sqlmembers" TextField="membername" 
											ValueField="member_id" ValueType="System.Int32">
										</PropertiesComboBox>
										<EditFormSettings Visible="False" />
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataComboBoxColumn Caption="Asset Type" FieldName="typeid" 
										ShowInCustomizationForm="True" VisibleIndex="10" Width="80px">
										<PropertiesComboBox DataSourceID="sqlassettypes" TextField="asset_type_name" 
											ValueField="asset_type_id" ValueType="System.Int32">
										</PropertiesComboBox>
										<EditFormSettings Visible="False" />
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataTextColumn Caption="Asset No" FieldName="asset_no" 
										ShowInCustomizationForm="True" VisibleIndex="11" Width="60px">
										<EditFormSettings Visible="False" />
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataComboBoxColumn Caption="Status" FieldName="statusid" 
										ShowInCustomizationForm="True" VisibleIndex="12" Width="70px">
										<PropertiesComboBox DataSourceID="sqlstatus" TextField="asset_status_status" 
											ValueField="asset_status_id" ValueType="System.Int32">
										</PropertiesComboBox>
										<EditFormSettings Visible="False" />
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="branch" 
										ShowInCustomizationForm="True" VisibleIndex="2" Width="80px">
										<PropertiesComboBox DataSourceID="sqlcompanys" TextField="company" 
											ValueField="id" ValueType="System.Int32">
										</PropertiesComboBox>
										<EditFormSettings Visible="False" />
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataImageColumn Caption="Image" FieldName="assets_iconpic" 
										ShowInCustomizationForm="True" VisibleIndex="13" Width="60px">
										<PropertiesImage ImageUrlFormatString="~/asset_pics/{0}" ImageHeight="50px" 
											ImageWidth="50px">
										</PropertiesImage>
										<EditFormSettings Visible="True" />
									</dx:GridViewDataImageColumn>
								</Columns>
								<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" 
									ConfirmDelete="True" />


                                    <SettingsPopup CustomizationWindow-VerticalAlign="TopSides" 
									CustomizationWindow-HorizontalAlign="LeftSides" 
									EditForm-HorizontalAlign="WindowCenter" EditForm-VerticalAlign="WindowCenter" 
									EditForm-Width="800px" EditForm-Height="700px" EditForm-Modal="True">
<EditForm Width="800px" Height="700px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>

<CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="TopSides"></CustomizationWindow>
								</SettingsPopup>
								<SettingsEditing EditFormColumnCount="4" Mode="PopupEditForm"/>

								<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True" 
									ShowHeaderFilterButton="True" />
								<SettingsText CommandClearFilter="Clear" 
									PopupEditFormCaption="Add / Edit Asset" />
									<Styles>
										
										<CommandColumn Spacing="5px">
										</CommandColumn>
								</Styles>
                                <StylesPopup  EditForm-Content-Paddings-Padding="10px" 
									EditForm-MainArea-Font-Names="Arial">
<EditForm>
<MainArea Font-Names="Arial"></MainArea>

<Content>
<Paddings Padding="10px"></Paddings>
</Content>
</EditForm>
								</StylesPopup>
									<Templates>
											<EditForm>
								
												
															<div align="middle">
																<dx:ASPxLabel ID="ASPxLabel1" runat="server" BackColor="Black" Font-Bold="True" 
																	Font-Names="Arial" ForeColor="White" Text="Event Details" Width="100%">
																</dx:ASPxLabel></div>
																<dx:ASPxImage ID="img1" runat="server" ClientInstanceName="img1" 
																EnableClientSideAPI="True" Width="150px">
															</dx:ASPxImage>
																<br />
																<br />
																<dx:ASPxGridViewTemplateReplacement ID="Editors" runat="server" 
																	ReplacementType="EditFormEditors" />
															
															<br />
															
															<table style="width:100%;">
																<tr>
																	<td class="style4">
																		&nbsp;</td>
																</tr>
															</table>
                           
                            <div style="margin-top: 10px; margin-bottom: 20px; padding-bottom:20px;">
                                <div style="float: left; margin-left: 5px">
                                    <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel" Width="100px"
                                        ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' Theme="NETheme01" />
                                </div>
                                <div style="float: left; margin-left: 10px">
                                    <dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" Text="Save" Width="100px"
                                        CssClass="input" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' Theme="NETheme01" />
                                </div>
                            </div>
											</EditForm>
										</Templates>
							</dx:ASPxGridView>
							<br />
						</td>
						
					</tr>
					
				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>
   
</asp:Content>




