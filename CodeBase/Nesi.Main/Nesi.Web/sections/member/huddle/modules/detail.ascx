<%@ Control Language="C#" AutoEventWireup="true" Inherits="sections_member_huddle_modules_detail" EnableTheming="True" Codebehind="detail.ascx.cs" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
    <%@ Register src="detail_tasks.ascx" tagname="detail_tasks" tagprefix="uc" %>
    <style type="text/css">
    	td { vertical-align: top; }
    	.draggingStyle { background-color: lightblue; }
    	.targetGrid { background-color: lightcoral; }
    </style>
<asp:HiddenField id="hid_id" runat="server" />
<div style="margin-left:10px">
<dx:ASPxPageControl id="pc" runat="server" ActiveTabIndex="1" 
		ClientInstanceName="pc" Width="98%">
	<TabPages>
		<dx:TabPage Text="Overview">
			<ContentCollection>
				<dx:ContentControl runat="server">
					<dx:ASPxCallbackPanel id="cbp_detail_overview" runat="server" ClientInstanceName="cbp_detail_overview" OnCallback="cbp_detail_overview_Callback">
						<PanelCollection>
							<dx:PanelContent>
								<div style="width:95%">
									<div style="margin-bottom:5px;">
									<dx:ASPxTextBox ID="dt_created" runat="server" Caption="Created Date:" Width="300px" 
											ClientInstanceName="ex_name" Theme="NETheme01" Enabled="False">
											<CaptionCellStyle Width="100px">
											</CaptionCellStyle>
											
										</dx:ASPxTextBox>
										
									</div>
									<div style="margin-bottom:5px;">
									<dx:ASPxDateEdit ID="date_of_huddle" Caption="Date of Huddle" Width="200px" 
				ClientInstanceName="new_date_of_huddle" runat="server" Theme="NETheme01" 
				DisplayFormatString="yyyy-MM-dd HH:mm:ss" EditFormat="Custom" EditFormatString="yyyy-MM-dd HH:mm:ss" 
				>
										<TimeSectionProperties Visible="True">
										</TimeSectionProperties>
				<CaptionCellStyle Width="100px">
				</CaptionCellStyle>
			</dx:ASPxDateEdit>
			</div>
									<div style="margin-bottom:5px;">
										<dx:ASPxTextBox ID="name" runat="server" Caption="Name:" Width="300px" 
											ClientInstanceName="ex_name" Theme="NETheme01">
											<CaptionCellStyle Width="100px">
											</CaptionCellStyle>
											
										</dx:ASPxTextBox>
										</div>
									<div style="margin-bottom:5px;">
										<dx:ASPxMemo ID="description" runat="server" Caption="Description" 
											Height="100px" Width="300px" ClientInstanceName="ex_description" 
											Theme="NETheme01">
											<CaptionCellStyle Width="100px">
											</CaptionCellStyle>
											
										</dx:ASPxMemo>
									</div>
									<div style="margin-bottom:5px;">
										<dx:ASPxComboBox ID="captain" runat="server" Caption="Captain:" Width="300px" 
											ValueField="id" TextField="name" ValueType="System.Int32" 
											ClientInstanceName="ex_captain" Theme="NETheme01">
											<CaptionCellStyle Width="100px">
											</CaptionCellStyle>
											
										</dx:ASPxComboBox>
									</div>
									<div style="margin-bottom:5px;">
										<dx:ASPxCheckbox ID="active" runat="server" Width="130px" 
											ClientInstanceName="ex_active"  Text="Active:" TextAlign="Left" 
											Theme="NETheme01">
										</dx:ASPxCheckbox>
										</div>
								</div>
								<div style="width:400px;height:50px;padding:5px;">
									<div style="float:left;">
										<dx:ASPxButton runat="server" ID="cancel_huddle" AutoPostBack="false" 
											Text="Cancel" Font-Size="13px" Height="25px" Font-Bold="False" Theme="NETheme01">
											<Image Url="/images/icon/icon[left].gif" Height="16px" Width="16px"></Image>
											<ClientSideEvents Click="huddle.existing_huddle.overview.cancel" />
										</dx:ASPxButton>
									</div>
									<div style="float:right;">
										<dx:ASPxButton runat="server" ID="save_huddle" AutoPostBack="false" Text="Save" 
											Font-Size="13px" Height="25px" Font-Bold="False" Theme="NETheme01">
											<Image Url="/images/icon/icon[save].gif" Height="16px" Width="16px"></Image>
											<ClientSideEvents Click="huddle.existing_huddle.overview.save" />
										</dx:ASPxButton>
									</div>
								</div>
								<dx:ASPxLabel id="lb_result" runat="server" EncodeHtml="false"></dx:ASPxLabel>
							</dx:PanelContent>
						</PanelCollection>
						<ClientSideEvents EndCallback="huddle.existing_huddle.overview.end_callback" />
					</dx:ASPxCallbackPanel>
				</dx:ContentControl>
			</ContentCollection>
		</dx:TabPage>
		<dx:TabPage Text="Stuff to Talk About">
			<ContentCollection>
				<dx:ContentControl runat="server">
					<uc:detail_tasks ID="uc_detail_tasks" runat="server" />
				</dx:ContentControl>
			</ContentCollection>
		</dx:TabPage>
		<dx:TabPage Text="People in the Huddle">
			<ContentCollection>
				<dx:ContentControl runat="server">
					<dx:ASPxCallbackPanel ID="cbp_people" runat="server" ClientInstanceName="cbp_people" OnCallback="cbp_people_Callback">
						<PanelCollection>
							<dx:PanelContent>
								<div id="column_left" 
									style="height:650px;margin-top:10px;float:left;border:0px solid #E13; width:49%">
									<div style="background-color:#EAEAEA; color:#808080; padding:3px;font-weight:normal; font-size:14px; font-family: 'Segoe UI';">Available Users</div>
									<div id="users_from" style="height:95%;width:100%;overflow-y:scroll;">
										<dx:ASPxGridView id="gv_users_from" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_users_from" KeyFieldName="id" Width="100%">
											<Columns>
												<dx:GridViewDataTextColumn FieldName="name" ShowInCustomizationForm="True" VisibleIndex="2">
													<Settings AutoFilterCondition="Contains" />
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsBehavior EnableRowHotTrack="True" />
											<SettingsPager PageSize="100">
											</SettingsPager>
											<Settings ShowColumnHeaders="False" ShowFilterRow="True" />
											<Styles>
												<Row CssClass="draggableRow left" Cursor="pointer">
												</Row>
												<Table CssClass="droppableLeft">
												</Table>
											</Styles>
											<Border BorderWidth="0px" />
										</dx:ASPxGridView>
									</div>
								</div>
								<div id="column_right" style="height:650px; margin-top:10px; float:right;border:solid 0px #047; width:49%">
									<div style="background-color:#006699; color:#fff;padding:3px;font-weight:normal; font-size:14px; font-family: 'Segoe UI';">Selected Users</div>
									<div id="users_to" style="height:95%;width:100%;overflow-y:scroll;">
										<dx:ASPxGridView id="gv_users_to" runat="server" AutoGenerateColumns="False" ClientInstanceName="gv_users_to" KeyFieldName="id" Width="100%">
											<Columns>
												<dx:GridViewDataTextColumn FieldName="name" ShowInCustomizationForm="True" VisibleIndex="0" Width="100%">
													<Settings AutoFilterCondition="Contains" />
												</dx:GridViewDataTextColumn>
												<dx:GridViewDataTextColumn Caption="Color" ShowInCustomizationForm="True" VisibleIndex="1" Width="35px">
													<DataItemTemplate>
														<input type='color' class='colorpicker' onchange='huddle.existing_huddle.people.color(this);' data-member='<%# Eval("id") %>' style='width:25px; height: 15px;' value='<%# Eval("color") %>' />
													</DataItemTemplate>
												</dx:GridViewDataTextColumn>
											</Columns>
											<SettingsBehavior EnableRowHotTrack="True" />
											<SettingsPager PageSize="100">
											</SettingsPager>
											<Settings ShowColumnHeaders="False" ShowFilterRow="True" />
											<SettingsText EmptyDataRow="Drag some users to me" />
											<Styles>
												<Row CssClass="draggableRow right" Cursor="pointer">
												</Row>
												<Table CssClass="droppableRight">
												</Table>
												<EmptyDataRow CssClass="emptyrow">
												</EmptyDataRow>
											</Styles>
											<Border BorderWidth="0px" />
										</dx:ASPxGridView>
									</div>
								</div>
							
							</dx:PanelContent>
						</PanelCollection>
						<ClientSideEvents BeginCallback="huddle.existing_huddle.people.start" EndCallback="huddle.existing_huddle.people.stop" CallbackError="huddle.existing_huddle.people.stop" />
					</dx:ASPxCallbackPanel>
				</dx:ContentControl>
			</ContentCollection>
		</dx:TabPage>
	</TabPages>
	<ClientSideEvents ActiveTabChanged="huddle.existing_huddle.handle_tab_change" />
</dx:ASPxPageControl>
</div>
    <dx:ASPxGlobalEvents ID="ge" runat="server">
        <ClientSideEvents ControlsInitialized="OnControlsInitialized" EndCallback="OnControlsInitialized" />
    </dx:ASPxGlobalEvents>
