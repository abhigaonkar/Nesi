<%@ Page Language="C#" MasterPageFile="~/IntraDefault.master" AutoEventWireup="true" Inherits="master_customers" Title="Master Customers Grid" CodeBehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>

<%@ Register Src="~/sections/customer/modules/sales.ascx" TagName="sales" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMasterMenu" runat="Server">
	<div id="divMenu" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMasterLeft" runat="Server">
	<div id="divSide" runat="server">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMasterSubMenu" runat="Server">
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="header_placeholder" runat="server">
	<link rel="Stylesheet" type="text/css" href="/css/customer.css" />
	<style type="text/css">
		.options {
			background: -moz-linear-gradient(top, rgba(204,204,204,0.65) 0%, rgba(204,204,204,0.64) 1%, rgba(0,0,0,0) 100%); /* FF3.6+ */
			background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(204,204,204,0.65)), color-stop(1%,rgba(204,204,204,0.64)), color-stop(100%,rgba(0,0,0,0))); /* Chrome,Safari4+ */
			background: -webkit-linear-gradient(top, rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* Chrome10+,Safari5.1+ */
			background: -o-linear-gradient(top, rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* Opera 11.10+ */
			background: -ms-linear-gradient(top, rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* IE10+ */
			background: linear-gradient(to bottom, rgba(204,204,204,0.65) 0%,rgba(204,204,204,0.64) 1%,rgba(0,0,0,0) 100%); /* W3C */
			filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#a6cccccc', endColorstr='#00000000',GradientType=0 ); /* IE6-9 */
		}

		.fleft {
			float: left;
			display: block;
		}

			.fleft .table {
				background-color: #fff;
				border: solid 1px #999;
				margin: 5px;
				border-radius: 3px;
			}

		.style5 {
			font-size: xx-small;
		}
	</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphMasterBody" runat="Server">
	<asp:ScriptManager runat="server" ID="sm">
	</asp:ScriptManager>
			<lc:LayoutControl runat="server" ID="layout" __is_private="True" GridviewID="gv_MasterCustomers" />
	<asp:UpdatePanel runat="server" ID="up">
		<ContentTemplate>

			<script type="text/javascript" language="javascript">

				$(document).ready(function () {
					Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginReqHandler);
					Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndReqHandler);
				});
				function EndReqHandler() {
					please_wait("stop");
				}
				function BeginReqHandler() {
					please_wait("start");
				}
				function note_show(obj) {
					var exists = $("#note_window").size() > 0;
					var o = $(obj).offset();
					var x = o.left;
					var y = o.top;
					var customer_id = $(obj).attr('data-cid');
					var address_id = $(obj).attr('data-aid');
					if (exists && $("#note_window")) {
						$("#note_window").remove();
					}
					var html = "<div id='note_window'>";
					html += "<b>Note:</b>\
										<br/><textarea style='width:99%;height:100px;' maxlength='512'></textarea>\
										<br><button style='font-size:11px;font-weight:bold;' onclick='handle_note(this)' data-customer_id='" + customer_id + "' data-address_id='" + address_id + "' type='button'><img align='absmiddle' src='/images/icon/icon[save].gif'/> Save</button>\
										<button style='font-size:11px;font-weight:bold;' onclick=\"$('#note_window').remove();\"><img align='absmiddle' src='/images/icon/icon[delete].gif'/> Close</button>";
					html += "</div>";
					$("body").append(html);
					var off = $(obj).offset();
					$("#note_window").css({
						"position": "absolute",
						"top": off.top + "px",
						"left": off.left + $(obj).width() + "px",
						"width": "300px",
						"padding": "5px",
						"border": "solid 1px #cc7",
						"background-color": "#ff7"
					}).find("textarea").focus().val();
					var left = $(document).outerWidth() - $(window).width();
					$('body, html').scrollLeft(left);
				}
				function handle_note(obj) {
					$(obj).attr("disabled", true);
					please_wait("start");
					var _defs = {
						a: "handle_note",
						customer_id: $(obj).attr('data-customer_id'),
						address_id: $(obj).attr('data-address_id'),
						note: $("#note_window").find("textarea").val()
					};
					$.get("./index.aspx", _defs, function (_returned) {
						if (_returned == "SUCCESS") {
							$('#note_window').remove();
							please_wait("stop");
							gv_MasterCustomers.Refresh();
						}
						else {
							please_wait("stop");
							$(obj).removeAttr('disabled');
							alert(_returned);
						}
					});
				}
				function handle_status(obj) {
					var client_instance = eval($(obj).attr('ciname'));
					var customer_id = $(obj).attr('data-cid');
					var address_id = $(obj).attr('data-aid');
					var status_id = $(obj).val();
					$(obj).attr("disabled", true);
					please_wait("start");
					var _defs = {
						a: "handle_status",
						customer_id: $(obj).attr('data-cid'),
						address_id: $(obj).attr('data-aid'),
						status_id: $(obj).val()
					};
					$.get("./index.aspx", _defs, function (_returned) {
						if (_returned == "SUCCESS") {
							please_wait("stop");
							gv_MasterCustomers.Refresh();
						}
						else {
							please_wait("stop");
							$(obj).removeAttr('disabled');
							alert(_returned);
						}
					});
				}
				function disable_grid(s, e) {
					gv_MasterCustomers.ClientEnabled = false;
				}
				function enable_grid(s, e) {
					gv_MasterCustomers.ClientEnabled = true;
				}
				function do_action(s, e) {

				}
				function get_customer(id) {
					boing("/#/opens/10/customers/" + id, "customer", 1280, 960);
				}
				function toggle_options(obj, force) {
					// force = keep open.
					if (force == undefined) {
						force = false;
					}
					var current = $(".fleft").is(":visible");
					if (!force || !current && force) {

						$(obj).text(!current ? "^^ Toggle Options ^^" : "vv Toggle Options vv");
						$(".fleft").each(
							function () {
								$(this).toggle();
							});
					}
				}
				function CancelEvent(evt) {
					return ASPxClientUtils.PreventEventAndBubble(evt);
				}
			</script>

			<dx:ASPxCallbackPanel ID="cb_action" runat="server" ClientInstanceName="cb_action" OnCallback="cb_action_Callback" Width="200px">
				<LoadingPanelStyle HorizontalAlign="Center" VerticalAlign="Middle">
				</LoadingPanelStyle>
			</dx:ASPxCallbackPanel>
			<asp:SqlDataSource ID="sds_masterCustomer" runat="server"
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
				SelectCommand="Call report_master_customer(CURDATE(),CURDATE(),@member_id)">
				<SelectParameters>
					<asp:Parameter Name="@member_id" />
				</SelectParameters>
			</asp:SqlDataSource>

			<asp:SqlDataSource ID="sds_acct_manager" runat="server"
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
				SelectCommand="SELECT Member_ID, member_fullname as _name FROM member where member_membertype_id in (8,11,27,34,35,37,38,39,46,49,52,53,54,55,56,59,4,5,12,17,24,25,41) and member_status ='Active' union select 0 Member_ID, 'No One' _name order by _name "></asp:SqlDataSource>
			<asp:SqlDataSource ID="sds_controls_manager" runat="server"
				ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
				ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"
				SelectCommand="SELECT Member_ID, member_fullname as _name FROM member where member_membertype_id in (17,23,5,11,2,55,33,42,38,25,4,24,34,49) and member_status ='Active' union select 0 Member_ID, 'No One' _name order by _name "></asp:SqlDataSource>
			<asp:SqlDataSource ID="sds_last_action" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT History_Action_Type_ID, History_Action_Type_Action FROM history_action_type"></asp:SqlDataSource>
			<dx:ASPxGridView ID="gv_MasterCustomers" runat="server" va
				AutoGenerateColumns="False" ClientInstanceName="gv_MasterCustomers"
				KeyFieldName="address_id" OnRowUpdating="gv_MasterCustomers_RowUpdating"
				OnHtmlDataCellPrepared="gv_MasterCustomers_HtmlDataCellPrepared"
				OnHtmlEditFormCreated="gv_MasterCustomers_HtmlEditFormCreated"
				OnCustomCallback="gv_MasterCustomers_CustomCallback"
				OnCustomJSProperties="gv_MasterCustomers_CustomJSProperties" Font-Names="Arial"
				Font-Size="9pt" Width="100%"
				DataSourceID="sds_masterCustomer"
				OnHtmlFooterCellPrepared="gv_MasterCustomers_HtmlFooterCellPrepared"
				Theme="NETheme01">
				<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control"
					AutoFilterRowInputDelay="4000" EnableCustomizationWindow="True" />
				<Styles>
					<Header Font-Bold="True">
					</Header>
					<Cell Wrap="False">
					</Cell>
					<EditFormColumnCaption Font-Bold="True">
					</EditFormColumnCaption>
				</Styles>

				<StylesPopup EditForm-Header-BackColor="#0066FF" EditForm-Header-ForeColor="White" EditForm-Header-Font-Bold="True" EditForm-Content-Paddings-Padding="20px">

					<Common>
						<ModalBackground Opacity="0">
						</ModalBackground>
					</Common>

					<EditForm>
						<Content>
							<Paddings Padding="20px" />
						</Content>
						<Header BackColor="#0066FF" Font-Bold="True" ForeColor="White">
						</Header>
						<ModalBackground Opacity="0">
						</ModalBackground>
					</EditForm>
					<FilterBuilder>
						<ModalBackground Opacity="0">
						</ModalBackground>
					</FilterBuilder>
				</StylesPopup>

				<SettingsPager AlwaysShowPager="True" NumericButtonCount="5" PageSize="50" Position="TopAndBottom">
					<AllButton Text="All">
					</AllButton>
				</SettingsPager>
				<SettingsEditing Mode="PopupEditForm" />
				<SettingsPopup EditForm-Height="700px" EditForm-HorizontalAlign="WindowCenter" EditForm-Modal="True" EditForm-VerticalAlign="WindowCenter" EditForm-Width="1100px">
					<EditForm Height="700px" HorizontalAlign="WindowCenter" Modal="True" VerticalAlign="WindowCenter" Width="1100px" />
					<CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="TopSides" />
				</SettingsPopup>
				<SettingsText CommandUpdate="Save" PopupEditFormCaption="Customer Sales" />
				<TotalSummary>
					<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="ytd_rev" ShowInColumn="YTD Rev" SummaryType="Sum" />
					<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="12months" ShowInColumn="12months" SummaryType="Sum" />
					<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="24months" ShowInColumn="24months" SummaryType="Sum" />
					<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="6months" ShowInColumn="6months" SummaryType="Sum" />
					<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="revfromdate" ShowInColumn="$ Rev Date Range" SummaryType="Sum" />
					<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="lytd_rev" ShowInColumn="Last YTD Rev" SummaryType="Sum" />
				</TotalSummary>
				<SettingsCommandButton>
					<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
					<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
					<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
				</SettingsCommandButton>
				<Columns>
					<dx:GridViewDataColumn Caption="address_id" FieldName="address_id" VisibleIndex="0" Visible="false"></dx:GridViewDataColumn>
					<dx:GridViewCommandColumn VisibleIndex="1" Width="30px" ButtonType="Image"
						Caption=" " MinWidth="30" ShowSelectCheckbox="True" ShowEditButton="false">

						<CellStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False">
						</CellStyle>
						<HeaderTemplate>
							<dx:ASPxCheckBox ID="chk_sa" runat="server" CheckState="Unchecked"
								ClientInstanceName="chk_sa" Text="Selected All">
								<ClientSideEvents CheckedChanged="function(s, e) {gv_MasterCustomers.SelectAllRowsOnPage(s.GetChecked());}" />
							</dx:ASPxCheckBox>
						</HeaderTemplate>
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn FieldName="ID" ReadOnly="True" VisibleIndex="2" Width="50px" MinWidth="10">
						<PropertiesTextEdit Width="50px">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="BV No" ReadOnly="True" VisibleIndex="3" Width="50px" ToolTip="BV Work Order" MinWidth="10">
						<PropertiesTextEdit Width="60px">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Customer" ReadOnly="True" VisibleIndex="4" Width="150px" ToolTip="Customer" MinWidth="10">
						<PropertiesTextEdit DisplayFormatString="{0}" Width="200px">
						</PropertiesTextEdit>
						<DataItemTemplate>
							<a href="javascript:void(-1)" onclick="get_customer(<%# Eval("ID") %>)">
								<%#Container.Text %>
							</a>
						</DataItemTemplate>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataMemoColumn FieldName="Notes" VisibleIndex="5" Width="100%" ToolTip="Sales Notes" MinWidth="10">
						<PropertiesMemoEdit Width="100%" Height="60px">
							<Style Wrap="True"></Style>
						</PropertiesMemoEdit>
						<EditFormSettings Caption="Sales Notes" Visible="False" />
						<DataItemTemplate>
							<dx:ASPxMemo ID="mem" runat="server" ClientInstanceName="mem" Height="20px" Text='<%# Eval("Notes") %>'
								Width="100%" Native="True" ReadOnly="True" Font-Names="Arial" Font-Size="8pt"
								OnDataBinding="mem_PreRender">
								<ClientSideEvents Init="function(s, e) {
	var text = s.GetText().replace(/\r/g, '');
	if (text.length &gt;1 )
		{
			s.SetHeight(60);
			s.GetMainElement().style.backgroundColor = &quot;yellow&quot;
			s.GetInputElement().style.backgroundColor = &quot;yellow&quot;
		}}" />
							</dx:ASPxMemo>
						</DataItemTemplate>
						<CellStyle Wrap="False" BackColor="#FFFFCC">
						</CellStyle>
					</dx:GridViewDataMemoColumn>
					<dx:GridViewDataCheckColumn FieldName="On Hold" VisibleIndex="6" Width="50px" ToolTip="On Hold" MinWidth="10">
						<PropertiesCheckEdit ValueChecked="T" ValueType="System.String" ValueUnchecked="F">
						</PropertiesCheckEdit>
						<Settings AllowHeaderFilter="True" FilterMode="DisplayText" ShowFilterRowMenu="True" />
					</dx:GridViewDataCheckColumn>
					<dx:GridViewDataTextColumn FieldName="Status" VisibleIndex="7" Width="100px" ToolTip="Customer Status" MinWidth="10">
						<PropertiesTextEdit Width="200px">
						</PropertiesTextEdit>
						<Settings FilterMode="DisplayText" />
						<DataItemTemplate>
							<asp:DropDownList ID="ASPxComboBox2" runat="server" DataTextField="status"
								DataValueField="id" Width="100%" OnInit="ASPxComboBox2_Init">
							</asp:DropDownList>
						</DataItemTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Account_Manager" VisibleIndex="8" Width="100px" ToolTip="Account Manager" MinWidth="10" Name="am">
						<PropertiesTextEdit Width="200px">
						</PropertiesTextEdit>
						<Settings AllowHeaderFilter="True" FilterMode="DisplayText" ShowFilterRowMenu="True" />
						<CellStyle Wrap="False">
						</CellStyle>
						<FooterTemplate>
							<dx:ASPxComboBox ID="cbam_footer" runat="server" ClientInstanceName="cbam_footer"
								Width="100%" TextField="_name" ValueField="Member_ID" ValueType="System.Int32" DataSourceID="sds_acct_manager"
								OnInit="footer_ddl_control_Init">
								<ClientSideEvents BeginCallback="disable_grid" EndCallback="enable_grid" />
							</dx:ASPxComboBox>
							<dx:ASPxButton ID="btn_ud_cbam" runat="server" AutoPostBack="False"
								Text="Update Selected" Wrap="True" OnInit="footer_btn_control_Init">
								<ClientSideEvents Click="function(s, e) {gv_MasterCustomers.PerformCallback('am|' + cbam_footer.GetValue());}" />
							</dx:ASPxButton>
						</FooterTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="Year End" VisibleIndex="9" Width="50px" ToolTip="Year End" MinWidth="10">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Last Action Date" FieldName="Customer_LastDateTime" VisibleIndex="10" Width="50px" ToolTip="Last Action Date" MinWidth="10">
						<PropertiesDateEdit DisplayFormatInEditMode="True" DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd" Width="200px">
						</PropertiesDateEdit>
						<EditFormSettings Caption="Last Action Date" Visible="False" />
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn FieldName="Call Cycle" VisibleIndex="11" Width="50px" ToolTip="Call Cycle" MinWidth="10">
						<PropertiesTextEdit Width="40px">
						</PropertiesTextEdit>
						<CellStyle BackColor="#CCFFCC">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn FieldName="business_unit" ReadOnly="True" VisibleIndex="12"
						Width="50px" ToolTip="Business Unit" MinWidth="10" Caption="Business Unit">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn FieldName="Countdown Date" VisibleIndex="13" Width="50px" ToolTip="Date Countdown Expires" MinWidth="10">
						<PropertiesDateEdit DisplayFormatInEditMode="True" DisplayFormatString="yyyy-MM-dd" Width="200px">
						</PropertiesDateEdit>
						<CellStyle BackColor="#CCFFFF">
						</CellStyle>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn FieldName="Countdown" VisibleIndex="14" Width="50px" ToolTip="Countdown to End of Time since Last Action" MinWidth="10">
						<PropertiesTextEdit Width="40px">
						</PropertiesTextEdit>
						<CellStyle Wrap="False" BackColor="#CCCCFF">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataComboBoxColumn Caption="Last Action" FieldName="Action" ToolTip="Last Action" VisibleIndex="15" Width="50px" MinWidth="10">
						<PropertiesComboBox DataSourceID="sds_last_action" TextField="History_Action_Type_Action" ValueField="History_Action_Type_ID" ValueType="System.Int32" Width="200px">
						</PropertiesComboBox>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataComboBoxColumn>
					<dx:GridViewDataMemoColumn Caption="Action History" Name="Action History"
						ReadOnly="True" Visible="False" VisibleIndex="16" Width="100px" MinWidth="10">
						<PropertiesMemoEdit Rows="10">
							<Style Wrap="False"></Style>
						</PropertiesMemoEdit>
						<EditItemTemplate>
							<dx:ASPxGridView ID="Grid_Customer_History" runat="server" AutoGenerateColumns="False" KeyFieldName="Customer_History_ID" OnRowDeleting="Grid_Customer_History_RowDeleting" Width="100%">
								<Styles GroupButtonWidth="28">
									<Header ImageSpacing="5px" SortingImageSpacing="5px">
									</Header>
									<LoadingPanel ImageSpacing="8px">
									</LoadingPanel>
								</Styles>
								<SettingsLoadingPanel ImagePosition="Top" />
								<SettingsCommandButton>
									<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
									<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
									<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
								</SettingsCommandButton>
								<Columns>
									<dx:GridViewDataTextColumn Caption="ID" FieldName="Customer_History_ID" Visible="False" VisibleIndex="0">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Date" FieldName="Customer_History_Date" VisibleIndex="0">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Member" FieldName="Member_FirstName" VisibleIndex="1">
									</dx:GridViewDataTextColumn>
									<dx:GridViewDataTextColumn Caption="Action" FieldName="History_Action_Type_Action" VisibleIndex="2">
									</dx:GridViewDataTextColumn>
									<dx:GridViewCommandColumn ButtonType="Image" Caption=" " VisibleIndex="3" ShowDeleteButton="true">
									</dx:GridViewCommandColumn>
								</Columns>
								<Paddings Padding="1px" />
								<StylesEditors>
									<CalendarHeader Spacing="1px">
									</CalendarHeader>
									<ProgressBar Height="29px">
									</ProgressBar>
								</StylesEditors>
							</dx:ASPxGridView>
						</EditItemTemplate>
						<CellStyle Wrap="False">
						</CellStyle>
					</dx:GridViewDataMemoColumn>
					<dx:GridViewDataTextColumn Caption="City" FieldName="City" VisibleIndex="17"
						Width="100px" ToolTip="City" MinWidth="10">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Phone" FieldName="phone" VisibleIndex="18"
						Width="60px" ToolTip="Phone Number" MinWidth="10">
						<PropertiesTextEdit Width="95px">
						</PropertiesTextEdit>
						<CellStyle Font-Bold="True">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Last Invoice (d)"
						FieldName="Days Since Last Invoice" ReadOnly="True" VisibleIndex="19"
						Width="35px" ToolTip="Days Since Last Invoice" MinWidth="10">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Controls Person" FieldName="Controls"
						VisibleIndex="20" Width="100px" ToolTip="Controls Person for this customer"
						MinWidth="10" Name="controlsguy">
						<PropertiesTextEdit Width="200px">
						</PropertiesTextEdit>
						<Settings FilterMode="DisplayText" />
						<EditItemTemplate>
							<dx:ASPxComboBox ID="ddlcustomer_controls" runat="server" DataSourceID="sds_controls_manager" TextField="_name" Value='<%# Bind("Controls") %>' ValueField="member_id" ValueType="System.Int32" Width="100%" OnInit="footer_ddl_control_Init">
							</dx:ASPxComboBox>
						</EditItemTemplate>
						<CellStyle Wrap="False">
						</CellStyle>
						<FooterTemplate>
							<dx:ASPxComboBox ID="cbcg_footer" runat="server" ClientInstanceName="cbcg_footer"
								Width="100%" TextField="_name" ValueField="member_id" ValueType="System.Int32" DataSourceID="sds_controls_manager"
								OnInit="footer_ddl_control_Init">
								<ClientSideEvents BeginCallback="disable_grid" EndCallback="enable_grid" />
							</dx:ASPxComboBox>
							<dx:ASPxButton ID="btn_ud_cbcg" runat="server" AutoPostBack="False"
								Text="Update Selected" Wrap="True" OnInit="footer_btn_control_Init">
								<ClientSideEvents Click="function(s, e) {gv_MasterCustomers.PerformCallback('c|' + cbcg_footer.GetValue());}" />
							</dx:ASPxButton>
						</FooterTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Discount" FieldName="Discount"
						VisibleIndex="21" Width="40px" ToolTip="Default Discount" MinWidth="10">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataColumn Caption="Days Since Added"
						FieldName="dayssinceadded" VisibleIndex="22" Width="30px"
						ToolTip="Days Since Added" MinWidth="10">
						<Settings FilterMode="Value" />
					</dx:GridViewDataColumn>
					<dx:GridViewDataTextColumn Caption="$ 12 Months" FieldName="12months"
						Name="12months" VisibleIndex="23" Width="40px" MinWidth="10">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Overall Margin" FieldName="overall_margin"
						VisibleIndex="24" Visible="False" ReadOnly="True" MinWidth="10" Width="50px">
						<PropertiesTextEdit DisplayFormatString="P2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="QC1" FieldName="is_qc1" Visible="False"
						VisibleIndex="25" MinWidth="10" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="QC2" FieldName="is_qc2" VisibleIndex="26"
						MinWidth="10" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Billing Address"
						FieldName="billing_address" MinWidth="50" Visible="False" VisibleIndex="27"
						Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Credit Limit" FieldName="credit_limit"
						MinWidth="25" Visible="False" VisibleIndex="28" Width="50px">
						<PropertiesTextEdit DisplayFormatString="{0:C2}">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Customer Requires PO"
						FieldName="customer_req_po" Visible="False" VisibleIndex="29" MinWidth="10"
						Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Location" FieldName="location"
						Visible="False" VisibleIndex="30" MinWidth="10" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="$ 24 Months" FieldName="24months"
						Name="24months" VisibleIndex="31" Width="40px" MinWidth="50" Visible="false">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="$ 6 Months" FieldName="6months"
						Name="6months" VisibleIndex="32" Width="40px" MinWidth="50" Visible="false">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Days Since Last Action"
						FieldName="days_since_lastdatetime" Name="days_since_lastdatetime"
						VisibleIndex="33" Width="40px" MinWidth="50" Visible="false">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="ISR" FieldName="isr" MinWidth="10"
						VisibleIndex="34" Width="50px" ToolTip="Inside Sales Rep" Name="isr">
						<Settings FilterMode="DisplayText" />
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="OSR (BD)" FieldName="osr" MinWidth="10"
						VisibleIndex="35" Width="50px" ToolTip="Outside Sales Rep" Name="osr">
						<EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="RAM" FieldName="ram" MinWidth="10"
						VisibleIndex="36" Width="50px" ToolTip="Regional Account Manager" Name="ram">
						<EditFormSettings Visible="False" />
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Account Code"
						FieldName="customer_accountcode" MinWidth="15" VisibleIndex="37" Width="50px"
						ToolTip="Location Account Code (A,B,C,D)">
						<EditFormSettings Visible="False" />
						<DataItemTemplate>
							<dx:ASPxComboBox ID="cb_ac" runat="server"
								Width="100%"
								Value='<%# Eval("customer_accountcode") %>' OnCustomJSProperties="cbddl_CustomJSProperties" OnInit="footer_ddl_control_Init">
								<ClientSideEvents SelectedIndexChanged="function(s,e){cb_action.PerformCallback('c|'+s.cpAddress_id+'|'+s.GetValue());}" />
								<Items>
									<dx:ListEditItem Text="" Value="" />
									<dx:ListEditItem Text="A" Value="A" />
									<dx:ListEditItem Text="B" Value="B" />
									<dx:ListEditItem Text="C" Value="C" />
									<dx:ListEditItem Text="D" Value="D" />
									<dx:ListEditItem Text="Target New" Value="TN" />
									<dx:ListEditItem Text="Target Expand" Value="TE" />
									<dx:ListEditItem Text="Prospect" Value="P" />
								</Items>
							</dx:ASPxComboBox>
						</DataItemTemplate>
						<FooterTemplate>
							<dx:ASPxComboBox ID="cb_ac_footer" runat="server" ClientInstanceName="cb_ac_footer"
								Width="100%"
								OnInit="footer_ddl_control_Init">

								<Items>
									<dx:ListEditItem Text="" Value="" />
									<dx:ListEditItem Text="A" Value="A" />
									<dx:ListEditItem Text="B" Value="B" />
									<dx:ListEditItem Text="C" Value="C" />
									<dx:ListEditItem Text="Target New" Value="TN" />
									<dx:ListEditItem Text="Target Expand" Value="TE" />
									<dx:ListEditItem Text="Prospect" Value="P" />
								</Items>
							</dx:ASPxComboBox>
							<dx:ASPxButton ID="btn_ud_ac" runat="server" AutoPostBack="False"
								Text="Update Selected" Wrap="True" OnInit="footer_btn_control_Init">
								<ClientSideEvents Click="function(s, e) {gv_MasterCustomers.PerformCallback('a|' + cb_ac_footer.GetValue());}" />
							</dx:ASPxButton>
						</FooterTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="PM" FieldName="Project_Manager" MinWidth="10"
						VisibleIndex="38" Width="50px" ToolTip="Default Project Manager" Name="pm">
						<Settings FilterMode="DisplayText" />
						<EditFormSettings Visible="False" />
						<FooterTemplate>
							<dx:ASPxComboBox ID="cbpm_footer" runat="server" ClientInstanceName="cbpm_footer"
								Width="100%" TextField="_name" ValueField="Member_ID" ValueType="System.Int32" DataSourceID="sds_acct_manager"
								ClientEnabled="False" OnInit="footer_ddl_control_Init">
								<ClientSideEvents BeginCallback="disable_grid" EndCallback="enable_grid" />
							</dx:ASPxComboBox>
							<dx:ASPxButton ID="btn_ud_pm" runat="server" AutoPostBack="False"
								Text="Update Selected" Wrap="True" ClientEnabled="False" OnInit="footer_btn_control_Init">
								<ClientSideEvents Click="function(s, e) {gv_MasterCustomers.PerformCallback('p|' + cbpm_footer.GetValue());}" />
							</dx:ASPxButton>
						</FooterTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="$ Rev Date Range" FieldName="revfromdate"
						MinWidth="36" Visible="False" VisibleIndex="39" Width="50px">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
						<HeaderCaptionTemplate>
							<div onmousedown="return CancelEvent(event)" onmouseup="return CancelEvent(event)">
								<asp:Panel ID="dte_pnl" runat="server" CssClass="fleft">
									<table cellpadding="5" cellspacing="0" class="table">
										<tr>
											<td align="left">
												<b>From </b>
											</td>
											<td>
												<dx:ASPxDateEdit ID="dte_from" runat="server" CssClass="style5"
													EditFormat="Custom" EditFormatString="yyyy-MM-dd" Width="75px" ClientInstanceName="dte_from">
												</dx:ASPxDateEdit>
											</td>
										</tr>
										<tr>
											<td align="left">
												<b>To </b>
											</td>
											<td>
												<dx:ASPxDateEdit ID="dte_to" runat="server" CssClass="style5"
													EditFormat="Custom" EditFormatString="yyyy-MM-dd" Width="75px" ClientInstanceName="dte_to">
												</dx:ASPxDateEdit>
											</td>
										</tr>
										<tr>
											<td align="center" colspan="2">
												<dx:ASPxButton ID="dte_button" runat="server" OnClick="dte_button_Click"
													Text="Submit" Width="95%">
												</dx:ASPxButton>
											</td>
										</tr>
										<tr>
											<td colspan="2">
												<dx:ASPxLabel ID="dte_error" runat="server" ForeColor="Red">
												</dx:ASPxLabel>
											</td>
										</tr>
									</table>
								</asp:Panel>
							</div>
						</HeaderCaptionTemplate>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Country" FieldName="country"
						Visible="False" VisibleIndex="40" Width="75px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Location Postal"
						FieldName="location_postal" VisibleIndex="41" Width="40px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Origin" FieldName="origin" Visible="False"
						VisibleIndex="42">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="YTD Rev" FieldName="ytd_rev"
						Visible="False" VisibleIndex="43" Width="75px">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Days since 2nd WO" FieldName="days_since_2nd_wo"
						Visible="False" VisibleIndex="44" Width="75px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Days since last quote" FieldName="last_quote" Visible="False" VisibleIndex="45" Width="50px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Next Followup Date" FieldName="Next Followup Date" Visible="False" VisibleIndex="46" Width="70px">
						<PropertiesDateEdit DisplayFormatString="yyyy-MM-dd" EditFormat="Custom" EditFormatString="yyyy-MM-dd">
						</PropertiesDateEdit>
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Followup Notes" FieldName="Followup Notes" Visible="False" VisibleIndex="47" Width="100px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Days Until Followup" FieldName="Days Until Followup" Visible="False" VisibleIndex="48" Width="60px">
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataDateColumn Caption="Invoice Email Address" FieldName="Invoice Address" Visible="False" VisibleIndex="49" Width="70px">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataDateColumn Caption="Statement Email Address" FieldName="Statement Address" Visible="False" VisibleIndex="50" Width="70px">
					</dx:GridViewDataDateColumn>
					<dx:GridViewDataTextColumn Caption="Last YTD Rev" FieldName="lytd_rev"
						Visible="False" VisibleIndex="51" Width="75px">
						<PropertiesTextEdit DisplayFormatString="C2">
						</PropertiesTextEdit>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataColumn Caption="Year Minus 1 - Revenue (total)" FieldName="yearminus1_total" VisibleIndex="52" Visible="false"></dx:GridViewDataColumn>
					<dx:GridViewDataColumn Caption="Year Minus 2 - Revenue (total)" FieldName="yearminus2_total" VisibleIndex="53" Visible="false"></dx:GridViewDataColumn>
				</Columns>
				<Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowFooter="True"
					ShowGroupFooter="VisibleIfExpanded" ShowGroupPanel="True"
					ShowFilterBar="Visible" ShowHeaderFilterButton="True" ColumnMinWidth="10" />
				<SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides" />
			</dx:ASPxGridView>
			<br />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
