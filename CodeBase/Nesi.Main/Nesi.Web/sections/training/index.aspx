<%@ Page Title="Training" Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="training" Codebehind="index.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>





<%@ Register assembly="DevExpress.Web.ASPxScheduler.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxScheduler" tagprefix="dxwschs" %>
<%@ Register assembly="DevExpress.XtraScheduler.v19.2.Core, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>



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
	<div id="main">
	  <asp:ScriptManager ID="sm" runat="server">
	  </asp:ScriptManager>
	  <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
		  AssociatedUpdatePanelID="UpdatePanel1">
	  </asp:UpdateProgress>
	  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		  <ContentTemplate>
		<dx:ASPxComboBox ID="ddlcompany" runat="server" AutoPostBack="True" 
			DataSourceID="ds_companies" 
			onselectedindexchanged="ddlcompany_SelectedIndexChanged" 
			TextField="name" ValueField="id" ValueType="System.Int32" Theme="NETheme01">
		</dx:ASPxComboBox>
		<asp:SqlDataSource ID="ds_companies" runat="server" 
			ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>">
		</asp:SqlDataSource>
	<br />
			  <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" 
				  Width="100%" Theme="NETheme01">
				  <TabPages>
					  <dx:TabPage Text="Admin" ClientEnabled="False">
						  <TabImage Url="~/images/icon/icon[depthead].gif">
						  </TabImage>
						  <ContentCollection>
							  <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
								  <table style="width:100%;">
									  <tr>
										  <td class="style2">
											  </td>
									  </tr>
									  <tr>
										  <td>
											  <dx:ASPxGridView ID="gv_modules" runat="server" AutoGenerateColumns="False" 
												  DataSourceID="ds_training_modules" EnableCallBacks="False" Font-Names="Arial" 
												  KeyFieldName="id" 
												  OnHtmlRowPrepared="gv_modules_HtmlRowPrepared" 
												  OnRowDeleting="gv_modules_RowDeleting" OnRowInserting="gv_modules_RowInserting" 
												  OnRowUpdating="gv_modules_RowUpdating" Width="100%">
												  <ClientSideEvents CallbackError="function(s, e) {
	inspect(e);
	alert(&quot;&quot;);
}" />
												  <SettingsCommandButton>
	                                                <EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px">
                                                        <Image Height="16px" Url="~/images/icon/icon[edit].gif" Width="16px">
                                                        </Image>
                                                      </EditButton>
	                                                <NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px">
                                                        <Image Height="16px" Url="~/images/icon/icon[add].gif" Width="16px">
                                                        </Image>
                                                      </NewButton>
	                                                <DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px">
                                                        <Image Height="16px" Url="~/images/icon/icon[delete].gif" Width="16px">
                                                        </Image>
                                                      </DeleteButton>
                                                </SettingsCommandButton>
                                                  <Columns>
													  <dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
														  ShowInCustomizationForm="True" VisibleIndex="0" Width="75px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"  ShowNewButton="true">
														  
														  												  
														  
														  <CellStyle Wrap="False">
														  </CellStyle>
													  </dx:GridViewCommandColumn>
													  <dx:GridViewDataTextColumn Caption="ID" FieldName="id" 
														  ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="1" Width="20px">
														  <EditFormSettings Visible="False" />
														  <EditFormSettings Visible="False" />
													  </dx:GridViewDataTextColumn>
													  <dx:GridViewDataTextColumn Caption="Module" FieldName="name" 
														  ShowInCustomizationForm="True" VisibleIndex="2" Width="200px">
														  <CellStyle Wrap="False">
														  </CellStyle>
													  </dx:GridViewDataTextColumn>
													  <dx:GridViewDataTextColumn Caption="Description" 
														  FieldName="description" ShowInCustomizationForm="True" 
														  VisibleIndex="3" Width="100%">
														  <PropertiesTextEdit Height="100px" Width="300px"></PropertiesTextEdit>
													  </dx:GridViewDataTextColumn>
													  <dx:GridViewDataDateColumn Caption="Created" 
														  FieldName="date_create" ShowInCustomizationForm="True" 
														  Visible="False" VisibleIndex="4" Width="75px">
														  <EditFormSettings Visible="False" />
														  <EditFormSettings Visible="False" />
													  </dx:GridViewDataDateColumn>
													  <dx:GridViewDataComboBoxColumn Caption="Mandatory" 
														  FieldName="is_mandatory" ShowInCustomizationForm="True" 
														  VisibleIndex="5" Width="75px">
														  <PropertiesComboBox><Items><dx:ListEditItem Text="Mandatory" Value="M" /><dx:ListEditItem Text="Elective" Value="E" /></Items></PropertiesComboBox>
													  </dx:GridViewDataComboBoxColumn>
													  <dx:GridViewDataComboBoxColumn Caption="Status" 
														  FieldName="is_active" ShowInCustomizationForm="True" 
														  VisibleIndex="6" Width="75px">
														  <PropertiesComboBox><Items><dx:ListEditItem Text="Active" Value="True" /><dx:ListEditItem Text="Inactive" Value="False" /></Items></PropertiesComboBox>
													  </dx:GridViewDataComboBoxColumn>
													  <dx:GridViewDataTextColumn FieldName="colour" 
														  ShowInCustomizationForm="True" Visible="False" VisibleIndex="8" Width="0px">
													  </dx:GridViewDataTextColumn>
												  	<dx:GridViewDataCheckColumn Caption="Is Certificate?" FieldName="is_certificate" 
														  ShowInCustomizationForm="True" VisibleIndex="7" Width="90px">
														<PropertiesCheckEdit AllowGrayedByClick="False" ValueChecked="True" 
															ValueType="System.Boolean" ValueUnchecked="False">
														</PropertiesCheckEdit>
													  </dx:GridViewDataCheckColumn>
												  </Columns>
												  <SettingsBehavior ColumnResizeMode="Control" 
													  EnableRowHotTrack="True" />
												  <SettingsBehavior ColumnResizeMode="Control" 
													  EnableRowHotTrack="True" />
												  <SettingsPager Mode="ShowAllRecords">
												  </SettingsPager>

                                                  
                                                  <SettingsPopup  EditForm-HorizontalAlign="WindowCenter" EditForm-Modal="True" 
													  EditForm-VerticalAlign="WindowCenter" EditForm-Width="300px">
												      <EditForm HorizontalAlign="WindowCenter" Modal="True" VerticalAlign="WindowCenter" Width="300px" />
                                                  </SettingsPopup>
												  <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
												  <Settings ShowFilterRow="True" ShowTitlePanel="True" />
												  <SettingsText Title="Modules / Certifications (Click on one to see course details below)" />
												  

                                                  <SettingsPopup  EditForm-HorizontalAlign="WindowCenter" EditForm-Modal="True" 
													  EditForm-VerticalAlign="WindowCenter" EditForm-Width="300px"/>
                                                  <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
												  <Settings ShowFilterRow="True" ShowTitlePanel="True" />
												  <SettingsText Title="Modules / Certifications" />
												  
                                                  <Styles>
													  <Header BackColor="#33CC33">
													  </Header>
													  <SelectedRow Font-Bold="False" ForeColor="Black">
													  </SelectedRow>
													  <FocusedRow Font-Bold="True" ForeColor="Black">
													  </FocusedRow>
													  <TitlePanel BackColor="#00CC00" Font-Bold="True" Font-Names="Arial" 
														  HorizontalAlign="Left">
													  </TitlePanel>
												  </Styles>
												  <Templates>
													  <EditForm>
														  <dx:ASPxGridViewTemplateReplacement ID="editors99" runat="server" 
															  ReplacementType="EditFormEditors" />
														  <div style="margin-top: 10px; margin-bottom: 20px; padding-bottom:20px;">
															  <div style="float: left; margin-left: 5px">
																  <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" 
																	  ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' 
																	  Text="Cancel" Width="100px" />
															  </div>
															  <div style="float: left; margin-left: 10px">
																  <dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" 
																	  ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' 
																	  CssClass="input" Text="Save" Width="100px" />
															  </div>
														  </div>
													  </EditForm>
												  </Templates>
											  </dx:ASPxGridView>
											  <asp:SqlDataSource ID="ds_training_modules" runat="server" 
												  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
												  ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
												  
												  SelectCommand="SELECT id, name, description, date_create, is_active, is_mandatory,colour,is_certificate FROM training_module">
											  </asp:SqlDataSource>
											  <asp:SqlDataSource ID="ds_training_type" runat="server" 
												  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
												  ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
												  SelectCommand="SELECT id, name FROM training_type">
											  </asp:SqlDataSource>
										  </td>
									  </tr>
									  <tr>
										  <td>
											  <dx:ASPxGridView ID="gv_curriculum" runat="server" AutoGenerateColumns="False" 
												  ClientInstanceName="gv_curriculum" DataSourceID="ds_curriculum" Font-Names="Arial" 
												  KeyFieldName="id" Width="100%" OnRowDeleting="gv_curriculum_RowDeleting" 
												  OnRowInserting="gv_curriculum_RowInserting" OnRowUpdating="gv_curriculum_RowUpdating">
                                                  <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px">
        <Image Height="16px" Url="~/images/icon/icon[edit].gif" Width="16px">
        </Image>
                                                      </EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px">
        <Image Height="16px" Url="~/images/icon/icon[add].gif" Width="16px">
        </Image>
                                                      </NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px">
        <Image Height="16px" Url="~/images/icon/icon[delete].gif" Width="16px">
        </Image>
                                                      </DeleteButton>
</SettingsCommandButton>
												  <Columns>
													  <dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
														  ShowInCustomizationForm="True" VisibleIndex="0" Width="60px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"   ShowNewButton="true">
														  
														  
														  
														  
														  <CellStyle Wrap="False">
														  </CellStyle>
													  </dx:GridViewCommandColumn>
													  <dx:GridViewDataTextColumn Caption="ID" FieldName="id" 
														  ReadOnly="True" ShowInCustomizationForm="True" Visible="False" VisibleIndex="1" 
														  Width="25px">
														  <EditFormSettings Visible="False" />
													  	<EditFormSettings Visible="False" />
													  </dx:GridViewDataTextColumn>
													  <dx:GridViewDataTextColumn Caption="Day #" FieldName="day" 
														  ShowInCustomizationForm="True" VisibleIndex="4" Width="30px">
													  </dx:GridViewDataTextColumn>
													  <dx:GridViewDataTextColumn Caption="Year #" 
														  FieldName="year" ShowInCustomizationForm="True" 
														  VisibleIndex="3" Width="30px">
													  </dx:GridViewDataTextColumn>
													  <dx:GridViewDataComboBoxColumn Caption="Module" 
														  FieldName="module_id" ShowInCustomizationForm="True" 
														  VisibleIndex="2" Width="250px">
														  <PropertiesComboBox DataSourceID="ds_training_modules" 
															  TextField="name" ValueField="id" 
															  ValueType="System.Int32"></PropertiesComboBox>
													  	<CellStyle Wrap="False">
														  </CellStyle>
													  </dx:GridViewDataComboBoxColumn>
													  <dx:GridViewDataTextColumn Caption="Description" 
														  FieldName="description" ShowInCustomizationForm="True" 
														  VisibleIndex="5" Width="100%">
													  	<PropertiesTextEdit Height="100px" Width="300px"></PropertiesTextEdit>
														  <CellStyle Wrap="False">
														  </CellStyle>
													  </dx:GridViewDataTextColumn>
													  <dx:GridViewDataComboBoxColumn Caption="Type" 
														  FieldName="type_id" ShowInCustomizationForm="True" 
														  VisibleIndex="6" Width="80px">
														  <PropertiesComboBox DataSourceID="ds_training_type" 
															  TextField="name" ValueField="id" 
															  ValueType="System.Int32"></PropertiesComboBox>
													  	<CellStyle Wrap="False">
														  </CellStyle>
													  </dx:GridViewDataComboBoxColumn>
													  <dx:GridViewDataComboBoxColumn Caption="Paid" 
														  FieldName="paid" ShowInCustomizationForm="True" 
														  VisibleIndex="7" Width="80px">
														  <PropertiesComboBox><Items><dx:ListEditItem Text="Paid" Value="True" /><dx:ListEditItem Text="Unpaid" Value="False" /></Items></PropertiesComboBox>
													  	<CellStyle Wrap="False">
														  </CellStyle>
													  </dx:GridViewDataComboBoxColumn>
												  </Columns>
												  <SettingsBehavior ColumnResizeMode="Control" />
												  <SettingsBehavior ColumnResizeMode="Control" />
												  <SettingsPager Mode="ShowAllRecords">
												  </SettingsPager>


                                                  <SettingsPopup EditForm-VerticalAlign="WindowCenter" EditForm-HorizontalAlign="WindowCenter" 
													  EditForm-Width="300px" >
												      <EditForm HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="300px" />
                                                  </SettingsPopup>
												  <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
												  <Settings ShowFilterRow="True" ShowTitlePanel="True" />
												  <SettingsText Title="Module Details" />

                                                  <SettingsPopup EditForm-VerticalAlign="WindowCenter" EditForm-HorizontalAlign="WindowCenter" 
													  EditForm-Width="300px" />
												  <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />
												  <Settings ShowFilterRow="True" ShowTitlePanel="True" />
												  <SettingsText Title="Module Details" />
												  <Styles>
													  <Header BackColor="#FFCC00">
													  </Header>
												  	<TitlePanel BackColor="#FFCC00" Font-Bold="True" Font-Names="Arial" 
														  HorizontalAlign="Left">
													  </TitlePanel>
												  </Styles>
                                                  <StylesPopup EditForm-Content-Paddings-Padding="10px">
												      <EditForm>
                                                          <Content>
                                                              <Paddings Padding="10px" />
                                                          </Content>
                                                      </EditForm>
                                                  </StylesPopup>
												  <Templates>
													  <EditForm>
														  <dx:ASPxGridViewTemplateReplacement ID="editors99" runat="server" 
															  ReplacementType="EditFormEditors" />
														  <div style="margin-top: 10px; margin-bottom: 20px; padding-bottom:20px;">
															  <div style="float: left; margin-left: 5px">
																  <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" 
																	  ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' 
																	  Text="Cancel" Width="100px" />
															  </div>
															  <div style="float: left; margin-left: 10px">
																  <dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" 
																	  ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' 
																	  CssClass="input" Text="Save" Width="100px" />
															  </div>
														  </div>
													  </EditForm>
												  </Templates>
											  </dx:ASPxGridView>
											  <asp:SqlDataSource ID="ds_curriculum" runat="server" 
												  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
												  ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
												  SelectCommand="SELECT a.id, a.day, a.year, a.module_id, a.description, a.type_id, a.paid, b.colour FROM training_curriculum a,training_module b WHERE a.module_id = b.id">
											  </asp:SqlDataSource>
										  </td>
									  </tr>
									  <tr>
										  <td>
											  &nbsp;</td>
									  </tr>
								  </table>
							  </dx:ContentControl>
						  </ContentCollection>
					  </dx:TabPage>
					  <dx:TabPage Text="Schedules" ClientEnabled="False" ClientVisible="False">
						  <TabImage Url="~/images/icon/icon[calendar].gif">
						  </TabImage>
						  <ContentCollection>
							  <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
							  	<dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" ActiveViewType="Week" 
									  AppointmentDataSourceID="ds_schedule" ClientIDMode="AutoID" 
									  GroupType="Date" ResourceDataSourceID="ds_members" Start="2012-04-30">
									<Storage>
										<Appointments>
											<Mappings AppointmentId="Training_schedule_ID" 
												Description="description" End="date_end" ResourceId="trainee_id" 
												Start="date_start" />
											<Mappings AppointmentId="Training_schedule_ID" 
												Description="training_cirriculum_description" End="Training_schedule_EndDate" 
												ResourceId="Training_schedule_Trainee_ID" Start="Training_schedule_StartDate" />
										</Appointments>
										<Resources>
											<Mappings Caption="name" 
												ResourceId="member_id" />
											<Mappings Caption="name" ResourceId="member_id" />
										</Resources>
									</Storage>
									<Views>
										<DayView ResourcesPerPage="1">
											<TimeRulers>
												<cc1:TimeRuler />
											</TimeRulers>
										    <AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4" />
										</DayView>
										<WorkWeekView>
											<TimeRulers>
												<cc1:TimeRuler />
											</TimeRulers>
										    <AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4" />
										</WorkWeekView>
										<WeekView ResourcesPerPage="1">
										</WeekView>
										<MonthView ResourcesPerPage="1">
										</MonthView>
										<TimelineView ResourcesPerPage="1">
											<Scales>
												<cc1:TimeScaleYear Enabled="False" />
												<cc1:TimeScaleQuarter Enabled="False" />
												<cc1:TimeScaleMonth Enabled="False" />
												<cc1:TimeScaleWeek />
												<cc1:TimeScaleDay />
												<cc1:TimeScaleHour Enabled="False" />
												<cc1:TimeScaleFixedInterval Enabled="False" />
											</Scales>
										</TimelineView>
									    <FullWeekView>
                                            <TimeRulers>
                                                <cc1:TimeRuler />
                                            </TimeRulers>
                                            <AppointmentDisplayOptions ColumnPadding-Left="2" ColumnPadding-Right="4" />
                                        </FullWeekView>
									</Views>
									<ClientSideEvents MouseUp="" />
								    <ClientSideEvents MouseUp="" />
								  </dxwschs:ASPxScheduler>
								  <br />
								  <asp:SqlDataSource ID="ds_schedule" runat="server" 
									  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
									  ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
									  SelectCommand="
									  SELECT 
										b.id, 
										b.date_start, 
										b.date_end, 
										b.notes, 
										b.trainee_id, 
										d.name type_name, 
										a.paid, 
										a.description,
										a.day, 
										a.year,
										c.name module_name 
									  FROM training_curriculum a 
									  INNER JOIN training_schedule b ON a.id = b.curriculum_id
									  LEFT OUTER JOIN training_module c ON a.module_id = b.id 
									  LEFT OUTER JOIN training_type d ON a.type_id = d.id">
								  </asp:SqlDataSource>
								  <asp:SqlDataSource ID="ds_members" runat="server" 
									  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
									  ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
									  
									  SelectCommand="Select member_id, MEMBER_NAME(member_id) as name from member where member_status = 'Active'">
								  </asp:SqlDataSource>
								  <br />
							  </dx:ContentControl>
						  </ContentCollection>
					  </dx:TabPage>
					  <dx:TabPage Text="History" ClientEnabled="False">
						  <TabImage Url="~/images/icon/icon[details].gif">
						  </TabImage>
						  <ContentCollection>
							  <dx:ContentControl runat="server" SupportsDisabledAttribute="True">
							  	  <asp:SqlDataSource ID="ds_company_members" runat="server" 
									  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
									  ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
									  
									  SelectCommand="SELECT
member_id id,
MEMBER_NAME(member_id) name 
FROM
member
WHERE
Member_Status = 'Active' and business_unit_id = ?companyid 
order by business_unit_id, name">
								  	<SelectParameters>
										<asp:ControlParameter ControlID="ddlcompany" Name="companyid" 
											PropertyName="Value" />
									  </SelectParameters>
								  </asp:SqlDataSource>
							  	<dx:ASPxGridView ID="gv_history" runat="server" Width="100%" 
									  AutoGenerateColumns="False" 
									  OnInitNewRow="gv_history_InitNewRow" DataSourceID="ds_history" KeyFieldName="id" 
									  OnRowInserting="gv_history_RowInserting" 
									  OnCommandButtonInitialize="gv_history_CommandButtonInitialize" 
									  OnHtmlCommandCellPrepared="gv_history_HtmlCommandCellPrepared" 
									  OnRowDeleting="gv_history_RowDeleting" 
									  OnRowUpdating="gv_history_RowUpdating" Theme="NETheme01">
								    <ClientSideEvents EndCallback="function(s, e) {
	 if (typeof(s.cp_newrowid) != &quot;undefined&quot;) {
        var key = s.cp_newrowid;
        delete s.cp_newrowid;
        s.StartEditRowByKey(key);}
}" />
                                      <SettingsResizing ColumnResizeMode="Control" />
                                      <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px">
        <Image Height="16px" Url="~/images/icon/icon[edit].gif" Width="16px">
        </Image>
                                          </EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px">
        <Image Height="16px" Url="~/images/icon/icon[add].gif" Width="16px">
        </Image>
                                          </NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px">
        <Image Height="16px" Url="~/images/icon/icon[delete].gif" Width="16px">
        </Image>
                                          </DeleteButton>
</SettingsCommandButton>
								    <Columns>
										<dx:GridViewCommandColumn ButtonType="Image" Caption=" " 
											ShowInCustomizationForm="True" VisibleIndex="0" Width="75px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true"   ShowNewButton="true">
											
											
											
											
										</dx:GridViewCommandColumn>
										<dx:GridViewDataDateColumn Caption="Date" FieldName="date" 
											ShowInCustomizationForm="True" VisibleIndex="2" Width="125px">
											<PropertiesDateEdit DisplayFormatInEditMode="True" 
												DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
												EditFormatString="yyyy-MM-dd" AnimationType="None" Width="200px"></PropertiesDateEdit>
										</dx:GridViewDataDateColumn>
										<dx:GridViewDataComboBoxColumn Caption="Module / Cert" FieldName="module_id" 
											ShowInCustomizationForm="True" VisibleIndex="3" Width="200px">
											<PropertiesComboBox TextField="name" ValueField="id" ValueType="System.Int32" 
												Width="200px" DataSourceID="ds_training_modules"></PropertiesComboBox>
										</dx:GridViewDataComboBoxColumn>
										<dx:GridViewDataComboBoxColumn Caption="Member" FieldName="member_id" 
											ShowInCustomizationForm="True" VisibleIndex="4" Width="150px">
											<PropertiesComboBox TextField="name" ValueField="id" ValueType="System.Int32" 
												Width="200px" DataSourceID="ds_company_members"></PropertiesComboBox>
										</dx:GridViewDataComboBoxColumn>
										<dx:GridViewDataMemoColumn Caption="Notes" FieldName="notes" 
											ShowInCustomizationForm="True" VisibleIndex="5" Width="200px">
											<PropertiesMemoEdit Height="100px" Width="400px"></PropertiesMemoEdit>
										</dx:GridViewDataMemoColumn>
										<dx:GridViewDataTextColumn Caption="Teacher" FieldName="teacher_name" 
											ShowInCustomizationForm="True" VisibleIndex="7" Width="100px">
											<PropertiesTextEdit Width="200px" ></PropertiesTextEdit>
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="Files" ShowInCustomizationForm="True" 
											VisibleIndex="10" Width="400px" Visible="False">
											
											<Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
											
											<EditFormSettings Visible="True" Caption="Member Files" />
											
											<EditFormSettings Visible="True" />
											
											<EditCellStyle BackColor="#99CCFF">
											</EditCellStyle>
											
											<EditItemTemplate>
											<iframe src="/filemanager.aspx?parent_page=member_files&id=<%# Eval("member_id") %>&show_folder=False&folder=Certificates" frameborder="no" width="100%" height="500px" scrolling="auto"></iframe>
											</EditItemTemplate>
											
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataTextColumn Caption="id" FieldName="id" 
											ShowInCustomizationForm="True" Visible="False" VisibleIndex="8">
										</dx:GridViewDataTextColumn>
										<dx:GridViewDataComboBoxColumn Caption="Branch" FieldName="business_unit_id" 
											ShowInCustomizationForm="True" VisibleIndex="1">
											<PropertiesComboBox DataSourceID="ds_companies" TextField="name" 
												ValueField="id" ValueType="System.Int32">
											</PropertiesComboBox>
											<EditFormSettings Visible="False" />
										</dx:GridViewDataComboBoxColumn>
										<dx:GridViewDataCheckColumn Caption="Is Certificate?" FieldName="is_certificate" 
											ReadOnly="True" ShowInCustomizationForm="True" VisibleIndex="9">
											<PropertiesCheckEdit AllowGrayedByClick="False" ValueChecked="True" 
												ValueType="System.Boolean" ValueUnchecked="False">
											</PropertiesCheckEdit>
											<EditFormSettings Visible="False" />
										</dx:GridViewDataCheckColumn>
										<dx:GridViewDataDateColumn Caption="Expires" FieldName="date_expires" ShowInCustomizationForm="True" VisibleIndex="6" Width="200px">
											<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Width="200px">
											</PropertiesDateEdit>
										</dx:GridViewDataDateColumn>
									</Columns>
									<SettingsBehavior ConfirmDelete="True" />

                                    <SettingsPopup EditForm-Height="800px" EditForm-HorizontalAlign="WindowCenter" 
										EditForm-Modal="True" EditForm-VerticalAlign="WindowCenter" 
										EditForm-Width="900px">
									    <EditForm Height="800px" HorizontalAlign="WindowCenter" Modal="True" VerticalAlign="WindowCenter" Width="900px" />
                                    </SettingsPopup>
									<SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />


                                    <SettingsPopup EditForm-Height="800px" EditForm-HorizontalAlign="WindowCenter" 
										EditForm-Modal="True" EditForm-VerticalAlign="WindowCenter" 
										EditForm-Width="900px"/>
									<SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" />


									<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True" 
										ShowHeaderFilterButton="True" />
									<SettingsText PopupEditFormCaption="Add / Edit History Of Training" />
									<Styles>

									</Styles>
                                    <StylesPopup EditForm-Content-Paddings-Padding="10px">
									    <EditForm>
                                            <Content>
                                                <Paddings Padding="10px" />
                                            </Content>
                                        </EditForm>
                                    </StylesPopup>
									<Templates>
									<EditForm>
									 <dx:ASPxGridViewTemplateReplacement ID="Editors" ReplacementType="EditFormEditors"
                                runat="server">
								 </dx:ASPxGridViewTemplateReplacement>
                           
                            <div style="margin-top: 10px; margin-bottom: 20px; padding-bottom:20px;">
                                <div style="float: left; margin-left: 5px">
                                    <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel" Width="100px"
                                        ClientSideEvents-Click='<%# "function(s, e) { " + Container.CancelAction + " }" %>' />
                                </div>
                                <div style="float: left; margin-left: 10px">
                                    <dx:ASPxButton ID="btnUpdate" runat="server" AutoPostBack="false" Text="Save" Width="100px"
                                        CssClass="input" ClientSideEvents-Click='<%# "function(s, e) { " + Container.UpdateAction + " }" %>' />
                                </div>
                            </div>
									
									</EditForm>
									</Templates>
								  </dx:ASPxGridView>
							  </dx:ContentControl>
						  </ContentCollection>
					  </dx:TabPage>
				  </TabPages>
			  </dx:ASPxPageControl>
		  </ContentTemplate>
	  </asp:UpdatePanel>
		<asp:SqlDataSource runat="server" 
			ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
			ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
			SelectCommand="
SELECT
	a.id,
	a.module_id,
	a.member_id,
	a.teacher_name,
	a.date,
	a.comments notes,
	a.business_unit_id,
	b.is_certificate,
	a.date_expires
FROM
	training_history a
inner JOIN 
	training_module b ON 
		a.module_id = b.id
" 
			ID="ds_history"></asp:SqlDataSource>
	<br />
	<br />
	<br />
	</div>
</asp:Content>

<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style2
		{
			height: 18px;
		}
	</style>
</asp:Content>



