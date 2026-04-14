<%@ Page Title="Skills / Responsibilities Link" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_hr_member_skills_responsibility_link" Codebehind="skills_responsibility_link.aspx.cs" %>	



<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>


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
<lc:LayoutControl runat="server" id="layout" __is_private="True" 
		GridviewID="gvcore" ShowExcelExport="True" ShowToggle="True" /></td></tr><td>
<dx:ASPxGridView ID="gvcore" runat="server" ClientInstanceName="gvcore" 
		Width="100%" AutoGenerateColumns="False" 
		Font-Names="Arial" KeyFieldName="id" 
		oncustomcallback="gvcore_CustomCallback" 
			oncustomjsproperties="gvcore_CustomJSProperties">
	<Columns>
		<dx:GridViewDataTextColumn Caption="Responsibility" FieldName="cr" 
			VisibleIndex="1" Width="200px">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="CR Status" FieldName="status" VisibleIndex="2" 
			Width="50px">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="description" 
			VisibleIndex="4" Caption="CR Description" Width="200px">
			<Settings AutoFilterCondition="Contains" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="CR Group" FieldName="gn" 
			VisibleIndex="0" Width="70px">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="name" 
			VisibleIndex="6" Caption="Skill" Width="200px">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Skill Status" FieldName="skill_status" 
			VisibleIndex="8" Width="60px">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Skill Description" FieldName="sd" 
			VisibleIndex="12" Width="200px">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Skill Type" FieldName="st" 
			VisibleIndex="14" Width="100px">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Can Be Trained" FieldName="cbt" 
			VisibleIndex="16" Width="60px">
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control" />
	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
	<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" 
		ShowFilterRowMenu="True" ShowFooter="True" ShowGroupPanel="True" />
	<SettingsText PopupEditFormCaption="  Enter New Responsibility" />
	<SettingsPopup>
		<EditForm HorizontalAlign="WindowCenter" Modal="True" 
			VerticalAlign="WindowCenter" Width="950px" Height="720px" />
	</SettingsPopup>
	<Styles>
		<Header BackColor="#FFFF66" Font-Bold="True">
		</Header>
	</Styles>
	 <StylesPopup>
		 <EditForm>
			 <Content BackColor="White">
				 <Border BorderColor="White" BorderStyle="Solid" BorderWidth="10px" />
				
		
			 </Content>
			 <Header BackColor="#FFFF66">
			 </Header>
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
								
								<td width="100%" class="style5" valign="middle">
									Search for Keyword -&gt;</td>
								
								<td align="right" valign="middle">
									<dx:ASPxButtonEdit ID="ASPxButtonEdit1" runat="server">
										<ClientSideEvents ButtonClick="function(s, e) {
	gvcore.PerformCallback('x|' + s.GetText());
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
					
                </Templates>
	</dx:ASPxGridView>

	<asp:HiddenField ID="hdncompany" runat="server" />
	
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

