<%@ Page Title="Cap Certificates Admin" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_member_certificates" EnableTheming="True" Theme="NETheme01" Codebehind="certificates.aspx.cs" %>	



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
	<lc:LayoutControl ID="LayoutControl1" runat="server" GridviewID="gvcert" />
<dx:ASPxGridView ID="gvcert" runat="server" ClientInstanceName="gvcert" 
		Width="100%" AutoGenerateColumns="False" 
		Font-Names="Arial" KeyFieldName="id" 
		onrowupdating="gvcert_RowUpdating" onrowinserting="gvcert_RowInserting" 
		onhtmleditformcreated="gvcert_HtmlEditFormCreated" 
		onhtmlrowprepared="gvcert_HtmlRowPrepared" 
		oncustomcallback="gvcert_CustomCallback" 
		onstartrowediting="gvcert_StartRowEditing" 
		oncancelrowediting="gvcert_CancelRowEditing" Theme="NETheme01" 
		oncustomjsproperties="gvcert_CustomJSProperties">
	<SettingsCommandButton>
		<EditButton Image-Url="~/images/icon/icon[edit].gif" />
	</SettingsCommandButton>
	<Columns>
		<dx:GridViewDataCheckColumn Caption="Is Internal" FieldName="is_internal" 
			VisibleIndex="4" Width="75px">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataCheckColumn>
		<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" ShowEditButton="true" 
			Width="30px">
			
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="id" 
			ReadOnly="True" VisibleIndex="1" Width="30px">
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Expiry Time (days)" FieldName="expires" 
			VisibleIndex="3" Width="100px">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Certificate" FieldName="certificate_name" VisibleIndex="2" 
			Width="100%">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataMemoColumn Caption="Notes" FieldName="notes" VisibleIndex="5" 
			Width="500px">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataMemoColumn>
		<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="6" 
			Width="100px">
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control" EnableRowHotTrack="True" />
	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
	<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" 
		ShowFilterRowMenu="True" ShowFooter="True" ColumnMinWidth="10" />
	<SettingsText PopupEditFormCaption="Add / Edit Certificate " />
	<SettingsPopup>
		<EditForm HorizontalAlign="WindowCenter" 
			VerticalAlign="WindowCenter" Width="950px" Height="720px" />
	</SettingsPopup>
	<Styles>
		
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
	s.SetText(gvcert.GetVisibleRowsOnPage());
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
										Text="Add Certificate" Wrap="False" UseSubmitBehavior="False" ClientEnabled="False" 
										ClientInstanceName="btnAddType" oninit="btnAddType_Init">
										<ClientSideEvents Click="function(s, e) {
	gvcert.AddNewRow();
}" />
									</dx:ASPxButton>
								</td>
								
								<td width="100%" align="right" valign="middle">
									Search for Keyword -&gt;</td>
								
								<td align="right" valign="middle">
									<dx:ASPxButtonEdit ID="ASPxButtonEdit1" runat="server">
										<ClientSideEvents ButtonClick="function(s, e) {
	gvcert.PerformCallback('x|' + s.GetText());
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
	gvcert.PerformCallback('prev');

}" />
										 </dx:ASPxButton>
									 </td>
									 <td class="dxtcRightAlignCell_Office2003Blue">
										 &nbsp;</td>
									 <td align="right">
										 <dx:ASPxButton ID="btnnext" runat="server" AutoPostBack="False" Text="Next">
											 <ClientSideEvents Click="function(s, e) {
	gvcert.PerformCallback('next');

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
		
	</style>
	</asp:Content>

