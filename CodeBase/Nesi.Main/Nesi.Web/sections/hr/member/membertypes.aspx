<%@ Page Title="Member Types Admin" Language="C#"  MasterPageFile="~/IntraDefault.master"  AutoEventWireup="true"  Inherits="sections_hr_member_membertypes" EnableTheming="True" Theme="NETheme01" Codebehind="membertypes.aspx.cs" %>	



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
<asp:Content ID="header1" ContentPlaceHolderID="header_placeholder" runat="server">

	<style type="text/css">
		.hidden
			{
			display: none;
			}
	</style>
	<script type="text/javascript">
		function bind_tooltips()
			{
			$(".opt1").each(function()
				{
				$(this).tip();
				});
			}
		var is_sortable		= false;
		function toggle_sort_mode()
			{
			var o	=  $('.dxgvTable tbody');
			if(is_sortable)
				{
				gvtypes.PerformCallback("init_sort|false");
				unbind_sortable(o);
				}
			else
				{
				gvtypes.PerformCallback("init_sort|true");
				is_sortable		= true;
				}
			$("#sorting_active").toggle();
			}
		function unbind_sortable(o)
			{
			o.sortable("destroy");
			o.enableSelection();
			is_sortable		= false;
			$(".titlebar, .headerbar, .filterbar, .editcol").removeClass("hidden");
			}
		function bind_sortable()
			{
			var o	= $('.dxgvTable tbody');
			o.sortable(	{ 
						cursor:		'move',
						tolerance:	'pointer',
						update:		
							function(event, ui)
								{
								var prev_item_index		= ui.item.index();
								var this_item_index		= ui.item.index()+1;
								var prev_id				= $('.dxgvTable tbody tr:nth-child('+prev_item_index+')').find(".m_id").text();
								var this_id				= $('.dxgvTable tbody tr:nth-child('+this_item_index+')').find(".m_id").text();
								gvtypes.PerformCallback("sort|"+this_id+"|"+prev_id);
								}
						}
						);
			o.disableSelection();
			}
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" Runat="Server">
	
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" Runat="Server">
	<table width="100%" cellpadding="5" cellspacing="0">
		<tr>
			<td>
				<dx:ASPxComboBox ID="ddlbranch" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" TextField="ddl_name" ValueField="id" ValueType="System.Int32" Visible="False">
				</dx:ASPxComboBox>
				<div style="padding:5px;width:400px;margin-top:3px;background-color:#090;color:#fff;">
				<button type="button" onclick="toggle_sort_mode()"><img src="/images/icon/icon[replicate].gif" height="16" width="16" align="absmiddle" alt="Toggle Rate Sheet Sort Mode" /> Toggle Rate Sheet Sort Mode</button><br />
				Upon pressing this button you will be able to drag and rearrange the membertypes shown on the rates sheet.
				</div>
				<div id="sorting_active" style="background-color:#090;color:#fff;text-align:center;padding:5px;margin-bottom:5px;display:none;"><b>Sorting is active!</b></div>
				<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" ></asp:SqlDataSource>
				<dx:ASPxGridView ID="gvtypes" runat="server" ClientInstanceName="gvtypes" 
					Width="100%" AutoGenerateColumns="False" DataSourceID="ds_membertypes" 
					Font-Names="Arial" KeyFieldName="membertype_id" 
					OnRowUpdating="gvtypes_RowUpdating" OnRowInserting="gvtypes_RowInserting" 
					OnHtmlEditFormCreated="gvtypes_HtmlEditFormCreated" 
					OnHtmlDataCellPrepared="gvtypes_HtmlDataCellPrepared" 
					OnCancelRowEditing="gvtypes_CancelRowEditing" 
					oncustomjsproperties="gvtypes_CustomJSProperties1" 
					oncustomcallback="gvtypes_CustomCallback" Theme="NETheme01">
					<ClientSideEvents EndCallback="function(s, e) {
	if(is_sortable)
		{
		bind_sortable();
		}
}" />
																<SettingsCommandButton>
																	<DeleteButton Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Text="Delete" />
																	<EditButton Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Text="Edit" />
																	<NewButton Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Text="New" />
																</SettingsCommandButton>
					<Columns>
						<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="0" Width="40px"  ShowEditButton="true" ShowClearFilterButton="true">
							
							
							<CellStyle CssClass="editcol">
							</CellStyle>
						</dx:GridViewCommandColumn>
						<dx:GridViewDataTextColumn Caption="ID" FieldName="membertype_id" ReadOnly="True" VisibleIndex="1" Width="30px">
							<EditFormSettings Visible="False" />
							<DataItemTemplate>
								<b class="m_id" runat="server" id="m_id" data-id='<%# Eval("membertype_id") %>'><%# Eval("membertype_id") %></b>
							</DataItemTemplate>
							<CellStyle HorizontalAlign="Center">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Title" FieldName="membertype_name" VisibleIndex="2" Width="100%">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataCheckColumn Caption="Active" FieldName="active" VisibleIndex="3" Width="50px">
							<Settings AutoFilterCondition="Equals" />
							<EditFormSettings Visible="False" />
						</dx:GridViewDataCheckColumn>
						<dx:GridViewDataCheckColumn Caption="Restricted Use" FieldName="is_elevated" VisibleIndex="4" Width="50px">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataCheckColumn>
						<dx:GridViewDataCheckColumn Caption="On Rates Sheet" FieldName="show_on_ratesheet" VisibleIndex="5" Width="50px">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataCheckColumn>
						<dx:GridViewDataComboBoxColumn Caption="Reports To" FieldName="reports_to" VisibleIndex="6" Width="300px">
							<PropertiesComboBox DataSourceID="ds_membertypes" TextField="membertype_name" ValueField="membertype_id" ValueType="System.Int32">
							</PropertiesComboBox>
							<EditFormSettings Visible="False" />
							<CellStyle Wrap="False">
							</CellStyle>
						</dx:GridViewDataComboBoxColumn>
						<dx:GridViewDataTextColumn Caption="Responsibilities" Name="responsibilities" Visible="False" VisibleIndex="9">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Chargeout Rate" Name="chargeout" Visible="False" VisibleIndex="8" Width="50px">
							<EditFormSettings Visible="False" />
							<CellStyle HorizontalAlign="Center">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Ratesheet Order" Visible="True" VisibleIndex="10" Width="50px" FieldName="ratesheet_order" Name="ratesheet_order">
							<EditFormSettings Visible="True" />
							<CellStyle HorizontalAlign="Center">
							</CellStyle>
						</dx:GridViewDataTextColumn>
						<dx:GridViewDataTextColumn Caption="Avg Wage" FieldName="avg_wage" Visible="False" VisibleIndex="7" Width="80px">
							<EditFormSettings Visible="False" />
						</dx:GridViewDataTextColumn>
					</Columns>
					<SettingsPager Mode="ShowAllRecords">
					</SettingsPager>
					<SettingsEditing EditFormColumnCount="7" Mode="PopupEditForm" />
					<Settings ShowFilterBar="Visible" ShowFilterRow="True" ShowTitlePanel="True" />
					<SettingsText PopupEditFormCaption="Add/Edit Membertype" />
					<SettingsPopup>
						<EditForm HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" Width="1000px" MinHeight="900px"  />
					</SettingsPopup>
					<Styles>
						<Header Font-Bold="False">
						</Header>
						<Row CssClass="draggable" BackColor="White">
						</Row>
						<TitlePanel CssClass="titlebar">
						</TitlePanel>
						<FilterRow CssClass="filterbar">
						</FilterRow>
						<FilterBar CssClass="filterbar">
						</FilterBar>
					</Styles>
					<Templates>
						<TitlePanel>
							<table style="width: 100%;">
								<tr>
									<td>
										<dx:ASPxButton ID="btnAddType" runat="server" AutoPostBack="False" Text="Add Member Type" Wrap="False">
											<ClientSideEvents Click="function(s, e) {gvtypes.AddNewRow();}" />
										</dx:ASPxButton>
									</td>
									<td width="100%">
										&nbsp;
									</td>
								</tr>
							</table>
						</TitlePanel>
						<EditForm>
							<editform>
								<iframe ID="IFrame_type" runat="server" frameborder="0" height="880" name="IFrame_type" width="950" scrolling="yes"></iframe>
							</editform>
						</EditForm>
					</Templates>
				</dx:ASPxGridView>
				<asp:SqlDataSource runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select membertype_id,membertype_name from membertype where active = 1 order by membertype_name" ID="membertype"></asp:SqlDataSource>
				<asp:SqlDataSource ID="ds_membertypes" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="Select *,0 avg_wage from membertype order by active desc,membertype_name asc "></asp:SqlDataSource>
			</td>
		</tr>
	</table>
</asp:Content>
