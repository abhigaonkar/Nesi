<%@ Page Title="Cap Training Admin" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_member_training_header" Codebehind="training_header.aspx.cs" %>	



<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

<%@ Register src="../../../modules/layout_control.ascx" tagname="LayoutControl" tagprefix="lc" %>
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
	<lc:LayoutControl ID="LayoutControl1" runat="server" GridviewID="gvtraining" />
<dx:ASPxGridView ID="gvtraining" runat="server" ClientInstanceName="gvtraining" 
		Width="100%" AutoGenerateColumns="False" 
		Font-Names="Arial" KeyFieldName="id" 
		onrowupdating="gvtraining_RowUpdating" onrowinserting="gvtraining_RowInserting" 
		onhtmleditformcreated="gvtraining_HtmlEditFormCreated" 
		onhtmlrowprepared="gvtraining_HtmlRowPrepared" 
		oncustomcallback="gvtraining_CustomCallback" 
		onstartrowediting="gvtraining_StartRowEditing" 
		oncancelrowediting="gvtraining_CancelRowEditing" 
		onrowdeleting="gvtraining_RowDeleting" Theme="NETheme01" 
		oncustomjsproperties="gvtraining_CustomJSProperties">
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
	<Columns>
		<dx:GridViewDataCheckColumn Caption="Is Internal" FieldName="is_internal" 
			VisibleIndex="4" Width="60px">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataCheckColumn>
		<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0"  ShowEditButton="true" ShowDeleteButton="true" 
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
		<dx:GridViewDataTextColumn Caption="URL" FieldName="url" 
			VisibleIndex="3" Width="100px">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Name" FieldName="name" VisibleIndex="2" 
			Width="250px">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataMemoColumn Caption="Notes" FieldName="notes" VisibleIndex="5" 
			Width="100%">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataMemoColumn>
		<dx:GridViewDataTextColumn Caption="Contact Info" FieldName="contact_info" 
			VisibleIndex="6" Width="100px">
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Phone" FieldName="phone_number" 
			VisibleIndex="7" Width="80px">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Cost" FieldName="cost" VisibleIndex="8" 
			Width="75px">
			<PropertiesTextEdit DisplayFormatString="c2">
			</PropertiesTextEdit>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataDateColumn Caption="Last Modified" FieldName="last_modified" 
			VisibleIndex="9" Width="75px">
			<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd">
			</PropertiesDateEdit>
		</dx:GridViewDataDateColumn>
		<dx:GridViewDataSpinEditColumn Caption="Quality" FieldName="quality" 
			VisibleIndex="10" Width="75px">
			<PropertiesSpinEdit DisplayFormatString="g" MaxValue="10" 
				ShowOutOfRangeWarning="False">
				<ValidationSettings Display="None">
				</ValidationSettings>
			</PropertiesSpinEdit>
		</dx:GridViewDataSpinEditColumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
	<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" 
		ShowFilterRowMenu="True" ShowFooter="True" />
	<SettingsText PopupEditFormCaption="Add / Edit Training Module " />
	<SettingsPopup>
		<EditForm HorizontalAlign="WindowCenter" 
			VerticalAlign="Above" Width="950px" Height="750px" />
	</SettingsPopup>
	<Styles>
		<Header Font-Bold="True" ForeColor="White">
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
	s.SetText(gvtraining.GetVisibleRowsOnPage());
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
									<dx:ASPxButton ID="btnAddType" runat="server" AutoPostBack="False" 
										Text="Add Training Module" Wrap="False">
										<ClientSideEvents Click="function(s, e) {
	gvtraining.AddNewRow();
}" />
									</dx:ASPxButton>
								</td>
								
								<td width="100%" class="style5" valign="middle">
									Search for Keyword -&gt;</td>
								
								<td align="right" valign="middle">
									<dx:ASPxButtonEdit ID="ASPxButtonEdit1" runat="server">
										<ClientSideEvents ButtonClick="function(s, e) {
	gvtraining.PerformCallback('x|' + s.GetText());
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
						</table>
                    
                    </TitlePanel>
					<EditForm>
							 <iframe id="iframe_cr" runat="server" height="700" width="100%" frameborder="0"></iframe>
					
							 <br />
							 <table style="width:100%;">
								 <tr>
									 <td>
										 <dx:ASPxButton ID="btnprev" runat="server" AutoPostBack="False" Text="Prev">
											 <ClientSideEvents Click="function(s, e) {
	gvtraining.PerformCallback('prev');

}" />
										 </dx:ASPxButton>
									 </td>
									 <td></td>
									 <td align="right">
										 <dx:ASPxButton ID="btnnext" runat="server" AutoPostBack="False" Text="Next">
											 <ClientSideEvents Click="function(s, e) {
	gvtraining.PerformCallback('next');

}" />
										 </dx:ASPxButton>
									 </td>
								 </tr>
							 </table>
					
					</EditForm>
                </Templates>
	</dx:ASPxGridView>
		
	
</td>
</tr>
</table>



</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	<style type="text/css">
		.style5
		{
			text-align: right;
		}
	</style>
	</asp:Content>

