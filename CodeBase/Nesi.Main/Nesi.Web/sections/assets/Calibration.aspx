<%@ Page Language="C#"  MasterPageFile="~/nonFrame.master" AutoEventWireup="true"   Title="Calibration"  Inherits="sections_assets_Asset_Calibration"  Codebehind="Calibration.aspx.cs"  %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
   
							<asp:SqlDataSource ID="sqlcompanys" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								>
							</asp:SqlDataSource>
							<asp:SqlDataSource ID="sdsCalibration" runat="server" 
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
								SelectCommand="SELECT assets_ID  as ID,CONCAT(IFNULL(assets_year, 'Year not provided'),' ',IFNULL(assets_Make, 'Assert maker not provided'),'-',IFNULL(assets_Model, 'Assert model not provided'),' No:',IFNULL(assets_no, 'Assert No not provided')) description, next_calib_date,a.business_unit_id AS branch  FROM assets a left join business_unit b on a.business_unit_id = b.id WHERE  needs_calib=TRUE">
							</asp:SqlDataSource>
							<dx:ASPxGridView ID="gv_assets_Calibration" runat="server" AutoGenerateColumns="False" 
								ClientInstanceName="gv_assets_Calibration" DataSourceID="sdsCalibration" 
								KeyFieldName="ID" Width="100%"
								Font-Names="Arial" Theme="NETheme01">
                       	<Columns>
									<dx:GridViewCommandColumn ButtonType="Image" Caption=" " ShowInCustomizationForm="True" VisibleIndex="0" Width="75px" ShowEditButton="true" ShowDeleteButton="true" ShowNewButton="true">
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewCommandColumn>
									<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" 
										ShowInCustomizationForm="True" VisibleIndex="1" ReadOnly="True" Visible="true" Width="45px">
									</dx:GridViewDataTextColumn>
                                    <dx:GridViewDataComboBoxColumn Caption="Business Unit" FieldName="branch" 
										ShowInCustomizationForm="True" VisibleIndex="2" Width="120px" ReadOnly="True"  >
										<PropertiesComboBox DataSourceID="sqlcompanys" TextField="company" 
											ValueField="Id" ValueType="System.Int32">
										</PropertiesComboBox>
										<EditFormSettings Visible="False" />
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataDateColumn Caption="Calibration Date" FieldName="next_calib_date" 
										ShowInCustomizationForm="true" VisibleIndex="3" Width="120px">
										<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" Width="120px">
										</PropertiesDateEdit>
									</dx:GridViewDataDateColumn>
                               
									<dx:GridViewDataComboBoxColumn Caption="Asset Description" 
										FieldName="description" ShowInCustomizationForm="True" 
										VisibleIndex="4" Width="100px">
										<PropertiesComboBox DataSourceID="sdsCalibration" TextField="description" 
											ValueField="id" ValueType="System.Int32" Width="350px">
										</PropertiesComboBox>
									</dx:GridViewDataComboBoxColumn>
									
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

								<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True"  ShowFilterBar="Visible"
									ShowHeaderFilterButton="True" />
								<SettingsText CommandClearFilter="Clear" 
									PopupEditFormCaption="Add / Edit Asset" />
									<Styles>
										
										<CommandColumn Spacing="5px">
										</CommandColumn>
								</Styles>
                                <StylesPopup  EditForm-Content-Paddings-Padding="10px" 
									EditForm-MainArea-Font-Names="Arial"><EditForm><MainArea Font-Names="Arial"></MainArea><Content><Paddings Padding="10px"></Paddings></Content></EditForm>
								</StylesPopup>
							</dx:ASPxGridView>
   
</asp:Content>
