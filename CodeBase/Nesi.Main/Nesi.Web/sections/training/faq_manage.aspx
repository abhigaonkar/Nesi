<%@ Page Language="C#" AutoEventWireup="true" Inherits="sections_training_faq_manage" MasterPageFile="~/IntraDefault.master" Codebehind="faq_manage.aspx.cs" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<asp:Content ID="hdrcontent" ContentPlaceHolderID="header_placeholder" runat="server">
	<script type="text/javascript" src="/js/faq_manage.js"></script>
</asp:Content>
<asp:CONTENT ID="Content1" ContentPlaceHolderID="cphMasterMenu" Runat="Server">
<div id="divMenu" runat="server">
</div>
</asp:CONTENT>
<ASP:CONTENT ID="Content2" ContentPlaceHolderID="cphMasterLeft" Runat="Server">
    <div id="divSide" runat="server">
    </div>
</ASP:CONTENT>
<ASP:CONTENT ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
</ASP:CONTENT>
<ASP:CONTENT ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<div>
		<table width='100%'>
			<tr>
				<td><b>Sections</b></td>
				<td><b>FAQ</b></td>
			</tr>
			<tr>
				<td width='25%' valign="top">
					<dx:ASPxGridView ID="gv_sections" runat="server" Width="100%" AutoGenerateColumns="False" DataSourceID="ds_sections" KeyFieldName="id" ClientInstanceName="gv_sections" onrowinserting="gv_sections_RowInserting" onrowupdating="gv_sections_RowUpdating" onhtmleditformcreated="gv_sections_HtmlEditFormCreated" oncustomjsproperties="gv_sections_CustomJSProperties" onrowdeleting="gv_sections_RowDeleting">
						<ClientSideEvents EndCallback="faq_manage.section.handle_callback" />
                        <SettingsCommandButton>
	                        <EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	                        <NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	                        <DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
                        </SettingsCommandButton>
						<Columns>
							<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" VisibleIndex="0" Width="25px" Visible="False">
								<EditFormSettings Visible="False" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Name" FieldName="name" VisibleIndex="1">
							</dx:GridViewDataTextColumn>
							<dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="4" Width="35px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true">
								
								
								
							</dx:GridViewCommandColumn>
							<dx:GridViewDataTextColumn Caption="Articles" FieldName="c" UnboundType="Integer" VisibleIndex="2" Width="75px">
								<HeaderStyle HorizontalAlign="Center" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
						</Columns>
						<SettingsBehavior ConfirmDelete="True" EnableRowHotTrack="True" />
						<SettingsEditing Mode="PopupEditForm" />
						<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" />
						<SettingsText ConfirmDelete="Are you sure?" />
						<SettingsPopup>
							<EditForm HorizontalAlign="WindowCenter" Modal="True" VerticalAlign="WindowCenter" Width="500px" />
						</SettingsPopup>
						<Templates>
							<TitlePanel>
								<dx:ASPxButton ID="bt_new_section" runat="server" AutoPostBack="False" Text="New" Width="100px">
									<ClientSideEvents Click="faq_manage.section.show_new" />
									<Image Url="~/images/icon/icon[add].gif">
									</Image>
								</dx:ASPxButton>
							</TitlePanel>
							<EditForm>
								<div align="center" style="width:100%;padding: 50px 0 50px 0">
								<table>
									<tr>
										<td colspan="2"><b>Section Name</b></td>
									</tr>
									<tr>
										<td colspan="2"><input type="text" ID="section_tb_name" onkeydown="faq_manage.section.catch_enter(this, event)" class="section_tb_name" maxlength="75" style="width:250px" runat="server" /></td>
									</tr>
									<tr>
										<td align="center">
											<button type="button" id="save_section" onclick="faq_manage.section.save(this)">Save</button>
										</td>
										<td align="center">
											<button type="button" id="cancel_section" onclick="faq_manage.section.cancel(this)">Cancel</button>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="center">
										<div id="section_error" class="section_error" runat="server" style="color:#f00"></div>
										</td>
									</tr>
								</table>
								</div>
							</EditForm>
						</Templates>
					</dx:ASPxGridView>
					<asp:SqlDataSource ID="ds_sections" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, a.name, COUNT(b.id) c FROM faq_section a left JOIN faq b ON a.id = b.section_id GROUP BY a.id,a.name"></asp:SqlDataSource>
					<br />
				</td>
				<td width='75%' valign="top">
					<dx:ASPxGridView ID="gv_faq" runat="server" Width="100%" AutoGenerateColumns="False" DataSourceID="ds_faq" KeyFieldName="id" ClientInstanceName="gv_faq" oncustomjsproperties="gv_faq_CustomJSProperties" onhtmleditformcreated="gv_faq_HtmlEditFormCreated" onrowinserting="gv_faq_RowInserting" onrowupdating="gv_faq_RowUpdating" onrowdeleting="gv_faq_RowDeleting">
						<ClientSideEvents RowClick="faq_manage.faq.row_click" DetailRowExpanding="faq_manage.faq.row_expanding" DetailRowCollapsing="faq_manage.faq.row_collapsing" />
                        <SettingsCommandButton>
	                        <EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	                        <NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	                        <DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
                        </SettingsCommandButton>
						<Columns>
							<dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" VisibleIndex="1" Width="50px" Visible="False">
								<EditFormSettings Visible="False" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Section" FieldName="section" VisibleIndex="2" Width="200px">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Subject" FieldName="subject" VisibleIndex="3">
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Customer Visible" FieldName="customer_visible" VisibleIndex="3" Width="100px">
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewCommandColumn ButtonType="Image" Caption="#" VisibleIndex="4" Width="35px" ShowEditButton="true" ShowDeleteButton="true" ShowClearFilterButton="true">
								
								
								
							</dx:GridViewCommandColumn>
						</Columns>
						<SettingsBehavior ConfirmDelete="True" EnableRowHotTrack="True" />
						<SettingsEditing Mode="PopupEditForm" />
						<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" ShowTitlePanel="True" />
						<SettingsDetail AllowOnlyOneMasterRowExpanded="True" ShowDetailRow="True" />
						<SettingsPopup>
							<EditForm HorizontalAlign="WindowCenter" Modal="True" VerticalAlign="WindowCenter" Width="800px" />
						</SettingsPopup>
						<Templates>
							<TitlePanel>
								<dx:ASPxButton ID="bt_new_section" runat="server" AutoPostBack="False" Text="New" Width="100px">
									<ClientSideEvents Click="faq_manage.faq.show_new" />
									<Image Url="~/images/icon/icon[add].gif">
									</Image>
								</dx:ASPxButton>
							</TitlePanel>
							<DetailRow>
								<div runat="server" id="detail"><%# Eval("body") %></div>
							</DetailRow>
							<EditForm>
								<div align="center" style="width:100%;padding: 50px 0 50px 0">
								<table>
									<tr>
										<td><b>Customer Visible?:</b></td>
										<td><asp:CheckBox AutoPostBack="false" runat="server" ID="cb_visible" style="color:#999;" class="cb_visible" Text="(This means this FAQ article will be available via the customer portal)" /></td>
									</tr>
									<tr>
										<td><b>Section:</b></td>
										<td><asp:DropDownList ID="ddl_sections" DataSourceID="ds_sections" runat="server" DataTextField="name" DataValueField="id"></asp:DropDownList></td>
									</tr>
									<tr>
										<td><b>Subject:</b></td>
										<td><input type="text" ID="faq_tb_subject" class="faq_tb_subject" maxlength="75" style="width:450px" runat="server" /></td>
									</tr>
									<tr>
										<td valign="top"><b>Body:</b></td>
										<td>
											<dx:ASPxHtmlEditor ID="html_body" ClientInstanceName="html_body" runat="server" Width="600px">
											</dx:ASPxHtmlEditor>
										</td>
									</tr>
									<tr>
										<td colspan="2">
											<table width="100%" cellpadding="2" cellspacing="0">
												<td align="right" width="50%">
													<button type="button" style="width:100px;font-weight:bold" id="save_faq" onclick="faq_manage.faq.save(this)">Save</button>
												</td>
												<td align="left" width="50%">
													<button type="button" style="width:100px;font-weight:bold" id="cancel_faq" onclick="faq_manage.faq.cancel(this)">Cancel</button>
												</td>
											</table>
										</td>
									</tr>
									<tr>
										<td colspan="2" align="center">
										<div id="faq_error" class="faq_error" runat="server" style="color:#f00"></div>
										</td>
									</tr>
								</table>
								</div>
							</EditForm>
						</Templates>
					</dx:ASPxGridView>
					<asp:SqlDataSource ID="ds_faq" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id, b.name section,a.section_id, a.subject,a.body, IF(a.customer_visible = 1, 'True', 'False') customer_visible FROM faq a LEFT JOIN faq_section b ON a.section_id = b.id;"></asp:SqlDataSource>
				</td>
			</tr>
		</table>
	</div>
</ASP:CONTENT>