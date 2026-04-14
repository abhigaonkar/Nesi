<%@ Page Title="Quote Recon Admin" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true" Inherits="sections_admin_quote_recon_admin_index" Codebehind="index.aspx.cs" %>	



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
		function bind_tooltips() {
			$(".opt1").each(function () {
				$(this).tip();
			});
		}
		$(document).ready(function () {
			//		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
			bind_tooltips();
		});

		function EndReqHandler() {
			bind_tooltips();

		}
	</script>

<table width = "100%">
<tr>
<td>
<dx:ASPxGridView ID="gvrecon" runat="server" ClientInstanceName="gvrecon" 
		Width="100%" AutoGenerateColumns="False" 
		Font-Names="Arial" KeyFieldName="id" 
		oncustomcallback="gvrecon_CustomCallback" 
		oncancelrowediting="gvrecon_CancelRowEditing" DataSourceID="SqlDataSource1">
	<Columns>
		<dx:GridViewDataTextColumn VisibleIndex="5" Caption="id" Visible="False" 
			FieldName="id">
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Question" FieldName="question" 
			VisibleIndex="0" Width="100%">
			<DataItemTemplate>
				<dx:ASPxMemo ID="ASPxMemo1" runat="server" BackColor="#FFFFCC" Height="25px" 
					oninit="ASPxMemo1_Init" Text='<%# Eval("question") %>' Width="100%" Font-Names="Arial">
					<Border BorderStyle="None" />
				</dx:ASPxMemo>
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="Type" FieldName="type" VisibleIndex="1" 
			Width="100px">
			<DataItemTemplate>
				<dx:ASPxComboBox ID="ASPxComboBox2" runat="server" oninit="ASPxComboBox2_Init" 
					Value='<%# Eval("type") %>' Width="90px" BackColor="#EEEEEE" Font-Names="Arial" 
					Height="25px">
					<Items>
						<dx:ListEditItem Text="Customer" Value="Customer" />
						<dx:ListEditItem Text="Market" Value="Market" />
						<dx:ListEditItem Text="Manpower" Value="Manpower" />
						<dx:ListEditItem Text="Finance" Value="Finance" />
					</Items>
					<Border BorderStyle="None" />
				</dx:ASPxComboBox>
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="ifyes" 
			VisibleIndex="2" Caption="If Yes" Width="60px">
			<DataItemTemplate>
				<dx:ASPxSpinEdit ID="ASPxSpinEdit2" runat="server" BackColor="#CCCCCC" 
					Height="21px" MaxValue="20" MinValue="-20" oninit="ASPxSpinEdit2_Init" Width="40px" 
					Increment="5" Value='<%# Eval("ifyes") %>'>
					<Border BorderStyle="None" />
				</dx:ASPxSpinEdit>
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn Caption="If No" FieldName="ifno" 
			VisibleIndex="3" Width="60px">
			<DataItemTemplate>
				<dx:ASPxSpinEdit ID="ASPxSpinEdit2" runat="server" BackColor="#CCCCCC" 
					Height="21px" MaxValue="20" MinValue="-20" oninit="ASPxSpinEdit2_Init1" Width="40px" 
					Increment="5" Value='<%# Eval("ifno") %>'>
					<Border BorderStyle="None" />
				</dx:ASPxSpinEdit>
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
		<dx:GridViewDataTextColumn FieldName="status" 
			VisibleIndex="4" Caption="Status" Width="100px">
			<DataItemTemplate>
				<dx:ASPxComboBox ID="ASPxComboBox3" runat="server" Width="100px" 
					BackColor="#EEEEEE" Font-Names="Arial" Text='<%# Eval("status") %>'>
					<Items>
						<dx:ListEditItem Text="Active" Value="Active" />
						<dx:ListEditItem Text="Inactive" Value="Inactive" />
					</Items>
					<Border BorderStyle="None" />
				</dx:ASPxComboBox>
			</DataItemTemplate>
		</dx:GridViewDataTextColumn>
	</Columns>
	<SettingsBehavior ColumnResizeMode="Control" />

<SettingsBehavior ColumnResizeMode="Control"></SettingsBehavior>

	<SettingsPager Mode="ShowAllRecords">
	</SettingsPager>
	
	<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" 
		ShowFilterRowMenu="True" ShowFooter="True" />

<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowFilterRowMenu="True" 
		ShowFooter="True" ShowFilterBar="Visible"></Settings>

	<SettingsText PopupEditFormCaption="  Enter New Responsibility" />
	<SettingsPopup>
		<EditForm HorizontalAlign="WindowCenter" Modal="True" 
			VerticalAlign="WindowCenter" Width="950px" Height="720px" />
	</SettingsPopup>
	<Styles>
		<Header BackColor="#CCFFCC" Font-Bold="True">
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
	s.SetText(gvrecon.GetVisibleRowsOnPage());
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
								<td width="100%">
									<dx:ASPxMemo ID="memnew" runat="server" Height="21px" Width="100%" 
										BackColor="#FFFFCC" ClientInstanceName="memnew" Font-Names="Arial" 
										NullText="Enter Question">
									</dx:ASPxMemo>
								</td>
								
								<td valign="middle">
									<dx:ASPxComboBox ID="ddltype" runat="server" Height="26px" 
										ClientInstanceName="ddltype" Font-Names="Arial" SelectedIndex="0">
										<Items>
											<dx:ListEditItem Text="Customer" Value="Customer" Selected="True" />
											<dx:ListEditItem Text="Market" Value="Market" />
											<dx:ListEditItem Text="Manpower" Value="Manpower" />
											<dx:ListEditItem Text="Finance" Value="Finance" />
										</Items>
									</dx:ASPxComboBox>
								</td>
								
								<td valign="middle">
									<table style="width:100%;">
										<tr>
											<td nowrap="nowrap">
												Value if Yes</td>
											<td>
												<dx:ASPxSpinEdit ID="spnyes" runat="server" Height="25px" Increment="5" 
													MaxValue="20" MinValue="-20" Number="0" NumberType="Integer" Width="50px" ClientInstanceName="spnyes" 
													Font-Names="Arial" />
											</td>
											<td nowrap="nowrap">
												Value if No</td>
											<td>
												<dx:ASPxSpinEdit ID="spnno" runat="server" Height="25px" Increment="5" 
													MaxValue="20" MinValue="-20" Number="0" NumberType="Integer" Width="50px" ClientInstanceName="spnno" 
													Font-Names="Arial" />
											</td>
										</tr>
									</table>
								</td>
								
								<td align="right" valign="middle">
									<dx:ASPxButton ID="btnAddType" runat="server" AutoPostBack="False" TabIndex="1" 
										Text="Add Question" UseSubmitBehavior="False" Wrap="False" Font-Names="Arial">
										<ClientSideEvents Click="function(s, e) {
	if ((memnew.GetText()!='')&amp;&amp;((spnyes.GetValue()!=0)||(spnno.GetValue()!=0)))
{
	gvrecon.PerformCallback('a|' + memnew.GetText() + '|' + ddltype.GetText() + '|' + spnyes.GetValue() + '|' + spnno.GetValue());

}
}" />
									</dx:ASPxButton>
								</td>
							</tr>
						</table>
                    
                    </TitlePanel>
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
	
	<br />
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT
*
FROM
quote_process_questions"></asp:SqlDataSource>
	
</td>
</tr>
</table>



</asp:Content>
<asp:Content ID="Content5" runat="server" 
	contentplaceholderid="header_placeholder">
	</asp:Content>