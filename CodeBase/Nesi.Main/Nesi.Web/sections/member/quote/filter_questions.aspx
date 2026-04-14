<%@ Page Title="Quote Screening Admin" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_member_quote_filter_questions" Codebehind="filter_questions.aspx.cs" %>	



<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>



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
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" 
		SelectCommand="Select * from quote_filter_questions"></asp:SqlDataSource>
<dx:ASPxGridView ID="gv" runat="server" ClientInstanceName="gv" 
		Width="100%" AutoGenerateColumns="False" 
		Font-Names="Arial" KeyFieldName="id" 
		oncustomcallback="gv_CustomCallback" DataSourceID="SqlDataSource1" 
		onrowdeleting="gv_RowDeleting">
	
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																	<CancelButton Image-Url="~/images/icon/icon[cancel].gif" Image-Width="16px" Text="Cancel" />
																</SettingsCommandButton>
	<Columns>
		<dx:GridViewDataCheckColumn Caption="Kills Quote" FieldName="kills_quote" 
			VisibleIndex="3" Width="75px">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataCheckColumn>
		<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" ShowDeleteButton="true"
			Width="30px">
			
			
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewCommandColumn>
		<dx:GridViewDataTextColumn FieldName="id" 
			ReadOnly="True" VisibleIndex="1" Width="30px" Visible="False">
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Weight" FieldName="max_score" 
			VisibleIndex="4" Width="100px">
			<EditFormSettings Visible="False" />
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Question" FieldName="filter_question" VisibleIndex="2" 
			Width="100%">
			<Settings AutoFilterCondition="Contains" />
			<EditFormSettings Visible="False" />
			<CellStyle Wrap="False">
			</CellStyle>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="6" 
			Width="120px">
			<DataItemTemplate>
				<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" Text='<%# Eval("status") %>' 
					Width="100px" oninit="ASPxComboBox2_Init">
					<Items>
						<dx:ListEditItem Text="Active" Value="Active" />
						<dx:ListEditItem Text="InActive" Value="InActive" />
					</Items>
				</dx:ASPxComboBox>
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control" />
	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
	<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" 
		ShowFilterRowMenu="True" ShowFooter="True" />
	<SettingsText PopupEditFormCaption="Add / Edit Filter Question " />
	<SettingsPopup>
		<EditForm HorizontalAlign="WindowCenter" Modal="True" 
			VerticalAlign="WindowCenter" Width="950px" Height="720px" />
	</SettingsPopup>
	<Styles>
		<Header BackColor="Blue" Font-Bold="True" ForeColor="White">
		</Header>
	</Styles>
	
	 <Templates>
                   
                    <TitlePanel>
                      
                     <table style="width:100%;">
							<tr>
								<td>
									&nbsp;</td>
								
								<td width="100%" class="style5" valign="middle">
									<dx:ASPxTextBox ID="tb" runat="server" ClientInstanceName="tb" 
										NullText="Enter Question" Width="100%" Font-Names="Arial">
									</dx:ASPxTextBox>
								</td>
								
								<td nowrap="nowrap">
									&nbsp;</td>
								<td nowrap="nowrap">
									<dx:ASPxCheckBox ID="kq" runat="server" ClientInstanceName="kq" 
										Text="Kills Quote" TextAlign="Left" Width="80px" Wrap="False" Font-Names="Arial">
									</dx:ASPxCheckBox>
								</td>
								<td nowrap="nowrap">
									Weight:</td>
								<td>
									<dx:ASPxSpinEdit ID="w" runat="server" ClientInstanceName="w" Height="21px" 
										MaxValue="5" MinValue="1" Number="1" Width="50px" Font-Names="Arial" />
								</td>
								
								<td align="right" valign="middle">
									<dx:ASPxButton ID="btnadd" runat="server" AutoPostBack="False" 
										ClientInstanceName="btnadd" Text="Add To Quote Screen" Wrap="False" Font-Names="Arial">
										<ClientSideEvents Click="function(s, e) {
	gv.PerformCallback('a');
}" />
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
                    
                    </TitlePanel>
					
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

