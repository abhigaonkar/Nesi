<%@ Page Title="Core Responsibilities" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_member_core_responsibilities" Theme="NETheme01" EnableTheming="True" Codebehind="core_responsibilities.aspx.cs" %>	



<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register src="../../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>

<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>


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

	<script type="text/javascript">
 function bind_tooltips()
			{
			$(".opt1").each(function()
				{
				$(this).tip();
				});
			}
		$(document).ready(function()
			{
	//		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
			});

function EndReqHandler()
	{
	bind_tooltips();
	
	}
	</script>

<table width = "100%">
<tr>
<td>
	<lc:LayoutControl ID="LayoutControl1" runat="server" GridviewID="gvcore" />
<dx:ASPxGridView ID="gvcore" runat="server" ClientInstanceName="gvcore" 
		Width="100%" AutoGenerateColumns="False" 
		Font-Names="Arial" KeyFieldName="id" 
		onrowupdating="gvtypes_RowUpdating" onrowinserting="gvtypes_RowInserting" 
		onhtmleditformcreated="gvcore_HtmlEditFormCreated" 
		onhtmldatacellprepared="gvcore_HtmlDataCellPrepared" 
		onhtmlrowprepared="gvcore_HtmlRowPrepared" 
		oncustomcallback="gvcore_CustomCallback" 
		onstartrowediting="gvcore_StartRowEditing" 
		oncancelrowediting="gvcore_CancelRowEditing" onrowdeleting="gvcore_RowDeleting" 
		Theme="NETheme01" oncustomjsproperties="gvcore_CustomJSProperties">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
	<Columns>
		<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" 
			Width="50px">
			
			
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="id" 
			ReadOnly="True" VisibleIndex="1" Width="30px">
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Responsibility" FieldName="core_responsibility" 
			VisibleIndex="3" Width="100%">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataComboBoxColumn Caption="Modified By" FieldName="member_id" 
			VisibleIndex="4" Width="75px">
			<PropertiesComboBox DataSourceID="sqlmember" TextField="name" 
				ValueField="member_id" ValueType="System.Int32">
			</PropertiesComboBox>
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataComboBoxColumn>
		<dx:GridViewDataDateColumn Caption="Date" FieldName="date_added" 
			VisibleIndex="5" Width="100px">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
				EditFormatString="yyyy-MM-dd">
			</PropertiesDateEdit>
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataDateColumn Caption="Last Modified" 
			FieldName="last_modified_date" VisibleIndex="6" Width="100px">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" 
				EditFormatString="yyyy-MM-dd">
			</PropertiesDateEdit>
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="7" 
			Width="50px">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="description" Visible="False" 
			VisibleIndex="10">
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataComboBoxColumn Caption="Group" FieldName="cr_group_id" 
			VisibleIndex="2" Width="70px">
			<PropertiesComboBox DataSourceID="SqlDataSource1" TextField="name" 
				ValueField="id" ValueType="System.Int32">
			</PropertiesComboBox>
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataComboBoxColumn>
		<dx:GridViewDataTextColumn Caption="Active in Business Unit" FieldName="a" 
			VisibleIndex="8" Width="110px">
			<EditFormSettings Visible="False" />
			<CellStyle Font-Bold="True" Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="cr_group_id" Visible="False" 
			VisibleIndex="9">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control" />
	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
	<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" 
		ShowFilterRowMenu="True" ShowFooter="True" ShowHeaderFilterButton="True" />
	<SettingsText PopupEditFormCaption="  Enter New Responsibility" />
	<SettingsPopup>
		<EditForm HorizontalAlign="WindowCenter" Modal="True" 
			VerticalAlign="WindowCenter" Width="950px" Height="720px" />
	</SettingsPopup>
	<Styles>
		<Header Font-Bold="True">
		</Header>
	</Styles>
	 <StylesPopup>
		 <EditForm>
			 <Content BackColor="White">
				 <Border BorderColor="White" BorderStyle="Solid" BorderWidth="10px" />
				
		
			 </Content>
		
		 </EditForm>
	</StylesPopup>
	 <Templates>
                    <FooterRow>
						<table style="width:100%;">
							<tr>
								<td>
									<dx:ASPxLabel ID="lblcount" runat="server" ClientInstanceName="lblcount">
										<ClientSideEvents Init="function(s, e) {
	s.SetText(gvcore.GetVisibleRowsOnPage());
}" />
									</dx:ASPxLabel>
								</td>
								<td>
									&nbsp;</td>
								<td>
									&nbsp;</td>
							</tr>
						</table>
					</FooterRow>
                    <TitlePanel>
                      
                     <table style="width:100%;">
							<tr>
								<td>
									&nbsp;</td>
								
								<td width="100%" align="right" valign="middle">
									Search for Keyword -&gt;</td>
								
								<td align="right" valign="middle">
									<dx:ASPxButtonEdit ID="ASPxButtonEdit1" runat="server" 
										ClientInstanceName="ASPxButtonEdit1" Theme="NETheme01">
										<ClientSideEvents ButtonClick="function(s, e) {
	gvcore.PerformCallback('x|' + s.GetText());
}" KeyDown="function(s, e) {
	      
          if (e.htmlEvent.keyCode == 13)
              s.GetButton(0).click();
    
}" />
										<Buttons>
											<dx:EditButton>
												<Image Url="~/images/icon/icon[search].gif">
												</Image>
											</dx:EditButton>
										</Buttons>
									</dx:ASPxButtonEdit>
								</td>
							</tr>
							<tr>
								<td>
									<dx:ASPxButton ID="btnAddType" runat="server" AutoPostBack="False" 
										Text="Add Responsibility" Wrap="False" TabIndex="1" UseSubmitBehavior="False">
										<ClientSideEvents Click="function(s, e) {
	gvcore.AddNewRow();
}" />
									</dx:ASPxButton>
								</td>
								<td align="right" valign="middle" width="100%">
									Show Active for this Business Unit -&gt;</td>
								<td align="right" valign="middle">
									<asp:SqlDataSource ID="SqlDataSource3" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										
										SelectCommand="Select 999 id, 'All Branches' name UNION Select id,ddl_name name from business_unit  where active='T' and find_in_set(id, @visibleBU)">
									    <SelectParameters>
									        <asp:SessionParameter Name="@visibleBU" SessionField="visibleBU" Type="String" />
									    </SelectParameters>
									</asp:SqlDataSource>
									<dx:ASPxComboBox ID="ddlbranch" runat="server" ClientInstanceName="ddlbranch" 
										DataSourceID="SqlDataSource3" oninit="ddlbranch_Init" TextField="name" 
										ValueField="id" ValueType="System.Int32" Theme="NETheme01">
										<ClientSideEvents SelectedIndexChanged="function(s, e) {
hdnCompany['value']= s.GetValue();
gvcore.PerformCallback('filter|' + s.GetValue());


}" />
									</dx:ASPxComboBox>
								</td>
							</tr>
						</table>
                    
                    </TitlePanel>
					<EditForm>
							 <iframe id="iframe_cr" runat="server" height="670" width="100%" frameborder="0"></iframe>
					
							 <br />
							 <table style="width:100%;">
								 <tr>
									 <td>
										 <dx:ASPxButton ID="btnprev" runat="server" AutoPostBack="False" Text="Prev">
											 <ClientSideEvents Click="function(s, e) {
	gvcore.PerformCallback('prev');

}" />
										 </dx:ASPxButton>
									 </td>
									 <td class="dxtcRightAlignCell_Office2003Blue">
										 &nbsp;</td>
									 <td align="right">
										 <dx:ASPxButton ID="btnnext" runat="server" AutoPostBack="False" Text="Next">
											 <ClientSideEvents Click="function(s, e) {
	gvcore.PerformCallback('next');

}" />
										 </dx:ASPxButton>
									 </td>
								 </tr>
							 </table>
					
					</EditForm>
                </Templates>
	</dx:ASPxGridView>
		<dx:ASPxCallbackPanel ID="cb_chkprivate" runat="server" 
				ClientInstanceName="cb_chkprivate" oncallback="cb_chkprivate_Callback" 
				Width="200px">

				<LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
				</LoadingPanelStyle>
				<PanelCollection>
					<dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
					</dx:PanelContent>
				</PanelCollection>
			</dx:ASPxCallbackPanel>
				<dx:ASPxHiddenField ID="hdnCompany" runat="server" 
		ClientInstanceName="hdnCompany">
	</dx:ASPxHiddenField>

			<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
										ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
										ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
										SelectCommand="Select * from cr_group">
	</asp:SqlDataSource>

			<asp:SqlDataSource ID="SqlDataSource6" runat="server" 
													ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
													ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
													
										
										
		SelectCommand="SELECT id, priority_name FROM cr_skills_priority order by id">
	</asp:SqlDataSource>

			<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from cr_group"></asp:SqlDataSource>

			<asp:SqlDataSource ID="sqlmember" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select member_id,get_name(member_id) name from member">
	</asp:SqlDataSource>

</td>
</tr>
</table>



</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		
	</style>
	</asp:Content>

