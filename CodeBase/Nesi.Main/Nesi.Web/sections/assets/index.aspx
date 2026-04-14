<%@ Page Language="C#" MasterPageFile="~/nonFrame.master" AutoEventWireup="true" Inherits="sections_assets_index" Title="Assets" EnableTheming="True" CodeBehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>




<%@ Register Src="../../modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<asp:Content ID="head" ContentPlaceHolderID="header_placeholder" runat="server">
<style type="text/css">
	 .required	{
		background: #cfc;
	 }
	 .endrow {
		padding-bottom: 10px;
		border-bottom: solid 1px #888;
	 }
</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">

<script type="text/javascript">
	function validateEdit(s, e) {
		if (ASPxClientEdit.ValidateGroup('assetEdit')) {
			if (needscalibration.GetChecked() && calibrationfrequency.GetText().trim() == '') {
				alert("You've set the 'Enable Calibration Schedule'... Please enter a calibration frequency before saving.", null, "calibrationfrequency.Focus()");
			}
			else {
				gv_assets.UpdateEdit();
			}
		}
	}
	var Validate = {
		CalibrationItem: function (s, e)
				{
			if (ASPxClientEdit.ValidateGroup('CalibrationItem'))
				{
				gv_calibration.PerformCallback('add');
				}
		},
		HistoryItem: function (s, e)
		{
			if (ASPxClientEdit.ValidateGroup('HistoryItem')) {
				gv_history.PerformCallback('add');
			}
		},
		TabPageRefresh: function (s, e)
		{
			if (pc_Asset.activeTabIndex == 0) {
				setTimeout("pc_Asset.PerformCallback()", 50);
			}
		}
		}
</script>

	<dx:ASPxCallbackPanel ID="cp" runat="server" ClientInstanceName="cp"
		OnCallback="cp_Callback">
		<PanelCollection>
			<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
							<asp:SqlDataSource ID="ds_business_units" runat="server"
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
							<asp:SqlDataSource ID="sqlmembers" runat="server"
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT 0 order_n, 0  member_id, 'Spare' membername UNION ALL SELECT 1 order_n, member_id, member_fullname AS membername FROM member WHERE find_in_set(business_unit_id,@bu) AND member_status = 'Active' ORDER BY order_n, membername">
								<SelectParameters>
									<asp:SessionParameter SessionField="global_visible_business_units" Name="@bu" />
								</SelectParameters>
							</asp:SqlDataSource>
							<asp:SqlDataSource ID="sqlassettypes" runat="server"
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
								SelectCommand="SELECT asset_type_id, asset_type_name FROM assets_type ORDER BY asset_type_name"></asp:SqlDataSource>
							<asp:SqlDataSource ID="sqlstatus" runat="server"
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
								SelectCommand="SELECT asset_status_id, asset_status_status FROM asset_status"></asp:SqlDataSource>
							<asp:SqlDataSource ID="sqlassets" runat="server"
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="
SELECT assets_ID, assets_No, assets_Make, assets_Model, assets_Year, assets_License, business_unit_id, assets_type, 
                assets_notes, assets_statusid, assets_iconpic, assets_owner, transponder, vin_no, mileage, capped, parent_asset_id,show_on_scheduler,lease_no
                ,  daily, weekly, monthly, description, calib_freq, next_calib_date, requires_maintenance, needs_calib, active from assets where find_in_set(business_unit_id, @bu)">
								<SelectParameters>
									<asp:SessionParameter SessionField="global_visible_business_units" Name="@bu" />
								</SelectParameters>

							</asp:SqlDataSource>

							<asp:SqlDataSource ID="sqlparent_assets" runat="server"
								ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
								ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"></asp:SqlDataSource>
				<table>
					<tr>
						<td>
							<lc:LayoutControl ID="layout" runat="server" is_private="True"
								GridviewID="gv_assets" />

						</td>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
					</tr>
					<tr>
						<td>
							<dx:ASPxGridView ID="gv_assets" runat="server" AutoGenerateColumns="False"
								ClientInstanceName="gv_assets" DataSourceID="sqlassets"
								KeyFieldName="assets_ID" OnCustomCallback="gv_assets_CustomCallback"
								OnCustomJSProperties="gv_assets_CustomJSProperties" Width="100%"
								OnHtmlEditFormCreated="gv_assets_HtmlEditFormCreated" OnRowUpdating="gv_assets_RowUpdating"
								Font-Names="Arial" OnRowInserting="gv_assets_RowInserting" OnRowDeleting="gv_assets_RowDeleting" Theme="NETheme01"
								SettingsBehavior-ConfirmDelete="True" SettingsBehavior-ColumnResizeMode="Control">
								<ClientSideEvents CustomButtonClick="function(s, e) {  
	                            if(e.buttonID == 'btnPrintBarcode'){  
                                 var rowVisibleIndex = e.visibleIndex;  
                                 var rowKeyValue = s.GetRowKey(rowVisibleIndex);  
                                 boing('/_tools/print_barcode/index.aspx?master_id=' + rowKeyValue, 'Print Barcode', 600, 200);
                             }  
                           }" />

								<SettingsCommandButton>
									<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px">
										<Image Height="16px" Width="16px" Url="~/images/icon/icon[edit].gif"></Image>
									</EditButton>
									<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px">
										<Image Height="16px" Width="16px" Url="~/images/icon/icon[add].gif"></Image>
									</NewButton>
								</SettingsCommandButton>
								<Columns>

									<dx:GridViewCommandColumn ButtonType="Image" Caption=" "
										ShowInCustomizationForm="True" VisibleIndex="0" Width="75px" ShowNewButtonInHeader="True" ShowEditButton="true" ShowDeleteButton="false" ShowClearFilterButton="true" ShowNewButton="true">
										<CustomButtons>
											<dx:GridViewCommandColumnCustomButton ID="btnPrintBarcode">
												<Image ToolTip="Print Barcode" Url="~/images/icon/icon[print_barcode].GIF" />
											</dx:GridViewCommandColumnCustomButton>
										</CustomButtons>
										<CellStyle Wrap="False">
										</CellStyle>
									</dx:GridViewCommandColumn>
									<dx:GridViewDataTextColumn Caption="ID" FieldName="assets_ID" ReadOnly="True"
										ShowInCustomizationForm="True" VisibleIndex="1" Width="30px">
										<EditFormSettings Visible="False" />
										<EditFormSettings Visible="False"></EditFormSettings>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="No" FieldName="assets_No"
										ShowInCustomizationForm="True" VisibleIndex="2" Width="30px">
										<PropertiesTextEdit Width="100px">
										</PropertiesTextEdit>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Make" FieldName="assets_Make"
										ShowInCustomizationForm="True" VisibleIndex="3" Width="100px">
										<PropertiesTextEdit Width="100px">
										</PropertiesTextEdit>
										<SettingsHeaderFilter Mode="CheckedList">
										</SettingsHeaderFilter>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Model" FieldName="assets_Model"
										ShowInCustomizationForm="True" VisibleIndex="4" Width="100px">
										<PropertiesTextEdit Width="100px">
										</PropertiesTextEdit>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Year" FieldName="assets_Year"
										ShowInCustomizationForm="True" VisibleIndex="5" Width="50px">
										<PropertiesTextEdit Width="100px">
										</PropertiesTextEdit>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Serial / License"
										FieldName="assets_License" ShowInCustomizationForm="True" VisibleIndex="6"
										Width="100px">
										<PropertiesTextEdit Width="100px">
										</PropertiesTextEdit>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataComboBoxColumn Caption="Type" FieldName="assets_type"
										ShowInCustomizationForm="True" VisibleIndex="7" Width="75px">
										<PropertiesComboBox DataSourceID="sqlassettypes" TextField="asset_type_name"
											ValueField="asset_type_id" ValueType="System.Int32" AnimationType="None" Width="100px">
										</PropertiesComboBox>

										<SettingsHeaderFilter Mode="CheckedList">
										</SettingsHeaderFilter>

									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataComboBoxColumn Caption="Status" FieldName="assets_statusid"
										ShowInCustomizationForm="True" VisibleIndex="8" Width="100px">
										<PropertiesComboBox DataSourceID="sqlstatus" TextField="asset_status_status"
											ValueField="asset_status_id" ValueType="System.Int32" AnimationType="None" Width="100px">
										</PropertiesComboBox>
										<SettingsHeaderFilter Mode="CheckedList">
										</SettingsHeaderFilter>
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataImageColumn FieldName="assets_iconpic" ShowInCustomizationForm="True"
										VisibleIndex="9" Width="100px" Caption="Pic">
										<EditFormSettings Visible="False" />

										<EditFormSettings Visible="False"></EditFormSettings>

										<CellStyle HorizontalAlign="Left">
										</CellStyle>
									</dx:GridViewDataImageColumn>
									<dx:GridViewDataComboBoxColumn Caption="Owner" FieldName="assets_owner"
										ShowInCustomizationForm="True" VisibleIndex="10" Width="100px">
										<PropertiesComboBox DataSourceID="sqlmembers" TextField="membername"
											ValueField="member_id" ValueType="System.Int32" AnimationType="None" Width="100px">
										</PropertiesComboBox>
										<SettingsHeaderFilter Mode="CheckedList">
										</SettingsHeaderFilter>
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataMemoColumn Caption="Notes" FieldName="assets_notes"
										ShowInCustomizationForm="True" VisibleIndex="11" Width="100%">
										<PropertiesMemoEdit Height="50px" Width="700px">

											<Style BackColor="#FFFFCC">
											</Style>

										</PropertiesMemoEdit>
										<EditFormSettings ColumnSpan="4" />

										<EditFormSettings ColumnSpan="4"></EditFormSettings>
									</dx:GridViewDataMemoColumn>
									<dx:GridViewDataTextColumn Caption="Transponder" FieldName="transponder"
										ShowInCustomizationForm="True" VisibleIndex="12" Width="60px">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Mileage" FieldName="mileage"
										ShowInCustomizationForm="True" VisibleIndex="13" Width="75px">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Vin No" FieldName="vin_no"
										ShowInCustomizationForm="True" VisibleIndex="15" Width="60px">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataCheckColumn Caption="Capped" FieldName="capped"
										ShowInCustomizationForm="True" VisibleIndex="14" Width="50px">
									</dx:GridViewDataCheckColumn>
									<dx:GridViewDataComboBoxColumn Caption="Parent Asset" FieldName="parent_asset_id"
										ShowInCustomizationForm="True" VisibleIndex="16" Width="100px">
										<PropertiesComboBox DataSourceID="sqlparent_assets" TextField="_name"
											ValueField="asset_ID" ValueType="System.Int32" AnimationType="None" Width="100px">
										</PropertiesComboBox>
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataComboBoxColumn Caption="Branch" FieldName="business_unit_id"
										ShowInCustomizationForm="True" VisibleIndex="17">
										<PropertiesComboBox DataSourceID="ds_business_units" TextField="name" ValueType="System.Int32"
											ValueField="id">
										</PropertiesComboBox>
										<Settings HeaderFilterMode="CheckedList" />
										<SettingsHeaderFilter Mode="CheckedList">
										</SettingsHeaderFilter>
									</dx:GridViewDataComboBoxColumn>
									<dx:GridViewDataCheckColumn Caption="Show On Scheduler" FieldName="show_on_scheduler"
										ShowInCustomizationForm="True" VisibleIndex="18" Width="50px">
									</dx:GridViewDataCheckColumn>
									<dx:GridViewDataTextColumn Caption="Lease No" FieldName="lease_no"
										ShowInCustomizationForm="True" VisibleIndex="19" Width="60px">
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Daily" FieldName="daily"
										ShowInCustomizationForm="True" VisibleIndex="20" Width="60px">
										<PropertiesTextEdit DisplayFormatString="c" DisplayFormatInEditMode="true">
										</PropertiesTextEdit>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Weekly" FieldName="weekly"
										ShowInCustomizationForm="True" VisibleIndex="21" Width="60px">
										<PropertiesTextEdit DisplayFormatString="c" DisplayFormatInEditMode="true">
										</PropertiesTextEdit>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Monthly" FieldName="monthly"
										ShowInCustomizationForm="True" VisibleIndex="22" Width="60px">
										<PropertiesTextEdit DisplayFormatString="c" DisplayFormatInEditMode="true">
										</PropertiesTextEdit>
									</dx:GridViewDataTextColumn>

									<dx:GridViewDataTextColumn Caption="Calibration Frequency (# of days)" FieldName="calib_freq"
										ShowInCustomizationForm="True" VisibleIndex="23" Width="60px">
										<PropertiesTextEdit DisplayFormatString="D" DisplayFormatInEditMode="true">
										</PropertiesTextEdit>
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataDateColumn Caption="Next calibration due" FieldName="next_calib_date"
										ShowInCustomizationForm="True" VisibleIndex="24" Width="120px">
										<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" ClientSideEvents-DateChanged="function(s,e){if(gv_assets.GetEditor('needs_calib').GetValue() == false){alert('Please first check that this asset needs calibration before stating a date');s.SetValue(null);}}"
											EditFormatString="yyyy-MM-dd" AnimationType="None">
<ClientSideEvents DateChanged="function(s,e){if(gv_assets.GetEditor(&#39;needs_calib&#39;).GetValue() == false){alert(&#39;Please first check that this asset needs calibration before stating a date&#39;);s.SetValue(null);}}"></ClientSideEvents>
										</PropertiesDateEdit>
									</dx:GridViewDataDateColumn>
									<dx:GridViewDataCheckColumn Caption="Requires Maintenance" FieldName="requires_maintenance"
										ShowInCustomizationForm="True" VisibleIndex="25" Width="50px">
									</dx:GridViewDataCheckColumn>
									<dx:GridViewDataCheckColumn Caption="Needs Calibration" FieldName="needs_calib"
										ShowInCustomizationForm="True" VisibleIndex="26" Width="50px">
										<PropertiesCheckEdit ClientSideEvents-CheckedChanged="function(s,e){if(!s.GetChecked()){gv_assets.GetEditor('next_calib_date').SetValue(null);}}" >
<ClientSideEvents CheckedChanged="function(s,e){if(!s.GetChecked()){gv_assets.GetEditor(&#39;next_calib_date&#39;).SetValue(null);}}"></ClientSideEvents>
										</PropertiesCheckEdit>
									</dx:GridViewDataCheckColumn>
									<dx:GridViewDataCheckColumn Caption="Active flag" FieldName="active"
										ShowInCustomizationForm="True" VisibleIndex="27" Width="50px">
									</dx:GridViewDataCheckColumn>
									<dx:GridViewDataMemoColumn Caption="Description" FieldName="description"
										ShowInCustomizationForm="True" VisibleIndex="30" Width="200px">
										<PropertiesMemoEdit Height="50px" Width="700px">
											<Style BackColor="#FFFFCC">
											</Style>
										</PropertiesMemoEdit>
										<EditFormSettings ColumnSpan="8" />
									</dx:GridViewDataMemoColumn>
								</Columns>
								<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True"
									ConfirmDelete="True" />
								<SettingsEditing EditFormColumnCount="4" Mode="PopupEditForm" />
								<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowGroupPanel="True"
									ShowHeaderFilterButton="True" />
								<SettingsPager PageSize="50" />
								<SettingsText CommandClearFilter="Clear"
									PopupEditFormCaption="Add / Edit Asset" />
								<SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides"
									CustomizationWindow-VerticalAlign="TopSides" EditForm-Height="700px" EditForm-HorizontalAlign="WindowCenter"
									EditForm-Modal="True" EditForm-VerticalAlign="WindowCenter"
									EditForm-Width="800px">


									<EditForm Width="800px" Height="700px" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Modal="True"></EditForm>

									<CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="TopSides"></CustomizationWindow>
								</SettingsPopup>


								<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control" ConfirmDelete="True"></SettingsBehavior>

								<SettingsEditing Mode="PopupEditForm" EditFormColumnCount="4"></SettingsEditing>

								<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowGroupPanel="True" UseFixedTableLayout="True"></Settings>

								<SettingsText PopupEditFormCaption="Add / Edit Asset" CommandClearFilter="Clear"></SettingsText>

								<SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides" EditForm-Width="800px" EditForm-Height="700px" EditForm-HorizontalAlign="WindowCenter" EditForm-VerticalAlign="WindowCenter" EditForm-Modal="True" />

								<Styles>
								</Styles>
								<StylesPopup
									EditForm-Content-Paddings-Padding="10px"
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




										<dx:ASPxPageControl ID="pc_Asset" ClientInstanceName="pc_Asset" runat="server" ActiveTabIndex="0" Width="100%" Theme="NETheme01" OnCallback="pc_Asset_Callback">
										<ClientSideEvents ActiveTabChanged="Validate.TabPageRefresh" />
											<TabPages>
												<dx:TabPage Text="Details">
													<ContentCollection>
														<dx:ContentControl ID="ContentControl3" runat="server" SupportsDisabledAttribute="True">
										<table width="100%" cellpadding="5" cellspacing="0" id="DivValidationContainer">
											<tr class="required">
												<td colspan="2">
													<dx:ASPxMemo ID="edit_description" runat="server" Caption="Description" CaptionSettings-Position="Top" Theme="NETheme01" Width="100%" Height="50px">
														<ValidationSettings RequiredField-IsRequired="true" ValidationGroup="assetEdit"></ValidationSettings>
													</dx:ASPxMemo>
												</td>
											</tr>
											<tr class="required">
												<td width="50%">
													<dx:ASPxComboBox ID="edit_branch" runat="server" DataSourceID="ds_business_units" ValueField="id" ValueType="System.Int32" TextField="name" Theme="NETheme01" Caption="Business Unit" CaptionCellStyle-Width="100px">
														<ValidationSettings RequiredField-IsRequired="true" ValidationGroup="assetEdit"></ValidationSettings>
													</dx:ASPxComboBox></td>
												<td width="50%">
													<dx:ASPxComboBox ID="edit_owner" runat="server" DataSourceID="sqlmembers" ValueField="member_id" ValueType="System.Int32" TextField="membername" Theme="NETheme01" Caption="Owner" CaptionCellStyle-Width="100px">
														<ValidationSettings RequiredField-IsRequired="true" ValidationGroup="assetEdit"></ValidationSettings>
									                </dx:ASPxComboBox>
												</td>
											</tr>
											<tr class="required">
												<td class="endrow"><dx:ASPxComboBox ID="edit_type" runat="server" DataSourceID="sqlassettypes" ValueField="asset_type_id" TextField="asset_type_name" ValueType="System.Int32" Theme="NETheme01" Caption="Type" CaptionCellStyle-Width="100px">
														<ValidationSettings RequiredField-IsRequired="true" ValidationGroup="assetEdit"></ValidationSettings>
												    </dx:ASPxComboBox>
												</td>
												<td class="endrow">
													<dx:ASPxComboBox ID="edit_status" runat="server" Caption="Status" DataSourceID="sqlstatus" ValueField="asset_status_id" TextField="asset_status_status" ValueType="System.Int32" CaptionCellStyle-Width="100px" Theme="NETheme01">
														<ValidationSettings RequiredField-IsRequired="true" ValidationGroup="assetEdit"></ValidationSettings>
													</dx:ASPxComboBox>
												</td>
											</tr>
											<tr>
												<td><dx:ASPxTextBox ID="edit_make" runat="server" Theme="NETheme01" Caption="Make" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled"></dx:ASPxTextBox></td>
												<td><dx:ASPxTextBox ID="edit_model" runat="server" Theme="NETheme01" Caption="Model" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled"></dx:ASPxTextBox></td>
											</tr>
											<tr>
												<td><dx:ASPxTextBox ID="edit_transponder" runat="server" Theme="NETheme01" Caption="Transponder" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled"></dx:ASPxTextBox></td>
												<td><dx:ASPxTextBox ID="edit_mileage" runat="server" Theme="NETheme01" Caption="Mileage" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled">
												<ClientSideEvents KeyDown="function(s, e) {
													only_int(e.htmlEvent);
												}" />
												    </dx:ASPxTextBox></td>
											</tr>
											<tr>
												<td><dx:ASPxTextBox ID="edit_vin" runat="server" Theme="NETheme01" Caption="VIN #" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled"></dx:ASPxTextBox></td>
												<td>
													<dx:ASPxTextBox ID="edit_year" runat="server" Theme="NETheme01" Caption="Year" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled">
													<ClientSideEvents KeyDown="function(s, e) {
													only_int(e.htmlEvent);
												}" />
													</dx:ASPxTextBox>
												</td>
											</tr>
											<tr>
												<td><dx:ASPxTextBox ID="edit_leaseno" runat="server" Theme="NETheme01" Caption="Lease #" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled"></dx:ASPxTextBox></td>
												<td><dx:ASPxTextBox ID="edit_daily" runat="server" Theme="NETheme01" Caption="Daily" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled">
												<ClientSideEvents KeyDown="function(s, e) {
													only_numeric(e.htmlEvent);
												}" />
												</dx:ASPxTextBox></td>
											</tr>
											<tr>
												<td><dx:ASPxTextBox ID="edit_number" runat="server" Caption="Number" CaptionCellStyle-Width="100px" Theme="NETheme01" Height="150px" AutoCompleteType="Disabled">
												    </dx:ASPxTextBox></td>
												<td><dx:ASPxTextBox ID="edit_weekly" runat="server" Theme="NETheme01" Caption="Weekly" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled">
												<ClientSideEvents KeyDown="function(s, e) {
													only_numeric(e.htmlEvent);
												}" />
												    </dx:ASPxTextBox></td>
											</tr>
											<tr>
												<td>
													<dx:ASPxTextBox ID="edit_calibrationfrequency" ClientInstanceName="calibrationfrequency" runat="server" Caption="Calib. Frequency" CaptionCellStyle-Width="100px" Theme="NETheme01" AutoCompleteType="Disabled">
												   <ClientSideEvents KeyDown="function(s, e) {
													only_int(e.htmlEvent);
												}" />
												   </dx:ASPxTextBox>
												</td>
												<td><dx:ASPxTextBox ID="edit_monthly" runat="server" Theme="NETheme01" Caption="Monthly" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled">
												<ClientSideEvents KeyDown="function(s, e) {
													only_numeric(e.htmlEvent);
												}" />
												    </dx:ASPxTextBox></td>
											</tr>
											<tr>
												<td class="endrow"><dx:ASPxTextBox ID="edit_serial_license" runat="server" Theme="NETheme01" Caption="Serial/License #" CaptionCellStyle-Width="100px" AutoCompleteType="Disabled"></dx:ASPxTextBox></td>
												<td class="endrow"><dx:ASPxDateEdit ID="edit_nextcalibrationdate" ClientInstanceName="edit_nextcalibrationdate" ToolTip="Automatically generated based on calibration frequency." runat="server" ClientEnabled="false" Theme="NETheme01" Caption="Next Calib. Date" CaptionCellStyle-Width="100px">
												

												    </dx:ASPxDateEdit></td>
											</tr>
											<tr>
												<td>
													<dx:ASPxCheckBox ID="edit_capped" runat="server" Text="Capped?" Theme="NETheme01">
													</dx:ASPxCheckBox>
												</td>
												<td>
													<dx:ASPxCheckBox ID="edit_active" runat="server" Text="Active?" Theme="NETheme01">
													</dx:ASPxCheckBox>
												</td>
											</tr>
											<tr>
												<td><dx:ASPxCheckBox ID="edit_requiresmaintenance" runat="server" Theme="NETheme01" Text="Requires Maintenance?"></dx:ASPxCheckBox></td>
												<td>
													<dx:ASPxCheckBox ID="edit_needscalibration" ClientInstanceName="needscalibration" runat="server" Theme="NETheme01" Text="Enable Calibration Schedule?">
														<ClientSideEvents CheckedChanged="function(s,e){if(s.GetChecked()){calibrationfrequency.Focus();}}" />
												    </dx:ASPxCheckBox>
												</td>
											</tr>
											<tr>
												<td class="endrow"><dx:ASPxCheckBox ID="edit_showonscheduler" runat="server" Theme="NETheme01" Text="Show on Scheduler?"></dx:ASPxCheckBox></td>
												<td class="endrow">&nbsp;</td>
											</tr>
											<tr>
												<td colspan="2">
													<dx:ASPxMemo ID="edit_notes" runat="server" Theme="NETheme01" Caption="Notes" CaptionSettings-Position="Top" Width="100%" Height="100px"></dx:ASPxMemo>
												</td>
											</tr>
											<tr>
												<td align="center">
													<dx:ASPxButton ID="edit_cancel" runat="server" Text="Cancel" AutoPostBack="false" Theme="NETheme01" Width="75%">
														<ClientSideEvents Click="function(s,e){gv_assets.CancelEdit();}" />
													</dx:ASPxButton>
												</td>
												<td align="center">
													<dx:ASPxButton ID="edit_save" runat="server" Text="Save" Theme="NETheme01" AutoPostBack="false" Width="75%" ValidationContainerID="DivValidationContainer">
														<ClientSideEvents Click="validateEdit" />
													</dx:ASPxButton>
												</td>
											</tr>
										</table>
														</dx:ContentControl>
													</ContentCollection>
												</dx:TabPage>
												<dx:TabPage Text="Image">
													<ContentCollection>
														<dx:ContentControl ID="ContentControl5" runat="server" SupportsDisabledAttribute="True">
													<table style="width: 100%;">
														<tr bgcolor="#CCCCCC">
															<td bgcolor="#EAEAEA" width="150">
																<dx:ASPxImage ID="img1" runat="server" ClientInstanceName="img1" Width="150px" EnableClientSideAPI="True">
																</dx:ASPxImage>
															</td>
															<td bgcolor="#EAEAEA">Image for this Asset:
															<dx:ASPxUploadControl ID="upload" runat="server"
																ClientInstanceName="upload" OnFileUploadComplete="upload_FileUploadComplete"
																ShowProgressPanel="True" ShowUploadButton="True" Width="280px">
																<ClientSideEvents FileUploadComplete="function(s, e) {img1.SetImageUrl(e.callbackData);gv_assets.Refresh();}" />
																<AdvancedModeSettings TemporaryFolder="c:/inetpub/temp/upload/" />
															</dx:ASPxUploadControl>
															</td>
														</tr>
													</table>
														</dx:ContentControl>
													</ContentCollection>
												</dx:TabPage>
												<dx:TabPage Text="Schedule/Calibration">
													<ContentCollection>
														<dx:ContentControl ID="ContentControl4" runat="server" SupportsDisabledAttribute="True">

										<table style="width: 100%">
											<tr>
												<td class="style4">
													<asp:SqlDataSource ID="sdsAssetSchedule" runat="server"
														ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
														ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
														SelectCommand="SELECT assets_history_id, assets_history_date, assets_history_assetid, assets_history_memberid, assets_history_dollars, assets_history_action, assets_history_notes FROM assets_history WHERE assets_history_action != 10 AND assets_history_assetid = @asset_id ORDER BY assets_history_id DESC">
														<SelectParameters>
															<asp:Parameter Name="@asset_id" />
														</SelectParameters>
													</asp:SqlDataSource>
													<asp:SqlDataSource ID="sdsHistoryCalibration" runat="server"
														ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
														ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
														SelectCommand="SELECT assets_history_id, assets_history_date, assets_history_assetid, assets_history_memberid, assets_history_dollars, assets_history_action, assets_history_notes FROM assets_history WHERE assets_history_action = 10 AND assets_history_assetid = @asset_id ORDER BY assets_history_id DESC">
														<SelectParameters>
															<asp:Parameter Name="@asset_id" />
														</SelectParameters>
													</asp:SqlDataSource>
													<dx:ASPxPageControl ID="pcScheduleLog" runat="server" Theme="NETheme01">
														<TabPages>
															<dx:TabPage Text="Schedule/Log">
																<ContentCollection>
																	<dx:ContentControl ID="ContentControl1" runat="server" SupportsDisabledAttribute="True">
																		<dx:ASPxGridView ID="gv_history" runat="server" AutoGenerateColumns="False"
																			ClientInstanceName="gv_history" DataSourceID="sdsAssetSchedule"
																			KeyFieldName="assets_history_id" OnCustomCallback="gv_history_CustomCallback"
																			OnRowDeleting="gv_history_RowDeleting" Width="100%" OnRowUpdating="gv_history_RowUpdating" Theme="NETheme01">
																			<SettingsCommandButton>
																				<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
																				<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
																				<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
																			</SettingsCommandButton>
																			<Columns>
																				<dx:GridViewCommandColumn ButtonType="Image" Caption=" "
																					ShowInCustomizationForm="True" VisibleIndex="0" Width="50px" ShowEditButton="true" ShowDeleteButton="true">
																				</dx:GridViewCommandColumn>
																				<dx:GridViewDataTextColumn FieldName="assets_history_id" ReadOnly="True"
																					ShowInCustomizationForm="True" Visible="False" VisibleIndex="1" Width="0px">
																					<EditFormSettings Visible="False" />
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataDateColumn Caption="Date" FieldName="assets_history_date"
																					ShowInCustomizationForm="True" VisibleIndex="2" Width="120px">
																					<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom"
																						EditFormatString="yyyy-MM-dd" AnimationType="None">
																					</PropertiesDateEdit>
																				</dx:GridViewDataDateColumn>
																				<dx:GridViewDataTextColumn FieldName="assets_history_assetid"
																					ShowInCustomizationForm="True" Visible="False" VisibleIndex="3" Width="0px">
																					<EditFormSettings Visible="False" />
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataComboBoxColumn Caption="Member"
																					FieldName="assets_history_memberid" ShowInCustomizationForm="True"
																					VisibleIndex="4" Width="100px">
																					<PropertiesComboBox DataSourceID="sqlmembers" TextField="membername"
																						ValueField="member_id" ValueType="System.Int32" AnimationType="None">
																					</PropertiesComboBox>
																				</dx:GridViewDataComboBoxColumn>
																				<dx:GridViewDataTextColumn Caption="Cost" FieldName="assets_history_dollars"
																					ShowInCustomizationForm="True" VisibleIndex="5" Width="50px">
																					<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataComboBoxColumn Caption="Action"
																					FieldName="assets_history_action" ShowInCustomizationForm="True"
																					VisibleIndex="6" Width="200px">
																					<PropertiesComboBox DataSourceID="sqlhistorytype"
																						TextField="asset_history_action_name" ValueField="asset_history_action_id"
																						ValueType="System.Int32" AnimationType="None">
																					</PropertiesComboBox>
																				</dx:GridViewDataComboBoxColumn>
																				<dx:GridViewDataMemoColumn Caption="Notes" FieldName="assets_history_notes"
																					ShowInCustomizationForm="True" VisibleIndex="7" Width="100%">
																					<PropertiesMemoEdit>
																						<Style BackColor="#FFFFCC"></Style>
																					</PropertiesMemoEdit>
																				</dx:GridViewDataMemoColumn>
																			</Columns>
																			<ClientSideEvents BeginCallback="function(s,e){please_wait('start');}" EndCallback="function(s,e){please_wait('stop');txtdollars.SetText('');memaddhistorynotes.SetText('');}" CallbackError="function(s,e){please_wait('start');}" />
																			<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
																			<Settings ShowTitlePanel="True" />
																			<Styles>
																				<TitlePanel BackColor="#EAEAEA" Font-Names="Arial" Font-Size="9pt"
																					ForeColor="Black">
																				</TitlePanel>
																			</Styles>
																			<Templates>
																				<TitlePanel>
																					<table style="width: 100%;">
																						<tr style="color: #000000">
																							<td>
																								<strong style="font-size: 9pt">Date</strong></td>
																							<td>
																								<strong style="font-size: 9pt">Member</strong></td>
																							<td>
																								<strong style="font-size: 9pt">Action</strong></td>
																							<td>
																								<strong style="font-size: 9pt">Price</strong></td>
																							<td>&nbsp;</td>
																						</tr>
																						<tr>
																							<td>
																								<dx:ASPxDateEdit ID="dte_addhistory" runat="server"
																									ClientInstanceName="dte_addhistory" DisplayFormatString="yyyy-MM-dd"
																									EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None"
																									NullText="Select Date" OnDataBound="dte_addhistory_DataBound" Theme="NETheme01">
																									<ValidationSettings ValidationGroup="HistoryItem" RequiredField-IsRequired="true"></ValidationSettings>
																								</dx:ASPxDateEdit>
																							</td>
																							<td>
																								<dx:ASPxComboBox ID="ddl_addhistorymember" runat="server"
																									ClientInstanceName="ddl_addhistorymember" DataSourceID="sqlmembers"
																									AnimationType="None" OnDataBound="ddl_addhistorymember_DataBound"
																									TextField="membername" ValueField="member_id" ValueType="System.Int32" Theme="NETheme01">
																									<ValidationSettings ValidationGroup="HistoryItem" RequiredField-IsRequired="true"></ValidationSettings>
																								</dx:ASPxComboBox>
																							</td>
																							<td>
																								<dx:ASPxComboBox ID="ddlaction" runat="server" ClientInstanceName="ddlaction"
																									DataSourceID="sqlhistorytype" AnimationType="None"
																									TextField="asset_history_action_name"
																									ValueField="asset_history_action_id" ValueType="System.Int32" Theme="NETheme01">
																									<ValidationSettings ValidationGroup="HistoryItem" RequiredField-IsRequired="true"></ValidationSettings>
																								</dx:ASPxComboBox>
																							</td>
																							<td>
																								<dx:ASPxTextBox ID="txtdollars" runat="server" ClientInstanceName="txtdollars" Width="60px" Theme="NETheme01">
																									<MaskSettings Mask="<0..999g>.<0..99g>" />
																								</dx:ASPxTextBox>
																							</td>
																							<td rowspan="2">
																								<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Save"
																									Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Height="60px"
																									HorizontalAlign="Center" Width="80px" Theme="NETheme01">
																									<ClientSideEvents Click="Validate.HistoryItem" />
																									<Image Url="~/images/icon/icon[add].gif">
																									</Image>
																								</dx:ASPxButton>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="4">
																								<dx:ASPxMemo ID="memaddhistorynotes" runat="server" BackColor="#FFFFCC"
																									ClientInstanceName="memaddhistorynotes" Height="31px" Width="100%" Theme="NETheme01">
																								</dx:ASPxMemo>
																							</td>
																							<td>&nbsp;</td>
																						</tr>
																					</table>
																				</TitlePanel>
																				<EditForm>
																					<div class="style5">
																						<dx:ASPxGridViewTemplateReplacement ID="Editors" runat="server"
																							ReplacementType="EditFormEditors" />
																					</div>
																					<div style="margin-top: 10px; margin-bottom: 20px; padding-bottom: 20px;">
																						<div style="float: left; margin-left: 5px">
																							<dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false"
																								ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>'
																								Text="Cancel" Width="100px" Theme="NETheme01" />
																						</div>
																						<div style="float: left; margin-left: 10px">
																							<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false"
																								ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>'
																								CssClass="input" Text="Save" Width="100px" Theme="NETheme01" />
																						</div>
																					</div>
																				</EditForm>
																			</Templates>
																		</dx:ASPxGridView>

																		<asp:SqlDataSource ID="sqlhistorytype" runat="server"
																			ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																			SelectCommand="SELECT asset_history_action_id, asset_history_action_name FROM asset_history_action WHERE asset_history_action_id != 10"></asp:SqlDataSource>
																	</dx:ContentControl>
																</ContentCollection>
															</dx:TabPage>
															<dx:TabPage Text="Calibration">
																<ContentCollection>
																	<dx:ContentControl ID="ContentControl2" runat="server" SupportsDisabledAttribute="True">
																		<dx:ASPxGridView ID="gv_calibration" runat="server" AutoGenerateColumns="False"
																			ClientInstanceName="gv_calibration" DataSourceID="sdsHistoryCalibration"
																			KeyFieldName="assets_history_id" OnCustomCallback="gv_calibration_CustomCallback"
																			OnRowDeleting="gv_history_RowDeleting" Width="100%" OnRowUpdating="gv_history_RowUpdating" Theme="NETheme01">
																			<SettingsCommandButton>
																				<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
																				<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
																				<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
																			</SettingsCommandButton>
																			<ClientSideEvents BeginCallback="function(s,e){please_wait('start');}" EndCallback="function(s,e){please_wait('stop');txtdollars_calibration.SetText('');memaddhistorynotes_calibration.SetText('');}" CallbackError="function(s,e){please_wait('start');}" />
																			<Columns>
																				<dx:GridViewCommandColumn ButtonType="Image" Caption=" "
																					ShowInCustomizationForm="True" VisibleIndex="0" Width="50px" ShowEditButton="true" ShowDeleteButton="true">
																				</dx:GridViewCommandColumn>
																				<dx:GridViewDataTextColumn FieldName="assets_history_id" ReadOnly="True"
																					ShowInCustomizationForm="True" Visible="False" VisibleIndex="1" Width="0px">
																					<EditFormSettings Visible="False" />
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataDateColumn Caption="Date" FieldName="assets_history_date"
																					ShowInCustomizationForm="True" VisibleIndex="2" Width="120px">
																					<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom"
																						EditFormatString="yyyy-MM-dd" AnimationType="None">
																					</PropertiesDateEdit>
																				</dx:GridViewDataDateColumn>
																				<dx:GridViewDataTextColumn FieldName="assets_history_assetid"
																					ShowInCustomizationForm="True" Visible="False" VisibleIndex="3" Width="0px">
																					<EditFormSettings Visible="False" />
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataComboBoxColumn Caption="Member"
																					FieldName="assets_history_memberid" ShowInCustomizationForm="True"
																					VisibleIndex="4" Width="100px">
																					<PropertiesComboBox DataSourceID="sqlmembers" TextField="membername"
																						ValueField="member_id" ValueType="System.Int32" AnimationType="None">
																					</PropertiesComboBox>
																				</dx:GridViewDataComboBoxColumn>
																				<dx:GridViewDataTextColumn Caption="Cost" FieldName="assets_history_dollars"
																					ShowInCustomizationForm="True" VisibleIndex="5" Width="50px">
																					<PropertiesTextEdit DisplayFormatString="C2"></PropertiesTextEdit>
																				</dx:GridViewDataTextColumn>
																				<dx:GridViewDataComboBoxColumn Caption="Action"
																					FieldName="assets_history_action" ShowInCustomizationForm="True" EditFormSettings-Visible="False"
																					VisibleIndex="6" Width="200px">
																					<PropertiesComboBox DataSourceID="sdsCalibrationType"
																						TextField="asset_history_action_name" ValueField="asset_history_action_id"
																						ValueType="System.Int32" AnimationType="None">
																					</PropertiesComboBox>
																				</dx:GridViewDataComboBoxColumn>
																				<dx:GridViewDataMemoColumn Caption="Notes" FieldName="assets_history_notes"
																					ShowInCustomizationForm="True" VisibleIndex="7" Width="100%">
																					<PropertiesMemoEdit>
																						<Style BackColor="#FFFFCC"></Style>
																					</PropertiesMemoEdit>
																				</dx:GridViewDataMemoColumn>
																			</Columns>
																			<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
																			<Settings ShowTitlePanel="True" />
																			<Styles>
																				<TitlePanel BackColor="#EAEAEA" Font-Names="Arial" Font-Size="9pt"
																					ForeColor="Black">
																				</TitlePanel>
																			</Styles>
																			<Templates>
																				<TitlePanel>
																					<table style="width: 100%;">
																						<tr style="color: #000000">
																							<td>
																								<strong style="font-size: 9pt">Date</strong></td>
																							<td>
																								<strong style="font-size: 9pt">Member</strong></td>
																							<td>
																								<strong style="font-size: 9pt">Action</strong></td>
																							<td>
																								<strong style="font-size: 9pt">Price</strong></td>
																							<td>&nbsp;</td>
																						</tr>
																						<tr>
																							<td>
																								<dx:ASPxDateEdit ID="dte_addhistory_calibration" runat="server"
																									ClientInstanceName="dte_addhistory_calibration" DisplayFormatString="yyyy-MM-dd"
																									EditFormat="Custom" EditFormatString="yyyy-MM-dd" AnimationType="None"
																									NullText="Select Date" OnDataBound="dte_addhistory_DataBound" Theme="NETheme01">
																									<ValidationSettings RequiredField-IsRequired="true" ValidationGroup="CalibrationItem"></ValidationSettings>
																								</dx:ASPxDateEdit>
																							</td>
																							<td>
																								<dx:ASPxComboBox ID="ddl_addhistorymember_calibration" runat="server"
																									ClientInstanceName="ddl_addhistorymember_calibration" DataSourceID="sqlmembers"
																									AnimationType="None" OnDataBound="ddl_addhistorymember_DataBound"
																									TextField="membername" ValueField="member_id" ValueType="System.Int32" Theme="NETheme01">
																									<ValidationSettings RequiredField-IsRequired="true" ValidationGroup="CalibrationItem"></ValidationSettings>
																								</dx:ASPxComboBox>
																							</td>
																							<td>
																								<dx:ASPxComboBox ID="ddlaction_calibration" SelectedIndex="0" runat="server" ClientInstanceName="ddlaction_calibration"
																									DataSourceID="sdsCalibrationType" AnimationType="None"
																									TextField="asset_history_action_name"
																									ValueField="asset_history_action_id" ValueType="System.Int32" Theme="NETheme01">
																									<ValidationSettings RequiredField-IsRequired="true" ValidationGroup="CalibrationItem"></ValidationSettings>
																								</dx:ASPxComboBox>
																							</td>
																							<td>
																								<dx:ASPxTextBox ID="txtdollars_calibration" runat="server" ClientInstanceName="txtdollars_calibration"
																									Width="60px" Theme="NETheme01">
																									<MaskSettings Mask="<0..999g>.<0..99g>" />
																								</dx:ASPxTextBox>
																							</td>
																							<td rowspan="2">
																								<dx:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Save"
																									Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Height="60px"
																									HorizontalAlign="Center" Width="80px" Theme="NETheme01">
																									<ClientSideEvents Click="Validate.CalibrationItem" />
																									<Image Url="~/images/icon/icon[add].gif">
																									</Image>
																								</dx:ASPxButton>
																							</td>
																						</tr>
																						<tr>
																							<td colspan="4">
																								<dx:ASPxMemo ID="memaddhistorynotes_calibration" runat="server" BackColor="#FFFFCC"
																									ClientInstanceName="memaddhistorynotes_calibration" Height="31px" Width="100%" Theme="NETheme01">
																								</dx:ASPxMemo>
																							</td>
																							<td>&nbsp;</td>
																						</tr>
																					</table>
																				</TitlePanel>
																				<EditForm>
																					<div class="style5">
																						
																						<dx:ASPxGridViewTemplateReplacement ID="Editors" runat="server"
																							ReplacementType="EditFormEditors" />
																					</div>
																					<div style="margin-top: 10px; margin-bottom: 20px; padding-bottom: 20px;">
																						<div style="float: left; margin-left: 5px">
																							<dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false"
																								ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>'
																								Text="Cancel" Width="100px" Theme="NETheme01" />
																						</div>
																						<div style="float: left; margin-left: 10px">
																							<dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false"
																								ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>'
																								CssClass="input" Text="Save" Width="100px" Theme="NETheme01" />
																						</div>
																					</div>
																				</EditForm>
																			</Templates>
																		</dx:ASPxGridView>

																		<asp:SqlDataSource ID="sdsCalibrationType" runat="server"
																			ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
																			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
																			SelectCommand="SELECT asset_history_action_id, asset_history_action_name FROM asset_history_action WHERE asset_history_action_id = 10"></asp:SqlDataSource>
																	</dx:ContentControl>
																</ContentCollection>
															</dx:TabPage>
														</TabPages>
													</dx:ASPxPageControl>
												</td>
											</tr>
										</table>
														</dx:ContentControl>
													</ContentCollection>
												</dx:TabPage>
											</TabPages>
										</dx:ASPxPageControl>
										<br />
									</EditForm>
								</Templates>
								<GroupSummary>
									<dx:ASPxSummaryItem FieldName="business_unit_id" ShowInColumn="Branch" SummaryType="Sum" />
								</GroupSummary>
							</dx:ASPxGridView>
						</td>

					</tr>

				</table>
			</dx:PanelContent>
		</PanelCollection>
	</dx:ASPxCallbackPanel>

</asp:Content>
